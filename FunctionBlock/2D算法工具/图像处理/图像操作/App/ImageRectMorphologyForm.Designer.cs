namespace FunctionBlock
{
    partial class ImageRectMorphologyForm
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
            this.宽度ComboBox = new System.Windows.Forms.ComboBox();
            this.高度comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "宽度";
            // 
            // 宽度ComboBox
            // 
            this.宽度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.宽度ComboBox.FormattingEnabled = true;
            this.宽度ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.宽度ComboBox.Location = new System.Drawing.Point(45, 6);
            this.宽度ComboBox.Name = "宽度ComboBox";
            this.宽度ComboBox.Size = new System.Drawing.Size(221, 20);
            this.宽度ComboBox.TabIndex = 5;
            // 
            // 高度comboBox
            // 
            this.高度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.高度comboBox.FormattingEnabled = true;
            this.高度comboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.高度comboBox.Location = new System.Drawing.Point(45, 33);
            this.高度comboBox.Name = "高度comboBox";
            this.高度comboBox.Size = new System.Drawing.Size(221, 20);
            this.高度comboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "高度";
            // 
            // ImageRectMorphologyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 80);
            this.Controls.Add(this.高度comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.宽度ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageRectMorphologyForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 宽度ComboBox;
        private System.Windows.Forms.ComboBox 高度comboBox;
        private System.Windows.Forms.Label label2;
    }
}