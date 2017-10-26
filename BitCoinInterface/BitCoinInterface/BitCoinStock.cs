using System;
using System.Collections;
using System.Threading;

namespace BitCoinInterface
{
    public class BitCoinStock
    {
        #region private value
        private bool firstBuyFlag = true;
        #endregion

        #region attribute
        public double BuyThreshold     { get; set; }                //购买阈值
        public double SellThreshold    { get; set; }               //售卖阈值
        public double AlertUpper       { get; set; }               //报警区间上限
        public double AlertLower       { get; set; }               //报警区间下限
        public Boolean IsAlert         { get; set; }
        public string XmlFile          { get; set; }               //配置xml地址
        public BitCoinTicker Ticker    { get; set; }
        public BitCoinTrader Trader    { get; private set; }
        public double SleepTime        { get; private set; }           //睡眠时间，用于控制查询频率
        public ExchangeStatus Decision { get; private set; }
        #endregion

        #region event
        public delegate void refreashedEventHandler(Object sender, RefreashedEventArgs e);
        public event refreashedEventHandler refreashed;

        public delegate void alertEventHabdler(Object sender, AlertEventArgs e);
        public event alertEventHabdler alert;

        public class RefreashedEventArgs : EventArgs
        {
            public readonly double currentValue;
            public readonly double lastValue;
            public readonly StockStatus status;
            public readonly ExchangeStatus decision;
            public readonly double currentBTC;
            public readonly double currentJPY;

            public RefreashedEventArgs(double currentValue, double lastValue, StockStatus status, ExchangeStatus decision,
                double currentBTC, double currentJPY)
            {
                this.currentValue = currentValue;
                this.lastValue = lastValue;
                this.status = status;
                this.decision = decision;
                this.currentBTC = currentBTC;
                this.currentJPY = currentJPY;
            }
        }
        public void onRefreash(RefreashedEventArgs e)
        {
            refreashed?.Invoke(this, e);
        }

        public class AlertEventArgs : EventArgs
        {
            public readonly double currentValue;
            public readonly double alertUpper;
            public readonly double alertLower;


            public AlertEventArgs(double currentValue, double alertUpper, double alertLower)
            {
                this.currentValue = currentValue;
                this.alertUpper   = alertUpper;
                this.alertLower   = alertLower;
            }
        }
        public void alerted(AlertEventArgs e)
        {
            alert?.Invoke(this, e);
        }
        #endregion

        #region constructor
        public BitCoinStock()
        {
            SetMemberValue();
            XmlFile = null;
            Trader = new BitCoinTrader();
            Ticker = new BitCoinTicker();
        }
        public BitCoinStock(double upThreshold, double downThreshold, double buyThreshold, double sellThreshold, double requestFrequency)
        {

            SetMemberValue(buyThreshold, sellThreshold, requestFrequency);
            XmlFile = null;
            Trader = new BitCoinTrader();
            Ticker = new BitCoinTicker(upThreshold, downThreshold);
        }
        public BitCoinStock(BitCoinTicker ticker, double buyThreshold, double sellThreshold, double requestFrequency)
        {

            SetMemberValue(buyThreshold, sellThreshold, requestFrequency);
            XmlFile = null;
            Trader = new BitCoinTrader();
            Ticker = ticker;
        }
        public BitCoinStock(string xmlFile)
        {
            XmlFile = xmlFile;
            Hashtable datas = XmlConfigHandler._LoadConfigXml(XmlFile);
            SetMemberValue((Hashtable)datas);
            Trader = new BitCoinTrader((Hashtable)datas["Trader"]);
            Ticker = new BitCoinTicker((Hashtable)datas["Ticker"]);
        }
        ~BitCoinStock()
        {
            SaveXml();
        }
        #endregion

