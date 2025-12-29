using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DoBlobAnalyse
    {
        public SegmentParam SegParam { get; set; }
        public BindingList<SelectOperateParam> SelectParam { get; set; }
        public SortParam SortParam { get; set; } //
        public BindingList<RegionOperateParam>  MorphologyParam { get; set; } //RegionMorphologyParam
        public OutputParam OutParam { get; set; }

        public DoBlobAnalyse()
        {
            this.SegParam = new ThresholdBlob();
            this.SelectParam = new BindingList<SelectOperateParam>();
            this.SortParam = new SortParam();
            this.MorphologyParam = new BindingList<RegionOperateParam>();
            this.OutParam = new OutputParam();
        }

        public bool Do(HImage hImage, out HRegion hRegion)
        {
            hRegion = new HRegion();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            ////////////////////////////////////////////////
            HRegion segRegion = ThresholdMethod.Instance.SegmentRegion(hImage, this.SegParam);
            HRegion morRegion = MorphologyMethod.Instance.RegionMorphology(segRegion, this.MorphologyParam);
            HRegion selectRegion = SelectRegionMethod.Instance.SelectRegion(hImage,morRegion, this.SelectParam);
            hRegion = SortRegionMethod.Instance.SortRegion(selectRegion, this.SortParam);
            return true;
        }





    }
}
