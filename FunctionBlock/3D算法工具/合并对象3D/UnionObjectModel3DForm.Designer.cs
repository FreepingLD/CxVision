﻿namespace FunctionBlock
{
    partial class UnionObjectModel3DForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.显示条目comboBox = new System.Windows.Forms.ComboBox();
            this.视图工具toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Translate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Auto = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_3D = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值1Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值2Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值3Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.行坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.列坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanel1.Controls.Add(this.显示条目comboBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip2, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1028, 668);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // 显示条目comboBox
            // 
            this.显示条目comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.显示条目comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.显示条目comboBox.FormattingEnabled = true;
            this.显示条目comboBox.Location = new System.Drawing.Point(771, 3);
            this.显示条目comboBox.Name = "显示条目comboBox";
            this.显示条目comboBox.Size = new System.Drawing.Size(254, 20);
            this.显示条目comboBox.TabIndex = 11;
            this.显示条目comboBox.SelectionChangeCommitted += new System.EventHandler(this.显示条目comboBox_SelectionChangeCommitted);
            // 
            // 视图工具toolStrip
            // 
            this.视图工具toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.视图工具toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Clear,
            this.toolStripButton_Select,
            this.toolStripButton_Translate,
            this.toolStripButton_Auto,
            this.toolStripButton_3D});
            this.视图工具toolStrip.Location = new System.Drawing.Point(360, 0);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(408, 28);
            this.视图工具toolStrip.TabIndex = 15;
            this.视图工具toolStrip.Text = "toolStrip2";
            this.视图工具toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.Image = global::FunctionBlock.Properties.Resources.清除;
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(52, 25);
            this.toolStripButton_Clear.Text = "清除";
            // 
            // toolStripButton_Select
            // 
            this.toolStripButton_Select.Image = global::FunctionBlock.Properties.Resources.选择光标;
            this.toolStripButton_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Select.Name = "toolStripButton_Select";
            this.toolStripButton_Select.Size = new System.Drawing.Size(52, 25);
            this.toolStripButton_Select.Text = "选择";
            // 
            // toolStripButton_Translate
            // 
            this.toolStripButton_Translate.Image = global::FunctionBlock.Properties.Resources.移动光标;
            this.toolStripButton_Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Translate.Name = "toolStripButton_Translate";
            this.toolStripButton_Translate.Size = new System.Drawing.Size(52, 25);
            this.toolStripButton_Translate.Text = "平移";
            // 
            // toolStripButton_Auto
            // 
            this.toolStripButton_Auto.Image = global::FunctionBlock.Properties.Resources.适配窗口光标;
            this.toolStripButton_Auto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Auto.Name = "toolStripButton_Auto";
            this.toolStripButton_Auto.Size = new System.Drawing.Size(76, 25);
            this.toolStripButton_Auto.Text = "适应窗口";
            // 
            // toolStripButton_3D
            // 
            this.toolStripButton_3D.Image = global::FunctionBlock.Properties.Resources._3D;
            this.toolStripButton_3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_3D.Name = "toolStripButton_3D";
            this.toolStripButton_3D.Size = new System.Drawing.Size(44, 25);
            this.toolStripButton_3D.Text = "3D";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 638);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(360, 30);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 25);
            this.toolStripStatusLabel1.Text = "执行结果：";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(28, 25);
            this.toolStripStatusLabel2.Text = "……";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 31);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 4);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(354, 604);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(346, 578);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 482F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(340, 572);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 566);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "引用对象";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 17);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(328, 546);
            this.listBox1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(360, 28);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources.Start;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(52, 25);
            this.toolStripButton_Run.Text = "执行";
            // 
            // statusStrip2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip2, 2);
            this.statusStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.灰度值1Label,
            this.灰度值2Label,
            this.灰度值3Label,
            this.toolStripStatusLabel7,
            this.行坐标Label,
            this.列坐标Label});
            this.statusStrip2.Location = new System.Drawing.Point(360, 638);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(668, 30);
            this.statusStrip2.TabIndex = 14;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(56, 25);
            this.toolStripStatusLabel3.Text = "灰度值：";
            // 
            // 灰度值1Label
            // 
            this.灰度值1Label.Name = "灰度值1Label";
            this.灰度值1Label.Size = new System.Drawing.Size(28, 25);
            this.灰度值1Label.Text = "……";
            // 
            // 灰度值2Label
            // 
            this.灰度值2Label.Name = "灰度值2Label";
            this.灰度值2Label.Size = new System.Drawing.Size(28, 25);
            this.灰度值2Label.Text = "……";
            // 
            // 灰度值3Label
            // 
            this.灰度值3Label.Name = "灰度值3Label";
            this.灰度值3Label.Size = new System.Drawing.Size(28, 25);
            this.灰度值3Label.Text = "……";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(44, 25);
            this.toolStripStatusLabel7.Text = "坐标：";
            // 
            // 行坐标Label
            // 
            this.行坐标Label.Name = "行坐标Label";
            this.行坐标Label.Size = new System.Drawing.Size(28, 25);
            this.行坐标Label.Text = "……";
            // 
            // 列坐标Label
            // 
            this.列坐标Label.Name = "列坐标Label";
            this.列坐标Label.Size = new System.Drawing.Size(28, 25);
            this.列坐标Label.Text = "……";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(363, 31);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 4);
            this.hWindowControl1.Size = new System.Drawing.Size(662, 604);
            this.hWindowControl1.TabIndex = 17;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(662, 604);
            // 
            // UnionObjectModel3DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 668);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UnionObjectModel3DForm";
            this.Text = "合并对象3D";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeformableMatchForm_FormClosing);
            this.Load += new System.EventHandler(this.AppBaseForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox 显示条目comboBox;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.ToolStrip 视图工具toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_Clear;
        private System.Windows.Forms.ToolStripButton toolStripButton_Select;
        private System.Windows.Forms.ToolStripButton toolStripButton_Translate;
        private System.Windows.Forms.ToolStripButton toolStripButton_Auto;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.ToolStripButton toolStripButton_3D;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox1;
    }
}