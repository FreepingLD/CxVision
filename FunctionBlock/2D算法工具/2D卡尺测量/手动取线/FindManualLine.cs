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
    public class FindManualLine
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsLine _fitLine;
        private GeometryMeasure lineGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsLine lineWcsPosition;
        private userPixLine linePixPosition = new userPixLine(100, 100, 300, 100); // 模型直线即参考直线位置
        public userPixLine LinePixPosition
        {
            get
            {
                return linePixPosition;
            }
            set
            {
                linePixPosition = value;
                if (this.linePixPosition.CamParams != null)
                    this.lineWcsPosition = linePixPosition.GetWcsLine();
            }
        }
        public userWcsLine LineWcsPosition
        {
            get
            {
                if (this.linePixPosition.CamParams != null)
                    return this.linePixPosition.GetWcsLine();
                else
                    return lineWcsPosition;
            }
            set
            {
                lineWcsPosition = value;
                this.linePixPosition = lineWcsPosition.GetPixLine();
            }
        }
        public userWcsLine FitLine { get => _fitLine; set => _fitLine = value; }
        public GeometryMeasure LineGeometry { get => lineGeometry; set => lineGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindManualLine()
        {
            this.lineGeometry = new GeometryMeasure(enMeasureType.point);
        }


        public bool FindManualLineMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y,z;
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
            this._fitLine = new userWcsLine(image.CamParams);
            /////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(homMat2DCompose); // 经坐标变换后的像素位置
            //userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(this.pixCoordSystem.GetVariationHomMat2D()); // 经坐标变换后的像素位置
            //this.lineGeometry.CreateLineMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, linePixPosition.DiffRadius, linePixPosition.NormalPhi, width, height);
            image.CamParams.ImagePointsToWorldPlane(new HTuple(linePixPosition.Row1, linePixPosition.Row2), new HTuple(linePixPosition.Col1, linePixPosition.Col2), image.Grab_X, image.Grab_Y, image.Grab_Z, out x, out y, out z);
            /////////////////////
            if (x != null && x.Length > 0)
            {
                this._fitLine = new userWcsLine(x[0].D, y[0].D, z[0].D, x[1].D, y[1].D, z[1].D, image.Grab_X, image.Grab_Y, image.CamParams);
                this._fitLine.Angle = Math.Atan2(_fitLine.Y2 - _fitLine.Y1, _fitLine.X2 - _fitLine.X1) * 180 / Math.PI;
                this._fitLine.CamName = image.CamName;
                this._fitLine.ViewWindow = image.ViewWindow;
                this._fitLine.Grab_z = image.Grab_Z;
                this._fitLine.Grab_u = image.Grab_U;
                this._fitLine.Grab_v = image.Grab_V;
                this._fitLine.Grab_theta = image.Grab_Theta;
                this._fitLine.EdgesPoint_xyz = this._fitLine.GetFitWcsPoint();
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
