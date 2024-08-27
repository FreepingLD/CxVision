using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class AlignResultInfo : ResultInfo
    {
        [DisplayName("描述")]
        public string Describe { get; set; }

        [DisplayName("数据对象")]
        public string DataSource { get; set; }

        [DisplayName("坐标值")]
        public double Value { get; set; }


        public AlignResultInfo()
        {
            this.Describe = "NONE";
            this.DataSource = "NONE";
            this.Value = 0;
        }

    }
}
