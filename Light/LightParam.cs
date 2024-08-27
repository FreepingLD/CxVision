using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    [Serializable]
    public class LightParam
    {
        public string LightName { get; set; }
        public enLightChannel Channel { get; set; }
        public int  LightValue { get; set; }
        public bool ChannelState { get; set; } // 用于控制是否打开该通道


        public LightParam()
        {
            this.LightName = "";
            this.Channel = enLightChannel.Channel_1;
            this.LightValue = 0;
            this.ChannelState = true;
        }
        public LightParam(int  Channel=1)
        {
            this.LightName = "NONE";
            this.Channel = (enLightChannel)Channel;
            this.LightValue = 0;
            this.ChannelState = true;
        }
        public LightParam Clone()
        {
            LightParam lightParam = new LightParam();
            lightParam.LightName = this.LightName;
            lightParam.LightValue = this.LightValue;
            lightParam.Channel = this.Channel;
            lightParam.ChannelState = this.ChannelState;
            return lightParam;
        }



        public bool SetLight()
        {
            //LightConnectManage.GetLight(this.LightName).Open(this.Channel);
            //bool isOk = true;
            return  LightConnectManage.GetLight(this.LightName).SetLight(this.Channel, this.LightValue);
        }
        public int GetLight()
        {
            return LightConnectManage.GetLight(this.LightName).GetLight(this.Channel);
        }
        public bool Open()
        {
            bool isOk = true;
            isOk = LightConnectManage.GetLight(this.LightName).Open(this.Channel);
            return isOk; //&& LightConnectManage.GetLight(this.LightName).SetLight(this.Channel, this.LightValue);
        }
        public bool Close()
        {
            return LightConnectManage.GetLight(this.LightName).Close(this.Channel);
        }


    }
}
