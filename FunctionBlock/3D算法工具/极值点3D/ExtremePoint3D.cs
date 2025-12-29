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
using System.ComponentModel;
using Common;
using System.Data;
namespace FunctionBlock
{

    /// <summary>
    /// 计算平面度，并返回平面度的值及位姿
    /// </summary>
    [Serializable]
    public class ExtremePoint3D : BaseFunction, IFunction
    {
        private double[] x_value;
        private double[] y_value;
        private double[] z_value;
        private HObjectModel3D[] selectObjectModel3D; // 测量结果
        private double offsetValue = 0.005;
        private enExtremePointCoord extremePointCoord = enExtremePointCoord.Z轴;
        private enExtremePointMode extremePointMode = enExtremePointMode.极大值;

        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;

        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }
        public double OffsetValue { get => offsetValue; set => offsetValue = value; }
        public enExtremePointCoord ExtremePointCoord { get => extremePointCoord; set => extremePointCoord = value; }
        public enExtremePointMode ExtremePointMode { get => extremePointMode; set => extremePointMode = value; }



        public ExtremePoint3D()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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
            return listObjectModel3D.ToArray();
        }
        private bool getExtremePoint(HObjectModel3D[] objectModel3D, out HObjectModel3D[] selectObjectModel3D,out double [] x_value, out double[] y_value, out double[] z_value)
        {
            bool result = false;
            selectObjectModel3D = new HObjectModel3D[0];
            x_value = new double[0];
            y_value = new double[0];
            z_value = new double[0];
            if (objectModel3D == null)
            {
                throw new ArgumentNullException("objectModel3D");
            }
            if (objectModel3D.Length == 0)
            {
                throw new ArgumentException("objectModel3D", "参数长度为0");
            }
            HTuple x1_ave, y1_ave, z1_ave;
            selectObjectModel3D = new HObjectModel3D[objectModel3D.Length];
            x_value = new double[objectModel3D.Length];
            y_value = new double[objectModel3D.Length];
            z_value = new double[objectModel3D.Length];
            for (int i = 0; i < objectModel3D.Length; i++)
            {
                if (objectModel3D[i].GetObjectModel3dParams("num_points").I == 0) continue;
                x1_ave = objectModel3D[i].GetObjectModel3dParams("point_coord_x").TupleSort();
                y1_ave = objectModel3D[i].GetObjectModel3dParams("point_coord_y").TupleSort();
                z1_ave = objectModel3D[i].GetObjectModel3dParams("point_coord_z").TupleSort();
                switch (this.extremePointCoord)
                {
                    case enExtremePointCoord.X轴:
                        switch (this.extremePointMode)
                        {
                            case enExtremePointMode.极大值:
                                selectObjectModel3D[i] = objectModel3D[i].SelectPointsObjectModel3d("point_coord_x", x1_ave.TupleMax().D - this.offsetValue, x1_ave.TupleMax().D);
                                break;
                            case enExtremePointMode.极小值:
                                selectObjectModel3D[i] = objectModel3D[i].SelectPointsObjectModel3d("point_coord_x", x1_ave.TupleMin().D, x1_ave.TupleMin().D + this.offsetValue);
                                break;
                        }
                        x_value[i] = selectObjectModel3D[i].GetObjectModel3dParams("point_coord_x").TupleMean().D;
                        break;
                    case enExtremePointCoord.Y轴:
                        switch (this.extremePointMode)
                        {
                            case enExtremePointMode.极大值:
                                selectObjectModel3D[i] = objectModel3D[i].SelectPointsObjectModel3d("point_coord_y", y1_ave.TupleMax().D - this.offsetValue, y1_ave.TupleMax().D);
                                break;
                            case enExtremePointMode.极小值:
                                selectObjectModel3D[i] = objectModel3D[i].SelectPointsObjectModel3d("point_coord_y", y1_ave.TupleMin().D, y1_ave.TupleMin().D + this.offsetValue);
                                break;
                        }
                        y_value[i] = selectObjectModel3D[i].GetObjectModel3dParams("point_coord_y").TupleMean().D;
                        break;
                    case enExtremePointCoord.Z轴:
                        switch (this.extremePointMode)
                        {
                            case enExtremePointMode.极大值:
                                selectObjectModel3D[i] = objectModel3D[i].SelectPointsObjectModel3d("point_coord_z", z1_ave.TupleMax().D - this.offsetValue, z1_ave.TupleMax().D);
                                break;
                            case enExtremePointMode.极小值:
                                selectObjectModel3D[i] = objectModel3D[i].SelectPointsObjectModel3d("point_coord_z", z1_ave.TupleMin().D, z1_ave.TupleMin().D + this.offsetValue);
                                break;
                        }
                        z_value[i] = selectObjectModel3D[i].GetObjectModel3dParams("point_coord_z").TupleMean().D;
                        break;
                }
            }
            result = true;
            return result;
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.selectObjectModel3D);
                Result.Succss = getExtremePoint(extractRefSource1Data(), out this.selectObjectModel3D,out this.x_value,out this.y_value,out  z_value);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this.x_value.Average());
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this.y_value.Average());
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this.z_value.Average());

                OnExcuteCompleted(this.name, this.selectObjectModel3D);
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
                    return extractRefSource1Data(); //
                case "结果":
                case "极值点":
                case "极值对象":
                case "3D对象":
                    return this.selectObjectModel3D; //
                default:
                    if (this.name == propertyName)
                        return this.selectObjectModel3D;
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
                //////////////////////////////////////////////                        
                default:
                    return true;
            }
        }

        public void ReleaseHandle()
        {

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
        }
    }
}
