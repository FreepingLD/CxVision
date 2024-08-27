
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
    public partial class ReadIoOutputGroupForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        public ReadIoOutputGroupForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            //new DataGridViewWrapClass().InitDataGridView(this, this._function, this.dataGridView1, ((LaserScanAcq)this._function).Coord1Table);
        }
        private void ReadIoOutputGroupForm_Load(object sender, EventArgs e)
        {
            BindProperty();

        }
        private void BindProperty()
        {
            try
            {
                ////////////////
                this.dataGridView1.DataSource = ((ReadIoOutputGroup)this._function).LevelDataTable;
                this.控制卡comboBox.DataSource = MotionControlCard.MotionCardManage.CardList;
                this.控制卡comboBox.DisplayMember = "Name";
                this.IO端口类型comboBox.DataSource = Enum.GetNames(typeof(enIoPortType));
                //////////////////
                this.控制卡comboBox.DataBindings.Add("SelectedItem", ((ReadIoOutputGroup)this._function), "Card", true, DataSourceUpdateMode.OnPropertyChanged); //WaiteTime;
                this.IO端口类型comboBox.DataBindings.Add("Text", (ReadIoOutputGroup)this._function, "IoPortType", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////
                this.IO端口numericUpDown.DataBindings.Add("Value", ((ReadIoOutputGroup)this._function), "IoOutputGroup", true, DataSourceUpdateMode.OnPropertyChanged);
                this.IO端口电平numericUpDown.DataBindings.Add("Value", ((ReadIoOutputGroup)this._function), "IoOutputlevel", true, DataSourceUpdateMode.OnPropertyChanged);  
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




        private void 运动轴comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                enAxisName enAxisName;
                Enum.TryParse(this.IO端口类型comboBox.SelectedItem.ToString(), out enAxisName);
                //switch (enAxisName)
                //{
                //    case enAxisName.X轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.Add(new DataColumn("X坐标"));
                //        break;
                //    case enAxisName.Y轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.Add(new DataColumn("Y坐标"));
                //        break;
                //    case enAxisName.Z轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.Add(new DataColumn("Z坐标"));
                //        break;
                //    case enAxisName.U轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.Add(new DataColumn("U坐标"));
                //        break;
                //    case enAxisName.V轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.Add(new DataColumn("V坐标"));
                //        break;
                //    case enAxisName.W轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.Add(new DataColumn("W坐标")); ;
                //        break;
                //    case enAxisName.XY轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[2] { new DataColumn("X坐标"), new DataColumn("Y坐标") });
                //        break;
                //    case enAxisName.XYZ轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[3] { new DataColumn("X坐标"), new DataColumn("Y坐标"), new DataColumn("Z坐标") });
                //        break;
                //    case enAxisName.XYZU轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[4] { new DataColumn("X坐标"), new DataColumn("Y坐标"), new DataColumn("Z坐标")
                //        , new DataColumn("U坐标") });
                //        break;
                //    case enAxisName.XYZUV轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[5] { new DataColumn("X坐标"), new DataColumn("Y坐标"), new DataColumn("Z坐标")
                //        , new DataColumn("U坐标"), new DataColumn("V坐标") });
                //        break;
                //    case enAxisName.XYZUVW轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[6] { new DataColumn("X坐标"), new DataColumn("Y坐标"), new DataColumn("Z坐标")
                //        , new DataColumn("U坐标"), new DataColumn("V坐标"), new DataColumn("W坐标") });
                //        break;
                //    case enAxisName.YZ轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[2] { new DataColumn("Y坐标"), new DataColumn("Z坐标") });
                //        break;
                //    case enAxisName.UVW轴:
                //        if (MessageBox.Show("确定要清空所有坐标位置并重新添加吗？", "重置坐标点", MessageBoxButtons.YesNo) == DialogResult.No) return;
                //        ((PointMove)this._function).Coord1Table.Columns.Clear();
                //        ((PointMove)this._function).Coord1Table.Rows.Clear();
                //        ((PointMove)this._function).Coord1Table.Columns.AddRange(new DataColumn[3] { new DataColumn("U坐标"), new DataColumn("V坐标"), new DataColumn("W坐标") });
                //        break;
                //    default:
                //        throw new Exception("点位运动不支持该模式");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
