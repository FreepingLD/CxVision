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
    public class ImageZoomMethod
    {

        public ImageZoomMethod()
        {

        }


        public HImage ImageZoom(HImage hImage, ImageZoomParam param)
        {
            HImage tarHImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (param == null)
                throw new ArgumentNullException("param");
            ////////////////////////////////////////////////
            switch (param.Method.Trim())
            {
                case "zoom_image_factor":
                    tarHImage = zoom_image_factor(hImage, (ZoomImageFactorParam)param);
                    break;
                case "zoom_image_size":
                    tarHImage = zoom_image_size(hImage, (ZoomImageSizeParam)param);
                    break;
            }
            return tarHImage;
        }


        private HImage zoom_image_factor(HImage hImage, ZoomImageFactorParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.ZoomImageFactor((int)param.ScaleWidth, (int)param.ScaleHeight, param.Interpolation.ToString());
            return tarImage;
        }
        private HImage zoom_image_size(HImage hImage, ZoomImageSizeParam param)
        {
            HImage tarImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            tarImage = hImage.ZoomImageSize(param.ImageWidth, param.ImageHeight, param.Interpolation.ToString());
            return tarImage;
        }

    }

    public enum enImageZoomMethod
    {
        zoom_image_factor,
        zoom_image_size,
    }





}
