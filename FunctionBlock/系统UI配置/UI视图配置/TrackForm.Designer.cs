namespace FunctionBlock
{
    partial class TrackForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ActiveCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MoveCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TrackCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeachCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeletCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.操作员comboBox = new System.Windows.Forms.ComboBox();
            this.加载点位button = new System.Windows.Forms.Button();
            this.更新点位button = new System.Windows.Forms.Button();
            this.采集图片Btn = new System.Windows.Forms.Button();
            this.LocadImageButton = new System.Windows.Forms.Button();
            this.清空button = new System.Windows.Forms.Button();
            this.删除button = new System.Windows.Forms.Button();
            this.插入button = new System.Windows.Forms.Button();
            this.添加线button = new System.Windows.Forms.Button();
            this.添加点button = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 5);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(355, 16);
            this.titleLabel.TabIndex = 22;
            this.titleLabel.Text = "CxVision";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 192F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 208F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 373F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.buttonMax, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonClose, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMin, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(440, 745);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActiveCol,
            this.MoveCol,
            this.TrackCol,
            this.TeachCol,
            this.DeletCol});
            this.tableLayoutPanel2.SetColumnSpan(this.dataGridView1, 8);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 23);
            this.dataGridView1.MaximumSize = new System.Drawing.Size(430, 590);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 60;
            this.tableLayoutPanel2.SetRowSpan(this.dataGridView1, 3);
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(430, 590);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            this.dataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            // 
            // ActiveCol
            // 
            this.ActiveCol.DataPropertyName = "IsActive";
            this.ActiveCol.HeaderText = "激活";
            this.ActiveCol.Name = "ActiveCol";
            this.ActiveCol.Width = 50;
            // 
            // MoveCol
            // 
            this.MoveCol.DataPropertyName = "MoveType";
            this.MoveCol.HeaderText = "轨迹类型";
            this.MoveCol.Name = "MoveCol";
            // 
            // TrackCol
            // 
            this.TrackCol.DataPropertyName = "RoiShape";
            this.TrackCol.HeaderText = "轨迹参数";
            this.TrackCol.Name = "TrackCol";
            // 
            // TeachCol
            // 
            this.TeachCol.DataPropertyName = "NONE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "示教";
            this.TeachCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.TeachCol.HeaderText = "示教";
            this.TeachCol.Name = "TeachCol";
            this.TeachCol.Width = 60;
            // 
            // DeletCol
            // 
            this.DeletCol.DataPropertyName = "NONE";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "删除";
            this.DeletCol.DefaultCellStyle = dataGridViewCellStyle4;
            this.DeletCol.HeaderText = "删除";
            this.DeletCol.Name = "DeletCol";
            this.DeletCol.Width = 60;
            // 
            // panel3
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel3, 8);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.操作员comboBox);
            this.panel3.Controls.Add(this.加载点位button);
            this.panel3.Controls.Add(this.更新点位button);
            this.panel3.Controls.Add(this.采集图片Btn);
            this.panel3.Controls.Add(this.LocadImageButton);
            this.panel3.Controls.Add(this.清空button);
            this.panel3.Controls.Add(this.删除button);
            this.panel3.Controls.Add(this.插入button);
            this.panel3.Controls.Add(this.添加线button);
            this.panel3.Controls.Add(this.添加点button);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 620);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(434, 122);
            this.panel3.TabIndex = 27;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 82);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // 操作员comboBox
            // 
            this.操作员comboBox.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.操作员comboBox.FormattingEnabled = true;
            this.操作员comboBox.Location = new System.Drawing.Point(543, 505);
            this.操作员comboBox.Name = "操作员comboBox";
            this.操作员comboBox.Size = new System.Drawing.Size(94, 27);
            this.操作员comboBox.TabIndex = 27;
            // 
            // 加载点位button
            // 
            this.加载点位button.Location = new System.Drawing.Point(182, 8);
            this.加载点位button.Name = "加载点位button";
            this.加载点位button.Size = new System.Drawing.Size(75, 32);
            this.加载点位button.TabIndex = 22;
            this.加载点位button.Text = "加载点位(Updata)";
            this.加载点位button.UseVisualStyleBackColor = true;
            this.加载点位button.Click += new System.EventHandler(this.加载点位button_Click);
            // 
            // 更新点位button
            // 
            this.更新点位button.Location = new System.Drawing.Point(182, 46);
            this.更新点位button.Name = "更新点位button";
            this.更新点位button.Size = new System.Drawing.Size(75, 32);
            this.更新点位button.TabIndex = 21;
            this.更新点位button.Text = "更新点位(Updata)";
            this.更新点位button.UseVisualStyleBackColor = true;
            this.更新点位button.Click += new System.EventHandler(this.更新点位button_Click);
            // 
            // 采集图片Btn
            // 
            this.采集图片Btn.Location = new System.Drawing.Point(101, 46);
            this.采集图片Btn.Name = "采集图片Btn";
            this.采集图片Btn.Size = new System.Drawing.Size(75, 32);
            this.采集图片Btn.TabIndex = 20;
            this.采集图片Btn.Text = "采集图片(Acq)";
            this.采集图片Btn.UseVisualStyleBackColor = true;
            // 
            // LocadImageButton
            // 
            this.LocadImageButton.Location = new System.Drawing.Point(101, 8);
            this.LocadImageButton.Name = "LocadImageButton";
            this.LocadImageButton.Size = new System.Drawing.Size(75, 32);
            this.LocadImageButton.TabIndex = 17;
            this.LocadImageButton.Text = "加载图片(Load)";
            this.LocadImageButton.UseVisualStyleBackColor = true;
            this.LocadImageButton.Click += new System.EventHandler(this.LocadImageButton_Click);
            // 
            // 清空button
            // 
            this.清空button.Location = new System.Drawing.Point(263, 46);
            this.清空button.Name = "清空button";
            this.清空button.Size = new System.Drawing.Size(75, 32);
            this.清空button.TabIndex = 5;
            this.清空button.Text = "清空(Clear)";
            this.清空button.UseVisualStyleBackColor = true;
            this.清空button.Click += new System.EventHandler(this.清空button_Click);
            // 
            // 删除button
            // 
            this.删除button.Location = new System.Drawing.Point(263, 8);
            this.删除button.Name = "删除button";
            this.删除button.Size = new System.Drawing.Size(75, 32);
            this.删除button.TabIndex = 3;
            this.删除button.Text = "删除(Delete)";
            this.删除button.UseVisualStyleBackColor = true;
            this.删除button.Click += new System.EventHandler(this.删除button_Click);
            // 
            // 插入button
            // 
            this.插入button.Location = new System.Drawing.Point(353, 82);
            this.插入button.Name = "插入button";
            this.插入button.Size = new System.Drawing.Size(75, 32);
            this.插入button.TabIndex = 2;
            this.插入button.Text = "插入(Inseter)";
            this.插入button.UseVisualStyleBackColor = true;
            this.插入button.Click += new System.EventHandler(this.插入button_Click);
            // 
            // 添加线button
            // 
            this.添加线button.Location = new System.Drawing.Point(353, 46);
            this.添加线button.Name = "添加线button";
            this.添加线button.Size = new System.Drawing.Size(75, 32);
            this.添加线button.TabIndex = 1;
            this.添加线button.Text = "添加线(Add)";
            this.添加线button.UseVisualStyleBackColor = true;
            this.添加线button.Click += new System.EventHandler(this.添加线button_Click);
            // 
            // 添加点button
            // 
            this.添加点button.Location = new System.Drawing.Point(353, 8);
            this.添加点button.Name = "添加点button";
            this.添加点button.Size = new System.Drawing.Size(75, 32);
            this.添加点button.TabIndex = 0;
            this.添加点button.Text = "添加点(Add)";
            this.添加点button.UseVisualStyleBackColor = true;
            this.添加点button.Click += new System.EventHandler(this.添加点button_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(386, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(27, 20);
            this.buttonMax.TabIndex = 1;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(413, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(27, 20);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Enabled = false;
            this.buttonMin.Location = new System.Drawing.Point(359, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(27, 20);
            this.buttonMin.TabIndex = 2;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // TrackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(437, 745);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TrackForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThicknessViewForm_FormClosing);
            this.Load += new System.EventHandler(this.TrackForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ThicknessViewForm_MouseDown);
            this.Move += new System.EventHandler(this.ThicknessViewForm_Move);
            this.Resize += new System.EventHandler(this.ThicknessViewForm_Resize);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button 清空button;
        private System.Windows.Forms.Button 删除button;
        private System.Windows.Forms.Button 插入button;
        private System.Windows.Forms.Button 添加线button;
        private System.Windows.Forms.Button 添加点button;
        private System.Windows.Forms.Button LocadImageButton;
        private System.Windows.Forms.Button 采集图片Btn;
        private System.Windows.Forms.Button 更新点位button;
        private System.Windows.Forms.Button 加载点位button;
        private System.Windows.Forms.ComboBox 操作员comboBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ActiveCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn MoveCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrackCol;
        private System.Windows.Forms.DataGridViewButtonColumn TeachCol;
        private System.Windows.Forms.DataGridViewButtonColumn DeletCol;
        private System.Windows.Forms.Button button1;
    }
}