using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class TrackExtractParam
    {
        private enShapeType shapeType = enShapeType.矩形1;

        [DisplayNameAttribute("形状类型")]
        public enShapeType ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        /// <summary>
        ///  3D中使用世界坐标
        /// </summary>
        [DisplayNameAttribute("形状参数")]
        public WcsROI RoiShape { get; set; }
        public TrackExtractParam()
        {

        }

        public TrackExtractParam(WcsROI RoiShape)
        {
            this.RoiShape = RoiShape;
        }


    }




}
