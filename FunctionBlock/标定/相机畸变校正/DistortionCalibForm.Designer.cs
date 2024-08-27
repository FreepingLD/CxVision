
namespace FunctionBlock
{
    partial class DistortionCalibForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistortionCalibForm));
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip3 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值1Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值2Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.灰度值3Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.行坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.列坐标Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.标定执行button = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.上相机实时采集checkBox = new System.Windows.Forms.CheckBox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.元素信息tabPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.打开toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.保存toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.工具toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label55 = new System.Windows.Forms.Label();
            this.圆心距textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.行数domainUpDown = new System.Windows.Forms.DomainUpDown();
            this.列数domainUpDown = new System.Windows.Forms.DomainUpDown();
            this.保存button = new System.Windows.Forms.Button();
            this.原图Btn = new System.Windows.Forms.Button();
            this.校正图Btn = new System.Windows.Forms.Button();
            this.tableLayoutPanel4.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 326F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.statusStrip3, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.tabControl2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox2, 0, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 44);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 164F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1066, 709);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // statusStrip3
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.statusStrip3, 2);
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
            this.statusStrip3.Location = new System.Drawing.Point(326, 677);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(740, 32);
            this.statusStrip3.TabIndex = 22;
            this.statusStrip3.Text = "statusStrip3";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(56, 27);
            this.toolStripStatusLabel4.Text = "灰度值：";
            // 
            // 灰度值1Label
            // 
            this.灰度值1Label.Name = "灰度值1Label";
            this.灰度值1Label.Size = new System.Drawing.Size(28, 27);
            this.灰度值1Label.Text = "……";
            // 
            // 灰度值2Label
            // 
            this.灰度值2Label.Name = "灰度值2Label";
            this.灰度值2Label.Size = new System.Drawing.Size(28, 27);
            this.灰度值2Label.Text = "……";
            // 
            // 灰度值3Label
            // 
            this.灰度值3Label.Name = "灰度值3Label";
            this.灰度值3Label.Size = new System.Drawing.Size(28, 27);
            this.灰度值3Label.Text = "……";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(44, 27);
            this.toolStripStatusLabel9.Text = "坐标：";
            // 
            // 行坐标Label
            // 
            this.行坐标Label.Name = "行坐标Label";
            this.行坐标Label.Size = new System.Drawing.Size(28, 27);
            this.行坐标Label.Text = "……";
            // 
            // 列坐标Label
            // 
            this.列坐标Label.Name = "列坐标Label";
            this.列坐标Label.Size = new System.Drawing.Size(28, 27);
            this.列坐标Label.Text = "……";
            // 
            // 标定执行button
            // 
            this.标定执行button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.标定执行button.Location = new System.Drawing.Point(228, 101);
            this.标定执行button.Name = "标定执行button";
            this.标定执行button.Size = new System.Drawing.Size(86, 38);
            this.标定执行button.TabIndex = 2;
            this.标定执行button.Text = "标定执行";
            this.标定执行button.UseVisualStyleBackColor = true;
            this.标定执行button.Click += new System.EventHandler(this.标定执行button_Click_1);
            // 
            // tabControl2
            // 
            this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tableLayoutPanel4.SetColumnSpan(this.tabControl2, 2);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.元素信息tabPage);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(326, 0);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tableLayoutPanel4.SetRowSpan(this.tabControl2, 4);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(740, 677);
            this.tabControl2.TabIndex = 7;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.上相机实时采集checkBox);
            this.tabPage4.Controls.Add(this.hWindowControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(732, 648);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "视图";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // 上相机实时采集checkBox
            // 
            this.上相机实时采集checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.上相机实时采集checkBox.BackColor = System.Drawing.Color.Lime;
            this.上相机实时采集checkBox.Location = new System.Drawing.Point(3, 2);
            this.上相机实时采集checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.上相机实时采集checkBox.Name = "上相机实时采集checkBox";
            this.上相机实时采集checkBox.Size = new System.Drawing.Size(20, 20);
            this.上相机实时采集checkBox.TabIndex = 21;
            this.上相机实时采集checkBox.TabStop = false;
            this.上相机实时采集checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.上相机实时采集checkBox.UseVisualStyleBackColor = false;
            this.上相机实时采集checkBox.CheckedChanged += new System.EventHandler(this.上相机实时采集checkBox_CheckedChanged);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(726, 642);
            this.hWindowControl1.TabIndex = 9;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(726, 642);
            // 
            // 元素信息tabPage
            // 
            this.元素信息tabPage.Location = new System.Drawing.Point(4, 25);
            this.元素信息tabPage.Name = "元素信息tabPage";
            this.元素信息tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.元素信息tabPage.Size = new System.Drawing.Size(732, 648);
            this.元素信息tabPage.TabIndex = 4;
            this.元素信息tabPage.Text = "元素信息";
            this.元素信息tabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel4.SetRowSpan(this.groupBox1, 3);
            this.groupBox1.Size = new System.Drawing.Size(320, 507);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标定程序";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 17);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(314, 487);
            this.treeView1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1072, 756);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 2);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run,
            this.打开toolStripButton,
            this.保存toolStripButton,
            this.工具toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1072, 41);
            this.toolStrip1.TabIndex = 20;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources._1606742721_1_;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(36, 38);
            this.toolStripButton_Run.Text = "执行";
            this.toolStripButton_Run.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 打开toolStripButton
            // 
            this.打开toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742449_1_;
            this.打开toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打开toolStripButton.Name = "打开toolStripButton";
            this.打开toolStripButton.Size = new System.Drawing.Size(36, 38);
            this.打开toolStripButton.Text = "打开";
            this.打开toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 保存toolStripButton
            // 
            this.保存toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742495_1_;
            this.保存toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.保存toolStripButton.Name = "保存toolStripButton";
            this.保存toolStripButton.Size = new System.Drawing.Size(36, 38);
            this.保存toolStripButton.Text = "保存";
            this.保存toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 工具toolStripButton
            // 
            this.工具toolStripButton.Image = global::FunctionBlock.Properties.Resources._1606742307_1_;
            this.工具toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.工具toolStripButton.Name = "工具toolStripButton";
            this.工具toolStripButton.Size = new System.Drawing.Size(60, 38);
            this.工具toolStripButton.Text = "检测工具";
            this.工具toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.校正图Btn);
            this.groupBox2.Controls.Add(this.原图Btn);
            this.groupBox2.Controls.Add(this.保存button);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.label41);
            this.groupBox2.Controls.Add(this.行数domainUpDown);
            this.groupBox2.Controls.Add(this.列数domainUpDown);
            this.groupBox2.Controls.Add(this.label55);
            this.groupBox2.Controls.Add(this.圆心距textBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.标定执行button);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 516);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel4.SetRowSpan(this.groupBox2, 2);
            this.groupBox2.Size = new System.Drawing.Size(320, 190);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标定参数";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label55.Location = new System.Drawing.Point(169, 89);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(21, 14);
            this.label55.TabIndex = 38;
            this.label55.Text = "mm";
            // 
            // 圆心距textBox
            // 
            this.圆心距textBox.Location = new System.Drawing.Point(55, 84);
            this.圆心距textBox.Name = "圆心距textBox";
            this.圆心距textBox.Size = new System.Drawing.Size(110, 21);
            this.圆心距textBox.TabIndex = 37;
            this.圆心距textBox.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "圆心距：";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(12, 60);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(41, 12);
            this.label30.TabIndex = 40;
            this.label30.Text = "列数：";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(12, 32);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(41, 12);
            this.label41.TabIndex = 39;
            this.label41.Text = "行数：";
            // 
            // 行数domainUpDown
            // 
            this.行数domainUpDown.Location = new System.Drawing.Point(55, 28);
            this.行数domainUpDown.Name = "行数domainUpDown";
            this.行数domainUpDown.Size = new System.Drawing.Size(110, 21);
            this.行数domainUpDown.TabIndex = 41;
            this.行数domainUpDown.Text = "7";
            // 
            // 列数domainUpDown
            // 
            this.列数domainUpDown.Location = new System.Drawing.Point(55, 56);
            this.列数domainUpDown.Name = "列数domainUpDown";
            this.列数domainUpDown.Size = new System.Drawing.Size(110, 21);
            this.列数domainUpDown.TabIndex = 42;
            this.列数domainUpDown.Text = "7";
            // 
            // 保存button
            // 
            this.保存button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.保存button.Location = new System.Drawing.Point(228, 146);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(86, 38);
            this.保存button.TabIndex = 43;
            this.保存button.Text = "保存参数";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // 原图Btn
            // 
            this.原图Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.原图Btn.Location = new System.Drawing.Point(228, 11);
            this.原图Btn.Name = "原图Btn";
            this.原图Btn.Size = new System.Drawing.Size(86, 38);
            this.原图Btn.TabIndex = 44;
            this.原图Btn.Text = "原图";
            this.原图Btn.UseVisualStyleBackColor = true;
            this.原图Btn.Click += new System.EventHandler(this.原图Btn_Click);
            // 
            // 校正图Btn
            // 
            this.校正图Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.校正图Btn.Location = new System.Drawing.Point(228, 57);
            this.校正图Btn.Name = "校正图Btn";
            this.校正图Btn.Size = new System.Drawing.Size(86, 38);
            this.校正图Btn.TabIndex = 45;
            this.校正图Btn.Text = "校正图";
            this.校正图Btn.UseVisualStyleBackColor = true;
            this.校正图Btn.Click += new System.EventHandler(this.校正图Btn_Click);
            // 
            // DistortionCalibForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 756);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DistortionCalibForm";
            this.Text = "相机畸变校正";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CaliCaliboardForm_FormClosing);
            this.Load += new System.EventHandler(this.CaliCaliboardForm_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.ToolStripButton 打开toolStripButton;
        private System.Windows.Forms.ToolStripButton 保存toolStripButton;
        private System.Windows.Forms.ToolStripButton 工具toolStripButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button 标定执行button;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值1Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值2Label;
        private System.Windows.Forms.ToolStripStatusLabel 灰度值3Label;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel 行坐标Label;
        private System.Windows.Forms.ToolStripStatusLabel 列坐标Label;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox 上相机实时采集checkBox;
        private System.Windows.Forms.TabPage 元素信息tabPage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox 圆心距textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.DomainUpDown 行数domainUpDown;
        private System.Windows.Forms.DomainUpDown 列数domainUpDown;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.Button 校正图Btn;
        private System.Windows.Forms.Button 原图Btn;
    }
}