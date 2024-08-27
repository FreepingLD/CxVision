using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Common
{
    #region 像素类型结构  

    [Serializable]
    public class PixData
    {

        public string CamName { get; set; }
        public CameraParam CamParams { get; set; }
        public double Grab_x { get; set; }
        public double Grab_y { get; set; }
        public double Grab_z { get; set; }
        public double Grab_theta { get; set; }
        public double Grab_u { get; set; }
        public double Grab_v { get; set; }
        public userPixPoint[] EdgesPoint { get; set; }
        public enColor Color { get; set; }
        public object Tag { get; set; }
        public string ViewWindow { get; set; }
    }

    [Serializable]
    public class userPixRectangle1 : PixData
    {

        public double Col1 { get; set; }
        public double Row1 { get; set; }
        public double Col2 { get; set; }
        public double Row2 { get; set; }


        public userPixRectangle1(CameraParam camParam)
        {
            this.Row1 = 0;
            this.Col1 = 0;
            this.Row2 = 0;
            this.Col2 = 0;
            this.CamParams = camParam;
            this.Color = enColor.green;
            this.CamParams = new CameraParam();
            this.EdgesPoint = null;
        }
        public userPixRectangle1()
        {
            this.Row1 = 0;
            this.Col1 = 0;
            this.Row2 = 0;
            this.Col2 = 0;
            this.Color = enColor.green;
            this.CamParams = new CameraParam();
        }

        public userPixRectangle1(double left_row, double left_col, double right_row, double right_col)
        {
            this.Row1 = left_row;
            this.Col1 = left_col;
            this.Row2 = right_row;
            this.Col2 = right_col;
            this.Color = enColor.green;
            this.CamParams = new CameraParam();
        }
        public userPixRectangle1(double left_row, double left_col, double right_row, double right_col, CameraParam camParam)
        {
            this.Row1 = left_row;
            this.Col1 = left_col;
            this.Row2 = right_row;
            this.Col2 = right_col;
            this.CamParams = camParam;
            this.Color = enColor.green;
            this.CamParams = new CameraParam();
        }
        public userWcsRectangle1 GetWcsRectangle1()
        {
            HTuple Qx = 0, Qy = 0, Qz = 0; ;
            userWcsRectangle1 wcsRect1;
            this.CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsRect1 = new userWcsRectangle1(Qx[0].D, Qy[0].D, 0.0, Qx[1].D, Qy[1].D, 0.0, this.CamParams);
            wcsRect1.Grab_x = this.Grab_x;
            wcsRect1.Grab_y = this.Grab_y;
            if (this.EdgesPoint != null)
            {
                wcsRect1.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsRect1.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsRect1;
        }
        public userWcsRectangle1 GetWcsRectangle1(double refPoint_x, double refPoint_y, double refPoint_z = 0)
        {
            HTuple Qx = 0, Qy = 0, Qz = 0;
            userWcsRectangle1 wcsRect1;
            this.CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), refPoint_x, refPoint_y, refPoint_z, out Qx, out Qy, out Qz);
            wcsRect1 = new userWcsRectangle1(Qx[0].D, Qy[0].D, 0.0, Qx[1].D, Qy[1].D, 0.0, this.CamParams);
            wcsRect1.Grab_x = refPoint_x;
            wcsRect1.Grab_y = refPoint_y;
            if (this.EdgesPoint != null)
            {
                wcsRect1.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsRect1.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsRect1;
        }
        public userPixRectangle1 AffineTransPixRect1(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy;
            double Sx, Sy, Phi, Theta, Tx, Ty;
            if (homMat2D == null) return this;
            HHomMat2D hHomMat2D = new HHomMat2D(homMat2D);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            Qx = hHomMat2D.AffineTransPoint2d(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), out Qy);
            userPixRectangle1 PixRect1 = new userPixRectangle1(Qx[0].D, Qy[0].D, Qx[1].D, Qy[1].D);
            //PixRect1.diffRadius = this.diffRadius;
            PixRect1.CamParams = this.CamParams;
            ////////////////////////////////////
            PixRect1.CamName = this.CamName;
            PixRect1.Tag = this.Tag;
            PixRect1.ViewWindow = this.ViewWindow;

            return PixRect1;
        }

        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenRectangle2ContourXld((Row1 + Row2) / 2.0, (Col1 + Col2) / 2.0, 0, (Col2 - Col1) / 2.0, (Row2 - Row1) / 2.0);
            return xld;
        }
        public HXLDCont GetAllXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenRectangle2ContourXld((Row1 + Row2) / 2.0, (Col1 + Col2) / 2.0, 0, (Col2 - Col1) / 2.0, (Row2 - Row1) / 2.0);
            if (this.EdgesPoint != null)
            {
                foreach (var item in this.EdgesPoint)
                {
                    xld = xld.ConcatObj(item.GetXLD());
                }
            }
            return xld;
        }
    }
    [Serializable]
    public class userPixRectangle2 : PixData
    {
        public double Row { get; set; }
        public double Col { get; set; }
        public double Rad { get; set; }
        public double Length1 { get; set; }
        public double Length2 { get; set; }
        public double Rectangularity { get; set; }
        // 用于绘图变量
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }
        public userPixRectangle2()
        {
            this.Row = 0;
            this.Col = 0;
            this.Rad = 0;
            this.Length1 = 0;
            this.Length2 = 0;
            //////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }

        public userPixRectangle2(double center_row, double center_col, double deg, double length1, double length2)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Rad = deg;
            this.Length1 = length1;
            this.Length2 = length2;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixRectangle2(double center_row, double center_col, double deg, double length1, double length2, CameraParam CamParams)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Rad = deg;
            this.Length1 = length1;
            this.Length2 = length2;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsRectangle2 GetWcsRectangle2()
        {
            HTuple Qx = 0, Qy = 0, Qz = 0, _wcsDeg = 0;
            userWcsRectangle2 wcsRect2;
            HOperatorSet.TupleDeg(this.Rad, out _wcsDeg);
            double _wcsLength1 = this.CamParams.TransPixLengthToWcsLength(this.Length1);
            double _wcsLength2 = this.CamParams.TransPixLengthToWcsLength(this.Length2);
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsRect2 = new userWcsRectangle2(Qx[0].D, Qy[0].D, 0, _wcsDeg.D, _wcsLength1, _wcsLength2, this.CamParams);
            wcsRect2.DiffRadius = this.DiffRadius;
            wcsRect2.Color = this.Color;
            wcsRect2.Grab_x = this.Grab_x;
            wcsRect2.Grab_y = this.Grab_y;
            wcsRect2.Grab_z = this.Grab_z;
            wcsRect2.Grab_theta = this.Grab_theta;
            wcsRect2.Grab_u = this.Grab_u;
            wcsRect2.Grab_v = this.Grab_v;
            if (this.EdgesPoint != null)
            {
                wcsRect2.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsRect2.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsRect2;
        }
        public userWcsRectangle2 GetWcsRectangle2(double grab_x, double grab_y, double grab_z = 0)
        {
            HTuple Qx = 0, Qy = 0, Qz = 0, _wcsDeg = 0;
            userWcsRectangle2 wcsRect2;
            HOperatorSet.TupleDeg(this.Rad, out _wcsDeg);
            double _wcsLength1 = this.CamParams.TransPixLengthToWcsLength(this.Length1);
            double _wcsLength2 = this.CamParams.TransPixLengthToWcsLength(this.Length2);
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, grab_x, grab_y, grab_z, out Qx, out Qy, out Qz);
            wcsRect2 = new userWcsRectangle2(Qx[0].D, Qy[0].D, 0, _wcsDeg.D, _wcsLength1, _wcsLength2, this.CamParams);
            wcsRect2.Grab_x = grab_x;
            wcsRect2.Grab_y = grab_y;
            wcsRect2.Grab_z = this.Grab_z;
            wcsRect2.Grab_theta = this.Grab_theta;
            wcsRect2.Grab_u = this.Grab_u;
            wcsRect2.Grab_v = this.Grab_v;
            wcsRect2.Color = this.Color;
            wcsRect2.DiffRadius = this.DiffRadius;

            wcsRect2.Tag = this.Tag;
            wcsRect2.ViewWindow = this.ViewWindow;
            wcsRect2.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsRect2.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsRect2.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsRect2;
        }
        public drawPixRect2 GetDrawPixRect2()
        {
            drawPixRect2 pixRect2 = new drawPixRect2();
            pixRect2.Row = this.Row;
            pixRect2.Col = this.Col;
            pixRect2.Rad = this.Rad;
            pixRect2.Length1 = this.Length1;
            pixRect2.Length2 = this.Length2;
            return pixRect2;
        }
        public userPixRectangle2 AffineTransPixRect2(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            double Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
            if (homMat2D == null) return this;
            HHomMat2D hHomMat2D = new HHomMat2D(homMat2D);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            Qx = hHomMat2D.AffineTransPoint2d(this.Row, this.Col, out Qy);
            // 变换角度
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Rad + Phi);
            hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            userPixRectangle2 PixRect2 = new userPixRectangle2(Qx, Qy, Phi, this.Length1, this.Length2);
            PixRect2.DiffRadius = this.DiffRadius;
            PixRect2.CamParams = this.CamParams;
            /////////////////////////////////////
            PixRect2.Tag = this.Tag;
            PixRect2.ViewWindow = this.ViewWindow;
            PixRect2.CamName = this.CamName;

            return PixRect2;
        }

        public userPixRectangle2 AffineTransPixRect2(HHomMat2D hHomMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            double Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            Qx = hHomMat2D.AffineTransPoint2d(this.Row, this.Col, out Qy);
            // 变换角度
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Rad + Phi);
            hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            userPixRectangle2 PixRect2 = new userPixRectangle2(Qx, Qy, Phi, this.Length1, this.Length2);
            PixRect2.DiffRadius = this.DiffRadius;
            /////////////////////////////////////
            PixRect2.Tag = this.Tag;
            PixRect2.ViewWindow = this.ViewWindow;
            PixRect2.CamName = this.CamName;

            return PixRect2;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenRectangle2ContourXld(this.Row, this.Col, this.Rad, Length1, Length2);
            return xld;
        }
        public HXLDCont GetAllXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenRectangle2ContourXld(this.Row, this.Col, this.Rad, Length1, Length2);
            if (this.EdgesPoint != null)
            {
                foreach (var item in this.EdgesPoint)
                {
                    xld = xld.ConcatObj(item.GetXLD());
                }
            }
            return xld;
        }

    }

    [Serializable]
    public class userPixCircle : PixData
    {
        public double Col { get; set; }
        public double Row { get; set; }
        public double Radius { get; set; }
        public double Start_phi { get; set; }
        public double End_phi { get; set; }
        public string PointOrder { get; set; }
        public string Circularity { get; set; }
        // 扩展属性
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }
        public userPixCircle()
        {
            this.Row = 0;
            this.Col = 0;
            this.Radius = 0;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
            this.EdgesPoint = new userPixPoint[0];
        }
        public userPixCircle(CameraParam CamParams)
        {
            this.Row = 0;
            this.Col = 0;
            this.Radius = 0;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
            this.EdgesPoint = new userPixPoint[0];
        }

        public userPixCircle(double center_row, double center_col, double radius, double start_phi, double end_phi, string pointOrder, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
            this.EdgesPoint = new userPixPoint[0];
        }
        public userPixCircle(double center_row, double center_col, double radius, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
            this.EdgesPoint = new userPixPoint[0];
        }
        public userPixCircle(double center_row, double center_col, double radius, double start_phi, double end_phi, string pointOrder)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
            this.EdgesPoint = new userPixPoint[0];
        }
        public userPixCircle(double center_row, double center_col, double radius, double start_phi, double end_phi, string pointOrder, CameraParam CamParams)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
            this.EdgesPoint = new userPixPoint[0];
        }
        public userPixCircle(double center_row, double center_col, double radius)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = radius * 0.5; // GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
            this.EdgesPoint = new userPixPoint[0];
        }
        public userPixCircle(double center_row, double center_col, double radius, CameraParam CamParams)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
            this.EdgesPoint = new userPixPoint[0];
        }
        public userWcsCircle GetWcsCircle()
        {
            HTuple Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0;
            userWcsCircle wcsCircle;
            double _wcsRadius = this.CamParams.TransPixLengthToWcsLength(this.Radius);
            HOperatorSet.TupleDeg(Start_phi, out start_deg);
            HOperatorSet.TupleDeg(Start_phi, out end_deg);
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsCircle = new userWcsCircle(Qx[0].D, Qy[0].D, 0.0, _wcsRadius, start_deg.D, end_deg.D, this.CamParams);
            wcsCircle.DiffRadius = this.DiffRadius;
            wcsCircle.PointOrder = this.PointOrder;
            wcsCircle.Color = this.Color;
            wcsCircle.Grab_x = this.Grab_x;
            wcsCircle.Grab_y = this.Grab_y;
            wcsCircle.Grab_z = this.Grab_z;
            wcsCircle.Grab_theta = this.Grab_theta;
            wcsCircle.Grab_u = this.Grab_u;
            wcsCircle.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsCircle.Tag = this.Tag;
            wcsCircle.ViewWindow = this.ViewWindow;
            wcsCircle.CamName = this.CamName;
            if (this.EdgesPoint != null && this.EdgesPoint.Length > 0)
            {
                wcsCircle.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsCircle.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsCircle;
        }
        public userWcsCircle GetWcsCircle(double ref_x, double ref_y, double ref_z = 0)
        {
            double Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0;
            double _wcsRadius = this.CamParams.TransPixLengthToWcsLength(this.Radius);
            start_deg = new HTuple(this.Start_phi).TupleDeg();
            end_deg = new HTuple(this.End_phi).TupleDeg();
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            userWcsCircle wcsCircle = new userWcsCircle(Qx, Qy, 0.0, _wcsRadius, start_deg, end_deg, this.CamParams);
            wcsCircle.DiffRadius = this.DiffRadius;
            wcsCircle.PointOrder = this.PointOrder;
            wcsCircle.Color = this.Color;
            wcsCircle.Grab_x = ref_x;
            wcsCircle.Grab_y = ref_y;
            wcsCircle.Grab_z = ref_z;
            wcsCircle.Grab_theta = this.Grab_theta;
            wcsCircle.Grab_u = this.Grab_u;
            wcsCircle.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsCircle.Tag = this.Tag;
            wcsCircle.ViewWindow = this.ViewWindow;
            wcsCircle.CamName = this.CamName;
            if (this.EdgesPoint != null && this.EdgesPoint.Length > 0)
            {
                wcsCircle.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsCircle.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsCircle;
        }
        public userPixCircle AffineTransPixCircle(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi;
            userPixCircle PixCircle;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
            PixCircle = new userPixCircle(Qx[0].D, Qy[0].D, this.Radius, this.CamParams);
            PixCircle.DiffRadius = this.DiffRadius;
            PixCircle.PointOrder = this.PointOrder;
            PixCircle.CamParams = this.CamParams;

            /////////////////////////////////////
            PixCircle.Tag = this.Tag;
            PixCircle.ViewWindow = this.ViewWindow;
            PixCircle.CamName = this.CamName;
            return PixCircle;
        }
        public HXLDCont GetXLD(double nodeSize = 0)
        {
            HXLDCont xld = new HXLDCont();
            if (this.PointOrder == null || this.PointOrder == "")
                xld.GenCircleContourXld(this.Row, this.Col, Math.Abs(this.Radius - nodeSize), 0, Math.PI * 2, "positive", 0.01);
            else
                xld.GenCircleContourXld(this.Row, this.Col, Math.Abs(this.Radius - nodeSize), 0, Math.PI * 2, this.PointOrder, 0.01);
            return xld;
        }
        public HXLDCont GetAllXLD()
        {
            HXLDCont xld = new HXLDCont();
            if (this.PointOrder == null || this.PointOrder == "")
                xld.GenCircleContourXld(this.Row, this.Col, this.Radius, 0, Math.PI * 2, "positive", 0.01);
            else
                xld.GenCircleContourXld(this.Row, this.Col, this.Radius, 0, Math.PI * 2, this.PointOrder, 0.01);
            if (this.EdgesPoint != null && this.EdgesPoint.Length > 0)
            {
                foreach (var item in this.EdgesPoint)
                {
                    xld = xld.ConcatObj(item.GetXLD());
                }
            }
            return xld;
        }
    }

    [Serializable]
    public class userPixCircleSector : PixData
    {
        public double Col { get; set; }
        public double Row { get; set; }
        public double Radius { get; set; }
        public double Start_phi { get; set; }
        public double End_phi { get; set; }
        public string PointOrder { get; set; }
        // 绘图变量
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }

        public userPixCircleSector()
        {
            this.Row = 0;
            this.Col = 0;
            this.Radius = 0;
            this.Start_phi = 0;
            this.End_phi = 0;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixCircleSector(CameraParam CamParams)
        {
            this.Row = 0;
            this.Col = 0;
            this.Radius = 0;
            this.Start_phi = 0;
            this.End_phi = 0;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixCircleSector(double center_row, double center_col, double radius, double start_phi, double end_phi, string pointOrder, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixCircleSector(double center_row, double center_col, double radius, double start_phi, double end_phi, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixCircleSector(double center_row, double center_col, double radius, double start_phi, double end_phi)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixCircleSector(double center_row, double center_col, double radius, double start_phi, double end_phi, CameraParam CamParams)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius = radius;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsCircleSector GetWcsCircleSector()
        {
            double Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0;
            double _wcsRadius = this.CamParams.TransPixLengthToWcsLength(this.Radius);
            start_deg = new HTuple(this.Start_phi).TupleDeg().D;
            end_deg = new HTuple(this.End_phi).TupleDeg().D;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            userWcsCircleSector wcsCircleSector = new userWcsCircleSector(Qx, Qy, 0.0, _wcsRadius, start_deg, end_deg, this.CamParams);
            wcsCircleSector.DiffRadius = this.DiffRadius;
            wcsCircleSector.PointOrder = this.PointOrder;
            wcsCircleSector.Color = this.Color;
            wcsCircleSector.Grab_x = this.Grab_x;
            wcsCircleSector.Grab_y = this.Grab_y;
            wcsCircleSector.Grab_z = this.Grab_z;
            wcsCircleSector.Grab_theta = this.Grab_theta;
            wcsCircleSector.Grab_u = this.Grab_u;
            wcsCircleSector.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsCircleSector.Tag = this.Tag;
            wcsCircleSector.ViewWindow = this.ViewWindow;
            wcsCircleSector.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsCircleSector.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsCircleSector.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsCircleSector;
        }
        public userWcsCircleSector GetWcsCircleSector(double ref_x, double ref_y, double ref_z = 0)
        {
            double Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0;
            double _wcsRadius = this.CamParams.TransPixLengthToWcsLength(this.Radius);
            start_deg = new HTuple(this.Start_phi).TupleDeg().D;
            end_deg = new HTuple(this.End_phi).TupleDeg().D;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            userWcsCircleSector wcsCircleSector = new userWcsCircleSector(Qx, Qy, 0.0, _wcsRadius, start_deg, end_deg, this.CamParams);
            wcsCircleSector.DiffRadius = this.DiffRadius;
            wcsCircleSector.PointOrder = this.PointOrder;
            wcsCircleSector.Color = this.Color;
            wcsCircleSector.Grab_x = ref_x;
            wcsCircleSector.Grab_y = ref_y;
            wcsCircleSector.Grab_z = ref_z;
            wcsCircleSector.Grab_theta = this.Grab_theta;
            wcsCircleSector.Grab_u = this.Grab_u;
            wcsCircleSector.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsCircleSector.Tag = this.Tag;
            wcsCircleSector.ViewWindow = this.ViewWindow;
            wcsCircleSector.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsCircleSector.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsCircleSector.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsCircleSector;
        }
        public userPixCircleSector AffineTransPixCircleSector(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HHomMat2D hHomMat2D = new HHomMat2D(homMat2D);
            double Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, startPhi, endPhi, initPhi;
            Qx = hHomMat2D.AffineTransPoint2d(new HTuple(this.Row), new HTuple(this.Col), out Qy);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out initPhi, out Theta, out Tx, out Ty);
            /////////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Start_phi + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            startPhi = Phi;
            //////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.End_phi + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            endPhi = Phi;
            userPixCircleSector PixCircleSector = new userPixCircleSector(Qx, Qy, this.Radius, startPhi, endPhi, this.CamParams);
            PixCircleSector.DiffRadius = this.DiffRadius;
            PixCircleSector.PointOrder = this.PointOrder;
            /////////////////////////////////////
            PixCircleSector.Tag = this.Tag;
            PixCircleSector.ViewWindow = this.ViewWindow;
            PixCircleSector.CamName = this.CamName;
            return PixCircleSector;
        }
        public userPixCircleSector AffineTransPixCircleSector(HTuple homMat2D, enAffineTransOrientation AffineTransOrientation)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            userPixCircleSector PixCircleSector = this;
            switch (AffineTransOrientation)
            {
                case enAffineTransOrientation.正向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
                    PixCircleSector = new userPixCircleSector(Qx[0].D, Qy[0].D, this.Radius, this.Start_phi + Phi.D, this.End_phi + Phi.D);
                    break;
                ///////////////
                case enAffineTransOrientation.反向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
                    start_phi = this.Start_phi + Phi.D >= 0 ? this.Start_phi + Phi.D : this.Start_phi + Phi.D + Math.PI * 2;
                    end_phi = this.End_phi + Phi.D >= 0 ? this.End_phi + Phi.D : this.End_phi - Phi.D + Math.PI * 2;
                    PixCircleSector = new userPixCircleSector(Qx[0].D, Qy[0].D, this.Radius, start_phi, end_phi);
                    break;
                default:
                    return this;
            }
            PixCircleSector.DiffRadius = this.DiffRadius;
            PixCircleSector.PointOrder = this.PointOrder;
            /////////////////////////////////////
            PixCircleSector.Tag = this.Tag;
            PixCircleSector.ViewWindow = this.ViewWindow;
            PixCircleSector.CamName = this.CamName;
            return PixCircleSector;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCircleContourXld(this.Row, this.Col, this.Radius, this.Start_phi, this.End_phi, this.PointOrder, 0.01);
            return xld;
        }
        public HXLDCont GetAllXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCircleContourXld(this.Row, this.Col, this.Radius, this.Start_phi, this.End_phi, this.PointOrder, 0.01);
            if (this.EdgesPoint != null)
            {
                foreach (var item in this.EdgesPoint)
                {
                    xld = xld.ConcatObj(item.GetXLD());
                }
            }
            return xld;
        }
    }
    [Serializable]
    public class userPixEllipse : PixData
    {
        public double Col { get; set; }
        public double Row { get; set; }
        public double Radius1 { get; set; }
        public double Radius2 { get; set; }
        public double Rad { get; set; }
        public double Start_phi { get; set; }
        public double End_phi { get; set; }
        public string PointOrder { get; set; }
        // 专用于绘图使用
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }

        public userPixEllipse()
        {
            this.Row = 0;
            this.Col = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Rad = 0;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipse(double center_row, double center_col, double rad, double radius1, double radius2, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            ////////////////////////////////////////////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipse(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi, string pointOrder, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipse(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi, string pointOrder)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipse(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi, CameraParam CamParams)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixEllipse(double center_row, double center_col, double rad, double radius1, double radius2)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipse(double center_row, double center_col, double rad, double radius1, double radius2, CameraParam CamParams)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsEllipse GetWcsEllipse()
        {
            double Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0, deg = 0;
            deg = new HTuple(this.Rad).D;
            start_deg = new HTuple(this.Start_phi).TupleDeg().D;
            end_deg = new HTuple(this.End_phi).TupleDeg().D;
            double _wcsRadius1 = this.CamParams.TransPixLengthToWcsLength(this.Radius1);
            double _wcsRadius2 = this.CamParams.TransPixLengthToWcsLength(this.Radius2);
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            userWcsEllipse wcsEllipse = new userWcsEllipse(Qx, Qy, 0, deg, _wcsRadius1, _wcsRadius2, start_deg, end_deg, this.CamParams);
            ///////////////////////
            wcsEllipse.DiffRadius = this.DiffRadius;
            wcsEllipse.Color = this.Color;
            wcsEllipse.PointOrder = this.PointOrder;
            wcsEllipse.Grab_x = this.Grab_x;
            wcsEllipse.Grab_y = this.Grab_y;
            wcsEllipse.Grab_z = this.Grab_z;
            wcsEllipse.Grab_theta = this.Grab_theta;
            wcsEllipse.Grab_u = this.Grab_u;
            wcsEllipse.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsEllipse.Tag = this.Tag;
            wcsEllipse.ViewWindow = this.ViewWindow;
            wcsEllipse.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsEllipse.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsEllipse.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsEllipse;
        }
        public userWcsEllipse GetWcsEllipse(double ref_x, double ref_y, double ref_z = 0)
        {
            double Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0, deg = 0;
            deg = new HTuple(this.Rad).D;
            start_deg = new HTuple(this.Start_phi).TupleDeg().D;
            end_deg = new HTuple(this.End_phi).TupleDeg().D;
            double _wcsRadius1 = this.CamParams.TransPixLengthToWcsLength(this.Radius1);
            double _wcsRadius2 = this.CamParams.TransPixLengthToWcsLength(this.Radius2);
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            userWcsEllipse wcsEllipse = new userWcsEllipse(Qx, Qy, 0, deg, _wcsRadius1, _wcsRadius2, start_deg, end_deg, this.CamParams);
            ///////////////////////
            wcsEllipse.DiffRadius = this.DiffRadius;
            wcsEllipse.Color = this.Color;
            wcsEllipse.PointOrder = this.PointOrder;
            wcsEllipse.Grab_x = ref_x;
            wcsEllipse.Grab_y = ref_y;
            wcsEllipse.Grab_z = ref_z;
            wcsEllipse.Grab_theta = this.Grab_theta;
            wcsEllipse.Grab_u = this.Grab_u;
            wcsEllipse.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsEllipse.Tag = this.Tag;
            wcsEllipse.ViewWindow = this.ViewWindow;
            wcsEllipse.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsEllipse.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsEllipse.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsEllipse;
        }


        public userPixEllipse AffineTransPixEllipse(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HHomMat2D hHomMat2D = new HHomMat2D(homMat2D);
            double Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, rad, startPhi, endPhi, initPhi;
            Qx = hHomMat2D.AffineTransPoint2d(new HTuple(this.Row), new HTuple(this.Col), out Qy); // 变换中心点
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out initPhi, out Theta, out Tx, out Ty);
            /////////////////////////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Rad + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            rad = Phi;
            /////////////////////////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Start_phi + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            startPhi = Phi;
            ////////////////////////////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.End_phi + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            endPhi = Phi;
            ///////////////////
            userPixEllipse pixEllipse = new userPixEllipse(Qx, Qy, rad, this.Radius1, this.Radius2, this.CamParams);
            pixEllipse.DiffRadius = this.DiffRadius;
            pixEllipse.PointOrder = this.PointOrder;
            pixEllipse.Start_phi = startPhi;
            pixEllipse.End_phi = endPhi;
            /////////////////////////////////////
            pixEllipse.Tag = this.Tag;
            pixEllipse.ViewWindow = this.ViewWindow;
            pixEllipse.CamName = this.CamName;
            return pixEllipse;
        }

        public userPixEllipse AffineTransPixEllipse(HTuple homMat2D, enAffineTransOrientation AffineTransOrientation)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, ratate_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            userPixEllipse pixEllipse = this;
            switch (AffineTransOrientation)
            {
                case enAffineTransOrientation.正向变换: // 椭圆旋转整体即可
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
                    pixEllipse = new userPixEllipse(Qx[0].D, Qy[0].D, this.Rad + Phi.D, this.Radius1, this.Radius2);
                    pixEllipse.DiffRadius = this.DiffRadius;
                    break;
                ///////////////
                case enAffineTransOrientation.反向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
                    ratate_phi = this.Rad + Phi.D >= 0 ? this.Rad + Phi.D : this.Rad + Phi.D + Math.PI * 2;
                    pixEllipse = new userPixEllipse(Qx[0].D, Qy[0].D, ratate_phi, this.Radius1, this.Radius2);
                    pixEllipse.DiffRadius = this.DiffRadius;
                    break;
            }
            pixEllipse.DiffRadius = this.DiffRadius;
            pixEllipse.PointOrder = this.PointOrder;
            /////////////////////////////////////
            pixEllipse.Tag = this.Tag;
            pixEllipse.ViewWindow = this.ViewWindow;
            pixEllipse.CamName = this.CamName;
            return pixEllipse;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenEllipseContourXld(this.Row, this.Col, this.Rad, this.Radius1, this.Radius2, 0, Math.PI * 2, this.PointOrder, 0.01);
            return xld;
        }
        public HXLDCont GetAllXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenEllipseContourXld(this.Row, this.Col, this.Rad, this.Radius1, this.Radius2, 0, Math.PI * 2, this.PointOrder, 0.01);
            if (this.EdgesPoint != null)
            {
                foreach (var item in this.EdgesPoint)
                {
                    xld = xld.ConcatObj(item.GetXLD());
                }
            }
            return xld;
        }

    }
    [Serializable]
    public class userPixEllipseSector : PixData
    {
        public double Col { get; set; }
        public double Row { get; set; }
        public double Radius1 { get; set; }
        public double Radius2 { get; set; }
        public double Rad { get; set; }
        public double Start_phi { get; set; }
        public double End_phi { get; set; }
        public string PointOrder { get; set; }
        // 用于绘图变量
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }

        public userPixEllipseSector()
        {
            this.Row = 0;
            this.Col = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Rad = 0;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            ///////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipseSector(CameraParam CamParams)
        {
            this.Row = 0;
            this.Col = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Rad = 0;
            this.Start_phi = 0;
            this.End_phi = Math.PI * 2;
            this.PointOrder = "positive";
            ///////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixEllipseSector(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = "positive";
            ///////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }

        public userPixEllipseSector(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi, string pointOrder, HTuple camParam, HTuple camPose)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            ///////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipseSector(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi, string pointOrder)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = pointOrder;
            ///////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipseSector(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = "positive";
            ///////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixEllipseSector(double center_row, double center_col, double rad, double radius1, double radius2, double start_phi, double end_phi, CameraParam CamParams)
        {
            this.Row = center_row;
            this.Col = center_col;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Rad = rad;
            this.Start_phi = start_phi;
            this.End_phi = end_phi;
            this.PointOrder = "positive";
            ///////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsEllipseSector GetWcsEllipseSector()
        {
            userWcsEllipseSector wcsEllipseSector;
            double Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0, deg = 0;
            deg = new HTuple(this.Rad).TupleRad().D;
            start_deg = new HTuple(this.Start_phi).TupleDeg().D;
            end_deg = new HTuple(this.End_phi).TupleDeg().D;
            double _wcsRadius1 = this.CamParams.TransPixLengthToWcsLength(this.Radius1);
            double _wcsRadius2 = this.CamParams.TransPixLengthToWcsLength(this.Radius2);
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsEllipseSector = new userWcsEllipseSector(Qx, Qy, 0, deg, _wcsRadius1, _wcsRadius2, start_deg, end_deg, this.CamParams);
            wcsEllipseSector.DiffRadius = this.DiffRadius;
            wcsEllipseSector.PointOrder = this.PointOrder;
            wcsEllipseSector.Color = this.Color;
            wcsEllipseSector.Grab_x = this.Grab_x;
            wcsEllipseSector.Grab_y = this.Grab_y;
            wcsEllipseSector.Grab_z = this.Grab_z;
            wcsEllipseSector.Grab_theta = this.Grab_theta;
            wcsEllipseSector.Grab_u = this.Grab_u;
            wcsEllipseSector.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsEllipseSector.Tag = this.Tag;
            wcsEllipseSector.ViewWindow = this.ViewWindow;
            wcsEllipseSector.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsEllipseSector.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsEllipseSector.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }

            return wcsEllipseSector;
        }
        public userWcsEllipseSector GetWcsEllipseSector(double ref_x, double ref_y, double ref_z = 0)
        {
            userWcsEllipseSector wcsEllipseSector;
            double Qx = 0, Qy = 0, Qz = 0, start_deg = 0, end_deg = 0, deg = 0;
            deg = new HTuple(this.Rad).TupleDeg().D;
            start_deg = new HTuple(this.Start_phi).TupleDeg().D;
            end_deg = new HTuple(this.End_phi).TupleDeg().D;
            double _wcsRadius1 = this.CamParams.TransPixLengthToWcsLength(this.Radius1);
            double _wcsRadius2 = this.CamParams.TransPixLengthToWcsLength(this.Radius2);
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            wcsEllipseSector = new userWcsEllipseSector(Qx, Qy, 0, deg, _wcsRadius1, _wcsRadius2, start_deg, end_deg, this.CamParams);
            wcsEllipseSector.DiffRadius = this.DiffRadius;
            wcsEllipseSector.PointOrder = this.PointOrder;
            wcsEllipseSector.Color = this.Color;
            wcsEllipseSector.Grab_x = ref_x;
            wcsEllipseSector.Grab_y = ref_y;
            wcsEllipseSector.Grab_z = ref_z;
            wcsEllipseSector.Grab_theta = this.Grab_theta;
            wcsEllipseSector.Grab_u = this.Grab_u;
            wcsEllipseSector.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsEllipseSector.Tag = this.Tag;
            wcsEllipseSector.ViewWindow = this.ViewWindow;
            wcsEllipseSector.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsEllipseSector.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsEllipseSector.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsEllipseSector;
        }
        public userPixEllipseSector AffineTransPixEllipseSector(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HHomMat2D hHomMat2D = new HHomMat2D(homMat2D);
            double Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, rad, startPhi, endPhi, initPhi;
            Qx = hHomMat2D.AffineTransPoint2d(new HTuple(this.Row), new HTuple(this.Col), out Qy); // 变换中心点
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out initPhi, out Theta, out Tx, out Ty);
            /////////////////////////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Rad + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            rad = Phi;
            /////////////////////////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Start_phi + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            startPhi = Phi;
            ////////////////////////////////////
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.End_phi + initPhi);
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            endPhi = Phi;
            ///////////////////
            userPixEllipseSector pixEllipseSector = new userPixEllipseSector(Qx, Qy, rad, this.Radius1, this.Radius2, this.Start_phi, this.End_phi, this.CamParams);
            pixEllipseSector.DiffRadius = this.DiffRadius;
            pixEllipseSector.PointOrder = this.PointOrder;
            /////////////////////////////////////
            pixEllipseSector.Tag = this.Tag;
            pixEllipseSector.ViewWindow = this.ViewWindow;
            pixEllipseSector.CamName = this.CamName;
            return pixEllipseSector;
        }
        public userPixEllipseSector AffineTransPixEllipseSector(HTuple homMat2D, enAffineTransOrientation AffineTransOrientation)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi, ratate_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            userPixEllipseSector pixEllipseSector = this;
            switch (AffineTransOrientation)
            {
                case enAffineTransOrientation.正向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
                    pixEllipseSector = new userPixEllipseSector(Qx[0].D, Qy[0].D, this.Rad + Phi.D, this.Radius1, this.Radius2, this.Start_phi + Phi.D, this.End_phi + Phi.D);
                    break;
                ///////////////
                case enAffineTransOrientation.反向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
                    start_phi = this.Start_phi + Phi.D >= 0 ? this.Start_phi + Phi.D : this.Start_phi + Phi.D + Math.PI * 2;
                    end_phi = this.End_phi + Phi.D >= 0 ? this.End_phi + Phi.D : this.End_phi - Phi.D + Math.PI * 2;
                    ratate_phi = this.Rad + Phi.D >= 0 ? this.Rad + Phi.D : this.Rad + Phi.D + Math.PI * 2; // 旋转整体了就不旋转起始角和终止角，两者只可旋转一个
                    pixEllipseSector = new userPixEllipseSector(Qx[0].D, Qy[0].D, this.Rad + ratate_phi, this.Radius1, this.Radius2, start_phi, end_phi);
                    break;
            }
            pixEllipseSector.DiffRadius = this.DiffRadius;
            /////////////////////////////////////
            pixEllipseSector.Tag = this.Tag;
            pixEllipseSector.ViewWindow = this.ViewWindow;
            pixEllipseSector.CamName = this.CamName;
            return pixEllipseSector;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenEllipseContourXld(this.Row, this.Col, this.Rad, this.Radius1, this.Radius2, this.Start_phi, this.End_phi, this.PointOrder, 0.1);
            return xld;
        }
        public HXLDCont GetAllXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenEllipseContourXld(this.Row, this.Col, this.Rad, this.Radius1, this.Radius2, this.Start_phi, this.End_phi, this.PointOrder, 0.1);
            if (this.EdgesPoint != null)
            {
                foreach (var item in this.EdgesPoint)
                {
                    xld = xld.ConcatObj(item.GetXLD());
                }
            }
            return xld;
        }
    }

    [Serializable]
    public class userPixPoint : PixData
    {
        public double Row { get; set; }
        public double Col { get; set; }
        public double Size { get; set; }
        // 专用于绘图使用
        public double DiffRadius { get; set; }
        public userPixPoint()
        {
            this.Row = 0;
            this.Col = 0;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = null;
            this.Size = 15;
        }
        public userPixPoint(double row, double col)
        {
            this.Row = row;
            this.Col = col;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = null;
            this.Size = 15;
        }

        public userPixPoint(double row, double col, CameraParam CamParams)
        {
            this.Row = row;
            this.Col = col;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = CamParams;
            this.Size = 15;
        }
        public userPixPoint(double row, double col, double grab_x, double grab_y, double grab_theta, CameraParam CamParams)
        {
            this.Row = row;
            this.Col = col;
            this.Grab_x = grab_x;
            this.Grab_y = grab_y;
            this.Grab_theta = grab_theta;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = CamParams;
            this.Size = 15;
        }
        public drawPixPoint GetPixPoint()
        {
            drawPixPoint pixPoint = new drawPixPoint();
            pixPoint.Row = this.Row;
            pixPoint.Col = this.Col;
            return pixPoint;
        }
        public userWcsPoint GetWcsPoint(double ref_x, double ref_y, double ref_z = 0)
        {
            userWcsPoint wcsPoint;
            double Qx = 0, Qy = 0, Qz = 0;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            wcsPoint = new userWcsPoint(Qx, Qy, 0.0, this.CamParams);
            wcsPoint.DiffRadius = this.DiffRadius;
            wcsPoint.Color = this.Color;
            wcsPoint.Grab_x = ref_x;
            wcsPoint.Grab_y = ref_y;
            return wcsPoint;
        }
        public userWcsPoint GetWcsPoint()
        {
            userWcsPoint wcsPoint;
            double Qx = 0, Qy = 0, Qz = 0;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsPoint = new userWcsPoint(Qx, Qy, 0.0, this.CamParams);
            wcsPoint.DiffRadius = this.DiffRadius;
            wcsPoint.Color = this.Color;
            wcsPoint.Grab_x = this.Grab_x;
            wcsPoint.Grab_y = this.Grab_y;
            /////////////////////////////////////
            wcsPoint.Tag = this.Tag;
            wcsPoint.ViewWindow = this.ViewWindow;
            wcsPoint.CamName = this.CamName;
            return wcsPoint;
        }
        public userPixPoint AffineTransPixPoint(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, ratate_phi;
            userPixPoint pixPoint;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
            pixPoint = new userPixPoint(Qx[0].D, Qy[0].D, this.CamParams);
            pixPoint.DiffRadius = this.DiffRadius;
            /////////////////////////////////////
            pixPoint.Tag = this.Tag;
            pixPoint.ViewWindow = this.ViewWindow;
            pixPoint.CamName = this.CamName;
            return pixPoint;
        }
        public HXLDCont GetXLD(double size = 5)
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCrossContourXld(this.Row, this.Col, size, 0);
            return xld;
        }

    }


    [Serializable]
    public class userPixVector : PixData
    {
        public double Row { get; set; }
        public double Col { get; set; }
        public double Rad { get; set; }
        public userPixVector()
        {
            this.Row = 0;
            this.Col = 0;
            this.Rad = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }

        public userPixVector(double row, double col, double phi)
        {
            this.Row = row;
            this.Col = col;
            this.Rad = phi;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixVector(double row, double col, double phi, CameraParam CamParams)
        {
            this.Row = row;
            this.Col = col;
            this.Rad = phi;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsVector GetWcsVector()
        {
            userWcsVector wcsVector;
            double Qx = 0, Qy = 0, Qz = 0, deg = 0;
            deg = new HTuple(this.Rad).TupleDeg().D * 1;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsVector = new userWcsVector(Qx, Qy, 0, deg, this.CamParams);
            wcsVector.Color = this.Color;
            wcsVector.Grab_x = this.Grab_x;
            wcsVector.Grab_y = this.Grab_y;
            wcsVector.Grab_z = this.Grab_z;
            wcsVector.Grab_theta = this.Grab_theta;
            wcsVector.Grab_u = this.Grab_u;
            wcsVector.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsVector.Tag = this.Tag;
            wcsVector.ViewWindow = this.ViewWindow;
            wcsVector.CamName = this.CamName;
            return wcsVector;
        }
        public userWcsVector GetWcsVector(double ref_x, double ref_y, double ref_z = 0)
        {
            userWcsVector wcsVector;
            double Qx = 0, Qy = 0, Qz = 0, deg = 0;
            deg = new HTuple(this.Rad).TupleDeg().D * 1;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            wcsVector = new userWcsVector(Qx, Qy, 0, deg, this.CamParams);
            wcsVector.Color = this.Color;
            wcsVector.Grab_x = ref_x;
            wcsVector.Grab_y = ref_y;
            wcsVector.Grab_z = ref_z;
            wcsVector.Grab_theta = this.Grab_theta;
            wcsVector.Grab_u = this.Grab_u;
            wcsVector.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsVector.Tag = this.Tag;
            wcsVector.ViewWindow = this.ViewWindow;
            wcsVector.CamName = this.CamName;
            return wcsVector;
        }

        public static userPixVector operator +(userPixVector Vector1, userPixVector Vector2)
        {
            userPixVector NewVector = new userPixVector();
            NewVector.Col = Vector1.Col + Vector2.Col;
            NewVector.Row = Vector1.Row + Vector2.Row;
            NewVector.Rad = Vector1.Rad + Vector2.Rad;
            NewVector.Color = Vector1.Color;
            return NewVector;
        }

        public static userPixVector operator -(userPixVector Vector1, userPixVector Vector2)
        {
            userPixVector NewVector = new userPixVector();
            NewVector.Col = Vector1.Col - Vector2.Col;
            NewVector.Row = Vector1.Row - Vector2.Row;
            NewVector.Rad = Vector1.Rad - Vector2.Rad;
            NewVector.Color = Vector1.Color;
            return NewVector;
        }
    }

    [Serializable]
    public class userPixScaleVector : PixData // 包含三种情况，不缩放，各向同向缩放，各向异向缩放
    {
        public double Row { get; set; }
        public double Col { get; set; }
        public double Rad { get; set; }
        public double Sr { get; set; }
        public double Sc { get; set; }
        public userPixScaleVector()
        {
            this.Row = 0;
            this.Col = 0;
            this.Rad = 0;
            this.Sr = 1;
            this.Sc = 1;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixScaleVector(double row, double col, double phi, double Sr, double Sc)
        {
            this.Row = row;
            this.Col = col;
            this.Rad = phi;
            this.Sr = Sr;
            this.Sc = Sc;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixScaleVector(double row, double col, double phi, double Sr, double Sc, CameraParam CamParams)
        {
            this.Row = row;
            this.Col = col;
            this.Rad = phi;
            this.Sr = Sr;
            this.Sc = Sc;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }

        public userWcsScaleVector GetWcsScaleVector()
        {
            userWcsScaleVector wcsPoint;
            double Qx = 0, Qy = 0, Qz = 0, deg = 0;
            deg = new HTuple(this.Rad).TupleDeg().D;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsPoint = new userWcsScaleVector(Qx, Qy, 0, deg, this.Sr, this.Sc, this.CamParams);
            wcsPoint.Grab_x = this.Grab_x;
            wcsPoint.Grab_y = this.Grab_y;
            wcsPoint.Grab_z = this.Grab_z;
            wcsPoint.Grab_theta = this.Grab_theta;
            wcsPoint.Grab_u = this.Grab_u;
            wcsPoint.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsPoint.Tag = this.Tag;
            wcsPoint.ViewWindow = this.ViewWindow;
            wcsPoint.CamName = this.CamName;
            return wcsPoint;
        }
        public userWcsScaleVector GetWcsScaleVector(double ref_x, double ref_y, double ref_z = 0)
        {
            userWcsScaleVector wcsPoint;
            double Qx = 0, Qy = 0, Qz = 0, deg = 0;
            deg = new HTuple(this.Rad).TupleDeg().D;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            wcsPoint = new userWcsScaleVector(Qx, Qy, 0, deg, this.Sr, this.Sc, this.CamParams);
            /////////////////////////////////////
            wcsPoint.Grab_x = ref_x;
            wcsPoint.Grab_y = ref_y;
            wcsPoint.Grab_z = ref_z;
            wcsPoint.Grab_theta = this.Grab_theta;
            wcsPoint.Grab_u = this.Grab_u;
            wcsPoint.Grab_v = this.Grab_v;
            wcsPoint.Tag = this.Tag;
            wcsPoint.ViewWindow = this.ViewWindow;
            wcsPoint.CamName = this.CamName;
            return wcsPoint;
        }
        public static userPixScaleVector operator +(userPixScaleVector Vector1, userPixScaleVector Vector2)
        {
            userPixScaleVector NewVector = new userPixScaleVector();
            NewVector.Col = Vector1.Col + Vector2.Col;
            NewVector.Row = Vector1.Row + Vector2.Row;
            NewVector.Rad = Vector1.Rad + Vector2.Rad;
            NewVector.CamParams = Vector1.CamParams;
            NewVector.Color = Vector1.Color;
            NewVector.Sr = Vector1.Sr;
            NewVector.Sc = Vector1.Sc;
            return NewVector;
        }
        public static userPixScaleVector operator -(userPixScaleVector Vector1, userPixScaleVector Vector2)
        {
            userPixScaleVector NewVector = new userPixScaleVector();
            NewVector.Col = Vector1.Col - Vector2.Col;
            NewVector.Row = Vector1.Row - Vector2.Row;
            NewVector.Rad = Vector1.Rad - Vector2.Rad;
            NewVector.CamParams = Vector1.CamParams;
            NewVector.Color = Vector1.Color;
            NewVector.Sr = Vector1.Sr;
            NewVector.Sc = Vector1.Sc;
            return NewVector;
        }


    }



    [Serializable]
    public class userPixLine : PixData
    {
        public double Col1 { get; set; }
        public double Row1 { get; set; }
        public double Col2 { get; set; }
        public double Row2 { get; set; }
        // 扩展属性
        public double DiffRadius { get; set; }
        public double NormalPhi { get; set; }

        public userPixLine()
        {
            this.Row1 = 0;
            this.Col1 = 0;
            this.Row2 = 0;
            this.Col2 = 0;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }

        public userPixLine(double row1, double col1, double row2, double col2, HTuple camParam, HTuple camPose)
        {
            this.Row1 = row1;
            this.Col1 = col1;
            this.Row2 = row2;
            this.Col2 = col2;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixLine(double row1, double col1, double row2, double col2)
        {
            this.Row1 = row1;
            this.Col1 = col1;
            this.Row2 = row2;
            this.Col2 = col2;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixLine(double row1, double col1, double row2, double col2, CameraParam CamParams)
        {
            this.Row1 = row1;
            this.Col1 = col1;
            this.Row2 = row2;
            this.Col2 = col2;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }

        public userWcsLine GetWcsLine()
        {
            userWcsLine wcsLine;
            HTuple Qx = 0, Qy = 0, Qz = 0;
            this.CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsLine = new userWcsLine(Qx[0].D, Qy[0].D, 0, Qx[1].D, Qy[1].D, 0, this.CamParams);
            wcsLine.DiffRadius = this.DiffRadius;
            wcsLine.NormalPhi = this.NormalPhi;
            wcsLine.Color = this.Color;
            wcsLine.Grab_x = this.Grab_x;
            wcsLine.Grab_y = this.Grab_y;
            wcsLine.Grab_z = this.Grab_z;
            wcsLine.Grab_theta = this.Grab_theta;
            wcsLine.Grab_u = this.Grab_u;
            wcsLine.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsLine.Tag = this.Tag;
            wcsLine.ViewWindow = this.ViewWindow;
            wcsLine.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsLine.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsLine.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsLine;
        }


        public userWcsLine GetWcsLine(double ref_x, double ref_y, double ref_z = 0)
        {
            userWcsLine wcsLine;
            HTuple Qx = 0, Qy = 0, Qz = 0;
            this.CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            wcsLine = new userWcsLine(Qx[0].D, Qy[0].D, 0, Qx[1].D, Qy[1].D, 0, this.CamParams);
            wcsLine.DiffRadius = this.DiffRadius;
            wcsLine.NormalPhi = this.NormalPhi;
            wcsLine.Color = this.Color;
            wcsLine.Grab_x = ref_x;
            wcsLine.Grab_y = ref_y;
            wcsLine.Grab_z = ref_z;
            wcsLine.Grab_theta = this.Grab_theta;
            wcsLine.Grab_u = this.Grab_u;
            wcsLine.Grab_v = this.Grab_v;
            /////////////////////////////////////
            wcsLine.Tag = this.Tag;
            wcsLine.ViewWindow = this.ViewWindow;
            wcsLine.CamName = this.CamName;
            if (this.EdgesPoint != null)
            {
                wcsLine.EdgesPoint_xyz = new userWcsPoint[this.EdgesPoint.Length];
                for (int i = 0; i < this.EdgesPoint.Length; i++)
                {
                    wcsLine.EdgesPoint_xyz[i] = this.EdgesPoint[i].GetWcsPoint();
                }
            }
            return wcsLine;
        }
        public userPixLine AffinePixLine2D(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, hTuple;
            userPixLine pixLine;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), out Qx, out Qy);
            HOperatorSet.VectorAngleToRigid(0, 0, 0, 0, 0, this.NormalPhi + Phi, out hTuple);
            HOperatorSet.HomMat2dToAffinePar(hTuple, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            ////////////////////////////////////////////
            pixLine = new userPixLine(Qx[0].D, Qy[0].D, Qx[1].D, Qy[1].D, this.CamParams);
            pixLine.DiffRadius = this.DiffRadius;
            pixLine.NormalPhi = Phi; // 法向角也需要变
            /////////////////////////////////////
            pixLine.Tag = this.Tag;
            pixLine.ViewWindow = this.ViewWindow;
            pixLine.CamName = this.CamName;
            return pixLine;
        }

        public HXLDCont GetXLD()
        {
            return new HXLDCont(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2));
        }

        public HXLDCont GetAllXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld = new HXLDCont(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2));
            if (this.EdgesPoint != null)
            {
                foreach (var item in this.EdgesPoint)
                {
                    xld = xld.ConcatObj(item.GetXLD());
                }
            }
            return xld;
        }
    }

    [Serializable]
    public class userPixCoordSystem
    {
        public userPixVector ReferencePoint { get; set; }
        public userPixVector CurrentPoint { get; set; }// 这里不用数组，是为了操作方便，在外面需要多个坐标的地方使用数组
        public enColor Color { get; set; }
        public bool Result { get; set; }

        public userPixCoordSystem()
        {
            this.ReferencePoint = new userPixVector();
            this.CurrentPoint = new userPixVector();
            this.Color = enColor.green;
            this.Result = false;
        }
        public userPixCoordSystem(userPixVector referencePoint, userPixVector currentPoint, bool result)
        {
            this.ReferencePoint = referencePoint;
            this.CurrentPoint = currentPoint;
            this.Color = enColor.green;
            this.Result = false;
        }
        public userPixCoordSystem(userPixVector referencePoint, userPixVector currentPoint)
        {
            this.ReferencePoint = referencePoint;
            this.CurrentPoint = currentPoint;
            this.Color = enColor.green;
            this.Result = false;
        }
        public userPixCoordSystem(userPixVector currentPoint)
        {
            ReferencePoint = new userPixVector();
            CurrentPoint = currentPoint;
            this.Color = enColor.green;
            this.Result = false;
        }

        public userWcsCoordSystem GetWcsCoordSystem()
        {
            userWcsVector wcsVector1 = this.ReferencePoint.GetWcsVector();
            userWcsVector wcsVector2 = this.CurrentPoint.GetWcsVector();
            userWcsCoordSystem coordSystem = new userWcsCoordSystem(wcsVector1, wcsVector2);
            coordSystem.Result = this.Result;
            coordSystem.Color = this.Color;
            return coordSystem;
        }

        public HTuple GetVariationHomMat2D()
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            //hHomMat2D.VectorAngleToRigid(this.CurrentPoint.row, this.CurrentPoint.col, this.CurrentPoint.rad,
            //    this.ReferencePoint.row, this.ReferencePoint.col, this.ReferencePoint.rad);
            hHomMat2D.VectorAngleToRigid(this.ReferencePoint.Row, this.ReferencePoint.Col, this.ReferencePoint.Rad,
                                         this.CurrentPoint.Row, this.CurrentPoint.Col, this.CurrentPoint.Rad);
            return hHomMat2D.RawData;
            //return homMat2dRotate;
        }
        public HHomMat2D GetHomMat2D()
        {
            HHomMat2D hHomMat2D = new HHomMat2D(); // 
            //hHomMat2D.VectorAngleToRigid(this.CurrentPoint.row, this.CurrentPoint.col, this.CurrentPoint.rad,
            //    this.ReferencePoint.row, this.ReferencePoint.col, this.ReferencePoint.rad);
            hHomMat2D.VectorAngleToRigid(this.ReferencePoint.Row, this.ReferencePoint.Col, this.ReferencePoint.Rad,
                                         this.CurrentPoint.Row, this.CurrentPoint.Col, this.CurrentPoint.Rad);
            return hHomMat2D;


            //return homMat2dRotate;
        }
        public HHomMat2D GetRectifyImageHomMat2D()
        {
            HHomMat2D hHomMat2D = new HHomMat2D(); // 
            hHomMat2D.VectorAngleToRigid(this.CurrentPoint.Row, this.CurrentPoint.Col, this.CurrentPoint.Rad,
                this.ReferencePoint.Row, this.ReferencePoint.Col, 0); // 将当前位置变换到参考位置
            return hHomMat2D;


            //return homMat2dRotate;
        }
        public HTuple GetInvertVariationHomMat2D()
        {
            HTuple homMat2dInvert;
            HOperatorSet.HomMat2dInvert(GetVariationHomMat2D(), out homMat2dInvert);
            return homMat2dInvert;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = GenArrowContourXld(new HTuple(CurrentPoint.Row, CurrentPoint.Row), new HTuple(CurrentPoint.Col, CurrentPoint.Col), new HTuple(CurrentPoint.Col + 100, CurrentPoint.Col), new HTuple(CurrentPoint.Row, CurrentPoint.Row + 100), 15, 10);
            return xld;
        }
        private HXLDCont GenArrowContourXld(HTuple Row1, HTuple Column1, HTuple Row2, HTuple Column2, double HeadLength, double HeadWidth)
        {
            HXLDCont Arrow = new HXLDCont();
            Arrow.GenEmptyObj();
            HTuple Length, ZeroLengthIndices;
            HOperatorSet.DistancePp(Row1, Column1, Row2, Column2, out Length);
            ZeroLengthIndices = Length.TupleFind(0);
            if (ZeroLengthIndices != -1)
                Length[ZeroLengthIndices] = -1;
            HTuple DR = 1.0 * (Row2 - Row1) / Length;
            HTuple DC = 1.0 * (Column2 - Column1) / Length;
            HTuple HalfHeadWidth = HeadWidth / 2.0;
            // Calculate end points of the arrow head.
            HTuple RowP1 = Row1 + (Length - HeadLength) * DR + HalfHeadWidth * DC;
            HTuple ColP1 = Column1 + (Length - HeadLength) * DC - HalfHeadWidth * DR;
            HTuple RowP2 = Row1 + (Length - HeadLength) * DR - HalfHeadWidth * DC;
            HTuple ColP2 = Column1 + (Length - HeadLength) * DC + HalfHeadWidth * DR;
            // Finally create output XLD contour for each input point pair
            HXLDCont TempArrow;
            for (int i = 0; i < Length.Length; i++)
            {
                if (Length[i].D == -1)
                    //Create_ single points for arrows with identical start and end point
                    TempArrow = new HXLDCont(Row1[i], Column1[i]);
                else
                    //Create arrow contour
                    TempArrow = new HXLDCont(new HTuple(Row1[i].D, Row2[i].D, RowP1[i].D, Row2[i].D, RowP2[i].D, Row2[i].D), new HTuple(Column1[i].D, Column2[i].D, ColP1[i].D, Column2[i].D, ColP2[i].D, Column2[i].D));
                Arrow = Arrow.ConcatObj(TempArrow);
            }
            Arrow = Arrow.UnionAdjacentContoursXld(10, 1, "attr_keep");
            return Arrow;
        }


    }

    [Serializable]
    public class userPixPolygon : PixData
    {
        public List<double> Row { get; set; }
        public List<double> Col { get; set; }
        // 扩展属性
        public double DiffRadius { get; set; }
        public double NormalPhi { get; set; }
        public userPixPolygon()
        {
            this.Row = new List<double>();
            this.Col = new List<double>();
        }
        public userPixPolygon(double[] rows, double[] cols)
        {
            this.Row = new List<double>();
            this.Col = new List<double>();
            this.Row.AddRange(rows);
            this.Col.AddRange(cols);
            this.CamParams = new CameraParam();
        }
        public userPixPolygon(double[] rows, double[] cols, CameraParam CamParams)
        {
            this.Row = new List<double>();
            this.Col = new List<double>();
            this.Row.AddRange(rows);
            this.Col.AddRange(cols);
            this.CamParams = CamParams;
        }
        public userPixPolygon AffinePixPolygon(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row.ToArray()), new HTuple(this.Col.ToArray()), out Qx, out Qy);
            userPixPolygon PixRect1 = new userPixPolygon(Qx.DArr, Qy.DArr);

            /////////////////////////////////////
            PixRect1.Tag = this.Tag;
            PixRect1.ViewWindow = this.ViewWindow;
            PixRect1.CamName = this.CamName;
            return PixRect1;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (this.Row.Count > 0)
                hXLDCont.GenContourPolygonXld(new HTuple(this.Row.ToArray(), this.Row[0]), new HTuple(this.Col.ToArray(), this.Col[0]));
            return hXLDCont;
        }


    }

    [Serializable]
    public class userPixPolyLine : PixData
    {
        public List<double> Row { get; set; }
        public List<double> Col { get; set; }
        // 扩展属性
        public double DiffRadius { get; set; }
        public double NormalPhi { get; set; }
        public userPixPolyLine()
        {
            this.Row = new List<double>();
            this.Col = new List<double>();
        }
        public userPixPolyLine(double[] rows, double[] cols)
        {
            this.Row = new List<double>();
            this.Col = new List<double>();
            this.Row.AddRange(rows);
            this.Col.AddRange(cols);
            this.CamParams = new CameraParam();
        }
        public userPixPolyLine(double[] rows, double[] cols, CameraParam CamParams)
        {
            this.Row = new List<double>();
            this.Col = new List<double>();
            this.Row.AddRange(rows);
            this.Col.AddRange(cols);
            this.CamParams = CamParams;
        }
        public userPixPolyLine AffinePixPolyLine(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row.ToArray()), new HTuple(this.Col.ToArray()), out Qx, out Qy);
            userPixPolyLine PixRect1 = new userPixPolyLine(Qx.DArr, Qy.DArr);

            /////////////////////////////////////
            PixRect1.Tag = this.Tag;
            PixRect1.ViewWindow = this.ViewWindow;
            PixRect1.CamName = this.CamName;
            return PixRect1;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (this.Row.Count > 0)
                hXLDCont.GenContourPolygonXld(new HTuple(this.Row.ToArray(), this.Row[0]), new HTuple(this.Col.ToArray(), this.Col[0]));
            return hXLDCont;
        }


    }
    [Serializable]
    public class userPixThick : PixData
    {
        public double Row { get; set; }
        public double Col { get; set; }
        public double Dist1 { get; set; }
        public double Dist2 { get; set; }
        public double Thick { get; set; }
        public double Size { get; set; }
        public string Result { get; set; }
        public userPixThick()
        {
            this.Row = 0;
            this.Col = 0;
            this.Color = enColor.green;
            this.CamParams = null;
            this.Size = 15;
            this.Result = "OK";
        }
        public userPixThick(double row, double col)
        {
            this.Row = row;
            this.Col = col;
            this.Color = enColor.green;
            this.CamParams = null;
            this.Size = 15;
            this.Result = "OK";
        }

        public userPixThick(double row, double col, CameraParam CamParams)
        {
            this.Row = row;
            this.Col = col;
            this.Color = enColor.green;
            this.CamParams = CamParams;
            this.Size = 15;
            this.Result = "OK";
        }


        public userWcsThick GetWcsThick(double ref_x, double ref_y, double ref_z = 0)
        {
            userWcsThick wcsThick;
            double Qx = 0, Qy = 0, Qz = 0;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
            wcsThick = new userWcsThick(Qx, Qy, 0.0, this.CamParams);
            wcsThick.Color = this.Color;
            wcsThick.Grab_x = ref_x;
            wcsThick.Grab_y = ref_y;
            wcsThick.Grab_z = ref_z;
            wcsThick.Grab_theta = Grab_theta;
            wcsThick.Grab_u = Grab_u;
            wcsThick.Grab_v = Grab_v;
            wcsThick.Dist1 = this.Dist1;
            wcsThick.Dist2 = this.Dist2;
            wcsThick.Thick = this.Thick;
            wcsThick.Result = this.Result;

            /////////////////////////////////////
            wcsThick.Tag = this.Tag;
            wcsThick.ViewWindow = this.ViewWindow;
            wcsThick.CamName = this.CamName;
            return wcsThick;
        }
        public userWcsThick GetWcsThick()
        {
            userWcsThick wcsThick;
            double Qx = 0, Qy = 0, Qz = 0;
            this.CamParams?.ImagePointsToWorldPlane(this.Row, this.Col, this.Grab_x, this.Grab_y, this.Grab_z, out Qx, out Qy, out Qz);
            wcsThick = new userWcsThick(Qx, Qy, 0.0, this.CamParams);
            wcsThick.Color = this.Color;
            wcsThick.Grab_x = this.Grab_x;
            wcsThick.Grab_y = this.Grab_y;
            wcsThick.Grab_z = this.Grab_z;
            wcsThick.Grab_theta = this.Grab_theta;
            wcsThick.Grab_u = this.Grab_u;
            wcsThick.Grab_v = this.Grab_v;
            wcsThick.Dist1 = this.Dist1;
            wcsThick.Dist2 = this.Dist2;
            wcsThick.Thick = this.Thick;
            wcsThick.Result = this.Result;
            /////////////////////////////////////
            wcsThick.Tag = this.Tag;
            wcsThick.ViewWindow = this.ViewWindow;
            wcsThick.CamName = this.CamName;
            return wcsThick;
        }
        public userPixThick AffineTransPixThick(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, ratate_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
            userPixThick pixPoint = new userPixThick(Qx[0].D, Qy[0].D, this.CamParams);
            /////////////////////////////////////
            pixPoint.Color = this.Color;
            pixPoint.Grab_x = this.Grab_x;
            pixPoint.Grab_y = this.Grab_y;
            pixPoint.Dist1 = this.Dist1;
            pixPoint.Dist2 = this.Dist2;
            pixPoint.Thick = this.Thick;
            pixPoint.Result = this.Result;
            pixPoint.Tag = this.Tag;
            pixPoint.ViewWindow = this.ViewWindow;
            pixPoint.CamName = this.CamName;
            return pixPoint;
        }
        public HXLDCont GetXLD(double size = 15)
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCrossContourXld(this.Row, this.Col, this.Size, 0);
            return xld;
        }

    }
    #endregion


    #region 世界类型结构
    [Serializable]
    public class WcsData
    {
        public CameraParam CamParams { get; set; }

        public double Grab_x { get; set; }
        public double Grab_y { get; set; }
        public double Grab_z { get; set; }
        public double Grab_theta { get; set; }
        public double Grab_u { get; set; }
        public double Grab_v { get; set; }
        public userWcsPoint[] EdgesPoint_xyz { get; set; }
        public enColor Color { get; set; }
        public object Tag { get; set; }
        public string ViewWindow { get; set; }
        public string CamName { get; set; }
    }
    [Serializable]
    public class userWcsScaleVector : WcsData // 包含三种情况，不缩放，各向同向缩放，各向异向缩放
    {
        public double Y { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public double Deg { get; set; }
        public double Sr { get; set; }
        public double Sc { get; set; }



        public userWcsScaleVector()
        {
            this.Y = 0;
            this.X = 0;
            this.Z = 0;
            this.Deg = 0;
            this.Sr = 0;
            this.Sc = 0;
            Grab_x = 0;
            Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsScaleVector(double x, double y, double z, double deg, double Sr, double Sc)
        {
            this.Y = y;
            this.X = x;
            this.Z = z;
            this.Deg = deg;
            this.Sr = Sr;
            this.Sc = Sc;
            Grab_x = 0;
            Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }

        public userWcsScaleVector(double x, double y, double z, double deg, double Sr, double Sc, CameraParam CamParams)
        {
            this.Y = y;
            this.X = x;
            this.Z = z;
            this.Deg = deg;
            this.Sr = Sr;
            this.Sc = Sc;
            Grab_x = 0;
            Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixScaleVector GetPixScaleVector()
        {
            userPixScaleVector ScaleVector;
            double row, col, rad;
            rad = new HTuple(this.Deg).TupleRad().D;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out row, out col);
            ScaleVector = new userPixScaleVector(row, col, rad, this.Sr, this.Sc, this.CamParams);
            ScaleVector.Color = this.Color;
            ScaleVector.Grab_x = this.Grab_x;
            ScaleVector.Grab_y = this.Grab_y;
            return ScaleVector;
        }
        public override string ToString()
        {
            return string.Join(",", "userWcsScaleVector", this.X, this.Y, this.Z, this.Deg, this.Sr, this.Sc);
            //return string.Format("userWcsScaleVector: x={0},y={1},z={2},deg={3},Sr={4},Sc={5}", this.X, this.Y, this.Z, this.Deg, this.Sr, this.Sc);
        }
    }

    [Serializable]
    public class userWcsRectangle1 : WcsData
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double Z1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Z2 { get; set; }


        public userWcsRectangle1()
        {

        }
        public userWcsRectangle1(CameraParam CamParams)
        {
            this.X1 = 0;
            this.Y1 = 0;
            this.Z1 = 0;
            this.X2 = 0;
            this.Y2 = 0;
            this.Z2 = 0;
            Grab_x = 0;
            Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsRectangle1(double left_x, double left_y, double left_z, double right_x, double right_y, double right_z, HTuple camParam, HTuple camPose)
        {
            this.X1 = left_x;
            this.Y1 = left_y;
            this.Z1 = left_z;
            this.X2 = right_x;
            this.Y2 = right_y;
            this.Z2 = right_z;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsRectangle1(double left_x, double left_y, double left_z, double right_x, double right_y, double right_z)
        {
            this.X1 = left_x;
            this.Y1 = left_y;
            this.Z1 = left_z;
            this.X2 = right_x;
            this.Y2 = right_y;
            this.Z2 = right_z;
            Grab_x = 0;
            Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsRectangle1(double left_x, double left_y, double left_z, double right_x, double right_y, double right_z, CameraParam CamParams)
        {
            this.X1 = left_x;
            this.Y1 = left_y;
            this.Z1 = left_z;
            this.X2 = right_x;
            this.Y2 = right_y;
            this.Z2 = right_z;
            Grab_x = 0;
            Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixRectangle1 GetPixRectangle1()
        {
            userPixRectangle1 pixRect1;
            double row1, col1, row2, col2;
            this.CamParams.WorldPointsToImagePlane(this.X1, this.Y1, this.Grab_x, this.Grab_y, out row1, out col1);
            this.CamParams.WorldPointsToImagePlane(this.X2, this.Y2, this.Grab_x, this.Grab_y, out row2, out col2);
            pixRect1 = new userPixRectangle1(row1, col1, row2, col2, this.CamParams);
            pixRect1.Color = this.Color;
            pixRect1.Grab_x = this.Grab_x;
            pixRect1.Grab_y = this.Grab_y;
            if (this.EdgesPoint_xyz != null)
            {
                pixRect1.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixRect1.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }

            return pixRect1;
        }
        public userPixRectangle1 GetPixRectangle1FromWcs3D()
        {
            userPixRectangle1 pixRect1;
            double row1, col1, row2, col2;
            this.CamParams.WorldPointsToImagePlane(this.X1, this.Y1, 0, 0, out row1, out col1);
            this.CamParams.WorldPointsToImagePlane(this.X2, this.Y2, 0, 0, out row2, out col2);
            pixRect1 = new userPixRectangle1(row1, col1, row2, col2, this.CamParams);
            pixRect1.Color = this.Color;
            pixRect1.Grab_x = 0;
            pixRect1.Grab_y = 0;
            if (this.EdgesPoint_xyz != null)
            {
                pixRect1.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixRect1.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixRect1;
        }

        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenRectangle2ContourXld((Y2 + Y1) / 2.0 * -1, (X1 + X2) / 2.0, 0, (X2 - X1) / 2.0, (Y2 - Y1) / 2.0);
            return xld;
        }
        public override string ToString()
        {
            return string.Join(",", "userWcsRectangle1", this.X1, this.Y1, this.Z1, this.X2, this.Y2, this.Z2);
            //return string.Format("userWcsRectangle1: left_x={0},left_y={1},left_z={2},right_x={3},right_y={4},right_z={5}", this.X1, this.Y1, this.Z1, this.X2, this.Y2, this.Z2);
        }
        private void AffinePoint3D(userWcsRectangle1 rect1, HTuple transPose3D, out userWcsRectangle1 rect)
        {
            HTuple Qx = new HTuple(0, 0);
            HTuple Qy = new HTuple(0, 0);
            HTuple Qz = new HTuple(0, 0);
            HTuple X = new HTuple(rect1.X1, rect1.X2);
            HTuple Y = new HTuple(rect1.Y1, rect1.Y2);
            HTuple Z = new HTuple(rect1.Z1, rect1.Z2);
            HTuple homMat3D = null;
            if (transPose3D != null && transPose3D.Length == 7)
                HOperatorSet.PoseToHomMat3d(transPose3D, out homMat3D);
            else
                HOperatorSet.HomMat3dIdentity(out homMat3D);
            if (X != null && X.Length > 0)
            {
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
            }
            rect = new userWcsRectangle1(Qx[0].D, Qy[0].D, rect1.Z1, Qx[1].D, Qy[1].D, rect1.Z2);
        }

    }

    [Serializable]
    public class userWcsRectangle2 : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Deg { get; set; }
        public double Length1 { get; set; }
        public double Length2 { get; set; }
        public double Rectangularity { get; set; }


        // 用于绘图变量
        public double DiffRadius;
        public double[] NormalPhi;



        public userWcsRectangle2()
        {

        }
        public userWcsRectangle2(CameraParam camParam)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Deg = 0;
            this.Length1 = 0;
            this.Length2 = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }
        public userWcsRectangle2(HTuple camParam, HTuple camPose)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Deg = 0;
            this.Length1 = 0;
            this.Length2 = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsRectangle2(double center_x, double center_y, double center_z, double deg, double length1, double length2, double ref_x, double ref_y, HTuple camParam, HTuple camPose)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Deg = deg;
            this.Length1 = length1;
            this.Length2 = length2;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsRectangle2(double center_x, double center_y, double center_z, double deg, double length1, double length2, double ref_x, double ref_y, CameraParam camParam)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Deg = deg;
            this.Length1 = length1;
            this.Length2 = length2;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }
        public userWcsRectangle2(double center_x, double center_y, double center_z, double deg, double length1, double length2, HTuple camParam, HTuple camPose)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Deg = deg;
            this.Length1 = length1;
            this.Length2 = length2;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsRectangle2(double center_x, double center_y, double center_z, double deg, double length1, double length2)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Deg = deg;
            this.Length1 = length1;
            this.Length2 = length2;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsRectangle2(double center_x, double center_y, double center_z, double deg, double length1, double length2, CameraParam CamParams)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Deg = deg;
            this.Length1 = length1;
            this.Length2 = length2;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixRectangle2 GetPixRectangle2()
        {
            double row, col, pixLength1, pixLength2, rad;
            rad = new HTuple(this.Deg).TupleRad().D;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out row, out col);
            pixLength1 = this.CamParams.TransWcsLengthToPixLength(this.Length1);
            pixLength2 = this.CamParams.TransWcsLengthToPixLength(this.Length2);
            userPixRectangle2 pixRect2 = new userPixRectangle2(row, col, rad, pixLength1, pixLength2, this.CamParams);
            pixRect2.DiffRadius = this.DiffRadius;
            pixRect2.Grab_x = this.Grab_x;
            pixRect2.Grab_y = this.Grab_y;
            pixRect2.Color = this.Color;
            if (this.EdgesPoint_xyz != null)
            {
                pixRect2.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixRect2.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixRect2;
        }
        public userPixRectangle2 GetPixRectangle2From3D()
        {
            double row, col, pixLength1, pixLength2, rad;
            rad = new HTuple(this.Deg).TupleRad().D;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out row, out col);
            pixLength1 = this.CamParams.TransWcsLengthToPixLength(this.Length1);
            pixLength2 = this.CamParams.TransWcsLengthToPixLength(this.Length2);
            userPixRectangle2 pixRect2 = new userPixRectangle2(row, col, rad, pixLength1, pixLength2, this.CamParams);
            pixRect2.DiffRadius = this.DiffRadius;
            pixRect2.Color = this.Color;
            pixRect2.Grab_x = 0;
            pixRect2.Grab_y = 0;
            pixRect2.Color = this.Color;
            if (this.EdgesPoint_xyz != null)
            {
                pixRect2.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixRect2.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixRect2;
        }
        public userWcsRectangle2 AffineWcsRectangle2(userWcsPose wcsPose)
        {
            userWcsRectangle2 wcsRect2;
            HTuple Qx = new HTuple(0);
            HTuple Qy = new HTuple(0);
            HTuple Qz = new HTuple(0);
            HTuple X = new HTuple(this.X);
            HTuple Y = new HTuple(this.Y);
            HTuple Z = new HTuple(this.Z);
            HTuple homMat3D = null;
            HOperatorSet.PoseToHomMat3d(wcsPose.GetHtuple(), out homMat3D);
            if (X != null && X.Length > 0)
            {
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
            }
            wcsRect2 = new userWcsRectangle2(Qx[0].D, Qy[0].D, Qz[0], this.Deg + wcsPose.Rz, this.Length1, this.Length2);
            wcsRect2.DiffRadius = this.DiffRadius;
            return wcsRect2;
        }
        public userWcsRectangle2 AffineWcsRectangle2(userWcsCoordSystem coordSystem)
        {
            return AffineWcsRectangle2(coordSystem.GetUserWcsPose3D());
        }
        public userWcsRectangle2 AffineWcsRectangle2D(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, x, y;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            userWcsRectangle2 wcsRect2 = this.Clone();
            // 变换中心点
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out x, out y);
            // 变换角度
            double sx, sy, phi, theta, tx, ty;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, new HTuple(this.Deg).TupleRad() + Phi);
            sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            ////////////////////////////////////////////////////////
            wcsRect2 = new userWcsRectangle2(x[0].D, y[0].D, this.Z, new HTuple(phi).TupleDeg().D, this.Length1, this.Length2, this.Grab_x, this.Grab_y, this.CamParams);
            wcsRect2.DiffRadius = this.DiffRadius;
            wcsRect2.NormalPhi = this.NormalPhi;
            wcsRect2.ViewWindow = this.ViewWindow;
            wcsRect2.Tag = this.Tag;
            wcsRect2.CamName = this.CamName;
            wcsRect2.Grab_x = this.Grab_x;
            wcsRect2.Grab_y = this.Grab_y;
            wcsRect2.Grab_theta = this.Grab_theta;
            return wcsRect2;
        }
        public userWcsRectangle2 Affine2DWcsRectangle2(HTuple homMat2D, enAffineTransOrientation AffineTransOrientation)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsRectangle2 wcsRect2 = this.Clone();
            switch (AffineTransOrientation)
            {
                case enAffineTransOrientation.正向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
                    wcsRect2 = new userWcsRectangle2(Qx[0].D, Qy[0].D, this.Z, this.Deg + Phi.D, this.Length1, this.Length2, 0, 0, this.CamParams);
                    //wcsRect2 = new userWcsRectangle2(Qx[0].D, Qy[0].D, this.z, this.deg + Phi.D, this.length1, this.length2, Qx[1].D, Qy[1].D, this.CamParams);
                    break;  // 这里不能变换拍照位置
                ///////////////
                case enAffineTransOrientation.反向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
                    start_phi = this.Deg + Phi.D >= 0 ? this.Deg + Phi.D : this.Deg + Phi.D + Math.PI * 2;
                    wcsRect2 = new userWcsRectangle2(Qx[0].D, Qy[0].D, this.Z, start_phi, this.Length1, this.Length2, 0, 0, this.CamParams);
                    //wcsRect2 = new userWcsRectangle2(Qx[0].D, Qy[0].D, this.z, start_phi, this.length1, this.length2, Qx[1].D, Qy[1].D, this.CamParams);
                    break;
                default:
                    return this;
            }
            wcsRect2.DiffRadius = this.DiffRadius;
            wcsRect2.NormalPhi = this.NormalPhi;
            wcsRect2.ViewWindow = this.ViewWindow;
            wcsRect2.Tag = this.Tag;
            wcsRect2.CamName = this.CamName;
            wcsRect2.Grab_x = this.Grab_x;
            wcsRect2.Grab_y = this.Grab_y;
            wcsRect2.Grab_theta = this.Grab_theta;
            return wcsRect2;
        }

        public double GetArea()
        {
            return this.Length1 * this.Length2;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenRectangle2ContourXld(this.Y * -1, this.X, this.Deg * Math.PI / 180, Length1, Length2);
            return xld;
        }
        public override string ToString()
        {
            return string.Join(",", "userWcsRectangle2", this.X, this.Y, this.Z, this.Deg, this.Length1, this.Length2);
            //return string.Format("userWcsRectangle2: x={0},y={1},z={2},deg={3},length1={4},length2={5}", this.X, this.Y, this.Z, this.Deg, this.Length1, this.Length2);
        }
        public userWcsRectangle2 Clone()
        {
            userWcsRectangle2 rectangle2 = new userWcsRectangle2();
            rectangle2.X = this.X;
            rectangle2.Y = this.Y;
            rectangle2.Z = this.Z;
            rectangle2.Deg = this.Deg;
            rectangle2.Length1 = this.Length1;
            rectangle2.Length2 = this.Length2;
            rectangle2.Grab_x = this.Grab_x;
            rectangle2.Grab_y = this.Grab_y;
            rectangle2.Grab_theta = this.Grab_theta;
            rectangle2.EdgesPoint_xyz = this.EdgesPoint_xyz;
            /////////////
            rectangle2.DiffRadius = this.DiffRadius;
            rectangle2.NormalPhi = this.NormalPhi;
            rectangle2.Color = this.Color;
            rectangle2.CamName = this.CamName;
            rectangle2.ViewWindow = this.ViewWindow;
            rectangle2.Tag = this.Tag;
            return rectangle2;
        }
    }

    [Serializable]
    public class userWcsCircle : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Radius { get; set; }
        public double Start_deg { get; set; }
        public double End_deg { get; set; }
        public string PointOrder { get; set; }
        public string Circularity { get; set; }


        // 扩展属性，用于绘图用
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }


        public userWcsCircle()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius = 0;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = new userWcsPoint[0];
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            //this.CamParams = new CameraParam();
        }
        public userWcsCircle(CameraParam camParam)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius = 0;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }
        public userWcsCircle(HTuple camParam, HTuple camPose)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius = 0;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircle(double center_x, double center_y, double center_z, double radius, double ref_x, double ref_y, HTuple camParam, HTuple camPose)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Radius = radius;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircle(double center_x, double center_y, double center_z, double radius, HTuple camParam, HTuple camPose)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Radius = radius;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircle(double center_x, double center_y, double center_z, double radius, double start_deg, double end_deg, string pointOrder, HTuple camParam, HTuple camPose)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircle(double center_x, double center_y, double center_z, double radius, double ref_x, double ref_y, CameraParam CamParams)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Radius = radius;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsCircle(double center_x, double center_y, double center_z, double radius)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Radius = radius;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircle(double center_x, double center_y, double center_z, double radius, CameraParam CamParams)
        {
            this.X = center_x;
            this.Y = center_y;
            this.Z = center_z;
            this.Radius = radius;
            this.Start_deg = 0;
            this.End_deg = 360;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = new userWcsPoint[0];
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixCircle GetPixCircle()
        {
            double row, col, start_rad, end_rad, pixRadius;
            start_rad = new HTuple(this.Start_deg).TupleRad();
            end_rad = new HTuple(this.End_deg).TupleRad();
            if (this.CamParams == null) throw new ArgumentNullException("CamParams");
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out row, out col);
            pixRadius = this.CamParams.TransWcsLengthToPixLength(this.Radius);
            userPixCircle pixCircle = new userPixCircle(row, col, pixRadius, start_rad, end_rad, this.PointOrder, this.CamParams);
            pixCircle.DiffRadius = this.DiffRadius;
            pixCircle.Color = this.Color;
            pixCircle.PointOrder = this.PointOrder;
            pixCircle.Grab_x = this.Grab_x;
            pixCircle.Grab_y = this.Grab_y;
            ////
            if (this.EdgesPoint_xyz != null && this.EdgesPoint_xyz.Length > 0)
            {
                pixCircle.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixCircle.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixCircle;
        }
        public userPixCircle GetPixCircleFromWcs3D()
        {
            double row = 0, col = 0, start_rad = 0, end_rad = 0, pixRadius = 0;
            start_rad = new HTuple(this.Start_deg).TupleRad();
            end_rad = new HTuple(this.End_deg).TupleRad();
            this.CamParams?.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out row, out col);
            pixRadius = this.CamParams.TransWcsLengthToPixLength(this.Radius);
            userPixCircle pixCircle = new userPixCircle(row, col, pixRadius, start_rad, end_rad, this.PointOrder, this.CamParams);
            pixCircle.DiffRadius = this.DiffRadius;
            pixCircle.Color = this.Color;
            pixCircle.PointOrder = this.PointOrder;
            pixCircle.Grab_x = this.Grab_x;
            pixCircle.Grab_y = this.Grab_y;
            ////
            if (this.EdgesPoint_xyz != null && this.EdgesPoint_xyz.Length > 0)
            {
                pixCircle.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixCircle.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPointFromWcs3D();
                }
            }
            return pixCircle;
        }
        public userWcsPoint[] GetFitWcsPoint()
        {
            userWcsPoint[] wcsPoint = new userWcsPoint[0];
            if (this.EdgesPoint_xyz != null)
            {
                wcsPoint = new userWcsPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < wcsPoint.Length; i++)
                {
                    wcsPoint[i] = new userWcsPoint();
                    double phi, rowPoint, colPoint;
                    phi = Math.Atan2(this.EdgesPoint_xyz[i].Y - this.Y, this.EdgesPoint_xyz[i].X - this.X) * -1;
                    HMisc.GetPointsEllipse(phi, this.Y, this.X, 0, this.Radius, this.Radius, out rowPoint, out colPoint);
                    wcsPoint[i].X = colPoint;
                    wcsPoint[i].Y = rowPoint;
                    wcsPoint[i].Z = this.EdgesPoint_xyz[i].Z;
                    wcsPoint[i].Grab_x = Grab_x;
                    wcsPoint[i].Grab_y = Grab_y;
                    wcsPoint[i].Grab_z = Grab_z;
                    wcsPoint[i].Grab_theta = this.Grab_theta;
                    wcsPoint[i].Grab_u = this.Grab_u;
                    wcsPoint[i].Grab_v = this.Grab_v;
                    /////////////////////////////////////
                    wcsPoint[i].Tag = this.Tag;
                    wcsPoint[i].ViewWindow = this.ViewWindow;
                    wcsPoint[i].CamName = this.CamName;
                    wcsPoint[i].CamParams = this.CamParams;
                }
            }
            return wcsPoint;
        }
        public userWcsCircle AffineWcsCircle(userWcsPose wcsPose)
        {
            userWcsCircle wcsCircle;
            HTuple Qx = new HTuple(0);
            HTuple Qy = new HTuple(0);
            HTuple Qz = new HTuple(0);
            HTuple X = new HTuple(this.X);
            HTuple Y = new HTuple(this.Y);
            HTuple Z = new HTuple(this.Z);
            HTuple homMat3D = null;
            HOperatorSet.PoseToHomMat3d(wcsPose.GetHtuple(), out homMat3D);
            if (X != null && X.Length > 0)
            {
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
            }
            wcsCircle = new userWcsCircle(Qx[0].D, Qy[0].D, Qz[0], this.Radius);
            wcsCircle.DiffRadius = this.DiffRadius;
            wcsCircle.PointOrder = this.PointOrder;
            //////////////////
            wcsCircle.Tag = this.Tag;
            wcsCircle.ViewWindow = this.ViewWindow;
            wcsCircle.CamName = this.CamName;
            wcsCircle.Grab_x = this.Grab_x;
            wcsCircle.Grab_y = this.Grab_y;
            wcsCircle.Grab_theta = this.Grab_theta;
            return wcsCircle;
        }
        public userWcsCircle AffineWcsCircle(userWcsCoordSystem coordSystem)
        {
            return AffineWcsCircle(coordSystem.GetUserWcsPose3D());
        }
        public userWcsCircle AffineWcsCircle2D(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsCircle wcsCircle;
            // 变换角度
            double sx, sy, phi, theta, tx, ty;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, new HTuple(this.Start_deg).TupleRad() + Phi);
            sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            start_phi = phi;
            // 变换终止角度;
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, new HTuple(this.End_deg).TupleRad() + Phi);
            sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            end_phi = phi;
            // 变换圆心点
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);
            ////////////////////////////////
            wcsCircle = new userWcsCircle(Qx[0].D, Qy[0].D, this.Z, this.Radius, Qx[1].D, Qy[1].D, this.CamParams);
            wcsCircle.Start_deg = start_phi.TupleDeg().D;
            wcsCircle.End_deg = end_phi.TupleDeg().D;
            wcsCircle.DiffRadius = this.DiffRadius;
            wcsCircle.NormalPhi = this.NormalPhi;
            wcsCircle.PointOrder = this.PointOrder;
            //////////////////
            wcsCircle.Tag = this.Tag;
            wcsCircle.ViewWindow = this.ViewWindow;
            wcsCircle.CamName = this.CamName;
            wcsCircle.Grab_x = this.Grab_x;
            wcsCircle.Grab_y = this.Grab_y;
            wcsCircle.Grab_theta = this.Grab_theta;
            return wcsCircle;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCircleContourXld(this.Y * -1, this.X, this.Radius, 0, Math.PI * 2, this.PointOrder, 0.1); // "positive"
            return xld;
        }

        public override string ToString()
        {
            return string.Join(",", "userWcsCircle", this.X, this.Y, this.Z, this.Radius);
            //return string.Format("userWcsCircle: x={0},y={1},z={2},radius={3}", this.X, this.Y, this.Z, this.Radius);
        }

    }
    [Serializable]
    public class userWcsCircleSector : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Radius { get; set; }
        public double Start_deg { get; set; }
        public double End_deg { get; set; }
        public string PointOrder { get; set; }
        // 绘图变量
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }


        public userWcsCircleSector()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius = 0;
            this.Start_deg = 0;
            this.End_deg = 0;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = new userWcsPoint[0];
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            //this.CamParams = new CameraParam();
        }
        public userWcsCircleSector(CameraParam camParam)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius = 0;
            this.Start_deg = 0;
            this.End_deg = 0;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }
        public userWcsCircleSector(HTuple camParam, HTuple camPose)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius = 0;
            this.Start_deg = 0;
            this.End_deg = 0;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircleSector(double x, double y, double z, double radius, double start_deg, double end_deg, string pointOrder, double ref_x, double ref_y, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircleSector(double x, double y, double z, double radius, double start_deg, double end_deg, double ref_x, double ref_y, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircleSector(double x, double y, double z, double radius, double start_deg, double end_deg, double ref_x, double ref_y, CameraParam camParam)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }
        public userWcsCircleSector(double x, double y, double z, double radius, double start_deg, double end_deg, string pointOrder, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircleSector(double x, double y, double z, double radius, double start_deg, double end_deg, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircleSector(double x, double y, double z, double radius, double start_deg, double end_deg)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsCircleSector(double x, double y, double z, double radius, double start_deg, double end_deg, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius = radius;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsCircle GetWcsCircle()
        {
            userWcsCircle wcsCircle = new userWcsCircle();
            wcsCircle.X = this.X;
            wcsCircle.Y = this.Y;
            wcsCircle.Z = this.Z;
            wcsCircle.Radius = this.Radius;
            wcsCircle.Grab_x = this.Grab_x;
            wcsCircle.Grab_y = this.Grab_y;
            wcsCircle.CamParams = this.CamParams;
            wcsCircle.EdgesPoint_xyz = this.EdgesPoint_xyz;
            wcsCircle.DiffRadius = this.DiffRadius;
            wcsCircle.Start_deg = this.Start_deg;
            wcsCircle.End_deg = this.End_deg;
            wcsCircle.PointOrder = this.PointOrder;
            return wcsCircle;
        }
        public userPixCircleSector GetPixCircleSector()
        {
            double row, col, star_rad, end_rad, pixRadius;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out row, out col);
            star_rad = new HTuple(this.Start_deg).TupleRad().D;
            end_rad = new HTuple(this.End_deg).TupleRad().D;
            pixRadius = this.CamParams.TransWcsLengthToPixLength(this.Radius);
            userPixCircleSector pixCircleSector = new userPixCircleSector(row, col, pixRadius, star_rad, end_rad, this.CamParams);
            pixCircleSector.DiffRadius = this.DiffRadius;
            pixCircleSector.Color = this.Color;
            pixCircleSector.PointOrder = this.PointOrder;
            pixCircleSector.Grab_x = this.Grab_x;
            pixCircleSector.Grab_y = this.Grab_y;
            if (this.EdgesPoint_xyz != null)
            {
                pixCircleSector.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixCircleSector.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixCircleSector;
        }
        public userWcsPoint[] GetFitWcsPoint()
        {
            userWcsPoint[] wcsPoint = new userWcsPoint[0];
            if (this.EdgesPoint_xyz != null)
            {
                wcsPoint = new userWcsPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < wcsPoint.Length; i++)
                {
                    wcsPoint[i] = new userWcsPoint();
                    double phi, rowPoint, colPoint;
                    phi = Math.Atan2(this.EdgesPoint_xyz[i].Y - this.Y, this.EdgesPoint_xyz[i].X - this.X) * -1;
                    HMisc.GetPointsEllipse(phi, this.Y, this.X, 0, this.Radius, this.Radius, out rowPoint, out colPoint);
                    wcsPoint[i].X = colPoint;
                    wcsPoint[i].Y = rowPoint;
                    wcsPoint[i].Z = this.EdgesPoint_xyz[i].Z;
                    wcsPoint[i].Grab_x = Grab_x;
                    wcsPoint[i].Grab_y = Grab_y;
                    wcsPoint[i].Grab_z = Grab_z;
                    wcsPoint[i].Grab_theta = this.Grab_theta;
                    wcsPoint[i].Grab_u = this.Grab_u;
                    wcsPoint[i].Grab_v = this.Grab_v;
                    /////////////////////////////////////
                    wcsPoint[i].Tag = this.Tag;
                    wcsPoint[i].ViewWindow = this.ViewWindow;
                    wcsPoint[i].CamName = this.CamName;
                    wcsPoint[i].CamParams = this.CamParams;
                }
            }
            return wcsPoint;
        }
        public userPixCircleSector GetPixCircleSectorFrom3D() // 不包含参考点
        {
            double row, col, star_rad, end_rad, pixRadius;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out row, out col);
            star_rad = new HTuple(this.Start_deg).TupleRad().D;
            end_rad = new HTuple(this.End_deg).TupleRad().D;
            pixRadius = this.CamParams.TransWcsLengthToPixLength(this.Radius);
            userPixCircleSector pixCircleSector = new userPixCircleSector(row, col, pixRadius, star_rad, end_rad, this.CamParams);
            pixCircleSector.DiffRadius = this.DiffRadius;
            pixCircleSector.Color = this.Color;
            pixCircleSector.PointOrder = this.PointOrder;
            pixCircleSector.Grab_x = 0;
            pixCircleSector.Grab_y = 0;
            if (this.EdgesPoint_xyz != null)
            {
                pixCircleSector.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixCircleSector.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixCircleSector;
        }



        public userWcsCircleSector Affine2DWcsCircleSector(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsCircleSector wcsCircleSector;
            // 变换角度
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D homMat2dRotate;
            // 变换起始角度
            homMat2dRotate = hHomMat2D.HomMat2dRotate(new HTuple(this.Start_deg).TupleRad() + Phi, 0, 0);
            Qx = homMat2dRotate.AffineTransPoint2d(new HTuple(0, 1), new HTuple(0, 0), out Qy);
            //start_phi = Math.Atan2(Qx[1] * -1, Qy[1]);
            start_phi = Math.Atan2(Qy[1], Qx[1]);
            // 变换终止角度
            homMat2dRotate = hHomMat2D.HomMat2dRotate(new HTuple(this.End_deg).TupleRad() + Phi, 0, 0);
            Qx = homMat2dRotate.AffineTransPoint2d(new HTuple(0, 1), new HTuple(0, 0), out Qy);
            end_phi = Math.Atan2(Qy[1], Qx[1]);
            // 变换圆心点
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
            ////////////////////////////////
            wcsCircleSector = new userWcsCircleSector(Qx[0].D, Qy[0].D, this.Z, this.Radius, start_phi.TupleDeg().D, end_phi.TupleDeg().D, this.Grab_x, this.Grab_y, this.CamParams);

            wcsCircleSector.DiffRadius = this.DiffRadius;
            wcsCircleSector.NormalPhi = this.NormalPhi;
            wcsCircleSector.PointOrder = this.PointOrder;
            //////////////////
            wcsCircleSector.Tag = this.Tag;
            wcsCircleSector.ViewWindow = this.ViewWindow;
            wcsCircleSector.CamName = this.CamName;
            wcsCircleSector.Grab_x = this.Grab_x;
            wcsCircleSector.Grab_y = this.Grab_y;
            wcsCircleSector.Grab_theta = this.Grab_theta;
            return wcsCircleSector;
        }

        public userWcsCircleSector Affine2DWcsCircleSector(HTuple homMat2D, enAffineTransOrientation AffineTransOrientation)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsCircleSector wcsCircleSector;
            switch (AffineTransOrientation)
            {
                case enAffineTransOrientation.正向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);
                    //wcsCircleSector = new userWcsCircleSector(Qx[0].D, Qy[0].D, this.z, this.radius, this.start_deg + Phi.D, this.end_deg + Phi.D, this.grab_x, this.grab_y, this.CamParams);
                    wcsCircleSector = new userWcsCircleSector(Qx[0].D, Qy[0].D, this.Z, this.Radius, this.Start_deg + Phi.D, this.End_deg + Phi.D, Qx[1].D, Qy[1].D, this.CamParams);
                    break;
                ///////////////
                case enAffineTransOrientation.反向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);
                    start_phi = this.Start_deg + Phi.D >= 0 ? this.Start_deg + Phi.D : this.Start_deg + Phi.D + Math.PI * 2;
                    end_phi = this.End_deg + Phi.D >= 0 ? this.End_deg + Phi.D : this.End_deg - Phi.D + Math.PI * 2;
                    //wcsCircleSector = new userWcsCircleSector(Qx[0].D, Qy[0].D, this.z, this.radius, start_phi, end_phi, this.grab_x, this.grab_y, this.CamParams);
                    wcsCircleSector = new userWcsCircleSector(Qx[0].D, Qy[0].D, this.Z, this.Radius, start_phi, end_phi, Qx[1].D, Qy[1].D, this.CamParams);
                    break;
                default:
                    return this;
            }
            wcsCircleSector.DiffRadius = this.DiffRadius;
            wcsCircleSector.NormalPhi = this.NormalPhi;
            wcsCircleSector.PointOrder = this.PointOrder;
            return wcsCircleSector;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCircleContourXld(this.Y, this.X, this.Radius, this.Start_deg * Math.PI / 180, this.End_deg * Math.PI / 180, this.PointOrder, 0.01); //"positive"
            return xld;
        }
        public override string ToString()
        {
            return string.Join(",", "userWcsCircleSector", this.X, this.Y, this.Z, this.Radius, this.Start_deg, this.End_deg);
            //return string.Format("userWcsCircleSector: x={0},y={1},z={2},radius={3},start_deg={4},end_deg={5}", this.X, this.Y, this.Z, this.Radius, this.Start_deg, this.End_deg);
        }

    }
    [Serializable]
    public class userWcsLine : WcsData
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double Z1 { get; set; }
        public double U1 { get; set; }
        public double V1 { get; set; }
        public double Theta1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Z2 { get; set; }
        public double U2 { get; set; }
        public double V2 { get; set; }
        public double Theta2 { get; set; }

        public double Angle { get; set; }
        // 扩展属性
        public double DiffRadius { get; set; }
        public double NormalPhi { get; set; }


        public userWcsLine()
        {
            this.X1 = 0;
            this.Y1 = 0;
            this.Z1 = 0;
            this.X2 = 0;
            this.Y2 = 0;
            this.Z2 = 0;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = new userWcsPoint[0];
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = new CameraParam();
            this.ViewWindow = "NONE";
        }

        public userWcsLine(CameraParam cameraParam)
        {
            this.X1 = 0;
            this.Y1 = 0;
            this.Z1 = 0;
            this.X2 = 0;
            this.Y2 = 0;
            this.Z2 = 0;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = cameraParam;
            this.ViewWindow = "NONE";
        }
        public userWcsLine(double x1, double y1, double z1, double x2, double y2, double z2, double refPointX, double refPointY, CameraParam camParam)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Z1 = z1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Z2 = z2;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = refPointX;
            this.Grab_y = refPointY;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = camParam;
            this.ViewWindow = "NONE";
        }
        public userWcsLine(double x1, double y1, double z1, double x2, double y2, double z2, double refPointX, double refPointY, double refPointTheta, CameraParam camParam)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Z1 = z1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Z2 = z2;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = refPointX;
            this.Grab_y = refPointY;
            this.Grab_theta = refPointTheta;
            this.EdgesPoint_xyz = null;
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = camParam;
            this.ViewWindow = "NONE";
        }
        public userWcsLine(double x1, double y1, double z1, double x2, double y2, double z2, double refPointX, double refPointY, HTuple camParam, HTuple camPose)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Z1 = z1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Z2 = z2;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = refPointX;
            this.Grab_y = refPointY;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = null;
            this.ViewWindow = "NONE";
        }
        public userWcsLine(double x1, double y1, double z1, double x2, double y2, double z2, HTuple camParam, HTuple camPose)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Z1 = z1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Z2 = z2;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = null;
            this.ViewWindow = "NONE";
        }
        public userWcsLine(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Z1 = z1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Z2 = z2;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = null;
            this.ViewWindow = "NONE";
        }
        public userWcsLine(double x1, double y1, double z1, double x2, double y2, double z2, CameraParam cameraParam)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Z1 = z1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Z2 = z2;
            this.U1 = 0;
            this.V1 = 0;
            this.Theta1 = 0;
            this.U2 = 0;
            this.V2 = 0;
            this.Theta2 = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = 0;
            this.Color = enColor.green;
            this.CamParams = cameraParam;
        }
        public userPixLine GetPixLine()
        {
            userPixLine pixLine;
            double row1 = 0, col1 = 0, row2 = 0, col2 = 0;
            this.CamParams?.WorldPointsToImagePlane(this.X1, this.Y1, this.Grab_x, this.Grab_y, out row1, out col1);
            this.CamParams?.WorldPointsToImagePlane(this.X2, this.Y2, this.Grab_x, this.Grab_y, out row2, out col2);
            pixLine = new userPixLine(row1, col1, row2, col2, this.CamParams);
            pixLine.DiffRadius = this.DiffRadius;
            pixLine.NormalPhi = this.NormalPhi;
            pixLine.Color = this.Color;
            pixLine.Grab_x = this.Grab_x;
            pixLine.Grab_y = this.Grab_y;
            pixLine.CamName = this.CamName;
            if (this.EdgesPoint_xyz != null)
            {
                pixLine.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixLine.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixLine;
        }
        public userPixLine GetPixLineFromWcs3D()
        {
            userPixLine pixLine;
            double row1 = 0, col1 = 0, row2 = 0, col2 = 0;
            this.CamParams?.WorldPointsToImagePlane(this.X1, this.Y1, 0, 0, out row1, out col1);
            this.CamParams?.WorldPointsToImagePlane(this.X2, this.Y2, 0, 0, out row2, out col2);
            pixLine = new userPixLine(row1, col1, row2, col2, this.CamParams);
            pixLine.DiffRadius = this.DiffRadius;
            pixLine.NormalPhi = this.NormalPhi;
            pixLine.Color = this.Color;
            pixLine.Grab_x = this.Grab_x;
            pixLine.Grab_y = this.Grab_y;
            if (this.EdgesPoint_xyz != null)
            {
                pixLine.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixLine.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixLine;
        }
        public userWcsPoint[] GetFitWcsPoint()
        {
            userWcsPoint[] wcsPoint = new userWcsPoint[0];
            if (this.EdgesPoint_xyz != null)
            {
                wcsPoint = new userWcsPoint[2];
                for (int i = 0; i < wcsPoint.Length; i++)
                {
                    wcsPoint[i] = new userWcsPoint();
                    switch (i)
                    {
                        case 0:
                            wcsPoint[i].X = this.X1;
                            wcsPoint[i].Y = this.Y1;
                            wcsPoint[i].Z = this.Z1;
                            break;
                        case 1:
                            wcsPoint[i].X = this.X2;
                            wcsPoint[i].Y = this.Y2;
                            wcsPoint[i].Z = this.Z2;
                            break;
                    }
                    wcsPoint[i].Grab_x = Grab_x;
                    wcsPoint[i].Grab_y = Grab_y;
                    wcsPoint[i].Grab_z = Grab_z;
                    wcsPoint[i].Grab_theta = this.Grab_theta;
                    wcsPoint[i].Grab_u = this.Grab_u;
                    wcsPoint[i].Grab_v = this.Grab_v;
                    /////////////////////////////////////
                    wcsPoint[i].Tag = this.Tag;
                    wcsPoint[i].ViewWindow = this.ViewWindow;
                    wcsPoint[i].CamName = this.CamName;
                    wcsPoint[i].CamParams = this.CamParams;
                }
            }
            return wcsPoint;
        }

        public userWcsPoint[] GetInterpretationWcsPoint(int count)
        {
            userWcsPoint[] wcsPoint = new userWcsPoint[count];
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_z = new List<double>();
            double k = 0, b = 0, y = 0;
            int index = 0;
            double dist = HMisc.DistancePp(X1, Y1, X2, Y2);
            double InterpretationDist = dist / count;
            if (X2 < X1)
                InterpretationDist = Math.Abs(dist) * -1; // 如果终点小于起点，那么递增值要小于 0 ，否则要大于0
            else
                InterpretationDist = Math.Abs(dist) * 1;
            //////////////////////////////////////////////////////////////////////
            if (X1 != X2)
            {
                k = (Y2 - Y1) / (X2 - X1);
                b = Y1 - k * X1;
                while (true)
                {
                    if (Math.Abs(X1 + index * InterpretationDist) > Math.Abs(X2)) break;
                    y = k * (X1 + index * InterpretationDist) + b;
                    list_x.Add(X1 + index * InterpretationDist);
                    list_y.Add(y);
                    list_z.Add(Z1);
                    index++;
                }
            }
            else
            {
                while (true)
                {
                    if (Math.Abs(Y1 + index * InterpretationDist) > Math.Abs(Y2)) break;
                    y = Y1 + index * InterpretationDist;
                    list_x.Add(X1);
                    list_y.Add(y);
                    list_z.Add(Z1);
                    index++;
                }
            }
            /////////////////////////////////////////
            list_x.Add(X2);
            list_y.Add(Y2);
            list_z.Add(Z2);
            ////////////////////////////////
            for (int i = 0; i < list_x.Count; i++)
            {
                wcsPoint[i].X = list_x[i];
                wcsPoint[i].Y = list_y[i];
                wcsPoint[i].Z = list_z[i];
                wcsPoint[i].Grab_x = Grab_x;
                wcsPoint[i].Grab_y = Grab_y;
                wcsPoint[i].Grab_z = Grab_z;
                wcsPoint[i].Grab_theta = this.Grab_theta;
                wcsPoint[i].Grab_u = this.Grab_u;
                wcsPoint[i].Grab_v = this.Grab_v;
                /////////////////////////////////////
                wcsPoint[i].Tag = this.Tag;
                wcsPoint[i].ViewWindow = this.ViewWindow;
                wcsPoint[i].CamName = this.CamName;
                wcsPoint[i].CamParams = this.CamParams;
            }
            list_x.Clear();
            list_y.Clear();
            list_z.Clear();
            return wcsPoint;
        }

        public userWcsLine AffineWcsLine(HTuple Pose)
        {
            userWcsLine wcsLine;
            AffinePoint3D(this, Pose, out wcsLine);
            wcsLine.DiffRadius = this.DiffRadius;
            wcsLine.NormalPhi = this.NormalPhi;
            return wcsLine;
        }
        public userWcsLine AffineWcsLine2D(HTuple homMat2D)
        {
            HTuple Qx, Qy;
            userWcsLine wcsLine;
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X1, this.X2, this.Grab_x), new HTuple(this.Y1, this.Y2, this.Grab_y), out Qx, out Qy);
            wcsLine = new userWcsLine(Qx[0].D, Qy[0].D, this.Z1, Qx[1], Qy[1], this.Z2, Qx[2].D, Qy[2].D, this.CamParams);
            wcsLine.DiffRadius = this.DiffRadius;
            wcsLine.NormalPhi = this.NormalPhi;
            wcsLine.CamName = this.CamName;
            wcsLine.ViewWindow = this.ViewWindow;
            wcsLine.Tag = this.Tag;
            wcsLine.Grab_x = this.Grab_x; // 拍照坐标是否需要跟着变换？
            wcsLine.Grab_y = this.Grab_y;
            wcsLine.Grab_theta = this.Grab_theta;
            return wcsLine;
        }

        public HXLDCont GetXLD()
        {
            return new HXLDCont(new HTuple(this.Y1 * 1, this.Y2 * 1), new HTuple(this.X1, this.X2));
        }
        public override string ToString()
        {
            return string.Join(",", "userWcsLine", this.X1, this.Y1, this.Z1, this.X2, this.Y2, this.Z2);
            //return string.Format("userWcsLine: x1={0},y1={1},z1={2},x2={3},y2={4},z2={5}", this.X1, this.Y1, this.Z1, this.X2, this.Y2, this.Z2);
        }

        private void AffinePoint3D(userWcsLine line1, HTuple transPose3D, out userWcsLine line)
        {
            HTuple Qx = new HTuple(0, 0);
            HTuple Qy = new HTuple(0, 0);
            HTuple Qz = new HTuple(0, 0);
            HTuple X = new HTuple(line1.X1, line1.X2);
            HTuple Y = new HTuple(line1.Y1, line1.Y2);
            HTuple Z = new HTuple(line1.Z1, line1.Z2);
            HTuple homMat3D = null;
            if (transPose3D != null && transPose3D.Length == 7)
                HOperatorSet.PoseToHomMat3d(transPose3D, out homMat3D);
            else
                HOperatorSet.HomMat3dIdentity(out homMat3D);
            if (X != null && X.Length > 0)
            {
                //if (Z == null || Z.Length != X.Length)
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
            }
            line = new userWcsLine(Qx[0].D, Qy[0].D, line1.Z1, Qx[1].D, Qy[1].D, line1.Z2); // 变换半径不会变
        }


        public userWcsLine Clone()
        {
            userWcsLine wcsLine = new userWcsLine();
            wcsLine.X1 = this.X1;
            wcsLine.Y1 = this.Y1;
            wcsLine.Z1 = this.Z1;
            wcsLine.X2 = this.X2;
            wcsLine.Y2 = this.Y2;
            wcsLine.Z2 = this.Z2;
            wcsLine.U1 = this.U1;
            wcsLine.V1 = this.V1;
            wcsLine.Theta1 = this.Theta1;
            wcsLine.U2 = this.U2;
            wcsLine.V2 = this.V2;
            wcsLine.Theta2 = this.Theta2;
            wcsLine.Grab_x = this.Grab_x;
            wcsLine.Grab_y = this.Grab_y;
            wcsLine.Grab_theta = this.Grab_theta;
            wcsLine.EdgesPoint_xyz = this.EdgesPoint_xyz;
            ////////////
            wcsLine.DiffRadius = this.DiffRadius;
            wcsLine.NormalPhi = this.NormalPhi;
            wcsLine.Color = this.Color;
            wcsLine.CamParams = this.CamParams;
            return wcsLine;
        }


    }

    [Serializable]
    public class userWcsPoint : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Theta { get; set; }
        public double U { get; set; }
        public double V { get; set; }

        // 专用于绘图使用
        public double DiffRadius { get; set; }


        public userWcsPoint()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 100;
            this.Grab_y = 100;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////////////////////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;

        }
        public userWcsPoint(CameraParam CamParam)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 100;
            this.Grab_y = 100;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ///
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = CamParam;
        }

        public userWcsPoint(double x, double y, double z, double ref_x, double ref_y, CameraParam camParam)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            //////////////////////////////////////////////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }
        public userWcsPoint(double x, double y, double z, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////////////////////////////////////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = new CameraParam();
        }
        public userWcsPoint(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ///
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsPoint(double x, double y, double z, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ///
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsPoint(double x, double y, double z, double u, double v, double theta)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = u;
            this.V = v;
            this.Theta = theta;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            ///////////////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userPixPoint GetPixPoint()
        {
            userPixPoint pixPoint;
            double row = 0, col = 0;
            this.CamParams?.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out row, out col);
            pixPoint = new userPixPoint(row, col, this.CamParams);
            pixPoint.DiffRadius = this.DiffRadius;
            pixPoint.Color = this.Color;
            return pixPoint;
        }
        public userPixPoint GetPixPointFromWcs3D()
        {
            userPixPoint pixPoint;
            double row, col;
            if (this.CamParams == null) return new userPixPoint();
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out row, out col);
            pixPoint = new userPixPoint(row, col, this.CamParams);
            pixPoint.DiffRadius = this.DiffRadius;
            pixPoint.Color = this.Color;
            return pixPoint;
        }
        public userWcsPoint AffineWcsPoint(HTuple homMat2D)
        {
            HTuple Qx, Qy;
            userWcsPoint wcsPoint;
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);
            wcsPoint = new userWcsPoint(Qx[0].D, Qy[0].D, this.Z, Qx[1], Qy[1], this.CamParams);
            wcsPoint.DiffRadius = this.DiffRadius;
            wcsPoint.CamName = this.CamName;
            wcsPoint.ViewWindow = this.ViewWindow;
            wcsPoint.Tag = this.Tag;
            wcsPoint.Grab_x = this.Grab_x;
            wcsPoint.Grab_y = this.Grab_y;
            wcsPoint.Grab_theta = this.Grab_theta;
            return wcsPoint;
        }

        /// <summary>
        /// 获取相机坐标系下的点
        /// </summary>
        /// <returns></returns>
        public userWcsPoint GetWcsCamPoint()
        {
            double cam_x, cam_y, cam_z;
            this.CamParams.TransWcsPointToCamPoint(this.X, this.Y, this.Grab_x, this.Grab_y, this.Grab_z, out cam_x, out cam_y, out cam_z);
            userWcsPoint wcsPoint = new userWcsPoint(cam_x, cam_y, cam_z, this.CamParams);
            wcsPoint.CamName = this.CamName;
            wcsPoint.ViewWindow = this.ViewWindow;
            wcsPoint.Tag = this.Tag;
            return wcsPoint;
        }
        public HXLDCont GetXLD(double size = 15)
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCrossContourXld(this.Y * 1, this.X, size, 0);
            return xld;
        }
        public override string ToString()
        {
            return string.Format(",", "userWcsPoint", this.X, this.Y, this.Z, this.Theta);
            //return string.Format("userWcsPoint: x={0},y={1},z={2},theta={3}", this.X, this.Y, this.Z, this.Theta);
        }
        public userWcsPoint Clone()
        {
            userWcsPoint wcsPoint = new userWcsPoint();
            wcsPoint.X = this.X;
            wcsPoint.Y = this.Y;
            wcsPoint.Z = this.Z;
            wcsPoint.U = this.U;
            wcsPoint.V = this.V;
            wcsPoint.Theta = this.Theta;
            wcsPoint.Grab_x = this.Grab_x;
            wcsPoint.Grab_y = this.Grab_y;
            wcsPoint.Grab_theta = this.Grab_theta;
            wcsPoint.EdgesPoint_xyz = this.EdgesPoint_xyz;
            wcsPoint.DiffRadius = this.DiffRadius;
            wcsPoint.Color = this.Color;
            wcsPoint.CamParams = this.CamParams;
            return wcsPoint;
        }
    }

    [Serializable]
    public class userWcsEllipse : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Radius1 { get; set; }
        public double Radius2 { get; set; }
        public double Deg { get; set; }
        public double Start_deg { get; set; }
        public double End_deg { get; set; }
        public string PointOrder { get; set; }

        // 专用于绘图使用
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }


        public userWcsEllipse()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Deg = 0;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = new userWcsPoint[0];
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
        }
        public userWcsEllipse(CameraParam camParam)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Deg = 0;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }

        public userWcsEllipse(HTuple camParam, HTuple camPose)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Deg = 0;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, string pointOrder, double ref_x, double ref_y, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }

        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, string pointOrder, double ref_x, double ref_y, CameraParam camParam)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, double ref_x, double ref_y, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, string pointOrder, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, string pointOrder)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsEllipse(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            /////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixEllipse GetPixEllipse()
        {
            double row, col, rad, start_rad, end_rad, pixRadius1, pixRadius2;
            rad = new HTuple(this.Deg).TupleRad().D;
            start_rad = new HTuple(this.Start_deg).TupleRad().D;
            end_rad = new HTuple(this.End_deg).TupleRad().D;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out row, out col);
            pixRadius1 = this.CamParams.TransWcsLengthToPixLength(this.Radius1);
            pixRadius2 = this.CamParams.TransWcsLengthToPixLength(this.Radius2);
            userPixEllipse pixEllipse = new userPixEllipse(row, col, rad, pixRadius1, pixRadius2, this.CamParams);
            pixEllipse.DiffRadius = this.DiffRadius;
            pixEllipse.Color = this.Color;
            pixEllipse.PointOrder = this.PointOrder;

            pixEllipse.Grab_x = this.Grab_x;
            pixEllipse.Grab_y = this.Grab_y;
            if (this.EdgesPoint_xyz != null)
            {
                pixEllipse.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixEllipse.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixEllipse;
        }
        public userPixEllipse GetPixEllipseFromWcs3D()
        {
            double row, col, rad, start_rad, end_rad, pixRadius1, pixRadius2;
            rad = new HTuple(this.Deg).TupleRad().D;
            start_rad = new HTuple(this.Start_deg).TupleRad().D;
            end_rad = new HTuple(this.End_deg).TupleRad().D;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out row, out col);
            pixRadius1 = this.CamParams.TransWcsLengthToPixLength(this.Radius1);
            pixRadius2 = this.CamParams.TransWcsLengthToPixLength(this.Radius2);
            userPixEllipse pixEllipse = new userPixEllipse(row, col, rad, pixRadius1, pixRadius2, this.CamParams);
            pixEllipse.DiffRadius = this.DiffRadius;
            pixEllipse.Color = this.Color;
            pixEllipse.PointOrder = this.PointOrder;

            pixEllipse.Grab_x = 0;
            pixEllipse.Grab_y = 0;
            if (this.EdgesPoint_xyz != null)
            {
                pixEllipse.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixEllipse.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixEllipse;
        }
        public userWcsPoint[] GetFitWcsPoint()
        {
            userWcsPoint[] wcsPoint = new userWcsPoint[0];
            if (this.EdgesPoint_xyz != null)
            {
                wcsPoint = new userWcsPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < wcsPoint.Length; i++)
                {
                    wcsPoint[i] = new userWcsPoint();
                    double rowPoint, colPoint;
                    double phi = Math.Atan2(this.EdgesPoint_xyz[i].Y - this.Y, this.EdgesPoint_xyz[i].X - this.X) * -1;
                    HMisc.GetPointsEllipse(phi, this.EdgesPoint_xyz[i].Y, this.EdgesPoint_xyz[i].X, this.Deg * Math.PI / 180, this.Radius1, this.Radius2, out rowPoint, out colPoint);
                    wcsPoint[i].X = colPoint;
                    wcsPoint[i].Y = rowPoint;
                    wcsPoint[i].Z = this.EdgesPoint_xyz[i].Z;
                    wcsPoint[i].Grab_x = Grab_x;
                    wcsPoint[i].Grab_y = Grab_y;
                    wcsPoint[i].Grab_z = Grab_z;
                    wcsPoint[i].Grab_theta = this.Grab_theta;
                    wcsPoint[i].Grab_u = this.Grab_u;
                    wcsPoint[i].Grab_v = this.Grab_v;
                    /////////////////////////////////////
                    wcsPoint[i].Tag = this.Tag;
                    wcsPoint[i].ViewWindow = this.ViewWindow;
                    wcsPoint[i].CamName = this.CamName;
                    wcsPoint[i].CamParams = this.CamParams;
                }
            }
            return wcsPoint;
        }

        public userWcsEllipse AffineWcsEllipse(HTuple transPose)
        {
            userWcsEllipse wcsEllipse;
            AffinePoint3D(this, transPose, out wcsEllipse);
            wcsEllipse.DiffRadius = this.DiffRadius;
            wcsEllipse.PointOrder = this.PointOrder;
            return wcsEllipse;
        }

        public userWcsEllipse AffineWcsEllipse2D(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi, ratate_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsEllipse wcsEllipse = this.Clone();
            // 变换角度
            // 变换角度
            double sx, sy, phi, theta, tx, ty;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, new HTuple(this.Deg).TupleRad() + Phi);
            sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            ratate_phi = phi;
            // 变换起始角度
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, new HTuple(this.Start_deg).TupleRad() + Phi);
            sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            start_phi = phi;
            // 变换终止角度
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, new HTuple(this.End_deg).TupleRad() + Phi);
            sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            end_phi = phi;
            //变换中心点
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
            //////////////////////////////////////////
            wcsEllipse = new userWcsEllipse(Qx[0], Qy[0], this.Z, ratate_phi.TupleDeg().D, this.Radius1, this.Radius2, this.Grab_x, this.Grab_y, this.CamParams); //
            wcsEllipse.Start_deg = start_phi.TupleDeg().D;
            wcsEllipse.End_deg = end_phi.TupleDeg().D;
            wcsEllipse.DiffRadius = this.DiffRadius;
            wcsEllipse.NormalPhi = this.NormalPhi;
            wcsEllipse.PointOrder = this.PointOrder;
            wcsEllipse.Tag = this.Tag;
            wcsEllipse.CamName = this.CamName;
            wcsEllipse.ViewWindow = this.ViewWindow;
            wcsEllipse.Grab_x = this.Grab_x;
            wcsEllipse.Grab_y = this.Grab_y;
            wcsEllipse.Grab_theta = this.Grab_theta;
            return wcsEllipse;
        }

        public userWcsEllipse AffineWcsEllipse2D(HTuple homMat2D, enAffineTransOrientation AffineTransOrientation)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsEllipse wcsEllipse = this;
            switch (AffineTransOrientation)
            {
                case enAffineTransOrientation.正向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);
                    //wcsEllipse = new userWcsEllipse(Qx[0].D, Qy[0].D, this.z, this.deg + Phi.D, this.radius1, this.radius2, this.grab_x, this.grab_y, this.CamParams); //
                    wcsEllipse = new userWcsEllipse(Qx[0].D, Qy[0].D, this.Z, this.Deg + Phi.D, this.Radius1, this.Radius2, Qx[1], Qy[1], this.CamParams); //
                    break;
                ///////////////
                case enAffineTransOrientation.反向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);
                    start_phi = this.Deg + Phi.D >= 0 ? this.Deg + Phi.D : this.Deg + Phi.D + Math.PI * 2;
                    // end_phi = this.end_deg + Phi.D >= 0 ? this.end_deg + Phi.D : this.end_deg - Phi.D + Math.PI * 2;
                    //wcsEllipse = new userWcsEllipse(Qx[0].D, Qy[0].D, this.z, start_phi, this.radius1, this.radius2, this.grab_x, this.grab_y, this.CamParams);
                    wcsEllipse = new userWcsEllipse(Qx[0].D, Qy[0].D, this.Z, start_phi, this.Radius1, this.Radius2, Qx[1].D, Qy[1].D, this.CamParams);
                    break;
            }
            wcsEllipse.DiffRadius = this.DiffRadius;
            wcsEllipse.NormalPhi = this.NormalPhi;
            wcsEllipse.PointOrder = this.PointOrder;
            wcsEllipse.Grab_x = this.Grab_x;
            wcsEllipse.Grab_y = this.Grab_y;
            wcsEllipse.Grab_theta = this.Grab_theta;
            return wcsEllipse;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenEllipseContourXld(this.Y * 1, this.X, this.Deg * Math.PI / 180, this.Radius1, this.Radius2, 0, Math.PI * 2, this.PointOrder, 0.005);
            return xld;
        }
        public override string ToString()
        {
            return string.Join(",", "userWcsEllipse", this.X, this.Y, this.Z, this.Deg, this.Radius1, this.Radius2);
            //return string.Format("userWcsEllipse: x={0},y={1},z={2},deg={3},radius1={4},radius2={5}", this.X, this.Y, this.Z, this.Deg, this.Radius1, this.Radius2);
        }
        private void AffinePoint3D(userWcsEllipse ellipse1, HTuple transPose3D, out userWcsEllipse ellipse)
        {
            HTuple Qx = new HTuple(0);
            HTuple Qy = new HTuple(0);
            HTuple Qz = new HTuple(0);
            HTuple X = new HTuple(ellipse1.X);
            HTuple Y = new HTuple(ellipse1.Y);
            HTuple Z = new HTuple(ellipse1.Z);
            HTuple Qx1, Qy1, Qz1, phi;
            double deg = 0;
            HTuple homMat3D = null;
            if (transPose3D != null && transPose3D.Length == 7)
                HOperatorSet.PoseToHomMat3d(transPose3D, out homMat3D);
            else
                HOperatorSet.HomMat3dIdentity(out homMat3D);
            if (X != null && X.Length > 0)
            {
                //if (Z == null || Z.Length != X.Length)
                HOperatorSet.TupleGenConst(X.Length, 0, out Z);
                HOperatorSet.AffineTransPoint3d(homMat3D, X, Y, Z, out Qx, out Qy, out Qz);
                HOperatorSet.AffineTransPoint3d(homMat3D, new HTuple(0, 10), new HTuple(0, 0), new HTuple(0, 0), out Qx1, out Qy1, out Qz1);
                HOperatorSet.LineOrientation(Qy1[0], Qx1[0], Qy1[1], Qx1[1], out phi);
                deg = phi.TupleDeg().D;
            }
            ellipse = new userWcsEllipse(Qx[0].D, Qy[0].D, ellipse1.Z, deg * -1, ellipse1.Radius1, ellipse1.Radius2); // 变换半径不会变
        }


        public userWcsEllipse Clone()
        {
            userWcsEllipse wcsEllipse = new userWcsEllipse();
            wcsEllipse.X = this.X;
            wcsEllipse.Y = this.Y;
            wcsEllipse.Z = this.Z;
            wcsEllipse.Radius1 = this.Radius1;
            wcsEllipse.Radius2 = this.Radius2;
            wcsEllipse.Deg = this.Deg;
            wcsEllipse.Start_deg = this.Start_deg;
            wcsEllipse.End_deg = this.End_deg;
            wcsEllipse.PointOrder = this.PointOrder;
            wcsEllipse.Grab_x = this.Grab_x;
            wcsEllipse.Grab_y = this.Grab_y;
            wcsEllipse.Grab_theta = this.Grab_theta;
            wcsEllipse.EdgesPoint_xyz = this.EdgesPoint_xyz;
            /////////
            wcsEllipse.X = this.DiffRadius;
            wcsEllipse.NormalPhi = this.NormalPhi;
            wcsEllipse.Color = this.Color;
            wcsEllipse.CamParams = this.CamParams;
            wcsEllipse.PointOrder = this.PointOrder;
            wcsEllipse.Tag = this.Tag;
            wcsEllipse.ViewWindow = this.ViewWindow;
            wcsEllipse.CamName = this.CamName;
            return wcsEllipse;
        }

    }

    [Serializable]
    public class userWcsEllipseSector : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Radius1 { get; set; }
        public double Radius2 { get; set; }
        public double Deg { get; set; }
        public double Start_deg { get; set; }
        public double End_deg { get; set; }
        public string PointOrder { get; set; }

        // 用于绘图变量
        public double DiffRadius { get; set; }
        public double[] NormalPhi { get; set; }

        public userWcsEllipseSector()
        {
            this.Y = 0;
            this.X = 0;
            this.Z = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Deg = 0;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = new userWcsPoint[0];
            //////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
        }
        public userWcsEllipseSector(CameraParam camParam)
        {
            this.Y = 0;
            this.X = 0;
            this.Z = 0;
            this.Radius1 = 0;
            this.Radius2 = 0;
            this.Deg = 0;
            this.Start_deg = 0;
            this.End_deg = Math.PI * 2;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            //////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }

        public userWcsEllipseSector(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, double ref_x, double ref_y, CameraParam camParam)
        {
            this.Y = y;
            this.X = x;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            //////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = camParam;
        }

        public userWcsEllipseSector(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, string pointOrder)
        {
            this.Y = y;
            this.X = x;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = pointOrder;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            //////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsEllipseSector(double x, double y, double z, double deg, double radius1, double radius2, double start_deg, double end_deg, CameraParam CamParams)
        {
            this.Y = y;
            this.X = x;
            this.Z = z;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
            this.Deg = deg;
            this.Start_deg = start_deg;
            this.End_deg = end_deg;
            this.PointOrder = "positive";
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.EdgesPoint_xyz = null;
            //////////
            this.DiffRadius = GlobalVariable.pConfig.ArrowLength;
            this.NormalPhi = null;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userPixEllipseSector GetPixEllipseSector()
        {
            double row = 0, col = 0, rad = 0, start_rad = 0, end_rad = 0, pixRadius1 = 0, pixRadius2 = 0;
            rad = new HTuple(this.Deg).TupleRad().D;
            start_rad = new HTuple(this.Start_deg).TupleRad().D;
            end_rad = new HTuple(this.End_deg).TupleRad().D;
            if (this.CamParams != null)
            {
                this.CamParams?.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out row, out col);
                pixRadius1 = this.CamParams.TransWcsLengthToPixLength(this.Radius1);
                pixRadius2 = this.CamParams.TransWcsLengthToPixLength(this.Radius2);
            }
            userPixEllipseSector pixEllipseSector = new userPixEllipseSector(row, col, rad, pixRadius1, pixRadius2, start_rad, end_rad, this.CamParams);
            pixEllipseSector.DiffRadius = this.DiffRadius;
            pixEllipseSector.PointOrder = this.PointOrder;
            pixEllipseSector.Color = this.Color;
            pixEllipseSector.PointOrder = this.PointOrder;

            pixEllipseSector.Grab_x = this.Grab_x;
            pixEllipseSector.Grab_y = this.Grab_y;
            if (this.EdgesPoint_xyz != null)
            {
                pixEllipseSector.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixEllipseSector.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixEllipseSector;
        }
        public userPixEllipseSector GetPixEllipseSectorFrom3D()
        {
            double row, col, rad, start_rad, end_rad, pixRadius1, pixRadius2;
            rad = new HTuple(this.Deg).TupleRad().D;
            start_rad = new HTuple(this.Start_deg).TupleRad().D;
            end_rad = new HTuple(this.End_deg).TupleRad().D;
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out row, out col);
            pixRadius1 = this.CamParams.TransWcsLengthToPixLength(this.Radius1);
            pixRadius2 = this.CamParams.TransWcsLengthToPixLength(this.Radius2);

            /////////////////////////////
            userPixEllipseSector pixEllipseSector = new userPixEllipseSector(row, col, rad, pixRadius1, pixRadius2, start_rad, end_rad, this.CamParams);
            pixEllipseSector.DiffRadius = this.DiffRadius;
            pixEllipseSector.PointOrder = this.PointOrder;
            pixEllipseSector.Color = this.Color;

            pixEllipseSector.Grab_x = 0;
            pixEllipseSector.Grab_y = 0;
            if (this.EdgesPoint_xyz != null)
            {
                pixEllipseSector.EdgesPoint = new userPixPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < this.EdgesPoint_xyz.Length; i++)
                {
                    pixEllipseSector.EdgesPoint[i] = this.EdgesPoint_xyz[i].GetPixPoint();
                }
            }
            return pixEllipseSector;
        }
        public userWcsPoint[] GetFitWcsPoint()
        {
            userWcsPoint[] wcsPoint = new userWcsPoint[0];
            if (this.EdgesPoint_xyz != null)
            {
                wcsPoint = new userWcsPoint[this.EdgesPoint_xyz.Length];
                for (int i = 0; i < wcsPoint.Length; i++)
                {
                    wcsPoint[i] = new userWcsPoint();
                    double rowPoint, colPoint;
                    double phi = Math.Atan2(this.EdgesPoint_xyz[i].Y - this.Y, this.EdgesPoint_xyz[i].X - this.X) * -1;
                    HMisc.GetPointsEllipse(phi, this.Y, this.X, this.Deg * Math.PI / 180, this.Radius1, this.Radius2, out rowPoint, out colPoint);
                    wcsPoint[i].X = colPoint;
                    wcsPoint[i].Y = rowPoint;
                    wcsPoint[i].Z = this.EdgesPoint_xyz[i].Z;
                    wcsPoint[i].Grab_x = Grab_x;
                    wcsPoint[i].Grab_y = Grab_y;
                    wcsPoint[i].Grab_z = Grab_z;
                    wcsPoint[i].Grab_theta = this.Grab_theta;
                    wcsPoint[i].Grab_u = this.Grab_u;
                    wcsPoint[i].Grab_v = this.Grab_v;
                    /////////////////////////////////////
                    wcsPoint[i].Tag = this.Tag;
                    wcsPoint[i].ViewWindow = this.ViewWindow;
                    wcsPoint[i].CamName = this.CamName;
                    wcsPoint[i].CamParams = this.CamParams;
                }
            }
            return wcsPoint;
        }

        public userWcsEllipse GetWcsEllipse()
        {
            userWcsEllipse wcsEllipse = new userWcsEllipse();
            wcsEllipse.X = this.X;
            wcsEllipse.Y = this.Y;
            wcsEllipse.Z = this.Z;
            wcsEllipse.Radius1 = this.Radius1;
            wcsEllipse.Radius2 = this.Radius2;
            wcsEllipse.Deg = this.Deg;
            wcsEllipse.Start_deg = this.Start_deg;
            wcsEllipse.End_deg = this.End_deg;
            wcsEllipse.Grab_x = this.Grab_x;
            wcsEllipse.Grab_y = this.Grab_y;
            wcsEllipse.Grab_theta = this.Grab_theta;
            wcsEllipse.EdgesPoint_xyz = this.EdgesPoint_xyz;
            //////////
            wcsEllipse.DiffRadius = this.DiffRadius;
            wcsEllipse.NormalPhi = this.NormalPhi;
            wcsEllipse.Color = this.Color;
            wcsEllipse.CamParams = this.CamParams;
            wcsEllipse.PointOrder = this.PointOrder;
            wcsEllipse.CamName = this.CamName;
            wcsEllipse.Tag = this.Tag;
            wcsEllipse.ViewWindow = this.ViewWindow;

            return wcsEllipse;
        }
        public userWcsEllipseSector Affine2DWcsEllipseSector(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi, ratate_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsEllipseSector wcsEllipseSector = this.Clone();

            // 变换角度
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D homMat2dRotate = hHomMat2D.HomMat2dRotate(new HTuple(this.Deg).TupleRad() + Phi, 0, 0);
            Qx = homMat2dRotate.AffineTransPoint2d(new HTuple(0, 1), new HTuple(0, 0), out Qy);
            ratate_phi = Math.Atan2(Qy[1], Qx[1]);
            // 变换起始角度
            homMat2dRotate = hHomMat2D.HomMat2dRotate(new HTuple(this.Start_deg).TupleRad() + Phi, 0, 0);
            Qx = homMat2dRotate.AffineTransPoint2d(new HTuple(0, 1), new HTuple(0, 0), out Qy);
            start_phi = Math.Atan2(Qy[1], Qx[1]);
            // 变换终止角度
            homMat2dRotate = hHomMat2D.HomMat2dRotate(new HTuple(this.End_deg).TupleRad() + Phi, 0, 0);
            Qx = homMat2dRotate.AffineTransPoint2d(new HTuple(0, 0), new HTuple(1, 0), out Qy);
            end_phi = Math.Atan2(Qy[1], Qx[1]);
            /// 变换中心点
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);//+ Phi.D
            ////////////////////////////
            wcsEllipseSector = new userWcsEllipseSector(Qx[0].D, Qy[0].D, this.Z, ratate_phi.TupleDeg().D, this.Radius1, this.Radius2, start_phi.TupleDeg().D, end_phi.TupleDeg().D, this.Grab_x, this.Grab_y, this.CamParams);
            wcsEllipseSector.DiffRadius = this.DiffRadius;
            wcsEllipseSector.NormalPhi = this.NormalPhi;
            wcsEllipseSector.PointOrder = this.PointOrder;
            //////////////////
            wcsEllipseSector.Tag = this.Tag;
            wcsEllipseSector.ViewWindow = this.ViewWindow;
            wcsEllipseSector.CamName = this.CamName;
            wcsEllipseSector.Grab_x = this.Grab_x;
            wcsEllipseSector.Grab_y = this.Grab_y;
            wcsEllipseSector.Grab_theta = this.Grab_theta;
            return wcsEllipseSector;
        }

        public userWcsEllipseSector Affine2DWcsEllipseSector(HTuple homMat2D, enAffineTransOrientation AffineTransOrientation)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi, end_phi, ratate_phi;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.TupleDeg(Phi, out Phi);
            userWcsEllipseSector wcsEllipseSector = this;
            switch (AffineTransOrientation)
            {
                case enAffineTransOrientation.正向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);//+ Phi.D
                    //wcsEllipseSector = new userWcsEllipseSector(Qx[0].D, Qy[0].D, this.z, this.deg, this.radius1, this.radius2, this.start_deg + Phi.D, this.end_deg + Phi.D, this.grab_x, this.grab_y, this.camParam, this.camPose);
                    wcsEllipseSector = new userWcsEllipseSector(Qx[0].D, Qy[0].D, this.Z, this.Deg, this.Radius1, this.Radius2, this.Start_deg + Phi.D, this.End_deg + Phi.D, Qx[1].D, Qy[1].D, this.CamParams);
                    break;
                ///////////////
                case enAffineTransOrientation.反向变换:
                    HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X, this.Grab_x), new HTuple(this.Y, this.Grab_y), out Qx, out Qy);
                    start_phi = this.Start_deg + Phi.D >= 0 ? this.Start_deg + Phi.D : this.Start_deg + Phi.D + Math.PI * 2; //
                    end_phi = this.End_deg + Phi.D >= 0 ? this.End_deg + Phi.D : this.End_deg + Phi.D + Math.PI * 2;
                    ratate_phi = this.Deg + Phi.D >= 0 ? this.Deg + Phi.D : this.Deg + Phi.D + Math.PI * 2; // 旋转整体了就不旋转起始角和终止角，两者只可旋转一个
                    //wcsEllipseSector = new userWcsEllipseSector(Qx[0].D, Qy[0].D, this.z, this.deg, this.radius1, this.radius2, start_phi, end_phi, this.grab_x, this.grab_y, this.camParam, this.camPose);
                    wcsEllipseSector = new userWcsEllipseSector(Qx[0].D, Qy[0].D, this.Z, this.Deg, this.Radius1, this.Radius2, start_phi, end_phi, Qx[1].D, Qy[1].D, this.CamParams);
                    break;
            }
            wcsEllipseSector.DiffRadius = this.DiffRadius;
            wcsEllipseSector.NormalPhi = this.NormalPhi;
            wcsEllipseSector.PointOrder = this.PointOrder;
            return wcsEllipseSector;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenEllipseContourXld(this.Y * 1, this.X, this.Deg * Math.PI / 180, this.Radius1, this.Radius2, this.Start_deg * Math.PI / 180, this.End_deg * Math.PI / 180, this.PointOrder, 0.1);
            return xld;
        }

        public userWcsEllipseSector Clone()
        {
            userWcsEllipseSector wcsEllipseSector = new userWcsEllipseSector();
            wcsEllipseSector.X = this.X;
            wcsEllipseSector.Y = this.Y;
            wcsEllipseSector.Z = this.Z;
            wcsEllipseSector.Radius1 = this.Radius1;
            wcsEllipseSector.Radius2 = this.Radius2;
            wcsEllipseSector.Deg = this.Deg;
            wcsEllipseSector.Start_deg = this.Start_deg;
            wcsEllipseSector.End_deg = this.End_deg;
            wcsEllipseSector.PointOrder = this.PointOrder;
            wcsEllipseSector.Grab_x = this.Grab_x;
            wcsEllipseSector.Grab_y = this.Grab_y;
            wcsEllipseSector.Grab_theta = this.Grab_theta;
            wcsEllipseSector.EdgesPoint_xyz = this.EdgesPoint_xyz;
            //////////
            wcsEllipseSector.DiffRadius = this.DiffRadius;
            wcsEllipseSector.NormalPhi = this.NormalPhi;
            wcsEllipseSector.Color = this.Color;
            wcsEllipseSector.CamParams = this.CamParams;
            wcsEllipseSector.PointOrder = this.PointOrder;
            wcsEllipseSector.CamName = this.CamName;
            wcsEllipseSector.ViewWindow = this.ViewWindow;
            wcsEllipseSector.Tag = this.Tag;

            return wcsEllipseSector;
        }

        public override string ToString()
        {
            return string.Join(",", "userWcsEllipseSector", this.X, this.Y, this.Z, this.Deg, this.Radius1, this.Radius2, this.Start_deg, this.End_deg);
            //return string.Format("userWcsEllipseSector: x={0},y={1},z={2},deg={3},radius1={4},radius2={5},start_deg={6},end_deg={7}", this.X, this.Y, this.Z, this.Deg, this.Radius1, this.Radius2, this.Start_deg, this.End_deg);
        }

    }

    [Serializable]
    public class userWcsVector : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        // 使用3个角度，可以描述2D和3D向量
        public double Angle_x { get; set; }
        public double Angle_y { get; set; }
        public double Angle { get; set; }

        public userWcsVector()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Angle_x = 0;
            this.Angle_y = 0;
            this.Angle = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
        }
        public userWcsVector(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
            this.Angle_x = 0;
            this.Angle_y = 0;
            this.Angle = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }


        public userWcsVector(double x, double y, double z, double deg_x, double deg_y, double angle, double refPointX, double refPointY, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Angle_x = deg_x;
            this.Angle_y = deg_y;
            this.Angle = angle;
            this.Grab_x = refPointX;
            this.Grab_y = refPointY;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }

        public userWcsVector(double x, double y, double z, double angle, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Angle_x = 0;
            this.Angle_y = 0;
            this.Angle = angle;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }

        public userWcsVector(double x, double y, double z, double deg_x, double deg_y, double deg_z, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Angle_x = deg_x;
            this.Angle_y = deg_y;
            this.Angle = deg_z;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = CamParams;
        }
        public userWcsVector(double x, double y, double z, double deg_x, double deg_y, double deg_z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Angle_x = deg_x;
            this.Angle_y = deg_y;
            this.Angle = deg_z;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsVector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Angle_x = 0;
            this.Angle_y = 0;
            this.Angle = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }
        public userWcsVector(double x, double y, double z, double angle)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Angle_x = 0;
            this.Angle_y = 0;
            this.Angle = angle;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Color = enColor.green;
            this.CamParams = null;
        }

        /// <summary>
        /// 该方法用于获取显示在图像上的像素坐标
        /// </summary>
        /// <returns></returns>
        public userPixVector GetPixVector()
        {
            double rows, cols, rad;
            userPixVector pixVector;
            rad = new HTuple(this.Angle).TupleRad().D * 1;
            if (this.CamParams == null) this.CamParams = new CameraParam();//   throw new ArgumentNullException("CamParams");
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, this.Grab_x, this.Grab_y, out rows, out cols);
            pixVector = new userPixVector(rows, cols, rad, this.CamParams);
            pixVector.Color = this.Color;
            pixVector.Grab_x = this.Grab_x;
            pixVector.Grab_y = this.Grab_y;
            return pixVector;
        }
        public userPixVector GetPixVectorFrom3D()
        {
            double rows, cols, rad;
            userPixVector pixVector;
            rad = new HTuple(this.Angle).TupleRad().D * 1;
            if (this.CamParams == null) throw new ArgumentNullException("CamParams");
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out rows, out cols);
            pixVector = new userPixVector(rows, cols, rad, this.CamParams);
            pixVector.Color = this.Color;
            pixVector.Grab_x = 0;
            pixVector.Grab_y = 0;
            return pixVector;
        }
        public userWcsVector AffineWcsVector(HTuple homMat2D)
        {
            HTuple Qx, Qy;
            userWcsVector wcsVector;
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
            wcsVector = new userWcsVector(Qx[0].D, Qy[0].D, this.Z, this.Angle, this.CamParams);
            wcsVector.CamName = this.CamName;
            wcsVector.ViewWindow = this.ViewWindow;
            wcsVector.Tag = this.Tag;
            wcsVector.Grab_x = this.Grab_x;
            wcsVector.Grab_y = this.Grab_y;
            wcsVector.Grab_theta = this.Grab_theta;
            wcsVector.Angle = this.Angle;
            wcsVector.Angle_x = this.Angle_x;
            wcsVector.Angle_y = this.Angle_y;
            return wcsVector;
        }

        public HHomMat2D GetHomMat2D()
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0, 0, 0, this.X, this.Y, this.Angle * Math.PI / 180);
            return hHomMat2D;
        }
        public override string ToString()
        {
            return string.Join(",", "userWcsVector", this.X, this.Y, this.Z, this.Angle_x, this.Angle_x, this.Angle);
        }

        public static userWcsVector operator +(userWcsVector Vector1, userWcsVector Vector2)
        {
            userWcsVector NewVector = new userWcsVector();
            NewVector.X = Vector1.X + Vector2.X;
            NewVector.Y = Vector1.Y + Vector2.Y;
            NewVector.Angle = Vector1.Angle + Vector2.Angle;
            return NewVector;
        }
        public static userWcsVector operator -(userWcsVector Vector1, userWcsVector Vector2)
        {
            userWcsVector NewVector = new userWcsVector();
            NewVector.X = Vector1.X - Vector2.X;
            NewVector.Y = Vector1.Y - Vector2.Y;
            NewVector.Angle = Vector1.Angle - Vector2.Angle;
            return NewVector;
        }
    }

    [Serializable]
    public class userWcsPose
    {
        public double Tx;
        public double Ty;
        public double Tz;
        public double Rx;
        public double Ry;
        public double Rz;
        public int Type;
        public enColor color;

        public userWcsPose()
        {

        }
        public userWcsPose(double t_x, double t_y, double t_z, double r_x = 0, double r_y = 0, double r_z = 0, int type = 0)
        {
            this.Tx = t_x;
            this.Ty = t_y;
            this.Tz = t_z;
            this.Rx = r_x;
            this.Ry = r_y;
            this.Rz = r_z;
            this.Type = type;
            this.color = enColor.green;
        }
        public userWcsPose(HTuple pose)
        {
            if (pose == null || pose.Length < 7)
            {
                this.Tx = 0;
                this.Ty = 0;
                this.Tz = 0;
                this.Rx = 0;
                this.Ry = 0;
                this.Rz = 0;
                this.Type = 0;
                this.color = enColor.green;
            }
            else
            {
                switch (pose.Type)
                {
                    case HTupleType.DOUBLE:
                        this.Tx = pose[0].D;
                        this.Ty = pose[1].D;
                        this.Tz = pose[2].D;
                        this.Rx = pose[3].D;
                        this.Ry = pose[4].D;
                        this.Rz = pose[5].D;
                        this.Type = (int)pose[6].D;
                        this.color = enColor.green;
                        break;

                    case HTupleType.INTEGER:
                        this.Tx = (double)pose[0].I;
                        this.Ty = (double)pose[1].I;
                        this.Tz = (double)pose[2].I;
                        this.Rx = (double)pose[3].I;
                        this.Ry = (double)pose[4].I;
                        this.Rz = (double)pose[5].I;
                        this.Type = (int)pose[6].I;
                        this.color = enColor.green;
                        break;

                    case HTupleType.LONG:
                        this.Tx = (double)pose[0].L;
                        this.Ty = (double)pose[1].L;
                        this.Tz = (double)pose[2].L;
                        this.Rx = (double)pose[3].L;
                        this.Ry = (double)pose[4].L;
                        this.Rz = (double)pose[5].L;
                        this.Type = (int)pose[6].L;
                        this.color = enColor.green;
                        break;

                    case HTupleType.MIXED:
                        this.Tx = Convert.ToDouble(pose[0].O);
                        this.Ty = Convert.ToDouble(pose[1].O);
                        this.Tz = Convert.ToDouble(pose[2].O);
                        this.Rx = Convert.ToDouble(pose[3].O);
                        this.Ry = Convert.ToDouble(pose[4].O);
                        this.Rz = Convert.ToDouble(pose[5].O);
                        this.Type = Convert.ToInt32(pose[6].O);
                        this.color = enColor.green;
                        break;

                    case HTupleType.EMPTY:
                    case HTupleType.STRING:
                    default:
                        this.Tx = 0;
                        this.Ty = 0;
                        this.Tz = 0;
                        this.Rx = 0;
                        this.Ry = 0;
                        this.Rz = 0;
                        this.Type = 0;
                        this.color = enColor.green;
                        break;
                }
            }
        }

        public userWcsPose(HPose pose)
        {
            if (pose == null || pose.RawData.Length < 7)
            {
                this.Tx = 0;
                this.Ty = 0;
                this.Tz = 0;
                this.Rx = 0;
                this.Ry = 0;
                this.Rz = 0;
                this.Type = 0;
                this.color = enColor.green;
            }
            else
            {
                HTuple tuple = pose.RawData;
                switch (tuple.Type)
                {
                    case HTupleType.DOUBLE:
                        this.Tx = pose[0].D;
                        this.Ty = pose[1].D;
                        this.Tz = pose[2].D;
                        this.Rx = pose[3].D;
                        this.Ry = pose[4].D;
                        this.Rz = pose[5].D;
                        this.Type = (int)pose[6].D;
                        this.color = enColor.green;
                        break;

                    case HTupleType.INTEGER:
                        this.Tx = (double)pose[0].I;
                        this.Ty = (double)pose[1].I;
                        this.Tz = (double)pose[2].I;
                        this.Rx = (double)pose[3].I;
                        this.Ry = (double)pose[4].I;
                        this.Rz = (double)pose[5].I;
                        this.Type = (int)pose[6].I;
                        this.color = enColor.green;
                        break;

                    case HTupleType.LONG:
                        this.Tx = (double)pose[0].L;
                        this.Ty = (double)pose[1].L;
                        this.Tz = (double)pose[2].L;
                        this.Rx = (double)pose[3].L;
                        this.Ry = (double)pose[4].L;
                        this.Rz = (double)pose[5].L;
                        this.Type = (int)pose[6].L;
                        this.color = enColor.green;
                        break;

                    case HTupleType.MIXED:
                        this.Tx = (double)pose[0].O;
                        this.Ty = (double)pose[1].O;
                        this.Tz = (double)pose[2].O;
                        this.Rx = (double)pose[3].O;
                        this.Ry = (double)pose[4].O;
                        this.Rz = (double)pose[5].O;
                        this.Type = (int)pose[6].O;
                        this.color = enColor.green;
                        break;

                    case HTupleType.EMPTY:
                    case HTupleType.STRING:
                    default:
                        this.Tx = 0;
                        this.Ty = 0;
                        this.Tz = 0;
                        this.Rx = 0;
                        this.Ry = 0;
                        this.Rz = 0;
                        this.Type = 0;
                        this.color = enColor.green;
                        break;
                }
            }

        }
        public userWcsPose(HHomMat3D HomMat3D)
        {
            HPose pose = HomMat3D.HomMat3dToPose();
            if (pose == null || pose.RawData.Length < 7)
            {
                this.Tx = 0;
                this.Ty = 0;
                this.Tz = 0;
                this.Rx = 0;
                this.Ry = 0;
                this.Rz = 0;
                this.Type = 0;
                this.color = enColor.green;
            }
            else
            {
                HTuple tuple = pose.RawData;
                switch (tuple.Type)
                {
                    case HTupleType.DOUBLE:
                        this.Tx = pose[0].D;
                        this.Ty = pose[1].D;
                        this.Tz = pose[2].D;
                        this.Rx = pose[3].D;
                        this.Ry = pose[4].D;
                        this.Rz = pose[5].D;
                        this.Type = (int)pose[6].D;
                        this.color = enColor.green;
                        break;

                    case HTupleType.INTEGER:
                        this.Tx = (double)pose[0].I;
                        this.Ty = (double)pose[1].I;
                        this.Tz = (double)pose[2].I;
                        this.Rx = (double)pose[3].I;
                        this.Ry = (double)pose[4].I;
                        this.Rz = (double)pose[5].I;
                        this.Type = (int)pose[6].I;
                        this.color = enColor.green;
                        break;

                    case HTupleType.LONG:
                        this.Tx = (double)pose[0].L;
                        this.Ty = (double)pose[1].L;
                        this.Tz = (double)pose[2].L;
                        this.Rx = (double)pose[3].L;
                        this.Ry = (double)pose[4].L;
                        this.Rz = (double)pose[5].L;
                        this.Type = (int)pose[6].L;
                        this.color = enColor.green;
                        break;

                    case HTupleType.MIXED:
                        this.Tx = (double)pose[0].O;
                        this.Ty = (double)pose[1].O;
                        this.Tz = (double)pose[2].O;
                        this.Rx = (double)pose[3].O;
                        this.Ry = (double)pose[4].O;
                        this.Rz = (double)pose[5].O;
                        this.Type = (int)pose[6].O;
                        this.color = enColor.green;
                        break;

                    case HTupleType.EMPTY:
                    case HTupleType.STRING:
                    default:
                        this.Tx = 0;
                        this.Ty = 0;
                        this.Tz = 0;
                        this.Rx = 0;
                        this.Ry = 0;
                        this.Rz = 0;
                        this.Type = 0;
                        this.color = enColor.green;
                        break;
                }
            }
        }
        public HTuple GetHtuple()
        {
            return new HTuple(this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type);
        }
        public HPose GetHPose()
        {
            return new HPose(this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, "Rp+T", "gba", "point");
        }
        public void RigidTranslatePoint3D(double Px, double Py, double Pz, out double Qx, out double Qy, out double Qz)
        {
            Qx = this.GetHomMat3D().AffineTransPoint3d(Px, Py, Pz, out Qy, out Qz);
        }
        public void RigidTranslatePoint3D(HTuple Px, HTuple Py, HTuple Pz, out HTuple Qx, out HTuple Qy, out HTuple Qz)
        {
            if (Px == null)
            {
                throw new ArgumentNullException("Px");
            }
            if (Py == null)
            {
                throw new ArgumentNullException("Py");
            }
            if (Pz == null)
            {
                throw new ArgumentNullException("Pz");
            }
            if (Px.Length != Py.Length)
            {
                throw new ArgumentException("Px与Py长度不相等");
            }
            Qx = this.GetHomMat3D().AffineTransPoint3d(Px, Py, Pz, out Qy, out Qz);
        }
        public void RigidTranslatePoint3D(HTuple Px, HTuple Py, out HTuple Qx, out HTuple Qy)
        {
            if (Px == null)
            {
                throw new ArgumentNullException("Px");
            }
            if (Py == null)
            {
                throw new ArgumentNullException("Py");
            }

            if (Px.Length != Py.Length)
            {
                throw new ArgumentException("Px与Py长度不相等");
            }
            HTuple Pz = HTuple.TupleGenConst(Px.Length, 0);
            HTuple Qz;
            Qx = this.GetHomMat3D().AffineTransPoint3d(Px, Py, Pz, out Qy, out Qz);
        }
        public void RigidTranslatePoint3D(double Px, double Py, out double Qx, out double Qy)
        {
            double Pz = 0;
            double Qz;
            Qx = this.GetHomMat3D().AffineTransPoint3d(Px, Py, Pz, out Qy, out Qz);
        }
        public userWcsPose InvertPose3D()
        {
            HPose hPose = new HPose(this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, "Rp+T", "gba", "point").PoseInvert();
            return new userWcsPose(hPose);
        }
        public HHomMat3D GetHomMat3D()
        {
            HHomMat3D hHomMat3D = GetHPose().PoseToHomMat3d();
            return hHomMat3D;
        }
        public userWcsCoordSystem GetUserWcsCoordSystem()
        {
            userWcsCoordSystem wcsCoord = new userWcsCoordSystem();
            wcsCoord.CurrentPoint = new userWcsVector(this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz * -1);
            return wcsCoord;
        }

        public override string ToString()
        {
            return string.Join(",", "userWcsPose3D", this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type);
            //return string.Format("{0},{1},{2},{3},{4},{5},{6}", this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type);
        }


    }

    [Serializable]
    public class userWcsPolygon : WcsData
    {
        public List<double> X { get; set; }
        public List<double> Y { get; set; }
        public userPixPoint[] EdgesPoint { get; set; }
        // 扩展属性
        public double DiffRadius { get; set; }
        public double NormalPhi { get; set; }
        public userWcsPolygon(double[] x, double[] y)
        {
            this.X = new List<double>();
            this.Y = new List<double>();
            this.X.AddRange(x);
            this.Y.AddRange(y);
        }
        public userWcsPolygon AffinePixPolygon(HTuple homMat2D)
        {
            // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
            if (homMat2D == null) return this;
            HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
            HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
            HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X.ToArray()), new HTuple(this.Y.ToArray()), out Qx, out Qy);
            userWcsPolygon PixRect1 = new userWcsPolygon(Qx.DArr, Qy.DArr);
            /////////////////////
            PixRect1.Tag = this.Tag;
            PixRect1.ViewWindow = this.ViewWindow;
            PixRect1.CamName = this.CamName;

            return PixRect1;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (this.X.Count > 0)
                hXLDCont.GenContourPolygonXld(new HTuple(this.X.ToArray(), this.X[0]), new HTuple(this.Y.ToArray(), this.Y[0]));
            return hXLDCont;
        }
        public userPixPolygon GetPixPolygon()
        {
            userPixPolygon pixPolygon;
            HTuple row = 0, col = 0;
            if (this.CamParams == null) return new userPixPolygon();
            this.CamParams.WorldPointsToImagePlane(this.X.ToArray(), this.Y.ToArray(), 0, 0, out row, out col);
            pixPolygon = new userPixPolygon(row, col);
            pixPolygon.Color = this.Color;
            pixPolygon.CamParams = this.CamParams;
            pixPolygon.DiffRadius = this.DiffRadius;
            ////////////////////////////
            pixPolygon.Tag = this.Tag;
            pixPolygon.ViewWindow = this.ViewWindow;
            pixPolygon.CamName = this.CamName;

            return pixPolygon;
        }
        public override string ToString()
        {
            string str1 = string.Join(",", this.X.ToArray());
            string str2 = string.Join(",", this.Y.ToArray());
            return string.Join(";", "userWcsPolygon", str1, str2);
            //return string.Join(";", this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type);
            //return string.Format("{0},{1},{2},{3},{4},{5},{6}", this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type);
        }
    }

    [Serializable]
    public class userWcsThick : WcsData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Dist1 { get; set; }
        public double Dist2 { get; set; }
        public double Thick { get; set; }
        public double Theta { get; set; }
        public double U { get; set; }
        public double V { get; set; }
        public string Result { get; set; }
        public userWcsThick()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 100;
            this.Grab_y = 100;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.Result = "OK";

        }
        public userWcsThick(CameraParam CamParam)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 100;
            this.Grab_y = 100;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.CamParams = CamParam;
            this.Result = "OK";
        }

        public userWcsThick(double x, double y, double z, double ref_x, double ref_y, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.CamParams = null;
            this.Result = "OK";
        }
        public userWcsThick(double x, double y, double z, double ref_x, double ref_y, CameraParam camParam)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = ref_x;
            this.Grab_y = ref_y;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.CamParams = camParam;
            this.Result = "OK";
        }
        public userWcsThick(double x, double y, double z, HTuple camParam, HTuple camPose)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.CamParams = new CameraParam();
            this.Result = "OK";
        }
        public userWcsThick(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.CamParams = null;
            this.Result = "OK";
        }
        public userWcsThick(double x, double y, double z, CameraParam CamParams)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = 0;
            this.V = 0;
            this.Theta = 0;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.CamParams = CamParams;
            this.Result = "OK";
        }
        public userWcsThick(double x, double y, double z, double u, double v, double theta)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.U = u;
            this.V = v;
            this.Theta = theta;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_theta = 0;
            this.Color = enColor.green;
            this.CamParams = null;
            this.Result = "OK";
        }

        public userPixThick GetPixThick()
        {
            userPixThick pixPoint;
            double row, col;
            if (this.CamParams == null) return new userPixThick();
            this.CamParams.WorldPointsToImagePlane(this.X, this.Y, 0, 0, out row, out col);
            pixPoint = new userPixThick(row, col, this.CamParams);
            pixPoint.Color = this.Color;
            pixPoint.Dist1 = this.Dist1;
            pixPoint.Dist2 = this.Dist2;
            pixPoint.Thick = this.Thick;
            pixPoint.Result = this.Result;
            return pixPoint;
        }
        public HXLDCont GetXLD()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCrossContourXld(this.Y * 1, this.X, 0.2, 0);
            return xld;
        }
        public override string ToString()
        {
            return string.Format(",", "userWcsThick", this.X, this.Y, this.Z, this.Theta, this.Dist1, this.Dist2, this.Thick);
            //return string.Format("userWcsPoint: x={0},y={1},z={2},theta={3}", this.X, this.Y, this.Z, this.Theta);
        }
        public userWcsThick Clone()
        {
            userWcsThick wcsPoint = new userWcsThick();
            wcsPoint.X = this.X;
            wcsPoint.Y = this.Y;
            wcsPoint.Z = this.Z;
            wcsPoint.U = this.U;
            wcsPoint.V = this.V;
            wcsPoint.Theta = this.Theta;
            wcsPoint.Grab_x = this.Grab_x;
            wcsPoint.Grab_y = this.Grab_y;
            wcsPoint.Grab_theta = this.Grab_theta;
            wcsPoint.Color = this.Color;
            wcsPoint.CamParams = this.CamParams;
            wcsPoint.Result = this.Result;
            return wcsPoint;
        }
    }
    #endregion

    [Serializable]
    public class userWcsCoordSystem  // 当有多个坐标点时，使用数组
    {
        public userWcsVector ReferencePoint { get; set; }
        public userWcsVector CurrentPoint { get; set; }
        public enColor Color { get; set; }
        public bool Result { get; set; }

        /// <summary>
        /// 弃用
        /// </summary>
        public double Grab_x { get; set; }
        /// <summary>
        /// 弃用
        /// </summary>
        public double Grab_y { get; set; }
        /// <summary>
        /// 弃用
        /// </summary>
        public double Grab_Theta { get; set; }
        public userWcsCoordSystem()
        {
            this.ReferencePoint = new userWcsVector();
            this.CurrentPoint = new userWcsVector();
            this.Color = enColor.orange;
            this.Result = false;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_Theta = 0;
        }
        public userWcsCoordSystem(CameraParam cameraParam)
        {
            this.ReferencePoint = new userWcsVector(0.0, 0.0, 0.0, 0.0, cameraParam);
            this.CurrentPoint = new userWcsVector(0.0, 0.0, 0.0, 0.0, cameraParam);
            this.Color = enColor.orange;
            this.Result = false;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_Theta = 0;
        }
        public userWcsCoordSystem(LaserParam laserParam)
        {
            //this.ReferencePoint = new userWcsVector(0.0, 0.0, 0.0, 0.0, laserParam);
            //this.CurrentPoint = new userWcsVector(0.0, 0.0, 0.0, 0.0, laserParam);
            this.Color = enColor.orange;
            this.Result = false;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_Theta = 0;
        }
        public userWcsCoordSystem(userWcsVector referencePoint, userWcsVector currentPoint)
        {
            this.ReferencePoint = referencePoint;
            this.CurrentPoint = currentPoint;
            this.Color = enColor.orange;
            this.Result = false;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_Theta = 0;
        }
        public userWcsCoordSystem(userWcsVector currentPoint)
        {
            this.ReferencePoint = new userWcsVector();
            this.CurrentPoint = currentPoint;
            this.Color = enColor.orange;
            this.Result = false;
            this.Grab_x = 0;
            this.Grab_y = 0;
            this.Grab_Theta = 0;
        }
        public userWcsCoordSystem(userWcsPose wcsPose)
        {
            this.ReferencePoint = new userWcsVector();
            this.CurrentPoint = new userWcsVector(wcsPose.Tx, wcsPose.Ty, wcsPose.Tz, wcsPose.Rx, wcsPose.Ry, wcsPose.Rz);
            this.Color = enColor.orange;
            this.Result = false;
        }
        public userPixCoordSystem GetPixCoordSystem()
        {
            userPixVector pixVector1 = this.ReferencePoint.GetPixVector();
            userPixVector pixVector2 = this.CurrentPoint.GetPixVector();
            userPixCoordSystem coordSystem = new userPixCoordSystem(pixVector1, pixVector2);
            coordSystem.Result = this.Result;
            coordSystem.Color = this.Color;
            return coordSystem;
        }

        /// <summary>
        /// 获取当前坐标系相对参考坐标系的变化矩阵，适用于坐标系跟随，当当前坐标系变化后，使基于坐标系的对象跟随当前坐标系变化
        /// </summary>
        /// <returns></returns>
        public HTuple GetVariationHomMat2D()
        {
            HTuple homMat2dIdentity, homMat2dTranslate, homMat2dRotate;
            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            HOperatorSet.HomMat2dTranslate(homMat2dIdentity, this.CurrentPoint.X - this.ReferencePoint.X, this.CurrentPoint.Y - this.ReferencePoint.Y, out homMat2dTranslate);// 
            HOperatorSet.HomMat2dRotate(homMat2dTranslate, (this.CurrentPoint.Angle * Math.PI / 180 - this.ReferencePoint.Angle * Math.PI / 180), this.CurrentPoint.X, this.CurrentPoint.Y, out homMat2dRotate);

            //HHomMat2D hHomMat2D = new HHomMat2D();
            //hHomMat2D.VectorAngleToRigid(this.ReferencePoint.x, this.ReferencePoint.y, this.ReferencePoint.Angle * Math.PI / 180,
            //    this.CurrentPoint.x, this.CurrentPoint.y, this.CurrentPoint.Angle * Math.PI / 180);
            //return hHomMat2D.RawData;

            return homMat2dRotate;
        }

        public HHomMat2D GetVariationHomMat2DNew()
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(this.ReferencePoint.X, this.ReferencePoint.Y, this.ReferencePoint.Angle * Math.PI / 180,
                this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Angle * Math.PI / 180);
            return hHomMat2D;
        }

        public HTuple GetVariationHomMat2D(userWcsVector CurrentPoint)
        {
            HTuple homMat2dIdentity, homMat2dTranslate, homMat2dRotate;
            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            HOperatorSet.HomMat2dTranslate(homMat2dIdentity, CurrentPoint.X - this.ReferencePoint.X, CurrentPoint.Y - this.ReferencePoint.Y, out homMat2dTranslate);// 
            HOperatorSet.HomMat2dRotate(homMat2dTranslate, (CurrentPoint.Angle * Math.PI / 180 - this.ReferencePoint.Angle * Math.PI / 180) * 1, CurrentPoint.X, CurrentPoint.Y, out homMat2dRotate);
            return homMat2dRotate;
        }
        public HTuple GetInvertVariationHomMat2D()
        {
            HTuple homMat2dInvert;
            HOperatorSet.HomMat2dInvert(GetVariationHomMat2D(), out homMat2dInvert);
            return homMat2dInvert;
        }

        public userWcsVector GetAddXyTheta()
        {
            HTuple homMat2dInvert;
            HOperatorSet.HomMat2dInvert(GetVariationHomMat2D(), out homMat2dInvert);
            HTuple sx, sy, phi, theta, tx, ty;
            HOperatorSet.HomMat2dToAffinePar(homMat2dInvert, out sx, out sy, out phi, out theta, out tx, out ty);
            HTuple angle = phi.TupleDeg();
            return new userWcsVector(tx.D, ty.D, 0.0, angle.D);
        }

        /// <summary>
        /// 获取当前坐标系的变换矩阵,通常用于将理论坐标变换到当前坐标系下, 这种情况下参考点默认为 ：0 ，0；
        /// </summary>
        /// <returns></returns>
        public HTuple GetCurrentHomMat2D()
        {
            HTuple homMat2dIdentity, homMat2dTranslate, homMat2dRotate;
            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            HOperatorSet.HomMat2dTranslate(homMat2dIdentity, this.CurrentPoint.X, this.CurrentPoint.Y, out homMat2dTranslate);// 
            HOperatorSet.HomMat2dRotate(homMat2dTranslate, (this.CurrentPoint.Angle * Math.PI / 180) * 1, this.CurrentPoint.X, this.CurrentPoint.Y, out homMat2dRotate);
            return homMat2dRotate;
        }
        public HTuple GetInvertCurrentHomMat2D()
        {
            HTuple homMat2dInvert;
            HOperatorSet.HomMat2dInvert(GetCurrentHomMat2D(), out homMat2dInvert);
            return homMat2dInvert;
        }

        /// <summary>
        /// 获取当前坐标系的变换矩阵,通常用于将理论坐标变换到当前坐标系下
        /// </summary>
        /// <returns></returns>
        public HTuple GetVariationHomMat3D()
        {
            HTuple homMat3dIdentity, homMat3dTranslate, homMat3dRotate_x, homMat3dRotate_y, homMat3dRotate_z;
            HOperatorSet.HomMat3dIdentity(out homMat3dIdentity);
            HOperatorSet.HomMat3dTranslate(homMat3dIdentity, this.CurrentPoint.X - this.ReferencePoint.X, this.CurrentPoint.Y - this.ReferencePoint.Y, this.CurrentPoint.Z - this.ReferencePoint.Z, out homMat3dTranslate);// 
            /////////////////////
            HOperatorSet.HomMat3dRotate(homMat3dTranslate, (this.CurrentPoint.Angle * Math.PI / 180 - this.ReferencePoint.Angle * Math.PI / 180) * 1, "z", this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, out homMat3dRotate_z);
            HOperatorSet.HomMat3dRotate(homMat3dRotate_z, (this.CurrentPoint.Angle_y * Math.PI / 180 - this.ReferencePoint.Angle_y * Math.PI / 180) * 1, "y", this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, out homMat3dRotate_y);
            HOperatorSet.HomMat3dRotate(homMat3dRotate_y, (this.CurrentPoint.Angle_x * Math.PI / 180 - this.ReferencePoint.Angle_x * Math.PI / 180) * 1, "x", this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, out homMat3dRotate_x);

            return homMat3dRotate_x;
        }
        public HTuple GetInvertVariationHomMat3D()
        {
            HTuple homMat3dInvert;
            HOperatorSet.HomMat3dInvert(GetVariationHomMat3D(), out homMat3dInvert);
            return homMat3dInvert;
        }

        /// <summary>
        /// 获取当前坐标系的变换矩阵,通常用于将理论坐标变换到当前坐标系下
        /// </summary>
        /// <returns></returns>
        public HTuple GetCurrentHomMat3D()
        {
            HTuple homMat3dIdentity, homMat3dTranslate, homMat3dRotate_x, homMat3dRotate_y, homMat3dRotate_z;
            HOperatorSet.HomMat3dIdentity(out homMat3dIdentity);
            HOperatorSet.HomMat3dTranslate(homMat3dIdentity, this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, out homMat3dTranslate);// 
            /////////////////////
            HOperatorSet.HomMat3dRotate(homMat3dTranslate, (this.CurrentPoint.Angle * Math.PI / 180) * 1, "z", this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, out homMat3dRotate_z);
            HOperatorSet.HomMat3dRotate(homMat3dRotate_z, (this.CurrentPoint.Angle_y * Math.PI / 180) * 1, "y", this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, out homMat3dRotate_y);
            HOperatorSet.HomMat3dRotate(homMat3dRotate_y, (this.CurrentPoint.Angle_x * Math.PI / 180) * 1, "x", this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, out homMat3dRotate_x);

            return homMat3dRotate_x;
        }
        public HTuple GetInvertCurrentHomMat3D()
        {
            HTuple homMat3dInvert;
            HOperatorSet.HomMat3dInvert(GetCurrentHomMat3D(), out homMat3dInvert);
            return homMat3dInvert;
        }
        public userWcsPose GetUserWcsPose3D(int index = 0)
        {
            double Tx = Convert.ToDouble(this.CurrentPoint.X);
            double Ty = Convert.ToDouble(this.CurrentPoint.Y);
            double Tz = Convert.ToDouble(this.CurrentPoint.Z);
            double Rx = Convert.ToDouble(this.CurrentPoint.Angle_x);
            double Ry = Convert.ToDouble(this.CurrentPoint.Angle_y);
            double Rz = Convert.ToDouble(this.CurrentPoint.Angle);
            int Type = 0;
            return new userWcsPose(Tx, Ty, Tz, Rx, Ry, Rz, Type);
        }

        /// <summary>
        /// 获取世界坐标系的变化3D位姿
        /// </summary>
        /// <returns></returns>
        public HTuple GetVariationPose3D()
        {
            return new HTuple(this.CurrentPoint.X - this.ReferencePoint.X, this.CurrentPoint.Y - this.ReferencePoint.Y, this.CurrentPoint.Z - this.ReferencePoint.Z, this.CurrentPoint.Angle_x - this.ReferencePoint.Angle_x,
                this.CurrentPoint.Angle_y - this.ReferencePoint.Angle_y, this.CurrentPoint.Angle - this.ReferencePoint.Angle, 0);
        }
        public HTuple GetCurrentPose3D()
        {
            return new HTuple(this.CurrentPoint.X * 1, this.CurrentPoint.Y * 1, this.CurrentPoint.Z, this.CurrentPoint.Angle_x * -1, this.CurrentPoint.Angle_y * -1, this.CurrentPoint.Angle * -1, 0);
        }
        public HTuple GetInvertVariationPose3D()
        {
            HTuple homMat3dInvert;
            HOperatorSet.HomMat3dInvert(GetVariationPose3D(), out homMat3dInvert);
            return homMat3dInvert;
        }
        public HTuple GetInvertCurrentPose3D()
        {
            HTuple homMat2dInvert;
            HOperatorSet.HomMat2dInvert(GetCurrentHomMat2D(), out homMat2dInvert);
            return homMat2dInvert;
        }
        public HXLDCont GetXLD()
        {
            HTuple homMat2dIdentity, homMat2dRotate;
            userPixVector pixVector = CurrentPoint.GetPixVector();
            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            HOperatorSet.HomMat2dRotate(homMat2dIdentity, pixVector.Rad, pixVector.Row, pixVector.Col, out homMat2dRotate); //这里的Y坐标一定要取反
            HXLDCont xld = GenArrowContourXld(new HTuple(pixVector.Row, pixVector.Row), new HTuple(pixVector.Col, pixVector.Col), new HTuple(pixVector.Row, (pixVector.Row + 50) * -1), new HTuple(pixVector.Col + 100, pixVector.Col), 15, 5);
            return xld.AffineTransContourXld(new HHomMat2D(homMat2dRotate));
        }
        public HXLDCont GetXLDFrom3D()
        {
            HTuple homMat2dIdentity, homMat2dRotate;
            userPixVector pixVector = CurrentPoint.GetPixVectorFrom3D();
            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            HOperatorSet.HomMat2dRotate(homMat2dIdentity, pixVector.Rad, pixVector.Row, pixVector.Col, out homMat2dRotate); //这里的Y坐标一定要取反  
            HXLDCont xld = GenArrowContourXld(new HTuple(pixVector.Row, pixVector.Row), new HTuple(pixVector.Col, pixVector.Col), new HTuple(pixVector.Row, (pixVector.Row - 100)), new HTuple(pixVector.Col + 200, pixVector.Col), 35, 15);
            return xld.AffineTransContourXld(new HHomMat2D(homMat2dRotate));
        }

        public HXLDCont getAxis_x_Arrow(userPixVector pixVector, double axisLength, out HTuple row, out HTuple col)
        {
            HTuple homMat2dIdentity, homMat2dRotate;
            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            HOperatorSet.HomMat2dRotate(homMat2dIdentity, pixVector.Rad, pixVector.Row, pixVector.Col, out homMat2dRotate); //这里的Y坐标一定要取反
            HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(pixVector.Row), new HTuple(pixVector.Col + axisLength), out row, out col);
            return GenArrowContourXld(new HTuple(pixVector.Row), new HTuple(pixVector.Col), row, col, 15, 5);
        }
        public HXLDCont getAxis_y_Arrow(userPixVector pixVector, double axisLength, out HTuple row, out HTuple col)
        {
            HTuple homMat2dIdentity, homMat2dRotate;
            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
            HOperatorSet.HomMat2dRotate(homMat2dIdentity, pixVector.Rad, pixVector.Row, pixVector.Col, out homMat2dRotate); //这里的Y坐标一定要取反
            HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(pixVector.Row - axisLength), new HTuple(pixVector.Col), out row, out col);
            return GenArrowContourXld(new HTuple(pixVector.Row), new HTuple(pixVector.Col), row, col, 15, 5);
        }
        public void WriteString(HWindow window, int row_pos, int col_pos, int size, string content, string color)
        {
            window.SetTposition(row_pos, col_pos);
            window.SetFont("-Consolas-" + size.ToString() + "- *-0-*-*-1-");
            window.SetColor(color);
            window.WriteString(content);
        }
        public override string ToString()
        {
            //return "参考点"+ this.ReferencePoint.x.ToString() + "," + this.ReferencePoint.y.ToString() + "," + this.ReferencePoint.z.ToString() + ";"+ "当前点"+ this.CurrentPoint.x.ToString() + "," + this.CurrentPoint.y.ToString() + "," + this.CurrentPoint.z.ToString();
            return string.Format("参考点：x={0},y={1},z={2},deg={3},当前点：x={4},y={5},z={6},deg={7}", this.ReferencePoint.X, this.ReferencePoint.Y, this.ReferencePoint.Z, this.ReferencePoint.Angle,
                this.CurrentPoint.X, this.CurrentPoint.Y, this.CurrentPoint.Z, this.CurrentPoint.Angle);
        }
        public string ToString2()
        {
            //return "参考点"+ this.ReferencePoint.x.ToString() + "," + this.ReferencePoint.y.ToString() + "," + this.ReferencePoint.z.ToString() + ";"+ "当前点"+ this.CurrentPoint.x.ToString() + "," + this.CurrentPoint.y.ToString() + "," + this.CurrentPoint.z.ToString();
            return string.Format("x={0},y={1},z={2},deg={3}", (int)this.CurrentPoint.X, (int)this.CurrentPoint.Y, (int)this.CurrentPoint.Z, (int)this.CurrentPoint.Angle);
        }
        private HXLDCont GenArrowContourXld(HTuple Row1, HTuple Column1, HTuple Row2, HTuple Column2, double HeadLength, double HeadWidth)
        {
            if (Row1.Length != Row2.Length) return new HXLDCont();
            HXLDCont arrows = new HXLDCont();
            arrows.GenEmptyObj();
            HTuple Length = HMisc.DistancePp(Row1, Column1, Row2, Column2);
            HTuple ZeroLengthIndices = Length.TupleFind(0);
            if (ZeroLengthIndices != -1)
                Length[ZeroLengthIndices] = -1;
            // Calculate auxiliary variables.
            HTuple DR = 1.0 * (Row2 - Row1) / Length;
            HTuple DC = 1.0 * (Column2 - Column1) / Length;
            HTuple HalfHeadWidth = HeadWidth / 2.0;
            // Calculate end points of the arrow head.
            HTuple RowP1 = Row1 + (Length - HeadLength) * DR + HalfHeadWidth * DC;
            HTuple ColP1 = Column1 + (Length - HeadLength) * DC - HalfHeadWidth * DR;
            HTuple RowP2 = Row1 + (Length - HeadLength) * DR - HalfHeadWidth * DC;
            HTuple ColP2 = Column1 + (Length - HeadLength) * DC + HalfHeadWidth * DR;
            // Finally create output XLD contour for each input point pair
            for (int Index = 0; Index < Length.Length; Index++)
            {
                if (Length[Index].D == -1)
                    //Create_ single points for arrows with identical start and end point
                    arrows = arrows.ConcatObj(new HXLDCont(Row1[Index], Column1[Index]));
                else
                    // Create arrow contour
                    arrows = arrows.ConcatObj(new HXLDCont(new HTuple(Row1[Index].D, Row2[Index].D, RowP1[Index].D, Row2[Index].D, RowP2[Index].D, Row2[Index].D), new HTuple(Column1[Index].D, Column2[Index].D, ColP1[Index].D, Column2[Index].D, ColP2[Index].D, Column2[Index].D)));
            }
            return arrows;
        }



    }

    [Serializable]
    public class UserHomMat2D
    {
        public double c00 { get; set; } // 第一行第一个
        public double c01 { get; set; }  // 第一行第二个
        public double c02 { get; set; }  // 第一行第三个
        public double c10 { get; set; }  // 第二行第一个
        public double c11 { get; set; }  // 第二行第二个
        public double c12 { get; set; }  // 第二行第三个


        public UserHomMat2D()
        {
            ///////////////////////////////
            this.c00 = 1;
            this.c01 = 0;
            this.c02 = 0;
            this.c10 = 0;
            this.c11 = 1;
            this.c12 = 0;
        }

        public UserHomMat2D(bool isInit = true)
        {
            ///////////////////////////////
            this.c00 = 1;
            this.c01 = 0;
            this.c02 = 0;
            this.c10 = 0;
            this.c11 = 1;
            this.c12 = 0;
        }
        public UserHomMat2D(double c00, double c11)
        {
            ///////////////////////////////
            this.c00 = c00;
            this.c01 = 0;
            this.c02 = 0;
            this.c10 = 0;
            this.c11 = c11;
            this.c12 = 0;
        }

        public UserHomMat2D Clone()
        {
            UserHomMat2D homMat2D = new UserHomMat2D();
            homMat2D.c00 = this.c00;
            homMat2D.c01 = this.c01;
            homMat2D.c02 = this.c02;
            homMat2D.c10 = this.c10;
            homMat2D.c11 = this.c11;
            homMat2D.c12 = this.c12;
            return homMat2D;
        }

        public UserHomMat2D(HHomMat2D hHomMat2D)
        {
            ///////////////////////////////
            switch (hHomMat2D.RawData.Type)
            {
                case HTupleType.DOUBLE:
                    this.c00 = hHomMat2D[0].D;
                    this.c01 = hHomMat2D[1].D;
                    this.c02 = hHomMat2D[2].D;
                    this.c10 = hHomMat2D[3].D;
                    this.c11 = hHomMat2D[4].D;
                    this.c12 = hHomMat2D[5].D;
                    break;
                case HTupleType.INTEGER:
                    this.c00 = hHomMat2D[0].I;
                    this.c01 = hHomMat2D[1].I;
                    this.c02 = hHomMat2D[2].I;
                    this.c10 = hHomMat2D[3].I;
                    this.c11 = hHomMat2D[4].I;
                    this.c12 = hHomMat2D[5].I;
                    break;
                case HTupleType.LONG:
                    this.c00 = hHomMat2D[0].L;
                    this.c01 = hHomMat2D[1].L;
                    this.c02 = hHomMat2D[2].L;
                    this.c10 = hHomMat2D[3].L;
                    this.c11 = hHomMat2D[4].L;
                    this.c12 = hHomMat2D[5].L;
                    break;
                default:
                case HTupleType.MIXED:
                    this.c00 = Convert.ToDouble(hHomMat2D[0].O);
                    this.c01 = Convert.ToDouble(hHomMat2D[1].O);
                    this.c02 = Convert.ToDouble(hHomMat2D[2].O);
                    this.c10 = Convert.ToDouble(hHomMat2D[3].O);
                    this.c11 = Convert.ToDouble(hHomMat2D[4].O);
                    this.c12 = Convert.ToDouble(hHomMat2D[5].O);
                    break;
            }
        }
        public HHomMat2D GetHHomMat()
        {
            HHomMat2D OutMyHomMat = new HHomMat2D(new HTuple(this.c00, this.c01, this.c02, this.c10, this.c11, this.c12));
            return OutMyHomMat;
        }

        /// <summary>
        /// 转换矩阵到相机内外参
        /// </summary>
        /// <param name="Magnification">镜头倍率</param>
        /// <param name="imageWidth">图像宽</param>
        /// <param name="imageHeight">图像高</param>
        /// <param name="camParam">相机内参</param>
        /// <param name="camPose">相机外参</param>
        public void GetCamPose(double Magnification, int imageWidth, int imageHeight, out userCamParam camParam, out userCamPose camPose)
        {
            camParam = new userCamParam();
            camPose = new userCamPose();
            camParam.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
            camParam.Focus = Magnification; // 这个值应该为镜头的倍率
            camParam.Sx = Math.Abs(this.c00);
            camParam.Sy = Math.Abs(this.c11);
            camParam.Cx = imageWidth * 0.5;
            camParam.Cy = imageHeight * 0.5;
            camParam.Width = imageWidth;
            camParam.Height = imageHeight;
            // 利用图像4个角点的世界坐标来计算位姿
            HHomMat2D hHomMat2D = this.GetHHomMat();
            HTuple Qx, Qy, Quality2;
            HTuple Rows = new HTuple(0, imageHeight, imageHeight, 0);
            HTuple Columns = new HTuple(0, 0, imageWidth, imageWidth);
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, camParam.GetHCamPar(), "telecentric_planar_robust", "error", out Quality2);
            camPose = new userCamPose(pose);
            camPose.Tz = 100;
        }
        public HTuple GetHTuple()
        {
            return new HTuple(this.c00, this.c01, this.c02, this.c10, this.c11, this.c12);
        }
        public override string ToString()
        {
            string str = "c00: ";
            str = str + c00.ToString("f8") + "   c01: " + c01.ToString("f8") + "   c02: " + c02.ToString("f8") + "   c10: " +
                  c10.ToString("f8") + "   c11: " + c11.ToString("f8") + "   c12: " + c12.ToString("f8") + "   ";
            return str;
        }

    }

    [Serializable]
    public class UserHomMat3D
    {
        public double c00 { get; set; } // 第一行第一个
        public double c01 { get; set; }  // 第一行第二个
        public double c02 { get; set; }  // 第一行第三个
        public double c10 { get; set; }  // 第二行第一个
        public double c11 { get; set; }  // 第二行第二个
        public double c12 { get; set; }  // 第二行第三个
        public double c20 { get; set; }  // 第三行第一个
        public double c21 { get; set; }  // 第三行第二个
        public double c22 { get; set; }  // 第三行第三个
        public UserHomMat3D()
        {

        }

        public UserHomMat3D(bool isInit = true)
        {
            ///////////////////////////////
            this.c00 = 1;
            this.c01 = 0;
            this.c02 = 0;
            this.c10 = 0;
            this.c11 = 1;
            this.c12 = 0;
            this.c20 = 0;
            this.c21 = 0;
            this.c22 = 1;
        }
        public UserHomMat3D(double c00, double c11, double c22)
        {
            ///////////////////////////////
            this.c00 = c00;
            this.c01 = 0;
            this.c02 = 0;
            this.c10 = 0;
            this.c11 = c11;
            this.c12 = 0;
            this.c20 = 0;
            this.c21 = 0;
            this.c22 = c22;
        }

        public UserHomMat3D Clone()
        {
            UserHomMat3D homMat3D = new UserHomMat3D();
            homMat3D.c00 = this.c00;
            homMat3D.c01 = this.c01;
            homMat3D.c02 = this.c02;
            homMat3D.c10 = this.c10;
            homMat3D.c11 = this.c11;
            homMat3D.c12 = this.c12;
            homMat3D.c20 = this.c20;
            homMat3D.c21 = this.c21;
            homMat3D.c22 = this.c22;
            return homMat3D;
        }

        public UserHomMat3D(HHomMat3D hHomMat3D)
        {
            ///////////////////////////////
            switch (hHomMat3D.RawData.Type)
            {
                case HTupleType.DOUBLE:
                    this.c00 = hHomMat3D[0].D;
                    this.c01 = hHomMat3D[1].D;
                    this.c02 = hHomMat3D[2].D;
                    this.c10 = hHomMat3D[3].D;
                    this.c11 = hHomMat3D[4].D;
                    this.c12 = hHomMat3D[5].D;
                    this.c20 = hHomMat3D[6].D;
                    this.c21 = hHomMat3D[7].D;
                    this.c22 = hHomMat3D[8].D;
                    break;
                case HTupleType.INTEGER:
                    this.c00 = hHomMat3D[0].I;
                    this.c01 = hHomMat3D[1].I;
                    this.c02 = hHomMat3D[2].I;
                    this.c10 = hHomMat3D[3].I;
                    this.c11 = hHomMat3D[4].I;
                    this.c12 = hHomMat3D[5].I;
                    this.c20 = hHomMat3D[6].I;
                    this.c21 = hHomMat3D[7].I;
                    this.c22 = hHomMat3D[8].I;
                    break;
                case HTupleType.LONG:
                    this.c00 = hHomMat3D[0].L;
                    this.c01 = hHomMat3D[1].L;
                    this.c02 = hHomMat3D[2].L;
                    this.c10 = hHomMat3D[3].L;
                    this.c11 = hHomMat3D[4].L;
                    this.c12 = hHomMat3D[5].L;
                    this.c20 = hHomMat3D[6].L;
                    this.c21 = hHomMat3D[7].L;
                    this.c22 = hHomMat3D[8].L;
                    break;
                default:
                case HTupleType.MIXED:
                    this.c00 = Convert.ToDouble(hHomMat3D[0].O);
                    this.c01 = Convert.ToDouble(hHomMat3D[1].O);
                    this.c02 = Convert.ToDouble(hHomMat3D[2].O);
                    this.c10 = Convert.ToDouble(hHomMat3D[3].O);
                    this.c11 = Convert.ToDouble(hHomMat3D[4].O);
                    this.c12 = Convert.ToDouble(hHomMat3D[5].O);
                    this.c20 = Convert.ToDouble(hHomMat3D[6].O);
                    this.c21 = Convert.ToDouble(hHomMat3D[7].O);
                    this.c22 = Convert.ToDouble(hHomMat3D[8].O);
                    break;
            }
        }
        public HHomMat3D GetHHomMat3D()
        {
            HHomMat3D OutMyHomMat = new HHomMat3D(new HTuple(this.c00, this.c01, this.c02, this.c10, this.c11, this.c12, this.c20, this.c21, this.c22));
            return OutMyHomMat;
        }
        public HTuple GetHTuple()
        {
            return new HTuple(this.c00, this.c01, this.c02, this.c10, this.c11, this.c12, this.c20, this.c21, this.c22);
        }
        public override string ToString()
        {
            string str = "c00: ";
            str = str + c00.ToString("f8") + "   c01: " + c01.ToString("f8") + "   c02: " + c02.ToString("f8") + "   c10: " +
                  c10.ToString("f8") + "   c11: " + c11.ToString("f8") + "   c12: " + c12.ToString("f8") + "   c20: " + c20.ToString("f8") + "   c21: " + c21.ToString("f8") + "   c22: " + c22.ToString("f8") + "   ";
            return str;
        }


    }
    [Serializable]
    public class userCamParam
    {
        public string CameraModel;
        public double Focus;
        public double Kappa;
        public double Sx;
        public double Sy;
        public double Cx;
        public double Cy;
        public int Width;
        public int Height;

        public userCamParam()
        {
            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
            this.Focus = 1;
            this.Kappa = 0;
            this.Sx = 1;
            this.Sy = 1;
            this.Cx = 0;
            this.Cy = 0;
            this.Width = 0;
            this.Height = 0;
        }

        /// <summary>
        /// 传入的相机参数一定要包含相机类型
        /// </summary>
        /// <param name="camParam"></param>
        public userCamParam(HTuple camParam)
        {
            if (camParam == null || camParam.Length == 0)
            {
                this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                this.Focus = 1;
                this.Kappa = 0;
                this.Sx = 0.0;
                this.Sy = 0.0;
                this.Cx = 0;
                this.Cy = 0;
                this.Width = 0;
                this.Height = 0;
            }
            else
            {
                switch (camParam.Type)
                {
                    case HTupleType.DOUBLE:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = camParam[1].D;
                            this.Kappa = camParam[2].D;
                            this.Sx = camParam[3].D;
                            this.Sy = camParam[4].D;
                            this.Cx = camParam[5].D;
                            this.Cy = camParam[6].D;
                            this.Width = (int)camParam[7].D;
                            this.Height = (int)camParam[8].D;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = camParam[0].D;
                            this.Kappa = camParam[1].D;
                            this.Sx = camParam[2].D;
                            this.Sy = camParam[3].D;
                            this.Cx = camParam[4].D;
                            this.Cy = camParam[5].D;
                            this.Width = (int)camParam[6].D;
                            this.Height = (int)camParam[7].D;
                        }
                        break;

                    case HTupleType.INTEGER:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = (double)camParam[1].I;
                            this.Kappa = (double)camParam[2].I;
                            this.Sx = (double)camParam[3].I;
                            this.Sy = (double)camParam[4].I;
                            this.Cx = (double)camParam[5].I;
                            this.Cy = (double)camParam[6].I;
                            this.Width = (int)camParam[7].I;
                            this.Height = (int)camParam[8].I;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = (double)camParam[0].I;
                            this.Kappa = (double)camParam[1].I;
                            this.Sx = (double)camParam[2].I;
                            this.Sy = (double)camParam[3].I;
                            this.Cx = (double)camParam[4].I;
                            this.Cy = (double)camParam[5].I;
                            this.Width = (int)camParam[6].I;
                            this.Height = (int)camParam[7].I;
                        }
                        break;

                    case HTupleType.LONG:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = (double)camParam[1].L;
                            this.Kappa = (double)camParam[2].L;
                            this.Sx = (double)camParam[3].L;
                            this.Sy = (double)camParam[4].L;
                            this.Cx = (double)camParam[5].L;
                            this.Cy = (double)camParam[6].L;
                            this.Width = (int)camParam[7].L;
                            this.Height = (int)camParam[8].L;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = (double)camParam[0].L;
                            this.Kappa = (double)camParam[1].L;
                            this.Sx = (double)camParam[2].L;
                            this.Sy = (double)camParam[3].L;
                            this.Cx = (double)camParam[4].L;
                            this.Cy = (double)camParam[5].L;
                            this.Width = (int)camParam[6].L;
                            this.Height = (int)camParam[7].L;
                        }
                        break;

                    case HTupleType.MIXED:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = (double)camParam[1].O;
                            this.Kappa = (double)camParam[2].O;
                            this.Sx = (double)camParam[3].O;
                            this.Sy = (double)camParam[4].O;
                            this.Cx = (double)camParam[5].O;
                            this.Cy = (double)camParam[6].O;
                            this.Width = (int)camParam[7].O;
                            this.Height = (int)camParam[8].O;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = (double)camParam[0].O;
                            this.Kappa = (double)camParam[1].O;
                            this.Sx = (double)camParam[2].O;
                            this.Sy = (double)camParam[3].O;
                            this.Cx = (double)camParam[4].O;
                            this.Cy = (double)camParam[5].O;
                            this.Width = (int)camParam[6].O;
                            this.Height = (int)camParam[7].O;
                        }
                        break;

                    case HTupleType.EMPTY:
                    case HTupleType.STRING:
                    default:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = 0;
                            this.Kappa = 0;
                            this.Sx = 0.0;
                            this.Sy = 0.0;
                            this.Cx = 0;
                            this.Cy = 0;
                            this.Width = 0;
                            this.Height = 0;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = 1;
                            this.Kappa = 0;
                            this.Sx = 0.0;
                            this.Sy = 0.0;
                            this.Cx = 0;
                            this.Cy = 0;
                            this.Width = 0;
                            this.Height = 0;
                        }
                        break;
                }
            }
        }
        public userCamParam(HCamPar camParam)
        {
            if (camParam == null || camParam.RawData.Length == 0)
            {
                this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                this.Focus = 1;
                this.Kappa = 0;
                this.Sx = 0.0;
                this.Sy = 0.0;
                this.Cx = 0;
                this.Cy = 0;
                this.Width = 0;
                this.Height = 0;
            }
            else
            {
                switch (camParam.RawData.Type)
                {
                    case HTupleType.DOUBLE:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = camParam[1].D;
                            this.Kappa = camParam[2].D;
                            this.Sx = camParam[3].D;
                            this.Sy = camParam[4].D;
                            this.Cx = camParam[5].D;
                            this.Cy = camParam[6].D;
                            this.Width = (int)camParam[7].D;
                            this.Height = (int)camParam[8].D;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = camParam[0].D;
                            this.Kappa = camParam[1].D;
                            this.Sx = camParam[2].D;
                            this.Sy = camParam[3].D;
                            this.Cx = camParam[4].D;
                            this.Cy = camParam[5].D;
                            this.Width = (int)camParam[6].D;
                            this.Height = (int)camParam[7].D;
                        }
                        break;

                    case HTupleType.INTEGER:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = (double)camParam[1].I;
                            this.Kappa = (double)camParam[2].I;
                            this.Sx = (double)camParam[3].I;
                            this.Sy = (double)camParam[4].I;
                            this.Cx = (double)camParam[5].I;
                            this.Cy = (double)camParam[6].I;
                            this.Width = (int)camParam[7].I;
                            this.Height = (int)camParam[8].I;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = (double)camParam[0].I;
                            this.Kappa = (double)camParam[1].I;
                            this.Sx = (double)camParam[2].I;
                            this.Sy = (double)camParam[3].I;
                            this.Cx = (double)camParam[4].I;
                            this.Cy = (double)camParam[5].I;
                            this.Width = (int)camParam[6].I;
                            this.Height = (int)camParam[7].I;
                        }
                        break;

                    case HTupleType.LONG:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = (double)camParam[1].L;
                            this.Kappa = (double)camParam[2].L;
                            this.Sx = (double)camParam[3].L;
                            this.Sy = (double)camParam[4].L;
                            this.Cx = (double)camParam[5].L;
                            this.Cy = (double)camParam[6].L;
                            this.Width = (int)camParam[7].L;
                            this.Height = (int)camParam[8].L;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = (double)camParam[0].L;
                            this.Kappa = (double)camParam[1].L;
                            this.Sx = (double)camParam[2].L;
                            this.Sy = (double)camParam[3].L;
                            this.Cx = (double)camParam[4].L;
                            this.Cy = (double)camParam[5].L;
                            this.Width = (int)camParam[6].L;
                            this.Height = (int)camParam[7].L;
                        }
                        break;

                    case HTupleType.MIXED:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = (double)camParam[1].O;
                            this.Kappa = (double)camParam[2].O;
                            this.Sx = (double)camParam[3].O;
                            this.Sy = (double)camParam[4].O;
                            this.Cx = (double)camParam[5].O;
                            this.Cy = (double)camParam[6].O;
                            this.Width = (int)camParam[7].O;
                            this.Height = (int)camParam[8].O;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = (double)camParam[0].O;
                            this.Kappa = (double)camParam[1].O;
                            this.Sx = (double)camParam[2].O;
                            this.Sy = (double)camParam[3].O;
                            this.Cx = (double)camParam[4].O;
                            this.Cy = (double)camParam[5].O;
                            this.Width = (int)camParam[6].O;
                            this.Height = (int)camParam[7].O;
                        }
                        break;

                    case HTupleType.EMPTY:
                    case HTupleType.STRING:
                    default:
                        if (camParam[0].Type == HTupleType.STRING)
                        {
                            this.CameraModel = camParam[0].ToString();
                            this.Focus = 0;
                            this.Kappa = 0;
                            this.Sx = 0.0;
                            this.Sy = 0.0;
                            this.Cx = 0;
                            this.Cy = 0;
                            this.Width = 0;
                            this.Height = 0;
                        }
                        else
                        {
                            this.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                            this.Focus = 1;
                            this.Kappa = 0;
                            this.Sx = 0.0;
                            this.Sy = 0.0;
                            this.Cx = 0;
                            this.Cy = 0;
                            this.Width = 0;
                            this.Height = 0;
                        }
                        break;
                }
            }
        }
        public userCamParam(enCameraModel cameraModel, double Focus, double Kappa, double Sx, double Sy, double Cx, double Cy, int ImageWidth, int ImageHeight)
        {
            this.CameraModel = cameraModel.ToString();
            this.Focus = Focus;
            this.Kappa = Kappa;
            this.Sx = Sx;
            this.Sy = Sy;
            this.Cx = Cx;
            this.Cy = Cy;
            this.Width = ImageWidth;
            this.Height = ImageHeight;
        }
        public userCamParam getUserCamParam(HTuple camParam)
        {
            userCamParam param = new userCamParam();
            if (camParam == null || camParam.Length == 0) return param;
            switch (camParam.Type)
            {
                case HTupleType.DOUBLE:
                    if (camParam[0].Type == HTupleType.STRING)
                    {
                        param.CameraModel = camParam[0].ToString();
                        param.Focus = camParam[1].D;
                        param.Kappa = camParam[2].D;
                        param.Sx = camParam[3].D;
                        param.Sy = camParam[4].D;
                        param.Cx = camParam[5].D;
                        param.Cy = camParam[6].D;
                        param.Width = (int)camParam[7].D;
                        param.Height = (int)camParam[8].D;
                    }
                    else
                    {
                        param.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                        param.Focus = camParam[0].D;
                        param.Kappa = camParam[1].D;
                        param.Sx = camParam[2].D;
                        param.Sy = camParam[3].D;
                        param.Cx = camParam[4].D;
                        param.Cy = camParam[5].D;
                        param.Width = (int)camParam[6].D;
                        param.Height = (int)camParam[7].D;
                    }
                    return param;

                case HTupleType.INTEGER:
                    if (camParam[0].Type == HTupleType.STRING)
                    {
                        param.CameraModel = camParam[0].ToString();
                        param.Focus = (double)camParam[1].I;
                        param.Kappa = (double)camParam[2].I;
                        param.Sx = (double)camParam[3].I;
                        param.Sy = (double)camParam[4].I;
                        param.Cx = (double)camParam[5].I;
                        param.Cy = (double)camParam[6].I;
                        param.Width = (int)camParam[7].I;
                        param.Height = (int)camParam[8].I;
                    }
                    else
                    {
                        param.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                        param.Focus = (double)camParam[0].I;
                        param.Kappa = (double)camParam[1].I;
                        param.Sx = (double)camParam[2].I;
                        param.Sy = (double)camParam[3].I;
                        param.Cx = (double)camParam[4].I;
                        param.Cy = (double)camParam[5].I;
                        param.Width = (int)camParam[6].I;
                        param.Height = (int)camParam[7].I;
                    }
                    return param;

                case HTupleType.LONG:
                    if (camParam[0].Type == HTupleType.STRING)
                    {
                        param.CameraModel = camParam[0].ToString();
                        param.Focus = (double)camParam[1].L;
                        param.Kappa = (double)camParam[2].L;
                        param.Sx = (double)camParam[3].L;
                        param.Sy = (double)camParam[4].L;
                        param.Cx = (double)camParam[5].L;
                        param.Cy = (double)camParam[6].L;
                        param.Width = (int)camParam[7].L;
                        param.Height = (int)camParam[8].L;
                    }
                    else
                    {
                        param.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                        param.Focus = (double)camParam[0].L;
                        param.Kappa = (double)camParam[1].L;
                        param.Sx = (double)camParam[2].L;
                        param.Sy = (double)camParam[3].L;
                        param.Cx = (double)camParam[4].L;
                        param.Cy = (double)camParam[5].L;
                        param.Width = (int)camParam[6].L;
                        param.Height = (int)camParam[7].L;
                    }
                    return param;

                case HTupleType.MIXED:
                    if (camParam[0].Type == HTupleType.STRING)
                    {
                        param.CameraModel = camParam[0].ToString();
                        param.Focus = (double)camParam[1].O;
                        param.Kappa = (double)camParam[2].O;
                        param.Sx = (double)camParam[3].O;
                        param.Sy = (double)camParam[4].O;
                        param.Cx = (double)camParam[5].O;
                        param.Cy = (double)camParam[6].O;
                        param.Width = (int)camParam[7].O;
                        param.Height = (int)camParam[8].O;

                    }
                    else
                    {
                        param.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                        param.Focus = (double)camParam[0].O;
                        param.Kappa = (double)camParam[1].O;
                        param.Sx = (double)camParam[2].O;
                        param.Sy = (double)camParam[3].O;
                        param.Cx = (double)camParam[4].O;
                        param.Cy = (double)camParam[5].O;
                        param.Width = (int)camParam[6].O;
                        param.Height = (int)camParam[7].O;

                    }
                    return param;

                case HTupleType.EMPTY:
                case HTupleType.STRING:
                default:
                    return param;
            }
        }
        public HTuple GetHtuple()
        {
            return new HTuple(this.CameraModel, this.Focus, this.Kappa, this.Sx, this.Sy, this.Cx, this.Cy, this.Width, this.Height);
        }

        public HCamPar GetHCamPar()
        {
            return new HCamPar(GetHtuple());// HTuple(this.CameraModel, this.Focus, this.Kappa, this.Sx, this.Sy, this.Cx, this.Cy, this.Width, this.Height);
        }
        public override string ToString()
        {
            return string.Format("相机内参：CameraModel={0}, Focus={1},Kappa={2},Sx={3},Sy={4},Cx={5},Cy={6},Width={7},Height={8}", this.CameraModel, this.Focus, this.Kappa, this.Sx, this.Sy, this.Cx, this.Cy, this.Width, this.Height);
        }

    }

    [Serializable]
    public class userCamPose
    {
        // 结构体中尽量都用结构体，如果没有结构可用，那么创建一个结构体
        public double Tx;
        public double Ty;
        public double Tz;
        public double Rx;
        public double Ry;
        public double Rz;
        public int Type;

        public userCamPose()
        {

        }
        public userCamPose(HTuple camPose)
        {
            if (camPose == null || camPose.Length < 7)
            {
                this.Tx = 0;
                this.Ty = 0;
                this.Tz = 0;
                this.Rx = 0;
                this.Ry = 0;
                this.Rz = 0;
                this.Type = 0;
            }
            else
            {
                switch (camPose.Type)
                {
                    case HTupleType.DOUBLE:
                        this.Tx = camPose[0].D;
                        this.Ty = camPose[1].D;
                        this.Tz = camPose[2].D;
                        this.Rx = camPose[3].D;
                        this.Ry = camPose[4].D;
                        this.Rz = camPose[5].D;
                        this.Type = (int)camPose[6].D;
                        break;

                    case HTupleType.INTEGER:
                        this.Tx = (double)camPose[0].I;
                        this.Ty = (double)camPose[1].I;
                        this.Tz = (double)camPose[2].I;
                        this.Rx = (double)camPose[3].I;
                        this.Ry = (double)camPose[4].I;
                        this.Rz = (double)camPose[5].I;
                        this.Type = (int)camPose[6].I;
                        break;

                    case HTupleType.LONG:
                        this.Tx = (double)camPose[0].L;
                        this.Ty = (double)camPose[1].L;
                        this.Tz = (double)camPose[2].L;
                        this.Rx = (double)camPose[3].L;
                        this.Ry = (double)camPose[4].L;
                        this.Rz = (double)camPose[5].L;
                        this.Type = (int)camPose[6].L;
                        break;

                    case HTupleType.MIXED:
                        this.Tx = Convert.ToInt32(camPose[0].O);
                        this.Ty = Convert.ToInt32(camPose[1].O);
                        this.Tz = Convert.ToInt32(camPose[2].O);
                        this.Rx = Convert.ToInt32(camPose[3].O);
                        this.Ry = Convert.ToInt32(camPose[4].O);
                        this.Rz = Convert.ToInt32(camPose[5].O);
                        this.Type = Convert.ToInt32(camPose[6].O);
                        break;

                    case HTupleType.EMPTY:
                    case HTupleType.STRING:
                    default:
                        this.Tx = 0;
                        this.Ty = 0;
                        this.Tz = 0;
                        this.Rx = 0;
                        this.Ry = 0;
                        this.Rz = 0;
                        this.Type = 0;
                        break;
                }
            }

        }
        public userCamPose(HPose camPose)
        {
            if (camPose == null || camPose.RawData.Length < 7)
            {
                this.Tx = 0;
                this.Ty = 0;
                this.Tz = 0;
                this.Rx = 0;
                this.Ry = 0;
                this.Rz = 0;
                this.Type = 0;
            }
            else
            {
                HTuple tuple = camPose.RawData;
                switch (tuple.Type)
                {
                    case HTupleType.DOUBLE:
                        this.Tx = camPose[0].D;
                        this.Ty = camPose[1].D;
                        this.Tz = camPose[2].D;
                        this.Rx = camPose[3].D;
                        this.Ry = camPose[4].D;
                        this.Rz = camPose[5].D;
                        this.Type = (int)camPose[6].D;
                        break;

                    case HTupleType.INTEGER:
                        this.Tx = (double)camPose[0].I;
                        this.Ty = (double)camPose[1].I;
                        this.Tz = (double)camPose[2].I;
                        this.Rx = (double)camPose[3].I;
                        this.Ry = (double)camPose[4].I;
                        this.Rz = (double)camPose[5].I;
                        this.Type = (int)camPose[6].I;
                        break;

                    case HTupleType.LONG:
                        this.Tx = (double)camPose[0].L;
                        this.Ty = (double)camPose[1].L;
                        this.Tz = (double)camPose[2].L;
                        this.Rx = (double)camPose[3].L;
                        this.Ry = (double)camPose[4].L;
                        this.Rz = (double)camPose[5].L;
                        this.Type = (int)camPose[6].L;
                        break;

                    case HTupleType.MIXED:
                        this.Tx = Convert.ToInt32(camPose[0].O);
                        this.Ty = Convert.ToInt32(camPose[1].O);
                        this.Tz = Convert.ToInt32(camPose[2].O);
                        this.Rx = Convert.ToInt32(camPose[3].O);
                        this.Ry = Convert.ToInt32(camPose[4].O);
                        this.Rz = Convert.ToInt32(camPose[5].O);
                        this.Type = Convert.ToInt32(camPose[6].O);
                        break;

                    case HTupleType.EMPTY:
                    case HTupleType.STRING:
                    default:
                        this.Tx = 0;
                        this.Ty = 0;
                        this.Tz = 0;
                        this.Rx = 0;
                        this.Ry = 0;
                        this.Rz = 0;
                        this.Type = 0;
                        break;
                }
            }

        }
        public userCamPose(double t_x, double t_y, double t_z, double r_x, double r_y, double r_z, int type = 0)
        {
            this.Tx = t_x;
            this.Ty = t_y;
            this.Tz = t_z;
            this.Rx = r_x;
            this.Ry = r_y;
            this.Rz = r_z;
            this.Type = type;
        }
        public userCamPose getUserCamPose(HTuple camPose)
        {
            userCamPose param = new userCamPose();
            if (camPose == null || camPose.Length < 7) return param;
            switch (camPose.Type)
            {
                case HTupleType.DOUBLE:
                    this.Tx = camPose[0].D;
                    this.Ty = camPose[1].D;
                    this.Tz = camPose[2].D;
                    this.Rx = camPose[3].D;
                    this.Ry = camPose[4].D;
                    this.Rz = camPose[5].D;
                    this.Type = (int)camPose[6].D;
                    return param;

                case HTupleType.INTEGER:
                    this.Tx = (double)camPose[0].I;
                    this.Ty = (double)camPose[1].I;
                    this.Tz = (double)camPose[2].I;
                    this.Rx = (double)camPose[3].I;
                    this.Ry = (double)camPose[4].I;
                    this.Rz = (double)camPose[5].I;
                    this.Type = (int)camPose[6].I;
                    return param;

                case HTupleType.LONG:
                    this.Tx = (double)camPose[0].L;
                    this.Ty = (double)camPose[1].L;
                    this.Tz = (double)camPose[2].L;
                    this.Rx = (double)camPose[3].L;
                    this.Ry = (double)camPose[4].L;
                    this.Rz = (double)camPose[5].L;
                    this.Type = (int)camPose[6].L;
                    return param;

                case HTupleType.MIXED:
                    this.Tx = Convert.ToDouble(camPose[0].O);
                    this.Ty = Convert.ToDouble(camPose[1].O);
                    this.Tz = Convert.ToDouble(camPose[2].O);
                    this.Rx = Convert.ToDouble(camPose[3].O);
                    this.Ry = Convert.ToDouble(camPose[4].O);
                    this.Rz = Convert.ToDouble(camPose[5].O);
                    this.Type = Convert.ToInt32(camPose[6].O);
                    return param;

                case HTupleType.EMPTY:
                case HTupleType.STRING:
                default:
                    return param;
            }
        }
        public userCamPose getUserCamPose(HPose camPose)
        {
            userCamPose param = new userCamPose();
            if (camPose == null || camPose.RawData.Length < 7) return param;
            HTuple tuple = camPose.RawData;
            switch (tuple.Type)
            {
                case HTupleType.DOUBLE:
                    this.Tx = tuple[0].D;
                    this.Ty = tuple[1].D;
                    this.Tz = tuple[2].D;
                    this.Rx = tuple[3].D;
                    this.Ry = tuple[4].D;
                    this.Rz = tuple[5].D;
                    this.Type = (int)tuple[6].D;
                    return param;

                case HTupleType.INTEGER:
                    this.Tx = (double)tuple[0].I;
                    this.Ty = (double)tuple[1].I;
                    this.Tz = (double)tuple[2].I;
                    this.Rx = (double)tuple[3].I;
                    this.Ry = (double)tuple[4].I;
                    this.Rz = (double)tuple[5].I;
                    this.Type = (int)tuple[6].I;
                    return param;

                case HTupleType.LONG:
                    this.Tx = (double)tuple[0].L;
                    this.Ty = (double)tuple[1].L;
                    this.Tz = (double)tuple[2].L;
                    this.Rx = (double)tuple[3].L;
                    this.Ry = (double)tuple[4].L;
                    this.Rz = (double)tuple[5].L;
                    this.Type = (int)tuple[6].L;
                    return param;

                case HTupleType.MIXED:
                    this.Tx = Convert.ToDouble(tuple[0].O);
                    this.Ty = Convert.ToDouble(tuple[1].O);
                    this.Tz = Convert.ToDouble(tuple[2].O);
                    this.Rx = Convert.ToDouble(tuple[3].O);
                    this.Ry = Convert.ToDouble(tuple[4].O);
                    this.Rz = Convert.ToDouble(tuple[5].O);
                    this.Type = Convert.ToInt32(tuple[6].O);
                    return param;

                case HTupleType.EMPTY:
                case HTupleType.STRING:
                default:
                    return param;
            }
        }
        public HTuple GetHtuple()
        {
            return new HTuple(this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type);
        }
        public HPose getHPose()
        {
            return new HPose(new HTuple(this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type));
        }
        public double AffinePoint3D(string axis, double value)
        {
            HHomMat3D hHomMat3D = new HHomMat3D(this.GetHtuple());
            double Qx, Qy, Qz, affineValue = value;
            switch (axis)
            {
                case "X":
                case "x":
                default:
                    Qx = hHomMat3D.AffineTransPoint3d(0, value, 0, out Qy, out Qz);
                    affineValue = Qy;
                    break;
                case "Y":
                case "y":
                    Qx = hHomMat3D.AffineTransPoint3d(value, 0, 0, out Qy, out Qz);
                    affineValue = Qz;
                    break;
            }
            return affineValue;
        }
        public override string ToString()
        {
            return string.Format("位姿坐标：Tx={0},Ty={1},Tz={2},Rx={3},Ry={4},Rz={5},Type={6}", this.Tx, this.Ty, this.Tz, this.Rx, this.Ry, this.Rz, this.Type);
        }


    }

    [Serializable]
    public struct userFaceRoughness
    {
        public double Sq; // 均方根高度
        public double Ssk; // 偏斜度
        public double Sku; // 峰度
        public double Sp; // 最大波峰高度
        public double Sv; // 最大凹陷高度
        public double Sz; // 最大高度
        public double Sa; // 算术平均高度

        // 结构体中尽量都用结构体，如果没有结构可用，那么创建一个结构体
        public override string ToString()
        {
            return string.Format("位姿坐标：Sq={0},Ssk={1},Sku={2},Sp={3},Sv={4},Sz={5},Sa={6}", this.Sa, this.Sz, this.Sv, this.Sp, this.Sku, this.Ssk, this.Sq);
        }

    }
    [Serializable]
    public struct userLineRoughness
    {
        public double Rp; // 粗糙轮廓的最大顶点高度
        public double Rv; // 粗糙轮廓的最大波谷深度
        public double Rz; // 粗糙轮廓的最大高度
        public double Rc; // 粗糙轮廓的元素的平均高度
        public double Rt; // 粗糙轮廓的总高度
        public double Ra; // 粗糙轮廓的算术平均偏差
        public double Rq; // 粗度轮廓的均方根偏差
        public double Rsk; // 粗度轮廓的斜度
        public double Rku; // 粗度轮廓的峰度

        // 结构体中尽量都用结构体，如果没有结构可用，那么创建一个结构体
        public override string ToString()
        {
            return string.Format("位姿坐标：Rp={0},Rv={1},Rz={2},Rc={3},Rt={4},Ra={5},Rq={6},Rsk={5},Rku={6}", this.Rp, this.Rv, this.Rz, this.Rc, this.Rt, this.Ra, this.Rq, this.Rsk, this.Rku);
        }

    }

    [Serializable]
    public class userTextLable
    {
        public string Text { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public enLablePosition LablePose { get; set; }

        [NonSerialized]
        public CameraParam _camParam;
        public CameraParam CamParam { get => this._camParam; set => this._camParam = value; }
        // 结构体中尽量都用结构体，如果没有结构可用，那么创建一个结构体
        public userTextLable(string text, string color)
        {
            this.Text = text;
            this.X = 0;
            this.Y = 0;
            this.Size = 10;
            this.Color = color;
            this.CamParam = null;
            this.LablePose = enLablePosition.用户定义;
        }
        public userTextLable(string text, double x, double y, int size, string color)
        {
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.Size = size;
            this.Color = color;
            this.CamParam = null;
            this.LablePose = enLablePosition.用户定义;
        }
        public userTextLable(string text, double x, double y, int size, string color, enLablePosition pose)
        {
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.Size = size;
            this.Color = color;
            this.CamParam = null;
            this.LablePose = pose;
        }
        public userTextLable(string text, double x, double y, int size, string color, CameraParam camParam)
        {
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.Size = size;
            this.Color = color;
            this.CamParam = camParam;
            this.LablePose = enLablePosition.用户定义;
        }
        public void WriteString(HWindow window)
        {
            double row = 0, col = 0;
            if (this.CamParam == null)
                this.CamParam = new CameraParam();
            //this.camParam.WorldPointsToImagePlane(this.x, this.y, 0, 0, out row, out col);
            // 用像素坐标
            row = this.Y;
            col = this.X;
            string[] item = this.Text.Split(new char[] { '\n', '\r', '\t', ',' });
            int row1, col1, row2, col2;
            window.GetPart(out row1, out col1, out row2, out col2); // 获取视图区域
            switch (this.LablePose)
            {
                default:
                case enLablePosition.用户定义:
                    break;
                case enLablePosition.左上角:
                    row += row1 + 200;
                    break;
                case enLablePosition.右上角:
                    row += row1 + 200;
                    break;
                case enLablePosition.左下角:
                    row += row2 - 200;
                    break;
                case enLablePosition.右下角:
                    row += row2 - 200;
                    break;
            }
            for (int i = 0; i < item.Length; i++)
            {
                window.SetTposition((int)(row + this.Size * 11 * i), (int)col);
                window.SetFont("-Consolas-" + this.Size.ToString() + "- *-0-*-*-1-");
                window.SetColor(this.Color);
                window.WriteString(item[i]);
            }
            //window.DispText(this.text, "image", row, col, this.color, new HTuple("box"), new HTuple("false")); //image  window
        }


    }



}
[Serializable]
public struct userOkNgText
{
    public string text;
    public int row_pos;
    public int col_pos;
    public int size;
    // 结构体中尽量都用结构体，如果没有结构可用，那么创建一个结构体
    public userOkNgText(string text)
    {
        this.text = text;
        this.row_pos = 0;
        this.col_pos = 0;
        this.size = 10;
    }
    public userOkNgText(string text, int row_pos, int col_pos, int size)
    {
        this.text = text;
        this.row_pos = row_pos;
        this.col_pos = col_pos;
        this.size = size;
    }
    public void WriteString(HWindow window, int row_pos, int col_pos, int size)
    {
        switch (text)
        {
            default:
            case "OK":
                //window.SetTposition(row_pos, col_pos);
                window.SetFont("-Consolas-" + size.ToString() + "- *-0-*-*-1-");
                //window.SetColor("green");
                //window.WriteString("OK");
                window.DispText(this.text, "window", "top", "right", "green", new HTuple("box"), new HTuple("false")); //image  window
                break;
            case "NG":
                //window.SetTposition(row_pos, col_pos);
                window.SetFont("-Consolas-" + size.ToString() + "- *-0-*-*-1-");
                //window.SetColor("red");
                //window.WriteString("NG");
                window.DispText(this.text, "window", "top", "right", "red", new HTuple("box"), new HTuple("false")); //image  window
                break;
            case "Waitting":
                //window.SetTposition(row_pos, col_pos);
                window.SetFont("-Consolas-" + size.ToString() + "- *-0-*-*-1-");
                //window.SetColor("yellow");
                //window.WriteString("Waitting");
                window.DispText(this.text, "window", "top", "right", "red", new HTuple("box"), new HTuple("false")); //image  window
                break;
        }

    }
    public override string ToString()
    {
        return string.Format("位姿坐标：color={0}", text);
    }


}

