using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("DataHandle3D")]
    public class SaveData : BaseFunction, IFunction
    {

        public SaveDataParam SaveParam { get; set; }
        public double[] _InputData;

        [DisplayName("输入数据")]
        [DescriptionAttribute("输入属性1")]
        public double [] InputData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        List<double> list = new List<double>();
                        double result = 0;  
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        foreach ( object item in oo )
                        {
                            if(double.TryParse(item.ToString(),out result))
                                list.Add(result);       
                        }
                        _InputData  = list.ToArray();
                        list.Clear();       
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _InputData;
            }
            set
            {
                _InputData = value;
            }
        }

        public SaveData()
        {
            this.SaveParam = new SaveDataParam();
        }




        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = this.SaveParam.SaveFileData(this.InputData);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                    break;
            }
            return null;
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                default:
                    return true;
            }
        }

        public void ReleaseHandle()
        {
            try
            {
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

        #endregion


    }
}
