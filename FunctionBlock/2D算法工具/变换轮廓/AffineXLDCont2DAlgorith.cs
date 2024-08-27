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
    public class AffineXLDCont2DAlgorith
    {
        private XldDataClass xldData = null; // 转换的对象可能是XLD　region，image等 
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


        public bool AffineObjectModel2D(XldDataClass hObjcet, userWcsCoordSystem wcsCoord, string isInvertPose, out XldDataClass affineXLD)
        {
            bool result = false;
            if (hObjcet == null)
            {
                affineXLD = null;
                return result;
            }
            affineXLD = hObjcet.Clone();
            HHomMat2D homMat2D;
            HHomMat2D homMat2DInvert;
            ///////////////////////////////////////////
            homMat2D = new HHomMat2D(wcsCoord.GetVariationHomMat2D());
            if (isInvertPose == "true")
                homMat2DInvert = homMat2D.HomMat2dInvert();
            else
                homMat2DInvert = homMat2D;
            ////////////////////////////////
            affineXLD.HXldCont = homMat2DInvert.AffineTransContourXld(hObjcet.HXldCont);
            result = true;
            return result;
        }


        public enum enAffineTransMethod
        {
            affine_trans_contour_xld,
            affine_trans_image,
            affine_trans_polygon_xld,
            affine_trans_region,
        }



    }
}
