using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class ScreenManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static ScreenManager _Instance;
        public static ScreenManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new ScreenManager();
                    }
                }
                return _Instance;
            }
        }

        private ScreenConfigParam _ScreenParam = new ScreenConfigParam();
        public ScreenConfigParam ScreenParam { get => _ScreenParam; set => _ScreenParam = value; }






        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<ScreenConfigParam>.Save(_ScreenParam, ParaPath + "\\" + "ScreenConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "ScreenConfigParam.xml"))
                this._ScreenParam = XML<ScreenConfigParam>.Read(ParaPath + "\\" + "ScreenConfigParam.xml");
            else
                this._ScreenParam = new ScreenConfigParam();
        }


    }
}
