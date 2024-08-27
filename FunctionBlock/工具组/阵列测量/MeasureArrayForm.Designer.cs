namespace FunctionBlock
{
    partial class MeasureArrayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureArrayForm));
            this.miniToolStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.行偏移textBox = new System.Windows.Forms.TextBox();
            this.行阵列数量numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.列阵列数量numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.列偏移textBox = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.程序路径Lable = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip3 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值1Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值2Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值3Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.行坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.列坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.运行工具条toolStrip = new System.Windows.Forms.ToolStrip();
            this.运行toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.停止toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.检测工具toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.保存配置toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.视图tabPage = new System.Windows.Forms.TabPage();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.阵列参数tabPage = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.行阵列数量numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.列阵列数量numericUpDown)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.运行工具条toolStrip.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.视图tabPage.SuspendLayout();
            this.阵列参数tabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AccessibleName = "新项选择";
            this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(682, 30);
            this.miniToolStrip.TabIndex = 14;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(56, 25);
            this.toolStripStatusLabel3.Text = "灰度值：";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(44, 25);
            this.toolStripStatusLabel7.Text = "坐标：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 46);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 4);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(316, 637);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(308, 611);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "程序";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.treeView1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(302, 605);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(0);
            this.treeView1.Name = "treeView1";
            this.tableLayoutPanel3.SetRowSpan(this.treeView1, 3);
            this.treeView1.Size = new System.Drawing.Size(302, 605);
            this.treeView1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.行偏移textBox);
            this.groupBox1.Controls.Add(this.行阵列数量numericUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.列阵列数量numericUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.列偏移textBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(732, 112);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "阵列参数";
            // 
            // 行偏移textBox
            // 
            this.行偏移textBox.Location = new System.Drawing.Point(60, 31);
            this.行偏移textBox.Name = "行偏移textBox";
            this.行偏移textBox.Size = new System.Drawing.Size(83, 21);
            this.行偏移textBox.TabIndex = 52;
            this.行偏移textBox.Text = "0";
            // 
            // 行阵列数量numericUpDown
            // 
            this.行阵列数量numericUpDown.Location = new System.Drawing.Point(208, 74);
            this.行阵列数量numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.行阵列数量numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.行阵列数量numericUpDown.Name = "行阵列数量numericUpDown";
            this.行阵列数量numericUpDown.Size = new System.Drawing.Size(107, 21);
            this.行阵列数量numericUpDown.TabIndex = 58;
            this.行阵列数量numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 51;
            this.label3.Text = "行偏移:";
            // 
            // 列阵列数量numericUpDown
            // 
            this.列阵列数量numericUpDown.Location = new System.Drawing.Point(208, 33);
            this.列阵列数量numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.列阵列数量numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.列阵列数量numericUpDown.Name = "列阵列数量numericUpDown";
            this.列阵列数量numericUpDown.Size = new System.Drawing.Size(107, 21);
            this.列阵列数量numericUpDown.TabIndex = 57;
            this.列阵列数量numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(153, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 53;
            this.label4.Text = "列数量：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(153, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 56;
            this.label5.Text = "行数量：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 54;
            this.label6.Text = "列偏移：";
            // 
            // 列偏移textBox
            // 
            this.列偏移textBox.Location = new System.Drawing.Point(60, 71);
            this.列偏移textBox.Name = "列偏移textBox";
            this.列偏移textBox.Size = new System.Drawing.Size(83, 21);
            this.列偏移textBox.TabIndex = 55;
            this.列偏移textBox.Text = "0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.程序路径Lable,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 683);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(316, 31);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // 程序路径Lable
            // 
            this.程序路径Lable.Name = "程序路径Lable";
            this.程序路径Lable.Size = new System.Drawing.Size(68, 26);
            this.程序路径Lable.Text = "执行结果：";
            this.程序路径Lable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(28, 26);
            this.toolStripStatusLabel2.Text = "……";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(28, 26);
            this.toolStripStatusLabel1.Text = "……";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 316F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 294F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip3, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.运行工具条toolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl2, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1035, 714);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip3, 2);
            this.statusStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel4,
            this.灰度值1Label,
            this.灰度值2Label,
            this.灰度值3Label,
            this.toolStripStatusLabel9,
            this.行坐标Label,
            this.列坐标Label});
            this.statusStrip3.Location = new System.Drawing.Point(316, 683);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(719, 31);
            this.statusStrip3.TabIndex = 20;
            this.statusStrip3.Text = "statusStrip3";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(56, 26);
            this.toolStripStatusLabel4.Text = "灰度值：";
            // 
            // 灰度值1Label
            // 
            this.灰度值1Label.Name = "灰度值1Label";
            this.灰度值1Label.Size = new System.Drawing.Size(28, 26);
            this.灰度值1Label.Text = "……";
            // 
            // 灰度值2Label
            // 
            this.灰度值2Label.Name = "灰度值2Label";
            this.灰度值2Label.Size = new System.Drawing.Size(28, 26);
            this.灰度值2Label.Text = "……";
            // 
            // 灰度值3Label
            // 
            this.灰度值3Label.Name = "灰度值3Label";
            this.灰度值3Label.Size = new System.Drawing.Size(28, 26);
            this.灰度值3Label.Text = "……";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(44, 26);
            this.toolStripStatusLabel9.Text = "坐标：";
            // 
            // 行坐标Label
            // 
            this.行坐标Label.Name = "行坐标Label";
            this.行坐标Label.Size = new System.Drawing.Size(28, 26);
            this.行坐标Label.Text = "……";
            // 
            // 列坐标Label
            // 
            this.列坐标Label.Name = "列坐标Label";
            this.列坐标Label.Size = new System.Drawing.Size(28, 26);
            this.列坐标Label.Text = "……";
            // 
            // 运行工具条toolStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.运行工具条toolStrip, 3);
            this.运行工具条toolStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.运行工具条toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.运行工具条toolStrip.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.运行工具条toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.运行toolStripButton,
            this.停止toolStripButton,
            this.检测工具toolStripButton,
            this.保存配置toolStripButton});
            this.运行工具条toolStrip.Location = new System.Drawing.Point(0, 0);
            this.运行工具条toolStrip.Name = "运行工具条toolStrip";
            this.tableLayoutPanel1.SetRowSpan(this.运行工具条toolStrip, 2);
            this.运行工具条toolStrip.Size = new System.Drawing.Size(1035, 46);
            this.运行工具条toolStrip.TabIndex = 19;
            this.运行工具条toolStrip.Text = "toolStrip2";
            this.运行工具条toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.运行工具条toolStrip_ItemClicked);
            // 
            // 运行toolStripButton
            // 
            this.运行toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.运行toolStripButton.Image = global::FunctionBlock.Properties.Resources.Start;
            this.运行toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.运行toolStripButton.Name = "运行toolStripButton";
            this.运行toolStripButton.Size = new System.Drawing.Size(44, 43);
            this.运行toolStripButton.Text = " 运行 ";
            this.运行toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 停止toolStripButton
            // 
            this.停止toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.停止toolStripButton.Image = global::FunctionBlock.Properties.Resources.Stop;
            this.停止toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.停止toolStripButton.Name = "停止toolStripButton";
            this.停止toolStripButton.Size = new System.Drawing.Size(36, 43);
            this.停止toolStripButton.Text = "停止";
            this.停止toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 检测工具toolStripButton
            // 
            this.检测工具toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.检测工具toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742307_1_;
            this.检测工具toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.检测工具toolStripButton.Name = "检测工具toolStripButton";
            this.检测工具toolStripButton.Size = new System.Drawing.Size(60, 43);
            this.检测工具toolStripButton.Text = "检测工具";
            this.检测工具toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 保存配置toolStripButton
            // 
            this.保存配置toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.保存配置toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742495_1_;
            this.保存配置toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.保存配置toolStripButton.Name = "保存配置toolStripButton";
            this.保存配置toolStripButton.Size = new System.Drawing.Size(60, 43);
            this.保存配置toolStripButton.Text = "保存配置";
            this.保存配置toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tabControl2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl2, 2);
            this.tabControl2.Controls.Add(this.视图tabPage);
            this.tabControl2.Controls.Add(this.阵列参数tabPage);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(316, 46);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl2, 4);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(719, 637);
            this.tabControl2.TabIndex = 21;
            // 
            // 视图tabPage
            // 
            this.视图tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.视图tabPage.Controls.Add(this.hWindowControl1);
            this.视图tabPage.Location = new System.Drawing.Point(4, 22);
            this.视图tabPage.Name = "视图tabPage";
            this.视图tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.视图tabPage.Size = new System.Drawing.Size(711, 611);
            this.视图tabPage.TabIndex = 0;
            this.视图tabPage.Text = "视图";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(705, 605);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(705, 605);
            // 
            // statusStrip2
            // 
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip2.Location = new System.Drawing.Point(0, 0);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(200, 22);
            this.statusStrip2.TabIndex = 0;
            // 
            // 阵列参数tabPage
            // 
            this.阵列参数tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.阵列参数tabPage.Controls.Add(this.groupBox1);
            this.阵列参数tabPage.Location = new System.Drawing.Point(4, 22);
            this.阵列参数tabPage.Name = "阵列参数tabPage";
            this.阵列参数tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.阵列参数tabPage.Size = new System.Drawing.Size(746, 650);
            this.阵列参数tabPage.TabIndex = 1;
            this.阵列参数tabPage.Text = "阵列参数";
            // 
            // MeasureArrayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 714);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MeasureArrayForm";
            this.Text = "阵列执行";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlueDetectForm_FormClosing);
            this.Load += new System.EventHandler(this.MeasureArrayForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.行阵列数量numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.列阵列数量numericUpDown)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
            this.运行工具条toolStrip.ResumeLayout(false);
            this.运行工具条toolStrip.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.视图tabPage.ResumeLayout(false);
            this.阵列参数tabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel 程序路径Lable;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.StatusStrip miniToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStrip 运行工具条toolStrip;
        private System.Windows.Forms.ToolStripButton 运行toolStripButton;
        private System.Windows.Forms.ToolStripButton 停止toolStripButton;
        private System.Windows.Forms.ToolStripButton 检测工具toolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripButton 保存配置toolStripButton;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage 视图tabPage;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.NumericUpDown 行阵列数量numericUpDown;
        private System.Windows.Forms.NumericUpDown 列阵列数量numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox 列偏移textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 行偏移textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage 阵列参数tabPage;
    }
}