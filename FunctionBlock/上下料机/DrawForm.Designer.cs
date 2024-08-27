namespace FunctionBlock
{
    partial class DrawForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.视图工具toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Translate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Auto = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_3D = new System.Windows.Forms.ToolStripButton();
            this.绘图toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.矩形1toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.矩形2toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.圆形toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.直线toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.椭圆toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.多边形toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.多段线toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ROItoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值1Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值2Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值3Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.行坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.列坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 241F));
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip2, 0, 5);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(839, 756);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 3);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(3, 31);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 4);
            this.hWindowControl1.Size = new System.Drawing.Size(833, 692);
            this.hWindowControl1.TabIndex = 17;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(833, 692);
            // 
            // 视图工具toolStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.视图工具toolStrip, 3);
            this.视图工具toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.视图工具toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.视图工具toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Clear,
            this.toolStripButton_Select,
            this.toolStripButton_Translate,
            this.toolStripButton_Auto,
            this.toolStripButton_3D,
            this.绘图toolStripDropDownButton,
            this.删除ROItoolStripButton});
            this.视图工具toolStrip.Location = new System.Drawing.Point(0, 0);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(839, 28);
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
            // 绘图toolStripDropDownButton
            // 
            this.绘图toolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.矩形1toolStripMenuItem,
            this.矩形2toolStripMenuItem,
            this.圆形toolStripMenuItem,
            this.直线toolStripMenuItem,
            this.椭圆toolStripMenuItem,
            this.多边形toolStripMenuItem,
            this.多段线toolStripMenuItem});
            this.绘图toolStripDropDownButton.Image = global::FunctionBlock.Properties.Resources.绘图;
            this.绘图toolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.绘图toolStripDropDownButton.Name = "绘图toolStripDropDownButton";
            this.绘图toolStripDropDownButton.Size = new System.Drawing.Size(65, 25);
            this.绘图toolStripDropDownButton.Text = "绘图";
            this.绘图toolStripDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.绘图toolStripDropDownButton_DropDownItemClicked);
            // 
            // 矩形1toolStripMenuItem
            // 
            this.矩形1toolStripMenuItem.Name = "矩形1toolStripMenuItem";
            this.矩形1toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.矩形1toolStripMenuItem.Text = "矩形1";
            // 
            // 矩形2toolStripMenuItem
            // 
            this.矩形2toolStripMenuItem.Name = "矩形2toolStripMenuItem";
            this.矩形2toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.矩形2toolStripMenuItem.Text = "矩形2";
            // 
            // 圆形toolStripMenuItem
            // 
            this.圆形toolStripMenuItem.Name = "圆形toolStripMenuItem";
            this.圆形toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.圆形toolStripMenuItem.Text = "圆形";
            // 
            // 直线toolStripMenuItem
            // 
            this.直线toolStripMenuItem.Name = "直线toolStripMenuItem";
            this.直线toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.直线toolStripMenuItem.Text = "直线";
            // 
            // 椭圆toolStripMenuItem
            // 
            this.椭圆toolStripMenuItem.Name = "椭圆toolStripMenuItem";
            this.椭圆toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.椭圆toolStripMenuItem.Text = "椭圆";
            // 
            // 多边形toolStripMenuItem
            // 
            this.多边形toolStripMenuItem.Name = "多边形toolStripMenuItem";
            this.多边形toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.多边形toolStripMenuItem.Text = "多边形";
            // 
            // 多段线toolStripMenuItem
            // 
            this.多段线toolStripMenuItem.Name = "多段线toolStripMenuItem";
            this.多段线toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.多段线toolStripMenuItem.Text = "多段线";
            // 
            // 删除ROItoolStripButton
            // 
            this.删除ROItoolStripButton.Image = global::FunctionBlock.Properties.Resources.删除ROI;
            this.删除ROItoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除ROItoolStripButton.Name = "删除ROItoolStripButton";
            this.删除ROItoolStripButton.Size = new System.Drawing.Size(78, 25);
            this.删除ROItoolStripButton.Text = "删除ROI";
            // 
            // statusStrip2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip2, 3);
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
            this.statusStrip2.Location = new System.Drawing.Point(0, 726);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(839, 30);
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
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 756);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DrawForm";
            this.Text = "区域设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DrawForm_FormClosing);
            this.Load += new System.EventHandler(this.DrawForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
        private System.Windows.Forms.ToolStripDropDownButton 绘图toolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem 矩形1toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 矩形2toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 圆形toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 直线toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 椭圆toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 多边形toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 多段线toolStripMenuItem;
        private System.Windows.Forms.ToolStripButton 删除ROItoolStripButton;
    }
}