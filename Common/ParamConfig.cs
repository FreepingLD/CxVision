
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
    /// <summary>
    /// 用于参数的设置和保存,这个类应该是一个单实例类，在程序打开时加载一次，关闭时保存一次
    /// </summary>
    [Serializable]
    public class ParamConfig
    {
        private string block1 = "[机台参数段]"; // 表示参数分段
        private double moveSpeed = 100;
        private double scanSpeed = 10;
        private double x_encoderResolution = 0.0002f;
        private double y_encoderResolution = 0.0002f;
        private double z_encoderResolution = 0.0002f;
        private double u_encoderResolution = 0.0002f;
        private double v_encoderResolution = 0.0002f;
        private double w_encoderResolution = 0.0002f;
        private bool enabeDisable_X = true;
        private bool enabeDisable_Y = true;
        private bool enabeDisable_Z = true;
        private bool enabeDisable_U = true;
        private bool enabeDisable_V = true;
        private bool enabeDisable_W = true;
        private double liftUp_Z = 10; // Z轴抬升高度
        private enMeasureEnvironmentConfig measureEnvironment = enMeasureEnvironmentConfig.影像仪测量;

        private string block2 = "[对射标定参数段]";
        private double standardThickValue = 1;
        private double cord_Gap = 0;

        private string block4 = "[Stil参数段]";
        private string trigMode = "NONE";
        private uint treNum = 1000;
        private string measureMode = "DistanceMeasure";
        private int stil_LedBrightness = 50;
        private int stil_ExposureTime = 500;
        private double stil_DetectionThreshold = 0.015f;
        private string stil_AltitudeModePeak = "StrongPeak";
        private string stil_ThickModePeak1 = "FirstPeak";
        private string stil_ThickModePeak2 = "SecondPeak";
        private double stil_PointPitch = 0.022f;
        private double stil_MeasureRange = 100f;


        private string block5 = "[3D可视化参数段]";
        private bool isShowCoordSys = false;
        private string viewType = "rainbow";
        private double imageWidthScale = 1.0f;
        private int nodeSize = 20;
        private int pointSize = 20;
        private int arrowLength = 50;
        private string colorAttrib = "coord_z";
        private bool depth_persistence = false;
        private string pointQuality = "low";
        private int imageUpdataTime = 100;
        private int oKNgSize = 100;
        private enFontPosition oKNgPosition = enFontPosition.右上角;
        private int oKNgRowOffset = 10;
        private int oKNgColOffset = 200;

        private string block6 = "[数据保存]";
        private string dataSaveTarget = "DataGridView";
        private int dataSaveGap = 10;
        private int dataUpdataGap = 100;
        private string dataOutputBinding = "true";

        private string block7 = "[校准配置]";
        private bool enableCameraCalibrate = true; // 启用相机补偿档
        private bool enableMachineCalibrate = true; // 启用机台补偿档


        // 产品参数
        private string productMarking;
        private string productSize;
        private string operatorName;


        /// //
        public double MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public double ScanSpeed { get => scanSpeed; set => scanSpeed = value; }
        public double X_encoderResolution { get => x_encoderResolution; set => x_encoderResolution = value; }
        public double Y_encoderResolution { get => y_encoderResolution; set => y_encoderResolution = value; }
        public double Z_encoderResolution { get => z_encoderResolution; set => z_encoderResolution = value; }
        public double U_encoderResolution { get => u_encoderResolution; set => u_encoderResolution = value; }
        public double V_encoderResolution { get => v_encoderResolution; set => v_encoderResolution = value; }
        public double W_encoderResolution { get => w_encoderResolution; set => w_encoderResolution = value; }
        public bool EnabeDisable_X { get => enabeDisable_X; set => enabeDisable_X = value; }
        public bool EnabeDisable_Y { get => enabeDisable_Y; set => enabeDisable_Y = value; }
        public bool EnabeDisable_Z { get => enabeDisable_Z; set => enabeDisable_Z = value; }
        public bool EnabeDisable_U { get => enabeDisable_U; set => enabeDisable_U = value; }
        public bool EnabeDisable_V { get => enabeDisable_V; set => enabeDisable_V = value; }
        public bool EnabeDisable_W { get => enabeDisable_W; set => enabeDisable_W = value; }
        public double LiftUp_Z { get => liftUp_Z; set => liftUp_Z = value; }
        public double StandardThickValue { get => standardThickValue; set => standardThickValue = value; }
        public double Cord_Gap { get => cord_Gap; set => cord_Gap = value; }
        public string TrigMode { get => trigMode; set => trigMode = value; }
        public uint TreNum { get => treNum; set => treNum = value; }
        public string MeasureMode { get => measureMode; set => measureMode = value; }
        public bool IsShowCoordSys { get => isShowCoordSys; set => isShowCoordSys = value; }
        public string ViewType { get => viewType; set => viewType = value; }
        public double ImageWidthScale { get => imageWidthScale; set => imageWidthScale = value; }
        public int Stil_LedBrightness { get => stil_LedBrightness; set => stil_LedBrightness = value; }
        public int Stil_ExposureTime { get => stil_ExposureTime; set => stil_ExposureTime = value; }
        public double Stil_DetectionThreshold { get => stil_DetectionThreshold; set => stil_DetectionThreshold = value; }
        public string Stil_AltitudeModePeak { get => stil_AltitudeModePeak; set => stil_AltitudeModePeak = value; }
        public string Stil_ThickModePeak1 { get => stil_ThickModePeak1; set => stil_ThickModePeak1 = value; }
        public string Stil_ThickModePeak2 { get => stil_ThickModePeak2; set => stil_ThickModePeak2 = value; }
        public double Stil_PointPitch { get => stil_PointPitch; set => stil_PointPitch = value; }
        public double Stil_MeasureRange { get => stil_MeasureRange; set => stil_MeasureRange = value; }
        public int NodeSize { get => nodeSize; set => nodeSize = value; }
        public int PointSize { get => pointSize; set => pointSize = value; }
        public string Block1 { get => block1; set => block1 = value; }
        public string Block2 { get => block2; set => block2 = value; }
        public string Block4 { get => block4; set => block4 = value; }
        public string Block5 { get => block5; set => block5 = value; }
        public string ColorAttrib { get => colorAttrib; set => colorAttrib = value; }
        public bool Depth_persistence { get => depth_persistence; set => depth_persistence = value; }
        public string PointQuality { get => pointQuality; set => pointQuality = value; }
        public enMeasureEnvironmentConfig MeasureEnvironment { get => measureEnvironment; set => measureEnvironment = value; }
        public int ImageUpdataTime { get => imageUpdataTime; set => imageUpdataTime = value; }
        public string DataSaveTarget { get => dataSaveTarget; set => dataSaveTarget = value; }
        public int DataSaveGap { get => dataSaveGap; set => dataSaveGap = value; }
        public int DataUpdataGap { get => dataUpdataGap; set => dataUpdataGap = value; }
        public string DataOutputBinding { get => dataOutputBinding; set => dataOutputBinding = value; }
        public int OKNgSize { get => oKNgSize; set => oKNgSize = value; }
        public enFontPosition OKNgPosition { get => oKNgPosition; set => oKNgPosition = value; }
        public int OKNgRowOffset { get => oKNgRowOffset; set => oKNgRowOffset = value; }
        public int OKNgColOffset { get => oKNgColOffset; set => oKNgColOffset = value; }
        public bool EnableCameraCalibrate { get => enableCameraCalibrate; set => enableCameraCalibrate = value; }
        public bool EnableMachineCalibrate { get => enableMachineCalibrate; set => enableMachineCalibrate = value; }
        public int ArrowLength { get => arrowLength; set => arrowLength = value; }
        public string ProductMarking { get => productMarking; set => productMarking = value; }
        public string ProductSize { get => productSize; set => productSize = value; }
        public string OperatorName { get => operatorName; set => operatorName = value; }






        /// <summary>
        /// 从启动目录中读取配置文件
        /// </summary>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public ParamConfig ReadParamConfig(string configFileName)
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
                                case "enMeasureEnvironmentConfig": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enMeasureEnvironmentConfig), value[1].Trim()));
                                    break;
                                case "enFontPosition": // 如果是自定的类型，这里一定要
                                    propertyInfo.SetValue(this, Enum.Parse(typeof(enFontPosition), value[1].Trim()));
                                    break;
                                default:
                                    MessageBox.Show("ParamConfig类中没有为类型：" + propertyInfo.PropertyType.Name + "指定相应的转换");
                                    break;
                            }
                        }

                    }
                }
            }
            else
            {
                return new ParamConfig();
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



    }


}
