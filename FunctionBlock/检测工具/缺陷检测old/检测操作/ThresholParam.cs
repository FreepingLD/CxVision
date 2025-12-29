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
    public class ThresholParam
    {
        public bool IsDetect { get; set; }
        public string Method { get; set; }
        public double MinArea{ get; set; }
        public ThresholParam()
        {
            this.IsDetect = true;
            this.Method = "";
            this.MinArea = 100;
        }

    }



    [Serializable]
    public class ThresholdParam : ThresholParam
    {
        public double MinThreshold { get; set; }
        public double MaxThreshold { get; set; }
        public string Operate { get; set; }

        public ThresholdParam()
        {
            this.MinThreshold = 128;
            this.MaxThreshold = 255;
            this.Method = enFlawDetectMethod.Threshold.ToString();
            this.Operate = "and";
        }
        public ThresholdParam(string method)
        {
            this.MinThreshold = 128;
            this.MaxThreshold = 255;
            this.Method = method;
            this.Operate = "and";
        }
    }
    [Serializable]
    public class AutoThresholdParam : ThresholParam
    {
        public double AutoSigma { get; set; }

        public AutoThresholdParam()
        {
            this.AutoSigma = 2;
            this.Method = enFlawDetectMethod.AutoThreshold.ToString();
        }
        public AutoThresholdParam(string method)
        {
            this.AutoSigma = 2;
            this.Method = method;
        }
    }

    [Serializable]
    public class BinaryThresholdParam : ThresholParam
    {
        public string BinaryMethod { get; set; }
        public string BinaryLightDark { get; set; }

        public BinaryThresholdParam()
        {
            this.BinaryMethod = "max_separability";
            this.BinaryLightDark = "dark";
            this.Method = enFlawDetectMethod.BinaryThreshold.ToString();
        }
        public BinaryThresholdParam(string method)
        {
            this.BinaryMethod = "max_separability";
            this.BinaryLightDark = "dark";
            this.Method = method;
        }
    }
    [Serializable]
    public class CharThresholdParam : ThresholParam
    {

        public double ChartSigma { get; set; }
        public double ChartPercent { get; set; }

        public CharThresholdParam()
        {
            this.Method = enFlawDetectMethod.CharThreshold.ToString();
            this.ChartSigma = 2.0;
            this.ChartPercent = 95;
        }
        public CharThresholdParam(string method)
        {
            this.Method = method;
            this.ChartSigma = 2.0;
            this.ChartPercent = 95;
        }
    }
    [Serializable]
    public class DualThresholdParam : ThresholParam
    {
        public int DualMinSize { get; set; }
        public double DualMinGray { get; set; }
        public double DuaThreshold { get; set; }

        public DualThresholdParam()
        {
            this.DualMinSize = 20;
            this.DualMinGray = 5;
            this.DuaThreshold = 2.0;
            this.Method = enFlawDetectMethod.DualThreshold.ToString();
        }
        public DualThresholdParam(string method)
        {
            this.DualMinSize = 20;
            this.DualMinGray = 5;
            this.DuaThreshold = 2.0;
            this.Method = method;
        }
    }
    [Serializable]
    public class DynThresholdParam : ThresholParam
    {
        public int DynMaskWidth { get; set; }
        public int DynMaskHeight { get; set; }
        public double DynOffset { get; set; }
        public string DynLightDark { get; set; }

        public DynThresholdParam()
        {
            this.DynMaskWidth = 40;
            this.DynMaskHeight = 40;
            this.DynOffset = 5;
            this.DynLightDark = "light";
            this.Method = enFlawDetectMethod.DynThreshold.ToString();
        }
        public DynThresholdParam(string method)
        {
            this.DynMaskWidth = 40;
            this.DynMaskHeight = 40;
            this.DynOffset = 5;
            this.DynLightDark = "light";
            this.Method = method;
        }

    }
    [Serializable]
    public class FastThresholdParam : ThresholParam
    {
        public int FastMinSize { get; set; }
        public double FastMinGray { get; set; }
        public double FastMaxGray { get; set; }

        public FastThresholdParam()
        {
            this.FastMinSize = 20;
            this.FastMinGray = 125;
            this.FastMaxGray = 255;
            this.Method = enFlawDetectMethod.FastThreshold.ToString();
        }
        public FastThresholdParam(string method)
        {
            this.FastMinSize = 20;
            this.FastMinGray = 125;
            this.FastMaxGray = 255;
            this.Method = method;
        }

    }
    [Serializable]
    public class HysteresisThresholdParam : ThresholParam
    {
        public double HysteresisLow { get; set; }
        public double HysteresisHight { get; set; }
        public int HysteresisMaxLength { get; set; }

        public HysteresisThresholdParam()
        {
            HysteresisLow = 30;
            HysteresisHight = 60;
            HysteresisMaxLength = 10;
            this.Method = enFlawDetectMethod.HysteresisThreshold.ToString();
        }
        public HysteresisThresholdParam(string method)
        {
            HysteresisLow = 30;
            HysteresisHight = 60;
            HysteresisMaxLength = 10;
            this.Method = method;
        }
    }
    [Serializable]
    public class LocalThresholdParam : ThresholParam
    {
        public string GenParamName { get; set; }
        public string GenParamValue { get; set; }
        public string LocalLightDark { get; set; }

        public LocalThresholdParam()
        {
            Method = "adapted_std_deviation";
            GenParamName = "";
            GenParamValue = "";
            LocalLightDark = "dark";
            this.Method = enFlawDetectMethod.LocalThreshold.ToString();
        }
        public LocalThresholdParam(string method)
        {
            Method = "adapted_std_deviation";
            GenParamName = "";
            GenParamValue = "";
            LocalLightDark = "dark";
            this.Method = method;
        }
    }
    [Serializable]
    public class VarThresholdParam : ThresholParam
    {
        public int VarMaskWidth { get; set; }
        public int VarMaskHeight { get; set; }
        public double VarStdDevScale { get; set; }
        public double VarAbsThreshold { get; set; }
        public string VarLightDark { get; set; }

        public VarThresholdParam()
        {
            VarMaskWidth = 15;
            VarMaskHeight = 15;
            VarStdDevScale = 0.2;
            VarAbsThreshold = 2;
            VarLightDark = "dark";
            Method = enFlawDetectMethod.VarThreshold.ToString();
        }
        public VarThresholdParam(string method)
        {
            VarMaskWidth = 15;
            VarMaskHeight = 15;
            VarStdDevScale = 0.2;
            VarAbsThreshold = 2;
            VarLightDark = "dark";
            Method = method;
        }
    }
    [Serializable]
    public class WatershedsThresholdParam : ThresholParam
    {
        public double Threshold { get; set; }

        public WatershedsThresholdParam()
        {
            Threshold = 128;
            Method = enFlawDetectMethod.WatershedsThreshold.ToString();
        }
        public WatershedsThresholdParam(string method)
        {
            Threshold = 128;
            Method = method;
        }
    }

    [Serializable]
    public class CallipersInspParam : ThresholParam
    {
        public int SampleDist { get; set; }
        /// <summary>
        /// 闭运算掩膜宽度
        /// </summary>
        public int MaskWidth { get; set; }

        /// <summary>
        /// 闭运算掩膜高度
        /// </summary>
        public int MaskHeight { get; set; }

        public double Sigma { get; set; }

        public double Threshold { get; set; }


        public CallipersInspParam()
        {
            this.SampleDist = 10;
            MaskWidth = 15;
            MaskHeight = 15;
            Sigma = 1;
            Threshold = 10;
            Method = enFlawDetectMethod.CallipersInsp.ToString();
        }
        public CallipersInspParam(string method)
        {
            this.SampleDist = 10;
            MaskWidth = 15;
            MaskHeight = 15;
            Sigma = 1;
            Threshold = 10;
            Method = method;
        }
    }


    [Serializable]
    public class ScriptInspParam : ThresholParam
    {
        public string ScriptPath { get; set; }
        public enScriptType ScriptType { get; set; }
        public BindingList<InParam> InParam { get; set; }

        public BindingList<OutParam> OutParam { get; set; }

        public ScriptInspParam()
        {
            this.ScriptType = enScriptType.Halcon;
            this.ScriptPath = "";
            this.InParam = new BindingList<InParam>();
            this.OutParam = new BindingList<OutParam>();
            this.Method = enFlawDetectMethod.ScriptInsp.ToString();
        }
        public ScriptInspParam(string method)
        {
            this.ScriptType = enScriptType.Halcon;
            this.ScriptPath = "";
            this.InParam = new BindingList<InParam>();
            this.OutParam = new BindingList<OutParam>();
            this.Method = method;
        }

    }

}
