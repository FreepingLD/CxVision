using Common;
using HalconDotNet;
using System;

namespace AlgorithmsLibrary
{
    [Serializable]
    public class SegmentContourAttribXldAlgorith
    {
        private enAttributeParam attribute = enAttributeParam.edge_direction;
        private enOperationParam operation = enOperationParam.and;
        private double min =150;
        private double max =999999;

        public enAttributeParam Attribute { get => attribute; set => attribute = value; }
        public enOperationParam Operation { get => operation; set => operation = value; }
        public double Min { get => min; set => min = value; }
        public double Max { get => max; set => max = value; }

        public HXLDCont SegmentContourAttribXld(HXLDCont xld)
        {
            HXLDCont Edges = null;
            if (xld == null) return new HXLDCont();
            Edges = xld.SegmentContourAttribXld(Attribute.ToString(), Operation.ToString(), Min, Max);
            return Edges;
        }



    }
}
