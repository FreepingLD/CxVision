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
    public class ReportDataManage
    {
        private static string SavePath = @"报表数据"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static ReportDataManage _Instance;
        public static ReportDataManage Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new ReportDataManage();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<ReportData> _ReportParamList;
        public BindingList<ReportData> ReportParamList
        {
            get
            {
                return _ReportParamList;
            }
            set
            {
                _ReportParamList = value;
            }
        }
        public bool Save(string Path = "")
        {
            bool IsOk = true;
            if (Path == null || Path.Trim().Length == 0)
            {
                string dic = new FileInfo(Path).DirectoryName;
                if (!DirectoryEx.Exist(new FileInfo(Path).DirectoryName)) DirectoryEx.Create(new FileInfo(Path).DirectoryName);
                IsOk = IsOk && XML<BindingList<ReportData>>.Save(_ReportParamList, Path); // 以类名作为文件名
            }
            else
            {
                string dic = new FileInfo(SavePath).DirectoryName;
                if (!DirectoryEx.Exist(new FileInfo(SavePath).DirectoryName)) DirectoryEx.Create(new FileInfo(SavePath).DirectoryName);
                IsOk = IsOk && XML<BindingList<ReportData>>.Save(_ReportParamList, SavePath + "\\" + "ReportData" + ".xml"); // 以类名作为文件名
            }
            return IsOk;
        }
        public void Read(string path)
        {
            if (path == null || path.Length == 0 || !File.Exists(path))
            {
                this._ReportParamList = XML<BindingList<ReportData>>.Read(SavePath + "ReportData" + ".xml");
            }
            else
            {
                this._ReportParamList = XML<BindingList<ReportData>>.Read(path);
            }
            if (this._ReportParamList == null)
                this._ReportParamList = new BindingList<ReportData>();
        }

    }
}
