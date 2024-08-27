using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
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
    public partial class DrawForm : Form
    {
        private Form form;
        private VisualizeView drawObject;
        private ImageDataClass _imageData;
        private PixROI _ROI;
        public PixROI ROI { get => _ROI; set => _ROI = value; }


        public DrawForm(ImageDataClass imageData, PixROI roi)
        {
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            this.drawObject.BackImage = imageData;
            this._imageData = imageData;
            this._ROI = roi;
        }
        private void DrawForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            //ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            //////////////////////////////////////////////////////////

        }
        private void BindProperty()
        {
            try
            {
                //this.显示条目comboBox.DataSource = Enum.GetValues(typeof(enShapeType));
                //this.显示条目comboBox.SelectedIndex = 4;
                if (this._ROI != null)
                {
                    switch (this._ROI.GetType().Name)
                    {
                        case nameof(drawPixCircle):
                            this.绘图toolStripDropDownButton.Text = enShapeType.圆.ToString();
                            break;
                        case nameof(drawPixRect1):
                            this.绘图toolStripDropDownButton.Text = enShapeType.矩形1.ToString();
                            break;
                        case nameof(drawPixRect2):
                            this.绘图toolStripDropDownButton.Text = enShapeType.矩形2.ToString();
                            break;
                        case nameof(drawPixEllipse):
                            this.绘图toolStripDropDownButton.Text = enShapeType.椭圆.ToString();
                            break;
                        case nameof(drawPixLine):
                            this.绘图toolStripDropDownButton.Text = enShapeType.线.ToString();
                            break;
                        case nameof(drawPixPolygon):
                            this.绘图toolStripDropDownButton.Text = enShapeType.多边形.ToString();
                            break;
                    }
                    this.drawObject.AddViewObject(new ViewData(this._ROI.GetXLD(), enColor.orange.ToString()));
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


        // 获取鼠标位置处的高度值
        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
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
        private void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent != null)
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent);
                            this._imageData = this.drawObject.BackImage;
                            this.drawObject.AttachPropertyData.Clear();
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            this._imageData = this.drawObject.BackImage;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "red"));
                            break;
                        case "XldDataClass[]":
                            XldDataClass[] xldDataClasses = (XldDataClass[])e.DataContent;
                            foreach (var item in xldDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(((XldDataClass)item).HXldCont, "red"));
                            }
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)e.DataContent), "red"));
                            break;
                        case "HRegion":
                            this.drawObject.AddViewObject(new ViewData(((HRegion)e.DataContent), "red"));
                            break;
                        case "RegionDataClass[]":
                            RegionDataClass[] regionDataClasses = (RegionDataClass[])e.DataContent;
                            foreach (var item in regionDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(item, "red"));
                            }
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent).Region, "red"));
                            break;
                    }
                }
            }
            catch (Exception he)
            {
                MessageBox.Show("DisplayObjectModel->操作失败");
            }
        }

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawObject.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawObject.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Translate":
                    this.drawObject.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawObject.AutoImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case "toolStripButton_3D":
                    this.drawObject.Show3D();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "绘图toolStripDropDownButton":
                    ToolStripItemClickedEventArgs toolStripItem = new ToolStripItemClickedEventArgs(new ToolStripMenuItem());
                    toolStripItem.ClickedItem.Text = this.绘图toolStripDropDownButton.Text;
                    //this.绘图toolStripDropDownButton_DropDownItemClicked(this, toolStripItem);
                    break;
                case "删除ROItoolStripButton":
                    this._ROI = null;
                    this.drawObject?.ClearViewObject();
                    break;
                default:
                    break;
            }
        }

        private void DrawForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                //BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            }
            catch
            {

            }
        }

        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                //if (this.显示条目comboBox.SelectedItem != null)
                //{
                //    PixROI pixShape;
                //    enShapeType ShapeType = enShapeType.矩形1;
                //    Enum.TryParse(this.显示条目comboBox.SelectedItem.ToString(), out ShapeType);
                //    switch (ShapeType)
                //    {
                //        case enShapeType.矩形2:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawRect2ROI))
                //            {
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //            }
                //            break;
                //        case enShapeType.矩形1:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawRect1ROI))
                //            {
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //            }
                //            break;
                //        case enShapeType.圆:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawCircleROI))
                //            {
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //            }
                //            break;
                //        case enShapeType.椭圆:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawEllipseROI))
                //            {
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //            }
                //            break;
                //        case enShapeType.多边形:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawPolygonROI))
                //            {
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //            }
                //            break;
                //        case enShapeType.点:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawPointROI))
                //            {
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //            }
                //            break;
                //        case enShapeType.线:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawLineROI))
                //            {
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //            }
                //            break;
                //        default:
                //            throw new NotImplementedException("未实现的绘图类型!");
                //    }
                //    this.drawObject.IsLiveState = true;
                //    //////////////////////////
                //    if (this.drawObject.BackImage == null)
                //        this.drawObject.BackImage = this._imageData;
                //    if (this._ROI == null)
                //        this.drawObject.SetParam(null);
                //    else
                //    {
                //        this.drawObject.SetParam(this._ROI);
                //    }
                //    this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                //    this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                //    /////////////////////////////////////////////////////////
                //    this._ROI = pixShape; //.GetWcsROI(this.drawObject.CameraParam);  // 这个地方的添加不能使用变换后数据
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 绘图toolStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                PixROI pixShape;
                enShapeType ShapeType = enShapeType.NONE;
                string name = e.ClickedItem.Text;
                Task.Run(() =>
                {
                    switch (name)
                    {
                        case "矩形1":
                            ShapeType = enShapeType.矩形1;
                            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.矩形1.ToString()));
                            this.绘图toolStripDropDownButton.Text = enShapeType.矩形1.ToString();
                            break;
                        case "矩形2":
                            ShapeType = enShapeType.矩形2;
                            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.矩形2.ToString()));
                            //this.绘图toolStripDropDownButton.Text = enShapeType.矩形2.ToString();
                            break;
                        case "圆形":
                            ShapeType = enShapeType.圆;
                            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.圆.ToString()));
                            //this.绘图toolStripDropDownButton.Text = enShapeType.圆.ToString();
                            break;
                        case "直线":
                            ShapeType = enShapeType.线;
                            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.线.ToString()));
                            //this.绘图toolStripDropDownButton.Text = enShapeType.线.ToString();
                            break;
                        case "椭圆":
                            ShapeType = enShapeType.椭圆;
                            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.椭圆.ToString()));
                            //this.绘图toolStripDropDownButton.Text = enShapeType.椭圆.ToString();
                            break;
                        case "多边形":
                            ShapeType = enShapeType.多边形;
                            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.多边形.ToString()));
                            //this.绘图toolStripDropDownButton.Text = enShapeType.多边形.ToString();
                            break;
                        default:
                            break;
                    }
                    if (ShapeType == enShapeType.NONE) return;
                    switch (ShapeType)
                    {
                        case enShapeType.矩形2:
                            this.drawObject.AttachPropertyData.Clear();
                            if (!(this.drawObject is userDrawRect2ROI))
                            {
                                this._ROI = null;
                                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                this.drawObject.ClearDrawingObject();
                                this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            }
                            break;
                        case enShapeType.矩形1:
                            this.drawObject.AttachPropertyData.Clear();
                            if (!(this.drawObject is userDrawRect1ROI))
                            {
                                this._ROI = null;
                                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                this.drawObject.ClearDrawingObject();
                                this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            }
                            break;
                        case enShapeType.圆:
                            this.drawObject.AttachPropertyData.Clear();
                            if (!(this.drawObject is userDrawCircleROI))
                            {
                                this._ROI = null;
                                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                this.drawObject.ClearDrawingObject();
                                this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            }
                            break;
                        case enShapeType.椭圆:
                            this.drawObject.AttachPropertyData.Clear();
                            if (!(this.drawObject is userDrawEllipseROI))
                            {
                                this._ROI = null;
                                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                this.drawObject.ClearDrawingObject();
                                this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            }
                            break;
                        case enShapeType.多边形:
                            this.drawObject.AttachPropertyData.Clear();
                            if (!(this.drawObject is userDrawPolygonROI))
                            {
                                this._ROI = null;
                                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                this.drawObject.ClearDrawingObject();
                                this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            }
                            break;
                        case enShapeType.点:
                            this.drawObject.AttachPropertyData.Clear();
                            if (!(this.drawObject is userDrawPointROI))
                            {
                                this._ROI = null;
                                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                this.drawObject.ClearDrawingObject();
                                this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            }
                            break;
                        case enShapeType.线:
                            this.drawObject.AttachPropertyData.Clear();
                            if (!(this.drawObject is userDrawLineROI))
                            {
                                this._ROI = null;
                                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                this.drawObject.ClearDrawingObject();
                                this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            }
                            break;
                        default:
                            throw new NotImplementedException("未实现的绘图类型!");
                    }
                    this.drawObject.IsLiveState = true;
                    //////////////////////////
                    if (this.drawObject.BackImage == null)
                        this.drawObject.BackImage = this._imageData;
                    if (this._ROI == null)
                        this.drawObject.SetParam(null);
                    else
                    {
                        this.drawObject.SetParam(this._ROI);
                    }
                    this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                    this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                    /////////////////////////////////////////////////////////
                    this._ROI = pixShape; //.GetWcsROI(this.drawObject.CameraParam);  // 这个地方的添加不能使用变换后数据
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }

}
