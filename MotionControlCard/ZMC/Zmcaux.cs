using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

/********************************** ZMC系列控制器  ************************************************
**--------------文件信息--------------------------------------------------------------------------------
**文件名: zmcaux.h
**创建人: 郑孝洋
**时间: 20130621
**描述: ZMCDLL 辅助函数

**------------修订历史记录----------------------------------------------------------------------------
** 修改人: 吴勇
** 版  本: V1.5
** 日　期: 20150927
** 描　述: 
**
**------------------------------------------------------------------------------------------------------
********************************************************************************************************/

namespace cszmcaux
{
	public class zmcaux
	{

        //ZAUX支持的最大轴数宏
        //#define MAX_AXIS_AUX   32  
        //#define MAX_CARD_AUX   16

        public const int MAX_AXIS_AUX = 32;
        public const int MAX_CARD_AUX = 16;




        
        /*********************************************************
        数据类型定义
        **********************************************************/

        //typedef unsigned __int64   uint64;  
        //typedef __int64   int64;  


        //#define BYTE           INT8U
        //#define WORD           INT16U
        //#define DWORD          INT32U
        //typedef unsigned char  BYTE;
        //typedef unsigned short  WORD;
        //typedef unsigned int  DWORD;
        //#define __stdcall 
        //typedef unsigned char  uint8;                   /* defined for unsigned 8-bits integer variable     无符号8位整型变量  */
        //typedef signed   char  int8;                    /* defined for signed 8-bits integer variable        有符号8位整型变量  */
        ///typedef unsigned short uint16;                  /* defined for unsigned 16-bits integer variable     无符号16位整型变量 */
        //typedef signed   short int16;                   /* defined for signed 16-bits integer variable         有符号16位整型变量 */
        //typedef unsigned int   uint32;                  /* defined for unsigned 32-bits integer variable     无符号32位整型变量 */
        //typedef signed   int   int32;                   /* defined for signed 32-bits integer variable         有符号32位整型变量 */
        //typedef float          fp32;                    /* single precision floating point variable (32bits) 单精度浮点数（32位长度） */
        //typedef double         fp64;                    /* double precision floating point variable (64bits) 双精度浮点数（64位长度） */
        //typedef unsigned int   uint;                  /* defined for unsigned 32-bits integer variable     无符号32位整型变量 */



        /************************************************/
        //错误码 
        /************************************************/
        //#define ERR_OK  0
        //#define ERROR_OK 0
        //#define ERR_SUCCESS  0

        //#define ERR_AUX_OFFSET       30000

        //#define ERR_NOACK               ERR_AUX_OFFSET      //无应答
        //#define ERR_ACKERROR            (ERR_AUX_OFFSET+1)  //应答错误
        //#define ERR_AUX_PARAERR         (ERR_AUX_OFFSET+2)  //参数错误
        //#define ERR_AUX_NOTSUPPORT      (ERR_AUX_OFFSET+3)  //参数错误

        public const int ERR_OK = 0;
        public const int ERROR_OK = 0;
        public const int ERR_SUCCESS = 0;

        public const int ERR_AUX_OFFSET = 30000;

