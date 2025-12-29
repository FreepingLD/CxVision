namespace FunctionBlock
{
    partial class SelectShapeStdFlawForm
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
            this.特征ComboBox = new System.Windows.Forms.ComboBox();
            this.启用算法checkBox = new System.Windows.Forms.CheckBox();
            this.极性comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.百分比comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "特征";
            // 
            // 特征ComboBox
            // 
            this.特征ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.特征ComboBox.FormattingEnabled = true;
            this.特征ComboBox.Items.AddRange(new object[] {
            "max_area",
            "rectangle1",
            "rectangle2"});
            this.特征ComboBox.Location = new System.Drawing.Point(51, 6);
            this.特征ComboBox.Name = "特征ComboBox";
            this.特征ComboBox.Size = new System.Drawing.Size(193, 20);
            this.特征ComboBox.TabIndex = 5;
            // 
            // 启用算法checkBox
            // 
            this.启用算法checkBox.AutoSize = true;
            this.启用算法checkBox.Location = new System.Drawing.Point(51, 86);
            this.启用算法checkBox.Name = "启用算法checkBox";
            this.启用算法checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用算法checkBox.TabIndex = 14;
            this.启用算法checkBox.Text = "启用算法";
            this.启用算法checkBox.UseVisualStyleBackColor = true;
            // 
            // 极性comboBox
            // 
            this.极性comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.极性comboBox.FormattingEnabled = true;
            this.极性comboBox.Items.AddRange(new object[] {
            "light",
            "dark",
            "all"});
            this.极性comboBox.Location = new System.Drawing.Point(51, 58);
            this.极性comboBox.Name = "极性comboBox";
            this.极性comboBox.Size = new System.Drawing.Size(193, 20);
            this.极性comboBox.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "极性";
            // 
            // 百分比comboBox
            // 
            this.百分比comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.百分比comboBox.FormattingEnabled = true;
            this.百分比comboBox.Items.AddRange(new object[] {
            "10.0",
            "30.0",
            "50.0",
            "60.0",
            "70.0",
            "80.0",
            "90.0",
            "95.0",
            "100.0"});
            this.百分比comboBox.Location = new System.Drawing.Point(51, 32);
            this.百分比comboBox.Name = "百分比comboBox";
            this.百分比comboBox.Size = new System.Drawing.Size(193, 20);
            this.百分比comboBox.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "百分比";
            // 
            // SelectShapeStdFlawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 123);
            this.Controls.Add(this.百分比comboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.极性comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.启用算法checkBox);
            this.Controls.Add(this.特征ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectShapeStdFlawForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 特征ComboBox;
        private System.Windows.Forms.CheckBox 启用算法checkBox;
        private System.Windows.Forms.ComboBox 极性comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 百分比comboBox;
        private System.Windows.Forms.Label label5;
    }
}