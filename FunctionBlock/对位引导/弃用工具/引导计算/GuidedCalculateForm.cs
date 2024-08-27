using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class GuidedCalculateForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        public GuidedCalculateForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            //new ListBoxWrapClass().InitListBox(this.listBox1, function);
            //new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public GuidedCalculateForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            this.Text = node.Text;
            //new ListBoxWrapClass().InitListBox(this.listBox1, node);
            //new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void GuidedCalculateForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////          
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                UserHomMat2D homMat2D = ((GuidedCalculate)this._function).HomMat2D;
                this.引导对象comboBox.DataSource = Enum.GetNames(typeof(enRobotJawEnum));
                this.引导对象comboBox.DataBindings.Add("Text", ((GuidedCalculate)this._function), "Jaw", true, DataSourceUpdateMode.OnPropertyChanged);
                this.C00_textBox.Text = homMat2D?.c00.ToString();
                this.C01_textBox.Text = homMat2D?.c01.ToString();
                this.C02_textBox.Text = homMat2D?.c02.ToString();
                this.C10_textBox.Text = homMat2D?.c10.ToString();
                this.C11_textBox.Text = homMat2D?.c11.ToString();
                this.C12_textBox.Text = homMat2D?.c12.ToString();
            }
            catch
            {

            }
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
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                                }));
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
        private void CalculateOffsetValueForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void PLC信息dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }




}
