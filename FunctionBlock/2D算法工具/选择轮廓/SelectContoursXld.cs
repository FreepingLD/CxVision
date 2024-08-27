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
    public class SelectContoursXld 
    {

        private string selectContourFeatures = "contour_length";
        private double minContourFeaturesValue1 = 1;
        private double maxContourFeaturesValue1 = 200;
        private double minContourFeaturesValue2 = -0.5;
        private double maxContourFeaturesValue2 = 0.5;

        public string SelectContourFeatures
        {
            get
            {
                return selectContourFeatures;
            }

            set
            {
                selectContourFeatures = value;
            }
        }
        public double MinContourFeaturesValue1
        {
            get
            {
                return minContourFeaturesValue1;
            }

            set
            {
                minContourFeaturesValue1 = value;
            }
        }
        public double MaxContourFeaturesValue1
        {
            get
            {
                return maxContourFeaturesValue1;
            }

            set
            {
                maxContourFeaturesValue1 = value;
            }
        }
        public double MinContourFeaturesValue2
        {
            get
            {
                return minContourFeaturesValue2;
            }

            set
            {
                minContourFeaturesValue2 = value;
            }
        }
        public double MaxContourFeaturesValue2
        {
            get
            {
                return maxContourFeaturesValue2;
            }

            set
            {
                maxContourFeaturesValue2 = value;
            }
        }


        public HXLDCont select_contours_xld(HXLDCont Contours)
        {
            HXLDCont SelectedContours = null;
            if (Contours == null)
                new HXLDCont();
            SelectedContours = Contours.SelectContoursXld(selectContourFeatures, minContourFeaturesValue1, maxContourFeaturesValue1, minContourFeaturesValue2, maxContourFeaturesValue2); // 这个算子只能选择一个参数
            return SelectedContours;
        }


        public enum enSelectContourParams
        {
            contour_length,
            maximum_extent,
            direction,
            curvature,
            closed,
            open,
        }



    }
}
