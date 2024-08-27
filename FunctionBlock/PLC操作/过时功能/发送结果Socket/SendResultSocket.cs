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
    [DefaultProperty(nameof(SendContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class SendResultSocket : BaseFunction, IFunction
    {
        [NonSerialized]
        private string _sendContent;

        [DisplayName("写入内容")]
        [DescriptionAttribute("输出属性")]
        public string SendContent { get { return _sendContent; } set { _sendContent = value; } }
        public BindingList<WriteCommunicateCommand> DataList = new BindingList<WriteCommunicateCommand>();

        public SendResultSocket()
        {
            this.DataList = new BindingList<WriteCommunicateCommand>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                bool isOk = true;
                SocketCommand command = new SocketCommand(true);
                enCoordSysName coordSysName = enCoordSysName.CoordSys_0;
                List<string> listState = new List<string>();
                //////////////////////////////////////////////////////
                int index = 0;
                foreach (var item in this.DataList)
                {
                    if (index == 0)
                    {
                        coordSysName = item.CoordSysName;
                        object value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.SocketCommand);
                        command = (SocketCommand)value;
                    }
                    index++;
                    switch (item.FlagBit)
                    {
                        case enFlag.OK:
                            listState.Add("OK");
                            break;
                        case enFlag.NG:
                            listState.Add("NG");
                            break;
                        case enFlag.NONE:
                        case enFlag.JudgeData:
                        case enFlag.结果:
                        case enFlag.OK_NG:
                        case enFlag.Int1_2:
                            if (item.IsOutput)
                                listState.Add(MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果).ToString());
                            else
                                listState.Add("OK");
                            break;
                    }
                }
                // 判断结果
                foreach (var item in listState)
                {
                    if (item == "NG" || item == "ng")
                    {
                        command.GrabResult = "NG";
                        break;
                    }
                    else
                        command.GrabResult = "OK";
                }
                if(SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 如果屏蔽了强制 OK 
                    command.GrabResult = "OK";
                isOk = CommunicationConfigParamManger.Instance.WriteValue(coordSysName, command); // 向服务器写入数据
                this._sendContent = command.ToString();
                /////////////////////////////////////////////
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-写入 Socket 数据:" + this._sendContent + "成功");
                else
                    LoggerHelper.Error(this.name + "-写入 Socket 数据:" + this._sendContent + "失败");
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "写入 Socket 数据：" + "报错" + ex);
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
