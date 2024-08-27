using AlgorithmsLibrary;
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
using Common;
using View;

namespace FunctionBlock
{
    public partial class ImageExtractLineParamForm : Form
    {
        private LineMeasure _lineMeasure;
        private DataGridView _dataGridView;
        private ImageDataClass _imageData;
        private userWcsLine fitLine;
        private userDrawLineMeasure drawObject;


        public ImageExtractLineParamForm(LineMeasure lineMeasure, userDrawLineMeasure drawLineObject, ImageDataClass imageData, DataGridView dataGridView)
        {
            InitializeComponent();
            this._lineMeasure = lineMeasure;
            this._dataGridView = dataGridView;
            this._imageData = imageData;
            this.drawObject = drawLineObject;
            DataInteractionClass.getInstance().MetrolegyComplete += new MetrolegyCompletedEventHandler(MetrolegyCompleted);
        }

        private void BindProperty()
        {
            try
            {
                //this.测量方向comboBox.DataSource = Enum.GetNames(typeof(enMeasureDirection));
                this.边缘选择comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enEdgeSelect));
                this.极性选择comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enEdgeTransition));
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                this.测量区域长度textBox.DataBindings.Add("Text", _lineMeasure.FindLine.LineGeometry, "Measure_length1", true, DataSourceUpdateMode.OnPropertyChanged);
                this.测量区域宽度textBox.DataBindings.Add("Text", _lineMeasure.FindLine.LineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                this.边缘振幅texBox.DataBindings.Add("Text", _lineMeasure.FindLine.LineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.平滑系数textBox.DataBindings.Add("Text", _lineMeasure.FindLine.LineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                this.边缘选择comboBox.DataBindings.Add("Text", _lineMeasure.FindLine.LineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性选择comboBox.DataBindings.Add("Text", _lineMeasure.FindLine.LineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.测量方向comboBox.DataBindings.Add("Text", _lineMeasure.LineGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                this.测量区域数量textBox.DataBindings.Add("Text", _lineMeasure.FindLine.LineGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void MetrolegyCompleted(MetrolegyCompletedEventArgs e)
        {
            if (e.EdgeData == null) return;
            switch (e.EdgeData.GetType().Name)
            {
                case "userWcsRectangle2":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsRect2);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsRect2.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsRectangle1":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsRect1);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsRect1.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsPoint":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsPoint wcsPoint = (userWcsPoint)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsPoint);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsPoint.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsLine":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsLine wcsLine = (userWcsLine)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsLine);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsLine.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsCircle":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsCircle wcsCircle = (userWcsCircle)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsCircle);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsCircle.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsCircleSector":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsCircleSector wcsCircleSector = (userWcsCircleSector)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsCircleSector);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsCircleSector.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsEllipse":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsEllipse wcsEllipse = (userWcsEllipse)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsEllipse);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsEllipse.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsEllipseSector":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsEllipseSector);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsEllipseSector.CamParams));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
            }
        }
        private void ImageExtractParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }
        private void UpdataMeasureRegion(object send, EventArgs e)
        {
            if (this.drawObject != null)
                this.drawObject.DrawingGraphicObject();
        }
        private void 应用测量button_Click(object sender, EventArgs e)
        {
            try
            {
                this._lineMeasure.FindLine.FindLineMethod(this._imageData, new userWcsCoordSystem(), out fitLine);
                this._dataGridView.Rows.Clear();
                this._dataGridView.Rows.Add(fitLine.X1, fitLine.Y1, fitLine.Z1, fitLine.X2, fitLine.Y2, fitLine.Z2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ImageExtractLineParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DataInteractionClass.getInstance().MetrolegyComplete -= new MetrolegyCompletedEventHandler(MetrolegyCompleted);
            }
            catch
            {

            }
        }


        public userWcsLine GetMeasureResult()
        {
            return this.fitLine;
        }

    }
}
