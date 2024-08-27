using Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STIL_NET;

namespace Sensor
{
    public class StilPointLaserAdaptive
    {
        private StilPointLaser _laser;
        public StilPointLaserAdaptive(StilPointLaser laser)
        {
            this._laser = laser;
        }
        #region 数据访问

        /// <summary>
        /// 检查某字符串中是否存在某字符
        /// </summary>
        /// <param name="str">查找的字符串</param>
        /// <param name="vaule">要比较的字符</param>
        /// <returns></returns>
        public bool IsExist(string str, char value)
        {
            char[] ch = str.ToCharArray();
            int length = ch.Length;
            for (int i = 0; i < length; i++)
            {
                if (char.ToUpper(str[i]) == char.ToUpper(value)) return true;
            }
            return false;
        }

        /// <summary>
        /// 判断某字符串中是否包含有指定数字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="vaule"></param>
        /// <returns></returns>
        public bool IsEqual(string str, int value)
        {
            char[] ch = str.ToCharArray();
            char[] Digit = new char[20];
            int j = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (char.IsNumber(ch[i]))
                {
                    Digit[j] = ch[i];
                    j++;
                }
            }
            int a = int.Parse(string.Concat(Digit).Trim('\0'));
            if (int.Parse(string.Concat(Digit).Trim('\0')) == value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断某字符串中是否包含指定数字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="vaule"></param>
        /// <returns></returns>
        public bool IsContain(string str, int value)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(Convert.ToChar(str[i])))
                {
                    if (Convert.ToInt32(Convert.ToChar(str[i])) == value) return true;
                }
            }
            return false;

        }

        /// <summary>
        /// 获取字符串中的数字，不包含数字0
        /// </summary>
        /// <param name="str"></param>
        /// <param name="vaule"></param>
        /// <returns></returns>
        public int GetDigitInString(string str)
        {
            int value = 0;
            char[] ch = str.ToCharArray();
            char[] Digit = new char[20];
            int j = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (char.IsNumber(ch[i]))
                {
                    Digit[j] = ch[i];
                    j++;
                }
            }
            int.TryParse(string.Concat(Digit).Trim('\0'), out value);
            return value;
        }

        /// <summary>
        /// 获取当前使用的光学笔，并将其显示出来
        /// </summary>
        /// <param name="laser"></param>
        public int GetCurrentOpticalPen()
        {
            if (_laser.Sensor != null)
            {
                int a = _laser.Sensor.OpticalPen;
                return a;
                //  OpticalPen1comboBox.Invoke(new Action(() => { OpticalPen1comboBox.SelectedIndex = a; }));
            }
            return -1;
        }

        /// <summary>
        /// 获取当前阈值
        /// </summary>
        /// <param name="laser"></param>
        /// <param name="OpticalPen1comboBox"></param>
        public double GetCurrentThreshoud()
        {
            if (_laser.Sensor != null)
            {
                double value = _laser.Sensor.DistanceDetectionThreshold;
                return value;
            }
            // ThreshoudTexBox.Invoke(new Action(() => { ThreshoudTexBox.Text = value.ToString(); }));
            return -1;
        }

        /// <summary>
        /// 获取当前的测量模式
        /// </summary>
        /// <param name="laser"></param>
        /// <param name="OpticalPen1comboBox"></param>
        public int GetCurrentMeasureMode()
        {
            if (_laser.Sensor != null)
            {
                if (_laser.Sensor.MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                    return 0;
                // DistMeasureMode.Invoke(new Action(() => { DistMeasureMode.Checked = true; }));
                else
                    return 1;
                // ThickMeasureMode.Invoke(new Action(() => { ThickMeasureMode.Checked = true; }));
            }
            return -1;

        }

        /// <summary>
        /// 获取当前的测量频率
        /// </summary>
        /// <param name="laser"></param>
        /// <param name="OpticalPen1comboBox"></param>
        public int GetCurrentMeasureRate()
        {
            if (_laser.Sensor != null)
            {
                string str = SendCommand("$SRA?");
                if (str.Length > 0)
                    return GetDigitInString(str);
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 获取当前的测量频率
        /// </summary>
        /// <param name="laser"></param>
        /// <param name="OpticalPen1comboBox"></param>
        public int GetCurrentMeasurePeak()
        {
            if (_laser.Sensor != null)
            {
                string str = _laser.Sensor.SendCommand("$MSP?");
                if (str.Length > 0 && IsEqual(str, 0))
                {
                    return 0;
                }
                if (str.Length > 0 && IsEqual(str, 1))
                {
                    return 1;
                }
            }
            return -1;
        }


        /// <summary>
        /// 获取Dark后的最小可用频率
        /// </summary>
        /// <param name="Msensor"></param>
        /// <param name="textbox"></param>
        public int GetMinDarkRate()
        {
            if (_laser.Sensor != null)
            {
                int frequence = _laser.Sensor.MinDarkFrequency;
                return frequence;
            }
            return -1;

        }

        /// <summary>
        /// 获取驱动列表
        /// </summary>
        /// <param name="combobox"></param>
        public string[] GetUsbDriveList()
        {
            return sensor.UsbDeviceList;
        }

        /// <summary>
        /// 获取频率列表并赋值给Combox控件
        /// </summary>
        /// <param name="Msensor"></param>
        /// <param name="combobox"></param>
        public List<int> GetRateList()
        {
            if (_laser.Sensor != null)
                return _laser.Sensor.RateList;
            return null;
        }

        /// <summary>
        /// 获取光学笔列表
        /// </summary>
        /// <param name="form"></param>
        /// 
        public List<string> GetOpticalPenList()
        {
            if (_laser.Sensor != null)
                return _laser.Sensor.PenList;
            return null;
        }

        /// <summary>
        /// 获取测量模式列表
        /// </summary>
        /// <param name="Msensor"></param>
        /// <param name="combobox"></param>
        public string[] GetMeasureModeList()
        {
            string[] measuremode = { "DISTANCE_MODE", "THICKNESS_MODE" };
            return measuremode;
        }

        /// <summary>
        /// 向控制器发送命令，并获取其除字母以外的返回值
        /// </summary>
        /// <param name="msensor"></param>
        /// <param name="msp"></param>
        /// <returns></returns>
        public string SendCommand(string msp)
        {
            msp = msp.ToUpper();
            try
            {
                string s = _laser.Sensor.SendCommand(msp);
                char[] ch = s.ToCharArray();
                char[] Digit = new char[20];
                int j = 0;
                for (int i = 0; i < ch.Length; i++)
                {
                    if (char.IsNumber(ch[i]) || char.IsPunctuation(ch[i]))
                    {
                        Digit[j] = ch[i];
                        j++;
                    }
                }
                return string.Concat(Digit).Trim('\0');
            }
            catch
            {
                return "-1";
            }
        }

        public string GetCurrentRate()
        {
            try
            {
                string s;
                //if (_laser.Sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH || _laser.Sensor.SensorType == STIL_NET.enSensorType.CCS_OPTIMA_PLUS)
                //{
                //    switch (_laser.Sensor.FreeFrequency)
                //    {
                //        case 250:
                //        case 400:
                //            s = "0";
                //            break;
                //        case 800:
                //        case 500:
                //            s = "1";
                //            break;
                //        case 1000:
                //            s = "2";
                //            break;
                //        case 1600:
                //        case 2500:
                //            s = "3";
                //            break;
                //        case 2000:
                //        case 5000:
                //            s = "4";
                //            break;
                //        case 10000:
                //            s = "5";
                //            break;
                //        default:
                //            s = "0";
                //            break;
                //    }
                //}
                //else
                //{
                    switch (_laser.Sensor.FreeFrequency)
                    {
                        case 100:
                            s = "0";
                            break;
                        case 400:
                            s = "1";
                            break;
                        case 1000:
                            s = "2";
                            break;
                        case 2000:
                            s = "3";
                            break;
                        default:
                            s = "0";
                            break;
                    }
                //}
                char[] ch = s.ToCharArray();
                char[] Digit = new char[20];
                int j = 0;
                for (int i = 0; i < ch.Length; i++)
                {
                    if (char.IsNumber(ch[i]) || char.IsPunctuation(ch[i]))
                    {
                        Digit[j] = ch[i];
                        j++;
                    }
                }
                return string.Concat(Digit).Trim('\0');
            }
            catch
            {
                return "-1";
            }
        }
        /// <summary>
        /// 判定光源模式;1：自动光源；0：手动光源
        /// </summary>
        /// <param name="laser"></param>
        /// <returns></returns>
        public int GetLightMode()
        {
            if (_laser != null)
            {
                string str ;
                //if (_laser.Sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                //    str = "0";
                //else
                    str = _laser.Sensor.SendCommand("$AAL?");
                //  string str = SendCommand(laser, "$AAL?");
                if (str.Length > 0)
                {
                    if (IsEqual(str, 1)) return 1; //0:表示禁用自适应LED;
                    else return 0; //1:表示启用自适应LED
                }
            }
            return -1;

        }

        /// <summary>
        /// 获取手动模式下的亮度值
        /// </summary>
        /// <param name="laser"></param>
        public int GetManualModeLightValue() //trackBar1
        {
            string str ;
            //if (_laser.Sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
            //    str = _laser.Sensor.LedBrightness.ToString();
            //else
                str = _laser.Sensor.SendCommand("$LED?");
            string value = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(Convert.ToChar(str[i])))
                    value = value + str[i];
            }
            value.TrimStart('0');
            return int.Parse(value);
            // string str = SendCommand(laser, "$LED?");
        }

        /// <summary>
        /// 获取自动模式下的光源亮度值
        /// </summary>
        /// <param name="laser"></param>
        public int GetAutoModeLightValue() //trackBar1
        {
            string str = _laser.Sensor.SendCommand("$VTH?");
            string value = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsNumber(Convert.ToChar(str[i])))
                    value = value + str[i];
            }
            value.TrimStart('0');
            return int.Parse(value);
            // string str = SendCommand(laser, "$LED?");
        }

        public void SetManualModeLightValue(int value)
        {
            if (value < 0)
                value = 0;
            if (value > 4095)
                value = 4095;
            _laser.Sensor.LedBrightness = value;
        }

        public void SetAutoModeLightValue(int value)
        {
            if (value < 0)
                value = 0;
            if (value > 4095)
                value = 4095;
            SendCommand("$VTH" + value.ToString());
        }
        /// <summary>
        /// 获取触发模式列表
        /// </summary>
        /// <param name="laser"></param>
        /// <returns></returns>
        public string[] GetTrigModeList()
        {
            string[] measuremode = { "NONE", "TRS", "TRE" };
            return measuremode;
        }

        public void SetThreahoudl(double value)
        {
            SendCommand("$MNP" + value.ToString());
        }

        public string SaveParam()
        {
            return SendCommand("$SSU");
        }

        public void SetDistMeasureMode()
        {
            string str = SendCommand("$MOD0");
        }

        public void SetThickMeasureMode()
        {
            string str = SendCommand("$MOD1");
        }
        public string SetOpticalPen(int Index)
        {
            string jj = "$SEN" + "0" + Index.ToString();
            if (Index < 10) return SendCommand("$SEN" + "0" + Index.ToString());
            return SendCommand("$SEN" + Index.ToString());
        }

        public void SetManualLightMode()
        {
            string str = SendCommand("$AAL0");
        }

        public void SetAutoLightMode()
        {
            string str = SendCommand("$AAL1");
        }
        public string SetPresetRate(int Index)
        {
            return SendCommand("$SRA" + Index.ToString());
        }

        public void SetStrongPeak()
        {
            SendCommand("$MSP0");
        }
        public void SetFirstPeak()
        {
            SendCommand("$MSP1");
        }

        public int Dark()
        {
            SendCommand("$DRK");
            return GetMinDarkRate();
        }
        #endregion
    }
}
