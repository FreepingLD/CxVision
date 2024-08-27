
namespace FunctionBlock
{
    partial class LightControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LightControlForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.控制器名称comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.通道4值label = new System.Windows.Forms.Label();
            this.通道3值label = new System.Windows.Forms.Label();
            this.通道2值label = new System.Windows.Forms.Label();
            this.通道1值label = new System.Windows.Forms.Label();
            this.通道4trackBar = new System.Windows.Forms.TrackBar();
            this.通道4checkBox = new System.Windows.Forms.CheckBox();
            this.通道3trackBar = new System.Windows.Forms.TrackBar();
            this.通道3checkBox = new System.Windows.Forms.CheckBox();
            this.通道2trackBar = new System.Windows.Forms.TrackBar();
            this.通道2checkBox = new System.Windows.Forms.CheckBox();
            this.通道1trackBar = new System.Windows.Forms.TrackBar();
            this.通道1checkBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.通道4trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.通道3trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.通道2trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.通道1trackBar)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 4);
            this.groupBox1.Controls.Add(this.控制器名称comboBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.通道4值label);
            this.groupBox1.Controls.Add(this.通道3值label);
            this.groupBox1.Controls.Add(this.通道2值label);
            this.groupBox1.Controls.Add(this.通道1值label);
            this.groupBox1.Controls.Add(this.通道4trackBar);
            this.groupBox1.Controls.Add(this.通道4checkBox);
            this.groupBox1.Controls.Add(this.通道3trackBar);
            this.groupBox1.Controls.Add(this.通道3checkBox);
            this.groupBox1.Controls.Add(this.通道2trackBar);
            this.groupBox1.Controls.Add(this.通道2checkBox);
            this.groupBox1.Controls.Add(this.通道1trackBar);
            this.groupBox1.Controls.Add(this.通道1checkBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 20);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(328, 164);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "光源控制";
            // 
            // 控制器名称comboBox
            // 
            this.控制器名称comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.控制器名称comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.控制器名称comboBox.FormattingEnabled = true;
            this.控制器名称comboBox.Location = new System.Drawing.Point(75, 17);
            this.控制器名称comboBox.Name = "控制器名称comboBox";
            this.控制器名称comboBox.Size = new System.Drawing.Size(234, 20);
            this.控制器名称comboBox.TabIndex = 13;
            this.控制器名称comboBox.SelectedIndexChanged += new System.EventHandler(this.控制器名称comboBox_SelectedIndexChanged);
            this.控制器名称comboBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.控制器名称comboBox_MouseDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "控制器";
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label5_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "通道4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "通道3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "通道2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "通道1";
            // 
            // 通道4值label
            // 
            this.通道4值label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.通道4值label.AutoSize = true;
            this.通道4值label.Location = new System.Drawing.Point(298, 134);
            this.通道4值label.Name = "通道4值label";
            this.通道4值label.Size = new System.Drawing.Size(11, 12);
            this.通道4值label.TabIndex = 11;
            this.通道4值label.Text = "0";
            // 
            // 通道3值label
            // 
            this.通道3值label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.通道3值label.AutoSize = true;
            this.通道3值label.Location = new System.Drawing.Point(298, 107);
            this.通道3值label.Name = "通道3值label";
            this.通道3值label.Size = new System.Drawing.Size(11, 12);
            this.通道3值label.TabIndex = 10;
            this.通道3值label.Text = "0";
            // 
            // 通道2值label
            // 
            this.通道2值label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.通道2值label.AutoSize = true;
            this.通道2值label.Location = new System.Drawing.Point(298, 79);
            this.通道2值label.Name = "通道2值label";
            this.通道2值label.Size = new System.Drawing.Size(11, 12);
            this.通道2值label.TabIndex = 9;
            this.通道2值label.Text = "0";
            // 
            // 通道1值label
            // 
            this.通道1值label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.通道1值label.AutoSize = true;
            this.通道1值label.Location = new System.Drawing.Point(298, 53);
            this.通道1值label.Name = "通道1值label";
            this.通道1值label.Size = new System.Drawing.Size(11, 12);
            this.通道1值label.TabIndex = 8;
            this.通道1值label.Text = "0";
            // 
            // 通道4trackBar
            // 
            this.通道4trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.通道4trackBar.Location = new System.Drawing.Point(65, 133);
            this.通道4trackBar.Margin = new System.Windows.Forms.Padding(0);
            this.通道4trackBar.Maximum = 255;
            this.通道4trackBar.Name = "通道4trackBar";
            this.通道4trackBar.Size = new System.Drawing.Size(230, 45);
            this.通道4trackBar.TabIndex = 7;
            this.通道4trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.通道4trackBar.Scroll += new System.EventHandler(this.通道4trackBar_Scroll);
            // 
            // 通道4checkBox
            // 
            this.通道4checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.通道4checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
            this.通道4checkBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.通道4checkBox.Location = new System.Drawing.Point(40, 130);
            this.通道4checkBox.Name = "通道4checkBox";
            this.通道4checkBox.Size = new System.Drawing.Size(25, 25);
            this.通道4checkBox.TabIndex = 6;
            this.通道4checkBox.UseVisualStyleBackColor = true;
            this.通道4checkBox.CheckedChanged += new System.EventHandler(this.通道4checkBox_CheckedChanged);
            // 
            // 通道3trackBar
            // 
            this.通道3trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.通道3trackBar.Location = new System.Drawing.Point(65, 106);
            this.通道3trackBar.Margin = new System.Windows.Forms.Padding(0);
            this.通道3trackBar.Maximum = 255;
            this.通道3trackBar.Name = "通道3trackBar";
            this.通道3trackBar.Size = new System.Drawing.Size(230, 45);
            this.通道3trackBar.TabIndex = 5;
            this.通道3trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.通道3trackBar.Scroll += new System.EventHandler(this.通道3trackBar_Scroll);
            // 
            // 通道3checkBox
            // 
            this.通道3checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.通道3checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
            this.通道3checkBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.通道3checkBox.Location = new System.Drawing.Point(40, 102);
            this.通道3checkBox.Name = "通道3checkBox";
            this.通道3checkBox.Size = new System.Drawing.Size(25, 25);
            this.通道3checkBox.TabIndex = 4;
            this.通道3checkBox.UseVisualStyleBackColor = true;
            this.通道3checkBox.CheckedChanged += new System.EventHandler(this.通道3checkBox_CheckedChanged);
            // 
            // 通道2trackBar
            // 
            this.通道2trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.通道2trackBar.Location = new System.Drawing.Point(65, 77);
            this.通道2trackBar.Margin = new System.Windows.Forms.Padding(0);
            this.通道2trackBar.Maximum = 255;
            this.通道2trackBar.Name = "通道2trackBar";
            this.通道2trackBar.Size = new System.Drawing.Size(230, 45);
            this.通道2trackBar.TabIndex = 3;
            this.通道2trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.通道2trackBar.Scroll += new System.EventHandler(this.通道2trackBar_Scroll);
            // 
            // 通道2checkBox
            // 
            this.通道2checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.通道2checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
            this.通道2checkBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.通道2checkBox.Location = new System.Drawing.Point(40, 73);
            this.通道2checkBox.Name = "通道2checkBox";
            this.通道2checkBox.Size = new System.Drawing.Size(25, 25);
            this.通道2checkBox.TabIndex = 2;
            this.通道2checkBox.UseVisualStyleBackColor = true;
            this.通道2checkBox.CheckedChanged += new System.EventHandler(this.通道2checkBox_CheckedChanged);
            // 
            // 通道1trackBar
            // 
            this.通道1trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.通道1trackBar.Location = new System.Drawing.Point(65, 49);
            this.通道1trackBar.Margin = new System.Windows.Forms.Padding(0);
            this.通道1trackBar.Maximum = 255;
            this.通道1trackBar.Name = "通道1trackBar";
            this.通道1trackBar.Size = new System.Drawing.Size(230, 45);
            this.通道1trackBar.TabIndex = 1;
            this.通道1trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.通道1trackBar.Scroll += new System.EventHandler(this.通道1trackBar_Scroll);
            // 
            // 通道1checkBox
            // 
            this.通道1checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.通道1checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
            this.通道1checkBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.通道1checkBox.Location = new System.Drawing.Point(40, 46);
            this.通道1checkBox.Name = "通道1checkBox";
            this.通道1checkBox.Size = new System.Drawing.Size(25, 25);
            this.通道1checkBox.TabIndex = 0;
            this.通道1checkBox.UseVisualStyleBackColor = true;
            this.通道1checkBox.CheckedChanged += new System.EventHandler(this.通道1checkBox_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMax, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(328, 184);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(299, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(29, 20);
            this.buttonClose.TabIndex = 30;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(270, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(29, 20);
            this.buttonMax.TabIndex = 29;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Enabled = false;
            this.buttonMin.Location = new System.Drawing.Point(242, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(28, 20);
            this.buttonMin.TabIndex = 28;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(242, 20);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "CxVision";
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // LightControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 184);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LightControlForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LightControlForm_FormClosing);
            this.Load += new System.EventHandler(this.LightControlForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LightControlForm_MouseDown);
            this.Move += new System.EventHandler(this.LightControlForm_Move);
            this.Resize += new System.EventHandler(this.LightControlForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.通道4trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.通道3trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.通道2trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.通道1trackBar)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label 通道4值label;
        private System.Windows.Forms.Label 通道3值label;
        private System.Windows.Forms.Label 通道2值label;
        private System.Windows.Forms.Label 通道1值label;
        private System.Windows.Forms.TrackBar 通道4trackBar;
        private System.Windows.Forms.CheckBox 通道4checkBox;
        private System.Windows.Forms.TrackBar 通道3trackBar;
        private System.Windows.Forms.CheckBox 通道3checkBox;
        private System.Windows.Forms.TrackBar 通道2trackBar;
        private System.Windows.Forms.CheckBox 通道2checkBox;
        private System.Windows.Forms.TrackBar 通道1trackBar;
        private System.Windows.Forms.CheckBox 通道1checkBox;
        private System.Windows.Forms.ComboBox 控制器名称comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
    }
}