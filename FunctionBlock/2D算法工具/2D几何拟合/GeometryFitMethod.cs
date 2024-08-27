using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class GeometryFitMethod
    {
        private static object lockState = new object();
        private static GeometryFitMethod _Instance = null;
        private GeometryFitMethod()
        {

        }
        public static GeometryFitMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new GeometryFitMethod();
                    }
                }
                return _Instance;
            }
        }

        public bool FitCircle(userWcsCircleSector[] circleSector, CircleFitParam param, out userWcsCircle _Circle)
        {
            bool result = false;
            _Circle = new userWcsCircle();
            HTuple Row = 0, Column = 0, Radius = 0, StartPhi = 0, EndPhi = 0, PointOrder = 0;
            if (circleSector == null)
                throw new ArgumentNullException("circleSector");
            if (circleSector.Length == 0) return result;
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            /////////////////////////////////
            for (int i = 0; i < circleSector.Length; i++)
            {
                if (circleSector[i].EdgesPoint_xyz == null) continue;
                foreach (var item in circleSector[i].EdgesPoint_xyz)
                {
                    listPoint.Add(item);
                    //X.Add(item.X);
                    //Y.Add(item.Y);
                    //Z.Add(item.Z);
                }
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.角度升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            //SortLinePoint(X.ToArray(), Y.ToArray(), out sort_x, out sort_y);
            new HXLDCont(sort_y, sort_x).FitCircleContourXld(param.Algorithm, (int)param.MaxNumPoints, param.MaxClosureDist, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            ///////////////////////////           
            _Circle = new userWcsCircle(Column.D, Row.D, 0, Radius.D, circleSector[0].CamParams);
            _Circle.ViewWindow = circleSector[0].ViewWindow;
            _Circle.CamName = circleSector[0].CamName;
            _Circle.Grab_x = circleSector[0].Grab_x;
            _Circle.Grab_y = circleSector[0].Grab_y;
            _Circle.Grab_theta = circleSector[0].Grab_theta;
            result = true;
            return result;
        }
        public bool FitEllipse(userWcsEllipseSector[] ellipseSector, EllipseFitParam param, out userWcsEllipse ellipse)
        {
            bool result = false;
            ellipse = new userWcsEllipse();
            double x = 0, y = 0, z = 0, r1 = 0, r2 = 0, rad = 0, start_deg = 0, end_deg = 0;
            HTuple Row = 0, Column = 0, Phi = 0, Radius1 = 0, Radius2 = 0, StartPhi = 0, EndPhi = 0, PointOrder = 0;
            if (ellipseSector == null)
                throw new ArgumentNullException("ellipseSector");
            if (ellipseSector.Length == 0) return result;
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            ////////////////////////////////////
            for (int i = 0; i < ellipseSector.Length; i++)
            {
                if (ellipseSector[i].EdgesPoint_xyz == null) continue;
                foreach (var item in ellipseSector[i].EdgesPoint_xyz)
                {
                    listPoint.Add(item);
                    //X.Add(item.X);
                    //Y.Add(item.Y);
                    //Z.Add(item.Z);
                }
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.角度升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            ///////////////////////////
            //SortLinePoint(X.ToArray(), Y.ToArray(), out sort_x, out sort_y);
            new HXLDCont(sort_y, sort_x).FitEllipseContourXld(param.Algorithm, (int)param.MaxNumPoints, param.MaxClosureDist, (int)param.ClippingEndPoints, (int)param.VossTabSize, param.Iterations, param.ClippingFactor, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
            ellipse = new userWcsEllipse(Column.D, Row.D, 0, Phi.TupleDeg().D, Radius1.D, Radius2.D, ellipseSector[0].CamParams);
            ellipse.ViewWindow = ellipseSector[0].ViewWindow;
            ellipse.CamName = ellipseSector[0].CamName;
            ellipse.Grab_x = ellipseSector[0].Grab_x;
            ellipse.Grab_y = ellipseSector[0].Grab_y;
            ellipse.Grab_theta = ellipseSector[0].Grab_theta;
            result = true;
            return result;
        }
        public bool FitLine(userWcsLine[] line, LineFitParam param, out userWcsLine wcsLine)
        {
            wcsLine = new userWcsLine();
            bool result = false;
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            if (line.Length == 0)
            {
                throw new ArgumentNullException("line：对象中不包含元素");
            }
            HTuple RowBegin = 0, ColBegin = 0, RowEnd = 0, ColEnd = 0, Nr = 0, Nc = 0, Dist = 0;
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            ///////////////////////////////////////////////
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i].EdgesPoint_xyz == null) continue;
                foreach (var item in line[i].EdgesPoint_xyz)
                {
                    listPoint.Add(item);
                    //X.Add(item.X);
                    //Y.Add(item.Y);
                    //Z.Add(item.Z);
                }
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.距离升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            //SortLinePoint(X.ToArray(), Y.ToArray(), out sort_x, out sort_y);
            new HXLDCont(sort_y, sort_x).FitLineContourXld(param.Algorithm, (int)param.MaxNumPoints, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
            ///////////////////////////
            wcsLine = new userWcsLine(ColBegin.D, RowBegin.D, 0, ColEnd.D, RowEnd.D, 0, line[0].CamParams);
            wcsLine.ViewWindow = line[0].ViewWindow;
            wcsLine.CamName = line[0].CamName;
            wcsLine.Grab_x = line[0].Grab_x;
            wcsLine.Grab_y = line[0].Grab_y;
            wcsLine.Grab_theta = line[0].Grab_theta;
            result = true;
            return result;
        }
        public bool NPointFitLine(userWcsPoint[] point, LineFitParam param, out userWcsLine wcsLine)
        {
            bool result = false;
            wcsLine = new userWcsLine();
            HTuple RowBegin = 0, ColBegin = 0, RowEnd = 0, ColEnd = 0, Nr = 0, Nc = 0, Dist = 0;
            HXLDCont UnionCircle = new HXLDCont();
            HXLDCont ContCircle = new HXLDCont();
            if (point == null) throw new ArgumentNullException("point");
            if (point.Length == 0) throw new ArgumentException("point");
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            foreach (var item in point)
            {
                listPoint.Add(item);
                //X.Add(item.X);
                //Y.Add(item.Y);
                //Z.Add(item.Z);
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.距离升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            ///////////////////////////   
            new HXLDCont(sort_y, sort_x).FitLineContourXld(param.Algorithm, (int)param.MaxNumPoints, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
            wcsLine = new userWcsLine(ColBegin[0].D, RowBegin[0].D, 0, ColEnd[0].D, RowEnd[0].D, 0, point[0].CamParams);
            wcsLine.ViewWindow = point[0].ViewWindow;
            wcsLine.CamName = point[0].CamName;
            wcsLine.Grab_x = point[0].Grab_x;
            wcsLine.Grab_y = point[0].Grab_y;
            wcsLine.Grab_theta = point[0].Grab_theta;
            result = true;
            return result;
        }
        public bool NPointFitCircle(userWcsPoint[] point, CircleFitParam param, out userWcsCircle _Circle)
        {
            bool result = false;
            _Circle = new userWcsCircle();
            HTuple Row = 0, Column = 0, Radius = 0, StartPhi = 0, EndPhi = 0, PointOrder = 0;
            if (point == null)
                throw new ArgumentNullException("point");
            if (point.Length == 0) return result;
            //////////////////////////////
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            foreach (var item in point)
            {
                listPoint.Add(item);
                //X.Add(item.X);
                //Y.Add(item.Y);
                //Z.Add(item.Z);
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.角度升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            new HXLDCont(sort_y, sort_x).FitCircleContourXld(param.Algorithm, (int)param.MaxNumPoints, param.MaxClosureDist, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            ///////////////////////////           
            _Circle = new userWcsCircle(Column.D, Row.D, 0, Radius.D, point[0].CamParams);
            _Circle.ViewWindow = point[0].ViewWindow;
            _Circle.CamName = point[0].CamName;
            _Circle.Grab_x = point[0].Grab_x;
            _Circle.Grab_y = point[0].Grab_y;
            _Circle.Grab_theta = point[0].Grab_theta;
            result = true;
            return result;
        }
        public bool NPointFitEllipse(userWcsPoint[] point, EllipseFitParam param, out userWcsEllipse ellipse)
        {
            bool result = false;
            ellipse = new userWcsEllipse();
            HTuple Row, Column, Phi, Radius1, Radius2, StartPhi, EndPhi, PointOrder;
            if (point == null) throw new ArgumentNullException("point");
            if (point.Length == 0) throw new ArgumentException("point");
            /////////////////////////////////////
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            foreach (var item in point)
            {
                listPoint.Add(item);
                //X.Add(item.X);
                //Y.Add(item.Y);
                //Z.Add(item.Z);
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.角度升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            ///////////////////////////     
            new HXLDCont(sort_y, sort_x).FitEllipseContourXld(param.Algorithm, (int)param.MaxNumPoints, param.MaxClosureDist, (int)param.ClippingEndPoints, (int)param.VossTabSize, param.Iterations, param.ClippingFactor, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
            ellipse = new userWcsEllipse(Column[0].D, Row[0].D, 0, Phi[0].D, Radius1[0].D, Radius2[0].D, point[0].CamParams);
            ellipse.ViewWindow = point[0].ViewWindow;
            ellipse.CamName = point[0].CamName;
            ellipse.Grab_x = point[0].Grab_x;
            ellipse.Grab_y = point[0].Grab_y;
            ellipse.Grab_theta = point[0].Grab_theta;
            result = true;
            return result;
        }
        public bool NPointFitRect2(userWcsPoint[] point, Rect2FitParam param, out userWcsRectangle2 rect2)
        {
            bool result = false;
            rect2 = new userWcsRectangle2();
            HTuple Row = 0, Column = 0, Phi = 0, Length1 = 0, Length2 = 0, PointOrder = 0;
            if (point == null) throw new ArgumentNullException("point");
            if (point.Length == 0) throw new ArgumentException("point");
            /////////////////////////////////
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            foreach (var item in point)
            {
                listPoint.Add(item);
                //X.Add(item.X);
                //Y.Add(item.Y);
                //Z.Add(item.Z);
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.角度升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            ///////////////////////////    
            new HXLDCont(sort_y, sort_x).FitRectangle2ContourXld(param.Algorithm, (int)param.MaxNumPoints, (int)param.MaxClosureDist, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
            rect2 = new userWcsRectangle2(Column.D, Row.D, 0, Phi[0].D, Length1[0].D, Length2[0].D, point[0].CamParams);
            rect2.ViewWindow = point[0].ViewWindow;
            rect2.CamName = point[0].CamName;
            rect2.Grab_x = point[0].Grab_x;
            rect2.Grab_y = point[0].Grab_y;
            rect2.Grab_theta = point[0].Grab_theta;
            result = true;
            return result;
        }
        public bool FitRect2(userWcsLine[] line, Rect2FitParam param, out userWcsRectangle2 rect2)
        {
            bool result = false;
            rect2 = new userWcsRectangle2();
            HTuple Row = 0, Column = 0, Phi = 0, Length1 = 0, Length2 = 0, PointOrder = 0;
            if (line == null) throw new ArgumentNullException("line");
            if (line.Length == 0) throw new ArgumentException("line");
            //List<double> X = new List<double>();
            //List<double> Y = new List<double>();
            //List<double> Z = new List<double>();
            List<userWcsPoint> listPoint = new List<userWcsPoint>();
            /////////////////////////////////
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i].EdgesPoint_xyz == null) continue;
                foreach (var item in line[i].EdgesPoint_xyz)
                {
                    listPoint.Add(item);
                    //X.Add(item.X);
                    //Y.Add(item.Y);
                    //Z.Add(item.Z);
                }
            }
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(listPoint.ToArray(), enSortPoint.角度升序, out wcsPointSort);
            double[] sort_x = new double[wcsPointSort.Length];
            double[] sort_y = new double[wcsPointSort.Length];
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                sort_x[i] = wcsPointSort[i].X;
                sort_y[i] = wcsPointSort[i].Y;
            }
            //SortLinePoint(X.ToArray(), Y.ToArray(), out sort_x, out sort_y);
            new HXLDCont(sort_y, sort_x).FitRectangle2ContourXld(param.Algorithm, (int)param.MaxNumPoints, (int)param.MaxClosureDist, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
            rect2 = new userWcsRectangle2(Column.D, Row.D, 0, Phi[0].D, Length1[0].D, Length2[0].D, line[0].CamParams);
            rect2.CamName = line[0].CamName;
            rect2.ViewWindow = line[0].ViewWindow;
            rect2.Grab_x = line[0].Grab_x;
            rect2.Grab_y = line[0].Grab_y;
            rect2.Grab_theta = line[0].Grab_theta;
            result = true;
            return result;
        }


        /// <summary>
        /// 排序非直线的轮廓点
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="sort_x"></param>
        /// <param name="sort_y"></param>
        private void SortProfilePoint(double[] X, double[] Y, out double[] sort_x, out double[] sort_y)
        {
            sort_x = new double[0];
            sort_y = new double[0];
            if (X == null)
                throw new ArgumentNullException("X");
            if (Y == null)
                throw new ArgumentNullException("Y");
            /////////////////////////////////////////////////////////
            double[] phi = new double[X.Length];
            double mean_x = X.Average();
            double mean_y = Y.Average();
            /////////////////////////////////////
            Dictionary<double, double> dic_x = new Dictionary<double, double>();
            Dictionary<double, double> dic_y = new Dictionary<double, double>();
            for (int i = 0; i < X.Length; i++)
            {
                double rad = Math.Atan2(Y[i] - mean_y, X[i] - mean_x);
                if (rad < 0)
                    phi[i] = rad + Math.PI * 2;
                else
                    phi[i] = rad;
                dic_x.Add(phi[i], X[i]);
                dic_y.Add(phi[i], Y[i]);
            }
            /////////////////////////////////////////////
            sort_x = new double[X.Length];
            sort_y = new double[X.Length];
            Array.Sort(phi);
            for (int i = 0; i < phi.Length; i++)
            {
                sort_x[i] = dic_x[phi[i]];
                sort_y[i] = dic_y[phi[i]];
            }

        }

        /// <summary>
        /// 排序直线上的点
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="sort_x"></param>
        /// <param name="sort_y"></param>
        private void SortLinePoint(double[] X, double[] Y, out double[] sort_x, out double[] sort_y)
        {
            sort_x = new double[0];
            sort_y = new double[0];
            if (X == null)
                throw new ArgumentNullException("X");
            if (Y == null)
                throw new ArgumentNullException("Y");
            /////////////////////////////////////////////////////////
            double[] dist = new double[X.Length];
            /////////////////////////////////////
            Dictionary<double, double> dic_x = new Dictionary<double, double>();
            Dictionary<double, double> dic_y = new Dictionary<double, double>();
            for (int i = 0; i < X.Length; i++)
            {
                dist[i] = HMisc.DistancePp(X[i], Y[i], 0.0, 0.0);
                dic_x.Add(dist[i], X[i]);
                dic_y.Add(dist[i], Y[i]);
            }
            /////////////////////////////////////////////
            sort_x = new double[X.Length];
            sort_y = new double[X.Length];
            Array.Sort(dist);
            for (int i = 0; i < dist.Length; i++)
            {
                sort_x[i] = dic_x[dist[i]];
                sort_y[i] = dic_y[dist[i]];
            }



        }

        private void SortTrackPoint(userWcsPoint[] wcsPoints, enSortPoint sortMethod, out userWcsPoint[] wcsPointSort)
        {
            wcsPointSort = new userWcsPoint[0];
            if (wcsPoints == null)
                throw new ArgumentNullException("wcsPoints");
            /////////////////////////////////////////////////////////
            double[] dist1 = new double[wcsPoints.Length];
            double[] dist2 = new double[wcsPoints.Length];
            double[] phi = new double[wcsPoints.Length];
            double[] X = new double[wcsPoints.Length];
            double[] Y = new double[wcsPoints.Length];
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                X[i] = wcsPoints[i].X;
                Y[i] = wcsPoints[i].Y;
            }
            double mean_x = X.Average();
            double mean_y = Y.Average();
            /////////////////////////////////////
            Dictionary<double, userWcsPoint> dic = new Dictionary<double, userWcsPoint>();
            Dictionary<double, userWcsPoint> dic1 = new Dictionary<double, userWcsPoint>();
            Dictionary<double, userWcsPoint> dic2 = new Dictionary<double, userWcsPoint>();
            switch (sortMethod)
            {
                case enSortPoint.角度降序:
                case enSortPoint.角度升序:
                    for (int i = 0; i < wcsPoints.Length; i++)
                    {
                        double rad = Math.Atan2(wcsPoints[i].Y - mean_y, wcsPoints[i].X - mean_x);
                        if (rad < 0)
                            phi[i] = rad + Math.PI * 2;
                        else
                            phi[i] = rad;
                        dic.Add(phi[i], wcsPoints[i]);
                    }
                    break;
                case enSortPoint.距离升序: // 用于直线排序
                case enSortPoint.距离降序:
                    for (int i = 0; i < wcsPoints.Length; i++)
                    {
                        double rad = Math.Atan2(wcsPoints[i].Y - mean_y, wcsPoints[i].X - mean_x);
                        phi[i] = rad;
                        dic.Add(phi[i], wcsPoints[i]);
                    }
                    /// 将直线上的点从中间分成两段
                    List<userWcsPoint> listPoint1 = new List<userWcsPoint>();
                    List<userWcsPoint> listPoint2 = new List<userWcsPoint>();
                    for (int i = 0; i < phi.Length; i++)
                    {
                        if (Math.Abs(phi[i] - phi[0]) < 0.5)
                            listPoint1.Add(dic[phi[i]]);
                        else
                            listPoint2.Add(dic[phi[i]]);
                    }
                    // 计算线段1区到直线中点的距离
                    dist1 = new double[listPoint1.Count];
                    for (int i = 0; i < listPoint1.Count; i++)
                    {
                        dist1[i] = HMisc.DistancePp(listPoint1[i].X, listPoint1[i].Y, mean_x, mean_y);
                        dic1.Add(dist1[i], listPoint1[i]);
                    }
                    // 计算线段2区到直线中点的距离
                    dist2 = new double[listPoint2.Count];
                    for (int i = 0; i < listPoint2.Count; i++)
                    {
                        dist2[i] = HMisc.DistancePp(listPoint2[i].X, listPoint2[i].Y, mean_x, mean_y);
                        dic2.Add(dist2[i], listPoint2[i]);
                    }
                    break;
            }
            /////////////////////////////////////////////
            List<userWcsPoint> listSortPoint = new List<userWcsPoint>();
            //wcsPointSort = new userWcsPoint[dist1.Length + dist2.Length];
            switch (sortMethod)
            {
                case enSortPoint.NONE:
                    for (int i = 0; i < wcsPoints.Length; i++)
                    {
                        listSortPoint.Add(new userWcsPoint(wcsPoints[i].X, wcsPoints[i].Y, wcsPoints[i].Z, wcsPoints[i].CamParams));
                    }
                    break;
                case enSortPoint.角度降序:
                    Array.Sort(phi);
                    for (int i = phi.Length - 1; i > 0; i--)
                    {
                        listSortPoint.Add(new userWcsPoint(dic[phi[i]].X, dic[phi[i]].Y, dic[phi[i]].Z, dic[phi[i]].CamParams));
                    }
                    break;
                case enSortPoint.角度升序:
                    Array.Sort(phi);
                    for (int i = 0; i < phi.Length; i++)
                    {
                        listSortPoint.Add(new userWcsPoint(dic[phi[i]].X, dic[phi[i]].Y, dic[phi[i]].Z, dic[phi[i]].CamParams));
                    }
                    break;
                case enSortPoint.距离升序:
                    Array.Sort(dist1); // 
                    Array.Sort(dist2);
                    for (int i = dist2.Length - 1; i >= 0; i--)
                    {
                        listSortPoint.Add(new userWcsPoint(dic2[dist2[i]].X, dic2[dist2[i]].Y, dic2[dist2[i]].Z, dic2[dist2[i]].CamParams));
                    }
                    for (int i = 0; i < dist1.Length; i++)
                    {
                        listSortPoint.Add(new userWcsPoint(dic1[dist1[i]].X, dic1[dist1[i]].Y, dic1[dist1[i]].Z, dic1[dist1[i]].CamParams));
                    }
                    break;
                case enSortPoint.距离降序:
                    Array.Sort(dist1); // 
                    Array.Sort(dist2);
                    for (int i = dist1.Length - 1; i >= 0; i--)
                    {
                        listSortPoint.Add(new userWcsPoint(dic1[dist1[i]].X, dic1[dist1[i]].Y, dic1[dist1[i]].Z, dic1[dist1[i]].CamParams));
                    }
                    for (int i = 0; i < dist2.Length; i++)
                    {
                        listSortPoint.Add(new userWcsPoint(dic2[dist2[i]].X, dic2[dist2[i]].Y, dic2[dist2[i]].Z, dic2[dist2[i]].CamParams));
                    }
                    break;
            }
            ////
            wcsPointSort = listSortPoint.ToArray();
            listSortPoint.Clear();

        }

    }
}
