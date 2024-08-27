using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;

namespace Sensor
{
    public class SensorBase
    {
        protected CancellationTokenSource cts;
        protected bool isSaveImage = false; // 用于控制图像采集
        protected bool streamProcessState = false;　// 用于表示流数据的处理状态 
        protected ImageDataClass imageData = null;
        protected List<HImage> listImage = new List<HImage>();
        private string _name;
        private CameraParam cameraParam;
        private LaserParam laserParam;
        protected object _lockState = new object();
        protected string _MapName = "";
        private SensorConnectConfigParam configParam;
        protected HImage _grabImage;
        protected HImage _grabDarkImage;
        public SensorConnectConfigParam ConfigParam { get => configParam; set => configParam = value; }
        public string Name { get => _name; set => _name = value; }
        public CameraParam CameraParam { get => cameraParam; set => cameraParam = value; }
        public LaserParam LaserParam { get => laserParam; set => laserParam = value; }
        public HImage GrabImage { get => _grabImage; set => _grabImage = value; }
        public HImage GrabDarkImage { get => _grabDarkImage; set => _grabDarkImage = value; }

        public event PointCloudAcqCompleteEventHandler PointsCloudAcqComplete;
        public event ImageAcqCompleteEventHandler ImageAcqComplete;
        private bool PointCloudSendState = false, ImageSendState = false;


        public void OnImageAcqComplete(string sensorName, ImageDataClass imageData)
        {
            //if (this.ImageSendState) return; // 没处理完上一个返回
            if (ImageAcqComplete != null)
            {
                this.ImageSendState = true;
                ImageAcqComplete.Invoke(this, new ImageAcqCompleteEventArgs(sensorName, imageData));
            }
        }
        public void OnPointCloudAcqComplete(string sensorName, HObjectModel3D hObjectModel3D)
        {
            //if (this.PointCloudSendState) return; // 没处理完上一个返回
            if (PointsCloudAcqComplete != null)
            {
                this.PointCloudSendState = true;
                PointsCloudAcqComplete.Invoke(this, new PointCloudAcqCompleteEventArgs(sensorName, hObjectModel3D));
            }
        }
        public void OnPointCloudAcqComplete(string sensorName, double[] X, double[] Y, double[] Dist1, double[] Dist2, double[] Thick)
        {
            //if (this.PointCloudSendState) return; // 没处理完上一个返回
            if (PointsCloudAcqComplete != null)
            {
                this.PointCloudSendState = true;
                PointsCloudAcqComplete.Invoke(this, new PointCloudAcqCompleteEventArgs(sensorName, X, Y, Dist1, Dist2, Thick));
            }
        }
        private void ImageAcqEndInvoke(IAsyncResult ar)
        {
            this.ImageSendState = false;
        }
        private void PointCloudAcqEndInvoke(IAsyncResult ar)
        {
            this.PointCloudSendState = false;
        }


        protected virtual bool AutoFocus(HImage[] ImgIn, out HImage ImgOut)
        {
            ImgOut = new HImage();
            HImage RotImg = new HImage(), MirrorXImg = new HImage(), MirrorYImg = new HImage(), ZoomImage = new HImage();
            if (ImgIn == null)
                return false;
            else
            {
                int Width, Height;
                double Deviation;
                double[] grayValue = new double[ImgIn.Length];
                for (int i = 0; i < ImgIn.Length; i++)
                {
                    ImgIn[i].GetImageSize(out Width, out Height);
                    HImage ImagePart00 = ImgIn[i].CropPart(0, 0, Width, Height - 2);
                    ImagePart00 = ImagePart00.ConvertImageType("real");
                    HImage ImagePart20 = ImgIn[i].CropPart(2, 0, Width, Height - 2);
                    ImagePart20 = ImagePart20.ConvertImageType("real");
                    HImage ImageSub = ImagePart20.SubImage(ImagePart00, 1.0, 0);
                    HImage multImage = ImageSub.MultImage(ImageSub, 1.0, 0);
                    grayValue[i] = multImage.Intensity(multImage, out Deviation);
                }
                HTuple index = new HTuple(grayValue).TupleSortIndex();
                ImgOut = ImgIn[index[index.Length - 1].I].Clone();  // 选出值最大的图像
            }
            return true;
        }

        protected virtual bool AdjImg(HImage ImgIn, out HImage ImgOut)
        {
            ImgOut = new HImage();
            HImage RotImg = new HImage(), MirrorXImg = new HImage(), MirrorYImg = new HImage(), ZoomImage = new HImage();
            if (ImgIn == null || !ImgIn.IsInitialized())
                return false;
            else
            {
                // 旋转图像
                if ((bool)this.CameraParam?.IsRot)
                    RotImg = ImgIn.RotateImage(90.0, "constant");
                else
                    RotImg = ImgIn;
                // X 轴镜像
                if ((bool)this.CameraParam?.IsMirrorX)
                    MirrorXImg = RotImg.MirrorImage("column");
                else
                    MirrorXImg = RotImg;
                // Y 轴镜像
                if ((bool)this.CameraParam?.IsMirrorY)
                    MirrorYImg = MirrorXImg.MirrorImage("row");
                else
                    MirrorYImg = MirrorXImg;
                // 畸变校正
                if ((this.CameraParam.EnableDistoryRectify && this.CameraParam?.Map != null) || (this.ConfigParam.IsActiveDistortionCorrect && this.CameraParam?.Map != null))
                    ZoomImage = MirrorYImg.MapImage(this.CameraParam?.Map);
                else
                    ZoomImage = MirrorYImg;
                // 图像缩放
                if ((bool)this.CameraParam?.EnableScale)
                    ImgOut = ZoomImage.ZoomImageFactor(this.CameraParam.ScaleWidth, this.CameraParam.ScaleHeight, "bilinear");
                else
                    ImgOut = MirrorYImg.Clone();

                // 释放对象
                if (RotImg != null && RotImg.IsInitialized())
                    RotImg?.Dispose();
                if (MirrorXImg != null && MirrorXImg.IsInitialized())
                    MirrorXImg?.Dispose();
                if (MirrorYImg != null && MirrorYImg.IsInitialized())
                    MirrorYImg?.Dispose();
                if (ZoomImage != null && ZoomImage.IsInitialized())
                    ZoomImage?.Dispose();
            }
            return true;
        }

