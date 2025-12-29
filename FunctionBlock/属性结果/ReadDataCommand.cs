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
    public class ReadDataCommand : ResultInfo
    {
        [DisplayName("激活")]
        public bool IsActive { get; set; }

        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("读取值")]
        public string ReadValue { get; set; }

        [DisplayName("管控")]
        public bool IsOutput { get; set; }
 

        public ReadDataCommand()
        {
            this.CommunicationCommand = enCommunicationCommand.NONE;
            this.CoordSysName = enCoordSysName.NONE;
            this.ReadValue = "NONE";
            this.IsActive = true;
            this.IsOutput = false;
        }



    }
}
