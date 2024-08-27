using AxMG17MotorLib;
using Common;
using MG17MotorLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotionControlCard
{
    [Serializable]
    public class AptMotionControl : MotionBase
    {
        AxMG17Motor axMG17Motor_x;
        AxMG17Motor axMG17Motor_y;
        form1 f1 = null;

        private int getAxisXState()
        {
            int plStatusBits = 0;
            axMG17Motor_x.LLGetStatusBits((int)HWCHANNEL.CHAN1_ID,ref plStatusBits);
            return plStatusBits;
        }
        private int getAxisYState()
        {
            int plStatusBits = 0;
            axMG17Motor_x.LLGetStatusBits((int)HWCHANNEL.CHAN2_ID, ref plStatusBits);
            return plStatusBits;
        }
        public override void EmgStopAxis()
        {
            axMG17Motor_x.StopImmediate((int)HWCHANNEL.CHAN1_ID);
            axMG17Motor_y.StopImmediate((int)HWCHANNEL.CHAN1_ID);
        }
        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            //bool isStart1 = false;
            //bool isStart2 = false;
            //this.axMG17Motor_x.GetCtrlStarted(ref isStart1);
            //this.axMG17Motor_y.GetCtrlStarted(ref isStart2);
            float pose = 0;
            position = 0;
            try
            {
                switch (axisName)
                {
                    case enAxisName.X轴:
                        this.axMG17Motor_x.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                        position = 54.85 - pose;
                        break;
                    case enAxisName.Y轴:
                        this.axMG17Motor_y.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                        position =37.5 - pose;
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
            try
            {
                switch (axisName)
                {
                    case enAxisName.X轴:
                        this.axMG17Motor_x.MoveHome((int)HWCHANNEL.CHAN1_ID, false);
                        break;
                    case enAxisName.Y轴:
                        this.axMG17Motor_y.MoveHome((int)HWCHANNEL.CHAN1_ID, false);
                        break;
                    case enAxisName.XY轴:
                        this.axMG17Motor_x.MoveHome((int)HWCHANNEL.CHAN1_ID, false);
                        this.axMG17Motor_y.MoveHome((int)HWCHANNEL.CHAN1_ID, false);
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
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            try
            {
                switch (axisName)
                {
                    case enAxisName.X轴:
                        this.axMG17Motor_x.MoveHome((int)HWCHANNEL.CHAN1_ID, false);
                        break;
                    case enAxisName.Y轴:
                        this.axMG17Motor_y.MoveHome((int)HWCHANNEL.CHAN1_ID, false);
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
        public override bool Init(DeviceConnectConfigParam name)
        {
            try
            {
                //this.InitParam(name);
                //this.cardType = enDeviceType.APT;
                this.Name = name.DeviceName;
                //////////
                f1 = new form1();
                axMG17Motor_x = new AxMG17Motor();
                axMG17Motor_y = new AxMG17Motor();
                ((System.ComponentModel.ISupportInitialize)(this.axMG17Motor_x)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.axMG17Motor_y)).BeginInit();
                // 一定要将这两个对象添加到窗体上才可以正常使用
                f1.Controls.Add(this.axMG17Motor_x);
                f1.Controls.Add(this.axMG17Motor_y);
                ((System.ComponentModel.ISupportInitialize)(this.axMG17Motor_x)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.axMG17Motor_y)).EndInit();
                axMG17Motor_x.HWSerialNum = 94870362;
                axMG17Motor_y.HWSerialNum = 94870361;
                ///////////////////////////////////
                axMG17Motor_x.StartCtrl();
                axMG17Motor_y.StartCtrl();
                axMG17Motor_x.EnableEventDlg(false);
                axMG17Motor_y.EnableEventDlg(false);
                /////////
               // this.connectState = true;
                //Task.Run(() =>
                //{
                //    while (true)
                //    {
                //        int BitX = getAxisXState();
                //        int BitY = getAxisYState();
                //        if (BitX == 0 || BitY == 0 )
                //            OnAxisMove(new EventArgs());
                //        Thread.Sleep(50);
                //    }
                //});
            }
            catch (Exception ee)
            {
                //this.connectState =false;
                return false;
            }
            return true;
        }
        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            float Acc = 1000;
            switch (axisName)
            {
                case enAxisName.X轴:
                    this.axMG17Motor_x.SetJogVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)Math.Abs(speed));
                    if (speed < 0)
                        this.axMG17Motor_x.MoveJog((int)HWCHANNEL.CHAN1_ID, (int)MOVEJOGDIR.JOG_FWD);
                    else
                        this.axMG17Motor_x.MoveJog((int)HWCHANNEL.CHAN1_ID, (int)MOVEJOGDIR.JOG_REV);
                    break;
                case enAxisName.Y轴:
                    this.axMG17Motor_y.SetJogVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)Math.Abs(speed));
                    if (speed < 0)
                        this.axMG17Motor_y.MoveJog((int)HWCHANNEL.CHAN1_ID, (int)MOVEJOGDIR.JOG_FWD);
                    else
                        this.axMG17Motor_y.MoveJog((int)HWCHANNEL.CHAN1_ID, (int)MOVEJOGDIR.JOG_REV);
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
            this.axMG17Motor_x.StopProfiled((int)HWCHANNEL.CHAN1_ID);
            this.axMG17Motor_y.StopProfiled((int)HWCHANNEL.CHAN1_ID);
        }
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            float pose1 = -1;
            float pose2 = -1;
            float Acc = 100;
            switch (axisName)
            {
                case enAxisName.X轴:
                    // this.axMG17Motor1.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                    this.axMG17Motor_x.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)speed);
                    this.axMG17Motor_x.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 54.85f - (float)axisPosition.X);
                    this.axMG17Motor_x.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                    // OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                    break;
                case enAxisName.Y轴:
                    // this.axMG17Motor2.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                    this.axMG17Motor_y.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)speed);
                    this.axMG17Motor_y.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 37.5f - (float)axisPosition.Y);
                    this.axMG17Motor_y.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                    // OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                    break;
                case enAxisName.XYZ轴:
                    ///// 移动X轴
                    this.axMG17Motor_x.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)speed);
                    this.axMG17Motor_x.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 54.85f - (float)axisPosition.X);  // 每一个实例关联一个APT硬件单元，为什么设置绝对位置时，在不同的实例上会出现覆盖？支持双轴联动吗？怎么样来实现，
                    this.axMG17Motor_x.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true); // 为什么只能一个一个轴的来移动，而不能同时联动？
                                                                                    ///// 移动Y轴
                    this.axMG17Motor_y.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)speed);
                    this.axMG17Motor_y.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 37.5f - (float)axisPosition.Y);
                    this.axMG17Motor_y.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                    this.axMG17Motor_x.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose1);
                    this.axMG17Motor_y.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose2);
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
        public override void MoveMultyAxis(MoveCommandParam motionCommandParam)
        {
            float pose1 = -1;
            float pose2 = -1;
            float Acc = 100;
            switch (motionCommandParam.MoveAxis)
            {
                case enAxisName.X轴:
                    // this.axMG17Motor1.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                    this.axMG17Motor_x.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)motionCommandParam.MoveSpeed);
                    this.axMG17Motor_x.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 54.85f - (float)motionCommandParam.AxisParam.X);
                    this.axMG17Motor_x.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                    // OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                    break;
                case enAxisName.Y轴:
                    // this.axMG17Motor2.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                    this.axMG17Motor_y.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)motionCommandParam.MoveSpeed);
                    this.axMG17Motor_y.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 37.5f - (float)motionCommandParam.AxisParam.Y);
                    this.axMG17Motor_y.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                    // OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                    break;
                case enAxisName.XYZ轴:
                    ///// 移动X轴
                    this.axMG17Motor_x.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)motionCommandParam.MoveSpeed);
                    this.axMG17Motor_x.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 54.85f - (float)motionCommandParam.AxisParam.X);  // 每一个实例关联一个APT硬件单元，为什么设置绝对位置时，在不同的实例上会出现覆盖？支持双轴联动吗？怎么样来实现，
                    this.axMG17Motor_x.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true); // 为什么只能一个一个轴的来移动，而不能同时联动？
                                                                                    ///// 移动Y轴
                    this.axMG17Motor_y.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)motionCommandParam.MoveSpeed);
                    this.axMG17Motor_y.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 37.5f - (float)motionCommandParam.AxisParam.Y);
                    this.axMG17Motor_y.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                    this.axMG17Motor_x.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose1);
                    this.axMG17Motor_y.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose2);
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
        public override void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            float pose = -1;
            float Acc = 100;
            switch (axisName)
            {
                case enAxisName.X轴:
                    // this.axMG17Motor1.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                    this.axMG17Motor_x.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)speed);
                    this.axMG17Motor_x.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 54.85f - (float)axisPosition);
                    this.axMG17Motor_x.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                   // OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                    break;
                case enAxisName.Y轴:
                    // this.axMG17Motor2.GetPosition((int)HWCHANNEL.CHAN1_ID, ref pose);
                    this.axMG17Motor_y.SetVelParams((int)HWCHANNEL.CHAN1_ID, 0, Acc, (float)speed);
                    this.axMG17Motor_y.SetAbsMovePos((int)HWCHANNEL.CHAN1_ID, 37.5f -(float)axisPosition);
                    this.axMG17Motor_y.MoveAbsolute((int)HWCHANNEL.CHAN1_ID, true);
                   // OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
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
        public override void SlowDownStopAxis()
        {
            this.axMG17Motor_x.StopProfiled((int)HWCHANNEL.CHAN1_ID);
            this.axMG17Motor_y.StopProfiled((int)HWCHANNEL.CHAN1_ID);
        }
        public override void UnInit()
        {
            try
            {          
                bool start_x = false;
                bool start_y = false;      
                this.axMG17Motor_x.GetCtrlStarted(ref start_x);
                this.axMG17Motor_y.GetCtrlStarted(ref start_y);
                if (start_x) this.axMG17Motor_x.StopCtrl();
                if (start_y) this.axMG17Motor_y.StopCtrl();
                if (f1 != null) f1.Close();  // 这里一定要关闭
            }
            catch
            {

            }
        }
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
            // throw new NotImplementedException();
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

    }

    public class form1 : Form
    {

    }

}
