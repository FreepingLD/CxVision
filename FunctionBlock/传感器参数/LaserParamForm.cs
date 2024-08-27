using Common;
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
    public partial class LaserParamForm : Form
    {
        private LaserParam _laserParam;
        private PointCloudAcqParam _acqParam;
        public LaserParamForm(LaserParam laserParam)
        {
            InitializeComponent();
            this._laserParam = laserParam;
        }
        public LaserParamForm(LaserParam laserParam, PointCloudAcqParam AcqParam)
        {
            InitializeComponent();
            this._laserParam = laserParam;
            this._acqParam = AcqParam;
        }

        private void CameraParamMangerForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._laserParam == null) return;
                this.触发源comboBox.DataSource = Enum.GetValues(typeof(enUserTriggerSource));
                this.触发模式comboBox.DataSource = Enum.GetValues(typeof(enUserTrigerMode));
                this.IO输出类型comboBox.DataSource = Enum.GetValues(typeof(enIoOutputMode));
                this.扫描轴comboBox.DataSource = Enum.GetValues(typeof(enScanAxis));
                this.设备模式comboBox.DataSource = Enum.GetValues(typeof(enDeviceMode));
                this.采集模式comboBox.DataSource = Enum.GetValues(typeof(enAcqMode));
                ////////////////////////////////////////////////////////////////////////////////
                this.采集模式comboBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.AcqMode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.SensorNameTextBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.SensorName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.DataWidthtextBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.DataWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.DataHeighttextBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.DataHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像宽缩放textBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.ScaleWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像高缩放textBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.ScaleHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.高度值缩放textBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.ScaleGrayValue), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用图像缩放checkBox.DataBindings.Add("Checked", this._laserParam, nameof(this._laserParam.EnableScale), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Resolution_XtextBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.Resolution_X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Resolution_YtextBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.Resolution_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Resolution_ZtextBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.Resolution_Z), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用X轴checkBox.DataBindings.Add("Checked", this._laserParam, nameof(this._laserParam.Enable_x), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用Y轴checkBox.DataBindings.Add("Checked", this._laserParam, nameof(this._laserParam.Enable_y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用Z轴checkBox.DataBindings.Add("Checked", this._laserParam, nameof(this._laserParam.Enable_z), true, DataSourceUpdateMode.OnPropertyChanged);
                this.X轴镜像checkBox.DataBindings.Add("Checked", this._laserParam, nameof(this._laserParam.IsMirrorX), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y轴镜像checkBox.DataBindings.Add("Checked", this._laserParam, nameof(this._laserParam.IsMirrorY), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Z轴镜像checkBox.DataBindings.Add("Checked", this._laserParam, nameof(this._laserParam.IsMirrorY), true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发源comboBox.DataBindings.Add("SelectedItem", this._laserParam, nameof(this._laserParam.TriggerSource), true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发模式comboBox.DataBindings.Add("SelectedItem", this._laserParam, nameof(this._laserParam.TriggerMode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.IO输出类型comboBox.DataBindings.Add("SelectedItem", this._laserParam, nameof(this._laserParam.IoOutputMode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发端口numericUpDown.DataBindings.Add("Value", this._laserParam, nameof(this._laserParam.TriggerPort), true, DataSourceUpdateMode.OnPropertyChanged);
                this.采集延时textBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.AcqWaiteTime), true, DataSourceUpdateMode.OnPropertyChanged);
                this.停顿时间textBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.WaiteTime), true, DataSourceUpdateMode.OnPropertyChanged);
                this.扫描轴comboBox.DataBindings.Add("SelectedItem", this._laserParam, nameof(this._laserParam.ScanAxis), true, DataSourceUpdateMode.OnPropertyChanged);
                this.扫描步长textBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.ScanStep), true, DataSourceUpdateMode.OnPropertyChanged);

                this.设备模式comboBox.DataBindings.Add("Text", this._laserParam, nameof(this._laserParam.DeviceMode), true, DataSourceUpdateMode.OnPropertyChanged);
                // 采集参数
                this.视图窗口comboBox.DataBindings.Add("Text", this._acqParam, nameof(this._acqParam.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {

            }
        }

        private void LaserParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this._laserParam.Save();
            }
            catch
            {

            }
        }

        private void 传感器参数button_Click(object sender, EventArgs e)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
