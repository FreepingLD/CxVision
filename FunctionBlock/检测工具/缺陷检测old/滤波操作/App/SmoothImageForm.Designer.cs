namespace FunctionBlock
{
    partial class SmoothImageForm
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
            this.滤波类型ComboBox = new System.Windows.Forms.ComboBox();
            this.透明度ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.启用滤波checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "滤波类型";
            // 
            // 滤波类型ComboBox
            // 
            this.滤波类型ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.滤波类型ComboBox.FormattingEnabled = true;
            this.滤波类型ComboBox.Items.AddRange(new object[] {
            "deriche1",
            "deriche2",
            "gauss",
            "shen"});
            this.滤波类型ComboBox.Location = new System.Drawing.Point(58, 6);
            this.滤波类型ComboBox.Name = "滤波类型ComboBox";
            this.滤波类型ComboBox.Size = new System.Drawing.Size(207, 20);
            this.滤波类型ComboBox.TabIndex = 5;
            // 
            // 透明度ComboBox
            // 
            this.透明度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.透明度ComboBox.FormattingEnabled = true;
            this.透明度ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.透明度ComboBox.Location = new System.Drawing.Point(58, 32);
            this.透明度ComboBox.Name = "透明度ComboBox";
            this.透明度ComboBox.Size = new System.Drawing.Size(207, 20);
            this.透明度ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "透明度";
            // 
            // 启用滤波checkBox
            // 
            this.启用滤波checkBox.AutoSize = true;
            this.启用滤波checkBox.Location = new System.Drawing.Point(58, 58);
            this.启用滤波checkBox.Name = "启用滤波checkBox";
            this.启用滤波checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用滤波checkBox.TabIndex = 13;
            this.启用滤波checkBox.Text = "启用滤波";
            this.启用滤波checkBox.UseVisualStyleBackColor = true;
            // 
            // SmoothImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 89);
            this.Controls.Add(this.启用滤波checkBox);
            this.Controls.Add(this.透明度ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.滤波类型ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SmoothImageForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 滤波类型ComboBox;
        private System.Windows.Forms.ComboBox 透明度ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox 启用滤波checkBox;
    }

}