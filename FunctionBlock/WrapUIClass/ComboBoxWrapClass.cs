
using Common;
using FunctionBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public class ComboBoxWrapClass
    {
        private ComboBox comboBox;
        private IFunction _function;
        private int targetSourceIndex = 1;
        public static event ItemsChangeEventHandler ItemsChangeToForm; // 使用静态事件，那么所有对象都会触发
        public static event ItemsChangeEventHandler ItemsChangeToTreeView; // 使用静态事件，那么所有对象都会触发

        public ComboBoxWrapClass()
        {

        }

        public ComboBoxWrapClass(ComboBox comboBox, IFunction function, int targetSourceIndex = 1)
        {
            this.comboBox = comboBox;
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.AllowDrop = true;
            this.comboBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.comboBox_DragDrop);
            this.comboBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.comboBox_DragEnter);
            addContextMenu();
            SetDataSource(function, targetSourceIndex);
            LoadListItems();
        }
        public void InitComboBox(ComboBox comboBox, IFunction function, int targetSourceIndex = 1)
        {
            this.comboBox = comboBox;
            this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox.BackColor = System.Drawing.SystemColors.Window;
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.AllowDrop = true;
            this.comboBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.comboBox_DragDrop);
            this.comboBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.comboBox_DragEnter);
            addContextMenu();
            SetDataSource(function, targetSourceIndex);
            LoadListItems();
        }

        private void SetDataSource(IFunction function, int targetSourceIndex)
        {
            this._function = function;
            this.targetSourceIndex = targetSourceIndex;
        }
        private void comboBox_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            try
            {
                //object object3D;
                switch (this.targetSourceIndex)
                {
                    default:
                    case 1:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            // 先判断RefSource1对象是否包含该条目
                            if (!((BaseFunction)this._function).RefSource1.ContainsKey(node.Name))  // 这里使用Name属性好还是 Text属性好？
                            {
                                this.comboBox.Items.Add(node.Name);
                                this.comboBox.Text = node.Name;
                                if (this.comboBox.Items.Count > 1)
                                    this.comboBox.DroppedDown = true;
                                ((BaseFunction)this._function).RefSource1.Add(node.Name, (IFunction)node.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Tag, "RefSource1", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource1", node.Name)); //这里要使用当前对象 
                            }
                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource1.ContainsKey(node.Parent.Name + "." + node.Name))
                            {
                                this.comboBox.Items.Add(node.Parent.Name + "." + node.Name);
                                ((BaseFunction)this._function).RefSource1.Add(node.Parent.Name + "." + node.Name, (IFunction)node.Parent.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Parent.Tag, "RefSource1", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource1", node.Name)); //这里要使用当前对象 
                            }
                        }
                        break;
                    case 2:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            if (!((BaseFunction)this._function).RefSource2.ContainsKey(node.Name))
                            {
                                this.comboBox.Items.Add(node.Name);
                                ((BaseFunction)this._function).RefSource2.Add(node.Name, (IFunction)node.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Tag, "RefSource2", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource2", node.Name)); //这里要使用当前对象 
                            }

                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource2.ContainsKey(node.Parent.Name + "." + node.Name))
                            {
                                this.comboBox.Items.Add(node.Parent.Name + "." + node.Name);
                                ((BaseFunction)this._function).RefSource2.Add(node.Parent.Name + "." + node.Name, (IFunction)node.Parent.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Parent.Tag, "RefSource2", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource2", node.Name)); //这里要使用当前对象 
                            }
                        }
                        break;
                    case 3:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            if (!((BaseFunction)this._function).RefSource3.ContainsKey(node.Name))
                            {
                                this.comboBox.Items.Add(node.Name);
                                ((BaseFunction)this._function).RefSource3.Add(node.Name, (IFunction)node.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Tag, "RefSource3", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource3", node.Name)); //这里要使用当前对象 
                            }
                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource3.ContainsKey(node.Parent.Name + "." + node.Name))
                            {
                                this.comboBox.Items.Add(node.Parent.Name + "." + node.Name);
                                ((BaseFunction)this._function).RefSource3.Add(node.Parent.Name + "." + node.Name, (IFunction)node.Parent.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Parent.Tag, "RefSource3", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource3", node.Name)); //这里要使用当前对象 
                            }
                        }
                        break;
                    case 4:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            if (!((BaseFunction)this._function).RefSource4.ContainsKey(node.Name))
                            {
                                this.comboBox.Items.Add(node.Name);
                                ((BaseFunction)this._function).RefSource4.Add(node.Name, (IFunction)node.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Tag, "RefSource4", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource4", node.Name)); //这里要使用当前对象 
                            }

                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource4.ContainsKey(node.Parent.Name + "." + node.Name))
                            {
                                this.comboBox.Items.Add(node.Parent.Name + "." + node.Name);
                                ((BaseFunction)this._function).RefSource4.Add(node.Parent.Name + "." + node.Name, (IFunction)node.Parent.Tag);
                                ItemsChangeToForm?.Invoke(this.comboBox, new ItemsChangeEventArgs((IFunction)node.Parent.Tag, "RefSource4", node.Name)); //这里要使用当前对象 
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource4", node.Name)); //这里要使用当前对象 
                            }
                        }
                        break;
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }
        private void comboBox_DragEnter(object sender, DragEventArgs e)
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
            this.comboBox.ContextMenuStrip = ContextMenuStrip1;
        }
        private void listBoxContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                switch (name)
                {
                    case "删除":
                        switch (this.targetSourceIndex)
                        {
                            default:
                            case 1:
                                if (this.comboBox.SelectedItem != null)
                                {
                                    if (((BaseFunction)this._function).RefSource1.ContainsKey(this.comboBox.SelectedItem.ToString()))
                                    {
                                        ItemsChangeToForm.Invoke(this.comboBox, new ItemsChangeEventArgs(((BaseFunction)this._function).RefSource1[this.comboBox.SelectedItem.ToString()], "RefSource1", this.comboBox.SelectedItem.ToString()));
                                        ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource1", this.comboBox.SelectedItem.ToString()));
                                        ((BaseFunction)this._function).RefSource1.Remove(this.comboBox.SelectedItem.ToString());
                                        this.comboBox.Items.Remove(this.comboBox.SelectedItem);
                                    }
                                }
                                break;
                            case 2:
                                if (this.comboBox.SelectedItem != null)
                                {
                                    if (((BaseFunction)this._function).RefSource2.ContainsKey(this.comboBox.SelectedItem.ToString()))
                                    {
                                        ItemsChangeToForm.Invoke(this.comboBox, new ItemsChangeEventArgs(((BaseFunction)this._function).RefSource2[this.comboBox.SelectedItem.ToString()], "RefSource2", this.comboBox.SelectedItem.ToString()));
                                        ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource2", this.comboBox.SelectedItem.ToString()));
                                        ((BaseFunction)this._function).RefSource2.Remove(this.comboBox.SelectedItem.ToString());
                                        this.comboBox.Items.Remove(this.comboBox.SelectedItem);
                                    }
                                }
                                break;
                            case 3:
                                if (this.comboBox.SelectedItem != null)
                                {
                                    if (((BaseFunction)this._function).RefSource3.ContainsKey(this.comboBox.SelectedItem.ToString()))
                                    {
                                        ItemsChangeToForm.Invoke(this.comboBox, new ItemsChangeEventArgs(((BaseFunction)this._function).RefSource3[this.comboBox.SelectedItem.ToString()], "RefSource3", this.comboBox.SelectedItem.ToString()));
                                        ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource3", this.comboBox.SelectedItem.ToString()));
                                        ((BaseFunction)this._function).RefSource3.Remove(this.comboBox.SelectedItem.ToString());
                                        this.comboBox.Items.Remove(this.comboBox.SelectedItem);
                                    }
                                }
                                break;
                            case 4:
                                if (this.comboBox.SelectedItem != null)
                                {
                                    if (((BaseFunction)this._function).RefSource4.ContainsKey(this.comboBox.SelectedItem.ToString()))
                                    {
                                        ItemsChangeToForm.Invoke(this.comboBox, new ItemsChangeEventArgs(((BaseFunction)this._function).RefSource4[this.comboBox.SelectedItem.ToString()], "RefSource4", this.comboBox.SelectedItem.ToString()));
                                        ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource4", this.comboBox.SelectedItem.ToString()));
                                        ((BaseFunction)this._function).RefSource4.Remove(this.comboBox.SelectedItem.ToString());
                                        this.comboBox.Items.Remove(this.comboBox.SelectedItem);
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
                                ((BaseFunction)this._function).RefSource1.Clear();
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource1"));
                                break;
                            case 2:
                                ((BaseFunction)this._function).RefSource2.Clear();
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource2"));
                                break;
                            case 3:
                                ((BaseFunction)this._function).RefSource3.Clear();
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource3"));
                                break;
                            case 4:
                                ((BaseFunction)this._function).RefSource4.Clear();
                                ItemsChangeToTreeView?.Invoke(this.comboBox, new ItemsChangeEventArgs(this._function, "RefSource4"));
                                break;
                        }
                        this.comboBox.Items.Clear();
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
            switch (this.targetSourceIndex)
            {
                default:
                case 1:
                    foreach (var item in ((BaseFunction)this._function).RefSource1.Keys)
                    {
                        this.comboBox.Items.Add(item);
                    }                   
                    break;
                case 2:
                    foreach (var item in ((BaseFunction)this._function).RefSource2.Keys)
                    {
                        this.comboBox.Items.Add(item);
                    }
                    break;
                case 3:
                    foreach (var item in ((BaseFunction)this._function).RefSource3.Keys)
                    {
                        this.comboBox.Items.Add(item);
                    }
                    break;
                case 4:
                    foreach (var item in ((BaseFunction)this._function).RefSource4.Keys)
                    {
                        this.comboBox.Items.Add(item);
                    }
                    break;
            }
        }





    }
}
