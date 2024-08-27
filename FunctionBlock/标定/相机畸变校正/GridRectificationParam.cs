using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class GridRectificationParam
    {
        [DisplayNameAttribute("FilterType"),Category("鞍点提取参数"), Description("鞍点提取的滤波类型")]
        public enFilter SaddleFilter { get; set; }

        [DisplayNameAttribute("Sigma"),Category("鞍点提取参数"), Description("鞍点提取的Sigma平滑系数")]
        public double SaddleSigma { get; set; }

        [DisplayNameAttribute("Threshold"),Category("鞍点提取参数"), Description("特征点的最小绝对阈值")]
        public double SaddleThreshold { get; set; }


        [DisplayNameAttribute("Sigma"), Category("网格点连接参数"), Description("网格点连接Sigma")]
        public double ConnectSigma { get; set; }
        [DisplayNameAttribute("MaxDist"), Category("网格点连接参数"),Description("网格点到连接线的最大距离")]
        public double ConnectMaxDist { get; set; }



        [DisplayNameAttribute("MaxDist"), Category("矫正参数"), Description("矫正网格点间的距离")]
        public int GridSpacing { get; set; }
        [DisplayNameAttribute("Rotation"), Category("矫正参数"), Description("矫正图像的旋转角度")]
        public int Rotation { get; set; }
        [DisplayNameAttribute("MapType"), Category("矫正参数"), Description("图像插值方式")]
        public enMapType MapType { get; set; }


        public GridRectificationParam()
        {
            this.SaddleFilter = enFilter.facet;
            this.SaddleSigma =0.7 ;
            this.SaddleThreshold = 5;
            //////////////////
            this.ConnectSigma = 0.9;
            this.ConnectMaxDist = 5.5;
            /////
            this.GridSpacing = 15;
            this.Rotation = 0;
            this.MapType = enMapType.bilinear;
        }


    }

    public enum enFilter
    {
        facet, 
        gauss,
    }
    public enum enMapType
    {
        bilinear, 
        coord_map_sub_pix,
    }


}
