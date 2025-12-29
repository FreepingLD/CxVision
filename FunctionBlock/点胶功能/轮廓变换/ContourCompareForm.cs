
using Common;
using FunctionBlock;
using HalconDotNet;
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
    public partial class ContourCompareForm : Form
    {
        protected Form form;
        protected IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;


        public ContourCompareForm(TreeNode node)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = node.Tag as IFunction;
            this.Text = this._function?.GetPropertyValues("名称").ToString();
            this._function.SetPropertyValues(nameof(TreeNode), this._refNode);
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            //new ListBoxWrapClass().InitListBox(this.listBox1, node);
            //new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }  
        public void ContourCompareForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
        }
        public enum enShowItemsContourAffine
        {
            输入轮廓,
            变换轮廓,
            输出显示,
        }

        protected void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItemsContourAffine));
                //ContourAffineParam param = ((ContourAffine)this._function).Param;
                //this.原点X偏移comboBox.DataBindings.Add("Text", param, nameof(param.Offset_X), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.原点Y偏移comboBox.DataBindings.Add("Text", param, nameof(param.Offset_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.变换方向comboBox.DataBindings.Add("Text", param, nameof(param.Orientation), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反变换方向checkBox.DataBindings.Add(nameof(this.取反变换方向checkBox.Checked), param, nameof(param.InvertAffineOrientation), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        public void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)
        {
            try
            {
                if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, ((XldDataClass)e.DataContent).Color.ToString()));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent).Region, ((RegionDataClass)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsCircle":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircle)e.DataContent), ((userWcsCircle)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircleSector)e.DataContent), ((userWcsCircleSector)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipse)e.DataContent), ((userWcsEllipse)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipseSector)e.DataContent), ((userWcsEllipseSector)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsLine":
                            this.drawObject.AddViewObject(new ViewData(((userWcsLine)e.DataContent), ((userWcsLine)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsPoint":
                            this.drawObject.AddViewObject(new ViewData(((userWcsPoint)e.DataContent), ((userWcsPoint)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle1)e.DataContent), ((userWcsRectangle1)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle2)e.DataContent), ((userWcsRectangle2)e.DataContent).Color.ToString()));
                            break;
                    }
                }
            }
            catch (Exception he)
            {

            }
        }
        private void listbox_AddItems(object send, ItemsChangeEventArgs e)
        {
            try
            {
                object object3D;
                if (e.ItemName == null || e.ItemName.Trim().Length == 0) return;
                if (e.ItemName.Split('.').Length == 1)
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName);
                else
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName.Split('.')[1]);
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)object3D).HXldCont, ((XldDataClass)object3D).Color.ToString()));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(object3D, "red"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, ((RegionDataClass)object3D).Color.ToString()));
                            break;
                        case "userWcsCircle":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircle)object3D), ((userWcsCircle)object3D).Color.ToString()));
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircleSector)object3D), ((userWcsCircleSector)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipse)object3D), ((userWcsEllipse)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipseSector)object3D), ((userWcsEllipseSector)object3D).Color.ToString()));
                            break;
                        case "userWcsLine":
                            this.drawObject.AddViewObject(new ViewData(((userWcsLine)object3D), ((userWcsLine)object3D).Color.ToString()));
                            break;
                        case "userWcsPoint":
                            this.drawObject.AddViewObject(new ViewData(((userWcsPoint)object3D), ((userWcsPoint)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle1)object3D), ((userWcsRectangle1)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle2)object3D), ((userWcsRectangle2)object3D).Color.ToString()));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                ///////////////////////////////////////////
                if (this.显示条目comboBox.SelectedIndex == -1) return;
                switch (this.显示条目comboBox.Text.Trim())
                {
                    case nameof(enShowItemsContourAffine.输入轮廓):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        userWcsPoint[] wcsPoints = ((ContourAffine)this._function).WcsPoint;
                        if (wcsPoints != null)
                        {
                            double[] x = new double[wcsPoints.Length];
                            double[] y = new double[wcsPoints.Length];
                            double[] z = new double[wcsPoints.Length];
                            for (int i = 0; i < wcsPoints.Length; i++)
                            {
                                x[i] = wcsPoints[i].X;
                                y[i] = wcsPoints[i].Y;
                                z[i] = wcsPoints[i].Z;
                            }
                            this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D(x, y, z));
                        }
                        break;
                    case nameof(enShowItemsContourAffine.变换轮廓):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        /////////////////////////////////////////////////////////////////////////
                        userWcsPolyLine wcsPolyLine = ((ContourAffine)this._function).WcsPolyLine;
                        if (wcsPolyLine != null)
                        {
                            double[] x = new double[wcsPolyLine.X.Count];
                            double[] y = new double[wcsPolyLine.X.Count];
                            double[] z = new double[wcsPolyLine.X.Count];
                            for (int i = 0; i < wcsPolyLine.X.Count; i++)
                            {
                                x[i] = wcsPolyLine.X[i];
                                y[i] = wcsPolyLine.Y[i];
                                z[i] = wcsPolyLine.Z[i];
                            }
                            this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D(x, y, z));
                        }
                        break;

                    case nameof(enShowItemsContourAffine.输出显示):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        //////////////////////////////////////////////////////////////////////////
                        HObjectModel3D hObjectModel3D1 = null, hObjectModel3D2 = null;
                        wcsPoints = ((ContourAffine)this._function).WcsTargetPoint;
                        if (wcsPoints != null)
                        {
                            double[] x = new double[wcsPoints.Length];
                            double[] y = new double[wcsPoints.Length];
                            double[] z = new double[wcsPoints.Length];
                            for (int i = 0; i < wcsPoints.Length; i++)
                            {
                                x[i] = wcsPoints[i].X;
                                y[i] = wcsPoints[i].Y;
                                z[i] = wcsPoints[i].Z;
                            }
                            hObjectModel3D1 = new HObjectModel3D(x, y, z);
                        }
                        /////////////////////////////////////////////////////////////////////////
                        wcsPolyLine = ((ContourAffine)this._function).WcsPolyLine;
                        if (wcsPolyLine != null)
                        {
                            double[] x = new double[wcsPolyLine.X.Count];
                            double[] y = new double[wcsPolyLine.X.Count];
                            double[] z = new double[wcsPolyLine.X.Count];
                            for (int i = 0; i < wcsPolyLine.X.Count; i++)
                            {
                                x[i] = wcsPolyLine.X[i];
                                y[i] = wcsPolyLine.Y[i];
                                z[i] = wcsPolyLine.Z[i];
                            }
                            hObjectModel3D2 = new HObjectModel3D(x, y, z);
                        }
                        if (hObjectModel3D1 != null && hObjectModel3D1.IsInitialized())
                            this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D[2] { hObjectModel3D1, hObjectModel3D2 });
                        else
                            this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D[1] { hObjectModel3D2 });
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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
                    this.drawObject.AutoWindows();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case "toolStripButton_3D":
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    this.drawObject.Show3D();
                    break;
                default:
                    break;
            }
        }

        protected void ContourAffineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
            }
            catch
            {

            }
        }



    }
}
