using AlgorithmsLibrary;
using Command;
using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace FunctionBlock
{

    [Serializable]
    [DefaultProperty(nameof(SendContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class SendDataRFID : BaseFunction, IFunction
    {
        [NonSerialized]
        private string[] _sendContent;

        [DisplayName("发送内容")]
        [DescriptionAttribute("输出属性")]
        public string[] SendContent { get { return _sendContent; } set { _sendContent = value; } }

        public BindingList<SendDataCommand> SendDataList { get; set; }
        public SendDataRFID()
        {
            this.SendDataList = new BindingList<SendDataCommand>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                bool isOk = true;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                Dictionary<string, string> dicResult = new Dictionary<string, string>();
                object value = "";
                bool result = true;
                //////////////////////////////////////////////////////
                if (this.SendDataList != null && this.SendDataList.Count > 0)
                {
                    enCoordSysName coordSysName = this.SendDataList[0].CoordSysName;
                    string grabNo = CommunicationConfigParamManger.Instance.ReadValue(coordSysName, enCommunicationCommand.GrabNo)?.ToString().Trim();
                    string ColCount = CommunicationConfigParamManger.Instance.ReadValue(coordSysName, enCommunicationCommand.ColCount)?.ToString().Trim();
                    switch (grabNo)
                    {
                        case "1":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X1, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X1);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X1.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X1.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X1.ToString()] = value;
                                        ///////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y1, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y1);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y1.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y1.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y1.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta1, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta1);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta1.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta1.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta1.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result1, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result1);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result1.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result1.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result1.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToSocket:
                                    case enCommunicationCommand.TriggerToPlc:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result  = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                        case "2":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X2, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X2);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X2.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X2.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X2.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y2, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y2);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y2.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y2.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y2.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta2, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta2);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta2.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta2.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta2.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result2, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result2);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result2.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result2.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result2.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToSocket:
                                    case enCommunicationCommand.TriggerToPlc:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                        case "3":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X3, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X3);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X3.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X3.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X3.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y3, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y3);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y3.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y3.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y3.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta3, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta3);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta3.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta3.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta3.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result3, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result3);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result3.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result3.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result3.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToSocket:
                                    case enCommunicationCommand.TriggerToPlc:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                        case "4":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X4, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X4);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X4.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X4.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X4.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y4, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y4);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y4.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y4.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y4.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta4, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta4);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta4.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta4.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta4.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result4, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result4);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result4.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result4.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result4.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;

                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToSocket:
                                    case enCommunicationCommand.TriggerToPlc:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                }
                            }
                            break;
                        case "5":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X5, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X5);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X5.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X5.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X5.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y5, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y5);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y5.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y5.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y5.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta5, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta5);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta5.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta5.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta5.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, item.FlagBit);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result5, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result5);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result5.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result5.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result5.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToSocket:
                                    case enCommunicationCommand.TriggerToPlc:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                        case "6":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X6, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X6);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X6.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X6.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X6.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y6, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y6);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y6.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y6.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y6.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta6, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta6);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta6.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta6.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta6.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result6, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result6);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result6.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result6.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result6.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToSocket:
                                    case enCommunicationCommand.TriggerToPlc:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result=  CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                        case "7":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X7, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X7);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X7.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X7.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X7.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y7, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y7);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y7.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y7.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y7.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta7, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta7);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta7.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta7.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta7.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result7, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result7);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result7.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result7.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result7.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToSocket:
                                    case enCommunicationCommand.TriggerToPlc:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                        case "8":
                            foreach (var item in this.SendDataList)
                            {
                                if (!item.IsActive) continue;
                                switch (item.CommunicationCommand)
                                {
                                    case enCommunicationCommand.WriteOffset_X:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X8, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_X8);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_X8.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_X8.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_X8.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Y:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y8, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Y8);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Y8.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Y8.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Y8.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.WriteOffset_Theta:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta8, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.WriteOffset_Theta8);
                                        if (!dic.ContainsKey(enCommunicationCommand.WriteOffset_Theta8.ToString()))
                                            dic.Add(enCommunicationCommand.WriteOffset_Theta8.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.WriteOffset_Theta8.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.Write_Result:
                                        value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.Write_Result8, value);
                                        isOk = isOk && result;
                                        value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.Write_Result8);
                                        if (!dic.ContainsKey(enCommunicationCommand.Write_Result8.ToString()))
                                            dic.Add(enCommunicationCommand.Write_Result8.ToString(), value);
                                        else
                                            dic[enCommunicationCommand.Write_Result8.ToString()] = value;
                                        //////////////////////////////////////////////////////////////////
                                        if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                        {
                                            if (result)
                                                dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                            else
                                                dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                        }
                                        else
                                        {
                                            if (result)
                                                dicResult[item.CommunicationCommand.ToString()] = "OK";
                                            else
                                                dicResult[item.CommunicationCommand.ToString()] = "NG";
                                        }
                                        break;
                                    case enCommunicationCommand.TriggerToPlc:
                                    case enCommunicationCommand.TriggerToSocket:
                                        if (grabNo == ColCount)
                                        {
                                            result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                            isOk = isOk && result;
                                        }
                                        break;
                                    case enCommunicationCommand.ResultToSocket:
                                    case enCommunicationCommand.ResultToPlc:
                                    case enCommunicationCommand.Result:
                                        string state = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果)?.ToString();
                                        switch (item.FlagBit)
                                        {
                                            case enFlag.OK:
                                            case enFlag.NG:
                                            case enFlag.OK_NG:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)  // 屏蔽检测
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    else
                                                        result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "NG");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, "OK");
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;

                                            default:
                                            case enFlag.Int1_2:
                                            case enFlag.NONE:
                                                if (state == "NG" || state.Contains("NG"))
                                                {
                                                    if (SystemParamManager.Instance.SysConfigParam.ShieldDetect) // 屏蔽检测
                                                        result= CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    else
                                                        result= CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 2);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                else
                                                {
                                                    result = CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, item.CommunicationCommand, 1);
                                                    isOk = isOk && result;
                                                    value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand);
                                                    if (!dic.ContainsKey(enCommunicationCommand.Result.ToString()))
                                                        dic.Add(enCommunicationCommand.Result.ToString(), value);
                                                    else
                                                        dic[enCommunicationCommand.Result.ToString()] = value;
                                                    //////////////////////////////////////////////////////////////////
                                                    if (!dicResult.ContainsKey(item.CommunicationCommand.ToString()))
                                                    {
                                                        if (result)
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "OK");
                                                        else
                                                            dicResult.Add(item.CommunicationCommand.ToString(), "NG");
                                                    }
                                                    else
                                                    {
                                                        if (result)
                                                            dicResult[item.CommunicationCommand.ToString()] = "OK";
                                                        else
                                                            dicResult[item.CommunicationCommand.ToString()] = "NG";
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
                this.CreateResultInfo(dic.Count + 1); // 因为要输出一个时间来，所以这里需要把 数据的数量 + 1 
                this._sendContent = new string[dic.Count];
                int index = 0;
                foreach (KeyValuePair<string, object> item in dic)
                {
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[index].SetValue(this.name, item.Key, item.Value.ToString());
                    if (dicResult.ContainsKey(item.Key))
                        ((BindingList<OcrResultInfo>)this.ResultInfo)[index].State = dicResult[item.Key];
                    this._sendContent[index] = item.Value.ToString();
                    index++;
                }
                stopwatch.Stop();
                ((BindingList<OcrResultInfo>)this.ResultInfo)[dic.Count].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                dic.Clear();
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "->发送数据:" + string.Join(",", this._sendContent) + "成功");
                else
                    LoggerHelper.Error(this.name + "->发送数据:" + string.Join(",", this._sendContent) + "失败");
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "发送数据：" + "报错" + ex);
            }
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                default:
                    return this.name;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
                    return true;
            }
        }

        public void ReleaseHandle()
        {
            try
            {
                OnItemDeleteEvent(this, this.name);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "删除节点出错" + ex.ToString());
            }
        }

        public void Read(string path)
        {
            // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            // throw new NotImplementedException();
        }
        #endregion



    }

}
