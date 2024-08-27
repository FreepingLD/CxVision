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
    public partial class StilPointLaserParamForm : Form
    {
        private AcqSource _acqSource;
        private ISensor _sensor;
        private STIL_NET.sensor _stilSensor;
        private FileOperate fo = new FileOperate();
        private StilPointSensorSetting stilPointParamConfig;
        private string savePath;
        private PointLaserForm pointForm;

        public StilPointLaserParamForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = this._acqSource.Sensor;
            this._stilSensor = ((CStil_P)this._sensor).Stil_P.Sensor;
            this.pointForm = new PointLaserForm(this._acqSource);
            this.AddForm(this.监控panel, this.pointForm);
            this.savePath = Application.StartupPath + "\\" + "LaserParam" + "\\" + this._sensor.ConfigParam.SensorName.ToString() + ".txt";
            stilPointParamConfig = ((CStil_P)this._acqSource.Sensor).StilPointParamConfig;
        }
        private void cmb_RateList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.cmb_RateList.SelectedIndex == -1) return;
            try
            {
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).ScanRate = (STIL_NET.enFixedScanRates_ZENITH)this.cmb_RateList.SelectedIndex;
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).ScanRate = (STIL_NET.enFixedScanRates_CCS_PRIMA)this.cmb_RateList.SelectedIndex;
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).ScanRate = (STIL_NET.enFixedScanRates_CCS_OPTIMA_PLUS)this.cmb_RateList.SelectedIndex;
                        break;
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void cmb_OpticalPen_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.cmb_OpticalPen.SelectedIndex == -1) return;
            try
            {
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).OpticalPen = this.cmb_OpticalPen.SelectedIndex;
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).OpticalPen = this.cmb_OpticalPen.SelectedIndex;
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).OpticalPen = this.cmb_OpticalPen.SelectedIndex;
                        break;

                }
                //_sensor.SetParam(enSensorParamType.Stil_设置预置频率, this.cmb_OpticalPen.SelectedIndex);
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void tb_Threshoud_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.tb_Threshoud.Text.Trim().Length != 0)
                    {
                        double value = double.Parse(this.tb_Threshoud.Text);
                        switch (this._stilSensor.SensorType)
                        {
                            //case STIL_NET.enSensorType.STIL_ZENITH:
                            //    if (((STIL_NET.sensorZenith)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                            //        ((STIL_NET.sensorZenith)this._stilSensor).DistanceDetectionThreshold = value;
                            //    else
                            //    {
                            //        ((STIL_NET.sensorZenith)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold = value;
                            //        ((STIL_NET.sensorZenith)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold = value;
                            //    }                                  
                            //    break;
                            case STIL_NET.enSensorType.CCS_PRIMA:
                                if (((STIL_NET.sensorCCSPrima)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                                    ((STIL_NET.sensorCCSPrima)this._stilSensor).DistanceDetectionThreshold = value;
                                else
                                {
                                    ((STIL_NET.sensorCCSPrima)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold = value;
                                    ((STIL_NET.sensorCCSPrima)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold = value;
                                }
                                break;
                            case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                                if (((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                                    ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).DistanceDetectionThreshold = value;
                                else
                                {
                                    ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold = value;
                                    ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold = value;
                                }
                                break;
                        }
                        MessageBox.Show("设置成功");
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }
        private void Darkbutton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    this.tb_MinRate.Text = ((STIL_NET.sensorZenith)this._stilSensor).AcqDark.ToString();
                    //this.tb_MinRate.Text = ((STIL_NET.sensorZenith)this._stilSensor).MinDarkFrequency.ToString();
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        this.tb_MinRate.Text = ((STIL_NET.sensorCCSPrima)this._stilSensor).AcqDark.ToString();
                        this.tb_MinRate.Text = ((STIL_NET.sensorCCSPrima)this._stilSensor).MinDarkFrequency.ToString();
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        this.tb_MinRate.Text = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).AcqDark.ToString();
                        this.tb_MinRate.Text = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).MinDarkFrequency.ToString();
                        break;
                }
                MessageBox.Show("Dark完成");
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void rb_manualMode_Click(object sender, EventArgs e)
        {
            try
            {
                this.trackbarManual.Minimum = 0;
                this.trackbarManual.Maximum = 100;
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).LedAuto = false;
                    //    this.trackbarManual.Value = ((STIL_NET.sensorZenith)this._stilSensor).LedBrightness;
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).LedAuto = false;
                        this.trackbarManual.Value = ((STIL_NET.sensorCCSPrima)this._stilSensor).LedBrightness;
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).LedAuto = false;
                        this.trackbarManual.Value = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).LedBrightness;
                        break;
                }
                this.ManualValuelabel.Text = this.trackbarManual.Value.ToString() + "%";
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void rb_Auto_Click(object sender, EventArgs e)
        {
            try
            {
                this.trackbarManual.Minimum = 0;
                this.trackbarManual.Maximum = 100;
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).LedAuto = true;
                    //    this.trackbarAuto.Value = ((STIL_NET.sensorZenith)this._stilSensor).AutoModeThreshold;
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).LedAuto = true;
                        this.trackbarAuto.Value = ((STIL_NET.sensorCCSPrima)this._stilSensor).AutoModeThreshold; // 用于自动光源调节
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).LedAuto = true;
                        this.trackbarAuto.Value = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).AutoModeThreshold;
                        break;
                }
                this.AutoLightlabel.Text = "Max" + " " + this.trackbarAuto.Value.ToString();
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void BindProperty()
        {
            try
            {
                this.触发模式comboBox.DataSource = Enum.GetNames(typeof(enUserTrigerMode));
                this.触发源comboBox.DataSource = Enum.GetNames(typeof(enUserTriggerSource));
                this.触发电平comboBox.DataSource = Enum.GetNames(typeof(enUserLevelEdgeFlag));
                ////////////////////////////////////////////////////////////////////////////////////
                this.触发源comboBox.DataBindings.Add("Text", stilPointParamConfig, "TriggerSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发模式comboBox.DataBindings.Add("Text", stilPointParamConfig, "TrigMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发电平comboBox.DataBindings.Add("Text", stilPointParamConfig, "LevelEdgeFlag", true, DataSourceUpdateMode.OnPropertyChanged);
                this.采集点数textBox.DataBindings.Add("Text", stilPointParamConfig, "AcqCount", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.采集延时textBox.DataBindings.Add("Text", stilPointParamConfig, "AcqWaiteTime", true, DataSourceUpdateMode.OnPropertyChanged);
                this.取消等待采集checkBox.DataBindings.Add("Checked", stilPointParamConfig, "CancelWaite", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        // 更新窗体上的参数
        private void UpDataForm()
        {
            try
            {
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    // 光源模式
                    //    if (((STIL_NET.sensorZenith)this._stilSensor).LedAuto)
                    //    {
                    //        this.rb_Auto.Checked = true;
                    //        this.trackbarAuto.Value = ((STIL_NET.sensorZenith)this._stilSensor).AutoModeThreshold;
                    //        this.trackbarAuto.Value = (this._stilSensor).LedBrightness;
                    //        this.AutoLightlabel.Text = "Max" + " " + this.trackbarAuto.Value.ToString();
                    //    }
                    //    else
                    //    {
                    //        this.rb_manualMode.Checked = true;
                    //        this.trackbarManual.Value = ((STIL_NET.sensorZenith)this._stilSensor).LedBrightness;
                    //        this.ManualValuelabel.Text = this.trackbarManual.Value.ToString() + "%";
                    //    }
                    //    ///////////////
                    //    cmb_RateList.DataSource = ((STIL_NET.sensorZenith)this._stilSensor).RateList;
                    //    cmb_RateList.SelectedIndex = (int)((STIL_NET.sensorZenith)this._stilSensor).ScanRate;
                    //    cmb_OpticalPen.DataSource = ((STIL_NET.sensorZenith)this._stilSensor).PenList;
                    //    cmb_OpticalPen.SelectedIndex = ((STIL_NET.sensorZenith)this._stilSensor).OpticalPen;
                    //    tb_MinRate.Text = ((STIL_NET.sensorZenith)this._stilSensor).MinDarkFrequency.ToString();
                    //    this.测量模式comboBox.DataSource =Enum.GetNames(typeof(STIL_NET.enMeasureMode));
                    //    this.峰值选择comboBox.DataSource = Enum.GetNames(typeof(STIL_NET.enPeakSelectionMode));
                    //    // 峰值
                    //    this.峰值选择comboBox.SelectedIndex = (int)((STIL_NET.sensorZenith)this._stilSensor).PeakSelectionMode;
                    //    ///测量模式
                    //    this.测量模式comboBox.SelectedIndex = (int)((STIL_NET.sensorZenith)this._stilSensor).MeasureMode;
                    //    /// 阈值
                    //    this.tb_Threshoud.Text = ((STIL_NET.sensorZenith)this._stilSensor).DistanceDetectionThreshold.ToString();
                    //                            if (((STIL_NET.sensorZenith)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                    //{
                    //    this.tb_Threshoud.Enabled = true;
                    //    this.SDPtextBox.Enabled = false;
                    //    this.SPPtextBox.Enabled = false;
                    //    this.tb_Threshoud.Text = ((STIL_NET.sensorZenith)this._stilSensor).DistanceDetectionThreshold.ToString();
                    //}
                    //else
                    //{
                    //    this.tb_Threshoud.Enabled = false;
                    //    this.SDPtextBox.Enabled = true;
                    //    this.SPPtextBox.Enabled = true;
                    //    this.SDPtextBox.Text = ((STIL_NET.sensorZenith)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold.ToString();
                    //    this.SPPtextBox.Text = ((STIL_NET.sensorZenith)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold.ToString();
                    //}
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        // 光源模式
                        if (((STIL_NET.sensorCCSPrima)this._stilSensor).LedAuto)
                        {
                            this.trackbarAuto.Value = ((STIL_NET.sensorCCSPrima)this._stilSensor).AutoModeThreshold;
                            this.AutoLightlabel.Text = "Max" + " " + this.trackbarAuto.Value.ToString();
                            this.rb_Auto.Checked = true;
                        }
                        else
                        {
                            this.trackbarManual.Value = ((STIL_NET.sensorCCSPrima)this._stilSensor).LedBrightness;
                            this.ManualValuelabel.Text = this.trackbarManual.Value.ToString() + "%";
                            this.rb_manualMode.Checked = true;
                        }
                        cmb_RateList.DataSource = ((STIL_NET.sensorCCSPrima)this._stilSensor).RateList;
                        cmb_RateList.SelectedIndex = (int)((STIL_NET.sensorCCSPrima)this._stilSensor).ScanRate;
                        cmb_OpticalPen.DataSource = ((STIL_NET.sensorCCSPrima)this._stilSensor).PenList;
                        cmb_OpticalPen.SelectedIndex = ((STIL_NET.sensorCCSPrima)this._stilSensor).OpticalPen;
                        tb_MinRate.Text = ((STIL_NET.sensorCCSPrima)this._stilSensor).MinDarkFrequency.ToString();
                        this.测量模式comboBox.DataSource = Enum.GetNames(typeof(STIL_NET.enMeasureMode));
                        this.峰值选择comboBox.DataSource = Enum.GetNames(typeof(STIL_NET.enPeakSelectionMode));
                        // 峰值
                        this.峰值选择comboBox.SelectedIndex = (int)((STIL_NET.sensorCCSPrima)this._stilSensor).PeakSelectionMode;
                        ///测量模式
                        this.测量模式comboBox.SelectedIndex = (int)((STIL_NET.sensorCCSPrima)this._stilSensor).MeasureMode;
                        /// 阈值
                        if (((STIL_NET.sensorCCSPrima)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                        {
                            this.tb_Threshoud.Text = ((STIL_NET.sensorCCSPrima)this._stilSensor).DistanceDetectionThreshold.ToString();
                            this.SDPtextBox.Enabled = false;
                            this.SPPtextBox.Enabled = false;
                        }
                        else
                        {
                            this.SDPtextBox.Text = ((STIL_NET.sensorCCSPrima)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold.ToString();
                            this.SPPtextBox.Text = ((STIL_NET.sensorCCSPrima)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold.ToString();
                            this.tb_Threshoud.Enabled = false;
                        }

                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        // 光源模式
                        if (((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).LedAuto)
                        {
                            this.rb_Auto.Checked = true;
                            this.trackbarAuto.Value = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).AutoModeThreshold;
                            this.AutoLightlabel.Text = "Max" + " " + this.trackbarAuto.Value.ToString();
                        }
                        else
                        {
                            this.rb_manualMode.Checked = true;
                            this.trackbarManual.Value = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).LedBrightness;
                            this.ManualValuelabel.Text = this.trackbarManual.Value.ToString() + "%";
                        }
                        cmb_RateList.DataSource = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).RateList;
                        cmb_RateList.SelectedIndex = (int)((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).ScanRate;
                        cmb_OpticalPen.DataSource = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).PenList;
                        cmb_OpticalPen.SelectedIndex = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).OpticalPen;
                        tb_MinRate.Text = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).MinDarkFrequency.ToString();
                        this.测量模式comboBox.DataSource = Enum.GetNames(typeof(STIL_NET.enMeasureMode));
                        this.峰值选择comboBox.DataSource = Enum.GetNames(typeof(STIL_NET.enPeakSelectionMode));
                        // 峰值
                        this.峰值选择comboBox.SelectedIndex = (int)((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).PeakSelectionMode;
                        ///测量模式
                        this.测量模式comboBox.SelectedIndex = (int)((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).MeasureMode;
                        /// 阈值
                        //this.tb_Threshoud.Text = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).DistanceDetectionThreshold.ToString();
                        if (((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                        {
                            this.tb_Threshoud.Enabled = true;
                            this.SDPtextBox.Enabled = false;
                            this.SPPtextBox.Enabled = false;
                            this.tb_Threshoud.Text = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).DistanceDetectionThreshold.ToString();
                        }
                        else
                        {
                            this.tb_Threshoud.Enabled = false;
                            this.SDPtextBox.Enabled = true;
                            this.SPPtextBox.Enabled = true;
                            this.SDPtextBox.Text = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold.ToString();
                            this.SPPtextBox.Text = ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold.ToString();
                        }
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void StilPointLaserParamForm_Load(object sender, EventArgs e)
        {
            UpDataForm();
            BindProperty();
        }

        private void StilPointLaserParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.pointForm != null)
                {
                    this.pointForm.Close();
                    this.pointForm.Dispose();
                }
                this.stilPointParamConfig.SaveParamConfig(this.savePath);
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).SaveCurrentConfiguration();
                    //    break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).SaveCurrentConfiguration();
                        break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).SaveCurrentConfiguration();
                        break;
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            try
            {
                this.stilPointParamConfig.SaveParamConfig(this.savePath);
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).SaveCurrentConfiguration();
                    //    break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).SaveCurrentConfiguration();
                        break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).SaveCurrentConfiguration();
                        break;
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
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

        private void trackbarManual_Scroll(object sender, EventArgs e)
        {
            try
            {
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).LedBrightness = this.trackbarManual.Value;
                    // this.ManualValuelabel.Text = this.trackbarManual.Value.ToString();
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).LedBrightness = this.trackbarManual.Value;
                        //((STIL_NET.sensorCCSPrima)this._stilSensor).LedAuto = false;
                        this.ManualValuelabel.Text = this.trackbarManual.Value.ToString() + "%";
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).LedBrightness = this.trackbarManual.Value;
                        //((STIL_NET.sensorCCSPrima)this._stilSensor).LedAuto = false; // LedBrightness:用于手动光源调节
                        this.ManualValuelabel.Text = this.trackbarManual.Value.ToString() + "%";
                        break;
                }
            }
            catch
            {

            }
        }

        private void trackbarAuto_Scroll(object sender, EventArgs e)
        {
            try
            {
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //    ((STIL_NET.sensorZenith)this._stilSensor).AutoModeThreshold = this.trackbarAuto.Value;
                    //this.AutoLightlabel.Text = "Max" + " " + (this.trackbarAuto.Value).ToString()+ "%";
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).AutoModeThreshold = this.trackbarAuto.Value;
                        //((STIL_NET.sensorCCSPrima)this._stilSensor).LedAuto = true;
                        this.AutoLightlabel.Text = "Max" + " " + (this.trackbarAuto.Value).ToString();
                        //int value = ((STIL_NET.sensorCCSPrima)this._stilSensor).AutoModeThreshold;  //AutoModeThreshold:用于自动光源的控制
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).AutoModeThreshold = this.trackbarAuto.Value;
                        //((STIL_NET.sensorCCSPrima)this._stilSensor).LedAuto = true;
                        this.AutoLightlabel.Text = "Max" + " " + (this.trackbarAuto.Value).ToString();
                        break;
                }
            }
            catch
            {

            }
        }

        private void 峰值选择comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.峰值选择comboBox.SelectedIndex == -1) return;
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //   ((STIL_NET.sensorZenith)this._stilSensor).PeakSelectionMode = (STIL_NET.enPeakSelectionMode)this.峰值选择comboBox.SelectedIndex;
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).PeakSelectionMode = (STIL_NET.enPeakSelectionMode)this.峰值选择comboBox.SelectedIndex;
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).PeakSelectionMode = (STIL_NET.enPeakSelectionMode)this.峰值选择comboBox.SelectedIndex;
                        break;
                }
            }
            catch
            {

            }
        }

        private void paramSetButton_Click(object sender, EventArgs e)
        {

        }

        private void 测量模式comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.测量模式comboBox.SelectedIndex == -1) return;
                switch (this._stilSensor.SensorType)
                {
                    //case STIL_NET.enSensorType.STIL_ZENITH:
                    //   ((STIL_NET.sensorZenith)this._stilSensor).MeasureMode = (STIL_NET.MeasureMode)this.测量模式comboBox.SelectedIndex;
                    //                            if (((STIL_NET.sensorZenith)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                    //{
                    //    this.tb_Threshoud.Enabled = true;
                    //    this.SDPtextBox.Enabled = false;
                    //    this.SPPtextBox.Enabled = false;
                    //}
                    //else
                    //{
                    //    this.tb_Threshoud.Enabled = false;
                    //    this.SDPtextBox.Enabled = true;
                    //    this.SPPtextBox.Enabled = true;
                    //}
                    //    break;
                    case STIL_NET.enSensorType.CCS_PRIMA:
                        //STIL_NET.enMeasureMode eee = (STIL_NET.enMeasureMode)this.测量模式comboBox.SelectedIndex;
                        //(STIL_NET.sensorCCSPrima)this._stilSensor.StopAcquisition_Measurement();
                        // 在没有停止采集的情况下是不能切换测量模式的
                        ((STIL_NET.sensorCCSPrima)this._stilSensor).MeasureMode = (STIL_NET.enMeasureMode)this.测量模式comboBox.SelectedIndex;
                        //////////////////////////////////////////
                        if (((STIL_NET.sensorCCSPrima)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                        {
                            this.tb_Threshoud.Enabled = true;
                            this.SDPtextBox.Enabled = false;
                            this.SPPtextBox.Enabled = false;
                        }
                        else
                        {
                            this.tb_Threshoud.Enabled = false;
                            this.SDPtextBox.Enabled = true;
                            this.SPPtextBox.Enabled = true;
                        }
                        break;
                    case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                        ((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).MeasureMode = (STIL_NET.enMeasureMode)this.测量模式comboBox.SelectedIndex;
                        ///////////////////////////////////////////////////////////////////////
                        if (((STIL_NET.sensorCCSOptimaPlus)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                        {
                            this.tb_Threshoud.Enabled = true;
                            this.SDPtextBox.Enabled = false;
                            this.SPPtextBox.Enabled = false;
                        }
                        else
                        {
                            this.tb_Threshoud.Enabled = false;
                            this.SDPtextBox.Enabled = true;
                            this.SPPtextBox.Enabled = true;
                        }
                        break;
                }
            }
            catch
            {

            }


        }

        private void SDPtextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.SDPtextBox.Text.Trim().Length != 0)
                    {
                        double value = double.Parse(this.SDPtextBox.Text);
                        switch (this._stilSensor.SensorType)
                        {
                            //case STIL_NET.enSensorType.STIL_ZENITH:
                            //    if (((STIL_NET.sensorZenith)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                            //        ((STIL_NET.sensorZenith)this._stilSensor).DistanceDetectionThreshold = value;
                            //    else
                            //    {
                            //        ((STIL_NET.sensorZenith)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold = value;
                            //    }                                  
                            //    break;
                            case STIL_NET.enSensorType.CCS_PRIMA:
                                if (((STIL_NET.sensorCCS)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                                    ((STIL_NET.sensorCCS)this._stilSensor).DistanceDetectionThreshold = value;
                                else
                                {
                                    ((STIL_NET.sensorCCS)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold = value;
                                }
                                break;
                            case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                                if (((STIL_NET.sensorCCS)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                                    ((STIL_NET.sensorCCS)this._stilSensor).DistanceDetectionThreshold = value;
                                else //sensorCCSOptimaPlus
                                {
                                    ((STIL_NET.sensorCCS)this._stilSensor).ThicknessDetectionThreshold.SecondPeakThreshold = value;
                                }
                                break;
                        }
                    }
                    MessageBox.Show("设置成功");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private void SPPtextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.SPPtextBox.Text.Trim().Length != 0)
                    {
                        double value = double.Parse(this.SPPtextBox.Text);
                        switch (this._stilSensor.SensorType)
                        {
                            //case STIL_NET.enSensorType.STIL_ZENITH:
                            //    if (((STIL_NET.sensorZenith)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                            //        ((STIL_NET.sensorZenith)this._stilSensor).DistanceDetectionThreshold = value;
                            //    else
                            //    {
                            //        ((STIL_NET.sensorZenith)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold = value;
                            //    }                                  
                            //    break;
                            case STIL_NET.enSensorType.CCS_PRIMA:
                                if (((STIL_NET.sensorCCS)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                                    ((STIL_NET.sensorCCS)this._stilSensor).DistanceDetectionThreshold = value;
                                else
                                {
                                    ((STIL_NET.sensorCCS)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold = value;
                                }
                                break;
                            case STIL_NET.enSensorType.CCS_OPTIMA_PLUS:
                                if (((STIL_NET.sensorCCS)this._stilSensor).MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                                    ((STIL_NET.sensorCCS)this._stilSensor).DistanceDetectionThreshold = value;
                                else //sensorCCSOptimaPlus
                                {
                                    ((STIL_NET.sensorCCS)this._stilSensor).ThicknessDetectionThreshold.FirstPeakThreshold = value;
                                }
                                break;
                        }
                    }
                    MessageBox.Show("设置成功");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }


    }
}
