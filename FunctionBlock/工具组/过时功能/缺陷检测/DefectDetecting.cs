using Common;
using HalconDotNet;
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
    public class DefectDetecting : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _ParentNode;
        public TreeNode ParentNode { get { return _ParentNode; } set { this._ParentNode = value; } }


        public DefectDetecting()
        {

        }


        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                //////////////程序执行
                ImageDataClass _imageData = null;
                TreeView treeView = null;
                foreach (var item in param)
                {
                    if (item != null)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(TreeNode):
                                treeView = ((TreeNode)item).TreeView; // 获取树控件
                                this.ParentNode = ((TreeNode)item);
                                break;
                            case nameof(ImageDataClass):
                                _imageData = item as ImageDataClass;
                                break;
                            case nameof(HImage):
                                _imageData = new ImageDataClass((HImage)item);
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
                        this.Result = ((IFunction)item.Tag).Execute(item, _imageData);
                        if (!this.Result.Succss)
                        {
                            this.Result.ErrorMessage += this.Name + "." + item.Text + "->" + "执行失败";
                            LoggerHelper.Error(this.Name + "." + item.Text + "->" + "执行失败");
                            IsOk = false;
                            //continue; // 保证执行到最后，才能停止传感器采集
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
                case "注册事件":

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
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->读取形状模型失败" + ex.ToString());
            }
        }
        public void Save(string path)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->读取形状模型失败" + ex.ToString());
            }
        }

    }
}
