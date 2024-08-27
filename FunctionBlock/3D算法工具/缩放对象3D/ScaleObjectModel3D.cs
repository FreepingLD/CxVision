using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensor;
using MotionControlCard;
using HalconDotNet;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using Common;

namespace FunctionBlock
{

    /// <summary>
    /// 计算平面度，并返回平面度的值及位姿
    /// </summary>
    [Serializable]
    public class ScaleObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;
        private double scale_x = 1.0;
        private double scale_y = 1.0;
        private double scale_z = 1.0;
        private int line_Num = 180;
        private double speed = 10;
        private double frequency = 1000;

        public double Scale_x
        {
            get
            {
                return scale_x;
            }

            set
            {
                scale_x = value;
            }
        }
        public double Scale_y
        {
            get
            {
                return scale_y;
            }

            set
            {
                scale_y = value;
            }
        }
        public double Scale_z
        {
            get
            {
                return scale_z;
            }

            set
            {
                scale_z = value;
            }
        }
        public int Line_Num
        {
            get
            {
                return line_Num;
            }

            set
            {
                line_Num = value;
            }
        }
        public double Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }
        public double Frequency
        {
            get
            {
                return frequency;
            }

            set
            {
                frequency = value;
            }
        }

        public ScaleObjectModel3D()
        {

        }

        private HObjectModel3D extractRefSource1Data()
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
            return HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
        }

        private bool AutoYStep(HTuple objectModel3D, out HTuple outObjectModel3D)
        {
            bool result = false;
            outObjectModel3D = null;
            HTuple unionObjectModel = null;
            HTuple Y_value, value, isInt;
            double[] new_y_value;
            try
            {
                double[] temp = new double[this.line_Num];
                double step = 1 / this.frequency * speed;
                ///////////////
                if (objectModel3D != null && objectModel3D.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(objectModel3D, "points_surface", out unionObjectModel);
                else
                    return result;
                /////////////////////////////////////////////////////              
                HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_y", out Y_value);
                if (Y_value == null || Y_value.Length == 0) return false;
                if (Y_value[0] > Y_value[Y_value.Length - 1])
                    step *= -1;
                HOperatorSet.TupleMod(Y_value.Length, this.line_Num, out value);
                HOperatorSet.TupleIsInt(value, out isInt);
                if (isInt < 1)
                {
                    MessageBox.Show("点数不是整数倍");
                    return false;
                }
                new_y_value = new double[Y_value.Length];
                //////////////////////////////////////////////////
                for (int i = 0; i < Y_value.Length / this.line_Num; i++)
                {
                    for (int ii = 0; ii < this.line_Num; ii++)
                    {
                        new_y_value[i * this.line_Num + ii] = i * step;
                    }
                }
                HOperatorSet.SetObjectModel3dAttrib(unionObjectModel, "point_coord_y", new HTuple(), new HTuple(new_y_value), out outObjectModel3D);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                new HalconLibrary().ClearObjectModel3D(unionObjectModel);
            }
        }
        public bool SetObjectModel3dAttribMod(HTuple objectModel3D, HTuple scale_x, HTuple scale_y, HTuple scale_z, out HTuple outObjectModel3D)
        {
            bool result = false;
            HTuple unionObjectModel = null;
            HTuple X, Y, Z;
            outObjectModel3D = null;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                if (objectModel3D != null && objectModel3D.Length > 0) // 都执行一次合并
                    HOperatorSet.UnionObjectModel3d(objectModel3D, "points_surface", out unionObjectModel);
                else
                    return result;
                /////////////////////////////
                if (ha.IsContainPoint(unionObjectModel))
                {
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_x", out X);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_y", out Y);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_z", out Z);
                    ///////////////////////////////////////////////
                    HOperatorSet.SetObjectModel3dAttrib(unionObjectModel, "point_coord_x", new HTuple(), X * scale_x, out outObjectModel3D);
                    HOperatorSet.SetObjectModel3dAttribMod(outObjectModel3D, "point_coord_y", new HTuple(), Y * scale_y);
                    HOperatorSet.SetObjectModel3dAttribMod(outObjectModel3D, "point_coord_z", new HTuple(), Z * scale_z);
                    //HOperatorSet.SetObjectModel3dAttribMod(unionObjectModel, "point_coord_x", new HTuple(), X * scale_x);
                    //HOperatorSet.SetObjectModel3dAttribMod(unionObjectModel, "point_coord_y", new HTuple(), Y * scale_y);
                    //HOperatorSet.SetObjectModel3dAttribMod(unionObjectModel, "point_coord_z", new HTuple(), Z * scale_z);
                    ////////////////////////
                    result = true;
                }
            }
          catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ha.ClearObjectModel3D(unionObjectModel);
            }
            return result;
        }
        public bool SetObjectModel3dAttribMod(HObjectModel3D objectModel3D, HTuple scale_x, HTuple scale_y, HTuple scale_z, out HObjectModel3D outObjectModel3D)
        {
            bool result = false;
            HTuple X, Y, Z;
            outObjectModel3D = null;
            //////////////
            if (objectModel3D == null || objectModel3D.GetObjectModel3dParams("num_points").I == 0) return result;// 都执行一次合并
            ////////////////////////////////////////////////////
            X = objectModel3D.GetObjectModel3dParams("point_coord_x");
            Y = objectModel3D.GetObjectModel3dParams("point_coord_y");
            Z = objectModel3D.GetObjectModel3dParams("point_coord_z");
            ///////////////////////////////////////////////
            outObjectModel3D = objectModel3D.SetObjectModel3dAttrib(new HTuple("point_coord_x"), "", X * scale_x);
            outObjectModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_y"), "", Y * scale_y);
            outObjectModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), "", Z * scale_z);

            ////////////////////////
            result = true;
            return result;
        }

        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = SetObjectModel3dAttribMod(extractRefSource1Data(), this.scale_x, this.scale_y, this.scale_z, out this.dataHandle3D);
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
                case "缩放3D对象":
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
                if (this.dataHandle3D != null) this.dataHandle3D.Dispose();
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
            缩放3D对象,
        }
    }
}
