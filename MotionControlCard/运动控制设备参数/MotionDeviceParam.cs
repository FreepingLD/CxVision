using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
  public  class MotionDeviceParam
    {
        public string SavePath{ get; set; }
        public string DeviceName { get; set; }
        public MotionDeviceParam()
        {
            this.SavePath = "运动控制设备参数";
        }



    }
}
