using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FunctionBlock;
using View;

namespace FunctionBlock
{
    public class HWindowControlViewWrapClass
    {
        private VisualizeView drawObject;
        private object _objectDataModel;
        private HWindowControl hWindowControl1;
        private HTuple mousePositionValue;
        private ToolStrip toolStrip;
        private ToolStripStatusLabel rowLabel;
        private ToolStripStatusLabel colLabel;
        private ToolStripStatusLabel grayValue1Label;
        private ToolStripStatusLabel grayValue2Label;
        private ToolStripStatusLabel grayValue3Label;
        private ComboBox 显示条目comboBox;

        public HWindowControlViewWrapClass(HWindowControl hWindowControl, ToolStrip toolStrip, ToolStripStatusLabel rowLabel, ToolStripStatusLabel colLabel, ToolStripStatusLabel grayValue1Label, ToolStripStatusLabel grayValue2Label, ToolStripStatusLabel grayValue3Label)
        {
            this.drawObject = new VisualizeView(hWindowControl,true);
            this.toolStrip = toolStrip;
            this.rowLabel = rowLabel;
            this.colLabel = colLabel;
            this.grayValue1Label = grayValue1Label;
            this.grayValue2Label = grayValue2Label;
            this.grayValue3Label = grayValue3Label;
            this.hWindowControl1 = hWindowControl;
            initEvent();
        }

        private void initEvent()
        {
            this.hWindowControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hWindowControl1_MouseMove);
            ///////////
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.视图工具toolStrip_ItemClicked);

        }
  
        private void hWindowControl1_MouseMove(object sender, MouseEventArgs e)
        {
            HTuple value = -1.0;
            HalconLibrary ha = new HalconLibrary();
            if (this._objectDataModel == null) return;
            try
            {
                switch (this._objectDataModel.GetType().Name)
                {
                    case "HImage": //HObject
                        ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, (HImage)this._objectDataModel, e.Y, e.X, out mousePositionValue);
                        break;
                    case "ImageDataClass": //HObject
                        ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, ((ImageDataClass)this._objectDataModel).Image, e.Y, e.X, out mousePositionValue);
                        break;                       
                    case "HObjectModel3D": //HObject
                        ha.GetHeightValueOnWindow(this.hWindowControl1.HalconWindow, drawObject.CameraParam.CamParam.GetHtuple(), drawObject.CameraParam.CamPose.GetHtuple(), e.Y, e.X, out mousePositionValue);
                        break;
                }
                if (mousePositionValue.Length > 0)
                {
                    this.rowLabel.Text = mousePositionValue[0].D.ToString();
                    this.colLabel.Text = mousePositionValue[1].D.ToString();
                    this.grayValue1Label.Text = mousePositionValue[2].D.ToString();
                    this.grayValue2Label.Text = mousePositionValue[2].ToString();
                    this.grayValue3Label.Text = mousePositionValue[2].ToString();
                }
            }
            catch
            {

            }
        }
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            string name = e.ClickedItem.Name;
            switch (name)
            {
                //case "toolStripButton_Clear":
                //    this.drawObject.ClearWindow();
                //    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                //    break;
                //case "toolStripButton_Select":
                //    this.drawObject.Select();
                //    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                //    break;
                //case "toolStripButton_Translate":
                //    this.drawObject.TranslateScaleImage();
                //    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                //    break;
                //case "toolStripButton_Auto":
                //    this.drawObject.AutoImage();
                //    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                //    break;
                //case "toolStripButton_3D":
                //    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                //    break;
                //default:
                //    break;
            }
        }







    }
}
