using System;
using System.Runtime.InteropServices;

namespace Sensor
{
    #region SCLib Native 方法
    /// <summary>
    /// SCLib 库中的native方法声明
    /// </summary>
    public class SCDev_Native
    {
        /// <summary>
        /// 创建一个设备,使用完后调用 SCDev_Delete()函数删除设备
        /// </summary>
        /// <returns>返回设备指针</returns>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_Create")]
        public extern static IntPtr Create();

        /// <summary>
        /// 删除一个设备,与SCDev_Create 配对使用
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_Delete")]
        public extern static void Delete(IntPtr in_pDev);

        /// <summary>
        /// 获取设备最后一次错误码,非设备错误码，请参考错误日志
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <returns>返回错误码，参见 DeviceError 枚举</returns>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_GetDeviceLastError")]
        public extern static enDeviceError GetDeviceLastError(IntPtr in_pDev);

        /// <summary>
        /// 打开设备.设备使用完毕，应当调用close()函数关闭设备，释放资源。在进行设备操作前应当调用此接口打开设备。
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <param name="in_pConnectionSetting">指定设备连接时的配置，参见ConnectionSetting_s结构体.</param>
        /// <param name="in_connectionOption">对于网口，是打开时连接的超时时间，默认5000ms；对于串口，此参数保留；</param>
        /// <returns>成功返回true，失败返回false</returns>
        [DllImport("SCLib.dll", CharSet =CharSet.Ansi, EntryPoint = "SCDev_Open")]
        public extern static bool Open(IntPtr in_pDev, IntPtr in_pConnectionSetting, int in_connectionOption);

        /// <summary>
        /// 关闭设备.此操作将强行终止设备连接；请确保执行此操作前没有正在进行的数据传输动作。
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <returns>成功返回true，失败返回false</returns>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_Close")]
        public extern static bool Close(IntPtr in_pDev);

        /// <summary>
        /// 设置 get/set 操作的超时返回时间
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <param name="in_setTimeout"> set 操作的超时返回时间,如果为负数，则使用上一次设置的值。</param>
        /// <param name="in_getTimeout">get操作的超时返回时间,如果为负数，则使用上一次设置的值。</param>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_SetTimeout")]
        public extern static void SetTimeout(IntPtr in_pDev, int in_setTimeout, int in_getTimeout);

        /// <summary>
        /// 设置设备属性.注意，一些属性的设置要在设置传感器采集动作启动或停止状态才可以.
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <param name="in_propId">要设置的source属性索引，参见 PropId 枚举</param>
        /// <param name="in_pPropData">要设置的属性的数据结构体指针，根据PropId的不同而不同；有些属性不需要提供数据，则此参数应为NULL.</param>
        /// <returns>成功返回true，失败返回false</returns>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_SetProperty")]
        public extern static bool SetProperty(IntPtr in_pDev, enPropId in_propId, IntPtr in_pPropData);

        /// <summary>
        /// 获取设备属性.注意，一些属性的设置要在设置传感器采集动作启动或停止状态才可以.
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <param name="in_propId">要获取的source属性索引，参见 PropId 枚举</param>
        /// <param name="out_pPropData">要获取的属性的数据结构体指针，根据PropId的不同而不同；</param>
        /// <returns>成功返回true，失败返回false</returns>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_GetProperty")]
        public extern static bool GetProperty(IntPtr in_pDev, enPropId in_propId, IntPtr out_pPropData);

        /// <summary>
        /// 获取数据流buffer中数据的大小,此函数与getStreamBuffer()配读使用；调用后应当再调用getStreamBuffer()函数获取 buffer中的数据
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <param name="in_propId">要获取的属性的索引，必须为 PropId::StoredMeasureValues 枚举</param>
        /// <param name="out_pPropLen">返回实际获取到的数据个数,由具体 in_propId 指定，不一定是字节单位。</param>
        /// <param name="in_errTimeout">当发生错误时，指定错误状态的阻塞超时时间，如果为负数，则默认为3000ms</param>
        /// <returns>成功返回true，失败返回false。</returns>
        [DllImport("SCLib.dll", EntryPoint = "SCDev_GetStreamBufferLen")]
        public extern static bool GetStreamBufferLen(IntPtr in_pDev, enPropId in_propId,  ref int out_pPropLen, int in_errTimeout);

