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
using View;

namespace FunctionBlock
{
    public partial class CalibRotateCenterPixForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        private TreeNode _refNode;
        public CalibRotateCenterPixForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
        }

        public CalibRotateCenterPixForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node,2);
        }
        private void CalibRotateCenterPixForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                userWcsPoint[] coordPoint1 = ((CalibRotateCenter)this._function).SourcePoint;
                userWcsPoint[] coordPoint2 = ((CalibRotateCenter)this._function).TargetPoint;
                if (coordPoint1 != null)
                {
                    for (int i = 0; i < coordPoint1.Length; i++)
                    {
                        this.P坐标dataGridView.Rows.Add(coordPoint1[i].X, coordPoint1[i].Y);
                    }
                }
                if (coordPoint2 != null)
                {
                    for (int i = 0; i < coordPoint2.Length; i++)
                    {
                        this.Q坐标dataGridView.Rows.Add(coordPoint2[i].X, coordPoint2[i].Y);
                    }
                }
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

        private void CoordMapForm_FormClosing(object sender, FormClosingEventArgs e)
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
                IMotionControl _card;
                if (e.RowIndex >= 0)
                {
                    //switch (坐标映射dataGridView.Columns[e.ColumnIndex].Name)
                    //{
                    //    case "DeleteBtn":
                    //        ((DataWrite)this._function).WriteDataList.RemoveAt(e.RowIndex);
                    //        break;
                    //    case "WriteBtn":
                    //        _card = MotionCardManage.GetCard(((DataWrite)this._function).WriteDataList[e.RowIndex].CoordSysName);
                    //        ((DataWrite)this._function).WriteDataList[e.RowIndex].WriteValue = _card.WriteValue(((DataWrite)this._function).WriteDataList[e.RowIndex].DataType,
                    //            ((DataWrite)this._function).WriteDataList[e.RowIndex].Adress, 
                    //            ((DataWrite)this._function).WriteDataList[e.RowIndex].WriteValue).ToString();

                    //        break;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }
}
