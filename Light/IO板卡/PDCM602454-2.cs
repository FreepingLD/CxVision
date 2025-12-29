using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Light
{
    public class PDCM602454 : LightBase, ILightControl
    {
        private enIOLightType lightType;
        private SerialPort serialPort;
        public bool Connect(object param)
        {
            try
            {
                this.Name = param.ToString();
                string[] paramName = this.Name.Split(';'); // 第一个值表示连接类型
                // 控制器类型
                if (Enum.TryParse(paramName[0], out connectType))
                {
                    switch (connectType)
                    {
                        case enConnectType.Network:
                            return false;

                        case enConnectType.USB:
                            return false;

                        case enConnectType.SerialPort:
                            int.TryParse(paramName[2], out this.baudRate);
                            Enum.TryParse(paramName[3], out lightType);
                            this.serialPort = new SerialPort(paramName[1].ToUpper(), this.baudRate);
                            this.serialPort.Open();
                            if (this.serialPort.IsOpen)
                                return true;
                            else
                                return false;

                        case enConnectType.SerialNumber:
                            return false;

                        default:
                            return false;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DisConnect()
        {
            try
            {
                switch (connectType)
                {
                    case enConnectType.Network:
                        return false;

                    case enConnectType.USB:
                        return false;

                    case enConnectType.SerialPort:
                        this.serialPort.Close();
                        return true;

                    case enConnectType.SerialNumber:
                        return false;

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public int GetLight(enLightChannel channel)
        {
            try
            {
                switch (channel)
                {
                    case enLightChannel.Channel_1:
                        return this.lightParam.ch1_Value;
                    case enLightChannel.Channel_2:
                        return this.lightParam.ch2_Value;
                    case enLightChannel.Channel_3:
                        return this.lightParam.ch3_Value;
                    case enLightChannel.Channel_4:
                        return this.lightParam.ch4_Value;
                    case enLightChannel.Channel_5:
                        return this.lightParam.ch5_Value;
                    case enLightChannel.Channel_6:
                        return this.lightParam.ch6_Value;
                    case enLightChannel.Channel_7:
                        return this.lightParam.ch7_Value;
                    case enLightChannel.Channel_8:
                        return this.lightParam.ch8_Value;
                    case enLightChannel.Channel_9:
                        return this.lightParam.ch9_Value;
                    case enLightChannel.Channel_10:
                        return this.lightParam.ch10_Value;
                    case enLightChannel.Channel_11:
                        return this.lightParam.ch11_Value;
                    case enLightChannel.Channel_12:
                        return this.lightParam.ch12_Value;
                    case enLightChannel.Channel_13:
                        return this.lightParam.ch13_Value;
                    case enLightChannel.Channel_14:
                        return this.lightParam.ch14_Value;
                    case enLightChannel.Channel_15:
                        return this.lightParam.ch15_Value;
                    case enLightChannel.Channel_16:
                        return this.lightParam.ch1_Value;
                    /////////////////////////////
                    default:
                        return this.lightParam.ch1_Value;
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + ":获取光源通道值报错", new Exception());
                return -1;
            }
        }
        public bool Open(enLightChannel channel = enLightChannel.Channel_All)
        {
            try
            {
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A1", "01"));
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A2", "01"));
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A3", "01"));
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A7", "01"));
                        break;

                    default:
                    case enLightChannel.Channel_All:// 这个产品支持所有光源同时打开吗

                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception());
                return false;
            }
            return true;
        }
        public bool Open(enLightChannel channel, userLightParam lightParam)
        {
            try
            {
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A1", "01"));
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A2", "01"));
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A3", "01"));
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A7", "01"));
                        break;

                    default:
                    case enLightChannel.Channel_All:// 这个产品支持所有光源同时打开吗

                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception());
                return false;
            }
            return true;
        }
        public bool Close(enLightChannel channel = enLightChannel.Channel_All)
        {
            try
            {
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A1", "00"));
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A2", "00"));
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A3", "00"));
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1}", "A7", "00"));
                        break;

                    default:
                    case enLightChannel.Channel_All:// 这个产品支持所有光源同时打开吗

                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception());
                return false;
            }
            return true;
        }
        public bool SetLight(userLightParam lightParam)
        {
            try
            {
                if (this.serialPort.IsOpen)
                    this.serialPort.WriteLine(string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", lightParam.ch1_Value, lightParam.ch2_Value, lightParam.ch3_Value, lightParam.ch4_Value, "45", "2A"));
                return true;
            }
            catch
            {
                LoggerHelper.Error(this.name + ":设置光源亮度报错", new Exception());
                return false;
            }
        }
        public bool SetLight(enLightChannel channel, int lightValue)
        {
            try
            {
                switch (channel)
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", lightValue, 00, 00, 00, "45", "2A"));
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", 00, lightValue, 00, 00, "45", "2A"));
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", 00, 00, lightValue, 00, "45", "2A"));
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", 00, 00, 00, lightValue, "45", "2A"));
                        break;

                    /////////////////////////////
                    default:
                        if (this.serialPort.IsOpen)
                            this.serialPort.WriteLine(string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", lightValue, 00, 00, 00, "45", "2A"));
                        break;
                }
                //////////////////////
                return true;
            }
            catch
            {
                LoggerHelper.Error(this.name + ":设置光源亮度报错", new Exception());
                return false;
            }
        }
        public object SetParam(enLightParamType paramType)
        {
            throw new NotImplementedException();
        }
    }

    public enum enIOLightType
    {


    }

}
