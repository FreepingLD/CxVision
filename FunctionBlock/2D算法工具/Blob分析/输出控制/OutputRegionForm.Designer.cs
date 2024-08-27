namespace FunctionBlock
{
    partial class OutputRegionForm
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
            this.spilitLine1 = new userControl.SpilitLine();
            this.输出区域checkBox = new System.Windows.Forms.CheckBox();
            this.输出二值化图checkBox = new System.Windows.Forms.CheckBox();
            this.输出二值化均图checkBox = new System.Windows.Forms.CheckBox();
            this.模式comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spilitLine1
            // 
            this.spilitLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spilitLine1.Content = "参数";
            this.spilitLine1.Location = new System.Drawing.Point(6, 1);
            this.spilitLine1.Name = "spilitLine1";
            this.spilitLine1.Size = new System.Drawing.Size(377, 20);
            this.spilitLine1.TabIndex = 10;
            // 
            // 输出区域checkBox
            // 
            this.输出区域checkBox.AutoSize = true;
            this.输出区域checkBox.Location = new System.Drawing.Point(43, 27);
            this.输出区域checkBox.Name = "输出区域checkBox";
            this.输出区域checkBox.Size = new System.Drawing.Size(72, 16);
            this.输出区域checkBox.TabIndex = 11;
            this.输出区域checkBox.Text = "输出区域";
            this.输出区域checkBox.UseVisualStyleBackColor = true;
            // 
            // 输出二值化图checkBox
            // 
            this.输出二值化图checkBox.AutoSize = true;
            this.输出二值化图checkBox.Location = new System.Drawing.Point(43, 53);
            this.输出二值化图checkBox.Name = "输出二值化图checkBox";
            this.输出二值化图checkBox.Size = new System.Drawing.Size(96, 16);
            this.输出二值化图checkBox.TabIndex = 12;
            this.输出二值化图checkBox.Text = "输出二值化图";
            this.输出二值化图checkBox.UseVisualStyleBackColor = true;
            // 
            // 输出二值化均图checkBox
            // 
            this.输出二值化均图checkBox.AutoSize = true;
            this.输出二值化均图checkBox.Location = new System.Drawing.Point(43, 79);
            this.输出二值化均图checkBox.Name = "输出二值化均图checkBox";
            this.输出二值化均图checkBox.Size = new System.Drawing.Size(108, 16);
            this.输出二值化均图checkBox.TabIndex = 13;
            this.输出二值化均图checkBox.Text = "输出二值化均图";
            this.输出二值化均图checkBox.UseVisualStyleBackColor = true;
            // 
            // 模式comboBox
            // 
            this.模式comboBox.FormattingEnabled = true;
            this.模式comboBox.Items.AddRange(new object[] {
            "margin",
            "fill"});
            this.模式comboBox.Location = new System.Drawing.Point(43, 102);
            this.模式comboBox.Name = "模式comboBox";
            this.模式comboBox.Size = new System.Drawing.Size(121, 20);
            this.模式comboBox.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "模式";
            // 
            // OutputRegionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 174);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.模式comboBox);
            this.Controls.Add(this.输出二值化均图checkBox);
            this.Controls.Add(this.输出二值化图checkBox);
            this.Controls.Add(this.输出区域checkBox);
            this.Controls.Add(this.spilitLine1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OutputRegionForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private userControl.SpilitLine spilitLine1;
        private System.Windows.Forms.CheckBox 输出区域checkBox;
        private System.Windows.Forms.CheckBox 输出二值化图checkBox;
        private System.Windows.Forms.CheckBox 输出二值化均图checkBox;
        private System.Windows.Forms.ComboBox 模式comboBox;
        private System.Windows.Forms.Label label1;
    }
}