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
    public class RegionOperateParam
    {
        [DisplayName("激活")]
        public bool Active { get; set; }

        [DisplayName("方法名称")]
        public enRegionMorphology Method { get; set; }

        [DisplayName("参数")]
        public RegionMorphologyParam RegionParam { get; set; }

        public RegionOperateParam()
        {
            this.Active = true;
            this.Method = enRegionMorphology.NONE;
            this.RegionParam = null;
        }
    }

    [Serializable]
    public class RegionMorphologyParam
    {
        public bool IsFill { get; set; }
        public string Method { get; set; }
        public bool IsConnection { get; set; }
        public RegionMorphologyParam()
        {
            this.IsConnection = false;
            this.Method = "NONE";
            this.IsFill = false;
        }


    }
    [Serializable]
    public class RegionCircleParam : RegionMorphologyParam
    {
        public double Radius { get; set; }
        public RegionCircleParam()
        {
            Radius = 3;
            Method = "NONE";
        }
        public RegionCircleParam(string method)
        {
            Radius = 3;
            Method = method;
        }
        public override string ToString()
        {
            return string.Join(",", Radius, this.IsFill, this.IsConnection);
        }
    }
    [Serializable]
    public class RegionRectParam : RegionMorphologyParam
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public RegionRectParam()
        {
            Width = 3;
            Height = 3;
            Method = "NONE";
        }
        public RegionRectParam(string method)
        {
            Width = 3;
            Height = 3;
            Method = method;
        }
        public override string ToString()
        {
            return string.Join(",", Width, Height, this.IsFill, this.IsConnection);
        }


    }

    [Serializable]
    public class ShapeTransParam : RegionMorphologyParam
    {
        public enShapeTransType ShapeType { get; set; }
        public ShapeTransParam()
        {
            ShapeType = enShapeTransType.convex;
            Method = "NONE";
        }
        public ShapeTransParam(string method)
        {
            ShapeType = enShapeTransType.convex;
            Method = method;
        }
        public override string ToString()
        {
            return string.Join(",", ShapeType, this.IsFill, this.IsConnection);
        }
    }

    [Serializable]
    public enum enShapeTransType
    {
        convex,
        ellipse,
        inner_center,
        inner_circle,
        inner_rectangle1,
        outer_circle,
        rectangle1,
        rectangle2
    }



}
