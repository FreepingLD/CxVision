using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class GeometryFitParam
    {
        public string DataSource { get; set; }
        public string Algorithm { get; set; }
        public double MaxNumPoints { get; set; }
        public int Iterations { get; set; }
        public double ClippingFactor { get; set; }
        public double ClippingEndPoints { get; set; }
    }
    [Serializable]
    public class LineFitParam : GeometryFitParam
    {
        public LineFitParam()
        {
            DataSource = "XLD";
            Algorithm = "tukey";
            MaxNumPoints = -1;
            Iterations = 10;
            ClippingFactor = 2.0;
            ClippingEndPoints = 0;
        }

    }
    [Serializable]
    public class CircleFitParam : GeometryFitParam
    {
        public double MaxClosureDist { get; set; }
        public CircleFitParam()
        {
            DataSource = "XLD";
            Algorithm = "algebraic";
            MaxNumPoints = -1;
            Iterations = 10;
            ClippingFactor = 2.0;
            ClippingEndPoints = 0;
            MaxClosureDist = 0;
        }

    }
    [Serializable]
    public class EllipseFitParam : GeometryFitParam
    {
        public double MaxClosureDist { get; set; }
        public double VossTabSize { get; set; }
        public EllipseFitParam()
        {
            DataSource = "XLD";
            Algorithm = "fitzgibbon";
            MaxNumPoints = -1;
            Iterations = 10;
            ClippingFactor = 2.0;
            ClippingEndPoints = 0;
            MaxClosureDist = 0;
            VossTabSize = 200;
        }
    }
    [Serializable]
    public class Rect2FitParam : GeometryFitParam
    {
        public double MaxClosureDist { get; set; }
        public Rect2FitParam()
        {
            DataSource = "XLD";
            Algorithm = "tukey";
            MaxNumPoints = -1;
            Iterations = 10;
            ClippingFactor = 2.0;
            ClippingEndPoints = 0;
            MaxClosureDist = 0;
        }
    }


}
