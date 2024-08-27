using Common;
using FunctionBlock;
using Light;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FunctionBlock
{
    /// <summary>
    /// 管理程序类
    /// </summary>
    [Serializable]
    public class TreeViewWrapClass
    {
        private Form form, acqSourceForm, parentForm;
        private TreeNode node;
        public event EditeNodeEventHandler Edite;
        private CancellationTokenSource cts;
        private TreeView treeView;
        public event EventHandler RunComplete;
        public event EventHandler StartRun;
        public event EventHandler RunError;

        public string ToolName { get; set; }

        public CancellationTokenSource Cts { get => cts; set => cts = value; }

        //public Dictionary<string, IFunction> ProgramItems
        //{
        //    get;
        //    set;
        //}
        public TreeView TreeView { get => treeView; set => treeView = value; }


        // 在这里面可以加一个点击事件 
        public static event ClickNodeEventHandler ClickNode;
        public static event AddNodeEventHandler AddNode;
        public static event DeleteNodeEventHandler DeleteNode;
        public static event EventHandler ClearNode;

        public TreeViewWrapClass()
        {
            //GlobalProgram.ProgramItems = new Dictionary<string, IFunction>();
            this.ToolName = "";
        }
        public TreeViewWrapClass(TreeView treeView, Form form)
        {
            this.ToolName = "";
            //GlobalProgram.ProgramItems = new Dictionary<string, IFunction>();
            this.parentForm = form;
            this.treeView = treeView;
            this.treeView.AllowDrop = true;
            this.treeView.ShowPlusMinus = false; // 设置节点样式
            this.treeView.ShowRootLines = false;
            this.treeView.Scrollable = true;
            addContextMenu(this.treeView);
            //////////////////订阅事件
            //this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick); // 封闭一个节点点击事件给外部调用 
            //// 因为需要将这两个关联起来，所以需要订阅他的事件，通过事件与ListBoxWrapClass类通信
            //ListBoxWrapClass.ItemsChangeToTreeView += new ItemsChangeEventHandler(this.listBoxWrapClass_ItemChange);  // 与之对应的listBox对象
            InitTreeview();
        }
        private TreeViewWrapClass(TreeView treeView)
        {
            this.ToolName = "";
            //GlobalProgram.ProgramItems = new Dictionary<string, IFunction>();
            this.parentForm = null;
            this.treeView = treeView;
            this.treeView.AllowDrop = true;
            addContextMenu(this.treeView);
            //////////////////订阅事件
            //this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick); // 封闭一个节点点击事件给外部调用 
            //// 因为需要将这两个关联起来，所以需要订阅他的事件，通过事件与ListBoxWrapClass类通信
            //ListBoxWrapClass.ItemsChangeToTreeView += new ItemsChangeEventHandler(this.listBoxWrapClass_ItemChange);  // 与之对应的listBox对象
        }

        private void InitTreeview()
        {
            this.treeView.Nodes.Clear();
            //AcqSourceManage.AcqSourceList.Clear();
            //AcqSourceManage.CamAcqSourceList.Clear();
            //AcqSourceManage.LaserAcqSourceList.Clear();
            //foreach (var item in SensorManage.SensorList)
            //{
            //    this.AddItems(new AcqSource(item), item.ConfigParam.SensorName, ""); // 窗体加载时，将所有传感器都加载出来
            //}
        }
        protected void OnClickNode(TreeNode node, MouseButtons button, int clicks, int x, int y)
        {
            var eventHandler = ClickNode;
            if (eventHandler != null)
                eventHandler(this.treeView, new TreeNodeMouseClickEventArgs(node, button, clicks, x, y));
        }
        protected void OnAddNode(TreeNode node)
        {
            var eventHandler = AddNode;
            if (eventHandler != null)
                eventHandler(this.treeView, new TreeViewEventArgs(node));
        }
        protected void OnDeleteNode(TreeNode node)
        {
            var eventHandler = DeleteNode;
            if (eventHandler != null)
                eventHandler(this.treeView, new TreeViewEventArgs(node));
        }
        protected void OnClearNode()
        {
            var eventHandler = ClearNode;
            if (eventHandler != null)
                eventHandler(this.treeView, new EventArgs());
        }

        /// <summary>
        /// 运行程序,返回值为 void 的异步方法，不会等待，将继续执行调用方法的其他代码，要使用其等，请将返回值设置为非 void
        /// </summary>
        /// <param name="token"></param>
        public async void RunAsyn(ToolStripItem item, int Count) //async
        {
            cts = new CancellationTokenSource();
            TreeNode node = null;
            IFunction _function;
            if (item != null)
                this.treeView.Invoke(new Func<object>(() => item.Enabled = false));
            this.StartRun?.Invoke(this, new EventArgs());
            try
            {
                await Task.Run(() =>
                {
                    int length = (int)(this.treeView.Invoke(new Func<object>(() => this.treeView.Nodes.Count))); //程序条目   
                    for (int jj = 0; jj < Count; jj++) // 循环次数
                    {
                        if (cts.IsCancellationRequested) break; //要跳出两次循环才行
                        DataInteractionClass.getInstance().OnClearGraphic(); // 清空图形窗口
                        for (int i = 0; i < length; i++) // 按顺序执行一次
                        {
                            if (this.treeView.Nodes[i].Checked) continue; // 如果节点是禁用的，该属性为 true，该节点也将不再执行;
                            if (cts.IsCancellationRequested) break;
                            //if (!(this.treeView.Nodes[i].Tag is IFunction)) continue;
                            this.treeView.Invoke(new Action(() => this.treeView.Focus()));
                            this.treeView.Invoke(new Action(() => this.treeView.SelectedNode = this.treeView.Nodes[i]));
                            this.treeView.Invoke(new Action(() => { node = this.treeView.Nodes[i]; })); // node.Index = Count
                            _function = this.treeView.Nodes[i].Tag as IFunction;
                            if (!_function.Execute(node, jj).Succss) break;
                        }
                        Thread.Sleep(200);
                    }
                }); // 需要把要执行的代码块放到任务中才可以
                if (LightConnectManage.CurrentLight != null)
                    LightConnectManage.CurrentLight.SetLight(new userLightParam());
                return;
            }
            catch (Exception ex)
            {
                this.RunError?.Invoke(this, new EventArgs());
                MessageBox.Show("停止运行" + ex.ToString());
                return;
            }
            finally
            {
                if (item != null)
                    this.treeView?.Invoke(new Func<object>(() => item.Enabled = true));
                this.RunComplete?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// 同步执行程序
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Count"></param>
        public void RunSyn(ToolStripItem item, int Count) //async
        {
            cts = new CancellationTokenSource();
            TreeNode node = null;
            IFunction _function;
            if (item != null)
                this.treeView.Invoke(new Func<object>(() => item.Enabled = false));
            this.StartRun?.Invoke(this, new EventArgs());
            try
            {
                int length = (int)(this.treeView.Invoke(new Func<object>(() => this.treeView.Nodes.Count))); //程序条目   
                for (int jj = 0; jj < Count; jj++) // 循环次数
                {
                    if (cts.Token.IsCancellationRequested) break; //要跳出两次循环才行
                    DataInteractionClass.getInstance().OnClearGraphic(); // 清空图形窗口
                    for (int i = 0; i < length; i++) // 按顺序执行一次
                    {
                        if (this.treeView.Nodes[i].Checked) continue; // 如果节点是禁用的，该属性为 true，该节点也将不再执行;
                        if (cts.Token.IsCancellationRequested) break;
                        if (!(this.treeView.Nodes[i].Tag is IFunction)) continue;
                        this.treeView.Invoke(new Action(() => this.treeView.Focus()));
                        this.treeView.Invoke(new Action(() => this.treeView.SelectedNode = this.treeView.Nodes[i]));
                        this.treeView.Invoke(new Action(() => { node = this.treeView.Nodes[i]; })); // node.Index = Count
                        _function = this.treeView.Nodes[i].Tag as IFunction;
                        if (!_function.Execute(node, jj).Succss) break;
                    }
                    Thread.Sleep(200);
                }
                // 测量结束，关闭光源
                if (LightConnectManage.CurrentLight != null)
                    LightConnectManage.CurrentLight.SetLight(new userLightParam());
            }
            catch (Exception ex)
            {
                this.RunError?.Invoke(this, new EventArgs());
                MessageBox.Show("停止运行" + ex.ToString());
                return;
            }
            finally
            {
                if (item != null)
                    this.treeView?.Invoke(new Func<object>(() => item.Enabled = true));
                this.RunComplete?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// 停止执行程序
        /// </summary>
        public void Stop()
        {
            if (this.cts != null && !this.cts.IsCancellationRequested)
                this.cts.Cancel();
        }

        /// <summary>
        ///  打开程序
        /// </summary>
        /// <returns></returns>
        public bool OpenProgram(string filePath)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            if (!Directory.Exists(filePath)) return false;
            string[] fileName = Directory.GetFiles(filePath, "*.dat");
            try
            {
                if (fileName.Length > 0 && File.Exists(fileName[0]))
                {
                    using (Stream fStream = File.OpenRead(fileName[0]))
                    {
                        object oo = binFormat.Deserialize(fStream);
                        LoadTreeViewNode((List<TreeNode>)oo);
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            ////////////////////  加载程序条目  ///////////////////////////
            GlobalProgram.ProgramItems.Clear();
            foreach (var item in Directory.GetFiles(filePath, "*.txt"))
            {
                object oo = null;
                IFunction function = null;
                try
                {
                    string fileName3 = new FileInfo(item).Name;
                    using (Stream fStream = File.OpenRead(item))
                    {
                        oo = binFormat.Deserialize(fStream);
                    }
                    function = (IFunction)oo;
                    function.Read(item);
                    //function.SetPropertyValues("注册事件", "null");
                    GlobalProgram.ProgramItems.Add(new FileInfo(item).Name.Split(new string[] { "-" }, StringSplitOptions.None)[0], function);
                    //this.ProgramItems.Add(new FileInfo(item).Name.Split(new string[] { ".txt" }, StringSplitOptions.None)[0], function);
                }
                catch (Exception ex)
                {
                    //如果反序列化异常，动态创建一个对象，根据节点来搜索节点
                    string name = new FileInfo(item).Name.Replace(".txt", "").Split('-').Last();
                    try
                    {
                        function = Activator.CreateInstance(Type.GetType(name)) as IFunction;
                        function.Name = new FileInfo(item).Name.Split(new string[] { "-" }, StringSplitOptions.None)[0];
                        GlobalProgram.ProgramItems.Add(function.Name, function);
                        // GlobalProgram.ProgramItems.Add(name, function);
                    }
                    catch
                    {

                    }
                }
            }
            //////// 将程序条目附加到节点的Tag属性上 ///////////////////////////
            for (int i = 0; i < this.treeView.Nodes.Count; i++)
            {
                this.SetTag(this.treeView.Nodes[i]);
            }
            ////// 更新引用源中的引用对象，因为反序列化后，引用对象已不是原来的那一个
            foreach (KeyValuePair<string, IFunction> item in GlobalProgram.ProgramItems)
            {
                this.InitRefSource(item.Value);
            }
            return true;
        }
        private void InitRefSource(IFunction function)
        {
            string[] keys;
            string subKey = "";
            if (function == null) return;
            ////// 引用1
            keys = new string[((BaseFunction)function).RefSource1.Count];
            ((BaseFunction)function).RefSource1.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                subKey = item1.Substring(0, item1.LastIndexOf('.'));
                if (GlobalProgram.ProgramItems.ContainsKey(subKey)) //item1.Substring(0, item1.LastIndexOf('.'))
                    (((BaseFunction)function).RefSource1)[item1] = GlobalProgram.ProgramItems[subKey];
            }
            ////// 引用2
            keys = new string[((BaseFunction)function).RefSource2.Count];
            ((BaseFunction)function).RefSource2.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                subKey = item1.Substring(0, item1.LastIndexOf('.'));
                if (GlobalProgram.ProgramItems.ContainsKey(subKey)) //item1.Substring(0, item1.LastIndexOf('.'))
                    (((BaseFunction)function).RefSource2)[item1] = GlobalProgram.ProgramItems[subKey];
            }
            ////// 引用3
            keys = new string[((BaseFunction)function).RefSource3.Count];
            ((BaseFunction)function).RefSource3.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                subKey = item1.Substring(0, item1.LastIndexOf('.'));
                if (GlobalProgram.ProgramItems.ContainsKey(subKey)) //item1.Substring(0, item1.LastIndexOf('.'))
                    (((BaseFunction)function).RefSource3)[item1] = GlobalProgram.ProgramItems[subKey];
            }
            ////// 引用4
            keys = new string[((BaseFunction)function).RefSource4.Count];
            ((BaseFunction)function).RefSource4.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                subKey = item1.Substring(0, item1.LastIndexOf('.'));
                if (GlobalProgram.ProgramItems.ContainsKey(subKey)) //item1.Substring(0, item1.LastIndexOf('.'))
                    (((BaseFunction)function).RefSource4)[item1] = GlobalProgram.ProgramItems[subKey];
            }
        }
        private void UninitRefSource(IFunction function)
        {
            string[] keys;
            if (function == null) return;
            keys = new string[((BaseFunction)function).RefSource1.Count];
            ((BaseFunction)function).RefSource1.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                (((BaseFunction)function).RefSource1)[item1] = null;
            }
            keys = new string[((BaseFunction)function).RefSource2.Count];
            ((BaseFunction)function).RefSource2.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                (((BaseFunction)function).RefSource2)[item1] = null;
            }
            keys = new string[((BaseFunction)function).RefSource3.Count];
            ((BaseFunction)function).RefSource3.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                (((BaseFunction)function).RefSource3)[item1] = null;
            }
            keys = new string[((BaseFunction)function).RefSource4.Count];
            ((BaseFunction)function).RefSource4.Keys.CopyTo(keys, 0);
            foreach (var item1 in keys)
            {
                (((BaseFunction)function).RefSource4)[item1] = null;
            }
        }

        /// <summary>
        /// 保存程序
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="items"></param>
        public bool SaveProgram(string filePath)
        {
            bool result = false;
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                // 保存树视图
                List<TreeNode> items = GetTreeViewNode();
                if (Directory.Exists(filePath))
                    Directory.Delete(filePath, true);
                Directory.CreateDirectory(filePath); // 每次保存都新建
                string fileName = filePath + "\\" + "treeview.dat";
                using (FileStream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, items);
                }
                result = true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString() + "保存程序树视图出错");
                result = false;
            }
            /// 保存程序条目 创建对象 
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(1)") && GlobalProgram.ProgramItems["特征定位(1)"] == null)
                GlobalProgram.ProgramItems["特征定位(1)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(2)") && GlobalProgram.ProgramItems["特征定位(2)"] == null)
                GlobalProgram.ProgramItems["特征定位(2)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(3)") && GlobalProgram.ProgramItems["特征定位(3)"] == null)
                GlobalProgram.ProgramItems["特征定位(3)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(4)") && GlobalProgram.ProgramItems["特征定位(4)"] == null)
                GlobalProgram.ProgramItems["特征定位(4)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(5)") && GlobalProgram.ProgramItems["特征定位(5)"] == null)
                GlobalProgram.ProgramItems["特征定位(5)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(6)") && GlobalProgram.ProgramItems["特征定位(6)"] == null)
                GlobalProgram.ProgramItems["特征定位(6)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(7)") && GlobalProgram.ProgramItems["特征定位(7)"] == null)
                GlobalProgram.ProgramItems["特征定位(7)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(8)") && GlobalProgram.ProgramItems["特征定位(8)"] == null)
                GlobalProgram.ProgramItems["特征定位(8)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(9)") && GlobalProgram.ProgramItems["特征定位(9)"] == null)
                GlobalProgram.ProgramItems["特征定位(9)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(10)") && GlobalProgram.ProgramItems["特征定位(10)"] == null)
                GlobalProgram.ProgramItems["特征定位(10)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(11)") && GlobalProgram.ProgramItems["特征定位(11)"] == null)
                GlobalProgram.ProgramItems["特征定位(11)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(12)") && GlobalProgram.ProgramItems["特征定位(12)"] == null)
                GlobalProgram.ProgramItems["特征定位(12)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(13)") && GlobalProgram.ProgramItems["特征定位(13)"] == null)
                GlobalProgram.ProgramItems["特征定位(13)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(14)") && GlobalProgram.ProgramItems["特征定位(14)"] == null)
                GlobalProgram.ProgramItems["特征定位(14)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(15)") && GlobalProgram.ProgramItems["特征定位(15)"] == null)
                GlobalProgram.ProgramItems["特征定位(15)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(16)") && GlobalProgram.ProgramItems["特征定位(16)"] == null)
                GlobalProgram.ProgramItems["特征定位(16)"] = new FeatureLocalization();
            if (GlobalProgram.ProgramItems.ContainsKey("特征定位(17)") && GlobalProgram.ProgramItems["特征定位(17)"] == null)
                GlobalProgram.ProgramItems["特征定位(17)"] = new FeatureLocalization();
            //// 再将Tag节点的引用还原回去
            for (int i = 0; i < this.treeView.Nodes.Count; i++)
            {
                this.SetTag(this.treeView.Nodes[i]);
            }
            /// 保存程序条目
            foreach (KeyValuePair<string, IFunction> item in GlobalProgram.ProgramItems)
            {
                try
                {
                    //binFormat = new BinaryFormatter();
                    this.UninitRefSource(item.Value); // 先去除算子的引用对象，再保存，这样做是为了解偶
                    string pathDirectoryName = filePath + "\\" + item.Key + "-" + item.Value?.GetType().FullName + ".txt";  // + "." + item.Value.GetType().Name,将完整的类型名保存到文件名中
                    item.Value?.Save(pathDirectoryName);
                    using (FileStream fStream = new FileStream(pathDirectoryName, FileMode.Create, FileAccess.Write))
                    {
                        if (item.Value != null)
                            binFormat.Serialize(fStream, item.Value);
                    }
                    this.InitRefSource(item.Value); // 保存后再恢复引用对象
                    result = result && true;
                }
                catch (Exception ex)
                {
                    result = false;
                    MessageBox.Show("保存对象：" + item.Key + "失败" + ex.ToString());
                }
            }
            return result;
        }

        public List<TreeNode> GetTreeViewNodeTag()
        {
            List<TreeNode> list = new List<TreeNode>();
            GlobalProgram.ProgramItems.Clear();
            for (int i = 0; i < this.treeView.Nodes.Count; i++)
            {
                GlobalProgram.ProgramItems.Add(this.treeView.Nodes[i].Text, (IFunction)this.treeView.Nodes[i].Tag);
                list.Add(this.treeView.Nodes[i]);
            }
            return list;
        }
        private void GetTag(TreeNode node)
        {
            if (node.Name.Contains("Tool"))
            {
                string keyName = "";  // node.Name.Substring(0, node.Name.LastIndexOf('.'));
                int index = node.Name.LastIndexOf('.');
                if (index > 0)
                    keyName = node.Name.Substring(0, index);
                if (!GlobalProgram.ProgramItems.ContainsKey(keyName))
                    GlobalProgram.ProgramItems.Add(keyName, (IFunction)node.Tag);
                node.Tag = null;
                foreach (TreeNode item in node.Nodes)
                {
                    this.GetTag(item);
                }
            }
            else
            {
                string keyName = " ";
                int index = node.Name.LastIndexOf('.');
                if (index > 0)
                    keyName = node.Name.Substring(0, index);
                if (!GlobalProgram.ProgramItems.ContainsKey(keyName))
                    GlobalProgram.ProgramItems.Add(keyName, (IFunction)node.Tag);
                node.Tag = null; // 去悼Tag属性的引用，是为了将节点与对象的保存隔离
                /// 对应的子节点属性也要去悼
                foreach (TreeNode item2 in node.Nodes)
                    item2.Tag = null;
            }
        }
        private void SetTag(TreeNode node)
        {
            if (node.Name.Contains("Tool"))
            {
                string keyName = "";
                int index = node.Name.LastIndexOf('.');
                if (index > 0)
                    keyName = node.Name.Substring(0, index);
                if (GlobalProgram.ProgramItems.ContainsKey(keyName))
                    node.Tag = GlobalProgram.ProgramItems[keyName]; // 去悼Tag属性的引用，是为了将节点与对象的保存隔离
                else
                    node.Tag = null;
                //////////////////////
                foreach (TreeNode item in node.Nodes)
                {
                    this.SetTag(item);
                }
            }
            else
            {
                string keyName = ""; //  node.Name.Substring(0, node.Name.LastIndexOf('.'));
                int index = node.Name.LastIndexOf('.');
                if (index > 0)
                    keyName = node.Name.Substring(0, index);
                if (GlobalProgram.ProgramItems.ContainsKey(keyName))
                    node.Tag = GlobalProgram.ProgramItems[keyName]; // 去悼Tag属性的引用，是为了将节点与对象的保存隔离
                else
                    node.Tag = null;
            }
        }

        private void SetTagName(TreeNode node)
        {
            if (node.Name.Contains("Tool")) // 表示该节点为工具节点
            {
                foreach (TreeNode item in node.Nodes)
                {
                    string name = item.FullPath.Replace("\\", ".") + item.Name.Substring(item.Name.LastIndexOf('.'));
                    item.Name = name;
                    ((IFunction)item.Tag).Name = name;
                    UpdataRefSource(((IFunction)item.Tag), "RefSource1");
                    UpdataRefSource(((IFunction)item.Tag), "RefSource2");
                    UpdataRefSource(((IFunction)item.Tag), "RefSource3");
                    UpdataRefSource(((IFunction)item.Tag), "RefSource4");
                    SetTagReferenceName(item);
                    if (item.Name.Contains("Tool"))
                    {
                        SetTagName(item);
                    }
                }
            }
            else
            {
                string name = node.FullPath.Replace("\\", ".") + node.Name.Substring(node.Name.LastIndexOf('.'));
                node.Name = name;
                ((IFunction)node.Tag).Name = name;
                SetTagReferenceName(node);
            }
        }
        private void UpdataRefSource(IFunction function, string RefSource = "RefSource1")
        {
            switch (RefSource)
            {
                case "RefSource1":
                    if (((BaseFunction)function).RefSource1.Count > 0)
                    {
                        string[] newKeys = new string[((BaseFunction)function).RefSource1.Count];
                        string[] keys = new string[((BaseFunction)function).RefSource1.Count];
                        IFunction[] functions = new IFunction[((BaseFunction)function).RefSource1.Count];
                        ((BaseFunction)function).RefSource1.Values.CopyTo(functions, 0);
                        ((BaseFunction)function).RefSource1.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            newKeys[i] = function.Name.Split('.')[0] + keys[i].Substring(keys[i].IndexOf('.'));
                        }
                        ((BaseFunction)function).RefSource1.Clear();
                        for (int i = 0; i < newKeys.Length; i++)
                        {
                            ((BaseFunction)function).RefSource1.Add(newKeys[i], functions[i]);
                        }
                    }
                    break;
                case "RefSource4":
                    if (((BaseFunction)function).RefSource4.Count > 0)
                    {
                        string[] newKeys = new string[((BaseFunction)function).RefSource4.Count];
                        string[] keys = new string[((BaseFunction)function).RefSource4.Count];
                        IFunction[] functions = new IFunction[((BaseFunction)function).RefSource4.Count];
                        ((BaseFunction)function).RefSource4.Values.CopyTo(functions, 0);
                        ((BaseFunction)function).RefSource4.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            newKeys[i] = function.Name.Split('.')[0] + keys[i].Substring(keys[i].IndexOf('.'));
                        }
                        ((BaseFunction)function).RefSource4.Clear();
                        for (int i = 0; i < newKeys.Length; i++)
                        {
                            ((BaseFunction)function).RefSource4.Add(newKeys[i], functions[i]);
                        }
                    }
                    break;
                case "RefSource2":
                    if (((BaseFunction)function).RefSource2.Count > 0)
                    {
                        string[] newKeys = new string[((BaseFunction)function).RefSource2.Count];
                        string[] keys = new string[((BaseFunction)function).RefSource2.Count];
                        IFunction[] functions = new IFunction[((BaseFunction)function).RefSource2.Count];
                        ((BaseFunction)function).RefSource2.Values.CopyTo(functions, 0);
                        ((BaseFunction)function).RefSource2.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            newKeys[i] = function.Name.Split('.')[0] + keys[i].Substring(keys[i].IndexOf('.'));
                        }
                        ((BaseFunction)function).RefSource2.Clear();
                        for (int i = 0; i < newKeys.Length; i++)
                        {
                            ((BaseFunction)function).RefSource2.Add(newKeys[i], functions[i]);
                        }
                    }
                    break;
                case "RefSource3":
                    if (((BaseFunction)function).RefSource3.Count > 0)
                    {
                        string[] newKeys = new string[((BaseFunction)function).RefSource3.Count];
                        string[] keys = new string[((BaseFunction)function).RefSource3.Count];
                        IFunction[] functions = new IFunction[((BaseFunction)function).RefSource3.Count];
                        ((BaseFunction)function).RefSource3.Values.CopyTo(functions, 0);
                        ((BaseFunction)function).RefSource3.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            newKeys[i] = function.Name.Split('.')[0] + keys[i].Substring(keys[i].IndexOf('.'));
                        }
                        ((BaseFunction)function).RefSource3.Clear();
                        for (int i = 0; i < newKeys.Length; i++)
                        {
                            ((BaseFunction)function).RefSource3.Add(newKeys[i], functions[i]);
                        }
                    }
                    break;
            }
        }
        private void SetTagReferenceName(TreeNode node)
        {
            foreach (TreeNode item in node.Nodes)
            {
                if (item.Name.Contains("RefSource"))
                {
                    foreach (TreeNode item3 in item.Nodes)
                    {
                        if (item3.Text.Contains(".")) // 表示引用的节点不是根节点
                        {
                            string str1 = item3.FullPath.Replace("\\", ".").Split('.')[0];
                            string str2 = item3.Name.Substring(item3.Name.IndexOf('.'));
                            item3.Name = str1 + str2;
                            str2 = item3.Text.Substring(item3.Text.IndexOf('.'));
                            item3.Text = str1 + str2;
                        }
                    }
                }
                else
                    SetTagReferenceName(item);
            }
        }
        public List<TreeNode> GetTreeViewNode()
        {
            List<TreeNode> list = new List<TreeNode>();
            GlobalProgram.ProgramItems.Clear();
            string keyName = "";
            for (int i = 0; i < this.treeView.Nodes.Count; i++)
            {
                GetTag(this.treeView.Nodes[i]);
                list.Add(this.treeView.Nodes[i]);
            }
            return list;
        }

        public void LoadTreeViewNode(TreeNodeCollection list)
        {
            this.treeView.Nodes.Clear();
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!this.treeView.Nodes.Contains(list[i]))
                        this.treeView.Nodes.Add(list[i]);
                }
            }
        }
        public void LoadTreeViewNode(List<TreeNode> list)
        {
            this.treeView.Nodes.Clear();
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!this.treeView.Nodes.Contains(list[i]))
                        this.treeView.Nodes.Add(list[i]);
                }
            }
        }
        public void ClearTreeView()
        {
            List<TreeNode> list = new List<TreeNode>();
            TreeNodeCollection Collection = this.treeView.Nodes;
            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].Tag != null && Collection[i].Tag is AcqSource)
                    list.Add(Collection[i]);
                else
                    ((IFunction)Collection[i].Tag)?.ReleaseHandle();
            }
            /////////////
            this.treeView.Nodes.Clear();
            this.OnClearNode();
            DataInteractionClass.getInstance().OnClearGraphic(); // 清空图形窗口
            if (list.Count > 0)
                this.treeView.Nodes.AddRange(list.ToArray());
        }

        #region 在TreeView上添加/减少控件

        private int getItemCount(TreeView control, string name)
        {
            int num = 0;
            string item = "";
            for (int i = 0; i < control.Nodes.Count; i++)
            {
                item = control.Nodes[i].Text;
                if (name == item.Split('(', ')')[0])
                    //num++;
                    num = int.Parse(item.Split('(', ')')[1]);
            }
            return num;
        }
        public void AddItems(IFunction function, string name, params string[] propertyName)
        {
            try
            {
                DescriptionAttribute descriptionCustomAttribute;
                DisplayNameAttribute displayCustomAttribute;
                DefaultPropertyAttribute DefaultCustomAttribute;
                PropertyInfo propertyInfo;
                // 添加子节点
                TreeNode childNode;
                int Count = getItemCount(this.treeView, name);
                string curName = name + "(" + (Count + 1).ToString() + ")";
                if (this.ToolName != null && this.ToolName.Length > 0)
                {
                    //curName = name + "(" + (Count + 1).ToString() + ")";
                    function.Name = this.ToolName + "." + curName;
                }
                else
                {
                    function.Name = curName; // name + "(" + (Count + 1).ToString() + ")";
                }
                TreeNode rootNode = this.treeView.Nodes.Add(function.Name, curName); //function.Name
                rootNode.Tag = function; // Tag 只引用对象的名称，
                rootNode.ToolTipText = function.GetType().FullName;
                /// 获取属性的描述符 
                DefaultCustomAttribute = Attribute.GetCustomAttribute(function.GetType(), typeof(DefaultPropertyAttribute)) as DefaultPropertyAttribute;  // 获取属性的定制特性
                if (DefaultCustomAttribute == null)
                    MessageBox.Show("添加的类型没有定义默认属性，请先定义类的默认属性");
                else
                    rootNode.Name = function.Name + "." + DefaultCustomAttribute.Name; // 根节点引用默认属性名 , 让根节点都引用默认属性, 用对象名 + 属性名来定义节点的名字
                //////////////////////////////////////////////////////////////////////////
                if (propertyName != null || propertyName.Length == 0) // 表示没有子节点，只输入或输出一个默认属性
                {
                    BaseFunction baseFun = ((BaseFunction)function);
                    for (int i = 0; i < propertyName.Length; i++)
                    {
                        propertyInfo = function.GetType().GetProperty(propertyName[i]);
                        displayCustomAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayNameAttribute)) as DisplayNameAttribute;  // 获取属性的显示特性
                        descriptionCustomAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;  // 获取属性的描述特性
                        if (displayCustomAttribute == null)
                        {
                            childNode = rootNode.Nodes.Add(function.Name + "." + propertyName[i], propertyName[i]);
                            childNode.Tag = function; // 子节点也引用同一个对象
                        }
                        else
                        {
                            childNode = rootNode.Nodes.Add(function.Name + "." + propertyName[i], displayCustomAttribute.DisplayName);
                            childNode.Tag = function; // 子节点也引用同一个对象
                        }
                        // 描述属性不为 null
                        if (descriptionCustomAttribute != null)
                        {
                            // 输入节点的名称设置为引用对象的源，输出节点的名称设置为该对象的属性名
                            switch (descriptionCustomAttribute.Description)
                            {
                                case "采集源":
                                case "采集源1":
                                case "采集源2":
                                case "采集源3":
                                case "采集源4":
                                case "激光采集源":
                                case "相机采集源":
                                    propertyInfo = function.GetType().GetProperty(propertyName[i]);
                                    AcqSource _acqSource = propertyInfo.GetValue(function) as AcqSource;
                                    //////////////////////////////////////采集源为空时，不添加
                                    if (_acqSource != null)
                                    {
                                        childNode.Text = "<-" + descriptionCustomAttribute.Description; //_acqSource.Name;
                                        childNode.Name = propertyName[i];
                                    }
                                    break;
                                case "输入属性1":
                                    childNode.Text = "<-" + childNode.Text;
                                    childNode.Name = nameof(baseFun.RefSource1);// "RefSource1";
                                    // 因为引用可以在创建对象时添加，也可以在拖动对象时添加，所以在创建节点时需要遍历引用源
                                    foreach (var item in ((BaseFunction)function).RefSource1)
                                    {
                                        DefaultCustomAttribute = Attribute.GetCustomAttribute(item.Value.GetType(), typeof(DefaultPropertyAttribute)) as DefaultPropertyAttribute;
                                        propertyInfo = item.Value.GetType().GetProperty(DefaultCustomAttribute.Name);
                                        //displayCustomAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayNameAttribute)) as DisplayNameAttribute;  // 获取属性的显示特性
                                        childNode.Nodes.Add(item.Key, propertyInfo.GetValue(item.Value).ToString()); // 这里是做什么用的？
                                    }
                                    break;
                                case "输入属性2":
                                    childNode.Text = "<-" + childNode.Text;
                                    childNode.Name = nameof(baseFun.RefSource2);//  "RefSource2";
                                    // 因为引用可以在创建对象时添加，也可以在拖动对象时添加，所以在创建节点时需要遍历引用源
                                    foreach (var item in ((BaseFunction)function).RefSource2)
                                    {
                                        DefaultCustomAttribute = Attribute.GetCustomAttribute(item.Value.GetType(), typeof(DefaultPropertyAttribute)) as DefaultPropertyAttribute;
                                        propertyInfo = item.Value.GetType().GetProperty(DefaultCustomAttribute.Name);
                                        //displayCustomAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayNameAttribute)) as DisplayNameAttribute;  // 获取属性的显示特性
                                        childNode.Nodes.Add(item.Key, propertyInfo.GetValue(item.Value).ToString()); // 这里是做什么用的？
                                    }
                                    break;
                                case "输入属性3":
                                    childNode.Text = "<-" + childNode.Text;
                                    childNode.Name = nameof(baseFun.RefSource3);// "RefSource3";
                                    // 因为引用可以在创建对象时添加，也可以在拖动对象时添加，所以在创建节点时需要遍历引用源
                                    foreach (var item in ((BaseFunction)function).RefSource3)
                                    {
                                        DefaultCustomAttribute = Attribute.GetCustomAttribute(item.Value.GetType(), typeof(DefaultPropertyAttribute)) as DefaultPropertyAttribute;
                                        propertyInfo = item.Value.GetType().GetProperty(DefaultCustomAttribute.Name);
                                        //displayCustomAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayNameAttribute)) as DisplayNameAttribute;  // 获取属性的显示特性
                                        childNode.Nodes.Add(item.Key, propertyInfo.GetValue(item.Value).ToString()); // 这里是做什么用的？
                                    }
                                    break;
                                case "输入属性4":
                                    childNode.Text = "<-" + childNode.Text;
                                    childNode.Name = nameof(baseFun.RefSource4);// "RefSource4";
                                    // 因为引用可以在创建对象时添加，也可以在拖动对象时添加，所以在创建节点时需要遍历引用源
                                    foreach (var item in ((BaseFunction)function).RefSource4)
                                    {
                                        DefaultCustomAttribute = Attribute.GetCustomAttribute(item.Value.GetType(), typeof(DefaultPropertyAttribute)) as DefaultPropertyAttribute;
                                        propertyInfo = item.Value.GetType().GetProperty(DefaultCustomAttribute.Name);
                                        childNode.Nodes.Add(item.Key, propertyInfo.GetValue(item.Value).ToString()); // 这里是做什么用的？
                                    }
                                    break;
                                //case "输入属性5":
                                //    childNode.Text = "<-" + childNode.Text;
                                //    childNode.Name = nameof(baseFun.RefSource5);// "RefSource5";
                                //    break;
                                //case "输入属性6":
                                //    childNode.Text = "<-" + childNode.Text;
                                //    childNode.Name = nameof(baseFun.RefSource6);// "RefSource6";
                                //    break;
                                //case "输入属性7":
                                //    childNode.Text = "<-" + childNode.Text;
                                //    childNode.Name = nameof(baseFun.RefSource7);// "RefSource7";  
                                //    break;
                                //case "输入属性8":
                                //    childNode.Text = "<-" + childNode.Text;
                                //    childNode.Name = nameof(baseFun.RefSource8);// "RefSource8";
                                //    break;
                                case "输出属性":
                                    childNode.Text = "->" + childNode.Text;
                                    childNode.Name = function.Name + "." + propertyName[i]; // propertyName[i];
                                    break;
                            }
                        }
                        childNode.Expand();
                    }
                }
                string key = rootNode.Name.Substring(0, rootNode.Name.LastIndexOf('.'));
                if (!GlobalProgram.ProgramItems.ContainsKey(key))
                    GlobalProgram.ProgramItems.Add(key, function);

                ////触发事件
                this.OnAddNode(rootNode); // 在treeView上添加节点时触发添加节点事件
                this.OnClickNode(rootNode, MouseButtons.Left, 2, 0, 0); // 在添加节点时也触发一次节点点击事件
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加对象出错：" + ex.ToString());
            }
        }
        public void AddItems(AcqSource acqSource, string name, params string[] propertyName)
        {
            int Count = getItemCount(this.treeView, name);
            if (Count > 0) return;
            acqSource.Name = "采集源" + "=" + name; //+ "(" + (Count + 1).ToString() + ")"
            string noodName = acqSource.Name;
            TreeNode rootNode = this.treeView.Nodes.Add(name, noodName);  // name:即键标识
            rootNode.Tag = acqSource;
            // 添加子节点
            if (propertyName == null) return;
            for (int i = 0; i < propertyName.Length; i++)
            {
                rootNode.Nodes.Add(propertyName[i], propertyName[i]);
            }
            this.OnAddNode(rootNode);
        }

        #endregion

        #region 右键菜单项
        private void addContextMenu(TreeView treeView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            this.treeView.LabelEdit = true; //  可以编辑结点的名称
                                            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("设置参数"),
                new ToolStripMenuItem("执行"),
                new ToolStripMenuItem("启用"),
                new ToolStripMenuItem("禁用"),
                //new ToolStripMenuItem("置为当前采集源"),
                //new ToolStripMenuItem("取消当前采集源"),
                new ToolStripMenuItem("重置光源"),
                new ToolStripMenuItem("复制"),
                new ToolStripMenuItem("粘贴"),
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
                new ToolStripMenuItem("重命名")
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            this.treeView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            IFunction item;
            string name = e.ClickedItem.Text;
            ContextMenuStrip context = (ContextMenuStrip)sender;
            try
            {
                switch (name)
                {
                    case "设置参数":
                        treeView1_Edite(this, this.treeView.SelectedNode);
                        break;
                    case "执行":
                        context.Close();
                        this.node = this.treeView.SelectedNode;
                        item = this.treeView.SelectedNode.Tag as IFunction;
                        Task.Run(() => item?.Execute(this.node));
                        if (item == null)
                            MessageBox.Show(this.node.Text + ":Tag对象为空引用");
                        break;
                    case "禁用":
                        context.Close();
                        this.treeView.SelectedNode.Checked = true; // 这里为什么要用相反的，只是因为节点在创建时，默认是非选择状态
                        this.treeView.SelectedNode.ForeColor = System.Drawing.SystemColors.GrayText;
                        break;
                    case "启用":
                        context.Close();
                        this.treeView.SelectedNode.Checked = false;
                        this.treeView.SelectedNode.ForeColor = System.Drawing.SystemColors.WindowText;
                        break;
                    case "复制":
                        context.Close();
                        this.node = (TreeNode)this.treeView.SelectedNode.Clone();
                        Clipboard.SetDataObject(this.node, false);
                        break;
                    case "粘贴":
                        context.Close();
                        this.node = Clipboard.GetDataObject().GetData(typeof(TreeNode)) as TreeNode;
                        if (this.node == null) return;
                        this.node.Name = this.node?.Name.Replace(this.node?.Text, this.node?.Text + "(复制)");
                        this.node.Text = this.node?.Text.Replace(this.node?.Text, this.node?.Text + "(复制)");
                        ((IFunction)this.node.Tag).Name = this.node.Name;
                        if (this.treeView.SelectedNode != null)
                            this.treeView.Nodes.Insert(this.treeView.SelectedNode.Index + 1, this.node);
                        else
                            this.treeView.Nodes.Insert(0, this.node);
                        this.SetTagName(this.node); // RefSource1
                        this.OnAddNode(this.node);
                        break;
                    case "删除":
                        context.Close();
                        if (this.treeView.SelectedNode.Parent == null) // 用来删除根节点
                        {
                            string key = this.treeView.SelectedNode.Name.Substring(0, this.treeView.SelectedNode.Name.LastIndexOf('.'));
                            if (GlobalProgram.ProgramItems.ContainsKey(key))
                                GlobalProgram.ProgramItems.Remove(key);
                            /////////////////////////
                            if (this.treeView.SelectedNode.Tag is IFunction)
                            {
                                item = this.treeView.SelectedNode.Tag as IFunction;
                                this.treeView.SelectedNode.Tag = null;
                                this.treeView.SelectedNode.Remove();
                                item.ReleaseHandle(); // 删除某节点时，先释放某节点的资源， 放到最后是为了，防止释放资源时报错，而没有删除节点
                            }
                            else // 可以删除采集源节点
                            {
                                this.treeView.SelectedNode.Tag = null;
                                this.treeView.SelectedNode.Remove();
                            }
                        }
                        else
                        {
                            switch (this.treeView.SelectedNode.Parent.Name) // 用来删除引用的节点对象
                            {
                                case "RefSource1":
                                    ((BaseFunction)this.treeView.SelectedNode.Parent.Parent.Tag).RefSource1.Remove(this.treeView.SelectedNode.Name);
                                    this.treeView.SelectedNode.Remove();
                                    break;
                                case "RefSource2":
                                    ((BaseFunction)this.treeView.SelectedNode.Parent.Parent.Tag).RefSource2.Remove(this.treeView.SelectedNode.Name);
                                    this.treeView.SelectedNode.Remove();
                                    break;
                                case "RefSource3":
                                    ((BaseFunction)this.treeView.SelectedNode.Parent.Parent.Tag).RefSource3.Remove(this.treeView.SelectedNode.Name);
                                    this.treeView.SelectedNode.Remove();
                                    break;
                                case "RefSource4":
                                    ((BaseFunction)this.treeView.SelectedNode.Parent.Parent.Tag).RefSource4.Remove(this.treeView.SelectedNode.Name);
                                    this.treeView.SelectedNode.Remove();
                                    break;
                                default:
                                    this.treeView.SelectedNode.Remove();
                                    break;
                            }
                            this.OnDeleteNode(this.treeView.SelectedNode); // 触发节点删除事件
                        }
                        //////////////////////////////////////////////////////////
                        break;
                    case "清空":
                        context.Close();
                        ClearTreeView();
                        GlobalProgram.ProgramItems.Clear();
                        break;
                    case "重置光源":
                        if (this.treeView.SelectedNode.Tag == null) return;
                        if (!(this.treeView.SelectedNode.Tag is IFunction)) return;
                        if (LightConnectManage.CurrentLight != null)
                        {
                            foreach (var item2 in LightConnectManage.CurrentLight.LightParamList)
                            {
                                ((BaseFunction)this.treeView.SelectedNode.Tag).LightParam.Add(item2.Clone());
                            }
                        }
                        break;

                    case "重命名":
                        if (this.treeView.SelectedNode == null) return;
                        this.node = this.treeView.SelectedNode;
                        ResetNameForm resetNameForm = new ResetNameForm(this.treeView.SelectedNode.Text);
                        resetNameForm.ShowDialog();
                        if (resetNameForm.newName.Contains('-'))
                        {
                            MessageBox.Show("新名字中不能包含有 - 字符!!!");
                            return;
                        }
                        if (resetNameForm.IsOk)
                        {
                            foreach (TreeNode node in this.treeView.Nodes)
                            {
                                if (node.Text == resetNameForm.newName)
                                {
                                    MessageBox.Show("已存在相同名字的节点!");
                                    return;
                                }
                            }
                            if (GlobalProgram.ProgramItems.ContainsKey(resetNameForm.oldName))
                            {
                                GlobalProgram.ProgramItems.Add(resetNameForm.newName, GlobalProgram.ProgramItems[resetNameForm.oldName]);
                                GlobalProgram.ProgramItems.Remove(resetNameForm.oldName);
                            }
                            this.treeView.SelectedNode.Name = this.treeView.SelectedNode.Name.Replace(resetNameForm.oldName, resetNameForm.newName);
                            this.treeView.SelectedNode.Text = resetNameForm.newName;
                            this.SetTagName(this.node); // 子节点的引用也需要同步更新
                            if (this.treeView.SelectedNode.Tag != null)
                            {
                                ((IFunction)this.treeView.SelectedNode.Tag).Name = ((IFunction)this.treeView.SelectedNode.Tag).Name.Replace(resetNameForm.oldName, resetNameForm.newName);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void ReSetNodeName(TreeNode node, string name)
        {
            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode item in node.Nodes)
                {
                    this.ReSetNodeName(item, name + "." + node.Text.Split(')')[0] + ")");
                }
            }
            else
            {
                node.Name = name + "." + node.Text.Split(')')[0] + ")";
            }
        }
        #endregion
        private void listBoxWrapClass_ItemChange(object send, ItemsChangeEventArgs e)
        {
            try
            {
                string key = ((IFunction)e.Function).GetPropertyValues("名称").ToString();  //先查找父节点 
                TreeNode[] nodes = this.treeView.Nodes.Find(key, false);  //表示父节点的集合
                switch (e.ItemSource) // 表示对谁执行了添加、删除操作
                {
                    case "RefSource1":
                        if (nodes != null && nodes.Length > 0)
                        {
                            if (nodes[0].Nodes["RefSource1"] != null)
                                nodes[0].Nodes["RefSource1"].Nodes.Clear();
                            foreach (var item in ((BaseFunction)e.Function).RefSource1.Keys)
                            {
                                nodes[0].Nodes["RefSource1"].Nodes.Add(item, item);
                            }
                        }
                        break;
                    case "RefSource2":
                        if (nodes != null && nodes.Length > 0)
                        {
                            if (nodes[0].Nodes["RefSource2"] != null)
                                nodes[0].Nodes["RefSource2"].Nodes.Clear();
                            foreach (var item in ((BaseFunction)e.Function).RefSource2.Keys)
                            {
                                nodes[0].Nodes["RefSource2"].Nodes.Add(item, item);
                            }
                        }
                        break;
                    case "RefSource3":
                        if (nodes != null && nodes.Length > 0)
                        {
                            if (nodes[0].Nodes["RefSource3"] != null)
                                nodes[0].Nodes["RefSource3"].Nodes.Clear();
                            foreach (var item in ((BaseFunction)e.Function).RefSource3.Keys)
                            {
                                nodes[0].Nodes["RefSource3"].Nodes.Add(item, item);
                            }
                        }
                        break;
                    case "RefSource4":
                        if (nodes != null && nodes.Length > 0)
                        {
                            if (nodes[0].Nodes["RefSource4"] != null)
                                nodes[0].Nodes["RefSource4"].Nodes.Clear();
                            foreach (var item in ((BaseFunction)e.Function).RefSource4.Keys)
                            {
                                nodes[0].Nodes["RefSource4"].Nodes.Add(item, item);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(new Exception().ToString());
            }
        }


        #region 在控件上执行拖放操作
        private void treeView1_DragDrop(object sender, DragEventArgs e) // 在自己身上实现拖放操作
        {
            try
            {
                TreeNode DragNode = (TreeNode)e.Data.GetData(typeof(TreeNode)); // 表示拖动的节点 
                TreeNode selectNode = this.treeView.GetNodeAt(this.treeView.PointToClient(new System.Drawing.Point(e.X, e.Y))); // 表示要拖放到其上的节点
                if (selectNode == null) return;
                if (selectNode?.Parent == null && DragNode?.Parent == null) // 表示落地处的节点为父节点,如果是父节点，则表示拖动节点
                {
                    int index = selectNode.Index;
                    // 拖动某节点时，必需先清除再插入
                    this.treeView.Nodes.Remove(DragNode);
                    this.treeView.Nodes.Insert(index, DragNode);
                }
                else  // 表示落地处的节点为子节点,表示为子节点添加元素
                {
                    if (DragNode.Name == null || DragNode.Name.Length == 0)
                    {
                        MessageBox.Show("拖动的节点没有绑定相应的属性名");
                        return;
                    }
                    if (DragNode.Name.Contains("RefSource"))
                    {
                        MessageBox.Show("拖动的节点为非输出节点，请选择输出节点!");
                        return;
                    }
                    if (!selectNode.Name.Contains("RefSource"))
                    {
                        MessageBox.Show("接受的节点为非输入节点! 请选择正确的输入节点");
                        return;
                    }
                    if (DragNode.Tag == null)
                        DragNode.Tag = DragNode.Parent.Tag;
                    if (selectNode.Tag == null)
                        selectNode.Tag = selectNode.Parent.Tag;
                    if (DragNode.Tag == null)
                    {
                        MessageBox.Show("拖动的节点或其父节点Tag属性没有绑定相应的对象");
                        return;
                    }
                    if (selectNode.Tag == null && selectNode.Parent.Tag == null) //selectNode.Tag == null &&  Parent.
                    {
                        MessageBox.Show("接受的节点或其父节点Tag属性没有绑定相应的对象");
                        return;
                    }
                    ////////////////////////////////////////////////
                    string sourceDataType = "", targetDataType = "";
                    DescriptionAttribute attributes;
                    PropertyInfo[] propertyInfos;
                    PropertyInfo propertyInfo;
                    ////////////////////////////////////////////////
                    switch (selectNode.Name)
                    {
                        case "AcqSource":
                            propertyInfo = selectNode.Tag.GetType().GetProperty(selectNode.Name); //Parent.
                            propertyInfo?.SetValue(selectNode.Tag, DragNode.Tag); //Parent.
                            break;
                        case "RefSource1":
                            // 获取拖动对象的对象类型
                            sourceDataType = DragNode.Tag.GetType().GetProperty(DragNode.Name.Split('.').Last())?.GetValue(DragNode.Tag)?.GetType().Name;
                            ///////////////////// 获取接爱数据的对象类型
                            propertyInfos = selectNode.Tag?.GetType().GetProperties();  //Parent.
                            foreach (var item in propertyInfos)
                            {
                                attributes = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;  // 获取属性的描述特性
                                if (attributes != null && attributes.Description == "输入属性1")
                                {
                                    targetDataType = item.GetValue(selectNode.Tag)?.GetType().Name;  //Parent.
                                    break;
                                }
                            }
                            ///////////////////////////////////////////////
                            if (DragNode.Parent == null)
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource1.ContainsKey(DragNode.Name)) //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource1.Remove(DragNode.Name);
                                }
                                selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.'))); // 拖过来的节点不引用对象
                                ((BaseFunction)selectNode.Tag).RefSource1.Add(DragNode.Name, (IFunction)DragNode.Tag);// 这里要添加拖动过来的对象  Parent.
                            }
                            else
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource1.ContainsKey(DragNode.Name))  //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource1.Remove(DragNode.Name);
                                }
                                if (DragNode.Name.Contains(DragNode.Text))
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')));
                                else
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')) + "." + DragNode.Text.Split(new string[2] { "->", "<-" }, StringSplitOptions.RemoveEmptyEntries)[0]);  //DragNode.Parent.Text.Substring(0, DragNode.Parent.Text.LastIndexOf(')') + 1)
                                ((BaseFunction)selectNode.Tag).RefSource1.Add(DragNode.Name, (IFunction)DragNode.Tag);  //Parent.  Parent.
                            }

                            selectNode.Expand();
                            break;
                        case "RefSource2":
                            // 获取拖动对象的对象类型
                            sourceDataType = DragNode.Tag.GetType().GetProperty(DragNode.Name.Split('.').Last())?.GetValue(DragNode.Tag)?.GetType().Name;
                            ///////////////////// 获取接爱数据的对象类型
                            propertyInfos = selectNode.Tag.GetType().GetProperties();  //Parent.
                            foreach (var item in propertyInfos)
                            {
                                attributes = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;  // 获取属性的描述特性
                                if (attributes != null && attributes.Description == "输入属性2")
                                {
                                    targetDataType = item.GetValue(selectNode.Tag)?.GetType().Name;  //Parent.
                                    break;
                                }
                            }
                            ///////////////////////////////////////////////
                            if (DragNode.Parent == null)
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource2.ContainsKey(DragNode.Name))  //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource2.Remove(DragNode.Name);
                                }
                                selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')));
                                ((BaseFunction)selectNode.Tag).RefSource2.Add(DragNode.Name, (IFunction)DragNode.Tag);// 这里要添加拖动过来的对象  Parent.
                            }
                            else
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource2.ContainsKey(DragNode.Name))  //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource2.Remove(DragNode.Name);
                                }
                                if (DragNode.Name.Contains(DragNode.Text))
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')));
                                else
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')) + "." + DragNode.Text.Split(new string[2] { "->", "<-" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                ((BaseFunction)selectNode.Tag).RefSource2.Add(DragNode.Name, (IFunction)DragNode.Tag);  //Parent. Parent.
                            }
                            selectNode.Expand();
                            break;
                        case "RefSource3":
                            // 获取拖动对象的对象类型
                            sourceDataType = DragNode.Tag.GetType().GetProperty(DragNode.Name.Split('.').Last())?.GetValue(DragNode.Tag)?.GetType().Name;
                            ///////////////////// 获取接爱数据的对象类型
                            propertyInfos = selectNode.Tag?.GetType().GetProperties();  //Parent.
                            foreach (var item in propertyInfos)
                            {
                                attributes = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;  // 获取属性的描述特性
                                if (attributes != null && attributes.Description == "输入属性3")
                                {
                                    targetDataType = item.GetValue(selectNode.Tag)?.GetType().Name;  //Parent.
                                    break;
                                }
                            }
                            ///////////////////////////////////////////////
                            if (DragNode.Parent == null)
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource3.ContainsKey(DragNode.Name))  //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource3.Remove(DragNode.Name);
                                }
                                selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')));
                                ((BaseFunction)selectNode.Tag).RefSource3.Add(DragNode.Name, (IFunction)DragNode.Tag);// 这里要添加拖动过来的对象  Parent.
                            }
                            else
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource3.ContainsKey(DragNode.Name))  //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource3.Remove(DragNode.Name);
                                }
                                if (DragNode.Name.Contains(DragNode.Text))
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')));
                                else
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')) + "." + DragNode.Text.Split(new string[2] { "->", "<-" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                ((BaseFunction)selectNode.Tag).RefSource3.Add(DragNode.Name, (IFunction)DragNode.Tag);  //Parent. Parent.
                            }
                            selectNode.Expand();
                            break;
                        case "RefSource4":
                            // 获取拖动对象的对象类型
                            sourceDataType = DragNode.Tag.GetType().GetProperty(DragNode.Name.Split('.').Last())?.GetValue(DragNode.Tag)?.GetType().Name;
                            ///////////////////// 获取接爱数据的对象类型
                            propertyInfos = selectNode.Tag.GetType().GetProperties();  //Parent.
                            foreach (var item in propertyInfos)
                            {
                                attributes = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;  // 获取属性的描述特性
                                if (attributes != null && attributes.Description == "输入属性4")
                                {
                                    targetDataType = item.GetValue(selectNode.Tag)?.GetType().Name;  //Parent.
                                    break;
                                }
                            }
                            ///////////////////////////////////////////////
                            if (DragNode.Parent == null)
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource4.ContainsKey(DragNode.Name))  //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource4.Remove(DragNode.Name);
                                }
                                selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')));
                                ((BaseFunction)selectNode.Tag).RefSource4.Add(DragNode.Name, (IFunction)DragNode.Tag);//   Parent.
                            }
                            else
                            {
                                if (((BaseFunction)selectNode.Tag).RefSource4.ContainsKey(DragNode.Name))  //Parent.
                                {
                                    if (selectNode.Nodes.Find(DragNode.Name, true).Length > 0)
                                    {
                                        MessageBox.Show("目标对象中已包含了一个相同的源对象");
                                        return;
                                    }
                                    else
                                        ((BaseFunction)selectNode.Tag).RefSource4.Remove(DragNode.Name);
                                }
                                if (DragNode.Name.Contains(DragNode.Text))
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')));
                                else
                                    selectNode.Nodes.Add(DragNode.Name, DragNode.Name.Substring(0, DragNode.Name.LastIndexOf('.')) + "." + DragNode.Text.Split(new string[2] { "->", "<-" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                ((BaseFunction)selectNode.Tag).RefSource4.Add(DragNode.Name, (IFunction)DragNode.Tag);  //Parent.  Parent.
                            }
                            selectNode.Expand();
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("拖动控件报错", ex);
            }
        }
        private void treeView1_DragEnter(object sender, DragEventArgs e) // 这一步必需设置拖放效果，不然拖放完成事件不触发
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                TreeNode node = (TreeNode)e.Item;
                string name = node.Text;
                if (node.Text.Split('(')[0] == "传感器") return;
                if (node.Tag is IFunction || node.Parent?.Tag is IFunction) // 只有当拖放对象是 IFunction类型时，才很高行拖放操作
                    this.treeView.DoDragDrop(e.Item, DragDropEffects.Copy);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("执行拖动报错", ex);
            }
        }
        #endregion

        protected void OnEdite(TreeViewEventArgs e)
        {
            if (this.Edite != null)
                this.Edite(this, e);
        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            for (int i = 0; i < this.treeView.Nodes.Count; i++)
            {
                this.treeView.Nodes[i].ForeColor = Color.Black;
            }
            if (e.Node.Checked && e.Node.Tag is AcqSource)
                e.Node.ForeColor = Color.Blue;
            else
                e.Node.ForeColor = Color.Black;
        }

        // 鼠标单击节点时的事件处理方法,用于显示当前选定对象的内容
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.treeView.SelectedNode = e.Node; // 让选择节点出现在点击节点之前，这样一来在选择的更新上就不会出现跳变现象了
                this.OnClickNode(e.Node, e.Button, e.Clicks, e.X, e.Y);
            }
        }

        public void treeView1_Edite(object sender, TreeNode node)
        {
            if (node == null) return;
            string name = node.Text;
            if (form != null)
            {
                form.Close(); // 保证每次只有一个窗体显示 acqSourceForm
                form = null;
            }
            if (acqSourceForm != null)
            {
                acqSourceForm.Close(); // 保证每次只有一个窗体显示 acqSourceForm
                acqSourceForm = null;
            }
            ///////////////////////////////////////////////       
            string aa = name.Split('(', '=')[0];
            if (node.Tag == null) return;
            switch (node.Tag.GetType().Name)
            {
                case "采集源":
                case nameof(AcqSource):
                    acqSourceForm = new AcqSourceForm(AcqSourceManage.Instance.GetAcqSource(node.Text.Split('=')[1]));     //(AcqSource)node.Tag
                    acqSourceForm.Owner = this.parentForm;
                    break;

                case "激光点":
                case nameof(LaserPointAcq):
                    form = new LaserPointAcqForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "点云采集":
                case nameof(PointCloudAcq):
                    form = new PointCloudAcqForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "激光线":
                case nameof(LaserLineScanAcq):
                    form = new LaserLineScanForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "矩形扫描":
                case nameof(RectangleScanAcq):
                    form = new RectangleScanForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "圆弧扫描":
                case nameof(SpiralScanAcq):
                    form = new SpiralScanForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "路径扫描":
                case nameof(LaserScanAcqPath):
                    form = new LaserLineScanPathForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "激光点2":
                case nameof(LaserPointAcqStandard):
                    form = new LaserPointAcqStandardForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "激光线2":
                    form = new LaserLineScanStandardForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "创建位姿3D":
                case nameof(CreatePose3D):
                    form = new Create3DPoseForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "翻转对象3D":
                case nameof(OverturnObjectModel3D):
                    form = new OverturnObjectModel3DForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "读取对象3D":
                case nameof(ReadObjectModel3D):
                    form = new ReadData3DForm((IFunction)node.Tag);
                    form.Owner = this.parentForm;
                    break;

                case "保存对象3D":
                case nameof(SaveObjectModel3D):
                    form = new Save3dDataForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "平面度":
                case nameof(Planeness):
                    form = new PlanenessForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "体积":
                case nameof(Volume):
                    form = new VolumeForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "厚度":
                case nameof(Thickness):
                    form = new ThicknessForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "提取截面3D":
                case nameof(Section):
                    form = new SectionForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "点到面距离3D":
                case nameof(PointToFaceDist):
                    form = new PointToFaceDist3DForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "点到线距离3D":
                case nameof(PointToLineDist3D):
                    form = new PointToLineDist3DForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "点到点距离3D":
                case nameof(PointToPointDist3D):
                    form = new PointToPointDist3DForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "线到线距离3D":
                case nameof(LineToLineDist3D):
                    form = new LineToLineDist3DForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "面到面距离3D":
                case nameof(FaceToFaceDist3D):
                    form = new FaceToFaceDist3DForm((IFunction)node.Tag, node); //选择3D对象范围  创建3D基本体
                    form.Owner = this.parentForm;
                    break;

                case "定点采样对象3D":
                case nameof(FixedPointSampleModel):
                    form = new FixedPointSampleModelForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "采样对象3D":
                case nameof(SampleObjectModel3D):
                    form = new SampleObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "选择对象3D":
                case nameof(SelectObjectModelRange3D):
                    form = new SelectionRangeObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "滤波对象3D":
                case nameof(FilterObjectModel3D):
                    form = new FilterObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "分割对象3D":
                case nameof(SegmentObjectModle3D):
                    form = new SegmentObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "合并对象3D":
                case nameof(Union3DobjectModel):
                    form = new UnionObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "平滑3D对象":
                case nameof(SmoothObjectModel3D):
                    form = new SmoothObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "平滑轮廓对象3D":
                case nameof(SmoothProfileModel3D):
                    form = new SmoothProfileObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "变换对象3D":
                case nameof(AffineObjectModel3D):
                    form = new AffineObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "缩放对象3D":
                case nameof(ScaleObjectModel3D):
                    form = new ScaleObjectModel3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "转换对象到图像3D":
                case nameof(TransformObject3DToXYZImage):
                    form = new TransformObject3DToImageForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "转换图像到对象3D":
                case nameof(TransformXYZImageToObject3D):
                    form = new TransformXYZImageToObject3DForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "图像采集":
                case nameof(ImageAcq):
                    form = new ImageAcqForm(node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "开始采集":
                case nameof(StartAcq):
                    form = new StartAcqForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;
                case "停止采集":
                case nameof(StopAcq):
                    form = new StopAcqForm((IFunction)node.Tag, node); //选择3D对象范围
                    form.Owner = this.parentForm;
                    break;

                case "角平分线":
                case nameof(CalculateAngleBisector):
                    form = new CalculateAngleBisectorForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "中分线":
                case nameof(CalculateMiddleBisector):
                    form = new CalculateMiddleBisectorForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "点点距离":
                case nameof(PointToPointDist2D):
                    form = new PointToPointDist2DForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "线线距离":
                case nameof(LineToLineDist2D):
                    form = new LineToLineDist2DForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "点线距离":
                case nameof(PointToLineDist2D):
                    form = new PointToLineDist2DForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "圆线距离":
                case nameof(CircleToLineDist2D):
                    form = new CircleToLineDist2DForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "圆圆距离":
                case nameof(CircleToCircleDist2D):
                    form = new CircleToCircleDist2DForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "直线中点":
                case nameof(LineMiddlePoint):
                    form = new LineMiddlePointForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "N点到直线":
                case nameof(NPointToLineDist2D):
                    form = new NPointToLineDist2DForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "轮廓到线距离":
                case nameof(ContToLineDist2D):
                    form = new ContToLineDist2DForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "Blob分析":
                case nameof(Blob):
                    form = new BlobForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "读取图像":
                case nameof(ReadImage):
                    form = new ReadImageForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;


                //case "裁剪图像":
                //case nameof(ImageReduce):
                //    form = new ImageReduceForm((IFunction)node.Tag, node);
                //    form.Owner = this.parentForm;
                //    break;

                case "提取轮廓":
                case nameof(ExtractXLD):
                    form = new ExtractContourXLDForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "选择轮廓":
                case nameof(SelectContoursXld):
                    form = new SelectContourXLDForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "变换轮廓":
                case nameof(AffineXLDCont2D):
                    form = new AffineXLDContForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "保存轮廓":
                case nameof(SaveContXLD):
                    form = new SaveXLDForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "读取轮廓":
                case nameof(ReadContourXLD):
                    form = new ReadXLDForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "采样轮廓":
                case nameof(SampleXLD):
                    form = new SampleXldForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "取反平移对象3D":
                case nameof(InvertTranslateObjectModel3D):
                    form = new InvertTranslateObject3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "坐标系-表面匹配":
                case nameof(SurfaceMatch):
                    form = new SurfaceModel3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "圆形取点":
                case nameof(CircleCrop):
                    form = new CircleCropForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "创建基本体3D":
                case nameof(CreatePrimitive3D):
                    form = new CreatePrimitive3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;


                case "保存图像":
                case nameof(SaveImage):
                    form = new SaveImageForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "校平对象3D":
                case nameof(RectifyObjectModel3D):
                    form = new RectifyObjectModel3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "裁剪对象3D":
                case nameof(CropObjectModel3D):
                    form = new CropObjectModel3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "平面相交对象3D":
                case nameof(IntersectPlaneObjectModel3D):
                    form = new IntersectPlaneObjectModel3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "变换位姿3D":
                case nameof(AffinePose3D):
                    form = new Affine3DPoseForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "获取极值轮廓":
                case nameof(GetMinMaxValueObjectModel):
                    form = new GetMinMaxValueObjectModelForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "线线交点":
                case nameof(IntersectionLineLine):
                    form = new IntersectionLineLineForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "线圆交点":
                case nameof(IntersectionLineCirlce):
                    form = new IntersectionLineCircleForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "圆圆交点":
                case nameof(IntersectionCircleCircle):
                    form = new IntersectionCircleCircleForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "显示多个对象3D":
                case nameof(ShowMultipleObjects):
                    form = new ShowMultipleObjectsForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "直线夹角3D":
                case nameof(AngleLineLine3D):
                    form = new AngleLineLine3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "直线方向3D":
                case nameof(LineOrientation3D):
                    form = new LineOrientationForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "直线长度3D":
                case nameof(LineLength3D):
                    form = new LineLength3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "直线度3D":
                case nameof(Straightness3D):
                    form = new Straightness3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "线轮廓度3D":
                case nameof(ProfileTolerance):
                    form = new ProfileTolerance3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "渲染对象到图像3D":
                case nameof(RenderObjectModel3DToImage):
                    form = new RenderObjectModel3DToImageForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "点":
                case nameof(PointMeasure):
                    form = new PointMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "十字取点":
                case nameof(CrossPointMeasure):
                    form = new CrossPointMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "线":
                case nameof(LineMeasure):
                    form = new LineMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "圆":
                case nameof(CircleMeasure):
                    form = new CircleMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "圆弧":
                case nameof(CircleSectorMeasure):
                    form = new CircleSectorMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "椭圆":
                case nameof(EllipseMeasure):
                    form = new EllipseMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "椭圆弧":
                case nameof(EllipseSectorMeasure):
                    form = new EllipseSectorMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "矩形":
                case nameof(Rectangle2Measure):
                    form = new Rect2MeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "宽度":
                case nameof(WidthMeasure):
                    form = new WidthMeasureForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "坐标系_点线":
                case nameof(CoordSystemPointLine):
                    form = new CoordSystemPointLineForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "坐标系_线":
                case nameof(CoordSystemLine):
                    form = new CoordSystemLineForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "坐标系_线线":
                case nameof(CoordSystemLineLine):
                    form = new CoordSystemLineLineForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "坐标系_圆线":
                case nameof(CoordSystemCircleLine):
                    form = new CoordSystemCircleLineForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break; //
                case "坐标系_矩形":
                case nameof(CoordSystemRect2):
                    form = new CoordSystemRect2Form((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break; //

                case "拟合平面3D":
                case nameof(FittingPlane3D):
                    form = new FittingPlane3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "拟合圆柱3D":
                case nameof(FittingCylinder3D):
                    form = new FittingCylinder3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "拟合球体3D":
                case nameof(FittingSphere3D):
                    form = new FittingSphere3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "拟合盒子3D":
                case nameof(FittingBox3D):
                    form = new FittingBox3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "拟合轮廓圆3D":
                case nameof(FitProfileCircle3D):
                    form = new FittingProfileCircle3DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                #region 2D拟合
                case "拟合直线":
                case nameof(FitLine):
                    form = new FitLineForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "N点拟合直线":
                case nameof(NPointsFitLine):
                    form = new NPointFitLineForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "拟合圆":
                case nameof(FitCircle):
                    form = new FitCircleForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;
                case "N点拟合圆":
                case nameof(NPointsFitCircle):
                    form = new NPointsFitCircleForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "拟合椭圆":
                case nameof(FitEllipse):
                    form = new FitEllipseForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "N点拟合椭圆":
                case nameof(NPointsFitEllipse):
                    form = new NPointFitEllipseForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                case "拟合矩形":
                case nameof(FitRect2):
                    form = new FitRect2Form((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                #endregion

                case "图像平铺":
                case nameof(TileImage):
                    form = new TileImageForm((IFunction)node.Tag, node); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "面面夹角":
                case nameof(FaceToFaceAngle):
                    form = new FaceToFaceAngleForm((IFunction)node.Tag, node); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "线线夹角":
                case "线线夹角2D":
                case nameof(AngleLineLine2D):
                    form = new AngleLineLine2DForm((IFunction)node.Tag, node); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "数值计算":
                case nameof(NumericalCalculation):
                    form = new NumericalCalculationForm((IFunction)node.Tag, node); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "极值点3D":
                case nameof(ExtremePoint3D):
                    form = new ExtremePointForm3D((IFunction)node.Tag, node); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "点位运动":
                case nameof(PointMove):
                    form = new PointMoveForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "直线插补":
                case nameof(LineInterpolationMove):
                    form = new LineInterpolationMoveForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "螺旋线插补":
                case nameof(SpiralLineInterpolationMove):
                    form = new SpiralLineInterpolationMoveForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "矩形插补":
                case nameof(RectangleInterpolationMove):
                    form = new RectangleInterpolationMoveForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "轨迹运动":
                case nameof(TrackMove):
                    form = new TrackMoveForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "写入IO输出位控制":
                case nameof(WriteIoOutputBit):
                    form = new WriteIoOutputBitForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "写入IO输出组控制":
                case nameof(WriteIoOutputGroup):
                    form = new WriteIoOutputGroupForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "读取IO输入位控制":
                case nameof(ReadIoInputBit):
                    form = new ReadIoInputBitForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "读取IO输出位控制":
                case nameof(ReadIoOutputBit):
                    form = new ReadIoOutputBitForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "读取IO输出组控制":
                case nameof(ReadIoOutputGroup):
                    form = new ReadIoOutputGroupForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "读取IO输入组控制":
                case nameof(ReadIoIntputGroup):
                    form = new ReadIoIntputGroupForm((IFunction)node.Tag); //选择3D对象范围  渲染3D对象到图像 转换像素坐标到世界坐标
                    form.Owner = this.parentForm;
                    break;

                case "直线偏置":
                case nameof(LineOffset):
                    form = new LineOffsetForm(node);
                    form.Owner = this.parentForm;
                    break;

                case "直线延伸":
                case nameof(LineExtend):
                    form = new LineExtendForm(node);
                    form.Owner = this.parentForm;
                    break;

                case "投影点":
                case nameof(ProjectionPoint):
                    form = new ProjectionPointForm((IFunction)node.Tag, node); //选择3D对象范围  坐标系-点线
                    form.Owner = this.parentForm;
                    break;

                case "直线截取":
                case nameof(LineCrop):
                    form = new LineCropForm((IFunction)node.Tag);
                    form.Owner = this.parentForm;
                    break;

                case "对射校准":
                case nameof(TroughCalibrate):
                    form = new TroughCalibrateForm(node);
                    form.Owner = this.parentForm;
                    break;

                case "获取标定Mark点":
                case nameof(GetCalibrateMarkPoint):
                    form = new GetCalibrateMarkPointForm((IFunction)node.Tag);
                    form.Owner = this.parentForm;
                    break;

                case "二维码识别":
                case nameof(DataCodeDetection):
                    form = new DataCodeForm(node);
                    form.Owner = this.parentForm;
                    break;

                case "条形码识别":
                case nameof(BarCodeDetection):
                    form = new BarCodeForm(node);
                    form.Owner = this.parentForm;
                    break;

                //case "读取PLC轴位置":  
                //case nameof(DataRead):
                //    form = new DataReadForm((IFunction)node.Tag);
                //    form.Owner = this.parentForm;
                //    break;

                //case "写入PLC轴位置":
                //case nameof(DataWrite):
                //    form = new DataWriteForm((IFunction)node.Tag);
                //    form.Owner = this.parentForm;
                //    break;

                case "光源配置":
                case nameof(LightConfig):
                    form = new LightConfigForm((IFunction)node.Tag);
                    form.Owner = this.parentForm;
                    break;


                #region  贴合对位功能
                case "参考位示教":
                case nameof(ReferenceTeach):
                    form = new ReferenceTeachForm(node); //以视野的中心定为参考点
                    form.Owner = this.parentForm;
                    break;  // 
                case "特征定位":
                case nameof(FeatureLocalization):
                    form = new FeatureLocalizationForm(node);
                    form.Owner = this.parentForm;
                    break;  // 
                case "胶路检测":
                case nameof(GlueDetect):
                    form = new GlueDetectForm(node);
                    form.Owner = this.parentForm;
                    break;  // 
                case "并发执行":
                case nameof(ConcurrentExecution):
                    form = new ConcurrentExecutionForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "计算偏移":
                case nameof(CalculateOffsetValue):
                    form = new CalculateOffsetValueForm(node);
                    form.Owner = this.parentForm;
                    break;
                case "对齐计算":
                case nameof(AlignmentGuidedCalculation):
                    form = new AlignmentGuidedCalculationForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "胶枪引导":
                case nameof(GlueAlignGuided):
                    form = new GlueAlignGuidedForm(node);
                    form.Owner = this.parentForm;
                    break;

                case "提取点":
                case nameof(ExtractPoint):
                    form = new ExtractPointForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "数据读取":
                case nameof(DataRead):
                    form = new DataReadForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "数据写入":
                case nameof(DataWrite):
                    form = new DataWriteForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "数据等待":
                case nameof(DataWaitePlc):
                    form = new DataWaiteForm(node);
                    form.Owner = this.parentForm;
                    break;  //  

                case "缺陷检测":
                case nameof(DefectDetecting):
                    form = new DefectDetectingForm(node);
                    form.Owner = this.parentForm;
                    break;  // StatisticsResult

                //case "对位引导":
                //case nameof(AlignmentGuided):
                //    form = new AlignmentGuidedForm(node);
                //    form.Owner = this.parentForm;
                //    break;  // 

                case "读取文件":
                case nameof(ReadFile):
                    form = new ReadFileForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "写入文件":
                case nameof(WriteFile):
                    form = new WriteFileForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "坐标映射":
                case nameof(CoordMap):
                    form = new CoordMapForm(node);
                    form.Owner = this.parentForm;
                    break;  // 

                case "坐标变换":
                case nameof(CoordAffine):
                    form = new CoordAffineForm(node);
                    form.Owner = this.parentForm;
                    break;  //  

                case "阵列测量":
                case nameof(MeasureArray):
                    form = new MeasureArrayForm(node);
                    form.Owner = this.parentForm;
                    break;  //  

                case "流程单元":
                case nameof(JobUnit):
                    form = new JobUnitForm(node);
                    form.Owner = this.parentForm;
                    break;  //  

                case "对位计算":
                case nameof(AlignCalculate):
                    form = new AlignCalculateForm(node);
                    form.Owner = this.parentForm;
                    break;  //  

                case "偏差计算":
                case nameof(OffsetCalculate):
                    form = new OffsetCalculateForm(node);
                    form.Owner = this.parentForm;
                    break;  //
                            // 
                #endregion

                #region  控制
                case "循环控制":
                case nameof(ForLoopControl):
                    form = new ForLoopControlForm(node);
                    form.Owner = this.parentForm;
                    break;  // 
                #endregion


                #region  2D匹配

                case "局部变形匹配":
                case nameof(LocalDeformableModelMatch):
                    form = new LocalDeformableMatchForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;  // 
                case "NCC匹配":
                case nameof(NccModelMatch):
                    form = new NccMatchForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;  //

                case "形状匹配":
                case "形状匹配2D":
                case "坐标系-形状匹配":
                case "坐标系-2D形状匹配":
                case "形状模型匹配":
                case "坐标系-形状模型匹配":
                case "坐标系-2D形状模型匹配":
                case nameof(ShapeModelMatch2D):
                    form = new ShapeModelMatch2DForm((IFunction)node.Tag, node); //
                    form.Owner = this.parentForm;
                    break;

                #endregion

                #region 区域拟合
                case "拟合矩形区域":
                case nameof(FitRect2Region):
                    form = new FitRect2RegionForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "区域运算":
                case nameof(RegionArithmetic):
                    form = new RegionArithmeticForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                #endregion

                #region 图像操作
                case nameof(Ocr):
                case "字符识别":
                    form = new OcrDetectionForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "裁剪图像":
                case "图像裁剪":
                case nameof(ImageReduce):
                    form = new ImageReduceForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "图像处理":
                case nameof(ImageMorphology):
                    form = new ImageMorphologyForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break;

                case "图像滤波":
                case nameof(ImageFilter):
                    form = new ImageFilterForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break; //

                case "图像增强":
                case nameof(ImageEnhancement):
                    form = new ImageEnhancementForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break; //

                case "图像变换":
                case nameof(ImageAffine):
                    form = new ImageAffineForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break; //

                case "图像旋转":
                case nameof(ImageRotate):
                    form = new ImageRotateForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break; //

                case "窗口图像":
                case nameof(GetHWindowImage):
                    form = new GetHWindowImageForm(node);
                    form.Owner = this.parentForm;
                    break;

                case "图像运算":
                case nameof(ImageArithmetic):
                    form = new ImageArithmeticForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break; // 

                case "图像缩放":
                case nameof(ImageZoom):
                    form = new ImageZoomForm((IFunction)node.Tag, node);
                    form.Owner = this.parentForm;
                    break; // 图像缩放

                #endregion

                #region PLC操作
                case "读取数据PLC":
                case nameof(ReadPlcData):
                    form = new ReadPlcDataForm(node);
                    form.Owner = this.parentForm;
                    break; //

                case "写入数据PLC":
                case nameof(WritePlcData):
                    form = new WritePlcDataForm(node);
                    form.Owner = this.parentForm;
                    break; //

                case "保存数据PLC":
                case nameof(SaveDataPlc):
                    form = new SaveDataPlcForm(node);
                    form.Owner = this.parentForm;
                    break; //

                case "保存数据Ftp":
                case nameof(SaveDataFtp):
                    form = new SaveDataFtpForm(node);
                    form.Owner = this.parentForm;
                    break; //

                case "写入数据Socket":
                case nameof(WriteDataSocket):
                    form = new WriteDataSocketForm(node);
                    form.Owner = this.parentForm;
                    break; //

                case "发送结果Socket":
                case nameof(SendResultSocket):
                    form = new SendResultSocketForm(node);
                    form.Owner = this.parentForm;
                    break; // 

                case "保存数据":
                case nameof(SaveDataNew):
                    form = new SaveDataNewForm(node);
                    form.Owner = this.parentForm;
                    break; // 

                case "延时等待":
                case nameof(Waite):
                    form = new WaiteForm(node);
                    form.Owner = this.parentForm;
                    break; // 

                case "发送数据":
                case nameof(SendData):
                    form = new SendDataForm(node);
                    form.Owner = this.parentForm;
                    break; // 

                case "写入数据":
                case nameof(WriteData):
                    form = new WriteDataForm(node);
                    form.Owner = this.parentForm;
                    break; // 

                case "读取数据":
                case nameof(ReadData):
                    form = new ReadDataForm(node);
                    form.Owner = this.parentForm;
                    break; // 等待数据

                case "等待信号":
                case nameof(WaiteSignal):
                    form = new WaiteSignalForm(node);
                    form.Owner = this.parentForm;
                    break; // 等待数据

                case "结果判断":
                case nameof(ResultJudge):
                    form = new ResultJudgeForm(node);
                    form.Owner = this.parentForm;
                    break;

                #endregion

                case "数据标签":
                case nameof(DataLable):
                    form = new DataLableForm(node);
                    form.Owner = this.parentForm;
                    break; //    



                #region 缺陷检测

                case "直线检测":
                case "边缘检测":
                case nameof(LineDetect):
                    form = new LineDetectForm(node);
                    form.Owner = this.parentForm;
                    break; //   
                case "破片检测":
                case nameof(GapDetect):
                    form = new GapDetectForm(node);
                    form.Owner = this.parentForm;
                    break; //   
                case "脚本工具":
                case nameof(ScriptTool):
                    form = new ScriptToolForm(node);
                    form.Owner = this.parentForm;
                    break; //  

                case "测量检测":
                case nameof(MeasureDetect):
                    form = new MeasureDetectForm(node);
                    form.Owner = this.parentForm;
                    break; // 

                #endregion


                #region 引导变换
                case "引导计算":
                case nameof(GuidedCalculate):
                    form = new GuidedCalculateForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                case "引导标定":
                case nameof(GuidedCalib):
                    form = new GuidedCalibForm(node);
                    form.Owner = this.parentForm;
                    break;
                case "点引导变换":
                case nameof(PointAffine):
                    form = new GuidedCalculateForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                case "线引导变换":
                case nameof(LineAffine):
                    form = new GuidedCalculateForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                case "圆引导变换":
                case nameof(CircleAffine):
                    form = new GuidedCalculateForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                case "椭圆引导变换":
                case nameof(EllpiseAffine):
                    form = new GuidedCalculateForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                case "矩形引导变换":
                case nameof(Rect2Affine):
                    form = new GuidedCalculateForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                case "轨迹变换":
                case nameof(TrackAffine):
                    form = new TrackAffineForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                case "轨迹计算":
                case nameof(TrackCalculate):
                    form = new TrackCalculateForm(node);
                    form.Owner = this.parentForm;
                    break; //  
                #endregion

                #region 上下料
                case "下料穴位":
                case nameof(RobotLayOff):
                    form = new RobotLayOffForm(node);
                    form.Owner = this.parentForm;
                    break; // 
                case "下料Try盘":
                case nameof(LayOffTryPlate):
                    form = new LayOffTryPlateForm(node);
                    form.Owner = this.parentForm;
                    break; // 
                case "上料Try盘":
                case nameof(LoadTryPlate):
                    form = new LoadTryPlateForm(node);
                    form.Owner = this.parentForm;
                    break; // 
                case "上料穴位":
                case nameof(RobotLoad):
                    form = new RobotLoadForm(node);
                    form.Owner = this.parentForm;
                    break; // 
                #endregion

                #region 标定
                case "N点标定":
                case nameof(MapCalib):
                    form = new MapCalibForm(node);
                    form.Owner = this.parentForm;
                    break; // 
                case "旋转标定":
                case nameof(CalibRotateCenter):
                    form = new CalibRotateCenterForm(node);
                    form.Owner = this.parentForm;
                    break; // 
                #endregion

                #region 流程控制
                case "条件语句":
                case nameof(Conditional):
                    form = new ConditionalForm(node);
                    form.Owner = this.parentForm;
                    break;  //
                case "条件语句<If>":
                case nameof(ConditionaIf):
                    form = new ConditionaIfForm(node);
                    form.Owner = this.parentForm;
                    break;  //
                case "条件语句<else>":
                case nameof(ConditionaElse):
                    form = new ConditionaElseForm(node);
                    form.Owner = this.parentForm;
                    break;  //
                #endregion



                default:
                    break;
            }
            if (this.form != null)
                this.form.Show();
            if (this.acqSourceForm != null)
                this.acqSourceForm.Show();
        }

        public void Uinit()
        {
            this.treeView.ItemDrag -= new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView.DragDrop -= new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView.DragEnter -= new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView.NodeMouseClick -= new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick); // 封闭一个节点点击事件给外部调用 
        }


    }
}
