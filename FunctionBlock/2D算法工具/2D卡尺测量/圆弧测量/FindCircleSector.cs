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
    public class FindCircleSector
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsCircleSector _fitCircleSector;
        private GeometryMeasure circleSectorGeometry;
        private userPixCircleSector circleSectorPixPosition;
        private userWcsCircleSector circleSectorWcsPosition; // 参考直线位置
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        public userPixCircleSector CircleSectorPixPosition
        {
            get
            {
                return circleSectorPixPosition;
            }

            set
            {
                circleSectorPixPosition = value;
            }
        }
        public userWcsCircleSector CircleSectorWcsPosition
        {
            get
            {
                if (circleSectorPixPosition.CamParams != null)
                    return circleSectorPixPosition.GetWcsCircleSector();
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

        public FindCircleSector()
        {
            circleSectorGeometry = new GeometryMeasure(enMeasureType.circle);
            circleSectorPixPosition = new userPixCircleSector(300, 300, 100, 0, 3.14);
        }

        public bool FindCircleSectorMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            HTuple Parameter, x, y, row, col, cal_x, cal_y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitCircleSector = new userWcsCircleSector(image.CamParams);
            ////////////////////////////////////
            if (wcsCoordSystem.ReferencePoint.X == 0 && wcsCoordSystem.ReferencePoint.Y == 0)
                this.wcsCoordSystem = new userWcsCoordSystem();
            else
                this.wcsCoordSystem = wcsCoordSystem;
            userPixCircleSector circlePixPosition = this.CircleSectorWcsPosition.Affine2DWcsCircleSector(this.wcsCoordSystem.GetVariationHomMat2D(), enAffineTransOrientation.正向变换).GetPixCircleSector();
            image.Image.GetImageSize(out width, out height);
            this.circleSectorGeometry.CreateCircleMeasure(circlePixPosition.Row, circlePixPosition.Col, circlePixPosition.Radius, circlePixPosition.Start_phi, circlePixPosition.End_phi, circlePixPosition.DiffRadius, circlePixPosition.PointOrder, width, height);
            this.circleSectorGeometry.ApplyMeasurePose(image.Image);
            this.circleSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.circleSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.circleSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitCircleSector.CamName = image.CamName;
                _fitCircleSector = new userWcsCircleSector(Parameter[0], Parameter[1], Parameter[2].D, Parameter[3].D, this.circleSectorWcsPosition.Start_deg,
                 this.circleSectorWcsPosition.End_deg, image.Grab_X, image.Grab_Y, image.CamParams); // 图像中需要包含相机对应的内参的外参,结果需要加上一个旋转角  
                                                                                                     //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                _fitCircleSector.Grab_u = image.Grab_U;
                _fitCircleSector.Grab_v = image.Grab_V;
                _fitCircleSector.Grab_theta = image.Grab_Theta;
                _fitCircleSector.PointOrder = this.circleSectorWcsPosition.PointOrder;
                //_fitCircleSector.edgesPoint_y = y ;
                //_fitCircleSector.edgesPoint_x = x ;
                _fitCircleSector.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    _fitCircleSector.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitCircleSector.EdgesPoint_xyz, this._fitCircleSector), null, null);
                result = true;
            }
            return result;
        }

        public bool FindCircleSectorMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y, row, col, cal_x, cal_y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            _fitCircleSector = new userWcsCircleSector(image.CamParams);
            ////////////////////////////////////
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
                this.pixCoordSystem = new userPixCoordSystem();
            else
                this.pixCoordSystem = pixCoordSystem;
            ///////////////////////////////////////////////////////////////////////
            if (offsetCoordSystem == null) offsetCoordSystem = new userPixCoordSystem();
            HTuple homMat2DCompose; // 合并变换矩阵,用于阵列测量
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵,用于阵列测量
            userPixCircleSector circlePixPosition = this.CircleSectorPixPosition.AffineTransPixCircleSector(homMat2DCompose);
            //userPixCircleSector circlePixPosition = this.CircleSectorPixPosition.AffineTransPixCircleSector(this.pixCoordSystem?.GetVariationHomMat2D());
            image.Image.GetImageSize(out width, out height);
            this.circleSectorGeometry.CreateCircleMeasure(circlePixPosition.Row, circlePixPosition.Col, circlePixPosition.Radius, circlePixPosition.Start_phi, circlePixPosition.End_phi, circlePixPosition.DiffRadius, circlePixPosition.PointOrder, width, height);
            this.circleSectorGeometry.ApplyMeasurePose(image.Image);
            this.circleSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            this.circleSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.circleSectorGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitCircleSector.CamName = image.CamName;
                _fitCircleSector = new userWcsCircleSector(Parameter[0], Parameter[1], Parameter[2].D, Parameter[3].D, Parameter[4].D,
                Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams); // 图像中需要包含相机对应的内参的外参,结果需要加上一个旋转角  
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                _fitCircleSector.Grab_u = image.Grab_U;
                _fitCircleSector.Grab_v = image.Grab_V;
                _fitCircleSector.Grab_theta = image.Grab_Theta;
                _fitCircleSector.PointOrder = this.circleSectorPixPosition.PointOrder;
                if (x != null && x.Length > 0)
                {

                    _fitCircleSector.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitCircleSector.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                    }
                    if (this.circleSectorGeometry.IsOutFitPoint)
                        _fitCircleSector.EdgesPoint_xyz = _fitCircleSector.GetFitWcsPoint();
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitCircleSector.EdgesPoint_xyz, this._fitCircleSector), null, null);
                result = true;
            }
            return result;
        }



    }
}
