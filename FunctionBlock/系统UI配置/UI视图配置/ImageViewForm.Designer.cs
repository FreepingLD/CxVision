namespace FunctionBlock
{
    partial class ImageViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.statusStrip3 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值1Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值2Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值3Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.行坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.列坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.实时采集checkBox = new System.Windows.Forms.CheckBox();
            this.传感器comboBox = new System.Windows.Forms.ComboBox();
            this.标定工具栏toolStrip = new System.Windows.Forms.ToolStrip();
            this.标定toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.相机toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.设置曝光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置增益ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Roi绘制toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.绘制矩形toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制圆形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制椭圆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制直线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.标定工具栏toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 212F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMax, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMin, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip3, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.实时采集checkBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.传感器comboBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.标定工具栏toolStrip, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(597, 534);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.AllowDrop = true;
            this.hWindowControl1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.hWindowControl1.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 7);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 40);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Size = new System.Drawing.Size(597, 469);
            this.hWindowControl1.TabIndex = 30;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(597, 469);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 20);
            this.label1.TabIndex = 29;
            this.label1.Text = "实时";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(572, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(25, 20);
            this.buttonClose.TabIndex = 28;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(547, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(25, 20);
            this.buttonMax.TabIndex = 27;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Enabled = false;
            this.buttonMin.Location = new System.Drawing.Point(522, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(25, 20);
            this.buttonMin.TabIndex = 26;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 4);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(518, 16);
            this.titleLabel.TabIndex = 25;
            this.titleLabel.Text = "CxVision";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // statusStrip3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip3, 4);
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
            this.statusStrip3.Location = new System.Drawing.Point(271, 509);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(326, 25);
            this.statusStrip3.TabIndex = 22;
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
            // 实时采集checkBox
            // 
            this.实时采集checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.实时采集checkBox.AutoSize = true;
            this.实时采集checkBox.BackColor = System.Drawing.Color.Lime;
            this.实时采集checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.实时采集checkBox.Location = new System.Drawing.Point(0, 20);
            this.实时采集checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.实时采集checkBox.Name = "实时采集checkBox";
            this.实时采集checkBox.Size = new System.Drawing.Size(20, 20);
            this.实时采集checkBox.TabIndex = 20;
            this.实时采集checkBox.TabStop = false;
            this.实时采集checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.实时采集checkBox.UseVisualStyleBackColor = false;
            this.实时采集checkBox.CheckedChanged += new System.EventHandler(this.实时采集checkBox_CheckedChanged);
            // 
            // 传感器comboBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.传感器comboBox, 5);
            this.传感器comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.传感器comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.传感器comboBox.FormattingEnabled = true;
            this.传感器comboBox.ItemHeight = 12;
            this.传感器comboBox.Location = new System.Drawing.Point(59, 20);
            this.传感器comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.传感器comboBox.Name = "传感器comboBox";
            this.传感器comboBox.Size = new System.Drawing.Size(538, 20);
            this.传感器comboBox.TabIndex = 3;
            this.传感器comboBox.SelectionChangeCommitted += new System.EventHandler(this.传感器comboBox_SelectionChangeCommitted);
            // 
            // 标定工具栏toolStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.标定工具栏toolStrip, 3);
            this.标定工具栏toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.标定工具栏toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.标定toolStripButton,
            this.相机toolStripDropDownButton,
            this.Roi绘制toolStripDropDownButton});
            this.标定工具栏toolStrip.Location = new System.Drawing.Point(0, 509);
            this.标定工具栏toolStrip.Name = "标定工具栏toolStrip";
            this.标定工具栏toolStrip.Size = new System.Drawing.Size(271, 25);
            this.标定工具栏toolStrip.TabIndex = 31;
            this.标定工具栏toolStrip.Text = "toolStrip1";
            this.标定工具栏toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.标定工具栏toolStrip_ItemClicked);
            // 
            // 标定toolStripButton
            // 
            this.标定toolStripButton.Image = global::FunctionBlock.Properties.Resources.标定_20_X_20;
            this.标定toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.标定toolStripButton.Name = "标定toolStripButton";
            this.标定toolStripButton.Size = new System.Drawing.Size(52, 22);
            this.标定toolStripButton.Text = "标定";
            // 
            // 相机toolStripDropDownButton
            // 
            this.相机toolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置曝光toolStripMenuItem,
            this.设置增益ToolStripMenuItem});
            this.相机toolStripDropDownButton.Image = global::FunctionBlock.Properties.Resources.设置20_X_20;
            this.相机toolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.相机toolStripDropDownButton.Name = "相机toolStripDropDownButton";
            this.相机toolStripDropDownButton.Size = new System.Drawing.Size(85, 22);
            this.相机toolStripDropDownButton.Text = "相机设置";
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
            // Roi绘制toolStripDropDownButton
            // 
            this.Roi绘制toolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制矩形toolStripMenuItem,
            this.绘制圆形ToolStripMenuItem,
            this.绘制点ToolStripMenuItem,
            this.绘制椭圆ToolStripMenuItem,
            this.绘制直线ToolStripMenuItem});
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
            this.绘制矩形toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制矩形toolStripMenuItem.Text = "绘制矩形";
            // 
            // 绘制圆形ToolStripMenuItem
            // 
            this.绘制圆形ToolStripMenuItem.Image = global::FunctionBlock.Properties.Resources.圆形_20_X_20;
            this.绘制圆形ToolStripMenuItem.Name = "绘制圆形ToolStripMenuItem";
            this.绘制圆形ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制圆形ToolStripMenuItem.Text = "绘制圆形";
            // 
            // 绘制点ToolStripMenuItem
            // 
            this.绘制点ToolStripMenuItem.Name = "绘制点ToolStripMenuItem";
            this.绘制点ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制点ToolStripMenuItem.Text = "绘制点";
            // 
            // 绘制椭圆ToolStripMenuItem
            // 
            this.绘制椭圆ToolStripMenuItem.Name = "绘制椭圆ToolStripMenuItem";
            this.绘制椭圆ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制椭圆ToolStripMenuItem.Text = "绘制椭圆";
            // 
            // 绘制直线ToolStripMenuItem
            // 
            this.绘制直线ToolStripMenuItem.Name = "绘制直线ToolStripMenuItem";
            this.绘制直线ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制直线ToolStripMenuItem.Text = "绘制直线";
            // 
            // ImageViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 534);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ImageViewForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageViewForm_FormClosing);
            this.Load += new System.EventHandler(this.ImageViewForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageViewForm_MouseDown);
            this.Move += new System.EventHandler(this.ImageViewForm_Move);
            this.Resize += new System.EventHandler(this.ImageViewForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
            this.标定工具栏toolStrip.ResumeLayout(false);
            this.标定工具栏toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox 传感器comboBox;
        private System.Windows.Forms.CheckBox 实时采集checkBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.Label label1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.ToolStrip 标定工具栏toolStrip;
        private System.Windows.Forms.ToolStripButton 标定toolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton 相机toolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem 设置曝光toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置增益ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton Roi绘制toolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem 绘制矩形toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制圆形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制椭圆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制直线ToolStripMenuItem;
    }
}