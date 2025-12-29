using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ReportConfigParam
    {
        private static string SavePath = @"ConfigParam"; // 
        private static object sycnObj = new object();
        private static ReportConfigParam _Instance;
        public static ReportConfigParam Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new ReportConfigParam();
                    }
                }
                return _Instance;
            }
        }


        public int ImageWidth { get; set; }
        public int Imageheight { get; set; }
        public string FilePath { get; set; }
        public string FolderPath
        {
            get;
            set;
        }
        public enSaveType SaveType
        {
            get;
            set;
        }
        public enSaveType DisplayType
        {
            get;
            set;
        }
        public bool AddDataTime
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public ReportConfigParam()
        {
            this.FolderPath = "D:\\测量数据";
            this.ImageWidth = 200;
            this.Imageheight = 200;
            this.FilePath = "";
            this.SaveType = enSaveType.NG; // 默认只保存NG
            this.ProductName = "123";
            this.AddDataTime = true;
            this.DisplayType = enSaveType.NG;
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            IsOk = IsOk && XML<ReportConfigParam>.Save(this, SavePath + @"\" + nameof(ReportConfigParam) + ".xml");
            return IsOk;
        }
        public ReportConfigParam Read()
        {
            ReportConfigParam cameraParam = null;
            if (File.Exists(SavePath + @"\" + nameof(ReportConfigParam) + ".xml"))
            {
                cameraParam = XML<ReportConfigParam>.Read(SavePath + @"\" + nameof(ReportConfigParam) + ".xml");
            }
            else
                cameraParam = new ReportConfigParam();
            if (cameraParam == null) cameraParam = new ReportConfigParam();
            ////////////////////////////////////////////
            return cameraParam;
        }

    }

    public enum enSaveType
    {
        All,
        OK,
        NG
    }
}
