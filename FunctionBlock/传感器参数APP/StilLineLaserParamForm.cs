using Common;
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

namespace FunctionBlock
{
    public partial class StilLineLaserParamForm : Form
    {
        private AcqSource _acqSource;
        private ISensor _sensor;
        private FileOperate fo = new FileOperate();
        private StilLineSensorSetting stilLineParamConfig;
        private string savePath;
        private LineLaserForm lineForm;

        public StilLineLaserParamForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = this._acqSource.Sensor;
            this.lineForm = new LineLaserForm(this._acqSource);
            this.AddForm(this.监控panel,this.lineForm);
            this.savePath = Application.StartupPath + "\\" + "LaserParam" + "\\" + this._sensor.ConfigParam.SensorName + ".txt";
            this.stilLineParamConfig = ((CStil_L)this._sensor).StilLineConfigParam;
        }
        private void LEDtextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int value = int.Parse(this.LEDtextBox.Text);
                    _sensor.SetParam(enSensorParamType.Stil_光源亮度, value);
                    stilLineParamConfig.Stil_LedBrightness = value;
                    MessageBox.Show("设置成功");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void ExposeTimetextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int value = int.Parse(this.ExposeTimetextBox.Text);
                    _sensor.SetParam(enSensorParamType.Stil_曝光, value);
                    stilLineParamConfig.Stil_ExposureTime = value;
                    MessageBox.Show("设置成功");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void DetectiontextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    double value = double.Parse(this.DetectiontextBox.Text);
                    _sensor.SetParam(enSensorParamType.Stil_检测阈值, value);
                    stilLineParamConfig.Stil_DetectionThreshold = value;
                    MessageBox.Show("设置成功");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void TriggerModeCombox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string trigmode = this.TriggerModeCombox.SelectedItem.ToString() + ";" + this.TRENumtextBox.Text;
                _sensor.SetParam(enSensorParamType.Stil_触发模式, trigmode);
                enUserTrigerMode TrigerMode;
                if (Enum.TryParse(trigmode, out TrigerMode))
                    stilLineParamConfig.TrigMode = TrigerMode;
                MessageBox.Show("设置成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void MeasureModecomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string Measuregmode = this.MeasureModecomboBox.SelectedItem.ToString();
                _sensor.SetParam(enSensorParamType.Stil_测量模式, Measuregmode);
                enUserMeasureMode MeasureMode;
                if (Enum.TryParse(Measuregmode, out MeasureMode))
                    stilLineParamConfig.MeasureMode = MeasureMode;
                MessageBox.Show("设置成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void AltitudePeakcomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string peakMode = this.AltitudePeakcomboBox.SelectedItem.ToString();
                switch(peakMode)
                {
                    case "FirstPeak":
                        _sensor.SetParam(enSensorParamType.Stil_高度模式的峰值选择, enUserPeakMode.FirstPeak);
                        stilLineParamConfig.Stil_AltitudeModePeak = enUserPeakMode.FirstPeak;
                        break;
                    case "StrongestPeak":
                        _sensor.SetParam(enSensorParamType.Stil_高度模式的峰值选择, enUserPeakMode.StrongestPeak);
                        stilLineParamConfig.Stil_AltitudeModePeak = enUserPeakMode.StrongestPeak;
                        break;
                    case "SecondPeak":
                        _sensor.SetParam(enSensorParamType.Stil_高度模式的峰值选择, enUserPeakMode.SecondPeak);
                        stilLineParamConfig.Stil_AltitudeModePeak = enUserPeakMode.SecondPeak;
                        break;
                    case "ThreePeak":
                        _sensor.SetParam(enSensorParamType.Stil_高度模式的峰值选择, enUserPeakMode.ThreePeak);
                        stilLineParamConfig.Stil_AltitudeModePeak = enUserPeakMode.ThreePeak;
                        break;
                    case "FourPeak":
                        _sensor.SetParam(enSensorParamType.Stil_高度模式的峰值选择, enUserPeakMode.FourPeak);
                        stilLineParamConfig.Stil_AltitudeModePeak = enUserPeakMode.FourPeak;
                        break;
                    case "FivePeak":
                        _sensor.SetParam(enSensorParamType.Stil_高度模式的峰值选择, enUserPeakMode.FirstPeak);
                        stilLineParamConfig.Stil_AltitudeModePeak = enUserPeakMode.FirstPeak;
                        break;
                    case "LastPeak":
                        _sensor.SetParam(enSensorParamType.Stil_高度模式的峰值选择, enUserPeakMode.LastPeak);
                        stilLineParamConfig.Stil_AltitudeModePeak = enUserPeakMode.LastPeak;
                        break;
                }             
                MessageBox.Show("设置成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }
        private void ThicknessPeak1comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string peakMode = this.ThicknessPeak1comboBox.SelectedItem.ToString();
                switch (peakMode)
                {
                    case "FirstPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值1, enUserPeakMode.FirstPeak);
                        stilLineParamConfig.Stil_ThickModePeak1 = enUserPeakMode.FirstPeak;
                        break;
                    case "StrongestPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值1, enUserPeakMode.StrongestPeak);
                        stilLineParamConfig.Stil_ThickModePeak1 = enUserPeakMode.StrongestPeak;
                        break;
                    case "SecondPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值1, enUserPeakMode.SecondPeak);
                        stilLineParamConfig.Stil_ThickModePeak1 = enUserPeakMode.SecondPeak;
                        break;
                    case "ThreePeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值1, enUserPeakMode.ThreePeak);
                        stilLineParamConfig.Stil_ThickModePeak1 = enUserPeakMode.ThreePeak;
                        break;
                    case "FourPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值1, enUserPeakMode.FourPeak);
                        stilLineParamConfig.Stil_ThickModePeak1 = enUserPeakMode.FourPeak;
                        break;
                    case "FivePeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值1, enUserPeakMode.FirstPeak);
                        stilLineParamConfig.Stil_ThickModePeak1 = enUserPeakMode.FirstPeak;
                        break;
                    case "LastPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值1, enUserPeakMode.LastPeak);
                        stilLineParamConfig.Stil_ThickModePeak1 = enUserPeakMode.LastPeak;
                        break;
                }
                MessageBox.Show("设置成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void ThicknessPeak2comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string peakMode = this.ThicknessPeak2comboBox.SelectedItem.ToString();
                switch (peakMode)
                {
                    case "FirstPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值2, enUserPeakMode.FirstPeak);
                        stilLineParamConfig.Stil_ThickModePeak2 = enUserPeakMode.FirstPeak;
                        break;
                    case "StrongestPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值2, enUserPeakMode.StrongestPeak);
                        stilLineParamConfig.Stil_ThickModePeak2 = enUserPeakMode.StrongestPeak;
                        break;
                    case "SecondPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值2, enUserPeakMode.SecondPeak);
                        stilLineParamConfig.Stil_ThickModePeak2 = enUserPeakMode.SecondPeak;
                        break;
                    case "ThreePeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值2, enUserPeakMode.ThreePeak);
                        stilLineParamConfig.Stil_ThickModePeak2 = enUserPeakMode.ThreePeak;
                        break;
                    case "FourPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值2, enUserPeakMode.FourPeak);
                        stilLineParamConfig.Stil_ThickModePeak2 = enUserPeakMode.FourPeak;
                        break;
                    case "FivePeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值2, enUserPeakMode.FivePeak);
                        stilLineParamConfig.Stil_ThickModePeak2 = enUserPeakMode.FivePeak;
                        break;
                    case "LastPeak":
                        _sensor.SetParam(enSensorParamType.Stil_厚度模式下峰值2, enUserPeakMode.LastPeak);
                        stilLineParamConfig.Stil_ThickModePeak2 = enUserPeakMode.LastPeak;
                        break;
                }
                MessageBox.Show("设置成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void StilLineLaserParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {
                this.AltitudePeakcomboBox.DataSource = Enum.GetNames(typeof(enUserPeakMode));
                this.ThicknessPeak1comboBox.DataSource = Enum.GetNames(typeof(enUserPeakMode));
                this.ThicknessPeak2comboBox.DataSource = Enum.GetNames(typeof(enUserPeakMode));
                this.TriggerModeCombox.DataSource = Enum.GetNames(typeof(enUserTrigerMode));
                this.MeasureModecomboBox.DataSource = Enum.GetNames(typeof(enUserMeasureMode));
                this.MeasureRange.Text = _sensor.GetParam(enSensorParamType.Stil_测量范围).ToString();
                this.触发源comboBox.DataSource = Enum.GetNames(typeof(enUserTriggerSource));
                /// stil线激光参数
                //////////////////
                this.触发源comboBox.DataBindings.Add("Text", stilLineParamConfig, "TriggerSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.LEDtextBox.DataBindings.Add("Text", stilLineParamConfig, "Stil_LedBrightness", true, DataSourceUpdateMode.OnPropertyChanged);
                this.ExposeTimetextBox.DataBindings.Add("Text", stilLineParamConfig, "Stil_ExposureTime", true, DataSourceUpdateMode.OnPropertyChanged);
                this.DetectiontextBox.DataBindings.Add("Text", stilLineParamConfig, "Stil_DetectionThreshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.TriggerModeCombox.DataBindings.Add("Text", stilLineParamConfig, "TrigMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.TRENumtextBox.DataBindings.Add("Text", stilLineParamConfig, "TreNum", true, DataSourceUpdateMode.OnPropertyChanged);
                this.MeasureModecomboBox.DataBindings.Add("Text", stilLineParamConfig, "MeasureMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.AltitudePeakcomboBox.DataBindings.Add("Text", stilLineParamConfig, "Stil_AltitudeModePeak", true, DataSourceUpdateMode.OnPropertyChanged);
                this.ThicknessPeak1comboBox.DataBindings.Add("Text", stilLineParamConfig, "Stil_ThickModePeak1", true, DataSourceUpdateMode.OnPropertyChanged);
                this.ThicknessPeak2comboBox.DataBindings.Add("Text", stilLineParamConfig, "Stil_ThickModePeak2", true, DataSourceUpdateMode.OnPropertyChanged);
                // this.MeasureRange.DataBindings.Add("Text", stilLineParamConfig, "MeasureRange", true, DataSourceUpdateMode.OnPropertyChanged);
                this.取消等待采集checkBox.DataBindings.Add("Checked", stilLineParamConfig, "CancelWaite", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {

            }
        }

        private void StilLineLaserParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                this.stilLineParamConfig.SaveParamConfig(this.savePath);
                if (this.lineForm != null)
                {
                    this.lineForm.Close();
                    this.lineForm.Dispose();
                }          
            }
            catch
            {

            }
        }

        private void SaveConfigbutton_Click(object sender, EventArgs e)
        {
            this.stilLineParamConfig.SaveParamConfig(this.savePath);
        }

        private void 置零编码器button_Click(object sender, EventArgs e)
        {
            _sensor.SetParam(enSensorParamType.Stil_置零编码器, null);
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