        /// <summary>
        /// 获取数据流buffer中数据,此函数与getStreamBufferLen()配读使用；调用前应当先调用getStreamBufferLen()函数获取 buffer中的大小
        /// </summary>
        /// <param name="in_pDev">设备指针</param>
        /// <param name="in_propId">要获取的属性的索引，必须为 PropId::StoredMeasureValues 枚举</param>
        /// <param name="out_pPropData">返回指定获取到的 buffer数据</param>
        /// <param name="in_propLen">要获取的buffer数据的个数,由具体 in_propId 指定，不一定是字节单位。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        //[DllImport("SCLib.dll", EntryPoint = "SCDev_GetStreamBuffer")]
        //public extern static bool GetStreamBuffer(IntPtr in_pDev, PropId in_propId, IntPtr out_pPropData, int in_propLen);

        [DllImport("SCLib.dll", EntryPoint = "SCDev_GetStreamBuffer")]
        public extern static bool GetStreamBuffer(IntPtr in_pDev, enPropId in_propId,IntPtr out_pPropData, int in_propLen);
    }
    #endregion

    #region SCLib Native 枚举
    /// <summary>
    /// 指定设备的连接方式；在调用 open() 接口时可作为参数传入
    /// </summary>
    public enum enConnectionType : int
    {
        Net,  // 软件与设备建立网络连接
        Uart, // 软件与设备串口建立串口连接
    }

    /// <summary>
    /// 属性ID，在调用property()和setProperty()接口时可以作为参数传入
    /// </summary>
    public enum enPropId : byte
    {
        SensorStatus = 0x01,                  // 启动/停止传感器采集，参见 SensorStauts_s 结构体
        StoredMeasureValues = 0x04,           // 获取缓存区测量值
        StoredMeasureValuesCount = 0x05,      // 设置/获取缓存的计数值 ， 参见 StoredMeasureValuesCount_s 结构体
        ClearStoredMeasureValuesCount = 0x07, // 清除缓存的计数值
        GSM = 0x0A,                           // 获取重心，强度，测量值， gravity, strength, measure value ， 参见 GSM_s 结构体
        SaveSettings = 0x30,                  // 保存设置,保存设置功能是将参数保存在FLASH上固化
        ResetSettingsNoSave = 0x31,           // 恢复出厂设置,不会固化到flash
        RAMSettings = 0x32,                   // 从RAM获取参数，参见 RAMSettings_s 结构体
        MeasureMode = 0x33,                   // 设置测量模式， 参见 MeasureMode_s 结构体
        TrigMode = 0x34,                      // 设置触发方式，参见 TrigMode_s 结构体
        TrigPeriod = 0x35,                    // 设置触发周期，参见 TrigPeriod_s 结构体
        ThresholdValue = 0x36,                // 设置阈值，参见 ThresholdValue_s 结构体
        AverageValue = 0x38,                  // 设置平均值，参见 AverageValue_s 结构体
        Exposure = 0x39,                      // 设置曝光， 参见 Exposure_s结构体
        PixelGain = 0x3A,                     // 设置增益PIX_GAIN_CTR, 参见 PixelGain_s 结构体
        PGAGain = 0x3B,                       // 设置增益PGA_GAIN_CTR，PGAGain_s结构体
        RefractiveIndexValue = 0x3D,          // 设置自定义折射率, 参见 RefractiveIndexValue_s 结构体
        MakeZero = 0x3E,                      // 设置归零
        LedValue = 0x3F,                      // 设置LED亮度 ，参见 LedValue_s 结构体
        FirstPeakMode = 0x40,                 // 设置第一峰模式 ，参见 FirstPeakMode_s 结构体
        InertiaAverage = 0x41,                // 设置惯性平均值， 参见 InertiaAverage_s 结构体
        FirmwareVersion = 0xE1,               // 获取固件版本号，参见 FirmwareVersion_s结构体
        LcdPageIndex = 0xE2,                  // 液晶屏页面选择, 参见 LcdPageIndex_s 结构体
        DeviceName = 0xE3,                    // 获取设备型号，参见 DeviceName_s结构体
        SerialNumber = 0xE5,                  // 获取设备序列号，参见 SerialNumber_s结构体
        KeyStatus = 0xE6,                     // 获取按键状态，参见 KeyStatus_s 结构体
        RefractiveIndexMode = 0x42,           // 设置折射率模式， 参见 RefractiveIndexMode_s 结构体
        MeasureHeaderName = 0xEA,             // 获取测头型号,    参见GetMeasureHeaderName_s 结构体
        MeasureHeaderSerialNumber = 0xEC,     // 获取测头序列号,  参见GetMeasureHeaderSerialNumber_s 结构体
        DeviceIP= 0xED,                       // 设置设备网络连接时的Ip地址，参见DeviceIP_s结构体
        SystemAddOn=0x44,
        AutoConfig=0x46,
        SetCalibrationFactor=0x47,            // 校准系数
        PeakPosition=0x48 ,                   // 波峰参数
        DividerFactor= 0x49,              // 硬件触发输入分频，例如触发脉冲 1KHz， 分频数2，则设备内部其实按照 500Hz 采样输出
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //TODO:添加新的属性支持
        每线点数,
        传感器名称,
        传感器类型,
    }

