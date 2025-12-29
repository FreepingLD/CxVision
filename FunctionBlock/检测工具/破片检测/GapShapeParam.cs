using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;

namespace FunctionBlock
{
    [Serializable]
    public class GapShapeParam
    {
        private enShapeType shapeType = enShapeType.多边形;
        private enDetectMethod _imageLable = enDetectMethod.边缘检测;

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

        [DisplayNameAttribute("图像标签")]
        public enDetectMethod DetectMethod
        {
            get
            {
                return _imageLable;
            }

            set
            {
                _imageLable = value;
            }
        }

        /// <summary>
        /// 2D 中使用像素形状
        /// </summary>
        [DisplayNameAttribute("形状参数")]
        public PixROI RoiShape { get; set; } 

        public GapShapeParam()
        {

        }

        public GapShapeParam(PixROI RoiShape)
        {
            this.RoiShape = RoiShape;
        }


    }

    public enum enDetectMethod
    {
        区域检测,
        边缘检测,
        裂纹检测,
    }

}
