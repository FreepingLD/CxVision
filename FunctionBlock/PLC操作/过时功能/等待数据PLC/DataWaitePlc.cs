using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WaiteContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class DataWaitePlc : BaseFunction, IFunction
    {
        //public BindingList<ReadCommunicateCommand> WaiteAdress { get; set; }

        [DisplayName("等待数据")]
        [DescriptionAttribute("输出属性")]
        public object[] WaiteContent { get; set; }


        public DataWaitePlc()
        {
            this.ResultInfo = new BindingList<ReadCommunicateCommand>();
        }





        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                List<object> list = new List<object>();
                object readValue = "";
                bool IsOk = true;
                BindingList<ReadCommunicateCommand> WaiteDataList = (BindingList<ReadCommunicateCommand>)this.ResultInfo;
                ///////////////////////////////////////////////////////////////
                foreach (var item in WaiteDataList)
                {
                    readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                    if (readValue != null)
                    {
                        if (item.IsCompare)
                        {
                            if (readValue.ToString().Trim() == item.TargetValue.Trim())
                            {
                                IsOk = IsOk && true;
                                item.SetValue(this.name, item.CommunicationCommand.ToString(), readValue.ToString());
                            }
                            else
                            {
                                IsOk = IsOk && false;
                            }
                        }
                        else
                        {
                            item.SetValue(this.name, item.CommunicationCommand.ToString(), readValue.ToString());
                            IsOk = IsOk && true;
                        }
                        list.Add(readValue);
                    }
                    else
                    {
                        list.Add("NULL");
                    }
                }
                this.WaiteContent = list.ToArray();
                list.Clear();
                this.Result.Succss = IsOk;
                // 使用发命令的方式来更新视图  
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "等待数据：" + string.Join(",", this.WaiteContent) + "成功");
                else
                    LoggerHelper.Error(this.name + "等待数据：" + string.Join(",", this.WaiteContent) + "失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "等待数据：" + "报错" + e);
                this.Result.Succss = false;
            }
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "值":
                case nameof(this.WaiteContent):
                    return this.WaiteContent;

                case "名称":
                case nameof(this.Name):
                default:
                    return this.name;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
                    return true;
            }
        }

        public void ReleaseHandle()
        {

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
