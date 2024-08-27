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
    public class RoiParam
    {
        private enShapeType shapeType = enShapeType.矩形1;
        private enInsideOrOutside insideOrOutside = enInsideOrOutside.保留;

        [DisplayNameAttribute("形状类型")]
        public enShapeType ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        [DisplayNameAttribute("操作方法")]
        public enInsideOrOutside InsideOrOutside
        {
            get
            {
                return insideOrOutside;
            }

            set
            {
                insideOrOutside = value;
            }
        }

        /// <summary>
        /// 2D 中使用像素形状
        /// </summary>
        [DisplayNameAttribute("形状参数")]
        public PixROI RoiShape { get; set; }

    }


    [Serializable]
    public class SegmentParam
    {
        public bool IsFill { get; set; }
        public bool IsConnection { get; set; }
        public string SegmentMethod { get; set; }
        public string SegmentMode { get; set; }
        public userPixCoordSystem PixCoordSys { get; set; }
        public BindingList<userPixRectangle2> SegmentRegion { get; set; }
        public BindingList<RoiParam> RegionParam { get; set; }
        public SegmentParam()
        {
            this.IsFill = false;
            this.IsConnection = false;
            this.PixCoordSys = new userPixCoordSystem();
            this.SegmentRegion = new BindingList<userPixRectangle2>();
            this.RegionParam = new BindingList<RoiParam>();
            this.SegmentMode = "合并分割";
        }

    }



    [Serializable]
    public class ThresholdBlob : SegmentParam
    {
        public double MinThreshold { get; set; }
        public double MaxThreshold { get; set; }
        public string Operate { get; set; }

        public ThresholdBlob()
        {
            this.MinThreshold = 128;
            this.MaxThreshold = 255;
            this.SegmentMethod = "Threshold";
            this.Operate = "and";
        }
        public ThresholdBlob(string method)
        {
            this.MinThreshold = 128;
            this.MaxThreshold = 255;
            this.SegmentMethod = method;
            this.Operate = "and";
        }
    }
    [Serializable]
    public class AutoThresholdBlob : SegmentParam
    {
        public double AutoSigma { get; set; }

        public AutoThresholdBlob()
        {
            this.AutoSigma = 2;
            this.SegmentMethod = "AutoThreshold";
        }
        public AutoThresholdBlob(string method)
        {
            this.AutoSigma = 2;
            this.SegmentMethod = method;
        }
    }

    [Serializable]
    public class BinaryThresholdBlob : SegmentParam
    {
        public string BinaryMethod { get; set; }
        public string BinaryLightDark { get; set; }

        public BinaryThresholdBlob()
        {
            this.BinaryMethod = "max_separability";
            this.BinaryLightDark = "dark";
            this.SegmentMethod = "BinaryThreshold";
        }
        public BinaryThresholdBlob(string method)
        {
            this.BinaryMethod = "max_separability";
            this.BinaryLightDark = "dark";
            this.SegmentMethod = method;
        }
    }
    [Serializable]
    public class CharThresholdBlob : SegmentParam
    {

        public double ChartSigma { get; set; }
        public double ChartPercent { get; set; }

        public CharThresholdBlob()
        {
            this.SegmentMethod = "CharThreshold";
            this.ChartSigma = 2.0;
            this.ChartPercent = 95;
        }
        public CharThresholdBlob(string method)
        {
            this.SegmentMethod = method;
            this.ChartSigma = 2.0;
            this.ChartPercent = 95;
        }
    }
    [Serializable]
    public class DualThresholdBlob : SegmentParam
    {
        public int DualMinSize { get; set; }
        public double DualMinGray { get; set; }
        public double DuaThreshold { get; set; }

        public DualThresholdBlob()
        {
            this.DualMinSize = 20;
            this.DualMinGray = 5;
            this.DuaThreshold = 2.0;
            this.SegmentMethod = "DualThreshold";
        }
        public DualThresholdBlob(string method)
        {
            this.DualMinSize = 20;
            this.DualMinGray = 5;
            this.DuaThreshold = 2.0;
            this.SegmentMethod = method;
        }
    }
    [Serializable]
    public class DynThresholdBlob : SegmentParam
    {
        public int DynMaskWidth { get; set; }
        public int DynMaskHeight { get; set; }
        public double DynOffset { get; set; }
        public string DynLightDark { get; set; }

        public DynThresholdBlob()
        {
            this.DynMaskWidth = 40;
            this.DynMaskHeight = 40;
            this.DynOffset = 5;
            this.DynLightDark = "ight";
            this.SegmentMethod = "DynThreshold";
        }
        public DynThresholdBlob(string method)
        {
            this.DynMaskWidth = 40;
            this.DynMaskHeight = 40;
            this.DynOffset = 5;
            this.DynLightDark = "ight";
            this.SegmentMethod = method;
        }

    }
    [Serializable]
    public class FastThresholdBlob : SegmentParam
    {
        public int FastMinSize { get; set; }
        public double FastMinGray { get; set; }
        public double FastMaxGray { get; set; }

        public FastThresholdBlob()
        {
            this.FastMinSize = 20;
            this.FastMinGray = 125;
            this.FastMaxGray = 255;
            this.SegmentMethod = "FastThreshold";
        }
        public FastThresholdBlob(string method)
        {
            this.FastMinSize = 20;
            this.FastMinGray = 125;
            this.FastMaxGray = 255;
            this.SegmentMethod = method;
        }

    }
    [Serializable]
    public class HysteresisThresholdBlob : SegmentParam
    {
        public double HysteresisLow { get; set; }
        public double HysteresisHight { get; set; }
        public int HysteresisMaxLength { get; set; }

        public HysteresisThresholdBlob()
        {
            HysteresisLow = 30;
            HysteresisHight = 60;
            HysteresisMaxLength = 10;
            this.SegmentMethod = "HysteresisThreshold";
        }
        public HysteresisThresholdBlob(string method)
        {
            HysteresisLow = 30;
            HysteresisHight = 60;
            HysteresisMaxLength = 10;
            this.SegmentMethod = method;
        }
    }
    [Serializable]
    public class LocalThresholdBlob : SegmentParam
    {
        public string Method { get; set; }
        public string GenParamName { get; set; }
        public string GenParamValue { get; set; }
        public string LocalLightDark { get; set; }

        public LocalThresholdBlob()
        {
            Method = "adapted_std_deviation";
            GenParamName = "";
            GenParamValue = "";
            LocalLightDark = "dark";
            this.SegmentMethod = "LocalThreshold";
        }
        public LocalThresholdBlob(string method)
        {
            Method = "adapted_std_deviation";
            GenParamName = "";
            GenParamValue = "";
            LocalLightDark = "dark";
            this.SegmentMethod = method;
        }
    }
    [Serializable]
    public class VarThresholdBlob : SegmentParam
    {
        public int VarMaskWidth { get; set; }
        public int VarMaskHeight { get; set; }
        public double VarStdDevScale { get; set; }
        public double VarAbsThreshold { get; set; }
        public string VarLightDark { get; set; }

        public VarThresholdBlob()
        {
            VarMaskWidth = 15;
            VarMaskHeight = 15;
            VarStdDevScale = 0.2;
            VarAbsThreshold = 2;
            VarLightDark = "dark";
            SegmentMethod = "VarThreshold";
        }
        public VarThresholdBlob(string method)
        {
            VarMaskWidth = 15;
            VarMaskHeight = 15;
            VarStdDevScale = 0.2;
            VarAbsThreshold = 2;
            VarLightDark = "dark";
            SegmentMethod = method;
        }
    }
    [Serializable]
    public class WatershedsThresholdBlob : SegmentParam
    {
        public double Threshold { get; set; }

        public WatershedsThresholdBlob()
        {
            Threshold = 128;
            SegmentMethod = "WatershedsThreshold";
        }
        public WatershedsThresholdBlob(string method)
        {
            Threshold = 128;
            SegmentMethod = method;
        }
    }


    public enum enRegionSegmentMethod
    {
        Threshold,
        AutoThreshold,
        BinaryThreshold,
        CharThreshold,
        DualThreshold,
        DynThreshold,
        FastThreshold,
        HysteresisThreshold,
        LocalThreshold,
        VarThreshold,
        WatershedsThreshold,
    }


}
