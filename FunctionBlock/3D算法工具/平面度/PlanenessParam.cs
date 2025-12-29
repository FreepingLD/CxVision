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
    public class PlanenessParam
    {
        private double _Value; // 测量结果

        [DisplayName("平面度")]
        [DescriptionAttribute("输出属性")]
        public double Value { get => _Value; set => _Value = value; }


        public string FittingAlgorithm { get; set; }





        public PlanenessParam()
        {
            this.FittingAlgorithm = "least_squares_tukey";
        }





        public bool CalculatePlaneness(HObjectModel3D objectModel3D, out double planeness)
        {
            bool result = false;
            planeness = 0;
            HObjectModel3D fitPlaneObjectModel = null;
            HTuple ParamValue = null;
            HTuple pose = null;
            HObjectModel3D planeObjectModel = new HObjectModel3D();
            /////////////////////////////
            if (objectModel3D == null)
            {
                throw new ArgumentNullException("objectModel3D");
            }
            /////////////////////////////
            HTuple num = objectModel3D.GetObjectModel3dParams("num_points").I;
            HTuple x = objectModel3D.GetObjectModel3dParams("point_coord_x");
            HTuple y = objectModel3D.GetObjectModel3dParams("point_coord_y");
            if (num.I < 3 || (x.TupleMax() - x.TupleMin()).D < 0.01 || (y.TupleMax() - y.TupleMin()).D < 0.01) // 少于3个点是不能拟合平面的,表示是定点采集
            {

                fitPlaneObjectModel = objectModel3D.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_tukey"));
                pose = fitPlaneObjectModel.GetObjectModel3dParams("primitive_pose");
                planeObjectModel.GenPlaneObjectModel3d(new HPose(pose), new HTuple(), new HTuple());
                objectModel3D.DistanceObjectModel3d(planeObjectModel, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                ParamValue = objectModel3D.GetObjectModel3dParams("&distance");
                planeness = ParamValue.TupleMax().D - ParamValue.TupleMin().D;
                planeObjectModel.Dispose();
                result = true;
            }
            return result;
        }
        public bool CalculatePlaneness(HObjectModel3D[] objectModel3D, out double planeness)
        {
            bool result = false;
            planeness = 0;
            HObjectModel3D fitPlaneObjectModel = null;
            HTuple ParamValue = null;
            HTuple pose = null;
            HObjectModel3D planeObjectModel = null;
            HObjectModel3D unionObjectModel = null;
            /////////////////////////////
            if (objectModel3D == null)
            {
                throw new ArgumentNullException("objectModel3D");
            }
            try
            {
                /////////////////////////////
                unionObjectModel = HObjectModel3D.UnionObjectModel3d(objectModel3D, "points_surface");
                HTuple num = unionObjectModel.GetObjectModel3dParams("num_points").I;
                HTuple x = unionObjectModel.GetObjectModel3dParams("point_coord_x");
                HTuple y = unionObjectModel.GetObjectModel3dParams("point_coord_y");
                if (num.I < 3)
                {
                    throw new ArgumentException("拟合平面至少需要三个点");
                }
                if ((x.TupleMax() - x.TupleMin()).D < 0.01 || (y.TupleMax() - y.TupleMin()).D < 0.01)
                {
                    throw new ArgumentException("拟合平面的点不能共线!");
                }
                fitPlaneObjectModel = unionObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_tukey"));
                pose = fitPlaneObjectModel.GetObjectModel3dParams("primitive_pose");
                planeObjectModel.GenPlaneObjectModel3d(new HPose(pose), new HTuple(), new HTuple());
                unionObjectModel.DistanceObjectModel3d(planeObjectModel, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                ParamValue = unionObjectModel.GetObjectModel3dParams("&distance");
                planeness = ParamValue.TupleMax().D - ParamValue.TupleMin().D;
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error("CalculatePlaneness " + "算子执行错误!");
            }
            finally
            {
                unionObjectModel?.ClearObjectModel3d();
                planeObjectModel?.ClearObjectModel3d();
                fitPlaneObjectModel?.ClearObjectModel3d();
            }
            return result;
        }








    }
}
