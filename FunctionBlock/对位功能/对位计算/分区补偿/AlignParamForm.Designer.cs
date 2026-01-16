namespace FunctionBlock
{
    partial class AlignParamForm
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
            this.采样间隔textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.结束点百分比textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.变换类型comboBox = new System.Windows.Forms.ComboBox();
            this.起始点百分比textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "采样间隔:";
            // 
            // 采样间隔textBox
            // 
            this.采样间隔textBox.Location = new System.Drawing.Point(85, 96);
            this.采样间隔textBox.Name = "采样间隔textBox";
            this.采样间隔textBox.Size = new System.Drawing.Size(163, 21);
            this.采样间隔textBox.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "结束点百分比:";
            // 
            // 结束点百分比textBox
            // 
            this.结束点百分比textBox.Location = new System.Drawing.Point(85, 68);
            this.结束点百分比textBox.Name = "结束点百分比textBox";
            this.结束点百分比textBox.Size = new System.Drawing.Size(163, 21);
            this.结束点百分比textBox.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "起始点百分比:";
            // 
            // 变换类型comboBox
            // 
            this.变换类型comboBox.FormattingEnabled = true;
            this.变换类型comboBox.Location = new System.Drawing.Point(85, 12);
            this.变换类型comboBox.Name = "变换类型comboBox";
            this.变换类型comboBox.Size = new System.Drawing.Size(163, 20);
            this.变换类型comboBox.TabIndex = 12;
            // 
            // 起始点百分比textBox
            // 
            this.起始点百分比textBox.Location = new System.Drawing.Point(85, 41);
            this.起始点百分比textBox.Name = "起始点百分比textBox";
            this.起始点百分比textBox.Size = new System.Drawing.Size(163, 21);
            this.起始点百分比textBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "变换类型:";
            // 
            // AlignParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 128);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.采样间隔textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.结束点百分比textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.变换类型comboBox);
            this.Controls.Add(this.起始点百分比textBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlignParamForm";
            this.ShowIcon = false;
            this.Text = "对位匹配参数";
            this.Load += new System.EventHandler(this.AlignParamForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 采样间隔textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 结束点百分比textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 变换类型comboBox;
        private System.Windows.Forms.TextBox 起始点百分比textBox;
        private System.Windows.Forms.Label label1;
    }


}