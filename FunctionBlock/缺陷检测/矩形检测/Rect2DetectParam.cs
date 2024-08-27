using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    [Serializable]
    public class Rect2DetectParam
    {
        /// <summary>
        /// 起始列偏移
        /// </summary>
        public double RowOffset { get; set; }
        public double ColOffset { get; set; }


        /// <summary>
        /// 示教面积
        /// </summary>
        public double TeachArea { get; set; }

        /// <summary>
        /// 是否检测缺口
        /// </summary>
        public bool IsDetectGap { get; set; }

        /// <summary>
        /// 是否检测裂纹
        /// </summary>
        public bool IsDetectCrack { get; set; }

        /// <summary>
        /// 低阈值
        /// </summary>
        public double LowTh { get; set; }

        /// <summary>
        /// 高阈值
        /// </summary>
        public double HighTh { get; set; }

        public int ClosingWidth { get; set; }
        public int ClosingHeight { get; set; }

        public int OpenWidth { get; set; }
        public int OpenHeight { get; set; }

        /// <summary>
        /// 兴趣区域
        /// </summary>
        public BindingList<RoiParam> RoiParam { get; set; }


        public Rect2DetectParam()
        {
            RowOffset = 10;
            ColOffset = 10;
            TeachArea = 0;
            RoiParam = new BindingList<RoiParam>();
            IsDetectGap = true;
            IsDetectCrack = true;
            LowTh = 20;
            HighTh = 255;
            ClosingWidth = 555;
            ClosingHeight = 555;
            OpenWidth = 1;
            OpenHeight = 155;
        }





    }


}