[Serializable]
public class userWcsBox : WcsData
{
    public double X;
    public double Y;
    public double Z;
    public double AxisLength_x;
    public double AxisLength_y;
    public double AxisLength_z;
    public userWcsPose WcsPose;

    public userWcsBox()
    {
        this.X = 0;
        this.Y = 0;
        this.Z = 0;
        this.AxisLength_x = 0;
        this.AxisLength_y = 0;
        this.AxisLength_z = 0;
        this.WcsPose = new userWcsPose();
    }
    public userWcsBox(double x, double y, double z, double xAxisLength, double yAxisLength, double zAxisLength)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.AxisLength_x = xAxisLength;
        this.AxisLength_y = yAxisLength;
        this.AxisLength_z = zAxisLength;
        this.WcsPose = new userWcsPose();
    }
    public userWcsBox(double x, double y, double z, double xAxisLength, double yAxisLength, double zAxisLength, userWcsPose wcsPose)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.AxisLength_x = xAxisLength;
        this.AxisLength_y = yAxisLength;
        this.AxisLength_z = zAxisLength;
        this.WcsPose = wcsPose;
    }


}

[Serializable]
public class userWcsCylinder : WcsData
{
    public double X;
    public double Y;
    public double Z;
    public double Radius;
    public userWcsPose WcsPose;
    public userWcsCylinder()
    {
        this.X = 0;
        this.Y = 0;
        this.Z = 0;
        this.Radius = 0;
        this.WcsPose = new userWcsPose();
    }
    public userWcsCylinder(double x, double y, double z, double radius)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Radius = radius;
        this.WcsPose = new userWcsPose();
    }
    public userWcsCylinder(double x, double y, double z, double radius, userWcsPose wcsPose)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Radius = radius;
        this.WcsPose = wcsPose;
    }

}

