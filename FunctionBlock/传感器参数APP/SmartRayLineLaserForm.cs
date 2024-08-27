
using Common;
using FunctionBlock;
using MotionControlCard;
using Sensor;
using Smartray;
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

    public partial class SmartRayLineLaserParamForm : Form
    {
        private AcqSource _acqSource;
        private ISensor _sensor;
        private string savePath;
        private SmartRayLineLaserMonitorForm smartForm;
        public SmartRayLineLaserParamForm(ISensor sensor)
        {
            InitializeComponent();
            this._sensor = sensor;
            //this.savePath =  Application.StartupPath + "\\" + "StilLaserParam" + "\\" + this._sensor.GetParam("传感器名称").ToString() + ".txt";
        }
        public SmartRayLineLaserParamForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = acqSource.Sensor;
            this.smartForm = new SmartRayLineLaserMonitorForm(this._acqSource);
            this.AddForm(this.监控panel, this.smartForm);

            //this.savePath =  Application.StartupPath + "\\" + "StilLaserParam" + "\\" + this._sensor.GetParam("传感器名称").ToString() + ".txt";
        }
        private void SmartRayLineLaserForm_Load(object sender, EventArgs e)
        {
            initCameraUI();
            initCapture3DUI();
            initTriggerUI();
        }  

        private void SmartRayLineLaserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this._sensor.SetParam(enSensorParamType.Coom_保存参数, null);
                if(this.smartForm!=null)
                {
                    this.smartForm.Close();
                    this.smartForm.Dispose();
                }
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
        private void initCameraUI()
        {
            try
            {               
                this.多重曝光合并模comboBox.DataSource = Enum.GetNames(typeof(Api.MultipleExposureMergeModeType));
                this.曝光模式comboBox.DataSource = Enum.GetNames(typeof(enExposeMode));
                this.光源模式comboBox.DataSource = Enum.GetNames(typeof(Api.LaserMode));
                ////////////////////////////////////////////////////////////////////////////
                int[] doubleExposeTime = (int[])_sensor.GetParam(enSensorParamType.smartRay_曝光时间);
                int exposeMode = (int)_sensor.GetParam(enSensorParamType.smartRay_曝光模式);
                object multipleExposureMergeModeType = _sensor.GetParam(enSensorParamType.smartRay_多重曝光合并模式);
                int gain = (int)_sensor.GetParam(enSensorParamType.smartRay_增益);
                int laserLight = (int)_sensor.GetParam(enSensorParamType.smartRay_激光亮度);
                object laserMode = _sensor.GetParam(enSensorParamType.smartRay_激光模式);
                ////////////////////////////////////////////////////////////////////
                if (doubleExposeTime.Length > 1)
                {
                    this.曝光时间1texBox.Text = doubleExposeTime[0].ToString();
                    this.曝光时间2texBox.Text = doubleExposeTime[1].ToString();
                }
                else
                    this.曝光时间1texBox.Text = doubleExposeTime[0].ToString();
                /////////////////////////////////////////////////////////////////////
                if (exposeMode == 2)
                    this.曝光模式comboBox.Text = "DoubleExpose";
                else
                    this.曝光模式comboBox.Text = "SingleExpose";
                ///////////////////
                this.增益texBox.Text = gain.ToString();
                this.光源亮度textBox.Text = laserLight.ToString();
                ////////////////////////////////////////////////////////////////////
                switch ((Api.MultipleExposureMergeModeType)multipleExposureMergeModeType)
                {
                    case Api.MultipleExposureMergeModeType.Merge:
                        this.多重曝光合并模comboBox.Text = Api.MultipleExposureMergeModeType.Merge.ToString();
                        break;
                    case Api.MultipleExposureMergeModeType.No_Merge:
                        this.多重曝光合并模comboBox.Text = Api.MultipleExposureMergeModeType.No_Merge.ToString();
                        break;
                    case Api.MultipleExposureMergeModeType.No_Merge_With_Prune:
                        this.多重曝光合并模comboBox.Text = Api.MultipleExposureMergeModeType.No_Merge_With_Prune.ToString();
                        break;
                    case Api.MultipleExposureMergeModeType.HighAccuracy_Point_Selector:
                        this.多重曝光合并模comboBox.Text = Api.MultipleExposureMergeModeType.HighAccuracy_Point_Selector.ToString();
                        break;
                    case Api.MultipleExposureMergeModeType.HighAccuracy_Profile_Selector:
                        this.多重曝光合并模comboBox.Text = Api.MultipleExposureMergeModeType.HighAccuracy_Profile_Selector.ToString();
                        break;
                    default:
                        this.多重曝光合并模comboBox.Text = Api.MultipleExposureMergeModeType.Merge.ToString();
                        break;
                }
                ////////////////////////////////////////////////////////////////////
                switch ((Api.LaserMode)laserMode)
                {
                    case Api.LaserMode.ContinousMode:
                        this.多重曝光合并模comboBox.Text = Api.LaserMode.ContinousMode.ToString();
                        break;
                    case Api.LaserMode.PulsedMode:
                        this.多重曝光合并模comboBox.Text = Api.LaserMode.PulsedMode.ToString();
                        break;
                    default:
                        this.多重曝光合并模comboBox.Text = Api.LaserMode.ContinousMode.ToString();
                        break;
                }
            }
            catch(Exception ee)
            {

            }
        }

        private void 曝光模式comboBox_TextChanged(object sender, EventArgs e)
        {
            string text = this.曝光模式comboBox.SelectedItem.ToString();
            switch (text)
            {
                case "SingleExpose":
                    _sensor.SetParam(enSensorParamType.smartRay_曝光模式, enExposeMode.SingleExpose);
                    this.曝光时间2texBox.ReadOnly = true;
                    break;
                case "DoubleExpose":
                    _sensor.SetParam(enSensorParamType.smartRay_曝光模式, enExposeMode.DoubleExpose);
                    this.曝光时间2texBox.ReadOnly = false;
                    break;
                default:
                    _sensor.SetParam(enSensorParamType.smartRay_曝光模式, enExposeMode.SingleExpose);
                    break;
            }
        }

        private void 多重曝光合并模comboBox_TextChanged(object sender, EventArgs e)
        {
            string text = this.多重曝光合并模comboBox.SelectedItem.ToString();
            switch (text)
            {
                case "Merge":
                    _sensor.SetParam(enSensorParamType.smartRay_多重曝光合并模式, Api.MultipleExposureMergeModeType.Merge);
                    break;
                case "No_Merge":
                    _sensor.SetParam(enSensorParamType.smartRay_多重曝光合并模式, Api.MultipleExposureMergeModeType.No_Merge);
                    break;
                case "No_Merge_With_Prune":
                    _sensor.SetParam(enSensorParamType.smartRay_多重曝光合并模式, Api.MultipleExposureMergeModeType.No_Merge_With_Prune);
                    break;
                case "HighAccuracy_Point_Selector":
                    _sensor.SetParam(enSensorParamType.smartRay_多重曝光合并模式, Api.MultipleExposureMergeModeType.HighAccuracy_Point_Selector);
                    break;
                case "HighAccuracy_Profile_Selector":
                    _sensor.SetParam(enSensorParamType.smartRay_多重曝光合并模式, Api.MultipleExposureMergeModeType.HighAccuracy_Profile_Selector);
                    break;
                default:
                    _sensor.SetParam(enSensorParamType.smartRay_多重曝光合并模式, Api.MultipleExposureMergeModeType.Merge);
                    break;
            }
        }

        private void 光源模式comboBox_TextChanged(object sender, EventArgs e)
        {
            string text = this.光源模式comboBox.SelectedItem.ToString();
            switch (text)
            {
                case "ContinousMode":
                    _sensor.SetParam(enSensorParamType.smartRay_激光模式, Api.LaserMode.ContinousMode);
                    break;
                case "PulsedMode":
                    _sensor.SetParam(enSensorParamType.smartRay_激光模式, Api.LaserMode.PulsedMode);
                    break;
                default:
                    _sensor.SetParam(enSensorParamType.smartRay_激光模式, Api.LaserMode.ContinousMode);
                    break;
            }
        }

        private void 曝光时间1texBox_KeyUp(object sender, KeyEventArgs e)
        {
            int expose1 = 0;
            int expose2 = 0;
            int.TryParse(曝光时间1texBox.Text, out expose1);
            int.TryParse(曝光时间2texBox.Text, out expose2);
            string exposeMode = this.曝光模式comboBox.SelectedItem.ToString();
            string multipleExposureMergeModeType = this.多重曝光合并模comboBox.SelectedItem.ToString();
            //////////////////////////
            if (e.KeyCode == Keys.Enter)
            {
                switch (exposeMode)
                {
                    case "SingleExpose":
                        _sensor.SetParam(enSensorParamType.smartRay_曝光时间, expose1);
                        break;
                    case "DoubleExpose":
                        switch (multipleExposureMergeModeType)
                        {
                            case "Merge":
                                _sensor.SetParam(enSensorParamType.smartRay_曝光时间, new object[3] { Api.MultipleExposureMergeModeType.Merge, expose1, expose2 });
                                break;
                            case "No_Merge":
                                _sensor.SetParam(enSensorParamType.smartRay_曝光时间, new object[3] { Api.MultipleExposureMergeModeType.No_Merge, expose1, expose2 });
                                break;
                            case "No_Merge_With_Prune":
                                _sensor.SetParam(enSensorParamType.smartRay_曝光时间, new object[3] { Api.MultipleExposureMergeModeType.No_Merge_With_Prune, expose1, expose2 });
                                break;
                            case "HighAccuracy_Point_Selector":
                                _sensor.SetParam(enSensorParamType.smartRay_曝光时间, new object[3] { Api.MultipleExposureMergeModeType.HighAccuracy_Point_Selector, expose1, expose2 });
                                break;
                            case "HighAccuracy_Profile_Selector":
                                _sensor.SetParam(enSensorParamType.smartRay_曝光时间, new object[3] { Api.MultipleExposureMergeModeType.HighAccuracy_Profile_Selector, expose1, expose2 });
                                break;
                        }
                        break;
                }
            }
        }

        private void 曝光时间2texBox_KeyUp(object sender, KeyEventArgs e)
        {
            曝光时间1texBox_KeyUp(null, e);
        }

        private void 增益texBox_KeyUp(object sender, KeyEventArgs e)
        {
            int value = 0;
            int.TryParse(增益texBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                _sensor.SetParam(enSensorParamType.smartRay_增益, value);
            }
        }

        private void 光源亮度textBox_KeyUp(object sender, KeyEventArgs e)
        {
            int value = 0;
            int.TryParse(光源亮度textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                _sensor.SetParam(enSensorParamType.smartRay_激光亮度, value);
            }
        }

        private void initCapture3DUI()
        {
            try
            {
                this.图像采集类型comboBox.DataSource = Enum.GetNames(typeof(Api.ImageAcquisitionType));
                int[] threshold = (int[])_sensor.GetParam(enSensorParamType.smartRay_激光线阈值);
                object type = _sensor.GetParam(enSensorParamType.smartRay_图像采集类型);
                uint lineCount = (uint)_sensor.GetParam(enSensorParamType.smartRay_扫描轮廓数量);
                /////////////
                this.测量点数textBox.Text = lineCount.ToString();
                if (threshold.Length > 1)
                {
                    this.激光线阈值1textBox.Text = threshold[0].ToString();
                    this.激光线阈值2textBox.Text = threshold[1].ToString();
                }
                else
                    this.激光线阈值1textBox.Text = threshold[0].ToString();
                ///////////////////
                this.曝光模式comboBox.Enabled = true;
                this.曝光时间1texBox.Enabled = true;
                this.曝光时间2texBox.Enabled = true;
                this.多重曝光合并模comboBox.Enabled = true;
                this.增益texBox.Enabled = true;
                this.光源亮度textBox.Enabled = true;
                this.光源模式comboBox.Enabled = true;
                this.激光线阈值1textBox.Enabled = true;
                this.激光线阈值2textBox.Enabled = true;
                this.测量点数textBox.Enabled = true;
                this.触发模式comBox.Enabled = true;
                this.内部触发频率textBox.Enabled = true;
                this.外部触发源comboBox.Enabled = true;
                this.触发分频textBox.Enabled = true;
                this.触发延时textBox.Enabled = true;
                this.触发方向comboBox.Enabled = true;
                switch ((Api.ImageAcquisitionType)type)
                {
                    case Api.ImageAcquisitionType.LiveImage:
                        this.图像采集类型comboBox.Text = Api.ImageAcquisitionType.LiveImage.ToString();
                        /////////////////////////////////
                        this.曝光模式comboBox.Enabled = false;
                        this.曝光时间1texBox.Enabled = true;
                        this.曝光时间2texBox.Enabled = true;
                        this.多重曝光合并模comboBox.Enabled = false;
                        this.增益texBox.Enabled = true;
                        this.光源亮度textBox.Enabled = true;
                        this.光源模式comboBox.Enabled = false;
                        this.激光线阈值1textBox.Enabled = false;
                        this.激光线阈值2textBox.Enabled = false;
                        this.测量点数textBox.Enabled = false;
                        this.触发模式comBox.Enabled = false;
                        this.内部触发频率textBox.Enabled = false;
                        this.外部触发源comboBox.Enabled = false;
                        this.触发分频textBox.Enabled = false;
                        this.触发延时textBox.Enabled = false;
                        this.触发方向comboBox.Enabled = false;
                        break;
                    case Api.ImageAcquisitionType.ProfileIntensityLaserLineThickness:
                        this.图像采集类型comboBox.Text = Api.ImageAcquisitionType.ProfileIntensityLaserLineThickness.ToString();
                        break;
                    case Api.ImageAcquisitionType.ZMapIntensityLaserLineThickness:
                        this.图像采集类型comboBox.Text = Api.ImageAcquisitionType.ZMapIntensityLaserLineThickness.ToString();
                        break;
                    case Api.ImageAcquisitionType.PointCloud:
                        this.图像采集类型comboBox.Text = Api.ImageAcquisitionType.PointCloud.ToString();
                        break;
                }
            }
            catch
            {

            }
        }
        private void 图像采集类型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.曝光模式comboBox.Enabled = true;
            this.曝光时间1texBox.Enabled = true;
            this.曝光时间2texBox.Enabled = true;
            this.多重曝光合并模comboBox.Enabled = true;
            this.增益texBox.Enabled = true;
            this.光源亮度textBox.Enabled = true;
            this.光源模式comboBox.Enabled = true;
            this.激光线阈值1textBox.Enabled = true;
            this.激光线阈值2textBox.Enabled = true;
            this.测量点数textBox.Enabled = true;
            this.触发模式comBox.Enabled = true;
            this.内部触发频率textBox.Enabled = true;
            this.外部触发源comboBox.Enabled = true;
            this.触发分频textBox.Enabled = true;
            this.触发延时textBox.Enabled = true;
            this.触发方向comboBox.Enabled = true;
            // _sensor.GetParam(enSmartRayParam.图像采集类型);
            switch (this.图像采集类型comboBox.Text)
            {
                case "LiveImage":
                    this._sensor.SetParam(enSensorParamType.smartRay_图像采集类型, Api.ImageAcquisitionType.LiveImage);
                    {
                        this.曝光模式comboBox.Enabled = false;
                        this.曝光时间1texBox.Enabled = true;
                        this.曝光时间2texBox.Enabled = true;
                        this.多重曝光合并模comboBox.Enabled = false;
                        this.增益texBox.Enabled = true;
                        this.光源亮度textBox.Enabled = true;
                        this.光源模式comboBox.Enabled = false;
                        this.激光线阈值1textBox.Enabled = false;
                        this.激光线阈值2textBox.Enabled = false;
                        this.测量点数textBox.Enabled = false;
                        this.触发模式comBox.Enabled = false;
                        this.内部触发频率textBox.Enabled = false;
                        this.外部触发源comboBox.Enabled = false;
                        this.触发分频textBox.Enabled = false;
                        this.触发延时textBox.Enabled = false;
                        this.触发方向comboBox.Enabled = false;
                    }
                    break;
                case "ProfileIntensityLaserLineThickness":
                    this._sensor.SetParam(enSensorParamType.smartRay_图像采集类型, Api.ImageAcquisitionType.ProfileIntensityLaserLineThickness);
                    break;
                case "ZMapIntensityLaserLineThickness":
                    this._sensor.SetParam(enSensorParamType.smartRay_图像采集类型, Api.ImageAcquisitionType.ZMapIntensityLaserLineThickness);
                    break;
                case "PointCloud":
                    this._sensor.SetParam(enSensorParamType.smartRay_图像采集类型, Api.ImageAcquisitionType.PointCloud);
                    break;
                default:
                    break;

            }
        }

        private void 激光线阈值1textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                int threshold1 = 0;
                int.TryParse(this.激光线阈值1textBox.Text, out threshold1);
                this._sensor.SetParam(enSensorParamType.smartRay_激光线阈值, new int[] { threshold1 });
            }
        }

        private void 激光线阈值2textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int threshold1 = 0;
                int threshold2 = 0;
                int.TryParse(this.激光线阈值1textBox.Text, out threshold1);
                int.TryParse(this.激光线阈值2textBox.Text, out threshold2);
                int[] threshold = (int[])_sensor.GetParam(enSensorParamType.smartRay_激光线阈值);
                if (threshold.Length > 1)
                    this._sensor.SetParam(enSensorParamType.smartRay_激光线阈值, new int[] { threshold1, threshold2 });
            }
        }

        private void 测量点数textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int value = 0;
                int.TryParse(this.测量点数textBox.Text, out value);
                this._sensor.SetParam(enSensorParamType.smartRay_扫描轮廓数量, value);
            }
        }


        private void initTriggerUI()
        {
            try
            {
                this.触发模式comBox.DataSource = Enum.GetNames(typeof(Api.DataTriggerMode));
                this.外部触发源comboBox.DataSource = Enum.GetNames(typeof(Api.DataTriggerSource));
                this.触发方向comboBox.DataSource = Enum.GetNames(typeof(enUserTriggerDirection));
                /////////////////////////////////////////////////
               // Api.DataTriggerMode triggerMode = (Api.DataTriggerMode)_sensor.GetParam(enSmartRayParam.触发模式);
                object[] triggerParam = (object[])_sensor.GetParam(enSensorParamType.smartRay_触发参数);
                /////////////////////////////////////////////////
                this.触发模式comBox.Text = triggerParam[0].ToString();
                ///////////////////
                switch ((Api.DataTriggerMode)triggerParam[0])
                {
                    case Api.DataTriggerMode.FreeRunning:
                        this.内部触发频率textBox.Enabled = false;
                        this.外部触发源comboBox.Enabled = false;
                        this.触发分频textBox.Enabled = false;
                        this.触发延时textBox.Enabled = false;
                        this.触发方向comboBox.Enabled = false;
                        break;
                    case Api.DataTriggerMode.Internal:
                        this.内部触发频率textBox.Enabled = true;
                        this.外部触发源comboBox.Enabled = false;
                        this.触发分频textBox.Enabled = false;
                        this.触发延时textBox.Enabled = false;
                        this.触发方向comboBox.Enabled = false;
                        /////
                        this.内部触发频率textBox.Text = _sensor.GetParam(enSensorParamType.smartRay_内部触发频率).ToString();
                        break;
                    case Api.DataTriggerMode.External:
                        this.内部触发频率textBox.Enabled = false;
                        this.外部触发源comboBox.Enabled = true;
                        this.触发分频textBox.Enabled = true;
                        this.触发延时textBox.Enabled = true;
                        this.触发方向comboBox.Enabled = true;
                        //////////
                        this.外部触发源comboBox.Text = triggerParam[1].ToString();
                        this.触发分频textBox.Text = triggerParam[2].ToString();
                        this.触发延时textBox.Text = triggerParam[3].ToString();
                        switch ((Api.TriggerEdgeMode)triggerParam[4])
                        {
                            case Api.TriggerEdgeMode.FallingEdge:
                                this.触发方向comboBox.Text = enUserTriggerDirection.AntiClockwiseBackward.ToString();
                                break;
                            case Api.TriggerEdgeMode.RisingEdge:
                                this.触发方向comboBox.Text = enUserTriggerDirection.ClockwiseForward.ToString();
                                break;
                            case Api.TriggerEdgeMode.Both:
                                this.触发方向comboBox.Text = enUserTriggerDirection.Both.ToString();
                                break;
                        }
                        break;
                }               
            }
            catch
            {

            }
        }

        private void 触发模式comBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string value = this.触发模式comBox.SelectedItem.ToString();
            int Frequency = 100;
            int.TryParse(内部触发频率textBox.Text, out Frequency);
            switch (value)
            {
                case "FreeRunning":
                    this.内部触发频率textBox.Enabled = false;
                    this.外部触发源comboBox.Enabled = false;
                    this.触发分频textBox.Enabled = false;
                    this.触发延时textBox.Enabled = false;
                    this.触发方向comboBox.Enabled = false;
                    this._sensor.SetParam(enSensorParamType.smartRay_自由触发, Api.DataTriggerMode.FreeRunning);
                    break;
                case "Internal":
                    this.内部触发频率textBox.Enabled = true;
                    this.外部触发源comboBox.Enabled = false;
                    this.触发分频textBox.Enabled = false;
                    this.触发延时textBox.Enabled = false;
                    this.触发方向comboBox.Enabled = false;
                    this._sensor.SetParam(enSensorParamType.smartRay_内部触发, Frequency);
                    break;
                case "External":
                    this.内部触发频率textBox.Enabled = false;
                    this.外部触发源comboBox.Enabled = true;
                    this.触发分频textBox.Enabled = true;
                    this.触发延时textBox.Enabled = true;
                    this.触发方向comboBox.Enabled = true;
                    外部触发源comboBox_SelectionChangeCommitted(null, null);
                    break;
            }
        }

        private void 内部触发频率textBox_KeyUp(object sender, KeyEventArgs e)
        {
            int value = 100;
            int.TryParse(内部触发频率textBox.Text, out value);
            if (e.KeyCode == Keys.Enter)
            {
                this._sensor.SetParam(enSensorParamType.smartRay_内部触发频率, value);
            }
        }

        private void 外部触发源comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.触发模式comBox.Text != "External") return;
            string value = this.外部触发源comboBox.SelectedItem.ToString();
            string triggerEdge = this.触发方向comboBox.SelectedItem.ToString(); //
            switch (triggerEdge)
            {
                case "ClockwiseForward":
                    SetTriggerMode(value, Api.TriggerEdgeMode.RisingEdge);
                    break;
                case "AntiClockwiseBackward":
                    SetTriggerMode(value, Api.TriggerEdgeMode.FallingEdge);
                    break;
                case "Both":
                    SetTriggerMode(value, Api.TriggerEdgeMode.Both);
                    break;
            }
        }

        private void 触发分频textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                外部触发源comboBox_SelectionChangeCommitted(null, null);
        }

        private void 触发延时textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                外部触发源comboBox_SelectionChangeCommitted(null, null);
        }

        private void 触发方向comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.触发模式comBox.Text != "External") return;
            string value = this.外部触发源comboBox.SelectedItem.ToString();
            string triggerEdge = this.触发方向comboBox.SelectedItem.ToString();
            switch (triggerEdge)
            {
                case "ClockwiseForward":
                    SetTriggerMode(value, Api.TriggerEdgeMode.RisingEdge);
                    break;
                case "AntiClockwiseBackward":
                    SetTriggerMode(value, Api.TriggerEdgeMode.FallingEdge);
                    break;
                case "Both":
                    SetTriggerMode(value, Api.TriggerEdgeMode.Both);
                    break;
            }
        }

        private void SetTriggerMode(object params1, Api.TriggerEdgeMode params2)
        {
            switch (params1.ToString())
            {
                case "Input1":
                    this.内部触发频率textBox.Enabled = false;
                    this.外部触发源comboBox.Enabled = true;
                    this.触发分频textBox.Enabled = true;
                    this.触发延时textBox.Enabled = true;
                    this.触发方向comboBox.Enabled = true;
                    this._sensor.SetParam(enSensorParamType.smartRay_外部触发, new object[4] { Api.DataTriggerSource.Input1, int.Parse(触发分频textBox.Text), int.Parse(触发延时textBox.Text), params2 });
                    break;
                case "Input2":
                    this.内部触发频率textBox.Enabled = false;
                    this.外部触发源comboBox.Enabled = true;
                    this.触发分频textBox.Enabled = true;
                    this.触发延时textBox.Enabled = true;
                    this.触发方向comboBox.Enabled = true;
                    this._sensor.SetParam(enSensorParamType.smartRay_外部触发, new object[4] { Api.DataTriggerSource.Input2, int.Parse(触发分频textBox.Text), int.Parse(触发延时textBox.Text), params2 });
                    break;
                case "Combined":
                    this.内部触发频率textBox.Enabled = false;
                    this.外部触发源comboBox.Enabled = true;
                    this.触发分频textBox.Enabled = true;
                    this.触发延时textBox.Enabled = true;
                    this.触发方向comboBox.Enabled = true;
                    this._sensor.SetParam(enSensorParamType.smartRay_外部触发, new object[4] { Api.DataTriggerSource.Combined, int.Parse(触发分频textBox.Text), int.Parse(触发延时textBox.Text), params2 });
                    break;
                case "QuadEncoder":
                    this.内部触发频率textBox.Enabled = false;
                    this.外部触发源comboBox.Enabled = true;
                    this.触发分频textBox.Enabled = true;
                    this.触发延时textBox.Enabled = true;
                    this.触发方向comboBox.Enabled = true;
                    this._sensor.SetParam(enSensorParamType.smartRay_外部触发, new object[4] { Api.DataTriggerSource.QuadEncoder, int.Parse(触发分频textBox.Text), int.Parse(触发延时textBox.Text), params2 });
                    break;
            }
        }



    }
}
