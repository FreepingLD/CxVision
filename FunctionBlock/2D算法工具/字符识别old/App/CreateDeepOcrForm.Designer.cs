namespace FunctionBlock
{
    partial class CreateDeepOcrForm
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
            this.字符区域高度comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.取反图像checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 16);
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
            this.模型comboBox.Location = new System.Drawing.Point(83, 13);
            this.模型comboBox.Name = "模型comboBox";
            this.模型comboBox.Size = new System.Drawing.Size(216, 20);
            this.模型comboBox.TabIndex = 65;
            // 
            // 字符区域宽度comboBox
            // 
            this.字符区域宽度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.字符区域宽度comboBox.FormattingEnabled = true;
            this.字符区域宽度comboBox.Location = new System.Drawing.Point(83, 39);
            this.字符区域宽度comboBox.Name = "字符区域宽度comboBox";
            this.字符区域宽度comboBox.Size = new System.Drawing.Size(216, 20);
            this.字符区域宽度comboBox.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "字符区域宽度:";
            // 
            // 字符区域高度comboBox
            // 
            this.字符区域高度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.字符区域高度comboBox.Enabled = false;
            this.字符区域高度comboBox.FormattingEnabled = true;
            this.字符区域高度comboBox.Location = new System.Drawing.Point(83, 65);
            this.字符区域高度comboBox.Name = "字符区域高度comboBox";
            this.字符区域高度comboBox.Size = new System.Drawing.Size(216, 20);
            this.字符区域高度comboBox.TabIndex = 70;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 69;
            this.label3.Text = "字符区域高度:";
            // 
            // 取反图像checkBox
            // 
            this.取反图像checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.取反图像checkBox.AutoSize = true;
            this.取反图像checkBox.Location = new System.Drawing.Point(227, 101);
            this.取反图像checkBox.Name = "取反图像checkBox";
            this.取反图像checkBox.Size = new System.Drawing.Size(72, 16);
            this.取反图像checkBox.TabIndex = 71;
            this.取反图像checkBox.Text = "取反图像";
            this.取反图像checkBox.UseVisualStyleBackColor = true;
            // 
            // CreateDeepOcrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(311, 129);
            this.Controls.Add(this.取反图像checkBox);
            this.Controls.Add(this.字符区域高度comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.字符区域宽度comboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.模型comboBox);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreateDeepOcrForm";
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
        private System.Windows.Forms.ComboBox 字符区域高度comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox 取反图像checkBox;
    }
}