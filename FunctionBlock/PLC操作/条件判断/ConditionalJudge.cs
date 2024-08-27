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
    [DefaultProperty(nameof(Content))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class ConditionalJudge : BaseFunction, IFunction
    {
        [NonSerialized]
        private string _content;

        [DisplayName("判断内容")]
        [DescriptionAttribute("输出属性")]
        public string Content { get { return _content; } set { _content = value; } }

        public BindingList<JudgeCommand> DataList { get; set; }
        public ConditionalJudge()
        {
            this.DataList = new BindingList<JudgeCommand>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "结果", ""));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Time(ms)",""));
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
                this._content = "OK";
                //////////////////////////////////////////////////////
                BindingList<JudgeCommand> sendDataList = ((BindingList<JudgeCommand>)this.DataList);
                foreach (var item in sendDataList)
                {
                    if (!item.IsActive) continue;
                    string value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值).ToString();
                    switch (item.OperateSign)
                    {
                        case enOperateSign.等于:
                            if (value.Trim() == item.TargetValue.Trim())
                                item.Result = "true";
                            else
                                item.Result = "false";
                            break;
                        case enOperateSign.不等于:
                            if (value.Trim() != item.TargetValue.Trim())
                                item.Result = "true";
                            else
                                item.Result = "false";
                            break;
                        case enOperateSign.大于:
                            double targetVale = 0, curValue = 0;
                            double.TryParse(value.Trim(), out curValue);
                            double.TryParse(item.TargetValue.Trim(), out targetVale);
                            if (curValue > targetVale)
                                item.Result = "true";
                            else
                                item.Result = "false";
                            break;
                        case enOperateSign.小于:
                            targetVale = 0;
                            curValue = 0;
                            double.TryParse(value.Trim(), out curValue);
                            double.TryParse(item.TargetValue.Trim(), out targetVale);
                            if (curValue < targetVale)
                                item.Result = "true";
                            else
                                item.Result = "false";
                            break;
                    }
                }
                ///////////////////////////////////
                foreach (var item in sendDataList)
                {
                    if (item.Result == "false")
                        this._content = "NG";
                }
                stopwatch.Stop();
                if (this._content == "NG")
                    this.Result.ExcuteState = enExcuteState.中断;
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "结果", this._content);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-条件判断:" + this._content + "成功");
                else
                    LoggerHelper.Error(this.name + "-条件判断:" + this._content + "失败");
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "条件判断：" + "报错" + ex);
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
