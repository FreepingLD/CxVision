namespace FunctionBlock
{
    partial class PointMoveForm
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
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.起始速度numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.运行速度numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.停止速度numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.加速度时间numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.减速度时间numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.S平滑时间numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.坐标类型comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.添加扫描线button = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.删除点button = new System.Windows.Forms.Button();
            this.坐标系comboBox = new System.Windows.Forms.ComboBox();
            this.清空点button = new System.Windows.Forms.Button();
            this.执行选中行Btn = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.IsSyn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IoPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Theta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MoveAxis = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.起始速度numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.运行速度numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.停止速度numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.加速度时间numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.减速度时间numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.S平滑时间numericUpDown)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 313F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 859F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox6, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1045, 651);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip1, 2);
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 621);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1172, 30);
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
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 2);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1172, 28);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "起始速度:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 43;
            this.label2.Text = "运行速度:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(283, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 44;
            this.label1.Text = "mm/s";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 111);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 12);
            this.label14.TabIndex = 45;
            this.label14.Text = "停止速度:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 46;
            this.label8.Text = "加速时间:";
            // 
            // 起始速度numericUpDown
            // 
            this.起始速度numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.起始速度numericUpDown.DecimalPlaces = 3;
            this.起始速度numericUpDown.Location = new System.Drawing.Point(82, 79);
            this.起始速度numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.起始速度numericUpDown.Name = "起始速度numericUpDown";
            this.起始速度numericUpDown.Size = new System.Drawing.Size(197, 21);
            this.起始速度numericUpDown.TabIndex = 59;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 189);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 48;
            this.label10.Text = "减速时间:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(283, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "mm/s";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 215);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 41;
            this.label12.Text = "S段平滑时间:";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(283, 112);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 56;
            this.label15.Text = "mm/s";
            // 
            // 运行速度numericUpDown
            // 
            this.运行速度numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.运行速度numericUpDown.DecimalPlaces = 3;
            this.运行速度numericUpDown.Location = new System.Drawing.Point(82, 133);
            this.运行速度numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.运行速度numericUpDown.Name = "运行速度numericUpDown";
            this.运行速度numericUpDown.Size = new System.Drawing.Size(197, 21);
            this.运行速度numericUpDown.TabIndex = 57;
            this.运行速度numericUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // 停止速度numericUpDown
            // 
            this.停止速度numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.停止速度numericUpDown.DecimalPlaces = 3;
            this.停止速度numericUpDown.Location = new System.Drawing.Point(82, 106);
            this.停止速度numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.停止速度numericUpDown.Name = "停止速度numericUpDown";
            this.停止速度numericUpDown.Size = new System.Drawing.Size(197, 21);
            this.停止速度numericUpDown.TabIndex = 58;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(199, 163);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 12);
            this.label9.TabIndex = 53;
            this.label9.Text = "s";
            // 
            // 加速度时间numericUpDown
            // 
            this.加速度时间numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.加速度时间numericUpDown.DecimalPlaces = 3;
            this.加速度时间numericUpDown.Location = new System.Drawing.Point(82, 159);
            this.加速度时间numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.加速度时间numericUpDown.Name = "加速度时间numericUpDown";
            this.加速度时间numericUpDown.Size = new System.Drawing.Size(197, 21);
            this.加速度时间numericUpDown.TabIndex = 61;
            this.加速度时间numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(199, 190);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 12);
            this.label11.TabIndex = 54;
            this.label11.Text = "s";
            // 
            // 减速度时间numericUpDown
            // 
            this.减速度时间numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.减速度时间numericUpDown.DecimalPlaces = 3;
            this.减速度时间numericUpDown.Location = new System.Drawing.Point(82, 185);
            this.减速度时间numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.减速度时间numericUpDown.Name = "减速度时间numericUpDown";
            this.减速度时间numericUpDown.Size = new System.Drawing.Size(197, 21);
            this.减速度时间numericUpDown.TabIndex = 62;
            this.减速度时间numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(199, 216);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(11, 12);
            this.label13.TabIndex = 55;
            this.label13.Text = "s";
            // 
            // S平滑时间numericUpDown
            // 
            this.S平滑时间numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.S平滑时间numericUpDown.DecimalPlaces = 3;
            this.S平滑时间numericUpDown.Location = new System.Drawing.Point(82, 211);
            this.S平滑时间numericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.S平滑时间numericUpDown.Name = "S平滑时间numericUpDown";
            this.S平滑时间numericUpDown.Size = new System.Drawing.Size(197, 21);
            this.S平滑时间numericUpDown.TabIndex = 64;
            this.S平滑时间numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // 坐标类型comboBox
            // 
            this.坐标类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.坐标类型comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.坐标类型comboBox.FormattingEnabled = true;
            this.坐标类型comboBox.Location = new System.Drawing.Point(82, 55);
            this.坐标类型comboBox.Name = "坐标类型comboBox";
            this.坐标类型comboBox.Size = new System.Drawing.Size(197, 20);
            this.坐标类型comboBox.TabIndex = 68;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 69;
            this.label3.Text = "坐标类型:";
            // 
            // 添加扫描线button
            // 
            this.添加扫描线button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.添加扫描线button.Location = new System.Drawing.Point(201, 267);
            this.添加扫描线button.Name = "添加扫描线button";
            this.添加扫描线button.Size = new System.Drawing.Size(78, 36);
            this.添加扫描线button.TabIndex = 45;
            this.添加扫描线button.Text = "添加点位(Add)";
            this.添加扫描线button.UseVisualStyleBackColor = true;
            this.添加扫描线button.Click += new System.EventHandler(this.添加点button_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(32, 32);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 12);
            this.label16.TabIndex = 72;
            this.label16.Text = "坐标系:";
            // 
            // 删除点button
            // 
            this.删除点button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.删除点button.Location = new System.Drawing.Point(109, 267);
            this.删除点button.Name = "删除点button";
            this.删除点button.Size = new System.Drawing.Size(78, 36);
            this.删除点button.TabIndex = 46;
            this.删除点button.Text = "删除点(Delete)";
            this.删除点button.UseVisualStyleBackColor = true;
            this.删除点button.Click += new System.EventHandler(this.删除点button_Click);
            // 
            // 坐标系comboBox
            // 
            this.坐标系comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.坐标系comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.坐标系comboBox.FormattingEnabled = true;
            this.坐标系comboBox.Location = new System.Drawing.Point(82, 29);
            this.坐标系comboBox.Name = "坐标系comboBox";
            this.坐标系comboBox.Size = new System.Drawing.Size(197, 20);
            this.坐标系comboBox.TabIndex = 71;
            // 
            // 清空点button
            // 
            this.清空点button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.清空点button.Location = new System.Drawing.Point(18, 267);
            this.清空点button.Name = "清空点button";
            this.清空点button.Size = new System.Drawing.Size(78, 36);
            this.清空点button.TabIndex = 47;
            this.清空点button.Text = "清空点 (Clear)";
            this.清空点button.UseVisualStyleBackColor = true;
            this.清空点button.Click += new System.EventHandler(this.清空点button_Click);
            // 
            // 执行选中行Btn
            // 
            this.执行选中行Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.执行选中行Btn.Location = new System.Drawing.Point(201, 324);
            this.执行选中行Btn.Name = "执行选中行Btn";
            this.执行选中行Btn.Size = new System.Drawing.Size(78, 36);
            this.执行选中行Btn.TabIndex = 73;
            this.执行选中行Btn.Text = "执行选中行(Excute)";
            this.执行选中行Btn.UseVisualStyleBackColor = true;
            this.执行选中行Btn.Click += new System.EventHandler(this.单步执行Btn_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.执行选中行Btn);
            this.groupBox6.Controls.Add(this.清空点button);
            this.groupBox6.Controls.Add(this.坐标系comboBox);
            this.groupBox6.Controls.Add(this.删除点button);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.添加扫描线button);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.坐标类型comboBox);
            this.groupBox6.Controls.Add(this.S平滑时间numericUpDown);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.减速度时间numericUpDown);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.加速度时间numericUpDown);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.停止速度numericUpDown);
            this.groupBox6.Controls.Add(this.运行速度numericUpDown);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.起始速度numericUpDown);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(1, 29);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox6.Name = "groupBox6";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox6, 4);
            this.groupBox6.Size = new System.Drawing.Size(311, 591);
            this.groupBox6.TabIndex = 41;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "运动参数";
            // 
            // IsSyn
            // 
            this.IsSyn.DataPropertyName = "IsWait";
            this.IsSyn.FillWeight = 105.5837F;
            this.IsSyn.HeaderText = "是否等待";
            this.IsSyn.Name = "IsSyn";
            // 
            // IoPort
            // 
            this.IoPort.DataPropertyName = "IoOutPort";
            this.IoPort.FillWeight = 105.5837F;
            this.IoPort.HeaderText = "IO输出";
            this.IoPort.Name = "IoPort";
            // 
            // Theta
            // 
            this.Theta.DataPropertyName = "Theta";
            this.Theta.FillWeight = 105.5837F;
            this.Theta.HeaderText = "Theta坐标";
            this.Theta.Name = "Theta";
            // 
            // Z
            // 
            this.Z.DataPropertyName = "Z";
            this.Z.FillWeight = 105.5837F;
            this.Z.HeaderText = "Z坐标";
            this.Z.Name = "Z";
            // 
            // Y
            // 
            this.Y.DataPropertyName = "Y";
            this.Y.FillWeight = 105.5837F;
            this.Y.HeaderText = "Y坐标";
            this.Y.Name = "Y";
            // 
            // X
            // 
            this.X.DataPropertyName = "X";
            this.X.FillWeight = 105.5837F;
            this.X.HeaderText = "X坐标";
            this.X.Name = "X";
            // 
            // MoveAxis
            // 
            this.MoveAxis.DataPropertyName = "MoveAxis";
            this.MoveAxis.FillWeight = 105.5837F;
            this.MoveAxis.HeaderText = "运动轴";
            this.MoveAxis.Name = "MoveAxis";
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.FillWeight = 60.9137F;
            this.IsActive.HeaderText = "激活";
            this.IsActive.Name = "IsActive";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsActive,
            this.MoveAxis,
            this.X,
            this.Y,
            this.Z,
            this.Theta,
            this.IoPort,
            this.IsSyn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(314, 29);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 60;
            this.tableLayoutPanel1.SetRowSpan(this.dataGridView1, 4);
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(857, 591);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // PointMoveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1045, 651);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PointMoveForm";
            this.Text = "点位运动";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointMoveForm_FormClosing);
            this.Load += new System.EventHandler(this.PointMoveForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.起始速度numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.运行速度numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.停止速度numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.加速度时间numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.减速度时间numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.S平滑时间numericUpDown)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewComboBoxColumn MoveAxis;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Z;
        private System.Windows.Forms.DataGridViewTextBoxColumn Theta;
        private System.Windows.Forms.DataGridViewTextBoxColumn IoPort;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSyn;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button 执行选中行Btn;
        private System.Windows.Forms.Button 清空点button;
        private System.Windows.Forms.ComboBox 坐标系comboBox;
        private System.Windows.Forms.Button 删除点button;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button 添加扫描线button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 坐标类型comboBox;
        private System.Windows.Forms.NumericUpDown S平滑时间numericUpDown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown 减速度时间numericUpDown;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown 加速度时间numericUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown 停止速度numericUpDown;
        private System.Windows.Forms.NumericUpDown 运行速度numericUpDown;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown 起始速度numericUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}