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

namespace AlgorithmsLibrary
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class SampleXLDAlgorith
    {
        private double sample_dist = 10;
        private double smooth_value = 15;
        private bool isSmooth = false;
        [NonSerialized]
        private XldDataClass smoothXldData = null;
        private userPixPoint[] pixPoint;
        private userWcsPoint[] wcsPoint;

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
        public bool IsSmooth { get => isSmooth; set => isSmooth = value; }





        public bool sampleXLD2(XldDataClass xldData)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            if (xldData == null) return result;
            //////////////////////////////////////
            HTuple rows = new HTuple();
            HTuple cols = new HTuple();
            HTuple sort_row, sort_col, row, col;
            HTuple smooth_row = new HTuple();
            HTuple smooth_col = new HTuple();
            int count = xldData.HXldCont.CountObj();
            for (int i = 0; i < count; i++)
            {
                xldData.HXldCont.SelectObj(i + 1).GetContourXld(out row, out col);
                rows.Append(row);
                cols.Append(col);
            }
            ha.sortPairs(rows, cols, 2, out sort_row, out sort_col); // 排一次序，便与轮廓绘制
            HFunction1D hFunction1D = new HFunction1D(sort_col, sort_row);
            HFunction1D hFunction1DSmooth, sampFunction;
            /////////////////////
            if (isSmooth)
            {
                //switch ("")
                //{
                //    case "平滑":
                //        hFunction1DSmooth = hFunction1D.SmoothFunct1dMean((int)this.smooth_value, 3);
                //        sampFunction = hFunction1DSmooth.SampleFunct1d(sort_col.TupleMin().D, sort_col.TupleMax().D, sample_dist, "constant");
                //        sampFunction.Funct1dToPairs(out smooth_col, out smooth_row);
                //        break;
                //    case "高斯":
                //        hFunction1DSmooth = hFunction1D.SmoothFunct1dGauss(this.smooth_value);
                //        sampFunction = hFunction1DSmooth.SampleFunct1d(sort_col.TupleMin().D, sort_col.TupleMax().D, sample_dist, "constant");
                //        sampFunction.Funct1dToPairs(out smooth_col, out smooth_row);
                //        break;
                //}
            }
            else
            {
                sampFunction = hFunction1D.SampleFunct1d(sort_col.TupleMin().D, sort_col.TupleMax().D, sample_dist, "constant");
                sampFunction.Funct1dToPairs(out smooth_col, out smooth_row);
            }
            ////////////
            this.pixPoint = new userPixPoint[smooth_col.Length];
            this.wcsPoint = new userWcsPoint[smooth_col.Length];
            for (int i = 0; i < smooth_col.Length; i++)
            {
                this.pixPoint[i] = new userPixPoint(smooth_row[i], smooth_col[i], xldData.CamParams);
                this.wcsPoint[i] = this.pixPoint[i].GetWcsPoint();
            }
            this.smoothXldData = new XldDataClass(new HXLDCont(smooth_row, smooth_col), xldData.CamParams);
            //////////////////
            result = true;
            return result;
        }

        public bool sampleXLD(XldDataClass xldData)
        {
            bool result = false;
            if (xldData == null) return result;
            //////////////////////////////////////
            HTuple rows = new HTuple();
            HTuple cols = new HTuple();
            double row, col;
            HTuple smooth_row = new HTuple();
            HTuple smooth_col = new HTuple();
            //////////////////////////
            HXLDCont smoothhXLDCont=new HXLDCont();
            HXLDCont unionhXLDCont = xldData.HXldCont.UnionAdjacentContoursXld(xldData.HXldCont.LengthXld().TupleMin(), xldData.HXldCont.LengthXld().TupleMin()*0.5, "attr_keep");
            if (isSmooth)
            {
                smoothhXLDCont = unionhXLDCont.SmoothContoursXld((int)this.smooth_value);
            }
            else
            {
                smoothhXLDCont = unionhXLDCont;
            }
            //////////////////////////////////
            if (smoothhXLDCont == null || smoothhXLDCont.CountObj() > 1) return result;
            smoothhXLDCont.GetContourXld(out rows, out cols);
            int count = (int)(smoothhXLDCont.LengthXld().D / sample_dist);
            for (int i = 0; i < count+1; i++)
            {
                GetPointXLD(smoothhXLDCont, rows[0].D, cols[0].D, sample_dist * i, out row, out col);
                smooth_row.Append(row);
                smooth_col.Append(col);
            }
            smooth_row.Append(rows[rows.Length-1]); // 保证首尾点都包含在内
            smooth_col.Append(cols[cols.Length-1]);
            ////////////
            this.pixPoint = new userPixPoint[smooth_col.Length];
            this.wcsPoint = new userWcsPoint[smooth_col.Length];
            for (int i = 0; i < smooth_col.Length; i++)
            {
                this.pixPoint[i] = new userPixPoint(smooth_row[i], smooth_col[i], xldData.CamParams);
                this.wcsPoint[i] = this.pixPoint[i].GetWcsPoint();
            }
            this.smoothXldData = new XldDataClass(new HXLDCont(smooth_row, smooth_col), xldData.CamParams);
            //////////////////
            result = true;
            return result;
        }

        /// <summary>
        /// 获取直线上指定长度处的点坐标
        /// </summary>
        /// <param name="RowBegin"></param>
        /// <param name="ColumnBegin"></param>
        /// <param name="RowEnd"></param>
        /// <param name="ColumnEnd"></param>
        /// <param name="length"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        private void GetPointXLD(HXLDCont xld, double startPointRow, double startPointCol, double length, out double Row, out double Col)
        {
            HTuple row, col;
            //////////////////
            HOperatorSet.IntersectionCircleContourXld(xld, startPointRow, startPointCol, length, 0, Math.PI * 2, "positive", out row, out col);
            if (row.Length > 0)
            {
                Row = row.D;
                Col = col.D;
            }
            else
            {
                Row = startPointRow;
                Col = startPointCol;
            }
        }




    }
}
