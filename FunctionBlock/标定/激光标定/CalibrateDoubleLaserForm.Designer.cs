namespace FunctionBlock
{
    partial class CalibrateDoubleLaserForm
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
            this.上激光comboBox = new System.Windows.Forms.ComboBox();
            this.下激光comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.上激光textBox = new System.Windows.Forms.TextBox();
            this.下激光textBox = new System.Windows.Forms.TextBox();
            this.执行button = new System.Windows.Forms.Button();
            this.标准块值textBox = new System.Windows.Forms.TextBox();
            this.lable1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.坐标间隔textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "上激光";
            // 
            // 上激光comboBox
            // 
            this.上激光comboBox.FormattingEnabled = true;
            this.上激光comboBox.Location = new System.Drawing.Point(64, 13);
            this.上激光comboBox.Name = "上激光comboBox";
            this.上激光comboBox.Size = new System.Drawing.Size(226, 20);
            this.上激光comboBox.TabIndex = 1;
            // 
            // 下激光comboBox
            // 
            this.下激光comboBox.FormattingEnabled = true;
            this.下激光comboBox.Location = new System.Drawing.Point(64, 46);
            this.下激光comboBox.Name = "下激光comboBox";
            this.下激光comboBox.Size = new System.Drawing.Size(226, 20);
            this.下激光comboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "下激光";
            // 
            // 上激光textBox
            // 
            this.上激光textBox.Location = new System.Drawing.Point(342, 13);
            this.上激光textBox.Name = "上激光textBox";
            this.上激光textBox.Size = new System.Drawing.Size(111, 21);
            this.上激光textBox.TabIndex = 6;
            // 
            // 下激光textBox
            // 
            this.下激光textBox.Location = new System.Drawing.Point(342, 46);
            this.下激光textBox.Name = "下激光textBox";
            this.下激光textBox.Size = new System.Drawing.Size(111, 21);
            this.下激光textBox.TabIndex = 7;
            // 
            // 执行button
            // 
            this.执行button.Location = new System.Drawing.Point(342, 100);
            this.执行button.Name = "执行button";
            this.执行button.Size = new System.Drawing.Size(111, 39);
            this.执行button.TabIndex = 10;
            this.执行button.Text = "对射标定";
            this.执行button.UseVisualStyleBackColor = true;
            this.执行button.Click += new System.EventHandler(this.button1_Click);
            // 
            // 标准块值textBox
            // 
            this.标准块值textBox.Location = new System.Drawing.Point(64, 83);
            this.标准块值textBox.Name = "标准块值textBox";
            this.标准块值textBox.Size = new System.Drawing.Size(226, 21);
            this.标准块值textBox.TabIndex = 11;
            this.标准块值textBox.Text = "0.0";
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Location = new System.Drawing.Point(3, 86);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(41, 12);
            this.lable1.TabIndex = 12;
            this.lable1.Text = "标准值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "标定值";
            // 
            // 坐标间隔textBox
            // 
            this.坐标间隔textBox.Location = new System.Drawing.Point(64, 115);
            this.坐标间隔textBox.Name = "坐标间隔textBox";
            this.坐标间隔textBox.Size = new System.Drawing.Size(226, 21);
            this.坐标间隔textBox.TabIndex = 13;
            this.坐标间隔textBox.Text = "0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "高度值";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(296, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "高度值";
            // 
            // CalibrateDoubleLaserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 140);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.坐标间隔textBox);
            this.Controls.Add(this.lable1);
            this.Controls.Add(this.标准块值textBox);
            this.Controls.Add(this.执行button);
            this.Controls.Add(this.下激光textBox);
            this.Controls.Add(this.上激光textBox);
            this.Controls.Add(this.下激光comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.上激光comboBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalibrateDoubleLaserForm";
            this.Text = "对射标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalibrateDoubleLaserForm_FormClosing);
            this.Load += new System.EventHandler(this.CalibrateDoubleLaserForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 上激光comboBox;
        private System.Windows.Forms.ComboBox 下激光comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 上激光textBox;
        private System.Windows.Forms.TextBox 下激光textBox;
        private System.Windows.Forms.Button 执行button;
        private System.Windows.Forms.TextBox 标准块值textBox;
        private System.Windows.Forms.Label lable1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 坐标间隔textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}