using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sensor;
using View;
using FunctionBlock;
using System.Threading;
using System.Threading.Tasks;
using Common;
using HalconDotNet;

namespace FunctionBlock
{
    public partial class StilPointLaserMonitorForm : Form
    {
        //CancellationTokenSource cts; // 提供了标准的协作式取消
        //AcqSource _acqSource;
        public StilPointLaserMonitorForm()
        {
            InitializeComponent();
        }
        public StilPointLaserMonitorForm(AcqSource acqSource)
        {
            //this._acqSource = acqSource;
            InitializeComponent();
        }
        private void PointLaserMonitorForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            BaseFunction.PointsCloudAcqComplete += new PointCloudAcqCompleteEventHandler(DisplayMeasureResult);
            //cts = new CancellationTokenSource();
            //Task.Run(() => UpdataView(cts.Token));
        }
        // 更新3D对象模型 ；响应测量完成/及响应鼠标点击事件
        public void DisplayMeasureResult(object send,PointCloudAcqCompleteEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.PointsCloudData != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    HTuple z = e.PointsCloudData.GetObjectModel3dParams("point_coord_z");
                    if (AcqSourceManage.Instance.LaserAcqSourceList().Count > 0)
                    {
                        if (AcqSourceManage.Instance.LaserAcqSourceList()[0].Name == e.SensorName)
                        {
                            if (z != null && z.Length > 0)
                                this.激光1距离textBox.Invoke(new Action(() => this.激光1距离textBox.Text = z[0].D.ToString()));
                            else
                                this.激光1距离textBox.Invoke(new Action(() => this.激光1距离textBox.Text = "……"));
                        }
                    }
                    if (AcqSourceManage.Instance.LaserAcqSourceList().Count > 1)
                    {
                        if (AcqSourceManage.Instance.LaserAcqSourceList()[1].Name == e.SensorName)
                        {
                            if (z != null && z.Length > 0)
                                this.激光2距离textBox.Invoke(new Action(() => this.激光2距离textBox.Text = z[0].D.ToString()));
                            else
                                this.激光2距离textBox.Invoke(new Action(() => this.激光2距离textBox.Text = "……"));
                        }
                    }
                }
            }
            catch (Exception he)
            {
                //LoggerHelper.Warn("显示3D对象错误", he);
            }
        }
        private void PointLaserMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            BaseFunction.PointsCloudAcqComplete -= new PointCloudAcqCompleteEventHandler(DisplayMeasureResult);
            //if (cts != null) cts.Cancel();
        }
        public void UpdataView(CancellationToken token)
        {
            //List<object> list;
            //double[] dist1, dist2, thick, intensity;
            //if (_acqSource == null) return;
            /////////////////////////////////
            //try
            //{
            //    while (true)
            //    {
            //        token.ThrowIfCancellationRequested();
            //        int length = _acqSource.NumPerLine();
            //        if (length != 1)
            //            continue;
            //        list = _acqSource.AcqPointData();
            //        dist1 = (double[])list[0];
            //        dist2 = (double[])list[1];
            //        thick = (double[])list[2];
            //        intensity = (double[])list[3];
            //        //////////////
            //        if (dist1 != null && dist1.Length>0)
            //            this.距离1textBox.Invoke(new Action(() => this.距离1textBox.Text = dist1[0].ToString()));
            //        else
            //            this.距离1textBox.Invoke(new Action(() => this.距离1textBox.Text = "……"));
            //        if (dist2 != null && dist2.Length > 0)
            //            this.距离2textBox.Invoke(new Action(() => this.距离2textBox.Text = dist2[0].ToString()));
            //        else
            //            this.距离2textBox.Invoke(new Action(() => this.距离2textBox.Text = "……"));
            //        if (thick != null && thick.Length > 0)
            //            this.厚度textBox.Invoke(new Action(() => this.厚度textBox.Text = thick[0].ToString()));
            //        else
            //            this.厚度textBox.Invoke(new Action(() => this.厚度textBox.Text = "……"));
            //        if (intensity != null && intensity.Length > 0)
            //            this.光强textBox.Invoke(new Action(() => this.光强textBox.Text = intensity[0].ToString()));
            //        else
            //            this.光强textBox.Invoke(new Action(() => this.光强textBox.Text = "……"));
            //        Thread.Sleep(100);
            //    }
            //}
            //catch
            //{

            //}
        }
        private void BindProperty()
        {
            try
            {

                // this.采集源comboBox.Text =  this._acqSource.SensoerSN;
                //this.采集源comboBox.DisplayMember = "Name";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }



    }
}
