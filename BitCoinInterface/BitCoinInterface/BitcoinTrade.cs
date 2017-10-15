using System;
using System.Collections;
using System.IO;

namespace BitCoinInterface
{
    public class BitCoinTrader
    {
        public double CurrentBitcoinNum { get; private set; }   //现在持有的比特币数量
        public double CurrentCashNum { get; private set; }      //现在持有的现金数量
        public double LastBuyValue { get; private set; }        //上一次购买时的价格
        public double LastSellValue { get; private set; }       //上一次出售时的价格
        public ExchangeStatus NextStep { get; private set; }    //正在等待的交易动作 买或卖

        public BitCoinTrader()
        {
            CurrentBitcoinNum = getBitcoinNum();
            CurrentCashNum = getCashNum();
            LastBuyValue = getLastBuyValue();
            LastSellValue = getLastSellValue();
            NextStep = calNextStep();
        }


        public ExchangeStatus CheckTrad(ExchangeStatus exStatus, double currentValue)
        {
            ExchangeStatus returnValue = ExchangeStatus.doNothing;
            if (exStatus == NextStep)
            {
                switch (exStatus)
                {
                    case ExchangeStatus.buy:
                        buy(currentValue);
                        NextStep = ExchangeStatus.sell;
                        returnValue = ExchangeStatus.buy;
                        break;
                    case ExchangeStatus.sell:
                        sell(currentValue);
                        NextStep = ExchangeStatus.buy;
                        returnValue = ExchangeStatus.sell;
                        break;
                }
            }
            return returnValue;
        }
        public Hashtable ToHashtable()
        {
            Hashtable returnValue = new Hashtable();

            return null;
        }


        private double getBitcoinNum()
        {
            return 0;
        }
        private double getCashNum()
        {
            return 100000;
        }
        private double getLastBuyValue()
        {
            return 0;
        }
        private double getLastSellValue()
        {
            return 0;
        }
        private ExchangeStatus calNextStep()
        {
            return ExchangeStatus.buy;
        }

        private void buy(double currentValue)
        {
            double canBuy = MathTool._RetainDecimal(CurrentCashNum / currentValue, 3);
            double move = MathTool._RetainDecimal((canBuy * 0.9991), 3);
            CurrentBitcoinNum += move;
            CurrentCashNum -= currentValue * canBuy;
            LastBuyValue = currentValue;
            writeLog(move, CurrentCashNum, CurrentBitcoinNum, ExchangeStatus.buy, (canBuy - move));
        }
        private void sell(double currentValue)
        {
            double canSell = MathTool._RetainDecimal(CurrentBitcoinNum, 3);
            double move = MathTool._RetainDecimal(CurrentBitcoinNum * 0.9991, 3);
            CurrentBitcoinNum -= canSell;
            CurrentCashNum += move * currentValue;
            LastSellValue = currentValue;
            writeLog(move, CurrentCashNum, CurrentBitcoinNum, ExchangeStatus.sell, (canSell - move));
        }
        private void writeLog(double moveNum, double currentCash, double currentBitcoin, ExchangeStatus action, double serFee)
        {
            using (StreamWriter logStream = new StreamWriter("bitcoin.log", true))
            {
                logStream.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", DateTime.UtcNow.ToString(), moveNum.ToString(), currentCash.ToString(), 
                    currentBitcoin.ToString(), action.ToString(), serFee.ToString());
            }

        }

    }
}
