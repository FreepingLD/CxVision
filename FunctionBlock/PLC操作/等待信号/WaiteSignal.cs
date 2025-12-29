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
    [DefaultProperty(nameof(WaiteContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class WaiteSignal : BaseFunction, IFunction
    {
        [NonSerialized]
        private string[] _waiteContent;

        [DisplayName("等待信号")]
        [DescriptionAttribute("输出属性")]
        public string[] WaiteContent { get { return _waiteContent; } set { _waiteContent = value; } }
        public BindingList<WaiteDataCommand> WaiteDataList { get; set; }
        public WaiteSignal()
        {
            this.WaiteDataList = new BindingList<WaiteDataCommand>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
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
                Dictionary<string, object> dic = new Dictionary<string, object>();
                object readValue = "";
                bool IsOk = true;
                //BindingList<WaiteDataCommand> WaiteDataList = this.ResultInfo as  BindingList<WaiteDataCommand>;
                foreach (var item in this.WaiteDataList) 
                {
                    if (!item.IsActive) continue;
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.NONE:
                        case enCommunicationCommand.MemoryInfo:
                            break;
                        case enCommunicationCommand.DateTime:

                            break;
                        default:
                            stopwatch.Restart();
                            while (true)
                            {
                                readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                if (readValue != null && readValue.ToString().Trim() == item.WaitValue.Trim())
                                {
                                    IsOk = true;
                                    break;
                                }
                                else
                                {
                                    IsOk = false;
                                }
                                if (stopwatch.ElapsedMilliseconds >= item.WaitTimeout)
                                {
                                    IsOk = false;
                                    break;
                                }
                            }
                            break;
                    }
                    dic.Add(item.CommunicationCommand.ToString(), readValue);
                }
                this.CreateResultInfo(dic.Count + 1); // 因为要输出一个时间来，所以这里需要把 数据的数量 + 1 
                this._waiteContent = new string[dic.Count];//.Join(",", dic.ToArray());
                int index = 0;
                foreach (KeyValuePair<string, object> item in dic)
                {
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[index].SetValue(this.name, item.Key, item.Value.ToString());
                    this._waiteContent[index] = item.Value.ToString();
                    index++;
                }
                stopwatch.Stop();
                ((BindingList<OcrResultInfo>)this.ResultInfo)[dic.Count].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                dic.Clear();
                this.Result.Succss = IsOk;
                if (IsOk)
                    this.Result.ExcuteState = enExcuteState.NONE;
                else
                    this.Result.ExcuteState = enExcuteState.中断; // 没有等到需要的信号，将中断执行
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-等待PLC数据：" + string.Join(",", this.WaiteContent) + "成功");
                else
                    LoggerHelper.Error(this.name + "-等待PLC数据：" + string.Join(",", this.WaiteContent) + "失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "-等待PLC数据：" + "报错" + e);
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
