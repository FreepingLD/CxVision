using Common;
using FunctionBlock;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
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
        public event RecipeEventHandler RecipeInfo;

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

        public string[] GetLableName(int index)
        {
            string[] lables = new string[0];
            List<string> listLable = new List<string>();
            List<TreeNode> listTreeNode = new List<TreeNode>();
            List<TreeNode> listToolNode = new List<TreeNode>();
            foreach (var item in ProgramForm.Instance.ProgramDic.Values)
            {
                listTreeNode.AddRange(item.GetTreeViewNodeTag());
            }
            foreach (TreeNode item in listTreeNode)
            {
                switch (item.Tag?.GetType().Name)
                {
                    case nameof(JobUnit):
                        BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)item.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
                        if (plcInfo.Count > 0 && (int)plcInfo[0].CoordSysName == index)
                        {
                            foreach (TreeNode node in item.Nodes)
                            {
                                switch (node.Tag?.GetType().Name)
                                {
                                    case nameof(UserLable):
                                        listLable.Add(node.Text);
                                        break;
                                    default:
                                        if (node.Name.Contains("Tool"))
                                            listToolNode.Add(node);
                                        break;
                                }
                            }
                        }
                        else
                            continue;
                        break;
                }
            }
            foreach (TreeNode item in listToolNode)
            {
                foreach (TreeNode node in item.Nodes)
                {
                    switch (node?.Tag?.GetType().Name)
                    {
                        case nameof(UserLable):
                            listLable.Add($"{item.Text}.{node.Text}");
                            break;
                        default:
                            break;
                    }
                }
            }
            lables = listLable.ToArray();
            return lables;
        }




        public void Init()
        {
            ///// 获取程序视图节点 /////////////
            listTreeNode.Clear();
            foreach (var item in ProgramForm.Instance.ProgramDic.Values)
            {
                listTreeNode.AddRange(item.GetTreeViewNodeTag());
            }
            if (listTreeNode.Count == 0)
            {
                new UserMessageForm().ShowDialog("程序面板节点数量为 0");
                return;
            }
            
            //string[] name = GetLableName(1);
            /////////////////////////////////////////////////////
            string info;
            SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
            ImageAcqDevice.Instance.IsCamSource = true; // 在联机状态下，必需是使用相机来采集
            ImageAcqDevice.Instance.IsFileSource = false;
            /// 获取坐标系数量,即触发的数量
            CommunicationParamTrigger = CommunicationConfigParamManger.Instance.GetCommunicationParamArray(enCommunicationCommand.TriggerFromPlc);
            this.cts = new CancellationTokenSource();
            DateTime initTime = DateTime.Now;
            #region Method1
            for (int i = 0; i < CommunicationParamTrigger.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        Task.Run(() =>
                        {
                            RunThread(0);
                        });
                        break;
                    case 1:
                        Task.Run(() =>
                        {
                            RunThread(1);
                        });
                        break;
                    case 2:
                        Task.Run(() =>
                        {
                            RunThread(2);
                        });
                        break;
                    case 3:
                        Task.Run(() =>
                        {
                            RunThread(3);
                        });
                        break;
                    case 4:
                        Task.Run(() =>
                        {
                            RunThread(4);
                        });
                        break;
                    case 5:
                        Task.Run(() =>
                        {
                            RunThread(5);
                        });
                        break;
                    case 6:
                        Task.Run(() =>
                        {
                            RunThread(6);
                        });
                        break;
                    case 7:
                        Task.Run(() =>
                        {
                            RunThread(7);
                        });
                        break;
                    case 8:
                        Task.Run(() =>
                        {
                            RunThread(8);
                        });
                        break;
                    case 9:
                        Task.Run(() =>
                        {
                            RunThread(9);
                        });
                        break;
                }
            }
            #endregion


            #region Method2
            //foreach (var item in CommunicationParamTrigger)
            //{
            //    Task.Run(() =>
            //    {
            //        Stopwatch stopwatch = new Stopwatch();
            //        stopwatch.Restart();
            //        while (true)
            //        {
            //            if (this.cts.IsCancellationRequested) break;
            //            object triggerValue = CommunicationConfigParamManger.Instance.ReadValue(item);
            //            if (stopwatch.ElapsedMilliseconds > 10000)
            //            {
            //                if (CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.OnLine, 1)) // 在线
            //                    LoggerHelper.Info("视觉在线信号");
            //                stopwatch.Restart();
            //            }
            //            if (triggerValue != null && triggerValue.ToString().Trim() == "1")
            //            {
            //                AcqSource acqSource = AcqSourceManage.Instance.GetAcqSource(item.CoordSysName);
            //                LoggerHelper.Info("视觉收到PLC触发信号：" + item.CoordSysName + "_" + item.Address, acqSource?.Sensor?.Name);
            //                string FunctionNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.FunctionNo).ToString();
            //                string GrabNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.GrabNo).ToString();
            //                CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.FunctionNoToPlc, FunctionNo); // 初始化值
            //                CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.GrabNoToPlc, GrabNo); // 初始化值
            //                LoggerHelper.Info($"视觉收到PLC功能值:{FunctionNo}", acqSource?.Sensor?.Name);
            //                LoggerHelper.Info($"视觉收到PLC标签值:{GrabNo}", acqSource?.Sensor?.Name);
            //                ////////////////////////////////////////
            //                switch (FunctionNo)
            //                {
            //                    case "grab":
            //                    case "Grab":
            //                    case "5":
            //                    case "11":
            //                    case "22":
            //                    case "60":
            //                    case "GrabNo":
            //                    case "grabNo":
            //                    default:
            //                        info = "";
            //                        TreeNode node = this.GetExecuteNode(item.CoordSysName, acqSource?.Sensor?.Name, out info);
            //                        CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
            //                        IMotionControl motion = MotionCardManage.GetCard(item.CoordSysName);
            //                        switch (motion.ConnectConfigParam.DeviceModel)
            //                        {
            //                            case enDeviceModel.SocketClient:
            //                            case enDeviceModel.SocketServer:
            //                            case enDeviceModel.ModbusTcpClient:
            //                            case enDeviceModel.OmronFinsTcpClient:
            //                                CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToSocket, "init");   // 初始化结果状态为OK，0:表示在开始时，初始化结果地址中的值
            //                                break;
            //                            default:
            //                                CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToPlc, "init");   //初始化结果状态为OK， 0:表示在开始时，初始化结果地址中的值
            //                                break;
            //                        }
            //                        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.NONE; // 复位中断信号 
            //                        string grabNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.GrabNo).ToString();
            //                        string productID = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.ProductID).ToString();
            //                        string detectEdge = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.DetectEdge).ToString();
            //                        ////// 这里用一个线程来执行，这样才会实时监控下一次的触发信号 
            //                        //Task.Run(() =>
            //                        //{
            //                        if (node != null)
            //                        {
            //                            LoggerHelper.Info("执行节点: " + node.Text, acqSource?.Sensor?.Name);
            //                            OperateResult state = ((IFunction)node.Tag)?.Execute(node, $"GrabNo={grabNo}", $"DetectEdge={detectEdge}", $"ProductID={productID}");
            //                            if (!state.Succss) // 如果有执行失败的节点，则写入 2 表示NG
            //                            {
            //                                switch (motion.ConnectConfigParam.DeviceModel)
            //                                {
            //                                    case enDeviceModel.SocketClient:
            //                                    case enDeviceModel.SocketServer:
            //                                    case enDeviceModel.ModbusTcpClient:
            //                                    case enDeviceModel.OmronFinsTcpClient:
            //                                        CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
            //                                        break;
            //                                    default:
            //                                        CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
            //                                        break;
            //                                }
            //                                CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.FunctionNoToPlc, FunctionNo); // Grab
            //                                CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // trig
            //                                LoggerHelper.Error(state.ErrorMessage + item.CoordSysName + "_" + item.Address + "_" + info, acqSource?.Sensor?.Name);
            //                                LoggerHelper.Info($"执行节点：{node?.Text}->失败", acqSource?.Sensor?.Name);
            //                                LoggerHelper.Info($"视觉处理完成,触发信号源：{item.CoordSysName.ToString()}_{item.Address}", acqSource?.Sensor?.Name);
            //                            }
            //                            else
            //                            {
            //                                LoggerHelper.Info($"执行节点：{node?.Text}->成功", acqSource?.Sensor?.Name);
            //                                LoggerHelper.Info($"视觉处理完成,触发信号源：{item.CoordSysName.ToString()}_{item.Address}", acqSource?.Sensor?.Name);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            LoggerHelper.Info("没有绑定相应的信号执行节点,触发信号未绑定相应节点：" + item.Address + "_" + info, acqSource?.Sensor?.Name);
            //                            ////////////////////////////////////////////////
            //                            if (this.TriggerInfo != null)
            //                            {
            //                                double x = 0, y = 0, z = 0, theta = 0;
            //                                PoseInfoEventArgs poseInfo = new PoseInfoEventArgs(item.CoordSysName);
            //                                this.TriggerInfo.Invoke(item, poseInfo);
            //                                LoggerHelper.Info("触发信号发送到相应窗口完成：" + item.CoordSysName + "_" + item.Address, acqSource?.Sensor?.Name);
            //                            }
            //                            else
            //                                LoggerHelper.Error("窗口没有绑定触发信号：" + item.CoordSysName + "_" + item.Address, acqSource?.Sensor?.Name);
            //                        }
            //                        //});
            //                        break;

            //                    case "Reset":
            //                    case "reset":
            //                    case "66": // 表示信号异常中断 
            //                        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.PLC复位中断;
            //                        CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
            //                        break;

            //                    case "Run":
            //                    case "Stop":
            //                    case "run":
            //                    case "stop":
            //                    case "Save":
            //                    case "save":
            //                    case "SaveAs":
            //                    case "saveas":
            //                        LoggerHelper.Error("收到运行、停止指令信号：" + item.CoordSysName + "-" + item.Address);
            //                        RecipeInfo?.Invoke(new RecipeEventArgs(item.CoordSysName, FunctionNo));
            //                        break;

            //                    case "88": // 88 表示运动结束信号
            //                    case "End":
            //                        if (this.TriggerInfo != null)
            //                        {
            //                            double x = 0, y = 0, z = 0, theta = 0;
            //                            IMotionControl _card = MotionCardManage.GetCard(item.CoordSysName); // ByCoordSysName.ToString()
            //                            grabNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.GrabNo).ToString();
            //                            productID = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.ProductID).ToString();
            //                            detectEdge = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.DetectEdge).ToString();
            //                            _card?.GetAxisPosition(item.CoordSysName, enAxisName.X轴, out x);
            //                            _card?.GetAxisPosition(item.CoordSysName, enAxisName.Y轴, out y);
            //                            _card?.GetAxisPosition(item.CoordSysName, enAxisName.Z轴, out z);
            //                            _card?.GetAxisPosition(item.CoordSysName, enAxisName.Theta轴, out theta);
            //                            PoseInfoEventArgs poseInfo = new PoseInfoEventArgs();
            //                            poseInfo.CoordSysName = item.CoordSysName;
            //                            poseInfo.FunctionNo = FunctionNo;
            //                            poseInfo.ProductID = productID;
            //                            poseInfo.DetectEdge = detectEdge;
            //                            poseInfo.PoseInfo = "end";
            //                            poseInfo.GrabNo = 0;
            //                            poseInfo.X = x;
            //                            poseInfo.Y = y;
            //                            poseInfo.Z = z;
            //                            poseInfo.Theta = theta;
            //                            poseInfo.DeviceName = CoordSysConfigParamManger.Instance.GetCardName(item.CoordSysName.ToString());
            //                            this.TriggerInfo.Invoke(item, poseInfo);
            //                        }
            //                        break;

            //                    case "99": // 切换配方
            //                    case "Recipe":
            //                    case "recipe":
            //                        LoggerHelper.Error("收到配方切换信号：" + item.CoordSysName + "-" + item.Address);
            //                        RecipeInfo?.Invoke(new RecipeEventArgs(item.CoordSysName, FunctionNo));
            //                        break;
            //                }
            //            }
            //            Thread.Sleep(10);
            //        }
            //    });
            //}
            #endregion
            ///////////////////////////////
            if (StartRunInfo != null)
            {
                StartRunInfo.Invoke(this, new EventArgs());
            }
        }

        private void RunThread(int index = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            CommunicationConfigParam item = CommunicationParamTrigger[index]; // 单独
            while (true)
            {
                if (this.cts.IsCancellationRequested) break;
                object triggerValue = CommunicationConfigParamManger.Instance.ReadValue(item);
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    if (CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.OnLine, 1)) // 在线
                        LoggerHelper.Info("视觉在线信号");
                    stopwatch.Restart();
                }
                if (triggerValue != null && triggerValue.ToString().Trim() == "1")
                {
                    AcqSource acqSource = AcqSourceManage.Instance.GetAcqSource(item.CoordSysName);
                    LoggerHelper.Info("视觉收到PLC触发信号：" + item.CoordSysName + "_" + item.Address, acqSource?.Sensor?.Name);
                    string FunctionNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.FunctionNo).ToString();
                    string GrabNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.GrabNo).ToString();
                    CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.FunctionNoToPlc, FunctionNo); // 初始化值
                    CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.GrabNoToPlc, GrabNo); // 初始化值
                    LoggerHelper.Info($"视觉收到PLC功能值:{FunctionNo}", acqSource?.Sensor?.Name);
                    LoggerHelper.Info($"视觉收到PLC标签值:{GrabNo}", acqSource?.Sensor?.Name);
                    ////////////////////////////////////////
                    switch (FunctionNo)
                    {
                        case "grab":
                        case "Grab":
                        case "5":
                        case "11":
                        case "22":
                        case "60":
                        case "GrabNo":
                        case "grabNo":
                        default:
                            string info = "";
                            TreeNode node = this.GetExecuteNode(item.CoordSysName, acqSource?.Sensor?.Name, out info);
                            #region 测试代码
                            //if (node == null)
                            //{
                            //    LoggerHelper.Debug($"程序面板流程单元数量:{this.listTreeNode?.Count}", acqSource?.Sensor?.Name);
                            //    foreach (var subNode in this.listTreeNode)
                            //    {
                            //        LoggerHelper.Debug($"寻找流程单元!", acqSource?.Sensor?.Name);
                            //        BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)subNode.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
                            //        if (plcInfo == null)
                            //        {
                            //            LoggerHelper.Debug($"节点:{subNode.Text} ResultInfo 对象为空", acqSource?.Sensor?.Name);
                            //            //continue;
                            //        }
                            //        else
                            //        {
                            //            if (plcInfo != null && plcInfo.Count > 0)
                            //            {
                            //                string value = CommunicationConfigParamManger.Instance.ReadValue(plcInfo[0].CoordSysName, plcInfo[0].CommunicationCommand)?.ToString();
                            //                LoggerHelper.Debug($"plcInfo长度:{plcInfo.Count}", acqSource?.Sensor?.Name);
                            //                LoggerHelper.Debug($"读取触发信号地址, 当前值:{value.Trim()}, 目标值:{plcInfo[0].TargetValue.Trim()}, 坐标系名称:{item.CoordSysName}", acqSource?.Sensor?.Name);
                            //                if (value == null)
                            //                {
                            //                    LoggerHelper.Debug($"读取触发信号地址值为空", acqSource?.Sensor?.Name);
                            //                    break;
                            //                }
                            //                plcInfo[0].ReadValue = value;
                            //                ///////////////////////////////////////////////
                            //                if (plcInfo[0].IsCompare)
                            //                {
                            //                    if (plcInfo[0].TargetValue.Trim() == value.Trim() && plcInfo[0].CoordSysName == item.CoordSysName)
                            //                    {
                            //                        node = subNode;
                            //                        LoggerHelper.Debug($"方法二中获取节点:{subNode.Text}", acqSource?.Sensor?.Name);
                            //                        break;
                            //                    }
                            //                    LoggerHelper.Debug($"当前值与目标值不相等!", acqSource?.Sensor?.Name);
                            //                }
                            //                else
                            //                    LoggerHelper.Debug($"没有启用地址值比较!", acqSource?.Sensor?.Name);
                            //            }
                            //            else
                            //            {
                            //                LoggerHelper.Debug($"节点:{subNode.Text}:PlcCommunicateInfo长度为 0", acqSource?.Sensor?.Name);
                            //            }
                            //        }
                            //    }
                            //    //////////////////////////////////////////////////////////////////
                            //    if (node == null && this.listTreeNode.Count > 0)
                            //    {
                            //        node = this.listTreeNode[0];//
                            //        LoggerHelper.Info("根据坐标系获取节点为空,手动赋值列表中得第一个节点", acqSource?.Sensor?.Name);
                            //    }
                            //    else
                            //        LoggerHelper.Info($"第二次获取节点成功,节点名:{node?.Text}", acqSource?.Sensor?.Name);
                            //}
                            #endregion
                            Thread.Sleep(200);
                            CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                            IMotionControl motion = MotionCardManage.GetCard(item.CoordSysName);
                            switch (motion.ConnectConfigParam.DeviceModel)
                            {
                                case enDeviceModel.SocketClient:
                                case enDeviceModel.SocketServer:
                                case enDeviceModel.ModbusTcpClient:
                                case enDeviceModel.OmronFinsTcpClient:
                                    CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToSocket, "init");   // 初始化结果状态为OK，0:表示在开始时，初始化结果地址中的值
                                    break;
                                default:
                                    CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToPlc, "init");   //初始化结果状态为OK， 0:表示在开始时，初始化结果地址中的值
                                    break;
                            }
                            SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.NONE; // 复位中断信号 
                            string grabNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.GrabNo).ToString();
                            string productID = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.ProductID).ToString();
                            string detectEdge = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.DetectEdge).ToString();
                            ////// 这里用一个线程来执行，这样才会实时监控下一次的触发信号 
                            //Task.Run(() =>
                            //{
                            if (node != null)
                            {
                                LoggerHelper.Info("执行节点: " + node.Text, acqSource?.Sensor?.Name);
                                OperateResult state = ((IFunction)node.Tag)?.Execute(node, $"GrabNo={grabNo}", $"DetectEdge={detectEdge}", $"ProductID={productID}");
                                if (!state.Succss) // 如果有执行失败的节点，则写入 2 表示NG
                                {
                                    switch (motion.ConnectConfigParam.DeviceModel)
                                    {
                                        case enDeviceModel.SocketClient:
                                        case enDeviceModel.SocketServer:
                                        case enDeviceModel.ModbusTcpClient:
                                        case enDeviceModel.OmronFinsTcpClient:
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                                            break;
                                        default:
                                            CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                            break;
                                    }
                                    CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.FunctionNoToPlc, FunctionNo); // Grab
                                    CommunicationConfigParamManger.Instance.WriteValue(item.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // trig
                                    LoggerHelper.Error(state.ErrorMessage + item.CoordSysName + "_" + item.Address + "_" + info, acqSource?.Sensor?.Name);
                                    LoggerHelper.Info($"执行节点：{node?.Text}->失败", acqSource?.Sensor?.Name);
                                    LoggerHelper.Info($"视觉处理完成,触发信号源：{item.CoordSysName.ToString()}_{item.Address}", acqSource?.Sensor?.Name);
                                }
                                else
                                {
                                    LoggerHelper.Info($"执行节点：{node?.Text}->成功", acqSource?.Sensor?.Name);
                                    LoggerHelper.Info($"视觉处理完成,触发信号源：{item.CoordSysName.ToString()}_{item.Address}", acqSource?.Sensor?.Name);
                                }
                            }
                            else
                            {
                                LoggerHelper.Info("没有绑定相应的信号执行节点,触发信号未绑定相应节点：" + item.Address + "_" + info, acqSource?.Sensor?.Name);
                                ////////////////////////////////////////////////
                                if (this.TriggerInfo != null)
                                {
                                    double x = 0, y = 0, z = 0, theta = 0;
                                    PoseInfoEventArgs poseInfo = new PoseInfoEventArgs(item.CoordSysName);
                                    this.TriggerInfo.Invoke(item, poseInfo);
                                    LoggerHelper.Info("触发信号发送到相应窗口完成：" + item.CoordSysName + "_" + item.Address, acqSource?.Sensor?.Name);
                                }
                                else
                                    LoggerHelper.Error("窗口没有绑定触发信号：" + item.CoordSysName + "_" + item.Address, acqSource?.Sensor?.Name);
                            }
                            //});
                            break;

                        case "Reset":
                        case "reset":
                        case "66": // 表示信号异常中断 
                            SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.PLC复位中断;
                            CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                            break;

                        case "Run":
                        case "Stop":
                        case "run":
                        case "stop":
                        case "Save":
                        case "save":
                        case "SaveAs":
                        case "saveas":
                            CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                            LoggerHelper.Error("收到运行、停止指令信号：" + item.CoordSysName + "-" + item.Address);
                            RecipeInfo?.Invoke(new RecipeEventArgs(item.CoordSysName, FunctionNo));
                            break;

                        case "88": // 88 表示运动结束信号
                        case "End":
                            if (this.TriggerInfo != null)
                            {
                                double x = 0, y = 0, z = 0, theta = 0;
                                IMotionControl _card = MotionCardManage.GetCard(item.CoordSysName); // ByCoordSysName.ToString()
                                grabNo = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.GrabNo).ToString();
                                productID = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.ProductID).ToString();
                                detectEdge = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, enCommunicationCommand.DetectEdge).ToString();
                                _card?.GetAxisPosition(item.CoordSysName, enAxisName.X轴, out x);
                                _card?.GetAxisPosition(item.CoordSysName, enAxisName.Y轴, out y);
                                _card?.GetAxisPosition(item.CoordSysName, enAxisName.Z轴, out z);
                                _card?.GetAxisPosition(item.CoordSysName, enAxisName.Theta轴, out theta);
                                PoseInfoEventArgs poseInfo = new PoseInfoEventArgs();
                                poseInfo.CoordSysName = item.CoordSysName;
                                poseInfo.FunctionNo = FunctionNo;
                                poseInfo.ProductID = productID;
                                poseInfo.DetectEdge = detectEdge;
                                poseInfo.PoseInfo = "end";
                                poseInfo.GrabNo = 0;
                                poseInfo.X = x;
                                poseInfo.Y = y;
                                poseInfo.Z = z;
                                poseInfo.Theta = theta;
                                poseInfo.DeviceName = CoordSysConfigParamManger.Instance.GetCardName(item.CoordSysName.ToString());
                                this.TriggerInfo.Invoke(item, poseInfo);
                            }
                            break;

                        case "99": // 切换配方
                        case "Recipe":
                        case "recipe":
                            CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                            LoggerHelper.Error("收到配方切换信号：" + item.CoordSysName + "-" + item.Address);
                            RecipeInfo?.Invoke(new RecipeEventArgs(item.CoordSysName, FunctionNo));
                            break;

                        case "Login":
                        case "login":
                            CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                            LoggerHelper.Error("收到用户登录信号：" + item.CoordSysName + "-" + item.Address);
                            RecipeInfo?.Invoke(new RecipeEventArgs(item.CoordSysName, FunctionNo));
                            break;

                        case "Lable":
                        case "lable":
                            CommunicationConfigParamManger.Instance.WriteValue(item, 0); // 清零触发信号   
                            LoggerHelper.Error("收到获取标签信号：" + item.CoordSysName + "-" + item.Address);
                            RecipeInfo?.Invoke(new RecipeEventArgs(item.CoordSysName, FunctionNo));
                            break;
                    }
                }
                Thread.Sleep(100);
            }
        }


        /// <summary>
        /// 获取绑定了指定坐标系的节点
        /// </summary>
        /// <param name="coordSysName"></param>
        /// <param name="Info"></param>
        /// <returns></returns>
        public TreeNode GetExecuteNode(enCoordSysName coordSysName, string cameName, out string Info)
        {
            TreeNode node = null;
            Info = "";
            bool IsOk = true;
            /////// 获取程序节点 /////////////
            LoggerHelper.Debug($"程序面板流程单元数量:{listTreeNode?.Count}", cameName);
            foreach (var item in listTreeNode)
            {
                IsOk = true;
                object oo = ((BaseFunction)item.Tag).ResultInfo;
                if (oo == null)
                {
                    LoggerHelper.Debug($"节点:{item?.Text} ResultInfo 对象为空", cameName);
                    continue;
                }
                switch (oo?.GetType().Name)
                {
                    case "BindingList`1":
                        switch (oo?.GetType().GetGenericArguments()[0].Name)
                        {
                            case nameof(PlcCommunicateInfo):
                                BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)item.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
                                if (plcInfo.Count > 0)
                                {
                                    foreach (var item2 in plcInfo)
                                    {
                                        if (item2.CoordSysName != coordSysName)
                                        {
                                            IsOk = false;
                                            continue; // 如果坐标系不相等，继续
                                        }
                                        string value = CommunicationConfigParamManger.Instance.ReadValue(item2.CoordSysName, item2.CommunicationCommand)?.ToString();
                                        LoggerHelper.Debug($"PlcCommunicateInfo 长度:{plcInfo.Count}", cameName);
                                        LoggerHelper.Debug($"读取触发信号地址, 当前值:{value.Trim()}, 目标值:{plcInfo[0].TargetValue.Trim()}, 坐标系名称:{item2.CoordSysName}", cameName);
                                        if (value == null)
                                        {
                                            LoggerHelper.Debug($"读取触发信号地址值为空", cameName);
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
                                            {
                                                IsOk = IsOk && true;
                                                LoggerHelper.Debug($"获取执行节点成功,节点名:{item.Text}", cameName);
                                            }
                                            else
                                            {
                                                IsOk = IsOk && false;
                                                LoggerHelper.Error($"当前值与目标值不相等,获取执行节点失败!", cameName);
                                            }
                                        }
                                        else
                                            LoggerHelper.Debug("没有启用地址值比较", cameName);
                                    }
                                }
                                else
                                {
                                    LoggerHelper.Debug($"｛{node.Text}:PlcCommunicateInfo 长度为 0", cameName);
                                    IsOk = false;
                                }
                                break;
                            default:
                                IsOk = false;
                                break;
                        }
                        break;
                    default:
                        LoggerHelper.Debug($"ResultInfo 类型名为:{oo?.GetType().Name}", cameName);
                        IsOk = false;
                        break;
                }
                if (IsOk)
                {
                    node = item;
                    LoggerHelper.Debug($"输出流程节点:{node.Text}", cameName);
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
