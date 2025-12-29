using HalconDotNet;
using System;

namespace FunctionBlock
{
    [Serializable]
    public class SegmentContourAttribXld
    {
        private string attribute = "edge_direction";
        private string operation = "and";
        private double min =150;
        private double max =999999;

        public string Attribute { get => attribute; set => attribute = value; }
        public string Operation { get => operation; set => operation = value; }
        public double Min { get => min; set => min = value; }
        public double Max { get => max; set => max = value; }

        public HXLDCont segment_contour_attrib_xld(HXLDCont xld)
        {
            HXLDCont Edges = null;
            if (xld == null) return new HXLDCont();
            Edges = xld.SegmentContourAttribXld(Attribute, Operation, Min, Max);
            return Edges;
        }

        public enum enAttributeParam
        {
            edge_direction,
            angle,
            response,
            width_right,
            width_left,
            contrast,
            asymmetry,
            distance,
        }

        public enum enOperationParam
        {
            and,
            or,
        }

    }
}
