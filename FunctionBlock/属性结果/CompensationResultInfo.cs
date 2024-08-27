using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class CompensationResultInfo:ResultInfo
    {
        [DisplayNameAttribute("X补偿值")]
        public double X { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle { get; set; }

        public CompensationResultInfo()
        {
            //this.TypeNameInfo = enResultInfoType.CompensationResultInfo;
        }

    }
}
