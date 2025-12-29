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
using System.Runtime.InteropServices;

namespace FunctionBlock
{
    [Serializable]
    public class ReadObjectModelParam
    {
        private List<string> filePath = new List<string>();
        private string singleFilePath = "";
        private string folderPath = "";
        public string SingleFilePath
        {
            get
            {
                return singleFilePath;
            }

            set
            {
                singleFilePath = value;
            }
        }
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


        public List<string> FilePath { get => filePath; set => filePath = value; }
        public double Unit { get; set; }
        public int Index { get; set; }
        public ReadObjectModelParam()
        {
            this.Unit = 1;
            this.Index = 0;
        }



        public bool ReadObjectModel(out HObjectModel3D objectModel)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
            HalconLibrary ha = new HalconLibrary();
            objectModel = new HObjectModel3D();
            string path = this.filePath[this.Index];
            string[] extenName = path.Split('.');
            //////////////////////
            if (extenName.Length > 0)
            {
                switch (extenName[1])
                {
                    case "txt":
                        double[] X = null;
                        double[] Y = null;
                        double[] Z = null;
                        fo.ReadTxt(path, this.Unit, out X, out Y, out Z);
                        objectModel.GenObjectModel3dFromPoints(X, Y, Z);
                        result = true;
                        break;
                    case "om3":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "om3");
                        result = true;
                        break;

                    case "dxf":
                        //objectModel.ReadObjectModel3dDxf(path,this.Unit, "file_type", "dxf");
                        result = true;
                        break;

                    case "off":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "off");
                        result = true;
                        break;

                    case "obj":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "obj");
                        result = true;
                        break;

                    case "ply":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "ply");
                        result = true;
                        break;

                    case "stl":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "stl");
                        result = true;
                        break;
                }
            }
            return result;
        }


        public HObjectModel3D ReadObjectModel(int dataWidth, int dataHeight, out ImageDataClass imageData)
        {
            FileOperate fo = new FileOperate();
            HalconLibrary ha = new HalconLibrary();
            HObjectModel3D objectModel = new HObjectModel3D();
            imageData = new ImageDataClass();
            string path = this.filePath[this.Index];
            string[] extenName = path.Split('.');
            double[] X = null;
            double[] Y = null;
            double[] Z = null;
            //////////////////////
            if (extenName.Length > 0)
            {
                switch (extenName[1])
                {
                    case "txt":
                        fo.ReadTxt(path, this.Unit, out X, out Y, out Z);
                        objectModel.GenObjectModel3dFromPoints(X, Y, Z);
                        /////////////////////////////
                        if (Z != null && Z.Length > 0)
                        {
                            IntPtr ptr = Marshal.AllocHGlobal(sizeof(double) * Z.Length);
                            HImage hImage = new HImage("real", dataWidth, dataHeight, ptr);
                            Marshal.FreeHGlobal(ptr);
                        }
                        break;
                    case "om3":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "om3");
                        break;

                    case "dxf":
                        //objectModel.ReadObjectModel3dDxf(path,this.Unit, "file_type", "dxf");
                        break;

                    case "off":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "off");
                        break;

                    case "obj":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "obj");

                        break;

                    case "ply":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "ply");

                        break;

                    case "stl":
                        objectModel.ReadObjectModel3d(path, this.Unit, "file_type", "stl");

                        break;
                }
            }
            return objectModel;
        }


    }
}
