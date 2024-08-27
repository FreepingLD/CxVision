using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
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
    public class FaceToFaceAngle : BaseFunction, IFunction
    {
        private double[] result;

        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string state;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }

        public FaceToFaceAngle()
        {
            //this.ResultDataTable.ColumnChanged += new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
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
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }
        private HObjectModel3D[] extractRefSource2Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                //////////////////////////////
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
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }

        public bool CalculateFaceToFaceAngle(HObjectModel3D[] planeObject1, HObjectModel3D[] planeObject2, out double[] deg)
        {
            bool result = false;
            deg = new double[0];
            HTuple value;
            HObjectModel3D fitPlaneObjectModel1 = null;
            HObjectModel3D fitPlaneObjectModel2 = null;
            HTuple plane1Primitive, plane2Primitive;
            HTuple plane1PoseNormal, plane2PoseNormal, paramValue;
            try
            {
                if (planeObject1 == null || planeObject2 == null)
                {
                    deg = new double[0];
                    return result;
                }
                /////////////
                int length = planeObject1.Length > planeObject2.Length ? planeObject2.Length : planeObject1.Length;
                deg = new double[length];
                for (int i = 0; i < length; i++)
                {
                    plane1Primitive = planeObject1[i].GetObjectModel3dParams("has_primitive_data");
                    plane2Primitive = planeObject2[i].GetObjectModel3dParams("has_primitive_data");
                    // 拟合第一个面
                    if (plane1Primitive.S == "false")
                    {
                        fitPlaneObjectModel1 = planeObject1[i].FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                        plane1PoseNormal = fitPlaneObjectModel1.GetObjectModel3dParams("primitive_parameter");
                    }
                    else
                    {
                        plane1PoseNormal = planeObject1[i].GetObjectModel3dParams("primitive_parameter");
                    }
                    // 拟合第二个面
                    if (plane2Primitive.S == "false")
                    {
                        fitPlaneObjectModel2 = planeObject2[i].FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                        plane2PoseNormal = fitPlaneObjectModel2.GetObjectModel3dParams("primitive_parameter");
                    }
                    else
                    {
                        plane2PoseNormal = planeObject2[i].GetObjectModel3dParams("primitive_parameter");
                    }
                    double[] N1 = plane1PoseNormal.ToDArr();
                    double[] N2 = plane2PoseNormal.ToDArr();
                    double cosA = Math.Abs(N1[0] * N2[0] + N1[1] * N2[1] + N1[2] * N2[2]) / (Math.Sqrt(N1[0] * N1[0] + N1[1] * N1[1] + N1[2] * N1[2]) * Math.Sqrt(N2[0] * N2[0] + N2[1] * N2[1] + N2[2] * N2[2]));
                    HOperatorSet.TupleAcos(cosA, out paramValue);
                    HOperatorSet.TupleDeg(paramValue, out value);
                    if (value.Length > 0)
                        deg[i] = value.D;
                }
                result = true;
            }
          catch (Exception ex)
            {
                result = false;
                //throw new HalconException();
            }
            return result;
        }
        private void resultDataTable_DataColumnChange(object sender, DataColumnChangeEventArgs e)
        {
            switch (e.Column.ColumnName)
            {
                case "标准值":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.stdValue))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "上偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.upTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "下偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.downTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
            }
        }

        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;

            try
            {
                Result.Succss = CalculateFaceToFaceAngle(extractRefSource1Data(), extractRefSource2Data(), out this.result);
               // this.ResultDataTable.Clear();
                for (int i = 0; i < this.result.Length; i++)
                {
                    if (this.stdValue + this.downTolerance <= this.result[i] && this.result[i] <= this.stdValue + this.upTolerance)
                        this.state = "OK";
                    else
                        this.state = "NG";
                    //this.ResultDataTable.Rows.Add(this.name + i.ToString(), "角度", this.result[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                }
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
                case "角度":
                    return this.result; //
                case "平面3D对象1":
                    return extractRefSource1Data(); //
                case "平面3D对象2":
                    return extractRefSource2Data(); //
                default:
                    if (this.name == propertyName)
                        return this.result;
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
                //this.ResultDataTable.ColumnChanged -= new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
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
          //  throw new NotImplementedException();
        }
        #endregion
        public enum enShowItems
        {
            平面3D对象1,
            平面3D对象2,
        }
    }
}
