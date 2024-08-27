using Common;
using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class UserDefinedCard : MotionBase
    {
        private Dictionary<string, object> _readWriteNet; // 虚拟地址
        public override bool Init(DeviceConnectConfigParam param)
        {
            bool result = false;
            try
            {
                this.name = param.DeviceName;
                _readWriteNet = new Dictionary<string, object>();
                for (int i = 0; i < 100; i++) // 添加虚拟地址
                {
                    _readWriteNet.Add("C" + (100 + i).ToString(), 0);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error("自定义控制卡" + param.IpAdress + "打开报错" + ex.ToString());
            }
            param.ConnectState = result;
            return result;
        }
        public override void UnInit()
        {
            try
            {
                this._readWriteNet.Clear();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(" 自定义控制卡" + "关闭报错" + ex.ToString());
            }
        }
        public override object ReadValue(enDataTypes dataType, string address, int length = 1)
        {
            object value = null;
            if (this._readWriteNet.ContainsKey(address))
                value = this._readWriteNet[address];
            return value;
        }
        public override bool WriteValue(enDataTypes dataType, string address, object value)
        {
            bool result = false;
            if (this._readWriteNet.ContainsKey(address))
            {
               this._readWriteNet[address] = value;
                result = true;
            }         
            return result;
        }
        public override bool WaiteValue(enDataTypes dataType, string address, object waitValue, int readInterval, int waitTimeout)
        {
            bool result = false;
            if (this._readWriteNet.ContainsKey(address))
            {
                if (this._readWriteNet[address] == waitValue)
                    result = true;
            }
            return result;
        }

        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            position = 0;
            CoordSysConfigParam coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
            if (coordSysParam == null) return;
            position = Convert.ToDouble(this.ReadValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), coordSysParam.DataLength)) * coordSysParam.PulseEquiv;
        }

        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            CoordSysConfigParam coordSysParam;//= CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(nameof(CoordSysName));
            switch (axisName)
            {
                case enAxisName.X轴:
                case enAxisName.Y轴:
                case enAxisName.Z轴:
                case enAxisName.Theta轴:
                    //if (axisPosition.Length < 1) throw new ArgumentException("给定的值长度小于要移动的轴数量");
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.X);
                    break;
                case enAxisName.XYZTheta轴:
                    //if (axisPosition.Length < 4) throw new ArgumentException("给定的值长度小于要移动的轴数量");
                    ///////1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.X);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.Y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.Z);
                    //////////4
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.Theta);
                    break;
                default:
                case enAxisName.XYTheta轴:
                   // if (axisPosition.Length < 3) throw new ArgumentException("给定的值长度小于要移动的轴数量");
                    ////////// 1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.X);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.Y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysParam == null) return;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString(), axisPosition.Theta);
                    break;
            }

        }


    }
}
