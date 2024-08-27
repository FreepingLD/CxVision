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
    public class ImageMorphologyMethod
    {
        //private static object lockState = new object();
        //private static ImageMorphologyMethod _Instance = null;
        public ImageMorphologyMethod()
        {

        }

        //public static ImageMorphologyMethod Instance
        //{
        //    get
        //    {
        //        if (_Instance == null)
        //        {
        //            lock (lockState)
        //            {
        //                if (_Instance == null)
        //                    _Instance = new ImageMorphologyMethod();
        //            }
        //        }
        //        return _Instance;
        //    }
        //}
        public HImage ImageMorphology(HImage hImage, ImageMorphologyParam param)
        {
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            /////////////////////////////////////////////
            HImage tarRegion = new HImage();
            switch (param.Method)
            {
                case nameof(enImageMorphology.gray_closing_rect):
                    tarRegion = gray_closing_rect(hImage, (ImageRectParam)param);
                    break;
                case nameof(enImageMorphology.gray_closing_shape):
                    tarRegion = gray_closing_shape(hImage, (ImageShapeParam)param);
                    break;
                case nameof(enImageMorphology.gray_dilation_rect):
                    tarRegion = gray_dilation_rect(hImage, (ImageRectParam)param);
                    break;
                case nameof(enImageMorphology.gray_dilation_shape):
                    tarRegion = gray_dilation_shape(hImage, (ImageShapeParam)param);
                    break;
                case nameof(enImageMorphology.gray_opening_rect):
                    tarRegion = gray_opening_rect(hImage, (ImageRectParam)param);
                    break;
                case nameof(enImageMorphology.gray_opening_shape):
                    tarRegion = gray_opening_shape(hImage, (ImageShapeParam)param);
                    break;
                case nameof(enImageMorphology.gray_erosion_rect):
                    tarRegion = gray_erosion_rect(hImage, (ImageRectParam)param);
                    break;
                case nameof(enImageMorphology.gray_erosion_shape):
                    tarRegion = gray_erosion_shape(hImage, (ImageShapeParam)param);
                    break;
            }
            return tarRegion;
        }


        private HImage gray_closing_rect(HImage hImage, ImageRectParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayClosingRect((int)param.MaskHeight, (int)param.MaskWidth);

            return tarImage;
        }
        private HImage gray_closing_shape(HImage hImage, ImageShapeParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayClosingShape(param.MaskHeight, param.MaskWidth, param.MaskShape);

            return tarImage;
        }
        private HImage gray_dilation_rect(HImage hImage, ImageRectParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayDilationRect((int)param.MaskHeight, (int)param.MaskWidth);

            return tarImage;
        }
        private HImage gray_dilation_shape(HImage hImage, ImageShapeParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayDilationShape(param.MaskHeight, param.MaskWidth, param.MaskShape);

            return tarImage;
        }
        private HImage gray_opening_rect(HImage hImage, ImageRectParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayOpeningRect((int)param.MaskHeight, (int)param.MaskWidth);

            return tarImage;
        }
        private HImage gray_opening_shape(HImage hImage, ImageShapeParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayOpeningShape(param.MaskHeight, param.MaskWidth, param.MaskShape);

            return tarImage;
        }
        private HImage gray_erosion_rect(HImage hImage, ImageRectParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayErosionRect((int)param.MaskHeight, (int)param.MaskWidth);

            return tarImage;
        }
        private HImage gray_erosion_shape(HImage hImage, ImageShapeParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GrayErosionShape(param.MaskHeight, param.MaskWidth, param.MaskShape);

            return tarImage;
        }
    }

    public enum enImageMorphology
    {
        gray_closing_rect,
        gray_closing_shape,
        gray_dilation_rect,
        gray_dilation_shape,
        gray_opening_rect,
        gray_opening_shape,
        gray_erosion_rect,
        gray_erosion_shape
    }





}
