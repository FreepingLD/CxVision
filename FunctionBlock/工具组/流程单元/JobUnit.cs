using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("Tool")] // 表示这个类是工具类
    public class JobUnit : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _parentNode;
        public TreeNode ParentNode { get { return _parentNode; } set { this._parentNode = value; } }

        public BindingList<MeasureResultInfo> MeasInfo;


        public JobUnit()
        {
            this.ResultInfo = new BindingList<PlcCommunicateInfo>();
            this.MeasInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<PlcCommunicateInfo>)this.ResultInfo).Add(new PlcCommunicateInfo(enCoordSysName.CoordSys_0, enCommunicationCommand.TriggerFromPlc, "1"));
            ((BindingList<MeasureResultInfo>)this.MeasInfo).Add(new MeasureResultInfo());
        }

        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            this.Result.ErrorMessage = "";
            string AutoMode = "auto";
            ImageDataClass _imageData = new ImageDataClass();
            string camName = null;
            string viewName = null;
            bool isContainLable = false; // 是否包含标签
            bool isExcuteLable = false; // 是否执行标签
            string lableText = "NONE";
            enCoordSysName coordSysName = enCoordSysName.CoordSys_0;
            try
            {
                bool IsOk = true;
                //////////////   程序执行
                TreeView treeView = null;
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item != null)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(ImageDataClass):
                                    _imageData = item as ImageDataClass;
                                    break;
                                case nameof(String):  // 流程单元作为顶层节点，将由监控线程监控
                                    AutoMode = item.ToString();
                                    break;
                                case nameof(TreeNode):
                                    treeView = ((TreeNode)item).TreeView; // 获取树控件
                                    this._parentNode = ((TreeNode)item);
                                    break;
                            }
                        }
                    }
                }
                /// 获取坐标系绑定的相机
                if (((BindingList<PlcCommunicateInfo>)this.ResultInfo).Count > 0)
                {
                    coordSysName = ((BindingList<PlcCommunicateInfo>)this.ResultInfo)[0].CoordSysName;
                    foreach (var item in AcqSourceManage.Instance.AcqSourceList)
                    {
                        if (item.CoordSysName == coordSysName)
                        {
                            camName = item.Sensor.Name;
                            viewName = item.Sensor.CameraParam.ViewWindow;
                        }
                    }
                }
                ///////////////////////////////////////////////////////
                if (treeView == null)
                {
                    LoggerHelper.Error(this.name + "->执行失败" + "treeView视图为空");
                    return this.Result;
                }
                ///////////////////////////////////////////////
                if (this._parentNode != null)
                {
                    bool isRun = true;
                    OperateResult tempResult = new OperateResult();
                    lableText = CommunicationConfigParamManger.Instance.ReadValue(coordSysName, enCommunicationCommand.GrabNo)?.ToString();
                    foreach (TreeNode item in _parentNode.Nodes)
                    {
                        if (item.Checked) continue; // 如果节点是禁用的，该属性为 true，该节点也将不再执行;
                        if (item.Tag != null)
                        {
                            switch (item.Tag.GetType().Name)
                            {
                                case nameof(UserLable):
                                    isContainLable = true;
                                    if (item.Text == lableText || $"{this._parentNode.Text}.{item.Text}" == lableText)
                                    {
                                        isRun = true;
                                        isExcuteLable = true;
                                        LoggerHelper.Info(this.name + $"->执行标签:{lableText}", camName);
                                    }
                                    else
                                        isRun = false;
                                    tempResult.Succss = true;
                                    break;
                                default:
                                    if (isRun)
                                    {
                                        LoggerHelper.Info(this.name + $"->执行节点:{item.FullPath.Replace("\\", ".")}", camName);
                                        treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                                        tempResult = ((IFunction)item.Tag).Execute(item, _imageData);
                                    }
                                    else
                                        tempResult.Succss = true;
                                    break;
                            }
                        }
                        if (!tempResult.Succss)
                        {
                            IsOk = false;
                            if (this.Result.ErrorMessage == "")
                                this.Result.ErrorMessage = this.Name + "." + item.Text + ": NG; " + tempResult.ErrorMessage;
                            else
                                this.Result.ErrorMessage += item.Text + ": NG; " + tempResult.ErrorMessage;
                            //////////////////////////
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 2); // 发送NG信号
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "NG"); // 发送NG信号
                            LoggerHelper.Error(this.name + $"->节点:{item.FullPath.Replace("\\", ".")} 执行失败,并写入结果NG", camName);

                        }
                    }
                }
                this.Result.Succss = IsOk;
                if (this._parentNode != null)
                    treeView?.Invoke(new Action(() => this._parentNode.Collapse()));
                stopwatch.Stop();
                if (this.MeasInfo == null)
                {
                    this.MeasInfo = new BindingList<MeasureResultInfo>();
                    ((BindingList<MeasureResultInfo>)this.MeasInfo).Add(new MeasureResultInfo());
                }
                ((BindingList<MeasureResultInfo>)this.MeasInfo)[0].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                ///// 记录时间
                LoggerHelper.Info(this.name + $"->执行时间:{stopwatch.ElapsedMilliseconds}(ms)", camName);
                ////////////////////////////////////
                if (this.Result.Succss)
                {
                    if (isContainLable)
                    {
                        if (isExcuteLable)
                            LoggerHelper.Info(this.name + "->执行成功", camName); // 
                        else
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 2); // 发送NG信号
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "NG"); // 发送NG信号
                            LoggerHelper.Info(this.name + $"->未找到与指定标签:{lableText} 相等的标签", camName); // 
                            LoggerHelper.Info(this.name + "->执行失败", camName); // 
                        }
                    }
                    else
                        LoggerHelper.Info(this.name + "->执行成功", camName); // 
                }
                else
                {
                    LoggerHelper.Error(this.name + $"->执行失败:{this.Result.ErrorMessage}", camName);
                    userTextLable _TextLable = new userTextLable(this.Result.ErrorMessage, "red");
                    _TextLable.LablePose = enLablePosition.左下角;
                    OnExcuteCompleted(camName, viewName, this.name, _TextLable);  //执行失败，在窗口上显示NG信息
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(this.name + "->执行错误" + ex, camName);
                this.Result.Succss = false;
            }
            finally
            {
                // 更改UI字体　
                UpdataNodeElementStyle(param, this.Result.Succss, camName, coordSysName);
                /////////////////////// 
                if (((BindingList<PlcCommunicateInfo>)this.ResultInfo).Count > 0)
                {
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                    string value = CommunicationConfigParamManger.Instance.ReadValue(coordSysName, enCommunicationCommand.TriggerToPlc).ToString();
                    LoggerHelper.Info(this.name + $"->视觉触发PLC:{coordSysName}_{enCommunicationCommand.TriggerToPlc}_写入值:{value}", camName);
                }
            }
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                    return ""; // this.FeaturePoint;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
                    return true;
                default:
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
            //throw new NotImplementedException();
        }

    }
}
