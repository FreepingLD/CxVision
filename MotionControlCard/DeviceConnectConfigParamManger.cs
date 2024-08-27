using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Common;
using HalconDotNet;

namespace MotionControlCard
{
    /// <summary>
    /// 用一个参数类来管理有一个好处，当在多处需要使用时，只需要加载一次，而不需要每在一个地方使用就加截一次
    /// </summary>
    public class DeviceConnectConfigParamManger
    {     
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static DeviceConnectConfigParamManger _Instance;
        public static DeviceConnectConfigParamManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new DeviceConnectConfigParamManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<DeviceConnectConfigParam> _DeviceConfigParamList;
        public BindingList<DeviceConnectConfigParam> DeviceConfigParamList { get => _DeviceConfigParamList; set => _DeviceConfigParamList = value; }


        public DeviceConnectConfigParam GetDeviceConfigParam(string deviceName)
        {
            foreach (var item in _DeviceConfigParamList)
            {
                if (item.DeviceName == deviceName)
                    return item;
            }
            return null;
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<DeviceConnectConfigParam>>.Save(_DeviceConfigParamList, ParaPath + "\\" + "DeviceConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "DeviceConfigParam.xml"))
                this._DeviceConfigParamList = XML<BindingList<DeviceConnectConfigParam>>.Read(ParaPath + "\\" + "DeviceConfigParam.xml");
            else
                this._DeviceConfigParamList = new BindingList<DeviceConnectConfigParam>();
            if(this._DeviceConfigParamList == null)
                this._DeviceConfigParamList = new BindingList<DeviceConnectConfigParam>();
        }


    }


}
