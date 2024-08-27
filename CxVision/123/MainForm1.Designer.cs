namespace MainVision
{
    partial class MainForm1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm1));
            this.label_ShowUserName = new System.Windows.Forms.Label();
            this.label_UserName = new System.Windows.Forms.Label();
            this.label_ShowEqStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_ShowEqName = new System.Windows.Forms.Label();
            this.label_EqName = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Programlabel = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label_ShowUserName
            // 
            this.label_ShowUserName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label_ShowUserName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_ShowUserName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_ShowUserName.Location = new System.Drawing.Point(686, 48);
            this.label_ShowUserName.Name = "label_ShowUserName";
            this.label_ShowUserName.Size = new System.Drawing.Size(134, 39);
            this.label_ShowUserName.TabIndex = 236;
            this.label_ShowUserName.Text = "Administrator";
            this.label_ShowUserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_UserName
            // 
            this.label_UserName.BackColor = System.Drawing.Color.Black;
            this.label_UserName.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_UserName.ForeColor = System.Drawing.Color.White;
            this.label_UserName.Location = new System.Drawing.Point(581, 9);
            this.label_UserName.Name = "label_UserName";
            this.label_UserName.Size = new System.Drawing.Size(231, 36);
            this.label_UserName.TabIndex = 235;
            this.label_UserName.Text = "使用者账号";
            this.label_UserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_ShowEqStatus
            // 
            this.label_ShowEqStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label_ShowEqStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_ShowEqStatus.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_ShowEqStatus.Location = new System.Drawing.Point(418, 47);
            this.label_ShowEqStatus.Name = "label_ShowEqStatus";
            this.label_ShowEqStatus.Size = new System.Drawing.Size(157, 40);
            this.label_ShowEqStatus.TabIndex = 234;
            this.label_ShowEqStatus.Text = "Ready";
            this.label_ShowEqStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(418, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 38);
            this.label2.TabIndex = 233;
            this.label2.Text = "设备状态";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_ShowEqName
            // 
            this.label_ShowEqName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label_ShowEqName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_ShowEqName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_ShowEqName.Location = new System.Drawing.Point(265, 47);
            this.label_ShowEqName.Name = "label_ShowEqName";
            this.label_ShowEqName.Size = new System.Drawing.Size(150, 40);
            this.label_ShowEqName.TabIndex = 229;
            this.label_ShowEqName.Text = "天马对位补偿";
            this.label_ShowEqName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_ShowEqName.Click += new System.EventHandler(this.label_ShowEqName_Click);
            // 
            // label_EqName
            // 
            this.label_EqName.BackColor = System.Drawing.Color.Black;
            this.label_EqName.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_EqName.ForeColor = System.Drawing.Color.White;
            this.label_EqName.Location = new System.Drawing.Point(265, 6);
            this.label_EqName.Name = "label_EqName";
            this.label_EqName.Size = new System.Drawing.Size(150, 38);
            this.label_EqName.TabIndex = 228;
            this.label_EqName.Text = "设备名称";
            this.label_EqName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold);
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(581, 48);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 39);
            this.button3.TabIndex = 225;
            this.button3.Text = "切换使用者";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(19, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 24);
            this.label5.TabIndex = 40;
            this.label5.Text = "系统日期";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.textBox4.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox4.Location = new System.Drawing.Point(126, 48);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(123, 29);
            this.textBox4.TabIndex = 37;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(19, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 24);
            this.label6.TabIndex = 41;
            this.label6.Text = "系统时间";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.textBox5.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox5.Location = new System.Drawing.Point(126, 7);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(123, 29);
            this.textBox5.TabIndex = 38;
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.textBox4);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.textBox5);
            this.panel3.Location = new System.Drawing.Point(1001, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(270, 89);
            this.panel3.TabIndex = 226;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1284, 100);
            this.splitter1.TabIndex = 223;
            this.splitter1.TabStop = false;
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.Location = new System.Drawing.Point(2, 106);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1391, 893);
            this.dockPanel1.TabIndex = 227;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.Location = new System.Drawing.Point(6, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(232, 78);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 224;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(817, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 19);
            this.label1.TabIndex = 239;
            this.label1.Text = "程序：";
            // 
            // Programlabel
            // 
            this.Programlabel.AutoSize = true;
            this.Programlabel.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Programlabel.Location = new System.Drawing.Point(875, 19);
            this.Programlabel.Name = "Programlabel";
            this.Programlabel.Size = new System.Drawing.Size(69, 19);
            this.Programlabel.TabIndex = 240;
            this.Programlabel.Text = "label3";
            // 
            // MainForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 1005);
            this.Controls.Add(this.Programlabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_ShowUserName);
            this.Controls.Add(this.label_UserName);
            this.Controls.Add(this.label_ShowEqStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_ShowEqName);
            this.Controls.Add(this.label_EqName);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.dockPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm1";
            this.Text = "LDVision";
            this.Load += new System.EventHandler(this.MainForm1_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ShowUserName;
        private System.Windows.Forms.Label label_UserName;
        private System.Windows.Forms.Label label_ShowEqStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_ShowEqName;
        private System.Windows.Forms.Label label_EqName;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Programlabel;
    }
}