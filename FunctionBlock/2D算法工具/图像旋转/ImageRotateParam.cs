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
    public class ImageRotateParam
    {

        public string RotateDeg { get; set; }
        public string Interpolation { get; set; }

        public ImageRotateParam()
        {
            this.RotateDeg = "0";
            this.Interpolation = "constant";
        }


        public bool ImageRotate(HImage hObjcet, userPixCoordSystem pixCoord, ImageRotateParam param, out HImage affineImage)
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
            double deg = 0;
            ///////////////////////////////////////////
            switch (param.RotateDeg)
            {
                case "X轴":
                    deg = pixCoord.CurrentPoint.Rad * 180 / Math.PI * -1;
                    break;
                case "Y轴":
                    deg = 90 - pixCoord.CurrentPoint.Rad * 180 / Math.PI;
                    break;
                default:
                    double.TryParse(param.RotateDeg, out deg);
                    break;
            }
            ////////////////////////////////
            affineImage = hObjcet.RotateImage(deg, param.Interpolation);
            result = true;
            return result;
        }

    }
}
