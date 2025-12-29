namespace FunctionBlock
{
    partial class DoImageZoomForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.spilitLine1 = new userControl.SpilitLine();
            this.label2 = new System.Windows.Forms.Label();
            this.方法comboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.spilitLine1);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.方法comboBox);
            this.splitContainer1.Size = new System.Drawing.Size(405, 293);
            this.splitContainer1.SplitterDistance = 59;
            this.splitContainer1.TabIndex = 6;
            // 
            // spilitLine1
            // 
            this.spilitLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spilitLine1.Content = "参数";
            this.spilitLine1.Location = new System.Drawing.Point(2, 30);
            this.spilitLine1.Margin = new System.Windows.Forms.Padding(4);
            this.spilitLine1.Name = "spilitLine1";
            this.spilitLine1.Size = new System.Drawing.Size(400, 20);
            this.spilitLine1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "方法";
            // 
            // 方法comboBox
            // 
            this.方法comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.方法comboBox.FormattingEnabled = true;
            this.方法comboBox.Location = new System.Drawing.Point(47, 7);
            this.方法comboBox.Name = "方法comboBox";
            this.方法comboBox.Size = new System.Drawing.Size(346, 20);
            this.方法comboBox.TabIndex = 2;
            this.方法comboBox.SelectedIndexChanged += new System.EventHandler(this.方法comboBox_SelectedIndexChanged);
            this.方法comboBox.SelectionChangeCommitted += new System.EventHandler(this.方法comboBox_SelectionChangeCommitted);
            // 
            // DoImageEnhancementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 293);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DoImageEnhancementForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DoImageMorphologyForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 方法comboBox;
        private userControl.SpilitLine spilitLine1;
    }
}