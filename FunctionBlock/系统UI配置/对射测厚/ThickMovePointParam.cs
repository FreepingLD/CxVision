using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    [Serializable]
    public class ThickMovePointParam
    {
        public bool IsActive { get; set; } // 是否触发传感器
        public enMoveType MoveType { get; set; } // 运动方式 
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public string ViewWindow { get; set; }

        public ThickMovePointParam()
        {
            this.IsActive = true;// 是否触发传感器
            this.MoveType = enMoveType.点位运动; // 运动方式 
            this.X1 = 0;
            this.Y1 = 0;
            this.X2 = 0;
            this.Y2 = 0;
            this.ViewWindow = "NONE";
        }
        public ThickMovePointParam(enMoveType moveType, double x1, double y1, double x2, double y2)
        {
            this.IsActive = true;// 是否触发传感器
            this.MoveType = moveType; // 运动方式 
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
            this.ViewWindow = "NONE";
        }


    }

    [Serializable]
    public class TrackMoveParam
    {
        [DisplayNameAttribute("激活")]
        public bool IsActive { get; set; } // 是否触发传感器

        [DisplayNameAttribute("轨迹类型")]
        public enMoveType MoveType { get; set; } // 运动方式 
   
        /// <summary>
        /// 2D 中使用像素形状
        /// </summary>
        [DisplayNameAttribute("轨迹参数")]
        [System.Xml.Serialization.XmlElement(nameof(WcsROI), typeof(WcsROI))]
        [System.Xml.Serialization.XmlElement(nameof(drawWcsPoint), typeof(drawWcsPoint))]
        [System.Xml.Serialization.XmlElement(nameof(drawWcsLine), typeof(drawWcsLine))]
        [System.Xml.Serialization.XmlElement(nameof(drawWcsCircle), typeof(drawWcsCircle))]
        [System.Xml.Serialization.XmlElement(nameof(drawWcsEllipse), typeof(drawWcsEllipse))]
        [System.Xml.Serialization.XmlElement(nameof(drawWcsPolyLine), typeof(drawWcsPolyLine))]
        [System.Xml.Serialization.XmlElement(nameof(drawWcsPolygon), typeof(drawWcsPolygon))]
        public WcsROI RoiShape { get; set; }


        [DisplayNameAttribute("功能参数")]
        public enFunction Function { get; set; }


        public TrackMoveParam()
        {
            this.IsActive = true;
            this.MoveType = enMoveType.点位运动;
            this.RoiShape = null;
            this.Function = enFunction.测量;
        }

        public TrackMoveParam(WcsROI RoiShape)
        {
            this.IsActive = true;
            this.MoveType = enMoveType.点位运动;
            this.RoiShape = RoiShape;
            this.Function = enFunction.测量;
        }

        public TrackMoveParam(WcsROI RoiShape, enMoveType moveType)
        {
            this.IsActive = true;
            this.MoveType = moveType;
            this.RoiShape = RoiShape;
            this.Function = enFunction.测量;
        }


    }


    public enum enFunction
    {
        测量,
        定位,
        校准,
        圆心,
        待机位,
    }


}
