using System;
using System.ComponentModel;
using System.Reflection;

namespace FunctionBlock
{

    public class OutputDataConfigParam
    {
        [DisplayName("序号")]
        [DefaultValue("")]
        public string DataItem1 { get; set; } // 数据类型不能为 object 类型，该类型不能绑定
        [DisplayName("时间")]
        [DefaultValue("")]
        public string DataItem2 { get; set; }
        [DisplayName("测量员")]
        [DefaultValue("")]
        public string DataItem3 { get; set; }
        [DisplayName("产品尺寸")]
        [DefaultValue("")]
        public string DataItem4 { get; set; }
        [DisplayName("产品编号")]
        [DefaultValue("")]
        public string DataItem5 { get; set; }
        [DisplayName("厚度")]
        [DefaultValue("")]
        public string DataItem6 { get; set; }
        [DisplayName("距离1")]
        [DefaultValue("")]
        public string DataItem7 { get; set; }
        [DisplayName("距离2")]
        [DefaultValue("")]
        public string DataItem8 { get; set; }
        [DisplayName("X坐标")]
        [DefaultValue("")]
        public string DataItem9 { get; set; }
        [DisplayName("Y坐标")]
        [DefaultValue("")]
        public string DataItem10 { get; set; }
        //[DisplayName("数据项11")]
        //[DefaultValue("")]
        //public string DataItem11 { get; set; }
        //[DisplayName("数据项12")]
        //[DefaultValue("")]
        //public string DataItem12 { get; set; }
        //[DisplayName("数据项13")]
        //[DefaultValue("")]
        //public string DataItem13 { get; set; }
        //[DisplayName("数据项14")]
        //[DefaultValue("")]
        //public string DataItem14 { get; set; }
        //[DisplayName("数据项15")]
        //[DefaultValue("")]
        //public string DataItem15 { get; set; }
        //[DisplayName("数据项16")]
        //[DefaultValue("")]
        //public string DataItem16 { get; set; }
        //[DisplayName("数据项17")]
        //[DefaultValue("")]
        //public string DataItem17 { get; set; }
        //[DisplayName("数据项18")]
        //[DefaultValue("")]
        //public string DataItem18 { get; set; }
        //[DisplayName("数据项19")]
        //[DefaultValue("")]
        //public string DataItem19 { get; set; }
        //[DisplayName("数据项20")]
        //[DefaultValue("")]
        //public string DataItem20 { get; set; }

        //[DisplayName("数据项21")]
        //[DefaultValue("")]
        //public string DataItem21 { get; set; }
        //[DisplayName("数据项22")]
        //[DefaultValue("")]
        //public string DataItem22 { get; set; }
        //[DisplayName("数据项23")]
        //[DefaultValue("")]
        //public string DataItem23 { get; set; }
        //[DisplayName("数据项24")]
        //[DefaultValue("")]
        //public string DataItem24 { get; set; }
        //[DisplayName("数据项25")]
        //[DefaultValue("")]
        //public string DataItem25 { get; set; }

        //[DisplayName("数据项26")]
        //[DefaultValue("")]
        //public string DataItem26 { get; set; }
        //[DisplayName("数据项27")]
        //[DefaultValue("")]
        //public string DataItem27 { get; set; }
        //[DisplayName("数据项28")]
        //[DefaultValue("")]
        //public string DataItem28 { get; set; }
        //[DisplayName("数据项29")]
        //public string DataItem29 { get; set; }
        //[DisplayName("数据项30")]
        //[DefaultValue("")]
        //public string DataItem30 { get; set; }
        //[DisplayName("数据项31")]
        //[DefaultValue("")]
        //public string DataItem31 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项32")]
        //public string DataItem32 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项33")]
        //public string DataItem33 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项34")]
        //public string DataItem34 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项35")]
        //public string DataItem35 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项36")]
        //public string DataItem36 { get; set; }
        //[DisplayName("数据项37")]
        //public string DataItem37 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项38")]
        //public string DataItem38 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项39")]
        //public string DataItem39 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项40")]
        //public string DataItem40 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项41")]
        //public string DataItem41 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项42")]
        //public string DataItem42 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项43")]
        //public string DataItem43 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项44")]
        //public string DataItem44 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项45")]
        //public string DataItem45 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项46")]
        //public string DataItem46 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项47")]
        //public string DataItem47 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项48")]
        //public string DataItem48 { get; set; }
        //[DefaultValue("")]
        //[DisplayName("数据项49")]
        //public string DataItem49 { get; set; }

        //[DisplayName("数据项50")]
        //[DefaultValue("")]
        //public string DataItem50 { get; set; }

        //[DisplayName("数据项51")]
        //[DefaultValue("")]
        //public string DataItem51 { get; set; }

        public OutputDataConfigParam()
        {

        }
        public OutputDataConfigParam(params object [] values)
        {
            if(values != null)
            {
                Type type = this.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties();
                int legnth = propertyInfos.Length > values.Length ? values.Length : propertyInfos.Length; // 需要判断长度
                //////////////////////////////////////////////////////////////////
                for (int i = 0; i < legnth; i++)
                {
                    PropertyInfo propertyInfo = type.GetProperty("DataItem" + (i + 1).ToString());
                    propertyInfo.SetValue(this, values[i].ToString());
                }
            }
        }
        public OutputDataConfigParam(string[] values)
        {
            Type type = this.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            int legnth  = propertyInfos.Length > values.Length? values.Length : propertyInfos.Length; // 需要判断长度
            ////////////////////////////////
            for (int i = 0; i < legnth; i++)
            {
                PropertyInfo propertyInfo = type.GetProperty("DataItem" + (i + 1).ToString());
                propertyInfo.SetValue(this, values[i]);
            }
        }
        public  OutputDataConfigParam Clon()
        {
            OutputDataConfigParam param = new OutputDataConfigParam();
            Type typeParam = this.GetType();
            PropertyInfo[] propertyInfosParam = typeParam.GetProperties();

            Type type = this.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            ////////////////////////////////
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo propertyInfo = type.GetProperty("DataItem" + (i + 1).ToString());
                PropertyInfo propertyInfoParam = type.GetProperty("DataItem" + (i + 1).ToString());
                propertyInfoParam.SetValue(param, propertyInfo.GetValue(this)?.ToString());
            }
            return param;
        }

        public override string ToString()
        {
            return string.Join(",",this.DataItem1,this.DataItem2, this.DataItem3, this.DataItem4, this.DataItem5, this.DataItem6, this.DataItem7, this.DataItem8, this.DataItem9, this.DataItem10);
        }

    }
}
