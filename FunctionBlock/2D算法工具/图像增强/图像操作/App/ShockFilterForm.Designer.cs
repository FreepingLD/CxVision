namespace FunctionBlock
{
    partial class ShockFilterForm
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
            this.时间步长ComboBox = new System.Windows.Forms.ComboBox();
            this.边缘检测平滑ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.边缘检测方式comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.迭代次数comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "时间步长";
            // 
            // 时间步长ComboBox
            // 
            this.时间步长ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.时间步长ComboBox.FormattingEnabled = true;
            this.时间步长ComboBox.Items.AddRange(new object[] {
            "0.1",
            "0.2",
            "0.3",
            "0.4",
            "0.5",
            "0.6",
            "0.7"});
            this.时间步长ComboBox.Location = new System.Drawing.Point(98, 8);
            this.时间步长ComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.时间步长ComboBox.Name = "时间步长ComboBox";
            this.时间步长ComboBox.Size = new System.Drawing.Size(254, 23);
            this.时间步长ComboBox.TabIndex = 5;
            // 
            // 边缘检测平滑ComboBox
            // 
            this.边缘检测平滑ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.边缘检测平滑ComboBox.FormattingEnabled = true;
            this.边缘检测平滑ComboBox.Items.AddRange(new object[] {
            "0.0",
            "0.5",
            "1.0",
            "2.0",
            "5.0",
            ""});
            this.边缘检测平滑ComboBox.Location = new System.Drawing.Point(98, 40);
            this.边缘检测平滑ComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.边缘检测平滑ComboBox.Name = "边缘检测平滑ComboBox";
            this.边缘检测平滑ComboBox.Size = new System.Drawing.Size(254, 23);
            this.边缘检测平滑ComboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "边缘检测平滑";
            // 
            // 边缘检测方式comboBox
            // 
            this.边缘检测方式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.边缘检测方式comboBox.FormattingEnabled = true;
            this.边缘检测方式comboBox.Items.AddRange(new object[] {
            "canny",
            "laplace"});
            this.边缘检测方式comboBox.Location = new System.Drawing.Point(98, 72);
            this.边缘检测方式comboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.边缘检测方式comboBox.Name = "边缘检测方式comboBox";
            this.边缘检测方式comboBox.Size = new System.Drawing.Size(254, 23);
            this.边缘检测方式comboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-3, 76);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "边缘检测方式";
            // 
            // 迭代次数comboBox
            // 
            this.迭代次数comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.迭代次数comboBox.FormattingEnabled = true;
            this.迭代次数comboBox.Items.AddRange(new object[] {
            "1",
            "3",
            "10",
            "100"});
            this.迭代次数comboBox.Location = new System.Drawing.Point(98, 107);
            this.迭代次数comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.迭代次数comboBox.Name = "迭代次数comboBox";
            this.迭代次数comboBox.Size = new System.Drawing.Size(254, 23);
            this.迭代次数comboBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 111);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "迭代次数";
            // 
            // ShockFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 150);
            this.Controls.Add(this.迭代次数comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.边缘检测方式comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.边缘检测平滑ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.时间步长ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ShockFilterForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 时间步长ComboBox;
        private System.Windows.Forms.ComboBox 边缘检测平滑ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 边缘检测方式comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 迭代次数comboBox;
        private System.Windows.Forms.Label label4;
    }
}