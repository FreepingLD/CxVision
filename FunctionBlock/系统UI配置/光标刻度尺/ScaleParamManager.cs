using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FunctionBlock
{
    public class ScaleParamManager
    {
        private static string ParaPath = @"VisionParam\ConfigParam";
        private static object sycnObj = new object();
        private static ScaleParamManager _Instance;
        public static ScaleParamManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new ScaleParamManager();
                    }
                }
                return _Instance;
            }
        }

        private ScaleParam _param = new ScaleParam();
        public ScaleParam Param { get => _param; set => _param = value; }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<ScaleParam>.Save(_param, ParaPath + "\\" + "ScaleParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "ScaleParam.xml"))
                this._param = XML<ScaleParam>.Read(ParaPath + "\\" + "ScaleParam.xml");
            else
                this._param = new ScaleParam();
            if (this._param == null)
                this._param = new ScaleParam();
        }




    }
}
