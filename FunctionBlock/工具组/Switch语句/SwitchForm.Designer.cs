namespace FunctionBlock
{
    partial class SwitchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SwitchForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
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
            this.脚本配置toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.视图tabPage = new System.Windows.Forms.TabPage();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.PLC交互信息tabPage = new System.Windows.Forms.TabPage();
            this.数据写入dataGridView = new System.Windows.Forms.DataGridView();
            this.IsActiveCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CooreSysNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CommunicationCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsOutCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InsertBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.UpMoveCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DownMoveCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.元素信息tabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.运行工具条toolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.视图tabPage.SuspendLayout();
            this.PLC交互信息tabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.数据写入dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 305F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 305F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMax, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMin, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip3, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.运行工具条toolStrip, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1047, 731);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(1022, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(25, 25);
            this.buttonClose.TabIndex = 31;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(997, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(25, 25);
            this.buttonMax.TabIndex = 30;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Location = new System.Drawing.Point(972, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(25, 25);
            this.buttonMin.TabIndex = 29;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 3);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(968, 21);
            this.titleLabel.TabIndex = 28;
            this.titleLabel.Text = "开关语句<Switch>";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // statusStrip3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip3, 5);
            this.statusStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel4,
            this.灰度值1Label,
            this.灰度值2Label,
            this.灰度值3Label,
            this.toolStripStatusLabel9,
            this.行坐标Label,
            this.列坐标Label});
            this.statusStrip3.Location = new System.Drawing.Point(305, 701);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(742, 30);
            this.statusStrip3.TabIndex = 22;
            this.statusStrip3.Text = "statusStrip3";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(56, 25);
            this.toolStripStatusLabel4.Text = "灰度值：";
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
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(44, 25);
            this.toolStripStatusLabel9.Text = "坐标：";
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
            // 运行工具条toolStrip
            // 
            this.运行工具条toolStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.运行工具条toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.运行工具条toolStrip.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.运行工具条toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.运行toolStripButton,
            this.停止toolStripButton,
            this.检测工具toolStripButton,
            this.脚本配置toolStripButton});
            this.运行工具条toolStrip.Location = new System.Drawing.Point(0, 25);
            this.运行工具条toolStrip.Name = "运行工具条toolStrip";
            this.运行工具条toolStrip.Size = new System.Drawing.Size(305, 48);
            this.运行工具条toolStrip.TabIndex = 20;
            this.运行工具条toolStrip.Text = "toolStrip2";
            this.运行工具条toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.运行工具条toolStrip_ItemClicked);
            // 
            // 运行toolStripButton
            // 
            this.运行toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.运行toolStripButton.Image = global::FunctionBlock.Properties.Resources.Start;
            this.运行toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.运行toolStripButton.Name = "运行toolStripButton";
            this.运行toolStripButton.Size = new System.Drawing.Size(36, 45);
            this.运行toolStripButton.Text = "运行";
            this.运行toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 停止toolStripButton
            // 
            this.停止toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.停止toolStripButton.Image = global::FunctionBlock.Properties.Resources.Stop;
            this.停止toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.停止toolStripButton.Name = "停止toolStripButton";
            this.停止toolStripButton.Size = new System.Drawing.Size(36, 45);
            this.停止toolStripButton.Text = "停止";
            this.停止toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 检测工具toolStripButton
            // 
            this.检测工具toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.检测工具toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742307_1_;
            this.检测工具toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.检测工具toolStripButton.Name = "检测工具toolStripButton";
            this.检测工具toolStripButton.Size = new System.Drawing.Size(60, 45);
            this.检测工具toolStripButton.Text = "检测工具";
            this.检测工具toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 脚本配置toolStripButton
            // 
            this.脚本配置toolStripButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.脚本配置toolStripButton.Image = global::FunctionBlock.Properties.Resources.脚本25_X_25_png;
            this.脚本配置toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.脚本配置toolStripButton.Name = "脚本配置toolStripButton";
            this.脚本配置toolStripButton.Size = new System.Drawing.Size(60, 45);
            this.脚本配置toolStripButton.Text = "脚本配置";
            this.脚本配置toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 701);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(305, 30);
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
            this.tabControl1.Location = new System.Drawing.Point(0, 73);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(305, 628);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(297, 602);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "程序配置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.treeView1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(291, 596);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(0);
            this.treeView1.Name = "treeView1";
            this.tableLayoutPanel2.SetRowSpan(this.treeView1, 4);
            this.treeView1.Size = new System.Drawing.Size(291, 596);
            this.treeView1.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl2, 5);
            this.tabControl2.Controls.Add(this.视图tabPage);
            this.tabControl2.Controls.Add(this.PLC交互信息tabPage);
            this.tabControl2.Controls.Add(this.元素信息tabPage);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(305, 25);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl2, 4);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(742, 676);
            this.tabControl2.TabIndex = 23;
            // 
            // 视图tabPage
            // 
            this.视图tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.视图tabPage.Controls.Add(this.hWindowControl1);
            this.视图tabPage.Location = new System.Drawing.Point(4, 22);
            this.视图tabPage.Name = "视图tabPage";
            this.视图tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.视图tabPage.Size = new System.Drawing.Size(734, 650);
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
            this.hWindowControl1.Size = new System.Drawing.Size(728, 644);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(728, 644);
            // 
            // PLC交互信息tabPage
            // 
            this.PLC交互信息tabPage.Controls.Add(this.数据写入dataGridView);
            this.PLC交互信息tabPage.Location = new System.Drawing.Point(4, 22);
            this.PLC交互信息tabPage.Name = "PLC交互信息tabPage";
            this.PLC交互信息tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PLC交互信息tabPage.Size = new System.Drawing.Size(734, 602);
            this.PLC交互信息tabPage.TabIndex = 1;
            this.PLC交互信息tabPage.Text = "PLC交互信息";
            this.PLC交互信息tabPage.UseVisualStyleBackColor = true;
            // 
            // 数据写入dataGridView
            // 
            this.数据写入dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.数据写入dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsActiveCol,
            this.CooreSysNameColumn,
            this.CommunicationCol,
            this.dataGridViewTextBoxColumn3,
            this.IsOutCol,
            this.InsertBtn,
            this.UpMoveCol,
            this.DownMoveCol,
            this.DeleteBtn});
            this.数据写入dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.数据写入dataGridView.Location = new System.Drawing.Point(3, 3);
            this.数据写入dataGridView.Name = "数据写入dataGridView";
            this.数据写入dataGridView.RowHeadersWidth = 5;
            this.数据写入dataGridView.RowTemplate.Height = 23;
            this.数据写入dataGridView.Size = new System.Drawing.Size(728, 596);
            this.数据写入dataGridView.TabIndex = 2;
            this.数据写入dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.数据读取dataGridView_CellContentClick);
            // 
            // IsActiveCol
            // 
            this.IsActiveCol.DataPropertyName = "IsActive";
            this.IsActiveCol.HeaderText = "激活";
            this.IsActiveCol.Name = "IsActiveCol";
            this.IsActiveCol.Width = 40;
            // 
            // CooreSysNameColumn
            // 
            this.CooreSysNameColumn.DataPropertyName = "CoordSysName";
            this.CooreSysNameColumn.HeaderText = "坐标系名";
            this.CooreSysNameColumn.MinimumWidth = 6;
            this.CooreSysNameColumn.Name = "CooreSysNameColumn";
            this.CooreSysNameColumn.Width = 125;
            // 
            // CommunicationCol
            // 
            this.CommunicationCol.DataPropertyName = "CommunicationCommand";
            this.CommunicationCol.HeaderText = "通信命令";
            this.CommunicationCol.MinimumWidth = 6;
            this.CommunicationCol.Name = "CommunicationCol";
            this.CommunicationCol.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ReadValue";
            this.dataGridViewTextBoxColumn3.HeaderText = "读取值";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // IsOutCol
            // 
            this.IsOutCol.DataPropertyName = "IsOutput";
            this.IsOutCol.HeaderText = "输出";
            this.IsOutCol.Name = "IsOutCol";
            this.IsOutCol.Width = 50;
            // 
            // InsertBtn
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "插入";
            this.InsertBtn.DefaultCellStyle = dataGridViewCellStyle5;
            this.InsertBtn.HeaderText = "插入";
            this.InsertBtn.MinimumWidth = 6;
            this.InsertBtn.Name = "InsertBtn";
            this.InsertBtn.Width = 60;
            // 
            // UpMoveCol
            // 
            this.UpMoveCol.DataPropertyName = "NONE";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = "上移";
            this.UpMoveCol.DefaultCellStyle = dataGridViewCellStyle6;
            this.UpMoveCol.HeaderText = "上移";
            this.UpMoveCol.Name = "UpMoveCol";
            this.UpMoveCol.Width = 60;
            // 
            // DownMoveCol
            // 
            this.DownMoveCol.DataPropertyName = "NONE";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = "下移";
            this.DownMoveCol.DefaultCellStyle = dataGridViewCellStyle7;
            this.DownMoveCol.HeaderText = "下移";
            this.DownMoveCol.Name = "DownMoveCol";
            this.DownMoveCol.Width = 60;
            // 
            // DeleteBtn
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle8;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.MinimumWidth = 6;
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 60;
            // 
            // 元素信息tabPage
            // 
            this.元素信息tabPage.Location = new System.Drawing.Point(4, 22);
            this.元素信息tabPage.Name = "元素信息tabPage";
            this.元素信息tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.元素信息tabPage.Size = new System.Drawing.Size(734, 602);
            this.元素信息tabPage.TabIndex = 2;
            this.元素信息tabPage.Text = "元素信息";
            this.元素信息tabPage.UseVisualStyleBackColor = true;
            // 
            // SwitchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 731);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SwitchForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConcurrentExecutionForm_FormClosing);
            this.Load += new System.EventHandler(this.ForLoopControlForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SwitchForm_MouseDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
            this.运行工具条toolStrip.ResumeLayout(false);
            this.运行工具条toolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.视图tabPage.ResumeLayout(false);
            this.PLC交互信息tabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.数据写入dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip 运行工具条toolStrip;
        private System.Windows.Forms.ToolStripButton 运行toolStripButton;
        private System.Windows.Forms.ToolStripButton 停止toolStripButton;
        private System.Windows.Forms.ToolStripButton 检测工具toolStripButton;
        private System.Windows.Forms.ToolStripButton 脚本配置toolStripButton;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage 视图tabPage;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TabPage PLC交互信息tabPage;
        private System.Windows.Forms.DataGridView 数据写入dataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActiveCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn CooreSysNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn CommunicationCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsOutCol;
        private System.Windows.Forms.DataGridViewButtonColumn InsertBtn;
        private System.Windows.Forms.DataGridViewButtonColumn UpMoveCol;
        private System.Windows.Forms.DataGridViewButtonColumn DownMoveCol;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TabPage 元素信息tabPage;
    }
}