[Serializable]
public class userWcsSphere : WcsData
{
    public double X;
    public double Y;
    public double Z;
    public double Radius;
    public userWcsPose WcsPose;
    public userWcsSphere()
    {
        this.X = 0;
        this.Y = 0;
        this.Z = 0;
        this.Radius = 0;
        this.WcsPose = new userWcsPose();
    }
    public userWcsSphere(double x, double y, double z, double radius)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Radius = radius;
        this.WcsPose = new userWcsPose();
    }
    public userWcsSphere(double x, double y, double z, double radius, userWcsPose wcsPose)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Radius = radius;
        this.WcsPose = wcsPose;
    }


}

[Serializable]
public class userWcsPlane : WcsData
{
    public double X;
    public double Y;
    public double Z;
    public userWcsPose WcsPose;

    public userWcsPlane()
    {
        this.X = 0;
        this.Y = 0;
        this.Z = 0;
        this.WcsPose = new userWcsPose();
    }
    public userWcsPlane(double x, double y, double z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.WcsPose = new userWcsPose();
    }
    public userWcsPlane(double x, double y, double z, userWcsPose wcsPose)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.WcsPose = wcsPose;
    }


}

[Serializable]
public class userLaserCalibrateParam
{
    public string LaserType;
    public userWcsPose LaserPose; // 用于描述激光的安装位姿
    public userWcsPose LaserAffinePose { set; get; } // 用于描述激光相对于参考对象的变换位姿
    public userLaserCalibrateParam(userWcsPose affinePose, string LaserType, userWcsPose laserPose)
    {
        //this.Tx = t_x;
        //this.Ty = t_y;
        //this.Tz = t_z;
        //this.Rx = r_x;
        //this.Ry = r_y;
        //this.Rz = r_z;
        this.LaserAffinePose = affinePose;
        this.LaserType = LaserType;
        this.LaserPose = laserPose;
    }
    public userLaserCalibrateParam()
    {

    }

