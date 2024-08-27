using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class CoordSysMethod
    {
        public static bool GenCircleLineCoordPoint(userPixCircle pixCircle, userPixLine pixLine, CoordSysParam param, out userPixCoordSystem pixCoordSystem)
        {
            bool result = false;
            double rowProj, colProj;
            pixCoordSystem = new userPixCoordSystem();
            if (pixCircle == null) throw new ArgumentNullException("pixCircle");
            if (pixLine == null) throw new ArgumentNullException("pixLine");
            /////////////
            HMisc.ProjectionPl(pixCircle.Row, pixCircle.Col, pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, out rowProj, out colProj); // 世界点与像素点刚好需要取反
            double phi = Math.Atan2((pixLine.Row2 - pixLine.Row1) * -1, pixLine.Col2 - pixLine.Col1);
            //double deg = phi * 180 / Math.PI;
            //////////////////////////////
            switch (param.OrigionType)
            {
                // 因为世界坐标系与图像坐标系相反，所以角度需要取反
                case enOrigionType.圆心:
                    pixCoordSystem.CurrentPoint = new userPixVector(pixCircle.Row, pixCircle.Col, phi, pixCircle.CamParams);
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = pixCircle.Row;
                        param.RefPoint_Col = pixCircle.Col;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
                case enOrigionType.交点:
                    pixCoordSystem.CurrentPoint = new userPixVector(rowProj, colProj, phi, pixCircle.CamParams);
                    if (param.IsInit)
                    {
                        param.RefPoint_Row = rowProj;
                        param.RefPoint_Col = colProj;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
                default:
                    pixCoordSystem.CurrentPoint = new userPixVector(rowProj, colProj, phi, pixCircle.CamParams);
                    if (param.IsInit)
                    {
                        param.RefPoint_Row = rowProj;
                        param.RefPoint_Col = colProj;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
            }
            if (pixCircle.Grab_x == pixLine.Grab_x)
                pixCoordSystem.ReferencePoint.Grab_x = pixCircle.Grab_x;
            else
                pixCoordSystem.ReferencePoint.Grab_x = 0;
            if (pixCircle.Grab_y == pixLine.Grab_y)
                pixCoordSystem.ReferencePoint.Grab_y = pixCircle.Grab_y;
            else
                pixCoordSystem.ReferencePoint.Grab_y = 0;
            if (pixCircle.Grab_theta == pixLine.Grab_theta)
                pixCoordSystem.ReferencePoint.Grab_theta = pixCircle.Grab_theta;
            else
                pixCoordSystem.ReferencePoint.Grab_theta = 0;
            if (pixCircle.Grab_x == pixLine.Grab_x)
                pixCoordSystem.CurrentPoint.Grab_x = pixCircle.Grab_x;
            else
                pixCoordSystem.CurrentPoint.Grab_x = 0;
            if (pixCircle.Grab_y == pixLine.Grab_y)
                pixCoordSystem.CurrentPoint.Grab_y = pixCircle.Grab_y;
            else
                pixCoordSystem.CurrentPoint.Grab_y = 0;
            if (pixCircle.Grab_theta == pixLine.Grab_theta)
                pixCoordSystem.CurrentPoint.Grab_theta = pixCircle.Grab_theta;
            else
                pixCoordSystem.CurrentPoint.Grab_theta = 0;
            pixCoordSystem.ReferencePoint.CamName = pixCircle.CamName;
            pixCoordSystem.ReferencePoint.ViewWindow = pixCircle.ViewWindow;
            pixCoordSystem.CurrentPoint.CamName = pixCircle.CamName;
            pixCoordSystem.CurrentPoint.ViewWindow = pixCircle.ViewWindow;
            pixCoordSystem.CurrentPoint.CamParams = pixCircle.CamParams;
            pixCoordSystem.ReferencePoint.CamParams = pixCircle.CamParams;

            return result;
        }
        public static bool GenLineLineCoordPoint(userPixLine pixLine1, userPixLine pixLine2, CoordSysParam param, out userPixCoordSystem pixCoordSystem)
        {
            bool result = false;
            double row, col;
            int isParallel;
            pixCoordSystem = new userPixCoordSystem();
            /////////////////////////////
            if (pixLine1 == null) throw new ArgumentNullException("pixLine1");
            if (pixLine2 == null) throw new ArgumentNullException("pixLine2");
            //////////////////
            HMisc.IntersectionLl(pixLine1.Row1, pixLine1.Col1, pixLine1.Row2, pixLine1.Col2,
                                  pixLine2.Row1, pixLine2.Col1, pixLine2.Row2, pixLine2.Col2, out row, out col, out isParallel);
            double phi = Math.Atan2((pixLine1.Row2 - pixLine1.Row1) * -1, pixLine1.Col2 - pixLine1.Col1);
            //double deg = phi * 180 / Math.PI;
            ///////////////////////////////////////
            if (isParallel == 0) // 表示有交点
            {
                if (!param.IsInit)
                {
                    param.RefPoint_Row = row;
                    param.RefPoint_Col = col;
                    param.RefPoint_Rad = phi;
                    param.IsInit = true;
                }
            }
            else
            {
                if (!param.IsInit)
                {
                    param.RefPoint_Row = pixLine1.Row1;
                    param.RefPoint_Col = pixLine1.Col1;
                    param.RefPoint_Rad = phi;
                    param.IsInit = true;
                }
            }
            pixCoordSystem.CurrentPoint = new userPixVector(row, col, phi, pixLine1.CamParams);
            /////////////////////////////
            if (pixLine1.Grab_x == pixLine2.Grab_x)
                pixCoordSystem.ReferencePoint.Grab_x = pixLine1.Grab_x;
            else
                pixCoordSystem.ReferencePoint.Grab_x = 0;
            if (pixLine1.Grab_y == pixLine2.Grab_y)
                pixCoordSystem.ReferencePoint.Grab_y = pixLine1.Grab_y;
            else
                pixCoordSystem.ReferencePoint.Grab_y = 0;
            if (pixLine1.Grab_theta == pixLine2.Grab_theta)
                pixCoordSystem.ReferencePoint.Grab_theta = pixLine1.Grab_theta;
            else
                pixCoordSystem.ReferencePoint.Grab_theta = 0;
            if (pixLine1.Grab_x == pixLine2.Grab_x)
                pixCoordSystem.CurrentPoint.Grab_x = pixLine1.Grab_x;
            else
                pixCoordSystem.CurrentPoint.Grab_x = 0;
            if (pixLine1.Grab_y == pixLine2.Grab_y)
                pixCoordSystem.CurrentPoint.Grab_y = pixLine1.Grab_y;
            else
                pixCoordSystem.CurrentPoint.Grab_y = 0;
            if (pixLine1.Grab_theta == pixLine2.Grab_theta)
                pixCoordSystem.CurrentPoint.Grab_theta = pixLine1.Grab_theta;
            else
                pixCoordSystem.CurrentPoint.Grab_theta = 0;
            pixCoordSystem.ReferencePoint.CamName = pixLine1.CamName;
            pixCoordSystem.ReferencePoint.ViewWindow = pixLine1.ViewWindow;
            pixCoordSystem.CurrentPoint.CamName = pixLine1.CamName;
            pixCoordSystem.CurrentPoint.ViewWindow = pixLine1.ViewWindow;
            pixCoordSystem.CurrentPoint.CamParams = pixLine1.CamParams;
            pixCoordSystem.ReferencePoint.CamParams = pixLine1.CamParams;
            result = true;
            return result;
        }
        public static bool GenLineCoordPoint(userPixLine pixLine, CoordSysParam param, out userPixCoordSystem pixCoordSystem)
        {
            bool result = false;
            pixCoordSystem = new userPixCoordSystem();
            //////////////
            if (pixLine == null) throw new ArgumentNullException("pixLine");
            double phi = Math.Atan2((pixLine.Row2 - pixLine.Row1) * -1, pixLine.Col2 - pixLine.Col1);
            //double deg = phi * 180 / Math.PI;
            //////////////////////////////////
            switch (param.OrigionType)
            {
                case enOrigionType.直线起点:
                    pixCoordSystem.CurrentPoint = new userPixVector(pixLine.Row1, pixLine.Col1, phi, pixLine.CamParams); // 世界坐标系与像素坐标系Y轴相反
                    if (param.IsInit)
                    {
                        param.RefPoint_Row = pixLine.Row1;
                        param.RefPoint_Col = pixLine.Col1;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
                case enOrigionType.直线终点:
                    pixCoordSystem.CurrentPoint = new userPixVector(pixLine.Row2, pixLine.Col2, phi, pixLine.CamParams); // 世界坐标系与像素坐标系Y轴相反
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = (pixLine.Row2 + pixLine.Row1) / 2.0;
                        param.RefPoint_Col = (pixLine.Col1 + pixLine.Col2) / 2.0;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
                case enOrigionType.直线中点:
                    pixCoordSystem.CurrentPoint = new userPixVector((pixLine.Row1 + pixLine.Row2) / 2.0, (pixLine.Col1 + pixLine.Col2) / 2.0, phi, pixLine.CamParams); // 世界坐标系与像素坐标系Y轴相反
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = (pixLine.Row1 + pixLine.Row2) / 2.0;
                        param.RefPoint_Col = (pixLine.Col1 + pixLine.Col2) / 2.0;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
                default:
                    pixCoordSystem.CurrentPoint = new userPixVector((pixLine.Row1 + pixLine.Row2) / 2.0, (pixLine.Col1 + pixLine.Col2) / 2.0, phi, pixLine.CamParams); // 世界坐标系与像素坐标系Y轴相反
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = (pixLine.Row1 + pixLine.Row2) / 2.0;
                        param.RefPoint_Col = (pixLine.Col1 + pixLine.Col2) / 2.0;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
                case enOrigionType.X轴交点:
                    double row, col;
                    if (pixLine.CamParams != null)
                    {
                        int ispara = 0;
                        //pixLine.CamParams.ImagePointsToWorldPlane(pixLine.CamParams.DataHeight * 0.5, 0, pixLine.Grab_x, pixLine.Grab_y, out wcs_x1, out wcs_y1);
                        //pixLine.CamParams.ImagePointsToWorldPlane(pixLine.CamParams.DataHeight * 0.5, pixLine.CamParams.DataWidth, pixLine.Grab_x, pixLine.Grab_y, out wcs_x2, out wcs_y2);
                        HMisc.IntersectionLl(pixLine.CamParams.DataHeight * 0.5, 0, pixLine.CamParams.DataHeight * 0.5, pixLine.CamParams.DataWidth, pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, out row, out col, out ispara);
                        pixCoordSystem.CurrentPoint = new userPixVector(row, col, phi, pixLine.CamParams);
                    }
                    else
                        pixCoordSystem.CurrentPoint = new userPixVector((pixLine.Row1 + pixLine.Row2) / 2.0, (pixLine.Col1 + pixLine.Col2) / 2.0, phi, pixLine.CamParams); // 世界坐标系与像素坐标系Y轴相反
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = (pixLine.Row1 + pixLine.Row2) / 2.0;
                        param.RefPoint_Col = (pixLine.Col1 + pixLine.Col2) / 2.0;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
                case enOrigionType.Y轴交点:
                    //double wcs_x1, wcs_y1, wcs_x2, wcs_y2, x, y;
                    if (pixLine.CamParams != null)
                    {
                        int ispara = 0;
                        //pixLine.CamParams.ImagePointsToWorldPlane(0, pixLine.CamParams.DataWidth * 0.5, pixLine.Grab_x, pixLine.Grab_y, out wcs_x1, out wcs_y1);
                        //pixLine.CamParams.ImagePointsToWorldPlane(pixLine.CamParams.DataHeight, pixLine.CamParams.DataWidth * 0.5, pixLine.Grab_x, pixLine.Grab_y, out wcs_x2, out wcs_y2);
                        HMisc.IntersectionLl(0, pixLine.CamParams.DataWidth * 0.5, pixLine.CamParams.DataHeight, pixLine.CamParams.DataWidth * 0.5, pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, out row, out col, out ispara);
                        pixCoordSystem.CurrentPoint = new userPixVector(row, col, phi, pixLine.CamParams);
                    }
                    else
                    {
                        row = (pixLine.Row1 + pixLine.Row2) / 2.0;
                        col = (pixLine.Col1 + pixLine.Col2) / 2.0;
                        pixCoordSystem.CurrentPoint = new userPixVector(row, col, phi, pixLine.CamParams); // 世界坐标系与像素坐标系Y轴相反
                    }
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = row;
                        param.RefPoint_Col = col;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
            }
            ////////////////////////
            pixCoordSystem.ReferencePoint.Grab_x = pixLine.Grab_x;
            pixCoordSystem.ReferencePoint.Grab_y = pixLine.Grab_y;
            pixCoordSystem.ReferencePoint.Grab_theta = pixLine.Grab_theta;
            pixCoordSystem.CurrentPoint.Grab_x = pixLine.Grab_x;
            pixCoordSystem.CurrentPoint.Grab_y = pixLine.Grab_y;
            pixCoordSystem.CurrentPoint.Grab_theta = pixLine.Grab_theta;
            pixCoordSystem.ReferencePoint.CamName = pixLine.CamName;
            pixCoordSystem.ReferencePoint.ViewWindow = pixLine.ViewWindow;
            pixCoordSystem.CurrentPoint.CamName = pixLine.CamName;
            pixCoordSystem.CurrentPoint.ViewWindow = pixLine.ViewWindow;
            pixCoordSystem.CurrentPoint.CamParams = pixLine.CamParams;
            pixCoordSystem.ReferencePoint.CamParams = pixLine.CamParams;
            return result;
        }
        public static bool GenRect2CoordPoint(userPixRectangle2 pixRect2, CoordSysParam param, out userPixCoordSystem pixCoordSystem)
        {
            bool result = false;
            pixCoordSystem = new userPixCoordSystem();
            //////////////
            if (pixRect2 == null) throw new ArgumentNullException("pixRect2");
            //////////////////////////////////
            switch (param.OrigionType)
            {
                default:
                case enOrigionType.直线起点:
                    pixCoordSystem.CurrentPoint = new userPixVector(pixRect2.Row, pixRect2.Col, pixRect2.Rad, pixRect2.CamParams); // 世界坐标系与像素坐标系Y轴相反
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = pixRect2.Row;
                        param.RefPoint_Col = pixRect2.Col;
                        param.RefPoint_Rad = pixRect2.Rad;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
            }
            ////////////////////////
            pixCoordSystem.ReferencePoint.Grab_x = pixRect2.Grab_x;
            pixCoordSystem.ReferencePoint.Grab_y = pixRect2.Grab_y;
            pixCoordSystem.ReferencePoint.Grab_theta = pixRect2.Grab_theta;
            pixCoordSystem.CurrentPoint.Grab_x = pixRect2.Grab_x;
            pixCoordSystem.CurrentPoint.Grab_y = pixRect2.Grab_y;
            pixCoordSystem.CurrentPoint.Grab_theta = pixRect2.Grab_theta;
            pixCoordSystem.ReferencePoint.CamName = pixRect2.CamName;
            pixCoordSystem.ReferencePoint.ViewWindow = pixRect2.ViewWindow;
            pixCoordSystem.CurrentPoint.CamName = pixRect2.CamName;
            pixCoordSystem.CurrentPoint.ViewWindow = pixRect2.ViewWindow;
            pixCoordSystem.CurrentPoint.CamParams = pixRect2.CamParams;
            pixCoordSystem.ReferencePoint.CamParams = pixRect2.CamParams;
            return result;
        }
        public static bool GenPointLineCoordPoint(userPixPoint pixPoint, userPixLine pixLine, CoordSysParam param, out userPixCoordSystem pixCoordSystem)
        {
            bool result = false;
            double rowProj, colProj;
            pixCoordSystem = new userPixCoordSystem();
            if (pixPoint == null) throw new ArgumentNullException("pixPoint");
            if (pixLine == null) throw new ArgumentNullException("pixLine");
            /// 在像素坐标系中使用世界坐标计算时，及在世界坐标系中使用像素坐标计算时，注意Y轴是相反的
            double phi = Math.Atan2((pixLine.Row2 - pixLine.Row1) * -1, pixLine.Col2 - pixLine.Col1); // 因为图像坐标系与世界坐标系Y轴相反，所以这里需要将Row计算取反

            //phi = HMisc.AngleLl(0, 0, 0, 1, pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2);
            //double deg = phi * 180 / Math.PI;
            /////////////////////////
            HMisc.ProjectionPl(pixPoint.Row, pixPoint.Col, pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, out rowProj, out colProj);
            //////////////////////////////////////////////////////////////////
            switch (param.OrigionType)
            {
                default:
                case enOrigionType.交点:
                    pixCoordSystem.CurrentPoint = new userPixVector(rowProj, colProj, phi, pixPoint.CamParams);
                    if (!param.IsInit)
                    {
                        param.RefPoint_Row = rowProj;
                        param.RefPoint_Col = colProj;
                        param.RefPoint_Rad = phi;
                        param.IsInit = true;
                    }
                    result = true;
                    break;
            }
            //////////////////////
            if (pixPoint.Grab_x == pixLine.Grab_x)
                pixCoordSystem.ReferencePoint.Grab_x = pixPoint.Grab_x;
            else
                pixCoordSystem.ReferencePoint.Grab_x = 0;
            if (pixPoint.Grab_y == pixLine.Grab_y)
                pixCoordSystem.ReferencePoint.Grab_y = pixPoint.Grab_y;
            else
                pixCoordSystem.ReferencePoint.Grab_y = 0;
            if (pixPoint.Grab_theta == pixLine.Grab_theta)
                pixCoordSystem.ReferencePoint.Grab_theta = pixPoint.Grab_theta;
            else
                pixCoordSystem.ReferencePoint.Grab_theta = 0;
            if (pixPoint.Grab_x == pixLine.Grab_x)
                pixCoordSystem.CurrentPoint.Grab_x = pixPoint.Grab_x;
            else
                pixCoordSystem.CurrentPoint.Grab_x = 0;
            if (pixPoint.Grab_y == pixLine.Grab_y)
                pixCoordSystem.CurrentPoint.Grab_y = pixPoint.Grab_y;
            else
                pixCoordSystem.CurrentPoint.Grab_y = 0;
            if (pixPoint.Grab_theta == pixLine.Grab_theta)
                pixCoordSystem.CurrentPoint.Grab_theta = pixPoint.Grab_theta;
            else
                pixCoordSystem.CurrentPoint.Grab_theta = 0;
            pixCoordSystem.ReferencePoint.CamName = pixPoint.CamName;
            pixCoordSystem.ReferencePoint.ViewWindow = pixPoint.ViewWindow;
            pixCoordSystem.CurrentPoint.CamName = pixPoint.CamName;
            pixCoordSystem.CurrentPoint.ViewWindow = pixPoint.ViewWindow;
            pixCoordSystem.CurrentPoint.CamParams = pixPoint.CamParams;
            pixCoordSystem.ReferencePoint.CamParams = pixPoint.CamParams;
            return result;
        }

    }


}
