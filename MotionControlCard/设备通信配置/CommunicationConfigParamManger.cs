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
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
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
            if (param == null) return value;  //throw new ArgumentNullException("param");
            if (!param.Active) return value;
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
                case enCommunicationCommand.Y:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Y轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_Y:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_Y轴, out pose);
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
                case enCommunicationCommand.V:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.V轴, out pose);
                    value = pose;
                    break;
                case enCommunicationCommand.Compensation_V:
                    MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_V轴, out pose);
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
                case enCommunicationCommand.DateTime:
                    //MotionCardManage.GetCard(param.CoordSysName).GetAxisPosition(param.CoordSysName, enAxisName.Compensation_Theta轴, out pose);
                    value = DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
                    break;
                default:
                    value = MotionCardManage.GetCard(param.CoordSysName).ReadValue(param.DataType, param.Address, param.DataLength);
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
            if (param?.AxisReadWriteState != enAxisReadWriteState.WriteOnly && param?.AxisReadWriteState != enAxisReadWriteState.ReadWrite) return false;
            switch (param.CommunicationCommand)
            {
                case enCommunicationCommand.X:
                case enCommunicationCommand.Compensation_X:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.X轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Y:
                case enCommunicationCommand.Compensation_Y:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Y轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Z:
                case enCommunicationCommand.Compensation_Z:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Z轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.U:
                case enCommunicationCommand.Compensation_U:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.U轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.V:
                case enCommunicationCommand.Compensation_V:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.V轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                case enCommunicationCommand.Theta:
                case enCommunicationCommand.Compensation_Theta:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Theta轴, 10, Convert.ToDouble(param.WriteValue));
                    result = true;
                    break;
                default:
                    result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, param.WriteValue);
                    break;
            }
            return result;
        }
        public bool WriteValue(CommunicationConfigParam param, object writeValue)
        {
            object value = "";
            bool result = false;
            if (param == null) return result;
            //throw new ArgumentNullException("param");
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
                case enCommunicationCommand.Y:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Y轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_Y:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_Y轴, 10, Convert.ToDouble(writeValue));
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
                case enCommunicationCommand.V:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.V轴, 10, Convert.ToDouble(writeValue));
                    result = true;
                    break;
                case enCommunicationCommand.Compensation_V:
                    MotionCardManage.GetCard(param.CoordSysName).MoveSingleAxis(param.CoordSysName, enAxisName.Compensation_V轴, 10, Convert.ToDouble(writeValue));
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
                case enCommunicationCommand.ResultToSocket: // 用于上位机通信
                    value = MotionCardManage.GetCard(param.CoordSysName).ReadValue(param.DataType, param.Address, 1);  // 先读取这个地址中的值
                    if (value != null && value.ToString().Trim() != "NG") // 如果地址中的值不等于 2， 那么将写入 
                        result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                    else
                        result = true;
                    break;
                case enCommunicationCommand.ResultToPlc: // 用于PLC通信
                    value = MotionCardManage.GetCard(param.CoordSysName).ReadValue(param.DataType, param.Address, 1);  // 先读取这个地址中的值
                    if (value != null && value.ToString().Trim() != "2") // 如果地址中的值不等于 2， 那么将写入 
                        result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                    else
                        result = true;
                    break;
                default:
                    result = MotionCardManage.GetCard(param.CoordSysName).WriteValue(param.DataType, param.Address, writeValue);
                    break;
            }
            return result;
        }
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
                    CoordSysConfigParam coordSysConfigParam = null;
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
            //foreach (var item in this._CommunicationParamList)
            //{
            //    if (index == this._CommunicationParamList.Count) break;
            //    if ((item.Active && item.CommunicationCommand == command && item.CoordSysName == coordSysName) ||
            //        (item.Active && item.CommunicationCommand == command && item.MapCoordSysName == coordSysName))
            //        return item; // 相等且是活动的
            //    index++;
            //}
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
            //foreach (var item in this._CommunicationParamList)
            //{
            //    if (item.Active && item.CommunicationCommand == command) list.Add(item); // 相等且是活动的
            //}
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
            if(this._CommunicationParamList != null)
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
            if (File.Exists(ParaPath + "\\" + "CommunicationConfigParam.xml"))
                this._CommunicationParamList = XML<BindingList<BindingList<CommunicationConfigParam>>>.Read(ParaPath + "\\" + "CommunicationConfigParam.xml");
            else
            {
                this._CommunicationParamList = new BindingList<BindingList<CommunicationConfigParam>>();
            }
            if (this._CommunicationParamList == null)
            {
                this._CommunicationParamList = new BindingList<BindingList<CommunicationConfigParam>>();
                for (int i = 1; i <= 10; i++)
                {
                    this._CommunicationParamList.Add(new BindingList<CommunicationConfigParam>());
                }
            }
        }



    }
}
