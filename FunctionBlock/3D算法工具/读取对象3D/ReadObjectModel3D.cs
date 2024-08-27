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
    public class ReadObjectModel3D :BaseFunction, IFunction 
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D;

        public ReadObjectModelParam ReadParam { get; set; }

        public ReadObjectModel3D()
        {
            this.ReadParam = new ReadObjectModelParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }


        [DisplayName("3D对象")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D DataHandle3D { get => dataHandle3D; set => dataHandle3D = value; }

        public bool ReadObjectModel(string path, double unit, out HObjectModel3D objectModel)
        {
            bool result = false;
            FileOperate fo = new FileOperate();
            HalconLibrary ha = new HalconLibrary();
            objectModel = new HObjectModel3D();
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
                        fo.ReadTxt(path, unit, out X, out Y, out Z);

                        //new HalconLibrary().GenRealImage(1749, 4714, Z);
                        objectModel.GenObjectModel3dFromPoints(X, Y, Z);
                        result = true;
                        break;
                    case "om3":
                        objectModel.ReadObjectModel3d(path, 1, "file_type", "om3");
                        result = true;
                        break;

                    case "dxf":
                        //objectModel.ReadObjectModel3dDxf(path, 1, "file_type", "dxf");
                        result = true;
                        break;

                    case "off":
                        objectModel.ReadObjectModel3d(path, 1, "file_type", "off");
                        result = true;
                        break;

                    case "obj":
                        objectModel.ReadObjectModel3d(path, 1, "file_type", "obj");
                        result = true;
                        break;

                    case "ply":
                        objectModel.ReadObjectModel3d(path, 1, "file_type", "ply");
                        result = true;
                        break;

                    case "stl":
                        objectModel.ReadObjectModel3d(path, 1, "file_type", "stl");
                        result = true;
                        break;
                    case "sfm":
                        //  ClearObjectModel3D(objectModel);
                        //HOperatorSet.ReadSurfaceModel(path, out objectModel);
                        result = true;
                        break;

                    case "shm":
                        // ClearObjectModel3D(objectModel);
                        //HOperatorSet.ReadShapeModel(path, out objectModel);
                        result = true;
                        break;


                }
            }
            return result;
        }



        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            int index = 0;
            try
            {                
                if (param == null) index = 0; // param不能为空
                else
                int.TryParse(param.ToString(), out index);
                if (this.ReadParam.FilePath.Count == 0) return this.Result;
                ClearHandle(this.dataHandle3D);
                this.ReadParam.Index = index;
                this.Result.Succss = this.ReadParam.ReadObjectModel(out this.dataHandle3D); // 读取第I个路径的图像
                if (this.dataHandle3D != null)
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "点云数量", this.dataHandle3D.GetObjectModel3dParams("num_points").I);
                    OnExcuteCompleted(this.name, this.dataHandle3D);
                    this.Result.Succss = true;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
                return this.Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功"+"-点云数量"+ this.dataHandle3D.GetObjectModel3dParams("num_points").I.ToString());
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
                case "3D对象":
                    return this.dataHandle3D;
                case "名称":
                    return this.name;
                case "输入3D对象":
                    return this.dataHandle3D;
                default:
                    if (this.name == propertyName)
                        return this.dataHandle3D;
                    else return null;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                if (this.dataHandle3D != null)
                    this.dataHandle3D.Dispose();
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
            finally
            {
                OnItemDeleteEvent(this, this.name);
            }
        }
        public void Read(string path)
        {
          //  throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }

        #endregion


    }
}