    public bool Save(string fileName)
    {
        bool IsOk = true;
        if (!DirectoryEx.Exist(fileName)) DirectoryEx.Create(fileName);
        if (Path.GetExtension(fileName) == ".xml")
            IsOk = IsOk && XML<userLaserCalibrateParam>.Save(this, fileName);
        else
            IsOk = IsOk && XML<userLaserCalibrateParam>.Save(this, Path.GetDirectoryName(fileName) + "\\" + Path.GetFileName(fileName) + ".xml");
        return IsOk;
    }
    public userLaserCalibrateParam Read(string fileName)
    {
        if (File.Exists(fileName + ".xml"))
            return XML<userLaserCalibrateParam>.Read(fileName + ".xml");
        else
            return new userLaserCalibrateParam();
    }
}



[Serializable]
public class drawWcsRect2 : WcsROI
{
    [DisplayNameAttribute("X坐标")]
    public double X { get; set; }

    [DisplayNameAttribute("Y坐标")]
    public double Y { get; set; }

    [DisplayNameAttribute("Z坐标")]
    public double Z { get; set; }

    [DisplayNameAttribute("角度")]
    public double Deg { get; set; }

    [DisplayNameAttribute("长度1")]
    public double Length1 { get; set; }

    [DisplayNameAttribute("长度2")]
    public double Length2 { get; set; }
    public drawWcsRect2()
    {

    }
    public drawWcsRect2(double X, double Y, double Z, double deg, double Length1, double Length2)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.Deg = deg;
        this.Length1 = Length1;
        this.Length2 = Length2;
    }

    public drawPixRect2 GetPixRect2(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        double row = 0, col = 0, pixLength1 = 0, pixLength2 = 0, rad = 0;
        rad = new HTuple(this.Deg).TupleRad().D;
        CamParams?.WorldPointsToImagePlane(this.X, this.Y, grab_x, grab_y, out row, out col);
        pixLength1 = CamParams.TransWcsLengthToPixLength(this.Length1);
        pixLength2 = CamParams.TransWcsLengthToPixLength(this.Length2);
        drawPixRect2 pixRect2 = new drawPixRect2(row, col, rad, pixLength1, pixLength2);
        return pixRect2;
    }

    public drawWcsRect2 AffineTransWcsRect2(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, x, y;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        // 变换中心点
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out x, out y);
        // 变换角度
        double sx, sy, phi, theta, tx, ty;
        HHomMat2D hHomMat2D = new HHomMat2D();
        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, new HTuple(this.Deg).TupleRad() + Phi);
        sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
        ////////////////////////////////////////////////////////
        drawWcsRect2 wcsRect2 = new drawWcsRect2(x[0].D, y[0].D, this.Z, new HTuple(phi).TupleDeg().D, this.Length1, this.Length2);
        return wcsRect2;
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = this.X;
        y = this.Y;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsRect2(homMat2DWcs);
    }
    public override string ToString()
    {
        return string.Join(",", this.X, this.Y, this.Deg, this.Length1, this.Length2);
    }
    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixRect2(camera);
    }

}

