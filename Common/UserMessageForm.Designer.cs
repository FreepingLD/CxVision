
namespace Common
{
    partial class UserMessageForm
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.确定Btn = new System.Windows.Forms.Button();
            this.取消Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(2, 27);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(255, 74);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // 确定Btn
            // 
            this.确定Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.确定Btn.Location = new System.Drawing.Point(182, 104);
            this.确定Btn.Name = "确定Btn";
            this.确定Btn.Size = new System.Drawing.Size(75, 23);
            this.确定Btn.TabIndex = 1;
            this.确定Btn.Text = "确定";
            this.确定Btn.UseVisualStyleBackColor = true;
            this.确定Btn.Click += new System.EventHandler(this.确定Btn_Click);
            // 
            // 取消Btn
            // 
            this.取消Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.取消Btn.Location = new System.Drawing.Point(83, 104);
            this.取消Btn.Name = "取消Btn";
            this.取消Btn.Size = new System.Drawing.Size(75, 23);
            this.取消Btn.TabIndex = 2;
            this.取消Btn.Text = "取消";
            this.取消Btn.UseVisualStyleBackColor = true;
            this.取消Btn.Click += new System.EventHandler(this.取消Btn_Click);
            // 
            // UserMessageForm
            // 
            this.AcceptButton = this.确定Btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 131);
            this.Controls.Add(this.取消Btn);
            this.Controls.Add(this.确定Btn);
            this.Controls.Add(this.richTextBox1);
            this.Name = "UserMessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "消息框";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserMessageForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button 确定Btn;
        private System.Windows.Forms.Button 取消Btn;
    }
}