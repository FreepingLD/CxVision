using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    [Serializable]
    public class LineDetectParam
    {
        public int NgPointNum { get; set; } // Ng区域最少点数量
        public double DistThreshold { get; set; } // Ng点距离阈值
        public double PToPDist{ get; set; } // 相邻点最大距离
        public string Algorithm { get; set; }
        public double MaxNumPoints { get; set; }
        public int Iterations { get; set; }
        public double ClippingFactor { get; set; }
        public double ClippingEndPoints { get; set; }
        public string Unit { get; set; } // 单位
        public double NgWidth { get; set; } // Ng区域的宽度
        public double NgHeight { get; set; }// Ng区域的高度
        public double PixScale { get; set; }// Ng区域的高度
        public LineDetectParam()
        {
            this.Algorithm = "tukey";
            this.MaxNumPoints = -1;
            this.Iterations = 10;
            this.ClippingFactor = 2.0;
            this.ClippingEndPoints = 0;
            this.DistThreshold = 15;
            this.NgPointNum = 5;
            this.PToPDist = 20; // 相邻两点间的距离
            this.Unit = "pix";
            this.NgWidth = 150;
            this.NgHeight = 150;
            this.PixScale = 1;

        }


    }


}
