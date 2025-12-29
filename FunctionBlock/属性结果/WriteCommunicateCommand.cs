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
    public class WriteCommunicateCommand :ResultInfo
    {
        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("数据对象")]
        public string DataSource { get; set; }

        [DisplayName("标志位")]
        public enFlag FlagBit { get; set; }

        [DisplayName("夹抓/胶枪")]
        public enRobotJawEnum JawGlue { get; set; }

        [DisplayName("写入值")]
        public string WriteValue { get; set; }

        [DisplayName("长度")]
        public int Length { get; set; }  //

        [DisplayName("输出")]
        public bool IsOutput { get; set; }

        [DisplayName("描述")]
        public string Describe { get; set; }

        public WriteCommunicateCommand ()
        {
            this.CommunicationCommand = enCommunicationCommand.NONE;
            this.CoordSysName = enCoordSysName.NONE;
            this.IsOutput = false;
            this.Describe = "";
            this.WriteValue = "";
            this.FlagBit = enFlag.NONE;
            this.DataSource = "NONE";
            this.Length = 1;
            this.JawGlue = enRobotJawEnum.NONE;
        }

        public void SetValue(string WriteValue)
        {
            this.Describe = WriteValue;
        }


    }
}
