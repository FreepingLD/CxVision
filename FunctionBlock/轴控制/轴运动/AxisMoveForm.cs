using MotionControlCard;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FunctionBlock
{
    public partial class AxisMoveForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        public AxisMoveForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            //new DataGridViewWrapClass().InitDataGridView(this, this._function, this.dataGridView1, ((LaserScanAcq)this._function).Coord1Table);
        }
        private void AxisMoveForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            //AddForm(this.运动panel, new JogMotionForm());
            //AddForm(this.位置panel, new DisplayPositionForm());
        }
        private void BindProperty()
        {
            try
            {
                ////////////////
                //this.dataGridView1.DataSource = ((PointMove)this._function).Coord1Table;
                this.控制卡comboBox.DataSource = MotionControlCard.MotionCardManage.CardList;
                this.控制卡comboBox.DisplayMember = "Name";
                this.运动轴comboBox.DataSource = Enum.GetNames(typeof(enAxisName));
                this.坐标类型comboBox.DataSource = Enum.GetNames(typeof(enCoordType));
                //////////////////
                this.控制卡comboBox.DataBindings.Add("SelectedItem", ((PointMove)this._function), "Card", true, DataSourceUpdateMode.OnPropertyChanged); //WaiteTime;
                this.运动轴comboBox.DataBindings.Add("Text", (PointMove)this._function, "MoveAxis", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////
                this.起始速度numericUpDown.DataBindings.Add("Value", ((PointMove)this._function), "StartVel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.停止速度numericUpDown.DataBindings.Add("Value", ((PointMove)this._function), "StopVel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.运行速度numericUpDown.DataBindings.Add("Value", (PointMove)_function, "MaxVel", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.加速度时间numericUpDown.DataBindings.Add("Value", (PointMove)_function, "Tacc", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.减速度时间numericUpDown.DataBindings.Add("Value", (PointMove)_function, "Tdec", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.S平滑时间numericUpDown.DataBindings.Add("Value", (PointMove)_function, "S_para", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.IO输出口numericUpDown.DataBindings.Add("Value", (PointMove)_function, "IoOutPort", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.异步执行checkBox.DataBindings.Add("Checked", (PointMove)_function, "Asynchronous", true, DataSourceUpdateMode.OnPropertyChanged); //   
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

        }

        private void 删除点button_Click(object sender, EventArgs e)
        {
            try
            {

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
       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 运动轴comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }


    }
}
