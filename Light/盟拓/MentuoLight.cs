using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using MENTO_CONTROLLER;



namespace Light
{
    public class MentuoLight : LightBase, ILightControl
    {
        //private Mento_controller mento_Controller;
        private Cmt_controller mento_Controller;
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
                    if (this.LightParamList == null)
                        this.LightParamList = new System.ComponentModel.BindingList<LightParam>();
                    this.LightParamList.Add(new LightParam(i + 1));
                }
                // 控制器类型
                switch (param.ConnectType)
                {
                    case enUserConnectType.Network:
                        return false;

                    case enUserConnectType.USB:
                        return false;

                    case enUserConnectType.SerialPort:
                        //this.serialPort = new System.IO.Ports.SerialPort();
                        mento_Controller = new Cmt_controller();
                        //if (param != null)
                        //{
                        //    serialPort.PortName = param.PortName.ToString();
                        //    serialPort.BaudRate = param.BaudRate;
                        //    serialPort.Handshake = System.IO.Ports.Handshake.None;
                        //    serialPort.Parity = param.Parity;
                        //    serialPort.DataBits = param.DataBits;
                        //    serialPort.StopBits = param.StopBits;
                        //}
                        mento_Controller.Open_serial(param.PortName, (uint)param.BaudRate);
                        //this.serialPort.Open();
                        param.ConnectState = true;
                        //OnOpenEvent(this, new EventArgs());
                        return true;
                    case enUserConnectType.SerialNumber:
                        return false;
                    default:
                        return false;
                }
            }
            catch
            {
                param.ConnectState = false;
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
                        mento_Controller.Close_serial();
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
            byte light = 0;
            byte LightChannel;
            try
            {
                LightChannel = Convert.ToByte(channel-1);
                mento_Controller.Get_lum_ex(LightChannel, ref light);
                return light;
            }
            catch
            {
                LoggerHelper.Error(this.name + ":获取光源通道值报错", new Exception());
                return -1;
            }
        }
        public bool Open(enLightChannel channel = enLightChannel.Channel_All)
        {
            byte State = 0;
            bool rlt = false;
            Byte Ch;
            try
            {
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_All:
                        for (int i = 0; i < this.channelCount; i++)
                        {
                            Ch = Convert.ToByte((int)i);
                            rlt = mento_Controller.Get_state(Ch, ref State);
                            if (!rlt) return false;
                            if (State == 0) 
                            rlt = mento_Controller.Set_state(Ch, 1);
                        }
                        break;
                    default:
                        Ch = Convert.ToByte((int)channel - 1);
                        rlt = mento_Controller.Get_state(Ch, ref State);
                        if (!rlt) return false;
                        if (State == 0)
                            rlt = mento_Controller.Set_state(Ch, 1);
                        break;
                }
            }
            catch
            {
                LoggerHelper.Error(this.name + ":打开光源通道报错", new Exception());
                return false;
            }
            return rlt;
        }
        public bool Close(enLightChannel channel = enLightChannel.Channel_All)
        {
            byte State = 0;
            bool rlt = false;
            Byte Ch;
            try
            {
                switch (channel)
                {
                    case enLightChannel.Channel_All:
                        for (int i = 0; i < this.channelCount; i++)
                        {
                            Ch = Convert.ToByte((int)i);
                            rlt = mento_Controller.Get_state(Ch, ref State);
                            if (State == 1)
                                rlt = mento_Controller.Set_state(Ch, 0);
                        }
                        break;
                    default:
                        Ch = Convert.ToByte((int)channel - 1);
                        rlt = mento_Controller.Get_state(Ch, ref State);
                        if (!rlt) return false;
                        if (State == 0) return true;
                        rlt = mento_Controller.Set_state(Ch, 0);
                        break;
                }
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
            byte LightValue = 0;
            byte LightChannel = 0;
            bool rlt = true;
            byte state = 0;
            try
            {
                LightValue = Convert.ToByte(lightValue);
                LightChannel = Convert.ToByte((int)channel - 1);
                mento_Controller.Get_state(LightChannel, ref state);
                if(state == 1)
                {
                    rlt = mento_Controller.Set_lum_ex(LightChannel, LightValue);
                    this.lightParam[LightChannel].LightValue = lightValue;  // 通道号从0开始
                }
                else
                {
                    mento_Controller.Set_state(LightChannel, 1);
                    rlt = mento_Controller.Set_lum_ex(LightChannel, LightValue);
                    this.lightParam[LightChannel].LightValue = lightValue;  // 通道号从0开始
                }
                return rlt;
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
                return true;
                //SetLight(lightParam.c)
            }
            catch
            {
                LoggerHelper.Error(this.name + ":设置光源亮度报错", new Exception());
                return false;
            }
        }
        public bool Open(enLightChannel channel, userLightParam lightParam)
        {
            Byte Ch = 0;
            byte State = 0;
            bool rlt = true;
            try
            {
                Ch = Convert.ToByte((int)channel - 1);
                rlt = true;
                rlt = mento_Controller.Get_state(Ch, ref State);
                if (!rlt) return false;
                if (State == 1) return true;
                rlt = mento_Controller.Set_state(Ch, 1);
            }
            catch
            {
                LoggerHelper.Error(this.name + ":打开光源通道报错", new Exception());
                return false;
            }
            return rlt;
        }


    }





}
