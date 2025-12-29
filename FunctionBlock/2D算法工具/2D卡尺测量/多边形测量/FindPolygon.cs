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

namespace FunctionBlock
{
    [Serializable]
    public class FindPolygon
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsPolygon _fitPolygon;
        private GeometryMeasure _polygonGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsPolygon polygonWcsPosition;
        private userPixPolygon polygonPixPosition = new userPixPolygon(new double[] { 100, 100, 400, 400 }, new double[] { 100, 400, 400, 100 }); // 模型直线即参考直线位置
        public userPixPolygon PolygonPixPosition
        {
            get
            {
                return polygonPixPosition;
            }

            set
            {
                polygonPixPosition = value;
                if (polygonPixPosition.CamParams != null)
                    polygonWcsPosition = polygonPixPosition.GetWcsPolygon();
            }
        }
        public userWcsPolygon PolygonWcsPosition
        {
            get
            {
                if (polygonPixPosition.CamParams != null)
                    return polygonPixPosition.GetWcsPolygon();
                else
                    return polygonWcsPosition;
            }
            set
            {
                polygonWcsPosition = value;
                this.polygonPixPosition = polygonWcsPosition.GetPixPolygon();
            }
        }
        public userWcsPolygon FitPolygon { get => _fitPolygon; set => _fitPolygon = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }
        public GeometryMeasure Geometry { get => _polygonGeometry; set => _polygonGeometry = value; }

        public FindPolygon()
        {
            this._polygonGeometry = new GeometryMeasure(enMeasureType.Polygon);
        }



        public bool FindPolygonMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
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
            this._fitPolygon = new userWcsPolygon(image.CamParams);
            /////////////////////////////////////
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵,用于阵列测量
            userPixPolygon polygonPixPosition = this.PolygonPixPosition.AffinePixPolygon(homMat2DCompose); // 经坐标变换后的像素位置
            //userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(this.pixCoordSystem?.GetVariationHomMat2D()); // 经坐标变换后的像素位置
            if (!this.pixCoordSystem.Result && SystemParamManager.Instance.SysConfigParam.EnableManualCalliper)
            {
                ManualMeasurePolygonForm form = new ManualMeasurePolygonForm(image, new drawPixPolygon(polygonPixPosition.Row.ToArray(), polygonPixPosition.Col.ToArray()));
                form.TopLevel = true;
                form.ShowDialog();
                while (true)
                {
                    Thread.Sleep(100);
                    if (form.DialogResult == DialogResult.OK || form.DialogResult == DialogResult.No) break;
                }
                if (form.DialogResult == DialogResult.OK)
                    this._polygonGeometry.CreatePolygonMeasure(form.PixPolygon.Row, form.PixPolygon.Col, polygonPixPosition.DiffRadius, polygonPixPosition.NormalPhi.ToArray(), width, height);
                else
                    this._polygonGeometry.CreatePolygonMeasure(polygonPixPosition.Row, polygonPixPosition.Col, polygonPixPosition.DiffRadius, polygonPixPosition.NormalPhi.ToArray(), width, height);
            }
            else
            {
                this._polygonGeometry.CreatePolygonMeasure(polygonPixPosition.Row, polygonPixPosition.Col, polygonPixPosition.DiffRadius, polygonPixPosition.NormalPhi.ToArray(), width, height);
            }
            this._polygonGeometry.ApplyMeasurePose(image.Image);
            //this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this._polygonGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this._polygonGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (x != null && x.Length > 0)
            {
                this._fitPolygon = new userWcsPolygon(image.CamParams);
                this._fitPolygon.Grab_x = image.Grab_X;
                this._fitPolygon.Grab_y = image.Grab_Y;
                this._fitPolygon.Grab_z = image.Grab_Z;
                this._fitPolygon.Grab_u = image.Grab_U;
                this._fitPolygon.Grab_v = image.Grab_V;
                this._fitPolygon.Grab_theta = image.Grab_Theta;
                this._fitPolygon.CamName = image.CamName;
                this._fitPolygon.ViewWindow = image.ViewWindow;
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                if (x != null && x.Length > 0)
                {
                    this._fitPolygon.X?.Clear();
                    this._fitPolygon.Y?.Clear();
                    if (this._polygonGeometry.IsOutFitPoint == "true")
                    {
                        double[] inter_x, inter_y;
                        this._fitPolygon.LineInterpretationByCount(x.ToDArr(), y.ToDArr(), this._polygonGeometry.FitPointNum, out inter_x, out inter_y);
                        this._fitPolygon.EdgesPoint_xyz = new userWcsPoint[inter_x.Length];
                        for (int i = 0; i < inter_x.Length; i++)
                        {
                            this._fitPolygon.EdgesPoint_xyz[i] = new userWcsPoint(inter_x[i], inter_y[i], 0, image.Grab_X, image.Grab_Y, image.CamParams);
                            this._fitPolygon.X.Add(inter_x[i]);
                            this._fitPolygon.Y.Add(inter_y[i]);
                        }
                    }
                    else
                    {
                        this._fitPolygon.EdgesPoint_xyz = new userWcsPoint[x.Length];
                        for (int i = 0; i < x.Length; i++)
                        {
                            this._fitPolygon.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], 0, image.Grab_X, image.Grab_Y, image.CamParams);
                            this._fitPolygon.X.Add(x[i]);
                            this._fitPolygon.Y.Add(y[i]);
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






    }
}
