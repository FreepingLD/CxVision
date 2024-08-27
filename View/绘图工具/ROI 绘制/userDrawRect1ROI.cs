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

namespace View
{
    [Serializable]
    public class userDrawRect1ROI : VisualizeView
    {
        private Rect1 rect;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        private userPixCoordSystem pixCoordSystem;

        /// <summary>
        /// 可调大小节点的位置
        /// </summary>
        private enum PosSizableRect
        {
            LeftUpCorner,
            LeftDownCorner,
            RightUpCorner,
            RightDownCorner,
            UpMidlleCorner,
            DownMidlleCorner,
            LeftMidlleCorner,
            RightMidlleCorner,
            Rect1Inside,
            Rect2Over,
            Arrow,
            None,
            All,
        }

        public userDrawRect1ROI(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {
            this.CurrentButton = MouseButtons.None;
        }


        public userDrawRect1ROI(HWindowControl hWindowControl, userPixRectangle1 pixRect1) : base(hWindowControl)
        {
            this.CurrentButton = MouseButtons.None;
            if (pixRect1 != null && pixRect1.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.rect = new Rect1(pixRect1.Row1, pixRect1.Col1, pixRect1.Row1, pixRect1.Col2, pixRect1.Row2, pixRect1.Col2, pixRect1.Row2, pixRect1.Col1);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.rect = new Rect1(row1 + (row2 - row1) * 0.2,
                                      col1 + (col2 - col1) * 0.2,
                                      row1 + (row2 - row1) * 0.2,
                                      col1 + (col2 - col1) * 0.4,
                                      row1 + (row2 - row1) * 0.4,
                                       col1 + (col2 - col1) * 0.4,
                                      col1 + (col2 - col1) * 0.2,
                                      row1 + (row2 - row1) * 0.4);
            }
        }

        public override void SetParam(object param)
        {
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixRectangle1):
                        if (!(param is userPixRectangle1)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixRectangle1 类型");
                        userPixRectangle1 pixRect1 = (userPixRectangle1)param;
                        if (pixRect1.CamParams != null)
                        {
                            this.rect = new Rect1(pixRect1.Row1, pixRect1.Col1,
                                                 pixRect1.Row1, pixRect1.Col2,
                                                 pixRect1.Row2, pixRect1.Col2,
                                                 pixRect1.Row2, pixRect1.Col1);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.rect = new Rect1(row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.2,
                                                  row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.4,
                                                  row1 + (row2 - row1) * 0.4,
                                                  col1 + (col2 - col1) * 0.4,
                                                  row1 + (row2 - row1) * 0.4,
                                                  col1 + (col2 - col1) * 0.2);
                        }
                        this.isDrawMode = true;
                        break;
                    case nameof(userWcsRectangle1):
                        if (!(param is userWcsRectangle1)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsRectangle1 类型");
                        userWcsRectangle1 wcsRect1 = (userWcsRectangle1)param;
                        if (wcsRect1.CamParams != null)
                        {
                            pixRect1 = wcsRect1.GetPixRectangle1();//.GetPixRectangle2();
                            this.rect = new Rect1(pixRect1.Row1, pixRect1.Col1,
                                pixRect1.Row1, pixRect1.Col2,
                                pixRect1.Row2, pixRect1.Col2,
                                pixRect1.Row2, pixRect1.Col1);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.rect = new Rect1(row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.2,
                                                  row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.4,
                                                  row1 + (row2 - row1) * 0.4,
                                                   col1 + (col2 - col1) * 0.4,
                                                   row1 + (row2 - row1) * 0.4,
                                                  col1 + (col2 - col1) * 0.2);
                        }
                        this.isDrawMode = true;
                        break;

                    case nameof(drawPixRect1):
                        if (!(param is drawPixRect1)) throw new ArgumentException("给定的数据类型异常，需要传入 drawPixRect1 类型");
                        drawPixRect1 PixRect1 = (drawPixRect1)param;
                        if (this.CameraParam != null)
                        {
                            this.rect = new Rect1(PixRect1.Row1, PixRect1.Col1,
                                PixRect1.Row1, PixRect1.Col2,
                                PixRect1.Row2, PixRect1.Col2,
                                PixRect1.Row2, PixRect1.Col1);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.rect = new Rect1(row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.2,
                                                  row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.4,
                                                  row1 + (row2 - row1) * 0.4,
                                                   col1 + (col2 - col1) * 0.4,
                                                   row1 + (row2 - row1) * 0.4,
                                                  col1 + (col2 - col1) * 0.2);
                        }
                        this.isDrawMode = true;
                        break;
                    case nameof(drawWcsRect1):
                        if (!(param is drawWcsRect1)) throw new ArgumentException("给定的数据类型异常，需要传入 drawWcsRect1 类型");
                        drawWcsRect1 WcsRect1 = (drawWcsRect1)param;
                        if (this.CameraParam != null)
                        {
                            PixRect1 = WcsRect1.GetPixRect1(this.CameraParam);
                            this.rect = new Rect1(PixRect1.Row1, PixRect1.Col1,
                                PixRect1.Row1, PixRect1.Col2,
                                PixRect1.Row2, PixRect1.Col2,
                                PixRect1.Row2, PixRect1.Col1);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.rect = new Rect1(row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.2,
                                                  row1 + (row2 - row1) * 0.2,
                                                  col1 + (col2 - col1) * 0.4,
                                                  row1 + (row2 - row1) * 0.4,
                                                   col1 + (col2 - col1) * 0.4,
                                                   row1 + (row2 - row1) * 0.4,
                                                  col1 + (col2 - col1) * 0.2);
                        }
                        this.isDrawMode = true;
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
                this.rect = new Rect1(row1 + (row2 - row1) * 0.2,
                                      col1 + (col2 - col1) * 0.2,
                                      row1 + (row2 - row1) * 0.2,
                                      col1 + (col2 - col1) * 0.4,
                                      row1 + (row2 - row1) * 0.4,
                                       col1 + (col2 - col1) * 0.4,
                                       row1 + (row2 - row1) * 0.4,
                                      col1 + (col2 - col1) * 0.2);
                this.pixCoordSystem = new userPixCoordSystem();
                this.isDrawMode = true;
            }
        }
        public override void DrawPixRect1OnWindow(enColor color, out userPixRectangle1 pixRec1)
        {            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.isDrawMode = true;
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
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            pixRec1 = this.GetPixRectangle1Param();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixRect1OnWindow(enColor color, out drawPixRect1 pixRec1)
        {            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.isDrawMode = true;
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
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            pixRec1 = this.GetDrawPixRect1Param();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawRoiShapeOnWindow(enColor color, out ROI roi)
        {            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.isDrawMode = true;
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
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixRect1Param();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixRoiShapeOnWindow(enColor color, out PixROI roi)
        {            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.isDrawMode = true;
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
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixRect1Param();
            this.DetachDrawingObjectFromWindow();
        }

        protected override void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                case MouseButtons.Left:
                    this.CurrentButton = e.Button;
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
                    this.CurrentButton = e.Button;
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
                case PosSizableRect.LeftUpCorner:
                    this.rect.leftUpRow += e.Y - oldY;
                    this.rect.leftUpCol += e.X - oldX;
                    this.rect.leftDownCol += e.X - oldX;
                    this.rect.rightUpRow += e.Y - oldY;
                    break;
                case PosSizableRect.LeftDownCorner:
                    this.rect.leftDownRow += e.Y - oldY;
                    this.rect.leftDownCol += e.X - oldX;
                    this.rect.rightDownRow += e.Y - oldY;
                    this.rect.leftUpCol += e.X - oldX;
                    break;
                case PosSizableRect.RightUpCorner:
                    this.rect.rightUpRow += e.Y - oldY;
                    this.rect.rightUpCol += e.X - oldX;
                    this.rect.rightDownCol += e.X - oldX;
                    this.rect.leftUpRow += e.Y - oldY;
                    break;
                case PosSizableRect.RightDownCorner:
                    this.rect.rightDownRow += e.Y - oldY;
                    this.rect.rightDownCol += e.X - oldX;
                    this.rect.leftDownRow += e.Y - oldY;
                    this.rect.rightUpCol += e.X - oldX;
                    break;
                case PosSizableRect.Rect1Inside:
                    this.rect.leftUpRow += e.Y - oldY;
                    this.rect.leftUpCol += e.X - oldX;
                    this.rect.leftDownRow += e.Y - oldY;
                    this.rect.leftDownCol += e.X - oldX;
                    this.rect.rightUpRow += e.Y - oldY;
                    this.rect.rightUpCol += e.X - oldX;
                    this.rect.rightDownRow += e.Y - oldY;
                    this.rect.rightDownCol += e.X - oldX;
                    /////////////////////////////////
                    break;
            }
            //上一点需要实时更新，这样一来图像才不会乱
            oldX = e.X;
            oldY = e.Y;
            //重绘图形
            if (mIsClick)
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
                //this.AttachDrawingPropertyData.Add(GetSizableRect(pos));
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont CreateRect2Object()
        {
            HXLDCont xld = new HXLDCont();
            if (this.rect.leftDownCol != 0 && this.rect.rightDownCol != 0)
                xld.GenContourPolygonXld(new HTuple(this.rect.leftUpRow, this.rect.rightUpRow, this.rect.rightDownRow, this.rect.rightDownRow, this.rect.leftUpRow)
                                          , new HTuple(this.rect.leftUpCol, this.rect.rightUpCol, this.rect.rightUpCol, this.rect.leftDownCol, this.rect.leftUpCol));
            return xld;
        }
        private HXLDCont GetSizableRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.RightUpCorner:
                    return CreateRectSizableNode(this.rect.rightUpCol, this.rect.rightUpRow);

                case PosSizableRect.RightDownCorner:
                    return CreateRectSizableNode(this.rect.rightDownCol, this.rect.rightDownRow);

                case PosSizableRect.LeftDownCorner:
                    return CreateRectSizableNode(this.rect.leftDownCol, this.rect.leftDownRow);

                case PosSizableRect.LeftUpCorner:
                    return CreateRectSizableNode(this.rect.leftUpCol, this.rect.leftUpRow);

                case PosSizableRect.Rect1Inside:
                    return CreateRect2Object();

                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double dist = Math.Sqrt((y - (this.rect.leftUpRow + this.rect.leftDownRow) * 0.5) * (y - (this.rect.leftUpRow + this.rect.leftDownRow) * 0.5)
                                   + (x - (this.rect.leftUpCol + this.rect.rightUpCol) * 0.5) * (x - (this.rect.leftUpCol + this.rect.rightUpCol) * 0.5));
            double dist1 = Math.Sqrt((y - this.rect.leftUpRow) * (y - this.rect.leftUpRow) + (x - this.rect.leftUpCol) * (x - this.rect.leftUpCol));
            double dist2 = Math.Sqrt((y - this.rect.rightUpRow) * (y - this.rect.rightUpRow) + (x - this.rect.rightUpCol) * (x - this.rect.rightUpCol));
            double dist3 = Math.Sqrt((y - this.rect.rightDownRow) * (y - this.rect.rightDownRow) + (x - this.rect.rightDownCol) * (x - this.rect.rightDownCol));
            double dist4 = Math.Sqrt((y - this.rect.leftDownRow) * (y - this.rect.leftDownRow) + (x - this.rect.leftDownCol) * (x - this.rect.leftDownCol));
            /////////////////////////////////////////////////////////
            //if (dist < Math.Min(this.rect.rightDownRow - this.rect.rightUpRow, this.rect.rightDownCol - this.rect.leftDownCol) * 0.9) // 表示移动
            //    selectNode = PosSizableRect.Rect1Inside;
            if (this.rect.GetHXLD(this.nodeSizeRect).TestXldPoint(y, x) > 0)
                selectNode = PosSizableRect.Rect1Inside;
            //////////////////////////////////////////////////第二点
            if (dist1 < this.nodeSizeRect)
                selectNode = PosSizableRect.LeftUpCorner;
            //////////////////////////////////////////////////第二点
            if (dist2 < this.nodeSizeRect)
                selectNode = PosSizableRect.RightUpCorner;
            //////////////////////////////////////////////////第二点
            if (dist3 < this.nodeSizeRect)
                selectNode = PosSizableRect.RightDownCorner;
            //////////////////////////////////////////////////第二点
            if (dist4 < this.nodeSizeRect)
                selectNode = PosSizableRect.LeftDownCorner;
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
                case PosSizableRect.RightDownCorner: //SizeNWSE
                    return Cursors.Cross;

                case PosSizableRect.RightUpCorner:
                    return Cursors.Cross;

                case PosSizableRect.LeftDownCorner: //SizeNESW
                    return Cursors.Cross;

                case PosSizableRect.LeftUpCorner:
                    return Cursors.Cross;

                case PosSizableRect.Rect1Inside:
                    return Cursors.Hand;

                default:
                    return Cursors.Default;
            }
        }

        private struct Rect1
        {
            public double leftUpRow;
            public double leftUpCol;
            public double leftDownRow;
            public double leftDownCol;
            public double rightUpRow;
            public double rightUpCol;
            public double rightDownRow;
            public double rightDownCol;

            public Rect1(double leftUpRow, double leftUpCol, double rightUpRow, double rightUpCol, double rightDownRow, double rightDownCol, double leftDownRow, double leftDownCol)
            {
                this.leftUpRow = leftUpRow;
                this.leftUpCol = leftUpCol;
                this.leftDownRow = leftDownRow;
                this.leftDownCol = leftDownCol;
                this.rightUpRow = rightUpRow;
                this.rightUpCol = rightUpCol;
                this.rightDownRow = rightDownRow;
                this.rightDownCol = rightDownCol;
            }

            public HXLDCont GetHXLD(double nodeSize = 0)
            {
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenRectangle2ContourXld((this.leftUpRow + this.rightDownRow) * 0.5, (this.leftUpCol + this.rightDownCol) * 0.5, 0,
                     (this.rightDownRow - this.leftUpRow) * 0.5 - nodeSize, (this.rightDownCol - this.leftUpCol) * 0.5 - nodeSize);
                return hXLDCont;
            }

        }

        public override void AttachDrawingObjectToWindow()
        {
            int row1, col1, row2, col2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out col1, out row2, out col2);
            if (this.rect.leftUpCol == 0 && this.rect.rightUpCol == 0)
            {
                this.rect = new Rect1(row1 + (row2 - row1) * 0.2,
                                      col1 + (col2 - col1) * 0.2,
                                      row1 + (row2 - row1) * 0.2,
                                      col1 + (col2 - col1) * 0.4,
                                      row1 + (row2 - row1) * 0.4,
                                      col1 + (col2 - col1) * 0.4,
                                      col1 + (col2 - col1) * 0.2,
                                      row1 + (row2 - row1) * 0.4);
            }
            base.AttachDrawingObjectToWindow();
        }
        public override void DetachDrawingObjectFromWindow()
        {
            base.DetachDrawingObjectFromWindow();
        }
        public override void ClearDrawingObject()
        {
            base.ClearDrawingObject();
        }




