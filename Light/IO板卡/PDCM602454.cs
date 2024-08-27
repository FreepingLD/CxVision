using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.ComponentModel;

namespace Light
{
    public class PDCM602454 : LightBase, ILightControl
    {
        private enIOLightType lightType;
        private SerialPort serialPort;
        public bool DisConnect()
        {
            try
            {
                switch (connectType)
                {
                    case enUserConnectType.Network:
                        return false;

                    case enUserConnectType.USB:
                        return false;

                    case enUserConnectType.SerialPort:
                        {
                            this.SetLight(new userLightParam());
                            this.serialPort.Close();
                        }

                        return true;

                    case enUserConnectType.SerialNumber:
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
                        if (this.lightParam.Count > 0)
                            return this.lightParam[0].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_2:
                        if (this.lightParam.Count > 1)
                            return this.lightParam[1].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_3:
                        if (this.lightParam.Count > 2)
                            return this.lightParam[2].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_4:
                        if (this.lightParam.Count > 3)
                            return this.lightParam[3].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_5:
                        if (this.lightParam.Count > 4)
                            return this.lightParam[4].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_6:
                        if (this.lightParam.Count > 5)
                            return this.lightParam[5].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_7:
                        if (this.lightParam.Count > 6)
                            return this.lightParam[6].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_8:
                        if (this.lightParam.Count > 7)
                            return this.lightParam[7].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_9:
                        if (this.lightParam.Count > 8)
                            return this.lightParam[8].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_10:
                        if (this.lightParam.Count > 9)
                            return this.lightParam[9].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_11:
                        if (this.lightParam.Count > 10)
                            return this.lightParam[10].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_12:
                        if (this.lightParam.Count > 11)
                            return this.lightParam[11].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_13:
                        if (this.lightParam.Count > 12)
                            return this.lightParam[12].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_14:
                        if (this.lightParam.Count > 13)
                            return this.lightParam[13].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_15:
                        if (this.lightParam.Count > 14)
                            return this.lightParam[14].LightValue;
                        else
                            return 0;
                    case enLightChannel.Channel_16:
                        if (this.lightParam.Count > 15)
                            return this.lightParam[15].LightValue;
                        else
                            return 0;
                    /////////////////////////////
                    default:
                        if (this.lightParam.Count > 0)
                            return this.lightParam[0].LightValue;
                        else
                            return 0;
                }
            }
            catch
            {
                // LoggerHelper.Error(this.name + ":获取光源通道值报错", new Exception());
                return -1;
            }
        }
        public bool Open(enLightChannel channel = enLightChannel.Channel_All)
        {
            try
            {
                string[] command;
                byte[] data = new byte[7];
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A1", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A2", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A3", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A4", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;

                    default:
                    case enLightChannel.Channel_All:// 这个产品支持所有光源同时打开吗

                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                // LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception());
                return false;
            }
            return true;
        }
        public bool Open(enLightChannel channel, userLightParam lightParam)
        {
            try
            {
                string[] command;
                byte[] data = new byte[7];
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A1", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A2", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A3", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A4", "01").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;

                    default:
                    case enLightChannel.Channel_All:// 这个产品支持所有光源同时打开吗

                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                // LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception());
                return false;
            }
            return true;
        }
        public bool Close(enLightChannel channel = enLightChannel.Channel_All)
        {
            try
            {
                string[] command;
                byte[] data = new byte[7];
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A1", "00").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A2", "00").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A3", "00").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1}", "A4", "00").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;

                    default:
                    case enLightChannel.Channel_All:// 这个产品支持所有光源同时打开吗

                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                // LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception());
                return false;
            }
            return true;
        }
        public bool SetLight(userLightParam lightParam)
        {
            try
            {
                string[] command;
                byte[] data = new byte[7];
                if (this.serialPort.IsOpen)
                {
                    command = string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", lightParam.ch1_Value, lightParam.ch2_Value, lightParam.ch3_Value, lightParam.ch4_Value, "45", "2A").Split(' ');
                    for (int i = 0; i < command.Length; i++)
                        data[i] = Convert.ToByte(command[i], 16);
                    this.serialPort.Write(data, 0, command.Length);
                }
                return true;
            }
            catch
            {
                // LoggerHelper.Error(this.name + ":设置光源亮度报错", new Exception());
                return false;
            }
        }
        public bool SetLight(enLightChannel channel, int lightValue)
        {
            try
            {
                string[] command;
                byte[] data = new byte[7];
                switch (channel)
                {
                    case enLightChannel.Channel_1:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", lightValue, "00", "00", "00", "45", "2A").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_2:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", "00", lightValue, "00", "00", "45", "2A").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_3:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", "00", "00", lightValue, "00", "45", "2A").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    case enLightChannel.Channel_4:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", "00", "00", "00", lightValue, "45", "2A").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                    /////////////////////////////
                    default:
                        if (this.serialPort.IsOpen)
                        {
                            command = string.Format("{0} {1:X} {2:X} {3:X} {4:X} {5} {6}", "42", "00", lightValue, "00", "00", "45", "2A").Split(' ');
                            for (int i = 0; i < command.Length; i++)
                                data[i] = Convert.ToByte(command[i], 16);
                            this.serialPort.Write(data, 0, command.Length);
                        }
                        break;
                }
                //////////////////////
                return true;
            }
            catch
            {
                // LoggerHelper.Error(this.name + ":设置光源亮度报错", new Exception());
                return false;
            }
        }
        public bool Connect(LightConnectConfigParam param)
        {
            try
            {
                this.ConfigParam = param;
                this.Name = param.LightName;
                this.channelCount = param.ChannelCount;
                for (int i = 0; i < param.ChannelCount; i++)
                {
                    this.LightParamList.Add(new LightParam(i + 1));
                }
                /////////////////////
                switch (param.ConnectType)
                {
                    case enUserConnectType.Network:
                        return false;

                    case enUserConnectType.USB:
                        return false;

                    case enUserConnectType.SerialPort:
                        this.serialPort = new SerialPort(param.PortName, param.BaudRate);
                        this.serialPort.Open();
                        if (this.serialPort.IsOpen)
                        {
                            this.SetLight(new userLightParam()); // 连接后把光源值全部设置为0
                            param.ConnectState = true;
                            return true;
                        }
                        else
                        {
                            param.ConnectState = false;
                            return false;
                        }                  
                    case enUserConnectType.SerialNumber:
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


    }

    public enum enIOLightType
    {


    }



}
