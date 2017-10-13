using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using BitCoinInterface;

namespace BitCoinWatcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void labelRefreash(Object sender,BitCoinStock.RefreashedEventArgs e)
        {
            BitCoinStock tmpBitcoinStock = (BitCoinStock)sender;
            changeText(e);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            xmlFile = "config.xml";
            exchangStatus = new Dictionary<ExchangeStatus, string>();
            exchangStatus[ExchangeStatus.buy] = "买";
            exchangStatus[ExchangeStatus.sell] = "卖";
            exchangStatus[ExchangeStatus.doNothing] = "冷静";
            stockStatus = new Dictionary<StockStatus, string>();
            stockStatus[StockStatus.down] = "降";
            stockStatus[StockStatus.up] = "升";
            stockStatus[StockStatus.noStatus] = "暂无状态";
            //bitCoinStock = new BitCoinStock(upThreshold, downThreshold, buyThreshold, sellThreshold, requestFrequency);
            bitCoinStock = new BitCoinStock(xmlFile);
            bitCoinStock.refreashed += this.labelRefreash;

            frequencyTextBox.Text = bitCoinStock.getRequestFrequency().ToString();
            sellThresholdTextBox.Text = bitCoinStock.SellThreshold.ToString();
            buyThresholTextBox.Text = bitCoinStock.BuyThreshold.ToString();
            upThresholdTextBox.Text = bitCoinStock.UpThreshold.ToString();
            downThresholTextBox.Text = bitCoinStock.DownThreshold.ToString();

            thread = new Thread(bitCoinStock.mainLoop);
            thread.Name = "mainloop";
            thread.Start();
        }
        private void changeText(BitCoinStock.RefreashedEventArgs e)
        {
            if(this.InvokeRequired)
            {
                changeTextEventHandler d = changeText;
                this.Invoke(d, e);
            }
            else
            {
                currentValueLabel.Text = e.currentValue.ToString();
                lastValueLabel.Text = e.lastValue.ToString();
                resultLabel.Text = stockStatus[e.status];
                decisionLabel.Text = exchangStatus[e.decision];
                this.Refresh();
            }
        }
        private void setBitcoinStockMember()
        {
            bitCoinStock.setMemberValue(Convert.ToDouble(upThresholdTextBox.Text), Convert.ToDouble(downThresholTextBox.Text),
                Convert.ToDouble(buyThresholTextBox.Text), Convert.ToDouble(sellThresholdTextBox.Text), Convert.ToDouble(frequencyTextBox.Text));
            bitCoinStock.saveXml(xmlFile);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setBitcoinStockMember();
        }

        private BitCoinStock bitCoinStock;
        private Thread thread;
        private string xmlFile;
        private Dictionary<ExchangeStatus, string> exchangStatus;
        private Dictionary<StockStatus, string> stockStatus;
        delegate void changeTextEventHandler(BitCoinStock.RefreashedEventArgs e);
    }
}
