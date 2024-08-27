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


namespace FunctionBlock
{
    public partial class WriteDataForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        private TreeNode _refNode;
        public WriteDataForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
        }

        public WriteDataForm(TreeNode node)
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
                //////////////////////////////////////////////////
                this.数据写入dataGridView.DataSource = ((WriteData)this._function).WriteDataList;
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
                    BindingList<WriteDataCommand> WriteDataList = ((BindingList<WriteDataCommand>)((WriteData)this._function).WriteDataList);
                    switch (数据写入dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn": //InsertBtn
                            WriteDataList.RemoveAt(e.RowIndex);
                            break;
                        case "InsertBtn": //
                            WriteDataList.Insert(e.RowIndex,new WriteDataCommand());
                            break;
                        case "WriteBtn":
                            WriteDataList[e.RowIndex].WriteValue = CommunicationConfigParamManger.Instance.WriteValue(WriteDataList[e.RowIndex].CoordSysName,
                            WriteDataList[e.RowIndex].CommunicationCommand,
                            WriteDataList[e.RowIndex].WriteValue).ToString();
                            break;
                        case "UpMoveCol":
                            if (e.RowIndex > 0)
                            {
                                WriteDataCommand up = WriteDataList[e.RowIndex - 1];
                                WriteDataCommand cur = WriteDataList[e.RowIndex];
                                WriteDataList[e.RowIndex - 1] = cur;
                                WriteDataList[e.RowIndex] = up;
                            }
                            break;
                        case "DownMoveCol":
                            if (e.RowIndex < WriteDataList.Count - 1)
                            {
                                WriteDataCommand down = WriteDataList[e.RowIndex + 1];
                                WriteDataCommand cur = WriteDataList[e.RowIndex];
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
