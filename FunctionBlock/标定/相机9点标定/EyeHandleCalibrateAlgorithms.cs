using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public class EyeHandleCalibrateAlgorithms
    {

        /// <summary>
        /// 九点标定，用于XY轴标定
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="Rows"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="HomMat2D00"></param>
        public static void Calibra9Point(HTuple Rows, HTuple Columns, HTuple x, HTuple y, out HHomMat2D HomMat2D, out double MaxError)
        {
            HomMat2D = new HHomMat2D();
            HTuple Qx, Qy, DistTest;
            HomMat2D.VectorToHomMat2d(Columns, Rows, x, y);
            Qx = HomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            DistTest = HMisc.DistancePp(x, y, Qx, Qy);
            MaxError = DistTest.TupleMax().D;
            /////////////////////////////////////////
        }

        /// <summary>
        /// 相似九点标定,用于单独标定 
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="Rows"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="HomMat2D00"></param>
        public static void Calibra9PointtSimilar(HTuple Columns, HTuple Rows, HTuple x, HTuple y, out HHomMat2D HomMat2D, out double MaxError)
        {
            HomMat2D = new HHomMat2D();
            HTuple Qx, Qy, DistTest;
            HomMat2D.VectorToSimilarity(Columns, Rows, x, y);
            Qx = HomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            DistTest = HMisc.DistancePp(x, y, Qx, Qy);
            MaxError = DistTest.TupleMax().D;
            /////////////////////////////////////////
        }

        /// <summary>
        ///  获取手眼标定的9点
        /// </summary>
        /// <param name="StartPt"></param>
        /// <param name="EndPt"></param>
        /// <param name="StepCount"></param>
        /// <returns></returns>
        public static List<userWcsVector> GetHandEyeMotionPt(userWcsVector StartPt, userWcsVector EndPt, int StepCount = 3)
        {
            List<userWcsVector> MotionPosW = new List<userWcsVector>();
            userWcsVector startPt = new userWcsVector(Math.Min(StartPt.X, EndPt.X), Math.Min(StartPt.Y, EndPt.Y));
            userWcsVector endPt = new userWcsVector(Math.Max(StartPt.X, EndPt.X), Math.Max(StartPt.Y, EndPt.Y));
            double recWidth = endPt.X - startPt.X;
            double recHeight = endPt.Y - startPt.Y;
            double stepX = recWidth / (StepCount - 1);
            double stepY = recHeight / (StepCount - 1);
            for (int row = 0; row <= StepCount - 1; row += 1)
            {
                for (int col = 0; col <= StepCount - 1; col += 1)
                {
                    //九点标定时的点，坐下角为第一个点，右上角为第九个点
                    MotionPosW.Add(new userWcsVector(startPt.X + col * stepX, startPt.Y + row * stepY));
                }
            }
            return MotionPosW;
        }

        public static void ListPt2dToHTuple(List<userPixVector> ListPtIn, out HTuple Rows, out HTuple Cols)
        {
            Rows = new HTuple();
            Cols = new HTuple();
            for (int i = 0; i < ListPtIn.Count; i++)
            {
                Rows[i] = ListPtIn[i].Row;
                Cols[i] = ListPtIn[i].Col;
            }
        }

        public static void FitCircle(HTuple Rows, HTuple Cols, out double CenterRow, out double CenterCol, out double radius)
        {
            CenterRow = 0;
            CenterCol = 0;
            radius = 0;
            HTuple Row, Column, StartPhi, Radius, EndPhi, PointOrder;
            if (Rows == null)
                throw new ArgumentNullException("Rows");
            if (Cols == null)
                throw new ArgumentNullException("Cols");
            if (Rows.Length < 3 || Cols.Length < 3)
                throw new ArgumentException("点数不能小于3个");
            if (Rows.Length != Cols.Length)
                throw new ArgumentException("行和列的点数不一致");
            /////////////////////////////////////////////////////////////////////////////////////////////
            new HXLDCont(Rows, Cols).FitCircleContourXld("geometric", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            if (Row.Length > 0)
            {
                CenterRow = Row[0].D;
                CenterCol = Column[0].D;
                radius = Radius[0].D;
            }
        }
        public static void Pt2dToHTuple(List<userWcsVector> Pt2Ds, out HTuple X, out HTuple Y)
        {
            X = new HTuple();
            Y = new HTuple();
            for (int i = 0; i < Pt2Ds.Count(); i++)
            {
                X[i] = Pt2Ds[i].X ;
                Y[i] = Pt2Ds[i].Y;
            }
        }




    }




}
