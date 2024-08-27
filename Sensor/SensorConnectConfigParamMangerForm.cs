using Common;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Sensor
{
    public partial class SensorConnectConfigParamMangerForm : Form
    {
        private BindingList<SensorConnectConfigParam> listConfigPara;
        public SensorConnectConfigParamMangerForm()
        {
            InitializeComponent();
        }

        private void SensorManageForm_Load(object sender, EventArgs e)
        {            // 读取传感器配置文件 
            //listConfigPara = XML<BindingList<SensorConfigParam>>.Read(Application.StartupPath + "\\" + "sensorConfig.txt");
            this.listConfigPara = SensorConnectConfigParamManger.Instance.ConfigParamList; // 使用一个全局的参数对象来控制
            if (listConfigPara == null)
                listConfigPara = new BindingList<SensorConnectConfigParam>();
            //////////////////////////////
            this.SensorType.Items.Clear();
            this.SensorType.ValueType = typeof(enUserSensorType);
            foreach (enUserSensorType item in Enum.GetValues(typeof(enUserSensorType)))
                this.SensorType.Items.Add(item);
            //////////////////////////////
            this.connectType.Items.Clear();
            this.connectType.ValueType = typeof(enUserConnectType);
            foreach (enUserConnectType item in Enum.GetValues(typeof(enUserConnectType)))
                this.connectType.Items.Add(item);
            /////////////////////////////
            this.HalInterfaceType.ValueType = typeof(enHalconInterfaceType);
            this.HalInterfaceType.Items.Clear();
            foreach (enHalconInterfaceType item in Enum.GetValues(typeof(enHalconInterfaceType)))
                this.HalInterfaceType.Items.Add(item);
            /////////////////////////////
            this.SensorBrand.ValueType = typeof(enSensorLinkLibrary);
            this.SensorBrand.Items.Clear();
            foreach (enSensorLinkLibrary item in Enum.GetValues(typeof(enSensorLinkLibrary)))
                this.SensorBrand.Items.Add(item);
            /////////////////////////////
            this.AcqMethodCol.ValueType = typeof(enImageAcqMethod);
            this.AcqMethodCol.Items.Clear();
            foreach (enImageAcqMethod item in Enum.GetValues(typeof(enImageAcqMethod)))
                this.AcqMethodCol.Items.Add(item);
            ////////////////////////////
            this.StopBits.Items.Clear();
            this.StopBits.ValueType = typeof(System.IO.Ports.StopBits);
            foreach (System.IO.Ports.StopBits item in Enum.GetValues(typeof(System.IO.Ports.StopBits)))
                this.StopBits.Items.Add(item);
            ////////////////////////////
            this.Parity.Items.Clear();
            this.Parity.ValueType = typeof(System.IO.Ports.Parity);
            foreach (System.IO.Ports.StopBits item in Enum.GetValues(typeof(System.IO.Ports.Parity)))
                this.Parity.Items.Add(item);
            ///////////////////////////////////////////////////// 添加项目一定要放到 数据源绑定源前面
            this.dataGridView1.DataSource = listConfigPara;
        }

        private void SensorManageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SensorConnectConfigParamManger.Instance.Save();
            }
            catch
            {
                LoggerHelper.Error("传感器配置文件保存报错");
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn":
                            //this.dataGridView1.Rows.RemoveAt(e.RowIndex);
                            this.listConfigPara.RemoveAt(e.RowIndex);
                            break;
                        case "OpenBtn":
                            SensorManage.GetSensor(this.listConfigPara[e.RowIndex].SensorName).Connect(this.listConfigPara[e.RowIndex]);
                            break;
                        case "AcqBtn":
                            SensorManage.GetSensor(this.listConfigPara[e.RowIndex].SensorName).StartTrigger();
                            SensorManage.GetSensor(this.listConfigPara[e.RowIndex].SensorName).StopTrigger();
                            Dictionary<enDataItem, object> list = SensorManage.GetSensor(this.listConfigPara[e.RowIndex].SensorName).ReadData();
                            this.hWindowControl1.HalconWindow.DispImage(((ImageDataClass)list[enDataItem.Image]).Image);
                            break;
                        case "SaveBtn":
                            SensorConnectConfigParamManger.Instance.Save();
                            break;
                        case "SetPara":
                            //switch (this.listConfigPara[e.RowIndex].SensorType)
                            //{
                            //    case enUserSensorType.点激光:
                            //    case enUserSensorType.线激光:
                            //    case enUserSensorType.面激光:
                            //        new LaserParamForm(SensorManage.GetSensor(this.listConfigPara[e.RowIndex].SensorName)?.LaserParam).Show();
                            //        break;
                            //    case enUserSensorType.线阵相机:
                            //    case enUserSensorType.面阵相机:
                            //        new CameraParamForm(SensorManage.GetSensor(this.listConfigPara[e.RowIndex].SensorName)?.CameraParam).Show();
                            //        break;
                            //}
                            break;
                    }
                    //this.dataGridView1.Refresh();
                }
            }
            catch
            {
            }
        }

        private void SaveButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (SensorConnectConfigParamManger.Instance.Save())
                    MessageBox.Show("配置文件保存成功");
            }
            catch
            {
                MessageBox.Show("配置文件保存失败");
            }
        }
    }
}
