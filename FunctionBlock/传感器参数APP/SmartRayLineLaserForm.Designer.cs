namespace FunctionBlock
{
    partial class SmartRayLineLaserParamForm
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.触发方向comboBox = new System.Windows.Forms.ComboBox();
            this.触发延时textBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.触发分频textBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.外部触发源comboBox = new System.Windows.Forms.ComboBox();
            this.内部触发频率textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.触发模式comBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.图像采集类型comboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.激光线阈值2textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.测量点数textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.激光线阈值1textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.多重曝光合并模comboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.光源模式comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.曝光时间2texBox = new System.Windows.Forms.TextBox();
            this.光源亮度textBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.曝光模式comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_lbl_Shutter = new System.Windows.Forms.Label();
            this.曝光时间1texBox = new System.Windows.Forms.TextBox();
            this.增益texBox = new System.Windows.Forms.TextBox();
            this.m_lbl_Gain = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.监控panel = new System.Windows.Forms.Panel();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.触发方向comboBox);
            this.groupBox3.Controls.Add(this.触发延时textBox);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.触发分频textBox);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.外部触发源comboBox);
            this.groupBox3.Controls.Add(this.内部触发频率textBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.触发模式comBox);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(273, 475);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "触发参数";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 148);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 53;
            this.label15.Text = "触发方向";
            // 
            // 触发方向comboBox
            // 
            this.触发方向comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.触发方向comboBox.FormattingEnabled = true;
            this.触发方向comboBox.Items.AddRange(new object[] {
            "软触发",
            "硬触发"});
            this.触发方向comboBox.Location = new System.Drawing.Point(87, 147);
            this.触发方向comboBox.Name = "触发方向comboBox";
            this.触发方向comboBox.Size = new System.Drawing.Size(183, 20);
            this.触发方向comboBox.TabIndex = 54;
            this.触发方向comboBox.SelectionChangeCommitted += new System.EventHandler(this.触发方向comboBox_SelectionChangeCommitted);
            // 
            // 触发延时textBox
            // 
            this.触发延时textBox.Location = new System.Drawing.Point(87, 121);
            this.触发延时textBox.Name = "触发延时textBox";
            this.触发延时textBox.Size = new System.Drawing.Size(183, 21);
            this.触发延时textBox.TabIndex = 52;
            this.触发延时textBox.Text = "0";
            this.触发延时textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.触发延时textBox_KeyUp);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 122);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 51;
            this.label14.Text = "触发延时";
            // 
            // 触发分频textBox
            // 
            this.触发分频textBox.Location = new System.Drawing.Point(87, 94);
            this.触发分频textBox.Name = "触发分频textBox";
            this.触发分频textBox.Size = new System.Drawing.Size(183, 21);
            this.触发分频textBox.TabIndex = 50;
            this.触发分频textBox.Text = "10";
            this.触发分频textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.触发分频textBox_KeyUp);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 49;
            this.label13.Text = "触发分频";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 47;
            this.label12.Text = "外部触发源";
            // 
            // 外部触发源comboBox
            // 
            this.外部触发源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.外部触发源comboBox.FormattingEnabled = true;
            this.外部触发源comboBox.Items.AddRange(new object[] {
            "软触发",
            "硬触发"});
            this.外部触发源comboBox.Location = new System.Drawing.Point(87, 68);
            this.外部触发源comboBox.Name = "外部触发源comboBox";
            this.外部触发源comboBox.Size = new System.Drawing.Size(183, 20);
            this.外部触发源comboBox.TabIndex = 48;
            this.外部触发源comboBox.SelectionChangeCommitted += new System.EventHandler(this.外部触发源comboBox_SelectionChangeCommitted);
            // 
            // 内部触发频率textBox
            // 
            this.内部触发频率textBox.Location = new System.Drawing.Point(87, 42);
            this.内部触发频率textBox.Name = "内部触发频率textBox";
            this.内部触发频率textBox.Size = new System.Drawing.Size(183, 21);
            this.内部触发频率textBox.TabIndex = 46;
            this.内部触发频率textBox.Text = "10";
            this.内部触发频率textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.内部触发频率textBox_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "内部触发频率";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 43;
            this.label4.Text = "触发模式";
            // 
            // 触发模式comBox
            // 
            this.触发模式comBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.触发模式comBox.FormattingEnabled = true;
            this.触发模式comBox.Items.AddRange(new object[] {
            "软触发",
            "硬触发"});
            this.触发模式comBox.Location = new System.Drawing.Point(87, 16);
            this.触发模式comBox.Name = "触发模式comBox";
            this.触发模式comBox.Size = new System.Drawing.Size(183, 20);
            this.触发模式comBox.TabIndex = 44;
            this.触发模式comBox.SelectionChangeCommitted += new System.EventHandler(this.触发模式comBox_SelectionChangeCommitted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.图像采集类型comboBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.激光线阈值2textBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.测量点数textBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.激光线阈值1textBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 475);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "3D抓取";
            // 
            // 图像采集类型comboBox
            // 
            this.图像采集类型comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.图像采集类型comboBox.FormattingEnabled = true;
            this.图像采集类型comboBox.Location = new System.Drawing.Point(144, 96);
            this.图像采集类型comboBox.Name = "图像采集类型comboBox";
            this.图像采集类型comboBox.Size = new System.Drawing.Size(128, 20);
            this.图像采集类型comboBox.TabIndex = 53;
            this.图像采集类型comboBox.SelectionChangeCommitted += new System.EventHandler(this.图像采集类型comboBox_SelectionChangeCommitted);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 52;
            this.label10.Text = "图像采集类型";
            // 
            // 激光线阈值2textBox
            // 
            this.激光线阈值2textBox.Location = new System.Drawing.Point(144, 42);
            this.激光线阈值2textBox.Name = "激光线阈值2textBox";
            this.激光线阈值2textBox.Size = new System.Drawing.Size(128, 21);
            this.激光线阈值2textBox.TabIndex = 57;
            this.激光线阈值2textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.激光线阈值2textBox_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 56;
            this.label5.Text = "激光线阈值2";
            // 
            // 测量点数textBox
            // 
            this.测量点数textBox.Location = new System.Drawing.Point(144, 69);
            this.测量点数textBox.Name = "测量点数textBox";
            this.测量点数textBox.Size = new System.Drawing.Size(128, 21);
            this.测量点数textBox.TabIndex = 55;
            this.测量点数textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.测量点数textBox_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 12);
            this.label7.TabIndex = 54;
            this.label7.Text = "测量轮廓数(1~100000)";
            // 
            // 激光线阈值1textBox
            // 
            this.激光线阈值1textBox.Location = new System.Drawing.Point(144, 16);
            this.激光线阈值1textBox.Name = "激光线阈值1textBox";
            this.激光线阈值1textBox.Size = new System.Drawing.Size(128, 21);
            this.激光线阈值1textBox.TabIndex = 51;
            this.激光线阈值1textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.激光线阈值1textBox_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 50;
            this.label8.Text = "激光线阈值1";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.多重曝光合并模comboBox);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.光源模式comboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.曝光时间2texBox);
            this.groupBox1.Controls.Add(this.光源亮度textBox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.曝光模式comboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_lbl_Shutter);
            this.groupBox1.Controls.Add(this.曝光时间1texBox);
            this.groupBox1.Controls.Add(this.增益texBox);
            this.groupBox1.Controls.Add(this.m_lbl_Gain);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 548);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机光源参数";
            // 
            // 多重曝光合并模comboBox
            // 
            this.多重曝光合并模comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.多重曝光合并模comboBox.FormattingEnabled = true;
            this.多重曝光合并模comboBox.Location = new System.Drawing.Point(144, 90);
            this.多重曝光合并模comboBox.Name = "多重曝光合并模comboBox";
            this.多重曝光合并模comboBox.Size = new System.Drawing.Size(123, 20);
            this.多重曝光合并模comboBox.TabIndex = 58;
            this.多重曝光合并模comboBox.TextChanged += new System.EventHandler(this.多重曝光合并模comboBox_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 12);
            this.label11.TabIndex = 57;
            this.label11.Text = "多重曝光图像合并模式";
            // 
            // 光源模式comboBox
            // 
            this.光源模式comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.光源模式comboBox.FormattingEnabled = true;
            this.光源模式comboBox.Location = new System.Drawing.Point(144, 168);
            this.光源模式comboBox.Name = "光源模式comboBox";
            this.光源模式comboBox.Size = new System.Drawing.Size(123, 20);
            this.光源模式comboBox.TabIndex = 56;
            this.光源模式comboBox.TextChanged += new System.EventHandler(this.光源模式comboBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 55;
            this.label3.Text = "光源模式";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 12);
            this.label6.TabIndex = 53;
            this.label6.Text = "曝光时间2（20~10000us)";
            // 
            // 曝光时间2texBox
            // 
            this.曝光时间2texBox.Location = new System.Drawing.Point(144, 63);
            this.曝光时间2texBox.Name = "曝光时间2texBox";
            this.曝光时间2texBox.Size = new System.Drawing.Size(123, 21);
            this.曝光时间2texBox.TabIndex = 54;
            this.曝光时间2texBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.曝光时间2texBox_KeyUp);
            // 
            // 光源亮度textBox
            // 
            this.光源亮度textBox.Location = new System.Drawing.Point(144, 141);
            this.光源亮度textBox.Name = "光源亮度textBox";
            this.光源亮度textBox.Size = new System.Drawing.Size(123, 21);
            this.光源亮度textBox.TabIndex = 52;
            this.光源亮度textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.光源亮度textBox_KeyUp);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 51;
            this.label9.Text = "光源亮度";
            // 
            // 曝光模式comboBox
            // 
            this.曝光模式comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.曝光模式comboBox.FormattingEnabled = true;
            this.曝光模式comboBox.Location = new System.Drawing.Point(144, 14);
            this.曝光模式comboBox.Name = "曝光模式comboBox";
            this.曝光模式comboBox.Size = new System.Drawing.Size(123, 20);
            this.曝光模式comboBox.TabIndex = 50;
            this.曝光模式comboBox.TextChanged += new System.EventHandler(this.曝光模式comboBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 49;
            this.label2.Text = "曝光模式";
            // 
            // m_lbl_Shutter
            // 
            this.m_lbl_Shutter.AutoSize = true;
            this.m_lbl_Shutter.Location = new System.Drawing.Point(8, 40);
            this.m_lbl_Shutter.Name = "m_lbl_Shutter";
            this.m_lbl_Shutter.Size = new System.Drawing.Size(137, 12);
            this.m_lbl_Shutter.TabIndex = 45;
            this.m_lbl_Shutter.Text = "曝光时间1（20~10000us)";
            // 
            // 曝光时间1texBox
            // 
            this.曝光时间1texBox.Location = new System.Drawing.Point(144, 38);
            this.曝光时间1texBox.Name = "曝光时间1texBox";
            this.曝光时间1texBox.Size = new System.Drawing.Size(123, 21);
            this.曝光时间1texBox.TabIndex = 46;
            this.曝光时间1texBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.曝光时间1texBox_KeyUp);
            // 
            // 增益texBox
            // 
            this.增益texBox.Location = new System.Drawing.Point(144, 115);
            this.增益texBox.Name = "增益texBox";
            this.增益texBox.Size = new System.Drawing.Size(123, 21);
            this.增益texBox.TabIndex = 48;
            this.增益texBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.增益texBox_KeyUp);
            // 
            // m_lbl_Gain
            // 
            this.m_lbl_Gain.AutoSize = true;
            this.m_lbl_Gain.Location = new System.Drawing.Point(8, 116);
            this.m_lbl_Gain.Name = "m_lbl_Gain";
            this.m_lbl_Gain.Size = new System.Drawing.Size(65, 12);
            this.m_lbl_Gain.TabIndex = 47;
            this.m_lbl_Gain.Text = "增益(1~16)";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(287, 580);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(279, 554);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "设置相机参数";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(279, 481);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "设置抓取参数";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(279, 481);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "设置触发参数";
            // 
            // 监控panel
            // 
            this.监控panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.监控panel.Location = new System.Drawing.Point(294, 0);
            this.监控panel.Name = "监控panel";
            this.监控panel.Size = new System.Drawing.Size(661, 578);
            this.监控panel.TabIndex = 26;
            // 
            // SmartRayLineLaserParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(956, 582);
            this.Controls.Add(this.监控panel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SmartRayLineLaserParamForm";
            this.Text = "SmartRay激光窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SmartRayLineLaserForm_FormClosing);
            this.Load += new System.EventHandler(this.SmartRayLineLaserForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox 多重曝光合并模comboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox 光源模式comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox 曝光时间2texBox;
        private System.Windows.Forms.TextBox 光源亮度textBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox 曝光模式comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label m_lbl_Shutter;
        private System.Windows.Forms.TextBox 曝光时间1texBox;
        private System.Windows.Forms.TextBox 增益texBox;
        private System.Windows.Forms.Label m_lbl_Gain;
        private System.Windows.Forms.ComboBox 图像采集类型comboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox 激光线阈值2textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox 测量点数textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox 激光线阈值1textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox 触发方向comboBox;
        private System.Windows.Forms.TextBox 触发延时textBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox 触发分频textBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox 外部触发源comboBox;
        private System.Windows.Forms.TextBox 内部触发频率textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 触发模式comBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel 监控panel;
    }
}