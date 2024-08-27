using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using cszmcaux;


namespace MotionControlCard
{
    public class ZmcMotionCard : MotionBase, IMotionControl
    {
        private IntPtr g_handle;
        /// ////////
        public override void EmgStopAxis()
        {
            SlowDownStopAxis();
        }

        public override void GetAxisPosition(enCoordSysName CoordSysName,enAxisName axisName, out double position)
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            float pos = 0;
            position = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    zmcaux.ZAux_Direct_GetMpos(this.g_handle, 0, ref pos);
                    MirrorAxisCoord(CoordSysName, axisName, pos,out position);
                    break;
                case enAxisName.Y轴:
                    zmcaux.ZAux_Direct_GetMpos(this.g_handle, 1, ref pos);
                    MirrorAxisCoord(CoordSysName, axisName, pos, out position);
                    break;
                case enAxisName.Z轴:
                    zmcaux.ZAux_Direct_GetMpos(this.g_handle, 2, ref pos);
                    MirrorAxisCoord(CoordSysName, axisName, pos, out position);
                    // 因为Z轴每转一圈走5mm(丝杆的导程为5mm),电机每转一圈所需要脉冲数为10000，所以走1mm对应的脉冲数是2000，而光栅尺每运动1mm,发出的脉冲是10000，所以他们间的传动比是1：5
                    break;
            }
        }
        public override void GetIoIntputBit(object param, int IoNum, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
        }
        public override void GetIoOutputBit(object param, int IoNum, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
        }
        public override object GetParam(enParamType paramType, params object[] paramValue)
        {
            return null;
            //throw new NotImplementedException();
        }
        public override bool Init(DeviceConnectConfigParam param)
        {
            bool result = false;
            try
            {
                //this.InitParam(name);
                this.Name = param.DeviceName;
                //this.cardType = enDeviceType.ZMC;               
                string[] paramName = this.Name.Split(';'); // 第一个值表示连接类型
                switch (param.ConnectType)
                {
                    default:
                        return false;
                    case enUserConnectType.Network:
                        zmcaux.ZAux_OpenEth(param.IpAdress, out g_handle);
                        if ((long)g_handle != 0)
                        {
                            param.ConnectState = true;
                            return true;
                        }
                        else
                        {
                            param.ConnectState = false;
                            return false;
                        }
                    case enUserConnectType.USB:
                        return false;

                    case enUserConnectType.SerialPort:
                        return false;
                    case enUserConnectType.SerialNumber:
                        return false;
                }
            }
            catch(Exception ex)
            {
                LoggerHelper.Error(this.name + "打开失败", ex);
                param.ConnectState = false;
                result = false;
            }
            param.ConnectState = result;
            return result;
        }
        public override void JogAxisStart(enCoordSysName CoordSysName,enAxisName axisName, double speed)
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            double moveSpeed;
            float pos = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    MirrorAxisJog(CoordSysName, axisName, speed,out moveSpeed);
                    zmcaux.ZAux_Direct_GetMpos(this.g_handle, 0, ref pos);
                    if (Math.Abs(pos) > 50) return;
                    zmcaux.ZAux_Direct_SetSpeed(g_handle, 0, Convert.ToSingle(Math.Abs(moveSpeed)));
                    if (moveSpeed > 0)
                        zmcaux.ZAux_Direct_Singl_Vmove(g_handle, 0, 1);
                    else
                        zmcaux.ZAux_Direct_Singl_Vmove(g_handle, 0, -1);
                    break;
                case enAxisName.Y轴:
                    MirrorAxisJog(CoordSysName, axisName, speed, out moveSpeed);
                    zmcaux.ZAux_Direct_GetMpos(this.g_handle, 1, ref pos);
                    if (Math.Abs(pos) > 50) return;
                    zmcaux.ZAux_Direct_SetSpeed(g_handle, 1, Convert.ToSingle(Math.Abs(moveSpeed)));
                    if (moveSpeed > 0)
                        zmcaux.ZAux_Direct_Singl_Vmove(g_handle, 1, 1);
                    else
                        zmcaux.ZAux_Direct_Singl_Vmove(g_handle, 1, -1);
                    break;
                case enAxisName.Z轴:
                    MirrorAxisJog(CoordSysName, axisName, speed, out moveSpeed);
                    zmcaux.ZAux_Direct_SetSpeed(g_handle, 2, Convert.ToSingle(Math.Abs(moveSpeed*0.5)));
                    if (moveSpeed > 0)
                        zmcaux.ZAux_Direct_Singl_Vmove(g_handle, 2, 1);
                    else
                        zmcaux.ZAux_Direct_Singl_Vmove(g_handle, 2, -1);
                    break;
            }
        }
        public override void JogAxisStop()
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            zmcaux.ZAux_Direct_Singl_Cancel(g_handle, 0, 2);
            zmcaux.ZAux_Direct_Singl_Cancel(g_handle, 1, 2);
            zmcaux.ZAux_Direct_Singl_Cancel(g_handle, 2, 2);
        }
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0"); // 
            int run_state = 0;
            double x, y, z;
            switch (axisName)
            {
                case enAxisName.XY轴:
                    //if (axisPosition.Length < 2) throw new ArgumentException("axisPosition参数长度小于对应的运动轴数");
                    MirrorAxisCoord(CoordSysName, enAxisName.X轴, axisPosition.X,out x);
                    MirrorAxisCoord(CoordSysName, enAxisName.Y轴, axisPosition.Y, out y);
                    zmcaux.ZAux_Direct_Base(g_handle, 2, new int[2] { 0, 1 });
                    zmcaux.ZAux_Direct_SetSpeed(g_handle, 0, Convert.ToSingle(Math.Abs(speed)));
                    zmcaux.ZAux_Direct_MoveAbs(g_handle, 2, new float[2] { Convert.ToSingle(x), Convert.ToSingle(y) });
                    // 0:表示运动
                    do //判断回零运动是否完成; 
                    {
                        zmcaux.ZAux_Direct_GetIfIdle(g_handle, 0, ref run_state);
                        Application.DoEvents();
                    }
                    while (run_state == 0);
                    break;
                case enAxisName.XYZ轴:
                    //if (axisPosition.Length < 3) throw new ArgumentException("axisPosition参数长度小于对应的运动轴数");
                    MirrorAxisCoord(CoordSysName, enAxisName.X轴, axisPosition.X, out x);
                    MirrorAxisCoord(CoordSysName, enAxisName.Y轴, axisPosition.Y, out y);
                    MirrorAxisCoord(CoordSysName, enAxisName.Z轴, axisPosition.Z, out z);
                    zmcaux.ZAux_Direct_Base(g_handle, 3, new int[3] { 0, 1, 2 });
                    zmcaux.ZAux_Direct_SetSpeed(g_handle, 0, Convert.ToSingle(Math.Abs(speed)));
                    zmcaux.ZAux_Direct_MoveAbs(g_handle, 3, new float[3] { Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z) });
                    // 0:表示运动
                    do //判断回零运动是否完成; 
                    {
                        zmcaux.ZAux_Direct_GetIfIdle(g_handle, 0, ref run_state);
                        Application.DoEvents();
                    }
                    while (run_state == 0);
                    break;
            }
        }
        public override void MoveMultyAxis(MoveCommandParam motionCommandParam)
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0"); // 
            int run_state = 0;
            double x, y, z;
            switch (motionCommandParam.MoveAxis)
            {
                case enAxisName.XY轴:
                    //if (motionCommandParam.AxisParam.Length < 2) throw new ArgumentException("axisPosition参数长度小于对应的运动轴数");
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.X轴, motionCommandParam.AxisParam.X, out x);
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Y轴, motionCommandParam.AxisParam.Y, out y);
                    zmcaux.ZAux_Direct_Base(g_handle, 2, new int[2] { 0, 1 });
                    zmcaux.ZAux_Direct_SetSpeed(g_handle, 0, Convert.ToSingle(Math.Abs(motionCommandParam.MoveSpeed)));
                    zmcaux.ZAux_Direct_MoveAbs(g_handle, 2, new float[2] { Convert.ToSingle(x), Convert.ToSingle(y) });
                    // 0:表示运动
                    do //判断回零运动是否完成; 
                    {
                        zmcaux.ZAux_Direct_GetIfIdle(g_handle, 0, ref run_state);
                        Application.DoEvents();
                    }
                    while (run_state == 0);
                    break;
                case enAxisName.XYZ轴:
                   // if (motionCommandParam.AxisParam.Length < 3) throw new ArgumentException("axisPosition参数长度小于对应的运动轴数");
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.X轴, motionCommandParam.AxisParam.X, out x);
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Y轴, motionCommandParam.AxisParam.Y, out y);
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Z轴, motionCommandParam.AxisParam.Z, out z);
                    zmcaux.ZAux_Direct_Base(g_handle, 3, new int[3] { 0, 1, 2 });
                    zmcaux.ZAux_Direct_SetSpeed(g_handle, 0, Convert.ToSingle(Math.Abs(motionCommandParam.MoveSpeed)));
                    zmcaux.ZAux_Direct_MoveAbs(g_handle, 3, new float[3] { Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z) });
                    // 0:表示运动
                    do //判断回零运动是否完成; 
                    {
                        zmcaux.ZAux_Direct_GetIfIdle(g_handle, 0, ref run_state);
                        Application.DoEvents();
                    }
                    while (run_state == 0);
                    break;
            }
        }
        public override void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            int run_state = 0;
            double targetPose;
            int axisAdress = GetAxisAdress(CoordSysName, axisName); // 获取指定坐标系指定轴的地址
            MirrorAxisCoord(CoordSysName, axisName, axisPosition, out targetPose);
            zmcaux.ZAux_Direct_SetSpeed(g_handle, axisAdress, Convert.ToSingle(Math.Abs(speed)));
            zmcaux.ZAux_Direct_Singl_MoveAbs(this.g_handle, axisAdress, Convert.ToSingle(targetPose));
            // 0:表示运动
            do //判断回零运动是否完成; 
            {
                zmcaux.ZAux_Direct_GetIfIdle(g_handle, axisAdress, ref run_state);
                Application.DoEvents();
            }
            while (run_state == 0);

            //switch (axisName)
            //{
            //    case enAxisName.X轴:
            //        MirrorAxisCoord(CoordSysName, axisName, axisPosition, out targetPose);
            //        zmcaux.ZAux_Direct_SetSpeed(g_handle, 0, Convert.ToSingle(Math.Abs(speed)));
            //        zmcaux.ZAux_Direct_Singl_MoveAbs(this.g_handle, 0, Convert.ToSingle(targetPose));
            //        // 0:表示运动
            //        do //判断回零运动是否完成; 
            //        {
            //            zmcaux.ZAux_Direct_GetIfIdle(g_handle, 0, ref run_state);
            //            Application.DoEvents();
            //        }
            //        while (run_state == 0);
            //        break;
            //    case enAxisName.Y轴:
            //        MirrorAxisCoord(CoordSysName, axisName, axisPosition, out targetPose);
            //        zmcaux.ZAux_Direct_SetSpeed(g_handle, 1, Convert.ToSingle(Math.Abs(speed)));
            //        zmcaux.ZAux_Direct_Singl_MoveAbs(this.g_handle, 1, Convert.ToSingle(targetPose));
            //        // 0:表示运动
            //        do //判断回零运动是否完成; 
            //        {
            //            zmcaux.ZAux_Direct_GetIfIdle(g_handle, 1, ref run_state);
            //            Application.DoEvents();
            //        }
            //        while (run_state == 0);
            //        break;
            //    case enAxisName.Z轴:
            //        MirrorAxisCoord(CoordSysName, axisName, axisPosition, out targetPose);
            //        zmcaux.ZAux_Direct_SetSpeed(g_handle, 2, Convert.ToSingle(Math.Abs(speed)));
            //        zmcaux.ZAux_Direct_Singl_MoveAbs(this.g_handle, 2, Convert.ToSingle(targetPose));
            //        // 0:表示运动
            //        do //判断回零运动是否完成; 
            //        {
            //            zmcaux.ZAux_Direct_GetIfIdle(g_handle, 2, ref run_state);
            //            Application.DoEvents();
            //        }
            //        while (run_state == 0);
            //        break;
            //}
        }
        public override void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
        }
        public override void SetIoIntputBit(object param, int IoNum, bool state)
        {
            //throw new NotImplementedException();
        }
        public override void SetIoOutputBit(object param, int IoNum, bool state)
        {
            //throw new NotImplementedException();
        }
        public override void SetParam(enParamType paramType, params object[] paramValue)
        {
            //throw new NotImplementedException();
        }
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");

        }
        public override void SlowDownStopAxis()
        {
            if (this.g_handle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            zmcaux.ZAux_Direct_Singl_Cancel(g_handle, 0, 2);
            zmcaux.ZAux_Direct_Singl_Cancel(g_handle, 1, 2);
            zmcaux.ZAux_Direct_Singl_Cancel(g_handle, 2, 2);
        }
        public override void UnInit()
        {
            //if (this.g_handle.ToInt32() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            if (this.g_handle.ToInt64() > 0)
                zmcaux.ZAux_Close(g_handle);
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

        public override object ReadValue(enDataTypes dataType, string address, ushort length)
        {
            throw new NotImplementedException();
        }

        public override bool WriteValue(enDataTypes dataType, string address, object value)
        {
            throw new NotImplementedException();
        }


    }

}
