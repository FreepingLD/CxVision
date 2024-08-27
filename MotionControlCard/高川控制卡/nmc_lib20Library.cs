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
        // 通讯设置
        public enum TSearchMode { USB = 0, Ethernet, RS485 } ;

        // 其他
        public const Int32 ACC_MAX = 9999;

        //----------------------------------------------------------
        //  结构体定义                                                 
        //----------------------------------------------------------
        /// <summary>
        /// 设备信息结构
        /// </summary>
        public struct TDevInfo
        {
            public ushort address;            // 在上位机系统中的设备序号,
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] idStr;               // 识别字符串  
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] description;     // 描述符
            public ushort ID;                  // 板上的ID
            public TDevInfo(ushort add, ushort id, byte[] des)
            {
                address = add;
                idStr = new byte[16];
                description = new byte[64];
                ID = id;
            }
        };

        //----------------------------------------------------------
        //  函数声明
        //----------------------------------------------------------
        ///////////////////////////////////////////////////////////////////function
        //----------------------------------------------------------
        //	1.控制器连接及配置等
        //----------------------------------------------------------


        /// <summary>
        /// 板卡搜寻
        /// </summary>
        /// <param name="mode">通讯模式</param>
        /// <param name="pDevNo">返回设备的数目</param>
        /// <param name="pInfoList">返回设备信息</param>
        /// <returns>0:OK,其他见返回值定义</returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevSearch(TSearchMode mode, ref UInt16 pDevNo,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 4 * 84)]byte[] pInfoList);

        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern short NMC_SysUpgradeEx(ushort devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)]byte cmd,
                  byte[] sendBuff, UInt32 sendLen, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)]byte[] recBuff, UInt32 recLen);

        /// <summary>
        /// 启动库调试
        /// </summary>
        /// <param name="enable">调试模式：0--关闭调试，默认状态；1--打印到文件；2--输出到GCS</param>
        /// <param name="debugOutputFile">enable为1时表示输出文件名</param>
        /// <returns></returns>
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetCmdDebug(short enable, string debugOutputFile);

        // 板卡打开（根据序号）
        // devNo: 设备序号，取值范围[0,n]； pDevHandle: 返回设备操作句柄
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevOpen(Int16 devNo, ref UInt16 pDevHandle);

        // 板卡打开（根据名称）
        // idStr: 板卡ID字符串, 可通过指令写入。pDevHandle: 返回设备操作句柄
        // 注：ID号用户可写入，掉电不丢失，可用于区分板卡。出厂板卡号ID号为0
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevOpenByID(string idStr, ref UInt16 pDevHandle);

        // 修改板卡ID号
        // idStr: 要写入的板卡ID字符串，最长16字节，以\0结尾
        // 注：修改ID号完成后，板卡要掉电重启，新的ID才有效。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevWriteID(HAND devHandle, ref byte idStr);

        // 读取板卡ID号
        // idStr: 存储字符串的数组，数组长度大于16字节
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevReadID(HAND devHandle, StringBuilder idStr);

        // 板卡关闭
        // pDevHandle : 设备句柄指针
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevClose(ref UInt16 pDevHandle);

        // 板卡复位
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevReset(HAND devHandle);

        // 打开坐标系组
        // pCrdHandle：返回坐标系组句柄
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdOpen(HAND devHandle, ref UInt16 pCrdHandle);

        // 关闭坐标系组
        // pCrdHandle：坐标系组句柄
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdClose(ref UInt16 pCrdHandle);

        // 打开单轴
        // devHandle : 设备句柄
        // itemNo：轴号，取值范围[0,n]
        // pCrdHandle：坐标系组句柄
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtOpen(HAND devHandle, Int16 itemNo, ref UInt16 pAxisHandle);

        // 设置通讯参数
        // devHandle : 设备句柄
        // waitTimeInUs：等待时间，微秒
        // retryTimes： 通讯重试次数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetCommPara(HAND devHandle, UInt32 waitTimeInUs, UInt32 retryTimes);

        // 关闭单轴
        // devHandle : 设备句柄指针
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClose(ref UInt16 pAxisHandle);

        // 设置单轴规划高级参数
        // axisHandle : 轴句柄
        // mapAxisNo：轴号，取值范围[0,n]
        // pCrdHandle：坐标系组句柄
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtPrfConfig(HAND axisHandle, Int16 mapAxisNo, Int16 port, Int32 startPos);

        // 读取单轴规划高级参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfConfig(HAND axisHandle, ref Int16 mapAxisNo, ref Int16 port, ref  Int32 startPos);

        // 读取轴速度滤波参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAxisVelFilter(HAND axisHandle, ref Int16 pFilterCoef);

        // 设置轴速度滤波参数
        // 注：filterCoef系数在0~5之间，值越大，速度越平滑
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAxisVelFilter(HAND axisHandle, Int16 filterCoef);

        // 设置单轴比例系数
        // axisHandle : 轴句柄
        // inCoe：单轴比例系数，取值范围(0,1]
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPrfCoe(HAND axisHandle, double inCoe);

        // 读取单轴比例系数
        // axisHandle : 轴句柄
        // pInCoe：单轴比例系数，取值范围(0,1]
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPrfCoe(HAND axisHandle, ref double pInCoe);

        // 设置轴通道编码器的系数，默认为1
        // axisHandle : 轴句柄
        // encCoe：单轴通道编码器比例系数，取值范围(0,1]
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetEncCoe(HAND axisHandle, double encCoe);


        // 读取轴通道编码器的系数
        // axisHandle : 轴句柄
        // pEncCoe：比例系数，取值范围(0,1]
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetEncCoe(HAND axisHandle, ref double pEncCoe);

        // 各轴的规划模式
        public const Int16 MT_NONE_PRF_MODE = -1;  // 无效
        public const Int16 MT_PTP_PRF_MODE = 0;  // 梯形规划
        public const Int16 MT_JOG_PRF_MODE = 1;  // 连续速度模式
        public const Int16 MT_CRD_PRF_MODE = 3;  // 坐标系
        public const Int16 MT_GANTRY_MODE = 4;  // 龙门跟随模式

        // 设置单轴规划模式
        // mode：单轴运动模式
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPrfMode(HAND axisHandle, Int16 mode);

        // 获取轴规划模式
        // pMode：返回轴运动模式
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

        // 设置多个参数, 并更新。
        // pAxPara：参数，参考结构定义。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPtpPara(HAND axisHandle, ref TPtpPara refAxPara);

        // 获取Ptp参数
        // pAxPara：参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPtpPara(HAND axisHandle, ref TPtpPara pAxPara);

        // 单轴JOG运动参数结构
        public struct TJogPara
        {
            public double acc;          // 加速度
            public double dec;          // 减速度
            public double smoothCoef;   // 平滑系数，取值范围[0,199]
        };

        // 设置Jog运动参数
        // pAxPara：参数，参考结构定义。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetJogPara(HAND axisHandle, ref TJogPara refAxPara);

        // 获取Jog参数
        // pAxPara：参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetJogPara(HAND axisHandle, ref TJogPara pAxPara);

        // 设置目标运动速度
        // vel: 目标速度（最大速度），单位是 脉冲/ms
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetVel(HAND axisHandle, double vel);

        // 获取目标运动速度
        // vel: 目标速度（最大速度），单位是 脉冲/ms
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetVel(HAND axisHandle, ref  Double pVel);

        // 设置目标运动位置
        // pos: 目标位置，单位是 脉冲
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPtpTgtPos(HAND axisHandle, Int32 pos);

        // 获取目标运动位置
        // pos: 目标位置，单位是 脉冲
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPtpTgtPos(HAND axisHandle, ref  Int32 pPos);

        // 单轴启动运动，参数更新， 只针对PTP和Jog
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtUpdate(HAND axisHandle);

        // 单轴运动停止
        // 注：运动会按设定的减速度停止。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtStop(HAND axisHandle);

        // 单轴急停
        // 注：运动会按设定的急停加速度停止。如果没有设置，则会立即停止。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtEstp(HAND axisHandle);

        // 读当前规划位置（发送到执行器的位置）
        // pos : 返回位置，单位是 脉冲
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCmdPos(HAND axisHandle, ref  Int32 pos);

        // 读当前轴规划速度
        // pVel: 返回速度，单位是 脉冲/ms
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfVel(HAND axisHandle, ref  Double pVel);

        // 坐标系运动扩展参数
        public struct TAxisStsPack
        {
            public Int32 cmdPos1;		// 规划位置
            public Int32 cmdPos2;		// 规划位置
            public Int32 cmdPos3;		// 规划位置
            public Int32 cmdPos4;		// 规划位置
            public Int32 atlPos1;		// 实际位置1
            public Int32 atlPos2;		// 实际位置2
            public Int32 atlPos3;		// 实际位置3
            public Int32 atlPos4;		// 实际位置4
            public Single cmdVel1;		// 规划速度1
            public Single cmdVel2;		// 规划速度2
            public Single cmdVel3;		// 规划速度3
            public Single cmdVel4;		// 规划速度4
            public Int32 motionIO1;	    // 轴专用IO1
            public Int32 motionIO2;	    // 轴专用IO2
            public Int32 motionIO3;	    // 轴专用IO3
            public Int32 motionIO4;	    // 轴专用IO4
            public short sts1;		    // 轴状态1
            public short sts2;		    // 轴状态2
            public short sts3;		    // 轴状态3
            public short sts4;		    // 轴状态4
            public Int32 gpo;			// 通用输出
            public Int32 gpi;			// 通用输入
        };

        // 打包读取多个轴的状态，从第一个轴开始读取后续四个轴的状态
        // axisFirstHandle : 第一个轴句柄
        // count :几个轴
        // packSts: 打包的状态数据，参考结构体定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStsPack(HAND axisFirstHandle, Int16 count, ref TAxisStsPack packSts);

        // 单轴位置系统清零，规划以及编码器
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtZeroPos(HAND axisHandle);

        // 设置伺服报警清除
        // swt: 设置开关有效状态。1, 有效（输出低电平），0，无效（输出高电平）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSvClr(HAND axisHandle, Int16 swt);

        // 读编码器通道位置
        // devHandle : 控制器句柄；encId: 编码器ID,取值范围[0,n]
        // pos:返回编码器数值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetEncPos(HAND devHandle, Int16 encId, ref  Int32 pos);

        // 写编码器通道位置
        // devHandle : 控制器句柄
        // encId: 编码器ID,取值范围[0,n]
        // encPos:编码器数值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetEncPos(HAND devHandle, Int16 encId, Int32 pos);

        // 读编码器速度
        // devHandle : 控制器句柄；encId: 编码器ID,取值范围[0,n]
        // vel:返回编码器速度
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetEncVel(HAND devHandle, Int16 encId, ref  double vel);


        // 设置允许的位置误差，当位置误差超过设定值时，电机停止运动，提示位置误差超限
        // posErr为0表示不检查
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetPosErrLmt(HAND axisHandle, Int32 posErr);

        // 读取允许的位置误差，当位置误差超过设定值时，电机停止运动，提示位置误差超限
        // posErr为0表示不检查
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPosErrLmt(HAND axisHandle, ref Int32 pPosErr);

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
        // 设置编码器模式
        // encId: 编码器ID,取值范围[0,n]
        // encMode:编码器模式，参考宏定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetEncMode(HAND devHandle, Int16 encId, Int16 encMode);

        // 读取编码器模式
        // encId: 编码器ID,取值范围[0,n]
        // encMode:返回编码器模式，参考宏定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetEncMode(HAND devHandle, Int16 encId, ref Int16 encMode);

        // 设置限位开关输入是否停止运动
        // posEn：正向限位触发允许设置，1为允许，0为禁止
        // negEn: 负向限位触发允许设置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtLmtOnOff(HAND axisHandle, Int16 posSwt, Int16 negSwt);

        // 读取限位开关输入是否停止运动
        // posEn：正向限位触发允许设置，1为允许，0为禁止
        // negEn: 负向限位触发允许设置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetLmtOnOff(HAND axisHandle, ref Int16 posSwt, ref Int16 negSwt);

        // 设置限位开关触发电平
        // posSwt：正向限位触发电平设置，1为高电平触发，0为低电平触发
        // negSwt: 负向限位触发电平设置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtLmtSns(HAND axisHandle, Int16 posSwt, Int16 negSwt);

        // 读取限位开关触发电平
        // posSwt：正向限位触发电平设置，1为高电平触发，0为低电平触发
        // negSwt: 负向限位触发电平设置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetLmtSns(HAND axisHandle, ref Int16 posSwt, ref Int16 negSwt);

        // 设置伺服报警开关是否停止运动
        // swt：伺服报警开关输入允许设置，1为允许，0为禁止。（默认为高电平触发）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtAlarmOnOff(HAND axisHandle, Int16 swt);

        // 读取伺服报警开关是否停止运动
        // swt：伺服报警开关输入允许设置，1为允许，0为禁止。（默认为高电平触发）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAlarmOnOff(HAND axisHandle, ref Int16 swt);

        // 设置伺服报警开关电平
        // swt：伺服报警触发电平设置，1为高电平触发，0为低电平触发
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtAlarmSns(HAND axisHandle, Int16 swt);

        // 读取伺服报警开关电平
        // swt：伺服报警触发电平设置，1为高电平触发，0为低电平触发
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAlarmSns(HAND axisHandle, ref Int16 swt);

        // 设置软件限位是否停止运动
        // enable：软件限位触发允许设置，1为允许，0为禁止
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSwLmtOnOff(HAND axisHandle, Int16 swt);

        // 读取软件限位是否停止运动
        // enable：软件限位触发允许设置，1为允许，0为禁止
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSwLmtOnOff(HAND axisHandle, ref Int16 swt);

        // 设置软件限位数值
        // posLmt：正向软件限位设置值
        // negLmt：负向软件限位设置值。单位为脉冲数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSwLmtValue(HAND axisHandle, Int32 posLmt, Int32 negLmt);

        // 读取软件限位数值
        // posLmt：正向软件限位设置值
        // negLmt：负向软件限位设置值。单位为脉冲数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSwLmtValue(HAND axisHandle, ref  Int32 posLmt, ref  Int32 negLmt);

        // 轴运动安全参数
        public struct TSafePara
        {
            public double estpDec;		// 急停减速度
            public double maxVel;			// 最大速度
            public double maxAcc;			// 最大加速度
        };

        // 设置轴安全参数
        // TSafePara：包含急停减速度、最大允许速度、最大允许加速度
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSafePara(HAND axisHandle, ref TSafePara refPara);

        // 读取轴安全参数
        // TSafePara：包含急停减速度、最大允许速度、最大允许加速度
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSafePara(HAND axisHandle, ref TSafePara pPara);

        // 设置伺服ON, 轴静止时执行，如果后面是update指令，需要延时一个周期
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSvOn(HAND axisHandle);

        // 设置伺服OFF, 轴静止时执行，如果后面是update指令，需要延时一个周期
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetSvOff(HAND axisHandle);

        // 设定实际位置, 轴静止时执行，如果后面是update指令，需要延时一个周期
        // encPos: 设定实际位置值
        // 注：只能在轴静止时使用。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAxisPos(HAND axisHandle, Int32 pos);

        // 设定编码器位置, 轴静止时执行，如果后面是update指令，需要延时一个周期
        // encPos: 设定编码器位置值
        // 注：只能在轴静止时使用
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetEncPos(HAND axisHandle, Int32 encPos);

        // 读机械位置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetAxisPos(HAND axisHandle, ref  Int32 pos);

        // 读规划位置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetPrfPos(HAND axisHandle, ref  Int32 pos);

        // 读编码器位置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetEncPos(HAND axisHandle, ref  Int32 pos);

        // 编码器硬件捕获模式选择 
        public const Int16 CAPT_MODE_Z = 0;   // 编码器Z相捕获 
        public const Int16 CAPT_MODE_IO = 1;  // IO 捕获 
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

        // 设置捕获有效电平
        // mode : 模式选择，参考宏定义
        // ioSrc：IO输入源选择，参考宏定义
        // level: 触发沿。0为下降沿，1为上升沿
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetCaptSns(HAND axisHandle, Int16 mode, Int16 ioSrc, Int16 level);

        // 读取捕获有效电平
        // mode : 模式选择，参考宏定义
        // ioSrc：IO输入源选择，参考宏定义
        // level: 触发沿。0为下降沿，1为上升沿
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCaptSns(HAND axisHandle, ref Int16 mode, ref Int16 ioSrc, ref Int16 level);

        // 设置启动捕获
        // 注：捕获触发标志在轴状态字里。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetCapt(HAND axisHandle);

        // 读捕获位置
        // pos : 返回位置，单位:脉冲
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetCaptPos(HAND axisHandle, ref  Int32 pos);

        // 轴状态字位定义 
        public const Int32 BIT_AXIS_BUSY = 0x00000001;  // bit 0  , 运动:1 ，静止 0 
        public const Int32 BIT_AXIS_POSREC = 0x00000002;  // bit 1 , 伺服位置到达，步进模式时位置到达，伺服模式时实际位置到达误差限 
        public const Int32 BIT_AXIS_MVERR = 0x00000004;  // bit 2 , 上次运动出错，或当前无法启动运动，需要软件复位 
        public const Int32 BIT_AXIS_SVON = 0x00000008;  // bit 3  , 伺服允许        
        public const Int32 BIT_AXIS_CRD = 0x00000010; // bit 4 , 坐标系模式      
        public const Int32 BIT_AXIS_STEP = 0x00000020; // bit 5  , 步进/伺服       
        public const Int32 BIT_AXIS_LMTP = 0x00000040;  // bit 6  , 正向限位        
        public const Int32 BIT_AXIS_LMTN = 0x00000080; // bit 7  , 负面限位        
        public const Int32 BIT_AXIS_SOFTPOSLMT = 0x00000100; // bit 8  , 正向软限位触发  
        public const Int32 BIT_AXIS_SOFTNEGLMT = 0x00000200; // bit 9  , 负向软限位触发  
        public const Int32 BIT_AXIS_ALM = 0x00000400;  // bit 10  , 伺服报警，需要软件复位 
        public const Int32 BIT_AXIS_POSERR = 0x00000800;  // bit 11  , 位置超限，需要软件复位 
        public const Int32 BIT_AXIS_ESTP = 0x00001000;  // bit 12 , 急停，需要软件复位 
        public const Int32 BIT_AXIS_HWERR = 0x00002000;  // bit 13 , 急停，需要软件复位 
        public const Int32 BIT_AXIS_CAPTSET = 0x00004000;  // bit 14  , 捕获触发      

        // 读当前轴状态
        // pStsWord: 返回轴状态字。参考位定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetSts(HAND axisHandle, ref Int16 pStsWord);

        // 清除轴错误状态
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtClrError(HAND axisHandle);

        // 设置脉冲输出模式
        // en, 1：输出允许，0：输出禁止
        // mode 0：脉冲方向 1：正负脉冲
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetStepMode(HAND axisHandle, Int16 en, Int16 mode);

        // 读取脉冲输出模式
        // en, 1：输出允许，0：输出禁止
        // mode 0：脉冲方向 1：正负脉冲
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetStepMode(HAND axisHandle, ref Int16 en, ref Int16 mode);

        // 专用IO定义，arrive alm : home : limit+ , limit- 
        public const Int32 BIT_AXMTIO_LMTN = 0x00000001;    // bit 0  ,负向限位  
        public const Int32 BIT_AXMTIO_LMTP = 0x00000002;    // bit 1  ,正向限位 
        public const Int32 BIT_AXMTIO_HOME = 0x00000004;    // bit 2  ,原点 
        public const Int32 BIT_AXMTIO_ALARM = 0x00000008;    // bit 3  ,驱动报警
        public const Int32 BIT_AXMTIO_ARRIVE = 0x00000010;    // bit 4  ,电机到位

        // 读运动控制专用IO
        // inValue : 返回专用IO的状态，原点，限位，报警。参考位定义。对应位为0为低电平，1为高电平。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetMotionIO(HAND axisHandle, ref  Int32 inValue);

        //----------------------------------------------------------
        //	3.通用IO及外部资源读写
        //----------------------------------------------------------
        // 设置通用输出(按通道,支持超过32位)
        // groupID:DO组，取值范围[0,n],具体需要看控制器是否存在多组数字量输出
        // value: 设置通用数字量输出。1, 输出高电平，0，输出低电平
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDO(HAND devHandle, Int16 groupID, Int32 value);

        // 按位设置通用输出
        // bitIndex:取值范围[0,n],位序号
        // value: 设置通用数字量输出。1, 输出高电平，0，输出低电平
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOBit(HAND devHandle, Int16 bitIndex, Int16 value);

        // 按位设置通用输出
        // bitIndex:取值范围[0,n],位序号
        // value: 设置通用数字量输出。1, 输出高电平，0，输出低电平
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOBitRevs(HAND devHandle, Int16 bitIndex, Int16 value);

        // 读取通用输出
        // groupID:DO组，取值范围[0,n],具体需要看控制器是否存在多组数字量输出
        // value: 设置通用数字量输出。1, 高电平，0，低电平
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDO(HAND devHandle, Int16 groupID, ref  Int32 value);

        // 读通用输入 
        // groupID:DI组，取值范围[0,n],具体需要看控制器是否存在多组数字量输入
        // inValue: 通用数字量输入值。1, 高电平，0，低电平
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDI(HAND devHandle, Int16 groupID, ref  Int32 inValue);

        // 按位读通用输入 
        // 读通用输入(按通道,支持超过32位) 
        // bitIndex:取值范围[0,n],位序号
        // bitValue: 通用数字量输入值。1, 高电平，0，低电平
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDIBit(HAND devHandle, Int16 bitIndex, ref Int16 bitValue);

        // 获取扩展IO模块的状态
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetIOModuleSts(HAND devHandle, ref UInt32 sts);

        // 使能扩展IO模块
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetIOModuleEn(HAND devHandle, byte chDevId);

        //----------------------------------------------------------
        //	4.缓冲区配置和管理
        //----------------------------------------------------------

        /// <summary>
        /// 坐标系配置
        /// </summary>
        public struct TCrdConfig
        {
            public short axCnts;    // 参与的轴数量
            public short dummy1;
            public short dummy2;
            public short dummy3;
            public short pAxis1;    // 坐标系轴映射表,轴序号取值范围[0,n]
            public short pAxis2;
            public short pAxis3;
            public short pAxis4;
            public short port1;     // 坐标系端口映射表,统一设为0
            public short port2;
            public short port3;
            public short port4;
        };

        // 建立插补坐标系系统
        // config:坐标系配置，参考结构体定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdConfig(HAND crdHandle, ref TCrdConfig refConfig);

        // 读取插补坐标系系统配置信息
        // config:返回坐标系配置，参考结构体定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetConfig(HAND crdHandle, ref TCrdConfig pConfig);

        // 删除坐标系
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdDelete(HAND crdHandle);

        // 设置坐标系运动缓冲区是否启用，默认为启用状态
        // enFlag:启用，1：启动，0：不启用
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_EnableCrdSdram(HAND crdHandle, Int16 enFlag);

        /// <summary>
        /// 坐标系运动参数
        /// </summary>
        public struct TCrdPara
        {
            public short orgFlag;       // 是否自定义坐标系原点
            public short dummy1;
            public short dummy2;
            public short dummy3;
            public Int32 offset1;       // 自定义坐标系原点偏置（基于机械原点）
            public Int32 offset2;
            public Int32 offset3;
            public Int32 offset4;
            public double synAccMax;    // 最大合成加速度
            public double synVelMax;    // 最大合成速度

        };

        // 设置坐标系参数
        // crdPara:坐标系参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetPara(HAND crdHandle, ref TCrdPara refCrdPara);

        // 获取坐标系参数
        // crdPara:坐标系参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetPara(HAND crdHandle, ref TCrdPara pCrdPara);

        // 设置圆弧插补参数（高级指令）
        // pSetting : 设置
        //MTN_API short __stdcall NMC_CrdSetArcSecPara( HAND crdHandle,  TArcSecSetting *pSetting);

        // 读圆弧插补参数（高级指令）
        //MTN_API short __stdcall NMC_CrdGetArcSecPara( HAND crdHandle,  TArcSecSetting *pSetting);

        /// <summary>
        /// 坐标系运动扩展参数
        /// </summary>
        public struct TExtCrdPara
        {
            public double startVel;					//（默认0）
            public double T;							// (默认1)
            public double smoothDec;					//（默认accMax）
            public double abruptDec;					//（默认无穷大）
            public short lookAheadSwitch;				//( 默认有前瞻)
            public short eventTime;                     // 最小匀速时间
            public short dummy2;
            public short dummy3;
        };

        // 设置坐标系扩展参数
        // extCrdPara:坐标系扩展参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetExtPara(HAND crdHandle, ref TExtCrdPara refExtCrdPara);

        // 读取坐标系扩展参数
        // extCrdPara:坐标系扩展参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetExtPara(HAND crdHandle, ref TExtCrdPara pExtCrdPara);

        // 坐标系缓冲运动启动  
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdStartMtn(HAND crdHandle);

        // 坐标系缓冲运动回到断点
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGotoBreak(HAND crdHandle, double acc, double vel);

        // 立即平滑停止运动
        // 注：立即停止运动。并不清空指令缓冲区。需要再次启动才能继续运行缓冲区指令。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdStopMtn(HAND crdHandle);

        // 急停
        // 并不清空指令缓冲区
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdEstopMtn(HAND crdHandle);

        // 坐标系状态字位定义 
        public const Int32 BIT_CORD_BUSY = (0x00000001);    // bit 0 , 运动:1 ，静止 0,立即运动下运动停止，完成 
        public const Int32 BIT_CORD_MVERR = (0x00000002);    // bit 1 , 运动出错，或当前运动指令无法启动，需要软件复位    
        public const Int32 BIT_CORD_EMPTY = (0x00000004);    // bit 2 , 缓冲区空　       
        public const Int32 BIT_CORD_FULL = (0x00000008);    // bit 3 , 缓冲区满　               
        public const Int32 BIT_CORD_NODATASTOP = (0x00000010);    // bit 4 , 缓冲区空异常停止或者急停　    
        public const Int32 BIT_CORD_SDRAM_HWERR = (0x00000020);    // bit 5, 插补缓冲区硬件或者其他错误    

        // 读取坐标系状态
        // pStsWord：返回状态字，参考宏定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetSts(HAND crdHandle, ref Int16 pStsWord);


        // 坐标系状态字位定义:内部扩展 
        public const Int32 BIT_CORD_POSREC = (0x00000040);  // bit 6 , 伺服位置到达，步进模式时位置到达，伺服模式时实际位置到达误差限    
        public const Int32 BIT_CORD_AUXAXIS_BUSY = (0x00000080);// bit 7 , 坐标系运动中的关联轴启动前处于运动状态错误
        public const Int32 BIT_CORD_AUXAXIS_ERR = (0x00000100); // bit 8 , 插补辅助轴错误             
        public const Int32 BIT_CORD_AXIS_ERR = (0x00000200);// bit 9 , 插补轴存在报警错误（如限位、驱动报警）   
        public const Int32 BIT_CORD_SDRAM_CALC_ERR = (0x00000400);// bit 10 , SDRAM缓冲区计算错误  

        // 读取内部坐标系状态
        // pStsWord：返回状态字，定义64bits(二维的long数组)，便于后续扩展状态位
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetInnerSts(HAND crdHandle, ref Int64 pStsWord);


        // 读取规划位置XYZ
        // cnts: 读取个数，1~N
        // pos：返回坐标数组
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetPrfPos(HAND crdHandle, Int16 cnts, ref  Int32 pos);

        // 坐标系模式下，读取多个轴的机械坐标位置
        // cnts: 读取个数，1~N
        // pos：返回坐标数组
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetAxisPos(HAND crdHandle, Int16 cnts, ref  Int32 pPosArray);

        // 获取坐标系合成速度
        // pVel：坐标系合成速度
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetVel(HAND crdHandle, ref  Double pVel);

        // 设置坐标系速度倍率
        // overRide：坐标系速度倍率
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetOverRide(HAND crdHandle, double overRide);

        // 设置坐标系速度倍率
        // overRide：坐标系速度倍率
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetOverRide(HAND crdHandle, ref double pOverRide);

        // 读取指令缓冲区空闲长度
        // pRes：返回空闲的长度
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufGetFree(HAND crdHandle, ref  Int32 pRes);

        // 读取指令缓冲区已用长度
        // pLen: 长度。缓冲区中还未执行的指令个数。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufGetUsed(HAND crdHandle, ref  Int32 pLen);

        // 指令缓冲区清空
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufClr(HAND crdHandle);

        // 读段号
        // pSegNo：返回的当前段号
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetUserSegNo(HAND crdHandle, ref  Int32 pSegNo);

        //	读取总共压了多少条指令
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetBufAllCmdCnt(HAND crdHandle, ref  Int32 pCnt);

        // 结束缓冲区运动（等待运动完后才结束区运动，并置空闲标志）
        // 注：缓冲区运动结束并不立即停止运动。把指令缓冲区全部执行完后，结束缓冲区运动状态。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdEndMtn(HAND crdHandle);

        // 读取编码器位置
        // cnts: 读取个数，1~N
        // pos：返回坐标数组。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetEncPos(HAND crdHandle, Int16 cnts, ref  Int32 pos);

        public struct TPackedCrdSts3
        {
            public Int16 crdSts;          // 坐标系状态
            public Int16 axSts1;        // 坐标系里各轴状态
            public Int16 axSts2;
            public Int16 axSts3;
            public Int32 prfPos1;       // 用户坐标系下的规划位置
            public Int32 prfPos2;
            public Int32 prfPos3;
            public Int32 axisPos1;      // 机械坐标系下的规划位置
            public Int32 axisPos2;
            public Int32 axisPos3;
            public Int32 encPos1;        // 编码器位置
            public Int32 encPos2;
            public Int32 encPos3;
            public Int32 userSeg;         // 运行的缓冲区段号
            public double prfVel;         // 运动速度
            public long gpDi;             // 通用输入0~31
            public long gpDo;             // 通用输出0~31
            public Int16 motDi1;        // 限位、原点、报警。请参考专用IO位定义( 搜索 BIT_AXMTIO_LMTN )
            public Int16 motDi2;
            public Int16 motDi3;
            public Int16 reserved;        // 保留
            public Int32 crdFreeSpace;      // 缓冲区剩余空间
            public Int32 reserved2;
        };

        // 坐标系运动模式下，打包读取控制器状态
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdGetStsPack3(HAND crdHandle, Int16 cnts, ref  TPackedCrdSts3 pPackSts);

        // 清坐标系运动错误状态，同时清除所包含轴的错误状态
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdClrError(HAND crdHandle);

        // 坐标系能量跟随设置
        // gourp:跟随的能力类别组号
        // channel:通道号
        // start：起始值
        // coe:跟随的系数（与速度的比值）
        // max:跟随的最大值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetValueFollow(HAND crdHandle, Int16 group, Int16 channel, double start, double coe, double max);


        //----------------------------------------------------------
        //	5.缓冲区指令
        //----------------------------------------------------------
        // 直线插补
        // segNo:段号；endVel:终点速度;vel:最大速度;synAcc:合成加速度
        // mask:参与的轴，按位表示
        // pTgPos：目标位置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdLineXYZ(HAND crdHandle, Int32 segNo, Int16 mask, Int32[] pTgPos, double endVel, double vel, double synAcc);

        // 圆弧插补：终点位置、半径、方向
        // segNo:段号；endVel:终点速度;vel:最大速度;synAcc:合成加速度
        // radius:圆弧半径
        // pTgPos：目标位置
        // circleDir:圆弧方向
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcRadius(HAND crdHandle, Int32 segNo, Int32[] pTgPos, double radius, Int16 circleDir, double endVel, double vel, double synAcc);

        // 圆弧插补：终点位置、圆心、方向
        // segNo:段号；endVel:终点速度;vel:最大速度;synAcc:合成加速度
        // pCenterPos:圆心坐标，注意：圆心坐标为相对于起点的相对位置
        // pTgPos：目标位置
        // circleDir:圆弧方向
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcCenter(HAND crdHandle, Int32 segNo, Int32[] pTgPos, Int32[] pCenterPos, Int16 circleDir, double endVel, double vel, double synAcc);

        // 圆弧插补：起点、中点、终点
        // segNo:段号；endVel:终点速度;vel:最大速度;synAcc:合成加速度
        // pMidPos:中间位置点坐标
        // pTgPos：终点位置坐标
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdArcPPP(HAND crdHandle, Int32 segNo, Int32[] pMidPos, Int32[] pTgPos, double endVel, double vel, double synAcc);

        // 螺旋线插补
        // segNo:段号；endVel:终点速度;vel:最大速度;synAcc:合成加速度
        // mask:参与的轴，按位表示
        // pTgPos：目标位置
        // pCenterPos:圆心位置，注意：圆心坐标为相对于起点的相对位置
        // rounds：螺距
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdHelixCenter(HAND crdHandle, Int32 segNo, Int32[] pTgPos, Int32[] pCenterPos, Int16 circleDir, double rounds, double endVel, double vel, double synAcc);


        // 缓冲区输出DO组定义
        public const Int32 CRD_BUFF_DO_MOTOR_ENABLE = 1;    // 电机使能
        public const Int32 CRD_BUFF_DO_MOTOR_CLEAR = 2;     // 电机报警清除
        public const Int32 CRD_BUFF_DO_GPDO1 = 3;           // 通用输出1
        public const Int32 CRD_BUFF_DO_GPDO2 = 4;           // 通用输出2

        // 缓冲区DO
        // segNo:段号
        // gourp:Do组别
        // opBits：操作位
        // value：输出值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufDo(HAND crdHandle, Int32 segNo, Int16 group, Int32 opBits, Int32 value);

        // 通用输出宏类型
        public const Int32 BUF_OUT_GROUP_DA = 0;    // 模拟量输出
        public const Int32 BUF_OUT_GROUP_PWM = 1;    // PWM输出

        // 缓冲区输出
        // segNo:段号
        // gourp:组别
        // opBits：通道
        // value：输出值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufOut(HAND crdHandle, Int32 segNo, Int16 group, Int16 ch, Int32 value);

        // 缓冲区DI等待
        // index:通道号
        // diValue：等待值
        // waitLastTime：超时
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufWaitDI(HAND crdHandle, Int32 segNo, Int16 index, Int16 diValue, Int32 waitLastTime);

        // 缓冲区延时单位
        public const Int32 CRD_BUFF_DELAY_SCALE_MS = 0;    // 毫秒
        public const Int32 CRD_BUFF_DELAY_SCALE_SECAND = 1;    // 秒

        // 缓冲区延时
        // segNo:段号
        // scale:延时单位
        // count：延时时长
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufDelay(HAND crdHandle, Int32 segNo, Int16 scale, Int32 count);

        // 缓冲区单轴移动
        // segNo:段号
        // axMask:参与的轴
        // pTgPos：目标位置
        // blockEn:是否为阻塞模式
        // synEn：是否为同步模式
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufAxMove(HAND crdHandle, Int32 segNo, Int16 axMask, Int32[] pTgPos, Int16 blockEn, Int16 synEn);

        // 缓冲区单轴移动参数设置
        // segNo:段号
        // axis：轴号，[0,n]
        // vel:运动速度
        // acc:运动加速度
        // smoothCoef:平滑系数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufSetPtpMovePara(HAND crdHandle, Int32 segNo, Int16 axis, double vel, double acc, Int16 soomthCoef);

        // 缓冲区跟随
        // segNo:段号
        // onOff:开关
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdBufValueFollow(HAND crdHandle, Int32 segNo, Int16 onOff);


        //----------------------------------------------------------
        //	6.回零
        //----------------------------------------------------------
        // 回零类型定义                                                 
        // 回零模式    单原点	   单限位	  单Z相	   原点+Z相 原点+ -Z相	限位+ -Z相
        public enum THomeMode { HM_MODE1 = 0, HM_MODE2, HM_MODE3, HM_MODE4, HM_MODE5, HM_MODE6 } ;

        /// <summary>
        ///  回零参数设置
        /// </summary>
        public struct THomeSetting
        {
            public short mode;             // 模式，HM_MODE1 ~ HM_MODE6 （必须）
            public short dir;              // 搜寻零点方向（必须）, 0:负向，1：正向，其它值无意义
            public Int32 offset;                 // 原点偏移（必须）
            public double scan1stVel;             // 基本搜寻速度（必须）
            public double scan2ndVel;             // 低速（两次搜寻时需要）
            // 当usePreSetPtpPara=0时，回零运动的减加速度默认等于acc,起跳速度、终点速度、平滑系数默认为0
            public double acc;					// 加速度

            public byte reScanEn;         // 是否两次搜寻零点（可选，不用时设为0）
            public byte homeEdge;         // 原点，触发沿（默认下降沿）
            public byte lmtEdge;          // 限位，触发沿（默认下降沿）
            public byte zEdge;            // 限位，触发沿（默认下降沿）
            public Int32 iniRetPos;        // 起始反向运动距离（可选，不用时设为0）
            public Int32 retSwOffset;      // 反向运动时离开开关距离（可选，不用时设为0）
            public Int32 safeLen;          // 安全距离，回零时最远搜寻距离（可选，不用时设为0，不限制距离）
            public byte usePreSetPtpPara;	// 为1表示用户需要在启动回零前，自己设置回零运动（点到点）的参数
            public byte reserved0;		// 保留
            public byte reserved1;		// 保留
            public byte reserved2;		// 保留
        } ;

        // 设置回零参数
        // pHomePara: 回零参数结构，参考结构定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetHomePara(HAND axisHandle, ref THomeSetting refHomePara);

        // 读取回零参数
        // pHomePara: 回零参数结构，参考结构定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetHomePara(HAND axisHandle, ref THomeSetting pHomePara);

        // 回零状态字位定义 
        public const Int32 BIT_AXHOME_BUSY = 0x00000001;  // bit 0  , 忙    
        public const Int32 BIT_AXHOME_OK = 0x00000002;  // bit 1  , 成功  
        public const Int32 BIT_AXHOME_FAIL = 0x00000004;  // bit 2  , 失败  
        public const Int32 BIT_AXHOME_ERR_MV = 0x00000008;  // bit 3  , 错误：运动参数出错导致不动 
        public const Int32 BIT_AXHOME_ERR_SWT = 0x00000010;  // bit 4  , 错误：搜寻过程中开关没触发 

        // 读回零状态
        // pStsWord: 返回状态字。参数宏定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetHomeSts(HAND axisHandle, ref Int16 pStsWord);

        // 启动回零
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtHome(HAND axisHandle);

        // 尝试性回零（测试回零误差，不清位置）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtTryHome(HAND axisHandle);

        // 终止回零
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtHomeStop(HAND axisHandle);

        // 读新回零位置和历史回零位置的差值
        // cmdPos: 位置偏差
        // 注：回零成功时才有意义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtGetHomeError(HAND axisHandle, ref  Int32 cmdPos);

        // 变量ID
        public const Int32 VAR_PWM0_CTL = 256;	// pwm通道0打开关闭，1表示打开，0表示关闭
        public const Int32 VAR_PWM0_VALUE = 257;	// pwm通道0输出值
        public const Int32 VAR_PWM1_CTL = 258;	// pwm通道1打开关闭，1表示打开，0表示关闭
        public const Int32 VAR_PWM1_VALUE = 259;	// pwm通道1输出值
        public const Int32 VAR_EXT_DAC0 = 260;	// 扩展的DAC通道0
        public const Int32 VAR_EXT_DAC1 = 261;	// 扩展的DAC通道1
        public const Int32 VAR_OUT_OPTION = 262;	// PWM输出通道选项
        public const Int32 VAR_DAC0 = 263;	// DAC通道0~7
        public const Int32 VAR_DAC1 = 264;
        public const Int32 VAR_DAC2 = 265;
        public const Int32 VAR_DAC3 = 266;
        public const Int32 VAR_DAC4 = 267;
        public const Int32 VAR_DAC5 = 268;
        public const Int32 VAR_DAC6 = 269;
        public const Int32 VAR_DAC7 = 270;
        public const Int32 VAR_ADC0 = 271;	// ADC通道0~7
        public const Int32 VAR_ADC1 = 272;
        public const Int32 VAR_ADC2 = 273;
        public const Int32 VAR_ADC3 = 274;
        public const Int32 VAR_ADC4 = 275;
        public const Int32 VAR_ADC5 = 276;
        public const Int32 VAR_ADC6 = 277;
        public const Int32 VAR_ADC7 = 278;

        // 设置通用可寻址变量
        // varID:变量ID
        // value:设置值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetVar8B(HAND devHandle, Int32 varID, Int64 value);

        // 读取通用可寻址变量  
        // varID:变量ID
        // pValue:变量当前值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetVar8B(HAND devHandle, Int32 varID, ref  Int64 pValue);

        // 系统变量宏定义
        public const Int32 SYS_VAR_SET_STAT_ENABLE = 100;
        public const Int32 SYS_VAR_GET_CLOCK = 101;
        public const Int32 SYS_VAR_GET_USERAPP_COUNT = 107;
        public const Int32 SYS_VAR_GET_USERAPP_MIN = 108;
        public const Int32 SYS_VAR_GET_USERAPP_MAX = 109;
        public const Int32 SYS_VAR_GET_USERAPP_AVG = 110;
        public const Int32 SYS_VAR_GET_USERAPP_CURT = 111;
        public const Int32 SYS_VAR_GET_PRFINT_COUNT = 112;
        public const Int32 SYS_VAR_GET_PRFINT_MIN = 113;
        public const Int32 SYS_VAR_GET_PRFINT_MAX = 114;
        public const Int32 SYS_VAR_GET_PRFINT_AVG = 115;
        public const Int32 SYS_VAR_GET_PRFINT_CURT = 116;
        public const Int32 SYS_VAR_GET_MAINLP_COUNT = 117;
        public const Int32 SYS_VAR_GET_MAINLP_MIN = 118;
        public const Int32 SYS_VAR_GET_MAINLP_MAX = 119;
        public const Int32 SYS_VAR_GET_MAINLP_AVG = 120;
        public const Int32 SYS_VAR_GET_MAINLP_CURT = 121;

        // 设置通用可寻址变量
        // varID:变量ID
        // value:设置值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SysSetVar8B(HAND devHandle, Int32 varID, Int64 value);

        // 读取通用可寻址变量  
        // varID:变量ID
        // pValue:变量当前值
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SysGetVar8B(HAND devHandle, Int32 varID, ref  Int64 pValue);

        //向备份内存写数据(总长度约510byte)
        //src: 要写入的数据
        //len: 要写入的长度，单位：byte
        //off: 要写入的地址(偏移量)
        // 注：1)写入的数据掉电不丢失。
        //     2)一次最多写1440字节。总长度约510byte。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BackSramWrite(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] src, Int32 len, Int32 off);

        //从备份内存读数据
        //dst: 读出的数据暂存区
        //len: 要读出的长度，单位：byte,一次最多读1440字节。总长度约510byte。
        //off: 读数据的地址(偏移量)
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BackSramRead(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] dest, Int32 len, Int32 off);


        // 写用户参数
        // add ：参数区的偏移地址(字节地址)
        // len : 写入长度,单位：byte
        // pWrData ：要写入的数据；
        // 注：1)写入的数据掉电不丢失。
        //     2)一次最多写1440字节。参数区总长度为2048字节。
        //     3)此指令比其它指令操作时间会长,如果出现通讯错误（返回-1），则需要将通过NMC_SetCommPara延长指令通讯时间
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserParaWr(HAND devHandle, UInt32 addr, UInt32 len, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] pWrData);

        // 读用户参数
        // add ：参数区的偏移地址(字节地址)
        // len : 写入长度,单位：byte
        // pRdData ：要读取的数据存储
        // 注：一次最多读1440字节。参数区总长度为2048字节。（参考函数 ： NMC_UserParaWr）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserParaRd(HAND devHandle, UInt32 addr, UInt32 len, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] pWrData);

        // 平台：控制器时间结构
        public struct TNMCTime
        {
            public Int16 year;           // 年，真实年份。取值范围[2000,2099]
            public Int16 mon;            // 月,取值范围[1,12]
            public Int16 day;            // 日,取值范围[1,31]
            public Int16 hour;           // 时,取值范围[0,23]
            public Int16 min;            // 分,取值范围[0,59]
            public Int16 second;         // 秒,取值范围[0,59]
        } ;

        // 读时间
        // pTime ：返回时间结构，参考 TNMCTime 结构定义
        // 注：时间在出厂时根据实际时间设定，用户不可通过指令修改
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetTime(HAND devHandle, ref TNMCTime pTime);

        // 掉电保存：备份数据定义
        public struct TBackGroup1
        {
            public Int32 crdSegNo;		// 坐标系运动的段号
            public Int32 crdPrfPos1;	// 坐标系运动的规划位置
            public Int32 crdPrfPos2;
            public Int32 crdPrfPos3;
            public Int32 axPrfPos1;	// 规划位置
            public Int32 axPrfPos2;
            public Int32 axPrfPos3;
            public Int32 axPrfPos4;
            public Int32 axPrfPos5;
            public Int32 axPrfPos6;
            public Int32 axPrfPos7;
            public Int32 axPrfPos8;
            public Int32 axisPos1;	// 机械位置
            public Int32 axisPos2;
            public Int32 axisPos3;
            public Int32 axisPos4;
            public Int32 axisPos5;
            public Int32 axisPos6;
            public Int32 axisPos7;
            public Int32 axisPos8;
            public Int32 encPos1;		// 编码器位置
            public Int32 encPos2;
            public Int32 encPos3;
            public Int32 encPos4;
            public Int32 encPos5;
            public Int32 encPos6;
            public Int32 encPos7;
            public Int32 encPos8;
        };
        // 读取当前备份的变量数值（断电自动保存）
        // pBackVar : 备份的变量值，参考结构体定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetBackedVarGroup1(HAND devHandle, ref TBackGroup1 pBackVar);

        // 开始或者关闭变量的自动备份（断电自动保存，默认为关闭状态）
        // en:是否开启，1：开启变量的自动备份，0：停止变量的自动备份
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetBackedVarOnOff(HAND devHandle, Int16 en);

        // 读取当前自动备份的开启状态
        // pEn:当前的开启状态
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetBackedVarOnOff(HAND devHandle, ref Int16 pEn);


        //----------------------------------------------------------
        //	6.数据收集
        //----------------------------------------------------------


        // Collect模块：变量类型
        public const Int32 COLLECT_ADDRESS_PRF_POS = 0;	// 规划位置
        public const Int32 COLLECT_ADDRESS_AXIS_POS = 1;			// 机械位置
        public const Int32 COLLECT_ADDRESS_ENC_POS = 2;		// 编码器位置
        public const Int32 COLLECT_ADDRESS_CMD_POS = 3;		// 命令位置
        public const Int32 COLLECT_ADDRESS_AXIS_VEL = 4;		// 电机速度
        public const Int32 COLLECT_ADDRESS_CRD_POS = 5;		// 坐标系位置
        public const Int32 COLLECT_ADDRESS_CRD_VEL = 6;	// 坐标系速度
        // 获取需要采集数据变量的地址
        //  index:变量的序号(从0开始)
        //  type: 变量的类型,参数‘Collect模块：变量类型’宏定义
        //  pAddr: 返回的数据地址
        // pDataLen: 返回的该数据长度
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetCollectDataAddr(HAND devHandle, short index, short dataType, out UInt32 pAddr, out short pDataLen);

        //缓冲区运动安全参数
        public struct TCrdSafePara
        {
            public double estpDec;     // 急停加速度
            public double maxVel;      // 最大速度
            public double maxAcc;      // 最大加速度
        }
        // 设置轴缓冲区运动安全参数
        // pPara：包含急停减速度、最大允许速度、最大允许加速度
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetSafePara(HAND crdHandle, ref TCrdSafePara pPara);

        // 设置轴缓冲区运动偏移
        // count:设置偏移的轴数
        // pOffsetArray：缓冲区运动偏移，long数组
        // 注：会同时修改坐标系内相关轴的运动偏移！
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CrdSetOffset(HAND crdHandle, short count, ref long[] pOffsetArray);

        public const Int32 LASER_GROUP = (12);		// 最大激光能量组数
        public const Int32 LASET_POINT = (40);		// 每组最大点数

        public const Int32 SPOT_WELDING = (0);		// 点焊
        public const Int32 LINE_WELDING = (1);		// 线焊

        public const Int32 LASER_DA = (1);			// DA输出
        public const Int32 LASER_PWM_DUTY = (2);		// 占空比输出
        public const Int32 LASER_PWM_FRQ = (3);			// 频率输出

        public struct TLaserPower
        {
            public short[] time;		// 每个点之间的间隔时间,长度LASET_POINT
            public short[] power;		// 各点的能量大小,长度LASET_POINT
            public short count;					// 实际压入点数
        }

        // 设置激光的能量
        // group : 设置哪一组激光能量
        // pLaserPower ： 设置的数据
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetLaserPower(HAND devHandle, short group, ref TLaserPower pLaserPower);

        // 立即指令打开激光
        // group : 设置哪一组激光能量
        // type: 激光焊接的类型
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserOn(HAND devHandle, short group, short type);

        // 立即指令关闭激光
        // group : 设置哪一组激光能量
        // type: 激光焊接的类型
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserOff(HAND devHandle, short group, short type);

        // 激光功能配置
        // outputType：输出的类型	1:LASER_DA  2:LASER_PWM_DUTY	3:LASER_PWM_FRQ
        // optionVal: 当作为占空比输出时，该值为PWM的频率，单位HZ；当为频率输出时，该值作为占空比值，（0~100）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_LaserConfig(HAND devHandle, short outputType, long optionVal);

        public struct THSIOPara
        {
            public Int16 isArray; 		// 是否固定间距还是数组。0：固定间距（仅支持），默认0
            public Int16 outMode; 		// 输出模式。默认1
            // 1：只输出gate 立即开或关,
            // 2: 根据位移输出gate。( 优先 )
            // 3: 根据位移输出gate，gate 和同部trigger 信号同步
            // 4: 根据位移输出gate，gate 和信号输入同步
            public Int16 posSrc; 		// 比较模式，外部编码器还是内部规划值。
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
        };

        // 配置HSIO功能的参数
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ConfigHSIOPara(HAND devHandle, ref THSIOPara pHSIOPara);


        //激光的开关光延时控制 延时单位：1ms  // 该接口不用于SHIO的设置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetLaserOnOffDelay(HAND devHandle, Int32 onDelay, Int32 offDelay);


        // 激光输出模式
        public const Int32 LASER_DISABLE_MODE = (0);    // 禁用激光功能
        public const Int32 IMMEDIT_OUTPUT_MODE = (1);	// 立即输出模式
        public const Int32 SIMPLE_FOLLOW_MODE = (2);	// 速度能量跟随模式
        public const Int32 TIME_ARRAY_OUTPUT_MODE = (3); 	// 波形输出模式
        public const Int32 SHIO_OUTPUT_MODE = (4); // 位置比较输出模式

        //设置激光输出的模式。该模式的设定，约束相应指令的功能和操作
        //mode: 激光输出模式，参考上述宏定义
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetLaserMode(HAND devHandle, Int16 mode);


        // 切换轴（允许和坐标系不完全一致）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOChangeAxisMask(HAND devHandle, Int16 axisMask);

        // 允许GATE输出
        // 注：设置后根据模式输出
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOGateOn(HAND devHandle);

        // 禁止GATE输出
        // 注：设置后立即禁止输出
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOGateOff(HAND devHandle);

        // 设置Trigger 输出，
        // triggerFreq : 设置triger 脉冲频率单位：HZ。
        // triggerWidth : 设置triger 脉冲宽度，单位：s（内部最小值：1/36us ）。默认0。
        // 注：设置后立即输出
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOTriggerOn(HAND devHandle, double freq, double width);

        // 禁止GATE输出
        // 注：设置后马上输出
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SHIOTriggerOff(HAND devHandle);


        // 允许GATE输出
        // 注：设置后根据模式输出
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BufSHIOGateOn(HAND devHandle, Int32 segNo);


        //禁止GATE输出
        //注：缓冲区执行到该指令后立即禁止输出
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_BufSHIOGateOff(HAND devHandle, Int32 segNo);

        // 设置龙门主动轴
        // axisHandle: 龙门主动轴句柄
        // group     : 龙门组号
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetGantryMaster(HAND axisHandle, short group);

        // 设置龙门从动轴
        // axisHandle  : 龙门从动轴句柄;
        // group       : 龙门组号
        // gantryErrLmt: 龙门保护误差;
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetGantrySlave(HAND axisHandle, short group, long gantryErrLmt);

        // 龙门功能关闭
        // group :       龙门组号
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DelGantryGroup(HAND axisHandle, short group);

        // 硬件限位配置
        // posLmtEnable: 正向限位触发允许设置，1为允许，0为禁止
        // negLmtEnable: 负向限位触发允许设置，1为允许，0为禁止
        // posLmtSns: 正向限位触发电平，1为高电平触发，0为低电平触发
        // negLmtSns: 正向限位触发电平，1为高电平触发，0为低电平触发
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetLmtCfg(HAND axisHandle, short posLmtEnable, short negLmtEnable, short posLmtSns, short negLmtSns);

        // 伺服报警配置
        // alarmEnable: 伺服报警触发允许设置，1为允许，0为禁止
        // alarmSns:     伺服报警触发电平，1为高电平触发，0为低电平触发
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetAlarmCfg(HAND axisHandle, short alarmEnable, short alarmSns);

        // 设置脉冲输出滤波
        // coe系数： 范围0~65535，0不滤波，数值越大滤波效果越明显。
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_MtSetStepFilter(HAND axisHandle, UInt16 coe);


        // 设置通用输出(按通道,支持超过32位),带默认group
        // value: 设置通用数字量输出。1, 输出高电平，0，输出低电平
        // groupID:DO组，取值范围[0,n],具体需要看控制器是否存在多组数字量输出
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDOGroup(HAND devHandle, long value, short groupID);

        // 读通用输入(按通道,支持超过32位) ,带默认group
        // pInValue: 通用数字量输入值。1, 高电平，0，低电平
        // groupID:DI组，取值范围[0,n],0: 本地DI31~DI0, 1: 本地DI63~DI32，其他指扩展IO模块
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDIGroup(HAND devHandle, out Int32 pInValue, short groupID);

        // 读取通用输出(按通道,支持超过32位),带默认group
        // pDoValue: 设置通用数字量输出。1, 高电平，0，低电平
        // groupID:DO组，取值范围[0,n],,0: 本地DO31~DO0, 1: 本地DO63~DO32，其他指扩展IO模块
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDOGroup(HAND devHandle, out Int32 pDoValue, short groupID);


        public struct TCollectCfg
        {
            public short count;								// 需要采集的变量个数
            public short interval;							// 采集的间隔时间,0表示每隔1毫米采集一次数据，1表示每隔2ms...

            public UInt32 address1;	// 变量的地址
            public UInt32 address2;	// 变量的地址
            public UInt32 address3;	// 变量的地址
            public UInt32 address4;	// 变量的地址
            public UInt32 address5;	// 变量的地址
            public UInt32 address6;	// 变量的地址
            public UInt32 address7;	// 变量的地址
            public UInt32 address8;	// 变量的地址

            public short length1;	// 每个变量的长度
            public short length2;	// 每个变量的长度
            public short length3;	// 每个变量的长度
            public short length4;	// 每个变量的长度
            public short length5;	// 每个变量的长度
            public short length6;	// 每个变量的长度
            public short length7;	// 每个变量的长度
            public short length8;	// 每个变量的长度

        }

        // 采集模块：触发模式
        public const Int32 COLLECT_MODE_NONE = 0;	// 无条件
        public const Int32 COLLECT_MODE_G_SRC1 = 1;	// 采集源1数值大于比较值
        public const Int32 COLLECT_MODE_L_SRC1 = 2;	// 采集源1数值小于比较值
        public const Int32 COLLECT_MODE_DIFF = 3;	// 采集源1与采集源2两项差值大于比较值
        public struct TCollectTrig
        {
            public short mode;								// 触发模式，
            public short source1;								// 触发源1
            public short source2;								// 触发源2
            public short startDelay;							// 触发启动的延时
            public double value;								// 触发比较值
        }
        // 配置采集数据通道,需要配置对应结构体参数
        // pCollect:采集模块配置
        // pTrig:采集模块触发方式配置
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ConfigCollect(HAND devHandle, ref TCollectCfg pCollect, ref TCollectTrig pTrig);


        // 启动或停止数据采集
        // en：1启动 0停止
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_CollectOnOff(HAND devHandle, short en);

        // Collect模块：采集状态
        public const Int32 COLLECT_BUSY = 0x0001;
        public const Int32 COLLECT_OVERRIDE_DATA = 0x0002;
        public const Int32 COLLECT_PUSH_DATA_ERR = 0x0004;
        // 获取采集状态:
        // pSts：采集状态，按位表示各自状态，参考‘Collect模块：采集状态’宏定义
        // pDataLen: 采集的数据量
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetCollectSts(HAND devHandle, out short pSts, out Int32 pDatalen);

        // 获取采数据:
        // len：采集数据长度（单位：char,一次最多读1440字节）
        // pData: 采集的数据（均以char为单元存储）
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetCollectData(HAND devHandle, short len, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)]double[] pData);

        // 清除采集状态
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_ClearCollectSts(HAND devHandle);

        // 读设备唯一序列号
        // pUID ：返回设备唯一序列号,为四个Int32的数据
        // example:unsigned long devID[4];
        //         NMC_GetUID(g_hDev,devID);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetUID(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)]UInt32[] pData);


        // 读取库的版本
        // pVersion：接收版本信息
        // example(c):
        //          char dllVersion[128];
        //          NMC_GetDllVersion(dllVersion);
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetDllVersion([MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] pVersion);

        // 当前运动控制器固件的版本等信息
        // 读取当前运动控制器固件的版本等信息
        // devHandle : 设备句柄
        // pVersion：版本信息
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetMtLibVersion(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]byte[] pVersion);

        // paraID
        public const UInt32 PARA_IP_ADDR = 100;		// IP地址，四个字节分别表示四段
        public const UInt32 PARA_IP_MSK = 101;	// IP mask
        public const UInt32 PARA_IP_GW = 102;	// Gateway
        public const UInt32 PARA_IP_DHCP = 103;		// DHCP
        public const UInt32 PARA_WRITE_EN = 999;		// 参数保存

        // 读取系统参数（long 型）
        // paraID :　系统参数ID，参见后面定义。pValue : 返回值
        // 注：此函数可用于设置板卡上的扩展资源
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevGetPara(HAND devHandle, UInt32 paraID, out Int32 pValue);

        // 设置系统参数（long 型）
        // paraID :　系统参数ID，参见后面定义。value : 设置值
        // 注：此函数可用于读取板卡上的扩展资源
        // 注：IP地址等参数写成功后，需要调用NMC_DevSetPara(devHandle,PARA_WRITE_EN,1)才能使得写下去的参数保存
        // 注：IP地址等参数写成功后，将在控制器重新启动后生效
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DevSetPara(HAND devHandle, UInt32 paraID, Int32 value);

        // 读取最后一次的错误代码
        // 返回值：错误代码
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_GetLastErr();

        // 设置指令错误返回值模式
        // mode:0-标准模式，将返回详细的错误代码；1--简洁模式，只返回错误代码类别
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetErrCodeMode(Int16 mode);

        // 通讯扩展
        // 用户指令传输，只写
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserCmdWrite(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] sendBuffer, UInt32 sendLen);

        // 用户指令传输，只读
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_UserCmdRead(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] sendBuffer, UInt32 sendLen,
                    [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] recBuffer, UInt32 waitLen);

        // 批量数据传输，只写
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_DataTransfer(HAND devHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] byte[] sendBuffer, UInt32 sendLen);

        // 设置调试信息
        [DllImport(DLL_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 NMC_SetDebugErrorEn(HAND devHandle, Int16 debugErrEn);

    }
}
