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
    public class ResultJudgeCommand : ResultInfo
    {
        [DisplayName("激活")]
        public bool IsActive { get; set; }

        [DisplayName("数据对象")]
        public string DataSource { get; set; }

        [DisplayName("结果")]
        public string Result { get; set; }


        public ResultJudgeCommand()
        {
            this.DataSource = "NONE";
            this.IsActive = true;
            this.Result = "false";
        }



    }
}
