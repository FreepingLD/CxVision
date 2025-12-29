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
    [DefaultProperty(nameof(WriteContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class WritePlcData : BaseFunction, IFunction
    {
        [NonSerialized]
        private object[] _writeContent;

        [DisplayName("写入内容")]
        [DescriptionAttribute("输出属性")]
        public object[] WriteContent { get { return _writeContent; } set { _writeContent = value; } }

        public WritePlcData()
        {
            this.ResultInfo = new BindingList<WriteCommunicateCommand>();
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                List<object> list = new List<object>();
                bool isOk = true;
                string state = "OK";
                BindingList<WriteCommunicateCommand> WriteDataList = ((BindingList<WriteCommunicateCommand>)this.ResultInfo);
                foreach (var item in WriteDataList)
                {
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.ResultToPlc:
                            foreach (var item2 in WriteDataList)
                            {
                                switch (item2.FlagBit)
                                {
                                    case enFlag.OK:
                                    case enFlag.NG:
                                    case enFlag.OK_NG:
                                    case enFlag.Int1_2:
                                    case enFlag.测量值_标准值_上偏差_下偏差_结果:
                                    case enFlag.测量值_标准值_上偏差_结果:
                                    case enFlag.测量值_标准值_下偏差_结果:
                                    case enFlag.测量值_标准值_结果:
                                    case enFlag.测量值_结果:
                                    case enFlag.结果:
                                    case enFlag.JudgeData:
                                        string OKNG = MemoryManager.Instance.GetValue(item2.DataSource, enFlag.结果).ToString();
                                        if (OKNG == "NG" || OKNG == "ng")
                                            state = "NG";
                                        break;
                                }
                            }
                            switch (item.FlagBit)
                            {
                                case enFlag.OK:
                                case enFlag.NG:
                                case enFlag.OK_NG:
                                    if (state == "NG")
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                        item.WriteValue = "NG";
                                    }
                                    else
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                        item.WriteValue = "OK";
                                    }
                                    break;
                                default:
                                case enFlag.Int1_2:
                                case enFlag.NONE:
                                    if (state == "NG")
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                        item.WriteValue = "2";
                                    }
                                    else
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                        item.WriteValue = "1";
                                    }

                                    break;
                            }
                            break;
                        default:
                            switch (item.DataSource)  // 数据有两种来源，一种是给定值，一种是引用内存源
                            {
                                case "NONE": // 表示按给定的值来写入数据
                                    isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, item.WriteValue);
                                    list.Add(item.WriteValue);
                                    break;
                                default: // 表示将引用的内存数据来写入数据
                                    item.WriteValue = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit).ToString();
                                    isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, item.WriteValue);
                                    break;
                            }
                            break;
                        case enCommunicationCommand.MemoryInfo:
                            item.WriteValue = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit).ToString();
                            break;
                    }
                }
                this._writeContent = list.ToArray();
                list.Clear();
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-写入PLC数据:" + string.Join(",", this._writeContent) + "成功");
                else
                    LoggerHelper.Error(this.name + "-写入PLC数据:" + string.Join(",", this._writeContent) + "失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "写入PLC数据：" + "报错" + ex);
            }
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
            // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            // throw new NotImplementedException();
        }
        #endregion




    }

}
