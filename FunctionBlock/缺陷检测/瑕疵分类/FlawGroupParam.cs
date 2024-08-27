using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    [Serializable]
    public class FlawGroupParam
    {
        /// <summary>
        /// 测量方法
        /// </summary>
        public enMeasureMethod MeasureMethod { get; set; } 
        /// <summary>
        /// 采样距离
        /// </summary>
        public int SampleDist { get; set; } 
        /// <summary>
        /// 采样距离缩放
        /// </summary>
        public int SampleScale { get; set; } 
        /// <summary>
        /// 平滑系数
        /// </summary>
        public double Sigma{ get; set; } 
        /// <summary>
        /// 边缘振幅
        /// </summary>
        public double Threshold { get; set; }
        /// <summary>
        /// 区域膨胀宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 区域膨胀高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 区域最小面积
        /// </summary>
        public double Area { get; set; }

        public FlawGroupParam()
        {
            this.MeasureMethod = enMeasureMethod.双向;
            this.SampleDist = 10;
            this.Sigma = 2;
            this.Threshold = 10;
            this.Width = 15;
            this.Height = 15;
            this.Area = 100;
            this.SampleScale = 1;
        }
    }


}
