using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 将那些会变化，不能保存的变量定义为全局变量，在软件初始化是，初始化他们
    /// </summary>
    public class GlobalVariable
    {
        public static object monitor = new object();
        public static ParamConfig pConfig;
        public static Dictionary<string, HWindow> windowID = new Dictionary<string, HWindow>();


    }
}
