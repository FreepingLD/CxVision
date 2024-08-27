using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FunctionBlock;
using Sensor;

namespace FunctionBlock
{
    public partial class ToolForm : Form
    {
        //private AddAcqSourceForm form;
        private TreeViewWrapClass _treeViewTarget;
        public TreeViewWrapClass TreeViewTarget { get => _treeViewTarget; set => _treeViewTarget = value; }

        public ToolForm()
        {
            InitializeComponent();
        }
        public ToolForm(TreeViewWrapClass treeViewTarget)
        {
            InitializeComponent();
            this._treeViewTarget = treeViewTarget;
            this._treeViewTarget.ToolName = "";
        }

        public ToolForm(TreeViewWrapClass treeViewTarget, string toolName = "")
        {
            InitializeComponent();
            this._treeViewTarget = treeViewTarget;
            this._treeViewTarget.ToolName = toolName;
        }

        public void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string name = e.Node.Text;
            if (this._treeViewTarget == null)
            {
                this._treeViewTarget = ProgramForm.Instance.ProgramDic[this.treeView1.Parent.Text];
                //this._treeViewTarget = ((ProgramForm)Application.OpenForms?["ProgramForm"]).TreeViewWrapClass;
            }
            string nam22e = this._treeViewTarget.TreeView.Parent.Text;
            //ProgramForm.Instance.ProgramD[this.treeView1.Parent.Text]

            switch (name.Trim())
            {
                case "相机采集源":
                    if (this._treeViewTarget != null)
                    {
                        this._treeViewTarget.AddItems(new AcqSource(SensorManage.CurrentCamSensor), "相机采集源");
                    }
                    break;
                case "激光采集源":
                    if (this._treeViewTarget != null)
                    {
                        this._treeViewTarget.AddItems(new AcqSource(SensorManage.CurrentLaserSensor), "激光采集源");
                    }
                    break;
                case "激光点":
                    if (this._treeViewTarget != null)
                    {
                        LaserPointAcq laserPointAcq;
                        if (AcqSourceManage.Instance.LaserAcqSourceList().Count > 0 && AcqSourceManage.Instance.GetCamAcqSourceList().Count > 0)
                            laserPointAcq = new LaserPointAcq(AcqSourceManage.Instance.LaserAcqSourceList().Last(), AcqSourceManage.Instance.GetCamAcqSourceList().Last());
                        else
                            laserPointAcq = new LaserPointAcq(new AcqSource("激光采集源"), new AcqSource("相机采集源"));
                        ///////////////////////////
                        this._treeViewTarget.AddItems(laserPointAcq, laserPointAcq.Name, nameof(laserPointAcq.LaserAcqSource), nameof(laserPointAcq.CamAcqSource), nameof(laserPointAcq.WcsCoordSystem),
                            nameof(laserPointAcq.Laser1Dist1DataHandle), nameof(laserPointAcq.Laser1Dist2DataHandle), nameof(laserPointAcq.Laser1ThickDataHandle));
                    }
                    break;
                case "激光线":
                    if (this._treeViewTarget != null)
                    {
                        LaserLineScanAcq laserLineAcq;
                        if (AcqSourceManage.Instance.LaserAcqSourceList().Count > 0 && AcqSourceManage.Instance.GetCamAcqSourceList().Count > 0)
                            laserLineAcq = new LaserLineScanAcq(AcqSourceManage.Instance.LaserAcqSourceList().Last(), AcqSourceManage.Instance.GetCamAcqSourceList().Last());
                        else
                            laserLineAcq = new LaserLineScanAcq(new AcqSource("激光采集源"), new AcqSource("相机采集源"));
                        ///////////////////////////
                        this._treeViewTarget.AddItems(laserLineAcq, laserLineAcq.Name, nameof(laserLineAcq.LaserAcqSource), nameof(laserLineAcq.CamAcqSource), nameof(laserLineAcq.WcsCoordSystem),
                            nameof(laserLineAcq.Laser1Dist1DataHandle), nameof(laserLineAcq.Laser1Dist2DataHandle), nameof(laserLineAcq.Laser1ThickDataHandle));
                    }
                    break;
                case "激光点2":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LaserPointAcqStandard(SensorManage.CurrentLaserSensor, GetCurrentCoordSystem()), "激光点2", "RefSource2-<=参考坐标系", "距离1-=>距离1", "距离2-=>距离2", "厚度-=>厚度");
                    break;
                case "激光线2":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LaserScanAcqStandard(SensorManage.CurrentLaserSensor, GetCurrentCoordSystem()), "激光线2", "RefSource2-<=参考坐标系", "距离1-=>距离1", "距离2-=>距离2", "厚度-=>厚度");
                    break;

                case "点云采集":  // 
                    if (this._treeViewTarget != null)
                    {
                        PointCloudAcq pointCloud = new PointCloudAcq();
                        this._treeViewTarget.AddItems(pointCloud, "点云采集", nameof(pointCloud.TrackPoint), nameof(pointCloud.WcsCoordSystem), nameof(pointCloud.Dist1DataHandle), nameof(pointCloud.Dist2DataHandle), nameof(pointCloud.ThickDataHandle));
                    }
                    break;

                case "图像采集":
                    if (this._treeViewTarget != null)
                    {
                        ImageAcq imag;
                        if (AcqSourceManage.Instance.GetCamAcqSourceList().Count > 0)
                            imag = new ImageAcq(AcqSourceManage.Instance.GetCamAcqSourceList()[0].Name);
                        else
                            imag = new ImageAcq(null);
                        this._treeViewTarget.AddItems(imag, "图像采集", nameof(imag.ImageData), nameof(imag.DarkImageData)); //选择3D对象范围
                    }
                    break;
                case "开始采集":
                    if (this._treeViewTarget != null)
                    {
                        StartAcq startAcq = new StartAcq();
                        this._treeViewTarget.AddItems(startAcq, "开始采集"); // , "图像对象-=>图像对象"  , nameof(imag.AcqSource)
                    }
                    break;
                case "停止采集":
                    if (this._treeViewTarget != null)
                    {
                        StopAcq stotAcq = new StopAcq();
                        this._treeViewTarget.AddItems(stotAcq, "停止采集"); // , "图像对象-=>图像对象"  , nameof(imag.AcqSource)
                    }
                    break;
                case "图像采集2":
                    if (this._treeViewTarget != null)
                    {
                        //ImagesAcq2 imagesAcq2 = new ImagesAcq2();
                        //this._treeViewTarget.AddItems(imagesAcq2, "图像采集2", nameof(imagesAcq2.AcqSourceName), nameof(imagesAcq2.AcqSourceName)); // , "图像对象-=>图像对象"
                    }
                    break;
                case "矩形扫描":
                    switch (SensorManage.LaserList.Count)
                    {
                        case 0:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new RectangleScanAcq(null, null, GetCurrentCoordSystem()), "矩形扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");   // 减少3D对象定义域  双激光扫描线
                            break;
                        case 1:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new RectangleScanAcq(SensorManage.LaserList[0].Name, null, GetCurrentCoordSystem()), "矩形扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                        default:
                        case 2:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new RectangleScanAcq(SensorManage.LaserList[0].Name, SensorManage.LaserList[1].Name, GetCurrentCoordSystem()), "矩形扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                    }
                    break;

                case "圆弧扫描":
                    switch (SensorManage.LaserList.Count)
                    {
                        case 0:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new SpiralScanAcq(null, null, GetCurrentCoordSystem()), "圆弧扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");   // 减少3D对象定义域  双激光扫描线

