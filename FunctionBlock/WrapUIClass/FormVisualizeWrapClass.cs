using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public class FormVisualizeWrapClass
    {
        private IFunction _function;
        private VisualizeView drawObject;
        private Label _grayValue1Label, _grayValue2Label, _grayValue3Label, _rowPosLabel, _colPosLabel;
        private ComboBox _comboBox;
        private ToolStrip _toolStrip;

        public void Init(Form form, IFunction function, VisualizeView drawObject, ComboBox comboBox, ToolStrip toolStrip, params Label[] labels)
        {
            this._function = function;
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            // 注册事件
            if (drawObject != null)
            {
                this.drawObject = drawObject;
                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GrayValueInfo);
            }
            if (comboBox != null)
            {
                this._comboBox = comboBox;
                this._comboBox.SelectionChangeCommitted += new System.EventHandler(this.显示条目comboBox_SelectionChangeCommitted);
            }
            if (toolStrip != null)
            {
                this._toolStrip = toolStrip;
                this._toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            }
            if (labels != null)
            {
                if (labels.Length > 0)
                    this._grayValue1Label = labels[0];
                if (labels.Length > 1)
                    this._grayValue2Label = labels[1];
                if (labels.Length > 2)
                    this._grayValue3Label = labels[2];
                if (labels.Length > 3)
                    this._rowPosLabel = labels[3];
                if (labels.Length > 4)
                    this._colPosLabel = labels[4];
            }
        }
        public void UnInit()
        {
            BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            // 注册事件
            if (this.drawObject != null)
            {
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GrayValueInfo);
            }
            if (this._comboBox != null)
            {
                this._comboBox.SelectionChangeCommitted -= new System.EventHandler(this.显示条目comboBox_SelectionChangeCommitted);
            }
            if (this._toolStrip != null)
            {
                this._toolStrip.ItemClicked -= new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);
            }
        }
        private void GrayValueInfo(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
                this._grayValue1Label.Text = e.GaryValue[0].ToString();
            else
                this._grayValue1Label.Text = 0.ToString();
            ///////////////////////////////////////////
            if (e.GaryValue.Length > 1)
                this._grayValue2Label.Text = e.GaryValue[1].ToString();
            else
                this._grayValue2Label.Text = 0.ToString();
            /////////////////////////////////////////
            if (e.GaryValue.Length > 2)
                this._grayValue3Label.Text = e.GaryValue[2].ToString();
            else
                this._grayValue3Label.Text = 0.ToString();
            ///////////////////////////////////////////////
            this._rowPosLabel.Text = e.Row.ToString();
            this._colPosLabel.Text = e.Col.ToString();
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
                            userWcsPoint[] wcsPoint = (userWcsPoint[])e.DataContent;
                            for (int i = 0; i < wcsPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
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
                object object3D = this._function.GetPropertyValues(this._comboBox.SelectedItem.ToString().Trim());
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)object3D );
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
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
                            this.drawObject.ShowAttachProperty();
                            break;
                        case "userPixPoint":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userPixPoint)object3D);
                            this.drawObject.ShowAttachProperty();
                            break;
                        case "userWcsPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userWcsPoint[] wcsPoint = (userWcsPoint[])object3D;
                            for (int i = 0; i < wcsPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            }
                            this.drawObject.ShowAttachProperty();
                            break;
                    }
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
                    foreach (ToolStripButton item in this._toolStrip.Items)
                    {
                        if (item.Name == "toolStripButton_Translate")
                        {
                            item.CheckState = CheckState.Unchecked;
                            break;
                        }
                    }
                    //this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawObject.Select();
                    foreach (ToolStripButton item in this._toolStrip.Items)
                    {
                        if (item.Name == "toolStripButton_Translate")
                        {
                            item.CheckState = CheckState.Unchecked;
                            break;
                        }
                    }
                    break;
                case "toolStripButton_Translate":
                    this.drawObject.TranslateScaleImage();
                    foreach (ToolStripButton item in this._toolStrip.Items)
                    {
                        if (item.Name == "toolStripButton_Translate")
                        {
                            item.CheckState = CheckState.Checked;
                            break;
                        }
                    }
                    //this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawObject.AutoImage();
                    foreach (ToolStripButton item in this._toolStrip.Items)
                    {
                        if (item.Name == "toolStripButton_Translate")
                        {
                            item.CheckState = CheckState.Unchecked;
                            break;
                        }
                    }
                    //this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case "toolStripButton_3D":
                    foreach (ToolStripButton item in this._toolStrip.Items)
                    {
                        if (item.Name == "toolStripButton_Translate")
                        {
                            item.CheckState = CheckState.Unchecked;
                            break;
                        }
                    }
                    // this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                default:
                    break;
            }
        }


    }


}
