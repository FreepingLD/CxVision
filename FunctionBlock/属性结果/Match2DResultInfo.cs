using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class Match2DResultInfo : ResultInfo
    {
        [DisplayNameAttribute("元素名称")]
        public string ElementName { get; set; }
        [DisplayNameAttribute("属性名称")]
        public string PropertyName { get; set; }

        [DisplayNameAttribute("行坐标")]
        public double Row_Value { get; set; }
        [DisplayNameAttribute("列坐标")]
        public double Col_Value { get; set; }
        [DisplayNameAttribute("角度")]
        public double Rad_Value { get; set; }
        [DisplayNameAttribute("得分")]
        public double Score_Value { get; set; }
        [DisplayNameAttribute("匹配时间")]
        public double Time { get; set; }

        [DisplayNameAttribute("结果")]
        public string State { get; set; }

        public Match2DResultInfo()
        {
            this.ElementName = "";
            this.PropertyName = "";
            this.Row_Value = 0;
            this.Col_Value = 0;
            this.Rad_Value = 0;
            this.Score_Value = 0;
            this.Time = 0;
            this.State = "OK";
        }
        public void SetValue(string elementName, string propertyName, params double[] Value)
        {
            this.ElementName = elementName;
            this.PropertyName = propertyName;
            if (Value.Length > 0)
                this.Row_Value = Value[0];
            if (Value.Length > 1)
                this.Col_Value = Value[1];
            if (Value.Length > 2)
                this.Rad_Value = Value[2];
            if (Value.Length > 3)
                this.Score_Value = Value[3];
            if (Value.Length > 4)
                this.Time = Value[4];
            this.State = "OK";
        }
        public static BindingList<Match2DResultInfo> InitList(int count)
        {
            BindingList<Match2DResultInfo> resultList = new BindingList<Match2DResultInfo>();
            for (int i = 0; i < count; i++)
            {
                resultList.Add(new Match2DResultInfo());
            }
            return resultList;
        }
        public Match2DResultInfo Copy()
        {
            Match2DResultInfo mm = new Match2DResultInfo();
            mm.ElementName = this.ElementName;
            mm.PropertyName = this.PropertyName;
            mm.Row_Value = this.Row_Value;
            mm.Col_Value = this.Col_Value;
            mm.Rad_Value = this.Rad_Value;
            mm.Score_Value = this.Score_Value;
            mm.Time = this.Time;
            mm.State = this.State;
            return mm;
        }
        public Match2DResultInfo[] Copy(int CopyNum)
        {
            Match2DResultInfo[] mm = new Match2DResultInfo[CopyNum];
            for (int i = 0; i < CopyNum; i++)
            {
                mm[i].ElementName = this.ElementName;
                mm[i].PropertyName = this.PropertyName;
                mm[i].Row_Value = this.Row_Value;
                mm[i].Col_Value = this.Col_Value;
                mm[i].Rad_Value = this.Rad_Value;
                mm[i].Score_Value = this.Score_Value;
                mm[i].Time = this.Time;
                mm[i].State = this.State;
            }
            return mm;
        }
        public static BindingList<Match2DResultInfo> Repetition(int CopyNum, Match2DResultInfo mInfo)
        {
            BindingList<Match2DResultInfo> list = new BindingList<Match2DResultInfo>();
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
