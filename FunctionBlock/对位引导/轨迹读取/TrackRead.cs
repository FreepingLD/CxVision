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
    [DefaultProperty(nameof(ReadTrackPoint))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class TrackRead : BaseFunction, IFunction
    {
        [NonSerialized]
        private userWcsPoint[] _readTrackPoint;

        [DisplayName("轨迹点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint []  ReadTrackPoint { get { return _readTrackPoint; } set { _readTrackPoint = value; } }

        public TrackRead()
        {
            this.ResultInfo = new BindingList<ReadDataCommand>();
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                List<string> list_x = new List<string>();
                List<string> list_y = new List<string>();
                List<string> list_z = new List<string>();
                List<string> list_u = new List<string>();
                List<string> list_v = new List<string>();
                List<string> list_theta = new List<string>();
                object readValue = "";
                bool IsOk = true;
                BindingList<ReadDataCommand> ReadDataList = (BindingList<ReadDataCommand>)this.ResultInfo;
                foreach (var item in ReadDataList)
                {
                    if (!item.IsActive) continue;
                    readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand); // 这里是读取别人发过来的，并不是自身的数据
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.X:
                            if (readValue != null)
                                list_x.AddRange(readValue.ToString().Split(',',';'));
                            else
                                list_x.Add("0");
                            break;
                        case enCommunicationCommand.Y:
                            if (readValue != null)
                                list_y.AddRange(readValue.ToString().Split(',', ';'));
                            else
                                list_y.Add("0");
                            break;
                        case enCommunicationCommand.Z:
                            if (readValue != null)
                                list_z.AddRange(readValue.ToString().Split(',', ';'));
                            else
                                list_z.Add("0");
                            break;
                        case enCommunicationCommand.U:
                            if (readValue != null)
                                list_u.AddRange(readValue.ToString().Split(',', ';'));
                            else
                                list_u.Add("0");
                            break;
                        case enCommunicationCommand.V:
                            if (readValue != null)
                                list_v.AddRange(readValue.ToString().Split(',', ';'));
                            else
                                list_v.Add("0");
                            break;
                        case enCommunicationCommand.Theta:
                        case enCommunicationCommand.W:
                            if (readValue != null)
                                list_theta.AddRange(readValue.ToString().Split(',', ';'));
                            else
                                list_theta.Add("0");
                            break;
                    }
                }
                HTuple Value = new HTuple(list_x.Count, list_y.Count, list_z.Count, list_u.Count, list_v.Count, list_theta.Count).TupleSort().TupleInverse();
                int maxValue = (int)Value[0].D;
                this.ReadTrackPoint = new userWcsPoint[maxValue];
                for (int i = 0; i < maxValue; i++)
                {
                    this.ReadTrackPoint[i] = new userWcsPoint();
                    double parseValue = 0;
                    if (list_x.Count > i)
                    {
                        double.TryParse(list_x[i],out parseValue);
                        this.ReadTrackPoint[i].X = parseValue;
                    }
                    if (list_y.Count > i)
                    {
                        double.TryParse(list_y[i], out parseValue);
                        this.ReadTrackPoint[i].Y = parseValue;
                    }
                    if (list_z.Count > i)
                    {
                        double.TryParse(list_z[i], out parseValue);
                        this.ReadTrackPoint[i].Z = parseValue;
                    }
                    if (list_u.Count > i)
                    {
                        double.TryParse(list_u[i], out parseValue);
                        this.ReadTrackPoint[i].U = parseValue;
                    }
                    if (list_v.Count > i)
                    {
                        double.TryParse(list_v[i], out parseValue);
                        this.ReadTrackPoint[i].V = parseValue;
                    }
                    if (list_theta.Count > i)
                    {
                        double.TryParse(list_theta[i], out parseValue);
                        this.ReadTrackPoint[i].Theta = parseValue;
                    }
                }
                list_x.Clear();
                list_y.Clear();
                list_z.Clear();
                list_u.Clear();
                list_v.Clear();
                list_theta.Clear();
                this.Result.Succss = IsOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-读取轨迹数据：" +  "成功");
                else
                    LoggerHelper.Error(this.name + "-读取轨迹数据：" + "失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "-读取轨迹数据：" + "报错" + e);
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
                case nameof(this.ReadTrackPoint):
                    return this.ReadTrackPoint;

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
