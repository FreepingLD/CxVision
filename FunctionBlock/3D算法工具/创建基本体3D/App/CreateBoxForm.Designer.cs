namespace FunctionBlock
{
    partial class CreateBoxForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.X长度comboBox = new System.Windows.Forms.ComboBox();
            this.Y长度comboBox = new System.Windows.Forms.ComboBox();
            this.Z长度comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "Z长度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Y长度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "X长度";
            // 
            // X长度comboBox
            // 
            this.X长度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.X长度comboBox.FormattingEnabled = true;
            this.X长度comboBox.Location = new System.Drawing.Point(49, 12);
            this.X长度comboBox.Name = "X长度comboBox";
            this.X长度comboBox.Size = new System.Drawing.Size(263, 20);
            this.X长度comboBox.TabIndex = 13;
            // 
            // Y长度comboBox
            // 
            this.Y长度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Y长度comboBox.FormattingEnabled = true;
            this.Y长度comboBox.Location = new System.Drawing.Point(49, 39);
            this.Y长度comboBox.Name = "Y长度comboBox";
            this.Y长度comboBox.Size = new System.Drawing.Size(263, 20);
            this.Y长度comboBox.TabIndex = 14;
            // 
            // Z长度comboBox
            // 
            this.Z长度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Z长度comboBox.FormattingEnabled = true;
            this.Z长度comboBox.Location = new System.Drawing.Point(49, 65);
            this.Z长度comboBox.Name = "Z长度comboBox";
            this.Z长度comboBox.Size = new System.Drawing.Size(263, 20);
            this.Z长度comboBox.TabIndex = 15;
            // 
            // CreateBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 114);
            this.Controls.Add(this.Z长度comboBox);
            this.Controls.Add(this.Y长度comboBox);
            this.Controls.Add(this.X长度comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreateBoxForm";
            this.Text = "CreateBoxForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox X长度comboBox;
        private System.Windows.Forms.ComboBox Y长度comboBox;
        private System.Windows.Forms.ComboBox Z长度comboBox;
    }
}