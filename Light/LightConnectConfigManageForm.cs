using Common;
using Light;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Light
{
    public partial class LightConnectConfigManageForm : Form
    {
        private BindingList<LightConnectConfigParam> lightConfigPara;
        public LightConnectConfigManageForm()
        {
            InitializeComponent();
        }

        private void LightConnectConfigManageForm_Load(object sender, EventArgs e)
        {            // 读取传感器配置文件 
            //listConfigPara = XML<BindingList<SensorConfigParam>>.Read(Application.StartupPath + "\\" + "sensorConfig.txt");
            this.lightConfigPara = LightConnectConfigParamManger.Instance.LightConfigParamList; // 使用一个全局的参数对象
            if (lightConfigPara == null)
                lightConfigPara = new BindingList<LightConnectConfigParam>();
            //////////////////////////////
            this.LightType.Items.Clear();
            this.LightType.ValueType = typeof(enLightType);
            foreach (enLightType item in Enum.GetValues(typeof(enLightType)))
                this.LightType.Items.Add(item);
            //////////////////////////////
            this.LightModel.Items.Clear();
            this.LightModel.ValueType = typeof(enLightModel);
            foreach (enLightModel item in Enum.GetValues(typeof(enLightModel)))
                this.LightModel.Items.Add(item);
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
            this.dataGridView1.DataSource = lightConfigPara;

        }

        private void LightConfigManageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //this.save();
                //XML<BindingList<SensorConfigParam>>.Save(this.listConfigPara, Application.StartupPath + "\\" + "sensorConfig.txt");
                //if (LightConnectConfigParamManger.Instance.Save())
                //    LoggerHelper.Error("传感器配置文件保存成功");
                //else
                //    LoggerHelper.Error("传感器配置文件保存失败");
            }
            catch
            {
                //LoggerHelper.Error("传感器配置文件保存报错");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                //XML<BindingList<SensorConfigParam>>.Save(this.listConfigPara, Application.StartupPath + "\\" + "sensorConfig.txt");
                if (LightConnectConfigParamManger.Instance.Save())
                    MessageBox.Show("配置文件保存成功");
            }
            catch
            {
                MessageBox.Show("配置文件保存失败");
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
                            this.lightConfigPara.RemoveAt(e.RowIndex);
                            break;
                            //case "SaveBtn":
                            //    DeviceConnectConfigParamManger.Instance.Save();
                            //    break;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
