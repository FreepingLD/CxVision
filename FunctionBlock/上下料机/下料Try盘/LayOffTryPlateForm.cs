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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class LayOffTryPlateForm : Form //
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        private TryPlateParam param;
        public LayOffTryPlateForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            //new ListBoxWrapClass().InitListBox(this.listBox2, function,2);
        }
        public LayOffTryPlateForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            this.Text = node.Text;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            //new ListBoxWrapClass().InitListBox(this.listBox2, node,2);
        }
        private void RobotLayOffForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.param = ((LayOffTryPlate)this._function).Param;
                this.相机comboBox.DataSource = SensorManage.GetCamSensorName();
                this.视图名称comboBox.DataSource = HWindowManage.GetKeysList(); // CamName ViewName
                this.相机comboBox.DataBindings.Add(nameof(this.相机comboBox.Text), param, nameof(param.CamName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图名称comboBox.DataBindings.Add(nameof(this.视图名称comboBox.Text), param, nameof(param.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
                this.行数comboBox.DataBindings.Add("Text", param, nameof(param.RowCount), true, DataSourceUpdateMode.OnPropertyChanged);
                this.列数comboBox.DataBindings.Add("Text", param, nameof(param.ColCount), true, DataSourceUpdateMode.OnPropertyChanged);
                this.LoadTryParam(this.tableLayoutPanel3, param.RowCount, param.ColCount, param.CoordsList);
            }
            catch
            {

            }
        }
        private void LoadTryParam(TableLayoutPanel tableLayoutPanel, int rowCount, int colCount, BindingList<UserTryPlateHoleParam> list)
        {
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowCount = rowCount;
            tableLayoutPanel.ColumnCount = colCount;
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();
            LoadTryForm testForm;
            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 10f));
                for (int j = 0; j < tableLayoutPanel.ColumnCount; j++)
                {
                    if (i == 0) // 列风格这里只需要设置一次
                        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
                    if (list.Count > i * tableLayoutPanel.ColumnCount + j)
                        testForm = new LoadTryForm(list[i * tableLayoutPanel.ColumnCount + j]);
                    else
                        continue;
                    testForm.Dock = DockStyle.Fill;
                    //testForm.Width = 200;
                    //testForm.Height = 200;
                    testForm.TopLevel = false;
                    tableLayoutPanel.Controls.Add(testForm, j, i);
                    testForm.Show();
                }
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
        private void RobotLayOffForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void 创建Try盘_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("确定要重置Try穴位参数吗", "重置参数", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    this.param.CoordsList = new BindingList<UserTryPlateHoleParam>();
                    for (int i = 0; i < this.param.RowCount * this.param.ColCount; i++)
                    {
                        this.param.CoordsList.Add(new UserTryPlateHoleParam());
                    }
                    ////////////////////////////////////////////////////////////////////
                    this.LoadTryParam(this.tableLayoutPanel3, param.RowCount, param.ColCount, param.CoordsList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





    }




}
