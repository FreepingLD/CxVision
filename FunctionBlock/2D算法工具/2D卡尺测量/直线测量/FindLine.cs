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
    public class FindLine
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
                if (linePixPosition.CamParams != null)
                    lineWcsPosition = linePixPosition.GetWcsLine();
            }
        }
        public userWcsLine LineWcsPosition
        {
            get
            {
                if (linePixPosition.CamParams != null)
                    return linePixPosition.GetWcsLine();
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
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }
        public GeometryMeasure LineGeometry { get => lineGeometry; set => lineGeometry = value; }

        public FindLine()
        {
            this.lineGeometry = new GeometryMeasure(enMeasureType.line);
        }

        public bool FindLineMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (image.Image == null || !image.Image.IsInitialized())
            {
                throw new ArgumentNullException("参数image中的图像对象为空值或未被初始化");
            }
            if (wcsCoordSystem.ReferencePoint.X == 0 && wcsCoordSystem.ReferencePoint.Y == 0)
                this.wcsCoordSystem = new userWcsCoordSystem();
            else
                this.wcsCoordSystem = wcsCoordSystem;
            image.Image.GetImageSize(out width, out height);
            this._fitLine = new userWcsLine(image.CamParams);
            /////////////////////////////////////
            userPixLine linePixPosition = this.LineWcsPosition.AffineWcsLine2D(this.wcsCoordSystem.GetVariationHomMat2D()).GetPixLine(); // 经坐标变换后的像素位置
            this.lineGeometry.CreateLineMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, linePixPosition.DiffRadius, linePixPosition.NormalPhi, width, height);
            this.lineGeometry.ApplyMeasurePose(image.Image);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitLine.CamName = image.CamName;
                _fitLine = new userWcsLine(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams);
                _fitLine.Grab_z = image.Grab_Z;
                _fitLine.Grab_u = image.Grab_U;
                _fitLine.Grab_v = image.Grab_V;
                _fitLine.Grab_theta = image.Grab_Theta;
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                _fitLine.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    _fitLine.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitLine.EdgesPoint_xyz, _fitLine), null, null);
                result = true;
            }
            return result;
        }

        public bool FindLineMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (image.Image == null || !image.Image.IsInitialized())
            {
                throw new ArgumentNullException("参数image中的图像对象为空值或未被初始化");
            }
            if (pixCoordSystem != null && pixCoordSystem.ReferencePoint.Row == 0 && pixCoordSystem.ReferencePoint.Col == 0)
                this.pixCoordSystem = new userPixCoordSystem();
            else
                this.pixCoordSystem = pixCoordSystem;
            image.Image.GetImageSize(out width, out height);
            this._fitLine = new userWcsLine(image.CamParams);
            /////////////////////////////////////
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵,用于阵列测量
            userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(homMat2DCompose); // 经坐标变换后的像素位置
            //userPixLine linePixPosition = this.LinePixPosition.AffinePixLine2D(this.pixCoordSystem?.GetVariationHomMat2D()); // 经坐标变换后的像素位置
            this.lineGeometry.CreateLineMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, linePixPosition.DiffRadius, linePixPosition.NormalPhi, width, height);
            this.lineGeometry.ApplyMeasurePose(image.Image);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                _fitLine.CamName = image.CamName;
                _fitLine = new userWcsLine(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams);
                _fitLine.Angle = Math.Atan2(_fitLine.Y2 - _fitLine.Y1, _fitLine.X2 - _fitLine.X1);
                _fitLine.Grab_z = image.Grab_Z;
                _fitLine.Grab_u = image.Grab_U;
                _fitLine.Grab_v = image.Grab_V;
                _fitLine.Grab_theta = image.Grab_Theta;
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                if (x != null && x.Length > 0)
                {
                    _fitLine.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitLine.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                    }
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(_fitLine.EdgesPoint_xyz, _fitLine), null, null);
                result = true;
            }
            return result;
        }

        public bool FindLineMethod(ImageDataClass image, userWcsCoordSystem wcsCoordSystem, out userWcsLine fitLine)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (image.Image == null || !image.Image.IsInitialized())
            {
                throw new ArgumentNullException("参数image中的图像对象为空值或未被初始化");
            }
            fitLine = new userWcsLine(image.CamParams);
            this.wcsCoordSystem = wcsCoordSystem;
            image.Image.GetImageSize(out width, out height);
            /////////////////////////////////////
            //this.lineWcsPosition.refPoint_x = image.Grab_X; // 采集位置也要做变换，参考位置要与图像位置相符
            //this.lineWcsPosition.refPoint_y = image.Grab_Y;
            userPixLine linePixPosition = this.lineWcsPosition.AffineWcsLine2D(wcsCoordSystem.GetVariationHomMat2D()).GetPixLine(); // 经坐标变换后的像素位置
            this.lineGeometry.CreateLineMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, linePixPosition.DiffRadius, linePixPosition.NormalPhi, width, height);
            //this.lineGeometry.SetCameraParam(image.CamParam, image.CamPose);
            this.lineGeometry.ApplyMeasurePose(image.Image);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            this.lineGeometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                fitLine.CamName = image.CamName;
                fitLine = new userWcsLine(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams);
                fitLine.Grab_z = image.Grab_Z;
                fitLine.Grab_u = image.Grab_U;
                fitLine.Grab_v = image.Grab_V;
                fitLine.Grab_theta = image.Grab_Theta;
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                //_fitLine.edgesPoint_y = y;
                //_fitLine.edgesPoint_x = x;
                fitLine.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    fitLine.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(fitLine.EdgesPoint_xyz, fitLine), null, null);
                result = true;
            }
            return result;
        }




    }
}
