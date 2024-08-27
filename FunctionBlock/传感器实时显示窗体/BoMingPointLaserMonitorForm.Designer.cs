namespace FunctionBlock
{
    partial class BoMingPointLaserMonitorForm
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
            this.距离textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 光强textBox
            // 
            this.光强textBox.Location = new System.Drawing.Point(53, 64);
            this.光强textBox.Name = "光强textBox";
            this.光强textBox.Size = new System.Drawing.Size(162, 21);
            this.光强textBox.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 49;
            this.label3.Text = "光强";
            // 
            // 厚度textBox
            // 
            this.厚度textBox.Location = new System.Drawing.Point(53, 37);
            this.厚度textBox.Name = "厚度textBox";
            this.厚度textBox.Size = new System.Drawing.Size(162, 21);
            this.厚度textBox.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 47;
            this.label2.Text = "厚度值";
            // 
            // 距离textBox
            // 
            this.距离textBox.Location = new System.Drawing.Point(53, 11);
            this.距离textBox.Name = "距离textBox";
            this.距离textBox.Size = new System.Drawing.Size(162, 21);
            this.距离textBox.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "距离值";
            // 
            // BoMingPointLaserMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 90);
            this.Controls.Add(this.光强textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.厚度textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.距离textBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BoMingPointLaserMonitorForm";
            this.Text = "点激光监控窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointLaserMonitorForm_FormClosing);
            this.Load += new System.EventHandler(this.PointLaserMonitorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox 光强textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 厚度textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 距离textBox;
        private System.Windows.Forms.Label label1;
    }
}