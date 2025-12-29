namespace FunctionBlock
{
    partial class ZoomImageSizeForm
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
            this.图像宽度ComboBox = new System.Windows.Forms.ComboBox();
            this.图像高度ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.插值方式comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图像宽度:";
            // 
            // 图像宽度ComboBox
            // 
            this.图像宽度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.图像宽度ComboBox.FormattingEnabled = true;
            this.图像宽度ComboBox.Items.AddRange(new object[] {
            "15",
            "31",
            "41",
            "51",
            "71",
            "101",
            "121",
            "151",
            "201"});
            this.图像宽度ComboBox.Location = new System.Drawing.Point(63, 6);
            this.图像宽度ComboBox.Name = "图像宽度ComboBox";
            this.图像宽度ComboBox.Size = new System.Drawing.Size(202, 20);
            this.图像宽度ComboBox.TabIndex = 5;
            // 
            // 图像高度ComboBox
            // 
            this.图像高度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.图像高度ComboBox.FormattingEnabled = true;
            this.图像高度ComboBox.Items.AddRange(new object[] {
            "15",
            "31",
            "41",
            "51",
            "71",
            "101",
            "121",
            "151",
            "201"});
            this.图像高度ComboBox.Location = new System.Drawing.Point(63, 32);
            this.图像高度ComboBox.Name = "图像高度ComboBox";
            this.图像高度ComboBox.Size = new System.Drawing.Size(202, 20);
            this.图像高度ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "图像高度:";
            // 
            // 插值方式comboBox
            // 
            this.插值方式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.插值方式comboBox.FormattingEnabled = true;
            this.插值方式comboBox.Items.AddRange(new object[] {
            "0.3",
            "0.5",
            "0.7",
            "1.0",
            "1.5",
            "2.0",
            "3.0",
            "5.0"});
            this.插值方式comboBox.Location = new System.Drawing.Point(63, 58);
            this.插值方式comboBox.Name = "插值方式comboBox";
            this.插值方式comboBox.Size = new System.Drawing.Size(202, 20);
            this.插值方式comboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "插值方式:";
            // 
            // ZoomImageSizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 98);
            this.Controls.Add(this.插值方式comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.图像高度ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.图像宽度ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ZoomImageSizeForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 图像宽度ComboBox;
        private System.Windows.Forms.ComboBox 图像高度ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 插值方式comboBox;
        private System.Windows.Forms.Label label3;
    }
}