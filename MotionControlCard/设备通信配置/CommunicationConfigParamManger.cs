using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class CommunicationConfigParamManger
    {
        private static string ParaPath = @"VisionParam\ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static CommunicationConfigParamManger _Instance;
        public static CommunicationConfigParamManger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new CommunicationConfigParamManger();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<BindingList<CommunicationConfigParam>> _CommunicationParamList;
        public BindingList<BindingList<CommunicationConfigParam>> CommunicationParamList { get => _CommunicationParamList; set => _CommunicationParamList = value; }

        public object ReadValue(CommunicationConfigParam param)
        {
            object value = "";
            double pose = 0;
            if (param == null) return value;
            if (!param.Active) return value;
            if (MotionCardManage.GetCard(param.CoordSysName) == null) return value;
            if (param?.AxisReadWriteState != enAxisReadWriteState.ReadOnly && param?.AxisReadWriteState != enAxisReadWriteState.ReadWrite) return false;
            switch (param.CommunicationCommand)
            {
                case enCommunicationCommand.X:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.X轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_X:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_X轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_X2:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_X2轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Y:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Y轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_Y:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_Y轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_Y2:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_Y2轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Z:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Z轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_Z:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_Z轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.U:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.U轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_U:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_U轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_U2:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_U2轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.V:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.V轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_V:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_V轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_V2:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_V2轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.W:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.W轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_W:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_W轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_W2:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_W2轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Theta:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Theta轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_Theta:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_Theta轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_Theta2:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_Theta2轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.DateTime:
                    value = DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                    break;
                default:
                    if (MotionCardManage.GetCard(param.CoordSysName) == null)
                        value = "";
                    else
                        value = MotionCardManage.GetCard(param.CoordSysName)?.ReadValue(param.DataType, param.Address, param.DataLength);
                    break;
            }
            return value;
        }
        public object ReadValue(enCoordSysName coordSysName)
        {
            object value = "";
            value = MotionCardManage.GetCard(coordSysName)?.ReadValue(enDataTypes.String, "", 1); // 用于Socket通信 
            return value;
        }
        public object ReadValue(enCoordSysName coordSysName, enCommunicationCommand command)
        {
            return ReadValue(GetCommunicationParam(coordSysName, command));
        }

        /// <summary>
        /// 后续将弃用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool WriteValue(CommunicationConfigParam param)
        {
            object value = "";
            bool result = false;
            if (!param.Active) return true;
            if (MotionCardManage.GetCard(param.CoordSysName) == null)
            {
                new UserMessageForm().ShowDialog($"坐标系{param.CoordSysName} 没有绑定控制设备，请在坐标系配置页面中配置!");
                return result;
            }
            if (param?.AxisReadWriteState != enAxisReadWriteState.WriteOnly && param?.AxisReadWriteState != enAxisReadWriteState.ReadWrite) return false;
            switch (param.CommunicationCommand)
            {
                case enCommunicationCommand.X:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.X轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_X:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_X轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_X2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_X2轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Y:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Y轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Y:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Y轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Y2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Y2轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Z:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Z轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Z:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Z轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.U:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.U轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_U:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_U轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_U2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_U2轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.V:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.V轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_V:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_V轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_V2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_V2轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.W:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.V轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_W:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_W轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_W2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_W2轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Theta:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Theta轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Theta:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Theta轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Theta2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Theta2轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                default:
                    if (MotionCardManage.GetCard(param.CoordSysName) == null) result = false;
                    else
                        result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, param.WriteValue, param.DataLength);
                    break;
            }
            return result;
        }
        public bool WriteValue(CommunicationConfigParam param, object writeValue)
        {
            object value = "";
            bool result = false;
            if (param == null) return result;
            if (MotionCardManage.GetCard(param.CoordSysName) == null) return result;
            if (!param.Active) return true;
            if (param?.AxisReadWriteState != enAxisReadWriteState.WriteOnly && param?.AxisReadWriteState != enAxisReadWriteState.ReadWrite) return false;
            switch (param.CommunicationCommand)
            {
                case enCommunicationCommand.X:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.X轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_X:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_X轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_X2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_X2轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Y:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Y轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Y:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Y轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Y2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Y2轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Z:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Z轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Z:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Z轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.U:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.U轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_U:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_U轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_U2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_U2轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.V:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.V轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_V:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_V轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_V2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_V2轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.W:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.W轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_W:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_W轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_W2:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_W2轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Theta:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Theta轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Theta:  // 这里一定要单独分开写，不能跟Theta轴写在一起，不然读取不了相应的地址
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Theta轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Theta2:  // 这里一定要单独分开写，不能跟Theta轴写在一起，不然读取不了相应的地址
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Theta2轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Result: // 用于上位机通信
                case enCommunicationCommand.ResultToSocket: // 用于上位机通信
                    switch (writeValue?.ToString().Trim())
                    {
                        default:
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                            break;
                        case "-1":
                        case "0":
                        case "NULL":
                        case "null":
                        case "Init":
                        case "init":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, "OK");
                            break;
                        case "1":
                        case "OK":
                        case "ok":
                            value = MotionCardManage.GetCard(param.CoordSysName).ReadValue(param.DataType, param.Address, 1);
                            if (value != null && (value.ToString().Trim() != "2" && value.ToString().Trim() != "NG"))
                                result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                            else
                                result = true;
                            break;
                        case "2":
                        case "NG":
                        case "ng":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                            break;
                        case "3":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, 3);
                            break;
                        case "Continue":
                        case "continue":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, "Continue");
                            break;
                    }
                    break;
                case enCommunicationCommand.ResultToPlc: // 用于PLC通信
                    switch (writeValue?.ToString().Trim())
                    {
                        default:
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                            break;
                        case "-1":
                        case "0":
                        case "NULL":
                        case "null":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, 1); // 表示初始化操作
                            break;
                        case "1":
                        case "OK":
                        case "ok":
                            value = MotionCardManage.GetCard(param.CoordSysName).ReadValue(param.DataType, param.Address, 1);  // 先读取这个地址中的值
                            if (value != null && (value.ToString().Trim() != "2" && value.ToString().Trim() != "NG")) // 如果地址中的值不等于2或不等于NG， 那么将写入 
                                result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                            else
                                result = true;
                            break;
                        case "2":
                        case "NG":
                        case "ng":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                            break;
                        case "Continue":
                        case "continue":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue); // 表示初始化操作
                            break;
                        case "Init":
                        case "init":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, 1); // 表示初始化操作
                            break;
                        case "3":
                            result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, 3);
                            break;
                    }
                    break;
                default:
                    if (MotionCardManage.GetCard(param.CoordSysName) == null)
                        result = false;
                    else
                        result = (bool)(MotionCardManage.GetCard(param.CoordSysName)?.WriteValue(param.DataType, param.Address, writeValue, param.DataLength));
                    break;
            }
            return result;
        }

        [Obsolete]
        /// <summary>
        /// 弃用方法
        /// </summary>
        /// <param name="coordSysName"></param>
        /// <param name="writeValue"></param>
        /// <returns></returns>
        public bool WriteValue(enCoordSysName coordSysName, object writeValue)
        {
            bool result = false;
            switch (writeValue.GetType().Name)
            {
                case nameof(SocketCommand):
                    SocketCommand command = (SocketCommand)writeValue;
                    CoordAxisConfigParam coordSysConfigParam = null;
                    SocketClientDevice clientDevice;
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(coordSysName, enAxisName.X轴);
                    clientDevice = MotionCardManage.GetCard(coordSysName) as SocketClientDevice;
                    if (coordSysConfigParam == null || clientDevice == null) return result;
                    command.X = clientDevice.MirrorAxisCoord(coordSysConfigParam, command.X);
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(coordSysName, enAxisName.Y轴);
                    if (coordSysConfigParam == null) return result;
                    command.Y = clientDevice.MirrorAxisCoord(coordSysConfigParam, command.Y);
                    result = MotionCardManage.GetCard(coordSysName).WriteValue(enDataTypes.SocketCommand, "", command);
                    break;
                default:
                    result = MotionCardManage.GetCard(coordSysName).WriteValue(enDataTypes.String, "", writeValue);
                    break;
            }
            return result;
        }

        public bool WriteValue(enCoordSysName coordSysName, enCommunicationCommand command, object writeValue)
        {
            CommunicationConfigParam communicationConfigParam = GetCommunicationParam(coordSysName, command);  // 获取该通信命令
            return WriteValue(communicationConfigParam, writeValue);
        }

        public CommunicationConfigParam GetCommunicationParam(enCoordSysName coordSysName, enCommunicationCommand command)
        {
            int index = 0;
            foreach (var item in this._CommunicationParamList)
            {
                if (index == this._CommunicationParamList.Count) break;
                foreach (var item2 in item)
                {
                    if ((item2.Active && item2.CommunicationCommand == command && item2.CoordSysName == coordSysName) ||
                        (item2.Active && item2.CommunicationCommand == command && item2.MapCoordSysName == coordSysName))
                        return item2; // 相等且是活动的
                }
                index++;
            }
            return null;
        }
        public CommunicationConfigParam[] GetCommunicationParamArray(enCommunicationCommand command)
        {
            List<CommunicationConfigParam> list = new List<CommunicationConfigParam>();
            foreach (var item in this._CommunicationParamList)
            {
                foreach (var item2 in item)
                {
                    if (item2.Active && item2.CommunicationCommand == command) list.Add(item2); // 相等且是活动的
                }
            }
            return list.ToArray();
        }


        public void Clear()
        {
            if (this._CommunicationParamList != null)
            {
                foreach (var item in this._CommunicationParamList)
                {
                    item?.Clear();
                }
            }
        }

        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<BindingList<CommunicationConfigParam>>>.Save(_CommunicationParamList, ParaPath + "\\" + "CommunicationConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            BindingList<CommunicationConfigParam> paramList = null;
            if (File.Exists(ParaPath + "\\" + "CommunicationConfigParam.xml"))
                this._CommunicationParamList = XML<BindingList<BindingList<CommunicationConfigParam>>>.Read(ParaPath + "\\" + "CommunicationConfigParam.xml");
            else
            {
                this._CommunicationParamList = new BindingList<BindingList<CommunicationConfigParam>>();
            }
            if (this._CommunicationParamList == null) // 如果读取失败，创建一个新对象 并读取一个旧的对象文件
            {
                this._CommunicationParamList = new BindingList<BindingList<CommunicationConfigParam>>();
                paramList = XML<BindingList<CommunicationConfigParam>>.Read(ParaPath + "\\" + "CommunicationConfigParam.xml");
            }
            /////////////////////////////////////////////
            if (this._CommunicationParamList.Count == 0 || this._CommunicationParamList.Count < 15)
            {
                for (int i = this._CommunicationParamList.Count; i < 15; i++)
                {
                    this._CommunicationParamList.Add(new BindingList<CommunicationConfigParam>());
                }
                if (paramList != null) // 表示有一个旧的文件存在
                    this._CommunicationParamList[0] = paramList;
            }
            //this._CommunicationParamList.Add(new BindingList<CommunicationConfigParam>());
        }





    }
}
