using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Common;
using HalconDotNet;

namespace Sensor
{
    /// <summary>
    /// 用一个参数类来管理有一个好处，当在多处需要使用时，只需要加载一次，而不需要每在一个地方使用就加截一次
    /// </summary>
    public class SensorConnectConfigParamManger
    {     
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static SensorConnectConfigParamManger _Instance;
        public static SensorConnectConfigParamManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new SensorConnectConfigParamManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<SensorConnectConfigParam> _ConfigParamList;
        public BindingList<SensorConnectConfigParam> ConfigParamList { get => _ConfigParamList; set => _ConfigParamList = value; }


        public string [] GetSensorName()
        {
            string[] name = new string[_ConfigParamList.Count];
            for (int i = 0; i < _ConfigParamList.Count; i++)
            {
                name[i] = _ConfigParamList[i].SensorName;
            }
            return name;
        }

        public SensorConnectConfigParam GetSensorConfigParam(string sensorName)
        {
            foreach (var item in _ConfigParamList)
            {
                if (item.SensorName == sensorName)
                    return item;
            }
            return null;
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<SensorConnectConfigParam>>.Save(_ConfigParamList, ParaPath + "\\" + "SensorConnectConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "SensorConnectConfigParam.xml"))
                this._ConfigParamList = XML<BindingList<SensorConnectConfigParam>>.Read(ParaPath + "\\" + "SensorConnectConfigParam.xml");
            else
                this._ConfigParamList = new BindingList<SensorConnectConfigParam>();
        }
    }


}
