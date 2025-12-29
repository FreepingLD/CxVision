namespace FunctionBlock
{
    partial class MultAddParamForm
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
            this.MultComboBox = new System.Windows.Forms.ComboBox();
            this.Add值comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mult系数:";
            // 
            // MultComboBox
            // 
            this.MultComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MultComboBox.FormattingEnabled = true;
            this.MultComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.MultComboBox.Location = new System.Drawing.Point(85, 8);
            this.MultComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MultComboBox.Name = "MultComboBox";
            this.MultComboBox.Size = new System.Drawing.Size(268, 23);
            this.MultComboBox.TabIndex = 5;
            // 
            // Add值comboBox
            // 
            this.Add值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Add值comboBox.FormattingEnabled = true;
            this.Add值comboBox.Items.AddRange(new object[] {
            "0",
            "100",
            "180",
            "200",
            "255"});
            this.Add值comboBox.Location = new System.Drawing.Point(85, 41);
            this.Add值comboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Add值comboBox.Name = "Add值comboBox";
            this.Add值comboBox.Size = new System.Drawing.Size(268, 23);
            this.Add值comboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Add值:";
            // 
            // MultAddParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 271);
            this.Controls.Add(this.Add值comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MultComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MultAddParamForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox MultComboBox;
        private System.Windows.Forms.ComboBox Add值comboBox;
        private System.Windows.Forms.Label label2;
    }
}