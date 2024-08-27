
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
    public partial class SaveDataNewForm : Form
    {
        private IFunction _function;

        public SaveDataNewForm(TreeNode node)
        {
            this._function = node.Tag as IFunction;
            InitializeComponent();
        }
        private void SaveDataPlcForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BindProperty();
            this.文件类型comboBox.SelectedItem = ".";
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
                //this.FlagColumn.Items.Clear();
                //this.FlagColumn.ValueType = typeof(enFlag);
                //foreach (enFlag item in Enum.GetValues(typeof(enFlag)))
                //    this.FlagColumn.Items.Add(item);
                //////////////////////////////// 
                this.DataColumn.Items.Clear();
                this.DataColumn.ValueType = typeof(string);
                this.DataColumn.Items.Add("NONE");
                foreach (var item in MemoryManager.Instance.GetKeysValue())
                    this.DataColumn.Items.Add(item);
                //////////////////////////////////////////////
                this.数据写入dataGridView.DataSource = ((SaveDataNew)this._function).SaveDataList;
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                this.保存方式comboBox.DataSource = Enum.GetValues(typeof(enSaveMethod));
                this.文件目录textBox.DataBindings.Add("Text", ((SaveDataNew)this._function).SaveParam, "SaveFolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件类型comboBox.DataBindings.Add("Text", ((SaveDataNew)this._function).SaveParam, "ExtendName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.日期checkBox.DataBindings.Add("Checked", ((SaveDataNew)this._function).SaveParam, "AddDataTime", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件名称TextBox.DataBindings.Add("Text", ((SaveDataNew)this._function).SaveParam, "FileName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存方式comboBox.DataBindings.Add("Text", ((SaveDataNew)this._function).SaveParam, "SaveMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Ftp_textBox.DataBindings.Add("Text", ((SaveDataPlc)this._function).SaveParam, "SaveFtpFolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Ftp_checkBox.DataBindings.Add("Checked", ((SaveDataPlc)this._function).SaveParam, "IsSaveFtpData", true, DataSourceUpdateMode.OnPropertyChanged);
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
        private void SaveDataPlcForm_FormClosing(object sender, FormClosingEventArgs e)
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
                    BindingList<SaveDataCommand > WriteDataList = ((BindingList<SaveDataCommand>)((SaveDataNew)this._function).SaveDataList);
                    switch (数据写入dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn": //InsertBtn
                            WriteDataList.RemoveAt(e.RowIndex);
                            break;
                        case "InsertBtn": //
                            WriteDataList.Insert(e.RowIndex, new SaveDataCommand());
                            break;
                        //case "WriteBtn":
                        //    WriteDataList[e.RowIndex].WriteValue = CommunicationConfigParamManger.Instance.WriteValue(WriteDataList[e.RowIndex].CoordSysName,
                        //    WriteDataList[e.RowIndex].CommunicationCommand,
                        //    WriteDataList[e.RowIndex].WriteValue).ToString();
                        //    break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Ftp_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                //this.Ftp_textBox.Text = fold.SelectedPath;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 数据写入dataGridView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    BindingList<SaveDataCommand> WriteDataList = ((BindingList<SaveDataCommand>)((SaveDataNew)this._function).ResultInfo);
                    switch (数据写入dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn": //InsertBtn
                            WriteDataList.RemoveAt(e.RowIndex);
                            break;
                        case "InsertBtn": //
                            WriteDataList.Insert(e.RowIndex, new SaveDataCommand());
                            break;
                        case "UpMoveCol":
                            if (e.RowIndex > 0)
                            {
                                SaveDataCommand up = WriteDataList[e.RowIndex - 1];
                                SaveDataCommand cur = WriteDataList[e.RowIndex];
                                WriteDataList[e.RowIndex - 1] = cur;
                                WriteDataList[e.RowIndex] = up;
                            }
                            break;
                        case "DownMoveCol":
                            if (e.RowIndex < WriteDataList.Count - 1)
                            {
                                SaveDataCommand down = WriteDataList[e.RowIndex + 1];
                                SaveDataCommand cur = WriteDataList[e.RowIndex];
                                WriteDataList[e.RowIndex + 1] = cur;
                                WriteDataList[e.RowIndex] = down;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 数据写入dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}
