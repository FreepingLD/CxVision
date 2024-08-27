namespace FunctionBlock
{
    partial class PointCloudAcqParamForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.图像取点button = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.addPointButton = new System.Windows.Forms.Button();
            this.deletePointButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.相机采集源comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.运动panel = new System.Windows.Forms.Panel();
            this.运行toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.相机hWindowControl = new HalconDotNet.HWindowControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.运行toolStrip.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 456F));
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1001, 640);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.运行结果toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 610);
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
            this.显示条目comboBox.Location = new System.Drawing.Point(776, 0);
            this.显示条目comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.显示条目comboBox.Name = "显示条目comboBox";
            this.显示条目comboBox.Size = new System.Drawing.Size(225, 20);
            this.显示条目comboBox.TabIndex = 11;
            this.显示条目comboBox.SelectionChangeCommitted += new System.EventHandler(this.显示条目comboBox_SelectionChangeCommitted);
            // 
            // 视图工具toolStrip
            // 
            this.视图工具toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.视图工具toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.视图工具toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Clear,
            this.toolStripButton_Select,
            this.toolStripButton_Translate,
            this.toolStripButton_Auto,
            this.toolStripButton_3D});
            this.视图工具toolStrip.Location = new System.Drawing.Point(320, 0);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(456, 22);
            this.视图工具toolStrip.TabIndex = 15;
            this.视图工具toolStrip.Text = "toolStrip2";
            this.视图工具toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(36, 19);
            this.toolStripButton_Clear.Text = "清除";
            // 
            // toolStripButton_Select
            // 
            this.toolStripButton_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Select.Name = "toolStripButton_Select";
            this.toolStripButton_Select.Size = new System.Drawing.Size(36, 19);
            this.toolStripButton_Select.Text = "选择";
            // 
            // toolStripButton_Translate
            // 
            this.toolStripButton_Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Translate.Name = "toolStripButton_Translate";
            this.toolStripButton_Translate.Size = new System.Drawing.Size(36, 19);
            this.toolStripButton_Translate.Text = "平移";
            // 
            // toolStripButton_Auto
            // 
            this.toolStripButton_Auto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Auto.Name = "toolStripButton_Auto";
            this.toolStripButton_Auto.Size = new System.Drawing.Size(60, 19);
            this.toolStripButton_Auto.Text = "适应窗口";
            // 
            // toolStripButton_3D
            // 
            this.toolStripButton_3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_3D.Image = global::FunctionBlock.Properties.Resources._1606742721_1_;
            this.toolStripButton_3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_3D.Name = "toolStripButton_3D";
            this.toolStripButton_3D.Size = new System.Drawing.Size(28, 19);
            this.toolStripButton_3D.Text = "3D";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 22);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 4);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(320, 588);
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
            this.tabPage1.Size = new System.Drawing.Size(312, 562);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 308F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.运动panel, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(306, 556);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.图像取点button);
            this.panel1.Controls.Add(this.ClearButton);
            this.panel1.Controls.Add(this.addPointButton);
            this.panel1.Controls.Add(this.deletePointButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 46);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(308, 40);
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
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 86);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 60;
            this.tableLayoutPanel2.SetRowSpan(this.dataGridView1, 2);
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(308, 300);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "x";
            this.Column1.HeaderText = "X";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "y";
            this.Column2.HeaderText = "Y";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "z";
            this.Column3.HeaderText = "Z";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 125;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "theta";
            this.Column4.HeaderText = "Theta";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 125;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "u";
            this.Column5.HeaderText = "U";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 125;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "v";
            this.Column6.HeaderText = "V";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 125;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "refPoint_x";
            this.Column7.HeaderText = "refPoint_x";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Width = 125;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "refPoint_y";
            this.Column8.HeaderText = "refPoint_y";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Width = 125;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.相机采集源comboBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 46);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "传感器设置 ";
            // 
            // 相机采集源comboBox
            // 
            this.相机采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.相机采集源comboBox.FormattingEnabled = true;
            this.相机采集源comboBox.Location = new System.Drawing.Point(75, 21);
            this.相机采集源comboBox.Name = "相机采集源comboBox";
            this.相机采集源comboBox.Size = new System.Drawing.Size(228, 20);
            this.相机采集源comboBox.TabIndex = 35;
            this.相机采集源comboBox.SelectedIndexChanged += new System.EventHandler(this.相机采集源comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "相机传感器";
            // 
            // 运动panel
            // 
            this.运动panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.运动panel.Location = new System.Drawing.Point(0, 386);
            this.运动panel.Margin = new System.Windows.Forms.Padding(0);
            this.运动panel.Name = "运动panel";
            this.运动panel.Size = new System.Drawing.Size(308, 170);
            this.运动panel.TabIndex = 25;
            // 
            // 运行toolStrip
            // 
            this.运行toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.运行toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.运行toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.运行toolStrip.Location = new System.Drawing.Point(0, 0);
            this.运行toolStrip.Name = "运行toolStrip";
            this.运行toolStrip.Size = new System.Drawing.Size(320, 22);
            this.运行toolStrip.TabIndex = 13;
            this.运行toolStrip.Text = "toolStrip1";
            this.运行toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources.Start;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(56, 19);
            this.toolStripButton_Run.Text = "执行";
            // 
            // tabControl2
            // 
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl2, 2);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(320, 22);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl2, 5);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(681, 618);
            this.tabControl2.TabIndex = 19;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.相机hWindowControl);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(673, 592);
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
            this.相机hWindowControl.Size = new System.Drawing.Size(667, 586);
            this.相机hWindowControl.TabIndex = 0;
            this.相机hWindowControl.WindowSize = new System.Drawing.Size(667, 586);
            // 
            // PointCloudAcqParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 640);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PointCloudAcqParamForm";
            this.Text = "采集配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LaserPointAcqForm_FormClosing);
            this.Load += new System.EventHandler(this.PointCloudAcqParamForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.运行toolStrip.ResumeLayout(false);
            this.运行toolStrip.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
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
        private System.Windows.Forms.ComboBox 相机采集源comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private HalconDotNet.HWindowControl 相机hWindowControl;
    }
}