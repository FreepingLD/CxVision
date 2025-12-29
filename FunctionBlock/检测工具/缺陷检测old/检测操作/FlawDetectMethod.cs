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
    public class FlawDetectMethod
    {
        private HImage _thresholdImage;
        public FlawDetectMethod()
        {

        }

        public HImage ThresholdImage { get => _thresholdImage; set => _thresholdImage = value; }

        public HRegion FlawDetect(HImage image, ThresholParam detectParam, BindingList<DetectROIParam> roiParams)
        {
            HRegion hRegionBlob = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image 对象为空或没有初始化");
            }
            if (detectParam == null)
            {
                throw new ArgumentNullException("detectParam 对象为空或没有初始化");
            }
            if (roiParams == null)
            {
                throw new ArgumentNullException("roiParams 对象为空或没有初始化");
            }
            HRegion hRegion = null;
            HImage removeImage = null;
            HRegion reserveRegion = new HRegion(); // 保留区域
            HRegion removeRegion = new HRegion(); // 移除区域
            reserveRegion.GenEmptyRegion();
            removeRegion.GenEmptyRegion();
            /////////////////////////////////////////////////
            foreach (var item in roiParams)
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
                                hRegion = (item.RoiShape).GetRegion();
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
                                hRegion = (item.RoiShape).GetRegion();
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
                _thresholdImage = removeImage.ReduceDomain(reserveRegion.Union1());
            else
                _thresholdImage = removeImage.Clone();
            removeImage?.Dispose();
            //////////////////////////////////////////////////////////
            switch (detectParam.Method)
            {
                default:
                case nameof(enFlawDetectMethod.Threshold):
                    hRegionBlob = threshold(_thresholdImage, (ThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.AutoThreshold):
                    hRegionBlob = auto_threshold(_thresholdImage, (AutoThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.BinaryThreshold):
                    hRegionBlob = binary_threshold(_thresholdImage, (BinaryThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.CharThreshold):
                    hRegionBlob = char_threshold(_thresholdImage, (CharThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.DualThreshold):
                    hRegionBlob = dual_threshold(_thresholdImage, (DualThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.DynThreshold):
                    hRegionBlob = dyn_threshold(_thresholdImage, (DynThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.FastThreshold):
                    hRegionBlob = fast_threshold(_thresholdImage, (FastThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.HysteresisThreshold):
                    hRegionBlob = hysteresis_threshold(_thresholdImage, (HysteresisThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.LocalThreshold):
                    hRegionBlob = local_threshold(_thresholdImage, (LocalThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.VarThreshold):
                    hRegionBlob = var_threshold(_thresholdImage, (VarThresholdParam)detectParam);
                    break;
                case nameof(enFlawDetectMethod.WatershedsThreshold):
                    hRegionBlob = watersheds_threshold(_thresholdImage, (WatershedsThresholdParam)detectParam);
                    break;  
                case nameof(enFlawDetectMethod.CallipersInsp):
                    hRegionBlob = Callipers_Insp(_thresholdImage, (CallipersInspParam)detectParam);
                    break;  
            }
            return hRegionBlob;
        }

        public HRegion FlawSelect(HImage image, HRegion hRegion, ThresholParam detectParam)
        {
            HRegion selectRegion = new HRegion();



            return selectRegion;
        }

        private HRegion threshold(HImage image, ThresholdParam param)
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
                    if (param.MinThreshold < 1) param.MinThreshold = 1;
                    HRegion hRegionLow = image.Threshold(1, param.MinThreshold);
                    HRegion hRegionHight = image.Threshold(param.MaxThreshold, 255);
                    hRegion = hRegionLow.Union2(hRegionHight);
                    break;
            }
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion auto_threshold(HImage image, AutoThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.AutoThreshold(param.AutoSigma);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion binary_threshold(HImage image, BinaryThresholdParam param)
        {
            int UsedThreshold;
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.BinaryThreshold(param.BinaryMethod, param.BinaryLightDark, out UsedThreshold);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion char_threshold(HImage image, CharThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            int UsedThreshold;
            if (image == null) return hRegion;
            hRegion = image.CharThreshold(image, param.ChartSigma, param.ChartPercent, out UsedThreshold);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion dual_threshold(HImage image, DualThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.DualThreshold(param.DualMinSize, param.DualMinGray, param.DuaThreshold);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion dyn_threshold(HImage image, DynThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            HImage MeanImage;
            if (image == null) return hRegion;
            MeanImage = image.MeanImage(param.DynMaskWidth, param.DynMaskHeight);
            hRegion = image.DynThreshold(MeanImage, param.DynOffset, param.DynLightDark);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion fast_threshold(HImage image, FastThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.FastThreshold(param.FastMinGray, param.FastMaxGray, param.FastMinSize);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion hysteresis_threshold(HImage image, HysteresisThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.HysteresisThreshold(param.HysteresisLow, param.HysteresisHight, param.HysteresisMaxLength);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion local_threshold(HImage image, LocalThresholdParam param)
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
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion var_threshold(HImage image, VarThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.VarThreshold(param.VarMaskWidth, param.VarMaskHeight, param.VarStdDevScale, param.VarAbsThreshold, param.VarLightDark);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion watersheds_threshold(HImage image, WatershedsThresholdParam param)
        {
            HRegion hRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            if (image == null) return hRegion;
            hRegion = image.WatershedsThreshold(param.Threshold);
            //if (param.IsFill)
            //{
            //    FillRegion = hRegion.FillUp();
            //    hRegion?.Dispose();
            //}
            //else
            //    FillRegion = hRegion;
            //if (param.IsConnection)
            //{
            //    ConnRegion = FillRegion.Connection();
            //    FillRegion?.Dispose();
            //}
            //else
            //    ConnRegion = FillRegion;
            return hRegion;
        }
        private HRegion Callipers_Insp(HImage image, CallipersInspParam param)
        {
            HRegion NgRegion = null;
            HRegion FillRegion = null;
            HRegion ConnRegion = null;
            List<double> list_row1 = new List<double>();
            List<double> list_col1 = new List<double>();
            List<double> list_row2 = new List<double>();
            List<double> list_col2 = new List<double>();
            List<double> list_row3 = new List<double>();
            List<double> list_col3 = new List<double>();
            List<double> list_row4 = new List<double>();
            List<double> list_col4 = new List<double>();
            NgRegion = new HRegion();
            NgRegion.GenEmptyRegion();
            int width, height, row1, col1, row2, col2;
            image.GetImageSize(out width, out height);
            HRegion hRegionDomain = image.GetDomain();
            hRegionDomain.SmallestRectangle1(out row1, out col1, out row2, out col2);
            HMeasure hMeasure1, hMeasure2, hMeasure3, hMeasure4;
            HTuple rowEdge, columnEdge, rowEdgeFirst, columnEdgeFirst, amplitudeFirst, rowEdgeSecond, columnEdgeSecond, amplitudeSecond, intraDistance, interDistance;

            #region  检测折痕
            // 在列方向上扫描，检测产品是否有折皱
            Task task1 = Task.Run(() =>
            {
                for (int i = (int)(param.SampleDist * 0.5) + col1; i < col2; i += (int)param.SampleDist)
                {
                    hMeasure1 = new HMeasure((row2 + row1) * 0.5, i, Math.PI * 0.5, (row2 - row1) * 0.5, (int)param.SampleDist * 0.5, width, height, "nearest_neighbor");
                    //hMeasure.MeasurePairs(image, myRectInspPara.Sigma, myRectInspPara.Threshold, "all", "all", out rowEdgeFirst, out columnEdgeFirst, out amplitudeFirst, out rowEdgeSecond, out columnEdgeSecond, out amplitudeSecond, out intraDistance, out interDistance);
                    hMeasure1.MeasurePos(image, param.Sigma, param.Threshold, "all", "all", out rowEdge, out columnEdge, out amplitudeFirst, out intraDistance);
                    if (rowEdge != null && rowEdge.Length > 0)
                    {
                        for (int k = 0; k < rowEdge.Length; k++)
                        {
                            list_row1.Add(rowEdge[k].D);
                            list_col1.Add(columnEdge[k].D);
                        }
                    }
                    hMeasure1.CloseMeasure();
                }
            });
            #endregion
            // 在列方向上扫描，检测产品是否有孔洞 , 找孔洞，逐列去找
            #region  检测孔洞
            Task task2 = Task.Run(() =>
            {
                for (int i = (int)col1; i < col2; i++)
                {
                    hMeasure2 = new HMeasure((row2 + row1) * 0.5, i, Math.PI * 0.5, (row2 - row1) * 0.5, 1, width, height, "nearest_neighbor");
                    //hMeasure.MeasurePairs(image, myRectInspPara.Sigma, myRectInspPara.Threshold, "all", "all", out rowEdgeFirst, out columnEdgeFirst, out amplitudeFirst, out rowEdgeSecond, out columnEdgeSecond, out amplitudeSecond, out intraDistance, out interDistance);
                    hMeasure2.MeasurePos(image, param.Sigma * 2, param.Threshold * 2, "all", "all", out rowEdge, out columnEdge, out amplitudeFirst, out intraDistance);
                    if (rowEdge != null && rowEdge.Length > 0)
                    {
                        for (int k = 0; k < rowEdge.Length; k++)
                        {
                            list_row2.Add(rowEdge[k].D);
                            list_col2.Add(columnEdge[k].D);
                        }
                    }
                    hMeasure2.CloseMeasure();
                }
            });
            #endregion

            #region  检测白色条带
            Task task3 = Task.Run(() =>
            {
                for (int i = (int)(param.SampleDist * 0.5) + col1; i < col2; i += (int)param.SampleDist)
                {
                    hMeasure3 = new HMeasure((row2 + row1) * 0.5, i, Math.PI * 0.5, (row2 - row1) * 0.5, (int)param.SampleDist * 0.5, width, height, "nearest_neighbor");
                    HTuple grayValue = hMeasure3.MeasureProjection(image);
                    double meanValue = grayValue.TupleMean().D;
                    for (int k = 0; k < grayValue.Length; k++) // 帅选出比平均值大的点
                    {
                        if (Math.Abs(grayValue[k].D - meanValue) > param.Threshold * 2)
                        {
                            list_row3.Add(row2 - k + 1);
                            list_col3.Add(i);
                        }
                    }
                    hMeasure3.CloseMeasure();
                }
            });
            #endregion

            #region 行方向扫描  检测比较暗的漏铜
            Task task4 = Task.Run(() =>
            {
                for (int i = (int)(param.SampleDist * 0.5) + row1; i < row2; i += (int)param.SampleDist)
                {
                    hMeasure4 = new HMeasure(i, (col1 + col2) * 0.5, 0, (col2 - col1) * 0.5, param.SampleDist * 0.5, width, height, "nearest_neighbor");
                    //hMeasure.MeasurePairs(image, param.Sigma, param.Threshold, "all", "all", out rowEdgeFirst, out columnEdgeFirst, out amplitudeFirst, out rowEdgeSecond, out columnEdgeSecond, out amplitudeSecond, out intraDistance, out interDistance);
                    hMeasure4.MeasurePos(image, param.Sigma, param.Threshold * 2, "all", "all", out rowEdge, out columnEdge, out amplitudeFirst, out intraDistance);
                    if (rowEdge != null && rowEdge.Length > 0)
                    {
                        for (int k = 0; k < rowEdge.Length; k++)
                        {
                            list_row4.Add(rowEdge[k].D);
                            list_col4.Add(columnEdge[k].D);
                        }
                    }
                    hMeasure4.CloseMeasure();
                }
            });
            #endregion
            /////////////////////
            Task.WaitAll(task1, task2, task3, task4);
            if (list_row1.Count > 0)
            {
                HRegion hRegion1 = new HRegion();
                hRegion1.GenRegionPoints(list_row1.ToArray(), list_col1.ToArray());
                HRegion holeRegion = hRegion1.ClosingRectangle1((int)param.MaskWidth, (int)param.MaskHeight);
                NgRegion = NgRegion.Union2(holeRegion.Union1());
                ///////
                hRegion1?.Dispose();
                holeRegion?.Dispose();
            }
            if (list_row2.Count > 0)
            {
                HRegion hRegion1 = new HRegion();
                hRegion1.GenRegionPoints(list_row2.ToArray(), list_col2.ToArray());
                HRegion holeRegion = hRegion1.ClosingRectangle1((int)param.MaskWidth, (int)param.MaskHeight);
                NgRegion = NgRegion.Union2(holeRegion.Union1());
                ///////
                hRegion1?.Dispose();
                holeRegion?.Dispose();
            }
            if (list_row3.Count > 0)
            {
                HRegion hRegion1 = new HRegion();
                hRegion1.GenRegionPoints(list_row3.ToArray(), list_col3.ToArray());
                HRegion holeRegion = hRegion1.ClosingRectangle1((int)param.MaskWidth, (int)param.MaskHeight);
                NgRegion = NgRegion.Union2(holeRegion.Union1());
                ///////
                hRegion1?.Dispose();
                holeRegion?.Dispose();
            }
            if (list_row4.Count > 0)
            {
                HRegion hRegion1 = new HRegion();
                hRegion1.GenRegionPoints(list_row4.ToArray(), list_col4.ToArray());
                HRegion holeRegion = hRegion1.ClosingRectangle1((int)param.MaskWidth, (int)param.MaskHeight);
                NgRegion = NgRegion.Union2(holeRegion.Union1());
                ///////
                hRegion1?.Dispose();
                holeRegion?.Dispose();
            }

            return NgRegion;
        }


    }


    public enum enFlawDetectMethod
    {
        Threshold,
        AutoThreshold,
        BinaryThreshold,
        CharThreshold,
        DualThreshold,
        DynThreshold,
        FastThreshold,
        HysteresisThreshold,
        LocalThreshold,
        VarThreshold,
        WatershedsThreshold,
        CallipersInsp,
        ScriptInsp,
    }
}
