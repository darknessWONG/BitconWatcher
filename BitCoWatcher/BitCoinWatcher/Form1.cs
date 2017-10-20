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
            changeText(e);
        }
        public void alerting(Object sender, BitCoinStock.AlertEventArgs e)
        {
            playSound(e);
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
            bitCoinStock = new BitCoinStock(xmlFile);
            bitCoinStock.refreashed += this.labelRefreash;
            bitCoinStock.alert += this.alerting;

            frequencyTextBox.Text = bitCoinStock.GetRequestFrequency().ToString();
            sellThresholdTextBox.Text = bitCoinStock.SellThreshold.ToString();
            buyThresholTextBox.Text = bitCoinStock.BuyThreshold.ToString();
            upThresholdTextBox.Text = bitCoinStock.Ticker.UpThreshold.ToString();
            downThresholTextBox.Text = bitCoinStock.Ticker.DownThreshold.ToString();
            alertUpperTextbox.Text = bitCoinStock.AlertUpper.ToString();
            alertLowerTextbox.Text = bitCoinStock.AlertLower.ToString();

            thread = new Thread(bitCoinStock.MainLoop);
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
        private void playSound(BitCoinStock.AlertEventArgs e)
        {
            if(this.InvokeRequired)
            {
                playSoundEventHandler d = playSound;
                this.Invoke(d, e);
            }
            else
            {
                alertPlayer.settings.volume = 50;
                alertPlayer.Ctlcontrols.play();
                string alertStr;
                if(e.currentValue >= e.alertUpper)
                {
                    alertStr = "超过上限";
                }
                else
                {
                    alertStr = "低于下限";
                }
                MessageBox.Show(alertStr);
                alertPlayer.Ctlcontrols.stop();
                alertCheckBox.CheckState = CheckState.Unchecked;
            }
        }
        private void setBitcoinStockMember()
        {

            bitCoinStock.SetMemberValue(Convert.ToDouble(buyThresholTextBox.Text), Convert.ToDouble(sellThresholdTextBox.Text), 
                Convert.ToDouble(frequencyTextBox.Text));
            bitCoinStock.Ticker.SetMemberValue(Convert.ToDouble(upThresholdTextBox.Text), Convert.ToDouble(downThresholTextBox.Text));
            
            bitCoinStock.SaveXml(xmlFile);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            setBitcoinStockMember();
        }
        private void alertCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (alertCheckBox.CheckState == CheckState.Checked)
            {
                double alertUpper = alertUpperTextbox.Text == "" ? 0 : Convert.ToDouble(alertUpperTextbox.Text);
                double alertLower = alertLowerTextbox.Text == "" ? 0 : Convert.ToDouble(alertLowerTextbox.Text);
                bitCoinStock.AlertUpper = alertUpper;
                bitCoinStock.AlertLower = alertLower;
                bitCoinStock.IsAlert = true;
            }
            else if (alertCheckBox.CheckState == CheckState.Unchecked)
            {
                bitCoinStock.IsAlert = false;
            }
        }

        private BitCoinStock bitCoinStock;
        private Thread thread;
        private string xmlFile;
        private Dictionary<ExchangeStatus, string> exchangStatus;
        private Dictionary<StockStatus, string> stockStatus;
        delegate void changeTextEventHandler(BitCoinStock.RefreashedEventArgs e);
        delegate void playSoundEventHandler(BitCoinStock.AlertEventArgs e);
    }
}
