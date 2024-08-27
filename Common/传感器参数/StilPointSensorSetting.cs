using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Common
{
    [Serializable]
    public class StilPointSensorSetting : LaserParam
    {
        public StilPointSensorSetting()
        {

        }
        public StilPointSensorSetting(string sensorName)
        {
            this.SensorName = sensorName;
        }
        private bool cancelWaite =false;
        private int acqCount = 1;
        private enUserTrigerMode trigMode = enUserTrigerMode.NONE;
        private enUserLevelEdgeFlag levelEdgeFlag = enUserLevelEdgeFlag.FALLING_EDGE;
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

        public int AcqCount { get => acqCount; set => acqCount = value; }

        public bool CancelWaite { get => cancelWaite; set => cancelWaite = value; }
        public enUserLevelEdgeFlag LevelEdgeFlag { get => levelEdgeFlag; set => levelEdgeFlag = value; }

        /// <summary>
        /// 从启动目录中读取配置文件
        /// </summary>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public StilPointSensorSetting ReadParamConfig(string configFileName)
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
                                case "enUserMeasureMode": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enUserMeasureMode), value[1].Trim()));
                                    break;
                                case "enUserPeakMode": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enUserPeakMode), value[1].Trim()));
                                    break;
                                case "enUserLightMode": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enUserLightMode), value[1].Trim()));
                                    break;
                                case "enUserTrigerMode": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enUserTrigerMode), value[1].Trim()));
                                    break;
                                case "enUserTriggerSource": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enUserTriggerSource), value[1].Trim()));
                                    break;
                                case "enUserLevelEdgeFlag": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enUserLevelEdgeFlag), value[1].Trim()));
                                    break;
                                default:
                                    throw new ArithmeticException(propertyInfo.PropertyType.Name+":该属性类型未指定转换,请设置类型的属性转换");
                                    //break;
                            }
                        }

                    }
                }
            }
            else
            {
                return new StilPointSensorSetting();
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
            IsOk = IsOk && XML<StilPointSensorSetting>.Save(this, SavePath + @"\" + this.SensorName + ".xml");
            return IsOk;
        }
        public override object Read()
        {
            if (File.Exists(SavePath + @"\" + this.SensorName + ".xml"))
                return XML<StilPointSensorSetting>.Read(SavePath + @"\" + this.SensorName + ".xml");
            else
                return new StilPointSensorSetting();
        }
        public override object Read(string sensorName)
        {
            if (File.Exists(SavePath + @"\" + sensorName + ".xml"))
                return XML<StilPointSensorSetting>.Read(SavePath + @"\" + sensorName + ".xml");
            else
                return new StilPointSensorSetting(sensorName);
        }

    }




}
