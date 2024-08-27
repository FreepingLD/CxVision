namespace FunctionBlock
{
    partial class WriteDataForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WriteDataForm));
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.数据写入dataGridView = new System.Windows.Forms.DataGridView();
            this.IsActiveCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CooreSysNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CommunicationCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.UpMoveCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DownMoveCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.InsertBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.数据写入dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AccessibleName = "新项选择";
            this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(762, 28);
            this.miniToolStrip.TabIndex = 13;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(755, 356);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip1, 2);
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 329);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(755, 27);
            this.statusStrip1.TabIndex = 43;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 22);
            this.toolStripStatusLabel1.Text = "执行结果：";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(28, 22);
            this.toolStripStatusLabel2.Text = "……";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 2);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(755, 28);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources.Start;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(56, 25);
            this.toolStripButton_Run.Text = "执行";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.数据写入dataGridView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 31);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 3);
            this.panel1.Size = new System.Drawing.Size(749, 295);
            this.panel1.TabIndex = 44;
            // 
            // 数据写入dataGridView
            // 
            this.数据写入dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.数据写入dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsActiveCol,
            this.CooreSysNameColumn,
            this.CommunicationCol,
            this.dataGridViewTextBoxColumn3,
            this.WriteBtn,
            this.UpMoveCol,
            this.DownMoveCol,
            this.InsertBtn,
            this.DeleteBtn});
            this.数据写入dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.数据写入dataGridView.Location = new System.Drawing.Point(0, 0);
            this.数据写入dataGridView.Name = "数据写入dataGridView";
            this.数据写入dataGridView.RowHeadersWidth = 5;
            this.数据写入dataGridView.RowTemplate.Height = 23;
            this.数据写入dataGridView.Size = new System.Drawing.Size(749, 295);
            this.数据写入dataGridView.TabIndex = 1;
            this.数据写入dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.数据写入dataGridView_CellContentClick);
            this.数据写入dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.数据写入dataGridView_DataError);
            // 
            // IsActiveCol
            // 
            this.IsActiveCol.DataPropertyName = "IsActive";
            this.IsActiveCol.HeaderText = "激活";
            this.IsActiveCol.Name = "IsActiveCol";
            this.IsActiveCol.Width = 40;
            // 
            // CooreSysNameColumn
            // 
            this.CooreSysNameColumn.DataPropertyName = "CoordSysName";
            this.CooreSysNameColumn.HeaderText = "坐标系名";
            this.CooreSysNameColumn.MinimumWidth = 6;
            this.CooreSysNameColumn.Name = "CooreSysNameColumn";
            this.CooreSysNameColumn.Width = 125;
            // 
            // CommunicationCol
            // 
            this.CommunicationCol.DataPropertyName = "CommunicationCommand";
            this.CommunicationCol.HeaderText = "通信命令";
            this.CommunicationCol.MinimumWidth = 6;
            this.CommunicationCol.Name = "CommunicationCol";
            this.CommunicationCol.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "WriteValue";
            this.dataGridViewTextBoxColumn3.HeaderText = "写入值";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // WriteBtn
            // 
            this.WriteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = "写入";
            this.WriteBtn.DefaultCellStyle = dataGridViewCellStyle6;
            this.WriteBtn.HeaderText = "写入";
            this.WriteBtn.MinimumWidth = 6;
            this.WriteBtn.Name = "WriteBtn";
            this.WriteBtn.Width = 60;
            // 
            // UpMoveCol
            // 
            this.UpMoveCol.DataPropertyName = "NONE";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = "上移";
            this.UpMoveCol.DefaultCellStyle = dataGridViewCellStyle7;
            this.UpMoveCol.HeaderText = "上移";
            this.UpMoveCol.Name = "UpMoveCol";
            this.UpMoveCol.Width = 60;
            // 
            // DownMoveCol
            // 
            this.DownMoveCol.DataPropertyName = "NONE";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "下移";
            this.DownMoveCol.DefaultCellStyle = dataGridViewCellStyle8;
            this.DownMoveCol.HeaderText = "下移";
            this.DownMoveCol.Name = "DownMoveCol";
            this.DownMoveCol.Width = 60;
            // 
            // InsertBtn
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.NullValue = "插入";
            this.InsertBtn.DefaultCellStyle = dataGridViewCellStyle9;
            this.InsertBtn.HeaderText = "插入";
            this.InsertBtn.MinimumWidth = 6;
            this.InsertBtn.Name = "InsertBtn";
            this.InsertBtn.Width = 60;
            // 
            // DeleteBtn
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle10;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.MinimumWidth = 6;
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 60;
            // 
            // WriteDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(755, 356);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WriteDataForm";
            this.Text = "写入数据PLC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WritePlcDataForm_FormClosing);
            this.Load += new System.EventHandler(this.WritePlcDataForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.数据写入dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView 数据写入dataGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActiveCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn CooreSysNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn CommunicationCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewButtonColumn WriteBtn;
        private System.Windows.Forms.DataGridViewButtonColumn UpMoveCol;
        private System.Windows.Forms.DataGridViewButtonColumn DownMoveCol;
        private System.Windows.Forms.DataGridViewButtonColumn InsertBtn;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
    }
}