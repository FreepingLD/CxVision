namespace FunctionBlock
{
    partial class HysteresisThresholdForm
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
            this.最大长度comboBox = new System.Windows.Forms.ComboBox();
            this.高阈值comboBox = new System.Windows.Forms.ComboBox();
            this.低阈值comboBox = new System.Windows.Forms.ComboBox();
            this.区域连通comboBox = new System.Windows.Forms.ComboBox();
            this.区域填充comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "低阈值";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "高阈值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "最大长度";
            // 
            // 最大长度comboBox
            // 
            this.最大长度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最大长度comboBox.FormattingEnabled = true;
            this.最大长度comboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "5",
            "7",
            "10",
            "12",
            "14",
            "17",
            "20",
            "25",
            "30",
            "35",
            "40",
            "50"});
            this.最大长度comboBox.Location = new System.Drawing.Point(64, 59);
            this.最大长度comboBox.Name = "最大长度comboBox";
            this.最大长度comboBox.Size = new System.Drawing.Size(247, 20);
            this.最大长度comboBox.TabIndex = 20;
            // 
            // 高阈值comboBox
            // 
            this.高阈值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.高阈值comboBox.FormattingEnabled = true;
            this.高阈值comboBox.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.高阈值comboBox.Location = new System.Drawing.Point(64, 31);
            this.高阈值comboBox.Name = "高阈值comboBox";
            this.高阈值comboBox.Size = new System.Drawing.Size(247, 20);
            this.高阈值comboBox.TabIndex = 19;
            // 
            // 低阈值comboBox
            // 
            this.低阈值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.低阈值comboBox.FormattingEnabled = true;
            this.低阈值comboBox.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.低阈值comboBox.Location = new System.Drawing.Point(64, 4);
            this.低阈值comboBox.Name = "低阈值comboBox";
            this.低阈值comboBox.Size = new System.Drawing.Size(247, 20);
            this.低阈值comboBox.TabIndex = 18;
            // 
            // 区域连通comboBox
            // 
            this.区域连通comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域连通comboBox.FormattingEnabled = true;
            this.区域连通comboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.区域连通comboBox.Location = new System.Drawing.Point(64, 111);
            this.区域连通comboBox.Name = "区域连通comboBox";
            this.区域连通comboBox.Size = new System.Drawing.Size(247, 20);
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
            this.区域填充comboBox.Location = new System.Drawing.Point(64, 85);
            this.区域填充comboBox.Name = "区域填充comboBox";
            this.区域填充comboBox.Size = new System.Drawing.Size(247, 20);
            this.区域填充comboBox.TabIndex = 34;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 33;
            this.label4.Text = "区域连通";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 32;
            this.label5.Text = "区域填充";
            // 
            // HysteresisThresholdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 161);
            this.Controls.Add(this.区域连通comboBox);
            this.Controls.Add(this.区域填充comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.最大长度comboBox);
            this.Controls.Add(this.高阈值comboBox);
            this.Controls.Add(this.低阈值comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HysteresisThresholdForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 最大长度comboBox;
        private System.Windows.Forms.ComboBox 高阈值comboBox;
        private System.Windows.Forms.ComboBox 低阈值comboBox;
        private System.Windows.Forms.ComboBox 区域连通comboBox;
        private System.Windows.Forms.ComboBox 区域填充comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}