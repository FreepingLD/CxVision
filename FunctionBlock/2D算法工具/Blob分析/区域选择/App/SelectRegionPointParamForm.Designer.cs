﻿namespace FunctionBlock
{
    partial class SelectRegionPointParamForm
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
            this.行坐标ComboBox = new System.Windows.Forms.ComboBox();
            this.列坐标comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "行坐标";
            // 
            // 行坐标ComboBox
            // 
            this.行坐标ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.行坐标ComboBox.FormattingEnabled = true;
            this.行坐标ComboBox.Items.AddRange(new object[] {
            "0.0",
            "0.5",
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0"});
            this.行坐标ComboBox.Location = new System.Drawing.Point(45, 6);
            this.行坐标ComboBox.Name = "行坐标ComboBox";
            this.行坐标ComboBox.Size = new System.Drawing.Size(279, 20);
            this.行坐标ComboBox.TabIndex = 5;
            // 
            // 列坐标comboBox
            // 
            this.列坐标comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.列坐标comboBox.FormattingEnabled = true;
            this.列坐标comboBox.Items.AddRange(new object[] {
            "0.0",
            "0.5",
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0"});
            this.列坐标comboBox.Location = new System.Drawing.Point(45, 32);
            this.列坐标comboBox.Name = "列坐标comboBox";
            this.列坐标comboBox.Size = new System.Drawing.Size(279, 20);
            this.列坐标comboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "列坐标";
            // 
            // SelectRegionPointFormOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 62);
            this.Controls.Add(this.列坐标comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.行坐标ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectRegionPointFormOld";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 行坐标ComboBox;
        private System.Windows.Forms.ComboBox 列坐标comboBox;
        private System.Windows.Forms.Label label2;
    }
}