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
    public class Ocr
    {
        public enOcrModel OcrModel { get; set; }
        public OcrResult OcrResult { get; set; }
        public OcrParam OcrParam { get; set; }

        public Ocr()
        {
            OcrModel = enOcrModel.OcrMLP; // 多层感知主要用于工业上的字符检测 
            OcrResult = new OcrResult();
            OcrParam = new OcrMlpParam();
        }
        public void DoOcr(HImage hImage)
        {
            HImage invertImage;
            if (this.OcrParam.InvertImage)
                invertImage = hImage.InvertImage();
            else
                invertImage = hImage;
            ///////
            switch (OcrModel)
            {
                case enOcrModel.OcrMLP:
                    OcrResult = DoOcrMlp(invertImage, (OcrMlpParam)OcrParam);
                    break;
                case enOcrModel.OcrCNN:
                    OcrResult = DoOcrCnn(invertImage, (OcrCnnParam)OcrParam);
                    break;
                case enOcrModel.OcrKNN:
                    OcrResult = DoOcrKnn(invertImage, (OcrKnnParam)OcrParam);
                    break;
                case enOcrModel.OcrSVM:
                    OcrResult = DoOcrSvm(invertImage, (OcrSvmParam)OcrParam);
                    break;
                case enOcrModel.OcrTextMode:
                    OcrResult = DoOcrTextModel(invertImage, (OcrTextModelParam)OcrParam);
                    break;
            }
        }



        private OcrResult DoOcrCnn(HImage hImage, OcrCnnParam ocrParam)
        {
            OcrResult ocrResult = new OcrResult();
            HTuple confidence, Class;
            string Word;
            double Score;
            HRegion hRegion = null, threRegion = null, operateRegion = null, selectRegion = null, connectionRegion = null, unionRegion = null,
                sortRegion = null;
            HImage reduceImage = null;
            HOCRCnn oCRCnn = new HOCRCnn(ocrParam.OcrFontName);
            try
            {
                switch (ocrParam.OcrMethod)
                {
                    default:
                    case enOcrMethod.MultipleCharacters:
                        hRegion = new HRegion();
                        hRegion.GenEmptyObj();
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            HRegion hRegion1 = new HRegion();
                            hRegion1.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            hRegion = hRegion.ConcatObj(hRegion1);
                            hRegion1.Dispose();
                        }
                        unionRegion = hRegion.Union1();
                        reduceImage = hImage.ReduceDomain(unionRegion);
                        threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                        switch (ocrParam.RegionOperate)
                        {
                            default:
                            case enRegionOperate.NONE:
                                operateRegion = threRegion;
                                break;
                            case enRegionOperate.closing_rectangle1:
                                operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                            case enRegionOperate.opening_rectangle1:
                                operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                        }
                        connectionRegion = operateRegion.Connection();
                        selectRegion = connectionRegion.SelectShape("area", "and", ocrParam.MinArea, ocrParam.MaxArea);
                        ///////////////////////////////////////////////
                        switch (ocrParam.SortRegion)
                        {
                            case enOrderMethod.row:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            case enOrderMethod.column:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            default:
                            case enOrderMethod.none:
                                sortRegion = selectRegion;
                                break;
                        }
                        /////////
                        Class = oCRCnn.DoOcrMultiClassCnn(sortRegion, hImage, out confidence);
                        foreach (var item in Class.SArr)
                        {
                            ocrResult.Character += item;
                        }   
                        ocrResult.Score.AddRange(confidence.DArr);
                        ocrResult.Result = true;
                        ////////////////////////
                        hRegion?.Dispose();
                        reduceImage?.Dispose();
                        threRegion?.Dispose();
                        selectRegion?.Dispose();
                        connectionRegion?.Dispose();
                        unionRegion?.Dispose();
                        sortRegion?.Dispose();
                        break;
                    case enOcrMethod.SingleCharacter:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            // 区域去噪
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            selectRegion = connectionRegion.SelectShapeStd("max_area", 70);
                            ///////////////////////////////////////
                            Class = oCRCnn.DoOcrSingleClassCnn(selectRegion, hImage, ocrParam.NumClasses, out confidence);
                            ocrResult.Character += Class.S;
                            ocrResult.Score.Add(Math.Round(confidence[0].D, 2));
                            ocrResult.Result = true;
                            /////////////////////////////////
                            hRegion?.Dispose();
                            reduceImage?.Dispose();
                            threRegion?.Dispose();
                            selectRegion?.Dispose();
                            connectionRegion?.Dispose();
                            sortRegion?.Dispose();
                        }
                        break;
                    case enOcrMethod.Word:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            selectRegion = connectionRegion.SelectShape("area'", "and", ocrParam.MinArea, ocrParam.MaxArea);
                            ///////////////////////////////////////////////  排序区域
                            switch (ocrParam.SortRegion)
                            {
                                case enOrderMethod.row:
                                    sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                    break;
                                case enOrderMethod.column:
                                    sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                    break;
                                default:
                                case enOrderMethod.none:
                                    sortRegion = selectRegion;
                                    break;
                            };
                            /////////////////////////////////////
                            Class = oCRCnn.DoOcrWordCnn(sortRegion, hImage, ocrParam.Expression, ocrParam.NumAlternatives, ocrParam.NumCorrections, out confidence, out Word, out Score);
                            foreach (var item1 in Class.SArr)
                            {
                                ocrResult.Character += item1;
                            }
                            ocrResult.Result = true;
                            ocrResult.Score.Add(Score);
                        }
                        break;
                }
            }
            catch (HalconException ex)
            {
                LoggerHelper.Error("DoOcrCnn ()执行报错" + ex);
            }
            finally
            {
                oCRCnn?.ClearOcrClassCnn();
            }
            return ocrResult;
        }
        private OcrResult DoOcrKnn(HImage hImage, OcrKnnParam ocrParam)
        {
            OcrResult ocrResult = new OcrResult();
            HTuple confidence, Class;
            string Word;
            double Score;
            HRegion hRegion = null, threRegion = null, operateRegion = null, selectRegion = null, connectionRegion = null, unionRegion = null,
                sortRegion = null;
            HImage reduceImage = null;
            HOCRKnn oCRKnn = new HOCRKnn(ocrParam.OcrFontName);
            try
            {
                switch (ocrParam.OcrMethod)
                {
                    default:
                    case enOcrMethod.MultipleCharacters:
                        hRegion = new HRegion();
                        hRegion.GenEmptyObj();
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            HRegion hRegion1 = new HRegion();
                            hRegion1.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            hRegion = hRegion.ConcatObj(hRegion1);
                            hRegion1.Dispose();
                        }
                        unionRegion = hRegion.Union1();
                        reduceImage = hImage.ReduceDomain(unionRegion);
                        threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                        switch (ocrParam.RegionOperate)
                        {
                            default:
                            case enRegionOperate.NONE:
                                operateRegion = threRegion;
                                break;
                            case enRegionOperate.closing_rectangle1:
                                operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                            case enRegionOperate.opening_rectangle1:
                                operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                        }
                        connectionRegion = operateRegion.Connection();
                        selectRegion = connectionRegion.SelectShape("area", "and", ocrParam.MinArea, ocrParam.MaxArea);
                        ///////////////////////////////////////////////
                        switch (ocrParam.SortRegion)
                        {
                            case enOrderMethod.row:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            case enOrderMethod.column:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            default:
                            case enOrderMethod.none:
                                sortRegion = selectRegion;
                                break;
                        }
                        /////////
                        Class = oCRKnn.DoOcrMultiClassKnn(sortRegion, hImage, out confidence);
                        foreach (var item in Class.SArr)
                        {
                            ocrResult.Character += item;
                        }
                        ocrResult.Score.AddRange(confidence.DArr);
                        ocrResult.Result = true;
                        ////////////////////////
                        hRegion?.Dispose();
                        reduceImage?.Dispose();
                        threRegion?.Dispose();
                        selectRegion?.Dispose();
                        connectionRegion?.Dispose();
                        unionRegion?.Dispose();
                        sortRegion?.Dispose();
                        break;
                    case enOcrMethod.SingleCharacter:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            // 区域去噪
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            selectRegion = connectionRegion.SelectShapeStd("max_area", 70);
                            ///////////////////////////////////////
                            Class = oCRKnn.DoOcrSingleClassKnn(selectRegion, hImage, ocrParam.NumClasses, ocrParam.NumNeighbors,out confidence);
                            ocrResult.Character += Class.S;
                            ocrResult.Score.Add(Math.Round(confidence[0].D, 2));
                            ocrResult.Result = true;
                            /////////////////////////////////
                            hRegion?.Dispose();
                            reduceImage?.Dispose();
                            threRegion?.Dispose();
                            selectRegion?.Dispose();
                            connectionRegion?.Dispose();
                            sortRegion?.Dispose();
                        }
                        break;
                    case enOcrMethod.Word:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            selectRegion = connectionRegion.SelectShape("area'", "and", ocrParam.MinArea, ocrParam.MaxArea);
                            ///////////////////////////////////////////////  排序区域
                            switch (ocrParam.SortRegion)
                            {
                                case enOrderMethod.row:
                                    sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                    break;
                                case enOrderMethod.column:
                                    sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                    break;
                                default:
                                case enOrderMethod.none:
                                    sortRegion = selectRegion;
                                    break;
                            };
                            /////////////////////////////////////
                            Class = oCRKnn.DoOcrWordKnn(sortRegion, hImage, ocrParam.Expression, ocrParam.NumAlternatives, ocrParam.NumCorrections, out confidence, out Word, out Score);
                            
                            foreach (var item1 in Class.SArr)
                            {
                                ocrResult.Character += item1;
                            }
                            ocrResult.Score.Add(Score);
                            ocrResult.Result = true;
                            ////////////////////////
                            hRegion?.Dispose();
                            reduceImage?.Dispose();
                            threRegion?.Dispose();
                            selectRegion?.Dispose();
                            connectionRegion?.Dispose();
                            unionRegion?.Dispose();
                            sortRegion?.Dispose();
                        }
                        break;
                }
            }
            catch (HalconException ex)
            {
                LoggerHelper.Error("DoOcrKnn ()执行报错" + ex);
            }
            finally
            {
                oCRKnn?.ClearOcrClassKnn();
            }
            return ocrResult;
        }
        private OcrResult DoOcrSvm(HImage hImage, OcrSvmParam ocrParam)
        {
            OcrResult ocrResult = new OcrResult();
            HTuple confidence, Class;
            string Word;
            double Score;
            HRegion hRegion = null, threRegion = null, operateRegion = null, selectRegion = null, connectionRegion = null, unionRegion = null,
                sortRegion = null;
            HImage reduceImage = null;
            HOCRSvm oCRSvm = new HOCRSvm(ocrParam.OcrFontName);
            try
            {
                switch (ocrParam.OcrMethod)
                {
                    default:
                    case enOcrMethod.MultipleCharacters:
                        hRegion = new HRegion();
                        hRegion.GenEmptyObj();
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            HRegion hRegion1 = new HRegion();
                            hRegion1.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            hRegion = hRegion.ConcatObj(hRegion1);
                            hRegion1.Dispose();
                        }
                        unionRegion = hRegion.Union1();
                        reduceImage = hImage.ReduceDomain(unionRegion);
                        threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                        switch (ocrParam.RegionOperate)
                        {
                            default:
                            case enRegionOperate.NONE:
                                operateRegion = threRegion;
                                break;
                            case enRegionOperate.closing_rectangle1:
                                operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                            case enRegionOperate.opening_rectangle1:
                                operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                        }
                        connectionRegion = operateRegion.Connection();
                        selectRegion = connectionRegion.SelectShape("area", "and", ocrParam.MinArea, ocrParam.MaxArea);
                        ///////////////////////////////////////////////
                        switch (ocrParam.SortRegion)
                        {
                            case enOrderMethod.row:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            case enOrderMethod.column:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            default:
                            case enOrderMethod.none:
                                sortRegion = selectRegion;
                                break;
                        }
                        /////////
                        Class = oCRSvm.DoOcrMultiClassSvm(sortRegion, hImage);
                        foreach (var item1 in Class.SArr)
                        {
                            ocrResult.Character += item1;
                        }                     
                        ocrResult.Score.Add(-1);
                        ocrResult.Result = true;
                        ////////////////////////
                        hRegion?.Dispose();
                        reduceImage?.Dispose();
                        threRegion?.Dispose();
                        selectRegion?.Dispose();
                        connectionRegion?.Dispose();
                        unionRegion?.Dispose();
                        sortRegion?.Dispose();
                        break;
                    case enOcrMethod.SingleCharacter:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            // 区域去噪
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            selectRegion = connectionRegion.SelectShapeStd("max_area", 70);
                            ///////////////////////////////////////
                            Class = oCRSvm.DoOcrSingleClassSvm(selectRegion, hImage, ocrParam.NumClasses);
                            ocrResult.Character += Class.S;
                            ocrResult.Score.Add(-1);
                            ocrResult.Result = true;
                            /////////////////////////////////
                            hRegion?.Dispose();
                            reduceImage?.Dispose();
                            threRegion?.Dispose();
                            selectRegion?.Dispose();
                            connectionRegion?.Dispose();
                            sortRegion?.Dispose();
                        }
                        break;
                    case enOcrMethod.Word:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            selectRegion = connectionRegion.SelectShape("area'", "and", ocrParam.MinArea, ocrParam.MaxArea);
                            ///////////////////////////////////////////////  排序区域
                            switch (ocrParam.SortRegion)
                            {
                                case enOrderMethod.row:
                                    sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                    break;
                                case enOrderMethod.column:
                                    sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                    break;
                                default:
                                case enOrderMethod.none:
                                    sortRegion = selectRegion;
                                    break;
                            };
                            /////////////////////////////////////
                            Class = oCRSvm.DoOcrWordSvm(sortRegion, hImage, ocrParam.Expression, ocrParam.NumAlternatives, ocrParam.NumCorrections,  out Word, out Score);
                            foreach (var item1 in Class.SArr)
                            {
                                ocrResult.Character += item1;
                            }
                            ocrResult.Score.Add(Score);
                            ocrResult.Result = true;
                            /////////////////////////////////
                            hRegion?.Dispose();
                            reduceImage?.Dispose();
                            threRegion?.Dispose();
                            selectRegion?.Dispose();
                            connectionRegion?.Dispose();
                            sortRegion?.Dispose();
                        }
                        break;
                }
            }
            catch (HalconException ex)
            {
                LoggerHelper.Error("DoOcrSvm ()执行报错" + ex);
            }
            finally
            {
                oCRSvm?.ClearOcrClassSvm();
            }
            return ocrResult;
        }
        private OcrResult DoOcrMlp(HImage hImage, OcrMlpParam ocrParam)
        {
            OcrResult ocrResult = new OcrResult();
            HTuple confidence, Class;
            string Word;
            double Score;
            HRegion hRegion = null, threRegion = null, operateRegion = null, selectRegion = null, connectionRegion = null, unionRegion = null,
                sortRegion = null;
            HImage reduceImage = null;
            HOCRMlp oCRMlp = new HOCRMlp(ocrParam.OcrFontName);
            try
            {
                switch (ocrParam.OcrMethod)
                {
                    default:
                    case enOcrMethod.MultipleCharacters:
                        hRegion = new HRegion();
                        hRegion.GenEmptyObj();
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            HRegion hRegion1 = new HRegion();
                            hRegion1.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            hRegion = hRegion.ConcatObj(hRegion1);
                            hRegion1.Dispose();
                        }
                        unionRegion = hRegion.Union1();
                        reduceImage = hImage.ReduceDomain(unionRegion);
                        threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                        switch (ocrParam.RegionOperate)
                        {
                            default:
                            case enRegionOperate.NONE:
                                operateRegion = threRegion;
                                break;
                            case enRegionOperate.closing_rectangle1:
                                operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                            case enRegionOperate.opening_rectangle1:
                                operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                        }
                        connectionRegion = operateRegion.Connection();
                        selectRegion = connectionRegion.SelectShape("area", "and", ocrParam.MinArea, ocrParam.MaxArea);
                        ///////////////////////////////////////////////
                        switch (ocrParam.SortRegion)
                        {
                            case enOrderMethod.row:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            case enOrderMethod.column:
                                sortRegion = selectRegion.SortRegion("first_point", "true", ocrParam.SortRegion.ToString());
                                break;
                            default:
                            case enOrderMethod.none:
                                sortRegion = selectRegion;
                                break;
                        }
                        /////////
                        Class = oCRMlp.DoOcrMultiClassMlp(sortRegion, hImage,out confidence);
                        foreach (var item in Class.SArr)
                        {
                            ocrResult.Character += item;
                        }
                        ocrResult.Result = true;
                        ocrResult.Score.AddRange(confidence.DArr);
                        ////////////////////////
                        hRegion?.Dispose();
                        reduceImage?.Dispose();
                        threRegion?.Dispose();
                        selectRegion?.Dispose();
                        connectionRegion?.Dispose();
                        unionRegion?.Dispose();
                        sortRegion?.Dispose();
                        break;
                    case enOcrMethod.SingleCharacter:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            // 区域去噪
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            selectRegion = connectionRegion.SelectShapeStd("max_area", 70);
                            ///////////////////////////////////////
                            Class = oCRMlp.DoOcrSingleClassMlp(selectRegion, hImage, ocrParam.NumClasses,out confidence);
                            ocrResult.Character += Class.S;
                            ocrResult.Score.Add(Math.Round(confidence[0].D, 2));
                            ocrResult.Result = true;
                            /////////////////////////////////
                            hRegion?.Dispose();
                            reduceImage?.Dispose();
                            threRegion?.Dispose();
                            selectRegion?.Dispose();
                            connectionRegion?.Dispose();
                            sortRegion?.Dispose();
                        }
                        break;
                    case enOcrMethod.Word:
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            hRegion = new HRegion();
                            hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            reduceImage = hImage.ReduceDomain(hRegion);
                            threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                            switch (ocrParam.RegionOperate)
                            {
                                default:
                                case enRegionOperate.NONE:
                                    operateRegion = threRegion;
                                    break;
                                case enRegionOperate.closing_rectangle1:
                                    operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                                case enRegionOperate.opening_rectangle1:
                                    operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                    break;
                            }
                            connectionRegion = operateRegion.Connection();
                            ///////////////////////////////////////////////  排序区域
                            switch (ocrParam.SortRegion)
                            {
                                case enOrderMethod.row:
                                    sortRegion = connectionRegion.SortRegion("first_point", "true", nameof(ocrParam.SortRegion));
                                    break;
                                case enOrderMethod.column:
                                    sortRegion = connectionRegion.SortRegion("first_point", "true", nameof(ocrParam.SortRegion));
                                    break;
                                default:
                                case enOrderMethod.none:
                                    sortRegion = connectionRegion;
                                    break;
                            };
                            /////////////////////////////////////
                            Class = oCRMlp.DoOcrWordMlp(sortRegion, hImage, ocrParam.Expression, ocrParam.NumAlternatives, ocrParam.NumCorrections,out confidence, out Word, out Score);
                            foreach (var item1 in Class.SArr)
                            {
                                ocrResult.Character += item1;
                            }
                            ocrResult.Result = true;
                            ocrResult.Score.Add(Score);
                            /////////////////////////////////
                            hRegion?.Dispose();
                            reduceImage?.Dispose();
                            threRegion?.Dispose();
                            selectRegion?.Dispose();
                            connectionRegion?.Dispose();
                            sortRegion?.Dispose();
                        }
                        break;
                }
            }
            catch (HalconException ex)
            {

            }
            finally
            {
                oCRMlp?.ClearOcrClassMlp();
            }
            return ocrResult;
        }
        private OcrResult DoOcrTextModel(HImage hImage, OcrTextModelParam ocrParam)
        {
            OcrResult ocrResult = new OcrResult();
            HRegion hRegion;
            HImage reduceImage;
            HTextModel oCRTextModel = new HTextModel("auto", ocrParam.OcrFontName);
            HTextResult hTextResult = null;
            try
            {
                foreach (var item in ocrParam.ChartRegion)
                {
                    hRegion = new HRegion();
                    hRegion.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                    reduceImage = hImage.ReduceDomain(hRegion);
                    ////////////////////////////////////////
                    oCRTextModel.SetTextModelParam("min_contrast", ocrParam.MinContrast);
                    oCRTextModel.SetTextModelParam("polarity", nameof(ocrParam.Polarity));
                    oCRTextModel.SetTextModelParam("min_char_height", ocrParam.MinCharHeight);
                    oCRTextModel.SetTextModelParam("max_char_height", ocrParam.MaxCharHeight);
                    oCRTextModel.SetTextModelParam("min_char_width", ocrParam.MinCharWidth);
                    oCRTextModel.SetTextModelParam("max_char_width", ocrParam.MaxCharWidth);
                    hTextResult = oCRTextModel.FindText(reduceImage);
                    HTuple hTuple = hTextResult.GetTextResult("class");
                    for (int i = 0; i < hTuple.SArr.Length; i++)
                    {
                        ocrResult.Character += hTuple[i].S;
                        ocrResult.Score.Add(-1);
                        ocrResult.Result = true;
                    }
                    hRegion?.Dispose();
                    reduceImage?.Dispose();
                    hTextResult?.ClearTextResult();
                }
            }
            catch (HalconException ex)
            {

            }
            finally
            {
                oCRTextModel?.ClearTextModel();
                hTextResult?.ClearTextResult();
            }
            return ocrResult;
        }



    }
}
