using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;

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

        [DefaultValue(1000)]
        public double Exposure { get; set; }

        [DefaultValue(1)]
        public double PixScale { get; set; }  // 像素当量用于确定相机标定时计算的当量值

        [DefaultValue(false)]
        public bool IsRot { get; set; }

        [DefaultValue(false)]
        public bool IsActiveDistortionCorrect { get; set; }

        [DefaultValue(0)]
        public double CamSlant { get; set; } // 相机倾斜

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public AxisCalibration CalibrateFile { get; set; }   // 相机测量平面二次校准，，这个应该可以忽略了

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

        /// <summary>
        /// 相机畸变映射参数
        /// </summary>
        [Browsable(false)]

        [System.Xml.Serialization.XmlIgnore]
        public HImage Map { get; set; }

        public bool EnableDistoryRectify
        {
            set;
            get;
        }





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
            this.FlatData = new List<double>();
        }
        public CameraParam(UserHomMat2D homMat2D)
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
            this.HomMat2D = homMat2D;
            this.MapHomMat2D = new UserHomMat2D(true);
            this.CaliParam.CamCaliModel = enCamCaliModel.HomMat2D;
            this.FlatData = new List<double>();
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
            this.FlatData = new List<double>();
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
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
            }
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
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
                        wcs_z = grabImage_z; // - CaliParam.RotateCenter.Z
                    else
                        wcs_z = grabImage_z * -1; //CaliParam.RotateCenter.Z
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心: // 即相对于旋转中心的坐标
                    /////////////////对位贴合场景坐标系定义必需以平台作为参考对象，往右为正，往前为正/////////////////////////
                    if (CaliParam.IsMoveX) 　// 拍照坐标 + 计算值 - 标定中心值 ()
                        wcs_x = (Qx - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X;　// CaliParam.RotateCenter.X/CaliParam.RotateCenter.Y: 表示标定位置的XY拍照坐标
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = (Qx - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        wcs_y = (Qy - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y = (Qy - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z; // Math.Abs(CaliParam.CalibCenterXy.Z) + (grabImage_z + CaliParam.RotateCenter.Z);
                    else
                        wcs_z = grabImage_z * -1; // Math.Abs(CaliParam.CalibCenterXy.Z) + (CaliParam.RotateCenter.Z - grabImage_z);
                    break;
                ///////// 上下料模式，只需把中心点移动到当前点位置////////
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    double caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    double caliCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    ///////////////////////////////
                    wcs_x = (Qx + caliCenter_x) + (0 - CaliParam.CalibCenterXy.X);
                    wcs_y = (Qy + caliCenter_y) + (0 - CaliParam.CalibCenterXy.Y);
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.映射变换:
                    caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    caliCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX)   // 表示以相机作为参考对象 2024-01-13
                        wcs_x = caliCenter_x + Qx;   // 拍照坐标 + 计算值
                    else
                        wcs_x = caliCenter_x * -1 + Qx;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y = caliCenter_y + Qy;
                    else
                        wcs_y = caliCenter_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    wcs_z = HTuple.TupleGenConst(wcs_x.Length, wcs_z.D);
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    HTuple temp_x = new HTuple(wcs_x);
                    HTuple temp_y = new HTuple(wcs_y);
                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(temp_x, temp_y, out wcs_y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x += grabImage_x - caliCenter_x + CaliParam.AdjHomMatC02X;
                    else
                        wcs_x += caliCenter_x - grabImage_x + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        wcs_y += grabImage_y - caliCenter_y + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y += caliCenter_y - grabImage_y + CaliParam.AdjHomMatC12Y;
                    break;
            }

        }
        public void ImagePointsToWorldPlane(double Rows, double Coluns, double grabImage_x, double grabImage_y, double grabImage_z, out double wcs_x, out double wcs_y, out double wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
            /////////////////////////////////////////////////////////  这个标定中心在这里没什么用了，应该使用旋转的拍照位姿 ////////////////
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
            }
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
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
                        wcs_z = grabImage_z; // grabImage_z - CaliParam.RotateCenter.Z;
                    else
                        wcs_z = grabImage_z * -1;// CaliParam.RotateCenter.Z - grabImage_z;
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    //wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心:
                    ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转    对位贴合场景坐标系定义必需以平台作为参考对象，往右为正，往前为正/////////////////////////
                    /////// 2024/06/20 修改: (Qx.D - CaliParam.CalibCenter.X) :表示拍照位的Mark点相对于旋转中心的坐标，(grabImage_x - CaliParam.RotateCenter.X)：表示拍照位置变化后的补偿值
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    if (CaliParam.IsMoveX) 　// 拍照坐标 + 计算值 - 标定中心值 ()
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X;　// CaliParam.RotateCenter.X/CaliParam.RotateCenter.Y: 表示标定位置的XY拍照坐标
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z; // Math.Abs(CaliParam.CalibCenterXy.Z) + (grabImage_z + CaliParam.RotateCenter.Z);
                    else
                        wcs_z = grabImage_z * -1; // Math.Abs(CaliParam.CalibCenterXy.Z) + (CaliParam.RotateCenter.Z - grabImage_z);
                    break;
                ///////// 上下料模式，只需把中心点移动到当前点位置////////
                case enCoordOriginType.IsLoading:  // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    double caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    double caliCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    //wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + caliCenter_x;  // 标定拍照位置是固定的
                    //wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + caliCenter_y;
                    wcs_x = (Qx.D + caliCenter_x) + (0 - CaliParam.CalibCenterXy.X);
                    wcs_y = (Qy.D + caliCenter_y) + (0 - CaliParam.CalibCenterXy.Y);
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.映射变换:
                    caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    caliCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = caliCenter_x + Qx;   // 拍照坐标 + 计算值
                    else
                        wcs_x = caliCenter_x * -1 + Qx;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y = caliCenter_y + Qy;
                    else
                        wcs_y = caliCenter_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x += grabImage_x - caliCenter_x + CaliParam.AdjHomMatC02X;   // 拍照坐标 + 计算值
                    else
                        wcs_x += caliCenter_x - grabImage_x + CaliParam.AdjHomMatC02X;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y += grabImage_y - caliCenter_y + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y += caliCenter_y - grabImage_y + CaliParam.AdjHomMatC12Y;
                    break;
            }

        }

        public void ImageCenterPointsToWorldPlane(double grabImage_x, double grabImage_y, double grabImage_z, out double wcs_x, out double wcs_y, out double wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
            /////////////////////////////////////////////////////////  这个标定中心在这里没什么用了，应该使用旋转的拍照位姿 ////////////////
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(this.DataWidth * 0.5, this.DataHeight * 0.5, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), this.DataHeight * 0.5, this.DataWidth * 0.5, 1, out Qx, out Qy);
                    break;
            }
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
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
                        wcs_z = grabImage_z;// - CaliParam.RotateCenter.Z;
                    else
                        wcs_z = grabImage_z * -1;//CaliParam.RotateCenter.Z - grabImage_z;
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    //wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心:
                    ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转    对位贴合场景坐标系定义必需以平台作为参考对象，往右为正，往前为正/////////////////////////
                    if (CaliParam.IsMoveX) 　// 拍照坐标 + 计算值 - 标定中心值 ()
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X;　// CaliParam.RotateCenter.X/CaliParam.RotateCenter.Y: 表示标定位置的XY拍照坐标
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;//Math.Abs(CaliParam.CalibCenterXy.Z) + (grabImage_z + CaliParam.RotateCenter.Z);
                    else
                        wcs_z = grabImage_z * -1;//Math.Abs(CaliParam.CalibCenterXy.Z) + (CaliParam.RotateCenter.Z - grabImage_z);
                    break;
                ///////// 上下料模式，只需把中心点移动到当前点位置////////
                case enCoordOriginType.IsLoading:  // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    double caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    //wcs_x = (Qx.D - CaliParam.CalibCenterXy.X) + caliCenter_x;  // 标定拍照位置是固定的
                    //wcs_y = (Qy.D - CaliParam.CalibCenterXy.Y) + caliCenter_y;
                    wcs_x = (Qx.D + caliCenter_x) + (0 - CaliParam.CalibCenterXy.X);
                    wcs_y = (Qy.D + caliCenter_y) + (0 - CaliParam.CalibCenterXy.Y);
                    wcs_z = grabImage_z;
                    break;
                case enCoordOriginType.映射变换:
                    caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    //caliCenter_z = 0.5 * (CaliParam.StartCaliPoint.Z + CaliParam.EndCalibPoint.Z);
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x = caliCenter_x + Qx;
                    else
                        wcs_x = caliCenter_x * -1 + Qx;
                    if (CaliParam.IsMoveY)
                        wcs_y = caliCenter_y + Qy;
                    else
                        wcs_y = caliCenter_y * -1 + Qy;
                    if (CaliParam.IsMoveZ)
                        wcs_z = grabImage_z;
                    else
                        wcs_z = grabImage_z * -1;
                    // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
                    wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX) // 表示以相机作为参考对象 2024-01-13
                        wcs_x += grabImage_x - caliCenter_x + CaliParam.AdjHomMatC02X;   // 拍照坐标 + 计算值
                    else
                        wcs_x += caliCenter_x - grabImage_x + CaliParam.AdjHomMatC02X;   // 拍照坐标*-1 + 计算值
                    if (CaliParam.IsMoveY)
                        wcs_y += grabImage_y - caliCenter_y + CaliParam.AdjHomMatC12Y;
                    else
                        wcs_y += caliCenter_y - grabImage_y + CaliParam.AdjHomMatC12Y;
                    break;
            }

            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            //wcs_x = this.MapHomMat2D.GetHHomMat().AffineTransPoint2d(wcs_x, wcs_y, out wcs_y);
        }
        public void WorldPointsToImagePlane(HTuple wcs_x, HTuple wcs_y, double grabImage_x, double grabImage_y, out HTuple Rows, out HTuple Coluns)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
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
            if (wcs_x.Length != wcs_y.Length)
            {
                throw new ArgumentException("参wcs_x与wcs_y长度不相等");
            }
            /////////////////////////////////////////////////////////
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            HTuple temp_x = new HTuple(wcs_x);
            HTuple temp_y = new HTuple(wcs_y);
            //if (this.MapHomMat2D == null || this.MapHomMat2D.c00 == 0 || this.MapHomMat2D.c11 == 0)
            //    this.MapHomMat2D = new UserHomMat2D(true);
            //temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
                case enCoordOriginType.机械原点:
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
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (CaliParam.RotateCalibPoint.X - grabImage_x) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (grabImage_y - CaliParam.RotateCalibPoint.Y) - CaliParam.AdjHomMatC12Y;
                    else
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (CaliParam.RotateCalibPoint.Y - grabImage_y) - CaliParam.AdjHomMatC12Y;
                    break;
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    double caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    Qx = temp_x - caliCenter_x - (0 - CaliParam.CalibCenterXy.X);
                    Qy = temp_y - caliCenter_y - (0 - CaliParam.CalibCenterXy.Y);
                    break;
                case enCoordOriginType.映射变换:
                    caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX)
                        temp_x = wcs_x - (grabImage_x - caliCenter_x) - CaliParam.AdjHomMatC02X;
                    else
                        temp_x = wcs_x - (caliCenter_x - grabImage_x) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        temp_y = wcs_y - (grabImage_y - caliCenter_y) - CaliParam.AdjHomMatC12Y;
                    else
                        temp_y = wcs_y - (caliCenter_y - grabImage_y) - CaliParam.AdjHomMatC12Y;
                    /// 反向映射变换
                    if (this.MapHomMat2D == null || this.MapHomMat2D.c00 == 0 || this.MapHomMat2D.c11 == 0)
                        this.MapHomMat2D = new UserHomMat2D(true);
                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(temp_x, temp_y, out temp_y);
                    ///// 转换到相机坐标系中
                    if (CaliParam.IsMoveX) //
                        Qx = temp_x - caliCenter_x;
                    else
                        Qx = temp_x - caliCenter_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - caliCenter_y;
                    else
                        Qy = temp_y - caliCenter_y * -1;
                    break;
            }
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
                    break;
                ////////////////////////////////////////////////
                case enCamCaliModel.CamParamPose:
                    // 世界点到图像点
                    HTuple ProjMat, Cam_x, Cam_y, Cam_z, Qz;
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
            }

        }
        public void WorldPointsToImagePlane(double wcs_x, double wcs_y, double grabImage_x, double grabImage_y, out double Rows, out double Coluns)
        {
            double Qx, Qy;
            Rows = 0;
            Coluns = 0;
            /////////////////////////////////////////////////////////);
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            double temp_x = wcs_x;
            double temp_y = wcs_y;
            //if (this.MapHomMat2D == null || this.MapHomMat2D.c00 == 0 || this.MapHomMat2D.c11 == 0)
            //    this.MapHomMat2D = new UserHomMat2D(true);
            //temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(wcs_x, wcs_y, out temp_y);
            ////////////////////////////////////////
            switch (CaliParam.CoordOriginType)
            {
                default:
                case enCoordOriginType.机械原点:
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - grabImage_x;
                    else
                        Qx = temp_x - grabImage_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - grabImage_y;
                    else
                        Qy = temp_y - grabImage_y * -1;  // 相当于上下料模式
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心: // 相对坐标与相机的移动无关
                    ///////////////////////// 2024/04/25 修改   先标定9点再标定旋转 /////////////////////////
                    if (CaliParam.IsMoveX) // 拍照坐标 + 计算值 - 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (grabImage_x - CaliParam.RotateCalibPoint.X) - CaliParam.AdjHomMatC02X;
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        Qx = temp_x + CaliParam.CalibCenterXy.X - (CaliParam.RotateCalibPoint.X - grabImage_x) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (grabImage_y - CaliParam.RotateCalibPoint.Y) - CaliParam.AdjHomMatC12Y;
                    else
                        Qy = temp_y + CaliParam.CalibCenterXy.Y - (CaliParam.RotateCalibPoint.Y - grabImage_y) - CaliParam.AdjHomMatC12Y;
                    break;
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    double caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X);
                    double caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    Qx = temp_x - caliCenter_x - (0 - CaliParam.CalibCenterXy.X);
                    Qy = temp_y - caliCenter_y - (0 - CaliParam.CalibCenterXy.Y);
                    break;
                case enCoordOriginType.映射变换:
                    caliCenter_x = 0.5 * (CaliParam.StartCaliPoint.X + CaliParam.EndCalibPoint.X); // 所有的标定都将原点平移到视野中心，所以，标定中心的坐标这里不再需要了
                    caliCenter_y = 0.5 * (CaliParam.StartCaliPoint.Y + CaliParam.EndCalibPoint.Y);
                    /// 补偿轴移动
                    if (CaliParam.IsMoveX)
                        temp_x = wcs_x - (grabImage_x - caliCenter_x) - CaliParam.AdjHomMatC02X;
                    else
                        temp_x = wcs_x - (caliCenter_x - grabImage_x) - CaliParam.AdjHomMatC02X;
                    if (CaliParam.IsMoveY)
                        temp_y = wcs_y - (grabImage_y - caliCenter_y) - CaliParam.AdjHomMatC12Y;
                    else
                        temp_y = wcs_y - (caliCenter_y - grabImage_y) - CaliParam.AdjHomMatC12Y;
                    /// 反向映射变换
                    if (this.MapHomMat2D == null || this.MapHomMat2D.c00 == 0 || this.MapHomMat2D.c11 == 0)
                        this.MapHomMat2D = new UserHomMat2D(true);
                    temp_x = this.MapHomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(temp_x, temp_y, out temp_y);
                    ///////////////// 转换到相机坐标系中 ////////////////////////////
                    if (CaliParam.IsMoveX)
                        Qx = temp_x - caliCenter_x;
                    else
                        Qx = temp_x - caliCenter_x * -1;
                    if (CaliParam.IsMoveY)
                        Qy = temp_y - caliCenter_y;
                    else
                        Qy = temp_y - caliCenter_y * -1;
                    break;
            }
            ///////////////////////////////////// 
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
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
            }

        }


        public double TransPixLengthToWcsLength(double pixLength)
        {
            HTuple Qx, Qy;
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(new HTuple(0, pixLength), new HTuple(0, 0), out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), new HTuple(0, 0), new HTuple(0, pixLength), 1, out Qx, out Qy);
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
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
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

        public void TransWcsPointToRotatePoint(double wcs_x, double wcs_y, double grabImage_x, double grabImage_y, double grabImage_z, out double rotate_x, out double rotate_y, out double rotate_z)
        {
            double Rows, Coluns;
            HTuple Qx, Qy;
            this.WorldPointsToImagePlane(wcs_x, wcs_y, grabImage_x, grabImage_y, out Rows, out Coluns);
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
            }
            ////////////////////////////////////////
            if (CaliParam.IsMoveX)  // 拍照坐标 + 计算值 - 标定中心值 ()
                rotate_x = (Qx.D - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X; // CaliParam.RotateCenter.X/CaliParam.RotateCenter.Y: 表示标定位置的XY拍照坐标
            else              // 拍照坐标*-1 + 计算值 + 标定中心值
                rotate_x = (Qx.D - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X;
            if (CaliParam.IsMoveY)
                rotate_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
            else
                rotate_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y;
            if (CaliParam.IsMoveZ)
                rotate_z = grabImage_z;
            else
                rotate_z = grabImage_z * -1;
        }

        public void TransWcsPointToRotatePoint(HTuple wcs_x, HTuple wcs_y, double grabImage_x, double grabImage_y, double grabImage_z, out HTuple rotate_x, out HTuple rotate_y, out HTuple rotate_z)
        {
            HTuple Rows, Coluns, Qx, Qy;
            this.WorldPointsToImagePlane(wcs_x, wcs_y, grabImage_x, grabImage_y, out Rows, out Coluns);
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
                    break;
            }
            ////////////////////////////////////////
            if (CaliParam.IsMoveX)  // 拍照坐标 + 计算值 - 标定中心值 ()
                rotate_x = (Qx.D - CaliParam.CalibCenterXy.X) + (grabImage_x - CaliParam.RotateCalibPoint.X) + CaliParam.AdjHomMatC02X;
            else              // 拍照坐标*-1 + 计算值 + 标定中心值
                rotate_x = (Qx.D - CaliParam.CalibCenterXy.X) + (CaliParam.RotateCalibPoint.X - grabImage_x) + CaliParam.AdjHomMatC02X;
            if (CaliParam.IsMoveY)
                rotate_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (grabImage_y - CaliParam.RotateCalibPoint.Y) + CaliParam.AdjHomMatC12Y;
            else
                rotate_y = (Qy.D - CaliParam.CalibCenterXy.Y) + (CaliParam.RotateCalibPoint.Y - grabImage_y) + CaliParam.AdjHomMatC12Y;
            if (CaliParam.IsMoveZ)
                rotate_z = grabImage_z;
            else
                rotate_z = grabImage_z * -1;
        }

        public void TransWcsPointToRotatePoint(double wcs_x, double wcs_y, out double rotate_x, out double rotate_y)
        {
            rotate_x = (wcs_x - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;
            rotate_y = (wcs_y - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
        }

        public void TransWcsPointToRotatePoint(HTuple wcs_x, HTuple wcs_y, out HTuple rotate_x, out HTuple rotate_y)
        {
            rotate_x = (wcs_x - CaliParam.CalibCenterXy.X) + CaliParam.AdjHomMatC02X;
            rotate_y = (wcs_y - CaliParam.CalibCenterXy.Y) + CaliParam.AdjHomMatC12Y;
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
            HTuple Qx, Qy;
            this.WorldPointsToImagePlane(wcs_x, wcs_y, grabImage_x, grabImage_y, out Rows, out Coluns);
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
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
            HTuple Rows, Coluns, Qx, Qy;
            this.WorldPointsToImagePlane(wcs_x, wcs_y, grabImage_x, grabImage_y, out Rows, out Coluns);
            switch (CaliParam.CamCaliModel)
            {
                case enCamCaliModel.HomMat2D:
                case enCamCaliModel.HandEyeCali:
                case enCamCaliModel.UpDnCamCali:
                case enCamCaliModel.Cali9PtCali:
                case enCamCaliModel.NPointCali:
                case enCamCaliModel.CaliCaliBoard:
                default:
                    Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
                    break;
                case enCamCaliModel.CamParamPose:
                    HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
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
            camParam.Sx = Sx;
            camParam.Sy = Sy;
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



        public virtual bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            if (this.SensorName != this.CaliParam.CamName)
                this.CaliParam.CamName = this.SensorName;
            IsOk = IsOk && XML<CameraParam>.Save(this, SavePath + @"\" + this.SensorName + ".xml");
            IsOk = IsOk && this.CalibrateFile.Save(SavePath + @"\" + this.SensorName + "-CalibrateFile" + ".txt");
            this.Map?.WriteImage("tiff", 0, SavePath + @"\" + this.SensorName + "-Map" + ".tiff");
            return IsOk;
        }
        public virtual object Read()
        {
            CameraParam cameraParam;
            if (File.Exists(SavePath + @"\" + this.SensorName + ".xml"))
            {
                cameraParam = XML<CameraParam>.Read(SavePath + @"\" + this.SensorName + ".xml");
                if (cameraParam != null)
                    cameraParam.CalibrateFile = new AxisCalibration().Read(SavePath + @"\" + this.SensorName + "-CalibrateFile" + ".txt"); // 相机的校准文件与相机参数放在一起
                if (File.Exists(SavePath + @"\" + this.SensorName + "-Map" + ".tiff"))
                    cameraParam.Map = new HImage(SavePath + @"\" + this.SensorName + "-Map" + ".tiff");
            }
            else
                cameraParam = new CameraParam();
            ////////////////////////////////////////////
            if (cameraParam.SensorName != cameraParam.CaliParam.CamName)
                cameraParam.CaliParam.CamName = cameraParam.SensorName;
            return cameraParam;
        }
        public virtual object Read(string sensorName)
        {
            CameraParam cameraParam;
            if (File.Exists(SavePath + @"\" + sensorName + ".xml"))
            {
                cameraParam = XML<CameraParam>.Read(SavePath + @"\" + sensorName + ".xml");
                if (cameraParam != null)
                    cameraParam.CalibrateFile = new AxisCalibration().Read(SavePath + @"\" + sensorName + "-CalibrateFile" + ".txt");
                else
                    cameraParam = new CameraParam(sensorName);
                if (File.Exists(SavePath + @"\" + sensorName + "-Map" + ".tiff"))
                    cameraParam.Map = new HImage(SavePath + @"\" + sensorName + "-Map" + ".tiff");
            }
            else
                cameraParam = new CameraParam(sensorName);
            ////////////////////////////////////////////
            if (cameraParam.SensorName != cameraParam.CaliParam.CamName)
                cameraParam.CaliParam.CamName = cameraParam.SensorName;
            return cameraParam;
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
