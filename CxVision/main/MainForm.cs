using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CxVision.算子App;
using CxVision.App;
using Sensor;
using Common;
using MotionControlCard;
using System.Threading;
using System.Threading.Tasks;
using userControl;

namespace CxVision
{
    public partial class MainForm : Form
    {
        private ProgramForm pf;
        private ViewForm df;
        private MotionControlForm mcf;
        private SensorSource sensorlist = new SensorSource();
        private MotionCardSource card = new MotionCardSource();
        private string programPath = ""; // 程序文件路径
        private CancellationTokenSource cts;
        public MainForm()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            // new MotionForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tool tool = new Tool();
            tool.Owner = this;
            tool.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            // fo.SaveConfigParam("ParamConfig.txt",new ParamConfig());
            GlobalVariable.pConfig = (Common.ParamConfig)fo.ReadConfigParam("ParamConfig.txt");
            ////////////////////////////////////////////////////////////////////////////
            sensorlist.Connect();
            card.Connect();
            /////////////////////////
            pf = new ProgramForm(); // 
            AddForm(this.tableLayoutPanel1, pf, 1, 0, 3, 1);
           // pf.BindTarget(this.panel1);
            mcf = new MotionControlForm();
            AddForm(this.tableLayoutPanel1, mcf, 4, 0, 1, 2);
            df = new ViewForm();
            AddForm(this.tableLayoutPanel1, df, 0, 2, 5, 3);
        }


        public void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }


        public void AddForm(TableLayoutPanel MastPanel, Form form, int rowPose, int colPose, int rowSpan, int colSpan)
        {
            //if (MastPanel.Controls.Count > 0)
            //    MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            MastPanel.SetRow(form, rowPose);
            MastPanel.SetColumn(form, colPose);
            MastPanel.SetRowSpan(form, rowSpan);
            MastPanel.SetColumnSpan(form, colSpan);
            form.Show();
        }

        private void 工具toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name)
            {
                case "检测工具":
                    form = new Tool();
                    form.Owner = this;
                    form.Show();
                    break;
                case "对射标定工具":
                    form = new CalibrateDoubleLaserForm();
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void 传感器配置toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new SensorConfig();
            form.Owner = this;
            form.Show();
        }

        private void 配置toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name.Trim())
            {
                case "传感器配置":
                    form = new SensorConfig();
                    form.Owner = this;
                    form.Show();
                    break;
                case "参数设置":
                    form = new paramConfigForm();
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.sensorlist.DisConnect();
        }

        private void 编辑工具条toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            FileOperate fo = new FileOperate();
            switch (name)
            {
                case "新建NToolStripButton":
                    this.programPath = "";
                    this.pf.ClearTreeView();
                    break;

                case "打开OToolStripButton":
                    this.programPath = fo.OpenFile(2);
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    List<TreeNode> program = fo.OpenProgram(this.programPath);
                    this.pf.LoadTreeViewNode(program);
                    break;

                case "保存SToolStripButton":
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        this.programPath = fo.SaveFile(2);
                        if (fo.SaveProgram(this.programPath, this.pf.GetTreeViewNode()))
                            MessageBox.Show("保存成功");
                    }
                    else
                    {
                        if (fo.SaveProgram(this.programPath, this.pf.GetTreeViewNode()))
                            MessageBox.Show("保存成功");
                    }
                    break;

                case "打印PToolStripButton":

                    break;

                case "剪切UToolStripButton":

                    break;

                case "复制CToolStripButton":

                    break;

                case "粘贴PToolStripButton":

                    break;

                case "帮助LToolStripButton":

                    break;

                default:
                    break;
            }
        }

        private bool isRun = false;
        private void 运行工具条toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            int count = int.Parse(循环次数toolStripTextBox.Text);
            switch (name)
            {
                case "运行toolStripButton":
                    run(count);
                    break;
                case "停止toolStripButton":
                    this.pf.Stop();
                    break;
                default:
                    break;
            }
        }
        private async void run(int count) // async
        {
            if (isRun) return;
            this.isRun = true;
            await Task.Run(() => this.pf.Run(count));
            this.isRun = false;
        }
        private void 调试toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name.Trim())
            {
                case "查看点激光":
                    form = new PointLaserMonitorForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "查看线激光":
                    form = new LineLaserMonitorForm();
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }
    }
}
