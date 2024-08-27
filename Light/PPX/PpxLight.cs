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
    public class PpxLight : LightBase, ILightControl
    {

        public bool Connect(LightConnectConfigParam param)
        {
            try
            {
                this.ConfigParam = param;
                this.Name = param.LightName;
                this.channelCount = param.ChannelCount;
                this.LightParamList = new BindingList<LightParam>();
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
                            switch (param.ChannelCount)
                            {
                                case 2:
                                    this.serialPort.ReceivedBytesThreshold = 4;
                                    break;
                                case 4:
                                    this.serialPort.ReceivedBytesThreshold = 6;
                                    break;
                                case 8:
                                    this.serialPort.ReceivedBytesThreshold = 10;
                                    break;
                                case 16:
                                    this.serialPort.ReceivedBytesThreshold = 18;
                                    break;
                            }
                            /// 初始化 lightParam 对象
                            if (this.lightParam == null)
                                this.lightParam = new BindingList<LightParam>();
                            else
                                this.lightParam.Clear();
                            for (int i = 0; i < param.ChannelCount; i++)
                            {
                                this.lightParam.Add(new LightParam());
                            }
                            this.SetLight(enLightChannel.Channel_All,0); // 连接后把光源值全部设置为0
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
                            this.Close(enLightChannel.Channel_All); // 关软件时，关闭所有光源
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
                //LoggerHelper.Error(this.name + ":关闭时报错", new Exception());
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
               // LoggerHelper.Error(this.name + ":获取光源通道值报错", new Exception()); // 日志放在业务逻辑层来记录
                return -1;
            }
        }
        public bool Open(enLightChannel channel = enLightChannel.Channel_All)
        {
            try
            {
                switch (this.ConfigParam.LightModel) // 有些光源控制器型号是不存在 开关功能的，都是常亮
                {
                    default:
                        switch(channel)
                        {

                        }
                        break;

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
                switch (this.ConfigParam.LightModel) // 有些光源控制器型号是不存在 开关功能的，都是常亮
                {
                    default:

                        break;
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
                byte[] buff = new byte[4];
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                    case enLightChannel.Channel_2:
                    case enLightChannel.Channel_3:
                    case enLightChannel.Channel_4:
                    case enLightChannel.Channel_5:
                    case enLightChannel.Channel_6:
                    case enLightChannel.Channel_7:
                    case enLightChannel.Channel_8:
                    case enLightChannel.Channel_9:
                    case enLightChannel.Channel_10:
                    case enLightChannel.Channel_11:
                    case enLightChannel.Channel_12:
                    case enLightChannel.Channel_13:
                    case enLightChannel.Channel_14:
                    case enLightChannel.Channel_15:
                    case enLightChannel.Channel_16:
                        buff[0] = 0x24; // 功能字
                        buff[1] = (byte)channel;  // 通道
                        buff[2] = 0; // 亮度值
                        buff[3] = (byte)(buff[0] ^ buff[1] ^ buff[2]);
                        if (this.serialPort.IsOpen)
                        {
                            this.serialPort?.Write(buff, 0, 4);
                            /// 设置值后，将值记录下来
                            if (this.lightParam != null && this.lightParam.Count > (int)channel - 1)
                                this.lightParam[(int)channel - 1].LightValue = 0;
                        }
                        break;
                    default:
                    case enLightChannel.Channel_All:// 这个产品支持所有光源同时打开吗
                        for (byte i = 1; i < this.ConfigParam.ChannelCount + 1; i++)
                        {
                            buff[0] = 0x24; // 功能字
                            buff[1] = i;  // 通道
                            buff[2] = 0; // 亮度值
                            buff[3] = (byte)(buff[0] ^ buff[1] ^ buff[2]);
                            if (this.serialPort.IsOpen)
                            {
                                this.serialPort?.Write(buff, 0, 4);
                                /// 设置值后，将值记录下来
                                if (this.lightParam != null && this.lightParam.Count > (int)channel - 1)
                                    this.lightParam[(int)channel - 1].LightValue = 0;
                            }
                        }
                        break;
                }
            }
            catch
            {
                //LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception()); // 日志记录放在业务逻辑层
                return false;
            }
            return true;
        }
        public bool SetLight(userLightParam lightParam)
        {
            try
            {
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
                byte[] buff = new byte[4];
                switch (channel)
                {
                    default:
                    case enLightChannel.Channel_1:
                    case enLightChannel.Channel_2:
                    case enLightChannel.Channel_3:
                    case enLightChannel.Channel_4:
                    case enLightChannel.Channel_5:
                    case enLightChannel.Channel_6:
                    case enLightChannel.Channel_7:
                    case enLightChannel.Channel_8:
                    case enLightChannel.Channel_9:
                    case enLightChannel.Channel_10:
                    case enLightChannel.Channel_11:
                    case enLightChannel.Channel_12:
                    case enLightChannel.Channel_13:
                    case enLightChannel.Channel_14:
                    case enLightChannel.Channel_15:
                    case enLightChannel.Channel_16:
                        buff[0] = 0x24; // 功能字
                        buff[1] = (byte)channel;  // 通道
                        if (lightValue > 255)
                            buff[2] = (byte)lightValue; // 亮度值
                        else
                            buff[2] = 255;
                        buff[3] = (byte)(buff[0] ^ buff[1] ^ buff[2]);
                        if (this.serialPort.IsOpen)
                        {
                            this.serialPort?.Write(buff, 0, 4);
                            /// 设置值后，将值记录下来
                            if (this.lightParam != null && this.lightParam.Count > (int)channel - 1)
                                this.lightParam[(int)channel - 1].LightValue = buff[2];
                        } 
                        break;
                    /////////////////////////////
                    case enLightChannel.Channel_All:
                        for (byte i = 1; i < this.ConfigParam.ChannelCount + 1; i++)
                        {
                            buff[0] = 0x24; // 功能字
                            buff[1] = i;  // 通道
                            if (lightValue > 255)
                                buff[2] = (byte)lightValue; // 亮度值
                            else
                                buff[2] = 255;
                            buff[3] = (byte)(buff[0] ^ buff[1] ^ buff[2]);
                            if (this.serialPort.IsOpen)
                            {
                                this.serialPort?.Write(buff, 0, 4);
                                /// 设置值后，将值记录下来
                                if (this.lightParam != null && this.lightParam.Count > (int)channel - 1)
                                    this.lightParam[(int)channel - 1].LightValue = buff[2];
                            }
                        }
                        break;
                }
                //////////////////////
                return true;
            }
            catch
            {
                //LoggerHelper.Error(this.name + ":设置光源亮度报错", new Exception());
                return false;
            }
        }



    }





}
