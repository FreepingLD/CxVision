using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ZoneCompensationParam
    {
        [DisplayNameAttribute("X补偿值")]
        public double X1 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y1 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle1 { get; set; }
        [DisplayNameAttribute("X补偿值")]
        public double X2 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y2 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle2 { get; set; }
        [DisplayNameAttribute("X补偿值")]
        public double X3 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y3 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle3 { get; set; }
        [DisplayNameAttribute("X补偿值")]
        public double X4 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y4 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle4 { get; set; }
        [DisplayNameAttribute("X补偿值")]
        public double X5 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y5 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle5 { get; set; }
        [DisplayNameAttribute("X补偿值")]
        public double X6 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y6 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle6 { get; set; }
        [DisplayNameAttribute("X补偿值")]
        public double X7 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y7 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle7 { get; set; }
        [DisplayNameAttribute("X补偿值")]
        public double X8 { get; set; }
        [DisplayNameAttribute("Y补偿值")]
        public double Y8 { get; set; }
        [DisplayNameAttribute("角度补偿值")]
        public double Angle8 { get; set; }

        [DisplayNameAttribute("启用自动补偿")]
        public bool IsAuto { get; set; }

        [DisplayNameAttribute("取反自动补偿")]
        public bool IsInvert { get; set; }

        [DisplayNameAttribute("自动补偿阈值")]
        public double Threshold { get; set; }
    }


}