        public const int ERR_NOACK = ERR_AUX_OFFSET;
        public const int ERR_ACKERROR =  (ERR_AUX_OFFSET+1);
        public const int ERR_AUX_PARAERR = (ERR_AUX_OFFSET+2);
        public const int ERR_AUX_NOTSUPPORT = (ERR_AUX_OFFSET+3);

        
/*********************************************************
函数声明
**********************************************************/


/*************************************************************
Description:    //封装 Excute 函数, 以便接收错误
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Execute(ZMC_HANDLE handle, const char* pszCommand, char* psResponse, uint32 uiResponseLength);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Execute", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Execute(IntPtr handle, string pszCommand, byte[] psResponse,UInt32 uiResponseLength);

/*************************************************************
Description:    //封装 Excute 函数, 以便接收错误
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_DirectCommand(ZMC_HANDLE handle, const char* pszCommand, char* psResponse, uint32 uiResponseLength);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_DirectCommand", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_DirectCommand(IntPtr handle, string pszCommand, byte[] psResponse,UInt32 uiResponseLength);

/*************************************************************
Description:    //命令跟踪设置.
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_SetTraceFile(int bifTofile, const char *pFilePathName);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_SetTraceFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_SetTraceFile(int bifTofile,byte[] pFilePathName);

        /*************************************************************
Description:    //与控制器建立链接， 串口方式.
Input:          //串口号COMId 
Output:         //卡链接phandle
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_OpenCom(uint32 comid, ZMC_HANDLE * phandle);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_OpenCom", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_OpenCom(UInt32 comid, out IntPtr phandle);

/*************************************************************
Description:    //快速控制器建立链接
Input:          //最小串口号uimincomidfind
Input:          //最大串口号uimaxcomidfind
Input:          //链接时间uims
Output:         //有效COM pcomid
Output:         //卡链接handle
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_SearchAndOpenCom(uint32 uimincomidfind, uint32 uimaxcomidfind,uint* pcomid, uint32 uims, ZMC_HANDLE * phandle);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_SearchAndOpenCom", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_SearchAndOpenCom(UInt32 uimincomidfind, UInt32 uimaxcomidfind, ref uint pcomid , UInt32 uims, out IntPtr phandle); 

/*************************************************************
Description:    //可以修改缺省的波特率等设置
uint32 dwByteSize = 8, uint32 dwParity = NOPARITY, uint32 dwStopBits = ONESTOPBIT
windows:
#define NOPARITY            0
#define ODDPARITY           1
#define EVENPARITY          2
注意: ONESTOPBIT 不是1
#define ONESTOPBIT          0
#define ONE5STOPBITS        1
#define TWOSTOPBITS         2
linux:
dwParity:0/1/2
dwStopBits
1:1个停止位
2:2个停止位
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_SetComDefaultBaud(uint32 dwBaudRate, uint32 dwByteSize, uint32 dwParity, uint32 dwStopBits);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_SetComDefaultBaud", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_SetComDefaultBaud(UInt32 dwbaudRate, UInt32 dwByteSize, UInt32 dwParity, UInt32 dwStopBits);  

/*************************************************************
Description:    //修改控制器IP地址
Input:          //卡链接handle 
Input:          //IP地址  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_SetIp(ZMC_HANDLE handle, char * ipaddress);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetIp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_SetIp(IntPtr handle, string ipaddress); 

/*************************************************************
Description:    //与控制器建立链接
Input:          //IP地址，字符串的方式输入
Output:         //卡链接handle
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_OpenEth(char *ipaddr, ZMC_HANDLE * phandle);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_OpenEth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_OpenEth(string ipaddr, out IntPtr phandle);

/*************************************************************
Description:    //快速检索IP列表
Input:          //uims 响应时间
Input:          //addrbufflength		最大长度
output:			//ipaddrlist		当前晚点IP列表
Return:         //错误码, ERR_OK表示有搜索到.
*************************************************************/
//int32 __stdcall ZAux_SearchEthlist(char *ipaddrlist, uint32 addrbufflength, uint32 uims);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_SearchEthlist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_SearchEthlist(byte[] ipaddrlist, UInt32 addrbufflength, UInt32 uims);

/*************************************************************
Description:    //快速检索控制器
Input:          //控制器IP地址
Input:          //响应时间
Output:         //
Return:         //错误码, ERR_OK表示有搜索到.
*************************************************************/
//int32 __stdcall ZAux_SearchEth(const char *ipaddress,  uint32 uims);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_SearchEth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_SearchEth(string ipaddress, UInt32 uims);

/*************************************************************
Description:    //关闭控制器链接
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Close(ZMC_HANDLE  handle);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Close", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Close(IntPtr handle);

/*************************************************************
Description:    //暂停继续运行BAS项目
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Resume(ZMC_HANDLE handle);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Resume", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Resume(IntPtr handle);

/*************************************************************
Description:    //暂停
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Pause(ZMC_HANDLE handle);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Pause", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Pause(IntPtr handle);

/*************************************************************
Description:    //单个BAS文件生成ZAR并且下载到ROM运行
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_BasDown(ZMC_HANDLE handle,const char *Filename);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_BasDown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_BasDown(IntPtr handle, string Filename);



//#if 0
    //IO指令
    // 可以使用 ZMC_GetIn ZMC_GetOutput 等
//#endif
/*************************************************************
Description:    //读取输入信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetIn(ZMC_HANDLE handle, int ionum , uint32 *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetIn(IntPtr handle,int ionum,ref UInt32 piValue);

/*************************************************************
Description:    //打开输出信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetOp(ZMC_HANDLE handle, int ionum, uint32 iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetOp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetOp(IntPtr handle,int ionum, UInt32 iValue);

/*************************************************************
Description:    //读取输出口状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetOp(ZMC_HANDLE handle, int ionum, uint32 *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetOp(IntPtr handle,int ionum,ref UInt32 piValue);

/*************************************************************
Description:    //读取模拟量输入信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAD(ZMC_HANDLE handle, int ionum , float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAD(IntPtr handle,int ionum,ref float pfValue);

/*************************************************************
Description:    //打开模拟量输出信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetDA(ZMC_HANDLE handle, int ionum, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetDA(IntPtr handle,int ionum, float fValue);

/*************************************************************
Description:    //读取模拟输出口状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetDA(ZMC_HANDLE handle, int ionum, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetDA(IntPtr handle,int ionum,ref float pfValue);

/*************************************************************
Description:    //设置输入口反转
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetInvertIn(ZMC_HANDLE handle, int ionum, int bifInvert);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetInvertIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetInvertIn(IntPtr handle,int ionum,int  bifInvert);

/*************************************************************
Description:    //读取输入口反转状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetInvertIn(ZMC_HANDLE handle, int ionum, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetInvertIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetInvertIn(IntPtr handle,int ionum,ref int  piValue);

/*************************************************************
Description:    //设置pwm频率
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetPwmFreq(ZMC_HANDLE handle, int ionum, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetPwmFreq", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetPwmFreq(IntPtr handle,int ionum,float fValue);

/*************************************************************
Description:    //读取pwm频率
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetPwmFreq(ZMC_HANDLE handle, int ionum, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetPwmFreq", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetPwmFreq(IntPtr handle,int ionum,ref float pfValue);

/*************************************************************
Description:    //设置pwm占空比
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetPwmDuty(ZMC_HANDLE handle, int ionum, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetPwmDuty", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetPwmDuty(IntPtr handle,int ionum, float fValue);

/*************************************************************
Description:    //读取pwm占空比
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetPwmDuty(ZMC_HANDLE handle, int ionum, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetPwmDuty", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetPwmDuty(IntPtr handle,int ionum,ref float pfValue);

//#if 0
    //通过modbus快速读取特殊寄存器
//#endif

/*************************************************************
Description:    //参数 快速读取多个输入
Input:          //卡链接handle  
Output:         //按位存储
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_GetModbusIn(ZMC_HANDLE handle, int ionumfirst, int ionumend, uint8 *pValueList);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_GetModbusIn(IntPtr handle,int ionumfirst,int ionumend, byte[] pValueList);

/*************************************************************
Description:    //参数 快速读取多个当前的输出状态
Input:          //卡链接handle  
Output:         //按位存储
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_GetModbusOut(ZMC_HANDLE handle, int ionumfirst, int ionumend, uint8 *pValueList);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusOut", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_GetModbusOut(IntPtr handle,int ionumfirst,int ionumend, byte[] pValueList);

/*************************************************************
Description:    //参数 快速读取多个当前的DPOS
Input:          //卡链接handle  
Output:         //按存储
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_GetModbusDpos(ZMC_HANDLE handle, int imaxaxises, float *pValueList);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusDpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_GetModbusDpos(IntPtr handle,int imaxaxises, float[] pValueList);

/*************************************************************
Description:    //参数 快速读取多个当前的MPOS
Input:          //卡链接handle  
Output:         //按存储
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_GetModbusMpos(ZMC_HANDLE handle, int imaxaxises, float *pValueList);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusMpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_GetModbusMpos(IntPtr handle,int imaxaxises, float[] pValueList);

/*************************************************************
Description:    //参数 快速读取多个当前的速度
Input:          //卡链接handle  
Output:         //按存储
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_GetModbusCurSpeed(ZMC_HANDLE handle, int imaxaxises, float *pValueList);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusCurSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_GetModbusCurSpeed(IntPtr handle,int imaxaxises, float[] pValueList);


//#if 0
    //采用ZMC_DirectCommand 来快速获取一些状态, ZMC_DirectCommand的执行比ZMC_Execute要快
	// 只有参数，变量，数组元素等能使用ZMC_DirectCommand
	// 20130901以后的版本，一些运动函数也可以调用ZMC_DirectCommand，当运动条件不满足的时候，会立刻返回失败。
	// ZMC_DirectCommand调用运动函数时，参数必须是具体的数值，不能是变量表达式。
//#endif

/*************************************************************
Description:    //通用的参数修改函数 sParam: 填写参数名称
Input:          //卡链接handle 
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetParam(ZMC_HANDLE handle,const char *sParam,int iaxis, float fset);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetParam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetParam(IntPtr handle,string sParam ,int iaxis, float fset);

/*************************************************************
Description:    //参数 通用的参数读取函数, sParam:填写参数名称
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetParam(ZMC_HANDLE handle,const char *sParam, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetParam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetParam(IntPtr handle,string sParam,int iaxis, ref  float pfValue);

/*************************************************************
Description:    //设置加速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetAccel(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAccel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetAccel(IntPtr handle,int iaxis, float fValue);

/*************************************************************
Description:    //读取加速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAccel(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAccel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAccel(IntPtr handle,int iaxis,ref float pfValue);

/*************************************************************
Description:    //读取叠加轴
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAddax(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAddax", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAddax(IntPtr handle,int iaxis,ref int piValue);

/*************************************************************
Description:    //设置轴告警信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetAlmIn(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAlmIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetAlmIn(IntPtr handle,int iaxis, int iValue);

/*************************************************************
Description:    //读取告警信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAlmIn(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAlmIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAlmIn(IntPtr handle,int iaxis,ref int piValue);

/*************************************************************
Description:    //设置轴类型
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetAtype(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAtype", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetAtype(IntPtr handle,int iaxis, int iValue);

/*************************************************************
Description:    //读取轴类型
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAtype(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAtype", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAtype(IntPtr handle,int iaxis,ref int piValue);

/*************************************************************
Description:    //读取轴状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAxisStatus(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAxisStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAxisStatus(IntPtr handle,int iaxis,ref int piValue);

/*************************************************************
Description:    //设置轴地址
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetAxisAddress(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAxisAddress", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetAxisAddress(IntPtr handle,int iaxis, int iValue);

/*************************************************************
Description:    //读取轴地址
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAxisAddress(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAxisAddress", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAxisAddress(IntPtr handle,int iaxis,ref int piValue);

/*************************************************************
Description:    //设置轴使能
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetAxisEnable(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAxisEnable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetAxisEnable(IntPtr handle,int iaxis, int iValue);

/*************************************************************
Description:    //读取轴使能状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetAxisEnable(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAxisEnable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetAxisEnable(IntPtr handle,int iaxis,ref int piValue);

/*************************************************************
Description:    //设置链接速率
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetClutchRate(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetClutchRate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetClutchRate(IntPtr handle,int iaxis, float fValue);

/*************************************************************
Description:    //读取链接速率
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetClutchRate(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetClutchRate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetClutchRate(IntPtr handle,int iaxis,ref float pfValue);

/*************************************************************
Description:    //设置锁存触发的结束坐标范围点
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetCloseWin(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetCloseWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetCloseWin(IntPtr handle,int iaxis, float fValue);

/*************************************************************
Description:    //读取锁存触发的结束坐标范围点
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetCloseWin(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetCloseWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetCloseWin(IntPtr handle,int iaxis,ref float pfValue);

/*************************************************************
Description:    //设置拐角减速
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetCornerMode(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetCornerMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetCornerMode(IntPtr handle,int iaxis, int pfValue);

/*************************************************************
Description:    //读取拐角减速
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetCornerMode(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetCornerMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetCornerMode(IntPtr handle,int iaxis,ref int piValue);

/*************************************************************
Description:    //设置回零爬行速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_SetCreep(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetCreep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_SetCreep(IntPtr handle,int iaxis, float fValue);

/*************************************************************
Description:    //读取回零爬行速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
////int32 __stdcall ZAux_Direct_GetCreep(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetCreep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32  ZAux_Direct_GetCreep(IntPtr handle,int iaxis,ref float pfValue);

/*************************************************************
Description:    //设置原点信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetDatumIn(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDatumIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetDatumIn(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取原点信号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetDatumIn(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDatumIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetDatumIn(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置减速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetDecel(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDecel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetDecel(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取减速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetDecel(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDecel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetDecel(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置拐角减速角度，开始减速角度，单位为弧度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetDecelAngle(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDecelAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetDecelAngle(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取拐角开始减速角度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetDecelAngle(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDecelAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetDecelAngle(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置轴位置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetDpos(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetDpos(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取轴位置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetDpos(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetDpos(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取内部编码器值
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetEncoder(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEncoder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetEncoder(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取当前运动的最终位置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetEndMove(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEndMove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetEndMove(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取当前和缓冲中运动的最终位置，可以用于相对绝对转换
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetEndMoveBuffer(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEndMoveBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetEndMoveBuffer(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置SP运动的结束速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetEndMoveSpeed(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetEndMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetEndMoveSpeed(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取SP运动的结束速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetEndMoveSpeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEndMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetEndMoveSpeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置错误标记，和AXISSTATUS做与运算来决定哪些错误需要关闭WDOG。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetErrormask(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetErrormask", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetErrormask(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取错误标记，和AXISSTATUS做与运算来决定哪些错误需要关闭WDOG。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetErrormask(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetErrormask", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetErrormask(IntPtr handle, int iaxis,ref int piValue);

/*************************************************************
Description:    //设置快速JOG输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFastJog(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFastJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFastJog(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取快速JOG输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFastJog(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFastJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFastJog(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置快速减速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFastDec(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFastDec", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFastDec(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取快速减速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFastDec(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFastDec", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFastDec(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取随动误差
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFe(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFe(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置最大允许的随动误差值
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFeLimit(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFeLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFeLimit(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取最大允许的随动误差值
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFeLimit(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFeLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFeLimit(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置报警时随动误差值
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFRange(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFRange", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFRange(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取报警时的随动误差值
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFeRange(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFeRange", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFeRange(IntPtr handle, int iaxis,ref float fValue);

/*************************************************************
Description:    //设置保持输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFholdIn(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFholdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFholdIn(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取保持输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFholdIn(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFholdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFholdIn(IntPtr handle, int iaxis,ref int piValue);

/*************************************************************
Description:    //设置轴保持速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFhspeed(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFhspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFhspeed(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取轴保持速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFhspeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFhspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFhspeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置SP运动的运行速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetForceSpeed(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetForceSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetForceSpeed(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取SP运动的运行速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetForceSpeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetForceSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetForceSpeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置正向软限位
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFsLimit(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFsLimit(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取正向软限位
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFsLimit(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFsLimit(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置小圆限速最小半径
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFullSpRadius(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFullSpRadius", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFullSpRadius(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取小圆限速最小半径
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFullSpRadius(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFullSpRadius", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFullSpRadius(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置正向硬限位输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFwdIn(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFwdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFwdIn(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取正向硬限位输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFwdIn(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFwdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFwdIn(IntPtr handle, int iaxis,ref int piValue);

/*************************************************************
Description:    //设置正向JOG输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetFwdJog(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFwdJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetFwdJog(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取正向JOG输入
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetFwdJog(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFwdJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetFwdJog(IntPtr handle, int iaxis,ref int piValue);

/*************************************************************
Description:    //读取轴是否运动结束
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetIfIdle(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetIfIdle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetIfIdle(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置脉冲输出模式
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetInvertStep(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetInvertStep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetInvertStep(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取脉冲输出模式
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetInvertStep(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetInvertStep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetInvertStep(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置插补时轴是否参与速度计算，缺省参与（1）。此参数只对直线和螺旋的第三个轴起作用
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetInterpFactor(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetInterpFactor", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetInterpFactor(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取插补时轴是否参与速度计算，缺省参与（1）。此参数只对直线和螺旋的第三个轴起作用
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetInterpFactor(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetInterpFactor", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetInterpFactor(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置JOG时速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetJogSpeed(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetJogSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetJogSpeed(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取JOG时速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetJogSpeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetJogSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetJogSpeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取当前链接运动的参考轴号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetLinkax(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetLinkax", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetLinkax(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //读取当前除了当前运动是否还有缓冲
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetLoaded(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetLoaded", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetLoaded(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置轴起始速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetLspeed(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetLspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetLspeed(IntPtr handle, int iaxis,  float fValue);

/*************************************************************
Description:    //读取轴起始速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetLspeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetLspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetLspeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置回零反找等待时间
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetHomeWait(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetHomeWait", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetHomeWait(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取回零反找等待时间
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetHomeWait(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetHomeWait", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetHomeWait(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取编码器锁存示教返回状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMark(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMark", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMark(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //读取编码器锁存示教返回状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMarkB(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMarkB", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMarkB(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置脉冲输出最高频率
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetMaxSpeed(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMaxSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetMaxSpeed(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取脉冲输出最高频率
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMaxSpeed(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMaxSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMaxSpeed(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置连续插补
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetMerge(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMerge", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetMerge(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取连续插补状态
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMerge(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMerge", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMerge(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //读取当前被缓冲起来的运动个数
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMovesBuffered(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMovesBuffered", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMovesBuffered(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //读取当前正在运动指令的MOVE_MARK标号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMoveCurmark(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMoveCurmark", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMoveCurmark(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置运动指令的MOVE_MARK标号
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetMovemark(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMovemark", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetMovemark(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //设置反馈位置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetMpos(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetMpos(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取反馈位置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMpos(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMpos(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取反馈速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMspeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMspeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取当前正在运动指令类型
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetMtype(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMtype", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetMtype(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置修改偏移位置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetOffpos(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetOffpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetOffpos(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取修改偏移位置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetOffpos(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOffpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetOffpos(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置锁存触发的结束坐标范围点。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetOpenWin(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetOpenWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetOpenWin(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取锁存触发的结束坐标范围点。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetOpenWin(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOpenWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetOpenWin(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取返回锁存的测量反馈位置(MPOS)
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRegPos(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRegPos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRegPos(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取返回锁存的测量反馈位置(MPOS)
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRegPosB(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRegPosB", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRegPosB(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取返回轴当前运动还未完成的距离
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRemain(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRemain", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRemain(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //参数  轴剩余的缓冲, 按直线段来计算
				  REMAIN_BUFFER为唯一一个可以加AXIS并用ZAux_DirectCommand获取的.
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRemain_LineBuffer(ZMC_HANDLE handle, int iaxis,int * piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRemain_LineBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRemain_LineBuffer(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //参数  轴剩余的缓冲, 按最复杂的空间圆弧来计算
				  REMAIN_BUFFER为唯一一个可以加AXIS并用ZAux_DirectCommand获取的.
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRemain_Buffer(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRemain_Buffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRemain_Buffer(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置根据REP_OPTION设置来自动循环轴DPOS和MPOS坐标。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetRepDist(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRepDist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetRepDist(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取根据REP_OPTION设置来自动循环轴DPOS和MPOS坐标。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetOpenRepDist(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOpenRepDist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetOpenRepDist(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置坐标重复设置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetRepOption(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRepOption", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetRepOption(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取坐标重复设置
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRepOption(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRepOption", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRepOption(IntPtr handle, int iaxis, ref int piValue);


/*************************************************************
Description:    //读取根据REP_OPTION设置来自动循环轴DPOS和MPOS坐标。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRepDist(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRepDist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRepDist(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置负向硬件限位开关对应的输入点编号，-1无效。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetRevIn(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRevIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetRevIn(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取负向硬件限位开关对应的输入点编号，-1无效。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRevIn(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRevIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRevIn(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置负向JOG输入对应的输入点编号，-1无效。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetRevJog(ZMC_HANDLE handle, int iaxis, int iValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRevJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetRevJog(IntPtr handle, int iaxis, int iValue);

/*************************************************************
Description:    //读取负向JOG输入对应的输入点编号，-1无效。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRevJog(ZMC_HANDLE handle, int iaxis, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRevJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRevJog(IntPtr handle, int iaxis, ref int piValue);

/*************************************************************
Description:    //设置负向软限位位置。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetRsLimit(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetRsLimit(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取负向软限位位置。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetRsLimit(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetRsLimit(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置轴速度，单位为units/s，当多轴运动时，作为插补运动的速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetSpeed(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetSpeed(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取轴速度，单位为units/s，当多轴运动时，作为插补运动的速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetSpeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetSpeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置 S曲线设置。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetSramp(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetSramp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetSramp(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取 S曲线设置。
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetSramp(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetSramp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetSramp(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置 自定义速度的SP运动的起始速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetStartMoveSpeed(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetStartMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetStartMoveSpeed(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取自定义速度的SP运动的起始速度
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetStartMoveSpeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetStartMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetStartMoveSpeed(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置 减速到最低的最小拐角
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetStopAngle(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetStopAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetStopAngle(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取减速到最低的最小拐角
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetStopAngle(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetStopAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetStopAngle(IntPtr handle, int iaxis, ref float pfValue);


/*************************************************************
Description:    //设置 减速倒角
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetZsmooth(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetZsmooth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetZsmooth(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取倒角半径
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetZsmooth(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetZsmooth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetZsmooth(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //设置 脉冲当量
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetUnits(ZMC_HANDLE handle, int iaxis, float fValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetUnits", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetUnits(IntPtr handle, int iaxis, float fValue);

/*************************************************************
Description:    //读取脉冲当量
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetUnits(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetUnits", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetUnits(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //读取返回轴当前当前运动和缓冲运动还未完成的距离
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetVectorBuffered(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVectorBuffered", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetVectorBuffered(IntPtr handle, int iaxis, ref float pfValue);

/*************************************************************
Description:    //参数
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetVpSpeed(ZMC_HANDLE handle, int iaxis, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVpSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetVpSpeed(IntPtr handle, int iaxis, ref float pfValue);




/*************************************************************
Description:    //全局变量读取, 也可以是参数等等
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetVariablef(ZMC_HANDLE handle, const char *pname, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVariablef", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetVariablef(IntPtr handle, string pname, ref float pfValue);

/*************************************************************
Description:    //全局变量读取, 也可以是参数等等
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetVariableInt(ZMC_HANDLE handle, const char *pname, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVariableInt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetVariableInt(IntPtr handle, string pname, ref int piValue);

///////////////////////  只有下面的运动函数支持直接调用，并不是所有的指令都支持
///////////////////////  必须 20130901 以后的控制器版本支持

/*************************************************************
Description:    //BASE指令调用

仅仅修改在线命令的BASE列表，不对控制器的运行任务的BASE进行修改.
修改后，后续的所有MOVE等指令都是以这个BASE为基础

Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Base(ZMC_HANDLE handle, int imaxaxises, int *piAxislist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Base", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Base(IntPtr handle, int imaxaxises, int[] piAxislist);

/*************************************************************
Description:    //定义DPOS
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Defpos(ZMC_HANDLE handle, int imaxaxises, float *pfDposlist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Defpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Defpos(IntPtr handle, int imaxaxises, float[] pfDposlist);

/*************************************************************
Description:    //相对插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Move(ZMC_HANDLE handle, int imaxaxises, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Move(IntPtr handle, int imaxaxises, float[] pfDposlist);

/*************************************************************
Description:    //相对插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveSp(ZMC_HANDLE handle, int imaxaxises, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveSp(IntPtr handle, int imaxaxises, float[] pfDposlist);


/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveAbs(ZMC_HANDLE handle, int imaxaxises, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveAbs(IntPtr handle, int imaxaxises, float[] pfDposlist);

/*************************************************************
Description:    //插补 BASE后立刻调用运动函数.
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_BaseAndMoveAbs(ZMC_HANDLE handle, int imaxaxises, int *piAxislist, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_BaseAndMoveAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_BaseAndMoveAbs(IntPtr handle, int imaxaxises, int[] piAxislist, float[] pfDposlist);

/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveAbsSp(ZMC_HANDLE handle, int imaxaxises, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveAbsSp(IntPtr handle, int imaxaxises, float[] pfDposlist);


/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveModify(ZMC_HANDLE handle, int imaxaxises, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveModify", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveModify(IntPtr handle, int imaxaxises, float[] pfDposlist);


/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

end1              第一个轴运动坐标
end2              第二个轴运动坐标
centre1    第一个轴运动圆心，相对与起始点。
centre2    第二个轴运动圆心，相对与起始点。
direction  0-逆时针，1-顺时针

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCirc(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCirc(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);

/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

end1              第一个轴运动坐标
end2              第二个轴运动坐标
centre1    第一个轴运动圆心，相对与起始点。
centre2    第二个轴运动圆心，相对与起始点。
direction  0-逆时针，1-顺时针

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCircSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCircSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCircSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);


/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

end1              第一个轴运动坐标，绝对位置
end2              第二个轴运动坐标，绝对位置
centre1    第一个轴运动圆心，绝对位置
centre2    第二个轴运动圆心，绝对位置
direction  0-逆时针，1-顺时针

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCircAbs(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCircAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCircAbs(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);

/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

end1              第一个轴运动坐标，绝对位置
end2              第二个轴运动坐标，绝对位置
centre1    第一个轴运动圆心，绝对位置
centre2    第二个轴运动圆心，绝对位置
direction  0-逆时针，1-顺时针

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCircAbsSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCircAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCircAbsSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);



/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

mid1       第一个轴中间点，相对起始点距离
mid2       第二个轴中间点，相对起始点距离
end1              第一个轴结束点，相对起始点距离
end2              第二个轴结束点，相对起始点距离

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCirc2(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCirc2(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2);


/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

mid1       第一个轴中间点，绝对位置
mid2       第二个轴中间点，绝对位置
end1              第一个轴结束点，绝对位置
end2              第二个轴结束点，绝对位置

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCirc2Abs(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2Abs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCirc2Abs(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2);

/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

mid1       第一个轴中间点，相对起始点距离
mid2       第二个轴中间点，相对起始点距离
end1              第一个轴结束点，相对起始点距离
end2              第二个轴结束点，相对起始点距离

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCirc2Sp(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2Sp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCirc2Sp(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2);


/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle

mid1       第一个轴中间点，绝对位置
mid2       第二个轴中间点，绝对位置
end1              第一个轴结束点，绝对位置
end2              第二个轴结束点，绝对位置

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveCirc2AbsSp(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2AbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveCirc2AbsSp(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2);



/*************************************************************
Description:    //插补
Input:          //卡链接handle

end1              第一个轴运动坐标

end2              第二个轴运动坐标

centre1    第一个轴运动圆心，相对与起始点

centre2    第二个轴运动圆心，相对与起始点

direction  0-逆时针，1-顺时针

distance3第三个轴运动距离。

mode      第三轴的速度计算:
0(缺省)
 第三轴参与速度计算。
1
 第三轴不参与速度计算。
 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelical(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelical(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

end1              第一个轴运动坐标

end2              第二个轴运动坐标

centre1    第一个轴运动圆心，相对与起始点

centre2    第二个轴运动圆心，相对与起始点

direction  0-逆时针，1-顺时针

distance3第三个轴运动距离。

mode      第三轴的速度计算:
0(缺省)
 第三轴参与速度计算。
1
 第三轴不参与速度计算。
 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelicalAbs(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelicalAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelicalAbs(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

end1              第一个轴运动坐标

end2              第二个轴运动坐标

centre1    第一个轴运动圆心，相对与起始点

centre2    第二个轴运动圆心，相对与起始点

direction  0-逆时针，1-顺时针

distance3第三个轴运动距离。

mode      第三轴的速度计算:
0(缺省)
 第三轴参与速度计算。
1
 第三轴不参与速度计算。
 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelicalSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelicalSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelicalSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

end1              第一个轴运动坐标

end2              第二个轴运动坐标

centre1    第一个轴运动圆心，相对与起始点

centre2    第二个轴运动圆心，相对与起始点

direction  0-逆时针，1-顺时针

distance3第三个轴运动距离。

mode      第三轴的速度计算:
0(缺省)
 第三轴参与速度计算。
1
 第三轴不参与速度计算。
 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelicalAbsSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelicalAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelicalAbsSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);


/*************************************************************
Description:    //插补
Input:          //卡链接handle

mid1       第一个轴中间点

mid2       第二个轴中间点

end1              第一个轴结束点

end2              第二个轴结束点

distance3第三个轴运动距离

mode      第三轴的速度计算:

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelical2(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelical2(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

mid1       第一个轴中间点

mid2       第二个轴中间点

end1              第一个轴结束点

end2              第二个轴结束点

distance3   第三个轴运动结束点

mode      第三轴的速度计算:

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelical2Abs(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2Abs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelical2Abs(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

mid1       第一个轴中间点

mid2       第二个轴中间点

end1              第一个轴结束点

end2              第二个轴结束点

distance3第三个轴运动距离

mode      第三轴的速度计算:

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelical2Sp(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2Sp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelical2Sp(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

mid1       第一个轴中间点

mid2       第二个轴中间点

end1              第一个轴结束点

end2              第二个轴结束点

distance3   第三个轴运动结束点

mode      第三轴的速度计算:

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MHelical2AbsSp(ZMC_HANDLE handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2AbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MHelical2AbsSp(IntPtr handle, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);


/*************************************************************
Description:    //插补
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipse(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipse", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipse(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipseAbs(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipseAbs(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipseSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipseSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

/*************************************************************
Description:    //插补
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipseAbsSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipseAbsSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);


/*************************************************************
Description:    //插补 椭圆 + 螺旋
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipseHelical(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelical", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipseHelical(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

/*************************************************************
Description:    //插补  椭圆 + 螺旋
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipseHelicalAbs(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis,float fDistance3);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelical", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipseHelical(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);  

/*************************************************************
Description:    //插补 椭圆 + 螺旋
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipseHelicalSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelicalSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipseHelicalSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);  

/*************************************************************
Description:    //插补  椭圆 + 螺旋
Input:          //卡链接handle

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MEclipseHelicalAbsSp(ZMC_HANDLE handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis,float fDistance3);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelicalAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MEclipseHelicalAbsSp(IntPtr handle, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);  


/*************************************************************
Description:    //插补  空间圆弧
Input:          //卡链接handle
end1              第1个轴运动距离参数1

end2              第2个轴运动距离参数1

end3              第3个轴运动距离参数1

centre1    第1个轴运动距离参数2

centre2    第2个轴运动距离参数2

centre3    第3个轴运动距离参数2

mode      指定前面参数的意义
0 当前点，中间点，终点三点定圆弧，距离参数1为终点距离，距离参数2为中间点距离。
1 走最小的圆弧，距离参数1为终点距离，距离参数2为圆心的距离。
2 当前点，中间点，终点三点定圆，距离参数1为终点距离，距离参数2为中间点距离。
3 先走最小的圆弧，再继续走完整圆，距离参数1为终点距离，距离参数2为圆心的距离。
 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MSpherical(ZMC_HANDLE handle, float fend1, float fend2, float fend3, float fcenter1, float fcenter2, float fcenter3, int imode,float fcenter4, float fcenter5);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MSpherical", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MSpherical(IntPtr handle, float fend1, float fend2, float fend3, float fcenter1, float fcenter2, float fcenter3, int imode, float fcenter4, float fcenter5);  

/*************************************************************
Description:    //插补  空间圆弧
Input:          //卡链接handle
end1              第1个轴运动距离参数1

end2              第2个轴运动距离参数1

end3              第3个轴运动距离参数1

centre1    第1个轴运动距离参数2

centre2    第2个轴运动距离参数2

centre3    第3个轴运动距离参数2

mode      指定前面参数的意义
0 当前点，中间点，终点三点定圆弧，距离参数1为终点距离，距离参数2为中间点距离。
1 走最小的圆弧，距离参数1为终点距离，距离参数2为圆心的距离。
2 当前点，中间点，终点三点定圆，距离参数1为终点距离，距离参数2为中间点距离。
3 先走最小的圆弧，再继续走完整圆，距离参数1为终点距离，距离参数2为圆心的距离。
 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MSphericalSp(ZMC_HANDLE handle, float fend1, float fend2, float fend3, float fcenter1, float fcenter2, float fcenter3, int imode,float fcenter4, float fcenter5);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MSphericalSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MSphericalSp(IntPtr handle, float fend1, float fend2, float fend3, float fcenter1, float fcenter2, float fcenter3, int imode, float fcenter4, float fcenter5); 


/*************************************************************
Description:    //渐开线圆弧插补运动，相对移动方式，当起始半径0直接扩散时从0角度开始
Input:          //卡链接handle

              centre1: 第1轴圆心的相对距离

              centre2: 第2轴圆心的相对距离

              circles:  要旋转的圈数，可以为小数圈，负数表示顺时针.

              pitch:   每圈的扩散距离，可以为负。

distance3        第3轴螺旋的功能，指定第3轴的相对距离，此轴不参与速度计算。

distance4        第4轴螺旋的功能，指定第4轴的相对距离，此轴不参与速度计算。
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveSpiral(ZMC_HANDLE handle, float centre1, float centre2, float circles, float pitch, float distance3, float distance4);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSpiral", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveSpiral(IntPtr handle, float centre1, float centre2, float circles, float pitch, float distance3, float distance4); 

/*************************************************************
Description:    //渐开线圆弧插补运动，相对移动方式，当起始半径0直接扩散时从0角度开始
Input:          //卡链接handle

              centre1: 第1轴圆心的相对距离

              centre2: 第2轴圆心的相对距离

              circles:  要旋转的圈数，可以为小数圈，负数表示顺时针.

              pitch:   每圈的扩散距离，可以为负。

distance3        第3轴螺旋的功能，指定第3轴的相对距离，此轴不参与速度计算。

distance4        第4轴螺旋的功能，指定第4轴的相对距离，此轴不参与速度计算。

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveSpiralSp(ZMC_HANDLE handle, float centre1, float centre2, float circles, float pitch, float distance3, float distance4);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSpiralSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveSpiralSp(IntPtr handle, float centre1, float centre2, float circles, float pitch, float distance3, float distance4); 

/*************************************************************
Description:    //空间直线运动，根据下一个直线运动的绝对坐标在拐角自动插入圆弧，加入圆弧后会使得运动的终点与直线的终点不一致，拐角过大时不会插入圆弧，当距离不够时会自动减小半径
Input:          //卡链接handle
end1              第1个轴运动绝对坐标
end2              第2个轴运动绝对坐标
end3              第3个轴运动绝对坐标
next1      第1个轴下一个直线运动绝对坐标
next2      第2个轴下一个直线运动绝对坐标
next3      第3个轴下一个直线运动绝对坐标
radius      插入圆弧的半径，当过大的时候自动缩小。

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveSmooth(ZMC_HANDLE handle, float end1, float end2, float end3, float next1, float next2, float next3, float radius);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSmooth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveSmooth(IntPtr handle, float end1, float end2, float end3, float next1, float next2, float next3, float radius); 

/*************************************************************
Description:    //空间直线运动，根据下一个直线运动的绝对坐标在拐角自动插入圆弧，加入圆弧后会使得运动的终点与直线的终点不一致，拐角过大时不会插入圆弧，当距离不够时会自动减小半径
Input:          //卡链接handle
end1              第1个轴运动绝对坐标
end2              第2个轴运动绝对坐标
end3              第3个轴运动绝对坐标
next1      第1个轴下一个直线运动绝对坐标
next2      第2个轴下一个直线运动绝对坐标
next3      第3个轴下一个直线运动绝对坐标
radius      插入圆弧的半径，当过大的时候自动缩小。

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveSmoothSp(ZMC_HANDLE handle, float end1, float end2, float end3, float next1, float next2, float next3, float radius);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSmoothSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveSmoothSp(IntPtr handle, float end1, float end2, float end3, float next1, float next2, float next3, float radius);



/*************************************************************
Description:    //运动暂停
Input:          //卡链接handle  

0（缺省） 暂停当前运动。 
1 在当前运动完成后正准备执行下一条运动指令时暂停。 
2 在当前运动完成后正准备执行下一条运动指令时，并且两条指令的MARK标识不一样时暂停。这个模式可以用于一个动作由多个指令来实现时，可以在一整个动作完成后暂停。 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MovePause(ZMC_HANDLE handle, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MovePause", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MovePause(IntPtr handle, int imode);

/*************************************************************
Description:    //运动暂停
Input:          //卡链接handle  

0（缺省） 暂停当前运动。 
1 在当前运动完成后正准备执行下一条运动指令时暂停。 
2 在当前运动完成后正准备执行下一条运动指令时，并且两条指令的MARK标识不一样时暂停。这个模式可以用于一个动作由多个指令来实现时，可以在一整个动作完成后暂停。 

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveResume(ZMC_HANDLE handle);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveResume", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveResume(IntPtr handle);

/*************************************************************
Description:    //在当前的运动末尾位置增加速度限制，用于强制拐角减速
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveLimit(ZMC_HANDLE handle, float limitspeed);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveLimit(IntPtr handle, float limitspeed);

/*************************************************************
Description:    //在运动缓冲中加入输出指令
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveOp(ZMC_HANDLE handle, int ioutnum, int ivalue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveOp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveOp(IntPtr handle, int ioutnum, int ivalue);

/*************************************************************
Description:    //在运动缓冲中加入输出指令
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveOpMulti(ZMC_HANDLE handle, int ioutnumfirst, int ioutnumend, int ivalue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveOpMulti", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveOpMulti(IntPtr handle, int ioutnumfirst, int ioutnumend, int ivalue);

/*************************************************************
Description:    //在运动缓冲中加入输出指令
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveOp2(ZMC_HANDLE handle, int ioutnum, int ivalue, int iofftimems);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveOp2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveOp2(IntPtr handle, int ioutnum, int ivalue, int iofftimems);

/*************************************************************
Description:    //在运动缓冲中加入AOUT输出指令
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveAout(ZMC_HANDLE handle, int ioutnum, float fvalue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveAout", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveAout(IntPtr handle, int ioutnum, float fvalue);

/*************************************************************
Description:    //在运动缓冲中加入延时指令
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveDelay(ZMC_HANDLE handle, int itimems);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveDelay", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveDelay(IntPtr handle, int itimems);

/*************************************************************
Description:    //插补 旋转的插补运动
Input:          //卡链接handle
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_MoveTurnabs(ZMC_HANDLE handle, int tablenum ,int imaxaxises, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveTurnabs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_MoveTurnabs(IntPtr handle, int tablenum, int imaxaxises, float[] pfDisancelist);

/*************************************************************
Description:    //插补 直接调用运动函数 20130901 以后的控制器版本支持
Input:          //卡链接handle
				tablenum       存储旋转参数的table编号
refpos1    第一个轴参考点，绝对位置  
refpos2    第二个轴参考点，绝对位置
mode      1-参考点是当前点前面，2-参考点是结束点后面，3-参考点在中间，采用三点定圆的方式。
end1              第一个轴结束点，绝对位置
end2              第二个轴结束点，绝对位置
imaxaxises        螺旋轴个数
pfDisancelist	螺旋轴距离列表
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_McircTurnabs(ZMC_HANDLE handle, int tablenum ,float refpos1,float refpos2,int mode,float end1,float end2,int imaxaxises, float *pfDisancelist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_McircTurnabs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_McircTurnabs(IntPtr handle, int tablenum, float refpos1, float refpos2, int mode, float end1, float end2, int imaxaxises, float[] pfDisancelist);

/*************************************************************
Description:    //电子凸轮
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Cam(ZMC_HANDLE handle, int istartpoint, int iendpoint, float ftablemulti, float fDistance);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Cam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Cam(IntPtr handle, int istartpoint, int iendpoint, float ftablemulti, float fDistance);

/*************************************************************
Description:    //电子凸轮
Input:          //卡链接handle  

Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Cambox(ZMC_HANDLE handle, int istartpoint, int iendpoint, float ftablemulti, float fDistance, int ilinkaxis, int ioption, float flinkstartpos);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Cambox", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Cambox(IntPtr handle, int istartpoint, int iendpoint, float ftablemulti, float fDistance, int ilinkaxis, int ioption, float flinkstartpos);


/*************************************************************
Description:    //连接 特殊凸轮
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Movelink(ZMC_HANDLE handle, float fDistance, float fLinkDis, float fLinkAcc, float fLinkDec,int iLinkaxis, int ioption, float flinkstartpos);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Movelink", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Movelink(IntPtr handle, float fDistance, float fLinkDis, float fLinkAcc, float fLinkDec, int iLinkaxis, int ioption, float flinkstartpos);

/*************************************************************
Description:    //连接 特殊凸轮
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Moveslink(ZMC_HANDLE handle, float fDistance, float fLinkDis, float startsp, float endsp,int iLinkaxis, int ioption, float flinkstartpos);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Moveslink", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Moveslink(IntPtr handle, float fDistance, float fLinkDis, float startsp, float endsp, int iLinkaxis, int ioption, float flinkstartpos);


/*************************************************************
Description:    //连接 同步运动指令
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Connect(ZMC_HANDLE handle, float ratio, int link_axis,int move_axis);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Connect(IntPtr handle, float ratio, int link_axis,int move_axis);

/*************************************************************
Description:    //连接 同步运动指令
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Connpath(ZMC_HANDLE handle, float ratio, int link_axis,int move_axis);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connpath", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Connpath(IntPtr handle, float ratio, int link_axis,int move_axis);

/*************************************************************
Description:    //位置锁存指令
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Regist(ZMC_HANDLE handle, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Regist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Regist(IntPtr handle, int imode);

/*************************************************************
Description:    //编码器输入齿轮比，缺省(1,1)
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_EncoderRatio(ZMC_HANDLE handle,int mpos_count,int input_count);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_EncoderRatio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_EncoderRatio(IntPtr handle, int mpos_count, int input_count);

/*************************************************************
Description:    //设置步进输出齿轮比，缺省(1,1)
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_StepRatio(ZMC_HANDLE handle,int mpos_count,int input_count);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_StepRatio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_StepRatio(IntPtr handle, int mpos_count, int input_count);

/*************************************************************
Description:    //所有轴立即停止
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Rapidstop(ZMC_HANDLE handle, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Rapidstop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Rapidstop(IntPtr handle, int imode);

/*************************************************************
Description:    //多个轴运动停止
Input:          //卡链接handle  轴号， 距离
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_CancelAxisList(ZMC_HANDLE handle, int imaxaxises, int *piAxislist, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_CancelAxisList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_CancelAxisList(IntPtr handle, int imaxaxises, int[] piAxislist, int imode);

/*************************************************************
Description:    //CONNFRAME机械手指令

Input:          //卡链接handle
					frame机械手类型
					tablenum  相关参数TABLE起点
					imaxaxises 关联虚拟轴个数
					piAxislist 虚拟轴列表
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Connframe(ZMC_HANDLE handle, int frame, int tablenum , int imaxaxises , int *piAxislist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connframe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Connframe(IntPtr handle, int frame, int tablenum, int imaxaxises, int[] piAxislist);

/*************************************************************
Description:    //CONNREFRAME机械手指令

Input:          //卡链接handle
					frame机械手类型
					tablenum  相关参数TABLE起点
					imaxaxises 关联关节轴个数
					piAxislist 关节轴列表
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Connreframe(ZMC_HANDLE handle, int frame, int tablenum , int imaxaxises , int *piAxislist);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connreframe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Connreframe(IntPtr handle, int frame, int tablenum, int imaxaxises, int[] piAxislist);

//#if 0
    //单轴函数
//#endif

/*************************************************************
Description:    //叠加
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Singl_Addax(ZMC_HANDLE handle, int iaxis, int iaddaxis);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Singl_Addax", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Singl_Addax(IntPtr handle, int iaxis, int iaddaxis);

/*************************************************************
Description:    //单轴运动停止
Input:          //卡链接handle  轴号， 模式
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Singl_Cancel(ZMC_HANDLE handle, int iaxis, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Singl_Cancel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Singl_Cancel(IntPtr handle, int iaxis, int imode);

/*************************************************************
Description:    //连续运动
Input:          //卡链接handle  轴号， 方向
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Singl_Vmove(ZMC_HANDLE handle, int iaxis, int idir);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Singl_Vmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Singl_Vmove(IntPtr handle, int iaxis, int idir);

/*************************************************************
Description:    //回零
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Singl_Datum(ZMC_HANDLE handle, int iaxis, int imode);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Singl_Datum", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Singl_Datum(IntPtr handle, int iaxis, int imode);

/*************************************************************
Description:    //单轴运动
Input:          //卡链接handle  轴号， 距离
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Singl_Move(ZMC_HANDLE handle, int iaxis, float fdistance);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Singl_Move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Singl_Move(IntPtr handle, int iaxis, float fdistance);

/*************************************************************
Description:    //单轴运动
Input:          //卡链接handle  轴号， 距离
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_Singl_MoveAbs(ZMC_HANDLE handle, int iaxis, float fdistance);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Singl_MoveAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_Singl_MoveAbs(IntPtr handle, int iaxis, float fdistance);


//#if 0
    //辅助函数
//#endif

/*********************内存操作

/*********************内存操作
/*************************************************************
Description:    //写VR, 
Input:          //卡链接handle  轴号， 距离
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetVrf(ZMC_HANDLE handle,int vrstartnum, int numes, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetVrf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetVrf(IntPtr handle, int vrstartnum, int numes, ref float pfValue);

/*************************************************************
Description:    //VR读取, 可以一次读取多个
Input:          //卡链接handle  
Output:         //pfValue  多个时必须分配空间.
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetVrf(ZMC_HANDLE handle, int vrstartnum, int numes, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVrf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetVrf(IntPtr handle, int vrstartnum, int numes, ref float pfValue);

/*************************************************************
Description:    //VRINT读取， 必须150401以上版本才支持VRINT的DIRECTCOMMAND读取
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetVrInt(ZMC_HANDLE handle, int vrstartnum, int numes, int *piValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVrInt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetVrInt(IntPtr handle, int vrstartnum, int numes, ref int piValue);

/*************************************************************
Description:    //写table 
Input:          //卡链接handle  轴号， 距离
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_SetTable(ZMC_HANDLE handle,int tabstart, int numes, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetTable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_SetTable(IntPtr handle, int vrstartnum, int numes,  float[] pfValue);

/*************************************************************
Description:    //table读取, 可以一次读取多个 
Input:          //卡链接handle  
Output:         //pfValue  多个时必须分配空间.
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Direct_GetTable(ZMC_HANDLE handle, int tabstart, int numes, float *pfValue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetTable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Direct_GetTable(IntPtr handle, int tabstart, int numes, ref float pfValue);

/*************************************************************
Description:    //字符串转为float
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_TransStringtoFloat(const char* pstringin, int inumes,  float* pfvlaue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_TransStringtoFloat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_TransStringtoFloat(string pstringin, int inumes, ref float pfValue);

/*************************************************************
Description:    //字符串转为int
Input:          //卡链接handle  
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_TransStringtoInt(const char* pstringin, int inumes,  int* pivlaue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_TransStringtoInt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_TransStringtoInt(string pstringin, int inumes, ref int pfValue);

/*************************************************************
Description:    //把float格式的变量列表存储到文件， 与控制器的U盘文件格式一致.
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_WriteUFile(const char *sFilename, float *pVarlist, int inum);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_WriteUFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_WriteUFile(string sFilename,  float[] pVarlist, int inum);

/*************************************************************
Description:    //读取float格式的变量列表， 与控制器的U盘文件格式一致.
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_ReadUFile(const char *sFilename, float *pVarlist, int* pinum);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_ReadUFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_ReadUFile(string sFilename, ref float pVarlist, ref int inum);

/*************************************************************
Description:    //modbus寄存器操作
Input:          //卡链接handle 寄存器地址
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Modbus_Set0x(ZMC_HANDLE handle, uint16 start, uint16 inum, uint8* pdata);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set0x", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Modbus_Set0x(IntPtr handle, UInt16 start, UInt16 inum, byte[] pdata);

/*************************************************************
Description:    //modbus寄存器操作
Input:          //卡链接handle 寄存器地址
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Modbus_Get0x(ZMC_HANDLE handle, uint16 start, uint16 inum, uint8* pdata);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get0x", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Modbus_Get0x(IntPtr handle, UInt16 start, UInt16 inum, byte[] pdata);


/*************************************************************
Description:    //modbus_ieee
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Modbus_Get4x_Float(ZMC_HANDLE handle, uint16 start, uint16 inum, float* pfdata)
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get4x_Float", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Modbus_Get4x_Float(IntPtr handle, UInt16 start, UInt16 inum, ref float pfdata);

/*************************************************************
Description:    //modbus_ieee
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Modbus_Set4x_Float(ZMC_HANDLE handle, uint16 start, uint16 inum, float* pfdata)
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set4x_Float", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Modbus_Set4x_Float(IntPtr handle, UInt16 start, UInt16 inum,  float[] pfdata);

/*************************************************************
Description:    //modbus_long
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Modbus_Get4x_Long(ZMC_HANDLE handle, uint16 start, uint16 inum, int32* pidata)
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get4x_Long", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Modbus_Get4x_Long(IntPtr handle, UInt16 start, UInt16 inum, ref Int32 pfdata);

/*************************************************************
Description:    //modbus_long
Input:          //
Output:         //
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_Modbus_Set4x_Long(ZMC_HANDLE handle, uint16 start, uint16 inum, int32* pidata)
[DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set4x_Long", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_Modbus_Set4x_Long(IntPtr handle, UInt16 start, UInt16 inum, Int32[] pfdata);

/*************************************************************
Description:    //写用户flash块, float数据
Input:          //卡链接handle
					uiflashid 	flash块号
					uinumes		变量个数
Output:         //
Return:         //错误码
*************************************************************///
//int32 __stdcall ZAux_FlashWritef(ZMC_HANDLE handle, uint16 uiflashid, uint32 uinumes, float *pfvlue);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_FlashWritef", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_FlashWritef(IntPtr handle, UInt16 uiflashid, UInt32 uinumes, float[] pfvlue);	

/*************************************************************
Description:    //读取用户flash块, float数据
Input:          //卡链接handle
					uiflashid 	flash块号
					uibuffnum	缓冲变量个数
Output:         //
					puinumesread 读取到的变量个数
Return:         //错误码
*************************************************************/
//int32 __stdcall ZAux_FlashReadf(ZMC_HANDLE handle, uint16 uiflashid, uint32 uibuffnum, float *pfvlue, uint32* puinumesread);
[DllImport("zauxdll.dll", EntryPoint = "ZAux_FlashReadf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
public static extern Int32 ZAux_FlashReadf(IntPtr handle, UInt16 uiflashid, UInt32 uibuffnum, float[] pfvlue, ref UInt32 puinumesread);

	}
}
