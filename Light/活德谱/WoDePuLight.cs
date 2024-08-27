using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO.Ports;
using static SerialCommunicate.ASCII_Data;
using System.Threading;
using System.Windows.Forms;

namespace Light
{
    public class WoDePuLight : LightBase, ILightControl
    {
        private enWDPLightType lightType;

        public bool Connect(LightConnectConfigParam param)
        {
            try
            {
                this.ConfigParam = param;
                this.Name = param.LightName;
                this.connectAdress = param.IpAdress;
                this.channelCount = param.ChannelCount;
                for (int i = 0; i < param.ChannelCount; i++)
                {
                    this.LightParamList.Add(new LightParam(i + 1));
                }
                // 控制器类型
                switch (connectType)
                {
                    case enUserConnectType.Network:
                        return false;

                    case enUserConnectType.USB:
                        return false;

                    case enUserConnectType.SerialPort:
                        if (param.BaudRate != 19200)
                            param.BaudRate = 19200;
                        if (Open_SerialPort(param.PortName, param.BaudRate))
                        {
                            this.Open(enLightChannel.Channel_All, new userLightParam()); // 连接上控制器后，将所有通道打开，并将光源值都设置为0
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
                //LoggerHelper.Error("WoDePuLight:在打开光源时报错", new Exception());
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
                        this.Close();
                        SerialCommunicate.ASCII_Data.serialPort.Close();
                        return true;

                    case enUserConnectType.SerialNumber:
                        return false;

                    default:
                        return false;
                }
            }
            catch
            {
                //LoggerHelper.Error("WoDePuLight:在关闭光源时报错", new Exception());
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
                        //if (this.lightParam.ch1_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 0);
                        break;
                    case enLightChannel.Channel_2:
                        //if (this.lightParam.ch2_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 1);
                        break;
                    case enLightChannel.Channel_3:
                        //if (this.lightParam.ch3_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 2);
                        break;
                    case enLightChannel.Channel_4:
                        //if (this.lightParam.ch4_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 3);
                        break;
                    case enLightChannel.Channel_5:
                        //if (this.lightParam.ch5_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 4);
                        break;
                    case enLightChannel.Channel_6:
                        //if (this.lightParam.ch6_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 5);
                        break;
                    case enLightChannel.Channel_7:
                        // if (this.lightParam.ch7_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 6);
                        break;
                    case enLightChannel.Channel_8:
                        //if (this.lightParam.ch8_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 7);
                        break;
                    case enLightChannel.Channel_9:
                        // if (this.lightParam.ch9_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 8);
                        break;
                    case enLightChannel.Channel_10:
                        // if (this.lightParam.ch10_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 9);
                        break;
                    case enLightChannel.Channel_11:
                        // if (this.lightParam.ch11_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 10);
                        break;
                    case enLightChannel.Channel_12:
                        // if (this.lightParam.ch12_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 11);
                        break;
                    case enLightChannel.Channel_13:
                        // if (this.lightParam.ch13_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 12);
                        break;
                    case enLightChannel.Channel_14:
                        //if (this.lightParam.ch14_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 13);
                        break;
                    case enLightChannel.Channel_15:
                        // if (this.lightParam.ch15_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 14);
                        break;
                    case enLightChannel.Channel_16:
                        //if (this.lightParam.ch16_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 1, 15);
                        break;

                    default:
                    case enLightChannel.Channel_All:// 表示同进打开四个通道
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F0={0},F1={1},F2={2},F3={3},F4={4},F5={5},F6={6},F7={7},F8={8},F9={9},F10={10},F11={11},F12={12},F13={13},F14={14},F15={15}#",
                             1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + ":打开光源通道报错", new Exception());
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
                        //if (!this.lightParam.ch1_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 0);
                        break;
                    case enLightChannel.Channel_2:
                        //if (!this.lightParam.ch2_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 1);
                        break;
                    case enLightChannel.Channel_3:
                        //if (!this.lightParam.ch3_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 2);
                        break;
                    case enLightChannel.Channel_4:
                        // if (!this.lightParam.ch4_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 3);
                        break;
                    case enLightChannel.Channel_5:
                        // if (!this.lightParam.ch5_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 4);
                        break;
                    case enLightChannel.Channel_6:
                        //if (!this.lightParam.ch6_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 5);
                        break;
                    case enLightChannel.Channel_7:
                        //if (!this.lightParam.ch7_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 6);
                        break;
                    case enLightChannel.Channel_8:
                        //if (!this.lightParam.ch8_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 7);
                        break;
                    case enLightChannel.Channel_9:
                        //if (!this.lightParam.ch9_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 8);
                        break;
                    case enLightChannel.Channel_10:
                        //if (!this.lightParam.ch10_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 9);
                        break;
                    case enLightChannel.Channel_11:
                        //if (!this.lightParam.ch11_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 10);
                        break;
                    case enLightChannel.Channel_12:
                        //if (!this.lightParam.ch12_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 11);
                        break;
                    case enLightChannel.Channel_13:
                        //if (!this.lightParam.ch13_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 12);
                        break;
                    case enLightChannel.Channel_14:
                        //if (!this.lightParam.ch14_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 13);
                        break;
                    case enLightChannel.Channel_15:
                        //if (!this.lightParam.ch15_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 14);
                        break;
                    case enLightChannel.Channel_16:
                        //if (!this.lightParam.ch16_State)// 表示同进打开一个通道
                        ASCII_SET_ON_OFF(1, 0, 15);
                        break;

                    default:
                    case enLightChannel.Channel_All:// 表示同进打开四个通道
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F0={0},F1={1},F2={2},F3={3},F4={4},F5={5},F6={6},F7={7},F8={8},F9={9},F10={10},F11={11},F12={12},F13={13},F14={14},F15={15}#",
                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                        break;
                        ///////////////////////////
                }
                //// 关闭是一次性全部关，开才需要组合开
                //SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F0={0},F1={1},F2={2},F3={3},F4={4},F5={5},F6={6},F7={7}#", 0, 0, 0, 0, 0, 0, 0, 0));
            }
            catch
            {
                LoggerHelper.Error(this.name + ":关闭光源通道报错", new Exception());
                return false;
            }
            return true;
        }
        public bool SetLight(enLightChannel channel, int lightValue)
        {
            try
            {
                switch (channel)
                {
                    case enLightChannel.Channel_1:
                        //if (this.lightParam.ch1_State)
                        //{
                        this.lightParam[0].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_1 - 1, (int)lightType);
                        // }

                        break;
                    case enLightChannel.Channel_2:
                        //if (this.lightParam.ch2_State)
                        //{
                        this.lightParam[1].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_2 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_3:
                        //if (this.lightParam.ch3_State)
                        //{
                        this.lightParam[2].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_3 - 1, (int)lightType);
                        // }
                        break;
                    case enLightChannel.Channel_4:
                        //if (this.lightParam.ch4_State)
                        //{
                        this.lightParam[3].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_4 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_5:
                        //if (this.lightParam.ch5_State)
                        //{
                        this.lightParam[4].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_5 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_6:
                        //if (this.lightParam.ch6_State)
                        //{
                        this.lightParam[5].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_6 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_7:
                        //if (this.lightParam.ch7_State)
                        //{
                        this.lightParam[6].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_7 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_8:
                        //if (this.lightParam.ch8_State)
                        //{
                        this.lightParam[7].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_8 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_9:
                        //if (this.lightParam.ch9_State)
                        //{
                        this.lightParam[8].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_9 - 1, (int)lightType);
                        // }
                        break;
                    case enLightChannel.Channel_10:
                        //if (this.lightParam.ch10_State)
                        //{
                        this.lightParam[9].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_10 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_11:
                        //if (this.lightParam.ch11_State)
                        //{
                        this.lightParam[10].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_11 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_12:
                        //if (this.lightParam.ch12_State)
                        //{
                        this.lightParam[11].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_12 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_13:
                        //if (this.lightParam.ch13_State)
                        //{
                        this.lightParam[12].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_13 - 1, (int)lightType);
                        // }
                        break;
                    case enLightChannel.Channel_14:
                        //if (this.lightParam.ch14_State)
                        //{
                        this.lightParam[13].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_14 - 1, (int)lightType);
                        //}
                        break;
                    case enLightChannel.Channel_15:
                        //if (this.lightParam.ch15_State)
                        //{
                        this.lightParam[14].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_15 - 1, (int)lightType);
                        // };
                        break;
                    case enLightChannel.Channel_16:
                        //if (this.lightParam.ch16_State)
                        //{
                        this.lightParam[15].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_16 - 1, (int)lightType);
                        // }
                        break;
                    /////////////////////////////
                    default:
                        //if (this.lightParam.ch1_State)
                        //{
                        this.lightParam[0].LightValue = lightValue;
                        ASCII_SET_LEVEL(1, lightValue, (int)enLightChannel.Channel_1 - 1, (int)lightType);
                        // }
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
        public bool SetLight(userLightParam lightParam)
        {
            try
            {
                switch (channelCount)
                {
                    default:
                        return true;
                    case 1:
                        return true;
                    case 4:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F0={0},F1={1},F2={2},F3={3},L0={4},L1={5},L2={6},L3={7}#", 1, 1, 1, 1, lightParam.ch1_Value, lightParam.ch2_Value, lightParam.ch3_Value, lightParam.ch4_Value));
                        return true;
                }
                //SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$L0={0},L1={1},L2={2},L3={3},L4={4},L5={5},L6={6},L7={7},L8={8},L9={9},L10={10},L11={11},L12={12},L13={13},L14={14},L15={15}#",
                //lightParam.ch1_Value, lightParam.ch2_Value, lightParam.ch3_Value, lightParam.ch4_Value, lightParam.ch5_Value, lightParam.ch6_Value, lightParam.ch7_Value, lightParam.ch8_Value,
                //lightParam.ch9_Value, lightParam.ch10_Value, lightParam.ch11_Value, lightParam.ch12_Value, lightParam.ch13_Value, lightParam.ch14_Value, lightParam.ch15_Value, lightParam.ch16_Value));
                //return true;
            }
            catch
            {
                LoggerHelper.Error(this.name + ":设置光源亮度报错", new Exception());
                return false;
            }
        }
        public bool Open(enLightChannel channel, userLightParam lightParam)
        {
            try
            {
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_1:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F0={0},L0={1}#", 1, lightParam.ch1_Value));
                        break;
                    case enLightChannel.Channel_2:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F1={0},L1={1}#", 1, lightParam.ch2_Value));
                        break;
                    case enLightChannel.Channel_3:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F2={0},L2={1}#", 1, lightParam.ch3_Value));
                        break;
                    case enLightChannel.Channel_4:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F3={0},L3={1}#", 1, lightParam.ch4_Value));
                        break;
                    case enLightChannel.Channel_5:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F4={0},L4={1}#", 1, lightParam.ch5_Value));
                        break;
                    case enLightChannel.Channel_6:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F5={0},L5={1}#", 1, lightParam.ch6_Value));
                        break;
                    case enLightChannel.Channel_7:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F6={0},L6={1}#", 1, lightParam.ch7_Value));
                        break;
                    case enLightChannel.Channel_8:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F7={0},L7={1}#", 1, lightParam.ch8_Value));
                        break;
                    case enLightChannel.Channel_9:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F8={0},L8={1}#", 1, lightParam.ch9_Value));
                        break;
                    case enLightChannel.Channel_10:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F9={0},L9={1}#", 1, lightParam.ch10_Value));
                        break;
                    case enLightChannel.Channel_11:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F10={0},L10={1}#", 1, lightParam.ch11_Value));
                        break;
                    case enLightChannel.Channel_12:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F11={0},L11={1}#", 1, lightParam.ch12_Value));
                        break;
                    case enLightChannel.Channel_13:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F12={0},L12={1}#", 1, lightParam.ch13_Value));
                        break;
                    case enLightChannel.Channel_14:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F13={0},L13={1}#", 1, lightParam.ch14_Value));
                        break;
                    case enLightChannel.Channel_15:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F14={0},L14={1}#", 1, lightParam.ch15_Value));
                        break;
                    case enLightChannel.Channel_16:
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F15={0},L15={1}#", 1, lightParam.ch16_Value));
                        break;

                    default:
                    case enLightChannel.Channel_All:// 表示同进打开四个通道
                        //SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F0={0},F1={1},F2={2},F3={3},F4={4},F5={5},F6={6},F7={7},F8={8},F9={9},F10={10},F11={11},F12={12},F13={13},F14={14},F15={15}," +
                        //    "L0={16},L1={17},L2={18},L3={19},L4={20},L5={21},L6={22},L7={23},L8={24},L9={25},L10={26},L11={27},L12={28},L13={29},L14={30},L15={31}#",
                        //     1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, lightParam.ch1_Value, lightParam.ch2_Value, lightParam.ch3_Value, lightParam.ch4_Value, lightParam.ch5_Value, lightParam.ch6_Value, lightParam.ch7_Value,
                        //     lightParam.ch8_Value, lightParam.ch9_Value, lightParam.ch10_Value, lightParam.ch11_Value, lightParam.ch12_Value, lightParam.ch13_Value, lightParam.ch14_Value, lightParam.ch15_Value));
                        SerialCommunicate.ASCII_Data.serialPort.WriteLine(string.Format("$F0={0},F1={1},F2={2},F3={3},L0={4},L1={5},L2={6},L3={7}#", 1, 1, 1, 1, lightParam.ch1_Value, lightParam.ch2_Value, lightParam.ch3_Value, lightParam.ch4_Value));

                        break;
                        ///////////////////////////
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + ":打开光源通道报错", new Exception());
                return false;
            }
            return true;
        }


    }

    public enum enWDPLightType
    {
        PD4_4CH_TYPE_ = 0,    //-----------PD4-4通道系列
        PD4_6CH_TYPE_ = 1,    //-----------PD4-6通道系列
        PD4_26_TYPE_ = 2, //-----------PD4-26通道以上系列
        PDS3_TYPE_ = 3,   //-----------PDS3系列
        PBT4_TYPE_ = 4,   //-----------PBT4系列
        PBT_8_TYPE_ = 5,  //-----------PBT-8通道系列
        PBT_27_TYPE_ = 6, //-----------PBT-27增亮系列.
        PBT_32_TYPE_ = 7, //-----------PBT-32通道系列
        PSC_DOT_TYPE_ = 8,    //-----------PSC点光系列
        PSC_LINE_TYPE_ = 9,   //-----------PSC线光系列
        PDMS_TYPE_ = 10,  //-----------PDMS组合系列
        PD3_4CH_TYPE_ = 11,   //-----------PD3 4通道系列
        PD3_6CH_TYPE_ = 12,   //-----------PD3 6通道系列
        PDMS2_TYPE_ = 13, //-----------PDMS2组合系列
        PDL_6_TYPE_ = 14, //-----------PDL6逻辑系列.
        PDL_27_TYPE_ = 15,    //-----------PDL27逻辑系列.
        PBL_4_TYPE_ = 16, //-----------PBL4逻辑系列.
        PHC_NORMAL_TYPE_ = 17,    //-----------PHC高速恒流标准系列.
        PHC_LOGIC_TYPE_ = 18,//-----------PHCL高速恒流逻辑系列.
        PD5_4CH_TYPE_ = 19,//-----------PD5-4通道系列
        PD5_8CH_TYPE_ = 20,//-----------PD5-8通道系列
    }




}
