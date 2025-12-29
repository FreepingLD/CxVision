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
    public class WaiteDataCommand : ResultInfo
    {
        [DisplayName("激活")]
        public bool IsActive { get; set; }

        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("等待值")]
        public string WaitValue { get; set; }// 等待值 作为绑定时不能使用Object类型

        [DisplayName("等待时间")]
        public int WaitTimeout { get; set; }// 等待时间



        public WaiteDataCommand()
        {
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.CommunicationCommand = enCommunicationCommand.TriggerFromPlc;
            this.WaitValue = "-1";
            this.WaitTimeout = 10;
            this.IsActive = true;
        }


    }
}
