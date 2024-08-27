using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sensor;
using AlgorithmsLibrary;
using HalconDotNet;
using FunctionBlock;
using Common;


namespace FunctionBlock
{
    public partial class LiYiPointLaserParamForm : Form
    {
        private FunctionBlock.AcqSource _acqSource;
        private ISensor _sensor;
        private FileOperate fo = new FileOperate();
        private LiyiPointSensorSetting liyiPointParamConfig;
        private string savePath;
        private PointLaserForm pointForm;
        public LiYiPointLaserParamForm(ISensor sensor)
        {
            InitializeComponent();
            this._sensor = sensor;
            this.savePath = Application.StartupPath + "\\" + "LaserParam" + "\\" + this._sensor.GetParam(enSensorParamType.Coom_传感器名称).ToString() + ".txt";
            if (fo.ReadConfigParam(this.savePath) != null)
                liyiPointParamConfig = (LiyiPointSensorSetting)fo.ReadConfigParam(this.savePath);
            else
                liyiPointParamConfig = new LiyiPointSensorSetting();
        }
        public LiYiPointLaserParamForm(FunctionBlock.AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = acqSource.Sensor;
            this.pointForm = new PointLaserForm(this._acqSource);
            this.AddForm(this.监控panel, this.pointForm);
            this.savePath = Application.StartupPath + "\\" + "LaserParam" + "\\" + this._sensor.GetParam(enSensorParamType.Coom_传感器名称).ToString() + ".txt";
            if (fo.ReadConfigParam(this.savePath) != null)
                liyiPointParamConfig = (LiyiPointSensorSetting)fo.ReadConfigParam(this.savePath);
            else
                liyiPointParamConfig = new LiyiPointSensorSetting();
        }
        private void 采集频率textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int value = int.Parse(this.采集频率textBox.Text);
                    _sensor.SetParam(enSensorParamType.Liyi_采集频率, value);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 曝光时间textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int value = int.Parse(this.曝光时间textBox.Text);
                    _sensor.SetParam(enSensorParamType.Liyi_曝光, value);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 触发模式Combox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                //enTrigMode value = (enTrigMode)(this.触发模式Combox.SelectedIndex);
                //_sensor.SetParam(enLiyiParamType.触发模式, value);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void LiYiPointLaserParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 填充下拉表
                //this.触发模式Combox.DataSource = Enum.GetNames(typeof(enTrigMode));
                //this.触发源comboBox.DataSource = Enum.GetNames(typeof(enTrigSource));
                this.测量模式comboBox.DataSource = Enum.GetNames(typeof(enUserMeasureMode));
                this.峰值模式comboBox.DataSource = Enum.GetNames(typeof(enUserPeakMode));
                this.厚度层峰1选择comboBox.DataSource = Enum.GetNames(typeof(enUserPeakMode));
                this.厚度层峰2选择comboBox.DataSource = Enum.GetNames(typeof(enUserPeakMode));
                // 获取控制器参数
                this.采集频率textBox.Text = _sensor.GetParam(enSensorParamType.Liyi_采集频率).ToString(); // 采集频率的优先级高于曝光
                this.曝光时间textBox.Text = _sensor.GetParam(enSensorParamType.Liyi_曝光).ToString();
                this.增益textBox.Text = _sensor.GetParam(enSensorParamType.Liyi_增益).ToString();
                this.测量范围textBox.Text = _sensor.GetParam(enSensorParamType.Liyi_测量范围).ToString();
                this.标准厚度textBox.Text = _sensor.GetParam(enSensorParamType.Liyi_标准厚度).ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 增益textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int value = int.Parse(this.增益textBox.Text);
                    _sensor.SetParam(enSensorParamType.Liyi_增益, value);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void SaveConfigbutton_Click(object sender, EventArgs e)
        {
            try
            {
                fo.SaveConfigParam(this.savePath, this.liyiPointParamConfig);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void DarkAcqbutton_Click(object sender, EventArgs e)
        {
            try
            {
                _sensor.GetParam(enSensorParamType.Liyi_暗黑校正);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void LiYiPointLaserParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.pointForm != null)
                {
                    this.pointForm.Close();
                    this.pointForm.Dispose();
                }             
                fo.SaveConfigParam(this.savePath, this.liyiPointParamConfig);
            }
            catch
            {

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
