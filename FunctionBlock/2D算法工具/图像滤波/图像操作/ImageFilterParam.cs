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
    public class ImageFilterParam
    {
        public string Method { get; set; }
    }
    [Serializable]
    public class MeanImageFilterParam : ImageFilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public MeanImageFilterParam()
        {
            MaskWidth = 9;
            MaskHeight = 9;
            Method = "mean_image";
        }
        public MeanImageFilterParam(string method)
        {
            MaskWidth = 9;
            MaskHeight = 9;
            Method = method;
        }
    }
    [Serializable]
    public class BilateralFilterParam : ImageFilterParam
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
            Method = "bilateral_filter";
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
    public class BinomialFilterParam : ImageFilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }

        public BinomialFilterParam()
        {
            MaskWidth = 3;
            MaskHeight = 3;
            Method = "binomial_filter";
        }
        public BinomialFilterParam(string method)
        {
            MaskWidth = 3;
            MaskHeight = 3;
            Method = method;
        }
    }
    [Serializable]
    public class GaussFilterParam : ImageFilterParam
    {
        public double Size { get; set; }

        public GaussFilterParam()
        {
            Size = 5;
            Method = "gauss_filter";
        }
        public GaussFilterParam(string method)
        {
            Size = 5;
            Method = method;
        }
    }
    [Serializable]
    public class MeanNFilterParam : ImageFilterParam
    {
        public MeanNFilterParam()
        {
            Method = "mean_n";
        }
        public MeanNFilterParam(string method)
        {
            Method = method;
        }
    }
    [Serializable]
    public class GuidedFilterParam : ImageFilterParam
    {
        public int Count { get; set; }
        public double Radius { get; set; }
        public double Amplitude { get; set; }
        public GuidedFilterParam()
        {
            Count = 5;
               Radius = 3;
            Amplitude = 20;
            Method = "guided_filter";
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
    public class MedianImageFilterParam : ImageFilterParam
    {
        public string MaskType { get; set; }
        public double Radius { get; set; }
        public string Margin { get; set; }
        public MedianImageFilterParam()
        {
            MaskType = "circle";
            Radius = 1;
            Margin = "mirrored";
            Method = "median_image";
        }
        public MedianImageFilterParam(string method)
        {
            MaskType = "circle";
            Radius = 1;
            Margin = "mirrored";
            Method = method;
        }
    }
    [Serializable]
    public class MedianRectFilterParam : ImageFilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public MedianRectFilterParam()
        {
            MaskWidth = 15;
            MaskHeight = 15;
            Method = "median_rect";
        }
        public MedianRectFilterParam(string method)
        {
            MaskWidth = 15;
            MaskHeight = 15;
            Method = method;
        }
    }
    [Serializable]
    public class MedianSeparateFilterParam : ImageFilterParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public string Margin { get; set; }
        
        public MedianSeparateFilterParam()
        {
            MaskWidth = 15;
            MaskHeight = 15;
            Margin = "mirrored";
            Method = "median_separate";
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
    public class MedianWeightedFilterParam : ImageFilterParam
    {
        public double MaskSize { get; set; }
        public string MaskType { get; set; }

        public MedianWeightedFilterParam()
        {
            MaskSize = 3;
            MaskType = "inner";
            Method = "median_weighted";
        }
        public MedianWeightedFilterParam(string method)
        {
            MaskSize = 3;
            MaskType = "inner";
            Method = method;
        }
    }
    [Serializable]
    public class SmoothImageParam : ImageFilterParam
    {
        public string Filter { get; set; }
        public double Alpha { get; set; }
        public SmoothImageParam()
        {
            Filter = "deriche2";
            Alpha = 0.5;
            Method = "smooth_image";
        }
        public SmoothImageParam(string method)
        {
            Filter = "deriche2";
            Alpha = 0.5;
            Method = method;
        }
    }



}
