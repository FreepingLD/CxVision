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
    public class ProgramManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static ProgramManager _Instance;
        public static ProgramManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new ProgramManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<ProgramParam> _programList = new BindingList<ProgramParam>();
        public BindingList<ProgramParam> ProgramList { get => _programList; set => _programList = value; }



        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<ProgramParam>>.Save(_programList, ParaPath + "\\" + "ProgramParam.xml"); // 以类名作为文件名
            return IsOk;
        }

        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "ProgramParam.xml"))
                this._programList = XML<BindingList<ProgramParam>>.Read(ParaPath + "\\" + "ProgramParam.xml");
            else
                this._programList = new BindingList<ProgramParam>();
            if(this._programList == null)
                this._programList = new BindingList<ProgramParam>();
        }




    }
}