[Serializable]
public class drawWcsRect1 : WcsROI
{
    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }


    public drawWcsRect1()
    {

    }

    public drawWcsRect1(double x1, double y1, double x2, double y2)
    {
        this.X1 = x1;
        this.Y1 = y1;
        this.X2 = x2;
        this.Y2 = y2;
    }
    public drawWcsRect1 AffineTransWcsRect1(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X1, this.X2), new HTuple(this.Y1, this.Y2), out Qx, out Qy);
        drawWcsRect1 WcsRect1 = new drawWcsRect1(Qx[0].D, Qy[0].D, Qx[1].D, Qy[1].D);
        return WcsRect1;
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = (this.X1 + this.X2) * 0.5;
        y = (this.Y1 + this.Y2) * 0.5;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsRect1(homMat2DWcs);
    }
    public drawPixRect1 GetPixRect1(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        drawPixRect1 pixRect1;
        double row1 = 0, col1 = 0, row2 = 0, col2 = 0;
        CamParams?.WorldPointsToImagePlane(this.X1, this.Y1, grab_x, grab_y, out row1, out col1);
        CamParams?.WorldPointsToImagePlane(this.X2, this.Y2, grab_x, grab_y, out row2, out col2);
        pixRect1 = new drawPixRect1(row1, col1, row2, col2);
        return pixRect1;
    }
    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixRect1(camera);
    }

    public override string ToString()
    {
        return string.Join(",", this.X1, this.Y1, this.X2, this.Y2);
    }
}

