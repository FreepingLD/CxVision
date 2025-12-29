namespace FunctionBlock
{
    partial class FlawDetectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlawDetectForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.miniToolStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.程序路径Lable = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.区域选择参数groupBox = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.重命名button = new System.Windows.Forms.Button();
            this.删除检测项button = new System.Windows.Forms.Button();
            this.添加检测项button = new System.Windows.Forms.Button();
            this.显示条目comboBox = new System.Windows.Forms.ComboBox();
            this.视图工具toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Translate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Auto = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_3D = new System.Windows.Forms.ToolStripButton();
            this.检测参数groupBox = new System.Windows.Forms.GroupBox();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ShapeCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.OperateCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.WcsRect2Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeachCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeletCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.滤波参数groupBox = new System.Windows.Forms.GroupBox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.控制参数groupBox = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.图像加载button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.执行button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.检测项listBox = new System.Windows.Forms.ListBox();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.控制参数groupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.程序路径Lable,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 755);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(172, 31);
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
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 358F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 184F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 254F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 201F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.区域选择参数groupBox, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.显示条目comboBox, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.检测参数groupBox, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMax, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMin, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip3, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.滤波参数groupBox, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.控制参数groupBox, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 195F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1411, 786);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // 区域选择参数groupBox
            // 
            this.区域选择参数groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.区域选择参数groupBox.Location = new System.Drawing.Point(968, 525);
            this.区域选择参数groupBox.Margin = new System.Windows.Forms.Padding(0);
            this.区域选择参数groupBox.Name = "区域选择参数groupBox";
            this.tableLayoutPanel1.SetRowSpan(this.区域选择参数groupBox, 2);
            this.区域选择参数groupBox.Size = new System.Drawing.Size(201, 230);
            this.区域选择参数groupBox.TabIndex = 44;
            this.区域选择参数groupBox.TabStop = false;
            this.区域选择参数groupBox.Text = "区域选择参数";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.重命名button);
            this.panel1.Controls.Add(this.删除检测项button);
            this.panel1.Controls.Add(this.添加检测项button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 723);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 29);
            this.panel1.TabIndex = 41;
            // 
            // 重命名button
            // 
            this.重命名button.Location = new System.Drawing.Point(5, 1);
            this.重命名button.Name = "重命名button";
            this.重命名button.Size = new System.Drawing.Size(56, 25);
            this.重命名button.TabIndex = 3;
            this.重命名button.Text = "重命名";
            this.重命名button.UseVisualStyleBackColor = true;
            this.重命名button.Click += new System.EventHandler(this.重命名button_Click);
            // 
            // 删除检测项button
            // 
            this.删除检测项button.Location = new System.Drawing.Point(67, 1);
            this.删除检测项button.Name = "删除检测项button";
            this.删除检测项button.Size = new System.Drawing.Size(43, 25);
            this.删除检测项button.TabIndex = 2;
            this.删除检测项button.Text = "删除检测项";
            this.删除检测项button.UseVisualStyleBackColor = true;
            this.删除检测项button.Click += new System.EventHandler(this.删除检测项button_Click);
            // 
            // 添加检测项button
            // 
            this.添加检测项button.Location = new System.Drawing.Point(116, 1);
            this.添加检测项button.Name = "添加检测项button";
            this.添加检测项button.Size = new System.Drawing.Size(43, 25);
            this.添加检测项button.TabIndex = 1;
            this.添加检测项button.Text = "添加检测项";
            this.添加检测项button.UseVisualStyleBackColor = true;
            this.添加检测项button.Click += new System.EventHandler(this.添加检测项button_Click);
            // 
            // 显示条目comboBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.显示条目comboBox, 4);
            this.显示条目comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.显示条目comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.显示条目comboBox.FormattingEnabled = true;
            this.显示条目comboBox.Items.AddRange(new object[] {
            "源图像",
            "区域"});
            this.显示条目comboBox.Location = new System.Drawing.Point(1170, 26);
            this.显示条目comboBox.Margin = new System.Windows.Forms.Padding(1);
            this.显示条目comboBox.Name = "显示条目comboBox";
            this.显示条目comboBox.Size = new System.Drawing.Size(240, 20);
            this.显示条目comboBox.TabIndex = 40;
            // 
            // 视图工具toolStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.视图工具toolStrip, 5);
            this.视图工具toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.视图工具toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.视图工具toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Clear,
            this.toolStripButton_Select,
            this.toolStripButton_Translate,
            this.toolStripButton_Auto,
            this.toolStripButton_3D});
            this.视图工具toolStrip.Location = new System.Drawing.Point(0, 25);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(1169, 27);
            this.视图工具toolStrip.TabIndex = 39;
            this.视图工具toolStrip.Text = "toolStrip2";
            this.视图工具toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.Image = global::FunctionBlock.Properties.Resources.清除;
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(56, 24);
            this.toolStripButton_Clear.Text = "清除";
            // 
            // toolStripButton_Select
            // 
            this.toolStripButton_Select.Image = global::FunctionBlock.Properties.Resources.选择光标;
            this.toolStripButton_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Select.Name = "toolStripButton_Select";
            this.toolStripButton_Select.Size = new System.Drawing.Size(56, 24);
            this.toolStripButton_Select.Text = "选择";
            // 
            // toolStripButton_Translate
            // 
            this.toolStripButton_Translate.Image = global::FunctionBlock.Properties.Resources.移动光标;
            this.toolStripButton_Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Translate.Name = "toolStripButton_Translate";
            this.toolStripButton_Translate.Size = new System.Drawing.Size(56, 24);
            this.toolStripButton_Translate.Text = "平移";
            // 
            // toolStripButton_Auto
            // 
            this.toolStripButton_Auto.Image = global::FunctionBlock.Properties.Resources.适配窗口光标;
            this.toolStripButton_Auto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Auto.Name = "toolStripButton_Auto";
            this.toolStripButton_Auto.Size = new System.Drawing.Size(80, 24);
            this.toolStripButton_Auto.Text = "适应窗口";
            // 
            // toolStripButton_3D
            // 
            this.toolStripButton_3D.Image = global::FunctionBlock.Properties.Resources._3D;
            this.toolStripButton_3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_3D.Name = "toolStripButton_3D";
            this.toolStripButton_3D.Size = new System.Drawing.Size(48, 24);
            this.toolStripButton_3D.Text = "3D";
            // 
            // 检测参数groupBox
            // 
            this.检测参数groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.检测参数groupBox.Location = new System.Drawing.Point(714, 525);
            this.检测参数groupBox.Margin = new System.Windows.Forms.Padding(0);
            this.检测参数groupBox.Name = "检测参数groupBox";
            this.tableLayoutPanel1.SetRowSpan(this.检测参数groupBox, 2);
            this.检测参数groupBox.Size = new System.Drawing.Size(254, 230);
            this.检测参数groupBox.TabIndex = 38;
            this.检测参数groupBox.TabStop = false;
            this.检测参数groupBox.Text = "检测参数";
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(1386, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(25, 25);
            this.buttonClose.TabIndex = 35;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(1361, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(25, 25);
            this.buttonMax.TabIndex = 29;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Location = new System.Drawing.Point(1336, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(25, 25);
            this.buttonMin.TabIndex = 28;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
            this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 6);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(1332, 21);
            this.titleLabel.TabIndex = 27;
            this.titleLabel.Text = "缺陷检测";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // statusStrip3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip3, 8);
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
            this.statusStrip3.Location = new System.Drawing.Point(172, 755);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(1239, 31);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(175, 528);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(352, 224);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "检测区域";
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ShapeCol,
            this.OperateCol,
            this.WcsRect2Col,
            this.TeachCol,
            this.DeletCol});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(346, 204);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // ShapeCol
            // 
            this.ShapeCol.DataPropertyName = "ShapeType";
            this.ShapeCol.HeaderText = "形状类型";
            this.ShapeCol.Name = "ShapeCol";
            this.ShapeCol.Width = 80;
            // 
            // OperateCol
            // 
            this.OperateCol.DataPropertyName = "InsideOrOutside";
            this.OperateCol.HeaderText = "操作类型";
            this.OperateCol.Name = "OperateCol";
            this.OperateCol.Width = 80;
            // 
            // WcsRect2Col
            // 
            this.WcsRect2Col.DataPropertyName = "RoiShape";
            this.WcsRect2Col.HeaderText = "形状参数";
            this.WcsRect2Col.Name = "WcsRect2Col";
            this.WcsRect2Col.Width = 80;
            // 
            // TeachCol
            // 
            this.TeachCol.DataPropertyName = "NONE";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "示教";
            this.TeachCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.TeachCol.HeaderText = "示教";
            this.TeachCol.Name = "TeachCol";
            this.TeachCol.Width = 50;
            // 
            // DeletCol
            // 
            this.DeletCol.DataPropertyName = "NONE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "删除";
            this.DeletCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.DeletCol.HeaderText = "删除";
            this.DeletCol.Name = "DeletCol";
            this.DeletCol.Width = 50;
            // 
            // 滤波参数groupBox
            // 
            this.滤波参数groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.滤波参数groupBox.Location = new System.Drawing.Point(530, 525);
            this.滤波参数groupBox.Margin = new System.Windows.Forms.Padding(0);
            this.滤波参数groupBox.Name = "滤波参数groupBox";
            this.tableLayoutPanel1.SetRowSpan(this.滤波参数groupBox, 2);
            this.滤波参数groupBox.Size = new System.Drawing.Size(184, 230);
            this.滤波参数groupBox.TabIndex = 37;
            this.滤波参数groupBox.TabStop = false;
            this.滤波参数groupBox.Text = "滤波参数";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 9);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 52);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Size = new System.Drawing.Size(1411, 473);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(1411, 473);
            // 
            // 控制参数groupBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.控制参数groupBox, 4);
            this.控制参数groupBox.Controls.Add(this.button3);
            this.控制参数groupBox.Controls.Add(this.button2);
            this.控制参数groupBox.Controls.Add(this.图像加载button);
            this.控制参数groupBox.Controls.Add(this.button1);
            this.控制参数groupBox.Controls.Add(this.执行button);
            this.控制参数groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.控制参数groupBox.Location = new System.Drawing.Point(1169, 525);
            this.控制参数groupBox.Margin = new System.Windows.Forms.Padding(0);
            this.控制参数groupBox.Name = "控制参数groupBox";
            this.tableLayoutPanel1.SetRowSpan(this.控制参数groupBox, 2);
            this.控制参数groupBox.Size = new System.Drawing.Size(242, 230);
            this.控制参数groupBox.TabIndex = 42;
            this.控制参数groupBox.TabStop = false;
            this.控制参数groupBox.Text = "控制参数";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(114, 165);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 36);
            this.button3.TabIndex = 4;
            this.button3.Text = "下一张 >>";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(6, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 3;
            this.button2.Text = "<< 上一张";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // 图像加载button
            // 
            this.图像加载button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.图像加载button.Location = new System.Drawing.Point(6, 26);
            this.图像加载button.Name = "图像加载button";
            this.图像加载button.Size = new System.Drawing.Size(75, 36);
            this.图像加载button.TabIndex = 2;
            this.图像加载button.Text = "图像加载";
            this.图像加载button.UseVisualStyleBackColor = true;
            this.图像加载button.Click += new System.EventHandler(this.图像加载button_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(6, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 1;
            this.button1.Text = "分类参数";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // 执行button
            // 
            this.执行button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.执行button.Location = new System.Drawing.Point(6, 123);
            this.执行button.Name = "执行button";
            this.执行button.Size = new System.Drawing.Size(75, 36);
            this.执行button.TabIndex = 0;
            this.执行button.Text = "执行";
            this.执行button.UseVisualStyleBackColor = true;
            this.执行button.Click += new System.EventHandler(this.执行button_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.检测项listBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 528);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(166, 189);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "检测项";
            // 
            // 检测项listBox
            // 
            this.检测项listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.检测项listBox.FormattingEnabled = true;
            this.检测项listBox.ItemHeight = 12;
            this.检测项listBox.Location = new System.Drawing.Point(3, 17);
            this.检测项listBox.Name = "检测项listBox";
            this.检测项listBox.Size = new System.Drawing.Size(160, 169);
            this.检测项listBox.TabIndex = 0;
            this.检测项listBox.SelectedIndexChanged += new System.EventHandler(this.检测项listBox_SelectedIndexChanged);
            // 
            // statusStrip2
            // 
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip2.Location = new System.Drawing.Point(0, 0);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(200, 22);
            this.statusStrip2.TabIndex = 0;
            // 
            // FlawDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1411, 786);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FlawDetectForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlawDetectForm_FormClosing);
            this.Load += new System.EventHandler(this.FlawDetectForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FeatureLocalizationForm_MouseDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.控制参数groupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel 程序路径Lable;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip miniToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox 检测参数groupBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox 滤波参数groupBox;
        private System.Windows.Forms.ToolStrip 视图工具toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_Clear;
        private System.Windows.Forms.ToolStripButton toolStripButton_Select;
        private System.Windows.Forms.ToolStripButton toolStripButton_Translate;
        private System.Windows.Forms.ToolStripButton toolStripButton_Auto;
        private System.Windows.Forms.ToolStripButton toolStripButton_3D;
        private System.Windows.Forms.ComboBox 显示条目comboBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button 添加检测项button;
        private System.Windows.Forms.Button 执行button;
        private System.Windows.Forms.DataGridViewComboBoxColumn ShapeCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn OperateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WcsRect2Col;
        private System.Windows.Forms.DataGridViewButtonColumn TeachCol;
        private System.Windows.Forms.DataGridViewButtonColumn DeletCol;
        private System.Windows.Forms.Button 删除检测项button;
        private System.Windows.Forms.GroupBox 控制参数groupBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox 检测项listBox;
        private System.Windows.Forms.Button 图像加载button;
        private System.Windows.Forms.Button 重命名button;
        private System.Windows.Forms.GroupBox 区域选择参数groupBox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
    }
}