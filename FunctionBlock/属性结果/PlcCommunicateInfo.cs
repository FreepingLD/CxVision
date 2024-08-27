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
    public class PlcCommunicateInfo : CommunicateInfo
    {
        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }
//
        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("读取值")]
        public string ReadValue { get; set; }
        
        [DisplayName("目标值")]
        public string TargetValue { get; set; }

        [DisplayName("比较")]
        public bool IsCompare { get; set; }

        [DisplayName("描述")]
        public string Describe { get; set; }

        public PlcCommunicateInfo()
        {
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.CommunicationCommand = enCommunicationCommand.NONE;
            this.Describe = "";
            this.TargetValue = "";
            this.ReadValue = "";
            this.IsCompare = true;
        }



    }
}