[Serializable]
public class drawWcsLine : WcsROI
{
    [DisplayNameAttribute("X1坐标")]
    public double X1 { get; set; }

    [DisplayNameAttribute("Y1坐标")]
    public double Y1 { get; set; }

    [DisplayNameAttribute("Z1坐标")]
    public double Z1 { get; set; }

    [DisplayNameAttribute("X2坐标")]
    public double X2 { get; set; }

    [DisplayNameAttribute("Y2坐标")]
    public double Y2 { get; set; }

    [DisplayNameAttribute("Z2坐标")]
    public double Z2 { get; set; }
    public drawWcsLine()
    {
        this.X1 = 0;
        this.Y1 = 0;
        this.Z1 = 0;
        this.X2 = 0;
        this.Y2 = 0;
        this.Z2 = 0;
    }
    public drawWcsLine(double x1, double y1, double z1, double x2, double y2, double z2)
    {
        this.X1 = x1;
        this.Y1 = y1;
        this.Z1 = z1;
        this.X2 = x2;
        this.Y2 = y2;
        this.Z2 = z2;
    }

    public drawPixLine GetPixLine(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        drawPixLine pixLine;
        double row1 = 0, col1 = 0, row2 = 0, col2 = 0;
        CamParams?.WorldPointsToImagePlane(this.X1, this.Y1, grab_x, grab_y, out row1, out col1);
        CamParams?.WorldPointsToImagePlane(this.X2, this.Y2, grab_x, grab_y, out row2, out col2);
        pixLine = new drawPixLine(row1, col1, row2, col2);
        return pixLine;
    }
    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixLine(camera);
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = (this.X1 + this.X2) * 0.5;
        y = (this.Y1 + this.Y2) * 0.5;
    }
    public drawWcsLine AffineTransWcsLine(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X1, this.X2), new HTuple(this.Y1, this.Y2), out Qx, out Qy);
        drawWcsLine WcsLine = new drawWcsLine(Qx[0].D, Qy[0].D, this.Z1, Qx[1].D, Qy[1].D, this.Z2);
        return WcsLine;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsLine(homMat2DWcs);
    }

    public override string ToString()
    {
        return string.Join(",", this.X1, this.Y1, this.Z1, this.X2, this.Y2, this.Z2);
    }
}
[Serializable]
public class drawWcsCircle : WcsROI
{
    [DisplayNameAttribute("X坐标")]
    public double X { get; set; }

