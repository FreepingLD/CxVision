namespace FunctionBlock
{
    partial class StilPointLaserParamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.测量模式comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_OpticalPen = new System.Windows.Forms.ComboBox();
            this.cmb_RateList = new System.Windows.Forms.ComboBox();
            this.tb_MinRate = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.峰值选择comboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_Threshoud = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rb_manualMode = new System.Windows.Forms.RadioButton();
            this.rb_Auto = new System.Windows.Forms.RadioButton();
            this.trackbarManual = new System.Windows.Forms.TrackBar();
            this.trackbarAuto = new System.Windows.Forms.TrackBar();
            this.AutoLightlabel = new System.Windows.Forms.Label();
            this.ManualValuelabel = new System.Windows.Forms.Label();
            this.Savebutton = new System.Windows.Forms.Button();
            this.Darkbutton = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.触发电平comboBox = new System.Windows.Forms.ComboBox();
            this.取消等待采集checkBox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.触发源comboBox = new System.Windows.Forms.ComboBox();
            this.采集点数textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.触发模式comboBox = new System.Windows.Forms.ComboBox();
            this.监控panel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SDPtextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SPPtextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarManual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarAuto)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.测量模式comboBox);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.cmb_OpticalPen);
            this.groupBox7.Controls.Add(this.cmb_RateList);
            this.groupBox7.Controls.Add(this.tb_MinRate);
            this.groupBox7.Location = new System.Drawing.Point(5, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(291, 68);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "一般参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(136, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "测量模式";
            // 
            // 测量模式comboBox
            // 
            this.测量模式comboBox.FormattingEnabled = true;
            this.测量模式comboBox.Location = new System.Drawing.Point(207, 42);
            this.测量模式comboBox.Name = "测量模式comboBox";
            this.测量模式comboBox.Size = new System.Drawing.Size(79, 20);
            this.测量模式comboBox.TabIndex = 20;
            this.测量模式comboBox.SelectionChangeCommitted += new System.EventHandler(this.测量模式comboBox_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "频率设置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "最低频率";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "光学笔列表";
            // 
            // cmb_OpticalPen
            // 
            this.cmb_OpticalPen.FormattingEnabled = true;
            this.cmb_OpticalPen.Location = new System.Drawing.Point(207, 18);
            this.cmb_OpticalPen.Name = "cmb_OpticalPen";
            this.cmb_OpticalPen.Size = new System.Drawing.Size(79, 20);
            this.cmb_OpticalPen.TabIndex = 6;
            this.cmb_OpticalPen.SelectionChangeCommitted += new System.EventHandler(this.cmb_OpticalPen_SelectionChangeCommitted);
            // 
            // cmb_RateList
            // 
            this.cmb_RateList.FormattingEnabled = true;
            this.cmb_RateList.Location = new System.Drawing.Point(61, 16);
            this.cmb_RateList.Name = "cmb_RateList";
            this.cmb_RateList.Size = new System.Drawing.Size(69, 20);
            this.cmb_RateList.TabIndex = 8;
            this.cmb_RateList.SelectionChangeCommitted += new System.EventHandler(this.cmb_RateList_SelectionChangeCommitted);
            // 
            // tb_MinRate
            // 
            this.tb_MinRate.Location = new System.Drawing.Point(61, 41);
            this.tb_MinRate.Name = "tb_MinRate";
            this.tb_MinRate.Size = new System.Drawing.Size(69, 21);
            this.tb_MinRate.TabIndex = 16;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label8);
            this.groupBox8.Controls.Add(this.峰值选择comboBox);
            this.groupBox8.Location = new System.Drawing.Point(5, 73);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(130, 44);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "峰值控制";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "峰值";
            // 
            // 峰值选择comboBox
            // 
            this.峰值选择comboBox.FormattingEnabled = true;
            this.峰值选择comboBox.Location = new System.Drawing.Point(61, 18);
            this.峰值选择comboBox.Name = "峰值选择comboBox";
            this.峰值选择comboBox.Size = new System.Drawing.Size(69, 20);
            this.峰值选择comboBox.TabIndex = 21;
            this.峰值选择comboBox.SelectionChangeCommitted += new System.EventHandler(this.峰值选择comboBox_SelectionChangeCommitted);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "阈值(0-1）";
            // 
            // tb_Threshoud
            // 
            this.tb_Threshoud.Location = new System.Drawing.Point(83, 14);
            this.tb_Threshoud.Name = "tb_Threshoud";
            this.tb_Threshoud.Size = new System.Drawing.Size(67, 21);
            this.tb_Threshoud.TabIndex = 20;
            this.tb_Threshoud.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tb_Threshoud_KeyUp);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rb_manualMode);
            this.groupBox3.Controls.Add(this.rb_Auto);
            this.groupBox3.Controls.Add(this.trackbarManual);
            this.groupBox3.Controls.Add(this.trackbarAuto);
            this.groupBox3.Controls.Add(this.AutoLightlabel);
            this.groupBox3.Controls.Add(this.ManualValuelabel);
            this.groupBox3.Location = new System.Drawing.Point(299, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 113);
            this.groupBox3.TabIndex = 52;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "光源控制";
            // 
            // rb_manualMode
            // 
            this.rb_manualMode.AutoSize = true;
            this.rb_manualMode.Location = new System.Drawing.Point(9, 26);
            this.rb_manualMode.Name = "rb_manualMode";
            this.rb_manualMode.Size = new System.Drawing.Size(47, 16);
            this.rb_manualMode.TabIndex = 11;
            this.rb_manualMode.TabStop = true;
            this.rb_manualMode.Text = "手动";
            this.rb_manualMode.UseVisualStyleBackColor = true;
            this.rb_manualMode.Click += new System.EventHandler(this.rb_manualMode_Click);
            // 
            // rb_Auto
            // 
            this.rb_Auto.AutoSize = true;
            this.rb_Auto.Location = new System.Drawing.Point(9, 68);
            this.rb_Auto.Name = "rb_Auto";
            this.rb_Auto.Size = new System.Drawing.Size(47, 16);
            this.rb_Auto.TabIndex = 10;
            this.rb_Auto.TabStop = true;
            this.rb_Auto.Text = "自动";
            this.rb_Auto.UseVisualStyleBackColor = true;
            this.rb_Auto.Click += new System.EventHandler(this.rb_Auto_Click);
            // 
            // trackbarManual
            // 
            this.trackbarManual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackbarManual.Location = new System.Drawing.Point(51, 21);
            this.trackbarManual.Maximum = 100;
            this.trackbarManual.Name = "trackbarManual";
            this.trackbarManual.Size = new System.Drawing.Size(184, 45);
            this.trackbarManual.TabIndex = 14;
            this.trackbarManual.TickFrequency = 10;
            this.trackbarManual.Value = 10;
            this.trackbarManual.Scroll += new System.EventHandler(this.trackbarManual_Scroll);
            // 
            // trackbarAuto
            // 
            this.trackbarAuto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackbarAuto.Location = new System.Drawing.Point(51, 65);
            this.trackbarAuto.Maximum = 4095;
            this.trackbarAuto.Name = "trackbarAuto";
            this.trackbarAuto.Size = new System.Drawing.Size(184, 45);
            this.trackbarAuto.TabIndex = 20;
            this.trackbarAuto.TickFrequency = 410;
            this.trackbarAuto.Scroll += new System.EventHandler(this.trackbarAuto_Scroll);
            // 
            // AutoLightlabel
            // 
            this.AutoLightlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoLightlabel.AutoSize = true;
            this.AutoLightlabel.Location = new System.Drawing.Point(233, 70);
            this.AutoLightlabel.Name = "AutoLightlabel";
            this.AutoLightlabel.Size = new System.Drawing.Size(23, 12);
            this.AutoLightlabel.TabIndex = 22;
            this.AutoLightlabel.Text = "Max";
            // 
            // ManualValuelabel
            // 
            this.ManualValuelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ManualValuelabel.AutoSize = true;
            this.ManualValuelabel.Location = new System.Drawing.Point(235, 23);
            this.ManualValuelabel.Name = "ManualValuelabel";
            this.ManualValuelabel.Size = new System.Drawing.Size(11, 12);
            this.ManualValuelabel.TabIndex = 21;
            this.ManualValuelabel.Text = "%";
            // 
            // Savebutton
            // 
            this.Savebutton.Location = new System.Drawing.Point(189, 18);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(63, 25);
            this.Savebutton.TabIndex = 49;
            this.Savebutton.Text = "参数保存";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // Darkbutton
            // 
            this.Darkbutton.Location = new System.Drawing.Point(189, 49);
            this.Darkbutton.Name = "Darkbutton";
            this.Darkbutton.Size = new System.Drawing.Size(63, 30);
            this.Darkbutton.TabIndex = 59;
            this.Darkbutton.Text = "暗黑校正";
            this.Darkbutton.UseVisualStyleBackColor = true;
            this.Darkbutton.Click += new System.EventHandler(this.Darkbutton_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.触发电平comboBox);
            this.groupBox6.Controls.Add(this.取消等待采集checkBox);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.触发源comboBox);
            this.groupBox6.Controls.Add(this.采集点数textBox);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.Savebutton);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.触发模式comboBox);
            this.groupBox6.Controls.Add(this.Darkbutton);
            this.groupBox6.Location = new System.Drawing.Point(5, 173);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(291, 130);
            this.groupBox6.TabIndex = 60;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "触发配置";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 71;
            this.label5.Text = "触发电平";
            // 
            // 触发电平comboBox
            // 
            this.触发电平comboBox.FormattingEnabled = true;
            this.触发电平comboBox.Location = new System.Drawing.Point(61, 72);
            this.触发电平comboBox.Name = "触发电平comboBox";
            this.触发电平comboBox.Size = new System.Drawing.Size(102, 20);
            this.触发电平comboBox.TabIndex = 70;
            // 
            // 取消等待采集checkBox
            // 
            this.取消等待采集checkBox.AutoSize = true;
            this.取消等待采集checkBox.Location = new System.Drawing.Point(189, 85);
            this.取消等待采集checkBox.Name = "取消等待采集checkBox";
            this.取消等待采集checkBox.Size = new System.Drawing.Size(96, 16);
            this.取消等待采集checkBox.TabIndex = 69;
            this.取消等待采集checkBox.Text = "取消等待采集";
            this.取消等待采集checkBox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 65;
            this.label9.Text = "触发源";
            // 
            // 触发源comboBox
            // 
            this.触发源comboBox.FormattingEnabled = true;
            this.触发源comboBox.Location = new System.Drawing.Point(61, 18);
            this.触发源comboBox.Name = "触发源comboBox";
            this.触发源comboBox.Size = new System.Drawing.Size(102, 20);
            this.触发源comboBox.TabIndex = 64;
            // 
            // 采集点数textBox
            // 
            this.采集点数textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.采集点数textBox.Location = new System.Drawing.Point(61, 98);
            this.采集点数textBox.Name = "采集点数textBox";
            this.采集点数textBox.Size = new System.Drawing.Size(102, 21);
            this.采集点数textBox.TabIndex = 61;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 60;
            this.label4.Text = "采集点数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "触发模式";
            // 
            // 触发模式comboBox
            // 
            this.触发模式comboBox.FormattingEnabled = true;
            this.触发模式comboBox.Location = new System.Drawing.Point(61, 44);
            this.触发模式comboBox.Name = "触发模式comboBox";
            this.触发模式comboBox.Size = new System.Drawing.Size(102, 20);
            this.触发模式comboBox.TabIndex = 19;
            // 
            // 监控panel
            // 
            this.监控panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.监控panel.Location = new System.Drawing.Point(299, 121);
            this.监控panel.Name = "监控panel";
            this.监控panel.Size = new System.Drawing.Size(267, 182);
            this.监控panel.TabIndex = 61;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SPPtextBox);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.SDPtextBox);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tb_Threshoud);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(141, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(155, 93);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "阈值";
            // 
            // SDPtextBox
            // 
            this.SDPtextBox.Location = new System.Drawing.Point(83, 41);
            this.SDPtextBox.Name = "SDPtextBox";
            this.SDPtextBox.Size = new System.Drawing.Size(67, 21);
            this.SDPtextBox.TabIndex = 22;
            this.SDPtextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SDPtextBox_KeyUp);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 12);
            this.label11.TabIndex = 21;
            this.label11.Text = "阈值SDP(0-1）";
            // 
            // SPPtextBox
            // 
            this.SPPtextBox.Location = new System.Drawing.Point(83, 68);
            this.SPPtextBox.Name = "SPPtextBox";
            this.SPPtextBox.Size = new System.Drawing.Size(67, 21);
            this.SPPtextBox.TabIndex = 24;
            this.SPPtextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SPPtextBox_KeyUp);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 74);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 12);
            this.label12.TabIndex = 23;
            this.label12.Text = "阈值SPP(0-1）";
            // 
            // StilPointLaserParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 304);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.监控panel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "StilPointLaserParamForm";
            this.Text = "Stil点光谱参数设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StilPointLaserParamForm_FormClosing);
            this.Load += new System.EventHandler(this.StilPointLaserParamForm_Load);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarManual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarAuto)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Darkbutton;
        private System.Windows.Forms.Button Savebutton;
        private System.Windows.Forms.TextBox tb_Threshoud;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb_manualMode;
        private System.Windows.Forms.RadioButton rb_Auto;
        private System.Windows.Forms.TrackBar trackbarManual;
        private System.Windows.Forms.TrackBar trackbarAuto;
        private System.Windows.Forms.Label AutoLightlabel;
        private System.Windows.Forms.Label ManualValuelabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_OpticalPen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_MinRate;
        private System.Windows.Forms.ComboBox cmb_RateList;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 触发模式comboBox;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Panel 监控panel;
        private System.Windows.Forms.TextBox 采集点数textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox 测量模式comboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox 峰值选择comboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox 触发源comboBox;
        private System.Windows.Forms.CheckBox 取消等待采集checkBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 触发电平comboBox;
        private System.Windows.Forms.TextBox SPPtextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox SDPtextBox;
        private System.Windows.Forms.Label label11;
    }
}