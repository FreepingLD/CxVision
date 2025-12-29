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
using System.Data;
using Light;
using System.Threading;

namespace FunctionBlock
{
    [Serializable]
    public class FindManualPolygon
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsPolygon _fitPolygon;
        private GeometryMeasure lineGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsPolygon polygonWcsPosition;
        private userPixPolygon polygonPixPosition = new userPixPolygon(new double[4] { 300, 300, 400, 400 }, new double[4] { 200, 300, 300, 200 }); // 
        public userPixPolygon PolygonPixPosition
        {
            get
            {
                return polygonPixPosition;
            }

            set
            {
                polygonPixPosition = value;
                if (this.polygonPixPosition.CamParams != null)
                    this.polygonWcsPosition = polygonPixPosition.GetWcsPolygon();
            }
        }
        public userWcsPolygon PointWcsPosition
        {
            get
            {
                if (this.polygonPixPosition.CamParams != null)
                    return this.polygonPixPosition.GetWcsPolygon();
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
        public GeometryMeasure PolygonGeometry { get => lineGeometry; set => lineGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindManualPolygon()
        {
            this.lineGeometry = new GeometryMeasure(enMeasureType.point);
        }


        public bool FindPolygonMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y, z;
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
            userPixPolygon polygonPixPosition = this.polygonPixPosition.AffinePixPolygon(homMat2DCompose); // 经坐标变换后的像素位置
            //////////////////////////////////
            image.CamParams.ImagePointsToWorldPlane(polygonPixPosition.Row.ToArray(), polygonPixPosition.Col.ToArray(), image.Grab_X, image.Grab_Y, image.Grab_Z, out x, out y, out z);
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
                    this._fitPolygon.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        this._fitPolygon.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], 0, image.Grab_X, image.Grab_Y, image.CamParams);
                        this._fitPolygon.X.Add(x[i]);
                        this._fitPolygon.Y.Add(y[i]);
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
