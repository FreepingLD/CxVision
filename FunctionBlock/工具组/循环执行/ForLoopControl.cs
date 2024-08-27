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
    public class ForLoopControl : BaseFunction, IFunction
    {
        [NonSerialized]
        public TreeNode _ParentNode;
        public TreeNode ParentNode { get { return _ParentNode; } set { _ParentNode = value; } }

        private string RunState = "";
        public int MaxCount
        {
            get;
            set;
        }

        public ForLoopControl()
        {
            this.MaxCount = 1;
            this.ParentNode = null;
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0)); 
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "NG对象索引", 0));
        }

        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            try
            {
                bool IsOk = true;
                TreeView treeView = null;
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item != null)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(TreeNode):
                                    treeView = ((TreeNode)item).TreeView;
                                    this.ParentNode = ((TreeNode)item);
                                    break;
                                case nameof(String):

                                    break;
                            }
                        }
                    }
                }
                ///////////////////////////////////////////////
                if (treeView == null)
                {
                    LoggerHelper.Error(this.name + "->执行失败" + "treeView视图为空");
                    return this.Result;
                }
                ///////////////////////////////////////////////
                int NgCount = 0;
                if (this.ParentNode != null)
                {
                    for (int i = 0; i < this.MaxCount; i++)
                    {
                        if (SystemParamManager.Instance.SysConfigParam.InterruptSingle == enInterruptType.PLC复位中断)
                        {
                            SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.NONE;
                            break;
                        }
                        foreach (TreeNode item in ParentNode.Nodes)
                        {
                            if (treeView != null)
                                treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                            this.Result = ((IFunction)item.Tag).Execute(item, i + 1);
                            if (this.Result.LableResult == "NG")
                                NgCount++;
                            /////////////////////////////////////////////////////////////////////
                            if (!this.Result.Succss)
                            {
                                this.Result.ErrorMessage += this.Name + "." + item.Text + "->" + "第" + (i + 1).ToString() + "次执行失败";
                                IsOk = false;
                            }
                            if (this.Result.ExcuteState == enExcuteState.中断) break;
                            if (this.Result.ExcuteState == enExcuteState.继续) continue;
                        }
                    }
                }
                LoggerHelper.Error(this.Result.ErrorMessage);
                this.Result.Succss = IsOk;
                stopwatch.Stop();
                //((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                if (((BindingList<MeasureResultInfo>)this.ResultInfo) != null && ((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 0)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0));
                //////////////////////////////////////
                if (treeView != null)
                    treeView.Invoke(new Action(() => this.ParentNode.Collapse()));
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
                default:
                case "名称":
                case nameof(this.name):
                    return this.name;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.name):
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

        }
        public void Save(string path)
        {

        }


    }
}
