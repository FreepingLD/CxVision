using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;


namespace FunctionBlock
{

    [Serializable]
    public class FlawGroupMethod
    {
        public static bool Detect(HImage image, MeasureDetectParam param, out HRegion hRegion)
        {
            bool result = false;
            hRegion = new HRegion();
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image");
            }
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }
            /////////////////////////////
            List<double> list_row = new List<double>();
            List<double> list_col = new List<double>();
            int width, height,row1,col1,row2,col2;
            image.GetImageSize(out width, out height);
            image.GetDomain().SmallestRectangle1(out row1, out col1, out row2, out col2); // 获取图像域的范围，只在图像域内执行，因为测量是忽略图像域的
            //HSystem.SetSystem("int_zooming", "true");
            /////////////////////////////////////////////////////////////////////
            HMeasure hMeasure;
            HTuple rowEdgeFirst, columnEdgeFirst, amplitudeFirst, rowEdgeSecond, columnEdgeSecond, amplitudeSecond, intraDistance, interDistance;
            switch (param.MeasureMethod)
            {
                case enMeasureMethod.仅水平:
                    for (int i = (int)(param.SampleDist * 0.5) + row1; i < row2; i += param.SampleDist * param.SampleScale)
                    {
                        hMeasure = new HMeasure(i, (col1 + col2) * 0.5, 0, (col2 - col1) * 0.5, param.SampleDist * 0.5, width, height, "nearest_neighbor");
                        hMeasure.MeasurePairs(image, param.Sigma, param.Threshold, "all", "all", out rowEdgeFirst, out columnEdgeFirst, out amplitudeFirst, out rowEdgeSecond, out columnEdgeSecond, out amplitudeSecond, out intraDistance, out interDistance);
                        if (rowEdgeFirst != null && rowEdgeFirst.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeFirst.Length; k++)
                            {
                                list_row.Add(rowEdgeFirst[k].D);
                                list_col.Add(columnEdgeFirst[k].D);
                            }
                        }
                        if (rowEdgeSecond != null && rowEdgeSecond.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeSecond.Length; k++)
                            {
                                list_row.Add(rowEdgeSecond[k].D);
                                list_col.Add(columnEdgeSecond[k].D);
                            }
                        }
                        hMeasure.CloseMeasure();
                    }
                    break;
                case enMeasureMethod.仅垂直:
                    for (int i = (int)(param.SampleDist * 0.5) + col1; i < col2; i += param.SampleDist * param.SampleScale)
                    {
                        hMeasure = new HMeasure((row2 + row1) * 0.5, i, Math.PI * 0.5, (row2 - row1) * 0.5, param.SampleDist * 0.5, width, height, "nearest_neighbor");
                        hMeasure.MeasurePairs(image, param.Sigma, param.Threshold, "all", "all", out rowEdgeFirst, out columnEdgeFirst, out amplitudeFirst, out rowEdgeSecond, out columnEdgeSecond, out amplitudeSecond, out intraDistance, out interDistance);
                        if (rowEdgeFirst != null && rowEdgeFirst.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeFirst.Length; k++)
                            {
                                list_row.Add(rowEdgeFirst[k].D);
                                list_col.Add(columnEdgeFirst[k].D);
                            }
                        }
                        if (rowEdgeSecond != null && rowEdgeSecond.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeSecond.Length; k++)
                            {
                                list_row.Add(rowEdgeSecond[k].D);
                                list_col.Add(columnEdgeSecond[k].D);
                            }
                        }
                        hMeasure.CloseMeasure();
                    }
                    break;
                case enMeasureMethod.双向:
                    //////////////////////////////// 水平 /////////////////////////////
                    for (int i = (int)(param.SampleDist * 0.5) + row1; i < row2; i += param.SampleDist * param.SampleScale)
                    {
                        hMeasure = new HMeasure(i, (col1 + col2) * 0.5, 0, (col2 - col1) * 0.5, param.SampleDist * 0.5, width, height, "nearest_neighbor");
                        hMeasure.MeasurePairs(image, param.Sigma, param.Threshold, "all", "all", out rowEdgeFirst, out columnEdgeFirst, out amplitudeFirst, out rowEdgeSecond, out columnEdgeSecond, out amplitudeSecond, out intraDistance, out interDistance);
                        if (rowEdgeFirst != null && rowEdgeFirst.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeFirst.Length; k++)
                            {
                                list_row.Add(rowEdgeFirst[k].D);
                                list_col.Add(columnEdgeFirst[k].D);
                            }
                        }
                        if (rowEdgeSecond != null && rowEdgeSecond.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeSecond.Length; k++)
                            {
                                list_row.Add(rowEdgeSecond[k].D);
                                list_col.Add(columnEdgeSecond[k].D);
                            }
                        }
                        hMeasure.CloseMeasure();
                    }
                    /////////////////////////////// 垂直 //////////////////////////////// 
                    for (int i = (int)(param.SampleDist * 0.5) + col1; i < col2; i += param.SampleDist * param.SampleScale)
                    {
                        hMeasure = new HMeasure((row2 + row1) * 0.5, i, Math.PI * 0.5, (row2 - row1) * 0.5, param.SampleDist * 0.5, width, height, "nearest_neighbor");
                        hMeasure.MeasurePairs(image, param.Sigma, param.Threshold, "all", "all", out rowEdgeFirst, out columnEdgeFirst, out amplitudeFirst, out rowEdgeSecond, out columnEdgeSecond, out amplitudeSecond, out intraDistance, out interDistance);
                        if (rowEdgeFirst != null && rowEdgeFirst.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeFirst.Length; k++)
                            {
                                list_row.Add(rowEdgeFirst[k].D);
                                list_col.Add(columnEdgeFirst[k].D);
                            }
                        }
                        if (rowEdgeSecond != null && rowEdgeSecond.Length > 0)
                        {
                            for (int k = 0; k < rowEdgeSecond.Length; k++)
                            {
                                list_row.Add(rowEdgeSecond[k].D);
                                list_col.Add(columnEdgeSecond[k].D);
                            }
                        }
                        hMeasure.CloseMeasure();
                    }
                    break;
            }
            if (list_row.Count > 0)
            {
                HRegion hRegion1 = new HRegion();
                hRegion1.GenRegionPoints(list_row.ToArray(), list_col.ToArray());
                HRegion dilationRegion = hRegion1.DilationRectangle1(param.Width, param.Height);
                HRegion connRegion = dilationRegion.Connection();
                hRegion = connRegion.SelectShape("area", "and", param.Area, double.MaxValue);
                hRegion1?.Dispose();
                dilationRegion?.Dispose();
                connRegion?.Dispose();
            }
            //HSystem.SetSystem("int_zooming", "false");
            result = true;
            return result;
        }





    }


}