    /// <summary>
    /// 图像传感器采集状态
    /// </summary>
    public enum enSensorStauts : byte
    {
        Stop = 0x00,
        Start = 0x01
    }

    /// <summary>
    /// 归零状态
    /// </summary>
    public enum enZeroStatus : byte
    {
        Off = 0x00,
        On = 0x01
    }
    

    /// <summary>
    /// 测量模式
    /// </summary>
    public enum enMeasureMode : byte
    {
        Distance = 0x01,
        Thickness = 0x02
    }

    /// <summary>
    /// 触发模式
    /// </summary>
    public enum enTrigMode : byte
    {
        Software = 0x01,
        Hardware = 0x02
    }

    /// <summary>
    /// 第一峰模式
    /// </summary>
    public enum enFirstPeakMode : byte
    {
        Disabled = 0x00,
        Enabled = 0x01
    }

    /// <summary>
    /// 设备错误码
    /// </summary>
    public enum enDeviceError : byte
    {
        OK = 0X30,                               // 无错误
        ITEM_NOT_IN_RANGE = 0xB1,                // 被测物不在量程范围内
        LIGHT_TOO_DIM = 0xB2,                    // 光线太暗
        OVER_EXPOSURE = 0xB3,                    // 曝光过曝
        MEASURE_TIME_OVERFLOW = 0xB4,            // 测量时间溢出
        INVALID_HARDWARE_TRIG = 0xB5,            // 硬件触发无效
        ITEM_NOT_MATCH_THICKNESS_MEASURE = 0xB6, // 被测物不适用于厚度测量
        IMAGE_SENSOR_CAPTURING = 0XE1,           // 图像传感器采集中
        NO_IMAGE_SENSOR = 0XE2,                  //  没有检测到图像创啊你
        NO_DATA_IN_BUFFER = 0XE3,                // 缓存无数据
        CMD_HEADER_ERR = 0XF1,                   // 错误的包头
        CMD_LEN_ERR = 0XF2,                      // 错误的数据长度
        CMD_TYPE_ERR = 0XF3,                     // 命令错误
        CMD_DATA_ERR = 0XF4,                     // 数据错误
        NET_CONNECT_ERROR = 0XD2,
        INCOMPLETE_DATA_PACKAGE = 0XD4,
        NO_NET_DATA = 0XFE,
        MEASURE_PERIOD_IS_TOO_SMALL = 0xB7,      // 测量周期太小
///////////////////////////////////////////////////////////////////////////////
//TODO: 添加新的错误码

        UKNOWN = 0XFF
    }

#endregion

#region SCLib Native 结构体

    /// <summary>
    /// 图像传感器状态，启动和停止图像传感器采集功能
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SensorStauts_s
    {
        //0：停止；
        //1：启动
        //如果是软件触发模式；图像传感器会立即开始采集数据；
        //如果是硬件触发模式，只有当硬件触发信号到来时才会开始采集数据
        public enSensorStauts value;
    }

    /// <summary>
    /// lcd 页面选择
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct LcdPageIndex_s
    {
        public byte value; // 取值 1,2,3
    }

    /// <summary>
    ///  led亮度
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct LedValue_s
    {
        public byte value; // 取值： 0-100
    }

    /// <summary>
    ///  自定义折射率 厚度模式下有效. 用于指定当前被测物材质的折射率。选择正确的折射率有利于提高测量准确性。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct RefractiveIndexValue_s
    {
        public float value; // 取值参见RefractiveIndex的value字段.
    }

    /// <summary>
    ///  光学校准表
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct OpticalCalibrationTable_s
    {
       public float factor1; // 系数1.
       public float factor2; // 系数2.
       public float factor3; // 系数3.
    }

    /// <summary>
    ///  测量模式，.距离算法拟合1个波峰，厚度算法拟合2个波峰。设置后缓存计数值清零。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MeasureMode_s
    {
        public enMeasureMode value; // 1：距离模式； 2：厚度模式
    }

    /// <summary>
    ///  触发模式，. 设置后缓存计数值清零。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TrigMode_s
    {
        public enTrigMode value; // 1：软件触发； 2：硬件触发
    }

    /// <summary>
    ///  触发周期， 软件，硬件触发均有效.设置后缓存计数值清零。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TrigPeriod_s
    {
        public int value; // 单位:us; MeasureMode为距离模式时最小值为200；厚度模式时最小值为600
    }

