////*****************************************************************
//
// Moudle Name  :   nmc_lib20.h
// Abstract     :   GaoChuan Motion 2.0 user header
// Modification History :
// Note :			1.结构体定义中所有的‘dummyxxx’的成员都是保留参数，请不要修改他们
//					2.无特别说明，所有API返回RTN_CMD_SUCCESS（即0值）表示执行成功，其他则表示错误代码
//					3.所有的API参数中，无特别说明，axisHandle表示操作轴的句柄，devHandle表示目标控制器的句柄，crdHandle表示目标坐标系组句柄
////*****************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using HAND = System.UInt16;

namespace MotionControlCard
{
    public class CNMCLib20
    {
        public const string DLL_PATH = @"nmc_lib20.dll";


        //----------------------------------------------------------
        //  常量宏定义                                                 
        //----------------------------------------------------------
        /// <summary>
        /// 通讯设置
        /// </summary>
        public enum TSearchMode { USB = 0, Ethernet, RS485 } ;

        /// <summary>
        ///  其他
        /// </summary>
        public const Int32 ACC_MAX = 9999;

        //----------------------------------------------------------
        //  结构体定义                                                 
        //----------------------------------------------------------
        /// <summary>
        /// 设备信息结构
        /// </summary>
        public struct TDevInfo
        {
            public ushort address;             // 在上位机系统中的设备序号,
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] idStr;               // 识别字符串  
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] description;         // 描述符
            public ushort ID;                  // 板上的ID
            public TDevInfo(ushort add, ushort id, byte[] des)
            {
                address = add;
                idStr = new byte[16];
                description = new byte[64];
                ID = id;
            }
        };

        /// <summary>
        /// 设备资源信息
        /// </summary>
        public struct TDevResourceInfo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] strVer;            // 控制器版本号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] strOemVer;               // OEM版本号(定制控制器才有)，默认为0
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] time;     // 时间，年月日，时分秒

            public short axisNum;          // 支持的轴数
            public short encNum;           // 支持的编码器数
            public short diNum;            // 数字输入数量
            public short doNum;            // 数字输出数量
            public short daNum;            // 模拟量通道
            public short adNum;            // 模拟量输入通道
            public short hsioNum;          // 高速IO通道
            public short reserved;         // 保留
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ipv4;           // IP地址
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] idStr;         // 板卡名称，多卡时可用名称打开参考NMC_DevOpenByID
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] uid;   // 唯一序列号
        };

        //----------------------------------------------------------
        //  函数声明
        //----------------------------------------------------------
        ///////////////////////////////////////////////////////////////////function
        //----------------------------------------------------------
        //	1.控制器的打开和基本操作
        //----------------------------------------------------------

        /// <summary>
        /// 板卡搜寻
        /// </summary>
        /// <param name="mode">通讯模式</param>
        /// <param name="pDevNo">返回设备的数目</param>
        /// <param name="pInfoList">返回设备信息</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevSearch(TSearchMode mode, ref UInt16 pDevNo,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 4 * 84)]byte[] pInfoList);

        /// <summary>
        /// 板卡打开（根据序号）
        /// </summary>
        /// <param name="devNo">设备序号，取值范围[0,n]</param>
        /// <param name="pDevHandle">返回设备操作句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevOpen(Int16 devNo, ref UInt16 pDevHandle);

        /// <summary>
        ///  板卡复位
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevReset(HAND devHandle);

        /// <summary>
        /// 板卡打开（根据名称）
        /// </summary>注：ID号用户可写入，掉电不丢失，可用于区分板卡。出厂板卡号ID号为CARD1
        /// <param name="idStr">板卡ID字符串, 可通过指令写入</param>
        /// <param name="pDevHandle">返回设备操作句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevOpenByID(string idStr, ref UInt16 pDevHandle);

        /// <summary>
        /// 板卡打开（根据IP地址）
        /// </summary>注：ID号用户可写入，掉电不丢失，可用于区分板卡。出厂板卡号ID号为CARD1
        /// <param name="pIPv4Array">板卡IP地址,四个字节,例如：unsinged char ipv4[4] = {192,168,1,110}</param>
        /// <param name="pDevHandle">返回设备操作句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevOpenByIP([MarshalAs(UnmanagedType.LPArray, SizeConst = 1440)] byte[] pIPv4Array, ref UInt16 pDevHandle);

        /// <summary>
        /// 板卡关闭
        /// </summary>
        /// <param name="pDevHandle">控制器句柄指针</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevClose(ref UInt16 pDevHandle);

        /// <summary>
        /// 打开单轴
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="itemNo">轴号，取值范围[0,n]</param>
        /// <param name="pAxisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtOpen(HAND devHandle, Int16 itemNo, ref UInt16 pAxisHandle);

        /// <summary>
        /// 关闭单轴
        /// </summary>
        /// <param name="pAxisHandle">轴句柄指针</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClose(ref UInt16 pAxisHandle);

        /// <summary>
        /// 打开坐标系组
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="itemNo">坐标系号，取值0~1</param>
        /// <param name="pCrdHandle">返回坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdOpenEx(HAND devHandle, short itemNo, ref UInt16 pCrdHandle);

        /// <summary>
        /// 关闭坐标系组
        /// </summary>
        /// <param name="pCrdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdClose(ref UInt16 pCrdHandle);

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pFilePath">加载的配置文件路径</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LoadConfigFromFile(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 128)]byte[] pFilePath);

        /// <summary>
        /// 保存基本的配置信息,将这些信息保存到系统参数区，控制器重启或者NMC_DevReset后，系统将使用这些参数,保存的参数包括报警、限位、脉冲方式、编码器方式、安全参数、滤波参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="enable">1表示保存当前配置，并使能自动加载参数功能，0：表示关闭自动加载参数功能</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SaveMotionConfig(HAND devHandle, short enable);

        //----------------------------------------------------------
        //	2.控制器的基本配置(单轴)
        //----------------------------------------------------------

        /// <summary>
        /// 硬件限位配置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posLmtEnable">正向限位触发允许设置，1为允许，0为禁止</param>
        /// <param name="negLmtEnable">负向限位触发允许设置，1为允许，0为禁止</param>
        /// <param name="posLmtSns">正向限位触发电平，1为高电平触发，0为低电平触发</param>
        /// <param name="negLmtSns">负向限位触发电平，1为高电平触发，0为低电平触发</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetLmtCfg(HAND axisHandle, short posLmtEnable, short negLmtEnable, short posLmtSns, short negLmtSns);

        /// <summary>
        /// 读取限位开关输入是否停止运动
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posSwt">返回正向限位触发允许设置，1为允许，0为禁止</param>
        /// <param name="negSwt">返回负向限位触发允许设置，1为允许，0为禁止</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetLmtOnOff(HAND axisHandle, ref Int16 posSwt, ref Int16 negSwt);

        /// <summary>
        /// 读取限位开关触发电平
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posSwt">返回正向限位触发电平设置，1为高电平触发，0为低电平触发</param>
        /// <param name="negSwt">返回负向限位触发电平设置，1为高电平触发，0为低电平触发</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetLmtSns(HAND axisHandle, ref Int16 posSwt, ref Int16 negSwt);

        /// <summary>
        /// 伺服报警配置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="alarmEnable">伺服报警触发允许设置，1为允许，0为禁止</param>
        /// <param name="alarmSns">伺服报警触发电平，1为高电平触发，0为低电平触发</param>
        /// <returns></returns>  
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAlarmCfg(HAND axisHandle, short alarmEnable, short alarmSns);

        /// <summary>
        /// 读取伺服报警开关是否停止运动
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="swt">返回伺服报警开关输入允许设置，1为允许，0为禁止。（默认为高电平触发）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAlarmOnOff(HAND axisHandle, ref Int16 swt);

        /// <summary>
        /// 读取伺服报警开关电平
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="swt">返回伺服报警触发电平设置，1为高电平触发，0为低电平触发</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAlarmSns(HAND axisHandle, ref Int16 swt);

        /// <summary>
        /// 设置软件限位是否停止运动
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="swt">软件限位触发允许设置，1为允许，0为禁止</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSwLmtOnOff(HAND axisHandle, Int16 swt);

        /// <summary>
        /// 读取软件限位是否停止运动
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="swt">返回软件限位触发允许设置，1为允许，0为禁止</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSwLmtOnOff(HAND axisHandle, ref Int16 swt);

        /// <summary>
        /// 设置软件限位数值
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posLmt">正向软件限位设置值,单位为脉冲</param>
        /// <param name="negLmt">负向软件限位设置值,单位为脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSwLmtValue(HAND axisHandle, Int32 posLmt, Int32 negLmt);

        /// <summary>
        /// 读取软件限位数值
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posLmt">返回正向软件限位设置值</param>
        /// <param name="negLmt">返回负向软件限位设置值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSwLmtValue(HAND axisHandle, ref  Int32 posLmt, ref  Int32 negLmt);

        /// <summary>
        ///  轴运动安全参数
        /// </summary>
        public struct TSafePara
        {
            public double estpDec;       // 急停减速度
            public double maxVel;        // 最大速度
            public double maxAcc;        // 最大加速度
        };

        /// <summary>
        /// 设置轴安全参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="refPara">轴运动安全参数结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSafePara(HAND axisHandle, ref TSafePara refPara);

        /// <summary>
        /// 读取轴安全参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pPara">返回轴运动安全参数结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSafePara(HAND axisHandle, ref TSafePara pPara);

        /// <summary>
        /// 设置脉冲输出模式
        /// </summary>
        /// <param name="axisHandle"></param>
        /// <param name="inv">1：取反，0：不取反</param>
        /// <param name="mode">0：脉冲方向 1：正负脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetStepMode(HAND axisHandle, Int16 inv, Int16 mode);

        /// <summary>
        /// 读取脉冲输出模式
        /// </summary>
        /// <param name="axisHandle"></param>
        /// <param name="inv">1：取反，0：不取反</param>
        /// <param name="mode">0：脉冲方向 1：正负脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStepMode(HAND axisHandle, ref Int16 inv, ref Int16 mode);

        /// <summary>
        /// 读取轴速度滤波参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pFilterCoef">返回滤波系数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAxisVelFilter(HAND axisHandle, ref Int16 pFilterCoef);

        /// <summary>
        /// 设置轴速度滤波参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="filterCoef">滤波系数，在0~5之间，值越大，速度越平滑</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAxisVelFilter(HAND axisHandle, Int16 filterCoef);

        /// <summary>
        /// 设置脉冲输出滤波
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="coe"> coe系数： 范围0~65535，0不滤波，数值越大滤波效果越明显。</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetStepFilter(HAND axisHandle, UInt16 coe);

        /// <summary>
        /// 读取脉冲输出滤波
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pCoe">返回pCoe系数： 范围0~65535，0不滤波，数值越大滤波效果越明显</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStepFilter(HAND axisHandle, ref  UInt16 pCoe);

        // 编码器模式：定义信号源，信号类型，取反，,编码器分频等 
        // Bit7:0 分频系数，数值为0~255。对应分频数值 1~256
        // Bit9:8 信号号源选择
        //   00：外部信号输入
        //   01: 轴脉冲输入
        //   10：自动产生信号（正脉冲）
        //   11：自动产生信号（负脉冲）
        // Bit11:10 信号类型（外部）
        //  00：AB相，90度差
        //  01：脉冲+方向
        //  10：正脉冲+负脉冲
        //  11：保留
        // Bit12 输入A、B交换（外部） 0：不交换，1：交换
        // Bit13 输入A取反（外部） 0：不取反，1：取反
        // Bit14 输入B取反（外部） 0：不取反，1：取反
        // Bti15 编码器饱和，0：最大最小值翻转，1：不翻转
        /// <summary>
        /// 设置编码器模式
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="encId">编码器ID,取值范围[0,n]</param>
        /// <param name="encMode">编码器模式，参考宏定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetEncMode(HAND devHandle, Int16 encId, Int16 encMode);

        /// <summary>
        /// 读取编码器模式
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="encId">编码器ID,取值范围[0,n]</param>
        /// <param name="encMode">返回编码器模式，参考宏定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetEncMode(HAND devHandle, Int16 encId, ref Int16 encMode);

        /// <summary>
        /// 设置允许的位置误差，当位置误差超过设定值时，电机停止运动，提示位置误差超限
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posErr">位置超限误差，单位 脉冲，为0表示不检查</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPosErrLmt(HAND axisHandle, Int32 posErr);

        /// <summary>
        /// 读取允许的位置误差
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pPosErr">返回位置超限误差，单位 脉冲，为0表示不检查</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPosErrLmt(HAND axisHandle, ref Int32 pPosErr);

        //----------------------------------------------------------
        //	3.控制器的状态检测
        //----------------------------------------------------------

        // 轴状态字位定义 
        public const Int32 BIT_AXIS_BUSY = 0x00000001;  // bit 0  , 运动:1 ，静止 0 
        public const Int32 BIT_AXIS_POSREC = 0x00000002;  // bit 1  , 伺服位置到达，步进模式时位置到达，伺服模式时实际位置到达误差限 
        public const Int32 BIT_AXIS_MVERR = 0x00000004;  // bit 2  , 上次运动出错，或当前无法启动运动，需要软件复位 
        public const Int32 BIT_AXIS_SVON = 0x00000008;  // bit 3  , 伺服允许        
        public const Int32 BIT_AXIS_CRD = 0x00000010;  // bit 4  , 坐标系模式      
        public const Int32 BIT_AXIS_STEP = 0x00000020;  // bit 5  , 步进/伺服       
        public const Int32 BIT_AXIS_LMTP = 0x00000040;  // bit 6  , 正向限位        
        public const Int32 BIT_AXIS_LMTN = 0x00000080;  // bit 7  , 负面限位        
        public const Int32 BIT_AXIS_SOFTPOSLMT = 0x00000100;  // bit 8  , 正向软限位触发  
        public const Int32 BIT_AXIS_SOFTNEGLMT = 0x00000200;  // bit 9  , 负向软限位触发  
        public const Int32 BIT_AXIS_ALM = 0x00000400;  // bit 10 , 伺服报警，需要软件复位 
        public const Int32 BIT_AXIS_POSERR = 0x00000800;  // bit 11 , 位置超限，需要软件复位 
        public const Int32 BIT_AXIS_ESTP = 0x00001000;  // bit 12 , 急停，需要软件复位 
        public const Int32 BIT_AXIS_HWERR = 0x00002000;  // bit 13 , 急停，需要软件复位 
        public const Int32 BIT_AXIS_CAPTSET = 0x00004000;  // bit 14 , 捕获触发      

        /// <summary>
        /// 读当前轴状态
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pStsWord">返回轴状态字。参考位定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSts(HAND axisHandle, ref Int16 pStsWord);

        /// <summary>
        /// 清除轴错误状态
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClrError(HAND axisHandle);

        /// <summary>
        /// 清除轴错误状态，按位清除
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="stsMask">对应位为1表明需要清除对应位的错误状态</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClrStsByBits(HAND axisHandle, Int16 stsMask);

        /// <summary>
        ///  读规划位置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pos">返回轴规划位置</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfPos(HAND axisHandle, ref  Int32 pos);

        /// <summary>
        /// 读当前轴规划速度
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pVel">返回速度，单位是 脉冲/ms</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfVel(HAND axisHandle, ref  Double pVel);

        /// <summary>
        ///  读机械位置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pos">返回轴机械位置</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAxisPos(HAND axisHandle, ref  Int32 pos);

        /// <summary>
        /// 读当前规划位置（发送到执行器的位置）
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pos">返回位置，单位是 脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCmdPos(HAND axisHandle, ref  Int32 pos);

        /// <summary>
        ///  读编码器位置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pos">返回轴编码器位置</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetEncPos(HAND axisHandle, ref  Int32 pos);

        /// <summary>
        /// 读编码器速度
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="encId">编码器ID,取值范围[0,n]</param>
        /// <param name="vel">返回编码器速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetEncVel(HAND devHandle, Int16 encId, ref  double vel);

        /// <summary>
        /// 打包轴状态结构体（12个轴）
        /// </summary>
        public struct TAxisStsPack12Ex
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public short[] mtSts;             // 单轴状态，位定义参考NMC_MtGetSts
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public short[] mtMio;             // 单轴专用IO,位定义参考NMC_MtGetMotionIO
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public short[] mtMioLog;             // 单轴专用IO:逻辑电平,位定义参考NMC_MtGetMotionIOLogical
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] mtPrfPos;             // 单轴规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] mtEncPos;             // 单轴实际位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Single[] mtPrfVel;             // 单轴规划速度

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] crdSts;             // 坐标系状态
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Single[] crdPrfVel;             // 坐标系运动速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] crdUserSeg;             // 坐标系运行的缓冲区段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] crdFreeSpace;             // 坐标系缓冲区剩余空间
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] crdUsedSpace;             // 坐标系缓冲区使用空间

            public Int32 gpi;        /// 通用输入
            public Int32 gpo;        /// 通用输出
             
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] crdLeftLen;             // 坐标系缓冲区剩余段长，单位:脉冲
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] crdAllCmdCnt;             // 坐标系缓冲区总共压入的指令数目
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public Int32[] extDi;             // 扩展模块输入，2号站~8号站
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public Int32[] extDo;             // 扩展模块输出，2号站~8号站
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Int32[] reserved;             // 预留

            public TAxisStsPack12Ex(bool bInit)
            { 
                mtSts = new short[12];
                mtMio = new short[12];
                mtMioLog = new short[12];
                mtPrfPos = new int[12];
                mtEncPos = new int[12];
                mtPrfVel = new Single[12];

                crdSts = new short[2];
                crdPrfVel = new Single[2];
                crdUserSeg = new int[2];
                crdFreeSpace = new int[2];
                crdUsedSpace = new int[2];

                gpo = gpi = 0;
                crdLeftLen = new int[2];
                crdAllCmdCnt = new int[2];
                extDi = new int[6];
                extDo = new int[6];
                reserved = new int[16];
                
            }
        }

        /// <summary>
        /// 打包读取12个轴的状态，从第一个轴开始读取
        /// </summary>
        /// <param name="axisFirstHandle">第一个轴句柄</param>
        /// <param name="packSts">打包的12轴状态数据，参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStsPack12Ex(HAND axisFirstHandle, ref TAxisStsPack12Ex packSts);

        //----------------------------------------------------------
        //	4.点位和JOG运动
        //----------------------------------------------------------

        // 各轴的规划模式
        public const Int16 MT_NONE_PRF_MODE = -1;      // 无效
        public const Int16 MT_PTP_PRF_MODE = 0;      // 梯形规划
        public const Int16 MT_JOG_PRF_MODE = 1;      // 连续速度模式
        public const Int16 MT_CRD_PRF_MODE = 3;      // 坐标系
        public const Int16 MT_GANTRY_MODE = 4;      // 龙门跟随模式

        /// <summary>
        /// 设置单轴规划模式
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="mode">单轴运动模式</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPrfMode(HAND axisHandle, Int16 mode);

        /// <summary>
        /// 获取轴规划模式
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pMode">返回轴运动模式</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfMode(HAND axisHandle, ref Int16 pMode);

        /// <summary>
        /// 单轴PTP运动参数结构
        /// </summary>
        public struct TPtpPara
        {
            public double acc;      // 加速度
            public double dec;      // 减速度
            public double startVel; // 起跳速度
            public double endVel;   // 终止速度
            public short smoothCoef;// 平滑系数，取值范围[0,199]
            public short dummy1;    // 保留
            public short dummy2;
            public short dummy3;
        };

        /// <summary>
        /// 设置Ptp参数, 并更新。
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="refAxPara">参数，参考结构定义。</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPtpPara(HAND axisHandle, ref TPtpPara refAxPara);

        /// <summary>
        /// 获取Ptp参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pAxPara">返回Ptp参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPtpPara(HAND axisHandle, ref TPtpPara pAxPara);

        /// <summary>
        /// 设置目标运动位置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pos">目标位置，单位是 脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPtpTgtPos(HAND axisHandle, Int32 pos);

        /// <summary>
        /// 获取目标运动位置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pPos">返回目标位置，单位是 脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPtpTgtPos(HAND axisHandle, ref  Int32 pPos);

        /// <summary>
        /// 设置目标运动速度
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="vel">目标速度（最大速度），单位是 脉冲/ms</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetVel(HAND axisHandle, double vel);

        /// <summary>
        /// 获取目标运动速度
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pVel">返回目标速度（最大速度），单位是 脉冲/ms</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetVel(HAND axisHandle, ref  Double pVel);

        /// <summary>
        /// 单轴JOG运动参数结构
        /// </summary>
        public struct TJogPara
        {
            public double acc;          // 加速度
            public double dec;          // 减速度
            public double smoothCoef;   // 平滑系数，取值范围[0,199]
        };

        /// <summary>
        /// 设置Jog运动参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="refAxPara">Jog运动参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetJogPara(HAND axisHandle, ref TJogPara refAxPara);

        /// <summary>
        /// 获取Jog参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pAxPara">返回Jog运动参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetJogPara(HAND axisHandle, ref TJogPara pAxPara);

        /// <summary>
        /// 参数更新， 只针对PTP和Jog
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtUpdate(HAND axisHandle);

        /// <summary>
        /// 多轴同时启动
        /// </summary>
        /// <param name="axisHandle">任意轴句柄</param>
        /// <param name="mask">按bit对应相应轴号，bit为1表示启动，bit为0表示不启动</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtUpdateMulti(HAND axisHandle, Int32 mask);

        /// <summary>
        /// 单轴运动停止 注：运动会按设定的减速度停止。
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtStop(HAND axisHandle);

        /// <summary>
        /// 单轴急停 注：运动会按设定的急停加速度停止。如果没有设置，则会立即停止
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtEstp(HAND axisHandle);

        /// <summary>
        /// 单轴急停，不会置起急停标志位   
        /// 注：运动会按设定的急停加速度停止。如果没有设置,则会立即停止。
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        ///  <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtAbruptStop(HAND axisHandle);

        /// <summary>
        /// 多轴同时停止
        /// </summary>
        /// <param name="axisHandle">任意轴句柄</param>
        /// <param name="mask">按bit对应相应轴号，bit为1表示停止，bit为0表示不停止</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtStopMulti(HAND axisHandle, Int32 mask);

        /// <summary>
        /// 单轴点位运动,包含NMC_MtSetPrfMode,NMC_MtSetPtpPara,NMC_MtSetVel,NMC_MtSetPtpTgtPos,NMC_MtUpdate
        /// </summary>
        /// <param name="acc">加速度</param>
        /// <param name="dec">减速度</param>
        /// <param name="startVel">起跳速度</param>
        /// <param name="endVel">终止速度</param>
        /// <param name="maxVel">最大速度</param>
        /// <param name="smoothCoef">平滑系数</param>
        /// <param name="tgtPos">目标位置,单位：脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtMovePtpAbs(HAND axisHandle, double acc, double dec, double startVel, double endVel, double maxVel, Int16 smoothCoef, Int32 tgtPos);

        /// <summary>
        ///  单轴点位运动,相对运动
        /// </summary>
        /// <param name="acc">加速度</param>
        /// <param name="dec">减速度</param>
        /// <param name="startVel">起跳速度</param>
        /// <param name="endVel">终止速度</param>
        /// <param name="maxVel">最大速度</param>
        /// <param name="smoothCoef">平滑系数</param>
        /// <param name="relPos">相对当前位置的移动量,单位：脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtMovePtpRel(HAND axisHandle, double acc, double dec, double startVel, double endVel, double maxVel, Int16 smoothCoef, Int32 relPos);

        /// <summary>
        /// 单轴点位运动打包
        /// </summary>
        public struct TMovePtpPack8
        {
            public short axisMask;            // 轴掩码，对应bit为1表示该轴参与运动
            public short clrStsFlag;            // 是否运动前先清除轴状态，0：不清除，1：清除
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] reserved;             // 保留
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public double[] acc;             // 加速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public double[] dec;             // 减加速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public double[] startVel;             // 起跳速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public double[] endVel;             // 终止速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public double[] maxVel;             // 最大速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] smoothCoef;             // 平滑系数
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] tgtPos;             // 位置，单位:脉冲

            public TMovePtpPack8(bool bInit)
            { 
                axisMask = clrStsFlag = 0;
                reserved = new short[2];
                acc = new double[8];
                dec = new double[8];
                startVel = new double[8];
                endVel = new double[8];
                maxVel = new double[8];
                smoothCoef = new short[8];
                tgtPos = new int[8];
                
            }
        }

        /// <summary>
        /// 单轴点位运动打包(绝对运动)
        /// </summary>
        /// <param name="axisFirstHandle">数组的第一个轴句柄</param>
        /// <param name="pPackData">打包参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtMovePtpAbsPack8(HAND axisFirstHandle, ref TMovePtpPack8 pPackData);

        /// <summary>
        /// 单轴点位运动打包(相对运动)
        /// </summary>
        /// <param name="axisFirstHandle">数组的第一个轴句柄</param>
        /// <param name="pPackData">打包参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtMovePtpRelPack8(HAND axisFirstHandle, ref TMovePtpPack8 pPackData);

        /// <summary>
        ///  单轴连续速度运动
        /// </summary>
        /// <param name="acc">加速度</param>
        /// <param name="dec">减速度</param>
        /// <param name="maxVel">最大速度</param>
        /// <param name="smoothCoef">平滑系数</param>
        /// <param name="clrStsFlag">是否运动前先清除轴状态，0：不清除，1：清除</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtMoveJog(HAND axisHandle, double acc, double dec, double maxVel, Int16 smoothCoef, Int16 clrStsFlag);

        //----------------------------------------------------------
        //	5.回零运动
        //----------------------------------------------------------

        /// <summary>回零类型定义 
        /// 回零模式    单原点	   单限位	  单Z相	   原点+Z相 原点+ -Z相	限位+ -Z相
        /// </summary>
        public enum THomeMode { HM_MODE1 = 0, HM_MODE2, HM_MODE3, HM_MODE4, HM_MODE5, HM_MODE6 } ;

        /// <summary>
        ///  回零参数设置
        /// </summary>
        public struct THomeSetting
        {
            public short mode;             // 模式，HM_MODE1 ~ HM_MODE6 （必须）
            public short dir;              // 搜寻零点方向（必须）, 0:负向，1：正向，其它值无意义
            public Int32 offset;           // 原点偏移（必须）
            public double scan1stVel;      // 基本搜寻速度（必须）
            public double scan2ndVel;      // 低速（两次搜寻时需要）            
            public double acc;			   // 加速度
            public byte reScanEn;          // 是否两次搜寻零点（可选，不用时设为0）
            public byte homeEdge;          // 原点，触发沿（默认下降沿）
            public byte lmtEdge;           // 限位，触发沿（默认下降沿）
            public byte zEdge;             // 限位，触发沿（默认下降沿）
            public Int32 iniRetPos;        // 起始反向运动距离（可选，不用时设为0）
            public Int32 retSwOffset;      // 反向运动时离开开关距离（可选，不用时设为0）
            public Int32 safeLen;          // 安全距离，回零时最远搜寻距离（可选，不用时设为0，不限制距离）
            public byte usePreSetPtpPara;  // 为1表示用户需要在启动回零前，自己设置回零运动（点到点）的参数,0时，回零运动的减加速度默认等于acc,起跳速度、终点速度、平滑系数默认为0
            public byte reserved0;         // 保留
            public byte reserved1;         // 保留
            public byte reserved2;         // 保留
        } ;

        /// <summary>
        /// 设置回零参数
        /// </summary>
        /// <param name="axisHandle"></param>
        /// <param name="refHomePara">回零参数结构，参考结构定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetHomePara(HAND axisHandle, ref THomeSetting refHomePara);

        /// <summary>
        /// 读取回零参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pHomePara">返回回零参数结构，参考结构定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetHomePara(HAND axisHandle, ref THomeSetting pHomePara);

        /// <summary>
        /// 回零状态字位定义
        /// </summary>
        public const Int32 BIT_AXHOME_BUSY = 0x00000001;  // bit 0  , 忙    
        public const Int32 BIT_AXHOME_OK = 0x00000002;  // bit 1  , 成功  
        public const Int32 BIT_AXHOME_FAIL = 0x00000004;  // bit 2  , 失败  
        public const Int32 BIT_AXHOME_ERR_MV = 0x00000008;  // bit 3  , 错误：运动参数出错导致不动 
        public const Int32 BIT_AXHOME_ERR_SWT = 0x00000010;  // bit 4  , 错误：搜寻过程中开关没触发 

        /// <summary>
        /// 读回零状态
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pStsWord">返回状态字。参数宏定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetHomeSts(HAND axisHandle, ref Int16 pStsWord);

        /// <summary>
        /// 启动回零
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtHome(HAND axisHandle);

        /// <summary>
        /// 尝试性回零（测试回零误差，不清位置）
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtTryHome(HAND axisHandle);

        /// <summary>
        /// 终止回零
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtHomeStop(HAND axisHandle);

        /// <summary>
        /// 读新回零位置和历史回零位置的差值 注：回零成功时才有意义
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="cmdPos">位置偏差</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetHomeError(HAND axisHandle, ref  Int32 cmdPos);

        //----------------------------------------------------------
        //	6.坐标系的运动
        //----------------------------------------------------------

        //----------------------------------------------------------
        //	6.1 坐标系的初始化
        //----------------------------------------------------------

        /// <summary>
        /// 坐标系配置
        /// </summary>
        public struct TCrdConfig
        {
            public short axCnts;     // 参与的轴数量
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] reserved; // 保留参数
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] pAxis;    // 坐标系XYZ轴映射表,轴序号取值范围[0,n]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] port;     // 坐标系端口映射表,统一设为0
            public TCrdConfig(bool init)
            {
                axCnts = 0;
                reserved = new short[3];
                pAxis = new short[4];
                port = new short[4];
            }
        };

        /// <summary>
        /// 建立插补坐标系系统
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="refConfig">坐标系配置，参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdConfig(HAND crdHandle, ref TCrdConfig refConfig);

        /// <summary>
        /// 读取插补坐标系系统配置信息
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pConfig">返回坐标系配置，参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetConfig(HAND crdHandle, ref TCrdConfig pConfig);

        /// <summary>
        ///  删除坐标系
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdDelete(HAND crdHandle);

        /// <summary>
        /// 坐标系运动参数
        /// </summary>
        public struct TCrdPara
        {
            public short orgFlag;       // 是否自定义坐标系原点,0 不指定，以当前位置为坐标系原点，1 指定，以机械原点加offset偏置为坐标系原点
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] reserved;    // 保留参数
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] offset;      // 自定义坐标系原点偏置（基于机械原点）
            public double synAccMax;    // 最大合成加速度
            public double synVelMax;    // 最大合成速度
            public TCrdPara(bool init)
            {
                orgFlag = 0;
                reserved = new short[3];
                offset = new int[4];
                synAccMax = synVelMax = 0;
            }
        };

        /// <summary>
        /// 设置坐标系参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="refCrdPara">坐标系参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetPara(HAND crdHandle, ref TCrdPara refCrdPara);

        /// <summary>
        /// 获取坐标系参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pCrdPara">返回坐标系参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetPara(HAND crdHandle, ref TCrdPara pCrdPara);


        /// <summary>
        /// 缓冲区运动安全参数结构体
        /// </summary>
        public struct TCrdSafePara
        {
            public double estpDec;     // 急停加速度
            public double maxVel;      // 最大速度
            public double maxAcc;      // 最大加速度
        }

        /// <summary>
        /// 设置轴缓冲区运动安全参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pPara">缓冲区运动安全参数结构体数据</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetSafePara(HAND crdHandle, ref TCrdSafePara pPara);

        /// <summary>
        /// 坐标系运动扩展参数
        /// </summary>
        public struct TExtCrdPara
        {
            public double startVel;         //（默认0）
            public double T;                // (默认1)
            public double smoothDec;        //（默认accMax）
            public double abruptDec;        //（默认无穷大）
            public short lookAheadSwitch;   //( 默认有前瞻)
            public short eventTime;         // 最小匀速时间
            public short dummy2;
            public short dummy3;
        };

        /// <summary>
        /// 设置坐标系扩展参数
        /// </summary>
        /// <param name="crdHandle"></param>
        /// <param name="refExtCrdPara">坐标系扩展参数结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetExtPara(HAND crdHandle, ref TExtCrdPara refExtCrdPara);

        /// <summary>
        /// 读取坐标系扩展参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pExtCrdPara">坐标系扩展参数结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetExtPara(HAND crdHandle, ref TExtCrdPara pExtCrdPara);

        /// <summary>
        ///  圆弧插补配置
        /// </summary>
        public const Int16 MAX_ERR_TABLE_SECTION = 2;
        public struct TArcSecSetting
        {
            public double minSectionLen;				    // 分解的最小段长,默认1脉冲,范围[1,10000]
            public double maxArcDiff;					// 最大的圆弧有效性误差,单位：脉冲
            // 圆弧插补误差配置表
            // 注意：MTN通过限制圆弧插补速度，从而保证插补误差。r全部为0，则表示关闭这个功能
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ERR_TABLE_SECTION)]
            public double[] r;     // 半径
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ERR_TABLE_SECTION + 1)]
            public double[] err;  // 半径对应的插补误差，半径 [0,r0],对应err0;半径[r0,r1],对应err1;半径[r1,+max],对应err2
            public TArcSecSetting(bool init)
            {
                minSectionLen = maxArcDiff = 0;
                r = new double[MAX_ERR_TABLE_SECTION];
                err = new double[MAX_ERR_TABLE_SECTION + 1];
            }
        };

        /// <summary>
        /// 设置圆弧插补参数（高级指令）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pSetting">圆弧插补配置结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetArcSecPara(HAND crdHandle, ref TArcSecSetting pSetting);

        /// <summary>
        /// 读圆弧插补参数（高级指令）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pSetting">返回圆弧插补配置结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetArcSecPara(HAND crdHandle, ref TArcSecSetting pSetting);

        /// <summary>
        /// 设置向心加速度限制
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetLookAheadCentriAcc(HAND crdHandle, short isUsingSetAcc, double centriAcc);

        // 设置4维插补下，A轴的最大容忍转弯速度，若大于该速度，则需要进行降速处理
        /// <summary>
        /// 设置4维插补下，A轴的最大容忍转弯速度，若大于该速度，则需要进行降速处理
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetFourthAxisTolTurnVel(HAND crdHandle, double tolVel);

        //----------------------------------------------------------
        //	6.2 直线插补和平面圆弧插补
        //----------------------------------------------------------

        /// <summary>
        /// 直线插补（带前瞻开关）
        /// </summary>
        /// <param name="crdHandle"></param>
        /// <param name="segNo">段号</param>
        /// <param name="mask">参与的轴，按位表示</param>
        /// <param name="pTgPos">目标位置</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">是否使用前瞻,0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZEx(HAND crdHandle, Int32 segNo, Int16 mask, Int32[] pTgPosArray, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// 4轴直线插补
        /// </summary>
        /// <param name="crdHandle"></param>
        /// <param name="segNo">段号</param>
        /// <param name="crdAxMask">参与的轴，按位表示</param>
        /// <param name="pTgPosArray">目标位置</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">是否使用前瞻,0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZA(HAND crdHandle, Int32 segNo, Int16 crdAxMask, Int32[] pTgPosArray, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// XY平面圆弧插补：终点位置、圆心、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（二维数组，分别表示XY轴的目标位置）</param>
        /// <param name="pCenterPosArray">圆心坐标（二维数组，分别表示XY轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcCenterEx(HAND crdHandle, Int32 segNo, Int32[] pTgPosArray, Int32[] pCenterPosArray, Int16 circleDir, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// YZ平面圆弧插补：终点位置、圆心、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（二维数组，分别表示YZ轴的目标位置）</param>
        /// <param name="pCenterPosArray">圆心坐标（二维数组，分别表示YZ轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcCenterYZEx(HAND crdHandle, Int32 segNo, Int32[] pTgPosArray, Int32[] pCenterPosArray, Int16 circleDir, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// ZX平面圆弧插补：终点位置、圆心、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（二维数组，分别表示ZX轴的目标位置）</param>
        /// <param name="pCenterPosArray">圆心坐标（二维数组，分别表示ZX轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcCenterZXEx(HAND crdHandle, Int32 segNo, Int32[] pTgPosArray, Int32[] pCenterPosArray, Int16 circleDir, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// XY平面圆弧插补：终点位置、半径、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPos">目标位置（二维数组，分别表示XY的位置）</param>
        /// <param name="radius">圆弧半径，大于0表示劣弧，小于0表示优弧</param>
        /// <param name="circleDir">圆弧方向,0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcRadiusEx(HAND crdHandle, Int32 segNo, Int32[] pTgPosArray, double radius, Int16 circleDir, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// YZ平面圆弧插补：终点位置、半径、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPos">目标位置（二维数组，分别表示YZ的位置）</param>
        /// <param name="radius">圆弧半径，大于0表示劣弧，小于0表示优弧</param>
        /// <param name="circleDir">圆弧方向,0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcRadiusYZEx(HAND crdHandle, Int32 segNo, Int32[] pTgPosArray, double radius, Int16 circleDir, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// ZX平面圆弧插补：终点位置、半径、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPos">目标位置（二维数组，分别表示ZX的位置）</param>
        /// <param name="radius">圆弧半径，大于0表示劣弧，小于0表示优弧</param>
        /// <param name="circleDir">圆弧方向,0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcRadiusZXEx(HAND crdHandle, Int32 segNo, Int32[] pTgPosArray, double radius, Int16 circleDir, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// XY平面圆弧插补：起点（当前点）、中点、终点
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pMidPosArray">中间位置点坐标（二维数组，分别表示中间点的XY轴的位置）</param>
        /// <param name="pTgPosArray">终点位置坐标（二维数组，分别表示终点的XY轴的位置）</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcPPPEx(HAND crdHandle, Int32 segNo, Int32[] pMidPosArray, Int32[] pTgPosArray, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// 椭圆插补，默认不参与速度前瞻，起始和终止速度为0（注意！椭圆为整圆）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="abRatio">表示椭圆AB轴的长度比例值 值范围：[0.05,1]</param>
        /// <param name="pCenterPos">椭圆圆心点位置（二维数组,分别表示中点的XY轴的位置）。注意！起点位置到圆心位置为长轴的</param>
        /// <param name="ellipseDir">椭圆绘画方向,0表示顺时针方向,1表示逆时针方向</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdEllipse(HAND crdHandle, int segNo, double abRatio, int[] pCenterPos, short ellipseDir, double vel, double synAcc);

        //----------------------------------------------------------
        //	6.3 多维(8)插补指令
        //----------------------------------------------------------

        /// <summary>
        /// 多轴直线插补（最多支持8轴）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="crdAxmask">坐标系中参与的轴，按位表示</param>
        /// <param name="extAxmask">其他参与轴，按位表示，不能包括坐标系中</param>
        /// <param name="pTgPosArray">目标位置数组，长度为所有参与运动的轴的数量,索引小的是坐标系内的轴的坐标（按crdAxMask位排列）。索引大的是其它联动轴的坐标（按extAxMask位排列</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZD8(HAND crdHandle, Int32 segNo, Int32 crdAxmask, Int32 extAxmask, Int32[] pTgPosArray, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// 8轴插补打包单元结构体
        /// </summary>
        public struct TCrdLineXYZD8Unit
        {
            public Int32 segNo;         // 用户自定义段号
            public Int16 crdAxMask;
            public Int16 extAxMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] tgPos;       // 目标位置     
            public Single vel;          // 最大速度
            public Single endVel;       // 终点速度
            public Single acc;			// 插补加速度
            public Int16 lookaheadDis;	// 是否使用前瞻
            public Int16 reserved;		// 保留
            public TCrdLineXYZD8Unit(bool init)
            {
                segNo = crdAxMask = extAxMask = 0;
                tgPos = new Int32[8];
                vel = endVel = acc = lookaheadDis = reserved = 0;
            }
        };

        /// <summary>
        /// 打包的多轴直线插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="count">打包指令数，取值范围[1,18]</param>
        /// <param name="pCmdArray">8轴插补打包单元结构体数据</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZD8Pack(HAND crdHandle, Int16 count, ref TCrdLineXYZD8Unit pCmdArray);

        //----------------------------------------------------------
        //	6.4 螺旋线插补
        //----------------------------------------------------------

        /// <summary>
        /// 螺旋线插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（三维数组，分别表示终点的XYZ轴的位置）</param>
        /// <param name="pCenterPosArray">圆心位置（二维数组，分别表示XY轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="rounds">方向圈数</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdHelixCenterEx(HAND crdHandle, Int32 segNo, Int32[] pTgPosArray, Int32[] pCenterPosArray, Int16 circleDir, double rounds, double endVel, double vel, double synAcc, Int16 lookaheadDis);

        //----------------------------------------------------------
        //	6.5 空间圆弧插补
        //----------------------------------------------------------

        /// <summary>
        /// 3D圆弧插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pMidPos">中点位置（三维数组，分别表示中点的XYZ轴的位置）</param>
        /// <param name="pTgPos">终点位置（三维数组，分别表示终点的XYZ轴的位置）</param>
        /// <param name="velEnd">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArc3DEx(HAND crdHandle, Int32 segNo, Int32[] pMidPos, Int32[] pTgPos, double velEnd, double vel, double synAcc, Int16 lookaheadDis);

        /// <summary>
        /// 3D圆弧插补(整圆)
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pMidPos">中点位置（三维数组，分别表示中点的XYZ轴的位置）</param>
        /// <param name="pTgPos">终点位置（三维数组，分别表示终点的XYZ轴的位置）</param>
        /// <param name="velEnd">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <param name="lookaheadDis">0：使用前瞻，则控制器自动计算终点速度，1：禁用前瞻，使用设定的终点速度（endVel）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdCircle3DEx(HAND crdHandle, long segNo, Int32[] pMidPos, Int32[] pTgPos, double velEnd, double vel, double synAcc, Int16 lookaheadDis);

        //----------------------------------------------------------
        //	6.6 缓存区的操作(IO、延时、单轴跟随等)
        //----------------------------------------------------------

        /// <summary>
        /// 缓冲区输出DO组类型定义
        /// </summary>
        public const Int32 CRD_BUFF_DO_MOTOR_ENABLE = 1;     // 电机使能
        public const Int32 CRD_BUFF_DO_MOTOR_CLEAR = 2;     // 电机报警清除
        public const Int32 CRD_BUFF_DO_GPDO1 = 3;     // 通用输出1
        public const Int32 CRD_BUFF_DO_GPDO2 = 4;     // 通用输出2

        /// <summary>
        /// 缓冲区DO（按位输出）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="group">类型</param>
        /// <param name="opBits">操作位</param>
        /// <param name="value">输出值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufDo(HAND crdHandle, Int32 segNo, Int16 group, Int32 opBits, Int32 value);

        /// <summary>
        /// 缓冲区DO（掩码输出）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="group">类型</param>
        /// <param name="doMask">位掩码</param>
        /// <param name="value">输出值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufDoEx(HAND crdHandle, Int32 segNo, Int16 group, Int32 opBits, Int32 value);

        /// <summary>
        /// OUT输出宏类型定义
        /// </summary>
        public const Int32 BUF_OUT_GROUP_DA = 0;    // 模拟量输出
        public const Int32 BUF_OUT_GROUP_PWM = 1;    // PWM输出

        /// <summary>
        /// 缓冲区OUT输出
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="group">类型</param>
        /// <param name="ch">通道</param>
        /// <param name="value">输出值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufOut(HAND crdHandle, Int32 segNo, Int16 group, Int16 ch, Int32 value);

        /// <summary>
        /// 缓冲区DI等待
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="index">通道号</param>
        /// <param name="diValue">等待值</param>
        /// <param name="waitLastTime">超时时间，单位：毫秒</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufWaitDI(HAND crdHandle, Int32 segNo, Int16 index, Int16 diValue, Int32 waitLastTime);

        /// <summary>
        /// 缓冲区设置急停DI
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axis">轴号</param>
        /// <param name="gpiIndex">通用输入序号</param>
        /// <param name="sense">触发电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSetEstopDI(HAND crdHandle, Int32 segNo, Int16 axis, Int16 gpiIndex, Int16 sense);

        /// <summary>
        /// 缓冲区延时单位类型
        /// </summary>
        public const Int32 CRD_BUFF_DELAY_SCALE_MS = 0;    // 毫秒
        public const Int32 CRD_BUFF_DELAY_SCALE_SECAND = 1;    // 秒

        /// <summary>
        /// 缓冲区延时
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="scale">延时单位</param>
        /// <param name="count">延时时长,时间单位由scale决定</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufDelay(HAND crdHandle, Int32 segNo, Int16 scale, Int32 count);

        /// <summary>
        /// 缓冲区单轴移动(带参数)
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axMask">参与的轴</param>
        /// <param name="pTgPos">目标位置</param>
        /// <param name="vel">速度</param>
        /// <param name="acc">加速度</param>
        /// <param name="blockEn">是否为阻塞模式</param>
        /// <param name="synEn">是否为同步模式</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufAxMoveEx(HAND crdHandle, Int32 segNo, Int16 axMask, Int32[] pTgPos, double vel, double acc, Int16 blockEn, Int16 synEn);

        /// <summary>
        /// 缓冲区单轴移动(相对位移移动
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axMask">参与的轴</param>
        /// <param name="pRelPos">目标位置</param>
        /// <param name="blockEn">是否为阻塞模式</param>
        /// <param name="synEn">是否为同步模式</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufAxMoveRel(HAND crdHandle, Int32 segNo, Int16 axMask, Int32[] pRelPos, Int16 blockEn, Int16 synEn);

        /// <summary>
        /// 缓冲区单轴移动参数设置
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axis">轴号，[0,n]</param>
        /// <param name="vel">运动速度</param>
        /// <param name="acc">运动加速度</param>
        /// <param name="soomthCoef">平滑系数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSetPtpMovePara(HAND crdHandle, Int32 segNo, Int16 axis, double vel, double acc, Int16 soomthCoef);

        /// <summary>
        /// 设置跟随运动前的运动补偿量
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axis">轴号，[0,n]</param>
        /// <param name="relDistance">相对补偿位移量</param>
        /// <param name="vel">补偿速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufBeforeAxSyncMove(HAND crdHandle, Int32 segNo, Int16 axis, Int32 relDistance, double vel);

        /// <summary>
        /// 功能:缓冲区等待电机运动到位
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axisMask">需要等待到位的轴掩码（按位对应轴号，不能超出控制器的最大轴数）</param>
        /// <param name="overTime">等待到位超时的时间,单位：毫秒</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufWaitEncInPosition(HAND crdHandle, Int32 segNo, Int32 axisMask, Int32 overTime);

        /// <summary>
        /// 功能:缓冲区等待特定位置，满足条件才执行下一条指令
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axisNo">需要等待到位的轴号</param>
        /// <param name="condition">到位条件，0：小于等于设定位置  1：大于等于设定位置</param>
        /// <param name="pos">设定位置</param>
        /// <param name="posSrc">等待规划还是编码器，0：编码器 1：内部规划</param>
        /// <param name="overTime">等待到位超时的时间，单位：ms</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufWaitPos(HAND crdHandle, Int32 segNo, Int16 axisNo, Int16 condition, Int32 pos, Int16 posSrc, Int32 overTime);

        //----------------------------------------------------------
        //	6.7 插补打包指令介绍
        //----------------------------------------------------------

        // 缓冲区数据打包
        public const  Int16 BUF_LINE =	 0; //缓冲区直线插补
        public const  Int16 BUF_DO	=	 1; //缓冲区DO输出
        public const  Int16 BUF_OUT	=	 2; //缓冲区OUT输出
        public const  Int16 BUF_DELAY =  3; //缓冲区延时
        public const  Int16 BUF_AXMOVE = 4; //缓冲区单轴运动
        public const  Int16 BUF_DOEX  =  5; //缓冲区DO输出（根据掩码输出）
        public const  Int16 BUF_ARC_R =  6; //平面圆弧插补：终点位置、半径、方向
        public const  Int16 BUF_ARC_C =  7; //平面圆弧插补：终点位置、圆心、方向
        public const  Int16 BUF_LASER_SETPOWER	=	8;	// 激光：设置能量
        public const  Int16 BUF_LASER_ONOFF		=	9;	// 激光：缓冲区开关光
        public const  Int16 BUF_LASER_SETFOLLOW	=	10;	// 激光：设置跟随
        public const  Int16 BUF_LASER_SETPARAM	=	11;	// 激光：设置参数
        public const  Int16 BUF_LINEXYZA		=	12;	// 缓冲区四轴直线插补
        public const  Int16 BUF_SHIO_GATEPULSE	=	13;	// 缓冲区输出Gate脉冲
        public const  Int16 BUF_DOBITPULSE		=	14;	// DoBitPulse功能
        public const  Int16 BUF_XYZD8          	=	15;	// LineXYZD8,数据结构体为：TCrdLineXYZD8Unit
        public const  Int16 BUF_SHIOMINFRQ     	=	16;	// 设置或清除SHIO最小频率
        public const  Int16 BUF_SHIOSETPARAM    =   17;	// 设置SHIO参数
        public const  Int16 BUF_SHIOGATEONOFF   =   18;	// 设置Gate开关
        public const  Int16 BUF_WAITENCINPOS    =   19;	// 等待电机到位
        public const  Int16 BUF_WAITDI          =   20;	// 等待DI

        public struct TCrdBufLine
        {  
	        public Int32 segNo;        // 用户自定义段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] tgPos;     // 目标位置
            public double endVel;     // 终点速度
            public double vel;        // 最大速度
            public double synAcc;     // 插补加速度
            public short mask;        // crdAxMask,参与的插补轴,按位表示
            public short lookaheadDis;//是否使用前瞻
            public long  reserved;     // 保留
            public TCrdBufLine(bool init)
            {
                segNo = 0;
                tgPos = new Int32[3];
                endVel = vel = synAcc  = 0;
                mask = lookaheadDis =  0;
                reserved =  0;
            }
        };

        public struct TCrdBufLineXYZA
        {  
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] tgPos;     // 目标位置
            public double endVel;     // 终点速度
            public double vel;        // 最大速度
            public double synAcc;     // 插补加速度
            public long segNo;        // 用户自定义段号
            public short mask;        // crdAxMask,参与的插补轴,按位表示
            public short lookaheadDis;//是否使用前瞻
            public TCrdBufLineXYZA(bool init)
            {
                tgPos = new Int32[4];
                endVel = vel = synAcc  = 0;
                segNo = 0;
                mask = lookaheadDis =  0;
            }
        };

        public struct TCrdBufOut
        {
            public long segNo;     // 用户自定义段号
            public short group;    // Out输出类型 
            public short ch;       // 通道号,取值范围[0,n]
            public long outValue;	// 输出
        };

        public struct TCrdBufDO
        {
            public long segNo;     // 用户自定义段号
            public short group;    // 缓冲区输出DO组 
            public short ch;       // 位序号,取值范围[0,31]
            public long value;     // 输出值,取值范围[0,1]
        };

        public struct TCrdBufDelay
        {
            public long segNo;     // 用户自定义段号
            public long scale;     // 延时单位,0表示单位为毫秒,1表示单位为秒
            public long count;     // 延时时长
        };

        public struct TCrdBufDOEx
        {
            public long segNo;     // 用户自定义段号
            public long group;    // 缓冲区输出DO组
            public long doMask;    // 输出操作位,bit位为1表示要输出
            public long value;     // 输出值,bit位指示输出值
        };

        public struct TCrdBufAxMove
        {
            public long segNo;         // 用户自定义段号
            public short axis;		    // 单轴移动的轴号 [0,n],非坐标系内轴	
            public short soomthCoef;   // smoothCoef:平滑系数
            public short blockEn;      // 是否为阻塞模式 0 不阻塞 ,1 阻塞
            public short synEn;        // 是否为同步模式 0 异步 ,1 同步
            public long  tgPos;	    // 目标位置
            public double vel;         // 运动速度
            public double acc;         // 运动加速度
        };

        public struct TCrdBufArcR
        {
            public long segNo;         // 用户自定义段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] tgPos;		// 目标位置
            public short circleDir;    // 是否为同步模式 0 异步 ,1 同步
            public byte lookaheadDis;	// 是否使用前瞻
            public byte panelIndex;	// 圆弧插补平面：0：XY平面，1：YZ平面，其他：ZX平面
            public double radius;		// 半径
            public double endVel;      // 终点速度
            public double vel;         // 运动速度
            public double acc;         
            public TCrdBufArcR(bool init)
            {
                segNo = 0;
                tgPos = new Int32[2];
                circleDir = 0;
                lookaheadDis = panelIndex  = 0;
                radius = endVel = vel =  acc =   0;
            }
        };

        public struct TCrdBufArcC
        {
            public long segNo;         // 用户自定义段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] tgPos;		// 目标位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] centerPos;	// 目标位置
            public short circleDir;    // 是否为同步模式 0 异步 ,1 同步
            public byte lookaheadDis;	// 是否使用前瞻
            public byte panelIndex;	// 圆弧插补平面：0：XY平面，1：YZ平面，其他：ZX平面
            public double endVel;      // 终点速度
            public double vel;         // 运动速度
            public double acc;         // 运动加速度
            public TCrdBufArcC(bool init)
            {
                segNo = 0;
                tgPos = new Int32[2];
                centerPos = new Int32[2];
                circleDir = 0;
                lookaheadDis = panelIndex  = 0;
                endVel = acc = vel= 0;
            }

        };
 
        public struct TCrdBufLaserSetParam
        {  
            public long segNo;        // 用户自定义段号
            public long onDelay;     // 开光延时
            public long offDelay;     // 关光延时
            public long minValue;     // 最小输出值
            public long maxValue;     // 最大输出值
            public long standbyPower;	// 待机能量
            public short ch;			// 激光控制通道
            public short reserved;		// 保留
        };

        public struct TCrdBufLaserSetFollow
        {  
            public long segNo;        // 用户自定义段号
            public short followType;    // 跟随类型
            public short ch;			// 激光通道号
            public double overRide;     // 跟随倍率
        };

        public struct TCrdBufLaserOnOff
        {  
            public long segNo;        // 用户自定义段号
            public short onOff;		// 开关,0：关光,1：开光
            public short ch;			// 激光通道号
        };

        public struct TCrdBufLaserSetPower
        {  
            public long segNo;        // 用户自定义段号
            public short ch;			// 激光通道号
            public short reserved;		// 保留
            public double power;		// 能量
        };

        public struct TCrdBufSHIOGatePulse
        {          
            public long segNo;        // 用户自定义段号
            public long outCount;		// 输出个数
            public double gateTime;	// gate输出的脉宽，单位：微秒
            public double gateFrq;		// gate输出的频率，单位：HZ
            public short ch;			// ch:通道
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int16[] reserved;  // 保留
            public TCrdBufSHIOGatePulse(bool init)
            {
                segNo = outCount = 0;
                gateTime = gateFrq  = 0;
                ch = 0;
                reserved = new Int16[3];
            }
        };


        public struct TCrdBufDoBitPulse
        {  
            public long segNo;			// 用户自定义段号
            public long highLevelTime;	// 高电平宽度，单位:us
            public long lowLevelTime;	// 低电平宽度，单位:us
            public long outCount;		// 输出脉冲个数
            public long initialLevel;	// 0：先输出低电平，1：先输出高电平
            public short ch;			// ch:通道
            public short enable;		// 1:使能,0:关闭
            public short doType;		// DO类型,默认CRD_BUFF_DO_GPDO1
            public short doIndex;		// DO序号，取值范围[0,n]
        };

        public struct TCrdBufSHIOMinFrq
        {  
            public long segNo;			// 用户自定义段号
            public long minFrq;        // 最小频率，仅当enable=1时有效
            public short ch;			// 通道
            public short setOrClr;		// 1:设置,0:清除
        };

        public struct TCrdBufSHIOSetParam
        {  
            public long segNo;			// 用户自定义段号
            public short ch;			// 通道
            public short reserved;		// 保留
            public double delay;       // 延时开关光时间
            public double gateTime;    // gate打开时间，单位:秒
            public double gateDistance;// 固定模式下的位置间隔，单位pulse
        };

        public struct TCrdBufSHIOGateOnOff
        {          
            public long segNo;			// 用户自定义段号
            public short ch;			// 通道
            public short onOff;		// 1:打开，0：关闭
        };

        public struct TCrdBufWaitEncInPosition
        {  
            public long segNo;			// 用户自定义段号
            public long axisMask;		// 轴掩码
            public long overTime;		// 等待超时时间，单位：毫秒
        };

        public struct TCrdBufWaitDI
        {
            public long segNo;			// 用户自定义段号
            public short index;		// 通道号,取值范围[0,127],前64通道代表通用DI，后64通道代表扩展IO
            public short level;		// 等待值
            public long waitLastTime;  //超时,单位：毫秒
        };

        /// <summary>
        /// 打包插补数据
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pBufData "> 插补数据结构存储数组</param>
        /// <param name="dataLen">数据长度</param>
        /// 压入数据时，先压入指令字，然后再压入指令字对应的工作数据，总的数据长度不超过1000个字节
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufDataPack(HAND crdHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1000)] byte[] pBufData, Int16 dataLen);

        //----------------------------------------------------------
        //	6.8 坐标系的状态检测
        //----------------------------------------------------------

        /// <summary>
        /// 坐标系状态字位定义
        /// </summary>
        public const Int32 BIT_CORD_BUSY = (0x00000001);    // bit 0 , 运动:1 ，静止 0,立即运动下运动停止，完成 
        public const Int32 BIT_CORD_MVERR = (0x00000002);    // bit 1 , 运动出错，或当前运动指令无法启动，需要软件复位    
        public const Int32 BIT_CORD_EMPTY = (0x00000004);    // bit 2 , 缓冲区空　       
        public const Int32 BIT_CORD_FULL = (0x00000008);    // bit 3 , 缓冲区满　               
        public const Int32 BIT_CORD_NODATASTOP = (0x00000010);    // bit 4 , 缓冲区空异常停止或者急停　    
        public const Int32 BIT_CORD_SDRAM_HWERR = (0x00000020);    // bit 5 , 插补缓冲区硬件或者其他错误    

        /// <summary>
        /// 读取坐标系状态
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pStsWord">返回状态字，参考宏定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetSts(HAND crdHandle, ref Int16 pStsWord);

        /// <summary>
        /// 读取规划位置XYZ
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="cnts">读取个数，1~N</param>
        /// <param name="pos">返回坐标数组</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetPrfPos(HAND crdHandle, Int16 cnts, ref  Int32 pos);

        /// <summary>
        /// 坐标系模式下，读取多个轴的机械坐标位置
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="cnts">读取个数，1~N</param>
        /// <param name="pPosArray">返回坐标数组</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetAxisPos(HAND crdHandle, Int16 cnts, ref  Int32 pPosArray);

        /// <summary>
        /// 获取坐标系合成速度
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pVel">坐标系合成速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetVel(HAND crdHandle, ref  Double pVel);

        /// <summary>
        /// 读取编码器位置
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="cnts">读取个数，1~N</param>
        /// <param name="pos">返回坐标数组</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetEncPos(HAND crdHandle, Int16 cnts, ref  Int32 pos);

        /// <summary>
        /// 坐标系状态打包结构
        /// </summary>
        public struct TPackedCrdSts4
        {
            public Int16 crdSts;          // 坐标系状态
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int16[] axSts;         // 坐标系里各轴状态    
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] prfPos;        // 用户坐标系下的规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] axisPos;       // 机械坐标系下的规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] encPos;        // 编码器位置
            public Int32 userSeg;         // 运行的缓冲区段号
            public double prfVel;         // 运动速度
            public Int32 gpDi;             // 通用输入0~31
            public Int32 gpDo;             // 通用输出0~31
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int16[] motDi;         // 限位、原点、报警。请参考专用IO位定义( 搜索 BIT_AXMTIO_LMTN )
            public Int16 reserved;        // 保留
            public Int32 crdFreeSpace;    // 缓冲区剩余空间
            public Int32 reserved2;
            public TPackedCrdSts4(bool init)
            {
                crdSts = 0;
                axSts = new Int16[4];
                prfPos = new Int32[4];
                axisPos = new Int32[4];
                encPos = new Int32[4];
                userSeg = 0;
                prfVel = 0;
                gpDi = 0;
                gpDo = 0;
                motDi = new Int16[4];
                reserved = 0;
                crdFreeSpace = 0;
                reserved2 = 0;
            }
        };

        /// <summary>
        /// 坐标系运动模式下，打包读取控制器状态
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="cnts">读取个数，1~N</param>
        /// <param name="pPackSts">返回坐标系状态打包结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetStsPack4(HAND crdHandle, Int16 cnts, ref  TPackedCrdSts4 pPackSts);

        /// <summary>
        /// 读取指令缓冲区空闲长度
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pRes">返回空闲的长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufGetFree(HAND crdHandle, ref  Int32 pRes);

        /// <summary>
        /// 读取指令缓冲区已用长度
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pLen">返回缓冲区中还未执行的指令个数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufGetUsed(HAND crdHandle, ref  Int32 pLen);

        /// <summary>
        /// 读段号
        /// </summary>
        /// <param name="crdHandle"></param>
        /// <param name="pSegNo">返回的当前段号</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetUserSegNo(HAND crdHandle, ref  Int32 pSegNo);

        /// <summary>
        /// 读取总共压了多少条指令
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pCnt">返回指令数量</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetBufAllCmdCnt(HAND crdHandle, ref  Int32 pCnt);

        /// <summary>
        ///  坐标系状态字位定义:内部扩展
        /// </summary>
        public const Int32 BIT_CORD_POSREC = (0x00000040);  // bit 6  , 伺服位置到达，步进模式时位置到达，伺服模式时实际位置到达误差限    
        public const Int32 BIT_CORD_AUXAXIS_BUSY = (0x00000080);  // bit 7  , 坐标系运动中的关联轴启动前处于运动状态错误
        public const Int32 BIT_CORD_AUXAXIS_ERR = (0x00000100);  // bit 8  , 插补辅助轴错误             
        public const Int32 BIT_CORD_AXIS_ERR = (0x00000200);  // bit 9  , 插补轴存在报警错误（如限位、驱动报警）   
        public const Int32 BIT_CORD_SDRAM_CALC_ERR = (0x00000400);  // bit 10 , SDRAM缓冲区计算错误  

        /// <summary>
        /// 读取内部坐标系状态
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pStsWord">返回状态字，定义64bits(二维的long数组)，便于后续扩展状态位</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetInnerSts(HAND crdHandle, ref Int64 pStsWord);

        /// <summary>
        /// 获取插补缓冲区中尚未完成的总位移量
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pLen">长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetBufLeftLength(HAND crdHandle, ref double pLen);

        //----------------------------------------------------------
        //	6.9 其他指令
        //----------------------------------------------------------

        /// <summary>
        /// 坐标系缓冲运动启动
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdStartMtn(HAND crdHandle);

        /// <summary>
        /// 平滑停止插补运动 注：并不清空指令缓冲区。需要再次启动才能继续运行缓冲区指令。
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdStopMtn(HAND crdHandle);

        /// <summary>
        /// 清坐标系运动错误状态，同时清除所包含轴的错误状态
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdClrError(HAND crdHandle);

        /// <summary>
        /// 指令缓冲区清空
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufClr(HAND crdHandle);

        /// <summary>
        /// 结束(指令压入)缓冲区运动（等待运动完后才结束区运动，并置空闲标志）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdEndMtn(HAND crdHandle);

        /// <summary>
        /// 设置坐标系速度倍率
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="overRide">坐标系速度倍率</param>
        /// <returns></returns
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetOverRide(HAND crdHandle, double overRide);

        /// <summary>
        /// 获取坐标系速度倍率
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pOverRide">坐标系速度倍率</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetOverRide(HAND crdHandle, ref double pOverRide);

        /// <summary>
        /// 设置轴缓冲区运动偏移
        /// 注：会同时修改坐标系内相关轴的运动偏移！
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="count">设置偏移的轴数</param>
        /// <param name="pOffsetArray">缓冲区运动偏移，数组</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetOffset(HAND crdHandle, short count, ref Int32 pOffsetArray);

        /// <summary>
        /// 急停插补运 注：并不清空指令缓冲区
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdEstopMtn(HAND crdHandle);

        /// <summary>
        /// 坐标系缓冲运动回到断点
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="acc">回断点使用的加速度</param>
        /// <param name="vel">回断点使用的速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGotoBreak(HAND crdHandle, double acc, double vel);


        /// <summary>
        /// 急设置坐标系剩余位移量，用于控制激光能量跟随后的关闭波形输出
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetLeftLenForLaserWaveOff ( HAND crdHandle,double length );

       /// <summary>
        /// 开始计算缓冲区执行时间
        /// 注意:1.如果需要计算执行时间，需要使用上位机前瞻
        /// 2.开始后，所有的缓冲区指令，都不会压入控制器
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16  NMC_CrdStartExeTimeCalc();

        /// <summary>
        /// 读取缓冲区指令的执行时间，并停止计算，单位:ms
        /// </summary>
        /// <param name="pTime">执行时间</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetExeTime(  ref double pTime);

        /// <summary>
        /// 设置是否计算所有线段长度
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="flag">启用,1：启动,0：不启用，默认启用</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetBufLengthFlag(HAND crdHandle, Int16 flag);

        //----------------------------------------------------------
        //	7.IO资源访问
        //----------------------------------------------------------

        /// <summary>
        /// 设置通用输出(按通道,支持超过32位),带默认group
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="value">设置32路通用数字量输出。对应bit位 为1, 输出高电平，0，输出低电平</param>
        /// <param name="groupID">DO组，取值范围[0,n],,0: 本地DO31~DO0, 1: 本地DO63~DO32，其他指扩展IO模块</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOGroup(HAND devHandle, Int32 value, short groupID);

        /// <summary>
        /// 按位设置通用输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="bitIndex">取值范围[0,n],位序号</param>
        /// <param name="value">1, 输出高电平，0，输出低电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOBit(HAND devHandle, Int16 bitIndex, Int16 value);

        /// <summary>
        /// 读取通用输出(按通道,支持超过32位),带默认group
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pDoValue">返回32路通用数字量输出值。对应bit位为 1, 高电平，0，低电平</param>
        /// <param name="groupID">DO组，取值范围[0,n],,0: 本地DO31~DO0, 1: 本地DO63~DO32，其他指扩展IO模块</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDOGroup(HAND devHandle, out Int32 pDoValue, short groupID);

        /// <summary>
        /// 读通用输入(按通道,支持超过32位) ,带默认group
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pInValue">返回32路通用数字量输入值。对应bit位为 1, 高电平，0，低电平</param>
        /// <param name="groupID">DI组，取值范围[0,n],0: 本地DI31~DI0, 1: 本地DI63~DI32，其他指扩展IO模块</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDIGroup(HAND devHandle, out Int32 pInValue, short groupID);

        /// <summary>
        /// 按位读通用输入 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="bitIndex">取值范围[0,n],位序号</param>
        /// <param name="bitValue">数字量输入值。1, 高电平，0，低电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDIBit(HAND devHandle, Int16 bitIndex, ref Int16 bitValue);

        /// <summary>
        ///  专用IO定义，arrive alm : home : limit+ , limit- 
        /// </summary>
        public const Int32 BIT_AXMTIO_LMTN = 0x00000001;    // bit 0  ,负向限位  
        public const Int32 BIT_AXMTIO_LMTP = 0x00000002;    // bit 1  ,正向限位 
        public const Int32 BIT_AXMTIO_HOME = 0x00000004;    // bit 2  ,原点 
        public const Int32 BIT_AXMTIO_ALARM = 0x00000008;    // bit 3  ,驱动报警
        public const Int32 BIT_AXMTIO_ARRIVE = 0x00000010;    // bit 4  ,电机到位

        /// <summary>
        /// 读运动控制专用IO
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="inValue">返回专用IO的状态，原点，限位，报警。参考位定义。对应位为0为低电平，1为高电平。</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetMotionIO(HAND axisHandle, ref  Int32 inValue);

        /// <summary>
        /// 读运动控制专用IO,逻辑电平
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="inValue">返回专用IO的状态，原点，限位，报警。参考位定义。对应位为0为低电平，1为高电平。</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetMotionIOLogical(HAND axisHandle, ref  Int32 inValue);

        /// <summary>
        /// 按位设置通用输入信号取反
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="bitIndex">取值范围[0,n],位序号,前64位为本地的通用输入,大于64为扩展Di</param>
        /// <param name="revs">是否取反,1：取反,0：不取反</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDIBitRevs(HAND devHandle, short bitIndex, short revs);

        /// <summary>
        /// 按位读取通用输入信号取反
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="bitIndex">取值范围[0,n],位序号,前64位为本地的通用输入,大于64为扩展Di</param>
        /// <param name="revs">是否取反,1：取反,0：不取反</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDIBitRevs(HAND devHandle, short bitIndex, ref Int16 revs);

        /// <summary>
        /// 按位设置通用输出信号取反
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="bitIndex">取值范围[0,n],位序号,前64位为本地的通用输入,大于64为扩展Di</param>
        /// <param name="revs">是否取反,1：取反,0：不取反</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOBitRevs(HAND devHandle, short bitIndex, short revs);

        /// <summary>
        /// 按位读取通用输出信号取反
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="bitIndex">取值范围[0,n],位序号,前64位为本地的通用输入,大于64为扩展Di</param>
        /// <param name="revs">是否取反,1：取反,0：不取反</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDOBitRevs(HAND devHandle, short bitIndex, ref Int16 revs);

        /// <summary>
        /// DO输出定时脉冲
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="bitIndex">取值范围[0,n],位序号,前64位为本地的通用输入,大于64为扩展Di</param>
        /// <param name="value">设置通用数字量输出。1, 输出高电平,0,输出低电平</param>
        /// <param name="reverseTime">持续的电平,单位:毫秒</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOBitAutoReverse(HAND devHandle, short bitIndex, short value, short reverseTime);

        /// <summary>
        ///  获取扩展IO模块的在线状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="sts">返回扩展IO模块的在线状态，对应模块地址bit位 1在线，0不在线</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetIOModuleSts(HAND devHandle, ref UInt32 sts);

        /// <summary>
        ///  扩展模块类型定义
        /// </summary>
        public const Int32 IOMODULE_TYPE_IO64	=	1;		// 32DI32DO模块（包括16DI16DO模块）
        public const Int32 IOMODULE_TYPE_IO32_DA  = 2;		// 4AD4DA模块

        /// <summary>
        ///  设置扩展IO模块有效(带模块类型)
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="chDevId">设备ID</param>
        ///  <param name="chDevType">模块类型，见宏定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16  NMC_IOModuleSetEn( HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1440)] byte chDevId,Int16 chDevType);

        /// <summary>
        ///  读取扩展IO模块类型
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="chDevId">设备ID</param>
        ///  <param name="pChDevType">模块类型，见宏定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_IOModuleGetType( HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1440)] byte chDevId,ref Int16 pChDevType);

        /// <summary>
        /// 设置伺服ON, 轴静止时执行，如果后面是update指令，需要延时一个周期
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSvOn(HAND axisHandle);

        /// <summary>
        ///  设置伺服OFF, 轴静止时执行，如果后面是update指令，需要延时一个周期
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSvOff(HAND axisHandle);

        /// <summary>
        /// 设置伺服报警清除
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="swt">设置开关有效状态。1, 有效（输出低电平），0，无效（输出高电平）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSvClr(HAND axisHandle, Int16 swt);

        /// <summary>
        /// 设置DAC（模拟量输出）通道的模式
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch"> 模拟量通道号,取值范围[0,n]</param>
        /// <param name="mode"> 模拟量输出范围,0:0~5V, 1:0~10V, 2: 0~10.8V, 3:+/-5V, 4:+/-10V, 5:+/-10.8V,其他无效，默认为4</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDacMode(HAND devHandle, short ch, short mode);

        /// <summary>
        /// 设置DAC（模拟量输出）通道的模式
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch"> 读取DAC（模拟量输出）通道的模式/param>
        /// <param name="pMode"> 返回的模拟量输出范围设定值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDacMode(HAND devHandle, short ch, ref short pMode);

        /// <summary>
        /// 设置Adc参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch"> 模拟量通道号,0~7表示轴通道上的AD，256：表示扩展AD/param>
        /// <param name="range"> 模拟量范围,0:0~5V, 1:0~10V, 2: 0~10.8V, 3:+/-5V, 4:+/-10V, 5:+/-10.8V,默认为0；注意：目前只支持+/-10V</param>
        /// <param name="filterCoe"> 滤波系数，取值范围[0,16],0表示取消Adc滤波，单位:ms，默认值为0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetAdcMode(HAND devHandle, short ch, short range, short filterCoe);

        /// <summary>
        /// 读取Adc参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch"> 模拟量通道号,0~7表示轴通道上的AD，256：表示扩展AD/param>
        /// <param name="pRange"> 模拟量范围</param>
        /// <param name="pFilterCoe"> 滤波系数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetAdcMode(HAND devHandle, short ch, ref short pRange, ref short pFilterCoe);

        /// <summary>
        /// 设置DAC（模拟量输出）输出值
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch"> 模拟量通道号，取值范围[0,n]/param>
        /// <param name="dacValue"> 模拟量输出值,取值范围[-32768,32767],对应DAC输出范围</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDac(HAND devHandle, short ch, short dacValue);

        /// <summary>
        ///  读取DAC（模拟量输出）输出值
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch"> 模拟量通道号，取值范围[0,n]/param>
        /// <param name="pDacValue">:返回模拟量输出值,取值范围[-32768,32767],对应DAC输出范围</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDac(HAND devHandle, short ch, ref short pDacValue);

        /// <summary>
        ///  读取ADC（模拟量输入）值
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch"> 模拟量通道号，取值范围[0,n]/param>
        /// <param name="pAdcValue">::返回模拟量输入值，取值范围[-32768,32767],对应Adc输入电压范围</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetAdc(HAND devHandle, short ch, ref short pAdcValue);

        // DIO映射
        public const Int16 DIO_MAP_MAX_NUM = 8;// 最多允许的映射关系

        // 信号类型定义
        public const Int16 DIO_TYPE_GPI = 1;	// 通用输入
        public const Int16 DIO_TYPE_HOME = 2;	// 轴Home信号
        public const Int16 DIO_TYPE_ALM = 3;	// 驱动器报警信号
        public const Int16 DIO_TYPE_LMTN = 4;	// 轴负向限位信号
        public const Int16 DIO_TYPE_LMTP = 5;	// 轴正向限位信号
        public const Int16 DIO_TYPE_GPO = 6;	// 通用输出信号
        public const Int16 DIO_TYPE_SVON = 7;	// 伺服使能信号
        public const Int16 DIO_TYPE_SVCLR = 8;	// 报警清除信号


        // DIo映射参数配置
        public struct TDioMappingCfg
        {
            public byte enable;		// 1：启用，0：禁用
            public byte pinGrp;        // 映射的信号类型
            public byte pinIndex;      // 映射的信号序号
            public byte outEnable;	    // 输出允许       
            public byte newGrp;        // 映射到的信号类型
            public byte newIndex;	    // 映射到的信号序号
        };

        /// <summary>
        /// 增加一组映射，映射关系目前最多存在DIO_MAP_MAX_NUM组
        /// </summary>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDioMapping(HAND devHandle, ref TDioMappingCfg pDioCfg);

        /// <summary>
        ///  获取所有的DIO映射数据   
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pDioCfg">映射数据数组,目前为DIO_MAP_MAX_NUM组</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetAllDioMapping(HAND devHandle, ref TDioMappingCfg pDioCfg);

        /// <summary>
        /// 清除所有的DIO映射关系
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ClrAllDioMapping(HAND devHandle);

        //----------------------------------------------------------
        //	8.手脉功能
        //----------------------------------------------------------

        /// <summary>
        /// 启动手轮
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="axis">轴号，取值范围[0,n]</param>
        /// <param name="ratio">跟随倍率，取值范围(0,..]，数值越大，则同样的输入，跟随轴运动距离越长</param>
        /// <param name="acc">跟随的加速度</param>
        /// <param name="vel">跟随的速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetHandWheel(HAND devHandle, Int16 axis, double ratio, double acc, double vel);

        /// <summary>
        /// 选择手轮跟随的编码器通道
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="index">编码器通道编号，取值范围[0,9]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetHandWheelInput(HAND devHandle, Int16 index);

        /// <summary>
        /// 设置手轮跟随的倍率
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ratio">跟随倍率，取值范围(0,..]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetHandWheelRatio(HAND devHandle, double ratio);

        /// <summary>
        /// 退出手轮
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ClrHandWheel(HAND devHandle);

        //----------------------------------------------------------
        //	9.龙门功能
        //----------------------------------------------------------

        /// <summary>
        /// 设置龙门主动轴
        /// </summary>
        /// <param name="axisHandle">龙门主动轴句柄</param>
        /// <param name="group">龙门组号</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetGantryMaster(HAND axisHandle, short group);

        /// <summary>
        /// 设置龙门从动轴
        /// </summary>
        /// <param name="axisHandle">龙门从动轴句柄</param>
        /// <param name="group">龙门组号</param>
        /// <param name="gantryErrLmt">龙门保护误差,单位：脉冲</param>
        /// <returns></returns
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetGantrySlave(HAND axisHandle, short group, Int32 gantryErrLmt);

        /// <summary>
        /// 龙门功能关闭
        /// </summary>
        /// <param name="axisHandle">龙门轴句柄</param>
        /// <param name="group">龙门组号</param>
        /// <returns></returns>     
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DelGantryGroup(HAND axisHandle, short group);

        //----------------------------------------------------------
        //	10.位置比较输出
        //----------------------------------------------------------

        //----------------------------------------------------------
        //	10.1 多维位置比较输出（软件比较）
        //----------------------------------------------------------
                /// <summary>
        /// 位置比较最大通道数定义
        /// </summary>
        public const Int16 CMP_OUTPUT_CHN_MAX = (3);
        /// <summary>
        /// X维位置点比较结构体
        /// </summary>
        public struct TCompXDimensParam
        {
            public Int16 dimens; 		                    // 维度
            public Int16 axMask;                           // 使用的轴，按位
            public Int16 src; 		                        // 轴位置类型 ：0规划1：编码器
            public Int16 outCnts; 							// 输出数量 		
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
            public Int16[] outType;      // 输出方式0：脉冲1：电平
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
            public Int16[] outChnType;      // 通道类型：0 GPO， 1  GATE通道
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
            public Int16[] outIndex;      	// GPO：0~63  GATE：0   
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
            public Int16[] outStLevel;      // 电平模式下的起始电平（0或1）	
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
            public Int16[] outGateTime;      // 脉冲模式下的脉冲时间:单位ms	
            public Int16 errZone; 			  // 进入比较点容差半径范围（pulse）
            public TCompXDimensParam(bool bInit)
            {
                dimens = axMask = src = outCnts = 0;
                outType = new Int16[CMP_OUTPUT_CHN_MAX];
                outChnType = new Int16[CMP_OUTPUT_CHN_MAX];
                outIndex = new Int16[CMP_OUTPUT_CHN_MAX];
                outStLevel = new Int16[CMP_OUTPUT_CHN_MAX];
                outGateTime = new Int16[CMP_OUTPUT_CHN_MAX];
                errZone = 0;
            }
        } ;

        /// <summary>
        /// 功能：设置X维位置比较的参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="param">X维比较参数</param>
        /// <param name="chn">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompXDimensSetParam(HAND devHandle, ref TCompXDimensParam param, Int16 chn);

        /// <summary>
        /// 功能：获取X维位置比较的参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="param">X维比较参数</param>
        /// <param name="chn">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompXDimensGetParam(HAND devHandle, ref  TCompXDimensParam param, Int16 chn);

        /// <summary>
        /// 功能：设置X维位置比较的输出模式
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="outMode">输出模式 0：同时输出模式 1： 轮循输出模式</param>
        /// <param name="chn">位置比较通道号 取值[0,3]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompXDimensSetCmpOutMode(HAND devHandle, Int16 outMode, Int16 chn);

        /// <summary>
        /// 功能：设置X维比较数据
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pPosArray">比较数组地址，注：若是1维比较，则pPosArray传入一维数组地址，若是2维比较，则pPosArray应传入2维数组地址</param>
        /// <param name="count">位置点数，注：若为2维数组比较时，每两个数据为一个点数</param>
        /// <param name="chn">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompXDimensSetData(HAND devHandle, Int32[] pPosArray, Int16 count, Int16 chn);

        /// <summary>
        /// 功能：X维位置比较开关
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="onOff">0 停止，1输出</param>
        /// <param name="chn">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompXDimensOnoff(HAND devHandle, Int16 onOff, Int16 chn);

        /// <summary>
        /// 功能：获取X维位置比较的输出状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pStatus">返回比较状态 0 未启动比较 1 比较输出中</param>
        /// <param name="pOutCount">已经比较输出的个数</param>
        /// <param name="chn">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompXDimensStatus(HAND devHandle, ref Int16 pStatus, ref Int16 pOutCount, Int16 chn);

        //----------------------------------------------------------
        //	10.2 一维高速位置比较（硬件比较）
        //----------------------------------------------------------


        public const Int16 CMP_HS1DIMENS_CHN_MAX	= 4;         // 最大支持四个通道
        public const Int16 CMP_HS1DIMENS_POS_MAX	= 8;         // 每个通道最多支持8个位置点

         /// <summary>
        /// 一维高速位置比较参数结构
        /// </summary>
        public struct TCompHs1DimensParam
        {
            public Int16 dirNo; 		                    // 编码器源（0~7），对应轴0~7的编码器。
            public Int16 out1StLevel;                           // out1起始电平（0或1）
            public Int16 out2StLevel; 		                    // out2起始电平（0或1）
            public Int16 reserved; 							// 保留		
            public Int32 out1Width; 							// out1脉冲宽度，0~65535,单位us 
	        public Int32 out2Width; 							// out2脉冲宽度，0~65535,单位us
            public Int32 out2Delay; 							// out2延时，0~65535,单位us 

            public TCompHs1DimensParam(bool bInit)
            {
                dirNo = out1StLevel = out2StLevel = reserved = 0;
                out1Width = out2Width = out2Delay =  0;
            }
        } ;

        /// <summary>
        /// 功能：设置高速1维位置比较的参数（比较编码器）
        /// 注：1)需要先设置编码器模式，可设置外部编码器或内部计数。
        /// 2)比较输出引脚位置请参考硬件手册。
        /// 3)out1比较位置到后马上输出, out2到位后延时输出。
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="param">一维比较参数</param>
        /// <param name="chn">通道号,范围0~3</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompHs1DimensSetParam(HAND devHandle, ref TCompHs1DimensParam param, Int16 chn);

        /// <summary>
        /// 功能：读取高速1维位置比较的参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="param">一维较参数</param>
        /// <param name="chn">通道号,范围0~3</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompHs1DimensGetParam(HAND devHandle, ref TCompHs1DimensParam param,Int16 chn);


        /// <summary>
        /// 功能：设置高速1维比较数据
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pArrayPos">比较数组地址</param>
        /// <param name="count">位置点数</param>
        /// <param name="chn">通道号,范围0~3</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompHs1DimensSetData(HAND devHandle,Int32[] pArrayPos,Int16 count,Int16 chn);

        /// <summary>
        /// 功能：高速1维位置比较使能
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="onOff"> 0:停止,1:输出,2:手动输出（必须在空闲状态下,也需要提前设置参数）</param>
        /// <param name="chn">通道号,范围0~3</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompHs1DimensOnOff(HAND devHandle, Int16 onOff,Int16 chn);

        /// <summary>
        /// 功能：高速1维位置比较的状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pBusy"> 0 未启动比较 1 比较输出中</param>
        /// <param name="pOutCount">输出的个数</param>
        /// <param name="pWaitCnts">等待比较的个数</param>
        /// <param name="pFreeCnts">缓冲区空闲的个数</param>
        /// <param name="chn">通道号,范围0~3</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CompHs1DimensStatus(HAND devHandle, ref Int16 pBusy, ref Int16 pOutCount, ref Int16 pWaitCnts, ref Int16 pFreeCnts, Int16 chn);

        //----------------------------------------------------------
        //	11.激光功能
        //----------------------------------------------------------

        //----------------------------------------------------------
        //	11.1 激光模式设置
        //----------------------------------------------------------

        /// <summary>
        /// 激光输出模式
        /// </summary>
        public const Int32 LASER_DISABLE_MODE = (0);   // 禁用激光功能
        public const Int32 BASIC_OUTPUT_MODE = (1);   // 基本控制模式
        public const Int32 TIME_ARRAY_OUTPUT_MODE = (2);   // 波形控制模式
        public const Int32 SHIO_OUTPUT_MODE = (3);   // 位置比较控制模式

        /// <summary>
        /// 设置激光输出的模式。该模式的设定，约束相应指令的功能和操作
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="mode">激光输出模式，参考上述宏定义</param>
        /// <param name="ch">激光控制通道，取值范围[0,n]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetMode(HAND devHandle, Int16 mode, Int16 ch);

        //----------------------------------------------------------
        //	11.2 基本控制模式
        //----------------------------------------------------------

        /// <summary>
        ///  激光物理信号输出类型配置(高精度)
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="outputType">输出的类型</param>
        /// <param name="index">输出通道序号,取值范围[0,n]</param>
        /// <param name="optionVal">当作为占空比输出时，该值为PWM的频率，单位HZ；当为LASER_PWM_FRQ时，该值作为占空比值，单位为百分比；                        
        ///当为LASER_PWM_FRQ_EXT时，该值为脉宽，单位为微秒，取值范围(0,~)</param>
        /// <param name="ch">激光控制通道，取值范围[0,n]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetOutputTypeEx(HAND devHandle, Int16 outputType, Int16 index, double optionVal, Int16 ch);

        /// <summary>
        /// 功能：设置立即输出激光的能量(高精度)
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="outVal">激光能量（设定值意义与NMC_LaserSetOutputTypeEx设置的输出类型对应）</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetPowerEx(HAND devHandle, double outVal, Int16 ch);

        /// <summary>
        /// 功能：设置立即输出激光的能量 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pVal">激光能量值</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserGetPower(HAND devHandle, ref double pVal, Int16 ch);

        /// <summary>
        /// 设置激光参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="onDelay">开光延时，单位us,取值范围[0,65535]</param>
        /// <param name="offDelay">关光延时，单位us,取值范围[0,65535]</param>
        /// <param name="minValue">最小输出值，取值范围[0,32767]</param>
        /// <param name="maxValue">最大输出值，取值范围[0,32767]</param>
        /// <param name="standbyPower">待机能量,为0表示取消待机能量输出功能</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetParam(HAND devHandle, Int32 onDelay, Int32 offDelay,
            Int32 minValue, Int32 maxValue, Int32 standbyPower, Int16 ch);

        /// <summary>
        /// 读取激光参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="onDelay">开光延时，单位us,取值范围[0,65535]</param>
        /// <param name="offDelay">关光延时，单位us,取值范围[0,65535]</param>
        /// <param name="minValue">最小输出值，取值范围[0,32767]</param>
        /// <param name="maxValue">最大输出值，取值范围[0,32767]</param>
        /// <param name="standbyPower">待机能量,为0表示取消待机能量输出功能</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserGetParam(HAND devHandle, out Int32 onDelay, out Int32 offDelay,
            out Int32 minValue, out Int32 maxValue, out Int32 standbyPower, Int16 ch);

        /// <summary>
        /// 缓冲区设置激光参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segment">段号</param>
        /// <param name="onDelay">开光延时,单位us,取值范围[0,65535]</param>
        /// <param name="offDelay">关光延时,单位us,取值范围[0,65535]</param>
        /// <param name="minValue">最小输出值，DA输出时，范围[0,32767],占空比输出时，范围[0,100],频率输出时，范围[0,2000000]</param>
        /// <param name="maxValue">最大输出值，DA输出时，范围[0,32767],占空比输出时，范围[0,100],频率输出时，范围[0,2000000]</param>
        /// <param name="standbyPower">待机能量,为0表示取消待机能量输出功能</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufLaserSetParam(HAND crdHandle, long segment, long onDelay, long offDelay, long minValue, long maxValue, long standbyPower, short ch);


        /// <summary>
        /// 设置缓冲区激光能量跟随
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="overRide">跟随倍率，为0表示取消激光能量跟随</param>
        /// <param name="followType">跟随类型，0：跟随规划速度，1：跟随实际速度</param>
        /// <param name="ch">激光控制通道，取值范围[0,n]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufLaserSetFollow(HAND crdHandle, Int32 segNo, double overRide, Int16 followType, Int16 ch);

        /// <summary>
        /// 设置缓冲区激光能量跟随滤波
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="powerFilter">能量输出滤波系数，取值范围[0,32]，0：不开启（默认）</param>
        /// <param name="followAdvMode">能量输出计算方法</param>
        /// <param name="ch">激光控制通道，取值范围[0,n]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetFollowParam(HAND devHandle, Int16 powerFilter, Int16 followAdvMode, Int16 ch);

        /// <summary>
        /// 设置激光能量补偿表
        /// </summary>
        /// <param name="devHandle">坐标系句柄</param>
        /// <param name="tableNo">补偿表号 ：支持20张表</param>
        /// <param name="pXCmpPos">X方向轴的比较位置,长度为xCount</param>
        /// <param name="pYCmpPos"> Y方向轴的比较位置,长度为yCount </param>
        /// <param name="xCount">表X方向的长度,取值范围[2,10]</param>
        /// <param name="yCount">表Y方向的长度,取值范围[2,10]</param>
        /// <param name="powerMin">大于该最小能量才补偿</param>
        /// <param name="powerMax">小于该最大能量才补偿</param>
        /// <param name="pLaserCmpPower">补偿表的值，该参数为2维数组的首地址</param>
        /// <param name="chn">对应的激光通道号</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetLaserPowerCmpTable ( HAND devHandle,Int16 tableNo,Int32[] pXCmpPos,Int32[] pYCmpPos,
	                                                           Int16 xCount,Int16 yCount, UInt32 powerMin,UInt32 powerMax, ref UInt32 pLaserCmpPower,Int16 chn);

        /// <summary>
        /// 启动激光能量补偿 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pAxisNo">pAxisNo[0]表X方向的位置比较轴号 pAxisNo[1]表Y方向的位置比较轴号</param>
        /// <param name="chn">补偿输出给哪路激光通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_StartLaserPowerComp ( HAND devHandle,Int16[] pAxisNo,Int16 chn);

        /// <summary>
        /// 停止激光能量补偿 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="chn">补偿输出给哪路激光通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_StopLaserPowerComp(HAND devHandle, Int16 chn);

        /// <summary>
        /// 功能：激光立即输出开关 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="onOff">0：关光，1：开光</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserOnOff(HAND devHandle, Int16 onOff, Int16 ch);

        /// <summary>
        /// 功能：读取当前激光开关状态 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pOnOffState">激光开关状态，1：激光处于打开状态，0:激光处于关闭状态</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserGetOnOff(HAND devHandle, ref short pOnOffState, short ch);

        /// <summary>
        /// 缓冲区激光开关
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="onOff">开关，0：关光，1：开光</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufLaserOnOff(HAND crdHandle, Int32 segNo, Int16 onOff, Int16 ch);

        //----------------------------------------------------------
        //	11.3 波形控制模式
        //----------------------------------------------------------

        /// <summary>
        /// 缓冲区设置激光能量，激光能量跟随模式下,调用这条指令无效
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="power">激光能量</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufLaserPower(HAND crdHandle, Int32 segNo, Int32 power, Int16 ch);

        /// <summary>
        /// 波形输出定义
        /// </summary>
        public const Int32 LASER_GROUP = (12);			// 最大激光能量组数
        public const Int32 LASET_POINT = (40);			// 每组最大点数

        /// <summary>
        /// 波形输出参数定义
        /// </summary>
        public struct TTimeArrayPara
        {
            public Int16 pwmEnable;		    // 是否需要输出PWM
            public Int16 outputType;		// 开关信号输出类型：0：gate，1：GPO
            public Int16 outputCh;			// 开关信号输出通道
            public Int16 reserved;			// 保留
            public Int32 pwmPeriod;			// PWM周期，单位us，不能小于时间数组的总周期
            public Int32 pwmWidth;			// PWM脉宽，单位us,此参数保留，脉宽等于时间数组的总周期
        };

        /// <summary>
        /// 设置时间数组输出参数
        /// 注：1.仅时间数组输出方式下有效
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pPara">波形输出参数</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetTimeArrayPara(HAND devHandle, ref TTimeArrayPara pPara, Int16 ch);

        /// <summary>
        /// 激光焊接的类型定义
        /// </summary>
        public const Int32 SPOT_WELDING = (0);		// 点焊
        public const Int32 LINE_WELDING = (1);		// 线焊

        /// <summary>
        /// 波形输出数据结构体
        /// </summary>
        public struct TLaserPower
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LASET_POINT)]
            public Int16[] time;		// 每个点之间的间隔时间
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LASET_POINT)]
            public Int16[] power;		// 各点的能量大小
            public Int16 count;					// 实际压入点数
        }

        /// <summary>
        /// 设置激光的能量，对于立即输出模式，group无效，pLaserPower为指定的单个能量信息
        /// 注：1.即波形设置 
        /// 2.该接口对于SHIO模式无效
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="group">设置哪一组激光能量</param>
        /// <param name="pLaserPower">设置的数据</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetTimeArrayPower(HAND devHandle, Int16 group, ref TLaserPower pLaserPower, Int16 ch);

        /// <summary>
        /// 执行时间序列激光,只在时间序列输出模式下有效
        /// 注：1.启用时间数组输出后，直到最后一个点，如果最后一个点能量不为0，则维持最后一个能量输出
        /// 2.如果最后一个点能量为0，且没有PWM输出配合，则自动关闭激光。有PWM配合则需要调用NMC_LaserOnOff或者NMC_CrdBufLaserOnOff关闭激光
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="group">设置哪一组激光能量</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserTimeArrayExe(HAND devHandle, Int16 group, Int16 ch);

        /// <summary>
        /// 缓冲区执行时间序列激光,只在时间序列输出模式下有效
        /// 注：1.启用时间数组输出后，直到最后一个点，如果最后一个点能量不为0，则维持最后一个能量输出
        /// 2.如果最后一个点能量为0，且没有PWM输出配合，则自动关闭激光。有PWM配合则需要调用NMC_LaserOnOff或者NMC_CrdBufLaserOnOff关闭激光
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="group">设置哪一组激光能量</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufLaserTimeArrayExe(HAND crdHandle, Int32 segNo, Int16 group, Int16 ch);

        //----------------------------------------------------------
        //	11.4 位置比较控制模式
        //----------------------------------------------------------

        /// <summary>
        /// SHIO参数结构体
        /// </summary>
        public struct TSHIOPara
        {
            public Int16 isArray; 		// 是否固定间距还是数组。0：固定间距（仅支持），默认0
            public Int16 outMode; 		// 输出模式。默认1
            // 1：只输出gate 立即开或关,
            // 2: 根据位移输出gate。( 优先 )
            // 3: 根据位移输出gate，gate 和同部trigger 信号同步
            // 4: 根据位移输出gate，gate 和信号输入同步
            public Int16 posSrc; // 比较模式，外部编码器还是内部规划值。
            // 0：外部编码器（优先），1：内部规划值。
            // 默认0。
            public Int16 axisMask; 	// 轴号，按bit 位对应。（一般两个轴）。
            // 默认 0。
            public double delay; 		// 延时开关光时间( 暂不用 )，单位：s。默认 0。
            public double gateTime; 	// 设置gate 打开时间，单位：s（内部最小值：1/36us ）。
            // 默认0。
            public Int32 gateDistance; 	// 固定模式下的位置间隔 单位：pulse。
            // 默认0，模式2~4 下会进行有效性检查。
            public Int32 reserved1;		// 保留参数，应设为0。
            public Int32 reserved2; 	// 保留参数，应设为0。
            public Int32 reserved3; 	// 保留参数，应设为0。
            public Int32 reserved4; 	// 保留参数，应设为0。
            public Int32 reserved5; 	// 保留参数，应设为0。
        }

        /// <summary>
        /// 配置SHIO功能的参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pSHIOPara">配置参数结构体</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOConfigPara(HAND devHandle, ref TSHIOPara pSHIOPara, Int16 ch);

        /// <summary>
        /// 缓冲区配置SHIO功能的参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segment">段号</param>
        /// <param name="delay">延时开关光时间( 暂不用 ),单位：s。默认 0。</param>
        /// <param name="gateTime">设置gate 打开时间,单位：s（内部最小值：1/36us ）,取值范围(0,0.0009),默认0。</param>
        /// <param name="gateDistance">固定模式下的位置间隔 单位：pulse。</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSHIOSetParam(HAND crdHandle, Int32 segment, double delay, double gateTime, double gateDistance, Int16 ch);

        /// <summary>
        /// SHIO输出的最小频率功能开
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="minFrq">最低频率，单位HZ</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOEnableMinFrq  (HAND devHandle,Int32 minFrq,Int16 ch);

        /// <summary>
        /// SHIO输出的最小频率功能关闭
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIODisableMinFrq  (HAND devHandle,Int16 ch);

        /// <summary>
        /// 缓冲区SHIO输出的最小频率功能开
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="segment">段号</param>
        /// <param name="minFrq">最低频率，单位HZ</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSHIOSetMinFrq(HAND crdHandle, Int32 segment, Int32 minFrq,Int16 ch);

        /// <summary>
        /// 缓冲区SHIO输出的最小频率功能关闭
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="segment">段号</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSHIOClrMinFrq(HAND crdHandle, Int32 segment, Int16 ch);

        /// <summary>
        /// 切换SHIO比较轴（允许和坐标系不完全一致）
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="axisMask">轴号，按bit 位对应。（一般两个轴）</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOChangeAxisMask(HAND devHandle, Int16 axisMask, Int16 ch);

        /// <summary>
        /// 切换轴（允许和坐标系不完全一致）,缓冲区指令
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="segment">段号</param>
        /// <param name="axisMask">轴号，按bit 位对应。（一般两个轴）</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSHIOChangeAxisMask(HAND crdHandle, Int32 segment, Int16 axisMask, Int16 ch);

        /// <summary>
        /// 允许GATE输出
        /// 注：设置后根据模式输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOGateOn(HAND devHandle, Int16 ch);

        /// <summary>
        /// 禁止GATE输出
        /// 注：设置后立即禁止输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOGateOff(HAND devHandle, Int16 ch);

        /// <summary>
        /// 设置Trigger 输出 注：设置后立即输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="freq">triger 脉冲频率单位：HZ</param>
        /// <param name="width">triger 脉冲宽度，单位：s（内部最小值：1/36us ）。默认0。</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOTriggerOn(HAND devHandle, double freq, double width, Int16 ch);

        /// <summary>
        /// 禁止Trigger输出 注：设置后马上关闭输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOTriggerOff(HAND devHandle, Int16 ch);

        /// <summary>
        /// 缓冲区允许GATE输出
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BufSHIOGateOn(HAND crdHandle, Int32 segNo, Int16 ch);

        /// <summary>
        ///  缓冲区禁止GATE输出 注：缓冲区执行到该指令后立即禁止输出
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BufSHIOGateOff(HAND crdHandle, Int32 segNo, Int16 ch);

        /// <summary>
        ///  PWM映射到GATE引脚输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pwmCh">pwm 通道号</param>
        /// <param name="gateCh">gate信号通道号</param>
        /// <param name="onOff">0 -- 不映射，1 -- 映射 （默认为0）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetPwmToGate(HAND devHandle, Int16 pwmCh, Int16 gateCh, Int16 onOff);

        /// <summary>
        ///  缓冲区SHIO点动出光，输出一段gate脉冲,注意：必须在激光SHIO控制模式下使用
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="gateTime">gate输出的脉宽，单位：微秒</param>
        /// <param name="gateFrq">gate输出的频率，单位：HZ</param>
        /// <param name="outCount">输出个数</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSHIOGatePulse(HAND crdHandle, Int32 segNo, double gateTime, double gateFrq, Int32 outCount, Int16 ch);

        /// <summary>
        ///  SHIO点动出光，输出一段gate脉冲,注意：必须在激光SHIO控制模式下使用
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="gateTime">gate输出的脉宽，单位：微秒</param>
        /// <param name="gateFrq">gate输出的频率，单位：HZ</param>
        /// <param name="outCount">输出个数,设为0表示持续输出</param>
        /// <param name="ch">通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOGatePulse(HAND devHandle, double gateTime, double gateFrq, Int32 outCount, Int16 ch);

        /// <summary>
        ///  输出一段PWM脉冲,注意：必须在激光控制模式下使用
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="onTime">PWM输出的脉宽，单位：微秒</param>
        /// <param name="pwmFrq">PWM输出的频率，单位：HZ</param>
        /// <param name="outCount">输出个数,设为0表示持续输出</param>
        /// <param name="ch">通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_PwmPulseOut(HAND devHandle, double onTime, double pwmFrq, Int32 outCount, Int16 ch);

        //----------------------------------------------------------
        //	12.PT运动
        //----------------------------------------------------------

        /// <summary>
        /// 设置PT的数据模式
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="onOff">1：数据驻存模式   0：数据刷新模式(默认模式)</param>
        /// <param name="loopCount">循环次数 ：仅数据驻存模式下有效</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPtSetStatic(HAND axisHandle, Int16 onOff, Int32 loopCount);

        /// <summary>
        /// 读取PT的数据模式
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pOnOff">返回模式 1：数据驻存模式   0：数据刷新模式(默认模式)</param>
        /// <param name="pLoopCount">返回循环次数 ：仅数据驻存模式下有效</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPtGetStatic(HAND axisHandle, ref Int16 pOnOff, ref Int32 pLoopCount);

        /// <summary>
        /// 查询PT数据剩余空间大小
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pSpace">返回的剩余空间大小</param>
        /// <param name="pUsed">返回已使用的空间段数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPtGetSpace(HAND axisHandle, ref Int16 pSpace, ref Int16 pUsed);


        /// <summary>
        /// PT数据段类型
        /// </summary>
        public const Int16 MT_PT_NORMAL = (0);
        public const Int16 MT_PT_STOP = (1);
        /// <summary>
        /// 运动缓存区中压运动数据段
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="count">压入的数据段数</param>
        /// <param name="pPosArray">段运行距离</param>
        /// <param name="pTimeArray">段运行时间</param>
        /// <param name="pTypeArray">PT数据段类型，见定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPtPush(HAND axisHandle, Int16 count, ref double pPosArray, ref Int32 pTimeArray, ref Int16 pTypeArray);

        /// <summary>
        /// 清空PT数据
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPtBufClr(HAND axisHandle);

        /// <summary>
        /// 启动Pt运动 同步的轴必须属于同一个控制器
        /// </summary>
        /// <param name="devHandle">轴句柄</param>
        /// <param name="otherSynAxCnts">不包括axisHandle 的其他同步启动轴数量</param>
        /// <param name="pOtherSynAxArray">其他同步启动轴的序号：0~N</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPtStartMtn(HAND axisHandle, Int16 otherSynAxCnts, ref Int16 pOtherSynAxArray);

        //----------------------------------------------------------
        //	13.闭环控制
        //----------------------------------------------------------

        /// <summary>
        /// 模拟量输出 Dac参数
        /// </summary>
        public struct TDacMotor
        {
            public Int16 inverse;			// 电压是否取反
            public Int16 bias;				// Dac零漂
            public Int16 dacLmt;			// Dac输出极限值
        };

        /// <summary>
        /// 设置单轴闭环控制的DA参数,轴与DA通道的对应
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pDacPara">模拟量输出 Dac参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetCloseLoopDac(HAND axisHandle, ref TDacMotor pDacPara);

        /// <summary>
        /// 获取单轴闭环控制的DA参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pDacPara">返回 模拟量输出 Dac参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCloseLoopDac(HAND axisHandle, ref TDacMotor pDacPara);

        /// <summary>
        /// 设置单轴的控制模式：默认使用对应轴的编码器作为输入反馈，对应序号的DAC作为输出
        /// 闭环模式下，先调用NMC_SetCloseLoopDac指令
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="mode">控制模式 0 脉冲控制  1 DA闭环控制</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetCtrlMode(HAND axisHandle, Int16 mode);

        /// <summary>
        /// 读取单轴的控制模式
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pMode">返回控制模式 0 脉冲控制  1 DA闭环控制</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCtrlMode(HAND axisHandle, ref Int16 pMode);

        /// <summary>
        /// PID参数结构
        /// </summary>
        public struct TPidPara
        {
            public Single kp;											// 增益系数
            public Single ki;											// 积分系数
            public Single kd;											// 微分系数
            public Single kvff;											// 速度前馈系数
            public Int32 integralLimit;                                 // 积分饱和极限
            public Int32 derivativeLimit;                               // 微分饱和极限
            public Int16 outLimit;                                     // 输出饱和极限
        } ;
        /// <summary>
        /// 设置对应组号的PID参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="index">PID参数组号</param>
        /// <param name="pidPara">PID参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPIDPara(HAND axisHandle, Int16 index, ref TPidPara pidPara);

        /// <summary>
        /// 获取对应组号的PID参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="index">PID参数组号</param>
        /// <param name="pPidPara">返回PID参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPIDPara(HAND axisHandle, Int16 index, ref TPidPara pPidPara);

        /// <summary>
        /// 设置使用哪组PID
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="index">PID参数组号</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPIDIndex(HAND axisHandle, Int16 index);

        /// <summary>
        /// 获取正使用的PID组号
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pIndex">返回 PID参数组号</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPIDIndex(HAND axisHandle, ref Int16 pIndex);

        //----------------------------------------------------------
        //	14 拓展资源及其他指令
        //----------------------------------------------------------

        //----------------------------------------------------------
        //	14.1 辅助编码器
        //----------------------------------------------------------

        /// <summary>
        /// 读编码器通道位置
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="encId">编码器ID,取值范围[0,n]</param>
        /// <param name="pos">返回编码器数值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetEncPos(HAND devHandle, Int16 encId, ref  Int32 pos);

        /// <summary>
        /// 写编码器通道位置
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="encId">编码器ID,取值范围[0,n]</param>
        /// <param name="pos">编码器数值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetEncPos(HAND devHandle, Int16 encId, Int32 pos);

        //----------------------------------------------------------
        //	14.2 捕获功能
        //----------------------------------------------------------

        // 编码器硬件捕获模式选择 
        public const Int16 CAPT_MODE_Z = 0;   // 编码器Z相捕获 
        public const Int16 CAPT_MODE_IO = 1;   // IO 捕获 
        public const Int16 CAPT_MODE_Z_AND_IO = 2;   // IO+Z相 捕获 
        public const Int16 CAPT_MODE_Z_AFT_IO = 3;   // 先IO触发再Z相触发 捕获 

        // 编码器硬件捕获IO源选择 
        public const Int16 CAPT_IO_SRC_HOME = 0;   // 原点输入作为捕获IO 
        public const Int16 CAPT_IO_SRC_LMTN = 1;   // 负向限位输入作为捕获IO 
        public const Int16 CAPT_IO_SRC_LMTP = 2;   // 正向限位输入作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI0 = 3;   // 通用数字输入0作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI1 = 4;   // 通用数字输入1作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI2 = 5;   // 通用数字输入2作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI3 = 6;   // 通用数字输入3作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI4 = 7;   // 通用数字输入4作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI5 = 8;   // 通用数字输入5作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI6 = 9;   // 通用数字输入6作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI7 = 10;  // 通用数字输入7作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI8 = 11;  // 通用数字输入8作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI9 = 12;  // 通用数字输入9作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI10 = 13;  // 通用数字输入10作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI11 = 14;  // 通用数字输入11作为捕获IO 
        public const Int16 CAPT_IO_SRC_DI12 = 15;  // 通用数字输入12作为捕获IO 

        /// <summary>
        /// 设置捕获参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="mode">模式选择，参考宏定义</param>
        /// <param name="ioSrc">IO输入源选择，参考宏定义</param>
        /// <param name="level">触发沿。0为下降沿，1为上升沿</param>
        /// <returns></returns
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetCaptSns(HAND axisHandle, Int16 mode, Int16 ioSrc, Int16 level);

        /// <summary>
        /// 读取捕获参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="mode">返回捕获模式选择</param>
        /// <param name="ioSrc">返回IO输入源选择</param>
        /// <param name="level">返回触发沿。0为下降沿，1为上升沿</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCaptSns(HAND axisHandle, ref Int16 mode, ref Int16 ioSrc, ref Int16 level);

        /// <summary>
        ///  设置启动捕获 注：捕获触发标志在轴状态字里。
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetCapt(HAND axisHandle);

        /// <summary>
        ///  清除轴的捕获状态
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClrCaptSts(HAND axisHandle);

        /// <summary>
        /// 读捕获位置
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pos">返回位置，单位:脉冲</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCaptPos(HAND axisHandle, ref  Int32 pos);

        // 高级捕获最大通道数
        public const Int16 CAPT_EX_MAX_CH = 16; // 高级捕获最大通道数

        // 捕获源
        public const Int16 CAPT_EX_SRC_GPI = 0;   // 通用输入
        public const Int16 CAPT_EX_SRC_NEGLMT = 1;   // 负向限位
        public const Int16 CAPT_EX_SRC_POSLMT = 2;   // 正向限位
        public const Int16 CAPT_EX_SRC_HOME = 3;   // 原点
        public const Int16 CAPT_EX_SRC_Z = 4;   // Z向信号
        public const Int16 CAPT_EX_SRC_PRFPOS = 5;   // 规划位置（当规划位置达到设定值时触发）
        public const Int16 CAPT_EX_SRC_ENCPOS = 6;   // 编码器位置

        public struct TAdvCaptureParam
        {
            public Int16 capPosIndex;	// 捕获的位置源序号,0~N：轴1~N+1。（默认0）注：捕获的编码器位置根据用户设置的编码器模式决定 
            public Int16 trigSrc;		// 触发源,见上面的定义。（默认：CAPT_EX_SRC_GPI）
            public Int16 trigIndex;	    // 触发源序号。（默认0）
            public Int16 filter;		// 滤波时间常数,单位0.1毫秒,取值范围[0,255]
            public Int32 trigValue;	    // 触发值,对于触发源为IO,表示信号触发的有效电平；对于触发源为位置,则表示触发捕获的位置。（默认0）
        };
        /// <summary>
        /// 设置高级捕获参数，并启动捕获
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pCaptureParam">高级捕获参数</param>
        /// <param name="ch">高级捕获通道号，[0,CAPT_EX_MAX_CH)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAdvCaptParam(HAND axisHandle, ref TAdvCaptureParam pCaptureParam, Int16 ch);

        /// <summary>
        /// 清除高级捕获状态,并取消该通道的捕获
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="ch">高级捕获通道号，[0,CAPT_EX_MAX_CH)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClrAdvCaptSts(HAND axisHandle, Int16 ch);

        /// <summary>
        /// 读取高级捕获状态
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="captSts">捕获状态，0：没有捕获，1：捕获完成</param>
        /// <param name="pPos">返回捕获位置，在captSts为1时，位置为捕获到的位置值，单位:脉冲</param>
        /// <param name="ch">高级捕获通道号,[0~3]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAdvCaptPos(HAND devHandle, ref Int16 captSts, ref Int32 pPos, Int16 ch);

        //----------------------------------------------------------
        //	14.3 位置系统操作
        //----------------------------------------------------------

        /// <summary>
        /// 单轴位置系统清零，规划以及编码器
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtZeroPos(HAND axisHandle);

        /// <summary>
        /// 设定轴位置, 轴静止时执行，如果后面是update指令，需要延时一个周期 注：只能在轴静止时使用。
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pos">设定轴位置值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAxisPos(HAND axisHandle, Int32 pos);

        /// <summary>
        /// 设定编码器位置, 轴静止时执行，如果后面是update指令，需要延时一个周期  注：只能在轴静止时使用
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="encPos">设定编码器位置值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetEncPos(HAND axisHandle, Int32 encPos);

        /// <summary>
        /// 设置单轴规划比例系数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="inCoe">单轴规划比例系数，取值范围(0,1]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPrfCoe(HAND axisHandle, double inCoe);

        /// <summary>
        /// 读取单轴规划比例系数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pInCoe">返回单轴规划比例系数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfCoe(HAND axisHandle, ref double pInCoe);

        /// <summary>
        /// 设置单轴编码器的比例系数，默认为1
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="encCoe">单轴编码器比例系数，取值范围(0,1]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetEncCoe(HAND axisHandle, double encCoe);

        /// <summary>
        /// 读取单轴编码器的比例系数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pEncCoe">返回单轴编码器的比例系数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetEncCoe(HAND axisHandle, ref double pEncCoe);

        /// <summary>
        /// 设置轴的到位误差参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="arrivalBand">到位误差，单位Pulse 取值大于0</param>
        /// <param name="stableTime">到位保持时间，单位 ms 取值大于0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAxisArrivalPara(HAND axisHandle, Int32 arrivalBand, Int32 stableTime);

        /// <summary>
        /// 获取轴的到位误差参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pArrivalBand">返回到位误差，单位Pulse</param>
        /// <param name="pStableTime">返回到位保持时间，单位 ms</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAxisArrivalPara(HAND axisHandle, ref Int32 pArrivalBand, ref Int32 pStableTime);

        /// <summary>
        /// 设置单轴急停DI
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="gpiIndex"> gpiIndex：通用输入序号，[0,N]</param>
        /// <param name="sense">触发电平，0：低电平，1：高电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetEstopDI(HAND axisHandle, Int16 gpiIndex, Int16 sense);

        /// <summary>
        /// 读取单轴急停DI
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="gpiIndex"> gpiIndex：通用输入序号，[0,N]</param>
        /// <param name="sense">触发电平，0：低电平，1：高电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetEstopDI(HAND axisHandle, ref Int16 gpiIndex, ref Int16 sense);

        //----------------------------------------------------------
        //	14.4 螺距补偿和间隙补偿
        //----------------------------------------------------------

        /// <summary>
        /// 设置螺距误差的补偿参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="num">补偿的段数,取值范围[2,128]</param>
        /// <param name="startPos">补偿起始位置，终止位置为 startPos + cmpLen</param>
        /// <param name="cmpLen">补偿长度</param>
        /// <param name="pCompPos">正向补偿值</param>
        /// <param name="pCompNeg">负向补偿值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetLeadScrewCompPara(HAND axisHandle, Int16 num, Int32 startPos, Int32 cmpLen,Int32[] pCompPos, Int32[] pCompNeg);


        /// <summary>
        /// 使能或禁止螺距位置补偿
        /// </summary>
        /// <param name="axisHandle"></param>
        /// <param name="enable">1：使能，0：禁用</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtEnableLeadScrew(HAND axisHandle, Int16 enable);

        /// <summary>
        /// 设置反向间隙补偿
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="compValue">补偿量</param>
        /// <param name="compDelta">周期补偿量，比如补偿量为100，周期补偿量为10，则反向补偿会在10个规划周期完成100个脉冲的补偿</param>
        /// <param name="compDir">补偿方向,0:正向补偿，1：负向补偿</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16  NMC_MtSetBacklash(HAND axisHandle, Int32 compValue,double compDelta,Int32 compDir);

        /// <summary>
        /// 获取反向间隙补偿
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="pCompValue">补偿量</param>
        /// <param name="pCompDelta">周期补偿量，比如补偿量为100，周期补偿量为10，则反向补偿会在10个规划周期完成100个脉冲的补偿</param>
        /// <param name="pCompDir">补偿方向,0:正向补偿，1：负向补偿</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetBacklash(HAND axisHandle, ref Int32 pCompValue, ref double pCompDelta, ref Int32 pCompDir);

        //----------------------------------------------------------
        //	14.5 控制器资源(参数、版本信息等)
        //----------------------------------------------------------

        /// <summary>
        /// 控制器时间结构
        /// </summary>
        public struct TNMCTime
        {
            public Int16 year;           // 年，真实年份。取值范围[2000,2099]
            public Int16 mon;            // 月,取值范围[1,12]
            public Int16 day;            // 日,取值范围[1,31]
            public Int16 hour;           // 时,取值范围[0,23]
            public Int16 min;            // 分,取值范围[0,59]
            public Int16 second;         // 秒,取值范围[0,59]
        } ;

        /// <summary>
        /// 读时间 注：时间在出厂时根据实际时间设定
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pTime">返回时间结构，参考 TNMCTime 结构定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetTime(HAND devHandle, ref TNMCTime pTime);

        /// <summary>
        /// 写时间 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pTime">时间结构，参考 TNMCTime 结构定义</param>
        /// <param name="pPassword">控制器系统密码，长度最多为16个字节</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetTime(HAND devHandle, ref TNMCTime pTime, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pPassword);

        /// <summary>
        /// 读设备唯一序列号
        /// example:UInt32[] devID = new UInt32[4];NMC_GetUID(g_hDev,devID);
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pData">返回设备唯一序列号,为四个Int32的数据</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetUID(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)]UInt32[] pData);

        /// <summary>
        /// 写用户参数
        /// 注：1)写入的数据掉电不丢失。
        /// 2)一次最多写256字节。参数区总长度为2048字节。
        /// 3)此指令比其它指令操作时间会长,如果出现通讯错误（返回-1），则需要将通过NMC_SetCommPara延长指令通讯时间
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="addr">参数区的偏移地址(字节地址)</param>
        /// <param name="len">写入长度,单位：byte</param>
        /// <param name="pWrData">要写入的数据</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserParaWr(HAND devHandle, UInt32 addr, UInt32 len, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1440)] byte[] pWrData);

        /// <summary>
        /// 读用户参数 注：一次最多读256字节。参数区总长度为2048字节。（参考函数 ： NMC_UserParaWr）
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="addr">参数区的偏移地址(字节地址)</param>
        /// <param name="len">写入长度,单位：byte</param>
        /// <param name="pWrData">要读取的数据存储</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserParaRd(HAND devHandle, UInt32 addr, UInt32 len, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1440)] byte[] pWrData);

        /// <summary>
        /// 设置通讯参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="waitTimeInUs">等待时间，微秒</param>
        /// <param name="retryTimes">通讯重试次数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetCommPara(HAND devHandle, UInt32 waitTimeInUs, UInt32 retryTimes);

        /// <summary>
        /// 修改板卡ID号
        /// </summary> 注：修改ID号完成后，板卡要掉电重启，新的ID才有效。
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="idStr">要写入的板卡ID字符串，最长16字节，以\0结尾</param>
        /// <returns>0:OK,其他见返回值定义</returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevWriteID(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]byte[] idStr);

        /// <summary>
        /// 读取板卡ID号
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="idStr">存储字符串的数组，数组长度大于16字节</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevReadID(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]byte[] idStr);

        /// <summary>
        /// 读取库的版本
        /// </summary>
        /// <param name="pVersion">返回版本信息</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDllVersion([MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] pVersion);

        /// <summary>
        /// 读取当前运动控制器固件的版本等信息
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pVersion">返回版本信息</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetMtLibVersion(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]byte[] pVersion);

        /// <summary>
        /// 读取当前运动控制器资源信息
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pInfo">控制器资源信息</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern short NMC_GetCardInfo(ushort devHandle, ref TDevResourceInfo pInfo);

        /// <summary>
        /// 启动库调试
        /// </summary>
        /// <param name="enable">调试模式：0--关闭调试，默认状态；1--打印到文件；2--输出到GCS</param>
        /// <param name="debugOutputFile">enable为1时表示输出文件名</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetCmdDebug(short enable, string debugOutputFile);

        /// <summary>
        /// 获取错误代码信息
        /// </summary>
        /// <param name="errCode">错误代码</param>
        /// <param name="errDesc">返回的错误代码描述</param>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetErrDesc(short errCode, ref string errDesc);

        /// <summary>
        ///  保存为配置文件
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pFilePath">保存的配置文件路径</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SaveConfigToFile(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 128)]byte[] pFilePath);

        /// <summary>
        /// 掉电保存：备份数据结构体定义
        /// </summary>
        public struct TBackGroup1
        {
            public Int32 crdSegNo;     // 坐标系运动的段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] crdPrfPos;  // 坐标系运动的规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] axPrfPos;  // 单轴规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] axisPos;    // 机械位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] encPos;     // 编码器位置
        };

        /// <summary>
        /// 读取当前备份的变量数值（断电自动保存）
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pBackVar">备份的变量值，参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetBackedVarGroup1(HAND devHandle, ref TBackGroup1 pBackVar);

        public struct TBackGroup2
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] crdSegNo;     // 坐标系运动的段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public Int32[] crdPrfPos;  // 坐标系运动的规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] axPrfPos;  // 规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] axisPos;    // 机械位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] encPos;     // 编码器位置
        };

        /// <summary>
        /// 读取当前备份的变量数值（断电自动保存）
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pBackVar">备份的变量值，参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetBackedVarGroup2(HAND devHandle, ref TBackGroup2 pBackVar);

        /// <summary>
        /// 开始或者关闭变量的自动备份（断电自动保存，默认为关闭状态）
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="en">1：开启变量的自动备份，0：停止变量的自动备份</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetBackedVarOnOff(HAND devHandle, Int16 en);

        /// <summary>
        /// 读取当前自动备份的开启状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pEn">返回当前的开启状态</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetBackedVarOnOff(HAND devHandle, ref Int16 pEn);

        /// <summary>
        /// paraID定义
        /// </summary>
        public const UInt32 PARA_IP_ADDR = 100;    // IP地址，四个字节分别表示四段
        public const UInt32 PARA_IP_MSK = 101;    // IP mask
        public const UInt32 PARA_IP_GW = 102;    // Gateway
        public const UInt32 PARA_IP_DHCP = 103;    // DHCP
        public const UInt32 PARA_WRITE_EN = 999;    // 参数保存

        /// <summary>
        /// 读取系统参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="paraID">系统参数ID，参见定义</param>
        /// <param name="pValue">返回系统参数值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevGetPara(HAND devHandle, UInt32 paraID, out Int32 pValue);

        /// <summary>
        /// 设置系统参数
        /// 注：IP地址等参数写成功后，将在控制器重新启动后生效
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="paraID">系统参数ID，参见定义</param>
        /// <param name="value">系统参数值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevSetPara(HAND devHandle, UInt32 paraID, Int32 value);

        /// <summary>
        ///  设置用户密码
        ///  注意：1.初始出厂无密码，一旦设置密码，则必须要登陆后，控制器某些指令才能正常工作（包括启动运动指令，设置DO输出指令）
        ///  2.修改密码后，请务必记住新的密码。忘记密码只能返厂复位。
        ///   3.密码大小写敏感
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pUserName">目前只支持"USER1"</param>
        /// <param name="pPasswordOld">该用户的当前密码，长度最多为15个字符</param>
        /// <param name="pPasswordNew">新的该用户的密码，长度最多为15个字符</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserSetPassword(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pUserName, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pPasswordOld, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pPasswordNew);

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pUserName">指定的用户名，目前只支持"USER1"</param>
        /// <param name="pPassword">该用户的密码</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserLogin(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pUserName, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pPassword);

        /// <summary>
        /// 用户退出登陆
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pUserName">指定的用户名，目前只支持"USER1"</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserLogout(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pUserName);

        /// <summary>
        /// 设置控制器的规划周期
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="period">单位us	只能为250 500 1000</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetProfilePeriod(HAND devHandle,Int16 period);

        /// <summary>
        /// 获取控制器的规划周期
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pPeriod">单位us	只能为250 500 1000</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetProfilePeriod(HAND devHandle, ref Int16 pPeriod);

        /// <summary>
        /// 读取最后一次的错误代码
        /// </summary>
        /// <returns> 返回值：错误代码</returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetLastErr();

        /// <summary>
        /// 设置指令错误返回值模式
        /// </summary>
        /// <param name="mode">0-标准模式，将返回详细的错误代码；1--简洁模式，只返回错误代码类别</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetErrCodeMode(Int16 mode);

        /// <summary>
        /// 设置指令通讯看门狗
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="timeout">看门狗超时时间，单位毫秒，小于等于0代表关闭看门狗功能</param>
        /// <param name="stopMode">超时停止模式 1:马上停止，0:缓冲区执行完毕后停止</param>
        /// <param name="groupID">超时输出do的组号</param>
        /// <param name="doValue">超时输出do状态</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetWatchDog(HAND devHandle, Int32 timeout, Int16 stopMode, Int16 groupID, Int32 doValue);

        //----------------------------------------------------------
        //	14.6 数据采集
        //----------------------------------------------------------

        /// <summary>
        /// Collect模块：变量类型定义
        /// </summary>
        public const Int32 COLLECT_ADDRESS_PRF_POS = 0;    // 规划位置
        public const Int32 COLLECT_ADDRESS_AXIS_POS = 1;    // 机械位置
        public const Int32 COLLECT_ADDRESS_ENC_POS = 2;    // 编码器位置
        public const Int32 COLLECT_ADDRESS_CMD_POS = 3;    // 命令位置
        public const Int32 COLLECT_ADDRESS_AXIS_VEL = 4;    // 电机速度
        public const Int32 COLLECT_ADDRESS_CRD_POS = 5;    // 坐标系0位置
        public const Int32 COLLECT_ADDRESS_CRD_VEL = 6;    // 坐标系0速度
        public const Int32 COLLECT_ADDRESS_CRD_POS0 = 5;	// 坐标系0位置
        public const Int32 COLLECT_ADDRESS_CRD_VEL0 = 6;    // 坐标系0速度
        public const Int32 COLLECT_ADDRESS_ENC_VEL = 7;    // 编码器速度
        public const Int32 COLLECT_ADDRESS_CMD_VEL = 8;    // 命令速度
        public const Int32 COLLECT_ADDRESS_CRD_POS1 = 9;    // 坐标系1位置		
        public const Int32 COLLECT_ADDRESS_CRD_VEL1 = 10;   // 坐标系1速度
        public const Int32 COLLECT_ADDRESS_LASER_OUTPUT = 11;// 激光输出
        public const Int32 COLLECT_ADDRESS_LASER_GATE = 12;// 激光gate信号状态

        /// <summary>
        /// 获取需要采集数据变量的地址
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="index">变量的序号(从0开始)</param>
        /// <param name="dataType">变量的类型,参数‘Collect模块：变量类型’定义</param>
        /// <param name="pAddr">返回的数据地址</param>
        /// <param name="pDataLen">返回的该数据长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetCollectDataAddr(HAND devHandle, short index, short dataType, out UInt32[] pAddr, out short[] pDataLen);

        /// <summary>
        /// 采集模块配置结构体
        /// </summary>
        public struct TCollectCfg
        {
            public short count;     // 需要采集的变量个数
            public short interval;  // 采集的间隔时间,0表示每隔1毫米采集一次数据，1表示每隔2ms...
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] address;	// 变量的地址
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] length;	// 每个变量的长度
            public TCollectCfg(bool init)
            {
                count = interval = 0;
                address = new UInt32[8];
                length = new short[8];
            }
        }

        /// <summary>
        ///  采集模块：触发模式
        /// </summary>
        public const Int32 COLLECT_MODE_NONE = 0;	// 无条件
        public const Int32 COLLECT_MODE_G_SRC1 = 1;	// 采集源1数值大于比较值
        public const Int32 COLLECT_MODE_L_SRC1 = 2;	// 采集源1数值小于比较值
        public const Int32 COLLECT_MODE_DIFF = 3;	// 采集源1与采集源2两项差值大于比较值
        /// <summary>
        /// 采集模块触发参数结构体
        /// </summary>
        public struct TCollectTrig
        {
            public short mode;         // 触发模式，
            public short source1;      // 触发源1
            public short source2;      // 触发源2
            public short startDelay;   // 触发启动的延时
            public double value;       // 触发比较值
        }

        /// <summary>
        /// 配置采集数据通道,需要配置对应结构体参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pCollect">采集模块配置,参考结构体定义</param>
        /// <param name="pTrig">采集模块触发方式配置,参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ConfigCollect(HAND devHandle, ref TCollectCfg pCollect, ref TCollectTrig pTrig);

        /// <summary>
        /// 启动或停止数据采集
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="en">1启动 0停止</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CollectOnOff(HAND devHandle, short en);

        /// <summary>
        ///  Collect模块：采集状态定义
        /// </summary>
        public const Int32 COLLECT_BUSY = 0x0001;
        public const Int32 COLLECT_OVERRIDE_DATA = 0x0002;
        public const Int32 COLLECT_PUSH_DATA_ERR = 0x0004;

        /// <summary>
        /// 获取采集状态:
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pSts">返回采集状态，按位表示各自状态，参考‘Collect模块：采集状态’宏定义</param>
        /// <param name="pDatalen">采集的数据量</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetCollectSts(HAND devHandle, out short pSts, out Int32 pDatalen);

        /// <summary>
        /// 获取采数据:
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="len">采集数据长度（单位：char,一次最多读1440字节）</param>
        /// <param name="pData">采集的数据（均以char为单元存储）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetCollectData(HAND devHandle, short len, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)]double[] pData);

        /// <summary>
        /// 清除采集状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ClearCollectSts(HAND devHandle);

        //----------------------------------------------------------
        //	14.7 坐标系变换功能
        //----------------------------------------------------------

        /// <summary>
        /// 功能：使能旋转转换处理
        /// 用户数据是按照直角坐标描述，实际加工在一个旋转面上加工，可以用此功能
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="rotAxisNo">旋转轴轴号</param>
        /// <param name="angleRadEqual">旋转轴脉冲转弧度系数</param>
        /// <param name="firstAxisInitPos">旋转中心，X轴的位置</param>
        /// <param name="secAxisInitPos">旋转中心，Y轴的位置</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetTransRotate(ushort crdHandle, short rotAxisNo,
                                double angleRadEqual, Int32 firstAxisInitPos, Int32 secAxisInitPos);

        /// <summary>
        /// 功能：关闭旋转转换处理
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdDelTransRotate(ushort crdHandle);

        /// <summary>
        /// 功能：使能极坐标转换处理
        ///  用户数据是按照直角坐标描述，实际机械机构是一个旋转轴和一个进给轴
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="rotAxNo">旋转轴轴号</param>
        /// <param name="transAxNo">平移轴轴号</param>
        /// <param name="rotEquiv">旋转轴当量</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetTransPolar(ushort crdHandle, short rotAxNo, short transAxNo, double rotEquiv);

        /// <summary>
        /// 功能：运行至设定的极坐标位置并且进行圈数清零处理（利用单轴PTP运行到指定位置）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="xPos">X轴位置 单位：脉冲</param>
        /// <param name="yPos">Y轴位置 单位：脉冲</param>
        /// <param name="rotVel">旋转轴速度 单位：脉冲/毫秒</param>
        /// <param name="transVel">平移轴速度 单位：脉冲/毫秒</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdRunToPolarPos(ushort crdHandle, double xPos, double yPos, double rotVel, double transVel);

        /// <summary>
        /// 功能：运行至设定的极坐标角度位置（利用单轴PTP运行到指定位置）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="theta">旋转轴目标角度</param>
        /// <param name="vel">脉冲旋转速度</param>
        /// <param name="clrRoundFlag">是否清除圈数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdRunToPolarTheta(HAND crdHandle, double theta, double vel, short clrRoundFlag);

        /// <summary>
        /// 功能：销毁极坐标机型（只恢复直角坐标系）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdDelTransPolar(ushort crdHandle);

        /// <summary>
        /// XYZA机型设置接口  
        /// 设置XYZA的映射轴号
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pAxisMapArray">pAxisMapArray[0]~[3]分别对应X、Y、Z和A</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetTransXYZA(HAND crdHandle, short[] pAxisMapArray );

        /// <summary>
        /// 销毁XYZA机型，回归XYZ结构
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdDelTransXYZA(HAND crdHandle);

        /// <summary>
        /// 求工具参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="deltaTheta">从0°往正方向旋转过的角度值</param>
        /// <param name="deltaX">旋转deltaTheta角度后，重新校正到同一点位置后，X轴的相对移动量</param>
        /// <param name="deltaY">旋转deltaTheta角度后，重新校正到同一点位置后，Y轴的相对移动量</param>
        /// <param name="pToolX">工具的X值</param>
        /// <param name="pToolY">工具的Y值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetXYZAToolCalc(HAND crdHandle, double deltaTheta,Int32 deltaX,Int32 deltaY,Int32[] pToolX,Int32[] pToolY );

        /// <summary>
        /// 设置XYZA机型参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pulse2Rad">A轴的每个脉冲对应的弧度值</param>
        /// <param name="toolX">工具的X值</param>
        /// <param name="toolY">工具的Y值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetXYZAPara(HAND crdHandle, double pulse2Rad,Int32 toolX,Int32 toolY );

        /// <summary>
        /// 读取XYZA机型参数
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pPulse2Rad">A轴的每个脉冲对应的弧度值</param>
        /// <param name="pToolX">工具的X值</param>
        /// <param name="pToolY">工具的Y值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetXYZAPara(HAND crdHandle, ref double pPulse2Rad, Int32[] pToolX, Int32[] pToolY);





 //------------------------------------------------------------------------------------------------------------------------------------------------------------------
