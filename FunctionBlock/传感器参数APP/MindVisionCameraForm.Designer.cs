namespace FunctionBlock
{
    partial class MindVisionCameraForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.软触发button = new System.Windows.Forms.Button();
            this.参数设置button = new System.Windows.Forms.Button();
            this.监控panel = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.软触发button);
            this.groupBox1.Controls.Add(this.参数设置button);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(728, 506);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "迈德威视相机参数设置 ";
            // 
            // 软触发button
            // 
            this.软触发button.Location = new System.Drawing.Point(6, 96);
            this.软触发button.Name = "软触发button";
            this.软触发button.Size = new System.Drawing.Size(129, 23);
            this.软触发button.TabIndex = 1;
            this.软触发button.Text = "软触发";
            this.软触发button.UseVisualStyleBackColor = true;
            this.软触发button.Click += new System.EventHandler(this.软触发button_Click);
            // 
            // 参数设置button
            // 
            this.参数设置button.Location = new System.Drawing.Point(6, 40);
            this.参数设置button.Name = "参数设置button";
            this.参数设置button.Size = new System.Drawing.Size(75, 23);
            this.参数设置button.TabIndex = 0;
            this.参数设置button.Text = "参数设置 ";
            this.参数设置button.UseVisualStyleBackColor = true;
            this.参数设置button.Click += new System.EventHandler(this.参数设置button_Click);
            // 
            // 监控panel
            // 
            this.监控panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.监控panel.Location = new System.Drawing.Point(147, 0);
            this.监控panel.Name = "监控panel";
            this.监控panel.Size = new System.Drawing.Size(578, 504);
            this.监控panel.TabIndex = 2;
            // 
            // MindVisionCameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 506);
            this.Controls.Add(this.监控panel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MindVisionCameraForm";
            this.Text = "大恒相机窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MindVisionCameraForm_FormClosing);
            this.Load += new System.EventHandler(this.MindVisionCameraForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button 软触发button;
        private System.Windows.Forms.Button 参数设置button;
        private System.Windows.Forms.Panel 监控panel;
    }
}