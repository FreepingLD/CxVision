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
    public class ProgramConfigParamManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static ProgramConfigParamManager _Instance;
        public static ProgramConfigParamManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new ProgramConfigParamManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<ProgramConfigParam> _programParamList = new BindingList<ProgramConfigParam>();
        public BindingList<ProgramConfigParam> ProgramParamList { get => _programParamList; set => _programParamList = value; }


        public ProgramConfigParam GetParam(string name)
        {
            foreach (var item in _programParamList)
            {
                if (item.Name == name) return item;
            }
            return null;
        }

        public void SetValue(string propertyName,object value)
        {
            foreach (var item in _programParamList)
            {
                switch(propertyName)
                {
                    case "ProgramPath":
                        item.ProgramPath = value.ToString();
                        break;
                    case "IsAuto":
                        item.IsAuto = (bool)value;
                        break;
                }
            }
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<ProgramConfigParam>>.Save(_programParamList, ParaPath + "\\" + "ProgramConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }

        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "ProgramConfigParam.xml"))
                this._programParamList = XML<BindingList<ProgramConfigParam>>.Read(ParaPath + "\\" + "ProgramConfigParam.xml");
            else
                this._programParamList = new BindingList<ProgramConfigParam>();
            if(this._programParamList == null)
                this._programParamList = new BindingList<ProgramConfigParam>();
        }




    }
}
