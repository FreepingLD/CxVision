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
    public class LiyiPointSensorSetting : LaserParam
    {
        public LiyiPointSensorSetting()
        { }
        public LiyiPointSensorSetting(string sensorName)
        {
            this.SensorName = sensorName;
        }


        private int frequency = 1000;
        private int minFrequency = 100;
        private int opticalPenIndex = 0;
        private enUserMeasureMode measureMode = enUserMeasureMode.Distance;
        private enUserPeakMode peakMode = enUserPeakMode.StrongestPeak;
        private double threshold = 0.012;
        private enUserLightMode lightMode = enUserLightMode.Auto;
        private double ledBright = 100;
        private int waiteAcqCount = 1000;
        private enUserTrigerMode trigMode = enUserTrigerMode.NONE;

        public int Frequency
        {
            get
            {
                return frequency;
            }

            set
            {
                frequency = value;
            }
        }
        public int MinFrequency
        {
            get
            {
                return minFrequency;
            }

            set
            {
                minFrequency = value;
            }
        }
        public int OpticalPenIndex
        {
            get
            {
                return opticalPenIndex;
            }

            set
            {
                opticalPenIndex = value;
            }
        }
        public enUserMeasureMode MeasureMode
        {
            get
            {
                return measureMode;
            }

            set
            {
                measureMode = value;
            }
        }
        public enUserPeakMode PeakMode
        {
            get
            {
                return peakMode;
            }

            set
            {
                peakMode = value;
            }
        }
        public double Threshold
        {
            get
            {
                return threshold;
            }

            set
            {
                threshold = value;
            }
        }
        public enUserLightMode LightMode
        {
            get
            {
                return lightMode;
            }

            set
            {
                lightMode = value;
            }
        }
        public double LedBright
        {
            get
            {
                return ledBright;
            }

            set
            {
                ledBright = value;
            }
        }
        public int WaiteAcqCount
        {
            get
            {
                return waiteAcqCount;
            }

            set
            {
                waiteAcqCount = value;
            }
        }
        public enUserTrigerMode TrigMode
        {
            get
            {
                return trigMode;
            }

            set
            {
                trigMode = value;
            }
        }

        /// <summary>
        /// 从启动目录中读取配置文件
        /// </summary>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public LiyiPointSensorSetting ReadParamConfig(string configFileName)
        {
            if (File.Exists(configFileName))
            {
                using (StreamReader reader = new StreamReader(configFileName))
                {
                    int count = 0;
                    string[] value;
                    string name;
                    Type type = this.GetType();
                    PropertyInfo propertyInfo;
                    while (true)
                    {
                        name = reader.ReadLine();
                        if (name == null)
                        {
                            count++;
                            if (count == 100) break;
                            else continue;
                        }
                        else
                            count = 0;
                        value = name.Split('=', '\t');
                        propertyInfo = type.GetProperty(value[0].Trim());
                        if (propertyInfo != null)
                        {
                            switch (propertyInfo.PropertyType.Name)
                            {
                                case "Double":
                                    propertyInfo.SetValue(this, double.Parse(value[1].Trim()));
                                    break;
                                case "Single":
                                    propertyInfo.SetValue(this, float.Parse(value[1].Trim()));
                                    break;
                                case "Int32":
                                    propertyInfo.SetValue(this, Int32.Parse(value[1].Trim()));
                                    break;
                                case "Int16":
                                    propertyInfo.SetValue(this, Int16.Parse(value[1].Trim()));
                                    break;
                                case "Int64":
                                    propertyInfo.SetValue(this, Int64.Parse(value[1].Trim()));
                                    break;
                                case "String":
                                    propertyInfo.SetValue(this, value[1].Trim());
                                    break;
                                case "Boolean":
                                    propertyInfo.SetValue(this, Boolean.Parse(value[1].Trim()));
                                    break;
                                case "UInt32":
                                    propertyInfo.SetValue(this, UInt32.Parse(value[1].Trim()));
                                    break;
                                case "UInt16":
                                    propertyInfo.SetValue(this, UInt16.Parse(value[1].Trim()));
                                    break;
                                case "UInt64":
                                    propertyInfo.SetValue(this, UInt64.Parse(value[1].Trim()));
                                    break;
                                default:
                                    throw new ArithmeticException(propertyInfo.PropertyType.Name + ":该属性类型未指定转换,请设置类型的属性转换");
                            }
                        }

                    }
                }
            }
            else
            {
                return new LiyiPointSensorSetting();
            }
            return this;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="Data"></param>
        /// <param name="OutPutMode"></param>
        public void SaveParamConfig(string configFileName)
        {
            try
            {
                using (StreamWriter write = new StreamWriter(configFileName, false))
                {
                    PropertyInfo[] propertyInfo = this.GetType().GetProperties();
                    for (int i = 0; i < propertyInfo.Length; i++)
                    {
                        write.WriteLine(propertyInfo[i].Name + "=" + propertyInfo[i].GetValue(this));
                    }
                }
            }
            catch
            {

            }
        }

        public override bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            IsOk = IsOk && XML<LiyiPointSensorSetting>.Save(this, SavePath + @"\" + this.SensorName + ".xml");
            return IsOk;
        }
        public override object Read()
        {
            if (File.Exists(SavePath + @"\" + this.SensorName + ".xml"))
                return XML<LiyiPointSensorSetting>.Read(SavePath + @"\" + this.SensorName + ".xml");
            else
                return new LiyiPointSensorSetting();
        }

        public override object Read(string sensorName)
        {
            if (File.Exists(SavePath + @"\" + sensorName + ".xml"))
                return XML<LiyiPointSensorSetting>.Read(SavePath + @"\" + sensorName + ".xml");
            else
                return new LiyiPointSensorSetting(sensorName);
        }



    }




}
