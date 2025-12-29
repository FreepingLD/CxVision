
namespace FunctionBlock
{
    partial class MoveTrackForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ActiveCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CoordCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.AxisCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MoveCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TrackCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FunctionCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeachCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.AddCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.加减速Col = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeletCol = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
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
            this.FunctionCol,
            this.TeachCol,
            this.AddCol,
            this.加减速Col,
            this.DeletCol});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 60;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(852, 629);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
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
            this.CoordCol.Frozen = true;
            this.CoordCol.HeaderText = "坐标系";
            this.CoordCol.Name = "CoordCol";
            // 
            // AxisCol
            // 
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
            // FunctionCol
            // 
            this.FunctionCol.DataPropertyName = "Function";
            this.FunctionCol.Frozen = true;
            this.FunctionCol.HeaderText = "功能参数";
            this.FunctionCol.Name = "FunctionCol";
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
            // MoveTrackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 629);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MoveTrackForm";
            this.Text = "运动轨迹设置";
            this.Load += new System.EventHandler(this.MoveTrackForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ActiveCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn CoordCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn AxisCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn MoveCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrackCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn FunctionCol;
        private System.Windows.Forms.DataGridViewButtonColumn TeachCol;
        private System.Windows.Forms.DataGridViewButtonColumn AddCol;
        private System.Windows.Forms.DataGridViewButtonColumn 加减速Col;
        private System.Windows.Forms.DataGridViewButtonColumn DeletCol;
    }
}