    [DisplayNameAttribute("Y坐标")]
    public double Y { get; set; }

    [DisplayNameAttribute("Z坐标")]
    public double Z { get; set; }

    [DisplayNameAttribute("半径")]
    public double Radius { get; set; }


    public drawWcsCircle()
    {
        this.X = 0;
        this.Y = 0;
        this.Z = 0;
        this.Radius = 0;
    }
    public drawWcsCircle(double x, double y, double z, double radius)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Radius = radius;
    }

    public drawPixCircle GetPixCircle(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        double row = 0, col = 0, pixRadius = 0;
        if (CamParams == null) throw new ArgumentNullException("CamParams");
        CamParams?.WorldPointsToImagePlane(this.X, this.Y, grab_x, grab_y, out row, out col);
        pixRadius = CamParams.TransWcsLengthToPixLength(this.Radius);
        drawPixCircle pixCircle = new drawPixCircle(row, col, pixRadius);
        return pixCircle;
    }
    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixCircle(camera);
    }
    public drawWcsCircle AffineTransWcsCircle(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
        drawWcsCircle WcsCircle = new drawWcsCircle(Qx[0].D, Qy[0].D, this.Z, this.Radius);
        return WcsCircle;
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = this.X;
        y = this.Y;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsCircle(homMat2DWcs);
    }
    public override string ToString()
    {
        return string.Join(",", this.X, this.Y, this.Z, this.Radius);
    }
}
[Serializable]
public class drawWcsPoint : WcsROI
{
    [DisplayNameAttribute("X坐标")]
    public double X { get; set; }

