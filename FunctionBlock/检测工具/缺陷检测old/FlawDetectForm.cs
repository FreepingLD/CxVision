
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class FlawDetectForm : Form
    {
        private CancellationTokenSource cts;
        private IFunction _function;
        private VisualizeView drawObject;
        private FlawDetect _detect;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private TreeNode _refNode;
        private BindingList<DetectROIParam> listShapeROI;
        private HImage sourceImage;
        private FilterForm _FilterForm;
        private DetectFlawForm _DetectForm;


        public FlawDetectForm(IFunction function)
        {
            InitializeComponent();
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
            this._detect = (FlawDetect)function;
        }
        public FlawDetectForm(TreeNode node, ImageDataClass imageData = null, string treeViewName = null)
        {
            this._refNode = node;
            this._function = (IFunction)node.Tag;
            InitializeComponent();
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
            this.drawObject.BackImage = imageData;
            this._detect = (FlawDetect)this._function;
            this.titleLabel.Text = node.Text;
        }

        private void FlawDetectForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            //this.addContextMenu(this.hWindowControl1);
            //////////////////////////////////////////
            //////////////////////////////////////////
            this.ShapeCol.Items.Clear();
            this.ShapeCol.ValueType = typeof(enShapeType);
            foreach (enShapeType item in Enum.GetValues(typeof(enShapeType)))
            {
                this.ShapeCol.Items.Add(item);
            }
            this.OperateCol.Items.Clear();
            this.OperateCol.ValueType = typeof(enInsideOrOutside);
            foreach (enInsideOrOutside item in Enum.GetValues(typeof(enInsideOrOutside)))
            {
                this.OperateCol.Items.Add(item);
            }
            this.dataGridView1.TopLeftHeaderCell.Value = "序号";
            this.dataGridView1.DataSource = this.listShapeROI;
            ////////////////////
            if (this._detect.DicParam != null)
            {
                this.检测项listBox.Items.Clear();
                foreach (KeyValuePair<string, FlawDetectParam> item in this._detect.DicParam)
                {
                    this.检测项listBox.Items.Add(item.Key);
                }
            }
            ///////////////////////////////
            this._FilterForm = new FilterForm(this._detect.DicParam);
            this._DetectForm = new DetectFlawForm(this._detect.DicParam);
            this.AddForm(this.滤波参数groupBox, this._FilterForm);
            this.AddForm(this.检测参数groupBox, this._DetectForm);
        }

        public void LoadTreeNode()
        {
            foreach (TreeNode item in this._refNode.Nodes)
            {
                //this.treeView1.Nodes.Add(item.Clone() as TreeNode);// 
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

        private void FlawDetectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                /// 转移节点到主面板上来
                //this._refNode?.Nodes.Clear();
                //foreach (TreeNode item1 in this.treeView1.Nodes)
                //{
                //    //if (item1.Tag != null && ((BaseFunction)item1.Tag).IsVisualNode)
                //    this._refNode?.Nodes.Add(item1.Clone() as TreeNode);
                //}
                //this.treeView1.Nodes.Clear();
                //this.treeView1.Dispose();
                this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                //TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                //this.treeViewWrapClass?.Uinit();
            }
            catch
            {

            }

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
        private bool IsSelect()
        {
            bool result = false;
            Control container = this.Parent;
            if (container == null) return result;
            switch (container.GetType().Name)
            {
                case nameof(TabPage):
                    this.Invoke(new Action(() =>
                    {
                        TabControl tabControl = ((TabPage)container).Parent as TabControl;
                        if (tabControl.SelectedTab != ((TabPage)container))
                            result = false;
                        else
                            result = true;
                    }));
                    break;
            }
            return result;
        }
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合 ImageAcqCompleteEventArgs e
        {
            try
            {
                //if (!IsSelect()) return; // 如果不是当前选择的，则返回
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                if (SystemParamManager.Instance.SysConfigParam.IsAutoRun) return;
                /////////////////////////////////////////////
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.listData.Clear();
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        //this.CurrentImageData = this.drawObject.BackImage;
                        break;
                    case nameof(RegionDataClass):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((RegionDataClass)e.DataContent).Region;
                        else
                            this.listData.Add(e.ItemName, ((RegionDataClass)e.DataContent).Region);
                        break;
                    case nameof(XldDataClass):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((XldDataClass)e.DataContent);
                        else
                            this.listData.Add(e.ItemName, ((XldDataClass)e.DataContent));
                        break;
                    case "HXLDCont":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = (HXLDCont)e.DataContent;
                        else
                            this.listData.Add(e.ItemName, (HXLDCont)e.DataContent); // = new XldDataClass((HXLDCont)e.DataContent);
                        break;
                    case "userWcsCircle":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsCircle)e.DataContent).GetPixCircle().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsCircle)e.DataContent).GetPixCircle().GetAllXLD()); //(userWcsCircle)e.DataContent
                        break;
                    case "userWcsCircleSector":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsCircleSector)e.DataContent).GetPixCircleSector().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsCircleSector)e.DataContent).GetPixCircleSector().GetAllXLD());
                        break;
                    case "userWcsEllipse":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsEllipse)e.DataContent).GetPixEllipse().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsEllipse)e.DataContent).GetPixEllipse().GetAllXLD());
                        break;
                    case "userWcsEllipseSector":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsEllipseSector)e.DataContent).GetPixEllipseSector().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsEllipseSector)e.DataContent).GetPixEllipseSector().GetAllXLD());
                        break;
                    case "userWcsLine":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsLine)e.DataContent).GetPixLine().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsLine)e.DataContent).GetPixLine().GetAllXLD());
                        break;

                    case "userWcsPoint":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsPoint)e.DataContent).GetPixPoint();
                        else
                            this.listData.Add(e.ItemName, ((userWcsPoint)e.DataContent).GetPixPoint()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        break;
                    case "userWcsRectangle1":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsRectangle1)e.DataContent).GetPixRectangle1().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsRectangle1)e.DataContent).GetPixRectangle1().GetAllXLD());
                        break;
                    case "userWcsRectangle2":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsRectangle2)e.DataContent).GetPixRectangle2().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsRectangle2)e.DataContent).GetPixRectangle2().GetAllXLD());
                        break;
                    case "userWcsRectangle2[]":
                        foreach (var item in (userWcsRectangle2[])e.DataContent)
                        {
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = item.GetPixRectangle2().GetAllXLD();
                            else
                                this.listData.Add(e.ItemName, item.GetPixRectangle2().GetAllXLD());
                        }
                        break;
                    case "userWcsCoordSystem":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        break;
                    case "userOkNgText":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        break;
                }
                /////////////////////////////
                Task.Run(() =>
                {
                    this.drawObject.AttachPropertyData.Clear();
                    //this.drawObject.IsDispalyAttachDrawingProperty = false;
                    foreach (KeyValuePair<string, object> item in this.listData)
                    {
                        this.drawObject.AttachPropertyData.Add(item.Value);
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            try
            {
                //switch (e.Node.Tag.GetType().Name)
                //{
                //    case "CircleMeasure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawCircleMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition, ((CircleMeasure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                //        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData != null ? ((CircleMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (CircleMeasure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;
                //    case "CircleSectorMeasure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawCircleSectorMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition, ((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                //        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;
                //    case "EllipseMeasure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawEllipseMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition, ((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                //        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (EllipseMeasure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;
                //    case "EllipseSectorMeasure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition, ((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                //        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;
                //    case "LineMeasure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawLineMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LinePixPosition, ((LineMeasure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                //        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (LineMeasure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;
                //    case "PointMeasure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawPointMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition, ((PointMeasure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                //        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (PointMeasure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;
                //    case "Rectangle2Measure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawRect2Measure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition, ((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                //        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;
                //    case "CrossPointMeasure":
                //        this.drawObject?.AttachPropertyData.Clear();
                //        if (!(this.drawObject is userDrawCrossMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawCrossMeasure(this.hWindowControl1, ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition, ((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (KeyValuePair<string, object> item in this.listData)
                //        {
                //            if (item.Key != e.Node.Text)
                //                this.drawObject.AttachPropertyData.Add(item.Value);
                //        }
                //        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                //        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                //        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData != null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (CrossPointMeasure)e.Node.Tag;
                //        //DisplayClickItem(sender, e);
                //        break;

                //    ///////////////////////////////////////// 显示测量距离对象
                //    case "CircleToCircleDist2D":
                //    case "CircleToLineDist2D":
                //    case "LineToLineDist2D":
                //    case "PointToLineDist2D":
                //        // DisplayClickItem(sender, e);
                //        break;
                //    default:

                //        break;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DisplayClickItem(object sender, TreeNodeMouseClickEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.Node.Tag == null) return;
                if (e.Node.Name == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                this.drawObject.AttachPropertyData.Clear();
                //switch (e.Node.Tag.GetType().Name)
                //{
                //    case "CircleMeasure":
                //    case "CircleSectorMeasure":
                //    case "EllipseMeasure":
                //    case "EllipseSectorMeasure":
                //    case "LineMeasure":
                //    case "PointMeasure":
                //    case "Rectangle2Measure":
                //    case "WidthMeasure":
                //        this.drawObject.AttachPropertyData.Clear(); // 清空附加属性
                //        this.drawObject.IsDispalyAttachEdgesProperty = true;
                //        // 添加需要显示的元素
                //        foreach (var items in this.listData.Keys)
                //        {
                //            if (items.Split('-')[0] == e.Node.Name) continue; // + "-" + "0"
                //            this.drawObject.AttachPropertyData.Add(this.listData[items]);
                //        }
                //        this.drawObject.DrawingGraphicObject();
                //        break;
                //}
            }
            catch (Exception he)
            {

            }
        }


        #region 右键菜单项
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            ToolStripItem[] items = null;
            switch (SystemParamManager.Instance.SysConfigParam.Language)
            {
                default:
                case "zh-CN":
                    // 添加右键菜单 
                    items = new ToolStripMenuItem[]
                   {
                     //new ToolStripMenuItem("执行",null,null,"执行"),
                     new ToolStripMenuItem("自适应图像",null,null,"自适应图像"),
                     //new ToolStripMenuItem("设置抓边参数",null,null,"设置抓边参数"),
                     //new ToolStripMenuItem("------------"),
                     new ToolStripMenuItem("清除窗口",null,null,"清除窗口"),
                     new ToolStripMenuItem("保存图像",null,null,"保存图像"),
                   };
                    break;
                case "en-US":
                    // 添加右键菜单 
                    items = new ToolStripMenuItem[]
                   {
                     //new ToolStripMenuItem("Excute",null,null,"执行"),
                     new ToolStripMenuItem("Auto Image Window",null,null,"自适应图像"),
                     //new ToolStripMenuItem("Set Callipers Param",null,null,"设置抓边参数"),
                     //new ToolStripMenuItem("------------"),
                     new ToolStripMenuItem("Clear Window",null,null,"清除窗口"),
                     new ToolStripMenuItem("Save Image",null,null,"保存图像"),
                   };
                    break;
            }
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContextMenuStrip_ItemClicked);
            hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void hWindowControlContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    //case "执行":
                    //    switch (this._currFunction.GetType().Name)
                    //    {
                    //        case "CircleMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixCircleParam());
                    //            break;
                    //        case "CircleSectorMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam());
                    //            break;
                    //        case "EllipseMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixEllipseParam());
                    //            break;
                    //        case "EllipseSectorMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixEllipseSectorParam());
                    //            break;
                    //        case "LineMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                    //            break;
                    //        case "PointMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                    //            break;
                    //        case "Rectangle2Measure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                    //            break;
                    //        case "WidthMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                    //            break;
                    //        case "CrossPointMeasure":
                    //            this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                    //            break;
                    //    }
                    //    break;
                    //////////////////////////////////////
                    case "自适应窗口":
                        this.drawObject?.AutoWindows();
                        break;
                    case "清除窗口":
                        this.drawObject?.ClearWindow();
                        this.listData.Clear(); // 清除窗口时,对象也清除
                        break;
                    //case "设置抓边参数":
                    //    MetrolegyParamForm paramForm = new MetrolegyParamForm(this._currFunction, this.drawObject);
                    //    paramForm.Show();
                    //    paramForm.Owner = this;
                    //    break;
                    case "保存图像":
                        ((ContextMenuStrip)sender).Close();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                            MessageBox.Show("图像内容为空");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region  窗体移动功能

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion

        #region  窗体绽放功能 
        private const int Guying_HTLEFT = 10;
        private const int Guying_HTRIGHT = 11;
        private const int Guying_HTTOP = 12;
        private const int Guying_HTTOPLEFT = 13;
        private const int Guying_HTTOPRIGHT = 14;
        private const int Guying_HTBOTTOM = 15;
        private const int Guying_HTBOTTOMLEFT = 0x10;
        private const int Guying_HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 2)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

        #region 防止改变窗口大小时控件闪烁功能
        protected override CreateParams CreateParams   //
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        #endregion

        #region 窗体控制盒功能，关闭，最大化，最小化
        private void buttonMin_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;  //最小化
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            //DialogResult dialogResult = MessageBox.Show("确定关闭窗体吗？", "关闭窗体", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            this.Close();  //关闭窗口
            //}
        }
        #endregion

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            FeatureLocalizationForm_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
            //this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }

        private void FeatureLocalizationForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private TreeNode rootNode;
        private void 添加检测项button_Click(object sender, EventArgs e)
        {
            try
            {
                this.检测项listBox.Items.Add($"检测项({this.检测项listBox.Items.Count})");
                this._detect.DicParam.Add(this.检测项listBox.Items[this.检测项listBox.Items.Count - 1].ToString(), new FlawDetectParam());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void 检测项listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.检测项listBox.SelectedItem == null) return;
                if (this._detect.DicParam != null && this._detect.DicParam.ContainsKey(this.检测项listBox.SelectedItem.ToString()))
                {
                    this.listShapeROI = this._detect.DicParam[this.检测项listBox.SelectedItem.ToString()].DetectROI;
                    this.dataGridView1.DataSource = this.listShapeROI;
                    this._FilterForm.CurrentItem = this.检测项listBox.SelectedItem.ToString();
                    this._DetectForm.CurrentItem = this.检测项listBox.SelectedItem.ToString();
                    this._FilterForm.方法comboBox.Text = "";
                    this._DetectForm.检测算法comboBox.Text = "";
                    this._FilterForm.方法comboBox.Text = this._detect.DicParam[this.检测项listBox.SelectedItem.ToString()].FilterParam.Method;
                    this._DetectForm.检测算法comboBox.Text = this._detect.DicParam[this.检测项listBox.SelectedItem.ToString()].DetectParam.Method;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除检测项button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.检测项listBox.SelectedItem == null) return;
                if (this._detect.DicParam != null && this._detect.DicParam.ContainsKey(this.检测项listBox.SelectedItem.ToString()))
                    this._detect.DicParam.Remove(this.检测项listBox.SelectedItem.ToString());
                this.检测项listBox.Items.Remove(this.检测项listBox.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void 执行button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.toolStripStatusLabel2.Text == "等待……") return;
                this.toolStripStatusLabel2.Text = "等待……";
                this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                this.cts = new CancellationTokenSource();
                this.执行button.Enabled = false;
                await Task.Run(() =>
               {
                   if (this._function.Execute(this._refNode, "manual").Succss)
                   {
                       if (!this.cts.IsCancellationRequested)
                       {
                           this.Invoke(new Action(() =>
                           {
                               //this.toolStripStatusLabel1.Text = "执行结果:";
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
                               //this.toolStripStatusLabel1.Text = "执行结果:";
                               this.toolStripStatusLabel2.Text = "失败";
                               this.toolStripStatusLabel2.ForeColor = Color.Red;
                           }));
                       }
                   }
               }
                );
                /////////////////////////////
                this.执行button.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    if (this.listShapeROI == null)
                        this.listShapeROI = new BindingList<DetectROIParam>();
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置参数!");
                                return;
                            }
                            switch (listShapeROI[e.RowIndex].ShapeType)
                            {
                                case enShapeType.矩形2:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect2ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    break;
                                case enShapeType.矩形1:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect1ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    break;
                                case enShapeType.圆:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawCircleROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    break;
                                case enShapeType.椭圆:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawEllipseROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    break;
                                case enShapeType.多边形:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPolygonROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    break;
                                case enShapeType.点:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPointROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    break;
                                case enShapeType.线:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawLineROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    break;
                                default:
                                    throw new NotImplementedException(listShapeROI[e.RowIndex].ShapeType.ToString() + "未实现!");
                            }
                            this.drawObject.IsLiveState = true;
                            //////////////////////////
                            foreach (var item in listShapeROI)
                            {
                                if (index != e.RowIndex && item.RoiShape != null)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            if (this.drawObject.BackImage == null)
                                this.drawObject.BackImage = ((FlawDetect)this._function).ImageData == null ? new ImageDataClass(this.sourceImage) : ((FlawDetect)this._function).ImageData;
                            if (listShapeROI[e.RowIndex].RoiShape == null)
                                this.drawObject.SetParam(null);
                            else
                            {
                                //this.drawObject.SetParam(pixCoordSystem);
                                this.drawObject.SetParam(listShapeROI[e.RowIndex].RoiShape);
                            }
                            this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString())); //这里也要做下变换
                            /////////////////////////////////////////////////////////
                            listShapeROI[e.RowIndex].RoiShape = pixShape;   // 这个地方的添加不能使用变换后数据
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "DeletCol":
                            if (listShapeROI == null) return;
                            if (listShapeROI.Count > e.RowIndex)
                                listShapeROI.RemoveAt(e.RowIndex);
                            if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                                this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                            this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        default:
                            this.drawObject.AttachPropertyData.Clear();
                            foreach (var item in listShapeROI)
                            {
                                if (item.RoiShape == null) return;
                                if (index == e.RowIndex)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.green.ToString()));
                                }
                                else
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            this.drawObject.DrawingGraphicObject();
                            break;
                    }
                    this.dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "RoiShape")
                {
                    /////////////////////////
                    e.Value = EvaluateValue(this.dataGridView1.Rows[e.RowIndex].DataBoundItem, this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                    if (e.Value != null)
                        e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public string EvaluateValue(object obj, string property)
        {
            string prop = property;
            string ret = string.Empty;
            if (obj == null) return ret;
            if (property.Contains("."))
            {
                prop = property.Substring(0, property.IndexOf("."));
                System.Reflection.PropertyInfo[] props = obj.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo propa in props)
                {
                    object obja = propa.GetValue(obj, new object[] { });
                    if (obja.GetType().Name.Contains(prop))
                    {
                        ret = this.EvaluateValue(obja, property.Substring(property.IndexOf(".") + 1)); // 回调
                        break;
                    }
                }
            }
            else
            {
                System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prop);
                ret = pi?.GetValue(obj, new object[] { })?.ToString();
            }
            return ret;
        }

        private void 图像加载button_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|hdev文件(.hdev)|*.hdev|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 0;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.sourceImage = new HImage(ofd.FileName);
                    this.drawObject.BackImage = new ImageDataClass(this.sourceImage); //, this._acqSource.Sensor.CameraParam
                    //this.drawObject.BackImage.ViewWindow = this._viewConfigParam.ViewName;
                    this.drawObject.BackImage.Tag = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 重命名button_Click(object sender, EventArgs e)
        {
            try
            {
                RenameForm form = new RenameForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string selectItemName = this.检测项listBox.SelectedItem.ToString();
                    if(!this._detect.DicParam.ContainsKey(form.ReName))
                    {
                        this._detect.DicParam.Add(form.ReName, this._detect.DicParam[selectItemName]);
                        this._detect.DicParam.Remove(selectItemName);
                        /////////////////////////////////
                        this.检测项listBox.Items.Insert(this.检测项listBox.SelectedIndex,form.ReName);
                        this.检测项listBox.Items.Remove(this.检测项listBox.SelectedItem);
                    }
                    else
                        MessageBox.Show("已包含有相同的项:" + form.ReName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

                    break;
                default:
                    break;
            }
        }


    }
}
