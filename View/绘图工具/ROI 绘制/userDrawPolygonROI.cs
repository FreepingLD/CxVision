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
using System.ComponentModel;
using System.Diagnostics;

namespace View
{
    [Serializable]
    public class userDrawPolygonROI : VisualizeView //DrawingBaseMeasure
    {
        private PixPolygon2 pixPolygon = new PixPolygon2();
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        private userPixCoordSystem pixCoordSystem;

        public userDrawPolygonROI(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {

        }
        public userDrawPolygonROI(HWindowControl hWindowControl, userPixPolygon pixPolygon) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            ////////////////////
            if (pixPolygon != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixPolygon = new PixPolygon2(pixPolygon.Row.ToArray(), pixPolygon.Col.ToArray());
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                double centerRow = row1 + (row2 - row1) * 0.2;
                double centerCol = col1 + (col2 - col1) * 0.2;
                double width = (row2 - row1) * 0.06;
                //////////////////////////////////////////////////////////////////
                this.pixPolygon = new PixPolygon2(new double[] { centerRow - width, centerRow - width, centerRow + width, centerRow + width },
                    new double[] { centerCol - width, centerCol + width, centerCol + width, centerCol - width });
            }

        }


        public override void SetParam(object param)
        {
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixPolygon):
                        if (!(param is userPixPolygon)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixPolygon 类型");
                        userPixPolygon pixPolygon = (userPixPolygon)param;
                        if (pixPolygon.CamParams != null)
                        {
                            this.pixPolygon = new PixPolygon2(pixPolygon.Row.ToArray(), pixPolygon.Col.ToArray());
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolygon = new PixPolygon2(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4, row1 + (row2 - row1) * 0.4),
                                                             new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.2));
                        }
                        this.isDrawMode = true;
                        break;
                    case nameof(userWcsPolygon):
                        if (!(param is userWcsPolygon)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsPolygon 类型");
                        userWcsPolygon wcsPolygon = (userWcsPolygon)param;
                        if (wcsPolygon.CamParams != null)
                        {
                            pixPolygon = wcsPolygon.GetPixPolygon();
                            this.pixPolygon = new PixPolygon2(pixPolygon.Row.ToArray(), pixPolygon.Col.ToArray());
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolygon = new PixPolygon2(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4, row1 + (row2 - row1) * 0.4),
                                                new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.2));
                        }
                        this.isDrawMode = true;
                        break;

                    case nameof(drawPixPolygon):
                        if (!(param is drawPixPolygon)) throw new ArgumentException("给定的数据类型异常，需要传入 drawPixPolygon 类型");
                        drawPixPolygon PixPolygon = (drawPixPolygon)param;
                        if (this.CameraParam != null)
                        {
                            this.pixPolygon = new PixPolygon2(PixPolygon.Row.ToArray(), PixPolygon.Col.ToArray());
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolygon = new PixPolygon2(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4, row1 + (row2 - row1) * 0.4),
                                         new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.2));
                        }
                        this.isDrawMode = true;
                        break;
                    case nameof(drawWcsPolygon):
                        if (!(param is drawWcsPolygon)) throw new ArgumentException("给定的数据类型异常，需要传入 drawWcsPolygon 类型");
                        drawWcsPolygon WcsPolygon = (drawWcsPolygon)param;
                        if (this.CameraParam != null)
                        {
                            PixPolygon = WcsPolygon.GetPixPolygon(this.CameraParam);
                            this.pixPolygon = new PixPolygon2(PixPolygon.Row.ToArray(), PixPolygon.Col.ToArray());
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolygon = new PixPolygon2(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4, row1 + (row2 - row1) * 0.4),
                                   new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.4, col1 + (col2 - col1) * 0.2));
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
                double centerRow = row1 + (row2 - row1) * 0.2;
                double centerCol = col1 + (col2 - col1) * 0.2;
                double width = (row2 - row1) * 0.06;
                //////////////////////////////////////////////////////////////////
                this.pixPolygon = new PixPolygon2(new double[] { centerRow - width, centerRow - width, centerRow + width, centerRow + width },
                                                  new double[] { centerCol - width, centerCol + width, centerCol + width, centerCol - width });
                this.pixCoordSystem = new userPixCoordSystem();
                this.isDrawMode = true;
            }
        }

        public override void DrawRoiShapeOnWindow(enColor color, out ROI roi)
        {
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
                if (stopwatch.ElapsedMilliseconds > 60 * 5 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixPolygonParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixRoiShapeOnWindow(enColor color, out PixROI roi)
        {
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
                if (stopwatch.ElapsedMilliseconds > 60 * 5 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixPolygonParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixPolygonOnWindow(enColor color, out userPixPolygon pixPolygon)
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
                if (stopwatch.ElapsedMilliseconds > 60 * 5 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            pixPolygon = this.GetPixPolygonParam();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixPolygonOnWindow(enColor color, out drawPixPolygon pixPolygon)
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
            pixPolygon = this.GetDrawPixPolygonParam();
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
            if (this.isDrawMode == false) return;
            ChangeCursor(e.X, e.Y); // 在移动过程中改变光标
            /////////////////////////
            if (mIsClick == false || IsTranslate) // 只有在鼠标按下的状态下才执行移动
            {
                return;
            }
            ////////////////////
            switch (selectedNode)
            {
                case PosSizableRect.CenterPoint: // 平移多边形
                    for (int i = 0; i < this.pixPolygon.RectCol.Count; i++)
                    {
                        this.pixPolygon.RectRow[i] += e.Y - oldY;
                        this.pixPolygon.RectCol[i] += e.X - oldX;
                    }
                    this.pixPolygon.UpDataCircleNode(); // 同时更新
                    break;
                case PosSizableRect.NodePoint: // 选择节点
                    if (this.pixPolygon.ActiveIndex >= this.pixPolygon.RectRow.Count) return;
                    this.pixPolygon.RectRow[this.pixPolygon.ActiveIndex] += e.Y - oldY;
                    this.pixPolygon.RectCol[this.pixPolygon.ActiveIndex] += e.X - oldX;
                    int LastIndex = 0;
                    int FirstIndex = 0;
                    if (this.pixPolygon.ActiveIndex == 0)
                    {
                        FirstIndex = 1;
                        LastIndex = this.pixPolygon.RectRow.Count - 1;
                    }
                    else if (this.pixPolygon.RectRow.Count - 1 == this.pixPolygon.ActiveIndex)
                    {
                        FirstIndex = this.pixPolygon.ActiveIndex - 1;
                        LastIndex = 0;
                    }
                    else
                    {
                        FirstIndex = this.pixPolygon.ActiveIndex - 1;
                        LastIndex = this.pixPolygon.ActiveIndex + 1;
                    }
                    /////////////////////////////////////////////////////////
                    if (HMisc.DistancePl(this.pixPolygon.RectRow[this.pixPolygon.ActiveIndex], this.pixPolygon.RectCol[this.pixPolygon.ActiveIndex],
                                        this.pixPolygon.RectRow[FirstIndex], this.pixPolygon.RectCol[FirstIndex],
                                        this.pixPolygon.RectRow[LastIndex], this.pixPolygon.RectCol[LastIndex]) < this.nodeSizeRect * 0.5)
                    {
                        this.pixPolygon.RectRow.RemoveAt(this.pixPolygon.ActiveIndex);
                        this.pixPolygon.RectCol.RemoveAt(this.pixPolygon.ActiveIndex);
                        this.selectedNode = PosSizableRect.None;
                    }
                    // 添加中点 
                    this.pixPolygon.UpDataCircleNode();
                    break;
                case PosSizableRect.MiddleNode: // 移动直线的中点时，将其转化为节点
                    if (this.pixPolygon.ActiveIndex > this.pixPolygon.CircleRow.Count) return;
                    this.pixPolygon.CircleRow[this.pixPolygon.ActiveIndex] += e.Y - oldY;
                    this.pixPolygon.CircleCol[this.pixPolygon.ActiveIndex] += e.X - oldX;
                    LastIndex = 0;
                    FirstIndex = 0;
                    if (this.pixPolygon.ActiveIndex == 0)
                    {
                        FirstIndex = 0;
                        LastIndex = this.pixPolygon.ActiveIndex + 1;
                    }
                    else if (this.pixPolygon.ActiveIndex == this.pixPolygon.CircleRow.Count - 1)
                    {
                        FirstIndex = this.pixPolygon.ActiveIndex;
                        LastIndex = 0;
                    }
                    else
                    {
                        FirstIndex = this.pixPolygon.ActiveIndex;
                        LastIndex = this.pixPolygon.ActiveIndex + 1;
                    }
                    if (HMisc.DistancePl(this.pixPolygon.CircleRow[this.pixPolygon.ActiveIndex], this.pixPolygon.CircleCol[this.pixPolygon.ActiveIndex],
                                        this.pixPolygon.RectRow[FirstIndex], this.pixPolygon.RectCol[FirstIndex],
                                        this.pixPolygon.RectRow[LastIndex], this.pixPolygon.RectCol[LastIndex]) > this.nodeSizeRect * 0.5)
                    {
                        if (FirstIndex + 1 > this.pixPolygon.RectRow.Count - 1)
                        {
                            this.pixPolygon.RectRow.Add(this.pixPolygon.CircleRow[this.pixPolygon.ActiveIndex]);
                            this.pixPolygon.RectCol.Add(this.pixPolygon.CircleCol[this.pixPolygon.ActiveIndex]);
                        }
                        else
                        {
                            this.pixPolygon.RectRow.Insert(FirstIndex + 1, this.pixPolygon.CircleRow[this.pixPolygon.ActiveIndex]);
                            this.pixPolygon.RectCol.Insert(FirstIndex + 1, this.pixPolygon.CircleCol[this.pixPolygon.ActiveIndex]);
                        }
                        // 添加中点 
                        this.pixPolygon.UpDataCircleNode();
                        this.selectedNode = PosSizableRect.NodePoint;
                        this.pixPolygon.ActiveIndex = FirstIndex + 1;
                    }
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
                    switch (pos)
                    {
                        case PosSizableRect.MiddleNode:
                            if (pixPolygon.CircleRow.Count > 0)
                            {
                                for (int i = 0; i < pixPolygon.CircleRow.Count; i++)
                                {
                                    //this.AttachDrawingPropertyData.Add(CreateCircleSizableNode(pixPolygon.CircleCol[i], pixPolygon.CircleRow[i]));
                                    if (this.pixPolygon.ActiveIndex == i && this.selectedNode ==  PosSizableRect.MiddleNode)
                                        this.AttachDrawingPropertyData.Add(new ViewData(CreateCircleSizableNode(pixPolygon.CircleCol[i], pixPolygon.CircleRow[i]), "green"));
                                    else
                                        this.AttachDrawingPropertyData.Add(new ViewData(CreateCircleSizableNode(pixPolygon.CircleCol[i], pixPolygon.CircleRow[i]), "red"));
                                }
                            }
                            break;
                        case PosSizableRect.NodePoint:
                            for (int i = 0; i < pixPolygon.RectRow.Count; i++)
                            {
                                //this.AttachDrawingPropertyData.Add(CreateRectSizableNode(pixPolygon.RectCol[i], pixPolygon.RectRow[i]));
                                if (this.pixPolygon.ActiveIndex == i && this.selectedNode == PosSizableRect.NodePoint)
                                    this.AttachDrawingPropertyData.Add(new ViewData(CreateRectSizableNode(pixPolygon.RectCol[i], pixPolygon.RectRow[i]), "green"));
                                else
                                    this.AttachDrawingPropertyData.Add(new ViewData(CreateRectSizableNode(pixPolygon.RectCol[i], pixPolygon.RectRow[i]), "red"));
                            }
                            break;
                    }
                }
                //this.AttachDrawingPropertyData.Add(this.pixPolygon.GetXLD());
                this.AttachDrawingPropertyData.Add(new ViewData(this.pixPolygon.GetXLD(), "red"));
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont GetDrawingObject(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.NodePoint:
                    return CreateRectSizableNode(this.pixPolygon.RectCol[this.pixPolygon.ActiveIndex], this.pixPolygon.RectRow[this.pixPolygon.ActiveIndex]);
                case PosSizableRect.MiddleNode:
                    return CreateCircleSizableNode(this.pixPolygon.CircleCol[this.pixPolygon.ActiveIndex], this.pixPolygon.CircleRow[this.pixPolygon.ActiveIndex]);
                case PosSizableRect.CenterPoint:
                    return CreateRectSizableNode(this.pixPolygon.MidCol, this.pixPolygon.MidRow);
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect posSizable = PosSizableRect.None;
            if (this.pixPolygon.RectRow.Count == 0) return posSizable;
            if (this.pixPolygon.GetXLD().TestXldPoint(y, x) > 0) // 只要在多边形内部，则可以拖动
            {
                //this.pixPolygon.ActiveIndex = this.pixPolygon.RectRow.Count + this.pixPolygon.CircleRow.Count;
                posSizable = PosSizableRect.CenterPoint;
            }
            ////////////////////////
            for (int i = 0; i < pixPolygon.RectRow.Count; i++)
            {
                double temDist = Math.Sqrt((y - this.pixPolygon.RectRow[i]) * (y - this.pixPolygon.RectRow[i]) + (x - this.pixPolygon.RectCol[i]) * (x - this.pixPolygon.RectCol[i]));
                if (temDist <= this.nodeSizeRect)
                {
                    this.pixPolygon.ActiveIndex = i;
                    posSizable = PosSizableRect.NodePoint;
                    break;
                }
            }
            //// 是否在中间节点
            for (int i = 0; i < pixPolygon.CircleRow.Count; i++)
            {
                double temDist = Math.Sqrt((y - this.pixPolygon.CircleRow[i]) * (y - this.pixPolygon.CircleRow[i]) + (x - this.pixPolygon.CircleCol[i]) * (x - this.pixPolygon.CircleCol[i]));
                if (temDist <= this.nodeSizeRect)
                {
                    this.pixPolygon.ActiveIndex = i;
                    posSizable = PosSizableRect.MiddleNode;
                    break;
                }
            }
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
                case PosSizableRect.MiddleNode:
                    cursors = Cursors.Arrow;
                    break;
                case PosSizableRect.CenterPoint:
                    cursors = Cursors.Hand;
                    break;
                case PosSizableRect.NodePoint:
                    cursors = Cursors.Arrow;
                    break;
            }
            return cursors;
        }

        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
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
                        break;

                    case "清空":
                        this.AttachPropertyData.Clear();
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

        public override userPixPolygon GetPixPolygonParam()
        {
            userPixPolygon pixPolygon = new userPixPolygon(this.pixPolygon.RectRow.ToArray(), this.pixPolygon.RectCol.ToArray(), this.CameraParam);
            pixPolygon = pixPolygon.AffinePixPolygon(this.pixCoordSystem?.GetInvertVariationHomMat2D()); // 变换为之前的
            return pixPolygon;
        }
        public drawPixPolygon GetDrawPixPolygonParam()
        {
            drawPixPolygon pixPolygon = new drawPixPolygon(this.pixPolygon.RectRow.ToArray(), this.pixPolygon.RectCol.ToArray());
            pixPolygon = pixPolygon.AffinePixPolygon(this.pixCoordSystem?.GetInvertVariationHomMat2D()); // 变换为之前的
            return pixPolygon;
        }
    }

    public class PixPolygon2
    {
        public List<double> RectRow { get; set; }
        public List<double> RectCol { get; set; }
        public List<double> CircleRow { get; set; }
        public List<double> CircleCol { get; set; }
        public double MidRow { get; set; }
        public double MidCol { get; set; }
        public int ActiveIndex { get; set; }    // 选中的当前节点的索引

        public PixPolygon2()
        {
            this.RectRow = new List<double>();
            this.RectCol = new List<double>();
            this.CircleRow = new List<double>();
            this.CircleCol = new List<double>();
            this.MidRow = 0;
            this.MidCol = 0;
            this.ActiveIndex = 0;
        }
        public PixPolygon2(double[] rows, double[] cols)
        {
            this.RectRow = new List<double>();
            this.RectCol = new List<double>();
            this.CircleRow = new List<double>();
            this.CircleCol = new List<double>();
            this.RectRow.AddRange(rows);
            this.RectCol.AddRange(cols);
            this.MidRow = rows.Average();
            this.MidCol = cols.Average();
            this.ActiveIndex = 0;
            // 添加中点 
            this.UpDataCircleNode();
        }
        public PixPolygon2(userPixRectangle1 pixRectangle1)
        {
            this.RectRow = new List<double>();
            this.RectCol = new List<double>();
            this.CircleRow = new List<double>();
            this.CircleCol = new List<double>();
            this.RectRow.AddRange(new double[] { pixRectangle1.Row1, pixRectangle1.Row1, pixRectangle1.Row2, pixRectangle1.Row2 });
            this.RectCol.AddRange(new double[] { pixRectangle1.Col1, pixRectangle1.Col2, pixRectangle1.Col2, pixRectangle1.Col1 });
            this.ActiveIndex = 0;
            // 添加中点 
            this.UpDataCircleNode();
        }

        public void UpDataCircleNode()
        {
            this.CircleRow.Clear();
            this.CircleCol.Clear();
            // 添加中点 
            if (this.RectRow.Count > 2 && this.RectCol.Count > 2 && this.RectRow.Count == this.RectCol.Count)
            {
                for (int i = 0; i < this.RectRow.Count - 1; i++)
                {
                    this.CircleRow.Add((this.RectRow[i] + this.RectRow[i + 1]) * 0.5);
                    this.CircleCol.Add((this.RectCol[i] + this.RectCol[i + 1]) * 0.5);
                }
                this.CircleRow.Add((this.RectRow[0] + this.RectRow[this.RectRow.Count - 1]) * 0.5);
                this.CircleCol.Add((this.RectCol[0] + this.RectCol[this.RectRow.Count - 1]) * 0.5);
            }
        }
        public HXLDCont GetXLD()
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (this.RectRow.Count > 0)
                hXLDCont.GenContourPolygonXld(new HTuple(this.RectRow.ToArray(), this.RectRow[0]), new HTuple(this.RectCol.ToArray(), this.RectCol[0]));
            return hXLDCont;
        }


    }

    /// <summary>
    /// 可调大小节点的位置
    /// </summary>
    public enum PosSizableRect
    {
        MiddleNode,
        NodePoint,
        CenterPoint,
        None,
    }

}
