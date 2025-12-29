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
using System.Data;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class OverturnObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D[] dataHandle3D = null;


        public OverturnObjectModel3D()
        {

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
                    }
                }
            }
            return listObjectModel3D.ToArray(); // HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
        }

        public bool OverturnObjectModel3d(HObjectModel3D[] objectModel, out HObjectModel3D[] transObjectModel)
        {
            bool result = false;
            transObjectModel = null;
            //////////////////////////////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel == null)
            {
                throw new ArgumentException("objectModel对象中不包含元素");
            }
            ////////////////////
            HTuple Px, Py, Pz;
            transObjectModel = new HObjectModel3D[objectModel.Length];
            for (int i = 0; i < objectModel.Length; i++)
            {

                Px = objectModel[i].GetObjectModel3dParams("point_coord_x");
                Py = objectModel[i].GetObjectModel3dParams("point_coord_y");
                Pz = objectModel[i].GetObjectModel3dParams("point_coord_z");
                ////////////////////////////////////////////
                transObjectModel[i] = objectModel[i].SetObjectModel3dAttrib(new HTuple("point_coord_x"), "", Px);
                transObjectModel[i].SetObjectModel3dAttribMod(new HTuple("point_coord_y"), "", Pz+ Py.TupleMean()- Pz.TupleMean());
                transObjectModel[i].SetObjectModel3dAttribMod(new HTuple("point_coord_z"), "", Py);
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
                Result.Succss = OverturnObjectModel3d(extractRefSource1Data(),  out this.dataHandle3D);
                //this.ResultDataTable.Clear();
                for (int i = 0; i < this.dataHandle3D.Length; i++)
                {
                    //this.ResultDataTable.Rows.Add(this.name, "点云数量", this.dataHandle3D[i].GetObjectModel3dParams("num_points").I, this.dataHandle3D[i].GetObjectModel3dParams("num_points").I, 0, 0, "OK");
                }
                OnExcuteCompleted(this.name, this.dataHandle3D);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case "变换3D对象":
                case "3D对象":
                    return this.dataHandle3D; //
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); //
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
                /////////////////////             
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
                    for (int i = 0; i < this.dataHandle3D.Length; i++)
                    {
                        this.dataHandle3D[i].Dispose();
                    }
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
            //throw new NotImplementedException();
        }
        #endregion

        public enum enTransMethod
        {
            rigid_trans_object_model_3d,
            projective_trans_object_model_3d,
            affine_trans_object_model_3d,

        }

        public enum enShowItems
        {
            输入3D对象,
            变换3D对象,
        }
    }
}
