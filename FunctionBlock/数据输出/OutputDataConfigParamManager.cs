using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace FunctionBlock
{
    public class OutputDataConfigParamManager
    {
        private static int headCount = 0;
        private static string ParaPath = @"报表模板"; 
        private static object sycnObj = new object();
        private static OutputDataConfigParamManager _Instance;
        public static OutputDataConfigParamManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new OutputDataConfigParamManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<OutputDataConfigParam> _DataItemParamList = new BindingList<OutputDataConfigParam>();

        public BindingList<OutputDataConfigParam> DataItemParamList { get => _DataItemParamList; set => _DataItemParamList = value; }
        public static int HeadCount { get => headCount; set => headCount = value; }


        //public OutputDataConfigParam GetMaxValue(int indexValue)
        //{
        //    if (this._DataItemParamList == null || this._DataItemParamList.Count == 0) return null;
        //    Type type = this._DataItemParamList[0].GetType();
        //    PropertyInfo[] propertyInfos = type.GetProperties();
        //    if (indexValue > propertyInfos.Length - 1) return null;
        //    double temp = double.MaxValue;
        //    foreach (var item in this._DataItemParamList)
        //    {

        //    }
        //}
        public bool FindConfigParam(string findName)
        {
            foreach (var item in this._DataItemParamList)
            {
                if (item.DataItem1.ToString() == findName) return true;
            }
            return false;
        }
        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<OutputDataConfigParam>>.Save(_DataItemParamList, ParaPath + "\\" + "OutputDataConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public bool Save(string ParaPath)
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<OutputDataConfigParam>>.Save(_DataItemParamList, ParaPath + "\\" + "OutputDataConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "OutputDataConfigParam.xml"))
                this._DataItemParamList = XML<BindingList<OutputDataConfigParam>>.Read(ParaPath + "\\" + "OutputDataConfigParam.xml");
            else
                this._DataItemParamList = new BindingList<OutputDataConfigParam>();
           //////////////////////////////////
           if(this._DataItemParamList == null)
                this._DataItemParamList = new BindingList<OutputDataConfigParam>();
        }
        public void Read(string ParaPath)
        {
            if (File.Exists(ParaPath + "\\" + "OutputDataConfigParam.xml"))
                this._DataItemParamList = XML<BindingList<OutputDataConfigParam>>.Read(ParaPath + "\\" + "OutputDataConfigParam.xml");
            else
                this._DataItemParamList = new BindingList<OutputDataConfigParam>();
            //////////////////////////////////
            if (this._DataItemParamList == null)
                this._DataItemParamList = new BindingList<OutputDataConfigParam>();
        }


    }
}
