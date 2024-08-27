namespace FunctionBlock
{
    partial class TrackCalculateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackCalculateForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.参数groupBox = new System.Windows.Forms.GroupBox();
            this.插值间隔comboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.启用轮廓插值checkBox = new System.Windows.Forms.CheckBox();
            this.Z补偿comboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Y补偿comboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.X补偿comboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Y轴旋转角comboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.X轴旋转角comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.可视化法向轮廓checkBox = new System.Windows.Forms.CheckBox();
            this.引导对象comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.偏移距离comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.法向量计算方法comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Z轴旋转角comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.排序方法comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
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
            this.运行toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStripButton();
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
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.参数groupBox.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.运行toolStrip.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 251F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.显示条目comboBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.运行toolStrip, 0, 0);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1086, 727);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 31);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 4);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(354, 663);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(346, 637);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.参数groupBox, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(340, 631);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 250);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "轨迹点";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 17);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(334, 230);
            this.listBox1.TabIndex = 0;
            // 
            // 参数groupBox
            // 
            this.参数groupBox.Controls.Add(this.插值间隔comboBox);
            this.参数groupBox.Controls.Add(this.label11);
            this.参数groupBox.Controls.Add(this.启用轮廓插值checkBox);
            this.参数groupBox.Controls.Add(this.Z补偿comboBox);
            this.参数groupBox.Controls.Add(this.label10);
            this.参数groupBox.Controls.Add(this.Y补偿comboBox);
            this.参数groupBox.Controls.Add(this.label9);
            this.参数groupBox.Controls.Add(this.X补偿comboBox);
            this.参数groupBox.Controls.Add(this.label8);
            this.参数groupBox.Controls.Add(this.Y轴旋转角comboBox);
            this.参数groupBox.Controls.Add(this.label7);
            this.参数groupBox.Controls.Add(this.X轴旋转角comboBox);
            this.参数groupBox.Controls.Add(this.label5);
            this.参数groupBox.Controls.Add(this.可视化法向轮廓checkBox);
            this.参数groupBox.Controls.Add(this.引导对象comboBox);
            this.参数groupBox.Controls.Add(this.label4);
            this.参数groupBox.Controls.Add(this.偏移距离comboBox);
            this.参数groupBox.Controls.Add(this.label3);
            this.参数groupBox.Controls.Add(this.法向量计算方法comboBox);
            this.参数groupBox.Controls.Add(this.label2);
            this.参数groupBox.Controls.Add(this.Z轴旋转角comboBox);
            this.参数groupBox.Controls.Add(this.label1);
            this.参数groupBox.Controls.Add(this.排序方法comboBox);
            this.参数groupBox.Controls.Add(this.label6);
            this.参数groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.参数groupBox.Location = new System.Drawing.Point(0, 250);
            this.参数groupBox.Margin = new System.Windows.Forms.Padding(0);
            this.参数groupBox.Name = "参数groupBox";
            this.tableLayoutPanel2.SetRowSpan(this.参数groupBox, 2);
            this.参数groupBox.Size = new System.Drawing.Size(340, 381);
            this.参数groupBox.TabIndex = 3;
            this.参数groupBox.TabStop = false;
            this.参数groupBox.Text = "参数:";
            // 
            // 插值间隔comboBox
            // 
            this.插值间隔comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.插值间隔comboBox.FormattingEnabled = true;
            this.插值间隔comboBox.Items.AddRange(new object[] {
            "0",
            "0.01",
            "0.05",
            "0.1"});
            this.插值间隔comboBox.Location = new System.Drawing.Point(123, 311);
            this.插值间隔comboBox.Name = "插值间隔comboBox";
            this.插值间隔comboBox.Size = new System.Drawing.Size(211, 20);
            this.插值间隔comboBox.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(61, 315);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 32;
            this.label11.Text = "插值间隔：";
            // 
            // 启用轮廓插值checkBox
            // 
            this.启用轮廓插值checkBox.AutoSize = true;
            this.启用轮廓插值checkBox.Location = new System.Drawing.Point(123, 359);
            this.启用轮廓插值checkBox.Name = "启用轮廓插值checkBox";
            this.启用轮廓插值checkBox.Size = new System.Drawing.Size(96, 16);
            this.启用轮廓插值checkBox.TabIndex = 31;
            this.启用轮廓插值checkBox.Text = "启用轮廓插值";
            this.启用轮廓插值checkBox.UseVisualStyleBackColor = true;
            // 
            // Z补偿comboBox
            // 
            this.Z补偿comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Z补偿comboBox.FormattingEnabled = true;
            this.Z补偿comboBox.Items.AddRange(new object[] {
            "0",
            "0.01",
            "0.05",
            "0.1"});
            this.Z补偿comboBox.Location = new System.Drawing.Point(123, 282);
            this.Z补偿comboBox.Name = "Z补偿comboBox";
            this.Z补偿comboBox.Size = new System.Drawing.Size(211, 20);
            this.Z补偿comboBox.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(78, 286);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 29;
            this.label10.Text = "Z补偿：";
            // 
            // Y补偿comboBox
            // 
            this.Y补偿comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Y补偿comboBox.FormattingEnabled = true;
            this.Y补偿comboBox.Items.AddRange(new object[] {
            "0",
            "0.01",
            "0.05",
            "0.1"});
            this.Y补偿comboBox.Location = new System.Drawing.Point(123, 256);
            this.Y补偿comboBox.Name = "Y补偿comboBox";
            this.Y补偿comboBox.Size = new System.Drawing.Size(211, 20);
            this.Y补偿comboBox.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(78, 260);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 27;
            this.label9.Text = "Y补偿：";
            // 
            // X补偿comboBox
            // 
            this.X补偿comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.X补偿comboBox.FormattingEnabled = true;
            this.X补偿comboBox.Items.AddRange(new object[] {
            "0",
            "0.01",
            "0.05",
            "0.1"});
            this.X补偿comboBox.Location = new System.Drawing.Point(123, 230);
            this.X补偿comboBox.Name = "X补偿comboBox";
            this.X补偿comboBox.Size = new System.Drawing.Size(211, 20);
            this.X补偿comboBox.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(78, 234);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "X补偿：";
            // 
            // Y轴旋转角comboBox
            // 
            this.Y轴旋转角comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Y轴旋转角comboBox.FormattingEnabled = true;
            this.Y轴旋转角comboBox.Items.AddRange(new object[] {
            "auto",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.Y轴旋转角comboBox.Location = new System.Drawing.Point(123, 114);
            this.Y轴旋转角comboBox.Name = "Y轴旋转角comboBox";
            this.Y轴旋转角comboBox.Size = new System.Drawing.Size(211, 20);
            this.Y轴旋转角comboBox.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(54, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "Y轴旋转角：";
            // 
            // X轴旋转角comboBox
            // 
            this.X轴旋转角comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.X轴旋转角comboBox.FormattingEnabled = true;
            this.X轴旋转角comboBox.Items.AddRange(new object[] {
            "auto",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.X轴旋转角comboBox.Location = new System.Drawing.Point(123, 84);
            this.X轴旋转角comboBox.Name = "X轴旋转角comboBox";
            this.X轴旋转角comboBox.Size = new System.Drawing.Size(211, 20);
            this.X轴旋转角comboBox.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "X轴旋转角：";
            // 
            // 可视化法向轮廓checkBox
            // 
            this.可视化法向轮廓checkBox.AutoSize = true;
            this.可视化法向轮廓checkBox.Location = new System.Drawing.Point(123, 337);
            this.可视化法向轮廓checkBox.Name = "可视化法向轮廓checkBox";
            this.可视化法向轮廓checkBox.Size = new System.Drawing.Size(108, 16);
            this.可视化法向轮廓checkBox.TabIndex = 20;
            this.可视化法向轮廓checkBox.Text = "可视化法向轮廓";
            this.可视化法向轮廓checkBox.UseVisualStyleBackColor = true;
            // 
            // 引导对象comboBox
            // 
            this.引导对象comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.引导对象comboBox.FormattingEnabled = true;
            this.引导对象comboBox.Items.AddRange(new object[] {
            "视野中心",
            "示教点"});
            this.引导对象comboBox.Location = new System.Drawing.Point(123, 24);
            this.引导对象comboBox.Name = "引导对象comboBox";
            this.引导对象comboBox.Size = new System.Drawing.Size(211, 20);
            this.引导对象comboBox.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "引导对象：";
            // 
            // 偏移距离comboBox
            // 
            this.偏移距离comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.偏移距离comboBox.FormattingEnabled = true;
            this.偏移距离comboBox.Items.AddRange(new object[] {
            "0.01",
            "0.05",
            "0.1"});
            this.偏移距离comboBox.Location = new System.Drawing.Point(123, 204);
            this.偏移距离comboBox.Name = "偏移距离comboBox";
            this.偏移距离comboBox.Size = new System.Drawing.Size(211, 20);
            this.偏移距离comboBox.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "法向量计算偏移距离：";
            // 
            // 法向量计算方法comboBox
            // 
            this.法向量计算方法comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.法向量计算方法comboBox.FormattingEnabled = true;
            this.法向量计算方法comboBox.Items.AddRange(new object[] {
            "regression_normal",
            "gradient",
            "contour_normal"});
            this.法向量计算方法comboBox.Location = new System.Drawing.Point(123, 174);
            this.法向量计算方法comboBox.Name = "法向量计算方法comboBox";
            this.法向量计算方法comboBox.Size = new System.Drawing.Size(211, 20);
            this.法向量计算方法comboBox.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "法向量计算方法：";
            // 
            // Z轴旋转角comboBox
            // 
            this.Z轴旋转角comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Z轴旋转角comboBox.FormattingEnabled = true;
            this.Z轴旋转角comboBox.Items.AddRange(new object[] {
            "auto",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.Z轴旋转角comboBox.Location = new System.Drawing.Point(123, 144);
            this.Z轴旋转角comboBox.Name = "Z轴旋转角comboBox";
            this.Z轴旋转角comboBox.Size = new System.Drawing.Size(211, 20);
            this.Z轴旋转角comboBox.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "Z轴旋转角：";
            // 
            // 排序方法comboBox
            // 
            this.排序方法comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.排序方法comboBox.FormattingEnabled = true;
            this.排序方法comboBox.Items.AddRange(new object[] {
            "逆时针",
            "顺时针"});
            this.排序方法comboBox.Location = new System.Drawing.Point(123, 54);
            this.排序方法comboBox.Name = "排序方法comboBox";
            this.排序方法comboBox.Size = new System.Drawing.Size(211, 20);
            this.排序方法comboBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "排序方法：";
            // 
            // 显示条目comboBox
            // 
            this.显示条目comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.显示条目comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.显示条目comboBox.FormattingEnabled = true;
            this.显示条目comboBox.Location = new System.Drawing.Point(838, 3);
            this.显示条目comboBox.Name = "显示条目comboBox";
            this.显示条目comboBox.Size = new System.Drawing.Size(245, 20);
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
            this.视图工具toolStrip.Size = new System.Drawing.Size(475, 28);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 697);
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
            // 运行toolStrip
            // 
            this.运行toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.运行toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStrip1});
            this.运行toolStrip.Location = new System.Drawing.Point(0, 0);
            this.运行toolStrip.Name = "运行toolStrip";
            this.运行toolStrip.Size = new System.Drawing.Size(360, 28);
            this.运行toolStrip.TabIndex = 13;
            this.运行toolStrip.Text = "toolStrip1";
            this.运行toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.运行toolStrip_ItemClicked);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Image = global::FunctionBlock.Properties.Resources.Start;
            this.toolStrip1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(52, 25);
            this.toolStrip1.Text = "执行";
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
            this.statusStrip2.Location = new System.Drawing.Point(360, 697);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(726, 30);
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
            this.hWindowControl1.Size = new System.Drawing.Size(720, 663);
            this.hWindowControl1.TabIndex = 17;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(720, 663);
            // 
            // TrackCalculateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 727);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TrackCalculateForm";
            this.Text = "轨迹计算";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LineOffsetForm_FormClosing);
            this.Load += new System.EventHandler(this.TrackCalculateForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.参数groupBox.ResumeLayout(false);
            this.参数groupBox.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.运行toolStrip.ResumeLayout(false);
            this.运行toolStrip.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox 显示条目comboBox;
        public System.Windows.Forms.ToolStrip 运行toolStrip;
        private System.Windows.Forms.ToolStripButton toolStrip1;
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
        private System.Windows.Forms.ToolStripButton toolStripButton_3D;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.GroupBox 参数groupBox;
        private System.Windows.Forms.ComboBox 排序方法comboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Z轴旋转角comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 法向量计算方法comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 偏移距离comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox 可视化法向轮廓checkBox;
        private System.Windows.Forms.ComboBox 引导对象comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Y轴旋转角comboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox X轴旋转角comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Z补偿comboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox Y补偿comboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox X补偿comboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox 启用轮廓插值checkBox;
        private System.Windows.Forms.ComboBox 插值间隔comboBox;
        private System.Windows.Forms.Label label11;
    }
}