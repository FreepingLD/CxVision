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


namespace View
{
    [Serializable]
    public class userDrawPointMeasure : DrawingBaseMeasure
    {
        private Line lineStruct;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        //private userWcsLine wcsLine;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        /// <summary>
        /// 可调大小节点的位置
        /// </summary>
        private enum PosSizableRect
        {
            StartRec,
            EndRec,
            MiddleRec,
            LineOver,
            Arrow,
            None,
        }

        public userDrawPointMeasure(HWindowControl hWindowControl) : base(hWindowControl)
        {

        }
        public userDrawPointMeasure(HWindowControl hWindowControl, userWcsLine wcsLine) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsLine != null && wcsLine.CamParams != null)// 等于null表示是第一次使用，否则不是第一次使用
            {
                //this.wcsLine = wcsLine;
                userPixLine pixLine = wcsLine.GetPixLine();
                this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col1, pixLine.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
            }
        }
        public userDrawPointMeasure(HWindowControl hWindowControl, userPixLine pixLine) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixLine != null && pixLine.CamParams != null)// 等于null表示是第一次使用，否则不是第一次使用
            {
                //this.wcsLine = wcsLine;
                //userPixLine pixLine = wcsLine.GetPixLine();
                this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col1, pixLine.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
            }
        }
        public userDrawPointMeasure(HWindowControl hWindowControl, userWcsLine wcsLine,userWcsCoordSystem wcsCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsLine != null && wcsLine.CamParams != null)// 等于null表示是第一次使用，否则不是第一次使用
            {
                //this.wcsLine = wcsLine;
                this.wcsCoordSystem = wcsCoordSystem;
                userPixLine pixLine = wcsLine.AffineWcsLine2D(wcsCoordSystem.GetVariationHomMat2D()).GetPixLine();
                this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col1, pixLine.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
            }
        }
        public userDrawPointMeasure(HWindowControl hWindowControl, userPixLine pixLine, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixLine != null && pixLine.CamParams != null)// 等于null表示是第一次使用，否则不是第一次使用
            {
                //this.wcsLine = pixLine;
                this.pixCoordSystem = pixCoordSystem;
                userPixLine pixLine2 = pixLine.AffinePixLine2D(pixCoordSystem.GetVariationHomMat2D());
                this.lineStruct = new Line(pixLine2.Row1, pixLine2.Col1, pixLine2.Row2, pixLine2.Col1, pixLine2.DiffRadius);
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
                    if (selectedNode == PosSizableRect.None || this.isDrwingObject == false)
                        this.isTranslate = true;
                    else
                        this.isTranslate = false;
                    /////////////
                    base.hWindowControl_HMouseDown(sender, e);
                    break;
                case MouseButtons.Middle:
                    //if (this.wcsLine != null)
                    //    this.wcsLine.Execute(null);
                    break;
            }
        }
        protected override void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            //UpdataMeasureRegion();
            base.hWindowControl_HMouseUp(sender, e);
        }
        protected override void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            base.hWindowControl_HMouseWheel(sender, e);
        }

        protected override void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            base.hWindowControl_HMouseMove(sender, e);
            if (!this.isDrwingObject) return;
            ChangeCursor(e.X, e.Y); // 在移动过程中改变光标
            /////////////////////////
            if (mIsClick == false || IsTranslate) // 只有在鼠标按下的状态下才执行移动
            {
                return;
            }
            ////////////////////
            switch (selectedNode)
            {
                case PosSizableRect.StartRec:
                    this.lineStruct.startPointCol += e.X - oldX;
                    this.lineStruct.startPointRow += e.Y - oldY;
                    break;

                case PosSizableRect.EndRec:
                    this.lineStruct.endPointCol += e.X - oldX;
                    this.lineStruct.endPointRow += e.Y - oldY;
                    break;

                case PosSizableRect.LineOver:
                case PosSizableRect.MiddleRec:
                    this.lineStruct.startPointCol += e.X - oldX;
                    this.lineStruct.startPointRow += e.Y - oldY;
                    this.lineStruct.endPointCol += e.X - oldX;
                    this.lineStruct.endPointRow += e.Y - oldY;
                    break;

                case PosSizableRect.Arrow:
                    double rowProj, colProj;
                    HMisc.ProjectionPl(e.Y, e.X, this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol, out rowProj, out colProj);
                    this.lineStruct.diffRadius += HMisc.DistancePp(e.Y, e.X, rowProj, colProj) - HMisc.DistancePp(this.oldY, this.oldX, rowProj, colProj);
                    break;
            }
            //上一点需要实时更新，这样一来图像才不会乱
            oldX = e.X;
            oldY = e.Y;
            //重绘图形
            if (this.mIsClick)
            {
                //UpdataMeasureRegion();
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
                    if (this.isDispalyAttachDrawingProperty)
                        this.AttachDrawingPropertyData.Add(GetSizableRect(pos));
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
                //case PosSizableRect.StartRec:
                //    return CreateRectSizableNode(this.lineStruct.startPointCol, this.lineStruct.startPointRow);

                //case PosSizableRect.EndRec:
                //    return CreateRectSizableNode(this.lineStruct.endPointCol, this.lineStruct.endPointRow);

                case PosSizableRect.LineOver:
                    return CreateLine();

                case PosSizableRect.None:
                    return GetArrowXLD();

                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double minDist, maxDist;
            HMisc.DistancePs(y, x, this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol, out minDist, out maxDist);
            double dist1 = Math.Sqrt((y - this.lineStruct.startPointRow) * (y - this.lineStruct.startPointRow) + (x - this.lineStruct.startPointCol) * (x - this.lineStruct.startPointCol));
            double dist2 = Math.Sqrt((y - this.lineStruct.endPointRow) * (y - this.lineStruct.endPointRow) + (x - this.lineStruct.endPointCol) * (x - this.lineStruct.endPointCol));

            //////////////////////////////////
            if (minDist < this.nodeSizeRect) // 表示移动
                selectNode = PosSizableRect.LineOver;
            //////////////////////////////////////////////////第二点
            if (dist1 < this.nodeSizeRect)
                selectNode = PosSizableRect.StartRec;
            //////////////////////////////////////////////////第三点
            if (dist2 < this.nodeSizeRect)
                selectNode = PosSizableRect.EndRec;

            if (Math.Abs(this.lineStruct.diffRadius) * 0.9 < minDist && minDist < Math.Abs(this.lineStruct.diffRadius) * 1.1)
                selectNode = PosSizableRect.Arrow;

            ////////////////////////////////////////////////// 第一点
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
                case PosSizableRect.StartRec:  //MovePos
                case PosSizableRect.EndRec:  //MovePos
                    return Cursors.Cross;
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
            public Line(double row1, double col1, double row2, double col2, double diffRadius)
            {
                this.startPointRow = row1;
                this.startPointCol = col1;
                this.endPointRow = row2;
                this.endPointCol = col2;
                this.diffRadius = diffRadius;
            }

        }

        public HTuple GetDrawingObjectParams()
        {
            return new HTuple(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol);
        }

        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            if (this.lineStruct.startPointRow == 0 && this.lineStruct.endPointRow == 0)
                this.lineStruct = new Line(row1 + 200, column1 + 200, row1 + 400, column1 + 200, this.nodeSizeRect);
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
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixLine):
                        if (!(param is userPixLine)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixLine 类型");
                        userPixLine pixLine = (userPixLine)param;
                        if (pixLine.CamParams != null)
                        {
                            //userPixLine pixLine = pixLine.GetPixLine();
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
                    case nameof(userWcsCoordSystem):
                        this.wcsCoordSystem = (userWcsCoordSystem)param;
                        this.pixCoordSystem = this.wcsCoordSystem.GetPixCoordSystem();
                        break;
                    case nameof(userPixCoordSystem):
                        this.pixCoordSystem = (userPixCoordSystem)param;
                        break;
                }
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
                this.wcsCoordSystem = new userWcsCoordSystem();
                this.pixCoordSystem = new userPixCoordSystem();
            }
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
            double phi = Math.Atan2((pixLine.startPointRow - pixLine.endPointRow) * -1, pixLine.startPointCol - pixLine.endPointCol);
            double normalPhi = phi - Math.PI * 0.5;
            ////////////////////////////////////////////
            double[] rows = new double[Num];
            double[] cols = new double[Num];
            for (int i = 0; i < Num; i++)
            {
                GetPointLine(pixLine.startPointRow, pixLine.startPointCol, pixLine.endPointRow, pixLine.endPointCol, i * dist1Mea, out rows[i], out cols[i]);
            }
            //
            GetNormalCoordPoint(rows, cols, normalPhi, pixLine.diffRadius, out Row1, out Col1, out Row2, out Col2);
        }
        public HXLDCont GetArrowXLD()
        {
            return GenArrowContourXld(this.lineStruct.startPointRow, this.lineStruct.startPointCol, (this.lineStruct.startPointRow + this.lineStruct.endPointRow) * 0.5,
                 (this.lineStruct.startPointCol + this.lineStruct.endPointCol) * 0.5, this.nodeSizeRect, this.nodeSizeRect);
        }
        private void UpdataMeasureRegion()
        {
           // return;
           // if (this.backImage == null ) return;
           // userPixLine LinePixPosition = new userPixLine(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol, this.backImage.CamParams);
           // LinePixPosition.diffRadius = this.lineStruct.diffRadius;
           // userWcsLine tempLine = LinePixPosition.GetWcsLine(this.backImage.Grab_X, this.backImage.Grab_Y).AffineWcsLine2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
           //if(this.wcsLine == null)
           // {
           //     this.wcsLine = tempLine;
           // }
           // else
           // {
           //     this.wcsLine.x1 = tempLine.x1;
           //     this.wcsLine.y1 = tempLine.y1;
           //     this.wcsLine.z1 = tempLine.z1;
           //     this.wcsLine.x2 = tempLine.x2;
           //     this.wcsLine.y2 = tempLine.y2;
           //     this.wcsLine.z2 = tempLine.z2;
           //     this.wcsLine.diffRadius = tempLine.diffRadius;
           // }
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

        public override userPixLine GetPixLineParam()
        {
            userPixLine LinePixPosition = new userPixLine(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol, this.backImage?.CamParams);
            LinePixPosition.DiffRadius = this.lineStruct.diffRadius;
            LinePixPosition = LinePixPosition.AffinePixLine2D(this.pixCoordSystem.GetInvertVariationHomMat2D());
            return LinePixPosition;
        }
        public override userWcsLine GetWcsLineParam()
        {
            userWcsLine LineWcsPosition = GetPixLineParam().GetWcsLine(this.backImage.Grab_X, this.backImage.Grab_Y).AffineWcsLine2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            return LineWcsPosition;
        }



    }





}
