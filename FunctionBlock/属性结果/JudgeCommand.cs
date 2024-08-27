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
    public class JudgeCommand : ResultInfo
    {
        [DisplayName("激活")]
        public bool IsActive { get; set; }
        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("数据对象")]
        public string DataSource { get; set; }

        [DisplayName("操作符")]
        public enOperateSign OperateSign { get; set; }

        [DisplayName("目标值")]
        public string TargetValue { get; set; }

        [DisplayName("结果")]
        public string Result { get; set; }


        public JudgeCommand()
        {
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.CommunicationCommand = enCommunicationCommand.MemoryInfo;
            this.OperateSign = enOperateSign.等于;
            this.DataSource = "NONE";
            this.IsActive = true;
            this.TargetValue = "OK";
            this.Result = "false";
        }



    }
}
