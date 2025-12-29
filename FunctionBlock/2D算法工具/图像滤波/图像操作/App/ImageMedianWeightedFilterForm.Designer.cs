namespace FunctionBlock
{
    partial class ImageMedianWeightedFilterForm
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
            this.掩膜类型ComboBox = new System.Windows.Forms.ComboBox();
            this.掩膜尺寸ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "掩膜类型";
            // 
            // 掩膜类型ComboBox
            // 
            this.掩膜类型ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜类型ComboBox.FormattingEnabled = true;
            this.掩膜类型ComboBox.Items.AddRange(new object[] {
            "gauss",
            "inner"});
            this.掩膜类型ComboBox.Location = new System.Drawing.Point(58, 6);
            this.掩膜类型ComboBox.Name = "掩膜类型ComboBox";
            this.掩膜类型ComboBox.Size = new System.Drawing.Size(207, 20);
            this.掩膜类型ComboBox.TabIndex = 5;
            // 
            // 掩膜尺寸ComboBox
            // 
            this.掩膜尺寸ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜尺寸ComboBox.FormattingEnabled = true;
            this.掩膜尺寸ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.掩膜尺寸ComboBox.Location = new System.Drawing.Point(58, 32);
            this.掩膜尺寸ComboBox.Name = "掩膜尺寸ComboBox";
            this.掩膜尺寸ComboBox.Size = new System.Drawing.Size(207, 20);
            this.掩膜尺寸ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "掩膜尺寸";
            // 
            // ImageMedianWeightedFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 98);
            this.Controls.Add(this.掩膜尺寸ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.掩膜类型ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageMedianWeightedFilterForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 掩膜类型ComboBox;
        private System.Windows.Forms.ComboBox 掩膜尺寸ComboBox;
        private System.Windows.Forms.Label label2;
    }
}