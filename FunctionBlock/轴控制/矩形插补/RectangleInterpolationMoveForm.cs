﻿
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
    public partial class RectangleInterpolationMoveForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        public RectangleInterpolationMoveForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            //new DataGridViewWrapClass().InitDataGridView(this, this._function, this.dataGridView1, ((LaserScanAcq)this._function).Coord1Table);
        }
        private void RectangleInterpolationMoveForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            AddForm(this.运动panel, new JogMotionForm());
            AddForm(this.位置panel, new DisplayPositionForm());
        }
        private void BindProperty()
        {
            try
            {
                ////////////////
                this.dataGridView1.DataSource = ((RectangleInterpolationMove)this._function).Coord1Table;
                this.控制卡comboBox.DataSource = MotionControlCard.MotionCardManage.CardList;
                this.控制卡comboBox.DisplayMember = "Name";
                this.运动轴comboBox.DataSource = Enum.GetNames(typeof(enAxisName));
                this.坐标类型comboBox.DataSource = Enum.GetNames(typeof(enCoordType));
                //////////////////
                this.控制卡comboBox.DataBindings.Add("SelectedItem", ((RectangleInterpolationMove)this._function), "Card", true, DataSourceUpdateMode.OnPropertyChanged); //WaiteTime;
                this.运动轴comboBox.DataBindings.Add("Text", (RectangleInterpolationMove)this._function, "MoveAxis", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////
                this.起始速度numericUpDown.DataBindings.Add("Value", ((RectangleInterpolationMove)this._function), "StartVel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.停止速度numericUpDown.DataBindings.Add("Value", ((RectangleInterpolationMove)this._function), "StopVel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.运行速度numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)_function, "MaxVel", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.加速度时间numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)_function, "Tacc", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.减速度时间numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)_function, "Tdec", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.S平滑时间numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)_function, "S_para", true, DataSourceUpdateMode.OnPropertyChanged); //   
                ///////////////////////
                this.IO输出口numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)_function, "IoOutPort", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.异步执行checkBox.DataBindings.Add("Checked", (RectangleInterpolationMove)_function, "Asynchronous", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.Y轴向量numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)this._function, "Vector_y", true, DataSourceUpdateMode.OnPropertyChanged);
                this.X轴向量numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)_function, "Vector_x", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.行数量numericUpDown.DataBindings.Add("Value", (RectangleInterpolationMove)_function, "LineCount", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.插补模式comboBox.DataBindings.Add("Text", (RectangleInterpolationMove)_function, "InterpolateMode", true, DataSourceUpdateMode.OnPropertyChanged); // 
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





        private void RectangleInterpolationMoveForm_FormClosing(object sender, FormClosingEventArgs e)
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


        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 添加点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, U = 0, V = 0, W = 0;
                switch (((RectangleInterpolationMove)this._function).MoveAxis)
                {
                    case enAxisName.XY轴矩形插补:
                        ((RectangleInterpolationMove)this._function).Card.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.X轴,  out X);
                        ((RectangleInterpolationMove)this._function).Card.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Y轴, out Y);
                        ((RectangleInterpolationMove)_function).Coord1Table.Rows.Add(X, Y);
                        break;               
                    default:
                        throw new Exception("点位运动不支持该模式");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除点button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                ((RectangleInterpolationMove)_function).Coord1Table.Rows.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 清空点button_Click(object sender, EventArgs e)
        {
            try
            {
                ((RectangleInterpolationMove)_function).Coord1Table.Rows.Clear();
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
                enAxisName enAxisName;
                Enum.TryParse(this.运动轴comboBox.SelectedItem.ToString(), out enAxisName);
                switch (enAxisName)
                {
                    case enAxisName.XY轴矩形插补:
                        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                        ((RectangleInterpolationMove)this._function).Coord1Table.Columns.Clear();
                        ((RectangleInterpolationMove)this._function).Coord1Table.Rows.Clear();
                        ((RectangleInterpolationMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[] { new DataColumn("X坐标"), new DataColumn("Y坐标") });
                        break;
                    default:
                        throw new Exception("点位运动不支持该模式");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
