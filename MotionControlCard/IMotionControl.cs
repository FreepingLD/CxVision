using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HalconDotNet;
using Common;


namespace MotionControlCard
{
    public interface IMotionControl
    {
        event PoseInfoEventHandler AxisINPose;
        event AxisMoveEventHandler AxisMoveEvent;
        string Name { get; set; }
        AxisCalibration CalibrateParam { get; set; }
        CoordSysConfigParam CoordSysConfigParam { get; set; }
        bool Init(DeviceConnectConfigParam param);
        void UnInit();
        void MoveSingleAxis(enCoordSysName CoordSysName,enAxisName axisName, double speed, double axisPosition);
        void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition);
        void MoveMultyAxis(MoveCommandParam motionCommandParam);
        void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed);
        void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed);
        void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position);
        void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed);
        void JogAxisStop();
        void SlowDownStopAxis();
        void EmgStopAxis();

        object ReadValue(enDataTypes dataType, string address, int length);
        bool WriteValue(enDataTypes dataType, string address, object value);


        bool WaiteValue(enDataTypes dataType, string address, object waitValue, int readInterval, int waitTimeout);

        void SetIoOutputBit(object param, int IoNum, bool value);
        void GetIoOutputBit(object param, int IoNum, out bool value);
        void SetIoIntputBit(object param, int IoNum, bool value);
        void GetIoIntputBit(object param, int IoNum, out bool value);


        void WriteIoOutputBit(enIoPortType ioPortType, int IoPort, params object[] value);
        void ReadIoOutputBit(enIoPortType ioPortType, int IoPort, out object[] value);
        void ReadIoIntputBit(enIoPortType ioPortType, int IoPort, out object[] value);
        void WriteIoOutputGroup(enIoPortType ioPortType, int IoPort, params object[] value);
        void ReadIoOutputGroup(enIoPortType ioPortType, int IoPort, out object[] value);
        void ReadIoIntputGroup(enIoPortType ioPortType, int IoPort, out object[] value);

        // 控制器IO时需要指定IO端口的类型,Io信号的输出方式,Io端口号,及IO端口输出电平值
       // void WriteIoOutputBit(enIoPortType ioPortType, enIoPluseOutputMode ioOutputMode, int IoPort, params object[] value);


        void SetParam(enParamType paramType, params object[] paramValue);
        object GetParam(enParamType paramType, params object[] paramValue);


    }

    public struct IoParam
    {
        public enIoPortType IoPortType;
        public enIoOutputMode IoOutputMode;
        public int IoPort; // IO 端口
        public int IoValue;  // Io 值
        public int IoReverseTime;  // Io输出持续时间


        public IoParam(bool isInit = true)
        {
            this.IoPortType = enIoPortType.通用Io端口;
            this.IoOutputMode = enIoOutputMode.脉冲输出;
            this.IoPort = 1; // IO 端口
            this.IoValue = 1;  // Io 值
            this.IoReverseTime = 100;  // Io输出持续时间
        }

    }


}
