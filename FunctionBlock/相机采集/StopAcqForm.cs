using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class StopAcqForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        //private AutoReSizeFormControls arfc = new AutoReSizeFormControls();
        private VisualizeView drawObject;
        private TreeNode _refNode;
        private AcqSource acqSource;

        public StopAcqForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, true);
        }
        public StopAcqForm(IFunction function, TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, true);
        }
        private void StopAcqForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.采集源comboBox.DataSource = AcqSourceManage.Instance.GetAcqSourceName();
                this.采集源comboBox.DataBindings.Add("Text", ((StopAcq)this._function), "AcqSourceName", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
                this.灰度值1Label.Text = e.GaryValue[0].ToString();
            else
                this.灰度值1Label.Text = 0.ToString();
            ///////////////////////////////////////////
            if (e.GaryValue.Length > 1)
                this.灰度值2Label.Text = e.GaryValue[1].ToString();
            else
                this.灰度值2Label.Text = 0.ToString();
            /////////////////////////////////////////
            if (e.GaryValue.Length > 2)
                this.灰度值3Label.Text = e.GaryValue[2].ToString();
            else
                this.灰度值3Label.Text = 0.ToString();
            ///////////////////////////////////////////////
            this.行坐标Label.Text = e.Row.ToString();
            this.列坐标Label.Text = e.Col.ToString();
        }
        private void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }
        private void AddForm(GroupBox groupBox, Form form)
        {
            if (groupBox == null) return;
            if (groupBox.Controls.Count > 0)
                groupBox.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            groupBox.Controls.Add(form);
            form.Show();
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
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                                }));
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

        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void StopAcqForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.采集源comboBox.SelectedItem.ToString());
                if (this.acqSource != null)
                {
                    this.acqSource.Sensor.CameraParam.Save();
                }
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            }
            catch
            {

            }
        }

        private void 相机采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (采集源comboBox.SelectedIndex == -1) return;
                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(((StopAcq)this._function).AcqSourceName);
                //this.acqSource = ((ImageAcq)this._function).AcqSourceName;
                if (this.acqSource == null) return;
                this.采集源comboBox.Text = this.acqSource.Name;
                switch (this.acqSource.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        AddForm(this.panel1, new LaserParamForm(this.acqSource.Sensor.LaserParam));
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        AddForm(this.panel1, new CameraParamForm(this.acqSource.Sensor.CameraParam, ((StopAcq)this._function).AcqParam));
                        break;
                    default:
                        AddForm(this.panel1, new CameraParamForm(null));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 采集源comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (采集源comboBox.SelectedIndex == -1) return;
                if (采集源comboBox.SelectedItem == null) return;
                ((StopAcq)this._function).AcqSourceName = this.采集源comboBox.SelectedItem.ToString();
                //this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.采集源comboBox.SelectedItem.ToString()); // 这个事件发生后才会触发 SelectedIndexChanged 事件 
                //if (this.acqSource != null)
                //    ((ImageAcq)this._function).AcqSourceName = this.acqSource.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }






    }
}
