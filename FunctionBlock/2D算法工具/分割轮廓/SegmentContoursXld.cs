using HalconDotNet;
using System;

namespace FunctionBlock
{
    [Serializable]
    public class SegmentContoursXld
    {
        private int smoothCont = 15;
        private double maxLineDist1 = 4.0;
        private double maxLineDist2 = 2.0;
        private string mode = "lines_circles";


        public string Mode { get => mode; set => mode = value; }
        public int SmoothCont { get => smoothCont; set => smoothCont = value; }
        public double MaxLineDist1 { get => maxLineDist1; set => maxLineDist1 = value; }
        public double MaxLineDist2 { get => maxLineDist2; set => maxLineDist2 = value; }

        public void segment_contours_xld(HXLDCont xld, out HXLDCont lines, out HXLDCont cirlces, out HXLDCont ellipses)
        {
            HXLDCont Edges = null;
            lines = new HXLDCont();
            cirlces = new HXLDCont();
            ellipses = new HXLDCont();
            ////////
            lines.GenEmptyObj();
            cirlces.GenEmptyObj();
            ellipses.GenEmptyObj();
            ////
            Edges = xld.SegmentContoursXld(Mode, SmoothCont, MaxLineDist1, MaxLineDist2);
            int num = Edges.CountObj();
            for (int i = 0; i < num; i++)
            {
                HTuple type = Edges.SelectObj(i + 1).GetContourGlobalAttribXld("cont_approx");
                switch (type.D)
                {
                    // 直线段
                    case -1:
                        lines = lines.ConcatObj(Edges.SelectObj(i + 1));
                        break;
                    // 椭圆
                    case 0:
                        cirlces = cirlces.ConcatObj(Edges.SelectObj(i + 1));
                        break;
                    //圆
                    case 1:
                        ellipses = ellipses.ConcatObj(Edges.SelectObj(i + 1));
                        break;
                }
            }
        }

        public enum enModeParam
        {
            lines,
            lines_circles,
            lines_ellipses
        }





    }
}
