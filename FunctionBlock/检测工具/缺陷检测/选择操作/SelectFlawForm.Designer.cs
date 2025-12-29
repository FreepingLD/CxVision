namespace FunctionBlock
{
    partial class SelectFlawForm
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
            this.spilitLine1 = new userControl.SpilitLine();
            this.label2 = new System.Windows.Forms.Label();
            this.方法comboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.参数panel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.测试选择button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // spilitLine1
            // 
            this.spilitLine1.Content = "参数";
            this.spilitLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spilitLine1.Location = new System.Drawing.Point(3, 40);
            this.spilitLine1.Name = "spilitLine1";
            this.spilitLine1.Size = new System.Drawing.Size(299, 14);
            this.spilitLine1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "选择方法";
            // 
            // 方法comboBox
            // 
            this.方法comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.方法comboBox.FormattingEnabled = true;
            this.方法comboBox.Location = new System.Drawing.Point(65, 6);
            this.方法comboBox.Name = "方法comboBox";
            this.方法comboBox.Size = new System.Drawing.Size(231, 20);
            this.方法comboBox.TabIndex = 2;
            this.方法comboBox.TextChanged += new System.EventHandler(this.方法comboBox_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.spilitLine1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.参数panel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(305, 264);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.方法comboBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(299, 31);
            this.panel1.TabIndex = 0;
            // 
            // 参数panel
            // 
            this.参数panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.参数panel.Location = new System.Drawing.Point(3, 60);
            this.参数panel.Name = "参数panel";
            this.参数panel.Size = new System.Drawing.Size(299, 168);
            this.参数panel.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.测试选择button);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 234);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(299, 27);
            this.panel2.TabIndex = 13;
            // 
            // 测试选择button
            // 
            this.测试选择button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.测试选择button.Location = new System.Drawing.Point(236, 1);
            this.测试选择button.Name = "测试选择button";
            this.测试选择button.Size = new System.Drawing.Size(63, 23);
            this.测试选择button.TabIndex = 0;
            this.测试选择button.Text = "测试选择";
            this.测试选择button.UseVisualStyleBackColor = true;
            this.测试选择button.Click += new System.EventHandler(this.测试选择button_Click);
            // 
            // SelectFlawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 264);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectFlawForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FilterForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private userControl.SpilitLine spilitLine1;
        public System.Windows.Forms.ComboBox 方法comboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel 参数panel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button 测试选择button;
    }
}