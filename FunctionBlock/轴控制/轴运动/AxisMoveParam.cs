using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class AxisMoveParam
    {
        public bool IsActive { get; set; } // 是否触发传感器

        [DisplayNameAttribute("坐标系名字")]
        public Common.enCoordSysName CoordSysName { get; set; }

        [DisplayNameAttribute("轨迹类型")]
        public enMoveType MoveType { get; set; } // 运动方式 

        /// <summary>
        /// 2D 中使用像素形状
        /// </summary>
        [DisplayNameAttribute("轨迹参数")]
        public WcsROI RoiShape { get; set; }

        public AxisMoveParam()
        {

        }

        public AxisMoveParam(WcsROI RoiShape)
        {
            this.IsActive = true;
            this.MoveType = enMoveType.点位运动;
            this.RoiShape = RoiShape;
        }

        public AxisMoveParam(WcsROI RoiShape, enMoveType moveType)
        {
            this.IsActive = true;
            this.MoveType = moveType;
            this.RoiShape = RoiShape;
        }

    }


}
