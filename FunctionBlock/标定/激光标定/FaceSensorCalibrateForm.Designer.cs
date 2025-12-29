namespace FunctionBlock
{
    partial class FaceSensorCalibrateForm
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
            this.加载参数button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.激光采集源comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.保存button = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.激光dataGridView = new System.Windows.Forms.DataGridView();
            this.值 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.启用自动校平功能checkBox = new System.Windows.Forms.CheckBox();
            this.标定button = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.X分辨率textBox = new System.Windows.Forms.TextBox();
            this.Y分辨率textBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.阈值textBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.角度textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.步长textBox = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.记录坐标2button = new System.Windows.Forms.Button();
            this.记录坐标1button = new System.Windows.Forms.Button();
            this.Z2_textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Y2_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.X2_textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Z1_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Y1_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.X1_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.标定角度button = new System.Windows.Forms.Button();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.激光dataGridView)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.加载参数button, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.保存button, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1025, 649);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // 加载参数button
            // 
            this.加载参数button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.加载参数button.Location = new System.Drawing.Point(3, 622);
            this.加载参数button.Name = "加载参数button";
            this.加载参数button.Size = new System.Drawing.Size(344, 24);
            this.加载参数button.TabIndex = 53;
            this.加载参数button.Text = "加载参数";
            this.加载参数button.UseVisualStyleBackColor = true;
            this.加载参数button.Click += new System.EventHandler(this.加载参数button_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.激光采集源comboBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 22);
            this.panel1.TabIndex = 27;
            // 
            // 激光采集源comboBox
            // 
            this.激光采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.激光采集源comboBox.FormattingEnabled = true;
            this.激光采集源comboBox.Location = new System.Drawing.Point(52, 1);
            this.激光采集源comboBox.Name = "激光采集源comboBox";
            this.激光采集源comboBox.Size = new System.Drawing.Size(292, 20);
            this.激光采集源comboBox.TabIndex = 31;
            this.激光采集源comboBox.SelectedIndexChanged += new System.EventHandler(this.激光采集源comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 30;
            this.label1.Text = "激光源:";
            // 
            // 保存button
            // 
            this.保存button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.保存button.Location = new System.Drawing.Point(3, 592);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(344, 24);
            this.保存button.TabIndex = 52;
            this.保存button.Text = "保存参数";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 31);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(344, 555);
            this.tabControl1.TabIndex = 34;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(336, 529);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "标定垂直度";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.激光dataGridView, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(330, 523);
            this.tableLayoutPanel2.TabIndex = 55;
            // 
            // 激光dataGridView
            // 
            this.激光dataGridView.AllowUserToAddRows = false;
            this.激光dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.激光dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.激光dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.值});
            this.激光dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.激光dataGridView.Location = new System.Drawing.Point(3, 3);
            this.激光dataGridView.Name = "激光dataGridView";
            this.激光dataGridView.RowHeadersWidth = 100;
            this.tableLayoutPanel2.SetRowSpan(this.激光dataGridView, 2);
            this.激光dataGridView.RowTemplate.Height = 23;
            this.激光dataGridView.Size = new System.Drawing.Size(324, 460);
            this.激光dataGridView.TabIndex = 54;
            // 
            // 值
            // 
            this.值.HeaderText = "值";
            this.值.Name = "值";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.启用自动校平功能checkBox);
            this.panel2.Controls.Add(this.标定button);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 469);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 51);
            this.panel2.TabIndex = 55;
            // 
            // 启用自动校平功能checkBox
            // 
            this.启用自动校平功能checkBox.AutoSize = true;
            this.启用自动校平功能checkBox.Location = new System.Drawing.Point(4, 5);
            this.启用自动校平功能checkBox.Name = "启用自动校平功能checkBox";
            this.启用自动校平功能checkBox.Size = new System.Drawing.Size(120, 16);
            this.启用自动校平功能checkBox.TabIndex = 1;
            this.启用自动校平功能checkBox.Text = "启用自动校平功能";
            this.启用自动校平功能checkBox.UseVisualStyleBackColor = true;
            // 
            // 标定button
            // 
            this.标定button.Location = new System.Drawing.Point(0, 25);
            this.标定button.Name = "标定button";
            this.标定button.Size = new System.Drawing.Size(324, 24);
            this.标定button.TabIndex = 0;
            this.标定button.Text = "标定";
            this.标定button.UseVisualStyleBackColor = true;
            this.标定button.Click += new System.EventHandler(this.标定button_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(336, 466);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "标定旋转角";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(330, 460);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.X分辨率textBox);
            this.groupBox1.Controls.Add(this.Y分辨率textBox);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.阈值textBox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.角度textBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.步长textBox);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.记录坐标2button);
            this.groupBox1.Controls.Add(this.记录坐标1button);
            this.groupBox1.Controls.Add(this.Z2_textBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.Y2_textBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.X2_textBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.Z1_textBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Y1_textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.X1_textBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.标定角度button);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 454);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标定板";
            // 
            // X分辨率textBox
            // 
            this.X分辨率textBox.Location = new System.Drawing.Point(204, 139);
            this.X分辨率textBox.Name = "X分辨率textBox";
            this.X分辨率textBox.Size = new System.Drawing.Size(84, 21);
            this.X分辨率textBox.TabIndex = 39;
            this.X分辨率textBox.Text = "0.01";
            // 
            // Y分辨率textBox
            // 
            this.Y分辨率textBox.Location = new System.Drawing.Point(204, 166);
            this.Y分辨率textBox.Name = "Y分辨率textBox";
            this.Y分辨率textBox.Size = new System.Drawing.Size(84, 21);
            this.Y分辨率textBox.TabIndex = 41;
            this.Y分辨率textBox.Text = "0.01";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(141, 171);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 12);
            this.label19.TabIndex = 40;
            this.label19.Text = "Y分辨率：";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(141, 143);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(59, 12);
            this.label20.TabIndex = 38;
            this.label20.Text = "X分辨率：";
            // 
            // 阈值textBox
            // 
            this.阈值textBox.Location = new System.Drawing.Point(32, 139);
            this.阈值textBox.Name = "阈值textBox";
            this.阈值textBox.Size = new System.Drawing.Size(90, 21);
            this.阈值textBox.TabIndex = 37;
            this.阈值textBox.Text = "128";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 143);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "阈值：";
            // 
            // 角度textBox
            // 
            this.角度textBox.Location = new System.Drawing.Point(32, 193);
            this.角度textBox.Name = "角度textBox";
            this.角度textBox.Size = new System.Drawing.Size(90, 21);
            this.角度textBox.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 34;
            this.label4.Text = "角度：";
            // 
            // 步长textBox
            // 
            this.步长textBox.Location = new System.Drawing.Point(32, 166);
            this.步长textBox.Name = "步长textBox";
            this.步长textBox.Size = new System.Drawing.Size(90, 21);
            this.步长textBox.TabIndex = 33;
            this.步长textBox.Text = "1";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(1, 170);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(41, 12);
            this.label28.TabIndex = 32;
            this.label28.Text = "步长：";
            // 
            // 记录坐标2button
            // 
            this.记录坐标2button.Location = new System.Drawing.Point(215, 104);
            this.记录坐标2button.Name = "记录坐标2button";
            this.记录坐标2button.Size = new System.Drawing.Size(73, 23);
            this.记录坐标2button.TabIndex = 31;
            this.记录坐标2button.Text = "记录坐标2";
            this.记录坐标2button.UseVisualStyleBackColor = true;
            this.记录坐标2button.Click += new System.EventHandler(this.记录坐标2button_Click);
            // 
            // 记录坐标1button
            // 
            this.记录坐标1button.Location = new System.Drawing.Point(37, 104);
            this.记录坐标1button.Name = "记录坐标1button";
            this.记录坐标1button.Size = new System.Drawing.Size(81, 23);
            this.记录坐标1button.TabIndex = 30;
            this.记录坐标1button.Text = "记录坐标1";
            this.记录坐标1button.UseVisualStyleBackColor = true;
            this.记录坐标1button.Click += new System.EventHandler(this.记录坐标1button_Click);
            // 
            // Z2_textBox
            // 
            this.Z2_textBox.Location = new System.Drawing.Point(202, 74);
            this.Z2_textBox.Name = "Z2_textBox";
            this.Z2_textBox.Size = new System.Drawing.Size(93, 21);
            this.Z2_textBox.TabIndex = 29;
            this.Z2_textBox.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(179, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "Z2";
            // 
            // Y2_textBox
            // 
            this.Y2_textBox.Location = new System.Drawing.Point(202, 47);
            this.Y2_textBox.Name = "Y2_textBox";
            this.Y2_textBox.Size = new System.Drawing.Size(93, 21);
            this.Y2_textBox.TabIndex = 27;
            this.Y2_textBox.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(179, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 26;
            this.label7.Text = "Y2";
            // 
            // X2_textBox
            // 
            this.X2_textBox.Location = new System.Drawing.Point(202, 20);
            this.X2_textBox.Name = "X2_textBox";
            this.X2_textBox.Size = new System.Drawing.Size(93, 21);
            this.X2_textBox.TabIndex = 25;
            this.X2_textBox.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(179, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "X2";
            // 
            // Z1_textBox
            // 
            this.Z1_textBox.Location = new System.Drawing.Point(32, 74);
            this.Z1_textBox.Name = "Z1_textBox";
            this.Z1_textBox.Size = new System.Drawing.Size(90, 21);
            this.Z1_textBox.TabIndex = 23;
            this.Z1_textBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "Z1";
            // 
            // Y1_textBox
            // 
            this.Y1_textBox.Location = new System.Drawing.Point(32, 47);
            this.Y1_textBox.Name = "Y1_textBox";
            this.Y1_textBox.Size = new System.Drawing.Size(90, 21);
            this.Y1_textBox.TabIndex = 21;
            this.Y1_textBox.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "Y1";
            // 
            // X1_textBox
            // 
            this.X1_textBox.Location = new System.Drawing.Point(32, 20);
            this.X1_textBox.Name = "X1_textBox";
            this.X1_textBox.Size = new System.Drawing.Size(90, 21);
            this.X1_textBox.TabIndex = 19;
            this.X1_textBox.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "X1";
            // 
            // 标定角度button
            // 
            this.标定角度button.Location = new System.Drawing.Point(6, 406);
            this.标定角度button.Name = "标定角度button";
            this.标定角度button.Size = new System.Drawing.Size(312, 23);
            this.标定角度button.TabIndex = 16;
            this.标定角度button.Text = "标定";
            this.标定角度button.UseVisualStyleBackColor = true;
            this.标定角度button.Click += new System.EventHandler(this.标定角度button_Click);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(353, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 6);
            this.hWindowControl1.Size = new System.Drawing.Size(669, 643);
            this.hWindowControl1.TabIndex = 54;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(669, 643);
            // 
            // FaceSensorCalibrateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 649);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FaceSensorCalibrateForm";
            this.Text = "面激光标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LaserCameraCalibrateForm_FormClosing);
            this.Load += new System.EventHandler(this.FaceSensorCalibrateForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.激光dataGridView)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.ComboBox 激光采集源comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 加载参数button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView 激光dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 值;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button 标定button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox 阈值textBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox 角度textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 步长textBox;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button 记录坐标2button;
        private System.Windows.Forms.Button 记录坐标1button;
        private System.Windows.Forms.TextBox Z2_textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Y2_textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox X2_textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Z1_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Y1_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox X1_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button 标定角度button;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox 启用自动校平功能checkBox;
        private System.Windows.Forms.TextBox X分辨率textBox;
        private System.Windows.Forms.TextBox Y分辨率textBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
    }
}