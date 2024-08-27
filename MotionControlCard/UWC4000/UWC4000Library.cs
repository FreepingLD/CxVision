using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MotionControlCard
{
    public class UWC4000Library
    {
        #region
        public double X_JogSpeedMax = 100;
        public double Y_JogSpeedMax = 100;
        public double Z_JogSpeedMax = 40;
        #endregion

        #region  初始化函数
        /// <summary>
        /// 该函数试图与UWC4000建立通讯连接，如果通讯建立，将初始化控制器。如果返回值非0，请查表4－2获得返回值含义
        /// </summary>
        /// <returns></returns>
        [DllImport("UWC4000.dll")]
        public static extern int uwc4000_initial();
        /// <summary>
        /// 断开与控制器的数据连接等操作
        /// </summary>
        /// <returns></returns>
        [DllImport("UWC4000.dll")]
        public static extern int uwc4000_close();
        #endregion

        #region  获取控制器状态函数
        /// <summary>
        /// 读取计数模块状态字；该函数用来检测探针数据锁存标记、MOF数据锁存标记等
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_count_status([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] status);
        /// <summary>
        /// 读取运动模块状态字,运动控制的重点在于运动模块状态字的理解，运动模块状态字含义丰富，是运动控制状态之窗
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_motion_status(ref int status);
        /// <summary>
        /// 读取X/Y/Z/ZOOM轴的轴状态字
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_axis_status(IntPtr status);
        #endregion

        #region  获取控制器信息的函数
        /// <summary>
        /// 读取各轴最大速度和最小加减速时间
        /// </summary>
        /// <param name="max_speed"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_profile([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] acc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  double[] max_speed);
        /// <summary>
        /// 读取操纵盒已去死区的偏移量、按钮、LED状态信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_joystick_Msg([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] message);

        #endregion

        #region  操作光学尺计数的函数
        /// <summary>
        /// 读取X／Y／Z轴光学尺位置（名义值）；控制器电源开启后该位置无意义，成功回原点后该位置为相对原点（光学尺RI）位置
        /// </summary>
        /// <param name="x_scale"></param>
        /// <param name="y_scale"></param>
        /// <param name="z_scale"></param>
        /// <returns></returns>

        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_scale([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] x_scale, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] y_scale, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] z_scale);
        /// <summary>
        /// 重置各轴光学尺位置值；回原点标记由于该函数会改变机械坐标，原来基于原点的工件坐标系将失效，已做好的测量程序可能失效，因此需慎用
        /// </summary>
        /// <param name="x_scale">X轴光学尺计数设置，单位为mm</param>
        /// <param name="y_scale">Y轴光学尺计数设置，单位为mm</param>
        /// <param name="z_scale">Z轴光学尺计数设置，单位为mm</param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_set_scale(double x_scale, double y_scale, double z_scale);

        #endregion

        #region  运动控制函数
        /// <summary>
        /// 令指定轴回原点，对于x／y／z轴，回原点即为找光学尺RI（又称零窗、尺中）
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="search_speed"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_go_home(int axis, double search_speed);
        /// <summary>
        ///  X／Y／Z轴：命令指定轴以绝对坐标方式定位到光学尺位置为target_pos ＋【定位精度】之内
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="speed">速度</param>
        /// <param name="target_pos">目标位置</param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_single_move_to(int axis, double speed, double target_pos);
        /// <summary>
        /// 设置各轴定位完成窗口大小（定位精度）。
        /// </summary>
        /// <param name="precision">定位误差</param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_set_inposition_precision([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] precision);
        /// <summary>
        /// 令XY轴按照直线插补运动规律运动到某处
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="target_pos"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_XY_move_to(double speed, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] target_pos);

        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_arc_move_xy(double cent_pos1, double cent_pos2, double line_speed, double rotary_angle);

        // 
        /// <summary>
        /// 令XYZ轴按照直线插补运动规律运动到某处
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="target_pos"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_XYZ_move_to(double speed, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] target_pos);
        /// <summary>
        /// 使指定轴开始JOG模式运动，该函数指定了本次启动的jog运动所允许的最大速度
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="jog_speed_max"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_jog_start(int axis, double jog_speed_max);
        /// <summary>
        /// jog_start函数调用之后、uwc4000_ stop调用之前，连续不断调用该函数以达到改变jog运动速度和方向的目的
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="curnt_speed">目标速度,该参数为正时，机台朝正向运动，为负时朝负向运动</param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_change_speed(int axis, double curnt_speed);
        /// <summary>
        /// 令所有处于JOG状态的轴减速停止。该函数任何时候调用、重复都不会报错
        /// </summary>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_jog_stop();
        /// <summary>
        /// 令指定轴停止运动
        /// </summary>
        /// <param name="axis">轴呈</param>
        /// <param name="mode">停止模式,0:减速停止;1:立即停止</param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_stop(int axis, int mode);
        /// <summary>
        /// 通知控制器，允许操纵杆控制机台运动
        /// </summary>
        /// <param name="low_speed"></param>
        /// <param name="high_speed"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_enable_joystick([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] low_speed, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] high_speed);
        /// <summary>
        /// 通知控制器，无论任何轴处于任何状态，扳动操纵杆都不要让机台产生运
        /// </summary>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_disable_joystick();

        #endregion

        #region  IO控制相头函数
        /// <summary>
        /// 读取输入口电平状态。空气压力检测开关等输入信号可连接到普通输入口，输入口还可以用来实现在线测量时与其他设备进行握手信号检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_input([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] input);
        /// <summary>
        /// 设置输出口状态。输出口可用于声光提示设备等，还可以用来实现在线测量时输出握手信号给其他设备。 输出口电路为三极管集电极开路输出，控制器电源开启
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_set_output( uint  output); //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
        #endregion

        #region  异常处理函数
        /// <summary>
        /// 用于读取最后一次的出错信息代码
        /// </summary>
        /// <param name="err_code"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_get_last_err([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] err_code);
        /// <summary>
        /// 超级指令。通过超级指令，可以对控制器内任何接口进行读写访问
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="Slen"></param>
        /// <param name="Rlen"></param>
        /// <param name="ucIn"></param>
        /// <param name="ucOut"></param>
        /// <returns></returns>
        [DllImport("UWC4000.dll", SetLastError = true)]
        public static extern int uwc4000_super_command(uint ch, int Slen, int Rlen, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] char[] ucIn, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] char[] ucOut);
        #endregion

        #region   C#调用C++编写的DLL函数, 以及各种类型的参数传递  


        //C/C++ Code Copy Code To Clipboard

        ////C++中的输出函数 
        //int __declspec(dllexport) test(const int N) 
        //{ 
        //return N+10; 
        //}
        //    对应的C#代码为:
        //C# Code Copy Code To Clipboard

        //[DllImport("test.dll", EntryPoint = "#1")]
        //public static extern int test(int m);

        //    private void button1_Click(object sender, EventArgs e)
        //    {
        //        textBox1.Text = test(10).ToString();
        //    } 

        //2. 如果函数有传出参数,比如:
        ////C++
        //void __declspec(dllexport) test(const int N, int& Z)
        //    {
        //        Z = N + 10;
        //    }

        //    对应的C#代码:
        //[DllImport("test.dll", EntryPoint = "#1")]
        //public static extern double test(int m, ref int n);

        //    private void button1_Click(object sender, EventArgs e)
        //    {
        //        int N = 0;
        //        test1(10, ref N);
        //        textBox1.Text = N.ToString();
        //    } 

        //3. 带传入数组:
        //void __declspec(dllexport) test(const int N, const int n[], int& Z)
        //    {
        //        for (int i = 0; i < N; i++)
        //        {
        //            Z += n[i];
        //        }
        //    }

        //    C#代码:
        //[DllImport("test.dll", EntryPoint = "#1")]
        //public static extern double test(int N, int[] n, ref int Z);

        //    private void button1_Click(object sender, EventArgs e)
        //    {
        //        int N = 0;
        //        int[] n;
        //        n = new int[10];
        //        for (int i = 0; i < 10; i++)
        //        {
        //            n[i] = i;
        //        }
        //        test(n.Length, n, ref N);
        //        textBox1.Text = N.ToString();
        //    } 

        //4. 带传出数组:
        //C++不能直接传出数组,只传出数组指针,
        //void __declspec(dllexport) test(const int M, const int n[], int* N)
        //    {
        //        for (int i = 0; i < M; i++)
        //        {
        //            N[i] = n[i] + 10;
        //        }
        //    }

        //    对应的C#代码:
        //[DllImport("test.dll", EntryPoint = "#1")]
        //public static extern void test(int N, int[] n, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] Z);

        //    private void button1_Click(object sender, EventArgs e)
        //    {
        //        int N = 1000;
        //        int[] n, Z;
        //        n = new int[N]; Z = new int[N];
        //        for (int i = 0; i < N; i++)
        //        {
        //            n[i] = i;
        //        }
        //        test(n.Length, n, Z);
        //        for (int i = 0; i < Z.Length; i++)
        //        {
        //            textBox1.AppendText(Z[i].ToString() + "n");
        //        }
        //    }

        //    这里声明函数入口时,注意这句[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] Z

        //在C#中数组是直接使用的,而在C++中返回的是数组的指针,这句用来转化这两种不同的类型.

        //关于MarshalAs的参数用法以及数组的Marshaling, 可以参见这篇转帖的文章: http://www.kycis.com/blog/read.php?21

        //5. 传出字符数组：
        //C/C++ Code Copy Code To Clipboard

        //void __declspec(dllexport) test(int i, double &a, double &b, char t[5])

        //C#对应声明：
        //[DllImport("dll.dll", EntryPoint = "test")]
        //public static extern void test(int i, ref double a, ref double b, [Out, MarshalAs(UnmanagedType.LPArray)] char[] t);    
        //            char[] t = new char[5];
        //            test(i, ref a, ref b, t);

        //  字符数组的传递基本与4相似，只是mashalAs 时前面加上Out。

        #endregion

        #region  DllImport使用说明 
        //DllImport具有6个命名参数：

        //a、CallingConvention参数：指示入口点的调用约定，如果未指定CallingConvention，则使用默认值CallingConvention.Winapi；

        //b、CharSet参数：指示用在入口点种的字符集。如果未指定CharSet，则使用默认值CharSet.Auto；

        //c、EntryPoint参数：给出所声明的方法在dll中入口点的名称。如果未指定EntryPoint，则使用方法本身的名称；

        //d、ExactSpelling参数：指示EntryPoint是否必须与指示的入口点的拼写完全匹配。如果未指定ExactSpelling，则使用默认值false；

        //e、PreserveSig参数：指示方法的签名应被应当被保留还是被转换。当签名被转换时，它被转换为一个具有HRESULT返回值和该返回值的一个名为retval的附加输出参数签名。如果未指定PreserveSig，则使用默认值false；

        //f、SetLastError参数：指示方法是否保留Win32上的错误，如果未指定SetLastError，则使用默认值false

        // MarshalAs特性是指示如何在托管代码和非托管代码之间封送数据的，
        // 后边的枚举类型UnmanagedType，是说如何将此参数封送到非托管代码
        //MarshalAs这个系统库在VS中是可以跟踪的，你看一下就懂了
        #endregion
    }
}
