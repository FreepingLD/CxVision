using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
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
    public partial class OffsetCalculateForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        public OffsetCalculateForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public OffsetCalculateForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            this.Text = node.Text;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void OffsetCalculateForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////
            
            List<drawWcsPoint> refPoint = new List<drawWcsPoint>();
            List<drawWcsPoint> curPoint = new List<drawWcsPoint>();
            if (((OffsetCalculate)this._function).TargetPoint != null)
            {
                foreach (var item in ((OffsetCalculate)this._function).TargetPoint)
                {
                    refPoint.Add(new drawWcsPoint(item.X, item.Y, item.Z));
                }
            }
            if (((OffsetCalculate)this._function).SourcePoint != null)
            {
                foreach (var item in ((OffsetCalculate)this._function).SourcePoint)
                {
                    curPoint.Add(new drawWcsPoint(item.X, item.Y, item.Z));
                }
            }
            this.目标点坐标dataGridView.DataSource = refPoint.ToArray();
            this.源点坐标dataGridView.DataSource = curPoint.ToArray();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                CompensationParam param = ((AlignCalculate)this._function).Param;
                this.参考点comboBox.DataSource = Enum.GetValues(typeof(enRefObject));

                this.参考点comboBox.DataBindings.Add("Text", param, nameof(param.RefObject), true, DataSourceUpdateMode.OnPropertyChanged);
                this.计算方式comboBox.DataBindings.Add("Text", param, nameof(param.AlignmentMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.补偿XtextBox.DataBindings.Add("Text", param, nameof(param.Add_X), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.补偿YtextBox.DataBindings.Add("Text", param, nameof(param.Add_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.补偿ThetatextBox.DataBindings.Add("Text", param, nameof(param.Add_Angle), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {

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
        private void AlignCalculateForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void PLC信息dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }



    }




}
