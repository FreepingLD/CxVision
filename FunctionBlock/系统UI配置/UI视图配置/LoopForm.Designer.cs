
namespace FunctionBlock
{
    partial class LoopForm
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
            this.LoopTextBox = new System.Windows.Forms.TextBox();
            this.确定button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "次数：";
            // 
            // LoopTextBox
            // 
            this.LoopTextBox.Location = new System.Drawing.Point(41, 22);
            this.LoopTextBox.Name = "LoopTextBox";
            this.LoopTextBox.Size = new System.Drawing.Size(95, 21);
            this.LoopTextBox.TabIndex = 1;
            this.LoopTextBox.Text = "1";
            this.LoopTextBox.TextChanged += new System.EventHandler(this.LoopTextBox_TextChanged);
            // 
            // 确定button
            // 
            this.确定button.Location = new System.Drawing.Point(142, 22);
            this.确定button.Name = "确定button";
            this.确定button.Size = new System.Drawing.Size(57, 23);
            this.确定button.TabIndex = 2;
            this.确定button.Text = "确定";
            this.确定button.UseVisualStyleBackColor = true;
            this.确定button.Click += new System.EventHandler(this.确定button_Click);
            // 
            // LoopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 65);
            this.Controls.Add(this.确定button);
            this.Controls.Add(this.LoopTextBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoopForm";
            this.ShowIcon = false;
            this.Text = "循环次数";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoopForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LoopTextBox;
        private System.Windows.Forms.Button 确定button;
    }
}