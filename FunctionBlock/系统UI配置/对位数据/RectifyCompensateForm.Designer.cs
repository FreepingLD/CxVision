namespace FunctionBlock
{
    partial class RectifyCompensateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RectifyCompensateForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.纠偏坐标系comboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.启用分区纠偏checkBox = new System.Windows.Forms.CheckBox();
            this.分区纠偏设置Btn = new System.Windows.Forms.Button();
            this.模式comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.视图窗口comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.目标角度XtextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMax, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 214F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(257, 230);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(228, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(29, 25);
            this.buttonClose.TabIndex = 30;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(198, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(30, 25);
            this.buttonMax.TabIndex = 29;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Enabled = false;
            this.buttonMin.Location = new System.Drawing.Point(168, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(30, 25);
            this.buttonMin.TabIndex = 28;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(168, 25);
            this.titleLabel.TabIndex = 15;
            this.titleLabel.Text = "对位补偿设置";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 4);
            this.groupBox1.Controls.Add(this.纠偏坐标系comboBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.启用分区纠偏checkBox);
            this.groupBox1.Controls.Add(this.分区纠偏设置Btn);
            this.groupBox1.Controls.Add(this.模式comboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.视图窗口comboBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.目标角度XtextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(257, 205);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            // 
            // 纠偏坐标系comboBox
            // 
            this.纠偏坐标系comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.纠偏坐标系comboBox.FormattingEnabled = true;
            this.纠偏坐标系comboBox.Items.AddRange(new object[] {
            "单点对齐",
            "点+角度对齐",
            "两点对齐",
            "三点对齐",
            "四点对齐"});
            this.纠偏坐标系comboBox.Location = new System.Drawing.Point(81, 20);
            this.纠偏坐标系comboBox.Name = "纠偏坐标系comboBox";
            this.纠偏坐标系comboBox.Size = new System.Drawing.Size(169, 20);
            this.纠偏坐标系comboBox.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 26;
            this.label7.Text = "纠偏坐标系:";
            // 
            // 启用分区纠偏checkBox
            // 
            this.启用分区纠偏checkBox.AutoSize = true;
            this.启用分区纠偏checkBox.Location = new System.Drawing.Point(81, 125);
            this.启用分区纠偏checkBox.Name = "启用分区纠偏checkBox";
            this.启用分区纠偏checkBox.Size = new System.Drawing.Size(96, 16);
            this.启用分区纠偏checkBox.TabIndex = 25;
            this.启用分区纠偏checkBox.Text = "启用分区纠偏";
            this.启用分区纠偏checkBox.UseVisualStyleBackColor = true;
            // 
            // 分区纠偏设置Btn
            // 
            this.分区纠偏设置Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.分区纠偏设置Btn.Location = new System.Drawing.Point(157, 163);
            this.分区纠偏设置Btn.Name = "分区纠偏设置Btn";
            this.分区纠偏设置Btn.Size = new System.Drawing.Size(93, 42);
            this.分区纠偏设置Btn.TabIndex = 24;
            this.分区纠偏设置Btn.Text = "分区纠偏设置";
            this.分区纠偏设置Btn.UseVisualStyleBackColor = true;
            this.分区纠偏设置Btn.Click += new System.EventHandler(this.分区纠偏设置Btn_Click);
            // 
            // 模式comboBox
            // 
            this.模式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.模式comboBox.FormattingEnabled = true;
            this.模式comboBox.Items.AddRange(new object[] {
            "手动",
            "自动"});
            this.模式comboBox.Location = new System.Drawing.Point(81, 72);
            this.模式comboBox.Name = "模式comboBox";
            this.模式comboBox.Size = new System.Drawing.Size(169, 20);
            this.模式comboBox.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "模式:";
            // 
            // 视图窗口comboBox
            // 
            this.视图窗口comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.视图窗口comboBox.FormattingEnabled = true;
            this.视图窗口comboBox.Items.AddRange(new object[] {
            "单点对齐",
            "点+角度对齐",
            "两点对齐",
            "三点对齐",
            "四点对齐"});
            this.视图窗口comboBox.Location = new System.Drawing.Point(81, 46);
            this.视图窗口comboBox.Name = "视图窗口comboBox";
            this.视图窗口comboBox.Size = new System.Drawing.Size(169, 20);
            this.视图窗口comboBox.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "视图窗口:";
            // 
            // 目标角度XtextBox
            // 
            this.目标角度XtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.目标角度XtextBox.Location = new System.Drawing.Point(81, 98);
            this.目标角度XtextBox.Name = "目标角度XtextBox";
            this.目标角度XtextBox.Size = new System.Drawing.Size(169, 21);
            this.目标角度XtextBox.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "目标角度:";
            // 
            // RectifyCompensateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 230);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RectifyCompensateForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CompensateForm_FormClosing);
            this.Load += new System.EventHandler(this.CompensateForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CompensateForm_MouseDown);
            this.Move += new System.EventHandler(this.CompensateForm_Move);
            this.Resize += new System.EventHandler(this.CompensateForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox 纠偏坐标系comboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox 启用分区纠偏checkBox;
        private System.Windows.Forms.Button 分区纠偏设置Btn;
        private System.Windows.Forms.ComboBox 模式comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 视图窗口comboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox 目标角度XtextBox;
        private System.Windows.Forms.Label label1;
    }
}