namespace FunctionBlock
{
    partial class ZoomImageFactorForm
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
            this.插值comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.高度缩放ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.宽度缩放ComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 插值comboBox
            // 
            this.插值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.插值comboBox.FormattingEnabled = true;
            this.插值comboBox.Items.AddRange(new object[] {
            "0.3",
            "0.5",
            "0.7",
            "1.0",
            "1.5",
            "2.0",
            "3.0",
            "5.0"});
            this.插值comboBox.Location = new System.Drawing.Point(60, 62);
            this.插值comboBox.Name = "插值comboBox";
            this.插值comboBox.Size = new System.Drawing.Size(240, 20);
            this.插值comboBox.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "插直方法:";
            // 
            // 高度缩放ComboBox
            // 
            this.高度缩放ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.高度缩放ComboBox.FormattingEnabled = true;
            this.高度缩放ComboBox.Items.AddRange(new object[] {
            "512",
            "2048"});
            this.高度缩放ComboBox.Location = new System.Drawing.Point(60, 36);
            this.高度缩放ComboBox.Name = "高度缩放ComboBox";
            this.高度缩放ComboBox.Size = new System.Drawing.Size(240, 20);
            this.高度缩放ComboBox.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "高度缩放:";
            // 
            // 宽度缩放ComboBox
            // 
            this.宽度缩放ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.宽度缩放ComboBox.FormattingEnabled = true;
            this.宽度缩放ComboBox.Items.AddRange(new object[] {
            "512",
            "2048"});
            this.宽度缩放ComboBox.Location = new System.Drawing.Point(60, 10);
            this.宽度缩放ComboBox.Name = "宽度缩放ComboBox";
            this.宽度缩放ComboBox.Size = new System.Drawing.Size(240, 20);
            this.宽度缩放ComboBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "宽度缩放:";
            // 
            // ZoomImageFactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 116);
            this.Controls.Add(this.插值comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.高度缩放ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.宽度缩放ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ZoomImageFactorForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox 插值comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 高度缩放ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 宽度缩放ComboBox;
        private System.Windows.Forms.Label label1;
    }
}