using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class SocketClientDevice : MotionBase
    {
        private string sign = "SocketClien";
        private string adress;
        private ClientSocket _clientSocket;
        private SocketParam _socketParam;
        private SocketCommand Command;
        private Dictionary<string, SocketCommand> Dic = new Dictionary<string, SocketCommand>();
        public ClientSocket ClientSocket { get => _clientSocket; set => _clientSocket = value; }



        public int TriggerCount = 0;
        public override bool Init(DeviceConnectConfigParam param)
        {
            bool result = false;
            try
            {
                this.ConnectConfigParam = param;
                this.name = param.DeviceName;
                this.Command = new SocketCommand();
                switch (param.ConnectType)
                {
                    case enUserConnectType.TcpIp:
                    case enUserConnectType.Network:
                        this.adress = param.IpAdress;
                        this._socketParam = new SocketParam();
                        this._socketParam.IpAdress = param.IpAdress;
                        this._socketParam.Port = param.Port;
                        this._socketParam.Describe = param.DeviceName;
                        this._clientSocket = new ClientSocket(this._socketParam);
                        if (!param.IsActive) return result;
                        result = this._clientSocket.ConnectAsync();
                        if (result)
                        {
                            this._socketParam.DataUpdata += new EventHandler(this.ClientReciveData);
                            LoggerHelper.Info(sign + param.IpAdress + "打开成功");
                        }
                        /// 初始化对象
                        this.Dic.Clear();
                        foreach (var item in Sensor.SensorManage.GetSensorName())
                        {
                            this.Dic.Add(item, this.Command);
                            this.Dic[item].TriggerFromSocket = 0; // 通知接收了一个触发信号
                        }
                        break;
                    case enUserConnectType.SerialPort:

                        break;
                    case enUserConnectType.Modbus:

                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(sign + param.IpAdress + "打开报错" + ex.ToString());
            }
            param.ConnectState = result;
            return result;
        }
        public override void UnInit()
        {
            try
            {
                this._socketParam.DataUpdata -= new EventHandler(this.ClientReciveData);
                this._clientSocket?.Disconnect();
                this._clientSocket?.Close();
                LoggerHelper.Info(this.sign + this.adress + "关闭成功");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.sign + this.adress + "关闭报错" + ex.ToString());
            }
        }

        /// <summary>
        ///  读取内容,需要先告诉服务器要读取什么内容
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="address">可以作为内容存储来处理</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override object ReadValue(enDataTypes dataType, string address, int length = 1)
        {
            object value = "";
            try
            {
                switch (dataType) // 用数据类型来区分
                {
                    /////////////////////  表示读取 Socket 指令  ////////////////////////////
                    default:
                        CoordSysConfigParam coordSysParam;
                        double position;
                        string[] content = address.Split('.');
                        if (content != null && content.Length > 1)
                        {
                            string camLable = "";
                            string lable = "";
                            if (content.Length > 0)
                                camLable = content[0];
                            if (content.Length > 1)
                                lable = content[1];
                            switch (lable.Trim())
                            {
                                case "X":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].X);
                                        value = position; // this.Dic[camLable].X;
                                    }
                                    break;
                                case "Y":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Y);
                                        value = position; //this.Dic[camLable].Y;
                                    }
                                    break;
                                case "Z":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Z轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Z);
                                        value = position; //this.Dic[camLable].Y;
                                    }
                                    break;
                                case "W":
                                case "Theta":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Theta轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Theta);
                                        double calibValue = 0;
                                        this.CalibrateParam?.SourceValueToCalibrateValue("Theta", position, out calibValue);
                                        value = calibValue;
                                    }
                                    break;
                                case "U":
                                case "A":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.U轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Theta);
                                        double calibValue = 0;
                                        this.CalibrateParam?.SourceValueToCalibrateValue("U", position, out calibValue);
                                        value = calibValue;
                                    }
                                    break;
                                case "V":
                                case "B":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.V轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Theta);
                                        double calibValue = 0;
                                        this.CalibrateParam?.SourceValueToCalibrateValue("V", position, out calibValue);
                                        value = calibValue;
                                    }
                                    break;
                                case "Command":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].Command;
                                    break;
                                case "GrabNo":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].GrabNo;
                                    break;
                                case "CamStation":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].CamStation;
                                    break;
                                case "GrabResult":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].GrabResult;
                                    break;
                                case "AlignCount":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].AlignCount;
                                    break;
                                case "Trigger":
                                    value = TriggerCount;
                                    break;
                                case "TriggerFromPlc":
                                case "TriggerFromPLC":
                                case "TriggerFromSocket":
                                case "TriggerFromPLc":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].TriggerFromSocket;
                                    break;
                                case "X1":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].X1);
                                        value = position;
                                    }
                                    break;
                                case "Y1":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Y1);
                                        value = position;
                                    }
                                    break;
                                case "X2":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].X2);
                                        value = position;
                                    }
                                    break;
                                case "Y2":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Y2);
                                        value = position;
                                    }
                                    break;
                                case "Addx":
                                case "Add_x":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Add_x);
                                        value = position;
                                    }
                                    break;
                                case "Addy":
                                case "Add_y":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Add_theta);
                                        value = position;
                                    }
                                    break;
                                case "Addz":
                                case "Add_z":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Z轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Add_theta);
                                        value = position;
                                    }
                                    break;
                                case "Addu":
                                case "Add_u":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.U轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Add_theta);
                                        value = position;
                                    }
                                    break;
                                case "Addv":
                                case "Add_v":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.V轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Add_theta);
                                        value = position;
                                    }
                                    break;
                                case "Addw":
                                case "Addtheta":
                                case "Add_theta":
                                case "Add_w":
                                    if (this.Dic.ContainsKey(camLable))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camLable, enAxisName.Theta轴);
                                        position = this.MirrorAxisCoord(coordSysParam, this.Dic[camLable].Add_theta);
                                        value = position;
                                    }
                                    break;
                                case "Path_X":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].Path_X;
                                    break;
                                case "Path_Y":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].Path_Y;
                                    break;
                                case "Path_Z":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].Path_Z;
                                    break;
                                case "Path_Theta":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].Path_Theta;
                                    break;
                                case "Path_U":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].Path_U;
                                    break;
                                case "Path_V":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable].Path_V;
                                    break;
                                default:
                                case "SocketCommand":
                                    if (this.Dic.ContainsKey(camLable))
                                        value = this.Dic[camLable];
                                    else
                                        value = new SocketCommand(true);
                                    break;
                            }
                        }
                        break;
                    case enDataTypes.SocketCommand: // 如果数据类型是 SocketCommand 则地址需用相机名称，表示读取整个指点令
                        if (this.Dic.ContainsKey(address))
                            value = this.Dic[address];
                        break;
                }
                if (value == null)
                    LoggerHelper.Error(this.sign + this.adress + "->读取数据失败");
                //if (value != null)
                //    LoggerHelper.Error(this.sign + this.adress + "->读取数据成功");
                //else
                //    LoggerHelper.Error(this.sign + this.adress + "->读取数据失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.sign + this.adress + "->读取数据失败", ex);
                //return null;
            }
            return value;
        }

        public override bool WriteValue(enDataTypes dataType, string address, object value)
        {
            bool result = false;
            try
            {
                string[] content = address.Split('.');
                switch (dataType)
                {
                    case enDataTypes.SocketCommand: //// Socket 指令只能一次读取或一次写入 
                        if (this.Dic.ContainsKey(address))
                        {
                            if (value.ToString().Trim() == "Reset" || value.ToString().Trim() == "reset")
                            {
                                if (this.Dic.ContainsKey(address))
                                    this.Dic[address].Reset();
                                result = true;
                            }
                            else
                            {
                                result = this._clientSocket.SendDataAsync(Encoding.UTF8.GetBytes(this.Dic[address].ToString()));
                                //result = this._clientSocket.WaiteSend();
                            }
                        }
                        else
                            result = false;
                        break;
                    ///////////////////////////////////
                    default:
                        if (content != null && content.Length > 1)
                        {
                            CoordSysConfigParam coordSysParam;
                            double position;
                            string camName = content[0];
                            string socketAdress = content[1];
                            switch (socketAdress)
                            {
                                case "X":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].X = position;
                                        result = true;
                                    }
                                    break;
                                case "Y":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Y = position;
                                        result = true;
                                    }
                                    break;
                                case "Z":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Z轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Z = position;
                                        result = true;
                                    }
                                    break;
                                case "Theta":
                                case "W":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Theta轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        double sourceValue = 0;
                                        this.CalibrateParam?.CalibrateValueToSourceValue("Theta", position, out sourceValue);
                                        this.Dic[camName].Theta = sourceValue;
                                        result = true;
                                    }
                                    break;
                                case "U":
                                case "A":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.U轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        double sourceValue = 0;
                                        this.CalibrateParam?.CalibrateValueToSourceValue("U", position, out sourceValue);
                                        this.Dic[camName].Theta = sourceValue;
                                        result = true;
                                    }
                                    break;
                                case "V":
                                case "B":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.V轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        double sourceValue = 0;
                                        this.CalibrateParam?.CalibrateValueToSourceValue("V", position, out sourceValue);
                                        this.Dic[camName].Theta = sourceValue;
                                        result = true;
                                    }
                                    break;
                                case "GrabNo":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        this.Dic[camName].GrabNo = Convert.ToInt32(value);
                                        result = true;
                                    }
                                    break;
                                case "Command":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        this.Dic[camName].Command = value.ToString();
                                        result = true;
                                    }
                                    break;
                                case "CamStation":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        this.Dic[camName].CamStation = value.ToString();
                                        result = true;
                                    }
                                    break;
                                case "GrabResult":
                                case "Result":
                                case "result":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        this.Dic[camName].GrabResult = value.ToString();
                                        result = true;
                                    }
                                    break;
                                case "AlignCount":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        value = this.Dic[camName].AlignCount = Convert.ToInt32(value);
                                        result = true;
                                    }
                                    break;
                                case "TriggerFromPlc":
                                case "TriggerFromSocket":
                                case "TriggerFromPLC":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        this.Dic[camName].TriggerFromSocket = Convert.ToInt32(value);
                                        result = true;
                                    }
                                    break;
                                case "X1":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].X1 = position;
                                        result = true;
                                    }
                                    break;
                                case "Y1":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Y1 = position;
                                        result = true;
                                    }
                                    break;
                                case "X2":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].X2 = position;
                                        result = true;
                                    }
                                    break;
                                case "Y2":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Y2 = position;
                                        result = true;
                                    }
                                    break;
                                case "Addx":
                                case "Add_x":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.X轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Add_x = position;
                                        result = true;
                                    }
                                    break;
                                case "Addy":
                                case "Add_y":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Y轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Add_theta = position;
                                        result = true;
                                    }
                                    break;
                                case "Addz":
                                case "Add_z":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Z轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Add_theta = position;
                                        result = true;
                                    }
                                    break;
                                case "Addtheta":
                                case "Add_theta":
                                case "Add_w":
                                case "Add_W":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Theta轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Add_theta = position;
                                        result = true;
                                    }
                                    break;
                                case "Addu":
                                case "Add_u":
                                case "Add_U":
                                case "Add_A":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.U轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Add_theta = position;
                                        result = true;
                                    }
                                    break;
                                case "Addv":
                                case "Add_v":
                                case "Add_V":
                                case "Add_B":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.V轴);
                                        position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(value));
                                        this.Dic[camName].Add_theta = position;
                                        result = true;
                                    }
                                    break;
                                case "Path_X":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.X轴);
                                        string[] x_value = value.ToString().Split(',');
                                        foreach (var item in x_value)
                                        {
                                            position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(item));
                                            this.Dic[camName].Path_X.Add(position);
                                        }
                                        result = true;
                                    }
                                    break;
                                case "Path_Y":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Y轴);
                                        string[] y_value = value.ToString().Split(',');
                                        foreach (var item in y_value)
                                        {
                                            position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(item));
                                            this.Dic[camName].Path_Y.Add(position);
                                        }
                                        result = true;
                                    }
                                    break;
                                case "Path_Z":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Z轴);
                                        string[] z_value = value.ToString().Split(',');
                                        foreach (var item in z_value)
                                        {
                                            position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(item));
                                            this.Dic[camName].Path_Z.Add(position);
                                        }
                                        result = true;
                                    }
                                    break;
                                case "Path_Theta":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.Theta轴);
                                        string[] theta_value = value.ToString().Split(',');
                                        foreach (var item in theta_value)
                                        {
                                            position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(item));
                                            this.Dic[camName].Path_Theta.Add(position);
                                        }
                                        result = true;
                                    }
                                    break;
                                case "Path_U":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.U轴);
                                        string[] u_value = value.ToString().Split(',');
                                        foreach (var item in u_value)
                                        {
                                            position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(item));
                                            this.Dic[camName].Path_U.Add(position);
                                        }
                                        result = true;
                                    }
                                    break;
                                case "Path_V":
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(camName, enAxisName.V轴);
                                        string[] v_value = value.ToString().Split(',');
                                        foreach (var item in v_value)
                                        {
                                            position = this.MirrorAxisCoord(coordSysParam, Convert.ToDouble(item));
                                            this.Dic[camName].Path_V.Add(position);
                                        }
                                        result = true;
                                    }
                                    break;
                                case "SocketCommand":
                                case "TriggerToPlc": ///
                                case "TriggerToSocket": ///
                                    //if (this.Dic.ContainsKey(camName))
                                    //{
                                    //    result = this._clientSocket.SendDataAsync(Encoding.UTF8.GetBytes(this.Dic[camName].ToString()));
                                    //    //result = this._clientSocket.WaiteSend();
                                    //}
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        if (value != null && value.ToString().Length > 10)
                                        {
                                            this.Command = SocketCommand.GetSocketCommand(value.ToString(), this.name);
                                            result = this._clientSocket.SendDataAsync(Encoding.UTF8.GetBytes(this.Command.ToString()));
                                        }
                                        else
                                            result = this._clientSocket.SendDataAsync(Encoding.UTF8.GetBytes(this.Dic[camName].ToString()));
                                    }
                                    break;
                                case "Reset": ///
                                case "reset": ///
                                    if (this.Dic.ContainsKey(camName))
                                    {
                                        this.Dic[camName].Reset();
                                        //this._clientSocket.SendDataAsync(Encoding.UTF8.GetBytes(this.Dic[camName].ToString()));
                                        //result = this._clientSocket.WaiteSend();
                                    }
                                    break;
                            }
                        }
                        break;
                }
                if (result)
                    LoggerHelper.Info(this.sign + this.adress + "->数据写入成功");
                else
                    LoggerHelper.Info(this.sign + this.adress + "->数据写入失败");
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.sign + this.adress + "->写入数据报错", ex);
            }
            return result;
        }

        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            position = 0;
            CoordSysConfigParam coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);   // 获取坐标系配置参数
            if (coordSysParam == null) return;
            SocketCommand command;//= this.Dic[coordSysParam.AxisLable];
            if (this.Dic.ContainsKey(coordSysParam.AxisLable))
                command = this.Dic[coordSysParam.AxisLable];
            else
                command = new SocketCommand(true);
            //////////////////////////////////
            switch (axisName)
            {
                case enAxisName.X轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.X);
                    break;
                case enAxisName.X2轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.X2);
                    break;
                case enAxisName.Y轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.Y);
                    break;
                case enAxisName.Y2轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.Y2);
                    break;
                case enAxisName.Z轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.Z);
                    break;
                case enAxisName.Theta轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.Theta);
                    double calibValue = position;
                    this.CalibrateParam?.SourceValueToCalibrateValue("Theta", position, out calibValue);
                    break;
                case enAxisName.U轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.U);
                    break;
                case enAxisName.V轴:
                    position = this.MirrorAxisCoord(coordSysParam, command.V);
                    break;
                default:
                    position = 0;
                    break;
            }
        }

        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            CoordSysConfigParam coordSysConfigParam;//
            SocketCommand command = null;
            switch (axisName)
            {
                case enAxisName.X轴:
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysConfigParam == null) return;
                    if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                        command = this.Dic[coordSysConfigParam.AxisLable];
                    else
                        command = new SocketCommand(true);
                    command.CamStation = coordSysConfigParam.AxisLable;
                    command.X = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.X);
                    /////////////////////
                    this.WriteValue(enDataTypes.SocketCommand, coordSysConfigParam.AxisLable, "");
                    break;
                case enAxisName.Y轴:
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysConfigParam == null) return;
                    if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                        command = this.Dic[coordSysConfigParam.AxisLable];
                    else
                        command = new SocketCommand(true);
                    command.CamStation = coordSysConfigParam.AxisLable;
                    command.Y = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Y);
                    /////////////////////
                    this.WriteValue(enDataTypes.SocketCommand, coordSysConfigParam.AxisLable, "");
                    break;
                case enAxisName.Z轴:
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, axisName);
                    if (coordSysConfigParam == null) return;
                    if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                        command = this.Dic[coordSysConfigParam.AxisLable];
                    else
                        command = new SocketCommand(true);
                    command.CamStation = coordSysConfigParam.AxisLable;
                    command.Z = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Z);
                    /////////////////////
                    this.WriteValue(enDataTypes.SocketCommand, coordSysConfigParam.AxisLable, "");
                    break;
                case enAxisName.XY轴:
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysConfigParam == null) return;
                    if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                        command = this.Dic[coordSysConfigParam.AxisLable];
                    else
                        command = new SocketCommand(true);
                    command.CamStation = coordSysConfigParam.AxisLable;
                    command.X = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.X);
                    //////2
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysConfigParam == null) return;
                    //if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                    //    command = this.Dic[coordSysConfigParam.AxisLable];
                    //else
                    //    command = new SocketCommand(true);
                    //command.CamStation = coordSysConfigParam.AxisLable;
                    command.Y = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Y);
                    /////////////////////
                    this.WriteValue(enDataTypes.SocketCommand, coordSysConfigParam.AxisLable, ""); // 触发 上位机
                    break;
                default:
                case enAxisName.XYTheta轴:
                    ////////// 1
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysConfigParam == null) return;
                    if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                        command = this.Dic[coordSysConfigParam.AxisLable];
                    else
                        command = new SocketCommand(true);
                    command.CamStation = coordSysConfigParam.AxisLable;
                    command.X = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.X);
                    ////////// 2
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysConfigParam == null) return;
                    //if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                    //    command = this.Dic[coordSysConfigParam.AxisLable];
                    //else
                    //    command = new SocketCommand(true);
                    //command.CamStation = coordSysConfigParam.AxisLable;
                    command.Y = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Y);
                    ////////// 3
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysConfigParam == null) return;
                    //if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                    //    command = this.Dic[coordSysConfigParam.AxisLable];
                    //else
                    //    command = new SocketCommand(true);
                    //command.CamStation = coordSysConfigParam.AxisLable;
                    command.Theta = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Theta);
                    /////////////////////
                    this.WriteValue(enDataTypes.SocketCommand, coordSysConfigParam.AxisLable, "");
                    break;
                case enAxisName.XYZTheta轴:
                    ////////// 1
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.X轴);
                    if (coordSysConfigParam == null) return;
                    if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                        command = this.Dic[coordSysConfigParam.AxisLable];
                    else
                        command = new SocketCommand(true);
                    command.CamStation = coordSysConfigParam.AxisLable;
                    command.X = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.X);
                    //this.WriteValue(coordSysConfigParam.DataType, coordSysConfigParam.AxisLable + ".X", command.X);
                    ////////// 2
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Y轴);
                    if (coordSysConfigParam == null) return;
                    //if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                    //    command = this.Dic[coordSysConfigParam.AxisLable];
                    //else
                    //    command = new SocketCommand(true);
                    //command.CamStation = coordSysConfigParam.AxisLable;
                    command.Y = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Y);
                    //this.WriteValue(coordSysConfigParam.DataType, coordSysConfigParam.AxisLable + ".Y", command.Y);  // AxisLable : 即为绑定的相机名
                    ////////// 3
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Z轴);
                    if (coordSysConfigParam == null) return;
                    //if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                    //    command = this.Dic[coordSysConfigParam.AxisLable];
                    //else
                    //    command = new SocketCommand(true);
                    //command.CamStation = coordSysConfigParam.AxisLable;
                    command.Z = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Z);
                    //this.WriteValue(coordSysConfigParam.DataType, coordSysConfigParam.AxisLable + ".Z", command.Z);
                    /////////////////////////////////////////////4
                    coordSysConfigParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(CoordSysName, enAxisName.Theta轴);
                    if (coordSysConfigParam == null) return;
                    //if (this.Dic.ContainsKey(coordSysConfigParam.AxisLable))
                    //    command = this.Dic[coordSysConfigParam.AxisLable];
                    //else
                    //    command = new SocketCommand(true);
                    //command.CamStation = coordSysConfigParam.AxisLable;
                    command.Theta = this.MirrorAxisCoord(coordSysConfigParam, axisPosition.Theta);
                    //this.WriteValue(coordSysConfigParam.DataType, coordSysConfigParam.AxisLable + ".Theta", command.Theta);
                    /////////////////////
                    this.WriteValue(enDataTypes.SocketCommand, coordSysConfigParam.AxisLable, "");
                    break;
            }
        }

        private void ClientReciveData(object send, EventArgs e)
        {
            // 根据相机名称来标识 Socket 命令
            if (this._socketParam.ReceiveData != null && this._socketParam.ReceiveData.Length > 0)
                LoggerHelper.Info("客户器端" + this.name + this.adress + "：接收数据" + "->" + this._socketParam.ReceiveData);
            this.Command = SocketCommand.GetSocketCommand(this._socketParam.ReceiveData, this.name);
            this.ConnectConfigParam.ReceiveData = this._socketParam.ReceiveData;
            foreach (var item in Sensor.SensorManage.GetSensorName())
            {
                if (this.Command.CamStation == item)
                {
                    if (this.Dic.ContainsKey(item))
                        this.Dic[item] = this.Command;
                    else
                        this.Dic.Add(item, this.Command);
                    /////////////////////////////////////////////
                    this.Dic[item].TriggerFromSocket = 1; // 通知接收了一个触发信号
                }
            }
            this.TriggerCount = 1;
        }

        private void ResetCVonnect(object send, EventArgs e)
        {
            this._clientSocket.ConnectAsync();
        }


    }






}
