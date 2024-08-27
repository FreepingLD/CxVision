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
    public class userDrawCircleROI : VisualizeView
    {
        private userPixCircle pixCircle = new userPixCircle();
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        private userPixCoordSystem pixCoordSystem;
        /// <summary>
        /// 可调大小节点的位置
        /// </summary>
        private enum PosSizableRect
        {
            UpMiddleNode,
            DownMiddleNode,
            LeftBottomNode,
            RightBottomNode,
            None,
            InnerCircle,
            MiddleCircle,
            OutSideCircle,
            CircleCenter,
        }

        public userDrawCircleROI(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {

        }


        public userDrawCircleROI(HWindowControl hWindowControl, userPixCircle pixCircle, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            ////////////////////
            if (pixCircle != null && pixCircle.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCoordSystem = pixCoordSystem;
                this.pixCircle = pixCircle.AffineTransPixCircle(pixCoordSystem.GetVariationHomMat2D());
                this.pixCircle.DiffRadius = pixCircle.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
            }
        }

        public userDrawCircleROI(HWindowControl hWindowControl, userPixCircle pixCircle) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            ////////////////////
            if (pixCircle != null && pixCircle.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCircle = pixCircle;
                this.pixCircle.DiffRadius = pixCircle.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
            }
        }
        public override void SetParam(object param)
        {
            this.isDrawMode = true;
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixCircle):
                        if (!(param is userPixCircle)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixCircle 类型");
                        userPixCircle pixCircle = (userPixCircle)param;
                        if (pixCircle.CamParams != null)
                        {
                            this.pixCircle = pixCircle;
                            this.pixCircle.DiffRadius = pixCircle.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                            this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
                        }
                        break;
                    case nameof(userWcsCircle):
                        if (!(param is userWcsCircle)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsCircle 类型");
                        userWcsCircle wcsCircle = (userWcsCircle)param;
                        if (wcsCircle.CamParams != null)
                        {
                            this.pixCircle = wcsCircle.GetPixCircle();
                            this.pixCircle.DiffRadius = wcsCircle.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                            this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
                        }
                        break;
                    case nameof(drawPixCircle):
                        if (!(param is drawPixCircle)) throw new ArgumentException("给定的数据类型异常，需要传入 drawPixCircle 类型");
                        drawPixCircle PixCircle = (drawPixCircle)param;
                        if (this.CameraParam != null)
                        {
                            this.pixCircle = new userPixCircle(PixCircle.Row, PixCircle.Col, PixCircle.Radius, this.CameraParam);
                            this.pixCircle.DiffRadius = 25;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                            this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
                        }
                        break;
                    case nameof(drawWcsCircle):
                        if (!(param is drawWcsCircle)) throw new ArgumentException("给定的数据类型异常，需要传入 drawWcsCircle 类型");
                        drawWcsCircle WcsCircle = (drawWcsCircle)param;
                        if (this.CameraParam != null)
                        {
                            drawPixCircle pix = WcsCircle.GetPixCircle(this.CameraParam);
                            this.pixCircle = new userPixCircle(pix.Row, pix.Col, pix.Radius, this.CameraParam);
                            this.pixCircle.DiffRadius = 25;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                            this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
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
                this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
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
            roi = this.GetDrawPixCircleParam();
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
            roi = this.GetDrawPixCircleParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixCircleOnWindow(enColor color, out userPixCircle pixCircle)
        {            ////// 等待结束 //////////
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
            pixCircle = this.GetPixCircleParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixCircleOnWindow(enColor color, out drawPixCircle pixCircle)
        {            ////// 等待结束 //////////
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
            pixCircle = this.GetDrawPixCircleParam();
            this.DetachDrawingObjectFromWindow();
        }

        protected override void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    selectedNode = PosSizableRect.None; // 先让节点处于非选中状态
                    selectedNode = GetNodeSelectable(e.X, e.Y);
                    if (selectedNode == PosSizableRect.None || this.isDrawMode == false)
                        this.IsTranslate = true;
                    else
                        this.IsTranslate = false;
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
            //UpdataMeasureRegion(); // 在鼠标松开时，刷新一次数据
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
                case PosSizableRect.CircleCenter:
                    this.pixCircle.Row += e.Y-oldY;
                    this.pixCircle.Col += e.X-oldX;
                    break;
                case PosSizableRect.OutSideCircle:
                    //this.pixCircle.diffRadius = HMisc.DistancePp(this.pixCircle.row, this.pixCircle.col, e.Y, e.X) - this.pixCircle.radius;
                    break;
                case PosSizableRect.InnerCircle:
                    //this.pixCircle.diffRadius = this.pixCircle.radius - HMisc.DistancePp(this.pixCircle.row, this.pixCircle.col, e.Y, e.X);
                    break;
                case PosSizableRect.MiddleCircle:
                case PosSizableRect.RightBottomNode:
                case PosSizableRect.LeftBottomNode:
                case PosSizableRect.UpMiddleNode:
                case PosSizableRect.DownMiddleNode:
                    this.pixCircle.Radius = HMisc.DistancePp(this.pixCircle.Row, this.pixCircle.Col, e.Y, e.X);
                    break;

            }
            //上一点需要实时更新，这样一来图像才不会乱
            oldX = e.X;
            oldY = e.Y;
            //重绘图形
            if (mIsClick)
            {
                this.DrawingGraphicObject();
            }
        }


        /// <summary>
        /// 绘制整个绘图对象
        /// </summary>
        public override void DrawingGraphicObject()
        {
            this.AttachDrawingPropertyData.Clear();
            foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (this.isDrawMode)
                {
                    if (this.selectedNode == pos)
                        this.AttachDrawingPropertyData.Add(new ViewData(GetDrawingObject(pos), "green"));
                    else
                        this.AttachDrawingPropertyData.Add(new ViewData(GetDrawingObject(pos), "red"));
                }
                //this.AttachDrawingPropertyData.Add(GetDrawingObject(pos));
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont CreateCircleSectorObject(userPixCircle pixCircle)
        {
            HXLDCont xld = new HXLDCont();
            if (pixCircle.Radius > 0)
                xld.GenCircleContourXld(pixCircle.Row, pixCircle.Col, Math.Abs(pixCircle.Radius), 0, Math.PI * 2, "positive", 0.1);
            return xld;
        }


        private HXLDCont GetDrawingObject(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.RightBottomNode:
                    return CreateRectSizableNode(this.pixCircle.Col + pixCircle.Radius, this.pixCircle.Row);
                case PosSizableRect.UpMiddleNode:
                    return CreateRectSizableNode(this.pixCircle.Col, this.pixCircle.Row - pixCircle.Radius);
                case PosSizableRect.LeftBottomNode:
                    return CreateRectSizableNode(this.pixCircle.Col - pixCircle.Radius, this.pixCircle.Row);
                case PosSizableRect.DownMiddleNode:
                    return CreateRectSizableNode(this.pixCircle.Col, this.pixCircle.Row + pixCircle.Radius);
                //////////////////////////////////
                case PosSizableRect.MiddleCircle:
                    return CreateCircleSectorObject(this.pixCircle);
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect posSizable = PosSizableRect.None;
            double dist1 = Math.Sqrt((y - (this.pixCircle.Row - this.pixCircle.Radius)) * (y - (this.pixCircle.Row - this.pixCircle.Radius)) + (x - this.pixCircle.Col) * (x - this.pixCircle.Col));
            double dist2 = Math.Sqrt((y - (this.pixCircle.Row + this.pixCircle.Radius)) * (y - (this.pixCircle.Row + this.pixCircle.Radius)) + (x - this.pixCircle.Col) * (x - this.pixCircle.Col));
            double dist3 = Math.Sqrt((y - this.pixCircle.Row) * (y - this.pixCircle.Row) + (x - (this.pixCircle.Col - this.pixCircle.Radius)) * (x - (this.pixCircle.Col - this.pixCircle.Radius)));
            double dist4 = Math.Sqrt((y - this.pixCircle.Row) * (y - this.pixCircle.Row) + (x - (this.pixCircle.Col + this.pixCircle.Radius)) * (x - (this.pixCircle.Col + this.pixCircle.Radius)));
            //if (dist < (this.pixCircle.Radius) * 0.7) //- Math.Abs(this.pixCircle.diffRadius)
            //    return PosSizableRect.CircleCenter;
            if (this.pixCircle.GetXLD(this.nodeSizeRect).TestXldPoint(y, x) > 0)
                posSizable = PosSizableRect.CircleCenter;
            //////////////////////////////////////////////////
            if (dist1 <= this.nodeSizeRect)
                posSizable = PosSizableRect.UpMiddleNode;
            if (dist2 <= this.nodeSizeRect)
                posSizable = PosSizableRect.DownMiddleNode;
            if (dist3 <= this.nodeSizeRect)
                posSizable = PosSizableRect.LeftBottomNode;
            if (dist4 <= this.nodeSizeRect)
                posSizable = PosSizableRect.RightBottomNode;
            //if ((this.pixCircle.Radius) * 0.9 < dist && (this.pixCircle.Radius) * 1.1 > dist)
            //    posSizable = PosSizableRect.MiddleCircle;
            //////////////////////////////////////////////////
            return posSizable;
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
            Cursor cursors = Cursors.Default;
            switch (p)
            {
                case PosSizableRect.OutSideCircle:
                case PosSizableRect.MiddleCircle:  //MovePos
                case PosSizableRect.InnerCircle:  //MovePos
                    //return Cursors.SizeNWSE;//SizeNS
                    cursors = Cursors.Arrow;
                    break;

                case PosSizableRect.CircleCenter:
                    cursors = Cursors.Hand;
                    break;
                //return Cursors.Hand;

                case PosSizableRect.UpMiddleNode:
                case PosSizableRect.DownMiddleNode:
                    cursors = Cursors.Arrow;
                    break;
                //return Cursors.SizeNS;

                case PosSizableRect.LeftBottomNode:
                case PosSizableRect.RightBottomNode:
                    cursors = Cursors.Arrow;
                    break;
                //return Cursors.SizeWE;

                default:
                    break;
                    //return Cursors.Default;
            }
            return cursors;
        }

        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            if (this.pixCircle.Radius == 0)
                this.pixCircle = new userPixCircle(100 + row1, 100 + column1, 50);
            base.AttachDrawingObjectToWindow();
        }
        public override void DetachDrawingObjectFromWindow()
        {
            //this.pixCircle = new userPixCircle();
            base.DetachDrawingObjectFromWindow();
        }
        public override void ClearDrawingObject()
        {
            base.ClearDrawingObject();
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
                        //this.circleMeasure.Execute(null);
                        break;

                    case "自适应图像":
                        this.IsTranslate = false;
                        //this.isDrwingObject = true;
                        this.AutoImage();
                        break;

                    //////////////////////////////////////
                    case "平移/缩放":
                        this.IsTranslate = true;
                        //this.isDrwingObject = true;
                        break;

                    case "选择":
                        this.IsTranslate = false;
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

        public drawPixCircle GetDrawPixCircleParam()
        {
            drawPixCircle circlePixPosition = new drawPixCircle(this.pixCircle.Row, this.pixCircle.Col, this.pixCircle.Radius);
            circlePixPosition = circlePixPosition.AffinePixCircle(this.pixCoordSystem?.GetInvertVariationHomMat2D()); // 变换为之前的
            return circlePixPosition;
        }

        public override userPixCircle GetPixCircleParam()
        {
            userPixCircle circlePixPosition = new userPixCircle(this.pixCircle.Row, this.pixCircle.Col, this.pixCircle.Radius, this.CameraParam);
            circlePixPosition.DiffRadius = this.pixCircle.DiffRadius;
            circlePixPosition.NormalPhi = this.pixCircle.NormalPhi;
            circlePixPosition = circlePixPosition.AffineTransPixCircle(this.pixCoordSystem?.GetInvertVariationHomMat2D()); // 变换为之前的
            return circlePixPosition;
        }
        public override userWcsCircle GetWcsCircleParam()
        {
            //userWcsCircle circleWcsPosition = GetPixCircleParam().GetWcsCircle(this.backImage.Grab_X, this.backImage.Grab_Y).AffineWcsCircle2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            userWcsCircle circleWcsPosition = GetPixCircleParam().GetWcsCircle(0, 0);
            return circleWcsPosition;
        }

    }





}
