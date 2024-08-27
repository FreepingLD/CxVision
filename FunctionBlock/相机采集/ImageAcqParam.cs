using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Windows.Forms;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using Sensor;
using System.IO;
using System.Linq;

namespace FunctionBlock
{
    /// <summary>
    /// 配置成单实例类
    /// </summary>
    [Serializable]
    public class ImageAcqParam
    {
        private CameraParam camParam;
        public List<string> FilePath { get; set; }
        public string SingleFilePath { get; set; }
        public string FolderPath { get; set; }
        public bool IsCamSource { get; set; }
        public bool IsFileSource { get; set; }
        public userWcsVector GrabPoint { get; set; }
        public bool SrartImageAcq { get; set; }
        public bool StopImageAcq { get; set; }
        public string ViewWindow { get; set; }
        public enCoordSysName CoordSysName { get; set; }


        public ImageAcqParam()
        {
            this.IsCamSource = true;
            this.IsFileSource = false;
            this.FilePath = new List<string>();
            this.SingleFilePath = "";
            this.FolderPath = "";
            this.GrabPoint = new userWcsVector(100, 100, 0, 0);
            this.SrartImageAcq = false;
            this.StopImageAcq = false;
            this.ViewWindow = "NONE";
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }

        public ImageDataClass ReadImage(string path, string acqSourceName)
        {
            if (File.Exists(path))
            {
                HImage image = new HImage(path);
                string[] name = SensorManage.GetSensorName();
                foreach (var item in name)
                {
                    if (acqSourceName != null && acqSourceName.Contains(item))
                    {
                        this.camParam = SensorManage.GetSensor(item).CameraParam;
                    }
                }
                if (this.camParam == null || this.camParam.SensorName == "NONE")
                    InitCamParam(image);
                return new ImageDataClass(image, this.camParam);
            }
            return null;
        }

        private void InitCamParam(HImage image)
        {
            int width, height;
            HTuple x, y;
            if (image == null)
            {
                this.camParam = new CameraParam();
            }
            else
            {
                image.GetImageSize(out width, out height);
                userCamParam camParam;
                userCamPose camPose;
                double Quality;
                UserHomMat2D homMat2D = new UserHomMat2D(true);
                this.camParam = new CameraParam();
                NinePointCalibParam.HHomMat2DToCamparaCamPos(homMat2D.GetHHomMat(), width, height, 0.5, out camParam, out camPose, out Quality);
                this.camParam.HomMat2D = homMat2D;
                this.camParam.CamParam = camParam;
                this.camParam.CamPose = camPose;
            }
        }





    }
}
