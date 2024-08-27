using HalconDotNet;
using System;

namespace FunctionBlock
{
    [Serializable]
    public class EdgesColorSubPix
    {
        private string filter = "canny";
        private double alpha = 1.0;
        private double low = 20;
        private double high = 40;
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

        public HXLDCont edges_color_sub_pix(HImage image)
        {
            HXLDCont Edges = null;
            if (image == null) return new HXLDCont();
            Edges = image.EdgesColorSubPix(Filter, Alpha, Low, High);
            return Edges;
        }

        public enum enEdgesColorSubPixFilterParam
        {
            canny,
            canny_junctions,
            deriche1,
            deriche1_junctions,
            deriche2,
            deriche2_junctions,
            shen,
            shen_junctions,
            sobel_fast,
        }



    }
}
