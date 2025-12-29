namespace FunctionBlock
{
    partial class ManualCalibForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.视图工具toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Translate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Auto = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_3D = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.设置tabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ShapeCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.WcsRect2Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeachCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeletCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.加载图像Btn = new System.Windows.Forms.Button();
            this.Y轴偏移_textBox = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.X轴偏移_textBox = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.保存Btn = new System.Windows.Forms.Button();
            this.标定Btn = new System.Windows.Forms.Button();
            this.行数numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.列数numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.标定轴comboBox = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.映射dataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.视图工具toolStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.设置tabPage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.行数numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.列数numericUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.映射dataGridView)).BeginInit();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 435F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 241F));
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1203, 745);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // 视图工具toolStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.视图工具toolStrip, 2);
            this.视图工具toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.视图工具toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.视图工具toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Clear,
            this.toolStripButton_Select,
            this.toolStripButton_Translate,
            this.toolStripButton_Auto,
            this.toolStripButton_3D});
            this.视图工具toolStrip.Location = new System.Drawing.Point(435, 0);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(768, 28);
            this.视图工具toolStrip.TabIndex = 15;
            this.视图工具toolStrip.Text = "toolStrip2";
            this.视图工具toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.Image = global::FunctionBlock.Properties.Resources.清除;
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(56, 25);
            this.toolStripButton_Clear.Text = "清除";
            // 
            // toolStripButton_Select
            // 
            this.toolStripButton_Select.Image = global::FunctionBlock.Properties.Resources.选择光标;
            this.toolStripButton_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Select.Name = "toolStripButton_Select";
            this.toolStripButton_Select.Size = new System.Drawing.Size(56, 25);
            this.toolStripButton_Select.Text = "选择";
            // 
            // toolStripButton_Translate
            // 
            this.toolStripButton_Translate.Image = global::FunctionBlock.Properties.Resources.移动光标;
            this.toolStripButton_Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Translate.Name = "toolStripButton_Translate";
            this.toolStripButton_Translate.Size = new System.Drawing.Size(56, 25);
            this.toolStripButton_Translate.Text = "平移";
            // 
            // toolStripButton_Auto
            // 
            this.toolStripButton_Auto.Image = global::FunctionBlock.Properties.Resources.适配窗口光标;
            this.toolStripButton_Auto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Auto.Name = "toolStripButton_Auto";
            this.toolStripButton_Auto.Size = new System.Drawing.Size(80, 25);
            this.toolStripButton_Auto.Text = "适应窗口";
            // 
            // toolStripButton_3D
            // 
            this.toolStripButton_3D.Image = global::FunctionBlock.Properties.Resources._3D;
            this.toolStripButton_3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_3D.Name = "toolStripButton_3D";
            this.toolStripButton_3D.Size = new System.Drawing.Size(48, 25);
            this.toolStripButton_3D.Text = "3D";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.设置tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 6);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(429, 739);
            this.tabControl1.TabIndex = 9;
            // 
            // 设置tabPage
            // 
            this.设置tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.设置tabPage.Controls.Add(this.tableLayoutPanel2);
            this.设置tabPage.Location = new System.Drawing.Point(4, 22);
            this.设置tabPage.Name = "设置tabPage";
            this.设置tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.设置tabPage.Size = new System.Drawing.Size(421, 713);
            this.设置tabPage.TabIndex = 0;
            this.设置tabPage.Text = "基本设置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox5, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 301F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 195F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(415, 707);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(409, 295);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "坐标拾取";
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.RowHeadersWidth = 60;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(403, 275);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // ShapeCol
            // 
            this.ShapeCol.DataPropertyName = "ShapeType";
            this.ShapeCol.HeaderText = "形状类型";
            this.ShapeCol.Name = "ShapeCol";
            // 
            // WcsRect2Col
            // 
            this.WcsRect2Col.DataPropertyName = "RoiShape";
            this.WcsRect2Col.HeaderText = "形状参数";
            this.WcsRect2Col.Name = "WcsRect2Col";
            this.WcsRect2Col.Width = 120;
            // 
            // TeachCol
            // 
            this.TeachCol.DataPropertyName = "NONE";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "示教";
            this.TeachCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.TeachCol.HeaderText = "示教";
            this.TeachCol.Name = "TeachCol";
            this.TeachCol.Width = 60;
            // 
            // DeletCol
            // 
            this.DeletCol.DataPropertyName = "NONE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "删除";
            this.DeletCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.DeletCol.HeaderText = "删除";
            this.DeletCol.Name = "DeletCol";
            this.DeletCol.Width = 60;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.加载图像Btn);
            this.panel1.Controls.Add(this.Y轴偏移_textBox);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.X轴偏移_textBox);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.保存Btn);
            this.panel1.Controls.Add(this.标定Btn);
            this.panel1.Controls.Add(this.行数numericUpDown);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.列数numericUpDown);
            this.panel1.Controls.Add(this.标定轴comboBox);
            this.panel1.Controls.Add(this.label41);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 499);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 205);
            this.panel1.TabIndex = 5;
            // 
            // 加载图像Btn
            // 
            this.加载图像Btn.Location = new System.Drawing.Point(3, 165);
            this.加载图像Btn.Name = "加载图像Btn";
            this.加载图像Btn.Size = new System.Drawing.Size(75, 32);
            this.加载图像Btn.TabIndex = 66;
            this.加载图像Btn.Text = "加载图像";
            this.加载图像Btn.UseVisualStyleBackColor = true;
            this.加载图像Btn.Click += new System.EventHandler(this.加载图像Btn_Click);
            // 
            // Y轴偏移_textBox
            // 
            this.Y轴偏移_textBox.Location = new System.Drawing.Point(167, 100);
            this.Y轴偏移_textBox.Name = "Y轴偏移_textBox";
            this.Y轴偏移_textBox.Size = new System.Drawing.Size(234, 21);
            this.Y轴偏移_textBox.TabIndex = 65;
            this.Y轴偏移_textBox.Text = "1";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(110, 105);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(59, 12);
            this.label23.TabIndex = 64;
            this.label23.Text = "Y轴偏移：";
            // 
            // X轴偏移_textBox
            // 
            this.X轴偏移_textBox.Location = new System.Drawing.Point(167, 69);
            this.X轴偏移_textBox.Name = "X轴偏移_textBox";
            this.X轴偏移_textBox.Size = new System.Drawing.Size(234, 21);
            this.X轴偏移_textBox.TabIndex = 63;
            this.X轴偏移_textBox.Text = "1";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(110, 74);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(59, 12);
            this.label22.TabIndex = 62;
            this.label22.Text = "X轴偏移：";
            // 
            // 保存Btn
            // 
            this.保存Btn.Location = new System.Drawing.Point(158, 165);
            this.保存Btn.Name = "保存Btn";
            this.保存Btn.Size = new System.Drawing.Size(75, 32);
            this.保存Btn.TabIndex = 48;
            this.保存Btn.Text = "保存";
            this.保存Btn.UseVisualStyleBackColor = true;
            this.保存Btn.Click += new System.EventHandler(this.保存Btn_Click);
            // 
            // 标定Btn
            // 
            this.标定Btn.Location = new System.Drawing.Point(326, 165);
            this.标定Btn.Name = "标定Btn";
            this.标定Btn.Size = new System.Drawing.Size(75, 32);
            this.标定Btn.TabIndex = 47;
            this.标定Btn.Text = "标定";
            this.标定Btn.UseVisualStyleBackColor = true;
            this.标定Btn.Click += new System.EventHandler(this.标定Btn_Click);
            // 
            // 行数numericUpDown
            // 
            this.行数numericUpDown.Location = new System.Drawing.Point(167, 16);
            this.行数numericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.行数numericUpDown.Name = "行数numericUpDown";
            this.行数numericUpDown.Size = new System.Drawing.Size(234, 21);
            this.行数numericUpDown.TabIndex = 45;
            this.行数numericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(112, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 41;
            this.label7.Text = "标定轴：";
            // 
            // 列数numericUpDown
            // 
            this.列数numericUpDown.Location = new System.Drawing.Point(167, 42);
            this.列数numericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.列数numericUpDown.Name = "列数numericUpDown";
            this.列数numericUpDown.Size = new System.Drawing.Size(234, 21);
            this.列数numericUpDown.TabIndex = 46;
            this.列数numericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // 标定轴comboBox
            // 
            this.标定轴comboBox.FormattingEnabled = true;
            this.标定轴comboBox.Items.AddRange(new object[] {
            "XY轴,",
            "X轴,",
            "Y轴,",
            "单轴, "});
            this.标定轴comboBox.Location = new System.Drawing.Point(167, 127);
            this.标定轴comboBox.Name = "标定轴comboBox";
            this.标定轴comboBox.Size = new System.Drawing.Size(234, 20);
            this.标定轴comboBox.TabIndex = 42;
            this.标定轴comboBox.Text = "X轴";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label41.Location = new System.Drawing.Point(124, 20);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(41, 12);
            this.label41.TabIndex = 43;
            this.label41.Text = "行数：";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label30.Location = new System.Drawing.Point(124, 47);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(41, 12);
            this.label30.TabIndex = 44;
            this.label30.Text = "列数：";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.映射dataGridView);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 304);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(409, 189);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "标定矩阵";
            // 
            // 映射dataGridView
            // 
            this.映射dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.映射dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.映射dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.映射dataGridView.Location = new System.Drawing.Point(3, 17);
            this.映射dataGridView.Name = "映射dataGridView";
            this.映射dataGridView.RowHeadersWidth = 51;
            this.映射dataGridView.RowTemplate.Height = 23;
            this.映射dataGridView.Size = new System.Drawing.Size(403, 169);
            this.映射dataGridView.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "C1";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "C2";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "C3";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 125;
            // 
            // statusStrip2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip2, 2);
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
            this.statusStrip2.Location = new System.Drawing.Point(435, 715);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(768, 30);
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
            this.hWindowControl1.Location = new System.Drawing.Point(438, 31);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 4);
            this.hWindowControl1.Size = new System.Drawing.Size(762, 681);
            this.hWindowControl1.TabIndex = 17;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(762, 681);
            // 
            // ManualCalibForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 745);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ManualCalibForm";
            this.ShowIcon = false;
            this.Tag = "1920,1080";
            this.Text = "手动标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManualCalibForm_FormClosing);
            this.Load += new System.EventHandler(this.ManualCalibForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.设置tabPage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.行数numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.列数numericUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.映射dataGridView)).EndInit();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 设置tabPage;
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
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.ToolStripButton toolStripButton_3D;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button 标定Btn;
        private System.Windows.Forms.NumericUpDown 行数numericUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown 列数numericUpDown;
        private System.Windows.Forms.ComboBox 标定轴comboBox;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView 映射dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button 保存Btn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ShapeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WcsRect2Col;
        private System.Windows.Forms.DataGridViewButtonColumn TeachCol;
        private System.Windows.Forms.DataGridViewButtonColumn DeletCol;
        private System.Windows.Forms.TextBox Y轴偏移_textBox;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox X轴偏移_textBox;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button 加载图像Btn;
    }
}