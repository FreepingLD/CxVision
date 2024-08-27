namespace FunctionBlock
{
    partial class FlawDetecteViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlawDetecteViewForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.titleLabel = new System.Windows.Forms.Label();
            this.传感器comboBox1 = new System.Windows.Forms.ComboBox();
            this.实时采集checkBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.edge3Panel = new System.Windows.Forms.Panel();
            this.edge2Panel = new System.Windows.Forms.Panel();
            this.edge1Panel = new System.Windows.Forms.Panel();
            this.SignLabel = new System.Windows.Forms.Label();
            this.程序节点comboBox = new System.Windows.Forms.ComboBox();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.瑕疵数据dataGridView = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.边缘数量textBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.边缘3图像数量textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.边缘2图像数量textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.边缘1图像数量textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.按钮高度textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.按钮宽度textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.当前检测结果label = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.加载图片button = new System.Windows.Forms.Button();
            this.保存图片checkBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.产品编号textBox = new System.Windows.Forms.TextBox();
            this.测试Btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.瑕疵数据dataGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.AllowDrop = true;
            this.hWindowControl1.BackColor = System.Drawing.Color.Gray;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Gray;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(981, 388);
            this.hWindowControl1.TabIndex = 2;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(981, 388);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 5);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(898, 16);
            this.titleLabel.TabIndex = 22;
            this.titleLabel.Text = "CxVision";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // 传感器comboBox1
            // 
            this.传感器comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.传感器comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.传感器comboBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.传感器comboBox1.FormattingEnabled = true;
            this.传感器comboBox1.ItemHeight = 12;
            this.传感器comboBox1.Location = new System.Drawing.Point(55, 20);
            this.传感器comboBox1.Margin = new System.Windows.Forms.Padding(0);
            this.传感器comboBox1.Name = "传感器comboBox1";
            this.传感器comboBox1.Size = new System.Drawing.Size(213, 20);
            this.传感器comboBox1.TabIndex = 3;
            this.传感器comboBox1.SelectionChangeCommitted += new System.EventHandler(this.传感器comboBox_SelectionChangeCommitted);
            // 
            // 实时采集checkBox
            // 
            this.实时采集checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.实时采集checkBox.BackColor = System.Drawing.Color.Lime;
            this.实时采集checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.实时采集checkBox.Location = new System.Drawing.Point(0, 20);
            this.实时采集checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.实时采集checkBox.Name = "实时采集checkBox";
            this.实时采集checkBox.Size = new System.Drawing.Size(20, 20);
            this.实时采集checkBox.TabIndex = 19;
            this.实时采集checkBox.TabStop = false;
            this.实时采集checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.实时采集checkBox.UseVisualStyleBackColor = false;
            this.实时采集checkBox.CheckedChanged += new System.EventHandler(this.实时采集checkBox_CheckedChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 213F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 195F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.edge3Panel, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.edge2Panel, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.edge1Panel, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.SignLabel, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.程序节点comboBox, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonMax, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonClose, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMin, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.传感器comboBox1, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.实时采集checkBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label5, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 10;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(983, 906);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // edge3Panel
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.edge3Panel, 8);
            this.edge3Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edge3Panel.Location = new System.Drawing.Point(1, 835);
            this.edge3Panel.Margin = new System.Windows.Forms.Padding(1);
            this.edge3Panel.Name = "edge3Panel";
            this.edge3Panel.Size = new System.Drawing.Size(981, 70);
            this.edge3Panel.TabIndex = 30;
            // 
            // edge2Panel
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.edge2Panel, 8);
            this.edge2Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edge2Panel.Location = new System.Drawing.Point(1, 763);
            this.edge2Panel.Margin = new System.Windows.Forms.Padding(1);
            this.edge2Panel.Name = "edge2Panel";
            this.edge2Panel.Size = new System.Drawing.Size(981, 70);
            this.edge2Panel.TabIndex = 30;
            // 
            // edge1Panel
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.edge1Panel, 8);
            this.edge1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edge1Panel.Location = new System.Drawing.Point(1, 693);
            this.edge1Panel.Margin = new System.Windows.Forms.Padding(1);
            this.edge1Panel.Name = "edge1Panel";
            this.tableLayoutPanel2.SetRowSpan(this.edge1Panel, 2);
            this.edge1Panel.Size = new System.Drawing.Size(981, 68);
            this.edge1Panel.TabIndex = 30;
            // 
            // SignLabel
            // 
            this.SignLabel.AutoSize = true;
            this.SignLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SignLabel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SignLabel.Location = new System.Drawing.Point(271, 23);
            this.SignLabel.Margin = new System.Windows.Forms.Padding(3);
            this.SignLabel.Name = "SignLabel";
            this.SignLabel.Size = new System.Drawing.Size(433, 14);
            this.SignLabel.TabIndex = 3;
            this.SignLabel.Text = "label1";
            this.SignLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 程序节点comboBox
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.程序节点comboBox, 4);
            this.程序节点comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.程序节点comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.程序节点comboBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.程序节点comboBox.FormattingEnabled = true;
            this.程序节点comboBox.ItemHeight = 12;
            this.程序节点comboBox.Location = new System.Drawing.Point(707, 20);
            this.程序节点comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.程序节点comboBox.Name = "程序节点comboBox";
            this.程序节点comboBox.Size = new System.Drawing.Size(276, 20);
            this.程序节点comboBox.TabIndex = 24;
            this.程序节点comboBox.DropDown += new System.EventHandler(this.程序节点comboBox_DropDown);
            this.程序节点comboBox.SelectionChangeCommitted += new System.EventHandler(this.程序节点comboBox_SelectionChangeCommitted);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(929, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(27, 20);
            this.buttonMax.TabIndex = 1;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(956, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(27, 20);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Enabled = false;
            this.buttonMin.Location = new System.Drawing.Point(902, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(27, 20);
            this.buttonMin.TabIndex = 2;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // panel1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 8);
            this.panel1.Controls.Add(this.hWindowControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel2.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(981, 388);
            this.panel1.TabIndex = 25;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.tabControl1, 8);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 433);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(977, 202);
            this.tabControl1.TabIndex = 34;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.瑕疵数据dataGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(969, 176);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "缺陷数据";
            // 
            // 瑕疵数据dataGridView
            // 
            this.瑕疵数据dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.瑕疵数据dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.瑕疵数据dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.瑕疵数据dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.瑕疵数据dataGridView.Location = new System.Drawing.Point(3, 3);
            this.瑕疵数据dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.瑕疵数据dataGridView.Name = "瑕疵数据dataGridView";
            this.瑕疵数据dataGridView.RowTemplate.Height = 23;
            this.瑕疵数据dataGridView.Size = new System.Drawing.Size(963, 170);
            this.瑕疵数据dataGridView.TabIndex = 31;
            this.瑕疵数据dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.瑕疵数据dataGridView_CellContentClick);
            this.瑕疵数据dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.瑕疵数据dataGridView_DataError);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.边缘数量textBox);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.边缘3图像数量textBox);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.边缘2图像数量textBox);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.边缘1图像数量textBox);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.按钮高度textBox);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.按钮宽度textBox);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(969, 176);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "参数";
            // 
            // 边缘数量textBox
            // 
            this.边缘数量textBox.Location = new System.Drawing.Point(64, 69);
            this.边缘数量textBox.Name = "边缘数量textBox";
            this.边缘数量textBox.Size = new System.Drawing.Size(100, 21);
            this.边缘数量textBox.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "边缘数量";
            // 
            // 边缘3图像数量textBox
            // 
            this.边缘3图像数量textBox.Location = new System.Drawing.Point(264, 69);
            this.边缘3图像数量textBox.Name = "边缘3图像数量textBox";
            this.边缘3图像数量textBox.Size = new System.Drawing.Size(100, 21);
            this.边缘3图像数量textBox.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(178, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "边缘3图像数量";
            // 
            // 边缘2图像数量textBox
            // 
            this.边缘2图像数量textBox.Location = new System.Drawing.Point(264, 42);
            this.边缘2图像数量textBox.Name = "边缘2图像数量textBox";
            this.边缘2图像数量textBox.Size = new System.Drawing.Size(100, 21);
            this.边缘2图像数量textBox.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(178, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "边缘2图像数量";
            // 
            // 边缘1图像数量textBox
            // 
            this.边缘1图像数量textBox.Location = new System.Drawing.Point(264, 15);
            this.边缘1图像数量textBox.Name = "边缘1图像数量textBox";
            this.边缘1图像数量textBox.Size = new System.Drawing.Size(100, 21);
            this.边缘1图像数量textBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "边缘1图像数量";
            // 
            // 按钮高度textBox
            // 
            this.按钮高度textBox.Location = new System.Drawing.Point(64, 42);
            this.按钮高度textBox.Name = "按钮高度textBox";
            this.按钮高度textBox.Size = new System.Drawing.Size(100, 21);
            this.按钮高度textBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "按钮高度";
            // 
            // 按钮宽度textBox
            // 
            this.按钮宽度textBox.Location = new System.Drawing.Point(64, 15);
            this.按钮宽度textBox.Name = "按钮宽度textBox";
            this.按钮宽度textBox.Size = new System.Drawing.Size(100, 21);
            this.按钮宽度textBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "按钮宽度";
            // 
            // panel2
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel2, 8);
            this.panel2.Controls.Add(this.当前检测结果label);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.加载图片button);
            this.panel2.Controls.Add(this.保存图片checkBox);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.产品编号textBox);
            this.panel2.Controls.Add(this.测试Btn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 641);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(977, 48);
            this.panel2.TabIndex = 35;
            // 
            // 当前检测结果label
            // 
            this.当前检测结果label.AutoSize = true;
            this.当前检测结果label.Location = new System.Drawing.Point(474, 18);
            this.当前检测结果label.Name = "当前检测结果label";
            this.当前检测结果label.Size = new System.Drawing.Size(47, 12);
            this.当前检测结果label.TabIndex = 7;
            this.当前检测结果label.Text = "Waiting";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(384, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "当前检测结果:";
            // 
            // 加载图片button
            // 
            this.加载图片button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.加载图片button.Location = new System.Drawing.Point(784, 4);
            this.加载图片button.Name = "加载图片button";
            this.加载图片button.Size = new System.Drawing.Size(86, 41);
            this.加载图片button.TabIndex = 4;
            this.加载图片button.Text = "加载图片";
            this.加载图片button.UseVisualStyleBackColor = true;
            this.加载图片button.Click += new System.EventHandler(this.加载图片button_Click);
            // 
            // 保存图片checkBox
            // 
            this.保存图片checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.保存图片checkBox.AutoSize = true;
            this.保存图片checkBox.Location = new System.Drawing.Point(708, 17);
            this.保存图片checkBox.Name = "保存图片checkBox";
            this.保存图片checkBox.Size = new System.Drawing.Size(72, 16);
            this.保存图片checkBox.TabIndex = 3;
            this.保存图片checkBox.Text = "保存图片";
            this.保存图片checkBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(5, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "产品编号:";
            // 
            // 产品编号textBox
            // 
            this.产品编号textBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.产品编号textBox.Location = new System.Drawing.Point(64, 14);
            this.产品编号textBox.Name = "产品编号textBox";
            this.产品编号textBox.Size = new System.Drawing.Size(312, 21);
            this.产品编号textBox.TabIndex = 1;
            // 
            // 测试Btn
            // 
            this.测试Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.测试Btn.Location = new System.Drawing.Point(888, 3);
            this.测试Btn.Name = "测试Btn";
            this.测试Btn.Size = new System.Drawing.Size(86, 41);
            this.测试Btn.TabIndex = 0;
            this.测试Btn.Text = "测试按钮";
            this.测试Btn.UseVisualStyleBackColor = true;
            this.测试Btn.Click += new System.EventHandler(this.测试Btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(23, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 20);
            this.label5.TabIndex = 36;
            this.label5.Text = "实时";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FlawDetecteViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(983, 906);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FlawDetecteViewForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlawDetecteViewForm_FormClosing);
            this.Load += new System.EventHandler(this.FlawDetecteViewForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FlawDetecteViewForm_MouseDown);
            this.Move += new System.EventHandler(this.FlawDetecteViewForm_Move);
            this.Resize += new System.EventHandler(this.FlawDetecteViewForm_Resize);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.瑕疵数据dataGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox 实时采集checkBox;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox 传感器comboBox1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ComboBox 程序节点comboBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label SignLabel;
        private System.Windows.Forms.Panel edge1Panel;
        private System.Windows.Forms.DataGridView 瑕疵数据dataGridView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox 边缘1图像数量textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 按钮高度textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 按钮宽度textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button 测试Btn;
        private System.Windows.Forms.CheckBox 保存图片checkBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 产品编号textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button 加载图片button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel edge3Panel;
        private System.Windows.Forms.Panel edge2Panel;
        private System.Windows.Forms.TextBox 边缘数量textBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox 边缘3图像数量textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox 边缘2图像数量textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label 当前检测结果label;
    }
}