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
    public class GlueAlignGuided : BaseFunction, IFunction
    {
        [NonSerialized]
        private string _writeContent;

        [DisplayName("写入内容")]
        [DescriptionAttribute("输出属性")]
        public string WriteContent { get { return _writeContent; } set { _writeContent = value; } }

        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector OffsetXYTheta { get; set; }

        public BindingList<WriteCommunicateCommand> DataList = new BindingList<WriteCommunicateCommand>();
        public GlueAlignGuided()
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
                RobotJawParam jawParam;
                CommunicationConfigParam CommunicationParam = null;
                SocketCommand command = new SocketCommand(true);
                enCoordSysName coordSysName = enCoordSysName.CoordSys_0;
                foreach (var item in this.DataList)
                {
                    if (CommunicationParam == null)
                    {
                        coordSysName = item.CoordSysName;
                        CommunicationParam = CommunicationConfigParamManger.Instance.GetCommunicationParam(item.CoordSysName, enCommunicationCommand.SocketCommand);
                        command = (SocketCommand)CommunicationConfigParamManger.Instance.ReadValue(CommunicationParam);
                    }
                    //////////////////////////////////////////////////////
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.X:
                            switch (item.FlagBit)
                            {
                                case enFlag.Glue1:
                                    jawParam = RobotJawParaManager.Instance.GetJawParam(nameof(enRobotJawEnum.Glue1));
                                    if (item.DataSource != "NONE")
                                        command.X = Convert.ToDouble(MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值)) + jawParam.X;
                                    else
                                    {
                                        isOk = false;
                                        MessageBox.Show("当给X坐标赋值时，必需指定相机坐标");
                                    }
                                    break;
                                case enFlag.Glue2:
                                    jawParam = RobotJawParaManager.Instance.GetJawParam(nameof(enRobotJawEnum.Glue2));
                                    if (item.DataSource != "NONE")
                                        command.X = Convert.ToDouble(MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值)) + jawParam.X;
                                    else
                                    {
                                        isOk = false;
                                        MessageBox.Show("当给X坐标赋值时，必需指定相机坐标");
                                    }
                                    break;
                            }
                            break;
                        case enCommunicationCommand.Y:
                            switch (item.FlagBit)
                            {
                                case enFlag.Glue1:
                                    jawParam = RobotJawParaManager.Instance.GetJawParam(nameof(enRobotJawEnum.Glue1));
                                    if (item.DataSource != "NONE")
                                        command.Y = Convert.ToDouble(MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值)) + jawParam.Y;
                                    else
                                    {
                                        isOk = false;
                                        MessageBox.Show("当给Y坐标赋值时，必需指定相机坐标");
                                    }
                                    break;
                                case enFlag.Glue2:
                                    jawParam = RobotJawParaManager.Instance.GetJawParam(nameof(enRobotJawEnum.Glue2));
                                    if (item.DataSource != "NONE")
                                        command.Y = Convert.ToDouble(MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值)) + jawParam.Y;
                                    else
                                    {
                                        isOk = false;
                                        MessageBox.Show("当给Y坐标赋值时，必需指定相机坐标");
                                    }
                                    break;
                            }
                            break;
                        case enCommunicationCommand.FunctionNo:
                            command.CamStation = item.WriteValue;
                            break;
                        case enCommunicationCommand.Result:
                            command.GrabResult = item.WriteValue;
                            break;
                        case enCommunicationCommand.GrabNo:
                            command.GrabNo =Convert.ToInt32(item.WriteValue);
                            break;
                    }
                }
                // 偏移值
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Tx", command.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Ty", command.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Theta", command.Theta);
                if (((BindingList<MeasureResultInfo>)this.ResultInfo)[0].State == "NG" || ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].State == "NG")
                    command.GrabResult = "NG";
                else
                    command.GrabResult = "OK";
                if (command.AlignCount == 2)
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, command); // 只有在第二次对位时才写入，向服务器写入数据
                this.WriteContent = command.ToString();
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-写入 Socket 数据:" + this.WriteContent + "->成功");
                else
                    LoggerHelper.Error(this.name + "-写入 Socket 数据:" + this.WriteContent + "->失败");
            }
            catch (Exception ex)
            {
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
