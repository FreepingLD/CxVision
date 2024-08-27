using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;

namespace FunctionBlock
{
    [Serializable]
    public class SaveDataParam
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
        public enWriteMode WriteMode
        {
            get;
            set;
        }

        public string Split
        {
            get;
            set;
        }
        public SaveDataParam()
        {
            this.FolderPath = "";
            this.ExtendName = ".txt";
            this.AddDataTime = false;
            this.FileName = "";
            this.WriteMode = enWriteMode.追加;
            this.Split = ",";
        }

        /// <summary>
        /// 保存3D对象模型到文件中，支持的文件格式“Txt", 'om3', 'dxf', 'off', 'ply', 'obj', 'stl'
        /// </summary>

        public bool SaveFileData(params double[] data)
        {
            bool result = false;
            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }
                string path = "";
                if (this.AddDataTime)
                {
                    path = this.FolderPath + "\\" + this.FileName + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + this.ExtendName;
                }
                else
                {
                    path = this.FolderPath + "\\" + this.FileName + this.ExtendName;
                }
                //////////////////////
                switch(this.WriteMode)
                {
                    case enWriteMode.覆盖:
                        using (StreamWriter sw = new StreamWriter(this.FileName, false))
                        {
                                sw.WriteLine(string.Join(this.Split, data));
                        }
                        break;
                    default:
                    case enWriteMode.追加:
                        using (StreamWriter sw = new StreamWriter(this.FileName, true))
                        {
                            sw.WriteLine(string.Join(this.Split, data));
                        }
                        break;
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

    }

    public enum enWriteMode
    {
        None = 0,
        覆盖,
        追加,
    }



}
