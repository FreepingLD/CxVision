
using Common;
using HalconDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    [Serializable]
    public class VisualizeView
    {
        [NonSerialized]
        protected HWindowControl hWindowControl;
        [NonSerialized]
        protected HWindow bufferWindow;
        protected bool mIsClick = false;
        protected double nodeSizeRect = GlobalVariable.pConfig.NodeSize;
        protected int imageWidth = 5000; // 这里的图像宽高用于显示3D对象
        protected int imageHeight = 5000;
        // 这两个变量用于记录初始图像大小
        private int initImageWidth = 100;
        private int initImageHeight = 100;
        private int initWindowWidth = 0;
        private int initWindowHeight = 0;
        private CameraParam cameraParam = new CameraParam();
        private enLensType lensType = enLensType.远心镜头;
        private enVisualeViewType viewType = enVisualeViewType.模型视图;
        // 数据源属性
        protected PointCloudData pointCloudModel3D;
        //protected HObjectModel3D[] pointCloudModel3D;
        private ImageDataClass backImage;
        private RegionDataClass regionData;
        private XldDataClass xldContourData;
        protected double oldX;
        protected double oldY;
        protected bool isTranslate = true;
        protected bool isDrwingObject = false;
        protected bool isInitEvent = false;
        private List<object> attachDrawingPropertyData = new List<object>();
        private List<object> attachPropertyData = new List<object>();
        //private object monitor = new object();
        //private bool lockState;
        // 显示控制
        private bool isAttachProperty = true;
        private bool isDisplayModel3D = false;
        protected bool isDisplayHImageData = false;
        private bool isDisplayXldData = false;
        private bool isDisplayRegionData = false;
        private MouseButtons _currentButton = MouseButtons.None;
        protected bool isDrawMode = false;
        private enShowMode _ShowMode = enShowMode.NONE;
        public event GrayValueInfoEventHandler GrayValueInfo;

        /// <summary>
        /// 表示实时采集状态
        /// </summary>
        public bool IsLiveState { get; set; }

        #region 与视图相关的参数放到视类 
        private HTuple instructions_Rotate = "Rotate: Left button";
        private HTuple instructions_Zoom = "Zoom:   Shift + left button";
        private HTuple instructions_Move = "Move:   Ctrl  + left button";
        // Configuration lut
        private string paramName_quality = "quality";
        private string paramName_lut = "lut";
        private string paramName_dispPose = "disp_pose";
        private string paramName_alpha = "alpha";
        private string paramName_color_attrib = "color_attrib"; //  paramName_intensity
        private string paramName_color_attrib_start = "color_attrib_start"; //  paramName_intensity
        private string paramName_color_attrib_end = "color_attrib_end"; //  paramName_intensity
        private string paramName_depthPersistence = "depth_persistence";
        ///////////////////// color1
        private string paramValue_quality = GlobalVariable.pConfig.PointQuality;  //"low";
        private string paramValue_lut = GlobalVariable.pConfig.ViewType;
        private string paramValue_dispPose = GlobalVariable.pConfig.IsShowCoordSys.ToString(); //false
        private double paramValue_alpha = 0.9;
        private string paramValue_color_attrib = GlobalVariable.pConfig.ColorAttrib; //"coord_z"; // 对应强度变化的值  quality  paramValue_intensity
        private object paramValue_color_attrib_start = "auto"; //  paramName_intensity
        private object paramValue_color_attrib_end = "auto"; //  paramName_intensity
        private string paramValue_depthPersistence = GlobalVariable.pConfig.Depth_persistence.ToString(); //"false";
        ///////////////////// colored
        private string colored = "colored";
        private int colorNum = 12;
        // 用于可视化的信息 
        private HTuple Instructions;
        private HTuple GenParamName;
        private HTuple GenParamValue;

        #endregion

        public PointCloudData PointCloudModel3D
        {
            get
            {
                return pointCloudModel3D;
            }
            set
            {
                if (value == null) return;
                ///////////////////////
                this.pointCloudModel3D = value;
                this.isDisplayModel3D = true;
                this.isDisplayHImageData = false;
                this.isDisplayXldData = false;
                this.backImage = null;
                this.xldContourData = null;
                this.lensType = enLensType.远心镜头;
                this.viewType = enVisualeViewType.模型视图;
                ///////////////
                //lockState = Monitor.TryEnter(this.monitor);
                //if (!lockState) return;
                this.cameraParam.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
                //InitCamParam();
                //InitMagnification(pointCloudModel3D);
                //InitCamPose(pointCloudModel3D.ObjectModel3D);
                this.cameraParam.CamParam = this.pointCloudModel3D.LaserParams?.CamParam;
                this.cameraParam.CamPose = this.pointCloudModel3D.LaserParams?.CamPose;
                if (this.pointCloudModel3D.LaserParams.CamParam != null && this.pointCloudModel3D.LaserParams.CamPose != null)
                    SetImagePartSize((int)this.cameraParam.CamParam.Width, (int)this.cameraParam.CamParam.Height, true);
                DrawingGraphicObject();
                //if (lockState)
                //    Monitor.Exit(this.monitor);
            }
        }
        //public HObjectModel3D[] PointCloudModel3D
        //{
        //    get
        //    {
        //        return pointCloudModel3D;
        //    }
        //    set
        //    {
        //        if (value == null) return;
        //        ///////////////////////
        //        this.pointCloudModel3D = value;
        //        this.isDisplayModel3D = true;
        //        this.isDisplayHImageData = false;
        //        this.isDisplayXldData = false;
        //        this.backImage = null;
        //        this.xldContourData = null;
        //        this.lensType = enLensType.远心镜头;
        //        this.viewType = enVisualeViewType.模型视图;
        //        ///////////////
        //        lockState = Monitor.TryEnter(this.monitor);
        //        if (!lockState) return;
        //        this.cameraParam.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
        //        //InitCamParam();
        //        //InitMagnification(pointCloudModel3D);
        //        InitCamPose(pointCloudModel3D);
        //        SetImagePartSize((int)this.cameraParam.CamParam.Width, (int)this.cameraParam.CamParam.Height, true);
        //        DrawingGraphicObject();
        //        if (lockState)
        //            Monitor.Exit(this.monitor);
        //    }
        //}
        public ImageDataClass BackImage
        {
            get
            {
                return backImage;
            }
            set
            {
                if (value == null) return;
                this.backImage = value;
                this.isDisplayModel3D = false;
                this.isDisplayHImageData = true;
                this.isDisplayXldData = false;
                this.xldContourData = null;
                this.pointCloudModel3D = null;
                this.viewType = enVisualeViewType.图像视图;
                ////////////////////////////
                //lockState = Monitor.TryEnter(this.monitor);
                //if (!lockState) return;
                this.cameraParam = backImage.CamParams;
                if (IsLiveState)
                    this.SetImagePartSize(this.backImage.Width, this.backImage.Height, false);
                else
                    this.SetImagePartSize(this.backImage.Width, this.backImage.Height, true);
                this.DrawingGraphicObject();
                //if (lockState)
                //    Monitor.Exit(this.monitor);
            }
        }

        /// <summary>
        /// 用来表示在图像中显示XLD轮廓数据
        /// </summary>
        public XldDataClass XldContourData
        {
            get
            {
                return xldContourData;
            }
            set
            {
                if (value == null) return;
                xldContourData = value;
                isDisplayModel3D = false;
                isDisplayHImageData = true;
                isDisplayRegionData = false;
                isDisplayXldData = true;
                this.pointCloudModel3D = null;
                this.viewType = enVisualeViewType.XLD视图;
                /////////////////
                //lockState = Monitor.TryEnter(this.monitor);
                //getXldImagePart((xldContourData));
                DrawingGraphicObject();
                //if (lockState)
                //Monitor.Exit(this.monitor);
            }
        }
        public List<object> AttachPropertyData
        {
            get
            {
                return attachPropertyData;
            }

            set
            {
                attachPropertyData = value;
                //DrawingGraphicObject();
            }
        }
        public bool DisplayAttachProperty
        {
            get
            {
                return isAttachProperty;
            }

            set
            {
                isAttachProperty = value;
            }
        }

        public RegionDataClass RegionData
        {
            get => regionData;
            set
            {
                regionData = value;
                if (regionData == null) return;
                isDisplayModel3D = false;
                isDisplayHImageData = true;
                isDisplayXldData = false;
                isDisplayRegionData = true;
                this.pointCloudModel3D = null;
                //this.backImage = null;
                this.viewType = enVisualeViewType.Region视图;
                /////////////////
                //lockState = Monitor.TryEnter(this.monitor);
                getRegionImagePart((regionData));
                DrawingGraphicObject();
                //if (lockState)
                //Monitor.Exit(this.monitor);
            }
        }

        public CameraParam CameraParam { get => cameraParam; set => cameraParam = value; }
        protected bool IsTranslate { get => isTranslate; set => isTranslate = value; }
        public enShowMode ShowMode { get => _ShowMode; set => _ShowMode = value; }
        public List<object> AttachDrawingPropertyData { get => attachDrawingPropertyData; set => attachDrawingPropertyData = value; }
        public MouseButtons CurrentButton { get => this._currentButton; set => this._currentButton = value; }
        public enVisualeViewType ViewType { get => viewType; set => viewType = value; }

        protected virtual void OnGaryValueInfo(GrayValueInfoEventArgs e)
        {
            if (this.GrayValueInfo != null)
                this.GrayValueInfo.Invoke(this, e);
        }
        public VisualizeView(HWindowControl hWindowControl)
        {
            this.hWindowControl = hWindowControl;
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
        }
        public VisualizeView(HWindowControl hWindowControl, bool appendContextMenuStrip)
        {
            this.hWindowControl = hWindowControl;
            if (appendContextMenuStrip)
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
        }
        public virtual void DrawingGraphicObject()
        {
            try
            {
                //this.isDrwingObject = false;
                this.bufferWindow.ClearWindow();
                if (isDisplayModel3D && this.pointCloudModel3D != null)
                    DisplayModelObject3D();
                if (isDisplayHImageData && this.backImage != null)
                    DisplayImageData();
                if (isDisplayXldData && this.xldContourData != null)
                    DisplayXldData();
                if (isDisplayRegionData && this.regionData != null)
                    DisplayRegionData();
                if ((isAttachProperty && !isDisplayHImageData) || (isAttachProperty && !isDisplayModel3D))
                {
                    //ShowAttachProperty();
                }
                this.AutoNodeSize();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("VisualizeView类中->DrawingGraphicObject:方法报错", ex);
            }
        }

        private void AutoNodeSize()
        {
            int row1, col1, row2, col2;
            this.bufferWindow.GetPart(out row1, out col1, out row2, out col2);
            this.nodeSizeRect = Math.Min(col2 - col1, row2 - row1) * 0.01 < 1 ? 1 : Math.Min(col2 - col1, row2 - row1) * 0.01;
        }
        protected virtual void addContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(new ToolStripItem[8]
            {
             new ToolStripMenuItem("自适应图像(Auto)"),
             new ToolStripMenuItem("确认(Confirm)"),
             new ToolStripMenuItem("平移/缩放(Scale)"),
             new ToolStripMenuItem("选择(Select)"),
             new ToolStripMenuItem("3D(View)"),
             new ToolStripMenuItem("保存图像") ,
             new ToolStripMenuItem("保存点云"),
             new ToolStripMenuItem("清除窗口(Clear)"),
            });
            contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(toolStripContextMenuStrip_ItemClicked);
            this.hWindowControl.ContextMenuStrip = contextMenuStrip;
        }

        public virtual void DrawPixRoiShapeOnWindow(enColor color, out PixROI roi)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawRoiShapeOnWindow(enColor color, out ROI roi)
        {
            throw new NotImplementedException();
        }
        public virtual void DrawWcsPointOnWindow(enColor color, double grab_x, double grab_y, out userWcsPoint wcsPoint)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row = 0;
            HTuple col = 0;
            HTuple button;
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.GetMbuttonSubPix(this.hWindowControl.HalconWindow, out row, out col, out button);
            HOperatorSet.DispObj(new HXLDCont(row, col), this.hWindowControl.HalconWindow);
            wcsPoint = new userPixPoint(row, col, this.cameraParam).GetWcsPoint(grab_x, grab_y);
            wcsPoint.Color = color;
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawWcsLineOnWindow(enColor color, double grab_x, double grab_y, out userWcsLine wcsLine)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row1 = 0;
            HTuple col1 = 0;
            HTuple row2 = 0;
            HTuple col2 = 0;
            HObject lineRegion = null;
            HTuple phi = 0;
            ////////////////////////
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawLine(this.hWindowControl.HalconWindow, out row1, out col1, out row2, out col2);
            HOperatorSet.GenRegionLine(out lineRegion, row1, col1, row2, col2);
            ////////////////////////////////////////////////////////////////
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(lineRegion, this.hWindowControl.HalconWindow);
            wcsLine = new userPixLine(row1.D, col1.D, row2.D, col2.D, this.cameraParam).GetWcsLine(grab_x, grab_y);
            wcsLine.Color = color;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixLineOnWindow(enColor color, out userPixLine pixLine)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row1 = 0;
            HTuple col1 = 0;
            HTuple row2 = 0;
            HTuple col2 = 0;
            HObject lineRegion = null;
            HTuple phi = 0;
            ////////////////////////
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawLine(this.hWindowControl.HalconWindow, out row1, out col1, out row2, out col2);
            HOperatorSet.GenRegionLine(out lineRegion, row1, col1, row2, col2);
            ////////////////////////////////////////////////////////////////
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(lineRegion, this.hWindowControl.HalconWindow);
            pixLine = new userPixLine(row1.D, col1.D, row2.D, col2.D);
            //pixLine.color = color;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
        }

        public virtual void DrawPixLineOnWindow(enColor color, out drawPixLine pixLine)
        {
            throw new NotImplementedException();
        }
        public virtual void DrawWcsCircleOnWindow(enColor color, double grab_x, double grab_y, out userWcsCircle wcsCircle)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row = 0;
            HTuple col = 0;
            HTuple radius = 0;
            HObject reccircleRegion = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawCircle(this.hWindowControl.HalconWindow, out row, out col, out radius);
            HOperatorSet.GenCircle(out reccircleRegion, row, col, radius);
            ////////////////////////////////////////////////////////////////
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(reccircleRegion, this.hWindowControl.HalconWindow);
            wcsCircle = new userPixCircle(row.D, col.D, radius.D, this.cameraParam).GetWcsCircle(grab_x, grab_y);
            wcsCircle.Color = color;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixCircleOnWindow(enColor color, out userPixCircle pixCircle)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row = 0;
            HTuple col = 0;
            HTuple radius = 0;
            HObject reccircleRegion = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawCircle(this.hWindowControl.HalconWindow, out row, out col, out radius);
            HOperatorSet.GenCircle(out reccircleRegion, row, col, radius);
            ////////////////////////////////////////////////////////////////
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(reccircleRegion, this.hWindowControl.HalconWindow);
            pixCircle = new userPixCircle(row.D, col.D, radius.D);
            //pixCircle.color = color;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixCircleOnWindow(enColor color, out drawPixCircle pixCircle)
        {
            throw new NotImplementedException();
        }
        public virtual void DrawPixEllipseOnWindow(enColor color, out userPixEllipse pixEllipse)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row = 0;
            HTuple col = 0;
            HTuple phi = 0;
            HTuple radius1 = 0;
            HTuple radius2 = 0;
            HObject reccircleRegion = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawEllipse(this.hWindowControl.HalconWindow, out row, out col, out phi, out radius1, out radius2);
            HOperatorSet.GenEllipse(out reccircleRegion, row, col, phi, radius1, radius2);
            ////////////////////////////////////////////////////////////////
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(reccircleRegion, this.hWindowControl.HalconWindow);
            pixEllipse = new userPixEllipse(row.D, col.D, phi.D, radius1.D, radius2.D);
            pixEllipse.Color = color;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
        }

        public virtual void DrawPixEllipseOnWindow(enColor color, out drawPixEllipse pixEllipse)
        {
            throw new NotImplementedException();
        }
        public virtual void DrawWcsRect2OnWindow(enColor color, double grab_x, double grab_y, out userWcsRectangle2 wcsRec2)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple center_row = 0;
            HTuple center_col = 0;
            HTuple phi = 0;
            HTuple length1 = 0;
            HTuple length2 = 0;
            HObject rectangle = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawRectangle2(this.hWindowControl.HalconWindow, out center_row, out center_col, out phi, out length1, out length2);
            HOperatorSet.GenRectangle2(out rectangle, center_row, center_col, phi, length1, length2);
            ////////////////////////////////////////////////////////////////   
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(rectangle, this.hWindowControl.HalconWindow);
            wcsRec2 = new userPixRectangle2(center_row.D, center_col.D, phi.D, length1.D, length2.D, this.cameraParam).GetWcsRectangle2(grab_x, grab_y);
            wcsRec2.Color = color;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixRect2OnWindow(enColor color, out userPixRectangle2 pixRec2)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple center_row = 0;
            HTuple center_col = 0;
            HTuple phi = 0;
            HTuple length1 = 0;
            HTuple length2 = 0;
            HObject rectangle = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawRectangle2(this.hWindowControl.HalconWindow, out center_row, out center_col, out phi, out length1, out length2);
            HOperatorSet.GenRectangle2(out rectangle, center_row, center_col, phi, length1, length2);
            ////////////////////////////////////////////////////////////////   
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(rectangle, this.hWindowControl.HalconWindow);
            pixRec2 = new userPixRectangle2(center_row.D, center_col.D, phi.D, length1.D, length2.D, this.cameraParam);
            pixRec2.Color = color;
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixRect2OnWindow(enColor color, out drawPixRect2 pixRec2)
        {
            throw new NotImplementedException();
        }
        public virtual void ModifyPixRect2OnWindow(userPixRectangle2 pixRec2, out userPixRectangle2 outPixRec2)  //
        {
            throw new NotImplementedException();
        }
        public virtual void DrawPixRect1OnWindow(enColor color, out userPixRectangle1 pixRec1)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row1 = 0;
            HTuple col1 = 0;
            HTuple row2 = 0;
            HTuple col2 = 0;
            HObject rectangle = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawRectangle1(this.hWindowControl.HalconWindow, out row1, out col1, out row2, out col2);
            HOperatorSet.GenRectangle1(out rectangle, row1, col1, row2, col2);
            ////////////////////////////////////////////////////////////////   
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(rectangle, this.hWindowControl.HalconWindow);
            pixRec1 = new userPixRectangle1(row1, col1, row2, col2, this.cameraParam);
            pixRec1.Color = color;
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixRect1OnWindow(enColor color, out drawPixRect1 pixRec1)
        {
            throw new NotImplementedException();
        }
        public virtual void DrawPixPointOnWindow(enColor color, out userPixPoint pixPoint)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row1 = 0;
            HTuple col1 = 0;
            HTuple row2 = 0;
            HTuple col2 = 0;
            HObject rectangle = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawPoint(this.hWindowControl.HalconWindow, out row1, out col1);
            HOperatorSet.GenRectangle1(out rectangle, row1, col1, row2, col2);
            ////////////////////////////////////////////////////////////////   
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(rectangle, this.hWindowControl.HalconWindow);
            pixPoint = new userPixPoint(row1, col1, this.cameraParam);
            pixPoint.Color = color;
            pixPoint.Size = this.nodeSizeRect * 1;
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixPointOnWindow(enColor color, out drawPixPoint pixPoint)
        {
            this.isTranslate = false;
            this.isDisplayHImageData = false;
            this.isDrawMode = true;
            HTuple row1 = 0;
            HTuple col1 = 0;
            HTuple row2 = 0;
            HTuple col2 = 0;
            HObject rectangle = null;
            if (this.hWindowControl.HalconWindow == null)
            {
                throw new ArgumentNullException("this.hWindowControl.HalconWindow");
            }
            HOperatorSet.SetColor(this.hWindowControl.HalconWindow, color.ToString());
            HOperatorSet.DrawPoint(this.hWindowControl.HalconWindow, out row1, out col1);
            HOperatorSet.GenRectangle1(out rectangle, row1, col1, row2, col2);
            ////////////////////////////////////////////////////////////////   
            HOperatorSet.SetDraw(this.hWindowControl.HalconWindow, "margin");
            HOperatorSet.DispObj(rectangle, this.hWindowControl.HalconWindow);
            pixPoint = new drawPixPoint(row1, col1);
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDisplayHImageData = true;
        }
        public virtual void DrawPixPolygonOnWindow(enColor color, out drawPixPolygon pixPolygon)  //
        {
            throw new NotImplementedException();
        }
        public virtual void DrawPixPolygonOnWindow(enColor color, out userPixPolygon pixPolygon)  //
        {
            throw new NotImplementedException();
        }
        public virtual userPixPoint GetPixPointParam()
        {
            throw new NotImplementedException();
        }
        public virtual userWcsPoint GetWcsPointParam()
        {
            throw new NotImplementedException();
        }
        public virtual userPixLine GetPixLineParam()
        {
            throw new NotImplementedException();
        }
        public virtual userWcsLine GetWcsLineParam()
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
        public virtual userPixPolygon GetPixPolygonParam()  //
        {
            throw new NotImplementedException();
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

        protected virtual void toolStripContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                //lockState = Monitor.TryEnter(this.monitor);
                //if (!lockState) return;
                switch (name)
                {
                    case "自适应图像(Auto)":
                        ((ContextMenuStrip)sender).Close();
                        this.isTranslate = true;
                        //this.isDrwingObject = true;
                        this.AutoImage();
                        break;

                    //////////////////////////////////////
                    case "平移/缩放(Scale)":
                        ((ContextMenuStrip)sender).Close();
                        this.isTranslate = true;
                        //this.isDrwingObject = true;
                        break;

                    case "选择(Select)":
                        ((ContextMenuStrip)sender).Close();
                        this.isTranslate = false;
                        break;

                    case "清除窗口(Clear)":
                        ((ContextMenuStrip)sender).Close();
                        this.hWindowControl.HalconWindow.ClearWindow();
                        this.bufferWindow.ClearWindow();
                        this.attachPropertyData.Clear();
                        this.BackImage = null; ;
                        this.RegionData = null;
                        this.XldContourData = null;
                        this.XldContourData?.Dispose();
                        break;

                    case "3D(View)":
                        ((ContextMenuStrip)sender).Close();
                        this.isTranslate = false;
                        Task.Run(() => { Show3D(); }); // 查看3D对象必需是FA镜头参数才可以放大缩小 
                        break;

                    case "保存图像":
                        ((ContextMenuStrip)sender).Close();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.BackImage != null && this.BackImage.Image.IsInitialized())
                        {
                            HImage hImage = this.BackImage.Image?.Clone();
                            hImage?.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        }
                        else
                            MessageBox.Show("图像内容为空");
                        break;

                    case "保存点云":
                        ((ContextMenuStrip)sender).Close();
                        saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "ply files (*.ply)|*.ply|txt files (*.txt)|*.txt|om3 files (*.om3)|*.om3|stl files (*.stl)|*.stl|obj files (*.obj)|*.obj|dxf files (*.dxf)|*.dxf|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 3;
                        saveFileDialog1.ShowDialog();
                        if (this.pointCloudModel3D != null)
                        {
                            HObjectModel3D hObjectModel3D = HObjectModel3D.UnionObjectModel3d(this.pointCloudModel3D.ObjectModel3D, "points_surface");
                            hObjectModel3D?.WriteObjectModel3d(new FileInfo(saveFileDialog1.FileName).Extension, saveFileDialog1.FileName, new HTuple(), new HTuple());
                            hObjectModel3D?.ClearObjectModel3d();
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("VisualizeView类中->toolStripContextMenuStrip_ItemClicked:方法报错", ex);
            }
            finally
            {
                //if (lockState)
                //    Monitor.Exit(this.monitor);
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
                    partHeight = this.hWindowControl.Height > imageHeight ? this.hWindowControl.Height : imageHeight; //以高度作为参考基准  imageHeight; //
                    partWidth = (int)(partHeight * scale) > imageWidth ? (int)(partHeight * scale) : imageWidth; //
                    if ((int)(partHeight * scale) < imageWidth)
                        partHeight = (int)(partWidth / scale);
                }
                else
                {
                    partWidth = this.hWindowControl.Width > imageWidth ? this.hWindowControl.Width : imageWidth; // 以宽度作为参考基准  this.initImageWidth   imageWidth; //
                    partHeight = (int)(partWidth / scale) > imageHeight ? (int)(partWidth / scale) : imageHeight;
                    if ((int)(partWidth / scale) < imageHeight)
                        partWidth = (int)(partHeight * scale);
                }
                int row, col; // 计算左上角起点
                col = this.initImageWidth < partWidth ? (int)((partWidth - this.initImageWidth) * -0.5) : 0;
                row = this.initImageHeight < partHeight ? (int)((partHeight - this.initImageHeight) * -0.5) : 0;
                //////////////////////
                if (this.bufferWindow != null && this.bufferWindow.IsInitialized())
                    this.bufferWindow.SetPart(row, col, partHeight - 1 + row, partWidth - 1 + col); // 设置窗口的图像部分与图像的大小相等 
            }
        }

        /// <summary>
        /// 过时的算子，不再使用
        /// </summary>
        public void UpdataGraphicView()
        {
            //lockState = false;
            try
            {
                //lockState = Monitor.TryEnter(this.monitor);
                getAttatchPropertyViewParam(); // 这个用来描述世界坐标系
                if (this.cameraParam.CamParam != null)
                    SetImagePartSize((int)this.cameraParam.CamParam.Width, (int)this.cameraParam.CamParam.Height, true);
                DrawingGraphicObject();
            }
            finally
            {
                //if (lockState)
                //    Monitor.Exit(this.monitor);
            }
        }


        /// <summary>
        /// 显示附加属性，并将源窗口内容复制到目标窗口中
        /// </summary>
        public void ShowAttachProperty()
        {
            if (this.isAttachProperty)
            {
                for (int i = 0; i < this.AttachDrawingPropertyData.Count; i++)
                {
                    this.bufferWindow.SetColor("red");
                    this.bufferWindow.SetLineWidth(2);
                    if (this.AttachDrawingPropertyData[i] == null) continue;
                    switch (this.AttachDrawingPropertyData[i].GetType().Name)
                    {
                        case "HImage":
                        case "HRegion":
                        case "HObject":
                            this.bufferWindow.SetDraw("margin");
                            if (((HObject)this.AttachDrawingPropertyData[i]).IsInitialized()) //这里要注意指针地址的益处
                                this.bufferWindow.DispObj((HObject)this.AttachDrawingPropertyData[i]);
                            break;
                        case "HXLDCont": // 用于显示绘图对象
                            if (((HXLDCont)this.AttachDrawingPropertyData[i]).IsInitialized())
                                this.bufferWindow.DispObj((HXLDCont)this.AttachDrawingPropertyData[i]);
                            break;
                        case "userPixRect2":
                            this.bufferWindow.SetColor("red");
                            this.bufferWindow.DispObj(((drawPixRect2)this.AttachDrawingPropertyData[i]).GetXLD());
                            break;
                        case nameof(ViewData):
                            ViewData viewObject = this.AttachDrawingPropertyData[i] as ViewData;
                            if (viewObject == null || viewObject.DataObject == null) return;
                            switch (viewObject.DataObject.GetType().Name)
                            {
                                case nameof(HObject):
                                    if (!((HObject)viewObject.DataObject).IsInitialized()) return;
                                    this.bufferWindow.SetColor(viewObject.Color);
                                    this.bufferWindow.DispObj(((HObject)viewObject.DataObject)); // 使用投影的方式，才能实现统一
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
                            }
                            break;
                    }
                }
                //////////////////////////////////////////////////////////////////
                for (int i = 0; i < this.attachPropertyData.Count; i++)
                {
                    this.bufferWindow.SetColor("green");
                    if (this.attachPropertyData[i] == null) continue;
                    switch (this.attachPropertyData[i].GetType().Name)
                    {
                        case "HObject":
                            if ((this.attachPropertyData[i]) != null)
                                this.bufferWindow.DispObj((HObject)this.attachPropertyData[i]);
                            break;
                        case "HRegion":
                            this.bufferWindow.SetDraw("margin");
                            this.bufferWindow.SetColor("red");
                            if ((this.attachPropertyData[i]) != null)
                                this.bufferWindow.DispObj((HRegion)this.attachPropertyData[i]);
                            break;
                        case "HXLD":
                            if ((this.attachPropertyData[i]) != null && ((HXLD)this.attachPropertyData[i]).IsInitialized())
                                this.bufferWindow.DispObj((HXLD)this.attachPropertyData[i]);
                            break;
                        case "HXLDCont":
                            if ((this.attachPropertyData[i]) != null && ((HXLDCont)this.attachPropertyData[i]).IsInitialized())
                                this.bufferWindow.DispObj((HXLDCont)this.attachPropertyData[i]);
                            break;
                        case "XldDataClass":
                            if ((this.attachPropertyData[i]) != null && (((XldDataClass)this.attachPropertyData[i]).HXldCont).IsInitialized())
                                this.bufferWindow.DispObj(((XldDataClass)this.attachPropertyData[i]).HXldCont);
                            break;
                        case "userWcsRectangle2":
                            if (this.attachPropertyData.Count == 0) return;
                            userWcsRectangle2 wcsRect2 = (userWcsRectangle2)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsRect2.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsRect2).GetPixRectangle2From3D().GetXLD()); // 这里为什么要用3D来显示？
                            else
                                this.bufferWindow.DispObj((wcsRect2).GetPixRectangle2().GetXLD());
                            break;
                        case "userWcsRectangle1":
                            if (this.attachPropertyData.Count == 0) return;
                            userWcsRectangle1 wcsRect1 = (userWcsRectangle1)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsRect1.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsRect1).GetPixRectangle1FromWcs3D().GetXLD());
                            else
                                this.bufferWindow.DispObj((wcsRect1).GetPixRectangle1().GetXLD());
                            break;
                        case "userWcsPoint":
                            userWcsPoint wcsPoint = (userWcsPoint)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsPoint.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsPoint).GetPixPointFromWcs3D().GetXLD());
                            else
                                this.bufferWindow.DispObj((wcsPoint).GetPixPoint().GetXLD());
                            break;
                        case "userWcsLine":
                            if (this.attachPropertyData.Count == 0) return;
                            userWcsLine wcsLine = (userWcsLine)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsLine.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsLine).GetPixLineFromWcs3D().GetXLD());
                            else
                                this.bufferWindow.DispObj((wcsLine).GetPixLine().GetXLD());
                            break;
                        case "drawWcsLine":
                            if (this.attachPropertyData.Count == 0) return;
                            drawWcsLine drawLine = (drawWcsLine)this.attachPropertyData[i];
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((drawLine).GetPixLine(this.cameraParam).GetXLD());
                            else
                                this.bufferWindow.DispObj((drawLine).GetPixLine(this.cameraParam, backImage.Grab_X, backImage.Grab_Y).GetXLD());
                            break;
                        case "userWcsCircle":
                            if (this.attachPropertyData.Count == 0) return;
                            userWcsCircle wcsCircle = (userWcsCircle)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsCircle.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsCircle).GetPixCircleFromWcs3D().GetXLD());
                            else
                                this.bufferWindow.DispObj((wcsCircle).GetPixCircle().GetXLD());
                            break;
                        case "userWcsCircleSector":
                            if (this.attachPropertyData.Count == 0) return;
                            userWcsCircleSector wcsCircleSector = (userWcsCircleSector)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsCircleSector.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsCircleSector).GetPixCircleSectorFrom3D().GetXLD());
                            else
                                this.bufferWindow.DispObj((wcsCircleSector).GetPixCircleSector().GetXLD());
                            break;
                        case "userWcsEllipse":
                            userWcsEllipse wcsEllipse = (userWcsEllipse)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsEllipse.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsEllipse).GetPixEllipseFromWcs3D().GetXLD());
                            else
                                this.bufferWindow.DispObj((wcsEllipse).GetPixEllipse().GetXLD());
                            break;
                        case "userWcsEllipseSector":
                            userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)this.attachPropertyData[i];
                            this.bufferWindow.SetColor(wcsEllipseSector.Color.ToString());
                            if ((isDisplayModel3D || isAttachProperty) && !isDisplayHImageData)
                                this.bufferWindow.DispObj((wcsEllipseSector).GetPixEllipseSectorFrom3D().GetXLD());
                            else
                                this.bufferWindow.DispObj((wcsEllipseSector).GetPixEllipseSector().GetXLD());
                            break;
                        case "userWcsCoordSystem":
                            userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)this.attachPropertyData[i];
                            HTuple row22, col22;
                            if (isDisplayModel3D || isAttachProperty)
                            {
                                int minLength = this.initImageWidth > this.initWindowHeight ? this.initWindowHeight : this.initImageWidth;
                                this.bufferWindow.SetColor("red");
                                this.bufferWindow.DispObj(wcsCoordSystem.getAxis_x_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                                wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D + (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "X", "red");
                                this.bufferWindow.SetColor("yellow");
                                this.bufferWindow.DispObj(wcsCoordSystem.getAxis_y_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                                wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D - (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "Y", "yellow");
                            }
                            else
                            {
                                int minLength = this.initImageWidth > this.initWindowHeight ? this.initWindowHeight : this.initImageWidth;
                                this.bufferWindow.SetColor("red");
                                this.bufferWindow.DispObj(wcsCoordSystem.getAxis_x_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                                wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D + (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "X", "red");
                                this.bufferWindow.SetColor("yellow");
                                this.bufferWindow.DispObj(wcsCoordSystem.getAxis_y_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), (int)(minLength * 0.5 / this.nodeSizeRect), out row22, out col22));
                                wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D - (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(50 / this.nodeSizeRect), "Y", "yellow");
                            }
                            break;
                        case "RegionDataClass":
                            //this.bufferWindow.SetColor("red"); // 区域显示一定要使用红色
                            this.bufferWindow.SetColored(12); // 区域显示一定要使用红色
                            this.bufferWindow.SetDraw(((RegionDataClass)this.attachPropertyData[i]).Draw);
                            this.bufferWindow.DispObj(((RegionDataClass)this.attachPropertyData[i]).Region);
                            break;
                        case "userPixPoint":
                            this.bufferWindow.SetColor(((userPixPoint)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixPoint)this.attachPropertyData[i]).GetXLD());
                            break;
                        case "userPixLine":
                            this.bufferWindow.SetColor(((userPixLine)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixLine)this.attachPropertyData[i]).GetXLD());
                            break;
                        case "userPixCircle":
                            this.bufferWindow.SetColor(((userPixCircle)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixCircle)this.attachPropertyData[i]).GetXLD());
                            break;
                        case "userPixCircleSector":
                            this.bufferWindow.SetColor(((userPixCircleSector)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixCircleSector)this.attachPropertyData[i]).GetXLD());
                            break;
                        case "userPixEllipse":
                            this.bufferWindow.SetColor(((userPixEllipse)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixEllipse)this.attachPropertyData[i]).GetXLD());
                            break;
                        case "userPixEllipseSector":
                            this.bufferWindow.SetColor(((userPixEllipseSector)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixEllipseSector)this.attachPropertyData[i]).GetXLD());
                            break;
                        case "userPixRectangle1":
                            this.bufferWindow.SetColor(((userPixRectangle1)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixRectangle1)this.attachPropertyData[i]).GetXLD());
                            break;
                        case "userPixRectangle2":
                            this.bufferWindow.SetColor(((userPixRectangle2)this.attachPropertyData[i]).Color.ToString());
                            this.bufferWindow.DispObj(((userPixRectangle2)this.attachPropertyData[i]).GetXLD()); // 使用投影的方式，才能实现统一
                            break;
                        case "userOkNgText":
                            int row, col, row222, col222;
                            this.bufferWindow.GetPart(out row, out col, out row222, out col222);
                            userOkNgText OkNGText = (userOkNgText)this.attachPropertyData[i];
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
                        case "userTextLable":
                            userTextLable TextLable = (userTextLable)this.attachPropertyData[i];
                            TextLable.CamParam = this.cameraParam;
                            //TextLable.camPose = this.camPose;
                            TextLable.WriteString(this.bufferWindow); // 字体放在右上角,文本的参考角为左上角
                            break;
                        case nameof(ViewData):
                            ViewData viewObject = this.attachPropertyData[i] as ViewData;
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
                                    wcsCoordSystem = (userWcsCoordSystem)viewObject.DataObject;
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
                            break;
                    }
                }
            }
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
                case nameof(ImageDataClass):
                    if (!((ImageDataClass)viewObject.DataObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(viewObject.Color);
                    this.bufferWindow.DispObj(((ImageDataClass)viewObject.DataObject).Image); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HObject):
                    if (!((HObject)viewObject.DataObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(viewObject.Color);
                    this.bufferWindow.DispObj(((HObject)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(RegionDataClass):
                    if (!((RegionDataClass)viewObject.DataObject).IsInitialized()) return;
                    int result = 0;
                    if (int.TryParse(viewObject.Color, out result))
                        this.bufferWindow.SetColored(result); // 区域显示一定要使用红色
                    else
                        this.bufferWindow.SetColor(viewObject.Color); // 区域显示一定要使用红色
                    this.bufferWindow.SetDraw(viewObject.Draw);
                    this.bufferWindow.DispObj(((RegionDataClass)viewObject.DataObject).Region); // 使用投影的方式，才能实现统一
                    break;
                case nameof(HRegion):
                    if (!((HRegion)viewObject.DataObject).IsInitialized()) return;
                    result = 0;
                    if (int.TryParse(viewObject.Color, out result))
                        this.bufferWindow.SetColored(result); // 区域显示一定要使用红色
                    else
                        this.bufferWindow.SetColor(viewObject.Color); // 区域显示一定要使用红色
                    this.bufferWindow.SetDraw(viewObject.Draw);
                    this.bufferWindow.DispObj(((HRegion)viewObject.DataObject)); // 使用投影的方式，才能实现统一
                    break;
                case nameof(XldDataClass):
                    if (!((XldDataClass)viewObject.DataObject).IsInitialized()) return;
                    this.bufferWindow.SetColor(viewObject.Color);
                    this.bufferWindow.DispObj(((XldDataClass)viewObject.DataObject).HXldCont); // 使用投影的方式，才能实现统一
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
                    this.bufferWindow.SetColor("red");
                    this.bufferWindow.DispObj(wcsCoordSystem.getAxis_x_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), this.nodeSizeRect * 10, out row22, out col22));
                    wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D + (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(this.nodeSizeRect * 2), "X", "red");
                    this.bufferWindow.SetColor("yellow");
                    this.bufferWindow.DispObj(wcsCoordSystem.getAxis_y_Arrow(wcsCoordSystem.CurrentPoint.GetPixVector(), this.nodeSizeRect * 10, out row22, out col22));
                    wcsCoordSystem.WriteString(this.bufferWindow, (int)row22.D - (int)(this.nodeSizeRect * 2), (int)col22.D + (int)(this.nodeSizeRect * 2), (int)(this.nodeSizeRect * 2), "Y", "yellow");
                    break;
                case nameof(userPixPoint):
                    userPixPoint pixPoint = (userPixPoint)viewObject.DataObject;
                    pixPoint.Size = this.nodeSizeRect;
                    this.bufferWindow.SetColor(pixPoint.Color.ToString());
                    this.bufferWindow.DispObj(pixPoint.GetXLD(this.nodeSizeRect));
                    break;
            }
            this.attachPropertyData.Add(viewObject);
            // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            int row1, col1, row2, col2;
            this.bufferWindow.GetPart(out row1, out col1, out row2, out col2); // 只有源窗口与目标窗口的图像部分相一致，这样在不同的窗口中显示才相同
            this.hWindowControl.ImagePart = new Rectangle(col1, row1, col2 - col1, row2 - row1); // 一定要设置hWindowControl控件的图像部分，这样获取到的鼠标坐标才是考虑的图像部分的
            //this.hWindowControl.HalconWindow.SetPart(row1, col1, row2, col2); // 目标窗口的图像部分要与buffer窗口的图像部分相同
            this.bufferWindow.CopyRectangle(this.hWindowControl.HalconWindow, 0, 0, this.hWindowControl.Height, this.hWindowControl.Width, 0, 0);
        }
        public void RemoveViewObjectAt(int index)
        {
            if (this.attachPropertyData.Count > index)
                this.attachPropertyData.RemoveAt(index);
            this.DrawingGraphicObject();
        }
        public void RemoveViewObject(ViewData viewObject)
        {
            if (this.AttachDrawingPropertyData.Contains(viewObject))
                this.attachPropertyData.Remove(viewObject);
            this.DrawingGraphicObject();
        }
        public void ClearViewObject(bool isUpdataView = true)
        {
            this.attachPropertyData.Clear();
            if (isUpdataView)
                this.DrawingGraphicObject();
        }
        public void ChangeColorViewObject(ViewData viewObject, string color)
        {
            foreach (var item in this.attachPropertyData)
            {
                if (item.Equals(viewObject))
                {
                    ((ViewData)item).Color = color;
                    break;
                }
            }
            this.DrawingGraphicObject();
        }



        private void getAttatchPropertyViewParam()
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> z = new List<double>();
            for (int i = 0; i < this.attachPropertyData.Count; i++)
            {
                switch (this.attachPropertyData[i].GetType().Name)
                {
                    case "userWcsRectangle2":
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)this.attachPropertyData[i];
                        if (wcsRect2.EdgesPoint_xyz != null)
                        {
                            foreach (var item in wcsRect2.EdgesPoint_xyz)
                            {
                                x.Add(item.X);
                                y.Add(item.Y);
                            }

                        }
                        else
                        {
                            x.Add(wcsRect2.X);
                            y.Add(wcsRect2.Y);
                        }
                        break;
                    case "userWcsRectangle2[]":
                        userWcsRectangle2[] wcsRect2Array = (userWcsRectangle2[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsRect2Array.Length; ii++)
                        {
                            if (wcsRect2Array[ii].EdgesPoint_xyz != null)
                            {
                                foreach (var item in wcsRect2Array[ii].EdgesPoint_xyz)
                                {
                                    x.Add(item.X);
                                    y.Add(item.Y);
                                }
                            }
                            else
                            {
                                x.Add(wcsRect2Array[ii].X);
                                y.Add(wcsRect2Array[ii].Y);
                            }
                        }
                        break;
                    case "userWcsRectangle1":
                        userWcsRectangle1 wcsRect1 = (userWcsRectangle1)this.attachPropertyData[i];
                        if (wcsRect1.EdgesPoint_xyz != null)
                        {
                            foreach (var item in wcsRect1.EdgesPoint_xyz)
                            {
                                x.Add(item.X);
                                y.Add(item.Y);
                            }
                        }
                        else
                        {
                            x.Add(wcsRect1.X1);
                            y.Add(wcsRect1.Y1);
                            x.Add(wcsRect1.X2);
                            y.Add(wcsRect1.Y2);
                        }
                        break;
                    case "userWcsRectangle1[]":
                        userWcsRectangle1[] wcsRect1Array = (userWcsRectangle1[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsRect1Array.Length; ii++)
                        {
                            if (wcsRect1Array[ii].EdgesPoint_xyz != null)
                            {
                                foreach (var item in wcsRect1Array[ii].EdgesPoint_xyz)
                                {
                                    x.Add(item.X);
                                    y.Add(item.Y);
                                }
                            }
                            else
                            {
                                x.Add(wcsRect1Array[ii].X1);
                                y.Add(wcsRect1Array[ii].Y1);
                                x.Add(wcsRect1Array[ii].X2);
                                y.Add(wcsRect1Array[ii].Y2);
                            }
                        }
                        break;
                    case "userWcsPoint":
                        userWcsPoint wcsPoint = (userWcsPoint)this.attachPropertyData[i];
                        x.Add(wcsPoint.X);
                        y.Add(wcsPoint.Y);
                        break;
                    case "userWcsPoint[]":
                        userWcsPoint[] wcsPointArray = (userWcsPoint[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsPointArray.Length; ii++)
                        {
                            x.Add(wcsPointArray[ii].X);
                            y.Add(wcsPointArray[ii].Y);
                        }
                        break;
                    case "userWcsLine":
                        userWcsLine wcsLine = (userWcsLine)this.attachPropertyData[i];
                        if (wcsLine.EdgesPoint_xyz != null)
                        {
                            foreach (var item in wcsLine.EdgesPoint_xyz)
                            {
                                x.Add(item.X);
                                y.Add(item.Y);
                            }
                        }
                        else
                        {
                            x.Add(wcsLine.X1);
                            y.Add(wcsLine.Y1);
                            x.Add(wcsLine.X2);
                            y.Add(wcsLine.Y2);
                        }
                        break;
                    case "userWcsLine[]":
                        userWcsLine[] wcsLineArray = (userWcsLine[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsLineArray.Length; ii++)
                        {
                            if (wcsLineArray[ii].EdgesPoint_xyz != null)
                            {
                                foreach (var item in wcsLineArray[ii].EdgesPoint_xyz)
                                {
                                    x.Add(item.X);
                                    y.Add(item.Y);
                                }
                            }
                            else
                            {
                                x.Add(wcsLineArray[ii].X1);
                                y.Add(wcsLineArray[ii].Y1);
                                x.Add(wcsLineArray[ii].X2);
                                y.Add(wcsLineArray[ii].Y2);
                            }
                        }
                        break;
                    case "userWcsCircle":
                        userWcsCircle wcsCircle = (userWcsCircle)this.attachPropertyData[i];
                        if (wcsCircle.EdgesPoint_xyz != null)
                        {
                            foreach (var item in wcsCircle.EdgesPoint_xyz)
                            {
                                x.Add(item.X);
                                y.Add(item.Y);
                            }
                        }
                        else
                        {
                            x.Add(wcsCircle.X);
                            y.Add(wcsCircle.Y);
                        }
                        break;
                    case "userWcsCircle[]":
                        userWcsCircle[] wcsCircleArray = (userWcsCircle[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsCircleArray.Length; ii++)
                        {
                            if (wcsCircleArray[ii].EdgesPoint_xyz != null)
                            {
                                foreach (var item in wcsCircleArray[ii].EdgesPoint_xyz)
                                {
                                    x.Add(item.X);
                                    y.Add(item.Y);
                                }
                            }
                            else
                            {
                                x.Add(wcsCircleArray[ii].X);
                                y.Add(wcsCircleArray[ii].Y);
                            }
                        }
                        break;
                    case "userWcsCircleSector":
                        userWcsCircleSector wcsCircleSector = (userWcsCircleSector)this.attachPropertyData[i];
                        if (wcsCircleSector.EdgesPoint_xyz != null)
                        {
                            foreach (var item in wcsCircleSector.EdgesPoint_xyz)
                            {
                                x.Add(item.X);
                                y.Add(item.Y);
                            }
                        }
                        else
                        {
                            x.Add(wcsCircleSector.X);
                            y.Add(wcsCircleSector.Y);
                        }
                        break;
                    case "userWcsCircleSector[]":
                        userWcsCircleSector[] wcsCircleSectorArray = (userWcsCircleSector[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsCircleSectorArray.Length; ii++)
                        {
                            if (wcsCircleSectorArray[ii].EdgesPoint_xyz != null)
                            {
                                foreach (var item in wcsCircleSectorArray[ii].EdgesPoint_xyz)
                                {
                                    x.Add(item.X);
                                    y.Add(item.Y);
                                }
                            }
                            else
                            {
                                x.Add(wcsCircleSectorArray[ii].X);
                                y.Add(wcsCircleSectorArray[ii].Y);
                            }
                        }
                        break;
                    case "userWcsEllipse":
                        userWcsEllipse wcsEllipse = (userWcsEllipse)this.attachPropertyData[i];
                        if (wcsEllipse.EdgesPoint_xyz != null)
                        {
                            foreach (var item in wcsEllipse.EdgesPoint_xyz)
                            {
                                x.Add(item.X);
                                y.Add(item.Y);
                            }
                        }
                        else
                        {
                            x.Add(wcsEllipse.X);
                            y.Add(wcsEllipse.Y);
                        }
                        break;
                    case "userWcsEllipse[]":
                        userWcsEllipse[] wcsEllipseArray = (userWcsEllipse[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsEllipseArray.Length; ii++)
                        {
                            if (wcsEllipseArray[i].EdgesPoint_xyz != null)
                            {
                                foreach (var item in wcsEllipseArray[i].EdgesPoint_xyz)
                                {
                                    x.Add(item.X);
                                    y.Add(item.Y);
                                }
                            }
                            else
                            {
                                x.Add(wcsEllipseArray[i].X);
                                y.Add(wcsEllipseArray[i].Y);
                            }
                        }
                        break;
                    case "userWcsEllipseSector":
                        userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)this.attachPropertyData[i];
                        if (wcsEllipseSector.EdgesPoint_xyz != null)
                        {
                            foreach (var item in wcsEllipseSector.EdgesPoint_xyz)
                            {
                                x.Add(item.X);
                                y.Add(item.Y);
                            }
                        }
                        else
                        {
                            x.Add(wcsEllipseSector.X);
                            y.Add(wcsEllipseSector.Y);
                        }
                        break;
                    case "userWcsEllipseSector[]":
                        userWcsEllipseSector[] wcsEllipseSectorArray = (userWcsEllipseSector[])this.attachPropertyData[i];
                        for (int ii = 0; ii < wcsEllipseSectorArray.Length; ii++)
                        {
                            if (wcsEllipseSectorArray[ii].EdgesPoint_xyz != null)
                            {
                                foreach (var item in wcsEllipseSectorArray[ii].EdgesPoint_xyz)
                                {
                                    x.Add(item.X);
                                    y.Add(item.Y);
                                }
                            }
                            else
                            {
                                x.Add(wcsEllipseSectorArray[ii].X);
                                y.Add(wcsEllipseSectorArray[ii].Y);
                            }
                        }
                        break;
                }
            }
            for (int i = 0; i < x.Count; i++)
            {
                z.Add(0);
            }
            if (x.Count > 0) // 初始化视图内外参
            {
                HObjectModel3D hObjectModel3D = new HObjectModel3D(x.ToArray(), y.ToArray(), z.ToArray());
                //InitCamParam();
                //InitMagnification(new HObjectModel3D[] { hObjectModel3D });
                //InitPixSize(new HObjectModel3D[] { hObjectModel3D });
                //InitCamPose(new HObjectModel3D[] { hObjectModel3D });
                hObjectModel3D.Dispose();
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
                if (this.bufferWindow != null && this.bufferWindow.Handle.ToInt64() > 0)
                    this.bufferWindow.CloseWindow();
                this.bufferWindow = new HWindow(0, 0, this.hWindowControl.Width, this.hWindowControl.Height, 0, "buffer", "");
                // 设置窗口参数
                this.bufferWindow.SetColor("red");
                this.bufferWindow.SetLineWidth(1);
                this.bufferWindow.SetPart(0, 0, this.hWindowControl.Height - 1, this.hWindowControl.Width - 1); //这里不要设置 
                                                                                                                //// 只在显示对象为3D对象时，才需要重置内外参
                if (isDisplayModel3D && this.pointCloudModel3D != null)
                {
                    //InitCamParam();// 因为相机内参与窗口大小相关，所在在改变窗口大小后需要改变相机内参
                    //InitPixSize(pointCloudModel3D);
                    //InitMagnification(pointCloudModel3D);
                    //InitCamPose(pointCloudModel3D.ObjectModel3D);
                    DrawingGraphicObject();
                }
                /// 显示图像的初始化
                if (isDisplayHImageData && this.backImage != null)
                {
                    DrawingGraphicObject();
                }
                /// 显示XLD初始化
                if (isDisplayXldData && this.xldContourData != null)
                {
                    DrawingGraphicObject();
                }
                /// 显示Region初始化
                if (isDisplayRegionData && this.regionData != null)
                {
                    DrawingGraphicObject();
                }
            }
            catch
            {
                LoggerHelper.Error("VisualizeView类initBufferWindow（）:初始化错误");
            }
        }
        protected virtual void AutoWindow()
        {
            try
            {
                int row, col, width, height;
                if (this.bufferWindow == null || this.bufferWindow.Handle.ToInt64() == 0) return;
                this.bufferWindow.ClearWindow();
                switch (viewType)
                {
                    case enVisualeViewType.模型视图:
                        if (this.lensType == enLensType.FA镜头)
                        {
                            this.lensType = enLensType.远心镜头;
                            //InitCamParam(); // 如果是从FA视图到远心视图，则需要重新初化相机内外参
                            //InitPixSize(this.pointCloudModel3D);
                            //InitMagnification(pointCloudModel3D);
                            //InitCamPose(this.pointCloudModel3D.ObjectModel3D);
                        }
                        //////////////////////////////////////
                        if (this.lensType == enLensType.远心镜头)
                        {
                            this.SetImagePartSize((int)this.cameraParam.CamParam?.Width, (int)this.cameraParam.CamParam?.Height, true);
                            //this.bufferWindow.SetPart(0, 0, (int)this.cameraParam.DataHeight - 1, (int)this.cameraParam.DataWidth - 1);
                        }
                        else
                        {
                            this.bufferWindow.GetWindowExtents(out row, out col, out width, out height);
                            this.bufferWindow.SetPart(0, 0, height - 1, width - 1);
                        }
                        break;
                    case enVisualeViewType.XLD视图:
                        //getXldImagePart(this.xldContourData);
                        SetImagePartSize(this.initImageWidth, this.initImageHeight, true);
                        break;
                    case enVisualeViewType.图像视图:
                        SetImagePartSize(this.initImageWidth, this.initImageHeight, true);
                        break;
                    case enVisualeViewType.Region视图:
                        getRegionImagePart(this.regionData);
                        break;
                }
                /// 节点在前调节
                this.AutoNodeSize();
                DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("VisualizeView类中->AutoWindow:方法报错", ex);
            }
        }
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
            this._currentButton = e.Button;
            this.mIsClick = true;
            this.CurrentButton = e.Button;
            /////////////
            oldX = e.X;
            oldY = e.Y;
        }
        protected virtual void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            this.mIsClick = false;
            if (this.isTranslate)
            {
                if (e.Button == MouseButtons.Right) return;
                //this.isDrwingObject = true;
                this.MoveImage((int)(e.X - oldX), (int)(e.Y - oldY));
            }
        }
        protected virtual void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (this.isTranslate)
            {
                //this.isDrwingObject = true;
                if (e.Delta < 0)
                    this.ZoomImage(e.Y, e.X, 0.95);
                else
                    this.ZoomImage(e.Y, e.X, 1.05);
            }
        }
        protected virtual void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                // 获取灰度图的灰度信息
                if (this.backImage != null && this.backImage.Image != null && this.backImage.Image.IsInitialized())
                {
                    if (e.X > 0 && e.X < this.initImageWidth && e.Y > 0 && e.Y < this.initImageHeight)
                        this.OnGaryValueInfo(new GrayValueInfoEventArgs(Math.Round(e.Y, 3), Math.Round(e.X, 3), this.backImage.Image.GetGrayval(e.Y, e.X)));
                    else
                        this.OnGaryValueInfo(new GrayValueInfoEventArgs(Math.Round(e.Y, 3), Math.Round(e.X, 3), new HTuple("-")));
                }
                /// 获取3D图的调试信息
                if (this.pointCloudModel3D != null)
                {
                    if (this.paramName_depthPersistence == "depth_persistence")
                    {
                        if (e.X > 0 && e.X < this.initImageWidth && e.Y > 0 && e.Y < this.initImageHeight)
                        {
                            double wcs_x, wcs_y, wcs_z;
                            this.cameraParam.ImagePointsToWorldPlane(e.Y, e.X, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                            wcs_z = this.pointCloudModel3D.GetHeightValueOnMouse(wcs_x, wcs_y, 0.5);
                            this.OnGaryValueInfo(new GrayValueInfoEventArgs(Math.Round(wcs_x, 5), Math.Round(wcs_y, 5), Math.Round(wcs_z, 5)));
                        }
                        else
                            this.OnGaryValueInfo(new GrayValueInfoEventArgs(Math.Round(e.Y, 3), Math.Round(e.X, 3), new HTuple("-")));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("获取灰度值出错" + ex.ToString());
            }
        }
        protected virtual void hWindowControl_SizeChanged(object sender, EventArgs e)
        {
            //bool lockTaken = false;
            try
            {
                //Monitor.TryEnter(this.monitor, ref lockTaken);
                initBufferWindow(this.hWindowControl);
                SetImagePartSize(this.initImageWidth, this.initImageHeight, true);
                this.DrawingGraphicObject();
            }
            finally
            {
                //if (lockTaken)
                //Monitor.Exit(this.monitor);
            }
        }
        protected HXLDCont CreateRectSizableNode(double x, double y)
        {
            HXLDCont rect = new HXLDCont();
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
        public virtual void ClearDrawingObject()
        {
            this.CurrentButton = MouseButtons.None;
            this.isDrawMode = false;
            if (this.isInitEvent)
            {
                this.isInitEvent = false;
                this.hWindowControl.HMouseDown -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseDown);
                this.hWindowControl.HMouseUp -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseUp);
                this.hWindowControl.HMouseWheel -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseWheel);
                this.hWindowControl.HMouseMove -= new HalconDotNet.HMouseEventHandler(hWindowControl_HMouseMove);
                this.hWindowControl.SizeChanged -= new System.EventHandler(hWindowControl_SizeChanged);
            }
            this.attachPropertyData.Clear();
            this.pointCloudModel3D = null;
            this.xldContourData = null;
            this.backImage = null;
            Thread.Sleep(100);
        }
        public virtual void AutoImage()
        {
            this.isTranslate = true;
            //this.isDrwingObject = true;
            this.AutoWindow();
        }
        public void TranslateScaleImage()
        {
            this.isTranslate = true;
            //this.isDrwingObject = true;
        }
        public void Select()
        {
            this.isTranslate = false;
        }
        public void ClearWindow()
        {
            this.hWindowControl.HalconWindow.ClearWindow();
            this.attachPropertyData.Clear();
            this.attachDrawingPropertyData.Clear();
            this.ClearViewObject();
        }


        private void InitCamPose2(HObjectModel3D[] objectModel)
        {
            //////////////////////////////////////////////
            HTuple center, PoseEstimated, X, Y, Z;
            if (objectModel == null || objectModel.Length == 0) return;
            HTuple num = objectModel[0].GetObjectModel3dParams("num_points");
            HTuple primitive = objectModel[0].GetObjectModel3dParams("has_primitive_data");
            if (num.I == 0 && primitive.S == "false") return;
            GetObjectModelsCenter(objectModel, out center);
            X = -center.TupleSelect(0);
            Y = -center.TupleSelect(1);
            Z = -center.TupleSelect(2);
            userCamPose camPose = new userCamPose(X, Y, Z, 0.0, 0.0, 0.0);
            // 调整到一个合适的位姿，主要用于计算Z方向距离
            DetermineOptimumPoseDistance(objectModel, this.cameraParam.CamParam.GetHtuple(), 0.9, camPose.GetHtuple(), out PoseEstimated);
            // 将位姿绕当前坐标系旋转180度，这样就和世界坐标系一致了
            HTuple homMat3D, phi, homMat3DRotate;
            HOperatorSet.PoseToHomMat3d(PoseEstimated, out homMat3D);
            HOperatorSet.TupleRad(180, out phi);
            HOperatorSet.HomMat3dRotate(homMat3D, phi, "x", 0.0, 0.0, 0.0, out homMat3DRotate);   // 如果绕固定坐标系旋转，那么中心Y需要取反
            // HOperatorSet.HomMat3dRotateLocal(homMat3D, phi, "x", out homMat3DRotate);   // 如果绕当前坐标系旋转，那么中心Y不需要取反
            HOperatorSet.HomMat3dToPose(homMat3DRotate, out PoseEstimated);
            // 给原始位姿与当前位姿赋值
            if (this.lensType == enLensType.FA镜头)
            {
                camPose = new userCamPose(PoseEstimated);
                double scale = this.cameraParam.CamParam.Height * 1.0 / this.cameraParam.CamParam.Width * 1.0 * 0.9;
                camPose.Tz = Math.Abs(camPose.Tz); // 给位姿赋值正值 / scale
            }
            else
            {
                camPose = new userCamPose(PoseEstimated);
                if (camPose.Tz < 1000)
                    camPose.Tz = 1000;
                else
                    camPose.Tz = Math.Abs(camPose.Tz) * 5;
            }
            this.cameraParam.CamPose = camPose;
        }
        private void InitCamParam()
        {
            HTuple partWidth, scale;
            HTuple partHeight;
            userCamParam camParam = new userCamParam();
            switch (this.lensType)
            {
                case enLensType.FA镜头:
                    partWidth = this.hWindowControl.Width;
                    partHeight = this.hWindowControl.Height;
                    //设置相机参数
                    camParam.CameraModel = "area_scan_division";
                    camParam.Focus = 0.01;
                    camParam.Kappa = 0.0;
                    camParam.Sx = 3.5e-3;
                    camParam.Sy = 3.5e-3;
                    camParam.Cx = partWidth / 2.0;
                    camParam.Cy = partHeight / 2.0;
                    camParam.Width = partWidth.I;
                    camParam.Height = partHeight.I;
                    // HOperatorSet.ResetObjDb(partWidth, partHeight, 0);// 这个方法为全局方法，在整个使用系统中只需要设置一次，通常可以放到初化中，所以在这里不要去设置，这里一设置，其他所有用到窗口的地方参数都会变 // 必需要初始化系统参数,因为halcon内部默认的图像宽度是128 X 128
                    //this.SetImagePartSize(partWidth, partHeight, true); // 这里一定要设置图像部分
                    break;
                /////////////////////////////////////////////////////按远尽镜头来做就不会有畸变了
                case enLensType.远心镜头:
                    scale = this.hWindowControl.Height * 1.0 / this.hWindowControl.Width;
                    partWidth = this.imageWidth; // 保持1:1才不压缩，不拉伸
                    partHeight = (int)(partWidth * scale).D;
                    camParam.CameraModel = "area_scan_telecentric_division";
                    camParam.Focus = 0.5; // 镜头倍率,这里最好不要使用 1
                    camParam.Kappa = 0.0;
                    camParam.Sx = 5e-3;   // 像元大小给1.0表示按1:1比例投影
                    camParam.Sy = 5e-3;  // 像元大小给1.0表示按1:1比例投影
                    camParam.Cx = partWidth / 2.0;
                    camParam.Cy = partHeight / 2.0;
                    camParam.Width = partWidth;
                    camParam.Height = partHeight;
                    // HOperatorSet.ResetObjDb(partWidth, partHeight, 0);// 必需要初始化系统参数,因为halcon内部默认的图像宽度是128 X 128 ; 固定一个512 X 512的区域
                    // this.SetImagePartSize(partWidth, partHeight, true);  // 这里一定要设置图像部分
                    break;
            }
            this.cameraParam.CamParam = camParam;
        }
        /// <summary>
        /// 根据对象直径计算远心镜头倍率，只使用平面上的直径，不使用高度值，因为高度值有可能为很大
        /// </summary>
        /// <param name="objectModel"></param>
        private void InitMagnification(HObjectModel3D[] objectModel)
        {
            HTuple ParamValue;
            double maxDiameter, x_range, y_range;
            if (objectModel == null || objectModel.Length == 0) return;
            int length = objectModel.Length;
            List<double> x_value = new List<double>();
            List<double> y_value = new List<double>();
            if (!objectModel[0].IsInitialized()) return;
            HTuple num = objectModel[0].GetObjectModel3dParams("num_points");
            HTuple primitive = objectModel[0].GetObjectModel3dParams("has_primitive_data");
            if (num.I == 0 && primitive.S == "false") return; //  表示没有点也不是基本体则返回
            ParamValue = HObjectModel3D.GetObjectModel3dParams(objectModel, "bounding_box1"); // 如果是多个对象，获取的是每个对象的值
            if (ParamValue == null || ParamValue.Length == 0 || ParamValue.Length != length * 6) return;
            for (int i = 0; i < length; i++)
            {
                x_value.Add(ParamValue[i * 6].D);
                x_value.Add(ParamValue[i * 6 + 3].D);
                y_value.Add(ParamValue[i * 6 + 1].D);
                y_value.Add(ParamValue[i * 6 + 4].D);
            }
            x_range = (x_value.Max() - x_value.Min());
            y_range = (y_value.Max() - y_value.Min());
            maxDiameter = Math.Sqrt(x_range * x_range + y_range * y_range);
            if (maxDiameter < 1)
                maxDiameter = 1;
            //////////////////////////////////////////////////
            userCamParam camParam = this.cameraParam.CamParam; // 值类型的对象不能夸级访问，即不能通过获取一个值类型对象，再来获取该对象的成员并修改他但可以访问他
            //double minValue = Math.Min(camParam.Width, camParam.Height);
            //camParam.Focus = (minValue * camParam.Sx) / (maxDiameter); // 对于远心镜头，focus：即倍率
            //this.cameraParam.CamParam = camParam;
            camParam.Width = (int)(x_range * 0.5 / this.cameraParam.CamParam.Sx);
            camParam.Height = (int)(y_range * 0.5 / this.cameraParam.CamParam.Sy);
            camParam.Cx = camParam.Width * 0.5;
            camParam.Cy = camParam.Height * 0.5;
            this.cameraParam.CamParam = camParam;
            //////////////////////////
            HTuple Qx = new HTuple(x_value.Min(), x_value.Max(), x_value.Min(), x_value.Max());
            HTuple Qy = new HTuple(y_value.Max(), y_value.Max(), y_value.Min(), y_value.Min());
            HTuple Rows = new HTuple(0, 0, camParam.Height, camParam.Height);
            HTuple Columns = new HTuple(0, camParam.Width, 0, camParam.Width);
            HTuple Quality2;
            HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, camParam.GetHCamPar(), "telecentric_planar_robust", "error", out Quality2);
            double Quality = Quality2[0].D;
            this.cameraParam.CamPose = new userCamPose(pose);
            this.cameraParam.CamPose.Tz = 100;
        }

        private void InitCamPose(HObjectModel3D[] objectModel)
        {
            HTuple ParamValue;
            double maxDiameter, x_range, y_range;
            if (objectModel == null || objectModel.Length == 0) return;
            int length = objectModel.Length;
            List<double> x_value = new List<double>();
            List<double> y_value = new List<double>();
            if (!objectModel[0].IsInitialized()) return;
            HTuple num = objectModel[0].GetObjectModel3dParams("num_points");
            HTuple primitive = objectModel[0].GetObjectModel3dParams("has_primitive_data");
            if (num.I == 0 && primitive.S == "false") return; //  表示没有点也不是基本体则返回
            ParamValue = HObjectModel3D.GetObjectModel3dParams(objectModel, "bounding_box1"); // 如果是多个对象，获取的是每个对象的值
            if (ParamValue == null || ParamValue.Length == 0 || ParamValue.Length != length * 6) return;
            for (int i = 0; i < length; i++)
            {
                x_value.Add(ParamValue[i * 6].D);
                x_value.Add(ParamValue[i * 6 + 3].D);
                y_value.Add(ParamValue[i * 6 + 1].D);
                y_value.Add(ParamValue[i * 6 + 4].D);
            }
            x_range = (x_value.Max() - x_value.Min());
            y_range = (y_value.Max() - y_value.Min());
            maxDiameter = Math.Sqrt(x_range * x_range + y_range * y_range);
            if (maxDiameter < 1)
                maxDiameter = 1;
            //////////////////////////////////////////////////
            userCamParam camParam = new userCamParam();
            if (maxDiameter < 20)
                camParam.Focus = 1;
            else
                camParam.Focus = 0.5;
            double imageWidth = x_range * camParam.Focus / 0.005; // 这里为啥需要乘以 0.5 ？ 因为镜头的倍率设置为 0.5 倍
            double imageHeight = y_range * camParam.Focus / 0.005;
            camParam.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
            //camParam.Focus = 0.5; // 这个值应该为镜头的倍率
            camParam.Sx = 0.005;
            camParam.Sy = 0.005;
            camParam.Cx = imageWidth * 0.5;
            camParam.Cy = imageHeight * 0.5;
            camParam.Width = (int)imageWidth;
            camParam.Height = (int)imageHeight;
            /////////////////////////////
            this.cameraParam.CamParam = camParam;
            this.cameraParam.DataHeight = camParam.Height;
            this.cameraParam.DataWidth = camParam.Width;
            //////////////////////////
            HTuple Qx = new HTuple(x_value.Min(), x_value.Max(), x_value.Min(), x_value.Max());
            HTuple Qy = new HTuple(y_value.Max(), y_value.Max(), y_value.Min(), y_value.Min());
            HTuple Rows = new HTuple(0, 0, camParam.Height, camParam.Height);
            HTuple Columns = new HTuple(0, camParam.Width, 0, camParam.Width);
            HTuple Quality2;
            HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, camParam.GetHCamPar(), "telecentric_planar_robust", "error", out Quality2);
            this.cameraParam.CamPose = new userCamPose(pose);
            this.cameraParam.CamPose.Tz = 200;
            this.PointCloudModel3D.LaserParams.CamParam = camParam;
            this.PointCloudModel3D.LaserParams.CamPose = this.cameraParam.CamPose;
        }




        /// <summary>
        /// 通过改变图像部分，可实现2D、3D的查看,图像的平移、缩放通过 改变图像部来实现，不管2D还是3D，都适用 
        /// </summary>
        /// <param name="Tx"></param>
        /// <param name="Ty"></param>
        private void MoveImage(int Tx, int Ty)
        {
            int row1, col1, row2, col2;
            try
            {
                if (!this.bufferWindow.IsInitialized()) return;
                //移动的本质是改变imagePart的位置坐标
                this.bufferWindow.GetPart(out row1, out col1, out row2, out col2);
                row1 -= Ty;
                col1 -= Tx;
                row2 -= Ty;
                col2 -= Tx;
                this.bufferWindow.SetPart(row1, col1, row2, col2);
                /////////
                this.DrawingGraphicObject();
                //this.CopyBufferWindowView();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("VisualizeView类中->MoveImage:方法报错", ex);
            }
        }
        private void ZoomImage(double currentRow, double currentCol, double scale)
        {
            int row1, col1, row2, col2, lengthC, lengthR;//
            double percentC, percentR;
            try
            {
                if (!this.bufferWindow.IsInitialized()) return;
                ////////////////////////////////
                this.bufferWindow.GetPart(out row1, out col1, out row2, out col2);
                //鼠标位置相对于图像部分的百分比
                percentC = (currentCol - col1) * 1.0 / (col2 - col1) * 1.0;
                percentR = (currentRow - row1) * 1.0 / (row2 - row1) * 1.0;
                //缩放后的ImagePart宽和高
                lengthC = Convert.ToInt32((col2 - col1) * scale);
                lengthR = Convert.ToInt32((row2 - row1) * scale);
                //缩放前的鼠标坐标减去缩放后的鼠标坐标即可以imagepart的起点坐标
                col1 = (int)currentCol - Convert.ToInt32(lengthC * percentC);
                row1 = (int)currentRow - Convert.ToInt32(lengthR * percentR);
                col2 = (int)currentCol + Convert.ToInt32(lengthC * (1 - percentC));
                row2 = (int)currentRow + Convert.ToInt32(lengthR * (1 - percentR));
                //////////////////////////////////////////
                this.bufferWindow.SetPart(row1, col1, row2, col2);
                /////////
                this.DrawingGraphicObject();
                //this.CopyBufferWindowView();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("VisualizeView类中->ZoomImage:方法报错", ex);
            }
        }
        public void DisplayModelObject3D()
        {
            if (this.pointCloudModel3D == null || this.pointCloudModel3D.ObjectModel3D.Length == 0) return;
            try
            {
                if (GlobalVariable.pConfig.ViewType == "colored")  // 当有多个对象时，分别用不同的颜色显示，当只有一个对象时，则用彩虹色显示 this.pointCloudModel3D != null && this.pointCloudModel3D.Length > 10
                {
                    this.GenParamName = new HTuple(this.colored);
                    this.GenParamValue = new HTuple(this.colorNum);
                }
                else
                {
                    HTuple Z_calue = 0;
                    double[] minValue = new double[this.pointCloudModel3D.ObjectModel3D.Length];
                    double[] maxValue = new double[this.pointCloudModel3D.ObjectModel3D.Length];
                    for (int i = 0; i < this.pointCloudModel3D.ObjectModel3D.Length; i++)
                    {
                        Z_calue = this.pointCloudModel3D.ObjectModel3D[i].GetObjectModel3dParams("point_coord_z");
                        minValue[i] = Z_calue.TupleMin();
                        maxValue[i] = Z_calue.TupleMax();
                    }
                    /// 上面获取所有对象中，Z坐标的最大最小值
                    this.paramValue_color_attrib = "coord_z"; // GlobalVariable.pConfig.ColorAttrib; //"coord_z"; // 对应强度变化的值  quality
                    this.paramValue_color_attrib_start = maxValue.Max(); //  设置颜色对应的起始值
                    this.paramValue_color_attrib_end = minValue.Min(); // 设置颜色对应的结束值
                    this.paramValue_depthPersistence = GlobalVariable.pConfig.Depth_persistence.ToString().ToLower();
                    this.paramValue_quality = GlobalVariable.pConfig.PointQuality;
                    this.paramValue_lut = GlobalVariable.pConfig.ViewType;
                    this.paramValue_dispPose = GlobalVariable.pConfig.IsShowCoordSys.ToString().ToLower();
                    //////////////////////////////////
                    this.GenParamName = new HTuple(this.paramName_quality, this.paramName_lut, this.paramName_dispPose, this.paramName_alpha,
                    this.paramName_color_attrib, this.paramName_color_attrib_start, this.paramName_color_attrib_end, this.paramName_depthPersistence);
                    this.GenParamValue = new HTuple(this.paramValue_quality, this.paramValue_lut, this.paramValue_dispPose, this.paramValue_alpha,
                    this.paramValue_color_attrib, this.paramValue_color_attrib_start, this.paramValue_color_attrib_end, this.paramValue_depthPersistence);
                }
                HObjectModel3D.DispObjectModel3d(this.bufferWindow, this.pointCloudModel3D.ObjectModel3D, new HCamPar(this.cameraParam.CamParam.GetHtuple()), new HPose[1] { new HPose(this.cameraParam.CamPose.GetHtuple()) }, this.GenParamName, this.GenParamValue);
                // 显示传入的对象
                ShowAttachProperty(); // 因为显示3D对象使用了线程，所以这里只能放到方法时面
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("DisplayModelObject3D", ex);
            }
        }
        public void DisplayImageData()
        {
            if (this.backImage != null && this.backImage.Image != null && this.backImage.Image.IsInitialized())// 对象不为空，且指针也不为空
                this.bufferWindow.DispObj(this.backImage.Image);
            // 显示传入的对象
            this.bufferWindow.SetColor("green");
            ShowAttachProperty(); // 因为显示3D对象使用了线程，所以这里只能放到方法时面
        }
        public void DisplayXldData()
        {
            this.bufferWindow.SetColored(12);
            if (this.backImage != null && this.backImage.Image.IsInitialized())//
                this.bufferWindow.DispObj(this.backImage.Image);
            ////////////////////////
            //for (int i = 0; i < this.xldContourData.Length; i++)
            //{
            if (this.xldContourData != null && this.xldContourData.HXldCont != null && this.xldContourData.HXldCont.IsInitialized())
                this.bufferWindow.DispObj(this.xldContourData.HXldCont);
            //}
            // 显示传入的对象
            //ShowAttachProperty(); // 因为显示3D对象使用了线程，所以这里只能放到方法时面  xldContourData本身即为图像数据 
        }
        public void DisplayRegionData()
        {
            this.bufferWindow.SetColored(12);
            if (this.backImage != null && this.backImage.Image.IsInitialized())//
                this.bufferWindow.DispObj(this.backImage.Image);
            ////////////////////////
            //for (int i = 0; i < this.regionData.Length; i++)
            //{
            if (this.regionData != null)
            {
                this.cameraParam = this.regionData.CamParams;
                SetImagePartSize(this.cameraParam.CamParam.Width, this.cameraParam.CamParam.Height, false);
                if (this.regionData.Region.IsInitialized())
                    this.bufferWindow.DispObj(this.regionData.Region);
            }
            //}
            // 显示传入的对象
            //ShowAttachProperty(); // 因为显示3D对象使用了线程，所以这里只能放到方法时面  xldContourData本身即为图像数据 
        }
        private void DetermineOptimumPoseDistance(HObjectModel3D[] ObjectModel3DID, HTuple CamParam, HTuple ImageCoverage, HTuple PoseIn, out HTuple PoseOut)
        {
            HTuple hv_NumModels = null, hv_Rows = null;
            HTuple hv_Cols = null, hv_MinMinZ = null, hv_BB = null;
            HTuple hv_Seq = null, hv_DXMax = null, hv_DYMax = null;
            HTuple hv_DZMax = null, hv_Diameter = null, hv_ZAdd = null;
            HTuple hv_IBB = null, hv_BB0 = null, hv_BB1 = null, hv_BB2 = null;
            HTuple hv_BB3 = null, hv_BB4 = null, hv_BB5 = null, hv_X = null;
            HTuple hv_Y = null, hv_Z = null, hv_PoseInter = null, hv_HomMat3D = null;
            HTuple hv_CX = null, hv_CY = null, hv_CZ = null, hv_DR = null;
            HTuple hv_DC = null, hv_MaxDist = null, hv_HomMat3DRotate = new HTuple();
            HTuple hv_MinImageSize = null, hv_Zs = null, hv_ZDiff = null;
            HTuple hv_ScaleZ = null, hv_ZNew = null;
            //
            hv_NumModels = new HTuple(ObjectModel3DID.Length);
            hv_Rows = new HTuple();
            hv_Cols = new HTuple();
            hv_MinMinZ = 1e30;
            hv_BB = HObjectModel3D.GetObjectModel3dParams(ObjectModel3DID, "bounding_box1");
            //Calculate diameter over all objects to be visualized
            hv_Seq = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_DXMax = (((hv_BB.TupleSelect(hv_Seq + 3))).TupleMax()) - (((hv_BB.TupleSelect(hv_Seq))).TupleMin());
            hv_DYMax = (((hv_BB.TupleSelect(hv_Seq + 4))).TupleMax()) - (((hv_BB.TupleSelect(hv_Seq + 1))).TupleMin());
            hv_DZMax = (((hv_BB.TupleSelect(hv_Seq + 5))).TupleMax()) - (((hv_BB.TupleSelect(hv_Seq + 2))).TupleMin());
            hv_Diameter = ((((hv_DXMax * hv_DXMax) + (hv_DYMax * hv_DYMax)) + (hv_DZMax * hv_DZMax))).TupleSqrt(); // 这里是否需要考虑Z方向的高度？当高度非常大时，这处很有必要 
            //hv_Diameter = ((((hv_DXMax * hv_DXMax) + (hv_DYMax * hv_DYMax)) + (0 * 0))).TupleSqrt();
            if ((int)(new HTuple(((((hv_BB.TupleAbs())).TupleSum())).TupleEqual(0.0))) != 0)
            {
                hv_BB = new HTuple();
                hv_BB = hv_BB.TupleConcat(-((new HTuple(HTuple.TupleRand(3) * 1e-20)).TupleAbs()));
                hv_BB = hv_BB.TupleConcat((new HTuple(HTuple.TupleRand(3) * 1e-20)).TupleAbs());
            }
            hv_ZAdd = 0.0;
            if ((int)(new HTuple(((hv_Diameter.TupleMax())).TupleLess(1e-10))) != 0)
            {
                hv_ZAdd = 0.01;
            }
            if ((int)(new HTuple(((hv_Diameter.TupleMin())).TupleLess(1e-10))) != 0)
            {
                hv_Diameter = hv_Diameter - (((((((hv_Diameter - 1e-10)).TupleSgn()) - 1)).TupleSgn()) * 1e-10);
            }
            hv_IBB = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_BB0 = hv_BB.TupleSelect(hv_IBB);     // 对象的中心X
            hv_BB1 = hv_BB.TupleSelect(hv_IBB + 1); // 对象的中心Y
            hv_BB2 = hv_BB.TupleSelect(hv_IBB + 2); // 对象的中心Z
            hv_BB3 = hv_BB.TupleSelect(hv_IBB + 3); // 对象的宽
            hv_BB4 = hv_BB.TupleSelect(hv_IBB + 4); // 对象的高
            hv_BB5 = hv_BB.TupleSelect(hv_IBB + 5); // 对象的Z方向高度
            hv_X = new HTuple();
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_Y = new HTuple();
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Z = new HTuple();
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_PoseInter = PoseIn.TupleReplace(2, (-(hv_Z.TupleMin())) + (2 * (hv_Diameter.TupleMax())));
            HOperatorSet.PoseToHomMat3d(hv_PoseInter, out hv_HomMat3D);
            //Determine the maximum extention of the projection
            HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_X, hv_Y, hv_Z, out hv_CX, out hv_CY, out hv_CZ);
            HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, CamParam, out hv_Rows, out hv_Cols);
            hv_MinMinZ = hv_CZ.TupleMin();
            hv_DR = hv_Rows - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 3));
            hv_DC = hv_Cols - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 4));
            hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
            hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
            hv_MaxDist = (((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt();
            //
            if ((int)(new HTuple(hv_MaxDist.TupleLess(1e-10))) != 0)
            {
                HOperatorSet.HomMat3dRotateLocal(hv_HomMat3D, (new HTuple(90)).TupleRad(), "x", out hv_HomMat3DRotate);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3DRotate, hv_X, hv_Y, hv_Z, out hv_CX, out hv_CY, out hv_CZ);
                HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, CamParam, out hv_Rows, out hv_Cols);
                hv_DR = hv_Rows - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength()
                    )) - 3));
                hv_DC = hv_Cols - (CamParam.TupleSelect((new HTuple(CamParam.TupleLength()
                    )) - 4));
                hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
                hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
                hv_MaxDist = ((hv_MaxDist.TupleConcat((((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt()
                    ))).TupleMax();
            }
            //
            hv_MinImageSize = ((((CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 2))).TupleConcat(CamParam.TupleSelect((new HTuple(CamParam.TupleLength())) - 1)))).TupleMin();
            //
            hv_Z = hv_PoseInter[2];
            hv_Zs = hv_MinMinZ.Clone();
            hv_ZDiff = hv_Z - hv_Zs;
            hv_ScaleZ = hv_MaxDist / (((0.5 * hv_MinImageSize) * ImageCoverage) * 2.0);
            hv_ZNew = ((hv_ScaleZ * hv_Zs) + hv_ZDiff) + hv_ZAdd;
            PoseOut = hv_PoseInter.TupleReplace(2, hv_ZNew);
        }
        private void GetObjectModelsCenter(HObjectModel3D[] hv_ObjectModel3DID, out HTuple hv_Center)
        {
            HTuple hv_Diameter = new HTuple(), hv_MD = new HTuple();
            HTuple hv_Weight = new HTuple(), hv_SumW = new HTuple();
            HTuple hv_Index = new HTuple();
            // HObjectModel3D hv_ObjectModel3DIDSelected = null;
            HTuple hv_C = new HTuple(), hv_InvSum = new HTuple();
            // Initialize local and output iconic variables 
            hv_Center = new HTuple();
            //HObjectModel3D unionObjectModel3D = null;
            // Compute the mean of all model centers (weighted by the diameter of the object models)
            if (hv_ObjectModel3DID != null && hv_ObjectModel3DID.Length > 0)
            {
                //unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(hv_ObjectModel3DID, "points_surface"); // 这里是否需要合并?
                //hv_Diameter = unionObjectModel3D.GetObjectModel3dParams("diameter_axis_aligned_bounding_box");
                hv_Diameter = HObjectModel3D.GetObjectModel3dParams(hv_ObjectModel3DID, "diameter_axis_aligned_bounding_box");
                // Normalize Diameter to use it as weights for a weighted mean of the individual centers
                hv_MD = hv_Diameter.TupleMean();
                if ((int)(new HTuple(hv_MD.TupleGreater(1e-10))) != 0)
                {
                    hv_Weight = hv_Diameter / hv_MD;
                }
                else
                {
                    hv_Weight = hv_Diameter.Clone();
                }
                hv_SumW = hv_Weight.TupleSum();
                if ((int)(new HTuple(hv_SumW.TupleLess(1e-10))) != 0)
                {
                    hv_Weight = HTuple.TupleGenConst(new HTuple(hv_Weight.TupleLength()), 1.0);
                    hv_SumW = hv_Weight.TupleSum();
                }
                hv_Center = new HTuple();
                hv_Center[0] = 0;
                hv_Center[1] = 0;
                hv_Center[2] = 0;
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ObjectModel3DID.Length)) - 1); hv_Index = (int)hv_Index + 1)
                {
                    // hv_ObjectModel3DIDSelected = hv_ObjectModel3DID[hv_Index];
                    hv_C = hv_ObjectModel3DID[hv_Index].GetObjectModel3dParams("center");
                    //HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DIDSelected, "center", out hv_C);
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[0] = (hv_Center.TupleSelect(0)) + ((hv_C.TupleSelect(0)) * (hv_Weight.TupleSelect(hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[1] = (hv_Center.TupleSelect(1)) + ((hv_C.TupleSelect(1)) * (hv_Weight.TupleSelect(hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[2] = (hv_Center.TupleSelect(2)) + ((hv_C.TupleSelect(2)) * (hv_Weight.TupleSelect(hv_Index)));
                }
                hv_InvSum = 1.0 / hv_SumW;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[0] = (hv_Center.TupleSelect(0)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[1] = (hv_Center.TupleSelect(1)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[2] = (hv_Center.TupleSelect(2)) * hv_InvSum;
            }
            else
            {
                hv_Center = new HTuple();
            }
            return;
        }

        public virtual void DetachDrawingObjectFromWindow()
        {
            try
            {
                this.CurrentButton = MouseButtons.None;
                this.isTranslate = true;// 在非绘图模式下移动图像 
                // this.isd = false;
                this.isDrawMode = false;
                this.bufferWindow.SetColor("green");
                this.attachDrawingPropertyData.Clear();
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
            //this.isDispalyAttachDrawingProperty = true;
            this.isDrawMode = true;
            this.bufferWindow.SetColor("red");
            this.bufferWindow.SetLineWidth(2);
            DrawingGraphicObject();
        }
        public virtual void SetParam(object param)
        {
            throw new NotImplementedException();
        }
        private HObjectModel3D TransformXldToPointCloud(XldDataClass xld)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            HTuple row, col;
            //////////////////////
            if (xld == null) { return new HObjectModel3D(); }
            int unm = xld.HXldCont.CountObj();
            for (int i = 1; i < unm; i++)
            {
                if (xld.HXldCont.SelectObj(i) == null) continue;
                xld.HXldCont.SelectObj(i).GetContourXld(out row, out col);
                if (row != null && row.Length > 0)
                {
                    x.AddRange(col.ToDArr());
                    y.AddRange(row.ToDArr());
                }
            }
            if (x.Count != 0)
                return new HObjectModel3D(new HTuple(x.ToArray()), new HTuple(y.ToArray()), HTuple.TupleGenConst(x.Count, 0));
            else
                return new HObjectModel3D();
        }
        private void getXldImagePart(XldDataClass xld)
        {
            //for (int i = 0; i < xld.Length; i++)
            //{
            if (xld != null && xld.CamParams != null)
            {
                this.cameraParam = xld.CamParams;
                if (this.bufferWindow != null)
                    this.bufferWindow.SetPart(0, 0, (int)(this.cameraParam.DataHeight - 1), (int)(this.cameraParam.DataWidth - 1)); // 设置窗口的图像部分与图像的大小相等 
            }
            // }
        }
        private void getRegionImagePart(RegionDataClass region)
        {
            //for (int i = 0; i < region.Length; i++)
            //{
            if (region != null && region.CamParams != null)
            {
                this.cameraParam = region.CamParams;
                this.bufferWindow.SetPart(0, 0, (int)(this.cameraParam.DataHeight - 1), (int)(this.cameraParam.DataWidth - 1));
                //SetImagePartSize(this.cameraParam.DataWidth, this.cameraParam.DataHeight, false);
            }
            // }
            //if (this.bufferWindow != null)
            //    this.bufferWindow.SetPart(0, 0, (int)(this.cameraParam.CamParam.Height - 1), (int)(this.cameraParam.CamParam.Width - 1)); // 设置窗口的图像部分与图像的大小相等 
        }
        public void Show3D()
        {
            HTuple object3D = new HTuple();
            HTuple pose = null;
            try
            {
                this.lensType = enLensType.FA镜头;
                //InitCamParam();
                //InitCamPose(this.pointCloudModel3D);
                ///////// 显示3D对象模型 ////////  
                // 这句指令一定要，但是这是什么意思了？ Push
                HOperatorSet.SetWindowAttr("background_color", "black");
                HDevWindowStack.Push(this.hWindowControl.HalconWindow);
                set_display_font(this.hWindowControl.HalconWindow, 16, "mono", "true", "false");
                ///////////////////
                if (this.pointCloudModel3D == null) return;
                for (int i = 0; i < this.pointCloudModel3D.ObjectModel3D.Length; i++)
                {
                    object3D.Append(new HHandle(this.pointCloudModel3D.ObjectModel3D[i].Handle));
                }
                if (GlobalVariable.pConfig.ViewType == "colored")  // 当有多个对象时，分别用不同的颜色显示，当只有一个对象时，则用彩虹色显示  object3D != null && object3D.Length > 1
                {
                    this.Instructions = new HTuple(this.instructions_Rotate, this.instructions_Zoom, this.instructions_Move);
                    this.GenParamName = new HTuple(this.colored);
                    this.GenParamValue = new HTuple(this.colorNum);
                }
                else
                {
                    HTuple Z_calue = 0;
                    double[] minValue = new double[this.pointCloudModel3D.ObjectModel3D.Length];
                    double[] maxValue = new double[this.pointCloudModel3D.ObjectModel3D.Length];
                    for (int i = 0; i < this.pointCloudModel3D.ObjectModel3D.Length; i++)
                    {
                        Z_calue = this.pointCloudModel3D.ObjectModel3D[i].GetObjectModel3dParams("point_coord_z");
                        minValue[i] = Z_calue.TupleMin();
                        maxValue[i] = Z_calue.TupleMax();
                    }
                    /// 上面获取所有对象中，Z坐标的最大最小值
                    this.paramValue_color_attrib = "coord_z"; // GlobalVariable.pConfig.ColorAttrib; //"coord_z"; // 对应强度变化的值  quality
                    this.paramValue_color_attrib_start = maxValue.Max(); //  设置颜色地应的起始值
                    this.paramValue_color_attrib_end = minValue.Min(); // 设置颜色地应的结束值
                    this.paramValue_depthPersistence = GlobalVariable.pConfig.Depth_persistence.ToString().ToLower();
                    this.paramValue_quality = GlobalVariable.pConfig.PointQuality;
                    this.paramValue_lut = GlobalVariable.pConfig.ViewType;
                    this.paramValue_dispPose = GlobalVariable.pConfig.IsShowCoordSys.ToString().ToLower();
                    //////////////////////////////////
                    this.GenParamName = new HTuple(this.paramName_quality, this.paramName_lut, this.paramName_dispPose, this.paramName_alpha,
                        this.paramName_color_attrib, this.paramName_color_attrib_start, this.paramName_color_attrib_end, this.paramName_depthPersistence);
                    this.GenParamValue = new HTuple(this.paramValue_quality, this.paramValue_lut, this.paramValue_dispPose, this.paramValue_alpha,
                        this.paramValue_color_attrib, this.paramValue_color_attrib_start, this.paramValue_color_attrib_end, this.paramValue_depthPersistence);
                    this.Instructions = new HTuple(this.instructions_Rotate, this.instructions_Zoom, this.instructions_Move);

                    //this.GenParamName = new HTuple(this.paramName_quality, this.paramName_lut, this.paramName_dispPose, this.paramName_alpha, this.paramName_color_attrib, this.paramName_depthPersistence);
                    //this.GenParamValue = new HTuple(this.paramValue_quality, this.paramValue_lut, this.paramValue_dispPose, this.paramValue_alpha, this.paramValue_color_attrib, this.paramValue_depthPersistence);
                }
                // 独占线程会造成死循环,要实现缩放，则相机参数必需为FA类型 this.cameraParam.CamParam.GetHtuple()this.cameraParam.CamPose.getHtuple()  这里使用默认的相机内参和外参
                visualize_object_model_3d(this.hWindowControl.HalconWindow, object3D, new HTuple(), new HTuple(), this.GenParamName, this.GenParamValue, new HTuple(), new HTuple(), this.Instructions, out pose);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Show3D:方法报错", ex);
            }
            finally
            {
                object3D = null;
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



        #region // Halcon导出部分
        private HTuple gInfoDecor;
        private HTuple gInfoPos;
        private HTuple gTitlePos;
        private HTuple gTitleDecor;
        private HTuple gIsSinglePose;
        private HTuple gDispObjOffset;
        private HTuple gLabelsDecor;
        private HTuple gTerminationButtonLabel;
        private HTuple gAlphaDeselected;
        private HTuple gUsesOpenGL;
        private HTuple ExpGetGlobalVar_gInfoDecor()
        {
            return gInfoDecor;
        }
        private void ExpSetGlobalVar_gInfoDecor(HTuple val)
        {
            gInfoDecor = val;
        }

        private HTuple ExpGetGlobalVar_gInfoPos()
        {
            return gInfoPos;
        }
        private void ExpSetGlobalVar_gInfoPos(HTuple val)
        {
            gInfoPos = val;
        }

        private HTuple ExpGetGlobalVar_gTitlePos()
        {
            return gTitlePos;
        }
        private void ExpSetGlobalVar_gTitlePos(HTuple val)
        {
            gTitlePos = val;
        }

        private HTuple ExpGetGlobalVar_gTitleDecor()
        {
            return gTitleDecor;
        }
        private void ExpSetGlobalVar_gTitleDecor(HTuple val)
        {
            gTitleDecor = val;
        }

        private HTuple ExpGetGlobalVar_gIsSinglePose()
        {
            return gIsSinglePose;
        }
        private void ExpSetGlobalVar_gIsSinglePose(HTuple val)
        {
            gIsSinglePose = val;
        }

        private HTuple ExpGetGlobalVar_gDispObjOffset()
        {
            return gDispObjOffset;
        }
        private void ExpSetGlobalVar_gDispObjOffset(HTuple val)
        {
            gDispObjOffset = val;
        }

        private HTuple ExpGetGlobalVar_gLabelsDecor()
        {
            return gLabelsDecor;
        }
        private void ExpSetGlobalVar_gLabelsDecor(HTuple val)
        {
            gLabelsDecor = val;
        }

        private HTuple ExpGetGlobalVar_gTerminationButtonLabel()
        {
            return gTerminationButtonLabel;
        }
        private void ExpSetGlobalVar_gTerminationButtonLabel(HTuple val)
        {
            gTerminationButtonLabel = val;
        }

        private HTuple ExpGetGlobalVar_gAlphaDeselected()
        {
            return gAlphaDeselected;
        }
        private void ExpSetGlobalVar_gAlphaDeselected(HTuple val)
        {
            gAlphaDeselected = val;
        }

        private HTuple ExpGetGlobalVar_gUsesOpenGL()
        {
            return gUsesOpenGL;
        }
        private void ExpSetGlobalVar_gUsesOpenGL(HTuple val)
        {
            gUsesOpenGL = val;
        }


        private void dev_update_off()
        {

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: This procedure writes a text message. 
        private void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_MaxAscent = null;
            HTuple hv_MaxDescent = null, hv_MaxWidth = null, hv_MaxHeight = null;
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRow = new HTuple();
            HTuple hv_FactorColumn = new HTuple(), hv_UseShadow = null;
            HTuple hv_ShadowColor = null, hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //Transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //Display text box depending on text size
            hv_UseShadow = 1;
            hv_ShadowColor = "gray";
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
            {
                if (hv_Box_COPY_INP_TMP == null)
                    hv_Box_COPY_INP_TMP = new HTuple();
                hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                hv_ShadowColor = "#f28d26";
            }
            if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                {
                    //Use default ShadowColor set above
                }
                else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                    "false"))) != 0)
                {
                    hv_UseShadow = 0;
                }
                else
                {
                    hv_ShadowColor = hv_Box_COPY_INP_TMP[1];
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(1));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)";
                        throw new HalconException(hv_Exception);
                    }
                }
            }
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
            {
                //Valid color?
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                //Calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //Display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                //Set shadow color
                HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                if ((int)(hv_UseShadow) != 0)
                {
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1, hv_C2 + 1);
                }
                //Set box color
                HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //Reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: Set font independent of OS 
        private void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //It is assumed that following fonts are installed on the system:
            //Windows: Courier New, Arial Times New Roman
            //Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
            //Linux: courier, helvetica, times
            //Because fonts are displayed smaller on Linux than on Windows,
            //a scaling factor of 1.25 is used the get comparable results.
            //For Linux, only a limited number of font sizes is supported,
            //to get comparable results, it is recommended to use one of the
            //following sizes: 9, 11, 14, 16, 20, 27
            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Set font on Windows systems
                try
                {
                    //Check, if font scaling is switched on
                    HOperatorSet.OpenWindow(0, 0, 256, 256, 0, "buffer", "", out hv_BufferWindowHandle);
                    HOperatorSet.SetFont(hv_BufferWindowHandle, "-Consolas-16-*-0-*-*-1-");
                    HOperatorSet.GetStringExtents(hv_BufferWindowHandle, "test_string", out hv_Ascent,
                        out hv_Descent, out hv_Width, out hv_Height);
                    //Expected width is 110
                    hv_Scale = 110.0 / hv_Width;
                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
                    HOperatorSet.CloseWindow(hv_BufferWindowHandle);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Consolas";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Dar"))) != 0)
            {
                //Set font on Mac OS X systems. Since OS X does not have a strict naming
                //scheme for font attributes, we use tables to determine the correct font
                //name.
                hv_SubFamily = 0;
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(1);
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(2);
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Menlo-Regular";
                    hv_Fonts[1] = "Menlo-Italic";
                    hv_Fonts[2] = "Menlo-Bold";
                    hv_Fonts[3] = "Menlo-BoldItalic";
                }
                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "CourierNewPSMT";
                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
                    hv_Fonts[2] = "CourierNewPS-BoldMT";
                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "ArialMT";
                    hv_Fonts[1] = "Arial-ItalicMT";
                    hv_Fonts[2] = "Arial-BoldMT";
                    hv_Fonts[3] = "Arial-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "TimesNewRomanPSMT";
                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
                }
                else
                {
                    //Attempt to figure out which of the fonts installed on the system
                    //the user could have meant.
                    HOperatorSet.QueryFont(hv_WindowHandle, out hv_SystemFonts);
                    hv_Fonts = new HTuple();
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                }
                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else
            {
                //Set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(((hv_OS.TupleSubstr(0, 4))).TupleEqual("Linux"))).TupleAnd(
                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                    {
                        HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
                            hv_FontSelRegexp);
                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
                            0))) != 0)
                        {
                            hv_Exception = "Wrong font name";
                            //throw (Exception)
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetFont(hv_WindowHandle, (((hv_FontsCourier.TupleSelect(
                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException2)
                            {
                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                //throw (Exception)
                            }
                        }
                    }
                    //throw (Exception)
                }
            }
            // dev_set_preferences(...); only in hdevelop

            return;
        }

        // Chapter: Graphics / Output
        private void disp_title_and_information(HTuple hv_WindowHandle, HTuple hv_Title,
            HTuple hv_Information)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_WinRow = null, hv_WinColumn = null;
            HTuple hv_WinWidth = null, hv_WinHeight = null, hv_NumTitleLines = null;
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_TextWidth = new HTuple(), hv_NumInfoLines = null;
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Information_COPY_INP_TMP = hv_Information.Clone();
            HTuple hv_Title_COPY_INP_TMP = hv_Title.Clone();

            // Initialize local and output iconic variables 
            //global tuple gInfoDecor
            //global tuple gInfoPos
            //global tuple gTitlePos
            //global tuple gTitleDecor
            //
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_WinRow, out hv_WinColumn,
                out hv_WinWidth, out hv_WinHeight);
            hv_Title_COPY_INP_TMP = ((("" + hv_Title_COPY_INP_TMP) + "")).TupleSplit("\n");
            hv_NumTitleLines = new HTuple(hv_Title_COPY_INP_TMP.TupleLength());
            if ((int)(new HTuple(hv_NumTitleLines.TupleGreater(0))) != 0)
            {
                hv_Row = 12;
                if ((int)(new HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual("UpperLeft"))) != 0)
                {
                    hv_Column = 12;
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual("UpperCenter"))) != 0)
                {
                    max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP, out hv_TextWidth);
                    hv_Column = (hv_WinWidth / 2) - (hv_TextWidth / 2);
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual("UpperRight"))) != 0)
                {
                    if ((int)(new HTuple(((ExpGetGlobalVar_gTitleDecor().TupleSelect(1))).TupleEqual(
                        "true"))) != 0)
                    {
                        max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP + "  ", out hv_TextWidth);
                    }
                    else
                    {
                        max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP, out hv_TextWidth);
                    }
                    hv_Column = (hv_WinWidth - hv_TextWidth) - 10;
                }
                else
                {
                    //Unknown position!
                    // stop(); only in hdevelop
                }
                disp_message(hv_WindowHandle, hv_Title_COPY_INP_TMP, "window", hv_Row, hv_Column,
                    ExpGetGlobalVar_gTitleDecor().TupleSelect(0), ExpGetGlobalVar_gTitleDecor().TupleSelect(
                    1));
            }
            hv_Information_COPY_INP_TMP = ((("" + hv_Information_COPY_INP_TMP) + "")).TupleSplit(
                "\n");
            hv_NumInfoLines = new HTuple(hv_Information_COPY_INP_TMP.TupleLength());
            if ((int)(new HTuple(hv_NumInfoLines.TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual("UpperLeft"))) != 0)
                {
                    hv_Row = 12;
                    hv_Column = 12;
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual("UpperRight"))) != 0)
                {
                    if ((int)(new HTuple(((ExpGetGlobalVar_gInfoDecor().TupleSelect(1))).TupleEqual(
                        "true"))) != 0)
                    {
                        max_line_width(hv_WindowHandle, hv_Information_COPY_INP_TMP + "  ", out hv_TextWidth);
                    }
                    else
                    {
                        max_line_width(hv_WindowHandle, hv_Information_COPY_INP_TMP, out hv_TextWidth);
                    }
                    hv_Row = 12;
                    hv_Column = (hv_WinWidth - hv_TextWidth) - 12;
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual("LowerLeft"))) != 0)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_Information_COPY_INP_TMP,
                        out hv_Ascent, out hv_Descent, out hv_Width, out hv_Height);
                    hv_Row = (hv_WinHeight - (hv_NumInfoLines * hv_Height)) - 12;
                    hv_Column = 12;
                }
                else
                {
                    //Unknown position!
                    // stop(); only in hdevelop
                }
                disp_message(hv_WindowHandle, hv_Information_COPY_INP_TMP, "window", hv_Row,
                    hv_Column, ExpGetGlobalVar_gInfoDecor().TupleSelect(0), ExpGetGlobalVar_gInfoDecor().TupleSelect(
                    1));
            }
            //

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Can replace disp_object_model_3d if there is no OpenGL available. 
        private void disp_object_model_no_opengl(out HObject ho_ModelContours, HTuple hv_ObjectModel3DID,
            HTuple hv_GenParamName, HTuple hv_GenParamValue, HTuple hv_WindowHandleBuffer,
            HTuple hv_CamParam, HTuple hv_PosesOut)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Idx = null, hv_CustomParamName = new HTuple();
            HTuple hv_CustomParamValue = new HTuple(), hv_Font = null;
            HTuple hv_IndicesDispBackGround = null, hv_Indices = new HTuple();
            HTuple hv_HasPolygons = null, hv_HasTri = null, hv_HasPoints = null;
            HTuple hv_HasLines = null, hv_NumPoints = null, hv_IsPrimitive = null;
            HTuple hv_Center = null, hv_Diameter = null, hv_OpenGlHiddenSurface = null;
            HTuple hv_CenterX = null, hv_CenterY = null, hv_CenterZ = null;
            HTuple hv_PosObjectsZ = null, hv_I = new HTuple(), hv_Pose = new HTuple();
            HTuple hv_HomMat3DObj = new HTuple(), hv_PosObjCenterX = new HTuple();
            HTuple hv_PosObjCenterY = new HTuple(), hv_PosObjCenterZ = new HTuple();
            HTuple hv_PosObjectsX = new HTuple(), hv_PosObjectsY = new HTuple();
            HTuple hv_Color = null, hv_Indices1 = new HTuple(), hv_IndicesIntensities = new HTuple();
            HTuple hv_Indices2 = new HTuple(), hv_J = null, hv_Indices3 = new HTuple();
            HTuple hv_HomMat3D = new HTuple(), hv_SampledObjectModel3D = new HTuple();
            HTuple hv_X = new HTuple(), hv_Y = new HTuple(), hv_Z = new HTuple();
            HTuple hv_HomMat3D1 = new HTuple(), hv_Qx = new HTuple();
            HTuple hv_Qy = new HTuple(), hv_Qz = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_ObjectModel3DConvexHull = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            //This procedure allows to use project_object_model_3d to simulate a disp_object_model_3d
            //call for small objects. Large objects are sampled down to display.
            hv_Idx = hv_GenParamName.TupleFind("point_size");
            if ((int)((new HTuple(hv_Idx.TupleLength())).TupleAnd(new HTuple(hv_Idx.TupleNotEqual(
                -1)))) != 0)
            {
                hv_CustomParamName = "point_size";
                hv_CustomParamValue = hv_GenParamValue.TupleSelect(hv_Idx);
                if ((int)(new HTuple(hv_CustomParamValue.TupleEqual(1))) != 0)
                {
                    hv_CustomParamValue = 0;
                }
            }
            else
            {
                hv_CustomParamName = new HTuple();
                hv_CustomParamValue = new HTuple();
            }
            HOperatorSet.GetFont(hv_WindowHandleBuffer, out hv_Font);
            HOperatorSet.TupleFind(hv_GenParamName, "disp_background", out hv_IndicesDispBackGround);
            if ((int)(new HTuple(hv_IndicesDispBackGround.TupleNotEqual(-1))) != 0)
            {
                HOperatorSet.TupleFind(hv_GenParamName.TupleSelect(hv_IndicesDispBackGround),
                    "false", out hv_Indices);
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.ClearWindow(hv_WindowHandleBuffer);
                }
            }
            set_display_font(hv_WindowHandleBuffer, 11, "mono", "false", "false");
            disp_message(hv_WindowHandleBuffer, "OpenGL missing!", "image", 5, (hv_CamParam.TupleSelect(
                6)) - 130, "red", "false");
            HOperatorSet.SetFont(hv_WindowHandleBuffer, hv_Font);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_polygons", out hv_HasPolygons);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_triangles", out hv_HasTri);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_points", out hv_HasPoints);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_lines", out hv_HasLines);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "num_points", out hv_NumPoints);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_primitive_data",
                out hv_IsPrimitive);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "center", out hv_Center);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "diameter", out hv_Diameter);
            HOperatorSet.GetSystem("opengl_hidden_surface_removal_enable", out hv_OpenGlHiddenSurface);
            HOperatorSet.SetSystem("opengl_hidden_surface_removal_enable", "false");
            //Sort the objects by inverse z
            hv_CenterX = hv_Center[HTuple.TupleGenSequence(0, (new HTuple(hv_Center.TupleLength()
                )) - 1, 3)];
            hv_CenterY = hv_Center[HTuple.TupleGenSequence(0, (new HTuple(hv_Center.TupleLength()
                )) - 1, 3) + 1];
            hv_CenterZ = hv_Center[HTuple.TupleGenSequence(0, (new HTuple(hv_Center.TupleLength()
                )) - 1, 3) + 2];
            hv_PosObjectsZ = new HTuple();
            if ((int)(new HTuple((new HTuple(hv_PosesOut.TupleLength())).TupleGreater(7))) != 0)
            {
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_Pose = hv_PosesOut.TupleSelectRange(hv_I * 7, (hv_I * 7) + 6);
                    HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3DObj);
                    HOperatorSet.AffineTransPoint3d(hv_HomMat3DObj, hv_CenterX.TupleSelect(hv_I),
                        hv_CenterY.TupleSelect(hv_I), hv_CenterZ.TupleSelect(hv_I), out hv_PosObjCenterX,
                        out hv_PosObjCenterY, out hv_PosObjCenterZ);
                    hv_PosObjectsZ = hv_PosObjectsZ.TupleConcat(hv_PosObjCenterZ);
                }
            }
            else
            {
                hv_Pose = hv_PosesOut.TupleSelectRange(0, 6);
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3DObj);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3DObj, hv_CenterX, hv_CenterY, hv_CenterZ,
                    out hv_PosObjectsX, out hv_PosObjectsY, out hv_PosObjectsZ);
            }
            hv_Idx = ((hv_PosObjectsZ.TupleSortIndex())).TupleInverse();
            hv_Color = "white";
            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color);
            if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleGreater(
                0))) != 0)
            {
                HOperatorSet.TupleFind(hv_GenParamName, "colored", out hv_Indices1);
                HOperatorSet.TupleFind(hv_GenParamName, "intensity", out hv_IndicesIntensities);
                HOperatorSet.TupleFind(hv_GenParamName, "color", out hv_Indices2);
                if ((int)(new HTuple(((hv_Indices1.TupleSelect(0))).TupleNotEqual(-1))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect(
                        0)))).TupleEqual(3))) != 0)
                    {
                        hv_Color = new HTuple();
                        hv_Color[0] = "red";
                        hv_Color[1] = "green";
                        hv_Color[2] = "blue";
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect(
                        0)))).TupleEqual(6))) != 0)
                    {
                        hv_Color = new HTuple();
                        hv_Color[0] = "red";
                        hv_Color[1] = "green";
                        hv_Color[2] = "blue";
                        hv_Color[3] = "cyan";
                        hv_Color[4] = "magenta";
                        hv_Color[5] = "yellow";
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect(
                        0)))).TupleEqual(12))) != 0)
                    {
                        hv_Color = new HTuple();
                        hv_Color[0] = "red";
                        hv_Color[1] = "green";
                        hv_Color[2] = "blue";
                        hv_Color[3] = "cyan";
                        hv_Color[4] = "magenta";
                        hv_Color[5] = "yellow";
                        hv_Color[6] = "coral";
                        hv_Color[7] = "slate blue";
                        hv_Color[8] = "spring green";
                        hv_Color[9] = "orange red";
                        hv_Color[10] = "pink";
                        hv_Color[11] = "gold";
                    }
                }
                else if ((int)(new HTuple(((hv_Indices2.TupleSelect(0))).TupleNotEqual(
                    -1))) != 0)
                {
                    hv_Color = hv_GenParamValue.TupleSelect(hv_Indices2.TupleSelect(0));
                }
                else if ((int)(new HTuple(((hv_IndicesIntensities.TupleSelect(0))).TupleNotEqual(
                    -1))) != 0)
                {
                }
            }
            for (hv_J = 0; (int)hv_J <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength())) - 1); hv_J = (int)hv_J + 1)
            {
                hv_I = hv_Idx.TupleSelect(hv_J);
                if ((int)((new HTuple((new HTuple((new HTuple(((hv_HasPolygons.TupleSelect(
                    hv_I))).TupleEqual("true"))).TupleOr(new HTuple(((hv_HasTri.TupleSelect(
                    hv_I))).TupleEqual("true"))))).TupleOr(new HTuple(((hv_HasPoints.TupleSelect(
                    hv_I))).TupleEqual("true"))))).TupleOr(new HTuple(((hv_HasLines.TupleSelect(
                    hv_I))).TupleEqual("true")))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        HOperatorSet.TupleFind(hv_GenParamName, "color_" + hv_I, out hv_Indices3);
                        if ((int)(new HTuple(((hv_Indices3.TupleSelect(0))).TupleNotEqual(-1))) != 0)
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_GenParamValue.TupleSelect(
                                hv_Indices3.TupleSelect(0)));
                        }
                        else
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color.TupleSelect(hv_I % (new HTuple(hv_Color.TupleLength()
                                ))));
                        }
                    }
                    if ((int)(new HTuple((new HTuple(hv_PosesOut.TupleLength())).TupleGreaterEqual(
                        (hv_I * 7) + 6))) != 0)
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(hv_I * 7, (hv_I * 7) + 6);
                    }
                    else
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(0, 6);
                    }
                    if ((int)(new HTuple(((hv_NumPoints.TupleSelect(hv_I))).TupleLess(10000))) != 0)
                    {
                        ho_ModelContours.Dispose();
                        HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_ObjectModel3DID.TupleSelect(
                            hv_I), hv_CamParam, hv_Pose, hv_CustomParamName, hv_CustomParamValue);
                        HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                    }
                    else
                    {
                        HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D);
                        HOperatorSet.SampleObjectModel3d(hv_ObjectModel3DID.TupleSelect(hv_I),
                            "fast", 0.01 * (hv_Diameter.TupleSelect(hv_I)), new HTuple(), new HTuple(),
                            out hv_SampledObjectModel3D);
                        ho_ModelContours.Dispose();
                        HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_SampledObjectModel3D,
                            hv_CamParam, hv_Pose, "point_size", 1);
                        HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, "point_coord_x",
                            out hv_X);
                        HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, "point_coord_y",
                            out hv_Y);
                        HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, "point_coord_z",
                            out hv_Z);
                        HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D1);
                        HOperatorSet.AffineTransPoint3d(hv_HomMat3D1, hv_X, hv_Y, hv_Z, out hv_Qx,
                            out hv_Qy, out hv_Qz);
                        HOperatorSet.Project3dPoint(hv_Qx, hv_Qy, hv_Qz, hv_CamParam, out hv_Row,
                            out hv_Column);
                        HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                        HOperatorSet.ClearObjectModel3d(hv_SampledObjectModel3D);
                    }
                }
                else
                {
                    if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        HOperatorSet.TupleFind(hv_GenParamName, "color_" + hv_I, out hv_Indices3);
                        if ((int)(new HTuple(((hv_Indices3.TupleSelect(0))).TupleNotEqual(-1))) != 0)
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_GenParamValue.TupleSelect(
                                hv_Indices3.TupleSelect(0)));
                        }
                        else
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color.TupleSelect(hv_I % (new HTuple(hv_Color.TupleLength()
                                ))));
                        }
                    }
                    if ((int)(new HTuple((new HTuple(hv_PosesOut.TupleLength())).TupleGreaterEqual(
                        (hv_I * 7) + 6))) != 0)
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(hv_I * 7, (hv_I * 7) + 6);
                    }
                    else
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(0, 6);
                    }
                    if ((int)(new HTuple(((hv_IsPrimitive.TupleSelect(hv_I))).TupleEqual("true"))) != 0)
                    {
                        try
                        {
                            HOperatorSet.ConvexHullObjectModel3d(hv_ObjectModel3DID.TupleSelect(hv_I),
                                out hv_ObjectModel3DConvexHull);
                            if ((int)(new HTuple(((hv_NumPoints.TupleSelect(hv_I))).TupleLess(10000))) != 0)
                            {
                                ho_ModelContours.Dispose();
                                HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_ObjectModel3DConvexHull,
                                    hv_CamParam, hv_Pose, hv_CustomParamName, hv_CustomParamValue);
                                HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                            }
                            else
                            {
                                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D);
                                HOperatorSet.SampleObjectModel3d(hv_ObjectModel3DConvexHull, "fast",
                                    0.01 * (hv_Diameter.TupleSelect(hv_I)), new HTuple(), new HTuple(),
                                    out hv_SampledObjectModel3D);
                                ho_ModelContours.Dispose();
                                HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_SampledObjectModel3D,
                                    hv_CamParam, hv_Pose, "point_size", 1);
                                HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                                HOperatorSet.ClearObjectModel3d(hv_SampledObjectModel3D);
                            }
                            HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DConvexHull);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        }
                    }
                }
            }
            HOperatorSet.SetSystem("opengl_hidden_surface_removal_enable", hv_OpenGlHiddenSurface);

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Reflect the pose change that was introduced by the user by moving the mouse 
        private void analyze_graph_event(HObject ho_BackgroundImage, HTuple hv_MouseMapping,
            HTuple hv_Button, HTuple hv_Row, HTuple hv_Column, HTuple hv_WindowHandle, HTuple hv_WindowHandleBuffer,
            HTuple hv_VirtualTrackball, HTuple hv_TrackballSize, HTuple hv_SelectedObjectIn,
            HTuple hv_Scene3D, HTuple hv_AlphaOrig, HTuple hv_ObjectModel3DID, HTuple hv_CamParam,
            HTuple hv_Labels, HTuple hv_Title, HTuple hv_Information, HTuple hv_GenParamName,
            HTuple hv_GenParamValue, HTuple hv_PosesIn, HTuple hv_ButtonHoldIn, HTuple hv_TBCenter,
            HTuple hv_TBSize, HTuple hv_WindowCenteredRotationlIn, HTuple hv_MaxNumModels,
            out HTuple hv_PosesOut, out HTuple hv_SelectedObjectOut, out HTuple hv_ButtonHoldOut,
            out HTuple hv_WindowCenteredRotationOut)
        {




            // Local iconic variables 

            HObject ho_ImageDump = null;

            // Local control variables 

            HTuple ExpTmpLocalVar_gIsSinglePose = new HTuple();
            HTuple hv_VisualizeTB = null, hv_InvLog2 = null, hv_Seconds = new HTuple();
            HTuple hv_ModelIndex = new HTuple(), hv_Exception1 = new HTuple();
            HTuple hv_HomMat3DIdentity = new HTuple(), hv_NumModels = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MinImageSize = new HTuple(), hv_TrackballRadiusPixel = new HTuple();
            HTuple hv_TrackballCenterRow = new HTuple(), hv_TrackballCenterCol = new HTuple();
            HTuple hv_NumChannels = new HTuple(), hv_ColorImage = new HTuple();
            HTuple hv_BAnd = new HTuple(), hv_SensFactor = new HTuple();
            HTuple hv_IsButtonTrans = new HTuple(), hv_IsButtonRot = new HTuple();
            HTuple hv_IsButtonDist = new HTuple(), hv_MRow1 = new HTuple();
            HTuple hv_MCol1 = new HTuple(), hv_ButtonLoop = new HTuple();
            HTuple hv_MRow2 = new HTuple(), hv_MCol2 = new HTuple();
            HTuple hv_PX = new HTuple(), hv_PY = new HTuple(), hv_PZ = new HTuple();
            HTuple hv_QX1 = new HTuple(), hv_QY1 = new HTuple(), hv_QZ1 = new HTuple();
            HTuple hv_QX2 = new HTuple(), hv_QY2 = new HTuple(), hv_QZ2 = new HTuple();
            HTuple hv_Len = new HTuple(), hv_Dist = new HTuple(), hv_Translate = new HTuple();
            HTuple hv_Index = new HTuple(), hv_PoseIn = new HTuple();
            HTuple hv_HomMat3DIn = new HTuple(), hv_HomMat3DOut = new HTuple();
            HTuple hv_PoseOut = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Sequence = new HTuple(), hv_Mod = new HTuple();
            HTuple hv_SequenceReal = new HTuple(), hv_Sequence2Int = new HTuple();
            HTuple hv_Selected = new HTuple(), hv_InvSelected = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_DRow = new HTuple();
            HTuple hv_TranslateZ = new HTuple(), hv_MX1 = new HTuple();
            HTuple hv_MY1 = new HTuple(), hv_MX2 = new HTuple(), hv_MY2 = new HTuple();
            HTuple hv_RelQuaternion = new HTuple(), hv_HomMat3DRotRel = new HTuple();
            HTuple hv_HomMat3DInTmp1 = new HTuple(), hv_HomMat3DInTmp = new HTuple();
            HTuple hv_PosesOut2 = new HTuple();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_PosesIn_COPY_INP_TMP = hv_PosesIn.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_TBCenter_COPY_INP_TMP = hv_TBCenter.Clone();
            HTuple hv_TBSize_COPY_INP_TMP = hv_TBSize.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageDump);
            try
            {
                //This procedure reflects
                //- the pose change that was introduced by the user by
                //  moving the mouse
                //- the selection of a single object
                //
                //global tuple gIsSinglePose
                //
                hv_ButtonHoldOut = hv_ButtonHoldIn.Clone();
                hv_PosesOut = hv_PosesIn_COPY_INP_TMP.Clone();
                hv_SelectedObjectOut = hv_SelectedObjectIn.Clone();
                hv_WindowCenteredRotationOut = hv_WindowCenteredRotationlIn.Clone();
                hv_VisualizeTB = new HTuple(((hv_SelectedObjectOut.TupleMax())).TupleNotEqual(
                    0));
                hv_InvLog2 = 1.0 / ((new HTuple(2)).TupleLog());
                //
                if ((int)(new HTuple(hv_Button.TupleEqual(hv_MouseMapping.TupleSelect(6)))) != 0)
                {
                    if ((int)(hv_ButtonHoldOut) != 0)
                    {
                        ho_ImageDump.Dispose();

                        return;
                    }
                    //Ctrl (16) + Alt (32) + left mouse button (1) => Toggle rotation center position
                    //If WindowCenteredRotation is not 1, set it to 1, otherwise, set it to 2
                    HOperatorSet.CountSeconds(out hv_Seconds);
                    if ((int)(new HTuple(hv_WindowCenteredRotationOut.TupleEqual(1))) != 0)
                    {
                        hv_WindowCenteredRotationOut = 2;
                    }
                    else
                    {
                        hv_WindowCenteredRotationOut = 1;
                    }
                    hv_ButtonHoldOut = 1;
                    ho_ImageDump.Dispose();

                    return;
                }
                if ((int)((new HTuple(hv_Button.TupleEqual(hv_MouseMapping.TupleSelect(5)))).TupleAnd(
                    new HTuple((new HTuple(hv_ObjectModel3DID.TupleLength())).TupleLessEqual(
                    hv_MaxNumModels)))) != 0)
                {
                    if ((int)(hv_ButtonHoldOut) != 0)
                    {
                        ho_ImageDump.Dispose();

                        return;
                    }
                    //Ctrl (16) + left mouse button (1) => Select an object
                    try
                    {
                        HOperatorSet.SetScene3dParam(hv_Scene3D, "object_index_persistence", "true");
                        HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                        HOperatorSet.GetDisplayScene3dInfo(hv_WindowHandleBuffer, hv_Scene3D, hv_Row_COPY_INP_TMP,
                            hv_Column_COPY_INP_TMP, "object_index", out hv_ModelIndex);
                        HOperatorSet.SetScene3dParam(hv_Scene3D, "object_index_persistence", "false");
                    }
                    // catch (Exception1) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception1);
                        //* NO OpenGL, no selection possible
                        ho_ImageDump.Dispose();

                        return;
                    }
                    if ((int)(new HTuple(hv_ModelIndex.TupleEqual(-1))) != 0)
                    {
                        //Background click:
                        if ((int)(new HTuple(((hv_SelectedObjectOut.TupleSum())).TupleEqual(new HTuple(hv_SelectedObjectOut.TupleLength()
                            )))) != 0)
                        {
                            //If all objects are already selected, deselect all
                            hv_SelectedObjectOut = HTuple.TupleGenConst(new HTuple(hv_ObjectModel3DID.TupleLength()
                                ), 0);
                        }
                        else
                        {
                            //Otherwise select all
                            hv_SelectedObjectOut = HTuple.TupleGenConst(new HTuple(hv_ObjectModel3DID.TupleLength()
                                ), 1);
                        }
                    }
                    else
                    {
                        //Object click:
                        if (hv_SelectedObjectOut == null)
                            hv_SelectedObjectOut = new HTuple();
                        hv_SelectedObjectOut[hv_ModelIndex] = ((hv_SelectedObjectOut.TupleSelect(
                            hv_ModelIndex))).TupleNot();
                    }
                    hv_ButtonHoldOut = 1;
                }
                else
                {
                    //Change the pose
                    HOperatorSet.HomMat3dIdentity(out hv_HomMat3DIdentity);
                    hv_NumModels = new HTuple(hv_ObjectModel3DID.TupleLength());
                    hv_Width = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 2];
                    hv_Height = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 1];
                    hv_MinImageSize = ((hv_Width.TupleConcat(hv_Height))).TupleMin();
                    hv_TrackballRadiusPixel = (hv_TrackballSize * hv_MinImageSize) / 2.0;
                    //Set trackball fixed in the center of the window
                    hv_TrackballCenterRow = hv_Height / 2;
                    hv_TrackballCenterCol = hv_Width / 2;
                    if ((int)(new HTuple((new HTuple(hv_ObjectModel3DID.TupleLength())).TupleLess(
                        hv_MaxNumModels))) != 0)
                    {
                        if ((int)(new HTuple(hv_WindowCenteredRotationOut.TupleEqual(1))) != 0)
                        {
                            get_trackball_center_fixed(hv_SelectedObjectIn, hv_TrackballCenterRow,
                                hv_TrackballCenterCol, hv_TrackballRadiusPixel, hv_Scene3D, hv_ObjectModel3DID,
                                hv_PosesIn_COPY_INP_TMP, hv_WindowHandleBuffer, hv_CamParam, hv_GenParamName,
                                hv_GenParamValue, out hv_TBCenter_COPY_INP_TMP, out hv_TBSize_COPY_INP_TMP);
                        }
                        else
                        {
                            get_trackball_center(hv_SelectedObjectIn, hv_TrackballRadiusPixel, hv_ObjectModel3DID,
                                hv_PosesIn_COPY_INP_TMP, out hv_TBCenter_COPY_INP_TMP, out hv_TBSize_COPY_INP_TMP);
                        }
                    }
                    if ((int)((new HTuple(((hv_SelectedObjectOut.TupleMin())).TupleEqual(0))).TupleAnd(
                        new HTuple(((hv_SelectedObjectOut.TupleMax())).TupleEqual(1)))) != 0)
                    {
                        //At this point, multiple objects do not necessary have the same
                        //pose any more. Consequently, we have to return a tuple of poses
                        //as output of visualize_object_model_3d
                        ExpTmpLocalVar_gIsSinglePose = 0;
                        ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                    }
                    HOperatorSet.CountChannels(ho_BackgroundImage, out hv_NumChannels);
                    hv_ColorImage = new HTuple(hv_NumChannels.TupleEqual(3));
                    //Alt (32) => lower sensitivity
                    HOperatorSet.TupleRsh(hv_Button, 5, out hv_BAnd);
                    if ((int)(hv_BAnd % 2) != 0)
                    {
                        hv_SensFactor = 0.1;
                    }
                    else
                    {
                        hv_SensFactor = 1.0;
                    }
                    hv_IsButtonTrans = (new HTuple(((hv_MouseMapping.TupleSelect(0))).TupleEqual(
                        hv_Button))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(0)))).TupleEqual(
                        hv_Button)));
                    hv_IsButtonRot = (new HTuple(((hv_MouseMapping.TupleSelect(1))).TupleEqual(
                        hv_Button))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(1)))).TupleEqual(
                        hv_Button)));
                    hv_IsButtonDist = (new HTuple((new HTuple((new HTuple((new HTuple((new HTuple(((hv_MouseMapping.TupleSelect(
                        2))).TupleEqual(hv_Button))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(
                        2)))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((hv_MouseMapping.TupleSelect(
                        3))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(
                        3)))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((hv_MouseMapping.TupleSelect(
                        4))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(
                        4)))).TupleEqual(hv_Button)));
                    if ((int)(hv_IsButtonTrans) != 0)
                    {
                        //Translate in XY-direction
                        hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                        hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                        while ((int)(hv_IsButtonTrans) != 0)
                        {
                            try
                            {
                                HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row_COPY_INP_TMP,
                                    out hv_Column_COPY_INP_TMP, out hv_ButtonLoop);
                                hv_IsButtonTrans = new HTuple(hv_ButtonLoop.TupleEqual(hv_Button));
                                hv_MRow2 = hv_MRow1 + ((hv_Row_COPY_INP_TMP - hv_MRow1) * hv_SensFactor);
                                hv_MCol2 = hv_MCol1 + ((hv_Column_COPY_INP_TMP - hv_MCol1) * hv_SensFactor);
                                HOperatorSet.GetLineOfSight(hv_MRow1, hv_MCol1, hv_CamParam, out hv_PX,
                                    out hv_PY, out hv_PZ, out hv_QX1, out hv_QY1, out hv_QZ1);
                                HOperatorSet.GetLineOfSight(hv_MRow2, hv_MCol2, hv_CamParam, out hv_PX,
                                    out hv_PY, out hv_PZ, out hv_QX2, out hv_QY2, out hv_QZ2);
                                hv_Len = ((((hv_QX1 * hv_QX1) + (hv_QY1 * hv_QY1)) + (hv_QZ1 * hv_QZ1))).TupleSqrt()
                                    ;
                                hv_Dist = (((((hv_TBCenter_COPY_INP_TMP.TupleSelect(0)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    0))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(1)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    1)))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(2)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    2))))).TupleSqrt();
                                hv_Translate = ((((((hv_QX2 - hv_QX1)).TupleConcat(hv_QY2 - hv_QY1))).TupleConcat(
                                    hv_QZ2 - hv_QZ1)) * hv_Dist) / hv_Len;
                                hv_PosesOut = new HTuple();
                                if ((int)(new HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels))) != 0)
                                {
                                    HTuple end_val110 = hv_NumModels - 1;
                                    HTuple step_val110 = 1;
                                    for (hv_Index = 0; hv_Index.Continue(end_val110, step_val110); hv_Index = hv_Index.TupleAdd(step_val110))
                                    {
                                        hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index * 7,
                                            (hv_Index * 7) + 6);
                                        if ((int)(hv_SelectedObjectOut.TupleSelect(hv_Index)) != 0)
                                        {
                                            HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_Translate.TupleSelect(
                                                0), hv_Translate.TupleSelect(1), hv_Translate.TupleSelect(
                                                2), out hv_HomMat3DOut);
                                            HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                            HOperatorSet.SetScene3dInstancePose(hv_Scene3D, hv_Index, hv_PoseOut);
                                        }
                                        else
                                        {
                                            hv_PoseOut = hv_PoseIn.Clone();
                                        }
                                        hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut);
                                    }
                                }
                                else
                                {
                                    HOperatorSet.TupleFind(hv_SelectedObjectOut, 1, out hv_Indices);
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange((hv_Indices.TupleSelect(
                                        0)) * 7, ((hv_Indices.TupleSelect(0)) * 7) + 6);
                                    HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_Translate.TupleSelect(
                                        0), hv_Translate.TupleSelect(1), hv_Translate.TupleSelect(2),
                                        out hv_HomMat3DOut);
                                    HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                                    HOperatorSet.TupleMod(hv_Sequence, 7, out hv_Mod);
                                    hv_SequenceReal = HTuple.TupleGenSequence(0, hv_NumModels - (1.0 / 7.0),
                                        1.0 / 7.0);
                                    hv_Sequence2Int = hv_SequenceReal.TupleInt();
                                    HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, out hv_Selected);
                                    hv_InvSelected = 1 - hv_Selected;
                                    HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, out hv_PosesOut);
                                    hv_PosesOut = (hv_PosesOut * hv_Selected) + (hv_PosesIn_COPY_INP_TMP * hv_InvSelected);
                                    HOperatorSet.SetScene3dInstancePose(hv_Scene3D, HTuple.TupleGenSequence(
                                        0, hv_NumModels - 1, 1), hv_PosesOut);
                                }
                                dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_Scene3D,
                                    hv_AlphaOrig, hv_ObjectModel3DID, hv_GenParamName, hv_GenParamValue,
                                    hv_CamParam, hv_PosesOut, hv_ColorImage, hv_Title, hv_Information,
                                    hv_Labels, hv_VisualizeTB, "true", hv_TrackballCenterRow, hv_TrackballCenterCol,
                                    hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, new HTuple(hv_WindowCenteredRotationOut.TupleEqual(
                                    1)), hv_TBCenter_COPY_INP_TMP);
                                ho_ImageDump.Dispose();
                                HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                                HDevWindowStack.SetActive(hv_WindowHandle);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                                }
                                //
                                hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                                hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                                hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone();
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException1)
                            {
                                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                                //Keep waiting
                            }
                        }
                    }
                    else if ((int)(hv_IsButtonDist) != 0)
                    {
                        //Change the Z distance
                        hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                        while ((int)(hv_IsButtonDist) != 0)
                        {
                            try
                            {
                                HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row_COPY_INP_TMP,
                                    out hv_Column_COPY_INP_TMP, out hv_ButtonLoop);
                                hv_IsButtonDist = new HTuple(hv_ButtonLoop.TupleEqual(hv_Button));
                                hv_MRow2 = hv_Row_COPY_INP_TMP.Clone();
                                hv_DRow = hv_MRow2 - hv_MRow1;
                                hv_Dist = (((((hv_TBCenter_COPY_INP_TMP.TupleSelect(0)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    0))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(1)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    1)))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(2)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    2))))).TupleSqrt();
                                hv_TranslateZ = (((-hv_Dist) * hv_DRow) * 0.003) * hv_SensFactor;
                                if (hv_TBCenter_COPY_INP_TMP == null)
                                    hv_TBCenter_COPY_INP_TMP = new HTuple();
                                hv_TBCenter_COPY_INP_TMP[2] = (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    2)) + hv_TranslateZ;
                                hv_PosesOut = new HTuple();
                                if ((int)(new HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels))) != 0)
                                {
                                    HTuple end_val164 = hv_NumModels - 1;
                                    HTuple step_val164 = 1;
                                    for (hv_Index = 0; hv_Index.Continue(end_val164, step_val164); hv_Index = hv_Index.TupleAdd(step_val164))
                                    {
                                        hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index * 7,
                                            (hv_Index * 7) + 6);
                                        if ((int)(hv_SelectedObjectOut.TupleSelect(hv_Index)) != 0)
                                        {
                                            //Transform the whole scene or selected object only
                                            HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, 0, 0, hv_TranslateZ,
                                                out hv_HomMat3DOut);
                                            HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                            HOperatorSet.SetScene3dInstancePose(hv_Scene3D, hv_Index, hv_PoseOut);
                                        }
                                        else
                                        {
                                            hv_PoseOut = hv_PoseIn.Clone();
                                        }
                                        hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut);
                                    }
                                }
                                else
                                {
                                    HOperatorSet.TupleFind(hv_SelectedObjectOut, 1, out hv_Indices);
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange((hv_Indices.TupleSelect(
                                        0)) * 7, ((hv_Indices.TupleSelect(0)) * 7) + 6);
                                    HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, 0, 0, hv_TranslateZ,
                                        out hv_HomMat3DOut);
                                    HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                                    HOperatorSet.TupleMod(hv_Sequence, 7, out hv_Mod);
                                    hv_SequenceReal = HTuple.TupleGenSequence(0, hv_NumModels - (1.0 / 7.0),
                                        1.0 / 7.0);
                                    hv_Sequence2Int = hv_SequenceReal.TupleInt();
                                    HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, out hv_Selected);
                                    hv_InvSelected = 1 - hv_Selected;
                                    HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, out hv_PosesOut);
                                    hv_PosesOut = (hv_PosesOut * hv_Selected) + (hv_PosesIn_COPY_INP_TMP * hv_InvSelected);
                                    HOperatorSet.SetScene3dInstancePose(hv_Scene3D, HTuple.TupleGenSequence(
                                        0, hv_NumModels - 1, 1), hv_PosesOut);
                                }
                                dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_Scene3D,
                                    hv_AlphaOrig, hv_ObjectModel3DID, hv_GenParamName, hv_GenParamValue,
                                    hv_CamParam, hv_PosesOut, hv_ColorImage, hv_Title, hv_Information,
                                    hv_Labels, hv_VisualizeTB, "true", hv_TrackballCenterRow, hv_TrackballCenterCol,
                                    hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, hv_WindowCenteredRotationOut,
                                    hv_TBCenter_COPY_INP_TMP);
                                ho_ImageDump.Dispose();
                                HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                                HDevWindowStack.SetActive(hv_WindowHandle);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                                }
                                //
                                hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                                hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone();
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException1)
                            {
                                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                                //Keep waiting
                            }
                        }
                    }
                    else if ((int)(hv_IsButtonRot) != 0)
                    {
                        //Rotate the object
                        hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                        hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                        while ((int)(hv_IsButtonRot) != 0)
                        {
                            try
                            {
                                HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row_COPY_INP_TMP,
                                    out hv_Column_COPY_INP_TMP, out hv_ButtonLoop);
                                hv_IsButtonRot = new HTuple(hv_ButtonLoop.TupleEqual(hv_Button));
                                hv_MRow2 = hv_Row_COPY_INP_TMP.Clone();
                                hv_MCol2 = hv_Column_COPY_INP_TMP.Clone();
                                //Transform the pixel coordinates to relative image coordinates
                                hv_MX1 = (hv_TrackballCenterCol - hv_MCol1) / (0.5 * hv_MinImageSize);
                                hv_MY1 = (hv_TrackballCenterRow - hv_MRow1) / (0.5 * hv_MinImageSize);
                                hv_MX2 = (hv_TrackballCenterCol - hv_MCol2) / (0.5 * hv_MinImageSize);
                                hv_MY2 = (hv_TrackballCenterRow - hv_MRow2) / (0.5 * hv_MinImageSize);
                                //Compute the quaternion rotation that corresponds to the mouse
                                //movement
                                trackball(hv_MX1, hv_MY1, hv_MX2, hv_MY2, hv_VirtualTrackball, hv_TrackballSize,
                                    hv_SensFactor, out hv_RelQuaternion);
                                //Transform the quaternion to a rotation matrix
                                HOperatorSet.QuatToHomMat3d(hv_RelQuaternion, out hv_HomMat3DRotRel);
                                hv_PosesOut = new HTuple();
                                if ((int)(new HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels))) != 0)
                                {
                                    HTuple end_val226 = hv_NumModels - 1;
                                    HTuple step_val226 = 1;
                                    for (hv_Index = 0; hv_Index.Continue(end_val226, step_val226); hv_Index = hv_Index.TupleAdd(step_val226))
                                    {
                                        hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index * 7,
                                            (hv_Index * 7) + 6);
                                        if ((int)(hv_SelectedObjectOut.TupleSelect(hv_Index)) != 0)
                                        {
                                            //Transform the whole scene or selected object only
                                            HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                0)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(1)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                2)), out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dCompose(hv_HomMat3DRotRel, hv_HomMat3DIn,
                                                out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                0), hv_TBCenter_COPY_INP_TMP.TupleSelect(1), hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                2), out hv_HomMat3DOut);
                                            HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                            HOperatorSet.SetScene3dInstancePose(hv_Scene3D, hv_Index, hv_PoseOut);
                                        }
                                        else
                                        {
                                            hv_PoseOut = hv_PoseIn.Clone();
                                        }
                                        hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut);
                                    }
                                }
                                else
                                {
                                    HOperatorSet.TupleFind(hv_SelectedObjectOut, 1, out hv_Indices);
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange((hv_Indices.TupleSelect(
                                        0)) * 7, ((hv_Indices.TupleSelect(0)) * 7) + 6);
                                    HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        0)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(1)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        2)), out hv_HomMat3DInTmp1);
                                    HOperatorSet.HomMat3dCompose(hv_HomMat3DRotRel, hv_HomMat3DInTmp1,
                                        out hv_HomMat3DInTmp);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DInTmp, hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        0), hv_TBCenter_COPY_INP_TMP.TupleSelect(1), hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        2), out hv_HomMat3DOut);
                                    HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                                    HOperatorSet.TupleMod(hv_Sequence, 7, out hv_Mod);
                                    hv_SequenceReal = HTuple.TupleGenSequence(0, hv_NumModels - (1.0 / 7.0),
                                        1.0 / 7.0);
                                    hv_Sequence2Int = hv_SequenceReal.TupleInt();
                                    HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, out hv_Selected);
                                    hv_InvSelected = 1 - hv_Selected;
                                    HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, out hv_PosesOut);
                                    hv_PosesOut2 = (hv_PosesOut * hv_Selected) + (hv_PosesIn_COPY_INP_TMP * hv_InvSelected);
                                    hv_PosesOut = hv_PosesOut2.Clone();
                                    HOperatorSet.SetScene3dInstancePose(hv_Scene3D, HTuple.TupleGenSequence(
                                        0, hv_NumModels - 1, 1), hv_PosesOut);
                                }
                                dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_Scene3D,
                                    hv_AlphaOrig, hv_ObjectModel3DID, hv_GenParamName, hv_GenParamValue,
                                    hv_CamParam, hv_PosesOut, hv_ColorImage, hv_Title, hv_Information,
                                    hv_Labels, hv_VisualizeTB, "true", hv_TrackballCenterRow, hv_TrackballCenterCol,
                                    hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, hv_WindowCenteredRotationOut,
                                    hv_TBCenter_COPY_INP_TMP);
                                ho_ImageDump.Dispose();
                                HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                                HDevWindowStack.SetActive(hv_WindowHandle);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                                }
                                //
                                hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                                hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                                hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone();
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException1)
                            {
                                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                                //Keep waiting
                            }
                        }
                    }
                    hv_PosesOut = hv_PosesIn_COPY_INP_TMP.Clone();
                }
                ho_ImageDump.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageDump.Dispose();

                throw HDevExpDefaultException;
            }
        }



        // Chapter: Graphics / Output
        // Short Description: Compute the center of all given 3D object models. 
        private void get_object_models_center(HTuple hv_ObjectModel3DID, out HTuple hv_Center)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_Diameter = new HTuple(), hv_MD = new HTuple();
            HTuple hv_Weight = new HTuple(), hv_SumW = new HTuple();
            HTuple hv_Index = new HTuple(), hv_ObjectModel3DIDSelected = new HTuple();
            HTuple hv_C = new HTuple(), hv_InvSum = new HTuple();
            // Initialize local and output iconic variables 
            hv_Center = new HTuple();
            //Compute the mean of all model centers (weighted by the diameter of the object models)
            if ((int)(new HTuple((new HTuple(hv_ObjectModel3DID.TupleLength())).TupleGreater(
                0))) != 0)
            {
                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "diameter_axis_aligned_bounding_box",
                    out hv_Diameter);
                //Normalize Diameter to use it as weights for a weighted mean of the individual centers
                hv_MD = hv_Diameter.TupleMean();
                if ((int)(new HTuple(hv_MD.TupleGreater(1e-10))) != 0)
                {
                    hv_Weight = hv_Diameter / hv_MD;
                }
                else
                {
                    hv_Weight = hv_Diameter.Clone();
                }
                hv_SumW = hv_Weight.TupleSum();
                if ((int)(new HTuple(hv_SumW.TupleLess(1e-10))) != 0)
                {
                    hv_Weight = HTuple.TupleGenConst(new HTuple(hv_Weight.TupleLength()), 1.0);
                    hv_SumW = hv_Weight.TupleSum();
                }
                hv_Center = new HTuple();
                hv_Center[0] = 0;
                hv_Center[1] = 0;
                hv_Center[2] = 0;
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_ObjectModel3DIDSelected = hv_ObjectModel3DID.TupleSelect(hv_Index);
                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DIDSelected, "center",
                        out hv_C);
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[0] = (hv_Center.TupleSelect(0)) + ((hv_C.TupleSelect(0)) * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[1] = (hv_Center.TupleSelect(1)) + ((hv_C.TupleSelect(1)) * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[2] = (hv_Center.TupleSelect(2)) + ((hv_C.TupleSelect(2)) * (hv_Weight.TupleSelect(
                        hv_Index)));
                }
                hv_InvSum = 1.0 / hv_SumW;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[0] = (hv_Center.TupleSelect(0)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[1] = (hv_Center.TupleSelect(1)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[2] = (hv_Center.TupleSelect(2)) * hv_InvSum;
            }
            else
            {
                hv_Center = new HTuple();
            }
            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Renders 3d object models in a buffer window. 
        private void dump_image_output(HObject ho_BackgroundImage, HTuple hv_WindowHandleBuffer,
            HTuple hv_Scene3D, HTuple hv_AlphaOrig, HTuple hv_ObjectModel3DID, HTuple hv_GenParamName,
            HTuple hv_GenParamValue, HTuple hv_CamParam, HTuple hv_Poses, HTuple hv_ColorImage,
            HTuple hv_Title, HTuple hv_Information, HTuple hv_Labels, HTuple hv_VisualizeTrackball,
            HTuple hv_DisplayContinueButton, HTuple hv_TrackballCenterRow, HTuple hv_TrackballCenterCol,
            HTuple hv_TrackballRadiusPixel, HTuple hv_SelectedObject, HTuple hv_VisualizeRotationCenter,
            HTuple hv_RotationCenter)
        {
            // Local iconic variables 
            HObject ho_ModelContours = null, ho_Image;
            HObject ho_TrackballContour = null, ho_CrossRotCenter = null;
            // Local control variables 
            HTuple ExpTmpLocalVar_gUsesOpenGL = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Position = new HTuple(), hv_PosIdx = new HTuple();
            HTuple hv_Substrings = new HTuple(), hv_I = new HTuple();
            HTuple hv_HasExtended = new HTuple(), hv_ExtendedAttributeNames = new HTuple();
            HTuple hv_Matches = new HTuple(), hv_Exception1 = new HTuple();
            HTuple hv_DeselectedIdx = new HTuple(), hv_DeselectedName = new HTuple();
            HTuple hv_DeselectedValue = new HTuple(), hv_Pose = new HTuple();
            HTuple hv_HomMat3D = new HTuple(), hv_Center = new HTuple();
            HTuple hv_CenterCamX = new HTuple(), hv_CenterCamY = new HTuple();
            HTuple hv_CenterCamZ = new HTuple(), hv_CenterRow = new HTuple();
            HTuple hv_CenterCol = new HTuple(), hv_Label = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_TextWidth = new HTuple(), hv_TextHeight = new HTuple();
            HTuple hv_RotCenterRow = new HTuple(), hv_RotCenterCol = new HTuple();
            HTuple hv_Orientation = new HTuple(), hv_Colors = new HTuple();
            HTuple hv_RotationCenter_COPY_INP_TMP = hv_RotationCenter.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_TrackballContour);
            HOperatorSet.GenEmptyObj(out ho_CrossRotCenter);
            try
            {
                //global tuple gAlphaDeselected
                //global tuple gTerminationButtonLabel
                //global tuple gDispObjOffset
                //global tuple gLabelsDecor
                //global tuple gUsesOpenGL
                //
                //Display background image
                HOperatorSet.ClearWindow(hv_WindowHandleBuffer);
                if ((int)(hv_ColorImage) != 0)
                {
                    HOperatorSet.DispColor(ho_BackgroundImage, hv_WindowHandleBuffer);
                }
                else
                {
                    HOperatorSet.DispImage(ho_BackgroundImage, hv_WindowHandleBuffer);
                }
                //
                //Display objects
                if ((int)(new HTuple(((hv_SelectedObject.TupleSum())).TupleEqual(new HTuple(hv_SelectedObject.TupleLength()
                    )))) != 0)
                {
                    if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("true"))) != 0)
                    {
                        try
                        {
                            HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            if ((int)((new HTuple((new HTuple((new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1306))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1305))))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1406))))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1405)))) != 0)
                            {
                                if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleEqual(
                                    new HTuple(hv_GenParamValue.TupleLength())))) != 0)
                                {
                                    //This case means we have a Parameter with structure parameter_x with x > |ObjectModel3DID|-1
                                    for (hv_Index = new HTuple(hv_ObjectModel3DID.TupleLength()); (int)hv_Index <= (int)((2 * (new HTuple(hv_ObjectModel3DID.TupleLength()
                                        ))) + 1); hv_Index = (int)hv_Index + 1)
                                    {
                                        HOperatorSet.TupleStrstr(hv_GenParamName, "" + hv_Index, out hv_Position);
                                        for (hv_PosIdx = 0; (int)hv_PosIdx <= (int)((new HTuple(hv_Position.TupleLength()
                                            )) - 1); hv_PosIdx = (int)hv_PosIdx + 1)
                                        {
                                            if ((int)(new HTuple(((hv_Position.TupleSelect(hv_PosIdx))).TupleNotEqual(
                                                -1))) != 0)
                                            {
                                                throw new HalconException((("One of the parameters is refferring to a non-existing object model 3D:\n" + (hv_GenParamName.TupleSelect(
                                                    hv_PosIdx))) + " -> ") + (hv_GenParamValue.TupleSelect(hv_PosIdx)));
                                            }
                                        }
                                    }
                                    //Test for non-existing extended attributes:
                                    HOperatorSet.TupleStrstr(hv_GenParamName, "intensity", out hv_Position);
                                    for (hv_PosIdx = 0; (int)hv_PosIdx <= (int)((new HTuple(hv_Position.TupleLength()
                                        )) - 1); hv_PosIdx = (int)hv_PosIdx + 1)
                                    {
                                        if ((int)(new HTuple(((hv_Position.TupleSelect(hv_PosIdx))).TupleNotEqual(
                                            -1))) != 0)
                                        {
                                            HOperatorSet.TupleSplit(hv_GenParamName.TupleSelect(hv_PosIdx),
                                                "_", out hv_Substrings);
                                            if ((int)((new HTuple((new HTuple(hv_Substrings.TupleLength()
                                                )).TupleGreater(1))).TupleAnd(((hv_Substrings.TupleSelect(
                                                1))).TupleIsNumber())) != 0)
                                            {
                                                hv_I = ((hv_Substrings.TupleSelect(1))).TupleNumber();
                                                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(
                                                    hv_I), "has_extended_attribute", out hv_HasExtended);
                                                if ((int)(hv_HasExtended) != 0)
                                                {
                                                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(
                                                        hv_I), "extended_attribute_names", out hv_ExtendedAttributeNames);
                                                    HOperatorSet.TupleFind(hv_ExtendedAttributeNames, hv_GenParamValue.TupleSelect(
                                                        hv_PosIdx), out hv_Matches);
                                                }
                                                if ((int)((new HTuple(hv_HasExtended.TupleNot())).TupleOr((new HTuple(hv_Matches.TupleEqual(
                                                    -1))).TupleOr(new HTuple((new HTuple(hv_Matches.TupleLength()
                                                    )).TupleEqual(0))))) != 0)
                                                {
                                                    throw new HalconException((((("One of the parameters is refferring to an extended attribute that is not contained in the object model 3d with the handle " + (hv_ObjectModel3DID.TupleSelect(
                                                        hv_I))) + ":\n") + (hv_GenParamName.TupleSelect(hv_PosIdx))) + " -> ") + (hv_GenParamValue.TupleSelect(
                                                        hv_PosIdx)));
                                                }
                                            }
                                            else
                                            {
                                                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength()
                                                    )) - 1); hv_I = (int)hv_I + 1)
                                                {
                                                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(
                                                        hv_I), "extended_attribute_names", out hv_ExtendedAttributeNames);
                                                    HOperatorSet.TupleFind(hv_ExtendedAttributeNames, hv_GenParamValue.TupleSelect(
                                                        hv_PosIdx), out hv_Matches);
                                                    if ((int)((new HTuple(hv_Matches.TupleEqual(-1))).TupleOr(
                                                        new HTuple((new HTuple(hv_Matches.TupleLength())).TupleEqual(
                                                        0)))) != 0)
                                                    {
                                                        throw new HalconException((("One of the parameters is refferring to an extended attribute that is not contained in all object models:\n" + (hv_GenParamName.TupleSelect(
                                                            hv_PosIdx))) + " -> ") + (hv_GenParamValue.TupleSelect(
                                                            hv_PosIdx)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //
                                    throw new HalconException((new HTuple("Wrong generic parameters for display\n") + "Wrong Values are:\n") + (((((("    " + ((hv_GenParamName + " -> ") + hv_GenParamValue)) + "\n")).TupleSum()
                                        ) + "Exeption was:\n    ") + (hv_Exception.TupleSelect(2))));
                                }
                                else
                                {
                                    throw new HalconException(hv_Exception);
                                }
                            }
                            else if ((int)((new HTuple((new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(5185))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(5188))))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(5187)))) != 0)
                            {
                                ExpTmpLocalVar_gUsesOpenGL = "false";
                                ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                            }
                            else
                            {
                                throw new HalconException(hv_Exception);
                            }
                        }
                    }
                    if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("false"))) != 0)
                    {
                        //* NO OpenGL, use fallback
                        ho_ModelContours.Dispose();
                        disp_object_model_no_opengl(out ho_ModelContours, hv_ObjectModel3DID, hv_GenParamName,
                            hv_GenParamValue, hv_WindowHandleBuffer, hv_CamParam, hv_Poses);
                    }
                }
                else
                {
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_AlphaOrig.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        if ((int)(new HTuple(((hv_SelectedObject.TupleSelect(hv_Index))).TupleEqual(
                            1))) != 0)
                        {
                            HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index, "alpha", hv_AlphaOrig.TupleSelect(
                                hv_Index));
                        }
                        else
                        {
                            HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index, "alpha", ExpGetGlobalVar_gAlphaDeselected());
                        }
                    }
                    try
                    {
                        if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("false"))) != 0)
                        {
                            throw new HalconException(new HTuple());
                        }
                        HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                    }
                    // catch (Exception1) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception1);
                        //* NO OpenGL, use fallback
                        hv_DeselectedIdx = hv_SelectedObject.TupleFind(0);
                        if ((int)(new HTuple(hv_DeselectedIdx.TupleNotEqual(-1))) != 0)
                        {
                            hv_DeselectedName = "color_" + hv_DeselectedIdx;
                            hv_DeselectedValue = HTuple.TupleGenConst(new HTuple(hv_DeselectedName.TupleLength()
                                ), "gray");
                        }
                        ho_ModelContours.Dispose();
                        disp_object_model_no_opengl(out ho_ModelContours, hv_ObjectModel3DID, hv_GenParamName.TupleConcat(
                            hv_DeselectedName), hv_GenParamValue.TupleConcat(hv_DeselectedValue),
                            hv_WindowHandleBuffer, hv_CamParam, hv_Poses);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_AlphaOrig.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index, "alpha", hv_AlphaOrig.TupleSelect(
                            hv_Index));
                    }
                }
                ho_Image.Dispose();
                HOperatorSet.DumpWindowImage(out ho_Image, hv_WindowHandleBuffer);
                //
                //Display labels
                if ((int)(new HTuple(hv_Labels.TupleNotEqual(0))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, ExpGetGlobalVar_gLabelsDecor().TupleSelect(
                        0));
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        //Project the center point of the current model
                        hv_Pose = hv_Poses.TupleSelectRange(hv_Index * 7, (hv_Index * 7) + 6);
                        HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D);
                        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(hv_Index),
                            "center", out hv_Center);
                        HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Center.TupleSelect(0),
                            hv_Center.TupleSelect(1), hv_Center.TupleSelect(2), out hv_CenterCamX,
                            out hv_CenterCamY, out hv_CenterCamZ);
                        HOperatorSet.Project3dPoint(hv_CenterCamX, hv_CenterCamY, hv_CenterCamZ,
                            hv_CamParam, out hv_CenterRow, out hv_CenterCol);
                        hv_Label = hv_Labels.TupleSelect(hv_Index);
                        if ((int)(new HTuple(hv_Label.TupleNotEqual(""))) != 0)
                        {
                            HOperatorSet.GetStringExtents(hv_WindowHandleBuffer, hv_Label, out hv_Ascent,
                                out hv_Descent, out hv_TextWidth, out hv_TextHeight);
                            disp_message(hv_WindowHandleBuffer, hv_Label, "window", (hv_CenterRow - (hv_TextHeight / 2)) + (ExpGetGlobalVar_gDispObjOffset().TupleSelect(
                                0)), (hv_CenterCol - (hv_TextWidth / 2)) + (ExpGetGlobalVar_gDispObjOffset().TupleSelect(
                                1)), new HTuple(), ExpGetGlobalVar_gLabelsDecor().TupleSelect(1));
                        }
                    }
                }
                //
                //Visualize the trackball if desired
                if ((int)(hv_VisualizeTrackball) != 0)
                {
                    HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, 1);
                    ho_TrackballContour.Dispose();
                    HOperatorSet.GenEllipseContourXld(out ho_TrackballContour, hv_TrackballCenterRow,
                        hv_TrackballCenterCol, 0, hv_TrackballRadiusPixel, hv_TrackballRadiusPixel,
                        0, 6.28318, "positive", 1.5);
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, "dim gray");
                    HOperatorSet.DispXld(ho_TrackballContour, hv_WindowHandleBuffer);
                }
                //
                //Visualize the rotation center if desired
                if ((int)((new HTuple(hv_VisualizeRotationCenter.TupleNotEqual(0))).TupleAnd(
                    new HTuple((new HTuple(hv_RotationCenter_COPY_INP_TMP.TupleLength())).TupleEqual(
                    3)))) != 0)
                {
                    if ((int)(new HTuple(((hv_RotationCenter_COPY_INP_TMP.TupleSelect(2))).TupleLess(
                        1e-10))) != 0)
                    {
                        if (hv_RotationCenter_COPY_INP_TMP == null)
                            hv_RotationCenter_COPY_INP_TMP = new HTuple();
                        hv_RotationCenter_COPY_INP_TMP[2] = 1e-10;
                    }
                    HOperatorSet.Project3dPoint(hv_RotationCenter_COPY_INP_TMP.TupleSelect(0),
                        hv_RotationCenter_COPY_INP_TMP.TupleSelect(1), hv_RotationCenter_COPY_INP_TMP.TupleSelect(
                        2), hv_CamParam, out hv_RotCenterRow, out hv_RotCenterCol);
                    hv_Orientation = (new HTuple(90)).TupleRad();
                    if ((int)(new HTuple(hv_VisualizeRotationCenter.TupleEqual(1))) != 0)
                    {
                        hv_Orientation = (new HTuple(45)).TupleRad();
                    }
                    ho_CrossRotCenter.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_CrossRotCenter, hv_RotCenterRow, hv_RotCenterCol,
                        hv_TrackballRadiusPixel / 25.0, hv_Orientation);
                    HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, 3);
                    HOperatorSet.QueryColor(hv_WindowHandleBuffer, out hv_Colors);
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, "light gray");
                    HOperatorSet.DispXld(ho_CrossRotCenter, hv_WindowHandleBuffer);
                    HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, 1);
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, "dim gray");
                    HOperatorSet.DispXld(ho_CrossRotCenter, hv_WindowHandleBuffer);
                }
                //
                //Display title
                disp_title_and_information(hv_WindowHandleBuffer, hv_Title, hv_Information);
                //
                //Display the 'Exit' button
                if ((int)(new HTuple(hv_DisplayContinueButton.TupleEqual("true"))) != 0)
                {
                    disp_continue_button(hv_WindowHandleBuffer);
                }
                //
                ho_ModelContours.Dispose();
                ho_Image.Dispose();
                ho_TrackballContour.Dispose();
                ho_CrossRotCenter.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContours.Dispose();
                ho_Image.Dispose();
                ho_TrackballContour.Dispose();
                ho_CrossRotCenter.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Text
        // Short Description: This procedure writes a text message. 
        private void disp_text_button(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_TextColor, HTuple hv_ButtonColor)
        {
            // Local iconic variables 
            HObject ho_UpperLeft, ho_LowerRight, ho_Rectangle;
            // Local control variables 
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_Exception = null;
            HTuple hv_Fac = null, hv_RGBL = null, hv_RGB = new HTuple();
            HTuple hv_RGBD = null, hv_ButtonColorBorderL = null, hv_ButtonColorBorderD = null;
            HTuple hv_MaxAscent = null, hv_MaxDescent = null, hv_MaxWidth = null;
            HTuple hv_MaxHeight = null, hv_R1 = new HTuple(), hv_C1 = new HTuple();
            HTuple hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = null, hv_Index = null, hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple();
            HTuple hv_H = new HTuple(), hv_FrameHeight = null, hv_FrameWidth = null;
            HTuple hv_R2 = null, hv_C2 = null, hv_ClipRegion = null;
            HTuple hv_DrawMode = null, hv_BorderWidth = null, hv_CurrentColor = new HTuple();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();
            HTuple hv_TextColor_COPY_INP_TMP = hv_TextColor.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UpperLeft);
            HOperatorSet.GenEmptyObj(out ho_LowerRight);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            try
            {
                //
                //prepare window
                HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
                HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part,
                    out hv_Row2Part, out hv_Column2Part);
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                    out hv_WidthWin, out hv_HeightWin);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
                //
                //default settings
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_TextColor_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
                {
                    hv_TextColor_COPY_INP_TMP = "";
                }
                //
                try
                {
                    color_string_to_rgb(hv_ButtonColor, out hv_RGB);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter ButtonColor (must be a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                hv_Fac = 0.4;
                hv_RGBL = hv_RGB + (((((255.0 - hv_RGB) * hv_Fac) + 0.5)).TupleInt());
                hv_RGBD = hv_RGB - ((((hv_RGB * hv_Fac) + 0.5)).TupleInt());
                hv_ButtonColorBorderL = "#" + ((("" + (hv_RGBL.TupleString("02x")))).TupleSum()
                    );
                hv_ButtonColorBorderD = "#" + ((("" + (hv_RGBD.TupleString("02x")))).TupleSum()
                    );
                //
                hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
                //
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                    out hv_MaxWidth, out hv_MaxHeight);
                if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
                {
                    hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                    hv_C1 = hv_Column_COPY_INP_TMP.Clone();
                }
                else
                {
                    //transform image to window coordinates
                    hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                    hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                    hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                    hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
                }
                //
                //display text box depending on text size
                //
                //calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //display rectangles
                HOperatorSet.GetSystem("clip_region", out hv_ClipRegion);
                HOperatorSet.SetSystem("clip_region", "false");
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                hv_BorderWidth = 2;
                ho_UpperLeft.Dispose();
                HOperatorSet.GenRegionPolygonFilled(out ho_UpperLeft, ((((((((hv_R1 - hv_BorderWidth)).TupleConcat(
                    hv_R1 - hv_BorderWidth))).TupleConcat(hv_R1))).TupleConcat(hv_R2))).TupleConcat(
                    hv_R2 + hv_BorderWidth), ((((((((hv_C1 - hv_BorderWidth)).TupleConcat(hv_C2 + hv_BorderWidth))).TupleConcat(
                    hv_C2))).TupleConcat(hv_C1))).TupleConcat(hv_C1 - hv_BorderWidth));
                ho_LowerRight.Dispose();
                HOperatorSet.GenRegionPolygonFilled(out ho_LowerRight, ((((((((hv_R2 + hv_BorderWidth)).TupleConcat(
                    hv_R1 - hv_BorderWidth))).TupleConcat(hv_R1))).TupleConcat(hv_R2))).TupleConcat(
                    hv_R2 + hv_BorderWidth), ((((((((hv_C2 + hv_BorderWidth)).TupleConcat(hv_C2 + hv_BorderWidth))).TupleConcat(
                    hv_C2))).TupleConcat(hv_C1))).TupleConcat(hv_C1 - hv_BorderWidth));
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetColor(hv_WindowHandle, hv_ButtonColorBorderL);
                HOperatorSet.DispObj(ho_UpperLeft, hv_WindowHandle);
                HOperatorSet.SetColor(hv_WindowHandle, hv_ButtonColorBorderD);
                HOperatorSet.DispObj(ho_LowerRight, hv_WindowHandle);
                HOperatorSet.SetColor(hv_WindowHandle, hv_ButtonColor);
                HOperatorSet.DispObj(ho_Rectangle, hv_WindowHandle);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                HOperatorSet.SetSystem("clip_region", hv_ClipRegion);
                //Write text.
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_CurrentColor = hv_TextColor_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_TextColor_COPY_INP_TMP.TupleLength()
                        )));
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                    HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                    HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index));
                }
                //reset changed window settings
                HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                    hv_Column2Part);
                ho_UpperLeft.Dispose();
                ho_LowerRight.Dispose();
                ho_Rectangle.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_UpperLeft.Dispose();
                ho_LowerRight.Dispose();
                ho_Rectangle.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Output
        // Short Description: Project an image point onto the trackball 
        private void project_point_on_trackball(HTuple hv_X, HTuple hv_Y, HTuple hv_VirtualTrackball,
            HTuple hv_TrackballSize, out HTuple hv_V)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_R = new HTuple(), hv_XP = new HTuple();
            HTuple hv_YP = new HTuple(), hv_ZP = new HTuple();
            // Initialize local and output iconic variables 
            if ((int)(new HTuple(hv_VirtualTrackball.TupleEqual("shoemake"))) != 0)
            {
                //Virtual Trackball according to Shoemake
                hv_R = (((hv_X * hv_X) + (hv_Y * hv_Y))).TupleSqrt();
                if ((int)(new HTuple(hv_R.TupleLessEqual(hv_TrackballSize))) != 0)
                {
                    hv_XP = hv_X.Clone();
                    hv_YP = hv_Y.Clone();
                    hv_ZP = (((hv_TrackballSize * hv_TrackballSize) - (hv_R * hv_R))).TupleSqrt();
                }
                else
                {
                    hv_XP = (hv_X * hv_TrackballSize) / hv_R;
                    hv_YP = (hv_Y * hv_TrackballSize) / hv_R;
                    hv_ZP = 0;
                }
            }
            else
            {
                //Virtual Trackball according to Bell
                hv_R = (((hv_X * hv_X) + (hv_Y * hv_Y))).TupleSqrt();
                if ((int)(new HTuple(hv_R.TupleLessEqual(hv_TrackballSize * 0.70710678))) != 0)
                {
                    hv_XP = hv_X.Clone();
                    hv_YP = hv_Y.Clone();
                    hv_ZP = (((hv_TrackballSize * hv_TrackballSize) - (hv_R * hv_R))).TupleSqrt();
                }
                else
                {
                    hv_XP = hv_X.Clone();
                    hv_YP = hv_Y.Clone();
                    hv_ZP = ((0.6 * hv_TrackballSize) * hv_TrackballSize) / hv_R;
                }
            }
            hv_V = new HTuple();
            hv_V = hv_V.TupleConcat(hv_XP);
            hv_V = hv_V.TupleConcat(hv_YP);
            hv_V = hv_V.TupleConcat(hv_ZP);

            return;
        }

        // Chapter: Tuple / Arithmetic
        // Short Description: Calculates the cross product of two vectors of length 3. 
        private void tuple_vector_cross_product(HTuple hv_V1, HTuple hv_V2, out HTuple hv_VC)
        {



            // Local iconic variables 
            // Initialize local and output iconic variables 
            //The caller must ensure that the length of both input vectors is 3
            hv_VC = ((hv_V1.TupleSelect(1)) * (hv_V2.TupleSelect(2))) - ((hv_V1.TupleSelect(2)) * (hv_V2.TupleSelect(
                1)));
            hv_VC = hv_VC.TupleConcat(((hv_V1.TupleSelect(2)) * (hv_V2.TupleSelect(0))) - ((hv_V1.TupleSelect(
                0)) * (hv_V2.TupleSelect(2))));
            hv_VC = hv_VC.TupleConcat(((hv_V1.TupleSelect(0)) * (hv_V2.TupleSelect(1))) - ((hv_V1.TupleSelect(
                1)) * (hv_V2.TupleSelect(0))));

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Compute the 3d rotation from the mose movement 
        private void trackball(HTuple hv_MX1, HTuple hv_MY1, HTuple hv_MX2, HTuple hv_MY2,
            HTuple hv_VirtualTrackball, HTuple hv_TrackballSize, HTuple hv_SensFactor, out HTuple hv_QuatRotation)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_D = null, hv_P2 = null, hv_P1 = null;
            HTuple hv_T = null, hv_RotAngle = null, hv_Len = null;
            HTuple hv_RotAxis = null;
            // Initialize local and output iconic variables 
            hv_QuatRotation = new HTuple();
            //Compute the 3d rotation from the mouse movement
            //
            if ((int)((new HTuple(hv_MX1.TupleEqual(hv_MX2))).TupleAnd(new HTuple(hv_MY1.TupleEqual(
                hv_MY2)))) != 0)
            {
                hv_QuatRotation = new HTuple();
                hv_QuatRotation[0] = 1;
                hv_QuatRotation[1] = 0;
                hv_QuatRotation[2] = 0;
                hv_QuatRotation[3] = 0;

                return;
            }
            //Project the image point onto the trackball
            project_point_on_trackball(hv_MX1, hv_MY1, hv_VirtualTrackball, hv_TrackballSize,
                out hv_P1);
            project_point_on_trackball(hv_MX2, hv_MY2, hv_VirtualTrackball, hv_TrackballSize,
                out hv_P2);
            //The cross product of the projected points defines the rotation axis
            tuple_vector_cross_product(hv_P1, hv_P2, out hv_RotAxis);
            //Compute the rotation angle
            hv_D = hv_P2 - hv_P1;
            hv_T = (((((hv_D * hv_D)).TupleSum())).TupleSqrt()) / (2.0 * hv_TrackballSize);
            if ((int)(new HTuple(hv_T.TupleGreater(1.0))) != 0)
            {
                hv_T = 1.0;
            }
            if ((int)(new HTuple(hv_T.TupleLess(-1.0))) != 0)
            {
                hv_T = -1.0;
            }
            hv_RotAngle = (2.0 * (hv_T.TupleAsin())) * hv_SensFactor;
            hv_Len = ((((hv_RotAxis * hv_RotAxis)).TupleSum())).TupleSqrt();
            if ((int)(new HTuple(hv_Len.TupleGreater(0.0))) != 0)
            {
                hv_RotAxis = hv_RotAxis / hv_Len;
            }
            HOperatorSet.AxisAngleToQuat(hv_RotAxis.TupleSelect(0), hv_RotAxis.TupleSelect(
                1), hv_RotAxis.TupleSelect(2), hv_RotAngle, out hv_QuatRotation);

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Get the center of the virtual trackback that is used to move the camera. 
        private void get_trackball_center(HTuple hv_SelectedObject, HTuple hv_TrackballRadiusPixel,
            HTuple hv_ObjectModel3D, HTuple hv_Poses, out HTuple hv_TBCenter, out HTuple hv_TBSize)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_NumModels = null, hv_Centers = null;
            HTuple hv_Diameter = null, hv_MD = null, hv_Weight = new HTuple();
            HTuple hv_SumW = null, hv_Index = null, hv_ObjectModel3DIDSelected = new HTuple();
            HTuple hv_PoseSelected = new HTuple(), hv_HomMat3D = new HTuple();
            HTuple hv_TBCenterCamX = new HTuple(), hv_TBCenterCamY = new HTuple();
            HTuple hv_TBCenterCamZ = new HTuple(), hv_InvSum = new HTuple();
            // Initialize local and output iconic variables 
            hv_TBCenter = new HTuple();
            hv_TBSize = new HTuple();
            hv_NumModels = new HTuple(hv_ObjectModel3D.TupleLength());
            if (hv_TBCenter == null)
                hv_TBCenter = new HTuple();
            hv_TBCenter[0] = 0;
            if (hv_TBCenter == null)
                hv_TBCenter = new HTuple();
            hv_TBCenter[1] = 0;
            if (hv_TBCenter == null)
                hv_TBCenter = new HTuple();
            hv_TBCenter[2] = 0;
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, "center", out hv_Centers);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, "diameter_axis_aligned_bounding_box",
                out hv_Diameter);
            //Normalize Diameter to use it as weights for a weighted mean of the individual centers
            hv_MD = hv_Diameter.TupleMean();
            if ((int)(new HTuple(hv_MD.TupleGreater(1e-10))) != 0)
            {
                hv_Weight = hv_Diameter / hv_MD;
            }
            else
            {
                hv_Weight = hv_Diameter.Clone();
            }
            hv_SumW = ((hv_Weight.TupleSelectMask(((hv_SelectedObject.TupleSgn())).TupleAbs()
                ))).TupleSum();
            if ((int)(new HTuple(hv_SumW.TupleLess(1e-10))) != 0)
            {
                hv_Weight = HTuple.TupleGenConst(new HTuple(hv_Weight.TupleLength()), 1.0);
                hv_SumW = ((hv_Weight.TupleSelectMask(((hv_SelectedObject.TupleSgn())).TupleAbs()
                    ))).TupleSum();
            }
            HTuple end_val18 = hv_NumModels - 1;
            HTuple step_val18 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val18, step_val18); hv_Index = hv_Index.TupleAdd(step_val18))
            {
                if ((int)(hv_SelectedObject.TupleSelect(hv_Index)) != 0)
                {
                    hv_ObjectModel3DIDSelected = hv_ObjectModel3D.TupleSelect(hv_Index);
                    hv_PoseSelected = hv_Poses.TupleSelectRange(hv_Index * 7, (hv_Index * 7) + 6);
                    HOperatorSet.PoseToHomMat3d(hv_PoseSelected, out hv_HomMat3D);
                    HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Centers.TupleSelect((hv_Index * 3) + 0),
                        hv_Centers.TupleSelect((hv_Index * 3) + 1), hv_Centers.TupleSelect((hv_Index * 3) + 2),
                        out hv_TBCenterCamX, out hv_TBCenterCamY, out hv_TBCenterCamZ);
                    if (hv_TBCenter == null)
                        hv_TBCenter = new HTuple();
                    hv_TBCenter[0] = (hv_TBCenter.TupleSelect(0)) + (hv_TBCenterCamX * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_TBCenter == null)
                        hv_TBCenter = new HTuple();
                    hv_TBCenter[1] = (hv_TBCenter.TupleSelect(1)) + (hv_TBCenterCamY * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_TBCenter == null)
                        hv_TBCenter = new HTuple();
                    hv_TBCenter[2] = (hv_TBCenter.TupleSelect(2)) + (hv_TBCenterCamZ * (hv_Weight.TupleSelect(
                        hv_Index)));
                }
            }
            if ((int)(new HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(0))) != 0)
            {
                hv_InvSum = 1.0 / hv_SumW;
                if (hv_TBCenter == null)
                    hv_TBCenter = new HTuple();
                hv_TBCenter[0] = (hv_TBCenter.TupleSelect(0)) * hv_InvSum;
                if (hv_TBCenter == null)
                    hv_TBCenter = new HTuple();
                hv_TBCenter[1] = (hv_TBCenter.TupleSelect(1)) * hv_InvSum;
                if (hv_TBCenter == null)
                    hv_TBCenter = new HTuple();
                hv_TBCenter[2] = (hv_TBCenter.TupleSelect(2)) * hv_InvSum;
                hv_TBSize = (0.5 + ((0.5 * (hv_SelectedObject.TupleSum())) / hv_NumModels)) * hv_TrackballRadiusPixel;
            }
            else
            {
                hv_TBCenter = new HTuple();
                hv_TBSize = 0;
            }

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Displays a continue button. 
        private void disp_continue_button(HTuple hv_WindowHandle)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_ContinueMessage = new HTuple(), hv_Exception = null;
            HTuple hv_Row = null, hv_Column = null, hv_Width = null;
            HTuple hv_Height = null, hv_Ascent = null, hv_Descent = null;
            HTuple hv_TextWidth = null, hv_TextHeight = null;
            // Initialize local and output iconic variables 
            //This procedure displays a 'Continue' text button
            //in the lower right corner of the screen.
            //It uses the procedure disp_message.
            //
            //Input parameters:
            //WindowHandle: The window, where the text shall be displayed
            //
            //Use the continue message set in the global variable gTerminationButtonLabel.
            //If this variable is not defined, set a standard text instead.
            //global tuple gTerminationButtonLabel
            try
            {
                hv_ContinueMessage = ExpGetGlobalVar_gTerminationButtonLabel().Clone();
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_ContinueMessage = "Continue";
            }
            //Display the continue button
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_Row, out hv_Column, out hv_Width,
                out hv_Height);
            HOperatorSet.GetStringExtents(hv_WindowHandle, (" " + hv_ContinueMessage) + " ",
                out hv_Ascent, out hv_Descent, out hv_TextWidth, out hv_TextHeight);
            disp_text_button(hv_WindowHandle, hv_ContinueMessage, "window", (hv_Height - hv_TextHeight) - 12,
                (hv_Width - hv_TextWidth) - 12, "black", "#f28f26");

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Get string extends of several lines. 
        private void max_line_width(HTuple hv_WindowHandle, HTuple hv_Lines, out HTuple hv_MaxWidth)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Index = null, hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_LineWidth = new HTuple();
            HTuple hv_LineHeight = new HTuple();
            // Initialize local and output iconic variables 
            hv_MaxWidth = 0;
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Lines.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.GetStringExtents(hv_WindowHandle, hv_Lines.TupleSelect(hv_Index),
                    out hv_Ascent, out hv_Descent, out hv_LineWidth, out hv_LineHeight);
                hv_MaxWidth = ((hv_LineWidth.TupleConcat(hv_MaxWidth))).TupleMax();
            }

            return;
        }

        // Chapter: Graphics / Parameters
        private void color_string_to_rgb(HTuple hv_Color, out HTuple hv_RGB)
        {



            // Local iconic variables 

            HObject ho_Rectangle, ho_Image;

            // Local control variables 

            HTuple hv_WindowHandleBuffer = null, hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Image);
            try
            {
                HOperatorSet.OpenWindow(0, 0, 1, 1, 0, "buffer", "", out hv_WindowHandleBuffer);
                HOperatorSet.SetPart(hv_WindowHandleBuffer, 0, 0, -1, -1);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, 0, 0);
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Color (must be a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                HOperatorSet.DispObj(ho_Rectangle, hv_WindowHandleBuffer);
                ho_Image.Dispose();
                HOperatorSet.DumpWindowImage(out ho_Image, hv_WindowHandleBuffer);
                HOperatorSet.CloseWindow(hv_WindowHandleBuffer);
                HOperatorSet.GetGrayval(ho_Image, 0, 0, out hv_RGB);
                hv_RGB = hv_RGB + ((new HTuple(0)).TupleConcat(0)).TupleConcat(0);
                ho_Rectangle.Dispose();
                ho_Image.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_Image.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Output
        // Short Description: Get the center of the virtual trackback that is used to move the camera (version for inspection_mode = 'surface'). 
        private void get_trackball_center_fixed(HTuple hv_SelectedObject, HTuple hv_TrackballCenterRow,
            HTuple hv_TrackballCenterCol, HTuple hv_TrackballRadiusPixel, HTuple hv_Scene3D,
            HTuple hv_ObjectModel3DID, HTuple hv_Poses, HTuple hv_WindowHandleBuffer, HTuple hv_CamParam,
            HTuple hv_GenParamName, HTuple hv_GenParamValue, out HTuple hv_TBCenter, out HTuple hv_TBSize)
        {



            // Local iconic variables 

            HObject ho_RegionCenter, ho_DistanceImage;
            HObject ho_Domain;

            // Local control variables 

            HTuple hv_NumModels = null, hv_Width = null;
            HTuple hv_Height = null, hv_SelectPose = null, hv_Index1 = null;
            HTuple hv_Rows = null, hv_Columns = null, hv_Grayval = null;
            HTuple hv_IndicesG = null, hv_Value = null, hv_Pos = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionCenter);
            HOperatorSet.GenEmptyObj(out ho_DistanceImage);
            HOperatorSet.GenEmptyObj(out ho_Domain);
            hv_TBCenter = new HTuple();
            hv_TBSize = new HTuple();
            try
            {
                //Determine the trackball center for the fixed trackball
                hv_NumModels = new HTuple(hv_ObjectModel3DID.TupleLength());
                hv_Width = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 2];
                hv_Height = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 1];
                //
                //Project the selected objects
                hv_SelectPose = new HTuple();
                for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_SelectedObject.TupleLength()
                    )) - 1); hv_Index1 = (int)hv_Index1 + 1)
                {
                    hv_SelectPose = hv_SelectPose.TupleConcat(HTuple.TupleGenConst(7, hv_SelectedObject.TupleSelect(
                        hv_Index1)));
                    if ((int)(new HTuple(((hv_SelectedObject.TupleSelect(hv_Index1))).TupleEqual(
                        0))) != 0)
                    {
                        HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index1, "visible",
                            "false");
                    }
                }
                HOperatorSet.SetScene3dParam(hv_Scene3D, "depth_persistence", "true");
                HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                HOperatorSet.SetScene3dParam(hv_Scene3D, "visible", "true");
                //
                //determine the depth of the object point that appears closest to the trackball
                //center
                ho_RegionCenter.Dispose();
                HOperatorSet.GenRegionPoints(out ho_RegionCenter, hv_TrackballCenterRow, hv_TrackballCenterCol);
                ho_DistanceImage.Dispose();
                HOperatorSet.DistanceTransform(ho_RegionCenter, out ho_DistanceImage, "chamfer-3-4-unnormalized",
                    "false", hv_Width, hv_Height);
                ho_Domain.Dispose();
                HOperatorSet.GetDomain(ho_DistanceImage, out ho_Domain);
                HOperatorSet.GetRegionPoints(ho_Domain, out hv_Rows, out hv_Columns);
                HOperatorSet.GetGrayval(ho_DistanceImage, hv_Rows, hv_Columns, out hv_Grayval);
                HOperatorSet.TupleSortIndex(hv_Grayval, out hv_IndicesG);
                HOperatorSet.GetDisplayScene3dInfo(hv_WindowHandleBuffer, hv_Scene3D, hv_Rows.TupleSelect(
                    hv_IndicesG), hv_Columns.TupleSelect(hv_IndicesG), "depth", out hv_Value);
                HOperatorSet.TupleFind(hv_Value.TupleSgn(), 1, out hv_Pos);
                //
                HOperatorSet.SetScene3dParam(hv_Scene3D, "depth_persistence", "false");
                //
                //
                //set TBCenter
                if ((int)(new HTuple(hv_Pos.TupleNotEqual(-1))) != 0)
                {
                    //if the object is visible in the image
                    hv_TBCenter = new HTuple();
                    hv_TBCenter[0] = 0;
                    hv_TBCenter[1] = 0;
                    hv_TBCenter = hv_TBCenter.TupleConcat(hv_Value.TupleSelect(
                        hv_Pos.TupleSelect(0)));
                }
                else
                {
                    //if the object is not visible in the image, set the z coordinate to -1
                    //to indicate, the the previous z value should be used instead
                    hv_TBCenter = new HTuple();
                    hv_TBCenter[0] = 0;
                    hv_TBCenter[1] = 0;
                    hv_TBCenter[2] = -1;
                }
                //
                if ((int)(new HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(0))) != 0)
                {
                    hv_TBSize = (0.5 + ((0.5 * (hv_SelectedObject.TupleSum())) / hv_NumModels)) * hv_TrackballRadiusPixel;
                }
                else
                {
                    hv_TBCenter = new HTuple();
                    hv_TBSize = 0;
                }
                ho_RegionCenter.Dispose();
                ho_DistanceImage.Dispose();
                ho_Domain.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionCenter.Dispose();
                ho_DistanceImage.Dispose();
                ho_Domain.Dispose();

                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 可视化3D对象模型
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_ObjectModel3D"></param>
        /// <param name="hv_CamParam"></param>
        /// <param name="hv_PoseIn"></param>
        /// <param name="hv_GenParamName"></param>
        /// <param name="hv_GenParamValue"></param>
        /// <param name="hv_Title"></param>
        /// <param name="hv_Label"></param>
        /// <param name="hv_Information"></param>
        /// <param name="hv_PoseOut"></param>
        private void visualize_object_model_3d(HTuple hv_WindowHandle, HTuple hv_ObjectModel3D,
HTuple hv_CamParam, HTuple hv_PoseIn, HTuple hv_GenParamName, HTuple hv_GenParamValue,
HTuple hv_Title, HTuple hv_Label, HTuple hv_Information, out HTuple hv_PoseOut)
        {
            HObject ho_Image, ho_ImageDump = null;
            HTuple ExpTmpLocalVar_gDispObjOffset = null;
            HTuple ExpTmpLocalVar_gLabelsDecor = null, ExpTmpLocalVar_gInfoDecor = null;
            HTuple ExpTmpLocalVar_gInfoPos = null, ExpTmpLocalVar_gTitlePos = null;
            HTuple ExpTmpLocalVar_gTitleDecor = null, ExpTmpLocalVar_gTerminationButtonLabel = null;
            HTuple ExpTmpLocalVar_gAlphaDeselected = null, ExpTmpLocalVar_gIsSinglePose = new HTuple();
            HTuple ExpTmpLocalVar_gUsesOpenGL = null, hv_TrackballSize = null;
            HTuple hv_VirtualTrackball = null, hv_MouseMapping = null;
            HTuple hv_WaitForButtonRelease = null, hv_MaxNumModels = null;
            HTuple hv_WindowCenteredRotation = null, hv_NumModels = null;
            HTuple hv_SelectedObject = null, hv_ClipRegion = null;
            HTuple hv_CPLength = null, hv_RowNotUsed = null, hv_ColumnNotUsed = null;
            HTuple hv_Width = null, hv_Height = null, hv_WPRow1 = null;
            HTuple hv_WPColumn1 = null, hv_WPRow2 = null, hv_WPColumn2 = null;
            HTuple hv_CamWidth = new HTuple(), hv_CamHeight = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Indices = null, hv_DispBackground = null;
            HTuple hv_Mask = new HTuple(), hv_Center = null, hv_Poses = new HTuple();
            HTuple hv_HomMat3Ds = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_PoseEstimated = new HTuple(), hv_WindowHandleBuffer = null;
            HTuple hv_Font = null, hv_Exception = null, hv_OpenGLInfo = new HTuple();
            HTuple hv_DummyObjectModel3D = new HTuple(), hv_Scene3DTest = new HTuple();
            HTuple hv_CameraIndexTest = new HTuple(), hv_PoseTest = new HTuple();
            HTuple hv_InstanceIndexTest = new HTuple(), hv_MinImageSize = null;
            HTuple hv_TrackballRadiusPixel = null, hv_Ascent = null;
            HTuple hv_Descent = null, hv_TextWidth = null, hv_TextHeight = null;
            HTuple hv_NumChannels = null, hv_ColorImage = null, hv_Scene3D = null;
            HTuple hv_CameraIndex = null, hv_AllInstances = null, hv_SetLight = null;
            HTuple hv_LightParam = new HTuple(), hv_LightPosition = new HTuple();
            HTuple hv_LightKind = new HTuple(), hv_LightIndex = new HTuple();
            HTuple hv_PersistenceParamName = null, hv_PersistenceParamValue = null;
            HTuple hv_ValueListSS3P = null, hv_ValueListSS3IP = null;
            HTuple hv_AlphaOrig = null, hv_UsedParamMask = null, hv_I = null;
            HTuple hv_ParamName = new HTuple(), hv_ParamValue = new HTuple();
            HTuple hv_UseParam = new HTuple(), hv_ParamNameTrunk = new HTuple();
            HTuple hv_Instance = new HTuple(), hv_GenParamNameRemaining = new HTuple();
            HTuple hv_GenParamValueRemaining = new HTuple(), hv_HomMat3D = null;
            HTuple hv_Qx = null, hv_Qy = null, hv_Qz = null, hv_TBCenter = null;
            HTuple hv_TBSize = null, hv_ButtonHold = null, hv_VisualizeTB = new HTuple();
            HTuple hv_MaxIndex = new HTuple(), hv_TrackballCenterRow = new HTuple();
            HTuple hv_TrackballCenterCol = new HTuple(), hv_GraphEvent = new HTuple();
            HTuple hv_Exit = new HTuple(), hv_GraphButtonRow = new HTuple();
            HTuple hv_GraphButtonColumn = new HTuple(), hv_GraphButton = new HTuple();
            HTuple hv_ButtonReleased = new HTuple();
            HTuple hv_CamParam_COPY_INP_TMP = hv_CamParam.Clone();
            HTuple hv_GenParamName_COPY_INP_TMP = hv_GenParamName.Clone();
            HTuple hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue.Clone();
            HTuple hv_Label_COPY_INP_TMP = hv_Label.Clone();
            HTuple hv_PoseIn_COPY_INP_TMP = hv_PoseIn.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageDump);
            hv_PoseOut = new HTuple();
            try
            {
                hv_TrackballSize = 1;
                hv_VirtualTrackball = "shoemake";
                //VirtualTrackball := 'bell'

                hv_MouseMapping = new HTuple();
                hv_MouseMapping[0] = 17;
                hv_MouseMapping[1] = 1;
                hv_MouseMapping[2] = 2;
                hv_MouseMapping[3] = 5;
                hv_MouseMapping[4] = 9;
                hv_MouseMapping[5] = 4;
                hv_MouseMapping[6] = 49;
                //
                //The labels of the objects appear next to their projected
                //center. With gDispObjOffset a fixed offset is added
                //                  R,  C
                ExpTmpLocalVar_gDispObjOffset = new HTuple();
                ExpTmpLocalVar_gDispObjOffset[0] = -30;
                ExpTmpLocalVar_gDispObjOffset[1] = 0;
                ExpSetGlobalVar_gDispObjOffset(ExpTmpLocalVar_gDispObjOffset);
                //
                //Customize the decoration of the different text elements
                //              Color,   Box
                ExpTmpLocalVar_gInfoDecor = new HTuple();
                ExpTmpLocalVar_gInfoDecor[0] = "white";
                ExpTmpLocalVar_gInfoDecor[1] = "false";
                ExpSetGlobalVar_gInfoDecor(ExpTmpLocalVar_gInfoDecor);
                ExpTmpLocalVar_gLabelsDecor = new HTuple();
                ExpTmpLocalVar_gLabelsDecor[0] = "white";
                ExpTmpLocalVar_gLabelsDecor[1] = "false";
                ExpSetGlobalVar_gLabelsDecor(ExpTmpLocalVar_gLabelsDecor);
                ExpTmpLocalVar_gTitleDecor = new HTuple();
                ExpTmpLocalVar_gTitleDecor[0] = "black";
                ExpTmpLocalVar_gTitleDecor[1] = "true";
                ExpSetGlobalVar_gTitleDecor(ExpTmpLocalVar_gTitleDecor);
                //
                //Customize the position of some text elements
                //  gInfoPos has one of the values
                //  {'UpperLeft', 'LowerLeft', 'UpperRight'}
                ExpTmpLocalVar_gInfoPos = "LowerLeft";
                ExpSetGlobalVar_gInfoPos(ExpTmpLocalVar_gInfoPos);
                //  gTitlePos has one of the values
                //  {'UpperLeft', 'UpperCenter', 'UpperRight'}
                ExpTmpLocalVar_gTitlePos = "UpperLeft";
                ExpSetGlobalVar_gTitlePos(ExpTmpLocalVar_gTitlePos);
                //Alpha value (=1-transparency) that is used for visualizing
                //the objects that are not selected
                ExpTmpLocalVar_gAlphaDeselected = 0.3;
                ExpSetGlobalVar_gAlphaDeselected(ExpTmpLocalVar_gAlphaDeselected);
                //Customize the label of the continue button
                ExpTmpLocalVar_gTerminationButtonLabel = " Continue ";
                ExpSetGlobalVar_gTerminationButtonLabel(ExpTmpLocalVar_gTerminationButtonLabel);
                hv_WaitForButtonRelease = "true";
                //supported anymore
                hv_MaxNumModels = 50000;
                hv_WindowCenteredRotation = 2;
                //Initialize some values
                hv_NumModels = new HTuple(hv_ObjectModel3D.TupleLength());
                hv_SelectedObject = HTuple.TupleGenConst(hv_NumModels, 1);
                //
                //Apply some system settings

                HOperatorSet.GetSystem("clip_region", out hv_ClipRegion);
                HOperatorSet.SetSystem("clip_region", "false");
                dev_update_off();
                //
                //Refactor camera parameters to fit to window size
                //
                hv_CPLength = new HTuple(hv_CamParam_COPY_INP_TMP.TupleLength());
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowNotUsed, out hv_ColumnNotUsed,
                    out hv_Width, out hv_Height);
                HOperatorSet.GetPart(hv_WindowHandle, out hv_WPRow1, out hv_WPColumn1, out hv_WPRow2,
                    out hv_WPColumn2);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height - 1, hv_Width - 1);
                if ((int)(new HTuple(hv_CPLength.TupleEqual(0))) != 0)
                {
                    hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[0] = 0.06;
                    hv_CamParam_COPY_INP_TMP[1] = 0;
                    hv_CamParam_COPY_INP_TMP[2] = 8.5e-6;
                    hv_CamParam_COPY_INP_TMP[3] = 8.5e-6;
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Width / 2);
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Height / 2);
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Width);
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Height);
                    hv_CPLength = new HTuple(hv_CamParam_COPY_INP_TMP.TupleLength());
                }
                else
                {
                    hv_CamWidth = ((hv_CamParam_COPY_INP_TMP.TupleSelect(hv_CPLength - 2))).TupleReal()
                        ;
                    hv_CamHeight = ((hv_CamParam_COPY_INP_TMP.TupleSelect(hv_CPLength - 1))).TupleReal()
                        ;
                    hv_Scale = ((((hv_Width / hv_CamWidth)).TupleConcat(hv_Height / hv_CamHeight))).TupleMin()
                        ;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 6] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 6)) / hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 5] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 5)) / hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 4] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 4)) * hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 3] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 3)) * hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 2] = (((hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 2)) * hv_Scale)).TupleInt();
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 1] = (((hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 1)) * hv_Scale)).TupleInt();
                }
                //
                //Check the generic parameters for window_centered_rotation
                //(Note that the default is set above to WindowCenteredRotation := 2)
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("inspection_mode");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        0)))).TupleEqual("surface"))) != 0)
                    {
                        hv_WindowCenteredRotation = 1;
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(
                        hv_Indices.TupleSelect(0)))).TupleEqual("standard"))) != 0)
                    {
                        hv_WindowCenteredRotation = 2;
                    }
                    else
                    {
                        //Wrong parameter value, use default value
                    }
                    hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices);
                    hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove(
                        hv_Indices);
                }
                //
                //Check the generic parameters for disp_background
                //(The former parameter name 'use_background' is still supported
                // for compatibility reasons)
                hv_DispBackground = "false";
                if ((int)(new HTuple((new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    )).TupleGreater(0))) != 0)
                {
                    hv_Mask = ((hv_GenParamName_COPY_INP_TMP.TupleEqualElem("disp_background"))).TupleOr(
                        hv_GenParamName_COPY_INP_TMP.TupleEqualElem("use_background"));
                    hv_Indices = hv_Mask.TupleFind(1);
                }
                else
                {
                    hv_Indices = -1;
                }
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    hv_DispBackground = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        0));
                    if ((int)((new HTuple(hv_DispBackground.TupleNotEqual("true"))).TupleAnd(
                        new HTuple(hv_DispBackground.TupleNotEqual("false")))) != 0)
                    {
                        //Wrong parameter value: Only 'true' and 'false' are allowed
                        throw new HalconException("Wrong value for parameter 'disp_background' (must be either 'true' or 'false')");
                    }
                    //Note the the background is handled explicitely in this procedure
                    //and therefore, the parameter is removed from the list of
                    //parameters and disp_background is always set to true (see below)
                    hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices);
                    hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove(
                        hv_Indices);
                }
                //
                //Read and check the parameter Label for each object
                if ((int)(new HTuple((new HTuple(hv_Label_COPY_INP_TMP.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    hv_Label_COPY_INP_TMP = 0;
                }
                else if ((int)(new HTuple((new HTuple(hv_Label_COPY_INP_TMP.TupleLength()
                    )).TupleEqual(1))) != 0)
                {
                    hv_Label_COPY_INP_TMP = HTuple.TupleGenConst(hv_NumModels, hv_Label_COPY_INP_TMP);
                }
                else
                {
                    if ((int)(new HTuple((new HTuple(hv_Label_COPY_INP_TMP.TupleLength())).TupleNotEqual(
                        hv_NumModels))) != 0)
                    {
                        //Error: Number of elements in Label does not match the
                        //number of object models
                        // stop(); only in hdevelop
                    }
                }
                //
                //Read and check the parameter PoseIn for each object
                get_object_models_center(hv_ObjectModel3D, out hv_Center);
                if ((int)(new HTuple((new HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    //If no pose was specified by the caller, automatically calculate
                    //a pose that is appropriate for the visualization.
                    //Set the initial model reference pose. The orientation is parallel
                    //to the object coordinate system, the position is at the center
                    //of gravity of all models.
                    HOperatorSet.CreatePose(-(hv_Center.TupleSelect(0)), -(hv_Center.TupleSelect(
                        1)) * -1, -(hv_Center.TupleSelect(2)), 180, 0, 0, "Rp+T", "gba", "point", out hv_PoseIn_COPY_INP_TMP);
                    determine_optimum_pose_distance(hv_ObjectModel3D, hv_CamParam_COPY_INP_TMP,
                        0.9, hv_PoseIn_COPY_INP_TMP, out hv_PoseEstimated);
                    hv_Poses = new HTuple();
                    hv_HomMat3Ds = new HTuple();
                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                    hv_Poses = hv_PoseEstimated.TupleSelect(hv_Sequence % 7);
                    ExpTmpLocalVar_gIsSinglePose = 1;
                    ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                }
                else if ((int)(new HTuple((new HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength()
                    )).TupleEqual(7))) != 0)
                {
                    hv_Poses = new HTuple();
                    hv_HomMat3Ds = new HTuple();
                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                    hv_Poses = hv_PoseIn_COPY_INP_TMP.TupleSelect(hv_Sequence % 7);
                    ExpTmpLocalVar_gIsSinglePose = 1;
                    ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                }
                else
                {
                    if ((int)(new HTuple((new HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength())).TupleNotEqual(
                        (new HTuple(hv_ObjectModel3D.TupleLength())) * 7))) != 0)
                    {
                        //Error: Wrong number of values of input control parameter 'PoseIn'
                        // stop(); only in hdevelop
                    }
                    else
                    {
                        hv_Poses = hv_PoseIn_COPY_INP_TMP.Clone();
                    }
                    ExpTmpLocalVar_gIsSinglePose = 0;
                    ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                }
                //
                //Open (invisible) buffer window to avoid flickering
                HOperatorSet.OpenWindow(0, 0, hv_Width, hv_Height, 0, "buffer", "", out hv_WindowHandleBuffer);
                HOperatorSet.SetPart(hv_WindowHandleBuffer, 0, 0, hv_Height - 1, hv_Width - 1);
                HOperatorSet.GetFont(hv_WindowHandle, out hv_Font);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandleBuffer, hv_Font);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                //
                // Is OpenGL available and should it be used?
                ExpTmpLocalVar_gUsesOpenGL = "true";
                ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("opengl");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    ExpTmpLocalVar_gUsesOpenGL = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        0));
                    ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                    hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices);
                    hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove(
                        hv_Indices);
                    if ((int)((new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleNotEqual("true"))).TupleAnd(
                        new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleNotEqual("false")))) != 0)
                    {
                        //Wrong parameter value: Only 'true' and 'false' are allowed
                        throw new HalconException("Wrong value for parameter 'opengl' (must be either 'true' or 'false')");
                    }
                }
                if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("true"))) != 0)
                {
                    HOperatorSet.GetSystem("opengl_info", out hv_OpenGLInfo);
                    if ((int)(new HTuple(hv_OpenGLInfo.TupleEqual("No OpenGL support included."))) != 0)
                    {
                        ExpTmpLocalVar_gUsesOpenGL = "false";
                        ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                    }
                    else
                    {
                        HOperatorSet.GenObjectModel3dFromPoints(0, 0, 0, out hv_DummyObjectModel3D);
                        HOperatorSet.CreateScene3d(out hv_Scene3DTest);
                        HOperatorSet.AddScene3dCamera(hv_Scene3DTest, hv_CamParam_COPY_INP_TMP,
                            out hv_CameraIndexTest);
                        determine_optimum_pose_distance(hv_DummyObjectModel3D, hv_CamParam_COPY_INP_TMP,
                            0.9, ((((((new HTuple(0)).TupleConcat(0)).TupleConcat(0)).TupleConcat(
                            0)).TupleConcat(0)).TupleConcat(0)).TupleConcat(0), out hv_PoseTest);
                        HOperatorSet.AddScene3dInstance(hv_Scene3DTest, hv_DummyObjectModel3D,
                            hv_PoseTest, out hv_InstanceIndexTest);
                        try
                        {
                            HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3DTest, hv_InstanceIndexTest);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            ExpTmpLocalVar_gUsesOpenGL = "false";
                            ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                        }
                        HOperatorSet.ClearScene3d(hv_Scene3DTest);
                        HOperatorSet.ClearObjectModel3d(hv_DummyObjectModel3D);
                    }
                }
                //
                //Compute the trackball
                hv_MinImageSize = ((hv_Width.TupleConcat(hv_Height))).TupleMin();
                hv_TrackballRadiusPixel = (hv_TrackballSize * hv_MinImageSize) / 2.0;
                //
                //Measure the text extents for the continue button in the
                //graphics window
                HOperatorSet.GetStringExtents(hv_WindowHandleBuffer, ExpGetGlobalVar_gTerminationButtonLabel() + "  ",
                    out hv_Ascent, out hv_Descent, out hv_TextWidth, out hv_TextHeight);
                //
                //Store background image
                if ((int)(new HTuple(hv_DispBackground.TupleEqual("false"))) != 0)
                {
                    HOperatorSet.ClearWindow(hv_WindowHandle);
                }
                ho_Image.Dispose();
                HOperatorSet.DumpWindowImage(out ho_Image, hv_WindowHandle);
                //Special treatment for color background images necessary
                HOperatorSet.CountChannels(ho_Image, out hv_NumChannels);
                hv_ColorImage = new HTuple(hv_NumChannels.TupleEqual(3));
                //
                HOperatorSet.CreateScene3d(out hv_Scene3D);
                HOperatorSet.AddScene3dCamera(hv_Scene3D, hv_CamParam_COPY_INP_TMP, out hv_CameraIndex);
                HOperatorSet.AddScene3dInstance(hv_Scene3D, hv_ObjectModel3D, hv_Poses, out hv_AllInstances);
                //Always set 'disp_background' to true,  because it is handled explicitely
                //in this procedure (see above)
                HOperatorSet.SetScene3dParam(hv_Scene3D, "disp_background", "true");
                //Check if we have to set light specific parameters
                hv_SetLight = new HTuple(hv_GenParamName_COPY_INP_TMP.TupleRegexpTest("light_"));
                if ((int)(hv_SetLight) != 0)
                {
                    //set position of light source
                    hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("light_position");
                    if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                        new HTuple())))) != 0)
                    {
                        //If multiple light positions are given, use the last one
                        hv_LightParam = ((((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                            (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleSplit(", "))).TupleNumber()
                            ;
                        if ((int)(new HTuple((new HTuple(hv_LightParam.TupleLength())).TupleNotEqual(
                            4))) != 0)
                        {
                            throw new HalconException("light_position must be given as a string that contains four space separated floating point numbers");
                        }
                        hv_LightPosition = hv_LightParam.TupleSelectRange(0, 2);
                        hv_LightKind = "point_light";
                        if ((int)(new HTuple(((hv_LightParam.TupleSelect(3))).TupleEqual(0))) != 0)
                        {
                            hv_LightKind = "directional_light";
                        }
                        //Currently, only one light source is supported
                        HOperatorSet.RemoveScene3dLight(hv_Scene3D, 0);
                        HOperatorSet.AddScene3dLight(hv_Scene3D, hv_LightPosition, hv_LightKind,
                            out hv_LightIndex);
                        HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                        HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                    }
                    //set ambient part of light source
                    hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("light_ambient");
                    if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                        new HTuple())))) != 0)
                    {
                        //If the ambient part is set multiple times, use the last setting
                        hv_LightParam = ((((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                            (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleSplit(", "))).TupleNumber()
                            ;
                        if ((int)(new HTuple((new HTuple(hv_LightParam.TupleLength())).TupleLess(
                            3))) != 0)
                        {
                            throw new HalconException("light_ambient must be given as a string that contains three space separated floating point numbers");
                        }
                        HOperatorSet.SetScene3dLightParam(hv_Scene3D, 0, "ambient", hv_LightParam.TupleSelectRange(
                            0, 2));
                        HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                        HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                    }
                    //set diffuse part of light source
                    hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("light_diffuse");
                    if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                        new HTuple())))) != 0)
                    {
                        //If the diffuse part is set multiple times, use the last setting
                        hv_LightParam = ((((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                            (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleSplit(", "))).TupleNumber()
                            ;
                        if ((int)(new HTuple((new HTuple(hv_LightParam.TupleLength())).TupleLess(
                            3))) != 0)
                        {
                            throw new HalconException("light_diffuse must be given as a string that contains three space separated floating point numbers");
                        }
                        HOperatorSet.SetScene3dLightParam(hv_Scene3D, 0, "diffuse", hv_LightParam.TupleSelectRange(
                            0, 2));
                        HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                        HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                    }
                }
                //
                //Handle persistence parameters separately because persistence will
                //only be activated immediately before leaving the visualization
                //procedure
                hv_PersistenceParamName = new HTuple();
                hv_PersistenceParamValue = new HTuple();
                //set position of light source
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("object_index_persistence");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual("true"))) != 0)
                    {
                        hv_PersistenceParamName = hv_PersistenceParamName.TupleConcat("object_index_persistence");
                        hv_PersistenceParamValue = hv_PersistenceParamValue.TupleConcat("true");
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(
                        hv_Indices.TupleSelect((new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual(
                        "false"))) != 0)
                    {
                    }
                    else
                    {
                        throw new HalconException("Wrong value for parameter 'object_index_persistence' (must be either 'true' or 'false')");
                    }
                    HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                    HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                }
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("depth_persistence");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual("true"))) != 0)
                    {
                        hv_PersistenceParamName = hv_PersistenceParamName.TupleConcat("depth_persistence");
                        hv_PersistenceParamValue = hv_PersistenceParamValue.TupleConcat("true");
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(
                        hv_Indices.TupleSelect((new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual(
                        "false"))) != 0)
                    {
                    }
                    else
                    {
                        throw new HalconException("Wrong value for parameter 'depth_persistence' (must be either 'true' or 'false')");
                    }
                    HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                    HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                }
                //
                //Parse the generic parameters  SetScene3dParam  set_scene_3d_param SetScene3dInstanceParam set_scene_3d_instance_param
                //- First, all parameters that are understood by set_scene_3d_instance_param
                // HOperatorSet.GetParamInfo("set_scene_3d_param", "GenParamName", "value_list",out hv_ValueListSS3P);   // 这个算子只支持32位操作，在64位版本上会报错
                // HOperatorSet.GetParamInfo("set_scene_3d_instance_param", "GenParamName", "value_list",out hv_ValueListSS3IP);
                //hv_ValueListSS3P = HInfo.GetParamInfo("set_scene_3d_param", "GenParamName", "value_list");  // 这个算子只支持32位操作，在64位版本上会报错
                //hv_ValueListSS3IP = HInfo.GetParamInfo("set_scene_3d_instance_param", "GenParamName", "value_list");
                /////////////////////////////  
                hv_ValueListSS3P = new HTuple("alpha", "attribute", "color", "colored", "depth_persistence", "disp_background", "disp_lines",
                    "disp_pose", "disp_normals", "line_color", "line_width", "normal_color", "object_index_persistence", "point_size",
                    "quality", "compatibility_mode_enable", "intensity", "intensity_red", "intensity_green", "intensity_blue", "lut", "visible",
                    "color_attrib_start", "color_attrib_end", "color_attrib");
                hv_ValueListSS3IP = new HTuple("alpha", "attribute", "color", "disp_lines", "disp_pose", "disp_normals", "line_color",
                    "line_width", "normal_color", "intensity", "intensity_red", "intensity_green", "intensity_blue", "lut", "visible", "point_size");
                //////////////////////////////////////////////////////////////////
                hv_AlphaOrig = HTuple.TupleGenConst(hv_NumModels, 1);
                hv_UsedParamMask = HTuple.TupleGenConst(new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    ), 0);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_ParamName = hv_GenParamName_COPY_INP_TMP.TupleSelect(hv_I);
                    hv_ParamValue = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_I);
                    //Check if this parameter is understood by set_scene_3d_param
                    hv_UseParam = new HTuple(hv_ValueListSS3P.TupleRegexpTest(("^" + hv_ParamName) + "$"));
                    if ((int)(hv_UseParam) != 0)
                    {
                        try
                        {
                            HOperatorSet.SetScene3dParam(hv_Scene3D, hv_ParamName, hv_ParamValue);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            if ((int)((new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1204))).TupleOr(
                                new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1304)))) != 0)
                            {
                                throw new HalconException((("Wrong type or value for parameter " + hv_ParamName) + ": ") + hv_ParamValue);
                            }
                            else
                            {
                                throw new HalconException(hv_Exception);
                            }
                        }
                        if (hv_UsedParamMask == null)
                            hv_UsedParamMask = new HTuple();
                        hv_UsedParamMask[hv_I] = 1;
                        if ((int)(new HTuple(hv_ParamName.TupleEqual("alpha"))) != 0)
                        {
                            hv_AlphaOrig = HTuple.TupleGenConst(hv_NumModels, hv_ParamValue);
                        }
                        continue;
                    }
                    //Check if it is a parameter that is valid for only one instance
                    //and therefore can be set only with set_scene_3d_instance_param
                    hv_ParamNameTrunk = hv_ParamName.TupleRegexpReplace("_\\d+$", "");
                    hv_UseParam = new HTuple(hv_ValueListSS3IP.TupleRegexpTest(("^" + hv_ParamNameTrunk) + "$"));
                    if ((int)(hv_UseParam) != 0)
                    {
                        hv_Instance = ((hv_ParamName.TupleRegexpReplace(("^" + hv_ParamNameTrunk) + "_(\\d+)$",
                            "$1"))).TupleNumber();
                        if ((int)((new HTuple(hv_Instance.TupleLess(0))).TupleOr(new HTuple(hv_Instance.TupleGreater(
                            hv_NumModels - 1)))) != 0)
                        {
                            throw new HalconException(("Parameter " + hv_ParamName) + " refers to a non existing 3D object model");
                        }
                        try
                        {
                            HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Instance, hv_ParamNameTrunk,
                                hv_ParamValue);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            if ((int)((new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1204))).TupleOr(
                                new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1304)))) != 0)
                            {
                                throw new HalconException((("Wrong type or value for parameter " + hv_ParamName) + ": ") + hv_ParamValue);
                            }
                            else
                            {
                                throw new HalconException(hv_Exception);
                            }
                        }
                        if (hv_UsedParamMask == null)
                            hv_UsedParamMask = new HTuple();
                        hv_UsedParamMask[hv_I] = 1;
                        if ((int)(new HTuple(hv_ParamNameTrunk.TupleEqual("alpha"))) != 0)
                        {
                            if (hv_AlphaOrig == null)
                                hv_AlphaOrig = new HTuple();
                            hv_AlphaOrig[hv_Instance] = hv_ParamValue;
                        }
                        continue;
                    }
                }
                //
                //Check if there are remaining parameters
                if ((int)(new HTuple((new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    )).TupleGreater(0))) != 0)
                {
                    hv_GenParamNameRemaining = hv_GenParamName_COPY_INP_TMP.TupleSelectMask(hv_UsedParamMask.TupleNot()
                        );
                    hv_GenParamValueRemaining = hv_GenParamValue_COPY_INP_TMP.TupleSelectMask(
                        hv_UsedParamMask.TupleNot());
                    if ((int)(new HTuple(hv_GenParamNameRemaining.TupleNotEqual(new HTuple()))) != 0)
                    {
                        throw new HalconException("Parameters that cannot be handled: " + (((((hv_GenParamNameRemaining + " := ") + hv_GenParamValueRemaining) + ", ")).TupleSum()
                            ));
                    }
                }
                //
                //Start the visualization loop
                HOperatorSet.PoseToHomMat3d(hv_Poses.TupleSelectRange(0, 6), out hv_HomMat3D);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Center.TupleSelect(0), hv_Center.TupleSelect(
                    1), hv_Center.TupleSelect(2), out hv_Qx, out hv_Qy, out hv_Qz);
                hv_TBCenter = new HTuple();
                hv_TBCenter = hv_TBCenter.TupleConcat(hv_Qx);
                hv_TBCenter = hv_TBCenter.TupleConcat(hv_Qy);
                hv_TBCenter = hv_TBCenter.TupleConcat(hv_Qz);
                hv_TBSize = (0.5 + ((0.5 * (hv_SelectedObject.TupleSum())) / hv_NumModels)) * hv_TrackballRadiusPixel;
                hv_ButtonHold = 0;

                // 循环显示
                while ((int)(1) != 0)
                // while (!token.IsCancellationRequested)
                {
                    hv_VisualizeTB = new HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(
                        0));
                    hv_MaxIndex = ((((new HTuple(hv_ObjectModel3D.TupleLength())).TupleConcat(
                        hv_MaxNumModels))).TupleMin()) - 1;
                    //Set trackball fixed in the center of the window
                    hv_TrackballCenterRow = hv_Height / 2;
                    hv_TrackballCenterCol = hv_Width / 2;
                    if ((int)(new HTuple(hv_WindowCenteredRotation.TupleEqual(1))) != 0)
                    {
                        try
                        {
                            get_trackball_center_fixed(hv_SelectedObject.TupleSelectRange(0, hv_MaxIndex),
                                hv_TrackballCenterRow, hv_TrackballCenterCol, hv_TrackballRadiusPixel,
                                hv_Scene3D, hv_ObjectModel3D.TupleSelectRange(0, hv_MaxIndex), hv_Poses.TupleSelectRange(
                                0, ((hv_MaxIndex + 1) * 7) - 1), hv_WindowHandleBuffer, hv_CamParam_COPY_INP_TMP,
                                hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, out hv_TBCenter,
                                out hv_TBSize);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            disp_message(hv_WindowHandle, "Surface inspection mode is not available.",
                                "image", 5, 20, "red", "true");
                            hv_WindowCenteredRotation = 2;
                            get_trackball_center(hv_SelectedObject.TupleSelectRange(0, hv_MaxIndex),
                                hv_TrackballRadiusPixel, hv_ObjectModel3D.TupleSelectRange(0, hv_MaxIndex),
                                hv_Poses.TupleSelectRange(0, ((hv_MaxIndex + 1) * 7) - 1), out hv_TBCenter,
                                out hv_TBSize);
                            HOperatorSet.WaitSeconds(1);
                        }
                    }
                    else
                    {
                        get_trackball_center(hv_SelectedObject.TupleSelectRange(0, hv_MaxIndex),
                            hv_TrackballRadiusPixel, hv_ObjectModel3D.TupleSelectRange(0, hv_MaxIndex),
                            hv_Poses.TupleSelectRange(0, ((hv_MaxIndex + 1) * 7) - 1), out hv_TBCenter,
                            out hv_TBSize);
                    }
                    dump_image_output(ho_Image, hv_WindowHandleBuffer, hv_Scene3D, hv_AlphaOrig,
                        hv_ObjectModel3D, hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP,
                        hv_CamParam_COPY_INP_TMP, hv_Poses, hv_ColorImage, hv_Title, hv_Information,
                        hv_Label_COPY_INP_TMP, hv_VisualizeTB, "true", hv_TrackballCenterRow,
                        hv_TrackballCenterCol, hv_TBSize, hv_SelectedObject, hv_WindowCenteredRotation,
                        hv_TBCenter);
                    ho_ImageDump.Dispose();
                    HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                    HDevWindowStack.SetActive(hv_WindowHandle);
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                    }

                    //
                    //Check for mouse events
                    hv_GraphEvent = 0;
                    hv_Exit = 0;
                    while ((int)(1) != 0)
                    {
                        //
                        //Check graphic event
                        try
                        {
                            HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_GraphButtonRow,
                                out hv_GraphButtonColumn, out hv_GraphButton);
                            if ((int)(new HTuple(hv_GraphButton.TupleNotEqual(0))) != 0)
                            {
                                if ((int)((new HTuple((new HTuple((new HTuple(hv_GraphButtonRow.TupleGreater(
                                    (hv_Height - hv_TextHeight) - 13))).TupleAnd(new HTuple(hv_GraphButtonRow.TupleLess(
                                    hv_Height))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleGreater(
                                    (hv_Width - hv_TextWidth) - 13))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleLess(
                                    hv_Width)))) != 0)
                                {
                                    //Wait until the continue button has been released
                                    if ((int)(new HTuple(hv_WaitForButtonRelease.TupleEqual("true"))) != 0)
                                    {
                                        while ((int)(1) != 0)
                                        {
                                            HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_GraphButtonRow,
                                                out hv_GraphButtonColumn, out hv_GraphButton);
                                            if ((int)((new HTuple(hv_GraphButton.TupleEqual(0))).TupleOr(
                                                new HTuple(hv_GraphButton.TupleEqual(new HTuple())))) != 0)
                                            {
                                                if ((int)((new HTuple((new HTuple((new HTuple(hv_GraphButtonRow.TupleGreater(
                                                    (hv_Height - hv_TextHeight) - 13))).TupleAnd(new HTuple(hv_GraphButtonRow.TupleLess(
                                                    hv_Height))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleGreater(
                                                    (hv_Width - hv_TextWidth) - 13))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleLess(
                                                    hv_Width)))) != 0)
                                                {
                                                    hv_ButtonReleased = 1;
                                                }
                                                else
                                                {
                                                    hv_ButtonReleased = 0;
                                                }
                                                //
                                                break;
                                            }
                                            //Keep waiting until mouse button is released or moved out of the window
                                        }
                                    }
                                    else
                                    {
                                        hv_ButtonReleased = 1;
                                    }
                                    //Exit the visualization loop
                                    if ((int)(hv_ButtonReleased) != 0)
                                    {
                                        hv_Exit = 1;
                                        break;
                                    }
                                }
                                hv_GraphEvent = 1;
                                break;
                            }
                            else
                            {
                                hv_ButtonHold = 0;
                            }
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            //Keep waiting
                        }
                    }
                    if ((int)(hv_GraphEvent) != 0)
                    {
                        analyze_graph_event(ho_Image, hv_MouseMapping, hv_GraphButton, hv_GraphButtonRow,
                            hv_GraphButtonColumn, hv_WindowHandle, hv_WindowHandleBuffer, hv_VirtualTrackball,
                            hv_TrackballSize, hv_SelectedObject, hv_Scene3D, hv_AlphaOrig, hv_ObjectModel3D,
                            hv_CamParam_COPY_INP_TMP, hv_Label_COPY_INP_TMP, hv_Title, hv_Information,
                            hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, hv_Poses,
                            hv_ButtonHold, hv_TBCenter, hv_TBSize, hv_WindowCenteredRotation, hv_MaxNumModels,
                            out hv_Poses, out hv_SelectedObject, out hv_ButtonHold, out hv_WindowCenteredRotation);
                    }
                    if ((int)(hv_Exit) != 0)
                    {
                        break;
                    }
                }
                //
                //Display final state with persistence, if requested
                //Note that disp_object_model_3d must be used instead of the 3D scene
                if ((int)(new HTuple((new HTuple(hv_PersistenceParamName.TupleLength())).TupleGreater(
                    0))) != 0)
                {
                    try
                    {
                        HOperatorSet.DispObjectModel3d(hv_WindowHandle, hv_ObjectModel3D, hv_CamParam_COPY_INP_TMP,
                            hv_Poses, ((new HTuple("disp_background")).TupleConcat("alpha")).TupleConcat(
                            hv_PersistenceParamName), ((new HTuple("true")).TupleConcat(0.0)).TupleConcat(
                            hv_PersistenceParamValue));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        // stop(); only in hdevelop
                    }
                }
                //
                //Compute the output pose
                if ((int)(ExpGetGlobalVar_gIsSinglePose()) != 0)
                {
                    hv_PoseOut = hv_Poses.TupleSelectRange(0, 6);
                }
                else
                {
                    hv_PoseOut = hv_Poses.Clone();
                }
                //
                //Clean up
                HOperatorSet.SetSystem("clip_region", hv_ClipRegion);
                // dev_set_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop
                dump_image_output(ho_Image, hv_WindowHandleBuffer, hv_Scene3D, hv_AlphaOrig,
                    hv_ObjectModel3D, hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP,
                    hv_CamParam_COPY_INP_TMP, hv_Poses, hv_ColorImage, hv_Title, new HTuple(),
                    hv_Label_COPY_INP_TMP, 0, "false", hv_TrackballCenterRow, hv_TrackballCenterCol,
                    hv_TBSize, hv_SelectedObject, hv_WindowCenteredRotation, hv_TBCenter);
                ho_ImageDump.Dispose();
                HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                HDevWindowStack.SetActive(hv_WindowHandle);
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                }
                HOperatorSet.CloseWindow(hv_WindowHandleBuffer);
                HOperatorSet.SetPart(hv_WindowHandle, hv_WPRow1, hv_WPColumn1, hv_WPRow2, hv_WPColumn2);
                HOperatorSet.ClearScene3d(hv_Scene3D);
                ho_Image.Dispose();
                ho_ImageDump.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();
                ho_ImageDump.Dispose();
                LoggerHelper.Error("可视化3D对象异常", HDevExpDefaultException);
                //throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 计算3D对象模型的最佳观察距离
        /// </summary>
        /// <param name="hv_ObjectModel3DID"></param>
        /// <param name="hv_CamParam"></param>
        /// <param name="hv_ImageCoverage"></param>
        /// <param name="hv_PoseIn"></param>
        /// <param name="hv_PoseOut"></param>
        private void determine_optimum_pose_distance(HTuple hv_ObjectModel3DID, HTuple hv_CamParam, HTuple hv_ImageCoverage, HTuple hv_PoseIn, out HTuple hv_PoseOut)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_NumModels = null, hv_Rows = null;
            HTuple hv_Cols = null, hv_MinMinZ = null, hv_BB = null;
            HTuple hv_Seq = null, hv_DXMax = null, hv_DYMax = null;
            HTuple hv_DZMax = null, hv_Diameter = null, hv_ZAdd = null;
            HTuple hv_IBB = null, hv_BB0 = null, hv_BB1 = null, hv_BB2 = null;
            HTuple hv_BB3 = null, hv_BB4 = null, hv_BB5 = null, hv_X = null;
            HTuple hv_Y = null, hv_Z = null, hv_PoseInter = null, hv_HomMat3D = null;
            HTuple hv_CX = null, hv_CY = null, hv_CZ = null, hv_DR = null;
            HTuple hv_DC = null, hv_MaxDist = null, hv_HomMat3DRotate = new HTuple();
            HTuple hv_MinImageSize = null, hv_Zs = null, hv_ZDiff = null;
            HTuple hv_ScaleZ = null, hv_ZNew = null;
            // Initialize local and output iconic variables 
            //Determine the optimum distance of the object to obtain
            //a reasonable visualization
            //
            hv_NumModels = new HTuple(hv_ObjectModel3DID.TupleLength());
            hv_Rows = new HTuple();
            hv_Cols = new HTuple();
            hv_MinMinZ = 1e30;
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "bounding_box1", out hv_BB);
            //Calculate diameter over all objects to be visualized
            hv_Seq = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_DXMax = (((hv_BB.TupleSelect(hv_Seq + 3))).TupleMax()) - (((hv_BB.TupleSelect(
                hv_Seq))).TupleMin());
            hv_DYMax = (((hv_BB.TupleSelect(hv_Seq + 4))).TupleMax()) - (((hv_BB.TupleSelect(
                hv_Seq + 1))).TupleMin());
            hv_DZMax = (((hv_BB.TupleSelect(hv_Seq + 5))).TupleMax()) - (((hv_BB.TupleSelect(
                hv_Seq + 2))).TupleMin());
            hv_Diameter = ((((hv_DXMax * hv_DXMax) + (hv_DYMax * hv_DYMax)) + (hv_DZMax * hv_DZMax))).TupleSqrt()
                ;
            if ((int)(new HTuple(((((hv_BB.TupleAbs())).TupleSum())).TupleEqual(0.0))) != 0)
            {
                hv_BB = new HTuple();
                hv_BB = hv_BB.TupleConcat(-((new HTuple(HTuple.TupleRand(
                    3) * 1e-20)).TupleAbs()));
                hv_BB = hv_BB.TupleConcat((new HTuple(HTuple.TupleRand(
                    3) * 1e-20)).TupleAbs());
            }
            //Allow the visualization of single points or extremely small objects
            hv_ZAdd = 0.0;
            if ((int)(new HTuple(((hv_Diameter.TupleMax())).TupleLess(1e-10))) != 0)
            {
                hv_ZAdd = 0.01;
            }
            //Set extremely small diameters to 1e-10 to avoid CZ == 0.0, which would lead
            //to projection errors
            if ((int)(new HTuple(((hv_Diameter.TupleMin())).TupleLess(1e-10))) != 0)
            {
                hv_Diameter = hv_Diameter - (((((((hv_Diameter - 1e-10)).TupleSgn()) - 1)).TupleSgn()
                    ) * 1e-10);
            }
            hv_IBB = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_BB0 = hv_BB.TupleSelect(hv_IBB);
            hv_BB1 = hv_BB.TupleSelect(hv_IBB + 1);
            hv_BB2 = hv_BB.TupleSelect(hv_IBB + 2);
            hv_BB3 = hv_BB.TupleSelect(hv_IBB + 3);
            hv_BB4 = hv_BB.TupleSelect(hv_IBB + 4);
            hv_BB5 = hv_BB.TupleSelect(hv_IBB + 5);
            hv_X = new HTuple();
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_Y = new HTuple();
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Z = new HTuple();
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_PoseInter = hv_PoseIn.TupleReplace(2, (-(hv_Z.TupleMin())) + (2 * (hv_Diameter.TupleMax()
                )));
            HOperatorSet.PoseToHomMat3d(hv_PoseInter, out hv_HomMat3D);
            //Determine the maximum extention of the projection
            HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_X, hv_Y, hv_Z, out hv_CX, out hv_CY,
                out hv_CZ);
            HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, hv_CamParam, out hv_Rows, out hv_Cols);
            hv_MinMinZ = hv_CZ.TupleMin();
            hv_DR = hv_Rows - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 3));
            hv_DC = hv_Cols - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 4));
            hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
            hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
            hv_MaxDist = (((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt();
            //
            if ((int)(new HTuple(hv_MaxDist.TupleLess(1e-10))) != 0)
            {
                //If the object has no extension in the above projection (looking along
                //a line), we determine the extension of the object in a rotated view
                HOperatorSet.HomMat3dRotateLocal(hv_HomMat3D, (new HTuple(90)).TupleRad(),
                    "x", out hv_HomMat3DRotate);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3DRotate, hv_X, hv_Y, hv_Z, out hv_CX,
                    out hv_CY, out hv_CZ);
                HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, hv_CamParam, out hv_Rows,
                    out hv_Cols);
                hv_DR = hv_Rows - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                    )) - 3));
                hv_DC = hv_Cols - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                    )) - 4));
                hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
                hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
                hv_MaxDist = ((hv_MaxDist.TupleConcat((((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt()
                    ))).TupleMax();
            }
            //
            hv_MinImageSize = ((((hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 2))).TupleConcat(hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 1)))).TupleMin();
            //
            hv_Z = hv_PoseInter[2];
            hv_Zs = hv_MinMinZ.Clone();
            hv_ZDiff = hv_Z - hv_Zs;
            hv_ScaleZ = hv_MaxDist / (((0.5 * hv_MinImageSize) * hv_ImageCoverage) * 2.0);
            hv_ZNew = ((hv_ScaleZ * hv_Zs) + hv_ZDiff) + hv_ZAdd;
            hv_PoseOut = hv_PoseInter.TupleReplace(2, hv_ZNew);
            //
            return;
        }

        #endregion





    }

}
