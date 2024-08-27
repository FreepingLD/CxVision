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
    [Serializable]
    [DefaultProperty(nameof(DistPp))]
    public class PointToPointDist3D : BaseFunction, IFunction
    {
        private double distPp;
        private double dist_x;
        private double dist_y;
        private double dist_z;
        [NonSerialized]
        private HObjectModel3D dataHandle3DPoint1 = null;
        [NonSerialized]
        private HObjectModel3D dataHandle3DPoint2 = null;


        [DisplayName("输入3D对象1")]
        [DescriptionAttribute("输入属性1")]
        public HObjectModel3D DataHandle3DPoint1
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        List<HObjectModel3D> list = new List<HObjectModel3D>();
                        if (oo != null)
                        {
                            if (oo.Length > 1)
                            {
                                foreach (var item in oo)
                                {
                                    if (item is HObjectModel3D)
                                        list.Add(item as HObjectModel3D);
                                }
                                this.dataHandle3DPoint1 = HObjectModel3D.UnionObjectModel3d(list.ToArray(), "points_surface");
                                list.Clear();
                            }
                            else
                            {
                                if (oo.Length == 1)
                                {
                                    if (oo.Last() is HObjectModel3D)
                                        this.dataHandle3DPoint1 = (oo.Last() as HObjectModel3D).Clone();
                                }
                            }
                        }
                        else
                        {
                            this.dataHandle3DPoint1 = new HObjectModel3D();
                        }
                    }
                    else
                    {
                        this.dataHandle3DPoint1 = new HObjectModel3D();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dataHandle3DPoint1;
            }
            set
            {
                dataHandle3DPoint1 = value;
            }
        }

        [DisplayName("输入3D对象2")]
        [DescriptionAttribute("输入属性2")]
        public HObjectModel3D DataHandle3DPoint2
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource2);
                        List<HObjectModel3D> list = new List<HObjectModel3D>();
                        if (oo != null)
                        {
                            if (oo.Length > 1)
                            {
                                foreach (var item in oo)
                                {
                                    if (item is HObjectModel3D)
                                        list.Add(item as HObjectModel3D);
                                }
                                this.dataHandle3DPoint2 = HObjectModel3D.UnionObjectModel3d(list.ToArray(), "points_surface");
                                list.Clear();
                            }
                            else
                            {
                                if (oo.Length == 1)
                                {
                                    if (oo.Last() is HObjectModel3D)
                                        this.dataHandle3DPoint2 = (oo.Last() as HObjectModel3D).Clone();
                                }
                            }
                        }
                        else
                        {
                            this.dataHandle3DPoint2 = new HObjectModel3D();
                        }
                    }
                    else
                    {
                        this.dataHandle3DPoint2 = new HObjectModel3D();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dataHandle3DPoint1;
            }
            set
            {
                dataHandle3DPoint1 = value;
            }
        }

        [DisplayName("距离")]
        [DescriptionAttribute("输出属性")]
        public double DistPp { get => distPp; set => distPp = value; }
        [DisplayName("X距离")]
        [DescriptionAttribute("输出属性")]
        public double Dist_x { get => dist_x; set => dist_x = value; }
        [DisplayName("Y距离")]
        [DescriptionAttribute("输出属性")]
        public double Dist_y { get => dist_y; set => dist_y = value; }
        [DisplayName("Z距离")]
        [DescriptionAttribute("输出属性")]
        public double Dist_z { get => dist_z; set => dist_z = value; }


        public PointToPointDist3D()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }

        public bool CalculaePointToPointDistance(HObjectModel3D pointObjectModel_1, HObjectModel3D pointObjectModel_2, out double distValue, out double dist_x, out double dist_y, out double dist_z)
        {
            bool result = false;
            distValue = 0;
            dist_x = 0;
            dist_y = 0;
            dist_z = 0;
            ///////////////////////////
            if (pointObjectModel_1 == null)
            {
                throw new ArgumentNullException("pointObjectModel_1", "参数为空");
            }
            if (pointObjectModel_2 == null)
            {
                throw new ArgumentNullException("pointObjectModel_2", "参数为空");
            }
            /////////////////////////////////////
            if (pointObjectModel_1.GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("对象中不包含数据点", "pointObjectModel_1");
            if (pointObjectModel_2.GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("对象中不包含数据点", "pointObjectModel_2");
            //////////////////////// 测量对象
            HTuple x1_ave, y1_ave, z1_ave, x2_ave, y2_ave, z2_ave;
            x1_ave = pointObjectModel_1.GetObjectModel3dParams("point_coord_x").TupleMean();
            y1_ave = pointObjectModel_1.GetObjectModel3dParams("point_coord_y").TupleMean();
            z1_ave = pointObjectModel_1.GetObjectModel3dParams("point_coord_z").TupleMean();
            x2_ave = pointObjectModel_2.GetObjectModel3dParams("point_coord_x").TupleMean();
            y2_ave = pointObjectModel_2.GetObjectModel3dParams("point_coord_y").TupleMean();
            z2_ave = pointObjectModel_2.GetObjectModel3dParams("point_coord_z").TupleMean();
            ////////////////////////////////////////////////////
            distValue = Math.Sqrt((x2_ave.D - x1_ave.D) * (x2_ave.D - x1_ave.D) + (y2_ave.D - y1_ave.D) * (y2_ave.D - y1_ave.D) + (z2_ave.D - z1_ave.D) * (z2_ave.D - z1_ave.D));
            dist_x = (Math.Abs(x2_ave.D - x1_ave.D));
            dist_y = (Math.Abs(y2_ave.D - y1_ave.D));
            dist_z = (Math.Abs(z1_ave.D - z2_ave.D));
            //////////////////////////
            result = true; ;
            return result;
        }



        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                if (this.DataHandle3DPoint1.IsInitialized())
                    this.DataHandle3DPoint1.Dispose();
                if (this.DataHandle3DPoint2.IsInitialized())
                    this.DataHandle3DPoint2.Dispose();
                this.Result.Succss = CalculaePointToPointDistance(this.DataHandle3DPoint1, this.DataHandle3DPoint2, out this.distPp, out this.dist_x, out this.dist_y, out this.dist_z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "距离", this.distPp);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "X距离", this.dist_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Y距离", this.dist_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Z距离", this.dist_z);
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
                case nameof(this.Name):
                    return this.name;
                case "最大值":
                case "两点距离":
                default:
                case nameof(this.DistPp):
                    return this.distPp; //
                case "距离Z":
                case nameof(this.Dist_z):
                    return this.dist_z; //
                case "距离Y":
                case nameof(this.Dist_y):
                    return this.dist_y; //
                case "距离X":
                case nameof(this.Dist_x):
                    return this.dist_x; //           
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
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
                if (this.DataHandle3DPoint1.IsInitialized())
                    this.DataHandle3DPoint1.Dispose();
                if (this.DataHandle3DPoint2.IsInitialized())
                    this.DataHandle3DPoint2.Dispose();
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
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




    }
}
