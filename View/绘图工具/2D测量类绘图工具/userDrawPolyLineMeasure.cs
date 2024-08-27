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
    public class userDrawPolyLineMeasure : DrawingBaseMeasure //DrawingBaseMeasure
    {
        private PixPolyLine pixPolyLine = new PixPolyLine();
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        private userPixCoordSystem pixCoordSystem;

        public userDrawPolyLineMeasure(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {

        }
        public userDrawPolyLineMeasure(HWindowControl hWindowControl, userPixLine pixLine) : base(hWindowControl)
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
                            this.pixPolyLine = new PixPolyLine(new double[] { pixLine.Row1, pixLine.Row2 }, new double[] { pixLine.Col1, pixLine.Col2 });
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
                    case nameof(userWcsLine):
                        if (!(param is userWcsLine)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsLine 类型");
                        userWcsLine wcsLine = (userWcsLine)param;
                        if (wcsLine.CamParams != null)
                        {
                            pixLine = wcsLine.GetPixLine();
                            this.pixPolyLine = new PixPolyLine(new double[] { pixLine.Row1, pixLine.Row2 }, new double[] { pixLine.Col1, pixLine.Col2 });
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
                            PixPolygon = WcsPolygon.GetPixPolygon(this.CameraParam);
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
                        case PosSizableRect.MiddleNode:
                            if (pixPolyLine.CircleRow.Count > 0)
                            {
                                for (int i = 0; i < pixPolyLine.CircleRow.Count; i++)
                                {
                                    //this.AttachDrawingPropertyData.Add(CreateCircleSizableNode(pixPolygon.CircleCol[i], pixPolygon.CircleRow[i]));
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
                                //this.AttachDrawingPropertyData.Add(CreateRectSizableNode(pixPolygon.RectCol[i], pixPolygon.RectRow[i]));
                                if (this.pixPolyLine.ActiveIndex == i && this.selectedNode == PosSizableRect.NodePoint)
                                    this.AttachDrawingPropertyData.Add(new ViewData(CreateRectSizableNode(pixPolyLine.RectCol[i], pixPolyLine.RectRow[i]), "green"));
                                else
                                    this.AttachDrawingPropertyData.Add(new ViewData(CreateRectSizableNode(pixPolyLine.RectCol[i], pixPolyLine.RectRow[i]), "red"));
                            }
                            break;
                    }
                }
                //this.AttachDrawingPropertyData.Add(this.pixPolygon.GetXLD());
                this.AttachDrawingPropertyData.Add(new ViewData(this.pixPolyLine.GetXLD(), "red"));
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont GetDrawingObject(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.NodePoint:
                    return CreateRectSizableNode(this.pixPolyLine.RectCol[this.pixPolyLine.ActiveIndex], this.pixPolyLine.RectRow[this.pixPolyLine.ActiveIndex]);
                case PosSizableRect.MiddleNode:
                    return CreateCircleSizableNode(this.pixPolyLine.CircleCol[this.pixPolyLine.ActiveIndex], this.pixPolyLine.CircleRow[this.pixPolyLine.ActiveIndex]);
                case PosSizableRect.CenterPoint:
                    return CreateRectSizableNode(this.pixPolyLine.MidCol, this.pixPolyLine.MidRow);
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect posSizable = PosSizableRect.None;
            if (this.pixPolyLine.RectRow.Count == 0) return posSizable;
            double minDist, maxDist;
            pixPolyLine.GetXLD().DistancePc(y, x, out minDist, out maxDist);
            if (minDist <= this.nodeSizeRect*0.8)
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

        public override userPixPolyLine GetPixPolyLineParam()
        {
            userPixPolyLine pixPolygon = new userPixPolyLine(this.pixPolyLine.RectRow.ToArray(), this.pixPolyLine.RectCol.ToArray(), this.CameraParam);
            pixPolygon = pixPolygon.AffinePixPolyLine(this.pixCoordSystem?.GetInvertVariationHomMat2D()); // 变换为之前的
            return pixPolygon;
        }

    }

    public class PixPolyLineMea
    {
        public List<double> RectRow { get; set; }
        public List<double> RectCol { get; set; }
        public List<double> CircleRow { get; set; }
        public List<double> CircleCol { get; set; }
        public double MidRow { get; set; }
        public double MidCol { get; set; }
        public int ActiveIndex { get; set; }    // 选中的当前节点的索引
        public double diffRadius { get; set; }
        public double normalPhi { get; set; }
        public PixPolyLineMea()
        {
            this.RectRow = new List<double>();
            this.RectCol = new List<double>();
            this.CircleRow = new List<double>();
            this.CircleCol = new List<double>();
            this.MidRow = 0;
            this.MidCol = 0;
            this.ActiveIndex = 0;
        }
        public PixPolyLineMea(double[] rows, double[] cols)
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
        public PixPolyLineMea(userPixRectangle1 pixRectangle1)
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
            if (this.RectRow.Count > 1 && this.RectCol.Count > 1 && this.RectRow.Count == this.RectCol.Count)
            {
                for (int i = 0; i < this.RectRow.Count - 1; i++)
                {
                    this.CircleRow.Add((this.RectRow[i] + this.RectRow[i + 1]) * 0.5);
                    this.CircleCol.Add((this.RectCol[i] + this.RectCol[i + 1]) * 0.5);
                }
            }
        }
        public HXLDCont GetXLD()
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (this.RectRow.Count > 0)
                hXLDCont.GenContourPolygonXld(new HTuple(this.RectRow.ToArray()), new HTuple(this.RectCol.ToArray()));
            return hXLDCont;
        }


    }



}
