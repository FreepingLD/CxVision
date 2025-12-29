namespace FunctionBlock
{
    partial class LaserLineScanPathForm
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
            this.显示条目comboBox = new System.Windows.Forms.ComboBox();
            this.视图工具toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Select = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Translate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Auto = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_3D = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.激光采集源comboBox = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.记录高度button = new System.Windows.Forms.Button();
            this.扫描高度textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.运动类型comboBox = new System.Windows.Forms.ComboBox();
            this.运动panel = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.视图工具toolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 330F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.Controls.Add(this.显示条目comboBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.视图工具toolStrip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(969, 580);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // 显示条目comboBox
            // 
            this.显示条目comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.显示条目comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.显示条目comboBox.FormattingEnabled = true;
            this.显示条目comboBox.Location = new System.Drawing.Point(546, 3);
            this.显示条目comboBox.Name = "显示条目comboBox";
            this.显示条目comboBox.Size = new System.Drawing.Size(420, 20);
            this.显示条目comboBox.TabIndex = 11;
            this.显示条目comboBox.SelectionChangeCommitted += new System.EventHandler(this.显示条目comboBox_SelectionChangeCommitted);
            // 
            // 视图工具toolStrip
            // 
            this.视图工具toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.视图工具toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Clear,
            this.toolStripButton_Select,
            this.toolStripButton_Translate,
            this.toolStripButton_Auto,
            this.toolStripButton_3D});
            this.视图工具toolStrip.Location = new System.Drawing.Point(330, 0);
            this.视图工具toolStrip.Name = "视图工具toolStrip";
            this.视图工具toolStrip.Size = new System.Drawing.Size(213, 28);
            this.视图工具toolStrip.TabIndex = 15;
            this.视图工具toolStrip.Text = "toolStrip2";
            this.视图工具toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(36, 25);
            this.toolStripButton_Clear.Text = "清除";
            // 
            // toolStripButton_Select
            // 
            this.toolStripButton_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Select.Name = "toolStripButton_Select";
            this.toolStripButton_Select.Size = new System.Drawing.Size(36, 25);
            this.toolStripButton_Select.Text = "选择";
            // 
            // toolStripButton_Translate
            // 
            this.toolStripButton_Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Translate.Name = "toolStripButton_Translate";
            this.toolStripButton_Translate.Size = new System.Drawing.Size(36, 25);
            this.toolStripButton_Translate.Text = "平移";
            // 
            // toolStripButton_Auto
            // 
            this.toolStripButton_Auto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Auto.Name = "toolStripButton_Auto";
            this.toolStripButton_Auto.Size = new System.Drawing.Size(60, 25);
            this.toolStripButton_Auto.Text = "适应窗口";
            // 
            // toolStripButton_3D
            // 
            this.toolStripButton_3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_3D.Image = global::FunctionBlock.Properties.Resources._1606742721_1_;
            this.toolStripButton_3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_3D.Name = "toolStripButton_3D";
            this.toolStripButton_3D.Size = new System.Drawing.Size(28, 25);
            this.toolStripButton_3D.Text = "3D";
            // 
            // statusStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip1, 3);
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 550);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(969, 30);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 25);
            this.toolStripStatusLabel1.Text = "执行结果：";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(28, 25);
            this.toolStripStatusLabel2.Text = "……";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 31);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 4);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(324, 516);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(316, 490);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox6, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.运动panel, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 202F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(316, 490);
            this.tableLayoutPanel2.TabIndex = 19;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBox1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 51);
            this.groupBox4.Name = "groupBox4";
            this.tableLayoutPanel2.SetRowSpan(this.groupBox4, 2);
            this.groupBox4.Size = new System.Drawing.Size(310, 184);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "路径元素";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 17);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(304, 164);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.激光采集源comboBox);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(310, 42);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "传感器";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "激光传感器:";
            // 
            // 激光采集源comboBox
            // 
            this.激光采集源comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.激光采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.激光采集源comboBox.FormattingEnabled = true;
            this.激光采集源comboBox.Location = new System.Drawing.Point(74, 15);
            this.激光采集源comboBox.Name = "激光采集源comboBox";
            this.激光采集源comboBox.Size = new System.Drawing.Size(233, 20);
            this.激光采集源comboBox.TabIndex = 29;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.记录高度button);
            this.groupBox6.Controls.Add(this.扫描高度textBox);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.运动类型comboBox);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 241);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(310, 89);
            this.groupBox6.TabIndex = 41;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "扫描参数";
            // 
            // 记录高度button
            // 
            this.记录高度button.Location = new System.Drawing.Point(66, 66);
            this.记录高度button.Name = "记录高度button";
            this.记录高度button.Size = new System.Drawing.Size(83, 23);
            this.记录高度button.TabIndex = 39;
            this.记录高度button.Text = "记录高度";
            this.记录高度button.UseVisualStyleBackColor = true;
            this.记录高度button.Click += new System.EventHandler(this.记录高度button_Click);
            // 
            // 扫描高度textBox
            // 
            this.扫描高度textBox.Location = new System.Drawing.Point(66, 41);
            this.扫描高度textBox.Name = "扫描高度textBox";
            this.扫描高度textBox.Size = new System.Drawing.Size(83, 21);
            this.扫描高度textBox.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "扫描高度:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 35;
            this.label3.Text = "运动类型:";
            // 
            // 运动类型comboBox
            // 
            this.运动类型comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.运动类型comboBox.FormattingEnabled = true;
            this.运动类型comboBox.Location = new System.Drawing.Point(66, 17);
            this.运动类型comboBox.Name = "运动类型comboBox";
            this.运动类型comboBox.Size = new System.Drawing.Size(83, 20);
            this.运动类型comboBox.TabIndex = 36;
            // 
            // 运动panel
            // 
            this.运动panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.运动panel.Location = new System.Drawing.Point(0, 333);
            this.运动panel.Margin = new System.Windows.Forms.Padding(0);
            this.运动panel.Name = "运动panel";
            this.运动panel.Size = new System.Drawing.Size(316, 157);
            this.运动panel.TabIndex = 43;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(330, 28);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources._1606742721_1_;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(52, 25);
            this.toolStripButton_Run.Text = "执行";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(333, 31);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 4);
            this.hWindowControl1.Size = new System.Drawing.Size(633, 516);
            this.hWindowControl1.TabIndex = 21;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(633, 516);
            // 
            // LaserLineScanPathForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(969, 580);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LaserLineScanPathForm";
            this.Text = "直线路径扫描";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LaserLineScanPathForm_FormClosing);
            this.Load += new System.EventHandler(this.LaserLineScanPathForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.视图工具toolStrip.ResumeLayout(false);
            this.视图工具toolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox 显示条目comboBox;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStrip 视图工具toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_Clear;
        private System.Windows.Forms.ToolStripButton toolStripButton_Select;
        private System.Windows.Forms.ToolStripButton toolStripButton_Translate;
        private System.Windows.Forms.ToolStripButton toolStripButton_Auto;
        private System.Windows.Forms.ToolStripButton toolStripButton_3D;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox 激光采集源comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 运动类型comboBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel 运动panel;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button 记录高度button;
        private System.Windows.Forms.TextBox 扫描高度textBox;
        private System.Windows.Forms.Label label1;
    }
}