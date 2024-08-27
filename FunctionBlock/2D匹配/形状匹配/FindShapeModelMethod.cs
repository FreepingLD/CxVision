using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FunctionBlock
{
    [Serializable]
    public class FindShapeModelMethod
    {
        [NonSerialized]
        private HImage searchImageRegion; // 属于临时变量
        [XmlIgnore]
        public HImage SearchImageRegion { get => searchImageRegion; set => searchImageRegion = value; }

        public bool find_aniso_shape_model(HImage image, HShapeModel shapeModelID, F_AnisoShapeModelParam fasmParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = 0;
            HTuple matchScaleR = 0;
            HTuple matchScaleC = 0;
            HTuple matchScore = 0;
            result = new MatchingResult();
            //result = new userPixCoordSystem[0];
            double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
            int NumLevels, minContrast;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image为空值或未初始化");
            }
            if (shapeModelID == null || !shapeModelID.IsInitialized())
            {
                throw new ArgumentNullException("shapeModelID为空值或未初始化");
            }
            if (fasmParam == null)
            {
                throw new ArgumentNullException("fasmParam 为空值或未初始化");
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            List<double> listScaleR = new List<double>();
            List<double> listScaleC = new List<double>();
            ///////////////////////////
            foreach (var item in fasmParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage); // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);
                /////////////////////////////////////////
                NumLevels = shapeModelID.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast);
                shapeModelID.SetShapeModelParam("timeout", fasmParam.TimeOut);
                //////////////////////////////////////////
                HTuple mathcNumLevels = new HTuple();
                switch (fasmParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        mathcNumLevels = new HTuple(NumLevels, -2);
                        break;
                }
                shapeModelID.FindAnisoShapeModel(this.searchImageRegion,
                    fasmParam.AngleStart * Math.PI / 180,
                    (fasmParam.AngleExtent - fasmParam.AngleStart) * Math.PI / 180,
                    fasmParam.ScaleRMin,
                    fasmParam.ScaleRMax,
                    fasmParam.ScaleCMin,
                    fasmParam.ScaleCMax,
                    new HTuple(fasmParam.MinScore),
                    fasmParam.NumMatches,
                    fasmParam.MaxOverlap,
                    new HTuple(fasmParam.SubPixel.ToString()),
                    mathcNumLevels, // 这种模式适合于噪声较多的图像，兼容性好
                    fasmParam.Greediness,
                    out matchRow,
                    out matchColumn,
                    out matchAngle,
                    out matchScaleR,
                    out matchScaleC,
                    out matchScore);
                //////////////////////////////////////////////////////////////////////////
                if (matchRow != null && matchRow.Length > 0)
                {
                    for (int i = 0; i < matchRow.Length; i++)
                    {
                        listRow.Add(matchRow[i].D);
                        listCol.Add(matchColumn[i].D);
                        listAngle.Add(matchAngle[i].D);
                        listScaleR.Add(matchScaleR[i].D);
                        listScaleC.Add(matchScaleC[i].D);
                        listScore.Add(matchScore[i].D);
                    }
                }
                //else
                //{
                //    listRow.Add(0);
                //    listCol.Add(0);
                //    listAngle.Add(0);
                //    listScaleR.Add(0);
                //    listScaleC.Add(0);
                //    listScore.Add(0);
                //}
            }
            ////////////////////////////////////////////////
            if (listRow != null && listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass();//result.MatchCont = new HXLDCont[matchRow.Length];
                                                      //////////////////////////////////
                                                      //HTuple sortRow, sortCol, sortAngle;
                                                      // SortPares(matchRow, matchColumn, matchAngle, out sortRow, out sortCol, out sortAngle);
                result.ModelIndex = 0;
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i] = new userPixCoordSystem();
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]); //寻找形状模型是寻找模型的当前实例位置                                                                                                          
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = matchScore[i].D;
                    if (matchScore[i].D > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(shapeModelID.GetShapeModelContours(1)));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }
        }
        public bool find_aniso_shape_models(HImage image, HShapeModel[] shapeModelID, F_AnisoShapeModelParam fasmParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = 0;
            HTuple matchScaleR = 0;
            HTuple matchScaleC = 0;
            HTuple matchScore = 0;
            result = new MatchingResult();
            HTuple modelIndex = 0;
            double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
            int NumLevels = int.MaxValue, minContrast;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image为空值或未初始化");
            }
            if (shapeModelID == null)
            {
                throw new ArgumentNullException("shapeModelID 为空值或未初始化");
            }
            foreach (var item in shapeModelID)
            {
                if (item != null && !item.IsInitialized())
                {
                    throw new ArgumentNullException("shapeModelID 有未初始化的模型句柄");
                }
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            List<double> listScaleR = new List<double>();
            List<double> listScaleC = new List<double>();
            //////////////////////////////////////////
            if (fasmParam.SearchRegion.Count == 0)
            {
                int width, height;
                image.GetImageSize(out width, out height);
                fasmParam.SearchRegion.Add(new ModelParam(new drawPixRect1(0, 0, height - 1, width - 1)));
            }
            foreach (var item in fasmParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                HTuple rowCenter, colCenter;
                hRegion.AreaCenter(out rowCenter, out colCenter);
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage); // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);
                /////////////////////////////////////////
                foreach (var item1 in shapeModelID)
                {
                    if (item1.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast) < NumLevels)
                        NumLevels = item1.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast);
                    item1.SetShapeModelParam("timeout", fasmParam.TimeOut);
                }
                //////////////////////////////  在匹配时，各个模型图像域间不能有无效区域   金字塔层级如果指定为
                HTuple mathcNumLevels = new HTuple();
                switch (fasmParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        for (int i = 0; i < shapeModelID.Length; i++)
                        {
                            mathcNumLevels = mathcNumLevels.TupleConcat(NumLevels, -2);
                        }
                        break;
                }

                HShapeModel.FindAnisoShapeModels(this.searchImageRegion,
                    shapeModelID,
                    fasmParam.AngleStart * Math.PI / 180,
                    (fasmParam.AngleExtent - fasmParam.AngleStart) * Math.PI / 180,
                    fasmParam.ScaleRMin,
                    fasmParam.ScaleRMax,
                    fasmParam.ScaleCMin,
                    fasmParam.ScaleCMax,
                    new HTuple(fasmParam.MinScore),
                    fasmParam.NumMatches,
                    fasmParam.MaxOverlap,
                    new HTuple(fasmParam.SubPixel.ToString()),
                    mathcNumLevels, // 这种模式适合于噪声较多的图像，兼容性好
                    fasmParam.Greediness,
                    out matchRow,
                    out matchColumn,
                    out matchAngle,
                    out matchScaleR,
                    out matchScaleC,
                    out matchScore,
                    out modelIndex);
                //////////////////////////////////////////////////////////////////////////
                if (matchRow != null && matchRow.Length > 0)
                {
                    for (int i = 0; i < matchRow.Length; i++)
                    {
                        listRow.Add(matchRow[i].D);
                        listCol.Add(matchColumn[i].D);
                        listAngle.Add(matchAngle[i].D);
                        listScaleR.Add(matchScaleR[i].D);
                        listScaleC.Add(matchScaleC[i].D);
                        listScore.Add(matchScore[i].D);
                    }
                }
                else
                {
                    if (fasmParam.FiilUp)
                    {
                        listRow.Add(rowCenter.D);
                        listCol.Add(colCenter.D);
                        listAngle.Add(0);
                        listScaleR.Add(0);
                        listScaleC.Add(0);
                        listScore.Add(0);
                    }
                }
                if (listRow.Count == fasmParam.NumMatches) break; // 如果找到的对象数量等于设定的数量，那么将中断寻找
            }
            ////////////////////////////////////////////////
            if (listRow != null && listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass();//result.MatchCont = new HXLDCont[matchRow.Length];
                result.ModelIndex = (int)modelIndex.D;
                //////////////////////////////////
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]); //寻找形状模型是寻找模型的当前实例位置                                                                                                          
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = listScore[i];
                    if (listScore[i] > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(shapeModelID[modelIndex].GetShapeModelContours(1)));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }
        }
        public bool find_scaled_shape_model(HImage image, HShapeModel shapeModelID, F_ScaledShapeModelParam fssmParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = null;
            HTuple matchScale = null;
            HTuple matchScore = null;
            //result = new userPixCoordSystem[0];
            result = new MatchingResult();
            ////////////////////////////////////////////////////////
            double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
            int NumLevels = 3, minContrast;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image为空值或未初始化");
            }
            if (shapeModelID == null || !shapeModelID.IsInitialized())
            {
                throw new ArgumentNullException("shapeModelID为空值或未初始化");
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            List<double> listScale = new List<double>();
            ///////////////////////////
            if (fssmParam.SearchRegion.Count == 0)
            {
                int width, height;
                image.GetImageSize(out width, out height);
                fssmParam.SearchRegion.Add(new ModelParam(new drawPixRect1(0, 0, height - 1, width - 1)));
            }
            foreach (var item in fssmParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage); // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);
                /////////////////////////////////////////
                NumLevels = shapeModelID.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast);
                shapeModelID.SetShapeModelParam("timeout", fssmParam.TimeOut);
                //////////////////////////////////
                HTuple mathcNumLevels = new HTuple();
                switch (fssmParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        mathcNumLevels = new HTuple(NumLevels, -2);
                        break;
                }
                shapeModelID.FindScaledShapeModel(this.searchImageRegion,
                    fssmParam.AngleStart * Math.PI / 180,
                    (fssmParam.AngleExtent - fssmParam.AngleStart) * Math.PI / 180,
                    fssmParam.ScaleMin,
                    fssmParam.ScaleMax,
                    new HTuple(fssmParam.MinScore),
                    fssmParam.NumMatches,
                    fssmParam.MaxOverlap,
                    new HTuple(fssmParam.SubPixel.ToString()),
                    mathcNumLevels,
                    fssmParam.Greediness,
                    out matchRow,
                    out matchColumn,
                    out matchAngle,
                    out matchScale,
                    out matchScore);
                //////////////////////////////////////////////////////////////////////////
                if (matchRow != null && matchRow.Length > 0)
                {
                    for (int i = 0; i < matchRow.Length; i++)
                    {
                        listRow.Add(matchRow[i].D);
                        listCol.Add(matchColumn[i].D);
                        listAngle.Add(matchAngle[i].D);
                        listScale.Add(matchScale[i].D);
                        listScore.Add(matchScore[i].D);
                    }
                }
                //else
                //{
                //    listRow.Add(0);
                //    listCol.Add(0);
                //    listAngle.Add(0);
                //    listScale.Add(0);
                //    listScore.Add(0);
                //}
            }
            ////////////////////////////////////
            if (listRow != null && listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass();//result.MatchCont = new HXLDCont[matchRow.Length];
                //////////////////////////////////////
                //HTuple sortRow, sortCol, sortAngle;
                // SortPares(matchRow, matchColumn, matchAngle, out sortRow, out sortCol, out sortAngle);
                result.ModelIndex = 0;
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i] = new userPixCoordSystem();
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]); //寻找形状模型是寻找模型的当前实例位置                                                                                                          
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = listScore[i];
                    if (listScore[i] > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(shapeModelID.GetShapeModelContours(1)));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }
        }
        public bool find_scaled_shape_models(HImage image, HShapeModel[] shapeModelID, F_ScaledShapeModelParam fssmParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = null;
            HTuple matchScale = null;
            HTuple matchScore = null;
            HTuple modelIndex = 0;
            result = new MatchingResult();
            ////////////////////////////////////////////////////////
            double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
            int NumLevels = int.MaxValue, minContrast;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image为空值或未初始化");
            }
            if (shapeModelID == null)
            {
                throw new ArgumentNullException("shapeModelID 为空值或未初始化");
            }
            foreach (var item in shapeModelID)
            {
                if (item != null && !item.IsInitialized())
                {
                    throw new ArgumentNullException("shapeModelID 有未初始化的模型句柄");
                }
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            List<double> listScale = new List<double>();
            //////////////////////////////////////////
            if (fssmParam.SearchRegion.Count == 0)
            {
                int width, height;
                image.GetImageSize(out width, out height);
                fssmParam.SearchRegion.Add(new ModelParam(new drawPixRect1(0, 0, height - 1, width - 1)));
            }
            foreach (var item in fssmParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                HTuple rowCenter, colCenter;
                hRegion.AreaCenter(out rowCenter, out colCenter);
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage); // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);
                /////////////////////////////////////////
                foreach (var item1 in shapeModelID)
                {
                    if (item1.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast) < NumLevels)
                        NumLevels = item1.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast);
                    item1.SetShapeModelParam("timeout", fssmParam.TimeOut);
                }
                //////////////////////////////  在匹配时，各个模型图像域间不能有无效区域   金字塔层级如果指定为
                HTuple mathcNumLevels = new HTuple();
                switch (fssmParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        for (int i = 0; i < shapeModelID.Length; i++)
                        {
                            mathcNumLevels = mathcNumLevels.TupleConcat(NumLevels, -2);
                        }
                        break;
                }
                HShapeModel.FindScaledShapeModels(this.searchImageRegion,
                    shapeModelID,
                fssmParam.AngleStart * Math.PI / 180,
                (fssmParam.AngleExtent - fssmParam.AngleStart) * Math.PI / 180,
                fssmParam.ScaleMin,
                fssmParam.ScaleMax,
                new HTuple(fssmParam.MinScore),
                fssmParam.NumMatches,
                fssmParam.MaxOverlap,
                new HTuple(fssmParam.SubPixel.ToString()),
                mathcNumLevels,
                fssmParam.Greediness,
                out matchRow,
                out matchColumn,
                out matchAngle,
                out matchScale,
                out matchScore,
                out modelIndex);
                //////////////////////////////////////////////////////////////////////////
                if (matchRow != null && matchRow.Length > 0)
                {
                    for (int i = 0; i < matchRow.Length; i++)
                    {
                        listRow.Add(matchRow[i].D);
                        listCol.Add(matchColumn[i].D);
                        listAngle.Add(matchAngle[i].D);
                        listScale.Add(matchScale[i].D);
                        listScore.Add(matchScore[i].D);
                    }
                }
                else
                {
                    if (fssmParam.FiilUp)
                    {
                        listRow.Add(rowCenter.D);
                        listCol.Add(colCenter.D);
                        listAngle.Add(0);
                        listScale.Add(0);
                        listScore.Add(0);
                    }
                }
                if (listRow.Count == fssmParam.NumMatches) break; // 如果找到的对象数量等于设定的数量，那么将中断寻找

            }
            ////////////////////////////////////
            if (listRow != null && listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass();//result.MatchCont = new HXLDCont[matchRow.Length];
                result.ModelIndex = (int)modelIndex.D;
                //////////////////////////////////////
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i] = new userPixCoordSystem();
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]); //寻找形状模型是寻找模型的当前实例位置                                                                                                          
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = listScore[i];
                    if (listScore[i] > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(shapeModelID[modelIndex].GetShapeModelContours(1)));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }
        }
        public bool find_shape_model(HImage image, HShapeModel shapeModelID, F_ShapeModelParam fsmParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = 0;
            HTuple matchScore = 0;
            result = new MatchingResult();
            double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
            int NumLevels = 3, minContrast;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image 为空值或未初始化");
            }
            if (shapeModelID == null || !shapeModelID.IsInitialized())
            {
                throw new ArgumentNullException("shapeModelID 为空值或未初始化");
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            //////////////////////////////////////////
            if (fsmParam.SearchRegion.Count == 0)
            {
                int width, height;
                image.GetImageSize(out width, out height);
                fsmParam.SearchRegion.Add(new ModelParam(new drawPixRect1(0, 0, height - 1, width - 1)));
            }
            foreach (var item in fsmParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage); // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);
                /////////////////////////////////////////
                NumLevels = shapeModelID.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast);
                shapeModelID.SetShapeModelParam("timeout", fsmParam.TimeOut);
                //////////////////////////////  在匹配时，各个模型图像域间不能有无效区域
                HTuple mathcNumLevels = new HTuple();
                switch (fsmParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        mathcNumLevels = new HTuple(NumLevels, -2);
                        break;
                }
                shapeModelID.FindShapeModel(this.searchImageRegion,
                fsmParam.AngleStart * Math.PI / 180,
                (fsmParam.AngleExtent - fsmParam.AngleStart) * Math.PI / 180,
                new HTuple(fsmParam.MinScore),
                fsmParam.NumMatches,
                fsmParam.MaxOverlap,
                new HTuple(fsmParam.SubPixel.ToString()),
                mathcNumLevels,
                fsmParam.Greediness,
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
            ///////////////////////////////////
            if (listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass();                 // new HXLDCont[matchRow.Length];
                result.ModelIndex = 0;
                //////////////////////////////////////////////
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i] = new userPixCoordSystem();
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);  //寻找形状模型是寻找模型的当前实例位置                                                                                                          
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = listScore[i];
                    if (listScore[i] > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(shapeModelID.GetShapeModelContours(1)));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }

        }

        public bool find_shape_models(HImage image, HShapeModel[] shapeModelID, F_ShapeModelParam fsmParam, out MatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = 0;
            HTuple matchScore = 0;
            HTuple modelIndex = 0;
            result = new MatchingResult();
            double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
            int NumLevels = int.MaxValue, minContrast;
            string metric;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image 为空值或未初始化");
            }
            if (shapeModelID == null)
            {
                throw new ArgumentNullException("shapeModelID 为空值或未初始化");
            }
            foreach (var item in shapeModelID)
            {
                if (item != null && !item.IsInitialized())
                {
                    throw new ArgumentNullException("shapeModelID 有未初始化的模型句柄");
                }
            }
            ///////////////////////////
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listAngle = new List<double>();
            List<double> listScore = new List<double>();
            //////////////////////////////////////////
            if (fsmParam.SearchRegion.Count == 0)
            {
                int width, height;
                image.GetImageSize(out width, out height);
                fsmParam.SearchRegion.Add(new ModelParam(new drawPixRect1(0, 0, height - 1, width - 1)));
            }
            foreach (var item in fsmParam.SearchRegion)
            {
                HRegion hRegion = item.RoiShape.GetRegion();
                HTuple rowCenter, colCenter;
                hRegion.AreaCenter(out rowCenter, out colCenter);
                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion);//.PaintGray(constImage); // 在匹配时，各个模型图像域间不能有无效区域，所以这里需要通过一个中间图像来转换,但如果是单个来匹配，就不受影响
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion);//.PaintGray(constImage);
                /////////////////////////////////////////
                foreach (var item1 in shapeModelID)
                {
                    if (item1.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast) < NumLevels)
                        NumLevels = item1.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast);
                    item1.SetShapeModelParam("timeout", fsmParam.TimeOut);
                }
                //////////////////////////////  在匹配时，各个模型图像域间不能有无效区域   金字塔层级如果指定为
                HTuple mathcNumLevels = new HTuple();  // 匹配时可以设置为正常模式与兼容模式
                switch (fsmParam.MatchMode)
                {
                    default:
                    case enMatchMode.正常模式:
                        mathcNumLevels = 0;
                        break;
                    case enMatchMode.兼容模式:
                        for (int i = 0; i < shapeModelID.Length; i++)
                        {
                            mathcNumLevels = mathcNumLevels.TupleConcat(NumLevels, -2);
                        }
                        break;
                }
                HShapeModel.FindShapeModels(this.searchImageRegion,
                                            shapeModelID,
                                            new HTuple(fsmParam.AngleStart * Math.PI / 180),
                                            new HTuple((fsmParam.AngleExtent - fsmParam.AngleStart) * Math.PI / 180),
                                            new HTuple(fsmParam.MinScore),
                                            new HTuple(fsmParam.NumMatches),
                                            new HTuple(fsmParam.MaxOverlap),
                                            new HTuple(fsmParam.SubPixel.ToString()),
                                            mathcNumLevels,
                                            new HTuple(fsmParam.Greediness),
                                            out matchRow,
                                            out matchColumn,
                                            out matchAngle,
                                            out matchScore,
                                            out modelIndex);
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
                else
                {
                    if (fsmParam.FiilUp)
                    {
                        listRow.Add(rowCenter.D);
                        listCol.Add(colCenter.D);
                        listAngle.Add(0);
                        listScore.Add(0);
                    }
                }
                if (listRow.Count == fsmParam.NumMatches) break; // 如果找到的对象数量等于设定的数量，那么将中断寻找
            }
            ///////////////////////////////////
            Sort(listRow, listCol, fsmParam.SortMethod, out listRow, out listCol);
            if (listRow.Count > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem[listRow.Count];
                result.MatchScore = new double[listRow.Count];
                result.MatchCont = new XldDataClass();                 // new HXLDCont[matchRow.Length];
                result.ModelIndex = (int)modelIndex.D;
                //////////////////////////////////////////////
                for (int i = 0; i < listRow.Count; i++)
                {
                    result.PixCoordSystem[i] = new userPixCoordSystem();
                    result.PixCoordSystem[i].CurrentPoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);  //寻找形状模型是寻找模型的当前实例位置                                                                                                          
                    result.PixCoordSystem[i].ReferencePoint = new Common.userPixVector(listRow[i], listCol[i], listAngle[i]);
                    result.MatchScore[i] = listScore[i];
                    if (listScore[i] > 0)
                        result.PixCoordSystem[i].Result = true;
                    else
                        result.PixCoordSystem[i].Result = false;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, listRow[i], listCol[i], listAngle[i]);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(shapeModelID[modelIndex].GetShapeModelContours(1)));
                }
                return true;
            }
            else
            {
                result = new MatchingResult(1);
                return false;
            }

        }

        private void SortPares(HTuple matchRow, HTuple matchColumn, HTuple matchAngle, out HTuple SortRow, out HTuple SortCol, out HTuple SortT)
        {
            SortRow = new HTuple();
            SortCol = new HTuple();
            SortT = new HTuple();
            if (matchRow == null)
            {
                throw new ArgumentNullException("T1");
            }
            if (matchColumn == null)
            {
                throw new ArgumentNullException("T2");
            }
            if (matchAngle == null)
            {
                throw new ArgumentNullException("T3");
            }
            // 行排序
            HTuple index = matchRow.TupleSortIndex();
            HTuple select_row = matchRow.TupleSelect(index);
            HTuple select_col = matchColumn.TupleSelect(index);
            HTuple select_t = matchAngle.TupleSelect(index);
            // 列排序
            HTuple index_col = select_col.TupleSortIndex();
            SortRow = select_row.TupleSelect(index_col);
            SortCol = select_col.TupleSelect(index_col);
            SortT = select_t.TupleSelect(index_col);

        }
        private void SortPares(HTuple matchRow, HTuple matchColumn, HTuple matchAngle, HTuple matchScore, double offsetRow, out HTuple SortRow, out HTuple SortCol, out HTuple SortAngle, out HTuple SortScore)
        {
            SortRow = new HTuple();
            SortCol = new HTuple();
            SortAngle = new HTuple();
            SortScore = new HTuple();
            if (matchRow == null)
            {
                throw new ArgumentNullException("matchRow");
            }
            if (matchColumn == null)
            {
                throw new ArgumentNullException("matchColumn");
            }
            if (matchAngle == null)
            {
                throw new ArgumentNullException("matchAngle");
            }
            if (matchScore == null)
            {
                throw new ArgumentNullException("matchScore");
            }
            //////////////////////////////////////////////
            //Dictionary<string, double[]> dic = new Dictionary<string, double[]>();
            //for (int i = 0; i < matchRow.Length; i++)
            //{
            //    dic.Add(matchRow[i].D.ToString() + "," + matchColumn[i].D.ToString(), new double[] { matchAngle[i].D, matchScore[i].D });
            //}
            // 行排序
            HTuple index = matchRow.TupleSortIndex();
            HTuple select_row = matchRow.TupleSelect(index);
            HTuple select_col = matchColumn.TupleSelect(index);
            HTuple select_Angle = matchAngle.TupleSelect(index);
            HTuple select_Score = matchScore.TupleSelect(index);
            ///////////////////////////////////////////////////
            List<double> sortRow = new List<double>();
            List<double> sortCol = new List<double>();
            List<double> sortAngle = new List<double>();
            List<double> sortScore = new List<double>();
            List<double> tempRow = new List<double>();
            List<double> tempCol = new List<double>();
            List<double> tempAngle = new List<double>();
            List<double> tempScore = new List<double>();
            double refRow = select_row[0].D;
            if (select_row.Length > 1)
            {
                for (int i = 0; i < select_row.Length + 1; i++)
                {
                    if (i < select_row.Length && Math.Abs(select_row[i].D - refRow) < offsetRow)
                    {
                        tempRow.Add(select_row[i].D);
                        tempCol.Add(select_col[i].D);
                        tempAngle.Add(select_Angle[i].D);
                        tempScore.Add(select_Score[i].D);
                    }
                    else
                    {
                        if (i < select_row.Length)
                            refRow = select_row[i].D;
                        index = new HTuple(tempCol.ToArray()).TupleSortIndex();
                        sortRow.AddRange(new HTuple(tempRow.ToArray()).TupleSelect(index).DArr);
                        sortCol.AddRange(new HTuple(tempCol.ToArray()).TupleSelect(index).DArr);
                        sortAngle.AddRange(new HTuple(tempAngle.ToArray()).TupleSelect(index).DArr);
                        sortScore.AddRange(new HTuple(tempScore.ToArray()).TupleSelect(index).DArr);
                        tempRow.Clear();
                        tempCol.Clear();
                        tempAngle.Clear();
                        tempScore.Clear();
                        //////////////////////////
                        if (i < select_row.Length)
                        {
                            tempRow.Add(select_row[i].D);
                            tempCol.Add(select_col[i].D);
                            tempAngle.Add(select_Angle[i].D);
                            tempScore.Add(select_Score[i].D);
                        }
                    }
                }
            }
            else
            {
                sortRow.Add(select_row[0].D);
                sortCol.Add(select_col[0].D);
                sortAngle.Add(select_Angle[0].D);
                sortScore.Add(select_Score[0].D);
            }
            SortRow = new HTuple(sortRow.ToArray());
            SortCol = new HTuple(sortCol.ToArray());
            SortAngle = new HTuple(sortAngle.ToArray());
            SortScore = new HTuple(sortScore.ToArray());
            sortRow.Clear();
            sortCol.Clear();
            sortAngle.Clear();
            sortScore.Clear();
        }


        private void SortParesAndUniq(HTuple matchRow, HTuple matchColumn, double offsetRow, out HTuple SortRow, out HTuple SortCol)
        {
            SortRow = new HTuple();
            SortCol = new HTuple();
            if (matchRow == null)
            {
                throw new ArgumentNullException("matchRow");
            }
            if (matchColumn == null)
            {
                throw new ArgumentNullException("matchColumn");
            }
            // 行排序
            HTuple index = matchRow.TupleSortIndex();
            HTuple select_row = matchRow.TupleSelect(index);
            HTuple select_col = matchColumn.TupleSelect(index);
            ///////////////////////////////////////////////////
            List<double> sortRow = new List<double>();
            List<double> sortCol = new List<double>();
            List<double> sortAngle = new List<double>();
            List<double> sortScore = new List<double>();
            List<double> tempRow = new List<double>();
            List<double> tempCol = new List<double>();
            double refRow = select_row[0].D;
            if (select_row.Length > 1)
            {
                for (int i = 0; i < select_row.Length + 1; i++)
                {
                    if (i < select_row.Length && Math.Abs(select_row[i].D - refRow) < offsetRow)
                    {
                        tempRow.Add(select_row[i].D);
                        tempCol.Add(select_col[i].D);
                    }
                    else
                    {
                        if (i < select_row.Length)
                            refRow = select_row[i].D;
                        index = new HTuple(tempCol.ToArray()).TupleSortIndex();
                        sortRow.AddRange(new HTuple(tempRow.ToArray()).TupleSelect(index).DArr); // 在列排序后去重
                        sortCol.AddRange(new HTuple(tempCol.ToArray()).TupleSelect(index).DArr);
                        tempRow.Clear();
                        tempCol.Clear();
                        //////////////////////////
                        if (i < select_row.Length)
                        {
                            tempRow.Add(select_row[i].D);
                            tempCol.Add(select_col[i].D);
                        }
                    }
                }
            }
            else
            {
                sortRow.Add(select_row[0].D);
                sortCol.Add(select_col[0].D);
            }
            SortRow = new HTuple(sortRow.ToArray());
            SortCol = new HTuple(sortCol.ToArray());
            sortRow.Clear();
            sortCol.Clear();
            sortAngle.Clear();
            sortScore.Clear();
        }


        private void Sort(List<double> listRow, List<double> listCol, enSortMethod sortMethod, out List<double> sortRow, out List<double> sortCol)
        {
            sortRow = new List<double>();
            sortCol = new List<double>();
            if (listRow.Count <= 1)
            {
                sortRow = listRow;
                sortCol = listCol;
                return;
            }
            HTuple hTupleRow, hTupleCol, hTupleSortIndex, hTupleSortRow, hTupleSortCol;
            switch (sortMethod)
            {
                default:
                case enSortMethod.NONE:
                    sortRow.AddRange(listRow.ToArray());
                    sortCol.AddRange(listCol.ToArray());
                    break;
                case enSortMethod.行排序:
                    hTupleRow = new HTuple(listRow.ToArray());
                    hTupleCol = new HTuple(listCol.ToArray());
                    hTupleSortIndex = hTupleRow.TupleSortIndex();
                    hTupleSortRow = hTupleRow.TupleSelect(hTupleSortIndex);
                    hTupleSortCol = hTupleCol.TupleSelect(hTupleSortIndex);
                    sortRow.AddRange(hTupleSortRow.DArr);
                    sortCol.AddRange(hTupleSortCol.DArr);
                    break;
                case enSortMethod.列排序:
                    hTupleRow = new HTuple(listRow.ToArray());
                    hTupleCol = new HTuple(listCol.ToArray());
                    hTupleSortIndex = hTupleCol.TupleSortIndex();
                    hTupleSortRow = hTupleRow.TupleSelect(hTupleSortIndex);
                    hTupleSortCol = hTupleCol.TupleSelect(hTupleSortIndex);
                    sortRow.AddRange(hTupleSortRow.DArr);
                    sortCol.AddRange(hTupleSortCol.DArr);
                    break;
                case enSortMethod.行列排序:
                    hTupleRow = new HTuple(listRow.ToArray());
                    hTupleCol = new HTuple(listCol.ToArray());
                    hTupleSortIndex = hTupleRow.TupleSortIndex();
                    hTupleSortRow = hTupleRow.TupleSelect(hTupleSortIndex);
                    hTupleSortCol = hTupleCol.TupleSelect(hTupleSortIndex);
                    ///////////////////////////
                    hTupleSortIndex = hTupleSortCol.TupleSortIndex();
                    hTupleSortRow = hTupleSortRow.TupleSelect(hTupleSortIndex);
                    hTupleSortCol = hTupleSortCol.TupleSelect(hTupleSortIndex);
                    sortRow.AddRange(hTupleSortRow.DArr);
                    sortCol.AddRange(hTupleSortCol.DArr);
                    break;
            }
        }




    }


}
