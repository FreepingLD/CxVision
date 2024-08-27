using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace View
{
    public class DrawingChartView : ViewBase
    {
        private Chart _chartView;  // 视图应该是独立的，可以用不同的视图来显示同一个数据也可以用不同的视图来显示不同的数据
        private ContextMenuStrip chartContextMenuStrip = new ContextMenuStrip();
        private double dataScaleFactor = 1;// 默认单位为mm
        public Chart ChartView { get => _chartView; set => _chartView = value; }
        public double DataScaleFactor { get => dataScaleFactor; set => dataScaleFactor = value; }

        public DrawingChartView(Chart chart, string name)
        {
            this._chartView = chart;
            this._chartView.Titles.Clear();
            this._chartView.Series.Clear();
            this._chartView.ChartAreas.Clear();
            this._chartView.Legends.Clear();
            //addContextMenu();
        }
        public DrawingChartView(Chart chart)
        {
            this._chartView = chart;
            this._chartView.Titles.Clear();
            this._chartView.Series.Clear();
            this._chartView.ChartAreas.Clear();
            this._chartView.Legends.Clear();
           // addContextMenu();
        }

        //添加右键菜单
        private void addContextMenu()
        {
            this.chartContextMenuStrip.Items.Add("显示距离");
            this.chartContextMenuStrip.Items.Add("显示厚度");
            this.chartContextMenuStrip.Items.Add("显示光强");
            this.chartContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            this._chartView.ContextMenuStrip = chartContextMenuStrip;
        }
        // 右键菜单项点击事件
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip context = sender as ContextMenuStrip;
            // 选择第二个选项:显示高度
            if (context.Items[0] == e.ClickedItem)
            {
                //this.viewName = "高度";
            }
            // 选择第二个选项:显示厚度
            if (context.Items[1] == e.ClickedItem)
            {
                //this.viewName = "厚度";
            }
            // 选择第二个选项:显示厚度
            if (context.Items[1] == e.ClickedItem)
            {
                //this.viewName = "光强";
            }
        }

        /// <summary>
        /// 添加系列对象,在chart中，系列是主角,LegendName:用来描述序列的
        /// </summary>
        /// <param name="count"></param>
        public void AddSerie(string SerieName, string LegendName, Color color, SeriesChartType type)
        {
            // 设置序列的一些参数
            this._chartView.Series.Add(SerieName);
            if (LegendName != null && this._chartView.Legends.IndexOf(LegendName) != -1)
                this._chartView.Series[SerieName].Legend = LegendName;
            this._chartView.Series[SerieName].ChartType = type;  // 数据线的类型
            this._chartView.Series[SerieName].MarkerSize = 1;
            this._chartView.Series[SerieName].IsVisibleInLegend = true;  // 不显示图例，这样可以扩大绘图的区域
            this._chartView.Series[SerieName].Color = color; // 数据线的颜色
        }

        /// <summary>
        /// 添加的图表区域应该包含坐标轴，同步来添加
        /// </summary>
        /// <param name="ChartAreaName"></param>
        /// <param name="XAxistitle"></param>
        /// <param name="YAxistitle"></param>
        public void AddChartArea(string ChartAreaName, string XAxistitle, string YAxistitle)
        {
            // 设置序列的一些参数
            this._chartView.ChartAreas.Add(ChartAreaName);
            // 允许划框来选择查看范围,并设置选择范围的颜色
            this._chartView.ChartAreas[ChartAreaName].CursorX.IsUserSelectionEnabled = true;
            this._chartView.ChartAreas[ChartAreaName].CursorY.IsUserSelectionEnabled = true;
            this._chartView.ChartAreas[ChartAreaName].CursorX.SelectionColor = Color.Red;
            this._chartView.ChartAreas[ChartAreaName].CursorY.SelectionColor = Color.Red;
            SetAxisParam(ChartAreaName, XAxistitle, YAxistitle);
        }
        public void SetChartTitle(string chartTitle)
        {
            // 设置序列的一些参数
            if (this._chartView != null)
                this._chartView.Titles.Add(new Title(chartTitle));
        }
        public void AddLegend(string LegendName, Docking docking, StringAlignment stringAlignment)
        {
            // 设置序列的一些参数
            if (LegendName != null && this._chartView.Legends.IndexOf(LegendName) == -1)
            {
                Legend legend = this._chartView.Legends.Add(LegendName);
                legend.Docking = docking;
                legend.Alignment = stringAlignment;
            }
        }


        /// <summary>
        /// 初始化图表区域
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="AxisXMaxValue"></param>
        /// <param name="AxisYMaxValue"></param>
        public void InitChartArea(string ChartAreaName, double AxisXMinValue, double AxisXMaxValue, double AxisYMinValue, double AxisYMaxValue)
        {
            // 设置两个轴的数据范围
            this._chartView.Invoke(new Action(() =>
            {
                if (this._chartView.ChartAreas.IndexOf(ChartAreaName) == -1) return; // 表示不包含该对象
                double maxValue = (Math.Abs(AxisYMinValue) >= Math.Abs(AxisYMaxValue)) ? Math.Abs(AxisYMinValue) : Math.Abs(AxisYMaxValue);
                double value = Math.Round(maxValue * this.dataScaleFactor) + 1;
                //double scale = Math.Round((AxisXMaxValue - AxisXMinValue) / (AxisYMaxValue - AxisYMinValue), 2);
                this._chartView.ChartAreas[ChartAreaName].AxisX.Minimum = Math.Round(AxisXMinValue * this.dataScaleFactor);
                this._chartView.ChartAreas[ChartAreaName].AxisX.Maximum = Math.Round(AxisXMaxValue * this.dataScaleFactor); //
                this._chartView.ChartAreas[ChartAreaName].AxisY.Minimum = Math.Round(AxisYMinValue);  //value * -1
                this._chartView.ChartAreas[ChartAreaName].AxisY.Maximum = Math.Round(AxisYMaxValue);
                //等比例绘制,只绘制 10 X 10 的网格
                this._chartView.ChartAreas[ChartAreaName].AxisY.Interval = Math.Round((AxisYMaxValue- AxisYMinValue) * 0.2, 2);
                this._chartView.ChartAreas[ChartAreaName].AxisX.Interval = Math.Round((AxisXMaxValue - AxisXMinValue) * 0.2, 2);//this._chartView.ChartAreas[ChartAreaName].AxisX.Maximum * 0.1; //*this.dataScaleFactor
                // 允许划框来选择查看范围,并设置选择范围的颜色
                this._chartView.ChartAreas[ChartAreaName].CursorX.IsUserSelectionEnabled = true;
                this._chartView.ChartAreas[ChartAreaName].CursorY.IsUserSelectionEnabled = true;
                this._chartView.ChartAreas[ChartAreaName].CursorX.SelectionColor = Color.Red;
                this._chartView.ChartAreas[ChartAreaName].CursorY.SelectionColor = Color.Red;
            }));
        }

        /// <summary>
        /// 初始化轴参数
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="XAxistitle"></param>
        /// <param name="YAxistitle"></param>
        private void SetAxisParam(string ChartAreaName, string XAxistitle, string YAxistitle)
        {
            if (ChartAreaName == null || ChartAreaName.Length == 0)
                ChartAreaName = "ChartArea1";
            if (this._chartView.ChartAreas.IndexOf(ChartAreaName) == -1) return;
            // 设置X/Y轴的参数
            this._chartView.ChartAreas[ChartAreaName].AxisX.ArrowStyle = AxisArrowStyle.Lines;
            this._chartView.ChartAreas[ChartAreaName].AxisY.ArrowStyle = AxisArrowStyle.Lines;
            this._chartView.ChartAreas[ChartAreaName].AxisX.Title = XAxistitle;
            this._chartView.ChartAreas[ChartAreaName].AxisY.Title = YAxistitle;
            this._chartView.ChartAreas[ChartAreaName].AxisX.TitleAlignment = StringAlignment.Center;
            this._chartView.ChartAreas[ChartAreaName].AxisY.TitleAlignment = StringAlignment.Center;
            this._chartView.ChartAreas[ChartAreaName].AxisX.TextOrientation = TextOrientation.Horizontal;
            this._chartView.ChartAreas[ChartAreaName].AxisY.TextOrientation = TextOrientation.Auto;
            this._chartView.ChartAreas[ChartAreaName].AxisY.LabelStyle.Angle = -0; // 轴标签的方向
            this._chartView.ChartAreas[ChartAreaName].AxisX.LineWidth = 1;
            this._chartView.ChartAreas[ChartAreaName].AxisY.LineWidth = 1;
            this._chartView.ChartAreas[ChartAreaName].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this._chartView.ChartAreas[ChartAreaName].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this._chartView.ChartAreas[ChartAreaName].AxisX.ScrollBar.ButtonColor = Color.Red;
            this._chartView.ChartAreas[ChartAreaName].AxisY.ScrollBar.ButtonColor = Color.Red;
            this._chartView.ChartAreas[ChartAreaName].AxisX.MajorGrid.LineColor = Color.DarkGray;
            this._chartView.ChartAreas[ChartAreaName].AxisY.MajorGrid.LineColor = Color.DarkGray;
        }


        /// <summary>
        /// 更新图表上的系列
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="SerieIndex"></param>
        /// <param name="Y"></param>
        public void UpDataChart(string ChartAreaName, string SerieName, double[] Y)
        {
            if (Y == null) return;
            ///////////////////////////////////
            double[] data_y = new double[Y.Length];
            for (int i = 0; i < Y.Length; i++)
            {
                data_y[i] = Y[i] * this.dataScaleFactor;
            }
            if (SerieName == null || SerieName.Length == 0)
            {
                this._chartView.Invoke(new Action(() =>
                {
                    this._chartView.Series[0].Points.DataBindY(data_y);
                }));
            }
            else
            {
                this._chartView.Invoke(new Action(() =>
                {
                    this._chartView.Series[SerieName].ChartArea = ChartAreaName;
                    this._chartView.Series[SerieName].Points.DataBindY(data_y);
                }
                ));
            }
        }

        /// <summary>
        /// 更新图表上的系列
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="SerieIndex"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void UpDataChart(string ChartAreaName, string SerieName, double[] X, double[] Y)
        {
            if (X == null || Y == null) return;
            if (X.Length != Y.Length) return;
            ////////////////////////////////////////
            double[] data_x = new double[Y.Length];
            double[] data_y = new double[Y.Length];
            //double offsetX = this._chartView.ChartAreas[ChartAreaName].AxisX.Minimum - X.Min();
            for (int i = 0; i < Y.Length; i++)
            {
                data_x[i] = (X[i]) * this.dataScaleFactor; //+ offsetX
                data_y[i] = Y[i] * this.dataScaleFactor;
            }
            /////////////////////////////////////////////
            if (SerieName == null || SerieName.Length == 0)
            {
                this._chartView.Invoke(new Action(() =>
                {
                    this._chartView.Series[0].Points.DataBindXY(data_x, data_y);
                }
                ));
            }
            else
            {
                this._chartView.Invoke(new Action(() =>
                {
                    this._chartView.Series[SerieName].ChartArea = ChartAreaName;
                    this._chartView.Series[SerieName].Points.DataBindXY(data_x, data_y);
                }
                ));
            }
        }





    }

}