    /// <summary>
    ///  阈值，阈值是对波峰灰度值的限制，小于阈值的波峰，其X坐标不作为重心处理。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ThresholdValue_s
    {
        public UInt16 value; // 取值 200-4095
    }

    /// <summary>
    ///  平滑滤波器，  设置的参数对应滤波点的个数。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SmoothFilter_s
    {
        public byte value; // 0: 禁止平滑滤波器； 其他取值：1,3,5,7,9
    }

    /// <summary>
    ///  平均值
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct AverageValue_s
    {
        public byte value; // 取值1-200
    }

    /// <summary>
    ///  惯性平均值，.对计算数据进行平均处理，平均后的数据作为测量值。平均值越大，采样速度成倍数递减。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct InertiaAverage_s
    {
        public byte value; // 取值1-100
    }

    /// <summary>
    ///  第一峰模式， 距离模式有效.启动第一峰后，图像传感器以第一个峰值的X坐标作为重心，不以最大灰度值的X坐标作为重心。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FirstPeakMode_s
    {
        public enFirstPeakMode value; // 1：使能； 0：禁止
    }

    /// <summary>
    ///  曝光，。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct Exposure_s
    {
        // 单位：us。 自动曝光：取值固定为0； 手动曝光：取值20-10000.
        // 距离模式时: 曝光最大值=触发周期(TrigPeriod)-120 // 触发周期要根据曝光时间来调整
        // 厚度模式时：曝光最大值=触发周期(TrigPeriod)-500
        public int value; 
    }

    /// <summary>
    ///  像素增益， 仅停止采集后可设置
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PixelGain_s
    {
        public byte value; // 取值：0-2
    }

    /// <summary>
    ///  PGA增益， 仅停止采集后可设置
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PGAGain_s
    {
        public byte value; // 取值: 0-15
    }

    /// <summary>
    ///  缓存计数值， 仅停止采集后可设置
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct StoredMeasureValuesCount_s
    {
        public int value; // 取值: 0 - 100,000
    }

    /// <summary>
    /// 缓存的测量值， 仅停止采集后可设置,适用固件版本≤150
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct StoredMeasureValues_s
    {
       // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public float value;  // float 类型
    }

    /// <summary>
    /// 缓存的测量值， 仅停止采集后可设置,适用固件版本>150
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct StoredMeasureValuesEx_s
    {
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public float value;         // float 类型
        public float upperSurface;  //上表面高度
        public float lowerSurface;  //下表面高度
    }

    /// <summary>
    /// 测量值，重心，强度；仅在启动采集后可获取。仅支持150及以前的固件版本
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct GSM_s
    {
        public float measureValue; // 测量值，单位: um
        public float gravity1;     // 波峰1重心
        public float gravity2;     // 波峰2重心， 仅在测量模式为"厚度模式"时有效
        public int strength1;      // 波峰1强度
        public int strength2;      // 波峰2强度， 仅在测量模式为"厚度模式"时有效
    }

    /// <summary>
    /// 测量值，重心，强度；仅在启动采集后可获取。仅支持150以后的固件版本
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct GSMEx_s
    {
        public float measureValue; // 测量值，单位: um
        public float gravity1;     // 波峰1重心
        public float gravity2;     // 波峰2重心， 仅在测量模式为"厚度模式"时有效
        public int strength1;      // 波峰1强度
        public int strength2;      // 波峰2强度， 仅在测量模式为"厚度模式"时有效
        public float upperSurface;      // 上表面高度
        public float lowerSurface;      // 下表面高度
    }

    /// <summary>
    /// ram中保存的参数值，一般初始化时读取
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct RAMSettings_s
    {
        public int reserved1;                   //
        public int measureMode;                 // 测量模式
        public int trigMode;                    // 触发模式
        public int trigPeriod;                  // 触发周期
        public int sensorStatus;                // 图像传感器采集状态
        public int firstPeakMode;               // 第一峰模式
        public int smoothFilter;                // 平滑滤波器
        public int thresholdValue;              // 阈值
        public int exposure;                    // 曝光值
        public int ledValue;                    // led 亮度
        public int pixelGain;                   // 像素增益
        public int pgaGain;                     // pga 增益
        public float opticalCalibrationFactor1; // 光学校准表系数1
        public float opticalCalibrationFactor2; // 光学校准表系数2
        public float opticalCalibrationFactor3; // 光学校准表系数3
        public float refractiveIndex;           // 自定义折射率
        public int averageValue;                // 平均值
        public int storedMeasureValuesCount;    // 缓存测量值计数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] devieIP;                  //设备ip地址,4个字节

        public int inertiaAverage;              // 惯性平均值
        public int autoExposure;                // 自动曝光状态
        public int refractiveIndexMode;         // 折射率模式
        public float calibrationFactor;         // 校准系数
        public UInt16 peakPixelRange;           // 波峰水平范围
        public UInt16 peakGrayRange;            // 波峰垂直范围
        public int fittingCount;             // 波峰拟合个数
        public int dividerFactor;            // 硬件触发分频系数

        ///////////////////////////////////////////////////////////////////////////////////
        // TODO: 添加新的属性
    }

