namespace FunctionBlock
{
    partial class VarThresholdForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.掩膜宽度comboBox = new System.Windows.Forms.ComboBox();
            this.掩膜高度comboBox = new System.Windows.Forms.ComboBox();
            this.标准差因子comboBox = new System.Windows.Forms.ComboBox();
            this.灰度值差comboBox = new System.Windows.Forms.ComboBox();
            this.区域连通comboBox = new System.Windows.Forms.ComboBox();
            this.区域填充comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "掩膜宽度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "掩膜高度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "标准差因子";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 107);
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
            this.极性comboBox.Location = new System.Drawing.Point(70, 105);
            this.极性comboBox.Name = "极性comboBox";
            this.极性comboBox.Size = new System.Drawing.Size(244, 20);
            this.极性comboBox.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "灰度值差";
            // 
            // 掩膜宽度comboBox
            // 
            this.掩膜宽度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜宽度comboBox.FormattingEnabled = true;
            this.掩膜宽度comboBox.Items.AddRange(new object[] {
            "9",
            "11",
            "13",
            "15"});
            this.掩膜宽度comboBox.Location = new System.Drawing.Point(70, 7);
            this.掩膜宽度comboBox.Name = "掩膜宽度comboBox";
            this.掩膜宽度comboBox.Size = new System.Drawing.Size(244, 20);
            this.掩膜宽度comboBox.TabIndex = 13;
            // 
            // 掩膜高度comboBox
            // 
            this.掩膜高度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.掩膜高度comboBox.FormattingEnabled = true;
            this.掩膜高度comboBox.Items.AddRange(new object[] {
            "9",
            "11",
            "13",
            "15"});
            this.掩膜高度comboBox.Location = new System.Drawing.Point(70, 31);
            this.掩膜高度comboBox.Name = "掩膜高度comboBox";
            this.掩膜高度comboBox.Size = new System.Drawing.Size(244, 20);
            this.掩膜高度comboBox.TabIndex = 14;
            // 
            // 标准差因子comboBox
            // 
            this.标准差因子comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.标准差因子comboBox.FormattingEnabled = true;
            this.标准差因子comboBox.Items.AddRange(new object[] {
            "-0.2",
            "-0.1",
            "0.1",
            "0.2"});
            this.标准差因子comboBox.Location = new System.Drawing.Point(70, 56);
            this.标准差因子comboBox.Name = "标准差因子comboBox";
            this.标准差因子comboBox.Size = new System.Drawing.Size(244, 20);
            this.标准差因子comboBox.TabIndex = 15;
            // 
            // 灰度值差comboBox
            // 
            this.灰度值差comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.灰度值差comboBox.FormattingEnabled = true;
            this.灰度值差comboBox.Items.AddRange(new object[] {
            "-2",
            "-1",
            "0",
            "1, 2"});
            this.灰度值差comboBox.Location = new System.Drawing.Point(70, 80);
            this.灰度值差comboBox.Name = "灰度值差comboBox";
            this.灰度值差comboBox.Size = new System.Drawing.Size(244, 20);
            this.灰度值差comboBox.TabIndex = 16;
            // 
            // 区域连通comboBox
            // 
            this.区域连通comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域连通comboBox.FormattingEnabled = true;
            this.区域连通comboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.区域连通comboBox.Location = new System.Drawing.Point(70, 157);
            this.区域连通comboBox.Name = "区域连通comboBox";
            this.区域连通comboBox.Size = new System.Drawing.Size(244, 20);
            this.区域连通comboBox.TabIndex = 35;
            // 
            // 区域填充comboBox
            // 
            this.区域填充comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域填充comboBox.FormattingEnabled = true;
            this.区域填充comboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.区域填充comboBox.Location = new System.Drawing.Point(70, 131);
            this.区域填充comboBox.Name = "区域填充comboBox";
            this.区域填充comboBox.Size = new System.Drawing.Size(244, 20);
            this.区域填充comboBox.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 33;
            this.label6.Text = "区域连通";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 32;
            this.label7.Text = "区域填充";
            // 
            // VarThresholdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 200);
            this.Controls.Add(this.区域连通comboBox);
            this.Controls.Add(this.区域填充comboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.灰度值差comboBox);
            this.Controls.Add(this.标准差因子comboBox);
            this.Controls.Add(this.掩膜高度comboBox);
            this.Controls.Add(this.掩膜宽度comboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.极性comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VarThresholdForm";
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 掩膜宽度comboBox;
        private System.Windows.Forms.ComboBox 掩膜高度comboBox;
        private System.Windows.Forms.ComboBox 标准差因子comboBox;
        private System.Windows.Forms.ComboBox 灰度值差comboBox;
        private System.Windows.Forms.ComboBox 区域连通comboBox;
        private System.Windows.Forms.ComboBox 区域填充comboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}