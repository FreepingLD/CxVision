namespace FunctionBlock
{
    partial class BinaryThresholdDetectForm
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
            this.方法comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.亮暗comboBox = new System.Windows.Forms.ComboBox();
            this.启用检测checkBox = new System.Windows.Forms.CheckBox();
            this.最小面积comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "方法";
            // 
            // 方法comboBox
            // 
            this.方法comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.方法comboBox.FormattingEnabled = true;
            this.方法comboBox.Items.AddRange(new object[] {
            "max_separability",
            "smooth_histo"});
            this.方法comboBox.Location = new System.Drawing.Point(63, 3);
            this.方法comboBox.Name = "方法comboBox";
            this.方法comboBox.Size = new System.Drawing.Size(310, 20);
            this.方法comboBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "暗/亮";
            // 
            // 亮暗comboBox
            // 
            this.亮暗comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.亮暗comboBox.FormattingEnabled = true;
            this.亮暗comboBox.Items.AddRange(new object[] {
            "dark",
            "light"});
            this.亮暗comboBox.Location = new System.Drawing.Point(63, 29);
            this.亮暗comboBox.Name = "亮暗comboBox";
            this.亮暗comboBox.Size = new System.Drawing.Size(310, 20);
            this.亮暗comboBox.TabIndex = 6;
            // 
            // 启用检测checkBox
            // 
            this.启用检测checkBox.AutoSize = true;
            this.启用检测checkBox.Location = new System.Drawing.Point(63, 83);
            this.启用检测checkBox.Name = "启用检测checkBox";
            this.启用检测checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用检测checkBox.TabIndex = 7;
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
            this.最小面积comboBox.Location = new System.Drawing.Point(63, 57);
            this.最小面积comboBox.Name = "最小面积comboBox";
            this.最小面积comboBox.Size = new System.Drawing.Size(310, 20);
            this.最小面积comboBox.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "最小面积";
            // 
            // BinaryThresholdDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(385, 144);
            this.Controls.Add(this.最小面积comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.启用检测checkBox);
            this.Controls.Add(this.亮暗comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.方法comboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BinaryThresholdDetectForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 方法comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 亮暗comboBox;
        private System.Windows.Forms.CheckBox 启用检测checkBox;
        private System.Windows.Forms.ComboBox 最小面积comboBox;
        private System.Windows.Forms.Label label3;
    }
}