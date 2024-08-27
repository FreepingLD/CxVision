

using FunctionBlock;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using View;
using System.Threading.Tasks;
using System.Threading;


namespace FunctionBlock
{

    public partial class StilLineLaserMonitorForm : Form
    {
        CancellationTokenSource cts; // 提供了标准的协作式取消
        AcqSource _acqSource;
        //ChartForm chartForm1;
        //ChartForm chartForm2;
        ChartView chartHeightView, chartIntensityView;
        public StilLineLaserMonitorForm() //AcqSource acqSource
        {
            // this._acqSource = acqSource;
            InitializeComponent();
            //chartForm1 = new ChartForm();
            //chartForm2 = new ChartForm();
            //AddForm(this.panel1, chartForm1);
            //AddForm(this.panel2, chartForm2);

            this.chartHeightView = new ChartView(this.chart1);
            this.chartHeightView.InitView(this.chart1);
            /////////////////////////////////////
            this.chartIntensityView = new ChartView(this.chart2);
            this.chartIntensityView.InitView(this.chart2);
        }

        public StilLineLaserMonitorForm(AcqSource acqSource) //AcqSource acqSource
        {
            this._acqSource = acqSource;
            InitializeComponent();
            //chartForm1 = new ChartForm();
            //chartForm2 = new ChartForm();
            //AddForm(this.panel1, chartForm1);
            //AddForm(this.panel2, chartForm2);
        }

        /// <summary>
        /// 初始化图表的显示项
        /// </summary>
        private void InitView()
        {
            this.chartHeightView.SetViewParam(enViewParamType.图表标题, "距离");
            this.chartHeightView.SetViewParam(enViewParamType.Y轴标题, "高度");
            this.chartHeightView.SetViewParam(enViewParamType.视图名字, "距离");
            if (this._acqSource != null)
            {
                this.chartHeightView.SetViewParam(enViewParamType.Y轴最大范围, this._acqSource.GetFullScale());
                this.chartHeightView.SetViewParam(enViewParamType.X轴最大范围, this._acqSource.NumPerLine());
            }
            else
                this.chartHeightView.SetViewParam(enViewParamType.Y轴最大范围, 1);
            //////////////////////
            this.chartIntensityView.SetViewParam(enViewParamType.X轴最大范围, this._acqSource.NumPerLine());
            this.chartIntensityView.SetViewParam(enViewParamType.视图名字, "光强");
            this.chartIntensityView.SetViewParam(enViewParamType.图表标题, "光强");
            this.chartIntensityView.SetViewParam(enViewParamType.Y轴标题, "光强");
            this.chartIntensityView.SetViewParam(enViewParamType.Y轴最大范围, 4095);
        }

        public void UpdataView(CancellationToken token)
        {
            Dictionary<enDataItem, object> list;
            double[] dist;
            double[] thick;
            double[] intensity;
            if (_acqSource == null) return;
            //////////////////////////
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    int length = _acqSource.NumPerLine();
                    if (length < 100)
                        continue;
                    list = _acqSource.AcqPointData();
                    dist = (double[])list[enDataItem.Dist1];
                    thick = (double[])list[enDataItem.Thick];
                    intensity = (double[])list[enDataItem.Intensity];
                    //////////////
                    if (!token.IsCancellationRequested)
                    {
                        this.chartHeightView.DisplayObject(0, selectDataRange(dist, length));
                        this.chartIntensityView.DisplayObject(0, selectDataRange(intensity, length));
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }
        }

        private void StilLineLaserMonitorForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            InitView();
            cts = new CancellationTokenSource();
            Task.Run(() => UpdataView(cts.Token));
        }

        private void LineLaserMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cts != null) cts.Cancel();
        }


        private float[] selectDataRange(float[] data, int length)
        {
            float[] dist = new float[length];
            for (int i = 0; i < length; i++)
            {
                if (data == null) break;
                dist[i] = data[i];
            }
            return dist;
        }
        private double[] selectDataRange(double[] data, int length)
        {
            double[] dist = new double[length];
            for (int i = 0; i < length; i++)
            {
                if (data == null) break;
                dist[i] = data[i];
            }
            return dist;
        }
        private void BindProperty()
        {
            try
            {
               // this.采集源comboBox.SelectedItem = this._acqSource.SensoerSN;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 采集源comboBox_TextChanged(object sender, EventArgs e)
        {
           // if (this.采集源comboBox.SelectedItem != null)
               // this._acqSource = this.采集源comboBox.SelectedItem as AcqSource;
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


    }
}
