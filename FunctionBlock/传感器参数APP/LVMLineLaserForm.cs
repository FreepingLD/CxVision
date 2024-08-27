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

    public partial class LVMLineLaserParamForm : Form
    {
        private AcqSource _acqSource;
        private ISensor _sensor;
        private string savePath;
        private LineLaserForm lineForm;
        public LVMLineLaserParamForm(ISensor sensor)
        {
            InitializeComponent();
            this._sensor = sensor;
            this.savePath = this.savePath = Application.StartupPath + "\\" + "StilLaserParam" + "\\" + this._sensor.GetParam("传感器名称").ToString() + ".txt";
        }
        public LVMLineLaserParamForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = acqSource.Sensor;
            this.lineForm = new LineLaserForm(this._acqSource);
            this.AddForm(this.监控panel,this.lineForm);
            this.savePath = this.savePath = Application.StartupPath + "\\" + "StilLaserParam" + "\\" + this._sensor.GetParam("传感器名称").ToString() + ".txt";
        }
        private void LVMLinetLaserForm_Load(object sender, EventArgs e)
        {
          //  _initUI();
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
        private void _initUI()
        {
            try
            {
                this.曝光模式comboBox.DataSource = Enum.GetNames(typeof(enUserExpourseMode));
                this.峰值模式comboBox.DataSource = Enum.GetNames(typeof(enUserPeakMode2));
                this.测量模式comboBox.DataSource = Enum.GetNames(typeof(enMeasureMode));
                this.折射率模式comboBox.DataSource = Enum.GetNames(typeof(enUserRefractiveMode));
                this.触发模式comBox.DataSource = Enum.GetNames(typeof(enTrigMode));
                ////////////////
                RAMSettings_s settings=(RAMSettings_s)this._sensor.GetParam(enPropId.RAMSettings);
                if (settings.exposure == 0)
                    this.曝光模式comboBox.Text = enUserExpourseMode.Auto.ToString();
                else
                    this.曝光模式comboBox.Text = enUserExpourseMode.Manual.ToString();
                //////////
                if(settings.firstPeakMode==0)
                    this.峰值模式comboBox.Text = enUserPeakMode2.StrongestPeak.ToString();
                else
                    this.峰值模式comboBox.Text = enUserPeakMode2.FirstPeak.ToString();
                ///////
                if (settings.measureMode == 1)
                    this.测量模式comboBox.Text = enMeasureMode.Distance.ToString();
                else
                    this.测量模式comboBox.Text = enMeasureMode.Thickness.ToString();
                ////////////
                if (settings.refractiveIndexMode == 0)
                    this.折射率模式comboBox.Text = enUserRefractiveMode.自定义.ToString();
                else
                    this.折射率模式comboBox.Text = settings.refractiveIndexMode.ToString();
                /////////////
                if (settings.trigMode == 1)
                    this.触发模式comBox.Text = enTrigMode.Software.ToString();
                else
                    this.触发模式comBox.Text = enTrigMode.Hardware.ToString();
                ///////////////////////////////////////////////
                this.曝光时间texBox.Text = settings.exposure.ToString();
                this.增益texBox.Text = settings.pgaGain.ToString();
                this.光源亮度textBox.Text = settings.ledValue.ToString();
                this.检测阈值textBox.Text = settings.thresholdValue.ToString();
                this.平均值textBox.Text = settings.averageValue.ToString();
                this.移动平滑系数textBox.Text = settings.inertiaAverage.ToString();
                this.折射率textBox.Text = settings.refractiveIndex.ToString();
                this.测量点数textBox.Text = settings.storedMeasureValuesCount.ToString();
                this.触发周期textBox.Text = settings.trigPeriod.ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void BoMingPointLaserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._sensor.SetParam(enPropId.SaveSettings, null);
        }

        private void 曝光模式comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (this.曝光模式comboBox.SelectedItem.ToString())
            {
                case "Auto":
                    if (this._sensor.SetParam(enPropId.Exposure, 0))
                    {                       
                        this.曝光时间texBox.ReadOnly = true;
                        MessageBox.Show("设置成功");
                    }
                    else
                        MessageBox.Show("设置失败");
                    break;
                case "Manual":
                    this._sensor.SetParam(enPropId.Exposure, double.Parse(this.曝光时间texBox.Text)); //改为手动时要设置一次曝光参数
                    this.曝光时间texBox.ReadOnly = false;
                    break;
            }
        }

        private void 曝光时间texBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.曝光时间texBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.Exposure, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 增益texBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.增益texBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.PGAGain, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 光源亮度textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.光源亮度textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.LedValue, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 峰值模式comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (this.峰值模式comboBox.SelectedItem.ToString())
            {
                case "FirstPeak":
                    if (this._sensor.SetParam(enPropId.FirstPeakMode, enFirstPeakMode.Enabled))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                    break;
                case "StrongestPeak":
                    if (this._sensor.SetParam(enPropId.FirstPeakMode, enFirstPeakMode.Disabled))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                    break;
            }
        }

        private void 测量模式comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (this.测量模式comboBox.SelectedItem.ToString())
            {
                case "Distance":
                    if (this._sensor.SetParam(enPropId.MeasureMode, enMeasureMode.Distance))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                    break;
                case "Thickness":
                    if (this._sensor.SetParam(enPropId.MeasureMode, enMeasureMode.Thickness))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                    break;
            }
        }

        private void 检测阈值textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.检测阈值textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.ThresholdValue, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 平均值textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.平均值textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.AverageValue, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 移动平滑系数textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.移动平滑系数textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.InertiaAverage, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 折射率模式comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (this.折射率模式comboBox.SelectedItem.ToString())
            {
                case "自定义":
                    if (this._sensor.SetParam(enPropId.RefractiveIndexMode, 0))
                    {
                        this.折射率textBox.ReadOnly = false;
                        MessageBox.Show("设置成功");
                    }                                  
                    else
                    {
                        this.折射率textBox.ReadOnly = true;
                        MessageBox.Show("设置失败");
                    }                               
                    break;
                default:
                    if (this._sensor.SetParam(enPropId.RefractiveIndexMode, int.Parse(this.折射率模式comboBox.SelectedIndex.ToString())))
                    {
                        this.折射率textBox.ReadOnly = true;
                        MessageBox.Show("设置成功");
                    }
                    else
                    {
                        this.折射率textBox.ReadOnly = false;
                        MessageBox.Show("设置失败");
                    }
                    break;
            }
        }

        private void 折射率textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.折射率textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.RefractiveIndexValue, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 测量点数textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.测量点数textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.StoredMeasureValuesCount, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }

        private void 触发模式comBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (this.折射率模式comboBox.SelectedItem.ToString())
            {
                case "Software":
                    if (this._sensor.SetParam(enPropId.TrigMode, enTrigMode.Software))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");

                    break;
                case "Hardware":
                    if (this._sensor.SetParam(enPropId.TrigMode, enTrigMode.Hardware))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                    break;
            }
        }

        private void 触发周期textBox_KeyUp(object sender, KeyEventArgs e)
        {
            double value;
            bool result = double.TryParse(this.触发周期textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                if (result)
                {
                    if (this._sensor.SetParam(enPropId.TrigPeriod, value))
                        MessageBox.Show("设置成功");
                    else
                        MessageBox.Show("设置失败");
                }
            }
        }


    }
}
