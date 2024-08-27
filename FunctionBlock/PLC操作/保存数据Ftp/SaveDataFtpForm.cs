
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class SaveDataFtpForm : Form
    {
        private IFunction _function;

        public SaveDataFtpForm(TreeNode node)
        {
            this._function = node.Tag as IFunction;
            InitializeComponent();
        }
        private void SaveDataFtpForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BindProperty();
            //this.文件类型comboBox.SelectedItem = ".";
        }

        private void BindProperty()
        {
            try
            {
                this.CooreSysNameColumn.Items.Clear();
                this.CooreSysNameColumn.ValueType = typeof(enCoordSysName);
                //this.CooreSysNameColumn.Items.Add("NONE");
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.CooreSysNameColumn.Items.Add(item);
                ///////////////////////////////
                this.CommunicationCol.Items.Clear();
                this.CommunicationCol.ValueType = typeof(enCommunicationCommand);
                foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                    this.CommunicationCol.Items.Add(item);
                ////////////////////////////////////////////
                this.FlagColumn.Items.Clear();
                this.FlagColumn.ValueType = typeof(enFlag);
                foreach (enFlag item in Enum.GetValues(typeof(enFlag)))
                    this.FlagColumn.Items.Add(item);
                //////////////////////////////// 
                this.DataColumn.Items.Clear();
                this.DataColumn.ValueType = typeof(string);
                this.DataColumn.Items.Add("NONE");
                foreach (var item in MemoryManager.Instance.GetKeysValue())
                    this.DataColumn.Items.Add(item);
                //////////////////////////////////////////////
                this.数据写入dataGridView.DataSource = ((SaveDataFtp)this._function).SaveDataList;
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                //this.保存方式comboBox.DataSource = Enum.GetValues(typeof(enSaveMethod));  
                //this.IP_textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "FtpURI", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件目录textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "SaveFolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.IP_textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "FtpServerIp", true, DataSourceUpdateMode.OnPropertyChanged);
                this.商店_textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "ShopCode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.站点textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "Step_ID", true, DataSourceUpdateMode.OnPropertyChanged);
                this.EQPID_textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "EQP_ID", true, DataSourceUpdateMode.OnPropertyChanged);
                this.UnitID_textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "UnitID", true, DataSourceUpdateMode.OnPropertyChanged);
                this.SubUnitID_textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "SubUnitID", true, DataSourceUpdateMode.OnPropertyChanged);
                this.DEFECTCODE_textBox.DataBindings.Add("Text", ((SaveDataFtp)this._function).SaveParam, "DEFECTCODE", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                                }));
                            }
                        }
                        );
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void directoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                this.文件目录textBox.Text = fold.SelectedPath;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void SaveDataFtpForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void 数据写入dataGridView_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 数据写入dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    BindingList<WriteCommunicateCommand > WriteDataList = ((BindingList<WriteCommunicateCommand >)((SaveDataFtp)this._function).SaveDataList);
                    switch (数据写入dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn": //InsertBtn
                            WriteDataList.RemoveAt(e.RowIndex);
                            break;
                        case "InsertBtn": //
                            WriteDataList.Insert(e.RowIndex, new WriteCommunicateCommand ());
                            break;
                        case "WriteBtn":
                            WriteDataList[e.RowIndex].WriteValue = CommunicationConfigParamManger.Instance.WriteValue(WriteDataList[e.RowIndex].CoordSysName,
                            WriteDataList[e.RowIndex].CommunicationCommand,
                            WriteDataList[e.RowIndex].WriteValue).ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
