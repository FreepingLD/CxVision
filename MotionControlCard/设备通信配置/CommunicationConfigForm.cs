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

namespace MotionControlCard
{
    public partial class DeviceCommunicationConfigForm : Form
    {
        private BindingList< BindingList<CommunicationConfigParam>> listConfigPara;

        private CancellationTokenSource cts;
        public DeviceCommunicationConfigForm()
        {
            InitializeComponent();
        }

        private void DeviceConfigParamManageForm_Load(object sender, EventArgs e)
        {
            // 读取传感器配置文件 
            this.listConfigPara = CommunicationConfigParamManger.Instance.CommunicationParamList; // 使用一个全局的参数对象, 配置文件统一在某个地方读取
            if (listConfigPara == null)
                listConfigPara = new BindingList<BindingList<CommunicationConfigParam>>();
            ///////////////////////////////////////////////////// 添加项目一定要放到 数据源绑定源前面
            for (int i = 0; i <= listConfigPara.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        this.InitDataGridView1(listConfigPara[i]);
                        break;
                    case 1:
                        this.InitDataGridView2(listConfigPara[i]);
                        break;
                    case 2:
                        this.InitDataGridView3(listConfigPara[i]);
                        break;
                    case 3:
                        this.InitDataGridView4(listConfigPara[i]);
                        break;
                    case 4:
                        this.InitDataGridView5(listConfigPara[i]);
                        break;
                    case 5:
                        this.InitDataGridView6(listConfigPara[i]);
                        break;
                    case 6:
                        this.InitDataGridView7(listConfigPara[i]);
                        break;
                    case 7:
                        this.InitDataGridView8(listConfigPara[i]);
                        break;
                    case 8:
                        this.InitDataGridView9(listConfigPara[i]);
                        break;
                    case 9:
                        this.InitDataGridView10(listConfigPara[i]);
                        break;
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
        }
        private void InitDataGridView8(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn8.Items.Clear();
            this.DataTypeColumn8.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn8.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite8.Items.Clear();
            this.AxisReadWrite8.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite8.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn8.Items.Clear();
            this.CooreSysNameColumn8.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn8.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn8.Items.Clear();
            this.MapCooreSysNameColumn8.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn8.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn8.Items.Clear();
            this.CommuniteColumn8.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn8.Items.Add(item);
            //////////////////////
            this.dataGridView8.DataSource = listConfigPara;
        }
        private void InitDataGridView9(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn9.Items.Clear();
            this.DataTypeColumn9.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn9.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite9.Items.Clear();
            this.AxisReadWrite9.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite9.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn9.Items.Clear();
            this.CooreSysNameColumn9.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn9.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn9.Items.Clear();
            this.MapCooreSysNameColumn9.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn9.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn9.Items.Clear();
            this.CommuniteColumn9.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn9.Items.Add(item);
            //////////////////////
            this.dataGridView9.DataSource = listConfigPara;
        }
        private void InitDataGridView10(BindingList<CommunicationConfigParam> listConfigPara)
        {
            /// 添加数据类型
            this.DataTypeColumn10.Items.Clear();
            this.DataTypeColumn10.ValueType = typeof(enDataTypes);
            foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                this.DataTypeColumn10.Items.Add(item);
            /// 添加轴读写状态
            this.AxisReadWrite10.Items.Clear();
            this.AxisReadWrite10.ValueType = typeof(enAxisReadWriteState);
            foreach (enAxisReadWriteState item in Enum.GetValues(typeof(enAxisReadWriteState)))
                this.AxisReadWrite10.Items.Add(item);
            /// 添加运动设备名称 CommuniteColumn
            this.CooreSysNameColumn10.Items.Clear();
            this.CooreSysNameColumn10.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CooreSysNameColumn10.Items.Add(item);
            ///////////映射坐标系 ///////////////////////
            this.MapCooreSysNameColumn10.Items.Clear();
            this.MapCooreSysNameColumn10.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.MapCooreSysNameColumn10.Items.Add(item);
            /// 添加运动设备名称 
            this.CommuniteColumn10.Items.Clear();
            this.CommuniteColumn10.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommuniteColumn10.Items.Add(item);
            //////////////////////
            this.dataGridView10.DataSource = listConfigPara;
        }
        private void DeviceCommunicationConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cts?.Cancel();
            }
            catch
            {
                LoggerHelper.Error("坐标系配置文件保存报错");
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
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn":
                            this.listConfigPara[0].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn":
                            this.listConfigPara[0][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[0][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[0][e.RowIndex]);
                            break;
                        case "InsertBtn":
                            this.listConfigPara[0].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView2.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn2":
                            this.listConfigPara[1].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn2":
                            this.listConfigPara[1][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[1][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn2":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[1][e.RowIndex]);
                            break;
                        case "InsertBtn2":
                            this.listConfigPara[1].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView3.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn3":
                            this.listConfigPara[2].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn3":
                            this.listConfigPara[2][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[2][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn3":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[2][e.RowIndex]);
                            break;
                        case "InsertBtn3":
                            this.listConfigPara[2].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView4.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn4":
                            this.listConfigPara[3].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn4":
                            this.listConfigPara[3][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[3][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn4":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[3][e.RowIndex]);
                            break;
                        case "InsertBtn4":
                            this.listConfigPara[3].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView5.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn5":
                            this.listConfigPara[4].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn5":
                            this.listConfigPara[4][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[4][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn5":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[4][e.RowIndex]);
                            break;
                        case "InsertBtn5":
                            this.listConfigPara[4].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView6.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn6":
                            this.listConfigPara[5].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn6":
                            this.listConfigPara[5][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[5][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn6":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[5][e.RowIndex]);
                            break;
                        case "InsertBtn6":
                            this.listConfigPara[5].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView7.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn7":
                            this.listConfigPara[6].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn7":
                            this.listConfigPara[6][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[6][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn7":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[6][e.RowIndex]);
                            break;
                        case "InsertBtn7":
                            this.listConfigPara[6].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView8_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView8.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn8":
                            this.listConfigPara[7].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn8":
                            this.listConfigPara[7][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[7][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn8":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[7][e.RowIndex]);
                            break;
                        case "InsertBtn8":
                            this.listConfigPara[7].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView9_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView9.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn9":
                            this.listConfigPara[8].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn9":
                            this.listConfigPara[8][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[8][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn9":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[8][e.RowIndex]);
                            break;
                        case "InsertBtn9":
                            this.listConfigPara[8].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView10.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn10":
                            this.listConfigPara[9].RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn10":
                            this.listConfigPara[9][e.RowIndex].ReadValue = CommunicationConfigParamManger.Instance.ReadValue(this.listConfigPara[9][e.RowIndex]).ToString();
                            break;
                        case "WriteBtn10":
                            CommunicationConfigParamManger.Instance.WriteValue(this.listConfigPara[9][e.RowIndex]);
                            break;
                        case "InsertBtn10":
                            this.listConfigPara[9].Insert(e.RowIndex, new CommunicationConfigParam());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void SaveButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (CommunicationConfigParamManger.Instance.Save())
                    MessageBox.Show("配置文件保存成功");
            }
            catch
            {
                MessageBox.Show("配置文件保存失败");
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
                Application.DoEvents();
                Thread.Sleep(100);
            }
        }

        private void 实时刷新checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.实时刷新checkBox.Checked)
                    this.Updata();
                else
                    this.cts.Cancel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.label1.Text = "通信命令配置" + (tabControl1.SelectedIndex + 1).ToString();    
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 清空配置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                CommunicationConfigParamManger.Instance.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }

}
