using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class AcqResultInfo: ResultInfo
    {
        [DisplayNameAttribute("元素名称")]
        public string ElementName { get; set; }

        [DisplayNameAttribute("属性名称")]
        public string PropertyName { get; set; }

        [DisplayNameAttribute("当前值")]
        public string Acq_Value { get; set; }

        [DisplayNameAttribute("数量")]
        public int AcqNum { get; set; }

        [DisplayNameAttribute("管控")]
        public bool IsOutput { get; set; }

        [DisplayNameAttribute("结果")]
        public string State { get; set; }

        public AcqResultInfo(string elementName)
        {
            //this.TypeNameInfo = enResultInfoType.AcqResultInfo;
            this.ElementName = elementName;
            this.PropertyName = "";
            this.Acq_Value = "";
            this.AcqNum = 0;
            this.IsOutput = false;
            ///////////////////////
            if (this.AcqNum > 0)
                this.State = "OK";
            else
                this.State = "NG";
        }
        public AcqResultInfo(string elementName, string propertyName, double m_value)
        {
            //this.TypeNameInfo = enResultInfoType.AcqResultInfo;
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Acq_Value = "";
            this.AcqNum = 0;
            this.IsOutput = false;
            ///////////////////////
            if (this.AcqNum > 0)
                this.State = "OK";
            else
                this.State = "NG";
        }

        public AcqResultInfo()
        {
            //this.TypeNameInfo = enResultInfoType.AcqResultInfo;
            this.ElementName = "";
            this.PropertyName = "";
            this.Acq_Value = "";
            this.AcqNum = 0;
            this.IsOutput = false;
            this.State = "NG";
        }

        public void SetValue(string propertyName, double Value)
        {
            this.PropertyName = propertyName;
            this.Acq_Value = "";
            ////////////////////////////////////////
            if (this.AcqNum > 0)
                this.State = "OK";
            else
                this.State = "NG";
        }
        public void SetValue(string elementName, string propertyName, string acqValue, int acqCount)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Acq_Value = acqValue;
            this.AcqNum = acqCount;
            ////////////////////////////////////////
            if (this.AcqNum > 0)
                this.State = "OK";
            else
                this.State = "NG";
        }

        public void SetValue(string elementName, string propertyName, double acqValue, int acqCount)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Acq_Value = acqValue.ToString();
            this.AcqNum = acqCount;
            ////////////////////////////////////////
            if (this.AcqNum > 0)
                this.State = "OK";
            else
                this.State = "NG";
        }
        public static BindingList<AcqResultInfo> InitList(int count)
        {
            BindingList<AcqResultInfo> resultList = new BindingList<AcqResultInfo>();
            for (int i = 0; i < count; i++)
            {
                resultList.Add(new AcqResultInfo());
            }
            return resultList;
        }



    }
}
