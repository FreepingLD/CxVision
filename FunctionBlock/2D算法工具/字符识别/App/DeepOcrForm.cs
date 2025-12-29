using Common;
using FunctionBlock;
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
using HalconDotNet;

namespace FunctionBlock
{
    public partial class DeepOcrForm : Form
    {
        private DeepOcrParam _param;
        private Ocr _ocr;
        //public VisualizeView drawObject;
        //public HWindowControl hWindowControl1;
        //public Ocr _Ocr;
        public DeepOcrForm(DeepOcrParam param,Ocr ocr)
        {
            InitializeComponent();
            this._param = param;
            this._ocr = ocr;
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {
                this.模型comboBox.DataSource = Enum.GetNames(typeof(enDeepOcrMode));
                this.模型comboBox.DataBindings.Add("Text", this._param, "DeepOcrMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.字符区域宽度comboBox.DataBindings.Add("Text", this._param, "RecognitionChartWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                ///this.字符区域高度comboBox.DataBindings.Add("Text", this._recParam, "RecognitionChartHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反图像checkBox.DataBindings.Add(nameof(this.取反图像checkBox.Checked), this._param, "InvertImage", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }


        private void CreateDeepOcrForm_Load(object sender, EventArgs e)
        {

        }

        private void 创建模型Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._ocr?.DeepOcr.CreateDeepOcrMode(this._param);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //int index = 0;
                    //PixROI pixShape;
                    //if (this._param.ReduceParam == null)
                    //    this._param.ReduceParam = new BindingList<ReduceParam>();
                    //BindingList<ReduceParam> listShape = this._param.ReduceParam;
                    //userPixCoordSystem pixCoordSystem = new userPixCoordSystem(); // this._param.PixCoordSystem;
                    //switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    //{
                    //    case "TeachCol":
                    //        if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                    //        {
                    //            new UserMessageForm().ShowDialog("未设置必需的图形参数，请先设置参数!");
                    //            return;
                    //        }
                    //        switch (listShape[e.RowIndex].ShapeType)
                    //        {
                    //            case enShapeType.矩形2:
                    //                this.drawObject.AttachPropertyData.Clear();
                    //                if (!(this.drawObject is userDrawRect2ROI))
                    //                {
                    //                    //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                    this.drawObject.ClearDrawingObject();
                    //                    this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                    //                    //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                }
                    //                break;
                    //            case enShapeType.矩形1:
                    //                this.drawObject.AttachPropertyData.Clear();
                    //                if (!(this.drawObject is userDrawRect1ROI))
                    //                {
                    //                    //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                    this.drawObject.ClearDrawingObject();
                    //                    this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                    //                    //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                }
                    //                break;
                    //            case enShapeType.圆:
                    //                this.drawObject.AttachPropertyData.Clear();
                    //                if (!(this.drawObject is userDrawCircleROI))
                    //                {
                    //                    //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                    this.drawObject.ClearDrawingObject();
                    //                    this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                    //                    //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                }
                    //                break;
                    //            case enShapeType.椭圆:
                    //                this.drawObject.AttachPropertyData.Clear();
                    //                if (!(this.drawObject is userDrawEllipseROI))
                    //                {
                    //                    //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                    this.drawObject.ClearDrawingObject();
                    //                    this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                    //                    //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                }
                    //                break;
                    //            case enShapeType.多边形:
                    //                this.drawObject.AttachPropertyData.Clear();
                    //                if (!(this.drawObject is userDrawPolygonROI))
                    //                {
                    //                    //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                    this.drawObject.ClearDrawingObject();
                    //                    this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                    //                    //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                }
                    //                break;
                    //            case enShapeType.点:
                    //                this.drawObject.AttachPropertyData.Clear();
                    //                if (!(this.drawObject is userDrawPointROI))
                    //                {
                    //                    //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                    this.drawObject.ClearDrawingObject();
                    //                    this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                    //                    //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                }
                    //                break;
                    //            case enShapeType.线:
                    //                this.drawObject.AttachPropertyData.Clear();
                    //                if (!(this.drawObject is userDrawLineROI))
                    //                {
                    //                    //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                    this.drawObject.ClearDrawingObject();
                    //                    this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                    //                    //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                    //                }
                    //                break;
                    //            default:
                    //                throw new NotImplementedException(listShape[e.RowIndex].ShapeType.ToString() + "未实现!");
                    //        }
                    //        this.drawObject.IsLiveState = true;
                    //        //////////////////////////
                    //        foreach (var item in listShape)
                    //        {
                    //            if (index != e.RowIndex && item.RoiShape != null)
                    //            {
                    //                this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                    //            }
                    //            index++;
                    //        }
                    //        if (this.drawObject.BackImage == null)
                    //            this.drawObject.BackImage = ((Ocr)this._function).ImageData;
                    //        if (listShape[e.RowIndex].RoiShape == null)
                    //            this.drawObject.SetParam(null);
                    //        else
                    //        {
                    //            this.drawObject.SetParam(pixCoordSystem);
                    //            this.drawObject.SetParam(listShape[e.RowIndex].RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()));
                    //        }
                    //        this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                    //        this.drawObject.AddViewObject(new ViewData(pixShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString())); //这里也要做下变换
                    //        /////////////////////////////////////////////////////////
                    //        listShape[e.RowIndex].RoiShape = pixShape;  // 这个地方的添加不能使用变换后数据
                    //        //////////////////////////////////////////////////////////
                    //        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                    //        {
                    //            this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                    //        }
                    //        break;
                    //    case "DeletCol":
                    //        if (listShape == null) return;
                    //        if (listShape.Count > e.RowIndex)
                    //            listShape.RemoveAt(e.RowIndex);
                    //        if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                    //            this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                    //        this.drawObject.DrawingGraphicObject();
                    //        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                    //        {
                    //            if (this.dataGridView1.Rows.Count > i)
                    //                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                    //        }
                    //        break;
                    //    default:
                    //        this.drawObject.AttachPropertyData.Clear();
                    //        foreach (var item in listShape)
                    //        {
                    //            if (item.RoiShape == null) return;
                    //            if (index == e.RowIndex)
                    //            {
                    //                this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.green.ToString()));
                    //            }
                    //            else
                    //            {
                    //                this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                    //            }
                    //            index++;
                    //        }
                    //        this.drawObject.DrawingGraphicObject();
                    //        break;
                    //}
                    //this.dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 字符提取button_Click(object sender, EventArgs e)
        {
            try
            {
                ExtractChartForm extractChart = new ExtractChartForm(this._param?.ExtractParam);
                extractChart.StartPosition = FormStartPosition.CenterParent;
                extractChart.Show();
                //new ExtractChartForm(this._param?.ExtractParam).Show();
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }


    }
}
