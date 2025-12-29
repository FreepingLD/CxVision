namespace FunctionBlock
{
    partial class PointSensorCalibrateForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.激光采集源comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.标定button = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.加载参数button = new System.Windows.Forms.Button();
            this.保存button = new System.Windows.Forms.Button();
            this.激光dataGridView = new System.Windows.Forms.DataGridView();
            this.值 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.台阶标准高度textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.获取当前位置1button = new System.Windows.Forms.Button();
            this.X1坐标textBox = new System.Windows.Forms.TextBox();
            this.Y1坐标textBox = new System.Windows.Forms.TextBox();
            this.Z1坐标textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.获取当前位置2button = new System.Windows.Forms.Button();
            this.X2坐标textBox = new System.Windows.Forms.TextBox();
            this.Y2坐标textBox = new System.Windows.Forms.TextBox();
            this.Z2坐标textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FormPanel = new System.Windows.Forms.Panel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.激光dataGridView)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 467F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.FormPanel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 178F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1320, 752);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.激光采集源comboBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(465, 33);
            this.panel1.TabIndex = 27;
            // 
            // 激光采集源comboBox
            // 
            this.激光采集源comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.激光采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.激光采集源comboBox.FormattingEnabled = true;
            this.激光采集源comboBox.Location = new System.Drawing.Point(107, 4);
            this.激光采集源comboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.激光采集源comboBox.Name = "激光采集源comboBox";
            this.激光采集源comboBox.Size = new System.Drawing.Size(351, 23);
            this.激光采集源comboBox.TabIndex = 31;
            this.激光采集源comboBox.SelectedIndexChanged += new System.EventHandler(this.激光采集源comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 30;
            this.label1.Text = "激光:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.标定button);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 717);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(459, 31);
            this.panel2.TabIndex = 32;
            // 
            // 标定button
            // 
            this.标定button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.标定button.Location = new System.Drawing.Point(0, 0);
            this.标定button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.标定button.Name = "标定button";
            this.标定button.Size = new System.Drawing.Size(459, 31);
            this.标定button.TabIndex = 0;
            this.标定button.Text = "标定";
            this.标定button.UseVisualStyleBackColor = true;
            this.标定button.Click += new System.EventHandler(this.标定button_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 255);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(459, 454);
            this.tabControl1.TabIndex = 34;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(451, 425);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "标定结果";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.加载参数button, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.保存button, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.激光dataGridView, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(443, 417);
            this.tableLayoutPanel2.TabIndex = 55;
            // 
            // 加载参数button
            // 
            this.加载参数button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.加载参数button.Location = new System.Drawing.Point(4, 383);
            this.加载参数button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.加载参数button.Name = "加载参数button";
            this.加载参数button.Size = new System.Drawing.Size(435, 30);
            this.加载参数button.TabIndex = 53;
            this.加载参数button.Text = "加载参数";
            this.加载参数button.UseVisualStyleBackColor = true;
            this.加载参数button.Click += new System.EventHandler(this.加载参数button_Click);
            // 
            // 保存button
            // 
            this.保存button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.保存button.Location = new System.Drawing.Point(4, 345);
            this.保存button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(435, 30);
            this.保存button.TabIndex = 52;
            this.保存button.Text = "保存参数";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // 激光dataGridView
            // 
            this.激光dataGridView.AllowUserToAddRows = false;
            this.激光dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.激光dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.激光dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.值});
            this.激光dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.激光dataGridView.Location = new System.Drawing.Point(4, 4);
            this.激光dataGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.激光dataGridView.Name = "激光dataGridView";
            this.激光dataGridView.RowHeadersWidth = 100;
            this.tableLayoutPanel2.SetRowSpan(this.激光dataGridView, 2);
            this.激光dataGridView.RowTemplate.Height = 23;
            this.激光dataGridView.Size = new System.Drawing.Size(435, 333);
            this.激光dataGridView.TabIndex = 54;
            // 
            // 值
            // 
            this.值.HeaderText = "值";
            this.值.MinimumWidth = 6;
            this.值.Name = "值";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.台阶标准高度textBox);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 35);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(467, 38);
            this.panel3.TabIndex = 35;
            // 
            // 台阶标准高度textBox
            // 
            this.台阶标准高度textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.台阶标准高度textBox.Location = new System.Drawing.Point(111, 6);
            this.台阶标准高度textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.台阶标准高度textBox.Name = "台阶标准高度textBox";
            this.台阶标准高度textBox.Size = new System.Drawing.Size(345, 25);
            this.台阶标准高度textBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "台阶标准高度：";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Controls.Add(this.groupBox3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(4, 77);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(459, 170);
            this.panel4.TabIndex = 36;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.获取当前位置1button);
            this.groupBox1.Controls.Add(this.X1坐标textBox);
            this.groupBox1.Controls.Add(this.Y1坐标textBox);
            this.groupBox1.Controls.Add(this.Z1坐标textBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(221, 156);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "激光点1";
            // 
            // 获取当前位置1button
            // 
            this.获取当前位置1button.Location = new System.Drawing.Point(73, 125);
            this.获取当前位置1button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.获取当前位置1button.Name = "获取当前位置1button";
            this.获取当前位置1button.Size = new System.Drawing.Size(139, 29);
            this.获取当前位置1button.TabIndex = 6;
            this.获取当前位置1button.Text = "获取当前激光值1";
            this.获取当前位置1button.UseVisualStyleBackColor = true;
            this.获取当前位置1button.Click += new System.EventHandler(this.获取当前位置1button_Click);
            // 
            // X1坐标textBox
            // 
            this.X1坐标textBox.Location = new System.Drawing.Point(73, 21);
            this.X1坐标textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.X1坐标textBox.Name = "X1坐标textBox";
            this.X1坐标textBox.Size = new System.Drawing.Size(137, 25);
            this.X1坐标textBox.TabIndex = 1;
            this.X1坐标textBox.Text = "0";
            // 
            // Y1坐标textBox
            // 
            this.Y1坐标textBox.Location = new System.Drawing.Point(73, 55);
            this.Y1坐标textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Y1坐标textBox.Name = "Y1坐标textBox";
            this.Y1坐标textBox.Size = new System.Drawing.Size(137, 25);
            this.Y1坐标textBox.TabIndex = 3;
            this.Y1坐标textBox.Text = "0";
            // 
            // Z1坐标textBox
            // 
            this.Z1坐标textBox.Location = new System.Drawing.Point(73, 89);
            this.Z1坐标textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Z1坐标textBox.Name = "Z1坐标textBox";
            this.Z1坐标textBox.Size = new System.Drawing.Size(137, 25);
            this.Z1坐标textBox.TabIndex = 5;
            this.Z1坐标textBox.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Y1:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "X1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 94);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "激光值:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.获取当前位置2button);
            this.groupBox3.Controls.Add(this.X2坐标textBox);
            this.groupBox3.Controls.Add(this.Y2坐标textBox);
            this.groupBox3.Controls.Add(this.Z2坐标textBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(233, 5);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(220, 155);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "激光点2";
            // 
            // 获取当前位置2button
            // 
            this.获取当前位置2button.Location = new System.Drawing.Point(71, 124);
            this.获取当前位置2button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.获取当前位置2button.Name = "获取当前位置2button";
            this.获取当前位置2button.Size = new System.Drawing.Size(147, 29);
            this.获取当前位置2button.TabIndex = 13;
            this.获取当前位置2button.Text = "获取当前激光值2";
            this.获取当前位置2button.UseVisualStyleBackColor = true;
            this.获取当前位置2button.Click += new System.EventHandler(this.获取当前位置2button_Click);
            // 
            // X2坐标textBox
            // 
            this.X2坐标textBox.Location = new System.Drawing.Point(71, 21);
            this.X2坐标textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.X2坐标textBox.Name = "X2坐标textBox";
            this.X2坐标textBox.Size = new System.Drawing.Size(145, 25);
            this.X2坐标textBox.TabIndex = 8;
            this.X2坐标textBox.Text = "0";
            // 
            // Y2坐标textBox
            // 
            this.Y2坐标textBox.Location = new System.Drawing.Point(71, 55);
            this.Y2坐标textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Y2坐标textBox.Name = "Y2坐标textBox";
            this.Y2坐标textBox.Size = new System.Drawing.Size(145, 25);
            this.Y2坐标textBox.TabIndex = 10;
            this.Y2坐标textBox.Text = "0";
            // 
            // Z2坐标textBox
            // 
            this.Z2坐标textBox.Location = new System.Drawing.Point(71, 89);
            this.Z2坐标textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Z2坐标textBox.Name = "Z2坐标textBox";
            this.Z2坐标textBox.Size = new System.Drawing.Size(145, 25);
            this.Z2坐标textBox.TabIndex = 12;
            this.Z2坐标textBox.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 60);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "Y2:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(36, 26);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 15);
            this.label8.TabIndex = 7;
            this.label8.Text = "X2:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 94);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "激光值:";
            // 
            // FormPanel
            // 
            this.FormPanel.Controls.Add(this.hWindowControl1);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormPanel.Location = new System.Drawing.Point(471, 4);
            this.FormPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FormPanel.Name = "FormPanel";
            this.tableLayoutPanel1.SetRowSpan(this.FormPanel, 7);
            this.FormPanel.Size = new System.Drawing.Size(845, 744);
            this.FormPanel.TabIndex = 37;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(845, 744);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(845, 744);
            // 
            // PointSensorCalibrateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 752);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PointSensorCalibrateForm";
            this.Text = "点激光标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LaserCameraCalibrateForm_FormClosing);
            this.Load += new System.EventHandler(this.PointSensorCalibrateForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.激光dataGridView)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button 标定button;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.ComboBox 激光采集源comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 加载参数button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView 激光dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 值;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox 台阶标准高度textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button 获取当前位置1button;
        private System.Windows.Forms.TextBox X1坐标textBox;
        private System.Windows.Forms.TextBox Y1坐标textBox;
        private System.Windows.Forms.TextBox Z1坐标textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button 获取当前位置2button;
        private System.Windows.Forms.TextBox X2坐标textBox;
        private System.Windows.Forms.TextBox Y2坐标textBox;
        private System.Windows.Forms.TextBox Z2坐标textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel FormPanel;
        private HalconDotNet.HWindowControl hWindowControl1;
    }
}