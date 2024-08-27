
namespace FunctionBlock
{
    partial class LoginForm
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
            this.用户名comboBox = new System.Windows.Forms.ComboBox();
            this.密码textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.确定button = new System.Windows.Forms.Button();
            this.添加用户Btn = new System.Windows.Forms.Button();
            this.修改密码Btn = new System.Windows.Forms.Button();
            this.删除用户Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // 用户名comboBox
            // 
            this.用户名comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.用户名comboBox.FormattingEnabled = true;
            this.用户名comboBox.Items.AddRange(new object[] {
            "操作员",
            "工程师"});
            this.用户名comboBox.Location = new System.Drawing.Point(75, 19);
            this.用户名comboBox.Name = "用户名comboBox";
            this.用户名comboBox.Size = new System.Drawing.Size(182, 20);
            this.用户名comboBox.TabIndex = 1;
            // 
            // 密码textBox
            // 
            this.密码textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.密码textBox.Location = new System.Drawing.Point(75, 55);
            this.密码textBox.Name = "密码textBox";
            this.密码textBox.PasswordChar = '*';
            this.密码textBox.Size = new System.Drawing.Size(182, 21);
            this.密码textBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密  码：";
            // 
            // 确定button
            // 
            this.确定button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.确定button.Location = new System.Drawing.Point(182, 98);
            this.确定button.Name = "确定button";
            this.确定button.Size = new System.Drawing.Size(75, 57);
            this.确定button.TabIndex = 4;
            this.确定button.Text = "登录";
            this.确定button.UseVisualStyleBackColor = true;
            this.确定button.Click += new System.EventHandler(this.确定button_Click);
            // 
            // 添加用户Btn
            // 
            this.添加用户Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.添加用户Btn.Location = new System.Drawing.Point(12, 89);
            this.添加用户Btn.Name = "添加用户Btn";
            this.添加用户Btn.Size = new System.Drawing.Size(75, 31);
            this.添加用户Btn.TabIndex = 5;
            this.添加用户Btn.Text = "添加用户";
            this.添加用户Btn.UseVisualStyleBackColor = true;
            this.添加用户Btn.Click += new System.EventHandler(this.添加用户Btn_Click);
            // 
            // 修改密码Btn
            // 
            this.修改密码Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.修改密码Btn.Location = new System.Drawing.Point(95, 126);
            this.修改密码Btn.Name = "修改密码Btn";
            this.修改密码Btn.Size = new System.Drawing.Size(75, 31);
            this.修改密码Btn.TabIndex = 6;
            this.修改密码Btn.Text = "修改密码";
            this.修改密码Btn.UseVisualStyleBackColor = true;
            this.修改密码Btn.Click += new System.EventHandler(this.修改密码Btn_Click);
            // 
            // 删除用户Btn
            // 
            this.删除用户Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.删除用户Btn.Location = new System.Drawing.Point(12, 126);
            this.删除用户Btn.Name = "删除用户Btn";
            this.删除用户Btn.Size = new System.Drawing.Size(75, 31);
            this.删除用户Btn.TabIndex = 7;
            this.删除用户Btn.Text = "删除用户";
            this.删除用户Btn.UseVisualStyleBackColor = true;
            this.删除用户Btn.Click += new System.EventHandler(this.删除用户Btn_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 166);
            this.Controls.Add(this.删除用户Btn);
            this.Controls.Add(this.修改密码Btn);
            this.Controls.Add(this.添加用户Btn);
            this.Controls.Add(this.确定button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.密码textBox);
            this.Controls.Add(this.用户名comboBox);
            this.Controls.Add(this.label1);
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.Text = "登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 用户名comboBox;
        private System.Windows.Forms.TextBox 密码textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button 确定button;
        private System.Windows.Forms.Button 添加用户Btn;
        private System.Windows.Forms.Button 修改密码Btn;
        private System.Windows.Forms.Button 删除用户Btn;
    }
}