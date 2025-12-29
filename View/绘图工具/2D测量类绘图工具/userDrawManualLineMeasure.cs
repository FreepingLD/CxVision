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
//using AlgorithmsLibrary;


namespace View
{
    [Serializable]
    public class userDrawManualLineMeasure : DrawingBaseMeasure
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
            StartRect,
            EndRect,
            MiddleRect,
            LineOver,
            Arrow,
            None,
        }

        public userDrawManualLineMeasure(HWindowControl hWindowControl) : base(hWindowControl)
        {

        }
        public userDrawManualLineMeasure(HWindowControl hWindowControl, userWcsLine wcsLine, userWcsCoordSystem wcsCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsLine != null && wcsLine.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //this.wcsLine = wcsLine;
                this.wcsCoordSystem = wcsCoordSystem;
                userPixLine pixLine = wcsLine.AffineWcsLine2D(wcsCoordSystem.GetVariationHomMat2D()).GetPixLine();
                this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
                //this.wcsLine = wcsLine;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
            }
        }
        public userDrawManualLineMeasure(HWindowControl hWindowControl, userPixLine pixLine, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
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
        public userDrawManualLineMeasure(HWindowControl hWindowControl, userWcsLine wcsLine) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsLine != null && wcsLine.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                userPixLine pixLine = wcsLine.GetPixLine();
                this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
                //this.wcsLine = wcsLine;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.lineStruct = new Line(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, row1 + (row2 - row1) * 0.3, col1 + (col2 - col1) * 0.1, 20);
                this.lineStruct.diffRadius = HMisc.DistancePp(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol) * 0.1;
            }
        }
        public userDrawManualLineMeasure(HWindowControl hWindowControl, userPixLine pixLine) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (pixLine != null && pixLine.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //userPixLine pixLine = pixLine.GetPixLine();
                this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
                //this.wcsLine = pixLine;
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
           // UpdataMeasureRegion(); // 在鼠标松开时，刷新一次数据
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

                case PosSizableRect.Arrow:
                    double phi = Math.Atan2((e.Y - this.oldY) * 1, e.X - this.oldX);
                    if ((0.785 < phi && phi < 2.355) || (-2.355 < phi && phi < -0.785))
                        this.lineStruct.diffRadius += (HMisc.DistancePp(e.Y, e.X, this.oldY, this.oldX) * Math.Sin(phi));
                    else
                        this.lineStruct.diffRadius += (HMisc.DistancePp(e.Y, e.X, this.oldY, this.oldX) * Math.Cos(phi));
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
                    switch (pos)
                    {
                        case PosSizableRect.StartRect:
                            //
                            if (this.isDispalyAttachDrawingProperty)
                                this.AttachDrawingPropertyData.Add(new ViewData(GetSizableRect(pos), "yellow"));
                            break;
                        default:
                            if (this.isDispalyAttachDrawingProperty)
                                this.AttachDrawingPropertyData.Add(new ViewData(GetSizableRect(pos), "red")); 
                            break;
                    }
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

                case PosSizableRect.None:
                    //return GetArrowXLD();

                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double minDist = 100, maxDist;
            //HMisc.DistancePs(y, x, this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol, out minDist, out maxDist);
            minDist = HMisc.DistancePl(y, x, this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol);
            double midRow = (this.lineStruct.startPointRow + this.lineStruct.endPointRow) * 0.5;
            double midCol = (this.lineStruct.startPointCol + this.lineStruct.endPointCol) * 0.5;
            //double minDist = Math.Sqrt((y - midRow) * (y - midRow) + (x - midCol) * (x - midCol));
            double dist1 = Math.Sqrt((y - this.lineStruct.startPointRow) * (y - this.lineStruct.startPointRow) + (x - this.lineStruct.startPointCol) * (x - this.lineStruct.startPointCol));
            double dist2 = Math.Sqrt((y - this.lineStruct.endPointRow) * (y - this.lineStruct.endPointRow) + (x - this.lineStruct.endPointCol) * (x - this.lineStruct.endPointCol));

            //////////////////////////////////
            if (minDist < this.nodeSizeRect * 1) // 表示移动
                selectNode = PosSizableRect.LineOver;
            //////////////////////////////////////////////////第二点
            if (dist1 < this.nodeSizeRect)
                selectNode = PosSizableRect.StartRect;
            //////////////////////////////////////////////////第三点
            if (dist2 < this.nodeSizeRect)
                selectNode = PosSizableRect.EndRect;

            if (Math.Abs(this.lineStruct.diffRadius) - this.nodeSizeRect < minDist && minDist < Math.Abs(this.lineStruct.diffRadius) + this.nodeSizeRect)
                selectNode = PosSizableRect.None;

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

        public HTuple GetDrawingObjectParams()
        {
            return new HTuple(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol);
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
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixLine):
                        if (!(param is userPixLine)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixLine 类型");
                        userPixLine pixLine = (userPixLine)param;
                        //this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
                        if (pixLine.CamParams != null)
                        {
                            //userPixLine pixLine = pixLine.GetPixLine();
                            this.lineStruct = new Line(pixLine.Row1, pixLine.Col1, pixLine.Row2, pixLine.Col2, pixLine.DiffRadius);
                            //this.wcsLine = pixLine.GetWcsLine();
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
                            //this.wcsLine = pixLine.GetWcsLine();
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



        private void UpdataMeasureRegion()
        {
            //if (this.backImage == null && this.wcsLine == null) return;
            //userPixLine LinePixPosition = new userPixLine(this.lineStruct.startPointRow, this.lineStruct.startPointCol, this.lineStruct.endPointRow, this.lineStruct.endPointCol, this.backImage.CamParams);
            //LinePixPosition.diffRadius = this.lineStruct.diffRadius;
            //LinePixPosition.normalPhi = this.lineStruct.normalPhi;
            //this.wcsLine = LinePixPosition.GetWcsLine(this.backImage.Grab_X, this.backImage.Grab_Y).Affine2DWcsLine(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
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
            LinePixPosition = LinePixPosition.AffinePixLine2D(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            LinePixPosition.DiffRadius = this.lineStruct.diffRadius;
            LinePixPosition.NormalPhi = this.lineStruct.normalPhi;
            return LinePixPosition;
        }
        public override userWcsLine GetWcsLineParam()
        {
            userWcsLine LineWcsPosition = GetPixLineParam().GetWcsLine(this.backImage.Grab_X, this.backImage.Grab_Y).AffineWcsLine2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            return LineWcsPosition;
        }

    }





}
