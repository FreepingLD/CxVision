namespace FunctionBlock
{
    partial class OcrParamForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.识别算法comboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.spilitLine1 = new userControl.SpilitLine();
            this.panel1 = new System.Windows.Forms.Panel();
            this.参数panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "识别算法";
            // 
            // 识别算法comboBox
            // 
            this.识别算法comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.识别算法comboBox.FormattingEnabled = true;
            this.识别算法comboBox.Location = new System.Drawing.Point(62, 3);
            this.识别算法comboBox.Name = "识别算法comboBox";
            this.识别算法comboBox.Size = new System.Drawing.Size(255, 20);
            this.识别算法comboBox.TabIndex = 2;
            this.识别算法comboBox.TextChanged += new System.EventHandler(this.检测算法comboBox_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.spilitLine1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.参数panel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(326, 293);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // spilitLine1
            // 
            this.spilitLine1.Content = "参数";
            this.spilitLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spilitLine1.Location = new System.Drawing.Point(3, 36);
            this.spilitLine1.Name = "spilitLine1";
            this.spilitLine1.Size = new System.Drawing.Size(320, 12);
            this.spilitLine1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.识别算法comboBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 27);
            this.panel1.TabIndex = 0;
            // 
            // 参数panel
            // 
            this.参数panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.参数panel.Location = new System.Drawing.Point(3, 54);
            this.参数panel.Name = "参数panel";
            this.tableLayoutPanel1.SetRowSpan(this.参数panel, 2);
            this.参数panel.Size = new System.Drawing.Size(320, 236);
            this.参数panel.TabIndex = 1;
            // 
            // OcrParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 293);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OcrParamForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DetectFlawForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private userControl.SpilitLine spilitLine1;
        public System.Windows.Forms.ComboBox 识别算法comboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel 参数panel;
    }
}