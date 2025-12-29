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
    public class ImageEnhancementParam
    {
        public string Method { get; set; }
    }
    [Serializable]
    public class EmphasizeParam : ImageEnhancementParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public double Factor { get; set; }
        public EmphasizeParam()
        {
            MaskWidth = 9;
            MaskHeight = 9;
            Method = "emphasize";
            this.Factor = 1;
        }
        public EmphasizeParam(string method)
        {
            MaskWidth = 9;
            MaskHeight = 9;
            Method = method;
            this.Factor = 1;
        }
    }
    [Serializable]
    public class CoherenceEnhancingDiffParam : ImageEnhancementParam
    {
        public double Sigma { get; set; }
        public double Rho { get; set; }
        public double Theta { get; set; }
        public int Iterations { get; set; }

        public CoherenceEnhancingDiffParam()
        {
            Sigma = 0.5;
            Rho = 3;
            Theta = 0.5;
            Iterations = 10;
            Method = "coherence_enhancing_diff";
        }
        public CoherenceEnhancingDiffParam(string method)
        {
            Sigma = 0.5;
            Rho = 3;
            Theta = 0.5;
            Iterations = 10;
            Method = method;
        }
    }
    [Serializable]
    public class EquHistoImageParam : ImageEnhancementParam
    {
        public EquHistoImageParam()
        {
            Method = "equ_histo_image";
        }
        public EquHistoImageParam(string method)
        {
            Method = method;
        }
    }
    [Serializable]
    public class IlluminateParam : ImageEnhancementParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public double Factor { get; set; }

        public IlluminateParam()
        {
            MaskWidth = 9;
            MaskHeight = 9;
            Method = "illuminate ";
            this.Factor = 1; 
        }
        public IlluminateParam(string method)
        {
            MaskWidth = 9;
            MaskHeight = 9;
            Method = "illuminate ";
            this.Factor = 1;
        }
    }
    [Serializable]
    public class MeanCurvatureFlowParam : ImageEnhancementParam
    {
        public double Sigma { get; set; }
        public double Theta { get; set; }
        public int Iterations { get; set; }
        public MeanCurvatureFlowParam()
        {
            this.Sigma = 0.5;
            this.Theta = 0.5;
            this.Iterations = 10;
            Method = "mean_curvature_flow";
        }
        public MeanCurvatureFlowParam(string method)
        {
            this.Sigma = 0.5;
            this.Theta = 0.5;
            this.Iterations = 10;
            Method = method;
        }
    }
    [Serializable]
    public class ScaleImageMaxParam : ImageEnhancementParam
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public ScaleImageMaxParam()
        {
            this.Method = "scale_image";
            this.Min = 5;
            this.Max = 20;
        }
        public ScaleImageMaxParam(string method)
        {
            this.Method = method;
            this.Min = 5;
            this.Max = 20;
        }
    }
    [Serializable]
    public class ShockFilterParam : ImageEnhancementParam
    {
        public double Theta { get; set; }
        public double Sigma { get; set; }
        public string Mode { get; set; }
        public int Iterations { get; set; }
        public ShockFilterParam()
        {
            this.Sigma = 1.0;
            this.Theta = 0.5;
            this.Iterations = 10;
            this.Mode = "canny";
            Method = "shock_filter ";
        }
        public ShockFilterParam(string method)
        {
            this.Sigma = 1.0;
            this.Theta = 0.5;
            this.Iterations = 10;
            this.Mode = "canny";
            this.Method = method;
        }
    }


}
