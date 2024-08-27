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

namespace FunctionBlock
{
    [Serializable]
    public class SelectShapeXld 
    {
        private string selectShapeFeatures = "area";
        private double minShapeFeaturesValue = 150;
        private double maxShapeFeaturesValue = 999999;
        private string shapeOperation = "and";


        public string SelectShapeFeatures
        {
            get
            {
                return selectShapeFeatures;
            }

            set
            {
                selectShapeFeatures = value;
            }
        }
        public double MinShapeFeaturesValue
        {
            get
            {
                return minShapeFeaturesValue;
            }

            set
            {
                minShapeFeaturesValue = value;
            }
        }
        public double MaxShapeFeaturesValue
        {
            get
            {
                return maxShapeFeaturesValue;
            }

            set
            {
                maxShapeFeaturesValue = value;
            }
        }
        public string ShapeOperation
        {
            get
            {
                return shapeOperation;
            }

            set
            {
                shapeOperation = value;
            }
        }


        public HXLDCont select_shape_xld(HXLDCont Contours)
        {
            HXLDCont SelectedXLD = null;
            if (Contours == null)
                new HXLDCont();
            SelectedXLD = Contours.SelectShapeXld(SelectShapeFeatures, ShapeOperation, MinShapeFeaturesValue, MaxShapeFeaturesValue); // 这里可以设置多个参数
            return SelectedXLD;
        }

        public enum enSelectShapeXLDParams
        {
            area,
            area_points,
            row,
            column,
            row_points,
            column_points,
            width,
            height,
            row1,
            column1,
            row2,
            column2,
            circularity,
            compactness,
            contlength,
            convexity,
            ra,
            rb,
            phi,
            ra_points,
            rb_points,
            phi_points,
            anisometry,
            anisometry_points,
            bulkiness,
            struct_factor,
            outer_radius,
            max_diameter,
            orientation,
            orientation_points,
            rect2_phi,
            rect2_len1,
            rect2_len2,
            moments_m11,
            moments_m20,
            moments_m02,
            moments_m11_points,
            moments_m20_points,
            moments_m02_points,
        }

        public enum enSelectShapeOperation
        {
            and,
            or
        }

    }
}
