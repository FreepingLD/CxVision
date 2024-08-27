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
    public class Conditional : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _ParentNode;
        public TreeNode ParentNode { get { return _ParentNode; } set { this._ParentNode = value; } }

        [DisplayName("判断结果")]
        [DescriptionAttribute("输出属性")]
        public string JudgmentResult { get; set; }
        private BindingList<JudgeCommand> _dataList;
        public BindingList<JudgeCommand> DataList { get => _dataList; set => _dataList = value; }


        public Conditional()
        {
            this._dataList = new BindingList<JudgeCommand>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "结果", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0));
        }

        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            try
            {
                //////////////  程序执行  //////////////////////////////
                TreeView treeView = null;
                foreach (var item in param)
                {
                    if (item != null)
                    {
                        if (item is TreeNode)
                        {
                            treeView = ((TreeNode)item).TreeView; // 获取树控件
                            this.ParentNode = ((TreeNode)item);
                            break;
                        }
                    }
                }
                if (treeView == null)
                {
                    LoggerHelper.Error(this.name + "->执行失败" + "treeView视图为空");
                    return this.Result;
                }
                ///////////////////////////////////////////////
                bool IsOk = true;
                if (this.ParentNode != null)
                {
                    BindingList<JudgeCommand> sendDataList = ((BindingList<JudgeCommand>)this.DataList);
                    foreach (var item in sendDataList)
                    {
                        if (item.IsActive) continue;
                        string value = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值).ToString();
                        switch (item.OperateSign)
                        {
                            case enOperateSign.等于:
                                if (value.Trim() == item.TargetValue.Trim())
                                    item.Result = "true";
                                else
                                    item.Result = "false";
                                break;
                            case enOperateSign.不等于:
                                if (value.Trim() != item.TargetValue.Trim())
                                    item.Result = "true";
                                else
                                    item.Result = "false";
                                break;
                            case enOperateSign.大于:
                                double targetVale = 0, curValue = 0;
                                double.TryParse(value.Trim(), out curValue);
                                double.TryParse(item.TargetValue.Trim(), out targetVale);
                                if (curValue > targetVale)
                                    item.Result = "true";
                                else
                                    item.Result = "false";
                                break;
                            case enOperateSign.小于:
                                targetVale = 0;
                                curValue = 0;
                                double.TryParse(value.Trim(), out curValue);
                                double.TryParse(item.TargetValue.Trim(), out targetVale);
                                if (curValue < targetVale)
                                    item.Result = "true";
                                else
                                    item.Result = "false";
                                break;
                        }
                    }
                    ///////////////////////////////////
                    foreach (var item in sendDataList)
                    {
                        if (item.Result == "false")
                            this.JudgmentResult = "NG";
                    }
                    switch (this.JudgmentResult)
                    {
                        case "OK":
                            foreach (TreeNode item in this.ParentNode.Nodes)
                            {
                                if(item.Tag != null && item.Tag is ConditionaIf)
                                {
                                    if (treeView != null)
                                        treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                                    this.Result = ((IFunction)item.Tag).Execute(item);
                                    if (!this.Result.Succss)
                                    {
                                        this.Result.ErrorMessage += this.Name + "." + item.Text + "->" + "执行失败";
                                        IsOk = false;
                                    }
                                }
                            }
                            break;
                        case "NG":
                            foreach (TreeNode item in this.ParentNode.Nodes)
                            {
                                if (item.Tag != null && item.Tag is ConditionaElse)
                                {
                                    if (treeView != null)
                                        treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                                    this.Result = ((IFunction)item.Tag).Execute(item);
                                    if (!this.Result.Succss)
                                    {
                                        this.Result.ErrorMessage += this.Name + "." + item.Text + "->" + "执行失败";
                                        IsOk = false;
                                    }
                                }
                            }
                            break;
                    }
                }
                this.Result.Succss = IsOk;
                if (this.ParentNode != null)
                    treeView?.Invoke(new Action(() => this.ParentNode.Collapse()));
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
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
