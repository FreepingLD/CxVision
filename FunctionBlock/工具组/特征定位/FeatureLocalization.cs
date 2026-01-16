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
    public class FeatureLocalization : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _parentNode;
        public TreeNode ParentNode { get { return _parentNode; } set { this._parentNode = value; } }

        public BindingList<PlcCommunicateInfo> PlcInfo = new BindingList<PlcCommunicateInfo>();


        public FeatureLocalization()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            this.PlcInfo = new BindingList<PlcCommunicateInfo>();
            //((BindingList<PlcCommunicateInfo>)this.PlcInfo).Add(new PlcCommunicateInfo(enCoordSysName.CoordSys_0, enCommunicationCommand.FunctionNo, "Grab"));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }

        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Result.Reset();
            this.Result.Succss = false;
            this.Result.ErrorMessage = "";
            this.Result.ExcuteState = enExcuteState.NONE;
            ImageDataClass _imageData = new ImageDataClass();
            bool IsOk = true;
            bool readInfo = true;
            string AutoMode = "auto";
            string camName = null;
            bool isContainLable = false; // 是否包含标签
            bool isExcuteLable = false; // 是否执行标签
            string lableText = "NONE";
            enCoordSysName coordSysName = enCoordSysName.CoordSys_0;
            try
            {
                //////////////  程序执行  //////////////////////////////
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
                                case nameof(String):
                                    AutoMode = item.ToString(); //"manual";
                                    break;
                                case nameof(TreeNode):
                                    treeView = ((TreeNode)item).TreeView; // 获取树控件
                                    this._parentNode = ((TreeNode)item);
                                    break;
                            }
                        }
                    }
                }
                /////////////////////////////////////////////
                if (treeView == null)
                {
                    LoggerHelper.Error(this.name + "->执行失败" + "treeView视图为空");
                    return this.Result;
                }
                ////////////////////////
                if (((BindingList<PlcCommunicateInfo>)this.PlcInfo).Count > 0)
                {
                    coordSysName = ((BindingList<PlcCommunicateInfo>)this.PlcInfo)[0].CoordSysName;
                    foreach (var item in AcqSourceManage.Instance.AcqSourceList)
                    {
                        if (item.CoordSysName == coordSysName)
                            camName = item.Sensor.Name;
                    }
                }
                ///////////////////////////////////////////////
                if (this._parentNode != null)
                {
                    // 判断PLC读取信息是否成立
                    if (this.PlcInfo != null && this.PlcInfo.Count > 0)
                    {
                        foreach (var item in this.PlcInfo)
                        {
                            if (!item.IsCompare) continue;
                            if (item.TargetValue == null) continue;
                            item.ReadValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand)?.ToString();
                            bool isContain = false;
                            string[] tempValue = item.TargetValue.Trim().Split(',', ';');
                            foreach (var item2 in tempValue)
                            {
                                if (item2.Trim() == item.ReadValue.Trim() || item2.Trim().ToLower() == item.ReadValue.Trim().ToLower()) isContain = true;
                            }
                            if (!isContain) readInfo = false;
                        }
                    }
                    /// 如果信息成立，那么将执行节点
                    if (readInfo || AutoMode == "manual")
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
                                        if (item.Text == lableText || $"{this._parentNode.Text}.{item.Text}" == lableText || item.FullPath.Replace("\\", ".") == lableText)
                                        {
                                            isRun = true;
                                            isExcuteLable = true;
                                            LoggerHelper.Info(this.name + $"->执行标签:{lableText}", camName);
                                        }
                                        else
                                            isRun = false;
                                        tempResult.Succss = true;
                                        break;

                                    case nameof(FeatureLocalization):
                                        treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                                        tempResult = ((IFunction)item.Tag).Execute(item, _imageData);
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
                                if (!tempResult.Succss)
                                {
                                    IsOk = false;
                                    if (this.Result.ErrorMessage == "")
                                        this.Result.ErrorMessage = this.Name + "." + item.Text + ": NG; " + tempResult.ErrorMessage;
                                    else
                                        this.Result.ErrorMessage += item.Text + ": NG; " + tempResult.ErrorMessage;
                                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 2); // 发送NG信号
                                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "NG"); // 发送NG信号
                                    LoggerHelper.Error(this.name + $"->节点:{item.FullPath.Replace("\\", ".")} 执行失败,并写入结果NG", camName);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Result.ErrorMessage = "获取到的值与指定值不相等";
                        IsOk = false;
                    }
                }
                this.Result.IsExcuteLable = isExcuteLable;
                this.Result.Succss = IsOk;
                if (this._parentNode != null)
                    treeView?.Invoke(new Action(() => this._parentNode.Collapse()));
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                /////////////////////////////////////////////////////////////
                if (this.Result.Succss)
                {
                    //if (isContainLable) // 这里不能判断标签是否执行
                    //{
                    //    if (isExcuteLable)
                    //        LoggerHelper.Info(this.name + $"->执行:{lableText} 成功", camName); // 
                    //    else
                    //    {
                    //        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 2); // 发送NG信号
                    //        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "NG"); // 发送NG信号
                    //        LoggerHelper.Error(this.name + $"->未找到与指定标签:{lableText} 相等的标签", camName); // 
                    //        LoggerHelper.Error(this.name + $"->执行标签:{lableText} 失败", camName); // 
                    //    }
                    //}
                    //else
                    LoggerHelper.Info(this.name + "->执行成功", camName); // 
                }
                else
                    LoggerHelper.Error(this.name + "->执行失败;" + this.Result.ErrorMessage, camName);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex, camName);
                this.Result.Succss = false;
            }
            finally
            {
                // 更改UI字体　
                UpdataNodeElementStyle(param, this.Result.Succss, camName, coordSysName);
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
