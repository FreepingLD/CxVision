using Sensor;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.IO;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光扫描采集的数据类
    /// </summary>
    public class AreaScanDivisionParam 
    {
        private string method = "telecentric_planar";
        private double threshold = 128;
        private double circleDist = 2;
        private double circleRadius = 0.5;
        private double axis_length = 1;
        private userPixRectangle2 rect2 = new userPixRectangle2();
        private int rowCount = 5;
        private int colCount = 5;
        private enCameraModel cameraModel = enCameraModel.area_scan_telecentric_division;
        private double x_offset = 0;
        private double y_offset = 0;
        private enCamCalibrateType camCalibrateType = enCamCalibrateType.单相机移动标定;
        private userCamParam camParam;




        public enCameraModel CameraType
        {
            get
            {
                return cameraModel;
            }

            set
            {
                cameraModel = value;
            }
        }
        public int RowCount
        {
            get
            {
                return rowCount;
            }

            set
            {
                rowCount = value;
            }
        }
        public int ColCount
        {
            get
            {
                return colCount;
            }

            set
            {
                colCount = value;
            }
        }
        public userPixRectangle2 Rect2
        {
            get
            {
                return rect2;
            }

            set
            {
                rect2 = value;
            }
        }
        public double CircleDist
        {
            get
            {
                return circleDist;
            }

            set
            {
                circleDist = value;
            }
        }
        public double Threshold
        {
            get
            {
                return threshold;
            }

            set
            {
                threshold = value;
            }
        }
        public string Method
        {
            get
            {
                return method;
            }

            set
            {
                method = value;
            }
        }
        public double Axis_length
        {
            get
            {
                return axis_length;
            }

            set
            {
                axis_length = value;
            }
        }

        public double X_offset { get => x_offset; set => x_offset = value; }
        public double Y_offset { get => y_offset; set => y_offset = value; }
        public enCamCalibrateType CamCalibrateType { get => camCalibrateType; set => camCalibrateType = value; }
        public double CircleRadius { get => circleRadius; set => circleRadius = value; }
        public userCamParam CamParam { get => camParam; set => camParam = value; }

        public AreaScanDivisionParam()
        {

        }


    }
}
