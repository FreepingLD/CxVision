using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class InvertTranslateObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D  [] dataHandle3D = null;
        private double measureRange = 4;
        private double translateDist = 0;
        private string invertTarget = "取反+平移Z";
        public double MeasureRange
        {
            get
            {
                return measureRange;
            }

            set
            {
                measureRange = value;
            }
        }
        public double TranslateDist
        {
            get
            {
                return translateDist;
            }

            set
            {
                translateDist = value;
            }
        }
        public string InvertTarget
        {
            get
            {
                return invertTarget;
            }

            set
            {
                invertTarget = value;
            }
        }
        private HObjectModel3D[] extractRefSource1Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HTuple":
                            listObjectModel3D.Add(new HObjectModel3D(((HTuple)object3D).IP));
                            break;
                        case "HObjectModel3D":
                            listObjectModel3D.Add((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D);
                            break;
                        case "userWcsPose3D": // 如果是位姿，将其转换成3D对象
                            HObjectModel3D plane3D = new HObjectModel3D();
                            plane3D.GenPlaneObjectModel3d(((userWcsPose)object3D).GetHPose(), new HTuple(), new HTuple());
                            listObjectModel3D.Add(plane3D);
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }
        private bool InvertObjectModel(HObjectModel3D []  objectModel, out HObjectModel3D  [] invertObjectModel)
        {
            bool result = false;
            invertObjectModel = new HObjectModel3D[0];
            HTuple y_Value, z_Value;
            HalconLibrary ha = new HalconLibrary();
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentException("objectModel参数中不包含元素");
            }
            //////////////////
            //invertObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            invertObjectModel = new HObjectModel3D[objectModel.Length];
            for (int i = 0; i < objectModel.Length; i++)
            {
                y_Value = objectModel[i].GetObjectModel3dParams("point_coord_y");
                z_Value = objectModel[i].GetObjectModel3dParams("point_coord_z");
                //////////////////////////
                switch (this.invertTarget)
                {
                    case "取反+平移Y":
                        invertObjectModel[i]= objectModel[i].SetObjectModel3dAttrib(new HTuple("point_coord_y"), "", this.measureRange + this.translateDist - y_Value);
                        break;
                    case "取反+平移Z":
                        invertObjectModel[i] = objectModel[i].SetObjectModel3dAttrib(new HTuple("point_coord_z"), "", this.measureRange + this.translateDist - z_Value);
                        break;
                    case "仅平移Y":
                        invertObjectModel[i] = objectModel[i].SetObjectModel3dAttrib(new HTuple("point_coord_y"), "", this.translateDist + y_Value);
                        break;
                    case "仅平移Z":
                        invertObjectModel[i] = objectModel[i].SetObjectModel3dAttrib(new HTuple("point_coord_z"), "", this.translateDist + z_Value);
                        break;
                }
                result = true;
            }
            return result;
        }


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
           this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = InvertObjectModel(extractRefSource1Data(), out this.dataHandle3D);
                //this.resultDataTable.Rows.Clear();
                for (int i = 0; i < this.dataHandle3D.Length; i++)
                {
                    //this.ResultDataTable.Rows.Add(this.name, "点云数量", this.dataHandle3D[i].GetObjectModel3dParams("num_points").I, 0, 0, 0, "OK");
                }
                OnExcuteCompleted(this.name, this.dataHandle3D);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); //
                case "3D对象":
                case "取反3D对象":
                    return this.dataHandle3D; //
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
                {
                    foreach (var item in this.dataHandle3D)
                        item.Dispose();
                }
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
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            ///throw new NotImplementedException();
        }

        #endregion

        public enum enShowItems
        {
            输入3D对象,
            取反3D对象,
        }
    }
}
