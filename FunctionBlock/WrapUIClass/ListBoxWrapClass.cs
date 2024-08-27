using Common;
using FunctionBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public class ListBoxWrapClass
    {
        private TreeNode treeNode;
        private ListBox listBox;
        private IFunction _function;
        private int targetSourceIndex = 1;
        public static event ItemsChangeEventHandler ItemsChangeToForm; // 给窗体订阅的事件，需要传递拖动过来的对象
        public static event ItemsChangeEventHandler ItemsChangeToTreeView; // 给树视图订阅的事件需要传递当前接受拖动对象的对象

        public ListBoxWrapClass()
        {

        }

        public void InitListBox(ListBox listBox, TreeNode node, int targetSourceIndex = 1)
        {
            this.listBox = listBox;
            this.listBox.BackColor = System.Drawing.SystemColors.Window;
            this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox.AllowDrop = true;
            this.listBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            this.treeNode = node;
            this.targetSourceIndex = targetSourceIndex;
            addContextMenu();
            LoadListItems();
        }
        public ListBoxWrapClass(ListBox listBox, IFunction function, int targetSourceIndex = 1)
        {
            this.listBox = listBox;
            this.listBox.BackColor = System.Drawing.SystemColors.Window;
            this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox.AllowDrop = true;
            this.listBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            addContextMenu();
            SetDataSource(function, targetSourceIndex);
            LoadListItems();
        }

        public void InitListBox(ListBox listBox, IFunction function, int targetSourceIndex = 1)
        {
            this.listBox = listBox;
            this.listBox.BackColor = System.Drawing.SystemColors.Window;
            this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox.AllowDrop = true;
            this.listBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            addContextMenu();
            SetDataSource(function, targetSourceIndex);
            LoadListItems();
        }

        private void SetDataSource(IFunction function, int targetSourceIndex)
        {
            this._function = function;
            this.targetSourceIndex = targetSourceIndex;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            try
            {
                TreeNode selectNode = null;
                string textContent = "";
                string subString = "";
                switch (this.targetSourceIndex)
                {
                    default:
                    case 1:
                        // 先判断RefSource1对象是否包含该条目
                        if (!((BaseFunction)this.treeNode.Tag).RefSource1.ContainsKey(node.Name))  // 这里使用Name属性好还是 Text属性好？
                        {
                            subString = node.Name.Substring(0, node.Name.LastIndexOf('.'));
                            if (node.Parent != null && !subString.Contains(node.Text)) // 需要判断拖动的是父节点还是子节点
                                textContent = subString + node.Text.Replace("->", ".");
                            else
                                textContent = subString;
                            this.listBox.Items.Add(textContent);
                            ((BaseFunction)this.treeNode.Tag).RefSource1.Add(node.Name, (IFunction)node.Tag);
                            if (this.treeNode?.Nodes.Find("RefSource1", true).Length > 0)
                                selectNode = this.treeNode?.Nodes.Find("RefSource1", true)[0];
                            else
                                MessageBox.Show("父节点下没有相应的输入节点1");
                            if (selectNode != null)
                            {
                                selectNode.Nodes.Add(node.Name, textContent);
                                selectNode.Expand();
                            }
                        }
                        else
                            MessageBox.Show("集合中引用了相同的对象属性");
                        break;
                    case 2:
                        // 先判断RefSource1对象是否包含该条目
                        if (!((BaseFunction)this.treeNode.Tag).RefSource2.ContainsKey(node.Name))  // 这里使用Name属性好还是 Text属性好？
                        {
                            subString = node.Name.Substring(0, node.Name.LastIndexOf('.'));
                            if (node.Parent != null && !subString.Contains(node.Text)) // 需要判断拖动的是父节点还是子节点
                                textContent = subString + node.Text.Replace("->", ".");
                            else
                                textContent = subString;
                            this.listBox.Items.Add(textContent);
                            ((BaseFunction)this.treeNode.Tag).RefSource2.Add(node.Name, (IFunction)node.Tag);
                            if (this.treeNode?.Nodes.Find("RefSource2", true).Length > 0)
                                selectNode = this.treeNode?.Nodes.Find("RefSource2", true)[0];
                            else
                                MessageBox.Show("父节点下没有相应的输入节点2");
                            if (selectNode != null)
                            {
                                selectNode.Nodes.Add(node.Name, textContent);
                                selectNode.Expand();
                            }
                        }
                        else
                            MessageBox.Show("集合中引用了相同的对象属性");
                        break;
                    case 3:
                        // 先判断RefSource1对象是否包含该条目
                        if (!((BaseFunction)this.treeNode.Tag).RefSource3.ContainsKey(node.Name))  // 这里使用Name属性好还是 Text属性好？
                        {
                            subString = node.Name.Substring(0, node.Name.LastIndexOf('.'));
                            if (node.Parent != null && !subString.Contains(node.Text)) // 需要判断拖动的是父节点还是子节点
                                textContent = subString + node.Text.Replace("->", ".");
                            else
                                textContent = subString;
                            this.listBox.Items.Add(textContent);
                            ((BaseFunction)this.treeNode.Tag).RefSource3.Add(node.Name, (IFunction)node.Tag);
                            if (this.treeNode?.Nodes.Find("RefSource3", true).Length > 0)
                                selectNode = this.treeNode?.Nodes.Find("RefSource3", true)[0];
                            else
                                MessageBox.Show("父节点下没有相应的输入节点3");
                            if (selectNode != null)
                            {
                                selectNode.Nodes.Add(node.Name, textContent);
                                selectNode.Expand();
                            }
                        }
                        else
                            MessageBox.Show("集合中引用了相同的对象属性");
                        break;
                    case 4:
                        // 先判断RefSource1对象是否包含该条目
                        if (!((BaseFunction)this.treeNode.Tag).RefSource4.ContainsKey(node.Name))  // 这里使用Name属性好还是 Text属性好？
                        {
                            subString = node.Name.Substring(0, node.Name.LastIndexOf('.'));
                            if (node.Parent != null && !subString.Contains(node.Text)) // 需要判断拖动的是父节点还是子节点
                                textContent = subString + node.Text.Replace("->", ".");
                            else
                                textContent = subString;
                            this.listBox.Items.Add(textContent);
                            ((BaseFunction)this.treeNode.Tag).RefSource4.Add(node.Name, (IFunction)node.Tag);
                            if (this.treeNode?.Nodes.Find("RefSource4", true).Length > 0)
                                selectNode = this.treeNode?.Nodes.Find("RefSource4", true)[0];
                            else
                                MessageBox.Show("父节点下没有相应的输入节点4");
                            if (selectNode != null)
                            {
                                selectNode.Nodes.Add(node.Name, textContent);
                                selectNode.Expand();
                            }
                        }
                        else
                            MessageBox.Show("集合中引用了相同的对象属性");
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }


        #region 右键菜单项
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空")
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(listBoxContextMenuStrip_ItemClicked);
            this.listBox.ContextMenuStrip = ContextMenuStrip1;
        }
        private void listBoxContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                TreeNodeCollection NodeCollection = null;
                switch (name)
                {
                    case "删除":
                        switch (this.targetSourceIndex)
                        {
                            default:
                            case 1:
                                if (this.listBox.SelectedItem != null)
                                {
                                    if (this.treeNode?.Nodes.Find("RefSource1", true).Length > 0)
                                        NodeCollection = this.treeNode?.Nodes.Find("RefSource1", true)[0]?.Nodes;
                                    else
                                    {
                                        this.listBox.Items.Remove(this.listBox.SelectedItem);
                                    }
                                    if (NodeCollection == null) return;
                                    ////////////////////////////////////////////////////////
                                    for (int i = 0; i < NodeCollection.Count; i++)
                                    {
                                        if (NodeCollection[i].Text == this.listBox.SelectedItem.ToString())
                                        {
                                            if (((BaseFunction)this.treeNode.Tag).RefSource1.ContainsKey(NodeCollection[i].Name))
                                            {
                                                ((BaseFunction)this.treeNode.Tag).RefSource1.Remove(NodeCollection[i].Name);
                                                this.listBox.Items.Remove(this.listBox.SelectedItem);
                                            }
                                            NodeCollection.Remove(NodeCollection[i]);
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (this.listBox.SelectedItem != null)
                                {
                                    if (this.treeNode?.Nodes.Find("RefSource2", true).Length > 0)
                                        NodeCollection = this.treeNode?.Nodes.Find("RefSource2", true)[0]?.Nodes;
                                    else
                                    {
                                        this.listBox.Items.Remove(this.listBox.SelectedItem);
                                    }
                                    if (NodeCollection == null) return;
                                    ////////////////////////////////////////////////////////
                                    for (int i = 0; i < NodeCollection.Count; i++)
                                    {
                                        if (NodeCollection[i].Text == this.listBox.SelectedItem.ToString())
                                        {
                                            if (((BaseFunction)this.treeNode.Tag).RefSource2.ContainsKey(NodeCollection[i].Name))
                                            {
                                                ((BaseFunction)this.treeNode.Tag).RefSource2.Remove(NodeCollection[i].Name);
                                                this.listBox.Items.Remove(this.listBox.SelectedItem);
                                            }
                                            NodeCollection.Remove(NodeCollection[i]);
                                        }
                                    }
                                }
                                break;
                            case 3:
                                if (this.listBox.SelectedItem != null)
                                {
                                    if (this.treeNode?.Nodes.Find("RefSource3", true).Length > 0)
                                        NodeCollection = this.treeNode?.Nodes.Find("RefSource3", true)[0]?.Nodes;
                                    else
                                    {
                                        this.listBox.Items.Remove(this.listBox.SelectedItem);
                                    }
                                    if (NodeCollection == null) return;
                                    ////////////////////////////////////////////////////////
                                    for (int i = 0; i < NodeCollection.Count; i++)
                                    {
                                        if (NodeCollection[i].Text == this.listBox.SelectedItem.ToString())
                                        {
                                            if (((BaseFunction)this.treeNode.Tag).RefSource3.ContainsKey(NodeCollection[i].Name))
                                            {
                                                ((BaseFunction)this.treeNode.Tag).RefSource3.Remove(NodeCollection[i].Name);
                                                this.listBox.Items.Remove(this.listBox.SelectedItem);
                                            }
                                            NodeCollection.Remove(NodeCollection[i]);
                                        }
                                    }
                                }
                                break;
                            case 4:
                                if (this.listBox.SelectedItem != null)
                                {
                                    if (this.treeNode?.Nodes.Find("RefSource4", true).Length > 0)
                                        NodeCollection = this.treeNode?.Nodes.Find("RefSource4", true)[0]?.Nodes;
                                    else
                                    {
                                        this.listBox.Items.Remove(this.listBox.SelectedItem);
                                    }
                                    if (NodeCollection == null) return;
                                    ////////////////////////////////////////////////////////
                                    for (int i = 0; i < NodeCollection.Count; i++)
                                    {
                                        if (NodeCollection[i].Text == this.listBox.SelectedItem.ToString())
                                        {
                                            if (((BaseFunction)this.treeNode.Tag).RefSource4.ContainsKey(NodeCollection[i].Name))
                                            {
                                                ((BaseFunction)this.treeNode.Tag).RefSource4.Remove(NodeCollection[i].Name);
                                                this.listBox.Items.Remove(this.listBox.SelectedItem);
                                            }
                                            NodeCollection.Remove(NodeCollection[i]);
                                        }
                                    }
                                }
                                break;
                        }

                        break;
                    //////////////////////////////////////
                    case "清空":
                        switch (this.targetSourceIndex)
                        {
                            default:
                            case 1:
                                // 窗体不响应删除操作与清空操作,窗体只响应添加操作
                                if (this.treeNode?.Nodes.Find("RefSource1", true).Length > 0)
                                {
                                    NodeCollection = this.treeNode?.Nodes.Find("RefSource1", true)[0]?.Nodes;
                                    if (NodeCollection == null) return;
                                    ((BaseFunction)this.treeNode.Tag).RefSource1.Clear();
                                    NodeCollection.Clear();
                                }
                                break;
                            case 2:
                                if (this.treeNode?.Nodes.Find("RefSource2", true).Length > 0)
                                {
                                    NodeCollection = this.treeNode?.Nodes.Find("RefSource2", true)[0]?.Nodes;
                                    if (NodeCollection == null) return;
                                    ((BaseFunction)this.treeNode.Tag).RefSource2.Clear();
                                    NodeCollection.Clear();
                                }
                                break;
                            case 3:
                                if (this.treeNode?.Nodes.Find("RefSource3", true).Length > 0)
                                {
                                    NodeCollection = this.treeNode?.Nodes.Find("RefSource3", true)[0]?.Nodes;
                                    if (NodeCollection == null) return;
                                    ((BaseFunction)this.treeNode.Tag).RefSource3.Clear();
                                    NodeCollection.Clear();
                                }
                                break;
                            case 4:
                                if (this.treeNode?.Nodes.Find("RefSource4", true).Length > 0)
                                {
                                    NodeCollection = this.treeNode?.Nodes.Find("RefSource4", true)[0]?.Nodes;
                                    if (NodeCollection == null) return;
                                    ((BaseFunction)this.treeNode.Tag).RefSource4.Clear();
                                    NodeCollection.Clear();
                                }
                                break;
                        }
                        this.listBox.Items.Clear();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
        }
        #endregion
        private void LoadListItems()
        {
            TreeNode[] findNode;
            TreeNodeCollection NodeCollection;
            switch (this.targetSourceIndex)
            {
                default:
                case 1:
                    findNode = this.treeNode?.Nodes.Find("RefSource1", true);
                    if (findNode != null && findNode.Length > 0)
                    {
                        NodeCollection = findNode[0].Nodes;
                        for (int i = 0; i < NodeCollection.Count; i++)
                        {
                            this.listBox.Items.Add(NodeCollection[i].Text);
                        }
                        if (NodeCollection.Count == 0) // 如果引用节点下没有节点，那么引用对象下应该也没有对象
                            ((BaseFunction)this.treeNode.Tag).RefSource1.Clear();
                    }
                    break;
                case 2:
                    findNode = this.treeNode?.Nodes.Find("RefSource2", true);
                    if (findNode != null && findNode.Length > 0)
                    {
                        NodeCollection = findNode[0].Nodes;
                        for (int i = 0; i < NodeCollection.Count; i++)
                        {
                            this.listBox.Items.Add(NodeCollection[i].Text);
                        }
                        if (NodeCollection.Count == 0) // 如果引用节点下没有节点，那么引用对象下应该也没有对象
                            ((BaseFunction)this.treeNode.Tag).RefSource2.Clear();
                    }
                    break;
                case 3:
                    findNode = this.treeNode?.Nodes.Find("RefSource3", true);
                    if (findNode != null && findNode.Length > 0)
                    {
                        NodeCollection = findNode[0].Nodes;
                        for (int i = 0; i < NodeCollection.Count; i++)
                        {
                            this.listBox.Items.Add(NodeCollection[i].Text);
                        }
                        if (NodeCollection.Count == 0) // 如果引用节点下没有节点，那么引用对象下应该也没有对象
                            ((BaseFunction)this.treeNode.Tag).RefSource3.Clear();
                    }
                    break;
                case 4:
                    findNode = this.treeNode?.Nodes.Find("RefSource4", true);
                    if (findNode != null && findNode.Length > 0)
                    {
                        NodeCollection = findNode[0].Nodes;
                        for (int i = 0; i < NodeCollection.Count; i++)
                        {
                            this.listBox.Items.Add(NodeCollection[i].Text);
                        }
                        if (NodeCollection.Count == 0) // 如果引用节点下没有节点，那么引用对象下应该也没有对象
                            ((BaseFunction)this.treeNode.Tag).RefSource4.Clear();
                    }
                    break;
            }
        }





    }
}
