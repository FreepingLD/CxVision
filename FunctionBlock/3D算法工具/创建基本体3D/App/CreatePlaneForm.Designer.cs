namespace FunctionBlock
{
    partial class CreatePlaneForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.最小YomboBox = new System.Windows.Forms.ComboBox();
            this.最大XomboBox = new System.Windows.Forms.ComboBox();
            this.最小XcomboBox = new System.Windows.Forms.ComboBox();
            this.最大YcomboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "最小X范围";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "最大X范围";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "最小Y范围";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "最大Y范围";
            // 
            // 最小YomboBox
            // 
            this.最小YomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小YomboBox.FormattingEnabled = true;
            this.最小YomboBox.Location = new System.Drawing.Point(70, 61);
            this.最小YomboBox.Name = "最小YomboBox";
            this.最小YomboBox.Size = new System.Drawing.Size(329, 20);
            this.最小YomboBox.TabIndex = 24;
            // 
            // 最大XomboBox
            // 
            this.最大XomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最大XomboBox.FormattingEnabled = true;
            this.最大XomboBox.Location = new System.Drawing.Point(70, 35);
            this.最大XomboBox.Name = "最大XomboBox";
            this.最大XomboBox.Size = new System.Drawing.Size(329, 20);
            this.最大XomboBox.TabIndex = 23;
            // 
            // 最小XcomboBox
            // 
            this.最小XcomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小XcomboBox.FormattingEnabled = true;
            this.最小XcomboBox.Location = new System.Drawing.Point(70, 8);
            this.最小XcomboBox.Name = "最小XcomboBox";
            this.最小XcomboBox.Size = new System.Drawing.Size(329, 20);
            this.最小XcomboBox.TabIndex = 22;
            // 
            // 最大YcomboBox
            // 
            this.最大YcomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最大YcomboBox.FormattingEnabled = true;
            this.最大YcomboBox.Location = new System.Drawing.Point(70, 87);
            this.最大YcomboBox.Name = "最大YcomboBox";
            this.最大YcomboBox.Size = new System.Drawing.Size(329, 20);
            this.最大YcomboBox.TabIndex = 25;
            // 
            // CreatePlaneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 132);
            this.Controls.Add(this.最大YcomboBox);
            this.Controls.Add(this.最小YomboBox);
            this.Controls.Add(this.最大XomboBox);
            this.Controls.Add(this.最小XcomboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreatePlaneForm";
            this.Text = "创建平面窗体";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 最小YomboBox;
        private System.Windows.Forms.ComboBox 最大XomboBox;
        private System.Windows.Forms.ComboBox 最小XcomboBox;
        private System.Windows.Forms.ComboBox 最大YcomboBox;
    }
}