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
    public partial class VectorAlignFormOld : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        public VectorAlignFormOld(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
            new ListBoxWrapClass().InitListBox(this.listBox3, function, 3);
            new ListBoxWrapClass().InitListBox(this.listBox4, function, 4);
        }
        public VectorAlignFormOld(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            this.Text = node.Text;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
            new ListBoxWrapClass().InitListBox(this.listBox3, node, 3);
            new ListBoxWrapClass().InitListBox(this.listBox4, node, 4);
        }
        private void VectorAlignForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////
            List<drawWcsPoint> PlateTeachVector = new List<drawWcsPoint>();
            List<drawWcsPoint> PlateCurVector = new List<drawWcsPoint>();
            List<drawWcsPoint> BandTeachVector = new List<drawWcsPoint>();
            List<drawWcsPoint> BandCurVector = new List<drawWcsPoint>();
            if (((VectorAlign)this._function).PlateTeachVector != null)
            {
                //userPixVector pixVector = ((VectorAlign)this._function).PlateTeachVector;
                userWcsVector wcsVector = ((VectorAlign)this._function).PlateTeachVector.GetWcsVector();
                PlateTeachVector.Add(new drawWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Angle));
            }
            if (((VectorAlign)this._function).PlateCurVector != null)
            {
                //userPixVector pixVector = ((VectorAlign)this._function).PlateCurVector;
                userWcsVector wcsVector = ((VectorAlign)this._function).PlateCurVector.GetWcsVector();
                PlateCurVector.Add(new drawWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Angle));
            }
            if (((VectorAlign)this._function).BandTeachVector != null)
            {
                //userPixVector pixVector = ((VectorAlign)this._function).BandTeachVector;
                userWcsVector wcsVector = ((VectorAlign)this._function).BandTeachVector.GetWcsVector();
                BandTeachVector.Add(new drawWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Angle));
            }
            if (((VectorAlign)this._function).BandCurVector != null)
            {
                //userPixVector pixVector = ((VectorAlign)this._function).BandCurVector;
                userWcsVector wcsVector = ((VectorAlign)this._function).BandCurVector.GetWcsVector();
                BandCurVector.Add(new drawWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Angle));
            }
            this.平台当前向量dataGridView.DataSource = PlateCurVector.ToArray();
            this.平台示教向量dataGridView.DataSource = PlateTeachVector.ToArray();
            this.贴合头示教向量dataGridView.DataSource = BandTeachVector.ToArray();
            this.贴合头当前向量dataGridView.DataSource = BandCurVector.ToArray();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                CompensationParam param = ((VectorAlign)this._function).Param; // enAlignmentMethod
                this.视图窗口comboBox.DataSource = HWindowManage.GetKeysList();
                this.补偿XtextBox.DataBindings.Add("Text", param, nameof(param.Add_X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿YtextBox.DataBindings.Add("Text", param, nameof(param.Add_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿ThetatextBox.DataBindings.Add("Text", param, nameof(param.Add_Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图窗口comboBox.DataBindings.Add("Text", param, nameof(param.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用自动补偿CheckBox.DataBindings.Add(nameof(this.启用自动补偿CheckBox.Checked), param, nameof(param.IsAuto), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反补偿CheckBox.DataBindings.Add(nameof(this.取反补偿CheckBox.Checked), param, nameof(param.IsInvert), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿阈值textBox.DataBindings.Add("Text", param, nameof(param.Threshold), true, DataSourceUpdateMode.OnPropertyChanged);
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
                            if (this._function.Execute(this._refNode).Succss)
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
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void VectorAlignForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void PLC信息dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }


    }




}
