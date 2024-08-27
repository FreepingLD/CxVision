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
using System.Reflection;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(DataLable))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class DataLable : BaseFunction, IFunction
    {
        private userTextLable[] _TextLable;

        [DisplayName("数据标签")]
        [DescriptionAttribute("输出属性")]
        public userTextLable[] TextLable
        {
            get
            {
                return _TextLable;
            }
            set
            {
                _TextLable = value;
            }
        }

        private BindingList<WriteLableCommand> _lableDataList;
        public BindingList<WriteLableCommand> LableDataList1 { get => _lableDataList; set => _lableDataList = value; }
        public DataLableParam LableParam { get; set; }


        public DataLable()
        {
            this.LableParam = new DataLableParam();
            this._lableDataList = new BindingList<WriteLableCommand>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "NG标签数量", ""));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Time(ms)", ""));
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.LableResult = "OK"; // 
            this.Result.ExcuteState = enExcuteState.NONE;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                bool isOk = true;
                object readContent = "";
                int NgLableCount = 0;
                int index = 0;
                foreach (var item in _lableDataList)
                {
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.NONE:
                            break;
                        case enCommunicationCommand.MemoryInfo:
                            readContent = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                            if (readContent != null)
                            {
                                item.WriteValue = readContent.ToString();
                                string[] desValue = item?.Describe.Split(',', ';', '，');
                                string[] readValue = readContent.ToString().Split(',', ';', ':', '，');
                                string content = "";
                                for (int i = 0; i < readValue.Length; i++)
                                {
                                    if (desValue.Length > i)
                                    {
                                        content += desValue[i] + readValue[i] + " ";
                                    }
                                    else
                                    {
                                        content += readValue[i] + " ";
                                    }
                                }
                                dic.Add(item.DataSource, content);
                            }
                            break;
                        default:
                            readContent = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                            if (readContent != null)
                            {
                                dic.Add(item.CommunicationCommand.ToString(), item.Describe + readContent.ToString());
                                item.WriteValue = readContent.ToString();
                            }
                            break;
                    }
                    index++;
                }
                this._TextLable = new userTextLable[dic.Count];
                index = 0;
                foreach (KeyValuePair<string, object> item in dic)
                {
                    string content = item.Value.ToString();
                    if (content.Contains("NG") || content.Contains("ng"))
                    {
                        NgLableCount++;
                        this.Result.LableResult = "NG";
                        this._TextLable[index] = new userTextLable(content, this.LableParam.Start_x, this.LableParam.Start_y + index * this.LableParam.Offset_y, this.LableParam.Size, "red", this.LableParam.LablePosition);
                    }
                    else
                    {
                        this._TextLable[index] = new userTextLable(content, this.LableParam.Start_x, this.LableParam.Start_y + index * this.LableParam.Offset_y, this.LableParam.Size, "green", this.LableParam.LablePosition);
                    }
                    //////////////////////////////
                    index++;
                }
                if (this._TextLable != null)
                {
                    for (int i = 0; i < this._TextLable.Length; i++)
                    {
                        OnExcuteCompleted(this.LableParam.CamName, this.LableParam.ViewName, this.name + ".标签" + (i + 0).ToString(), this._TextLable[i]);
                    }
                }
                this.CreateResultInfo(dic.Count + 2); // 因为要输出一个时间来，所以这里需要把 数据的数量 + 1 
                foreach (KeyValuePair<string, object> item in dic)
                {
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[index].SetValue(this.name, item.Key, item.Value.ToString());
                    index++;
                }
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[dic.Count].SetValue(this.name, "NG标签数量", NgLableCount);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[dic.Count + 1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                dic.Clear();
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "添加数据标签成功");
                else
                    LoggerHelper.Error(this.name + "添加数据标签失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "添加数据标签报错：" + ex);
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
