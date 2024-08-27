
namespace FunctionBlock
{
    partial class AddViewForm
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
            this.确定button = new System.Windows.Forms.Button();
            this.窗体类型comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.视图名称comboBox = new System.Windows.Forms.ComboBox();
            this.取消Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "窗体类型：";
            // 
            // 确定button
            // 
            this.确定button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.确定button.Location = new System.Drawing.Point(209, 109);
            this.确定button.Name = "确定button";
            this.确定button.Size = new System.Drawing.Size(95, 23);
            this.确定button.TabIndex = 2;
            this.确定button.Text = "确定";
            this.确定button.UseVisualStyleBackColor = true;
            this.确定button.Click += new System.EventHandler(this.确定button_Click);
            // 
            // 窗体类型comboBox
            // 
            this.窗体类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.窗体类型comboBox.FormattingEnabled = true;
            this.窗体类型comboBox.Location = new System.Drawing.Point(72, 15);
            this.窗体类型comboBox.Name = "窗体类型comboBox";
            this.窗体类型comboBox.Size = new System.Drawing.Size(232, 20);
            this.窗体类型comboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "视图名称：";
            // 
            // 视图名称comboBox
            // 
            this.视图名称comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.视图名称comboBox.FormattingEnabled = true;
            this.视图名称comboBox.Location = new System.Drawing.Point(72, 60);
            this.视图名称comboBox.Name = "视图名称comboBox";
            this.视图名称comboBox.Size = new System.Drawing.Size(232, 20);
            this.视图名称comboBox.TabIndex = 7;
            this.视图名称comboBox.Text = "NONE";
            // 
            // 取消Btn
            // 
            this.取消Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.取消Btn.Location = new System.Drawing.Point(72, 109);
            this.取消Btn.Name = "取消Btn";
            this.取消Btn.Size = new System.Drawing.Size(95, 23);
            this.取消Btn.TabIndex = 8;
            this.取消Btn.Text = "取消";
            this.取消Btn.UseVisualStyleBackColor = true;
            this.取消Btn.Click += new System.EventHandler(this.取消Btn_Click);
            // 
            // AddViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 144);
            this.Controls.Add(this.取消Btn);
            this.Controls.Add(this.视图名称comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.窗体类型comboBox);
            this.Controls.Add(this.确定button);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddViewForm";
            this.Text = "添加视图窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddViewForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 确定button;
        private System.Windows.Forms.ComboBox 窗体类型comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 视图名称comboBox;
        private System.Windows.Forms.Button 取消Btn;
    }
}