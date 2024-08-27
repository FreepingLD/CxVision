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
    public partial class ToolThickness : Form
    {
        //private AddAcqSourceForm form;
        //private ProgramForm _up; // 因为是这里面的事件引发 ProcessForm 里的动作
        private TreeViewWrapClass _treeViewTarget;
        public ToolThickness()
        {
            InitializeComponent();
        }

        public void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string name = e.Node.Text;
            if (this._treeViewTarget == null)
            {
                //this._up = getForm().TreeViewWrapClass;
            }
            //return;
            switch (name.Trim())
            {
                case "激光线":
                    if (this._treeViewTarget != null)
                    {
                        LaserLineScanAcq laserLineAcq;
                        if (AcqSourceManage.Instance.LaserAcqSourceList().Count > 0 && AcqSourceManage.Instance.GetCamAcqSourceList().Count > 0)
                            laserLineAcq = new LaserLineScanAcq(AcqSourceManage.Instance.LaserAcqSourceList().Last(), AcqSourceManage.Instance.GetCamAcqSourceList().Last());
                        else
                            laserLineAcq = new LaserLineScanAcq(new AcqSource(), new AcqSource());
                        ///////////////////////////
                        this._treeViewTarget.AddItems(laserLineAcq, laserLineAcq.Name, nameof(laserLineAcq.LaserAcqSource), nameof(laserLineAcq.CamAcqSource),
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
                case "图像采集":
                    if (this._treeViewTarget != null)
                    {
                        ImageAcq imageAcq = new ImageAcq(AcqSourceManage.Instance.CurrentAcqSource.Name);
                        this._treeViewTarget.AddItems(imageAcq, "图像采集", nameof(imageAcq.ImageData), nameof(imageAcq.DarkImageData)); //选择3D对象范围
                    }
                    break;
                case "双激光采点测厚":

                    break;

                case "双激光圆弧测厚":

                    break;

                case "双激光扫描测厚":

                    break;

                case "双激光采点测厚2":

                    break;

                case "双激光扫描测厚2":

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


                case "创建3D位姿":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CreatePose3D(), "创建3D位姿", "3D坐标系-=>3D坐标系");
                    break;

                case "翻转对象3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new OverturnObjectModel3D(), "翻转对象3D", "RefSource1-<=输入3D对象", "3D对象-=>3D对象");
                    break;

                case "读取3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ReadObjectModel3D(), "读取3D对象", "3D对象-=>3D对象");
                    break;
                case "保存3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SaveObjectModel3D(), "保存3D对象", "RefSource1-<=输入3D对象");
                    break;
                case "平面度":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Planeness(), "平面度", "RefSource1-<=输入3D对象", "平面度-=>平面度");
                    break;
                case "体积":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Volume(), "体积", "RefSource1-<=输入3D对象", "体积-=>体积");
                    break;
                case "厚度":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Thickness(), "厚度", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "厚度-=>厚度", "激光1距离-=>激光1距离", "激光2距离-=>激光2距离", "坐标位置-=>坐标位置");
                    break;
                case "提取3D截面":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Section(), "提取3D截面", "RefSource1-<=输入3D对象", "轮廓3D对象-=>轮廓3D对象");
                    break;
                case "点到面距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToFaceDist(), "点到面距离3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值");
                    break;
                case "点到线距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToLineDist3D(), "点到线距离3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值");
                    break;
                case "点到点距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToPointDist3D(), "点到点距离3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "两点距离-=>两点距离", "距离X-=>距离X", "距离Y-=>距离Y", "距离Z-=>距离Z");
                    break;
                case "线到线距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineToLineDist3D(), "线到线距离3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值");
                    break;

                case "面到面距离3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FaceToFaceDist3D(), "面到面距离3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "平均值-=>平均值");
                    break;

                case "采样3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SampleObjectModel3D(), "采样3D对象", "RefSource1-<=输入3D对象1", "3D对象-=>3D对象"); //选择3D对象范围
                    break;

                case "选择3D对象范围":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SelectObjectModelRange3D(), "选择3D对象范围", "RefSource1-<=输入3D对象", "3D对象-=>3D对象"); //选择3D对象范围
                    break;
                case "滤波3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FilterObjectModel3D(), "滤波3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象"); //选择3D对象范围
                    break;
                case "分割3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SegmentObjectModle3D(), "分割3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象"); //选择3D对象范围
                    break;
                case "合并3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Union3DobjectModel(), "合并3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象"); //选择3D对象范围
                    break;
                case "平滑3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SmoothObjectModel3D(), "平滑3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象"); //选择3D对象范围
                    break;
                case "平滑3D轮廓对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SmoothProfileModel3D(), "平滑3D轮廓对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象"); //选择3D对象范围
                    break;
                //case "注册匹配":
                //    if (this._up != null)
                //        this._up.AddItems(new RegisterMatchObjectModel3D(), "注册匹配", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象1", "3D坐标系-=>"); //选择3D对象范围
                //    break;

                case "坐标系-表面匹配":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new DoSurfaceModelMatch(), "坐标系-表面匹配", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "3D坐标系-=>3D坐标系", "场景采样点3D对象-=>场景采样点3D对象", "关键点3D对象-=>关键点3D对象", "变形采样点3D对象-=>变形采样点3D对象"); //选择3D对象范围
                    break;

                //case "创建表面模型":
                //    if (this._up != null)
                //        this._up.AddItems(new CreateSurfaceMatchModel3D(), "创建表面模型", "RefSource1-<=输入3D对象", "表面模型-=>表面模型", "模型采样点-=>模型采样点"); //选择3D对象范围
                //    break;

                case "变换3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AffineObjectModel3D(), "变换3D对象", "RefSource1-<=输入3D对象", "RefSource2-<=坐标系", "3D对象-=>3D对象"); //选择3D对象范围
                    break;

                case "缩放3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ScaleObjectModel3D(), "缩放3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象"); //选择3D对象范围
                    break;
                case "转换3D对象到图像":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new TransformObject3DToXYZImage(), "转换3D对象到图像", "RefSource1-<=输入3D对象", "实值图像X-=>实值图像X", "实值图像Y-=>实值图像Y", "实值图像Z-=>实值图像Z", "字节图像X-=>字节图像X", "字节图像Y-=>字节图像Y", "字节图像Z-=>字节图像Z"); //选择3D对象范围
                    break;

                case "转换图像到3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new TransformXYZImageToObject3D(), "转换图像到3D对象", "RefSource1-<=输入图像X", "RefSource2-<=输入图像Y", "RefSource3-<=输入图像Z", "3D对象-=>3D对象"); // "3D位姿"
                    break;

                case "角平分线":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CalculateAngleBisector(), "角平分线", "RefSource1-<=输入直线1", "RefSource2-<=输入直线1", "直线对象-=>直线对象"); //选择3D对象范围
                    break;

                case "线线距离":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineToLineDist2D(), "线线距离", "RefSource1-<=输入直线1", "RefSource2-<=输入直线2", "距离-=>距离"); //选择3D对象范围
                    break;

                case "点点距离":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToLineDist2D(), "点点距离", "RefSource1-<=输入点1", "RefSource2-<=输入点2", "距离-=>距离", "水平距离-=>水平距离", "垂直距离-=>垂直距离"); //选择3D对象范围
                    break;

                case "点线距离":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new PointToLineDist2D(), "点线距离", "RefSource1-<=输入点", "RefSource2-<=输入直线", "距离-=>距离"); //选择3D对象范围
                    break;

                case "圆线距离":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CircleToLineDist2D(), "圆线距离", "RefSource1-<=输入圆", "RefSource2-<=输入直线", "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值"); //选择3D对象范围
                    break;

                case "圆圆距离":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CircleToCircleDist2D(), "圆圆距离", "RefSource1-<=输入圆1", "RefSource2-<=输入圆2", "最大值-=>最大值", "最小值-=>最小值", "平均值-=>平均值"); //选择3D对象范围
                    break;

                case "直线中点":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineMiddlePoint(), "直线中点", "RefSource1-<=输入直线", "中点-=>中点", "X-=>X", "Y-=>Y", "Z-=>Z"); //选择3D对象范围
                    break;


                //case "图像分割":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new FunctionBlock.DoBlob(), "图像分割", "RefSource1-<=输入图像", "分割区域-=>分割区域"); //选择3D对象范围 ,"选择区域"
                //    break;

                //case "区域膨胀腐蚀":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new FunctionBlock.RegionDilationErosion(), "区域膨胀腐蚀", "RefSource1-<=输入区域", "区域-=>区域"); //选择3D对象范围
                //    break;

                //case "选择区域":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new FunctionBlock.SelectRegion(), "选择区域", "RefSource1-<=输入区域", "区域-=>区域", "参考点-=>参考点"); //选择3D对象范围
                //    break;

                case "读取图像":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FunctionBlock.ReadImage(), "读取图像", "图像-=>图像"); //选择3D对象范围
                    break;

                //case "减小图像定义域":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new FunctionBlock.ImageReduce(), "减小图像定义域", "RefSource1-<=输入图像", "RefSource2-<=输入区域", "图像对象-=>图像对象"); //选择3D对象范围   
                //    break;

                case "提取XLD轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ExtractXLD(), "提取XLD轮廓", "RefSource1-<=输入图像", "XLD轮廓-=>XLD轮廓"); //选择3D对象范围   
                    break;

                case "选择XLD轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SelectionXLD(), "选择XLD轮廓", "RefSource1-<=输入XLD轮廓", "XLD轮廓-=>XLD轮廓"); //选择3D对象范围   
                    break;

                case "数据输出":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new OutputData(), "数据输出", "RefSource1-<=输入对象");
                    break;

                case "取反平移3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new InvertTranslateObjectModel3D(), "取反平移3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象");
                    break;

                //case "矩形取点":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new Rectangle2Crop(), "矩形取点", "RefSource1-<=输入3D对象", "RefSource2-<=坐标系", "3D对象-=>3D对象");
                //    break;

                case "圆形取点":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CircleCrop(), "圆形取点", "RefSource1-<=输入3D对象", "RefSource2-<=坐标系", "3D对象-=>3D对象");
                    break;


                case "创建3D基本体":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new CreatePrimitive3D(), "创建3D基本体", "RefSource1-<=输入3D位姿", "3D对象-=>3D对象");
                    break;

                case "变换XLD轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AffineXLDCont2D(), "变换XLD轮廓", "RefSource1-<=输入XLD轮廓", "RefSource2-<=坐标系", "XLD轮廓-=>XLD轮廓");
                    break;


                case "保存图像":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SaveImage(), "保存图像", "RefSource1-<=输入图像", "图像-=>图像");
                    break;

                case "保存XLD轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SaveContXLD(), "保存XLD轮廓", "RefSource1-<=输入XLD轮廓", "XLD轮廓-=>XLD轮廓");
                    break;

                case "读取XLD轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ReadContourXLD(), "读取XLD轮廓", "XLD轮廓-=>XLD轮廓");
                    break;

                case "采样XLD轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new SampleXLD(), "采样XLD轮廓", "RefSource1-<=输入XLD轮廓", "平滑XLD轮廓-=>", "像素采样点-=>像素采样点", "世界采样点-=>世界采样点");
                    break;

                case "计算对象3D位姿":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new GetPrimitive3DPose(), "计算对象3D位姿", "RefSource1-<=输入3D对象", "3D位姿-=>3D位姿");
                    break;

                case "校平3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new RectifyObjectModel3D(), "校平3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象");
                    break;

                case "平面相交3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new IntersectPlaneObjectModel3D(), "平面相交3D对象", "RefSource1-<=平面对象", "RefSource2-<=输入3D对象", "3D对象-=>3D对象");
                    break;

                case "变换3D位姿":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AffinePose3D(), "变换3D位姿", "RefSource1-<=输入坐标系", "3D坐标系-=>3D坐标系");
                    break;

                //case "创建圆形区域":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new CreateCircleRegion(), "创建圆形区域", "RefSource1-<=坐标系", "圆区域-=>圆区域");
                //    break;

                //case "创建矩形区域":
                //    if (this._treeViewTarget != null)
                //        this._treeViewTarget.AddItems(new CreateRectangle2Region(), "创建矩形区域", "RefSource1-<=坐标系", "矩形区域-=>矩形区域");   // 减少3D对象定义域
                //    break;

                case "获取极值轮廓":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new GetMinMaxValueObjectModel(), "获取极值轮廓", "RefSource1-<=输入3D对象", "RefSource2-<=参考直线", "3D对象-=>3D对象", "直线对象-=>直线对象");   // 减少3D对象定义域
                    break;

                case "线线交点":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new IntersectionLineLine(), "线线交点", "RefSource1-<=输入直线1", "RefSource2-<=输入直线2", "点对象-=>点对象");   // 减少3D对象定义域
                    break;

                case "线圆交点":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new IntersectionLineCirlce(), "线圆交点", "RefSource1-<=输入直线", "RefSource2-<=输入圆", "点对象-=>点对象");   // 减少3D对象定义域
                    break;

                case "圆圆交点":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new IntersectionCircleCircle(), "圆圆交点", "RefSource1-<=输入圆对象1", "RefSource2-<=输入圆对象2", "点对象-=>点对象");   // 减少3D对象定义域
                    break;

                case "线线夹角":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AngleLineLine2D(), "线线夹角", "RefSource1-<=输入直线1", "RefSource2-<=输入直线2", "角度-=>角度");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "显示多个3D对象":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ShowMultipleObjects(), "显示多个3D对象", "RefSource1-<=输入3D对象", "3D对象-=>3D对象");   // 减少3D对象定义域  
                    break;

                case "直线夹角3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new AngleLineLine3D(), "直线夹角3D", "RefSource1-<=输入3D对象1", "RefSource2-<=输入3D对象2", "角度-=>角度");   // 减少3D对象定义域  
                    break;

                case "直线方向3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineOrientation3D(), "直线方向3D", "RefSource1-<=输入3D对象", "角度X-=>角度X", "角度Y-=>角度Y", "角度Z-=>角度Z");   // 减少3D对象定义域  
                    break;

                case "直线长度3D": // 这个算子已经没意义了
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new LineLength3D(), "直线长度3D", "RefSource1-<=输入3D对象", "长度3D-=>长度3D");   // 减少3D对象定义域   渲染3D对象到图像  定点采样3D对象
                    break;

                case "定点采样3D对象": // 这个算子已经没意义了
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FixedPointSampleModel(), "定点采样3D对象", "RefSource1-<=输入3D对象", "RefSource2-<=输入采样点", "采样3D对象-=>采样3D对象");   // 减少3D对象定义域   渲染3D对象到图像  
                    break;


                case "渲染3D对象到图像":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new RenderObjectModel3DToImage(), "渲染3D对象到图像", "RefSource1-<=输入3D对象", "图像1-=>图像1", "图像2-=>图像2", "图像3-=>图像3");   // 减少3D对象定义域   渲染3D对象到图像 转换像素点到世界点
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


                case "拟合3D平面":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FittingPlane3D(), "拟合3D平面", "RefSource1-<=输入3D对象", "平面对象-=>平面对象");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;
                case "拟合3D圆柱":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FittingCylinder3D(), "拟合3D圆柱", "RefSource1-<=输入3D对象", "圆柱对象-=>圆柱对象", "半径-=>半径", "直径-=>直径");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;
                case "拟合3D球体":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FittingSphere3D(), "拟合3D球体", "RefSource1-<=输入3D对象", "球对象-=>球对象", "半径-=>半径", "直径-=>直径");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;
                case "拟合3D盒子":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FittingBox3D(), "拟合3D盒子", "RefSource1-<=输入3D对象", "3D位姿-=>3D位姿", "盒子对象-=>盒子对象");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break; //Y

                case "拟合3D轮廓圆":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FitProfileCircle3D(), "拟合3D轮廓圆", "RefSource1-<=输入3D对象", "半径-=>半径", "直径-=>直径");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break; //拟合3D轮廓圆

                case "拟合直线":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FitLine(), "拟合直线", "RefSource1-<=输入对象", "直线坐标-=>直线坐标");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "拟合圆":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FitCircle(), "拟合圆", "RefSource1-<=输入对象", "半径-=>半径", "直径-=>直径", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break; // N点拟合圆

                case "N点拟合圆":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new NPointsFitCircle(), "N点拟合圆", "RefSource1-<=输入对象", "半径-=>半径", "直径-=>直径", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break; // 

                case "拟合椭圆":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FitEllipse(), "拟合椭圆", "RefSource1-<=输入对象", "半径R1-=>半径R1", "半径R2-=>半径R2", "长轴-=>长轴", "短轴-=>短轴", "角度-=>角度", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "图像平铺":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new TileImage(), "图像拼接", "图像对象-=>图像对象", "高度图像-=>高度图像");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "面面夹角":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new FaceToFaceAngle(), "面面夹角", "RefSource1-<=输入平面1", "RefSource2-<=输入平面2", "角度-=>角度");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "直线度3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new Straightness3D(), "直线度3D", "RefSource1-<=输入3D对象", "直线度-=>直线度");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "线轮廓度3D":
                    if (this._treeViewTarget != null)
                        this._treeViewTarget.AddItems(new ProfileTolerance(), "线轮廓度3D", "RefSource1-<=测量3D对象", "RefSource2-<=参考3D对象", "轮廓度-=>轮廓度");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;



                case "点":
                    PointMeasure pointMeasure = new PointMeasure();
                    this._treeViewTarget.AddItems(pointMeasure, "点", nameof(pointMeasure.ImageData), nameof(pointMeasure.WcsCoordSystem), nameof(pointMeasure.WcsPoint)); //, "点坐标-=>点坐标"
                    //if (SensorManage.CameraList.Count == 0 || GetCurrentImageSource() != null) //GetImageSource() != null  GetCurrentCamAcqSource()
                    //    this._treeViewTarget.AddItems(new PointMeasure(GetCurrentImageSource(), GetCurrentCoordSystem()), "点", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系"); //, "点坐标-=>点坐标"
                    //else
                    //    this._treeViewTarget.AddItems(new PointMeasure(AcqSourceManage.CurrentAcqSource.Name, GetCurrentCoordSystem()), "点", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系"); //, "点坐标-=>点坐标"
                    break;

                case "线":
                    LineMeasure lineMeasure = new LineMeasure();
                    this._treeViewTarget.AddItems(lineMeasure, "线", nameof(lineMeasure.ImageData), nameof(lineMeasure.WcsCoordSystem), nameof(lineMeasure.WcsLine));

                    //if (SensorManage.CameraList.Count == 0 || GetCurrentImageSource() != null) //GetImageSource() != null
                    //    this._treeViewTarget.AddItems(new LineMeasure(GetCurrentImageSource(), GetCurrentCoordSystem()), "线", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "直线坐标-=>直线坐标", "机台坐标-=>机台坐标" 
                    //else
                    //    this._treeViewTarget.AddItems(new LineMeasure(AcqSourceManage.CurrentAcqSource, GetCurrentCoordSystem()), "线", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "直线坐标-=>直线坐标", "机台坐标-=>机台坐标"
                    break;

                case "圆":
                    CircleMeasure circleMeasure = new CircleMeasure();
                    this._treeViewTarget.AddItems(circleMeasure, "圆", nameof(circleMeasure.ImageData), nameof(circleMeasure.WcsCoordSystem), nameof(circleMeasure.WcsCircle));

                    ////if (SensorManage.CameraList.Count == 0 || GetCurrentImageSource() != null) // =前面的是Name，后面的是Text  GetImageSource() != null
                    ////    this._treeViewTarget.AddItems(new CircleMeasure(GetCurrentImageSource(), GetCurrentCoordSystem()), "圆", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径-=>半径", "直径-=>直径", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    ////else
                    ////    this._treeViewTarget.AddItems(new CircleMeasure(AcqSourceManage.CurrentAcqSource, GetCurrentCoordSystem()), "圆", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径-=>半径", "直径-=>直径", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    break;

                case "圆弧":
                    CircleSectorMeasure circleSectorMeasure = new CircleSectorMeasure();
                    this._treeViewTarget.AddItems(circleSectorMeasure, "圆弧", nameof(circleSectorMeasure.ImageData), nameof(circleSectorMeasure.WcsCoordSystem), nameof(circleSectorMeasure.WcsCircleSector));

                    //if (SensorManage.CameraList.Count == 0 || GetCurrentImageSource() != null)// GetImageSource() != null
                    //    this._treeViewTarget.AddItems(new CircleSectorMeasure(GetCurrentImageSource(), GetCurrentCoordSystem()), "圆弧", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径-=>半径", "直径-=>直径", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    //else
                    //    this._treeViewTarget.AddItems(new CircleSectorMeasure(AcqSourceManage.CurrentAcqSource, GetCurrentCoordSystem()), "圆弧", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径-=>半径", "直径-=>直径", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    break;

                case "椭圆":
                    EllipseMeasure ellipseMeasure = new EllipseMeasure();
                    this._treeViewTarget.AddItems(ellipseMeasure, "椭圆", nameof(ellipseMeasure.ImageData), nameof(ellipseMeasure.WcsCoordSystem), nameof(ellipseMeasure.WcsEllipse));

                    //if (SensorManage.CameraList.Count == 0 || GetCurrentImageSource() != null)//GetImageSource() != null
                    //    this._treeViewTarget.AddItems(new EllipseMeasure(GetCurrentImageSource(), GetCurrentCoordSystem()), "椭圆", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径R1-=>半径R1", "半径R2-=>半径R2", "长轴-=>长轴", "短轴-=>短轴", "角度-=>角度", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    //else
                    //    this._treeViewTarget.AddItems(new EllipseMeasure(AcqSourceManage.CurrentAcqSource.Name, GetCurrentCoordSystem()), "椭圆", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径R1-=>半径R1", "半径R2-=>半径R2", "长轴-=>长轴", "短轴-=>短轴", "角度-=>角度", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    break;

                case "椭圆弧":
                    EllipseSectorMeasure ellipseSectorMeasure = new EllipseSectorMeasure();
                    this._treeViewTarget.AddItems(ellipseSectorMeasure, "椭圆", nameof(ellipseSectorMeasure.ImageData), nameof(ellipseSectorMeasure.WcsCoordSystem), nameof(ellipseSectorMeasure.WcsEllipseSector));

                    //if (SensorManage.CameraList.Count == 0 || GetCurrentImageSource() != null)// GetImageSource() != null
                    //    this._treeViewTarget.AddItems(new EllipseSectorMeasure(GetCurrentImageSource(), GetCurrentCoordSystem()), "椭圆弧", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径R1-=>半径R1", "半径R2-=>半径R2", "长轴-=>长轴", "短轴-=>短轴", "角度-=>角度", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    //else
                    //    this._treeViewTarget.AddItems(new EllipseSectorMeasure(AcqSourceManage.CurrentAcqSource.Name, GetCurrentCoordSystem()), "椭圆弧", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半径R1-=>半径R1", "半径R2-=>半径R2", "长轴-=>长轴", "短轴-=>短轴", "角度-=>角度", "X坐标-=>X坐标", "Y坐标-=>Y坐标", "Z坐标-=>Z坐标"
                    break;

                case "矩形":
                    Rectangle2Measure rect2Measure = new Rectangle2Measure();
                    this._treeViewTarget.AddItems(rect2Measure, "椭圆", nameof(rect2Measure.ImageData), nameof(rect2Measure.WcsCoordSystem), nameof(rect2Measure.WcsRect2));

                    //if (SensorManage.CameraList.Count == 0 || GetCurrentImageSource() != null) // GetImageSource() != null
                    //    this._treeViewTarget.AddItems(new Rectangle2Measure(GetCurrentImageSource(), GetCurrentCoordSystem()), "矩形", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半宽-=>半宽", "半高-=>半高", "角度-=>角度", "矩形坐标-=>矩形坐标"
                    //else
                    //    this._treeViewTarget.AddItems(new Rectangle2Measure(AcqSourceManage.CurrentAcqSource.Name, GetCurrentCoordSystem()), "矩形", "RefSource1-<=输入图像", "RefSource2-<=参考坐标系");   // , "半宽-=>半宽", "半高-=>半高", "角度-=>角度", "矩形坐标-=>矩形坐标"
                    break;

                case "数值计算":
                    this._treeViewTarget.AddItems(new NumericalCalculation(), "数值计算", "RefSource1-<=输入对象1", "RefSource2-<=输入对象2", "结果-=>结果");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "极值点3D":
                    this._treeViewTarget.AddItems(new ExtremePoint3D(), "极值点3D", "RefSource1-<=输入对象3D", "3D对象-=>3D对象");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;


                case "获取窗口图像":
                    this._treeViewTarget.AddItems(new GetHWindowImage(), "获取窗口图像", "图像-=>图像");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;



                case "点位运动":
                    this._treeViewTarget.AddItems(new PointMove(), "点位运动");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "直线插补":
                    this._treeViewTarget.AddItems(new LineInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "直线插补");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "螺旋线插补":
                    this._treeViewTarget.AddItems(new SpiralLineInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "螺旋线插补");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "矩形插补":
                    this._treeViewTarget.AddItems(new RectangleInterpolationMove(MotionControlCard.MotionCardManage.CurrentCard), "矩形插补");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "轨迹运动":
                    this._treeViewTarget.AddItems(new TrackMove(), "轨迹运动");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "写入IO输出位控制":
                    this._treeViewTarget.AddItems(new WriteIoOutputBit(MotionControlCard.MotionCardManage.CurrentCard), "写入IO输出位控制");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "写入IO输出组控制":
                    this._treeViewTarget.AddItems(new WriteIoOutputGroup(MotionControlCard.MotionCardManage.CurrentCard), "写入IO输出组控制");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "读取IO输入位控制":
                    this._treeViewTarget.AddItems(new ReadIoInputBit(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输入位控制");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "读取IO输出位控制":
                    this._treeViewTarget.AddItems(new ReadIoOutputBit(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输出位控制");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "读取IO输出组控制":
                    this._treeViewTarget.AddItems(new ReadIoOutputGroup(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输出组控制");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "读取IO输入组控制":
                    this._treeViewTarget.AddItems(new ReadIoIntputGroup(MotionControlCard.MotionCardManage.CurrentCard), "读取IO输入组控制");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "直线偏置":
                    this._treeViewTarget.AddItems(new LineOffset(), "直线偏置", "RefSource1-<=输入直线对象", "输出直线对象-=>输出直线对象");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;

                case "直线截取":
                    this._treeViewTarget.AddItems(new LineCrop(), "直线截取", "RefSource1-<=输入3D对象", "RefSource2-<=输入直线对象", "3D对象-=>3D对象");   // 减少3D对象定义域   渲染3D对象到图像  3D坐标系
                    break;


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


        //private ProgramForm getForm()
        //{
        //    foreach (var item in Application.OpenForms) // 获取所有窗口来遍历
        //    {
        //        if (item is ProgramForm)
        //        {
        //            return item as ProgramForm;
        //        }
        //    }
        //    return null;
        //}

        private void Tool_Load(object sender, EventArgs e)
        {

        }


    }
}
