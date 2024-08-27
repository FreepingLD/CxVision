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
    public class ZeroCrossingSubPix 
    {
        private double laplaceSigma = 2.0;                  
        public double LaplaceSigma
        {
            get
            {
                return laplaceSigma;
            }

            set
            {
                laplaceSigma = value;
            }
        }


        public HXLDCont zero_crossing_sub_pix(HImage image)
        {
            HXLDCont Edges = null;
            if (image == null) return new HXLDCont();
            Edges = image.LaplaceOfGauss(laplaceSigma).ZeroCrossingSubPix();
            return Edges;
        }





    }
}
