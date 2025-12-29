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
    public class ThresholdSubPix 
    {
        private double threshold = 128;
        public double Threshold
        {
            get
            {
                return threshold;
            }

            set
            {
                threshold = value;
            }
        }

        public HXLDCont threshold_sub_pix(HImage image)
        {
            HXLDCont Edges = null;
            if (image == null) return new HXLDCont();
            Edges = image.ThresholdSubPix(Threshold);
            return Edges;
        }

    }
}
