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
    public class MeasureManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static MeasureManager _Instance;
        public static MeasureManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new MeasureManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<MeasureParam> _measureList = new BindingList<MeasureParam>();
        public BindingList<MeasureParam> MeasureList { get => _measureList; set => _measureList = value; }



        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<MeasureParam>>.Save(_measureList, ParaPath + "\\" + "MeasureParam.xml"); // 以类名作为文件名
            return IsOk;
        }

        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "MeasureParam.xml"))
                this._measureList = XML<BindingList<MeasureParam>>.Read(ParaPath + "\\" + "MeasureParam.xml");
            else
                this._measureList = new BindingList<MeasureParam>();
            if(this._measureList == null)
                this._measureList = new BindingList<MeasureParam>();
        }




    }
}
