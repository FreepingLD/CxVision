namespace FunctionBlock
{
    partial class OperateViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperateViewForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.readDirectoryButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.多文件目录textBox = new System.Windows.Forms.TextBox();
            this.保存button = new System.Windows.Forms.Button();
            this.停止but = new System.Windows.Forms.Button();
            this.运行but = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.操作员comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.产品标识号textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.产品尺寸comboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 5);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(446, 16);
            this.titleLabel.TabIndex = 22;
            this.titleLabel.Text = "CxVision";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.label1_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.buttonMax, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonClose, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMin, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(531, 212);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(476, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(28, 20);
            this.buttonMax.TabIndex = 31;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(504, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(27, 20);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Enabled = false;
            this.buttonMin.Location = new System.Drawing.Point(450, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(26, 20);
            this.buttonMin.TabIndex = 2;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // panel1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 8);
            this.panel1.Controls.Add(this.readDirectoryButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.多文件目录textBox);
            this.panel1.Controls.Add(this.保存button);
            this.panel1.Controls.Add(this.停止but);
            this.panel1.Controls.Add(this.运行but);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.操作员comboBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.产品标识号textBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.产品尺寸comboBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 23);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel2.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(525, 186);
            this.panel1.TabIndex = 25;
            // 
            // readDirectoryButton
            // 
            this.readDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.readDirectoryButton.Location = new System.Drawing.Point(494, 126);
            this.readDirectoryButton.Name = "readDirectoryButton";
            this.readDirectoryButton.Size = new System.Drawing.Size(23, 21);
            this.readDirectoryButton.TabIndex = 42;
            this.readDirectoryButton.Text = "……";
            this.readDirectoryButton.UseVisualStyleBackColor = true;
            this.readDirectoryButton.Click += new System.EventHandler(this.readDirectoryButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "数据保存目录：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // 多文件目录textBox
            // 
            this.多文件目录textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.多文件目录textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.多文件目录textBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.多文件目录textBox.Location = new System.Drawing.Point(282, 124);
            this.多文件目录textBox.Name = "多文件目录textBox";
            this.多文件目录textBox.Size = new System.Drawing.Size(206, 26);
            this.多文件目录textBox.TabIndex = 39;
            this.多文件目录textBox.TextChanged += new System.EventHandler(this.多文件目录textBox_TextChanged);
            // 
            // 保存button
            // 
            this.保存button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.保存button.BackColor = System.Drawing.SystemColors.Control;
            this.保存button.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.保存button.Location = new System.Drawing.Point(421, 47);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(95, 54);
            this.保存button.TabIndex = 38;
            this.保存button.Text = "保存配置";
            this.保存button.UseVisualStyleBackColor = false;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // 停止but
            // 
            this.停止but.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.停止but.BackColor = System.Drawing.Color.Gray;
            this.停止but.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.停止but.Location = new System.Drawing.Point(250, 3);
            this.停止but.Name = "停止but";
            this.停止but.Size = new System.Drawing.Size(151, 98);
            this.停止but.TabIndex = 37;
            this.停止but.Text = "停止";
            this.停止but.UseVisualStyleBackColor = false;
            this.停止but.Click += new System.EventHandler(this.停止but_Click);
            // 
            // 运行but
            // 
            this.运行but.BackColor = System.Drawing.Color.Yellow;
            this.运行but.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.运行but.ForeColor = System.Drawing.Color.Black;
            this.运行but.Location = new System.Drawing.Point(6, 3);
            this.运行but.Name = "运行but";
            this.运行but.Size = new System.Drawing.Size(154, 98);
            this.运行but.TabIndex = 36;
            this.运行but.Text = "运行";
            this.运行but.UseVisualStyleBackColor = false;
            this.运行but.Click += new System.EventHandler(this.运行but_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 35;
            this.label4.Text = "操作员：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // 操作员comboBox
            // 
            this.操作员comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.操作员comboBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.操作员comboBox.FormattingEnabled = true;
            this.操作员comboBox.Location = new System.Drawing.Point(65, 124);
            this.操作员comboBox.Name = "操作员comboBox";
            this.操作员comboBox.Size = new System.Drawing.Size(120, 24);
            this.操作员comboBox.TabIndex = 34;
            this.操作员comboBox.SelectedIndexChanged += new System.EventHandler(this.操作员comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 33;
            this.label2.Text = "产品标识号：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // 产品标识号textBox
            // 
            this.产品标识号textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.产品标识号textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.产品标识号textBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.产品标识号textBox.Location = new System.Drawing.Point(282, 156);
            this.产品标识号textBox.Name = "产品标识号textBox";
            this.产品标识号textBox.Size = new System.Drawing.Size(234, 26);
            this.产品标识号textBox.TabIndex = 32;
            this.产品标识号textBox.TextChanged += new System.EventHandler(this.产品标识号textBox_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(0, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "产品尺寸：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // 产品尺寸comboBox
            // 
            this.产品尺寸comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.产品尺寸comboBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.产品尺寸comboBox.FormattingEnabled = true;
            this.产品尺寸comboBox.Items.AddRange(new object[] {
            "4寸",
            "6寸",
            "8寸",
            "12寸"});
            this.产品尺寸comboBox.Location = new System.Drawing.Point(65, 156);
            this.产品尺寸comboBox.Name = "产品尺寸comboBox";
            this.产品尺寸comboBox.Size = new System.Drawing.Size(120, 24);
            this.产品尺寸comboBox.TabIndex = 30;
            this.产品尺寸comboBox.SelectedIndexChanged += new System.EventHandler(this.产品尺寸comboBox_SelectedIndexChanged);
            // 
            // OperateViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(531, 212);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "OperateViewForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewForm_FormClosing);
            this.Load += new System.EventHandler(this.OperateViewForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewForm_MouseDown);
            this.Move += new System.EventHandler(this.ViewForm_Move);
            this.Resize += new System.EventHandler(this.ViewForm_Resize);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button 停止but;
        private System.Windows.Forms.Button 运行but;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 操作员comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 产品标识号textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 产品尺寸comboBox;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox 多文件目录textBox;
        private System.Windows.Forms.Button readDirectoryButton;
    }
}