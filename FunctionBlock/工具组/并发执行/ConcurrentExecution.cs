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
    public class ConcurrentExecution : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _ParentNode;
        public TreeNode ParentNode { get { return _ParentNode; } set { this._ParentNode = value; } }

        public BindingList<PlcCommunicateInfo> PlcInfo = new BindingList<PlcCommunicateInfo>();
        public ConcurrentExecution()
        {
            this.ParentNode = new TreeNode();
            this.PlcInfo = new BindingList<PlcCommunicateInfo>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }

        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Result.Succss = false;
            try
            {
                /////////////////////////// 并发执行程序
                List<Task<OperateResult>> taskList = new List<Task<OperateResult>>();
                TreeView treeView = null;
                if (treeView == null)
                {
                    foreach (var item in param)
                    {
                        if (item is TreeNode)
                        {
                            treeView = ((TreeNode)item).TreeView;
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
                ///////////////////////////
                bool isok = true;
                bool readInfo = true;
                if (this.ParentNode != null)
                {                    // 判断PLC读取信息是否成立
                    if (this.PlcInfo != null)
                    {
                        foreach (var item in this.PlcInfo)
                        {
                            if (!item.IsCompare) continue;
                            item.ReadValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand).ToString();
                            if (item.ReadValue.Trim() != item.TargetValue.Trim()) readInfo = false;
                        }
                    }
                    ////////////////////
                    if (readInfo)
                    {
                        foreach (TreeNode item in ParentNode.Nodes)
                        {
                            if (treeView != null)
                                treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                            taskList.Add(Task.Run(() => ((IFunction)item.Tag).Execute(item)));
                        }
                        /////////////////////////////
                        Task.WaitAll(taskList.ToArray()); // 等待所有任务执行完
                        foreach (Task<OperateResult> item in taskList)
                        {
                            isok = isok && item.Result.Succss;
                        }
                    }
                }
                this.Result.Succss = isok;
                if (this.ParentNode != null)
                    treeView?.Invoke(new Action(() => this.ParentNode.Collapse()));
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
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
                    return this.name;
                    //default:
                    //    return this.FeaturePoint;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
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
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }


    }
}
