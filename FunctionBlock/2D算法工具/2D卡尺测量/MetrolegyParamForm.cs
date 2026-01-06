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

namespace FunctionBlock
{
    public partial class MetrolegyParamForm : Form
    {
        private IFunction _function;
        public DrawingBaseMeasure drawObject, drawBaseObject;
        private Form _form;
        public MetrolegyParamForm()
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
        }
        public MetrolegyParamForm(DrawingBaseMeasure drawingBaseClass)
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.drawObject = drawingBaseClass;
            this._form = null;
        }
        public MetrolegyParamForm(IFunction function, DrawingBaseMeasure drawingClass)
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this._function = function;
            this.drawObject = drawingClass;
            this._form = null;
        }
        public MetrolegyParamForm(IFunction function, Form form)
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this._function = function;
            this._form = form;
        }
        private void MetrolegyParamForm_Load(object sender, EventArgs e)
        {
            //TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(DisplayClickNodeResult);
            this.边缘选择comboBox.DataSource = Enum.GetNames(typeof(enEdgeSelect));
            this.极性选择comboBox.DataSource = Enum.GetNames(typeof(enEdgeTransition));
            this.矩形方向comboBox.DataSource = Enum.GetNames(typeof(enShapeDirection));
            this.滤波方法comboBox.DataSource = Enum.GetNames(typeof(enPointFilterMethod));
            //this.测量方向comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enMeasureDirection));
            Bind();
        }

        private void Bind()
        {
            if (this._function == null) return;
            switch (this._function.GetType().Name)
            {
                case nameof(CircleMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear(); //* this._dataPercent
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(CircleSectorMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(EllipseMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(EllipseSectorMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(LineMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(PointMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(Rectangle2Measure):
                    this.矩形方向comboBox.Enabled = true;
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    break;
                case nameof(WidthMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(CrossPointMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(PolyLineMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(PolygonMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(ManualPointMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(ManualPolygonMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(ManualCircleSectorMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                case nameof(ManualPolyLineMeasure):
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.矩形方向comboBox.DataBindings.Clear();
                    this.滤波方法comboBox.DataBindings.Clear();
                    this.滤波参数textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波方法comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.滤波参数textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.矩形方向comboBox.Enabled = false;
                    break;
                default:

                    break;
            }
        }
        public void DisplayClickNodeResult(object send, TreeNodeMouseClickEventArgs e)  //DisplayMetrolegyObjectEventArgs
        {
            if (e.Node.Tag == null) return;
            if (e.Button == MouseButtons.Right) return;
            try
            {
                if (!(e.Node.Tag is IFunction)) return;
                switch (e.Node.Tag.GetType().Name)
                {
                    case nameof(CircleMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (CircleMeasure)e.Node.Tag;
                        break;
                    case nameof(CircleSectorMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (CircleSectorMeasure)e.Node.Tag;
                        break;
                    case nameof(EllipseMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (EllipseMeasure)e.Node.Tag;
                        break;
                    case nameof(EllipseSectorMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (EllipseSectorMeasure)e.Node.Tag;
                        break;
                    case nameof(LineMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (LineMeasure)e.Node.Tag;
                        break;
                    case nameof(PointMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (PointMeasure)e.Node.Tag;
                        break;
                    case nameof(Rectangle2Measure):
                        this.矩形方向comboBox.Enabled = true;
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (Rectangle2Measure)e.Node.Tag;
                        break;
                    case nameof(WidthMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (WidthMeasure)e.Node.Tag;
                        break;
                    case nameof(CrossPointMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((CrossPointMeasure)this._function).FindCrossPoint.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (CrossPointMeasure)e.Node.Tag;
                        break;
                    case nameof(PolyLineMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((PolyLineMeasure)e.Node.Tag).FindPolyLine.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((PolyLineMeasure)e.Node.Tag).FindPolyLine.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((PolyLineMeasure)e.Node.Tag).FindPolyLine.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((PolyLineMeasure)e.Node.Tag).FindPolyLine.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((PolyLineMeasure)e.Node.Tag).FindPolyLine.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((PolyLineMeasure)this._function).FindPolyLine.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (PolyLineMeasure)e.Node.Tag;
                        break;
                    case nameof(PolygonMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((PolygonMeasure)e.Node.Tag).FindPolygon.Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((PolygonMeasure)e.Node.Tag).FindPolygon.Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((PolygonMeasure)e.Node.Tag).FindPolygon.Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((PolygonMeasure)e.Node.Tag).FindPolygon.Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((PolygonMeasure)e.Node.Tag).FindPolygon.Geometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((PolygonMeasure)this._function).FindPolygon.Geometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (PolygonMeasure)e.Node.Tag;
                        break;
                    case nameof(ManualPointMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((ManualPointMeasure)this._function).FindCrossPoint.LineGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (ManualPointMeasure)e.Node.Tag;
                        break;
                    case nameof(ManualPolygonMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((ManualPolygonMeasure)this._function).FindPolygon.PolygonGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (ManualPolygonMeasure)e.Node.Tag;
                        break;
                    case nameof(ManualCircleSectorMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((ManualCircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (ManualCircleSectorMeasure)e.Node.Tag;
                        break;
                    case nameof(ManualPolyLineMeasure):
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.矩形方向comboBox.DataBindings.Clear();
                        this.滤波方法comboBox.DataBindings.Clear();
                        this.滤波参数textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "CallipersCount", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波方法comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "PointFilterMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.矩形方向comboBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "ShapeDirection", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.滤波参数textBox.DataBindings.Add("Text", ((ManualPolyLineMeasure)this._function).FindPolyLine.PolyLineGeometry, "PointFilterParam", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.矩形方向comboBox.Enabled = false;
                        this._function = (ManualPolyLineMeasure)e.Node.Tag;
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void MetrolegyParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(DisplayClickNodeResult);
        }

        private void 示教button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._form != null)
                {
                    switch (this._form.GetType().Name)
                    {
                        case nameof(PointMeasureForm):
                            ((PointMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(PolyLineMeasureForm):
                            ((PolyLineMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(PolygonMeasureForm):
                            ((PolygonMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(Rect2MeasureForm):
                            ((Rect2MeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(WidthMeasureForm):
                            ((WidthMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(CrossPointMeasureForm):
                            ((CrossPointMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(EllipseMeasureForm):
                            ((EllipseMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(EllipseSectorMeasureForm):
                            ((EllipseSectorMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(CircleMeasureForm):
                            ((CircleMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(CircleSectorMeasureForm):
                            ((CircleSectorMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(LineMeasureForm):
                            ((LineMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualPointMeasureForm):
                            ((ManualPointMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualCircleSectorMeasureForm):
                            ((ManualCircleSectorMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualPolyLineMeasureForm):
                            ((ManualPolyLineMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualPolygonMeasureForm):
                            ((ManualPolygonMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                    }
                }
                else
                    this.drawObject?.AttachDrawingObjectToWindow();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 确认button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._function == null) return;
                switch (this._function.GetType().Name)
                {
                    case nameof(CircleMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawCircleMeasure)this.drawObject).GetPixCircleParam());
                        else
                            ((CircleMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(CircleSectorMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawCircleSectorMeasure)this.drawObject).GetPixCircleSectorParam());
                        else
                            ((CircleSectorMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(EllipseMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawEllipseMeasure)this.drawObject).GetPixEllipseParam());
                        else
                            ((EllipseMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(EllipseSectorMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawEllipseSectorMeasure)this.drawObject).GetPixEllipseSectorParam());
                        else
                            ((EllipseSectorMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(LineMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawLineMeasure)this.drawObject).GetPixLineParam());
                        else
                            ((LineMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(PointMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawPointMeasure)this.drawObject).GetPixLineParam());
                        else
                            ((PointMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(Rectangle2Measure):
                        if (this._form == null)
                            this._function.Execute(((userDrawRect2Measure)this.drawObject).GetPixRectangle2Param());
                        else
                            ((Rect2MeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(WidthMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawWidthMeasure)this.drawObject).GetPixRectangle2Param());
                        else
                            ((WidthMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(CrossPointMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawCrossMeasure)this.drawObject).GetPixLineParam());
                        else
                            ((CrossPointMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(PolyLineMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawPolyLineMeasure)this.drawObject).GetPixPolyLineParam());
                        else
                            ((PolyLineMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(PolygonMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawPolygonMeasure)this.drawObject).GetPixPolygonParam());
                        else
                            ((PolygonMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualPointMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualPointMeasure)this.drawObject).GetPixPointParam());
                        else
                            ((ManualPointMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualCircleSectorMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualCircleSectorMeasure)this.drawObject).GetPixCircleSectorParam());
                        else
                            ((ManualCircleSectorMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualPolyLineMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualPolyLineMeasure)this.drawObject).GetPixPolyLineParam());
                        else
                            ((ManualPolyLineMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualPolygonMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualPolygonMeasure)this.drawObject).GetPixPolygonParam());
                        else
                            ((ManualPolygonMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }




    }
}
