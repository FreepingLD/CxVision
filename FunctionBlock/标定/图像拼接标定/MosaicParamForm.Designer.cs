namespace FunctionBlock
{
    partial class MosaicParamForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MosaicParamForm));
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.内参标定配置tabPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.清空图像集Button = new System.Windows.Forms.Button();
            this.提取特征点Button = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.保存button = new System.Windows.Forms.Button();
            this.加载图像button = new System.Windows.Forms.Button();
            this.采集图像button = new System.Windows.Forms.Button();
            this.标定button = new System.Windows.Forms.Button();
            this.搜索区域button = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.采集源textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.miniToolStrip = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值1Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值2Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值3Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.行坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.列坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.MoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.MovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.NavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.NavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip3 = new System.Windows.Forms.StatusStrip();
            this.PathLabel = new System.Windows.Forms.Label();
            this.显示拼接图Button = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.内参标定配置tabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
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
            this.hWindowControl1.Size = new System.Drawing.Size(681, 606);
            this.hWindowControl1.TabIndex = 30;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(681, 606);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.内参标定配置tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 34);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 2);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(322, 636);
            this.tabControl1.TabIndex = 29;
            // 
            // 内参标定配置tabPage
            // 
            this.内参标定配置tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.内参标定配置tabPage.Controls.Add(this.splitContainer1);
            this.内参标定配置tabPage.Location = new System.Drawing.Point(4, 22);
            this.内参标定配置tabPage.Margin = new System.Windows.Forms.Padding(0);
            this.内参标定配置tabPage.Name = "内参标定配置tabPage";
            this.内参标定配置tabPage.Size = new System.Drawing.Size(314, 610);
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
            this.splitContainer1.Panel1.Controls.Add(this.显示拼接图Button);
            this.splitContainer1.Panel1.Controls.Add(this.清空图像集Button);
            this.splitContainer1.Panel1.Controls.Add(this.提取特征点Button);
            this.splitContainer1.Panel1.Controls.Add(this.SaveButton);
            this.splitContainer1.Panel1.Controls.Add(this.保存button);
            this.splitContainer1.Panel1.Controls.Add(this.加载图像button);
            this.splitContainer1.Panel1.Controls.Add(this.采集图像button);
            this.splitContainer1.Panel1.Controls.Add(this.标定button);
            this.splitContainer1.Panel1.Controls.Add(this.搜索区域button);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(314, 610);
            this.splitContainer1.SplitterDistance = 121;
            this.splitContainer1.TabIndex = 0;
            // 
            // 清空图像集Button
            // 
            this.清空图像集Button.Location = new System.Drawing.Point(114, 42);
            this.清空图像集Button.Name = "清空图像集Button";
            this.清空图像集Button.Size = new System.Drawing.Size(86, 36);
            this.清空图像集Button.TabIndex = 38;
            this.清空图像集Button.Text = "清空图像集";
            this.清空图像集Button.UseVisualStyleBackColor = true;
            this.清空图像集Button.Click += new System.EventHandler(this.清空图像集Button_Click);
            // 
            // 提取特征点Button
            // 
            this.提取特征点Button.Location = new System.Drawing.Point(114, 82);
            this.提取特征点Button.Name = "提取特征点Button";
            this.提取特征点Button.Size = new System.Drawing.Size(86, 36);
            this.提取特征点Button.TabIndex = 37;
            this.提取特征点Button.Text = "测试特征点";
            this.提取特征点Button.UseVisualStyleBackColor = true;
            this.提取特征点Button.Click += new System.EventHandler(this.提取网格点Button_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(225, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(86, 36);
            this.SaveButton.TabIndex = 36;
            this.SaveButton.Text = "保存图像";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // 保存button
            // 
            this.保存button.Location = new System.Drawing.Point(225, 83);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(86, 36);
            this.保存button.TabIndex = 32;
            this.保存button.Text = "保存参数";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // 加载图像button
            // 
            this.加载图像button.Location = new System.Drawing.Point(114, 4);
            this.加载图像button.Name = "加载图像button";
            this.加载图像button.Size = new System.Drawing.Size(86, 36);
            this.加载图像button.TabIndex = 31;
            this.加载图像button.Text = "加载图像集";
            this.加载图像button.UseVisualStyleBackColor = true;
            this.加载图像button.Click += new System.EventHandler(this.加载图像button_Click);
            // 
            // 采集图像button
            // 
            this.采集图像button.Location = new System.Drawing.Point(4, 4);
            this.采集图像button.Name = "采集图像button";
            this.采集图像button.Size = new System.Drawing.Size(86, 36);
            this.采集图像button.TabIndex = 30;
            this.采集图像button.Text = "采集图像";
            this.采集图像button.UseVisualStyleBackColor = true;
            this.采集图像button.Click += new System.EventHandler(this.采集图像button_Click);
            // 
            // 标定button
            // 
            this.标定button.Location = new System.Drawing.Point(225, 44);
            this.标定button.Name = "标定button";
            this.标定button.Size = new System.Drawing.Size(86, 36);
            this.标定button.TabIndex = 16;
            this.标定button.Text = "标定";
            this.标定button.UseVisualStyleBackColor = true;
            this.标定button.Click += new System.EventHandler(this.标定button_Click_1);
            // 
            // 搜索区域button
            // 
            this.搜索区域button.Location = new System.Drawing.Point(4, 44);
            this.搜索区域button.Name = "搜索区域button";
            this.搜索区域button.Size = new System.Drawing.Size(86, 36);
            this.搜索区域button.TabIndex = 20;
            this.搜索区域button.Text = "设置搜索区域";
            this.搜索区域button.UseVisualStyleBackColor = true;
            this.搜索区域button.Click += new System.EventHandler(this.标定区域button_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(314, 485);
            this.propertyGrid1.TabIndex = 39;
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
            this.采集源textBox.Size = new System.Drawing.Size(246, 21);
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
            // miniToolStrip
            // 
            this.miniToolStrip.AccessibleName = "新项选择";
            this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(687, 26);
            this.miniToolStrip.TabIndex = 32;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 328F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.bindingNavigator1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PathLabel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1015, 673);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // statusStrip2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip2, 2);
            this.statusStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.灰度值1Label,
            this.灰度值2Label,
            this.灰度值3Label,
            this.toolStripStatusLabel7,
            this.行坐标Label,
            this.列坐标Label});
            this.statusStrip2.Location = new System.Drawing.Point(328, 643);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(687, 30);
            this.statusStrip2.TabIndex = 35;
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
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MoveFirstItem,
            this.MovePreviousItem,
            this.bindingNavigatorSeparator,
            this.NavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.NavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(795, 0);
            this.bindingNavigator1.MoveFirstItem = this.MoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.NavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.MovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.NavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(220, 31);
            this.bindingNavigator1.TabIndex = 34;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 28);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // MoveFirstItem
            // 
            this.MoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("MoveFirstItem.Image")));
            this.MoveFirstItem.Name = "MoveFirstItem";
            this.MoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.MoveFirstItem.Size = new System.Drawing.Size(23, 28);
            this.MoveFirstItem.Text = "移到第一条记录";
            this.MoveFirstItem.Click += new System.EventHandler(this.MoveFirstItem_Click);
            // 
            // MovePreviousItem
            // 
            this.MovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("MovePreviousItem.Image")));
            this.MovePreviousItem.Name = "MovePreviousItem";
            this.MovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.MovePreviousItem.Size = new System.Drawing.Size(23, 28);
            this.MovePreviousItem.Text = "移到上一条记录";
            this.MovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 31);
            // 
            // NavigatorPositionItem
            // 
            this.NavigatorPositionItem.AccessibleName = "位置";
            this.NavigatorPositionItem.AutoSize = false;
            this.NavigatorPositionItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.NavigatorPositionItem.Name = "NavigatorPositionItem";
            this.NavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.NavigatorPositionItem.Text = "0";
            this.NavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // NavigatorMoveNextItem
            // 
            this.NavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("NavigatorMoveNextItem.Image")));
            this.NavigatorMoveNextItem.Name = "NavigatorMoveNextItem";
            this.NavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.NavigatorMoveNextItem.Size = new System.Drawing.Size(23, 28);
            this.NavigatorMoveNextItem.Text = "移到下一条记录";
            this.NavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 28);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // statusStrip3
            // 
            this.statusStrip3.Location = new System.Drawing.Point(0, 0);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(200, 22);
            this.statusStrip3.TabIndex = 0;
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PathLabel.Location = new System.Drawing.Point(331, 0);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(461, 31);
            this.PathLabel.TabIndex = 36;
            this.PathLabel.Text = "label1";
            this.PathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // 显示拼接图Button
            // 
            this.显示拼接图Button.Location = new System.Drawing.Point(3, 82);
            this.显示拼接图Button.Name = "显示拼接图Button";
            this.显示拼接图Button.Size = new System.Drawing.Size(86, 36);
            this.显示拼接图Button.TabIndex = 39;
            this.显示拼接图Button.Text = "显示拼接图";
            this.显示拼接图Button.UseVisualStyleBackColor = true;
            this.显示拼接图Button.Click += new System.EventHandler(this.显示拼接图Button_Click);
            // 
            // MosaicParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 673);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MosaicParamForm";
            this.Text = "图像拼接校正";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GridRectificationParamForm_FormClosing);
            this.Load += new System.EventHandler(this.GridRectificationParamForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.内参标定配置tabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton MoveFirstItem;
        private System.Windows.Forms.ToolStripButton MovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox NavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton NavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox 采集源textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 内参标定配置tabPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button 提取特征点Button;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.Button 加载图像button;
        private System.Windows.Forms.Button 采集图像button;
        private System.Windows.Forms.Button 标定button;
        private System.Windows.Forms.Button 搜索区域button;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.StatusStrip miniToolStrip;
        //private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        //private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        //private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        //private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        //private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        //private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        //private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.Button 清空图像集Button;
        private System.Windows.Forms.Label PathLabel;
        private System.Windows.Forms.Button 显示拼接图Button;
    }
}