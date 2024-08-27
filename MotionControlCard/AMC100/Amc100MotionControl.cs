using Common;
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
    public class Amc100_MotionControl : MotionBase
    {
        private int device_id = -1;
        private int max_Amplitude = 45;
        private double  run_Dir;
        enAxisName axis_name;

        #region 实现接口
        public override void EmgStopAxis()
        {
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return ;
            int eable = 0;
            amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1); // 使能电机
        }
        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName,out double position)
        {
            int pose;
            position = -1;
            int connect = -1;
            try
            {
                amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
                if (connect != 1) return;
                switch (axisName)
                {
                    case enAxisName.X轴:
                        amc100Library.AMC_getPosition(this.device_id, 0, out pose);
                        position = pose * 0.000001;

                        break;
                    case enAxisName.Y轴:
                        amc100Library.AMC_getPosition(this.device_id, 1, out pose);
                        position = pose * 0.000001;
 
                        break;
                    case enAxisName.Z轴:
                        amc100Library.AMC_getPosition(this.device_id, 2, out pose);
                        position = pose * 0.000001;

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
            int eable = 1;
            int pose = 0;
            // 设置移动速度
            int Amplitude = (int)Math.Abs(homSpeed) / 2;
            if (Amplitude > this.max_Amplitude) Amplitude = this.max_Amplitude;// 不改变频率，改变振幅来改变速度 
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    // amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1); // 使能电机
                    amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1);// 设置移动速度
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose, 1);// 设置移动点位
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动
                    break;
                case enAxisName.Y轴:
                    //  amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1); // 使能电机
                    amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose, 1);
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1);
                    break;
                case enAxisName.Z轴:
                    //  amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1); // 使能电机
                    amc100Library.AMC_controlAmplitude(this.device_id, 2, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 2, ref pose, 1);
                    amc100Library.AMC_controlMove(this.device_id, 2, ref eable, 1);
                    break;
                case enAxisName.XY轴:
                  //  amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1); // 使能电机
                  //  amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1); // 使能电机
                    amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1);// 设置移动速度
                    amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose, 1);// 设置移动点位
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose, 1);
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1);
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
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            int eable = 1;
            int pose = 0;
            // 设置移动速度
            int Amplitude = (int)Math.Abs(homSpeed) / 2;
            if (Amplitude > this.max_Amplitude) Amplitude = this.max_Amplitude;// 不改变频率，改变振幅来改变速度  
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return;
            /////
            switch (axisName)
            {
                case enAxisName.X轴:
                   // amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1); // 使能电机
                    amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1);// 设置移动速度
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose, 1);// 设置移动点位
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动
                    break;
                case enAxisName.Y轴:
                  //  amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1); // 使能电机
                    amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose, 1);
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1);
                    break;
                case enAxisName.Z轴:
                  //  amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1); // 使能电机
                    amc100Library.AMC_controlAmplitude(this.device_id, 2, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 2, ref pose, 1);
                    amc100Library.AMC_controlMove(this.device_id, 2, ref eable, 1);
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
        public override bool Init(DeviceConnectConfigParam name)
        {
            try
            {
                //this.InitParam(name);
                //this.cardType = enDeviceType.AMC100;
                //this.Name = name;
                //////////////////////
                //int eable = 1;
                //int connect = -1;
                //amc100Library.AMC_Connect("192.168.1.1", ref this.device_id);
                //amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
                //if (connect != 1) return false;
                //amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1); // 使能电机
                //amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1); // 使能电机
                //amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1); // 使能电机
                //this.connectState = true;
                return true;
            }
            catch
            {
                //this.connectState = false;
                return false;
            }
        }
        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            int eable = 1;
            int Amplitude = (int)Math.Abs(speed) / 2;
            if (Amplitude > this.max_Amplitude) Amplitude = this.max_Amplitude;// 不改变频率，改变振幅来改变速度  
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return;
            this.run_Dir = speed;
            ////////////////////////////////
            switch (axisName)
            {
                case enAxisName.X轴:
                    axis_name = enAxisName.X轴;
                   // amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1); // 使能电机
                   //  amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1); // 这里振幅为什么不能设置？
                    if (speed < 0)
                        amc100Library.AMC_controlContinousBkwd(this.device_id, 0, ref eable, 1);
                    else
                        amc100Library.AMC_controlContinousFwd(this.device_id, 0, ref eable, 1);
                    break;
                case enAxisName.Y轴:
                    axis_name = enAxisName.Y轴;
                    //   amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1); // 使能电机
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    if (speed < 0)
                        amc100Library.AMC_controlContinousBkwd(this.device_id, 1, ref eable, 1);
                    else
                        amc100Library.AMC_controlContinousFwd(this.device_id, 1, ref eable, 1);
                    break;
                case enAxisName.Z轴:
                    axis_name = enAxisName.Z轴;
                    //  amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1); // 使能电机
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 2, ref Amplitude, 1);
                    if (speed < 0)
                        amc100Library.AMC_controlContinousBkwd(this.device_id, 2, ref eable, 1);
                    else
                        amc100Library.AMC_controlContinousFwd(this.device_id, 2, ref eable, 1);
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
            int eable = 1;
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return;
            ////////////////////////////////
            switch (this.axis_name)
            {
                case enAxisName.X轴:
                    if (this.run_Dir > 0)
                        amc100Library.AMC_controlContinousBkwd(this.device_id, 0, ref eable, 1);
                    else
                        amc100Library.AMC_controlContinousFwd(this.device_id, 0, ref eable, 1);
                    break;
                case enAxisName.Y轴:
                    if (this.run_Dir > 0)
                        amc100Library.AMC_controlContinousBkwd(this.device_id, 1, ref eable, 1);
                    else
                        amc100Library.AMC_controlContinousFwd(this.device_id, 1, ref eable, 1);
                    break;
                case enAxisName.Z轴:
                    if (this.run_Dir > 0)
                        amc100Library.AMC_controlContinousBkwd(this.device_id, 2, ref eable, 1);
                    else
                        amc100Library.AMC_controlContinousFwd(this.device_id, 2, ref eable, 1);
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
        /// 多轴回零与单轴回零，多轴移动与单轴移动，留一个就可以了
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed"></param>
        /// <param name="axisPosition"></param>
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            int isMoveX, isMoveY, isMove;
            int pose1 = (int)(axisPosition.X * 1000000);
            int pose2 = (int)(axisPosition.Y * 1000000);
            // 设置移动速度
            int Amplitude = (int)Math.Abs(speed) / 2;
            if (Amplitude > this.max_Amplitude) Amplitude = this.max_Amplitude;// 不改变频率，改变振幅来改变速度   
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return;
            ///////////
            int eable = 1;
            switch (axisName)
            {
                case enAxisName.X轴:
                    amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1);
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1);// 设置移动速度
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose1, 1);// 设置移动点位
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动              
                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 0, out isMove);
                        if (isMove == 0) break;
                    }
                    break;
                case enAxisName.Y轴:
                    amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1);
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose1, 1);
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1);

                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 1, out isMove);
                        if (isMove == 0) break;
                    }
                    break;
                case enAxisName.Z轴:
                    amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1);
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 2, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 2, ref pose1, 1);
                    amc100Library.AMC_controlMove(this.device_id, 2, ref eable, 1);

                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 2, out isMove);
                        if (isMove == 0) break;
                    }
                    break;
                case enAxisName.XY轴:
                    amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1);
                    amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1);
                    //amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1); //振幅不能设置
                    //amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose1, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose2, 1);
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1); // 控制轴移动
                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 0, out isMoveX);
                        amc100Library.AMC_getStatusMoving(this.device_id, 0, out isMoveY);
                        if (isMoveX == 0 && isMoveY == 0) break;
                    }
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
            int isMoveX, isMoveY, isMove;
            int pose1 = (int)(motionCommandParam.AxisParam.X * 1000000);
            int pose2 = (int)(motionCommandParam.AxisParam.Y * 1000000);
            // 设置移动速度
            int Amplitude = (int)Math.Abs(motionCommandParam.MoveSpeed) / 2;
            if (Amplitude > this.max_Amplitude) Amplitude = this.max_Amplitude;// 不改变频率，改变振幅来改变速度   
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return;
            ///////////
            int eable = 1;
            switch (motionCommandParam.MoveAxis)
            {
                case enAxisName.X轴:
                    amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1);
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1);// 设置移动速度
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose1, 1);// 设置移动点位
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动              
                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 0, out isMove);
                        if (isMove == 0) break;
                    }
                    break;
                case enAxisName.Y轴:
                    amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1);
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose1, 1);
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1);

                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 1, out isMove);
                        if (isMove == 0) break;
                    }
                    break;
                case enAxisName.Z轴:
                    amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1);
                    //  amc100Library.AMC_controlAmplitude(this.device_id, 2, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 2, ref pose1, 1);
                    amc100Library.AMC_controlMove(this.device_id, 2, ref eable, 1);

                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 2, out isMove);
                        if (isMove == 0) break;
                    }
                    break;
                case enAxisName.XY轴:
                    amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1);
                    amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1);
                    //amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1); //振幅不能设置
                    //amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose1, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose2, 1);
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1); // 控制轴移动
                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 0, out isMoveX);
                        amc100Library.AMC_getStatusMoving(this.device_id, 0, out isMoveY);
                        if (isMoveX == 0 && isMoveY == 0) break;
                    }
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
            int eable = 1;
            int pose = (int)(axisPosition * 1000000); // 
            // 设置移动速度
            int Amplitude = (int)Math.Abs(speed) / 2;
            if (Amplitude > this.max_Amplitude) Amplitude = this.max_Amplitude;// 不改变频率，改变振幅来改变速度  
            int connect = -1;
            amc100Library.AMC_getStatusConnected(this.device_id, 0, out connect);
            if (connect != 1) return;
            /////////////
            int isMove;
            switch (axisName)
            {
                case enAxisName.X轴:
                    amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1);
                  //  amc100Library.AMC_controlAmplitude(this.device_id, 0, ref Amplitude, 1);// 设置移动速度
                    amc100Library.AMC_controlTargetPosition(this.device_id, 0, ref pose, 1);// 设置移动点位
                    amc100Library.AMC_controlMove(this.device_id, 0, ref eable, 1); // 控制轴移动              
                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 0, out isMove);
                        if (isMove == 0 ) break;
                    }
                    break;
                case enAxisName.Y轴:
                    amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1);
                  //  amc100Library.AMC_controlAmplitude(this.device_id, 1, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 1, ref pose, 1);
                    amc100Library.AMC_controlMove(this.device_id, 1, ref eable, 1);

                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 1, out isMove);
                        if (isMove == 0) break;
                    }
                    break;
                case enAxisName.Z轴:
                    amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1);
                  //  amc100Library.AMC_controlAmplitude(this.device_id, 2, ref Amplitude, 1);
                    amc100Library.AMC_controlTargetPosition(this.device_id, 2, ref pose, 1);
                    amc100Library.AMC_controlMove(this.device_id, 2, ref eable, 1);

                    while (true)
                    {
                        Application.DoEvents();
                        amc100Library.AMC_getStatusMoving(this.device_id, 2, out isMove);
                        if (isMove == 0) break;
                    }
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
            int eable = 0;
            amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1);
        }
        public override void UnInit()
        {
            try
            {
                int eable = 0;
                amc100Library.AMC_controlOutput(this.device_id, 0, ref eable, 1);
                amc100Library.AMC_controlOutput(this.device_id, 1, ref eable, 1);
                amc100Library.AMC_controlOutput(this.device_id, 2, ref eable, 1);
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
            throw new NotImplementedException();
        }
        public override void GetIoIntputBit(object param, int IoNum, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
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



        #endregion

    }
}
