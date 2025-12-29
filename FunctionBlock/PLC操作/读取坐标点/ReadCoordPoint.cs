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
    [DefaultProperty(nameof(WcsPoint))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class ReadCoordPoint : BaseFunction, IFunction
    {
        private userWcsPoint _wcsPoint;

        [DisplayName("坐标点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint WcsPoint { get { return _wcsPoint; } set { _wcsPoint = value; } }
        public BindingList<ReadDataCommand> ReadDataList { get; set; }
        public ReadCoordPoint()
        {
            this.ReadDataList = new BindingList<ReadDataCommand>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            this.Result.ErrorMessage = "";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                this._wcsPoint = new userWcsPoint();
                object readValue = "";
                bool IsOk = true;
                foreach (var item in this.ReadDataList)
                {
                    if (!item.IsActive) continue;
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.X:
                            readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                            if (readValue != null)
                            {
                                double result = 0;
                                if (double.TryParse(readValue.ToString(), out result))
                                    this._wcsPoint.X = result;
                            }
                            break;
                        case enCommunicationCommand.Y:
                            readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                            if (readValue != null)
                            {
                                double result = 0;
                                if (double.TryParse(readValue.ToString(), out result))
                                    this._wcsPoint.Y = result;
                            }
                            break;
                        case enCommunicationCommand.Z:
                            readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                            if (readValue != null)
                            {
                                double result = 0;
                                if (double.TryParse(readValue.ToString(), out result))
                                    this._wcsPoint.Z = result;
                            }
                            break;
                        case enCommunicationCommand.Theta:
                            readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                            if (readValue != null)
                            {
                                double result = 0;
                                if (double.TryParse(readValue.ToString(), out result))
                                    this._wcsPoint.Theta = result;
                            }
                            break;
                    }
                }
                this.CreateResultInfo(5); // 因为要输出一个时间来，所以这里需要把 数据的数量 + 1 
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._wcsPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._wcsPoint.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Theta", this._wcsPoint.Theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                this.Result.Succss = IsOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-读取坐标数据：" + "成功");
                else
                    LoggerHelper.Error(this.name + "-读取坐标数据：" + "失败");
            }
            catch (Exception e)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "-读取坐标数据：" + "报错" + e);
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
                case nameof(this.WcsPoint):
                    return this._wcsPoint;
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
