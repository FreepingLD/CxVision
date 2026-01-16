using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Profinet.Keyence;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Siemens;


namespace MotionControlCard
{
    public class OmronFinsTcpClient : MotionBase
    {
        private string sign = "欧姆龙客户端";
        private IReadWriteNet _readWriteNet;
        private OmronFinsNet _FinsTcpNet;
        private string adress;
        public override bool Init(DeviceConnectConfigParam param)
        {
            bool result = false;
            try
            {
                //this.sign = param.ToString();
                this.ConnectConfigParam = param;
                this.name = param.DeviceName;
                OperateResult connect = new OperateResult();
                switch (param.ConnectType)
                {
                    case enUserConnectType.TcpIp:
                        this.adress = param.IpAdress;
                        switch (param.DeviceModel)
                        {
                            default:
                            case enDeviceModel.OmronFinsTcpNet:
                                this._FinsTcpNet?.ConnectClose();
                                this._FinsTcpNet = new OmronFinsNet(param.IpAdress, param.Port);
                                this._FinsTcpNet.ByteTransform.IsStringReverseByteWord = true;
                                connect = this._FinsTcpNet.ConnectServer();
                                if (connect.IsSuccess)
                                {
                                    result = true;
                                    this._readWriteNet = this._FinsTcpNet;
                                    LoggerHelper.Info(this.sign + "PLC" + param.IpAdress + "打开成功");
                                }
                                else
                                {
                                    result = false;
                                    LoggerHelper.Warn(this.sign + "PLC" + param.IpAdress + "打开失败");
                                }
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.sign + "PLC" + param.IpAdress + " 打开报错" + ex.ToString());
            }
            param.ConnectState = result;
            return result;
        }
        public override void UnInit()
        {
            try
            {
                OperateResult connect = new OperateResult();
                switch (this.ConnectConfigParam.ConnectType)
                {
                    case enUserConnectType.TcpIp:
                        this.adress = this.ConnectConfigParam.IpAdress;
                        switch (this.ConnectConfigParam.DeviceModel)
                        {
                            default:
                            case enDeviceModel.OmronFinsTcpNet:
                                this._FinsTcpNet?.ConnectClose();
                                LoggerHelper.Info(this.sign + "PLC" + this.ConnectConfigParam.IpAdress + "关闭成功");
                                break;
                        }
                        break;
                }
                LoggerHelper.Info(this.sign + "PLC" + this.adress + "关闭成功");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.sign + "PLC" + this.adress + "关闭报错" + ex.ToString());
            }
        }
        public override object ReadValue(enDataTypes dataType, string address, int length)
        {
            object value = null;
            switch (dataType)
            {
                case enDataTypes.Bool:
                    value = this._readWriteNet?.ReadBool(address).Content;
                    break;
                case enDataTypes.BoolArray:
                    value = this._readWriteNet?.ReadBool(address, (ushort)length).Content;
                    break;
                ////////////////
                case enDataTypes.Byte:
                case enDataTypes.ByteArray:
                    value = this._readWriteNet?.Read(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.Short:
                    value = this._readWriteNet?.ReadInt16(address).Content;
                    break;
                case enDataTypes.ShortArray:
                    value = this._readWriteNet?.ReadInt16(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.Ushort:
                    value = this._readWriteNet?.ReadUInt16(address).Content;
                    break;
                case enDataTypes.UshortArray:
                    value = this._readWriteNet?.ReadUInt16(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.Int:
                    value = this._readWriteNet?.ReadInt32(address).Content;
                    break;
                case enDataTypes.IntArray:
                    value = this._readWriteNet?.ReadInt32(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.UInt:
                    value = this._readWriteNet?.ReadUInt32(address).Content;
                    break;
                case enDataTypes.UIntArray:
                    value = this._readWriteNet?.ReadUInt32(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.Long:
                    value = this._readWriteNet?.ReadInt64(address).Content;
                    break;
                case enDataTypes.LongArray:
                    value = this._readWriteNet?.ReadInt64(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.ULong:
                    value = this._readWriteNet?.ReadUInt64(address).Content;
                    break;
                case enDataTypes.ULongArray:
                    value = this._readWriteNet?.ReadUInt64(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.Float:
                    value = this._readWriteNet?.ReadFloat(address).Content;
                    break;
                case enDataTypes.FloatArray:
                    value = this._readWriteNet?.ReadFloat(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.Double:
                    value = this._readWriteNet?.ReadDouble(address).Content;
                    break;
                case enDataTypes.DoubleArray:
                    value = this._readWriteNet?.ReadDouble(address, (ushort)length).Content;
                    break;
                ///////////////////////////////////
                case enDataTypes.String:
                case enDataTypes.StringArray:
                    value = this._readWriteNet?.ReadString(address, (ushort)(length), this.Encode).Content;
                    int len = 0;
                    if (value != null)
                    {
                        len = value.ToString().Length;
                        value = value.ToString().Replace("\0", "").Replace("\\0", "");
                    }
                    break;
                ///////////////////////////////////
                default:
                    return null;
            }
            return value;
        }
        public override bool WriteValue(enDataTypes dataType, string address, object value, int length = 1)
        {
            bool result = false;
            OperateResult content = new OperateResult();
            switch (dataType)
            {
                case enDataTypes.Bool:
                    content = this._readWriteNet?.Write(address, Convert.ToBoolean(value));
                    break;
                case enDataTypes.BoolArray:
                    content = this._readWriteNet?.Write(address, (bool[])(value));
                    break;
                ////////////////
                case enDataTypes.Byte:
                    content = this._readWriteNet?.Write(address, Convert.ToByte(value));
                    break;
                case enDataTypes.ByteArray:
                    content = this._readWriteNet?.Write(address, (byte[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.Short:
                    content = this._readWriteNet?.Write(address, (short)Convert.ToDouble(value));
                    break;
                case enDataTypes.ShortArray:
                    content = this._readWriteNet?.Write(address, (short[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.Ushort:
                    content = this._readWriteNet?.Write(address, (ushort)Convert.ToDouble(value));
                    break;
                case enDataTypes.UshortArray:
                    content = this._readWriteNet?.Write(address, (ushort[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.Int:
                    content = this._readWriteNet?.Write(address, Convert.ToInt32(value));
                    break;
                case enDataTypes.IntArray:
                    content = this._readWriteNet?.Write(address, (int[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.UInt:
                    content = this._readWriteNet?.Write(address, (int)Convert.ToDouble(value));
                    break;
                case enDataTypes.UIntArray:
                    content = this._readWriteNet?.Write(address, (uint[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.Long:
                    content = this._readWriteNet?.Write(address, Convert.ToInt64(value));
                    break;
                case enDataTypes.LongArray:
                    content = this._readWriteNet?.Write(address, (long[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.ULong:
                    content = this._readWriteNet?.Write(address, Convert.ToUInt64(value));
                    break;
                case enDataTypes.ULongArray:
                    content = this._readWriteNet?.Write(address, (ulong[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.Float:
                    content = this._readWriteNet?.Write(address, Convert.ToSingle(value));
                    break;
                case enDataTypes.FloatArray:
                    content = this._readWriteNet?.Write(address, (float[])(value));
                    break;
                ///////////////////////////////////
                case enDataTypes.Double:
                    content = this._readWriteNet?.Write(address, Convert.ToDouble(value));
                    break;
                case enDataTypes.DoubleArray:
                    content = this._readWriteNet?.Write(address, (double[])(value));
                    break;
                //////////////////////////////////
                case enDataTypes.String:
                case enDataTypes.StringArray:
                    string[] valueTemp = new string[length * 2]; // 一个字里包含两个字符‘\0’,所以这里需要乘以 2 
                    for (int i = 0; i < length * 2; i++)
                    {
                        valueTemp[i] = "\0";
                    }
                    // 先清空地址,再写入
                    string con = string.Join("", valueTemp);
                    this._readWriteNet?.Write(address, con, con.Length, this.Encode);
                    content = this._readWriteNet?.Write(address, value?.ToString().Trim(), this.Encode);
                    break;
                ///////////////////////////////////
                default:
                    break;
            }
            if (content == null)
            {
                LoggerHelper.Info(this.sign + "PLC:" + "PLC 没有连接，" + "数据写入失败");
                return result;
            }
            if (content.IsSuccess)
                LoggerHelper.Info(this.sign + "PLC:" + this.adress + "数据写入成功");
            else
                LoggerHelper.Info(this.sign + "PLC:" + this.adress + "数据写入失败");

            result = content.IsSuccess;
            return result;
        }
        public override bool WaiteValue(enDataTypes dataType, string address, object waitValue, int readInterval, int waitTimeout)
        {
            bool result = false;
            OperateResult<TimeSpan> content = new OperateResult<TimeSpan>();
            //try
            //{
            switch (dataType)
            {
                case enDataTypes.Bool:
                    content = this._readWriteNet?.Wait(address, Convert.ToBoolean(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.BoolArray:
                    return false;
                //break;
                ////////////////
                case enDataTypes.Byte:
                    content = this._readWriteNet?.Wait(address, Convert.ToByte(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.ByteArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.Short:
                    content = this._readWriteNet?.Wait(address, Convert.ToInt16(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.ShortArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.Ushort:
                    content = this._readWriteNet?.Wait(address, Convert.ToUInt16(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.UshortArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.Int:
                    content = this._readWriteNet?.Wait(address, Convert.ToInt32(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.IntArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.UInt:
                    content = this._readWriteNet?.Wait(address, Convert.ToUInt32(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.UIntArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.Long:
                    content = this._readWriteNet?.Wait(address, Convert.ToInt64(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.LongArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.ULong:
                    content = this._readWriteNet?.Wait(address, Convert.ToUInt64(waitValue), readInterval, waitTimeout);
                    break;
                case enDataTypes.ULongArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.Float:
                    return false;
                case enDataTypes.FloatArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.Double:
                    return false;
                case enDataTypes.DoubleArray:
                    return false;
                ///////////////////////////////////
                case enDataTypes.String:
                case enDataTypes.StringArray:
                    return false;
                ///////////////////////////////////
                default:
                    break;
            }
            if (content.IsSuccess)
                LoggerHelper.Info(sign + this.adress + "数据写入成功");
            else
                LoggerHelper.Info(sign + this.adress + "数据写入失败");
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.Error(sign + this.adress + "写入数据报错", ex);
            //}
            result = content.IsSuccess;
            return result;
        }

        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            position = 0;
            CoordAxisConfigParam coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
            if (coordSysParam == null) return;
            double pos = Convert.ToDouble(this.ReadValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), coordSysParam.DataLength));
            if (coordSysParam.InvertAxisFeedBack)
                position = pos * coordSysParam.PulseEquiv * -1;
            else
                position = pos * coordSysParam.PulseEquiv;
        }

        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisPosParam axisPosition)
        {
            CoordAxisConfigParam coordSysParam;                      //= CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(nameof(CoordSysName));
            double targetPose_x, targetPose_y, targetPose_z, targetPose_u, targetPose_v, targetPose_theta;
            switch (axisName)
            {
                case enAxisName.X轴:
                case enAxisName.Compensation_X轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = axisPosition.X / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
                    break;
                case enAxisName.Y轴:
                case enAxisName.Compensation_Y轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = axisPosition.Y / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
                    break;
                case enAxisName.Z轴:
                case enAxisName.Compensation_Z轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_z = (axisPosition.Z / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_z = axisPosition.Z / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_z);
                    break;
                case enAxisName.Theta轴:
                case enAxisName.Compensation_Theta轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_theta = (axisPosition.Theta / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_theta = axisPosition.Theta / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
                    break;
                case enAxisName.U轴:
                case enAxisName.Compensation_U轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_u = (axisPosition.U / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_u = axisPosition.U / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_u);
                    break;
                case enAxisName.V轴:
                case enAxisName.Compensation_V轴:
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_v = (axisPosition.V / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_v = axisPosition.V / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_v);
                    break;
                case enAxisName.XYZTheta轴:
                    ///////1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = axisPosition.X / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = axisPosition.Y / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_z = (axisPosition.Z / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_z = axisPosition.Z / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_z);
                    //////////4
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_theta = (axisPosition.Theta / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_theta = axisPosition.Theta / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
                    break;
                case enAxisName.Compensation_XYZTheta轴:
                    ///////1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_X轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = axisPosition.X / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Y轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = axisPosition.Y / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Z轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_z = (axisPosition.Z / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_z = axisPosition.Z / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_z);
                    //////////4
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Theta轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_theta = (axisPosition.Theta / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_theta = axisPosition.Theta / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
                    break;
                default:
                case enAxisName.XYTheta轴:
                    ////////// 1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = axisPosition.X / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = axisPosition.Y / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_theta = (axisPosition.Theta / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_theta = axisPosition.Theta / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
                    break;
                case enAxisName.Compensation_XYTheta轴:
                    ////////// 1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_X轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = axisPosition.X / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Y轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = axisPosition.Y / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Theta轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_theta = (axisPosition.Theta / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_theta = axisPosition.Theta / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
                    break;
                case enAxisName.XYZUVW轴:
                    ///////1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = axisPosition.X / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = axisPosition.Y / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_z = (axisPosition.Z / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_z = axisPosition.Z / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_z);
                    //////////4
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_theta = (axisPosition.Theta / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_theta = axisPosition.Theta / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.U轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_u = (axisPosition.U / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_u = axisPosition.U / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_u);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.V轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_v = (axisPosition.V / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_v = axisPosition.V / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_v);
                    break;
                case enAxisName.Compensation_XYZUVW轴:
                    ///////1
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_X轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_x = (axisPosition.X / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_x = axisPosition.X / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
                    //////2
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Y轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_y = (axisPosition.Y / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_y = axisPosition.Y / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Z轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_z = (axisPosition.Z / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_z = axisPosition.Z / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_z);
                    //////////4
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_Theta轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_theta = (axisPosition.Theta / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_theta = axisPosition.Theta / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_U轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_u = (axisPosition.U / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_u = axisPosition.U / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_u);
                    ///////////3
                    coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Compensation_V轴);
                    if (coordSysParam == null) return;
                    //// 转换数据
                    if (coordSysParam.InvertAxisFeedBack)
                        targetPose_v = (axisPosition.V / coordSysParam.PulseEquiv) * -1;
                    else
                        targetPose_v = axisPosition.V / coordSysParam.PulseEquiv;
                    this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_v);
                    break;
            }

        }

        public override void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            CoordAxisConfigParam coordSysParam;
            double targetPose = 0;
            coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
            if (coordSysParam == null) return;
            //// 转换数据
            if (coordSysParam.InvertAxisFeedBack)
                targetPose = (axisPosition / coordSysParam.PulseEquiv) * -1;
            else
                targetPose = axisPosition / coordSysParam.PulseEquiv;
            this.WriteValue(coordSysParam.DataType, coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose);

            //double targetPose_x, targetPose_y, targetPose_z, targetPose_u, targetPose_v, targetPose_theta;
            //switch (axisName)
            //{
            //    case enAxisName.X轴:
            //    case enAxisName.Compensation_X轴:
            //        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
            //        if (coordSysParam == null) return;
            //        //// 转换数据
            //        if (coordSysParam.InvertAxisFeedBack)
            //            targetPose_x = (axisPosition / coordSysParam.PulseEquiv) * -1;
            //        else
            //            targetPose_x = axisPosition / coordSysParam.PulseEquiv;
            //        this.WriteValue(coordSysParam.DataType,coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_x);
            //        break;
            //    case enAxisName.Y轴:
            //    case enAxisName.Compensation_Y轴:
            //        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
            //        if (coordSysParam == null) return;
            //        //// 转换数据
            //        if (coordSysParam.InvertAxisFeedBack)
            //            targetPose_y = (axisPosition / coordSysParam.PulseEquiv) * -1;
            //        else
            //            targetPose_y = axisPosition / coordSysParam.PulseEquiv;
            //        this.WriteValue(coordSysParam.DataType,coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_y);
            //        break;
            //    case enAxisName.Z轴:
            //    case enAxisName.Compensation_Z轴:
            //        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Z轴);
            //        ///////////3
            //        if (coordSysParam == null) return;
            //        //// 转换数据
            //        if (coordSysParam.InvertAxisFeedBack)
            //            targetPose_z = (axisPosition / coordSysParam.PulseEquiv) * -1;
            //        else
            //            targetPose_z = axisPosition / coordSysParam.PulseEquiv;
            //        this.WriteValue(coordSysParam.DataType,coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_z);
            //        break;
            //    case enAxisName.U轴:
            //    case enAxisName.Compensation_U轴:
            //        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.U轴);
            //        if (coordSysParam == null) return;
            //        //// 转换数据
            //        if (coordSysParam.InvertAxisFeedBack)
            //            targetPose_u = (axisPosition / coordSysParam.PulseEquiv) * -1;
            //        else
            //            targetPose_u = axisPosition / coordSysParam.PulseEquiv;
            //        this.WriteValue(coordSysParam.DataType,coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_u);
            //        break;
            //    case enAxisName.V轴:
            //    case enAxisName.Compensation_V轴:
            //        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.V轴);
            //        if (coordSysParam == null) return;
            //        //// 转换数据
            //        if (coordSysParam.InvertAxisFeedBack)
            //            targetPose_v = (axisPosition / coordSysParam.PulseEquiv) * -1;
            //        else
            //            targetPose_v = axisPosition / coordSysParam.PulseEquiv;
            //        this.WriteValue(coordSysParam.DataType,coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_v);
            //        break;
            //    case enAxisName.Theta轴:
            //    case enAxisName.Compensation_Theta轴:
            //        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
            //        if (coordSysParam == null) return;
            //        //// 转换数据
            //        if (coordSysParam.InvertAxisFeedBack)
            //            targetPose_theta = (axisPosition / coordSysParam.PulseEquiv) * -1;
            //        else
            //            targetPose_theta = axisPosition / coordSysParam.PulseEquiv;
            //        this.WriteValue(coordSysParam.DataType,coordSysParam.AdressPrefix + coordSysParam.AxisAddress.ToString().Trim(), targetPose_theta);
            //        break;
            //}
        }

    }


}
