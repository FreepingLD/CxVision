namespace FunctionBlock
{
    partial class LinearCalibrateForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.标定执行tabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.校准方式comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.保存button = new System.Windows.Forms.Button();
            this.当前标准位置textBox = new System.Windows.Forms.TextBox();
            this.清空button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.删除位置button = new System.Windows.Forms.Button();
            this.校准轴comboBox = new System.Windows.Forms.ComboBox();
            this.添加位置button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.坐标系comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.运动pane2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.标定程序tabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.打开toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.保存toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.工具toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.传感器comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.实时采集checkBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.标定执行tabPage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.标定程序tabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 336F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 639F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 579F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1042, 780);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.标定执行tabPage);
            this.tabControl1.Controls.Add(this.标定程序tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 40);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(336, 740);
            this.tabControl1.TabIndex = 34;
            // 
            // 标定执行tabPage
            // 
            this.标定执行tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.标定执行tabPage.Controls.Add(this.tableLayoutPanel2);
            this.标定执行tabPage.Location = new System.Drawing.Point(4, 22);
            this.标定执行tabPage.Name = "标定执行tabPage";
            this.标定执行tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.标定执行tabPage.Size = new System.Drawing.Size(328, 714);
            this.标定执行tabPage.TabIndex = 3;
            this.标定执行tabPage.Text = "标定参数";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.运动pane2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(322, 708);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.校准方式comboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.保存button);
            this.groupBox1.Controls.Add(this.当前标准位置textBox);
            this.groupBox1.Controls.Add(this.清空button);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.删除位置button);
            this.groupBox1.Controls.Add(this.校准轴comboBox);
            this.groupBox1.Controls.Add(this.添加位置button);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.坐标系comboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel2.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(316, 491);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标定参数";
            // 
            // 校准方式comboBox
            // 
            this.校准方式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.校准方式comboBox.FormattingEnabled = true;
            this.校准方式comboBox.Items.AddRange(new object[] {
            "影像",
            "激光"});
            this.校准方式comboBox.Location = new System.Drawing.Point(55, 83);
            this.校准方式comboBox.Name = "校准方式comboBox";
            this.校准方式comboBox.Size = new System.Drawing.Size(255, 20);
            this.校准方式comboBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "校准方式：";
            // 
            // 保存button
            // 
            this.保存button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.保存button.Location = new System.Drawing.Point(5, 447);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(75, 38);
            this.保存button.TabIndex = 3;
            this.保存button.Text = "保存";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // 当前标准位置textBox
            // 
            this.当前标准位置textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.当前标准位置textBox.Location = new System.Drawing.Point(55, 403);
            this.当前标准位置textBox.Name = "当前标准位置textBox";
            this.当前标准位置textBox.Size = new System.Drawing.Size(255, 21);
            this.当前标准位置textBox.TabIndex = 5;
            this.当前标准位置textBox.Text = "0";
            // 
            // 清空button
            // 
            this.清空button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.清空button.Location = new System.Drawing.Point(84, 447);
            this.清空button.Name = "清空button";
            this.清空button.Size = new System.Drawing.Size(75, 38);
            this.清空button.TabIndex = 2;
            this.清空button.Text = "清空位置";
            this.清空button.UseVisualStyleBackColor = true;
            this.清空button.Click += new System.EventHandler(this.清空button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 408);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "当前位置：";
            // 
            // 删除位置button
            // 
            this.删除位置button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.删除位置button.Location = new System.Drawing.Point(163, 447);
            this.删除位置button.Name = "删除位置button";
            this.删除位置button.Size = new System.Drawing.Size(75, 38);
            this.删除位置button.TabIndex = 1;
            this.删除位置button.Text = "删除位置";
            this.删除位置button.UseVisualStyleBackColor = true;
            this.删除位置button.Click += new System.EventHandler(this.删除位置button_Click);
            // 
            // 校准轴comboBox
            // 
            this.校准轴comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.校准轴comboBox.FormattingEnabled = true;
            this.校准轴comboBox.Items.AddRange(new object[] {
            "X轴",
            "Y轴",
            "Z轴",
            "Theta轴"});
            this.校准轴comboBox.Location = new System.Drawing.Point(55, 57);
            this.校准轴comboBox.Name = "校准轴comboBox";
            this.校准轴comboBox.Size = new System.Drawing.Size(255, 20);
            this.校准轴comboBox.TabIndex = 9;
            this.校准轴comboBox.SelectedIndexChanged += new System.EventHandler(this.校准轴comboBox_SelectedIndexChanged);
            // 
            // 添加位置button
            // 
            this.添加位置button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.添加位置button.Location = new System.Drawing.Point(241, 447);
            this.添加位置button.Name = "添加位置button";
            this.添加位置button.Size = new System.Drawing.Size(75, 38);
            this.添加位置button.TabIndex = 0;
            this.添加位置button.Text = "添加位置";
            this.添加位置button.UseVisualStyleBackColor = true;
            this.添加位置button.Click += new System.EventHandler(this.添加位置button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "校准轴：";
            // 
            // 坐标系comboBox
            // 
            this.坐标系comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.坐标系comboBox.FormattingEnabled = true;
            this.坐标系comboBox.Location = new System.Drawing.Point(55, 31);
            this.坐标系comboBox.Name = "坐标系comboBox";
            this.坐标系comboBox.Size = new System.Drawing.Size(255, 20);
            this.坐标系comboBox.TabIndex = 7;
            this.坐标系comboBox.SelectedIndexChanged += new System.EventHandler(this.相机comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "坐标系：";
            // 
            // 运动pane2
            // 
            this.运动pane2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.运动pane2.Location = new System.Drawing.Point(3, 536);
            this.运动pane2.Name = "运动pane2";
            this.运动pane2.Size = new System.Drawing.Size(316, 169);
            this.运动pane2.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 497);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(245, 12);
            this.label7.TabIndex = 24;
            this.label7.Text = "以左上角作为参考点，X往右为正，Y往上为正";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // 标定程序tabPage
            // 
            this.标定程序tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.标定程序tabPage.Controls.Add(this.groupBox2);
            this.标定程序tabPage.Location = new System.Drawing.Point(4, 22);
            this.标定程序tabPage.Name = "标定程序tabPage";
            this.标定程序tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.标定程序tabPage.Size = new System.Drawing.Size(328, 716);
            this.标定程序tabPage.TabIndex = 4;
            this.标定程序tabPage.Text = "标定程序";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 710);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标定程序";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 17);
            this.treeView1.Margin = new System.Windows.Forms.Padding(0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(316, 690);
            this.treeView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 3);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run,
            this.toolStripButton1,
            this.打开toolStripButton,
            this.保存toolStripButton,
            this.工具toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1042, 40);
            this.toolStrip1.TabIndex = 33;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources._1606742721_1_;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(36, 35);
            this.toolStripButton_Run.Text = "执行";
            this.toolStripButton_Run.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::FunctionBlock.Properties.Resources.Stop;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 35);
            this.toolStripButton1.Text = "停止";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 打开toolStripButton
            // 
            this.打开toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742449_1_;
            this.打开toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打开toolStripButton.Name = "打开toolStripButton";
            this.打开toolStripButton.Size = new System.Drawing.Size(36, 35);
            this.打开toolStripButton.Text = "打开";
            this.打开toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 保存toolStripButton
            // 
            this.保存toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742495_1_;
            this.保存toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.保存toolStripButton.Name = "保存toolStripButton";
            this.保存toolStripButton.Size = new System.Drawing.Size(36, 35);
            this.保存toolStripButton.Text = "保存";
            this.保存toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 工具toolStripButton
            // 
            this.工具toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742307_1_;
            this.工具toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.工具toolStripButton.Name = "工具toolStripButton";
            this.工具toolStripButton.Size = new System.Drawing.Size(60, 35);
            this.工具toolStripButton.Text = "检测工具";
            this.工具toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 2);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(339, 644);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(700, 133);
            this.dataGridView1.TabIndex = 1;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(339, 65);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(700, 573);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(700, 573);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.传感器comboBox);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.实时采集checkBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(336, 40);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 22);
            this.panel1.TabIndex = 35;
            // 
            // 传感器comboBox
            // 
            this.传感器comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.传感器comboBox.FormattingEnabled = true;
            this.传感器comboBox.ItemHeight = 12;
            this.传感器comboBox.Location = new System.Drawing.Point(61, 2);
            this.传感器comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.传感器comboBox.Name = "传感器comboBox";
            this.传感器comboBox.Size = new System.Drawing.Size(310, 20);
            this.传感器comboBox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(25, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 20);
            this.label5.TabIndex = 30;
            this.label5.Text = "实时";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 实时采集checkBox
            // 
            this.实时采集checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.实时采集checkBox.BackColor = System.Drawing.Color.Lime;
            this.实时采集checkBox.Location = new System.Drawing.Point(2, 2);
            this.实时采集checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.实时采集checkBox.Name = "实时采集checkBox";
            this.实时采集checkBox.Size = new System.Drawing.Size(20, 20);
            this.实时采集checkBox.TabIndex = 21;
            this.实时采集checkBox.TabStop = false;
            this.实时采集checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.实时采集checkBox.UseVisualStyleBackColor = false;
            this.实时采集checkBox.CheckedChanged += new System.EventHandler(this.实时采集checkBox_CheckedChanged);
            // 
            // LinearCalibrateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 780);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LinearCalibrateForm";
            this.Text = "轴校准";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LinearCalibrateForm_FormClosing);
            this.Load += new System.EventHandler(this.LinearCalibrateForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.标定执行tabPage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.标定程序tabPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox 当前标准位置textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.Button 清空button;
        private System.Windows.Forms.Button 删除位置button;
        private System.Windows.Forms.Button 添加位置button;
        private System.Windows.Forms.ComboBox 坐标系comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 校准轴comboBox;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton 打开toolStripButton;
        private System.Windows.Forms.ToolStripButton 保存toolStripButton;
        private System.Windows.Forms.ToolStripButton 工具toolStripButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 标定执行tabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel 运动pane2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage 标定程序tabPage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ComboBox 校准方式comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox 实时采集checkBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 传感器comboBox;
    }
}