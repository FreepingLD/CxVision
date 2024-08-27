using Common;
using HalconDotNet;
using System;

namespace FunctionBlock
{
    [Serializable]
    public class FindEllipse
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsEllipse _fitEllipse;
        private GeometryMeasure ellipseGeometry;
        private userPixEllipse ellipsePixPosition; // 给绘图对像一个初始值
        private userWcsEllipse ellipseWcsPosition;
        public userPixEllipse EllipsePixPosition
        {
            get
            {
                return ellipsePixPosition;
            }
            set
            {
                ellipsePixPosition = value;
                if (ellipsePixPosition.CamParams != null)
                    this.ellipseWcsPosition = ellipsePixPosition.GetWcsEllipse();
            }
        }
        public userWcsEllipse EllipseWcsPosition
        {
            get
            {
                if (ellipsePixPosition.CamParams != null)
                    return ellipsePixPosition.GetWcsEllipse();
                else
                    return ellipseWcsPosition;
            }
            set
            {
                ellipseWcsPosition = value;
                this.ellipsePixPosition = ellipseWcsPosition.GetPixEllipse();
            }
        }

        public userWcsEllipse FitEllipse { get => _fitEllipse; set => _fitEllipse = value; }
        public GeometryMeasure EllipseGeometry { get => ellipseGeometry; set => ellipseGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindEllipse()
        {
            ellipseGeometry = new GeometryMeasure(enMeasureType.ellipse);
            ellipsePixPosition = new userPixEllipse(300, 300, 0, 100, 80);
        }

        public bool FindEllipseMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitEllipse = new userWcsEllipse(image.CamParams);
            image.Image.GetImageSize(out width, out height);
            ///////////////////
            if (wcsCoordSystem.ReferencePoint.X == 0 && wcsCoordSystem.ReferencePoint.Y == 0)
                this.wcsCoordSystem = new userWcsCoordSystem();
            else
                this.wcsCoordSystem = wcsCoordSystem;
            userPixEllipse ellipsePixPosition = this.EllipseWcsPosition.AffineWcsEllipse2D(this.wcsCoordSystem.GetVariationHomMat2D(), enAffineTransOrientation.正向变换).GetPixEllipse();
            this.ellipseGeometry.CreateEllipseMeasure(ellipsePixPosition.Row, ellipsePixPosition.Col, ellipsePixPosition.Rad, ellipsePixPosition.Radius1, ellipsePixPosition.Radius2, 0, Math.PI * 2, ellipsePixPosition.DiffRadius, width, height);
            //this.ellipseGeometry.SetCameraParam(image.CamParam, image.CamPose);
            this.ellipseGeometry.ApplyMeasurePose(image.Image);
            this.ellipseGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.ellipseGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.ellipseGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitEllipse.CamName = image.CamName;
                _fitEllipse = new userWcsEllipse(Parameter[0], Parameter[1], Parameter[2], Parameter[3], Parameter[4], Parameter[5], image.Grab_X, image.Grab_Y, image.CamParams);
                _fitEllipse.Grab_z = image.Grab_Z;
                _fitEllipse.Grab_u = image.Grab_U;
                _fitEllipse.Grab_v = image.Grab_V;
                _fitEllipse.Grab_theta = image.Grab_Theta;
                if (x != null && x.Length > 0)
                {
                    _fitEllipse.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitEllipse.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2], image.Grab_X, image.Grab_Y, image.CamParams);
                    }
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitEllipse.EdgesPoint_xyz, this._fitEllipse), null, null);
                result = true;
            }
            return result;
        }
        public bool FindEllipseMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitEllipse = new userWcsEllipse(image.CamParams);
            image.Image.GetImageSize(out width, out height);
            ///////////////////
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
                this.pixCoordSystem = new userPixCoordSystem();
            else
                this.pixCoordSystem = pixCoordSystem;
            ////////////////////////////////////////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixEllipse ellipsePixPosition = this.EllipsePixPosition.AffineTransPixEllipse(homMat2DCompose);
            //userPixEllipse ellipsePixPosition = this.EllipsePixPosition.AffineTransPixEllipse(this.pixCoordSystem?.GetVariationHomMat2D());
            this.ellipseGeometry.CreateEllipseMeasure(ellipsePixPosition.Row, ellipsePixPosition.Col, ellipsePixPosition.Rad, ellipsePixPosition.Radius1, ellipsePixPosition.Radius2, 0, Math.PI * 2, ellipsePixPosition.DiffRadius, width, height);
            //this.ellipseGeometry.SetCameraParam(image.CamParam, image.CamPose);
            this.ellipseGeometry.ApplyMeasurePose(image.Image);
            this.ellipseGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.ellipseGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.ellipseGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitEllipse.CamName = image.CamName;
                _fitEllipse = new userWcsEllipse(Parameter[0], Parameter[1], Parameter[2], Parameter[3], Parameter[4], Parameter[5], image.Grab_X, image.Grab_Y, image.CamParams);
                _fitEllipse.Grab_z = image.Grab_Z;
                _fitEllipse.Grab_u = image.Grab_U;
                _fitEllipse.Grab_v = image.Grab_V;
                _fitEllipse.Grab_theta = image.Grab_Theta;
                //_fitEllipse.edgesPoint_x = x ;
                //_fitEllipse.edgesPoint_y = y ;
                if (x != null && x.Length > 0)
                {
                    _fitEllipse.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitEllipse.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2], image.Grab_X, image.Grab_Y, image.CamParams);
                    }
                    if (this.ellipseGeometry.IsOutFitPoint)
                     _fitEllipse.EdgesPoint_xyz = _fitEllipse.GetFitWcsPoint();
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitEllipse.EdgesPoint_xyz, this._fitEllipse), null, null);
                result = true;
            }
            return result;
        }



    }
}
