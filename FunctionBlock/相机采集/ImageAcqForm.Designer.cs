﻿namespace FunctionBlock
{
    partial class ImageAcqForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageAcqForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.文件类型comboBox = new System.Windows.Forms.ComboBox();
            this.readDirectoryButton = new System.Windows.Forms.Button();
            this.多文件目录textBox = new System.Windows.Forms.TextBox();
            this.目录源radioButton = new System.Windows.Forms.RadioButton();
            this.拍照点textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.readFileButton = new System.Windows.Forms.Button();
            this.单文件路径textBox = new System.Windows.Forms.TextBox();
            this.采集源comboBox = new System.Windows.Forms.ComboBox();
            this.文件源radioButton = new System.Windows.Forms.RadioButton();
            this.相机源radioButton = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 222F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.bindingNavigator1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip2, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1068, 706);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(583, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(222, 28);
            this.bindingNavigator1.TabIndex = 37;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 25);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(24, 25);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            this.bindingNavigatorMoveFirstItem.Click += new System.EventHandler(this.bindingNavigatorMoveFirstItem_Click);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(24, 25);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 28);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(24, 25);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(24, 25);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 676);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(320, 30);
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
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(320, 28);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources.Start;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(56, 25);
            this.toolStripButton_Run.Text = "执行";
            // 
            // statusStrip2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip2, 3);
            this.statusStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.灰度值1Label,
            this.灰度值2Label,
            this.灰度值3Label,
            this.toolStripStatusLabel7,
            this.行坐标Label,
            this.列坐标Label});
            this.statusStrip2.Location = new System.Drawing.Point(320, 676);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(748, 30);
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
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 3);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(323, 31);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 4);
            this.hWindowControl1.Size = new System.Drawing.Size(742, 642);
            this.hWindowControl1.TabIndex = 17;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(742, 642);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(1, 188);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox2, 3);
            this.groupBox2.Size = new System.Drawing.Size(318, 487);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "采集源参数";
            this.groupBox2.UseCompatibleTextRendering = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 467);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.文件类型comboBox);
            this.groupBox3.Controls.Add(this.readDirectoryButton);
            this.groupBox3.Controls.Add(this.多文件目录textBox);
            this.groupBox3.Controls.Add(this.目录源radioButton);
            this.groupBox3.Controls.Add(this.拍照点textBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.readFileButton);
            this.groupBox3.Controls.Add(this.单文件路径textBox);
            this.groupBox3.Controls.Add(this.采集源comboBox);
            this.groupBox3.Controls.Add(this.文件源radioButton);
            this.groupBox3.Controls.Add(this.相机源radioButton);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(1, 31);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(1, 3, 1, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(318, 155);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "图像源";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 43;
            this.label1.Text = "文件类型:";
            // 
            // 文件类型comboBox
            // 
            this.文件类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.文件类型comboBox.FormattingEnabled = true;
            this.文件类型comboBox.Items.AddRange(new object[] {
            "*.png",
            "*.tiff",
            "*.bmp",
            "*.gif",
            "*.jpg",
            "*.jpeg",
            "*.jp2",
            "*.*"});
            this.文件类型comboBox.Location = new System.Drawing.Point(66, 102);
            this.文件类型comboBox.Name = "文件类型comboBox";
            this.文件类型comboBox.Size = new System.Drawing.Size(222, 20);
            this.文件类型comboBox.TabIndex = 42;
            this.文件类型comboBox.Text = "*.bmp";
            this.文件类型comboBox.SelectionChangeCommitted += new System.EventHandler(this.文件类型comboBox_SelectionChangeCommitted);
            // 
            // readDirectoryButton
            // 
            this.readDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.readDirectoryButton.Location = new System.Drawing.Point(292, 73);
            this.readDirectoryButton.Name = "readDirectoryButton";
            this.readDirectoryButton.Size = new System.Drawing.Size(23, 21);
            this.readDirectoryButton.TabIndex = 41;
            this.readDirectoryButton.Text = "……";
            this.readDirectoryButton.UseVisualStyleBackColor = true;
            this.readDirectoryButton.Click += new System.EventHandler(this.readDirectoryButton_Click);
            // 
            // 多文件目录textBox
            // 
            this.多文件目录textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.多文件目录textBox.Location = new System.Drawing.Point(66, 72);
            this.多文件目录textBox.Name = "多文件目录textBox";
            this.多文件目录textBox.Size = new System.Drawing.Size(222, 21);
            this.多文件目录textBox.TabIndex = 40;
            this.多文件目录textBox.TextChanged += new System.EventHandler(this.多文件目录textBox_TextChanged);
            // 
            // 目录源radioButton
            // 
            this.目录源radioButton.AutoSize = true;
            this.目录源radioButton.Location = new System.Drawing.Point(8, 75);
            this.目录源radioButton.Name = "目录源radioButton";
            this.目录源radioButton.Size = new System.Drawing.Size(53, 16);
            this.目录源radioButton.TabIndex = 39;
            this.目录源radioButton.TabStop = true;
            this.目录源radioButton.Text = "目录:";
            this.目录源radioButton.UseVisualStyleBackColor = true;
            // 
            // 拍照点textBox
            // 
            this.拍照点textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.拍照点textBox.Location = new System.Drawing.Point(66, 128);
            this.拍照点textBox.Name = "拍照点textBox";
            this.拍照点textBox.Size = new System.Drawing.Size(222, 21);
            this.拍照点textBox.TabIndex = 38;
            this.拍照点textBox.TextChanged += new System.EventHandler(this.拍照点textBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 37;
            this.label6.Text = "X/Y/R:";
            // 
            // readFileButton
            // 
            this.readFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.readFileButton.Location = new System.Drawing.Point(294, 45);
            this.readFileButton.Name = "readFileButton";
            this.readFileButton.Size = new System.Drawing.Size(21, 21);
            this.readFileButton.TabIndex = 36;
            this.readFileButton.Text = "……";
            this.readFileButton.UseVisualStyleBackColor = true;
            this.readFileButton.Click += new System.EventHandler(this.readFileButton_Click);
            // 
            // 单文件路径textBox
            // 
            this.单文件路径textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.单文件路径textBox.Location = new System.Drawing.Point(66, 45);
            this.单文件路径textBox.Name = "单文件路径textBox";
            this.单文件路径textBox.Size = new System.Drawing.Size(222, 21);
            this.单文件路径textBox.TabIndex = 35;
            // 
            // 采集源comboBox
            // 
            this.采集源comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.采集源comboBox.FormattingEnabled = true;
            this.采集源comboBox.Location = new System.Drawing.Point(66, 20);
            this.采集源comboBox.Name = "采集源comboBox";
            this.采集源comboBox.Size = new System.Drawing.Size(222, 20);
            this.采集源comboBox.TabIndex = 33;
            this.采集源comboBox.SelectedIndexChanged += new System.EventHandler(this.相机采集源comboBox_SelectedIndexChanged);
            this.采集源comboBox.SelectionChangeCommitted += new System.EventHandler(this.采集源comboBox_SelectionChangeCommitted);
            // 
            // 文件源radioButton
            // 
            this.文件源radioButton.AutoSize = true;
            this.文件源radioButton.Location = new System.Drawing.Point(8, 47);
            this.文件源radioButton.Name = "文件源radioButton";
            this.文件源radioButton.Size = new System.Drawing.Size(53, 16);
            this.文件源radioButton.TabIndex = 1;
            this.文件源radioButton.TabStop = true;
            this.文件源radioButton.Text = "文件:";
            this.文件源radioButton.UseVisualStyleBackColor = true;
            this.文件源radioButton.CheckedChanged += new System.EventHandler(this.文件源radioButton_CheckedChanged);
            // 
            // 相机源radioButton
            // 
            this.相机源radioButton.AutoSize = true;
            this.相机源radioButton.Location = new System.Drawing.Point(8, 22);
            this.相机源radioButton.Name = "相机源radioButton";
            this.相机源radioButton.Size = new System.Drawing.Size(53, 16);
            this.相机源radioButton.TabIndex = 0;
            this.相机源radioButton.TabStop = true;
            this.相机源radioButton.Text = "相机:";
            this.相机源radioButton.UseVisualStyleBackColor = true;
            this.相机源radioButton.CheckedChanged += new System.EventHandler(this.相机源radioButton_CheckedChanged);
            // 
            // ImageAcqForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 706);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImageAcqForm";
            this.Text = "图像采集";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageAcqForm_FormClosing);
            this.Load += new System.EventHandler(this.ImageAcqForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.ComboBox 采集源comboBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton 文件源radioButton;
        private System.Windows.Forms.RadioButton 相机源radioButton;
        private System.Windows.Forms.Button readFileButton;
        private System.Windows.Forms.TextBox 单文件路径textBox;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.TextBox 拍照点textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button readDirectoryButton;
        private System.Windows.Forms.TextBox 多文件目录textBox;
        private System.Windows.Forms.RadioButton 目录源radioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 文件类型comboBox;
    }
}