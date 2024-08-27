using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class SaveDataPlcParam
    {
        private string _childDolder = "";
        private string _saveFolderPath;
        private string extendName = ".csv";
        private bool addDataTime = false;
        private string fileName = "AoiData";

        public string SaveFtpFolderPath
        {
            get;
            set;
        }
        public string SaveFolderPath
        {
            get
            {
                return _saveFolderPath;
            }

            set
            {
                _saveFolderPath = value;
            }
        }
        public string ExtendName
        {
            get
            {
                return extendName;
            }

            set
            {
                extendName = value;
            }
        }
        public bool AddDataTime
        {
            get
            {
                return addDataTime;
            }

            set
            {
                addDataTime = value;
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }

        public string FilePath
        {
            get;
            set;
        }

        public enSaveMethod SaveMethod { get; set; }

        public bool IsSaveFtpData
        {
            get;
            set;
        }
        public bool IsSavePanelFormat
        {
            get;
            set;
        }
        public bool IsSaveDmyFormat
        {
            get;
            set;
        }
        public SaveDataPlcParam()
        {
            this.SaveMethod = enSaveMethod.按天保存;
            this.AddDataTime = true;
            this.IsSaveFtpData = false;
        }

        public bool WriteData(string panel_ID, string[] imagePath, string[] title, object[] data, string[] state)
        {
            bool result = false;
            try
            {
                string path_csv = "", path_txt = "";
                if (this.SaveFolderPath == null)
                    this.SaveFolderPath = "D:\\AoiData";
                if (!Directory.Exists(this.SaveFolderPath))
                    Directory.CreateDirectory(this.SaveFolderPath);
                switch (this.SaveMethod)
                {
                    default:
                    case enSaveMethod.按天保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMdd");
                        if (!Directory.Exists(this.SaveFolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.SaveFolderPath + "\\" + this._childDolder);
                        break;
                    case enSaveMethod.按周保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMww");
                        if (!Directory.Exists(this.SaveFolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.SaveFolderPath + "\\" + this._childDolder);
                        break;
                    case enSaveMethod.按小时保存:
                        this._childDolder = DateTime.Now.ToString("yyyyMMddhh");
                        if (!Directory.Exists(this.SaveFolderPath + "\\" + this._childDolder))
                            Directory.CreateDirectory(this.SaveFolderPath + "\\" + this._childDolder);
                        break;
                }
                path_csv = this.SaveFolderPath + "\\" + this._childDolder + "\\" + this.fileName + this.ExtendName;
                path_txt = this.SaveFolderPath + "\\" + this._childDolder + "\\" + this.fileName + ".txt";
                this.FilePath = path_csv;
                List<string> list = new List<string>();
                List<string> titelList = new List<string>();
                bool fileExist = true;
                if (!File.Exists(path_csv)) // 如果文件不存在，则在第一次创建文件时写入列头
                    fileExist = false;
                using (StreamWriter sw = new StreamWriter(path_csv, true))
                {
                    list.Clear();
                    if (this.AddDataTime)
                    {
                        titelList.Add("DataTime");
                        list.Add(DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss"));
                    }
                    //////  添加列头 //////////////
                    if (!fileExist)
                    {
                        foreach (var item in title)
                        {
                            titelList.Add(item.ToString());
                        }
                        sw.WriteLine(string.Join(",", titelList.ToArray()));
                    }
                    //////////////////////////////
                    foreach (var item in data)
                    {
                        list.Add(item.ToString());
                    }
                    sw.WriteLine(string.Join(",", list.ToArray()));
                }
                //////////////////////
                using (StreamWriter sw = new StreamWriter(path_txt, true, Encoding.UTF8))
                {
                    list.Clear();
                    if (this.AddDataTime)
                    {
                        titelList.Add("DateTime");
                        list.Add(DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss"));
                    }
                    //////  添加列头 //////////////
                    if (!fileExist)
                    {
                        foreach (var item in title)
                        {
                            titelList.Add(item.ToString());
                        }
                        sw.WriteLine(string.Join(",", titelList.ToArray()));
                    }
                    //////////////////////////////
                    foreach (var item in data)
                    {
                        list.Add(item.ToString());
                    }
                    sw.WriteLine(string.Join(",", list.ToArray()));
                }

                if (this.IsSaveFtpData && this.SaveFtpFolderPath != null)
                {
                    using (StreamWriter sw = new StreamWriter(this.SaveFtpFolderPath + "\\" + panel_ID + ".txt", false))
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            sw.WriteLine(string.Join("^", (i + 1).ToString(), title[i + 1], data[i], state[i], imagePath[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error("保存文件出错" + ex.ToString());
            }
            result = true;
            return result;
        }
    }

    public enum enSaveMethod
    {
        按天保存,
        按小时保存,
        按周保存,
    }
}
