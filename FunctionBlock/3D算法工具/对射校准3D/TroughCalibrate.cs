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
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    [DefaultProperty(nameof(Param))]
    public class TroughCalibrate : BaseFunction, IFunction
    {
        public TroughParam Param { get; set; }

        public TroughCalibrate()
        {
            this.Param = new TroughParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }




        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                AcqSource _laserAcqSource1 = AcqSourceManage.Instance.GetAcqSource(this.Param.LaserAcqSource1);
                AcqSource _laserAcqSource2 = AcqSourceManage.Instance.GetAcqSource(this.Param.LaserAcqSource2);
                Thread.Sleep(_laserAcqSource1.Sensor.LaserParam.WaiteTime);
                _laserAcqSource1.StartTrigger();
                _laserAcqSource2.StartTrigger();
                if (_laserAcqSource2.Card != null)
                    _laserAcqSource2.Card.SetIoOutputBit(_laserAcqSource2.Sensor.LaserParam.IoOutputMode, _laserAcqSource2.Sensor.LaserParam.TriggerPort, true);
                /////////////////
                Thread.Sleep(_laserAcqSource1.Sensor.LaserParam.AcqWaiteTime);
                if (_laserAcqSource2.Card != null)
                    _laserAcqSource2.Card.SetIoOutputBit(_laserAcqSource2.Sensor.LaserParam.IoOutputMode, _laserAcqSource2.Sensor.LaserParam.TriggerPort, false);
                _laserAcqSource1.StopTrigger();
                _laserAcqSource2.StopTrigger();
                ////// 更新激光数据
                Dictionary<enDataItem, object> list1 = _laserAcqSource1.getData();
                Dictionary<enDataItem, object> list2 = _laserAcqSource2.getData();
                double[] laser1ListValue = (double[])list1[enDataItem.Dist1];
                double[] laser2ListValue = (double[])list1[enDataItem.Dist1];
                double aveDist1 = laser1ListValue.Average();
                double aveDist2 = laser2ListValue.Average();
                if (laser1ListValue != null && laser1ListValue.Length > 0 && laser2ListValue != null && laser2ListValue.Length > 0)
                    GlobalVariable.pConfig.Cord_Gap = GlobalVariable.pConfig.StandardThickValue - (aveDist1 + aveDist2);
                this.Param.CalibValue = GlobalVariable.pConfig.Cord_Gap;
                this.Param.Laser1Value = aveDist1;
                this.Param.Laser2Value = aveDist2;
                /////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "激光1距离", aveDist1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "激光2距离", aveDist2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "标准值", this.Param.StdValue);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "标定值", this.Param.CalibValue);
                //////////// 使用发命令的方式来更新视图  
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "->对射校准完成,校准坐标 = " + GlobalVariable.pConfig.Cord_Gap.ToString());
                else
                    LoggerHelper.Error(this.name + "->对射校准失败");
                // 更改UI字体　
                UpdataNodeElementStyle(param, this.Result.Succss);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->对射校准报错" + ex);
                return Result;
            }
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case "名称":
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
                case "名称":
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
