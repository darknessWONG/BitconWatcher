namespace BitCoinWatcher
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.sellThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buyThresholTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.upThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.downThresholTextBox = new System.Windows.Forms.TextBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.currentValueLabel = new System.Windows.Forms.Label();
            this.lastValueLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.decisionLabel = new System.Windows.Forms.Label();
            this.alertUpperTextbox = new System.Windows.Forms.TextBox();
            this.alertLowerTextbox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.alertPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.alertCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.alertPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(548, 66);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.Size = new System.Drawing.Size(100, 21);
            this.frequencyTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(548, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "查询频率";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(655, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "次/分钟";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(548, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "售卖阈值";
            // 
            // sellThresholdTextBox
            // 
            this.sellThresholdTextBox.Location = new System.Drawing.Point(548, 147);
            this.sellThresholdTextBox.Name = "sellThresholdTextBox";
            this.sellThresholdTextBox.Size = new System.Drawing.Size(100, 21);
            this.sellThresholdTextBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(546, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "购买阈值";
            // 
            // buyThresholTextBox
            // 
            this.buyThresholTextBox.Location = new System.Drawing.Point(548, 212);
            this.buyThresholTextBox.Name = "buyThresholTextBox";
            this.buyThresholTextBox.Size = new System.Drawing.Size(100, 21);
            this.buyThresholTextBox.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(546, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "升转跌判定阈值";
            // 
            // upThresholdTextBox
            // 
            this.upThresholdTextBox.Location = new System.Drawing.Point(548, 268);
            this.upThresholdTextBox.Name = "upThresholdTextBox";
            this.upThresholdTextBox.Size = new System.Drawing.Size(100, 21);
            this.upThresholdTextBox.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(546, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "跌转升判定阈值";
            // 
            // downThresholTextBox
            // 
            this.downThresholTextBox.Location = new System.Drawing.Point(548, 322);
            this.downThresholTextBox.Name = "downThresholTextBox";
            this.downThresholTextBox.Size = new System.Drawing.Size(100, 21);
            this.downThresholTextBox.TabIndex = 10;
            // 
            // resultLabel
            // 
            this.resultLabel.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.resultLabel.Location = new System.Drawing.Point(152, 349);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(46, 38);
            this.resultLabel.TabIndex = 11;
            this.resultLabel.Text = "买";
            // 
            // currentValueLabel
            // 
            this.currentValueLabel.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.currentValueLabel.Location = new System.Drawing.Point(275, 90);
            this.currentValueLabel.Name = "currentValueLabel";
            this.currentValueLabel.Size = new System.Drawing.Size(300, 54);
            this.currentValueLabel.TabIndex = 12;
            this.currentValueLabel.Text = "300000";
            // 
            // lastValueLabel
            // 
            this.lastValueLabel.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lastValueLabel.Location = new System.Drawing.Point(275, 171);
            this.lastValueLabel.Name = "lastValueLabel";
            this.lastValueLabel.Size = new System.Drawing.Size(300, 42);
            this.lastValueLabel.TabIndex = 13;
            this.lastValueLabel.Text = "300000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(12, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(217, 40);
            this.label7.TabIndex = 14;
            this.label7.Text = "当前价格：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(12, 169);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(257, 40);
            this.label8.TabIndex = 15;
            this.label8.Text = "上次查询价格";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(550, 401);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 70);
            this.button1.TabIndex = 16;
            this.button1.Text = "设置变更";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // decisionLabel
            // 
            this.decisionLabel.Font = new System.Drawing.Font("宋体", 30F);
            this.decisionLabel.Location = new System.Drawing.Point(273, 349);
            this.decisionLabel.Name = "decisionLabel";
            this.decisionLabel.Size = new System.Drawing.Size(100, 38);
            this.decisionLabel.TabIndex = 17;
            this.decisionLabel.Text = "label9";
            // 
            // alertUpperTextbox
            // 
            this.alertUpperTextbox.Location = new System.Drawing.Point(735, 66);
            this.alertUpperTextbox.Name = "alertUpperTextbox";
            this.alertUpperTextbox.Size = new System.Drawing.Size(100, 21);
            this.alertUpperTextbox.TabIndex = 18;
            // 
            // alertLowerTextbox
            // 
            this.alertLowerTextbox.Location = new System.Drawing.Point(737, 147);
            this.alertLowerTextbox.Name = "alertLowerTextbox";
            this.alertLowerTextbox.Size = new System.Drawing.Size(100, 21);
            this.alertLowerTextbox.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(735, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "报警区间上限";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(735, 132);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 21;
            this.label10.Text = "报警区间下限";
            // 
            // alertPlayer
            // 
            this.alertPlayer.Enabled = true;
            this.alertPlayer.Location = new System.Drawing.Point(735, 186);
            this.alertPlayer.Name = "alertPlayer";
            this.alertPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("alertPlayer.OcxState")));
            this.alertPlayer.Size = new System.Drawing.Size(75, 23);
            this.alertPlayer.TabIndex = 22;
            this.alertPlayer.Visible = false;
            // 
            // alertCheckBox
            // 
            this.alertCheckBox.AutoSize = true;
            this.alertCheckBox.Location = new System.Drawing.Point(735, 216);
            this.alertCheckBox.Name = "alertCheckBox";
            this.alertCheckBox.Size = new System.Drawing.Size(72, 16);
            this.alertCheckBox.TabIndex = 23;
            this.alertCheckBox.Text = "使用报警";
            this.alertCheckBox.UseVisualStyleBackColor = true;
            this.alertCheckBox.CheckStateChanged += new System.EventHandler(this.alertCheckBox_CheckStateChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 594);
            this.Controls.Add(this.alertCheckBox);
            this.Controls.Add(this.alertPlayer);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.alertLowerTextbox);
            this.Controls.Add(this.alertUpperTextbox);
            this.Controls.Add(this.decisionLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buyThresholTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lastValueLabel);
            this.Controls.Add(this.currentValueLabel);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.downThresholTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.upThresholdTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sellThresholdTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.frequencyTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alertPlayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox frequencyTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sellThresholdTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox buyThresholTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox upThresholdTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox downThresholTextBox;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Label currentValueLabel;
        private System.Windows.Forms.Label lastValueLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label decisionLabel;
        private System.Windows.Forms.TextBox alertUpperTextbox;
        private System.Windows.Forms.TextBox alertLowerTextbox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private AxWMPLib.AxWindowsMediaPlayer alertPlayer;
        private System.Windows.Forms.CheckBox alertCheckBox;
    }
}

