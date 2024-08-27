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
    public class OptLight : LightBase, ILightControl
    {
        private OPTControllerAPI OPTController = null;
        private long lRet = -1;
        public bool Connect(LightConnectConfigParam param)
        {
            try
            {
                OPTController = new OPTControllerAPI();
                this.ConfigParam = param;
                this.Name = param.LightName;
                this.connectAdress = param.IpAdress;
                this.channelCount = param.ChannelCount;
                param.ConnectState = false;
                lRet = -1;
                for (int i = 0; i < param.ChannelCount; i++)
                {
                    if (this.LightParamList == null)
                        this.LightParamList = new System.ComponentModel.BindingList<LightParam>();
                    this.LightParamList.Add(new LightParam(i + 1));
                }
                // 控制器类型
                switch (param.ConnectType)
                {
                    case enUserConnectType.TcpIp:
                    case enUserConnectType.Network:
                        lRet = OPTController.CreateEthernetConnectionByIP(param.IpAdress);
                        if (lRet == 0)
                            param.ConnectState = true;
                        else
                            param.ConnectState = false;
                        break;
                    case enUserConnectType.USB:
                        return false;

                    case enUserConnectType.SerialPort:
                        lRet = OPTController.InitSerialPort(param.PortName);
                        if (lRet == 0)
                            param.ConnectState = true;
                        else
                            param.ConnectState = false;
                        return true;
                    case enUserConnectType.SerialNumber:
                        lRet = lRet = OPTController.CreateEthernetConnectionBySN(param.IpAdress);
                        if (lRet == 0)
                            param.ConnectState = true;
                        else
                            param.ConnectState = false;
                        break;
                    default:
                        MessageBox.Show(param.ConnectType.ToString()+"该连接类型未实现!!!");
                        return false;
                }
            }
            catch (Exception ex)
            {
                param.ConnectState = false;
                LoggerHelper.Error("OPt光源控制器连接时报错", ex);
                return false;
            }
            return param.ConnectState;
        }
        public bool DisConnect()
        {
            try
            {
                lRet = -1;
                switch (connectType)
                {
                    case enUserConnectType.Network:
                        this.Close();
                        lRet = OPTController.DestroyEthernetConnect();
                        if (lRet == 0)
                            return true;
                        else
                            return false;

                    case enUserConnectType.USB:
                        return false;

                    case enUserConnectType.SerialPort:
                        this.Close();
                        lRet = OPTController.ReleaseSerialPort();
                        if (lRet == 0)
                            return true;
                        else
                            return false;
                    case enUserConnectType.SerialNumber:
                        this.Close();
                        lRet = OPTController.DestroyEthernetConnect();
                        if (lRet == 0)
                            return true;
                        else
                            return false;

                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("WoDePuLight:在关闭光源时报错", ex);
                return false;
            }
        }
        public int GetLight(enLightChannel channel)
        {
            byte light = 0;
            byte LightChannel;
            try
            {
                LightChannel = Convert.ToByte(channel);
                //mento_Controller.Get_lum_ex(LightChannel, ref light);
                return light;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + ":获取光源通道值报错", ex);
                return -1;
            }
        }
        public bool Open(enLightChannel channel = enLightChannel.Channel_All)
        {
            bool rlt = false;
            try
            {
                switch (channel) // 需要组合开，但不需要组合关
                {
                    case enLightChannel.Channel_All:
                        for (int i = 0; i < this.channelCount; i++)
                        {
                            lRet = OPTController.TurnOnChannel(i + 1);
                        }
                        if (lRet == 0)
                            rlt = true;
                        else
                            rlt = false;
                        break;
                    default:
                        lRet = OPTController.TurnOnChannel((int)channel);
                        if (lRet == 0)
                            rlt = true;
                        else
                            rlt = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + ":打开光源通道报错", ex);
                return false;
            }
            return rlt;
        }
        public bool Close(enLightChannel channel = enLightChannel.Channel_All)
        {
            bool rlt = false;
            try
            {
                switch (channel)
                {
                    case enLightChannel.Channel_All:
                        for (int i = 0; i < this.channelCount; i++)
                        {
                            lRet = OPTController.TurnOffChannel(i + 1);
                        }
                        if (lRet == 0)
                            rlt = true;
                        else
                            rlt = false;
                        break;
                    default:
                        lRet = OPTController.TurnOffChannel((int)channel);
                        if (lRet == 0)
                            rlt = true;
                        else
                            rlt = false;
                        break;
                }
            }
            catch(Exception ex)
            {
                LoggerHelper.Error(this.name + ":关闭光源通道报错", ex);
                return false;
            }
            return rlt;
        }
        public bool SetLight(enLightChannel channel, int lightValue)
        {
            byte LightValue = 0;
            byte LightChannel = 0;
            bool rlt = true;
            int state = -1;
            try
            {
                LightValue = Convert.ToByte(lightValue);
                LightChannel = Convert.ToByte((int)channel);
                this.lRet = OPTController.GetChannelState(LightChannel, ref state);
                this.lRet = OPTController.SetIntensity(LightChannel, LightValue);
                this.lightParam[LightChannel].LightValue = lightValue;  // 通道号从0开始
                if (this.lRet == 0)
                    rlt = true;
                else
                    rlt = false;
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
            bool rlt = true;
            try
            {
                lRet = OPTController.TurnOnMultiChannel(new int[] { (int)channel }, 1);
                this.lRet = OPTController.SetIntensity((int)channel, lightParam.ch0_Value);
                if (lRet == 0)
                    rlt = true;
                else

                    rlt = false;
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

