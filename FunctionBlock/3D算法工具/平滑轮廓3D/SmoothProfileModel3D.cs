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
    [DefaultProperty("")]
    public class SmoothProfileModel3D : BaseFunction, IFunction
    {
        private double sample_dist = 0.02;
        private double smooth_value = 15;
        private string smooth_method = "平滑"; // "平滑";"高斯"
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;

        private HObjectModel3D outObjectModel3D = null;
        public double Smooth_value
        {
            get
            {
                return smooth_value;
            }

            set
            {
                smooth_value = value;
            }
        }
        public string Smooth_method
        {
            get
            {
                return smooth_method;
            }

            set
            {
                smooth_method = value;
            }
        }
        public double Sample_dist
        {
            get
            {
                return sample_dist;
            }

            set
            {
                sample_dist = value;
            }
        }

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

        [DisplayName("输出3D对象")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D OutObjectModel3D
        {
            get => outObjectModel3D;
            set => outObjectModel3D = value;
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
            return listObjectModel3D.ToArray(); //HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
        }
        public bool smoothProfileModel3D(HObjectModel3D[] profileModel3D, string method, HTuple smooth, HTuple sampleDist, out HObjectModel3D[] smoothProfileModel3D)
        {
            bool result = false;
            HObject originContour = null;
            HObject smoothContour = null;
            HTuple smooth_x = new HTuple();
            HTuple smooth_y = new HTuple();
            HTuple smooth_z = new HTuple();
            HTuple function, smoothFunction, line_x, line_y, line_z;
            smoothProfileModel3D = new HObjectModel3D[0];
            HalconLibrary ha = new HalconLibrary();
            //////////////////////////////////////
            if (profileModel3D == null)
            {
                throw new ArgumentNullException("profileModel3D");
            }
            smoothProfileModel3D = new HObjectModel3D[profileModel3D.Length];
            for (int i = 0; i < profileModel3D.Length; i++)
            {
                if (profileModel3D[i].GetObjectModel3dParams("num_points").I > 0)
                {
                    // 测量对象
                    line_x = profileModel3D[i].GetObjectModel3dParams("point_coord_x"); //要把三维的图形转换成二维图形
                    line_y = profileModel3D[i].GetObjectModel3dParams("point_coord_y");
                    line_z = profileModel3D[i].GetObjectModel3dParams("point_coord_z");
                    //////////////////////////////////////////////
                    if (line_x == null || line_x.Length == 0) continue;
                    switch (method)
                    {
                        default:
                        case "平滑":
                            new HXLDCont(line_z.DArr, HTuple.TupleGenSequence(1.0, line_z.Length, 1.0)).SmoothContoursXld((int)smooth.D).GetContourXld(out smooth_z, out smooth_x);
                            smooth_x = line_x;
                            smooth_y = line_y;
                            break;
                        case "高斯":
                            HOperatorSet.CreateFunct1dArray(line_z, out function);
                            HOperatorSet.SmoothFunct1dGauss(function, smooth, out smoothFunction);
                            HOperatorSet.Funct1dToPairs(smoothFunction, out smooth_x, out smooth_z);
                            smooth_x = line_x;
                            smooth_y = line_y;
                            break;
                    }
                    // 生成2D轮廓对象
                    smoothProfileModel3D[i] = new HObjectModel3D(smooth_x, smooth_y, smooth_z);
                    if (originContour != null)
                        originContour.Dispose();
                    if (smoothContour != null)
                        smoothContour.Dispose();
                    //////////////////
                    result = true;
                }
            }
            return result;
        }

        public bool smoothProfileModel3D(HObjectModel3D profileModel3D, string method, HTuple smooth, HTuple sampleDist, out HObjectModel3D smoothProfileModel3D)
        {
            bool result = false;
            HObject originContour = null;
            HObject smoothContour = null;
            HTuple smooth_x = new HTuple();
            HTuple smooth_y = new HTuple();
            HTuple smooth_z = new HTuple();
            HTuple function, smoothFunction, line_x, line_y, line_z;
            smoothProfileModel3D = null;
            HalconLibrary ha = new HalconLibrary();
            //////////////////////////////////////
            if (profileModel3D == null)
            {
                throw new ArgumentNullException("profileModel3D");
            }

            if (profileModel3D.GetObjectModel3dParams("num_points").I > 0)
            {
                // 测量对象
                line_x = profileModel3D.GetObjectModel3dParams("point_coord_x"); //要把三维的图形转换成二维图形
                line_y = profileModel3D.GetObjectModel3dParams("point_coord_y");
                line_z = profileModel3D.GetObjectModel3dParams("point_coord_z");
                //////////////////////////////////////////////
                if (line_x == null || line_x.Length == 0) ;
                switch (method)
                {
                    default:
                    case "平滑":
                        new HXLDCont(line_z.DArr, HTuple.TupleGenSequence(1.0, line_z.Length, 1.0)).SmoothContoursXld((int)smooth.D).GetContourXld(out smooth_z, out smooth_x);
                        smooth_x = line_x;
                        smooth_y = line_y;
                        break;
                    case "高斯":
                        HOperatorSet.CreateFunct1dArray(line_z, out function);
                        HOperatorSet.SmoothFunct1dGauss(function, smooth, out smoothFunction);
                        HOperatorSet.Funct1dToPairs(smoothFunction, out smooth_x, out smooth_z);
                        smooth_x = line_x;
                        smooth_y = line_y;
                        break;
                }
                // 生成2D轮廓对象
                smoothProfileModel3D = new HObjectModel3D(smooth_x, smooth_y, smooth_z);
                if (originContour != null)
                    originContour.Dispose();
                if (smoothContour != null)
                    smoothContour.Dispose();
                //////////////////
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
                ClearHandle(this.outObjectModel3D);
                Result.Succss = smoothProfileModel3D(this.DataHandle3D, this.smooth_method, this.smooth_value, this.sample_dist, out this.outObjectModel3D);
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
                case "3D对象":
                case "平滑3D对象":
                    return this.dataHandle3D; //
                case "源3D对象": //输入3D对象
                case "输入3D对象": //
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
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                this.dataHandle3D?.Dispose();
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

        public enum enShowItems
        {
            输入3D对象,
            平滑3D对象,
        }
    }
}
