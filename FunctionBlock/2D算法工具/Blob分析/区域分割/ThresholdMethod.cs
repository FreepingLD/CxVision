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
    [Serializable]
    public class ThresholdMethod
    {
        private static object lockState = new object();
        private static ThresholdMethod _Instance = null;
        private HImage ThresholdImage;
        private ThresholdMethod()
        {

        }

        public static ThresholdMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new ThresholdMethod();
                    }
                }
                return _Instance;
            }
        }

        public HRegion SegmentRegion(HImage image, SegmentParam segmentParam)
        {
            HRegion hRegionBlob = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (segmentParam == null)
            {
                throw new ArgumentNullException("segmentParam 对象为空或没有初始化");
            }
            HRegion hRegion = null;
            HImage removeImage = null;
            HRegion reserveRegion = new HRegion(); // 保留区域
            HRegion removeRegion = new HRegion(); // 移除区域
            reserveRegion.GenEmptyRegion();
            removeRegion.GenEmptyRegion();
            foreach (var item in segmentParam.RegionParam)
            {
                switch (item.InsideOrOutside)
                {
                    default:
                    case enInsideOrOutside.保留:
                        switch (item.ShapeType)
                        {
                            case enShapeType.椭圆:
                            case enShapeType.圆:
                            case enShapeType.矩形1:
                            case enShapeType.矩形2:
                            case enShapeType.多边形:
                            case enShapeType.点:
                            case enShapeType.线:
                                hRegion = (item.RoiShape).AffinePixROI(new HHomMat2D(segmentParam.PixCoordSys?.GetVariationHomMat2D())).GetRegion();
                                reserveRegion = reserveRegion.ConcatObj(hRegion);
                                break;
                            default:
                                continue;
                        }
                        break;
                    case enInsideOrOutside.移除:
                        switch (item.ShapeType)
                        {
                            case enShapeType.椭圆:
                            case enShapeType.圆:
                            case enShapeType.矩形1:
                            case enShapeType.矩形2:
                            case enShapeType.多边形:
                            case enShapeType.点:
                            case enShapeType.线:
                                hRegion = (item.RoiShape).AffinePixROI(new HHomMat2D(segmentParam.PixCoordSys?.GetVariationHomMat2D())).GetRegion();
                                removeRegion = removeRegion.ConcatObj(hRegion);
                                break;
                            default:
                                continue;
                        }
                        break;
                }
            }
            removeImage = image.ReduceDomain(removeRegion.Union1().Complement());
            if (reserveRegion.CountObj() > 1)
                ThresholdImage = removeImage.ReduceDomain(reserveRegion.Union1());
            else
                ThresholdImage = removeImage.Clone();
            removeImage?.Dispose();
            //////////////////////////////////////////////////////////
            switch (segmentParam.SegmentMode)
            {
                case "单个分割":
                    HRegion empyRegion = new HRegion();
                    empyRegion.GenEmptyRegion();
                    int count = reserveRegion.CountObj();
                    for (int i = 1; i <= count; i++)
                    {
                        HRegion selectRegion = reserveRegion.SelectObj(i);
                        HImage hImage = ThresholdImage.ReduceDomain(selectRegion);
                        switch (segmentParam.SegmentMethod)
                        {
                            default:
                            case nameof(enRegionSegmentMethod.Threshold):
                                hRegionBlob = threshold(hImage, (ThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.AutoThreshold):
                                hRegionBlob = auto_threshold(hImage, (AutoThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.BinaryThreshold):
                                hRegionBlob = binary_threshold(hImage, (BinaryThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.CharThreshold):
                                hRegionBlob = char_threshold(hImage, (CharThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.DualThreshold):
                                hRegionBlob = dual_threshold(hImage, (DualThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.DynThreshold):
                                hRegionBlob = dyn_threshold(hImage, (DynThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.FastThreshold):
                                hRegionBlob = fast_threshold(hImage, (FastThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.HysteresisThreshold):
                                hRegionBlob = hysteresis_threshold(hImage, (HysteresisThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.LocalThreshold):
                                hRegionBlob = local_threshold(hImage, (LocalThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.VarThreshold):
                                hRegionBlob = var_threshold(hImage, (VarThresholdBlob)segmentParam);
                                break;
                            case nameof(enRegionSegmentMethod.WatershedsThreshold):
                                hRegionBlob = watersheds_threshold(hImage, (WatershedsThresholdBlob)segmentParam);
                                break;
                        }
                        selectRegion?.Dispose();
                        hImage?.Dispose();
                        empyRegion = empyRegion.ConcatObj(hRegionBlob);
                    }
                    hRegionBlob = empyRegion.SelectShape("area", "and", 1, double.MaxValue);
                    empyRegion?.Dispose();
                    break;
                default:
                case "合并分割":
                    switch (segmentParam.SegmentMethod)
                    {
                        default:
                        case nameof(enRegionSegmentMethod.Threshold):
                            hRegionBlob = threshold(ThresholdImage, (ThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.AutoThreshold):
                            hRegionBlob = auto_threshold(ThresholdImage, (AutoThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.BinaryThreshold):
                            hRegionBlob = binary_threshold(ThresholdImage, (BinaryThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.CharThreshold):
                            hRegionBlob = char_threshold(ThresholdImage, (CharThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.DualThreshold):
                            hRegionBlob = dual_threshold(ThresholdImage, (DualThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.DynThreshold):
                            hRegionBlob = dyn_threshold(ThresholdImage, (DynThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.FastThreshold):
                            hRegionBlob = fast_threshold(ThresholdImage, (FastThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.HysteresisThreshold):
                            hRegionBlob = hysteresis_threshold(ThresholdImage, (HysteresisThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.LocalThreshold):
                            hRegionBlob = local_threshold(ThresholdImage, (LocalThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.VarThreshold):
                            hRegionBlob = var_threshold(ThresholdImage, (VarThresholdBlob)segmentParam);
                            break;
                        case nameof(enRegionSegmentMethod.WatershedsThreshold):
                            hRegionBlob = watersheds_threshold(ThresholdImage, (WatershedsThresholdBlob)segmentParam);
                            break;
                    }
                    break;
            }
            return hRegionBlob;
        }

        public HRegion threshold(HImage image, ThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            switch (param.Operate)
            {
                default:
                case "and":
                    hRegion = image.Threshold(param.MinThreshold, param.MaxThreshold);
                    break;
                case "or":
                    HRegion hRegionLow = image.Threshold(0, param.MinThreshold);
                    HRegion hRegionHight = image.Threshold(param.MaxThreshold, 255);
                    hRegion = hRegionLow.Union2(hRegionHight);
                    break;
            }
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion auto_threshold(HImage image, AutoThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.AutoThreshold(param.AutoSigma);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion binary_threshold(HImage image, BinaryThresholdBlob param)
        {
            int UsedThreshold;
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.BinaryThreshold(param.BinaryMethod, param.BinaryLightDark, out UsedThreshold);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion char_threshold(HImage image, CharThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            int UsedThreshold;
            if (image == null) return hRegion;
            hRegion = image.CharThreshold(image, param.ChartSigma, param.ChartPercent, out UsedThreshold);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion dual_threshold(HImage image, DualThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.DualThreshold(param.DualMinSize, param.DualMinGray, param.DuaThreshold);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion dyn_threshold(HImage image, DynThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            HImage MeanImage;
            if (image == null) return hRegion;
            MeanImage = image.MeanImage(param.DynMaskWidth, param.DynMaskHeight);
            hRegion = image.DynThreshold(MeanImage, param.DynOffset, param.DynLightDark);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion fast_threshold(HImage image, FastThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.FastThreshold(param.FastMinGray, param.FastMaxGray, param.FastMinSize);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion hysteresis_threshold(HImage image, HysteresisThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.HysteresisThreshold(param.HysteresisLow, param.HysteresisHight, param.HysteresisMaxLength);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion local_threshold(HImage image, LocalThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            string[] ParamName = param.GenParamName.Split(',', ';', ':');
            string[] ParamVlaue = param.GenParamName.Split(',', ';', ':');
            if (ParamName.Length != ParamVlaue.Length)
                throw new ArgumentException("ParamName 与 ParamVlaue 长度不相等");
            hRegion = image.LocalThreshold(param.Method, param.LocalLightDark, new HTuple(ParamName), new HTuple(ParamVlaue));
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion var_threshold(HImage image, VarThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.VarThreshold(param.VarMaskWidth, param.VarMaskHeight, param.VarStdDevScale, param.VarAbsThreshold, param.VarLightDark);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }
        public HRegion watersheds_threshold(HImage image, WatershedsThresholdBlob param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.WatershedsThreshold(param.Threshold);
            if (param.IsFill)
            {
                FillRegion = hRegion.FillUp();
                hRegion?.Dispose();
            }
            else
                FillRegion = hRegion;
            if (param.IsConnection)
            {
                ConnRegion = FillRegion.Connection();
                FillRegion?.Dispose();
            }
            else
                ConnRegion = FillRegion;
            return ConnRegion;
        }

    }





}
