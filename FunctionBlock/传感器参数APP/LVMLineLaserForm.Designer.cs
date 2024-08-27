namespace FunctionBlock
{
    partial class LVMLineLaserParamForm
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.触发周期textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.触发模式comBox = new System.Windows.Forms.ComboBox();
            this.m_lbl_Gain = new System.Windows.Forms.Label();
            this.增益texBox = new System.Windows.Forms.TextBox();
            this.m_lbl_Shutter = new System.Windows.Forms.Label();
            this.曝光时间texBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.光源亮度textBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.曝光模式comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.测量点数textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.移动平滑系数textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.折射率模式comboBox = new System.Windows.Forms.ComboBox();
            this.折射率textBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.测量模式comboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.平均值textBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.检测阈值textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.峰值模式comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.监控panel = new System.Windows.Forms.Panel();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.触发周期textBox);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.触发模式comBox);
            this.groupBox5.Location = new System.Drawing.Point(5, 347);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(272, 170);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "触发控制";
            // 
            // 触发周期textBox
            // 
            this.触发周期textBox.Location = new System.Drawing.Point(138, 46);
            this.触发周期textBox.Name = "触发周期textBox";
            this.触发周期textBox.Size = new System.Drawing.Size(128, 21);
            this.触发周期textBox.TabIndex = 22;
            this.触发周期textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.触发周期textBox_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "触发周期(200-10000us)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "触发模式";
            // 
            // 触发模式comBox
            // 
            this.触发模式comBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.触发模式comBox.FormattingEnabled = true;
            this.触发模式comBox.Items.AddRange(new object[] {
            "软触发",
            "硬触发"});
            this.触发模式comBox.Location = new System.Drawing.Point(138, 21);
            this.触发模式comBox.Name = "触发模式comBox";
            this.触发模式comBox.Size = new System.Drawing.Size(128, 20);
            this.触发模式comBox.TabIndex = 6;
            this.触发模式comBox.SelectionChangeCommitted += new System.EventHandler(this.触发模式comBox_SelectionChangeCommitted);
            // 
            // m_lbl_Gain
            // 
            this.m_lbl_Gain.AutoSize = true;
            this.m_lbl_Gain.Location = new System.Drawing.Point(2, 66);
            this.m_lbl_Gain.Name = "m_lbl_Gain";
            this.m_lbl_Gain.Size = new System.Drawing.Size(65, 12);
            this.m_lbl_Gain.TabIndex = 19;
            this.m_lbl_Gain.Text = "增益(1~16)";
            // 
            // 增益texBox
            // 
            this.增益texBox.Location = new System.Drawing.Point(138, 63);
            this.增益texBox.Name = "增益texBox";
            this.增益texBox.Size = new System.Drawing.Size(128, 21);
            this.增益texBox.TabIndex = 20;
            this.增益texBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.增益texBox_KeyUp_1);
            // 
            // m_lbl_Shutter
            // 
            this.m_lbl_Shutter.AutoSize = true;
            this.m_lbl_Shutter.Location = new System.Drawing.Point(2, 42);
            this.m_lbl_Shutter.Name = "m_lbl_Shutter";
            this.m_lbl_Shutter.Size = new System.Drawing.Size(131, 12);
            this.m_lbl_Shutter.TabIndex = 17;
            this.m_lbl_Shutter.Text = "曝光时间（20~10000us)";
            // 
            // 曝光时间texBox
            // 
            this.曝光时间texBox.Location = new System.Drawing.Point(138, 38);
            this.曝光时间texBox.Name = "曝光时间texBox";
            this.曝光时间texBox.Size = new System.Drawing.Size(128, 21);
            this.曝光时间texBox.TabIndex = 18;
            this.曝光时间texBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.曝光时间texBox_KeyUp_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.光源亮度textBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.曝光模式comboBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.m_lbl_Shutter);
            this.groupBox2.Controls.Add(this.曝光时间texBox);
            this.groupBox2.Controls.Add(this.增益texBox);
            this.groupBox2.Controls.Add(this.m_lbl_Gain);
            this.groupBox2.Location = new System.Drawing.Point(5, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 114);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置相机/光源参数";
            // 
            // 光源亮度textBox
            // 
            this.光源亮度textBox.Location = new System.Drawing.Point(138, 89);
            this.光源亮度textBox.Name = "光源亮度textBox";
            this.光源亮度textBox.Size = new System.Drawing.Size(128, 21);
            this.光源亮度textBox.TabIndex = 24;
            this.光源亮度textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.光源亮度textBox_KeyUp);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "光源亮度";
            // 
            // 曝光模式comboBox
            // 
            this.曝光模式comboBox.FormattingEnabled = true;
            this.曝光模式comboBox.Location = new System.Drawing.Point(138, 14);
            this.曝光模式comboBox.Name = "曝光模式comboBox";
            this.曝光模式comboBox.Size = new System.Drawing.Size(128, 20);
            this.曝光模式comboBox.TabIndex = 22;
            this.曝光模式comboBox.SelectionChangeCommitted += new System.EventHandler(this.曝光模式comboBox_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "曝光模式";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.测量点数textBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.移动平滑系数textBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.折射率模式comboBox);
            this.groupBox3.Controls.Add(this.折射率textBox);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.测量模式comboBox);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.平均值textBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.检测阈值textBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.峰值模式comboBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(5, 119);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(272, 224);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "其他参数设置";
            // 
            // 测量点数textBox
            // 
            this.测量点数textBox.Location = new System.Drawing.Point(138, 193);
            this.测量点数textBox.Name = "测量点数textBox";
            this.测量点数textBox.Size = new System.Drawing.Size(128, 21);
            this.测量点数textBox.TabIndex = 39;
            this.测量点数textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.测量点数textBox_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 12);
            this.label7.TabIndex = 38;
            this.label7.Text = "测量点数(1~100000)";
            // 
            // 移动平滑系数textBox
            // 
            this.移动平滑系数textBox.Location = new System.Drawing.Point(138, 115);
            this.移动平滑系数textBox.Name = "移动平滑系数textBox";
            this.移动平滑系数textBox.Size = new System.Drawing.Size(128, 21);
            this.移动平滑系数textBox.TabIndex = 37;
            this.移动平滑系数textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.移动平滑系数textBox_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "移动平滑系数";
            // 
            // 折射率模式comboBox
            // 
            this.折射率模式comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.折射率模式comboBox.FormattingEnabled = true;
            this.折射率模式comboBox.Location = new System.Drawing.Point(138, 141);
            this.折射率模式comboBox.Name = "折射率模式comboBox";
            this.折射率模式comboBox.Size = new System.Drawing.Size(128, 20);
            this.折射率模式comboBox.TabIndex = 35;
            this.折射率模式comboBox.SelectionChangeCommitted += new System.EventHandler(this.折射率模式comboBox_SelectionChangeCommitted);
            // 
            // 折射率textBox
            // 
            this.折射率textBox.Location = new System.Drawing.Point(138, 166);
            this.折射率textBox.Name = "折射率textBox";
            this.折射率textBox.Size = new System.Drawing.Size(128, 21);
            this.折射率textBox.TabIndex = 34;
            this.折射率textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.折射率textBox_KeyUp);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 169);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 33;
            this.label13.Text = "自定义模式折射率";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 145);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 31;
            this.label12.Text = "拆射率模式";
            // 
            // 测量模式comboBox
            // 
            this.测量模式comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.测量模式comboBox.FormattingEnabled = true;
            this.测量模式comboBox.Location = new System.Drawing.Point(138, 40);
            this.测量模式comboBox.Name = "测量模式comboBox";
            this.测量模式comboBox.Size = new System.Drawing.Size(128, 20);
            this.测量模式comboBox.TabIndex = 30;
            this.测量模式comboBox.SelectionChangeCommitted += new System.EventHandler(this.测量模式comboBox_SelectionChangeCommitted);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 29;
            this.label11.Text = "测量模式";
            // 
            // 平均值textBox
            // 
            this.平均值textBox.Location = new System.Drawing.Point(138, 89);
            this.平均值textBox.Name = "平均值textBox";
            this.平均值textBox.Size = new System.Drawing.Size(128, 21);
            this.平均值textBox.TabIndex = 28;
            this.平均值textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.平均值textBox_KeyUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 27;
            this.label10.Text = "平均值";
            // 
            // 检测阈值textBox
            // 
            this.检测阈值textBox.Location = new System.Drawing.Point(138, 64);
            this.检测阈值textBox.Name = "检测阈值textBox";
            this.检测阈值textBox.Size = new System.Drawing.Size(128, 21);
            this.检测阈值textBox.TabIndex = 26;
            this.检测阈值textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.检测阈值textBox_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "检测阈值（0~4095）";
            // 
            // 峰值模式comboBox
            // 
            this.峰值模式comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.峰值模式comboBox.FormattingEnabled = true;
            this.峰值模式comboBox.Location = new System.Drawing.Point(138, 16);
            this.峰值模式comboBox.Name = "峰值模式comboBox";
            this.峰值模式comboBox.Size = new System.Drawing.Size(128, 20);
            this.峰值模式comboBox.TabIndex = 24;
            this.峰值模式comboBox.SelectionChangeCommitted += new System.EventHandler(this.峰值模式comboBox_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "峰值模式";
            // 
            // 监控panel
            // 
            this.监控panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.监控panel.Location = new System.Drawing.Point(284, 3);
            this.监控panel.Name = "监控panel";
            this.监控panel.Size = new System.Drawing.Size(607, 514);
            this.监控panel.TabIndex = 25;
            // 
            // LVMLineLaserParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 518);
            this.Controls.Add(this.监控panel);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LVMLineLaserParamForm";
            this.Text = "LVM激光窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BoMingPointLaserForm_FormClosing);
            this.Load += new System.EventHandler(this.LVMLinetLaserForm_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 触发模式comBox;
        private System.Windows.Forms.Label m_lbl_Gain;
        private System.Windows.Forms.TextBox 增益texBox;
        private System.Windows.Forms.Label m_lbl_Shutter;
        private System.Windows.Forms.TextBox 曝光时间texBox;
        private System.Windows.Forms.TextBox 触发周期textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox 曝光模式comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox 检测阈值textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox 峰值模式comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 光源亮度textBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox 平均值textBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox 测量点数textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox 移动平滑系数textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 折射率模式comboBox;
        private System.Windows.Forms.TextBox 折射率textBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox 测量模式comboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel 监控panel;
    }
}