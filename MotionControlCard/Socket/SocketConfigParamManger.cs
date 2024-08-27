using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class SocketConfigParamManger
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static SocketConfigParamManger _Instance;
        public static SocketConfigParamManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new SocketConfigParamManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<SocketParam> _socketConfigParamList = new BindingList<SocketParam>();
        public BindingList<SocketParam> SocketConfigParamList { get => _socketConfigParamList; set => _socketConfigParamList = value; }


        public SocketParam GetSocketConfigParam(string ip)
        {
            foreach (var item in _socketConfigParamList)
            {
                if (item.IpAdress == ip)
                    return item;
            }
            return null;
        }
        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<SocketParam>>.Save(_socketConfigParamList, ParaPath + "\\" + "SocketConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "SocketConfigParam.xml"))
                this._socketConfigParamList = XML<BindingList<SocketParam>>.Read(ParaPath + "\\" + "SocketConfigParam.xml");
            else
                this._socketConfigParamList = new BindingList<SocketParam>();
        }




    }
}
