using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Light
{
    public interface ILightControl
    {
        BindingList<LightParam> LightParamList { get; set; }
        string Name { get; set; }
        LightConnectConfigParam ConfigParam { get; set; }
        bool Connect(LightConnectConfigParam param);
        bool DisConnect();

        /// <summary>
        /// 打开指令的通道或所有通道
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        bool Open(enLightChannel channel);

        /// <summary>
        /// 打开指令的通道或所有通道,并设置相应通道的亮度值   ,这个接口弃用
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        bool Open(enLightChannel channel, userLightParam lightParam);

        /// <summary>
        /// 关闭指定通道或所有通道
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        bool Close(enLightChannel channel);

        /// <summary>
        /// 设置所有通道的亮度值 ,这个接口弃用
        /// </summary>
        /// <param name="lightParam"></param>
        /// <returns></returns>
        bool SetLight(userLightParam lightParam); // 设置所有通道的亮度

        /// <summary>
        /// 设置指定通道的亮度值
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="lightValue"></param>
        /// <returns></returns>
        bool SetLight(enLightChannel channel, int lightValue);

        /// <summary>
        /// 获取指定通道的亮度值
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        int GetLight(enLightChannel channel);

    }

    [Serializable]
    public enum enLightChannel
    {
        //Channel_0 = 0,
        Channel_All = 0,
        Channel_1 = 1,  // 表示同时打开的通道数
        Channel_2 = 2,
        Channel_3 = 3,
        Channel_4 = 4,
        Channel_5 = 5,
        Channel_6 = 6,
        Channel_7 = 7,
        Channel_8 = 8,
        Channel_9 = 9,
        Channel_10 = 10,
        Channel_11 = 11,
        Channel_12 = 12,
        Channel_13 = 13,
        Channel_14 = 14,
        Channel_15 = 15,
        Channel_16 = 16,
      
    }

    [Serializable]
    public struct userLightParam
    {
        public int ch0_Value;
        public int ch1_Value;
        public int ch2_Value;
        public int ch3_Value;
        public int ch4_Value;
        public int ch5_Value;
        public int ch6_Value;
        public int ch7_Value;
        public int ch8_Value;
        public int ch9_Value;
        public int ch10_Value;
        public int ch11_Value;
        public int ch12_Value;
        public int ch13_Value;
        public int ch14_Value;
        public int ch15_Value;
        public int ch16_Value;
        //////////////
        public bool ch0_State;
        public bool ch1_State;
        public bool ch2_State;
        public bool ch3_State;
        public bool ch4_State;
        public bool ch5_State;
        public bool ch6_State;
        public bool ch7_State;
        public bool ch8_State;
        public bool ch9_State;
        public bool ch10_State;
        public bool ch11_State;
        public bool ch12_State;
        public bool ch13_State;
        public bool ch14_State;
        public bool ch15_State;
        public bool ch16_State;
    }





}
