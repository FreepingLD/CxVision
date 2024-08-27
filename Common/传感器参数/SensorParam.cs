using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;

namespace Common
{
    [Serializable]
    public class SensorParam
    {
        [Browsable(false)]
        public userCamParam CamParam { get; set; } // 相机内参

        [Browsable(false)]
        public userCamPose CamPose { get; set; } // 相机外参

        //[Browsable(false)]
        public string SavePath { set; get; }

        public enAcqMode AcqMode { set; get; }

        public int AverangeCount { set; get; }

        public int AcqImageCount { set; get; }

        public int Timeout { set; get; }

        //[Category("传感器参数")]
        //[Browsable(false)]
        //[DefaultValue("Cam1")]
        public string SensorName { set; get; }

        //[Category("传感器参数")]
        //[DisplayName("数据宽")]
        //[DefaultValue(false)]
        public int DataWidth { get; set; }

        //[Category("传感器参数")]
        //[DisplayName("数据高")]
        public int DataHeight { get; set; }

        /// <summary>
        /// 宽度缩放比例
        /// </summary>
        public double ScaleWidth { get; set; }

        /// <summary>
        /// 高度缩放比例
        /// </summary>
        public double ScaleHeight { get; set; }

        /// <summary>
        /// 灰度值缩放
        /// </summary>
        public double ScaleGrayValue { get; set; }

        /// <summary>
        /// 启用缩放
        /// </summary>
        public bool EnableScale { get; set; }

        // 是否平场校正
        public bool IsFlat { set; get; }
        //[Category("采集参数")]
        //[DisplayName("X轴镜像")]
        //[DefaultValue(false)]
        public bool IsMirrorX { set; get; }

        //[Category("采集参数")]
        //[DisplayName("Y轴镜像")]
        //[DefaultValue(false)]
        public bool IsMirrorY { set; get; }

        //[Category("采集参数")]
        //[DisplayName("Z轴镜像")]
        //[DefaultValue(false)]
        public bool IsMirrorZ { set; get; }

        //[Category("采集参数")]
        //[DisplayName("到位等待时间")]
        //[DefaultValue(0)]
        public int WaiteTime { set; get; }

        //[Category("采集参数")]
        //[DefaultValue(100)]
        //[DisplayName("采集时间")]
        public int AcqWaiteTime { set; get; }

        //[Category("触发参数")]
        //[DisplayName("触发源")]
        public enUserTriggerSource TriggerSource { set; get; }

        //[Category("触发参数")]
        //[DisplayName("触发模式")]
        public enUserTrigerMode TriggerMode { set; get; }

        //[Category("触发参数")]
        //[DisplayName("IO输出类型")]
        public enIoOutputMode IoOutputMode { set; get; }

        //[Category("触发参数")]
        //[DefaultValue(0)]
        //[DisplayName("触发端口")]
        public int TriggerPort { set; get; }

        public CamSlantCalibParam SlantCalibParam { set; get; }

        public string ConfigPath { set; get; }

        public string ViewWindow { set; get; }

        public SensorParam()
        {
            this.AcqMode = enAcqMode.同步采集;
            this.AverangeCount = 1;
            this.AcqImageCount = 1;
            this.Timeout = 2000;
            this.WaiteTime = 0;
            this.AcqWaiteTime = 100;
            this.TriggerPort = 0;
            this.TriggerMode = enUserTrigerMode.NONE;
            this.TriggerSource = enUserTriggerSource.NONE;
            this.IoOutputMode = enIoOutputMode.NONE;
            this.IsMirrorX = false;
            this.IsMirrorY = false;
            this.IsMirrorZ = false;
            this.DataWidth = 1;
            this.DataHeight = 1;
            this.SensorName = "Cam1";
            this.SavePath = @"传感器参数";
            this.SlantCalibParam = new CamSlantCalibParam();
            this.ConfigPath = "";
            this.ViewWindow = "Cam1";
            this.EnableScale = false;
            this.ScaleWidth = 1;
            this.ScaleHeight = 1;
            this.ScaleGrayValue = 1;
        }




    }

    [Serializable]
    public enum enAcqMode
    {
        同步采集,
        异步采集,
        异步取图,
    }

    [Serializable]
    public class Editor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            //指定为模式窗体属性编辑器类型
            return UITypeEditorEditStyle.Modal;
        }
    }

    [Serializable]
    public class DistortParaItem
    {

        private double _widOfGrid = 0.17;
        private int _numSqueres = 17;

        private int _minContrast = 25;
        private double _radius = 10;

        private double _sigmaSaddle = 1.5;
        private int _saddleThreshold = 5;

        private double _sigmaConnectGridPoints = 0.9;
        private double _maxDist = 5.0;
        private double _gridSpacing = 20;

        private string _distortionName = "未定义";

        public HObject Map;
        /// <summary> 网格线宽度 </summary>
        public double WidOfGrid { get => _widOfGrid; set => _widOfGrid = value; }

        /// <summary>  网格数量 </summary>
        public int NumSqueres { get => _numSqueres; set => _numSqueres = value; }

        /// <summary>网格区域提取最小对比度 </summary>
        public int MinContrast
        {
            get => _minContrast;
            set
            {
                if (_saddleThreshold > 4 && _saddleThreshold < 255)
                    _minContrast = value;
            }
        }

        /// <summary> 获取网格Roi区域，膨胀半径 </summary>
        public double Radius { get => _radius; set => _radius = value; }

        /// <summary> 鞍点平滑系数 </summary>
        public double SigmaSaddle { get => _sigmaSaddle; set => _sigmaSaddle = value; }
        /// <summary>暗点提取阈值 </summary>
        public int SaddleThreshold
        {
            get => _saddleThreshold;
            set
            {
                if (_saddleThreshold > 4 && _saddleThreshold < 255)
                    _saddleThreshold = value;
            }
        }

        /// <summary>网格链接的平滑系数 </summary>
        public double SigmaConnectGridPoints { get => _sigmaConnectGridPoints; set => _sigmaConnectGridPoints = value; }

        /// <summary> 网格曲线拟合时的最大偏差距离 </summary>
        public double MaxDist
        {
            get => _maxDist;
            set
            {
                if (value > 0)
                    _maxDist = value;
            }
        }

        /// <summary> 网格空间间距 </summary>
        public double GridSpacing
        {
            get => _gridSpacing;
            set
            {
                if (_gridSpacing > 4)
                    _gridSpacing = value;
            }
        }

        /// <summary>  畸变map名字，用来保存即便地图 </summary>
        public string DistortionMapName
        {
            get => _distortionName;
            set => _distortionName = value;
        }

        public DistortParaItem()
        {
            this._widOfGrid = 0.17;
            this._numSqueres = 17;
            this._minContrast = 25;
            this._radius = 10;
            this._sigmaSaddle = 1.5;
            this._saddleThreshold = 5;

            this._sigmaConnectGridPoints = 0.9;
            this._maxDist = 5.0;
            this._gridSpacing = 20;
            this._distortionName = "未定义";
            this.Map = new HObject();
        }


    }


}
