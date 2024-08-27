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
    public class ReduceParam
    {

        private enShapeType shapeType = enShapeType.矩形1;
        private enInsideOrOutside insideOrOutside = enInsideOrOutside.保留;

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

        [DisplayNameAttribute("操作方法")]
        public enInsideOrOutside InsideOrOutside
        {
            get
            {
                return insideOrOutside;
            }

            set
            {
                insideOrOutside = value;
            }
        }

        /// <summary>
        /// 2D 中使用像素形状
        /// </summary>
        [DisplayNameAttribute("形状参数")]
        public PixROI RoiShape { get; set; } 

        public ReduceParam()
        {

        }

        public ReduceParam(PixROI RoiShape)
        {
            this.RoiShape = RoiShape;
        }


    }



}
