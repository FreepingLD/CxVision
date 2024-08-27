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
    public partial class RobotLoadForm : Form //
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        private RobotParam param;
        public RobotLoadForm(IFunction function)
        {
            this._function = function;
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public RobotLoadForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            InitializeComponent();
            this.Text = node.Text;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void RobotLoadForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.param = ((RobotLayOff)this._function).Param;
                this.补偿计算参考位comboBox.DataSource = Enum.GetValues(typeof(enRefGrabPose));
                this.夹抓comboBox.DataSource = Enum.GetValues(typeof(enRobotJawEnum));
                //this.操作comboBox.DataSource = Enum.GetValues(typeof(enOperation));
                //this.最小灰度值comboBox.DataBindings.Add("Text", param, nameof(param.MinGray), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.最大灰度值comboBox.DataBindings.Add("Text", param, nameof(param.MaxGray), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.矩形尺寸comboBox.DataBindings.Add("Text", param, nameof(param.RectSize), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿计算参考位comboBox.DataBindings.Add("Text", param, nameof(param.RefGrabPose), true, DataSourceUpdateMode.OnPropertyChanged);
                this.夹抓comboBox.DataBindings.Add("Text", param, nameof(param.RobotJaw), true, DataSourceUpdateMode.OnPropertyChanged);
                this.X轴正限位comboBox.DataBindings.Add("Text", param, nameof(param.LimitP_X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.X轴负限位comboBox.DataBindings.Add("Text", param, nameof(param.LimitN_X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y轴正限位comboBox.DataBindings.Add("Text", param, nameof(param.LimitP_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y轴负限位comboBox.DataBindings.Add("Text", param, nameof(param.LimitN_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度正限位comboBox.DataBindings.Add("Text", param, nameof(param.LimitP_Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度负限位comboBox.DataBindings.Add("Text", param, nameof(param.LimitN_Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图名称comboBox.DataBindings.Add("Text", param, nameof(param.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged); //
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
        private void RobotLayOffForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.显示条目comboBox.SelectedItem == null) return;
                switch (this.显示条目comboBox.SelectedItem.ToString())
                {
                    case "输入图像":
                        this.drawObject.AttachPropertyData.Clear();
                        this.drawObject.BackImage = ((RobotLayOff)this._function).ImageData;
                        break;
                    case "Try盘坐标":
                        foreach (var item in ((RobotLayOff)this._function).TryPlatformParam)
                        {
                            double row, col;
                            this.drawObject.BackImage.CamParams.WorldPointsToImagePlane(item.X, item.Y, this.drawObject.BackImage.Grab_X, this.drawObject.BackImage.Grab_Y, out row, out col);
                            this.drawObject.AddViewObject(new ViewData(new userPixPoint(row, col)));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }




}
