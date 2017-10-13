using System;
using System.Threading;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace BitCoinInterface
{
    public class BitCoinStock
    {
        #region private value
        private Ticker ticker;
        #endregion

        #region attribute
        public double UpThreshold { get; set; }                 //跌转升判断阈值
        public double DownThreshold { get; set; }               //升转跌判断阈值
        public double BuyThreshold { get; set; }                //购买阈值
        public double SellThreshold { get; set; }               //售卖阈值
        public string XmlFile { get; set; }
        public StockStatus Status { get; private set; }         //目前的升跌状态
        public double SleepTime { get; private set; }           //睡眠时间，用于控制查询频率
        public double PeakValue { get; private set; }           //目前峰值
        public double TroughVaule { get; private set; }         //目前谷值
        public double CurrentValue { get; private set; }        //当前价格
        public double LastValue { get; private set; }           //上次询问的价格
        public ExchangeStatus Decision { get; private set; }

        public double CurrentBitcoinNum { get; private set; }   //现在持有的比特币数量
        public double CurrentCashNum { get; private set; }      //现在持有的现金数量
        public double LastBuyValue { get; private set; }        //上一次购买时的价格
        public double LastSellVaule { get; private set; }       //上一次出售时的价格
        public ExchangeStatus NextStep { get; private set; }    //正在等待的交易动作 买或卖

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
            setMemberValue();
            XmlFile = null;
            ticker = new Ticker();
            ticker.start();
        }
        public BitCoinStock(double upThreshold, double downThreshold, double buyThreshold, double sellThreshold, double requestFrequency)
        {
            setMemberValue(upThreshold, downThreshold, buyThreshold, sellThreshold, requestFrequency);
            XmlFile = null;
            ticker = new Ticker();
            ticker.start();
        }
        public BitCoinStock(string xmlFile)
        {
            XmlFile = xmlFile;
            Dictionary<string, string> datas = loadConfigXml(XmlFile);
            setMemberValue(datas);
            ticker = new Ticker();
            ticker.start();
        }
        ~BitCoinStock()
        {
            if(XmlFile != null)
            {
                saveXml(XmlFile);
            }
        }
        #endregion

        public ExchangeStatus refreash()
        {
            getCurrentValue();
            Boolean statusChange = trendContain();
            if(statusChange)
            {
                if(Status == StockStatus.up && (CurrentValue < LastSellVaule * BuyThreshold))
                {
                    return ExchangeStatus.buy;
                }
                else if(Status == StockStatus.down && (CurrentValue > LastBuyValue * SellThreshold))
                {
                    return ExchangeStatus.sell;
                }
            }
            return ExchangeStatus.doNothing;
        }
        public void mainLoop()
        {
            while(true)
            {
                Decision = refreash();
                checkTrad(Decision);
                RefreashedEventArgs e = new RefreashedEventArgs(CurrentValue, LastValue, Status, Decision);
                onRefreash(e);
                Thread.Sleep((int)SleepTime);
            }
        }
        public void setSleepTime(double requestFrequency)
        {
            SleepTime = calSleepTime(requestFrequency);
        }
        public double getRequestFrequency()
        {
            return 6000 / SleepTime;
        }
        public void setMemberValue(double upthreshold = 0, double downThreshold = 0, double buyThreshold = 0, double sellThreshold = 0, double requestFrequency = 10, 
            StockStatus status = StockStatus.noStatus, double peakVaule = 0, double trougValue = 0, double currentValue = 0, double lastValue =0, 
            ExchangeStatus decision = ExchangeStatus.doNothing)
        {
            PeakValue = peakVaule;
            TroughVaule = trougValue;
            CurrentValue = currentValue;
            LastValue = lastValue;
            UpThreshold = upthreshold;
            DownThreshold = downThreshold;
            BuyThreshold = buyThreshold;
            SellThreshold = sellThreshold;
            setSleepTime(requestFrequency);
            Status = status;
            Decision = decision;

            CurrentCashNum = 1000000;
            CurrentBitcoinNum = 0;
            LastBuyValue = 0;
            LastSellVaule = 0;
            NextStep = ExchangeStatus.buy;
        }
        public void setMemberValue(Dictionary<string, string> datas)
        {
            UpThreshold = datas.ContainsKey("UpThreshold") ? Convert.ToDouble(datas["UpThreshold"]) : 0;
            DownThreshold = datas.ContainsKey("DownThreshold") ? Convert.ToDouble(datas["DownThreshold"]) : 0;
            BuyThreshold = datas.ContainsKey("BuyThreshold") ? Convert.ToDouble(datas["BuyThreshold"]) : 0;
            SellThreshold = datas.ContainsKey("SellThreshold") ? Convert.ToDouble(datas["SellThreshold"]) : 0;
            double requestFrequency = datas.ContainsKey("RequestFrequency") ? Convert.ToDouble(datas["RequestFrequency"]) : 2;
            setSleepTime(requestFrequency);
            Status = datas.ContainsKey("Status") ? (StockStatus)Convert.ToInt32(datas["Status"]) : StockStatus.noStatus;
            PeakValue = datas.ContainsKey("PeakValue") ? Convert.ToDouble(datas["PeakValue"]) : 0;
            TroughVaule = datas.ContainsKey("TroughVaule") ? Convert.ToDouble(datas["TroughVaule"]) : 0;
            CurrentValue = datas.ContainsKey("CurrentValue") ? Convert.ToDouble(datas["CurrentValue"]) : 0;
            LastValue = datas.ContainsKey("LastValue") ? Convert.ToDouble(datas["LastValue"]) : 0;

            Decision = ExchangeStatus.doNothing;


            CurrentCashNum = 1000000;
            CurrentBitcoinNum = 0;
            LastBuyValue = 0;
            LastSellVaule = 0;
            NextStep = ExchangeStatus.buy;
        }
        public void saveXml(string xmlFile)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xEle = xDoc.CreateElement("config");
            XmlElement xChild = xDoc.CreateElement("UpThreshold");
            xChild.InnerText = UpThreshold.ToString();
            xEle.AppendChild(xChild);
            xChild = xDoc.CreateElement("DownThreshold");
            xChild.InnerText = DownThreshold.ToString();
            xEle.AppendChild(xChild);
            xChild = xDoc.CreateElement("BuyThreshold");
            xChild.InnerText = BuyThreshold.ToString();
            xEle.AppendChild(xChild);
            xChild = xDoc.CreateElement("SellThreshold");
            xChild.InnerText = SellThreshold.ToString();
            xEle.AppendChild(xChild);
            xChild = xDoc.CreateElement("SleepTime");
            xChild.InnerText = SleepTime.ToString();
            xEle.AppendChild(xChild);
            xDoc.AppendChild(xEle);
            xDoc.Save(xmlFile);

        }

        private void getCurrentValue()
        {
            ticker.start();
            LastValue = CurrentValue;
            CurrentValue = ticker.getLtpVal();
        }
        /*走势监控
         * 根据升跌状态来调整最高价和最低价
         * 同时判断升跌装填
         * */
        private Boolean trendContain()
        {
            StockStatus lastStatus = Status;
            if(CurrentValue >= LastValue)
            {
                if(Status == StockStatus.noStatus)
                {
                    Status = StockStatus.up;
                }
                else if(Status == StockStatus.down && (CurrentValue - TroughVaule) / TroughVaule > UpThreshold)
                {
                    Status = StockStatus.up;
                    PeakValue = CurrentValue;
                }
                else if(Status == StockStatus.up && CurrentValue > PeakValue)
                {
                    PeakValue = CurrentValue;
                }
            }
            else
            {
                if (Status == StockStatus.noStatus)
                {
                    Status = StockStatus.down;
                }
                else if (Status == StockStatus.up && (PeakValue - CurrentValue) / PeakValue > DownThreshold)
                {
                    Status = StockStatus.down;
                    TroughVaule = CurrentValue;
                }
                else if (Status == StockStatus.down && CurrentValue < TroughVaule)
                {
                    TroughVaule = CurrentValue;
                }
            }

            if (lastStatus != StockStatus.noStatus && Status != lastStatus)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private double calSleepTime(double requestFrequency)
        {
            return 6000 / requestFrequency;
        }
        private Dictionary<string, string> loadConfigXml(string xmlFile)
        {
            Dictionary<string, string> xmlData = new Dictionary<string, string>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlFile);
            XmlNode xNode = xDoc.SelectSingleNode("config");
            foreach(XmlNode node in xNode.ChildNodes)
            {
                xmlData[node.Name] = node.InnerText;
            }
            return xmlData;
        }

        private void checkTrad(ExchangeStatus exStatus)
        {
            if(exStatus == NextStep)
            {
                switch (exStatus)
                {
                    case ExchangeStatus.buy:
                        buy();
                        NextStep = ExchangeStatus.sell;
                        break;
                    case ExchangeStatus.sell:
                        sell();
                        NextStep = ExchangeStatus.buy;
                        break;
                }
            }
        }
        private void writeLog(double moveNum, double currentCash, double currentBitcoin, ExchangeStatus action, double serFee)
        {
            using (StreamWriter logStream = new StreamWriter("bitcoin.log", true))
            {
                logStream.WriteLine("%s\t%s\t%s\t%s\t%s\t%s", DateTime.UtcNow.ToString(), moveNum.ToString(), currentCash.ToString(), currentBitcoin.ToString(), action.ToString(), serFee.ToString());
            }

        }
        private void buy()
        {
            double canBuy = (double)Decimal.Round(Decimal.Parse((CurrentCashNum / CurrentValue).ToString()), 3);
            double move = (double)Decimal.Round(Decimal.Parse((canBuy * 0.9991).ToString()), 3);
            CurrentBitcoinNum += move;
            CurrentCashNum -= CurrentValue * canBuy;
            writeLog(move, CurrentCashNum, CurrentBitcoinNum, ExchangeStatus.buy, (canBuy - move));
        }
        private void sell()
        {
            double canSell = (double)Decimal.Round(Decimal.Parse((CurrentBitcoinNum).ToString()), 3);
            double move = (double)Decimal.Round(Decimal.Parse((CurrentBitcoinNum * 0.9991).ToString()), 3);
            CurrentBitcoinNum -= canSell;
            CurrentCashNum += move * CurrentValue;
            writeLog(move, CurrentCashNum, CurrentCashNum, ExchangeStatus.sell, (canSell - move));
        }
    }



    /* 价格状态
     * 用以表示价格升跌状态的枚举类型。
     * noStatus只有在初始化是才会使用。
     **/
    public enum StockStatus
    {
        noStatus,
        up,
        down
    }
    public enum ExchangeStatus
    {
        doNothing,
        buy,
        sell
    }
}
