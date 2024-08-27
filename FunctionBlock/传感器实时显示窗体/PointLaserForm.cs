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
    public partial class PointLaserForm : Form
    {
        CancellationTokenSource cts; // 提供了标准的协作式取消
        AcqSource _acqSource;
        public PointLaserForm()
        {
            InitializeComponent();
        }
        public PointLaserForm(AcqSource acqSource)
        {
            this._acqSource = acqSource;
            InitializeComponent();
        }
        private void PointLaserForm_Load(object sender, EventArgs e)
        {
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
            double[] dist1, dist2;
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
                    dist1 = (double[])list[enDataItem.Dist1];
                    dist2 = (double[])list[enDataItem.Dist2];
                    thick = (double[])list[enDataItem.Thick];
                    intensity = (double[])list[enDataItem.Intensity];
                    //////////////
                    if (dist1 != null && dist1.Length>0)
                        this.距离1textBox.Invoke(new Action(() => this.距离1textBox.Text = dist1[0].ToString()));
                    else
                        this.距离1textBox.Invoke(new Action(() => this.距离1textBox.Text = "……"));
                    if (dist2 != null && dist2.Length > 0)
                        this.距离2textBox.Invoke(new Action(() => this.距离2textBox.Text = dist2[0].ToString()));
                    else
                        this.距离2textBox.Invoke(new Action(() => this.距离2textBox.Text = "……"));
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

    }
}
