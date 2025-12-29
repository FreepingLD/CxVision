namespace FunctionBlock
{
    partial class StilLineLaserParamForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LEDtextBox = new System.Windows.Forms.TextBox();
            this.ExposeTimetextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DetectiontextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TriggerModeCombox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.AltitudePeakcomboBox = new System.Windows.Forms.ComboBox();
            this.TRENumtextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.MeasureModecomboBox = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.ThicknessPeak1comboBox = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.ThicknessPeak2comboBox = new System.Windows.Forms.ComboBox();
            this.IntensityNormalizationbutton = new System.Windows.Forms.Button();
            this.SaveConfigbutton = new System.Windows.Forms.Button();
            this.ThremalCorrectionbutton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.MeasureRange = new System.Windows.Forms.TextBox();
            this.DarkAcqbutton = new System.Windows.Forms.Button();
            this.置零编码器button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.触发源comboBox = new System.Windows.Forms.ComboBox();
            this.监控panel = new System.Windows.Forms.Panel();
            this.取消等待采集checkBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "亮度 %";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "曝光 [us]";
            // 
            // LEDtextBox
            // 
            this.LEDtextBox.Location = new System.Drawing.Point(88, 18);
            this.LEDtextBox.Name = "LEDtextBox";
            this.LEDtextBox.Size = new System.Drawing.Size(92, 21);
            this.LEDtextBox.TabIndex = 2;
            this.LEDtextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LEDtextBox_KeyUp);
            // 
            // ExposeTimetextBox
            // 
            this.ExposeTimetextBox.Location = new System.Drawing.Point(88, 45);
            this.ExposeTimetextBox.Name = "ExposeTimetextBox";
            this.ExposeTimetextBox.Size = new System.Drawing.Size(92, 21);
            this.ExposeTimetextBox.TabIndex = 3;
            this.ExposeTimetextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExposeTimetextBox_KeyUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "阈值 [0-1]";
            // 
            // DetectiontextBox
            // 
            this.DetectiontextBox.Location = new System.Drawing.Point(88, 72);
            this.DetectiontextBox.Name = "DetectiontextBox";
            this.DetectiontextBox.Size = new System.Drawing.Size(92, 21);
            this.DetectiontextBox.TabIndex = 5;
            this.DetectiontextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DetectiontextBox_KeyUp);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "触发模式";
            // 
            // TriggerModeCombox
            // 
            this.TriggerModeCombox.FormattingEnabled = true;
            this.TriggerModeCombox.Location = new System.Drawing.Point(88, 123);
            this.TriggerModeCombox.Name = "TriggerModeCombox";
            this.TriggerModeCombox.Size = new System.Drawing.Size(92, 20);
            this.TriggerModeCombox.TabIndex = 11;
            this.TriggerModeCombox.SelectionChangeCommitted += new System.EventHandler(this.TriggerModeCombox_SelectionChangeCommitted);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 206);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "高度模式峰值";
            // 
            // AltitudePeakcomboBox
            // 
            this.AltitudePeakcomboBox.FormattingEnabled = true;
            this.AltitudePeakcomboBox.Location = new System.Drawing.Point(88, 202);
            this.AltitudePeakcomboBox.Name = "AltitudePeakcomboBox";
            this.AltitudePeakcomboBox.Size = new System.Drawing.Size(92, 20);
            this.AltitudePeakcomboBox.TabIndex = 19;
            this.AltitudePeakcomboBox.SelectionChangeCommitted += new System.EventHandler(this.AltitudePeakcomboBox_SelectionChangeCommitted);
            // 
            // TRENumtextBox
            // 
            this.TRENumtextBox.Location = new System.Drawing.Point(88, 149);
            this.TRENumtextBox.Name = "TRENumtextBox";
            this.TRENumtextBox.Size = new System.Drawing.Size(92, 21);
            this.TRENumtextBox.TabIndex = 9;
            this.TRENumtextBox.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "NumberTRE ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 179);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 28;
            this.label13.Text = "测量模式";
            // 
            // MeasureModecomboBox
            // 
            this.MeasureModecomboBox.FormattingEnabled = true;
            this.MeasureModecomboBox.Location = new System.Drawing.Point(88, 176);
            this.MeasureModecomboBox.Name = "MeasureModecomboBox";
            this.MeasureModecomboBox.Size = new System.Drawing.Size(92, 20);
            this.MeasureModecomboBox.TabIndex = 29;
            this.MeasureModecomboBox.SelectionChangeCommitted += new System.EventHandler(this.MeasureModecomboBox_SelectionChangeCommitted);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 232);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 12);
            this.label18.TabIndex = 31;
            this.label18.Text = "厚度模式峰值1";
            // 
            // ThicknessPeak1comboBox
            // 
            this.ThicknessPeak1comboBox.FormattingEnabled = true;
            this.ThicknessPeak1comboBox.Location = new System.Drawing.Point(88, 228);
            this.ThicknessPeak1comboBox.Name = "ThicknessPeak1comboBox";
            this.ThicknessPeak1comboBox.Size = new System.Drawing.Size(92, 20);
            this.ThicknessPeak1comboBox.TabIndex = 32;
            this.ThicknessPeak1comboBox.SelectionChangeCommitted += new System.EventHandler(this.ThicknessPeak1comboBox_SelectionChangeCommitted);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 258);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 12);
            this.label19.TabIndex = 34;
            this.label19.Text = "厚度模式峰值2";
            // 
            // ThicknessPeak2comboBox
            // 
            this.ThicknessPeak2comboBox.FormattingEnabled = true;
            this.ThicknessPeak2comboBox.Location = new System.Drawing.Point(88, 254);
            this.ThicknessPeak2comboBox.Name = "ThicknessPeak2comboBox";
            this.ThicknessPeak2comboBox.Size = new System.Drawing.Size(92, 20);
            this.ThicknessPeak2comboBox.TabIndex = 35;
            this.ThicknessPeak2comboBox.SelectionChangeCommitted += new System.EventHandler(this.ThicknessPeak2comboBox_SelectionChangeCommitted);
            // 
            // IntensityNormalizationbutton
            // 
            this.IntensityNormalizationbutton.Location = new System.Drawing.Point(114, 343);
            this.IntensityNormalizationbutton.Name = "IntensityNormalizationbutton";
            this.IntensityNormalizationbutton.Size = new System.Drawing.Size(67, 24);
            this.IntensityNormalizationbutton.TabIndex = 24;
            this.IntensityNormalizationbutton.Text = "光强校正";
            this.IntensityNormalizationbutton.UseVisualStyleBackColor = true;
            // 
            // SaveConfigbutton
            // 
            this.SaveConfigbutton.Location = new System.Drawing.Point(7, 342);
            this.SaveConfigbutton.Name = "SaveConfigbutton";
            this.SaveConfigbutton.Size = new System.Drawing.Size(69, 27);
            this.SaveConfigbutton.TabIndex = 21;
            this.SaveConfigbutton.Text = "保存配置";
            this.SaveConfigbutton.UseVisualStyleBackColor = true;
            this.SaveConfigbutton.Click += new System.EventHandler(this.SaveConfigbutton_Click);
            // 
            // ThremalCorrectionbutton
            // 
            this.ThremalCorrectionbutton.Enabled = false;
            this.ThremalCorrectionbutton.Location = new System.Drawing.Point(7, 404);
            this.ThremalCorrectionbutton.Name = "ThremalCorrectionbutton";
            this.ThremalCorrectionbutton.Size = new System.Drawing.Size(69, 27);
            this.ThremalCorrectionbutton.TabIndex = 23;
            this.ThremalCorrectionbutton.Text = "执校正";
            this.ThremalCorrectionbutton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 284);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 37;
            this.label2.Text = "量程";
            // 
            // MeasureRange
            // 
            this.MeasureRange.Location = new System.Drawing.Point(88, 280);
            this.MeasureRange.Name = "MeasureRange";
            this.MeasureRange.Size = new System.Drawing.Size(92, 21);
            this.MeasureRange.TabIndex = 38;
            this.MeasureRange.Text = "100";
            // 
            // DarkAcqbutton
            // 
            this.DarkAcqbutton.Location = new System.Drawing.Point(7, 375);
            this.DarkAcqbutton.Name = "DarkAcqbutton";
            this.DarkAcqbutton.Size = new System.Drawing.Size(69, 26);
            this.DarkAcqbutton.TabIndex = 15;
            this.DarkAcqbutton.Text = "暗校正";
            this.DarkAcqbutton.UseVisualStyleBackColor = true;
            // 
            // 置零编码器button
            // 
            this.置零编码器button.Location = new System.Drawing.Point(114, 375);
            this.置零编码器button.Name = "置零编码器button";
            this.置零编码器button.Size = new System.Drawing.Size(67, 24);
            this.置零编码器button.TabIndex = 39;
            this.置零编码器button.Text = "置零编码器";
            this.置零编码器button.UseVisualStyleBackColor = true;
            this.置零编码器button.Click += new System.EventHandler(this.置零编码器button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.取消等待采集checkBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.触发源comboBox);
            this.groupBox1.Controls.Add(this.置零编码器button);
            this.groupBox1.Controls.Add(this.DarkAcqbutton);
            this.groupBox1.Controls.Add(this.MeasureRange);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ThremalCorrectionbutton);
            this.groupBox1.Controls.Add(this.SaveConfigbutton);
            this.groupBox1.Controls.Add(this.IntensityNormalizationbutton);
            this.groupBox1.Controls.Add(this.ThicknessPeak2comboBox);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.ThicknessPeak1comboBox);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.MeasureModecomboBox);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TRENumtextBox);
            this.groupBox1.Controls.Add(this.AltitudePeakcomboBox);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.TriggerModeCombox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.DetectiontextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.ExposeTimetextBox);
            this.groupBox1.Controls.Add(this.LEDtextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 533);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stil线激光参数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "触发源";
            // 
            // 触发源comboBox
            // 
            this.触发源comboBox.FormattingEnabled = true;
            this.触发源comboBox.Location = new System.Drawing.Point(88, 98);
            this.触发源comboBox.Name = "触发源comboBox";
            this.触发源comboBox.Size = new System.Drawing.Size(92, 20);
            this.触发源comboBox.TabIndex = 66;
            // 
            // 监控panel
            // 
            this.监控panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.监控panel.Location = new System.Drawing.Point(199, -2);
            this.监控panel.Name = "监控panel";
            this.监控panel.Size = new System.Drawing.Size(685, 547);
            this.监控panel.TabIndex = 13;
            // 
            // 取消等待采集checkBox
            // 
            this.取消等待采集checkBox.AutoSize = true;
            this.取消等待采集checkBox.Location = new System.Drawing.Point(84, 307);
            this.取消等待采集checkBox.Name = "取消等待采集checkBox";
            this.取消等待采集checkBox.Size = new System.Drawing.Size(96, 16);
            this.取消等待采集checkBox.TabIndex = 70;
            this.取消等待采集checkBox.Text = "取消等待采集";
            this.取消等待采集checkBox.UseVisualStyleBackColor = true;
            // 
            // StilLineLaserParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 544);
            this.Controls.Add(this.监控panel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "StilLineLaserParamForm";
            this.Text = "Stil线光谱参数设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StilLineLaserParamForm_FormClosing);
            this.Load += new System.EventHandler(this.StilLineLaserParamForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox LEDtextBox;
        private System.Windows.Forms.TextBox ExposeTimetextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox DetectiontextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox TriggerModeCombox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox AltitudePeakcomboBox;
        private System.Windows.Forms.TextBox TRENumtextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox MeasureModecomboBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox ThicknessPeak1comboBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox ThicknessPeak2comboBox;
        private System.Windows.Forms.Button IntensityNormalizationbutton;
        private System.Windows.Forms.Button SaveConfigbutton;
        private System.Windows.Forms.Button ThremalCorrectionbutton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MeasureRange;
        private System.Windows.Forms.Button DarkAcqbutton;
        private System.Windows.Forms.Button 置零编码器button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel 监控panel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 触发源comboBox;
        private System.Windows.Forms.CheckBox 取消等待采集checkBox;
    }
}