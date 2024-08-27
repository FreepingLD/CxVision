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
        public MetrolegyParamForm()
        {
            InitializeComponent();
        }
        public MetrolegyParamForm(DrawingBaseMeasure drawingBaseClass)
        {
            InitializeComponent();
            this.drawObject = drawingBaseClass;
        }
        public MetrolegyParamForm(IFunction function, DrawingBaseMeasure drawingClass)
        {
            InitializeComponent();
            this._function = function;
            this.drawObject = drawingClass;
        }
        private void MetrolegyParamForm_Load(object sender, EventArgs e)
        {
            //TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(DisplayClickNodeResult);
            this.边缘选择comboBox.DataSource = Enum.GetNames(typeof(enEdgeSelect));
            this.极性选择comboBox.DataSource = Enum.GetNames(typeof(enEdgeTransition));
            //this.测量方向comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enMeasureDirection));
            Bind();
        }

        private void Bind()
        {
            if (this._function == null) return;
            switch (this._function.GetType().Name)
            {
                case "CircleMeasure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear(); //* this._dataPercent
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);                   
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);                    
                    break;
                case "CircleSectorMeasure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    break;
                case "EllipseMeasure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    break;
                case "EllipseSectorMeasure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    break;
                case "LineMeasure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    break;
                case "PointMeasure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    break;
                case "Rectangle2Measure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                    break;
                case "WidthMeasure":
                    this.边缘振幅texBox.DataBindings.Clear();
                    this.平滑系数textBox.DataBindings.Clear();
                    this.边缘选择comboBox.DataBindings.Clear();
                    this.极性选择comboBox.DataBindings.Clear();
                    this.无效数据填充comboBox.DataBindings.Clear();
                    this.数据百分比comboBox.DataBindings.Clear();
                    this.输出拟合点comboBox.DataBindings.Clear();
                    this.卡尺数量textBox.DataBindings.Clear();
                    this.卡尺宽度textBox.DataBindings.Clear();
                    this.边缘振幅texBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.平滑系数textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.边缘选择comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.极性选择comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                    //this.测量方向comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺数量textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.卡尺宽度textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.无效数据填充comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.数据百分比comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                    this.输出拟合点comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
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
                    case "CircleMeasure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.CircleGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.CircleGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.CircleGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.CircleGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.CircleGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((CircleMeasure)e.Node.Tag).FindCircle.CircleGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleMeasure)this._function).FindCircle.CircleGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (CircleMeasure)e.Node.Tag;
                        break;
                    case "CircleSectorMeasure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((CircleSectorMeasure)this._function).FindCircleSector.CircleSectorGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (CircleSectorMeasure)e.Node.Tag;
                        break;
                    case "EllipseMeasure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseMeasure)this._function).FindEllipse.EllipseGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (EllipseMeasure)e.Node.Tag;
                        break;
                    case "EllipseSectorMeasure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((EllipseSectorMeasure)this._function).FindEllipseSector.EllipseSectorGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (EllipseSectorMeasure)e.Node.Tag;
                        break;
                    case "LineMeasure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.LineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.LineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.LineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.LineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.LineGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((LineMeasure)e.Node.Tag).FindLine.LineGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((LineMeasure)this._function).FindLine.LineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (LineMeasure)e.Node.Tag;
                        break;
                    case "PointMeasure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.LineGeometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.LineGeometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.LineGeometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.LineGeometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.LineGeometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((PointMeasure)e.Node.Tag).FindPoint.LineGeometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((PointMeasure)this._function).FindPoint.LineGeometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (PointMeasure)e.Node.Tag;
                        break;
                    case "Rectangle2Measure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2Geometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2Geometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((Rectangle2Measure)this._function).FindRect2.Rect2Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (Rectangle2Measure)e.Node.Tag;
                        break;
                    case "WidthMeasure":
                        this.边缘振幅texBox.DataBindings.Clear();
                        this.平滑系数textBox.DataBindings.Clear();
                        this.边缘选择comboBox.DataBindings.Clear();
                        this.极性选择comboBox.DataBindings.Clear();
                        this.无效数据填充comboBox.DataBindings.Clear();
                        this.数据百分比comboBox.DataBindings.Clear();
                        this.输出拟合点comboBox.DataBindings.Clear();
                        this.卡尺数量textBox.DataBindings.Clear();
                        this.卡尺宽度textBox.DataBindings.Clear();
                        this.边缘振幅texBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Rect2Geometry, "Measure_threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.平滑系数textBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Rect2Geometry, "Measure_sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.边缘选择comboBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Rect2Geometry, "Measure_select", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.极性选择comboBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Rect2Geometry, "Measure_transition", true, DataSourceUpdateMode.OnPropertyChanged);
                        //this.测量方向comboBox.DataBindings.Add("Text", ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2Geometry, "Measure_direction", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺数量textBox.DataBindings.Add("Text", ((WidthMeasure)e.Node.Tag).FindWidth.Rect2Geometry, "Num_measures", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.卡尺宽度textBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "Measure_length2", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.无效数据填充comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "FillUpInvalidData", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.数据百分比comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "DataPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                        this.输出拟合点comboBox.DataBindings.Add("Text", ((WidthMeasure)this._function).FindWidth.Rect2Geometry, "IsOutFitPoint", true, DataSourceUpdateMode.OnPropertyChanged);
                        this._function = (WidthMeasure)e.Node.Tag;
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MetrolegyParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(DisplayClickNodeResult);
        }

        private void 确认button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._function == null) return;
                if (this.drawObject == null) return;
                switch (this._function.GetType().Name)
                {
                    case "CircleMeasure":
                        this._function.Execute(((userDrawCircleMeasure)this.drawObject).GetPixCircleParam());
                        break;
                    case "CircleSectorMeasure":
                        this._function.Execute(((userDrawCircleSectorMeasure)this.drawObject).GetPixCircleSectorParam());
                        break;
                    case "EllipseMeasure":
                        this._function.Execute(((userDrawEllipseMeasure)this.drawObject).GetPixEllipseParam());
                        break;
                    case "EllipseSectorMeasure":
                        this._function.Execute(((userDrawEllipseSectorMeasure)this.drawObject).GetPixEllipseSectorParam());
                        break;
                    case "LineMeasure":
                        this._function.Execute(((userDrawLineMeasure)this.drawObject).GetPixLineParam());
                        break;
                    case "PointMeasure":
                        this._function.Execute(((userDrawLineMeasure)this.drawObject).GetPixLineParam());
                        break;
                    case "Rectangle2Measure":
                        this._function.Execute(((userDrawRect2Measure)this.drawObject).GetPixRectangle2Param());
                        break;
                    case "WidthMeasure":
                        this._function.Execute(((userDrawWidthMeasure)this.drawObject).GetPixRectangle2Param());
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
