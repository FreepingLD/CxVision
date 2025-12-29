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
    public class EdgesSubPix
    {
        private string filter = "canny";
        private double alpha = 1.0;
        private double low = 20;
        private double high = 40;
        //
        public string Filter
        {
            get
            {
                return filter;
            }

            set
            {
                filter = value;
            }
        }
        public double Alpha
        {
            get
            {
                return alpha;
            }

            set
            {
                alpha = value;
            }
        }
        public double Low
        {
            get
            {
                return low;
            }

            set
            {
                low = value;
            }
        }
        public double High
        {
            get
            {
                return high;
            }

            set
            {
                high = value;
            }
        }


        public HXLDCont edges_sub_pix(HImage image)
        {
            HXLDCont Edges = null;
            if (image == null) return new HXLDCont();
            Edges = image.EdgesSubPix(Filter, Alpha, Low, High);
            return Edges;
        }

        public enum enEdgesSubPixFilterParam
        {
            canny,
            canny_junctions,
            deriche1,
            deriche1_junctions,
            deriche2,
            deriche2_junctions,
            lanser1,
            lanser1_junctions,
            lanser2,
            lanser2_junctions,
            mshen,
            mshen_junctions,
            shen,
            shen_junctions,
            sobel,
            sobel_fast,
            sobel_junctions,
        }


    }
}
