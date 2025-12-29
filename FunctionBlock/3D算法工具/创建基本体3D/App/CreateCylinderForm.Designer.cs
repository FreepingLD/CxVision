namespace FunctionBlock
{
    partial class CreateCylinderForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.最大长度comboBox = new System.Windows.Forms.ComboBox();
            this.最小长度comboBox = new System.Windows.Forms.ComboBox();
            this.半径comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "半径";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "最小长度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "最大长度";
            // 
            // 最大长度comboBox
            // 
            this.最大长度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最大长度comboBox.FormattingEnabled = true;
            this.最大长度comboBox.Location = new System.Drawing.Point(66, 62);
            this.最大长度comboBox.Name = "最大长度comboBox";
            this.最大长度comboBox.Size = new System.Drawing.Size(336, 20);
            this.最大长度comboBox.TabIndex = 21;
            // 
            // 最小长度comboBox
            // 
            this.最小长度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小长度comboBox.FormattingEnabled = true;
            this.最小长度comboBox.Location = new System.Drawing.Point(66, 36);
            this.最小长度comboBox.Name = "最小长度comboBox";
            this.最小长度comboBox.Size = new System.Drawing.Size(336, 20);
            this.最小长度comboBox.TabIndex = 20;
            // 
            // 半径comboBox
            // 
            this.半径comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.半径comboBox.FormattingEnabled = true;
            this.半径comboBox.Location = new System.Drawing.Point(66, 9);
            this.半径comboBox.Name = "半径comboBox";
            this.半径comboBox.Size = new System.Drawing.Size(336, 20);
            this.半径comboBox.TabIndex = 19;
            // 
            // CreateCylinderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 255);
            this.Controls.Add(this.最大长度comboBox);
            this.Controls.Add(this.最小长度comboBox);
            this.Controls.Add(this.半径comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "CreateCylinderForm";
            this.Text = "CreateCylinderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 最大长度comboBox;
        private System.Windows.Forms.ComboBox 最小长度comboBox;
        private System.Windows.Forms.ComboBox 半径comboBox;
    }
}