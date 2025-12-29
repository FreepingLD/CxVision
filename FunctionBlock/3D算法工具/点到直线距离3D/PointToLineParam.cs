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
    public class PointToLineParam 
    {
        private int average_count = 5;
        private string  _ValueType = "单个值";
        public int Average_count
        {
            get
            {
                return average_count;
            }

            set
            {
                average_count = value;
            }
        }

        public string ValueType { get => _ValueType; set => _ValueType = value; }


        public PointToLineParam()
        {

        }


        private bool CalculaePointToLine3DDist(HObjectModel3D[] pointObjectModel, HObjectModel3D[] lineObjcetModel,  out double[] maxValue, out double[] minValue, out double[] meanValue)
        {
            bool result = false;
            maxValue = new double[0];
            minValue = new double[0];
            meanValue = new double[0];
            ///////////////////////////
            if (pointObjectModel == null)
            {
                throw new ArgumentNullException("pointObjectModel", "参数pointObjectModel为空");
            }
            if (lineObjcetModel == null)
            {
                throw new ArgumentNullException("lineObjcetModel", "参数lineObjcetModel为空");
            }
            if ( pointObjectModel.Length == 0)
            {
                throw new ArgumentNullException("pointObjectModel", "参数pointObjectModel长度为0");
            }
            if ( lineObjcetModel.Length == 0)
            {
                throw new ArgumentNullException("lineObjcetModel", "参数lineObjcetModel长度为0");
            }

            double RowBegin, ColBegin, RowEnd, ColEnd;
            double[] x, y;
            HalconLibrary ha = new HalconLibrary();
            ///////////////////////////////////////////////////////////////
            switch (this.ValueType)
            {
                default:
                case nameof(enValueType.单个值):

                    break;
                case nameof(enValueType.多个值):
                    if (lineObjcetModel.Length > 1 && lineObjcetModel.Length != pointObjectModel.Length)
                    {
                        throw new ArgumentException("两参数pointObjectModel和lineObjcetModel的长度不一致");
                    }

                    for (int i = 0; i < lineObjcetModel.Length; i++)
                    {

                    }
                    break;
            }


            ////////////////////////////
            maxValue = new double[lineObjcetModel.Length];
            minValue = new double[lineObjcetModel.Length];
            meanValue = new double[lineObjcetModel.Length];
            for (int i = 0; i < pointObjectModel.Length; i++)
            {
                if(lineObjcetModel.Length>1)
                {
                    if (lineObjcetModel[i].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                    }
                }
                else
                {
                    if (lineObjcetModel[0].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                    }
                }
                if (pointObjectModel[i].GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("对象中不包含数据点", "targetObjectModel");
                }
                if (lineObjcetModel.Length > 1)
                    ha.FitLine3D(lineObjcetModel[i], 3, out RowBegin, out ColBegin, out RowEnd, out ColEnd);
                else
                    ha.FitLine3D(lineObjcetModel[0], 3, out RowBegin, out ColBegin, out RowEnd, out ColEnd);
                ha.TransformModel3DTo2DPoint(pointObjectModel[i], 3, out x, out y);
                HTuple value = HMisc.DistancePl(y, x, RowBegin, ColBegin, RowEnd, ColEnd);
                // 计算最大值、最小值、平均值
                if (value.Length > this.average_count)
                {
                    maxValue[i] = ((value.TupleSort().TupleSelectRange(value.Length - average_count, value.Length - 1).TupleSum().D / average_count));
                    minValue[i] = ((value.TupleSort().TupleSelectRange(0, average_count - 1).TupleSum().D / average_count));
                    meanValue[i] = ((value.TupleMean()).D); // 计算所有点的平面值
                }
                else // 当总点数小于平均点数的时候
                {
                    maxValue[i] = ((value.TupleSort().TupleSelect(value.Length - 1)).D);
                    minValue[i] = ((value.TupleSort().TupleSelect(0)).D);
                    meanValue[i] = ((value.TupleMean()).D); // 计算所有点的平面值
                }
            }
            result = true;
            return result;
        }




    }

    public enum enValueType
    {
        单个值,
        多个值,
    }
}
