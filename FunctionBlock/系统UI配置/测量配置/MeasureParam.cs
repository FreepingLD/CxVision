using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
   public class MeasureParam
    {

        [DisplayNameAttribute("标准值")]
        public double StdValue { get; set; }
        [DisplayNameAttribute("上偏差")]
        public double LimitUp { get; set; }
        [DisplayNameAttribute("下偏差")]
        public double LimitDown { get; set; }

        [DisplayNameAttribute("激活")]
        public bool IsActive { get; set; }

        [DisplayNameAttribute("描述")]
        public string TargetItem { get; set; }

        [DisplayNameAttribute("管控项目")]
        public string TargetProperty { get; set; }
        public MeasureParam()
        {
            this.StdValue = 0;
            this.LimitUp = 0;
            this.LimitDown = 0;
            this.IsActive = false;
            this.TargetItem = "NONE";
            this.TargetProperty = "NONE";
        }



    }



}
