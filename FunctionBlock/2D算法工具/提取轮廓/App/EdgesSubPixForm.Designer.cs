namespace FunctionBlock
{
    partial class EdgesSubPixForm
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
            this.透明度textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.低阈值textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.高阈值textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.滤波器comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "滤波器";
            // 
            // 透明度textBox
            // 
            this.透明度textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.透明度textBox.Location = new System.Drawing.Point(45, 30);
            this.透明度textBox.Name = "透明度textBox";
            this.透明度textBox.Size = new System.Drawing.Size(270, 21);
            this.透明度textBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "透明度";
            // 
            // 低阈值textBox
            // 
            this.低阈值textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.低阈值textBox.Location = new System.Drawing.Point(45, 54);
            this.低阈值textBox.Name = "低阈值textBox";
            this.低阈值textBox.Size = new System.Drawing.Size(270, 21);
            this.低阈值textBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "低阈值";
            // 
            // 高阈值textBox
            // 
            this.高阈值textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.高阈值textBox.Location = new System.Drawing.Point(45, 78);
            this.高阈值textBox.Name = "高阈值textBox";
            this.高阈值textBox.Size = new System.Drawing.Size(270, 21);
            this.高阈值textBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "高阈值";
            // 
            // 滤波器comboBox
            // 
            this.滤波器comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.滤波器comboBox.FormattingEnabled = true;
            this.滤波器comboBox.Location = new System.Drawing.Point(45, 6);
            this.滤波器comboBox.Name = "滤波器comboBox";
            this.滤波器comboBox.Size = new System.Drawing.Size(270, 20);
            this.滤波器comboBox.TabIndex = 17;
            // 
            // EdgesSubPixForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 100);
            this.Controls.Add(this.滤波器comboBox);
            this.Controls.Add(this.高阈值textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.低阈值textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.透明度textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EdgesSubPixForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox 透明度textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 低阈值textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 高阈值textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 滤波器comboBox;
    }
}