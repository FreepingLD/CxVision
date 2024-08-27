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
using System.IO.Ports;
using MotionControlCard;
using MVSDK;
using Light;
using View;

namespace FunctionBlock
{
    public partial class AcqSourceForm : Form
    {
        private Form monitorForm = null;
        private Form form = null;
        private AcqSource _acqSource;
        private VisualizeView drawObject;

        public AcqSourceForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            /////////////////////////////           
        }
        public AcqSourceForm()
        {
            InitializeComponent();
            /////////////////////////////           
        }
        private void AcqSourceForm_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            try
            {
                this.采集源comboBox.Text = this._acqSource.Name;
                this.UpdataData(this._acqSource);
                //////
                switch (this._acqSource.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        AddForm(this.采集源参数tabPage, new LaserParamForm(this._acqSource.Sensor.LaserParam));
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        AddForm(this.采集源参数tabPage, new CameraParamForm(this._acqSource.Sensor.CameraParam));
                        break;
                    default:
                        AddForm(this.采集源参数tabPage, new CameraParamForm(null));
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdataData(AcqSource acqSource)
        {
            try
            {
                if (acqSource == null) return;
                this.传感器名称comboBox.DataSource = SensorManage.GetSensorName();
                this.运动坐标系comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));//
                this.运动轴comboBox.DataSource = Enum.GetValues(typeof(enAxisName));//
                //this.标定矩阵comboBox.DataSource = Enum.GetValues(typeof(enAxisName));//
                /////////////////////////////////////////////////////////////////////////////////
                this.LightControlColumn.Items.Clear();
                this.LightControlColumn.ValueType = typeof(string);
                foreach (var item in global::Light.LightConnectManage.GetLightName())
                {
                    this.LightControlColumn.Items.Add(item);
                }
                ///////////////////////////////////////////
                this.LightChennelColumn.Items.Clear();
                this.LightChennelColumn.ValueType = typeof(global::Light.enLightChannel);
                foreach (var item in Enum.GetValues(typeof(global::Light.enLightChannel)))
                {
                    this.LightChennelColumn.Items.Add(item);
                }
                this.光源dataGridView1.DataSource = acqSource?.LightParamList;
                this.传感器listBox.DataSource = acqSource?.SensorName;
                ///  绑定机台坐标
                //this.运动坐标系comboBox.DataBindings.Add("SelectedItem", acqSource, nameof(acqSource.MotionCoordSysName), true, DataSourceUpdateMode.OnPropertyChanged); //WaiteTime
                this.运动坐标系comboBox.DataBindings.Add("SelectedItem", acqSource, nameof(acqSource.CoordSysName), true, DataSourceUpdateMode.OnPropertyChanged); //WaiteTime
                this.运动轴comboBox.DataBindings.Add("SelectedItem", acqSource, nameof(acqSource.MoveAxisName), true, DataSourceUpdateMode.OnPropertyChanged); //WaiteTime
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void AddForm(Control MastPanel, Form form)
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

        private void AcqSourceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (form != null) form.Close();
            if (monitorForm != null) monitorForm.Close();
            //AcqSourceManage.Instance.Save();
            this._acqSource.Sensor?.CameraParam.Save();
        }

        private void 传感器参数设置button_Click(object sender, EventArgs e)
        {
            if (this._acqSource == null || this._acqSource.Sensor == null) return;
            if (form != null) form.Close();
            /////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                switch (this._acqSource.Sensor.GetType().Name)
                {
                    case "CStil_L":
                        form = new StilLineLaserParamForm(this._acqSource); // 这里的代码比绑定中的代码先执行，所以这里只能通过键值来获取，基于事件的程序执行存在随机性
                        form.Owner = this;
                        form.Show();
                        break;
                    case "CStil_P":
                        form = new StilPointLaserParamForm(this._acqSource);
                        form.Owner = this;
                        form.Show();
                        break;
                    case "LiYiPointLaser":
                        form = new LiYiPointLaserParamForm(this._acqSource);
                        form.Owner = this;
                        form.Show();
                        break;
                    case "SSZNLineLaser":
                        break;
                    case "DaHengCamera":
                        form = new DaHengCameraParamForm(this._acqSource);
                        form.Owner = this;
                        form.Show();
                        break;
                    case "BoMingPointLaser":
                        form = new BoMingPointLaserParamForm(this._acqSource);
                        form.Owner = this;
                        form.Show();
                        break;
                    case "BoMingStructuredLight":
                        form = new BoMingStructuredLightFrom(this._acqSource);
                        form.Owner = this;
                        form.Show();
                        break;

                    case "SmartRayLineLaser":
                        form = new SmartRayLineLaserParamForm(this._acqSource);
                        form.Owner = this;
                        form.Show();
                        break;
                    case "LVM_LineLaser":
                        form = new LVMLineLaserParamForm(this._acqSource);
                        form.Owner = this;
                        form.Show();
                        break;

                    case "MdvsCamera":
                        tSdkCameraDevInfo[] tCameraDevInfoList;
                        int m_hCamera = (int)this._acqSource.Sensor.GetParam(enSensorParamType.MindVision_相机句柄);
                        if (m_hCamera > 0)
                        {
                            MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
                            MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);
                            MvApi.CameraShowSettingPage(m_hCamera, 1);//1 show ; 0 hide
                        }
                        break;

                    case "GbsFaceWliLocal":
                    case "GbsFaceWliRemote":
                        form = new GBSWliParamForm(this._acqSource, true); // 传入true,表示在关闭窗体时不会断开连接
                        form.Owner = this;
                        form.Show();
                        break;

                    default:
                        //AddForm(this.panel3, new Form());
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 光源dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (光源dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn":
                            this._acqSource?.LightParamList.RemoveAt(e.RowIndex);
                            this.光源dataGridView1.Rows.RemoveAt(e.RowIndex);
                            break;
                        case "SetLight":
                            LightConnectManage.GetLight(this._acqSource.LightParamList[e.RowIndex].LightName)?.SetLight(this._acqSource.LightParamList[e.RowIndex].Channel, this._acqSource.LightParamList[e.RowIndex].LightValue);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 光源dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.采集源comboBox.SelectedIndex == -1) return;
                AcqSource acqSource = this.采集源comboBox.SelectedItem as AcqSource;
                this._acqSource = acqSource;
                this.UpdataData(acqSource);
                //////
                switch (acqSource.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        AddForm(this.采集源参数tabPage, new LaserParamForm(acqSource.Sensor.LaserParam));
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        AddForm(this.采集源参数tabPage, new CameraParamForm(acqSource.Sensor.CameraParam));
                        break;
                    default:
                        AddForm(this.采集源参数tabPage, new CameraParamForm(null));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 图像采集btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._acqSource == null) return;
                Dictionary<enDataItem, object> data = this._acqSource?.AcqImageData(null);
                switch (this._acqSource.Sensor?.ConfigParam.SensorType)
                {
                    case enUserSensorType.面阵相机:
                        if (data?.Count > 0)
                        {
                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 图像保存btn_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|所有文件(*.*)|*.**";
                fileDialog.RestoreDirectory = false;
                fileDialog.FilterIndex = 0;
                fileDialog.ShowDialog();
                this.drawObject.BackImage.Image?.WriteImage("bmp", 0, fileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
