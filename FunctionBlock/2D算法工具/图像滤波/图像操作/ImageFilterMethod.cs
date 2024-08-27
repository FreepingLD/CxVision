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
    public class ImageFilterMethod
    {
        //private static object lockState = new object();
        //private static ImageFilterMethod _Instance = null;
        public ImageFilterMethod()
        {

        }

        //public static ImageFilterMethod Instance
        //{
        //    get
        //    {
        //        if (_Instance == null)
        //        {
        //            lock (lockState)
        //            {
        //                if (_Instance == null)
        //                    _Instance = new ImageFilterMethod();
        //            }
        //        }
        //        return _Instance;
        //    }
        //}
        public HImage ImageFilter(HImage hImage, ImageFilterParam param)
        {
            HImage tarRegion = new HImage();
            switch (param.Method)
            {
                case nameof(enImageFilterMethod.bilateral_filter):
                    tarRegion = bilateral_filter(hImage, (BilateralFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.binomial_filter):
                    tarRegion = binomial_filter(hImage, (BinomialFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.gauss_filter):
                    tarRegion = gauss_filter(hImage, (GaussFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.guided_filter):
                    tarRegion = guided_filter(hImage, (GuidedFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.mean_image):
                    tarRegion = mean_image(hImage, (MeanImageFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.mean_n):
                    tarRegion = mean_n(hImage, (MeanNFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.median_image):
                    tarRegion = median_image(hImage, (MedianImageFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.median_rect):
                    tarRegion = median_rect(hImage, (MedianRectFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.median_separate):
                    tarRegion = median_separate(hImage, (MedianSeparateFilterParam)param);
                    break;
                case nameof(enImageFilterMethod.median_weighted):
                    tarRegion = median_weighted(hImage, (MedianWeightedFilterParam)param);
                    break;
            }
            return tarRegion;
        }


        private HImage bilateral_filter(HImage hImage, BilateralFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            string[] GenParamName = param.GenParamName.Split(',', ';', ':');
            string[] GenParamValue = param.GenParamValue.Split(',', ';', ':');
            if (GenParamName.Length  != GenParamValue.Length)
                throw new ArgumentNullException("GenParamName与GenParamValue长度不相等");
            tarImage = hImage;
            for (int i = 0; i < param.Count; i++)
            {
                tarImage = hImage.BilateralFilter(tarImage, param.SigmaSpatial, param.SigmaRange, param.GenParamName, param.GenParamValue);
            }
            return tarImage;
        }
        private HImage binomial_filter(HImage hImage, BinomialFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.BinomialFilter((int)param.MaskWidth, (int)param.MaskHeight);

            return tarImage;
        }
        private HImage gauss_filter(HImage hImage, GaussFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.GaussFilter((int)param.Size);

            return tarImage;
        }
        private HImage guided_filter(HImage hImage, GuidedFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage;
            for (int i = 0; i < param.Count; i++)
            {
                tarImage = hImage.GuidedFilter(tarImage, (int)param.Radius, param.Amplitude);
            }           
            return tarImage;
        }
        private HImage mean_image(HImage hImage, MeanImageFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.MeanImage((int)param.MaskWidth,(int)param.MaskHeight);

            return tarImage;
        }
        private HImage mean_n(HImage hImage, MeanNFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.MeanN();

            return tarImage;
        }
        private HImage median_image(HImage hImage, MedianImageFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.MedianImage(param.MaskType, (int)param.Radius,param.Margin);

            return tarImage;
        }
        private HImage median_rect(HImage hImage, MedianRectFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.MedianRect((int)param.MaskWidth, (int)param.MaskHeight);

            return tarImage;
        }
        private HImage median_separate(HImage hImage, MedianSeparateFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.MedianSeparate((int)param.MaskWidth, (int)param.MaskHeight,  param.Margin);

            return tarImage;
        }
        private HImage median_weighted(HImage hImage, MedianWeightedFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.MedianWeighted(param.MaskType, (int)param.MaskSize);

            return tarImage;
        }


    }

    public enum enImageFilterMethod
    {
        bilateral_filter,
        binomial_filter,
        gauss_filter,
        guided_filter,
        mean_image,
        mean_n,
        median_image,
        median_rect,
        median_separate,
        median_weighted,
        smooth_image,
    }





}
