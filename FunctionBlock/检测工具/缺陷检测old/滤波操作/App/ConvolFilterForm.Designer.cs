namespace FunctionBlock
{
    partial class ConvolFilterForm
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
            this.滤波掩膜ComboBox = new System.Windows.Forms.ComboBox();
            this.边缘处理ComboBox = new System.Windows.Forms.ComboBox();
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
            this.label1.Text = "滤波掩膜";
            // 
            // 滤波掩膜ComboBox
            // 
            this.滤波掩膜ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.滤波掩膜ComboBox.FormattingEnabled = true;
            this.滤波掩膜ComboBox.Items.AddRange(new object[] {
            "sobel",
            "laplace4",
            "lowpas_3_3"});
            this.滤波掩膜ComboBox.Location = new System.Drawing.Point(58, 6);
            this.滤波掩膜ComboBox.Name = "滤波掩膜ComboBox";
            this.滤波掩膜ComboBox.Size = new System.Drawing.Size(207, 20);
            this.滤波掩膜ComboBox.TabIndex = 5;
            // 
            // 边缘处理ComboBox
            // 
            this.边缘处理ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.边缘处理ComboBox.FormattingEnabled = true;
            this.边缘处理ComboBox.Items.AddRange(new object[] {
            "mirrored",
            "cyclic",
            "continued"});
            this.边缘处理ComboBox.Location = new System.Drawing.Point(58, 32);
            this.边缘处理ComboBox.Name = "边缘处理ComboBox";
            this.边缘处理ComboBox.Size = new System.Drawing.Size(207, 20);
            this.边缘处理ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "边缘处理";
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
            // ConvolFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 89);
            this.Controls.Add(this.启用滤波checkBox);
            this.Controls.Add(this.边缘处理ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.滤波掩膜ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConvolFilterForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 滤波掩膜ComboBox;
        private System.Windows.Forms.ComboBox 边缘处理ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox 启用滤波checkBox;
    }

}