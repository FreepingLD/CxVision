
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using MotionControlCard;
using Common;
using AlgorithmsLibrary;
using HalconDotNet;
using Sensor;
using FunctionBlock;
using View;

namespace FunctionBlock
{
    public class DataGridViewWrapClass
    {
        private Form baseForm;
        private Form form;
        private DataGridView dataGridView, dataGridView2;
        private IFunction _function;
        private int targetSourceIndex = 1;
        private DataTable dataTable, dataTable2;
        private VisualizeView drawObject;
        public DataGridViewWrapClass()
        {

        }


        public void InitDataGridView(Form baseForm, DataGridView dataGridView, DataTable dataTable)
        {
            this.baseForm = baseForm;
            this.dataTable = dataTable;
            this.dataGridView = dataGridView;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.AllowDrop = true;
            this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            this.dataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragEnter);
            addContextMenu();
            LoadListItems();
        }
        public void InitDataGridView(Form baseForm, IFunction function, DataGridView dataGridView, DataTable dataTable, VisualizeView viewClass)
        {
            this._function = function;
            this.baseForm = baseForm;
            this.dataTable = dataTable;
            this.dataGridView = dataGridView;
            this.drawObject = viewClass;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.AllowDrop = true;
            this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            this.dataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragEnter);
            addContextMenu();
            LoadListItems();
        }
        public void InitDataGridView(Form baseForm, IFunction function, DataGridView dataGridView, DataGridView dataGridView2, DataTable dataTable, DataTable dataTable2)
        {
            this._function = function;
            this.baseForm = baseForm;
            this.dataTable = dataTable;
            this.dataGridView = dataGridView;
            this.dataTable2 = dataTable2;
            this.dataGridView2 = dataGridView2;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.AllowDrop = true;
            this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            this.dataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragEnter);
            addContextMenu();
            LoadListItems();
        }

        public void InitDataGridView(Form baseForm, IFunction function, DataGridView dataGridView, DataTable dataTable)
        {
            this._function = function;
            this.baseForm = baseForm;
            this.dataTable = dataTable;
            this.dataGridView = dataGridView;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.AllowDrop = true;
            this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            this.dataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragEnter);
            addContextMenu();
            LoadListItems();
        }
        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            try
            {
                switch (this.targetSourceIndex)
                {
                    default:
                    case 1:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            if (!((BaseFunction)this._function).RefSource1.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource1.Add(node.Text, (IFunction)node.Tag);
                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource1.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource1.Add(node.Parent.Text + "." + node.Text, (IFunction)node.Parent.Tag);
                        }
                        break;
                    case 2:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            if (!((BaseFunction)this._function).RefSource2.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource2.Add(node.Text, (IFunction)node.Tag);
                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource2.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource2.Add(node.Parent.Text + "." + node.Text, (IFunction)node.Parent.Tag);
                        }
                        break;
                    case 3:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            if (!((BaseFunction)this._function).RefSource3.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource3.Add(node.Text, (IFunction)node.Tag);
                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource3.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource3.Add(node.Parent.Text + "." + node.Text, (IFunction)node.Parent.Tag);
                        }
                        break;
                    case 4:
                        if (node.Parent == null) // 父节点为Null，表示本身为父节点
                        {
                            if (!((BaseFunction)this._function).RefSource4.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource4.Add(node.Text, (IFunction)node.Tag);
                        }
                        else
                        {
                            if (!((BaseFunction)this._function).RefSource4.ContainsKey(node.Text))
                                ((BaseFunction)this._function).RefSource4.Add(node.Parent.Text + "." + node.Text, (IFunction)node.Parent.Tag);
                        }
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void dataGridView_DragEnter(object sender, DragEventArgs e)
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
                new ToolStripMenuItem("清空"),
                new ToolStripMenuItem("矩形阵列"),
                new ToolStripMenuItem("圆形阵列"),
                 new ToolStripMenuItem("偏置"),
                new ToolStripMenuItem("移动到激光位置"),
                new ToolStripMenuItem("移动到相机位置"),
                // new ToolStripMenuItem("更新Z值"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            this.dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        if (this.dataGridView.CurrentRow == null) return;
                        this.dataGridView.Rows.Remove(this.dataGridView.CurrentRow);
                        for (int i = 0; i < this.dataGridView.Rows.Count; i++)
                        {
                            this.dataGridView.Rows[i].HeaderCell.Value = i.ToString();
                        }
                        break;

                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        this.dataTable.Rows.Clear();
                        break;

                    case "矩形阵列":
                        form = new RectangleArrayDataForm(this.dataGridView, this.dataTable);
                        form.Owner = baseForm;
                        form.Show();
                        break;

                    case "圆形阵列":
                        form = new CircleArrayDataForm(this.dataGridView, this.dataTable);
                        form.Owner = baseForm;
                        form.Show();
                        break;

                    case "移动到激光位置":
                        if (this.dataGridView.CurrentRow == null) return;
                        MoveCommandParam CommandParam, affineCommandParam;
                        CommandParam = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.MoveSpeed);
                        CommandParam.AxisParam = new CoordSysAxisParam();
                        for (int i = 0; i < 6; i++)
                            CommandParam.AxisParam = new CoordSysAxisParam();
                        affineCommandParam = CommandParam.Affine2DCommandParam((userWcsCoordSystem)this._function.GetPropertyValues("坐标系"));
                        MotionCardManage.CurrentCard.MoveMultyAxis(affineCommandParam.CoordSysName,affineCommandParam.MoveAxis, affineCommandParam.MoveSpeed, affineCommandParam.AxisParam);
                        break;

                    case "移动到相机位置":
                        //HTuple laserPose;
                        if (this.dataGridView.CurrentRow == null) return;
                        CommandParam = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.MoveSpeed);
                        CommandParam.AxisParam = new CoordSysAxisParam();
                        /////////////////////////////////////////////////
                        userWcsPose laserPose = ((FunctionBlock.AcqSource)this.dataGridView.Tag).Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
                        for (int i = 0; i < 6; i++)
                        {
                            if (i == 0 || i == 6)
                                CommandParam.AxisParam.X = Convert.ToDouble(this.dataGridView.CurrentRow.Cells[i].Value) - laserPose.Tx;
                            if (i == 1 || i == 7)
                                CommandParam.AxisParam.Y = Convert.ToDouble(this.dataGridView.CurrentRow.Cells[i].Value) - laserPose.Ty;
                            if (i == 2 || i == 8)
                                CommandParam.AxisParam.Z = Convert.ToDouble(this.dataGridView.CurrentRow.Cells[i].Value) - laserPose.Tz;
                        }
                        /////////////////////////////////////////////
                        affineCommandParam = CommandParam.Affine2DCommandParam((userWcsCoordSystem)this._function.GetPropertyValues("坐标系"));
                        MotionCardManage.CurrentCard.MoveMultyAxis(affineCommandParam.CoordSysName,affineCommandParam.MoveAxis, affineCommandParam.MoveSpeed, affineCommandParam.AxisParam);
                        break;
                    ///////////////////////////////////////////////
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
                    // this.dataGridView.Items.AddRange(((BaseFunction)this._function).RefSource1.ToArray());
                    break;
                case 2:
                    // this.dataGridView.Items.AddRange(((BaseFunction)this._function).RefSource2.ToArray());
                    break;
                case 3:
                    //  this.dataGridView.Items.AddRange(((BaseFunction)this._function).RefSource3.ToArray());
                    break;
                case 4:
                    //  this.dataGridView.Items.AddRange(((BaseFunction)this._function).RefSource4.ToArray());
                    break;
            }
        }





    }
}
