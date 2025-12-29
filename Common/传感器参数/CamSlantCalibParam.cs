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
    public class CamSlantCalibParam //: CaliParam
    {
        public enCoordValueType CoordValueType { get; set; }
        public enScanAxis MoveAxis { get; set; }
        public userWcsVector StartCaliPoint { get; set; }
        public userWcsVector EndCalibPoint { get; set; }
        public double MoveStep { get; set; }
        public double CamSlantDeg { get; set; }




        public CamSlantCalibParam()
        {
            this.MoveAxis = enScanAxis.X轴;
            this.CoordValueType = enCoordValueType.绝对坐标;
            this.StartCaliPoint = new userWcsVector();
            this.EndCalibPoint = new userWcsVector();
        }

    }



}
