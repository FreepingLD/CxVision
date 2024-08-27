using Sensor;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.IO;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光扫描采集的数据类
    /// </summary>
    public class CalibrateMethod
    {

        private static object lockState = new object();
        private static CalibrateMethod _Instance = null;
        private CalibrateMethod()
        {

        }

        public static CalibrateMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new CalibrateMethod();
                    }
                }
                return _Instance;
            }
        }


        public UserHomMat2D NpointCalib(BindingList<userPixPoint> pixPoint, BindingList<userWcsPoint> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row);
                Columns.Append(item.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib(BindingList<userPixPoint> pixPoint, BindingList<CoordSysAxisParam> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row);
                Columns.Append(item.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }

        public UserHomMat2D NpointCalib(double[] rows, double[] cols, double[] x, double[] y, out double error)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (cols == null)
            {
                throw new ArgumentNullException("cols");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (rows.Length != cols.Length)
            {
                throw new ArgumentException("rows 与 cols");
            }
            if (x.Length != y.Length)
            {
                throw new ArgumentException("x 与 y");
            }
            if (rows.Length != y.Length)
            {
                throw new ArgumentException("rows 与 y");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(cols, rows, x, y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(cols, rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, x, y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib(HTuple rows, HTuple cols, HTuple x, HTuple y, out double error)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (cols == null)
            {
                throw new ArgumentNullException("cols");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (rows.Length != cols.Length)
            {
                throw new ArgumentException("rows 与 cols");
            }
            if (x.Length != y.Length)
            {
                throw new ArgumentException("x 与 y");
            }
            if (rows.Length != y.Length)
            {
                throw new ArgumentException("rows 与 y");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(cols, rows, x, y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(cols, rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, x, y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib(Dictionary<string, userPixPoint> pixPoint, BindingList<CoordSysAxisParam> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (KeyValuePair<string, userPixPoint> item in pixPoint)
            {
                Rows.Append(item.Value.Row);
                Columns.Append(item.Value.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib2(Dictionary<string, userPixPoint> pixPoint, BindingList<CoordSysAxisParam> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (KeyValuePair<string, userPixPoint> item in pixPoint)
            {
                Rows.Append(item.Value.Row);
                Columns.Append(item.Value.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib(Dictionary<string, userPixPoint> pixPoint, double offsetRow, double offsetCol, BindingList<CoordSysAxisParam> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (KeyValuePair<string, userPixPoint> item in pixPoint)
            {
                Rows.Append(item.Value.Row + offsetRow);
                Columns.Append(item.Value.Col + offsetCol);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib(BindingList<userPixPoint> pixPoint, double offsetRow, double offsetCol, BindingList<CoordSysAxisParam> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row + offsetRow);
                Columns.Append(item.Col + offsetCol);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib(double[] rows, double[] cols, double offsetRow, double offsetCol, double[] x, double[] y, out double error)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (cols == null)
            {
                throw new ArgumentNullException("cols");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (rows.Length != cols.Length)
            {
                throw new ArgumentException("rows 与 cols");
            }
            if (x.Length != y.Length)
            {
                throw new ArgumentException("x 与 y");
            }
            if (rows.Length != y.Length)
            {
                throw new ArgumentException("rows 与 y");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(new HTuple(cols) + offsetCol, new HTuple(rows) + offsetRow, x, y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(cols, rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, x, y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalib2(Dictionary<string, userPixPoint> pixPoint, double offsetRow, double offsetCol, BindingList<CoordSysAxisParam> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (KeyValuePair<string, userPixPoint> item in pixPoint)
            {
                Rows.Append(item.Value.Row + offsetRow);
                Columns.Append(item.Value.Col + offsetCol);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }

        public UserHomMat2D NpointCalib(Dictionary<string, userPixPoint> pixPoint, BindingList<userWcsPoint> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (KeyValuePair<string, userPixPoint> item in pixPoint)
            {
                Rows.Append(item.Value.Row);
                Columns.Append(item.Value.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);

            //hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }
        public UserHomMat2D NpointCalibSingleAxis(BindingList<userPixPoint> pixPoint, BindingList<userWcsPoint> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row);
                Columns.Append(item.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }

        public UserHomMat2D NpointCalibSingleAxis(Dictionary<string, userPixPoint> pixPoint, BindingList<userWcsPoint> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Value.Row);
                Columns.Append(item.Value.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }

        public UserHomMat2D NpointCalibSingleAxis(Dictionary<string, userPixPoint> pixPoint, BindingList<CoordSysAxisParam> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Value.Row);
                Columns.Append(item.Value.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis(BindingList<userPixPoint> pixPoint, BindingList<CoordSysAxisParam> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row);
                Columns.Append(item.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            X = X - X.TupleMean();
            Y = Y - Y.TupleMean();
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis2(Dictionary<string, userPixPoint> pixPoint, BindingList<CoordSysAxisParam> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Value.Row);
                Columns.Append(item.Value.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis(Dictionary<string, userPixPoint> pixPoint, double offsetRow, double offsetCol, BindingList<CoordSysAxisParam> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Value.Row + offsetRow);
                Columns.Append(item.Value.Col + offsetCol);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis2(Dictionary<string, userPixPoint> pixPoint, double offsetRow, double offsetCol, BindingList<CoordSysAxisParam> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Value.Row + offsetRow);
                Columns.Append(item.Value.Col + offsetCol);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis(BindingList<userPixPoint> pixPoint, double offsetRow, double offsetCol, BindingList<CoordSysAxisParam> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row + offsetRow);
                Columns.Append(item.Col + offsetCol);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis(double[] rows, double[] cols, double[] x, double[] y, enInvertAxis InvertAxis, out double error)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (cols == null)
            {
                throw new ArgumentNullException("cols");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (rows.Length != cols.Length)
            {
                throw new ArgumentException("rows 与 cols");
            }
            if (x.Length != y.Length)
            {
                throw new ArgumentException("x 与 y");
            }
            if (rows.Length != y.Length)
            {
                throw new ArgumentException("rows 与 y");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToSimilarity(cols, rows, x, y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(cols, rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, x, y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis(HTuple rows, HTuple cols, HTuple x, HTuple y, enInvertAxis InvertAxis, out double error)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (cols == null)
            {
                throw new ArgumentNullException("cols");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (rows.Length != cols.Length)
            {
                throw new ArgumentException("rows 与 cols");
            }
            if (x.Length != y.Length)
            {
                throw new ArgumentException("x 与 y");
            }
            if (rows.Length != y.Length)
            {
                throw new ArgumentException("rows 与 y");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToSimilarity(cols, rows, x, y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(cols, rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, x, y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }
        public UserHomMat2D NpointCalibSingleAxis2(BindingList<userPixPoint> pixPoint, double offsetRow, double offsetCol, BindingList<CoordSysAxisParam> wcsPoint, enInvertAxis InvertAxis, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row + offsetRow);
                Columns.Append(item.Col + offsetCol);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToSimilarity(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            UserHomMat2D homMat2D = new UserHomMat2D(hHomMat2D);
            switch (InvertAxis)
            {
                case enInvertAxis.X轴:
                    homMat2D.c00 *= -1;
                    break;
                case enInvertAxis.Y轴:
                    homMat2D.c11 *= -1;
                    break;
            }
            return homMat2D;
        }


        /// <summary>
        /// 单轴标定
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="IsInvert_x"></param>
        /// <param name="IsInvert_y"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public UserHomMat2D NpointCalibSingleAxis(HTuple row, HTuple col, HTuple x, HTuple y, bool IsInvert_x, bool IsInvert_y, out double error)
        {
            error = 0;
            UserHomMat2D HomMat2D = new UserHomMat2D();
            HTuple DistPix = 0, DistWcs = 0;
            HOperatorSet.DistancePp(row[0].D, col[0].D, row[row.Length - 1].D, col[col.Length - 1].D, out DistPix);
            HOperatorSet.DistancePp(x[0].D, y[0].D, x[x.Length - 1].D, y[y.Length - 1].D, out DistWcs);
            double dist_row = Math.Abs((row[row.Length - 1].D - row[0].D));
            double dist_col = Math.Abs((col[col.Length - 1].D - col[0].D));
            double phi = Math.Atan2(col[col.Length - 1].D - col[0].D, row[row.Length - 1].D - row[0].D) * -1;
            int step = row.Length > col.Length ? row.Length : col.Length;
            double offsetPix = DistPix.D / (step - 1);
            double offsetWcs = DistWcs.D / (step - 1);
            List<double> list_row = new List<double>();
            List<double> list_col = new List<double>();
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            HTuple P_x = new HTuple();
            HTuple P_y = new HTuple();
            HTuple Q_x = new HTuple();
            HTuple Q_y = new HTuple();
            /////////////////////////////////
            for (int i = (int)((step - 1) * -0.5); i < (int)((step + 1) * 0.5); i++)
            {
                for (int k = (int)((step - 1) * -0.5); k < (int)((step + 1) * 0.5); k++)
                {
                    list_row.Add(offsetPix * i);
                    list_col.Add(offsetPix * k);
                    list_x.Add(offsetWcs * k);
                    list_y.Add(offsetWcs * i - 1);
                }
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            List<double> temp_row = new List<double>();
            List<double> temp_col = new List<double>();
            if (dist_row > dist_col) // 表示标定单轴Y
            {
                for (int i = 0; i < list_row.Count; i++)
                {
                    if (list_col[i] == 0)
                    {
                        temp_row.Add(list_row[i]);
                        temp_col.Add(list_col[i]);
                    }
                }
            }
            else // 表示标定单轴X
            {
                for (int i = 0; i < list_row.Count; i++)
                {
                    if (list_row[i] == 0)
                    {
                        temp_row.Add(list_row[i]);
                        temp_col.Add(list_col[i]);
                    }
                }
            }
            hHomMat2D.VectorToRigid(temp_row.ToArray(), temp_col.ToArray(), row, col); // 大于或等于两个点
            // hHomMat2D.VectorAngleToRigid(list_row[(int)((step * step - 1) * 0.5)], list_col[(int)((step * step - 1) * 0.5)], 0, row[(int)((step - 1) * 0.5)], col[(int)((step - 1) * 0.5)], phi);
            P_y = hHomMat2D.AffineTransPoint2d(list_row.ToArray(), list_col.ToArray(), out P_x);
            Q_x = new HTuple(list_x.ToArray());
            Q_y = new HTuple(list_y.ToArray());
            if (IsInvert_x)
                Q_x *= -1;
            if (IsInvert_y)
                Q_y *= -1;
            //////////////////////////////
            hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(P_x, P_y, Q_x, Q_y);
            HTuple OutQx = new HTuple(), OutQy = new HTuple();
            HOperatorSet.AffineTransPoint2d(hHomMat2D, P_x, P_y, out OutQx, out OutQy);
            HTuple DistTest = new HTuple();
            HOperatorSet.DistancePp(Q_x, Q_y, OutQx, OutQy, out DistTest);
            HTuple MaxDist = new HTuple();
            HOperatorSet.TupleMax(DistTest, out MaxDist);
            HomMat2D = new UserHomMat2D(hHomMat2D);
            error = MaxDist.D;
            return HomMat2D;
        }
        public UserHomMat2D NpointCalib2(HTuple Rows, HTuple Columns, HTuple X, HTuple Y, out double error)
        {
            if (Rows == null)
            {
                throw new ArgumentNullException("Rows");
            }
            if (Columns == null)
            {
                throw new ArgumentNullException("Columns");
            }
            if (X == null)
            {
                throw new ArgumentNullException("X");
            }
            if (Y == null)
            {
                throw new ArgumentNullException("Y");
            }
            if (Rows.Length != X.Length)
            {
                throw new ArgumentException("Rows 与 X长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }


        /// <summary>
        /// 有两个圆心，手动选择
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Cols"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="centerRow"></param>
        /// <param name="centerCol"></param>
        /// <param name="centerRow2"></param>
        /// <param name="centerCol2"></param>
        /// 
        public void CalculateFitCenter(double[] Rows, double[] Cols, out double centerRow, out double centerCol)
        {
            if (Rows.Length < 2)
                throw new ArgumentException("元素个数不能小于2");
            if (Rows.Length != Cols.Length)
                throw new ArgumentException("输入元素个数不相等");
            // 计算圆心的方向
            double Row, Column, Radius, StartPhi, EndPhi;
            string PointOrder;
            new HXLDCont(Rows, Cols).FitCircleContourXld("algebraic", -1, 0.0, 0, 5, 2.0, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            centerRow = Row;
            centerCol = Column;
        }
        public void CalculateCenterXzPlane(double[] x, double[] y, double[] angle, out double center_x, out double center_z, out double radius)
        {
            if (x.Length < 3)
                throw new ArgumentException("元素个数不能小于3");
            if (x.Length != y.Length)
                throw new ArgumentException("输入元素个数不相等");
            //////////////////////////////////////////////////////
            int index = (int)((angle.Length - 1) * 0.5);
            double stepAngle = 0, dist_x = 0;
            double[] z = new double[angle.Length];
            double[] new_x = new double[angle.Length];
            for (int i = 0; i < angle.Length; i++)
            {
                stepAngle = Math.Abs(angle[i] - angle[index]);
                dist_x = Math.Abs(x[i] - x[index]);
                new_x[i] = x[i] - x[index];
                z[i] = Math.Tan(stepAngle * 0.5 * Math.PI / 180) * dist_x * -1;
            }
            // 计算圆心的方向
            double Row, Column, Radius, StartPhi, EndPhi;
            string PointOrder;
            new HXLDCont(new_x, z).FitCircleContourXld("algebraic", -1, 0.0, 0, 10, 2.0, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            center_x = Row;
            center_z = Column;
            radius = Radius;
        }
        public void CalculateCenterAngleMethod(double[] Rows, double[] Cols, double rotateAngle, out double centerRow, out double centerCol, out double error)
        {
            if (Rows.Length < 2)
                throw new ArgumentException("元素个数不能小于2");
            if (Rows.Length != Cols.Length)
                throw new ArgumentException("输入元素个数不相等");
            double line1Row1, line1Row2, line1Col1, line1Col2;
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listRow2 = new List<double>();
            List<double> listCol2 = new List<double>();
            List<double> radius = new List<double>();
            List<double> list_error = new List<double>();
            HTuple row, col;
            for (int i = 0; i < Rows.Length - 1; i++)
            {
                NormalLine(Rows[i], Cols[i], Rows[i + 1], Cols[i + 1], out line1Row1, out line1Col1, out line1Row2, out line1Col2);
                double dist = HMisc.DistancePp(Rows[i], Cols[i], Rows[i + 1], Cols[i + 1]);
                double R = dist * 0.5 / Math.Tan(rotateAngle * 0.5 * Math.PI / 180.0);
                radius.Add(R);
                HOperatorSet.IntersectionLineCircle(line1Row1, line1Col1, line1Row2, line1Col2, (Rows[i] + Rows[i + 1]) * 0.5, (Cols[i] + Cols[i + 1]) * 0.5,
                R, 0, Math.PI * 2, "positive", out row, out col);
                ///////////////////////////
                if (row.Length > 0)
                {
                    listRow.Add(row[0]);
                    listCol.Add(col[0]);
                }
                if (row.Length > 1)
                {
                    listRow2.Add(row[1]);
                    listCol2.Add(col[1]);
                }
            }
            // 计算圆心的方向
            double Row, Column, Radius, StartPhi, EndPhi;
            string PointOrder;
            new HXLDCont(Rows, Cols).FitCircleContourXld("algebraic", -1, 0.0, 0, 5, 2.0, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            if (HMisc.DistancePp(Row, Column, listRow.Average(), listCol.Average()) < HMisc.DistancePp(Row, Column, listRow2.Average(), listCol2.Average()))
            {
                centerRow = listRow.Average();
                centerCol = listCol.Average();
                for (int i = 0; i < listRow.Count; i++)
                {
                    list_error.Add(HMisc.DistancePp(listRow[i], listCol[i], centerRow, centerCol));
                }
            }
            else
            {
                centerRow = listRow2.Average();
                centerCol = listCol2.Average();
                for (int i = 0; i < listRow2.Count; i++)
                {
                    list_error.Add(HMisc.DistancePp(listRow2[i], listCol2[i], centerRow, centerCol));
                }
            }
            error = list_error.Average(); // 圆心平均误差
        }

        private void NormalLine(double Row1, double Column1, double Row2, double Column2, out double lineRow1, out double lineCol1, out double lineRow2, out double lineCol2)
        {
            double mid_row = (Row1 + Row2) * 0.5;
            double mid_col = (Column1 + Column2) * 0.5;
            double ATan = Math.Atan2(Row1 - Row2, Column1 - Column2);
            double k = 1 / Math.Tan(ATan) * -1;
            double b = mid_row - mid_col * k;
            ///////////////////
            double y = (mid_col + 1) * k + b;
            lineRow1 = mid_row;
            lineCol1 = mid_col;
            lineRow2 = y;
            lineCol2 = mid_col + 1;
        }
        public void CalculateCenter(BindingList<userWcsPoint> wcsPoint, double rotateAngle, out double centerRow, out double centerCol, out double error)
        {
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            double[] X = new double[wcsPoint.Count];
            double[] Y = new double[wcsPoint.Count];
            centerRow = 0;
            centerCol = 0;
            for (int i = 0; i < wcsPoint.Count; i++)
            {
                X[i] = wcsPoint[i].X;
                Y[i] = wcsPoint[i].Y;
            }
            CalculateCenterAngleMethod(Y, X, rotateAngle, out centerRow, out centerCol, out error);
        }
        public void CalculateCenter(List<userWcsPoint> wcsPoint, double rotateAngle, out double centerRow, out double centerCol, out double error)
        {
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            double[] X = new double[wcsPoint.Count];
            double[] Y = new double[wcsPoint.Count];
            centerRow = 0;
            centerCol = 0;
            for (int i = 0; i < wcsPoint.Count; i++)
            {
                X[i] = wcsPoint[i].X;
                Y[i] = wcsPoint[i].Y;
            }
            CalculateCenterAngleMethod(Y, X, rotateAngle, out centerRow, out centerCol, out error);
        }
        public void CalculateCenter(Dictionary<string, userWcsPoint> wcsPoint, double rotateAngle, out double centerRow, out double centerCol, out double error)
        {
            if (wcsPoint == null)
                throw new ArgumentNullException(nameof(wcsPoint));
            List<double> X = new List<double>();
            List<double> Y = new List<double>();
            centerRow = 0;
            centerCol = 0;
            error = 0;
            foreach (var item in wcsPoint)
            {
                X.Add(item.Value.X);
                Y.Add(item.Value.Y);
            }
            CalculateCenter(Y.ToArray(), X.ToArray(), rotateAngle, out centerRow, out centerCol, out error);
            /// 计算角度的最大误差
            List<double> listError = new List<double>();
            for (int i = 0; i < X.Count - 1; i++)
            {
                double angle = HMisc.AngleLl(centerRow, centerCol, Y[i], X[i], centerRow, centerCol, Y[i + 1], X[i + 1]);
                listError.Add(Math.Abs(rotateAngle - angle));
            }
            error = listError.Max();
        }
        //public void CalculateCenter(BindingList<userWcsPoint> wcsPoint, double rotateAngle, out double centerRow, out double centerCol, out double error)
        //{
        //    if (wcsPoint == null)
        //        throw new ArgumentNullException(nameof(wcsPoint));
        //    List<double> X = new List<double>();
        //    List<double> Y = new List<double>();
        //    centerRow = 0;
        //    centerCol = 0;
        //    error = 0;
        //    foreach (var item in wcsPoint)
        //    {
        //        X.Add(item.X - item.Grab_x);
        //        Y.Add(item.Y - item.Grab_y);
        //    }
        //    CalculateCenterAngleMethod(Y.ToArray(), X.ToArray(), rotateAngle, out centerRow, out centerCol,out error);
        //    /// 计算角度的最大误差
        //    //List<double> listError = new List<double>();
        //    //for (int i = 0; i < X.Count - 1; i++)
        //    //{
        //    //    double angle = HMisc.AngleLl(centerRow, centerCol, Y[i], X[i], centerRow, centerCol, Y[i + 1], X[i + 1]);
        //    //    listError.Add(Math.Abs(rotateAngle - angle));
        //    //}
        //    //error = listError.Max();
        //}
        public void CalculateCenter(Dictionary<string, userPixPoint> pixPoint, double angleStep, out double centerRow, out double centerCol, out double error)
        {
            if (pixPoint == null)
                throw new ArgumentNullException(nameof(pixPoint));
            List<double> col = new List<double>();
            List<double> row = new List<double>();
            centerRow = 0;
            centerCol = 0;
            error = 0;
            foreach (var item in pixPoint)
            {
                col.Add(item.Value.Col);
                row.Add(item.Value.Row);
            }
            CalculateCenterAngleMethod(row.ToArray(), col.ToArray(), angleStep, out centerRow, out centerCol, out error);
            /// 计算角度的最大误差
            //List<double> listError = new List<double>();
            //for (int i = 0; i < col.Count - 1; i++)
            //{
            //    double angle = HMisc.AngleLl(centerRow, centerCol, row[i], col[i], centerRow, centerCol, row[i + 1], col[i + 1]);
            //    listError.Add(Math.Abs(angleStep - angle));
            //}
            //error = listError.Max();
        }
        public void CalculateCenter(BindingList<userPixPoint> pixPoint, double angleStep, out double centerRow, out double centerCol, out double error)
        {
            if (pixPoint == null)
                throw new ArgumentNullException(nameof(pixPoint));
            List<double> col = new List<double>();
            List<double> row = new List<double>();
            centerRow = 0;
            centerCol = 0;
            error = 0;
            foreach (var item in pixPoint)
            {
                col.Add(item.Col);
                row.Add(item.Row);
            }
            CalculateCenterAngleMethod(row.ToArray(), col.ToArray(), angleStep, out centerRow, out centerCol, out error);
            /// 计算角度的最大误差
            //List<double> listError = new List<double>();
            //for (int i = 0; i < col.Count - 1; i++)
            //{
            //    double angle = HMisc.AngleLl(centerRow, centerCol, row[i], col[i], centerRow, centerCol, row[i + 1], col[i + 1]);
            //    listError.Add(Math.Abs(angleStep - angle));
            //}
            //error = listError.Max();
        }
        public void CalculateCenter(double[] row, double[] col, double rotateAngle, out double centerRow, out double centerCol, out double error)
        {
            CalculateCenterAngleMethod(row, col, rotateAngle, out centerRow, out centerCol, out error);
            /// 计算角度的最大误差
            //List<double> listError = new List<double>();
            //for (int i = 0; i < row.Length - 1; i++)
            //{
            //    double angle = HMisc.AngleLl(centerRow, centerCol, row[i], col[i], centerRow, centerCol, row[i + 1], col[i + 1]);
            //    listError.Add(Math.Abs(rotateAngle - angle));
            //}
            //listError.Clear();

            //error = listError.Max();
        }
        public void CalculateCenterWcsXzPlane(double[] x, double[] y, double[] rotateAngle, out double center_x, out double center_z, out double error)
        {
            int index = (int)((rotateAngle.Length - 1) * 0.5);
            double stepAngle = 0, dist_x = 0;
            double[] z = new double[rotateAngle.Length];
            double[] new_x = new double[rotateAngle.Length];
            for (int i = 0; i < rotateAngle.Length; i++)
            {
                stepAngle = Math.Abs(rotateAngle[i] - rotateAngle[index]);
                dist_x = Math.Abs(x[i] - x[index]);
                new_x[i] = x[i] - x[index];
                z[i] = Math.Tan(stepAngle * 0.5 * Math.PI / 180) * dist_x * -1;
            }
            CalculateCenterAngleMethod(new_x, z, Math.Abs(rotateAngle[0] - rotateAngle[1]), out center_x, out center_z, out error);
            /// 计算角度的最大误差
            //List<double> listError = new List<double>();
            //for (int i = 0; i < x.Length - 1; i++)
            //{
            //    double angle = HMisc.AngleLl(center_x, center_z, x[i], y[i], center_x, center_z, x[i + 1], y[i + 1]);
            //    listError.Add(Math.Abs(Math.Abs(rotateAngle[0] - rotateAngle[1]) - angle));
            //}
            //error = listError.Max();
        }
        public void PixPointToHtuple(Dictionary<string, userPixPoint> pixPoint, out HTuple Row, out HTuple Col)
        {
            Row = new HTuple();
            Col = new HTuple();
            if (pixPoint == null)
                throw new ArgumentNullException("pixPoint");
            foreach (KeyValuePair<string, userPixPoint> item in pixPoint)
            {
                Row.Append(item.Value.Row);
                Col.Append(item.Value.Col);
            }
        }
        public void PixPointToHtuple(BindingList<userPixPoint> pixPoint, out HTuple Row, out HTuple Col)
        {
            Row = new HTuple();
            Col = new HTuple();
            if (pixPoint == null)
                throw new ArgumentNullException("pixPoint");
            foreach (var item in pixPoint)
            {
                Row.Append(item.Row);
                Col.Append(item.Col);
            }
        }
        public void WcsPointToHtuple(BindingList<CoordSysAxisParam> wcsPoint, out double[] x, out double[] y)
        {
            x = new double[0];
            y = new double[0];
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            x = new double[wcsPoint.Count];
            y = new double[wcsPoint.Count];
            for (int i = 0; i < wcsPoint.Count; i++)
            {
                x[i] = (wcsPoint[i].X);
                y[i] = (wcsPoint[i].Y);
            }
        }
        public void WcsPointToHtuple(BindingList<CoordSysAxisParam> wcsPoint, out HTuple x, out HTuple y)
        {
            x = new HTuple();
            y = new HTuple();
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            for (int i = 0; i < wcsPoint.Count; i++)
            {
                x.Append(wcsPoint[i].X);
                y.Append(wcsPoint[i].Y);
            }
        }
        public void WcsPointToHtuple(BindingList<CoordSysAxisParam> wcsPoint, enCalibPlane calibPlane, out HTuple x, out HTuple y, out HTuple z)
        {
            x = new HTuple();
            y = new HTuple();
            z = new HTuple();
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            switch (calibPlane)
            {
                case enCalibPlane.XY:
                    for (int i = 0; i < wcsPoint.Count; i++)
                    {
                        x.Append(wcsPoint[i].X);
                        y.Append(wcsPoint[i].Y);
                        z.Append(0);
                    }
                    break;
                case enCalibPlane.XZ:
                    for (int i = 0; i < wcsPoint.Count; i++)
                    {
                        x.Append(wcsPoint[i].X);
                        y.Append(0);
                        z.Append(wcsPoint[i].Z);
                    }
                    break;
                case enCalibPlane.YZ:
                    for (int i = 0; i < wcsPoint.Count; i++)
                    {
                        x.Append(0);
                        y.Append(wcsPoint[i].Y);
                        z.Append(wcsPoint[i].Z);
                    }
                    break;
            }
        }
        public void GetNPointCoord(userWcsVector startPoint, userWcsVector endPoint, int rowCount, int colCount, out BindingList<CoordSysAxisParam> wcsPoint)
        {
            wcsPoint = new BindingList<CoordSysAxisParam>();
            double step_x = (endPoint.X - startPoint.X) / (colCount - 1);
            double step_y = (endPoint.Y - startPoint.Y) / (rowCount - 1);
            double step_z = (endPoint.Z - startPoint.Z) / (rowCount - 1);
            double center_x = 0;// (endPoint.X + startPoint.X) * 0.5;
            double center_y = 0;// (endPoint.Y + startPoint.Y) * 0.5;
            double center_z = 0;// (endPoint.Y + startPoint.Y) * 0.5;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    wcsPoint.Add(new CoordSysAxisParam(
                                     startPoint.X - center_x + j * step_x,
                                     startPoint.Y - center_y + i * step_y,
                                     startPoint.Z - center_z + i * step_z,
                                     startPoint.Angle));
                }
            }
        }
        public void GetNPointCoord(userWcsVector startPoint, userWcsVector endPoint, int rowCount, int colCount,enCalibPlane calibPlane, out BindingList<CoordSysAxisParam> wcsPoint)
        {
            wcsPoint = new BindingList<CoordSysAxisParam>();
            double step_x = (endPoint.X - startPoint.X) / (colCount - 1);
            double step_y = (endPoint.Y - startPoint.Y) / (rowCount - 1);
            double step_z = (endPoint.Z - startPoint.Z) / (rowCount - 1);
            double center_x = 0;// (endPoint.X + startPoint.X) * 0.5;
            double center_y = 0;// (endPoint.Y + startPoint.Y) * 0.5;
            double center_z = 0;// (endPoint.Y + startPoint.Y) * 0.5;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    switch(calibPlane)
                    {
                        case enCalibPlane.XY:
                        case enCalibPlane.XZ:
                            wcsPoint.Add(new CoordSysAxisParam(
                            startPoint.X - center_x + j * step_x,
                            startPoint.Y - center_y + i * step_y,
                            startPoint.Z - center_z + i * step_z,
                            startPoint.Angle));
                            break;
                        case enCalibPlane.YZ:
                            wcsPoint.Add(new CoordSysAxisParam(
                            startPoint.X - center_x + i * step_x,
                            startPoint.Y - center_y + j * step_y,
                            startPoint.Z - center_z + i * step_z,
                            startPoint.Angle));
                            break;
                    }
                }
            }
        }
        public void GetNPointCoord(BindingList<CoordSysAxisParam> listPoint, out BindingList<CoordSysAxisParam> wcsPoint)
        {
            wcsPoint = new BindingList<CoordSysAxisParam>();
            if (listPoint == null) throw new ArgumentNullException("listPoint");
            if (listPoint.Count == 0) return;

            double center_x = (listPoint[0].X + listPoint.Last().X) * 0.5;
            double center_y = (listPoint[0].Y + listPoint.Last().Y) * 0.5;
            for (int i = 0; i < listPoint.Count; i++)
            {
                wcsPoint.Add(new CoordSysAxisParam(
                                 (listPoint[i].X - center_x) * -1,
                                 (listPoint[i].Y - center_y) * -1,
                                 0,
                                 0));
            }
        }
        public void GetNPointCoord2(userWcsVector startPoint, userWcsVector endPoint, int rowCount, int colCount, out BindingList<CoordSysAxisParam> wcsPoint)
        {
            wcsPoint = new BindingList<CoordSysAxisParam>();
            double step_x = (endPoint.X - startPoint.X) / (colCount - 1);
            double step_y = (endPoint.Y - startPoint.Y) / (rowCount - 1);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    wcsPoint.Add(new CoordSysAxisParam(
                        startPoint.X + j * step_x,
                        startPoint.Y + i * step_y,
                        startPoint.Z,
                        startPoint.Angle,
                        0,
                        0));
                }
            }
        }

        /// <summary>
        /// 先计算旋转中心，再标定9点
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="rad1">弧度</param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="rad2">弧度</param>
        /// <param name="centerRow"></param>
        /// <param name="centerCol"></param>
        public void CalculateCenterHomMatPix(double row1, double col1, double rad1, double row2, double col2, double rad2, out double centerRow, out double centerCol)
        {
            centerRow = 0;
            centerCol = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(row1, col1, rad1, row2, col2, rad2);
            UserHomMat2D HomMat = new UserHomMat2D(hHomMat2D);
            double x = 0, y = 0, x2 = 0, y2 = 0;
            //x* (1 - HomMat.c00) = (HomMat.c01 * y + HomMat.c02);
            //y = (HomMat.c10 * x + HomMat.c12)/(1- HomMat.c11);
            //x * (1 - HomMat.c00) = (HomMat.c01 * (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11) + HomMat.c02);
            x = ((HomMat.c01 * HomMat.c12) + HomMat.c02 * (1 - HomMat.c11)) / ((1 - HomMat.c00) * (1 - HomMat.c11) - HomMat.c01 * HomMat.c10);
            y = (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11);
            //x2 = ((hHomMat2D[1] * hHomMat2D[5]) + hHomMat2D[2] * (1 - hHomMat2D[4])) / ((1 - hHomMat2D[0]) * (1 - hHomMat2D[4]) - hHomMat2D[1] * hHomMat2D[3]);
            //y2 = (hHomMat2D[3] * x + hHomMat2D[5]) / (1 - hHomMat2D[4]);
            centerRow = x;
            centerCol = y;
        }

        public void CalculateCenterHomMatPix(double[] row1, double[] col1, double[] row2, double[] col2, out double centerRow, out double centerCol, out double rotateRad)
        {
            centerRow = 0;
            centerCol = 0;
            rotateRad = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(row1, col1, row2, col2);
            UserHomMat2D HomMat = new UserHomMat2D(hHomMat2D);
            double x = 0, y = 0;
            //x* (1 - HomMat.c00) = (HomMat.c01 * y + HomMat.c02);
            //y = (HomMat.c10 * x + HomMat.c12)/(1- HomMat.c11);
            //x * (1 - HomMat.c00) = (HomMat.c01 * (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11) + HomMat.c02);
            x = ((HomMat.c01 * HomMat.c12) + HomMat.c02 * (1 - HomMat.c11)) / ((1 - HomMat.c00) * (1 - HomMat.c11) - HomMat.c01 * HomMat.c10);
            y = (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11);
            //x2 = ((hHomMat2D[1] * hHomMat2D[5]) + hHomMat2D[2] * (1 - hHomMat2D[4])) / ((1 - hHomMat2D[0]) * (1 - hHomMat2D[4]) - hHomMat2D[1] * hHomMat2D[3]);
            //y2 = (hHomMat2D[3] * x + hHomMat2D[5]) / (1 - hHomMat2D[4]);
            double sy, phi, theta, tx, ty;
            hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            rotateRad = phi;
            centerRow = x;
            centerCol = y;
        }
        public void CalculateCenterHomMatPix(double[] rows, double[] cols, double radStep, out double centerRow, out double centerCol, out double error)
        {
            centerRow = 0;
            centerCol = 0;
            error = 0;
            if (rows == null) throw new ArgumentNullException("rows");
            if (cols == null) throw new ArgumentNullException("cols");
            if (cols.Length != rows.Length) throw new ArgumentNullException("rows与cols长度不相等!");
            /////////////////////////////////////////////////
            double row, col;
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> list_error = new List<double>();
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate;
            double Qx, Qy;
            for (int i = 0; i < rows.Length - 1; i++)
            {
                this.CalculateCenterHomMatPix(rows[i], cols[i], 0, rows[i + 1], cols[i + 1], radStep, out row, out col);
                hHomMat2DRotate = hHomMat2D.HomMat2dRotate(radStep, row, col);
                Qx = hHomMat2DRotate.AffineTransPoint2d(rows[i], cols[i], out Qy);
                double dist = HMisc.DistancePp(Qx, Qy, rows[i + 1], cols[i + 1]);
                if (dist < 10)      // 如果原点经变换后与目标点不重合，说明机构的旋转角度有问题，如果一致，说明标定没问题 ，10：表示 10_Pix
                {
                    listRow.Add(row);
                    listCol.Add(col);
                    list_error.Add(dist);
                }
            }
            list_error.Clear();
            double mean_x = listRow.Average();
            double mean_y = listCol.Average();
            for (int i = 0; i < listCol.Count; i++)
            {
                list_error.Add(HMisc.DistancePp(listRow[i], listCol[i], mean_x, mean_y));
            }
            error = list_error.Average(); // 圆心的平均误
            centerRow = listRow.Average();
            centerCol = listCol.Average();
        }

        public void CalculateCenterHomMatPixNpoint(double[] rows, double[] cols, double rotateCount, out double centerRow, out double centerCol, out double error, out double rotateAngle)
        {
            centerRow = 0;
            centerCol = 0;
            error = 0;
            rotateAngle = 0;
            if (rows == null) throw new ArgumentNullException("rows");
            if (cols == null) throw new ArgumentNullException("cols");
            if (cols.Length != rows.Length) throw new ArgumentNullException("rows与cols长度不相等!");
            /////////////////////////////////////////////////
            double row, col, radStep;
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> list_error = new List<double>();
            List<double> list_angle = new List<double>();
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate;
            HTuple Qx, Qy;
            double[] temp_row = new double[(int)(rows.Length / rotateCount)];
            double[] temp_col = new double[(int)(rows.Length / rotateCount)];
            double[] temp_row2 = new double[(int)(rows.Length / rotateCount)];
            double[] temp_col2 = new double[(int)(rows.Length / rotateCount)];
            if ((rows.Length / rotateCount) >= 3)
            {
                for (int i = 0; i < rotateCount - 1; i++)
                {
                    for (int j = 0; j < temp_row.Length; j++)
                    {
                        temp_row[j] = rows[temp_row.Length * i + j];
                        temp_col[j] = cols[temp_row.Length * i + j];
                    }
                    for (int j = 0; j < temp_row.Length; j++)
                    {
                        temp_row2[j] = rows[temp_row.Length * (i + 1) + j];
                        temp_col2[j] = cols[temp_row.Length * (i + 1) + j];
                    }
                    //////////////////////////////////////////////////
                    this.CalculateCenterHomMatPix(temp_row, temp_col, temp_row2, temp_col2, out row, out col, out radStep);
                    hHomMat2DRotate = hHomMat2D.HomMat2dRotate(radStep, row, col);
                    Qx = hHomMat2DRotate.AffineTransPoint2d(temp_row, temp_col, out Qy);
                    HTuple dist = HMisc.DistancePp(Qx, Qy, temp_row2, temp_col2);
                    //if (dist < 10)      // 如果原点经变换后与目标点不重合，说明机构的旋转角度有问题，如果一致，说明标定没问题 ，10：表示 10_Pix
                    //{
                    listRow.Add(row);
                    listCol.Add(col);
                    list_error.Add(dist.TupleMean().D);
                    list_angle.Add(radStep * 180 / Math.PI);
                    //}
                }
                error = list_error.Average();
                centerRow = listRow.Average();
                centerCol = listCol.Average();
                rotateAngle = list_angle.Average();
            }
            else
            {
                MessageBox.Show("使用N点矩阵变换至少三个以上点对应，请确保每执行一次有三个点生成");
            }
        }


        /// <summary>
        /// 先标定9点再计算旋转中心
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="angle1">单位为角度</param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="angle2">单位为角度</param>
        /// <param name="center_x">中心 X </param>
        /// <param name="center_y">中心 y </param>
        public void CalculateCenterHomMatWcs(double x1, double y1, double angle1, double x2, double y2, double angle2, out double center_x, out double center_y)
        {
            center_x = 0;
            center_y = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(x1, y1, angle1 * Math.PI / 180, x2, y2, angle2 * Math.PI / 180);
            UserHomMat2D HomMat = new UserHomMat2D(hHomMat2D);
            double x = 0, y = 0;
            //x* (1 - HomMat.c00) = (HomMat.c01 * y + HomMat.c02);
            //y = (HomMat.c10 * x + HomMat.c12)/(1- HomMat.c11);
            //x * (1 - HomMat.c00) = (HomMat.c01 * (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11) + HomMat.c02);
            x = ((HomMat.c01 * HomMat.c12) + HomMat.c02 * (1 - HomMat.c11)) / ((1 - HomMat.c00) * (1 - HomMat.c11) - HomMat.c01 * HomMat.c10);
            y = (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11);
            //x = ((hHomMat2D[1] * hHomMat2D[5]) + hHomMat2D[2] * (1 - hHomMat2D[4])) / ((1 - hHomMat2D[0]) * (1 - hHomMat2D[4]) - hHomMat2D[1] * hHomMat2D[3]);
            //y = (hHomMat2D[3] * x + hHomMat2D[5]) / (1 - hHomMat2D[4]);
            center_x = x;
            center_y = y;
        }

        public void CalculateCenterHomMatWcs(double[] x1, double[] y1, double[] x2, double[] y2, out double center_x, out double center_y, out double rotateRad)
        {
            center_x = 0;
            center_y = 0;
            rotateRad = 0;
            double sy, phi, theta, tx, ty;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(x1, y1, x2, y2);
            UserHomMat2D HomMat = new UserHomMat2D(hHomMat2D);
            hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            double x = 0, y = 0;
            //x* (1 - HomMat=.c00) = (HomMat.c01 * y + HomMat.c02);
            //y = (HomMat.c10 * x + HomMat.c12)/(1- HomMat.c11);
            //x * (1 - HomMat.c00) = (HomMat.c01 * (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11) + HomMat.c02);
            x = ((HomMat.c01 * HomMat.c12) + HomMat.c02 * (1 - HomMat.c11)) / ((1 - HomMat.c00) * (1 - HomMat.c11) - HomMat.c01 * HomMat.c10);
            y = (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11);
            //x = ((hHomMat2D[1] * hHomMat2D[5]) + hHomMat2D[2] * (1 - hHomMat2D[4])) / ((1 - hHomMat2D[0]) * (1 - hHomMat2D[4]) - hHomMat2D[1] * hHomMat2D[3]);
            //y = (hHomMat2D[3] * x + hHomMat2D[5]) / (1 - hHomMat2D[4]);
            center_x = x;
            center_y = y;
            rotateRad = phi;

            double deg = phi * 180 / Math.PI;
        }

        public void CalculateCenterHomMatWcsXzPlane(double[] x, double[] y, double[] angle, out double center_x, out double center_z, out double error)
        {
            center_x = 0;
            center_z = 0;
            error = 0;
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            if (y.Length != x.Length) throw new ArgumentNullException("x 与 y 长度不相等!");
            /////////////////////////////////////////////////
            double cen_x, cen_y;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_error = new List<double>();
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate;
            double Qx, Qy;
            int index = (int)((angle.Length - 1) * 0.5);
            double stepAngle = 0, dist_x = 0;
            double[] z = new double[angle.Length];
            double[] new_x = new double[angle.Length];
            for (int i = 0; i < angle.Length; i++)
            {
                stepAngle = Math.Abs(angle[i] - angle[index]);
                dist_x = Math.Abs(x[i] - x[index]);
                new_x[i] = x[i] - x[index];
                z[i] = Math.Tan(stepAngle * 0.5 * Math.PI / 180) * dist_x * -1;
            }
            ////////////////////
            for (int i = 0; i < new_x.Length - 1; i++)
            {
                this.CalculateCenterHomMatWcs(new_x[i], z[i], 0, new_x[i + 1], z[i + 1], Math.Abs(angle[0] - angle[1]), out cen_x, out cen_y);
                hHomMat2DRotate = hHomMat2D.HomMat2dRotate(Math.Abs(angle[0] - angle[1]) * Math.PI / 180, cen_x, cen_y);
                Qx = hHomMat2DRotate.AffineTransPoint2d(new_x[i], z[i], out Qy);
                double dist = HMisc.DistancePp(Qx, Qy, new_x[i + 1], z[i + 1]);
                if (dist < 0.1) // 如果原点经变换后与目标点不重合，说明机构的旋转角度有问题，如果一致，说明标定没问题
                {
                    list_x.Add(cen_x);
                    list_y.Add(cen_y);
                    list_error.Add(dist);
                }
            }
            error = list_error.Average();
            center_x = list_x.Average();
            center_z = list_y.Average();
        }
        public void CalculateCenterHomMatWcs(double[] x, double[] y, double angleStep, out double center_x, out double center_y, out double error)
        {
            center_x = 0;
            center_y = 0;
            error = 0;
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            if (y.Length != x.Length) throw new ArgumentNullException("x 与 y 长度不相等!");
            /////////////////////////////////////////////////
            double cen_x, cen_y;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_error = new List<double>();
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate;
            double Qx, Qy;
            for (int i = 0; i < x.Length - 1; i++)
            {
                this.CalculateCenterHomMatWcs(x[i], y[i], 0, x[i + 1], y[i + 1], angleStep, out cen_x, out cen_y);
                hHomMat2DRotate = hHomMat2D.HomMat2dRotate(angleStep * Math.PI / 180, cen_x, cen_y);
                Qx = hHomMat2DRotate.AffineTransPoint2d(x[i], y[i], out Qy);
                double dist = HMisc.DistancePp(Qx, Qy, x[i + 1], y[i + 1]);
                if (dist < 0.1) // 如果原点经变换后与目标点不重合，说明机构的旋转角度有问题，如果一致，说明标定没问题
                {
                    list_x.Add(cen_x);
                    list_y.Add(cen_y);
                    list_error.Add(dist);
                }
            }
            list_error.Clear();
            double mean_x = list_x.Average();
            double mean_y = list_y.Average();
            for (int i = 0; i < list_x.Count; i++)
            {
                list_error.Add(HMisc.DistancePp(list_x[i], list_y[i], mean_x, mean_y));
            }
            error = list_error.Average(); // 圆心的平均误
            center_x = list_x.Average();
            center_y = list_y.Average();
        }

        public void CalculateCenterHomMatWcsNpoint(double[] x, double[] y, double rotateCount, out double center_x, out double center_y, out double error, out double rotateAngle)
        {
            center_x = 0;
            center_y = 0;
            error = 0;
            rotateAngle = 0;
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            if (y.Length != x.Length) throw new ArgumentNullException("x 与 y 长度不相等!");
            /////////////////////////////////////////////////
            double cen_x, cen_y, radStep;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_error = new List<double>();
            List<double> list_angle = new List<double>();
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate;
            HTuple Qx1, Qy1;
            double[] Px = new double[(int)(x.Length / rotateCount)];
            double[] Py = new double[(int)(x.Length / rotateCount)];
            double[] Qx = new double[(int)(x.Length / rotateCount)];
            double[] Qy = new double[(int)(x.Length / rotateCount)];
            if ((x.Length / rotateCount) > 2)
            {
                for (int i = 0; i < rotateCount - 1; i++)
                {
                    for (int j = 0; j < Px.Length; j++)
                    {
                        Px[j] = x[Px.Length * i + j];
                        Py[j] = y[Px.Length * i + j];
                    }
                    for (int j = 0; j < Px.Length; j++)
                    {
                        Qx[j] = x[Px.Length * (i + 1) + j];
                        Qy[j] = y[Px.Length * (i + 1) + j];
                    }
                    this.CalculateCenterHomMatWcs(Px, Py, Qx, Qy, out cen_x, out cen_y, out radStep);
                    hHomMat2DRotate = hHomMat2D.HomMat2dRotate(radStep, cen_x, cen_y);
                    Qx1 = hHomMat2DRotate.AffineTransPoint2d(Px, Py, out Qy1);
                    HTuple dist = HMisc.DistancePp(Qx1, Qy1, Qx, Qy);
                    //if (dist < 0.1) // 如果原点经变换后与目标点不重合，说明机构的旋转角度有问题，如果一致，说明标定没问题
                    //{
                    list_x.Add(cen_x);
                    list_y.Add(cen_y);
                    list_error.Add(dist.TupleMean().D);
                    list_angle.Add(radStep * 180 / Math.PI);
                    //}
                }
                list_error.Clear();
                double mean_x = list_x.Average();
                double mean_y = list_y.Average();
                for (int i = 0; i < list_x.Count; i++)
                {
                    list_error.Add(HMisc.DistancePp(list_x[i], list_y[i], mean_x, mean_y));
                }
                error = list_error.Average(); // 圆心的平均误
                center_x = list_x.Average();
                center_y = list_y.Average();
                rotateAngle = list_angle.Average();
            }
            else
            {
                MessageBox.Show("使用N点矩阵变换至少三个以上点对应，请确保每执行一次有三个点生成");
            }
        }
        public void CalculateCenterHomMatWcs(double[] x, double[] y, double[] angle, out double center_x, out double center_y, out double error)
        {
            center_x = 0;
            center_y = 0;
            error = 0;
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            if (y.Length != x.Length) throw new ArgumentNullException("x 与 y 长度不相等!");
            /////////////////////////////////////////////////
            double cen_x, cen_y;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_error = new List<double>();
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate;
            double Qx, Qy;
            for (int i = 0; i < x.Length - 1; i++)
            {
                this.CalculateCenterHomMatWcs(x[i], y[i], 0, x[i + 1], y[i + 1], angle[i + 1] - angle[i], out cen_x, out cen_y);
                hHomMat2DRotate = hHomMat2D.HomMat2dRotate((angle[i + 1] - angle[i]) * Math.PI / 180, cen_x, cen_y);
                Qx = hHomMat2DRotate.AffineTransPoint2d(x[i], y[i], out Qy);
                double dist = HMisc.DistancePp(Qx, Qy, x[i + 1], y[i + 1]);
                if (dist < 0.1) // 如果原点经变换后与目标点不重合，说明机构的旋转角度有问题，如果一致，说明标定没问题
                {
                    list_x.Add(cen_x);
                    list_y.Add(cen_y);
                    list_error.Add(dist);
                }
            }
            error = list_error.Average();
            center_x = list_x.Average();
            center_y = list_y.Average();
        }

        #region  机台校准

        public UserHomMat2D CalibMachineFace(Dictionary<string, userWcsPoint> wcsPoint, int rowCount, int colCount, double offset_x, double offset_y,
            out double error, out Dictionary<double, XyValuePairs> Calibrate_X, out Dictionary<double, XyValuePairs> Calibrate_Y)
        {
            error = 0;
            Calibrate_X = new Dictionary<double, XyValuePairs>();
            Calibrate_Y = new Dictionary<double, XyValuePairs>();
            UserHomMat2D homMat2D = new UserHomMat2D();
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_std_x = new List<double>();
            List<double> list_std_y = new List<double>();
            double refCam_x = 0, refCam_y = 0, refMachine_x = 0, refMachine_y = 0;
            ////// 调整机台坐标点，让每个Mark都在相机坐标系的同一个位置
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            int index = 0;
            foreach (var item in wcsPoint)
            {
                if (index == 0)
                {
                    list_x.Add(item.Value.Grab_x);
                    list_y.Add(item.Value.Grab_y);
                    refCam_x = item.Value.X - item.Value.Grab_x;
                    refCam_y = item.Value.Y - item.Value.Grab_y;
                    refMachine_x = item.Value.Grab_x;
                    refMachine_y = item.Value.Grab_y;
                }
                else
                {
                    list_x.Add(item.Value.Grab_x * 2 + refCam_x - item.Value.X); // 后面的每个机以坐标都要加上一个Mark点在相机坐标系下的偏差值
                    list_y.Add(item.Value.Grab_y * 2 + refCam_y + item.Value.Y);
                }
                index++;
            }
            ////// 生成理论坐标值
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    list_std_x.Add(j * offset_x + refMachine_x);
                    list_std_y.Add(i * offset_y + refMachine_y);
                }
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(list_x.ToArray(), list_y.ToArray(), list_std_x.ToArray(), list_std_y.ToArray());
            homMat2D = new UserHomMat2D(hHomMat2D);
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(list_x.ToArray(), list_y.ToArray(), out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, list_std_x.ToArray(), list_std_y.ToArray());
            error = dist.TupleMax();

            ////// 生成坐标值对用于线性插值校准 //////////////////////////
            ///// 生成X序列
            List<double> tempCur_x = new List<double>();
            List<double> tempStd_x = new List<double>();
            List<double> tempCur_y = new List<double>();
            List<double> tempStd_y = new List<double>();
            for (int i = 0; i < rowCount; i++)
            {
                tempCur_x.Clear();
                tempStd_x.Clear();
                for (int j = 0; j < colCount; j++)
                {
                    tempCur_x.Add(list_x[i * colCount + j]);
                    tempStd_x.Add(j * offset_x + refMachine_x);
                }
                Calibrate_Y.Add(list_y[i * colCount], new XyValuePairs(tempCur_x.ToArray(), tempStd_x.ToArray()));
            }
            ///// 生成Y序列
            for (int i = 0; i < colCount; i++)
            {
                tempCur_y.Clear();
                tempStd_y.Clear();
                for (int j = 0; j < rowCount; j++)
                {
                    tempCur_y.Add(list_y[j * colCount + i]);
                    tempStd_y.Add(j * offset_y + refMachine_y);
                }
                Calibrate_X.Add(list_x[i * colCount], new XyValuePairs(tempCur_y.ToArray(), tempStd_y.ToArray()));
            }
            /////////////////////////////////
            return homMat2D;
        }




        //
        #endregion


    }
}
