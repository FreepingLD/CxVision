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
using System.ComponentModel;
using AlgorithmsLibrary;
using Common;
using System.Data;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class FilterObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D [] dataHandle3D = null;
        // 3D对象联接
        private string connectFeature = "distance_3d";
        private double connectValue = 0.05;
        // 3D对象选择
        private string selectFeature = "num_points";
        private string selectOperation = "and";
        private int minFeatureValue = 15;
        private int maxFeatureValue = 50000;
        public string ConnectFeature
        {
            get
            {
                return connectFeature;
            }

            set
            {
                connectFeature = value;
            }
        }
        public double ConnectValue
        {
            get
            {
                return connectValue;
            }

            set
            {
                connectValue = value;
            }
        }
        public string SelectOperation
        {
            get
            {
                return selectOperation;
            }

            set
            {
                selectOperation = value;
            }
        }
        public int MinFeatureValue
        {
            get
            {
                return minFeatureValue;
            }

            set
            {
                minFeatureValue = value;
            }
        }
        public int MaxFeatureValue
        {
            get
            {
                return maxFeatureValue;
            }

            set
            {
                maxFeatureValue = value;
            }
        }
        public string SelectFeature
        {
            get
            {
                return selectFeature;
            }

            set
            {
                selectFeature = value;
            }
        }

        public FilterObjectModel3D()
        {
            // resultDataTable.Columns.AddRange(new DataColumn[3] { new DataColumn("元素名称"),new DataColumn("元素类型"),new DataColumn("点数")});
        }


        private HObjectModel3D [] extractRefSource1Data()
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

        public bool FilterObject3D(HObjectModel3D [] objectModel, HTuple connectFeature, HTuple connectFeatureValue, HTuple selectFeature, HTuple selectOperation, HTuple minFeatureValue, HTuple maxFeatureValue, out HObjectModel3D [] objectModel3DOut)
        {
            bool result = false;
            HObjectModel3D [] objectModel3DConnected = null;
            objectModel3DOut = null;
            ///////////////
            try
            {
                if (objectModel == null || objectModel.Length==0) return result;// 都执行一次合并
                objectModel3DConnected = HObjectModel3D.ConnectionObjectModel3d(objectModel, connectFeature, connectFeatureValue);
                objectModel3DOut = HObjectModel3D.SelectObjectModel3d(objectModel3DConnected, selectFeature, selectOperation, minFeatureValue, maxFeatureValue); // HObjectModel3D.UnionObjectModel3d( , "points_surface");              
                result = true;
            }
            catch
            {
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
                Result.Succss = FilterObject3D(extractRefSource1Data(), this.connectFeature, this.connectValue, this.selectFeature, this.selectOperation, new HTuple(this.minFeatureValue), new HTuple(this.maxFeatureValue), out this.dataHandle3D);
                //this.ResultDataTable.Clear();
                for (int i = 0; i < this.dataHandle3D.Length; i++)
                {
                   // this.ResultDataTable.Rows.Add(this.name, "点云数量", this.dataHandle3D[i].GetObjectModel3dParams("num_points").I,0,0,0,"OK");
                }               
                OnExcuteCompleted(this.name, this.dataHandle3D); // 引发这个事件 
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
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data();
                case "3D对象":
                case "滤波3D对象":
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
           // throw new NotImplementedException();
        }

        #endregion

        public enum enConnectFeatue
        {
            angle,
            distance_3d,
            distance_mapping,
            lines,
            mesh,
        }

        public enum enSelecctFeatue
        {
            mean_points_x,
            mean_points_y,
            mean_points_z,
            diameter_axis_aligned_bounding_box,
            diameter_bounding_box,
            diameter_object,
            volume,
            volume_axis_aligned_bounding_box,
            area,
            central_moment_2_x,
            central_moment_2_y,
            central_moment_2_z,
            central_moment_2_xy,
            central_moment_2_xz,
            central_moment_2_yz,
            num_points,
            num_triangles,
            num_faces,
            num_lines,
            has_points,
            has_point_normals,
            has_triangles,
            has_faces,
            has_lines,
            has_xyz_mapping,
            has_shape_based_matching_3d_data,
            has_surface_based_matching_data,
            has_segmentation_data,
            has_primitive_data,
        }

        public enum enShowItems
        {
            输入3D对象,
            滤波3D对象,
        }

    }
}