        protected virtual bool AdjObject3D(HObjectModel3D Model3D, out HObjectModel3D Model3DOut)
        {
            Model3DOut = new HObjectModel3D();
            if (Model3D == null || !Model3D.IsInitialized())
                return false;
            else
            {
                HTuple hTuple_x, hTuple_y, hTuple_z;
                // 旋转图像
                if ((bool)this.CameraParam?.IsRot)
                {
                    hTuple_x = Model3D.GetObjectModel3dParams("point_coord_x");
                    hTuple_y = Model3D.GetObjectModel3dParams("point_coord_y");
                    Model3D.SetObjectModel3dAttribMod(new HTuple("point_coord_x"), "", hTuple_y);
                    Model3D.SetObjectModel3dAttribMod(new HTuple("point_coord_y"), "", hTuple_x);
                }
                // X 轴镜像
                if ((bool)this.CameraParam?.IsMirrorX)
                {
                    hTuple_x = Model3D.GetObjectModel3dParams("point_coord_x");
                    Model3D.SetObjectModel3dAttribMod(new HTuple("point_coord_x"), "", hTuple_x * -1);
                }
                // Y 轴镜像
                if ((bool)this.CameraParam?.IsMirrorY)
                {
                    hTuple_y = Model3D.GetObjectModel3dParams("point_coord_y");
                    Model3D.SetObjectModel3dAttribMod(new HTuple("point_coord_y"), "", hTuple_y * -1);
                }
                // 畸变校正
                //if (this.CameraParam.EnableDistoryRectify && this.CameraParam?.Map != null)
                //    ZoomImage = MirrorYImg.MapImage(this.CameraParam?.Map);
                //else
                //    ZoomImage = MirrorYImg;
                // 图像缩放
                if ((bool)this.CameraParam?.EnableScale)
                {
                    hTuple_x = Model3D.GetObjectModel3dParams("point_coord_x");
                    hTuple_y = Model3D.GetObjectModel3dParams("point_coord_y");
                    hTuple_z = Model3D.GetObjectModel3dParams("point_coord_z");
                    Model3D.SetObjectModel3dAttribMod(new HTuple("point_coord_x"), "", hTuple_x * this.CameraParam.ScaleWidth); // object
                    Model3D.SetObjectModel3dAttribMod(new HTuple("point_coord_y"), "", hTuple_y * this.CameraParam.ScaleHeight);
                    Model3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), "", hTuple_y * this.CameraParam.ScaleGrayValue);
                }
                Model3DOut = Model3D.Clone();
                Model3D.ClearObjectModel3d();
            }
            return true;
        }

        protected virtual HImage AveImage(List<HImage> imageList)
        {
            HImage hImage = new HImage();
            switch (imageList.Count)
            {
                case 1:
                    hImage = imageList.Last().Clone();
                    break;
                case 2:
                    hImage = imageList[0].Compose2(imageList[1]);
                    hImage = hImage.MeanN();
                    break;
                case 3:
                    hImage = imageList[0].Compose3(imageList[1], imageList[1]);
                    hImage = hImage.MeanN();
                    break;
                case 4:
                    hImage = imageList[0].Compose4(imageList[1], imageList[2], imageList[3]);
                    hImage = hImage.MeanN();
                    break;
                case 5:
                    hImage = imageList[0].Compose5(imageList[1], imageList[2], imageList[3], imageList[4]);
                    hImage = hImage.MeanN();
                    break;
                case 6:
                    hImage = imageList[0].Compose6(imageList[1], imageList[2], imageList[3], imageList[4], imageList[5]);
                    hImage = hImage.MeanN();
                    break;
                case 7:
                    hImage = imageList[0].Compose7(imageList[1], imageList[2], imageList[3], imageList[4], imageList[5], imageList[6]);
                    hImage = hImage.MeanN();
                    break;
                default:
                    hImage = imageList.Last().Clone();
                    break;
            }
            return hImage;
        }

        protected virtual bool GetImageAsyn(out HImage hImage, out HImage darkImage)
        {
            throw new NotImplementedException();
        }
        protected virtual void GetImageAsyn()
        {
            throw new NotImplementedException();
        }
        protected virtual bool GetImageSyn(out HImage hImage, out HImage darkImage)
        {
            throw new NotImplementedException();
        }

    }
}
