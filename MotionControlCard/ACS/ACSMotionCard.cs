using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices; //for COMException class
using System.Threading;
using MotionControlCard;
using Common;
using System.Linq;
using SPIIPLUSCOM660Lib;
using System.Text;


namespace MotionControlCard
{
    [Serializable]
    public class AcsMotionControl : MotionBase
    {            
         private Channel Ch ;
        private int getAxisXState()
        {
            int plStatusBits = 0;
            int MotorState = this.Ch.GetMotorState(0);
            if (Convert.ToBoolean(MotorState & this.Ch.ACSC_MST_MOVE))
                plStatusBits = 1; // 1： 表示轴在移动
            return plStatusBits;
        }
        private int getAxisYState()
        {
            int plStatusBits = 0;
            int MotorState = this.Ch.GetMotorState(1);
            if (Convert.ToBoolean(MotorState & this.Ch.ACSC_MST_MOVE))
                plStatusBits = 1; // 1： 表示轴在移动
            return plStatusBits;
        }
        public override void EmgStopAxis()
        {
            try
            {
                this.Ch.KillAll();
            }
            catch
            {

            }
        }
        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            double pose = 0;
            position = 0;
            try
            {
                //if (!this.connectState)
                //{
                //    return;
                //}
                switch (axisName)
                {
                    case enAxisName.X轴:
                        pose = this.Ch.GetFPosition(0);
                        position =  pose;
                        break;
                    case enAxisName.Y轴:
                        pose = this.Ch.GetFPosition(1);
                        position =  pose;
                        break;
                    case enAxisName.Z轴:
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
            catch
            {

            }
        }
        public override void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            //if (!this.connectState)
            //{
            //    MessageBox.Show("运动控制器打开失败");
            //    return;
            //}
            try
            {
                switch (axisName)
                {
                    default:
                        this.Ch.Transaction("#8C"); // 回零程序
                        this.Ch.RunBuffer(8, "");
                        break;
                }
            }
            catch
            {
                LoggerHelper.Error(axisName.ToString() + ":回零失败");
            }
        }
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            //if (!this.connectState)
            //{
            //    MessageBox.Show("运动控制器打开失败");
            //    return;
            //}
            try
            {
                switch (axisName)
                {
                    case enAxisName.Y轴:
                    case enAxisName.X轴:
                        this.Ch.Transaction("#8C"); // 回零程序
                        this.Ch.RunBuffer(8, "");
                        break;                   
                    case enAxisName.Z轴:
                        this.Ch.Transaction("#8C"); // 回零程序
                        this.Ch.RunBuffer(8, "");
                        break;
                    case enAxisName.U轴:
                        this.Ch.Transaction("#8C"); // 回零程序
                        this.Ch.RunBuffer(8, "");
                        break;
                    default:
                        this.Ch.Transaction("#8C"); // 回零程序
                        this.Ch.RunBuffer(8, "");
                        break;
                }
            }
            catch
            {
                LoggerHelper.Error(axisName.ToString() + ":回零失败");
            }
        }
        public override bool Init(DeviceConnectConfigParam ConfigParam)
        {
            //try
            //{
            //    this.cardType = enDeviceType.ACS;
            //    this.Name = name;
            //    Ch = new SPIIPLUSCOM660Lib.Channel();
            //ACSC_SOCKET_STREAM_PORT: Protocol is TCP / IP in case of network connection; ACSC_SOCKET_DGRAM_PORT: UDP in case of point-to - point connection
            //    this.Ch.OpenCommEthernet("10.0.0.100", Ch.ACSC_SOCKET_STREAM_PORT);
            //    this.Ch.Enable(0);
            //    this.Ch.Enable(1);
            //    this.connectState = true;
            //    ////////               
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.Error(this.name + ":初始化失败", ex);
            //    this.connectState = false;
            //    return false;
            //}
            return true;
        }
        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            //if (!this.connectState) MessageBox.Show("运动控制器打开失败");
            switch (axisName)
            {
                case enAxisName.X轴:
                    this.Ch.Jog(this.Ch.ACSC_AMF_VELOCITY, 0, speed);
                    break;
                case enAxisName.Y轴:
                    this.Ch.Jog(this.Ch.ACSC_AMF_VELOCITY, 1, speed);
                    break;
                case enAxisName.Z轴:

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
        public override void JogAxisStop()
        {
            //if (!this.connectState)
            //{
            //    MessageBox.Show("运动控制器打开失败");
            //    return;
            //}
            this.Ch.KillAll();
        }
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            try
            {
                //if (!this.connectState)
                //{
                //    MessageBox.Show("运动控制器打开失败");
                //    return;
                //}
                switch (axisName)
                {
                    case enAxisName.XY轴:
                    case enAxisName.XYZ轴:
                        break;
                    default:
                        this.Ch.ToPoint(this.Ch.ACSC_AMF_SYNCHRONOUS, 0, axisPosition.X);
                        this.Ch.ToPoint(this.Ch.ACSC_AMF_SYNCHRONOUS, 1, axisPosition.Y);
                        while (getAxisXState() == 1 || getAxisYState() == 1)
                        {
                            Application.DoEvents();
                        }
                        break;
                }
            }
            catch
            {

            }
        }
        public override void MoveMultyAxis(MoveCommandParam motionCommandParam)
        {
            try
            {
                //if (!this.connectState)
                //{
                //    MessageBox.Show("运动控制器打开失败");
                //    return;
                //}
                switch (motionCommandParam.MoveAxis)
                {
                    case enAxisName.XY轴:
                    case enAxisName.XYZ轴:
                        break;
                    default:
                        this.Ch.ToPoint(this.Ch.ACSC_AMF_SYNCHRONOUS, 0, motionCommandParam.AxisParam.X);
                        this.Ch.ToPoint(this.Ch.ACSC_AMF_SYNCHRONOUS, 1, motionCommandParam.AxisParam.Y);
                        while (getAxisXState() == 1 || getAxisYState() == 1)
                        {
                            Application.DoEvents();
                        }
                        break;
                }
            }
            catch
            {

            }
        }
        public override void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            try
            {
                //if (!this.connectState)
                //{
                //    MessageBox.Show("运动控制器打开失败");
                //    return;
                //}
                switch (axisName)
                {
                    case enAxisName.X轴:
                        this.Ch.ToPoint(this.Ch.ACSC_AMF_SYNCHRONOUS, 0, axisPosition);
                        while (getAxisXState() == 1)
                        {
                            Application.DoEvents();
                        }
                        break;
                    case enAxisName.Y轴:
                        this.Ch.ToPoint(this.Ch.ACSC_AMF_SYNCHRONOUS, 1, axisPosition);
                        while (getAxisYState() == 1)
                        {
                            Application.DoEvents();
                        }
                        break;
                }
            }
            catch
            {

            }
        }
        public override void SlowDownStopAxis()
        {
            //if (!this.connectState)
            //{
            //    MessageBox.Show("运动控制器打开失败");
            //    return;
            //}
            this.Ch.KillAll();
        }
        public override void UnInit()
        {
            try
            {
                //if (!this.connectState)
                //{
                //    //MessageBox.Show("运动控制器打开失败");
                //    return;
                //}
                this.Ch.CloseComm();
            }
             catch
            {

            }
        }

        public override void SetParam(enParamType paramType, params object[] paramValue)
        {
            //throw new NotImplementedException();
        }
        public override object GetParam(enParamType paramType, params object[] paramValue)
        {
            return null;
            //throw new NotImplementedException();
        }

        #region 运动控制卡IO操作
        public override void SetIoOutputBit(object param, int IoNum, bool state)
        {
            //throw new NotImplementedException();
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



        #endregion

        #region PLC 数据读取操作

        public override object ReadValue(enDataTypes dataType, string address, int length)
        {
            throw new NotImplementedException();
        }

        public override bool WriteValue(enDataTypes dataType, string address, object value)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
