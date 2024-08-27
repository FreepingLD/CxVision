using Common;
using FunctionBlock;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{

    public partial class DaHengCameraParamForm : Form
    {
        private AcqSource _acqSource;
        private ISensor _sensor;
        private string  savePath;
        private CameraMonitorForm camForm;
        public DaHengCameraParamForm(ISensor sensor)
        {
            InitializeComponent();
            this._sensor = sensor;
        }
        public DaHengCameraParamForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = this._acqSource.Sensor;
            this.camForm = new CameraMonitorForm(this._acqSource);
            this.AddForm(this.监控panel, this.camForm);
        }

        private void 曝光时间texBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.曝光时间texBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    this._sensor.SetParam(enSensorParamType.DaHeng_曝光, value);
                }
            }
        }
        private void 增益texBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.曝光时间texBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    this._sensor.SetParam(enSensorParamType.DaHeng_增益, value);
                }
            }
        }
        private void m_btn_SoftTriggerCommand_Click(object sender, EventArgs e)
        {
            this._sensor.StartTrigger();
        }
        private void DaHengCameraForm_Load(object sender, EventArgs e)
        {
            _initUI();
        }
        private void 触发模式comBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string value = this.触发模式comBox.SelectedItem.ToString();
            try
            {
                this._sensor.SetParam(enSensorParamType.DaHeng_触发模式, value);
            }
            catch
            {

            }
        }
        private void 触发源comBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string value = this.触发源comBox.SelectedItem.ToString();
            try
            {
                this._sensor.SetParam(enSensorParamType.DaHeng_触发源, value);
            }
            catch
            {

            }
        }
        private void 触发极性comBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string value = this.触发极性comBox.SelectedItem.ToString();
            try
            {
                this._sensor.SetParam(enSensorParamType.DaHeng_触发极性, value);
            }
            catch
            {

            }
        }
        private void _initUI()
        {
            string strText;
            try
            {
                double[] expose = (double[])this._sensor.GetParam(enSensorParamType.DaHeng_曝光);
                double[] gain = (double[])this._sensor.GetParam(enSensorParamType.DaHeng_增益);
                string trigMode = this._sensor.GetParam(enSensorParamType.DaHeng_触发模式).ToString();
                string trigSource = this._sensor.GetParam(enSensorParamType.DaHeng_触发源).ToString();
                string Activation = this._sensor.GetParam(enSensorParamType.DaHeng_触发极性).ToString();
                this.曝光时间texBox.Text = expose[0].ToString();
                this.增益texBox.Text = gain[0].ToString();
                this.触发模式comBox.Text = trigMode;
                this.触发源comBox.Text = trigSource;
                this.触发极性comBox.Text = Activation;
                strText = string.Format("曝光时间({0}~{1})", expose[1].ToString("0.00"), expose[2].ToString("0.00"));
                m_lbl_Shutter.Text = strText;
                strText = string.Format("增益({0}~{1})", gain[1].ToString("0.00"), gain[2].ToString("0.00"));
                m_lbl_Gain.Text = strText;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void DaHengCameraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.camForm != null)
            {
                this.camForm.Close();
                this.camForm.Dispose();
            }
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

    }
}
