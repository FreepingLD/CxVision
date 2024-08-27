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
    [DefaultProperty(nameof(ReadContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class ReadPlcData : BaseFunction, IFunction
    {
        [NonSerialized]
        private object[] _readContent;

        [DisplayName("读取内容")]
        [DescriptionAttribute("输出属性")]
        public object[] ReadContent { get { return _readContent; } set { _readContent = value; } }

        public ReadPlcData()
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
                BindingList<ReadCommunicateCommand> ReadDataList = (BindingList<ReadCommunicateCommand>)this.ResultInfo;
                foreach (var item in ReadDataList)
                {
                    switch(item.CommunicationCommand)
                    {
                        case enCommunicationCommand.NONE:
                            break;
                        case enCommunicationCommand.MemoryInfo:
                            readValue = MemoryManager.Instance.GetValue(item.DataSource,enFlag.NONE);
                            list.Add(readValue);
                            break;
                        default:
                            readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                            if (readValue != null)
                            {
                                if (item.IsCompare)
                                {
                                    if (readValue.ToString() == item.TargetValue)
                                    {
                                        item.SetValue(this.name, item.CommunicationCommand.ToString(), readValue.ToString());
                                        IsOk = IsOk && true;
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
                            break;
                    }
                }
                this.ReadContent = list.ToArray();
                list.Clear();
                this.Result.Succss = IsOk;
                // 复位触发信号
                if (this.Result.Succss)
                {
                    foreach (var item in ReadDataList)
                    {
                        if (item.IsReset)
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 0); // 复位触发信号
                        }
                    }
                }
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-读取PLC数据：" + string.Join(",", this.ReadContent) + "成功");
                else
                    LoggerHelper.Error(this.name + "-读取PLC数据：" + string.Join(",", this.ReadContent) + "失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "-读取PLC数据：" + "报错" + e);
                //return this.Result;
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
                case nameof(this.ReadContent):
                    return this.ReadContent;

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
            try
            {
                OnItemDeleteEvent(this, this.name);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "删除节点出错" + ex.ToString());
            }
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            // throw new NotImplementedException();
        }

        #endregion



    }

}
