using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class DaHengCameraSetting: CameraParam
    {
        public DaHengCameraSetting()
        {

        }
        public DaHengCameraSetting(string sensorName)
        {
            this.SensorName = sensorName;
        }


        public override bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            IsOk = IsOk && XML<DaHengCameraSetting>.Save(this, SavePath + @"\" + this.SensorName + ".xml");
            return IsOk;
        }
        public override object Read()
        {
            if (File.Exists(SavePath + @"\" + this.SensorName + ".xml"))
                return XML<DaHengCameraSetting>.Read(SavePath + @"\" + this.SensorName + ".xml");
            else
                return new DaHengCameraSetting();
        }
        public override object Read(string sensorName)
        {
            if (File.Exists(SavePath + @"\" + sensorName + ".xml"))
                return XML<DaHengCameraSetting>.Read(SavePath + @"\" + sensorName + ".xml");
            else
                return new DaHengCameraSetting(sensorName);
        }



    }




}
