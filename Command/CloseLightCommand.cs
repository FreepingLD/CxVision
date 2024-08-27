using Light;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{

    /// <summary>
    /// 运动命令类,发送命令来控制机台运动 
    /// </summary>
    public class CloseLightCommand : CommandBase
    {
        private ILightControl i_light; // 对于光源控制命令来说，光源接口是执行者，所以该接口需要

        public CloseLightCommand(ILightControl light)
        {
            this.i_light = light;
        }

        public override void execute()
        {
            if (i_light != null)
                i_light.Close(enLightChannel.Channel_All);
        }



    }

}
