namespace FunctionBlock
{
    partial class EmphasizeForm
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
            this.因子comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.掩膜高度ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.掩膜宽度ComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 因子comboBox
            // 
            this.因子comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.因子comboBox.FormattingEnabled = true;
            this.因子comboBox.Items.AddRange(new object[] {
            "0.3",
            "0.5",
            "0.7",
            "1.0",
            "1.5",
            "2.0",
            "3.0",
            "5.0"});
            this.因子comboBox.Location = new System.Drawing.Point(78, 77);
            this.因子comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.因子comboBox.Name = "因子comboBox";
            this.因子comboBox.Size = new System.Drawing.Size(275, 23);
            this.因子comboBox.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "因子";
            // 
            // 掩膜高度ComboBox
            // 
            this.掩膜高度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜高度ComboBox.FormattingEnabled = true;
            this.掩膜高度ComboBox.Items.AddRange(new object[] {
            "15",
            "31",
            "41",
            "51",
            "71",
            "101",
            "121",
            "151",
            "201"});
            this.掩膜高度ComboBox.Location = new System.Drawing.Point(78, 45);
            this.掩膜高度ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.掩膜高度ComboBox.Name = "掩膜高度ComboBox";
            this.掩膜高度ComboBox.Size = new System.Drawing.Size(275, 23);
            this.掩膜高度ComboBox.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "掩膜高度";
            // 
            // 掩膜宽度ComboBox
            // 
            this.掩膜宽度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜宽度ComboBox.FormattingEnabled = true;
            this.掩膜宽度ComboBox.Items.AddRange(new object[] {
            "15",
            "31",
            "41",
            "51",
            "71",
            "101",
            "121",
            "151",
            "201"});
            this.掩膜宽度ComboBox.Location = new System.Drawing.Point(78, 13);
            this.掩膜宽度ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.掩膜宽度ComboBox.Name = "掩膜宽度ComboBox";
            this.掩膜宽度ComboBox.Size = new System.Drawing.Size(275, 23);
            this.掩膜宽度ComboBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "掩膜宽度";
            // 
            // EmphasizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 115);
            this.Controls.Add(this.因子comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.掩膜高度ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.掩膜宽度ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EmphasizeForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox 因子comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 掩膜高度ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 掩膜宽度ComboBox;
        private System.Windows.Forms.Label label1;
    }
}