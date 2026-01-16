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
using System.Threading;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public partial class DeviceCommunicationConfigForm : Form
    {
        private int _curIndex = 0;
        public bool _userEnable;
        public bool UserEnable
        {
            get
            {
                return _userEnable;
            }
            set
            {
                this._userEnable = value;
                if (this._userEnable)
                {
                    this.dataGridView1.ReadOnly = false;
                    this.SaveButton.Enabled = true;
                    this.清空配置Btn.Enabled = true;
                    this.通信命令配置label.Enabled = true;
                    this.通信配置1Btn.Enabled = true;
                    this.通信配置2Btn.Enabled = true;
                    this.通信配置3Btn.Enabled = true;
                    this.通信配置4Btn.Enabled = true;
                    this.通信配置5Btn.Enabled = true;
                    this.通信配置6Btn.Enabled = true;
                    this.通信配置7Btn.Enabled = true;
                    this.通信配置8Btn.Enabled = true;
                    this.通信配置9Btn.Enabled = true;
                    this.通信配置10Btn.Enabled = true;
                    this.通信配置11Btn.Enabled = true;
                    this.通信配置12Btn.Enabled = true;
                    this.通信配置13Btn.Enabled = true;
                    this.通信配置14Btn.Enabled = true;
                    this.通信配置15Btn.Enabled = true;
                }
                else
                {
                    this.dataGridView1.ReadOnly = true;
                    this.SaveButton.Enabled = false;
                    this.清空配置Btn.Enabled = false;
                    this.通信命令配置label.Enabled = false;
                    this.通信配置1Btn.Enabled = false;
                    this.通信配置2Btn.Enabled = false;
                    this.通信配置3Btn.Enabled = false;
                    this.通信配置4Btn.Enabled = false;
                    this.通信配置5Btn.Enabled = false;
                    this.通信配置6Btn.Enabled = false;
                    this.通信配置7Btn.Enabled = false;
                    this.通信配置8Btn.Enabled = false;
                    this.通信配置9Btn.Enabled = false;
                    this.通信配置10Btn.Enabled = false;
                    this.通信配置11Btn.Enabled = false;
                    this.通信配置12Btn.Enabled = false;
                    this.通信配置13Btn.Enabled = false;
                    this.通信配置14Btn.Enabled = false;
                    this.通信配置15Btn.Enabled = false;
                }
            }
        }

        private BindingList<BindingList<CommunicationConfigParam>> listConfigPara;

        private CancellationTokenSource cts;

        public DeviceCommunicationConfigForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void DeviceConfigParamManageForm_Load(object sender, EventArgs e)
        {
            // 读取传感器配置文件 
            this.listConfigPara = CommunicationConfigParamManger.Instance.CommunicationParamList; // 使用一个全局的参数对象, 配置文件统一在某个地方读取
            if (listConfigPara == null)
                listConfigPara = new BindingList<BindingList<CommunicationConfigParam>>();
            //////////////////////////////////////////////////////////////////////////////
            this.通信配置1Btn_Click(null, null);
        }


        private void DeviceCommunicationConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cts?.Cancel();
            }
            catch
            {
                LoggerHelper.Error("通信配置文件保存报错");
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.listConfigPara.Clear();
            }
            catch (Exception ex)
            {

            }
        }


        private void SaveButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (CommunicationConfigParamManger.Instance.Save())
                    new UserMessageForm().ShowDialog("配置文件保存成功");
            }
            catch
            {
                new UserMessageForm().ShowDialog("配置文件保存失败");
            }
        }



        private void Updata()
        {
            if (this.cts != null && !this.cts.IsCancellationRequested)
                this.cts.Cancel();
            this.cts = new CancellationTokenSource();
            while (true)
            {
                if (this.cts.IsCancellationRequested) break;
                foreach (var item in this.listConfigPara)
                {
                    foreach (var item2 in item)
                    {
                        if (item2.Active)
                            item2.ReadValue = CommunicationConfigParamManger.Instance.ReadValue(item2).ToString();
                    }
                }
                //Application.DoEvents();
                Thread.Sleep(200);
            }
        }

        private void 实时刷新checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (this.实时刷新checkBox.Checked)
                //    this.Updata();
                //else
                //    this.cts.Cancel();
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }



        private void 清空配置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = new UserMessageForm().ShowDialog("确定要清空参数吗?", "清空参数");
                if (dialogResult == DialogResult.OK)
                    CommunicationConfigParamManger.Instance.Clear();
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void initBtn(Button curBtn)
        {
            this.通信配置1Btn.BackColor = SystemColors.Control;
            this.通信配置2Btn.BackColor = SystemColors.Control;
            this.通信配置3Btn.BackColor = SystemColors.Control;
            this.通信配置4Btn.BackColor = SystemColors.Control;
            this.通信配置5Btn.BackColor = SystemColors.Control;
            this.通信配置6Btn.BackColor = SystemColors.Control;
            this.通信配置7Btn.BackColor = SystemColors.Control;
            this.通信配置8Btn.BackColor = SystemColors.Control;
            this.通信配置9Btn.BackColor = SystemColors.Control;
            this.通信配置10Btn.BackColor = SystemColors.Control;
            this.通信配置11Btn.BackColor = SystemColors.Control;
            this.通信配置12Btn.BackColor = SystemColors.Control;
            this.通信配置13Btn.BackColor = SystemColors.Control;
            this.通信配置14Btn.BackColor = SystemColors.Control;
            this.通信配置15Btn.BackColor = SystemColors.Control;
            curBtn.BackColor = SystemColors.Window;    
        }

        private void 通信配置1Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 0;
                this.通信命令配置label.Text = "通信配置1";
                this.initBtn(this.通信配置1Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置2Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 1;
                this.通信命令配置label.Text = "通信配置2";
                this.initBtn(this.通信配置2Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置3Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 2;
                this.通信命令配置label.Text = "通信配置3";
                this.initBtn(this.通信配置3Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置4Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 3;
                this.通信命令配置label.Text = "通信配置4";
                this.initBtn(this.通信配置4Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置5Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 4;
                this.通信命令配置label.Text = "通信配置5";
                this.initBtn(this.通信配置5Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置6Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 5;
                this.通信命令配置label.Text = "通信配置6";
                this.initBtn(this.通信配置6Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置7Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 6;
                this.通信命令配置label.Text = "通信配置7";
                this.initBtn(this.通信配置7Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置8Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 7;
                this.通信命令配置label.Text = "通信配置8";
                this.initBtn(this.通信配置8Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置9Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 8;
                this.通信命令配置label.Text = "通信配置9";
                this.initBtn(this.通信配置9Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置10Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 9;
                this.通信命令配置label.Text = "通信配置10";
                this.initBtn(this.通信配置10Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置11Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 10;
                this.通信命令配置label.Text = "通信配置11";
                this.initBtn(this.通信配置11Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置12Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 11;
                this.通信命令配置label.Text = "通信配置12";
                this.initBtn(this.通信配置12Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置13Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 12;
                this.通信命令配置label.Text = "通信配置13";
                this.initBtn(this.通信配置13Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置14Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 13;
                this.通信命令配置label.Text = "通信配置14";
                this.initBtn(this.通信配置14Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通信配置15Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this._curIndex = 14;
                this.通信命令配置label.Text = "通信配置15";
                this.initBtn(this.通信配置15Btn);
                /// 添加数据类型
                this.DataTypeColumn.Items.Clear();
                this.DataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.DataTypeColumn.Items.Add(item);
                /// 添加轴读写状态
                this.AxisReadWrite.Items.Clear();
                this.AxisReadWrite.ValueType = typeof(enAxisReadWriteState);
                foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                    this.AxisReadWrite.Items.Add(item);
                /// 添加运动设备名称 CommuniteColumn
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////映射坐标系 ///////////////////////
                this.MapCooreSysNameColumn.Items.Clear();
                this.MapCooreSysNameColumn.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.MapCooreSysNameColumn.Items.Add(item);
                /// 添加运动设备名称 
                this.CommuniteColumn.Items.Clear();
                this.CommuniteColumn.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommuniteColumn.Items.Add(item);
                ///////////////////////////////////////////////
                if (listConfigPara.Count > this._curIndex)
                {
                    this.dataGridView1.DataSource = listConfigPara[this._curIndex];
                    this.dataGridView1.DoubleBuffere(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    if (e.RowIndex >= 0)
                    {
                        switch (dataGridView1.Columns[e.ColumnIndex].Name)
                        {
                            case "DeleteBtn":
                                if (e.RowIndex < 0) return;
                                this.listConfigPara[this._curIndex].RemoveAt(e.RowIndex);
                                break;
                            case "ReadBtn":
                                this.listConfigPara[this._curIndex][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[this._curIndex][e.RowIndex])?.ToString();
                                break;
                            case "WriteBtn":
                                CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[this._curIndex][e.RowIndex]);
                                break;
                            case "InsertBtn":
                                this.listConfigPara[this._curIndex].Insert(e.RowIndex, new CommunicationConfigParam());
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }



    }

}
