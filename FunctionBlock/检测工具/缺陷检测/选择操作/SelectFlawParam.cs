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
    public class SelectFlawParam
    {
        public string Method { get; set; }  // 记录方法名称
        public bool IsSelect { get; set; }  // 启用滤波 
        public enFlawPolarity FlawPolarity { get; set; }
        public SelectFlawParam()
        {
            this.Method = "none";
            this.IsSelect = true;
            this.FlawPolarity = enFlawPolarity.all;
        }
        public SelectFlawParam(string method)
        {
            this.Method = method;
            this.IsSelect = true;
            this.FlawPolarity = enFlawPolarity.all;
        }

    }

    [Serializable]
    public class SelectShapeFlawParam : SelectFlawParam
    {
        public string Features { get; set; }
        public string Operation { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }

        public SelectShapeFlawParam()
        {
            this.Features = "area";
            this.Operation = "and";
            this.Min = "10";
            this.Max = "999999";
            this.Method = enFlawSelectMethod.select_shape.ToString();
        }
        public SelectShapeFlawParam(string method)
        {
            this.Features = "area";
            this.Operation = "and";
            this.Min = "10";
            this.Max = "999999";
            this.Method = method;
        }
    }

    [Serializable]
    public class SelectShapeProtoFlawParam : SelectFlawParam
    {
        public string Features { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public SelectShapeProtoFlawParam()
        {
            this.Features = "covers";
            this.Min = "50";
            this.Max = "100";
            Method = enFlawSelectMethod.select_shape_proto.ToString();
        }
        public SelectShapeProtoFlawParam(string method)
        {
            this.Features = "covers";
            this.Min = "50";
            this.Max = "100";
            Method = method;
        }
    }

    [Serializable]
    public class SelectShapeStdFlawParam : SelectFlawParam
    {
        public string Features { get; set; }
        public string Percent { get; set; }

        public SelectShapeStdFlawParam()
        {
            this.Features = "max_area";
            this.Percent = "70";
            Method = enFlawSelectMethod.select_shape_std.ToString();
        }
        public SelectShapeStdFlawParam(string method)
        {
            this.Features = "max_area";
            this.Percent = "70";
            Method = method;
        }
    }


    public enum enFlawPolarity
    {
        light,
        dark,
        all,
    }

}
