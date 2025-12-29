using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    [Serializable]
    public class RectifyCalculateParam
    {
        public string ViewWindow { get; set; }
        public string Mode { get; set; }
        public string Orientation { get; set; }
        public double Angle { get; set; }
        public bool IsZoneCompensation { get; set; }
        public enCoordSysName CoordSysName { get; set; }
        public RectifyCalculateParam()
        {
            this.ViewWindow = "NONE";
            this.Mode = "自动";
            this.Orientation = "To示教位"; // To示教位,To当前位
            this.Angle = 0;
            this.IsZoneCompensation = false;
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }


    }



}
