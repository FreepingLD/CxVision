﻿using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class SystemParamManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static SystemParamManager _Instance;
        public static SystemParamManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new SystemParamManager();
                    }
                }
                return _Instance;
            }
        }

        private SystemParam _sysConfigParam = new SystemParam();
        public SystemParam SysConfigParam { get => _sysConfigParam; set => _sysConfigParam = value; }






        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<SystemParam>.Save(_sysConfigParam, ParaPath + "\\" + "SystemParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "SystemParam.xml"))
                this._sysConfigParam = XML<SystemParam>.Read(ParaPath + "\\" + "SystemParam.xml");
            else
                this._sysConfigParam = new SystemParam();
        }


    }
}
