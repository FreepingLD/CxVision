namespace FunctionBlock
{
    partial class FindBarCodeForm
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
            this.解码个数textBox = new System.Windows.Forms.TextBox();
            this.解码超时textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.条码类型comboBox = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.保存方式comboBox = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.取反图像comboBox = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 解码个数textBox
            // 
            this.解码个数textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.解码个数textBox.Location = new System.Drawing.Point(74, 68);
            this.解码个数textBox.Name = "解码个数textBox";
            this.解码个数textBox.Size = new System.Drawing.Size(168, 21);
            this.解码个数textBox.TabIndex = 109;
            // 
            // 解码超时textBox
            // 
            this.解码超时textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.解码超时textBox.Location = new System.Drawing.Point(74, 97);
            this.解码超时textBox.Name = "解码超时textBox";
            this.解码超时textBox.Size = new System.Drawing.Size(168, 21);
            this.解码超时textBox.TabIndex = 105;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 104;
            this.label3.Text = "解码超时:";
            // 
            // 条码类型comboBox
            // 
            this.条码类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.条码类型comboBox.FormattingEnabled = true;
            this.条码类型comboBox.Items.AddRange(new object[] {
            "auto",
            "2/5 Industrial",
            "2/5 Interleaved",
            "Codabar",
            "Code 128",
            "Code 39",
            "Code 93",
            "EAN-13 Add-On 2",
            "EAN-13 Add-On 5",
            "EAN-13",
            "EAN-8 Add-On 2",
            "EAN-8 Add-On 5",
            "EAN-8",
            "GS1 DataBar Expanded Stacked",
            "GS1 DataBar Expanded",
            "GS1 DataBar Limited",
            "GS1 DataBar Omnidir",
            "GS1 DataBar Stacked Omnidir",
            "GS1 DataBar Stacked",
            "GS1 DataBar Truncated",
            "GS1-128",
            "MSI",
            "PharmaCode",
            "UPC-A Add-On 2",
            "UPC-A Add-On 5",
            "UPC-A",
            "UPC-E Add-On 2",
            "UPC-E Add-On 5",
            "UPC-E"});
            this.条码类型comboBox.Location = new System.Drawing.Point(74, 12);
            this.条码类型comboBox.Name = "条码类型comboBox";
            this.条码类型comboBox.Size = new System.Drawing.Size(168, 20);
            this.条码类型comboBox.TabIndex = 103;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 15);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(59, 12);
            this.label23.TabIndex = 102;
            this.label23.Text = "条码类型:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(10, 73);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 12);
            this.label21.TabIndex = 100;
            this.label21.Text = "解码个数:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(247, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 12);
            this.label8.TabIndex = 114;
            this.label8.Text = "int";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(247, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 116;
            this.label10.Text = "string";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(247, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 117;
            this.label11.Text = "int(ms)";
            // 
            // 保存方式comboBox
            // 
            this.保存方式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.保存方式comboBox.FormattingEnabled = true;
            this.保存方式comboBox.Items.AddRange(new object[] {
            "0",
            "1"});
            this.保存方式comboBox.Location = new System.Drawing.Point(74, 126);
            this.保存方式comboBox.Name = "保存方式comboBox";
            this.保存方式comboBox.Size = new System.Drawing.Size(168, 20);
            this.保存方式comboBox.TabIndex = 121;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(247, 129);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(41, 12);
            this.label25.TabIndex = 120;
            this.label25.Text = "string";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(10, 130);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(59, 12);
            this.label26.TabIndex = 119;
            this.label26.Text = "保存方式:";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(247, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 131;
            this.label15.Text = "string";
            // 
            // 取反图像comboBox
            // 
            this.取反图像comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.取反图像comboBox.FormattingEnabled = true;
            this.取反图像comboBox.Items.AddRange(new object[] {
            "true",
            "false"});
            this.取反图像comboBox.Location = new System.Drawing.Point(74, 40);
            this.取反图像comboBox.Name = "取反图像comboBox";
            this.取反图像comboBox.Size = new System.Drawing.Size(168, 20);
            this.取反图像comboBox.TabIndex = 130;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 43);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 129;
            this.label16.Text = "取反图像:";
            // 
            // FindBarCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(299, 168);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.取反图像comboBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.保存方式comboBox);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.解码个数textBox);
            this.Controls.Add(this.解码超时textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.条码类型comboBox);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FindBarCodeForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FindBarCodeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox 解码个数textBox;
        private System.Windows.Forms.TextBox 解码超时textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 条码类型comboBox;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox 保存方式comboBox;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox 取反图像comboBox;
        private System.Windows.Forms.Label label16;
    }
}