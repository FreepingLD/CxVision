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
        X轴,
        Y轴,
        Z轴,
        Theta轴,
        U轴,
        V轴,
        W轴,
        X2轴,
        Y2轴,
        X3轴,
        Y3轴,
        XY轴,
        XZ轴,
        YZ轴,
        XYZ轴,
        XYTheta轴,
        XYZTheta轴,
        XYZU轴,
        XYZUV轴,
        XYZUVW轴,
        UVW轴,
        Compensation_X轴,
        Compensation_Y轴,
        Compensation_Z轴,
        Compensation_Theta轴,
        Compensation_U轴,
        Compensation_V轴,
        Compensation_XYTheta轴,
        Compensation_XYZUVW轴,
        Compensation_XYZTheta轴,
        OK,
        NG,
        Continue,
        TriggerToPlc,
        TriggerFromPlc,
        XY轴直线插补,
        XZ轴直线插补,
        YZ轴直线插补,
        XYZ轴直线插补,
        XY轴圆弧插补,
        XZ轴圆弧插补,
        YZ轴圆弧插补,
        XYZ轴圆弧插补,
        XY轴椭圆插补,
        XZ轴椭圆插补1,
        YZ轴椭圆插补,
        XYZ轴椭圆插补,
        XY螺旋线插补,
        XY同心圆插补,
        XY轴矩形插补,
        XYZ螺旋线插补,
        X轴轨迹运动,
        Y轴轨迹运动,
        Z轴轨迹运动,
        U轴轨迹运动,
        V轴轨迹运动,
        W轴轨迹运动,
        XY轴轨迹运动,
        XZ轴轨迹运动,
        YZ轴轨迹运动,
        XYZ轴轨迹运动,
        XYZU轴轨迹运动,
        XYZUV轴轨迹运动,
        XYZUVW轴轨迹运动,
        XY轴十字运动,
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

        public CoordSysAxisParam AxisParam;  // 用于直线扫描的第一点或点采集的采集点
        public CoordSysAxisParam AxisParam2;  // 用于直线扫描的第二点
        public CircleInterpolateParam CircleParam;
        public Rect2InterpolateParam Rect2Param;
        public List<double[]> TrackData;

        public WcsROI MoveTrack;
        public MoveCommandParam(bool isInit = true)
        {
            this.MoveType = enMoveType.点位运动;
            this.MoveAxis = enAxisName.XY轴;
            this.MoveSpeed = 100;
            this.AxisParam = new CoordSysAxisParam();
            this.AxisParam2 = new CoordSysAxisParam();
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
            this.AxisParam = new CoordSysAxisParam();
            this.AxisParam2 = new CoordSysAxisParam();
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
        public MoveCommandParam(enAxisName moveAxis, double moveSpeed, CoordSysAxisParam sysAxisParam)
        {
            this.MoveType = enMoveType.点位运动;
            this.MoveAxis = moveAxis;
            this.MoveSpeed = moveSpeed;
            this.AxisParam = sysAxisParam;
            this.AxisParam2 = new CoordSysAxisParam();
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
            tempCommand.MoveTrack = this.MoveTrack.AffineTransWcsROI(coordSystem.GetVariationHomMat2DNew());
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
    public class CoordSysAxisParam
    {
        [DisplayNameAttribute("X轴")]
        public double X { get; set; }
        [DisplayNameAttribute("Y轴")]
        public double Y { get; set; }
        [DisplayNameAttribute("Z轴")]
        public double Z { get; set; }
        [DisplayNameAttribute("Theta/C轴")]
        public double Theta { get; set; }
        [DisplayNameAttribute("U/A轴")]
        public double U { get; set; }
        [DisplayNameAttribute("V/B轴")]
        public double V { get; set; }

        public CoordSysAxisParam()
        {

        }

        public CoordSysAxisParam(bool isInit = true)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Theta = 0;
            this.U = 0;
            this.V = 0;
        }
        public CoordSysAxisParam(double X, double Y, double Z, double Theta, double U, double V)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Theta = Theta;
            this.U = U;
            this.V = V;
        }
        public CoordSysAxisParam(double X, double Y, double Theta)
        {
            this.X = X;
            this.Y = Y;
            this.Z = 0;
            this.Theta = Theta;
            this.U = 0;
            this.V = 0;
        }
        public CoordSysAxisParam(double X, double Y, double Z, double Theta)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Theta = Theta;
            this.U = 0;
            this.V = 0;
        }
        public CoordSysAxisParam(string CoordSysName)
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
        public CoordSysAxisParam(enCoordSysName CoordSysName)
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

        public CoordSysAxisParam AffineAxisParam(userWcsCoordSystem coordSystem)
        {
            double Px, Py;
            CoordSysAxisParam axisParam = new CoordSysAxisParam();
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
