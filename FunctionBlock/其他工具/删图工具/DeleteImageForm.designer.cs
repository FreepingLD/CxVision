using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class DeleteImageForm
    {
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteImageForm));
            this.文件目录1textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.保存时间textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.stop_button = new System.Windows.Forms.Button();
            this.start_button = new System.Windows.Forms.Button();
            this.directoryButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.刷新时间textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Savebutton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.删除路径1信息tabPage = new System.Windows.Forms.TabPage();
            this.删除路径2信息tabPage = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.删除路径3信息tabPage = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.listBox6 = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.文件目录2textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.文件目录3textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.删除路径1信息tabPage.SuspendLayout();
            this.删除路径2信息tabPage.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.删除路径3信息tabPage.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 文件目录1textBox
            // 
            this.文件目录1textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.文件目录1textBox.Location = new System.Drawing.Point(81, 25);
            this.文件目录1textBox.Name = "文件目录1textBox";
            this.文件目录1textBox.Size = new System.Drawing.Size(411, 21);
            this.文件目录1textBox.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 25);
            this.label2.TabIndex = 16;
            this.label2.Text = "删除路径1：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 保存时间textBox
            // 
            this.保存时间textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.保存时间textBox.Location = new System.Drawing.Point(81, 102);
            this.保存时间textBox.Name = "保存时间textBox";
            this.保存时间textBox.Size = new System.Drawing.Size(411, 21);
            this.保存时间textBox.TabIndex = 15;
            this.保存时间textBox.TextChanged += new System.EventHandler(this.保存时间textBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 25);
            this.label1.TabIndex = 14;
            this.label1.Text = "保存时间：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Location = new System.Drawing.Point(6, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 140);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "当前删除文件";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 17);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(580, 120);
            this.listBox1.TabIndex = 2;
            // 
            // stop_button
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.stop_button, 3);
            this.stop_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stop_button.Location = new System.Drawing.Point(525, 47);
            this.stop_button.Margin = new System.Windows.Forms.Padding(0);
            this.stop_button.Name = "stop_button";
            this.stop_button.Size = new System.Drawing.Size(90, 26);
            this.stop_button.TabIndex = 11;
            this.stop_button.Text = "停止";
            this.stop_button.UseVisualStyleBackColor = true;
            this.stop_button.Click += new System.EventHandler(this.stop_button_Click);
            // 
            // start_button
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.start_button, 3);
            this.start_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.start_button.Location = new System.Drawing.Point(525, 22);
            this.start_button.Margin = new System.Windows.Forms.Padding(0);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(90, 25);
            this.start_button.TabIndex = 10;
            this.start_button.Text = "开始";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // directoryButton
            // 
            this.directoryButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryButton.Location = new System.Drawing.Point(495, 22);
            this.directoryButton.Margin = new System.Windows.Forms.Padding(0);
            this.directoryButton.Name = "directoryButton";
            this.directoryButton.Size = new System.Drawing.Size(30, 25);
            this.directoryButton.TabIndex = 18;
            this.directoryButton.Text = "……";
            this.directoryButton.UseVisualStyleBackColor = true;
            this.directoryButton.Click += new System.EventHandler(this.directoryButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listBox2);
            this.groupBox2.Location = new System.Drawing.Point(9, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(586, 239);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "所有目录文件";
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(3, 17);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(580, 219);
            this.listBox2.TabIndex = 2;
            // 
            // 刷新时间textBox
            // 
            this.刷新时间textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.刷新时间textBox.Location = new System.Drawing.Point(0, 0);
            this.刷新时间textBox.Name = "刷新时间textBox";
            this.刷新时间textBox.Size = new System.Drawing.Size(417, 21);
            this.刷新时间textBox.TabIndex = 21;
            this.刷新时间textBox.TextChanged += new System.EventHandler(this.刷新时间textBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 25);
            this.label3.TabIndex = 20;
            this.label3.Text = "刷新时间：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 4);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(498, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 25);
            this.label4.TabIndex = 22;
            this.label4.Text = "(ms-删图间隔时间)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Savebutton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.Savebutton, 3);
            this.Savebutton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Savebutton.Location = new System.Drawing.Point(525, 73);
            this.Savebutton.Margin = new System.Windows.Forms.Padding(0);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(90, 26);
            this.Savebutton.TabIndex = 23;
            this.Savebutton.Text = "保存配置";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 6);
            this.tabControl1.Controls.Add(this.删除路径1信息tabPage);
            this.tabControl1.Controls.Add(this.删除路径2信息tabPage);
            this.tabControl1.Controls.Add(this.删除路径3信息tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 152);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(609, 425);
            this.tabControl1.TabIndex = 24;
            // 
            // 删除路径1信息tabPage
            // 
            this.删除路径1信息tabPage.Controls.Add(this.groupBox1);
            this.删除路径1信息tabPage.Controls.Add(this.groupBox2);
            this.删除路径1信息tabPage.Location = new System.Drawing.Point(4, 22);
            this.删除路径1信息tabPage.Name = "删除路径1信息tabPage";
            this.删除路径1信息tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.删除路径1信息tabPage.Size = new System.Drawing.Size(601, 399);
            this.删除路径1信息tabPage.TabIndex = 0;
            this.删除路径1信息tabPage.Text = "删除路径1信息";
            this.删除路径1信息tabPage.UseVisualStyleBackColor = true;
            // 
            // 删除路径2信息tabPage
            // 
            this.删除路径2信息tabPage.Controls.Add(this.groupBox5);
            this.删除路径2信息tabPage.Controls.Add(this.groupBox3);
            this.删除路径2信息tabPage.Location = new System.Drawing.Point(4, 22);
            this.删除路径2信息tabPage.Name = "删除路径2信息tabPage";
            this.删除路径2信息tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.删除路径2信息tabPage.Size = new System.Drawing.Size(605, 403);
            this.删除路径2信息tabPage.TabIndex = 1;
            this.删除路径2信息tabPage.Text = "删除路径2信息";
            this.删除路径2信息tabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.listBox4);
            this.groupBox5.Location = new System.Drawing.Point(9, 149);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(598, 256);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "所有目录文件";
            // 
            // listBox4
            // 
            this.listBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.ItemHeight = 12;
            this.listBox4.Location = new System.Drawing.Point(3, 17);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(592, 236);
            this.listBox4.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.listBox3);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(601, 140);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "当前删除文件";
            // 
            // listBox3
            // 
            this.listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 12;
            this.listBox3.Location = new System.Drawing.Point(3, 17);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(595, 120);
            this.listBox3.TabIndex = 2;
            // 
            // 删除路径3信息tabPage
            // 
            this.删除路径3信息tabPage.Controls.Add(this.groupBox6);
            this.删除路径3信息tabPage.Controls.Add(this.groupBox4);
            this.删除路径3信息tabPage.Location = new System.Drawing.Point(4, 22);
            this.删除路径3信息tabPage.Name = "删除路径3信息tabPage";
            this.删除路径3信息tabPage.Size = new System.Drawing.Size(605, 403);
            this.删除路径3信息tabPage.TabIndex = 2;
            this.删除路径3信息tabPage.Text = "删除路径3信息";
            this.删除路径3信息tabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.listBox6);
            this.groupBox6.Location = new System.Drawing.Point(6, 155);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(599, 251);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "所有目录文件";
            // 
            // listBox6
            // 
            this.listBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox6.FormattingEnabled = true;
            this.listBox6.ItemHeight = 12;
            this.listBox6.Location = new System.Drawing.Point(3, 17);
            this.listBox6.Name = "listBox6";
            this.listBox6.Size = new System.Drawing.Size(593, 231);
            this.listBox6.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.listBox5);
            this.groupBox4.Location = new System.Drawing.Point(6, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(602, 140);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "当前删除文件";
            // 
            // listBox5
            // 
            this.listBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox5.FormattingEnabled = true;
            this.listBox5.ItemHeight = 12;
            this.listBox5.Location = new System.Drawing.Point(3, 17);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(596, 120);
            this.listBox5.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(495, 47);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 26);
            this.button1.TabIndex = 27;
            this.button1.Text = "……";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.directory2Button_Click);
            // 
            // 文件目录2textBox
            // 
            this.文件目录2textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.文件目录2textBox.Location = new System.Drawing.Point(81, 50);
            this.文件目录2textBox.Name = "文件目录2textBox";
            this.文件目录2textBox.Size = new System.Drawing.Size(411, 21);
            this.文件目录2textBox.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 26);
            this.label5.TabIndex = 25;
            this.label5.Text = "删除路径2：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(495, 73);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 26);
            this.button2.TabIndex = 30;
            this.button2.Text = "……";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.directory3Button_Click);
            // 
            // 文件目录3textBox
            // 
            this.文件目录3textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.文件目录3textBox.Location = new System.Drawing.Point(81, 76);
            this.文件目录3textBox.Name = "文件目录3textBox";
            this.文件目录3textBox.Size = new System.Drawing.Size(411, 21);
            this.文件目录3textBox.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 26);
            this.label6.TabIndex = 28;
            this.label6.Text = "删除路径3：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonClose, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonMax, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMin, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Savebutton, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.stop_button, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.文件目录3textBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.start_button, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.文件目录2textBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.文件目录1textBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.保存时间textBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.button2, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.directoryButton, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(615, 580);
            this.tableLayoutPanel1.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label7, 4);
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(498, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 25);
            this.label7.TabIndex = 35;
            this.label7.Text = "(天)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(585, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(30, 22);
            this.buttonClose.TabIndex = 34;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(555, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(30, 22);
            this.buttonMax.TabIndex = 33;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Location = new System.Drawing.Point(525, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(30, 22);
            this.buttonMin.TabIndex = 32;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 3);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(521, 18);
            this.titleLabel.TabIndex = 31;
            this.titleLabel.Text = "自动删除图片";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.刷新时间textBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(78, 124);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 25);
            this.panel1.TabIndex = 30;
            // 
            // DeleteImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 580);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DeleteImageForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.删除路径1信息tabPage.ResumeLayout(false);
            this.删除路径2信息tabPage.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.删除路径3信息tabPage.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private TextBox 文件目录1textBox;

        private Label label2;

        private TextBox 保存时间textBox;

        private Label label1;

        private GroupBox groupBox1;

        private ListBox listBox1;

        private Button stop_button;

        private Button start_button;

        private Button directoryButton;

        private GroupBox groupBox2;

        private ListBox listBox2;

        private TextBox 刷新时间textBox;

        private Label label3;
        private Button Savebutton;
        private TabControl tabControl1;
        private TabPage 删除路径1信息tabPage;
        private TabPage 删除路径2信息tabPage;
        private Button button1;
        private TextBox 文件目录2textBox;
        private Label label5;
        private Button button2;
        private TextBox 文件目录3textBox;
        private Label label6;
        private TabPage 删除路径3信息tabPage;
        private GroupBox groupBox5;
        private ListBox listBox4;
        private GroupBox groupBox3;
        private ListBox listBox3;
        private GroupBox groupBox6;
        private ListBox listBox6;
        private GroupBox groupBox4;
        private ListBox listBox5;
        private Label label4;
        private Label label7;
    }
}

