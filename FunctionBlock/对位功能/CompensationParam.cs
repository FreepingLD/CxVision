using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    [Serializable]
    public class CompensationParam
    {
        public enRefObject RefObject { get; set; }
        public enAlignmentMethod AlignmentMethod { get; set; }
        public enOffsetMethod OffsetMethod { get; set; }

        public enCoordSysName CoordSysName { get; set; }
        public bool IsWcsToRotate { get; set; }
        public double Add_X { get; set; }
        public double Add_Y { get; set; }
        public double Add_Angle { get; set; }
        public string ViewWindow { get; set; }

        public bool IsAuto { get; set; }
        public bool IsInvert { get; set; }
        public double Threshold { get; set; }

        public bool IsZoneCompensation { get; set; }

        public bool IsOutputUvw { get; set; }
        public CompensationParam()
        {
            this.RefObject = enRefObject.目标点;
            this.AlignmentMethod = enAlignmentMethod.四点对齐;
            this.IsWcsToRotate = true;
            this.OffsetMethod = enOffsetMethod.两点向量差;
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.ViewWindow = "NONE";
            this.IsAuto = false;
            this.IsInvert = false;
            this.Threshold = 0;
            this.IsZoneCompensation = false;
            this.IsOutputUvw = false;
        }
    }

    [Serializable]
    public enum enRefObject
    {
        视野中心,
        示教点,
        目标点,
        映射相机,
        当前点,
    }

    [Serializable]
    public enum enAlignmentMethod
    {
        单点对齐,
        两点对齐_起点角度,
        向量对齐,
        两点对齐,
        三点对齐,
        四点对齐,
        四点对齐_Rigid,
        N点对齐,
        四点对齐_两点平移,
        向量对位_放料计算,
    }

    [Serializable]
    public enum enOffsetMethod
    {
        两点向量差,
        三点向量差,
        四点向量差,
    }

}
