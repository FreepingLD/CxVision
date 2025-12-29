namespace FunctionBlock
{
    partial class KeyEnceLJV700ParamForm
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
            this.label9 = new System.Windows.Forms.Label();
            this.触发模式Combox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.采集点数textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.触发源comboBox = new System.Windows.Forms.ComboBox();
            this.监控panel = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "触发模式";
            // 
            // 触发模式Combox
            // 
            this.触发模式Combox.FormattingEnabled = true;
            this.触发模式Combox.Location = new System.Drawing.Point(70, 46);
            this.触发模式Combox.Name = "触发模式Combox";
            this.触发模式Combox.Size = new System.Drawing.Size(111, 21);
            this.触发模式Combox.TabIndex = 11;
            this.触发模式Combox.SelectionChangeCommitted += new System.EventHandler(this.TriggerModeCombox_SelectionChangeCommitted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.采集点数textBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.触发源comboBox);
            this.groupBox1.Controls.Add(this.触发模式Combox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(6, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 577);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LJV7000激光参数";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 71;
            this.label8.Text = "采集点数";
            // 
            // 采集点数textBox
            // 
            this.采集点数textBox.Location = new System.Drawing.Point(70, 73);
            this.采集点数textBox.Name = "采集点数textBox";
            this.采集点数textBox.Size = new System.Drawing.Size(111, 20);
            this.采集点数textBox.TabIndex = 72;
            this.采集点数textBox.Text = "1";
            this.采集点数textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.采集点数textBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 67;
            this.label1.Text = "触发源";
            // 
            // 触发源comboBox
            // 
            this.触发源comboBox.FormattingEnabled = true;
            this.触发源comboBox.Location = new System.Drawing.Point(70, 19);
            this.触发源comboBox.Name = "触发源comboBox";
            this.触发源comboBox.Size = new System.Drawing.Size(111, 21);
            this.触发源comboBox.TabIndex = 66;
            // 
            // 监控panel
            // 
            this.监控panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.监控panel.Location = new System.Drawing.Point(199, -2);
            this.监控panel.Name = "监控panel";
            this.监控panel.Size = new System.Drawing.Size(685, 593);
            this.监控panel.TabIndex = 13;
            // 
            // KeyEnceLJV700ParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 589);
            this.Controls.Add(this.监控panel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "KeyEnceLJV700ParamForm";
            this.Text = "基恩士LJV7000参数设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StilLineLaserParamForm_FormClosing);
            this.Load += new System.EventHandler(this.StilLineLaserParamForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox 触发模式Combox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel 监控panel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 触发源comboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox 采集点数textBox;
    }
}