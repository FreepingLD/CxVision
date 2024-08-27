using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class MeasureResultInfo : ResultInfo
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
        [DisplayNameAttribute("缩放")]
        public double Scale { get; set; }

        [DisplayNameAttribute("补偿值")]
        public double Compensate { get; set; }

        [DisplayNameAttribute("管控")]
        public bool IsOutput { get; set; }
        [DisplayNameAttribute("结果")]
        public string State { get; set; }


        public MeasureResultInfo(string elementName)
        {
            this.ElementName = elementName;
            this.PropertyName = "";
            this.Mea_Value = 0;
            this.Std_Value = 0;
            this.LimitUp = 0;
            this.LimitDown = 0;
            this.Scale = 1;
            this.IsOutput = false;
            this.Compensate = 0;
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
        public MeasureResultInfo(string elementName, string propertyName, double m_value = 0)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            this.Mea_Value = m_value * this.Scale + Compensate;
            this.Std_Value = 0;
            this.LimitUp = 0;
            this.LimitDown = 0;
            this.Scale = 1;
            this.IsOutput = false;
            this.Compensate = 0;
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
        public MeasureResultInfo()
        {
            this.ElementName = "";
            this.PropertyName = "";
            this.Mea_Value = 0;
            this.Std_Value = 0;
            this.LimitUp = 0;
            this.LimitDown = 0;
            this.Scale = 1;
            this.IsOutput = false;
            this.State = "NG";
            this.Compensate = 0;
        }
        public void SetValue(string propertyName, double Value)
        {
            this.PropertyName = propertyName;
            this.Mea_Value = Math.Round(Value * this.Scale + Compensate, 4);
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
            this.Mea_Value = Math.Round(Value * this.Scale + Compensate, 4);
            //////////////////////////////////////// 实现全局参数的配置  ////////////
            foreach (var item in MeasureManager.Instance.MeasureList)
            {
                if (!this.IsOutput) continue; // 不管控的项 ，不往下执行
                bool isContains = true;
                string[] itemName = item.TargetItem.Split(',', ';', ':','.');
                foreach (var item22 in itemName) // 测试 elementName 是否包含有指定的元素
                {
                    if (!elementName.Contains(item22))
                        isContains = false;
                }
                /////////////////////////////////////////////////////
                if (item.IsActive && this.IsOutput && isContains && propertyName == item.TargetProperty)
                {
                    if (this.Std_Value != item.StdValue)
                        this.Std_Value = item.StdValue;
                    if (this.LimitUp != item.LimitUp)
                        this.LimitUp = item.LimitUp;
                    if (this.LimitDown != item.LimitDown)
                        this.LimitDown = item.LimitDown;
                }
            }
            //////////////////////////////////
            if ((this.LimitUp == 0 && this.LimitDown == 0) || !this.IsOutput)
            {
                this.State = "OK";
            }
            else
            {
                if (this.Std_Value + this.LimitDown <= this.Mea_Value && this.Mea_Value <= this.Std_Value + this.LimitUp)
                {
                    this.State = "OK";
                }
                else
                {
                    this.State = "NG";
                }
            }
            /////////////////////////////
            if (this.IsOutput)
                MemoryManager.Instance.AddValue(elementName + "." + propertyName, this); // 将整个对象添加进去
        }
        public static BindingList<MeasureResultInfo> InitList(int count)
        {
            BindingList<MeasureResultInfo> resultList = new BindingList<MeasureResultInfo>();
            for (int i = 0; i < count; i++)
            {
                resultList.Add(new MeasureResultInfo());
            }
            return resultList;
        }
        public MeasureResultInfo Copy()
        {
            MeasureResultInfo mm = new MeasureResultInfo();
            mm.ElementName = this.ElementName;
            mm.PropertyName = this.PropertyName;
            mm.Mea_Value = this.Mea_Value;
            mm.LimitUp = this.LimitUp;
            mm.LimitDown = this.LimitDown;
            mm.Scale = this.Scale;
            mm.IsOutput = this.IsOutput;
            mm.State = this.State;
            mm.Compensate = this.Compensate;
            return mm;
        }
        public MeasureResultInfo[] Copy(int CopyNum)
        {
            MeasureResultInfo[] mm = new MeasureResultInfo[CopyNum];
            for (int i = 0; i < CopyNum; i++)
            {
                mm[i].ElementName = this.ElementName;
                mm[i].PropertyName = this.PropertyName;
                mm[i].Mea_Value = this.Mea_Value;
                mm[i].LimitUp = this.LimitUp;
                mm[i].LimitDown = this.LimitDown;
                mm[i].Scale = this.Scale;
                mm[i].IsOutput = this.IsOutput;
                mm[i].State = this.State;
                mm[i].Compensate = this.Compensate;
            }
            return mm;
        }
        public static BindingList<MeasureResultInfo> Repetition(int CopyNum, MeasureResultInfo mInfo)
        {
            BindingList<MeasureResultInfo> list = new BindingList<MeasureResultInfo>();
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

        public override string ToString()
        {
            return string.Join(",", "测量值:", Math.Round(this.Mea_Value, 3), ";", "标准值:", this.Std_Value, ";", "上偏差:", this.LimitUp, ";", "下偏差:", this.LimitDown);
        }


    }
}
