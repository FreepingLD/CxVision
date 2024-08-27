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

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class SampleObjectModel3D : BaseFunction, IFunction
    {
        private double sampleDist = 0.05;
        private string sampleMethod = "fast";
        private string sampleType = "sample_object_model_3d";
        [NonSerialized]
        private HObjectModel3D[] dataHandle3D = null;
        private double max_angle_diff = 150;
        private double min_num_points = 100;
        // simplify_object_model_3d
        private double amount = 95;
        private string amount_type = "percentage_remaining";
        private string avoid_triangle_flips = "true";
        public string SampleMethod
        {
            get
            {
                return sampleMethod;
            }

            set
            {
                sampleMethod = value;
            }
        }
        public double SampleDist
        {
            get
            {
                return sampleDist;
            }

            set
            {
                sampleDist = value;
            }
        }
        public double Max_angle_diff
        {
            get
            {
                return max_angle_diff;
            }

            set
            {
                max_angle_diff = value;
            }
        }
        public double Min_num_points
        {
            get
            {
                return min_num_points;
            }

            set
            {
                min_num_points = value;
            }
        }
        public string SampleType
        {
            get
            {
                return sampleType;
            }

            set
            {
                sampleType = value;
            }
        }

        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }
        public string Amount_type
        {
            get
            {
                return amount_type;
            }

            set
            {
                amount_type = value;
            }
        }
        public string Avoid_triangle_flips
        {
            get
            {
                return avoid_triangle_flips;
            }

            set
            {
                avoid_triangle_flips = value;
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

        public bool sampleObjectModel3D(HTuple objectModel, HTuple sampleMethod, HTuple sampleDist, HTuple paramName, HTuple paramValue, out HTuple sampleObjectModel3D)
        {
            bool result = false;
            HTuple unionObjectModel = null;
            sampleObjectModel3D = null;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                if (objectModel != null && objectModel.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(objectModel, "points_surface", out unionObjectModel);
                else
                    return result;
                //////////////////////////////////////
                if (ha.IsContainPoint(unionObjectModel))
                {
                    HOperatorSet.SampleObjectModel3d(unionObjectModel, sampleMethod, sampleDist, paramName, paramValue, out sampleObjectModel3D);
                    //
                    result = true;
                }
            }
            catch
            {
                throw new HalconException();
            }
            finally
            {
                ha.ClearObjectModel3D(unionObjectModel);
            }
            return result;
        }

        public bool simplifyObjectModel3D(HTuple objectModel, HTuple Amount, HTuple paramName, HTuple paramValue, out HTuple simplifyObjectModel3D)
        {
            bool result = false;
            HTuple unionObjectModel = null;
            HTuple triangles;
            simplifyObjectModel3D = null;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                if (objectModel != null && objectModel.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(objectModel, "points_surface", out unionObjectModel);
                else
                    return result;
                //////////////////////////////////////
                if (ha.IsContainPoint(unionObjectModel))
                {
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "has_triangles", out triangles);
                    if (triangles.S == "false")
                    {
                        MessageBox.Show("3D对象中不包含三角形或多边形");
                        return result;
                    }
                    HOperatorSet.SimplifyObjectModel3d(unionObjectModel, "preserve_point_coordinates", Amount, paramName, paramValue, out simplifyObjectModel3D);
                }
            }
            catch
            {
                throw new HalconException();
            }
            finally
            {
                ha.ClearObjectModel3D(unionObjectModel);
            }
            return result;
        }
        public bool sampleObjectModel3D(HObjectModel3D[] objectModel, HTuple sampleMethod, HTuple sampleDist, HTuple paramName, HTuple paramValue, out HObjectModel3D[] sampleObjectModel3D)
        {
            bool result = false;
            sampleObjectModel3D = null;

            if (objectModel == null && objectModel.Length == 0) return result; // 都执行一次合并
                                                                               //////////////////////////////////////
            sampleObjectModel3D = HObjectModel3D.SampleObjectModel3d(objectModel, sampleMethod, sampleDist, paramName, paramValue);
            result = true;

            return result;
        }
        public bool simplifyObjectModel3D(HObjectModel3D[] objectModel, HTuple Amount, HTuple paramName, HTuple paramValue, out HObjectModel3D[] simplifyObjectModel3D)
        {
            bool result = false;
            HTuple triangles;
            simplifyObjectModel3D = null;
            ////////////////////////////
            if (objectModel == null && objectModel.Length == 0) return result; // 都执行一次合并
            /////////////////////////////////////
            triangles = HObjectModel3D.GetObjectModel3dParams(objectModel, "has_triangles");
            if (triangles[0].S == "false")
            {
                MessageBox.Show("3D对象中不包含三角形或多边形");
                return result;
            }
            simplifyObjectModel3D = HObjectModel3D.SimplifyObjectModel3d(objectModel, "preserve_point_coordinates", Amount, paramName, paramValue);
            result = true;

            return result;
        }


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                switch (this.sampleType)
                {
                    case "sample_object_model_3d":
                        ClearHandle(this.dataHandle3D);
                        Result.Succss = sampleObjectModel3D(extractRefSource1Data(), this.sampleMethod, this.sampleDist, new HTuple("max_angle_diff", "min_num_points"), new HTuple(this.max_angle_diff, this.min_num_points), out this.dataHandle3D);
                        break;
                    case "simplify_object_model_3d":
                        ClearHandle(this.dataHandle3D);
                        Result.Succss = simplifyObjectModel3D(extractRefSource1Data(), this.amount, new HTuple("amount_type", "avoid_triangle_flips"), new HTuple(this.amount_type, this.avoid_triangle_flips), out this.dataHandle3D);
                        break;
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
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data();
                case "3D对象":
                case "采样3D对象":
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
                //////////////////////            
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
           // throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }

        #endregion

        public enum enSampleMethod
        {
            accurate,
            accurate_use_normals,
            fast,
            fast_compute_normals,
        }

        public enum enSampleType
        {
            sample_object_model_3d,
            simplify_object_model_3d,
        }
        public enum enAmount_type
        {
            percentage_remaining,
            percentage_to_remove,
            num_points_remaining,
            num_points_to_remove,
        }
        public enum enShowItems
        {
            输入3D对象,
            采样3D对象,
        }
    }

}
