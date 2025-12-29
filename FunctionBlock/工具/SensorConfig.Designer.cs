namespace CxVision
{
    partial class SensorConfig
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Add_button = new System.Windows.Forms.Button();
            this.Delete_button = new System.Windows.Forms.Button();
            this.项目内容textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.showCategory_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.clear_button = new System.Windows.Forms.Button();
            this.AddCategory_button = new System.Windows.Forms.Button();
            this.Add_category_comboBox = new System.Windows.Forms.ComboBox();
            this.ClearCategory_button = new System.Windows.Forms.Button();
            this.sub_category_comboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.describe_textBox = new System.Windows.Forms.TextBox();
            this.configNameComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.保存button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 49);
            this.listBox1.Name = "listBox1";
            this.tableLayoutPanel1.SetRowSpan(this.listBox1, 6);
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(285, 326);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // Add_button
            // 
            this.Add_button.Location = new System.Drawing.Point(294, 49);
            this.Add_button.Name = "Add_button";
            this.Add_button.Size = new System.Drawing.Size(74, 21);
            this.Add_button.TabIndex = 2;
            this.Add_button.Text = "添加编号";
            this.Add_button.UseVisualStyleBackColor = true;
            this.Add_button.Click += new System.EventHandler(this.add_button1_Click);
            // 
            // Delete_button
            // 
            this.Delete_button.Location = new System.Drawing.Point(294, 77);
            this.Delete_button.Name = "Delete_button";
            this.Delete_button.Size = new System.Drawing.Size(74, 21);
            this.Delete_button.TabIndex = 3;
            this.Delete_button.Text = "删除编号 ";
            this.Delete_button.UseVisualStyleBackColor = true;
            this.Delete_button.Click += new System.EventHandler(this.delete_tbutton2_Click);
            // 
            // 项目内容textBox
            // 
            this.项目内容textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.项目内容textBox.Location = new System.Drawing.Point(3, 26);
            this.项目内容textBox.Name = "项目内容textBox";
            this.项目内容textBox.Size = new System.Drawing.Size(285, 21);
            this.项目内容textBox.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.项目内容textBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.showCategory_comboBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.clear_button, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.Add_button, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Delete_button, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.AddCategory_button, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.Add_category_comboBox, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.ClearCategory_button, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.sub_category_comboBox, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.configNameComboBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 378);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // showCategory_comboBox
            // 
            this.showCategory_comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showCategory_comboBox.FormattingEnabled = true;
            this.showCategory_comboBox.Location = new System.Drawing.Point(3, 3);
            this.showCategory_comboBox.Name = "showCategory_comboBox";
            this.showCategory_comboBox.Size = new System.Drawing.Size(285, 20);
            this.showCategory_comboBox.TabIndex = 5;
            this.showCategory_comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(294, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "传感器类型";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(294, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "传感器编号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(294, 105);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(74, 21);
            this.clear_button.TabIndex = 8;
            this.clear_button.Text = "清空编号";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // AddCategory_button
            // 
            this.AddCategory_button.Location = new System.Drawing.Point(294, 133);
            this.AddCategory_button.Name = "AddCategory_button";
            this.AddCategory_button.Size = new System.Drawing.Size(74, 21);
            this.AddCategory_button.TabIndex = 9;
            this.AddCategory_button.Text = "添加类型";
            this.AddCategory_button.UseVisualStyleBackColor = true;
            this.AddCategory_button.Click += new System.EventHandler(this.AddCategory_button1_Click);
            // 
            // Add_category_comboBox
            // 
            this.Add_category_comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Add_category_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Add_category_comboBox.FormattingEnabled = true;
            this.Add_category_comboBox.Location = new System.Drawing.Point(374, 133);
            this.Add_category_comboBox.Name = "Add_category_comboBox";
            this.Add_category_comboBox.Size = new System.Drawing.Size(94, 20);
            this.Add_category_comboBox.TabIndex = 11;
            // 
            // ClearCategory_button
            // 
            this.ClearCategory_button.Location = new System.Drawing.Point(294, 161);
            this.ClearCategory_button.Name = "ClearCategory_button";
            this.ClearCategory_button.Size = new System.Drawing.Size(74, 21);
            this.ClearCategory_button.TabIndex = 10;
            this.ClearCategory_button.Text = "删除类型";
            this.ClearCategory_button.UseVisualStyleBackColor = true;
            this.ClearCategory_button.Click += new System.EventHandler(this.ClearCategory_button_Click);
            // 
            // sub_category_comboBox
            // 
            this.sub_category_comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sub_category_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sub_category_comboBox.FormattingEnabled = true;
            this.sub_category_comboBox.Location = new System.Drawing.Point(374, 161);
            this.sub_category_comboBox.Name = "sub_category_comboBox";
            this.sub_category_comboBox.Size = new System.Drawing.Size(94, 20);
            this.sub_category_comboBox.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.describe_textBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(294, 189);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 186);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "注意事项";
            // 
            // describe_textBox
            // 
            this.describe_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.describe_textBox.Location = new System.Drawing.Point(3, 17);
            this.describe_textBox.Multiline = true;
            this.describe_textBox.Name = "describe_textBox";
            this.describe_textBox.Size = new System.Drawing.Size(168, 166);
            this.describe_textBox.TabIndex = 0;
            // 
            // configNameComboBox
            // 
            this.configNameComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configNameComboBox.FormattingEnabled = true;
            this.configNameComboBox.Items.AddRange(new object[] {
            "自定义配置文件名",
            "ConfigWireView",
            "ConfigDeepView",
            "ConfigMicroView"});
            this.configNameComboBox.Location = new System.Drawing.Point(374, 26);
            this.configNameComboBox.Name = "configNameComboBox";
            this.configNameComboBox.Size = new System.Drawing.Size(94, 20);
            this.configNameComboBox.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(374, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "预置配置文件名";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.保存button);
            this.panel1.Location = new System.Drawing.Point(374, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(94, 22);
            this.panel1.TabIndex = 16;
            // 
            // 保存button
            // 
            this.保存button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.保存button.Location = new System.Drawing.Point(0, 0);
            this.保存button.Name = "保存button";
            this.保存button.Size = new System.Drawing.Size(94, 22);
            this.保存button.TabIndex = 0;
            this.保存button.Text = "保存配置";
            this.保存button.UseVisualStyleBackColor = true;
            this.保存button.Click += new System.EventHandler(this.保存button_Click);
            // 
            // SensorConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 378);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SensorConfig";
            this.Text = "传感器配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorConfig_FormClosing);
            this.Load += new System.EventHandler(this.SensorConfig_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Add_button;
        private System.Windows.Forms.Button Delete_button;
        private System.Windows.Forms.TextBox 项目内容textBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox showCategory_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clear_button;
        private System.Windows.Forms.Button AddCategory_button;
        private System.Windows.Forms.Button ClearCategory_button;
        private System.Windows.Forms.ComboBox Add_category_comboBox;
        private System.Windows.Forms.ComboBox sub_category_comboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox describe_textBox;
        private System.Windows.Forms.ComboBox configNameComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button 保存button;
    }
}