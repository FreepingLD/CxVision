﻿using System;
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

namespace FunctionBlock
{
    [Serializable]
    public class FindPoint
    {

        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsPoint fitPoint;
        private GeometryMeasure lineGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsLine lineWcsPosition;
        private userPixLine linePixPosition = new userPixLine(200, 200, 400, 200); // 模型直线即参考直线位置
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
        public userWcsPoint FitPoint { get => fitPoint; set => fitPoint = value; }
        public GeometryMeasure LineGeometry { get => lineGeometry; set => lineGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindPoint()
        {
            this.lineGeometry = new GeometryMeasure(enMeasureType.point);
        }

        public bool FindPointMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            HTuple Parameter, row, col, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            this.fitPoint = new userWcsPoint(image.CamParams);
            ////////////////////////////////
            if (wcsCoordSystem.ReferencePoint.X == 0 && wcsCoordSystem.ReferencePoint.Y == 0)
                this.wcsCoordSystem = new userWcsCoordSystem();
            else
                this.wcsCoordSystem = wcsCoordSystem;
            image.Image.GetImageSize(out width, out height);
            /////////////////////////////////////
            userPixLine linePixPosition = this.LineWcsPosition.AffineWcsLine2D(this.wcsCoordSystem.GetVariationHomMat2D()).GetPixLine(); // 经坐标变换后的像素位置
            this.lineGeometry.CreatePointMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, width, height);
            this.lineGeometry.ApplyMeasurePose(image.Image);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); // image.CalibrateFile
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.column_Edges1, out x);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.row_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                this.fitPoint.CamName = image.CamName;
                this.fitPoint = new userWcsPoint(Parameter[0].D, Parameter[1].D, Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                this.fitPoint.EdgesPoint_xyz = new userWcsPoint[1] { this.fitPoint };
                //MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(this.fitPoint.EdgesPoint_xyz, this.fitPoint), null, null);
                result = true;
            }
            return result;
        }

        public bool FindPointMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, row, col, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            this.fitPoint = new userWcsPoint(image.CamParams);
            ////////////////////////////////
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
                this.pixCoordSystem = new userPixCoordSystem();
            else
                this.pixCoordSystem = pixCoordSystem;
            image.Image.GetImageSize(out width, out height);
            /////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(homMat2DCompose); // 经坐标变换后的像素位置
            //userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(this.pixCoordSystem?.GetVariationHomMat2D()); // 经坐标变换后的像素位置
            this.lineGeometry.CreatePointMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, width, height);
            this.lineGeometry.ApplyMeasurePose(image.Image);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); // image.CalibrateFile
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.column_Edges1, out x);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.row_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                this.fitPoint.CamName = image.CamName;
                this.fitPoint = new userWcsPoint(Parameter[0].D, Parameter[1].D, Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                this.fitPoint.EdgesPoint_xyz = new userWcsPoint[1] { this.fitPoint };
                this.fitPoint.Grab_z = 0;
                this.fitPoint.Grab_theta = image.Grab_Theta;
                this.fitPoint.Grab_u = image.Grab_U;
                this.fitPoint.Grab_v = image.Grab_V;
                //MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(this.fitPoint.EdgesPoint_xyz, this.fitPoint), null, null);
                result = true;
            }
            return result;
        }


    }
}