//	非常用指令或重复指令
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------
        //	控制器的操作
        //----------------------------------------------------------

        /// <summary>
        /// 打开坐标系组
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pCrdHandle">返回坐标系句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdOpen(HAND devHandle, ref UInt16 pCrdHandle);

        /// <summary>
        /// 设置单轴规划高级参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="mapAxisNo">映射轴号，取值范围[0,n]</param>
        /// <param name="port">端口号，取值范围[0,1]，默认为0</param>
        /// <param name="startPos">偏置，默认为0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPrfConfig(HAND axisHandle, Int16 mapAxisNo, Int16 port, Int32 startPos);

        /// <summary>
        /// 读取单轴规划高级参数
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="mapAxisNo">返回映射轴号</param>
        /// <param name="port">返回端口号</param>
        /// <param name="startPos">返回偏置值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfConfig(HAND axisHandle, ref Int16 mapAxisNo, ref Int16 port, ref  Int32 startPos);

        /// <summary>
        /// 打包轴状态结构体（一次可以读取最多4个轴）
        /// </summary>
        public struct TAxisStsPack
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] cmdPos;    // 规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] atlPos;    // 实际位置1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Single[] cmdVel;   // 规划速度1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] motionIO;  // 轴专用IO1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] sts;       // 轴状态1
            public Int32 gpo;        // 通用输出
            public Int32 gpi;        // 通用输入

            public TAxisStsPack(bool bInit)
            {
                cmdPos = new int[4];
                atlPos = new int[4];
                cmdVel = new Single[4];
                motionIO = new int[4];
                sts = new short[4];
                gpo = gpi = 0;
            }
        };

        /// <summary>
        /// 打包读取多个轴的状态，从第一个轴开始读取后续四个轴的状态
        /// </summary>
        /// <param name="axisFirstHandle">第一个轴句柄</param>
        /// <param name="count">读取轴数</param>
        /// <param name="packSts">返回打包的状态数据，参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStsPack(HAND axisFirstHandle, Int16 count, ref TAxisStsPack packSts);

        /// <summary>
        /// 打包轴状态结构体（12个轴）
        /// </summary>
        public struct TAxisStsPack12
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] prfPos;             // 规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] encPos;             // 实际位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Single[] prfVel;             // 规划速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public Int32[] motionIO;             // 轴专用IO
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public short[] sts;             // 轴状态
            public Int32 gpo;        /// 通用输出
            public Int32 gpi;        /// 通用输入 

            public TAxisStsPack12(bool bInit)
            {
                prfPos = new int[12];
                encPos = new int[12];
                prfVel = new Single[12];
                motionIO = new int[12];
                sts = new short[12];
                gpo = gpi = 0;
            }
        }

        /// <summary>
        /// 打包读取12个轴的状态，从第一个轴开始读取
        /// </summary>
        /// <param name="axisFirstHandle">第一个轴句柄</param>
        /// <param name="packSts">打包的12轴状态数据，参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStsPack12(HAND axisFirstHandle,  ref TAxisStsPack12 packSts);

        /// <summary>
        /// 打包轴状态结构体（8轴）
        /// </summary>
        public struct TAxisStsPack8
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] prfPos;             // 规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] encPos;             // 实际位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Single[] prfVel;             // 规划速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] motionIO;             // 轴专用IO
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] sts;             // 轴状态
            public Int32 gpo;        /// 通用输出
            public Int32 gpi;        /// 通用输入 

            public TAxisStsPack8(bool bInit)
            {
                prfPos = new int[8];
                encPos = new int[8];
                prfVel = new Single[8];
                motionIO = new int[8];
                sts = new short[8];
                gpo = gpi = 0;
            }
        }

        /// <summary>
        /// 打包读取8个轴的状态,从第一个轴开始读取
        /// </summary>
        /// <param name="axisFirstHandle">第一个轴句柄</param>
        /// <param name="pPackSts">打包的状态数据,参考结构体定义</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStsPack8(HAND axisFirstHandle, ref TAxisStsPack8 pPackSts);

        /// <summary>
        /// 设置限位开关输入是否停止运动
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posSwt">正向限位触发允许设置，1为允许，0为禁止</param>
        /// <param name="negSwt">负向限位触发允许设置，1为允许，0为禁止</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtLmtOnOff(HAND axisHandle, Int16 posSwt, Int16 negSwt);

        /// <summary>
        /// 设置限位开关触发电平
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="posSwt">正向限位触发电平设置，1为高电平触发，0为低电平触发</param>
        /// <param name="negSwt">负向限位触发电平设置，1为高电平触发，0为低电平触发</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtLmtSns(HAND axisHandle, Int16 posSwt, Int16 negSwt);

        /// <summary>
        /// 设置原点开关触发电平，注意：这里的电平设置不会影响捕获的电平
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="sns">原点电平设置,1为高电平触发,0为低电平触发</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtHomeSns(HAND axisHandle, Int16 sns);

        /// <summary>
        /// 读取原点开关触发电平
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="sns">原点电平设置,1为高电平触发,0为低电平触发</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetHomeSns(HAND axisHandle, ref Int16 sns);

        /// <summary>
        /// 设置伺服报警开关是否停止运动
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="swt">伺服报警开关输入允许设置，1为允许，0为禁止。（默认为高电平触发）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtAlarmOnOff(HAND axisHandle, Int16 swt);

        /// <summary>
        /// 设置伺服报警开关电平
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="swt">伺服报警触发电平设置，1为高电平触发，0为低电平触发</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtAlarmSns(HAND axisHandle, Int16 swt);

        //----------------------------------------------------------
        //	通用IO及外部资源读写
        //----------------------------------------------------------
        /// <summary>
        /// 设置通用输出(按通道,支持超过32位)
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupID">DO组，取值范围[0,n],具体需要看控制器是否存在多组数字量输出</param>
        /// <param name="value">设置32路通用数字量输出。对应bit位 为1, 输出高电平，0，输出低电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDO(HAND devHandle, Int16 groupID, Int32 value);

        /// <summary>
        /// 读取通用输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupID">DO组，取值范围[0,n],具体需要看控制器是否存在多组数字量输出</param>
        /// <param name="value">返回32路通用数字量输出。对应bit位 1, 高电平，0，低电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDO(HAND devHandle, Int16 groupID, ref  Int32 value);

        /// <summary>
        /// 读通用输入 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupID">DI组，取值范围[0,n],具体需要看控制器是否存在多组数字量输入</param>
        /// <param name="inValue">返回32路通用数字量输入 对应bit位 1, 高电平，0，低电平</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDI(HAND devHandle, Int16 groupID, ref  Int32 inValue);

        /// <summary>
        ///  使能扩展IO模块
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="chDevId">扩展IO模块ID，取值范围2~n</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetIOModuleEn(HAND devHandle, byte chDevId);

        //----------------------------------------------------------
        //	缓冲区配置和管理
        //----------------------------------------------------------

        /// <summary>
        /// 设置坐标系运动缓冲区是否启用，默认为启用状态
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="enFlag">1：启用，0：不启用</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_EnableCrdSdram(HAND crdHandle, Int16 enFlag);



 

        /// <summary>
        /// 坐标系状态打包结构
        /// </summary>
        public struct TPackedCrdSts3
        {
            public Int16 crdSts;          // 坐标系状态
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int16[] axSts;         // 坐标系里各轴状态    
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] prfPos;        // 用户坐标系下的规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] axisPos;       // 机械坐标系下的规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] encPos;        // 编码器位置
            public Int32 userSeg;         // 运行的缓冲区段号
            public double prfVel;         // 运动速度
            public Int32 gpDi;             // 通用输入0~31
            public Int32 gpDo;             // 通用输出0~31
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int16[] motDi;         // 限位、原点、报警。请参考专用IO位定义( 搜索 BIT_AXMTIO_LMTN )
            public Int16 reserved;        // 保留
            public Int32 crdFreeSpace;    // 缓冲区剩余空间
            public Int32 reserved2;
            public TPackedCrdSts3(bool init)
            {
                crdSts = 0;
                axSts = new Int16[3];
                prfPos = new Int32[3];
                axisPos = new Int32[3];
                encPos = new Int32[3];
                userSeg = 0;
                prfVel = 0;
                gpDi = 0;
                gpDo = 0;
                motDi = new Int16[3];
                reserved = 0;
                crdFreeSpace = 0;
                reserved2 = 0;
            }
        };

        /// <summary>
        /// 坐标系运动模式下，打包读取控制器状态
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="cnts">读取个数，1~N</param>
        /// <param name="pPackSts">返回坐标系状态打包结构</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetStsPack3(HAND crdHandle, Int16 cnts, ref  TPackedCrdSts3 pPackSts);

        /// <summary>
        /// 直接修改坐标系偏移
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pOffsetArray">坐标系偏移</param>
        /// <param name="cnt">数组长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdModifyOffset( HAND crdHandle, Int32[] pOffsetArray,  Int16 cnt);

        /// <summary>
        /// 功能：缓冲区探位置停止并后续加位置偏置
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axis">轴号</param>
        /// <param name="gpiIndex">通用输入序号</param>
        /// <param name="sense">触发电平,0：低电平,1：高电平</param>
        /// <param name="useCaptPos">是否使用捕获位置  1使用,0不使用</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSetStopOffset(HAND crdHandle, Int32 segNo, Int16 axis, Int16 gpiIndex, Int16 sense, Int16 useCaptPos);

        /// <summary>
        /// 功能：解除缓冲区探位置停止并后续加位置偏置
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufClrStopOffset(HAND crdHandle, Int32 segNo, Int16 axis);

        /// <summary>
        /// 读取插补线段长度
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="pLen">插补线段长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetBufLength(HAND crdHandle, ref double pLen);

        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetIsShortLine(HAND crdHandle, short isShortLine);

        //----------------------------------------------------------
        // 缓冲区指令
        //----------------------------------------------------------
  
        /// <summary>
        /// 直线插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="mask">参与的轴，按位表示</param>
        /// <param name="pTgPos">目标位置</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZ(HAND crdHandle, Int32 segNo, Int16 mask, Int32[] pTgPos, double endVel, double vel, double synAcc);

        /// <summary>
        /// 4轴插补打包单元结构体
        /// </summary>
        public struct TCrdLineXYZD4Unit
        {
            public Int32 segNo;         // 用户自定义段号
            public Int16 crdAxMask;
            public Int16 extAxMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] 
            public Int32[] tgPos;      // 目标位置
            public Single vel;         // 最大速度
            public Single endVel;      // 终点速度
            public Single acc;      // 插补加速度
            public Single lookaheadDis;	// 是否使用前瞻
            public Single reserved;		// 保留
            public TCrdLineXYZD4Unit(bool init)
            {
                segNo = crdAxMask = extAxMask = 0;
                tgPos = new Int32[4];
                vel = endVel = acc = lookaheadDis = reserved = 0;
            }
        };

        /// <summary>
        ///  打包的4轴直线插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="count">打包指令数，取值范围[1,25]</param>
        /// <param name="pCmdArray">4轴插补打包单元结构体数据</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZD4Pack(  HAND crdHandle, Int16 count, ref TCrdLineXYZD4Unit pCmdArray);

        /// <summary>
        /// 轴插补打包单元结构体
        /// </summary>
        public struct TCrdLineXYZUnit
        {
            public Int32 segNo;         // 用户自定义段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] tgPos;      // 目标位置
            public double endVel;      // 终点速度
            public double vel;         // 最大速度
            public double synAcc;      // 插补加速度
            public Int16 mask;         // 参与的轴，按位表示
            public Int16 lookaheadDis;	// 是否使用前瞻
            public Int16 reserved1;
            public Int16 reserved2;     // 保留
            public TCrdLineXYZUnit(bool init)
            {
                segNo = 0;
                tgPos = new Int32[3];
                endVel = vel = synAcc = mask = lookaheadDis = reserved1 = reserved2 = 0;
            }
        };

        /// <summary>
        /// 打包的3轴直线插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="count">打包指令数，取值范围[1,20]</param>
        /// <param name="pCmdArray">3轴插补打包单元结构体数据</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZPack(  HAND crdHandle, Int16 count, ref TCrdLineXYZUnit pCmdArray);

        /// <summary>
        ///  XY平面圆弧插补：终点位置、半径、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPos">目标位置（二维数组，分别表示XY的位置）</param>
        /// <param name="radius">圆弧半径，大于0表示劣弧，小于0表示优弧</param>
        /// <param name="circleDir">圆弧方向,0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcRadius(HAND crdHandle, Int32 segNo, Int32[] pTgPos, double radius, Int16 circleDir, double endVel, double vel, double synAcc);

         /// <summary>
        ///  YZ平面圆弧插补：终点位置、半径、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPos">目标位置（二维数组，分别表示YZ的位置）</param>
        /// <param name="radius">圆弧半径，大于0表示劣弧，小于0表示优弧</param>
        /// <param name="circleDir">圆弧方向,0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcRadiusYZ(HAND crdHandle, Int32 segNo, Int32[] pTgPos, double radius, Int16 circleDir, double endVel, double vel, double synAcc);

        /// <summary>
        ///  ZX平面圆弧插补：终点位置、半径、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPos">目标位置（二维数组，分别表示ZX的位置）</param>
        /// <param name="radius">圆弧半径，大于0表示劣弧，小于0表示优弧</param>
        /// <param name="circleDir">圆弧方向,0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcRadiusZX(HAND crdHandle, Int32 segNo, Int32[] pTgPos, double radius, Int16 circleDir, double endVel, double vel, double synAcc);

        /// <summary>
        /// XY平面圆弧插补：终点位置、圆心、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（二维数组，分别表示XY轴的目标位置）</param>
        /// <param name="pCenterPosArray">圆心坐标（二维数组，分别表示XY轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcCenter(HAND crdHandle, Int32 segNo, Int32[] pTgPos, Int32[] pCenterPos, Int16 circleDir, double endVel, double vel, double synAcc);

        /// <summary>
        /// YZ平面圆弧插补：终点位置、圆心、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（二维数组，分别表示YZ轴的目标位置）</param>
        /// <param name="pCenterPosArray">圆心坐标（二维数组，分别表示YZ轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcCenterYZ(HAND crdHandle, Int32 segNo, Int32[] pTgPos, Int32[] pCenterPos, Int16 circleDir, double endVel, double vel, double synAcc); 

        /// <summary>
        /// ZX平面圆弧插补：终点位置、圆心、方向
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（二维数组，分别表示ZX轴的目标位置）</param>
        /// <param name="pCenterPosArray">圆心坐标（二维数组，分别表示ZX轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcCenterZX(HAND crdHandle, Int32 segNo, Int32[] pTgPos, Int32[] pCenterPos, Int16 circleDir, double endVel, double vel, double synAcc);

        /// <summary>
        /// 圆弧插补：起点、中点、终点
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pMidPosArray">中间位置点坐标（二维数组，分别表示中间点的XY轴的位置）</param>
        /// <param name="pTgPosArray">终点位置坐标（二维数组，分别表示终点的XY轴的位置）</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcPPP(HAND crdHandle, Int32 segNo, Int32[] pMidPos, Int32[] pTgPos, double endVel, double vel, double synAcc);

        /// <summary>
        /// 螺旋线插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pTgPosArray">目标位置（三维数组，分别表示终点的XYZ轴的位置）</param>
        /// <param name="pCenterPosArray">圆心位置（二维数组，分别表示XY轴相对于起点的圆心位置），注意：圆心坐标为相对于起点的相对位置</param>
        /// <param name="circleDir">圆弧方向，0表示顺时针方向，1表示逆时针方向</param>
        /// <param name="rounds">方向圈数</param>
        /// <param name="endVel">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdHelixCenter(HAND crdHandle, Int32 segNo, Int32[] pTgPos, Int32[] pCenterPos, Int16 circleDir, double rounds, double endVel, double vel, double synAcc);

        /// <summary>
        /// 3D圆弧插补
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="pMidPos">中点位置（三维数组，分别表示中点的XYZ轴的位置）</param>
        /// <param name="pTgPos">终点位置（三维数组，分别表示终点的XYZ轴的位置）</param>
        /// <param name="velEnd">终点速度</param>
        /// <param name="vel">最大速度</param>
        /// <param name="synAcc">合成加速度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArc3D(  HAND crdHandle,Int32 segNo, Int32[] pMidPos, Int32 pTgPos,double velEnd, double vel,double synAcc );

        /// <summary>
        /// 缓冲区单轴移动(绝对位移移动)
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="axMask">参与的轴</param>
        /// <param name="pTgPos">目标位置</param>
        /// <param name="blockEn">是否为阻塞模式</param>
        /// <param name="synEn">是否为同步模式</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufAxMove(HAND crdHandle, Int32 segNo, Int16 axMask, Int32[] pTgPos, Int16 blockEn, Int16 synEn);

        /// <summary>
        /// 缓冲区跟随开关
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo">段号</param>
        /// <param name="onOff">开关</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufValueFollow(HAND crdHandle, Int32 segNo, Int16 onOff);


        //----------------------------------------------------------
        //	扩展资源
        //----------------------------------------------------------
                                               
        /// <summary>
        /// 通用可寻址变量ID
        /// </summary>
        public const Int32 VAR_PWM0_CTL   = 256;	// pwm通道0打开关闭，1表示打开，0表示关闭
        public const Int32 VAR_PWM0_VALUE = 257;	// pwm通道0输出值
        public const Int32 VAR_PWM1_CTL   = 258;	// pwm通道1打开关闭，1表示打开，0表示关闭
        public const Int32 VAR_PWM1_VALUE = 259;	// pwm通道1输出值
        public const Int32 VAR_EXT_DAC0   = 260;	// 扩展的DAC通道0
        public const Int32 VAR_EXT_DAC1   = 261;	// 扩展的DAC通道1
        public const Int32 VAR_OUT_OPTION = 262;	// PWM输出通道选项
        public const Int32 VAR_DAC0       = 263;	// DAC通道0~7
        public const Int32 VAR_DAC1       = 264;
        public const Int32 VAR_DAC2       = 265;
        public const Int32 VAR_DAC3       = 266;
        public const Int32 VAR_DAC4       = 267;
        public const Int32 VAR_DAC5       = 268;
        public const Int32 VAR_DAC6       = 269;
        public const Int32 VAR_DAC7       = 270;
        public const Int32 VAR_ADC0       = 271;	// ADC通道0~7
        public const Int32 VAR_ADC1       = 272;
        public const Int32 VAR_ADC2       = 273;
        public const Int32 VAR_ADC3       = 274;
        public const Int32 VAR_ADC4       = 275;
        public const Int32 VAR_ADC5       = 276;
        public const Int32 VAR_ADC6       = 277;
        public const Int32 VAR_ADC7       = 278;

        /// <summary>
        /// 设置通用可寻址变量
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="varID">变量ID</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetVar8B(HAND devHandle, Int32 varID, Int64 value);

        /// <summary>
        /// 读取通用可寻址变量
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="varID">变量ID</param>
        /// <param name="pValue">返回变量当前值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetVar8B(HAND devHandle, Int32 varID, ref  Int64 pValue);

        /// <summary>
        /// 通用可寻址系统变量ID
        /// </summary>
        public const Int32 SYS_VAR_SET_STAT_ENABLE   = 100;
        public const Int32 SYS_VAR_GET_CLOCK         = 101;
        public const Int32 SYS_VAR_GET_USERAPP_COUNT = 107;
        public const Int32 SYS_VAR_GET_USERAPP_MIN   = 108;
        public const Int32 SYS_VAR_GET_USERAPP_MAX   = 109;
        public const Int32 SYS_VAR_GET_USERAPP_AVG   = 110;
        public const Int32 SYS_VAR_GET_USERAPP_CURT  = 111;
        public const Int32 SYS_VAR_GET_PRFINT_COUN   = 112;
        public const Int32 SYS_VAR_GET_PRFINT_MIN    = 113;
        public const Int32 SYS_VAR_GET_PRFINT_MAX    = 114;
        public const Int32 SYS_VAR_GET_PRFINT_AVG    = 115;
        public const Int32 SYS_VAR_GET_PRFINT_CURT   = 116;
        public const Int32 SYS_VAR_GET_MAINLP_COUNT  = 117;
        public const Int32 SYS_VAR_GET_MAINLP_MIN    = 118;
        public const Int32 SYS_VAR_GET_MAINLP_MAX    = 119;
        public const Int32 SYS_VAR_GET_MAINLP_AVG    = 120;
        public const Int32 SYS_VAR_GET_MAINLP_CURT   = 121;

        /// <summary>
        /// 设置通用可寻址系统变量
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="varID">变量ID</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SysSetVar8B(HAND devHandle, Int32 varID, Int64 value);

        /// <summary>
        /// 读取通用可寻址系统变量 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="varID">变量ID</param>
        /// <param name="pValue">返回变量当前值</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SysGetVar8B(HAND devHandle, Int32 varID, ref  Int64 pValue);

        /// <summary>
        /// 向备份内存写数据(总长度约1500byte) 注：1)写入的数据掉电不丢失。2)一次最多写1440字节。总长度约1500byte
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="src"> 要写入的数据</param>
        /// <param name="len">要写入的长度，单位：byte</param>
        /// <param name="off">要写入的地址(偏移量)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BackSramWrite(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1440)] byte[] src, Int32 len, Int32 off);

        /// <summary>
        /// 从备份内存读数据
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="dest">读出的数据暂存区</param>
        /// <param name="len">要读出的长度，单位：byte,一次最多读1440字节。总长度约1500byte</param>
        /// <param name="off">读数据的地址(偏移量)</param>
        /// <returns></returns
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BackSramRead(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1440)] byte[] dest, Int32 len, Int32 off);
       
        /// <summary>
        /// 修改控制器系统密码，密码用于修改系统时钟等
        /// 注意：1.初始出厂无密码
        /// 2.修改密码后，请务必记住新的密码。忘记密码只能返厂复位。
        /// 3.密码大小写敏感
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pPasswordOld">控制器系统当前密码，长度最多为15个字符</param>
        /// <param name="pPasswordNew">新的控制器系统密码，长度最多为15个字符</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ChangePassword(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pPasswordOld,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pPasswordNew);

        /// <summary>
        /// 验证系统密码
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pPassword">控制器系统当前密码，长度最多为15个字符</param>
        /// <returns>返回0表示验证通过，其他表示错误</returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_VerifyPassword(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] byte[] pPassword);

        /// <summary>
        /// 清除系统密码及用户密码
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pData">解密数据，长度为8</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ClrPassword(HAND devHandle, ref  short pData);


        //----------------------------------------------------------
        //	激光
        //----------------------------------------------------------

         /// <summary>
         /// 激光物理信号类型
         /// </summary>
        public const Int32 LASER_NONE     = (0);   // 关闭激光输出模式
        public const Int32 LASER_DA     = (1);   // DA输出
        public const Int32 LASER_PWM_DUTY     = (2);   // 占空比输出
        public const Int32 LASER_PWM_FRQ     = (3);   // 频率输出
        public const Int32 LASER_PWM_FRQ_EXT	=(4);		// 频率输出,脉宽固定

        /// <summary>
        /// 激光物理信号输出类型配置
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="outputType">输出的类型</param>
        /// <param name="index">输出通道序号,取值范围[0,n]</param>
        /// <param name="optionVal">当作为占空比输出时，该值为PWM的频率，单位HZ；当为频率输出时，该值作为占空比值，（0~100）</param>
        /// <param name="ch">激光控制通道，取值范围[0,n]</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetOutputType(HAND devHandle, Int16 outputType,Int16 index, Int32 optionVal,Int16 ch);
 
        /// <summary>
        /// 功能：设置立即输出激光的能量 
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="outVal">激光能量（设定值意义与NMC_LaserSetOutputType设置的输出类型对应）</param>
        /// <param name="ch">激光控制通道</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserSetPower(HAND devHandle, Int32 outVal,Int16 ch);

        //----------------------------------------------------------
        //	系统操作
        //----------------------------------------------------------

        /// <summary>
        /// 用户指令传输，只写
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="sendBuffer">写入数据</param>
        /// <param name="sendLen">写入长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserCmdWrite(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] sendBuffer, UInt32 sendLen);

        /// <summary>
        /// 用户指令传输，只读
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="sendBuffer">写入数据</param>
        /// <param name="sendLen">写入长度</param>
        /// <param name="recBuffer">读出数据</param>
        /// <param name="waitLen">读出长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserCmdRead(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] sendBuffer, UInt32 sendLen,
                    [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] recBuffer, UInt32 waitLen);

        /// <summary>
        /// 批量数据传输，只写
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="sendBuffer">传输数据</param>
        /// <param name="sendLen">传输长度</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DataTransfer(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] sendBuffer, UInt32 sendLen);

        /// <summary>
        /// 禁止用户程序运行
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="mode">0,关闭用户程序，1，运行用户程序</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SysSetUserApp( HAND devHandle,  byte mode);

        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SysUpgrade(HAND devHandle);

        /// <summary>
        /// 升级固件
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SysUpgradeEx(HAND devHandle, byte cmd, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] sendBuffer, UInt32 sendLen, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] recBuffer, UInt32 recvedLen);

        /// <summary>
        /// 设置调试信息
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="debugErrEn"></param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDebugErrorEn(HAND devHandle, Int16 debugErrEn);

        //----------------------------------------------------------
        //	二维位置点比较
        //----------------------------------------------------------


        /// <summary>
        /// 二维位置点比较结构体
        /// </summary>
        public struct TComp2DimensParam
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
	        public Int16 [] outputchn;      // 比较输出的通道:-1表示不输出处理
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
	        public Int16 [] outputType;      // 输出方式0：脉冲1：电平	
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
	        public Int16 [] chnType;      // 通道类型：0 GPO， 1  GATE通道
	        public Int16 dir1No; 								// 方向1 的位置源轴号（0~11）
	        public Int16 dir2No; 								// 方向2 的位置源轴号（0~11）
	        public Int16 posSrc; 								// 轴位置类型 ：0规划1：编码器
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
	        public Int16 [] stLevel;      // 电平模式下的起始电平（0或1)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CMP_OUTPUT_CHN_MAX)]
	        public Int16 [] gateTime;      // 脉冲方式脉冲时间:单位ms
	        public Int16 errZone; 			// 进入比较点容差半径范围（pulse）
            public TComp2DimensParam(bool init)
            {
                outputchn = new Int16[CMP_OUTPUT_CHN_MAX];
                outputType = new Int16[CMP_OUTPUT_CHN_MAX];
                chnType = new Int16[CMP_OUTPUT_CHN_MAX];
                dir1No = dir2No = posSrc = 0;
                stLevel = new Int16[CMP_OUTPUT_CHN_MAX];
                gateTime = new Int16[CMP_OUTPUT_CHN_MAX];
                errZone = 0;
            }
        } ;

        /// <summary>
        /// 功能：设置2维位置比较的参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="group">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <param name="param">二维比较参数</param>
        /// <param name="chn">保留，设为0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_Comp2DimensSetParam(HAND devHandle,Int16 group, ref TComp2DimensParam param,Int16 chn);

        /// <summary>
        /// 功能：获取2维位置比较的参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="group">返回 位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <param name="param">返回 二维比较参数</param>
        /// <param name="chn">保留，设为0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_Comp2DimensGetParam(HAND devHandle,Int16 group,  ref TComp2DimensParam param,Int16 chn);

        /// <summary>
        /// 设置二维比较数据
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="group"></param>
        /// <param name="pArrayPos">比较数组地址</param>
        /// <param name="count">位置点数</param>
        /// <param name="chn">保留，设为0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_Comp2DimensSetData(HAND devHandle,Int16 group,Int32[] pArrayPos,Int16 count,Int16 chn);


        /// <summary>
        /// 功能：2维位置比较使能
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="group">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <param name="onOff">0 停止，1输出</param>
        /// <param name="chn">保留，设为0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_Comp2DimensOnoff(HAND devHandle,Int16 group,Int16 onOff,Int16 chn);

        /// <summary>
        /// 功能：获取2维位置比较的输出状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="group">位置比较通道号 取值[0,CMP_OUTPUT_CHN_MAX-1]</param>
        /// <param name="pStatus">返回比较状态 0 未启动比较 1 比较输出中</param>
        /// <param name="pOutCount">已经比较输出的个数</param>
        /// <param name="chn">保留，设为0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_Comp2DimensStatus(HAND devHandle,Int16 group, ref Int16 pStatus, ref Int16 pOutCount,Int16 chn);

        //----------------------------------------------------------
        //	结构体初始化函数
        //----------------------------------------------------------

        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TSafeParaStructInit(HAND devHandle, ref TSafePara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TCrdConfigStructInit(HAND devHandle, ref TCrdConfig pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TCrdSafeParaStructInit(HAND devHandle, ref TCrdSafePara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_THomeSettingStructInit(HAND devHandle, ref THomeSetting pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TArcSecSettingStructInit(HAND devHandle, ref TArcSecSetting pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TCrdParaStructInit(HAND devHandle, ref TCrdPara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TCollectCfgStructInit(HAND devHandle, ref TCollectCfg pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TCollectTrigStructInit(HAND devHandle, ref TCollectTrig pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TTimeArrayParaStructInit(HAND devHandle, ref TTimeArrayPara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TLaserPowerStructInit(HAND devHandle, ref TLaserPower pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TSHIOParaStructInit(HAND devHandle, ref TSHIOPara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TLinearCmpParaStructInit(HAND devHandle, ref TLinearCmpPara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TComp2DimensParamStructInit(HAND devHandle, ref TComp2DimensParam pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TPtpParaStructInit(HAND devHandle, ref TPtpPara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TJogParaStructInit(HAND devHandle, ref TJogPara pPara);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_TNMCTimeStructInit(HAND devHandle, ref TNMCTime pPara);

        /// <summary>
        /// 清除轴错误状态,同时多个
        /// </summary>
        /// <param name="axisHandle">轴句柄</param>
        /// <param name="count">轴个数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClrErrorEx(HAND axisHandle, short count);

        /// <summary>
        /// 输出DO,16位
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="value">0：低电平，1：高电平</param>
        /// <param name="groupID">组号</param>
        /// <param name="bitsFlag">bitsFlag,0:低16位,1：高16位</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOGroupEx(HAND devHandle, Int32 value, short groupID, short bitsFlag);


        //----------------------------------------------------------
        //	线性距离位置比较
        //----------------------------------------------------------

        // 精确输出结构体
        public struct TLinearCmpPara
        {
	        public Int16 dimens;
	        public Int16 compMode;
	        public Int16 axMask;
	        public Int16 src;
	        public double gateTime;
        };
        /// <summary>
        /// 功能：设置精确位置比较的参数，并清除线性位置比较位置数据
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="dimens">比较的维数 1或者2</param>
        /// <param name="axisMask">选比较的轴号,按bit选择:不能超过2个bit同时为1</param>
        /// <param name="src">比较源：  0：规划位置 1:编码器位置</param>
        /// <param name="cmpMode">比较的模式：0 按输出数组位置比较,1 线性比较,按interval间距比较</param>
        /// <param name="gateTime">输出的脉冲宽度,单位：s,取值范围值(0,0.0009]</param>
        /// <param name="chn">输出通道 目前只支持0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LinearCompSetParam(HAND devHandle, Int16 dimens, Int16 axisMask, Int16 src, Int16 cmpMode, double gateTime, Int16 chn);

        /// <summary>
        /// 功能：获取配置参数
        /// </summary>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LinearCompGetParam(HAND devHandle, ref TLinearCmpPara pLinearCmpPara, Int16 chn);

        /// <summary>
        /// 功能：设置精确比较位置数据
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pStartPos">起始比较位置，注意：暂时没用</param>
        /// <param name="pPos">比较的位置数组地址。注意：这里为相对距离，第一点为相对于NMC_LinearCompOnOff调用时的位置的距离，第二点为相对第一点的距离（小于32767）</param>
        /// <param name="count">比较的位置个数，最大不能超过250，如果比较数据过多，可以通过多次调用的方式下压数据，缓冲区长度</param>
        /// <param name="chn">输出通道 目前只支持0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LinearCompSetData(HAND devHandle, Int32[] pStartPos, Int32[] pPos, Int32 count, Int16 chn);           

        /// <summary>
        /// 功能：线性比较输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pStartPos">起始比较位置</param>
        /// <param name="interval">线性比较间距（小于32767）</param>
        /// <param name="count">线性比较次数</param>
        /// <param name="chn">输出通道 目前只支持0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LinearCompInterval(HAND devHandle, Int32[] pStartPos, Int32 interval, Int32 count, Int16 chn);

        /// <summary>
        /// 功能：精确位置比较使能
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="onOff"> 0 停止,1输出</param>
        /// <param name="chn">输出通道 目前只支持0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LinearCompOnOff(HAND devHandle, Int16 onOff, Int16 chn);

        /// <summary>
        /// 功能：比较的输出状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pStatus">  0 未启动比较 1 比较输出中</param>
        /// <param name="pOutCount"> 输出的个数,对于数组输出模式，输出计数计算请咨询我们</param>
        /// <param name="chn">输出通道 目前只支持0</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LinearCompStatus(HAND devHandle, ref Int16 pStatus, ref Int16 pOutCount, Int16 chn);
        //----------------------------------------------------------
        //	高级BufIo输出配置及控制
        //----------------------------------------------------------
        // BufIo输出最大组数
        public const Int16 MAX_ADV_BUFIO_GROUP = 2;
        // 高级BufIo输出参数
        public struct TAdvBufIoParam
        {
	        // 输出点
	        public short outType;				// 输出类型，0：通用输出；1：Gate信号；其他保留
	        public short outGroup;				// 输出组，取值范围[0,n]
	        public short outIndex;				// 输出序号，取值范围[0,n]
	        public short outSns;				// 有效电平,0:低电平有效，1：高电平
	
	        // 信号
	        public short pulseMode;		    // 0：电平输出，1：脉冲输出W
	        public short  pulseOnTime;		// 有效电平时间（脉冲输出方式下有效），单位：微秒
	        public Int32  pulseOffTime;		// 无效电平时间（脉冲输出方式下有效），单位：微秒

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] reserved;             	// 保留
        };
        /// <summary>
        /// 设置BufIo输出参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pPrm"> 具体查看参数结构体</param>
        /// <param name="ch"> 输出组，取值范围：[0,MAX_ADV_BUFIO_GROUP)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_AdvBufIoSetParam(HAND devHandle,  ref TAdvBufIoParam pPrm, short ch);

        /// <summary>
        /// 读取BufIo输出参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pPrm"> 具体查看参数结构体</param>
        /// <param name="ch"> 输出组，取值范围：[0,MAX_ADV_BUFIO_GROUP)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_AdvBufIoGetParam(HAND devHandle, ref TAdvBufIoParam pPrm, short ch);

        /// <summary>
        /// 缓冲区设置高级BufIo输出有效（运动一段距离后输出特定状态）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo"> 段号</param>
        /// <param name="outLength"> 距离，单位：脉冲</param>
        /// <param name="value"> 脉冲模式下输出脉冲个数，电平方式下无意义</param>
        /// <param name="ch"> 输出组，取值范围：[0,MAX_ADV_BUFIO_GROUP)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdAdvBufIoOnAfterLen(HAND crdHandle, Int32 segNo, Int32 outLength, Int32 value, short ch);

        /// <summary>
        /// 缓冲区设置高级BufIo关闭输出（缓冲区全部运动结束前提前一段距离输出特定状态）
        /// </summary>
        /// <param name="crdHandle">坐标系句柄</param>
        /// <param name="segNo"> 段号</param>
        /// <param name="outLength"> 距离，单位：脉冲</param>
        /// <param name="ch"> 输出组，取值范围：[0,MAX_ADV_BUFIO_GROUP)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdAdvBufIoOffBeforeLen(HAND crdHandle, Int32 segNo, Int32 outLength, short ch);

        /// <summary>
        /// 立即设置高级BufIo输出
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="value"> 脉冲模式下输出脉冲个数，电平方式下0:关闭，1:打开。</param>
        /// <param name="ch"> 输出组，取值范围：[0,MAX_ADV_BUFIO_GROUP)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_AdvBufIoOut(HAND devHandle, long value, short ch);

        /// <summary>
        /// 读取BufIo的输出数量
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="pOutCout"> 输出脉冲个数</param>
        /// <param name="ch"> 输出组，取值范围：[0,MAX_ADV_BUFIO_GROUP)</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_AdvBufIoGetPulseCnt(HAND devHandle, ref Int32 pOutCout, short ch);

        //-------------------------------------------------------------------------------
        // 坐标系干涉保护指令
        //-------------------------------------------------------------------------------
        public const Int16 CRD_INTERF_GROUP_MAX	=	(2);
        /// <summary>
        /// 启动坐标系的干涉保护
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupNo"> 干涉坐标系组号（目前只有两个坐标系，所以该值为0）</param>
        /// <param name="mode"> 0:基本模式，出现干涉，两坐标系均停止  1：激光模式，出现干涉，其中一个停止并离开</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetMvProtect(HAND devHandle,short groupNo,short mode);

        /// <summary>
        /// 启动坐标系的干涉保护
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupNo"> 干涉坐标系组号（目前只有两个坐标系，所以该值为0）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DelMvProtect(HAND devHandle,short groupNo);

        /// <summary>
        /// 获取坐标系的干涉状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupNo"> 干涉坐标系组号（目前只有两个坐标系，所以该值为0）</param>
        /// <param name="pSts"> 0 没有启动干涉功能 1  没有出现干涉  2 干涉发生</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetMvProtectSts(HAND devHandle,short groupNo,ref short pSts);

        public struct TMvProtectPara
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public Int16 [] crdNo;		// 需要干涉保护的两坐标系号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public Int16 [] crdAxNo;	// 坐标系的干涉轴号,指XYZ中哪一个（X:0  Y:1    Z:2）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public Int16 [] moveDir;    // 两干涉轴分别往距离减小时的运动方向，1：正向 -1：负向

	        public long orgDis;			// 两坐标系的crdAxNo轴在原点时的距离
	        public long mvProtectDis;	 // 干涉的保护距离   
	        public long safeWaitPos;	// mode为激光模式时，干涉轴需要停止到的等待位置（mode为0时无效）
	        public double mvToSafeAcc;  // 运动到等待位置的移动加速度
	        public double mvToSafeVel;  // 运动到等待位置的移动速度
            public short stopCrdNo;		// mode为激光模式时，需要停止的坐标系号（mode为0时无效）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public Int16 [] reserved;      // 保留参数
            public TMvProtectPara(bool init)
            {
                crdNo = new Int16[2];
                crdAxNo = new Int16[2];
                moveDir = new Int16[2];
                orgDis = mvProtectDis = safeWaitPos = 0;
                mvToSafeAcc = mvToSafeVel = 0;
                stopCrdNo = 0;
                reserved = new Int16[3];
            }
   
        };
        /// <summary>
        /// 设置坐标系的干涉保护参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupNo"> 干涉坐标系组号（目前只有两个坐标系，所以该值为0）</param>
        /// <param name="pCrdInterfProtectPara"> 干涉参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetMvProtectMode01Para(HAND devHandle,short groupNo, ref TMvProtectPara  pMvProtectPara);

        /// <summary>
        /// 获取坐标系的干涉保护参数
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="groupNo"> 干涉坐标系组号（目前只有两个坐标系，所以该值为0）</param>
        /// <param name="pCrdInterfProtectPara"> 干涉参数</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetMvProtectMode01Para(HAND devHandle,short groupNo, ref TMvProtectPara  pMvProtectPara);

        //------------------------------------------------------------------------
        //                                                     
        //  自动状态返回
        //                                                   
        //------------------------------------------------------------------------
        public const Int16 AUTO_RTN_MAX_AXIS = 12;
        // 打包返回状态结构
        public struct TAutoRtnPackedSts
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	        public Int16 [] mtSts;          // 单轴状态
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	        public Int16 []  mtMio;          // 单轴专用IO
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	        public long [] mtPrfPos;       // 单轴规划位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	        public long [] mtEncPos;       // 单轴实际位置
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	        public float [] mtPrfVel;       // 单轴规划速度

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public Int16 [] crdSts;          // 坐标系状态
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public float [] crdPrfVel;	   // 坐标系运动速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public long [] crdUserSeg;      // 坐标系运行的缓冲区段号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public long [] crdFreeSpace;    // 坐标系缓冲区剩余空间
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public long [] crdUsedSpace;    // 坐标系缓冲区使用空间

            public long gpi;                // 通用输入
            public long gpo;                // 通用输出
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        public Int16 [] reserved;       // 预留

            public TAutoRtnPackedSts(bool init)
            {
                mtSts = new Int16[12];
                mtMio = new Int16[12];
                mtPrfPos = new long[12];
                mtEncPos = new long[12];
                mtPrfVel = new float[12];

                crdSts = new Int16[2];
                crdPrfVel = new float[2];
                crdUserSeg = new long[2];
                crdFreeSpace = new long[2];
                crdUsedSpace = new long[2];

                
                gpi = gpo = 0;;
                reserved = new Int16[32];
            }

         };

        /// <summary>
        /// 设置启用自动状态返回
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="mode1"> 设置哪些状态主动返回，0：固定时间周期返回（默认）; 1：周期+轴状态变化，坐标系状态变化；（目前只有两个坐标系，所以该值为0）</param>
        /// <param name="mode2"> // mode2 : 备用</param>
        /// <param name="cycleInMs"> 周期，单位ms。取值5~10000。建议值100ms。</param>
        /// <param name="port"> 端口，写0（库里会修改）</param>
        /// <param name="hostTag"> 上位机标签，写0（库里会修改）</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAutoRtnSet( HAND devHandle, long mode1, long mode2, long cycleInMs, long port, long hostTag );

        /// <summary>
        /// 设置禁用自动状态返回
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAutoRtnClr( HAND devHandle );

        /// <summary>
        /// 同步
        /// 把之前的状态缓冲清除掉。以后读到的是同步指令发送后的状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="hostTag">上位机标签，写0</param> 
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAutoRtnSyn( HAND devHandle, long hostTag );

        /// <summary>
        /// 读状态
        /// </summary>
        /// <param name="devHandle">控制器句柄</param>
        /// <param name="packedSts">打包状态</param> 
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAutoRtnGet( HAND devHandle, ref TAutoRtnPackedSts packedSts, ref short pReady );

        /// <summary>
        /// 设置PTP位置比较输出
        /// </summary>
        /// <param name="axisHandle">控制器句柄</param>
        /// <param name="dir">方向</param> 
        /// <param name="comparaPos">目标位置，单位是 脉冲</param>  
        /// <param name="upDateVel">比较后的更新速度 pulse/ms</param> 
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPtpComparePara( HAND axisHandle,short dir, long comparaPos, double upDateVel);

        /// <summary>
        /// 获取PTP比较输出状态
        /// </summary>
        /// <param name="axisHandle">控制器句柄</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPtpCompareSts( HAND axisHandle, ref short pIsCompareActive,ref short pIsCompareOut);

        //------------------------------------------------------------------------
        //                                                     
        //  点位自动收发
        //                                                   
        //------------------------------------------------------------------------

        /// <summary>
        /// 设置启用点位自动收发
        /// </summary>
        /// /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAsynCommSet( HAND devHandle, UInt32 delayInMs );

        /// <summary>
        /// 设置关闭点位自动收发
        /// </summary>
        /// /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16  NMC_DevAsynCommClr( HAND devHandle );

        /// <summary>
        /// 自动状态返回
        /// </summary>
        /// /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAsynGetSts( HAND devHandle, ref TAutoRtnPackedSts packedSts, ref short pReady );
   
        /// <summary>
        /// 多轴点位运动
        /// </summary>
        /// /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAsynAbsMoveType1( HAND axisHandle, float acc,float dec,float vel, short smooth,long arrivalBand,long stableTime,long tgtPos);

        /// <summary>
        /// 设置Do
        /// </summary>
        /// /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevAsynSetDoBit(HAND devHandle, short bitIdx, short setValue);


    }
}
