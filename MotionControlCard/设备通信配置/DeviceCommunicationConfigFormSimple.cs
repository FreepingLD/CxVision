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
    public partial class DeviceCommunicationConfigFormSimple : Form
    {
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
                    this.dataGridView2.ReadOnly = false;
                    this.dataGridView3.ReadOnly = false;
                    this.dataGridView4.ReadOnly = false;
                    this.dataGridView5.ReadOnly = false;
                    this.dataGridView6.ReadOnly = false;
                    this.dataGridView7.ReadOnly = false;
                    //this.dataGridView8.ReadOnly = false;
                    //this.dataGridView9.ReadOnly = false;
                    //this.dataGridView10.ReadOnly = false;
                    //this.dataGridView11.ReadOnly = false;
                    //this.dataGridView12.ReadOnly = false;
                    //this.dataGridView13.ReadOnly = false;
                    //this.dataGridView14.ReadOnly = false;
                    //this.dataGridView15.ReadOnly = false;
                    this.SaveButton.Enabled = true;
                    this.清空配置Btn.Enabled = true;
                    this.通信命令配置label.Enabled = true;
                }
                else
                {
                    this.dataGridView1.ReadOnly = true;
                    this.dataGridView2.ReadOnly = true;
                    this.dataGridView3.ReadOnly = true;
                    this.dataGridView4.ReadOnly = true;
                    this.dataGridView5.ReadOnly = true;
                    this.dataGridView6.ReadOnly = true;
                    this.dataGridView7.ReadOnly = true;
                    //this.dataGridView8.ReadOnly = true;
                    //this.dataGridView9.ReadOnly = true;
                    //this.dataGridView10.ReadOnly = true;
                    //this.dataGridView11.ReadOnly = true;
                    //this.dataGridView12.ReadOnly = true;
                    //this.dataGridView13.ReadOnly = true;
                    //this.dataGridView14.ReadOnly = true;
                    //this.dataGridView15.ReadOnly = true;
                    this.SaveButton.Enabled = false;
                    this.清空配置Btn.Enabled = false;
                    this.通信命令配置label.Enabled = false;
                }
            }
        }


        private BindingList<BindingList<CommunicationConfigParam>> listConfigPara;

        private CancellationTokenSource cts;
        public DeviceCommunicationConfigFormSimple()
        {
            InitializeComponent();
            //this.DoubleBuffered = true;
        }

        private void DeviceConfigParamManageForm_Load(object sender, EventArgs e)
        {
            //return;
            // 读取传感器配置文件 
            this.listConfigPara = CommunicationConfigParamManger.Instance.CommunicationParamList; // 使用一个全局的参数对象, 配置文件统一在某个地方读取
            if (this.listConfigPara == null)
                this.listConfigPara = new BindingList<BindingList<CommunicationConfigParam>>();
            ///////////////////////////////////////////////////// 添加项目一定要放到 数据源绑定源前面
            for (int i = 0; i < listConfigPara.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        this.InitDataGridView1(listConfigPara[i]);
                        break;
                        //case 1:
                        //    this.InitDataGridView2(listConfigPara[i]);
                        //    break;
                        //case 2:
                        //    this.InitDataGridView3(listConfigPara[i]);
                        //    break;
                        //case 3:
                        //    this.InitDataGridView4(listConfigPara[i]);
                        //    break;
                        //case 4:
                        //    this.InitDataGridView5(listConfigPara[i]);
                        //    break;
                        //case 5:
                        //    this.InitDataGridView6(listConfigPara[i]);
                        //    break;
                        //case 6:
                        //    this.InitDataGridView7(listConfigPara[i]);
                        //    break;
                        //case 7:
                        //    this.InitDataGridView8(listConfigPara[i]);
                        //    break;
                        //case 8:
                        //    this.InitDataGridView9(listConfigPara[i]);
                        //    break;
                        //case 9:
                        //    this.InitDataGridView10(listConfigPara[i]);
                        //    break;
                        //case 10:
                        //    this.InitDataGridView11(listConfigPara[i]);
                        //    break;
                        //case 11:
                        //    this.InitDataGridView12(listConfigPara[i]);
                        //    break;
                        //case 12:
                        //    this.InitDataGridView13(listConfigPara[i]);
                        //    break;
                        //case 13:
                        //    this.InitDataGridView14(listConfigPara[i]);
                        //    break;
                        //case 14:
                        //    this.InitDataGridView15(listConfigPara[i]);
                        //    break;
                }
            }
        }

        private void InitDataGridView1(BindingList<CommunicationConfigParam> listConfigPara)
        {
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
            //////////////////////
            this.dataGridView1.DataSource = listConfigPara;
            this.dataGridView1.DoubleBuffere(true);
        }
        private void InitDataGridView2(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn2.Items.Clear();
            this.DataTypeColumn2.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn2.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite2.Items.Clear();
            this.AxisReadWrite2.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite2.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn2.Items.Clear();
            this.CooreSysNameColumn2.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn2.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn2.Items.Clear();
            this.MapCooreSysNameColumn2.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn2.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn2.Items.Clear();
            this.CommuniteColumn2.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn2.Items.Add(item);
            //////////////////////
            this.dataGridView2.DataSource = listConfigPara;
            this.dataGridView2.DoubleBuffere(true);
        }
        private void InitDataGridView3(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn3.Items.Clear();
            this.DataTypeColumn3.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn3.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite3.Items.Clear();
            this.AxisReadWrite3.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite3.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn3.Items.Clear();
            this.CooreSysNameColumn3.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn3.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn3.Items.Clear();
            this.MapCooreSysNameColumn3.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn3.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn3.Items.Clear();
            this.CommuniteColumn3.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn3.Items.Add(item);
            //////////////////////
            this.dataGridView3.DataSource = listConfigPara;
            this.dataGridView3.DoubleBuffere(true);
        }
        private void InitDataGridView4(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn4.Items.Clear();
            this.DataTypeColumn4.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn4.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite4.Items.Clear();
            this.AxisReadWrite4.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite4.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn4.Items.Clear();
            this.CooreSysNameColumn4.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn4.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn4.Items.Clear();
            this.MapCooreSysNameColumn4.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn4.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn4.Items.Clear();
            this.CommuniteColumn4.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn4.Items.Add(item);
            //////////////////////
            this.dataGridView4.DataSource = listConfigPara;
            this.dataGridView4.DoubleBuffere(true);
        }
        private void InitDataGridView5(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn5.Items.Clear();
            this.DataTypeColumn5.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn5.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite5.Items.Clear();
            this.AxisReadWrite5.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite5.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn5.Items.Clear();
            this.CooreSysNameColumn5.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn5.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn5.Items.Clear();
            this.MapCooreSysNameColumn5.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn5.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn5.Items.Clear();
            this.CommuniteColumn5.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn5.Items.Add(item);
            //////////////////////
            this.dataGridView5.DataSource = listConfigPara;
            this.dataGridView5.DoubleBuffere(true);
        }
        private void InitDataGridView6(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn6.Items.Clear();
            this.DataTypeColumn6.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn6.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite6.Items.Clear();
            this.AxisReadWrite6.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite6.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn6.Items.Clear();
            this.CooreSysNameColumn6.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn6.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn6.Items.Clear();
            this.MapCooreSysNameColumn6.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn6.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn6.Items.Clear();
            this.CommuniteColumn6.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn6.Items.Add(item);
            //////////////////////
            this.dataGridView6.DataSource = listConfigPara;
            this.dataGridView6.DoubleBuffere(true);
        }
        private void InitDataGridView7(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn7.Items.Clear();
            this.DataTypeColumn7.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn7.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite7.Items.Clear();
            this.AxisReadWrite7.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite7.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn7.Items.Clear();
            this.CooreSysNameColumn7.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn7.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn7.Items.Clear();
            this.MapCooreSysNameColumn7.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn7.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn7.Items.Clear();
            this.CommuniteColumn7.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn7.Items.Add(item);
            //////////////////////
            this.dataGridView7.DataSource = listConfigPara;
            this.dataGridView7.DoubleBuffere(true);
        }
        //private void InitDataGridView8(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn8.Items.Clear();
        //    this.DataTypeColumn8.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn8.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite8.Items.Clear();
        //    this.AxisReadWrite8.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite8.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn8.Items.Clear();
        //    this.CooreSysNameColumn8.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn8.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn8.Items.Clear();
        //    this.MapCooreSysNameColumn8.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn8.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn8.Items.Clear();
        //    this.CommuniteColumn8.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn8.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView8.DataSource = listConfigPara;
        //    this.dataGridView8.DoubleBuffere(true);
        //}
        //private void InitDataGridView9(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn9.Items.Clear();
        //    this.DataTypeColumn9.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn9.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite9.Items.Clear();
        //    this.AxisReadWrite9.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite9.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn9.Items.Clear();
        //    this.CooreSysNameColumn9.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn9.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn9.Items.Clear();
        //    this.MapCooreSysNameColumn9.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn9.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn9.Items.Clear();
        //    this.CommuniteColumn9.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn9.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView9.DataSource = listConfigPara;
        //    this.dataGridView9.DoubleBuffere(true);
        //}
        //private void InitDataGridView10(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn10.Items.Clear();
        //    this.DataTypeColumn10.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn10.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite10.Items.Clear();
        //    this.AxisReadWrite10.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite10.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn10.Items.Clear();
        //    this.CooreSysNameColumn10.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn10.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn10.Items.Clear();
        //    this.MapCooreSysNameColumn10.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn10.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn10.Items.Clear();
        //    this.CommuniteColumn10.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn10.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView10.DataSource = listConfigPara;
        //    this.dataGridView10.DoubleBuffere(true);
        //}
        //private void InitDataGridView11(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn11.Items.Clear();
        //    this.DataTypeColumn11.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn11.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite11.Items.Clear();
        //    this.AxisReadWrite11.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite11.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn11.Items.Clear();
        //    this.CooreSysNameColumn11.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn11.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn11.Items.Clear();
        //    this.MapCooreSysNameColumn11.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn11.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn11.Items.Clear();
        //    this.CommuniteColumn11.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn11.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView11.DataSource = listConfigPara;
        //    this.dataGridView11.DoubleBuffere(true);
        //}
        //private void InitDataGridView12(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn12.Items.Clear();
        //    this.DataTypeColumn12.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn12.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite12.Items.Clear();
        //    this.AxisReadWrite12.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite12.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn12.Items.Clear();
        //    this.CooreSysNameColumn12.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn12.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn12.Items.Clear();
        //    this.MapCooreSysNameColumn12.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn12.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn12.Items.Clear();
        //    this.CommuniteColumn12.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn12.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView12.DataSource = listConfigPara;
        //    this.dataGridView12.DoubleBuffere(true);
        //}
        //private void InitDataGridView13(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn13.Items.Clear();
        //    this.DataTypeColumn13.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn13.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite13.Items.Clear();
        //    this.AxisReadWrite13.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite13.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn13.Items.Clear();
        //    this.CooreSysNameColumn13.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn13.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn13.Items.Clear();
        //    this.MapCooreSysNameColumn13.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn13.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn13.Items.Clear();
        //    this.CommuniteColumn13.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn13.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView13.DataSource = listConfigPara;
        //    this.dataGridView13.DoubleBuffere(true);
        //}
        //private void InitDataGridView14(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn14.Items.Clear();
        //    this.DataTypeColumn14.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn14.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite14.Items.Clear();
        //    this.AxisReadWrite14.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite14.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn14.Items.Clear();
        //    this.CooreSysNameColumn14.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn14.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn14.Items.Clear();
        //    this.MapCooreSysNameColumn14.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn14.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn14.Items.Clear();
        //    this.CommuniteColumn14.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn14.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView14.DataSource = listConfigPara;
        //    this.dataGridView14.DoubleBuffere(true);
        //}
        //private void InitDataGridView15(BindingList<CommunicationConfigParam> listConfigPara)
        //{
        //    /// 添加数据类型
        //    this.DataTypeColumn15.Items.Clear();
        //    this.DataTypeColumn15.ValueType = typeof(enDataTypes);
        //    foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
        //        this.DataTypeColumn15.Items.Add(item);
        //    /// 添加轴读写状态
        //    this.AxisReadWrite15.Items.Clear();
        //    this.AxisReadWrite15.ValueType = typeof(enAxisReadWriteState);
        //    foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
        //        this.AxisReadWrite15.Items.Add(item);
        //    /// 添加运动设备名称 CommuniteColumn
        //    this.CooreSysNameColumn15.Items.Clear();
        //    this.CooreSysNameColumn15.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.CooreSysNameColumn15.Items.Add(item);
        //    ///////////映射坐标系 ///////////////////////
        //    this.MapCooreSysNameColumn15.Items.Clear();
        //    this.MapCooreSysNameColumn15.ValueType = typeof(enCoordSysName);
        //    foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
        //        this.MapCooreSysNameColumn15.Items.Add(item);
        //    /// 添加运动设备名称 
        //    this.CommuniteColumn15.Items.Clear();
        //    this.CommuniteColumn15.ValueType = typeof(enCommunicationCommand);
        //    foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
        //        this.CommuniteColumn15.Items.Add(item);
        //    //////////////////////
        //    this.dataGridView15.DataSource = listConfigPara;
        //    this.dataGridView15.DoubleBuffere(true);
        //}

        private void ClearDataBinding()
        {
            this.dataGridView1.DataBindings.Clear();
            this.dataGridView2.DataBindings.Clear();
            this.dataGridView3.DataBindings.Clear();
            this.dataGridView4.DataBindings.Clear();
            this.dataGridView5.DataBindings.Clear();
            this.dataGridView6.DataBindings.Clear();
            this.dataGridView7.DataBindings.Clear();
            //this.dataGridView8.DataBindings.Clear();
            //this.dataGridView9.DataBindings.Clear();
            //this.dataGridView10.DataBindings.Clear();
            //this.dataGridView11.DataBindings.Clear();
            //this.dataGridView12.DataBindings.Clear();
            //this.dataGridView13.DataBindings.Clear();
            //this.dataGridView14.DataBindings.Clear();
            //this.dataGridView15.DataBindings.Clear();
            //this.dataGridView1.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView2.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView3.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView4.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView5.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView6.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView7.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView8.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView9.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView10.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView11.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView12.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView13.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView14.DataSource = new BindingList<CommunicationConfigParam>();
            //this.dataGridView15.DataSource = new BindingList<CommunicationConfigParam>();
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

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            switch (dataGridView1.Columns[e.ColumnIndex].Name)
                            {
                                case "DeleteBtn":
                                    if (e.RowIndex < 0) return;
                                    this.listConfigPara[0].RemoveAt(e.RowIndex);
                                    break;
                                case "ReadBtn":
                                    this.listConfigPara[0][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[0][e.RowIndex])?.ToString();
                                    break;
                                case "WriteBtn":
                                    CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[0][e.RowIndex]);
                                    break;
                                case "InsertBtn":
                                    this.listConfigPara[0].Insert(e.RowIndex, new CommunicationConfigParam());
                                    break;
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            switch (dataGridView2.Columns[e.ColumnIndex].Name)
                            {
                                case "DeleteBtn2":
                                    if (e.RowIndex < 0) return;
                                    this.listConfigPara[1].RemoveAt(e.RowIndex);
                                    break;
                                case "ReadBtn2":
                                    this.listConfigPara[1][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[1][e.RowIndex])?.ToString();
                                    break;
                                case "WriteBtn2":
                                    CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[1][e.RowIndex]);
                                    break;
                                case "InsertBtn2":
                                    this.listConfigPara[1].Insert(e.RowIndex, new CommunicationConfigParam());
                                    break;
                            }
                        }
                    }));

                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            switch (dataGridView3.Columns[e.ColumnIndex].Name)
                            {
                                case "DeleteBtn3":
                                    if (e.RowIndex < 0) return;
                                    this.listConfigPara[2].RemoveAt(e.RowIndex);
                                    break;
                                case "ReadBtn3":
                                    this.listConfigPara[2][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[2][e.RowIndex])?.ToString();
                                    break;
                                case "WriteBtn3":
                                    CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[2][e.RowIndex]);
                                    break;
                                case "InsertBtn3":
                                    this.listConfigPara[2].Insert(e.RowIndex, new CommunicationConfigParam());
                                    break;
                            }
                        }
                    }));

                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            switch (dataGridView4.Columns[e.ColumnIndex].Name)
                            {
                                case "DeleteBtn4":
                                    if (e.RowIndex < 0) return;
                                    this.listConfigPara[3].RemoveAt(e.RowIndex);
                                    break;
                                case "ReadBtn4":
                                    this.listConfigPara[3][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[3][e.RowIndex])?.ToString();
                                    break;
                                case "WriteBtn4":
                                    CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[3][e.RowIndex]);
                                    break;
                                case "InsertBtn4":
                                    this.listConfigPara[3].Insert(e.RowIndex, new CommunicationConfigParam());
                                    break;
                            }
                        }
                    }));

                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            switch (dataGridView5.Columns[e.ColumnIndex].Name)
                            {
                                case "DeleteBtn5":
                                    if (e.RowIndex < 0) return;
                                    this.listConfigPara[4].RemoveAt(e.RowIndex);
                                    break;
                                case "ReadBtn5":
                                    this.listConfigPara[4][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[4][e.RowIndex])?.ToString();
                                    break;
                                case "WriteBtn5":
                                    CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[4][e.RowIndex]);
                                    break;
                                case "InsertBtn5":
                                    this.listConfigPara[4].Insert(e.RowIndex, new CommunicationConfigParam());
                                    break;
                            }
                        }
                    }));

                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            switch (dataGridView6.Columns[e.ColumnIndex].Name)
                            {
                                case "DeleteBtn6":
                                    if (e.RowIndex < 0) return;
                                    this.listConfigPara[5].RemoveAt(e.RowIndex);
                                    break;
                                case "ReadBtn6":
                                    this.listConfigPara[5][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[5][e.RowIndex])?.ToString();
                                    break;
                                case "WriteBtn6":
                                    CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[5][e.RowIndex]);
                                    break;
                                case "InsertBtn6":
                                    this.listConfigPara[5].Insert(e.RowIndex, new CommunicationConfigParam());
                                    break;
                            }
                        }
                    }));

                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            switch (dataGridView7.Columns[e.ColumnIndex].Name)
                            {
                                case "DeleteBtn7":
                                    if (e.RowIndex < 0) return;
                                    this.listConfigPara[6].RemoveAt(e.RowIndex);
                                    break;
                                case "ReadBtn7":
                                    this.listConfigPara[6][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[6][e.RowIndex])?.ToString();
                                    break;
                                case "WriteBtn7":
                                    CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[6][e.RowIndex]);
                                    break;
                                case "InsertBtn7":
                                    this.listConfigPara[6].Insert(e.RowIndex, new CommunicationConfigParam());
                                    break;
                            }
                        }
                    }));

                }
                catch (Exception ex)
                {
                    new UserMessageForm(ex.ToString()).ShowDialog();
                }
            });
        }

        //private void dataGridView8_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView8.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn8":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[7].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn8":
        //                            this.listConfigPara[7][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[7][e.RowIndex])?.ToString();
        //                            break;
        //                        case "WriteBtn8":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[7][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn8":
        //                            this.listConfigPara[7].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

        //private void dataGridView9_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView9.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn9":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[8].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn9":
        //                            this.listConfigPara[8][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[8][e.RowIndex])?.ToString();
        //                            break;
        //                        case "WriteBtn9":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[8][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn9":
        //                            this.listConfigPara[8].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

        //private void dataGridView10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView10.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn10":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[9].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn10":
        //                            this.listConfigPara[9][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[9][e.RowIndex])?.ToString();
        //                            break;
        //                        case "WriteBtn10":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[9][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn10":
        //                            this.listConfigPara[9].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

        //private void dataGridView11_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView11.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn11":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[10].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn11":
        //                            this.listConfigPara[10][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[10][e.RowIndex]).ToString();
        //                            break;
        //                        case "WriteBtn11":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[10][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn11":
        //                            this.listConfigPara[10].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

        //private void dataGridView12_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView12.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn12":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[11].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn12":
        //                            this.listConfigPara[11][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[11][e.RowIndex]).ToString();
        //                            break;
        //                        case "WriteBtn12":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[11][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn12":
        //                            this.listConfigPara[11].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

        //private void dataGridView13_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView13.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn13":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[12].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn13":
        //                            this.listConfigPara[12][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[12][e.RowIndex]).ToString();
        //                            break;
        //                        case "WriteBtn13":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[12][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn13":
        //                            this.listConfigPara[12].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

        //private void dataGridView14_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView14.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn14":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[13].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn14":
        //                            this.listConfigPara[13][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[13][e.RowIndex]).ToString();
        //                            break;
        //                        case "WriteBtn14":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[13][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn14":
        //                            this.listConfigPara[13].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

        //private void dataGridView15_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                if (e.RowIndex >= 0)
        //                {
        //                    switch (dataGridView15.Columns[e.ColumnIndex].Name)
        //                    {
        //                        case "DeleteBtn15":
        //                            if (e.RowIndex < 0) return;
        //                            this.listConfigPara[14].RemoveAt(e.RowIndex);
        //                            break;
        //                        case "ReadBtn15":
        //                            this.listConfigPara[14][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[14][e.RowIndex]).ToString();
        //                            break;
        //                        case "WriteBtn15":
        //                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[14][e.RowIndex]);
        //                            break;
        //                        case "InsertBtn15":
        //                            this.listConfigPara[14].Insert(e.RowIndex, new CommunicationConfigParam());
        //                            break;
        //                    }
        //                }
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            new UserMessageForm(ex.ToString()).ShowDialog();
        //        }
        //    });
        //}

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
                // Application.DoEvents();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //return;
                this.通信命令配置label.Text = "通信命令配置" + (tabControl1.SelectedIndex + 1).ToString();
                this.ClearDataBinding();
                switch (tabControl1.SelectedTab.Name)
                {
                    case nameof(通信配置1tabPage):
                        this.InitDataGridView1(listConfigPara[0]);
                        break;
                    case nameof(通信配置2tabPage):
                        this.InitDataGridView2(listConfigPara[1]);
                        break;
                    case nameof(通信配置3tabPage):
                        this.InitDataGridView3(listConfigPara[2]);
                        break;
                    case nameof(通信配置4tabPage):
                        this.InitDataGridView4(listConfigPara[3]);
                        break;
                    case nameof(通信配置5tabPage):
                        this.InitDataGridView5(listConfigPara[4]);
                        break;
                    case nameof(通信配置6tabPage):
                        this.InitDataGridView6(listConfigPara[5]);
                        break;
                    case nameof(通信配置7tabPage):
                        this.InitDataGridView7(listConfigPara[6]);
                        break;
                    //case nameof(通信配置8tabPage):
                    //    this.InitDataGridView8(listConfigPara[7]);
                    //    break;
                    //case nameof(通信配置9tabPage):
                    //    this.InitDataGridView9(listConfigPara[8]);
                    //    break;
                    //case nameof(通信配置10tabPage):
                    //    this.InitDataGridView10(listConfigPara[9]);
                    //    break;
                    //case nameof(通信配置11tabPage):
                    //    this.InitDataGridView11(listConfigPara[10]);
                    //    break;
                    //case nameof(通信配置12tabPage):
                    //    this.InitDataGridView12(listConfigPara[11]);
                    //    break;
                    //case nameof(通信配置13tabPage):
                    //    this.InitDataGridView13(listConfigPara[12]);
                    //    break;
                    //case nameof(通信配置14tabPage):
                    //    this.InitDataGridView14(listConfigPara[13]);
                    //    break;
                    //case nameof(通信配置15tabPage):
                    //    this.InitDataGridView15(listConfigPara[14]);
                    //    break;
                }
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





    }

}
