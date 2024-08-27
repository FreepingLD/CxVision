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
    public class SaveObjectModelParam
    {
        private string folderPath;
        private string extendName = ".om3";
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


        public SaveObjectModelParam()
        {
            this.FolderPath = "";
            this.ExtendName = ".om3";
            this.AddDataTime = false;
            this.FileName = "";
        }


        /// <summary>
        /// 保存3D对象模型到文件中，支持的文件格式“Txt", 'om3', 'dxf', 'off', 'ply', 'obj', 'stl'
        /// </summary>

        public bool WriteObjectModelToFile(HObjectModel3D objectModel)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
            string path = "";
            if (this.AddDataTime)
            {
               path = this.FolderPath + "\\" + this.FileName + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + this.ExtendName;
            }
            else
            {
                path = this.FolderPath + "\\" + this.FileName + this.ExtendName;
            }
            string[] extenName = path.Split('.');
            HalconLibrary ha = new HalconLibrary();
            //////////////////////
            if (objectModel == null)
            {
                return result;
            }
            ////////////////////////
            if (extenName.Length > 0)
            {
                if (extenName[1] == "txt") //txt om3 dxf off obj ply stl
                {
                    HTuple X = null;
                    HTuple Y = null;
                    HTuple Z = null;
                    X = objectModel.GetObjectModel3dParams("point_coord_x");
                    Y = objectModel.GetObjectModel3dParams("point_coord_y");
                    Z = objectModel.GetObjectModel3dParams("point_coord_z");
                    fo.WriteTxt(path, X, Y, Z);
                }
                //////////////////////
                if (extenName[1] == "om3")
                {
                    objectModel.WriteObjectModel3d("om3", path, new HTuple(), new HTuple());
                }
                //////////////////////
                if (extenName[1] == "dxf")
                {
                    objectModel.WriteObjectModel3d("dxf", path, new HTuple(), new HTuple());
                }
                //////////////////////
                if (extenName[1] == "off")
                {
                    objectModel.WriteObjectModel3d("off", path, new HTuple(), new HTuple());
                }
                //////////////////////
                if (extenName[1] == "obj")
                {
                    objectModel.WriteObjectModel3d("obj", path, new HTuple(), new HTuple());
                }
                //////////////////////
                if (extenName[1] == "ply")
                {
                    objectModel.WriteObjectModel3d("ply", path, new HTuple(), new HTuple());
                }
                //////////////////////
                if (extenName[1] == "stl") // "stl"格式只能
                {
                    if (objectModel.GetObjectModel3dParams("has_triangles").S == "true")
                        objectModel.WriteObjectModel3d("stl", path, new HTuple(), new HTuple());
                    else
                    {
                        int info;
                        HObjectModel3D hObjectModel3D = objectModel.TriangulateObjectModel3d("greedy", "greedy_kNN", 20, out info);
                        hObjectModel3D.WriteObjectModel3d("stl", path, new HTuple(), new HTuple());
                        if (hObjectModel3D != null)
                            hObjectModel3D.Dispose();
                    }
                }
                result = true;
            }
            return result;
        }




    }

    public enum enFileExtendName
    {
        ply,
        txt,
        om3,
        stl,
        obj,
        off,
        dxf,
        csv,
        tiff,
        bmp,
        jpeg,
        png,
    }


}
