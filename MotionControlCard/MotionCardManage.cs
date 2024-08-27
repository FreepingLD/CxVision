using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotionControlCard
{
    [Serializable]
    public class MotionCardManage
    {
        private static BindingList<IMotionControl> _cardList = new BindingList<IMotionControl>(); // 用于扩展多张运动控制卡
        private static IMotionControl currentCard;
        private static enCoordSysName currentCoordSys = enCoordSysName.CoordSys_0;
        private IMotionControl _card;
        private BindingList<DeviceConnectConfigParam> ConfigParaList;
        public static IMotionControl CurrentCard
        {
            get
            {
                return currentCard;
            }

            set
            {
                currentCard = value;
            }
        }

        public static BindingList<IMotionControl> CardList { get => _cardList; set => _cardList = value; }
        public static enCoordSysName CurrentCoordSys { get => currentCoordSys; set => currentCoordSys = value; }

        public MotionCardManage()
        {
            DeviceConnectConfigParamManger.Instance.Read();
            this.ConfigParaList = DeviceConnectConfigParamManger.Instance.DeviceConfigParamList;
        }
        public bool Connect()
        {
            if (ConfigParaList == null || ConfigParaList.Count == 0)
            {
                LoggerHelper.Warn("传感器配置文件中不包含任何传感器");
                return false;
            }
            try
            {
                foreach (var item in ConfigParaList)
                {
                    try
                    {
                        this._card = MotionCardFactory.GetMotionCard(item.DeviceType);
                        currentCard = this._card;
                        if (_card.Init(item))
                        {
                            LoggerHelper.Info("运动控制器：" + item.DeviceType.ToString() + item.IpAdress + " 打开成功");
                        }
                        else
                        {
                            LoggerHelper.Info("运动控制器：" + item.DeviceType.ToString() + item.IpAdress + " 打开失败");
                            MessageBox.Show("运动控制器：" + item.DeviceType.ToString() + item.IpAdress + " 打开失败", " 打开控制卡或PLC");
                        }
                        _cardList.Add(_card);
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Error("运动控制器：" + item.DeviceType.ToString() + item.IpAdress + " 打开报错", ex);
                        MessageBox.Show("运动控制器：" + item.DeviceType.ToString() + item.IpAdress + " 打开报错" + ex, " 打开控制卡或PLC");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("打开运动控制器报错", ex);
            }
            return true;
        }
        public void DisConnect()
        {
            try
            {
                foreach (var item in _cardList)
                {
                    item.UnInit();
                }
            }
            catch
            {

            }
        }
        public static IMotionControl GetCard(string cardName)
        {
            if (cardName == null)
                cardName = "";
            foreach (var item in _cardList)
            {
                if (item.Name == cardName)
                    return item;
            }
            return null;
        }
        public static IMotionControl GetCard(enCoordSysName coordSys,enCoordSysName MapCoordSys = enCoordSysName.NONE)
        {
            string CardName = CoordSysConfigParamManger.Instance.GetCardName(coordSys.ToString());
            foreach (var item in _cardList)
            {
                if (item.Name == CardName)
                    return item;
            }
            return null;
        }
        public static IMotionControl GetCardByCoordSysName(string coordSys, string mapCoordSys = "NONE")
        {
            string CardName = CoordSysConfigParamManger.Instance.GetCardName(coordSys);
            foreach (var item in _cardList)
            {
                if (item.Name == CardName)
                    return item;
            }
            return null;
        }
    }
}
