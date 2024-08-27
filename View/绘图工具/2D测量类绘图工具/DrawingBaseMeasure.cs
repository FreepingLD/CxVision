
using Common;
using HalconDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public class DrawingBaseMeasure
    {
        protected HWindowControl hWindowControl;
        protected HWindow bufferWindow;
        // 这两个变量用于记录初始图像大小
        private int initImageWidth = 0;
        private int initImageHeight = 0;
        private int initWindowWidth = 0;
        private int initWindowHeight = 0;
        private int imagePartRow1;
        private int imagePartCol1;
        private int imagePartRow2;
        private int imagePartCol2;
        protected ImageDataClass backImage;
        protected bool mIsClick = false;
        protected double oldX;
        protected double oldY;
        protected double nodeSizeRect = GlobalVariable.pConfig.NodeSize;
        protected bool isTranslate = true; // 一开始就可以移动
        protected bool isDrwingObject = false; // 表示窗口上是否有绘图对象
        protected bool isInitEvent = false;
        private List<object> attachDrawingPropertyData = new List<object>();
        private List<object> _attachPropertyData = new List<object>();
        protected bool isDispalyAttachDrawingProperty = true;
        protected bool isDispalyAttachEdgesProperty = true;
        private CameraParam cameraParam;
        private double refPoint_x;
        private double refPoint_y;
        private double refPoint_z;
        //private object monitor = new object();
        //private bool lockState;
        public event GrayValueInfoEventHandler GrayValueInfo;
        private enShowMode _ShowMode = enShowMode.箭头;

        /// <summary>
        /// 表示实时采集状态
        /// </summary>
        public bool IsLiveState { get; set; }
        public bool IsTranslate
        {
            get
            {
                return isTranslate;
            }

            set
            {
                isTranslate = value;
            }
        }
        public ImageDataClass BackImage
        {
            get
            {
                return backImage;
            }

            set
            {
                if (value == null) return;
                if (!mIsClick)
                {
                    ///////////////////////
                    backImage = value;
                    //lockState = Monitor.TryEnter(this.monitor);
                    //if (!lockState) return;
                    this.cameraParam = backImage.CamParams;
                    this.refPoint_x = backImage.Grab_X;
                    this.refPoint_y = backImage.Grab_Y;
                    this.refPoint_z = backImage.Grab_Z;
                    //SetImagePartSize(this.cameraParam.DataWidth, this.cameraParam.DataHeight, false); // 在控件大小，图像大小都不变的情况下，不更新视图
                    SetImagePartSize(backImage.Width, backImage.Height, false); // 在控件大小，图像大小都不变的情况下，不更新视图
                    DrawingGraphicObject();
                    //Monitor.Exit(this.monitor);
                }
            }
        }
        protected List<object> AttachDrawingPropertyData
        {
            get
            {
                return attachDrawingPropertyData;
            }

            set
            {
                attachDrawingPropertyData = value;
            }
        }
        public bool IsDispalyAttachDrawingProperty
        {
            get
            {
                return isDispalyAttachDrawingProperty;
            }

            set
            {
                isDispalyAttachDrawingProperty = value;
            }
        }

        public List<object> AttachPropertyData
        {
            get
            {
                return _attachPropertyData;
            }

            set
            {
                _attachPropertyData = value;
            }
        }

        public bool IsDispalyAttachEdgesProperty { get => isDispalyAttachEdgesProperty; set => isDispalyAttachEdgesProperty = value; }
        public int InitImageWidth { get => initImageWidth; set => initImageWidth = value; }
        public int InitImageHeight { get => initImageHeight; set => initImageHeight = value; }
        public int InitWindowWidth { get => initWindowWidth; set => initWindowWidth = value; }
        public int InitWindowHeight { get => initWindowHeight; set => initWindowHeight = value; }
        public int ImagePartRow1 { get => imagePartRow1; set => imagePartRow1 = value; }
        public int ImagePartCol1 { get => imagePartCol1; set => imagePartCol1 = value; }
        public int ImagePartCol2 { get => imagePartCol2; set => imagePartCol2 = value; }
        public int ImagePartRow2 { get => imagePartRow2; set => imagePartRow2 = value; }
        public enShowMode ShowMode { get => _ShowMode; set => _ShowMode = value; }
        public CameraParam CameraParam { get => cameraParam; set => cameraParam = value; }

        protected virtual void OnGaryValueInfo(GrayValueInfoEventArgs e)
        {
            if (this.GrayValueInfo != null)
                this.GrayValueInfo.Invoke(this, e);
        }
        public DrawingBaseMeasure(HWindowControl hWindowControl, ImageDataClass backImage)
        {
            this.isDrwingObject = true;
            //this.addContextMenuStrip();
            this.hWindowControl = hWindowControl;
            this.backImage = backImage;
            if (!this.isInitEvent)
            {
                this.isInitEvent = true;
                this.hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseDown);
                this.hWindowControl.HMouseUp += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseUp);
                this.hWindowControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseWheel);
                this.hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseMove);
                this.hWindowControl.SizeChanged += new System.EventHandler(hWindowControl_SizeChanged);
            }
            initBufferWindow(this.hWindowControl);
            //////////////
            //HOperatorSet.ResetObjDb(1280, 1024, 0); // 这里需要设置一个值以满足全局的参数需要 ：这个算子会影响图像视图的大小，其大小等于
        }
        public DrawingBaseMeasure(HWindowControl hWindowControl)
        {
            this.isDrwingObject = true;
            this.hWindowControl = hWindowControl;
            //this.addContextMenuStrip();
            if (!this.isInitEvent)
            {
                this.isInitEvent = true;
                this.hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseDown);
                this.hWindowControl.HMouseUp += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseUp);
                this.hWindowControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseWheel);
                this.hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseMove);
                this.hWindowControl.SizeChanged += new System.EventHandler(hWindowControl_SizeChanged);
            }
            initBufferWindow(this.hWindowControl); // 初始化时要创建Buffer窗口           
                                                   //HOperatorSet.ResetObjDb(1280, 1024, 0); // 这个方法为全局方法，后面的设置会赋盖前面的设置,：这个算子会影响图像视图的大小，其设置值等视图大小等于
        }
        public DrawingBaseMeasure(HWindowControl hWindowControl, bool isAppendContext)
        {
            this.isDrwingObject = true;
            this.hWindowControl = hWindowControl;
            if (isAppendContext)
                this.addContextMenuStrip();
            if (!this.isInitEvent)
            {
                this.isInitEvent = true;
                this.hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseDown);
                this.hWindowControl.HMouseUp += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseUp);
                this.hWindowControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseWheel);
                this.hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseMove);
                this.hWindowControl.SizeChanged += new System.EventHandler(hWindowControl_SizeChanged);
            }
            initBufferWindow(this.hWindowControl); // 初始化时要创建Buffer窗口
            //HOperatorSet.ResetObjDb(1280, 1024, 0); // 这个方法为全局方法，后面的设置会赋盖前面的设置 ：这个算子会影响图像视图的大小，其大小等于
        }

        public void setViewParam(DrawingBaseMeasure previousDrawingObject)
        {
            this.initImageWidth = previousDrawingObject.InitImageWidth;
            this.initImageHeight = previousDrawingObject.InitImageHeight;
            this.initWindowWidth = previousDrawingObject.InitWindowWidth;
            this.initWindowHeight = previousDrawingObject.InitWindowHeight;
            this.imagePartRow1 = previousDrawingObject.ImagePartRow1;
            this.imagePartCol1 = previousDrawingObject.ImagePartCol1;
            this.imagePartRow2 = previousDrawingObject.ImagePartRow2;
            this.imagePartCol2 = previousDrawingObject.ImagePartCol2;
            if (this.bufferWindow != null)
                this.bufferWindow.SetPart(this.imagePartRow1, this.imagePartCol1, this.imagePartRow2, this.imagePartCol2);   // 设置窗口的图像部分与图像的大小相等 
        }
        public virtual void SetParam(object param)
        {
            throw new NotImplementedException();
        }
        protected virtual void addContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(new ToolStripItem[4] { new ToolStripMenuItem("自适应图像"), new ToolStripMenuItem("平移/缩放"), new ToolStripMenuItem("选择"), new ToolStripMenuItem("清空") }); //, new ToolStripMenuItem("编辑绘图位置")
            contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(toolStripContextMenuStrip_ItemClicked);
            this.hWindowControl.ContextMenuStrip = contextMenuStrip;
        }
        protected virtual void toolStripContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                switch (name)
                {
                    case "自适应图像":
                        this.isTranslate = false;
                        //this.isDrwingObject = true;
                        this.AutoImage();
                        break;

                    //////////////////////////////////////
                    case "平移/缩放":
                        this.isTranslate = true;
                        //this.isDrwingObject = true;
                        break;

                    case "选择":
                        this.isTranslate = false;
                        //this.isDrwingObject = true;
                        break;

                    case "清除窗口":
                        this.hWindowControl.HalconWindow.ClearWindow();
                        this.BackImage = null;
                        this.AttachPropertyData.Clear();
                        this.attachDrawingPropertyData.Clear();
                        break;

                    case "清空":
                        this.AttachPropertyData.Clear();
                        this.attachDrawingPropertyData.Clear();
                        this.AutoImage();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DrawingBaseMeasureClass->toolStripContextMenuStrip_ItemClicked:方法报错", ex);
            }
        }
        protected void SetImagePartSize(int imageWidth, int imageHeight, bool isSetImagePart)
        {
            int partWidth, partHeight;
            double scale = this.hWindowControl.Width * 1.0 / this.hWindowControl.Height;
            if (imageWidth != this.initImageWidth || imageHeight != this.initImageHeight || this.initWindowWidth != this.hWindowControl.Width || this.initWindowHeight != this.hWindowControl.Height || isSetImagePart)// 当某一个条件变化时，即更新视图
            {
                if (this.initImageWidth != imageWidth || this.initImageHeight != imageHeight)
                {
                    this.initImageWidth = imageWidth; // 只有当不相等时才更新初始值
                    this.initImageHeight = imageHeight;
                }
                if (this.initWindowWidth != this.hWindowControl.Width || this.initWindowHeight != this.hWindowControl.Height)
                {
                    this.initWindowWidth = this.hWindowControl.Width; // 只有当不相等时才更新初始值
                    this.initWindowHeight = this.hWindowControl.Height;
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (scale >= 1)
                {
                    partHeight = this.hWindowControl.Height > imageHeight ? this.hWindowControl.Height : imageHeight; //以高度作为参考基准 imageHeight; //
                    partWidth = (int)(partHeight * scale) > imageWidth ? (int)(partHeight * scale) : imageWidth; //
                    if ((int)(partHeight * scale) < imageWidth)
                        partHeight = (int)(partWidth / scale);
                }
                else
                {
                    partWidth = this.hWindowControl.Width > imageWidth ? this.hWindowControl.Width : imageWidth; // 以宽度作为参考基准  imageWidth; // 
                    partHeight = (int)(partWidth / scale) > imageHeight ? (int)(partWidth / scale) : imageHeight;
                    if ((int)(partWidth / scale) < imageHeight)
                        partWidth = (int)(partHeight * scale);
                }
                int row, col;
                col = this.initImageWidth < partWidth ? (int)((partWidth - this.initImageWidth) * -0.5) : 0;
                row = this.initImageHeight < partHeight ? (int)((partHeight - this.initImageHeight) * -0.5) : 0;
                //////////////////////
                if (this.bufferWindow != null)
                {
                    this.imagePartRow1 = row;
                    this.imagePartCol1 = col;
                    this.imagePartRow2 = partHeight - 1 + row;
                    this.imagePartCol2 = partWidth - 1 + col;
                    this.bufferWindow.SetPart(row, col, partHeight - 1 + row, partWidth - 1 + col); // 设置窗口的图像部分与图像的大小相等 
                }

            }
        }

        /// <summary>
        /// 这个函数的作用是使用绶冲区的窗口与传入的HWindowControl控件尺寸相一致，这样才有利于不同窗口间的内容复制
        /// </summary>
        /// <param name="hWindowControl"></param>
        protected void initBufferWindow(HWindowControl hWindowControl)
        {
            try
            {
                if (this.bufferWindow != null && this.bufferWindow.IsInitialized())
                    this.bufferWindow.CloseWindow();
                this.bufferWindow = new HWindow(0, 0, this.hWindowControl.Width, this.hWindowControl.Height, 0, "buffer", "");
                // 设置窗口参数
                this.bufferWindow.SetColor("red");
                this.bufferWindow.SetLineWidth(2);
                this.bufferWindow.SetPart(0, 0, this.hWindowControl.Height - 1, this.hWindowControl.Width - 1); //这里不要设置 ，，默认的视图大小设置为控件大小,设置初始视图大小
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("initBufferWindow", ex);
            }
        }
        protected void GetImagePart(out int imagePartRow1, out int imagePartCol1, out int imagePartRow2, out int imagePartCol2)
        {
            if (this.bufferWindow != null && this.bufferWindow.IsInitialized())
                this.bufferWindow.GetPart(out imagePartRow1, out imagePartCol1, out imagePartRow2, out imagePartCol2);
            else
            {
                imagePartRow1 = 0;
                imagePartCol1 = 0;
                imagePartRow2 = 0;
                imagePartCol2 = 0;
            }
        }
        protected HXLDCont CreateRectSizableNode(double x, double y)
        {
            HXLDCont rect = new HXLDCont();
            if (x != 0 && y != 0)
                rect.GenRectangle2ContourXld(y, x, 0, this.nodeSizeRect, this.nodeSizeRect);
            return rect;
        }
        protected HXLDCont CreateCircleSizableNode(double x, double y)
        {
            HXLDCont rect = new HXLDCont();
            if (x != 0 && y != 0)
                rect.GenCircleContourXld(y, x, this.nodeSizeRect, 0, 6.28, "positive", 0.01);
            return rect;
        }
        protected void MoveImage(int Tx, int Ty)
        {
            try
            {
                if (this.bufferWindow.Handle.ToInt64() == 0) return;
                //移动的本质是改变imagePart的位置坐标
                this.bufferWindow.GetPart(out imagePartRow1, out imagePartCol1, out imagePartRow2, out imagePartCol2);
                imagePartRow1 -= Ty;
                imagePartCol1 -= Tx;
                imagePartRow2 -= Ty;
                imagePartCol2 -= Tx;
                this.bufferWindow.SetPart(imagePartRow1, imagePartCol1, imagePartRow2, imagePartCol2);
                /////////
                this.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DrawingBaseMeasureClass->MoveImage:方法报错", ex);
            }
        }
        protected void ZoomImage(double currentRow, double currentCol, double scale)
        {
            //int imagePartRow1, imagePartCol1, imagePartRow2, imagePartCol2;
            int lengthC, lengthR;//
            double percentC, percentR;
            try
            {
                if (this.bufferWindow.Handle.ToInt64() == 0) return;
                ////////////////////////////////
                this.bufferWindow.GetPart(out imagePartRow1, out imagePartCol1, out imagePartRow2, out imagePartCol2);
                //鼠标位置相对于图像部分的百分比
                percentC = (currentCol - imagePartCol1) * 1.0 / (imagePartCol2 - imagePartCol1) * 1.0;
                percentR = (currentRow - imagePartRow1) * 1.0 / (imagePartRow2 - imagePartRow1) * 1.0;
                //缩放后的ImagePart宽和高
                lengthC = Convert.ToInt32((imagePartCol2 - imagePartCol1) * scale);
                lengthR = Convert.ToInt32((imagePartRow2 - imagePartRow1) * scale);
                //缩放前的鼠标坐标减去缩放后的鼠标坐标即可以imagepart的起点坐标
                imagePartCol1 = (int)currentCol - Convert.ToInt32(lengthC * percentC);
                imagePartRow1 = (int)currentRow - Convert.ToInt32(lengthR * percentR);
                imagePartCol2 = (int)currentCol + Convert.ToInt32(lengthC * (1 - percentC));
                imagePartRow2 = (int)currentRow + Convert.ToInt32(lengthR * (1 - percentR));
                //////////////////////////////////////////
                this.bufferWindow.SetPart(imagePartRow1, imagePartCol1, imagePartRow2, imagePartCol2);
                /////////
                //DetachDrawingObjectFromWindow();
                this.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DrawingBaseMeasureClass->ZoomImage:方法报错", ex);
            }
        }
        protected HXLDCont GenArrowContourXld(HTuple Row1, HTuple Column1, HTuple Row2, HTuple Column2, double HeadLength, double HeadWidth)
        {
            if (Row1.Length != Row2.Length) return new HXLDCont();
            HXLDCont arrows = new HXLDCont();
            arrows.GenEmptyObj();
            HTuple Length = HMisc.DistancePp(Row1, Column1, Row2, Column2);
            HTuple ZeroLengthIndices = Length.TupleFind(0);
            if (ZeroLengthIndices != -1)
                Length[ZeroLengthIndices] = -1;
            // Calculate auxiliary variables.
            HTuple DR = 1.0 * (Row2 - Row1) / Length;
            HTuple DC = 1.0 * (Column2 - Column1) / Length;
            HTuple HalfHeadWidth = HeadWidth / 2.0;
            // Calculate end points of the arrow head.
            HTuple RowP1 = Row1 + (Length - HeadLength) * DR + HalfHeadWidth * DC;
            HTuple ColP1 = Column1 + (Length - HeadLength) * DC - HalfHeadWidth * DR;
            HTuple RowP2 = Row1 + (Length - HeadLength) * DR - HalfHeadWidth * DC;
            HTuple ColP2 = Column1 + (Length - HeadLength) * DC + HalfHeadWidth * DR;
            // Finally create output XLD contour for each input point pair
            for (int Index = 0; Index < Length.Length; Index++)
            {
                if (Length[Index].D == -1)
                    //Create_ single points for arrows with identical start and end point
                    arrows = arrows.ConcatObj(new HXLDCont(Row1[Index], Column1[Index]));
                else
                    // Create arrow contour
                    arrows = arrows.ConcatObj(new HXLDCont(new HTuple(Row1[Index].D, Row2[Index].D, RowP1[Index].D, Row2[Index].D, RowP2[Index].D, Row2[Index].D), new HTuple(Column1[Index].D, Column2[Index].D, ColP1[Index].D, Column2[Index].D, ColP2[Index].D, Column2[Index].D)));
            }
            return arrows;
        }
        public virtual void DrawingGraphicObject()
        {
            try
            {
                /////////////////////////////
                this.bufferWindow.ClearWindow();
                if (this.backImage != null)
                {
                    // 这里为什么必需要重设一次图像部分大小？,在图形窗口中的参数，为什么在这里面起作用？&& !this.IsTranslate
                    if (this.backImage.Image != null && this.backImage.Image.IsInitialized()) //
                        this.bufferWindow.DispObj(this.backImage.Image);
                }
                // 显示传入的对象
                ShowAttachProperty(); // 因为显示3D对象使用了线程，所以这里只能放到方法时面
                // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
                int row1, col1, row2, col2;
                //int intprt = this.bufferWindow.Handle.ToInt64();
                this.bufferWindow.GetPart(out row1, out col1, out row2, out col2);
                this.hWindowControl.ImagePart = new Rectangle(col1, row1, col2 - col1, row2 - row1); // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
                //this.hWindowControl.HalconWindow.SetPart(row1, col1, row2, col2); // 目标窗口的图像部分要与buffer窗口的图像部分相同
                this.bufferWindow.CopyRectangle(this.hWindowControl.HalconWindow, 0, 0, this.hWindowControl.Height, this.hWindowControl.Width, 0, 0);
                // 调节点大小
                this.nodeSizeRect = Math.Min(col2 - col1, row2 - row1) * 0.01 < 1 ? 1 : Math.Min(col2 - col1, row2 - row1) * 0.01;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DrawingBaseMeasureClass->DrawingGraphicObject:方法报错", ex);
            }
        }
        private void ShowAttachProperty()
        {
            try
            {
                if (isDispalyAttachDrawingProperty)
                {
                    // 显示绘图对象
                    for (int i = 0; i < this.attachDrawingPropertyData.Count; i++)
                    {
                        this.bufferWindow.SetColor("red");
                        this.bufferWindow.SetLineWidth(2);
                        if (this.attachDrawingPropertyData[i] == null) continue;
                        switch (this.attachDrawingPropertyData[i].GetType().Name)
                        {
                            case "HImage":
                            case "HRegion":
                            case "HObject":
                                if (((HObject)this.attachDrawingPropertyData[i]).IsInitialized()) //这里要注意指针地址的益处
                                    this.bufferWindow.DispObj((HObject)this.attachDrawingPropertyData[i]);
                                break;
                            case "HXLDCont": // 用于显示绘图对象
                                if (((HXLDCont)this.attachDrawingPropertyData[i]) != null && ((HXLDCont)this.attachDrawingPropertyData[i]).IsInitialized())
                                    this.bufferWindow.DispObj((HXLDCont)this.attachDrawingPropertyData[i]);
                                break;
                            case "userPixRect2":
                                this.bufferWindow.SetColor("red");
                                this.bufferWindow.DispObj(((drawPixRect2)this.attachDrawingPropertyData[i]).GetXLD());
                                break;
                        }
                    }
                }
                if (this.isDispalyAttachEdgesProperty)
                {
                    this.bufferWindow.SetLineWidth(1);
                    this.bufferWindow.SetColor("green");
                    ////////// 显示测量边缘轮廓 
                    for (int i = 0; i < this._attachPropertyData.Count; i++)
                    {
                        if (this._attachPropertyData[i] == null) continue;
                        switch (this._attachPropertyData[i].GetType().Name)
                        {
                            case "HImage":
                                if (((HImage)this._attachPropertyData[i]).IsInitialized()) //这里要注意指针地址的益处
                                    this.bufferWindow.DispObj(((HImage)this._attachPropertyData[i]));
                                break;
                            case "HRegion":
                                this.bufferWindow.SetColored(12);
                                if (((HRegion)this._attachPropertyData[i]).IsInitialized()) //这里要注意指针地址的益处
                                    this.bufferWindow.DispObj(((HRegion)this._attachPropertyData[i]));
                                break;
                            case "HObject":
                                this.bufferWindow.SetColored(12);
                                if (((HObject)this._attachPropertyData[i]).IsInitialized()) //这里要注意指针地址的益处
                                    this.bufferWindow.DispObj((HObject)this._attachPropertyData[i]);
                                break;
                            case nameof(XldDataClass): // 用于显示绘图对象
                                if (((XldDataClass)this._attachPropertyData[i]).HXldCont != null
                                    && ((XldDataClass)this._attachPropertyData[i]).HXldCont.IsInitialized())
                                {
                                    this.bufferWindow.SetColor("red");
                                    this.bufferWindow.DispObj(((XldDataClass)this._attachPropertyData[i]).HXldCont);
                                }
                                break;
                            case nameof(RegionDataClass): // 用于显示绘图对象
                                this.bufferWindow.SetColored(12);
                                this.bufferWindow.SetDraw(((RegionDataClass)this._attachPropertyData[i]).Draw);
                                if (((RegionDataClass)this._attachPropertyData[i]).Region != null && ((RegionDataClass)this._attachPropertyData[i]).Region.IsInitialized())
                                    this.bufferWindow.DispObj(((RegionDataClass)this._attachPropertyData[i]).Region);
                                break;
                            case "userWcsRectangle2":
                                userWcsRectangle2 wcsRect2 = (userWcsRectangle2)this._attachPropertyData[i];
                                wcsRect2.Grab_x = this.refPoint_x;
                                wcsRect2.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsRect2.Color.ToString());
                                this.bufferWindow.DispObj((wcsRect2).GetPixRectangle2().GetXLD());
                                break;

                            case "userWcsRectangle1":
                                userWcsRectangle1 wcsRect1 = (userWcsRectangle1)this._attachPropertyData[i];
                                wcsRect1.Grab_x = this.refPoint_x;
                                wcsRect1.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsRect1.Color.ToString());
                                this.bufferWindow.DispObj((wcsRect1).GetPixRectangle1().GetXLD());
                                break;

                            case "userWcsPoint":
                                userWcsPoint wcsPoint = (userWcsPoint)this._attachPropertyData[i];
                                if (wcsPoint == null) return;
                                wcsPoint.Grab_x = this.refPoint_x;
                                wcsPoint.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsPoint.Color.ToString());
                                this.bufferWindow.DispObj((wcsPoint).GetPixPoint().GetXLD(this.nodeSizeRect));
                                break;

                            case "userWcsLine":
                                userWcsLine wcsLine = (userWcsLine)this._attachPropertyData[i];
                                wcsLine.Grab_x = this.refPoint_x;
                                wcsLine.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsLine.Color.ToString());
                                this.bufferWindow.DispObj((wcsLine).GetPixLine().GetXLD());
                                break;

                            case "userWcsCircle":
                                userWcsCircle wcsCircle = (userWcsCircle)this._attachPropertyData[i];
                                wcsCircle.Grab_x = this.refPoint_x;  // 必需要使用当前的图像参考来更新对象的参考点位置，不然图形不会跟随移动,当计量边缘不与图像边缘重合时，是因为机台的补偿出问题了，机台坐标与补偿后的机台坐标相差较大
                                wcsCircle.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsCircle.Color.ToString());
                                this.bufferWindow.DispObj((wcsCircle).GetPixCircle().GetXLD());
                                break;

                            case "userWcsCircleSector":
                                userWcsCircleSector wcsCircleSector = (userWcsCircleSector)this._attachPropertyData[i];
                                wcsCircleSector.Grab_x = this.refPoint_x;
                                wcsCircleSector.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsCircleSector.Color.ToString());
                                this.bufferWindow.DispObj((wcsCircleSector).GetPixCircleSector().GetXLD());
                                break;

                            case "userWcsEllipse":
                                userWcsEllipse wcsEllipse = (userWcsEllipse)this._attachPropertyData[i];
                                wcsEllipse.Grab_x = this.refPoint_x;
                                wcsEllipse.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsEllipse.Color.ToString());
                                this.bufferWindow.DispObj((wcsEllipse).GetPixEllipse().GetXLD());
                                break;

                            case "userWcsEllipseSector":
                                userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)this._attachPropertyData[i];
                                wcsEllipseSector.Grab_x = this.refPoint_x;
                                wcsEllipseSector.Grab_y = this.refPoint_y;
                                this.bufferWindow.SetColor(wcsEllipseSector.Color.ToString());
                                this.bufferWindow.DispObj((wcsEllipseSector).GetPixEllipseSector().GetXLD());
                                break;

                            case "HXLDCont":
                                //this.bufferWindow.SetColor(item.color.ToString());
                                this.bufferWindow.SetColor("green");
                                if(this._attachPropertyData[i] != null && ((HXLDCont)this._attachPropertyData[i]).IsInitialized())
                                this.bufferWindow.DispObj((HXLDCont)this._attachPropertyData[i]);
                                break;

                            case "userPixPoint":
                                this.bufferWindow.SetColor(((userPixPoint)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixPoint)this._attachPropertyData[i]).GetXLD(this.nodeSizeRect * 2));
                                break;

                            case "userPixLine":
                                this.bufferWindow.SetColor(((userPixLine)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixLine)this._attachPropertyData[i]).GetXLD());
                                break;

                            case "userPixCircle":
                                this.bufferWindow.SetColor(((userPixCircle)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixCircle)this._attachPropertyData[i]).GetXLD());
                                break;

                            case "userPixCircleSector":
                                this.bufferWindow.SetColor(((userPixCircleSector)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixCircleSector)this._attachPropertyData[i]).GetXLD());
                                break;

                            case "userPixEllipse":
                                this.bufferWindow.SetColor(((userPixEllipse)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixEllipse)this._attachPropertyData[i]).GetXLD());
                                break;

                            case "userPixEllipseSector":
                                this.bufferWindow.SetColor(((userPixEllipseSector)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixEllipseSector)this._attachPropertyData[i]).GetXLD());
                                break;

                            case "userPixRectangle1":
                                this.bufferWindow.SetColor(((userPixRectangle1)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixRectangle1)this._attachPropertyData[i]).GetXLD());
                                break;

                            case "userPixRectangle2":
                                this.bufferWindow.SetColor(((userPixRectangle2)this._attachPropertyData[i]).Color.ToString());
                                this.bufferWindow.DispObj(((userPixRectangle2)this._attachPropertyData[i]).GetXLD());
                                break;

                            case "userPixRect2":
                                this.bufferWindow.SetColor("red");
                                this.bufferWindow.DispObj(((drawPixRect2)this._attachPropertyData[i]).GetXLD());
                                break;
                            case "userOkNgText":
                                int row, col, row2, col2, width, height;
                                this.bufferWindow.GetPart(out row, out col, out row2, out col2);
                                this.bufferWindow.GetWindowExtents(out row, out col, out width, out height);
                                userOkNgText OkNGText = (userOkNgText)this._attachPropertyData[i];
                                if (OkNGText.row_pos == 0 && OkNGText.col_pos == 0)
                                    (OkNGText).WriteString(this.bufferWindow, row + 10, col2 - 180, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角
                                else
                                    (OkNGText).WriteString(this.bufferWindow, OkNGText.row_pos, OkNGText.col_pos, GlobalVariable.pConfig.OKNgSize); // 原点放在左上角
                                break;
                            case "userTextLable":
                                userTextLable TextLable = (userTextLable)this._attachPropertyData[i];
                                TextLable.CamParam = this.cameraParam;
                                //TextLable.camPose = this.camPose;
                                TextLable.WriteString(this.bufferWindow); // 字体放在右上角,文本的参考角为左上角
                                break;
                            case nameof(userWcsCoordSystem):
                                HTuple row22 = new HTuple(), col22 = new HTuple();
                                userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)this._attachPropertyData[i];
                                int minLength = this.initImageWidth > this.InitImageHeight ? this.InitImageHeight : this.initImageWidth;
                                this.bufferWindow.SetColor("red");
                                this.bufferWindow.DispObj(wcsCoordSystem.getAxis_x_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                                wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D + (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "X", "red");
                                this.bufferWindow.SetColor("yellow");
                                this.bufferWindow.DispObj(wcsCoordSystem.getAxis_y_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                                wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D - (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "Y", "yellow");
                                break;
                            case nameof(ViewData):
                                ViewData viewObject = this._attachPropertyData[i] as ViewData;
                                if (viewObject == null || viewObject.DataObject == null) continue;
                                switch (viewObject.DataObject.GetType().Name)
                                {
                                    case nameof(HImage):
                                        if (((HImage)viewObject.DataObject) == null || !((HImage)viewObject.DataObject).IsInitialized()) continue;
                                        this.bufferWindow.SetColor(viewObject.Color);
                                        this.bufferWindow.DispObj(((HImage)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                                        break;
                                    case nameof(HObject):
                                        if (((HObject)viewObject.DataObject) == null || !((HObject)viewObject.DataObject).IsInitialized()) continue;
                                        this.bufferWindow.SetColor(viewObject.Color);
                                        this.bufferWindow.DispObj(((HObject)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                                        break;
                                    case nameof(HRegion):
                                        if (((HRegion)viewObject.DataObject) == null || !((HRegion)viewObject.DataObject).IsInitialized()) continue;
                                        int result = 0;
                                        if (int.TryParse(viewObject.Color, out result))
                                            this.bufferWindow.SetColored(result); // 区域显示一定要使用红色
                                        else
                                            this.bufferWindow.SetColor(viewObject.Color); // 区域显示一定要使用红色
                                        this.bufferWindow.SetDraw(((ViewData)viewObject).Draw);
                                        this.bufferWindow.DispObj(((HRegion)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                                        break;
                                    case nameof(HXLDCont):
                                        if (((HXLDCont)viewObject.DataObject) == null || !((HXLDCont)viewObject.DataObject).IsInitialized()) continue;
                                        this.bufferWindow.SetColor(viewObject.Color);
                                        this.bufferWindow.DispObj(((HXLDCont)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                                        break;
                                    case nameof(HXLD):
                                        if (((HXLD)viewObject.DataObject) == null || !((HXLD)viewObject.DataObject).IsInitialized()) continue;
                                        this.bufferWindow.SetColor(viewObject.Color);
                                        this.bufferWindow.DispObj(((HXLD)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                                        break;
                                    case nameof(userOkNgText):
                                        int row222, col222;
                                        this.bufferWindow.GetPart(out row, out col, out row222, out col222);
                                        OkNGText = (userOkNgText)viewObject.DataObject;
                                        switch (GlobalVariable.pConfig.OKNgPosition)
                                        {
                                            case enFontPosition.左上角:
                                                (OkNGText).WriteString(this.bufferWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                                break;
                                            case enFontPosition.右上角:
                                                (OkNGText).WriteString(this.bufferWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                                break;
                                            case enFontPosition.左下角:
                                                (OkNGText).WriteString(this.bufferWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                                break;
                                            case enFontPosition.右下角:
                                                (OkNGText).WriteString(this.bufferWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                                break;
                                        }
                                        break;
                                    case nameof(userTextLable):
                                        TextLable = (userTextLable)viewObject.DataObject;
                                        TextLable.CamParam = this.cameraParam;
                                        TextLable.WriteString(this.bufferWindow); // 字体放在右上角,文本的参考角为左上角
                                        break;
                                    case nameof(userWcsCoordSystem):
                                        row22 = new HTuple();
                                        col22 = new HTuple();
                                        wcsCoordSystem = (userWcsCoordSystem)viewObject.DataObject;
                                        minLength = this.initImageWidth > this.InitImageHeight ? this.InitImageHeight : this.initImageWidth;
                                        this.bufferWindow.SetColor("red");
                                        this.bufferWindow.DispObj(wcsCoordSystem.getAxis_x_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5/ this.nodeSizeRect), out row22, out col22));
                                        wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D + (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50/this.nodeSizeRect), "X", "red");
                                        this.bufferWindow.SetColor("yellow");
                                        this.bufferWindow.DispObj(wcsCoordSystem.getAxis_y_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                                        wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D - (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "Y", "yellow");
                                        break;
                                    case nameof(userPixPoint):
                                        userPixPoint pixPoint = (userPixPoint)viewObject.DataObject;
                                        pixPoint.Size = this.nodeSizeRect;
                                        this.bufferWindow.SetColor(pixPoint.Color.ToString());
                                        this.bufferWindow.DispObj(pixPoint.GetXLD(this.nodeSizeRect));
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("ShowAttachProperty->显示附加属性方法报错", ex);
            }
        }
        public void AddViewObject(object viewObject, string color)
        {
            if (viewObject == null) return;
            this.isDispalyAttachDrawingProperty = false;
            switch (viewObject.GetType().Name)
            {
                case nameof(HImage):
                    if (!((HImage)viewObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(color);
                    this.bufferWindow.DispObj(((HImage)viewObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HObject):
                    if (!((HObject)viewObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(color);
                    this.bufferWindow.DispObj(((HObject)viewObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HRegion):
                    if (!((HRegion)viewObject).IsInitialized()) return;
                    this.bufferWindow.SetColor("red"); // 区域显示一定要使用红色
                    this.bufferWindow.SetColored(12); // 区域显示一定要使用红色
                    this.bufferWindow.SetDraw("margin");
                    this.bufferWindow.DispObj(((HRegion)viewObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HXLDCont):
                    if (!((HXLDCont)viewObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(color);
                    this.bufferWindow.DispObj(((HXLDCont)viewObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HXLD):
                    if (!((HXLD)viewObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(color);
                    this.bufferWindow.DispObj(((HXLD)viewObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(userOkNgText):
                    int row, col, row222, col222;
                    this.bufferWindow.GetPart(out row, out col, out row222, out col222);
                    userOkNgText OkNGText = (userOkNgText)viewObject;
                    switch (GlobalVariable.pConfig.OKNgPosition)
                    {
                        case enFontPosition.左上角:
                            (OkNGText).WriteString(this.bufferWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                        case enFontPosition.右上角:
                            (OkNGText).WriteString(this.bufferWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                        case enFontPosition.左下角:
                            (OkNGText).WriteString(this.bufferWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                        case enFontPosition.右下角:
                            (OkNGText).WriteString(this.bufferWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                    }
                    break;
                case nameof(userTextLable):
                    userTextLable TextLable = (userTextLable)viewObject;
                    TextLable.CamParam = this.cameraParam;
                    TextLable.WriteString(this.bufferWindow); // 字体放在右上角,文本的参考角为左上角
                    break;
                case nameof(userWcsCoordSystem):
                    userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)viewObject;
                    HTuple row22, col22;
                    int minLength = this.initImageWidth > this.InitImageHeight ? this.InitImageHeight : this.initImageWidth;
                    this.bufferWindow.SetColor("red");
                    this.bufferWindow.DispObj(wcsCoordSystem.getAxis_x_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                    wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D + (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "X", "red");
                    this.bufferWindow.SetColor("yellow");
                    this.bufferWindow.DispObj(wcsCoordSystem.getAxis_y_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                    wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D - (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "Y", "yellow");
                    break;
                case nameof(userPixPoint):
                    userPixPoint pixPoint = (userPixPoint)viewObject;
                    pixPoint.Size = this.nodeSizeRect;
                    this.bufferWindow.SetColor(pixPoint.Color.ToString());
                    this.bufferWindow.DispObj(pixPoint.GetXLD(this.nodeSizeRect));
                    break;
            }
            this.AttachPropertyData.Add(viewObject);
            // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            int row1, col1, row2, col2;
            this.bufferWindow.GetPart(out row1, out col1, out row2, out col2); // 只有源窗口与目标窗口的图像部分相一致，这样在不同的窗口中显示才相同
            this.hWindowControl.ImagePart = new Rectangle(col1, row1, col2 - col1, row2 - row1); // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            //this.hWindowControl.HalconWindow.SetPart(row1, col1, row2, col2); // 目标窗口的图像部分要与buffer窗口的图像部分相同
            this.bufferWindow.CopyRectangle(this.hWindowControl.HalconWindow, 0, 0, this.hWindowControl.Height, this.hWindowControl.Width, 0, 0);
        }
        public void CopyBufferWindowView()
        {
            // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            int row1, col1, row2, col2;
            this.bufferWindow.GetPart(out row1, out col1, out row2, out col2); // 只有源窗口与目标窗口的图像部分相一致，这样在不同的窗口中显示才相同
            this.hWindowControl.ImagePart = new Rectangle(col1, row1, col2 - col1, row2 - row1); // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            //this.hWindowControl.HalconWindow.SetPart(row1, col1, row2, col2); // 目标窗口的图像部分要与buffer窗口的图像部分相同
            this.bufferWindow.CopyRectangle(this.hWindowControl.HalconWindow, 0, 0, this.hWindowControl.Height, this.hWindowControl.Width, 0, 0);
        }

        public void AddViewObject(ViewData viewObject)
        {
            if (viewObject == null || viewObject.DataObject == null) return;
            switch (viewObject.DataObject.GetType().Name)
            {
                case nameof(HImage):
                    if (!((HImage)viewObject.DataObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(viewObject.Color);
                    this.bufferWindow.DispObj(((HImage)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HObject):
                    if (!((HObject)viewObject.DataObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(viewObject.Color);
                    this.bufferWindow.DispObj(((HObject)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HRegion):
                    if (!((HRegion)viewObject.DataObject).IsInitialized()) return;
                    int result = 0;
                    if (int.TryParse(viewObject.Color, out result))
                        this.bufferWindow.SetColored(result); // 区域显示一定要使用红色
                    else
                        this.bufferWindow.SetColor(viewObject.Color); // 区域显示一定要使用红色
                    this.bufferWindow.SetDraw(viewObject.Draw);
                    this.bufferWindow.DispObj(((HRegion)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HXLDCont):
                    if (!((HXLDCont)viewObject.DataObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(viewObject.Color);
                    this.bufferWindow.DispObj(((HXLDCont)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HXLD):
                    if (!((HXLD)viewObject.DataObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(viewObject.Color);
                    this.bufferWindow.DispObj(((HXLD)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(userOkNgText):
                    int row, col, row222, col222;
                    this.bufferWindow.GetPart(out row, out col, out row222, out col222);
                    userOkNgText OkNGText = (userOkNgText)viewObject.DataObject;
                    switch (GlobalVariable.pConfig.OKNgPosition)
                    {
                        case enFontPosition.左上角:
                            (OkNGText).WriteString(this.bufferWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                        case enFontPosition.右上角:
                            (OkNGText).WriteString(this.bufferWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                        case enFontPosition.左下角:
                            (OkNGText).WriteString(this.bufferWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                        case enFontPosition.右下角:
                            (OkNGText).WriteString(this.bufferWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                            break;
                    }
                    break;
                case nameof(userTextLable):
                    userTextLable TextLable = (userTextLable)viewObject.DataObject;
                    TextLable.CamParam = this.cameraParam;
                    TextLable.Size = (int)this.nodeSizeRect;
                    TextLable.WriteString(this.bufferWindow); // 字体放在右上角,文本的参考角为左上角
                    break;
                case nameof(userWcsCoordSystem):
                    userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)viewObject.DataObject;
                    HTuple row22, col22;
                    int minLength = this.initImageWidth > this.initWindowHeight ? this.initWindowHeight : this.initImageWidth;
                    this.bufferWindow.SetColor("red");
                    this.bufferWindow.DispObj(wcsCoordSystem.getAxis_x_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                    wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D + (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "X", "red");
                    this.bufferWindow.SetColor("yellow");
                    this.bufferWindow.DispObj(wcsCoordSystem.getAxis_y_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                    wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D - (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "Y", "yellow");
                    break;
                case nameof(userPixPoint):
                    userPixPoint pixPoint = (userPixPoint)viewObject.DataObject;
                    pixPoint.Size = this.nodeSizeRect;
                    this.bufferWindow.SetColor(pixPoint.Color.ToString());
                    this.bufferWindow.DispObj(pixPoint.GetXLD(this.nodeSizeRect));
                    break;
            }
            this._attachPropertyData.Add(viewObject);
            // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            int row1, col1, row2, col2;
            this.bufferWindow.GetPart(out row1, out col1, out row2, out col2); // 只有源窗口与目标窗口的图像部分相一致，这样在不同的窗口中显示才相同
            this.hWindowControl.ImagePart = new Rectangle(col1, row1, col2 - col1, row2 - row1); // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            //this.hWindowControl.HalconWindow.SetPart(row1, col1, row2, col2); // 目标窗口的图像部分要与buffer窗口的图像部分相同
            this.bufferWindow.CopyRectangle(this.hWindowControl.HalconWindow, 0, 0, this.hWindowControl.Height, this.hWindowControl.Width, 0, 0);
        }
        public void RemoveViewObjectAt(int index)
        {
            if (this._attachPropertyData.Count > index)
                this._attachPropertyData.RemoveAt(index);
            this.DrawingGraphicObject();
        }
        public void RemoveViewObject(ViewData viewObject)
        {
            if (this.AttachDrawingPropertyData.Contains(viewObject))
                this._attachPropertyData.Remove(viewObject);
            this.DrawingGraphicObject();
        }
        public void ClearViewObject()
        {
            this._attachPropertyData.Clear();
            this.DrawingGraphicObject();
        }
        public void ChangeColorViewObject(ViewData viewObject, string color)
        {
            foreach (var item in this._attachPropertyData)
            {
                if (item.Equals(viewObject))
                {
                    ((ViewData)item).Color = color;
                    break;
                }
            }
            this.DrawingGraphicObject();
        }


        // 鼠标操作都是公共操作部分，全部放到基类中,
        ///  用来触发双击事件
        public HMouseEventHandler HMouseDoubleClick;
        private TimeSpan firstTime = TimeSpan.Zero, secondTime = TimeSpan.Zero;
        protected virtual void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            /// 由于Halcon 中没有双击事件，所以这里通过代码的方式来创建一个双击事件
            if (firstTime == TimeSpan.Zero && secondTime == TimeSpan.Zero)
            {
                firstTime = DateTime.Now.TimeOfDay;
            }
            else
            {
                secondTime = DateTime.Now.TimeOfDay;
                TimeSpan diffSpan = secondTime - firstTime;
                firstTime = TimeSpan.Zero;
                secondTime = TimeSpan.Zero;
                if (diffSpan.TotalMilliseconds < 300) // 表示在 500ms 时间内触发了两次
                {
                    if (this.HMouseDoubleClick != null)
                        this.HMouseDoubleClick.Invoke(sender, e);
                }
            }
            //////////////
            mIsClick = true;
            oldX = e.X;
            oldY = e.Y;
        }
        protected virtual void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            mIsClick = false;
            if (this.IsTranslate)
            {
                if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) return;
                //this.isDrwingObject = true;
                this.MoveImage((int)(e.X - oldX), (int)(e.Y - oldY));
                DrawingGraphicObject();
            }
        }
        protected virtual void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (this.IsTranslate)
            {
                //this.isDrwingObject = true;
                if (e.Delta < 0)
                    this.ZoomImage(e.Y, e.X, 0.95);
                else
                    this.ZoomImage(e.Y, e.X, 1.05);
                DrawingGraphicObject();
            }
        }
        protected virtual void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                if (this.backImage != null && this.backImage.Image != null && this.backImage.Image.IsInitialized())
                {
                    if (e.X > 0 && e.X < this.initImageWidth && e.Y > 0 && e.Y < this.initImageHeight)
                        this.OnGaryValueInfo(new GrayValueInfoEventArgs(Math.Round(e.Y, 3), Math.Round(e.X, 3), this.backImage.Image.GetGrayval(e.Y, e.X)));
                    else
                        this.OnGaryValueInfo(new GrayValueInfoEventArgs(Math.Round(e.Y, 3), Math.Round(e.X, 3), new HTuple("-")));
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("获取灰度值出错" + ex.ToString());
            }
        }

        private void hWindowControl_SizeChanged(object sender, EventArgs e)
        {
            //bool lockTaken = false;
            try
            {
                //Monitor.TryEnter(this.monitor, ref lockTaken);
                initBufferWindow(this.hWindowControl);
                this.SetImagePartSize(this.initImageWidth, this.initImageHeight, true); // 窗口尺寸改变后也要重设置一次
                this.DrawingGraphicObject();
            }
            finally
            {
                //if (lockTaken)
                //    Monitor.Exit(this.monitor);
            }
        }
        public virtual void ClearWindow()
        {
            try
            {
                this.hWindowControl.HalconWindow.ClearWindow();
                this.AttachPropertyData.Clear();
                this.attachDrawingPropertyData.Clear();
                this.isDispalyAttachDrawingProperty = false;
                //this.backImage = null;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DrawingBaseMeasureClass->AutoImage:方法报错", ex);
            }
        }
        public virtual void AutoImage()
        {
            try
            {
                this.SetImagePartSize(this.initImageWidth, this.initImageHeight, true); // 自适应窗口时也要重置一次
                int row1, col1, row2, col2;
                this.bufferWindow.GetPart(out row1, out col1, out row2, out col2);
                this.nodeSizeRect = Math.Min(col2 - col1, row2 - row1) * 0.01 < 1 ? 1 : Math.Min(col2 - col1, row2 - row1) * 0.01;
                this.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DrawingBaseMeasureClass->AutoImage:方法报错", ex);
            }
        }
        public virtual void ClearDrawingObject()
        {
            this.isDrwingObject = false;
            if (this.isInitEvent)
            {
                this.isInitEvent = false;
                this.hWindowControl.HMouseDown -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseDown);
                this.hWindowControl.HMouseUp -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseUp);
                this.hWindowControl.HMouseWheel -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseWheel);
                this.hWindowControl.HMouseMove -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseMove);
                this.hWindowControl.SizeChanged -= new System.EventHandler(hWindowControl_SizeChanged);
            }
            this.attachDrawingPropertyData.Clear();
            this.backImage = null;
        }
        public virtual void DetachDrawingObjectFromWindow()
        {
            try
            {
                this.isTranslate = true;// 在非绘图模式下移动图像 
                this.isDispalyAttachDrawingProperty = false;
                this.isDrwingObject = false;
                this.bufferWindow.SetColor("green");
                this.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DrawingBaseMeasureClass->DetachDrawingObjectFromWindow:方法报错", ex);
            }
        }
        public virtual void AttachDrawingObjectToWindow()
        {
            ////////////////
            this.isTranslate = false;// 在绘图模式下不能移动图像 
            this.isDispalyAttachDrawingProperty = true;
            this.isDrwingObject = true;
            this.bufferWindow.SetColor("red");
            this.bufferWindow.SetLineWidth(2);
            DrawingGraphicObject();
        }

        public virtual userWcsLine GetWcsLineParam()
        {
            throw new NotImplementedException();
        }
        public virtual userPixLine GetPixLineParam()
        {
            throw new NotImplementedException();
        }
        public virtual userWcsCircle GetWcsCircleParam()
        {
            throw new NotImplementedException();
        }
        public virtual userPixCircle GetPixCircleParam()
        {
            throw new NotImplementedException();
        }
        public virtual userWcsCircleSector GetWcsCircleSectorParam()
        {
            throw new NotImplementedException();
        }
        public virtual userPixCircleSector GetPixCircleSectorParam()
        {
            throw new NotImplementedException();
        }
        public virtual userWcsEllipse GetWcsEllipseParam()
        {
            throw new NotImplementedException();
        }
        public virtual userPixEllipse GetPixEllipseParam()
        {
            throw new NotImplementedException();
        }
        public virtual userWcsEllipseSector GetWcsEllipseSectorParam()
        {
            throw new NotImplementedException();
        }
        public virtual userPixEllipseSector GetPixEllipseSectorParam()
        {
            throw new NotImplementedException();
        }
        public virtual userWcsRectangle2 GetWcsRectangle2Param()
        {
            throw new NotImplementedException();
        }
        public virtual userPixRectangle2 GetPixRectangle2Param()
        {
            throw new NotImplementedException();
        }
        public virtual userPixPolygon GetPixPolygonParam()
        {
            throw new NotImplementedException();
        }
        public virtual userPixPolyLine GetPixPolyLineParam()
        {
            throw new NotImplementedException();
        }



    }

    public enum enShowMode
    {
        箭头,
        矩形,
        NONE,
        箭头矩形,
    }
}
