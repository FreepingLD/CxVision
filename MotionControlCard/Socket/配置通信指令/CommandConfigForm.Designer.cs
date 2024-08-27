
namespace MotionControlCard
{
    partial class CommandConfigForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.命令列表comboBox = new System.Windows.Forms.ComboBox();
            this.添加Btn = new System.Windows.Forms.Button();
            this.删除Btn = new System.Windows.Forms.Button();
            this.清空Btn = new System.Windows.Forms.Button();
            this.保存配置Btn = new System.Windows.Forms.Button();
            this.清空配置Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(272, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(235, 508);
            this.listBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "命令列表:";
            // 
            // 命令列表comboBox
            // 
            this.命令列表comboBox.FormattingEnabled = true;
            this.命令列表comboBox.Location = new System.Drawing.Point(68, 14);
            this.命令列表comboBox.Name = "命令列表comboBox";
            this.命令列表comboBox.Size = new System.Drawing.Size(198, 20);
            this.命令列表comboBox.TabIndex = 2;
            // 
            // 添加Btn
            // 
            this.添加Btn.Location = new System.Drawing.Point(210, 54);
            this.添加Btn.Name = "添加Btn";
            this.添加Btn.Size = new System.Drawing.Size(56, 23);
            this.添加Btn.TabIndex = 3;
            this.添加Btn.Text = "Add(+)";
            this.添加Btn.UseVisualStyleBackColor = true;
            this.添加Btn.Click += new System.EventHandler(this.添加Btn_Click);
            // 
            // 删除Btn
            // 
            this.删除Btn.Location = new System.Drawing.Point(210, 98);
            this.删除Btn.Name = "删除Btn";
            this.删除Btn.Size = new System.Drawing.Size(56, 23);
            this.删除Btn.TabIndex = 4;
            this.删除Btn.Text = "Del(-)";
            this.删除Btn.UseVisualStyleBackColor = true;
            this.删除Btn.Click += new System.EventHandler(this.删除Btn_Click);
            // 
            // 清空Btn
            // 
            this.清空Btn.Location = new System.Drawing.Point(210, 143);
            this.清空Btn.Name = "清空Btn";
            this.清空Btn.Size = new System.Drawing.Size(56, 23);
            this.清空Btn.TabIndex = 5;
            this.清空Btn.Text = "Clear";
            this.清空Btn.UseVisualStyleBackColor = true;
            this.清空Btn.Click += new System.EventHandler(this.清空Btn_Click);
            // 
            // 保存配置Btn
            // 
            this.保存配置Btn.Location = new System.Drawing.Point(168, 454);
            this.保存配置Btn.Name = "保存配置Btn";
            this.保存配置Btn.Size = new System.Drawing.Size(89, 54);
            this.保存配置Btn.TabIndex = 6;
            this.保存配置Btn.Text = "保存配置";
            this.保存配置Btn.UseVisualStyleBackColor = true;
            this.保存配置Btn.Click += new System.EventHandler(this.保存配置Btn_Click);
            // 
            // 清空配置Btn
            // 
            this.清空配置Btn.Location = new System.Drawing.Point(68, 454);
            this.清空配置Btn.Name = "清空配置Btn";
            this.清空配置Btn.Size = new System.Drawing.Size(89, 54);
            this.清空配置Btn.TabIndex = 7;
            this.清空配置Btn.Text = "清空配置";
            this.清空配置Btn.UseVisualStyleBackColor = true;
            this.清空配置Btn.Click += new System.EventHandler(this.清空配置Btn_Click);
            // 
            // CommandConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 514);
            this.Controls.Add(this.清空配置Btn);
            this.Controls.Add(this.保存配置Btn);
            this.Controls.Add(this.清空Btn);
            this.Controls.Add(this.删除Btn);
            this.Controls.Add(this.添加Btn);
            this.Controls.Add(this.命令列表comboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommandConfigForm";
            this.ShowIcon = false;
            this.Text = "命令配置窗体";
            this.Load += new System.EventHandler(this.CommandConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 命令列表comboBox;
        private System.Windows.Forms.Button 添加Btn;
        private System.Windows.Forms.Button 删除Btn;
        private System.Windows.Forms.Button 清空Btn;
        private System.Windows.Forms.Button 保存配置Btn;
        private System.Windows.Forms.Button 清空配置Btn;
    }
}