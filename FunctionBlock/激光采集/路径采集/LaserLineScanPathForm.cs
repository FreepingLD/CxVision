using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class LaserLineScanPathForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private bool isFormClose = false;
        public LaserLineScanPathForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node, 1);;
        }
        private void LaserLineScanPathForm_Load(object sender, EventArgs e)
        {
            BaseFunction.PointsCloudAcqComplete += new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                ////////////////
                this.激光采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList();//SensorManage.LaserList;
                this.激光采集源comboBox.DisplayMember = "Name";
                this.运动类型comboBox.DataSource = Enum.GetNames(typeof(enAxisName));
                //this.运动类型comboBox.DisplayMember = "Key";
                //////////////////
                this.激光采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.LaserScanAcqPath)this._function), "LaserAcqSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.运动类型comboBox.DataBindings.Add("Text", (FunctionBlock.LaserScanAcqPath)this._function, "MotionType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.扫描高度textBox.DataBindings.Add("Text", (FunctionBlock.LaserScanAcqPath)_function, "ScanHeight", true, DataSourceUpdateMode.OnPropertyChanged); //    
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FunctionBlock.LaserScanAcqPath.enShowItems item;
            Enum.TryParse(this.显示条目comboBox.Text.Trim(), out item);
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        this.cts = new CancellationTokenSource();
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.toolStripStatusLabel1.Text = "执行结果:";
                                        this.toolStripStatusLabel2.Text = "成功";
                                        this.toolStripStatusLabel2.ForeColor = Color.Green;
                                    }));
                                }
                            }
                            else
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                   {
                                       this.toolStripStatusLabel1.Text = "执行结果:";
                                       this.toolStripStatusLabel2.Text = "失败";
                                       this.toolStripStatusLabel2.ForeColor = Color.Red;
                                   }));
                                }
                            }
                        }
                        );
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawObject.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawObject.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Translate":
                    this.drawObject.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawObject.AutoImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case "toolStripButton_3D":
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
            }
        }

        private void PointCloudAcqComplete_Event(object send, PointCloudAcqCompleteEventArgs e)
        {
            if (((FunctionBlock.LaserScanAcqPath)this._function).LaserAcqSource == null) return;
            string name = ((FunctionBlock.LaserScanAcqPath)this._function).LaserAcqSource.Sensor.ConfigParam.SensorName;
            switch (e.SensorName.Split('(')[0])
            {
                case "读取3D对象":
                    if (this.drawObject != null)
                        this.drawObject.PointCloudModel3D = new PointCloudData( e.PointsCloudData );
                    break;
                default:
                    if (name == e.SensorName) // 因为是用同一个事件来发送图像，所以这里使用名字来判断是不是同一个相机发送的
                    {
                        if (this.drawObject != null)
                            this.drawObject.PointCloudModel3D = new PointCloudData(e.PointsCloudData);
                    }
                    break;
            }
        }

        private void LaserLineScanPathForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cts != null)
                    cts.Cancel();
                BaseFunction.PointsCloudAcqComplete -= new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
            }
            catch
            {

            }
        }

        private void 记录高度button_Click(object sender, EventArgs e)
        {
            try
            {
                double Z;
                ((FunctionBlock.LaserScanAcqPath)this._function).LaserAcqSource.Card.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Z轴, out Z);
                this.扫描高度textBox.Text = Z.ToString();
                ((FunctionBlock.LaserScanAcqPath)this._function).ScanHeight = Z;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }



    

    }
}
