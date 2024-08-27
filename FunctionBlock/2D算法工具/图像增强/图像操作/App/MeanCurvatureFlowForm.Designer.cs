namespace FunctionBlock
{
    partial class MeanCurvatureFlowForm
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
            this.平滑系数ComboBox = new System.Windows.Forms.ComboBox();
            this.时间步长ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.迭代次数comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "平滑系数";
            // 
            // 平滑系数ComboBox
            // 
            this.平滑系数ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.平滑系数ComboBox.FormattingEnabled = true;
            this.平滑系数ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.平滑系数ComboBox.Location = new System.Drawing.Point(58, 6);
            this.平滑系数ComboBox.Name = "平滑系数ComboBox";
            this.平滑系数ComboBox.Size = new System.Drawing.Size(206, 20);
            this.平滑系数ComboBox.TabIndex = 5;
            // 
            // 时间步长ComboBox
            // 
            this.时间步长ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.时间步长ComboBox.FormattingEnabled = true;
            this.时间步长ComboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.时间步长ComboBox.Location = new System.Drawing.Point(58, 32);
            this.时间步长ComboBox.Name = "时间步长ComboBox";
            this.时间步长ComboBox.Size = new System.Drawing.Size(206, 20);
            this.时间步长ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "时间步长";
            // 
            // 迭代次数comboBox
            // 
            this.迭代次数comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.迭代次数comboBox.FormattingEnabled = true;
            this.迭代次数comboBox.Items.AddRange(new object[] {
            "3.0",
            "5.0",
            "7.0",
            "9.0"});
            this.迭代次数comboBox.Location = new System.Drawing.Point(58, 60);
            this.迭代次数comboBox.Name = "迭代次数comboBox";
            this.迭代次数comboBox.Size = new System.Drawing.Size(206, 20);
            this.迭代次数comboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "迭代次数";
            // 
            // MeanCurvatureFlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 98);
            this.Controls.Add(this.迭代次数comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.时间步长ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.平滑系数ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MeanCurvatureFlowForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 平滑系数ComboBox;
        private System.Windows.Forms.ComboBox 时间步长ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 迭代次数comboBox;
        private System.Windows.Forms.Label label3;
    }
}