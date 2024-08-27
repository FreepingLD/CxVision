
namespace FunctionBlock
{
    partial class CameraParamForm
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
            this.增益numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.曝光textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.传感器参数button = new System.Windows.Forms.Button();
            this.启用相机畸变校正checkBox = new System.Windows.Forms.CheckBox();
            this.触发模式comboBox = new System.Windows.Forms.ComboBox();
            this.触发源comboBox = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.采集延时textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.IO输出类型comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.触发间隔textBox = new System.Windows.Forms.TextBox();
            this.触发端口numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.停顿时间textBox = new System.Windows.Forms.TextBox();
            this.旋转checkBox = new System.Windows.Forms.CheckBox();
            this.Y轴镜像checkBox = new System.Windows.Forms.CheckBox();
            this.X轴镜像checkBox = new System.Windows.Forms.CheckBox();
            this.CamSlanttextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.像素当量textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DataHeighttextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DataWidthtextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SensorNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.传感器测试button = new System.Windows.Forms.Button();
            this.启用图像缩放checkBox = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.图像宽缩放textBox = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.图像高缩放textBox = new System.Windows.Forms.TextBox();
            this.视图窗口comboBox = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.采集数量textBox = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.readFileButton = new System.Windows.Forms.Button();
            this.配置文件textBox = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.采集超时textBox = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.gainBtn = new System.Windows.Forms.Button();
            this.exposeBtn = new System.Windows.Forms.Button();
            this.平均值textBox = new System.Windows.Forms.TextBox();
            this.采集模式comboBox = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.增益numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.触发端口numericUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 增益numericUpDown
            // 
            this.增益numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.增益numericUpDown.Location = new System.Drawing.Point(84, 683);
            this.增益numericUpDown.Name = "增益numericUpDown";
            this.增益numericUpDown.Size = new System.Drawing.Size(164, 21);
            this.增益numericUpDown.TabIndex = 109;
            this.增益numericUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.增益numericUpDown_KeyUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(47, 685);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 108;
            this.label10.Text = "增益:";
            // 
            // 曝光textBox
            // 
            this.曝光textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.曝光textBox.Location = new System.Drawing.Point(84, 655);
            this.曝光textBox.Name = "曝光textBox";
            this.曝光textBox.Size = new System.Drawing.Size(163, 21);
            this.曝光textBox.TabIndex = 107;
            this.曝光textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.曝光textBox_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(46, 658);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 106;
            this.label5.Text = "曝光:";
            // 
            // 传感器参数button
            // 
            this.传感器参数button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.传感器参数button.Location = new System.Drawing.Point(188, 710);
            this.传感器参数button.Name = "传感器参数button";
            this.传感器参数button.Size = new System.Drawing.Size(98, 33);
            this.传感器参数button.TabIndex = 105;
            this.传感器参数button.Text = "标定参数";
            this.传感器参数button.UseVisualStyleBackColor = true;
            this.传感器参数button.Click += new System.EventHandler(this.传感器参数button_Click);
            // 
            // 启用相机畸变校正checkBox
            // 
            this.启用相机畸变校正checkBox.AutoSize = true;
            this.启用相机畸变校正checkBox.Location = new System.Drawing.Point(84, 512);
            this.启用相机畸变校正checkBox.Name = "启用相机畸变校正checkBox";
            this.启用相机畸变校正checkBox.Size = new System.Drawing.Size(120, 16);
            this.启用相机畸变校正checkBox.TabIndex = 104;
            this.启用相机畸变校正checkBox.Text = "启用相机畸变校正";
            this.启用相机畸变校正checkBox.UseVisualStyleBackColor = true;
            // 
            // 触发模式comboBox
            // 
            this.触发模式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.触发模式comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.触发模式comboBox.FormattingEnabled = true;
            this.触发模式comboBox.Location = new System.Drawing.Point(84, 111);
            this.触发模式comboBox.Name = "触发模式comboBox";
            this.触发模式comboBox.Size = new System.Drawing.Size(163, 20);
            this.触发模式comboBox.TabIndex = 103;
            // 
            // 触发源comboBox
            // 
            this.触发源comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.触发源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.触发源comboBox.FormattingEnabled = true;
            this.触发源comboBox.Location = new System.Drawing.Point(84, 85);
            this.触发源comboBox.Name = "触发源comboBox";
            this.触发源comboBox.Size = new System.Drawing.Size(163, 20);
            this.触发源comboBox.TabIndex = 102;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(23, 115);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 101;
            this.label17.Text = "触发模式:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(35, 88);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(47, 12);
            this.label18.TabIndex = 100;
            this.label18.Text = "触发源:";
            // 
            // 采集延时textBox
            // 
            this.采集延时textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.采集延时textBox.Location = new System.Drawing.Point(84, 243);
            this.采集延时textBox.Name = "采集延时textBox";
            this.采集延时textBox.Size = new System.Drawing.Size(163, 21);
            this.采集延时textBox.TabIndex = 83;
            this.采集延时textBox.Text = "100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 246);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 82;
            this.label7.Text = "采集延时:";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(251, 195);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 12);
            this.label12.TabIndex = 84;
            this.label12.Text = "mm";
            // 
            // IO输出类型comboBox
            // 
            this.IO输出类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IO输出类型comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IO输出类型comboBox.FormattingEnabled = true;
            this.IO输出类型comboBox.Location = new System.Drawing.Point(84, 138);
            this.IO输出类型comboBox.Name = "IO输出类型comboBox";
            this.IO输出类型comboBox.Size = new System.Drawing.Size(163, 20);
            this.IO输出类型comboBox.TabIndex = 80;
            this.IO输出类型comboBox.SelectedIndexChanged += new System.EventHandler(this.IO输出类型comboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 81;
            this.label6.Text = "IO输出类型:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 193);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 12);
            this.label13.TabIndex = 77;
            this.label13.Text = "触发间隔:";
            // 
            // 触发间隔textBox
            // 
            this.触发间隔textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.触发间隔textBox.Location = new System.Drawing.Point(84, 189);
            this.触发间隔textBox.Name = "触发间隔textBox";
            this.触发间隔textBox.Size = new System.Drawing.Size(163, 21);
            this.触发间隔textBox.TabIndex = 78;
            this.触发间隔textBox.Text = "0.02";
            // 
            // 触发端口numericUpDown
            // 
            this.触发端口numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.触发端口numericUpDown.Location = new System.Drawing.Point(84, 164);
            this.触发端口numericUpDown.Name = "触发端口numericUpDown";
            this.触发端口numericUpDown.Size = new System.Drawing.Size(164, 21);
            this.触发端口numericUpDown.TabIndex = 73;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(35, 168);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 72;
            this.label14.Text = "IO端口:";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(251, 221);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 76;
            this.label15.Text = "ms";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(23, 220);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 74;
            this.label16.Text = "停顿时间:";
            // 
            // 停顿时间textBox
            // 
            this.停顿时间textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.停顿时间textBox.Location = new System.Drawing.Point(84, 216);
            this.停顿时间textBox.Name = "停顿时间textBox";
            this.停顿时间textBox.Size = new System.Drawing.Size(163, 21);
            this.停顿时间textBox.TabIndex = 75;
            this.停顿时间textBox.Text = "50";
            // 
            // 旋转checkBox
            // 
            this.旋转checkBox.AutoSize = true;
            this.旋转checkBox.Location = new System.Drawing.Point(84, 576);
            this.旋转checkBox.Name = "旋转checkBox";
            this.旋转checkBox.Size = new System.Drawing.Size(48, 16);
            this.旋转checkBox.TabIndex = 26;
            this.旋转checkBox.Text = "旋转";
            this.旋转checkBox.UseVisualStyleBackColor = true;
            // 
            // Y轴镜像checkBox
            // 
            this.Y轴镜像checkBox.AutoSize = true;
            this.Y轴镜像checkBox.Location = new System.Drawing.Point(84, 556);
            this.Y轴镜像checkBox.Name = "Y轴镜像checkBox";
            this.Y轴镜像checkBox.Size = new System.Drawing.Size(66, 16);
            this.Y轴镜像checkBox.TabIndex = 25;
            this.Y轴镜像checkBox.Text = "Y轴镜像";
            this.Y轴镜像checkBox.UseVisualStyleBackColor = true;
            // 
            // X轴镜像checkBox
            // 
            this.X轴镜像checkBox.AutoSize = true;
            this.X轴镜像checkBox.Location = new System.Drawing.Point(84, 535);
            this.X轴镜像checkBox.Name = "X轴镜像checkBox";
            this.X轴镜像checkBox.Size = new System.Drawing.Size(66, 16);
            this.X轴镜像checkBox.TabIndex = 24;
            this.X轴镜像checkBox.Text = "X轴镜像";
            this.X轴镜像checkBox.UseVisualStyleBackColor = true;
            // 
            // CamSlanttextBox
            // 
            this.CamSlanttextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CamSlanttextBox.Location = new System.Drawing.Point(84, 432);
            this.CamSlanttextBox.Name = "CamSlanttextBox";
            this.CamSlanttextBox.Size = new System.Drawing.Size(163, 21);
            this.CamSlanttextBox.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 437);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "相机倾斜:";
            // 
            // 像素当量textBox
            // 
            this.像素当量textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.像素当量textBox.Location = new System.Drawing.Point(84, 405);
            this.像素当量textBox.Name = "像素当量textBox";
            this.像素当量textBox.Size = new System.Drawing.Size(163, 21);
            this.像素当量textBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 409);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "像素当量:";
            // 
            // DataHeighttextBox
            // 
            this.DataHeighttextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataHeighttextBox.Location = new System.Drawing.Point(84, 325);
            this.DataHeighttextBox.Name = "DataHeighttextBox";
            this.DataHeighttextBox.Size = new System.Drawing.Size(163, 21);
            this.DataHeighttextBox.TabIndex = 5;
            this.DataHeighttextBox.Text = "1000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "图像高:";
            // 
            // DataWidthtextBox
            // 
            this.DataWidthtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataWidthtextBox.Location = new System.Drawing.Point(84, 298);
            this.DataWidthtextBox.Name = "DataWidthtextBox";
            this.DataWidthtextBox.Size = new System.Drawing.Size(163, 21);
            this.DataWidthtextBox.TabIndex = 3;
            this.DataWidthtextBox.Text = "2048";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 302);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "图像宽:";
            // 
            // SensorNameTextBox
            // 
            this.SensorNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SensorNameTextBox.Location = new System.Drawing.Point(84, 628);
            this.SensorNameTextBox.Name = "SensorNameTextBox";
            this.SensorNameTextBox.Size = new System.Drawing.Size(163, 21);
            this.SensorNameTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 630);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "相机名称:";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.传感器测试button);
            this.panel1.Controls.Add(this.启用图像缩放checkBox);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.图像宽缩放textBox);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.图像高缩放textBox);
            this.panel1.Controls.Add(this.视图窗口comboBox);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.采集数量textBox);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.readFileButton);
            this.panel1.Controls.Add(this.配置文件textBox);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.采集超时textBox);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.gainBtn);
            this.panel1.Controls.Add(this.exposeBtn);
            this.panel1.Controls.Add(this.平均值textBox);
            this.panel1.Controls.Add(this.采集模式comboBox);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.增益numericUpDown);
            this.panel1.Controls.Add(this.触发源comboBox);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.曝光textBox);
            this.panel1.Controls.Add(this.SensorNameTextBox);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.传感器参数button);
            this.panel1.Controls.Add(this.DataWidthtextBox);
            this.panel1.Controls.Add(this.启用相机畸变校正checkBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.触发模式comboBox);
            this.panel1.Controls.Add(this.DataHeighttextBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.像素当量textBox);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.采集延时textBox);
            this.panel1.Controls.Add(this.CamSlanttextBox);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.X轴镜像checkBox);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.Y轴镜像checkBox);
            this.panel1.Controls.Add(this.IO输出类型comboBox);
            this.panel1.Controls.Add(this.旋转checkBox);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.停顿时间textBox);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.触发间隔textBox);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.触发端口numericUpDown);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(291, 751);
            this.panel1.TabIndex = 1;
            // 
            // 传感器测试button
            // 
            this.传感器测试button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.传感器测试button.Location = new System.Drawing.Point(82, 710);
            this.传感器测试button.Name = "传感器测试button";
            this.传感器测试button.Size = new System.Drawing.Size(98, 33);
            this.传感器测试button.TabIndex = 134;
            this.传感器测试button.Text = "传感器测试";
            this.传感器测试button.UseVisualStyleBackColor = true;
            // 
            // 启用图像缩放checkBox
            // 
            this.启用图像缩放checkBox.AutoSize = true;
            this.启用图像缩放checkBox.Location = new System.Drawing.Point(84, 489);
            this.启用图像缩放checkBox.Name = "启用图像缩放checkBox";
            this.启用图像缩放checkBox.Size = new System.Drawing.Size(96, 16);
            this.启用图像缩放checkBox.TabIndex = 133;
            this.启用图像缩放checkBox.Text = "启用图像缩放";
            this.启用图像缩放checkBox.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(11, 356);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(71, 12);
            this.label25.TabIndex = 129;
            this.label25.Text = "图像宽缩放:";
            // 
            // 图像宽缩放textBox
            // 
            this.图像宽缩放textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.图像宽缩放textBox.Location = new System.Drawing.Point(85, 352);
            this.图像宽缩放textBox.Name = "图像宽缩放textBox";
            this.图像宽缩放textBox.Size = new System.Drawing.Size(163, 21);
            this.图像宽缩放textBox.TabIndex = 130;
            this.图像宽缩放textBox.Text = "1";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(11, 384);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(71, 12);
            this.label26.TabIndex = 131;
            this.label26.Text = "图像高缩放:";
            // 
            // 图像高缩放textBox
            // 
            this.图像高缩放textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.图像高缩放textBox.Location = new System.Drawing.Point(85, 379);
            this.图像高缩放textBox.Name = "图像高缩放textBox";
            this.图像高缩放textBox.Size = new System.Drawing.Size(163, 21);
            this.图像高缩放textBox.TabIndex = 132;
            this.图像高缩放textBox.Text = "1";
            // 
            // 视图窗口comboBox
            // 
            this.视图窗口comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.视图窗口comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.视图窗口comboBox.FormattingEnabled = true;
            this.视图窗口comboBox.Location = new System.Drawing.Point(84, 459);
            this.视图窗口comboBox.Name = "视图窗口comboBox";
            this.视图窗口comboBox.Size = new System.Drawing.Size(163, 20);
            this.视图窗口comboBox.TabIndex = 128;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(23, 462);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(59, 12);
            this.label24.TabIndex = 127;
            this.label24.Text = "视图窗口:";
            // 
            // 采集数量textBox
            // 
            this.采集数量textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.采集数量textBox.Location = new System.Drawing.Point(84, 33);
            this.采集数量textBox.Name = "采集数量textBox";
            this.采集数量textBox.Size = new System.Drawing.Size(163, 21);
            this.采集数量textBox.TabIndex = 126;
            this.采集数量textBox.Text = "1";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(22, 604);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(59, 12);
            this.label23.TabIndex = 125;
            this.label23.Text = "配置文件:";
            // 
            // readFileButton
            // 
            this.readFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.readFileButton.Location = new System.Drawing.Point(253, 601);
            this.readFileButton.Name = "readFileButton";
            this.readFileButton.Size = new System.Drawing.Size(33, 21);
            this.readFileButton.TabIndex = 124;
            this.readFileButton.Text = "……";
            this.readFileButton.UseVisualStyleBackColor = true;
            this.readFileButton.Click += new System.EventHandler(this.readFileButton_Click);
            // 
            // 配置文件textBox
            // 
            this.配置文件textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.配置文件textBox.Location = new System.Drawing.Point(84, 601);
            this.配置文件textBox.Name = "配置文件textBox";
            this.配置文件textBox.Size = new System.Drawing.Size(163, 21);
            this.配置文件textBox.TabIndex = 123;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(1, 38);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(83, 12);
            this.label22.TabIndex = 120;
            this.label22.Text = "采集图像数量:";
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(251, 275);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(17, 12);
            this.label21.TabIndex = 119;
            this.label21.Text = "ms";
            // 
            // 采集超时textBox
            // 
            this.采集超时textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.采集超时textBox.Location = new System.Drawing.Point(84, 270);
            this.采集超时textBox.Name = "采集超时textBox";
            this.采集超时textBox.Size = new System.Drawing.Size(163, 21);
            this.采集超时textBox.TabIndex = 118;
            this.采集超时textBox.Text = "2000";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(23, 273);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(59, 12);
            this.label20.TabIndex = 117;
            this.label20.Text = "采集超时:";
            // 
            // gainBtn
            // 
            this.gainBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gainBtn.Location = new System.Drawing.Point(253, 684);
            this.gainBtn.Margin = new System.Windows.Forms.Padding(2);
            this.gainBtn.Name = "gainBtn";
            this.gainBtn.Size = new System.Drawing.Size(33, 21);
            this.gainBtn.TabIndex = 116;
            this.gainBtn.Text = "set";
            this.gainBtn.UseVisualStyleBackColor = true;
            this.gainBtn.Click += new System.EventHandler(this.gainBtn_Click);
            // 
            // exposeBtn
            // 
            this.exposeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exposeBtn.Location = new System.Drawing.Point(252, 657);
            this.exposeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.exposeBtn.Name = "exposeBtn";
            this.exposeBtn.Size = new System.Drawing.Size(35, 21);
            this.exposeBtn.TabIndex = 115;
            this.exposeBtn.Text = "set";
            this.exposeBtn.UseVisualStyleBackColor = true;
            this.exposeBtn.Click += new System.EventHandler(this.exposeBtn_Click);
            // 
            // 平均值textBox
            // 
            this.平均值textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.平均值textBox.Location = new System.Drawing.Point(84, 58);
            this.平均值textBox.Name = "平均值textBox";
            this.平均值textBox.Size = new System.Drawing.Size(163, 21);
            this.平均值textBox.TabIndex = 114;
            this.平均值textBox.Text = "1";
            // 
            // 采集模式comboBox
            // 
            this.采集模式comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.采集模式comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.采集模式comboBox.FormattingEnabled = true;
            this.采集模式comboBox.Items.AddRange(new object[] {
            "同步采集",
            "异步采集",
            "异步取图"});
            this.采集模式comboBox.Location = new System.Drawing.Point(84, 10);
            this.采集模式comboBox.Name = "采集模式comboBox";
            this.采集模式comboBox.Size = new System.Drawing.Size(163, 20);
            this.采集模式comboBox.TabIndex = 113;
            this.采集模式comboBox.SelectedIndexChanged += new System.EventHandler(this.采集模式comboBox_SelectedIndexChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(24, 13);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 12);
            this.label19.TabIndex = 112;
            this.label19.Text = "采集模式:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 12);
            this.label11.TabIndex = 110;
            this.label11.Text = "图像平均值:";
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(251, 248);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(17, 12);
            this.label27.TabIndex = 135;
            this.label27.Text = "ms";
            // 
            // CameraParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 751);
            this.Controls.Add(this.panel1);
            this.Name = "CameraParamForm";
            this.Text = "相机参数管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraParamForm_FormClosing);
            this.Load += new System.EventHandler(this.CameraParamMangerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.增益numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.触发端口numericUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox CamSlanttextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox 像素当量textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DataHeighttextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DataWidthtextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SensorNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox 旋转checkBox;
        private System.Windows.Forms.CheckBox Y轴镜像checkBox;
        private System.Windows.Forms.CheckBox X轴镜像checkBox;
        private System.Windows.Forms.TextBox 采集延时textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox IO输出类型comboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox 触发间隔textBox;
        private System.Windows.Forms.NumericUpDown 触发端口numericUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox 停顿时间textBox;
        private System.Windows.Forms.ComboBox 触发模式comboBox;
        private System.Windows.Forms.ComboBox 触发源comboBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox 启用相机畸变校正checkBox;
        private System.Windows.Forms.Button 传感器参数button;
        private System.Windows.Forms.NumericUpDown 增益numericUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox 曝光textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox 平均值textBox;
        private System.Windows.Forms.ComboBox 采集模式comboBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button gainBtn;
        private System.Windows.Forms.Button exposeBtn;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox 采集超时textBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button readFileButton;
        private System.Windows.Forms.TextBox 配置文件textBox;
        private System.Windows.Forms.TextBox 采集数量textBox;
        private System.Windows.Forms.ComboBox 视图窗口comboBox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox 图像宽缩放textBox;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox 图像高缩放textBox;
        private System.Windows.Forms.CheckBox 启用图像缩放checkBox;
        private System.Windows.Forms.Button 传感器测试button;
        private System.Windows.Forms.Label label27;
    }
}