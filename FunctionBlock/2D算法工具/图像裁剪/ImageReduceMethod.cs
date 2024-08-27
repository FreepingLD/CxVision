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
    public class ImageReduceMethod
    {

        public ImageReduceMethod()
        {

        }

        public bool ReduceImageDomain(HImage image, ImageReduceParam param, out HImage ReduceImage)
        {
            bool result = false;
            ReduceImage = null;
            HRegion hRegion = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (param == null)
                throw new ArgumentNullException("param");
            HTuple row = new HTuple(); HTuple col = new HTuple(); HTuple rad = new HTuple(); HTuple length1 = new HTuple(); HTuple length2 = new HTuple();
            foreach (var item in param.ReduceRegion)
            {
                drawPixRect2 pixRect2 = item.AffinePixRect2(param.PixCoordSys?.GetVariationHomMat2D());
                if (pixRect2.Length1 > 0 && pixRect2.Length2 > 0)
                {
                    row.Append(pixRect2.Row);
                    col.Append(pixRect2.Col);
                    rad.Append(pixRect2.Rad);
                    length1.Append(pixRect2.Length1);
                    length2.Append(pixRect2.Length2);
                }
            }
            if (row.Length > 0)
            {
                HRegion hRegionTemp = new HRegion();
                int width, height;
                image.GetImageSize(out width, out height);
                hRegionTemp.GenRectangle2(row, col, rad, length1, length2);
                ReduceImage = image.ReduceDomain(hRegionTemp.Union1());
            }
            else
                ReduceImage = image;
            result = true;
            /////////////////////////////
            return result;
        }


        public bool ReduceImageDomain(HImage image,userPixCoordSystem pixCoordSystem, BindingList<ReduceParam> paramList, out HImage ReduceImage)
        {
            bool result = false;
            ReduceImage = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (paramList == null)
                throw new ArgumentNullException("param");
            HRegion hRegion = null;
            HImage removeImage = null;
            HRegion reserveRegion = new HRegion(); // 保留区域
            HRegion removeRegion = new HRegion(); // 移除区域
            reserveRegion.GenEmptyRegion();
            removeRegion.GenEmptyRegion();
            foreach (var item in paramList)
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
                                hRegion = (item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetRegion();
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
                                hRegion = (item.RoiShape).AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetRegion();
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
                ReduceImage = removeImage.ReduceDomain(reserveRegion.Union1());
            else
                ReduceImage = removeImage.Clone();
            removeImage?.Dispose();
            result = true;
            /////////////////////////////
            return result;
        }




    }
}
