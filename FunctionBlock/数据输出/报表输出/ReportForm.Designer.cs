
namespace FunctionBlock
{
    partial class ReportForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.readFileButton = new System.Windows.Forms.Button();
            this.读取文件路径textBox = new System.Windows.Forms.TextBox();
            this.radioButton_file = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.imageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.RowColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DegColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.日期checkBox = new System.Windows.Forms.CheckBox();
            this.文件目录textBox = new System.Windows.Forms.TextBox();
            this.directoryButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.保存配置button = new System.Windows.Forms.Button();
            this.导出数据button = new System.Windows.Forms.Button();
            this.显示类型comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.保存类型comboBox = new System.Windows.Forms.ComboBox();
            this.产品名称TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.图像列高comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.图像列宽comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.拖动label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.拖动label, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1188, 714);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.readFileButton);
            this.groupBox2.Controls.Add(this.读取文件路径textBox);
            this.groupBox2.Controls.Add(this.radioButton_file);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 66);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "读取数据文件";
            // 
            // readFileButton
            // 
            this.readFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.readFileButton.Location = new System.Drawing.Point(314, 31);
            this.readFileButton.Name = "readFileButton";
            this.readFileButton.Size = new System.Drawing.Size(21, 21);
            this.readFileButton.TabIndex = 2;
            this.readFileButton.Text = "……";
            this.readFileButton.UseVisualStyleBackColor = true;
            this.readFileButton.Click += new System.EventHandler(this.readFileButton_Click);
            // 
            // 读取文件路径textBox
            // 
            this.读取文件路径textBox.Location = new System.Drawing.Point(75, 31);
            this.读取文件路径textBox.Name = "读取文件路径textBox";
            this.读取文件路径textBox.Size = new System.Drawing.Size(233, 21);
            this.读取文件路径textBox.TabIndex = 1;
            // 
            // radioButton_file
            // 
            this.radioButton_file.AutoSize = true;
            this.radioButton_file.Location = new System.Drawing.Point(15, 32);
            this.radioButton_file.Name = "radioButton_file";
            this.radioButton_file.Size = new System.Drawing.Size(59, 16);
            this.radioButton_file.TabIndex = 0;
            this.radioButton_file.TabStop = true;
            this.radioButton_file.Text = "文件：";
            this.radioButton_file.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageColumn,
            this.RowColumn,
            this.ColColumn,
            this.DxColumn,
            this.DyColumn,
            this.DegColumn,
            this.Column1});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(353, 11);
            this.dataGridView1.Name = "dataGridView1";
            this.tableLayoutPanel1.SetRowSpan(this.dataGridView1, 3);
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(832, 700);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // imageColumn
            // 
            this.imageColumn.DataPropertyName = "Image";
            this.imageColumn.HeaderText = "image";
            this.imageColumn.Name = "imageColumn";
            // 
            // RowColumn
            // 
            this.RowColumn.DataPropertyName = "Row";
            this.RowColumn.HeaderText = "Row";
            this.RowColumn.Name = "RowColumn";
            // 
            // ColColumn
            // 
            this.ColColumn.DataPropertyName = "Col";
            this.ColColumn.HeaderText = "Col";
            this.ColColumn.Name = "ColColumn";
            // 
            // DxColumn
            // 
            this.DxColumn.DataPropertyName = "Dx";
            this.DxColumn.HeaderText = "Dx";
            this.DxColumn.Name = "DxColumn";
            // 
            // DyColumn
            // 
            this.DyColumn.DataPropertyName = "Dy";
            this.DyColumn.HeaderText = "Dy";
            this.DyColumn.Name = "DyColumn";
            // 
            // DegColumn
            // 
            this.DegColumn.DataPropertyName = "Deg";
            this.DegColumn.HeaderText = "Deg";
            this.DegColumn.Name = "DegColumn";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Result";
            this.Column1.HeaderText = "Result";
            this.Column1.Name = "Column1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.日期checkBox);
            this.groupBox1.Controls.Add(this.文件目录textBox);
            this.groupBox1.Controls.Add(this.directoryButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 83);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据存储目录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "存储目录：";
            // 
            // 日期checkBox
            // 
            this.日期checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.日期checkBox.AutoSize = true;
            this.日期checkBox.Location = new System.Drawing.Point(75, 57);
            this.日期checkBox.Name = "日期checkBox";
            this.日期checkBox.Size = new System.Drawing.Size(156, 16);
            this.日期checkBox.TabIndex = 17;
            this.日期checkBox.Text = "文件名后增加日期和时间";
            this.日期checkBox.UseVisualStyleBackColor = true;
            // 
            // 文件目录textBox
            // 
            this.文件目录textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.文件目录textBox.Location = new System.Drawing.Point(75, 20);
            this.文件目录textBox.Name = "文件目录textBox";
            this.文件目录textBox.Size = new System.Drawing.Size(233, 21);
            this.文件目录textBox.TabIndex = 15;
            // 
            // directoryButton
            // 
            this.directoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryButton.Location = new System.Drawing.Point(314, 20);
            this.directoryButton.Name = "directoryButton";
            this.directoryButton.Size = new System.Drawing.Size(21, 21);
            this.directoryButton.TabIndex = 16;
            this.directoryButton.Text = "……";
            this.directoryButton.UseVisualStyleBackColor = true;
            this.directoryButton.Click += new System.EventHandler(this.directoryButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.保存配置button);
            this.groupBox3.Controls.Add(this.导出数据button);
            this.groupBox3.Controls.Add(this.显示类型comboBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.保存类型comboBox);
            this.groupBox3.Controls.Add(this.产品名称TextBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.图像列高comboBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.图像列宽comboBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(344, 539);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据视图参数";
            // 
            // 保存配置button
            // 
            this.保存配置button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.保存配置button.Location = new System.Drawing.Point(75, 208);
            this.保存配置button.Name = "保存配置button";
            this.保存配置button.Size = new System.Drawing.Size(75, 23);
            this.保存配置button.TabIndex = 26;
            this.保存配置button.Text = "保存配置";
            this.保存配置button.UseVisualStyleBackColor = true;
            this.保存配置button.Click += new System.EventHandler(this.保存配置button_Click);
            // 
            // 导出数据button
            // 
            this.导出数据button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.导出数据button.Location = new System.Drawing.Point(233, 208);
            this.导出数据button.Name = "导出数据button";
            this.导出数据button.Size = new System.Drawing.Size(75, 23);
            this.导出数据button.TabIndex = 25;
            this.导出数据button.Text = "导出数据";
            this.导出数据button.UseVisualStyleBackColor = true;
            this.导出数据button.Click += new System.EventHandler(this.导出数据button_Click);
            // 
            // 显示类型comboBox
            // 
            this.显示类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.显示类型comboBox.FormattingEnabled = true;
            this.显示类型comboBox.Items.AddRange(new object[] {
            ".dxf",
            ".tiff",
            ".bmp",
            ".jpeg",
            ".png",
            ".hobj"});
            this.显示类型comboBox.Location = new System.Drawing.Point(75, 141);
            this.显示类型comboBox.Name = "显示类型comboBox";
            this.显示类型comboBox.Size = new System.Drawing.Size(233, 20);
            this.显示类型comboBox.TabIndex = 24;
            this.显示类型comboBox.SelectionChangeCommitted += new System.EventHandler(this.显示类型comboBox_SelectionChangeCommitted);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "显示类型：";
            // 
            // 保存类型comboBox
            // 
            this.保存类型comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.保存类型comboBox.FormattingEnabled = true;
            this.保存类型comboBox.Items.AddRange(new object[] {
            ".dxf",
            ".tiff",
            ".bmp",
            ".jpeg",
            ".png",
            ".hobj"});
            this.保存类型comboBox.Location = new System.Drawing.Point(75, 103);
            this.保存类型comboBox.Name = "保存类型comboBox";
            this.保存类型comboBox.Size = new System.Drawing.Size(233, 20);
            this.保存类型comboBox.TabIndex = 18;
            // 
            // 产品名称TextBox
            // 
            this.产品名称TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.产品名称TextBox.Location = new System.Drawing.Point(75, 181);
            this.产品名称TextBox.Name = "产品名称TextBox";
            this.产品名称TextBox.Size = new System.Drawing.Size(233, 21);
            this.产品名称TextBox.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "产品名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "保存类型：";
            // 
            // 图像列高comboBox
            // 
            this.图像列高comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.图像列高comboBox.FormattingEnabled = true;
            this.图像列高comboBox.Items.AddRange(new object[] {
            ".dxf",
            ".tiff",
            ".bmp",
            ".jpeg",
            ".png",
            ".hobj"});
            this.图像列高comboBox.Location = new System.Drawing.Point(75, 68);
            this.图像列高comboBox.Name = "图像列高comboBox";
            this.图像列高comboBox.Size = new System.Drawing.Size(233, 20);
            this.图像列高comboBox.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "图像列高：";
            // 
            // 图像列宽comboBox
            // 
            this.图像列宽comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.图像列宽comboBox.FormattingEnabled = true;
            this.图像列宽comboBox.Items.AddRange(new object[] {
            ".dxf",
            ".tiff",
            ".bmp",
            ".jpeg",
            ".png",
            ".hobj"});
            this.图像列宽comboBox.Location = new System.Drawing.Point(75, 33);
            this.图像列宽comboBox.Name = "图像列宽comboBox";
            this.图像列宽comboBox.Size = new System.Drawing.Size(233, 20);
            this.图像列宽comboBox.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "图像列宽：";
            // 
            // 拖动label
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.拖动label, 2);
            this.拖动label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.拖动label.Location = new System.Drawing.Point(3, 0);
            this.拖动label.Name = "拖动label";
            this.拖动label.Size = new System.Drawing.Size(1182, 8);
            this.拖动label.TabIndex = 4;
            this.拖动label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.拖动label_MouseDown);
            this.拖动label.MouseEnter += new System.EventHandler(this.拖动label_MouseEnter);
            this.拖动label.MouseLeave += new System.EventHandler(this.拖动label_MouseLeave);
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 714);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReportForm";
            this.Text = "报表输出";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportForm_FormClosing);
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ReportForm_MouseDown);
            this.Move += new System.EventHandler(this.ReportForm_Move);
            this.Resize += new System.EventHandler(this.ReportForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button readFileButton;
        private System.Windows.Forms.TextBox 读取文件路径textBox;
        private System.Windows.Forms.RadioButton radioButton_file;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn imageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DegColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox 日期checkBox;
        private System.Windows.Forms.TextBox 文件目录textBox;
        private System.Windows.Forms.Button directoryButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button 保存配置button;
        private System.Windows.Forms.Button 导出数据button;
        private System.Windows.Forms.ComboBox 显示类型comboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox 保存类型comboBox;
        private System.Windows.Forms.TextBox 产品名称TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 图像列高comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 图像列宽comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label 拖动label;
    }
}