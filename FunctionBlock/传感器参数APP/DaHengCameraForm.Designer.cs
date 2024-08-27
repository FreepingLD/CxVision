namespace FunctionBlock
{
    partial class DaHengCameraParamForm
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
            this.触发极性comBox = new System.Windows.Forms.ComboBox();
            this.触发源comBox = new System.Windows.Forms.ComboBox();
            this.m_btn_SoftTriggerCommand = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.触发模式comBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_lbl_Gain = new System.Windows.Forms.Label();
            this.增益texBox = new System.Windows.Forms.TextBox();
            this.m_lbl_Shutter = new System.Windows.Forms.Label();
            this.曝光时间texBox = new System.Windows.Forms.TextBox();
            this.监控panel = new System.Windows.Forms.Panel();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.触发极性comBox);
            this.groupBox5.Controls.Add(this.触发源comBox);
            this.groupBox5.Controls.Add(this.m_btn_SoftTriggerCommand);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.触发模式comBox);
            this.groupBox5.Location = new System.Drawing.Point(0, 89);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(281, 127);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "触发控制";
            // 
            // 触发极性comBox
            // 
            this.触发极性comBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.触发极性comBox.FormattingEnabled = true;
            this.触发极性comBox.Items.AddRange(new object[] {
            "RisingEdge",
            "FallingEdge"});
            this.触发极性comBox.Location = new System.Drawing.Point(149, 74);
            this.触发极性comBox.Name = "触发极性comBox";
            this.触发极性comBox.Size = new System.Drawing.Size(125, 20);
            this.触发极性comBox.TabIndex = 12;
            this.触发极性comBox.SelectionChangeCommitted += new System.EventHandler(this.触发极性comBox_SelectionChangeCommitted);
            // 
            // 触发源comBox
            // 
            this.触发源comBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.触发源comBox.FormattingEnabled = true;
            this.触发源comBox.Items.AddRange(new object[] {
            "Software",
            "Line0",
            "Line1",
            "Line2",
            "Line3"});
            this.触发源comBox.Location = new System.Drawing.Point(149, 47);
            this.触发源comBox.Name = "触发源comBox";
            this.触发源comBox.Size = new System.Drawing.Size(125, 20);
            this.触发源comBox.TabIndex = 8;
            this.触发源comBox.SelectionChangeCommitted += new System.EventHandler(this.触发源comBox_SelectionChangeCommitted);
            // 
            // m_btn_SoftTriggerCommand
            // 
            this.m_btn_SoftTriggerCommand.Location = new System.Drawing.Point(149, 99);
            this.m_btn_SoftTriggerCommand.Name = "m_btn_SoftTriggerCommand";
            this.m_btn_SoftTriggerCommand.Size = new System.Drawing.Size(125, 23);
            this.m_btn_SoftTriggerCommand.TabIndex = 10;
            this.m_btn_SoftTriggerCommand.Text = "发送软触发命令";
            this.m_btn_SoftTriggerCommand.UseVisualStyleBackColor = true;
            this.m_btn_SoftTriggerCommand.Click += new System.EventHandler(this.m_btn_SoftTriggerCommand_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "触发极性";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "软触发";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "触发源";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
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
            "On",
            "Off"});
            this.触发模式comBox.Location = new System.Drawing.Point(149, 21);
            this.触发模式comBox.Name = "触发模式comBox";
            this.触发模式comBox.Size = new System.Drawing.Size(125, 20);
            this.触发模式comBox.TabIndex = 6;
            this.触发模式comBox.SelectionChangeCommitted += new System.EventHandler(this.触发模式comBox_SelectionChangeCommitted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_lbl_Gain);
            this.groupBox1.Controls.Add(this.增益texBox);
            this.groupBox1.Controls.Add(this.m_lbl_Shutter);
            this.groupBox1.Controls.Add(this.曝光时间texBox);
            this.groupBox1.Location = new System.Drawing.Point(0, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 82);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机参数设置 ";
            // 
            // m_lbl_Gain
            // 
            this.m_lbl_Gain.AutoSize = true;
            this.m_lbl_Gain.Location = new System.Drawing.Point(6, 56);
            this.m_lbl_Gain.Name = "m_lbl_Gain";
            this.m_lbl_Gain.Size = new System.Drawing.Size(29, 12);
            this.m_lbl_Gain.TabIndex = 19;
            this.m_lbl_Gain.Text = "增益";
            // 
            // 增益texBox
            // 
            this.增益texBox.Location = new System.Drawing.Point(149, 56);
            this.增益texBox.Name = "增益texBox";
            this.增益texBox.Size = new System.Drawing.Size(126, 21);
            this.增益texBox.TabIndex = 20;
            this.增益texBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.增益texBox_KeyUp);
            // 
            // m_lbl_Shutter
            // 
            this.m_lbl_Shutter.AutoSize = true;
            this.m_lbl_Shutter.Location = new System.Drawing.Point(6, 29);
            this.m_lbl_Shutter.Name = "m_lbl_Shutter";
            this.m_lbl_Shutter.Size = new System.Drawing.Size(53, 12);
            this.m_lbl_Shutter.TabIndex = 17;
            this.m_lbl_Shutter.Text = "曝光时间";
            // 
            // 曝光时间texBox
            // 
            this.曝光时间texBox.Location = new System.Drawing.Point(149, 29);
            this.曝光时间texBox.Name = "曝光时间texBox";
            this.曝光时间texBox.Size = new System.Drawing.Size(126, 21);
            this.曝光时间texBox.TabIndex = 18;
            this.曝光时间texBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.曝光时间texBox_KeyUp);
            // 
            // 监控panel
            // 
            this.监控panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.监控panel.Location = new System.Drawing.Point(287, 0);
            this.监控panel.Name = "监控panel";
            this.监控panel.Size = new System.Drawing.Size(778, 633);
            this.监控panel.TabIndex = 6;
            // 
            // DaHengCameraParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 637);
            this.Controls.Add(this.监控panel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DaHengCameraParamForm";
            this.Text = "大恒相机窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DaHengCameraForm_FormClosing);
            this.Load += new System.EventHandler(this.DaHengCameraForm_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox 触发极性comBox;
        private System.Windows.Forms.ComboBox 触发源comBox;
        private System.Windows.Forms.Button m_btn_SoftTriggerCommand;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 触发模式comBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label m_lbl_Gain;
        private System.Windows.Forms.TextBox 增益texBox;
        private System.Windows.Forms.Label m_lbl_Shutter;
        private System.Windows.Forms.TextBox 曝光时间texBox;
        private System.Windows.Forms.Panel 监控panel;
    }
}