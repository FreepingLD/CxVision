namespace FunctionBlock
{
    partial class SimplifyObjectModel3DParamForm
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
            this.简化类型comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.三角形反向comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.简化百分比textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 简化类型comboBox
            // 
            this.简化类型comboBox.FormattingEnabled = true;
            this.简化类型comboBox.Location = new System.Drawing.Point(80, 12);
            this.简化类型comboBox.Name = "简化类型comboBox";
            this.简化类型comboBox.Size = new System.Drawing.Size(145, 21);
            this.简化类型comboBox.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "简化类型";
            // 
            // 三角形反向comboBox
            // 
            this.三角形反向comboBox.FormattingEnabled = true;
            this.三角形反向comboBox.Items.AddRange(new object[] {
            "true",
            "false"});
            this.三角形反向comboBox.Location = new System.Drawing.Point(80, 69);
            this.三角形反向comboBox.Name = "三角形反向comboBox";
            this.三角形反向comboBox.Size = new System.Drawing.Size(145, 21);
            this.三角形反向comboBox.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "避免三角反向";
            // 
            // 简化百分比textBox
            // 
            this.简化百分比textBox.Location = new System.Drawing.Point(80, 41);
            this.简化百分比textBox.Name = "简化百分比textBox";
            this.简化百分比textBox.Size = new System.Drawing.Size(145, 20);
            this.简化百分比textBox.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "简化百分比";
            // 
            // SimplifyObjectModel3DParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.简化百分比textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.三角形反向comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.简化类型comboBox);
            this.Controls.Add(this.label1);
            this.Name = "SimplifyObjectModel3DParamForm";
            this.Text = "SimplifyObjectModel3DForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox 简化类型comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 三角形反向comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 简化百分比textBox;
        private System.Windows.Forms.Label label4;
    }
}