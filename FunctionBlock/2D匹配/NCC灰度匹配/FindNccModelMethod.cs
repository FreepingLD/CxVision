using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FunctionBlock
{
    [Serializable]
    public class FindNccModelMethod
    {
        [NonSerialized]
        private HImage searchImageRegion;
        [XmlIgnore]
        public HImage SearchImageRegion { get => searchImageRegion; set => searchImageRegion = value; }

        public bool find_ncc_model(HImage image, HNCCModel nccModelID, FindNccModelParam NccModelParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = 0;
            HTuple matchScore = 0;
            result = new MatchingResult();
            double angleStart, angleExtent, angleStep;
            int NumLevels;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image为空值或未初始化");
            }
            if (nccModelID == null || !nccModelID.IsInitialized())
            {
                throw new ArgumentNullException("shapeModelID为空值或未初始化");
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            //////////////////////////////////////////
            if (NccModelParam.SearchRegion.Count == 0)
            {
                //if (image.CountChannels().D > 1)
                //    this.searchImageRegion = image.AccessChannel(1);
                //else
                //    this.searchImageRegion = image;
                int width, height;
                image.GetImageSize(out width, out height);
                NccModelParam.SearchRegion.Add(new ModelParam(new drawPixRect1(0, 0, height - 1, width - 1)));
            }
            foreach (var item in NccModelParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                HTuple rowCenter, colCenter;
                hRegion.AreaCenter(out rowCenter, out colCenter);
                //int width, height;
                //image.GetImageSize(out width, out height);
                //HTuple type = image.GetImageType();
                //HImage constImage = new HImage(type.S, width, height).GenImageProto(0.0); // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage);
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);
                //constImage.Dispose();
                /////////////////////////////////////////
                NumLevels = nccModelID.GetNccModelParams(out angleStart, out angleExtent, out angleStep, out metric);
                nccModelID.SetNccModelParam("timeout", NccModelParam.TimeOut);
                //////////////////////////////////////////
                //int limitLevels = (int)(NumLevels * 0.5) > 2 ? (int)(NumLevels * 0.5) : 2;
                HTuple mathcNumLevels = new HTuple();
                switch (NccModelParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        mathcNumLevels = new HTuple(NumLevels, -2);
                        break;
                }
                nccModelID.FindNccModel(this.searchImageRegion,
                                          NccModelParam.AngleStart * Math.PI / 180,
                                          (NccModelParam.AngleExtent - NccModelParam.AngleStart) * Math.PI / 180,
                                          NccModelParam.MinScore,
                                          NccModelParam.NumMatches,
                                          NccModelParam.MaxOverlap,
                                          new HTuple(NccModelParam.SubPixel),
                                          mathcNumLevels,
                                          out matchRow,
                                          out matchColumn,
                                          out matchAngle,
                                          out matchScore);
                //////////////////////////////////////////////////////////////////////////
                if (matchRow != null && matchRow.Length > 0)
                {
                    for (int i = 0; i < matchRow.Length; i++)
                    {
                        listRow.Add(matchRow[i].D);
                        listCol.Add(matchColumn[i].D);
                        listAngle.Add(matchAngle[i].D);
                        listScore.Add(0);
                    }
                }
                else
                {
                    if (NccModelParam.FiilUp)
                    {
                        listRow.Add(rowCenter.D);
                        listCol.Add(colCenter.D);
                        listAngle.Add(0);
                        listScore.Add(0);
                    }
                }
                if (listRow.Count == NccModelParam.NumMatches) break; // 如果找到的对象数量等于设定的数量，那么将中断寻找
            }

            ////////////////////////////////////////////////
            if (listRow != null && listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass(); // new HXLDCont[matchRow.Length];
                result.ModelIndex = 0;
                //////////////////////////////////
                //HTuple sortRow, sortCol, sortAngle;
                //SortPares(matchRow, matchColumn, matchAngle, out sortRow, out sortCol, out sortAngle);
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i] = new userPixCoordSystem();
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = listScore[i];
                    if (listScore[i] > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    ///////////////////////////////
                    HRegion hRegion = nccModelID.GetNccModelRegion();
                    HXLDCont cont = hRegion.GenContourRegionXld("border_holes");
                    int num = cont.CountObj();
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(cont));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }
        }

        public bool find_ncc_models(HImage image, HNCCModel[] nccModelID, FindNccModelParam NccModelParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = 0;
            HTuple matchScore = 0;
            HTuple ModelIndex = 0;
            result = new MatchingResult();
            double angleStart, angleExtent, angleStep;
            int NumLevels = int.MaxValue;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image为空值或未初始化");
            }
            if (nccModelID == null)
            {
                throw new ArgumentNullException("shapeModelID为空值或未初始化");
            }
            foreach (var item in nccModelID)
            {
                if (!item.IsInitialized())
                {
                    throw new ArgumentNullException("shapeModelID 有未初始化模型!");
                }
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            //////////////////////////////////////////
            if (NccModelParam.SearchRegion.Count == 0)
            {
                int width, height;
                image.GetImageSize(out width, out height);
                NccModelParam.SearchRegion.Add(new ModelParam(new drawPixRect1(0, 0, height - 1, width - 1)));
            }
            foreach (var item in NccModelParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage);
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);;
                /////////////////////////////////////////
                foreach (var item1 in nccModelID)
                {
                    if (item1.GetNccModelParams(out angleStart, out angleExtent, out angleStep, out metric) < NumLevels)
                        NumLevels = item1.GetNccModelParams(out angleStart, out angleExtent, out angleStep, out metric);
                    item1.SetNccModelParam("timeout", NccModelParam.TimeOut);
                }
                //////////////////////////////////////////
                //int limitLevels = (int)(NumLevels * 0.5) > 2 ? (int)(NumLevels * 0.5) : 2;
                //for (int i = NumLevels; i > limitLevels; i--)
                //{
                HTuple mathcNumLevels = new HTuple();
                switch (NccModelParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        for (int i = 0; i < nccModelID.Length; i++)
                        {
                            mathcNumLevels = mathcNumLevels.TupleConcat(NumLevels, -2);
                        }
                        break;
                }
                HNCCModel.FindNccModels(this.searchImageRegion,
                                    nccModelID,
                                    new HTuple(NccModelParam.AngleStart * Math.PI / 180),
                                    new HTuple((NccModelParam.AngleExtent - NccModelParam.AngleStart) * Math.PI / 180),
                                    new HTuple(NccModelParam.MinScore),
                                    new HTuple(NccModelParam.NumMatches),
                                    new HTuple(NccModelParam.MaxOverlap),
                                    new HTuple(NccModelParam.SubPixel),
                                    mathcNumLevels,
                                    out matchRow,
                                    out matchColumn,
                                    out matchAngle,
                                    out matchScore,
                                    out ModelIndex);
                //    if (matchRow != null && matchRow.Length > 0) break; // NumLevels, 2
                //}
                //////////////////////////////////////////////////////////////////////////
                if (matchRow != null && matchRow.Length > 0)
                {
                    for (int i = 0; i < matchRow.Length; i++)
                    {
                        listRow.Add(matchRow[i].D);
                        listCol.Add(matchColumn[i].D);
                        listAngle.Add(matchAngle[i].D);
                        listScore.Add(matchScore[i].D);
                    }
                }
                //else
                //{
                //    listRow.Add(0);
                //    listCol.Add(0);
                //    listAngle.Add(0);
                //    listScore.Add(0);
                //}
            }
            ////////////////////////////////////////////////
            if (listRow != null && listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass(); // new HXLDCont[matchRow.Length];
                result.ModelIndex = (int)ModelIndex.D;
                //////////////////////////////////
                //HTuple sortRow, sortCol, sortAngle;
                //SortPares(matchRow, matchColumn, matchAngle, out sortRow, out sortCol, out sortAngle);
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i] = new userPixCoordSystem();
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = listScore[i];
                    if (listScore[i] > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    ///////////////////////////////
                    HRegion hRegion = nccModelID[ModelIndex].GetNccModelRegion();
                    HXLDCont cont = hRegion.GenContourRegionXld("border_holes");
                    int num = cont.CountObj();
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(cont));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }
        }
        private void SortPares(HTuple Row, HTuple Col, HTuple T, out HTuple SortRow, out HTuple SortCol, out HTuple SortT)
        {
            SortRow = new HTuple();
            SortCol = new HTuple();
            SortT = new HTuple();
            if (Row == null)
            {
                throw new ArgumentNullException("T1");
            }
            if (Col == null)
            {
                throw new ArgumentNullException("T2");
            }
            if (T == null)
            {
                throw new ArgumentNullException("T3");
            }
            // 行排序
            HTuple index = Row.TupleSortIndex();
            HTuple select_row = Row.TupleSelect(index);
            HTuple select_col = Col.TupleSelect(index);
            HTuple select_t = T.TupleSelect(index);
            // 列排序
            HTuple index_col = select_col.TupleSortIndex();
            SortRow = select_row.TupleSelect(index_col);
            SortCol = select_col.TupleSelect(index_col);
            SortT = select_t.TupleSelect(index_col);

        }







    }


}
