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
    public class LinearCalibrateParam //: CaliParam
    {
        public enCoordValueType CoordValueType { get; set; }
        public userWcsVector StartCaliPoint { get; set; }
        public userWcsVector EndCalibPoint { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }


        public LinearCalibrateParam()
        {
            this.CoordValueType = enCoordValueType.绝对坐标;
            this.RowCount = 3;
            this.ColCount = 3;
        }



    }



}
