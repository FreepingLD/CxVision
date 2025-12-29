namespace FunctionBlock
{
    partial class MeanNFilterForm
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
            this.启用滤波checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // 启用滤波checkBox
            // 
            this.启用滤波checkBox.AutoSize = true;
            this.启用滤波checkBox.Location = new System.Drawing.Point(12, 12);
            this.启用滤波checkBox.Name = "启用滤波checkBox";
            this.启用滤波checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用滤波checkBox.TabIndex = 13;
            this.启用滤波checkBox.Text = "启用滤波";
            this.启用滤波checkBox.UseVisualStyleBackColor = true;
            // 
            // MeanNFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 98);
            this.Controls.Add(this.启用滤波checkBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MeanNFilterForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox 启用滤波checkBox;
    }
}