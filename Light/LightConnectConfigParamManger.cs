using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Common;


namespace Light
{
    /// <summary>
    /// 用一个参数类来管理有一个好处，当在多处需要使用时，只需要加载一次，而不需要每在一个地方使用就加截一次
    /// </summary>
    public class LightConnectConfigParamManger
    {     
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static LightConnectConfigParamManger _Instance;
        public static LightConnectConfigParamManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new LightConnectConfigParamManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<LightConnectConfigParam> _LightConfigParamList;
        public BindingList<LightConnectConfigParam> LightConfigParamList { get => _LightConfigParamList; set => _LightConfigParamList = value; }


        public LightConnectConfigParam GetLightConfigParam(string lightName)
        {
            foreach (var item in _LightConfigParamList)
            {
                if (item.LightName == lightName)
                    return item;
            }
            return null;
        }
        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<LightConnectConfigParam>>.Save(_LightConfigParamList, ParaPath + "\\" + "LightConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "LightConfigParam.xml"))
                this._LightConfigParamList = XML<BindingList<LightConnectConfigParam>>.Read(ParaPath + "\\" + "LightConfigParam.xml");
            else
                this._LightConfigParamList = new BindingList<LightConnectConfigParam>();
        }



    }


}
