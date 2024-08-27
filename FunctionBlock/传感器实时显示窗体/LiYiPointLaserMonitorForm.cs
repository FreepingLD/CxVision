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
    public partial class LiYiPointLaserMonitorForm : Form
    {
        CancellationTokenSource cts; // 提供了标准的协作式取消
        AcqSource _acqSource;
        public LiYiPointLaserMonitorForm()
        {
            InitializeComponent();
        }
        public LiYiPointLaserMonitorForm(AcqSource acqSource)
        {
            this._acqSource = acqSource;
            InitializeComponent();
        }
        private void LiYiPointLaserMonitorForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            cts = new CancellationTokenSource();
            Task.Run(() => UpdataView(cts.Token));
        }

        private void LiYiPointLaserMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
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
                //this.采集源comboBox.DataSource = GetAcqSource();
                //this.采集源comboBox.DisplayMember = "Name";
                //////////////////
               // this.采集源comboBox.SelectedIndex = 0;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        //private ProgramForm getProgramForm()
        //{
        //    foreach (var item in Application.OpenForms) // 获取所有窗口来遍历
        //    {
        //        if (item is ProgramForm)
        //        {
        //            return item as ProgramForm;
        //        }
        //    }
        //    return null;
        //}
        //private AcqSource[] GetAcqSource()
        //{
        //    //AcqSource[] source;
        //    //ProgramForm _up = getProgramForm();
        //    //TreeNode[] nodes = _up.treeView1.Nodes.Find(_up.treeView1.Nodes[0].Name, false);
        //    //source = new AcqSource[nodes.Length];
        //    //for (int i = 0; i < nodes.Length; i++)
        //    //{
        //    //    source[i] = (AcqSource)nodes[i].Tag;
        //    //}
        //    //return source;
        //}
        private void 采集源comboBox_TextChanged(object sender, EventArgs e)
        {
            //if (this.采集源comboBox.SelectedItem != null)
               // this._acqSource = this.采集源comboBox.SelectedItem as AcqSource;
        }


    }
}
