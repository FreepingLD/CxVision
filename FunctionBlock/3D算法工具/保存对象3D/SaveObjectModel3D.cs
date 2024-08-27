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
    [DefaultProperty("DataHandle3D")]
    public class SaveObjectModel3D : BaseFunction, IFunction
    {
        private string folderPath;
        private string extendName = ".om3";
        private bool addDataTime = false;
        private string fileName;
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;
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


        public SaveObjectModelParam SaveParam { get; set; }


        [DisplayName("输入3D对象")]
        [DescriptionAttribute("输入属性1")]
        public HObjectModel3D DataHandle3D
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                        this.dataHandle3D = this.GetPropertyValue(this.RefSource1).Last() as HObjectModel3D;
                    else
                    {
                        this.dataHandle3D = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dataHandle3D;
            }
            set
            {
                dataHandle3D = value;
            }
        }

        public SaveObjectModel3D()
        {
            this.SaveParam = new SaveObjectModelParam();
        }



        /// <summary>
        /// 保存3D对象模型到文件中，支持的文件格式“Txt", 'om3', 'dxf', 'off', 'ply', 'obj', 'stl'
        /// </summary>

        public bool WriteObjectModelToFile(HObjectModel3D objectModel, string path)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
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

        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = this.SaveParam.WriteObjectModelToFile(this.DataHandle3D);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case "文件路径":
                    return this.folderPath;
                case "源3D对象":
                case "输入3D对象":
                case nameof(this.DataHandle3D):
                    return this.DataHandle3D;
                default:
                    break;
            }
            return null;
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                case "文件扩展名":
                    this.extendName = value[0].ToString();
                    return true;
                case "添加日期时间":
                    this.addDataTime = (bool)value[0];
                    return true;
                case "文件路径":
                    this.folderPath = value.ToString();
                    return true;
                default:
                    return true;
            }
        }

        public void ReleaseHandle()
        {
            try
            {
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

        #endregion



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
        public enum enShowItems
        {
            输入3D对象,
        }
    }
}
