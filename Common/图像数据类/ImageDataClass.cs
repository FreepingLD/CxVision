using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Runtime.InteropServices;
using Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
    [Serializable]
    /// <summary>
    /// 封装图像数据及图像对应相机的内外参
    /// </summary>
    public class ImageDataClass
    {
        private double grab_x = 0; // 当前采集图像的机台坐标
        private double grab_y = 0;
        private double grab_theta = 0;
        private double grab_z = 0;
        private double grab_u = 0;
        private double grab_v = 0;
        private double camAxis_x = 0;
        private double camAxis_y = 0;
        private CameraParam camParams;
        //[NonSerialized]
        private HImage image;
        private int width = 2048;
        private int height = 2048;
        private object _Tag = 1;
        private int _detectEdge = 1;

        public HImage Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }
        public double Grab_X
        {
            get
            {
                return grab_x;
            }

            set
            {
                grab_x = value;
            }
        }
        public double Grab_Y
        {
            get
            {
                return grab_y;
            }

            set
            {
                grab_y = value;
            }
        }
        public double Grab_Z
        {
            get
            {
                return grab_z;
            }

            set
            {
                grab_z = value;
            }
        }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public double Grab_Theta { get => grab_theta; set => grab_theta = value; }
        public double Grab_U { get => grab_u; set => grab_u = value; }
        public double Grab_V { get => grab_v; set => grab_v = value; }
        public CameraParam CamParams { get => camParams; set => camParams = value; }
        public string CamName { get; set; }
        public string ViewWindow { get; set; }
        public object Tag { get => this._Tag; set => _Tag = value; }
        public int DetectEdge { get => this._detectEdge; set => _detectEdge = value; }
        public double CamAxis_X { get => camAxis_x; set => camAxis_x = value; }
        public double CamAxis_Y { get => camAxis_y; set => camAxis_y = value; }
        public string ProductID { get; set; } = ""; // 产品ID
        public string PathNode { get; set; } = ""; // 节点路径


        public ImageDataClass()
        {
            this.camParams = new CameraParam();
        }

        public ImageDataClass(HImage hImage, double X_Pos = 0, double Y_Pos = 0, double Theta = 0)
        {
            if (hImage != null)
            {
                this.image = hImage;
                this.image.GetImageSize(out this.width, out this.height);
                InitCamParam(image);
                this.grab_x = X_Pos;
                this.grab_y = Y_Pos;
                this.grab_theta = Theta;
            }
            else
            {
                this.grab_x = 0;
                this.grab_y = 0;
                this.grab_theta = 0;
                this.camParams = new CameraParam();
            }
            this.ViewWindow = "NONE";
        }
        public ImageDataClass(HImage image)
        {
            if (image != null)
            {
                this.CamName = "NONE";
                this.image = image;
                this.image?.GetImageSize(out this.width, out this.height);
                InitCamParam(image);
            }
            else
                this.camParams = new CameraParam();
            this.ViewWindow = "NONE";
        }
        public ImageDataClass(HImage image, CameraParam camParam, object imageIndex)
        {
            if (image != null && image.IsInitialized())
            {
                this.image = image;
                this.camParams = camParam;
                this.CamName = camParam.SensorName;
                this.image?.GetImageSize(out this.width, out this.height);
                this.ViewWindow = "NONE";
                if (imageIndex.ToString() == "auto")
                    this._Tag = 1;
                else
                    this._Tag = imageIndex;
            }
        }

        public ImageDataClass(HImage image, CameraParam camParam)
        {
            if (image != null && image.IsInitialized())
            {
                this.image = image;
                this.camParams = camParam;
                this.CamName = camParam.SensorName;
                this.image?.GetImageSize(out this.width, out this.height);
                this.ViewWindow = "NONE";
            }
        }

        public ImageDataClass(HImage image, CameraParam camParam, double X_Pos, double Y_Pos, double Z_Pos, double Theta)
        {
            if (image != null && image.IsInitialized())
            {
                this.image = image;
                this.camParams = camParam;
                this.grab_x = X_Pos;
                this.grab_y = Y_Pos;
                this.grab_z = Z_Pos;
                this.grab_theta = Theta;
                this.CamName = camParam.SensorName;
                this.image?.GetImageSize(out this.width, out this.height);
                this.ViewWindow = "NONE";
            }
        }

        public ImageDataClass Clone()
        {
            if (this == null) return null;
            ImageDataClass imageData = new ImageDataClass();
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, this);  //序列化
                ms.Seek(0, SeekOrigin.Begin);  // 将当前流的位置移动到开始处
                imageData = (ImageDataClass)bf.Deserialize(ms);  //反序列化
            }
            return imageData;
        }


        public void Dispose()
        {
            if (this.image != null && this.image.Key.ToInt64() > 0)
            {
                this.image.Dispose();
                this.image = null;
            }
        }

        private void InitCamParam(HImage image)
        {
            int width, height;
            HTuple x, y;
            if (image == null)
            {
                this.camParams = new CameraParam();
            }
            else
            {
                image.GetImageSize(out width, out height);
                userCamParam camParam;
                userCamPose camPose;
                double Quality;
                UserHomMat2D homMat2D = new UserHomMat2D(true);
                this.camParams = new CameraParam();
                NinePointCalibParam.HHomMat2DToCamparaCamPos(homMat2D.GetHHomMat(), width, height, 0.5, out camParam, out camPose, out Quality);
                this.camParams.HomMat2D = homMat2D;
                this.camParams.CamParam = camParam;
                this.camParams.CamPose = camPose;
            }
        }

        public override string ToString()
        {
            return string.Format("宽度：{0},高度：{1}", this.width, this.height);
        }

        public bool IsInitialized()
        {
            if (this.image != null && this.image.IsInitialized())
            {
                return true;
            }
            else
                return false;
        }

        public string GetImageType()
        {
            if (this.image != null && this.image.IsInitialized())
                return this.image.GetImageType().S;
            else
                return "";
        }

        public HImage GetImage(string type = "byte")
        {
            if (this.image != null && this.image.IsInitialized())
            {
                string ss = this.image.GetImageType().S;
                if (this.image.GetImageType().S == type)
                {
                    return this.image;
                }
                else
                {
                    return this.image.ConvertImageType(type);
                }
            }
            else
            {
                return this.image;
            }
        }


        public ImageDataClass InvertImage()
        {
            if (this.image == null || !this.image.IsInitialized()) return null;
            ImageDataClass imageData = this.Clone();
            imageData.image = this.image.InvertImage();
            return imageData;
        }

    }

}
