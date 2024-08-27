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
    public class WriteFileParam
    {
        private string folderPath;
        private string extendName = ".txt";
        private bool addDataTime = false;
        private string fileName;
        public string FolderPath
        {
            get
            {
                return folderPath;
            }

            set
            {
                folderPath = value;
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


        public bool WriteFile(CoordPoint[] coordPoints)
        {
            bool result = false;
            try
            {
                string path = "";
                if (this.AddDataTime)
                {
                    path = this.FolderPath + "\\" + this.FileName + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + this.ExtendName;
                }
                else
                {
                    path = this.FolderPath + "\\" + this.FileName + this.ExtendName;
                }
                this.FilePath = path;   
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (var item in coordPoints)
                    {
                        if (item.Sign.Contains("Row") && item.Count > 0)
                        {
                            if (item.Sign.Contains("Row"))
                                sw.WriteLine("{0}", item.Sign);
                            if (item.Count > 0)
                                sw.WriteLine("{0}", "Count="+item.Count);
                            for (int i = 0; i < item.X.Length; i++)
                            {
                                sw.WriteLine("{0},{1},{2},{3}", item.Row[i], item.Col[i], item.X[i], item.Y[i]);
                            }
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
}
