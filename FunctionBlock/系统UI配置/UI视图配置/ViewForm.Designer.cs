namespace FunctionBlock
{
    partial class ViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewForm));
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.titleLabel = new System.Windows.Forms.Label();
            this.传感器comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.实时采集checkBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SignLabel = new System.Windows.Forms.Label();
            this.程序节点comboBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.管控checkBox = new System.Windows.Forms.CheckBox();
            this.补偿checkBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.标定工具栏toolStrip = new System.Windows.Forms.ToolStrip();
            this.执行toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.编辑toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.相机toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.设置曝光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置增益ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.日志面板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标定toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.九点标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.旋转标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.九点旋转标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.世界坐标映射标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标定板标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.像素坐标映射标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标定板映射标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.手动九点标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机针头标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Roi绘制toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.绘制矩形toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制圆形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制椭圆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制直线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Socket连接状态 = new System.Windows.Forms.ToolStripLabel();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.标定工具栏toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.AllowDrop = true;
            this.hWindowControl1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.hWindowControl1.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(405, 295);
            this.hWindowControl1.TabIndex = 2;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(405, 295);
            this.hWindowControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.hWindowControl1_DragEnter);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 5);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(328, 20);
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
            this.传感器comboBox1.Location = new System.Drawing.Point(56, 20);
            this.传感器comboBox1.Margin = new System.Windows.Forms.Padding(0);
            this.传感器comboBox1.Name = "传感器comboBox1";
            this.传感器comboBox1.Size = new System.Drawing.Size(165, 20);
            this.传感器comboBox1.TabIndex = 3;
            this.传感器comboBox1.SelectionChangeCommitted += new System.EventHandler(this.传感器comboBox_SelectionChangeCommitted);
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(378, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(29, 20);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(353, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(25, 20);
            this.buttonMax.TabIndex = 1;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Location = new System.Drawing.Point(328, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(25, 20);
            this.buttonMin.TabIndex = 2;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
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
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.45901F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.54099F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.SignLabel, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.程序节点comboBox, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonMax, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonClose, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMin, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.传感器comboBox1, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.实时采集checkBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.标定工具栏toolStrip, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(407, 362);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // SignLabel
            // 
            this.SignLabel.AutoSize = true;
            this.SignLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SignLabel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SignLabel.Location = new System.Drawing.Point(224, 20);
            this.SignLabel.Name = "SignLabel";
            this.SignLabel.Size = new System.Drawing.Size(50, 20);
            this.SignLabel.TabIndex = 3;
            this.SignLabel.Text = "label";
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
            this.程序节点comboBox.Location = new System.Drawing.Point(277, 20);
            this.程序节点comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.程序节点comboBox.Name = "程序节点comboBox";
            this.程序节点comboBox.Size = new System.Drawing.Size(130, 20);
            this.程序节点comboBox.TabIndex = 24;
            this.程序节点comboBox.SelectionChangeCommitted += new System.EventHandler(this.程序节点comboBox_SelectionChangeCommitted);
            this.程序节点comboBox.Click += new System.EventHandler(this.程序节点comboBox_Click);
            // 
            // panel1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 8);
            this.panel1.Controls.Add(this.管控checkBox);
            this.panel1.Controls.Add(this.补偿checkBox);
            this.panel1.Controls.Add(this.hWindowControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 295);
            this.panel1.TabIndex = 25;
            // 
            // 管控checkBox
            // 
            this.管控checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.管控checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.管控checkBox.AutoSize = true;
            this.管控checkBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.管控checkBox.Location = new System.Drawing.Point(-1, 273);
            this.管控checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.管控checkBox.Name = "管控checkBox";
            this.管控checkBox.Size = new System.Drawing.Size(39, 22);
            this.管控checkBox.TabIndex = 6;
            this.管控checkBox.Text = "管控";
            this.管控checkBox.UseVisualStyleBackColor = true;
            this.管控checkBox.Click += new System.EventHandler(this.管控checkBox_Click);
            // 
            // 补偿checkBox
            // 
            this.补偿checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.补偿checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.补偿checkBox.AutoSize = true;
            this.补偿checkBox.Enabled = false;
            this.补偿checkBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.补偿checkBox.Location = new System.Drawing.Point(367, 273);
            this.补偿checkBox.Name = "补偿checkBox";
            this.补偿checkBox.Size = new System.Drawing.Size(39, 22);
            this.补偿checkBox.TabIndex = 5;
            this.补偿checkBox.Text = "补偿";
            this.补偿checkBox.UseVisualStyleBackColor = true;
            this.补偿checkBox.Click += new System.EventHandler(this.补偿checkBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "实时";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 标定工具栏toolStrip
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.标定工具栏toolStrip, 8);
            this.标定工具栏toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.执行toolStripButton,
            this.编辑toolStripButton,
            this.相机toolStripDropDownButton,
            this.标定toolStripDropDownButton,
            this.Roi绘制toolStripDropDownButton,
            this.Socket连接状态});
            this.标定工具栏toolStrip.Location = new System.Drawing.Point(0, 337);
            this.标定工具栏toolStrip.Name = "标定工具栏toolStrip";
            this.标定工具栏toolStrip.Size = new System.Drawing.Size(407, 25);
            this.标定工具栏toolStrip.TabIndex = 27;
            this.标定工具栏toolStrip.Text = "toolStrip1";
            this.标定工具栏toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.标定工具栏toolStrip_ItemClicked);
            // 
            // 执行toolStripButton
            // 
            this.执行toolStripButton.Image = global::FunctionBlock.Properties.Resources.开始20_X_20;
            this.执行toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.执行toolStripButton.Name = "执行toolStripButton";
            this.执行toolStripButton.Size = new System.Drawing.Size(52, 22);
            this.执行toolStripButton.Text = "执行";
            this.执行toolStripButton.ToolTipText = "标定开始/停止";
            // 
            // 编辑toolStripButton
            // 
            this.编辑toolStripButton.Image = global::FunctionBlock.Properties.Resources.编辑20_x_20_jpg;
            this.编辑toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.编辑toolStripButton.Name = "编辑toolStripButton";
            this.编辑toolStripButton.Size = new System.Drawing.Size(52, 22);
            this.编辑toolStripButton.Text = "编辑";
            // 
            // 相机toolStripDropDownButton
            // 
            this.相机toolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置曝光toolStripMenuItem,
            this.设置增益ToolStripMenuItem,
            this.日志面板ToolStripMenuItem});
            this.相机toolStripDropDownButton.Image = global::FunctionBlock.Properties.Resources.设置20_X_20;
            this.相机toolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.相机toolStripDropDownButton.Name = "相机toolStripDropDownButton";
            this.相机toolStripDropDownButton.Size = new System.Drawing.Size(85, 22);
            this.相机toolStripDropDownButton.Text = "相机设置";
            this.相机toolStripDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.相机toolStripDropDownButton_DropDownItemClicked);
            // 
            // 设置曝光toolStripMenuItem
            // 
            this.设置曝光toolStripMenuItem.Name = "设置曝光toolStripMenuItem";
            this.设置曝光toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.设置曝光toolStripMenuItem.Text = "设置曝光";
            // 
            // 设置增益ToolStripMenuItem
            // 
            this.设置增益ToolStripMenuItem.Name = "设置增益ToolStripMenuItem";
            this.设置增益ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.设置增益ToolStripMenuItem.Text = "设置增益";
            // 
            // 日志面板ToolStripMenuItem
            // 
            this.日志面板ToolStripMenuItem.Name = "日志面板ToolStripMenuItem";
            this.日志面板ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.日志面板ToolStripMenuItem.Text = "日志面板";
            // 
            // 标定toolStripDropDownButton
            // 
            this.标定toolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.九点标定ToolStripMenuItem,
            this.旋转标定ToolStripMenuItem,
            this.九点旋转标定ToolStripMenuItem,
            this.世界坐标映射标定ToolStripMenuItem,
            this.标定板标定ToolStripMenuItem,
            this.像素坐标映射标定toolStripMenuItem,
            this.标定板映射标定toolStripMenuItem,
            this.手动九点标定ToolStripMenuItem,
            this.相机针头标定ToolStripMenuItem});
            this.标定toolStripDropDownButton.Image = global::FunctionBlock.Properties.Resources.标定_20_X_20;
            this.标定toolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.标定toolStripDropDownButton.Name = "标定toolStripDropDownButton";
            this.标定toolStripDropDownButton.Size = new System.Drawing.Size(61, 22);
            this.标定toolStripDropDownButton.Text = "标定";
            this.标定toolStripDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.标定toolStripDropDownButton_DropDownItemClicked);
            // 
            // 九点标定ToolStripMenuItem
            // 
            this.九点标定ToolStripMenuItem.Name = "九点标定ToolStripMenuItem";
            this.九点标定ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.九点标定ToolStripMenuItem.Text = "九点标定";
            // 
            // 旋转标定ToolStripMenuItem
            // 
            this.旋转标定ToolStripMenuItem.Name = "旋转标定ToolStripMenuItem";
            this.旋转标定ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.旋转标定ToolStripMenuItem.Text = "旋转标定";
            // 
            // 九点旋转标定ToolStripMenuItem
            // 
            this.九点旋转标定ToolStripMenuItem.Name = "九点旋转标定ToolStripMenuItem";
            this.九点旋转标定ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.九点旋转标定ToolStripMenuItem.Text = "九点&旋转标定";
            // 
            // 世界坐标映射标定ToolStripMenuItem
            // 
            this.世界坐标映射标定ToolStripMenuItem.Name = "世界坐标映射标定ToolStripMenuItem";
            this.世界坐标映射标定ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.世界坐标映射标定ToolStripMenuItem.Text = "世界坐标映射标定";
            // 
            // 标定板标定ToolStripMenuItem
            // 
            this.标定板标定ToolStripMenuItem.Name = "标定板标定ToolStripMenuItem";
            this.标定板标定ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.标定板标定ToolStripMenuItem.Text = "标定板N点标定";
            // 
            // 像素坐标映射标定toolStripMenuItem
            // 
            this.像素坐标映射标定toolStripMenuItem.Name = "像素坐标映射标定toolStripMenuItem";
            this.像素坐标映射标定toolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.像素坐标映射标定toolStripMenuItem.Text = "像素坐标映射标定";
            // 
            // 标定板映射标定toolStripMenuItem
            // 
            this.标定板映射标定toolStripMenuItem.Name = "标定板映射标定toolStripMenuItem";
            this.标定板映射标定toolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.标定板映射标定toolStripMenuItem.Text = "标定板映射标定";
            // 
            // 手动九点标定ToolStripMenuItem
            // 
            this.手动九点标定ToolStripMenuItem.Name = "手动九点标定ToolStripMenuItem";
            this.手动九点标定ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.手动九点标定ToolStripMenuItem.Text = "手动九点标定";
            // 
            // 相机针头标定ToolStripMenuItem
            // 
            this.相机针头标定ToolStripMenuItem.Name = "相机针头标定ToolStripMenuItem";
            this.相机针头标定ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.相机针头标定ToolStripMenuItem.Text = "相机&&针头标定";
            // 
            // Roi绘制toolStripDropDownButton
            // 
            this.Roi绘制toolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制矩形toolStripMenuItem,
            this.绘制圆形ToolStripMenuItem,
            this.绘制椭圆ToolStripMenuItem,
            this.绘制直线ToolStripMenuItem,
            this.绘制点ToolStripMenuItem});
            this.Roi绘制toolStripDropDownButton.Image = global::FunctionBlock.Properties.Resources.绘图;
            this.Roi绘制toolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Roi绘制toolStripDropDownButton.Name = "Roi绘制toolStripDropDownButton";
            this.Roi绘制toolStripDropDownButton.Size = new System.Drawing.Size(80, 22);
            this.Roi绘制toolStripDropDownButton.Text = "Roi绘制";
            this.Roi绘制toolStripDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Roi绘制toolStripDropDownButton_DropDownItemClicked);
            // 
            // 绘制矩形toolStripMenuItem
            // 
            this.绘制矩形toolStripMenuItem.Image = global::FunctionBlock.Properties.Resources.矩形_20_X_20;
            this.绘制矩形toolStripMenuItem.Name = "绘制矩形toolStripMenuItem";
            this.绘制矩形toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.绘制矩形toolStripMenuItem.Text = "绘制矩形";
            // 
            // 绘制圆形ToolStripMenuItem
            // 
            this.绘制圆形ToolStripMenuItem.Image = global::FunctionBlock.Properties.Resources.圆形_20_X_20;
            this.绘制圆形ToolStripMenuItem.Name = "绘制圆形ToolStripMenuItem";
            this.绘制圆形ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.绘制圆形ToolStripMenuItem.Text = "绘制圆形";
            // 
            // 绘制椭圆ToolStripMenuItem
            // 
            this.绘制椭圆ToolStripMenuItem.Name = "绘制椭圆ToolStripMenuItem";
            this.绘制椭圆ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.绘制椭圆ToolStripMenuItem.Text = "绘制椭圆";
            // 
            // 绘制直线ToolStripMenuItem
            // 
            this.绘制直线ToolStripMenuItem.Name = "绘制直线ToolStripMenuItem";
            this.绘制直线ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.绘制直线ToolStripMenuItem.Text = "绘制直线";
            // 
            // 绘制点ToolStripMenuItem
            // 
            this.绘制点ToolStripMenuItem.Name = "绘制点ToolStripMenuItem";
            this.绘制点ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.绘制点ToolStripMenuItem.Text = "绘制点";
            // 
            // Socket连接状态
            // 
            this.Socket连接状态.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Socket连接状态.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Socket连接状态.Image = global::FunctionBlock.Properties.Resources.green1;
            this.Socket连接状态.Name = "Socket连接状态";
            this.Socket连接状态.Size = new System.Drawing.Size(16, 22);
            this.Socket连接状态.Text = "触发信号状态";
            this.Socket连接状态.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(407, 362);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(100, 30);
            this.Name = "ViewForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewForm_FormClosing);
            this.Load += new System.EventHandler(this.ViewForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewForm_MouseDown);
            this.Move += new System.EventHandler(this.ViewForm_Move);
            this.Resize += new System.EventHandler(this.ViewForm_Resize);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.标定工具栏toolStrip.ResumeLayout(false);
            this.标定工具栏toolStrip.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip 标定工具栏toolStrip;
        private System.Windows.Forms.ToolStripButton 执行toolStripButton;
        private System.Windows.Forms.ToolStripButton 编辑toolStripButton;
        private System.Windows.Forms.CheckBox 管控checkBox;
        private System.Windows.Forms.CheckBox 补偿checkBox;
        private System.Windows.Forms.ToolStripDropDownButton Roi绘制toolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem 绘制矩形toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制圆形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton 相机toolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem 设置曝光toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置增益ToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel Socket连接状态;
        private System.Windows.Forms.ToolStripDropDownButton 标定toolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem 九点标定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 旋转标定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 九点旋转标定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 世界坐标映射标定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标定板标定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 像素坐标映射标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标定板映射标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制椭圆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制直线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 手动九点标定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 日志面板ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 相机针头标定ToolStripMenuItem;
    }
}