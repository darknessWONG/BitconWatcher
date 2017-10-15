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
        public string XmlFile          { get; set; }
        public BitCoinTicker Ticker    { get; set; }
        public BitCoinTrader Trader    { get; private set; }
        public double SleepTime        { get; private set; }           //睡眠时间，用于控制查询频率
        public ExchangeStatus Decision { get; private set; }
        #endregion

        #region event
        public delegate void refreashedEventHandler(Object sender, RefreashedEventArgs e);
        public event refreashedEventHandler refreashed;

        public class RefreashedEventArgs : EventArgs
        {
            public readonly double currentValue;
            public readonly double lastValue;
            public readonly StockStatus status;
            public readonly ExchangeStatus decision;

            public RefreashedEventArgs(double currentValue, double lastValue, StockStatus status, ExchangeStatus decision)
            {
                this.currentValue = currentValue;
                this.lastValue = lastValue;
                this.status = status;
                this.decision = decision;
            }
        }
        public void onRefreash(RefreashedEventArgs e)
        {
            refreashed?.Invoke(this, e);
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
            Trader = new BitCoinTrader();
            Ticker = new BitCoinTicker((Hashtable)datas["Ticker"]);
        }
        ~BitCoinStock()
        {
            SaveXml();
        }
        #endregion

        public void SetMemberValue(double buyThreshold = 0, double sellThreshold = 0, double requestFrequency = 10, ExchangeStatus decision = ExchangeStatus.doNothing)
        {
            BuyThreshold = buyThreshold;
            SellThreshold = sellThreshold;
            SetSleepTime(requestFrequency);
            Decision = decision;
        }
        public void SetMemberValue(Hashtable datas)
        {
            BuyThreshold = datas.ContainsKey("BuyThreshold") ? Convert.ToDouble(datas["BuyThreshold"]) : 0;
            SellThreshold = datas.ContainsKey("SellThreshold") ? Convert.ToDouble(datas["SellThreshold"]) : 0;
            double requestFrequency = datas.ContainsKey("RequestFrequency") ? Convert.ToDouble(datas["RequestFrequency"]) : 2;
            SetSleepTime(requestFrequency);

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
                RefreashedEventArgs e = new RefreashedEventArgs(Ticker.CurrentValue, Ticker.LastValue, Ticker.Status, Decision);
                onRefreash(e);
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