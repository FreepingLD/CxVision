
using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using View;
using System.Threading; 

namespace FunctionBlock
{
    public partial class Straightness3DForm : Form
    {
        private HObjectModel3D[] PointCloudModel3D;
        private IFunction _function;
        private DrawingChartView chartView;
        private VisualizeView drawObject;
        private CancellationTokenSource cts;

        public Straightness3DForm(IFunction function,TreeNode node)
        {      
            this._function = function;
            InitializeComponent();
            this.chartView = new DrawingChartView(this.chart1);
            this.Text = this._function.GetPropertyValues("名称").ToString();
            //this.drawObject = new VisualizeView(this.hWindowControl1, true);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void Straightness3DForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            //ListBoxWrapClass.AddItems += new AddItemsEventHandler(listbox_AddItems);
            BindProperty();
            initChartView();
        }

        private void initChartView()
        {
            if (this.chartView!=null)
            {
                this.chartView.AddChartArea("直线度区域", "X坐标(单位：um)", "Y坐标(单位：um)");
                this.chartView.AddLegend("legend1", Docking.Top, StringAlignment.Center);
                this.chartView.AddSerie("测量曲线", "legend1", Color.Black, SeriesChartType.Line); // 图例添加一个就可以了
                this.chartView.AddSerie("理想曲线", "legend1", Color.Green, SeriesChartType.Line);
                this.chartView.AddSerie("上偏差", "legend1", Color.Red, SeriesChartType.Line);
                this.chartView.AddSerie("下偏差", "legend1", Color.Red, SeriesChartType.Line);
                this.chartView.SetChartTitle("直线度");
                this.chartView.DataScaleFactor = 1000;
                this.chartView.InitChartArea("直线度区域",0,0.1,-0.01,0.01);              
                this.chartView.UpDataChart("直线度区域", "理想曲线", HTuple.TupleGenConst(100, 0.0).DArr);
            }
        }

