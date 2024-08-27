namespace FunctionBlock
{
    partial class ImageBilateralFilterForm
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
            this.高斯大小ComboBox = new System.Windows.Forms.ComboBox();
            this.高斯范围comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.迭代次数comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.参数名称comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.参数值comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sigma尺寸";
            // 
            // 高斯大小ComboBox
            // 
            this.高斯大小ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.高斯大小ComboBox.FormattingEnabled = true;
            this.高斯大小ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.高斯大小ComboBox.Location = new System.Drawing.Point(64, 6);
            this.高斯大小ComboBox.Name = "高斯大小ComboBox";
            this.高斯大小ComboBox.Size = new System.Drawing.Size(202, 20);
            this.高斯大小ComboBox.TabIndex = 5;
            // 
            // 高斯范围comboBox
            // 
            this.高斯范围comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.高斯范围comboBox.FormattingEnabled = true;
            this.高斯范围comboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.高斯范围comboBox.Location = new System.Drawing.Point(64, 33);
            this.高斯范围comboBox.Name = "高斯范围comboBox";
            this.高斯范围comboBox.Size = new System.Drawing.Size(202, 20);
            this.高斯范围comboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sigma范围";
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
            this.迭代次数comboBox.Location = new System.Drawing.Point(64, 59);
            this.迭代次数comboBox.Name = "迭代次数comboBox";
            this.迭代次数comboBox.Size = new System.Drawing.Size(202, 20);
            this.迭代次数comboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "迭代次数";
            // 
            // 参数名称comboBox
            // 
            this.参数名称comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.参数名称comboBox.FormattingEnabled = true;
            this.参数名称comboBox.Items.AddRange(new object[] {
            "sampling_method",
            "sampling_ratio"});
            this.参数名称comboBox.Location = new System.Drawing.Point(64, 85);
            this.参数名称comboBox.Name = "参数名称comboBox";
            this.参数名称comboBox.Size = new System.Drawing.Size(202, 20);
            this.参数名称comboBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "参数名称";
            // 
            // 参数值comboBox
            // 
            this.参数值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.参数值comboBox.FormattingEnabled = true;
            this.参数值comboBox.Items.AddRange(new object[] {
            "grid",
            "poisson_disk",
            "exact",
            "0.5",
            "0.25",
            "0.75",
            "1.0"});
            this.参数值comboBox.Location = new System.Drawing.Point(64, 111);
            this.参数值comboBox.Name = "参数值comboBox";
            this.参数值comboBox.Size = new System.Drawing.Size(202, 20);
            this.参数值comboBox.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "参数值";
            // 
            // ImageBilateralFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 217);
            this.Controls.Add(this.参数值comboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.参数名称comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.迭代次数comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.高斯范围comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.高斯大小ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageBilateralFilterForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 高斯大小ComboBox;
        private System.Windows.Forms.ComboBox 高斯范围comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 迭代次数comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 参数名称comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 参数值comboBox;
        private System.Windows.Forms.Label label5;
    }
}