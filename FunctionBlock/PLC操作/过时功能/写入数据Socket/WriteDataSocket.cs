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
    public class WriteDataSocket : BaseFunction, IFunction
    {
        [NonSerialized]
        private string _writeContent;

        [DisplayName("写入内容")]
        [DescriptionAttribute("输出属性")]
        public string WriteContent { get { return _writeContent; } set { _writeContent = value; } }
        public BindingList<WriteCommunicateCommand> DataList = new BindingList<WriteCommunicateCommand>();

        public WriteDataSocket()
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
                List<object> list = new List<object>();
                string state = "OK";
                //////////////////////////////////////////////////////
                foreach (var item in this.DataList)
                {
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.ResultToPlc:
                        case enCommunicationCommand.ResultToSocket:
                            foreach (var item2 in DataList)
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
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                    else
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                    break;
                                default:
                                case enFlag.Int1_2:
                                case enFlag.NONE:
                                    if (state == "NG")
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                    else
                                        isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                    break;
                            }
                            break;
                        case enCommunicationCommand.X:
                        case enCommunicationCommand.Path_X:
                            double value = 0;
                            RobotJawParam jawParam = RobotJawParaManager.Instance.GetJawParam(nameof(item.JawGlue));
                            switch (item.DataSource)  // 数据有两种来源，一种是给定值，一种是引用内存源
                            {
                                case "NONE": // 表示按给定的值来写入数据
                                    value = Convert.ToDouble(item.WriteValue) + jawParam.X;
                                    item.WriteValue = value.ToString();
                                    isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, value);
                                    list.Add(item.WriteValue);
                                    break;
                                default: // 表示将引用的内存数据来写入数据
                                    value = Convert.ToDouble(MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值)) + jawParam.X;
                                    item.WriteValue = value.ToString();
                                    isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, value);
                                    list.Add(item.WriteValue);
                                    break;
                            }
                            break;
                        case enCommunicationCommand.Y:
                        case enCommunicationCommand.Path_Y:
                            jawParam = RobotJawParaManager.Instance.GetJawParam(nameof(item.JawGlue));
                            switch (item.DataSource)  // 数据有两种来源，一种是给定值，一种是引用内存源
                            {
                                case "NONE": // 表示按给定的值来写入数据
                                    value = Convert.ToDouble(item.WriteValue) + jawParam.Y;
                                    item.WriteValue = value.ToString();
                                    isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, value);
                                    list.Add(item.WriteValue);
                                    break;
                                default: // 表示将引用的内存数据来写入数据
                                    value = Convert.ToDouble(MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值)) + jawParam.Y;
                                    item.WriteValue = value.ToString();
                                    isOk = isOk && CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, value);
                                    list.Add(item.WriteValue);
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
                                    list.Add(item.WriteValue);
                                    break;
                            }
                            break;
                        case enCommunicationCommand.TriggerToPlc:
                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, "");
                            break;
                        case enCommunicationCommand.TriggerToSocket:
                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToSocket, "");
                            break;
                        case enCommunicationCommand.SocketCommand:
                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.SocketCommand, "");
                            break;
                    }
                }
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-写入 Socket 数据:" + this._writeContent + "成功");
                else
                    LoggerHelper.Error(this.name + "-写入 Socket 数据:" + this._writeContent + "失败");
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
