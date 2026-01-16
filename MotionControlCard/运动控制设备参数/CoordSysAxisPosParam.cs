using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{

    public enum enAxisName
    {
        NONE = 0,
        X轴 = 1,
        Y轴 = 2,
        Z轴 = 3,
        Theta轴 = 4,
        U轴 = 5,
        V轴 = 6,
        W轴 = 7,
        相机轴X = 8,
        相机轴Y = 9,
        X2轴 = 10,
        Y2轴 = 11,
        XY轴 = 12,
        XZ轴 = 13,
        YZ轴 = 14,
        XYZ轴 = 15,
        XYTheta轴 = 16,
        XYZTheta轴 = 17,
        XYZU轴 = 18,
        XYZUV轴 = 19,
        XYZUVW轴 = 20,
        UVW轴 = 21,
        Compensation_X轴 = 22,
        Compensation_Y轴 = 23,
        Compensation_Z轴 = 24,
        Compensation_Theta轴 = 25,
        Compensation_U轴 = 26,
        Compensation_V轴 = 27,
        Compensation_W轴 = 28,
        Compensation_X2轴 = 29,
        Compensation_Y2轴 = 30,
        Compensation_Z2轴 = 31,
        Compensation_Theta2轴 = 32,
        Compensation_U2轴 = 33,
        Compensation_V2轴 = 34,
        Compensation_W2轴 = 35,
        Compensation_XYTheta轴 = 36,
        Compensation_XYZUVW轴 = 37,
        Compensation_XYZTheta轴 = 38,
        Compensation_UVW轴 = 39,
        XZ轴直线插补 = 40,
        YZ轴直线插补 = 41,
        XYZ轴直线插补 = 42,
        XY轴圆弧插补 = 43,
        XZ轴圆弧插补 = 44,
        YZ轴圆弧插补 = 45,
        XYZ轴圆弧插补 = 46,
        XY轴椭圆插补 = 47,
        XZ轴椭圆插补 = 48,
        YZ轴椭圆插补 = 49,
        XYZ轴椭圆插补 = 50,
        XY螺旋线插补 = 51,
        XY同心圆插补 = 52,
        XY轴矩形插补 = 53,
        XYZ螺旋线插补 = 54,
        X轴轨迹运动 = 55,
        Y轴轨迹运动 = 56,
        Z轴轨迹运动 = 57,
        U轴轨迹运动 = 58,
        V轴轨迹运动 = 59,
        W轴轨迹运动 = 60,
        XY轴轨迹运动 = 61,
        XZ轴轨迹运动 = 62,
        YZ轴轨迹运动 = 63,
        XYZ轴轨迹运动 = 64,
        XYZU轴轨迹运动 = 65,
        XYZUV轴轨迹运动 = 66,
        XYZUVW轴轨迹运动 = 67,
        XY轴十字运动 = 68,
        A轴 = 69,
        B轴 = 70,
        C轴 = 71,
        XY轴直线插补 = 72,
        OK = 888,
        NG = 889,
        Continue = 900,
        TriggerToPlc = 901,
        TriggerFromPlc = 902,
    }
    public enum enParamType
    {
        NONE,
        IO控制,
        清除轴1错误,
        清除轴2错误,
        清除轴3错误,
        清除轴4错误,
        清除轴5错误,
        清除轴6错误,
        清除轴7错误,
        清除轴8错误,
        清除轴9错误,
        清除轴10错误,
        清除轴11错误,
        清除轴12错误,
        触发间隔,
    }

    public enum enCoordType
    {
        机台坐标,
        补偿坐标,
    }

    public enum enMoveType
    {
        none,
        点位运动,
        直线运动,
        矩形1运动,
        矩形2运动,
        圆运动,
        椭圆运动,
        多边形运动,
        多段线,
    }

    [Serializable]
    public struct MoveCommandParam
    {
        public enMoveType MoveType;
        public enCoordSysName CoordSysName;
        public enAxisName MoveAxis;
        public double MoveSpeed;
        public double StartVel;//起始速度
        public double Tacc;//加速时间
        public double Tdec;//减速时间
        public double StopVel;//停止速度
        public double S_para;//S段时间
        public int FactorError;
        public string PoseInfo;
        public bool IsWait;

        public CoordSysAxisPosParam AxisParam;  // 用于直线扫描的第一点或点采集的采集点
        public CoordSysAxisPosParam AxisParam2;  // 用于直线扫描的第二点
        public CircleInterpolateParam CircleParam;
        public Rect2InterpolateParam Rect2Param;
        public List<double[]> TrackData;

        public WcsROI MoveTrack;
        public MoveCommandParam(bool isInit = true)
        {
            this.MoveType = enMoveType.点位运动;
            this.MoveAxis = enAxisName.XY轴;
            this.MoveSpeed = 100;
            this.AxisParam = new CoordSysAxisPosParam();
            this.AxisParam2 = new CoordSysAxisPosParam();
            this.PoseInfo = "1";
            this.IsWait = true;
            /////////////////////
            this.StartVel = 0;
            this.Tacc = 0.5;
            this.Tdec = 0.5;
            this.StopVel = 0;
            this.S_para = 0.1;
            this.FactorError = 10;
            this.CircleParam = new CircleInterpolateParam();
            this.Rect2Param = new Rect2InterpolateParam();
            this.TrackData = new List<double[]>();
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveTrack = new WcsROI();
        }
        public MoveCommandParam(enAxisName moveAxis, double moveSpeed)
        {
            this.MoveType = enMoveType.点位运动;
            this.MoveAxis = moveAxis;
            this.MoveSpeed = moveSpeed;
            this.AxisParam = new CoordSysAxisPosParam();
            this.AxisParam2 = new CoordSysAxisPosParam();
            this.PoseInfo = "1";
            this.IsWait = true;
            /////////////////////
            this.StartVel = 0;
            this.Tacc = 0.5;
            this.Tdec = 0.5;
            this.StopVel = 0;
            this.S_para = 0.1;
            this.FactorError = 10;
            this.CircleParam = new CircleInterpolateParam();
            this.Rect2Param = new Rect2InterpolateParam();
            this.TrackData = new List<double[]>();
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveTrack = new WcsROI();
        }
        public MoveCommandParam(enAxisName moveAxis, double moveSpeed, CoordSysAxisPosParam sysAxisParam)
        {
            this.MoveType = enMoveType.点位运动;
            this.MoveAxis = moveAxis;
            this.MoveSpeed = moveSpeed;
            this.AxisParam = sysAxisParam;
            this.AxisParam2 = new CoordSysAxisPosParam();
            this.PoseInfo = "1";
            this.IsWait = true;
            /////////////////////
            this.StartVel = 0;
            this.Tacc = 0.5;
            this.Tdec = 0.5;
            this.StopVel = 0;
            this.S_para = 0.1;
            this.FactorError = 10;
            this.CircleParam = new CircleInterpolateParam();
            this.Rect2Param = new Rect2InterpolateParam();
            this.TrackData = new List<double[]>();
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveTrack = new WcsROI();
        }

        public MoveCommandParam TransformStanderCameraCoordToLaserCoord(userWcsCoordSystem coordSystem, userWcsPose laserAffinePose)
        {
            //HTuple Qx = new HTuple(0);
            //HTuple Qy = new HTuple(0);
            //HTuple Qz = new HTuple(0);

            //HTuple X = new HTuple(this.AxisParam.X);
            //HTuple Y = new HTuple(this.AxisParam.Y);
            //HTuple Z = new HTuple(this.AxisParam.Z); // 表示当前的相机坐标位置
            ///////////////////////////////////////
            //HOperatorSet.AffineTransPoint2d(coordSystem.GetCurrentHomMat2D(), X, Y, out Qx, out Qy); // 将理论的相机坐标点变换到当前坐标系下，然后再加上一个激光位姿，以获取激光的采集坐标
            /////////////////////////////
            //laserAffinePose.RigidTranslatePoint3D(Qx,  Qy, out Qx, out Qy);
            MoveCommandParam tempCommand = new MoveCommandParam();
            //tempCommand = this;
            //tempCommand.moveAxis = this.moveAxis;
            //tempCommand.moveSpeed = this.moveSpeed;
            //tempCommand.AxisParam = this.AxisParam;
            //tempCommand.AxisParam.X = Qx[0].D;
            //tempCommand.AxisParam.Y = Qy[0].D;
            //tempCommand.AxisParam.Z = Z[0].D;
            ////tempCommand.targetPosition = new double[3] { Qx[0].D + laserPose[0].D, Qy[0].D + laserPose[1].D, Z[0].D + coordSystem.CurrentPoint.z };//+ coordSystem.CurrentPoint.z + laserPose[2].D
            //tempCommand.poseInfo = this.poseInfo;
            return tempCommand;
        }
        public MoveCommandParam Affine2DCommandParam(userWcsCoordSystem coordSystem)
        {
            //HTuple Qx = new HTuple(0);
            //HTuple Qy = new HTuple(0);
            //HTuple Qz = new HTuple(0);
            //HTuple X = new HTuple(this.AxisParam.X);
            //HTuple Y = new HTuple(this.AxisParam.Y);
            //HTuple Z = new HTuple(this.AxisParam.Z); // 表示当前的相机坐标位置
            //                                         ///////////////////////////////////
            //HTuple homMat2dIdentity, homMat2dTranslate, homMat2dRotate;
            //HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            //HOperatorSet.HomMat2dTranslate(homMat2dIdentity, coordSystem.CurrentPoint.x - coordSystem.ReferencePoint.x, coordSystem.CurrentPoint.y - coordSystem.ReferencePoint.y, out homMat2dTranslate);// 
            //HOperatorSet.HomMat2dRotate(homMat2dTranslate, ((coordSystem.CurrentPoint.Angle - coordSystem.ReferencePoint.Angle) * Math.PI / 180) * 1, coordSystem.CurrentPoint.x, coordSystem.CurrentPoint.y, out homMat2dRotate);
            //HOperatorSet.AffineTransPoint2d(homMat2dRotate, X, Y, out Qx, out Qy); // 将理论点变换到当前坐标系下
            //                                                                       /////////////////////////
            MoveCommandParam tempCommand = new MoveCommandParam();
            tempCommand = this;
            //tempCommand.moveAxis = this.moveAxis;
            //tempCommand.moveSpeed = this.moveSpeed;
            //tempCommand.AxisParam = this.AxisParam;
            //tempCommand.AxisParam.X = Qx[0].D;
            //tempCommand.AxisParam.Y = Qy[0].D;
            //tempCommand.AxisParam.Z = Z[0].D + coordSystem.CurrentPoint.z;
            ////tempCommand.targetPosition = new double[3] { Qx[0].D, Qy[0].D, Z[0].D + coordSystem.CurrentPoint.z };
            //tempCommand.poseInfo = this.poseInfo;
            return tempCommand;
        }
        public MoveCommandParam AffineCommandParam(userWcsCoordSystem coordSystem)
        {
            if (coordSystem == null) return this;
            MoveCommandParam tempCommand = new MoveCommandParam();
            tempCommand = this;
            tempCommand.MoveAxis = this.MoveAxis;
            tempCommand.MoveSpeed = this.MoveSpeed;
            tempCommand.AxisParam = this.AxisParam.AffineAxisParam(coordSystem);
            tempCommand.AxisParam2 = this.AxisParam2.AffineAxisParam(coordSystem);
            tempCommand.PoseInfo = this.PoseInfo;
            tempCommand.MoveTrack = this.MoveTrack.AffineWcsROI(coordSystem.GetVariationHomMat2DNew());
            return tempCommand;
        }

        public override string ToString()
        {
            return "移动轴:" + this.MoveAxis.ToString() + "移动速度:" + this.MoveSpeed.ToString() + "加速度:" + this.Tacc.ToString() + "," + "减速度:" + this.Tdec.ToString();
        }



    }

    [Serializable]
    public struct CircleInterpolateParam
    {
        public double center_x;
        public double center_y;
        public double center_z;
        public double start_deg;
        public double end_deg;
        public double[] centerPosition;
        public double Radius;
        public ushort Dir;
        public int Count;
        public double OffsetValue_z;
        public double StartPointOffset;
    }
    [Serializable]
    public struct Rect2InterpolateParam
    {
        public double vector_x;
        public double vector_y;
        public int lineCount;
        public ushort InterpolateMode;
    }
    [Serializable]
    public struct TrackMoveParam
    {
        private List<double[]> listData;

    }

    [Serializable]
    public class CoordSysAxisPosParam
    {
        [DisplayNameAttribute("X轴")]
        public double X { get; set; }
        [DisplayNameAttribute("Y轴")]
        public double Y { get; set; }
        [DisplayNameAttribute("Z轴")]
        public double Z { get; set; }
        [DisplayNameAttribute("Theta/C/W轴")]
        public double Theta { get; set; }
        [DisplayNameAttribute("U/A轴")]
        public double U { get; set; }
        [DisplayNameAttribute("V/B轴")]
        public double V { get; set; }


        public CoordSysAxisPosParam()
        {

        }

        public CoordSysAxisPosParam(bool isInit = true)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Theta = 0;
            this.U = 0;
            this.V = 0;
        }
        public CoordSysAxisPosParam(double X, double Y, double Z, double Theta, double U, double V)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Theta = Theta;
            this.U = U;
            this.V = V;
        }

        public CoordSysAxisPosParam(double X, double Y, double Theta)
        {
            this.X = X;
            this.Y = Y;
            this.Z = 0;
            this.Theta = Theta;
            this.U = 0;
            this.V = 0;
        }
        public CoordSysAxisPosParam(double X, double Y, double Z, double Theta)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Theta = Theta;
            this.U = 0;
            this.V = 0;
        }
        public CoordSysAxisPosParam(string CoordSysName)
        {
            double x = 0, y = 0, z = 0, theta = 0, u = 0, v = 0;
            enCoordSysName sysName = enCoordSysName.CoordSys_0;
            Enum.TryParse(CoordSysName, out sysName);
            IMotionControl card = MotionCardManage.GetCard(CoordSysName);
            if (card != null)
            {
                card.GetAxisPosition(sysName, enAxisName.X轴, out x);
                card.GetAxisPosition(sysName, enAxisName.Y轴, out y);
                card.GetAxisPosition(sysName, enAxisName.Z轴, out z);
                card.GetAxisPosition(sysName, enAxisName.Theta轴, out theta);
                card.GetAxisPosition(sysName, enAxisName.U轴, out u);
                card.GetAxisPosition(sysName, enAxisName.V轴, out v);
            }
            ////////////////////////////////////////
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Theta = theta;
            this.U = u;
            this.V = v;
        }
        public CoordSysAxisPosParam(enCoordSysName CoordSysName)
        {
            double x = 0, y = 0, z = 0, theta = 0, u = 0, v = 0;
            IMotionControl card = MotionCardManage.GetCard(CoordSysName);
            if (card != null)
            {
                card.GetAxisPosition(CoordSysName, enAxisName.X轴, out x);
                card.GetAxisPosition(CoordSysName, enAxisName.Y轴, out y);
                card.GetAxisPosition(CoordSysName, enAxisName.Z轴, out z);
                card.GetAxisPosition(CoordSysName, enAxisName.Theta轴, out theta);
                card.GetAxisPosition(CoordSysName, enAxisName.U轴, out u);
                card.GetAxisPosition(CoordSysName, enAxisName.V轴, out v);
            }
            ////////////////////////////////////////
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Theta = theta;
            this.U = u;
            this.V = v;
        }

        public void UpdataAxisPosition(enCoordSysName CoordSysName) // 保存后有意义的参数和保存后没意义的参数
        {
            double x = 0, y = 0, z = 0, theta = 0, u = 0, v = 0;
            IMotionControl card = MotionCardManage.GetCard(CoordSysName);
            if (card == null) return;
            card.GetAxisPosition(CoordSysName, enAxisName.X轴, out x);
            card.GetAxisPosition(CoordSysName, enAxisName.Y轴, out y);
            card.GetAxisPosition(CoordSysName, enAxisName.Z轴, out z);
            card.GetAxisPosition(CoordSysName, enAxisName.Theta轴, out theta);
            card.GetAxisPosition(CoordSysName, enAxisName.U轴, out u);
            card.GetAxisPosition(CoordSysName, enAxisName.V轴, out v);
            ////////////////////////////////////////
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Theta = theta;
            this.U = u;
            this.V = v;
        }
        public double GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName) // 保存后有意义的参数和保存后没意义的参数
        {
            double pose= 0;
            IMotionControl card = MotionCardManage.GetCard(CoordSysName);
            if (card == null) return 0;
            card.GetAxisPosition(CoordSysName, axisName, out pose);
            return pose;
        }
        public userWcsPoint GetWcsPoint()
        {
            userWcsPoint wcsPoint = new userWcsPoint();
            wcsPoint.X = this.X;
            wcsPoint.Y = this.Y;
            wcsPoint.Z = this.Z;
            wcsPoint.Theta = this.Theta;
            wcsPoint.U = this.U;
            wcsPoint.V = this.V;
            wcsPoint.CamParams = null;
            return wcsPoint;
        }

        public double[] GetDrr()
        {
            return new double[6] { this.X, this.Y, this.Z, this.Theta, this.U, this.V };
        }

        public CoordSysAxisPosParam AffineAxisParam(userWcsCoordSystem coordSystem)
        {
            double Px, Py;
            CoordSysAxisPosParam axisParam = new CoordSysAxisPosParam();
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(coordSystem.ReferencePoint.X, coordSystem.ReferencePoint.Y, coordSystem.ReferencePoint.Angle,
                                         coordSystem.CurrentPoint.X, coordSystem.CurrentPoint.Y, coordSystem.CurrentPoint.Angle);
            Px = hHomMat2D.AffineTransPoint2d(this.X, this.Y, out Py);
            axisParam.X = Px;
            axisParam.Y = Py;
            axisParam.Z = this.Z;
            axisParam.U = this.U;
            axisParam.V = this.V;
            axisParam.Theta = this.Theta;
            return axisParam;
        }


    }



}