    [DisplayNameAttribute("Y坐标")]
    public double Y { get; set; }

    [DisplayNameAttribute("Z坐标")]
    public double Z { get; set; }


    public drawWcsPoint()
    {

    }

    public drawWcsPoint(double x, double y, double z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    public drawPixPoint GetPixPoint(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        double row = 0, col = 0;
        if (CamParams == null) throw new ArgumentNullException("CamParams");
        CamParams?.WorldPointsToImagePlane(this.X, this.Y, grab_x, grab_y, out row, out col);
        drawPixPoint pixPoint = new drawPixPoint(row, col);
        return pixPoint;
    }

    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixPoint(camera);
    }
    public drawWcsPoint AffineTransWcsPoint(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
        drawWcsPoint WcsPoint = new drawWcsPoint(Qx[0].D, Qy[0].D, this.Z);
        return WcsPoint;
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = this.X;
        y = this.Y;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsPoint(homMat2DWcs);
    }
    public override string ToString()
    {
        return string.Join(",", this.X, this.Y, this.Z);
    }

}

[Serializable]
public class drawWcsEllipse : WcsROI
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Deg { get; set; }
    public double Radius1 { get; set; }
    public double Radius2 { get; set; }
    public drawWcsEllipse()
    {
        this.X = 0;
        this.Y = 0;
        this.Deg = 0;
        this.Radius1 = 0;
        this.Radius2 = 0;
    }
    public drawWcsEllipse(double x, double y, double deg, double radius1, double radius2)
    {
        this.X = x;
        this.Y = y;
        this.Deg = deg;
        this.Radius1 = radius1;
        this.Radius2 = radius2;
    }
    public drawWcsEllipse AffineTransWcsCircle(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X), new HTuple(this.Y), out Qx, out Qy);
        drawWcsEllipse EllipseCircle = new drawWcsEllipse(Qx[0].D, Qy[0].D, this.Deg, this.Radius1, this.Radius2);
        return EllipseCircle;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsCircle(homMat2DWcs);
    }
    public drawPixEllipse GetPixEllipse(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        double row = 0, col = 0, pixRadius1 = 0, pixRadius2 = 0;
        if (CamParams == null) throw new ArgumentNullException("CamParams");
        CamParams?.WorldPointsToImagePlane(this.X, this.Y, grab_x, grab_y, out row, out col);
        pixRadius1 = CamParams.TransWcsLengthToPixLength(this.Radius1);
        pixRadius2 = CamParams.TransWcsLengthToPixLength(this.Radius2);
        drawPixEllipse pixCircle = new drawPixEllipse(row, col, this.Deg * Math.PI / 180, pixRadius1, pixRadius2);
        return pixCircle;
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = this.X;
        y = this.Y;
    }
    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixEllipse(camera);
    }
    //public override ROI GetWcsObject(CameraParam camera)
    //{
    //    return this;
    //}
    public override string ToString()
    {
        return string.Join(",", this.X, this.Y, this.Deg, this.Radius1, this.Radius2);
    }

}

[Serializable]
public class drawWcsPolygon : WcsROI
{
    public List<double> X { get; set; }
    public List<double> Y { get; set; }

    public drawWcsPolygon()
    {
        this.X = new List<double>();
        this.Y = new List<double>();
    }

    public drawWcsPolygon(double[] x, double[] y)
    {
        this.X = new List<double>();
        this.Y = new List<double>();
        this.X.AddRange(x);
        this.Y.AddRange(y);
    }
    public drawWcsPolygon AffineTransWcsPolygon(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X.ToArray()), new HTuple(this.Y.ToArray()), out Qx, out Qy);
        drawWcsPolygon PixRect1 = new drawWcsPolygon(Qx.DArr, Qy.DArr);
        return PixRect1;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsPolygon(homMat2DWcs);
    }
    public drawPixPolygon GetPixPolygon(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        HTuple row = 0, col = 0;
        if (CamParams == null) throw new ArgumentNullException("CamParams");
        CamParams?.WorldPointsToImagePlane(this.X.ToArray(), this.Y.ToArray(), grab_x, grab_y, out row, out col);
        drawPixPolygon pixCircle = new drawPixPolygon(row.DArr, col.DArr);
        return pixCircle;
    }
    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixPolygon(camera);
    }

    public double GetCenter_X()
    {
        return this.X.Average();
    }

    public double GetCenter_Y()
    {
        return this.Y.Average();
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = 0;
        y = 0;
        if (this.X.Count > 0)
            x = this.X.Average();
        if (this.Y.Count > 0)
            y = this.Y.Average();
    }
    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < this.X.Count; i++)
        {
            str += Math.Round(this.X[i], 3) + "," + Math.Round(this.Y[i], 3);

        }
        return str; // string.Join(",", this.Row, this.Col);
        //return string.Join(",", this.X, this.Y);
    }

}

[Serializable]
public class drawWcsPolyLine : WcsROI
{
    public List<double> X { get; set; }
    public List<double> Y { get; set; }
    public drawWcsPolyLine()
    {
        this.X = new List<double>();
        this.Y = new List<double>();
    }

    public drawWcsPolyLine(double[] x, double[] y)
    {
        this.X = new List<double>();
        this.Y = new List<double>();
        this.X.AddRange(x);
        this.Y.AddRange(y);
    }
    public drawWcsPolyLine AffineTransWcsPolygon(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.X.ToArray()), new HTuple(this.Y.ToArray()), out Qx, out Qy);
        drawWcsPolyLine PixRect1 = new drawWcsPolyLine(Qx.DArr, Qy.DArr);
        return PixRect1;
    }
    public override WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        return AffineTransWcsPolygon(homMat2DWcs);
    }
    public drawPixPolyLine GetPixPolygon(CameraParam CamParams, double grab_x = 0, double grab_y = 0)
    {
        HTuple row = 0, col = 0;
        if (CamParams == null) throw new ArgumentNullException("CamParams");
        CamParams?.WorldPointsToImagePlane(this.X.ToArray(), this.Y.ToArray(), grab_x, grab_y, out row, out col);
        drawPixPolyLine pixCircle = new drawPixPolyLine(row.DArr, col.DArr);
        return pixCircle;
    }
    public override PixROI GetPixROI(CameraParam camera)
    {
        return GetPixPolygon(camera);
    }
    public override void GetRoiCenter(out double x, out double y)
    {
        x = 0;
        y = 0;
        if (this.X.Count > 0)
            x = this.X.Average();
        if (this.Y.Count > 0)
            y = this.Y.Average();
    }
    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < this.X.Count; i++)
        {
            str += Math.Round(this.X[i], 3) + "," + Math.Round(this.Y[i], 3);

        }
        return str; // string.Join(",", this.Row, this.Col);
        //return string.Join(",", this.X, this.Y);
    }

}
/// <summary>
///  绘图基类
/// </summary>
[Serializable]
public class ROI
{
    //public virtual ROI GetPixObject(CameraParam camera = null)
    //{
    //    throw new NotImplementedException();
    //}
    //public virtual ROI GetWcsObject(CameraParam camera = null)
    //{
    //    throw new NotImplementedException();
    //}
    //public virtual HXLDCont GetXLD()
    //{
    //    throw new NotImplementedException();
    //}
    //public virtual HRegion GetRegion()
    //{
    //    throw new NotImplementedException();
    //}
}

[Serializable]
public class PixROI : ROI
{
    public virtual WcsROI GetWcsROI(CameraParam camera = null)
    {
        throw new NotImplementedException();
    }
    public virtual HXLDCont GetXLD()
    {
        throw new NotImplementedException();
    }
    public virtual HRegion GetRegion()
    {
        throw new NotImplementedException();
    }
    public virtual PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        throw new NotImplementedException();
    }
    public virtual void GetRoiCenter(out double row, out double col)
    {
        throw new NotImplementedException();
    }

}
[Serializable]
public class WcsROI : ROI
{
    public virtual PixROI GetPixROI(CameraParam camera)
    {
        throw new NotImplementedException();
    }

    public virtual WcsROI AffineTransWcsROI(HHomMat2D homMat2DWcs)
    {
        throw new NotImplementedException();
    }
    public virtual void GetRoiCenter(out double x, out double y)
    {
        throw new NotImplementedException();
    }
}
[Serializable]
public class drawPixPoint : PixROI
{
    [DisplayNameAttribute("行坐标")]
    public double Row { get; set; }

    [DisplayNameAttribute("列坐标")]
    public double Col { get; set; }




    public drawPixPoint()
    {

    }

    public drawPixPoint(double row, double col)
    {
        this.Row = row;
        this.Col = col;
    }

    public drawPixPoint AffinePixPoint(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
        drawPixPoint PixLine = new drawPixPoint(Qx[0].D, Qy[0].D);
        return PixLine;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffinePixPoint(homMat2D.RawData);
    }
    public override void GetRoiCenter(out double row, out double col)
    {
        row = this.Row;
        col = this.Col;
    }

    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        hXLDCont.GenCrossContourXld(this.Row, this.Col, 15, 0);
        return hXLDCont;
    }
    public drawWcsPoint GetWcsPoint(CameraParam CamParams, double grab_x = 0, double grab_y = 0, double grab_z = 0)
    {
        drawWcsPoint wcsPoint;
        HTuple Qx = 0, Qy = 0, Qz = 0;
        CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row), new HTuple(this.Col), grab_x, grab_y, grab_z, out Qx, out Qy, out Qz);
        wcsPoint = new drawWcsPoint(Qx[0].D, Qy[0].D, 0);
        return wcsPoint;
    }
    public override HRegion GetRegion()
    {
        HRegion hRegion = new HRegion();
        hRegion.GenRegionPoints(this.Row, this.Col);
        return hRegion;
    }

    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsPoint(camera);
    }
    public override string ToString()
    {
        return string.Join(",", this.Row, this.Col);
    }
}

[Serializable]
public class drawPixLine : PixROI
{
    [DisplayNameAttribute("Row1坐标")]
    public double Row1 { get; set; }

    [DisplayNameAttribute("Col1坐标")]
    public double Col1 { get; set; }

    [DisplayNameAttribute("Row2坐标")]
    public double Row2 { get; set; }

    [DisplayNameAttribute("Col2坐标")]
    public double Col2 { get; set; }


    public drawPixLine(double row1, double col1, double row2, double col2)
    {
        this.Row1 = row1;
        this.Col1 = col1;
        this.Row2 = row2;
        this.Col2 = col2;
    }
    public drawPixLine AffinePixLine(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), out Qx, out Qy);
        drawPixLine PixLine = new drawPixLine(Qx[0].D, Qy[0].D, Qx[1].D, Qy[1].D);
        return PixLine;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffinePixLine(homMat2D.RawData);
    }
    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        hXLDCont.GenContourPolygonXld(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2));
        return hXLDCont;
    }
    public drawWcsLine GetWcsLine(CameraParam CamParams, double grab_x = 0, double grab_y = 0, double grab_z = 0)
    {
        drawWcsLine wcsLine;
        HTuple Qx = 0, Qy = 0, Qz = 0;
        CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), grab_x, grab_y, grab_z, out Qx, out Qy, out Qz);
        wcsLine = new drawWcsLine(Qx[0].D, Qy[0].D, 0, Qx[1].D, Qy[1].D, 0);
        return wcsLine;
    }
    //public override ROI GetPixObject(CameraParam camera)
    //{
    //    return this;
    //}
    public override HRegion GetRegion()
    {
        HRegion hRegion = new HRegion();
        hRegion.GenRegionLine(this.Row1, this.Col1, this.Row2, this.Col2);
        return hRegion;
    }
    public override void GetRoiCenter(out double row, out double col)
    {
        row = (this.Row1 + this.Row2) * 0.5;
        col = (this.Col1 + this.Col2) * 0.5;
    }
    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsLine(camera);
    }
    public override string ToString()
    {
        return string.Join(",", this.Row1, this.Col1, this.Row2, this.Col2);
    }
}

[Serializable]
public class drawPixCircle : PixROI
{
    [DisplayNameAttribute("行坐标")]
    public double Row { get; set; }
    [DisplayNameAttribute("列坐标")]
    public double Col { get; set; }
    [DisplayNameAttribute("半径")]
    public double Radius { get; set; }



    public drawPixCircle(double row, double col, double radius)
    {
        this.Row = row;
        this.Col = col;
        this.Radius = radius;
    }
    public drawPixCircle AffinePixCircle(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
        drawPixCircle PixCircle = new drawPixCircle(Qx[0].D, Qy[0].D, this.Radius);
        return PixCircle;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffinePixCircle(homMat2D.RawData);
    }
    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        hXLDCont.GenCircleContourXld(this.Row, this.Col, this.Radius, 0, Math.PI * 2, "positive", 0.005);
        return hXLDCont;
    }
    public drawWcsCircle GetWcsCircle(CameraParam CamParams, double grab_x = 0, double grab_y = 0, double grab_z = 0)
    {
        drawWcsCircle wcsLine;
        HTuple Qx = 0, Qy = 0, Qz = 0;
        CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row), new HTuple(this.Col), grab_x, grab_y, grab_z, out Qx, out Qy, out Qz);
        double radius = CamParams.TransPixLengthToWcsLength(this.Radius);
        wcsLine = new drawWcsCircle(Qx[0].D, Qy[0].D, 0, radius);
        return wcsLine;
    }
    public override HRegion GetRegion()
    {
        HRegion hRegion = new HRegion();
        hRegion.GenCircle(this.Row, this.Col, this.Radius);
        return hRegion;
    }
    public override void GetRoiCenter(out double row, out double col)
    {
        row = (this.Row);
        col = (this.Col);
    }
    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsCircle(camera);
    }
    public override string ToString()
    {
        return string.Join(",", this.Row, this.Col, this.Radius);
    }
}

[Serializable]
public class drawPixEllipse : PixROI
{
    [DisplayNameAttribute("行坐标")]
    public double Row { get; set; }

    [DisplayNameAttribute("列坐标")]
    public double Col { get; set; }

    [DisplayNameAttribute("角度")]
    public double Rad { get; set; }

    [DisplayNameAttribute("半径1")]
    public double Radius1 { get; set; }

    [DisplayNameAttribute("半径2")]
    public double Radius2 { get; set; }

    public drawPixEllipse(double row, double col, double rad, double radius1, double radius2)
    {
        this.Row = row;
        this.Col = col;
        this.Rad = rad;
        this.Radius1 = radius1;
        this.Radius2 = radius2;
    }
    public drawPixEllipse AffinePixEllipse(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row), new HTuple(this.Col), out Qx, out Qy);
        drawPixEllipse PixCircle = new drawPixEllipse(Qx[0].D, Qy[0].D, this.Rad, this.Radius1, this.Radius2);
        return PixCircle;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffinePixEllipse(homMat2D.RawData);
    }

    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        hXLDCont.GenEllipseContourXld(this.Row, this.Col, this.Rad, this.Radius1, this.Radius2, 0, Math.PI * 2, "positive", 0.005);
        return hXLDCont;
    }

    public drawWcsEllipse GetWcsCircle(CameraParam CamParams, double grab_x = 0, double grab_y = 0, double grab_z = 0)
    {
        drawWcsEllipse wcsLine;
        HTuple Qx = 0, Qy = 0, Qz = 0;
        CamParams?.ImagePointsToWorldPlane(new HTuple(this.Row), new HTuple(this.Col), grab_x, grab_y, grab_z, out Qx, out Qy, out Qz);
        double radius1 = CamParams.TransPixLengthToWcsLength(this.Radius1);
        double radius2 = CamParams.TransPixLengthToWcsLength(this.Radius2);
        double deg = this.Rad * 180 / Math.PI;
        wcsLine = new drawWcsEllipse(Qx[0].D, Qy[0].D, deg, radius1, radius2);
        return wcsLine;
    }
    public override HRegion GetRegion()
    {
        HRegion hRegion = new HRegion();
        hRegion.GenEllipse(this.Row, this.Col, this.Rad, this.Radius1, this.Radius2);
        return hRegion;
    }

    public override void GetRoiCenter(out double row, out double col)
    {
        row = (this.Row);
        col = (this.Col);
    }
    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsCircle(camera);
    }
    public override string ToString()
    {
        return string.Join(",", this.Row, this.Col, this.Rad, this.Radius1, this.Radius2);
    }
}


[Serializable]
public class drawPixRect2 : PixROI
{
    [DisplayNameAttribute("行坐标")]
    public double Row { get; set; }

    [DisplayNameAttribute("列坐标")]
    public double Col { get; set; }

    [DisplayNameAttribute("角度")]
    public double Rad { get; set; }

    [DisplayNameAttribute("长度1")]
    public double Length1 { get; set; }

    [DisplayNameAttribute("长度2")]
    public double Length2 { get; set; }

    public drawPixRect2()
    {
        this.Row = 0;
        this.Col = 0;
        this.Rad = 0;
        this.Length1 = 0;
        this.Length2 = 0;
    }

    public drawPixRect2(double row, double col, double Rad, double Length1, double Length2)
    {
        this.Row = row;
        this.Col = col;
        this.Rad = Rad;
        this.Length1 = Length1;
        this.Length2 = Length2;
    }
    public drawPixRect2 AffinePixRect2(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        double Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        if (homMat2D == null) return this;
        HHomMat2D hHomMat2D = new HHomMat2D(homMat2D);
        Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
        Qx = hHomMat2D.AffineTransPoint2d(this.Row, this.Col, out Qy);
        // 变换角度
        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, this.Rad + Phi);
        hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
        drawPixRect2 PixRect2 = new drawPixRect2(Qx, Qy, Phi, this.Length1, this.Length2);
        return PixRect2;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffinePixRect2(homMat2D.RawData);
    }
    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        hXLDCont.GenRectangle2ContourXld(this.Row, this.Col, this.Rad, this.Length1, this.Length2);
        return hXLDCont;
    }
    public override HRegion GetRegion()
    {
        HRegion hXLDCont = new HRegion();
        hXLDCont.GenRectangle2(this.Row, this.Col, this.Rad, this.Length1, this.Length2);
        return hXLDCont;
    }
    public override void GetRoiCenter(out double row, out double col)
    {
        row = (this.Row);
        col = (this.Col);
    }
    public drawWcsRect2 GetWcsRect2(CameraParam CamParams, double ref_x = 0, double ref_y = 0, double ref_z = 0)
    {
        HTuple Qx, Qy, Qz, _wcsDeg;
        drawWcsRect2 wcsRect2;
        HOperatorSet.TupleDeg(this.Rad, out _wcsDeg);
        double _wcsLength1 = CamParams.TransPixLengthToWcsLength(this.Length1);
        double _wcsLength2 = CamParams.TransPixLengthToWcsLength(this.Length2);
        CamParams.ImagePointsToWorldPlane(this.Row, this.Col, ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
        wcsRect2 = new drawWcsRect2(Qx[0].D, Qy[0].D, 0, _wcsDeg.D, _wcsLength1, _wcsLength2);
        /////////////////////////
        return wcsRect2;
    }

    public userPixRectangle2 GetUserPixRectangle2()
    {
        userPixRectangle2 pixRectangle2 = new userPixRectangle2();
        pixRectangle2.Row = this.Row;
        pixRectangle2.Col = this.Col;
        pixRectangle2.Rad = this.Rad;
        pixRectangle2.Length1 = this.Length1;
        pixRectangle2.Length2 = this.Length2;
        /////////////////////////
        return pixRectangle2;
    }
    //public override ROI GetPixObject(CameraParam camera)
    //{
    //    return this;
    //}
    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsRect2(camera);
    }
    public override string ToString()
    {
        return string.Join(",", this.Row, this.Col, this.Rad, this.Length1, this.Length2);
    }
}

[Serializable]
public class drawPixRect1 : PixROI
{
    [DisplayNameAttribute("行坐标1")]
    public double Row1 { get; set; }

    [DisplayNameAttribute("列坐标1")]
    public double Col1 { get; set; }

    [DisplayNameAttribute("行坐标2")]
    public double Row2 { get; set; }

    [DisplayNameAttribute("列坐标2")]
    public double Col2 { get; set; }


    public drawPixRect1()
    {

    }

    public drawPixRect1(double row1, double col1, double row2, double col2)
    {
        this.Row1 = row1;
        this.Col1 = col1;
        this.Row2 = row2;
        this.Col2 = col2;
    }
    public drawPixRect1 AffineTransPixRect1(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty, start_phi;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), out Qx, out Qy);
        drawPixRect1 PixRect1 = new drawPixRect1(Qx[0].D, Qy[0].D, Qx[1].D, Qy[1].D);
        return PixRect1;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffineTransPixRect1(homMat2D.RawData);
    }
    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        hXLDCont.GenRectangle2ContourXld((this.Row1 + this.Row2) * 0.5, (this.Col1 + this.Col2) * 0.5, 0, (this.Col2 - this.Col1) * 0.5, (this.Row2 - this.Row1) * 0.5);
        return hXLDCont;
    }
    public drawWcsRect1 GetWcsRect1(CameraParam CamParams, double ref_x = 0, double ref_y = 0, double ref_z = 0)
    {
        HTuple Qx, Qy, Qz;
        drawWcsRect1 wcsRect1;
        CamParams.ImagePointsToWorldPlane(new HTuple(this.Row1, this.Row2), new HTuple(this.Col1, this.Col2), ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
        wcsRect1 = new drawWcsRect1(Qx[0].D, Qy[0].D, Qx[1].D, Qy[1].D);
        /////////////////////////
        return wcsRect1;
    }
    public override HRegion GetRegion()
    {
        HRegion hRegion = new HRegion();
        hRegion.GenRectangle1(this.Row1, this.Col1, this.Row2, this.Col2);
        return hRegion;
    }
    public override void GetRoiCenter(out double row, out double col)
    {
        row = (this.Row1 + this.Row2) * 0.5;
        col = (this.Col1 + this.Col2) * 0.5;
    }
    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsRect1(camera);
    }
    public override string ToString()
    {
        return string.Join(",", this.Row1, this.Col1, this.Row2, this.Col2);
    }
}


[Serializable]
public class drawPixPolygon : PixROI
{

    [DisplayNameAttribute("行坐标")]
    public List<double> Row { get; set; }

    [DisplayNameAttribute("列坐标")]
    public List<double> Col { get; set; }

    public drawPixPolygon(double[] rows, double[] cols)
    {
        this.Row = new List<double>();
        this.Col = new List<double>();
        this.Row.AddRange(rows);
        this.Col.AddRange(cols);
    }
    public drawPixPolygon AffinePixPolygon(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row.ToArray()), new HTuple(this.Col.ToArray()), out Qx, out Qy);
        drawPixPolygon PixRect1 = new drawPixPolygon(Qx.DArr, Qy.DArr);
        return PixRect1;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffinePixPolygon(homMat2D.RawData);
    }
    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        if (this.Row.Count > 0)
            hXLDCont.GenContourPolygonXld(new HTuple(this.Row.ToArray(), this.Row[0]), new HTuple(this.Col.ToArray(), this.Col[0]));
        return hXLDCont;
    }
    public drawWcsPolygon GetWcsPolygon(CameraParam CamParams, double ref_x = 0, double ref_y = 0, double ref_z = 0)
    {
        HTuple Qx, Qy, Qz;
        drawWcsPolygon wcsRect1;
        CamParams.ImagePointsToWorldPlane(new HTuple(this.Row.ToArray()), new HTuple(this.Col.ToArray()), ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
        wcsRect1 = new drawWcsPolygon(Qx.DArr, Qy.DArr);
        /////////////////////////
        return wcsRect1;
    }
    public override HRegion GetRegion()
    {
        HRegion hRegion = new HRegion();
        hRegion.GenRegionPolygonFilled(this.Row.ToArray(), this.Col.ToArray());
        return hRegion;
    }
    public override void GetRoiCenter(out double row, out double col)
    {
        row = 0;
        col = 0;
        if (this.Row.Count > 0)
            row = this.Row.Average();
        if (this.Col.Count > 0)
            col = this.Col.Average();
    }
    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsPolygon(camera);
    }
    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < this.Row.Count; i++)
        {
            str += Math.Round(this.Row[i], 3) + "," + Math.Round(this.Col[i], 3);
        }
        return str; // string.Join(",", this.Row, this.Col);
    }
}

[Serializable]
public class drawPixPolyLine : PixROI
{

    [DisplayNameAttribute("行坐标")]
    public List<double> Row { get; set; }

    [DisplayNameAttribute("列坐标")]
    public List<double> Col { get; set; }

    public drawPixPolyLine(double[] rows, double[] cols)
    {
        this.Row = new List<double>();
        this.Col = new List<double>();
        this.Row.AddRange(rows);
        this.Col.AddRange(cols);
    }
    public drawPixPolyLine AffinePixPolygon(HTuple homMat2D)
    {
        // 这里是否需要使用一个固定的角度作为输入，还是从矩阵中获取？
        if (homMat2D == null) return this;
        HTuple Qx, Qy, Sx, Sy, Phi, Theta, Tx, Ty;
        HOperatorSet.HomMat2dToAffinePar(homMat2D, out Sx, out Sy, out Phi, out Theta, out Tx, out Ty);
        HOperatorSet.AffineTransPoint2d(homMat2D, new HTuple(this.Row.ToArray()), new HTuple(this.Col.ToArray()), out Qx, out Qy);
        drawPixPolyLine PixRect1 = new drawPixPolyLine(Qx.DArr, Qy.DArr);
        return PixRect1;
    }
    public override PixROI AffinePixROI(HHomMat2D homMat2D)
    {
        return AffinePixPolygon(homMat2D.RawData);
    }
    public override HXLDCont GetXLD()
    {
        HXLDCont hXLDCont = new HXLDCont();
        if (this.Row.Count > 0)
            hXLDCont.GenContourPolygonXld(new HTuple(this.Row.ToArray()), new HTuple(this.Col.ToArray()));
        return hXLDCont;
    }
    public drawWcsPolyLine GetWcsPolygon(CameraParam CamParams, double ref_x = 0, double ref_y = 0, double ref_z = 0)
    {
        HTuple Qx, Qy, Qz;
        drawWcsPolyLine wcsRect1;
        CamParams.ImagePointsToWorldPlane(new HTuple(this.Row.ToArray()), new HTuple(this.Col.ToArray()), ref_x, ref_y, ref_z, out Qx, out Qy, out Qz);
        wcsRect1 = new drawWcsPolyLine(Qx.DArr, Qy.DArr);
        /////////////////////////
        return wcsRect1;
    }
    public override HRegion GetRegion()
    {
        HRegion hRegion = new HRegion();
        hRegion.GenRegionPolygonFilled(this.Row.ToArray(), this.Col.ToArray());
        return hRegion;
    }

    public override WcsROI GetWcsROI(CameraParam camera)
    {
        return GetWcsPolygon(camera);
    }
    public override void GetRoiCenter(out double row, out double col)
    {
        row = 0;
        col = 0;
        if (this.Row.Count > 0)
            row = this.Row.Average();
        if (this.Col.Count > 0)
            col = this.Col.Average();
    }
    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < this.Row.Count; i++)
        {
            str += Math.Round(this.Row[i], 3) + "," + Math.Round(this.Col[i], 3);
        }
        return str; // string.Join(",", this.Row, this.Col);
    }
}

[Serializable]
public class CoordPoint
{
    public string Sign { get; set; }
    public int Count { get; set; }
    public int[] Row { get; set; }
    public int[] Col { get; set; }
    public double[] X { get; set; }
    public double[] Y { get; set; }

    public CoordPoint(int count)
    {
        this.Sign = "";
        this.Count = count;
        this.Row = new int[count];
        this.Col = new int[count];
        this.X = new double[count];
        this.Y = new double[count];
    }

}

[Serializable]
public class ViewData
{
    public object DataObject { get; set; }
    public string Color { get; set; }
    public string Draw { get; set; }
    public ViewData(object content)
    {
        this.DataObject = content;
        this.Color = "green";
        this.Draw = "margin";
    }
    public ViewData(object content, string color = "green")
    {
        this.DataObject = content;
        this.Color = color;
        this.Draw = "margin";
    }
    public ViewData(object content, string color = "green", string draw = "margin")
    {
        this.DataObject = content;
        this.Color = color;
        this.Draw = draw;
    }

}