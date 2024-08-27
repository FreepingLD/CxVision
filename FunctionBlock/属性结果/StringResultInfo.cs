using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class StringResultInfo : ResultInfo
    {
        [DisplayNameAttribute("元素名称")]
        public string ElementName { get; set; }
        [DisplayNameAttribute("属性名称")]
        public string PropertyName { get; set; }

        [DisplayNameAttribute("测量值")]
        public double Mea_Value { get; set; }
        [DisplayNameAttribute("标准值")]
        public double Std_Value { get; set; }
        [DisplayNameAttribute("上偏差")]
        public double LimitUp { get; set; }
        [DisplayNameAttribute("下偏差")]
        public double LimitDown { get; set; }
        [DisplayNameAttribute("管控")]
        public bool IsOutput { get; set; }
        [DisplayNameAttribute("结果")]
        public string State { get; set; }
        public StringResultInfo(string elementName)
        {
            this.ElementName = elementName;
            this.PropertyName = "";
            this.Mea_Value = 0;
            this.Std_Value = 0;
            this.LimitUp = 0;
            this.LimitDown = 0;
            this.IsOutput = false;
            ///////////////////////
            if (this.LimitUp == 0 && this.LimitDown == 0)
                this.State = "OK";
            else
            {
                if (this.Std_Value + this.LimitDown <= this.Mea_Value && this.Mea_Value <= this.Std_Value + this.LimitUp)
                    this.State = "OK";
                else
                    this.State = "NG";
            }
        }
        public StringResultInfo(string elementName, string propertyName, double m_value)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Mea_Value = m_value;
            this.Std_Value = 0;
            this.LimitUp = 0;
            this.LimitDown = 0;
            this.IsOutput = false;
            ///////////////////////
            if (this.LimitUp == 0 && this.LimitDown == 0)
                this.State = "OK";
            else
            {
                if (this.Std_Value + this.LimitDown <= this.Mea_Value && this.Mea_Value <= this.Std_Value + this.LimitUp)
                    this.State = "OK";
                else
                    this.State = "NG";
            }
        }
        public void CalculateResult()
        {
            if (this.LimitUp == 0 && this.LimitDown == 0)
                this.State = "OK";
            else
            {
                if (this.Std_Value + this.LimitDown <= this.Mea_Value && this.Mea_Value <= this.Std_Value + this.LimitUp)
                    this.State = "OK";
                else
                    this.State = "NG";
            }
        }
        public StringResultInfo()
        {

            this.ElementName = "";
            this.PropertyName = "";
            this.Mea_Value = 0;
            this.Std_Value = 0;
            this.LimitUp = 0;
            this.LimitDown = 0;
            this.IsOutput = false;
            this.State = "NG";
        }
        public void SetValue(string propertyName, double Value)
        {
            this.PropertyName = propertyName;
            this.Mea_Value = Value;
            ////////////////////////////////////////
            if (this.LimitUp == 0 && this.LimitDown == 0)
                this.State = "OK";
            else
            {
                switch (this.Mea_Value.GetType().Name)
                {
                    case "String":
                        this.State = "OK";
                        break;
                    default:
                        if (this.Std_Value + this.LimitDown <= this.Mea_Value && this.Mea_Value <= this.Std_Value + this.LimitUp)
                            this.State = "OK";
                        else
                            this.State = "NG";
                        break;
                }
            }
        }
        public void SetValue(string elementName, string propertyName, double Value)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Mea_Value = Value;
            ////////////////////////////////////////
            if ((this.LimitUp == 0 && this.LimitDown == 0) || !this.IsOutput)
            {
                this.State = "OK";
                //if (this.IsOutput)
                //    MemoryManager.Instance.AddValue(elementName + "." + propertyName, this.Mea_Value, this.State);
            }
            else
            {
                if (this.Std_Value + this.LimitDown <= this.Mea_Value && this.Mea_Value <= this.Std_Value + this.LimitUp)
                {
                    this.State = "OK";
                    //if (this.IsOutput)
                    //    MemoryManager.Instance.AddValue(elementName + "." + propertyName, this.Mea_Value, this.State);
                }
                else
                {
                    //if (this.IsOutput)
                    //    MemoryManager.Instance.AddValue(elementName + "." + propertyName, this.Mea_Value, this.State);
                    this.State = "NG";
                }
            }
            if (this.IsOutput)
                MemoryManager.Instance.AddValue(elementName + "." + propertyName, this); // 将整个对象添加进去
        }
        public static BindingList<StringResultInfo> InitList(int count)
        {
            BindingList<StringResultInfo> resultList = new BindingList<StringResultInfo>();
            for (int i = 0; i < count; i++)
            {
                resultList.Add(new StringResultInfo());
            }
            return resultList;
        }
        public StringResultInfo Copy()
        {
            StringResultInfo mm = new StringResultInfo();
            mm.ElementName = this.ElementName;
            mm.PropertyName = this.PropertyName;
            mm.Mea_Value = this.Mea_Value;
            mm.LimitUp = this.LimitUp;
            mm.LimitDown = this.LimitDown;
            mm.IsOutput = this.IsOutput;
            mm.State = this.State;
            return mm;
        }
        public StringResultInfo[] Copy(int CopyNum)
        {
            StringResultInfo[] mm = new StringResultInfo[CopyNum];
            for (int i = 0; i < CopyNum; i++)
            {
                mm[i].ElementName = this.ElementName;
                mm[i].PropertyName = this.PropertyName;
                mm[i].Mea_Value = this.Mea_Value;
                mm[i].LimitUp = this.LimitUp;
                mm[i].LimitDown = this.LimitDown;
                mm[i].IsOutput = this.IsOutput;
                mm[i].State = this.State;
            }
            return mm;
        }
        public static BindingList<StringResultInfo> Repetition(int CopyNum, StringResultInfo mInfo)
        {
            BindingList<StringResultInfo> list = new BindingList<StringResultInfo>();
            for (int i = 0; i < CopyNum; i++)
            {
                list.Add(mInfo.Copy());
            }
            return list;
        }

        public bool IsContainProperty(string propertyName)
        {
            Type type = this.GetType();
            if (type.GetProperty(propertyName) != null)
                return true;
            else
                return false;
        }


    }
}
