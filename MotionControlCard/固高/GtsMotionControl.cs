using Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static MotionControlCard.Gts;

namespace MotionControlCard
{
    [Serializable]
    public class GtsMotionControl : MotionBase
    {

        #region  实现接口的操作

        public override bool Init(DeviceConnectConfigParam param)
        {
            bool result = false;
            this.Name = param.DeviceName;
            if (!param.IsActive) return result;
            try
            {
                short state = Gts.GT_Open(0, 1);//打开运动控制卡
                if (File.Exists(Application.StartupPath + "\\" + this.name + ".cfg"))
                    state = Gts.GT_LoadConfig(Application.StartupPath + "\\" + this.name + ".cfg");//下载配置文件 以设备名称来命名配置文件名义
                state = Gts.GT_ClrSts(1, 8);//清除各轴报警和限位
                state = Gts.GT_AxisOn(1);
                state = Gts.GT_AxisOn(2);
                this.CreateCoordSys(); // 初始化一个坐标系
                if (state == 0)
                {
                    result = true;
                    LoggerHelper.Info("运动控制卡：" + this.name + "初始化成功");
                }
                else
                {
                    LoggerHelper.Error("运动控制卡：" + this.name + "初始化失败");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("运动控制卡：" + this.name + "初始化失败", ex);
            }
            param.ConnectState = result;
            return result;
        }

        /// <summary>
        /// 单轴移动
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="axisPosition"></param>
        /// <param name="speed"></param>
        public override void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            try
            {
                int targetPose = 0;
                double velocity = 0;
                CoordSysConfigParam coordSysParam;
                switch (axisName)
                {
                    case enAxisName.X轴:
                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                        if (coordSysParam == null)
                        {
                            MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                        }
                        if (coordSysParam.InvertAxisFeedBack)
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv) * -1;
                        else
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv);
                        velocity = speed / coordSysParam.PulseEquiv;
                        this.MoveAxis(1, velocity, targetPose);
                        break;
                    case enAxisName.Y轴:
                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                        if (coordSysParam == null)
                        {
                            MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                        }
                        if (coordSysParam.InvertAxisFeedBack)
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv) * -1;
                        else
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv);
                        velocity = speed / coordSysParam.PulseEquiv;
                        this.MoveAxis(2, velocity, targetPose);
                        break;
                    case enAxisName.Z轴:
                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                        if (coordSysParam == null)
                        {
                            MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Z 轴");
                        }
                        if (coordSysParam.InvertAxisFeedBack)
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv) * -1;
                        else
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv);
                        velocity = speed / coordSysParam.PulseEquiv;
                        this.MoveAxis(3, velocity, targetPose);
                        break;
                    case enAxisName.U轴:
                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.U轴);
                        if (coordSysParam == null)
                        {
                            MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 U 轴");
                        }
                        if (coordSysParam.InvertAxisFeedBack)
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv) * -1;
                        else
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv);
                        velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                        this.MoveAxis(4, velocity, targetPose);
                        break;
                    case enAxisName.V轴:
                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.V轴);
                        if (coordSysParam == null)
                        {
                            MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 V 轴");
                        }
                        if (coordSysParam.InvertAxisFeedBack)
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv) * -1;
                        else
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv);
                        velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                        this.MoveAxis(5, velocity, targetPose);
                        break;
                    case enAxisName.W轴:
                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.W轴);
                        if (coordSysParam == null)
                        {
                            MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 W 轴");
                        }
                        if (coordSysParam.InvertAxisFeedBack)
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv) * -1;
                        else
                            targetPose = (int)(axisPosition / coordSysParam.PulseEquiv);
                        velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                        this.MoveAxis(6, velocity, targetPose);
                        break;
                    default:
                        break;

                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 多轴移动，XY轴；XYZ轴
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="axisPosition"></param>
        /// <param name="speed"></param>
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            int targetPose_x = 0;
            int targetPose_y = 0;
            int targetPose_z = 0;
            double velocity = 0;
            CoordSysConfigParam coordSysParam;
            switch (axisName)
            {
                case enAxisName.X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (int)(axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = (int)(axisPosition.X / coordSysParam.PulseEquiv);
                    velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(1, velocity, targetPose_x);
                    break;
                case enAxisName.Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (int)(axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = (int)(axisPosition.Y / coordSysParam.PulseEquiv);
                    velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(2, velocity, targetPose_y);
                    break;
                case enAxisName.Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Z 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_z = (int)(axisPosition.Z / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_z = (int)(axisPosition.Z / coordSysParam.PulseEquiv);
                    velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(3, velocity, targetPose_z);
                    break;
                case enAxisName.XY轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (int)(axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = (int)(axisPosition.X / coordSysParam.PulseEquiv);
                    velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(1, velocity, targetPose_x);
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (int)(axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = (int)(axisPosition.Y / coordSysParam.PulseEquiv);
                    velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(2, velocity, targetPose_y);
                    break;
                case enAxisName.XY轴直线插补:
                    int x1, y1, x2, y2;
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        x1 = (int)(axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        x1 = (int)(axisPosition.X / coordSysParam.PulseEquiv);
                    if (coordSysParam.InvertAxisFeedBack)
                        y1 = (int)(axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        y1 = (int)(axisPosition.Y / coordSysParam.PulseEquiv);
                    ////////////////////////////////////////////////////////////////////////////////////
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        x2 = (int)(axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        x2 = (int)(axisPosition.X / coordSysParam.PulseEquiv);
                    if (coordSysParam.InvertAxisFeedBack)
                        y2 = (int)(axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        y2 = (int)(axisPosition.Y / coordSysParam.PulseEquiv);
                    velocity = speed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.LineInterpolationMoveXY(x1, y1, x2, y2, velocity);
                    break;
            }
        }

        public override void MoveMultyAxis(MoveCommandParam Param)
        {
            CoordSysConfigParam coordSysParam, coordSysParam_y;
            int targetPose_x = 0;
            int targetPose_y = 0;
            int targetPose_z = 0;
            double velocity = 0;
            switch (Param.MoveAxis)
            {
                case enAxisName.X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (int)(Param.AxisParam.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = (int)(Param.AxisParam.X / coordSysParam.PulseEquiv);
                    velocity = Param.MoveSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(1, velocity, targetPose_x);
                    break;
                case enAxisName.Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (int)(Param.AxisParam.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = (int)(Param.AxisParam.Y / coordSysParam.PulseEquiv);
                    velocity = Param.MoveSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(2, velocity, targetPose_y);
                    break;
                case enAxisName.Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 Z 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_z = (int)(Param.AxisParam.Z / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_z = (int)(Param.AxisParam.Z / coordSysParam.PulseEquiv);
                    velocity = Param.MoveSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.MoveAxis(3, velocity, targetPose_z);
                    break;
                case enAxisName.XY轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (int)(Param.AxisParam.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = (int)(Param.AxisParam.X / coordSysParam.PulseEquiv);
                    velocity = Param.MoveSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    //this.MoveAxis(1, velocity, targetPose_x);
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (int)(Param.AxisParam.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = (int)(Param.AxisParam.Y / coordSysParam.PulseEquiv);
                    velocity = Param.MoveSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    //this.MoveAxis(2, velocity, targetPose_y);
                    this.MoveAxisXY(1, 2, velocity, targetPose_x, targetPose_y);
                    break;
                case enAxisName.XY轴直线插补:
                    int x1, y1, x2, y2;
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.X轴);
                    coordSysParam_y = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam_y == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        x1 = (int)(Param.AxisParam.X / coordSysParam.PulseEquiv) * -1;
                    else
                        x1 = (int)(Param.AxisParam.X / coordSysParam.PulseEquiv);
                    if (coordSysParam_y.InvertAxisFeedBack)
                        y1 = (int)(Param.AxisParam.Y / coordSysParam_y.PulseEquiv) * -1;
                    else
                        y1 = (int)(Param.AxisParam.Y / coordSysParam_y.PulseEquiv);
                    ////////////////////////////////////////////////////////////////////////////////////                 
                    if (coordSysParam.InvertAxisFeedBack)
                        x2 = (int)(Param.AxisParam2.X / coordSysParam.PulseEquiv) * -1;
                    else
                        x2 = (int)(Param.AxisParam2.X / coordSysParam.PulseEquiv);
                    if (coordSysParam_y.InvertAxisFeedBack)
                        y2 = (int)(Param.AxisParam2.Y / coordSysParam_y.PulseEquiv) * -1;
                    else
                        y2 = (int)(Param.AxisParam2.Y / coordSysParam_y.PulseEquiv);
                    velocity = Param.MoveSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.LineInterpolationMoveXY(x1, y1, x2, y2, velocity);
                    break;
                case enAxisName.XYZ轴:
                case enAxisName.XYZU轴:
                case enAxisName.XYZUV轴:

                    break;
                case enAxisName.XY轴圆弧插补:
                    int x, y, radius;
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.X轴);
                    coordSysParam_y = CoordSysConfigParamManger.Instance.GetCoordSysParam(Param.CoordSysName, enAxisName.Y轴);
                    if (coordSysParam_y == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + Param.CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam.InvertAxisFeedBack)
                        x = (int)(Param.CircleParam.center_x / coordSysParam.PulseEquiv) * -1;
                    else
                        x = (int)(Param.CircleParam.center_x / coordSysParam.PulseEquiv);
                    ////////////////////////////////////////////////////////////////////////////////////
                    if (coordSysParam_y.InvertAxisFeedBack)
                        y = (int)(Param.CircleParam.center_y / coordSysParam_y.PulseEquiv) * -1;
                    else
                        y = (int)(Param.CircleParam.center_y / coordSysParam_y.PulseEquiv);
                    ////////////////////////////
                    //if (coordSysParam.InvertAxisFeedBack)
                    //    radius = (int)(Param.CircleParam.Radius / coordSysParam.PulseEquiv) * -1;
                    //else
                    radius = (int)(Param.CircleParam.Radius / coordSysParam.PulseEquiv);
                    velocity = Param.MoveSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.CircleInterpolationMoveXY(velocity, x, y, radius, Param.CircleParam.start_deg, Param.CircleParam.end_deg);
                    break;
            }
        }

        /// <summary>
        /// 执行单轴回零
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="homSpeed"></param>
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            CoordSysConfigParam coordSysParam;
            double velocity = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(1, velocity);
                    break;
                case enAxisName.Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(2, velocity);
                    break;
                case enAxisName.Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Z 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(3, velocity);
                    break;
                case enAxisName.U轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.U轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 U 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(4, velocity);
                    break;
                case enAxisName.V轴:

                    break;
                case enAxisName.W轴:

                    break;
                case enAxisName.XYZ轴:

                    break;
                case enAxisName.XYZU轴:

                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 执行三轴或四轴回零
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="homSpeed"></param>
        public override void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            CoordSysConfigParam coordSysParam;
            double velocity = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(1, velocity);
                    break;
                case enAxisName.Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(2, velocity);
                    break;
                case enAxisName.Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Z 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(3, velocity);
                    break;

                case enAxisName.U轴:

                    break;
                case enAxisName.V轴:

                    break;
                case enAxisName.W轴:

                    break;
                case enAxisName.XY轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(1, velocity);
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    velocity = homSpeed / coordSysParam.PulseEquiv; // 将mm 转化为 pulse/s
                    this.AxisHome(2, velocity);
                    break;
                case enAxisName.XYZ轴:
                    ;
                    break;
                case enAxisName.XYZU轴:

                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 获取单轴的当前位置
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="position"></param>
        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            position = 0;
            short axis = 0;
            double pos;
            int pos2;
            uint clk;
            CoordSysConfigParam coordSysParam;
            switch (axisName)
            {
                case enAxisName.X轴:
                    axis = 1;
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    Gts.GT_GetEncPos(axis, out pos, 1, out clk);
                    //Gts.GT_GetPos(axis, out pos2);
                    if (coordSysParam.InvertAxisFeedBack)
                        position = pos * coordSysParam.PulseEquiv * -1 ;
                    else
                        position = pos * coordSysParam.PulseEquiv;
                    break;
                case enAxisName.Y轴:
                    axis = 2;
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    //Gts.GT_GetEncPos(axis, out encpos, 1, out clk);
                    //Gts.GT_GetPos(axis, out pos);
                    Gts.GT_GetEncPos(axis, out pos, 1, out clk);
                    if (coordSysParam.InvertAxisFeedBack)
                        position = pos * coordSysParam.PulseEquiv * -1;
                    else
                        position = pos * coordSysParam.PulseEquiv;
                    break;
                case enAxisName.Z轴:
                    axis = 3;
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Z 轴");
                    }
                    //Gts.GT_GetEncPos(axis, out encpos, 1, out clk);
                    //Gts.GT_GetPos(axis, out pos);
                    Gts.GT_GetEncPos(axis, out pos, 1, out clk);
                    if (coordSysParam.InvertAxisFeedBack)
                        position = pos * coordSysParam.PulseEquiv * -1;
                    else
                        position = pos * coordSysParam.PulseEquiv;
                    break;
                case enAxisName.U轴:
                    position = 0;
                    break;
                case enAxisName.V轴:
                    position = 0;
                    break;
                case enAxisName.W轴:
                    position = 0;
                    break;
                default:
                    break;

            }

        }

        /// <summary>
        /// Jog移动轴
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed">速度为正，往正方向移动，速度为负，往负方向移动</param>
        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            CoordSysConfigParam coordSysParam;
            double vel = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 X 轴");
                    }
                    if (coordSysParam.InvertJogAxis)
                        vel = speed * -1 / coordSysParam.PulseEquiv;
                    else
                        vel = speed * 1 / coordSysParam.PulseEquiv;
                    this.JogAxis(1, vel);
                    break;
                case enAxisName.Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Y 轴");
                    }
                    if (coordSysParam.InvertJogAxis)
                        vel = speed * -1 / coordSysParam.PulseEquiv;
                    else
                        vel = speed * 1 / coordSysParam.PulseEquiv;
                    this.JogAxis(2, vel);
                    break;
                case enAxisName.Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null)
                    {
                        MessageBox.Show("指定坐标系：" + CoordSysName + "中未配置 Z 轴");
                    }
                    if (coordSysParam.InvertJogAxis)
                        vel = speed * -1 / coordSysParam.PulseEquiv;
                    else
                        vel = speed * 1 / coordSysParam.PulseEquiv;
                    this.JogAxis(3, vel);
                    break;
                case enAxisName.U轴:

                    break;
                case enAxisName.V轴:

                    break;
                case enAxisName.W轴:

                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 停止Jog移动
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed"></param>
        public override void JogAxisStop()
        {
            for (int i = 0; i < 4; i++)
            {
                Gts.GT_Stop(1 << ((i + 1) - 1), 0);
            }
        }

        /// <summary>
        /// 减速停止
        /// </summary>
        public override void SlowDownStopAxis()
        {
            for (int i = 0; i < 4; i++)
            {
                Gts.GT_Stop(1 << ((i + 1) - 1), 0);
            }
        }

        /// <summary>
        /// 立即停止 
        /// </summary>
        public override void EmgStopAxis()
        {
            for (int i = 0; i < 4; i++)
            {
                Gts.GT_Stop(1 << ((i + 1) - 1), 0);
            }
        }
        public override void UnInit()
        {
            try
            {
                Gts.GT_Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("关闭控制器：" + this.name + "失败");
            }
        }

        /// <summary>
        /// 使用结构体来传参是最灵活的一种方式，所有的不确定个数的传参都可以使用结构体来传递  
        /// </summary>
        /// <param name="paramType"></param>
        /// <param name="paramValue"></param>
        public override void SetParam(enParamType paramType, params object[] paramValue)
        {
            try
            {
                if (paramValue == null) return;
                switch (paramType)
                {
                    case enParamType.IO控制:
                        IoParam param = (IoParam)paramValue[0];
                        switch (param.IoPortType)
                        {
                            case enIoPortType.通用Io端口:
                                switch (param.IoOutputMode)
                                {
                                    case enIoOutputMode.脉冲输出:
                                        Gts.GT_SetDoBitReverse(12, (short)param.IoPort, (short)param.IoValue, (short)param.IoReverseTime);
                                        break;
                                    case enIoOutputMode.高电平输出:
                                        Gts.GT_SetDoBit(12, (short)param.IoPort, (short)param.IoValue);
                                        break;
                                }
                                break;
                            case enIoPortType.高速Io端口:

                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(paramType.ToString() + "参数设置错误!" + ex.ToString());
            }
        }
        public override object GetParam(enParamType paramType, params object[] paramValue)
        {
            //return null;
            throw new NotImplementedException();
        }



        public override void SetIoOutputBit(object IoType, int IoNum, bool state)
        {

        }
        public override void GetIoOutputBit(object param, int IoNum, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
        }
        public override void SetIoIntputBit(object param, int IoNum, bool state)
        {
            //throw new NotImplementedException();
        }
        public override void GetIoIntputBit(object param, int IoNum, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
        }

        public override void WriteIoOutputBit(enIoPortType ioPortType, int IoPort, params object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoOutputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoIntputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override void WriteIoOutputGroup(enIoPortType ioPortType, int IoPort, params object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoOutputGroup(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoIntputGroup(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override object ReadValue(enDataTypes dataType, string address, int length)
        {
            throw new NotImplementedException();
        }
        public override bool WriteValue(enDataTypes dataType, string address, object value)
        {
            throw new NotImplementedException();
        }


        #endregion


        /// <summary>
        /// 移动轴
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed">脉冲/秒</param>
        /// <param name="axisPosition"></param>
        /// <param name="isWaite"></param>
        private void MoveAxis(short axis, double speed, int axisPosition, bool isWaite = true)
        {
            Gts.TTrapPrm trap;
            int state = 1;
            uint pClock;
            //读取数据
            trap.acc = 0.5;
            trap.dec = 0.5;
            trap.smoothTime = 5;
            trap.velStart = 0;
            Gts.GT_PrfTrap(axis); // 设置点位模式
            Gts.GT_SetTrapPrm(axis, ref trap);//设置点位运动参数
            Gts.GT_SetVel(axis, speed * 0.001);//设置目标速度 pulse / ms   ,每ms多少脉冲
            Gts.GT_SetPos(axis, (int)(axisPosition));//设置目标位置
            //Gts.GT_SetEncPos(axis, (int)(axisPosition));//设置目标位置
            Gts.GT_Update(1 << (axis - 1));//更新轴运动
            /////////////////////// 等待轴运动到位停止 ////////////
            if (isWaite)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                do
                {
                    Gts.GT_GetSts(axis, out state, 1, out pClock);
                    Application.DoEvents();
                    if (stopwatch.ElapsedMilliseconds >= 1000 * 60) break; // 超过60S未到位，将停止运动
                } while ((state & 0x400) > 0); // 11 表示轴静止
                stopwatch.Stop();
            }
        }

        private void MoveAxisXY(short axis1, short axis2, double speed, int axisPosition_x, int axisPosition_y, bool isWaite = true)
        {
            Gts.TTrapPrm trap;
            int state = 1;
            uint pClock;
            //读取数据
            trap.acc = 0.5;
            trap.dec = 0.5;
            trap.smoothTime = 5;
            trap.velStart = 0;
            Gts.GT_PrfTrap(axis1); // 设置点位模式
            Gts.GT_SetTrapPrm(axis1, ref trap);//设置点位运动参数
            Gts.GT_SetVel(axis1, speed * 0.001);//设置目标速度 pulse / ms   ,每ms多少脉冲
            Gts.GT_SetPos(axis1, (int)(axisPosition_x));//设置目标位置           
            Gts.GT_Update(1 << (axis1 - 1));//更新轴运动
                                            ///////////////////////XYY
            Gts.GT_PrfTrap(axis2); // 设置点位模式
            Gts.GT_SetTrapPrm(axis2, ref trap);//设置点位运动参数
            Gts.GT_SetVel(axis2, speed * 0.001);//设置目标速度 pulse / ms   ,每ms多少脉冲
            Gts.GT_SetPos(axis2, (int)(axisPosition_y));//设置目标位置           
            Gts.GT_Update(1 << (axis2 - 1));//更新轴运动
            /////////////////////// 等待轴运动到位停止 ////////////
            if (isWaite)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                do
                {
                    Gts.GT_GetSts(axis1, out state, 1, out pClock);
                    Application.DoEvents();
                    if (stopwatch.ElapsedMilliseconds >= 1000 * 60) break; // 超过60S未到位，将停止运动
                } while ((state & 0x400) > 0); // 11 表示轴静止
                stopwatch.Stop();
            }
            /////////////////////////
            if (isWaite)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                do
                {
                    Gts.GT_GetSts(axis2, out state, 1, out pClock);
                    Application.DoEvents();
                    if (stopwatch.ElapsedMilliseconds >= 1000 * 60) break; // 超过60S未到位，将停止运动
                } while ((state & 0x400) > 0); // 11 表示轴静止
                stopwatch.Stop();
            }
        }
        /// <summary>
        /// 轴回零
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed">脉冲/秒</param>
        private void AxisHome(short axis, double speed)
        {
            THomeStatus tHomeSts;
            //设置 Smart Home 回原点参数
            THomePrm tHomePrm;
            short sRtn = GT_GetHomePrm(1, out tHomePrm);
            tHomePrm.mode = 11;
            tHomePrm.moveDir = -1;
            tHomePrm.indexDir = 1;
            tHomePrm.edge = 0;
            tHomePrm.velHigh = speed * 0.001;
            tHomePrm.velLow = speed * 0.1 * 0.001;
            tHomePrm.acc = 0.5;
            tHomePrm.dec = 0.5;
            tHomePrm.searchHomeDistance = 0;
            tHomePrm.searchIndexDistance = 0;
            tHomePrm.escapeStep = 1000;
            tHomePrm.homeOffset = 0;
            ///////////////////////////////////////
            Gts.GT_ZeroPos(axis, 1); // 回零前清零
            sRtn = GT_GoHome(axis, ref tHomePrm); //启动 Smart Home 回原点
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                sRtn = GT_GetHomeStatus(axis, out tHomeSts); //获取回原点状态
                Application.DoEvents();
                if (stopwatch.ElapsedMilliseconds >= 1000 * 60) break; // 超过60S 将停止回零
            } while (tHomeSts.run != 0);
            stopwatch.Stop();
            Gts.GT_ZeroPos(axis, 1);// 回零前清零
        }

        /// <summary>
        /// Jog移动轴
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed">脉冲/秒速度</param>
        private void JogAxis(short axis, double speed)
        {
            Gts.TJogPrm jog = new Gts.TJogPrm(); ;
            jog.acc = 0.5;
            jog.dec = 0.5;
            jog.smooth = 0.1;
            Gts.GT_ClrSts(1, 8);//清除各轴报警和限位
            Gts.GT_PrfJog(axis);
            Gts.GT_SetJogPrm(axis, ref jog);//设置jog运动参数
            Gts.GT_SetVel(axis, speed * 0.001);//设置目标速度
            Gts.GT_Update(1 << (axis - 1));//更新轴运动
        }

        /// <summary>
        /// 创建坐标系
        /// </summary>
        private void CreateCoordSys()
        {
            short sRtn;
            Gts.TCrdPrm crdPrm;
            crdPrm.dimension = 2;                        // 建立三维的坐标系
            crdPrm.synVelMax = 500;                      // 坐标系的最大合成速度是: 500 pulse/ms
            crdPrm.synAccMax = 2;                        // 坐标系的最大合成加速度是: 2 pulse/ms^2
            crdPrm.evenTime = 20;                         // 坐标系的最小匀速时间为0
            crdPrm.profile1 = 1;                       // 规划器1对应到X轴                       
            crdPrm.profile2 = 2;                       // 规划器2对应到Y轴
            crdPrm.profile3 = 0;                       // 规划器3对应到Z轴
            crdPrm.profile4 = 0;
            crdPrm.profile5 = 0;
            crdPrm.profile6 = 0;
            crdPrm.profile7 = 0;
            crdPrm.profile8 = 0;
            crdPrm.setOriginFlag = 1;                    // 需要设置加工坐标系原点位置
            crdPrm.originPos1 = 0;                     // 加工坐标系原点位置在(0,0,0)，即与机床坐标系原点重合
            crdPrm.originPos2 = 0;
            crdPrm.originPos3 = 0;
            crdPrm.originPos4 = 0;
            crdPrm.originPos5 = 0;
            crdPrm.originPos6 = 0;
            crdPrm.originPos7 = 0;
            crdPrm.originPos8 = 0;
            sRtn = Gts.GT_SetCrdPrm(1, ref crdPrm);
        }

        /// <summary>
        /// XY平面直插补
        /// </summary>
        /// <param name="x1">脉冲位置</param>
        /// <param name="y1">脉冲位置</param>
        /// <param name="x2">脉冲位置</param>
        /// <param name="y2">脉冲位置</param>
        /// <param name="speed">脉冲速度</param>
        /// <param name="isWaite">是否等待完成</param>
        private void LineInterpolationMoveXY(int x1, int y1, int x2, int y2, double speed, bool isWaite = true)
        {
            short sRtn;// 指令返回值变量
            //this.MoveAxis(1, speed, x1); // 移动到起点位置
            //this.MoveAxis(2, speed, y1);
            this.MoveAxisXY(1, 2, speed, x1, y1);
            this.CreateCoordSys(); // 坐标系必须在插补前建立
            // 即将把数据存入坐标系1的FIFO0中，所以要首先清除此缓存区中的数据
            sRtn = Gts.GT_CrdClear(1, 0);
            // 向缓存区写入第一段插补数据
            sRtn = Gts.GT_LnXY(
                1,              // 该插补段的坐标系是坐标系1
                x2,
                y2,     // 该插补段的终点坐标(200000, 0)
                speed * 0.001,              // 该插补段的目标速度：100pulse/ms ， 输入参数为： 多少脉冲/秒
                0.1,                // 插补段的加速度：0.1pulse/ms^2
                0.0,              // 终点速度为0
                0);
            // 启动坐标系1的FIFO0的插补运动
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            sRtn = Gts.GT_CrdStart(1, 0);
            if (isWaite)
            {
                int state;
                uint pClock;
                short crd = 1, pRun = 1;
                int pSegment;
                do
                {
                    //Gts.GT_GetSts(1, out state, 1, out pClock);
                    sRtn = Gts.GT_CrdStatus(crd, out pRun, out pSegment, 0);
                    Application.DoEvents();
                    if (stopwatch.ElapsedMilliseconds >= 1000 * 60) break; // 超过60S 将停止执行
                } while (pRun == 1); //    (state & 0x400) > 0 
                stopwatch.Stop();
            }
        }

        /// <summary>
        /// 圆弧插补
        /// </summary>
        /// <param name="speed">脉冲/秒速度</param>
        /// <param name="center_x">圆心坐标</param>
        /// <param name="center_y">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="start_deg">起始角</param>
        /// <param name="end_deg">终止角</param>
        /// <param name="isWaite">是否待待</param>
        private void CircleInterpolationMoveXY(double speed, int center_x, int center_y, int radius, double start_deg = 0, double end_deg = 360, bool isWaite = true)
        {
            // 指令返回值变量
            short sRtn;
            double startPos_x = center_x + radius * Math.Cos(start_deg * Math.PI / 180);
            double startPos_y = center_y + radius * Math.Sin(start_deg * Math.PI / 180);
            double endPos_x = center_x + radius * Math.Cos(end_deg * Math.PI / 180);
            double endPos_y = center_y + radius * Math.Cos(end_deg * Math.PI / 180);
            ////////////////////////////
            this.MoveAxis(1, speed, (int)startPos_x); // 移动到起点位置
            this.MoveAxis(2, speed, (int)startPos_y);
            this.CreateCoordSys(); // 坐标系必须在插补前建立
            // 即将把数据存入坐标系1的FIFO0中，所以要首先清除此缓存区中的数据
            sRtn = Gts.GT_CrdClear(1, 0);
            // 向缓存区写入第三段插补数据，该段数据是以半径描述方法描述了一个1/4圆弧
            sRtn = Gts.GT_ArcXYR(
                1,					// 坐标系是坐标系1
                (int)endPos_x,
                (int)endPos_y,			// 该圆弧的终点坐标(0, 200000)
                radius,				// 半径：200000pulse
                1,					// 该圆弧是逆时针圆弧
                speed * 0.001,					// 该插补段的目标速度：100pulse/ms
                0.1,					// 该插补段的加速度：0.1pulse/ms^2
                0,					// 终点速度为0
                0);                 // 向坐标系1的FIFO0缓存区传递该直线插补数据
                                    // 启动坐标系1的FIFO0的插补运动
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            sRtn = Gts.GT_CrdStart(1, 0);
            //////////////////////////
            if (isWaite)
            {
                short crd = 1, pRun = 1;
                int pSegment;
                do
                {
                    Gts.GT_CrdStatus(crd, out pRun, out pSegment, 1);
                    Application.DoEvents();
                    if (stopwatch.ElapsedMilliseconds >= 1000 * 60) break; // 超过60S 将停止执行
                } while (pRun != 0);
            }
            stopwatch.Stop();
        }


    }
}
