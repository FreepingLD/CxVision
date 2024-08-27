using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace FunctionBlock
{

    [Serializable]
    public class GapMethod
    {


        /// <summary>
        /// 破片检测
        /// </summary>
        /// <param name="hRegion"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static enImagePose GetImagePose(HImage hImage, GapDetectParam param)
        {
            enImagePose imagePose = enImagePose.NONE;
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            ///////////////////////////////////////////////////////
            int imageWidth, imageHeight, row1, col1, row2, col2;
            hImage.GetImageSize(out imageWidth, out imageHeight);
            HRegion foregroundRegion = hImage.Threshold(param.MinThreshold, param.MaxThreshold);
            HRegion fillForegroundupRegion = foregroundRegion.FillUp(); // 填充区域
            HRegion foregroundRegionClose = fillForegroundupRegion.ClosingRectangle1(param.CloseWidth, param.CloseHeight); // 封闭区域
            foregroundRegionClose.SmallestRectangle1(out row1, out col1, out row2, out col2);
            if (col1 == 0 && col2 == 0)
                imagePose = enImagePose.NONE;
            if (col1 > 1)
                imagePose = enImagePose.首端;
            if (col2 < imageWidth - 2)
                imagePose = enImagePose.尾端;
            if (col1 == 0 && col2 == imageWidth - 1)
                imagePose = enImagePose.中间;
            /////////////////////////////
            foregroundRegion?.Dispose();
            fillForegroundupRegion?.Dispose();
            foregroundRegionClose?.Dispose();
            return imagePose;
        }

        public static bool GapDetect2(HImage hImage, userPixCoordSystem pixCoordSystem, GapDetectParam param, out HRegion hRegionGap, out HXLDCont detectShapeContour)
        {
            bool result = false;
            hRegionGap = new HRegion();
            hRegionGap.GenEmptyRegion();
            detectShapeContour = new HXLDCont();
            detectShapeContour.GenEmptyObj();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            ///////////////////////////////////////////////////////  判定图像的位置 /////////////////////////////////////////////
            HRegion hRegionClosing = null, diffHregion = null, foregroundRegion = null,
                    fillForegroundupRegion = null, foregroundRegionClose = null, connectRegion = null, selectRegion = null,
                    roiShapeRegion = null, edgeShapeRegion = null;
            //enImagePose imagePose;
            ///////////////////////////////////////////////////
            foreach (var item in param.ShapeParam)
            {
                switch (item.DetectMethod)
                {
                    case enDetectMethod.区域检测:
                        //imagePose = GetImagePose(hImage, param);
                        //if (imagePose != param.ImagePose) return true;
                        roiShapeRegion = (item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetRegion();
                        detectShapeContour = detectShapeContour.ConcatObj((item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD());
                        //// 检测两端区域，检测两侧的缺口
                        if (roiShapeRegion != null)
                        {
                            HImage reduceImage = hImage.ReduceDomain(roiShapeRegion);
                            foregroundRegion = reduceImage.Threshold(param.MinThreshold, param.MaxThreshold);
                            fillForegroundupRegion = foregroundRegion.FillUp(); // 填充孔洞
                            double width = Math.Sqrt(param.MinArea); // 计算最小瑕疵面积的平方根，由他来过滤悼小的区域
                            foregroundRegionClose = fillForegroundupRegion.ClosingRectangle1((int)width, (int)width); // 封闭小的区域
                            if (foregroundRegionClose.Area > 0)
                                diffHregion = roiShapeRegion.Difference(foregroundRegionClose);
                            else
                                diffHregion = foregroundRegionClose;
                            connectRegion = diffHregion.Connection();
                            selectRegion = connectRegion.SelectShape("area", "and", param.MinArea, double.MaxValue);
                            hRegionGap = hRegionGap.ConcatObj(selectRegion);
                            ////////////////////////////////////////////////
                            roiShapeRegion?.Dispose();
                            foregroundRegion?.Dispose();
                            fillForegroundupRegion?.Dispose();
                            connectRegion?.Dispose();
                            diffHregion?.Dispose();
                            selectRegion?.Dispose();
                            foregroundRegionClose?.Dispose();
                        }
                        break;
                    case enDetectMethod.边缘检测:
                        //imagePose = GetImagePose(hImage, param);
                        //if (imagePose != param.ImagePose) return true;
                        edgeShapeRegion = (item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetRegion();
                        detectShapeContour = detectShapeContour.ConcatObj((item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD());
                        //// 检测中间区域，检测边缘上的缺口
                        if (edgeShapeRegion != null)
                        {
                            HImage reduceImage = hImage.ReduceDomain(edgeShapeRegion);
                            foregroundRegion = reduceImage.Threshold(param.MinThreshold, param.MaxThreshold);
                            fillForegroundupRegion = foregroundRegion.FillUp();
                            hRegionClosing = fillForegroundupRegion.ClosingRectangle1(1, param.CloseHeight); // 只封闭高度方向上的，这样一来，缺口就不会被吃悼了
                            HRegion hRegionClosing2 = hRegionClosing.ClosingRectangle1(param.CloseWidth, 1);
                            if (foregroundRegionClose.Area > 0)
                                diffHregion = hRegionClosing2.Difference(hRegionClosing);
                            else
                                diffHregion = foregroundRegionClose;
                            connectRegion = diffHregion.Connection();
                            selectRegion = connectRegion.SelectShape("area", "and", param.MinArea, double.MaxValue);
                            hRegionGap = hRegionGap.ConcatObj(selectRegion);
                            ////////////////////////////////////////////////
                            edgeShapeRegion?.Dispose();
                            foregroundRegion?.Dispose();
                            fillForegroundupRegion?.Dispose();
                            hRegionClosing?.Dispose();
                            hRegionClosing2?.Dispose();
                            diffHregion?.Dispose();
                            connectRegion?.Dispose();
                            selectRegion?.Dispose();
                        }
                        break;
                    case enDetectMethod.裂纹检测:
                        edgeShapeRegion = (item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetRegion();
                        detectShapeContour = detectShapeContour.ConcatObj((item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD());
                        //// 检测中间区域，检测边缘上的缺口
                        if (edgeShapeRegion != null)
                        {
                            HImage reduceImage = hImage.ReduceDomain(edgeShapeRegion);
                            foregroundRegion = reduceImage.Threshold(param.MinThreshold, param.MaxThreshold);
                            fillForegroundupRegion = foregroundRegion.FillUp();
                            hRegionClosing = fillForegroundupRegion.ClosingRectangle1(1, param.CloseHeight);
                            HRegion hRegionClosing2 = hRegionClosing.ClosingRectangle1(param.CloseWidth, 1);
                            if (foregroundRegionClose.Area > 0)
                                diffHregion = hRegionClosing2.Difference(hRegionClosing);
                            else
                                diffHregion = foregroundRegionClose;
                            connectRegion = diffHregion.Connection();
                            selectRegion = connectRegion.SelectShape("area", "and", param.MinArea, double.MaxValue);
                            hRegionGap = hRegionGap.ConcatObj(selectRegion);
                            ////////////////////////////////////////////////
                            edgeShapeRegion?.Dispose();
                            foregroundRegion?.Dispose();
                            fillForegroundupRegion?.Dispose();
                            hRegionClosing?.Dispose();
                            hRegionClosing2?.Dispose();
                            diffHregion?.Dispose();
                            connectRegion?.Dispose();
                            selectRegion?.Dispose();
                        }
                        break;
                }
            }
            ////////////////////////////////////
            hRegionGap = hRegionGap.OpeningRectangle1(param.OpenWidth, param.OpenHeight);
            hRegionGap = hRegionGap.SelectShape("area", "and", param.MinArea, double.MaxValue);
            result = true;
            return result;
        }



    }

}
