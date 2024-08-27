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
    public class OperateManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static OperateManager _Instance;
        public static OperateManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new OperateManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<OperateParam> _programList = new BindingList<OperateParam>();
        public BindingList<OperateParam> ProgramList { get => _programList; set => _programList = value; }

        public OperateParam GetParam(string ProductSize)
        {
            OperateParam param = null;
            foreach (var item in _programList)
            {
                if (item.ProductSize == ProductSize)
                    param = item;
            }
            return param;
        }


        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<OperateParam>>.Save(_programList, ParaPath + "\\" + "OperateParam.xml"); // 以类名作为文件名
            for (int i = 0; i < _programList.Count; i++)
            {
                if(_programList[i].Image != null && _programList[i].Image.IsInitialized())
                _programList[i].Image.WriteImage("bmp",0,ParaPath + "\\" + "OperateParam_Image" +i.ToString() + ".bmp");
            }
            return IsOk;
        }

        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "OperateParam.xml"))
                this._programList = XML<BindingList<OperateParam>>.Read(ParaPath + "\\" + "OperateParam.xml");
            else
                this._programList = new BindingList<OperateParam>();
            if(this._programList == null)
                this._programList = new BindingList<OperateParam>();
            /////////////////////////////////////////////
            for (int i = 0; i < _programList.Count; i++)
            {
                if(File.Exists(ParaPath + "\\" + "OperateParam_Image" + i.ToString() + ".bmp"))
                {
                    _programList[i].Image = new HalconDotNet.HImage();
                    _programList[i].Image.ReadImage(ParaPath + "\\" + "OperateParam_Image" + i.ToString() + ".bmp");
                }
            }
        }




    }
}
