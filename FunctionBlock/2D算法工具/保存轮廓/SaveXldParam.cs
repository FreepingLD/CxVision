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

namespace AlgorithmsLibrary
{
    [Serializable]
    public class SaveXldParam
    {
        private string folderPath;
        private string extendName = ".dxf";
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


        public bool SaveXld(XldDataClass xldData)
        {
            bool result = false;
            string date = DateTime.Now.ToString("yyyyMMddhhmmss");
            if (xldData == null || xldData.HXldCont == null)
            {
                throw new ArgumentNullException("xldData");
            }
            if (addDataTime)
            {
                result = this.WriteXLDToFile(xldData.HXldCont, this.folderPath + "\\" + this.FileName + date + this.extendName);
            }
            else
            {
                string name = this.folderPath + "\\" + this.FileName + this.extendName;
                result = this.WriteXLDToFile(xldData.HXldCont, this.folderPath + "\\" + this.FileName + this.extendName);
            }
            return result;
        }
        private bool WriteXLDToFile(object objectModel, string path)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
            string[] extenName = path.Split('.');
            //////////////////////
            if (objectModel == null) return result;
            ////////////////////////
            switch (objectModel.GetType().Name)
            {
                case "XldDataClass":
                    if (((XldDataClass)objectModel).HXldCont == null) return result;
                    if (extenName[1] == "dxf")
                        HOperatorSet.WriteContourXldDxf(((XldDataClass)objectModel).HXldCont, path);
                    break;
                case "HXLDCont":
                    if (((HXLDCont)objectModel) == null) return result;
                    if (extenName[1] == "dxf")
                        HOperatorSet.WriteContourXldDxf(((HXLDCont)objectModel), path);
                    break;
                case "HXLDPoly":
                    if (((HXLDPoly)objectModel) == null) return result;
                    if (extenName[1] == "dxf")
                        HOperatorSet.WritePolygonXldDxf(((HXLDPoly)objectModel), path);
                    break;
            }
            result = true;
            return result;
        }


    }
}
