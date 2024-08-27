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
    public class AlignDataManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static AlignDataManager _Instance;
        public static AlignDataManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new AlignDataManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<AlignResultInfo> _alignData = new BindingList<AlignResultInfo>();
        public BindingList<AlignResultInfo> AlignData { get => _alignData; set => _alignData = value; }




        public bool Save(string name = "")
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<AlignResultInfo>>.Save(_alignData, ParaPath + "\\" + "AlignResultInfo_" + name + ".xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read(string name = "")
        {
            if (File.Exists(ParaPath + "\\" + "AlignResultInfo_" + name + ".xml"))
                this._alignData = XML<BindingList<AlignResultInfo>>.Read(ParaPath + "\\" + "AlignResultInfo_" + name + ".xml");
            else
                this._alignData = new BindingList<AlignResultInfo>();
            if (this._alignData == null)
                this._alignData = new BindingList<AlignResultInfo>();
        }


    }
}
