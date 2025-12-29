using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class CalliperMethod
    {

        private static object lockState = new object();
        private static CalliperMethod _Instance = null;
        private CalliperMethod()
        {

        }
        public static CalliperMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new CalliperMethod();
                    }
                }
                return _Instance;
            }
        }

        public bool FindPointMethod(ImageDataClass image, PointCalliperParam param,out userWcsPoint wcsPoint)
        {
            bool result = false;
            HTuple Parameter, row, col, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            wcsPoint = new userWcsPoint(image.CamParams);
            ////////////////////////////////
            image.Image.GetImageSize(out width, out height);
            /////////////////////////////////////
            param.LineWcsPosition.Grab_x = image.Grab_X; // 采集位置也要做变换，参考位置要与图像位置相符
            param.LineWcsPosition.Grab_y = image.Grab_Y;
            userPixLine linePixPosition = param.LineWcsPosition.AffineWcsLine2D(param.WcsCoordSystem.GetVariationHomMat2D()).GetPixLine(); // 经坐标变换后的像素位置
            param.Geometry.CreatePointMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, width, height);
            param.Geometry.ApplyMeasurePose(image.Image);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); // image.CalibrateFile
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.column_Edges1, out x);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.row_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                wcsPoint.CamName = image.CamName;
                wcsPoint = new userWcsPoint(Parameter[0].D, Parameter[1].D, Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                wcsPoint.EdgesPoint_xyz = new userWcsPoint[1] { wcsPoint };
                wcsPoint.Grab_z = image.Grab_Z;
                wcsPoint.Grab_u = image.Grab_U;
                wcsPoint.Grab_v = image.Grab_V;
                wcsPoint.Grab_theta = image.Grab_Theta;
                //param.MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(new double[] { wcsPoint.edgesPoint_x }, new double[] { wcsPoint.edgesPoint_y }, wcsPoint), null, null);
                result = true;
            }
            return result;
        }

        public bool FindLineMethod(ImageDataClass image, LineCalliperParam param,out userWcsLine wcsLine)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            wcsLine = new userWcsLine(image.CamParams);
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (image.Image == null || !image.Image.IsInitialized())
            {
                throw new ArgumentNullException("参数image中的图像对象为空值或未被初始化");
            }
            image.Image.GetImageSize(out width, out height);
            /////////////////////////////////////
            param.LineWcsPosition.Grab_x = image.Grab_X; // 采集位置也要做变换，参考位置要与图像位置相符
            param.LineWcsPosition.Grab_y = image.Grab_Y;
            userPixLine linePixPosition = param.LineWcsPosition.AffineWcsLine2D(param.WcsCoordSystem.GetVariationHomMat2D()).GetPixLine(); // 经坐标变换后的像素位置
            param.Geometry.CreateLineMeasure(linePixPosition.Row1, linePixPosition.Col1, linePixPosition.Row2, linePixPosition.Col2, linePixPosition.DiffRadius, linePixPosition.NormalPhi, width, height);
            //param.Geometry.SetCameraParam(image.CamParam, image.CamPose);
            param.Geometry.ApplyMeasurePose(image.Image);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                wcsLine = new userWcsLine(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams);
                //// 添加边缘点到结果中，这样一来，就可以最终使用边缘点来拟合圆或椭圆了
                wcsLine.EdgesPoint_xyz = new userWcsPoint[x.Length];
                wcsLine.Grab_z = image.Grab_Z;
                wcsLine.Grab_u = image.Grab_U;
                wcsLine.Grab_v = image.Grab_V;
                wcsLine.Grab_theta = image.Grab_Theta;
                for (int i = 0; i < x.Length; i++)
                {
                    wcsLine.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], 0, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                //MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(wcsLine.edgesPoint_xyz, wcsLine), null, null);
                result = true;
            }
            return result;
        }

        public bool FindCircleMethod(ImageDataClass image, CircleCalliperParam param ,out userWcsCircle wcsCircle)
        {
            bool result = false;
            HTuple Parameter, Row, Col, x, y, cla_x, cal_y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            wcsCircle = new userWcsCircle(image.CamParams);
            ////////////////////////////////////
            image.Image.GetImageSize(out width, out height);
            param.CircleWcsPosition.Grab_x = image.Grab_X; // 采集位置也要做变换，参考位置要与图像位置相符
            param.CircleWcsPosition.Grab_y = image.Grab_Y;
            userPixCircle circlePixPosition = param.CircleWcsPosition.AffineWcsCircle2D(param.WcsCoordSystem.GetVariationHomMat2D()).GetPixCircle(); // 使用世界点来变换，然后再转换为像素点
            /////////////////////////////////////
            param.Geometry.CreateCircleMeasure(circlePixPosition.Row, circlePixPosition.Col, circlePixPosition.Radius, 0, Math.PI * 2, circlePixPosition.DiffRadius, circlePixPosition.PointOrder, width, height);
            param.Geometry.ApplyMeasurePose(image.Image);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter); //image.CalibrateFile
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_X, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                wcsCircle = new userWcsCircle(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, image.Grab_X, image.Grab_Y, image.CamParams);
                wcsCircle.EdgesPoint_xyz = new userWcsPoint[x.Length];
                wcsCircle.Grab_z = image.Grab_Z;
                wcsCircle.Grab_u = image.Grab_U;
                wcsCircle.Grab_v = image.Grab_V;
                wcsCircle.Grab_theta = image.Grab_Theta;
                for (int i = 0; i < x.Length; i++)
                {
                    wcsCircle.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, 0, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                //MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(wcsCircle.edgesPoint_xyz, this.wcsCircle), null, null);
                result = true;
            }
            return result;
        }

        public bool FindEllipseMethod(ImageDataClass image, EllipseCalliperParam param,out userWcsEllipse wcsEllipse)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            wcsEllipse = new userWcsEllipse(image.CamParams);
            image.Image.GetImageSize(out width, out height);
            ///////////////////
            param.EllipseWcsPosition.Grab_x = image.Grab_X; // 采集位置也要做变换，参考位置要与图像位置相符
            param.EllipseWcsPosition.Grab_y = image.Grab_Y;
            userPixEllipse ellipsePixPosition = param.EllipseWcsPosition.AffineWcsEllipse2D(param.WcsCoordSystem.GetVariationHomMat2D()).GetPixEllipse();
            param.Geometry.CreateEllipseMeasure(ellipsePixPosition.Row, ellipsePixPosition.Col, ellipsePixPosition.Rad, ellipsePixPosition.Radius1, ellipsePixPosition.Radius2, 0, Math.PI * 2, ellipsePixPosition.DiffRadius, width, height);
            //param.Geometry.SetCameraParam(image.CamParam, image.CamPose);
            param.Geometry.ApplyMeasurePose(image.Image);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            /////////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                wcsEllipse.CamName = image.CamName;
                wcsEllipse = new userWcsEllipse(Parameter[0], Parameter[1], Parameter[2], Parameter[3], Parameter[4], Parameter[5], image.Grab_X, image.Grab_Y, image.CamParams);
                wcsEllipse.EdgesPoint_xyz = new userWcsPoint[x.Length];
                wcsEllipse.Grab_z = image.Grab_Z;
                wcsEllipse.Grab_u = image.Grab_U;
                wcsEllipse.Grab_v = image.Grab_V;
                wcsEllipse.Grab_theta = image.Grab_Theta;
                for (int i = 0; i < x.Length; i++)
                {
                    wcsEllipse.EdgesPoint_xyz[i] = new userWcsPoint(x[i].D, y[i].D, Parameter[2], image.Grab_X, image.Grab_Y, image.CamParams);
                }
                //MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(wcsEllipse.edgesPoint_xyz, this.wcsEllipse), null, null);
                result = true;
            }
            return result;
        }

        public bool FindRect2Method(ImageDataClass image, Rect2CalliperParam param,out userWcsRectangle2 wcsRectangle2)
        {
            bool result = false;
            HTuple Parameter, x, y;
            int width, height;
            if (image == null) throw new ArgumentNullException("image");
            wcsRectangle2 = new userWcsRectangle2(image.CamParams);
            /////////////////////////////
            image.Image.GetImageSize(out width, out height);
            ////////////////////////////
            param.Rect2WcsPosition.Grab_x = image.Grab_X; // 采集位置也要做变换，参考位置要与图像位置相符
            param.Rect2WcsPosition.Grab_y = image.Grab_Y;
            userPixRectangle2 rect2PixPosition = param.Rect2WcsPosition.AffineWcsRectangle2D(param.WcsCoordSystem.GetVariationHomMat2D()).GetPixRectangle2();
            param.Geometry.CreateRect2Measure(rect2PixPosition.Row, rect2PixPosition.Col, rect2PixPosition.Rad, rect2PixPosition.Length1, rect2PixPosition.Length2, rect2PixPosition.DiffRadius, width, height);
            //param.Geometry.SetCameraParam(image.CamParam, image.CamPose);
            param.Geometry.ApplyMeasurePose(image.Image);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.fitResult_Edges1, out Parameter);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.X_Edges1, out x);
            param.Geometry.GetMeasureObjectParam(image.CamParams, image.Grab_X, image.Grab_Y, image.Grab_Z, enParamType.Y_Edges1, out y);
            ////////////////
            if (Parameter != null && Parameter.Length > 0)
            {
                wcsRectangle2.CamName = image.CamName;
                wcsRectangle2 = new userWcsRectangle2(Parameter[0].D, Parameter[1].D, Parameter[2].D, Parameter[3].D, Parameter[4].D, Parameter[5].D, image.Grab_X, image.Grab_Y, image.CamParams);
                wcsRectangle2.Grab_z= image.Grab_Z;
                wcsRectangle2.Grab_u = image.Grab_U;
                wcsRectangle2.Grab_v = image.Grab_V;
                wcsRectangle2.Grab_theta = image.Grab_Theta;
                wcsRectangle2.EdgesPoint_xyz = new userWcsPoint[x.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    wcsRectangle2.EdgesPoint_xyz[i] = new userWcsPoint(x[i], y[i], Parameter[2].D, image.Grab_X, image.Grab_Y, image.CamParams);
                }
                //MetrolegyComplete?.BeginInvoke(new MetrolegyCompletedEventArgs(wcsRectangle2.edgesPoint_xyz, this.wcsRectangle2), null, null);
                result = true;
            }
            return result;
        }




    }

   


}
