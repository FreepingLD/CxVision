namespace FunctionBlock
{
    partial class SelectShapeProtoFlawForm
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
            this.最小值comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.最大值comboBox = new System.Windows.Forms.ComboBox();
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
            "covers",
            "distance_center",
            "distance_contour",
            "distance_dilate",
            "fits",
            "overlaps_abs",
            "overlaps_rel"});
            this.特征ComboBox.Location = new System.Drawing.Point(51, 6);
            this.特征ComboBox.Name = "特征ComboBox";
            this.特征ComboBox.Size = new System.Drawing.Size(165, 20);
            this.特征ComboBox.TabIndex = 5;
            // 
            // 启用算法checkBox
            // 
            this.启用算法checkBox.AutoSize = true;
            this.启用算法checkBox.Location = new System.Drawing.Point(51, 112);
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
            this.极性comboBox.Location = new System.Drawing.Point(51, 84);
            this.极性comboBox.Name = "极性comboBox";
            this.极性comboBox.Size = new System.Drawing.Size(165, 20);
            this.极性comboBox.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "极性";
            // 
            // 最小值comboBox
            // 
            this.最小值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小值comboBox.FormattingEnabled = true;
            this.最小值comboBox.Items.AddRange(new object[] {
            "0.0",
            "1.0",
            "5.0",
            "10.0",
            "20.0",
            "30.0",
            "50.0",
            "60.0",
            "70.0",
            "80.0",
            "90.0",
            "95.0",
            "99.0",
            "100.0",
            "200.0",
            "400.0"});
            this.最小值comboBox.Location = new System.Drawing.Point(51, 32);
            this.最小值comboBox.Name = "最小值comboBox";
            this.最小值comboBox.Size = new System.Drawing.Size(165, 20);
            this.最小值comboBox.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "最小值";
            // 
            // 最大值comboBox
            // 
            this.最大值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最大值comboBox.FormattingEnabled = true;
            this.最大值comboBox.Items.AddRange(new object[] {
            "0.0",
            "1.0",
            "5.0",
            "10.0",
            "20.0",
            "30.0",
            "50.0",
            "60.0",
            "70.0",
            "80.0",
            "90.0",
            "95.0",
            "99.0",
            "100.0",
            "200.0",
            "400.0"});
            this.最大值comboBox.Location = new System.Drawing.Point(51, 58);
            this.最大值comboBox.Name = "最大值comboBox";
            this.最大值comboBox.Size = new System.Drawing.Size(165, 20);
            this.最大值comboBox.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "最大值";
            // 
            // SelectShapeProtoFlawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 143);
            this.Controls.Add(this.最大值comboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.最小值comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.极性comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.启用算法checkBox);
            this.Controls.Add(this.特征ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectShapeProtoFlawForm";
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
        private System.Windows.Forms.ComboBox 最小值comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 最大值comboBox;
        private System.Windows.Forms.Label label5;
    }
}