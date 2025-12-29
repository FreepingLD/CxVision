namespace FunctionBlock
{
    partial class LocalThresholdForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.参数值comboBox = new System.Windows.Forms.ComboBox();
            this.参数名称comboBox = new System.Windows.Forms.ComboBox();
            this.亮暗comboBox = new System.Windows.Forms.ComboBox();
            this.方法comboBox = new System.Windows.Forms.ComboBox();
            this.区域连通comboBox = new System.Windows.Forms.ComboBox();
            this.区域填充comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "方法";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "亮/暗";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "参数名称";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "参数值";
            // 
            // 参数值comboBox
            // 
            this.参数值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.参数值comboBox.FormattingEnabled = true;
            this.参数值comboBox.Items.AddRange(new object[] {
            "0.2",
            "15",
            "30",
            "128.0"});
            this.参数值comboBox.Location = new System.Drawing.Point(61, 82);
            this.参数值comboBox.Name = "参数值comboBox";
            this.参数值comboBox.Size = new System.Drawing.Size(255, 20);
            this.参数值comboBox.TabIndex = 11;
            // 
            // 参数名称comboBox
            // 
            this.参数名称comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.参数名称comboBox.FormattingEnabled = true;
            this.参数名称comboBox.Items.AddRange(new object[] {
            "mask_size",
            "range",
            "scale"});
            this.参数名称comboBox.Location = new System.Drawing.Point(61, 56);
            this.参数名称comboBox.Name = "参数名称comboBox";
            this.参数名称comboBox.Size = new System.Drawing.Size(255, 20);
            this.参数名称comboBox.TabIndex = 12;
            // 
            // 亮暗comboBox
            // 
            this.亮暗comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.亮暗comboBox.FormattingEnabled = true;
            this.亮暗comboBox.Items.AddRange(new object[] {
            "dark",
            "light"});
            this.亮暗comboBox.Location = new System.Drawing.Point(61, 29);
            this.亮暗comboBox.Name = "亮暗comboBox";
            this.亮暗comboBox.Size = new System.Drawing.Size(255, 20);
            this.亮暗comboBox.TabIndex = 13;
            // 
            // 方法comboBox
            // 
            this.方法comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.方法comboBox.FormattingEnabled = true;
            this.方法comboBox.Items.AddRange(new object[] {
            "adapted_std_deviation"});
            this.方法comboBox.Location = new System.Drawing.Point(61, 4);
            this.方法comboBox.Name = "方法comboBox";
            this.方法comboBox.Size = new System.Drawing.Size(255, 20);
            this.方法comboBox.TabIndex = 14;
            // 
            // 区域连通comboBox
            // 
            this.区域连通comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域连通comboBox.FormattingEnabled = true;
            this.区域连通comboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.区域连通comboBox.Location = new System.Drawing.Point(61, 134);
            this.区域连通comboBox.Name = "区域连通comboBox";
            this.区域连通comboBox.Size = new System.Drawing.Size(255, 20);
            this.区域连通comboBox.TabIndex = 35;
            // 
            // 区域填充comboBox
            // 
            this.区域填充comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域填充comboBox.FormattingEnabled = true;
            this.区域填充comboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.区域填充comboBox.Location = new System.Drawing.Point(61, 108);
            this.区域填充comboBox.Name = "区域填充comboBox";
            this.区域填充comboBox.Size = new System.Drawing.Size(255, 20);
            this.区域填充comboBox.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "区域连通";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 32;
            this.label6.Text = "区域填充";
            // 
            // LocalThresholdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 166);
            this.Controls.Add(this.区域连通comboBox);
            this.Controls.Add(this.区域填充comboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.方法comboBox);
            this.Controls.Add(this.亮暗comboBox);
            this.Controls.Add(this.参数名称comboBox);
            this.Controls.Add(this.参数值comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LocalThresholdForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 参数值comboBox;
        private System.Windows.Forms.ComboBox 参数名称comboBox;
        private System.Windows.Forms.ComboBox 亮暗comboBox;
        private System.Windows.Forms.ComboBox 方法comboBox;
        private System.Windows.Forms.ComboBox 区域连通comboBox;
        private System.Windows.Forms.ComboBox 区域填充comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}