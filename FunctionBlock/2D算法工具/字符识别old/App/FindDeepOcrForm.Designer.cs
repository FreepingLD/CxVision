namespace FunctionBlock
{
    partial class FindDeepOcrForm
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
            this.最小得分textBox = new System.Windows.Forms.TextBox();
            this.匹配个数textBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.匹配模式comboBox = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.更多参数button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 最小得分textBox
            // 
            this.最小得分textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小得分textBox.Location = new System.Drawing.Point(64, 7);
            this.最小得分textBox.Name = "最小得分textBox";
            this.最小得分textBox.Size = new System.Drawing.Size(294, 21);
            this.最小得分textBox.TabIndex = 110;
            // 
            // 匹配个数textBox
            // 
            this.匹配个数textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.匹配个数textBox.Location = new System.Drawing.Point(64, 32);
            this.匹配个数textBox.Name = "匹配个数textBox";
            this.匹配个数textBox.Size = new System.Drawing.Size(294, 21);
            this.匹配个数textBox.TabIndex = 109;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(3, 36);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 12);
            this.label21.TabIndex = 100;
            this.label21.Text = "匹配个数:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 11);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(59, 12);
            this.label22.TabIndex = 99;
            this.label22.Text = "最小得分:";
            // 
            // 匹配模式comboBox
            // 
            this.匹配模式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.匹配模式comboBox.FormattingEnabled = true;
            this.匹配模式comboBox.Location = new System.Drawing.Point(64, 59);
            this.匹配模式comboBox.Name = "匹配模式comboBox";
            this.匹配模式comboBox.Size = new System.Drawing.Size(294, 20);
            this.匹配模式comboBox.TabIndex = 133;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 63);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 132;
            this.label16.Text = "匹配模式:";
            // 
            // 更多参数button
            // 
            this.更多参数button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.更多参数button.Location = new System.Drawing.Point(294, 85);
            this.更多参数button.Name = "更多参数button";
            this.更多参数button.Size = new System.Drawing.Size(64, 20);
            this.更多参数button.TabIndex = 134;
            this.更多参数button.Text = "更多参数";
            this.更多参数button.UseVisualStyleBackColor = true;
            this.更多参数button.Click += new System.EventHandler(this.更多参数button_Click);
            // 
            // FindDeepOcrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(370, 115);
            this.Controls.Add(this.更多参数button);
            this.Controls.Add(this.匹配模式comboBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.最小得分textBox);
            this.Controls.Add(this.匹配个数textBox);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label22);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FindDeepOcrForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox 最小得分textBox;
        private System.Windows.Forms.TextBox 匹配个数textBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox 匹配模式comboBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button 更多参数button;
    }
}