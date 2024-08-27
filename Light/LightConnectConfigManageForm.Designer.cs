namespace Light
{
    partial class LightConnectConfigManageForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LightType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LightModel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.connectType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ConnectAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BaudRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopBits = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Parity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DataBits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.SaveButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.LightType,
            this.LightModel,
            this.connectType,
            this.ConnectAddress,
            this.Column2,
            this.Column3,
            this.Column4,
            this.PortName,
            this.BaudRate,
            this.StopBits,
            this.Parity,
            this.DataBits,
            this.DeleteBtn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1122, 285);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "LightName";
            this.Column1.HeaderText = "光源名称";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LightType
            // 
            this.LightType.DataPropertyName = "LightType";
            this.LightType.HeaderText = "光源类型";
            this.LightType.Name = "LightType";
            // 
            // LightModel
            // 
            this.LightModel.DataPropertyName = "LightModel";
            this.LightModel.HeaderText = "光源型号";
            this.LightModel.Name = "LightModel";
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
            // Column2
            // 
            this.Column2.DataPropertyName = "ChannelCount";
            this.Column2.HeaderText = "通道数";
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "IsActive";
            this.Column3.HeaderText = "激活";
            this.Column3.Name = "Column3";
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ConnectState";
            this.Column4.HeaderText = "连接状态 ";
            this.Column4.Name = "Column4";
            this.Column4.Width = 50;
            // 
            // PortName
            // 
            this.PortName.DataPropertyName = "PortName";
            this.PortName.HeaderText = "串口名";
            this.PortName.Name = "PortName";
            this.PortName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PortName.Width = 50;
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
            this.StopBits.Width = 50;
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
            // DeleteBtn
            // 
            this.DeleteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle1;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 50;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(3, 344);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(107, 44);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "保存光源配置(Save)";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SaveButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1128, 391);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1122, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "光源连接配置";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LightConnectConfigManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 391);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LightConnectConfigManageForm";
            this.Text = "光源配置管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LightConfigManageForm_FormClosing);
            this.Load += new System.EventHandler(this.LightConnectConfigManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightType;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightModel;
        private System.Windows.Forms.DataGridViewComboBoxColumn connectType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnectAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BaudRate;
        private System.Windows.Forms.DataGridViewComboBoxColumn StopBits;
        private System.Windows.Forms.DataGridViewComboBoxColumn Parity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataBits;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
    }
}