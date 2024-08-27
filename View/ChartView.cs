using Sensor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace View
{
    public class ChartView : ViewBase
    {
        private Chart _chart;  // 视图应该是独立的，可以用不同的视图来显示同一个数据也可以用不同的视图来显示不同的数据
        private string viewName;
        private string chartTitle = "距离";
        private string xAxisTitle = "坐标点";
        private string yAxisTitle = "距离值";
        private double xAxisMaxRange = 180;
        private double yAxisMaxRange = 5;
        private Color curveColor = Color.Red;
        private SeriesChartType curveType = SeriesChartType.Line;
        public ContextMenuStrip chartContextMenuStrip = new ContextMenuStrip();
        // 属性的本质是方法
        public string ChartTitle
        {
            get
            {
                return chartTitle;
            }

            set
            {
                chartTitle = value;
                InitChartArea(this.chartTitle, this.xAxisMaxRange, this.yAxisMaxRange);
            }
        }
        public string XAxisTitle
        {
            get
            {
                return xAxisTitle;
            }

            set
            {
                xAxisTitle = value;
                InitAxisParam(this.xAxisTitle, this.yAxisTitle);
            }
        }
        public string YAxisTitle
        {
            get
            {
                return yAxisTitle;
            }

            set
            {
                yAxisTitle = value;
                InitAxisParam( this.xAxisTitle, this.yAxisTitle);
            }
        }
        public double XAxisMaxRange
        {
            get
            {
                return xAxisMaxRange;
            }

            set
            {
                xAxisMaxRange = value;
                InitChartArea( this.chartTitle, this.xAxisMaxRange, this.yAxisMaxRange);
            }
        }
        public double YAxisMaxRange
        {
            get
            {
                return yAxisMaxRange;
            }

            set
            {
                yAxisMaxRange = value;
                InitChartArea(this.chartTitle, this.xAxisMaxRange, this.yAxisMaxRange);
            }
        } // 只有更改属性才会进入属性访问器
        public Color CurveColor
        {
            get
            {
                return curveColor;
            }

            set
            {
                curveColor = value;
            }
        }
        public SeriesChartType CurveType
        {
            get
            {
                return curveType;
            }

            set
            {
                curveType = value;
            }
        }

        public ChartView(Chart chart, string name)
        {
            this._chart = chart;
            this.viewName = name;
            addContextMenu();
        }

        public ChartView(Chart chart)
        {
            this._chart = chart;
            addContextMenu();
        }
        //添加右键菜单
        private void addContextMenu()
        {
            this.chartContextMenuStrip.Items.Add("显示距离");
            this.chartContextMenuStrip.Items.Add("显示厚度");
            this.chartContextMenuStrip.Items.Add("显示光强");
            this.chartContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            this._chart.ContextMenuStrip = chartContextMenuStrip;
        }
        // 右键菜单项点击事件
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip context = sender as ContextMenuStrip;
            // 选择第二个选项:显示高度
            if (context.Items[0] == e.ClickedItem)
            {
                this.viewName = "高度";
            }
            // 选择第二个选项:显示厚度
            if (context.Items[1] == e.ClickedItem)
            {
                this.viewName = "厚度";
            }
            // 选择第二个选项:显示厚度
            if (context.Items[1] == e.ClickedItem)
            {
                this.viewName = "光强";
            }
        }

        /// <summary>
        /// 添加指定数量的系列对象
        /// </summary>
        /// <param name="count"></param>
        private void AddSerie(Chart chart, int count)
        {
            for (int i = 0; i < count; i++)
            {
                chart.Series.Add(new Series());
            }
        }

        /// <summary>
        /// 初始化图表区域
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="AxisXMaxRange"></param>
        /// <param name="AxisYMaxRange"></param>
        private void InitChartArea(string title, double AxisXMaxRange, double AxisYMaxRange)
        {
            // 设置两个轴的数据范围
            this._chart.ChartAreas[0].AxisX.Minimum = 0;
            this._chart.ChartAreas[0].AxisX.Maximum = AxisXMaxRange;
            this._chart.ChartAreas[0].AxisY.Minimum = 0;
            this._chart.ChartAreas[0].AxisY.Maximum = AxisYMaxRange;
            //等比例绘制,只绘制 10 X 10 的风格
            this._chart.ChartAreas[0].AxisY.Interval = this._chart.ChartAreas[0].AxisY.Maximum / 1;
            this._chart.ChartAreas[0].AxisX.Interval = this._chart.ChartAreas[0].AxisX.Maximum / 10;
            Title ti = new Title(title);
            if (this._chart.Titles.Count < 1)
                this._chart.Titles.Add(ti);
            // 允许划框来选择查看范围,并设置选择范围的颜色
            this._chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            this._chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            this._chart.ChartAreas[0].CursorX.SelectionColor = Color.Red;
            this._chart.ChartAreas[0].CursorY.SelectionColor = Color.Red;
        }

        /// <summary>
        /// 初始化轴参数
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="XAxistitle"></param>
        /// <param name="YAxistitle"></param>
        private void InitAxisParam(string XAxistitle, string YAxistitle)
        {
            // 设置X/Y轴的参数
            this._chart.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Lines;
            this._chart.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Lines;
            this._chart.ChartAreas[0].AxisX.Title = XAxistitle;
            this._chart.ChartAreas[0].AxisY.Title = YAxistitle;
            this._chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
            this._chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
            this._chart.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            this._chart.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Auto;
            this._chart.ChartAreas[0].AxisY.LabelStyle.Angle = -90; // 轴标签的方向
            this._chart.ChartAreas[0].AxisX.LineWidth = 1;
            this._chart.ChartAreas[0].AxisY.LineWidth = 1;
            this._chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this._chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this._chart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.Red;
            this._chart.ChartAreas[0].AxisY.ScrollBar.ButtonColor = Color.Red;
            this._chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.DarkGray;
            this._chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.DarkGray;
        }

        /// <summary>
        /// 初始化图表上的系列
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="SerieIndex"></param>
        /// <param name="color"></param>
        /// <param name="type"></param>
        private void InitSeriesParam( int SerieIndex, Color color, SeriesChartType type)
        {
            // 设置序列的一些参数
            this._chart.Series[SerieIndex].ChartType = type;  // 数据线的类型
            this._chart.Series[SerieIndex].MarkerSize = 1;
            this._chart.Series[SerieIndex].IsVisibleInLegend = false;  // 不显示图例，这样可以扩大绘图的区域
            this._chart.Series[SerieIndex].Color = color; // 数据线的颜色
            // 初始化图表
            double[] X = new double[10];
            double[] Y = new double[10];
            this._chart.Series[SerieIndex].Points.DataBindXY(X, Y);
        }

        /// <summary>
        /// 更新图表数据
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void UpDataChart( int SerieIndex, IEnumerable X, IEnumerable Y)
        {
            if (Y == null || X==null) return;
            this._chart.Invoke(new Action(() => this._chart.Series[SerieIndex].Points.DataBindXY(X, Y)));
        }

        /// <summary>
        /// 更新图表上的系列
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="SerieIndex"></param>
        /// <param name="Y"></param>
        public void UpDataChart( int SerieIndex, IEnumerable Y)
        {
            if (Y == null) return;
            this._chart.Invoke(new Action(()=> this._chart.Series[SerieIndex].Points.DataBindY(Y)));
        }

        /// <summary>
        /// 更新图表上的系列
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="SerieIndex"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void UpDataChart(int SerieIndex, double[] X, double[] Y)
        {
            if (X == null && Y != null)
            {
                X = new double[Y.Length];
                for (int i = 0; i < Y.Length; i++)
                {
                    X[i] = i / 100.0f;
                }
            }
            this._chart.Invoke(new Action(() => this._chart.Series[SerieIndex].Points.DataBindXY(X, Y)));
        }

        public  void InitView()
        {
            // 初始化chart1
            // this.chart.Refresh(); 这一行是否需要？
            InitChartArea( this.chartTitle, this.xAxisMaxRange, this.yAxisMaxRange);
            InitAxisParam( this.xAxisTitle, "");
            InitSeriesParam( 0, CurveColor, CurveType);
        }


        #region 实现接口
        public override void DisplayObject(params object[] Object)
        {
            UpDataChart(Convert.ToInt32(Object[0]), (double[])Object[1]);
        }
        public override void DisplayObject(object Object1, object Object2)
        {
            UpDataChart( Convert.ToInt32(Object1), (double[])Object2);
        }
        public override void SetViewParam(enViewParamType paramType, object paramValue)
        {
            try
            {
                switch (paramType)
                {
                    case enViewParamType.图表标题:
                        this.ChartTitle = paramValue.ToString();
                        break;
                    case enViewParamType.X轴标题:
                        this.XAxisTitle = paramValue.ToString();
                        break;
                    case enViewParamType.Y轴标题:
                        this.YAxisTitle = paramValue.ToString();
                        break;
                    case enViewParamType.X轴最大范围:
                        this.XAxisMaxRange = double.Parse(paramValue.ToString());
                        break;
                    case enViewParamType.Y轴最大范围:
                        this.YAxisMaxRange = double.Parse(paramValue.ToString());
                        break;
                    case enViewParamType.曲线颜色:
                        this.CurveColor = (Color)paramValue;
                        break;
                    case enViewParamType.曲线类型:
                        this.CurveType = (SeriesChartType)paramValue;
                        break;
                    case enViewParamType.视图名字:
                        this.viewName = paramValue.ToString();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                throw new Exception();
            }
        }
        public override object GetViewParam(enViewParamType paramType)
        {
            try
            {
                // 只有更改属性才会进入属性访问器，所在这里只能用属性，属性是对字段的包装
                switch (paramType)
                {
                    case enViewParamType.图表标题:
                        return this.ChartTitle;

                    case enViewParamType.X轴标题:
                        return this.XAxisTitle;

                    case enViewParamType.Y轴标题:
                        return this.YAxisTitle;

                    case enViewParamType.X轴最大范围:
                        return this.XAxisMaxRange;

                    case enViewParamType.Y轴最大范围:
                        return this.YAxisMaxRange;

                    case enViewParamType.曲线颜色:
                        return this.CurveColor;

                    case enViewParamType.曲线类型:
                        return this.CurveType;

                    case enViewParamType.视图名字:
                        return this.viewName;

                    default:
                        return null;
                }
            }
            catch
            {
                throw new Exception();
            }
        }
        public override void InitView(object chartView)
        {
            // 初始化chart1
            // this.chart.Refresh(); 这一行是否需要？
            InitChartArea( this.chartTitle, this.xAxisMaxRange, this.yAxisMaxRange);
            InitAxisParam(this.xAxisTitle, "");
            InitSeriesParam(0, CurveColor, CurveType);
        }

        #endregion


    }

}
