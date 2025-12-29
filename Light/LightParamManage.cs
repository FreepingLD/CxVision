using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    /// <summary>
    /// 这个类不需要使用
    /// </summary>
    public class LightParamManage
    {
        private static string ParaPath = @"光源参数"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static LightParamManage _Instance;
        public static LightParamManage Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new LightParamManage();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<LightParam> _lightParamList;
        public BindingList<LightParam> LightParamList
        {
            get
            {
                return _lightParamList;
            }
            set
            {
                _lightParamList = value;
            }
        }


    }
}
