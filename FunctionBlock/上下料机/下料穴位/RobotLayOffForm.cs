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
    public partial class RobotLayOffForm : Form //
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        private RobotParam param;
        public RobotLayOffForm(IFunction function)
        {
            InitializeComponent();
            this._function = function;
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            this.Text = function.GetPropertyValues("名称").ToString();
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public RobotLayOffForm(TreeNode node)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            this.Text = node.Text;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void RobotLayOffForm_Load(object sender, EventArgs e)
        {
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
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
                this.操作comboBox.DataSource = Enum.GetValues(typeof(enOperation));
                this.最小灰度值comboBox.DataBindings.Add("Text", param, nameof(param.MinGray), true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大灰度值comboBox.DataBindings.Add("Text", param, nameof(param.MaxGray), true, DataSourceUpdateMode.OnPropertyChanged);
                this.矩形尺寸comboBox.DataBindings.Add("Text", param, nameof(param.RectSize), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿计算参考位comboBox.DataBindings.Add("Text", param, nameof(param.RefGrabPose), true, DataSourceUpdateMode.OnPropertyChanged);
                this.夹抓comboBox.DataBindings.Add("Text", param, nameof(param.RobotJaw), true, DataSourceUpdateMode.OnPropertyChanged);
                this.操作comboBox.DataBindings.Add("Text", param, nameof(param.Operate), true, DataSourceUpdateMode.OnPropertyChanged);
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
            BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
        }
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
                this.灰度值1Label.Text = e.GaryValue[0].ToString();
            else
                this.灰度值1Label.Text = 0.ToString();
            ///////////////////////////////////////////
            if (e.GaryValue.Length > 1)
                this.灰度值2Label.Text = e.GaryValue[1].ToString();
            else
                this.灰度值2Label.Text = 0.ToString();
            /////////////////////////////////////////
            if (e.GaryValue.Length > 2)
                this.灰度值3Label.Text = e.GaryValue[2].ToString();
            else
                this.灰度值3Label.Text = 0.ToString();
            ///////////////////////////////////////////////
            this.行坐标Label.Text = e.Row.ToString();
            this.列坐标Label.Text = e.Col.ToString();
        }
        private void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            if (e.DataContent != null)
            {
                ViewData viewData;
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this.drawObject.AttachPropertyData.Clear(); // 更新图像时清空
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        /////////////////////////////////////////////////////////////////////////////////////////////
                        break;
                    case nameof(HXLDCont):
                        this.drawObject.AddViewObject(new ViewData((HXLDCont)e.DataContent, "red"));
                        break;
                    case nameof(XldDataClass):
                        this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "red"));
                        break;
                    case nameof(RegionDataClass):
                        viewData = new ViewData(((RegionDataClass)e.DataContent).Region, "red", ((RegionDataClass)e.DataContent).Draw);
                        viewData.Draw = "margin";
                        viewData.Color = "red";
                        this.drawObject.AddViewObject(viewData);
                        break;
                    case nameof(userWcsRectangle2):
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect2).GetPixRectangle2().GetXLD(), wcsRect2.Color.ToString()));
                        break;
                    case nameof(userWcsRectangle1):
                        userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect1).GetPixRectangle1().GetXLD(), wcsRect1.Color.ToString()));
                        break;

                    case nameof(userWcsPoint):
                        userWcsPoint wcsPoint = (userWcsPoint)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPoint).GetPixPoint(), wcsPoint.Color.ToString()));
                        break;
                    case nameof(userWcsLine):
                        userWcsLine wcsLine = (userWcsLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsLine).GetPixLine().GetXLD(), wcsLine.Color.ToString()));
                        break;

                    case nameof(userWcsCircle):
                        userWcsCircle wcsCircle = (userWcsCircle)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsCircle).GetPixCircle().GetXLD(), wcsCircle.Color.ToString()));
                        break;
                    case nameof(userWcsCircleSector):
                        userWcsCircleSector wcsCircleSector = (userWcsCircleSector)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsCircleSector).GetPixCircleSector().GetXLD(), wcsCircleSector.Color.ToString()));
                        break;
                    case nameof(userWcsEllipse):
                        userWcsEllipse wcsEllipse = (userWcsEllipse)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsEllipse).GetPixEllipse().GetXLD(), wcsEllipse.Color.ToString()));
                        break;
                    case nameof(userWcsEllipseSector):
                        userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsEllipseSector).GetPixEllipseSector().GetXLD(), wcsEllipseSector.Color.ToString()));
                        break;
                    case nameof(userWcsCoordSystem):
                        userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData(wcsCoordSystem, "red"));
                        break;
                    case nameof(userPixPoint):
                        this.drawObject.AddViewObject(new ViewData(((userPixPoint)e.DataContent).GetXLD(), ((userPixPoint)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixLine):
                        this.drawObject.AddViewObject(new ViewData(((userPixLine)e.DataContent).GetXLD(), ((userPixLine)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircle):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircle)e.DataContent).GetXLD(), ((userPixCircle)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircleSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircleSector)e.DataContent).GetXLD(), ((userPixCircleSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipse):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipse)e.DataContent).GetXLD(), ((userPixEllipse)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipseSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipseSector)e.DataContent).GetXLD(), ((userPixEllipseSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle1):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle1)e.DataContent).GetXLD(), ((userPixRectangle1)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle2):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle2)e.DataContent).GetXLD(), ((userPixRectangle2)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userOkNgText):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    case nameof(userTextLable):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    default:
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                }
            }
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
