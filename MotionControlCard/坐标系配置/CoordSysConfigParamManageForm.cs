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
    public partial class CoordSysConfigParamManageForm : Form
    {
        private BindingList<CoordSysConfigParam> listConfigPara;

        public CoordSysConfigParamManageForm()
        {
            InitializeComponent();
        }

        private void DeviceConfigParamManageForm_Load(object sender, EventArgs e)
        {
            // 读取传感器配置文件 
            this.listConfigPara = CoordSysConfigParamManger.Instance.CoordSysConfigParamList; // 使用一个全局的参数对象, 配置文件统一在某个地方读取
            if (listConfigPara == null)
                listConfigPara = new BindingList<CoordSysConfigParam>();
            ///////////////////////////////////////////////////// 添加项目一定要放到 数据源绑定源前面
            this.CoordSysName.Items.Clear();
            this.CoordSysName.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CoordSysName.Items.Add(item);
            /// 添加轴名称
            this.AxisName.Items.Clear();
            this.AxisName.ValueType = typeof(enAxisName);
            foreach (enAxisName item in Enum.GetValues(typeof(enAxisName)))
                this.AxisName.Items.Add(item);
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
            /// 添加运动设备名称
            this.CardNameColumn.Items.Clear();
            this.CardNameColumn.ValueType = typeof(string);
            this.CardNameColumn.Items.Add("NONE");
            foreach (IMotionControl item in MotionCardManage.CardList)
                this.CardNameColumn.Items.Add(item.Name);
            //if(this.CardNameColumn.Items.Count==0)
            //    this.CardNameColumn.Items.Add("NONE");
            //////////////////////
            this.dataGridView1.DataSource = listConfigPara;
        }

        private void SensorManageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //this.save();
                //XML<BindingList<SensorConfigParam>>.Save(this.listConfigPara, Application.StartupPath + "\\" + "sensorConfig.txt");
                if (CoordSysConfigParamManger.Instance.Save())
                    LoggerHelper.Error("坐标系配置文件保存成功");
                else
                    LoggerHelper.Error("坐标系配置文件保存失败");
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
                            this.dataGridView1.Rows.RemoveAt(e.RowIndex);
                            this.listConfigPara.RemoveAt(e.RowIndex);
                            break;
                        case "InsertBtn":
                            this.listConfigPara.Insert(e.RowIndex, new CoordSysConfigParam());
                            break;
                    }
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
                if (CoordSysConfigParamManger.Instance.Save())
                    MessageBox.Show("配置文件保存成功");
            }
            catch
            {
                MessageBox.Show("配置文件保存失败");
            }
        }
    }
}
