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
    public class FindManualCircleSector
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsCircleSector _fitCircleSector;
        private GeometryMeasure circleSectorGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsCircleSector circleSectorWcsPosition;
        private userPixCircleSector circleSectorPixPosition = new userPixCircleSector(300, 300, 100, 0, 3.14); // 
        public userPixCircleSector CircleSectorPixPosition
        {
            get
            {
                return circleSectorPixPosition;
            }

            set
            {
                circleSectorPixPosition = value;
                if (this.circleSectorPixPosition.CamParams != null)
                    this.circleSectorWcsPosition = circleSectorPixPosition.GetWcsCircleSector();
            }
        }
        public userWcsCircleSector CircleSectorWcsPosition
        {
            get
            {
                if (this.circleSectorPixPosition.CamParams != null)
                    return this.circleSectorPixPosition.GetWcsCircleSector();
                else
                    return circleSectorWcsPosition;
            }
            set
            {
                circleSectorWcsPosition = value;
                this.circleSectorPixPosition = circleSectorWcsPosition.GetPixCircleSector();
            }
        }
        public userWcsCircleSector FitCircleSector { get => _fitCircleSector; set => _fitCircleSector = value; }
        public GeometryMeasure CircleSectorGeometry { get => circleSectorGeometry; set => circleSectorGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindManualCircleSector()
        {
            this.circleSectorGeometry = new GeometryMeasure(enMeasureType.circleSector);
        }

        public bool FindCrossPointMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y, z;
            int width, height;
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (!image.Image.IsInitialized())
            {
                throw new ArgumentNullException("参数image未初始化");
            }
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
            {
                this.pixCoordSystem = new userPixCoordSystem();
                this.pixCoordSystem.Result = true;
            }
            else
                this.pixCoordSystem = pixCoordSystem;
            image.Image.GetImageSize(out width, out height);
            this._fitCircleSector = new userWcsCircleSector(image.CamParams);
            /////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixCircleSector circleSectorPixPosition = this.CircleSectorPixPosition.AffineTransPixCircleSector(homMat2DCompose); // 经坐标变换后的像素位置
            //userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(this.pixCoordSystem.GetVariationHomMat2D()); // 经坐标变换后的像素位置
            this.circleSectorGeometry.CreateCircleMeasure(circleSectorPixPosition.Row, circleSectorPixPosition.Col, circleSectorPixPosition.Radius, circleSectorPixPosition.Start_phi, circleSectorPixPosition.End_phi, circleSectorPixPosition.DiffRadius, circleSectorPixPosition.PointOrder, width, height);
            image.CamParams.ImagePointsToWorldPlane(new HTuple(circleSectorPixPosition.Row), new HTuple(circleSectorPixPosition.Col), image.Grab_X, image.Grab_Y, image.Grab_Z, out x, out y, out z);
            double radius = image.CamParams.TransPixLengthToWcsLength(circleSectorPixPosition.Radius);
            /////////////////////
            if (x != null && x.Length > 0)
            {

                this._fitCircleSector = new userWcsCircleSector(x[0], y[0], z[0].D, radius, circleSectorPixPosition.Start_phi * 180 / Math.PI,
                                                                circleSectorPixPosition.End_phi * 180 / Math.PI, image.Grab_X, image.Grab_Y, image.CamParams); // 图像中需要包含相机对应的内参的外参,结果需要加上一个旋转角  
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                this._fitCircleSector.CamName = image.CamName;
                this._fitCircleSector.ViewWindow = image.ViewWindow;
                this._fitCircleSector.Grab_z = image.Grab_Z;
                this._fitCircleSector.Grab_u = image.Grab_U;
                this._fitCircleSector.Grab_v = image.Grab_V;
                this._fitCircleSector.Grab_theta = image.Grab_Theta;
                this._fitCircleSector.PointOrder = this.circleSectorPixPosition.PointOrder;
                this._fitCircleSector.EdgesPoint_xyz = this._fitCircleSector.GetInterpolateWcsPoint(this.circleSectorGeometry.Num_measures);

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