                            break;
                        case 1:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new SpiralScanAcq(SensorManage.LaserList[0].Name, null, GetCurrentCoordSystem()), "圆弧扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                        default:
                        case 2:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new SpiralScanAcq(SensorManage.LaserList[0].Name, SensorManage.LaserList[1].Name, GetCurrentCoordSystem()), "圆弧扫描", "RefSource2-<=参考坐标系", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                    }
                    break;
                case "直线扫描":
                    switch (SensorManage.LaserList.Count)
                    {
                        case 0:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new LaserScanAcqPath(null), "直线扫描", "RefSource1-<=扫描路径", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");   // 减少3D对象定义域  双激光扫描线
                            break;
                        default:
                        case 1:
                            if (this._treeViewTarget != null)
                                this._treeViewTarget.AddItems(new LaserScanAcqPath(SensorManage.LaserList[0]), "直线扫描", "RefSource1-<=扫描路径", "激光1距离1-=>激光1距离1", "激光1距离2-=>激光1距离2", "激光1厚度-=>激光1厚度", "激光2距离1-=>激光2距离1", "激光2距离2-=>激光2距离2", "激光2厚度-=>激光2厚度");
                            break;
                    }
                    break;

                case "对射校准":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new TroughCalibrate(), "对射校准");
                    break;

                case "对射测厚":
                    if (this._treeViewTarget != null)
                    {
                        ThicknessMeasure measure = new ThicknessMeasure();
                        this._treeViewTarget.AddItems(measure, "对射测厚"); //, "3D坐标系-=>3D坐标系"
                    }
                    break;

                case "创建位姿3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CreatePose3D(), "创建位姿3D"); //, "3D坐标系-=>3D坐标系"
                    break;

                case "翻转对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new OverturnObjectModel3D(), "翻转对象3D", "RefSource1-<=输入对象3D"); //, "3D对象-=>3D对象"
                    break;

                case "读取对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ReadObjectModel3D(), "读取对象3D"); //, "3D对象-=>3D对象"
                    break;
                case "保存对象3D":
                    if (this._treeViewTarget != null)
                    {
                        SaveObjectModel3D saveObjectModel3D = new SaveObjectModel3D();
                        this._treeViewTarget.AddItems(saveObjectModel3D, "保存对象3D", nameof(saveObjectModel3D.DataHandle3D));
                    }
                    break;
                case "平面度":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Planeness(), "平面度", "RefSource1-<=输入对象3D"); //, "平面度-=>平面度"
                    break;
                case "体积":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Volume(), "体积", "RefSource1-<=输入对象3D"); //, "体积-=>体积"
                    break;
                case "厚度":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Thickness(), "厚度", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2"); //, "厚度-=>厚度", "激光1距离-=>激光1距离", "激光2距离-=>激光2距离", "坐标位置-=>坐标位置"
                    break;
                case "提取截面3D":
                    if (this._treeViewTarget != null)
                    {
                        Section section = new Section();
                        this._treeViewTarget.AddItems(section, "提取截面3D", nameof(section.DataHandle3D), nameof(section.OutObjectModel3D)); // , "轮廓3D对象-=>轮廓3D对象"
                    }

                    break;
                case "点到面距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToFaceDist(), "点到面距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2"); //, "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值"
                    break;
                case "点到线距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToLineDist3D(), "点到线距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2"); //, "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值"
                    break;
                case "点到点距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToPointDist3D(), "点到点距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2"); //, "两点距离-=>两点距离", "距离X-=>距离X", "距离Y-=>距离Y", "距离Z-=>距离Z"
                    break;
                case "线到线距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineToLineDist3D(), "线到线距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2"); //, "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值"
                    break;

                case "面到面距离3D":
                    if (this._treeViewTarget != null)
                    {
                        FaceToFaceDist3D faceDist3D = new FaceToFaceDist3D();
                        this._treeViewTarget.AddItems(new FaceToFaceDist3D(), "面到面距离3D", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2"); //, "平均值-=>平均值"
                    }
                    break;

                case "采样对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SampleObjectModel3D(), "采样对象3D", "RefSource1-<=输入对象1"); //, "3D对象-=>3D对象"
                    break;

                case "选择对象3D":
                    if (this._treeViewTarget != null)
                    {
                        SelectObjectModelRange3D selectObjectModel = new SelectObjectModelRange3D();
                        this._treeViewTarget.AddItems(selectObjectModel, "选择对象3D", nameof(selectObjectModel.DataObjectModel), nameof(selectObjectModel.SelectObjectModel)); //选择3D对象范围
                    }
                    break;
                case "滤波对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FilterObjectModel3D(), "滤波3D对象", "RefSource1-<=输入对象"); //, "3D对象-=>3D对象"
                    break;
                case "分割对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SegmentObjectModle3D(), "分割3D对象", "RefSource1-<=输入对象"); //, "3D对象-=>3D对象"
                    break;
                case "合并对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Union3DobjectModel(), "合并3D对象", "RefSource1-<=输入对象"); //, "3D对象-=>3D对象"
                    break;
                case "平滑对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SmoothObjectModel3D(), "平滑3D对象", "RefSource1-<=输入对象"); //, "3D对象-=>3D对象"
                    break;
                case "平滑3D轮廓对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SmoothProfileModel3D(), "平滑3D轮廓对象", "RefSource1-<=输入对象"); //, "3D对象-=>3D对象"
                    break;

                case "坐标系-表面匹配":
                case "表面匹配":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new DoSurfaceModelMatch(), "表面匹配", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2", "坐标系-=>坐标系", "场景采样点对象-=>场景采样点对象", "关键点3D对象-=>关键点3D对象", "变形采样点3D对象-=>变形采样点3D对象"); //选择3D对象范围
                    break;


                case "变换对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AffineObjectModel3D(), "变换对象3D", "RefSource1-<=输入对象", "RefSource2-<=坐标系"); //, "3D对象-=>3D对象"
                    break;

                case "缩放对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ScaleObjectModel3D(), "缩放对象", "RefSource1-<=输入对象"); //, "3D对象-=>3D对象"
                    break;
                case "转换对象到图像3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new TransformObject3DToXYZImage(), "转换对象到图像3D", "RefSource1-<=输入对象", "实值图像X-=>实值图像X", "实值图像Y-=>实值图像Y", "实值图像Z-=>实值图像Z", "字节图像X-=>字节图像X", "字节图像Y-=>字节图像Y", "字节图像Z-=>字节图像Z"); //选择3D对象范围
                    break;

                case "转换图像到对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new TransformXYZImageToObject3D(), "转换图像到对象3D", "RefSource1-<=输入图像X", "RefSource2-<=输入图像Y", "RefSource3-<=输入图像Z"); // "3D位姿" , "3D对象-=>3D对象"
                    break;

                case "角平分线":
                    if (this._treeViewTarget != null)
                    {
                        CalculateAngleBisector angleBisector = new CalculateAngleBisector();
                        this._treeViewTarget.AddItems(angleBisector, "角平分线", nameof(angleBisector.WcsLine1), nameof(angleBisector.WcsLine2), nameof(angleBisector.WcsLine)); //, "直线对象-=>直线对象"
                    }
                    break;


                case "中分线":
                    if (this._treeViewTarget != null)
                    {
                        CalculateMiddleBisector middleBisector = new CalculateMiddleBisector();
                        this._treeViewTarget.AddItems(middleBisector, "中分线", nameof(middleBisector.WcsRect2), nameof(middleBisector.WcsLine));
                    }
                    break;

                case "线线距离":
                    if (this._treeViewTarget != null)
                    {
                        LineToLineDist2D doLineToLineDist2D = new LineToLineDist2D();
                        this._treeViewTarget.AddItems(doLineToLineDist2D, "线线距离", nameof(doLineToLineDist2D.WcsLine1), nameof(doLineToLineDist2D.WcsLine2), nameof(doLineToLineDist2D.MeanDist),
                            nameof(doLineToLineDist2D.MaxDist), nameof(doLineToLineDist2D.MinDist)
                          ); //, "距离-=>距离"   nameof(doLineToLineDist2D.ResultInfo), nameof(doLineToLineDist2D.Result)
                    }
                    break;

                case "点点距离":
                    if (this._treeViewTarget != null)
                    {
                        PointToPointDist2D pointToPointDist2D = new PointToPointDist2D();
                        this._treeViewTarget.AddItems(pointToPointDist2D, "点点距离", nameof(pointToPointDist2D.WcsPoint1), nameof(pointToPointDist2D.WcsPoint2),
                            nameof(pointToPointDist2D.MaxDist), nameof(pointToPointDist2D.VerticalDist), nameof(pointToPointDist2D.LevelDist)); //, nameof(pointToPointDist2D.ResultInfo), nameof(pointToPointDist2D.Result)
                    }
                    break;

                case "点线距离":
                    if (this._treeViewTarget != null)
                    {
                        PointToLineDist2D doPointToLineDist2D = new PointToLineDist2D();
                        this._treeViewTarget.AddItems(doPointToLineDist2D, "点线距离", nameof(doPointToLineDist2D.WcsPoint), nameof(doPointToLineDist2D.WcsLine),
                             nameof(doPointToLineDist2D.Dist));
                    }
                    break;

                case "圆线距离":
                    if (this._treeViewTarget != null)
                    {
                        CircleToLineDist2D circleToLineDist2D = new CircleToLineDist2D();
                        this._treeViewTarget.AddItems(circleToLineDist2D, "圆线距离", nameof(circleToLineDist2D.WcsCircle), nameof(circleToLineDist2D.WcsLine),
                            nameof(circleToLineDist2D.CircleDist), nameof(circleToLineDist2D.MaxDist), nameof(circleToLineDist2D.MinDist));
                    }
                    break;

                case "圆圆距离":
                    if (this._treeViewTarget != null)
                    {
                        CircleToCircleDist2D circleToCircleDist2D = new CircleToCircleDist2D();
                        this._treeViewTarget.AddItems(circleToCircleDist2D, "圆圆距离", nameof(circleToCircleDist2D.WcsCircle1), nameof(circleToCircleDist2D.WcsCircle2),
                            nameof(circleToCircleDist2D.CircleDist), nameof(circleToCircleDist2D.MaxDist), nameof(circleToCircleDist2D.MinDist));
                    }
                    break;

                case "直线中点":
                    if (this._treeViewTarget != null)
                    {
                        LineMiddlePoint lineMiddlePoint = new LineMiddlePoint();
                        this._treeViewTarget.AddItems(lineMiddlePoint, "直线中点", nameof(lineMiddlePoint.WcsLine), nameof(lineMiddlePoint.WcsVector));
                    }
                    break;

                case "N点到直线":
                    if (this._treeViewTarget != null)
                    {
                        NPointToLineDist2D pointToLineDist2D = new NPointToLineDist2D();
                        this._treeViewTarget.AddItems(pointToLineDist2D, "N点到直线", nameof(pointToLineDist2D.WcsPoint), nameof(pointToLineDist2D.WcsLine), nameof(pointToLineDist2D.MinWcsPoint), nameof(pointToLineDist2D.MaxWcsPoint));
                    }
                    break;
                case "轮廓到线距离":
                    if (this._treeViewTarget != null)
                    {
                        ContToLineDist2D contToLineDist2D = new ContToLineDist2D();
                        this._treeViewTarget.AddItems(contToLineDist2D, "轮廓到线距离", nameof(contToLineDist2D.WcsPoint), nameof(contToLineDist2D.WcsLine), nameof(contToLineDist2D.MinWcsPoint), nameof(contToLineDist2D.MaxWcsPoint));
                    }
                    break;

                case "投影点":
                    if (this._treeViewTarget != null)
                    {
                        ProjectionPoint projection = new ProjectionPoint();
                        this._treeViewTarget.AddItems(projection, "投影点", nameof(projection.WcsPoint1), nameof(projection.WcsLine2), nameof(projection.WcsPoint));
                    }
                    break;

                case "Blob分析":
                    if (this._treeViewTarget != null)
                    {
                        Blob doBlob = new Blob();
                        this._treeViewTarget.AddItems(doBlob, "Blob分析", nameof(doBlob.ImageData), nameof(doBlob.PixCoordSystem), nameof(doBlob.BlobRegion),
                                                      nameof(doBlob.BinaryMeanImage), nameof(doBlob.BinaryImage), nameof(doBlob.RegionImage)); // 
                    }
                    break;

                case "读取图像":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ReadImage(), "读取图像"); //, "图像-=>图像"
                    break;

                case "提取轮廓":
                    if (this._treeViewTarget != null)
                    {
                        ExtractXLD extract = new ExtractXLD();
                        this._treeViewTarget.AddItems(extract, "提取轮廓", "RefSource1-<=输入图像"); //, "XLD轮廓-=>XLD轮廓"
                    }
                    break;

                case "选择轮廓":
                    if (this._treeViewTarget != null)
                    {
                        SelectionXLD selection = new SelectionXLD();
                        this._treeViewTarget.AddItems(selection, "选择轮廓", "RefSource1-<=输入XLD轮廓"); //, "XLD轮廓-=>XLD轮廓"
                    }
                    break;

                case "数据输出":
                    if (this._treeViewTarget != null)
                    {
                        OutputData outputData = new OutputData();
                        this._treeViewTarget.AddItems(outputData, "数据输出", nameof(outputData.OutPutContent));
                    }
                    break;

                case "取反平移对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new InvertTranslateObjectModel3D(), "取反平移对象3D", "RefSource1-<=输入对象");
                    break;

                //case "矩形取点3D":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new Rectangle2Crop(), "矩形取点", "RefSource1-<=输入对象", "RefSource2-<=坐标系"); 
                //    break;

                case "圆形取点3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CircleCrop(), "圆形取点", "RefSource1-<=输入对象", "RefSource2-<=坐标系");
                    break;


                case "裁剪对象3D":
                    if (this._treeViewTarget != null)
                    {
                        CropObjectModel3D cropObject = new CropObjectModel3D();
                        this._treeViewTarget.AddItems(cropObject, "裁剪对象3D", nameof(cropObject.DataHandle3D), nameof(cropObject.WcsCoordSystem), nameof(cropObject.CropObjectModel));
                    }
                    break;

                case "创建基本体3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CreatePrimitive3D(), "创建基本体3D", "RefSource1-<=输入位姿"); //, "3D对象-=>3D对象"
                    break;

                case "变换轮廓XLD":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AffineXLDCont2D(), "变换轮廓XLD", "RefSource1-<=输入轮廓XLD", "RefSource2-<=坐标系"); //, "XLD轮廓-=>XLD轮廓"
                    break;


                case "保存图像":
                    if (this._treeViewTarget != null)
                    {
                        SaveImage saveImage = new SaveImage();
                        this._treeViewTarget.AddItems(saveImage, "保存图像", nameof(saveImage.ImageData), nameof(saveImage.InputRegion), nameof(saveImage.SavePath)); //, "图像-=>图像"
                    }
                    break;

                case "保存轮廓":
                    if (this._treeViewTarget != null)
                    {
                        SaveContXLD saveXLDCont = new SaveContXLD();
                        this._treeViewTarget.AddItems(saveXLDCont, "保存轮廓", nameof(saveXLDCont.XldContData)); //, "XLD轮廓-=>XLD轮廓"
                    }
                    break;

                case "读取轮廓":
                    if (this._treeViewTarget != null)
                    {
                        ReadContourXLD readXLD = new ReadContourXLD();
                        this._treeViewTarget.AddItems(readXLD, "读取轮廓", nameof(readXLD.XldContour)); //, "XLD轮廓-=>XLD轮廓"
                    }
                    break;

                case "采样轮廓":
                    if (this._treeViewTarget != null)
                    {
                        SampleXLD sampleXLD = new SampleXLD();
                        this._treeViewTarget.AddItems(sampleXLD, "采样轮廓", "RefSource1-<=输入轮廓XLD", "像素采样点-=>像素采样点", "世界采样点-=>世界采样点");
                    }

                    break;

                case "计算对象位姿3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new GetPrimitive3DPose(), "计算对象位姿3D", "RefSource1-<=输入对象"); //, "3D位姿-=>3D位姿"
                    break;

                case "校平对象3D":
                    if (this._treeViewTarget != null)
                    {
                        RectifyObjectModel3D rectifyObject = new RectifyObjectModel3D();
                        this._treeViewTarget.AddItems(new RectifyObjectModel3D(), "校平对象3D", nameof(rectifyObject.DataHandle3D), nameof(rectifyObject.WcsCoordSystem), nameof(rectifyObject.RectifyHandle3D),
                                                      nameof(rectifyObject.Pose)); //, "3D对象-=>3D对象"
                    }
                    break;

                case "平面相交对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new IntersectPlaneObjectModel3D(), "平面相交对象3D", "RefSource1-<=平面对象", "RefSource2-<=输入对象"); //, "3D对象-=>3D对象"
                    break;

                case "变换位姿3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AffinePose3D(), "变换位姿3D", "RefSource1-<=输入坐标系"); //, "3D坐标系-=>3D坐标系"
                    break;

                //case "创建圆形区域":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new CreateCircleRegion(), "创建圆形区域", "RefSource1-<=坐标系"); //, "圆区域-=>圆区域"
                //    break;

                //case "创建矩形区域":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new CreateRectangle2Region(), "创建矩形区域", "RefSource1-<=坐标系");   // , "矩形区域-=>矩形区域"
                //    break;

                case "获取极值轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new GetMinMaxValueObjectModel(), "获取极值轮廓", "RefSource1-<=输入3D对象", "RefSource2-<=参考直线", "3D对象-=>3D对象", "直线对象-=>直线对象");   // 减少3D对象定义域
                    break;

                case "线线交点":
                    if (this._treeViewTarget != null)
                    {
                        IntersectionLineLine intersectionLineLine = new IntersectionLineLine();
                        this._treeViewTarget.AddItems(intersectionLineLine, "线线交点", nameof(intersectionLineLine.WcsLine1), nameof(intersectionLineLine.WcsLine2), nameof(intersectionLineLine.WcsPoints)
                            , nameof(intersectionLineLine.Wcs_X), nameof(intersectionLineLine.Wcs_Y), nameof(intersectionLineLine.Cam_X), nameof(intersectionLineLine.Cam_Y)); //, nameof(intersectionLineLine.ResultInfo), nameof(intersectionLineLine.Result)
                    }
                    break;

                case "线圆交点":
                    if (this._treeViewTarget != null)
                    {
                        IntersectionLineCirlce intersectionLineCirlce = new IntersectionLineCirlce();
                        this._treeViewTarget.AddItems(intersectionLineCirlce, "线圆交点", nameof(intersectionLineCirlce.WcsLine), nameof(intersectionLineCirlce.WcsCircle), nameof(intersectionLineCirlce.WcsPoints));
                    }
                    break;

                case "圆圆交点":
                    if (this._treeViewTarget != null)
                    {
                        IntersectionCircleCircle intersectionCircleCircle = new IntersectionCircleCircle();
                        this._treeViewTarget.AddItems(intersectionCircleCircle, "线圆交点", nameof(intersectionCircleCircle.WcsCircle1), nameof(intersectionCircleCircle.WcsCircle2), nameof(intersectionCircleCircle.WcsPoints));
                    }
                    break;

                case "线线夹角":
                    if (this._treeViewTarget != null)
                    {
                        AngleLineLine2D angleLineLine2D = new AngleLineLine2D();
                        this._treeViewTarget.AddItems(angleLineLine2D, "线线夹角", nameof(angleLineLine2D.WcsLine1), nameof(angleLineLine2D.WcsLine2), nameof(angleLineLine2D.Deg));
                    }
                    break;

                case "显示多个对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ShowMultipleObjects(), "显示多个对象3D", "RefSource1-<=输入3D对象");   // , "3D对象-=>3D对象"
                    break;

                case "直线夹角3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AngleLineLine3D(), "直线夹角3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2");   // , "角度-=>角度"
                    break;

                case "直线方向3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineOrientation3D(), "直线方向3D", "RefSource1-<=输入3D对象", "角度X-=>角度X", "角度Y-=>角度Y", "角度Z-=>角度Z");   // 减少3D对象定义域  
                    break;

                case "直线长度3D": // 这个算子已经没意义了
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineLength3D(), "直线长度3D", "RefSource1-<=输入3D对象");   // , "长度3D-=>长度3D"
                    break;

                case "定点采样对象3D": // 这个算子已经没意义了
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FixedPointSampleModel(), "定点采样对象3D", "RefSource1-<=输入对象", "RefSource2-<=输入采样点");   // , "采样3D对象-=>采样3D对象" 
                    break;


                case "渲染对象到图像3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new RenderObjectModel3DToImage(), "渲染对象到图像3D", "RefSource1-<=输入对象", "图像1-=>图像1", "图像2-=>图像2", "图像3-=>图像3");   // 减少3D对象定义域   渲染3D对象到图像 转换像素点到世界点
                    break;


                case "坐标系-点线":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemPointLine coordSystemPointLine = new CoordSystemPointLine();
                        this._treeViewTarget.AddItems(coordSystemPointLine, coordSystemPointLine.Name, nameof(coordSystemPointLine.PixPoint), nameof(coordSystemPointLine.PixLine), nameof(coordSystemPointLine.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                    }
                    break;
                case "坐标系-线":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemLine coordSystemLine = new CoordSystemLine();
                        this._treeViewTarget.AddItems(coordSystemLine, coordSystemLine.Name, nameof(coordSystemLine.PixLine), nameof(coordSystemLine.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                    }
                    break;
                case "坐标系-线线":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemLineLine coordSystemLineLine = new CoordSystemLineLine();
                        this._treeViewTarget.AddItems(coordSystemLineLine, coordSystemLineLine.Name, nameof(coordSystemLineLine.PixLine1), nameof(coordSystemLineLine.PixLine2), nameof(coordSystemLineLine.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                    }
                    break;
                case "坐标系-圆线":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemCircleLine coordSystemCircleLine = new CoordSystemCircleLine();
                        this._treeViewTarget.AddItems(coordSystemCircleLine, coordSystemCircleLine.Name, nameof(coordSystemCircleLine.PixCircle), nameof(coordSystemCircleLine.PixLine), nameof(coordSystemCircleLine.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                    }
                    break;
                case "坐标系-矩形":
                    if (this._treeViewTarget != null)
                    {
                        CoordSystemRect2 coordSystemRect2 = new CoordSystemRect2();
                        this._treeViewTarget.AddItems(coordSystemRect2, coordSystemRect2.Name, nameof(coordSystemRect2.PixRect2), nameof(coordSystemRect2.WcsCoordSystem));   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系  坐标系(点-线)
                    }
                    break;

                case "拟合平面3D":
                    if (this._treeViewTarget != null)
                    {
                        FittingPlane3D fittingPlane = new FittingPlane3D();
                        this._treeViewTarget.AddItems(fittingPlane, "拟合平面3D", nameof(fittingPlane.DataObjectModel), nameof(fittingPlane.WcsPlane), nameof(fittingPlane.WcsPose));
                    }
                    break;
                case "拟合圆柱3D":
                    if (this._treeViewTarget != null)
                    {
                        FittingCylinder3D fittingCylinder = new FittingCylinder3D();
                        this._treeViewTarget.AddItems(fittingCylinder, "拟合圆柱3D", nameof(fittingCylinder.DataObjectModel), nameof(fittingCylinder.WcsCylinder), nameof(fittingCylinder.WcsPose));
                    }
                    break;
                case "拟合球体3D":
                    if (this._treeViewTarget != null)
                    {
                        FittingSphere3D fittingSphere = new FittingSphere3D();
                        this._treeViewTarget.AddItems(fittingSphere, "拟合球体3D", nameof(fittingSphere.DataObjectModel), nameof(fittingSphere.WcsSphere), nameof(fittingSphere.WcsPose));
                    }
                    break;
                case "拟合盒子3D":
                    if (this._treeViewTarget != null)
                    {
                        FittingBox3D fittingBox = new FittingBox3D();
                        this._treeViewTarget.AddItems(fittingBox, "拟合盒子3D", nameof(fittingBox.DataObjectModel), nameof(fittingBox.WcsBox), nameof(fittingBox.WcsPose));
                    }
                    break; //Y

                case "拟合轮廓圆3D":
                    if (this._treeViewTarget != null)
                    {
                        FitProfileCircle3D fitProfileCircle = new FitProfileCircle3D();
                        this._treeViewTarget.AddItems(fitProfileCircle, "拟合轮廓圆3D", nameof(fitProfileCircle.DataObjectModel), nameof(fitProfileCircle.WcsCircle));
                    }
                    break; //拟合3D轮廓圆

                case "面面夹角3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FaceToFaceAngle(), "面面夹角3D", "RefSource1-<=输入平面1", "RefSource2-<=输入平面2");   //, "角度-=>角度"
                    break;

                case "直线度":
                case "直线度3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Straightness3D(), "直线度3D", "RefSource1-<=输入3D对象");   // , "直线度-=>直线度"
                    break;

                case "线轮廓度3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ProfileTolerance(), "线轮廓度3D", "RefSource1-<=测量3D对象", "RefSource2-<=参考3D对象");   // , "轮廓度-=>轮廓度"
                    break;

                case "极值点3D":
                    this._treeViewTarget.AddItems(new ExtremePoint3D(), "极值点3D", "RefSource1-<=输入对象3D", "3D对象-=>3D对象");   // 
                    break;


                #region 2D 拟合

                case "拟合直线":
                    if (this._treeViewTarget != null)
                    {
                        FitLine fitLine = new FitLine();
                        this._treeViewTarget.AddItems(fitLine, "拟合直线", nameof(fitLine.WcsLine), nameof(fitLine.Line));
                    }
                    break;

                case "拟合圆":
                    if (this._treeViewTarget != null)
                    {
                        FitCircle fitCircle = new FitCircle();
                        this._treeViewTarget.AddItems(fitCircle, "拟合圆", nameof(fitCircle.CircleSector), nameof(fitCircle.Circle));
                    }
                    break; // N点拟合圆

                case "拟合椭圆":
                    if (this._treeViewTarget != null)
                    {
                        FitEllipse fitEllipse = new FitEllipse();
                        this._treeViewTarget.AddItems(fitEllipse, "拟合椭圆", nameof(fitEllipse.WcsEllipseSector), nameof(fitEllipse.Ellipse));
                    }
                    break; // N点拟合圆

                case "拟合矩形":
                    if (this._treeViewTarget != null)
                    {
                        FitRect2 fitRect2 = new FitRect2();
                        this._treeViewTarget.AddItems(fitRect2, "拟合矩形", nameof(fitRect2.WcsLine), nameof(fitRect2.Rect2));
                    }
                    break; // N点拟合圆


                case "N点拟合圆":
                    if (this._treeViewTarget != null)
                    {
                        NPointsFitCircle nPointsFitCircle = new NPointsFitCircle();
                        this._treeViewTarget.AddItems(nPointsFitCircle, "N点拟合圆", nameof(nPointsFitCircle.WcsPoint), nameof(nPointsFitCircle.Circle));
                    }
                    break; // 

                case "N点拟合椭圆":
                    if (this._treeViewTarget != null)
                    {
                        NPointsFitEllipse nPointsFitEllipse = new NPointsFitEllipse();
                        this._treeViewTarget.AddItems(nPointsFitEllipse, "N点拟合椭圆", nameof(nPointsFitEllipse.WcsPoint), nameof(nPointsFitEllipse.Ellipse));
                    }
                    break;

                case "N点拟合直线":
                    if (this._treeViewTarget != null)
                    {
                        NPointsFitLine nPointsFitLine = new NPointsFitLine();
                        this._treeViewTarget.AddItems(nPointsFitLine, "N点拟合直线", nameof(nPointsFitLine.WcsPoint), nameof(nPointsFitLine.Line));
                    }
                    break;
                #endregion

                case "点": //
                    PointMeasure pointMeasure = new PointMeasure();
                    this._treeViewTarget.AddItems(pointMeasure, "点", nameof(pointMeasure.ImageData), nameof(pointMeasure.PixCoordSystem), nameof(pointMeasure.WcsPoint)); //, "点坐标-=>点坐标", nameof(pointMeasure.Result)
                    break;

                case "十字取点": //
                    CrossPointMeasure pointCrossMeasure = new CrossPointMeasure();
                    this._treeViewTarget.AddItems(pointCrossMeasure, "十字取点", nameof(pointCrossMeasure.ImageData), nameof(pointCrossMeasure.PixCoordSystem), nameof(pointCrossMeasure.WcsPoint)); //, "点坐标-=>点坐标", nameof(pointMeasure.Result)
                    break;

                case "线":
                    LineMeasure lineMeasure = new LineMeasure();
                    this._treeViewTarget.AddItems(lineMeasure, "线", nameof(lineMeasure.ImageData), nameof(lineMeasure.PixCoordSystem), nameof(lineMeasure.WcsLine)); //, nameof(lineMeasure.Result)
                    break;

                case "圆":
                    CircleMeasure circleMeasure = new CircleMeasure();
                    this._treeViewTarget.AddItems(circleMeasure, "圆", nameof(circleMeasure.ImageData), nameof(circleMeasure.PixCoordSystem), nameof(circleMeasure.WcsCircle)); //, nameof(circleMeasure.Result)

                    break;

                case "圆弧":
                    CircleSectorMeasure circleSectorMeasure = new CircleSectorMeasure();
                    this._treeViewTarget.AddItems(circleSectorMeasure, "圆弧", nameof(circleSectorMeasure.ImageData), nameof(circleSectorMeasure.PixCoordSystem), nameof(circleSectorMeasure.WcsCircleSector)); //, nameof(circleSectorMeasure.Result)
                    break;

                case "椭圆":
                    EllipseMeasure ellipseMeasure = new EllipseMeasure();
                    this._treeViewTarget.AddItems(ellipseMeasure, "椭圆", nameof(ellipseMeasure.ImageData), nameof(ellipseMeasure.PixCoordSystem), nameof(ellipseMeasure.WcsEllipse)); //, nameof(ellipseMeasure.Result)
                    break;

                case "椭圆弧":
                    EllipseSectorMeasure ellipseSectorMeasure = new EllipseSectorMeasure();
                    this._treeViewTarget.AddItems(ellipseSectorMeasure, "椭圆弧", nameof(ellipseSectorMeasure.ImageData), nameof(ellipseSectorMeasure.PixCoordSystem), nameof(ellipseSectorMeasure.WcsEllipseSector)); //, nameof(ellipseSectorMeasure.Result)
                    break;

                case "矩形":
                    Rectangle2Measure rect2Measure = new Rectangle2Measure();
                    this._treeViewTarget.AddItems(rect2Measure, "矩形", nameof(rect2Measure.ImageData), nameof(rect2Measure.PixCoordSystem)); //, nameof(rect2Measure.Result)
                    break;

                case "宽度":
                    WidthMeasure width = new WidthMeasure();
                    this._treeViewTarget.AddItems(width, "宽度", nameof(width.ImageData), nameof(width.PixCoordSystem)); //, nameof(rect2Measure.Result)
                    break;
                case "数值计算":
                    this._treeViewTarget.AddItems(new NumericalCalculation(), "数值计算", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2");   // , "结果-=>结果"
                    break;

                case "窗口图像":
                    this._treeViewTarget.AddItems(new GetHWindowImage(), "窗口图像");   // 
                    break;

                case "点位运动":
                    this._treeViewTarget.AddItems(new PointMove(), "点位运动");   // 
                    break;

                case "直线插补":
                    this._treeViewTarget.AddItems(new LineInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "直线插补");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "螺旋线插补":
                    this._treeViewTarget.AddItems(new SpiralLineInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "螺旋线插补");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "矩形插补":
                    this._treeViewTarget.AddItems(new RectangleInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "矩形插补");   // 
                    break;

                case "轨迹运动":
                    this._treeViewTarget.AddItems(new TrackMove(), "轨迹运动");   // 
                    break;

                case "写入IO输出位控制":
                    this._treeViewTarget.AddItems(new WriteIoOutputBit(MotionControlCard.MotionCardManage.CurrentCard), "写入IO输出位控制");   // 
                    break;

                case "写入IO输出组控制":
                    this._treeViewTarget.AddItems(new WriteIoOutputGroup(MotionControlCard.MotionCardManage.CurrentCard), "写入IO输出组控制");   // 
                    break;

                case "读取IO输入位控制":
                    this._treeViewTarget.AddItems(new ReadIoInputBit(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输入位控制");   // 
                    break;

                case "读取IO输出位控制":
                    this._treeViewTarget.AddItems(new ReadIoOutputBit(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输出位控制");   // 
                    break;

                case "读取IO输出组控制":
                    this._treeViewTarget.AddItems(new ReadIoOutputGroup(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输出组控制");   // 
                    break;

                case "读取IO输入组控制":
                    this._treeViewTarget.AddItems(new ReadIoIntputGroup(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输入组控制");   // 
                    break;

                case "直线偏置":
                    if (this._treeViewTarget != null)
                    {
                        LineOffset offset = new LineOffset();
                        this._treeViewTarget.AddItems(offset, "直线偏置", nameof(offset.WcsLine), nameof(offset.OffsetWcsLine));   // , "
                    }
                    break;
                case "直线延伸":
                    if (this._treeViewTarget != null)
                    {
                        LineExtend lineExtend = new LineExtend();
                        this._treeViewTarget.AddItems(lineExtend, "直线延伸", nameof(lineExtend.WcsLine), nameof(lineExtend.ExtendWcsLine));   // , "
                    }
                    break;
                case "直线截取":
                    this._treeViewTarget.AddItems(new LineCrop(), "直线截取", "RefSource1-<=输入3D对象", "RefSource2-<=输入直线对象");   // , 
                    break;  // 

                case "获取标定Mark点":
                    this._treeViewTarget.AddItems(new GetCalibrateMarkPoint(), "获取标定Mark点", "RefSource1-<=输入对象");    //, 
                    break;  // 


                case "二维码识别":
                    DataCodeDetection dataCodeDetection = new DataCodeDetection();
                    this._treeViewTarget.AddItems(dataCodeDetection, "二维码识别", nameof(dataCodeDetection.ImageData), nameof(dataCodeDetection.DataCodeContent));    //, 
                    break;  // 

                case "条形码识别":
                    BarCodeDetection barCodeDetection = new BarCodeDetection();
                    this._treeViewTarget.AddItems(barCodeDetection, "条形码识别", nameof(barCodeDetection.ImageData), nameof(barCodeDetection.DataCodeContent));    //, 
                    break;  // 

                case "光源配置":
                    this._treeViewTarget.AddItems(new LightConfig(), "光源配置");   // 
                    break;

                #region  贴合对位功能

                case "参考位示教":
                    this._treeViewTarget.AddItems(new ReferenceTeach(), "参考位示教");
                    break;  // 
                case "特征定位":
                    this._treeViewTarget.AddItems(new FeatureLocalization(), "特征定位"); // DefectDetecting
                    break;  // 
                case "缺陷检测":
                    this._treeViewTarget.AddItems(new DefectDetecting(), "缺陷检测"); // 
                    break;  // 
                //case "对位引导":
                //    this._treeViewTarget.AddItems(new AlignmentGuided(), "对位引导"); // 
                //    break;  // 

                case "胶枪引导":
                    this._treeViewTarget.AddItems(new GlueAlignGuided(), "胶枪引导"); // 
                    break;  // 

                case "对齐计算":
                    AlignmentGuidedCalculation alignmentGuided = new AlignmentGuidedCalculation();
                    this._treeViewTarget.AddItems(alignmentGuided, "对齐计算", nameof(alignmentGuided.RefPoint), nameof(alignmentGuided.CurPoint), nameof(alignmentGuided.AddXYTheta));
                    break;  // 

                case "并发执行":
                    this._treeViewTarget.AddItems(new ConcurrentExecution(), "并发执行"); //, "RefSource1-<=输入对象"
                    break;  // 

                case "提取点":
                    this._treeViewTarget.AddItems(new ExtractPoint(), "提取点", "RefSource1-<=输入对象");  // , "点对象-=>点对象" 
                    break;  // 

                case "计算偏移":
                    CalculateOffsetValue calculateOffset = new CalculateOffsetValue();
                    this._treeViewTarget.AddItems(calculateOffset, "计算偏移", nameof(calculateOffset.RefPoint), nameof(calculateOffset.CurPoint), nameof(calculateOffset.OffsetXYTheta));
                    break;  //

                case "循环控制":
                    this._treeViewTarget.AddItems(new ForLoopControl(), "循环控制");  // ,字符识别 
                    break;  //

                case "数据读取":
                    DataRead dataRead = new DataRead();
                    this._treeViewTarget.AddItems(dataRead, "数据读取", nameof(dataRead.ReadContent));  // ,字符识别   , nameof(dataRead.Result)
                    break;  //

                case "数据写入":
                    DataWrite dataWrite = new DataWrite();
                    this._treeViewTarget.AddItems(dataWrite, "数据写入", nameof(dataWrite.WriteData));  // ,字符识别  , nameof(dataWrite.Result)
                    break;  //

                case "数据等待":
                    DataWaitePlc dataWaite = new DataWaitePlc();
                    this._treeViewTarget.AddItems(dataWaite, "数据等待", nameof(dataWaite.WaiteContent));  // ,字符识别  , nameof(dataWaite.Result)
                    break;  //

                case "胶路检测":
                    GlueDetect glueDetect = new GlueDetect();
                    this._treeViewTarget.AddItems(glueDetect, "胶路检测");  // ,字符识别 
                    break;  //统计结果

                case "读取文件":
                    ReadFile readFile = new ReadFile();
                    this._treeViewTarget.AddItems(readFile, "读取文件", nameof(readFile.CoordPointData));  // ,字符识别 
                    break;  //统计结果

                case "写入文件":
                    WriteFile writeFile = new WriteFile();
                    this._treeViewTarget.AddItems(writeFile, "写入文件", nameof(writeFile.CoordPoint));  // ,字符识别 
                    break;  //统计结果

                case "坐标映射":
                    CoordMap coordMap = new CoordMap();
                    this._treeViewTarget.AddItems(coordMap, "坐标映射", nameof(coordMap.CoordPoint1), nameof(coordMap.CoordPoint2), nameof(coordMap.HomMat2D));  // ,字符识别 
                    break;  //统计结果

                case "坐标变换":
                    CoordAffine coordAffine = new CoordAffine();
                    this._treeViewTarget.AddItems(coordAffine, "坐标变换", nameof(coordAffine.CoordPointData), nameof(coordAffine.HomMat2D), nameof(coordAffine.OutCoordPointData));  // ,字符识别 
                    break;  // 

                case "阵列测量":
                    MeasureArray measureArray = new MeasureArray();
                    this._treeViewTarget.AddItems(measureArray, "阵列测量");  // ,  , nameof(measureArray.PixCoordSystem)
                    break;  //

                case "流程单元":
                    JobUnit jobUnit = new JobUnit();
                    this._treeViewTarget.AddItems(jobUnit, "流程单元");  // , 
                    break;  // 

                case "偏差计算":
                    OffsetCalculate offsetCalculate = new OffsetCalculate();
                    this._treeViewTarget.AddItems(offsetCalculate, "偏差计算", nameof(offsetCalculate.TargetPoint), nameof(offsetCalculate.SourcePoint), nameof(offsetCalculate.AddXYTheta));
                    break;  // 

                case "对位计算":
                    AlignCalculate alignCalculate = new AlignCalculate();
                    this._treeViewTarget.AddItems(alignCalculate, "对位计算", nameof(alignCalculate.TargetPoint), nameof(alignCalculate.SourcePoint), nameof(alignCalculate.AffinePoint), nameof(alignCalculate.AddXYTheta));
                    break;  // 

                #endregion

                #region 2D 匹配
                case "变形匹配":
                    if (this._treeViewTarget != null)
                    {
                        LocalDeformableModelMatch deformableModelMatch = new LocalDeformableModelMatch();
                        this._treeViewTarget.AddItems(deformableModelMatch, "变形匹配",
                                                      nameof(deformableModelMatch.ImageData),
                                                      nameof(deformableModelMatch.RectifyImageData));

                    }
                    break;

                case "NCC匹配":
                    if (this._treeViewTarget != null)
                    {
                        NccModelMatch doNccModelMatch2D = new NccModelMatch();
                        this._treeViewTarget.AddItems(doNccModelMatch2D, "NCC匹配",
                            nameof(doNccModelMatch2D.ImageData),
                            nameof(doNccModelMatch2D.WcsSingleCoordSystem),
                            nameof(doNccModelMatch2D.RectifyImageData),
                            nameof(doNccModelMatch2D.PixCoordSystem),
                            nameof(doNccModelMatch2D.WcsCoordSystem));
                    }
                    break;
                case "形状匹配":
                case "形状匹配2D":
                case "坐标系-形状匹配":
                case "坐标系-2D形状匹配":
                    if (this._treeViewTarget != null)
                    {
                        ShapeModelMatch2D doShapeModelMatch2D = new ShapeModelMatch2D();
                        this._treeViewTarget.AddItems(doShapeModelMatch2D, "形状匹配",
                        nameof(doShapeModelMatch2D.ImageData),
                        nameof(doShapeModelMatch2D.WcsSingleCoordSystem),
                        nameof(doShapeModelMatch2D.RectifyImageData),
                        nameof(doShapeModelMatch2D.PixCoordSystem),
                        nameof(doShapeModelMatch2D.WcsCoordSystem)
                        ); //   
                    }

                    break;
                #endregion

                #region 区域操作

                case "拟合矩形区域":
                    FitRect2Region fitRect2Region = new FitRect2Region();
                    this._treeViewTarget.AddItems(fitRect2Region, "拟合矩形区域", nameof(fitRect2Region.RegionData), nameof(fitRect2Region.Rect2));
                    break;  //

                case "区域运算":
                    RegionArithmetic regionArithmetic = new RegionArithmetic();
                    this._treeViewTarget.AddItems(regionArithmetic, "区域运算", nameof(regionArithmetic.RegionData), nameof(regionArithmetic.RegionData2), nameof(regionArithmetic.OutRegionData));
                    break;  //
                #endregion

                #region 图像操作
                case "字符识别":
                    Ocr doOcr = new Ocr();
                    this._treeViewTarget.AddItems(doOcr, "字符识别", nameof(doOcr.ImageData), nameof(doOcr.ChartRegion)); //, nameof(doOcr.OcrResult)
                    break;  //
                case "图像处理":
                    ImageMorphology doImageMorphology = new ImageMorphology();
                    this._treeViewTarget.AddItems(doImageMorphology, "图像处理", nameof(doImageMorphology.ImageData), nameof(doImageMorphology.OutImageData)); // nameof(doImageMorphology.ResultInfo),  nameof(doImageMorphology.Result)
                    break;  //
                case "图像滤波":
                    ImageFilter imageFilter = new ImageFilter();
                    this._treeViewTarget.AddItems(imageFilter, "图像滤波", nameof(imageFilter.ImageData), nameof(imageFilter.OutImageData));
                    break;  //
                case "图像增强":
                    ImageEnhancement imageEnhan = new ImageEnhancement();
                    this._treeViewTarget.AddItems(imageEnhan, "图像增强", nameof(imageEnhan.ImageData), nameof(imageEnhan.OutImageData));
                    break;  //
                case "图像变换":
                    ImageAffine imageAffine = new ImageAffine();
                    this._treeViewTarget.AddItems(imageAffine, "图像变换", nameof(imageAffine.ImageData), nameof(imageAffine.PixCoordSystem), nameof(imageAffine.OutImageData));
                    break;  //
                case "图像旋转":
                    ImageRotate imageRotate = new ImageRotate();
                    this._treeViewTarget.AddItems(imageRotate, "图像旋转", nameof(imageRotate.ImageData), nameof(imageRotate.PixCoordSystem), nameof(imageRotate.OutImageData));
                    break;  //
                case "图像运算":
                    ImageArithmetic imageArith = new ImageArithmetic();
                    this._treeViewTarget.AddItems(imageArith, "图像运算", nameof(imageArith.ImageData), nameof(imageArith.ImageData2), nameof(imageArith.OutImageData));
                    break;  //
                case "图像平铺":
                    if (this._treeViewTarget != null)
                    {
                        TileImage tileImage = new TileImage();
                        this._treeViewTarget.AddItems(tileImage, "图像平铺", nameof(tileImage.ImageData), nameof(tileImage.TileImageData));
                    }
                    break;
                case "图像裁剪":
                    ImageReduce reduceImageDomain = new ImageReduce();
                    this._treeViewTarget.AddItems(reduceImageDomain, "图像裁剪", nameof(reduceImageDomain.ImageData), nameof(reduceImageDomain.PixCoordSystem), nameof(reduceImageDomain.ReduceImage));
                    break; // 

                case "图像缩放":
                    ImageZoom imageZoom = new ImageZoom();
                    this._treeViewTarget.AddItems(imageZoom, "图像缩放", nameof(imageZoom.ImageData), nameof(imageZoom.OutImageData));
                    break; // 

                #endregion

                #region PLC操作
                ///   ,过时操作
                case "读取数据PLC":
                    ReadPlcData readPlcData = new ReadPlcData();
                    this._treeViewTarget.AddItems(readPlcData, "读取数据PLC", nameof(readPlcData.ReadContent)); //, 
                    break;  //

                case "写入数据PLC":
                    WritePlcData writePlcData = new WritePlcData();
                    this._treeViewTarget.AddItems(writePlcData, "写入数据PLC", nameof(writePlcData.WriteContent)); //, 
                    break;  //

                case "写入数据Socket":
                    WriteDataSocket writeDataSocket = new WriteDataSocket();
                    this._treeViewTarget.AddItems(writeDataSocket, "写入数据Socket", nameof(writeDataSocket.WriteContent)); //,  
                    break;  //

                case "发送结果Socket":
                    SendResultSocket SendResul = new SendResultSocket();
                    this._treeViewTarget.AddItems(SendResul, "发送结果Socket", nameof(SendResul.SendContent)); //,  
                    break;  //

                case "保存数据PLC":
                    SaveDataPlc saveDataPlc = new SaveDataPlc();
                    this._treeViewTarget.AddItems(saveDataPlc, "保存数据PLC"); //, 
                    break;  //
                            ////////  ,过时操作
                case "延时等待":
                    Waite waite = new Waite();
                    this._treeViewTarget.AddItems(waite, "延时等待"); //
                    break;  //


                case "数据标签":
                    DataLable dataLable = new DataLable();
                    this._treeViewTarget.AddItems(dataLable, "数据标签", nameof(dataLable.TextLable));
                    break;  //

                case "发送数据":
                    SendData sendData = new SendData();
                    this._treeViewTarget.AddItems(sendData, "发送数据", nameof(sendData.SendContent));
                    break;  //

                case "等待信号":
                    WaiteSignal waiteData = new WaiteSignal();
                    this._treeViewTarget.AddItems(waiteData, "等待信号", nameof(waiteData.WaiteContent));
                    break;  //

                case "写入数据":
                    WriteData writeData = new WriteData();
                    this._treeViewTarget.AddItems(writeData, "写入数据", nameof(writeData.WriteContent));
                    break;  //

                case "读取数据":
                    ReadData readData = new ReadData();
                    this._treeViewTarget.AddItems(readData, "读取数据", nameof(readData.ReadContent));
                    break;  // 

                case "保存数据":
                    SaveDataNew saveDataNew = new SaveDataNew();
                    this._treeViewTarget.AddItems(saveDataNew, "保存数据", nameof(saveDataNew.SaveContent));
                    break;  //

                case "保存数据Ftp":
                    SaveDataFtp saveDataFtp  = new SaveDataFtp();
                    this._treeViewTarget.AddItems(saveDataFtp, "保存数据Ftp", nameof(saveDataFtp.SaveContent));
                    break;  //保存数据Ftp

                case "条件判断":
                    ConditionalJudge conditionalJudge = new ConditionalJudge();
                    this._treeViewTarget.AddItems(conditionalJudge, "条件判断", nameof(conditionalJudge.Content));
                    break;  //
                case "结果判断":
                    ResultJudge resultJudge = new ResultJudge();
                    this._treeViewTarget.AddItems(resultJudge, "结果判断", nameof(resultJudge.Content));
                    break;  //
                #endregion

                #region 缺陷检测  //
                case "直线检测":
                case "边缘检测":
                    LineDetect lineDetect = new LineDetect();
                    this._treeViewTarget.AddItems(lineDetect, "直线检测", nameof(lineDetect.WcsLine), nameof(lineDetect.XldData));
                    break;  //
                case "破片检测":
                    GapDetect gapDetect = new GapDetect();
                    this._treeViewTarget.AddItems(gapDetect, "破片检测", nameof(gapDetect.ImageData), nameof(gapDetect.PixCoordSystem), nameof(gapDetect.GapRegion));
                    break;  // 
                case "脚本工具":
                    ScriptTool scriptTool = new ScriptTool();
                    this._treeViewTarget.AddItems(scriptTool, "脚本工具", nameof(scriptTool.ImageData), nameof(scriptTool.PixCoordSystem), nameof(scriptTool.FlawRegion));
                    break;  // 

                case "测量检测":
                    MeasureDetect measureDetect = new MeasureDetect();
                    this._treeViewTarget.AddItems(measureDetect, "测量检测", nameof(measureDetect.ImageData), nameof(measureDetect.FlawRegion));
                    break;  // 
                #endregion


                #region 引导变换
                case "引导计算":
                    GuidedCalculate guidedCalculate = new GuidedCalculate();
                    this._treeViewTarget.AddItems(guidedCalculate, "引导计算", nameof(guidedCalculate.HomMat2D));
                    break;
                case "引导标定":
                    GuidedCalib guidedCalib = new GuidedCalib();
                    this._treeViewTarget.AddItems(guidedCalib, "引导标定", nameof(guidedCalib.WcsPoint), nameof(guidedCalib.HomMat2D));
                    break;
                case "点变换":
                case "点引导变换":
                    PointAffine pointAffine = new PointAffine();
                    this._treeViewTarget.AddItems(pointAffine, "点引导变换", nameof(pointAffine.WcsPoint), nameof(pointAffine.AffinePoint)); //, nameof(pointAffine.HomMat2D)
                    break;  //
                case "线变换":
                case "线引导变换":
                    LineAffine lineAffine = new LineAffine();
                    this._treeViewTarget.AddItems(lineAffine, "线引导变换", nameof(lineAffine.WcsLine), nameof(lineAffine.AffineLine)); //, nameof(lineAffine.HomMat2D)
                    break;  //
                case "圆变换":
                case "圆引导变换":
                    CircleAffine circleAffine = new CircleAffine();
                    this._treeViewTarget.AddItems(circleAffine, "圆引导变换", nameof(circleAffine.WcsCircle), nameof(circleAffine.AffineCircle)); //, , nameof(circleAffine.HomMat2D)
                    break;  //
                case "椭圆变换":
                case "椭圆引导变换":
                    EllpiseAffine ellpiseAffine = new EllpiseAffine();
                    this._treeViewTarget.AddItems(ellpiseAffine, "椭圆引导变换", nameof(ellpiseAffine.WcsEllipse), nameof(ellpiseAffine.AffineEllipse)); //, , nameof(ellpiseAffine.HomMat2D)
                    break;  //
                case "矩形变换":
                case "矩形引导变换":
                    Rect2Affine rect2Affine = new Rect2Affine();
                    this._treeViewTarget.AddItems(rect2Affine, "矩形引导变换", nameof(rect2Affine.WcsRect2), nameof(rect2Affine.AffineRect2)); //, , nameof(rect2Affine.HomMat2D)
                    break;  //
                case "轨迹变换":
                    TrackAffine trackAffine = new TrackAffine();
                    this._treeViewTarget.AddItems(trackAffine, "轨迹变换", nameof(trackAffine.TrackPoint), nameof(trackAffine.AffinePoint)); //, , nameof(rect2Affine.HomMat2D)
                    break;  //
                case "轨迹计算":
                    TrackCalculate trackCalculate = new TrackCalculate();
                    this._treeViewTarget.AddItems(trackCalculate, "轨迹计算", nameof(trackCalculate.TrackPoint), nameof(trackCalculate.WcsVector)); //, , nameof(rect2Affine.HomMat2D)
                    break;  //
                #endregion

                #region 上下料
                case "下料Try盘":
                    LayOffTryPlate layOffTryPlate = new LayOffTryPlate();
                    this._treeViewTarget.AddItems(layOffTryPlate, "下料Try盘", nameof(layOffTryPlate.TargetPoint), nameof(layOffTryPlate.PlatformParam));
                    break;  //
                case "下料穴位":
                    RobotLayOff robotLayOff = new RobotLayOff();
                    this._treeViewTarget.AddItems(robotLayOff, "下料穴位", nameof(robotLayOff.ImageData), nameof(robotLayOff.TryPlatformParam), nameof(robotLayOff.AddXYTheta));
                    break;  //
                case "上料Try盘":
                    LoadTryPlate loadTryPlate = new LoadTryPlate();
                    this._treeViewTarget.AddItems(loadTryPlate, "上料Try盘", nameof(loadTryPlate.ImageData), nameof(loadTryPlate.PlatformParam));
                    break;  //
                case "上料穴位":
                    RobotLoad robotLoad = new RobotLoad();
                    this._treeViewTarget.AddItems(robotLoad, "上料穴位", nameof(robotLoad.WcsVector), nameof(robotLoad.TryPlatformParam), nameof(robotLoad.AddXYTheta));
                    break;  //
                #endregion


                #region 标定
                case "N点标定":
                    MapCalib nPointCalib = new MapCalib();
                    this._treeViewTarget.AddItems(nPointCalib, "N点标定", nameof(nPointCalib.SourcePoint), nameof(nPointCalib.TargetPoint), nameof(nPointCalib.HomMat2D));
                    break;  //
                case "旋转标定":
                    CalibRotateCenter rotateCenter = new CalibRotateCenter();
                    this._treeViewTarget.AddItems(rotateCenter, "旋转标定", nameof(rotateCenter.SourcePoint), nameof(rotateCenter.TargetPoint), nameof(rotateCenter.WcsCircle), nameof(rotateCenter.PixCircle));
                    break;  //
                #endregion

                #region 流程控制
                case "条件语句":
                    Conditional conditional = new Conditional();
                    this._treeViewTarget.AddItems(conditional, "条件语句");
                    break;  //
                case "条件语句<If>":
                    ConditionaIf conditionaIf = new ConditionaIf();
                    this._treeViewTarget.AddItems(conditionaIf, "条件语句<If>");
                    break;  //
                case "条件语句<else>":
                    ConditionaElse conditionaElse = new ConditionaElse();
                    this._treeViewTarget.AddItems(conditionaElse, "条件语句<else>");
                    break;  //
                #endregion

                default:
                    break;
            }
        }

        private IFunction GetCurrentCoordSystem()
        {
            //if (FunctionManage.CoordSystemList.Count > 0)
            //    return FunctionManage.CoordSystemList.Last();
            //else
            return null;


        }
        private IFunction GetCurrentImageSource()
        {
            //if (FunctionManage.ImageSourceList.Count > 0)
            //    return FunctionManage.ImageSourceList.Last();
            //else
            return null;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }



    }
}
