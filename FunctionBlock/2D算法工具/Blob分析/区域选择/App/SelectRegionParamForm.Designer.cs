namespace FunctionBlock
{
    partial class SelectRegionParamForm
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
            this.区域距离ComboBox = new System.Windows.Forms.ComboBox();
            this.区域数量comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "区域距离：";
            // 
            // 区域距离ComboBox
            // 
            this.区域距离ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域距离ComboBox.FormattingEnabled = true;
            this.区域距离ComboBox.Items.AddRange(new object[] {
            "max_area",
            "rectangle1",
            "rectangle2"});
            this.区域距离ComboBox.Location = new System.Drawing.Point(71, 6);
            this.区域距离ComboBox.Name = "区域距离ComboBox";
            this.区域距离ComboBox.Size = new System.Drawing.Size(228, 20);
            this.区域距离ComboBox.TabIndex = 5;
            // 
            // 区域数量comboBox
            // 
            this.区域数量comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.区域数量comboBox.FormattingEnabled = true;
            this.区域数量comboBox.Items.AddRange(new object[] {
            "70",
            "90",
            "95",
            "100"});
            this.区域数量comboBox.Location = new System.Drawing.Point(71, 32);
            this.区域数量comboBox.Name = "区域数量comboBox";
            this.区域数量comboBox.Size = new System.Drawing.Size(228, 20);
            this.区域数量comboBox.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "区域数量：";
            // 
            // SelectRegionParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 78);
            this.Controls.Add(this.区域数量comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.区域距离ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectRegionParamForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 区域距离ComboBox;
        private System.Windows.Forms.ComboBox 区域数量comboBox;
        private System.Windows.Forms.Label label2;
    }
}