        public void SetMemberValue(double buyThreshold = 0, double sellThreshold = 0, double requestFrequency = 10, double alertUpper = 0, double alertLower = 0)
        {
            BuyThreshold = buyThreshold;
            SellThreshold = sellThreshold;
            SetSleepTime(requestFrequency);
            AlertUpper = alertUpper;
            AlertLower = alertLower;

            IsAlert = false;
            Decision = ExchangeStatus.doNothing;
        }
        public void SetMemberValue(Hashtable datas)
        {
            BuyThreshold = datas.ContainsKey("BuyThreshold") ? Convert.ToDouble(datas["BuyThreshold"]) : 0;
            SellThreshold = datas.ContainsKey("SellThreshold") ? Convert.ToDouble(datas["SellThreshold"]) : 0;
            AlertUpper = datas.ContainsKey("AlertUpper") ? Convert.ToDouble(datas["AlertUpper"]) : 0;
            AlertLower = datas.ContainsKey("AlertLower") ? Convert.ToDouble(datas["AlertLower"]) : 0;
            double requestFrequency = datas.ContainsKey("RequestFrequency") ? Convert.ToDouble(datas["RequestFrequency"]) : 2;
            SetSleepTime(requestFrequency);

            IsAlert = false;
            Decision = ExchangeStatus.doNothing;
        }


        public ExchangeStatus Refreash()
        {
            Boolean statusChange = false;
            try
            {
                statusChange = Ticker.GetCurrentValue();
            }catch(Exception e)
            {
                //do something let the user know
            }
            if(statusChange)
            {
                if(Ticker.Status == StockStatus.up && (firstBuyFlag || (Ticker.CurrentValue < Trader.LastSellValue * BuyThreshold)))
                {
                    return ExchangeStatus.buy;
                }
                else if(Ticker.Status == StockStatus.down && (Ticker.CurrentValue > Trader.LastBuyValue * SellThreshold))
                {
                    return ExchangeStatus.sell;
                }
            }
            return ExchangeStatus.doNothing;
        }
        public void MainLoop()
        {
            while(true)
            {
                Decision = Refreash();
                if(Trader.CheckTrad(Decision, Ticker.CurrentValue) == ExchangeStatus.buy && firstBuyFlag)
                {
                    firstBuyFlag = false;
                }
                RefreashedEventArgs refreashEA = new RefreashedEventArgs(Ticker.CurrentValue, Ticker.LastValue, Ticker.Status, Decision, 
                    Trader.CurrentBitcoinNum, Trader.CurrentCashNum);
                onRefreash(refreashEA);
               if(IsAlert && ((AlertUpper != 0 && Ticker.CurrentValue >= AlertUpper) || (AlertLower != 0 && Ticker.CurrentValue <= AlertLower)))
                {
                    AlertEventArgs alertEA = new AlertEventArgs(Ticker.CurrentValue, AlertUpper, AlertLower);
                    alerted(alertEA);
                }

                Thread.Sleep((int)SleepTime);
            }
        }
        public void SetSleepTime(double requestFrequency)
        {
            SleepTime = calSleepTime(requestFrequency);
        }
        public double GetRequestFrequency()
        {
            return 60000 / SleepTime;
        }
        public void SaveXml()
        {
            if (XmlFile != null)
            {
                Hashtable datas = ToHashtable();
                XmlConfigHandler._SaveConfigXml(datas, XmlFile);
            }

        }
        public void SaveXml(string fileName)
        {
            XmlFile = fileName;
            Hashtable datas = ToHashtable();
            XmlConfigHandler._SaveConfigXml(datas, XmlFile);
        }
 
        public Hashtable ToHashtable()
        {
            Hashtable returnValue = new Hashtable();
            returnValue["BuyThreshold"] = BuyThreshold.ToString();
            returnValue["SellThreshold"] = SellThreshold.ToString();
            returnValue["RequestFrequency"] = GetRequestFrequency().ToString();
            returnValue["AlertUpper"] = AlertUpper.ToString();
            returnValue["AlertLower"] = AlertLower.ToString();
            Hashtable tickerData = Ticker.ToHashtable();
            if(tickerData != null)
            {
                returnValue["Ticker"] = tickerData;
            }
            Hashtable trakderData = Trader.ToHashtable();
            if (trakderData != null)
            {
                returnValue["Trader"] = Trader.ToHashtable();
            }
            return returnValue;
        }

 
        private double calSleepTime(double requestFrequency)
        {
            return 60000 / requestFrequency;
        }

    }






}