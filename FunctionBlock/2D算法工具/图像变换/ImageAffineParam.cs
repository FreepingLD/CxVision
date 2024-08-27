using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace AlgorithmsLibrary
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ImageAffineParam
    {
        public string InvertPose { get; set; }
        public string Interpolation { get; set; }
        public string AdaptImageSize { get; set; }

        public ImageAffineParam()
        {
            this.InvertPose = "false";
            this.Interpolation = "constant";
            this.AdaptImageSize = "false";
        }

        public bool ImageAffine(HImage hObjcet, userPixCoordSystem pixCoord, ImageAffineParam param, out HImage affineImage)
        {
            bool result = false;
            if (hObjcet == null)
            {
                throw new ArgumentNullException("hObjcet");
            }
            if (pixCoord == null)
            {
                throw new ArgumentNullException("pixCoord");
            }
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }
            HHomMat2D homMat2D;
            HHomMat2D homMat2DInvert;
            ///////////////////////////////////////////
            homMat2D = new HHomMat2D(pixCoord.GetVariationHomMat2D());
            if (param.InvertPose == "true")
                homMat2DInvert = homMat2D.HomMat2dInvert();
            else
                homMat2DInvert = homMat2D;
            ////////////////////////////////
            affineImage = homMat2DInvert.AffineTransImage(hObjcet, param.Interpolation, param.AdaptImageSize);
            result = true;
            return result;
        }

    }

}
