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


namespace FunctionBlock
{
    public partial class BoMingPointLaserMonitorForm : Form
    {
        private CancellationTokenSource cts; // 提供了标准的协作式取消
        private AcqSource _acqSource;
        public BoMingPointLaserMonitorForm()
        {
            InitializeComponent();
        }
        public BoMingPointLaserMonitorForm(AcqSource acqSource)
        {
            this._acqSource = acqSource;
            InitializeComponent();
        }
        private void PointLaserMonitorForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            cts = new CancellationTokenSource();
            Task.Run(() => UpdataView(cts.Token));
        }

        private void PointLaserMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cts != null) cts.Cancel();
        }
        public void UpdataView(CancellationToken token)
        {
            Dictionary<enDataItem, object> list;
            double[] dist;
            double[] thick;
            double[] intensity;
            if (_acqSource == null) return;
            ///////////////////////////////
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    int length = _acqSource.NumPerLine();
                    if (length != 1)
                        continue;
                    list = _acqSource.AcqPointData();
                    dist = (double[])list[enDataItem.Dist1];
                    thick = (double[])list[enDataItem.Thick];
                    intensity = (double[])list[enDataItem.Intensity];
                    //////////////
                    if (dist != null && dist.Length>0)
                        this.距离textBox.Invoke(new Action(() => this.距离textBox.Text = dist[0].ToString()));
                    else
                        this.距离textBox.Invoke(new Action(() => this.距离textBox.Text = "……"));
                    if (thick != null && thick.Length > 0)
                        this.厚度textBox.Invoke(new Action(() => this.厚度textBox.Text = thick[0].ToString()));
                    else
                        this.厚度textBox.Invoke(new Action(() => this.厚度textBox.Text = "……"));
                    if (intensity != null && intensity.Length > 0)
                        this.光强textBox.Invoke(new Action(() => this.光强textBox.Text = intensity[0].ToString()));
                    else
                        this.光强textBox.Invoke(new Action(() => this.光强textBox.Text = "……"));
                    Thread.Sleep(100);
                }
            }
            catch
            {

            }
        }
        private void BindProperty()
        {
            try
            {
               // this.采集源comboBox.DataSource = GetAcqSource();
                //this.采集源comboBox.DisplayMember = "Name";
                //////////////////
               // this.采集源comboBox.SelectedIndex = 0;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 采集源comboBox_TextChanged(object sender, EventArgs e)
        {
            //if (this.采集源comboBox.SelectedItem != null)
               // this._acqSource = this.采集源comboBox.SelectedItem as AcqSource;
        }


    }
}
