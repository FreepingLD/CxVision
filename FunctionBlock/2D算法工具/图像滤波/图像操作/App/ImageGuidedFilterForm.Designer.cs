namespace FunctionBlock
{
    partial class ImageGuidedFilterForm
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
            this.半径ComboBox = new System.Windows.Forms.ComboBox();
            this.振幅ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.迭代次数comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "半径";
            // 
            // 半径ComboBox
            // 
            this.半径ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.半径ComboBox.FormattingEnabled = true;
            this.半径ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.半径ComboBox.Location = new System.Drawing.Point(58, 6);
            this.半径ComboBox.Name = "半径ComboBox";
            this.半径ComboBox.Size = new System.Drawing.Size(207, 20);
            this.半径ComboBox.TabIndex = 5;
            // 
            // 振幅ComboBox
            // 
            this.振幅ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.振幅ComboBox.FormattingEnabled = true;
            this.振幅ComboBox.Items.AddRange(new object[] {
            "3.0",
            "10.0",
            "20.0",
            "50.0",
            "100.0"});
            this.振幅ComboBox.Location = new System.Drawing.Point(58, 32);
            this.振幅ComboBox.Name = "振幅ComboBox";
            this.振幅ComboBox.Size = new System.Drawing.Size(207, 20);
            this.振幅ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "振幅";
            // 
            // 迭代次数comboBox
            // 
            this.迭代次数comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.迭代次数comboBox.FormattingEnabled = true;
            this.迭代次数comboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.迭代次数comboBox.Location = new System.Drawing.Point(58, 58);
            this.迭代次数comboBox.Name = "迭代次数comboBox";
            this.迭代次数comboBox.Size = new System.Drawing.Size(207, 20);
            this.迭代次数comboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "迭代次数";
            // 
            // ImageGuidedFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 98);
            this.Controls.Add(this.迭代次数comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.振幅ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.半径ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageGuidedFilterForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 半径ComboBox;
        private System.Windows.Forms.ComboBox 振幅ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 迭代次数comboBox;
        private System.Windows.Forms.Label label3;
    }
}