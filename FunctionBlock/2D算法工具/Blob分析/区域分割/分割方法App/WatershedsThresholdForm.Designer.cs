﻿namespace FunctionBlock
{
    partial class WatershedsThresholdForm
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
            this.阈值comboBox = new System.Windows.Forms.ComboBox();
            this.区域连通comboBox = new System.Windows.Forms.ComboBox();
            this.区域填充comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "阈值";
            // 
            // 阈值comboBox
            // 
            this.阈值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.阈值comboBox.FormattingEnabled = true;
            this.阈值comboBox.Items.AddRange(new object[] {
            "dark",
            "light"});
            this.阈值comboBox.Location = new System.Drawing.Point(61, 4);
            this.阈值comboBox.Name = "阈值comboBox";
            this.阈值comboBox.Size = new System.Drawing.Size(338, 20);
            this.阈值comboBox.TabIndex = 14;
            // 
            // 区域连通comboBox
            // 
            this.区域连通comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域连通comboBox.FormattingEnabled = true;
            this.区域连通comboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.区域连通comboBox.Location = new System.Drawing.Point(61, 56);
            this.区域连通comboBox.Name = "区域连通comboBox";
            this.区域连通comboBox.Size = new System.Drawing.Size(338, 20);
            this.区域连通comboBox.TabIndex = 35;
            // 
            // 区域填充comboBox
            // 
            this.区域填充comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域填充comboBox.FormattingEnabled = true;
            this.区域填充comboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.区域填充comboBox.Location = new System.Drawing.Point(61, 30);
            this.区域填充comboBox.Name = "区域填充comboBox";
            this.区域填充comboBox.Size = new System.Drawing.Size(338, 20);
            this.区域填充comboBox.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "区域连通";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 32;
            this.label4.Text = "区域填充";
            // 
            // WatershedsThresholdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 100);
            this.Controls.Add(this.区域连通comboBox);
            this.Controls.Add(this.区域填充comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.阈值comboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WatershedsThresholdForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 阈值comboBox;
        private System.Windows.Forms.ComboBox 区域连通comboBox;
        private System.Windows.Forms.ComboBox 区域填充comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}