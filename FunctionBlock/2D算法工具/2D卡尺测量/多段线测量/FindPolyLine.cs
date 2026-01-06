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
using System.Runtime.InteropServices;
using System.Data;
using Light;
using System.Threading;
using System.Windows.Controls.Primitives;
using FluentFTP;

namespace FunctionBlock
{
    [Serializable]
    public class FindPolyLine
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsPolyLine _fitPolyLine;
        private GeometryMeasure polyLineGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsPolyLine lineWcsPosition;
        private userPixPolyLine linePixPosition = new userPixPolyLine(new double[] { 100, 300 }, new double[] { 100, 100 }); // 模型直线即参考直线位置
        public userPixPolyLine PolyLinePixPosition
        {
            get
            {
                return linePixPosition;
            }

            set
            {
                linePixPosition = value;
                if (linePixPosition.CamParams != null)
                    lineWcsPosition = linePixPosition.GetWcsPolyLine();
            }
        }
        public userWcsPolyLine PolyLineWcsPosition
        {
            get
            {
                if (linePixPosition.CamParams != null)
                    return linePixPosition.GetWcsPolyLine();
                else
                    return lineWcsPosition;
            }
            set
            {
                lineWcsPosition = value;
                this.linePixPosition = lineWcsPosition.GetPixPolyLine();
            }
        }
        public userWcsPolyLine FitPolyLine { get => _fitPolyLine; set => _fitPolyLine = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }
        public GeometryMeasure Geometry { get => polyLineGeometry; set => polyLineGeometry = value; }

        public FindPolyLine()
        {
            this.polyLineGeometry = new GeometryMeasure(enMeasureType.polyLine);
        }



