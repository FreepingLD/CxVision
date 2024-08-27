using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.Diagnostics;
//using AlgorithmsLibrary;


namespace View
{
    [Serializable]
    public class userDrawLineROI : VisualizeView
    {
        private Line lineStruct;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        private userPixCoordSystem pixCoordSystem;

        /// <summary>
        /// 可调大小节点的位置
        /// </summary>
        private enum PosSizableRect
        {
            StartRect,
            EndRect,
            MiddleRect,
            LineOver,
            Arrow,
            None,
        }

        public userDrawLineROI(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {

        }

        public userDrawLineROI(HWindowControl hWindowControl, userPixLine pixLine, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixLine != null && pixLine.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCoordSystem = pixCoordSystem;
                userPixLine pixLine2 = pixLine.AffinePixLine2D(pixCoordSystem.GetVariationHomMat2D());
                this.lineStruct = new Line(pixLine2.Row1, pixLine2.Col1, pixLine2.Row2, pixLine2.Col2, pixLine2.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
            }
        }

        public userDrawLineROI(HWindowControl hWindowControl, userPixLine pixLine) : base(hWindowControl)
        {
            if (pixLine != null && pixLine.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
            }
        }
        protected override void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    selectedNode = PosSizableRect.None; // 先让节点处于非选中状态
                    selectedNode = GetNodeSelectable(e.X, e.Y);
                    ////// 判断是平移还是移动对象
                    if (selectedNode == PosSizableRect.None || this.isDrawMode == false)
                        this.isTranslate = true;
                    else
                        this.isTranslate = false;
                    /////////////
                    base.hWindowControl_HMouseDown(sender, e);
                    break;
                case MouseButtons.Right:
                    this.CurrentButton = MouseButtons.Right;
                    break;
            }
        }
        protected override void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            base.hWindowControl_HMouseUp(sender, e);
        }
        protected override void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            base.hWindowControl_HMouseWheel(sender, e);
        }

        protected override void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            base.hWindowControl_HMouseMove(sender, e);
            if (!this.isDrawMode) return;
            ChangeCursor(e.X, e.Y); // 在移动过程中改变光标
            /////////////////////////
            if (mIsClick == false || IsTranslate) // 只有在鼠标按下的状态下才执行移动
            {
                return;
            }
            ////////////////////
            switch (selectedNode)
            {
                case PosSizableRect.StartRect:
                    this.lineStruct.startPointCol += e.X - oldX;
                    this.lineStruct.startPointRow += e.Y - oldY;
                    break;

                case PosSizableRect.EndRect:
                    this.lineStruct.endPointCol += e.X - oldX;
                    this.lineStruct.endPointRow += e.Y - oldY;
                    break;

                case PosSizableRect.LineOver:
                case PosSizableRect.MiddleRect:
                    this.lineStruct.startPointCol += e.X - oldX;
                    this.lineStruct.startPointRow += e.Y - oldY;
                    this.lineStruct.endPointCol += e.X - oldX;
                    this.lineStruct.endPointRow += e.Y - oldY;
                    break;
            }
            //上一点需要实时更新，这样一来图像才不会乱
            oldX = e.X;
            oldY = e.Y;
            //重绘图形
            if (this.mIsClick)
            {
                DrawingGraphicObject();
            }
        }


        /// <summary>
        /// 绘制整个绘图对象
        /// </summary>
        public override void DrawingGraphicObject()
        {
            this.AttachDrawingPropertyData.Clear();
            if (this.lineStruct.endPointRow != 0)
            {
                ///////////////// 将图中所有节点位置的矩形绘制出来 //////////////////////
                foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
                {
                    if (this.isDrawMode)
                    {
                        if (this.selectedNode == pos)
                            this.AttachDrawingPropertyData.Add(new ViewData(GetSizableRect(pos), "green"));
                        else
                            this.AttachDrawingPropertyData.Add(new ViewData(GetSizableRect(pos), "red"));
                    }
                    // this.AttachDrawingPropertyData.Add(GetSizableRect(pos));
                }
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont CreateLine()
        {
            HXLDCont rect = new HXLDCont();
            rect.GenContourPolygonXld(new HTuple(this.lineStruct.startPointRow, this.lineStruct.endPointRow), new HTuple(this.lineStruct.startPointCol, this.lineStruct.endPointCol));
            return rect;
        }
        private HXLDCont GetSizableRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.StartRect:
                    return CreateRectSizableNode(this.lineStruct.startPointCol, this.lineStruct.startPointRow);
                case PosSizableRect.EndRect:
                    return CreateRectSizableNode(this.lineStruct.endPointCol, this.lineStruct.endPointRow);
                case PosSizableRect.MiddleRect:
                    return CreateRectSizableNode((this.lineStruct.endPointCol + this.lineStruct.startPointCol) * 0.5, (this.lineStruct.endPointRow + this.lineStruct.startPointRow) * 0.5);
                case PosSizableRect.LineOver:
                    return CreateLine();
                //case PosSizableRect.None:
                //    return GetArrowXLD();
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double minDist, maxDist;
            minDist = HMisc.DistancePl(y, x, this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol);
            //double midRow = (this.lineStruct.startPointRow + this.lineStruct.endPointRow) * 0.5;
            //double midCol = (this.lineStruct.startPointCol + this.lineStruct.endPointCol) * 0.5;
            //double midDist = Math.Sqrt((y - midRow) * (y - midRow) + (x - midCol) * (x - midCol));
            double dist1 = Math.Sqrt((y - this.lineStruct.startPointRow) * (y - this.lineStruct.startPointRow) + (x - this.lineStruct.startPointCol) * (x - this.lineStruct.startPointCol));
            double dist2 = Math.Sqrt((y - this.lineStruct.endPointRow) * (y - this.lineStruct.endPointRow) + (x - this.lineStruct.endPointCol) * (x - this.lineStruct.endPointCol));
            //////////////////////////////////
            if (minDist < this.nodeSizeRect) // 表示移动
                selectNode = PosSizableRect.LineOver;
            //////////////////////////////////////////////////第二点
            if (dist1 < this.nodeSizeRect)
                selectNode = PosSizableRect.StartRect;
            //////////////////////////////////////////////////第三点
            if (dist2 < this.nodeSizeRect)
                selectNode = PosSizableRect.EndRect;
            return selectNode;
        }
        private void ChangeCursor(double x, double y)
        {
            this.hWindowControl.Cursor = GetCursor(GetNodeSelectable(x, y));
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Cursor GetCursor(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LineOver:
                    return Cursors.Hand;
                ///////////////////////////////
                case PosSizableRect.StartRect:  //MovePos
                case PosSizableRect.EndRect:  //MovePos
                    return Cursors.Default;
                default:
                    return Cursors.Default;
            }
        }

        private struct Line
        {
            public double startPointRow;
            public double startPointCol;
            public double endPointRow;
            public double endPointCol;

            public double diffRadius;
            public double normalPhi;
            public Line(double row1, double col1, double row2, double col2, double diffRadius)
            {
                this.startPointRow = row1;
                this.startPointCol = col1;
                this.endPointRow = row2;
                this.endPointCol = col2;
                this.diffRadius = diffRadius;
                this.normalPhi = 0;
            }


        }


        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            if (this.lineStruct.startPointRow == 0 && this.lineStruct.endPointRow == 0)
                this.lineStruct = new Line(row1 + 200, column1 + 200, row1 + 400, column1 + 200, 20);
            base.AttachDrawingObjectToWindow();
        }
        public override void DetachDrawingObjectFromWindow()
        {
            //this.lineStruct = new Line();
            base.DetachDrawingObjectFromWindow();
        }
        public override void ClearDrawingObject()
        {
            base.ClearDrawingObject();
        }

        public override void SetParam(object param)
        {
            this.isDrawMode = true;
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixLine):
                        if (!(param is userPixLine)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixLine 类型");
                        userPixLine pixLine = (userPixLine)param;
                        if (pixLine.CamParams != null)
                        {
                            this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                            this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
                        }
                        break;
                    case nameof(userWcsLine):
                        if (!(param is userWcsLine)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsLine 类型");
                        userWcsLine wcsLine = (userWcsLine)param;
                        if (wcsLine.CamParams != null)
                        {
                            pixLine = wcsLine.GetPixLine();
                            this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                            this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
                        }
                        break;

                    case nameof(drawPixLine):
                        if (!(param is drawPixLine)) throw new ArgumentException("给定的数据类型异常，需要传入 drawPixLine 类型");
                        drawPixLine PixLine = (drawPixLine)param;
                        if (this.CameraParam != null)
                        {
                            this.lineStruct = new Line(PixLine.Row1, PixLine.Col1, PixLine.Row2, PixLine.Col2, 25);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                            this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
                        }
                        break;
                    case nameof(drawWcsLine):
                        if (!(param is drawWcsLine)) throw new ArgumentException("给定的数据类型异常，需要传入 drawWcsLine 类型");
                        drawWcsLine WcsLine = (drawWcsLine)param;
                        if (this.CameraParam != null)
                        {
                            PixLine = WcsLine.GetPixLine(this.CameraParam);
                            this.lineStruct = new Line(PixLine.Row1, PixLine.Col1, PixLine.Row2, PixLine.Col2, 25);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                            this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
                        }
                        break;
                    case nameof(userWcsCoordSystem):
                        if (param != null)
                            this.pixCoordSystem = ((userWcsCoordSystem)param).GetPixCoordSystem();
                        else
                            this.pixCoordSystem = new userPixCoordSystem();
                        break;
                    case nameof(userPixCoordSystem):
                        if (param != null)
                            this.pixCoordSystem = (userPixCoordSystem)param;
                        else
                            this.pixCoordSystem = new userPixCoordSystem();
                        break;
                }
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
                this.pixCoordSystem = new userPixCoordSystem();
            }
        }

        public override void DrawRoiShapeOnWindow(enColor color, out ROI roi)
        {
            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.DrawingGraphicObject();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                Application.DoEvents();
                if (MouseButtons.Right == this.CurrentButton || MouseButtons.None == this.CurrentButton) break;
                Thread.Sleep(10);
                if (stopwatch.ElapsedMilliseconds > 60 * 2 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixLineParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixRoiShapeOnWindow(enColor color, out PixROI roi)
        {
            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.DrawingGraphicObject();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                Application.DoEvents();
                if (MouseButtons.Right == this.CurrentButton || MouseButtons.None == this.CurrentButton) break;
                Thread.Sleep(10);
                if (stopwatch.ElapsedMilliseconds > 60 * 2 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixLineParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixLineOnWindow(enColor color, out userPixLine pixLine)
        {
            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.DrawingGraphicObject();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                Application.DoEvents();
                if (MouseButtons.Right == this.CurrentButton || MouseButtons.None == this.CurrentButton) break;
                Thread.Sleep(10);
                if (stopwatch.ElapsedMilliseconds > 60 * 2 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            pixLine = this.GetPixLineParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixLineOnWindow(enColor color, out drawPixLine pixLine)
        {
            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.DrawingGraphicObject();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                Application.DoEvents();
                if (MouseButtons.Right == this.CurrentButton || MouseButtons.None == this.CurrentButton) break;
                Thread.Sleep(10);
                if (stopwatch.ElapsedMilliseconds > 60 * 2 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            pixLine = this.GetDrawPixLineParam();
            this.DetachDrawingObjectFromWindow();
        }
        /// <summary>
        /// 获取直线上指定长度处的点坐标
        /// </summary>
        /// <param name="RowBegin"></param>
        /// <param name="ColumnBegin"></param>
        /// <param name="RowEnd"></param>
        /// <param name="ColumnEnd"></param>
        /// <param name="length"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        private void GetPointLine(double RowBegin, double ColumnBegin, double RowEnd, double ColumnEnd, double length, out double Row, out double Col)
        {
            HTuple row, col;
            //////////////////
            if (HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) == 0)
            {
                Row = RowBegin;
                Col = ColumnBegin;
            }
            else
            {
                HOperatorSet.IntersectionSegmentCircle(RowBegin, ColumnBegin, RowEnd, ColumnEnd, RowBegin, ColumnBegin, length, 0, Math.PI * 2, "positive", out row, out col);
                if (row.Length > 0)
                {
                    Row = row.D;
                    Col = col.D;
                }
                else
                {
                    Row = RowEnd;
                    Col = ColumnEnd;
                }
            }

        }

        /// <summary>
        /// 获取点的法向线坐标点
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="normalPhi"></param>
        /// <param name="normalLength"></param>
        /// <param name="Row1"></param>
        /// <param name="Col1"></param>
        /// <param name="Row2"></param>
        /// <param name="Col2"></param>
        private void GetNormalCoordPoint(double[] rows, double[] cols, double normalPhi, double normalLength, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            Row1 = new double[rows.Length];
            Col1 = new double[rows.Length];
            Row2 = new double[rows.Length];
            Col2 = new double[rows.Length];
            // 已知直线上的点坐标及该点的法线坐标，求法线直线的两点
            for (int i = 0; i < rows.Length; i++)
            {
                Row1[i] = rows[i] + normalLength * Math.Sin(normalPhi); // 因为像素坐标系与世界坐标系Y轴相反，所以这里Y也要用相反的操作符
                Col1[i] = cols[i] - normalLength * Math.Cos(normalPhi);
                Row2[i] = rows[i] - normalLength * Math.Sin(normalPhi);
                Col2[i] = cols[i] + normalLength * Math.Cos(normalPhi);
            }
        }
        private void GetArrowPoint(Line pixLine, int Num, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            double dist1Mea = HMisc.DistancePp(pixLine.startPointRow, pixLine.startPointCol, pixLine.endPointRow, pixLine.endPointCol) / (Num - 1);
            // = Math.Atan2((pixLine.startPointRow - pixLine.endPointRow) * -1, pixLine.startPointCol - pixLine.endPointCol);
            double normalPhi, phi;
            if (Math.Sqrt(pixLine.startPointRow * pixLine.startPointRow + pixLine.startPointCol * pixLine.startPointCol) < Math.Sqrt(pixLine.endPointRow * pixLine.endPointRow + pixLine.endPointCol * pixLine.endPointCol))
            {
                phi = Math.Atan2((pixLine.startPointRow - pixLine.endPointRow) * -1, pixLine.startPointCol - pixLine.endPointCol);
                normalPhi = phi - Math.PI * 0.5;
            }
            else
            {
                phi = Math.Atan2((pixLine.endPointRow - pixLine.startPointRow) * -1, pixLine.endPointCol - pixLine.startPointCol);
                normalPhi = phi + Math.PI * 0.5;
            }
            //phi = Math.Atan2((pixLine.endPointRow- pixLine.startPointRow) * -1,  pixLine.endPointCol- pixLine.startPointCol);
            //double normalPhi = phi - Math.PI * 0.5;
            ////////////////////////////////////////////
            double[] rows = new double[Num];
            double[] cols = new double[Num];
            for (int i = 0; i < Num; i++)
            {
                GetPointLine(pixLine.startPointRow, pixLine.startPointCol, pixLine.endPointRow, pixLine.endPointCol, i * dist1Mea, out rows[i], out cols[i]);
            }
            //
            GetNormalCoordPoint(rows, cols, normalPhi, pixLine.diffRadius, out Row1, out Col1, out Row2, out Col2);
            this.lineStruct.normalPhi = Math.Atan2((Row2[0] - Row1[0]) * -1, Col2[0] - Col1[0]); //以箭头的终点和起点来计算角度 
        }

        public HXLDCont GetArrowXLD()
        {
            double[] row1, col1, row2, col2;
            HXLDCont hXLD = new HXLDCont();
            switch (this.ShowMode)
            {
                default:
                case enShowMode.箭头:
                    GetArrowPoint(this.lineStruct, 5, out row1, out col1, out row2, out col2);
                    hXLD = GenArrowContourXld(row1, col1, row2, col2, this.nodeSizeRect, this.nodeSizeRect);
                    break;
                case enShowMode.矩形:
                    GetArrowPoint(this.lineStruct, 5, out row1, out col1, out row2, out col2);
                    for (int i = 0; i < row1.Length; i++)
                    {
                        HXLDCont rect2 = new HXLDCont();
                        rect2.GenRectangle2ContourXld((row1[i] + row2[i]) * 0.5, (col1[i] + col2[i]) * 0.5, this.lineStruct.normalPhi, this.lineStruct.diffRadius, 5);
                        hXLD = hXLD.ConcatObj(rect2);
                    }
                    break;
            }
            return hXLD;
        }



        protected override void addContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(new ToolStripItem[5] { new ToolStripMenuItem("验证"), new ToolStripMenuItem("自适应图像"), new ToolStripMenuItem("平移/缩放"), new ToolStripMenuItem("选择"), new ToolStripMenuItem("清空") }); //, new ToolStripMenuItem("编辑绘图位置")
            contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(toolStripContextMenuStrip_ItemClicked);
            this.hWindowControl.ContextMenuStrip = contextMenuStrip;
        }
        protected override void toolStripContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                switch (name)
                {
                    case "验证":
                        //this.wcsLine.Execute(null);
                        break;

                    case "自适应图像":
                        this.isTranslate = false;
                        //this.isDrwingObject = true;
                        this.AutoImage();
                        break;

                    //////////////////////////////////////
                    case "平移/缩放":
                        this.isTranslate = true;
                        //this.isDrwingObject = true;
                        break;

                    case "选择":
                        this.isTranslate = false;
                        //this.isDrwingObject = true;
                        break;

                    case "清除窗口":
                        this.hWindowControl.HalconWindow.ClearWindow();
                        this.BackImage = null;
                        this.AttachPropertyData.Clear();
                        this.AttachDrawingPropertyData.Clear();
                        break;

                    case "清空":
                        this.AttachDrawingPropertyData.Clear();
                        this.AutoImage();
                        break;

                    default:
                        break;
                }
            }
            catch
            {
            }
        }
        public drawPixLine GetDrawPixLineParam()
        {
            drawPixLine LinePixPosition = new drawPixLine(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol);
            LinePixPosition = LinePixPosition.AffinePixLine(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            return LinePixPosition;
        }

        public override userPixLine GetPixLineParam()
        {
            userPixLine LinePixPosition = new userPixLine(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol, this.CameraParam);
            LinePixPosition.DiffRadius = this.lineStruct.diffRadius;
            LinePixPosition.NormalPhi = this.lineStruct.normalPhi;
            LinePixPosition = LinePixPosition.AffinePixLine2D(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            return LinePixPosition;
        }
        public override userWcsLine GetWcsLineParam()
        {
            userWcsLine LineWcsPosition = GetPixLineParam().GetWcsLine(this.BackImage.Grab_X, this.BackImage.Grab_Y);//.AffineWcsLine2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            return LineWcsPosition;
        }

    }





}
