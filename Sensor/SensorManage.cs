using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Sensor
{
    /// <summary>
    /// 用于打开软件中配置的所有传感器, 应该将这几个类实现成单实例类
    /// </summary>
    public class SensorManage
    {
        private static BindingList<ISensor> sensorList = new BindingList<ISensor>();
        private ISensor[] _sensor;
        private BindingList<SensorConnectConfigParam> listConfigPara;
        private static ISensor currentCamSensor;
        private static ISensor currentLaserSensor;
        private static BindingList<ISensor> cameraList = new BindingList<ISensor>();
        private static BindingList<ISensor> laserList = new BindingList<ISensor>();
        public static BindingList<ISensor> SensorList
        {
            get
            {
                return sensorList;
            }

            set
            {
                sensorList = value;
            }
        }
        public static ISensor CurrentCamSensor { get => currentCamSensor; set => currentCamSensor = value; }
        public static BindingList<ISensor> CameraList { get => cameraList; set => cameraList = value; }
        public static BindingList<ISensor> LaserList { get => laserList; set => laserList = value; }
        public static ISensor CurrentLaserSensor { get => currentLaserSensor; set => currentLaserSensor = value; }

        public SensorManage()
        {
            // 读取传感器配置文件 
            SensorConnectConfigParamManger.Instance.Read();
            this.listConfigPara = SensorConnectConfigParamManger.Instance.ConfigParamList;
        }
        public void Connect()
        {
            if (listConfigPara.Count == 0)
            {
                LoggerHelper.Warn("传感器配置文件中不包含任何传感器");
                return;
            }
            try
            {
                /// 先根据配置的传感器数量，将所有相应对象先创建出来
                this._sensor = new ISensor[listConfigPara.Count];
                for (int i = 0; i < listConfigPara.Count; i++)
                {
                    this._sensor[i] = SensorFactory.GetSensor(listConfigPara[i].SensorLinkLibrary);
                }
                int index = 0;
                foreach (var item in listConfigPara)
                {
                    try
                    {
                        //this._sensor = SensorFactory.GetSensor(item.SensorLinkLibrary); 创建对象这一步不要放到这里，有些传感器在创建对象时会影响其他已打开的传感器，所以需要先将所有传感器对象提前创建好
                        _sensor[index]?.Init();
                        if (_sensor[index] == null) continue;
                        sensorList.Add(_sensor[index]);  // 不管打开成功与否都要添加到集合中去     
                        if (_sensor[index].Connect(item)) // 在连接有时候要同步读取传感器的参数
                        {
                            LoggerHelper.Info("传感器：" + item.SensorName + " 打开成功");
                        }
                        else
                        {
                            LoggerHelper.Warn("传感器：" + item.SensorName + " 打开失败");
                            MessageBox.Show("传感器：" + item.SensorName + " 打开失败", "打开传感器");
                        }
                        switch (item.SensorType)
                        {
                            case enUserSensorType.线阵相机:
                            case enUserSensorType.面阵相机:
                                currentCamSensor = _sensor[index];
                                cameraList.Add(_sensor[index]);
                                break;
                            case enUserSensorType.点激光:
                            case enUserSensorType.线激光:
                            case enUserSensorType.面激光:
                                currentLaserSensor = _sensor[index];
                                laserList.Add(_sensor[index]);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("传感器：" + item.SensorName + " 打开报错" + ex.ToString(), " 打开传感器");
                        LoggerHelper.Error("传感器：" + item.SensorName + " 打开报错", ex);
                    }
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开传感器报错" + ex.ToString(), " 打开传感器");
                LoggerHelper.Error("打开传感器报错", ex);
            }
        }
        public void DisConnect()
        {
            try
            {
                foreach (var item in SensorList)
                {
                    try
                    {
                        item.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Error("传感器:" + item.ConfigParam.SensorName + " 关闭时报错", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("打开传感器报错", ex);
            }
        }

        public static ISensor GetSensor(string name)
        {
            foreach (var item in sensorList)
            {
                if (item.Name == name) return item;
            }
            return null;
        }

        public static string[] GetSensorName()
        {
            string[] name = new string[sensorList.Count];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = sensorList[i].Name;
            }
            return name;
        }

        public static string[] GetLaserSensorName()
        {
            string[] name = new string[laserList.Count];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = laserList[i].Name;
            }
            return name;
        }

        public static string[] GetCamSensorName()
        {
            string[] name = new string[cameraList.Count];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = cameraList[i].Name;
            }
            return name;
        }




    }

}
