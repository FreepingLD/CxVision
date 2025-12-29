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
    public class WriteDataCommand : ResultInfo
    {

        [DisplayName("激活")]
        public bool IsActive { get; set; }

        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("写入值")]
        public string WriteValue { get; set; }


        public WriteDataCommand()
        {
            this.CommunicationCommand = enCommunicationCommand.NONE;
            this.CoordSysName = enCoordSysName.NONE;
            this.WriteValue = "NONE";
            this.IsActive = true;
        }


    }
}
