namespace FunctionBlock
{
    partial class AcqSourceConfigForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcqSourceConfigForm));
            this.label1 = new System.Windows.Forms.Label();
            this.传感器列表comboBox = new System.Windows.Forms.ComboBox();
            this.运动坐标系comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.光源dataGridView1 = new System.Windows.Forms.DataGridView();
            this.LightControlColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LightChennelColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LightValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SetLightBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.OpenBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.曝光参数comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.运动轴comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.采集源listBox = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.保存button = new System.Windows.Forms.Button();
            this.删除button = new System.Windows.Forms.Button();
            this.AddSourceButton = new System.Windows.Forms.Button();
            this.新建Button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.采集源名称textBox = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.设置采集源参数Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.光源dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "传感器名称:";
            // 
            // 传感器列表comboBox
            // 
            this.传感器列表comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.传感器列表comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.传感器列表comboBox.FormattingEnabled = true;
            this.传感器列表comboBox.ItemHeight = 12;
            this.传感器列表comboBox.Location = new System.Drawing.Point(83, 65);
            this.传感器列表comboBox.Name = "传感器列表comboBox";
            this.传感器列表comboBox.Size = new System.Drawing.Size(454, 20);
            this.传感器列表comboBox.TabIndex = 1;
            // 
            // 运动坐标系comboBox
            // 
            this.运动坐标系comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.运动坐标系comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.运动坐标系comboBox.FormattingEnabled = true;
            this.运动坐标系comboBox.Location = new System.Drawing.Point(83, 8);
            this.运动坐标系comboBox.Name = "运动坐标系comboBox";
            this.运动坐标系comboBox.Size = new System.Drawing.Size(454, 20);
            this.运动坐标系comboBox.TabIndex = 4;
            this.运动坐标系comboBox.SelectionChangeCommitted += new System.EventHandler(this.运动坐标系comboBox_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "采集坐标系:";
            // 
            // 光源dataGridView1
            // 
            this.光源dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.光源dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LightControlColumn,
            this.LightChennelColumn,
            this.LightValue,
            this.SetLightBtn,
            this.OpenBtn,
            this.DeleteBtn,
            this.Column1});
            this.光源dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.光源dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.光源dataGridView1.Name = "光源dataGridView1";
            this.光源dataGridView1.RowHeadersWidth = 5;
            this.光源dataGridView1.RowTemplate.Height = 23;
            this.光源dataGridView1.Size = new System.Drawing.Size(528, 252);
            this.光源dataGridView1.TabIndex = 31;
            this.光源dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.光源dataGridView1_CellContentClick);
            this.光源dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.光源dataGridView1_DataError);
            // 
            // LightControlColumn
            // 
            this.LightControlColumn.DataPropertyName = "LightName";
            this.LightControlColumn.HeaderText = "光源控制器";
            this.LightControlColumn.Name = "LightControlColumn";
            // 
            // LightChennelColumn
            // 
            this.LightChennelColumn.DataPropertyName = "Channel";
            this.LightChennelColumn.HeaderText = "光源通道";
            this.LightChennelColumn.Name = "LightChennelColumn";
            this.LightChennelColumn.Width = 80;
            // 
            // LightValue
            // 
            this.LightValue.DataPropertyName = "LightValue";
            this.LightValue.HeaderText = "光源值";
            this.LightValue.Name = "LightValue";
            this.LightValue.Width = 80;
            // 
            // SetLightBtn
            // 
            this.SetLightBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "设置亮度";
            this.SetLightBtn.DefaultCellStyle = dataGridViewCellStyle4;
            this.SetLightBtn.HeaderText = "设置亮度";
            this.SetLightBtn.Name = "SetLightBtn";
            this.SetLightBtn.Width = 80;
            // 
            // OpenBtn
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "打开";
            this.OpenBtn.DefaultCellStyle = dataGridViewCellStyle5;
            this.OpenBtn.HeaderText = "打开";
            this.OpenBtn.Name = "OpenBtn";
            this.OpenBtn.Width = 80;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle6;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 50;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ChannelState";
            this.Column1.HeaderText = "状态";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 656);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "采集源配置";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 184F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 636);
            this.tableLayoutPanel1.TabIndex = 40;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.曝光参数comboBox);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.运动轴comboBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.运动坐标系comboBox);
            this.panel1.Controls.Add(this.传感器列表comboBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(540, 149);
            this.panel1.TabIndex = 0;
            // 
            // 曝光参数comboBox
            // 
            this.曝光参数comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.曝光参数comboBox.FormattingEnabled = true;
            this.曝光参数comboBox.Items.AddRange(new object[] {
            "-1",
            "1000",
            "3000",
            "5000",
            "10000",
            "30000",
            "50000"});
            this.曝光参数comboBox.Location = new System.Drawing.Point(83, 96);
            this.曝光参数comboBox.Name = "曝光参数comboBox";
            this.曝光参数comboBox.Size = new System.Drawing.Size(454, 20);
            this.曝光参数comboBox.TabIndex = 39;
            this.曝光参数comboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.曝光参数comboBox_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 38;
            this.label5.Text = "曝光参数:";
            // 
            // 运动轴comboBox
            // 
            this.运动轴comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.运动轴comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.运动轴comboBox.FormattingEnabled = true;
            this.运动轴comboBox.Location = new System.Drawing.Point(83, 36);
            this.运动轴comboBox.Name = "运动轴comboBox";
            this.运动轴comboBox.Size = new System.Drawing.Size(454, 20);
            this.运动轴comboBox.TabIndex = 37;
            this.运动轴comboBox.SelectionChangeCommitted += new System.EventHandler(this.运动轴comboBox_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 36;
            this.label4.Text = "运动轴:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.采集源listBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 479);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox2, 2);
            this.groupBox2.Size = new System.Drawing.Size(534, 104);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "采集源列表";
            // 
            // 采集源listBox
            // 
            this.采集源listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.采集源listBox.FormattingEnabled = true;
            this.采集源listBox.ItemHeight = 12;
            this.采集源listBox.Location = new System.Drawing.Point(3, 17);
            this.采集源listBox.Name = "采集源listBox";
            this.采集源listBox.Size = new System.Drawing.Size(528, 84);
            this.采集源listBox.TabIndex = 0;
            this.采集源listBox.SelectedIndexChanged += new System.EventHandler(this.采集源listBox_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.光源dataGridView1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 152);
            this.groupBox4.Name = "groupBox4";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox4, 2);
            this.groupBox4.Size = new System.Drawing.Size(534, 272);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "光源列表";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.保存button);
            this.panel3.Controls.Add(this.删除button);
            this.panel3.Controls.Add(this.AddSourceButton);
            this.panel3.Controls.Add(this.新建Button);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.采集源名称textBox);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 427);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(540, 49);
            this.panel3.TabIndex = 4;
            // 
            // 保存button
            // 
            this.保存button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.保存button.Location = new System.Drawing.Point(395, 5);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(70, 37);
            this.保存button.TabIndex = 41;
            this.保存button.Text = "保存采集源配置(Save)";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // 删除button
            // 
            this.删除button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.删除button.Location = new System.Drawing.Point(323, 5);
            this.删除button.Name = "删除button";
            this.删除button.Size = new System.Drawing.Size(70, 37);
            this.删除button.TabIndex = 40;
            this.删除button.Text = "删除采集源(De)";
            this.删除button.UseVisualStyleBackColor = true;
            this.删除button.Click += new System.EventHandler(this.删除button_Click);
            // 
            // AddSourceButton
            // 
            this.AddSourceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddSourceButton.Location = new System.Drawing.Point(468, 5);
            this.AddSourceButton.Name = "AddSourceButton";
            this.AddSourceButton.Size = new System.Drawing.Size(70, 37);
            this.AddSourceButton.TabIndex = 36;
            this.AddSourceButton.Text = "添加采集源(Add+)";
            this.AddSourceButton.UseVisualStyleBackColor = true;
            this.AddSourceButton.Click += new System.EventHandler(this.AddSourceButton_Click);
            // 
            // 新建Button
            // 
            this.新建Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.新建Button.Location = new System.Drawing.Point(251, 5);
            this.新建Button.Name = "新建Button";
            this.新建Button.Size = new System.Drawing.Size(70, 37);
            this.新建Button.TabIndex = 39;
            this.新建Button.Text = "新建采集源(New)";
            this.新建Button.UseVisualStyleBackColor = true;
            this.新建Button.Click += new System.EventHandler(this.新建Button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "采集源名称:";
            // 
            // 采集源名称textBox
            // 
            this.采集源名称textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.采集源名称textBox.Location = new System.Drawing.Point(72, 12);
            this.采集源名称textBox.Name = "采集源名称textBox";
            this.采集源名称textBox.Size = new System.Drawing.Size(178, 21);
            this.采集源名称textBox.TabIndex = 38;
            this.采集源名称textBox.Text = "采集源";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.设置采集源参数Btn);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 586);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(540, 50);
            this.panel4.TabIndex = 36;
            // 
            // 设置采集源参数Btn
            // 
            this.设置采集源参数Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.设置采集源参数Btn.Location = new System.Drawing.Point(423, 3);
            this.设置采集源参数Btn.Name = "设置采集源参数Btn";
            this.设置采集源参数Btn.Size = new System.Drawing.Size(113, 44);
            this.设置采集源参数Btn.TabIndex = 0;
            this.设置采集源参数Btn.Text = "设置采集源参数";
            this.设置采集源参数Btn.UseVisualStyleBackColor = true;
            this.设置采集源参数Btn.Click += new System.EventHandler(this.设置采集源参数Btn_Click);
            // 
            // AcqSourceConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 656);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AcqSourceConfigForm";
            this.Text = "采集源配置窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AcqSourceConfigForm_FormClosing);
            this.Load += new System.EventHandler(this.AcqSourceConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.光源dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 传感器列表comboBox;
        private System.Windows.Forms.ComboBox 运动坐标系comboBox;
        private System.Windows.Forms.Label label2;
        //private System.Windows.Forms.ListBox 传感器listBox;
        private System.Windows.Forms.DataGridView 光源dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox 采集源listBox;
        private System.Windows.Forms.Button AddSourceButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 采集源名称textBox;
        private System.Windows.Forms.Button 新建Button;
        private System.Windows.Forms.ComboBox 运动轴comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button 删除button;
        private System.Windows.Forms.Button 保存button;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button 设置采集源参数Btn;
        private System.Windows.Forms.ComboBox 曝光参数comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightControlColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightChennelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LightValue;
        private System.Windows.Forms.DataGridViewButtonColumn SetLightBtn;
        private System.Windows.Forms.DataGridViewButtonColumn OpenBtn;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
    }
}