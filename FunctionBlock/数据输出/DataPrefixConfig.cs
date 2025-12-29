using System;
using System.ComponentModel;
using System.Reflection;

namespace FunctionBlock
{

    public class DataPrefixConfig
    {
        [DisplayName("数据前缀1")]
        [DefaultValue("")]
        public string DataItem1 { get; set; } // object 类型数据不能绑定

        [DisplayName("数据前缀2")]
        [DefaultValue("")]
        public string DataItem2 { get; set; }

        [DisplayName("数据前缀3")]
        [DefaultValue("")]
        public string DataItem3 { get; set; }

        [DisplayName("数据前缀4")]
        [DefaultValue("")]
        public string DataItem4 { get; set; }

        [DisplayName("数据前缀5")]
        [DefaultValue("")]
        public string DataItem5 { get; set; }
        [DisplayName("数据前缀6")]
        [DefaultValue("")]
        public string DataItem6 { get; set; }

        [DisplayName("数据前缀7")]
        [DefaultValue("")]
        public string DataItem7 { get; set; }

        [DisplayName("数据前缀8")]
        [DefaultValue("")]
        public string DataItem8 { get; set; }

        [DisplayName("数据前缀9")]
        [DefaultValue("")]
        public string DataItem9 { get; set; }

        [DisplayName("数据前缀10")]
        [DefaultValue("")]
        public string DataItem10 { get; set; }



        public DataPrefixConfig()
        {
            Type type = this.GetType();
            PropertyInfo [] propertyInfo = type.GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                propertyInfo[i].SetValue(this, "NONE");
            }
        }
      

    }
}