        public bool FindPolyLineMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (image.Image == null || !image.Image.IsInitialized())
            {
                throw new ArgumentNullException("参数image中的图像对象为空值或未被初始化");
            }
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
            {
                this.pixCoordSystem = new userPixCoordSystem();
                this.pixCoordSystem.Result = true;
            }
            else
                this.pixCoordSystem = pixCoordSystem;
            image.Image.GetImageSize(out width, out height);
            this._fitPolyLine = new userWcsPolyLine(image.CamParams);
            /////////////////////////////////////
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵,用于阵列测量
            userPixPolyLine linePixPosition = this.PolyLinePixPosition.AffinePixPolyLine(homMat2DCompose); // 经坐标变换后的像素位置
            //userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(this.pixCoordSystem?.GetVariationHomMat2D()); // 经坐标变换后的像素位置
            if (!this.pixCoordSystem.Result && SystemParamManager.Instance.SysConfigParam.EnableManualCalliper)
            {
                ManualMeasurePolyLineForm form = new ManualMeasurePolyLineForm(image, new drawPixPolyLine(linePixPosition.Row.ToArray(), linePixPosition.Col.ToArray()));
                form.TopLevel = true;
                form.ShowDialog();
                while (true)
                {
                    Thread.Sleep(100);
                    if (form.DialogResult == DialogResult.OK || form.DialogResult == DialogResult.No) break;
                }
                if (form.DialogResult == DialogResult.OK)
                    this.polyLineGeometry.CreatePolyLineMeasure(form.PixPolyLine.Row, form.PixPolyLine.Col, linePixPosition.DiffRadius, linePixPosition.NormalPhi.ToArray(), width, height);
                else
                    this.polyLineGeometry.CreatePolyLineMeasure(linePixPosition.Row, linePixPosition.Col, linePixPosition.DiffRadius, linePixPosition.NormalPhi.ToArray(), width, height);
            }
            else
                this.polyLineGeometry.CreatePolyLineMeasure(linePixPosition.Row, linePixPosition.Col, linePixPosition.DiffRadius, linePixPosition.NormalPhi.ToArray(), width, height);
            this.polyLineGeometry.ApplyMeasurePose(image.Image);
            //this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this.polyLineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.polyLineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            if (x != null && x.Length > 5)
            {
                switch (this.polyLineGeometry.PointFilterMethod)
                {
                    default:
                    case enPointFilterMethod.NONE:

                        break;
                    case enPointFilterMethod.平滑滤波:
                        HTuple rows, cols;
                        HXLDCont hXLDCont = new HXLDCont(y * -1, x);
                        int smoothCount = 1;
                        int.TryParse(this.polyLineGeometry.PointFilterParam, out smoothCount);
                        if (smoothCount < 3) smoothCount = 3;
                        if (smoothCount > x.Length * 0.5) smoothCount = (int)(x.Length * 0.5);
                        HXLDCont hXLDContSmooth = hXLDCont.SmoothContoursXld(smoothCount);
                        hXLDContSmooth.GetContourXld(out rows, out cols);
                        x = cols;
                        y = rows * -1;
                        break;
                    case enPointFilterMethod.最小点数:
                        List<double> list_x = new List<double>();
                        List<double> list_y = new List<double>();
                        HTuple z = HTuple.TupleGenConst(x.Length, 0);
                        HObjectModel3D hObjectModel3D = new HObjectModel3D(x, y, z);
                        smoothCount = 3;
                        int.TryParse(this.polyLineGeometry.PointFilterParam, out smoothCount);
                        if (smoothCount < 1) smoothCount = 1;
                        double dist = image.CamParams.TransPixLengthToWcsLength(this.polyLineGeometry.Measure_length2 * 2 * 1.4); // 1.5倍距离缩放
                        double[] filter_x, filter_y;
                        this.FilterByDist(x.DArr, y.DArr, dist, smoothCount, out filter_x, out filter_y);
                        x = new HTuple(filter_x);
                        y = new HTuple(filter_y);
                        //HObjectModel3D[] hObjectModel3Ds = hObjectModel3D.ConnectionObjectModel3d("distance_3d", dist);
                        //foreach (var item in hObjectModel3Ds)
                        //{
                        //    HTuple hTuple_x = item.GetObjectModel3dParams("point_coord_x");
                        //    HTuple hTuple_y = item.GetObjectModel3dParams("point_coord_y");
                        //    if (hTuple_x != null && hTuple_x.Length > smoothCount)
                        //    {
                        //        for (int i = 0; i < hTuple_x.Length; i++)
                        //        {
                        //            list_x.Add(hTuple_x[i].D);
                        //            list_y.Add(hTuple_y[i].D);
                        //        }
                        //    }
                        //}
                        //if (list_x.Count > 0)
                        //{
                        //    x = list_x.ToArray();
                        //    y = list_y.ToArray();
                        //}
                        //hObjectModel3D?.ClearObjectModel3d();
                        //HObjectModel3D.ClearObjectModel3d(hObjectModel3Ds);
                        break;
                }
            }
            /////////////////////
            if (x != null && x.Length > 0)
            {
                this._fitPolyLine = new userWcsPolyLine(image.CamParams);
                this._fitPolyLine.Grab_x = image.Grab_X;
                this._fitPolyLine.Grab_y = image.Grab_Y;
                this._fitPolyLine.Grab_z = image.Grab_Z;
                this._fitPolyLine.Grab_u = image.Grab_U;
                this._fitPolyLine.Grab_v = image.Grab_V;
                this._fitPolyLine.Grab_theta = image.Grab_Theta;
                this._fitPolyLine.CamName = image.CamName;
                this._fitPolyLine.ViewWindow = image.ViewWindow;
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                if (x != null && x.Length > 0)
                {
                    this._fitPolyLine.X?.Clear();
                    this._fitPolyLine.Y?.Clear();
                    if (this.polyLineGeometry.IsOutFitPoint == "true")
                    {
                        double[] inter_x, inter_y;
                        this._fitPolyLine.LineInterpretationByCount(x.ToDArr(), y.ToDArr(), this.polyLineGeometry.FitPointNum, out inter_x, out inter_y); //this.polyLineGeometry.FitPointNum:获取的是测量区域的数量
                        this._fitPolyLine.EdgesPoint_xyz = new userWcsPoint[inter_x.Length];
                        for (int i = 0; i < inter_x.Length; i++)
                        {
                            this._fitPolyLine.EdgesPoint_xyz[i] = new userWcsPoint(inter_x[i], inter_y[i], 0, image.Grab_X, image.Grab_Y, image.CamParams);
                            this._fitPolyLine.X.Add(inter_x[i]);
                            this._fitPolyLine.Y.Add(inter_y[i]);
                        }
                    }
                    else
                    {
                        this._fitPolyLine.EdgesPoint_xyz = new userWcsPoint[x.Length];
                        for (int i = 0; i < x.Length; i++)
                        {
                            this._fitPolyLine.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], 0, image.Grab_X, image.Grab_Y, image.CamParams);
                            this._fitPolyLine.X.Add(x[i]);
                            this._fitPolyLine.Y.Add(y[i]);
                        }
                    }
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }


        public void FilterByDist(double[] x, double[] y, double dist, int minCount, out double[] filter_x, out double[] filter_y)
        {
            filter_x = new double[0];
            filter_y = new double[0];
            List<List<double>> list_x = new List<List<double>>();
            List<List<double>> list_y = new List<List<double>>();
            List<double> temp_x = new List<double>();
            List<double> temp_y = new List<double>();
            double cur_x = x[0];
            double cur_y = y[0];
            temp_x.Add(cur_x);
            temp_y.Add(cur_y);
            for (int i = 0; i < x.Length; i++)
            {
                if (i > 0)
                {
                    cur_x = x[i - 1];
                    cur_y = y[i - 1];
                }
                double _dist = Math.Sqrt((x[i] - cur_x) * (x[i] - cur_x) + (y[i] - cur_y) * (y[i] - cur_y));
                if (_dist < dist)
                {
                    temp_x.Add(x[i]);
                    temp_y.Add(y[i]);
                }
                else
                {
                    temp_x.Add(cur_x); //把最后一个点加进来
                    temp_y.Add(cur_y);
                    list_x.Add(temp_x);
                    list_y.Add(temp_y);
                    temp_x = new List<double>();
                    temp_y = new List<double>();
                }
            }
            // 最后需要判断一次
            if(temp_x.Count > 0)
            {
                list_x.Add(temp_x);
                list_y.Add(temp_y);
            }
            ////////////////////////////////
            temp_x = new List<double>();
            temp_y = new List<double>();
            for (int i = 0; i < list_x.Count; i++)
            {
                if (list_x[i].Count > minCount)
                {
                    temp_x.AddRange(list_x[i]);
                    temp_y.AddRange(list_y[i]);
                }
            }
            filter_x = temp_x.ToArray();
            filter_y = temp_y.ToArray();
        }



    }
}
