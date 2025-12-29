using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{

    [Serializable]
    public class AcqTrackParam
    {
        [DefaultValue(true)]
        [DisplayNameAttribute("激活")]
        public bool IsActive { get; set; } // 是否触发传感器

        [DefaultValue(enCoordSysName.CoordSys_0)]
        [DisplayNameAttribute("坐标系名称")]
        public enCoordSysName CoordSysName { get; set; }

        [DefaultValue(enAxisName.XYZTheta轴)]
        [DisplayNameAttribute("移动轴")]
        public enAxisName MoveAxis { get; set; }

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

        [DefaultValue(-1)]
        [DisplayNameAttribute("IO输出")]
        public int IoOutPort { get; set; }

        [DefaultValue(true)]
        [DisplayNameAttribute("是否同步")]
        public bool IsWait { get; set; }


        public AcqTrackParam()
        {
            this.IsActive = true;
            this.MoveType = enMoveType.点位运动;
            this.RoiShape = new drawWcsPoint();
            this.Function = enFunction.测量;
        }

        public AcqTrackParam(WcsROI RoiShape)
        {
            this.IsActive = true;
            this.MoveType = enMoveType.点位运动;
            this.RoiShape = RoiShape;
            this.Function = enFunction.测量;
        }

        public AcqTrackParam(WcsROI RoiShape, enMoveType moveType)
        {
            this.IsActive = true;
            this.MoveType = moveType;
            this.RoiShape = RoiShape;
            this.Function = enFunction.测量;
        }


    }

}
