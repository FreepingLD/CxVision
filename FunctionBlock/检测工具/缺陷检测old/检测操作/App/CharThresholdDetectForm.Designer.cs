namespace FunctionBlock
{
    partial class CharThresholdDetectForm
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
            this.百分比comboBox = new System.Windows.Forms.ComboBox();
            this.SigmaComboBox = new System.Windows.Forms.ComboBox();
            this.启用检测checkBox = new System.Windows.Forms.CheckBox();
            this.最小面积comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "sigma";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "百分比";
            // 
            // 百分比comboBox
            // 
            this.百分比comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.百分比comboBox.FormattingEnabled = true;
            this.百分比comboBox.Items.AddRange(new object[] {
            "90",
            "92",
            "95",
            "96",
            "97",
            "98",
            "99",
            "99.5",
            "100"});
            this.百分比comboBox.Location = new System.Drawing.Point(59, 34);
            this.百分比comboBox.Name = "百分比comboBox";
            this.百分比comboBox.Size = new System.Drawing.Size(238, 20);
            this.百分比comboBox.TabIndex = 8;
            // 
            // SigmaComboBox
            // 
            this.SigmaComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SigmaComboBox.FormattingEnabled = true;
            this.SigmaComboBox.Items.AddRange(new object[] {
            "0.0",
            "0.5",
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0"});
            this.SigmaComboBox.Location = new System.Drawing.Point(59, 6);
            this.SigmaComboBox.Name = "SigmaComboBox";
            this.SigmaComboBox.Size = new System.Drawing.Size(238, 20);
            this.SigmaComboBox.TabIndex = 7;
            // 
            // 启用检测checkBox
            // 
            this.启用检测checkBox.AutoSize = true;
            this.启用检测checkBox.Location = new System.Drawing.Point(59, 86);
            this.启用检测checkBox.Name = "启用检测checkBox";
            this.启用检测checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用检测checkBox.TabIndex = 9;
            this.启用检测checkBox.Text = "启用检测";
            this.启用检测checkBox.UseVisualStyleBackColor = true;
            // 
            // 最小面积comboBox
            // 
            this.最小面积comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小面积comboBox.FormattingEnabled = true;
            this.最小面积comboBox.Items.AddRange(new object[] {
            "0",
            "10",
            "50",
            "100",
            "150",
            "200"});
            this.最小面积comboBox.Location = new System.Drawing.Point(59, 60);
            this.最小面积comboBox.Name = "最小面积comboBox";
            this.最小面积comboBox.Size = new System.Drawing.Size(238, 20);
            this.最小面积comboBox.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "最小面积";
            // 
            // CharThresholdDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 120);
            this.Controls.Add(this.最小面积comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.启用检测checkBox);
            this.Controls.Add(this.百分比comboBox);
            this.Controls.Add(this.SigmaComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CharThresholdDetectForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 百分比comboBox;
        private System.Windows.Forms.ComboBox SigmaComboBox;
        private System.Windows.Forms.CheckBox 启用检测checkBox;
        private System.Windows.Forms.ComboBox 最小面积comboBox;
        private System.Windows.Forms.Label label3;
    }
}