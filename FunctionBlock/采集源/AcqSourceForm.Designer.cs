namespace FunctionBlock
{
    partial class AcqSourceForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.传感器参数设置button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.采集源comboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.运动坐标系comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.传感器名称comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.运动轴comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.光源dataGridView1 = new System.Windows.Forms.DataGridView();
            this.LightControlColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LightChennelColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SetLight = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.传感器listBox = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.DeleSensorButton = new System.Windows.Forms.Button();
            this.AddSensoButton = new System.Windows.Forms.Button();
            this.采集源参数tabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.图像保存btn = new System.Windows.Forms.Button();
            this.图像采集btn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.光源dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 传感器参数设置button
            // 
            this.传感器参数设置button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.传感器参数设置button.Location = new System.Drawing.Point(102, 15);
            this.传感器参数设置button.Name = "传感器参数设置button";
            this.传感器参数设置button.Size = new System.Drawing.Size(80, 45);
            this.传感器参数设置button.TabIndex = 27;
            this.传感器参数设置button.Text = "传感器参数设置";
            this.传感器参数设置button.UseVisualStyleBackColor = true;
            this.传感器参数设置button.Click += new System.EventHandler(this.传感器参数设置button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 379F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 245F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1138, 698);
            this.tableLayoutPanel1.TabIndex = 29;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(382, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 4);
            this.hWindowControl1.Size = new System.Drawing.Size(753, 692);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(753, 692);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.采集源参数tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 629);
            this.tabControl1.TabIndex = 36;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(365, 603);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "配置";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.采集源comboBox);
            this.groupBox3.Location = new System.Drawing.Point(0, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 46);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "采集源名称";
            // 
            // 采集源comboBox
            // 
            this.采集源comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.采集源comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.采集源comboBox.FormattingEnabled = true;
            this.采集源comboBox.Location = new System.Drawing.Point(78, 17);
            this.采集源comboBox.Name = "采集源comboBox";
            this.采集源comboBox.Size = new System.Drawing.Size(271, 20);
            this.采集源comboBox.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(0, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 548);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "采集源配置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.光源dataGridView1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(356, 528);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.运动坐标系comboBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.传感器名称comboBox);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.运动轴comboBox);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(356, 93);
            this.panel2.TabIndex = 0;
            // 
            // 运动坐标系comboBox
            // 
            this.运动坐标系comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.运动坐标系comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.运动坐标系comboBox.FormattingEnabled = true;
            this.运动坐标系comboBox.Location = new System.Drawing.Point(75, 7);
            this.运动坐标系comboBox.Name = "运动坐标系comboBox";
            this.运动坐标系comboBox.Size = new System.Drawing.Size(275, 20);
            this.运动坐标系comboBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "运动坐标系:";
            // 
            // 传感器名称comboBox
            // 
            this.传感器名称comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.传感器名称comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.传感器名称comboBox.FormattingEnabled = true;
            this.传感器名称comboBox.Location = new System.Drawing.Point(75, 65);
            this.传感器名称comboBox.Name = "传感器名称comboBox";
            this.传感器名称comboBox.Size = new System.Drawing.Size(275, 20);
            this.传感器名称comboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "传感器名称:";
            // 
            // 运动轴comboBox
            // 
            this.运动轴comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.运动轴comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.运动轴comboBox.FormattingEnabled = true;
            this.运动轴comboBox.Location = new System.Drawing.Point(75, 35);
            this.运动轴comboBox.Name = "运动轴comboBox";
            this.运动轴comboBox.Size = new System.Drawing.Size(275, 20);
            this.运动轴comboBox.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 34;
            this.label3.Text = "运动轴:";
            // 
            // 光源dataGridView1
            // 
            this.光源dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.光源dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LightControlColumn,
            this.LightChennelColumn,
            this.Column2,
            this.SetLight,
            this.DeleteBtn});
            this.光源dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.光源dataGridView1.Location = new System.Drawing.Point(3, 203);
            this.光源dataGridView1.Name = "光源dataGridView1";
            this.光源dataGridView1.RowHeadersWidth = 5;
            this.光源dataGridView1.RowTemplate.Height = 23;
            this.光源dataGridView1.Size = new System.Drawing.Size(350, 322);
            this.光源dataGridView1.TabIndex = 31;
            this.光源dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.光源dataGridView1_CellContentClick);
            this.光源dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.光源dataGridView1_DataError);
            // 
            // LightControlColumn
            // 
            this.LightControlColumn.DataPropertyName = "LightName";
            this.LightControlColumn.HeaderText = "光源控制器";
            this.LightControlColumn.MinimumWidth = 6;
            this.LightControlColumn.Name = "LightControlColumn";
            this.LightControlColumn.Width = 125;
            // 
            // LightChennelColumn
            // 
            this.LightChennelColumn.DataPropertyName = "Channel";
            this.LightChennelColumn.HeaderText = "光源通道";
            this.LightChennelColumn.MinimumWidth = 6;
            this.LightChennelColumn.Name = "LightChennelColumn";
            this.LightChennelColumn.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "LightValue";
            this.Column2.HeaderText = "光源值";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // SetLight
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "设置亮度";
            this.SetLight.DefaultCellStyle = dataGridViewCellStyle1;
            this.SetLight.HeaderText = "设置亮度";
            this.SetLight.MinimumWidth = 6;
            this.SetLight.Name = "SetLight";
            this.SetLight.Width = 80;
            // 
            // DeleteBtn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle2;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.MinimumWidth = 6;
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 80;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.传感器listBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 69);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "传感器列表";
            // 
            // 传感器listBox
            // 
            this.传感器listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.传感器listBox.FormattingEnabled = true;
            this.传感器listBox.ItemHeight = 12;
            this.传感器listBox.Location = new System.Drawing.Point(3, 17);
            this.传感器listBox.Name = "传感器listBox";
            this.传感器listBox.Size = new System.Drawing.Size(344, 49);
            this.传感器listBox.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.DeleSensorButton);
            this.panel3.Controls.Add(this.AddSensoButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 93);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(356, 32);
            this.panel3.TabIndex = 1;
            // 
            // DeleSensorButton
            // 
            this.DeleSensorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleSensorButton.Location = new System.Drawing.Point(286, 6);
            this.DeleSensorButton.Name = "DeleSensorButton";
            this.DeleSensorButton.Size = new System.Drawing.Size(64, 23);
            this.DeleSensorButton.TabIndex = 33;
            this.DeleSensorButton.Text = "Dele(-)";
            this.DeleSensorButton.UseVisualStyleBackColor = true;
            // 
            // AddSensoButton
            // 
            this.AddSensoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddSensoButton.Location = new System.Drawing.Point(223, 6);
            this.AddSensoButton.Name = "AddSensoButton";
            this.AddSensoButton.Size = new System.Drawing.Size(56, 23);
            this.AddSensoButton.TabIndex = 32;
            this.AddSensoButton.Text = "Add(+)";
            this.AddSensoButton.UseVisualStyleBackColor = true;
            // 
            // 采集源参数tabPage
            // 
            this.采集源参数tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.采集源参数tabPage.Location = new System.Drawing.Point(4, 22);
            this.采集源参数tabPage.Name = "采集源参数tabPage";
            this.采集源参数tabPage.Size = new System.Drawing.Size(361, 603);
            this.采集源参数tabPage.TabIndex = 1;
            this.采集源参数tabPage.Text = "采集源参数设置";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.图像保存btn);
            this.panel1.Controls.Add(this.传感器参数设置button);
            this.panel1.Controls.Add(this.图像采集btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 635);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 63);
            this.panel1.TabIndex = 28;
            // 
            // 图像保存btn
            // 
            this.图像保存btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.图像保存btn.Location = new System.Drawing.Point(204, 15);
            this.图像保存btn.Name = "图像保存btn";
            this.图像保存btn.Size = new System.Drawing.Size(75, 45);
            this.图像保存btn.TabIndex = 35;
            this.图像保存btn.Text = "图像保存";
            this.图像保存btn.UseVisualStyleBackColor = true;
            this.图像保存btn.Click += new System.EventHandler(this.图像保存btn_Click);
            // 
            // 图像采集btn
            // 
            this.图像采集btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.图像采集btn.Location = new System.Drawing.Point(297, 15);
            this.图像采集btn.Name = "图像采集btn";
            this.图像采集btn.Size = new System.Drawing.Size(75, 45);
            this.图像采集btn.TabIndex = 34;
            this.图像采集btn.Text = "图像采集";
            this.图像采集btn.UseVisualStyleBackColor = true;
            this.图像采集btn.Click += new System.EventHandler(this.图像采集btn_Click);
            // 
            // AcqSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 698);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AcqSourceForm";
            this.Text = "采集源窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AcqSourceForm_FormClosing);
            this.Load += new System.EventHandler(this.AcqSourceForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.光源dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button 传感器参数设置button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightNameColumn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button 图像保存btn;
        private System.Windows.Forms.Button 图像采集btn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox 采集源comboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox 运动坐标系comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 传感器名称comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 运动轴comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView 光源dataGridView1;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightControlColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightChennelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewButtonColumn SetLight;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox 传感器listBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button DeleSensorButton;
        private System.Windows.Forms.Button AddSensoButton;
        private System.Windows.Forms.TabPage 采集源参数tabPage;
    }
}