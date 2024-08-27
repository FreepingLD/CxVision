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
using System.Threading;
using Light;

namespace FunctionBlock
{
    [Serializable]
    public class FindCircle
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsCircle _fitCircle;
        private GeometryMeasure circleGeometry;
        private userPixCircle circlePixPosition;// 参考圆像素坐标位置
        private userWcsCircle circleWcsPosition; // 参考圆世界坐标位置
        public userPixCircle CirclePixPosition
        {
            get
            {
                return circlePixPosition;
            }

            set
            {
                circlePixPosition = value;
                if (circlePixPosition.CamParams != null)
                    circleWcsPosition = circlePixPosition.GetWcsCircle();
            }
        }
        public userWcsCircle CircleWcsPosition
        {
            get
            {
                if (circlePixPosition.CamParams != null)
                    return circlePixPosition.GetWcsCircle();
                else
                    return circleWcsPosition;
            }

            set
            {
                circleWcsPosition = value;
                circlePixPosition = circleWcsPosition.GetPixCircle();
            }
        }

        public userWcsCircle FitCircle { get => _fitCircle; set => _fitCircle = value; }
        public GeometryMeasure CircleGeometry { get => circleGeometry; set => circleGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindCircle()
        {
            circleGeometry = new GeometryMeasure(enMeasureType.circle);
            circlePixPosition = new userPixCircle(300, 300, 100);// 参考圆像素坐标位置
        }
        public bool FindCircleMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            HTuple Parameter, Row, Col, x, y, cla_x, cal_y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitCircle = new userWcsCircle(image.CamParams);
            ////////////////////////////////////
            if (wcsCoordSystem.ReferencePoint.X == 0 && wcsCoordSystem.ReferencePoint.Y == 0)
                this.wcsCoordSystem = new userWcsCoordSystem();
            else
                this.wcsCoordSystem = wcsCoordSystem;
            image.Image.GetImageSize(out width, out height);
            userPixCircle circlePixPosition = this.CircleWcsPosition.AffineWcsCircle2D(this.wcsCoordSystem.GetVariationHomMat2D()).GetPixCircle(); // 使用世界点来变换，然后再转换为像素点
            /////////////////////////////////////
            this.circleGeometry.CreateCircleMeasure(circlePixPosition.Row, circlePixPosition.Col, circlePixPosition.Radius, 0, Math.PI * 2, circlePixPosition.DiffRadius, circlePixPosition.PointOrder, width, height);
            //this.circleGeometry.SetCameraParam(image.CamParam, image.CamPose);
            this.circleGeometry.ApplyMeasurePose(image.Image);
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitCircle.CamName = image.CamName;
                _fitCircle = new userWcsCircle(Parameter[0], Parameter[1], Parameter[2], Parameter[3], image.Grab_X, image.Grab_Y, image.CamParams);
                _fitCircle.Grab_z = image.Grab_Z;
                _fitCircle.Grab_u = image.Grab_U;
                _fitCircle.Grab_v = image.Grab_V;
                _fitCircle.Grab_theta = image.Grab_Theta;
                _fitCircle.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    _fitCircle.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2], image.Grab_X, image.Grab_Y, image.CamParams);
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitCircle.EdgesPoint_xyz, this._fitCircle), null, null);
                result = true;
            }
            return result;
        }

        public bool FindCircleMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, Row, Col, x, y, cla_x, cal_y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitCircle = new userWcsCircle(image.CamParams);
            ////////////////////////////////////
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
                this.pixCoordSystem = new userPixCoordSystem();
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
            this.circleGeometry.CreateCircleMeasure(circlePixPosition.Row, circlePixPosition.Col, circlePixPosition.Radius, 0, Math.PI * 2, circlePixPosition.DiffRadius, circlePixPosition.PointOrder, width, height);
            //this.circleGeometry.SetCameraParam(image.CamParam, image.CamPose);
            this.circleGeometry.ApplyMeasurePose(image.Image);
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitCircle.CamName = image.CamName;
                _fitCircle = new userWcsCircle(Parameter[0], Parameter[1], Parameter[2], Parameter[3], image.Grab_X, image.Grab_Y, image.CamParams);
                _fitCircle.Grab_z = image.Grab_Z;
                _fitCircle.Grab_u = image.Grab_U;
                _fitCircle.Grab_v = image.Grab_V;
                _fitCircle.Grab_theta = image.Grab_Theta;
                if (x != null && x.Length > 0)
                {
                    _fitCircle.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitCircle.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2], image.Grab_X, image.Grab_Y, image.CamParams);
                    }
                    if (this.circleGeometry.IsOutFitPoint)
                        _fitCircle.EdgesPoint_xyz = _fitCircle.GetFitWcsPoint();
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitCircle.EdgesPoint_xyz, this._fitCircle), null, null);
                result = true;
            }
            return result;
        }
        public bool FindCircleMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem, out userWcsCircle fitCircle)
        {
            bool result = false;
            HTuple Parameter, Row, Col, x, y, cla_x, cal_y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            fitCircle = new userWcsCircle(image.CamParams);
            ////////////////////////////////////
            this.wcsCoordSystem = wcsCoordSystem;
            image.Image.GetImageSize(out width, out height);
            //this.circleWcsPosition.refPoint_x = image.Grab_X; // 采集位置也要做变换，参考位置要与图像位置相符
            //this.circleWcsPosition.refPoint_y = image.Grab_Y;
            userPixCircle circlePixPosition = this.circleWcsPosition.AffineWcsCircle2D(wcsCoordSystem.GetVariationHomMat2D()).GetPixCircle(); // 使用世界点来变换，然后再转换为像素点
            /////////////////////////////////////
            this.circleGeometry.CreateCircleMeasure(circlePixPosition.Row, circlePixPosition.Col, circlePixPosition.Radius, 0, Math.PI * 2, circlePixPosition.DiffRadius, circlePixPosition.PointOrder, width, height);
            //this.circleGeometry.SetCameraParam(image.CamParam, image.CamPose);
            this.circleGeometry.ApplyMeasurePose(image.Image);
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.circleGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                fitCircle.CamName = image.CamName;
                fitCircle = new userWcsCircle(Parameter[0], Parameter[1], Parameter[2], Parameter[3], image.Grab_X, image.Grab_Y, image.CamParams);
                fitCircle.Grab_z = image.Grab_Z;
                fitCircle.Grab_u = image.Grab_U;
                fitCircle.Grab_v = image.Grab_V;
                fitCircle.Grab_theta = image.Grab_Theta;
                fitCircle.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    fitCircle.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2], image.Grab_X, image.Grab_Y, image.CamParams);
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitCircle.EdgesPoint_xyz, this._fitCircle), null, null);
                result = true;
            }
            return result;
        }



    }
}
