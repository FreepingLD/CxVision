using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using Common;

namespace FunctionBlock
{

    [Serializable]
    public class SelectObjectParam
    {
        public double MinZ_range
        {
            get;
            set;
        }
        public double MaxZ_range
        {
            get;
            set;
        }
        public double Min_segment_value { get; set; }
        public double Max_segment_value { get; set; }

        public string SelectProperty  { get; set; }

        public SelectObjectParam()
        {
            this.SelectProperty = "point_coord_z"; // 默认值
        }

        public bool SelectObjectModel3D(HObjectModel3D objectModel, out HObjectModel3D selectObjectModel)
        {
            bool result = false;
            selectObjectModel = null;
            HTuple num = 0, value = 0;
            ///////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel == null || objectModel.GetObjectModel3dParams("num_points").I == 0) return result;
            value = objectModel.GetObjectModel3dParams("point_coord_z");
            if (this.Min_segment_value >= this.Max_segment_value)
            {
                this.Min_segment_value = value.TupleMin();
                this.Max_segment_value = value.TupleMax();
            }
            selectObjectModel = objectModel.SelectPointsObjectModel3d("point_coord_z", this.Min_segment_value, this.Max_segment_value);
            ///
            result = true;
            return result;
        }

        public bool SelectObjectModel3D(HObjectModel3D [] objectModel, out HObjectModel3D  [] selectObjectModel)
        {
            bool result = false;
            selectObjectModel = null;
            HTuple num = 0, value = 0;
            ///////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            value = HObjectModel3D.GetObjectModel3dParams(objectModel, "point_coord_z");
            if (this.Min_segment_value >= this.Max_segment_value)
            {
                this.Min_segment_value = value.TupleMin();
                this.Max_segment_value = value.TupleMax();
            }
            selectObjectModel = HObjectModel3D.SelectPointsObjectModel3d(objectModel, "point_coord_z", this.Min_segment_value, this.Max_segment_value);
            /////
            result = true;
            return result;
        }


    }
}
