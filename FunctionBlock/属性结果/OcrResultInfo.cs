using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class OcrResultInfo : ResultInfo
    {
        [DisplayNameAttribute("元素名称")]
        public string ElementName { get; set; }
        [DisplayNameAttribute("属性名称")]
        public string PropertyName { get; set; }

        [DisplayNameAttribute("检测值")]
        public string Mea_Value { get; set; }
        [DisplayNameAttribute("标准值")]
        public string Std_Value { get; set; }

        [DisplayNameAttribute("上偏差")]
        public double LimitUp { get; set; }
        [DisplayNameAttribute("下偏差")]
        public double LimitDown { get; set; }
        [DisplayNameAttribute("管控")]

        public bool IsOutput { get; set; }
        [DisplayNameAttribute("结果")]
        public string State { get; set; }
        public OcrResultInfo(string elementName)
        {
            this.ElementName = elementName;
            this.PropertyName = "";
            this.Mea_Value = "";
            this.Std_Value = "";
            this.IsOutput = false;
            ///////////////////////
            if (this.Std_Value == null || this.Std_Value == "" )
                this.State = "OK";
            else
            {
                if (this.Std_Value == this.Mea_Value)
                    this.State = "OK";
                else
                    this.State = "NG";
            }
        }
        public OcrResultInfo(string elementName, string propertyName, string m_value)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Mea_Value = "";
            this.Std_Value = "";
            this.IsOutput = false;
            ///////////////////////
            if (this.Std_Value == null || this.Std_Value == "")
                this.State = "OK";
            else
            {
                if (this.Std_Value == this.Mea_Value)
                    this.State = "OK";
                else
                    this.State = "NG";
            }
        }

        public OcrResultInfo()
        {
            this.ElementName = "";
            this.PropertyName = "";
            this.Mea_Value = "";
            this.Std_Value = "";
            this.IsOutput = false;
            this.State = "NG";
        }
        public void SetValue(string propertyName, string Value)
        {
            this.PropertyName = propertyName;
            this.Mea_Value = Value;
            ////////////////////////////////////////
            if (this.Std_Value == null || this.Std_Value == "")
                this.State = "OK";
            else
            {
                if (this.Std_Value == this.Mea_Value)
                    this.State = "OK";
                else
                    this.State = "NG";
            }
        }
        public void SetValue(string elementName, string propertyName, string Value)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Mea_Value = Value;
            ////////////////////////////////////////
            if (this.Std_Value == null || this.Std_Value == "")
            {
                this.State = "OK";
                //if (this.IsOutput)
                //    MemoryManager.Instance.AddValue(elementName + "." + propertyName, this.Mea_Value, this.State);
            }
            else
            {
                if (this.Std_Value == this.Mea_Value)
                    this.State = "OK";             
                else
                    this.State = "NG";
                /////////////////////////////////
                //if (this.IsOutput)
                //    MemoryManager.Instance.AddValue(elementName + "." + propertyName, this.Mea_Value, this.State);
            }
            if (this.IsOutput)
                MemoryManager.Instance.AddValue(elementName + "." + propertyName, this);
        }
        public static BindingList<OcrResultInfo> InitList(int count)
        {
            BindingList<OcrResultInfo> resultList = new BindingList<OcrResultInfo>();
            for (int i = 0; i < count; i++)
            {
                resultList.Add(new OcrResultInfo());
            }
            return resultList;
        }
        public OcrResultInfo Copy()
        {
            OcrResultInfo mm = new OcrResultInfo();
            mm.ElementName = this.ElementName;
            mm.PropertyName = this.PropertyName;
            mm.Mea_Value = this.Mea_Value;
            mm.IsOutput = this.IsOutput;
            mm.State = this.State;
            return mm;
        }
        public OcrResultInfo[] Copy(int CopyNum)
        {
            OcrResultInfo[] mm = new OcrResultInfo[CopyNum];
            for (int i = 0; i < CopyNum; i++)
            {
                mm[i].ElementName = this.ElementName;
                mm[i].PropertyName = this.PropertyName;
                mm[i].Mea_Value = this.Mea_Value;
                mm[i].IsOutput = this.IsOutput;
                mm[i].State = this.State;
            }
            return mm;
        }
        public static BindingList<OcrResultInfo> Repetition(int CopyNum, OcrResultInfo mInfo)
        {
            BindingList<OcrResultInfo> list = new BindingList<OcrResultInfo>();
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
