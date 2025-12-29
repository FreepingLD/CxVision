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
    public class FilterMethod
    {

        public FilterMethod()
        {

        }

        public HImage ImageFilter(HImage hImage, FilterParam param)
        {
            HImage tarHImage = new HImage();
            switch (param.Method)
            {
                case nameof(enDetectFilterMethod.bilateral_filter):
                    tarHImage = bilateral_filter(hImage, (BilateralFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.binomial_filter):
                    tarHImage = binomial_filter(hImage, (BinomialFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.gauss_filter):
                    tarHImage = gauss_filter(hImage, (GaussFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.guided_filter):
                    tarHImage = guided_filter(hImage, (GuidedFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.mean_image):
                    tarHImage = mean_image(hImage, (MeanFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.mean_n):
                    tarHImage = mean_n(hImage, (MeanNFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.median_image):
                    tarHImage = median_image(hImage, (MedianFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.median_rect):
                    tarHImage = median_rect(hImage, (MedianRectFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.median_separate):
                    tarHImage = median_separate(hImage, (MedianSeparateFilterParam)param);
                    break;
                case nameof(enDetectFilterMethod.median_weighted):
                    tarHImage = median_weighted(hImage, (MedianWeightedFilterParam)param);
                    break;
                default:
                    tarHImage = hImage.Clone();
                    break;
            }
            ////////////////////////////////
            return tarHImage;
        }


        private HImage bilateral_filter(HImage hImage, BilateralFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            string[] GenParamName = param.GenParamName.Split(',', ';', ':');
            string[] GenParamValue = param.GenParamValue.Split(',', ';', ':');
            if (GenParamName.Length  != GenParamValue.Length)
                throw new ArgumentNullException("GenParamName与GenParamValue长度不相等");
            tarImage = hImage;
            for (int i = 0; i < param.Count; i++)
            {
                tarImage = hImage.BilateralFilter(tarImage, param.SigmaSpatial, param.SigmaRange, param.GenParamName, param.GenParamValue);
            }
            ////////////////////////////////
            return tarImage;
        }
        private HImage binomial_filter(HImage hImage, BinomialFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.BinomialFilter((int)param.MaskWidth, (int)param.MaskHeight);
            ////////////////////////////////
            return tarImage;
        }
        private HImage gauss_filter(HImage hImage, GaussFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.GaussFilter((int)param.Size);
            ////////////////////////////////
            return tarImage;
        }
        private HImage guided_filter(HImage hImage, GuidedFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage;
            for (int i = 0; i < param.Count; i++)
            {
                tarImage = hImage.GuidedFilter(tarImage, (int)param.Radius, param.Amplitude);
            }
            ////////////////////////////////
            return tarImage;
        }
        private HImage mean_image(HImage hImage, MeanFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.MeanImage((int)param.MaskWidth,(int)param.MaskHeight);
            ////////////////////////////////
            return tarImage;
        }
        private HImage mean_n(HImage hImage, MeanNFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.MeanN();
            ////////////////////////////////
            return tarImage;
        }
        private HImage median_image(HImage hImage, MedianFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.MedianImage(param.MaskType, (int)param.Radius,param.Margin);
            ////////////////////////////////
            return tarImage;
        }
        private HImage median_rect(HImage hImage, MedianRectFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.MedianRect((int)param.MaskWidth, (int)param.MaskHeight);
            ////////////////////////////////
            return tarImage;
        }
        private HImage median_separate(HImage hImage, MedianSeparateFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.MedianSeparate((int)param.MaskWidth, (int)param.MaskHeight,  param.Margin);
            ////////////////////////////////
            return tarImage;
        }
        private HImage median_weighted(HImage hImage, MedianWeightedFilterParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            tarImage = hImage.MedianWeighted(param.MaskType, (int)param.MaskSize);
            ////////////////////////////////
            return tarImage;
        }


    }

    public enum enDetectFilterMethod
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
        convol_image
    }


}
