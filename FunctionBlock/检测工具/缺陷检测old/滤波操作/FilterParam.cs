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
    public class FilterParam
    {
        public string Method { get; set; }  // 记录方法名称
        public bool IsFilter { get; set; }  // 启用滤波 
        public FilterParam()
        {
            this.IsFilter = true;
        }
    }

    [Serializable]
    public class MeanFilterParam : FilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public MeanFilterParam()
        {
            this.MaskWidth = 9;
            this.MaskHeight = 9;
            this.Method = enDetectFilterMethod.mean_image.ToString();
        }
        public MeanFilterParam(string method)
        {
            this.MaskWidth = 9;
            this.MaskHeight = 9;
            this.Method = method;
        }
    }

    [Serializable]
    public class BilateralFilterParam : FilterParam
    {
        public double SigmaSpatial { get; set; }
        public double SigmaRange { get; set; }

        public int Count { get; set; }
        public string GenParamName { get; set; }
        public string GenParamValue { get; set; }

        public BilateralFilterParam()
        {
            SigmaSpatial = 3;
            SigmaRange = 20;
            Count = 5;
            GenParamName = "";
            GenParamValue = "";
            Method = enDetectFilterMethod.bilateral_filter.ToString();
        }
        public BilateralFilterParam(string method)
        {
            SigmaSpatial = 3;
            SigmaRange = 20;
            Count = 5;
            GenParamName = "";
            GenParamValue = "";
            Method = method;
        }
    }

    [Serializable]
    public class BinomialFilterParam : FilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }

        public BinomialFilterParam()
        {
            MaskWidth = 3;
            MaskHeight = 3;
            Method = enDetectFilterMethod.binomial_filter.ToString();
        }
        public BinomialFilterParam(string method)
        {
            MaskWidth = 3;
            MaskHeight = 3;
            Method = method;
        }
    }

    [Serializable]
    public class GaussFilterParam : FilterParam
    {
        public double Size { get; set; }

        public GaussFilterParam()
        {
            Size = 5;
            Method = enDetectFilterMethod.gauss_filter.ToString();
        }
        public GaussFilterParam(string method)
        {
            Size = 5;
            Method = method;
        }
    }

    [Serializable]
    public class MeanNFilterParam : FilterParam
    {
        public MeanNFilterParam()
        {
            Method = enDetectFilterMethod.mean_n.ToString();
        }
        public MeanNFilterParam(string method)
        {
            Method = method;
        }
    }

    [Serializable]
    public class GuidedFilterParam : FilterParam
    {
        public int Count { get; set; }
        public double Radius { get; set; }
        public double Amplitude { get; set; }
        public GuidedFilterParam()
        {
            Count = 5;
            Radius = 3;
            Amplitude = 20;
            Method = enDetectFilterMethod.guided_filter.ToString();
        }
        public GuidedFilterParam(string method)
        {
            Count = 5;
            Radius = 3;
            Amplitude = 20;
            Method = method;
        }
    }

    [Serializable]
    public class MedianFilterParam : FilterParam
    {
        public string MaskType { get; set; }
        public double Radius { get; set; }
        public string Margin { get; set; }
        public MedianFilterParam()
        {
            MaskType = "circle";
            Radius = 1;
            Margin = "mirrored";
            Method = enDetectFilterMethod.median_image.ToString();
        }
        public MedianFilterParam(string method)
        {
            MaskType = "circle";
            Radius = 1;
            Margin = "mirrored";
            Method = method;
        }
    }

    [Serializable]
    public class MedianRectFilterParam : FilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public MedianRectFilterParam()
        {
            MaskWidth = 15;
            MaskHeight = 15;
            Method = enDetectFilterMethod.median_rect.ToString();
        }
        public MedianRectFilterParam(string method)
        {
            MaskWidth = 15;
            MaskHeight = 15;
            Method = method;
        }
    }

    [Serializable]
    public class MedianSeparateFilterParam : FilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public string Margin { get; set; }

        public MedianSeparateFilterParam()
        {
            MaskWidth = 15;
            MaskHeight = 15;
            Margin = "mirrored";
            Method = enDetectFilterMethod.median_separate.ToString();
        }
        public MedianSeparateFilterParam(string method)
        {
            MaskWidth = 15;
            MaskHeight = 15;
            Margin = "mirrored";
            Method = method;
        }
    }

    [Serializable]
    public class MedianWeightedFilterParam : FilterParam
    {
        public double MaskSize { get; set; }
        public string MaskType { get; set; }

        public MedianWeightedFilterParam()
        {
            MaskSize = 3;
            MaskType = "inner";
            Method = enDetectFilterMethod.median_weighted.ToString();
        }
        public MedianWeightedFilterParam(string method)
        {
            MaskSize = 3;
            MaskType = "inner";
            Method = method;
        }
    }

    [Serializable]
    public class SmoothFilterParam : FilterParam
    {
        public string Filter { get; set; }
        public double Alpha { get; set; }
        public SmoothFilterParam()
        {
            Filter = "deriche2";
            Alpha = 0.5;
            Method = enDetectFilterMethod.smooth_image.ToString();
        }
        public SmoothFilterParam(string method)
        {
            Filter = "deriche2";
            Alpha = 0.5;
            Method = method;
        }
    }


    [Serializable]
    public class ConvolFilterParam : FilterParam
    {
        public string FilterMask { get; set; }
        public string Margin { get; set; }
        public ConvolFilterParam()
        {
            FilterMask = "sobel";
            Margin = "mirrored";
            Method = enDetectFilterMethod.convol_image.ToString();
        }
        public ConvolFilterParam(string method)
        {
            FilterMask = "sobel";
            Margin = "mirrored";
            Method = method;
        }
    }

}
