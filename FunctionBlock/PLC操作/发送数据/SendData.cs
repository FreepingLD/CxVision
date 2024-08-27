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
using System.Diagnostics;

namespace FunctionBlock
{

    [Serializable]
    [DefaultProperty(nameof(SendContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class SendData : BaseFunction, IFunction
    {
        [NonSerialized]
        private string [] _sendContent;

        [DisplayName("发送内容")]
        [DescriptionAttribute("输出属性")]
        public string [] SendContent { get { return _sendContent; } set { _sendContent = value; } }
        //public BindingList<SendDataCommand> DataList = new BindingList<SendDataCommand>();

        public BindingList<SendDataCommand> SendDataList { get; set; }
        public SendData()
        {
            this.SendDataList = new BindingList<SendDataCommand>();
            //this.ResultInfo = new BindingList<SendDataCommand>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                bool isOk = true;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                string state = "OK";
                //////////////////////////////////////////////////////
                BindingList<SendDataCommand> sendDataList = ((BindingList<SendDataCommand>)this.SendDataList);
                foreach (var item in sendDataList)
                {
                    if (!item.IsActive) continue;
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.ResultToPlc:
                        case enCommunicationCommand.ResultToSocket:
                            state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果).ToString();
                            //foreach (var item2 in sendDataList)
                            //{
                            //    if (!item2.IsActive) continue;
                            //    switch (item2.FlagBit)
                            //    {
                            //        case enFlag.OK:
                            //        case enFlag.NG:
                            //        case enFlag.OK_NG:
                            //        case enFlag.Int1_2:
                            //        case enFlag.测量值_标准值_上偏差_下偏差_结果:
                            //        case enFlag.测量值_标准值_上偏差_结果:
                            //        case enFlag.测量值_标准值_下偏差_结果:
                            //        case enFlag.测量值_标准值_结果:
                            //        case enFlag.测量值_结果:
                            //        case enFlag.结果:
                            //        case enFlag.JudgeData:
                            //        case enFlag.测量值:
                            //            string OKNG = MemoryManager.Instance.GetValue(item2.DataSource, enFlag.结果).ToString();
                            //            if (OKNG == "NG" || OKNG == "ng")
                            //                state = "NG";
                            //            break;
                            //    }
                            //}
                            switch (item.FlagBit)
                            {
                                case enFlag.OK:
                                case enFlag.NG:
                                case enFlag.OK_NG:
                                    if (state == "NG")
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                        dic.Add(item.CommunicationCommand.ToString(),"NG");
                                    }
                                    else
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                        dic.Add(item.CommunicationCommand.ToString(), "OK");
                                    }
                                    break;
                                default:
                                case enFlag.Int1_2:
                                case enFlag.NONE:
                                    if (state == "NG")
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                        dic.Add(item.CommunicationCommand.ToString(), "2");
                                    }
                                    else
                                    {
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                        dic.Add(item.CommunicationCommand.ToString(), "1");
                                    }
                                    break;
                            }
                            break;
                        default:
                            object value;
                            switch (item.DataSource)
                            {
                                case "NONE":
                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                    break;
                                default:
                                    value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                    break;
                            }
                            if (item.CommunicationCommand != enCommunicationCommand.NONE)
                                isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, value);
                            dic.Add(item.CommunicationCommand.ToString(), value);
                            break;
                        case enCommunicationCommand.TriggerToPlc:
                            isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                            break;
                        case enCommunicationCommand.TriggerToSocket:
                            isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToSocket, 1);
                            break;
                        case enCommunicationCommand.SocketCommand:
                            isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.SocketCommand, 1);
                            break;
                    }
                }
                this.CreateResultInfo(dic.Count + 1); // 因为要输出一个时间来，所以这里需要把 数据的数量 + 1 
                this._sendContent = new string[dic.Count];// string.Join(",", list.ToArray());
                int index = 0;
                foreach (KeyValuePair<string, object> item in dic)
                {
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[index].SetValue(this.name, item.Key, item.Value.ToString());
                    this._sendContent[index] = item.Value.ToString();
                    index++;
                }
                stopwatch.Stop();
                ((BindingList<OcrResultInfo>)this.ResultInfo)[dic.Count].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                dic.Clear();
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-发送数据:" + this._sendContent + "成功");
                else
                    LoggerHelper.Error(this.name + "-发送数据:" + this._sendContent + "失败");
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "发送数据：" + "报错" + ex);
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
