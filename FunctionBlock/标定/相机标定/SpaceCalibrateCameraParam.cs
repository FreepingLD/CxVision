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
    public class SpaceCalibrateCameraParam : INotifyPropertyChanged
    {
        private HTuple calibDataID;
        private double calibrateObjectThick = 1;
        private enCalibrationSetupType calibrationSetupType = enCalibrationSetupType.calibration_object;
        private enCameraModel cameraType = enCameraModel.area_scan_division;
        private double pixWidth = 3.45;
        private double pixHeight = 3.45;
        private double focus = 8;
        private double kappa;
        private double tilt;
        private double rot;
        private string calibObjDescr = "caltab_30mm.descr";
        private HTuple camParam;
        private List<HImage> listImage = new List<HImage>();
        private double imageWidth = 2048;
        private double imageHeight = 1536;
        /// /////////////
        private double K1;
        private double K2;
        private double K3;
        private double P1;
        private double P2;

        public event PropertyChangedEventHandler PropertyChanged;

        public double CalibrateObjectThick
        {
            get
            {
                return calibrateObjectThick;
            }

            set
            {
                calibrateObjectThick = value;
            }
        }
        public enCameraModel CameraType
        {
            get
            {
                return cameraType;
            }

            set
            {
                cameraType = value;
                InitcamParam();
                HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }
        public double PixWidth
        {
            get
            {
                return pixWidth;
            }

            set
            {
                pixWidth = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PixWidth"));
                InitcamParam();
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }
        public double PixHeight
        {
            get
            {
                return pixHeight;
            }

            set
            {
                pixHeight = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PixHeight"));
                InitcamParam();
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }
        public double Focus
        {
            get
            {
                return focus;
            }

            set
            {
                focus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Focus"));
                InitcamParam();
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }
        public enCalibrationSetupType CalibrationSetupType
        {
            get
            {
                return calibrationSetupType;
            }

            set
            {
                calibrationSetupType = value;
            }
        }
        public string CalibObjDescr
        {
            get
            {
                return calibObjDescr;
            }

            set
            {
                calibObjDescr = value;
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCalibObject(this.calibDataID, 0, value);
            }
        }
        public List<HImage> ListImage
        {
            get
            {
                return listImage;
            }

            set
            {
                listImage = value;
            }
        }
        public double Tilt
        {
            get
            {
                return tilt;
            }

            set
            {
                tilt = value;
                InitcamParam();
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }
        public double Rot
        {
            get
            {
                return rot;
            }

            set
            {
                rot = value;
                InitcamParam();
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }
        public double ImageWidth
        {
            get
            {
                return imageWidth;
            }

            set
            {
                imageWidth = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ImageWidth"));
                InitcamParam();
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }
        public double ImageHeight
        {
            get
            {
                return imageHeight;
            }

            set
            {
                imageHeight = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ImageHeight"));
                InitcamParam();
                if (this.calibDataID != null)
                    HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, cameraType.ToString(), this.camParam);
            }
        }


        public SpaceCalibrateCameraParam()
        {
            InitcamParam();
            InitCalibDataModel();
        }

        private void InitcamParam()
        {
            switch (cameraType)
            {
                case enCameraModel.area_scan_division:
                    this.camParam = new HTuple(focus, kappa, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_polynomial:
                    this.camParam = new HTuple(focus, K1, K2, K3, P1, P2, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_tilt_division:
                    this.camParam = new HTuple(focus, kappa, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_tilt_polynomial:
                    this.camParam = new HTuple(focus, K1, K2, K3, P1, P2, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_telecentric_division:
                    focus = 0;
                    this.camParam = new HTuple(focus, kappa, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_telecentric_polynomial:
                    focus = 0;
                    this.camParam = new HTuple(focus, K1, K2, K3, P1, P2, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_telecentric_tilt_division:
                    focus = 0;
                    this.camParam = new HTuple(focus, kappa, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_telecentric_tilt_polynomial:
                    focus = 0;
                    this.camParam = new HTuple(focus, K1, K2, K3, P1, P2, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
            }
            //HOperatorSet.ResetObjDb(imageWidth, imageHeight, 0); // 一定要这个语句来初始化系统
        }
        private void InitCalibDataModel()
        {
            HOperatorSet.CreateCalibData(enCalibrationSetupType.calibration_object.ToString(), 1, 1, out this.calibDataID);
            HOperatorSet.SetCalibDataCamParam(this.calibDataID, 0, this.cameraType.ToString(), this.camParam); // 焦距跟类型比较
            HOperatorSet.SetCalibDataCalibObject(this.calibDataID, 0, this.calibObjDescr);
        }

        public void FindCalibObject(HImage image, HWindow window)
        {
            HTuple width, height;
            HTuple row, column, index, pose, CamPar;
            HObject cross;
            double ArrowLength = 0.02;
            HTuple ArrowX_CPCS = new HTuple(0, ArrowLength, 0);
            HTuple ArrowY_CPCS = new HTuple(0, 0, ArrowLength);
            HTuple ArrowZ_CPCS = new HTuple(0, 0, 0);
            HalconLibrary ha = new HalconLibrary();
            ////////////////
            HOperatorSet.GetImageSize(image, out width, out height);
            if (width.D != this.imageWidth || height.D != this.imageHeight)
            {
                this.ImageWidth = width.D;
                this.ImageHeight = height.D;
            }
            HOperatorSet.FindCalibObject(image, this.calibDataID, 0, 0, 0, new HTuple("sigma", "alpha"), new HTuple(1.0, 1.0));
            HOperatorSet.GetCalibDataObservPoints(this.calibDataID, 0, 0, 0, out row, out column, out index, out pose);
            HOperatorSet.GetCalibData(this.calibDataID, "camera", 0, "init_params", out CamPar);
            HOperatorSet.GenCrossContourXld(out cross, row, column, 0.5, 0);
            window.SetColor("red");
            //window.ClearWindow();
            //window.DispObj(image);
            window.DispObj(cross);
            ha.Gen3DCoordSystem(window, CamPar, pose, 0.02);
        }
        public void CalibrateCamera(HWindow window, out HTuple camParam, out HTuple Error)
        {
            camParam = null;
            Error = null;
            ////////////////
            HTuple width, height;
            HTuple row, column, index, pose, CamPar;
            HObject cross;
            HalconLibrary ha = new HalconLibrary();
            if (this.ListImage.Count > 0)
            {
                HOperatorSet.GetImageSize(this.ListImage[0], out width, out height);
                if (width.D != this.imageWidth || height.D != this.imageHeight)
                {
                    this.ImageWidth = width.D; // 改变图像后一定要重设一次参数
                    this.ImageHeight = height.D;
                }
            }
            //////////////////////////////////////////
            for (int i = 0; i < this.ListImage.Count; i++)
            {
                try
                {
                    HOperatorSet.FindCalibObject(ListImage[i], this.calibDataID, 0, 0, i, new HTuple(), new HTuple());
                    HOperatorSet.GetCalibDataObservPoints(this.calibDataID, 0, 0, i, out row, out column, out index, out pose);
                    HOperatorSet.GetCalibData(this.calibDataID, "camera", 0, "init_params", out CamPar);
                    HOperatorSet.GenCrossContourXld(out cross, row, column, 0.5, 0);
                    window.SetColor("red");
                    window.ClearWindow();
                    window.DispObj(this.ListImage[i]);
                    window.DispObj(cross);
                    ha.Gen3DCoordSystem(window, CamPar, pose, 0.02);
                }
                catch (Exception e)
                {
                    MessageBox.Show(i.ToString() + e.ToString());
                }
            }
            HOperatorSet.CalibrateCameras(this.calibDataID, out Error);
            HOperatorSet.GetCalibData(this.calibDataID, "camera", 0, "params", out camParam);

        }
        public void AddImage(HImage image)
        {
            if (image != null)
                this.ListImage.Add(image);
        }
        public void ClearHandle()
        {
            if (this.calibDataID != null)
                HOperatorSet.ClearCalibData(this.calibDataID);
        }



    }
}
