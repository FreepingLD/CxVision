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
    public class userDrawEllipseMeasure : DrawingBaseMeasure
    {
        private Ellipse ellipse;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        //private userWcsEllipse wcsEllipse;
        private userWcsCoordSystem wcsCoordSystem;
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
            CircleCenter,
            InnerCircle,
            OutSideCircle,
            Arrow,
            None,
        }

        public userDrawEllipseMeasure(HWindowControl hWindowControl) : base(hWindowControl)
        {
            
        }
        public userDrawEllipseMeasure(HWindowControl hWindowControl, userWcsEllipse wcsEllipse, userWcsCoordSystem wcsCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsEllipse != null && wcsEllipse.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //this.wcsEllipse = wcsEllipse;
                this.wcsCoordSystem = wcsCoordSystem;
                userPixEllipse PixEllipse = wcsEllipse.AffineWcsEllipse2D(wcsCoordSystem.GetVariationHomMat2D()).GetPixEllipse();
                this.ellipse = new Ellipse(PixEllipse.Row, PixEllipse.Col, PixEllipse.Rad, PixEllipse.Radius1, PixEllipse.Radius2, PixEllipse.DiffRadius);
                this.ellipse.diffRadius = wcsEllipse.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipse = new Ellipse(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2,  0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, this.nodeSizeRect);
                this.ellipse.diffRadius = this.ellipse.radius2 * 0.5;
            }
        }
        public userDrawEllipseMeasure(HWindowControl hWindowControl, userPixEllipse pixEllipse, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (pixEllipse != null && pixEllipse.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //this.wcsEllipse = pixEllipse;
                this.pixCoordSystem = pixCoordSystem;
                userPixEllipse PixEllipse = pixEllipse.AffineTransPixEllipse(wcsCoordSystem.GetVariationHomMat2D());//.GetPixEllipse();
                this.ellipse = new Ellipse(PixEllipse.Row, PixEllipse.Col, PixEllipse.Rad, PixEllipse.Radius1, PixEllipse.Radius2, PixEllipse.DiffRadius);
                this.ellipse.diffRadius = pixEllipse.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipse = new Ellipse(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, this.nodeSizeRect);
                this.ellipse.diffRadius = this.ellipse.radius2 * 0.5;
            }
        }
        public userDrawEllipseMeasure(HWindowControl hWindowControl, userWcsEllipse wcsEllipse) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsEllipse != null && wcsEllipse .CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                userPixEllipse PixEllipse = wcsEllipse.GetPixEllipse();
                this.ellipse = new Ellipse(PixEllipse.Row, PixEllipse.Col, PixEllipse.Rad, PixEllipse.Radius1, PixEllipse.Radius2, PixEllipse.DiffRadius);
                this.ellipse.diffRadius = wcsEllipse.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipse = new Ellipse(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, this.nodeSizeRect);
                this.ellipse.diffRadius = this.ellipse.radius2 * 0.5;
            }
        }
        public userDrawEllipseMeasure(HWindowControl hWindowControl, userPixEllipse pixEllipse) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (pixEllipse != null && pixEllipse.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //userPixEllipse PixEllipse = pixEllipse.GetPixEllipse();
                this.ellipse = new Ellipse(pixEllipse.Row, pixEllipse.Col, pixEllipse.Rad, pixEllipse.Radius1, pixEllipse.Radius2, pixEllipse.DiffRadius);
                this.ellipse.diffRadius = pixEllipse.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipse = new Ellipse(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, this.nodeSizeRect);
                this.ellipse.diffRadius = this.ellipse.radius2 * 0.5;
            }
        }
        public override void SetParam(object param)
        {
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixEllipse):
                        if (!(param is userPixEllipse)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixEllipse 类型");
                        userPixEllipse PixEllipse = (userPixEllipse)param;
                        if (PixEllipse.CamParams != null)
                        {
                            //userPixEllipse PixEllipse = pixEllipse.GetPixEllipse();
                            this.ellipse = new Ellipse(PixEllipse.Row, PixEllipse.Col, PixEllipse.Rad, PixEllipse.Radius1, PixEllipse.Radius2, PixEllipse.DiffRadius);
                            this.ellipse.diffRadius = PixEllipse.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.ellipse = new Ellipse(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, this.nodeSizeRect);
                            this.ellipse.diffRadius = this.ellipse.radius2 * 0.5;
                        }
                        break;
                    case nameof(userWcsEllipse):
                        if (!(param is userWcsEllipse)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsEllipse 类型");
                        userWcsEllipse wcsEllipse = (userWcsEllipse)param;
                        if (wcsEllipse.CamParams != null)
                        {
                            PixEllipse = wcsEllipse.GetPixEllipse();
                            this.ellipse = new Ellipse(PixEllipse.Row, PixEllipse.Col, PixEllipse.Rad, PixEllipse.Radius1, PixEllipse.Radius2, PixEllipse.DiffRadius);
                            this.ellipse.diffRadius = PixEllipse.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.ellipse = new Ellipse(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, this.nodeSizeRect);
                            this.ellipse.diffRadius = this.ellipse.radius2 * 0.5;
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
                this.ellipse = new Ellipse(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, this.nodeSizeRect);
                this.ellipse.diffRadius = this.ellipse.radius2 * 0.5;
                this.wcsCoordSystem = new userWcsCoordSystem();
                this.pixCoordSystem = new userPixCoordSystem();
            }
        }
        protected override void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            switch (e.Button)
            {
                //case MouseButtons.Right:
                //    selectedNode = PosSizableRect.None; // 先让节点处于非选中状态
                //    selectedNode = GetNodeSelectable(e.X, e.Y);
                //    if (selectedNode == PosSizableRect.None)
                //        this.isTranslate = true;
                //    else
                //        this.isTranslate = false;
                //    base.hWindowControl_HMouseDown(sender, e);
                //    break;

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
                    //if (this.wcsEllipse != null)
                    //    this.wcsEllipse.Execute(null);
                    break;
            }
        }
        protected override void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            UpdataMeasureRegion(); // 在鼠标松开时，刷新一次数据
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
                case PosSizableRect.DownMiddleNode:
                case PosSizableRect.UpMiddleNode:
                    this.ellipse.radius2 += HMisc.DistancePp(e.Y, e.X, this.ellipse.row, this.ellipse.col) - HMisc.DistancePp(this.oldY, this.oldX, this.ellipse.row, this.ellipse.col);
                    if (this.ellipse.radius1 < 5)
                        this.ellipse.radius1 = 5;
                    if (this.ellipse.radius2 < 5)
                        this.ellipse.radius2 = 5;
                    //////////////////////
                    this.ellipse.leftPointCol = this.ellipse.col - this.ellipse.radius2 - this.ellipse.radius1;
                    this.ellipse.leftPointRow = this.ellipse.row;
                    this.ellipse.rightPointCol = this.ellipse.col + this.ellipse.radius2 + this.ellipse.radius1;
                    this.ellipse.rightPointRow = this.ellipse.row;
                    this.ellipse.upMiddlePointCol = this.ellipse.col;
                    this.ellipse.upMiddlePointRow = this.ellipse.row + this.ellipse.radius1 * -1;
                    this.ellipse.downMiddlePointCol = this.ellipse.col;
                    this.ellipse.downMiddlePointRow = this.ellipse.row + this.ellipse.radius1;
                    /////////////////////////////////
                    this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi + Math.PI, out this.ellipse.leftPointRow, out this.ellipse.leftPointCol);
                    this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi, out this.ellipse.rightPointRow, out this.ellipse.rightPointCol);
                    this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0, out this.ellipse.upMiddlePointRow, out this.ellipse.upMiddlePointCol);
                    this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0 + Math.PI, out this.ellipse.downMiddlePointRow, out this.ellipse.downMiddlePointCol);
                    break;

                case PosSizableRect.LeftBottomNode:
                case PosSizableRect.RightBottomNode:
                    this.ellipse.radius1 += HMisc.DistancePp(e.Y, e.X, this.ellipse.row, this.ellipse.col) - HMisc.DistancePp(this.oldY, this.oldX, this.ellipse.row, this.ellipse.col);
                    if (this.ellipse.radius1 < 5)
                        this.ellipse.radius1 = 5;
                    if (this.ellipse.radius2 < 5)
                        this.ellipse.radius2 = 5;
                    //////////////////////
                    this.ellipse.leftPointCol = this.ellipse.col - this.ellipse.radius2 - this.ellipse.radius1;
                    this.ellipse.leftPointRow = this.ellipse.row;
                    this.ellipse.rightPointCol = this.ellipse.col + this.ellipse.radius2 + this.ellipse.radius1;
                    this.ellipse.rightPointRow = this.ellipse.row;
                    this.ellipse.upMiddlePointCol = this.ellipse.col;
                    this.ellipse.upMiddlePointRow = this.ellipse.row + this.ellipse.radius1 * -1;
                    this.ellipse.downMiddlePointCol = this.ellipse.col;
                    this.ellipse.downMiddlePointRow = this.ellipse.row + this.ellipse.radius1;
                    /////////////////////////////////
                    this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi + Math.PI, out this.ellipse.leftPointRow, out this.ellipse.leftPointCol);
                    this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi, out this.ellipse.rightPointRow, out this.ellipse.rightPointCol);
                    this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0, out this.ellipse.upMiddlePointRow, out this.ellipse.upMiddlePointCol);
                    this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0 + Math.PI, out this.ellipse.downMiddlePointRow, out this.ellipse.downMiddlePointCol);
                    break;

                case PosSizableRect.CircleCenter:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.ellipse.col += e.X - oldX;
                        this.ellipse.row += e.Y - oldY;
                        this.ellipse.leftPointCol += e.X - oldX;
                        this.ellipse.leftPointRow += e.Y - oldY;
                        this.ellipse.rightPointCol += e.X - oldX;
                        this.ellipse.rightPointRow += e.Y - oldY;
                        this.ellipse.upMiddlePointCol += e.X - oldX;
                        this.ellipse.upMiddlePointRow += e.Y - oldY;
                        this.ellipse.downMiddlePointCol += e.X - oldX;
                        this.ellipse.downMiddlePointRow += e.Y - oldY;
                        /////////////////////////////////
                        this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi, out this.ellipse.leftPointRow, out this.ellipse.leftPointCol);
                        this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi + Math.PI, out this.ellipse.rightPointRow, out this.ellipse.rightPointCol);
                        this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0, out this.ellipse.upMiddlePointRow, out this.ellipse.upMiddlePointCol);
                        this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0 + Math.PI, out this.ellipse.downMiddlePointRow, out this.ellipse.downMiddlePointCol);
                    }
                    else
                    {
                        //HTuple ATan;
                        //HOperatorSet.LineOrientation(this.ellipse.row, this.ellipse.col, e.Y, e.X, out ATan);
                        //this.ellipse.phi = ATan.D;
                        this.ellipse.phi += Math.Atan2(oldY - this.ellipse.row, oldX - this.ellipse.col) - Math.Atan2(e.Y - this.ellipse.row, e.X - this.ellipse.col);
                        this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi, out this.ellipse.leftPointRow, out this.ellipse.leftPointCol);
                        this.ellipse.AffinePoint(this.ellipse.radius1, this.ellipse.phi + Math.PI, out this.ellipse.rightPointRow, out this.ellipse.rightPointCol);
                        this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0, out this.ellipse.upMiddlePointRow, out this.ellipse.upMiddlePointCol);
                        this.ellipse.AffinePoint(this.ellipse.radius2, this.ellipse.phi + Math.PI / 2.0 + Math.PI, out this.ellipse.downMiddlePointRow, out this.ellipse.downMiddlePointCol);
                    }
                    break;
                case PosSizableRect.OutSideCircle:
                case PosSizableRect.InnerCircle:
                    this.ellipse.diffRadius += HMisc.DistancePp(e.Y, e.X, this.ellipse.row, this.ellipse.col) - HMisc.DistancePp(this.oldY, this.oldX, this.ellipse.row, this.ellipse.col);
                    break;

            }
            //上一点需要实时更新，这样一来图像才不会乱
            oldX = e.X;
            oldY = e.Y;
            //重绘图形
            if (mIsClick)
            {
                UpdataMeasureRegion();
                DrawingGraphicObject();
            }

        }


        /// <summary>
        /// 绘制整个绘图对象
        /// </summary>
        public override void DrawingGraphicObject()
        {
            if (this.ellipse.radius1 != 0 || this.ellipse.row != 0)
            {
                this.AttachDrawingPropertyData.Clear();
                ///////////////// 将图中所有节点位置的矩形绘制出来 //////////////////////
                foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
                {
                    if (this.isDispalyAttachDrawingProperty)
                        this.AttachDrawingPropertyData.Add(GetSizableRect(pos));
                }
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont CreateEllipseObject(double diffRadius)
        {
            HXLDCont xld = new HXLDCont();
            if (this.ellipse.radius1 != 0 && this.ellipse.radius2 != 0)
                xld.GenEllipseContourXld(this.ellipse.row, this.ellipse.col, this.ellipse.phi, Math.Abs((this.ellipse.radius1 + diffRadius)), Math.Abs((this.ellipse.radius2 + diffRadius)), 0, Math.PI * 2, "positive", 0.1);
            return xld;
        }
        private HXLDCont GetSizableRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftBottomNode:
                    return CreateRectSizableNode(this.ellipse.leftPointCol, this.ellipse.leftPointRow);

                case PosSizableRect.RightBottomNode:
                    return CreateRectSizableNode(this.ellipse.rightPointCol, this.ellipse.rightPointRow);

                case PosSizableRect.UpMiddleNode:
                    return CreateRectSizableNode(this.ellipse.upMiddlePointCol, this.ellipse.upMiddlePointRow);

                case PosSizableRect.DownMiddleNode:
                    return CreateRectSizableNode(this.ellipse.downMiddlePointCol, this.ellipse.downMiddlePointRow);

                case PosSizableRect.CircleCenter:
                    return CreateEllipseObject(0);

                case PosSizableRect.Arrow:
                    return GetArrowXLD();
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double dist = Math.Sqrt((y - this.ellipse.row) * (y - this.ellipse.row) + (x - this.ellipse.col) * (x - this.ellipse.col));
            double dist1 = Math.Sqrt((y - this.ellipse.leftPointRow) * (y - this.ellipse.leftPointRow) + (x - this.ellipse.leftPointCol) * (x - this.ellipse.leftPointCol));
            double dist2 = Math.Sqrt((y - this.ellipse.upMiddlePointRow) * (y - this.ellipse.upMiddlePointRow) + (x - this.ellipse.upMiddlePointCol) * (x - this.ellipse.upMiddlePointCol));
            double dist3 = Math.Sqrt((y - this.ellipse.rightPointRow) * (y - this.ellipse.rightPointRow) + (x - this.ellipse.rightPointCol) * (x - this.ellipse.rightPointCol));
            double dist4 = Math.Sqrt((y - this.ellipse.downMiddlePointRow) * (y - this.ellipse.downMiddlePointRow) + (x - this.ellipse.downMiddlePointCol) * (x - this.ellipse.downMiddlePointCol));
            HXLDCont innerXLD = CreateEllipseObject(this.ellipse.diffRadius * -1);
            HXLDCont outXLD = CreateEllipseObject(this.ellipse.diffRadius);
            double min1Dist = 0, min2Dist = 0, max1Dist = 0, max2Dist = 0;
            if (innerXLD.IsInitialized())
                innerXLD.DistancePc(y, x, out min1Dist, out max1Dist);
            if (outXLD.IsInitialized())
                outXLD.DistancePc(y, x, out min2Dist, out max2Dist);
            //////////////////////////////////
            //if (dist < (this.ellipse.radius1 - Math.Abs(this.ellipse.diffRadius)) * 0.8) // 表示移动
            //    selectNode = PosSizableRect.CircleCenter;
            if (this.ellipse.GetHXLD(this.ellipse.diffRadius * 1.1).TestXldPoint(y, x) > 0)
                selectNode = PosSizableRect.CircleCenter;
            //////////////////////////////////////////////////第二点
            if (min1Dist < this.nodeSizeRect) //Math.Abs(0.1 * this.ellipse.radius1)
                selectNode = PosSizableRect.InnerCircle;   //　移动箭头
            //////////////////////////////////////////////////第三点
            if (min2Dist < this.nodeSizeRect) // Math.Abs(0.1 * this.ellipse.radius1)
                selectNode = PosSizableRect.OutSideCircle;　//　移动箭头
            //////////////////////////////////////////////////
            if (dist1 <= this.nodeSizeRect)  //Math.Abs(0.1 * this.ellipse.radius2)
                selectNode = PosSizableRect.LeftBottomNode;
            //////////////////////////////////////////////////
            if (dist2 <= this.nodeSizeRect)  //Math.Abs(0.1 * this.ellipse.radius1)
                selectNode = PosSizableRect.UpMiddleNode;
            //////////////////////////////////////////////////
            if (dist3 <= this.nodeSizeRect)  //Math.Abs(0.1 * this.ellipse.radius2)
                selectNode = PosSizableRect.RightBottomNode;
            ////////////////////////////////////////////////// 第一点
            if (dist4 <= this.nodeSizeRect)  //Math.Abs(0.1 * this.ellipse.radius1)
                selectNode = PosSizableRect.DownMiddleNode;

            //////////////////////////
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
                case PosSizableRect.CircleCenter:
                    return Cursors.Hand;

                case PosSizableRect.LeftBottomNode:
                case PosSizableRect.RightBottomNode:
                case PosSizableRect.UpMiddleNode:  //MovePos
                case PosSizableRect.DownMiddleNode:  //MovePos
                    return Cursors.Cross;
                default:
                    return Cursors.Default;
            }
        }

        private struct Ellipse
        {
            public double leftPointRow;
            public double leftPointCol;
            public double upMiddlePointRow;
            public double upMiddlePointCol;
            public double downMiddlePointRow;
            public double downMiddlePointCol;
            public double rightPointRow;
            public double rightPointCol;
            public double row;
            public double col;
            public double phi;
            public double radius1;
            public double radius2;
            public double diffRadius;
            public double[] normalPhi;
            public Ellipse(double row, double col, double phi, double radius1, double radius2, double diffRadius)
            {
                this.row = row;
                this.col = col;
                this.phi = phi;
                this.radius1 = radius1;
                this.radius2 = radius2;
                this.leftPointRow = row;
                this.leftPointCol = col - radius1;
                this.upMiddlePointRow = row - radius2;
                this.upMiddlePointCol = col;
                this.rightPointRow = row;
                this.rightPointCol = col + radius1;
                this.downMiddlePointRow = row + radius2;
                this.downMiddlePointCol = col;
                this.diffRadius = diffRadius;
                this.normalPhi = null;
                /////////////////////////////////////////////
                AffinePoint(radius1, phi + Math.PI, out this.leftPointRow, out this.leftPointCol);
                AffinePoint(radius1, phi, out this.rightPointRow, out this.rightPointCol);
                AffinePoint(radius2, phi + Math.PI / 2.0, out this.upMiddlePointRow, out this.upMiddlePointCol);
                AffinePoint(radius2, phi + Math.PI / 2.0 + Math.PI, out this.downMiddlePointRow, out this.downMiddlePointCol);
            }
            public void AffinePoint(double length, double phi, out double outRow, out double outCol)
            {
                HTuple homMat2dIdentity, homMat2dRotate, Qx, Qy;
                HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
                HOperatorSet.HomMat2dRotate(homMat2dIdentity, phi, this.row, this.col, out homMat2dRotate);
                HOperatorSet.AffineTransPoint2d(homMat2dRotate, this.row, this.col + length, out Qx, out Qy);
                outRow = Qx.D;
                outCol = Qy.D;
            }
            public HXLDCont GetHXLD(double nodeSize = 0)
            {
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenEllipseContourXld(this.row, this.col, this.phi, this.radius1 - nodeSize, this.radius2 - nodeSize, 0, Math.PI * 2, "positive", 0.05);
                return hXLDCont;
            }

        }


        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            if(this.ellipse.radius1 == 0)
                this.ellipse = new Ellipse(row1 + 100, column1 + 100, 0, 50, 40, this.nodeSizeRect);
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

        private void GetArrowPoint(double[] rows, double[] cols, double[] normalPhi, double normalLength, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            Row1 = new double[rows.Length];
            Col1 = new double[rows.Length];
            Row2 = new double[rows.Length];
            Col2 = new double[rows.Length];
            // 已知直线上的点坐标及该点的法线坐标，求法线直线的两点
            for (int i = 0; i < rows.Length; i++)
            {
                Row1[i] = rows[i] + normalLength * Math.Sin(normalPhi[i]); // 因为像素坐标系与世界坐标系Y轴相反，所以这里Y也要用相反的操作符
                Col1[i] = cols[i] - normalLength * Math.Cos(normalPhi[i]);
                Row2[i] = rows[i] - normalLength * Math.Sin(normalPhi[i]);
                Col2[i] = cols[i] + normalLength * Math.Cos(normalPhi[i]);
            }
        }

        private void GetNormal(Ellipse pixEllipse, int Num, out double[] Row, out double[] Col, out double[] Phi)
        {
            Row = new double[Num];
            Col = new double[Num];
            double[] Row2 = new double[Num];
            double[] Col2 = new double[Num];
            double step = Math.PI * 2 / (Num);
            for (int i = 0; i < Num; i++)
            {
                HTuple rowPoint1, colPoint1, rowPoint2, colPoint2;
                HOperatorSet.GetPointsEllipse(step * i, pixEllipse.row, pixEllipse.col, pixEllipse.phi, Math.Abs(pixEllipse.radius1), Math.Abs(pixEllipse.radius2), out rowPoint1, out colPoint1); //pixEllipse.diffRadius
                HOperatorSet.GetPointsEllipse(step * i, pixEllipse.row, pixEllipse.col, pixEllipse.phi, Math.Abs(pixEllipse.radius1 + 0.01), Math.Abs(pixEllipse.radius2 + 0.01), out rowPoint2, out colPoint2);
                if (rowPoint1.Length > 0)
                {
                    Row[i] = rowPoint1[0].D;
                    Col[i] = colPoint1[0].D;
                }
                else
                {
                    Row[i] = pixEllipse.row + pixEllipse.radius1;
                    Col[i] = pixEllipse.col + pixEllipse.radius1;
                }
                //////////
                if (rowPoint2.Length > 0)
                {
                    Row2[i] = rowPoint2[0].D;
                    Col2[i] = colPoint2[0].D;
                }
                else
                {
                    Row2[i] = pixEllipse.row + pixEllipse.radius1;
                    Col2[i] = pixEllipse.col + pixEllipse.radius1;
                }
            }
            //////////
            pixEllipse.normalPhi = new double[Row2.Length];
            Phi = new double[Row2.Length];
            for (int i = 0; i < Row2.Length; i++)
            {
                pixEllipse.normalPhi[i] = Math.Atan2((Row2[i] - Row[i]) * -1, Col2[i] - Col[i]);
                Phi[i] = pixEllipse.normalPhi[i];
            }

        }
        public HXLDCont GetArrowXLD()
        {
            double[] row1, col1, row2, col2, phi, row, col;
            HXLDCont hXLD = new HXLDCont();
            switch (this.ShowMode)
            {
                default:
                case enShowMode.箭头:
                    GetNormal(this.ellipse, 8, out row, out col, out phi);
                    GetArrowPoint(row, col, phi, this.ellipse.diffRadius, out row1, out col1, out row2, out col2);
                    hXLD = GenArrowContourXld(row1, col1, row2, col2, this.nodeSizeRect, this.nodeSizeRect);
                    break;
                case enShowMode.矩形:
                    GetNormal(this.ellipse, 8, out row, out col, out phi);
                    GetArrowPoint(row, col, phi, this.ellipse.diffRadius, out row1, out col1, out row2, out col2);
                    for (int i = 0; i < row1.Length; i++)
                    {
                        HXLDCont rect2 = new HXLDCont();
                        rect2.GenRectangle2ContourXld((row1[i] + row2[i]) * 0.5, (col1[i] + col2[i]) * 0.5, phi[i], this.ellipse.diffRadius, 5);
                        hXLD = hXLD.ConcatObj(rect2);
                    }
                    break;
            }
            return hXLD;
        }


        private void UpdataMeasureRegion()
        {
            /// 设置计量模型
            //if (this.backImage == null && this.wcsEllipse == null) return;
            //userPixEllipse EllipsePixPosition = new userPixEllipse(this.ellipse.row, this.ellipse.col, this.ellipse.phi, this.ellipse.radius1, this.ellipse.radius2, this.backImage.CamParams);
            //EllipsePixPosition.diffRadius = this.ellipse.diffRadius;
            //EllipsePixPosition.normalPhi = this.ellipse.normalPhi;
            //this.wcsEllipse = EllipsePixPosition.GetWcsEllipse(this.backImage.Grab_X, this.backImage.Grab_Y).Affine2DWcsEllipse(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
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
                        //this.wcsEllipse.Execute(null);
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
        public override userPixEllipse GetPixEllipseParam()
        {
            userPixEllipse pixEllipse = new userPixEllipse(this.ellipse.row, this.ellipse.col, this.ellipse.phi,this.ellipse.radius1, this.ellipse.radius2,  this.backImage?.CamParams);
            pixEllipse.DiffRadius = this.ellipse.diffRadius;
            pixEllipse.NormalPhi = this.ellipse.normalPhi;
            pixEllipse = pixEllipse.AffineTransPixEllipse(this.pixCoordSystem.GetInvertVariationHomMat2D());
            return pixEllipse;
        }
        public override userWcsEllipse GetWcsEllipseParam()
        {
            userWcsEllipse wcsEllipse = GetPixEllipseParam().GetWcsEllipse(this.backImage.Grab_X, this.backImage.Grab_Y).AffineWcsEllipse2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            return wcsEllipse;
        }

    }
}
