
namespace VisionBase
{
    partial class UsrInspParaTeach
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.InspDgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InspModelDgvCbx = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeachDgvBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.StartTeachBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.ParaResetBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.UsrInspPanel = new System.Windows.Forms.Panel();
            this.InspEnumCbx = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InspDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(696, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "2.检测参数分组示教";
            // 
            // InspDgv
            // 
            this.InspDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InspDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.InspModelDgvCbx,
            this.Column4,
            this.Column3,
            this.TeachDgvBtn});
            this.InspDgv.Location = new System.Drawing.Point(13, 62);
            this.InspDgv.Name = "InspDgv";
            this.InspDgv.RowTemplate.Height = 23;
            this.InspDgv.Size = new System.Drawing.Size(359, 182);
            this.InspDgv.TabIndex = 1;
            this.InspDgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InspDgv_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "编号";
            this.Column1.Name = "Column1";
            this.Column1.Width = 45;
            // 
            // InspModelDgvCbx
            // 
            this.InspModelDgvCbx.HeaderText = "检测类型";
            this.InspModelDgvCbx.Name = "InspModelDgvCbx";
            this.InspModelDgvCbx.ReadOnly = true;
            this.InspModelDgvCbx.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InspModelDgvCbx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.InspModelDgvCbx.Width = 80;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "首索引";
            this.Column4.Name = "Column4";
            this.Column4.Width = 70;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "数量";
            this.Column3.Name = "Column3";
            this.Column3.Width = 60;
            // 
            // TeachDgvBtn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "示教";
            this.TeachDgvBtn.DefaultCellStyle = dataGridViewCellStyle1;
            this.TeachDgvBtn.HeaderText = "示教";
            this.TeachDgvBtn.Name = "TeachDgvBtn";
            this.TeachDgvBtn.Width = 60;
            // 
            // StartTeachBtn
            // 
            this.StartTeachBtn.Location = new System.Drawing.Point(378, 100);
            this.StartTeachBtn.Name = "StartTeachBtn";
            this.StartTeachBtn.Size = new System.Drawing.Size(75, 42);
            this.StartTeachBtn.TabIndex = 2;
            this.StartTeachBtn.Text = "参数示教";
            this.StartTeachBtn.UseVisualStyleBackColor = true;
            this.StartTeachBtn.Click += new System.EventHandler(this.StartTeachBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Enabled = false;
            this.DeleteBtn.Location = new System.Drawing.Point(461, 100);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(75, 42);
            this.DeleteBtn.TabIndex = 2;
            this.DeleteBtn.Text = "删除";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            this.DeleteBtn.MouseEnter += new System.EventHandler(this.DeleteBtn_MouseEnter);
            // 
            // AddBtn
            // 
            this.AddBtn.Enabled = false;
            this.AddBtn.Location = new System.Drawing.Point(377, 147);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 42);
            this.AddBtn.TabIndex = 2;
            this.AddBtn.Text = "新增";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            this.AddBtn.MouseEnter += new System.EventHandler(this.AddBtn_MouseEnter);
            // 
            // ParaResetBtn
            // 
            this.ParaResetBtn.Enabled = false;
            this.ParaResetBtn.Location = new System.Drawing.Point(460, 147);
            this.ParaResetBtn.Name = "ParaResetBtn";
            this.ParaResetBtn.Size = new System.Drawing.Size(75, 42);
            this.ParaResetBtn.TabIndex = 2;
            this.ParaResetBtn.Text = "重置参数";
            this.ParaResetBtn.UseVisualStyleBackColor = true;
            this.ParaResetBtn.Click += new System.EventHandler(this.ParaResetBtn_Click);
            this.ParaResetBtn.MouseEnter += new System.EventHandler(this.ParaResetBtn_MouseEnter);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Enabled = false;
            this.SaveBtn.Location = new System.Drawing.Point(462, 196);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 42);
            this.SaveBtn.TabIndex = 2;
            this.SaveBtn.Text = "保存";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            this.SaveBtn.MouseEnter += new System.EventHandler(this.SaveBtn_MouseEnter);
            // 
            // UsrInspPanel
            // 
            this.UsrInspPanel.Enabled = false;
            this.UsrInspPanel.Location = new System.Drawing.Point(560, 60);
            this.UsrInspPanel.Name = "UsrInspPanel";
            this.UsrInspPanel.Size = new System.Drawing.Size(1100, 180);
            this.UsrInspPanel.TabIndex = 3;
            // 
            // InspEnumCbx
            // 
            this.InspEnumCbx.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InspEnumCbx.FormattingEnabled = true;
            this.InspEnumCbx.Location = new System.Drawing.Point(425, 62);
            this.InspEnumCbx.Name = "InspEnumCbx";
            this.InspEnumCbx.Size = new System.Drawing.Size(109, 24);
            this.InspEnumCbx.TabIndex = 4;
            this.InspEnumCbx.SelectedIndexChanged += new System.EventHandler(this.InspEnumCbx_SelectedIndexChanged);
            this.InspEnumCbx.MouseEnter += new System.EventHandler(this.InspEnumCbx_MouseEnter);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.Location = new System.Drawing.Point(543, 52);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 199);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DimGray;
            this.button2.Location = new System.Drawing.Point(3, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(1663, 10);
            this.button2.TabIndex = 6;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // UsrInspParaTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.InspEnumCbx);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.ParaResetBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.StartTeachBtn);
            this.Controls.Add(this.InspDgv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UsrInspPanel);
            this.Name = "UsrInspParaTeach";
            this.Size = new System.Drawing.Size(1671, 250);
            this.Load += new System.EventHandler(this.UsrInspParaTeach_Load);
            ((System.ComponentModel.ISupportInitialize)(this.InspDgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView InspDgv;
        private System.Windows.Forms.Button StartTeachBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button ParaResetBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Panel UsrInspPanel;
        private System.Windows.Forms.ComboBox InspEnumCbx;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn InspModelDgvCbx;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewButtonColumn TeachDgvBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
