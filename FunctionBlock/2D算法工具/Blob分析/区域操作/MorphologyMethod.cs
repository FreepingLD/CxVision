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
    public class MorphologyMethod
    {
        private static object lockState = new object();
        private static MorphologyMethod _Instance = null;
        private MorphologyMethod()
        {

        }

        public static MorphologyMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new MorphologyMethod();
                    }
                }
                return _Instance;
            }
        }
        public HRegion RegionMorphology(HRegion hRegion, RegionMorphologyParam param)
        {
            HRegion tarRegion = new HRegion();
            HRegion connRegion = new HRegion();
            HRegion fillRegion = new HRegion();
            switch (param.Method)
            {
                case nameof(enRegionMorphology.closing_circle):
                    tarRegion = closing_circle(hRegion, (RegionCircleParam)param);
                    break;
                case nameof(enRegionMorphology.closing_rectangle1):
                    tarRegion = closing_rectangle1(hRegion, (RegionRectParam)param);
                    break;
                case nameof(enRegionMorphology.dilation_circle):
                    tarRegion = dilation_circle(hRegion, (RegionCircleParam)param);
                    break;
                case nameof(enRegionMorphology.dilation_rectangle1):
                    tarRegion = dilation_rectangle1(hRegion, (RegionRectParam)param);
                    break;
                case nameof(enRegionMorphology.erosion_circle):
                    tarRegion = erosion_circle(hRegion, (RegionCircleParam)param);
                    break;
                case nameof(enRegionMorphology.erosion_rectangle1):
                    tarRegion = erosion_rectangle1(hRegion, (RegionRectParam)param);
                    break;
                case nameof(enRegionMorphology.opening_circle):
                    tarRegion = opening_circle(hRegion, (RegionCircleParam)param);
                    break;
                case nameof(enRegionMorphology.opening_rectangle1):
                    tarRegion = opening_rectangle1(hRegion, (RegionRectParam)param);
                    break;
                default:
                    tarRegion = hRegion;
                    break;
            }
            ///////////////
            if (param.IsConnection)
            {
                connRegion = tarRegion.Connection();
                tarRegion?.Dispose();
            }
            else
                connRegion = tarRegion;
            ///////////////
            if (param.IsFill)
            {
                fillRegion = connRegion.FillUp();
                connRegion?.Dispose();
            }
            else
                fillRegion = connRegion;
            ////////////////////////////////////
            return fillRegion;
        }
        public HRegion RegionMorphology(HRegion hRegion, BindingList<RegionOperateParam> param)
        {
            HRegion tempRegion = new HRegion();
            HRegion connRegion = new HRegion();
            HRegion fillRegion = new HRegion();
            HRegion tarRegion = new HRegion();
            //tarRegion.GenEmptyObj();
            if (hRegion.IsInitialized())
                tarRegion = hRegion.Clone();
            else
                return tarRegion;
            ////////////////////////////
            foreach (var item in param)
            {
                switch (item.Method.ToString())
                {
                    case nameof(enRegionMorphology.closing_circle):
                        if (!item.Active) continue;
                        tempRegion = closing_circle(tarRegion, (RegionCircleParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.closing_rectangle1):
                        if (!item.Active) continue;
                        tempRegion = closing_rectangle1(tarRegion, (RegionRectParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.dilation_circle):
                        if (!item.Active) continue;
                        tempRegion = dilation_circle(tarRegion, (RegionCircleParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.dilation_rectangle1):
                        if (!item.Active) continue;
                        tempRegion = dilation_rectangle1(tarRegion, (RegionRectParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.erosion_circle):
                        if (!item.Active) continue;
                        tempRegion = erosion_circle(tarRegion, (RegionCircleParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.erosion_rectangle1):
                        if (!item.Active) continue;
                        tempRegion = erosion_rectangle1(tarRegion, (RegionRectParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.opening_circle):
                        if (!item.Active) continue;
                        tempRegion = opening_circle(tarRegion, (RegionCircleParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.opening_rectangle1):
                        if (!item.Active) continue;
                        tempRegion = opening_rectangle1(tarRegion, (RegionRectParam)item.RegionParam);
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                    case nameof(enRegionMorphology.shape_trans):
                        if (!item.Active) continue;
                        tempRegion = shape_trans(tarRegion, (ShapeTransParam)item.RegionParam);
                        item.RegionParam.IsFill = false; // 形状转换后不再需要填充
                        ///////////////
                        if (item.RegionParam.IsFill && tempRegion.IsInitialized())
                        {
                            fillRegion = tempRegion.FillUp();
                            tempRegion?.Dispose();
                        }
                        else
                            fillRegion = tempRegion;
                        if (item.RegionParam.IsConnection && fillRegion.IsInitialized())
                        {
                            connRegion = fillRegion.Connection();
                            fillRegion?.Dispose();
                        }
                        else
                            connRegion = fillRegion;
                        tarRegion = connRegion.Clone();
                        connRegion?.Dispose();
                        break;
                }
            }
            ////////////////////////////////////
            return tarRegion;
        }

        public HRegion closing_circle(HRegion hRegion, RegionCircleParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.ClosingCircle(param.Radius);

            return tarRegion;
        }
        public HRegion closing_rectangle1(HRegion hRegion, RegionRectParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.ClosingRectangle1((int)param.Width, (int)param.Height);

            return tarRegion;
        }
        public HRegion dilation_circle(HRegion hRegion, RegionCircleParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.DilationCircle(param.Radius);

            return tarRegion;
        }
        public HRegion dilation_rectangle1(HRegion hRegion, RegionRectParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.DilationRectangle1((int)param.Width, (int)param.Height);

            return tarRegion;
        }
        public HRegion erosion_circle(HRegion hRegion, RegionCircleParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.ErosionCircle(param.Radius);

            return tarRegion;
        }
        public HRegion erosion_rectangle1(HRegion hRegion, RegionRectParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.ErosionRectangle1((int)param.Width, (int)param.Height);

            return tarRegion;
        }
        public HRegion opening_circle(HRegion hRegion, RegionCircleParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.OpeningCircle(param.Radius);

            return tarRegion;
        }
        public HRegion opening_rectangle1(HRegion hRegion, RegionRectParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.OpeningRectangle1((int)param.Width, (int)param.Height);

            return tarRegion;
        }
        public HRegion shape_trans(HRegion hRegion, ShapeTransParam param)
        {
            HRegion tarRegion = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            tarRegion = hRegion.ShapeTrans(param.ShapeType.ToString());

            return tarRegion;
        }

    }

    public enum enRegionMorphology
    {
        NONE,
        closing_circle,
        closing_rectangle1,
        dilation_circle,
        dilation_rectangle1,
        erosion_circle,
        erosion_rectangle1,
        opening_circle,
        opening_rectangle1,
        shape_trans,
    }





}
