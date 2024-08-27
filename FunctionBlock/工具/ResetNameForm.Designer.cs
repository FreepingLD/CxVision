
namespace FunctionBlock
{
    partial class ResetNameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResetNameForm));
            this.label1 = new System.Windows.Forms.Label();
            this.旧名称textBox = new System.Windows.Forms.TextBox();
            this.确定button = new System.Windows.Forms.Button();
            this.新名称textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.取消Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "旧名称:";
            // 
            // 旧名称textBox
            // 
            this.旧名称textBox.Location = new System.Drawing.Point(54, 28);
            this.旧名称textBox.Name = "旧名称textBox";
            this.旧名称textBox.Size = new System.Drawing.Size(235, 21);
            this.旧名称textBox.TabIndex = 1;
            this.旧名称textBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // 确定button
            // 
            this.确定button.Location = new System.Drawing.Point(214, 107);
            this.确定button.Name = "确定button";
            this.确定button.Size = new System.Drawing.Size(75, 23);
            this.确定button.TabIndex = 2;
            this.确定button.Text = "确定";
            this.确定button.UseVisualStyleBackColor = true;
            this.确定button.Click += new System.EventHandler(this.确定button_Click);
            // 
            // 新名称textBox
            // 
            this.新名称textBox.Location = new System.Drawing.Point(54, 67);
            this.新名称textBox.Name = "新名称textBox";
            this.新名称textBox.Size = new System.Drawing.Size(235, 21);
            this.新名称textBox.TabIndex = 4;
            this.新名称textBox.TextChanged += new System.EventHandler(this.新名称textBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "新名称:";
            // 
            // 取消Btn
            // 
            this.取消Btn.Location = new System.Drawing.Point(110, 107);
            this.取消Btn.Name = "取消Btn";
            this.取消Btn.Size = new System.Drawing.Size(75, 23);
            this.取消Btn.TabIndex = 5;
            this.取消Btn.Text = "取消";
            this.取消Btn.UseVisualStyleBackColor = true;
            this.取消Btn.Click += new System.EventHandler(this.取消Btn_Click);
            // 
            // ResetNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 136);
            this.Controls.Add(this.取消Btn);
            this.Controls.Add(this.新名称textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.确定button);
            this.Controls.Add(this.旧名称textBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResetNameForm";
            this.Text = "重命名";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox 旧名称textBox;
        private System.Windows.Forms.Button 确定button;
        private System.Windows.Forms.TextBox 新名称textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button 取消Btn;
    }
}