    /// <summary>
    /// 固件版本
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FirmwareVersion_s
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] fimwareVersion; // 例如版本号: 2.1.3，则 fimwareVersion = {0x03, 0x01, 0x02}

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] reserved;
    }

    /// <summary>
    /// 设备名称
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DeviceName_s
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] name;
    }

    /// <summary>
    /// 设备序列号
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SerialNumber_s
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] number;
    }

    /// <summary>
    /// 按键状态
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct KeyStatus_s
    {
        public enSensorStauts sensorStatus; // 图像传感器采集状态
        public enZeroStatus zeroStatus;   // 归零状态
        public byte lcdPageIndex; // 液晶屏页面
    }

    /// <summary>
    ///  折射率模式：预定义模式或自定义模式 用于指定当前被测物材质的折射率。选择正确的折射率有利于提高测量准确性。
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct RefractiveIndexMode_s
    {
        public byte value; // 0:自定义模式； 1-20: 预定义模式，且预定义模式的值为value指定的序号。参见 gPredefineRefractiveIndexTable 的描述
    }

    /// <summary>
    /// 测头型号
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MeasureHeaderName_s
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] name;
    }

    /// <summary>
    /// 测头序列号
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MeasureHeaderSerialNumber_s
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] number;
    }

    /// <summary>
    /// 4字节整型ip: 例如ip为"10.10.1.30"， 则ip[0]=10,ip[1]=10,ip[2]=1,ip[3]=30
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DeviceIP_s
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] ip;
    }

    /// <summary>
    /// 附加系统配置
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SystemAddOn_s
    {
        // 循环初始化网络使能， 0: 仅初始化一次失败，不会重新初始化 1:初始化失败后重新循环初始化
        public char netInitCycleEnabled;
        // 无效数据缓存使能，开启后，设备缓存中将保留测得的无效数据，否则无效数据将被过滤。
        public char storeInvalidDataEnabled;
        public char reserved2;
        public char reserved3;
        public char reserved4;
        public char reserved5;
        public char reserved6;
        public char reserved7;
    }

    /// <summary>
    /// 设备连接配置，用于open()函数的传入参数
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ConnectionSetting_s
    {
        // 连接类型，参见ConnectionType枚举
        public enConnectionType connectionType;
        // softwareId。网口时代表ip，例如"10.10.1.30"；串口时代表串口号，例如"COM7"
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string sofwareId;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CalibrationFactor_s
    {
        // 校准系数 0-10
        public float factor;
    };

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct PeakPosition_s
    {
        //
        // 水平： 波峰像素值范围，此范围外的波峰将被过滤
        //
        public byte peakPixelRange;

        //
        // 垂直： 波峰灰度值范围，此范围外的波峰将被过滤
        //
        public byte peakGrayRange;

        //
        // 波峰拟合个数
        //
        public byte fittingCount;

    };

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct DividerFactor
    {
        public UInt16 dividerFactor;
    };
    /////////////////////////////////////////////////////////////////////////////////////
    // TODO: 添加新的属性数据结构
}

#endregion

//
//预定义折射率
//0, "H-FK61", 1.497f,
//1, "H-QK3L", 1.487f,
//2, "H-K9L", 1.517f,
//3, "H-K51", 1.523f,
//4, "H-BaK7", 1.569f,
//5, "H-ZK3", 1.589f,
//6, "H-ZK9B", 1.620f,
//7, "H-ZF2", 1.673f,
//8, "H-ZF4A", 1.728f,
//9, "H-ZF6", 1.755f,
//10, "H-ZF7LA", 1.805f,
//11, "H-ZF52", 1.847f,
//12, "H-ZF62", 1.923f,
//13, "H-ZF72A", 1.923f,
//14, "H-ZF88", 1.946f,
//15, "H-LaF3B", 1.744f,
//16, "H-LaF10LA", 1.788f,
//17, "H-ZLaF4LA", 1.911f,
//18, "H-ZLaF90", 1.001f,
//19, "D-LaF50", 1.774f};

