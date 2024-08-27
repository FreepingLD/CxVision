using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    /// <summary>
    /// 类型转换器
    /// </summary>
    public class ObjectConverter<T> : ExpandableObjectConverter // 可展开的类型转换器，可将一个属性对象把包含的公开属性在PropertyGrid中展开
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value != null && value.GetType() == typeof(T))
            {
                T obj = (T)this.GetType().Module.Assembly.CreateInstance(typeof(T).ToString());

                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    string[] split = value.ToString().Split(',');
                    for (int i = 0; i < obj.GetType().GetProperties().Length; i++)
                    {
                        PropertyInfo p = obj.GetType().GetProperties()[i];
                        p.SetValue(obj, split[0]);
                    }
                }
                return obj;
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            T obj = (T)value;
            if (value != null && obj != null && destinationType == typeof(string))
            {
                string propStr = string.Empty;
                PropertyInfo[] propArray = obj.GetType().GetProperties();
                object propValue = null;

                for (int i = 0; i < propArray.Length; i++)
                {
                    propValue = propArray[i].GetValue(obj);

                    if (propValue != null)
                    {
                        if (!string.IsNullOrEmpty(propStr))
                            propStr += ",";
                        propStr += propValue.ToString();
                    }
                }
                return propStr;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    public class TestExample
    {
        [DisplayNameAttribute("测试")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ObjectConverter<Student>))]
        public Student Stu
        {
            get
            {
                return stu;
            }
            set
            {
                stu = value;
            }
        }
        private Student stu = new Student();//属性必须初始值，否则无法转换
    }

    public class Student
    {
        private string no;

        public string NO
        {
            get
            {
                return no;
            }
            set
            {
                no = value;
            }
        }
        private int age;
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }
        private string name = string.Empty;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }



}

