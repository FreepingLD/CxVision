namespace FunctionBlock
{
    partial class JogMotionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JogMotionForm));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.坐标系名称comboBox = new System.Windows.Forms.ComboBox();
            this.参数button = new System.Windows.Forms.Button();
            this.速度lable = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.速度trackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.StopButton = new System.Windows.Forms.Button();
            this.MoveWAddButton = new System.Windows.Forms.Button();
            this.MoveUAddButton = new System.Windows.Forms.Button();
            this.MoveV轴AddButton = new System.Windows.Forms.Button();
            this.MoveWminusButton = new System.Windows.Forms.Button();
            this.MoveUminusButton = new System.Windows.Forms.Button();
            this.MoveV轴minusbutton = new System.Windows.Forms.Button();
            this.MoveZAddbutton = new System.Windows.Forms.Button();
            this.MoveYAddbutton = new System.Windows.Forms.Button();
            this.MoveXAddbutton = new System.Windows.Forms.Button();
            this.MoveZminusbutton = new System.Windows.Forms.Button();
            this.MoveYminusbutton = new System.Windows.Forms.Button();
            this.MoveXminusbutton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.速度trackBar)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox4, 4);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.坐标系名称comboBox);
            this.groupBox4.Controls.Add(this.参数button);
            this.groupBox4.Controls.Add(this.速度lable);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.速度trackBar);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.StopButton);
            this.groupBox4.Controls.Add(this.MoveWAddButton);
            this.groupBox4.Controls.Add(this.MoveUAddButton);
            this.groupBox4.Controls.Add(this.MoveV轴AddButton);
            this.groupBox4.Controls.Add(this.MoveWminusButton);
            this.groupBox4.Controls.Add(this.MoveUminusButton);
            this.groupBox4.Controls.Add(this.MoveV轴minusbutton);
            this.groupBox4.Controls.Add(this.MoveZAddbutton);
            this.groupBox4.Controls.Add(this.MoveYAddbutton);
            this.groupBox4.Controls.Add(this.MoveXAddbutton);
            this.groupBox4.Controls.Add(this.MoveZminusbutton);
            this.groupBox4.Controls.Add(this.MoveYminusbutton);
            this.groupBox4.Controls.Add(this.MoveXminusbutton);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(1, 21);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(325, 170);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "运动控制";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 33;
            this.label4.Text = "坐标系：";
            // 
            // 坐标系名称comboBox
            // 
            this.坐标系名称comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.坐标系名称comboBox.FormattingEnabled = true;
            this.坐标系名称comboBox.Location = new System.Drawing.Point(56, 19);
            this.坐标系名称comboBox.Name = "坐标系名称comboBox";
            this.坐标系名称comboBox.Size = new System.Drawing.Size(153, 20);
            this.坐标系名称comboBox.TabIndex = 32;
            this.坐标系名称comboBox.SelectedIndexChanged += new System.EventHandler(this.坐标系comboBox_SelectedIndexChanged);
            // 
            // 参数button
            // 
            this.参数button.Location = new System.Drawing.Point(224, 18);
            this.参数button.Name = "参数button";
            this.参数button.Size = new System.Drawing.Size(43, 24);
            this.参数button.TabIndex = 31;
            this.参数button.Text = "参数";
            this.参数button.UseVisualStyleBackColor = true;
            this.参数button.Click += new System.EventHandler(this.参数button_Click);
            // 
            // 速度lable
            // 
            this.速度lable.AutoSize = true;
            this.速度lable.Location = new System.Drawing.Point(284, 37);
            this.速度lable.Name = "速度lable";
            this.速度lable.Size = new System.Drawing.Size(17, 12);
            this.速度lable.TabIndex = 28;
            this.速度lable.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "mm/s";
            // 
            // 速度trackBar
            // 
            this.速度trackBar.AutoSize = false;
            this.速度trackBar.LargeChange = 1;
            this.速度trackBar.Location = new System.Drawing.Point(283, 53);
            this.速度trackBar.Margin = new System.Windows.Forms.Padding(0);
            this.速度trackBar.Maximum = 500;
            this.速度trackBar.Name = "速度trackBar";
            this.速度trackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.速度trackBar.Size = new System.Drawing.Size(24, 83);
            this.速度trackBar.TabIndex = 26;
            this.速度trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.速度trackBar.Value = 10;
            this.速度trackBar.Scroll += new System.EventHandler(this.速度trackBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(281, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "速度";
            // 
            // StopButton
            // 
            this.StopButton.BackgroundImage = global::FunctionBlock.Properties.Resources.Stop;
            this.StopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.StopButton.Location = new System.Drawing.Point(113, 125);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(43, 37);
            this.StopButton.TabIndex = 23;
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // MoveWAddButton
            // 
            this.MoveWAddButton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveW_add;
            this.MoveWAddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveWAddButton.Location = new System.Drawing.Point(3, 61);
            this.MoveWAddButton.Name = "MoveWAddButton";
            this.MoveWAddButton.Size = new System.Drawing.Size(25, 35);
            this.MoveWAddButton.TabIndex = 19;
            this.MoveWAddButton.UseVisualStyleBackColor = true;
            this.MoveWAddButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWAddButton_MouseDown);
            this.MoveWAddButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWAddButton_MouseUp);
            // 
            // MoveUAddButton
            // 
            this.MoveUAddButton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveU_add;
            this.MoveUAddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveUAddButton.Location = new System.Drawing.Point(63, 59);
            this.MoveUAddButton.Name = "MoveUAddButton";
            this.MoveUAddButton.Size = new System.Drawing.Size(25, 35);
            this.MoveUAddButton.TabIndex = 18;
            this.MoveUAddButton.UseVisualStyleBackColor = true;
            this.MoveUAddButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveU轴AddButton_MouseDown);
            this.MoveUAddButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveU轴AddButton_MouseUp);
            // 
            // MoveV轴AddButton
            // 
            this.MoveV轴AddButton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveV_add;
            this.MoveV轴AddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveV轴AddButton.Location = new System.Drawing.Point(87, 93);
            this.MoveV轴AddButton.Name = "MoveV轴AddButton";
            this.MoveV轴AddButton.Size = new System.Drawing.Size(35, 25);
            this.MoveV轴AddButton.TabIndex = 17;
            this.MoveV轴AddButton.UseVisualStyleBackColor = true;
            this.MoveV轴AddButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveV轴AddButton_MouseDown);
            this.MoveV轴AddButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveV轴AddButton_MouseUp);
            // 
            // MoveWminusButton
            // 
            this.MoveWminusButton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveW_minus;
            this.MoveWminusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveWminusButton.Location = new System.Drawing.Point(3, 115);
            this.MoveWminusButton.Name = "MoveWminusButton";
            this.MoveWminusButton.Size = new System.Drawing.Size(25, 35);
            this.MoveWminusButton.TabIndex = 16;
            this.MoveWminusButton.UseVisualStyleBackColor = true;
            this.MoveWminusButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWminusButton_MouseDown);
            this.MoveWminusButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWminusButton_MouseUp);
            // 
            // MoveUminusButton
            // 
            this.MoveUminusButton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveU_minus;
            this.MoveUminusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveUminusButton.Location = new System.Drawing.Point(63, 116);
            this.MoveUminusButton.Name = "MoveUminusButton";
            this.MoveUminusButton.Size = new System.Drawing.Size(25, 35);
            this.MoveUminusButton.TabIndex = 15;
            this.MoveUminusButton.UseVisualStyleBackColor = true;
            this.MoveUminusButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveU轴minusButton_MouseDown);
            this.MoveUminusButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveU轴minusButton_MouseUp);
            // 
            // MoveV轴minusbutton
            // 
            this.MoveV轴minusbutton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveV_minus;
            this.MoveV轴minusbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveV轴minusbutton.Location = new System.Drawing.Point(30, 93);
            this.MoveV轴minusbutton.Name = "MoveV轴minusbutton";
            this.MoveV轴minusbutton.Size = new System.Drawing.Size(35, 25);
            this.MoveV轴minusbutton.TabIndex = 14;
            this.MoveV轴minusbutton.UseVisualStyleBackColor = true;
            this.MoveV轴minusbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveVminusbutton_MouseDown);
            this.MoveV轴minusbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveVminusbutton_MouseUp);
            // 
            // MoveZAddbutton
            // 
            this.MoveZAddbutton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveZ_add;
            this.MoveZAddbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveZAddbutton.Location = new System.Drawing.Point(242, 62);
            this.MoveZAddbutton.Name = "MoveZAddbutton";
            this.MoveZAddbutton.Size = new System.Drawing.Size(25, 35);
            this.MoveZAddbutton.TabIndex = 13;
            this.MoveZAddbutton.UseVisualStyleBackColor = true;
            this.MoveZAddbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveZAddbutton_MouseDown);
            this.MoveZAddbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveZAddbutton_MouseUp);
            // 
            // MoveYAddbutton
            // 
            this.MoveYAddbutton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveY_minus;
            this.MoveYAddbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveYAddbutton.Location = new System.Drawing.Point(177, 60);
            this.MoveYAddbutton.Name = "MoveYAddbutton";
            this.MoveYAddbutton.Size = new System.Drawing.Size(25, 35);
            this.MoveYAddbutton.TabIndex = 12;
            this.MoveYAddbutton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MoveYAddbutton.UseVisualStyleBackColor = true;
            this.MoveYAddbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveYAddbutton_MouseDown);
            this.MoveYAddbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveYAddbutton_MouseUp);
            // 
            // MoveXAddbutton
            // 
            this.MoveXAddbutton.BackColor = System.Drawing.SystemColors.Control;
            this.MoveXAddbutton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveX_add;
            this.MoveXAddbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveXAddbutton.Location = new System.Drawing.Point(201, 95);
            this.MoveXAddbutton.Margin = new System.Windows.Forms.Padding(0);
            this.MoveXAddbutton.Name = "MoveXAddbutton";
            this.MoveXAddbutton.Size = new System.Drawing.Size(35, 25);
            this.MoveXAddbutton.TabIndex = 11;
            this.MoveXAddbutton.UseVisualStyleBackColor = false;
            this.MoveXAddbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveXAddbutton_MouseDown);
            this.MoveXAddbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveXAddbutton_MouseUp);
            // 
            // MoveZminusbutton
            // 
            this.MoveZminusbutton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveZ_minus;
            this.MoveZminusbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveZminusbutton.Location = new System.Drawing.Point(242, 119);
            this.MoveZminusbutton.Name = "MoveZminusbutton";
            this.MoveZminusbutton.Size = new System.Drawing.Size(25, 35);
            this.MoveZminusbutton.TabIndex = 10;
            this.MoveZminusbutton.UseVisualStyleBackColor = true;
            this.MoveZminusbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveZminusbutton_MouseDown);
            this.MoveZminusbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveZminusbutton_MouseUp);
            // 
            // MoveYminusbutton
            // 
            this.MoveYminusbutton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveY_add;
            this.MoveYminusbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveYminusbutton.Location = new System.Drawing.Point(177, 119);
            this.MoveYminusbutton.Margin = new System.Windows.Forms.Padding(0);
            this.MoveYminusbutton.Name = "MoveYminusbutton";
            this.MoveYminusbutton.Size = new System.Drawing.Size(25, 35);
            this.MoveYminusbutton.TabIndex = 9;
            this.MoveYminusbutton.UseVisualStyleBackColor = true;
            this.MoveYminusbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveYminusbutton_MouseDown);
            this.MoveYminusbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveYminusbutton_MouseUp);
            // 
            // MoveXminusbutton
            // 
            this.MoveXminusbutton.BackgroundImage = global::FunctionBlock.Properties.Resources.MoveX_minus;
            this.MoveXminusbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveXminusbutton.Location = new System.Drawing.Point(142, 94);
            this.MoveXminusbutton.Name = "MoveXminusbutton";
            this.MoveXminusbutton.Size = new System.Drawing.Size(35, 25);
            this.MoveXminusbutton.TabIndex = 4;
            this.MoveXminusbutton.UseVisualStyleBackColor = true;
            this.MoveXminusbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveXminusbutton_MouseDown);
            this.MoveXminusbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveXminusbutton_MouseUp);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMax, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 192);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(298, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(29, 20);
            this.buttonClose.TabIndex = 29;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(271, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(27, 20);
            this.buttonMax.TabIndex = 28;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Enabled = false;
            this.buttonMin.Location = new System.Drawing.Point(239, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(32, 20);
            this.buttonMin.TabIndex = 27;
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
            this.titleLabel.Size = new System.Drawing.Size(239, 20);
            this.titleLabel.TabIndex = 14;
            this.titleLabel.Text = "CxVision";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.拖动label_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.拖动label_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.拖动label_MouseLeave);
            // 
            // JogMotionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 192);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JogMotionForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MotionControl_FormClosing);
            this.Load += new System.EventHandler(this.JogMotionForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JogMotionForm_MouseDown);
            this.MouseEnter += new System.EventHandler(this.JogMotionForm_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.JogMotionForm_MouseLeave);
            this.Move += new System.EventHandler(this.JogMotionForm_Move);
            this.Resize += new System.EventHandler(this.JogMotionForm_Resize);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.速度trackBar)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button MoveZAddbutton;
        private System.Windows.Forms.Button MoveYAddbutton;
        private System.Windows.Forms.Button MoveXAddbutton;
        private System.Windows.Forms.Button MoveZminusbutton;
        private System.Windows.Forms.Button MoveYminusbutton;
        private System.Windows.Forms.Button MoveXminusbutton;
        private System.Windows.Forms.Button MoveWAddButton;
        private System.Windows.Forms.Button MoveUAddButton;
        private System.Windows.Forms.Button MoveV轴AddButton;
        private System.Windows.Forms.Button MoveWminusButton;
        private System.Windows.Forms.Button MoveUminusButton;
        private System.Windows.Forms.Button MoveV轴minusbutton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TrackBar 速度trackBar;
        private System.Windows.Forms.Label 速度lable;
        private System.Windows.Forms.Button 参数button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 坐标系名称comboBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
    }
}