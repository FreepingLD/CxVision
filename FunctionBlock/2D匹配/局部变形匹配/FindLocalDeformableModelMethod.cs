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
    public class FindLocalDeformableModelMethod
    {

        private HImage searchImageRegion;
        public HImage SearchImageRegion { get => searchImageRegion; set => searchImageRegion = value; }

        public bool find_local_deformable_model(HImage image, HDeformableModel deformableModel, FindLocalDeformableModelParam deformableModelParam, out LocalDeformableMatchingResult result)
        {
            // 输出匹配坐标
            HTuple matchRow = null;
            HTuple matchColumn = null;
            HTuple matchAngle = 0;
            HTuple matchScore = 0;
            HImage RectifiedImage = null;
            HImage vectorField = null;
            HXLDCont deformedContours = null;
            result = new LocalDeformableMatchingResult();
            int NumLevels;
            int width, height;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image 为空值或未初始化");
            }
            if (deformableModel == null || !deformableModel.IsInitialized())
            {
                throw new ArgumentNullException("deformableModel 为空值或未初始化");
            }
            if (deformableModelParam == null)
            {
                throw new ArgumentNullException("deformableModelParam 为空值或未初始化");
            }
            if (deformableModelParam.SearchRegion.Count > 0)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in deformableModelParam.SearchRegion)
                {
                    hRegion.ConcatObj(item.GetRegion());
                }
                image.GetImageSize(out width, out height);
                HTuple type = image.GetImageType();
                HImage constImage = new HImage(type.S, width, height).GenImageProto(0.0);

                if (image.CountChannels().D > 1)
                    this.searchImageRegion = image.AccessChannel(1).ReduceDomain(hRegion.Union1()).PaintGray(constImage);
                else
                    this.searchImageRegion = image.ReduceDomain(hRegion.Union1()).PaintGray(constImage);
                constImage?.Dispose();
            }
            else
            {
                if (image.CountChannels().D > 0)
                    this.searchImageRegion = image.AccessChannel(1);
                else
                    this.searchImageRegion = image;
            }
            /////////////////////////////////////////
            NumLevels = deformableModel.GetDeformableModelParams("num_levels").I;
            deformableModel.SetDeformableModelParam("timeout", deformableModelParam.TimeOut);
            //////////////////////////////////////////
            int limitLevels = (int)(NumLevels * 0.5) > 2 ? (int)(NumLevels * 0.5) : 2;
            RectifiedImage = deformableModel.FindLocalDeformableModel(this.searchImageRegion,
                                      out vectorField,
                                      out deformedContours,
                                      deformableModelParam.AngleStart * Math.PI / 180,
                                      (deformableModelParam.AngleExtent - deformableModelParam.AngleStart) * Math.PI / 180,
                                      deformableModelParam.ScaleRMin,
                                      deformableModelParam.ScaleRMax,
                                      deformableModelParam.ScaleCMin,
                                      deformableModelParam.ScaleCMax,
                                      deformableModelParam.MinScore,
                                      deformableModelParam.NumMatches,
                                      deformableModelParam.MaxOverlap,
                                      new HTuple(NumLevels, -2),
                                      deformableModelParam.Greediness,
                                      new HTuple("deformed_contours", "image_rectified"),
                                      deformableModelParam.ParamName,
                                      deformableModelParam.ParamValue,
                                      out matchScore,
                                      out matchRow,
                                      out matchColumn
                                      );
            ////////////////////////////////////////////////
            if (matchRow != null && matchRow.Length > 0)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                result.PixCoordSystem = new userPixCoordSystem();
                result.MatchScore = 0;
                result.MatchCont = new XldDataClass();
                ////////////////////////////////
                for (int i = 0; i < matchRow.Length; i++)
                {
                    result.PixCoordSystem.CurrentPoint = new Common.userPixVector(matchRow[i].D, matchColumn[i].D, 0);
                    result.PixCoordSystem.ReferencePoint = new Common.userPixVector(matchRow[i].D, matchColumn[i].D, 0);
                    result.MatchScore = matchScore[i].D;
                    result.RectifiedImage = RectifiedImage;
                    ///////////////////////////////////////////////
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, matchRow[i], matchColumn[i], 0.0);
                    result.MatchCont.AddXLDCont(hHomMat2D.AffineTransContourXld(deformableModel.GetDeformableModelContours(1)));
                }
                return true;
            }
            else
            {
                result = new LocalDeformableMatchingResult(1);
                return false;
            }
        }









    }


}
