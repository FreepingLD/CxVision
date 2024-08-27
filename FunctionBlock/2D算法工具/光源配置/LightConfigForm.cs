using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FunctionBlock
{
    public partial class LightConfigForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        private TreeNode _refNode;
        public LightConfigForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
        }

        public LightConfigForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            //new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void LightConfigForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                //////////////////////////////////////////////////
                this.LightControlColumn.Items.Clear();
                this.LightControlColumn.ValueType = typeof(string);
                foreach (string temp in LightConnectManage.GetLightName())
                    this.LightControlColumn.Items.Add(temp.ToString());
                this.LightChennelColumn.Items.Clear();
                this.LightChennelColumn.ValueType = typeof(enLightChannel);
                foreach (enLightChannel temp in Enum.GetValues(typeof(enLightChannel)))
                    this.LightChennelColumn.Items.Add(temp);
                this.光源dataGridView1.DataSource = ((LightConfig)this._function).LightList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }
        private void AddForm(GroupBox groupBox, Form form)
        {
            if (groupBox == null) return;
            if (groupBox.Controls.Count > 0)
                groupBox.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            groupBox.Controls.Add(form);
            form.Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        this.cts = new CancellationTokenSource();
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.toolStripStatusLabel1.Text = "执行结果:";
                                        this.toolStripStatusLabel2.Text = "成功";
                                        this.toolStripStatusLabel2.ForeColor = Color.Green;
                                    }));
                                }
                            }
                            else
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                   {
                                       this.toolStripStatusLabel1.Text = "执行结果:";
                                       this.toolStripStatusLabel2.Text = "失败";
                                       this.toolStripStatusLabel2.ForeColor = Color.Red;
                                   }));
                                }
                            }
                        }
                        );
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void LightConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cts != null)
                    cts.Cancel();
            }
            catch
            {

            }
        }




        private void 光源dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    BindingList<LightParam> LightList = ((BindingList<LightParam>)((LightConfig)this._function).LightList);
                    switch (光源dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "SetLightBtn":
                            if (LightList == null) return;
                            if (LightList.Count > e.RowIndex)
                            {
                                LightList[e.RowIndex].SetLight();
                            }
                            break;
                        case "DeleteBtn":
                            if (LightList == null) return;
                            if (LightList.Count > e.RowIndex)
                            {
                                LightList.RemoveAt(e.RowIndex);
                            }
                            break;
                        case "OpenBtn":
                            if (LightList == null) return;
                            if (LightList.Count > e.RowIndex)
                            {
                                LightList[e.RowIndex].Open();
                            }
                            break;
                    }
                    this.光源dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 光源dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }




    }
}
