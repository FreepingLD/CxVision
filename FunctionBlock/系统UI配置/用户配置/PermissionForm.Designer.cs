namespace FunctionBlock
{
    partial class PermissionForm
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
            this.操作checkBox = new System.Windows.Forms.CheckBox();
            this.编辑checkBox = new System.Windows.Forms.CheckBox();
            this.管理checkBox = new System.Windows.Forms.CheckBox();
            this.开发checkBox = new System.Windows.Forms.CheckBox();
            this.取消Btn = new System.Windows.Forms.Button();
            this.确定button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 操作checkBox
            // 
            this.操作checkBox.AutoSize = true;
            this.操作checkBox.Location = new System.Drawing.Point(12, 24);
            this.操作checkBox.Name = "操作checkBox";
            this.操作checkBox.Size = new System.Drawing.Size(48, 16);
            this.操作checkBox.TabIndex = 0;
            this.操作checkBox.Text = "操作";
            this.操作checkBox.UseVisualStyleBackColor = true;
            // 
            // 编辑checkBox
            // 
            this.编辑checkBox.AutoSize = true;
            this.编辑checkBox.Location = new System.Drawing.Point(11, 57);
            this.编辑checkBox.Name = "编辑checkBox";
            this.编辑checkBox.Size = new System.Drawing.Size(48, 16);
            this.编辑checkBox.TabIndex = 1;
            this.编辑checkBox.Text = "编辑";
            this.编辑checkBox.UseVisualStyleBackColor = true;
            // 
            // 管理checkBox
            // 
            this.管理checkBox.AutoSize = true;
            this.管理checkBox.Location = new System.Drawing.Point(117, 24);
            this.管理checkBox.Name = "管理checkBox";
            this.管理checkBox.Size = new System.Drawing.Size(48, 16);
            this.管理checkBox.TabIndex = 2;
            this.管理checkBox.Text = "管理";
            this.管理checkBox.UseVisualStyleBackColor = true;
            // 
            // 开发checkBox
            // 
            this.开发checkBox.AutoSize = true;
            this.开发checkBox.Location = new System.Drawing.Point(117, 56);
            this.开发checkBox.Name = "开发checkBox";
            this.开发checkBox.Size = new System.Drawing.Size(48, 16);
            this.开发checkBox.TabIndex = 3;
            this.开发checkBox.Text = "开发";
            this.开发checkBox.UseVisualStyleBackColor = true;
            // 
            // 取消Btn
            // 
            this.取消Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.取消Btn.Location = new System.Drawing.Point(106, 156);
            this.取消Btn.Name = "取消Btn";
            this.取消Btn.Size = new System.Drawing.Size(69, 31);
            this.取消Btn.TabIndex = 8;
            this.取消Btn.Text = "取消";
            this.取消Btn.UseVisualStyleBackColor = true;
            this.取消Btn.Click += new System.EventHandler(this.取消Btn_Click);
            // 
            // 确定button
            // 
            this.确定button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.确定button.Location = new System.Drawing.Point(187, 156);
            this.确定button.Name = "确定button";
            this.确定button.Size = new System.Drawing.Size(69, 31);
            this.确定button.TabIndex = 7;
            this.确定button.Text = "确认";
            this.确定button.UseVisualStyleBackColor = true;
            this.确定button.Click += new System.EventHandler(this.确定button_Click);
            // 
            // PermissionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 195);
            this.Controls.Add(this.取消Btn);
            this.Controls.Add(this.确定button);
            this.Controls.Add(this.开发checkBox);
            this.Controls.Add(this.管理checkBox);
            this.Controls.Add(this.编辑checkBox);
            this.Controls.Add(this.操作checkBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PermissionForm";
            this.ShowIcon = false;
            this.Text = "权限配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox 操作checkBox;
        private System.Windows.Forms.CheckBox 编辑checkBox;
        private System.Windows.Forms.CheckBox 管理checkBox;
        private System.Windows.Forms.CheckBox 开发checkBox;
        private System.Windows.Forms.Button 取消Btn;
        private System.Windows.Forms.Button 确定button;
    }
}