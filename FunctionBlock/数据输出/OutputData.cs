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
    [DefaultProperty(nameof(OutPutContent))]
    public class OutputData : BaseFunction, IFunction
    {
        public static event DataSendEventHandler DataSend;
        [NonSerialized]
        private DataItem[] _outPutContent;

        [DisplayName("输出数据")]
        [DescriptionAttribute("输出属性")]
        public DataItem[] OutPutContent { get { return _outPutContent; } set { _outPutContent = value; } }

        private void OnDataSend(DataSendEventArgs e)
        {
            var eventHandler = DataSend;
            if (eventHandler != null)
                eventHandler(this, e);
        }

        public OutputData()
        {
            this.ResultInfo = new BindingList<SaveDataCommand>();

        }


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                BindingList<SaveDataCommand> SaveDataList = (BindingList<SaveDataCommand>)this.ResultInfo;
                this._outPutContent = new DataItem[SaveDataList.Count];
                int index = 0;
                foreach (var item in SaveDataList)
                {
                    if (!item.IsActive) continue;
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.NONE:
                        case enCommunicationCommand.MemoryInfo:
                            this._outPutContent[index].Describe = item.Describe;
                            this._outPutContent[index].StdValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.标准值).ToString();
                            this._outPutContent[index].LimitUpTolerance = MemoryManager.Instance.GetValue(item.DataSource, enFlag.上偏差).ToString();
                            this._outPutContent[index].LimitDownTolerance = MemoryManager.Instance.GetValue(item.DataSource, enFlag.下偏差).ToString();
                            this._outPutContent[index].Value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值).ToString();
                            this._outPutContent[index].Result = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果).ToString();
                            break;
                        default:
                            object value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand); // 读取附加信息
                            if (value != null)
                            {
                                this._outPutContent[index].Describe = item.Describe;
                                this._outPutContent[index].StdValue = "0";
                                this._outPutContent[index].LimitUpTolerance = "0";
                                this._outPutContent[index].LimitDownTolerance = "0";
                                this._outPutContent[index].Value = value.ToString();
                                this._outPutContent[index].Result = "OK";
                            }
                            else
                            {
                                this._outPutContent[index].Describe = item.Describe;
                                this._outPutContent[index].StdValue = "0";
                                this._outPutContent[index].LimitUpTolerance = "0";
                                this._outPutContent[index].LimitDownTolerance = "0";
                                this._outPutContent[index].Value = "0";
                                this._outPutContent[index].Result = "OK";
                            }
                            break;
                    }
                    index++;
                }
                this.Result.Succss = true;
                OnDataSend(new DataSendEventArgs(this._outPutContent));
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
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
                case nameof(this._outPutContent):
                    return this._outPutContent;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
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
