using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class HalconParamSetting:CameraParam
    {
        public HalconParamSetting()
        {

        }
        public HalconParamSetting(string sensorName)
        {
            this.SensorName = sensorName;
        }
        public override bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            IsOk = IsOk && XML<HalconParamSetting>.Save(this, SavePath + @"\" + this.SensorName + ".xml");
            return IsOk;
        }
        public override object Read()
        {
            if (File.Exists(SavePath + @"\" + this.SensorName + ".xml"))
                return XML<HalconParamSetting>.Read(SavePath + @"\" + this.SensorName + ".xml");
            else
                return new HalconParamSetting();
        }
        public override object Read(string sensorName)
        {
            if (File.Exists(SavePath + @"\" + sensorName + ".xml"))
                return XML<HalconParamSetting>.Read(SavePath + @"\" + sensorName + ".xml");
            else
                return new HalconParamSetting(sensorName);
        }


    }
}
