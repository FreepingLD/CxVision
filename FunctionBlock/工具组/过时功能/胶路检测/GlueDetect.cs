﻿using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("Tool")] // 表示这个类是工具类
    public class GlueDetect : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _ParentNode;
        public TreeNode ParentNode { get { return _ParentNode; } set { this._ParentNode = value; } }
        public GlueDetect()
        {
            //this.ResultInfo = new BindingList<PlcResultInfo>();
        }

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                //////////////程序执行
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
                    foreach (TreeNode item in ParentNode.Nodes)
                    {
                        if (treeView != null)
                            treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                        this.Result = ((IFunction)item.Tag).Execute(item);
                        //this.Result = (GlobalProgram.ProgramItems[item.Name.Substring(0, item.Name.LastIndexOf('.'))]).Execute(item);
                        if (!this.Result.Succss)
                        {
                            this.Result.ErrorMessage = this.Name + "." + item.Text + "->" + "执行失败";
                            IsOk = false;
                            break; // 保证执行到最后
                        }
                    }
                }
                this.Result.Succss = IsOk;
                if (this.ParentNode != null)
                    treeView?.Invoke(new Action(() => this.ParentNode.Collapse()));
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
