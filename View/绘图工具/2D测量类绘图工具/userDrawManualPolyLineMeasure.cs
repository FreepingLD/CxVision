using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace View
{
    [Serializable]
    public class userDrawManualPolyLineMeasure : DrawingBaseMeasure //DrawingBaseMeasure
    {
        private PixPolyLine pixPolyLine = new PixPolyLine();
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        private userPixCoordSystem pixCoordSystem;

        public userDrawManualPolyLineMeasure(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {

        }
        public userDrawManualPolyLineMeasure(HWindowControl hWindowControl, userPixLine pixLine) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            ////////////////////
            if (pixLine != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixPolyLine = new PixPolyLine(new double[] { pixLine.Row1, pixLine.Row2 }, new double[] { pixLine.Col1, pixLine.Col2 });
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                double centerRow = row1 + (row2 - row1) * 0.2;
                double centerCol = col1 + (col2 - col1) * 0.2;
                double width = (row2 - row1) * 0.06;
                //////////////////////////////////////////////////////////////////
                this.pixPolyLine = new PixPolyLine(new double[] { centerRow - width, centerRow - width, centerRow + width, centerRow + width },
                    new double[] { centerCol - width, centerCol + width, centerCol + width, centerCol - width });
            }

        }
        public userDrawManualPolyLineMeasure(HWindowControl hWindowControl, userPixPolyLine pixPolyLine, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixPolyLine != null && pixPolyLine.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCoordSystem = pixCoordSystem;
                userPixPolyLine pixpolyLine = pixPolyLine.AffinePixPolyLine(pixCoordSystem.GetVariationHomMat2D());
                this.pixPolyLine = new PixPolyLine(pixpolyLine.Row.ToArray(), pixpolyLine.Col.ToArray(), pixpolyLine.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.pixPolyLine = new PixPolyLine(new double[] { row1 + (row2 - row1) * 0.1, row1 + (row2 - row1) * 0.3 }, new double[] { col1 + (col2 - col1) * 0.1, col1 + (col2 - col1) * 0.1 }, 20);
                this.pixPolyLine.DiffRadius = this.nodeSizeRect * 10;
            }
        }

        public override void SetParam(object param)
        {
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixPolyLine):
                        if (!(param is userPixPolyLine)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixPolyLine 类型");
                        userPixPolyLine pixPolyLine = (userPixPolyLine)param;
                        if (pixPolyLine.CamParams != null)
                        {
                            this.pixPolyLine = new PixPolyLine(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray(), pixPolyLine.DiffRadius);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolyLine = new PixPolyLine(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4),
                                                               new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.2), 20);
                        }
                        this.isDrwingObject = true;
                        break;
                    case nameof(userWcsPolyLine):
                        if (!(param is userWcsPolyLine)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsPolyLine 类型");
                        userWcsPolyLine wcsPolyLine = (userWcsPolyLine)param;
                        if (wcsPolyLine.CamParams != null)
                        {
                            pixPolyLine = wcsPolyLine.GetPixPolyLine();
                            this.pixPolyLine = new PixPolyLine(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray(), pixPolyLine.DiffRadius);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolyLine = new PixPolyLine(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4),
                                                               new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.2), 20);
                        }
                        this.isDrwingObject = true;
                        break;

                    case nameof(drawPixPolyLine):
                        if (!(param is drawPixPolyLine)) throw new ArgumentException("给定的数据类型异常，需要传入 drawPixPolyLine 类型");
                        drawPixPolyLine PixPolygon = (drawPixPolyLine)param;
                        if (this.CameraParam != null)
                        {
                            this.pixPolyLine = new PixPolyLine(PixPolygon.Row.ToArray(), PixPolygon.Col.ToArray());
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolyLine = new PixPolyLine(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4),
                                         new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.2));
                        }
                        this.isDrwingObject = true;
                        break;
                    case nameof(drawWcsPolyLine):
                        if (!(param is drawWcsPolygon)) throw new ArgumentException("给定的数据类型异常，需要传入 drawWcsPolyLine 类型");
                        drawWcsPolyLine WcsPolygon = (drawWcsPolyLine)param;
                        if (this.CameraParam != null)
                        {
                            PixPolygon = WcsPolygon.GetPixPolyLine(this.CameraParam);
                            this.pixPolyLine = new PixPolyLine(PixPolygon.Row.ToArray(), PixPolygon.Col.ToArray());
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.pixPolyLine = new PixPolyLine(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4),
                                         new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.2));
                        }
                        this.isDrwingObject = true;
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
                this.pixPolyLine = new PixPolyLine(new HTuple(row1 + (row2 - row1) * 0.2, row1 + (row2 - row1) * 0.4),
                                                   new HTuple(col1 + (col2 - col1) * 0.2, col1 + (col2 - col1) * 0.2));
                this.pixCoordSystem = new userPixCoordSystem();
                this.isDrwingObject = true;
            }
        }

        protected override void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    selectedNode = PosSizableRect.None; // 先让节点处于非选中状态
                    selectedNode = GetNodeSelectable(e.X, e.Y);
                    if (selectedNode == PosSizableRect.None || this.isDrwingObject == false)
                        this.IsTranslate = true;
                    else
                        this.IsTranslate = false;
                    /////////////
                    base.hWindowControl_HMouseDown(sender, e);
                    break;
                case MouseButtons.Right:
                    //this.CurrentButton = e.Button;
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
            if (this.isDrwingObject == false) return;
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
                    for (int i = 0; i < this.pixPolyLine.RectCol.Count; i++)
                    {
                        this.pixPolyLine.RectRow[i] += e.Y - oldY;
                        this.pixPolyLine.RectCol[i] += e.X - oldX;
                    }
                    this.pixPolyLine.UpDataCircleNode(); // 同时更新
                    break;
                case PosSizableRect.NodePoint: // 选择节点
                    if (this.pixPolyLine.ActiveIndex >= this.pixPolyLine.RectRow.Count) return;
                    this.pixPolyLine.RectRow[this.pixPolyLine.ActiveIndex] += e.Y - oldY;
                    this.pixPolyLine.RectCol[this.pixPolyLine.ActiveIndex] += e.X - oldX;
                    int LastIndex = 0;
                    int FirstIndex = 0;
                    if (this.pixPolyLine.ActiveIndex == 0 || this.pixPolyLine.RectRow.Count - 1 == this.pixPolyLine.ActiveIndex)
                    {
                        FirstIndex = 1;
                        LastIndex = this.pixPolyLine.RectRow.Count - 1;
                    }
                    else
                    {
                        FirstIndex = this.pixPolyLine.ActiveIndex - 1;
                        LastIndex = this.pixPolyLine.ActiveIndex + 1;
                        /////////////////////////////////////////////////////////
                        if (HMisc.DistancePl(this.pixPolyLine.RectRow[this.pixPolyLine.ActiveIndex], this.pixPolyLine.RectCol[this.pixPolyLine.ActiveIndex],
                                            this.pixPolyLine.RectRow[FirstIndex], this.pixPolyLine.RectCol[FirstIndex],
                                            this.pixPolyLine.RectRow[LastIndex], this.pixPolyLine.RectCol[LastIndex]) < this.nodeSizeRect * 0.5)
                        {
                            if (this.pixPolyLine.RectRow.Count > 2)
                            {
                                this.pixPolyLine.RectRow.RemoveAt(this.pixPolyLine.ActiveIndex);
                                this.pixPolyLine.RectCol.RemoveAt(this.pixPolyLine.ActiveIndex);
                                this.selectedNode = PosSizableRect.None;
                            }
                        }
                    }
                    // 添加中点 
                    this.pixPolyLine.UpDataCircleNode();
                    break;
                case PosSizableRect.MiddleNode: // 移动直线的中点时，将其转化为节点
                    if (this.pixPolyLine.ActiveIndex > this.pixPolyLine.CircleRow.Count) return;
                    this.pixPolyLine.CircleRow[this.pixPolyLine.ActiveIndex] += e.Y - oldY;
                    this.pixPolyLine.CircleCol[this.pixPolyLine.ActiveIndex] += e.X - oldX;
                    LastIndex = 0;
                    FirstIndex = 0;
                    FirstIndex = this.pixPolyLine.ActiveIndex;
                    LastIndex = this.pixPolyLine.ActiveIndex + 1;
                    if (HMisc.DistancePl(this.pixPolyLine.CircleRow[this.pixPolyLine.ActiveIndex], this.pixPolyLine.CircleCol[this.pixPolyLine.ActiveIndex],
                                         this.pixPolyLine.RectRow[FirstIndex], this.pixPolyLine.RectCol[FirstIndex],
                                         this.pixPolyLine.RectRow[LastIndex], this.pixPolyLine.RectCol[LastIndex]) > this.nodeSizeRect * 0.5)
                    {
                        if (FirstIndex + 1 > this.pixPolyLine.RectRow.Count - 1)
                        {
                            this.pixPolyLine.RectRow.Add(this.pixPolyLine.CircleRow[this.pixPolyLine.ActiveIndex]);
                            this.pixPolyLine.RectCol.Add(this.pixPolyLine.CircleCol[this.pixPolyLine.ActiveIndex]);
                        }
                        else
                        {
                            this.pixPolyLine.RectRow.Insert(FirstIndex + 1, this.pixPolyLine.CircleRow[this.pixPolyLine.ActiveIndex]);
                            this.pixPolyLine.RectCol.Insert(FirstIndex + 1, this.pixPolyLine.CircleCol[this.pixPolyLine.ActiveIndex]);
                        }
                        // 添加中点 
                        this.pixPolyLine.UpDataCircleNode();
                        this.selectedNode = PosSizableRect.NodePoint;
                        this.pixPolyLine.ActiveIndex = FirstIndex + 1;
                    }
                    break;
                //case PosSizableRect.Arrow:
                //    double phi = Math.Atan2((e.Y - this.oldY) * 1, e.X - this.oldX);
                //    if ((0.785 < phi && phi < 2.355) || (-2.355 < phi && phi < -0.785))
                //        this.pixPolyLine.DiffRadius += (HMisc.DistancePp(e.Y, e.X, this.oldY, this.oldX) * Math.Sin(phi));
                //    else
                //        this.pixPolyLine.DiffRadius += (HMisc.DistancePp(e.Y, e.X, this.oldY, this.oldX) * Math.Cos(phi));
                //    break;

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
                if (this.isDrwingObject)
                {
                    switch (pos)
                    {
                        case PosSizableRect.FirstNode:
                            if (pixPolyLine.RectRow.Count > 0)
                            {
                                this.AttachDrawingPropertyData.Add(new ViewData(CreateRectSizableNode(pixPolyLine.RectCol[0], pixPolyLine.RectRow[0]), "yellow"));
                            }
                            break;
                        case PosSizableRect.MiddleNode:
                            if (pixPolyLine.CircleRow.Count > 0)
                            {
                                for (int i = 0; i < pixPolyLine.CircleRow.Count; i++)
                                {
                                    if (this.pixPolyLine.ActiveIndex == i && this.selectedNode == PosSizableRect.MiddleNode)
                                        this.AttachDrawingPropertyData.Add(new ViewData(CreateCircleSizableNode(pixPolyLine.CircleCol[i], pixPolyLine.CircleRow[i]), "green"));
                                    else
                                        this.AttachDrawingPropertyData.Add(new ViewData(CreateCircleSizableNode(pixPolyLine.CircleCol[i], pixPolyLine.CircleRow[i]), "red"));
                                }
                            }
                            break;
                        case PosSizableRect.NodePoint:
                            for (int i = 0; i < pixPolyLine.RectRow.Count; i++)
                            {
                                if (this.pixPolyLine.ActiveIndex == i && this.selectedNode == PosSizableRect.NodePoint)
                                    this.AttachDrawingPropertyData.Add(new ViewData(CreateRectSizableNode(pixPolyLine.RectCol[i], pixPolyLine.RectRow[i]), "green"));
                                else
                                    this.AttachDrawingPropertyData.Add(new ViewData(CreateRectSizableNode(pixPolyLine.RectCol[i], pixPolyLine.RectRow[i]), "red"));
                            }
                            break;
                        //case PosSizableRect.Arrow:
                        //    double[] row1, col1, row2, col2;
                        //    GetArrowPoint(pixPolyLine.RectRow, pixPolyLine.RectCol, out row1, out col1, out row2, out col2);
                        //    HXLDCont hXLD = GenArrowContourXld(row1, col1, row2, col2, this.nodeSizeRect, this.nodeSizeRect);
                        //    this.AttachDrawingPropertyData.Add(new ViewData(hXLD, "red"));
                        //    break;
                    }
                }
            }
            this.AttachDrawingPropertyData.Add(new ViewData(this.pixPolyLine.GetXLD(), "red"));
            base.DrawingGraphicObject();
        }

        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect posSizable = PosSizableRect.None;
            if (this.pixPolyLine.RectRow.Count == 0) return posSizable;
            double minDist, maxDist;
            pixPolyLine.GetXLD().DistancePc(y, x, out minDist, out maxDist);
            if (minDist <= this.nodeSizeRect * 0.8)
                posSizable = PosSizableRect.CenterPoint;
            ////////////////////////
            for (int i = 0; i < pixPolyLine.RectRow.Count; i++)
            {
                double temDist = Math.Sqrt((y - this.pixPolyLine.RectRow[i]) * (y - this.pixPolyLine.RectRow[i]) + (x - this.pixPolyLine.RectCol[i]) * (x - this.pixPolyLine.RectCol[i]));
                if (temDist <= this.nodeSizeRect)
                {
                    this.pixPolyLine.ActiveIndex = i;
                    posSizable = PosSizableRect.NodePoint;
                    break;
                }
            }
            //// 是否在中间节点
            for (int i = 0; i < pixPolyLine.CircleRow.Count; i++)
            {
                double temDist = Math.Sqrt((y - this.pixPolyLine.CircleRow[i]) * (y - this.pixPolyLine.CircleRow[i]) + (x - this.pixPolyLine.CircleCol[i]) * (x - this.pixPolyLine.CircleCol[i]));
                if (temDist <= this.nodeSizeRect)
                {
                    this.pixPolyLine.ActiveIndex = i;
                    posSizable = PosSizableRect.MiddleNode;
                    break;
                }
            }
            /// 是否选中箭头
            //if (Math.Abs(this.pixPolyLine.DiffRadius) - this.nodeSizeRect < minDist && minDist < Math.Abs(this.pixPolyLine.DiffRadius) + this.nodeSizeRect)
            //    posSizable = PosSizableRect.Arrow;


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
            if (this.pixPolyLine.RectRow.Count == 0 && this.pixPolyLine.RectCol.Count == 0)
                this.pixPolyLine = new PixPolyLine(new double[] { row1 + 200, row1 + 400 }, new double[] { column1 + 200, column1 + 200 }, 20);
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

        public override userPixPolyLine GetPixPolyLineParam()
        {
            userPixPolyLine pixPolyLine = new userPixPolyLine(this.pixPolyLine.RectRow.ToArray(), this.pixPolyLine.RectCol.ToArray(), this.CameraParam);
            pixPolyLine = pixPolyLine.AffinePixPolyLine(this.pixCoordSystem?.GetInvertVariationHomMat2D()); // 变换为之前的
            pixPolyLine.DiffRadius = this.pixPolyLine.DiffRadius;
            pixPolyLine.NormalPhi.Clear();
            pixPolyLine.NormalPhi.AddRange(this.pixPolyLine.NormalPhi.ToArray());
            return pixPolyLine;
        }

    }





}
