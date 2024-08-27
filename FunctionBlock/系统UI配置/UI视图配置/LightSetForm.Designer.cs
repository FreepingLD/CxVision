namespace FunctionBlock
{
    partial class LightSetForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.保存button = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.光源dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DeleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.OpenBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.SetLightBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.LightValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LightChennelColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LightControlColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.光源dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 338);
            this.tableLayoutPanel1.TabIndex = 40;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.保存button);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 288);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(540, 50);
            this.panel3.TabIndex = 4;
            // 
            // 保存button
            // 
            this.保存button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.保存button.Location = new System.Drawing.Point(458, 3);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(70, 44);
            this.保存button.TabIndex = 41;
            this.保存button.Text = "保存采集源配置";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.光源dataGridView1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox4, 4);
            this.groupBox4.Size = new System.Drawing.Size(534, 282);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "光源列表";
            // 
            // 光源dataGridView1
            // 
            this.光源dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.光源dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LightControlColumn,
            this.LightChennelColumn,
            this.LightValue,
            this.SetLightBtn,
            this.OpenBtn,
            this.DeleteBtn,
            this.Column1});
            this.光源dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.光源dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.光源dataGridView1.Name = "光源dataGridView1";
            this.光源dataGridView1.RowHeadersWidth = 5;
            this.光源dataGridView1.RowTemplate.Height = 23;
            this.光源dataGridView1.Size = new System.Drawing.Size(528, 262);
            this.光源dataGridView1.TabIndex = 31;
            this.光源dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.光源dataGridView1_CellContentClick);
            this.光源dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.光源dataGridView1_DataError);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ChannelState";
            this.Column1.HeaderText = "状态";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "删除";
            this.DeleteBtn.DefaultCellStyle = dataGridViewCellStyle3;
            this.DeleteBtn.HeaderText = "删除";
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Width = 50;
            // 
            // OpenBtn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "打开";
            this.OpenBtn.DefaultCellStyle = dataGridViewCellStyle2;
            this.OpenBtn.HeaderText = "打开";
            this.OpenBtn.Name = "OpenBtn";
            this.OpenBtn.Width = 80;
            // 
            // SetLightBtn
            // 
            this.SetLightBtn.DataPropertyName = "NONE";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "设置亮度";
            this.SetLightBtn.DefaultCellStyle = dataGridViewCellStyle1;
            this.SetLightBtn.HeaderText = "设置亮度";
            this.SetLightBtn.Name = "SetLightBtn";
            this.SetLightBtn.Width = 80;
            // 
            // LightValue
            // 
            this.LightValue.DataPropertyName = "LightValue";
            this.LightValue.HeaderText = "光源值";
            this.LightValue.Name = "LightValue";
            this.LightValue.Width = 80;
            // 
            // LightChennelColumn
            // 
            this.LightChennelColumn.DataPropertyName = "Channel";
            this.LightChennelColumn.HeaderText = "光源通道";
            this.LightChennelColumn.Name = "LightChennelColumn";
            this.LightChennelColumn.Width = 80;
            // 
            // LightControlColumn
            // 
            this.LightControlColumn.DataPropertyName = "LightName";
            this.LightControlColumn.HeaderText = "光源控制器";
            this.LightControlColumn.Name = "LightControlColumn";
            // 
            // LightConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 338);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LightConfigForm";
            this.Text = "光源设置窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LightConfigForm_FormClosing);
            this.Load += new System.EventHandler(this.LightConfigForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.光源dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView 光源dataGridView1;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightControlColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn LightChennelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LightValue;
        private System.Windows.Forms.DataGridViewButtonColumn SetLightBtn;
        private System.Windows.Forms.DataGridViewButtonColumn OpenBtn;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button 保存button;
    }
}