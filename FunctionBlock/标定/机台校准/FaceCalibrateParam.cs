using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class FaceCalibrateParam //: CaliParam
    {
        public enCoordValueType CoordValueType { get; set; }
        public userWcsVector StartCaliPoint { get; set; }
        public userWcsVector EndCalibPoint { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }

        public double Offset_X { get; set; }
        public double Offset_Y { get; set; }

        public FaceCalibrateParam()
        {
            this.CoordValueType = enCoordValueType.绝对坐标;
            this.RowCount = 10;
            this.ColCount = 10;
            this.Offset_X = 2;
            this.Offset_Y = -2;
        }



    }



}
