namespace FunctionBlock
{
    partial class SpiralScanForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.运行结果toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.显示条目comboBox = new System.Windows.Forms.ComboBox();
            this.视图工具toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Translate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Auto = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_3D = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.相机采集源comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.激光采集源comboBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.图像取点button = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.addPointButton = new System.Windows.Forms.Button();
            this.deletePointButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.起点偏移numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.运动轴comboBox = new System.Windows.Forms.ComboBox();
            this.减速度时间numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.加速度时间numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Z向偏移numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.圆弧半径numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.圆弧方向comboBox = new System.Windows.Forms.ComboBox();
            this.圆弧数量numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.运动panel = new System.Windows.Forms.Panel();
            this.运行toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.相机hWindowControl = new HalconDotNet.HWindowControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.激光hWindowControl = new HalconDotNet.HWindowControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.起点偏移numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.减速度时间numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.加速度时间numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Z向偏移numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.圆弧半径numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.圆弧数量numericUpDown)).BeginInit();
            this.运行toolStrip.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.显示条目comboBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.运行toolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1029, 684);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.运行结果toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 654);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(320, 30);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 25);
            this.toolStripStatusLabel1.Text = "执行结果：";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // 运行结果toolStripStatusLabel
            // 
            this.运行结果toolStripStatusLabel.Name = "运行结果toolStripStatusLabel";
            this.运行结果toolStripStatusLabel.Size = new System.Drawing.Size(28, 25);
            this.运行结果toolStripStatusLabel.Text = "……";
            // 
            // 显示条目comboBox
            // 
            this.显示条目comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.显示条目comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.显示条目comboBox.FormattingEnabled = true;
            this.显示条目comboBox.Location = new System.Drawing.Point(570, 0);
            this.显示条目comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.显示条目comboBox.Name = "显示条目comboBox";
            this.显示条目comboBox.Size = new System.Drawing.Size(459, 20);
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
            this.视图工具toolStrip.Location = new System.Drawing.Point(320, 0);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(250, 28);
            this.视图工具toolStrip.TabIndex = 15;
            this.视图工具toolStrip.Text = "toolStrip2";
            this.视图工具toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(36, 25);
            this.toolStripButton_Clear.Text = "清除";
            // 
            // toolStripButton_Select
            // 
            this.toolStripButton_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Select.Name = "toolStripButton_Select";
            this.toolStripButton_Select.Size = new System.Drawing.Size(36, 25);
            this.toolStripButton_Select.Text = "选择";
            // 
            // toolStripButton_Translate
            // 
            this.toolStripButton_Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Translate.Name = "toolStripButton_Translate";
            this.toolStripButton_Translate.Size = new System.Drawing.Size(36, 25);
            this.toolStripButton_Translate.Text = "平移";
            // 
            // toolStripButton_Auto
            // 
            this.toolStripButton_Auto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Auto.Name = "toolStripButton_Auto";
            this.toolStripButton_Auto.Size = new System.Drawing.Size(60, 25);
            this.toolStripButton_Auto.Text = "适应窗口";
            // 
            // toolStripButton_3D
            // 
            this.toolStripButton_3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_3D.Image = global::FunctionBlock.Properties.Resources._1606742721_1_;
            this.toolStripButton_3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_3D.Name = "toolStripButton_3D";
            this.toolStripButton_3D.Size = new System.Drawing.Size(28, 25);
            this.toolStripButton_3D.Text = "3D";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 4);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(320, 626);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(312, 600);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 306F));
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.运动panel, 0, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 127F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(306, 594);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 301);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 60;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(306, 123);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 70);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(306, 63);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "坐标系";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 17);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(300, 43);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.相机采集源comboBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.激光采集源comboBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 70);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "传感器设置 ";
            // 
            // 相机采集源comboBox
            // 
            this.相机采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.相机采集源comboBox.FormattingEnabled = true;
            this.相机采集源comboBox.Location = new System.Drawing.Point(74, 41);
            this.相机采集源comboBox.Name = "相机采集源comboBox";
            this.相机采集源comboBox.Size = new System.Drawing.Size(228, 20);
            this.相机采集源comboBox.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 31;
            this.label2.Text = "激光传感器";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "相机传感器";
            // 
            // 激光采集源comboBox
            // 
            this.激光采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.激光采集源comboBox.FormattingEnabled = true;
            this.激光采集源comboBox.Location = new System.Drawing.Point(74, 15);
            this.激光采集源comboBox.Name = "激光采集源comboBox";
            this.激光采集源comboBox.Size = new System.Drawing.Size(228, 20);
            this.激光采集源comboBox.TabIndex = 33;
            this.激光采集源comboBox.SelectedIndexChanged += new System.EventHandler(this.激光采集源comboBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.图像取点button);
            this.panel1.Controls.Add(this.ClearButton);
            this.panel1.Controls.Add(this.addPointButton);
            this.panel1.Controls.Add(this.deletePointButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 260);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 41);
            this.panel1.TabIndex = 24;
            // 
            // 图像取点button
            // 
            this.图像取点button.Location = new System.Drawing.Point(235, 2);
            this.图像取点button.Name = "图像取点button";
            this.图像取点button.Size = new System.Drawing.Size(69, 36);
            this.图像取点button.TabIndex = 43;
            this.图像取点button.Text = "图像取点(G)";
            this.图像取点button.UseVisualStyleBackColor = true;
            this.图像取点button.Click += new System.EventHandler(this.图像取点button_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(2, 1);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(69, 36);
            this.ClearButton.TabIndex = 33;
            this.ClearButton.Text = "清空(C)";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.清空点Button_Click);
            // 
            // addPointButton
            // 
            this.addPointButton.Location = new System.Drawing.Point(158, 1);
            this.addPointButton.Name = "addPointButton";
            this.addPointButton.Size = new System.Drawing.Size(69, 36);
            this.addPointButton.TabIndex = 31;
            this.addPointButton.Text = "添加(A)";
            this.addPointButton.UseVisualStyleBackColor = true;
            this.addPointButton.Click += new System.EventHandler(this.添加点Button_Click);
            // 
            // deletePointButton
            // 
            this.deletePointButton.Location = new System.Drawing.Point(79, 1);
            this.deletePointButton.Name = "deletePointButton";
            this.deletePointButton.Size = new System.Drawing.Size(69, 36);
            this.deletePointButton.TabIndex = 32;
            this.deletePointButton.Text = "删除(D)";
            this.deletePointButton.UseVisualStyleBackColor = true;
            this.deletePointButton.Click += new System.EventHandler(this.删除点Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.起点偏移numericUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.运动轴comboBox);
            this.groupBox1.Controls.Add(this.减速度时间numericUpDown);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.加速度时间numericUpDown);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.Z向偏移numericUpDown);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.圆弧半径numericUpDown);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.圆弧方向comboBox);
            this.groupBox1.Controls.Add(this.圆弧数量numericUpDown);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 121);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "圆弧参数";
            // 
            // 起点偏移numericUpDown
            // 
            this.起点偏移numericUpDown.DecimalPlaces = 3;
            this.起点偏移numericUpDown.Location = new System.Drawing.Point(223, 43);
            this.起点偏移numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.起点偏移numericUpDown.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.起点偏移numericUpDown.Name = "起点偏移numericUpDown";
            this.起点偏移numericUpDown.Size = new System.Drawing.Size(73, 21);
            this.起点偏移numericUpDown.TabIndex = 98;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 97;
            this.label3.Text = "起点偏移:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 96;
            this.label5.Text = "运动轴:";
            // 
            // 运动轴comboBox
            // 
            this.运动轴comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.运动轴comboBox.FormattingEnabled = true;
            this.运动轴comboBox.Location = new System.Drawing.Point(69, 17);
            this.运动轴comboBox.Name = "运动轴comboBox";
            this.运动轴comboBox.Size = new System.Drawing.Size(79, 20);
            this.运动轴comboBox.TabIndex = 95;
            // 
            // 减速度时间numericUpDown
            // 
            this.减速度时间numericUpDown.DecimalPlaces = 3;
            this.减速度时间numericUpDown.Location = new System.Drawing.Point(223, 96);
            this.减速度时间numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.减速度时间numericUpDown.Name = "减速度时间numericUpDown";
            this.减速度时间numericUpDown.Size = new System.Drawing.Size(73, 21);
            this.减速度时间numericUpDown.TabIndex = 94;
            this.减速度时间numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(357, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 12);
            this.label11.TabIndex = 92;
            this.label11.Text = "s";
            // 
            // 加速度时间numericUpDown
            // 
            this.加速度时间numericUpDown.DecimalPlaces = 3;
            this.加速度时间numericUpDown.Location = new System.Drawing.Point(223, 70);
            this.加速度时间numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.加速度时间numericUpDown.Name = "加速度时间numericUpDown";
            this.加速度时间numericUpDown.Size = new System.Drawing.Size(73, 21);
            this.加速度时间numericUpDown.TabIndex = 93;
            this.加速度时间numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(357, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 12);
            this.label9.TabIndex = 91;
            this.label9.Text = "s";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(164, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 90;
            this.label10.Text = "减速时间:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(164, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 89;
            this.label8.Text = "加速时间:";
            // 
            // Z向偏移numericUpDown
            // 
            this.Z向偏移numericUpDown.DecimalPlaces = 3;
            this.Z向偏移numericUpDown.Location = new System.Drawing.Point(223, 17);
            this.Z向偏移numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.Z向偏移numericUpDown.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.Z向偏移numericUpDown.Name = "Z向偏移numericUpDown";
            this.Z向偏移numericUpDown.Size = new System.Drawing.Size(73, 21);
            this.Z向偏移numericUpDown.TabIndex = 88;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(164, 21);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 87;
            this.label20.Text = "Z向偏移:";
            // 
            // 圆弧半径numericUpDown
            // 
            this.圆弧半径numericUpDown.DecimalPlaces = 3;
            this.圆弧半径numericUpDown.Location = new System.Drawing.Point(69, 95);
            this.圆弧半径numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.圆弧半径numericUpDown.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.圆弧半径numericUpDown.Name = "圆弧半径numericUpDown";
            this.圆弧半径numericUpDown.Size = new System.Drawing.Size(79, 21);
            this.圆弧半径numericUpDown.TabIndex = 86;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(5, 99);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 12);
            this.label18.TabIndex = 85;
            this.label18.Text = "圆弧半径:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(5, 46);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 12);
            this.label19.TabIndex = 84;
            this.label19.Text = "圆弧方向:";
            // 
            // 圆弧方向comboBox
            // 
            this.圆弧方向comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.圆弧方向comboBox.FormattingEnabled = true;
            this.圆弧方向comboBox.Items.AddRange(new object[] {
            "0",
            "1"});
            this.圆弧方向comboBox.Location = new System.Drawing.Point(69, 43);
            this.圆弧方向comboBox.Name = "圆弧方向comboBox";
            this.圆弧方向comboBox.Size = new System.Drawing.Size(79, 20);
            this.圆弧方向comboBox.TabIndex = 83;
            // 
            // 圆弧数量numericUpDown
            // 
            this.圆弧数量numericUpDown.DecimalPlaces = 3;
            this.圆弧数量numericUpDown.Location = new System.Drawing.Point(69, 68);
            this.圆弧数量numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.圆弧数量numericUpDown.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.圆弧数量numericUpDown.Name = "圆弧数量numericUpDown";
            this.圆弧数量numericUpDown.Size = new System.Drawing.Size(79, 21);
            this.圆弧数量numericUpDown.TabIndex = 82;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 72);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 81;
            this.label16.Text = "圆弧数量:";
            // 
            // 运动panel
            // 
            this.运动panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.运动panel.Location = new System.Drawing.Point(0, 424);
            this.运动panel.Margin = new System.Windows.Forms.Padding(0);
            this.运动panel.Name = "运动panel";
            this.运动panel.Size = new System.Drawing.Size(306, 170);
            this.运动panel.TabIndex = 25;
            // 
            // 运行toolStrip
            // 
            this.运行toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.运行toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.运行toolStrip.Location = new System.Drawing.Point(0, 0);
            this.运行toolStrip.Name = "运行toolStrip";
            this.运行toolStrip.Size = new System.Drawing.Size(320, 28);
            this.运行toolStrip.TabIndex = 13;
            this.运行toolStrip.Text = "toolStrip1";
            this.运行toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources._1606742721_1_;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(52, 25);
            this.toolStripButton_Run.Text = "执行";
            // 
            // tabControl2
            // 
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl2, 2);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(320, 28);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl2, 5);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(709, 656);
            this.tabControl2.TabIndex = 19;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.相机hWindowControl);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(701, 630);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "相机";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // 相机hWindowControl
            // 
            this.相机hWindowControl.BackColor = System.Drawing.Color.Black;
            this.相机hWindowControl.BorderColor = System.Drawing.Color.Black;
            this.相机hWindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.相机hWindowControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.相机hWindowControl.Location = new System.Drawing.Point(3, 3);
            this.相机hWindowControl.Margin = new System.Windows.Forms.Padding(0);
            this.相机hWindowControl.Name = "相机hWindowControl";
            this.相机hWindowControl.Size = new System.Drawing.Size(695, 624);
            this.相机hWindowControl.TabIndex = 0;
            this.相机hWindowControl.WindowSize = new System.Drawing.Size(695, 624);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.激光hWindowControl);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(701, 630);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "激光";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // 激光hWindowControl
            // 
            this.激光hWindowControl.BackColor = System.Drawing.Color.Black;
            this.激光hWindowControl.BorderColor = System.Drawing.Color.Black;
            this.激光hWindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.激光hWindowControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.激光hWindowControl.Location = new System.Drawing.Point(3, 3);
            this.激光hWindowControl.Margin = new System.Windows.Forms.Padding(0);
            this.激光hWindowControl.Name = "激光hWindowControl";
            this.激光hWindowControl.Size = new System.Drawing.Size(695, 624);
            this.激光hWindowControl.TabIndex = 0;
            this.激光hWindowControl.WindowSize = new System.Drawing.Size(695, 624);
            // 
            // SpiralScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 684);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SpiralScanForm";
            this.Text = "圆弧扫描采集";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpiralScanForm_FormClosing);
            this.Load += new System.EventHandler(this.SpiralScanForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.起点偏移numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.减速度时间numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.加速度时间numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Z向偏移numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.圆弧半径numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.圆弧数量numericUpDown)).EndInit();
            this.运行toolStrip.ResumeLayout(false);
            this.运行toolStrip.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox 显示条目comboBox;
        public System.Windows.Forms.ToolStrip 运行toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.ToolStrip 视图工具toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_Clear;
        private System.Windows.Forms.ToolStripButton toolStripButton_Select;
        private System.Windows.Forms.ToolStripButton toolStripButton_Translate;
        private System.Windows.Forms.ToolStripButton toolStripButton_Auto;
        private System.Windows.Forms.ToolStripButton toolStripButton_3D;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox 激光采集源comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 相机采集源comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private HalconDotNet.HWindowControl 相机hWindowControl;
        private System.Windows.Forms.TabPage tabPage4;
        private HalconDotNet.HWindowControl 激光hWindowControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button 图像取点button;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button addPointButton;
        private System.Windows.Forms.Button deletePointButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel 运行结果toolStripStatusLabel;
        private System.Windows.Forms.Panel 运动panel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown Z向偏移numericUpDown;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown 圆弧半径numericUpDown;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox 圆弧方向comboBox;
        private System.Windows.Forms.NumericUpDown 圆弧数量numericUpDown;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown 减速度时间numericUpDown;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown 加速度时间numericUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 运动轴comboBox;
        private System.Windows.Forms.NumericUpDown 起点偏移numericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox1;
    }
}