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
    public class FindManualPoint
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsPoint fitPoint;
        private GeometryMeasure lineGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsPoint pointWcsPosition;
        private userPixPoint pointPixPosition = new userPixPoint(300, 300); // 
        public userPixPoint PointPixPosition
        {
            get
            {
                return pointPixPosition;
            }

            set
            {
                pointPixPosition = value;
                if (this.pointPixPosition.CamParams != null)
                    this.pointWcsPosition = pointPixPosition.GetWcsPoint();
            }
        }
        public userWcsPoint PointWcsPosition
        {
            get
            {
                if (this.pointPixPosition.CamParams != null)
                    return this.pointPixPosition.GetWcsPoint();
                else
                    return pointWcsPosition;
            }
            set
            {
                pointWcsPosition = value;
                this.pointPixPosition = pointWcsPosition.GetPixPoint();
            }
        }
        public userWcsPoint FitPoint { get => fitPoint; set => fitPoint = value; }
        public GeometryMeasure LineGeometry { get => lineGeometry; set => lineGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindManualPoint()
        {
            this.lineGeometry = new GeometryMeasure(enMeasureType.point);
        }


        public bool FindManualPointMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
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
            this.fitPoint = new userWcsPoint(image.CamParams);
            /////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixPoint pointPixPosition = this.PointPixPosition.AffineTransPixPoint(homMat2DCompose); // 经坐标变换后的像素位置
            if (this.lineGeometry.FillUpInvalidData == "origin_坐标原点") // 以相机坐标系的原点为捨取点
            {
                double zeroRow, zeroCol;
                image.CamParams.WorldPointsToImagePlane(0, 0, 0, 0, 0, 0, out zeroRow, out zeroCol);
                image.CamParams.ImagePointsToWorldPlane(zeroRow, zeroCol, image.Grab_X, image.Grab_Y, image.Grab_Z, out x, out y, out z);
            }
            else
            {
                image.CamParams.ImagePointsToWorldPlane(pointPixPosition.Row, pointPixPosition.Col, image.Grab_X, image.Grab_Y, image.Grab_Z, out x, out y, out z);
            }
            /////////////////////
            if (x != null && x.Length > 0)
            {
                this.fitPoint = new userWcsPoint(x[0].D, y[0].D, z[0].D, image.Grab_X, image.Grab_Y, image.CamParams);
                this.fitPoint.CamName = image.CamName;
                this.fitPoint.ViewWindow = image.ViewWindow;
                this.fitPoint.Grab_z = image.Grab_Z;
                this.fitPoint.Grab_u = image.Grab_U;
                this.fitPoint.Grab_v = image.Grab_V;
                this.fitPoint.Grab_theta = image.Grab_Theta;
                this.fitPoint.EdgesPoint_xyz = new userWcsPoint[1] { new userWcsPoint(x[0].D, y[0].D, z[0].D, image.Grab_X, image.Grab_Y, image.CamParams) };
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
