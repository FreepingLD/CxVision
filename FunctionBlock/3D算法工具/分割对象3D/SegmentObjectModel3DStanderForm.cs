
using Common;
using FunctionBlock;
using HalconDotNet;
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
using View;

namespace FunctionBlock
{
    public partial class SegmentObjectModel3DStanderForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView  drawObject;

        public SegmentObjectModel3DStanderForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.Text = ((SegmentObjectModle3D)this._function).Name;
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
            //new DataGridViewWrapClass().InitDataGridView(this, function, this.dataGridView1, ((Rectangle2Crop)this._function).Coord1Table);
        }
        private void SegmentObjectModel3DStanderForm_Load(object sender, EventArgs e)
        {
            BindProperty();
           // ((SegmentObjectModle3D)this._function).DisplayExcuteResultEvent += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
        }
        private void BindProperty()
        {
            try
            {
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                //this.dataGridView1.DataSource = ((Rectangle2Crop)this._function).Coord1Table;
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(SegmentObjectModle3D.enShowItems));
                this.分割极性comboBox.DataSource = Enum.GetNames(typeof(SegmentObjectModle3D.enSegmentPolarity));
                this.截取方向comboBox.DataSource = Enum.GetNames(typeof(SegmentObjectModle3D.enKeepRegion));
                //////////////////////////////////////////////////
                this.平面偏置textBox.DataBindings.Add("Text", (SegmentObjectModle3D)this._function, "Plane_offset", true, DataSourceUpdateMode.OnPropertyChanged);
                this.分割极性comboBox.DataBindings.Add("Text", (SegmentObjectModle3D)this._function, "Polarity", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.半长textBox.DataBindings.Add("Text", (SegmentObjectModle3D)this._function, "Length1", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.半宽textBox.DataBindings.Add("Text", (SegmentObjectModle3D)this._function, "Length2", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.角度textBox.DataBindings.Add("Text", (SegmentObjectModle3D)this._function, "Deg", true, DataSourceUpdateMode.OnPropertyChanged);
                this.截取方向comboBox.DataBindings.Add("Text", (SegmentObjectModle3D)this._function, "InsideOrOutside", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void DisplayObjectModel(object send, ExcuteCompletedEventArgs e)
        {
            try
            {
                switch (e.DataContent.GetType().Name)
                {
                    case "HObjectModel3D":
                        this.drawObject.PointCloudModel3D = new PointCloudData( (HObjectModel3D)e.DataContent );
                        break;
                    case "HObjectModel3D[]":
                        this.drawObject.PointCloudModel3D =  new PointCloudData((HObjectModel3D[])e.DataContent) ;
                        break;
                }
            }
            catch
            {

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
                if(object3D!=null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData( (HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
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
                object object3D = this._function.GetPropertyValues(this.显示条目comboBox.SelectedItem.ToString().Trim());
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
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawObject.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawObject.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Translate":
                    this.drawObject.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawObject.AutoImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_3D":
                    this.drawObject.Show3D();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                default:
                    break;
            }
        }


        private void SegmentObjectModel3DStanderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }


        private void 绘制矩形button_Click(object sender, EventArgs e)
        {
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
            userWcsRectangle2 wcsRect2;
            //////////////////////////////////////
            try
            {
                this.drawObject.DrawWcsRect2OnWindow(enColor.white, 0, 0, out wcsRect2);
                //ha.DrawRectangle2OnWindow(this.hWindowControl1.HalconWindow, enColor.white, this.drawObject.CamParam, this.drawObject.CamPose, out wcsRect2);
                //this.dataGridView1.Rows.Add(wcsRect2.x, wcsRect2.y, wcsRect2.z, wcsRect2.deg, wcsRect2.length1, wcsRect2.length2);
                ((SegmentObjectModle3D)this._function).Coord1Table.Rows.Add(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Deg, wcsRect2.Length1, wcsRect2.Length2);
            }
            catch (Exception ee)
            {
                MessageBox.Show("绘制圆形button_Click操作失败" + ee.ToString());
            }
        }

        private void 图形取点button_Click(object sender, EventArgs e)
        {
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
            userWcsPoint wcsPoint;
            double length1, length2, deg;
            try
            {
                if (double.TryParse(this.角度textBox.Text, out deg) && double.TryParse(this.半长textBox.Text, out length1) && double.TryParse(this.半宽textBox.Text, out length2))
                {
                    this.drawObject.DrawWcsPointOnWindow(enColor.white,0,0, out wcsPoint);
                    //ha.DrawPointOnWindow(this.hWindowControl1.HalconWindow, this.drawObject.CamParam, this.drawObject.CamPose, out wcsPoint);
                    ((SegmentObjectModle3D)this._function).Coord1Table.Rows.Add(wcsPoint.X, wcsPoint.Y, wcsPoint.Z, deg, length1, length2);
                    //this.drawObject.AttachDrawingObjectToWindow((new userWcsRectangle2(wcsPoint.x, wcsPoint.y, wcsPoint.z, deg, length1, length2, this.drawObject.CamParam, this.drawObject.CamPose)));
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("图形取点button_Click操作失败" + ee.ToString());
            }
        }

        private void 写入点button_Click(object sender, EventArgs e)
        {
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
            double X, Y, Z, length1, length2, deg; //radius,
            try
            {
                //((SegmentObjectModle3D)this._function).CoordSystemType = Common.enCoordinateSystemType.当前坐标系;
                userWcsCoordSystem wcsCoord = ((SegmentObjectModle3D)this._function).extractRefSource2Data();
                if (double.TryParse(this.X坐标textBox.Text, out X) && double.TryParse(this.Y坐标textBox.Text, out Y) && double.TryParse(this.Z坐标textBox.Text, out Z)
                    && double.TryParse(this.角度textBox.Text, out deg) && double.TryParse(this.半长textBox.Text, out length1) && double.TryParse(this.半宽textBox.Text, out length2))
                {
                    ((SegmentObjectModle3D)this._function).Coord1Table.Rows.Add(X, Y, Z, deg, length1, length2);
                    //this.dataGridView1.Rows.Add(X, Y, Z, deg, length1, length2);
                    //this.drawObject.AttachDrawingObjectToWindow(new userWcsRectangle2(X, Y, Z, deg, length1, length2, this.drawObject.CamParam, this.drawObject.CamPose).AffineWcsRectangle2(wcsCoord));
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("添加点button_Click操作失败" + ee.ToString());
            }
        }

        private void 删除点button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                ((SegmentObjectModle3D)this._function).Coord1Table.Rows.RemoveAt(index);
            }
            catch (Exception ee)
            {
                MessageBox.Show("删除点button_Click" + ee.ToString());
            }
        }

        private void 清空点button_Click(object sender, EventArgs e)
        {
            try
            {
                ((SegmentObjectModle3D)this._function).Coord1Table.Rows.Clear();
            }
            catch (Exception ee)
            {
                MessageBox.Show("清空点button_Click" + ee.ToString());
            }
        }

        private void 导入点button_Click(object sender, EventArgs e)
        {
            try
            {
                FileOperate fo = new FileOperate();
                ////////////////////
                ImportData(fo.OpenFile());
            }
            catch (Exception ee)
            {
                MessageBox.Show("导入点button_Click操作失败" + ee.ToString());
            }
        }
        private void ImportData(string Path)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, deg = 0, legnth1 = 0, legnth2 = 0;
                if (Path == null || Path.Length == 0) return;
                using (StreamReader reader = new StreamReader(Path))
                {
                    string line;
                    while (true)
                    {
                        line = reader.ReadLine();
                        if (line == null) break;
                        string[] st = line.Split(',', '\t', ';'); //如果其他分隔符替换掉就OK了 
                        if (st.Length > 0)
                            X = Convert.ToDouble(st[0]);
                        if (st.Length > 1)
                            Y = Convert.ToDouble(st[1]);
                        if (st.Length > 2)
                            Z = Convert.ToDouble(st[2]);
                        if (st.Length > 3)
                            deg = Convert.ToDouble(st[3]);
                        if (st.Length > 4)
                            legnth1 = Convert.ToDouble(st[4]);
                        if (st.Length > 5)
                            legnth2 = Convert.ToDouble(st[5]);
                       // ((Rectangle2Crop)_function).Coord1Table.Rows.Add(new object[] { X, Y, Z, deg, legnth1, legnth2 }); // 每更改一次数据源会触发一次dataGridView的数据绑定完成事件
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                userWcsRectangle2 wcsRect2;
                int index = this.dataGridView1.CurrentRow.Index;
                for (int i = 0; i < this.drawObject.AttachPropertyData.Count; i++)
                {
                    if (i == index + 1)
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsRectangle2)
                        {
                            wcsRect2 = (userWcsRectangle2)this.drawObject.AttachPropertyData[i];
                            wcsRect2.Color = enColor.orange;
                            this.drawObject.AttachPropertyData[i] = wcsRect2;
                        }
                    }
                    else
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsRectangle2)
                        {
                            wcsRect2 = (userWcsRectangle2)this.drawObject.AttachPropertyData[i];
                            wcsRect2.Color = enColor.white;
                            this.drawObject.AttachPropertyData[i] = wcsRect2;
                        }
                    }
                }
                //////////
                /////////////
                if (this.drawObject.PointCloudModel3D == null)
                    this.drawObject.UpdataGraphicView();
                else
                    this.drawObject.ShowAttachProperty();
            }
            catch (Exception ee)
            {
                MessageBox.Show("dataGridView1_SelectionChanged操作失败" + ee.ToString());
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                HTuple Qx, Qy;
                userWcsRectangle2 wcsRect2 = new userWcsRectangle2();
                this.drawObject.AttachPropertyData.Clear();
                //userWcsCoordSystem userWcsCoordSystem = ((Rectangle2Crop)_function).extractRefSource2Data();
                //this.drawObject.AttachPropertyData.Add(userWcsCoordSystem);
                //DataGridViewCellCollection dataGridViewCellCollection;
                //for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                //{
                //    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                //    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
                //    if (dataGridViewCellCollection.Count > 0)
                //        wcsRect2.x = Convert.ToDouble(dataGridViewCellCollection[0].Value);
                //    if (dataGridViewCellCollection.Count > 1)
                //        wcsRect2.y = Convert.ToDouble(dataGridViewCellCollection[1].Value);
                //    if (dataGridViewCellCollection.Count > 2)
                //        wcsRect2.z = Convert.ToDouble(dataGridViewCellCollection[2].Value);
                //    if (dataGridViewCellCollection.Count > 3)
                //        wcsRect2.deg = Convert.ToDouble(dataGridViewCellCollection[3].Value);
                //    if (dataGridViewCellCollection.Count > 4)
                //        wcsRect2.length1 = Convert.ToDouble(dataGridViewCellCollection[4].Value);
                //    if (dataGridViewCellCollection.Count > 5)
                //        wcsRect2.length2 = Convert.ToDouble(dataGridViewCellCollection[5].Value);
                //    // 将点绘制到图像上                  
                //    //HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetVariationHomMat2D(), wcsRect2.x, wcsRect2.y, out Qx, out Qy);
                //    wcsRect2.x = Qx.D;
                //    wcsRect2.y = Qy.D;
                //    wcsRect2.color = enColor.white;
                //    this.drawObject.AttachPropertyData.Add(wcsRect2);
                //}
                /////////////
                if (this.drawObject.PointCloudModel3D == null)
                    this.drawObject.UpdataGraphicView();
                else
                    this.drawObject.DisplayModelObject3D();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}
