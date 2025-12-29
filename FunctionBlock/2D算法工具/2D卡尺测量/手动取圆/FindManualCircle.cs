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
    public class FindManualCircle
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsCircle _fitCircle;
        private GeometryMeasure circleGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsCircle circleWcsPosition;
        private userPixCircle circlePixPosition = new userPixCircle(300, 300,30); // 
        public userPixCircle CirclePixPosition
        {
            get
            {
                return circlePixPosition;
            }
            set
            {
                circlePixPosition = value;
                if (this.circlePixPosition.CamParams != null)
                    this.circleWcsPosition = circlePixPosition.GetWcsCircle();
            }
        }
        public userWcsCircle CircleWcsPosition
        {
            get
            {
                if (this.circlePixPosition.CamParams != null)
                    return this.circlePixPosition.GetWcsCircle();
                else
                    return circleWcsPosition;
            }
            set
            {
                circleWcsPosition = value;
                this.circlePixPosition = circleWcsPosition.GetPixCircle();
            }
        }
        public userWcsCircle FitCircle { get => _fitCircle; set => _fitCircle = value; }
        public GeometryMeasure CircleGeometry { get => circleGeometry; set => circleGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindManualCircle()
        {
            this.circleGeometry = new GeometryMeasure(enMeasureType.circle);
        }


        public bool FindCircleMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, Row, Col, x, y, z,cla_x, cal_y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitCircle = new userWcsCircle(image.CamParams);
            ////////////////////////////////////
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
            {
                this.pixCoordSystem = new userPixCoordSystem();
                this.pixCoordSystem.Result = true;
            }
            else
                this.pixCoordSystem = pixCoordSystem;
            image.Image.GetImageSize(out width, out height);
            //////////////////////////////////////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixCircle circlePixPosition = this.CirclePixPosition.AffineTransPixCircle(homMat2DCompose); // 使用世界点来变换，然后再转换为像素点
            //userPixCircle circlePixPosition = this.CirclePixPosition.AffineTransPixCircle(this.pixCoordSystem?.GetVariationHomMat2D()); // 使用世界点来变换，然后再转换为像素点
            /////////////////////////////////////
            this.circleGeometry.CreateCircleMeasure(circlePixPosition.Row, circlePixPosition.Col, circlePixPosition.Radius, circlePixPosition.Start_phi, circlePixPosition.End_phi, circlePixPosition.DiffRadius, circlePixPosition.PointOrder, width, height);
            image.CamParams.ImagePointsToWorldPlane(new HTuple(circlePixPosition.Row), new HTuple(circlePixPosition.Col), image.Grab_X, image.Grab_Y, image.Grab_Z, out x, out y, out z);
            double radius = image.CamParams.TransPixLengthToWcsLength(circlePixPosition.Radius);
            /////////////////////
            if (x != null && x.Length > 0)
            {
                this._fitCircle = new userWcsCircle(x[0], y[0], z[0], radius, image.Grab_X, image.Grab_Y, image.CamParams);
                this._fitCircle.CamName = image.CamName;
                this._fitCircle.ViewWindow = image.ViewWindow;
                this._fitCircle.Grab_z = image.Grab_Z;
                this._fitCircle.Grab_u = image.Grab_U;
                this._fitCircle.Grab_v = image.Grab_V;
                this._fitCircle.Grab_theta = image.Grab_Theta;
                this._fitCircle.EdgesPoint_xyz =  this._fitCircle.GetInterpolateWcsPoint(this.circleGeometry.Num_measures);
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
