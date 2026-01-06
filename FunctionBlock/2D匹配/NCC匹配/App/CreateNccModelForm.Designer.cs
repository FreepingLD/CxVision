namespace FunctionBlock
{
    partial class CreateNccModelParamForm
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
            this.终止角度textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.起始角度textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.更多参数button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 终止角度textBox
            // 
            this.终止角度textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.终止角度textBox.Location = new System.Drawing.Point(66, 29);
            this.终止角度textBox.Name = "终止角度textBox";
            this.终止角度textBox.Size = new System.Drawing.Size(249, 21);
            this.终止角度textBox.TabIndex = 67;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 66;
            this.label4.Text = "终止角度:";
            // 
            // 起始角度textBox
            // 
            this.起始角度textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.起始角度textBox.Location = new System.Drawing.Point(66, 3);
            this.起始角度textBox.Name = "起始角度textBox";
            this.起始角度textBox.Size = new System.Drawing.Size(249, 21);
            this.起始角度textBox.TabIndex = 65;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 64;
            this.label2.Text = "起始角度:";
            // 
            // 更多参数button
            // 
            this.更多参数button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.更多参数button.Location = new System.Drawing.Point(243, 54);
            this.更多参数button.Name = "更多参数button";
            this.更多参数button.Size = new System.Drawing.Size(72, 24);
            this.更多参数button.TabIndex = 103;
            this.更多参数button.Text = "更多参数";
            this.更多参数button.UseVisualStyleBackColor = true;
            this.更多参数button.Click += new System.EventHandler(this.更多参数button_Click);
            // 
            // CreateNccModelParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(320, 81);
            this.Controls.Add(this.更多参数button);
            this.Controls.Add(this.终止角度textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.起始角度textBox);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreateNccModelParamForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.CShapeModelForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox 终止角度textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 起始角度textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button 更多参数button;
    }
}