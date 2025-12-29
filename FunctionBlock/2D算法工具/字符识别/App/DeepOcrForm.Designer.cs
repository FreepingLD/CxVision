namespace FunctionBlock
{
    partial class DeepOcrForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.模型comboBox = new System.Windows.Forms.ComboBox();
            this.字符区域宽度comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.取反图像checkBox = new System.Windows.Forms.CheckBox();
            this.创建模型Btn = new System.Windows.Forms.Button();
            this.字符提取button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 64;
            this.label2.Text = "模型:";
            // 
            // 模型comboBox
            // 
            this.模型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.模型comboBox.FormattingEnabled = true;
            this.模型comboBox.Location = new System.Drawing.Point(94, 12);
            this.模型comboBox.Name = "模型comboBox";
            this.模型comboBox.Size = new System.Drawing.Size(263, 20);
            this.模型comboBox.TabIndex = 65;
            // 
            // 字符区域宽度comboBox
            // 
            this.字符区域宽度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.字符区域宽度comboBox.FormattingEnabled = true;
            this.字符区域宽度comboBox.Location = new System.Drawing.Point(94, 38);
            this.字符区域宽度comboBox.Name = "字符区域宽度comboBox";
            this.字符区域宽度comboBox.Size = new System.Drawing.Size(263, 20);
            this.字符区域宽度comboBox.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "字符区域宽度:";
            // 
            // 取反图像checkBox
            // 
            this.取反图像checkBox.AutoSize = true;
            this.取反图像checkBox.Location = new System.Drawing.Point(94, 64);
            this.取反图像checkBox.Name = "取反图像checkBox";
            this.取反图像checkBox.Size = new System.Drawing.Size(72, 16);
            this.取反图像checkBox.TabIndex = 71;
            this.取反图像checkBox.Text = "取反图像";
            this.取反图像checkBox.UseVisualStyleBackColor = true;
            // 
            // 创建模型Btn
            // 
            this.创建模型Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.创建模型Btn.Location = new System.Drawing.Point(267, 160);
            this.创建模型Btn.Name = "创建模型Btn";
            this.创建模型Btn.Size = new System.Drawing.Size(90, 37);
            this.创建模型Btn.TabIndex = 72;
            this.创建模型Btn.Text = "创建OCR模型";
            this.创建模型Btn.UseVisualStyleBackColor = true;
            this.创建模型Btn.Click += new System.EventHandler(this.创建模型Btn_Click);
            // 
            // 字符提取button
            // 
            this.字符提取button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.字符提取button.Location = new System.Drawing.Point(267, 75);
            this.字符提取button.Name = "字符提取button";
            this.字符提取button.Size = new System.Drawing.Size(90, 37);
            this.字符提取button.TabIndex = 81;
            this.字符提取button.Text = "字符提取参数";
            this.字符提取button.UseVisualStyleBackColor = true;
            this.字符提取button.Click += new System.EventHandler(this.字符提取button_Click);
            // 
            // DeepOcrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(369, 199);
            this.Controls.Add(this.字符提取button);
            this.Controls.Add(this.字符区域宽度comboBox);
            this.Controls.Add(this.创建模型Btn);
            this.Controls.Add(this.模型comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.取反图像checkBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DeepOcrForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.CreateDeepOcrForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 模型comboBox;
        private System.Windows.Forms.ComboBox 字符区域宽度comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox 取反图像checkBox;
        private System.Windows.Forms.Button 创建模型Btn;
        private System.Windows.Forms.Button 字符提取button;
    }
}