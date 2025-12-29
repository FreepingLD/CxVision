namespace FunctionBlock
{
    partial class ScriptDetectForm
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
            this.脚本类型comboBox = new System.Windows.Forms.ComboBox();
            this.启用检测checkBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.脚本路径textBox = new System.Windows.Forms.TextBox();
            this.读取Btn = new System.Windows.Forms.Button();
            this.输入参数设置Btn = new System.Windows.Forms.Button();
            this.输出参数设置Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "脚本类型";
            // 
            // 脚本类型comboBox
            // 
            this.脚本类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.脚本类型comboBox.FormattingEnabled = true;
            this.脚本类型comboBox.Items.AddRange(new object[] {
            "0.0",
            "0.5",
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0"});
            this.脚本类型comboBox.Location = new System.Drawing.Point(60, 6);
            this.脚本类型comboBox.Name = "脚本类型comboBox";
            this.脚本类型comboBox.Size = new System.Drawing.Size(186, 20);
            this.脚本类型comboBox.TabIndex = 5;
            // 
            // 启用检测checkBox
            // 
            this.启用检测checkBox.AutoSize = true;
            this.启用检测checkBox.Location = new System.Drawing.Point(60, 62);
            this.启用检测checkBox.Name = "启用检测checkBox";
            this.启用检测checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用检测checkBox.TabIndex = 6;
            this.启用检测checkBox.Text = "启用检测";
            this.启用检测checkBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "脚本路径";
            // 
            // 脚本路径textBox
            // 
            this.脚本路径textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.脚本路径textBox.Location = new System.Drawing.Point(60, 33);
            this.脚本路径textBox.Name = "脚本路径textBox";
            this.脚本路径textBox.Size = new System.Drawing.Size(160, 21);
            this.脚本路径textBox.TabIndex = 10;
            // 
            // 读取Btn
            // 
            this.读取Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.读取Btn.Location = new System.Drawing.Point(226, 33);
            this.读取Btn.Name = "读取Btn";
            this.读取Btn.Size = new System.Drawing.Size(21, 23);
            this.读取Btn.TabIndex = 11;
            this.读取Btn.Text = "……";
            this.读取Btn.UseVisualStyleBackColor = true;
            this.读取Btn.Click += new System.EventHandler(this.读取Btn_Click);
            // 
            // 输入参数设置Btn
            // 
            this.输入参数设置Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.输入参数设置Btn.Location = new System.Drawing.Point(159, 90);
            this.输入参数设置Btn.Name = "输入参数设置Btn";
            this.输入参数设置Btn.Size = new System.Drawing.Size(87, 35);
            this.输入参数设置Btn.TabIndex = 12;
            this.输入参数设置Btn.Text = "输入参数设置";
            this.输入参数设置Btn.UseVisualStyleBackColor = true;
            this.输入参数设置Btn.Click += new System.EventHandler(this.输入参数设置Btn_Click);
            // 
            // 输出参数设置Btn
            // 
            this.输出参数设置Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.输出参数设置Btn.Location = new System.Drawing.Point(159, 131);
            this.输出参数设置Btn.Name = "输出参数设置Btn";
            this.输出参数设置Btn.Size = new System.Drawing.Size(87, 35);
            this.输出参数设置Btn.TabIndex = 13;
            this.输出参数设置Btn.Text = "输出参数设置";
            this.输出参数设置Btn.UseVisualStyleBackColor = true;
            this.输出参数设置Btn.Click += new System.EventHandler(this.输出参数设置Btn_Click);
            // 
            // ScriptDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(258, 172);
            this.Controls.Add(this.输出参数设置Btn);
            this.Controls.Add(this.输入参数设置Btn);
            this.Controls.Add(this.读取Btn);
            this.Controls.Add(this.脚本路径textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.启用检测checkBox);
            this.Controls.Add(this.脚本类型comboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ScriptDetectForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 脚本类型comboBox;
        private System.Windows.Forms.CheckBox 启用检测checkBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 脚本路径textBox;
        private System.Windows.Forms.Button 读取Btn;
        private System.Windows.Forms.Button 输入参数设置Btn;
        private System.Windows.Forms.Button 输出参数设置Btn;
    }
}