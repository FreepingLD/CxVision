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
    public class MeasureArray : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _ParentNode;
        private userPixCoordSystem[] _pixCoordSystem = null;
        public TreeNode ParentNode { get { return _ParentNode; } set { this._ParentNode = value; } }

        public ArrayParam Param { get; set; }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性1")]
        public userPixCoordSystem[] PixCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        List<userPixCoordSystem> listPix = new List<userPixCoordSystem>();
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        foreach (var item in oo)
                        {
                            if (item != null)
                            {
                                switch (item.GetType().Name)
                                {
                                    case "userPixCoordSystem[]":
                                        this._pixCoordSystem = item as userPixCoordSystem[];
                                        break;
                                    case nameof(userPixCoordSystem):
                                        listPix.Add(((userPixCoordSystem)item));
                                        break;
                                    case nameof(userWcsCoordSystem):
                                        listPix.Add(((userWcsCoordSystem)item).GetPixCoordSystem());
                                        break;
                                    case "userWcsCoordSystem[]":
                                        userWcsCoordSystem[] userWcsCoords = item as userWcsCoordSystem[];
                                        foreach (var item1 in userWcsCoords)
                                        {
                                            listPix.Add((item1).GetPixCoordSystem());
                                        }
                                        break;
                                }
                            }
                        }
                        this._pixCoordSystem = listPix.ToArray();
                        listPix.Clear();
                    }
                    else
                        this._pixCoordSystem = null;
                    return _pixCoordSystem;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._pixCoordSystem = value;
            }
        }


        public MeasureArray()
        {
            this.Param = new ArrayParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time", 0));
        }

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.ErrorMessage = "";
            this.Result.ExcuteState = enExcuteState.NONE;
            this.Result.LableResult = "";
            this.Result.DataContent = "";
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //////////////程序执行
                TreeView treeView = null;
                foreach (var item in param)
                {
                    if (item is TreeNode)
                    {
                        treeView = ((TreeNode)item).TreeView; // 获取树控件
                        this.ParentNode = ((TreeNode)item);
                        break;
                    }
                }
                if (treeView == null)
                {
                    LoggerHelper.Error(this.name + "->执行失败" + "treeView视图为空");
                    return this.Result;
                }
                ///////////////////////////////////////////////
                bool IsOk = true;
                userPixCoordSystem pixCoordSystem = new userPixCoordSystem();
                for (int ii = 0; ii < this.Param.RowCount; ii++)
                {
                    for (int j = 0; j < this.Param.ColCount; j++)
                    {
                        for (int i = 0; i < ParentNode.Nodes.Count; i++)
                        {
                            treeView.Invoke(new Action(() => treeView.SelectedNode = ParentNode.Nodes[i]));
                            if (ParentNode.Nodes[i].Tag != null)
                            {
                                switch (ParentNode.Nodes[i].Tag.GetType().Name)
                                {
                                    case nameof(ShapeModelMatch2D):
                                    case nameof(NccModelMatch):
                                        this.Result = ((IFunction)ParentNode.Nodes[i].Tag).Execute(ParentNode.Nodes[i]);
                                        userPixCoordSystem[] userPixCoords = ((IFunction)ParentNode.Nodes[i].Tag).GetPropertyValues("userPixCoordSystem[]") as userPixCoordSystem[];
                                        if (userPixCoords != null)
                                        {
                                            int sysNum = 0;
                                            foreach (var item in userPixCoords)        // 多个匹配坐标系来定位测量，同时也要在同一个坐标系下可以阵列,在同一个坐标系中时，希望是延着轴线阵列
                                            {
                                                pixCoordSystem.ReferencePoint = item.ReferencePoint;
                                                pixCoordSystem.CurrentPoint.Row = item.CurrentPoint.Row;
                                                pixCoordSystem.CurrentPoint.Col = item.CurrentPoint.Col;
                                                pixCoordSystem.CurrentPoint.Rad = item.CurrentPoint.Rad;
                                                for (int k = i + 1; k < ParentNode.Nodes.Count; k++)
                                                {
                                                    if (treeView != null)
                                                        treeView.Invoke(new Action(() => treeView.SelectedNode = ParentNode.Nodes[k]));
                                                    if ((IFunction)ParentNode.Nodes[k].Tag != null)
                                                        this.Result = ((IFunction)ParentNode.Nodes[k].Tag).Execute(ParentNode.Nodes[k], pixCoordSystem, "Index=" + sysNum + "_" + k);
                                                    if (!this.Result.Succss)
                                                    {
                                                        this.Result.ErrorMessage += this.Name + "." + ParentNode.Nodes[k].Text + "->" + "第 " + sysNum.ToString() + "执行失败;";
                                                        IsOk = false;
                                                    }
                                                    if (this.Result.ExcuteState == enExcuteState.中断) break;
                                                    if (this.Result.ExcuteState == enExcuteState.继续) continue;
                                                }
                                                sysNum++;
                                            }
                                        }
                                        /////////////////////////
                                        i = ParentNode.Nodes.Count;
                                        this.Param.RowCount = 1;
                                        this.Param.ColCount = 1;
                                        break;
                                    default:
                                        pixCoordSystem.ReferencePoint = new userPixVector(0, 0, 0);
                                        pixCoordSystem.CurrentPoint = new userPixVector(this.Param.RowOffset * ii, this.Param.ColOffset * j, 0);
                                        this.Result = ((IFunction)ParentNode.Nodes[i].Tag).Execute(ParentNode.Nodes[i], pixCoordSystem, "Index=" + (ii * this.Param.ColCount + j));
                                        if (!this.Result.Succss)
                                        {
                                            this.Result.ErrorMessage += this.Name + "." + ParentNode.Nodes[i].Text + "->" + "第 " + this.Param.RowCount.ToString() + "行" + this.Param.ColCount.ToString() + "执行失败;";
                                            IsOk = false;
                                        }
                                        if (this.Result.ExcuteState == enExcuteState.中断) break;
                                        if (this.Result.ExcuteState == enExcuteState.继续) continue;
                                        break;
                                }
                            }
                            else
                            {
                                IsOk = false;
                                this.Result.ErrorMessage += this.Name + "." + ParentNode.Nodes[i].Text + "执行失败;";
                            }
                        }
                    }
                }
                LoggerHelper.Error(this.Result.ErrorMessage);
                stopwatch.Stop();
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 0)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time", stopwatch.ElapsedMilliseconds);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time", 0));
                this.Result.Succss = IsOk;
                if (this.ParentNode != null)
                    treeView?.Invoke(new Action(() => this.ParentNode.Collapse()));
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                //bool IsOk = true;
                //userPixCoordSystem pixCoordSystem = new userPixCoordSystem();
                //if (this.ParentNode != null)
                //{
                //    if (this.PixCoordSystem == null)
                //    {
                //        for (int i = 0; i < this.Param.RowCount; i++)
                //        {
                //            for (int j = 0; j < this.Param.ColCount; j++)
                //            {
                //                pixCoordSystem.ReferencePoint = new userPixVector(0, 0, 0);
                //                pixCoordSystem.CurrentPoint = new userPixVector(this.Param.RowOffset * i, this.Param.ColOffset * j, 0);
                //                foreach (TreeNode item in ParentNode.Nodes)
                //                {
                //                    if (treeView != null)
                //                        treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                //                    if (item.Tag != null)
                //                        this.Result = ((IFunction)item.Tag).Execute(item, pixCoordSystem, "Index=" + (i * this.Param.ColCount + j));
                //                    if (!this.Result.Succss)
                //                    {
                //                        this.Result.ErrorMessage += this.Name + "." + item.Text + "->" + "第 " + this.Param.RowCount.ToString() + "行" + this.Param.ColCount.ToString() + "列执行失败";
                //                        IsOk = false;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        int sysNum = 0;
                //        foreach (var item in this.PixCoordSystem)        // 多个匹配坐标系来定位测量，同时也要在同一个坐标系下可以阵列,在同一个坐标系中时，希望是延着轴线阵列
                //        {
                //            for (int i = 0; i < this.Param.RowCount; i++)
                //            {
                //                for (int j = 0; j < this.Param.ColCount; j++)
                //                {
                //                    pixCoordSystem.ReferencePoint = item.ReferencePoint;
                //                    pixCoordSystem.CurrentPoint.Row = item.CurrentPoint.Row;// + dist * Math.Sin(item.CurrentPoint.Rad*-1);
                //                    pixCoordSystem.CurrentPoint.Col = item.CurrentPoint.Col;// + dist * Math.Cos(item.CurrentPoint.Rad * -1);
                //                    pixCoordSystem.CurrentPoint.Rad = item.CurrentPoint.Rad;
                //                    foreach (TreeNode node in ParentNode.Nodes)
                //                    {
                //                        if (treeView != null)
                //                            treeView.Invoke(new Action(() => treeView.SelectedNode = node));
                //                        if (node.Text != "<-坐标系")
                //                        {
                //                            if ((IFunction)node.Tag != null)
                //                                this.Result = ((IFunction)node.Tag).Execute(node, pixCoordSystem, "Index=" + sysNum + "_" + (i * this.Param.ColCount + j));
                //                            else
                //                                this.Result.Succss = false;
                //                        }
                //                        else
                //                            this.Result.Succss = true;
                //                        if (!this.Result.Succss)
                //                        {
                //                            this.Result.ErrorMessage += this.Name + "." + node.Text + "->" + "第 " + sysNum.ToString() + "列执行失败";
                //                            IsOk = false;
                //                        }
                //                    }
                //                }
                //                sysNum++;
                //            }
                //        }
                //    }
                //    LoggerHelper.Error(this.Result.ErrorMessage);
                //    stopwatch.Stop();
                //    if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 0)
                //        ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time", stopwatch.ElapsedMilliseconds);
                //    else
                //        ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time", 0));
                //    this.Result.Succss = IsOk;
                //    if (this.ParentNode != null)
                //        treeView?.Invoke(new Action(() => this.ParentNode.Collapse()));
                //}

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
