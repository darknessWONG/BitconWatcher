using System;
using System.Collections;

namespace BitCoinInterface
{
    public class BitCoinTicker
    {

        private TickerApi ticker;
        private ErrorCounter errorCounter;

        public double UpThreshold { get; set; }                 //跌转升判断阈值
        public double DownThreshold { get; set; }               //升转跌判断阈值
        public StockStatus Status { get; private set; }         //目前的升跌状态
        public double PeakValue { get; private set; }           //目前峰值
        public double TroughVaule { get; private set; }         //目前谷值
        public double CurrentValue { get; private set; }        //当前价格
        public double LastValue { get; private set; }           //上次询问的价格

        public BitCoinTicker()
        {
            SetMemberValue();
            ticker = new TickerApi();
            errorCounter = new ErrorCounter(10, 4);
        }
        public BitCoinTicker(double upThreshold, double downThreshold)
        {
            SetMemberValue(upThreshold, downThreshold);
            ticker = new TickerApi();
            errorCounter = new ErrorCounter(10, 4);
        }
        public BitCoinTicker(Hashtable datas)
        {
            SetMemberValue(datas);
            ticker = new TickerApi();
            errorCounter = new ErrorCounter(10, 4);
        }



        public void SetMemberValue(double upthreshold = 0, double downThreshold = 0, StockStatus status = StockStatus.noStatus,
            double peakVaule = 0, double trougValue = 0, double currentValue = 0, double lastValue = 0)
        {
            PeakValue = peakVaule;
            TroughVaule = trougValue;
            CurrentValue = currentValue;
            LastValue = lastValue;
            UpThreshold = upthreshold;
            DownThreshold = downThreshold;
            Status = status;

        }
        public void SetMemberValue(Hashtable datas)
        {
            UpThreshold = datas.ContainsKey("UpThreshold") ? Convert.ToDouble(datas["UpThreshold"]) : 0;
            DownThreshold = datas.ContainsKey("DownThreshold") ? Convert.ToDouble(datas["DownThreshold"]) : 0;
            Status = datas.ContainsKey("Status") ? (StockStatus)Convert.ToInt32(datas["Status"]) : StockStatus.noStatus;
            PeakValue = datas.ContainsKey("PeakValue") ? Convert.ToDouble(datas["PeakValue"]) : 0;
            TroughVaule = datas.ContainsKey("TroughVaule") ? Convert.ToDouble(datas["TroughVaule"]) : 0;
            CurrentValue = datas.ContainsKey("CurrentValue") ? Convert.ToDouble(datas["CurrentValue"]) : 0;
            LastValue = datas.ContainsKey("LastValue") ? Convert.ToDouble(datas["LastValue"]) : 0;
        }

        public Boolean GetCurrentValue()
        {
            try
            {
                ticker.start();
                LastValue = CurrentValue;
                CurrentValue = ticker.getLtpVal();
                errorCounter.AddRecord(true);
                return trendContain();
            }
            catch (Exception)
            {
                if (!errorCounter.AddRecord(false))
                {
                    throw;
                }else
                {
                    return false;
                }
            }
        }

        public Hashtable ToHashtable()
        {
            Hashtable returnValue = new Hashtable();
            returnValue["UpThreshold"] = UpThreshold.ToString();
            returnValue["DownThreshold"] = DownThreshold.ToString();

            return returnValue;
        }

        /*走势监控
        * 根据升跌状态来调整最高价和最低价
        * 同时判断升跌装填
        * */
        private Boolean trendContain()
        {
            StockStatus lastStatus = Status;
            if (CurrentValue >= LastValue)
            {
                if (Status == StockStatus.noStatus)
                {
                    Status = StockStatus.up;
                }
                else if (Status == StockStatus.down && (CurrentValue - TroughVaule) / TroughVaule > UpThreshold)
                {
                    Status = StockStatus.up;
                    PeakValue = CurrentValue;
                }
                else if (Status == StockStatus.up && CurrentValue > PeakValue)
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

    }
}
