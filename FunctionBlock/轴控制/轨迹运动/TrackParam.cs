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
    public class TrackParam
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

        [DisplayNameAttribute("加减速参数")]
        public AccDec AccDecParam { get; set; }

        [DefaultValue(-1)]
        [DisplayNameAttribute("IO输出")]
        public int IoOutPort { get; set; }

        [DefaultValue(true)]
        [DisplayNameAttribute("同步")]
        public bool IsWait { get; set; }


        public TrackParam()
        {
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveAxis = enAxisName.XY轴;
            this.IsActive = true;
            this.MoveType = enMoveType.点位运动;
            this.RoiShape = null;
            this.IoOutPort = -1;
            this.IsWait = true;
            this.AccDecParam = new AccDec();
        }

        public TrackParam(WcsROI RoiShape)
        {
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveAxis = enAxisName.XY轴;
            this.IsActive = true;
            this.MoveType = enMoveType.点位运动;
            this.RoiShape = RoiShape;
            this.IoOutPort = -1;
            this.IsWait = true;
            this.AccDecParam = new AccDec();
        }

        public TrackParam(WcsROI RoiShape, enMoveType moveType)
        {
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveAxis = enAxisName.XY轴;
            this.IsActive = true;
            this.MoveType = moveType;
            this.RoiShape = RoiShape;
            this.IoOutPort = -1;
            this.IsWait = true;
            this.AccDecParam = new AccDec();
        }

    }

    [Serializable]
    public class AccDec
    {
        public double StartVel { get; set; }
        public double StopVel { get; set; }
        public double Tacc { get; set; }
        public double Tdec { get; set; }
        public double S_para { get; set; }


        public AccDec()
        {
            this.StartVel = 0;
            this.StopVel = 0;
            this.Tacc = 0.1;
            this.Tdec = 0.1;
            this.S_para = 0.1;
        }

        public override string ToString()
        {
            return string.Join(",", this.StartVel, this.StopVel, this.Tacc, this.Tdec, this.S_para);
        }


    }


}
