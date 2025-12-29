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
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class AffineHRegionAlgorith
    {
        private RegionDataClass regionData = null; // 转换的对象可能是XLD　region，image等 
        private string invertPose = "false";

        public string InvertPose
        {
            get
            {
                return invertPose;
            }

            set
            {
                invertPose = value;
            }
        }
        public bool AffineObjectModel2D(RegionDataClass hObjcet, userWcsCoordSystem wcsCoord, string isInvertPose, out RegionDataClass affineRegion)
        {
            bool result = false;
            if (hObjcet == null)
            {
                affineRegion = null;
                return result;
            }
            affineRegion = hObjcet.Clone();
            HHomMat2D homMat2D;
            HHomMat2D homMat2DInvert;
            ///////////////////////////////////////////
            homMat2D = new HHomMat2D(wcsCoord.GetVariationHomMat2D());
            if (isInvertPose == "true")
                homMat2DInvert = homMat2D.HomMat2dInvert();
            else
                homMat2DInvert = homMat2D;
            ////////////////////////////////
            affineRegion.Region = homMat2DInvert.AffineTransRegion(hObjcet.Region, "bilinear");
            result = true;
            return result;
        }


    }
}
