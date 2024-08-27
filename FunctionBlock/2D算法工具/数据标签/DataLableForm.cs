using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;


namespace FunctionBlock
{
    public partial class DataLableForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        private TreeNode _refNode;
        public DataLableForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
        }

        public DataLableForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            //new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void WritePlcDataForm_Load(object sender, EventArgs e)
        {
            BindProperty();
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
                //this.JawCol.Items.Clear();
                //this.JawCol.ValueType = typeof(enRobotJawEnum);
                //foreach (enRobotJawEnum item in Enum.GetValues(typeof(enRobotJawEnum)))
                //    this.JawCol.Items.Add(item);
                //////////////////////////////////////////////
                this.FlagCol.Items.Clear();
                this.FlagCol.ValueType = typeof(enFlag);
                foreach (enFlag item in Enum.GetValues(typeof(enFlag)))
                    this.FlagCol.Items.Add(item);
                //////////////////////////////// 
                this.DataColumn.Items.Clear();
                this.DataColumn.ValueType = typeof(string);
                this.DataColumn.Items.Add("NONE");
                foreach (var item in MemoryManager.Instance.GetKeysValue())
                    this.DataColumn.Items.Add(item);
                //////////////////////////////////////////////////
                this.数据写入dataGridView.DataSource = ((DataLable)this._function).LableDataList1;
                //////////////////////////////////////////
                this.相机comboBox.DataSource = SensorManage.GetCamSensorName();
                this.视图名称comboBox.DataSource = HWindowManage.GetKeysList(); //
                this.标签位置comboBox.DataSource = Enum.GetValues(typeof(enLablePosition)); //
                DataLableParam param = ((DataLable)this._function).LableParam;
                this.相机comboBox.DataBindings.Add(nameof(this.相机comboBox.Text), param, nameof(param.CamName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图名称comboBox.DataBindings.Add(nameof(this.视图名称comboBox.Text), param, nameof(param.ViewName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标签位置comboBox.DataBindings.Add(nameof(this.标签位置comboBox.Text), param, nameof(param.LablePosition), true, DataSourceUpdateMode.OnPropertyChanged);
                this.起点XcomboBox.DataBindings.Add(nameof(this.起点XcomboBox.Text), param, nameof(param.Start_x), true, DataSourceUpdateMode.OnPropertyChanged);
                this.起点YcomboBox.DataBindings.Add(nameof(this.起点YcomboBox.Text), param, nameof(param.Start_y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y偏移comboBox.DataBindings.Add(nameof(this.Y偏移comboBox.Text), param, nameof(param.Offset_y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.字体大小comboBox.DataBindings.Add(nameof(this.字体大小comboBox.Text), param, nameof(param.Size), true, DataSourceUpdateMode.OnPropertyChanged);
                this.强制结果checkBox.DataBindings.Add(nameof(this.强制结果checkBox.Checked), param, nameof(param.SendResult), true, DataSourceUpdateMode.OnPropertyChanged);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }
        private void AddForm(GroupBox groupBox, Form form)
        {
            if (groupBox == null) return;
            if (groupBox.Controls.Count > 0)
                groupBox.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            groupBox.Controls.Add(form);
            form.Show();
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
                        this.cts = new CancellationTokenSource();
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.toolStripStatusLabel1.Text = "执行结果:";
                                        this.toolStripStatusLabel2.Text = "成功";
                                        this.toolStripStatusLabel2.ForeColor = Color.Green;
                                    }));
                                }
                            }
                            else
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                   {
                                       this.toolStripStatusLabel1.Text = "执行结果:";
                                       this.toolStripStatusLabel2.Text = "失败";
                                       this.toolStripStatusLabel2.ForeColor = Color.Red;
                                   }));
                                }
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

        private void WritePlcDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cts != null)
                    cts.Cancel();
            }
            catch
            {

            }
        }


        private void 数据写入dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    BindingList<WriteLableCommand> WriteDataList = ((BindingList<WriteLableCommand>)((DataLable)this._function).LableDataList1);
                    switch (数据写入dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn":
                            WriteDataList.RemoveAt(e.RowIndex);
                            break;
                        case "InsertBtn":
                            WriteDataList.Insert(e.RowIndex, new WriteLableCommand());
                            break;
                        //case "WriteBtn":
                        //    WriteDataList[e.RowIndex].WriteValue = CommunicationConfigParamManger.Instance.WriteValue(WriteDataList[e.RowIndex].CoordSysName,
                        //    WriteDataList[e.RowIndex].CommunicationCommand,
                        //    WriteDataList[e.RowIndex].WriteValue).ToString();
                        //    break;
                        case "UpMoveCol":
                            if (e.RowIndex > 0)
                            {
                                WriteLableCommand up = WriteDataList[e.RowIndex - 1];
                                WriteLableCommand cur = WriteDataList[e.RowIndex];
                                WriteDataList[e.RowIndex - 1] = cur;
                                WriteDataList[e.RowIndex] = up;
                            }
                            break;
                        case "DownMoveCol":
                            if (e.RowIndex < WriteDataList.Count - 1)
                            {
                                WriteLableCommand down = WriteDataList[e.RowIndex + 1];
                                WriteLableCommand cur = WriteDataList[e.RowIndex];
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
