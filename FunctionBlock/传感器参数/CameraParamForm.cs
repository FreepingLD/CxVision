using Common;
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
    public partial class CameraParamForm : Form
    {
        private CameraParam _cameraParam;
        private ImageAcqParam _acqParam;
        public CameraParamForm(CameraParam cameraParam)
        {
            InitializeComponent();
            this._cameraParam = cameraParam;
            //this._cameraParam = new CameraParam();
        }
        public CameraParamForm(CameraParam cameraParam, ImageAcqParam acqParam)
        {
            InitializeComponent();
            this._cameraParam = cameraParam;
            this._acqParam = acqParam;
        }

        private void CameraParamMangerForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._cameraParam == null) return;
                this.触发源comboBox.DataSource = Enum.GetValues(typeof(enUserTriggerSource));
                this.触发模式comboBox.DataSource = Enum.GetValues(typeof(enUserTrigerMode));
                this.IO输出类型comboBox.DataSource = Enum.GetValues(typeof(enIoOutputMode));
                this.采集模式comboBox.DataSource = Enum.GetValues(typeof(enAcqMode));
                this.视图窗口comboBox.DataSource = HWindowManage.GetKeysList();
                /////////////////////////////////////////////////////
                this.采集模式comboBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.AcqMode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.平均值textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.AverangeCount), true, DataSourceUpdateMode.OnPropertyChanged);
                this.采集数量textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.AcqImageCount), true, DataSourceUpdateMode.OnPropertyChanged);
                this.采集超时textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.Timeout), true, DataSourceUpdateMode.OnPropertyChanged);
                this.SensorNameTextBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.SensorName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.DataWidthtextBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.DataWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.DataHeighttextBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.DataHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像宽缩放textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.ScaleWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像高缩放textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.ScaleHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用图像缩放checkBox.DataBindings.Add("Checked", this._cameraParam, nameof(this._cameraParam.EnableScale), true, DataSourceUpdateMode.OnPropertyChanged);
                this.像素当量textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.PixScale), true, DataSourceUpdateMode.OnPropertyChanged);
                this.X轴镜像checkBox.DataBindings.Add("Checked", this._cameraParam, nameof(this._cameraParam.IsMirrorX), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y轴镜像checkBox.DataBindings.Add("Checked", this._cameraParam, nameof(this._cameraParam.IsMirrorY), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.清空数据checkBox.DataBindings.Add("Checked", this._cameraParam, nameof(this._cameraParam.ClearData), true, DataSourceUpdateMode.OnPropertyChanged);
                this.旋转checkBox.DataBindings.Add("Checked", this._cameraParam, nameof(this._cameraParam.IsRot), true, DataSourceUpdateMode.OnPropertyChanged);
                this.CamSlanttextBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.CamSlant), true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发源comboBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.TriggerSource), true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发模式comboBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.TriggerMode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.IO输出类型comboBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.IoOutputMode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发端口numericUpDown.DataBindings.Add("Value", this._cameraParam, nameof(this._cameraParam.TriggerPort), true, DataSourceUpdateMode.OnPropertyChanged);
                this.采集延时textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.AcqWaiteTime), true, DataSourceUpdateMode.OnPropertyChanged);
                this.停顿时间textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.WaiteTime), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用相机畸变校正checkBox.DataBindings.Add("Checked", this._cameraParam, nameof(this._cameraParam.EnableDistoryRectify), true, DataSourceUpdateMode.OnPropertyChanged);
                this.配置文件textBox.DataBindings.Add("Text", this._cameraParam, nameof(this._cameraParam.ConfigPath), true, DataSourceUpdateMode.OnPropertyChanged);

                // 采集参数
                this.视图窗口comboBox.DataBindings.Add("Text", this._acqParam, nameof(this._acqParam.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////
                this.曝光textBox.Text = SensorManage.GetSensor(this._cameraParam.SensorName).GetParam("曝光").ToString();
                double gain = 0;
                double.TryParse(SensorManage.GetSensor(this._cameraParam.SensorName).GetParam("增益").ToString(), out gain);
                this.增益numericUpDown.Value = (decimal)gain ;
            }
            catch (Exception ex)
            {

            }
        }

        private void 采集button_Click(object sender, EventArgs e)
        {
            //SensorManage.SensorList[this._cameraParam.SensorName];
        }

        private void CameraParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this._cameraParam.Save();
            }
            catch
            {

            }
        }

        private void IO输出类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.IO输出类型comboBox.SelectedIndex == -1) return;
                switch (this.IO输出类型comboBox.SelectedItem.ToString())
                {
                    case nameof(enIoOutputMode.线性比较IO输出):
                        this.触发间隔textBox.Enabled = true;
                        this.触发端口numericUpDown.ReadOnly = false;
                        break;
                    case nameof(enIoOutputMode.NONE):
                        this.触发端口numericUpDown.ReadOnly = true;
                        break;
                    default:
                        this.触发间隔textBox.Enabled = false;
                        this.触发端口numericUpDown.ReadOnly = false;
                        break;
                }
            }
            catch
            {

            }
        }

        private void 传感器参数button_Click(object sender, EventArgs e)
        {
            try
            {
                new CamHomMatParamForm(this._cameraParam).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 曝光textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (曝光textBox.Text == null || 曝光textBox.Text.Trim().Length == 0) return;
                    SensorManage.GetSensor(this._cameraParam.SensorName).SetParam("曝光", 曝光textBox.Text);
                    int exposure = 0;
                    int.TryParse(曝光textBox.Text, out exposure);
                    this._cameraParam.Exposure = exposure;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 增益numericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (增益numericUpDown.Value < 0) return;
                    SensorManage.GetSensor(this._cameraParam.SensorName).SetParam("增益", 增益numericUpDown.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void exposeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (曝光textBox.Text == null || 曝光textBox.Text.Trim().Length == 0) return;
                SensorManage.GetSensor(this._cameraParam.SensorName).SetParam("曝光", 曝光textBox.Text);
                int exposure = 0;
                int.TryParse(曝光textBox.Text, out exposure);
                this._cameraParam.Exposure = exposure;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void gainBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (增益numericUpDown.Value < 0) return;
                SensorManage.GetSensor(this._cameraParam.SensorName).SetParam("增益", 增益numericUpDown.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 采集模式comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.采集模式comboBox.SelectedItem == null) return;
                switch (this.采集模式comboBox.SelectedItem.ToString())
                {
                    case "同步采集":
                        this.采集数量textBox.Enabled = false;
                        break;
                    case "异步采集":
                        this.采集数量textBox.Enabled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void readFileButton_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            string path = fo.OpenFile();
            try
            {
                if (path != null && path.Trim().Length > 0)
                {
                    this._cameraParam.ConfigPath = path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