        protected override void addContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(new ToolStripItem[5]
            {
                new ToolStripMenuItem("确认"),
                new ToolStripMenuItem("自适应图像"),
                new ToolStripMenuItem("平移/缩放"),
                new ToolStripMenuItem("选择"),
                new ToolStripMenuItem("清空")
            }); //, new ToolStripMenuItem("编辑绘图位置")
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
                    case "确认":
                        this.isTranslate = true;
                        this.DetachDrawingObjectFromWindow();
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
        public drawPixRect1 GetDrawPixRect1Param()
        {
            drawPixRect1 PixRectangle1 = new drawPixRect1(this.rect.leftUpRow, this.rect.leftUpCol, this.rect.rightDownRow, this.rect.rightDownCol);
            PixRectangle1 = PixRectangle1.AffineTransPixRect1(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            return PixRectangle1;
        }
        public userPixRectangle1 GetPixRectangle1Param()
        {
            userPixRectangle1 PixRectangle2 = new userPixRectangle1(this.rect.leftUpRow, this.rect.leftUpCol, this.rect.rightDownRow, this.rect.rightDownCol);
            PixRectangle2.CamParams = this.CameraParam;
            PixRectangle2 = PixRectangle2.AffineTransPixRect1(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            return PixRectangle2;
        }
        public override userWcsRectangle2 GetWcsRectangle2Param()
        {
            //userWcsRectangle2 WcsRectangle2 = GetPixRectangle2Param().GetWcsRectangle2(0, 0).AffineWcsRectangle2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            userWcsRectangle2 WcsRectangle2 = GetPixRectangle2Param().GetWcsRectangle2(0, 0);
            return WcsRectangle2;
        }

    }





}
