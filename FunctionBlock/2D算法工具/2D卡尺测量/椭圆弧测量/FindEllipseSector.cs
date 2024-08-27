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

namespace FunctionBlock
{
    [Serializable]
    public class FindEllipseSector
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsEllipseSector _fitEllipseSector;
        private GeometryMeasure ellipseSectorGeometry;
        // 记录计量对象位置
        private userPixEllipseSector ellipseSectorPixPosition;
        private userWcsEllipseSector ellipseSectorWcsPosition;

        public userPixEllipseSector EllipseSectorPixPosition
        {
            get
            {
                return ellipseSectorPixPosition;
            }

            set
            {
                ellipseSectorPixPosition = value;
            }
        }
        public userWcsEllipseSector EllipseSectorWcsPosition
        {
            get
            {
                if (ellipseSectorPixPosition.CamParams != null)
                    return ellipseSectorPixPosition.GetWcsEllipseSector();
                else
                    return ellipseSectorWcsPosition;
            }
            set
            {
                ellipseSectorWcsPosition = value;
                ellipseSectorPixPosition = ellipseSectorWcsPosition.GetPixEllipseSector();
            }
        }

        public userWcsEllipseSector FitEllipseSector { get => _fitEllipseSector; set => _fitEllipseSector = value; }
        public GeometryMeasure EllipseSectorGeometry { get => ellipseSectorGeometry; set => ellipseSectorGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindEllipseSector()
        {
            ellipseSectorGeometry = new GeometryMeasure(enMeasureType.ellipse);
            ellipseSectorPixPosition = new userPixEllipseSector(300, 300, 0, 100, 80, 0, 3.14);
        }
        public bool FindEllipseSectorMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int Width, Height;
            if (image == null) throw new ArgumentNullException("image");
            _fitEllipseSector = new userWcsEllipseSector(image.CamParams);
            ////////////////////////////////////
            image.Image.GetImageSize(out Width, out Height);
            /////////////////////////////////
            if (wcsCoordSystem.ReferencePoint.X == 0 && wcsCoordSystem.ReferencePoint.Y == 0)
                this.wcsCoordSystem = new userWcsCoordSystem();
            else
                this.wcsCoordSystem = wcsCoordSystem;
            userPixEllipseSector ellipseSectorPixPosition = this.EllipseSectorWcsPosition.Affine2DWcsEllipseSector(this.wcsCoordSystem.GetVariationHomMat2D(), enAffineTransOrientation.正向变换).GetPixEllipseSector();
            this.ellipseSectorGeometry.CreateEllipseMeasure(ellipseSectorPixPosition.Row, ellipseSectorPixPosition.Col, ellipseSectorPixPosition.Rad, ellipseSectorPixPosition.Radius1, ellipseSectorPixPosition.Radius2, ellipseSectorPixPosition.Start_phi, ellipseSectorPixPosition.End_phi, ellipseSectorPixPosition.DiffRadius, Width, Height);
            //this.ellipseSectorGeometry.SetCameraParam(image.CamParam, image.CamPose);
            this.ellipseSectorGeometry.ApplyMeasurePose(image.Image);
            this.ellipseSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.ellipseSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.ellipseSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitEllipseSector.CamName = image.CamName;
                _fitEllipseSector = new userWcsEllipseSector(Parameter[0].D, Parameter[1].D, Parameter[2].D, this.ellipseSectorWcsPosition.Deg, Parameter[3].D, Parameter[4].D,
                            this.ellipseSectorWcsPosition.Start_deg, this.ellipseSectorWcsPosition.End_deg, image.Grab_X, image.Grab_Y, image.CamParams); //- Parameter[2].D
                ///////////////////////////                                                                                                                                                                                                                 /////////// 对于椭圆，先通过起始角和终止角来截取还是先旋转  、、- Parameter[2].D   
                _fitEllipseSector.Grab_z = image.Grab_Z;
                _fitEllipseSector.Grab_u = image.Grab_U;
                _fitEllipseSector.Grab_v = image.Grab_V;
                _fitEllipseSector.Grab_theta = image.Grab_Theta;

                _fitEllipseSector.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    _fitEllipseSector.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, 0, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitEllipseSector.EdgesPoint_xyz, this._fitEllipseSector), null, null);
                result = true;
            }
            return result;
        }
        public bool FindEllipseSectorMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int Width, Height;
            if (image == null) throw new ArgumentNullException("image");
            _fitEllipseSector = new userWcsEllipseSector(image.CamParams);
            ////////////////////////////////////
            image.Image.GetImageSize(out Width, out Height);
            /////////////////////////////////
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
                this.pixCoordSystem = new userPixCoordSystem();
            else
                this.pixCoordSystem = pixCoordSystem;
            //////////////////////////////////////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵
            userPixEllipseSector ellipseSectorPixPosition = this.EllipseSectorPixPosition.AffineTransPixEllipseSector(homMat2DCompose);
            //userPixEllipseSector ellipseSectorPixPosition = this.EllipseSectorPixPosition.AffineTransPixEllipseSector(this.pixCoordSystem?.GetVariationHomMat2D());
            this.ellipseSectorGeometry.CreateEllipseMeasure(ellipseSectorPixPosition.Row, ellipseSectorPixPosition.Col, ellipseSectorPixPosition.Rad, ellipseSectorPixPosition.Radius1, ellipseSectorPixPosition.Radius2, ellipseSectorPixPosition.Start_phi, ellipseSectorPixPosition.End_phi, ellipseSectorPixPosition.DiffRadius, Width, Height);
            this.ellipseSectorGeometry.ApplyMeasurePose(image.Image);
            this.ellipseSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.ellipseSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.ellipseSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitEllipseSector.CamName = image.CamName;
                _fitEllipseSector = new userWcsEllipseSector(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D,
                                                             Parameter[6].D, Parameter[7].D, image.Grab_X, image.Grab_Y, image.CamParams); //- Parameter[2].D
                _fitEllipseSector.Grab_z = image.Grab_Z;
                _fitEllipseSector.Grab_u = image.Grab_U;
                _fitEllipseSector.Grab_v = image.Grab_V;
                _fitEllipseSector.Grab_theta = image.Grab_Theta;
                ////////////////////////////////////// 对于椭圆，先通过起始角和终止角来截取还是先旋转  、、- Parameter[2].D   
                if (x != null && x.Length > 0)
                {
                    _fitEllipseSector.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitEllipseSector.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, 0, image.Grab_X, image.Grab_Y, image.CamParams);
                    }
                    if (this.ellipseSectorGeometry.IsOutFitPoint)
                     _fitEllipseSector.EdgesPoint_xyz = _fitEllipseSector.GetFitWcsPoint();
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitEllipseSector.EdgesPoint_xyz, this._fitEllipseSector), null, null);
                result = true;
            }
            return result;
        }




    }
}
