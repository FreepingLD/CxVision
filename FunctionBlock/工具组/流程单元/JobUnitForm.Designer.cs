namespace FunctionBlock
{
    partial class JobUnitForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobUnitForm));
            this.miniToolStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
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
            this.PLC交互信息tabPage = new System.Windows.Forms.TabPage();
            this.数据读取dataGridView = new System.Windows.Forms.DataGridView();
            this.CoordSysNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CommunicationCommandCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InseterBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.运行工具条toolStrip.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.视图tabPage.SuspendLayout();
            this.PLC交互信息tabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.数据读取dataGridView)).BeginInit();
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
            this.tabControl1.Size = new System.Drawing.Size(306, 683);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(298, 657);
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
            this.tableLayoutPanel3.Size = new System.Drawing.Size(292, 651);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(0);
            this.treeView1.Name = "treeView1";
            this.tableLayoutPanel3.SetRowSpan(this.treeView1, 3);
            this.treeView1.Size = new System.Drawing.Size(292, 651);
            this.treeView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.程序路径Lable,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 729);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(306, 31);
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 306F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 304F));
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1098, 760);
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
            this.statusStrip3.Location = new System.Drawing.Point(306, 729);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(792, 31);
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
            this.运行工具条toolStrip.Size = new System.Drawing.Size(1098, 46);
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
            this.tabControl2.Controls.Add(this.PLC交互信息tabPage);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(306, 46);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl2, 4);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(792, 683);
            this.tabControl2.TabIndex = 21;
            // 
            // 视图tabPage
            // 
            this.视图tabPage.Controls.Add(this.hWindowControl1);
            this.视图tabPage.Location = new System.Drawing.Point(4, 22);
            this.视图tabPage.Name = "视图tabPage";
            this.视图tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.视图tabPage.Size = new System.Drawing.Size(784, 657);
            this.视图tabPage.TabIndex = 0;
            this.视图tabPage.Text = "视图";
            this.视图tabPage.UseVisualStyleBackColor = true;
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
            this.hWindowControl1.Size = new System.Drawing.Size(778, 651);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(778, 651);
            // 
            // PLC交互信息tabPage
            // 
            this.PLC交互信息tabPage.Controls.Add(this.数据读取dataGridView);
            this.PLC交互信息tabPage.Location = new System.Drawing.Point(4, 22);
            this.PLC交互信息tabPage.Name = "PLC交互信息tabPage";
            this.PLC交互信息tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PLC交互信息tabPage.Size = new System.Drawing.Size(784, 657);
            this.PLC交互信息tabPage.TabIndex = 1;
            this.PLC交互信息tabPage.Text = "PLC交互信息";
            this.PLC交互信息tabPage.UseVisualStyleBackColor = true;
            // 
            // 数据读取dataGridView
            // 
            this.数据读取dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.数据读取dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CoordSysNameColumn,
            this.CommunicationCommandCol,
            this.Column4,
            this.Column5,
            this.Column1,
            this.Column7,
            this.InseterBtn,
            this.DeleteBtn});
            this.数据读取dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.数据读取dataGridView.Location = new System.Drawing.Point(3, 3);
            this.数据读取dataGridView.Name = "数据读取dataGridView";
            this.数据读取dataGridView.RowHeadersWidth = 5;
            this.数据读取dataGridView.RowTemplate.Height = 23;
            this.数据读取dataGridView.Size = new System.Drawing.Size(778, 651);
            this.数据读取dataGridView.TabIndex = 1;
            this.数据读取dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.数据读取dataGridView_CellContentClick);
            // 
            // CoordSysNameColumn
            // 
            this.CoordSysNameColumn.DataPropertyName = "CoordSysName";
            this.CoordSysNameColumn.HeaderText = "坐标系名";
            this.CoordSysNameColumn.MinimumWidth = 6;
            this.CoordSysNameColumn.Name = "CoordSysNameColumn";
            this.CoordSysNameColumn.Width = 125;
            // 
            // CommunicationCommandCol
            // 
            this.CommunicationCommandCol.DataPropertyName = "CommunicationCommand";
            this.CommunicationCommandCol.HeaderText = "通信命令";
            this.CommunicationCommandCol.MinimumWidth = 6;
            this.CommunicationCommandCol.Name = "CommunicationCommandCol";
            this.CommunicationCommandCol.Width = 150;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ReadValue";
            this.Column4.HeaderText = "读取值";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "TargetValue";
            this.Column5.HeaderText = "目标值";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "IsCompare";
            this.Column1.HeaderText = "比较";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "Describe";
            this.Column7.HeaderText = "描述";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Width = 125;
            // 
            // InseterBtn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "插入";
            this.InseterBtn.DefaultCellStyle = dataGridViewCellStyle1;
            this.InseterBtn.HeaderText = "插入";
            this.InseterBtn.MinimumWidth = 6;
            this.InseterBtn.Name = "InseterBtn";
            this.InseterBtn.Width = 60;
            // 
            // DeleteBtn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle2;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.MinimumWidth = 6;
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 60;
            // 
            // statusStrip2
            // 
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip2.Location = new System.Drawing.Point(0, 0);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(200, 22);
            this.statusStrip2.TabIndex = 0;
            // 
            // JobUnitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 760);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JobUnitForm";
            this.Text = "流程单元";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FeatureLocalizationForm_FormClosing);
            this.Load += new System.EventHandler(this.JobUnitForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
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
            this.PLC交互信息tabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.数据读取dataGridView)).EndInit();
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
        private System.Windows.Forms.TabPage PLC交互信息tabPage;
        private System.Windows.Forms.DataGridView 数据读取dataGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn CoordSysNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn CommunicationCommandCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewButtonColumn InseterBtn;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
    }
}