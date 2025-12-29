using Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class DeleteImageForm : Form
    {
        private ConfigParam param = new ConfigParam();

        private CancellationTokenSource cts = null;


        //private IContainer components = null;

        //private TextBox 文件目录1textBox;

        //private Label label2;

        //private TextBox 保存时间textBox;

        //private Label label1;

        //private GroupBox groupBox1;

        //private ListBox listBox1;

        //private Button stop_button;

        //private Button start_button;

        //private Button directoryButton;

        //private GroupBox groupBox2;

        //private ListBox listBox2;

        //private TextBox 刷新时间textBox;

        //private Label label3;
        //private Button Savebutton;
        //private TabControl tabControl1;
        //private TabPage 删除路径1信息tabPage;
        //private TabPage 删除路径2信息tabPage;
        //private Button button1;
        //private TextBox 文件目录2textBox;
        //private Label label5;
        //private Button button2;
        //private TextBox 文件目录3textBox;
        //private Label label6;
        //private TabPage 删除路径3信息tabPage;
        //private GroupBox groupBox5;
        //private ListBox listBox4;
        //private GroupBox groupBox3;
        //private ListBox listBox3;
        //private GroupBox groupBox6;
        //private ListBox listBox6;
        //private GroupBox groupBox4;
        //private ListBox listBox5;
        //private Label label4;

        //private void InitializeComponent()
        //{
        //    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        //    this.文件目录1textBox = new System.Windows.Forms.TextBox();
        //    this.label2 = new System.Windows.Forms.Label();
        //    this.保存时间textBox = new System.Windows.Forms.TextBox();
        //    this.label1 = new System.Windows.Forms.Label();
        //    this.groupBox1 = new System.Windows.Forms.GroupBox();
        //    this.listBox1 = new System.Windows.Forms.ListBox();
        //    this.stop_button = new System.Windows.Forms.Button();
        //    this.start_button = new System.Windows.Forms.Button();
        //    this.directoryButton = new System.Windows.Forms.Button();
        //    this.groupBox2 = new System.Windows.Forms.GroupBox();
        //    this.listBox2 = new System.Windows.Forms.ListBox();
        //    this.刷新时间textBox = new System.Windows.Forms.TextBox();
        //    this.label3 = new System.Windows.Forms.Label();
        //    this.label4 = new System.Windows.Forms.Label();
        //    this.Savebutton = new System.Windows.Forms.Button();
        //    this.tabControl1 = new System.Windows.Forms.TabControl();
        //    this.删除路径1信息tabPage = new System.Windows.Forms.TabPage();
        //    this.删除路径2信息tabPage = new System.Windows.Forms.TabPage();
        //    this.groupBox5 = new System.Windows.Forms.GroupBox();
        //    this.listBox4 = new System.Windows.Forms.ListBox();
        //    this.groupBox3 = new System.Windows.Forms.GroupBox();
        //    this.listBox3 = new System.Windows.Forms.ListBox();
        //    this.删除路径3信息tabPage = new System.Windows.Forms.TabPage();
        //    this.groupBox6 = new System.Windows.Forms.GroupBox();
        //    this.listBox6 = new System.Windows.Forms.ListBox();
        //    this.groupBox4 = new System.Windows.Forms.GroupBox();
        //    this.listBox5 = new System.Windows.Forms.ListBox();
        //    this.button1 = new System.Windows.Forms.Button();
        //    this.文件目录2textBox = new System.Windows.Forms.TextBox();
        //    this.label5 = new System.Windows.Forms.Label();
        //    this.button2 = new System.Windows.Forms.Button();
        //    this.文件目录3textBox = new System.Windows.Forms.TextBox();
        //    this.label6 = new System.Windows.Forms.Label();
        //    this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        //    this.buttonMin = new System.Windows.Forms.Button();
        //    this.titleLabel = new System.Windows.Forms.Label();
        //    this.panel1 = new System.Windows.Forms.Panel();
        //    this.buttonMax = new System.Windows.Forms.Button();
        //    this.buttonClose = new System.Windows.Forms.Button();
        //    this.groupBox1.SuspendLayout();
        //    this.groupBox2.SuspendLayout();
        //    this.tabControl1.SuspendLayout();
        //    this.删除路径1信息tabPage.SuspendLayout();
        //    this.删除路径2信息tabPage.SuspendLayout();
        //    this.groupBox5.SuspendLayout();
        //    this.groupBox3.SuspendLayout();
        //    this.删除路径3信息tabPage.SuspendLayout();
        //    this.groupBox6.SuspendLayout();
        //    this.groupBox4.SuspendLayout();
        //    this.tableLayoutPanel1.SuspendLayout();
        //    this.panel1.SuspendLayout();
        //    this.SuspendLayout();
        //    // 
        //    // 文件目录1textBox
        //    // 
        //    this.文件目录1textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.文件目录1textBox.Location = new System.Drawing.Point(84, 25);
        //    this.文件目录1textBox.Name = "文件目录1textBox";
        //    this.文件目录1textBox.Size = new System.Drawing.Size(604, 21);
        //    this.文件目录1textBox.TabIndex = 17;
        //    // 
        //    // label2
        //    // 
        //    this.label2.AutoSize = true;
        //    this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.label2.Location = new System.Drawing.Point(3, 22);
        //    this.label2.Name = "label2";
        //    this.label2.Size = new System.Drawing.Size(75, 25);
        //    this.label2.TabIndex = 16;
        //    this.label2.Text = "删除路径1：";
        //    this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //    // 
        //    // 保存时间textBox
        //    // 
        //    this.保存时间textBox.Location = new System.Drawing.Point(84, 102);
        //    this.保存时间textBox.Name = "保存时间textBox";
        //    this.保存时间textBox.Size = new System.Drawing.Size(147, 21);
        //    this.保存时间textBox.TabIndex = 15;
        //    this.保存时间textBox.TextChanged += new System.EventHandler(this.保存时间textBox_TextChanged);
        //    // 
        //    // label1
        //    // 
        //    this.label1.AutoSize = true;
        //    this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.label1.Location = new System.Drawing.Point(3, 99);
        //    this.label1.Name = "label1";
        //    this.label1.Size = new System.Drawing.Size(75, 25);
        //    this.label1.TabIndex = 14;
        //    this.label1.Text = "保存时间：";
        //    this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //    // 
        //    // groupBox1
        //    // 
        //    this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.groupBox1.Controls.Add(this.listBox1);
        //    this.groupBox1.Location = new System.Drawing.Point(6, 8);
        //    this.groupBox1.Name = "groupBox1";
        //    this.groupBox1.Size = new System.Drawing.Size(782, 140);
        //    this.groupBox1.TabIndex = 12;
        //    this.groupBox1.TabStop = false;
        //    this.groupBox1.Text = "当前删除文件";
        //    // 
        //    // listBox1
        //    // 
        //    this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.listBox1.FormattingEnabled = true;
        //    this.listBox1.ItemHeight = 12;
        //    this.listBox1.Location = new System.Drawing.Point(3, 17);
        //    this.listBox1.Name = "listBox1";
        //    this.listBox1.Size = new System.Drawing.Size(776, 120);
        //    this.listBox1.TabIndex = 2;
        //    // 
        //    // stop_button
        //    // 
        //    this.tableLayoutPanel1.SetColumnSpan(this.stop_button, 3);
        //    this.stop_button.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.stop_button.Location = new System.Drawing.Point(721, 47);
        //    this.stop_button.Margin = new System.Windows.Forms.Padding(0);
        //    this.stop_button.Name = "stop_button";
        //    this.stop_button.Size = new System.Drawing.Size(90, 26);
        //    this.stop_button.TabIndex = 11;
        //    this.stop_button.Text = "停止";
        //    this.stop_button.UseVisualStyleBackColor = true;
        //    this.stop_button.Click += new System.EventHandler(this.stop_button_Click);
        //    // 
        //    // start_button
        //    // 
        //    this.tableLayoutPanel1.SetColumnSpan(this.start_button, 3);
        //    this.start_button.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.start_button.Location = new System.Drawing.Point(721, 22);
        //    this.start_button.Margin = new System.Windows.Forms.Padding(0);
        //    this.start_button.Name = "start_button";
        //    this.start_button.Size = new System.Drawing.Size(90, 25);
        //    this.start_button.TabIndex = 10;
        //    this.start_button.Text = "开始";
        //    this.start_button.UseVisualStyleBackColor = true;
        //    this.start_button.Click += new System.EventHandler(this.start_button_Click);
        //    // 
        //    // directoryButton
        //    // 
        //    this.directoryButton.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.directoryButton.Location = new System.Drawing.Point(691, 22);
        //    this.directoryButton.Margin = new System.Windows.Forms.Padding(0);
        //    this.directoryButton.Name = "directoryButton";
        //    this.directoryButton.Size = new System.Drawing.Size(30, 25);
        //    this.directoryButton.TabIndex = 18;
        //    this.directoryButton.Text = "……";
        //    this.directoryButton.UseVisualStyleBackColor = true;
        //    this.directoryButton.Click += new System.EventHandler(this.directoryButton_Click);
        //    // 
        //    // groupBox2
        //    // 
        //    this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        //    | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.groupBox2.Controls.Add(this.listBox2);
        //    this.groupBox2.Location = new System.Drawing.Point(9, 154);
        //    this.groupBox2.Name = "groupBox2";
        //    this.groupBox2.Size = new System.Drawing.Size(782, 393);
        //    this.groupBox2.TabIndex = 19;
        //    this.groupBox2.TabStop = false;
        //    this.groupBox2.Text = "所有目录文件";
        //    // 
        //    // listBox2
        //    // 
        //    this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.listBox2.FormattingEnabled = true;
        //    this.listBox2.ItemHeight = 12;
        //    this.listBox2.Location = new System.Drawing.Point(3, 17);
        //    this.listBox2.Name = "listBox2";
        //    this.listBox2.Size = new System.Drawing.Size(776, 373);
        //    this.listBox2.TabIndex = 2;
        //    // 
        //    // 刷新时间textBox
        //    // 
        //    this.刷新时间textBox.Location = new System.Drawing.Point(3, 5);
        //    this.刷新时间textBox.Name = "刷新时间textBox";
        //    this.刷新时间textBox.Size = new System.Drawing.Size(147, 21);
        //    this.刷新时间textBox.TabIndex = 21;
        //    this.刷新时间textBox.TextChanged += new System.EventHandler(this.刷新时间textBox_TextChanged);
        //    // 
        //    // label3
        //    // 
        //    this.label3.AutoSize = true;
        //    this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.label3.Location = new System.Drawing.Point(3, 124);
        //    this.label3.Name = "label3";
        //    this.label3.Size = new System.Drawing.Size(75, 36);
        //    this.label3.TabIndex = 20;
        //    this.label3.Text = "刷新时间：";
        //    this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //    // 
        //    // label4
        //    // 
        //    this.label4.AutoSize = true;
        //    this.label4.Location = new System.Drawing.Point(156, 11);
        //    this.label4.Name = "label4";
        //    this.label4.Size = new System.Drawing.Size(107, 12);
        //    this.label4.TabIndex = 22;
        //    this.label4.Text = "(ms-删图间隔时间)";
        //    // 
        //    // Savebutton
        //    // 
        //    this.tableLayoutPanel1.SetColumnSpan(this.Savebutton, 3);
        //    this.Savebutton.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.Savebutton.Location = new System.Drawing.Point(721, 73);
        //    this.Savebutton.Margin = new System.Windows.Forms.Padding(0);
        //    this.Savebutton.Name = "Savebutton";
        //    this.Savebutton.Size = new System.Drawing.Size(90, 26);
        //    this.Savebutton.TabIndex = 23;
        //    this.Savebutton.Text = "保存配置";
        //    this.Savebutton.UseVisualStyleBackColor = true;
        //    this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
        //    // 
        //    // tabControl1
        //    // 
        //    this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 6);
        //    this.tabControl1.Controls.Add(this.删除路径1信息tabPage);
        //    this.tabControl1.Controls.Add(this.删除路径2信息tabPage);
        //    this.tabControl1.Controls.Add(this.删除路径3信息tabPage);
        //    this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.tabControl1.Location = new System.Drawing.Point(3, 163);
        //    this.tabControl1.Name = "tabControl1";
        //    this.tabControl1.SelectedIndex = 0;
        //    this.tabControl1.Size = new System.Drawing.Size(805, 579);
        //    this.tabControl1.TabIndex = 24;
        //    // 
        //    // 删除路径1信息tabPage
        //    // 
        //    this.删除路径1信息tabPage.Controls.Add(this.groupBox1);
        //    this.删除路径1信息tabPage.Controls.Add(this.groupBox2);
        //    this.删除路径1信息tabPage.Location = new System.Drawing.Point(4, 22);
        //    this.删除路径1信息tabPage.Name = "删除路径1信息tabPage";
        //    this.删除路径1信息tabPage.Padding = new System.Windows.Forms.Padding(3);
        //    this.删除路径1信息tabPage.Size = new System.Drawing.Size(797, 553);
        //    this.删除路径1信息tabPage.TabIndex = 0;
        //    this.删除路径1信息tabPage.Text = "删除路径1信息";
        //    this.删除路径1信息tabPage.UseVisualStyleBackColor = true;
        //    // 
        //    // 删除路径2信息tabPage
        //    // 
        //    this.删除路径2信息tabPage.Controls.Add(this.groupBox5);
        //    this.删除路径2信息tabPage.Controls.Add(this.groupBox3);
        //    this.删除路径2信息tabPage.Location = new System.Drawing.Point(4, 22);
        //    this.删除路径2信息tabPage.Name = "删除路径2信息tabPage";
        //    this.删除路径2信息tabPage.Padding = new System.Windows.Forms.Padding(3);
        //    this.删除路径2信息tabPage.Size = new System.Drawing.Size(797, 553);
        //    this.删除路径2信息tabPage.TabIndex = 1;
        //    this.删除路径2信息tabPage.Text = "删除路径2信息";
        //    this.删除路径2信息tabPage.UseVisualStyleBackColor = true;
        //    // 
        //    // groupBox5
        //    // 
        //    this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        //    | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.groupBox5.Controls.Add(this.listBox4);
        //    this.groupBox5.Location = new System.Drawing.Point(9, 149);
        //    this.groupBox5.Name = "groupBox5";
        //    this.groupBox5.Size = new System.Drawing.Size(782, 398);
        //    this.groupBox5.TabIndex = 20;
        //    this.groupBox5.TabStop = false;
        //    this.groupBox5.Text = "所有目录文件";
        //    // 
        //    // listBox4
        //    // 
        //    this.listBox4.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.listBox4.FormattingEnabled = true;
        //    this.listBox4.ItemHeight = 12;
        //    this.listBox4.Location = new System.Drawing.Point(3, 17);
        //    this.listBox4.Name = "listBox4";
        //    this.listBox4.Size = new System.Drawing.Size(776, 378);
        //    this.listBox4.TabIndex = 2;
        //    // 
        //    // groupBox3
        //    // 
        //    this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.groupBox3.Controls.Add(this.listBox3);
        //    this.groupBox3.Location = new System.Drawing.Point(6, 6);
        //    this.groupBox3.Name = "groupBox3";
        //    this.groupBox3.Size = new System.Drawing.Size(785, 140);
        //    this.groupBox3.TabIndex = 13;
        //    this.groupBox3.TabStop = false;
        //    this.groupBox3.Text = "当前删除文件";
        //    // 
        //    // listBox3
        //    // 
        //    this.listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.listBox3.FormattingEnabled = true;
        //    this.listBox3.ItemHeight = 12;
        //    this.listBox3.Location = new System.Drawing.Point(3, 17);
        //    this.listBox3.Name = "listBox3";
        //    this.listBox3.Size = new System.Drawing.Size(779, 120);
        //    this.listBox3.TabIndex = 2;
        //    // 
        //    // 删除路径3信息tabPage
        //    // 
        //    this.删除路径3信息tabPage.Controls.Add(this.groupBox6);
        //    this.删除路径3信息tabPage.Controls.Add(this.groupBox4);
        //    this.删除路径3信息tabPage.Location = new System.Drawing.Point(4, 22);
        //    this.删除路径3信息tabPage.Name = "删除路径3信息tabPage";
        //    this.删除路径3信息tabPage.Size = new System.Drawing.Size(797, 553);
        //    this.删除路径3信息tabPage.TabIndex = 2;
        //    this.删除路径3信息tabPage.Text = "删除路径3信息";
        //    this.删除路径3信息tabPage.UseVisualStyleBackColor = true;
        //    // 
        //    // groupBox6
        //    // 
        //    this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        //    | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.groupBox6.Controls.Add(this.listBox6);
        //    this.groupBox6.Location = new System.Drawing.Point(6, 155);
        //    this.groupBox6.Name = "groupBox6";
        //    this.groupBox6.Size = new System.Drawing.Size(783, 393);
        //    this.groupBox6.TabIndex = 20;
        //    this.groupBox6.TabStop = false;
        //    this.groupBox6.Text = "所有目录文件";
        //    // 
        //    // listBox6
        //    // 
        //    this.listBox6.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.listBox6.FormattingEnabled = true;
        //    this.listBox6.ItemHeight = 12;
        //    this.listBox6.Location = new System.Drawing.Point(3, 17);
        //    this.listBox6.Name = "listBox6";
        //    this.listBox6.Size = new System.Drawing.Size(777, 373);
        //    this.listBox6.TabIndex = 2;
        //    // 
        //    // groupBox4
        //    // 
        //    this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.groupBox4.Controls.Add(this.listBox5);
        //    this.groupBox4.Location = new System.Drawing.Point(6, 12);
        //    this.groupBox4.Name = "groupBox4";
        //    this.groupBox4.Size = new System.Drawing.Size(786, 140);
        //    this.groupBox4.TabIndex = 13;
        //    this.groupBox4.TabStop = false;
        //    this.groupBox4.Text = "当前删除文件";
        //    // 
        //    // listBox5
        //    // 
        //    this.listBox5.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.listBox5.FormattingEnabled = true;
        //    this.listBox5.ItemHeight = 12;
        //    this.listBox5.Location = new System.Drawing.Point(3, 17);
        //    this.listBox5.Name = "listBox5";
        //    this.listBox5.Size = new System.Drawing.Size(780, 120);
        //    this.listBox5.TabIndex = 2;
        //    // 
        //    // button1
        //    // 
        //    this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.button1.Location = new System.Drawing.Point(691, 47);
        //    this.button1.Margin = new System.Windows.Forms.Padding(0);
        //    this.button1.Name = "button1";
        //    this.button1.Size = new System.Drawing.Size(30, 26);
        //    this.button1.TabIndex = 27;
        //    this.button1.Text = "……";
        //    this.button1.UseVisualStyleBackColor = true;
        //    this.button1.Click += new System.EventHandler(this.directory2Button_Click);
        //    // 
        //    // 文件目录2textBox
        //    // 
        //    this.文件目录2textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.文件目录2textBox.Location = new System.Drawing.Point(84, 50);
        //    this.文件目录2textBox.Name = "文件目录2textBox";
        //    this.文件目录2textBox.Size = new System.Drawing.Size(604, 21);
        //    this.文件目录2textBox.TabIndex = 26;
        //    // 
        //    // label5
        //    // 
        //    this.label5.AutoSize = true;
        //    this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.label5.Location = new System.Drawing.Point(3, 47);
        //    this.label5.Name = "label5";
        //    this.label5.Size = new System.Drawing.Size(75, 26);
        //    this.label5.TabIndex = 25;
        //    this.label5.Text = "删除路径2：";
        //    this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //    // 
        //    // button2
        //    // 
        //    this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.button2.Location = new System.Drawing.Point(691, 73);
        //    this.button2.Margin = new System.Windows.Forms.Padding(0);
        //    this.button2.Name = "button2";
        //    this.button2.Size = new System.Drawing.Size(30, 26);
        //    this.button2.TabIndex = 30;
        //    this.button2.Text = "……";
        //    this.button2.UseVisualStyleBackColor = true;
        //    this.button2.Click += new System.EventHandler(this.directory3Button_Click);
        //    // 
        //    // 文件目录3textBox
        //    // 
        //    this.文件目录3textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        //    | System.Windows.Forms.AnchorStyles.Right)));
        //    this.文件目录3textBox.Location = new System.Drawing.Point(84, 76);
        //    this.文件目录3textBox.Name = "文件目录3textBox";
        //    this.文件目录3textBox.Size = new System.Drawing.Size(604, 21);
        //    this.文件目录3textBox.TabIndex = 29;
        //    // 
        //    // label6
        //    // 
        //    this.label6.AutoSize = true;
        //    this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.label6.Location = new System.Drawing.Point(3, 73);
        //    this.label6.Name = "label6";
        //    this.label6.Size = new System.Drawing.Size(75, 26);
        //    this.label6.TabIndex = 28;
        //    this.label6.Text = "删除路径3：";
        //    this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //    // 
        //    // tableLayoutPanel1
        //    // 
        //    this.tableLayoutPanel1.ColumnCount = 6;
        //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
        //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        //    this.tableLayoutPanel1.Controls.Add(this.buttonClose, 5, 0);
        //    this.tableLayoutPanel1.Controls.Add(this.buttonMax, 3, 0);
        //    this.tableLayoutPanel1.Controls.Add(this.buttonMin, 3, 0);
        //    this.tableLayoutPanel1.Controls.Add(this.titleLabel, 0, 0);
        //    this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
        //    this.tableLayoutPanel1.Controls.Add(this.Savebutton, 3, 3);
        //    this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
        //    this.tableLayoutPanel1.Controls.Add(this.stop_button, 3, 2);
        //    this.tableLayoutPanel1.Controls.Add(this.文件目录3textBox, 1, 3);
        //    this.tableLayoutPanel1.Controls.Add(this.start_button, 3, 1);
        //    this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 6);
        //    this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
        //    this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
        //    this.tableLayoutPanel1.Controls.Add(this.文件目录2textBox, 1, 2);
        //    this.tableLayoutPanel1.Controls.Add(this.label3, 0, 5);
        //    this.tableLayoutPanel1.Controls.Add(this.文件目录1textBox, 1, 1);
        //    this.tableLayoutPanel1.Controls.Add(this.保存时间textBox, 1, 4);
        //    this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 5);
        //    this.tableLayoutPanel1.Controls.Add(this.button2, 2, 3);
        //    this.tableLayoutPanel1.Controls.Add(this.button1, 2, 2);
        //    this.tableLayoutPanel1.Controls.Add(this.directoryButton, 2, 1);
        //    this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        //    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        //    this.tableLayoutPanel1.RowCount = 7;
        //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
        //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
        //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
        //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
        //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
        //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
        //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        //    this.tableLayoutPanel1.Size = new System.Drawing.Size(811, 745);
        //    this.tableLayoutPanel1.TabIndex = 31;
        //    // 
        //    // buttonMin
        //    // 
        //    this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
        //    this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        //    this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.buttonMin.Location = new System.Drawing.Point(721, 0);
        //    this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
        //    this.buttonMin.Name = "buttonMin";
        //    this.buttonMin.Size = new System.Drawing.Size(30, 22);
        //    this.buttonMin.TabIndex = 32;
        //    this.buttonMin.UseVisualStyleBackColor = true;
        //    this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
        //    // 
        //    // titleLabel
        //    // 
        //    this.titleLabel.AutoSize = true;
        //    this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        //    this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 3);
        //    this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.titleLabel.Location = new System.Drawing.Point(2, 2);
        //    this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
        //    this.titleLabel.Name = "titleLabel";
        //    this.titleLabel.Size = new System.Drawing.Size(717, 18);
        //    this.titleLabel.TabIndex = 31;
        //    this.titleLabel.Text = "CxVision";
        //    this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //    this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
        //    this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
        //    this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
        //    // 
        //    // panel1
        //    // 
        //    this.tableLayoutPanel1.SetColumnSpan(this.panel1, 5);
        //    this.panel1.Controls.Add(this.刷新时间textBox);
        //    this.panel1.Controls.Add(this.label4);
        //    this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.panel1.Location = new System.Drawing.Point(81, 124);
        //    this.panel1.Margin = new System.Windows.Forms.Padding(0);
        //    this.panel1.Name = "panel1";
        //    this.panel1.Size = new System.Drawing.Size(730, 36);
        //    this.panel1.TabIndex = 30;
        //    // 
        //    // buttonMax
        //    // 
        //    this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
        //    this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        //    this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.buttonMax.Location = new System.Drawing.Point(751, 0);
        //    this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
        //    this.buttonMax.Name = "buttonMax";
        //    this.buttonMax.Size = new System.Drawing.Size(30, 22);
        //    this.buttonMax.TabIndex = 33;
        //    this.buttonMax.UseVisualStyleBackColor = true;
        //    this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
        //    // 
        //    // buttonClose
        //    // 
        //    this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
        //    this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        //    this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.buttonClose.Location = new System.Drawing.Point(781, 0);
        //    this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
        //    this.buttonClose.Name = "buttonClose";
        //    this.buttonClose.Size = new System.Drawing.Size(30, 22);
        //    this.buttonClose.TabIndex = 34;
        //    this.buttonClose.UseVisualStyleBackColor = true;
        //    this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
        //    // 
        //    // Form1
        //    // 
        //    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(811, 745);
        //    this.Controls.Add(this.tableLayoutPanel1);
        //    this.Name = "Form1";
        //    this.Text = "自动删除图片";
        //    this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        //    this.Load += new System.EventHandler(this.Form1_Load);
        //    this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
        //    this.groupBox1.ResumeLayout(false);
        //    this.groupBox2.ResumeLayout(false);
        //    this.tabControl1.ResumeLayout(false);
        //    this.删除路径1信息tabPage.ResumeLayout(false);
        //    this.删除路径2信息tabPage.ResumeLayout(false);
        //    this.groupBox5.ResumeLayout(false);
        //    this.groupBox3.ResumeLayout(false);
        //    this.删除路径3信息tabPage.ResumeLayout(false);
        //    this.groupBox6.ResumeLayout(false);
        //    this.groupBox4.ResumeLayout(false);
        //    this.tableLayoutPanel1.ResumeLayout(false);
        //    this.tableLayoutPanel1.PerformLayout();
        //    this.panel1.ResumeLayout(false);
        //    this.panel1.PerformLayout();
        //    this.ResumeLayout(false);

        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && components != null)
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}





        public DeleteImageForm()
        {
            InitializeComponent();
            param = param.Read();
            //文件目录1textBox.Text = param.FolderPath;
            //保存时间textBox.Text = param.SaveTime.ToString();
            //刷新时间textBox.Text = param.UpdataTime.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.文件目录1textBox.DataBindings.Add(nameof(this.文件目录1textBox.Text), param, "FolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件目录2textBox.DataBindings.Add(nameof(this.文件目录2textBox.Text), param, "FolderPath2", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件目录3textBox.DataBindings.Add(nameof(this.文件目录3textBox.Text), param, "FolderPath3", true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存时间textBox.DataBindings.Add(nameof(this.文件目录3textBox.Text), param, "SaveTime", true, DataSourceUpdateMode.OnPropertyChanged);
                this.刷新时间textBox.DataBindings.Add(nameof(this.文件目录3textBox.Text), param, "UpdataTime", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////
                this.start_button_Click(null, null);
            }
            catch
            {

            }
        }

        private void DeleteImage(string path, ConfigParam param)
        {
            string[] DirectoryName = Directory.GetDirectories(path);
            if (DirectoryName != null && DirectoryName.Length != 0)
            {
                string[] array = DirectoryName;
                foreach (string item2 in array)
                {
                    DeleteImage(item2, param);
                }
                return;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DateTime dateTime = directoryInfo.CreationTime;
            int Day = DateTime.Now.Subtract(dateTime).Days;
            if (param.SaveTime > Day)
            {
                return;
            }
            string[] fileName = Directory.GetFiles(path);
            if (fileName == null)
            {
                return;
            }
            string[] array2 = fileName;
            foreach (string item in array2)
            {
                FileInfo fileInfo = new FileInfo(item);
                dateTime = fileInfo.CreationTime;
                Day = DateTime.Now.Subtract(dateTime).Days;
                if (param.SaveTime > Day)
                    continue;
                ////////////////////////////////////////////
                File.Delete(item);
                if (!cts.IsCancellationRequested)
                {
                    Invoke((Action)delegate
                    {
                        this.listBox1?.Items.Clear();
                        this.listBox1?.Items.Add(item);
                    });
                }
                Thread.Sleep(param.UpdataTime);
            }
            if (Directory.Exists(path) && param.FolderPath != path)
            {
                DirectoryName = Directory.GetDirectories(path);
                if (DirectoryName == null || DirectoryName.Length == 0)
                    Directory.Delete(path);
            }
        }

        private void DeleteImage2(string path, ConfigParam param)
        {
            string[] DirectoryName = Directory.GetDirectories(path);
            if (DirectoryName != null && DirectoryName.Length != 0)
            {
                string[] array = DirectoryName;
                foreach (string item2 in array)
                {
                    DeleteImage2(item2, param);
                }
                return;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DateTime dateTime = directoryInfo.CreationTime;
            int Day = DateTime.Now.Subtract(dateTime).Days;
            if (param.SaveTime > Day)
            {
                return;
            }
            string[] fileName = Directory.GetFiles(path);
            if (fileName == null)
            {
                return;
            }
            string[] array2 = fileName;
            foreach (string item in array2)
            {
                FileInfo fileInfo = new FileInfo(item);
                dateTime = fileInfo.CreationTime;
                Day = DateTime.Now.Subtract(dateTime).Days;
                if (param.SaveTime > Day)
                    continue;
                ///////////////////////////////////
                File.Delete(item);
                if (!cts.IsCancellationRequested)
                {
                    Invoke((Action)delegate
                    {
                        this.listBox3?.Items.Clear();
                        this.listBox3?.Items.Add(item);
                    });
                }
                Thread.Sleep(param.UpdataTime);
            }
            if (Directory.Exists(path) && param.FolderPath2 != path)
            {
                DirectoryName = Directory.GetDirectories(path);
                if (DirectoryName == null || DirectoryName.Length == 0)
                    Directory.Delete(path);
            }
        }
        private void DeleteImage3(string path, ConfigParam param)
        {
            string[] DirectoryName = Directory.GetDirectories(path);
            if (DirectoryName != null && DirectoryName.Length != 0)
            {
                string[] array = DirectoryName;
                foreach (string item2 in array)
                {
                    DeleteImage3(item2, param);
                }
                return;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DateTime dateTime = directoryInfo.CreationTime;
            int Day = DateTime.Now.Subtract(dateTime).Days;
            if (param.SaveTime > Day)
            {
                return;
            }
            string[] fileName = Directory.GetFiles(path);
            if (fileName == null)
            {
                return;
            }
            string[] array2 = fileName;
            foreach (string item in array2)
            {
                FileInfo fileInfo = new FileInfo(item);
                dateTime = fileInfo.CreationTime;
                Day = DateTime.Now.Subtract(dateTime).Days;
                if (param.SaveTime > Day)
                    continue;
                /////////////////////////////////
                File.Delete(item);
                if (!cts.IsCancellationRequested)
                {
                    Invoke((Action)delegate
                    {
                        this.listBox5?.Items.Clear();
                        this.listBox5?.Items.Add(item);
                    });
                }
                Thread.Sleep(param.UpdataTime);
            }
            if (Directory.Exists(path) && param.FolderPath3 != path)
            {
                DirectoryName = Directory.GetDirectories(path);
                if (DirectoryName == null || DirectoryName.Length == 0)
                    Directory.Delete(path);
            }
        }
        private void directoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                文件目录1textBox.Text = fold.SelectedPath;
                param.FolderPath = fold.SelectedPath;
            }
            catch
            {
                new Common.UserMessageForm(new Exception().ToString()).ShowDialog();
            }
        }
        private void directory2Button_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                文件目录2textBox.Text = fold.SelectedPath;
                param.FolderPath2 = fold.SelectedPath;
            }
            catch
            {
                new Common.UserMessageForm(new Exception().ToString()).ShowDialog();
            }
        }

        private void directory3Button_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                文件目录3textBox.Text = fold.SelectedPath;
                param.FolderPath3 = fold.SelectedPath;
            }
            catch
            {
                new Common.UserMessageForm(new Exception().ToString()).ShowDialog();
            }
        }
        private void 保存时间textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = 0;
                if (int.TryParse(保存时间textBox.Text, out value))
                {
                    param.SaveTime = value;
                }
            }
            catch
            {
                new Common.UserMessageForm(new Exception().ToString()).ShowDialog();
            }
        }

        private void 刷新时间textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int value = 0;
                if (int.TryParse(刷新时间textBox.Text, out value))
                {
                    param.UpdataTime = value;
                }
            }
            catch
            {
                new Common.UserMessageForm(new Exception().ToString()).ShowDialog();
            }
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            try
            {
                this.start_button.BackColor = System.Drawing.Color.Green;
                this.stop_button.BackColor = System.Drawing.Color.Red;
                cts = new CancellationTokenSource();
                Task.Run(() =>
                {
                    string[] fileName = default(string[]);
                    if (param.FolderPath == null || param.FolderPath.Length == 0) return;
                    while (!cts.IsCancellationRequested && Directory.Exists(param.FolderPath))
                    {
                        fileName = Directory.GetDirectories(param.FolderPath);
                        if (!cts.IsCancellationRequested)
                        {
                            Invoke((Action)delegate
                            {
                                listBox2?.Items.Clear();
                                if (fileName != null && fileName.Length != 0)
                                {
                                    ListBox listBox = listBox2;
                                    if (listBox != null)
                                    {
                                        ListBox.ObjectCollection items = listBox.Items;
                                        object[] items2 = fileName;
                                        items.AddRange(items2);
                                    }
                                }
                                else
                                {
                                    listBox2.Items.Add(param.FolderPath);
                                    //listBox2?.Items.Add("路径中暂没有需要删除的文件");
                                }
                            });
                        }
                        this.DeleteImage(param.FolderPath, param);
                        Thread.Sleep(10 * 60 * 1000);
                    }
                    this.Invoke(new Action(() =>
                    {
                        //this.start_button.BackColor = System.Drawing.Color.LightGray;
                        //this.stop_button.BackColor = System.Drawing.Color.LightGray;
                        this.listBox2.Items.Clear();
                        this.listBox2.Items.Add("指定的目录路径" + param.FolderPath + "为空或路径不存在!");
                        //new UserMessageForm().ShowDialog("指定的目录路径为空或路径不存在!");
                    }));
                });
                //////////////////////////////////
                Task.Run(() =>
                {
                    string[] fileName = default(string[]);
                    if (param.FolderPath2 == null || param.FolderPath2.Length == 0) return;
                    while (!cts.IsCancellationRequested && Directory.Exists(param.FolderPath2))
                    {
                        fileName = Directory.GetDirectories(param.FolderPath2);
                        if (!cts.IsCancellationRequested)
                        {
                            Invoke((Action)delegate
                            {
                                listBox4?.Items.Clear();
                                if (fileName != null && fileName.Length != 0)
                                {
                                    ListBox listBox = listBox4;
                                    if (listBox != null)
                                    {
                                        ListBox.ObjectCollection items = listBox.Items;
                                        object[] items2 = fileName;
                                        items.AddRange(items2);
                                    }
                                }
                                else
                                {
                                    listBox4?.Items.Add(param.FolderPath2);
                                    //listBox4?.Items.Add("路径中暂没有需要删除的文件");
                                }
                            });
                        }
                        this.DeleteImage2(param.FolderPath2, param);
                        Thread.Sleep(10 * 60 * 1000);
                    }
                    this.Invoke(new Action(() =>
                    {
                        //this.start_button.BackColor = System.Drawing.Color.LightGray;
                        //this.stop_button.BackColor = System.Drawing.Color.LightGray;
                        this.listBox4.Items.Clear();
                        this.listBox4.Items.Add("指定的目录路径" + param.FolderPath2 + "为空或路径不存在!");
                        //new UserMessageForm().ShowDialog("指定的目录路径为空或路径不存在!" + param.FolderPath2);
                    }));
                });
                //////////////////////////////////
                Task.Run(() =>
                {
                    string[] fileName = default(string[]);
                    if (param.FolderPath3 == null || param.FolderPath3.Length == 0) return;
                    while (!cts.IsCancellationRequested && Directory.Exists(param.FolderPath3))
                    {
                        fileName = Directory.GetDirectories(param.FolderPath3);
                        if (!cts.IsCancellationRequested)
                        {
                            Invoke((Action)delegate
                            {
                                listBox6?.Items.Clear();
                                if (fileName != null && fileName.Length != 0)
                                {
                                    ListBox listBox = listBox6;
                                    if (listBox != null)
                                    {
                                        ListBox.ObjectCollection items = listBox.Items;
                                        object[] items2 = fileName;
                                        items.AddRange(items2);
                                    }
                                }
                                else
                                {
                                    listBox6?.Items.Add(param.FolderPath3);
                                    //listBox6?.Items.Add("路径中暂没有需要删除的文件");
                                }
                            });
                        }
                        this.DeleteImage3(param.FolderPath3, param);
                        Thread.Sleep(10 * 60 * 1000);
                    }
                    this.Invoke(new Action(() =>
                    {
                        //this.start_button.BackColor = System.Drawing.Color.LightGray;
                        //this.stop_button.BackColor = System.Drawing.Color.LightGray;
                        this.listBox6.Items.Clear();
                        this.listBox6.Items.Add("指定的目录路径" + param.FolderPath3 + "为空或路径不存在!");
                        //new UserMessageForm().ShowDialog("指定的目录路径为空或路径不存在!" + param.FolderPath3);
                    }));
                });
            }
            catch
            {
                start_button_Click(null, null);
            }
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            try
            {
                this.start_button.BackColor = System.Drawing.Color.LightGray;
                this.stop_button.BackColor = System.Drawing.Color.LightGray;
                cts.Cancel();
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                param.Save();
                this.stop_button_Click(null, null);
            }
            catch
            {
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            try
            {
                param.Save();
            }
            catch
            {

            }
        }



        #region  窗体移动功能

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion

        #region  窗体绽放功能 
        private const int Guying_HTLEFT = 10;
        private const int Guying_HTRIGHT = 11;
        private const int Guying_HTTOP = 12;
        private const int Guying_HTTOPLEFT = 13;
        private const int Guying_HTTOPRIGHT = 14;
        private const int Guying_HTBOTTOM = 15;
        private const int Guying_HTBOTTOMLEFT = 0x10;
        private const int Guying_HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 2)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

        #region 防止改变窗口大小时控件闪烁功能
        protected override CreateParams CreateParams   //
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        #endregion

        private void buttonMin_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = new UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            if (dialogResult == DialogResult.OK)
            {
                this.Close();  //关闭窗口
            }
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            Form1_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }





    }
}
