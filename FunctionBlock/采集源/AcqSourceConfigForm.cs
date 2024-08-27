using Common;
using FunctionBlock;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class AcqSourceConfigForm : Form
    {
        private AcqSource acqSource;
        public AcqSourceConfigForm()
        {
            InitializeComponent();
        }

        private void AcqSourceConfigForm_Load(object sender, EventArgs e)
        {
            this.LightControlColumn.Items.Clear();
            this.LightControlColumn.ValueType = typeof(string);
            foreach (string temp in LightConnectManage.GetLightName())
                this.LightControlColumn.Items.Add(temp.ToString());
            this.LightChennelColumn.Items.Clear();
            this.LightChennelColumn.ValueType = typeof(enLightChannel);
            foreach (enLightChannel temp in Enum.GetValues(typeof(enLightChannel)))
                this.LightChennelColumn.Items.Add(temp);
            this.光源dataGridView1.DataSource = this.acqSource?.LightParamList;
            ///////////////////////////////////////
            this.采集源listBox.DataSource = AcqSourceManage.Instance.AcqSourceList;// AcqSourceManage.Instance.GetAcqSourceName();
            this.采集源listBox.DisplayMember = "Name";
            this.传感器列表comboBox.Items.Clear();
            foreach (var item in SensorManage.GetSensorName())
            {
                this.传感器列表comboBox.Items.Add(item);
            }
            //this.传感器列表comboBox.DataSource = SensorManage.GetSensorName();
            this.传感器列表comboBox.SelectedIndex = 0;
            //this.传感器列表comboBox.DisplayMember = "Name";
            this.运动坐标系comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
            this.运动坐标系comboBox.Text = enCoordSysName.CoordSys_0.ToString();
            this.运动轴comboBox.DataSource = Enum.GetValues(typeof(enAxisName));//
            this.运动轴comboBox.Text = enAxisName.XY轴.ToString();
        }


        private void AddSourceButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 添加时先清空
                string sensoerName = "";
                this.acqSource = new AcqSource();
                //foreach (var item in this.传感器listBox.Items)
                //{
                object item = this.传感器列表comboBox.SelectedItem;
                foreach (var item2 in item.ToString().Split(',',';','.'))
                {
                    this.acqSource.SensorName.Add(item2); // SensorName 中存储传感器
                }
                if (sensoerName.Length > 0)
                    sensoerName += "," + item.ToString();
                else
                    sensoerName += item.ToString();
                //}
                if (this.采集源名称textBox.Text.ToUpper().Contains("(" + sensoerName.ToUpper() + ")"))  // 都转化为大写再比较
                    this.acqSource.Name = this.采集源名称textBox.Text;
                else
                    this.acqSource.Name = this.采集源名称textBox.Text + "(" + sensoerName + ")";
                this.acqSource.CoordSysName = (enCoordSysName)this.运动坐标系comboBox.SelectedIndex;
                /////////////////////////////////////////////////////////////
                if (AcqSourceManage.Instance.ContainsName(this.acqSource.Name))
                {
                    MessageBox.Show("集合中包含了相同名称的采集源，请先修改名称再添加!!!");
                }
                else
                {
                    AcqSourceManage.Instance.AcqSourceList.Add(this.acqSource);
                    AcqSourceManage.Instance.CurrentAcqSource = this.acqSource;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 新建Button_Click(object sender, EventArgs e)
        {
            this.光源dataGridView1?.Rows?.Clear();
            //this.传感器listBox?.Items?.Clear(); ;
        }

        private void 采集源listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.采集源listBox.SelectedIndex == -1) return;
                this.acqSource = this.采集源listBox.SelectedItem as AcqSource;//AcqSourceManage.Instance.GetAcqSource(this.采集源listBox.SelectedItem.ToString());
                this.采集源名称textBox.Text = this.acqSource?.Name;
                this.运动坐标系comboBox.Text = this.acqSource?.CoordSysName.ToString();
                this.运动轴comboBox.Text = this.acqSource?.MoveAxisName.ToString();
                if (this.acqSource?.SensorName.Count > 0)
                    this.传感器列表comboBox.Text = this.acqSource?.SensorName.Last();
                else
                    this.传感器列表comboBox.Text = "NONE";
                this.曝光参数comboBox.Text = this.acqSource?.Exposure.ToString();
                //foreach (var item in acqSource.SensorName)
                //{
                //    this.传感器listBox.Items.Add(item);
                //}
                ///////////////////////////////////////////////
                ///////////////////////////////  
                this.光源dataGridView1.DataSource = this.acqSource.LightParamList;  // 绑定语句一定要放在后面，不能放在前面
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void AcqSourceConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }

        private void 删除button_Click(object sender, EventArgs e)
        {
            try
            {
                AcqSource acqSource = this.采集源listBox.SelectedItem as AcqSource;//AcqSourceManage.Instance.GetAcqSource(this.采集源listBox.SelectedItem.ToString());
                if (acqSource == null) return;
                if (AcqSourceManage.Instance.ContainsName(acqSource.Name))
                    AcqSourceManage.Instance.AcqSourceList.Remove(acqSource);
                //this.采集源listBox.Items.Remove(this.采集源listBox.SelectedItem);    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                //LightParam lightParam;
                //enLightChannel lightChannel;
                //this.acqSource = AcqSourceManage.Instance.GetAcqSource(((AcqSource)this.采集源listBox.SelectedItem).Name);
                //DataGridViewRowCollection rowCollection = this.光源dataGridView1.Rows;
                //for (int i = 0; i < rowCollection.Count - 1; i++)
                //{
                //    lightParam = new LightParam();
                //    lightParam.LightName = this.光源dataGridView1["LightControlColumn", i].Value.ToString();
                //    Enum.TryParse(this.光源dataGridView1["LightChennelColumn", i].Value.ToString(), out lightChannel);
                //    lightParam.Channel = lightChannel;
                //    lightParam.LightValue = Convert.ToInt32(this.光源dataGridView1["LightValue", i].Value.ToString());
                //    acqSource.LightParamList.Add(lightParam);
                //}
                //////
                AcqSourceManage.Instance.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 设置采集源参数Btn_Click(object sender, EventArgs e)
        {
            try
            {
                AcqSource acqSource = this.采集源listBox.SelectedItem as AcqSource;// AcqSourceManage.Instance.GetAcqSource(this.采集源listBox.SelectedItem.ToString());
                AcqSourceForm form = new AcqSourceForm(acqSource);
                form.Owner = this;
                form.Show();
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

        private void 光源dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (光源dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "SetLightBtn":
                            if (this.acqSource == null) return;
                            if (acqSource.LightParamList.Count > e.RowIndex)
                            {
                                acqSource.LightParamList[e.RowIndex].SetLight();
                            }
                            break;
                        case "DeleteBtn":
                            if (this.acqSource == null) return;
                            if (acqSource.LightParamList.Count > e.RowIndex)
                            {
                                this.acqSource.LightParamList.RemoveAt(e.RowIndex);
                            }
                            break;
                        case "OpenBtn":
                            if (this.acqSource == null) return;
                            if (acqSource.LightParamList.Count > e.RowIndex)
                            {
                                acqSource.LightParamList[e.RowIndex].Open();
                            }
                            break;
                    }
                    this.光源dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 曝光参数comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            try
            {
                double value = 0;
                double.TryParse(曝光参数comboBox.Text, out value);
                if (this.acqSource != null)
                    this.acqSource.Exposure = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 运动坐标系comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.acqSource != null)
                {
                    enCoordSysName coordSysName;
                    Enum.TryParse(this.运动坐标系comboBox.SelectedItem.ToString(), out coordSysName);
                    this.acqSource.CoordSysName = coordSysName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 运动轴comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.acqSource != null)
                {
                    enAxisName axisName;
                    Enum.TryParse(this.运动轴comboBox.SelectedItem.ToString(), out axisName);
                    this.acqSource.MoveAxisName = axisName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
