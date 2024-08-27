using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MotionControlCard
{
    public partial class DeviceConnectConfigParamManageForm : Form
    {
        private BindingList<DeviceConnectConfigParam> listConfigPara;
        public DeviceConnectConfigParamManageForm()
        {
            InitializeComponent();
        }

        private void DeviceConfigParamManageForm_Load(object sender, EventArgs e)
        {            // 读取传感器配置文件 
            //listConfigPara = XML<BindingList<SensorConfigParam>>.Read(Application.StartupPath + "\\" + "sensorConfig.txt");
            this.listConfigPara = DeviceConnectConfigParamManger.Instance.DeviceConfigParamList; // 使用一个全局的参数对象
            if (listConfigPara == null)
                listConfigPara = new BindingList<DeviceConnectConfigParam>();
            //////////////////////////////
            this.DeviceType.Items.Clear();
            this.DeviceType.ValueType = typeof(enDeviceType);
            foreach (enDeviceType item in Enum.GetValues(typeof(enDeviceType)))
                this.DeviceType.Items.Add(item);
            //////////////////////////////
            this.DeviceModel.Items.Clear();
            this.DeviceModel.ValueType = typeof(enDeviceModel);
            foreach (enDeviceModel item in Enum.GetValues(typeof(enDeviceModel)))
                this.DeviceModel.Items.Add(item);
            //////////////////////////////
            this.connectType.Items.Clear();
            this.connectType.ValueType = typeof(enUserConnectType);
            foreach (enUserConnectType item in Enum.GetValues(typeof(enUserConnectType)))
                this.connectType.Items.Add(item);
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
            /// 订阅所有设备的事件 
            if (listConfigPara != null)
            {
                foreach (var item in listConfigPara)
                {
                    if (MotionCardManage.GetCard(item.DeviceName) != null)
                        MotionCardManage.GetCard(item.DeviceName).AxisINPose += new PoseInfoEventHandler(this.SocketReciveData);
                }
            }
        }

        private void DeviceConfigParamManageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.listConfigPara != null)
                {
                    foreach (var item in listConfigPara)
                    {
                        if (MotionCardManage.GetCard(item.DeviceName) != null)
                            MotionCardManage.GetCard(item.DeviceName).AxisINPose -= new PoseInfoEventHandler(this.SocketReciveData);
                    }
                }
                //if (DeviceConnectConfigParamManger.Instance.Save())
                //    LoggerHelper.Error("传感器配置文件保存成功");
                //else
                //    LoggerHelper.Error("传感器配置文件保存失败");
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
                            DialogResult dialogResult = MessageBox.Show("确定删除吗？", "删除选项配置", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                this.listConfigPara.RemoveAt(e.RowIndex);
                            }
                            break;
                        case "Open":
                            MotionCardManage.GetCard(this.listConfigPara[e.RowIndex].DeviceName)?.Init(this.listConfigPara[e.RowIndex]);
                            this.dataGridView1.Refresh();
                            break;
                        case "Close":
                            MotionCardManage.GetCard(this.listConfigPara[e.RowIndex].DeviceName)?.UnInit();
                            this.listConfigPara[e.RowIndex].ConnectState = false;
                            this.listConfigPara[e.RowIndex].ReceiveData = "";
                            this.listConfigPara[e.RowIndex].SendData = "";
                            this.dataGridView1.Refresh();
                            break;
                        case "Send":
                            //string[] value = this.listConfigPara[e.RowIndex].SendData.Split(';');  //.TriggerToPlc
                            //if (value.Length > 0)
                            MotionCardManage.GetCard(this.listConfigPara[e.RowIndex].DeviceName)?.WriteValue(enDataTypes.Int, "test" + ".TriggerToPlc", "test" + this.listConfigPara[e.RowIndex].SendData);
                            //MotionCardManage.GetCard(this.listConfigPara[e.RowIndex].DeviceName)?.WriteValue(enDataTypes.SocketCommand, value[0], this.listConfigPara[e.RowIndex].SendData);
                            break;
                        case "CommandCol":
                            new CommandConfigForm(this.listConfigPara[e.RowIndex].DeviceName).Show();
                            break;
                    }
                }
            }
            catch
            {
            }
        }
        private void SocketReciveData(object send, PoseInfoEventArgs e)
        {
            foreach (var item in this.listConfigPara)
            {
                if (item.DeviceName == e.DeviceName)
                    item.ReceiveData = e.PoseInfo;
            }
            this.BeginInvoke(new Action(() => this.dataGridView1.Refresh()));// 事件处理方法运行在引发该事件的线程上,以异步的方式处理事件处理程序,防止阻塞
        }
        private void SocketConnectData(object send, EventArgs e)
        {
            this.BeginInvoke(new Action(() => this.dataGridView1.Refresh())); // 以异步的方式处理事件处理程序
        }
        private void SaveButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (DeviceConnectConfigParamManger.Instance.Save())
                    MessageBox.Show("配置文件保存成功");
            }
            catch
            {
                MessageBox.Show("配置文件保存失败");
            }
        }
    }
}
