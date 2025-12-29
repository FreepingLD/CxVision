namespace FunctionBlock
{
    partial class DynThresholdDetectForm
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
            this.极性comboBox = new System.Windows.Forms.ComboBox();
            this.掩膜宽度comboBox = new System.Windows.Forms.ComboBox();
            this.掩膜高度comboBox = new System.Windows.Forms.ComboBox();
            this.偏置comboBox = new System.Windows.Forms.ComboBox();
            this.启用检测checkBox = new System.Windows.Forms.CheckBox();
            this.最小面积comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "掩膜宽度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "掩膜高度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "灰度值偏置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "亮/暗";
            // 
            // 极性comboBox
            // 
            this.极性comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.极性comboBox.FormattingEnabled = true;
            this.极性comboBox.Items.AddRange(new object[] {
            "dark",
            "equal",
            "light",
            "not_equal"});
            this.极性comboBox.Location = new System.Drawing.Point(78, 77);
            this.极性comboBox.Name = "极性comboBox";
            this.极性comboBox.Size = new System.Drawing.Size(236, 20);
            this.极性comboBox.TabIndex = 11;
            // 
            // 掩膜宽度comboBox
            // 
            this.掩膜宽度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜宽度comboBox.FormattingEnabled = true;
            this.掩膜宽度comboBox.Items.AddRange(new object[] {
            "5",
            "7",
            "9",
            "11"});
            this.掩膜宽度comboBox.Location = new System.Drawing.Point(78, 4);
            this.掩膜宽度comboBox.Name = "掩膜宽度comboBox";
            this.掩膜宽度comboBox.Size = new System.Drawing.Size(236, 20);
            this.掩膜宽度comboBox.TabIndex = 12;
            // 
            // 掩膜高度comboBox
            // 
            this.掩膜高度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜高度comboBox.FormattingEnabled = true;
            this.掩膜高度comboBox.Items.AddRange(new object[] {
            "5",
            "7",
            "9",
            "11"});
            this.掩膜高度comboBox.Location = new System.Drawing.Point(78, 28);
            this.掩膜高度comboBox.Name = "掩膜高度comboBox";
            this.掩膜高度comboBox.Size = new System.Drawing.Size(236, 20);
            this.掩膜高度comboBox.TabIndex = 13;
            // 
            // 偏置comboBox
            // 
            this.偏置comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.偏置comboBox.FormattingEnabled = true;
            this.偏置comboBox.Items.AddRange(new object[] {
            "1.0",
            "3.0",
            "5.0",
            "7.0",
            "10.0",
            "20.0",
            "30.0"});
            this.偏置comboBox.Location = new System.Drawing.Point(78, 52);
            this.偏置comboBox.Name = "偏置comboBox";
            this.偏置comboBox.Size = new System.Drawing.Size(236, 20);
            this.偏置comboBox.TabIndex = 14;
            // 
            // 启用检测checkBox
            // 
            this.启用检测checkBox.AutoSize = true;
            this.启用检测checkBox.Location = new System.Drawing.Point(78, 129);
            this.启用检测checkBox.Name = "启用检测checkBox";
            this.启用检测checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用检测checkBox.TabIndex = 15;
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
            this.最小面积comboBox.Location = new System.Drawing.Point(78, 103);
            this.最小面积comboBox.Name = "最小面积comboBox";
            this.最小面积comboBox.Size = new System.Drawing.Size(236, 20);
            this.最小面积comboBox.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "最小面积";
            // 
            // DynThresholdDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 162);
            this.Controls.Add(this.最小面积comboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.启用检测checkBox);
            this.Controls.Add(this.偏置comboBox);
            this.Controls.Add(this.掩膜高度comboBox);
            this.Controls.Add(this.掩膜宽度comboBox);
            this.Controls.Add(this.极性comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DynThresholdDetectForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 极性comboBox;
        private System.Windows.Forms.ComboBox 掩膜宽度comboBox;
        private System.Windows.Forms.ComboBox 掩膜高度comboBox;
        private System.Windows.Forms.ComboBox 偏置comboBox;
        private System.Windows.Forms.CheckBox 启用检测checkBox;
        private System.Windows.Forms.ComboBox 最小面积comboBox;
        private System.Windows.Forms.Label label5;
    }
}