namespace MotionControlCard
{
    partial class CoordSysConfigParamManageForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CardNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CoordSysName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.AxisName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.AxisAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisReadWrite = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DataTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvertAxisFeedBack = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InvertAxisCommandPos = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InvertJogAxis = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InsertBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CardNameColumn,
            this.CoordSysName,
            this.AxisName,
            this.AxisAddress,
            this.Column2,
            this.AxisReadWrite,
            this.DataTypeColumn,
            this.Column3,
            this.Column5,
            this.Column4,
            this.InvertAxisFeedBack,
            this.InvertAxisCommandPos,
            this.InvertJogAxis,
            this.Column1,
            this.InsertBtn,
            this.DeleteBtn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1462, 716);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(3, 775);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 44);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "保存(Save)";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1462, 50);
            this.label1.TabIndex = 6;
            this.label1.Text = "坐标系配置";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SaveButton, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1468, 822);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // CardNameColumn
            // 
            this.CardNameColumn.DataPropertyName = "CardName";
            this.CardNameColumn.HeaderText = "控制设备名称";
            this.CardNameColumn.Name = "CardNameColumn";
            // 
            // CoordSysName
            // 
            this.CoordSysName.DataPropertyName = "CoordSysName";
            this.CoordSysName.HeaderText = "坐标系名称";
            this.CoordSysName.Name = "CoordSysName";
            // 
            // AxisName
            // 
            this.AxisName.DataPropertyName = "AxisName";
            this.AxisName.HeaderText = "轴名称";
            this.AxisName.Name = "AxisName";
            // 
            // AxisAddress
            // 
            this.AxisAddress.DataPropertyName = "AxisAddress";
            this.AxisAddress.HeaderText = "轴地址";
            this.AxisAddress.Name = "AxisAddress";
            this.AxisAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AxisAddress.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "AxisLable";
            this.Column2.HeaderText = "轴标签(相机名)";
            this.Column2.Name = "Column2";
            this.Column2.Width = 120;
            // 
            // AxisReadWrite
            // 
            this.AxisReadWrite.DataPropertyName = "AxisReadWriteState";
            this.AxisReadWrite.HeaderText = "轴读写标志";
            this.AxisReadWrite.Name = "AxisReadWrite";
            // 
            // DataTypeColumn
            // 
            this.DataTypeColumn.DataPropertyName = "DataType";
            this.DataTypeColumn.HeaderText = "数据类型";
            this.DataTypeColumn.Name = "DataTypeColumn";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "DataLength";
            this.Column3.HeaderText = "数据长度";
            this.Column3.Name = "Column3";
            this.Column3.Width = 80;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "AdressPrefix";
            this.Column5.HeaderText = "地址前缀";
            this.Column5.Name = "Column5";
            this.Column5.Width = 80;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "PulseEquiv";
            this.Column4.HeaderText = "脉冲当量";
            this.Column4.Name = "Column4";
            this.Column4.Width = 80;
            // 
            // InvertAxisFeedBack
            // 
            this.InvertAxisFeedBack.DataPropertyName = "InvertAxisFeedBack";
            this.InvertAxisFeedBack.HeaderText = "取反轴反馈";
            this.InvertAxisFeedBack.Name = "InvertAxisFeedBack";
            this.InvertAxisFeedBack.Width = 80;
            // 
            // InvertAxisCommandPos
            // 
            this.InvertAxisCommandPos.DataPropertyName = "InvertAxisCommandPos";
            this.InvertAxisCommandPos.HeaderText = "取反轴指令";
            this.InvertAxisCommandPos.Name = "InvertAxisCommandPos";
            this.InvertAxisCommandPos.Width = 80;
            // 
            // InvertJogAxis
            // 
            this.InvertJogAxis.DataPropertyName = "InvertJogAxis";
            this.InvertJogAxis.HeaderText = "取反轴Jog";
            this.InvertJogAxis.Name = "InvertJogAxis";
            this.InvertJogAxis.Width = 80;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "TransmissionRatio";
            this.Column1.HeaderText = "轴传动比";
            this.Column1.Name = "Column1";
            this.Column1.Width = 80;
            // 
            // InsertBtn
            // 
            this.InsertBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "插入";
            this.InsertBtn.DefaultCellStyle = dataGridViewCellStyle3;
            this.InsertBtn.HeaderText = "插入";
            this.InsertBtn.Name = "InsertBtn";
            this.InsertBtn.Width = 80;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle4;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Text = "";
            this.DeleteBtn.Width = 80;
            // 
            // CoordSysConfigParamManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1468, 822);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordSysConfigParamManageForm";
            this.Text = "坐标系配置管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorManageForm_FormClosing);
            this.Load += new System.EventHandler(this.DeviceConfigParamManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewComboBoxColumn CardNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn CoordSysName;
        private System.Windows.Forms.DataGridViewComboBoxColumn AxisName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewComboBoxColumn AxisReadWrite;
        private System.Windows.Forms.DataGridViewComboBoxColumn DataTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn InvertAxisFeedBack;
        private System.Windows.Forms.DataGridViewCheckBoxColumn InvertAxisCommandPos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn InvertJogAxis;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn InsertBtn;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
    }
}