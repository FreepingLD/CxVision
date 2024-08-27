using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace MotionControlCard
{
    [Serializable]
    public class MotionControlCardParamSetting:MotionDeviceParam
    {
        private double x_encoderResolution = 0.0002;
        private double y_encoderResolution = 0.0002;
        private double z_encoderResolution = 0.0002;
        private double u_encoderResolution = 0.0002;
        private double v_encoderResolution = 0.0002;
        private double w_encoderResolution = 0.0002;

        private bool enabeDisable_X = true;
        private bool enabeDisable_Y = true;
        private bool enabeDisable_Z = true;
        private bool enabeDisable_U = true;
        private bool enabeDisable_V = true;
        private bool enabeDisable_W = true;

        /// /
        private bool invertAxisFeedBack_x = false;
        private bool invertAxisFeedBack_y = false;
        private bool invertAxisFeedBack_z = false;
        private bool invertAxisFeedBack_u = false;
        private bool invertAxisFeedBack_v = false;
        private bool invertAxisFeedBack_w = false;

        private bool invertAxisCommandPos_x = false;
        private bool invertAxisCommandPos_y = false;
        private bool invertAxisCommandPos_z = false;
        private bool invertAxisCommandPos_u = false;
        private bool invertAxisCommandPos_v = false;
        private bool invertAxisCommandPos_w = false;

        private bool invertJogAxisX = false;
        private bool invertJogAxisY = false;
        private bool invertJogAxisZ = false;
        private bool invertJogAxisU = false;
        private bool invertJogAxisV = false;
        private bool invertJogAxisW = false;

        private double transmissionRatio_x = 1;
        private double transmissionRatio_y = 1;
        private double transmissionRatio_z = 5;
        private double transmissionRatio_u = 1;
        private double transmissionRatio_v = 1;
        private double transmissionRatio_w = 1;

        public double X_encoderResolution { get => x_encoderResolution; set => x_encoderResolution = value; }
        public double Y_encoderResolution { get => y_encoderResolution; set => y_encoderResolution = value; }
        public double Z_encoderResolution { get => z_encoderResolution; set => z_encoderResolution = value; }
        public double U_encoderResolution { get => u_encoderResolution; set => u_encoderResolution = value; }
        public double V_encoderResolution { get => v_encoderResolution; set => v_encoderResolution = value; }
        public double W_encoderResolution { get => w_encoderResolution; set => w_encoderResolution = value; }
        public bool InvertJogAxisX { get => invertJogAxisX; set => invertJogAxisX = value; }
        public bool InvertJogAxisY { get => invertJogAxisY; set => invertJogAxisY = value; }
        public bool InvertJogAxisZ { get => invertJogAxisZ; set => invertJogAxisZ = value; }
        public bool InvertJogAxisU { get => invertJogAxisU; set => invertJogAxisU = value; }
        public bool InvertJogAxisV { get => invertJogAxisV; set => invertJogAxisV = value; }
        public bool InvertJogAxisW { get => invertJogAxisW; set => invertJogAxisW = value; }
        public bool InvertAxisFeedBack_x { get => invertAxisFeedBack_x; set => invertAxisFeedBack_x = value; } //InvertAxisFeedBack_x
        public bool InvertAxisFeedBack_y { get => invertAxisFeedBack_y; set => invertAxisFeedBack_y = value; } //InvertAxisFeedBack_y
        public bool InvertAxisFeedBack_z { get => invertAxisFeedBack_z; set => invertAxisFeedBack_z = value; } //InvertAxisFeedBack_z
        public bool InvertAxisFeedBack_u { get => invertAxisFeedBack_u; set => invertAxisFeedBack_u = value; }  //InvertAxisFeedBack_u
        public bool InvertAxisFeedBack_v { get => invertAxisFeedBack_v; set => invertAxisFeedBack_v = value; } //InvertAxisFeedBack_v
        public bool InvertAxisFeedBack_w { get => invertAxisFeedBack_w; set => invertAxisFeedBack_w = value; } //InvertAxisFeedBack_w


        public bool InvertAxisCommandPos_x { get => invertAxisCommandPos_x; set => invertAxisCommandPos_x = value; }
        public bool InvertAxisCommandPos_y { get => invertAxisCommandPos_y; set => invertAxisCommandPos_y = value; }
        public bool InvertAxisCommandPos_z { get => invertAxisCommandPos_z; set => invertAxisCommandPos_z = value; }
        public bool InvertAxisCommandPos_u { get => invertAxisCommandPos_u; set => invertAxisCommandPos_u = value; }
        public bool InvertAxisCommandPos_v { get => invertAxisCommandPos_v; set => invertAxisCommandPos_v = value; }
        public bool InvertAxisCommandPos_w { get => invertAxisCommandPos_w; set => invertAxisCommandPos_w = value; }
        public double TransmissionRatio_x { get => transmissionRatio_x; set => transmissionRatio_x = value; }
        public double TransmissionRatio_y { get => transmissionRatio_y; set => transmissionRatio_y = value; }
        public double TransmissionRatio_z { get => transmissionRatio_z; set => transmissionRatio_z = value; }
        public double TransmissionRatio_u { get => transmissionRatio_u; set => transmissionRatio_u = value; }
        public double TransmissionRatio_v { get => transmissionRatio_v; set => transmissionRatio_v = value; }
        public double TransmissionRatio_w { get => transmissionRatio_w; set => transmissionRatio_w = value; }
        public bool EnabeDisable_X { get => enabeDisable_X; set => enabeDisable_X = value; }
        public bool EnabeDisable_Y { get => enabeDisable_Y; set => enabeDisable_Y = value; }
        public bool EnabeDisable_Z { get => enabeDisable_Z; set => enabeDisable_Z = value; }
        public bool EnabeDisable_U { get => enabeDisable_U; set => enabeDisable_U = value; }
        public bool EnabeDisable_V { get => enabeDisable_V; set => enabeDisable_V = value; }
        public bool EnabeDisable_W { get => enabeDisable_W; set => enabeDisable_W = value; }



        /// <summary>
        /// 从启动目录中读取配置文件
        /// </summary>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public MotionControlCardParamSetting ReadParamConfig(string configFileName)
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
                                default:
                                    throw new ArithmeticException(propertyInfo.PropertyType.Name + ":该属性类型未指定转换,请设置类型的属性转换");
                                    //break;
                            }
                        }

                    }
                }
            }
            else
            {
                return new MotionControlCardParamSetting();
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


        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            IsOk = IsOk && XML<MotionControlCardParamSetting>.Save(this, SavePath + @"\" + this.DeviceName + ".xml");
            return IsOk;
        }
        public MotionControlCardParamSetting Read()
        {
            if (File.Exists(SavePath + @"\" + this.DeviceName + ".xml"))
                return XML<MotionControlCardParamSetting>.Read(SavePath + @"\" + this.DeviceName + ".xml");
            else
                return new MotionControlCardParamSetting();
        }

    }




}
