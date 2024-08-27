using Common;
using HalconDotNet;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MotionControlCard
{
    [Serializable]
    public class MotionBase : IMotionControl
    {
        public event PoseInfoEventHandler AxisINPose;
        public event AxisMoveEventHandler AxisMoveEvent;

        private AxisCalibration calibrateFile;
        private CoordSysConfigParam _CoordSysParam;
        protected string name;

        protected DeviceConnectConfigParam ConnectConfigParam;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public AxisCalibration CalibrateParam
        {
            get
            {
                return calibrateFile;
            }

            set
            {
                calibrateFile = value;
            }
        }

        public CoordSysConfigParam CoordSysConfigParam
        {
            get
            {
                return _CoordSysParam;
            }
            set
            {
                value = _CoordSysParam;
            }
        }
        public DeviceConnectConfigParam ConnectParam
        {
            get;
            set;
        }

        protected void InitParam(string name)
        {
            this.calibrateFile = new AxisCalibration().Read(Application.StartupPath + "\\" + "机台校准" + "\\" + name + ".txt");
        }
        protected virtual void OnPoseChanged(PoseInfoEventArgs e)
        {
            if (AxisINPose != null)
                AxisINPose(this, e);
        }
        protected virtual void OnAxisMove(EventArgs e)
        {
            if (AxisMoveEvent != null)
                AxisMoveEvent(this, e);
        }


        protected void CalculateCmpPoint(double x1, double y1, double x2, double y2, double cmpDist, out double[] cmp_x, out double[] cmp_y)
        {
            cmp_x = new double[0];
            cmp_y = new double[0];
            double phi;
            if (x2 == x1) // 分母不能为0
            {
                if (cmpDist > 0)
                    phi = Math.PI * 0.5;
                else
                    phi = Math.PI * -0.5;
            }
            else
            {
                if (y2 == y1)
                {
                    if (cmpDist > 0)
                        phi = Math.PI;
                    else
                        phi = Math.PI * -1;
                }
                else
                    phi = Math.Atan2(y2 - y1, x2 - x1);
            }
            double lineLength = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            int count = (int)(lineLength / Math.Abs(cmpDist)) + 1;
            cmp_x = new double[count];
            cmp_y = new double[count];
            for (int i = 0; i < count; i++)
            {
                cmp_x[i] = x1 + Math.Cos(phi) * cmpDist * i;
                cmp_y[i] = y1 + Math.Sin(phi) * cmpDist * i;
            }
        }

        protected void MirrorAxisCoord(enCoordSysName CoordSysName, CoordSysAxisParam axisPosition, out double x, out double y, out double z, out double theta, out double u, out double v)
        {
            x = 0;
            y = 0;
            z = 0;
            theta = 0;
            v = 0;
            u = 0;
            CoordSysConfigParam coordSysParam_x = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
            CoordSysConfigParam coordSysParam_y = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
            CoordSysConfigParam coordSysParam_z = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
            CoordSysConfigParam coordSysParam_theta = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Theta轴);
            CoordSysConfigParam coordSysParam_u = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.U轴);
            CoordSysConfigParam coordSysParam_v = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.V轴);
            if (coordSysParam_x.InvertAxisFeedBack && !coordSysParam_x.InvertAxisCommandPos)
                x = axisPosition.X * -1 * coordSysParam_x .PulseEquiv/ coordSysParam_x.TransmissionRatio;
            else
                x = axisPosition.X * coordSysParam_x.PulseEquiv / coordSysParam_x.TransmissionRatio;
            /////////
            if (coordSysParam_y.InvertAxisFeedBack && !coordSysParam_y.InvertAxisCommandPos)
                y = axisPosition.Y * -1 * coordSysParam_y.PulseEquiv / coordSysParam_y.TransmissionRatio;
            else
                y = axisPosition.Y * coordSysParam_y.PulseEquiv / coordSysParam_y.TransmissionRatio;
            /////////
            if (coordSysParam_z.InvertAxisFeedBack && !coordSysParam_z.InvertAxisCommandPos)
                z = axisPosition.Z * -1 * coordSysParam_z.PulseEquiv / coordSysParam_z.TransmissionRatio;
            else
                z = axisPosition.Z * coordSysParam_z.PulseEquiv / coordSysParam_z.TransmissionRatio;
            /////////
            if (coordSysParam_theta.InvertAxisFeedBack && !coordSysParam_theta.InvertAxisCommandPos)
                theta = axisPosition.Theta * -1 * coordSysParam_theta.PulseEquiv / coordSysParam_theta.TransmissionRatio;
            else
                theta = axisPosition.Theta * coordSysParam_theta.PulseEquiv / coordSysParam_theta.TransmissionRatio;
            /////////
            if (coordSysParam_u.InvertAxisFeedBack && !coordSysParam_u.InvertAxisCommandPos)
                u = axisPosition.U * -1 * coordSysParam_u.PulseEquiv / coordSysParam_u.TransmissionRatio;
            else
                u = axisPosition.U * coordSysParam_u.PulseEquiv / coordSysParam_u.TransmissionRatio;
            /////////
            if (coordSysParam_v.InvertAxisFeedBack && !coordSysParam_v.InvertAxisCommandPos)
                v = axisPosition.V * -1 * coordSysParam_v.PulseEquiv / coordSysParam_v.TransmissionRatio;
            else
                v = axisPosition.V * coordSysParam_v.PulseEquiv / coordSysParam_v.TransmissionRatio;
        }

        protected void MirrorAxisCoord(enCoordSysName CoordSysName, enAxisName axisName, double sourceValue, out double adjValue)
        {
            CoordSysConfigParam coordSysParam;
            switch (axisName)
            {
                case enAxisName.X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam.InvertAxisFeedBack && !coordSysParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    break;
                case enAxisName.Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam.InvertAxisFeedBack && !coordSysParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    break;
                case enAxisName.Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam.InvertAxisFeedBack && !coordSysParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    break;
                case enAxisName.U轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.U轴);
                    if (coordSysParam.InvertAxisFeedBack && !coordSysParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    break;
                case enAxisName.V轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.V轴);
                    if (coordSysParam.InvertAxisFeedBack && !coordSysParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    break;
                case enAxisName.Theta轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysParam.InvertAxisFeedBack && !coordSysParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysParam.PulseEquiv / coordSysParam.TransmissionRatio;
                    break;
                default:
                    adjValue = sourceValue;
                    break;
            }
        }

        public double MirrorAxisCoord(CoordSysConfigParam coordSysConfigParam, double sourceValue)
        {
            double adjValue = 0;
            switch (coordSysConfigParam.AxisName)
            {
                case enAxisName.X轴:
                    if (coordSysConfigParam.InvertAxisFeedBack && !coordSysConfigParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    break;
                case enAxisName.Y轴:
                    if (coordSysConfigParam.InvertAxisFeedBack && !coordSysConfigParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    break;
                case enAxisName.Z轴:
                    if (coordSysConfigParam.InvertAxisFeedBack && !coordSysConfigParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    break;
                case enAxisName.U轴:
                    if (coordSysConfigParam.InvertAxisFeedBack && !coordSysConfigParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    break;
                case enAxisName.V轴:
                    if (coordSysConfigParam.InvertAxisFeedBack && !coordSysConfigParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    break;
                case enAxisName.Theta轴:
                    if (coordSysConfigParam.InvertAxisFeedBack && !coordSysConfigParam.InvertAxisCommandPos)
                        adjValue = sourceValue * -1 * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    else
                        adjValue = sourceValue * coordSysConfigParam.PulseEquiv / coordSysConfigParam.TransmissionRatio;
                    break;
                default:
                    adjValue = sourceValue;
                    break;
            }
            return adjValue;
        }

        protected void MirrorAxisJog(enCoordSysName CoordSysName, enAxisName axisName, double sourceValue, out double adjValue)
        {
            CoordSysConfigParam coordSysParam;
            switch (axisName)
            {
                case enAxisName.X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.U轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.U轴);
                    if (coordSysParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.V轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.V轴);
                    if (coordSysParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.Theta轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                default:
                    adjValue = sourceValue;
                    break;
            }
        }
        protected double MirrorAxisJog(CoordSysConfigParam coordSysConfigParam, double sourceValue)
        {
            double adjValue = 0;
            switch (coordSysConfigParam.AxisName)
            {
                case enAxisName.X轴:
                    if (coordSysConfigParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.Y轴:
                    if (coordSysConfigParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.Z轴:
                    if (coordSysConfigParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.U轴:
                    if (coordSysConfigParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.V轴:
                    if (coordSysConfigParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                case enAxisName.Theta轴:
                    if (coordSysConfigParam.InvertJogAxis)
                        adjValue = sourceValue * -1;
                    else
                        adjValue = sourceValue;
                    break;
                default:
                    adjValue = sourceValue;
                    break;
            }
            return adjValue;
        }
        protected int GetAxisAdress(enCoordSysName coordSysName, enAxisName axisName)
        {
            foreach (var item in CoordSysConfigParamManger.Instance.CoordSysConfigParamList)
            {
                if (item.CoordSysName == coordSysName && item.AxisName == axisName)
                    return Convert.ToInt32(item.AxisAddress);
            }
            return -1;
        }


        public virtual bool Init(DeviceConnectConfigParam param)
        {
            throw new NotImplementedException();
        }

        public virtual void UnInit()
        {
            throw new NotImplementedException();
        }

        public virtual void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            throw new NotImplementedException();
        }

        public virtual void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            throw new NotImplementedException();
        }

        public virtual void MoveMultyAxis(MoveCommandParam motionCommandParam)
        {
            throw new NotImplementedException();
        }

        public virtual void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            throw new NotImplementedException();
        }

        public virtual void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            throw new NotImplementedException();
        }

        public virtual void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            throw new NotImplementedException();
        }

        public virtual void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            throw new NotImplementedException();
        }

        public virtual void JogAxisStop()
        {
            throw new NotImplementedException();
        }

        public virtual void SlowDownStopAxis()
        {
            throw new NotImplementedException();
        }

        public virtual void EmgStopAxis()
        {
            throw new NotImplementedException();
        }

        public virtual object ReadValue(enDataTypes dataType, string address, int length)
        {
            throw new NotImplementedException();
        }

        public virtual bool WriteValue(enDataTypes dataType, string address, object value)
        {
            throw new NotImplementedException();
        }
        public virtual bool WaiteValue(enDataTypes dataType, string address, object waitValue, int readInterval, int waitTimeout)
        {
            throw new NotImplementedException();
        }

        public virtual void SetIoOutputBit(object param, int IoNum, bool value)
        {
            throw new NotImplementedException();
        }

        public virtual void GetIoOutputBit(object param, int IoNum, out bool value)
        {
            throw new NotImplementedException();
        }

        public virtual void SetIoIntputBit(object param, int IoNum, bool value)
        {
            throw new NotImplementedException();
        }

        public virtual void GetIoIntputBit(object param, int IoNum, out bool value)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteIoOutputBit(enIoPortType ioPortType, int IoPort, params object[] value)
        {
            throw new NotImplementedException();
        }

        public virtual void ReadIoOutputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NotImplementedException();
        }

        public virtual void ReadIoIntputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteIoOutputGroup(enIoPortType ioPortType, int IoPort, params object[] value)
        {
            throw new NotImplementedException();
        }

        public virtual void ReadIoOutputGroup(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NotImplementedException();
        }

        public virtual void ReadIoIntputGroup(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NotImplementedException();
        }

        public virtual void SetParam(enParamType paramType, params object[] paramValue)
        {
            throw new NotImplementedException();
        }

        public virtual object GetParam(enParamType paramType, params object[] paramValue)
        {
            throw new NotImplementedException();
        }

        public virtual object ReadValue(enDataTypes dataType, string address, ushort length)
        {
            throw new NotImplementedException();
        }


    }
}
