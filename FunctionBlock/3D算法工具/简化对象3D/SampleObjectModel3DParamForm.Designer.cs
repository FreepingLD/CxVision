namespace FunctionBlock
{
    partial class SampleObjectModel3DParamForm
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
            this.最小点数textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.角度偏差textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.采样距离textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.采样方法comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 最小点数textBox
            // 
            this.最小点数textBox.Location = new System.Drawing.Point(82, 91);
            this.最小点数textBox.Name = "最小点数textBox";
            this.最小点数textBox.Size = new System.Drawing.Size(145, 20);
            this.最小点数textBox.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "最小点数";
            // 
            // 角度偏差textBox
            // 
            this.角度偏差textBox.Location = new System.Drawing.Point(82, 65);
            this.角度偏差textBox.Name = "角度偏差textBox";
            this.角度偏差textBox.Size = new System.Drawing.Size(145, 20);
            this.角度偏差textBox.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "最大角度偏差";
            // 
            // 采样距离textBox
            // 
            this.采样距离textBox.Location = new System.Drawing.Point(82, 39);
            this.采样距离textBox.Name = "采样距离textBox";
            this.采样距离textBox.Size = new System.Drawing.Size(145, 20);
            this.采样距离textBox.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "采样距离";
            // 
            // 采样方法comboBox
            // 
            this.采样方法comboBox.FormattingEnabled = true;
            this.采样方法comboBox.Items.AddRange(new object[] {
            "accurate",
            "accurate_use_normals",
            "fast",
            "fast_compute_normals"});
            this.采样方法comboBox.Location = new System.Drawing.Point(82, 12);
            this.采样方法comboBox.Name = "采样方法comboBox";
            this.采样方法comboBox.Size = new System.Drawing.Size(145, 21);
            this.采样方法comboBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "采样方法";
            // 
            // SampleObjectModel3DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.最小点数textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.角度偏差textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.采样距离textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.采样方法comboBox);
            this.Controls.Add(this.label1);
            this.Name = "SampleObjectModel3DForm";
            this.Text = "SampleObjectModel3DForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox 最小点数textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 角度偏差textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 采样距离textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 采样方法comboBox;
        private System.Windows.Forms.Label label1;
    }
}