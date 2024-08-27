//20200602更新
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Sensor
{
    public class LE_ConstVal
    {
        public const int APP_FILTER = 0;      //滤波功能编号
        public const int APP_EXTREME = 1;     //去极值功能编号
    }
    //滤波参数结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct FilterParam
    {
        //是否启用滤波功能
        public Byte use;
        //滤波方式，0为1*3近似高斯滤波,1为1*3中值滤波					
        public Byte mode;
        //2项预留参数				
        public Byte data0;							
        public Byte data1;
        //滤波次数
        public int count;							
    }
    //去极值参数结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct ExtremeDataParam
    {
        //是否启用去极值功能
        public Byte use;
        //去极值模式,模式0-使用目标点与参考点做高度差判断是否极值；模式1-使用二阶导数来判断是否极值，
        //适应性更好，优先推荐此模式；模式2-使用圆弧趋势。
        public Byte mode;
        //极值数据去除后填充模式，0-使用插值填充，1-使用给定有效数据范围最小值填充，2-使用给定有效数
        //据范围最大值填充,3-使用自定义值填充，自定义设置值由data2保存。
        public Byte fittedMode;
        //若去极值模式为4时此参数表示极限高度差，其他模式不起作用。
        public Byte data0;
        //数据是否有效判断点数,默认为5，最小可设置3。
        public int refPoint;
        //若去极值模式为0时此参数表示待判断点与参考点的差值的最大范围，默认为40um；
        //若去极值模式为1时此参数表示相邻点间斜率变化的最大范围，默认为0.6。
        public float HThreshold;
        //待判断点有效时左右有效参考点占总参考点数量的最低比例通常为0.6，最大可设置1。
        public float validRa;
        //数据取样间隔，单位um，默认为2um。
        public int data1;
        //自定义填充值，仅当fittedMode模式为3时才有效
        public int data2;
    }
    //圆孔胶水计算参数结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct GlueCalcParam
    {
        //寻找胶水模式，0-自动寻找最高点胶水厚度，适用于扫描高度截面包含2个上表面的数据，1-直接找两边最大值作为胶水高度，
        //然后计算胶水厚度，此模式适用于扫描高度截面不包含2个上表面的数据，默认为0。
        public int mode;
        //导脚过滤占孔深的百分比,一道胶默认为5%,二道胶默认为20%。
        public int refRa;
        //平面斜率变化率阈值，默认为0.1，可设范围0.1~0.5。
        public float planeRa;
        //曲面斜率变化率阈值，默认为0.6，可设范围0.1~5,一道胶经验值为0.3，二道胶经验值为0.6。
        public float glueRa;
        //滤波次数，默认为20，可根据原始数据是否有很多毛刺来加大或减小，一道胶经验值为10，二道胶经验值为20。
        public int smooth;
        //判断一段连贯数据是否为有效数据的最少数据点数，默认为20，可根据实际测试时干扰数据段的长短来调节，通常要设置为大于感染数据长度的值。
        public int subsectionCnt;
        //产品扫描时的倾斜角度，默认为0，如果实际测量时使产品倾斜了则将倾斜角度填入此参数。
        public float RtAngle;
        //相邻数据点之间的物理间距，单位um	，默认为1um，按实际设置。
        public float step;
        //（一道胶计算专用）判断与上表面相平胶水时的斜率变化率阈值，默认为0.2。
        public float slopRanage;
        //（一道胶计算专用）曲面斜率最小值，默认为0.1。
        public float slopRaThd;
        //（一道胶计算专用）曲面最少有效点数量，默认为30。
        public int slopeValidPts;
        //是否启用导脚过滤，0-表示不开启，1-表示开启，默认启用。
        public char footFilter;
        //自动倾斜修正开关，0-表示不开启，1-表示开启，默认不开启，此功能目前效果欠佳，优化中。
        public char titleCorrection;
        //（一道胶计算专用）胶水与上表面相平判断开关，0-表示不开启，1-表示开启,注意此判断功能仅限于一道胶测量，测量二道胶时禁止开启。
        public char judgeGlueFlatPlane;
        //预留
        public char data0;
    }
    //圆孔胶水计算结果结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct ProductResult
    {
        //计算出的胶水厚度
        public float glueThickness;
        //找到的最高点胶水读数
        public float glue;
        //圆孔底部高度读数
        public float bottom;
        //圆孔上表面高度读数
        public float top;
        //寻找到的胶水最高点数据下标
        public int gluePos;
        //寻找到的圆孔上表面第一个数据下标
        public int topPos;
        //寻找的圆孔底部第一个数据下标
        public int bottomPos;
    }
    //单边测厚单层上下表面峰序号结构体
   [StructLayout(LayoutKind.Sequential)]
    public struct TKPeakIdx
    {
        //上表面的序号
        public Byte top;
        //下表面的序号，注意下表面的序号必须大于上表面
        public Byte bottom;
        //层编号，最小为1
        public Byte idx;
        //预留
        public Byte data1;

        public  TKPeakIdx(int topPeak, int bottomPeak, int thickIdx)
        {
            this.top =Convert.ToByte(topPeak);
            this.bottom = Convert.ToByte(bottomPeak);
            this.idx = Convert.ToByte(thickIdx);
            this.data1 = 0;
        }
    }

   class LEConfocalDLL
   {
       #region bytesToStruct
       // Byte array to struct or classs.     
       public static object bytesToStruct(byte[] bytes, Type type)
       {
           //Get size of the struct or class.
           int size = Marshal.SizeOf(type);
           if (bytes.Length < size)
           {
               return null;
           }
           //Allocate memory space of the struct or class. 
           IntPtr structPtr = Marshal.AllocHGlobal(size);
           //Copy byte array to the memory space.
           Marshal.Copy(bytes, 0, structPtr, size);
           //Convert memory space to destination struct or class. 
           object obj = Marshal.PtrToStructure(structPtr, type);
           //Release memory space.
           Marshal.FreeHGlobal(structPtr);
           return obj;
       }
       #endregion
       //将byte数组转换成指定数量的TKPeakIdx数组。
       public static bool ConvertBytesToTKPeakIdx(byte[] bytes, TKPeakIdx[] tkPeakIdx, int len)
       {
           if (tkPeakIdx.Length > 0)
           {
               int size = Marshal.SizeOf(tkPeakIdx[0]);
               if (bytes.Length < size * len)
               {
                   return false;
               }
               IntPtr structPtr = Marshal.AllocHGlobal(size);
               for (int i = 0; i < len; ++i)
               {
                   Marshal.Copy(bytes, i * size, structPtr, size);
                   tkPeakIdx[i] = (TKPeakIdx)Marshal.PtrToStructure(structPtr, typeof(TKPeakIdx)); ;
               }
               Marshal.FreeHGlobal(structPtr);
               return true;
           }
           else
               return false;
       }

       /*
       函数功能：数据采集任务完成或被主动停止采集任务后DLL内部执行的回调函数类型。
       参数：
       capturedCount-保存当前任务已采集到的数据数量。
       返回值：无。
       */
       [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
       public delegate void CBCaptureComplete(int capturedCount);
       /*
       函数功能：获取当前DLL的版本号。
       参数：
       pDllVersion-[out]保存获取到的DLL版本号，格式类似于“V1.76.8”。
       len-[in]pDllVersion的存储长度，需大于等于32。
       返回值：无。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern void LE_GetDLLVersion(char[] pDllVersion, int len);
       /*
       函数功能：选择所使用的控制器类型，本函数必须在LE_InitDLL前调用，一旦调用LE_InitDLL初始化成功后，
       本函数将不能再调用，除非调用LE_UnInitDLL进行反初始化才可以。
       参数：
       dcType-[in]选择使用的控制器类型，0-USB 3.0接口1代，1-以太网接口，2-USB 3.0接口2代,DLL内部默认为
       USB 3.0接口2代。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SelectDeviceType(int dcType);

       /*
       函数功能：获取当前所使用的控制器类型，本函数可以在任意位置调用。
       参数：
       返回值：
       0-USB 3.0接口1代。
       1-以太网接口。
       2-USB 3.0接口2代。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetDeviceType();

       /*
       函数功能：初始化控制器DLL，调用本函数前需插入匹配的加密狗，使用LE_InitDLL进行初始化成功后，必须
       在应用程序关闭之前调用一次且只能调用一次LE_UnInitDLL释放初始化所获得的资源。
       参数：
       返回值：
       1-初始化成功。
       -29-未找到匹配的加密狗。
       -30-USB一代控制器初始化失败，请检查是否已安装USB一代最新版驱动。
       -31-以太网控制器初始化失败，请检查是否已安装以太网版本驱动。
       -32-USB二代控制器初始化失败，请检查是否已安装USB二代最新版驱动。
       其他返回值请参考返回值定义表，返回值类型在20190725由bool改成int类型。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_InitDLL();

       /*
       函数功能：反初始化控制器DLL。
       参数：
       返回值：成功返回true，失败返回false。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern bool LE_UnInitDLL();

       /*
       函数功能：[调用时不能有控制器被打开]获取控制器数量，此函数每次执行都会重新枚举一次控制器数量，所
       以调用此函数时请确保没有控制器为打开状态，否则可能会出现控制器状态异常的现象。
       参数：
       返回值：成功返回获取到的数量，失败返回-10。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetSpecCount();

       /*
       函数功能：获取已连接电脑的所有控制器的序列号。
       参数：
       pSns-[out]保存所有控制器序列号的数组，每个控制器序列号固定占用32个数组元素，pSns数组长度需大于等
       于32*count。
       count-[in]控制器的数量,最大为10个。
       返回值：成功返回true，失败返回false。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern bool LE_GetSpecSN(char[] pSns, int count);

       /*
       函数功能：获取控制器SN号，名称、固件版本号等信息，每个控制器的每一项属性固定占用32个数组元素，例
       如第一个控制器SN号的长度只有12个字符，那第二个控制器的SN号第一个字符存储的位置为pSNs[1*32]，其他
       属性也是如此。
       参数：
       pSns-[out]保存获取到的序列号数组，长度需大于等于32*count。
       pName-[out]保存获取到的控制器名称数组，长度需大于等于32*count。
       pFMVersion-[out]保存获取到的固件数组，长度需大于等于32*count。
       count-[in]期望获取信息的控制器数量。
       返回值：成功返回true；失败返回false。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern bool LE_GetSpecMsg(char[] pSns, char[] pName, char[] pFMVersion, int count);

       /*
        函数功能：打开指定序列号控制器。
        参数：
        pSn-[in]控制器序列号，长度为32的字符串。
        specHandle-[out]打开成功后传出的控制器句柄。
        返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_Open(char[] pSn, ref int specHandle);

       /*
       函数功能：关闭指定句柄控制器。
       参数： 
       specHandle-[in]需关闭的控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表	。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_Close(ref int specHandle);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器采样频率，注意控制器采样频率设置可能会影响到
       曝光时间，比如说当前控制器的曝光时间为1.5ms的时候，如果设置采样频率为1000Hz的时候，控制器内部会
       优先满足采样频率，从而内部强行降低曝光时间至0.92ms（1000Hz频率下曝光时间能设置的理论上限为1ms，
       然后再减去0.08ms的计算时间就等于0.92ms）左右。
       参数：
       sampleHz-[in]待设置采样频率。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetSampleFrequency(int sampleHz, int specHandle);

       /*
       函数功能：获取控制器采样频率。
       参数：
       sampleHz-[out]待获取采样频率。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetSampleFrequency(ref int sampleHz, int specHandle);

       /*
       函数功能：获取控制器曝光时间范围，单位ms。
       参数：
       eptMin-[out]保存获取到的曝光时间范围最小值。
       eptMax-[out]保存获取到的曝光时间范围最大值。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetExposureRange(ref float eptMin, ref float eptMax, int specHandle);

       /*
       函数功能：设置曝光时间，单位为ms。
       参数：
       expTime-[in]待设置曝光时间。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetExposureTime(float expTime, int specHandle);

       /*
       函数功能：获取曝光时间,单位为ms。
       参数：
       expTime-[out]保存获取到的曝光时间值。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetExposureTime(ref float expTime, int specHandle);

       /*
       函数功能：获取控制器增益范围。
       参数：
       gainMin-[out]保存获取到的增益范围最小值。
       gainMax-[out]保存获取到的增益范围最大值。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetGainRange(ref float gainMin, ref float gainMax, int specHandle);

       /*
       函数功能：设置控制器增益。
       参数：
       ga-[in]待设置增益值。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetGain(float ga, int specHandle);

       /*
       函数功能：获取控制器增益。
       参数： 
       ga-[out]保存获取到的增益值。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetGain(ref float ga, int specHandle);

       /*
       函数功能：获取通道光斑中心位置,及通道光斑所占行数。
       参数： 
       offsetLine-[out]保持获取到的能量中心位置。
       lines-[out]保存获取到的光斑所占行数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetPosOffset(ref int offsetLine, ref int lines, int specHandle, int channel = 1);

       /*
       函数功能：设置控制器触发模式。
       参数：
       triggerMode-[in]触发模式，0表示连续获取模式，2表示硬件触发。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetTriggerMode(int triggerMode, int specHandle);


       /*
       函数功能：获取控制器触发模式。
       参数：
       triggerMode-[out]保存获取到的触发模式，0表示连续获取模式，2表示硬件触发。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetTriggerMode(ref int triggerMode, int specHandle);

       /*
       函数功能：设置控制器外触发模式下的触发源，只能在硬件触发模式修改，且只在硬件触发模式下起作用。
       参数：
       sourceMode-[in]待设置触发源类型，0-上升沿触发，1-下降沿触发，当使用边沿触发时，传感器按外触发信
       号频率来采集数据，2-高电平触发，3-低电平触发，当使用有效电平来触发时，在信号有效时控制器按设定的
       频率连续采集数据。
       specHandle-[in]控制器句柄。
       返回值：1-设置成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetTriggerSource(int sourceMode, int specHandle);


       /*
       函数功能：获取控制器外触发模式下的触发源。
       参数：
       sourceMode-[out]保存获取到的触发源类型，含义同LE_SetTriggerSource中第一个参数保持一致。
       specHandle-[in]控制器句柄。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetTriggerSource(ref int sourceMode, int specHandle);

       /*
       函数功能：[只能在控制器空闲状态下调用]通过文件载入控制器配置数据。
       参数：
       filePath-[in]目标文件路径字符数组，允许最大长度2000，路径末尾必须有空字符（即ASCII码为0的字符）
       结尾,文件格式为.dcfg。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_LoadDeviceConfigureFromFile([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder filePath, int specHandle);

       /*
       函数功能：获取指定控制器通道数量。
       参数：
       specHandle-[in]控制器句柄。
       返回值：成功返回获取到的大于等于0的通道数量；负数-获取失败，含义请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetChannels(int specHandle);

       /*
       函数功能：[只能在控制器空闲状态下调用]开启或关闭指定控制器指定通道，此函数只能在指定控制器无采集
       任务时才能执行成功。
       参数：
       specHandle-[in]控制器句柄。
       channel-[in]通道序号，当前默认为1。
       bEnable-[in]true表示启用通道，false表示禁用通道。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_EnableChannel(int specHandle, int channel = 1, bool bEnable = true);

       /*
       函数功能：[只能在控制器空闲状态下调用]通过文件载入单通道配置数据,LE_LoadDeviceConfigureFromFile
       函数调用后已载入所有通道参数，当前函数仅在某通道参数修改乱了之后需重新加载文件内参数使用，一般无
       需使用。
       参数：
       filePath-[in]目标文件路径字符数组，允许最大长度2000，路径末尾必须有空字符（即ASCII码为0的字符）
       结尾，文件格式为.dcfg。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_LoadChannelConfigureFromFile([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder filePath, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);

        
       /*
       函数功能：[只能在控制器空闲状态下调用]通过文件载入通道高度校准数据，如果使用多通道，则此函数需要
       执行多次。
       参数：
       filePath-[in]目标文件路径字符数组，允许最大长度2000，路径末尾必须有空字符（即ASCII码为0的字符）
       结尾，文件格式为.hwc。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_LoadLWCalibrationData([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder filePath, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);

       /*
       函数功能：获取控制器指定通道量程，单位为um。
       参数：
       dRange-[out]保存获取到的量程值，量程范围为0~dRange。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号，当前默认为1。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetDistanceRange(ref float dRange, int specHandle, int channel = 1);

       /*
       函数功能：将当前控制器计算参数保存到指定文件中，注意保存之前必须至少成功载入过一次参数。
       参数：
       filePath-[in]目标文件路径字符数组，允许最大长度2000，路径末尾必须有空字符（即ASCII码为0的字符）
       结尾,文件格式为.dcfg。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN - [in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN - [in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：[in]成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_SaveParamToFile([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder filePath, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);

       /*
       函数功能：[只能在控制器空闲状态下调用]采集控制器已启用通道的dark数据，采集前请先用黑色盖子盖住对
       应通道探头，直到采集完成。
       参数：
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_CaptureChannelsIntensityCaliData(int specHandle);

       /*
       函数功能：获取控制器通道当前Dark完成进度，单位为%，当开启了多通道时，所有通道Dark进度是一致的只
       需获取一个通道进度即可。
       参数：	
       rate-[out]保存获取到的进度比例，单位为%。
       specHandle - [in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetDarkRate(ref int rate, int specHandle, int channel = 1);

       /*
       函数功能：获得指定通道单帧波形曲线数据长度及最大能达到的能量值。
       参数：
       specHandle-[in]控制器句柄。
       length-[out]保存获取到的单帧波形曲线数据数组的长度，不同类型控制器长度可能不一样。
       maxValue-[out]保存波形曲线数据中波峰能达到的最大能量值。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetChannelIntensityMsg(int specHandle, ref int length, ref int maxValue, int channel = 1);

       /*
       函数功能：设置测量到无效数据时的处理方式。
       参数：
       prdMod-[in]待设置数据处理模式,0-不处理，1-保持，2-改为量程内最小值，3-改为量程内最大值。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_SetInvalidDataPrdModel(int prdMod, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);


       /*
       函数功能：获取测量到无效数据的处理方式。
       参数：
       prdMod-[out]保存获取到的数据处理模式,0-不处理，1-保持，2-改为量程内最小值，3-改为量程内最大值。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_GetInvalidDataPrdModel(ref int prdMod, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);

       /*
       函数功能：设置通道取峰模式。
       参数：
       CPeakMode-[in]待设置取峰模式，0-取最优峰；1-取最近峰；2-取最远峰；3-自定义。
       specHandle-[in]控制器句柄。
       channel-[in]通道号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetCapturePeakMode(int CPeakMode, int specHandle, int channel = 1);

       /*
       函数功能：获取通道取峰模式。
       参数：
       specHandle-[in]控制器句柄。
       channel-[in]通道号。
       返回值：小于0表示获取失败；0-取最优峰；1-取最近峰；2-取最远锋；3-自定义。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetCapturePeakMode(int specHandle, int channel = 1);

       /*
       函数功能：设置通道高度模式时“自定义”取峰模式下的目标峰序号。
       参数：
       peakIdx-[in]待设置峰序号，最小可设置为1。
       specHandle-[in]控制器句柄。
       channel-[in]通道号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetDistancePeakIdx(int peakIdx, int specHandle, int channel = 1);

       /*
       函数功能：获取通道取峰模式为“自定义”模式时的期望峰编号。
       参数：
       specHandle-[in]控制器句柄。
       channel-[in]通道号。
       返回值：返回大于等于1的值表示成功，获取失败返回1。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetDistancePeakIdx(int specHandle, int channel = 1);

       /*
       函数功能：获取指定控制器当前实际的采样频率。
       参数：
       spFrequency-[out]保存获取到的采样频率，控制器如果在采集数据则输出实时帧率，如果控制器未在采集数
       据则输出-1。
       specHandle - [in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetCurrentSampleFrequency(ref int spFrequency, int specHandle);

       /*
       函数功能：获取指定控制器最大能达到的采样频率。
       参数：
       maxSPFrequency-[out]保存获取到的最大采样频率。
       specHandle - [in]控制器句柄。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetLimitSampleFrequency(ref int maxSPFrequency, int specHandle);

       /*
       函数功能：开启或关闭DLL内部自动调节曝光、增益功能。
       参数：
       bAuto-[in]true开启自动调节功能,false关闭自动调节功能。
       specHandle-[in]控制器句柄。
       返回值：1-设置成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetAutoEptGain(bool bAuto, int specHandle);

       /*
       函数功能：获取DLL内部自动调节曝光、增益功能状态。
       参数：
       bAuto-[out]保存获取到的自动调节功能状态。
       specHandle-[in]控制器句柄。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetAutoEptGainSta(ref bool bAuto, int specHandle);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器当前与软件连接的通讯超时时间，默认设置为3s，
       即如果软件出现意外情况崩溃或者卡死时，控制器等待的时间，如果超过指定时间控制器仍未与软件通讯上则
       控制器将主动断开与软件的连接，此时软件要继续访问控制器采集数据需重新打开连接控制器，该功能通常用
       于二次开发时调试程序使用，调试程序时该时间需设置大一些，这样在程序进入断点中断时控制器不至于断开
       与软件的连接，发布程序请将该时间设置为3s钟左右，如果设置时间过长会导致软件意外结束时，需至少等待
       大于通讯超时时间的设置值才能重新打开控制器，或者控制器需要断电重启。
       参数：	
       hTime-[in]待设置通讯超时时间，单位为ms。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetHeartTime(int hTime, int specHandle);

       /*
       函数功能：获取指定控制器通讯超时时间。
       参数：	
       hTime-[out]保存获取到的通讯超时时间，单位为ms。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetHeartTime(ref int hTime, int specHandle);

       /***********************数据采集函数，适用于每次采集任务只需要单个通道的场景*******************/
       /*
       函数功能：[只能在控制器空闲状态下调用]获取指定数量的一种或多种类型数据，数据类型包含高度测量结果
       、感光度、峰像素位置、波形曲线数据，实际调用时函数内部会根据给定参数指针是否为0来判断是否需要保
       存到调用方给定的内存中，当采集任务完成或者被用户主动停止后如果此函数调用时给定了回调函数指针，那
       么DLL内部将执行回调函数，此函数一旦调用成功则控制器开始进入数据采集状态。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组，数组长度为pts。
       pts-[in]期望采集测量结果点数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pUnitRa-[out]pts个点的感光度数组，数组长度为pts，如果不需要获取此项数据则此参数传入0或空指针。
       pPeakWave-[out]pts个点峰像素位置数组，数组长度为pts，如果不需要获取此项数据则此参数传入0或空指针。
       pPrdItn-[out]pts个点能量曲线数据数组，数组长度为pts*length（length代表单帧能量曲线的长度，可以通
       过LE_GetChannelIntensityMsg函数获得），如果不需要获取此项数据则此参数传入0或空指针。
       pFunction-[in]采集任务完成或者被用户主动停止后执行的回调函数指针，如果不需要此功能则此参数传入0
       或空指针。
       executeMode-[in]回调函数在DLL内部执行方式，0-表示同步执行，DLL内部将调用并等到回调函数执行完成后
       ，当前采集任务才算完成，目前只支持此模式，其他异步等模式将后续集成开放。
       data0-[in]预留参数，默认为0，目前不起作用。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       -12-给定指针参数无效,无法开启新的采集任务。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetAllValues(double[] pRstHigh, int pts, int specHandle, int channel = 1, float[] pUnitRa = null,
           float[] pPeakWave = null, int[] pPrdItn = null, CBCaptureComplete pFunction = null, int executeMode = 0, int data0 = 0);
       /**********************************************************************************************/

       /**********适用于单通道采集与多通道采集，当使用多通道采集时，可以通过下列函数设置需要获取的数据
       类型及保存的位置，然后通过LE_StartGetChannelsValues函数来同步启用多个通道的采集任务**********/
       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器指定通道获取指定数量的一种或多种类型数据，数
       据类型包含高度测量结果、波形曲线、感光度、峰像素位置数据，实际调用时函数内部会根据给定参数指针是
       否为0来判断该类型数据是否需要保存到调用方给定的内存中，此函数只设置好保存数据的数组及期望采集数
       据量，如果使用多个通道同时采集数据则此函数需调用多次分别设置各通道的参数，需使用
       LE_StartGetChannelsValues或LE_StartGetChannelsValuesEx来统一开启一个或多个通道的采集任务。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组，数组长度为pts。
       pts-[in]期望采集测量结果点数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pUnitRa-[out]pts个点的感光度数组，数组长度为pts，如果不需要获取此项数据则此参数传入0或空指针。
       pPeakWave-[out]pts个点峰像素位置数组，数组长度为pts，如果不需要获取此项数据则此参数传入0或空指针。
       pPrdItn-[out]pts个点能量曲线数据数组，数组长度为pts*length（length代表单帧能量曲线的长度，可以通
       过LE_GetChannelIntensityMsg函数获得），如果不需要获取此项数据则此参数传入0或空指针。
       data0-[in]预留参数，默认为0，目前不起作用。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       -12-给定指针参数无效,无法开启新的采集任务。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetAllValues(double[] pRstHigh, int pts, int specHandle, int channel = 1, float[] pUnitRa = null,
           float[] pPeakWave = null, int[] pPrdItn = null, int data0 = 0);

       /*
       函数功能：[只能在控制器空闲状态下调用]开启指定控制器多通道采集任务。
       参数：
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetChannelsValues(int specHandle);

       /*
       函数功能：[只能在控制器空闲状态下调用]开启指定控制器多通道采集任务,同时需要给定回调函数指针，当
       采集任务完成或者被用户主动停止后，DLL内部将执行给定回调函数。
       参数：
       specHandle-[in]控制器句柄；
       pFunction-[in]采集任务完成或者被用户主动停止后执行的回调函数指针，capturedCount保存了当前任务已
       采集数据数量。
       executeMode-[in]回调函数在DLL内部执行方式，0-表示同步执行，DLL内部将调用并等到回调函数执行完成后
       ，当前采集任务才算完成，目前只支持此模式，其他异步等模式将后续集成开放。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetChannelsValuesEx(int specHandle, CBCaptureComplete pFunction, int executeMode = 0);

       /**********************************************************************************************/
       /*
       函数功能：获取指定控制器通道状态。
       参数：
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：
       0-控制器已完成上一次采集任务或无采集任务，等待开启采集任务指令。
       1-高度数据采集中。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetDeviceStatus(int specHandle, int channel = 1);

       /*
       函数功能：查询指定通道已采集到的数据量。
       参数：
       values-[out]保存查询到的已采集的数据量。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetCapturedPoints(ref int values, int specHandle, int channel = 1);

       /*
       函数功能：主动停止指定控制器数据采集任务，目前因多通道产品采集数据时需同开同停，故此函数即便在使
       用多通道产品的时候也只需要调用一次即可将所用通道的采集任务停掉。
       参数：
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StopGetPoints(int specHandle, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定通道目标峰数,传感器配置文件内出厂已设置好，一般不
       需要修改。
       参数：
       peaks-[in]待写入的“目标峰数”。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetTargetPeaks(int peaks, int specHandle, int channel = 1);


       /*
       函数功能：获取指定通道目标峰数。
       参数：
       peaks-[out]保存获取的“目标峰数”参数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetTargetPeaks(ref int peaks, int specHandle, int channel = 1);

       /*
       函数功能：设置指定控制器外触发模式下分频触发比例,注意该功能需控制器本身支持该功能才可使用!
       参数：
       specHandle-[in]控制器句柄。
       counterVal-[in]分频比例，默认为1，例如想每5个外部触发信号使控制器采集一帧数据，则该参数设为5。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetFractionalFrequency(int specHandle, int counterVal = 1);

       /*
       函数功能：获取指定控制器外触发模式下分频触发比例,注意该功能需控制器本身支持该功能才可使用!
       参数：
       specHandle-[in]控制器句柄。
       返回值：
       非零-获取到当前控制器分频比例。
       -36-控制器句柄无效或不存在。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetFractionalFrequency(int specHandle);

       /*
       函数功能：清除指定控制器外触发模式下分频计数，例如设置分频比例为5时，已发送了3个触发信号时，希望
       控制器对触发信号重新计数时可调用该函数,注意该功能需控制器本身支持该功能才可使用!
       参数：	
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_ClearFractionalCountesy(int specHandle);

       /***************************************单边测厚相关函数***************************************/
       /*
       函数功能：设置单层或多层厚度上下表面峰序号。
       参数：
       pTKPeakIdxs-[in]待设置的单层或多层厚度上下表面峰序号定义结构体数组。
       tkCount-[in]期望设置层数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetTKPeakIdxs(TKPeakIdx[] pTKPeakIdxs, int tkCount, int specHandle, int channel = 1);

       /*
       函数功能：获取指定通道多层厚度上下表面峰序号定义，tkCount为希望获取的层数，注意此函数只能按顺序
       输出多层的上下表面峰序号定义，例如tkCount为2的时候，输出的是第一第二层厚度的定义。
       参数：
       pTKPeakIdxs-[out]保存获取到的多层厚度上下表面峰序号定义结构体
       tkCount-[in]期望获取层数，目前最大为5层。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetTKPeakIdxs(byte[] pTKPeakIdxs, int tkCount, int specHandle, int channel = 1);

       /*
       函数功能：通过文件载入指定通道厚度校准数据。
       参数：
       count-[out]载入成功后保存指定通道包含的材质校准数据数量。
       filePath-[in]目标文件路径字符数组，允许最大长度2000，路径末尾必须有空字符（即ASCII码为0的字符）
       结尾，文件格式为.tkc。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_LoadTKCalibrationData(ref int count, [In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder filePath, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);


       /*
       函数功能：获得当前通道包含的所有厚度校准数据材质名称。
       参数：
       materialName-[out]保存获取到的材质名称列表字符数组,长度为count*20。
       count-[in]最大为LE_LoadTKCalibrationData获得的材质校准数据数量。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_GetMaterialsName(char[] materialsName, int count, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);


       /*
       函数功能：选择指定名称的材质厚度校准数据作为第一层厚度的校准数据。
       参数：
       materialName-[in]待选择材质名称,名称长度最大为20个双字节字符。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_SelectTKCaliData([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder materialName, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);

       /*
       函数功能：选择单层或按顺序选择多层指定名称的材质厚度校准数据。
       参数：
       materialNames-[in]待选择材质名称数组,单个名称长度最大为20个双字节字符，目前最多一次选择5层，数组
       长度为20*count。
       count-[in]期望选择层数，注意此函数选择时层数是按递增顺序的，例如count=3，则分别选择1、2、3层的厚
       度校准数据。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_SelectTKCaliDataEx([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder materialNames, int count, int specHandle, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]在线校准已知厚度样品的单边测厚系数。
       参数：
       thickness-[in]待校准材质厚度。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_CaliTKRa(float thickness, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);
       /*
       函数功能：[只能在控制器空闲状态下调用]在线校准已知单层或多层厚度样品的单边测厚系数。
       参数：
       pThickness-[in]待校准材质单层或多层厚度，数组长度为count。
       pTKIdx-[in]待校准的层序号，数组长度为count。
       count-[in]期望校准层数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_CaliTKRaEx(float[] pThickness, int[] pTKIdx, int count, int specHandle, int channel);

       /*
       函数功能：将当前使用的第一层单边测厚系数存入指定文件。
       参数：
       filePathp-[in]待保存文件路径，允许最大长度2000，路径末尾必须有空字符（即ASCII码为0的字符）结尾。
       materialName-[in]待保存材质名称,单个名称长度最大为20个双字节字符，数组长度为20。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pFiberSN-[in]光纤序列号，目前默认为空，调用函数时无需输入参数。
       pLensSN-[in]探头序列号，目前默认为空，调用时无需输入参数。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_SaveTKRa([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder filePath, [In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder materialName, int specHandle, int channel = 1, char[] pFiberSN = null, char[] pLensSN = null);

       /*
       函数功能：将当前单层或多层使用的单边测厚系数存入指定文件。
       参数：
       filePathp-[in]待保存文件路径，允许最大长度2000，路径末尾必须有空字符（即ASCII码为0的字符）结尾。
       materialNames-[in]单层或多层待保存材质名称,单个名称长度最大为20个双字节字符，数组长度为20*count
       ，目前最多一次保存5层。
       pTKIdx-[in]待保存的层序号，数组长度为count。
       count-[in]期望校准层数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll", CharSet = CharSet.Unicode)]
       public static extern int LE_SaveTKRaEx([In, MarshalAs(UnmanagedType.LPWStr)]StringBuilder filePath, char[] materialNames, int[] pTKIdx,
           int count, int specHandle, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器指定通道采集多种类型数据，其中包含第一层厚度
       数据、上下表面高度数据、感光度、上下表面波峰像素位置数据、波形曲线数据，具体需采集哪些数据由给定
       的指针或数组首地址是否等于0或为空来决定，此函数只设置好保存数据的数组及期望采集数据量，如果使用
       多个通道同时采集数据则此函数需调用多次分别设置各通道的参数，需使用LE_StartGetChannelsValues或
       LE_StartGetChannelsValuesEx来统一开启一个或多个通道的采集任务。
       参数：
       pRstTK-[out]保存采集到的厚度数据数组。
       pts-[in]期望获取厚度点数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pDistance1-[out]保存采集到的上表面高度数组，如果不需要获取此项数据则此参数传入0或空指针。
       pDistance2-[out]保存采集到的下表面高度数组，如果不需要获取此项数据则此参数传入0或空指针,注意
       pDistance1和pDistance1是否获取需保持一致，即要么2个都获取，要么都不获取。
       pUnitRa-[out]pts个点感光度数组，如果不需要获取此项数据则此参数传入0或空指针。
       pPeakWave1-[out]保存上表面波峰像素位置数组，数组长度为pts，如果不需要获取此项数据则此参数传入0或
       空指针。
       pPeakWave2-[out]保存下表面波峰像素位置数组，数组长度为pts，如果不需要获取此项数据则此参数传入0或
       空指针,注意pPeakWave1和pPeakWave2是否获取需保持一致，即要么2个都获取，要么都不获取。
       pPrdItn-[out]pts个点波形曲线数据数组,数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得），如果不需要获取此项数据则此参数传入0或空指针。
       data0-[out]预留。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessAllValuesParam(double[] pRstTK, int pts, int specHandle, int channel = 1, double[] pDistance1 = null,
           double[] pDistance2 = null, float[] pUnitRa = null, float[] pPeakWave1 = null, float[] pPeakWave2 = null, int[] pPrdItn = null, float[] data0 = null);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器指定通道采集多种类型数据，其中包含多层厚度数
       据、多层上下表面高度数据、感光度、多层上下表面波峰像素位置数据、波形曲线数据，具体需采集哪些数据
       由给定的指针或数组首地址是否等于0或为空来决定，此函数只设置好保存数据的数组及期望采集数据量，如
       果使用多个通道同时采集数据则此函数需调用多次分别设置各通道的参数，需使用
       LE_StartGetChannelsValues或LE_StartGetChannelsValuesEx来统一开启一个或多个通道的采集任务。
       参数：
       pRstTK-[out]保存采集到的厚度数据数组，长度等于pts*tkCount。
       pts-[in]期望获取厚度点数。
       tkCount-[in]单帧能量数据中期望获取的层数，最小为1层，目前最大为5层。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       pDistance1-[out]保存采集到的上表面高度数组，长度等于pts*tkCount，如果不需要获取此项数据则此参数
       传入0或空指针。
       pDistance2-[out]保存采集到的下表面高度数组，长度等于pts*tkCount，如果不需要获取此项数据则此参数
       传入0或空指针,注意pDistance1和pDistance2要么2个都获取，要么都不获取。
       pUnitRa-[out]pts个点感光度数组，如果不需要获取此项数据则此参数传入0或空指针。
       pPeakWave1-[out]保存上表面波峰像素位置数组，长度等于pts*tkCount，如果不需要获取此项数据则此参数
       传入0或空指针。
       pPeakWave2-[out]保存下表面波峰像素位置数组，长度等于pts*tkCount，如果不需要获取此项数据则此参数
       传入0或空指针,注意pPeakWave1和pPeakWave2要么2个都获取，要么都不获取。
       pPrdItn-[out]pts个点波形曲线数据数组,数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得），如果不需要获取此项数据则此参数传入0或空指针。
       data0-[out]预留。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessAllValuesParamEx(double[] pRstTK, int pts, int tkCount, int specHandle, int channel = 1,
           double[] pDistance1 = null, double[] pDistance2 = null, float[] pUnitRa = null, float[] pPeakWave1 = null, float[] pPeakWave2 = null, int[] pPrdItn = null,
           float[] data0 = null);
       /**********************************************************************************************/

       /********************************************************进阶功能、二次校准相关****************/
       /*
       函数功能：通过给定像素位置数组以及高度数组计算传感器高度读数二次校准数据，像素及高度数组由用户提
       供。
       参数：	
       pWave-[in]像素数组。
       pDistance-[in]高度数组。
       count-[in]像素与高度数组长度。
       bSave-[in]计算完后是否直接保存到已加载的高度校准文件。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_CalcDistanceCalibrationDataSecond(float[] pWave, double[] pDistance, int count, bool bSave, int specHandle, int channel = 1);

       /*
       函数功能：保存高度二次校准数据。
       参数：
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-保存成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SaveDistanceSecondCalibrationData(int specHandle, int channel = 1);

       /*
       函数功能：获取指定通道的高度校准数据数组。
       参数：	
       pDistance-[out]保存获取到的高度数组。
       count-[in]pDistance的数组长度,通常等于传感器的像素长度。
       calibrationType-[in]要获取的高度校准数据类型，0表示获取传感器出厂校准数组，1表示获取传感器二次校
       准数组。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetDistanceCalibrationData(double[] pDistance, int count, int calibrationType, int specHandle, int channel = 1);

       /*
       函数功能：设置指定通道高度模式时所使用的校准数据类型,未调用此函数时默认使用出厂校准数据。
       参数：	
       dataType-[in]0表示使用出厂校准数据，1表示使用用户二次校准数据。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-设置成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetUseDistancesDataType(int dataType, int specHandle, int channel = 1);

       /*
       函数功能：获取指定通道高度模式时所使用的校准数据类型,未调用此函数时默认使用出厂校准数据。
       参数：	
       dataType-[out]保存获取到的校准数据类型。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetUseDistanceDataType(ref int dataType, int specHandle, int channel = 1);

       /******************应用测量与数据处理功能开关、参数设置相关函数*****************************/
       /*
       函数功能：开启或关闭指定通道结果数据处理功能。
       参数：	
       funcationIdx-[in]数据处理或功能编号，0-滤波功能，1-去极值功能，其他请参考数据处理功能定义表。
       bUse-[in]为true时开启，为false时关闭。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_EnableAppFuncation(int funcationIdx, bool bUse, int specHandle, int channel);

       /*
       函数功能：设置结果数据滤波功能参数。
       参数：	
       filterParam-[in]滤波参数结构体。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-设置成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetFilterParam(FilterParam filterParam, int specHandle, int channel);

       /*
       函数功能：获取结果数据滤波功能参数。
       参数：	
       filterParam-[out]用来保存获取到的滤波参数，注意指针filterParam只包含一个元素。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetFilterParam(ref FilterParam filterParam, int specHandle, int channel);

       /*
       函数功能：设置结果数据去极值功能参数。
       参数：	
       extremeDataParam-[in]去极值参数结构体。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-设置成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetExtremeParam(ExtremeDataParam extremeDataParam, int specHandle, int channel);

       /*
       函数功能：获取结果数据去极值功能参数。
       参数：	
       extremeDataParam-[out]用来保存获取到的去极值参数，注意指针extremeDataParam只包含一个元素。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetExtremeParam(ref ExtremeDataParam extremeDataParam, int specHandle, int channel);

       /*
       函数功能：获取结果数据丢帧插补功能是否开启。
       参数：	
       sta-[out]用来保存获取到的丢帧插补功能状态，true-已开启，false-已关闭。
       specHandle-[in]控制器句柄。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetFillLostFrameStatus(ref bool sta, int specHandle);

       /*
       函数功能：启用或禁用结果数据丢帧插补功能。
       参数：	
       sta-[in]true:启用控制器丢帧插补功能，false-禁用控制器丢帧插补功能，设置对象为控制器。
       specHandle-[in]控制器句柄。
       返回值：1-获取成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_EnableFillLostFrameStatus(bool sta, int specHandle);

       /*
       函数功能：滤波函数，用户给定数据，函数执行完后可获得处理后的数据，并且支持过滤给定有效高度范围外
       的值不做处理。
       参数：	
       pData-[in,out]待处理数据指针或数组。
       count-[in]pData的长度。
       filterParam-[in]滤波参数。
       RanageLow-[in]有效数据的下限。
       RanageHigh-[in]有效数据的上限。
       返回值：1-执行成功；其他返回值请参照返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_FilterEx(double[] pData, int count, FilterParam filterParam, double RanageLow, double RanageHigh);

       /*
       函数功能：使用给定高度阈值进行去极值处理。
       参数：
       pRscH-[in,out]需进行去极值的源数据。
       count-[in]pRscH的数据长度。
       EDParam-[in]去极值参数结构体。
       RanageLow-[in]有效数据的下边界。
       RanageHeight-[in]有效数据的上边界。
       exportFstDrv-[out]去极值计算出的一阶导数，算法调试用。
       exportSecDrv-[in]去极值计算出的二阶导数，算法调试用。
       返回值：成功返回1,其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_ExtremeDataProcessEx(double[] pRscH, int count, ExtremeDataParam EDParam, double RanageLow, double RanageHeight, double[] exportFstDrv = null, double[] exportSecDrv = null);

       /*
       功能：一段轨迹内使用自动寻找算法计算n点胶高。
       参数：
       pRscData-[in,out]原始高度数据数组,注意计算完胶高后，对数据的滤波去极值等处理会作用在此输入数据上。
       count-[in]原始高度数据数组长度。
       PRVal1-[out]保存计算出来的左边胶水厚度及其他相关信息。
       PRVal2-[out]保存计算出来的右边胶水厚度及其他相关信息。
       GCParam-[in]计算使用参数结构体。
       glueCount-[in]希望计算的胶水厚度数量，默认为2，当扫描出的高度截面曲线包含2个点的胶高时，此参数输
       入2，当扫描出的高度截面曲线包含1个点的胶高时，此参数输入1，此参数会决定DLL内部所调用的算法，请根
       据实际情况设置，否则可能会导致计算结果不准。
       返回值：返回已找到的胶水厚度数量。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_RoundHoleGlueCalc(double[] pRscData, int count, ref ProductResult PRVal1, ref ProductResult PRVal2, ref GlueCalcParam GCParam, int glueCount = 2);


#region ********************新版本DLL不再推荐使用函数，以下函数为旧版本DLL函数接口，新版本DLL兼容以下函数，客户新开发程序推荐使用新的替代函数。

       /*
       函数功能：[只能在控制器空闲状态下调用]开启指定通道采集指定数量的高度测量结果任务。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValues(double[] pRstHigh, int pts, int specHandle, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]开启指定通道采集指定数量的高度测量结果、波形数据任务。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       pPrdItn-[out]pts个点波形数据指针，数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得）。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValuesIntensity(double[] pRstHigh, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa, int channel = 1);

       /*
       函数功能：获取指定数量的测量结果、感光度、峰像素位置数据。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       pPeakWave-[out]pts个点波峰像素位置数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValuesPeakWave(double[] pRstHigh, int pts, int specHandle, float[] pUnitRa, float[] pPeakWave, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]获取指定数量的测量结果、波形曲线、感光度、峰像素位置数据。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       pPrdItn-[out]pts个点波形数据指针，数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得）。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       pPeakWave-[out]pts个点像素位置数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValuesIntensityPeakWave(double[] pRstHigh, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa, float[] pPeakWave, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]开启指定通道采集指定数量的高度测量结果任务,同时需要给定回
       调函数指针，当采集任务完成或者被用户主动停止后，DLL内部将执行给定回调函数。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValuesEx(double[] pRstHigh, int pts, int specHandle, CBCaptureComplete pFunction,
           int executeMode = 0, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]开启指定通道采集指定数量的高度测量结果、能量曲线数据、感光
       度数据任务，当采集任务完成或者被用户主动停止后，DLL内部将执行给定回调函数。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValuesIntensityEx(double[] pRstHigh, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa,
           CBCaptureComplete pFunction, int executeMode = 0, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]获取指定数量的测量结果、感光度、峰像素位置数据,当采集任务
       完成或者被用户主动停止后，DLL内部将执行给定回调函数。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValuesPeakWaveEx(double[] pRstHigh, int pts, int specHandle, float[] pUnitRa, float[] pPeakWave,
           CBCaptureComplete pFunction, int executeMode = 0, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]获取指定数量的测量结果、波形曲线、感光度、峰像素位置数据，
       当采集任务完成或者被用户主动停止后，DLL内部将执行给定回调函数。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_StartGetValuesIntensityPeakWaveEx(double[] pRstHigh, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa,
       float[] pPeakWave, CBCaptureComplete pFunction, int executeMode = 0, int channel = 1);

       /********以下为旧版本函数不推荐使用，适用于单通道采集与多通道采集，当使用多通道采集时，可以通过
       下列函数设置需要获取的数据类型及保存的位置，然后通过LE_StartGetChannelsValues函数来同
       步启用多个通道的采集任务*********/
       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道高度数据采集相关参数，但不开启采集任务。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetValuesParam(double[] pRstHigh, int pts, int specHandle, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道高度数据、波形曲线数据、及感光度等数据采
       集相关参数，但不开启采集任务。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       pPrdItn-[out]pts个点波形数据指针，数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得）。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetValuesIntensityParam(double[] pRstHigh, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道高度数据、波形曲线数据、感光度、像素位置
       数据采集相关参数，但不开启采集任务。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       pPrdItn-[out]pts个点波形数据指针，数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得）。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       pPeakWave-[out]pts个点波峰像素位置数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetValuesIntensityPeakWaveParam(double[] pRstHigh, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa, float[] pPeakWave, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道高度数据、波形曲线数据、感光度、像素位置
       数据采集相关参数，但不开启采集任务。
       参数：
       pRstHigh-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集高度点数。
       specHandle-[in]控制器句柄。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       pPeakWave-[out]pts个点波峰像素位置数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetValuesPeakWaveParam(double[] pRstHigh, int pts, int specHandle, float[] pUnitRa, float[] pPeakWave, int channel = 1);

       /*************************旧版本单边测厚数据获取相关函数************************/
       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定通道厚度数据采集相关参数，但不开启采集任务。
       参数：
       pRstTK-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集测量结果数量。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessParam(double[] pRstTK, int pts, int specHandle, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定通道厚度数据、波形曲线数据、及感光度等数据采集相关
       参数，但不开启采集任务。
       参数：
       pRstTK-[out]保存采集到的测量结果数据数组。
       pts-[in]期望采集测量结果数量。
       specHandle-[in]控制器句柄。
       pPrdItn-[out]pts个点波形数据指针，数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得）。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：
       1-开始采集指令成功。
       -1当前设备上一次采集任务尚未完成。
       其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessItnParam(double[] pRstTK, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa, int channel = 1);


       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道厚度数据、上下表面等高度数据采集相关参数
       ，但不开启采集任务。
       参数：
       pRstTK-[out]保存采集到的测量结果数据数组。
       pDistance1-[out]保存采集到的上表面高度数组。
       pDistance2-[out]保存采集到的下表面高度数组。
       pts-[in]期望采集测量结果数量。
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessDTParam(double[] pRstTK, double[] pDistance1, double[] pDistance2, int pts, int specHandle, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道厚度数据、上下表面高度数据、波形曲线数据
       、及感光度等数据采集相关参数，但不开启采集任务。
       参数：
       pRstTK-[out]保存采集到的测量结果数据数组。
       pDistance1-[out]保存采集到的上表面高度数组。
       pDistance2-[out]保存采集到的下表面高度数组。
       pts-[in]期望采集测量结果数量。
       specHandle-[in]控制器句柄。
       pPrdItn-[out]pts个点波形数据指针，数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得）。
       pUnitRa-[out]pts个单位像素能量数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessDTItnParam(double[] pRstTK, double[] pDistance1, double[] pDistance2, int pts, int specHandle, int[] pPrdItn, float[] pUnitRa, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道厚度数据、上下表面高度数据、波形曲线数据
       、感光度、像素位置数据采集相关参数，但不开启采集任务。
       参数：
       pRstTK-[out]保存采集到的测量结果数据数组。
       pDistance1-[out]保存采集到的上表面高度数组。
       pDistance2-[out]保存采集到的下表面高度数组。
       pts-[in]期望采集测量结果数量。
       specHandle-[in]控制器句柄。
       pPrdItn-[out]pts个点波形数据指针，数组长度为pts*length（length代表单帧光谱的长度，可以通过
       LE_GetChannelIntensityMsg函数获得）。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       pPeakWave1-[out]保存上表面波峰像素位置数组，数组长度为pts。
       pPeakWave2-[out]保存下表面波峰像素位置数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessDTItnPeakWaveParam(double[] pRstTK, double[] pDistance1, double[] pDistance2, int pts,
           int specHandle, int[] pPrdItn, float[] pUnitRa, float[] pPeakWave1, float[] pPeakWave2, int channel = 1);

       /*
       函数功能：[只能在控制器空闲状态下调用]设置指定控制器通道厚度数据、上下表面高度数据、波形曲线数据
       、感光度、波峰像素位置数据采集相关参数，但不开启采集任务。
       参数：
       pRstTK-[out]保存采集到的测量结果数据数组。
       pDistance1-[out]保存采集到的上表面高度数组。
       pDistance2-[out]保存采集到的下表面高度数组。
       pts-[in]期望采集测量结果数量。
       specHandle-[in]控制器句柄。
       pUnitRa-[out]pts个点感光度数组，数组长度为pts。
       pPeakWave1-[out]保存上表面波峰像素位置数组，数组长度为pts。
       pPeakWave2-[out]保存下表面波峰像素位置数组，数组长度为pts。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetChannelGetThicknessDTPeakWaveParam(double[] pRstTK, double[] pDistance1, double[] pDistance2, int pts, int specHandle, float[] pUnitRa, float[] pPeakWave1, float[] pPeakWave2, int channel = 1);

       /**********************************新版本废弃函数，只保留接口以便兼容旧版本DLL,函数执行将不做任
        何操作，若之前有使用到可直接删除相关代码******************************************************/
       /*
       函数功能：设置通道光斑中心位置,及单个通道光斑所占行数,当前函数于20190701已停止使用。
       输入参数：
       offsetLine-[in]待设置光斑中心位置。
       lines-[in]光斑所占行数。
       specHandle-[in]待设置控制器句柄。
       channel-[in]通道序号。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetPosOffset(int offsetLine, int lines, int specHandle, int channel);

       /*
       函数功能：采集控制器指定通道dark数据，采集前请先用黑色盖子盖住对应通道探头，直到采集完成，当前函
       数已于20170701停止使用，请使用LE_CaptureChannelsIntensityCaliData函数进行dark操作。
       参数：
       specHandle-[in]控制器句柄。
       channel-[in]通道序号。
       返回值：-16-指定功能不支持。
       备注：20190509废除，即使调用也不启任何作用！！！
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_CaptureIntensityCaliData(int specHandle, int channel = 1);

       /*
       函数功能：获取控制器指定通道调试数据是否导出开关,旧版函数，只保留接口，函数调用不起任何作用。
       参数：
       exportSta-[out]保存获取到的状态值，0-不导出，1-导出。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_GetDebugDataExportState(ref int exportSta, int specHandle);

       /*
       函数功能：设置控制器指定通道调试数据是否导出开关,旧版函数，只保留接口，函数调用不起任何作用。
       参数：
       exportSta-[in]保存获取到的状态值，0-不导出，1-导出，默认为0。
       specHandle-[in]控制器句柄。
       返回值：成功返回1，其他返回值请参考返回值定义表。
       */
       [DllImport("ConfocalDLL_x64.dll")]
       public static extern int LE_SetDebugDataExportState(int exportSta, int specHandle);
#endregion




   }
}
