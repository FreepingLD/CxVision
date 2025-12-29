using Common;
using HalconDotNet;
using System;
using System.Threading;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    public class FindManualPolyLine
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        private userWcsPolyLine _fitPolyLine;
        private GeometryMeasure polyLineGeometry;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        private userWcsPolyLine polyLineWcsPosition;
        private userPixPolyLine polyLinePixPosition = new userPixPolyLine(new double[] { 100, 300 }, new double[] { 100, 100 }); // 
        public userPixPolyLine PolyLinePixPosition
        {
            get
            {
                return polyLinePixPosition;
            }
            set
            {
                polyLinePixPosition = value;
                if (this.polyLinePixPosition.CamParams != null)
                    this.polyLineWcsPosition = polyLinePixPosition.GetWcsPolyLine();
            }
        }
        public userWcsPolyLine PolyLineWcsPosition
        {
            get
            {
                if (this.polyLinePixPosition.CamParams != null)
                    return this.polyLinePixPosition.GetWcsPolyLine();
                else
                    return polyLineWcsPosition;
            }
            set
            {
                polyLineWcsPosition = value;
                this.polyLinePixPosition = polyLineWcsPosition.GetPixPolyLine();
            }
        }

        public userWcsPolyLine FitPolyLine { get => _fitPolyLine; set => _fitPolyLine = value; }
        public GeometryMeasure PolyLineGeometry { get => polyLineGeometry; set => polyLineGeometry = value; }
        public userWcsCoordSystem WcsCoordSystem { get => wcsCoordSystem; set => wcsCoordSystem = value; }

        public FindManualPolyLine()
        {
            this.polyLineGeometry = new GeometryMeasure(enMeasureType.polyLine);
        }


        public bool FindPolyLineMethod(ImageDataClass image, userPixCoordSystem pixCoordSystem, userPixCoordSystem offsetCoordSystem = null)
        {
            bool result = false;
            HTuple Parameter, x, y,z;
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
            {
                this.pixCoordSystem = new userPixCoordSystem();
                this.pixCoordSystem.Result = true;
            }
            else
                this.pixCoordSystem = pixCoordSystem;
            image.Image.GetImageSize(out width, out height);
            this._fitPolyLine = new userWcsPolyLine(image.CamParams);
            /////////////////////////////////////
            HTuple homMat2DCompose;
            HOperatorSet.HomMat2dCompose(this.pixCoordSystem?.GetVariationHomMat2D(), offsetCoordSystem?.GetVariationHomMat2D(), out homMat2DCompose); // 合并变换矩阵,用于阵列测量
            userPixPolyLine linePixPosition = this.polyLinePixPosition.AffinePixPolyLine(homMat2DCompose); // 经坐标变换后的像素位置
            //this.polyLineGeometry.CreatePolyLineMeasure(linePixPosition.Row, linePixPosition.Col, linePixPosition.DiffRadius, linePixPosition.NormalPhi.ToArray(), width, height);
            image.CamParams.ImagePointsToWorldPlane(linePixPosition.Row.ToArray(), linePixPosition.Col.ToArray(), image.Grab_X, image.Grab_Y, image.Grab_Z,out x, out y, out z);
            /////////////////////
            if (x != null && x.Length > 0)
            {
                this._fitPolyLine = new userWcsPolyLine(image.CamParams);
                this._fitPolyLine.Grab_x = image.Grab_X;
                this._fitPolyLine.Grab_y = image.Grab_Y;
                this._fitPolyLine.Grab_z = image.Grab_Z;
                this._fitPolyLine.Grab_u = image.Grab_U;
                this._fitPolyLine.Grab_v = image.Grab_V;
                this._fitPolyLine.Grab_theta = image.Grab_Theta;
                this._fitPolyLine.CamName = image.CamName;
                this._fitPolyLine.ViewWindow = image.ViewWindow;
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                if (x != null && x.Length > 0)
                {
                    _fitPolyLine.X?.Clear();
                    _fitPolyLine.Y?.Clear();
                    _fitPolyLine.EdgesPoint_xyz = new userWcsPoint[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        _fitPolyLine.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], 0, image.Grab_X, image.Grab_Y, image.CamParams);
                        _fitPolyLine.X.Add(x[i]);
                        _fitPolyLine.Y.Add(y[i]);
                    }
                }
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
