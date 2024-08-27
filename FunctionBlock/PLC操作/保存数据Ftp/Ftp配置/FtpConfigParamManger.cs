using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Common;

namespace FunctionBlock
{
    /// <summary>
    /// 用一个参数类来管理有一个好处，当在多处需要使用时，只需要加载一次，而不需要每在一个地方使用就加截一次
    /// </summary>
    public class FtpConfigParamManger
    {     
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static FtpConfigParamManger _Instance;
        public static FtpConfigParamManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new FtpConfigParamManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<FtpConfigParam> _FtpConfigParamList;
        public BindingList<FtpConfigParam> FtpConfigParamList { get => _FtpConfigParamList; set => _FtpConfigParamList = value; }



        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<FtpConfigParam>>.Save(_FtpConfigParamList, ParaPath + "\\" + "FtpConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "FtpConfigParam.xml"))
                this._FtpConfigParamList = XML<BindingList<FtpConfigParam>>.Read(ParaPath + "\\" + "FtpConfigParam.xml");
            else
                this._FtpConfigParamList = new BindingList<FtpConfigParam>();
            if(this._FtpConfigParamList == null)
                this._FtpConfigParamList = new BindingList<FtpConfigParam>();
        }





    }


}
