
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
    public partial class PointMoveForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        public PointMoveForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
        }
        private void PointMoveForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            //AddForm(this.运动panel, new JogMotionForm());
            //AddForm(this.位置panel, new DisplayPositionForm());
        }
        private void BindProperty()
        {
            try
            {

                this.MoveAxis.Items.Clear();
                this.MoveAxis.ValueType = typeof(enAxisName);
                foreach (enAxisName item in Enum.GetValues(typeof(enAxisName)))
                {
                    this.MoveAxis.Items.Add(item);
                }
                this.dataGridView1.DataSource = ((PointMove)this._function).PointParam.TrackParam;
                ////////////////
                this.坐标系comboBox.DataSource = Enum.GetNames(typeof(enCoordSysName));
                this.坐标类型comboBox.DataSource = Enum.GetNames(typeof(enCoordType));
                /////////////////////////////////////////////
                this.坐标系comboBox.DataBindings.Add("Text", ((PointMove)this._function).PointParam, "CoordSysName", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////
                this.起始速度numericUpDown.DataBindings.Add("Value", ((PointMove)this._function).PointParam, "StartVel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.停止速度numericUpDown.DataBindings.Add("Value", ((PointMove)this._function).PointParam, "StopVel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.运行速度numericUpDown.DataBindings.Add("Value", ((PointMove)this._function).PointParam, "MaxVel", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.加速度时间numericUpDown.DataBindings.Add("Value", ((PointMove)this._function).PointParam, "Tacc", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.减速度时间numericUpDown.DataBindings.Add("Value", ((PointMove)this._function).PointParam, "Tdec", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.S平滑时间numericUpDown.DataBindings.Add("Value", ((PointMove)this._function).PointParam, "S_para", true, DataSourceUpdateMode.OnPropertyChanged); //   
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
                CoordSysAxisParam axisParam = new CoordSysAxisParam(((PointMove)this._function).PointParam.CoordSysName);
                PointTrackParam trackParam = new PointTrackParam();
                trackParam.IsActive = true;
                trackParam.IoOutPort = 0;
                trackParam.MoveAxis = enAxisName.XYZTheta轴;
                trackParam.IsWait = false;
                trackParam.X = axisParam.X;
                trackParam.Y = axisParam.Y;
                trackParam.Z = axisParam.Z;
                trackParam.Theta = axisParam.Theta;
                ((PointMove)this._function).PointParam.TrackParam.Add(trackParam);
                for (int i = 0; i < ((PointMove)this._function).PointParam.TrackParam.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
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
                ((PointMove)this._function).PointParam.TrackParam.RemoveAt(index);
                for (int i = 0; i < ((PointMove)this._function).PointParam.TrackParam.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
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
                ((PointMove)this._function).PointParam.TrackParam.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 单步执行Btn_Click(object sender, EventArgs e)
        {
            try
            {
                IMotionControl card = MotionCardManage.GetCard(((PointMove)this._function).PointParam.CoordSysName);
                if(card != null)
                {
                    int i = this.dataGridView1.CurrentRow.Index;
                    MoveCommandParam CommandParam = new MoveCommandParam();
                    CommandParam.MoveAxis = ((PointMove)this._function).PointParam.TrackParam[i].MoveAxis;
                    CommandParam.MoveSpeed = ((PointMove)this._function).PointParam.MaxVel;
                    CommandParam.StartVel = ((PointMove)this._function).PointParam.StartVel;
                    CommandParam.StopVel = ((PointMove)this._function).PointParam.StopVel;
                    CommandParam.Tacc = ((PointMove)this._function).PointParam.Tacc;
                    CommandParam.Tdec = ((PointMove)this._function).PointParam.Tdec;
                    CommandParam.S_para = ((PointMove)this._function).PointParam.S_para;
                    CommandParam.AxisParam = new CoordSysAxisParam(((PointMove)this._function).PointParam.TrackParam[i].X, ((PointMove)this._function).PointParam.TrackParam[i].Y,
                                             ((PointMove)this._function).PointParam.TrackParam[i].Z, ((PointMove)this._function).PointParam.TrackParam[i].Theta, 0, 0);
                    card.MoveMultyAxis(CommandParam);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 单步执行Btn_Click_1(object sender, EventArgs e)
        {

        }
    }
}
