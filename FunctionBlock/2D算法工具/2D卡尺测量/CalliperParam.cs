using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class CalliperParam
    {
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        public GeometryMeasure Geometry { get; set; }
        public userWcsCoordSystem WcsCoordSystem { get; set; }

        // 与运动及相机相关的一些参数
        public CoordSysAxisParam AxisPositionParam { get; set; }
        public AcqSource AcqSource { get; set; }
        public enCoordSysName CoordSysName { get; set; }

    }


    public class LineCalliperParam: CalliperParam
    {
        public userPixLine LinePixPosition { get; set; }
        public userWcsLine LineWcsPosition { get; set; }
        public LineCalliperParam()
        {
            this.Geometry = new GeometryMeasure(enMeasureType.line);
            this.WcsCoordSystem = new userWcsCoordSystem();
            this.LineWcsPosition = new userWcsLine();
            this.LinePixPosition = new userPixLine(200,200,400,200);
            //////////// 与运动及相机相关的参数
            this.AxisPositionParam = new CoordSysAxisParam(true);
            this.AcqSource = new AcqSource();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }
    }
    public class PointCalliperParam : CalliperParam
    {
        public userPixLine LinePixPosition { get; set; }
        public userWcsLine LineWcsPosition { get; set; }
        public PointCalliperParam()
        {
            this.Geometry = new GeometryMeasure(enMeasureType.point);
            this.WcsCoordSystem = new userWcsCoordSystem();
            this.LineWcsPosition = new userWcsLine();
            this.LinePixPosition = new userPixLine(200, 200, 400, 200);
            //////////// 与运动及相机相关的参数
            this.AxisPositionParam = new CoordSysAxisParam(true);
            this.AcqSource = new AcqSource();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }
    }
    public class CircleCalliperParam : CalliperParam
    {
        public userPixCircle CirclePixPosition { get; set; }
        public userWcsCircle CircleWcsPosition { get; set; }

        public CircleCalliperParam()
        {
            this.Geometry = new GeometryMeasure(enMeasureType.circle);
            this.WcsCoordSystem = new userWcsCoordSystem();
            this.CircleWcsPosition = new userWcsCircle();
            this.CirclePixPosition = new userPixCircle(300, 300, 100);// 参考圆像素坐标位置
            //////////// 与运动及相机相关的参数
            this.AxisPositionParam = new CoordSysAxisParam(true);
            this.AcqSource = new AcqSource();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }
    }
    public class CircleSectorCalliperParam : CalliperParam
    {
        public userPixCircleSector CircleSectorPixPosition { get; set; }

        public userWcsCircleSector CircleSectorWcsPosition { get; set; }

        public CircleSectorCalliperParam()
        {
            this.Geometry = new GeometryMeasure(enMeasureType.point);
            this.WcsCoordSystem = new userWcsCoordSystem();
            this.CircleSectorPixPosition = new userPixCircleSector(300, 300, 100, 0, 3.14);
            this.CircleSectorWcsPosition = new userWcsCircleSector();
            //////////// 与运动及相机相关的参数
            this.AxisPositionParam = new CoordSysAxisParam(true);
            this.AcqSource = new AcqSource();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }

    }
    public class EllipseCalliperParam : CalliperParam
    {
        public userPixEllipse EllipsePixPosition { get; set; }

        public userWcsEllipse EllipseWcsPosition { get; set; }

        public EllipseCalliperParam()
        {
            this.Geometry = new GeometryMeasure(enMeasureType.ellipse);
            this.WcsCoordSystem = new userWcsCoordSystem();
            this.EllipsePixPosition = new userPixEllipse(300, 300, 0, 100, 80);
            this.EllipseWcsPosition = new userWcsEllipse();
            //////////// 与运动及相机相关的参数
            this.AxisPositionParam = new CoordSysAxisParam(true);
            this.AcqSource = new AcqSource();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }

    }
    public class EllipseSectorCalliperParam : CalliperParam
    {
        public userPixEllipseSector EllipseSectorPixPosition { get; set; }
        public userWcsEllipseSector EllipseSectorWcsPosition { get; set; }

        public EllipseSectorCalliperParam()
        {
            this.Geometry = new GeometryMeasure(enMeasureType.ellipse);
            this.WcsCoordSystem = new userWcsCoordSystem();
            this.EllipseSectorPixPosition = new userPixEllipseSector(300, 300, 0, 100, 80, 0, 3.14);
            this.EllipseSectorWcsPosition = new userWcsEllipseSector();
            //////////// 与运动及相机相关的参数
            this.AxisPositionParam = new CoordSysAxisParam(true);
            this.AcqSource = new AcqSource();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }

    }
    public class Rect2CalliperParam : CalliperParam
    {
        public userPixRectangle2 Rect2PixPosition { get; set; }
        public userWcsRectangle2 Rect2WcsPosition { get; set; }

        public Rect2CalliperParam()
        {
            this.Geometry = new GeometryMeasure(enMeasureType.rect2);
            this.WcsCoordSystem = new userWcsCoordSystem();
            this.Rect2PixPosition = new userPixRectangle2(300, 300, 0, 100, 50);
            this.Rect2WcsPosition = new userWcsRectangle2();
            //////////// 与运动及相机相关的参数
            this.AxisPositionParam = new CoordSysAxisParam(true);
            this.AcqSource = new AcqSource();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }

    }






}
