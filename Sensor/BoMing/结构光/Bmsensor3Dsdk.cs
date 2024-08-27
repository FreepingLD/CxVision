using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Sensor
{
    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct ImageROI
    {
        //ROI偏移，左上角坐标
        public int leftTop_x;
        public int leftTop_y;

        //ROI尺寸（注意：有序点云尺寸即为该参数 height *width）
        public int height;
        public int width;
    };

    //滤波参数，用于高精度版本，速度较慢，精度较好
    [Serializable]
    public struct FilterParas
    {
        /*
        视野大小不同，默认参数不同，针对条纹光传感器500W像素大中小视野三款，默认参数可为：
            meanBlurKernel=3
            medianBlurKernel=9；
            d=15
            sigmaColor=0.1；
            sigmaSpace=9；	
        其余情况可适当调节。后面三个参数为双边滤波所需。
        */

        public int meanBlurKernel;
        public int medianBlurKernel;
        public int d;
        public double sigmaColor;
        public  double sigmaSpace;
    };

    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct AlgoParas
    {
        /*是否使用mask模板.*/
        [MarshalAs(UnmanagedType.I1)]
        public bool maskFlag;

        /*mask模板的阈值，默认为 2.*/
        public double threshold;

        /*需要去除的高度最小值阈值，默认设定【 - 10，10】之间，removalMin需小于removalMax。*/
        public float removalMin;

        /*需要去除的高度最大值阈值，默认设定【 - 10，10】之间，removalMax需大于removalMin。*/
        public float removalMax;

        /*聚类的K值。默认24.*/
        public int meanK;

        /*标准差阈值。默认0.6。*/
        public double stdThresh;

        /*迭代次数, 默认为 1.*/
        public int repeats;

        /*是否进行平滑和滤波，如果 isFiltered = true，则数据重复性精度提高，算法处理速度变慢。该选项为高精度版本。*/
        [MarshalAs(UnmanagedType.I1)]
        public bool isFiltered;

        //是否使用ROI数据。（非常注意：在双投单目时，左右投影算法该参数 isUseROI 与 ROI 参数必须一致）
        [MarshalAs(UnmanagedType.I1)]
        public bool isUseROI;

        //提取的ROI数据。如果 isUseROI 为 false，则该参数不起作用。
        [MarshalAs(UnmanagedType.Struct)]
        public ImageROI ROI;

        //是否修补空洞
        [MarshalAs(UnmanagedType.I1)]
        public bool isFillHole;

        //默认为5，效果相对较好。
        //空洞大小，比如 5*5。如果 isFillHole 为 true，holeMake=5，会增加 250ms 时间 (cpu:i7-8700)，点云成像效果较好。
        public int holeMaker;

        //修补空洞的次数，默认为1，高精度版本默认为2.
        public int fillHoleRepeats;

        //正序倒序，默认正序。 用于决定点云的成像方向
        [MarshalAs(UnmanagedType.I1)]
        public bool isUpDown;

        [MarshalAs(UnmanagedType.I1)]
        public bool enableUserInvalidValue;  // 允许用户定义无效点数值。为true 时下面的 nanValue 才有效

        public float nanValue;              // 设置这个值将使点云无效点由NaN标记为设定的值(默认NaN)
    };

    public struct Bms3DDataXYZ
    {
        /// X 坐标，单位mm。\n X Coordinate in mm.
        public float X;
        /// Y 坐标，单位mm。\n Y Coordinate in mm. 
        public float Y;
        /// Z 坐标，单位mm。\n Z Coordinate in mm. 
        public float Z;
    };

    public struct BmsSystemStatus
    {
        public char PrjStatus;      /*投影仪连接状态 0 表示正常 1 表示第一路连接失败  （2 表示第二路连接失败 3 表示两路都连接失败 【预留】）*/
        public char WorkStatus;     /*0 表示空闲态， 1 表示正在投图  2 表示投图结束*/
        public char Reserved;       /*备用状态位*/
    }

    public struct BmsVersionNumber
    {
        public char Major;
        public char Minor;
        public char Patch;
        public char Reserved; /*保留位 四字节对齐*/
    }
    [Serializable]
    public struct BmsIoOutPara
    {
        /// <summary>
        /// IO 通道。 0： 该io将被配置成通用io功能；1：该io将被特殊io功能：指示忙信号，正在曝光信号
        /// </summary>
        public byte IoType;
        /// <summary>
        /// 输出信号的电平模式， 0： 电平模式；1：脉冲模式
        /// </summary>
        public byte OutType; 
        /// <summary>
        /// 电平模式电平。 0:低电平； 1：高电平
        /// </summary>
        public byte LelDtLevel;
        /// <summary>
        /// 脉冲模式默认电平。 0: 低电平； 1：高电平
        /// </summary>
        public byte PwDtLevel;
        /// <summary>
        /// 脉冲宽度：单位us。  [10us-65ms]
        /// </summary>
        public Int16 PulseWidth;
        public byte Reserved; /*保留位*/
    }
    public enum enMvGvspPixelType
    {
        enPixelType
    }

    public enum enBmsDeviceType
    {
        SingleProjector = 1,   /*单投*/
        DoubleProjector = 2,   /*双投*/

        TripleProjector = 3,    // 三投
        QuadrupleProjector = 4   //四投
    }

    public struct MV_FRAME_OUT_INFO
    {
        public Int16 nWidth;             // ch:图像宽 | en:Image Width
        public Int16 nHeight;            // ch:图像高 | en:Image Height
        public enMvGvspPixelType     enPixelType;        // ch:像素格式 | en:Pixel Type

        public int nFrameNum;          // ch:帧号 | en:Frame Number
        public int nDevTimeStampHigh;  // ch:时间戳高32位 | en:Timestamp high 32 bits
        public int nDevTimeStampLow;   // ch:时间戳低32位 | en:Timestamp low 32 bits
        public int nReserved0;         // ch:保留，8字节对齐 | en:Reserved, 8-byte aligned
        public Int64 nHostTimeStamp;     // ch:主机生成的时间戳 | en:Host-generated timestamp
        public int nFrameLen;
        public int nLostPacket;  // 本帧丢包数
        public int nReserved;
    }
    public enum enBmsTriggerMode
    {
        NoTrigger = 0x0, /*无触发*/
        SoftwareTrigger = 0x1, /*软件触发*/
        HardwareTriggerOne = 0x2, /*硬件触发1*/
        HardwareTriggerTwo = 0x3, /*硬件触发2*/
        HardWareTriggerOneAndTwo = 0x4  /*硬件1 和 2 同时触发*/
    }
    public  enum enBmsGernalIoOutType
    {
        OutLowLevel = 0, /*输出低电平*/
        OutHighLeve = 1, /*输出高电平*/
        OutPulseWidth = 2 /*输出脉宽*/
    }
    public enum enBmsConnectEventType
    {
        EventCtlerConnect = 1, /*控制器连接*/
        EventCamConnectU2 = 2, /*相机异常连接（USB2.0）*/
        EventCamConnectU3 = 3, /*相机正常连接（USB3.0）*/
        EventCtlerNoConnect = 4, /*控制器未连接*/
        EventCamNoConnect = 5, /*相机未连接*/
        EventCamConnect =6     // 相机连接
        //EventDevConnect = 1, /*设备连接*/
        //EventCamConnect = 2, /*相机连接*/
        //EventDevNoConnect = 3, /*设备未连接*/
        //EventCamNoConnect = 4 /*相机未连接*/
    }
   public enum enBmsWorkModeType
    {
        WorkMode2D = 1,  /*2D预览模式*/
        WorkMode3D = 2   /*3D点云模式*/
    }

    #region Native 方法导出
    public class Bmsensor3Dsdk
    {
        /// <summary>
        /// 创建bmssdk sensor对象
        /// </summary>
        /// <returns>返回bmssdk sensor指针</returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr BmsSDK_Create();

        /// <summary>
        /// 删除BmsSDK
        /// </summary>
        /// <param name="pBmsSDK"></param>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void BmsSDK_Delete(IntPtr pBmsSDK);

        /// <summary>
        /// 打开控制器  
        /// </summary>
        /// <param name="pBmsSDK"></param>输入参数，BmsSDK 指针
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenController(IntPtr pBmsSDK);

        /// <summary>
        /// 关闭控制器
        /// </summary>pBmsSDK：输入参数，BmsSDK 指针
        /// <param name="pBmsSDK"></param>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void CloseController(IntPtr pBmsSDK);

        /// <summary>
        /// 设置USB连接事件回调函数
        /// </summary>
        /// <param name="pBmsSDK"></param>输入参数，BmsSDK 指针。
        /// <param name="pFnCB"></param>输出参数，函数指针
        /// <param name="pPara"></param>输出参数，回调指针
        /// <param name="isUnicode"></param>应用程序是否unicde编码
        /// <returns></returns>返回TRUE 成功， FALSE失败
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetUsbConnectStatusCallBack(IntPtr pBmsSDK, BmsCallBackConnectEventPtr pFnCB, IntPtr pPara, bool isUnicode);

        public delegate void BmsCallBackConnectEventPtr(IntPtr pPara, enBmsConnectEventType ConnectEvent);

        /// <summary>
        /// 选择设备类型，这个函数必须在打开设备前最先被调用
        /// </summary>
        /// <param name="pBmsSDK"><pBmsSDK：输入参数，BmsSDK 指针/param>
        /// <param name="DevType"><DevType：输人参数，参考结构体BmsDeviceType定义/param>
        /// <returns><返回TRUE 成功， FALSE失败/returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SelectDeviceType(IntPtr pBmsSDK, enBmsDeviceType DevType);


        /// <summary>
        /// 获取系统状态。
        /// </summary>
        /// <param name="pBmsSDK">SysStatus：输出参数，参考结构体BmsSystemStatus定义。</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetSystemStatus(IntPtr pBmsSDK, out BmsSystemStatus SysStatus);

        /// <summary>
        /// 清除系统状态。
        /// </summary>
        /// <param name="pBmsSDK">SysStatus：输出参数，参考结构体BmsSystemStatus定义。</param>
        /// <returns></returns>

        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClearSystemStatus(IntPtr pBmsSDK);


        /// <summary>
        /// 获取丢失触发计数。
        /// </summary>
        /// <param name="pBmsSDK">SysStatus：输出参数，参考结构体BmsSystemStatus定义。</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetLossTriggerNumber(IntPtr pBmsSDK, out Int16 TriNum);

        /// <summary>
        /// 清除丢失触发计数。
        /// </summary>
        /// <param name="pBmsSDK">SysStatus：输出参数，参考结构体BmsSystemStatus定义。</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClearLossTriggerNumber(IntPtr pBmsSDK);

        /// <summary>
        /// 获取产品型号。
        /// </summary>
        ///  <param name="pBmsSDK">
        /// <param name="ProModel">输出参数，6位字节的字符产品型号
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetProductModel(IntPtr pBmsSDK, [Out] char[] ProModel);

        /// <summary>
        /// 获取设备版本号。
        /// </summary>VerInfo： 输出参数，参考结构体BmsVersionNumber定义。
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetDeviceVersion(IntPtr pBmsSDK, out BmsVersionNumber VerInfo);

        /// <summary>
        /// 获取产品序列号。
        /// </summary>VerInfo： 输入参数，9位字节的字符产品型号
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetProductSerialNumber(IntPtr pBmsSDK, [Out] char[] SerialNum);


        /// <summary>
        /// 获取投影仪曝光周期帧周期设置状态
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="SerialNum"><Status:输出参数， 0 状态OK， 1 正在初始化系列中/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetPrjTimeSetStatus(IntPtr pBmsSDK, [Out] char SerialNum);


        /// <summary>
        /// 启动相机重传
        /// </summary>
        /// <param name="pBmsSDK"><返回TRUE 成功， FALSE失败/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartCameraRetrans(IntPtr pBmsSDK);

        /// <summary>
        /// 设置led亮度
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="Brightness"><输入参数，亮度值范围 [0,200]/param>
        /// <param name="PrjChn"><输入参数，投影仪通道 1 CH1  2 CH2/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetLEDBrightness(IntPtr pBmsSDK, [In] byte Brightness, [In] byte PrjChn);


        /// <summary>
        /// 获取led亮度值
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="Brightness"><输出参数，亮度值范围 [0,200]/param>
        /// <param name="PrjChn"><输入参数，投影仪通道 1 CH1  2 CH2。/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetLEDBrightness(IntPtr pBmsSDK, out byte Brightness, [In] byte PrjChn);

        /// <summary>
        /// 设置触发模式
        /// </summary>TriType：输入参数，触发类型参考枚举BmsTriggerMode定义。
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetTriggerMode(IntPtr pBmsSDK, enBmsTriggerMode TriType);

        /// <summary>
        /// 获取触发模式
        /// </summary>TriType：输入参数，触发类型参考枚举BmsTriggerMode定义。
        /// <param name="pBmsSDK">
        /// <returns></returns>

        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetTriggerMode(IntPtr pBmsSDK, out enBmsTriggerMode TriType);

        /// <summary>
        /// 启动触发
        /// </summary>
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartTrigger(IntPtr pBmsSDK);

        /// <summary>
        /// 同步方式启动触发.调用此函数触发后，将会一直等待直到采集到足够投影图像才会返回。如果失败或超时，此函数将会空指针。
        /// </summary>
        /// <param name="pBmsSDK">
        /// <returns>返回排序后的图像的内存指针，可传入  ComputeWorldPointXYZN() 或 ComputeDeepMap() 函数用于生成点云/深度图。如果失败返回NULL。</returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr StartTriggerSync(IntPtr pBmsSDK);

        /// <summary>
        /// 设置硬件触发模式
        /// </summary>chn：输入参数， 0: 硬件通道1;   1:硬件通道2。
        /// </summary>mode：输入参数，0: 上升沿触发;   1: 下降沿触发。
        /// <param name="pBmsSDK">
        /// <returns></returns>

        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetHardwareTriggerMode(IntPtr pBmsSDK, byte chn, byte mode);

        /// <summary>
        /// 获取硬件触发模式
        /// </summary>chn：输入参数， 0 硬件通道1   1硬件通道2。
        /// </summary>mode：输入参数，0 上升沿触发   1 下降沿触发。
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetHardwareTriggerMode(IntPtr pBmsSDK, byte chn, out byte mode);

        /// <summary>
        /// 设置输出IO参数。
        /// </summary>
        /// <param name="pBmsSDK">设备指针</param>
        /// <param name="chn">通道。1：通道1； 2: 通道2</param>
        /// <param name="OutPara">参见BmsIoOutPara定义</param>
        /// <returns>如果IO1 配置成特殊IO 系统检测到系统忙  IO1就会输出忙信号。如果IO2 配置成特殊IO 系统检测到正在曝光  IO2就会输出正在曝光信号。</returns>
        /// 通用IO输出需要手动调用StartGernalIoOut接口 去控制输出。
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetIoOutPara(IntPtr pBmsSDK, byte chn, BmsIoOutPara OutPara);

        /// <summary>
        /// 获取输出IO参数
        /// </summary>IoChn：输入参数， 输出通道1   1 输出通道2。
        /// </summary>OutPara：输入参数，IO输出参数参考结构体BmsIoOutPara定义。
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetIoOutPara(IntPtr pBmsSDK, byte chn, out BmsIoOutPara OutPara);

        /// <summary>
        /// 启动通用IO输出
        /// </summary>IoChn：输入参数， 输出通道1   1 输出通道2。
        /// </summary>OutType：输入参数，通用IO输出类型参考枚举BmsGernalIoOutType定义。
        /// <param name="pBmsSDK">
        /// <returns></returns>

        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartGernalIoOut(IntPtr pBmsSDK, byte chn, enBmsGernalIoOutType OutPara);


        /// <summary>
        /// 加载本地系统标定系数，双投SD系列设备适用
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="paths">标定文件路径，多投时有多个</param>
        /// <param name="height">标定文件高，一般是相机最大分辨率</param>
        /// <param name="width">标定文件宽，一般是相机最大分辨率</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll",  CharSet =CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "LoadPhaseCoefficientsN")]
        public static extern bool LoadPhaseCoefficientsN_Native(IntPtr pBmsSDK, string[] paths, int count, int height, int width,int encode);
        public static bool LoadPhaseCoefficientsN(IntPtr pBmsSDK, string[] paths, int count, int height, int width)
        {
            return LoadPhaseCoefficientsN_Native(pBmsSDK, paths, count, height, width, 1);
        }


        /// <summary>
        /// 加载本地相机标定文件
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="filename">标定文件路径</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "LoadCalibrationFile")]
        public static extern bool LoadCalibrationFile_Native(IntPtr pBmsSDK, string filename, int encode);
        public static bool LoadCalibrationFile(IntPtr pBmsSDK, string filename)
        {
            return LoadCalibrationFile_Native(pBmsSDK, filename, 1);
        }

        /// <summary>
        /// 加载标定基地文件
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="paths">标定文件路径，多投时有多个</param>
        /// <param name="count">设备类型。1：SS设备； 2：SD设备; 3: ST设备；4：SQ色号被</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "LoadBaseMatFileN")]
        public static extern bool LoadBaseMatFileN_Native(IntPtr pBmsSDK, string[] paths, int coun, int encodet);
        public static bool LoadBaseMatFileN(IntPtr pBmsSDK, string[] paths, int count)
        {
            return LoadBaseMatFileN_Native(pBmsSDK, paths, count,1);
        }


        /// <summary>
        /// 获取点云
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="pSortData">排序过的2D图像，从回调函数中得到</param>
        /// <param name="height">2D 图像高</param>
        /// <param name="width">>2d 图像宽</param>
        /// <param name="frequency">频率</param>
        /// <param name="isDownSampling">是否下采样</param>
        /// <param name="aloParas">算法参数</param>
        /// <param name="count">设备类型,1:单投 SSXXX系列设备； 2: 双投 SDXXX系列设备；3：三投，STXXX系列设备</param>
        /// <param name="pointsXYZ">返回深度图/高度图数据。如果是多投SD,ST,SQ设备，指的是融合后的点云</param>
        /// <param name="pointsXYZSingle">返回独立点云，对于单投SS设备，实际就是 pointsXYZ。对于多图SD,ST,SQ设备，指的是融合前各个投影自己独立的点云</param>
        /// <param name="isHighPrecision">是否启用高精度参数</param>
        /// <param name="filterParas">高精度参数，isHighPrecision为true时才有效</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ComputeWorldPointXYZN(IntPtr pBmsSDK, IntPtr pSortData, int height, int width, int frequency, bool isDownSampling, AlgoParas[] aloParas, int count, [Out] Bms3DDataXYZ[] pointsXYZ, [Out] Bms3DDataXYZ[] pointsXYZSingle, bool isHighPrecision, ref FilterParas filterParas);

        /// <summary>
        /// 计算世界坐标系系数。
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="coordinateZ">输入点云Z值</param>
        /// <param name="countSize">输入点云个数</param>
        /// <param name="coeff">输出点云世界坐标系系数</param>
        /// <param name="index">输入点云 是第几副点云，0代表第一副，1第二副，2第三副(投影索引)</param>
        /// <returns>返回true是加载成功，false是加载失败。</returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool CalcWorldCoeff(IntPtr pBmsSDK, [In] float[] coordinateZ, int countSize, [Out] float[] coeff, int index);

        /// <summary>
        /// 计算世界坐标系系数。
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="coordinateZ">输入点云Z值</param>
        /// <param name="countSize">输入点云个数</param>
        /// <param name="coeff">输出点云世界坐标系系数</param>
        /// <param name="index">输入点云 是第几副点云，0代表第一副，1第二副，2第三副(投影索引)</param>
        /// <returns>返回true是加载成功，false是加载失败。</returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool CalcWorldCoeff(IntPtr pBmsSDK, [In] IntPtr coordinateZ, int countSize, [Out] IntPtr coeff, int index);

        /// <summary>
        /// 获取深度图/高度图
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="pSortData">排序过的2D图像，从回调函数中得到</param>
        /// <param name="height">2D 图像高</param>
        /// <param name="width">2d 图像宽</param>
        /// <param name="frequency">频率</param>
        /// <param name="isDownSampling">是否下采样</param>
        /// <param name="aloParas">算法参数</param>
        /// <param name="count">设备类型,1:单投 SSXXX系列设备； 2: 双投 SDXXX系列设备；3：三投，STXXX系列设备</param>
        /// <param name="deepMap">返回深度图/高度图数据，单位 um</param>
        /// <param name="isFiltered">是否启动高精度参数</param>
        /// <param name="filterParas">高精度参数，isFiltered参数为true时才有效</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void ComputeDeepMap(IntPtr pBmsSDK, IntPtr pSortData, int height, int width, int frequency, bool isDownSampling, AlgoParas[] aloParas, int count, ushort[] deepMap, bool isFiltered, ref FilterParas filterParas);

        /// <summary>
        /// 获取深度图/高度图
        /// </summary>
        /// <param name="pBmsSDK">设备句柄</param>
        /// <param name="pSortData">排序过的2D图像，从回调函数中得到</param>
        /// <param name="height">2D 图像高</param>
        /// <param name="width">2d 图像宽</param>
        /// <param name="frequency">频率</param>
        /// <param name="isDownSampling">是否下采样</param>
        /// <param name="aloParas">算法参数</param>
        /// <param name="count">设备类型,1:单投 SSXXX系列设备； 2: 双投 SDXXX系列设备；3：三投，STXXX系列设备</param>
        /// <param name="deepMap">返回深度图/高度图数据</param>
        /// <param name="isFiltered">是否启动高精度参数</param>
        /// <param name="filterParas">高精度参数，isFiltered参数为true时才有效</param>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void ComputeDeepMap(IntPtr pBmsSDK, IntPtr pSortData, int height, int width, int frequency, bool isDownSampling, AlgoParas[] aloParas, int count, IntPtr deepMap, bool isFiltered, ref FilterParas filterParas);

        /// <summary>
        /// 保存pcd点云文件
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="filename"><保存文件路径/param>
        /// <param name="pointsXYZ"><需要保存的数据/param>
        /// <param name="height">相机分辨率 高</param>
        /// <param name="width"><相机分辨率 宽/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SavePCDFile(IntPtr pBmsSDK, string filename, Bms3DDataXYZ[] pointsXYZ, int height, int width);

        /// <summary>
        /// 加载pcd点云文件
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="path"><pcd文件路径/param>
        /// <param name="pointsXYZ"><加载后的数据/param>
        /// <param name="length"><一般默认为相机 height * width/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool LoadPCDFile(IntPtr pBmsSDK, string path, [Out] Bms3DDataXYZ[] pointsXYZ, ulong length);

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="pBmsSDK">设备指针</param>
        /// <param name="ConnectType">返回当前连接的usb协议。 2： usb2.0； 3：usb3.0</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenCamera(IntPtr pBmsSDK, out byte ConnectType);

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool CloseCamera(IntPtr pBmsSDK);

        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetSortImageDataCallBack(IntPtr pBmsSDK, BmsCallBackSortImageDataPtr pFnCB, IntPtr pPara);
        public delegate void BmsCallBackSortImageDataPtr(IntPtr pData, MV_FRAME_OUT_INFO pFrameInfo, IntPtr pSortData, IntPtr pUser);


        /// <summary>
        /// 清除相机采集数据缓存
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClearImageDataBuffer(IntPtr pBmsSDK);


        /// <summary>
        /// 设置工作模式
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="WrokMode"><模式类型 参考枚举 BmsWorkModeType 定义/param>
        /// <param name="hwnd"><如果需要实时显示就传入窗口指针, hwnd = 0 即不实时显示/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetWorkMode(IntPtr pBmsSDK, enBmsWorkModeType WrokMode, IntPtr hwnd);


        /// <summary>
        /// 获取工作模式
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="WrokMode"><模式类型 参考枚举 BmsWorkModeType 定义/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetWorkMode(IntPtr pBmsSDK, out enBmsWorkModeType WrokMode);

        /// <summary>
        /// 设置相机曝光时间
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="ExposureTime"><2字节曝光时间(单位ms)/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetCameraExposureTime(IntPtr pBmsSDK, Int16 ExposureTime);

        /// <summary>
        /// 获取相机曝光时间
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="ExposureTime"><2字节曝光时间(单位ms)/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetCameraExposureTime(IntPtr pBmsSDK, out Int16 ExposureTime);

        /// <summary>
        /// 设置相机增益
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="Gain"><4字节相机增益/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetCameraGain(IntPtr pBmsSDK, float Gain);

        /// <summary>
        /// 获取相机增益
        /// </summary>
        /// <param name="pBmsSDK"></param>
        /// <param name="Gain"><4字节相机增益/param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetCameraGain(IntPtr pBmsSDK, out float Gain);

        /// <summary>
        /// 保存12张点云的原始图片
        /// <param name="pBmsSDK">
        /// <returns></returns>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SavePointCloudBitMap(IntPtr pBmsSDK, string path, Int16 width, Int16 higth);

        /// <summary>
        /// 获取操作指令结果
        /// </summary>
        /// <param name="in_pDev">设备句柄</param>
        /// <param name="in_cmdCode">指令码，参见 CmdCode_SSSensor 枚举</param>
        /// <param name="in_out_pCmdData">指令数据</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", EntryPoint = "get")]
        public extern static bool get(IntPtr in_pDev, enCmdCode_SSSensor in_cmdCode, IntPtr out_pCmdData);

        /// <summary>
        /// 获取操作指令结果
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <param name="in_cmdCode">指令码，参见 CmdCode_SSSensor 枚举</param>
        /// <param name="in_structureType">数据类型</param>
        /// <param name="out_structure">返回的数据</param>
        /// <returns></returns>
        public static bool Get(IntPtr in_pDev, enCmdCode_SSSensor in_cmdCode, Type in_structureType, ref object out_structure)
        {
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(in_structureType));
            bool ok = Bmsensor3Dsdk.get(in_pDev, in_cmdCode, p);

            out_structure = Marshal.PtrToStructure(p, in_structureType);
            Marshal.FreeHGlobal(p);

            return ok;
        }


        [DllImport("SsAPI.dll", EntryPoint = "set")]
        public extern static bool set(IntPtr in_pDev, enCmdCode_SSSensor in_cmdCode, IntPtr in_pCmdData);

        public static bool Set(IntPtr in_pDev, enCmdCode_SSSensor in_cmdCode, Type in_structureType, object in_structure = null)
        {
            bool ok = false;
            if (in_structure != null)
            {
                IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(in_structureType));
                Marshal.StructureToPtr(in_structureType, p, true);
                ok = Bmsensor3Dsdk.set(in_pDev, in_cmdCode, p);
                Marshal.FreeHGlobal(p);
            }
            else // 没有 data 字段被set下去！
            {
                ok = Bmsensor3Dsdk.set(in_pDev, in_cmdCode, IntPtr.Zero);
            }
            return ok;
        }

        public static bool Set(IntPtr in_pDev, enCmdCode_SSSensor in_cmdCode, object in_structure = null)
        {
            bool ok = false;
            if (in_structure != null)
            {
                IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(in_structure));
                Marshal.StructureToPtr(in_structure, p, true);
                ok = Bmsensor3Dsdk.set(in_pDev, in_cmdCode, p);
                Marshal.FreeHGlobal(p);
            }
            else // 没有 data 字段被set下去！
            {
                ok = Bmsensor3Dsdk.set(in_pDev, in_cmdCode, IntPtr.Zero);
            }
            return ok;
        }

        /// <summary>
        /// 设置相机 ROI， 设置后，算法参数的roi要同步；可以针对指定区域生成点云，加快3D扫描速度
        /// </summary>
        /// <param name="in_pDev"></param>
        /// <param name="roi">roi 参数</param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", EntryPoint = "SetCameraROI")]
        public extern static bool SetCameraROI(IntPtr in_pDev, ImageROI roi);

        /// <summary>
        /// 关闭 roi，恢复最大视野
        /// </summary>
        /// <param name="in_pDev"></param>
        /// <returns></returns>
        [DllImport("SsAPI.dll", EntryPoint = "StopCameraROI")]
        public extern static bool StopCameraROI(IntPtr in_pDev);

        #region 已经弃用的接口，将在后续的版本删除
        /// <summary>
        /// 该接口已经弃用，功能已内部自动实现，调用该接口将不会有任何效果
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetPrjExposure_FramePeriod(IntPtr pBmsSDK, Int16 exposurePeriod, Int16 framePeriod, byte PrjChn);

        /// <summary>
        /// 该接口已经弃用，功能已内部自动实现，调用该接口将不会有任何效果
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetPrjExposure_FramePeriod(IntPtr pBmsSDK, out Int16 exposurePeriod, out Int16 framePeriod, byte PrjChn);

        /// <summary>
        /// 此接口函数在新的版本已弃用，请使用 get()/set()接口
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static bool cmd(IntPtr in_pDev, enCmdCode_SSSensor in_cmdCode, IntPtr in_out_pCmdData);

        /// <summary>
        /// 该接口已经弃用，请使用 LoadPhaseCoefficientsN() 接口替代。
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool LoadPhaseCoefficients2(IntPtr pBmsSDK, string path1, string path2, int height, int width);

        /// <summary>
        /// 该接口已经弃用，请使用 LoadPhaseCoefficientsN() 接口替代。
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool LoadPhaseCoefficients(IntPtr pBmsSDK, string path, int height, int width);

        /// <summary>
        /// 该接口已经弃用，请使用 LoadBaseMatFileN() 接口替代。
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool LoadBaseMatFile(IntPtr pBmsSDK, string filename);

        /// <summary>
        /// 该接口已经弃用，请使用 LoadBaseMatFileN() 接口替代。
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool LoadBaseMatFile2(IntPtr pBmsSDK, string Base_L_path, string Base_R_path);

        /// <summary>
        /// 该接口已弃用，请使用 ComputeWorldPointXYZN() 替代.
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ComputeWorldPointXYZ(IntPtr pBmsSDK, int height, int width, int frequency, bool isDownSampling, ref AlgoParas aloParas, [Out] Bms3DDataXYZ[] pointsXYZ);


        /// <summary>
        /// 该接口已弃用，请使用 ComputeWorldPointXYZN() 替代.
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ComputeWorldPointXYZ2(IntPtr pBmsSDK, int height, int width, int frequency, bool isDownSampling, ref AlgoParas algoParas_L, ref AlgoParas algoParas_R, [Out] Bms3DDataXYZ[] pointsXYZ);

        /// <summary>
        /// 该接口已经弃用，调用该接口将不会有任何效果
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SaveDataToBitMap(IntPtr pBmsSDK, byte[] pBuf, string pPath, Int16 width, Int16 higth);


        /// <summary>
        /// 该接口已经弃用，请使用 SetSortImageDataCallBack() 替代。
        /// </summary>
        [DllImport("SsAPI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetImageDataCallBack(IntPtr pBmsSDK, BmsCallBackImageDataPtr pFnCB, IntPtr pPara);
        public delegate void BmsCallBackImageDataPtr(IntPtr pData, MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser);
        #endregion
    }

    #endregion

    #region 枚举，数据结构
    /// <summary>
    /// 指令码，get()/set()操作的参数
    /// </summary>
    public enum enCmdCode_SSSensor
    {
        Start = 0,
        MirrorFlip = 1,             // 镜像翻转，数据结构为 SSSensor_MirrorFlip_s
        CameraSoftTrig = 2,           // 软触发一次相机输出一帧图像, 设备必须工作在软件触发模式. 该指令没有数据.
        Reserved1 = 3,               //
        CaptureStatus = 4,            // 设置预览状态，2D模式有效，数据结构为 SSSensor_CaptureStatus_s
        RingLampBrightness = 5        // 设置环形光亮度

    };

    /// <summary>
    /// 镜像翻转操作的数据参数
    /// </summary>
    public struct enSSSensor_MirrorFlip_s
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool reverseX;

        [MarshalAs(UnmanagedType.I1)]
        public bool reverseY;
    }

    /// <summary>
    /// 2D 采集，预览状态
    /// </summary>
    public enum enSSSensor_CaptureStatus_e
    {
        Running, // 预览
        Stopped, // 停止预览
    };

    /// <summary>
    ///  2D 预览状态参数
    /// </summary>
    public struct enSSSensor_CaptureStatus_s
    {
        public enSSSensor_CaptureStatus_e value;
    };

    ///
    /// \brief   "SSSensor_RingLampBrightness"指令数据结构
    ///
    public struct enSSSensor_RingLampBrightness_s
    {
        public int value;
    };
    #endregion

}
