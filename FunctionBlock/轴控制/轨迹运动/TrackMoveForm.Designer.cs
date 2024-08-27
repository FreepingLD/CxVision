namespace FunctionBlock
{
    partial class TrackMoveForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.ActiveCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CoordCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.AxisCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MoveCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TrackCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TacDecCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IoCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SynCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TeachCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.AddCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.加减速Col = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeletCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 313F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1010, 651);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActiveCol,
            this.CoordCol,
            this.AxisCol,
            this.MoveCol,
            this.TrackCol,
            this.TacDecCol,
            this.IoCol,
            this.SynCol,
            this.TeachCol,
            this.AddCol,
            this.加减速Col,
            this.DeletCol});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 2);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataGridView1.RowHeadersWidth = 60;
            this.tableLayoutPanel1.SetRowSpan(this.dataGridView1, 4);
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1004, 587);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // statusStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip1, 2);
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 621);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1010, 30);
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
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 2);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Run});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1010, 28);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.Image = global::FunctionBlock.Properties.Resources.Start;
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(52, 25);
            this.toolStripButton_Run.Text = "执行";
            // 
            // ActiveCol
            // 
            this.ActiveCol.DataPropertyName = "IsActive";
            this.ActiveCol.Frozen = true;
            this.ActiveCol.HeaderText = "激活";
            this.ActiveCol.Name = "ActiveCol";
            this.ActiveCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ActiveCol.Width = 50;
            // 
            // CoordCol
            // 
            this.CoordCol.DataPropertyName = "CoordSysName";
            this.CoordCol.Frozen = true;
            this.CoordCol.HeaderText = "坐标系";
            this.CoordCol.Name = "CoordCol";
            // 
            // AxisCol
            // 
            this.AxisCol.DataPropertyName = "MoveAxis";
            this.AxisCol.Frozen = true;
            this.AxisCol.HeaderText = "移动轴";
            this.AxisCol.Name = "AxisCol";
            // 
            // MoveCol
            // 
            this.MoveCol.DataPropertyName = "MoveType";
            this.MoveCol.Frozen = true;
            this.MoveCol.HeaderText = "轨迹类型";
            this.MoveCol.Name = "MoveCol";
            // 
            // TrackCol
            // 
            this.TrackCol.DataPropertyName = "RoiShape";
            this.TrackCol.Frozen = true;
            this.TrackCol.HeaderText = "轨迹参数";
            this.TrackCol.Name = "TrackCol";
            this.TrackCol.ReadOnly = true;
            // 
            // TacDecCol
            // 
            this.TacDecCol.DataPropertyName = "AccDecParam";
            this.TacDecCol.Frozen = true;
            this.TacDecCol.HeaderText = "加减速参数";
            this.TacDecCol.Name = "TacDecCol";
            // 
            // IoCol
            // 
            this.IoCol.DataPropertyName = "IoOutPort";
            this.IoCol.Frozen = true;
            this.IoCol.HeaderText = "IO输出";
            this.IoCol.Name = "IoCol";
            // 
            // SynCol
            // 
            this.SynCol.DataPropertyName = "IsWait";
            this.SynCol.Frozen = true;
            this.SynCol.HeaderText = "同步";
            this.SynCol.Name = "SynCol";
            this.SynCol.Width = 50;
            // 
            // TeachCol
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "示教";
            this.TeachCol.DefaultCellStyle = dataGridViewCellStyle1;
            this.TeachCol.Frozen = true;
            this.TeachCol.HeaderText = "示教";
            this.TeachCol.Name = "TeachCol";
            this.TeachCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TeachCol.Width = 60;
            // 
            // AddCol
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "添加";
            this.AddCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.AddCol.Frozen = true;
            this.AddCol.HeaderText = "添加";
            this.AddCol.Name = "AddCol";
            this.AddCol.ReadOnly = true;
            this.AddCol.Width = 60;
            // 
            // 加减速Col
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "加减速";
            this.加减速Col.DefaultCellStyle = dataGridViewCellStyle3;
            this.加减速Col.Frozen = true;
            this.加减速Col.HeaderText = "加减速";
            this.加减速Col.Name = "加减速Col";
            this.加减速Col.Width = 60;
            // 
            // DeletCol
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "删除";
            this.DeletCol.DefaultCellStyle = dataGridViewCellStyle4;
            this.DeletCol.Frozen = true;
            this.DeletCol.HeaderText = "删除";
            this.DeletCol.Name = "DeletCol";
            this.DeletCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DeletCol.Width = 60;
            // 
            // TrackMoveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1010, 651);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TrackMoveForm";
            this.Text = "轨迹配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointMoveForm_FormClosing);
            this.Load += new System.EventHandler(this.PointMoveForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ActiveCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn CoordCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn AxisCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn MoveCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrackCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TacDecCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn IoCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SynCol;
        private System.Windows.Forms.DataGridViewButtonColumn TeachCol;
        private System.Windows.Forms.DataGridViewButtonColumn AddCol;
        private System.Windows.Forms.DataGridViewButtonColumn 加减速Col;
        private System.Windows.Forms.DataGridViewButtonColumn DeletCol;
    }
}