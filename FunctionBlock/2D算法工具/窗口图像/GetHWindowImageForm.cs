using Common;
using FunctionBlock;
using Sensor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using View;

namespace FunctionBlock
{
    public partial class GetHWindowImageForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        public GetHWindowImageForm(IFunction function)
        {      
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
        }
        public GetHWindowImageForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
        }
        private void GetHWindowImage_Load(object sender, EventArgs e)
        {
            // 注册事件
            BindProperty();
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
        }
        private void BindProperty()
        {
            try
            {
                this.窗口名称comboBox.DataSource = HWindowManage.GetKeysList();// GetCamSensor()
                //////////////////
                this.窗口名称comboBox.DataBindings.Add(nameof(this.窗口名称comboBox.SelectedItem), ((GetHWindowImage)this._function), "TargetWindow", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
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
                            if (this._function.Execute(this._refNode).Succss)
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
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            try
            {
                if (e.GaryValue.Length > 0)
                    this.灰度值1Label.Text = e.GaryValue[0].ToString();
                else
                    this.灰度值1Label.Text = 0.ToString();
                ///////////////////////////////////////////
                if (e.GaryValue.Length > 1)
                    this.灰度值2Label.Text = e.GaryValue[1].ToString();
                else
                    this.灰度值2Label.Text = 0.ToString();
                /////////////////////////////////////////
                if (e.GaryValue.Length > 2)
                    this.灰度值3Label.Text = e.GaryValue[2].ToString();
                else
                    this.灰度值3Label.Text = 0.ToString();
                ///////////////////////////////////////////////
                this.行坐标Label.Text = e.Row.ToString();
                this.列坐标Label.Text = e.Col.ToString();
            }
            catch
            { }
        }
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void GetHWindowImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            }
            catch
            {

            }
        }


    }
}
