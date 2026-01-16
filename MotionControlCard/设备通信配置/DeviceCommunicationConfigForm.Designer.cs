namespace MotionControlCard
{
    partial class DeviceCommunicationConfigForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.SaveButton = new System.Windows.Forms.Button();
            this.通信命令配置label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ActiveCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CooreSysNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MapCooreSysNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CommuniteColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DescribeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AxisReadWrite = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DataTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReadBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.InsertBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.清空配置Btn = new System.Windows.Forms.Button();
            this.实时刷新checkBox = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.通信配置15Btn = new System.Windows.Forms.Button();
            this.通信配置14Btn = new System.Windows.Forms.Button();
            this.通信配置13Btn = new System.Windows.Forms.Button();
            this.通信配置12Btn = new System.Windows.Forms.Button();
            this.通信配置11Btn = new System.Windows.Forms.Button();
            this.通信配置10Btn = new System.Windows.Forms.Button();
            this.通信配置9Btn = new System.Windows.Forms.Button();
            this.通信配置8Btn = new System.Windows.Forms.Button();
            this.通信配置7Btn = new System.Windows.Forms.Button();
            this.通信配置6Btn = new System.Windows.Forms.Button();
            this.通信配置5Btn = new System.Windows.Forms.Button();
            this.通信配置4Btn = new System.Windows.Forms.Button();
            this.通信配置3Btn = new System.Windows.Forms.Button();
            this.通信配置2Btn = new System.Windows.Forms.Button();
            this.通信配置1Btn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(1, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(85, 40);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "保存(Save)";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click_1);
            // 
            // 通信命令配置label
            // 
            this.通信命令配置label.AutoSize = true;
            this.通信命令配置label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.通信命令配置label.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.通信命令配置label.Location = new System.Drawing.Point(3, 0);
            this.通信命令配置label.Name = "通信命令配置label";
            this.通信命令配置label.Size = new System.Drawing.Size(1538, 53);
            this.通信命令配置label.TabIndex = 6;
            this.通信命令配置label.Text = "通信命令配置";
            this.通信命令配置label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.通信命令配置label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1544, 822);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActiveCol,
            this.CooreSysNameColumn,
            this.MapCooreSysNameColumn,
            this.CommuniteColumn,
            this.DescribeCol,
            this.AxisAddress,
            this.AxisReadWrite,
            this.DataTypeColumn,
            this.Column3,
            this.Column2,
            this.Column1,
            this.ReadBtn,
            this.Column5,
            this.WriteBtn,
            this.DeleteBtn,
            this.InsertBtn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 56);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1538, 683);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // ActiveCol
            // 
            this.ActiveCol.DataPropertyName = "Active";
            this.ActiveCol.HeaderText = "激活";
            this.ActiveCol.Name = "ActiveCol";
            this.ActiveCol.Width = 60;
            // 
            // CooreSysNameColumn
            // 
            this.CooreSysNameColumn.DataPropertyName = "CoordSysName";
            this.CooreSysNameColumn.HeaderText = "坐标系名称";
            this.CooreSysNameColumn.Name = "CooreSysNameColumn";
            // 
            // MapCooreSysNameColumn
            // 
            this.MapCooreSysNameColumn.DataPropertyName = "MapCoordSysName";
            this.MapCooreSysNameColumn.HeaderText = "映射坐标系";
            this.MapCooreSysNameColumn.Name = "MapCooreSysNameColumn";
            // 
            // CommuniteColumn
            // 
            this.CommuniteColumn.DataPropertyName = "CommunicationCommand";
            this.CommuniteColumn.HeaderText = "通信指令";
            this.CommuniteColumn.Name = "CommuniteColumn";
            // 
            // DescribeCol
            // 
            this.DescribeCol.DataPropertyName = "Description";
            this.DescribeCol.HeaderText = "指令描述";
            this.DescribeCol.Name = "DescribeCol";
            this.DescribeCol.Width = 150;
            // 
            // AxisAddress
            // 
            this.AxisAddress.DataPropertyName = "Address";
            this.AxisAddress.HeaderText = "地址";
            this.AxisAddress.Name = "AxisAddress";
            this.AxisAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AxisAddress.Width = 80;
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
            // Column2
            // 
            this.Column2.DataPropertyName = "DataScale";
            this.Column2.HeaderText = "数据缩放";
            this.Column2.Name = "Column2";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ReadValue";
            this.Column1.HeaderText = "读取值";
            this.Column1.Name = "Column1";
            // 
            // ReadBtn
            // 
            this.ReadBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "读取";
            this.ReadBtn.DefaultCellStyle = dataGridViewCellStyle5;
            this.ReadBtn.HeaderText = "读取";
            this.ReadBtn.Name = "ReadBtn";
            this.ReadBtn.Width = 80;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "WriteValue";
            this.Column5.HeaderText = "写入值";
            this.Column5.Name = "Column5";
            // 
            // WriteBtn
            // 
            this.WriteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = "写入";
            this.WriteBtn.DefaultCellStyle = dataGridViewCellStyle6;
            this.WriteBtn.HeaderText = "写入";
            this.WriteBtn.Name = "WriteBtn";
            this.WriteBtn.Width = 80;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle7;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 80;
            // 
            // InsertBtn
            // 
            this.InsertBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "插入";
            this.InsertBtn.DefaultCellStyle = dataGridViewCellStyle8;
            this.InsertBtn.HeaderText = "插入";
            this.InsertBtn.Name = "InsertBtn";
            this.InsertBtn.Width = 80;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.清空配置Btn);
            this.panel1.Controls.Add(this.实时刷新checkBox);
            this.panel1.Controls.Add(this.SaveButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 775);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1538, 44);
            this.panel1.TabIndex = 7;
            // 
            // 清空配置Btn
            // 
            this.清空配置Btn.Location = new System.Drawing.Point(219, 4);
            this.清空配置Btn.Name = "清空配置Btn";
            this.清空配置Btn.Size = new System.Drawing.Size(85, 40);
            this.清空配置Btn.TabIndex = 7;
            this.清空配置Btn.Text = "清空配置(Clear)";
            this.清空配置Btn.UseVisualStyleBackColor = true;
            this.清空配置Btn.Click += new System.EventHandler(this.清空配置Btn_Click);
            // 
            // 实时刷新checkBox
            // 
            this.实时刷新checkBox.AutoSize = true;
            this.实时刷新checkBox.Location = new System.Drawing.Point(116, 28);
            this.实时刷新checkBox.Name = "实时刷新checkBox";
            this.实时刷新checkBox.Size = new System.Drawing.Size(72, 16);
            this.实时刷新checkBox.TabIndex = 6;
            this.实时刷新checkBox.Text = "实时刷新";
            this.实时刷新checkBox.UseVisualStyleBackColor = true;
            this.实时刷新checkBox.CheckedChanged += new System.EventHandler(this.实时刷新checkBox_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.通信配置15Btn);
            this.panel2.Controls.Add(this.通信配置14Btn);
            this.panel2.Controls.Add(this.通信配置13Btn);
            this.panel2.Controls.Add(this.通信配置12Btn);
            this.panel2.Controls.Add(this.通信配置11Btn);
            this.panel2.Controls.Add(this.通信配置10Btn);
            this.panel2.Controls.Add(this.通信配置9Btn);
            this.panel2.Controls.Add(this.通信配置8Btn);
            this.panel2.Controls.Add(this.通信配置7Btn);
            this.panel2.Controls.Add(this.通信配置6Btn);
            this.panel2.Controls.Add(this.通信配置5Btn);
            this.panel2.Controls.Add(this.通信配置4Btn);
            this.panel2.Controls.Add(this.通信配置3Btn);
            this.panel2.Controls.Add(this.通信配置2Btn);
            this.panel2.Controls.Add(this.通信配置1Btn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 742);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1544, 30);
            this.panel2.TabIndex = 8;
            // 
            // 通信配置15Btn
            // 
            this.通信配置15Btn.Location = new System.Drawing.Point(1137, 4);
            this.通信配置15Btn.Name = "通信配置15Btn";
            this.通信配置15Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置15Btn.TabIndex = 14;
            this.通信配置15Btn.Text = "通信配置15";
            this.通信配置15Btn.UseVisualStyleBackColor = true;
            this.通信配置15Btn.Click += new System.EventHandler(this.通信配置15Btn_Click);
            // 
            // 通信配置14Btn
            // 
            this.通信配置14Btn.Location = new System.Drawing.Point(1056, 4);
            this.通信配置14Btn.Name = "通信配置14Btn";
            this.通信配置14Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置14Btn.TabIndex = 13;
            this.通信配置14Btn.Text = "通信配置14";
            this.通信配置14Btn.UseVisualStyleBackColor = true;
            this.通信配置14Btn.Click += new System.EventHandler(this.通信配置14Btn_Click);
            // 
            // 通信配置13Btn
            // 
            this.通信配置13Btn.Location = new System.Drawing.Point(975, 4);
            this.通信配置13Btn.Name = "通信配置13Btn";
            this.通信配置13Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置13Btn.TabIndex = 12;
            this.通信配置13Btn.Text = "通信配置13";
            this.通信配置13Btn.UseVisualStyleBackColor = true;
            this.通信配置13Btn.Click += new System.EventHandler(this.通信配置13Btn_Click);
            // 
            // 通信配置12Btn
            // 
            this.通信配置12Btn.Location = new System.Drawing.Point(894, 4);
            this.通信配置12Btn.Name = "通信配置12Btn";
            this.通信配置12Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置12Btn.TabIndex = 11;
            this.通信配置12Btn.Text = "通信配置12";
            this.通信配置12Btn.UseVisualStyleBackColor = true;
            this.通信配置12Btn.Click += new System.EventHandler(this.通信配置12Btn_Click);
            // 
            // 通信配置11Btn
            // 
            this.通信配置11Btn.Location = new System.Drawing.Point(813, 4);
            this.通信配置11Btn.Name = "通信配置11Btn";
            this.通信配置11Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置11Btn.TabIndex = 10;
            this.通信配置11Btn.Text = "通信配置11";
            this.通信配置11Btn.UseVisualStyleBackColor = true;
            this.通信配置11Btn.Click += new System.EventHandler(this.通信配置11Btn_Click);
            // 
            // 通信配置10Btn
            // 
            this.通信配置10Btn.AccessibleDescription = "";
            this.通信配置10Btn.Location = new System.Drawing.Point(732, 4);
            this.通信配置10Btn.Name = "通信配置10Btn";
            this.通信配置10Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置10Btn.TabIndex = 9;
            this.通信配置10Btn.Text = "通信配置10";
            this.通信配置10Btn.UseVisualStyleBackColor = true;
            this.通信配置10Btn.Click += new System.EventHandler(this.通信配置10Btn_Click);
            // 
            // 通信配置9Btn
            // 
            this.通信配置9Btn.Location = new System.Drawing.Point(651, 4);
            this.通信配置9Btn.Name = "通信配置9Btn";
            this.通信配置9Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置9Btn.TabIndex = 8;
            this.通信配置9Btn.Text = "通信配置9";
            this.通信配置9Btn.UseVisualStyleBackColor = true;
            this.通信配置9Btn.Click += new System.EventHandler(this.通信配置9Btn_Click);
            // 
            // 通信配置8Btn
            // 
            this.通信配置8Btn.Location = new System.Drawing.Point(570, 4);
            this.通信配置8Btn.Name = "通信配置8Btn";
            this.通信配置8Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置8Btn.TabIndex = 7;
            this.通信配置8Btn.Text = "通信配置8";
            this.通信配置8Btn.UseVisualStyleBackColor = true;
            this.通信配置8Btn.Click += new System.EventHandler(this.通信配置8Btn_Click);
            // 
            // 通信配置7Btn
            // 
            this.通信配置7Btn.Location = new System.Drawing.Point(489, 4);
            this.通信配置7Btn.Name = "通信配置7Btn";
            this.通信配置7Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置7Btn.TabIndex = 6;
            this.通信配置7Btn.Text = "通信配置7";
            this.通信配置7Btn.UseVisualStyleBackColor = true;
            this.通信配置7Btn.Click += new System.EventHandler(this.通信配置7Btn_Click);
            // 
            // 通信配置6Btn
            // 
            this.通信配置6Btn.Location = new System.Drawing.Point(408, 4);
            this.通信配置6Btn.Name = "通信配置6Btn";
            this.通信配置6Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置6Btn.TabIndex = 5;
            this.通信配置6Btn.Text = "通信配置6";
            this.通信配置6Btn.UseVisualStyleBackColor = true;
            this.通信配置6Btn.Click += new System.EventHandler(this.通信配置6Btn_Click);
            // 
            // 通信配置5Btn
            // 
            this.通信配置5Btn.Location = new System.Drawing.Point(327, 4);
            this.通信配置5Btn.Name = "通信配置5Btn";
            this.通信配置5Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置5Btn.TabIndex = 4;
            this.通信配置5Btn.Text = "通信配置5";
            this.通信配置5Btn.UseVisualStyleBackColor = true;
            this.通信配置5Btn.Click += new System.EventHandler(this.通信配置5Btn_Click);
            // 
            // 通信配置4Btn
            // 
            this.通信配置4Btn.Location = new System.Drawing.Point(246, 4);
            this.通信配置4Btn.Name = "通信配置4Btn";
            this.通信配置4Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置4Btn.TabIndex = 3;
            this.通信配置4Btn.Text = "通信配置4";
            this.通信配置4Btn.UseVisualStyleBackColor = true;
            this.通信配置4Btn.Click += new System.EventHandler(this.通信配置4Btn_Click);
            // 
            // 通信配置3Btn
            // 
            this.通信配置3Btn.Location = new System.Drawing.Point(165, 4);
            this.通信配置3Btn.Name = "通信配置3Btn";
            this.通信配置3Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置3Btn.TabIndex = 2;
            this.通信配置3Btn.Text = "通信配置3";
            this.通信配置3Btn.UseVisualStyleBackColor = true;
            this.通信配置3Btn.Click += new System.EventHandler(this.通信配置3Btn_Click);
            // 
            // 通信配置2Btn
            // 
            this.通信配置2Btn.Location = new System.Drawing.Point(84, 4);
            this.通信配置2Btn.Name = "通信配置2Btn";
            this.通信配置2Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置2Btn.TabIndex = 1;
            this.通信配置2Btn.Text = "通信配置2";
            this.通信配置2Btn.UseVisualStyleBackColor = true;
            this.通信配置2Btn.Click += new System.EventHandler(this.通信配置2Btn_Click);
            // 
            // 通信配置1Btn
            // 
            this.通信配置1Btn.Location = new System.Drawing.Point(3, 4);
            this.通信配置1Btn.Name = "通信配置1Btn";
            this.通信配置1Btn.Size = new System.Drawing.Size(75, 23);
            this.通信配置1Btn.TabIndex = 0;
            this.通信配置1Btn.Text = "通信配置1";
            this.通信配置1Btn.UseVisualStyleBackColor = true;
            this.通信配置1Btn.Click += new System.EventHandler(this.通信配置1Btn_Click);
            // 
            // DeviceCommunicationConfigFormOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1544, 822);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceCommunicationConfigFormOld";
            this.Text = "通信命令配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeviceCommunicationConfigForm_FormClosing);
            this.Load += new System.EventHandler(this.DeviceConfigParamManageForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label 通信命令配置label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox 实时刷新checkBox;
        private System.Windows.Forms.Button 清空配置Btn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button 通信配置1Btn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ActiveCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn CooreSysNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn MapCooreSysNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn CommuniteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescribeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AxisAddress;
        private System.Windows.Forms.DataGridViewComboBoxColumn AxisReadWrite;
        private System.Windows.Forms.DataGridViewComboBoxColumn DataTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn ReadBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewButtonColumn WriteBtn;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.DataGridViewButtonColumn InsertBtn;
        private System.Windows.Forms.Button 通信配置15Btn;
        private System.Windows.Forms.Button 通信配置14Btn;
        private System.Windows.Forms.Button 通信配置13Btn;
        private System.Windows.Forms.Button 通信配置12Btn;
        private System.Windows.Forms.Button 通信配置11Btn;
        private System.Windows.Forms.Button 通信配置10Btn;
        private System.Windows.Forms.Button 通信配置9Btn;
        private System.Windows.Forms.Button 通信配置8Btn;
        private System.Windows.Forms.Button 通信配置7Btn;
        private System.Windows.Forms.Button 通信配置6Btn;
        private System.Windows.Forms.Button 通信配置5Btn;
        private System.Windows.Forms.Button 通信配置4Btn;
        private System.Windows.Forms.Button 通信配置3Btn;
        private System.Windows.Forms.Button 通信配置2Btn;
    }
}