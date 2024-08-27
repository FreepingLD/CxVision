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
    public class SaveDataCommand : ResultInfo
    {
        [DisplayName("激活")]
        public bool IsActive { get; set; }

        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("数据对象")]
        public string DataSource { get; set; }

        //[DisplayName("标志位")]
        //public enFlag FlagBit { get; set; }

        [DisplayName("保存值")]
        public string SaveValue { get; set; }

        [DisplayName("描述")]
        public string Describe { get; set; }
        

        public SaveDataCommand()
        {
            this.CommunicationCommand = enCommunicationCommand.MemoryInfo;
            this.CoordSysName = enCoordSysName.NONE;
            //this.FlagBit = enFlag.NONE;
            this.DataSource = "NONE";
            this.IsActive = true;
            this.SaveValue = "";
        }


    }
}
