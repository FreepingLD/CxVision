using Common;
using FunctionBlock;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CxVision
{
    public class AutoRunThreadPlc
    {
        private static object lockState = new object();
        private static AutoRunThreadPlc _Instance = null;
        private CancellationTokenSource cts;
        private CommunicationConfigParam[] CommunicationParamTrigger;

        public event PoseInfoEventHandler TriggerInfo;
        public event EventHandler StartRunInfo;
        public event EventHandler CancelRunInfo;

        private List<TreeNode> listTreeNode = new List<TreeNode>();
        private AutoRunThreadPlc()
        {

        }

        public static AutoRunThreadPlc Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new AutoRunThreadPlc();
                    }
                }
                return _Instance;
            }
        }


        public void Init()
        {
            ///// 获取程序视图节点
            foreach (var item in ProgramForm.Instance.ProgramDic.Values)
            {
                listTreeNode = item.GetTreeViewNodeTag();
            }
            string info;
            SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
            /// 获取坐标系数量,即触发的数量
            CommunicationParamTrigger = CommunicationConfigParamManger.Instance.GetCommunicationParamArray(enCommunicationCommand.TriggerFromPlc);
            this.cts = new CancellationTokenSource();
            foreach (var item in CommunicationParamTrigger)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        if (this.cts.IsCancellationRequested) break;
                        object readValue = CommunicationConfigParamManger.Instance.ReadValue(item);
                        if (readValue != null && readValue.ToString().Trim() == "1")
                        {
                            string FunctionNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.FunctionNo).ToString();
                            switch (FunctionNo)
                            {
                                case "grab":
                                case "Grab":
                                case "5":
                                case "11":
                                case "60":
                                    TreeNode node = GetExecuteNode(item.CoordSysName, out info); // 根据触发信号来获取执行节点
                                    CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                                    CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToPlc,1); // 清零结果信号，结果信号里应该加一个逻辑，为NG时不再写入其他值
                                    if (node != null)
                                    {
                                        LoggerHelper.Info("视觉收到PLC触发信号：" + item.CoordSysName + "-" + item.Address + "-" + info + "->开始执行");
                                        OperateResult state = ((IFunction)node.Tag)?.Execute(node);
                                        if (!state.Succss) // 如果有执行失败的节点，则写入 2 表示NG
                                        {
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToPlc, 2); // N G  
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.FunctionNoToPlc, FunctionNo); // Grab
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // trig
                                            LoggerHelper.Error(state.ErrorMessage + item.CoordSysName + "-" + item.Address + "-" + info + "-" + 2.ToString());
                                        }
                                    }
                                    else
                                        LoggerHelper.Error("PLC发送的指令内容错误，没有获取相应的执行节点：" + item.Address + "-" + info);
                                    LoggerHelper.Info("执行节点：" + node?.Text);
                                    LoggerHelper.Info("视觉处理完成：" + item.CoordSysName + "-" + item.Address + "-" + info);
                                    break;
                                case "Reset":
                                case "reset":
                                case "66": // 表示信号异常中断 
                                    SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.PLC复位中断;
                                    CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                                    break;
                            }
                        }
                        Thread.Sleep(100);
                    }
                });
            }
            ///////////////////////////////
            if (StartRunInfo != null)
            {
                StartRunInfo.Invoke(this, new EventArgs());
            }
        }

        public void Init(TreeViewWrapClass treeViewWrapClass)
        {
            ///// 获取程序视图节点
            listTreeNode = treeViewWrapClass.GetTreeViewNodeTag();
            string info;
            SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
            /// 获取坐标系数量,即触发的数量
            CommunicationParamTrigger = CommunicationConfigParamManger.Instance.GetCommunicationParamArray(enCommunicationCommand.TriggerFromPlc);
            this.cts = new CancellationTokenSource();
            foreach (var item in CommunicationParamTrigger)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        if (this.cts.IsCancellationRequested) break;
                        if (Convert.ToInt32(CommunicationConfigParamManger.Instance.ReadValue(item)) == 1)
                        {
                            string FunctionNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.FunctionNoToPlc).ToString();
                            switch (FunctionNo)
                            {
                                case "grab":
                                case "Grab":
                                case "60":
                                    TreeNode node = GetExecuteNode(item.CoordSysName, out info); // 根据触发信号来获取执行节点
                                    CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                                    if (node != null)
                                    {
                                        LoggerHelper.Info("视觉收到PLC触发信号：" + item.CoordSysName + "-" + item.Address + "-" + info + "->开始执行");
                                        OperateResult state = ((IFunction)node.Tag)?.Execute(node);
                                        if (!state.Succss) // 如果有执行失败的节点，则写入 2 表示NG
                                        {
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToPlc, 2); // N G  
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.FunctionNoToPlc, FunctionNo); // Grab
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // trig
                                            LoggerHelper.Error(state.ErrorMessage + item.CoordSysName + "-" + item.Address + "-" + info + "-" + 2.ToString());
                                        }
                                    }
                                    else
                                        LoggerHelper.Error("PLC发送的指令内容错误，没有获取相应的执行节点：" + item.Address + "-" + info);
                                    LoggerHelper.Info("执行节点：" + node?.Text);
                                    LoggerHelper.Info("视觉处理完成：" + item.CoordSysName + "-" + item.Address + "-" + info);
                                    break;
                                case "Reset":
                                case "reset":
                                case "66": // 表示信号异常中断 
                                    SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.PLC复位中断;
                                    CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                                    break;
                            }
                        }
                        Thread.Sleep(100);
                    }
                });
            }
            ///////////////////////////////
            if (StartRunInfo != null)
            {
                StartRunInfo.Invoke(this, new EventArgs());
            }
        }
        private TreeNode GetExecuteNode(string ExecuteNodeInfo)
        {
            TreeNode node = null;
            foreach (var item in listTreeNode)
            {
                if (item.Text.Contains(ExecuteNodeInfo))
                    node = item;
            }
            return node;
        }
        public TreeNode GetExecuteNode(enCoordSysName coordSysName, out string Info)
        {
            TreeNode node = null;
            Info = "";
            bool IsOk = true;
            foreach (var item in listTreeNode)
            {
                if (item.Checked) continue; // 如果节点是禁用的，该属性为 true;
                IsOk = true;
                object oo = ((BaseFunction)item.Tag).ResultInfo;
                if (oo == null) continue;
                switch (oo.GetType().Name)
                {
                    case "BindingList`1":
                        switch (oo.GetType().GetGenericArguments()[0].Name)
                        {
                            case nameof(PlcCommunicateInfo):
                                BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)item.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
                                foreach (var item2 in plcInfo)
                                {
                                    string value = CommunicationConfigParamManger.Instance.ReadValue(item2.CoordSysName, item2.CommunicationCommand)?.ToString();
                                    if (value == null)
                                    {
                                        IsOk = false;
                                        break;
                                    }
                                    item2.ReadValue = value;
                                    if (Info.Length == 0)
                                        Info = value;
                                    else
                                        Info += "," + value;
                                    ///////////////////////////////////////////////
                                    if (item2.IsCompare)
                                    {
                                        if (item2.TargetValue.Trim() == value.Trim() && item2.CoordSysName == coordSysName)
                                            IsOk = IsOk && true;
                                        else
                                            IsOk = IsOk && false;
                                    }
                                }
                                break;
                            default:
                                IsOk = false;
                                break;
                        }
                        break;
                    default:
                        IsOk = false;
                        break;

                }
                if (IsOk)
                {
                    node = item;
                    break;
                }
            }
            return node;
        }

        public void UnInit()
        {
            this.cts?.Cancel();
            SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
            ///// 获取程序视图节点
            this.listTreeNode.Clear();
            ///////////////////////////////
            if (CancelRunInfo != null)
            {
                CancelRunInfo.Invoke(this, new EventArgs());
            }

        }



    }
}
