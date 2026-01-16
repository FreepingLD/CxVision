using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace MotionControlCard
{
    [Serializable]
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

    /// <summary>
    /// 给每一个枚举变量都赋值一个ID绑定，这样即使顺序乱了，也不会改变引用
    /// </summary>
    [Serializable]
    public enum enCommunicationCommand
    {
        NONE = 0,
        X = 1,                         // X 轴坐标
        Y = 2,                         // Y 轴坐标
        Z = 3,                         // Z 轴坐标
        Theta = 4,                     // Theta 轴坐标
        U = 5,                         // U 轴坐标
        V = 6,                         // V 轴坐标
        W = 7,                         // W 轴坐标
        OK = 8,                        // 结果OK
        NG = 9,                        // 结果NG
        Continue = 10,                 // 继续
        Waiting = 11,                  // 等待
        TriggerToPlc = 12,             // 发送触发信号给PLC
        TriggerFromPlc = 13,           // 接收PLC触发信号
        StationNum = 14,               // 工位信息 
        StationNumToPlc = 15,          // 工位信息
        Result = 16,                   // PLC 结果信息
        ResultToPlc = 17,              // 发送给PLC结果信息
        ResultToSocket = 18,           // 弃用
        ExChangePlat = 19,             // 换盘信号 
        ExChangePlatToPlc = 20,        // 换盘信号 
        FunctionNo = 21,               // 功能号 
        FunctionNoToPlc = 22,          // 功能号
        GrabNo = 23,                   // 拍照位
        GrabNoToPlc = 24,              // 拍照位
        ProductID = 25,                // 产品ID
        ProgramNo = 26,                // 程序ID
        ProgramNoToPlc = 27,           // 程序ID
        DetectEdge = 28,               // 产品边缘号
        DetectEdgeToPlc = 29,          // 产品边缘号
        TotalEdgeNum = 30,             // 产品总边缘 ，弃用
        DateTime = 31,                 // 获取系统时间
        JawGrabType = 32,              // 夹抓类型
        JawGrabTypeToPlc = 33,         // 夹抓类型
        OnLine = 34,  // 在线
        // 写入偏移值
        WriteOffset_X1 = 44,          // 用于PLC的写地址
        WriteOffset_Y1 = 45,         // 用于PLC的写地址
        WriteOffset_Theta1 = 46,    // 用于PLC的写地址
        WriteOffset_X2 = 47,      // 用于PLC的写地址
        WriteOffset_Y2 = 48,     // 用于PLC的写地址
        WriteOffset_Theta2 = 49,// 用于PLC的写地址
        WriteOffset_X3 = 50,   // 用于PLC的写地址
        WriteOffset_Y3 = 51,      // 用于PLC的写地址
        WriteOffset_Theta3 = 52,  // 用于PLC的写地址
        WriteOffset_X4 = 53,      // 用于PLC的写地址
        WriteOffset_Y4 = 54,      // 用于PLC的写地址
        WriteOffset_Theta4 = 55, // 用于PLC的写地址
        WriteOffset_X5 = 56,  // 用于PLC的写地址
        WriteOffset_Y5 = 57, // 用于PLC的写地址
        WriteOffset_Theta5 = 58, // 用于PLC的写地址
        WriteOffset_X6 = 59,  // 用于PLC的写地址
        WriteOffset_Y6 = 60, // 用于PLC的写地址
        WriteOffset_Theta6 = 61, // 用于PLC的写地址
        WriteOffset_X7 = 62,  // 用于PLC的写地址
        WriteOffset_Y7 = 63, // 用于PLC的写地址
        WriteOffset_Theta7 = 64, // 用于PLC的写地址
        WriteOffset_X8 = 65,  // 用于PLC的写地址
        WriteOffset_Y8 = 66, // 用于PLC的写地址
        WriteOffset_Theta8 = 67, // 用于PLC的写地址
        WriteOffset_X9 = 68,  // 用于PLC的写地址
        WriteOffset_Y9 = 69, // 用于PLC的写地址
        WriteOffset_Theta9 = 70, // 用于PLC的写地址
        WriteOffset_X10 = 71,  // 用于PLC的写地址
        WriteOffset_Y10 = 72, // 用于PLC的写地址
        WriteOffset_Theta10 = 73, // 用于PLC的写地址
        WriteOffset_X11 = 74,  // 用于PLC的写地址
        WriteOffset_Y11 = 75, // 用于PLC的写地址
        WriteOffset_Theta11 = 76, // 用于PLC的写地址
        WriteOffset_X12 = 77,  // 用于PLC的写地址
        WriteOffset_Y12 = 78, // 用于PLC的写地址
        WriteOffset_Theta12 = 79, // 用于PLC的写地址
        ImagePath = 80, // 用于PLC的写地址
        WriteUpTolerance = 81, // 用于PLC的写地址
        WriteDownTolerance = 82, // 用于PLC的写地址
        WriteUpTolerance2 = 83, // 用于PLC的写地址
        WriteDownTolerance2 = 84, // 用于PLC的写地址
        WriteUpTolerance3 = 85, // 用于PLC的写地址
        WriteDownTolerance3 = 86, // 用于PLC的写地址
        WriteUpTolerance4 = 87, // 用于PLC的写地址
        WriteDownTolerance4 = 88, // 用于PLC的写地址
        Polarity = 89,
        ColCount = 90,
        RowAngle = 91,
        Write_Result1 = 92,
        Write_Result2 = 93,
        Write_Result3 = 94,
        Write_Result4 = 95,
        Write_Result5 = 96,
        Write_Result6 = 97,
        Write_Result7 = 98,
        Write_Result8 = 99,
        WriteOffset_X = 100,          // 用于PLC的写地址
        WriteOffset_Y = 101,
        WriteOffset_Z = 102, // 用于PLC的写地址
        WriteOffset_Theta = 103,    // 用于PLC的写地址
        Write_Result = 104,
        ColDist = 105,
        RFIDInfo = 106,
        Compensation_X = 107,           // 对位补偿 X  
        Compensation_Y = 108,           // 对位补偿 Y
        Compensation_Z = 109,           // 对位补偿 Z
        Compensation_U = 110,           // 对位补偿 U
        Compensation_V = 111,           // 对位补偿 V
        Compensation_W = 112,           // 对位补偿 W
        Compensation_Theta = 113,       // 对位补偿 Theta  
        Compensation_X2 = 114,          // 对位补偿 X2  
        Compensation_Y2 = 115,          // 对位补偿 Y2  
        Compensation_Theta2 = 116,      // 对位补偿 Theta2
        Compensation_U2 = 117,          // 对位补偿 U2 
        Compensation_V2 = 118,          // 对位补偿 V2 
        Compensation_W2 = 119,          // 对位补偿 W2 
        LayoffAdd_X = 120, // 下料补偿 X
        LayoffAdd_Y = 121, // 下料补偿 Y
        LayoffAdd_Theta = 122, // 下料补偿 Theta
        LayoffAdd_X2 = 123,
        LayoffAdd_Y2 = 124,
        LayoffAdd_Theta2 = 125,
        AlignCount = 126,
        MemoryInfo = 127,
        Reset = 128,
        RotCali = 129,
        Cali9Pt = 130,  //
        CommandSendTime = 131,
        ResponseTime = 132,
        RowIndex = 133,
        DieIndex = 134,
        CurRowIndex = 135,
        CurColIndex = 136,
        TargetRowIndex = 137,
        TargetColIndex = 138,
        Track = 139, // 轨迹
        TrackLength = 140, // 轨迹
        TrackToPlc = 141, // 轨迹
        TrackLengthToPlc = 142, // 轨迹
        Lable = 143,
        LableLength = 144,
        LableTrigger = 145,
        相机轴X = 146,
        相机轴Y = 147,
        Path_X = 880,// 弃用
        Path_Y = 881,// 弃用
        Path_Z = 882,// 弃用
        Path_U = 883,// 弃用
        Path_V = 884,// 弃用
        Path_W = 885,// 弃用
        Path_Theta = 886,// 弃用
        Add_X = 887, // 对位补偿 X  弃用
        Add_Y = 888, // 对位补偿 Y  弃用
        Add_Z = 889, // 对位补偿 Y  弃用
        Add_Theta = 890, // 对位补偿 Theta  弃用
        Add_X2 = 891,  // 弃用
        Add_Y2 = 892, // 弃用
        Add_Z2 = 893, // 弃用
        Add_Theta2 = 894, // 弃用
        X1 = 895,// 弃用
        Y1 = 896,// 弃用
        X2 = 897,// 弃用
        Y2 = 898,// 弃用
        X3 = 899,// 弃用
        Y3 = 900,// 弃用
        TriggerFromSocket = 901, // 弃用
        TriggerToSocket = 902,// 弃用
        SocketCommand = 903, // 弃用


    }

}
