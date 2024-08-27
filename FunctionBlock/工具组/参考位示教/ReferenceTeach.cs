using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("Tool")] // 表示这个类是工具类
    public class ReferenceTeach : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _ParentNode;
        public TreeNode ParentNode { get { return _ParentNode; } set { this._ParentNode = value; } }

        public bool EnableEdite { get; set; }


        public ReferenceTeach()
        {

        }


        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            try
            {
                if (!EnableEdite) return this.Result;
                // 参考位示教不需要执行
                TreeView treeView = null;
                foreach (var item in param)
                {
                    if (item is TreeNode)
                    {
                        treeView = ((TreeNode)item).TreeView;
                        this.ParentNode = ((TreeNode)item);
                        break;
                    }
                }
                if (treeView == null)
                {
                    LoggerHelper.Error(this.name + "->执行失败" + "treeView视图为空");
                    return this.Result;
                }
                //////////////////////////////////////////
                bool IsOk = true;
                if (this.ParentNode != null)
                {
                    foreach (TreeNode item in ParentNode.Nodes)
                    {
                        if (treeView != null)
                            treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                        Result = ((IFunction)item.Tag).Execute(item);
                        if (!this.Result.Succss)
                        {
                            IsOk = false;
                            continue; // 保证执行到最后
                        }
                    }
                }
                this.Result.Succss = IsOk;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    return this.name;

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
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            // throw new NotImplementedException();
        }

    }

}
