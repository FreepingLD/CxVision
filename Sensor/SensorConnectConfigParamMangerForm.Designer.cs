namespace Sensor
{
    partial class SensorConnectConfigParamMangerForm
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
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SensorType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SensorBrand = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.HalInterfaceType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.connectType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ConnectAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AcqMethodCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DeviceDescribe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BaudRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopBits = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Parity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DataBits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SetPara = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.AcqBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(1663, 553);
            this.hWindowControl1.TabIndex = 1;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(1663, 553);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SaveButton);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 562);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1663, 312);
            this.panel1.TabIndex = 0;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(3, 6);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 33);
            this.SaveButton.TabIndex = 7;
            this.SaveButton.Text = "保存(Save)";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.名称,
            this.SensorType,
            this.SensorBrand,
            this.HalInterfaceType,
            this.connectType,
            this.ConnectAddress,
            this.Column2,
            this.AcqMethodCol,
            this.DeviceDescribe,
            this.Column5,
            this.Column1,
            this.Column3,
            this.Column4,
            this.PortName,
            this.BaudRate,
            this.StopBits,
            this.Parity,
            this.DataBits,
            this.SetPara,
            this.DeleteBtn,
            this.AcqBtn});
            this.dataGridView1.Location = new System.Drawing.Point(3, 45);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1658, 267);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.81579F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.18421F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1669, 877);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // 名称
            // 
            this.名称.DataPropertyName = "SensorName";
            this.名称.HeaderText = "传感器名称";
            this.名称.Name = "名称";
            this.名称.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SensorType
            // 
            this.SensorType.DataPropertyName = "SensorType";
            this.SensorType.HeaderText = "传感器类型";
            this.SensorType.Name = "SensorType";
            // 
            // SensorBrand
            // 
            this.SensorBrand.DataPropertyName = "SensorLinkLibrary";
            this.SensorBrand.HeaderText = "链接库";
            this.SensorBrand.Name = "SensorBrand";
            // 
            // HalInterfaceType
            // 
            this.HalInterfaceType.DataPropertyName = "HalInterfaceType";
            this.HalInterfaceType.HeaderText = "Halcon接口类型";
            this.HalInterfaceType.Name = "HalInterfaceType";
            // 
            // connectType
            // 
            this.connectType.DataPropertyName = "ConnectType";
            this.connectType.HeaderText = "连接类型";
            this.connectType.Name = "connectType";
            // 
            // ConnectAddress
            // 
            this.ConnectAddress.DataPropertyName = "ConnectAddress";
            this.ConnectAddress.HeaderText = "连接地址";
            this.ConnectAddress.Name = "ConnectAddress";
            this.ConnectAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "ChannelCount";
            this.Column2.HeaderText = "传感器通道数";
            this.Column2.Name = "Column2";
            // 
            // AcqMethodCol
            // 
            this.AcqMethodCol.DataPropertyName = "ImageAcqMethod";
            this.AcqMethodCol.HeaderText = "采图方式";
            this.AcqMethodCol.Items.AddRange(new object[] {
            "明场",
            "暗场",
            "明暗场"});
            this.AcqMethodCol.Name = "AcqMethodCol";
            // 
            // DeviceDescribe
            // 
            this.DeviceDescribe.DataPropertyName = "DeviceDescribe";
            this.DeviceDescribe.HeaderText = "传感器描述";
            this.DeviceDescribe.Name = "DeviceDescribe";
            this.DeviceDescribe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ConnectState";
            this.Column5.HeaderText = "连接";
            this.Column5.Name = "Column5";
            this.Column5.Width = 50;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "IsActive";
            this.Column1.HeaderText = "激活";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "IsAutoFocus";
            this.Column3.HeaderText = "自动对焦";
            this.Column3.Name = "Column3";
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "IsActiveDistortionCorrect";
            this.Column4.HeaderText = "畸变校正";
            this.Column4.Name = "Column4";
            this.Column4.Width = 60;
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
            // 
            // SetPara
            // 
            this.SetPara.DataPropertyName = "NONE";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "参数";
            this.SetPara.DefaultCellStyle = dataGridViewCellStyle1;
            this.SetPara.HeaderText = "";
            this.SetPara.Name = "SetPara";
            this.SetPara.Width = 50;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle2;
            this.DeleteBtn.HeaderText = "";
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 50;
            // 
            // AcqBtn
            // 
            this.AcqBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "采集";
            this.AcqBtn.DefaultCellStyle = dataGridViewCellStyle3;
            this.AcqBtn.HeaderText = "";
            this.AcqBtn.Name = "AcqBtn";
            this.AcqBtn.Width = 50;
            // 
            // SensorConnectConfigParamMangerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1669, 877);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SensorConnectConfigParamMangerForm";
            this.Text = "传感器连接配置管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorManageForm_FormClosing);
            this.Load += new System.EventHandler(this.SensorManageForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn 名称;
        private System.Windows.Forms.DataGridViewComboBoxColumn SensorType;
        private System.Windows.Forms.DataGridViewComboBoxColumn SensorBrand;
        private System.Windows.Forms.DataGridViewComboBoxColumn HalInterfaceType;
        private System.Windows.Forms.DataGridViewComboBoxColumn connectType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnectAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewComboBoxColumn AcqMethodCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceDescribe;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BaudRate;
        private System.Windows.Forms.DataGridViewComboBoxColumn StopBits;
        private System.Windows.Forms.DataGridViewComboBoxColumn Parity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataBits;
        private System.Windows.Forms.DataGridViewButtonColumn SetPara;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.DataGridViewButtonColumn AcqBtn;
    }
}