        private void BindProperty()
        {
            try
            {
                //this.显示条目comboBox.DataSource = Enum.GetNames(typeof(Straightness3D.enShowItems));
                this.dataGridView1.DataBindings.Add("DataSource", (Straightness3D)this._function, "ResultDataTable", true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准值textBox.DataBindings.Add("Text", (Straightness3D)this._function, "StdValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.上公差textBox.DataBindings.Add("Text", (Straightness3D)this._function, "UpTolerance", true, DataSourceUpdateMode.OnPropertyChanged);
                this.下公差textBox.DataBindings.Add("Text", (Straightness3D)this._function, "DownTolerance", true, DataSourceUpdateMode.OnPropertyChanged);
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
        public void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)
        {
            if (sender is Straightness3D)
            {
                try
                {
                    if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                    {
                        switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                        {
                            case "HObjectModel3D":
                                this.PointCloudModel3D = new HObjectModel3D[] { (HObjectModel3D)e.DataContent };
                                for (int i = 0; i < PointCloudModel3D.Length; i++)
                                {
                                    HTuple tar_x = PointCloudModel3D[i].GetObjectModel3dParams("point_coord_x");
                                    HTuple tar_y = PointCloudModel3D[i].GetObjectModel3dParams("point_coord_y");
                                    HTuple tar_z = PointCloudModel3D[i].GetObjectModel3dParams("point_coord_z");
                                    this.chartView.InitChartArea("直线度区域", tar_x.TupleMin(), tar_x.TupleMax(), tar_y.TupleMin(), tar_y.TupleMax());
                                    this.chartView.UpDataChart("直线度区域", "测量曲线", tar_x.DArr, tar_y.DArr);
                                    this.chartView.UpDataChart("直线度区域", "理想曲线", tar_x.DArr, tar_z.DArr);
                                    this.chartView.UpDataChart("直线度区域", "上偏差", tar_x.DArr, HTuple.TupleGenConst(tar_y.Length,tar_y.TupleMax()));
                                    this.chartView.UpDataChart("直线度区域", "下偏差", tar_x.DArr, HTuple.TupleGenConst(tar_y.Length, tar_y.TupleMin()));
                                }
                                break;
                            case "HObjectModel3D[]":
                                this.PointCloudModel3D = (HObjectModel3D[])e.DataContent;
                                for (int i = 0; i < PointCloudModel3D.Length; i++)
                                {
                                    HTuple tar_x = PointCloudModel3D[i].GetObjectModel3dParams("point_coord_x");
                                    HTuple tar_y = PointCloudModel3D[i].GetObjectModel3dParams("point_coord_y");
                                    HTuple tar_z = PointCloudModel3D[i].GetObjectModel3dParams("point_coord_z");
                                    this.chartView.InitChartArea("直线度区域", 0, tar_x.TupleMax()- tar_x.TupleMin(), tar_y.TupleMin(), tar_y.TupleMax());
                                    this.chartView.UpDataChart("直线度区域", "测量曲线", tar_x.DArr, tar_y.DArr);
                                    this.chartView.UpDataChart("直线度区域", "理想曲线", tar_x.DArr, tar_z.DArr);
                                    this.chartView.UpDataChart("直线度区域", "上偏差", tar_x.DArr, HTuple.TupleGenConst(tar_y.Length, tar_y.TupleMax()).DArr);
                                    this.chartView.UpDataChart("直线度区域", "下偏差", tar_x.DArr, HTuple.TupleGenConst(tar_y.Length, tar_y.TupleMin()).DArr);
                                }
                                break;
                        }
                    }
                }
                catch (Exception he)
                {

                }
            }
        }
        private void listbox_AddItems(object send, ItemsChangeEventArgs e)
        {
            try
            {
                object object3D;
                if (e.ItemName == null || e.ItemName.Trim().Length == 0) return;
                if (e.ItemName.Split('.').Length == 1)
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName);
                else
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName.Split('.')[1]);
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)object3D;
                            break;
                        case "XldDataClass[]":
                            this.drawObject.XldContourData = (XldDataClass)object3D;
                            break;
                        case "RegionDataClass":
                            this.drawObject.RegionData =  (RegionDataClass)object3D;
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.RegionData = (RegionDataClass)object3D;
                            break;
                        case "userPixPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userPixPoint[] pixPoint = (userPixPoint[])object3D;
                            for (int i = 0; i < pixPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(pixPoint[i]);
                            }
                            break;
                        case "userWcsPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userWcsPoint[] wcsPoint = (userWcsPoint[])object3D;
                            for (int i = 0; i < wcsPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            }
                            break;
                    }
                }
            }
            catch
            {

            }
        }
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                object object3D = null; // this._function.GetPropertyValues(this.显示条目comboBox.SelectedItem.ToString().Trim());
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "HImage":
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D); // 图形窗口不显示图像
                            break;
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)object3D;
                            break;
                        case "HXLDCont":
                            this.drawObject.XldContourData =  new XldDataClass((HXLDCont)object3D);
                            break;
                        case "XldDataClass[]":
                            this.drawObject.XldContourData = (XldDataClass)object3D;
                            break;
                        case "RegionDataClass":
                            this.drawObject.RegionData =  (RegionDataClass)object3D;
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.RegionData = (RegionDataClass)object3D;
                            break;
                        case "userPixPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userPixPoint[] pixPoint = (userPixPoint[])object3D;
                            for (int i = 0; i < pixPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(pixPoint[i]);
                            }
                            break;
                        case "userWcsPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userWcsPoint[] wcsPoint = (userWcsPoint[])object3D;
                            for (int i = 0; i < wcsPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            }
                            break;
                    }
                }
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
        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
        }

        private void Straightness3DForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.cts != null)
                    this.cts.Cancel();
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                // ListBoxWrapClass.AddItems -= new AddItemsEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }








    }
}
