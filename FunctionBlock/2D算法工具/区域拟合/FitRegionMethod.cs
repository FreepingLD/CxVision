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

namespace FunctionBlock
{
    public class FitRegionMethod
    {
        private static object lockState = new object();
        private static FitRegionMethod _Instance = null;
        private FitRegionMethod()
        {

        }
        public static FitRegionMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new FitRegionMethod();
                    }
                }
                return _Instance;
            }
        }


        public bool FitCircleRegion(HRegion hRegion, CircleFitParam param, out userPixCircle[] _Circle)
        {
            bool result = false;
            _Circle = new userPixCircle[0];
            HTuple Row = 0, Column = 0, Radius = 0, StartPhi = 0, EndPhi = 0, PointOrder = 0;
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            int count = hRegion.CountObj();
            _Circle = new userPixCircle[count];
            HTuple row, col;
            for (int i = 0; i < count; i++)
            {
                hRegion.SelectObj(i + 1).GetRegionContour(out row, out col);
                if (row.Length > 0)
                {
                    new HXLDCont(row, col).FitCircleContourXld(param.Algorithm, (int)param.MaxNumPoints, param.MaxClosureDist, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
                    _Circle[i] = new userPixCircle(Row.D, Column.D, Radius.D, StartPhi.D, EndPhi.D, PointOrder.S);
                }
                else
                    _Circle[i] = new userPixCircle(0, 0, 0, 0, 0);
            }
            return result;
        }
        public bool FitEllipseRegion(HRegion hRegion, EllipseFitParam param, out userPixEllipse[] ellipse)
        {
            bool result = false;
            ellipse = new userPixEllipse[0];
            HTuple Row = 0, Column = 0, Phi = 0, Radius1 = 0, Radius2 = 0, StartPhi = 0, EndPhi = 0, PointOrder = 0;
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            int count = hRegion.CountObj();
            ellipse = new userPixEllipse[count];
            HTuple row, col;
            for (int i = 0; i < count; i++)
            {
                hRegion.SelectObj(i + 1).GetRegionContour(out row, out col);
                if (row.Length > 0)
                {
                    new HXLDCont(row, col).FitEllipseContourXld(param.Algorithm, (int)param.MaxNumPoints, param.MaxClosureDist, (int)param.ClippingEndPoints, (int)param.VossTabSize, param.Iterations, param.ClippingFactor, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                    ellipse[i] = new userPixEllipse(Row.D, Column.D, Phi.D, Radius1.D, Radius2.D, StartPhi.D, EndPhi.D, PointOrder.S);
                }
                else
                    ellipse[i] = new userPixEllipse(0, 0, 0, 0, 0, 0, 0);
            }
            return result;
        }
        public bool FitRect2Region(HRegion hRegion, Rect2FitParam param, out userPixRectangle2[] rect2)
        {
            bool result = false;
            rect2 = new userPixRectangle2[0];
            HTuple Row = 0, Column = 0, Phi = 0, Length1 = 0, Length2 = 0, PointOrder = 0;
            if (hRegion == null) throw new ArgumentNullException("hRegion");
            /////////////////////////////////
            int count = hRegion.CountObj();
            rect2 = new userPixRectangle2[count];
            HTuple row, col;
            for (int i = 0; i < count; i++)
            {
                hRegion.SelectObj(i + 1).GetRegionContour(out row, out col);
                if (row.Length > 0)
                {
                    try
                    {
                        // 形状不对，可能拟合失败
                        new HXLDCont(row, col).FitRectangle2ContourXld(param.Algorithm, (int)param.MaxNumPoints, (int)param.MaxClosureDist, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
                        rect2[i] = new userPixRectangle2(Row.D, Column.D, Phi[0].D, Length1[0].D, Length2[0].D);
                    }
                    catch
                    {
                        new HXLDCont(row, col).SmallestRectangle2Xld(out Row, out Column, out Phi, out Length1, out Length2);
                        rect2[i] = new userPixRectangle2(Row.D, Column.D, Phi[0].D, Length1[0].D, Length2[0].D);
                    }
                }
                else
                    rect2[i] = new userPixRectangle2(0, 0, 0, 0, 0);
            }
            result = true;
            return result;
        }

    }


}
