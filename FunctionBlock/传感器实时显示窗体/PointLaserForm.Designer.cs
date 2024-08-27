namespace FunctionBlock
{
    partial class PointLaserForm
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
            this.光强textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.厚度textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.距离1textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.距离2textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 光强textBox
            // 
            this.光强textBox.Location = new System.Drawing.Point(51, 90);
            this.光强textBox.Name = "光强textBox";
            this.光强textBox.Size = new System.Drawing.Size(162, 21);
            this.光强textBox.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 49;
            this.label3.Text = "光强:";
            // 
            // 厚度textBox
            // 
            this.厚度textBox.Location = new System.Drawing.Point(51, 63);
            this.厚度textBox.Name = "厚度textBox";
            this.厚度textBox.Size = new System.Drawing.Size(162, 21);
            this.厚度textBox.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 47;
            this.label2.Text = "厚度:";
            // 
            // 距离1textBox
            // 
            this.距离1textBox.Location = new System.Drawing.Point(51, 11);
            this.距离1textBox.Name = "距离1textBox";
            this.距离1textBox.Size = new System.Drawing.Size(162, 21);
            this.距离1textBox.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "距离1:";
            // 
            // 距离2textBox
            // 
            this.距离2textBox.Location = new System.Drawing.Point(51, 37);
            this.距离2textBox.Name = "距离2textBox";
            this.距离2textBox.Size = new System.Drawing.Size(162, 21);
            this.距离2textBox.TabIndex = 52;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 51;
            this.label4.Text = "距离2:";
            // 
            // PointLaserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 117);
            this.Controls.Add(this.距离2textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.光强textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.厚度textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.距离1textBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PointLaserForm";
            this.Text = "点激光监控窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointLaserMonitorForm_FormClosing);
            this.Load += new System.EventHandler(this.PointLaserForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox 光强textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 厚度textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 距离1textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox 距离2textBox;
        private System.Windows.Forms.Label label4;
    }
}