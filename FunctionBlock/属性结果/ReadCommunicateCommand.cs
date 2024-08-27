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
    public class ReadCommunicateCommand : ResultInfo
    {
        [DisplayName("坐标系名")]
        public enCoordSysName CoordSysName { get; set; }

        [DisplayName("通信命令")]
        public enCommunicationCommand CommunicationCommand { get; set; }

        [DisplayName("数据对象")]
        public string DataSource { get; set; }

        [DisplayName("读取值")]
        public string ReadValue
        {
            get;
            set;
        }

        [DisplayName("目标值")]
        public string TargetValue { get; set; }
        [DisplayName("长度")]
        public int Length { get; set; }  //
        [DisplayName("比较")]
        public bool IsCompare { get; set; }
        [DisplayName("输出")]
        public bool IsOutput { get; set; }
        [DisplayName("复位")]
        public bool IsReset { get; set; }
        [DisplayName("描述")]
        public string Describe { get; set; }  //


        public ReadCommunicateCommand()
        {
            this.CommunicationCommand = enCommunicationCommand.NONE;
            this.DataSource = "";
            this.Describe = "";
            this.Length = 1;
            this.ReadValue = "";
            this.TargetValue = "";
            this.IsCompare = false;
            this.IsOutput = false;
            this.IsReset = false;
        }
        public void SetValue(string elementName, string propertyName, string Value)
        {
            this.ReadValue = Value;
            ////////////////////////////////////////
            if (this.IsOutput)
                MemoryManager.Instance.AddValue(elementName + "." + propertyName, this);
        }


    }
}

