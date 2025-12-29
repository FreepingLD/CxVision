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
    public class LineMethod
    {

        public static bool DetectLine(userWcsLine[] wcsLines, LineDetectParam param, out HXLDCont hXLDContNg, out userWcsLine fitLine)
        {
            bool result = false;
            hXLDContNg = new HXLDCont();
            if (wcsLines == null || wcsLines.Length == 0)
            {
                throw new ArgumentNullException("wcsLines");
            }
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }
            if (wcsLines.Length == 0)
            {
                throw new ArgumentException("wcsLines 长度为0 ");
            }
            /////////////////////////////
            userPixLine pixLine;
            userPixPoint pixPoint;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_row = new List<double>();
            List<double> list_col = new List<double>();
            List<double> list_edge_row = new List<double>();
            List<double> list_edge_col = new List<double>();
            foreach (var item in wcsLines)
            {
                list_x.Add(item.X1);
                list_x.Add(item.X2);
                list_y.Add(item.Y1);
                list_y.Add(item.Y2);
                pixLine = item.GetPixLine();
                list_row.Add(pixLine.Row1);
                list_row.Add(pixLine.Row2);
                list_col.Add(pixLine.Col1);
                list_col.Add(pixLine.Col2);
                if (item.EdgesPoint_xyz != null)
                {
                    foreach (var item2 in item.EdgesPoint_xyz)
                    {
                        pixPoint = item2.GetPixPoint();
                        list_edge_row.Add(pixPoint.Row);
                        list_edge_col.Add(pixPoint.Col);
                    }
                }
            }
            double RowBegin = 0, ColBegin = 0, RowEnd = 0, ColEnd = 0, Nr = 0, Nc = 0, Dist = 0, sort_x = 0, sort_y = 0;
            new HXLDCont(list_y.ToArray(), list_x.ToArray())?.FitLineContourXld(param.Algorithm, (int)param.MaxNumPoints, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
            fitLine = new userWcsLine(ColBegin, RowBegin,0, ColEnd, RowEnd,0, wcsLines[0].CamParams);
            fitLine.Grab_x = wcsLines[0].Grab_x;
            fitLine.Grab_y = wcsLines[0].Grab_y;
            fitLine.Grab_theta = wcsLines[0].Grab_theta;
            fitLine.CamName = wcsLines[0]?.CamName;
            fitLine.ViewWindow = wcsLines[0]?.ViewWindow;
            fitLine.Color =enColor.green;
            //////////////////////////////////////////////////////////////
            new HXLDCont(list_row.ToArray(), list_col.ToArray())?.FitLineContourXld(param.Algorithm, (int)param.MaxNumPoints, (int)param.ClippingEndPoints, param.Iterations, param.ClippingFactor, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
            double rowProj, colProj;
            List<double> listProjRow = new List<double>();
            List<double> listProjCol = new List<double>();
            HTuple dist = HMisc.DistancePl(list_edge_row.ToArray(), list_edge_col.ToArray(), RowBegin, ColBegin, RowEnd, ColEnd); // 计算所有点的距离
            ///////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < list_edge_row.Count; i++)
            {
                HMisc.ProjectionPl(list_edge_row[i], list_edge_col[i], RowBegin, ColBegin, RowEnd, ColEnd, out rowProj, out colProj);
                listProjRow.Add(rowProj);
                listProjCol.Add(colProj);
            }

            double linePhi = Math.Atan2(RowEnd - RowBegin, ColEnd - ColBegin);
            HHomMat2D hHomMat2D = new HHomMat2D();
            //HHomMat2D hHomMat2D1 = hHomMat2D.HomMat2dRotate(Math.PI * 0.5, (RowEnd + RowBegin) * 0.5, (ColEnd + ColBegin));
            double normalPhi1 = linePhi + Math.PI * 0.5;
            double normalPhi = 0;
            List<double> listRow1 = new List<double>();
            List<double> listCol1 = new List<double>();
            List<double> listRow2 = new List<double>();
            List<double> listCol2 = new List<double>();
            for (int i = 0; i < dist.Length; i++)
            {
                switch (param.Unit)
                {
                    case "mm":
                        if (dist[i]*param.PixScale >= param.DistThreshold) // 将直线两边的点分开，将坐标点分隔在直线的两侧
                        {
                            normalPhi = Math.Atan2(list_edge_row[i] - listProjRow[i], list_edge_col[i] - listProjCol[i]) * -1;
                            if (normalPhi - normalPhi1 < 0.1) // 将直线两边的点分开，将坐标点分隔在直线的两侧
                            {
                                listRow1.Add(list_edge_row[i]);
                                listCol1.Add(list_edge_col[i]);
                            }
                            else
                            {
                                listRow2.Add(list_edge_row[i]);
                                listCol2.Add(list_edge_col[i]);
                            }
                        }
                        break;
                    default:
                    case "pix":
                        if (dist[i] >= param.DistThreshold) // 将直线两边的点分开，将坐标点分隔在直线的两侧
                        {
                            normalPhi = Math.Atan2(list_edge_row[i] - listProjRow[i], list_edge_col[i] - listProjCol[i]) * -1;
                            if (normalPhi - normalPhi1 < 0.1) // 将直线两边的点分开，将坐标点分隔在直线的两侧
                            {
                                listRow1.Add(list_edge_row[i]);
                                listCol1.Add(list_edge_col[i]);
                            }
                            else
                            {
                                listRow2.Add(list_edge_row[i]);
                                listCol2.Add(list_edge_col[i]);
                            }
                        }
                        break;
                }
            }
            HXLDCont hXLD1 = SelectNgPoint(listRow1, listCol1, param, RowBegin, ColBegin, RowEnd, ColEnd);
            HXLDCont hXLD2 = SelectNgPoint(listRow2, listCol2, param, RowBegin, ColBegin, RowEnd, ColEnd);
            hXLDContNg = hXLD1.ConcatObj(hXLD2);
            result = true;
            return result;

        }

        private static HXLDCont SelectNgPoint(List<double> listRow, List<double> listCol, LineDetectParam param, double RowBegin, double ColBegin, double RowEnd, double ColEnd)
        {
            HXLDCont hXLDContNg = new HXLDCont();
            ///////  帅选点 ////////////////////////////////////////
            List<List<double>> hTuplesRow = new List<List<double>>();
            List<List<double>> hTuplesCol = new List<List<double>>();
            List<double> tempRow = new List<double>();
            List<double> tempCol = new List<double>();
            if (listRow.Count > 0)
            {
                double distPp = 0;
                double currentPointRow = listRow[0];
                double currentPointCol = listCol[0];
                for (int i = 0; i < listRow.Count; i++)
                {
                    distPp = HMisc.DistancePp(currentPointRow, currentPointCol, listRow[i], listCol[i]);
                    if (param.Unit == "mm") // 转化成mm单位
                        distPp *= param.PixScale;
                    if (distPp <= param.PToPDist)    // 根据相邻两点间的距离来分组区域
                    {
                        tempRow.Add(listRow[i]);
                        tempCol.Add(listCol[i]);
                    }
                    else
                    {
                        if (tempRow.Count >= param.NgPointNum)
                        {
                            hTuplesRow.Add(tempRow);
                            hTuplesCol.Add(tempCol);
                        }
                        tempRow = new List<double>();
                        tempCol = new List<double>();
                        tempRow.Add(listRow[i]);
                        tempCol.Add(listCol[i]);
                    }
                    currentPointRow = listRow[i];
                    currentPointCol = listCol[i];
                }
            }
            // 这里最后一定要再做一次判定，以防所有的点满足条件而不能添加
            if (tempRow.Count >= param.NgPointNum)
            {
                hTuplesRow.Add(tempRow);
                hTuplesCol.Add(tempCol);
            }
            // 创建 NG 轮廓
            double row1Proj, col1Proj, row2Proj, col2Proj;
            hXLDContNg.GenEmptyObj();
            for (int i = 0; i < hTuplesRow.Count; i++)
            {
                List<double> listTempRow = hTuplesRow[i];
                List<double> listTempCol = hTuplesCol[i];
                HMisc.ProjectionPl(listTempRow[0], listTempCol[0], RowBegin, ColBegin, RowEnd, ColEnd, out row1Proj, out col1Proj);
                HMisc.ProjectionPl(listTempRow.Last(), listTempCol.Last(), RowBegin, ColBegin, RowEnd, ColEnd, out row2Proj, out col2Proj);
                listTempRow.Insert(0, row1Proj);
                listTempRow.Add(row2Proj);
                listTempCol.Insert(0, col1Proj);
                listTempCol.Add(col2Proj);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                switch (param.Unit)
                {
                    case "mm":
                        if((listTempRow.Max()- listTempRow.Min())*param.PixScale > param.NgHeight || (listTempCol.Max() - listTempCol.Min()) * param.PixScale > param.NgWidth)
                            hXLDContNg = hXLDContNg.ConcatObj(new HXLDCont(listTempRow.ToArray(), listTempCol.ToArray()).CloseContoursXld());
                        break;
                    case "pix":
                        if ((listTempRow.Max() - listTempRow.Min()) > param.NgHeight || (listTempCol.Max() - listTempCol.Min()) > param.NgWidth)
                            hXLDContNg = hXLDContNg.ConcatObj(new HXLDCont(listTempRow.ToArray(), listTempCol.ToArray()).CloseContoursXld());
                        break;
                }
                //hXLDContNg = hXLDContNg.ConcatObj(new HXLDCont(listTempRow.ToArray(), listTempCol.ToArray()).CloseContoursXld());
            }
            hTuplesRow.Clear();
            hTuplesCol.Clear();

            return hXLDContNg;
        }


    }


}
