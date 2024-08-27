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
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class SmoothObjectModel3D : BaseFunction, IFunction
    {
        private HTuple paramName;
        private HTuple paramValue;
        private HObjectModel3D [] dataHandle3D = null;
        private int mls_kNN = 60;
        private int mls_order = 2;
        private double mls_relative_sigma = 1;
        private string mls_force_inwards = "false";  // "true", "false"


        [DisplayName("最近点数量"), DescriptionAttribute("指定用于将MLS曲面拟合到每个点的最近邻k的数目"),
         ReadOnlyAttribute(false)]
        public int Mls_kNN
        {
            get
            {
                return mls_kNN;
            }

            set
            {
                mls_kNN = value;
            }
        }

        [DisplayName("拟合顺序"), DescriptionAttribute("指定MLS多项式曲面的顺序。对于'mls_order'=1，曲面是一个平面"),
         ReadOnlyAttribute(false)]
        public int Mls_order
        {
            get
            {
                return mls_order;
            }

            set
            {
                mls_order = value;
            }
        }

        [DisplayName("拟合的相对阈值"), DescriptionAttribute("根据公式指定一个用于计算点P的乘法系数"),
         ReadOnlyAttribute(false)]
        public double Mls_relative_sigma
        {
            get
            {
                return mls_relative_sigma;
            }

            set
            {
                mls_relative_sigma = value;
            }
        }

        [DisplayName("强制平面法向向里"), DescriptionAttribute("如果这个参数设置为“true”，所有的表面法线都指向原点的方向，如果结果SmoothObjectModel3D用于基于表面的匹配，这可能是必要的，无论是作为create_surface_model中的模型，还是作为find_surface_model中的3D场景，因为在这里，法线的一致方向对匹配过程很重要。如果'mls_force_inwards'设置为'false'，则法向量的方向是任意的"),
         ReadOnlyAttribute(false), TypeConverter(typeof(Smooth3DobjectModelConveter))]
        public string Mls_force_inwards
        {
            get
            {
                return mls_force_inwards;
            }

            set
            {
                mls_force_inwards = value;
            }
        }


        private void initParam()
        {
            this.paramName = new HTuple("mls_kNN", "mls_order", "mls_relative_sigma", "mls_force_inwards");
            this.paramValue = new HTuple(this.mls_kNN, this.mls_order, this.mls_relative_sigma, this.mls_force_inwards);
        }

        public SmoothObjectModel3D()
        {

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

        public bool SmoothObjectModel(HTuple objectModel, HTuple paramName, HTuple paramValue, out HTuple smoothObjectModel)
        {
            bool result = false;
            smoothObjectModel = null;
            HalconLibrary ha = new HalconLibrary();
            HTuple unionObjectModel = null;
            try
            {
                // 获取元素中包含的包含的数
                if (objectModel != null && objectModel.Length > 0)
                    HOperatorSet.UnionObjectModel3d(objectModel, "points_surface", out unionObjectModel);
                else
                    return result;
                //////////////////
                HOperatorSet.SmoothObjectModel3d(unionObjectModel, "mls", paramName, paramValue, out smoothObjectModel);
                result = true;
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
        public bool SmoothObjectModel(HObjectModel3D [] objectModel, HTuple paramName, HTuple paramValue, out HObjectModel3D [] smoothObjectModel)
        {
            bool result = false;
            smoothObjectModel = null;
            //////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");

            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentException("objectModel中不包含元素");
            }
            smoothObjectModel = HObjectModel3D.SmoothObjectModel3d(objectModel, "mls", paramName, paramValue);
            //smoothObjectModel = objectModel.SmoothObjectModel3d("mls", paramName, paramValue);
            //smoothObjectModel.SetObjectModel3dAttribMod(new HTuple("&RefPoint"), "object", objectModel.GetObjectModel3dParams("&RefPoint"));
            //////////////////
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
                initParam();
                ClearHandle(this.dataHandle3D);
                Result.Succss = SmoothObjectModel(extractRefSource1Data(), this.paramName, this.paramValue, out this.dataHandle3D);
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
                    {
                        item.Dispose();
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

        public enum enShowItems
        {
            输入3D对象,
            平滑3D对象,
        }
    }

    class Smooth3DobjectModelConveter : StringConverter
    {

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            PropertyDescriptor pd = context.PropertyDescriptor;
            string name = pd.Name;
            switch (name)
            {
                case "Mls_force_inwards":
                    return new StandardValuesCollection(new string[] { "true", "false" }); //fast', 'accurate', 'robust
                default:
                    return null;
            }
        }


        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }


    }
}
