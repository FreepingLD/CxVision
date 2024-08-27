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
    public class SelectOperateParam
    {
        [DisplayName("激活")]
        public bool IsActive { get; set; }

        [DisplayName("方法名称")]
        public enSelectRegionMethod Method { get; set; }

        [DisplayName("参数")]
        public SelectParamValue SelectParam { get; set; }

        [DisplayName("合并区域")]
        public bool IsUnion { get; set; }

        public SelectOperateParam()
        {
            this.IsActive = true;
            this.IsUnion = false;
        }

    }

    [Serializable]
    public class SelectParamValue
    {
        [DisplayName("特征名称")]
        public enSelectShapeFeatures Features { get; set; }

        [DisplayName("操作")]
        public enOperation Operation { get; set; }

    }
    [Serializable]
    public class SelectConnectParam : SelectParamValue
    {

        [DisplayName("合并区域")]
        public bool IsConnect { get; set; }

        [DisplayName("合并距离")]
        public double ConnectDist { get; set; }

        [DisplayName("合并数量")]
        public int ConnectCount { get; set; }
        [DisplayName("最小面积")]
        public int MinArea { get; set; }

        [DisplayName("最大面积")]
        public int MaxArea { get; set; }
        public SelectConnectParam()
        {
            this.IsConnect = false;
            this.ConnectDist = 1;
            this.ConnectCount = 1;
            this.MinArea = 10;
            this.MaxArea = 100;
        }

    }
    [Serializable]
    public class IntensityParam : SelectParamValue
    {
        [DisplayName("特征名称")]
        public enSelectIntensityFeatures IntensityFeatures { get; set; }

        [DisplayName("平均灰度")]
        public double Mean { get; set; }

        [DisplayName("标准差")]
        public double Deviation { get; set; }

        public IntensityParam()
        {
            this.IntensityFeatures = enSelectIntensityFeatures.NONE;
            this.Mean = 10;
            this.Deviation = 1;
        }
    }

    [Serializable]
    public class SelectRegionDistParam : SelectParamValue
    {
        public double Dist { get; set; }
        public double Count { get; set; }

        public SelectRegionDistParam()
        {
            this.Dist = 1;
            this.Count = 1;
        }
        public SelectRegionDistParam(string method)
        {
            this.Dist = 1;
            this.Count = 1;
        }

        public override string ToString()
        {
            return string.Join(",", this.Dist, this.Count);
        }
    }

    [Serializable]
    public class SelectRegionPointParam : SelectParamValue
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public SelectRegionPointParam()
        {
            this.Row = 100;
            this.Col = 100;
        }
        public SelectRegionPointParam(string method)
        {
            this.Row = 100;
            this.Col = 100;
        }

        public override string ToString()
        {
            return string.Join(",", this.Row, this.Col);
        }
    }
    [Serializable]
    public class SelectShapeParam : SelectParamValue
    {
        [DisplayName("最小值")]
        public string Min { get; set; }

        [DisplayName("最大值")]
        public string Max { get; set; }

        public SelectShapeParam()
        {
            this.Min = "100";
            this.Max = "999999999";
            this.Features = enSelectShapeFeatures.area;
            this.Operation = enOperation.and;
        }
        public SelectShapeParam(string method)
        {
            this.Min = "100";
            this.Max = "999999999";
            this.Features = enSelectShapeFeatures.area;
            this.Operation = enOperation.and;
        }

        public override string ToString()
        {
            return string.Join(",", this.Features, this.Operation, this.Max, this.Min);
        }
    }
    [Serializable]
    public class SelectShapeStdParam : SelectParamValue
    {
        [DisplayName("特征名")]
        public enSelectStdFeatures StdFeatures { get; set; }

        [DisplayName("百分比")]
        public double Percent { get; set; }

        public SelectShapeStdParam()
        {
            this.StdFeatures = enSelectStdFeatures.max_area;
            this.Percent = 70;
        }
        public SelectShapeStdParam(string method)
        {
            this.StdFeatures = enSelectStdFeatures.max_area;
            this.Percent = 70;
        }
        public override string ToString()
        {
            return string.Join(",", this.StdFeatures, this.Percent);
        }


    }

    [Serializable]
    public enum enSelectRegionMethod
    {
        NONE,
        select_region_point,
        select_shape,
        select_shape_std,
        intensity_deviation,
        select_connect_region,
    }

    [Serializable]
    public enum enOperation
    {
        and,
        or,
    }

    [Serializable]
    public enum enSelectShapeFeatures
    {
        none,
        area,
        row,
        column,
        width,
        height,
        ratio,
        row1,
        column1,
        row2,
        column2,
        circularity,
        compactness,
        contlength,
        convexity,
        rectangularity,
        ra,
        rb,
        phi,
        anisometry,
        bulkiness,
        struct_factor,
        outer_radius,
        inner_radius,
        inner_width,
        inner_height,
        dist_mean,
        dist_deviation,
        roundness,
        num_sides,
        connect_num,
        holes_num,
        area_holes,
        max_diameter,
        orientation,
        euler_number,
        rect2_phi,
        rect2_len1,
        rect2_len2,
        SelectStdFeatures,
    }

    [Serializable]
    public enum enSelectStdFeatures
    {
        none,
        max_area,
        rectangle1,
        rectangle2,
    }

    [Serializable]
    public enum enSelectIntensityFeatures
    {
        NONE,
        Mean,
        Deviation,
        Mean_Deviation,
    }

}
