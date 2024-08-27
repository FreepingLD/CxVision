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
    public class userDrawPointROI : VisualizeView
    {
        private Cross pointStruct;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
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
            StartRect2,
            EndRect2,
            MiddleRect2,
            CrossOver,
            Arrow,
            Arrow2,
            None,
        }

        public userDrawPointROI(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {

        }

        public userDrawPointROI(HWindowControl hWindowControl, userPixPoint pixPoint, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixPoint != null && pixPoint.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCoordSystem = pixCoordSystem;
                userPixPoint PixPoint = pixPoint.AffineTransPixPoint(pixCoordSystem.GetVariationHomMat2D());
                this.pointStruct = new Cross(PixPoint.Row, PixPoint.Col);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1);
            }
        }
        public userDrawPointROI(HWindowControl hWindowControl, userWcsPoint wcsPoint) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsPoint != null && wcsPoint.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                userPixPoint pixPoint = wcsPoint.GetPixPoint();
                this.pointStruct = new Cross(pixPoint.Row, pixPoint.Col);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1);
            }
        }
        public userDrawPointROI(HWindowControl hWindowControl, userPixPoint pixPoint) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (pixPoint != null && pixPoint.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pointStruct = new Cross(pixPoint.Row, pixPoint.Col);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1);
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
                case MouseButtons.Middle:
                    //if (this.wcsLine != null)
                    //    this.wcsLine.Execute(null);
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
                case PosSizableRect.CrossOver:
                case PosSizableRect.MiddleRect:
                    this.pointStruct.Col += e.X - oldX;
                    this.pointStruct.Row += e.Y - oldY;
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
            ///////////////// 将图中所有节点位置的矩形绘制出来 //////////////////////
            foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (this.isDrawMode)
                {
                    switch(this.selectedNode)
                    {
                        case PosSizableRect.MiddleRect:
                            this.AttachDrawingPropertyData.Add( new ViewData( GetSizableRect(pos),"green"));
                            break;
                        default:
                            this.AttachDrawingPropertyData.Add(new ViewData(GetSizableRect(pos), "red"));
                            break;
                    }
                }
                    //this.AttachDrawingPropertyData.Add(GetSizableRect(pos));
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont CreateCross()
        {
            HXLDCont cross = new HXLDCont();
            cross.GenCrossContourXld(this.pointStruct.Row, this.pointStruct.Col, this.nodeSizeRect * 5, 0);
            return cross;
        }
        private HXLDCont GetSizableRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.MiddleRect:
                    return CreateRectSizableNode(this.pointStruct.Col, this.pointStruct.Row);
                case PosSizableRect.CrossOver:
                    return CreateCross();
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double dist = Math.Sqrt((y - this.pointStruct.Row) * (y - this.pointStruct.Row) + (x - this.pointStruct.Col) * (x - this.pointStruct.Col));
            //////////////////////////////////////////////////第二点
            if (dist < this.nodeSizeRect)
                selectNode = PosSizableRect.MiddleRect;
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
                case PosSizableRect.MiddleRect:
                case PosSizableRect.CrossOver:
                    return Cursors.Hand;
                ///////////////////////////////
                default:
                    return Cursors.Default;
            }
        }

        private struct Cross
        {
            public double Row;
            public double Col;
            public double Rad;
            public double Size;
            public Cross(double row, double col)
            {
                this.Row = row;
                this.Col = col;
                this.Rad = 0;
                this.Size = 15;
            }
            public Cross(double row, double col, double size)
            {
                this.Row = row;
                this.Col = col;
                this.Rad = 0;
                this.Size = size;
            }
        }


        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            this.pointStruct = new Cross(row1 + 200, column1 + 200, this.nodeSizeRect);
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
        public override void SetParam(object param)
        {
            this.isDrawMode = true;
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixPoint):
                        if (!(param is userPixPoint)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixPoint 类型");
                        userPixPoint pixPoint = (userPixPoint)param;
                        if (pixPoint.CamParams != null)
                        {
                            this.pointStruct = new Cross(pixPoint.Row, pixPoint.Col, this.nodeSizeRect);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, this.nodeSizeRect);
                        }
                        break;
                    case nameof(userWcsPoint):
                        if (!(param is userWcsPoint)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsPoint 类型");
                        userWcsPoint wcsPoint = (userWcsPoint)param;
                        if (wcsPoint.CamParams != null)
                        {
                            pixPoint = wcsPoint.GetPixPoint();
                            this.pointStruct = new Cross(pixPoint.Row, pixPoint.Col, this.nodeSizeRect);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, this.nodeSizeRect);
                        }
                        break;

                    case nameof(drawPixPoint):
                        if (!(param is drawPixPoint)) throw new ArgumentException("给定的数据类型异常，需要传入 drawPixPoint 类型");
                        drawPixPoint PixPoint = (drawPixPoint)param;
                        if (this.CameraParam != null)
                        {
                            this.pointStruct = new Cross(PixPoint.Row, PixPoint.Col, this.nodeSizeRect);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, this.nodeSizeRect);
                        }
                        break;
                    case nameof(drawWcsPoint):
                        if (!(param is drawWcsPoint)) throw new ArgumentException("给定的数据类型异常，需要传入 drawWcsPoint 类型");
                        drawWcsPoint WcsPoint = (drawWcsPoint)param;
                        if (this.CameraParam != null)
                        {
                            PixPoint = WcsPoint.GetPixPoint(this.CameraParam);
                            this.pointStruct = new Cross(PixPoint.Row, PixPoint.Col, this.nodeSizeRect);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, this.nodeSizeRect); ;
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
                this.pointStruct = new Cross(row1 + (row2 - row1) * 0.1, col1 + (col2 - col1) * 0.1, this.nodeSizeRect);
                this.wcsCoordSystem = new userWcsCoordSystem();
                this.pixCoordSystem = new userPixCoordSystem();
            }
        }
        public override void DrawRoiShapeOnWindow(enColor color, out ROI roi)
        {
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
            roi = this.GetDrawPixPointParam();
            //pixPoint.Size = this.nodeSizeRect * 2;
            //roi = pixPoint;
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixRoiShapeOnWindow(enColor color, out PixROI roi)
        {
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
            roi = this.GetDrawPixPointParam();
            //pixPoint.Size = this.nodeSizeRect * 2;
            //roi = pixPoint;
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixPointOnWindow(enColor color, out userPixPoint pixPoint)
        {
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
            stopwatch.Stop(); ;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            pixPoint = this.GetPixPointParam();
            pixPoint.Size = this.nodeSizeRect * 2;
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixPointOnWindow(enColor color, out drawPixPoint pixPoint)
        {
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
            pixPoint = this.GetDrawPixPointParam();
            this.DetachDrawingObjectFromWindow();
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

        public  drawPixPoint GetDrawPixPointParam()
        {
            drawPixPoint LinePixPosition = new drawPixPoint(this.pointStruct.Row, this.pointStruct.Col);
            LinePixPosition = LinePixPosition.AffinePixPoint(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            return LinePixPosition;
        }
        public override userPixPoint GetPixPointParam()
        {
            userPixPoint LinePixPosition = new userPixPoint(this.pointStruct.Row, this.pointStruct.Col, this.CameraParam);
            LinePixPosition.Size = this.nodeSizeRect*2;
            LinePixPosition = LinePixPosition.AffineTransPixPoint(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            return LinePixPosition;
        }
        public override userWcsPoint GetWcsPointParam()
        {
            userWcsPoint LineWcsPosition = GetPixPointParam().GetWcsPoint(this.BackImage.Grab_X, this.BackImage.Grab_Y); //.AffineTransPixPoint(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            return LineWcsPosition;
        }

    }





}
