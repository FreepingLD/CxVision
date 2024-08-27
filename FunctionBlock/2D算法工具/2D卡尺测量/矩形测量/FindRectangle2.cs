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

namespace FunctionBlock
{
    [Serializable]
    public class FindRectangle2
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsRectangle2 _fitRect2;
        private GeometryMeasure rect2Geometry;
        // 记录计量对象位置
        private userPixRectangle2 rect2PixPosition = new userPixRectangle2(200, 200, 0, 100, 20); // 像素坐标用于提供初始位置
        private userWcsRectangle2 rect2WcsPosition;
        public userPixRectangle2 Rect2PixPosition
        {
            get
            {
                return rect2PixPosition;
            }

            set
            {
                rect2PixPosition = value;
                if (rect2PixPosition.CamParams != null)
                    this.rect2WcsPosition = rect2PixPosition.GetWcsRectangle2();
            }
        }
        public userWcsRectangle2 Rect2WcsPosition
        {
            get
            {
                if (rect2PixPosition.CamParams != null)
                    return rect2PixPosition.GetWcsRectangle2();
                else
                    return rect2WcsPosition;
            }

            set
            {
                rect2WcsPosition = value;
                this.rect2PixPosition = rect2WcsPosition.GetPixRectangle2();
            }
        }

        public userWcsRectangle2 FitRect2 { get => _fitRect2; set => _fitRect2 = value; }
        public GeometryMeasure Rect2Geometry { get => rect2Geometry; set => rect2Geometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindRectangle2()
        {
            rect2Geometry = new GeometryMeasure(enMeasureType.rect2);
        }

        public bool FindRect2Method(ImageDataClass image, userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitRect2 = new userWcsRectangle2(image.CamParams);
            /////////////////////////////
            if (wcsCoordSystem.ReferencePoint.X == 0 && wcsCoordSystem.ReferencePoint.Y == 0)
                this.wcsCoordSystem = new userWcsCoordSystem();
            else
                this.wcsCoordSystem = wcsCoordSystem;
            image.Image.GetImageSize(out width, out height);
            ////////////////////////////
            userPixRectangle2 rect2PixPosition = this.Rect2WcsPosition.Affine2DWcsRectangle2(this.wcsCoordSystem.GetVariationHomMat2D(), enAffineTransOrientation.正向变换).GetPixRectangle2();
            userPixRectangle2 rect2PixPosition2 = this.Rect2WcsPosition.GetPixRectangle2().AffineTransPixRect2(this.wcsCoordSystem.GetPixCoordSystem().GetVariationHomMat2D());
            this.rect2Geometry.CreateRect2Measure(rect2PixPosition.Row, rect2PixPosition.Col, rect2PixPosition.Rad, rect2PixPosition.Length1, rect2PixPosition.Length2, rect2PixPosition.DiffRadius, width, height);
            this.rect2Geometry.SetCameraParam(image.CamParams.CamParam, image.CamParams.CamPose);
            this.rect2Geometry.ApplyMeasurePose(image.Image);
            this.rect2Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.rect2Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.rect2Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            ////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitRect2.CamName = image.CamName;
                _fitRect2 = new userWcsRectangle2(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams);
                //_fitRect2.edgesPoint_x = x ;
                //_fitRect2.edgesPoint_y = y ;
                _fitRect2.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    _fitRect2.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitRect2.EdgesPoint_xyz, this._fitRect2), null, null);
                result = true;
            }
            return result;
        }

        public bool FindRect2Method(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitRect2 = new userWcsRectangle2(image.CamParams);
            /////////////////////////////
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
                this.pixCoordSystem = new userPixCoordSystem();
            else
                this.pixCoordSystem = pixCoordSystem;
            image.Image.GetImageSize(out width, out height);
            ////////////////////////////  
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixRectangle2 rect2PixPosition = this.Rect2PixPosition.AffineTransPixRect2(homMat2DCompose);
            //userPixRectangle2 rect2PixPosition = this.Rect2PixPosition.AffineTransPixRect2(this.pixCoordSystem?.GetVariationHomMat2D());
            this.rect2Geometry.CreateRect2Measure(rect2PixPosition.Row, rect2PixPosition.Col, rect2PixPosition.Rad, rect2PixPosition.Length1, rect2PixPosition.Length2, rect2PixPosition.DiffRadius, width, height);
            this.rect2Geometry.SetCameraParam(image.CamParams.CamParam, image.CamParams.CamPose);
            this.rect2Geometry.ApplyMeasurePose(image.Image);
            this.rect2Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.rect2Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.rect2Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            ////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitRect2.CamName = image.CamName;
                _fitRect2 = new userWcsRectangle2(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams);
                if (x != null && x.Length > 0)
                {
                    _fitRect2.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitRect2.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                    }
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitRect2.EdgesPoint_xyz, this._fitRect2), null, null);
                result = true;
            }
            return result;
        }


    }
}
