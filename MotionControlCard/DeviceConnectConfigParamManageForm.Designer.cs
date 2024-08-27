namespace MotionControlCard
{
    partial class DeviceConnectConfigParamManageForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DeviceModel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.connectType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ConnectAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BaudRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopBits = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Parity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DataBits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Send = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Open = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Close = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CommandCol = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.DeviceType,
            this.DeviceModel,
            this.connectType,
            this.ConnectAddress,
            this.DeviceName,
            this.Column4,
            this.Column5,
            this.Column3,
            this.Column2,
            this.PortName,
            this.BaudRate,
            this.StopBits,
            this.Parity,
            this.DataBits,
            this.Column8,
            this.Column9,
            this.Send,
            this.Open,
            this.Close,
            this.DeleteBtn,
            this.CommandCol});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1765, 650);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(3, 709);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(97, 44);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "保存设备配置(Save)";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click_1);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SaveButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1771, 756);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1765, 50);
            this.label2.TabIndex = 7;
            this.label2.Text = "设备连接配置";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "DeviceName";
            this.Column1.HeaderText = "设备名称";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 80;
            // 
            // DeviceType
            // 
            this.DeviceType.DataPropertyName = "DeviceType";
            this.DeviceType.HeaderText = "设备类型";
            this.DeviceType.Name = "DeviceType";
            // 
            // DeviceModel
            // 
            this.DeviceModel.DataPropertyName = "DeviceModel";
            this.DeviceModel.HeaderText = "设备型号";
            this.DeviceModel.Name = "DeviceModel";
            // 
            // connectType
            // 
            this.connectType.DataPropertyName = "ConnectType";
            this.connectType.HeaderText = "连接类型";
            this.connectType.Name = "connectType";
            // 
            // ConnectAddress
            // 
            this.ConnectAddress.DataPropertyName = "IpAdress";
            this.ConnectAddress.HeaderText = "连接地址";
            this.ConnectAddress.Name = "ConnectAddress";
            this.ConnectAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DeviceName
            // 
            this.DeviceName.DataPropertyName = "Port";
            this.DeviceName.HeaderText = "连接端口";
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "StationNo";
            this.Column4.HeaderText = "工站号";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 50;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "HeartTime";
            this.Column5.HeaderText = "心跳时间";
            this.Column5.Name = "Column5";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "IsActive";
            this.Column3.HeaderText = "激活";
            this.Column3.Name = "Column3";
            this.Column3.Width = 50;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "ConnectState";
            this.Column2.HeaderText = "连接状态";
            this.Column2.Name = "Column2";
            this.Column2.Width = 50;
            // 
            // PortName
            // 
            this.PortName.DataPropertyName = "PortName";
            this.PortName.HeaderText = "串口名";
            this.PortName.Name = "PortName";
            this.PortName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BaudRate
            // 
            this.BaudRate.DataPropertyName = "BaudRate";
            this.BaudRate.HeaderText = "波特率";
            this.BaudRate.Name = "BaudRate";
            this.BaudRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StopBits
            // 
            this.StopBits.DataPropertyName = "StopBits";
            this.StopBits.HeaderText = "停止位";
            this.StopBits.Name = "StopBits";
            this.StopBits.Width = 60;
            // 
            // Parity
            // 
            this.Parity.DataPropertyName = "Parity";
            this.Parity.HeaderText = "校验位";
            this.Parity.Name = "Parity";
            // 
            // DataBits
            // 
            this.DataBits.DataPropertyName = "DataBits";
            this.DataBits.HeaderText = "数据位";
            this.DataBits.Name = "DataBits";
            this.DataBits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DataBits.Width = 50;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "ReceiveData";
            this.Column8.HeaderText = "接收数据";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "SendData";
            this.Column9.HeaderText = "发送数据";
            this.Column9.Name = "Column9";
            // 
            // Send
            // 
            this.Send.DataPropertyName = "NONE ";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "发送";
            this.Send.DefaultCellStyle = dataGridViewCellStyle1;
            this.Send.HeaderText = "发送";
            this.Send.Name = "Send";
            this.Send.Text = "发送";
            this.Send.Width = 50;
            // 
            // Open
            // 
            this.Open.DataPropertyName = "NONE";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "连接";
            this.Open.DefaultCellStyle = dataGridViewCellStyle2;
            this.Open.HeaderText = "连接";
            this.Open.Name = "Open";
            this.Open.Width = 50;
            // 
            // Close
            // 
            this.Close.DataPropertyName = "NONE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "断开";
            this.Close.DefaultCellStyle = dataGridViewCellStyle3;
            this.Close.HeaderText = "断开";
            this.Close.Name = "Close";
            this.Close.Width = 50;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle4;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 50;
            // 
            // CommandCol
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "命令配置";
            this.CommandCol.DefaultCellStyle = dataGridViewCellStyle5;
            this.CommandCol.HeaderText = "命令配置";
            this.CommandCol.Name = "CommandCol";
            this.CommandCol.Width = 80;
            // 
            // DeviceConnectConfigParamManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1771, 756);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceConnectConfigParamManageForm";
            this.Text = "设备连接配置管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeviceConfigParamManageForm_FormClosing);
            this.Load += new System.EventHandler(this.DeviceConfigParamManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn DeviceType;
        private System.Windows.Forms.DataGridViewComboBoxColumn DeviceModel;
        private System.Windows.Forms.DataGridViewComboBoxColumn connectType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnectAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BaudRate;
        private System.Windows.Forms.DataGridViewComboBoxColumn StopBits;
        private System.Windows.Forms.DataGridViewComboBoxColumn Parity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataBits;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewButtonColumn Send;
        private System.Windows.Forms.DataGridViewButtonColumn Open;
        private System.Windows.Forms.DataGridViewButtonColumn Close;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.DataGridViewButtonColumn CommandCol;
    }
}