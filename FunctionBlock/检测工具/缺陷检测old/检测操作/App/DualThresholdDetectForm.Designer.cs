namespace FunctionBlock
{
    partial class DualThresholdDetectForm
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
            this.最小灰度值comboBox = new System.Windows.Forms.ComboBox();
            this.最小尺寸ComboBox = new System.Windows.Forms.ComboBox();
            this.阈值comboBox = new System.Windows.Forms.ComboBox();
            this.启用检测checkBox = new System.Windows.Forms.CheckBox();
            this.最小面积comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "最小尺寸";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "最小灰度值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "阈值";
            // 
            // 最小灰度值comboBox
            // 
            this.最小灰度值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小灰度值comboBox.FormattingEnabled = true;
            this.最小灰度值comboBox.Items.AddRange(new object[] {
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0",
            "6.0",
            "7.0",
            "9.0",
            "11.0",
            "15.0",
            "20.0"});
            this.最小灰度值comboBox.Location = new System.Drawing.Point(71, 31);
            this.最小灰度值comboBox.Name = "最小灰度值comboBox";
            this.最小灰度值comboBox.Size = new System.Drawing.Size(243, 20);
            this.最小灰度值comboBox.TabIndex = 10;
            // 
            // 最小尺寸ComboBox
            // 
            this.最小尺寸ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小尺寸ComboBox.FormattingEnabled = true;
            this.最小尺寸ComboBox.Items.AddRange(new object[] {
            "0",
            "10",
            "20",
            "50",
            "100",
            "200",
            "500",
            "1000"});
            this.最小尺寸ComboBox.Location = new System.Drawing.Point(71, 3);
            this.最小尺寸ComboBox.Name = "最小尺寸ComboBox";
            this.最小尺寸ComboBox.Size = new System.Drawing.Size(243, 20);
            this.最小尺寸ComboBox.TabIndex = 9;
            // 
            // 阈值comboBox
            // 
            this.阈值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.阈值comboBox.FormattingEnabled = true;
            this.阈值comboBox.Items.AddRange(new object[] {
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0",
            "6.0",
            "7.0",
            "9.0",
            "11.0",
            "15.0",
            "20.0"});
            this.阈值comboBox.Location = new System.Drawing.Point(71, 58);
            this.阈值comboBox.Name = "阈值comboBox";
            this.阈值comboBox.Size = new System.Drawing.Size(243, 20);
            this.阈值comboBox.TabIndex = 11;
            // 
            // 启用检测checkBox
            // 
            this.启用检测checkBox.AutoSize = true;
            this.启用检测checkBox.Location = new System.Drawing.Point(71, 111);
            this.启用检测checkBox.Name = "启用检测checkBox";
            this.启用检测checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用检测checkBox.TabIndex = 12;
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
            this.最小面积comboBox.Location = new System.Drawing.Point(71, 85);
            this.最小面积comboBox.Name = "最小面积comboBox";
            this.最小面积comboBox.Size = new System.Drawing.Size(243, 20);
            this.最小面积comboBox.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "最小面积";
            // 
            // DualThresholdDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 139);
            this.Controls.Add(this.最小面积comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.启用检测checkBox);
            this.Controls.Add(this.阈值comboBox);
            this.Controls.Add(this.最小灰度值comboBox);
            this.Controls.Add(this.最小尺寸ComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DualThresholdDetectForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DualThreshold_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 最小灰度值comboBox;
        private System.Windows.Forms.ComboBox 最小尺寸ComboBox;
        private System.Windows.Forms.ComboBox 阈值comboBox;
        private System.Windows.Forms.CheckBox 启用检测checkBox;
        private System.Windows.Forms.ComboBox 最小面积comboBox;
        private System.Windows.Forms.Label label4;
    }
}