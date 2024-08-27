
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
using View;


namespace FunctionBlock
{
    public partial class DataReadForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        private TreeNode _refNode;
        public DataReadForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
        }

        public DataReadForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
        }
        private void ReadPlcAxisCoordForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                //////////////////////////////
                this.ReadCoordSysName.Items.Clear();
                this.ReadCoordSysName.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                    this.ReadCoordSysName.Items.Add(item);
                ///////////////////////////////////
                this.ReadDataTypeColumn.Items.Clear();
                this.ReadDataTypeColumn.ValueType = typeof(enDataTypes);
                foreach (enDataTypes item in Enum.GetValues(typeof(enDataTypes)))
                    this.ReadDataTypeColumn.Items.Add(item);
                this.数据读取dataGridView.DataSource = ((DataRead)this._function).ReadDataList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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





        private void PointMoveForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void 数据读取dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (数据读取dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn":
                            ((DataRead)this._function).ReadDataList.RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn":
                            //IMotionControl _card = MotionCardManage.GetCard(((DataRead)this._function).ReadDataList[e.RowIndex].CoordSysName);
                            //((DataRead)this._function).ReadDataList[e.RowIndex].ReadValue = _card.ReadValue(((DataRead)this._function).ReadDataList[e.RowIndex].DataType,
                            //                                                                              ((DataRead)this._function).ReadDataList[e.RowIndex].Adress, 
                            //                                                                              (ushort)((DataRead)this._function).ReadDataList[e.RowIndex].Length).ToString();
                            break;
                        case "InsertBtn":
                            ((DataRead)this._function).ReadDataList.Insert(e.RowIndex, new ReadCommunicateCommand()); // 
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
