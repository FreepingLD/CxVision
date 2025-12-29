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
    public class ConditionalCommand : ResultInfo
    {
        [DisplayName("激活")]
        public bool IsActive { get; set; }

        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("数据对象")]
        public string DataSource { get; set; }

        [DisplayName("结果")]
        public string Result { get; set; }

        [DisplayName("描述")]
        public string Describe { get; set; }
        

        public ConditionalCommand()
        {
            this.CommunicationCommand = enCommunicationCommand.MemoryInfo;  // 默认从内存中读取数据
            this.CoordSysName = enCoordSysName.NONE;
            this.DataSource = "NONE";
            this.IsActive = true;
            this.Result = "OK";
        }



    }
}
