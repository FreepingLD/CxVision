using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Light
{
    /// <summary>
    /// 用于打开软件中配置的所有传感器
    /// </summary>
    public class LightConnectManage
    {
        private static BindingList<ILightControl> _lightList = new BindingList<ILightControl>();
        private ILightControl _light;
        private BindingList<LightConnectConfigParam> listConfigPara;
        private static ILightControl _currentLight;
        public static BindingList<ILightControl> LightList
        {
            get
            {
                return _lightList;
            }

            set
            {
                _lightList = value;
            }
        }
        public static ILightControl CurrentLight { get => _currentLight; set => _currentLight = value; }


        public LightConnectManage()
        {
            LightConnectConfigParamManger.Instance.Read();
            this.listConfigPara = LightConnectConfigParamManger.Instance.LightConfigParamList;
        }
        public bool Connect()
        {
            if (listConfigPara.Count == 0)
            {
                LoggerHelper.Warn("传感器配置文件中不包含任何传感器");
                return false;
            }
            try
            {
                foreach (var item in listConfigPara)
                {
                    try
                    {
                        this._light = LightFactory.GetLight(item.LightType);
                        _currentLight = this._light;
                        if (this._light == null) continue;
                        if (_light.Connect(item))
                        {
                            LoggerHelper.Info("光源控制器：" + item.LightType.ToString() + item.IpAdress + " 打开成功");
                        }
                        else
                        {
                            LoggerHelper.Info("光源控制器：" + item.LightType.ToString() + item.IpAdress + " 打开失败");
                            MessageBox.Show("光源控制器：" + item.LightType.ToString() + item.IpAdress + " 打开失败", "打开光源");
                        }
                        _lightList.Add(_light);
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Error("光源控制器：" + item.LightType.ToString() + item.IpAdress + " 打开报错", ex);
                        MessageBox.Show("光源控制器：" + item.LightType.ToString() + item.IpAdress + " 打开报错" + ex, " 打开光源");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("打开光源控制器报错", ex);
            }
            return true;
        }
        public void DisConnect()
        {
            try
            {
                foreach (var item in _lightList)
                {
                    item.DisConnect();
                }
            }
            catch
            {
                LoggerHelper.Error("关闭光源控制器报错", new Exception());
            }
        }

        public static ILightControl GetLight(string lightName)
        {
            foreach (var item in _lightList)
            {
                if (lightName == item.Name)
                    return item;
            }
            return null;
        }
        public static string [] GetLightName()
        {
            string[] name = new string[_lightList.Count];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = _lightList[i].Name;
            }
            return name;
        }


    }

}
