using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    /// <summary>
    /// 所有跟相机有关的参数都跟着相机走
    /// <summary>
    /// </summary>
    /// </summary>
    [Serializable]
    public class CameraParam : SensorParam
    {

        [System.Xml.Serialization.XmlIgnore]
        public static Dictionary<string, CameraParam> DicSensorParam { get; set; } = new Dictionary<string, CameraParam>(); // 用于存储所有的相机参数

        [DefaultValue(1000)]
        public double Exposure { get; set; }

        [DefaultValue(1)]
        public double PixScale { get; set; }  // 像素当量用于确定相机标定时计算的当量值

        [DefaultValue(false)]
        public bool IsRot { get; set; }

        [DefaultValue(false)]
        public bool IsActiveDistortionCorrect { get; set; }

        [DefaultValue(0)]
        public string SlantAxis { get; set; } // 相机倾斜

        [DefaultValue(0)]
        public double CamSlant { get; set; } // 相机倾斜

        [NonSerialized]
        private AxisCalibration _CalibrateFile;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public AxisCalibration CalibrateFile { get => this._CalibrateFile; set => this._CalibrateFile = value; }   // 相机测量平面二次校准，，这个应该可以忽略了

        public List<double> FlatData { get; set; }

        /// <summary>
        /// 标定放置与平移参数，通过9点实现
        /// </summary>
        [Browsable(false)]
        public NinePointCalibParam CaliParam { get; set; }

        /// <summary>
        /// N点标定参数，用于直将将图像坐标与世界坐标对应
        /// </summary>
        public CamNPointCalibParam NPointCalibParam { get; set; }

        /// <summary> 相机标定矩阵 </summary>
        public UserHomMat2D HomMat2D { get; set; }

        /// <summary> 相对位姿标定矩阵 </summary>
        public UserHomMat2D MapHomMat2D { get; set; }

        /// <summary> 相对位姿标定矩阵 </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, UserHomMat2D> DicMapHomMat2D { get; set; }
        /// <summary> 相对位姿标定矩阵 </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, UserHomMat3D> DicMapHomMat3D { get; set; }

        public string MapType { get; set; }  //enMapMethod

        /// <summary> 相对位姿标定矩阵 </summary>
        public UserHomMat2D AxisMapHomMat2D { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<CoordValuePairs, UserHomMat2D> DicCalibHomMat2D { get; set; }

        [NonSerialized]
        public HImage _Map;
        /// <summary>
        /// 相机畸变映射参数
        /// </summary>
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public HImage Map { get => this._Map; set => this._Map = value; }

        public bool EnableDistoryRectify
        {
            set;
            get;
        }

        public bool EnableSlantRectify
        {
            set;
            get;
        }
        public bool EnableFlatRectify
        {
            set;
            get;
        }
        /// <summary>
        /// 用天保存 示教向量点
        /// </summary>
        public List<userPixVector> TeachVectorPointPix { get; set; }

        public UvwPlatformParam UvwParam { get; set; }

        /// <summary>
        /// 用于标定板映射，通过标定板将相机关联起来
        /// </summary>
        public CalibBoardParam BoardParam { get; set; }



        public CameraParam()
        {
            this.SensorName = "NONE";
            this.CamSlant = 0;
            this.CalibrateFile = new AxisCalibration();
            this.CaliParam = new NinePointCalibParam();
            this.NPointCalibParam = new CamNPointCalibParam();
            this.PixScale = 1;
            this.DataHeight = 2048;
            this.DataWidth = 2048;
            this.Map = null;
            this.EnableDistoryRectify = false;
            this.HomMat2D = new UserHomMat2D(true);
            this.MapHomMat2D = new UserHomMat2D(true);
            this.AxisMapHomMat2D = new UserHomMat2D();
            this.FlatData = new List<double>();
            this.MapType = "NONE";// enMapMethod.WcsToWcs;
            this.TeachVectorPointPix = new List<userPixVector>();
            this.UvwParam = new UvwPlatformParam();
            this.BoardParam = new CalibBoardParam();
            this.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
            this.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            this.SlantAxis = "Y";
        }
        public CameraParam(UserHomMat2D homMat2D, int imageWidth = 2048, int imageHeight = 2048)
        {
            this.SensorName = "NONE";
            this.CamSlant = 0;
            this.CalibrateFile = new AxisCalibration();
            this.CaliParam = new NinePointCalibParam();
            this.NPointCalibParam = new CamNPointCalibParam();
            this.PixScale = 1;
            this.DataHeight = imageHeight;
            this.DataWidth = imageWidth;
            this.Map = null;
            this.EnableDistoryRectify = false;
            this.HomMat2D = homMat2D;
            this.MapHomMat2D = new UserHomMat2D(true);
            this.AxisMapHomMat2D = new UserHomMat2D();
            this.CaliParam.CamCaliModel = enCamCaliModel.HomMat2D;
            this.FlatData = new List<double>();
            this.MapType = "NONE";// enMapMethod.WcsToWcs;
            this.TeachVectorPointPix = new List<userPixVector>();
            this.UvwParam = new UvwPlatformParam();
            this.BoardParam = new CalibBoardParam();
            this.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
            this.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            this.SlantAxis = "Y";
        }
        public CameraParam(string sensorName)
        {
            this.SensorName = sensorName;
            this.CamSlant = 0;
            this.CalibrateFile = new AxisCalibration();
            this.CaliParam = new NinePointCalibParam(sensorName);
            this.NPointCalibParam = new CamNPointCalibParam();
            this.PixScale = 1;
            this.DataHeight = 2048;
            this.DataWidth = 2048;
            this.Map = null;
            this.EnableDistoryRectify = false;
            this.HomMat2D = new UserHomMat2D(true);
            this.MapHomMat2D = new UserHomMat2D(true);
            this.AxisMapHomMat2D = new UserHomMat2D();
            this.FlatData = new List<double>();
            this.MapType = "NONE";// enMapMethod.WcsToWcs;
            this.TeachVectorPointPix = new List<userPixVector>();
            this.UvwParam = new UvwPlatformParam();
            this.BoardParam = new CalibBoardParam();
            this.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
            this.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            this.SlantAxis = "Y";
        }
        public CameraParam(userCamParam camParam, userCamPose camPose)
        {
            this.CamParam = camParam;
            this.CamPose = camPose;
            this.CalibrateFile = new AxisCalibration();
            this.CaliParam = new NinePointCalibParam();
            this.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
            this.Map = null;
            this.FlatData = new List<double>();
            this.MapType = "NONE";// enMapMethod.WcsToWcs;
            this.TeachVectorPointPix = new List<userPixVector>();
            this.UvwParam = new UvwPlatformParam();
            this.BoardParam = new CalibBoardParam();
            this.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
            this.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            this.SlantAxis = "Y";
        }
        public CameraParam(HTuple camParam, HTuple camPose)
        {
            this.CamParam = new userCamParam(camParam);
            this.CamPose = new userCamPose(camPose);
            this.CalibrateFile = new AxisCalibration();
            this.CaliParam = new NinePointCalibParam();
            this.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
            this.Map = null;
            this.FlatData = new List<double>();
            this.MapType = "NONE";// enMapMethod.WcsToWcs;
            this.TeachVectorPointPix = new List<userPixVector>();
            this.UvwParam = new UvwPlatformParam();
            this.BoardParam = new CalibBoardParam();
            this.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
            this.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            this.SlantAxis = "Y";
        }


        /// <summary>
        /// 像素坐标转换到机械坐标或者世界坐标系,
        /// 2)相机移动：机械坐标 = 计算出的坐标+当前拍照的坐标;相机固定：机械坐标 = 计算出的坐标+当前拍照的坐标*-1
        /// 物体相对于机械坐标原点的坐标：
        /// 1)相机移动：机械坐标 = 计算出的坐标+当前拍照的坐标;
        /// 2）相机固定：机械坐标 = 计算出的坐标+当前拍照的坐标*-1
        /// 物体相对于旋转中心的坐标：
        /// 1）相机移动：相机坐标 = 计算出的坐标 + 当前拍照的坐标 - 标定中心坐标;
        /// 1）相机固定：相机坐标 = 计算出的坐标 + 当前拍照的坐标*-1 + 标定中心坐标;
        /// 注：X轴向右移动为正，Y轴向前移动为正
        /// // 如果以相机作为轴正负的参考对象，相对于旋转中心坐标的计算公式为：cam_x  + grabImage_x - caliCenter_x,
        /// cam_y  + grabImage_y - caliCenter_y,即相机坐标 + 拍照位置 - 标定中心;当以平台做为参考对象时，计算方式相反
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Coluns"></param>
        /// <param name="grabImage_x"></param>
        /// <param name="grabImage_y"></param>
        /// <param name="wcs_x"></param>
        /// <param name="wcs_y"></param>
        public void ImagePointsToWorldPlane(HTuple Rows, HTuple Coluns, double grabImage_x, double grabImage_y, double grabImage_z, out HTuple wcs_x, out HTuple wcs_y, out HTuple wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
            if (Rows == null)
            {
                throw new ArgumentNullException("Rows");
            }
            if (Coluns == null)
            {
                throw new ArgumentNullException("Coluns");
            }
            if (Rows.Length != Coluns.Length)
            {
                throw new ArgumentException("参Rows与Coluns长度不相等");
            }
            if (Rows.Length == 0 || Coluns.Length == 0)
            {
                throw new ArgumentException("参Rows或Coluns长度等于0");
            }
            /////////////////////////////////////////////////////////
            switch (CaliParam.CamCaliModel)
            {
                /// 标定模型这里只实现像素点世界点的转换 
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.CaliBoardMap:
                case enCamCaliModel.九点标定:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.UpDnCamCalibWcs:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    ////////////////////////////////////////////
                    HTuple mapRow, mapCol;
                    mapRow = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(Rows, Coluns, out mapCol);
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(mapCol, mapRow, out Qy);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double x = 0, y = 0;
                        for (int i = 0; i < Rows.Length; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Rows[i].D, Coluns[i].D, item.Key.row, item.Key.col);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            x = crrentHomMat2D.GetHHomMat().AffineTransPoint2d(Coluns[i].D, Rows[i].D, out y);
                            Qx[i] = x;
                            Qy[i] = y;
                        }
                    }
                    else
                        Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
            }
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
                case enCoordOriginType.NONE:
                case enCoordOriginType.相机坐标:
                    wcs_x = Qx;
                    wcs_y = Qy;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = grabImage_x + Qx;   // 拍照坐标 + 计算值
                    else
                        wcs_x = grabImage_x * -1 + Qx;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy;
                    else
                        wcs_y = grabImage_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    break;
                case enCoordOriginType.映射原点:
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = grabImage_x + Qx;   // 拍照坐标 + 计算值
                    else
                        wcs_x = grabImage_x * -1 + Qx;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy;
                    else
                        wcs_y = grabImage_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    ////////映射变换///////
                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心: // 即相对于旋转中心的坐标
                    /////////////////  对位贴合场景坐标系定义必需以平台作为参考对象，往右为正，往前为正  /////////////////////////  这里应该还需要区分圆心移动与不移动
                    if (CaliParam.IsMoveX) 　// 拍照坐标 + 计算值 - 标定中心值 ()
                        wcs_x = (Qx - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X;
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = (Qx - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X + this.CaliParam.CamAxisIncrementX;
                    if (CaliParam.IsMoveY)
                        wcs_y = (Qy - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y = (Qy - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y + this.CaliParam.CamAxisIncrementY;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;    
                    else
                        wcs_z = grabImage_z * -1; 
                    break;
                ///////// 上下料模式，只需把中心点移动到当前点位置////////
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    double nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    double nineCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    ////////////// 这里也需要区分相机移动与相机固定两种情况  /////////////////
                    if (CaliParam.IsMoveX) // 相机移动的处理方式
                        wcs_x = (Qx + nineCenter_x) + (grabImage_x - nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;  // 相机移动方式，相当于将相机原点平移了
                    else // 相机固定的处理方式
                        wcs_x = (Qx + nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY) // 相机移动的处理方式
                        wcs_y = (Qy + nineCenter_y) + (grabImage_y - nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    else // 相机固定的处理方式
                        wcs_y = (Qy + nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.映射变换:
                    switch (this.MapType)
                    {
                        default: // 映射以世界坐标系
                        case "NONE":
                        case "none":
                            if (CaliParam.IsMoveX)
                                wcs_x = grabImage_x + Qx;
                            else
                                wcs_x = grabImage_x * -1 + Qx;
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy;
                            else
                                wcs_y = grabImage_y * -1 + Qy;
                            if (CaliParam.IsMoveZ)
                                Qz = grabImage_z;
                            else
                                Qz = grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.McsToMcs):
                        case nameof(enMapMethod.McsToWcs):
                        case nameof(enMapMethod.WcsToWcs):
                        case nameof(enMapMethod.相机到五轴):
                        case nameof(enMapMethod.相机到激光):
                            if (CaliParam.IsMoveX)
                                wcs_x = grabImage_x + Qx;
                            else
                                wcs_x = grabImage_x * -1 + Qx;
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy;
                            else
                                wcs_y = grabImage_y * -1 + Qy;
                            if (CaliParam.IsMoveZ)
                                Qz = grabImage_z;
                            else
                                Qz = grabImage_z * -1;
                            ////////////////////////////////////////////////////////////
                            HTuple temp_x = new HTuple(wcs_x);
                            HTuple temp_y = new HTuple(wcs_y);
                            HTuple temp_z = HTuple.TupleGenConst(wcs_x.Length, Qz);
                            if (this.DicMapHomMat2D.ContainsKey(this.MapType))
                                wcs_x = this.DicMapHomMat2D[this.MapType].GetHHomMat().AffineTransPoint2d(temp_x, temp_y, out wcs_y);
                            else
                            {
                                if (this.DicMapHomMat3D.ContainsKey(this.MapType))
                                    wcs_x = this.DicMapHomMat3D[this.MapType].GetHHomMat3D().AffineTransPoint3d(temp_x, temp_y, temp_z, out wcs_y, out wcs_z);
                                else
                                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(temp_x, temp_y, out wcs_y);
                            }
                            break;
                        case nameof(enMapMethod.PixToWcs):
                        case nameof(enMapMethod.PixToPix):
                            wcs_x = Qx;
                            wcs_y = Qy;
                            break;
                    }
                    break;
                case enCoordOriginType.标定原点: // 标定原点:以标定中心位置作为原点来计算
                    nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    nineCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX)   // 表示以相机作为参考对象 2024-01-13
                        wcs_x = Qx;   // 拍照坐标 + 计算值  caliCenter_x +
                    else
                        wcs_x = Qx;   // 拍照坐标*-1 + 计算值  caliCenter_x * -1 +
                    if (CaliParam.IsMoveY)
                        wcs_y = Qy;  //
                    else
                        wcs_y = Qy;  //
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    wcs_z = HTuple.TupleGenConst(wcs_x.Length, wcs_z.D);
                    /////////////  补偿轴移动
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x += grabImage_x - nineCenter_x + CaliParam.AdjHomMatC02X;
                    else
                        wcs_x += nineCenter_x - grabImage_x + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        wcs_y += grabImage_y - nineCenter_y + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y += nineCenter_y - grabImage_y + CaliParam.AdjHomMatC12Y;
                    break;
            }
        }
        public void ImagePointsToWorldPlane(double Rows, double Coluns, double grabImage_x, double grabImage_y, double grabImage_z, out double wcs_x, out double wcs_y, out double wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
            /////////////////////////////////////////////////////////  这个标定中心在这里没什么用了，应该使用旋转的拍照位姿 ////////////////
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.九点标定:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.UpDnCamCalibWcs:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    HTuple mapRow, mapCol;
                    mapRow = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(Rows, Coluns, out mapCol);
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(mapCol, mapRow, out Qy);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double x = 0, y = 0;
                        for (int i = 0; i < 1; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Rows, Coluns, item.Key.row, item.Key.col);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            x = crrentHomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out y);
                            Qx[i] = x;
                            Qy[i] = y;
                        }
                    }
                    else
                        Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
            }
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
                case enCoordOriginType.相机坐标:
                    wcs_x = Qx;
                    wcs_y = Qy;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                        wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                    else                             // 拍照坐标*-1 + 计算值
                        wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy.D;
                    else
                        wcs_y = grabImage_y * -1 + Qy.D;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    break;
                case enCoordOriginType.映射原点:
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = grabImage_x + Qx;   // 拍照坐标 + 计算值
                    else
                        wcs_x = grabImage_x * -1 + Qx;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy;
                    else
                        wcs_y = grabImage_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    ////////映射变换///////
                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心:
                    ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转    对位贴合场景坐标系定义必需以平台作为参考对象，往右为正，往前为正/////////////////////////
                    /////// 2024/06/20 修改: (Qx.D - CaliParam.CalibCenter.X) :表示拍照位的Mark点相对于旋转中心的坐标，(grabImage_x - CaliParam.RotateCenter.X)：表示拍照位置变化后的补偿值
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    if (CaliParam.IsMoveX)  // 拍照坐标 + 计算值 - 标定中心值 ()
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X; // CaliParam.RotateCenter.X/CaliParam.RotateCenter.Y: 表示标定位置的XY拍照坐标
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X + this.CaliParam.CamAxisIncrementX;
                    if (CaliParam.IsMoveY)
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y + this.CaliParam.CamAxisIncrementY;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z; // Math.Abs(CaliParam.CalibCenterXy.Z) + (grabImage_z + CaliParam.RotateCenter.Z);
                    else
                        wcs_z = grabImage_z * -1; // Math.Abs(CaliParam.CalibCenterXy.Z) + (CaliParam.RotateCenter.Z - grabImage_z);
                    break;
                ///////// 上下料模式，只需把中心点移动到当前点位置////////
                case enCoordOriginType.IsLoading:  // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); //
                    double nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    double nineCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX) // 相机移动的处理方式
                        wcs_x = (Qx.D + nineCenter_x) + (grabImage_x - nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;  // 相机移动方式，相当于将相机原点平移了
                    else // 相机固定的处理方式
                        wcs_x = (Qx.D + nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY) // 相机移动的处理方式
                        wcs_y = (Qy.D + nineCenter_y) + (grabImage_y - nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    else // 相机固定的处理方式
                        wcs_y = (Qy.D + nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.映射变换:
                    switch (this.MapType)
                    {
                        default:
                        case "NONE":
                        case "none":
                            if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                                wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                            else              // 拍照坐标*-1 + 计算值
                                wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy.D;
                            else
                                wcs_y = grabImage_y * -1 + Qy.D;
                            if (CaliParam.IsMoveZ)
                                wcs_z = grabImage_z;
                            else
                                wcs_z = grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.McsToMcs):
                        case nameof(enMapMethod.McsToWcs):
                        case nameof(enMapMethod.WcsToWcs):
                        case nameof(enMapMethod.相机到五轴):
                        case nameof(enMapMethod.相机到激光):
                            if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                                wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                            else              // 拍照坐标*-1 + 计算值
                                wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy.D;
                            else
                                wcs_y = grabImage_y * -1 + Qy.D;
                            if (CaliParam.IsMoveZ)
                                wcs_z = grabImage_z;
                            else
                                wcs_z = grabImage_z * -1;
                            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                            if (this.DicMapHomMat2D.ContainsKey(this.MapType))
                                wcs_x = this.DicMapHomMat2D[this.MapType].GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                            else
                            {
                                if (this.DicMapHomMat3D.ContainsKey(this.MapType))
                                    wcs_x = this.DicMapHomMat3D[this.MapType].GetHHomMat3D().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out wcs_y, out wcs_z);
                                else
                                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                            }
                            break;
                        case nameof(enMapMethod.PixToWcs):
                        case nameof(enMapMethod.PixToPix):
                            wcs_x = Qx;
                            wcs_y = Qy;
                            break;
                    }
                    break;
                case enCoordOriginType.标定原点: // 以标定点作为参考原点
                    nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    nineCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = Qx;   // 拍照坐标 + 计算值 caliCenter_x + 
                    else
                        wcs_x = Qx;   // 拍照坐标*-1 + 计算值  caliCenter_x * -1 +
                    if (CaliParam.IsMoveY)
                        wcs_y = Qy;
                    else
                        wcs_y = Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x += grabImage_x - nineCenter_x + CaliParam.AdjHomMatC02X;   // 拍照坐标 + 计算值
                    else
                        wcs_x += nineCenter_x - grabImage_x + CaliParam.AdjHomMatC02X;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y += grabImage_y - nineCenter_y + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y += nineCenter_y - grabImage_y + CaliParam.AdjHomMatC12Y;
                    break;
            }

        }

        public void ImagePointsToWorldPlane(double Rows, double Coluns, double grabImage_x, double grabImage_y, double grabImage_z, enCoordOriginType coordOriginType, string mapType, out double wcs_x, out double wcs_y, out double wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
            /////////////////////////////////////////////////////////  这个标定中心在这里没什么用了，应该使用旋转的拍照位姿 ////////////////
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.九点标定:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.UpDnCamCalibWcs:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    HTuple mapRow, mapCol;
                    mapRow = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(Rows, Coluns, out mapCol);
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(mapCol, mapRow, out Qy);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double x = 0, y = 0;
                        for (int i = 0; i < 1; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Rows, Coluns, item.Key.row, item.Key.col);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            x = crrentHomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out y);
                            Qx[i] = x;
                            Qy[i] = y;
                        }
                    }
                    else
                        Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
            }
            ////////////////////////////////////////
            switch (coordOriginType)
            {
                default:
                case enCoordOriginType.相机坐标:
                    wcs_x = Qx;
                    wcs_y = Qy;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                        wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                    else                             // 拍照坐标*-1 + 计算值
                        wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy.D;
                    else
                        wcs_y = grabImage_y * -1 + Qy.D;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    break;
                case enCoordOriginType.映射原点:
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = grabImage_x + Qx;   // 拍照坐标 + 计算值
                    else
                        wcs_x = grabImage_x * -1 + Qx;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy;
                    else
                        wcs_y = grabImage_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    ////////映射变换///////
                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心:
                    ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转    对位贴合场景坐标系定义必需以平台作为参考对象，往右为正，往前为正/////////////////////////
                    /////// 2024/06/20 修改: (Qx.D - CaliParam.CalibCenter.X) :表示拍照位的Mark点相对于旋转中心的坐标，(grabImage_x - CaliParam.RotateCenter.X)：表示拍照位置变化后的补偿值
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    if (CaliParam.IsMoveX)  // 拍照坐标 + 计算值 - 标定中心值 ()
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X; // CaliParam.RotateCenter.X/CaliParam.RotateCenter.Y: 表示标定位置的XY拍照坐标
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X + this.CaliParam.CamAxisIncrementX;
                    if (CaliParam.IsMoveY)
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y + this.CaliParam.CamAxisIncrementY;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z; // Math.Abs(CaliParam.CalibCenterXy.Z) + (grabImage_z + CaliParam.RotateCenter.Z);
                    else
                        wcs_z = grabImage_z * -1; // Math.Abs(CaliParam.CalibCenterXy.Z) + (CaliParam.RotateCenter.Z - grabImage_z);
                    break;
                ///////// 上下料模式，只需把中心点移动到当前点位置////////
                case enCoordOriginType.IsLoading:  // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); //
                    double nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    double nineCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX) // 相机移动的处理方式
                        wcs_x = (Qx.D + nineCenter_x) + (grabImage_x - nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;  // 相机移动方式，相当于将相机原点平移了
                    else // 相机固定的处理方式
                        wcs_x = (Qx.D + nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY) // 相机移动的处理方式
                        wcs_y = (Qy.D + nineCenter_y) + (grabImage_y - nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    else // 相机固定的处理方式
                        wcs_y = (Qy.D + nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.映射变换:
                    switch (mapType)
                    {
                        default:
                        case "NONE":
                        case "none":
                            if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                                wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                            else              // 拍照坐标*-1 + 计算值
                                wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy.D;
                            else
                                wcs_y = grabImage_y * -1 + Qy.D;
                            if (CaliParam.IsMoveZ)
                                wcs_z = grabImage_z;
                            else
                                wcs_z = grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.McsToMcs):
                        case nameof(enMapMethod.McsToWcs):
                        case nameof(enMapMethod.WcsToWcs):
                        case nameof(enMapMethod.相机到五轴):
                        case nameof(enMapMethod.相机到激光):
                            if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                                wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                            else              // 拍照坐标*-1 + 计算值
                                wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy.D;
                            else
                                wcs_y = grabImage_y * -1 + Qy.D;
                            if (CaliParam.IsMoveZ)
                                wcs_z = grabImage_z;
                            else
                                wcs_z = grabImage_z * -1;
                            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                            if (this.DicMapHomMat2D.ContainsKey(mapType))
                                wcs_x = this.DicMapHomMat2D[mapType].GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                            else
                            {
                                if (this.DicMapHomMat3D.ContainsKey(mapType))
                                    wcs_x = this.DicMapHomMat3D[mapType].GetHHomMat3D().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out wcs_y, out wcs_z);
                                else
                                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                            }
                            break;
                        case nameof(enMapMethod.PixToWcs):
                        case nameof(enMapMethod.PixToPix):
                            wcs_x = Qx;
                            wcs_y = Qy;
                            break;
                    }
                    break;
                case enCoordOriginType.标定原点: // 以标定点作为参考原点
                    nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    nineCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = Qx;   // 拍照坐标 + 计算值 caliCenter_x + 
                    else
                        wcs_x = Qx;   // 拍照坐标*-1 + 计算值  caliCenter_x * -1 +
                    if (CaliParam.IsMoveY)
                        wcs_y = Qy;
                    else
                        wcs_y = Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x += grabImage_x - nineCenter_x + CaliParam.AdjHomMatC02X;
                    else
                        wcs_x += nineCenter_x - grabImage_x + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        wcs_y += grabImage_y - nineCenter_y + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y += nineCenter_y - grabImage_y + CaliParam.AdjHomMatC12Y;
                    break;
            }

        }
        public void ImageCenterPointsToWorldPlane(double grabImage_x, double grabImage_y, double grabImage_z, out double wcs_x, out double wcs_y, out double wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
            /////////////////////////////////////////////////////////  这个标定中心在这里没什么用了，应该使用旋转的拍照位姿 ////////////////
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.九点标定:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(this.DataWidth * 0.5, this.DataHeight * 0.5, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), this.DataHeight * 0.5, this.DataWidth * 0.5, 1, out Qx, out Qy);
                    break;
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.UpDnCamCalibWcs:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(this.DataWidth * 0.5, this.DataHeight * 0.5, out Qy);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    HTuple mapRow, mapCol;
                    mapRow = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(this.DataHeight * 0.5, this.DataWidth * 0.5, out mapCol);
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(mapCol, mapRow, out Qy);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double x = 0, y = 0;
                        for (int i = 0; i < 1; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(this.DataHeight * 0.5, this.DataWidth * 0.5, item.Key.row, item.Key.col);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            x = crrentHomMat2D.GetHHomMat().AffineTransPoint2d(this.DataWidth * 0.5, this.DataHeight * 0.5, out y);
                            Qx[i] = x;
                            Qy[i] = y;
                        }
                    }
                    else
                        Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(this.DataWidth * 0.5, this.DataHeight * 0.5, out Qy);
                    break;
            }
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
                case enCoordOriginType.相机坐标:
                    wcs_x = Qx;
                    wcs_y = Qy;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                        wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                    else              // 拍照坐标*-1 + 计算值
                        wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy.D;
                    else
                        wcs_y = grabImage_y * -1 + Qy.D;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    break;
                case enCoordOriginType.映射原点:
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = grabImage_x + Qx;   // 拍照坐标 + 计算值
                    else
                        wcs_x = grabImage_x * -1 + Qx;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y = grabImage_y + Qy;
                    else
                        wcs_y = grabImage_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    ////////映射变换///////
                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心:
                    ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转    对位贴合场景坐标系定义必需以平台作为参考对象，往右为正，往前为正/////////////////////////
                    if (CaliParam.IsMoveX)  // 拍照坐标 + 计算值 - 标定中心值 ()
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X;   // CaliParam.RotateCenter.X/CaliParam.RotateCenter.Y: 表示标定位置的XY拍照坐标
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X + this.CaliParam.CamAxisIncrementX;
                    if (CaliParam.IsMoveY)
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y + this.CaliParam.CamAxisIncrementY;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;  //Math.Abs(CaliParam.CalibCenterXy.Z) + (grabImage_z + CaliParam.RotateCenter.Z);
                    else
                        wcs_z = grabImage_z * -1;  //Math.Abs(CaliParam.CalibCenterXy.Z) + (CaliParam.RotateCenter.Z - grabImage_z);
                    break;
                ///////// 上下料模式，只需把中心点移动到当前点位置////////
                case enCoordOriginType.IsLoading:  // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    double nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    if (CaliParam.IsMoveX) // 相机移动的处理方式
                        wcs_x = (Qx.D + nineCenter_x) + (grabImage_x - nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;  // 相机移动方式，相当于将相机原点平移了
                    else // 相机固定的处理方式
                        wcs_x = (Qx.D + nineCenter_x) + (0 - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY) // 相机移动的处理方式
                        wcs_y = (Qy.D + nineCenter_y) + (grabImage_y - nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    else // 相机固定的处理方式
                        wcs_y = (Qy.D + nineCenter_y) + (0 - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.映射变换:
                    switch (this.MapType)
                    {
                        default:
                        case "NONE":
                        case "none":
                            if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                                wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                            else              // 拍照坐标*-1 + 计算值
                                wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy.D;
                            else
                                wcs_y = grabImage_y * -1 + Qy.D;
                            if (CaliParam.IsMoveZ)
                                wcs_z = grabImage_z;
                            else
                                wcs_z = grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.McsToMcs):
                        case nameof(enMapMethod.McsToWcs):
                        case nameof(enMapMethod.WcsToWcs):
                        case nameof(enMapMethod.相机到五轴):
                        case nameof(enMapMethod.相机到激光):
                            if (CaliParam.IsMoveX) // 拍照坐标 + 计算值
                                wcs_x = grabImage_x + Qx.D;  // 相机移动的计算方式
                            else              // 拍照坐标*-1 + 计算值
                                wcs_x = grabImage_x * -1 + Qx.D;  // 平台移动的计算方式
                            if (CaliParam.IsMoveY)
                                wcs_y = grabImage_y + Qy.D;
                            else
                                wcs_y = grabImage_y * -1 + Qy.D;
                            if (CaliParam.IsMoveZ)
                                wcs_z = grabImage_z;
                            else
                                wcs_z = grabImage_z * -1;
                            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                            if (this.DicMapHomMat2D.ContainsKey(this.MapType))
                                wcs_x = this.DicMapHomMat2D[this.MapType].GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                            else
                            {
                                if (this.DicMapHomMat3D.ContainsKey(this.MapType))
                                    wcs_x = this.DicMapHomMat3D[this.MapType].GetHHomMat3D().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out wcs_y, out wcs_z);
                                else
                                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                            }
                            break;
                        case nameof(enMapMethod.PixToWcs):
                        case nameof(enMapMethod.PixToPix):
                            wcs_x = Qx;
                            wcs_y = Qy;
                            break;
                    }
                    break;
                case enCoordOriginType.标定原点:
                    nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    //caliCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = Qx; //caliCenter_x +
                    else
                        wcs_x = Qx; //caliCenter_x * -1 +
                    if (CaliParam.IsMoveY)
                        wcs_y = Qy;
                    else
                        wcs_y = Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x += grabImage_x - nineCenter_x + CaliParam.AdjHomMatC02X;   // 拍照坐标 + 计算值
                    else
                        wcs_x += nineCenter_x - grabImage_x + CaliParam.AdjHomMatC02X;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y += grabImage_y - nineCenter_y + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y += nineCenter_y - grabImage_y + CaliParam.AdjHomMatC12Y;
                    break;
            }
        }

        public void WorldPointsToImagePlane(HTuple wcs_x, HTuple wcs_y, HTuple wcs_z, double grabImage_x, double grabImage_y, double grabImage_z, out HTuple Rows, out HTuple Coluns)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            Rows = 0;
            Coluns = 0;
            if (wcs_x == null)
            {
                throw new ArgumentNullException("wcs_x");
            }
            if (wcs_y == null)
            {
                throw new ArgumentNullException("wcs_y");
            }
            if (wcs_z == null)
            {
                throw new ArgumentNullException("wcs_z");
            }
            if (wcs_x.Length != wcs_y.Length)
            {
                throw new ArgumentException("参wcs_x与wcs_y长度不相等");
            }
            /////////////////////////////////////////////////////////
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            HTuple temp_x = new HTuple(wcs_x);
            HTuple temp_y = new HTuple(wcs_y);
            HTuple temp_z = new HTuple(wcs_z);
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
                case enCoordOriginType.相机坐标:
                    Qx = wcs_x;
                    Qy = wcs_y;
                    Qz = wcs_z;
                    break;
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - grabImage_x;
                    else
                        Qx = temp_x - grabImage_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - grabImage_y;
                    else
                        Qy = temp_y - grabImage_y * -1;
                    if (CaliParam.IsMoveZ)
                        Qz = temp_z - grabImage_z;
                    else
                        Qz = temp_z - grabImage_z * -1;
                    break;
                case enCoordOriginType.映射原点:
                    ////////映射变换///////
                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(temp_x, temp_y, out temp_y);
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - grabImage_x;
                    else
                        Qx = temp_x - grabImage_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - grabImage_y;
                    else
                        Qy = temp_y - grabImage_y * -1;
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心，用于9点标模型
                case enCoordOriginType.旋转中心: // 相对坐标与相机的移动无关
                    if (CaliParam.IsMoveX) // 拍照坐标 + 计算值 - 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (grabImage_x - CaliParam.RotateCalibPoint.X) - CaliParam.AdjHomMatC02X;
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (CaliParam.RotateCalibPoint.X - grabImage_x) - CaliParam.AdjHomMatC02X - this.CaliParam.CamAxisIncrementX;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (grabImage_y - CaliParam.RotateCalibPoint.Y) - CaliParam.AdjHomMatC12Y;
                    else
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (CaliParam.RotateCalibPoint.Y - grabImage_y) - CaliParam.AdjHomMatC12Y - this.CaliParam.CamAxisIncrementY;
                    break;
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    double nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    if (CaliParam.IsMoveX) // 相机移动的处理方式
                        Qx = temp_x - nineCenter_x - (grabImage_x - nineCenter_x) - (0 - CaliParam.CalibCenterXy.X) - CaliParam.AdjHomMatC02X;
                    else // 相机固定的处理方式
                        Qx = temp_x - nineCenter_x - (0 - CaliParam.CalibCenterXy.X) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY) // 相机移动的处理方式
                        Qy = temp_y - nineCenter_y - (grabImage_y - nineCenter_y) - (0 - CaliParam.CalibCenterXy.Y) - CaliParam.AdjHomMatC12Y;
                    else // 相机固定的处理方式
                        Qy = temp_y - nineCenter_y - (0 - CaliParam.CalibCenterXy.Y) - CaliParam.AdjHomMatC12Y;
                    break;
                case enCoordOriginType.映射变换:
                    switch (this.MapType)
                    {
                        default:
                        case "NONE":
                        case "none":
                            /// 转换到相机坐标系中
                            if (CaliParam.IsMoveX)
                                Qx = temp_x - grabImage_x;
                            else
                                Qx = temp_x - grabImage_x * -1;
                            if (CaliParam.IsMoveY)
                                Qy = temp_y - grabImage_y;
                            else
                                Qy = temp_y - grabImage_y * -1;
                            if (CaliParam.IsMoveZ)
                                Qz = temp_z - grabImage_z;
                            else
                                Qz = temp_z - grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.McsToMcs):
                        case nameof(enMapMethod.McsToWcs):
                        case nameof(enMapMethod.WcsToWcs):
                        case nameof(enMapMethod.相机到五轴):
                        case nameof(enMapMethod.相机到激光):
                            /// 反向映射变换
                            if (this.DicMapHomMat2D.ContainsKey(this.MapType))
                                temp_x = this.DicMapHomMat2D[this.MapType].GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
                            else
                            {
                                if (this.DicMapHomMat3D.ContainsKey(this.MapType))
                                    temp_x = this.DicMapHomMat3D[this.MapType].GetHHomMat3D().HomMat3dInvert().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out temp_y, out temp_z);
                                else
                                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
                            }
                            /// 转换到相机坐标系中
                            if (CaliParam.IsMoveX)
                                Qx = temp_x - grabImage_x;
                            else
                                Qx = temp_x - grabImage_x * -1;
                            if (CaliParam.IsMoveY)
                                Qy = temp_y - grabImage_y;
                            else
                                Qy = temp_y - grabImage_y * -1;
                            if (CaliParam.IsMoveZ)
                                Qz = temp_z - grabImage_z;
                            else
                                Qz = temp_z - grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.PixToWcs):
                        case nameof(enMapMethod.PixToPix):
                            Qx = wcs_x;
                            Qy = wcs_y;
                            break;
                    }
                    break;
                case enCoordOriginType.标定原点:
                    nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX)
                        temp_x = wcs_x - (grabImage_x - nineCenter_x) - CaliParam.AdjHomMatC02X;
                    else
                        temp_x = wcs_x - (nineCenter_x - grabImage_x) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        temp_y = wcs_y - (grabImage_y - nineCenter_y) - CaliParam.AdjHomMatC12Y;
                    else
                        temp_y = wcs_y - (nineCenter_y - grabImage_y) - CaliParam.AdjHomMatC12Y;
                    ///// 转换到相机坐标系中
                    if (CaliParam.IsMoveX) //
                        Qx = temp_x; // - caliCenter_x;
                    else
                        Qx = temp_x; // - caliCenter_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y;
                    else
                        Qy = temp_y;
                    break;
            }
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.九点标定:
                default:
                    Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
                ////////////////////////////////////////////////
                case enCamCaliModel.CamParamPose:
                    // 世界点到图像点
                    HTuple ProjMat, Cam_x, Cam_y, Cam_z;
                    HOperatorSet.TupleGenConst(Qx.Length, 0, out Qz);
                    if (this.CamParam.Kappa == 0)
                    {
                        HOperatorSet.CamParPoseToHomMat3d(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), out ProjMat);
                        HOperatorSet.ProjectPointHomMat3d(ProjMat, Qx, Qy, Qz, out Coluns, out Rows);
                    }
                    else
                    {
                        HOperatorSet.PoseToHomMat3d(this.CamPose.GetHtuple(), out ProjMat);
                        HOperatorSet.AffineTransPoint3d(ProjMat, Qx, Qy, Qz, out Cam_x, out Cam_y, out Cam_z);
                        HOperatorSet.Project3dPoint(Cam_x, Cam_y, Cam_z, this.CamParam.GetHtuple(), out Rows, out Coluns);
                    }
                    break;
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.UpDnCamCalibWcs:
                    Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    HOperatorSet.TupleGenConst(Qx.Length, 0, out Qz);
                    HTuple mapRow, mapCol;
                    mapCol = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out mapRow);
                    Rows = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(mapRow, mapCol, out Coluns);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double row = 0, col = 0;
                        Rows = new HTuple();
                        Coluns = new HTuple();
                        for (int i = 0; i < Qx.Length; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Qx[i], Qy[i], item.Key.x, item.Key.y);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            col = crrentHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out row);
                            Rows[i] = row;
                            Coluns[i] = col;
                        }
                    }
                    else
                        Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
            }

        }
        public void WorldPointsToImagePlane(double wcs_x, double wcs_y, double wcs_z, double grabImage_x, double grabImage_y, double grabImage_z, out double Rows, out double Coluns)
        {
            double Qx, Qy, Qz;
            Rows = 0;
            Coluns = 0;
            /////////////////////////////////////////////////////////);
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            double temp_x = wcs_x;
            double temp_y = wcs_y;
            double temp_z = wcs_z;
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
                case enCoordOriginType.相机坐标:
                    Qx = wcs_x;
                    Qy = wcs_y;
                    Qz = wcs_z;
                    break;
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - grabImage_x;
                    else
                        Qx = temp_x - grabImage_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - grabImage_y;
                    else
                        Qy = temp_y - grabImage_y * -1;
                    if (CaliParam.IsMoveZ)
                        Qz = temp_z - grabImage_z;
                    else
                        Qz = temp_z - grabImage_z * -1;
                    break;
                case enCoordOriginType.映射原点:
                    ////////映射变换///////
                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(temp_x, temp_y, out temp_y);
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - grabImage_x;
                    else
                        Qx = temp_x - grabImage_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - grabImage_y;
                    else
                        Qy = temp_y - grabImage_y * -1;
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心: // 相对坐标与相机的移动无关
                                             ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转 /////////////////////////
                    if (CaliParam.IsMoveX) // 拍照坐标 + 计算值 - 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (grabImage_x - CaliParam.RotateCalibPoint.X) - CaliParam.AdjHomMatC02X;
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (CaliParam.RotateCalibPoint.X - grabImage_x) - CaliParam.AdjHomMatC02X - this.CaliParam.CamAxisIncrementX;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (grabImage_y - CaliParam.RotateCalibPoint.Y) - CaliParam.AdjHomMatC12Y;
                    else
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (CaliParam.RotateCalibPoint.Y - grabImage_y) - CaliParam.AdjHomMatC12Y - this.CaliParam.CamAxisIncrementY;
                    break;
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    double nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    if (CaliParam.IsMoveX) // 相机移动的处理方式
                        Qx = temp_x - nineCenter_x - (grabImage_x - nineCenter_x) - (0 - CaliParam.CalibCenterXy.X) - CaliParam.AdjHomMatC02X;
                    else // 相机固定的处理方式
                        Qx = temp_x - nineCenter_x - (0 - CaliParam.CalibCenterXy.X) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY) // 相机移动的处理方式
                        Qy = temp_y - nineCenter_y - (grabImage_y - nineCenter_y) - (0 - CaliParam.CalibCenterXy.Y) - CaliParam.AdjHomMatC12Y;
                    else // 相机固定的处理方式
                        Qy = temp_y - nineCenter_y - (0 - CaliParam.CalibCenterXy.Y) - CaliParam.AdjHomMatC12Y;
                    break;
                case enCoordOriginType.映射变换:
                    switch (this.MapType)
                    {
                        default:
                        case "NONE":
                        case "none":
                            ///////////////// 转换到相机坐标系中 ////////////////////////////
                            if (CaliParam.IsMoveX)
                                Qx = temp_x - grabImage_x;
                            else
                                Qx = temp_x - grabImage_x * -1;
                            if (CaliParam.IsMoveY)
                                Qy = temp_y - grabImage_y;
                            else
                                Qy = temp_y - grabImage_y * -1;
                            if (CaliParam.IsMoveY)
                                Qz = temp_z - grabImage_z;
                            else
                                Qz = temp_z - grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.McsToMcs):
                        case nameof(enMapMethod.McsToWcs):
                        case nameof(enMapMethod.WcsToWcs):
                        case nameof(enMapMethod.相机到五轴):
                        case nameof(enMapMethod.相机到激光):
                            /// 反向映射变换
                            if (this.DicMapHomMat2D.ContainsKey(this.MapType))
                                temp_x = this.DicMapHomMat2D[this.MapType].GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
                            else
                            {
                                if (this.DicMapHomMat3D.ContainsKey(this.MapType))
                                    temp_x = this.DicMapHomMat3D[this.MapType].GetHHomMat3D().HomMat3dInvert().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out temp_y, out temp_z);
                                else
                                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
                            }
                            ///////////////// 转换到相机坐标系中 ////////////////////////////
                            if (CaliParam.IsMoveX)
                                Qx = temp_x - grabImage_x;
                            else
                                Qx = temp_x - grabImage_x * -1;
                            if (CaliParam.IsMoveY)
                                Qy = temp_y - grabImage_y;
                            else
                                Qy = temp_y - grabImage_y * -1;
                            if (CaliParam.IsMoveY)
                                Qz = temp_z - grabImage_z;
                            else
                                Qz = temp_z - grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.PixToWcs):
                        case nameof(enMapMethod.PixToPix):
                            Qx = wcs_x;
                            Qy = wcs_y;
                            break;
                    }
                    break;
                case enCoordOriginType.标定原点:
                    nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX)
                        temp_x = wcs_x - (grabImage_x - nineCenter_x) - CaliParam.AdjHomMatC02X;
                    else
                        temp_x = wcs_x - (nineCenter_x - grabImage_x) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        temp_y = wcs_y - (grabImage_y - nineCenter_y) - CaliParam.AdjHomMatC12Y;
                    else
                        temp_y = wcs_y - (nineCenter_y - grabImage_y) - CaliParam.AdjHomMatC12Y;
                    ///////////////// 转换到相机坐标系中 ////////////////////////////
                    if (CaliParam.IsMoveX)
                        Qx = temp_x;// - caliCenter_x;
                    else
                        Qx = temp_x;// - caliCenter_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y;
                    else
                        Qy = temp_y;
                    break;
            }
            ///////////////////////////////////// 
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.九点标定:
                default:
                    Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
                case enCamCaliModel.CamParamPose:
                    // 世界点到图像点
                    HTuple ProjMat, Cam_x, Cam_y, Cam_z, row = 0, col = 0;
                    if (this.CamParam != null && this.CamPose != null)
                    {
                        if (this.CamParam?.Kappa == 0)
                        {
                            HOperatorSet.CamParPoseToHomMat3d(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), out ProjMat);
                            HOperatorSet.ProjectPointHomMat3d(ProjMat, Qx, Qy, 0, out col, out row);
                        }
                        else
                        {
                            HOperatorSet.PoseToHomMat3d(this.CamPose.GetHtuple(), out ProjMat);
                            HOperatorSet.AffineTransPoint3d(ProjMat, Qx, Qy, 0, out Cam_x, out Cam_y, out Cam_z);
                            HOperatorSet.Project3dPoint(Cam_x, Cam_y, Cam_z, this.CamParam.GetHtuple(), out row, out col);
                        }
                    }
                    Rows = row.D;
                    Coluns = col.D;
                    break;
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.UpDnCamCalibWcs:
                    Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    Qz = 0;
                    HTuple mapRow, mapCol;
                    mapCol = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out mapRow);
                    Rows = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(mapRow.D, mapCol.D, out Coluns);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        for (int i = 0; i < 1; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Qx, Qy, item.Key.x, item.Key.y);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            Coluns = crrentHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                        }
                    }
                    else
                        Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
            }

        }

        public void WorldPointsToImagePlane(double wcs_x, double wcs_y, double wcs_z, double grabImage_x, double grabImage_y, double grabImage_z, enCoordOriginType coordOriginType, string mapType, out double Rows, out double Coluns)
        {
            double Qx, Qy, Qz;
            Rows = 0;
            Coluns = 0;
            /////////////////////////////////////////////////////////);
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            double temp_x = wcs_x;
            double temp_y = wcs_y;
            double temp_z = wcs_z;
            ////////////////////////////////////////
            switch (coordOriginType)
            {
                default:
                case enCoordOriginType.相机坐标:
                    Qx = wcs_x;
                    Qy = wcs_y;
                    Qz = wcs_z;
                    break;
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - grabImage_x;
                    else
                        Qx = temp_x - grabImage_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - grabImage_y;
                    else
                        Qy = temp_y - grabImage_y * -1;
                    if (CaliParam.IsMoveZ)
                        Qz = temp_z - grabImage_z;
                    else
                        Qz = temp_z - grabImage_z * -1;
                    break;
                case enCoordOriginType.映射原点:
                    ////////映射变换///////
                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(temp_x, temp_y, out temp_y);
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - grabImage_x;
                    else
                        Qx = temp_x - grabImage_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - grabImage_y;
                    else
                        Qy = temp_y - grabImage_y * -1;
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心: // 相对坐标与相机的移动无关
                                             ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转 /////////////////////////
                    if (CaliParam.IsMoveX) // 拍照坐标 + 计算值 - 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (grabImage_x - CaliParam.RotateCalibPoint.X) - CaliParam.AdjHomMatC02X;
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (CaliParam.RotateCalibPoint.X - grabImage_x) - CaliParam.AdjHomMatC02X - this.CaliParam.CamAxisIncrementX;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (grabImage_y - CaliParam.RotateCalibPoint.Y) - CaliParam.AdjHomMatC12Y;
                    else
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (CaliParam.RotateCalibPoint.Y - grabImage_y) - CaliParam.AdjHomMatC12Y - this.CaliParam.CamAxisIncrementY;
                    break;
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    double nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    if (CaliParam.IsMoveX) // 相机移动的处理方式
                        Qx = temp_x - nineCenter_x - (grabImage_x - nineCenter_x) - (0 - CaliParam.CalibCenterXy.X) - CaliParam.AdjHomMatC02X;
                    else // 相机固定的处理方式
                        Qx = temp_x - nineCenter_x - (0 - CaliParam.CalibCenterXy.X) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY) // 相机移动的处理方式
                        Qy = temp_y - nineCenter_y - (grabImage_y - nineCenter_y) - (0 - CaliParam.CalibCenterXy.Y) - CaliParam.AdjHomMatC12Y;
                    else // 相机固定的处理方式
                        Qy = temp_y - nineCenter_y - (0 - CaliParam.CalibCenterXy.Y) - CaliParam.AdjHomMatC12Y;
                    break;
                case enCoordOriginType.映射变换:
                    switch (mapType)
                    {
                        default:
                        case "NONE":
                        case "none":
                            ///////////////// 转换到相机坐标系中 ////////////////////////////
                            if (CaliParam.IsMoveX)
                                Qx = temp_x - grabImage_x;
                            else
                                Qx = temp_x - grabImage_x * -1;
                            if (CaliParam.IsMoveY)
                                Qy = temp_y - grabImage_y;
                            else
                                Qy = temp_y - grabImage_y * -1;
                            if (CaliParam.IsMoveY)
                                Qz = temp_z - grabImage_z;
                            else
                                Qz = temp_z - grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.McsToMcs):
                        case nameof(enMapMethod.McsToWcs):
                        case nameof(enMapMethod.WcsToWcs):
                        case nameof(enMapMethod.相机到五轴):
                        case nameof(enMapMethod.相机到激光):
                            /// 反向映射变换
                            if (this.DicMapHomMat2D.ContainsKey(mapType))
                                temp_x = this.DicMapHomMat2D[mapType].GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
                            else
                            {
                                if (this.DicMapHomMat3D.ContainsKey(mapType))
                                    temp_x = this.DicMapHomMat3D[mapType].GetHHomMat3D().HomMat3dInvert().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out temp_y, out temp_z);
                                else
                                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
                            }
                            ///////////////// 转换到相机坐标系中 ////////////////////////////
                            if (CaliParam.IsMoveX)
                                Qx = temp_x - grabImage_x;
                            else
                                Qx = temp_x - grabImage_x * -1;
                            if (CaliParam.IsMoveY)
                                Qy = temp_y - grabImage_y;
                            else
                                Qy = temp_y - grabImage_y * -1;
                            if (CaliParam.IsMoveY)
                                Qz = temp_z - grabImage_z;
                            else
                                Qz = temp_z - grabImage_z * -1;
                            break;
                        case nameof(enMapMethod.PixToWcs):
                        case nameof(enMapMethod.PixToPix):
                            Qx = wcs_x;
                            Qy = wcs_y;
                            break;
                    }
                    break;
                case enCoordOriginType.标定原点:
                    nineCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    nineCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX)
                        temp_x = wcs_x - (grabImage_x - nineCenter_x) - CaliParam.AdjHomMatC02X;
                    else
                        temp_x = wcs_x - (nineCenter_x - grabImage_x) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        temp_y = wcs_y - (grabImage_y - nineCenter_y) - CaliParam.AdjHomMatC12Y;
                    else
                        temp_y = wcs_y - (nineCenter_y - grabImage_y) - CaliParam.AdjHomMatC12Y;
                    ///////////////// 转换到相机坐标系中 ////////////////////////////
                    if (CaliParam.IsMoveX)
                        Qx = temp_x;// - caliCenter_x;
                    else
                        Qx = temp_x;// - caliCenter_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y;
                    else
                        Qy = temp_y;
                    break;
            }
            ///////////////////////////////////// 
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.九点标定:
                default:
                    Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
                case enCamCaliModel.CamParamPose:
                    // 世界点到图像点
                    HTuple ProjMat, Cam_x, Cam_y, Cam_z, row = 0, col = 0;
                    if (this.CamParam != null && this.CamPose != null)
                    {
                        if (this.CamParam?.Kappa == 0)
                        {
                            HOperatorSet.CamParPoseToHomMat3d(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), out ProjMat);
                            HOperatorSet.ProjectPointHomMat3d(ProjMat, Qx, Qy, 0, out col, out row);
                        }
                        else
                        {
                            HOperatorSet.PoseToHomMat3d(this.CamPose.GetHtuple(), out ProjMat);
                            HOperatorSet.AffineTransPoint3d(ProjMat, Qx, Qy, 0, out Cam_x, out Cam_y, out Cam_z);
                            HOperatorSet.Project3dPoint(Cam_x, Cam_y, Cam_z, this.CamParam.GetHtuple(), out row, out col);
                        }
                    }
                    Rows = row.D;
                    Coluns = col.D;
                    break;
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.UpDnCamCalibWcs:
                    Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    Qz = 0;
                    HTuple mapRow, mapCol;
                    mapCol = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out mapRow);
                    Rows = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(mapRow.D, mapCol.D, out Coluns);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        for (int i = 0; i < 1; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Qx, Qy, item.Key.x, item.Key.y);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            Coluns = crrentHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                        }
                    }
                    else
                        Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
            }

        }

        public double TransPixLengthToWcsLength(double pixLength)
        {
            HTuple Qx, Qy;
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCalibWcs:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.九点标定:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(new HTuple(0, pixLength), new HTuple(0, 0), out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), new HTuple(0, 0), new HTuple(0, pixLength), 1, out Qx, out Qy);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    HTuple mapRow, mapCol;
                    mapRow = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(new HTuple(0, pixLength), new HTuple(0, 0), out mapCol);
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(mapCol, mapRow, out Qy);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double x = 0, y = 0;
                        Qx = new HTuple();
                        Qy = new HTuple();
                        HTuple Rows = new HTuple(this.DataHeight * 0.5, this.DataHeight * 0.5 + pixLength);
                        HTuple Coluns = new HTuple(this.DataWidth * 0.5, this.DataWidth * 0.5);
                        for (int i = 0; i < Rows.Length; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Rows[i].D, Coluns[i].D, item.Key.row, item.Key.col);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            x = crrentHomMat2D.GetHHomMat().AffineTransPoint2d(Coluns[i].D, Rows[i].D, out y);
                            Qx[i] = x;
                            Qy[i] = y;
                        }
                    }
                    else
                        Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(new HTuple(0, pixLength), new HTuple(0, 0), out Qy);
                    break;
            }
            double length = HMisc.DistancePp(Qy[0].D, Qx[0].D, Qy[1].D, Qx[1].D);
            return length;
        }

        public double TransWcsLengthToPixLength(double wcsLength)
        {
            HTuple ProjMat = null;
            HTuple Rows = null;
            HTuple Columns = null;
            HTuple Qx, Qy, Qz, pix_Length;
            /////////////////////////////////////
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCalibWcs:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.九点标定:
                default:
                    Columns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(new HTuple(0.0, 0.0), new HTuple(0.0, wcsLength), out Rows);
                    break;
                case enCamCaliModel.CamParamPose:
                    if (this.CamParam.Kappa == 0)
                    {
                        HOperatorSet.CamParPoseToHomMat3d(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), out ProjMat);
                        HOperatorSet.ProjectPointHomMat3d(ProjMat, new HTuple(0.0, 0.0), new HTuple(0.0, wcsLength), new HTuple(0.0, 0.0), out Columns, out Rows);
                    }
                    else
                    {
                        HOperatorSet.PoseToHomMat3d(this.CamPose.GetHtuple(), out ProjMat);
                        HOperatorSet.AffineTransPoint3d(ProjMat, new HTuple(0.0, 0.0), new HTuple(0.0, wcsLength), new HTuple(0.0, 0.0), out Qx, out Qy, out Qz);
                        HOperatorSet.Project3dPoint(Qx, Qy, Qz, this.CamParam.GetHtuple(), out Rows, out Columns);
                    }
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    //////////////////////////////
                    HTuple mapRow, mapCol;
                    mapCol = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(new HTuple(0.0, 0.0), new HTuple(0.0, wcsLength), out mapRow);
                    Rows = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(mapRow, mapCol, out Columns);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        Rows = new HTuple();
                        Columns = new HTuple();
                        double row = 0, col = 0;
                        HTuple x = new HTuple(0, wcsLength);
                        HTuple y = new HTuple(0, 0);
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        for (int i = 0; i < x.Length; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(x[i], y[i], item.Key.x, item.Key.y);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            col = crrentHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(x[i], y[i], out row);
                            Rows[i] = row;
                            Columns[i] = col;
                        }
                    }
                    else
                        Columns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(new HTuple(0.0, 0.0), new HTuple(0.0, wcsLength), out Rows);
                    break;
            }
            HOperatorSet.DistancePp(Rows[0], Columns[0], Rows[1], Columns[1], out pix_Length);
            ///////////////////////////////////////
            switch (pix_Length.Type)
            {
                case HTupleType.DOUBLE:
                    return pix_Length.D;
                case HTupleType.INTEGER:
                    return Convert.ToDouble(pix_Length.I);
                case HTupleType.LONG:
                    return Convert.ToDouble(pix_Length.L);
                case HTupleType.MIXED:
                    return Convert.ToDouble(pix_Length.O);
                case HTupleType.EMPTY:
                case HTupleType.STRING:
                default:
                    return 0;
            }
        }


        /// <summary>
        /// 将世界坐标系下的点转换到相机坐标系下
        /// </summary>
        /// <param name="wcs_x"></param>
        /// <param name="wcs_y"></param>
        /// <param name="grabImage_x"></param>
        /// <param name="grabImage_y"></param>
        /// <param name="grabImage_z"></param>
        /// <param name="cam_x"></param>
        /// <param name="cam_y"></param>
        /// <param name="cam_z"></param>
        public void TransWcsPointToCamPoint(double wcs_x, double wcs_y, double grabImage_x, double grabImage_y, double grabImage_z, out double cam_x, out double cam_y, out double cam_z)
        {
            double Rows, Coluns;
            HTuple Qx = new HTuple(), Qy = new HTuple();
            this.WorldPointsToImagePlane(wcs_x, wcs_y, 0, grabImage_x, grabImage_y, 0, out Rows, out Coluns);
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCalibWcs:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.九点标定:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    ///////////////////////////////////
                    HTuple mapRow, mapCol;
                    mapRow = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(Rows, Coluns, out mapCol);
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(mapCol, mapRow, out Qy);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double x = 0, y = 0;
                        for (int i = 0; i < 1; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Rows, Coluns, item.Key.row, item.Key.col);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            x = crrentHomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out y);
                            Qx[i] = x;
                            Qy[i] = y;
                        }
                    }
                    else
                        Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
            }
            cam_x = Qx.D;
            cam_y = Qy.D;
            cam_z = grabImage_z;
        }

        /// <summary>
        /// 将世界坐标系下的点转换到相机坐标系下
        /// </summary>
        /// <param name="wcs_x"></param>
        /// <param name="wcs_y"></param>
        /// <param name="grabImage_x"></param>
        /// <param name="grabImage_y"></param>
        /// <param name="grabImage_z"></param>
        /// <param name="cam_x"></param>
        /// <param name="cam_y"></param>
        /// <param name="cam_z"></param>
        public void TransWcsPointToCamPoint(HTuple wcs_x, HTuple wcs_y, double grabImage_x, double grabImage_y, double grabImage_z, out HTuple cam_x, out HTuple cam_y, out HTuple cam_z)
        {
            HTuple Rows = new HTuple(), Coluns = new HTuple(), Qx = new HTuple(), Qy = new HTuple();
            this.WorldPointsToImagePlane(wcs_x, wcs_y, 0, grabImage_x, grabImage_y, 0, out Rows, out Coluns);
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCalibWcs:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                case enCamCaliModel.映射标定_世界:
                case enCamCaliModel.九点标定:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
                case enCamCaliModel.映射标定_像素:
                case enCamCaliModel.UpDnCamCalibPix:
                    /// 更新标定参数用旋转中心标定参数
                    if (DicSensorParam.ContainsKey(this.CaliParam.MapCamName))
                    {
                        this.HomMat2D = DicSensorParam[this.CaliParam.MapCamName].HomMat2D;
                        this.CaliParam.CalibCenterXy = DicSensorParam[this.CaliParam.MapCamName].CaliParam.CalibCenterXy;
                        this.CaliParam.AdjHomMatC02X = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC02X;
                        this.CaliParam.AdjHomMatC12Y = DicSensorParam[this.CaliParam.MapCamName].CaliParam.AdjHomMatC12Y;
                    }
                    ///////////////////////////////////
                    HTuple mapRow, mapCol;
                    mapRow = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(Rows, Coluns, out mapCol);
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(mapCol, mapRow, out Qy);
                    break;
                case enCamCaliModel.FaceCalib: // 面阵标定
                    if (this.DicCalibHomMat2D != null)
                    {
                        double dist = double.MaxValue;
                        UserHomMat2D crrentHomMat2D = new UserHomMat2D();
                        double x = 0, y = 0;
                        for (int i = 0; i < Rows.Length; i++)
                        {
                            dist = double.MaxValue;
                            foreach (KeyValuePair<CoordValuePairs, UserHomMat2D> item in this.DicCalibHomMat2D)
                            {
                                double distTemp = HMisc.DistancePp(Rows[i].D, Coluns[i].D, item.Key.row, item.Key.col);
                                if (distTemp < dist)
                                {
                                    dist = distTemp;
                                    crrentHomMat2D = item.Value;
                                }
                            }
                            x = crrentHomMat2D.GetHHomMat().AffineTransPoint2d(Coluns[i].D, Rows[i].D, out y);
                            Qx[i] = x;
                            Qy[i] = y;
                            //Qz[i] = 0;
                        }
                    }
                    else
                        Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
            }
            cam_x = Qx;
            cam_y = Qy;
            cam_z = HTuple.TupleGenConst(Qx.Length, grabImage_z);
        }

        public static void HHomMat2DToCamparaCamPos(HHomMat2D hHomMat2D, int imageWidth, int imageHeight, double Magnification, out userCamParam camParam, out userCamPose camPose, out double Quality)
        {
            camParam = new userCamParam();
            camPose = new userCamPose();
            Quality = -1;
            if (hHomMat2D == null) throw new ArgumentNullException("hHomMat2D");
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            camParam.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
            camParam.Focus = Magnification; // 这个值应该为镜头的倍率
            camParam.Sx = Math.Abs(Sx);
            camParam.Sy = Math.Abs(Sy);
            camParam.Cx = imageWidth * 0.5;
            camParam.Cy = imageHeight * 0.5;
            camParam.Width = imageWidth;
            camParam.Height = imageHeight;
            // 计算位姿
            HTuple Qx, Qy, Quality2;
            HTuple Rows = new HTuple(0, imageHeight, imageHeight, 0);
            HTuple Columns = new HTuple(0, 0, imageWidth, imageWidth);
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, new HCamPar(camParam.GetHtuple()), "telecentric_planar_robust", "error", out Quality2);
            Quality = Quality2[0].D;
            camPose = new userCamPose(pose);
        }


        public virtual bool Save(bool isSaveMapImage = false)
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            if (this.SensorName != this.CaliParam.CamName)
                this.CaliParam.CamName = this.SensorName;
            IsOk = IsOk && XML<CameraParam>.Save(this, SavePath + @"\" + this.SensorName + ".xml");
            if (this.CalibrateFile != null)
                IsOk = IsOk && this.CalibrateFile.Save(SavePath + @"\" + this.SensorName + "-CalibrateFile" + ".txt");
            if (isSaveMapImage)
                this.Map?.WriteImage("tiff", 0, SavePath + @"\" + this.SensorName + "-Map" + ".tiff");
            if (this.DicCalibHomMat2D != null)
                BinarySerialize.Save(this.DicCalibHomMat2D, SavePath + @"\" + this.SensorName + "-DicCalib" + ".dat");
            //////////////////// 映射字典2D  ////////
            if (this.DicMapHomMat2D != null)
                BinarySerialize.Save(this.DicMapHomMat2D, SavePath + @"\" + this.SensorName + "-DicMap2D" + ".dat");
            //////////////////// 映射字典3D  ////////
            if (this.DicMapHomMat3D != null)
                BinarySerialize.Save(this.DicMapHomMat3D, SavePath + @"\" + this.SensorName + "-DicMap3D" + ".dat");
            return IsOk;
        }
        public virtual bool Save(string directName, bool isSaveMapImage = false)
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(directName + @"\" + SavePath)) DirectoryEx.Create(directName + @"\" + SavePath);
            if (this.SensorName != this.CaliParam.CamName)
                this.CaliParam.CamName = this.SensorName;
            IsOk = IsOk && XML<CameraParam>.Save(this, directName + @"\" + SavePath + @"\" + this.SensorName + ".xml");
            if (this.CalibrateFile != null)
                IsOk = IsOk && this.CalibrateFile.Save(directName + @"\" + SavePath + @"\" + this.SensorName + "-CalibrateFile" + ".txt");
            if (isSaveMapImage)
                this.Map?.WriteImage("tiff", 0, directName + @"\" + SavePath + @"\" + this.SensorName + "-Map" + ".tiff");
            if (this.DicCalibHomMat2D != null)
                BinarySerialize.Save(this.DicCalibHomMat2D, SavePath + @"\" + this.SensorName + "-DicCalib" + ".dat");
            /////////////////////////// 映射字典2D  ////////
            if (this.DicMapHomMat2D != null)
                BinarySerialize.Save(this.DicMapHomMat2D, SavePath + @"\" + this.SensorName + "-DicMap2D" + ".dat");
            /////////////////////////// 映射字典3D  ////////
            if (this.DicMapHomMat3D != null)
                BinarySerialize.Save(this.DicMapHomMat3D, SavePath + @"\" + this.SensorName + "-DicMap3D" + ".dat");
            return IsOk;
        }
        public virtual object Read()
        {
            CameraParam cameraParam;
            //if (this.SavePath != @"VisionParam\传感器参数")
            //    this.SavePath = @"VisionParam\传感器参数";
            if (File.Exists(SavePath + @"\" + this.SensorName + ".xml"))
            {
                cameraParam = XML<CameraParam>.Read(SavePath + @"\" + this.SensorName + ".xml");
                if (cameraParam != null)
                    cameraParam.CalibrateFile = new AxisCalibration().Read(SavePath + @"\" + this.SensorName + "-CalibrateFile" + ".txt"); // 相机的校准文件与相机参数放在一起
                if (File.Exists(SavePath + @"\" + this.SensorName + "-Map" + ".tiff"))
                    cameraParam.Map = new HImage(SavePath + @"\" + this.SensorName + "-Map" + ".tiff");
                if (File.Exists(SavePath + @"\" + this.SensorName + "-DicCalib" + ".dat"))
                    cameraParam.DicCalibHomMat2D = BinarySerialize.Read<Dictionary<CoordValuePairs, UserHomMat2D>>(SavePath + @"\" + this.SensorName + "-DicCalib" + ".dat");
                else
                    cameraParam.DicCalibHomMat2D = new Dictionary<CoordValuePairs, UserHomMat2D>();
                ///////////////////////// 映射字典  ////////
                if (File.Exists(SavePath + @"\" + this.SensorName + "-DicMap2D" + ".dat"))
                    cameraParam.DicMapHomMat2D = BinarySerialize.Read<Dictionary<string, UserHomMat2D>>(SavePath + @"\" + this.SensorName + "-DicMap2D" + ".dat");
                else
                    cameraParam.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
                ///////////////////////// 映射字典3D  ////////
                if (File.Exists(SavePath + @"\" + this.SensorName + "-DicMap3D" + ".dat"))
                    cameraParam.DicMapHomMat3D = BinarySerialize.Read<Dictionary<string, UserHomMat3D>>(SavePath + @"\" + this.SensorName + "-DicMap3D" + ".dat");
                else
                    cameraParam.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
                //////////////////////////////////////////
                if (cameraParam.DicCalibHomMat2D == null) cameraParam.DicCalibHomMat2D = new Dictionary<CoordValuePairs, UserHomMat2D>();
                if (cameraParam.DicMapHomMat2D == null) cameraParam.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
                if (cameraParam.DicMapHomMat3D == null) cameraParam.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            }
            else
                cameraParam = new CameraParam();
            ////////////////////////////////////////////
            if (cameraParam.SensorName != cameraParam.CaliParam.CamName)
                cameraParam.CaliParam.CamName = cameraParam.SensorName;
            ///////// 将所有读取的参数都加载到内存中来 ////////////////////
            if (DicSensorParam.ContainsKey(this.SensorName))
                DicSensorParam[this.SensorName] = cameraParam;
            else
                DicSensorParam.Add(this.SensorName, cameraParam);
            ////////////////////////  ////////////////////////////////////
            return cameraParam;
        }
        public virtual object Read(string sensorName)
        {
            CameraParam cameraParam;
            //if (this.SavePath != @"VisionParam\传感器参数")
            //    this.SavePath = @"VisionParam\传感器参数";
            if (File.Exists(SavePath + @"\" + sensorName + ".xml"))
            {
                cameraParam = XML<CameraParam>.Read(SavePath + @"\" + sensorName + ".xml");
                if (cameraParam != null)
                    cameraParam.CalibrateFile = new AxisCalibration().Read(SavePath + @"\" + sensorName + "-CalibrateFile" + ".txt");
                else
                    cameraParam = new CameraParam(sensorName);
                if (File.Exists(SavePath + @"\" + sensorName + "-Map" + ".tiff"))
                    cameraParam.Map = new HImage(SavePath + @"\" + sensorName + "-Map" + ".tiff");
                //////////////////////
                if (File.Exists(SavePath + @"\" + sensorName + "-DicCalib" + ".dat"))
                    cameraParam.DicCalibHomMat2D = BinarySerialize.Read<Dictionary<CoordValuePairs, UserHomMat2D>>(SavePath + @"\" + sensorName + "-DicCalib" + ".dat");
                else
                    cameraParam.DicCalibHomMat2D = new Dictionary<CoordValuePairs, UserHomMat2D>();
                ////////////////////////////////////////////////// 映射字典2D  ////////
                if (File.Exists(SavePath + @"\" + sensorName + "-DicMap2D" + ".dat"))
                    cameraParam.DicMapHomMat2D = BinarySerialize.Read<Dictionary<string, UserHomMat2D>>(SavePath + @"\" + sensorName + "-DicMap2D" + ".dat");
                else
                    cameraParam.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
                ////////////////////////////////////////////////// 映射字典3D  ////////
                if (File.Exists(SavePath + @"\" + sensorName + "-DicMap3D" + ".dat"))
                    cameraParam.DicMapHomMat3D = BinarySerialize.Read<Dictionary<string, UserHomMat3D>>(SavePath + @"\" + sensorName + "-DicMap3D" + ".dat");
                else
                    cameraParam.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
                //////////////////////////////////////////
                if (cameraParam.DicCalibHomMat2D == null) cameraParam.DicCalibHomMat2D = new Dictionary<CoordValuePairs, UserHomMat2D>();
                if (cameraParam.DicMapHomMat2D == null) cameraParam.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
                if (cameraParam.DicMapHomMat3D == null) cameraParam.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            }
            else
                cameraParam = new CameraParam(sensorName);
            ////////////////////////////////////////////
            if (cameraParam.SensorName != cameraParam.CaliParam.CamName)
                cameraParam.CaliParam.CamName = cameraParam.SensorName;
            ///////// 将所有读取的参数都加载到内存中来 ////////////////////
            if (DicSensorParam.ContainsKey(sensorName))
                DicSensorParam[sensorName] = cameraParam;
            else
                DicSensorParam.Add(sensorName, cameraParam);
            ////////////////////////////////////////////////////////////
            return cameraParam;
        }
        public virtual object Read(string directName, string sensorName)
        {
            CameraParam cameraParam;
            //if (this.SavePath != @"VisionParam\传感器参数")
            //    this.SavePath = @"VisionParam\传感器参数";
            if (File.Exists(directName + @"\" + SavePath + @"\" + sensorName + ".xml"))
            {
                cameraParam = XML<CameraParam>.Read(directName + @"\" + SavePath + @"\" + sensorName + ".xml");
                if (cameraParam != null)
                    cameraParam.CalibrateFile = new AxisCalibration().Read(directName + @"\" + SavePath + @"\" + sensorName + "-CalibrateFile" + ".txt");
                else
                    cameraParam = new CameraParam(sensorName);
                if (File.Exists(directName + @"\" + SavePath + @"\" + sensorName + "-Map" + ".tiff"))
                    cameraParam.Map = new HImage(directName + @"\" + SavePath + @"\" + sensorName + "-Map" + ".tiff");
                ////////////////////
                if (File.Exists(directName + @"\" + SavePath + @"\" + sensorName + "-DicCalib" + ".dat"))
                    cameraParam.DicCalibHomMat2D = BinarySerialize.Read<Dictionary<CoordValuePairs, UserHomMat2D>>(directName + @"\" + SavePath + @"\" + sensorName + "-DicCalib" + ".dat");
                else
                    cameraParam.DicCalibHomMat2D = new Dictionary<CoordValuePairs, UserHomMat2D>();
                //////////////////// 映射字典  ////////
                if (File.Exists(directName + @"\" + SavePath + @"\" + sensorName + "-DicMap2D" + ".dat"))
                    cameraParam.DicMapHomMat2D = BinarySerialize.Read<Dictionary<string, UserHomMat2D>>(directName + @"\" + SavePath + @"\" + sensorName + "-DicMap2D" + ".dat");
                else
                    cameraParam.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
                //////////////////// 映射字典  ////////
                if (File.Exists(directName + @"\" + SavePath + @"\" + sensorName + "-DicMap3D" + ".dat"))
                    cameraParam.DicMapHomMat3D = BinarySerialize.Read<Dictionary<string, UserHomMat3D>>(directName + @"\" + SavePath + @"\" + sensorName + "-DicMap3D" + ".dat");
                else
                    cameraParam.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
                //////////////////////////////////////////
                if (cameraParam.DicCalibHomMat2D == null) cameraParam.DicCalibHomMat2D = new Dictionary<CoordValuePairs, UserHomMat2D>();
                if (cameraParam.DicMapHomMat2D == null) cameraParam.DicMapHomMat2D = new Dictionary<string, UserHomMat2D>();
                if (cameraParam.DicMapHomMat3D == null) cameraParam.DicMapHomMat3D = new Dictionary<string, UserHomMat3D>();
            }
            else
                cameraParam = new CameraParam(sensorName);
            ////////////////////////////////////////////
            if (cameraParam.SensorName != cameraParam.CaliParam.CamName)
                cameraParam.CaliParam.CamName = cameraParam.SensorName;
            ///////// 将所有读取的参数都加载到内存中来 ////////////////////
            if (DicSensorParam.ContainsKey(sensorName))
                DicSensorParam[sensorName] = cameraParam;
            else
                DicSensorParam.Add(sensorName, cameraParam);
            //////////////////////////////////////////////////////////////
            return cameraParam;
        }
        public CameraParam Clone()
        {
            CameraParam camera = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, this);//序列化
                ms.Seek(0, SeekOrigin.Begin); // 将当前流的位置移动到开始处
                camera = (CameraParam)bf.Deserialize(ms);//反序列化
            }
            return camera;
        }

        public void CopyPropertyValue(CameraParam camera)
        {
            object value = "";
            PropertyInfo[] propertiesTarget = this.GetType().GetProperties();
            PropertyInfo[] propertiesSource = camera.GetType().GetProperties();
            PropertyInfo[] properSubTarget, properSubSource;
            int length = propertiesTarget.Length;
            for (int i = 0; i < length; i++)
            {
                switch (propertiesTarget[i].Name)
                {
                    case nameof(this.CaliParam):
                    case nameof(this.UvwParam):
                    case nameof(this.BoardParam):
                    case nameof(this.NPointCalibParam):
                        object value1 = propertiesTarget[i].GetValue(this);
                        object value2 = propertiesSource[i].GetValue(camera);
                        properSubTarget = value1.GetType().GetProperties();
                        properSubSource = value2.GetType().GetProperties();
                        for (int k = 0; k < properSubTarget.Length; k++)
                        {
                            if (properSubSource[k].Name == properSubTarget[k].Name) // 只有当源属性名称与目标属性名称相同时，才进行属性值复制
                            {
                                value = properSubSource[k].GetValue(value2);// 获取源中的值，
                                properSubTarget[k].SetValue(value1, value); // 将从源中获取的值赋值给目标对象
                            }
                            else
                                throw new ArgumentException("源属性名称与目标属性名称不相同");
                        }
                        break;
                    default:
                        if (propertiesSource[i].Name == propertiesTarget[i].Name) // 只有当源属性名称与目标属性名称相同时，才进行属性值复制
                        {
                            value = propertiesSource[i].GetValue(camera);
                            propertiesTarget[i].SetValue(this, value);
                        }
                        else
                            throw new ArgumentException("源属性名称与目标属性名称不相同");
                        break;
                }
            }
        }

        private bool TransformPixImageToWcsImage(HImage pixImage, HTuple camParam, HTuple camPose, HTuple camSlant, out HImage wcsImage)
        {
            bool result = false;
            wcsImage = null;
            if (pixImage == null) return result;
            HTuple width, height, leftUp_X, leftUp_Y, leftDown_X, leftDown_Y, rigthUp_X, rigthUp_Y, VerticalWCSDist, HorizontalWCSDist, VerticalScale, HorizontalScale, MeanScale;
            pixImage.GetImageSize(out width, out height);
            if (width == null || height == null || width.Length == 0 || height.Length == 0) return result;
            HOperatorSet.ImagePointsToWorldPlane(camParam, camPose, 0, 0, 1, out leftUp_X, out leftUp_Y);
            HOperatorSet.ImagePointsToWorldPlane(camParam, camPose, height - 1, 0, 1, out leftDown_X, out leftDown_Y);
            HOperatorSet.ImagePointsToWorldPlane(camParam, camPose, 0, width - 1, 1, out rigthUp_X, out rigthUp_Y);
            HOperatorSet.DistancePp(leftUp_Y, leftUp_X, leftDown_Y, leftDown_X, out VerticalWCSDist);
            HOperatorSet.DistancePp(leftUp_Y, leftUp_X, rigthUp_Y, rigthUp_X, out HorizontalWCSDist);
            VerticalScale = VerticalWCSDist / height;
            HorizontalScale = HorizontalWCSDist / width;
            MeanScale = (VerticalScale.D + HorizontalScale.D) / 2.0;
            ///////////////////////////
            HTuple homMat3d, homMat3dRotateX, homMat3dRotateZ, pose, NewPose;
            HOperatorSet.PoseToHomMat3d(camPose, out homMat3d);
            HOperatorSet.HomMat3dRotateLocal(homMat3d, camSlant.D * Math.PI / 180, "z", out homMat3dRotateZ);//0.55*Math.PI/180*-1
            HOperatorSet.HomMat3dRotateLocal(homMat3dRotateZ, Math.PI, "x", out homMat3dRotateX);
            HOperatorSet.HomMat3dToPose(homMat3dRotateX, out pose);
            HOperatorSet.SetOriginPose(pose, leftUp_X, leftUp_Y * -1, 0, out NewPose);
            wcsImage = pixImage.ImageToWorldPlane(new HCamPar(camParam), new HPose(NewPose), width.I, height.I, MeanScale, "bilinear");
            //this.rowScaleFactor = VerticalScale.D;
            //this.colScaleFactor = HorizontalScale.D;
            result = true;
            return result;
        }
        private void GenHeightImage(HImage image, double Z_Pos, out HImage heigthImage)
        {
            heigthImage = null;
            if (image == null) return;
            HTuple width, height;
            image.GetImageSize(out width, out height);
            float[] value = new float[width.I * height.I];
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (float)Z_Pos;
            }
            IntPtr ptr = Marshal.AllocHGlobal(value.Length * 4);
            Marshal.Copy(value, 0, ptr, value.Length);
            heigthImage = new HImage("real", width.I, height.I, ptr);
            Marshal.FreeHGlobal(ptr);
        }




    }



}
