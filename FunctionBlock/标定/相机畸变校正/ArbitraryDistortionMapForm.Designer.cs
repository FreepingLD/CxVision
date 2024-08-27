namespace FunctionBlock
{
    partial class ArbitraryDistortionMapForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.采集源textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.内参标定配置tabPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.保存映射Button = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label55 = new System.Windows.Forms.Label();
            this.圆心距textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.保存button = new System.Windows.Forms.Button();
            this.加载图像button = new System.Windows.Forms.Button();
            this.采集图像button = new System.Windows.Forms.Button();
            this.阈值textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.标定button = new System.Windows.Forms.Button();
            this.行数domainUpDown = new System.Windows.Forms.DomainUpDown();
            this.标定区域button = new System.Windows.Forms.Button();
            this.列数domainUpDown = new System.Windows.Forms.DomainUpDown();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.视图工具toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Translate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Auto = new System.Windows.Forms.ToolStripButton();
            this.statusStrip3 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值1Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值2Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值3Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.行坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.列坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.panel1.SuspendLayout();
            this.内参标定配置tabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.采集源textBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(328, 31);
            this.panel1.TabIndex = 27;
            // 
            // 采集源textBox
            // 
            this.采集源textBox.Location = new System.Drawing.Point(75, 5);
            this.采集源textBox.Name = "采集源textBox";
            this.采集源textBox.ReadOnly = true;
            this.采集源textBox.Size = new System.Drawing.Size(250, 21);
            this.采集源textBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "相机传感器：";
            // 
            // 内参标定配置tabPage
            // 
            this.内参标定配置tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.内参标定配置tabPage.Controls.Add(this.splitContainer1);
            this.内参标定配置tabPage.Location = new System.Drawing.Point(4, 22);
            this.内参标定配置tabPage.Margin = new System.Windows.Forms.Padding(0);
            this.内参标定配置tabPage.Name = "内参标定配置tabPage";
            this.内参标定配置tabPage.Size = new System.Drawing.Size(314, 637);
            this.内参标定配置tabPage.TabIndex = 0;
            this.内参标定配置tabPage.Text = "畸变校正参数";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.保存映射Button);
            this.splitContainer1.Panel1.Controls.Add(this.SaveButton);
            this.splitContainer1.Panel1.Controls.Add(this.label55);
            this.splitContainer1.Panel1.Controls.Add(this.圆心距textBox);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.保存button);
            this.splitContainer1.Panel1.Controls.Add(this.加载图像button);
            this.splitContainer1.Panel1.Controls.Add(this.采集图像button);
            this.splitContainer1.Panel1.Controls.Add(this.阈值textBox);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label30);
            this.splitContainer1.Panel1.Controls.Add(this.label41);
            this.splitContainer1.Panel1.Controls.Add(this.标定button);
            this.splitContainer1.Panel1.Controls.Add(this.行数domainUpDown);
            this.splitContainer1.Panel1.Controls.Add(this.标定区域button);
            this.splitContainer1.Panel1.Controls.Add(this.列数domainUpDown);
            this.splitContainer1.Size = new System.Drawing.Size(314, 637);
            this.splitContainer1.SplitterDistance = 590;
            this.splitContainer1.TabIndex = 0;
            // 
            // 保存映射Button
            // 
            this.保存映射Button.Location = new System.Drawing.Point(223, 273);
            this.保存映射Button.Name = "保存映射Button";
            this.保存映射Button.Size = new System.Drawing.Size(86, 38);
            this.保存映射Button.TabIndex = 37;
            this.保存映射Button.Text = "保存映射图像";
            this.保存映射Button.UseVisualStyleBackColor = true;
            this.保存映射Button.Click += new System.EventHandler(this.保存映射Button_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(223, 101);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(86, 33);
            this.SaveButton.TabIndex = 36;
            this.SaveButton.Text = "保存图像";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label55.Location = new System.Drawing.Point(175, 77);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(21, 14);
            this.label55.TabIndex = 35;
            this.label55.Text = "mm";
            // 
            // 圆心距textBox
            // 
            this.圆心距textBox.Location = new System.Drawing.Point(61, 73);
            this.圆心距textBox.Name = "圆心距textBox";
            this.圆心距textBox.Size = new System.Drawing.Size(110, 21);
            this.圆心距textBox.TabIndex = 34;
            this.圆心距textBox.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "圆心距：";
            // 
            // 保存button
            // 
            this.保存button.Location = new System.Drawing.Point(223, 229);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(86, 38);
            this.保存button.TabIndex = 32;
            this.保存button.Text = "保存参数";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // 加载图像button
            // 
            this.加载图像button.Location = new System.Drawing.Point(223, 59);
            this.加载图像button.Name = "加载图像button";
            this.加载图像button.Size = new System.Drawing.Size(86, 33);
            this.加载图像button.TabIndex = 31;
            this.加载图像button.Text = "加载图像";
            this.加载图像button.UseVisualStyleBackColor = true;
            this.加载图像button.Click += new System.EventHandler(this.加载图像button_Click);
            // 
            // 采集图像button
            // 
            this.采集图像button.Location = new System.Drawing.Point(223, 17);
            this.采集图像button.Name = "采集图像button";
            this.采集图像button.Size = new System.Drawing.Size(86, 36);
            this.采集图像button.TabIndex = 30;
            this.采集图像button.Text = "采集图像";
            this.采集图像button.UseVisualStyleBackColor = true;
            this.采集图像button.Click += new System.EventHandler(this.采集图像button_Click);
            // 
            // 阈值textBox
            // 
            this.阈值textBox.Location = new System.Drawing.Point(61, 103);
            this.阈值textBox.Name = "阈值textBox";
            this.阈值textBox.Size = new System.Drawing.Size(110, 21);
            this.阈值textBox.TabIndex = 26;
            this.阈值textBox.Text = "128";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "阈值：";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(18, 48);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(41, 12);
            this.label30.TabIndex = 22;
            this.label30.Text = "列数：";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(18, 20);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(41, 12);
            this.label41.TabIndex = 21;
            this.label41.Text = "行数：";
            // 
            // 标定button
            // 
            this.标定button.Location = new System.Drawing.Point(223, 184);
            this.标定button.Name = "标定button";
            this.标定button.Size = new System.Drawing.Size(86, 37);
            this.标定button.TabIndex = 16;
            this.标定button.Text = "标定";
            this.标定button.UseVisualStyleBackColor = true;
            this.标定button.Click += new System.EventHandler(this.标定button_Click_1);
            // 
            // 行数domainUpDown
            // 
            this.行数domainUpDown.Location = new System.Drawing.Point(61, 17);
            this.行数domainUpDown.Name = "行数domainUpDown";
            this.行数domainUpDown.Size = new System.Drawing.Size(110, 21);
            this.行数domainUpDown.TabIndex = 23;
            this.行数domainUpDown.Text = "4";
            // 
            // 标定区域button
            // 
            this.标定区域button.Location = new System.Drawing.Point(223, 141);
            this.标定区域button.Name = "标定区域button";
            this.标定区域button.Size = new System.Drawing.Size(86, 35);
            this.标定区域button.TabIndex = 20;
            this.标定区域button.Text = "设置标定区域";
            this.标定区域button.UseVisualStyleBackColor = true;
            this.标定区域button.Click += new System.EventHandler(this.标定区域button_Click);
            // 
            // 列数domainUpDown
            // 
            this.列数domainUpDown.Location = new System.Drawing.Point(61, 44);
            this.列数domainUpDown.Name = "列数domainUpDown";
            this.列数domainUpDown.Size = new System.Drawing.Size(110, 21);
            this.列数domainUpDown.TabIndex = 24;
            this.列数domainUpDown.Text = "4";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.内参标定配置tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 34);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(322, 663);
            this.tabControl1.TabIndex = 29;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 328F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 330F));
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 635F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1095, 700);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // 视图工具toolStrip
            // 
            this.视图工具toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.视图工具toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.视图工具toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Clear,
            this.toolStripButton_Select,
            this.toolStripButton_Translate,
            this.toolStripButton_Auto});
            this.视图工具toolStrip.Location = new System.Drawing.Point(328, 0);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(437, 31);
            this.视图工具toolStrip.TabIndex = 33;
            this.视图工具toolStrip.Text = "toolStrip2";
            this.视图工具toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.Image = global::FunctionBlock.Properties.Resources.清除;
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(56, 28);
            this.toolStripButton_Clear.Text = "清除";
            // 
            // toolStripButton_Select
            // 
            this.toolStripButton_Select.Image = global::FunctionBlock.Properties.Resources.选择光标;
            this.toolStripButton_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Select.Name = "toolStripButton_Select";
            this.toolStripButton_Select.Size = new System.Drawing.Size(56, 28);
            this.toolStripButton_Select.Text = "选择";
            // 
            // toolStripButton_Translate
            // 
            this.toolStripButton_Translate.Image = global::FunctionBlock.Properties.Resources.移动光标;
            this.toolStripButton_Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Translate.Name = "toolStripButton_Translate";
            this.toolStripButton_Translate.Size = new System.Drawing.Size(56, 28);
            this.toolStripButton_Translate.Text = "平移";
            // 
            // toolStripButton_Auto
            // 
            this.toolStripButton_Auto.Image = global::FunctionBlock.Properties.Resources.适配窗口光标;
            this.toolStripButton_Auto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Auto.Name = "toolStripButton_Auto";
            this.toolStripButton_Auto.Size = new System.Drawing.Size(80, 28);
            this.toolStripButton_Auto.Text = "适应窗口";
            // 
            // statusStrip3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip3, 2);
            this.statusStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel4,
            this.灰度值1Label,
            this.灰度值2Label,
            this.灰度值3Label,
            this.toolStripStatusLabel9,
            this.行坐标Label,
            this.列坐标Label});
            this.statusStrip3.Location = new System.Drawing.Point(328, 675);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(767, 25);
            this.statusStrip3.TabIndex = 32;
            this.statusStrip3.Text = "statusStrip3";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(56, 20);
            this.toolStripStatusLabel4.Text = "灰度值：";
            // 
            // 灰度值1Label
            // 
            this.灰度值1Label.Name = "灰度值1Label";
            this.灰度值1Label.Size = new System.Drawing.Size(28, 20);
            this.灰度值1Label.Text = "……";
            // 
            // 灰度值2Label
            // 
            this.灰度值2Label.Name = "灰度值2Label";
            this.灰度值2Label.Size = new System.Drawing.Size(28, 20);
            this.灰度值2Label.Text = "……";
            // 
            // 灰度值3Label
            // 
            this.灰度值3Label.Name = "灰度值3Label";
            this.灰度值3Label.Size = new System.Drawing.Size(28, 20);
            this.灰度值3Label.Text = "……";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(44, 20);
            this.toolStripStatusLabel9.Text = "坐标：";
            // 
            // 行坐标Label
            // 
            this.行坐标Label.Name = "行坐标Label";
            this.行坐标Label.Size = new System.Drawing.Size(28, 20);
            this.行坐标Label.Text = "……";
            // 
            // 列坐标Label
            // 
            this.列坐标Label.Name = "列坐标Label";
            this.列坐标Label.Size = new System.Drawing.Size(28, 20);
            this.列坐标Label.Text = "……";
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "源图像",
            "映射图像",
            "校正图像"});
            this.comboBox1.Location = new System.Drawing.Point(768, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(324, 20);
            this.comboBox1.TabIndex = 31;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(331, 34);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Size = new System.Drawing.Size(761, 638);
            this.hWindowControl1.TabIndex = 30;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(761, 638);
            // 
            // ArbitraryDistortionMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 700);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ArbitraryDistortionMapForm";
            this.Text = "相机畸变校正";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DistortionMapParamForm_FormClosing);
            this.Load += new System.EventHandler(this.DistortionMapParamForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.内参标定配置tabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 内参标定配置tabPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox 阈值textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DomainUpDown 列数domainUpDown;
        private System.Windows.Forms.DomainUpDown 行数domainUpDown;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Button 标定区域button;
        private System.Windows.Forms.Button 标定button;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Button 加载图像button;
        private System.Windows.Forms.Button 采集图像button;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.ToolStrip 视图工具toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_Clear;
        private System.Windows.Forms.ToolStripButton toolStripButton_Select;
        private System.Windows.Forms.ToolStripButton toolStripButton_Translate;
        private System.Windows.Forms.ToolStripButton toolStripButton_Auto;
        private System.Windows.Forms.TextBox 采集源textBox;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox 圆心距textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button 保存映射Button;
    }
}