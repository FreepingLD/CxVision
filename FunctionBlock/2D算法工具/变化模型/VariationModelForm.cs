using AlgorithmsLibrary;
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
using View;

namespace FunctionBlock
{
    public partial class VariationModelForm : Form
    {
        private Form Creatform, Findform;
        private IFunction _function;
        private VisualizeView drawObject;
        private HImage sourceImage;
        public VariationModelForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            addContextMenu(this.模型区域dataGridView);
           // addContextMenu(this.搜索区域dataGridView);
        }

        private void addContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
            };
            ContextMenuStrip1.Name = dataGridView.Name;
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
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
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                //if (this.搜索区域dataGridView.CurrentRow == null || this.搜索区域dataGridView.CurrentRow.Index < 0) return;
                                //((FunctionBlock.DoShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion.RemoveAt(this.搜索区域dataGridView.CurrentRow.Index);
                                break;
                            case "模型区域dataGridView":
                                if (this.模型区域dataGridView.CurrentRow == null || this.模型区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion.RemoveAt(this.模型区域dataGridView.CurrentRow.Index);
                                break;
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                //if (this.搜索区域dataGridView.CurrentRow == null || this.搜索区域dataGridView.CurrentRow.Index < 0) return;
                                //((FunctionBlock.DoShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion.RemoveAt(this.搜索区域dataGridView.CurrentRow.Index);
                                break;
                            case "模型区域dataGridView":
                                if (this.模型区域dataGridView.CurrentRow == null || this.模型区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion.RemoveAt(this.模型区域dataGridView.CurrentRow.Index);
                                break;
                        }
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
        private void ShapeModelMatchForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            //////////        
            AddForm(this.创建模型参数groupBox, new CreateNccModelParamForm(((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelParam));
            AddForm(this.匹配参数panel, new FindNccModelParamForm(((FunctionBlock.NccModelMatch)this._function).DoNccModel.FindNccModelParam));
        }



        private void BindProperty()
        {
            try
            {
                //this.搜索区域dataGridView.DataSource = ((FunctionBlock.DoNccModelMatch2D)this._function).NccModelMatch.FindNccModelParam.SearchRegion;
                this.模型区域dataGridView.DataSource = ((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion;
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                // 创建匹配参数
                //this.模型创建方法comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enLocalDeformableType));
                //this.模型创建方法comboBox.DataBindings.Add("Text", ((FunctionBlock.DoNccModelMatch2D)this._function).NccModelMatch, "ModelMacthType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.dataGridView1.DataSource = ((FunctionBlock.NccModelMatch)this._function).ResultInfo;
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
            try
            {
                if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)e.DataContent);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])e.DataContent);
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                            break;
                        case "HImage":
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)e.DataContent;
                            break;
                        case "XldDataClass[]":
                            this.drawObject.XldContourData =                             this.drawObject.XldContourData = (XldDataClass)e.DataContent;;
                            break;
                        case "HXLDCont":
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont)e.DataContent) ;
                            break;
                        case "HXLDCont[]":
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont[])e.DataContent);
                            break;
                        case "RegionDataClass":
                            this.drawObject.RegionData =  (RegionDataClass)e.DataContent ;
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.RegionData = (RegionDataClass)e.DataContent;
                            break;
                        case "userWcsCircle":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsCircle)e.DataContent); //(userWcsCircle)e.DataContent
                            this.drawObject.UpdataGraphicView(); // 背影不刷新
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsCircleSector)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsEllipse)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsEllipseSector)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsLine":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsLine)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsPoint":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsPoint)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsRectangle1)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsRectangle2)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userPixPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userPixPoint[] pixPoint = (userPixPoint[])e.DataContent;
                            for (int i = 0; i < pixPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(pixPoint[i]);
                            }
                            break;
                        case "userPixPoint":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userPixPoint[])e.DataContent);
                            break;
                        case "userWcsPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userWcsPoint[] wcsPoint = (userWcsPoint[])this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim());
                            for (int i = 0; i < wcsPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            }
                            break;
                    }
                }
            }
            catch (Exception he)
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
                        case "userPixPoint":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userPixPoint[])object3D);
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
                enShowItems showItem; //= (enShowItems)this.显示条目comboBox.SelectedItem;
                Enum.TryParse(this.显示条目comboBox.SelectedItem.ToString(), out showItem);
                switch (showItem)
                {
                    case enShowItems.输入对象:
                        if (this.sourceImage == null || !this.sourceImage.IsInitialized())
                            this.drawObject.BackImage = ((FunctionBlock.ShapeModelMatch2D)this._function).ImageData;
                        else
                            this.drawObject.BackImage = new ImageDataClass(this.sourceImage);
                        break;
                    case enShowItems.模板图像:
                        this.drawObject.BackImage = new ImageDataClass(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.TemplateImage);
                        break;
                    case enShowItems.模型轮廓:
                        this.drawObject.XldContourData = new XldDataClass(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.ShapeModelContour);
                        break;
                    case enShowItems.搜索区域:
                        this.drawObject.BackImage = new ImageDataClass(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.SearchImageRegion);
                        break;
                }
            }
            catch
            {

            }
        }
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
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

        // 获取鼠标位置处的高度值
        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
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

        private void NccModelMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                //this.hWindowControl1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.hWindowControl1_MouseMove);
            }
            catch
            {

            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }



        private void 区域dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 设置搜索区域button_Click_1(object sender, EventArgs e)
        {
            try
            {
                drawPixRect1 pixRecrt2;
                this.drawObject.DrawPixRect1OnWindow(enColor.red, out pixRecrt2);
                //((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion.Add(new drawPixRect2(pixRecrt2.row, pixRecrt2.col, pixRecrt2.rad, pixRecrt2.length1, pixRecrt2.length2));
                //int index = this.搜索区域dataGridView.Rows.Add(pixRecrt2.center_row, pixRecrt2.center_col, pixRecrt2.rad, pixRecrt2.length1, pixRecrt2.length2);
                //this.搜索区域dataGridView.Rows[index].HeaderCell.Value = index.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 检查形状模型button_Click_1(object sender, EventArgs e)
        {
            try
            {
                HImage reduceImage;
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                if (((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion .Count > 0)
                {
                    foreach (var item in ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion)
                    {
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                    }
                    reduceImage = this.drawObject.BackImage.Image.ReduceDomain(hRegion.Union1());
                }
                else
                    reduceImage = this.drawObject.BackImage.Image;
                reduceImage.InspectShapeModel(out hRegion, 5, 10);
                HRegion selectRegion1 = hRegion.SelectObj(1);
                HXLDCont hXLDCont = selectRegion1.GenContourRegionXld("border");
                this.drawObject.AttachPropertyData.Clear();
                this.drawObject.AttachPropertyData.Add(hXLDCont);
                this.drawObject.ShowAttachProperty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 设置模型区域button_Click_1(object sender, EventArgs e)
        {
            try
            {
                userPixRectangle1 pixRecrt1;
                this.drawObject.DrawPixRect1OnWindow(enColor.red, out pixRecrt1);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void LocadImageButton_Click(object sender, EventArgs e)
        {
            try
            {
                string path = new FileOperate().OpenImage();
                this.sourceImage = new HImage(path);
                if (this.sourceImage != null)
                    this.drawObject.BackImage = new ImageDataClass(this.sourceImage); //, this._acqSource.Sensor.CameraParam
                else
                    throw new ArgumentException("读取的图像为空");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 创建模型button_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelsFromImage(this.drawObject.BackImage.Image))
                {
                    this.drawObject.XldContourData = ((FunctionBlock.NccModelMatch)this._function).DoNccModel.NccMatchResult.MatchCont;
                    //this.drawObject.AttachPropertyData.Clear();
                    //foreach (var item in ((FunctionBlock.NccModelMatch)this._function).DoNccModel.NccMatchResult.MatchCont)
                    //    if (item != null && item.IsInitialized())
                    //    {
                    //        this.drawObject.AttachPropertyData.Add(item);
                    //        this.drawObject.ShowAttachProperty();
                    //    }
                    MessageBox.Show("模型创建成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 搜索区域dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 模型创建方法comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Creatform != null) Creatform.Dispose();
            if (Findform != null) Findform.Dispose();
            this.Creatform = new CreateNccModelParamForm(((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelParam);
            this.Findform = new FindNccModelParamForm(((FunctionBlock.NccModelMatch)this._function).DoNccModel.FindNccModelParam);
            //////////////
            AddForm(this.创建模型参数groupBox, this.Creatform);
            AddForm(this.匹配参数panel, this.Findform);  //.
        }



        public enum enShowItems
        {
            输入对象,
            模板图像,
            模板轮廓,
            模型轮廓,
            搜索区域,
            参考点,
            匹配点
        }

    }
}
