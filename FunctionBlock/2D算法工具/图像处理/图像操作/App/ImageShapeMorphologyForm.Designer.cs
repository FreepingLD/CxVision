namespace FunctionBlock
{
    partial class ImageShapeMorphologyForm
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
            this.高度ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.掩膜形状comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 9);
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
            this.宽度ComboBox.Location = new System.Drawing.Point(63, 6);
            this.宽度ComboBox.Name = "宽度ComboBox";
            this.宽度ComboBox.Size = new System.Drawing.Size(202, 20);
            this.宽度ComboBox.TabIndex = 5;
            // 
            // 高度ComboBox
            // 
            this.高度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.高度ComboBox.FormattingEnabled = true;
            this.高度ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.高度ComboBox.Location = new System.Drawing.Point(63, 32);
            this.高度ComboBox.Name = "高度ComboBox";
            this.高度ComboBox.Size = new System.Drawing.Size(202, 20);
            this.高度ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "高度";
            // 
            // 掩膜形状comboBox
            // 
            this.掩膜形状comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜形状comboBox.FormattingEnabled = true;
            this.掩膜形状comboBox.Items.AddRange(new object[] {
            "octagon",
            "rectangle",
            "rhombus"});
            this.掩膜形状comboBox.Location = new System.Drawing.Point(63, 58);
            this.掩膜形状comboBox.Name = "掩膜形状comboBox";
            this.掩膜形状comboBox.Size = new System.Drawing.Size(202, 20);
            this.掩膜形状comboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "掩膜形状";
            // 
            // ImageShapeMorphologyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 98);
            this.Controls.Add(this.掩膜形状comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.高度ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.宽度ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageShapeMorphologyForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 宽度ComboBox;
        private System.Windows.Forms.ComboBox 高度ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 掩膜形状comboBox;
        private System.Windows.Forms.Label label3;
    }
}