
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 用于参数的设置和保存,这个类应该是一个单实例类，在程序打开时加载一次，关闭时保存一次
    /// </summary>
    [Serializable]
    public class paramConfigBinary
    {
        private double moveSpeed = 100;
        private double scanSpeed =10;
        private double x_encoderResolution = 0.0002f;
        private double y_encoderResolution = 0.0002f;
        private double z_encoderResolution = 0.0002f;
        private double u_encoderResolution = 0.0002f;
        private double v_encoderResolution = 0.0002f;
        private double w_encoderResolution = 0.0002f;
        private bool enabeDisable_X = true;
        private bool enabeDisable_Y = true;
        private bool enabeDisable_Z = true;
        private bool enabeDisable_U = true;
        private bool enabeDisable_V = true;
        private bool enabeDisable_W = true;
        private double liftUp_Z = 10; // Z轴抬升高度
        // 对射标定坐标间隔
        private double standardThickValue = 1;
        private double cord_Gap = 0;
        // 传感器参数设置
        private string trigMode= "NONE";
        private uint treNum=1000;
        private string measureMode= "DistanceMeasure";

        // 3D显示 
        private bool isShowCoordSys = false;
        private string viewType = "true";
        private double imageWidthScale = 1.0f;

        private int stil_LedBrightness=50;
        private int stil_ExposureTime=500;
        private double stil_DetectionThreshold =0.015f;
        private string stil_AltitudeModePeak= "StrongPeak";
        private string stil_ThickModePeak1= "FirstPeak";
        private string stil_ThickModePeak2= "SecondPeak";
        private double stil_PointPitch = 0.022f;
        private double stil_MeasureRange = 100f;


        // 可视化参数
        private int nodeSize = 20;
        private int pointSize = 20;
        // 传感器参数设置
        public double ScanSpeed
        {
            get
            {
                return scanSpeed;
            }

            set
            {
                scanSpeed = value;
            }
        }
        public double X_encoderResolution
        {
            get
            {
                return x_encoderResolution;
            }

            set
            {
                x_encoderResolution = value;
            }
        }
        public double Y_encoderResolution
        {
            get
            {
                return y_encoderResolution;
            }

            set
            {
                y_encoderResolution = value;
            }
        }
        public double Z_encoderResolution
        {
            get
            {
                return z_encoderResolution;
            }

            set
            {
                z_encoderResolution = value;
            }
        }
        public double U_encoderResolution
        {
            get
            {
                return u_encoderResolution;
            }

            set
            {
                u_encoderResolution = value;
            }
        }
        public double V_encoderResolution
        {
            get
            {
                return v_encoderResolution;
            }

            set
            {
                v_encoderResolution = value;
            }
        }
        public double W_encoderResolution
        {
            get
            {
                return w_encoderResolution;
            }

            set
            {
                w_encoderResolution = value;
            }
        }
        public bool EnabeDisable_X
        {
            get
            {
                return enabeDisable_X;
            }

            set
            {
                enabeDisable_X = value;
            }
        }
        public bool EnabeDisable_Y
        {
            get
            {
                return enabeDisable_Y;
            }

            set
            {
                enabeDisable_Y = value;
            }
        }
        public bool EnabeDisable_Z
        {
            get
            {
                return enabeDisable_Z;
            }

            set
            {
                enabeDisable_Z = value;
            }
        }
        public bool EnabeDisable_U
        {
            get
            {
                return enabeDisable_U;
            }

            set
            {
                enabeDisable_U = value;
            }
        }
        public bool EnabeDisable_V
        {
            get
            {
                return enabeDisable_V;
            }

            set
            {
                enabeDisable_V = value;
            }
        }
        public bool EnabeDisable_W
        {
            get
            {
                return enabeDisable_W;
            }

            set
            {
                enabeDisable_W = value;
            }
        }
        public double LiftUp_Z
        {
            get
            {
                return liftUp_Z;
            }

            set
            {
                liftUp_Z = value;
            }
        }
        public string TrigMode
        {
            get
            {
                return trigMode;
            }

            set
            {
                trigMode = value;
            }
        }
        public uint TreNum
        {
            get
            {
                return treNum;
            }

            set
            {
                treNum = value;
            }
        }
        public string MeasureMode
        {
            get
            {
                return measureMode;
            }

            set
            {
                measureMode = value;
            }
        }
        public bool IsShowCoordSys
        {
            get
            {
                return isShowCoordSys;
            }

            set
            {
                isShowCoordSys = value;
            }
        }
        public string ViewType
        {
            get
            {
                return viewType;
            }

            set
            {
                viewType = value;
            }
        }
        public double ImageWidthScale
        {
            get
            {
                return imageWidthScale;
            }

            set
            {
                imageWidthScale = value;
            }
        }
        public double Cord_Gap
        {
            get
            {
                return cord_Gap;
            }

            set
            {
                cord_Gap = value;
            }
        }
        public int Stil_LedBrightness
        {
            get
            {
                return stil_LedBrightness;
            }

            set
            {
                stil_LedBrightness = value;
            }
        }
        public int Stil_ExposureTime
        {
            get
            {
                return stil_ExposureTime;
            }

            set
            {
                stil_ExposureTime = value;
            }
        }
        public double Stil_DetectionThreshold
        {
            get
            {
                return stil_DetectionThreshold;
            }

            set
            {
                stil_DetectionThreshold = value;
            }
        }
        public string Stil_AltitudeModePeak
        {
            get
            {
                return stil_AltitudeModePeak;
            }

            set
            {
                stil_AltitudeModePeak = value;
            }
        }
        public string Stil_ThickModePeak1
        {
            get
            {
                return stil_ThickModePeak1;
            }

            set
            {
                stil_ThickModePeak1 = value;
            }
        }
        public string Stil_ThickModePeak2
        {
            get
            {
                return stil_ThickModePeak2;
            }

            set
            {
                stil_ThickModePeak2 = value;
            }
        }
        public double Stil_PointPitch
        {
            get
            {
                return stil_PointPitch;
            }

            set
            {
                stil_PointPitch = value;
            }
        }
        public double Stil_MeasureRange
        {
            get
            {
                return stil_MeasureRange;
            }

            set
            {
                stil_MeasureRange = value;
            }
        }
        public double MoveSpeed
        {
            get
            {
                return moveSpeed;
            }

            set
            {
                moveSpeed = value;
            }
        }
        public double StandardThickValue
        {
            get
            {
                return standardThickValue;
            }

            set
            {
                standardThickValue = value;
            }
        }

        public int NodeSize
        {
            get
            {
                return nodeSize;
            }

            set
            {
                nodeSize = value;
            }
        }

        public int PointSize
        {
            get
            {
                return pointSize;
            }

            set
            {
                pointSize = value;
            }
        }
    }


}
