using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class MemoryManager
    {
        private static object sycnObj = new object();
        private static MemoryManager _Instance;
        public static MemoryManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new MemoryManager();
                    }
                }
                return _Instance;
            }
        }
        private Dictionary<string, object> Variable
        {
            get;
            set;
        }

        private Dictionary<string, string> State { get; set; }

        private Dictionary<string, object> MeasureInfo { get; set; }

        public void AddValue(string key, object value)
        {
            if (MeasureInfo == null) MeasureInfo = new Dictionary<string, object>();
            if (MeasureInfo.ContainsKey(key))
            {
                MeasureInfo[key] = value;
            }
            else
            {
                MeasureInfo.Add(key, value);
            }
        }


        public object GetValue(string key, enFlag flag = enFlag.测量值)
        {
            object oo = "0";
            if (MeasureInfo == null) return oo;
            if (MeasureInfo.ContainsKey(key))
            {
                if (this.MeasureInfo[key] != null)
                {
                    switch (this.MeasureInfo[key].GetType().Name)
                    {
                        case "BindingList`1":
                        case "List`1":
                            switch (this.MeasureInfo[key].GetType().GenericTypeArguments[0].Name)
                            {
                                default:
                                    break;
                            }
                            break;
                        case nameof(MeasureResultInfo):
                            MeasureResultInfo info = this.MeasureInfo[key] as MeasureResultInfo;
                            switch (flag)
                            {
                                default:
                                case enFlag.NONE:
                                case enFlag.测量值:
                                    oo = info.Mea_Value;
                                    break;
                                case enFlag.标准值:
                                    oo = info.Std_Value;
                                    break;
                                case enFlag.上偏差:
                                    oo = info.LimitUp;
                                    break;
                                case enFlag.下偏差:
                                    oo = info.LimitDown;
                                    break;
                                case enFlag.结果:
                                    oo = info.State;
                                    break;
                                case enFlag.OK_NG:
                                case enFlag.JudgeData:
                                case enFlag.Int1_2:
                                case enFlag.测量值_结果:
                                    oo = string.Join(",", info.Mea_Value, info.State);
                                    break;
                                case enFlag.测量值_标准值:
                                    oo = string.Join(",", info.Mea_Value, info.Std_Value);
                                    break;
                                case enFlag.测量值_标准值_结果:
                                    oo = string.Join(",", info.Mea_Value, info.Std_Value,info.State);
                                    break;
                                case enFlag.测量值_标准值_上偏差:
                                    oo = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp);
                                    break;
                                case enFlag.测量值_标准值_上偏差_下偏差_结果:
                                    oo = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp, info.LimitDown, info.State);
                                    break;
                                case enFlag.测量值_标准值_上偏差_结果:
                                    oo = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp, info.State);
                                    break;
                                case enFlag.测量值_标准值_下偏差_结果:
                                    oo = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitDown, info.State);
                                    break;
                                case enFlag.数据标签:
                                    //oo = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp, info.LimitDown, info.State);
                                    break;
                            }
                            break;
                        case nameof(ReadCommunicateCommand):
                            ReadCommunicateCommand infoPLc = this.MeasureInfo[key] as ReadCommunicateCommand;
                            switch (flag)
                            {
                                default:
                                case enFlag.NONE:
                                case enFlag.测量值:
                                    oo = infoPLc.ReadValue;
                                    break;
                                case enFlag.测量值_结果:
                                    oo = string.Join(",", infoPLc.ReadValue);
                                    break;
                                case enFlag.测量值_标准值:
                                    oo = string.Join(",", infoPLc.ReadValue, infoPLc.TargetValue);
                                    break;
                            }
                            break;
                        case nameof(WriteCommunicateCommand):
                            WriteCommunicateCommand infoPLc2 = this.MeasureInfo[key] as WriteCommunicateCommand;
                            switch (flag)
                            {
                                default:
                                case enFlag.NONE:
                                case enFlag.测量值:
                                    oo = infoPLc2.WriteValue;
                                    break;
                                case enFlag.测量值_标准值_上偏差_下偏差_结果:
                                case enFlag.测量值_标准值:
                                    oo = string.Join(",", infoPLc2.WriteValue);
                                    break;
                            }
                            break;
                        case nameof(OcrResultInfo):
                            OcrResultInfo infoOcr = this.MeasureInfo[key] as OcrResultInfo;
                            switch (flag)
                            {
                                default:
                                case enFlag.NONE:
                                case enFlag.测量值:
                                    oo = infoOcr.Mea_Value;
                                    break;
                                case enFlag.标准值:
                                    oo = infoOcr.Std_Value;
                                    break;
                                case enFlag.结果:
                                    oo = infoOcr.State;
                                    break;
                                case enFlag.测量值_标准值:
                                    oo = string.Join(",", infoOcr.Mea_Value, infoOcr.Std_Value);
                                    break;
                                case enFlag.测量值_标准值_上偏差_下偏差_结果:
                                    oo = string.Join(",", infoOcr.Mea_Value, infoOcr.Std_Value, infoOcr.LimitUp, infoOcr.LimitDown, infoOcr.State);
                                    break;
                            }
                            break;
                        case nameof(StringResultInfo):
                            StringResultInfo info2 = this.MeasureInfo[key] as StringResultInfo;
                            switch (flag)
                            {
                                default:
                                case enFlag.NONE:
                                case enFlag.测量值:
                                    oo = info2.Mea_Value;
                                    break;
                                case enFlag.标准值:
                                    oo = info2.Std_Value;
                                    break;
                                case enFlag.上偏差:
                                    oo = info2.LimitUp;
                                    break;
                                case enFlag.下偏差:
                                    oo = info2.LimitDown;
                                    break;
                                case enFlag.结果:
                                    oo = info2.State;
                                    break;
                                case enFlag.测量值_标准值:
                                    oo = string.Join(",", info2.Mea_Value, info2.Std_Value);
                                    break;
                                case enFlag.测量值_标准值_上偏差_下偏差_结果:
                                    oo = string.Join(",", info2.Mea_Value, info2.Std_Value, info2.LimitUp, info2.LimitDown, info2.State);
                                    break;
                            }
                            break;
                        case nameof(RobotJawParam):
                            RobotJawParam info3 = this.MeasureInfo[key] as RobotJawParam;
                            switch (flag)
                            {
                                default:
                                case enFlag.NONE:
                                case enFlag.X:
                                    oo = info3.X;
                                    break;
                                case enFlag.Y:
                                    oo = info3.Y;
                                    break;
                                case enFlag.Angle:
                                    oo = info3.Angle;
                                    break;
                            }
                            break;
                    }
                }
            }
            return oo;
        }

        public string[] GetKeysValue()
        {
            if (MeasureInfo == null) return new string[0];
            string[] key = new string[MeasureInfo.Count];
            MeasureInfo.Keys.CopyTo(key, 0);
            return key;
        }

        public void Clear()
        {
            if (this.Variable != null)
                this.Variable.Clear();
            if (this.State != null)
                this.State.Clear();
            if (this.MeasureInfo != null)
                this.MeasureInfo.Clear();
        }

        public void Remove(string key)
        {
            if (key != null)
            {
                if (this.Variable.ContainsKey(key))
                    this.Variable.Remove(key);
                if (this.MeasureInfo.ContainsKey(key))
                    this.MeasureInfo.Remove(key);
            }
        }

        public void RemoveOf(string key)
        {
            if (this.Variable != null && key != null)
            {
                foreach (KeyValuePair<string, object> item in this.Variable)
                {
                    if (item.Key.Contains(key))
                        this.Variable.Remove(item.Key);
                }
                foreach (KeyValuePair<string, object> item in this.MeasureInfo)
                {
                    if (item.Key.Contains(key))
                        this.MeasureInfo.Remove(item.Key);
                }
            }
        }


    }
}
