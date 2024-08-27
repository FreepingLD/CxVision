using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace MotionControlCard
{
    /// <summary>
    /// 设置通信配置参数
    /// </summary>
    public class CommunicationConfigParam
    {
        public bool Active { get; set; }
        public enCoordSysName CoordSysName { get; set; }
        public enCoordSysName MapCoordSysName { get; set; }
        public enCommunicationCommand CommunicationCommand { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public enAxisReadWriteState AxisReadWriteState { get; set; }
        public enDataTypes DataType { get; set; }
        public int DataLength { get; set; }
        public double DataScale { get; set; }
        public string ReadValue { get; set; }
        public string WriteValue { get; set; }
        public object NONE { get; set; }


        public CommunicationConfigParam()
        {
            this.Active = true;
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MapCoordSysName = enCoordSysName.NONE;
            this.AxisReadWriteState = enAxisReadWriteState.ReadWrite;
            this.Address = "D1000";
            this.DataType = enDataTypes.Int;
            this.DataLength = 1;
            this.ReadValue = "";
            this.WriteValue = "";
            this.DataScale = 1;
        }
        public CommunicationConfigParam(enAxisName axisName, int AxisAddress)
        {
            this.Active = true;
            this.AxisReadWriteState = enAxisReadWriteState.ReadWrite;
            this.Address = "D1000";
            this.DataType = enDataTypes.Int;
            this.DataLength = 1;
            this.ReadValue = "";
            this.WriteValue = "";
        }



    }
    [Serializable]
    public enum enCommunicationCommand
    {
        NONE,
        X,
        Y,
        Z,
        Theta,
        U,
        V,
        W,
        OK,
        NG,
        Continue,
        Waiting,
        TriggerToPlc,
        TriggerFromPlc,
        StationNum,
        StationNumToPlc,
        Result,
        ResultToPlc,
        ResultToSocket, 
        ExChangePlat,
        ExChangePlatToPlc,
        FunctionNo,
        FunctionNoToPlc,
        GrabNo,
        GrabNoToPlc,
        ProductID,
        ProgramNo,
        TotalEdgeNum,
        DateTime,
        // 写入偏移值
        WriteOffset_X,    // 用于PLC的写地址
        WriteOffset_Y,    // 用于PLC的写地址
        WriteOffset_Theta,// 用于PLC的写地址
        WriteOffset_X2,   // 用于PLC的写地址
        WriteOffset_Y2,   // 用于PLC的写地址
        WriteOffset_Theta2,// 用于PLC的写地址
        WriteOffset_X3, // 用于PLC的写地址
        WriteOffset_Y3, // 用于PLC的写地址
        WriteOffset_Theta3, // 用于PLC的写地址
        WriteOffset_X4, // 用于PLC的写地址
        WriteOffset_Y4, // 用于PLC的写地址
        WriteOffset_Theta4, // 用于PLC的写地址
        WriteOffset_X5, // 用于PLC的写地址
        WriteOffset_Y5, // 用于PLC的写地址
        WriteOffset_Theta5, // 用于PLC的写地址
        WriteOffset_X6, // 用于PLC的写地址
        WriteOffset_Y6, // 用于PLC的写地址
        WriteOffset_Theta6, // 用于PLC的写地址
        WriteOffset_X7, // 用于PLC的写地址
        WriteOffset_Y7, // 用于PLC的写地址
        WriteOffset_Theta7, // 用于PLC的写地址
        WriteOffset_X8, // 用于PLC的写地址
        WriteOffset_Y8, // 用于PLC的写地址
        WriteOffset_Theta8, // 用于PLC的写地址
        Compensation_X, 
        Compensation_Y,
        Compensation_Z,
        Compensation_U,
        Compensation_V,
        Compensation_Theta, 
        Compensation_X2, 
        Compensation_Y2, 
        Compensation_Theta2, 
        Add_X, // 对位补偿 X
        Add_Y, // 对位补偿 Y
        Add_Z, // 对位补偿 Y
        Add_Theta, // 对位补偿 Theta
        Add_X2,
        Add_Y2,
        Add_Z2,
        Add_Theta2,
        LayoffAdd_X, // 下料补偿 X
        LayoffAdd_Y, // 下料补偿 Y
        LayoffAdd_Theta, // 下料补偿 Theta
        LayoffAdd_X2,
        LayoffAdd_Y2,
        LayoffAdd_Theta2,
        AlignCount,
        MemoryInfo,
        Path_X,
        Path_Y,
        Path_Z,
        Path_U,
        Path_V,
        Path_Theta,
        Reset,
        RotCali,
        Cali9Pt,
        //DateTime, // 读取当前时间
        X1,// 弃用
        Y1,// 弃用
        X2,// 弃用
        Y2,// 弃用
        X3,// 弃用
        Y3,// 弃用
        TriggerFromSocket, // 弃用
        TriggerToSocket,// 弃用
        SocketCommand, // 弃用
        WriteCompensation_X, // 弃用
        WriteCompensation_Y, // 弃用
        WriteCompensation_Theta, // 弃用
        WriteCompensation_X2, // 弃用
        WriteCompensation_Y2, // 弃用
        WriteCompensation_Theta2, // 弃用
    }

}
