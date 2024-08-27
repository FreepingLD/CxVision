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
    public class userDrawCircleSectorMeasure : DrawingBaseMeasure
    {
        private int state = 0;
        private CircleSector currentDrawCircle;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;
        /// <summary>
        /// 可调大小节点的位置
        /// </summary>
        private enum PosSizableRect
        {
            MiddlePoint,
            StartPoint,
            EndPoint,
            CircleCenter,
            InnerCircle,
            MiddleCircle,
            OutSideCircle,
            None,
        }



        public userDrawCircleSectorMeasure(HWindowControl hWindowControl) : base(hWindowControl)
        {

        }
        public userDrawCircleSectorMeasure(HWindowControl hWindowControl, userWcsCircleSector wcsCircleSector, userWcsCoordSystem wcsCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsCircleSector != null && wcsCircleSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.wcsCoordSystem = wcsCoordSystem;
                this.pixCoordSystem = wcsCoordSystem.GetPixCoordSystem();
                userPixCircleSector pixCircleSector = wcsCircleSector.GetPixCircleSector();
                this.currentDrawCircle = new CircleSector(pixCircleSector.Row, pixCircleSector.Col, pixCircleSector.Radius, pixCircleSector.Start_phi, pixCircleSector.End_phi, pixCircleSector.DiffRadius, pixCircleSector.PointOrder);
                this.currentDrawCircle.diffRadius = wcsCircleSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.currentDrawCircle = new CircleSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06, 0, 3.14, this.nodeSizeRect, "positive");
                this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius * 0.5;
            }
        }
        public userDrawCircleSectorMeasure(HWindowControl hWindowControl, userPixCircleSector pixCircleSector, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixCircleSector != null && pixCircleSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCoordSystem = pixCoordSystem;
                userPixCircleSector pixCircleSector2 = pixCircleSector.AffineTransPixCircleSector(pixCoordSystem.GetVariationHomMat2D());//.GetPixCircleSector();
                this.currentDrawCircle = new CircleSector(pixCircleSector2.Row, pixCircleSector2.Col, pixCircleSector2.Radius, pixCircleSector2.Start_phi, pixCircleSector2.End_phi, pixCircleSector2.DiffRadius, pixCircleSector2.PointOrder);
                this.currentDrawCircle.diffRadius = pixCircleSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.currentDrawCircle = new CircleSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06, 0, 3.14, this.nodeSizeRect, "positive");
                this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius * 0.5;
            }
        }
        public userDrawCircleSectorMeasure(HWindowControl hWindowControl, userWcsCircleSector wcsCircleSector) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsCircleSector != null && wcsCircleSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                userPixCircleSector pixCircleSector = wcsCircleSector.GetPixCircleSector();
                this.currentDrawCircle = new CircleSector(pixCircleSector.Row, pixCircleSector.Col, pixCircleSector.Radius, pixCircleSector.Start_phi, pixCircleSector.End_phi, pixCircleSector.DiffRadius, pixCircleSector.PointOrder);
                this.currentDrawCircle.diffRadius = wcsCircleSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.currentDrawCircle = new CircleSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06, 0, 3.14, this.nodeSizeRect, "positive");
                this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius * 0.5;
            }
        }
        public userDrawCircleSectorMeasure(HWindowControl hWindowControl, userPixCircleSector pixCircleSector) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixCircleSector != null && pixCircleSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //userPixCircleSector pixCircleSector = wcsCircleSector.GetPixCircleSector();
                this.currentDrawCircle = new CircleSector(pixCircleSector.Row, pixCircleSector.Col, pixCircleSector.Radius, pixCircleSector.Start_phi, pixCircleSector.End_phi, pixCircleSector.DiffRadius, pixCircleSector.PointOrder);
                this.currentDrawCircle.diffRadius = pixCircleSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.currentDrawCircle = new CircleSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06, 0, 3.14, this.nodeSizeRect, "positive");
                this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius * 0.5;
            }
        }
        public override void SetParam(object param)
        {
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixCircleSector):
                        if (!(param is userPixCircleSector)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixCircleSector 类型");
                        userPixCircleSector pixCircleSector = (userPixCircleSector)param;
                        if (pixCircleSector.CamParams != null)
                        {
                            //userPixCircleSector pixCircleSector = pixCircleSector;//.GetPixCircleSector();
                            this.currentDrawCircle = new CircleSector(pixCircleSector.Row, pixCircleSector.Col, pixCircleSector.Radius, pixCircleSector.Start_phi, pixCircleSector.End_phi, pixCircleSector.DiffRadius, pixCircleSector.PointOrder);
                            this.currentDrawCircle.diffRadius = pixCircleSector.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.currentDrawCircle = new CircleSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06, 0, 3.14, this.nodeSizeRect, "positive");
                            this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius * 0.5;
                        }
                        break;
                    case nameof(userWcsCircleSector):
                        if (!(param is userWcsCircleSector)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsCircleSector 类型");
                        userWcsCircleSector wcsCircleSector = (userWcsCircleSector)param;
                        if (wcsCircleSector.CamParams != null)
                        {
                            pixCircleSector = wcsCircleSector.GetPixCircleSector();
                            this.currentDrawCircle = new CircleSector(pixCircleSector.Row, pixCircleSector.Col, pixCircleSector.Radius, pixCircleSector.Start_phi, pixCircleSector.End_phi, pixCircleSector.DiffRadius, pixCircleSector.PointOrder);
                            this.currentDrawCircle.diffRadius = pixCircleSector.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.currentDrawCircle = new CircleSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06, 0, 3.14, this.nodeSizeRect, "positive");
                            this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius * 0.5;
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
                this.currentDrawCircle = new CircleSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06, 0, 3.14, this.nodeSizeRect, "positive");
                this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius * 0.5;
                this.wcsCoordSystem = new userWcsCoordSystem();
                this.pixCoordSystem = new userPixCoordSystem();
            }
        }
        protected override void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:

                    state = 3;
                    selectedNode = PosSizableRect.None; // 先让节点处于非选中状态
                    selectedNode = GetNodeSelectable(e.X, e.Y);
                    ////// 判断是平移还是移动对象
                    if (selectedNode == PosSizableRect.None  || this.isDrwingObject == false)
                        this.isTranslate = true;
                    else
                        this.isTranslate = false;
                    /////////////
                    base.hWindowControl_HMouseDown(sender, e); // 只有在按下左键时才
                    break;
                case MouseButtons.Middle:
                    //if (this.wcsCircleSector != null)
                    //    this.wcsCircleSector.Execute(null);
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
            //////////////////////
            ChangeCursor(e.X, e.Y); // 在移动过程中改变光标
            /////////////////////////
            if (mIsClick == false || IsTranslate) // 只有在鼠标按下的状态下才执行移动
            {
                return;
            }
            ////////////////////
            switch (selectedNode)
            {

                case PosSizableRect.StartPoint:
                    this.currentDrawCircle.startPointCol = e.X;
                    this.currentDrawCircle.startPointRow = e.Y;
                    break;

                case PosSizableRect.EndPoint:
                    this.currentDrawCircle.endPointCol = e.X;
                    this.currentDrawCircle.endPointRow = e.Y;
                    break;

                case PosSizableRect.MiddlePoint:
                    this.currentDrawCircle.middlePointCol = e.X;
                    this.currentDrawCircle.middlePointRow = e.Y;
                    break;

                case PosSizableRect.InnerCircle:
                    this.currentDrawCircle.diffRadius = this.currentDrawCircle.radius - HMisc.DistancePp(this.currentDrawCircle.row, this.currentDrawCircle.col, e.Y, e.X);
                    break;
                case PosSizableRect.MiddleCircle:
                    this.currentDrawCircle.radius = HMisc.DistancePp(this.currentDrawCircle.row, this.currentDrawCircle.col, e.Y, e.X);
                    break;
                case PosSizableRect.OutSideCircle:
                    this.currentDrawCircle.diffRadius = HMisc.DistancePp(this.currentDrawCircle.row, this.currentDrawCircle.col, e.Y, e.X) - this.currentDrawCircle.radius;
                    break;

                case PosSizableRect.CircleCenter:
                    this.currentDrawCircle.startPointCol += e.X - oldX;
                    this.currentDrawCircle.startPointRow += e.Y - oldY;
                    this.currentDrawCircle.endPointCol += e.X - oldX;
                    this.currentDrawCircle.endPointRow += e.Y - oldY;
                    this.currentDrawCircle.middlePointCol += e.X - oldX;
                    this.currentDrawCircle.middlePointRow += e.Y - oldY;
                    break;
            }
            //上一点需要实时更新，这样一来图像才不会乱
            oldX = e.X;
            oldY = e.Y;
            //重绘图形
            if (mIsClick)
            {
                FitCircleParam();
                UpdataMeasureRegion();
                DrawingGraphicObject();
            }

        }


        /// <summary>
        /// 绘制整个绘图对象
        /// </summary>
        public override void DrawingGraphicObject()
        {
            if (this.currentDrawCircle.radius != 0) // 半径等于0，表示在绘图模式下
            {
                this.AttachDrawingPropertyData.Clear();
                ///////////////// 将图中所有节点位置的矩形绘制出来 //////////////////////
                foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
                {
                    if (this.isDispalyAttachDrawingProperty)
                        this.AttachDrawingPropertyData.Add(GetDrawingObject(pos));
                }
            }
            base.DrawingGraphicObject();
        }


        private void FitCircleParam()
        {
            //////////////        
            string PointOrder;
            double Row, Column, Radius, StartPhi, EndPhi;
            HXLDCont xld = new HXLDCont(new HTuple(this.currentDrawCircle.startPointRow, this.currentDrawCircle.middlePointRow, this.currentDrawCircle.endPointRow),
                               new HTuple(this.currentDrawCircle.startPointCol, this.currentDrawCircle.middlePointCol, this.currentDrawCircle.endPointCol));
            ////////////////////////////////////////////////////////////////
            double phi1 = Math.Atan2(this.currentDrawCircle.middlePointRow - this.currentDrawCircle.startPointRow, this.currentDrawCircle.middlePointCol - this.currentDrawCircle.startPointCol);
            double phi2 = Math.Atan2(this.currentDrawCircle.endPointRow - this.currentDrawCircle.startPointRow, this.currentDrawCircle.endPointCol - this.currentDrawCircle.startPointCol);
            if (Math.Abs(phi1) != Math.Abs(phi2) && phi1 != 0 && phi2 != 0)  // 三点不能在同一直线上
            {
                xld.FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder); // 中点的位置决定的轮廓点的顺序
                xld.Dispose();
                this.currentDrawCircle.row = Row;
                this.currentDrawCircle.col = Column;
                this.currentDrawCircle.radius = Radius;
                this.currentDrawCircle.pointOrder = PointOrder;
            }
        }

        private HXLDCont CreateCircleSectorObject()
        {
            HXLDCont xld = new HXLDCont();
            if (this.currentDrawCircle.radius != 0)
            {
                // 因为移动任何一个节点都会改变圆心位置和半径大小，所以需要重新计算起始和结束角度，因为像素坐标系与世界坐标系Y轴相反，所以计算的角度值需要取反
                this.currentDrawCircle.start_phi = Math.Atan2(this.currentDrawCircle.startPointRow - this.currentDrawCircle.row, this.currentDrawCircle.startPointCol - this.currentDrawCircle.col) * -1;
                this.currentDrawCircle.end_phi = Math.Atan2(this.currentDrawCircle.endPointRow - this.currentDrawCircle.row, this.currentDrawCircle.endPointCol - this.currentDrawCircle.col) * -1;
                this.currentDrawCircle.mid_phi = Math.Atan2(this.currentDrawCircle.middlePointRow - this.currentDrawCircle.row, this.currentDrawCircle.middlePointCol - this.currentDrawCircle.col) * -1;
                ////////////////////////////////////////////////////
                xld.GenCircleContourXld(this.currentDrawCircle.row, this.currentDrawCircle.col, this.currentDrawCircle.radius, this.currentDrawCircle.start_phi, this.currentDrawCircle.end_phi, this.currentDrawCircle.pointOrder, 0.05); //"positive": 默认就为逆时针方向
            }
            return xld;
        }
        private HXLDCont GetDrawingObject(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.StartPoint:
                    return CreateRectSizableNode(this.currentDrawCircle.startPointCol, this.currentDrawCircle.startPointRow);
                case PosSizableRect.MiddlePoint:
                    return CreateRectSizableNode(this.currentDrawCircle.middlePointCol, this.currentDrawCircle.middlePointRow);
                case PosSizableRect.EndPoint:
                    return CreateRectSizableNode(this.currentDrawCircle.endPointCol, this.currentDrawCircle.endPointRow);
                case PosSizableRect.None:
                    return GetArrowXLD();
                case PosSizableRect.MiddleCircle:
                    return CreateCircleSectorObject();
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double dist = Math.Sqrt((y - this.currentDrawCircle.row) * (y - this.currentDrawCircle.row) + (x - this.currentDrawCircle.col) * (x - this.currentDrawCircle.col));
            double dist1 = Math.Sqrt((y - this.currentDrawCircle.startPointRow) * (y - this.currentDrawCircle.startPointRow) + (x - this.currentDrawCircle.startPointCol) * (x - this.currentDrawCircle.startPointCol));
            double dist2 = Math.Sqrt((y - this.currentDrawCircle.middlePointRow) * (y - this.currentDrawCircle.middlePointRow) + (x - this.currentDrawCircle.middlePointCol) * (x - this.currentDrawCircle.middlePointCol));
            double dist3 = Math.Sqrt((y - this.currentDrawCircle.endPointRow) * (y - this.currentDrawCircle.endPointRow) + (x - this.currentDrawCircle.endPointCol) * (x - this.currentDrawCircle.endPointCol));
            //////////////////////////////////
            if (dist < (this.currentDrawCircle.radius - Math.Abs(this.currentDrawCircle.diffRadius)) * 0.8)
                selectNode = PosSizableRect.CircleCenter;
            //////////////////////////////////////////////////
            if ((this.currentDrawCircle.radius - this.currentDrawCircle.diffRadius) * 0.9 < dist && (this.currentDrawCircle.radius - this.currentDrawCircle.diffRadius) * 1.1 > dist)
                selectNode = PosSizableRect.InnerCircle;
            //////////////////////////////////////////////////
            if ((this.currentDrawCircle.radius) * 0.9 < dist && (this.currentDrawCircle.radius) * 1.1 > dist)
                selectNode = PosSizableRect.MiddleCircle;
            //////////////////////////////////////////////////
            if ((this.currentDrawCircle.radius + this.currentDrawCircle.diffRadius) * 0.9 < dist && (this.currentDrawCircle.radius + this.currentDrawCircle.diffRadius) * 1.1 > dist)
                selectNode = PosSizableRect.OutSideCircle;
            ////////////////////////////////////////////////// 第一点
            //if ((this.currentDrawCircle.radius) * 0.9 < dist && (this.currentDrawCircle.radius) * 1.1 > dist && dist1 < (this.currentDrawCircle.radius) * 0.1)
            //    selectNode = PosSizableRect.StartPoint;
            ////////////////////////////////////////////////////第二点
            //if ((this.currentDrawCircle.radius) * 0.9 < dist && (this.currentDrawCircle.radius) * 1.1 > dist && dist2 < (this.currentDrawCircle.radius) * 0.1)
            //    selectNode = PosSizableRect.MiddlePoint;
            ////////////////////////////////////////////////////第三点
            //if ((this.currentDrawCircle.radius) * 0.9 < dist && (this.currentDrawCircle.radius) * 1.1 > dist && dist3 < (this.currentDrawCircle.radius) * 0.1)
            //    selectNode = PosSizableRect.EndPoint;

            if ( dist1 <= this.nodeSizeRect)
                selectNode = PosSizableRect.StartPoint;
            //////////////////////////////////////////////////第二点
            if ( dist2 <= this.nodeSizeRect)
                selectNode = PosSizableRect.MiddlePoint;
            //////////////////////////////////////////////////第三点
            if ( dist3 <= this.nodeSizeRect)
                selectNode = PosSizableRect.EndPoint;
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
                /////////////////////////////////
                case PosSizableRect.InnerCircle:
                case PosSizableRect.MiddleCircle:
                case PosSizableRect.OutSideCircle:
                    return Cursors.Cross;

                case PosSizableRect.StartPoint:
                case PosSizableRect.EndPoint:
                case PosSizableRect.MiddlePoint:  //MovePos
                    return Cursors.Default;
                default:
                    return Cursors.Default;
            }
        }

        private struct CircleSector
        {
            public double startPointRow;
            public double startPointCol;
            public double middlePointRow;
            public double middlePointCol;
            public double endPointRow;
            public double endPointCol;
            public double row;
            public double col;
            public double radius;
            public double start_phi;
            public double mid_phi;
            public double end_phi;
            public double diffRadius;
            public double[] normalPhi;
            public string pointOrder;

            public CircleSector(double row, double col, double radius, double start_phi, double end_phi, double diffRadius, string pointOrder)
            {
                this.row = row;
                this.col = col;
                this.radius = radius;
                this.diffRadius = diffRadius;
                this.start_phi = start_phi;
                this.mid_phi = 0;
                this.end_phi = end_phi;
                this.pointOrder = pointOrder; // 
                this.startPointRow = row - radius * Math.Sin(start_phi);
                this.startPointCol = col + radius * Math.Cos(start_phi);
                this.endPointRow = row - radius * Math.Sin(end_phi);
                this.endPointCol = col + radius * Math.Cos(end_phi);
                this.middlePointRow = row + radius * Math.Sin((start_phi + end_phi) * 0.5);
                this.middlePointCol = col - radius * Math.Cos((start_phi + end_phi) * 0.5);
                this.normalPhi = null;
                ////////////////////////////////////  统一角度方向
                switch (pointOrder)
                {
                    default:
                    case "positive":
                        if (this.start_phi < 0)
                            this.start_phi += Math.PI * 2; // 逆时针方向用正角度表示
                        if (this.end_phi < 0)
                            this.end_phi += Math.PI * 2; // 逆时针方向用正角度表示
                        break;
                    case "negative": // 顺时针方向的用负角度表示，
                        if (this.start_phi > 0)
                            this.start_phi -= Math.PI * 2;
                        if (this.end_phi > 0)
                            this.end_phi -= Math.PI * 2;
                        break;
                }
                ///////////  统一角度范围 
                double Sx, Sy, Phi, Theta, Tx, Ty;
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.VectorAngleToRigid(this.row, this.col, start_phi, this.row, this.col, end_phi);
                hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
                switch (this.pointOrder)
                {
                    default:
                    case "positive":
                        if (Phi < 0)
                            Phi += Math.PI * 2;
                        break;
                    case "negative": //
                        if (Phi > 0)
                            Phi -= Math.PI * 2;
                        break;
                }
                this.mid_phi = this.start_phi + (Phi) / 2;
                //////////////////////////
                AffinePoint(this.row, this.col, this.start_phi, this.radius, out this.startPointRow, out this.startPointCol);
                AffinePoint(this.row, this.col, this.mid_phi, this.radius, out this.middlePointRow, out this.middlePointCol);
                AffinePoint(this.row, this.col, this.end_phi, this.radius, out this.endPointRow, out this.endPointCol);              
            }

            private void AffinePoint(double row, double col, double phi, double radius, out double affinRow, out double affinCol)
            {
                affinRow = row;
                affinCol = col;
                double Qx = 0, Qy = 0;
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.HomMat2dIdentity();
                HHomMat2D hHomMat2DRotate = hHomMat2D.HomMat2dRotate(phi, row, col);
                Qx = hHomMat2DRotate.AffineTransPoint2d(row, col + radius, out Qy);
                affinRow = Qx;
                affinCol = Qy;
            }
        }

        private void AffinePoint(double row, double col, double phi, double radius, out double affinRow, out double affinCol)
        {
            affinRow = row;
            affinCol = col;
            double Qx = 0, Qy = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.HomMat2dIdentity();
            HHomMat2D hHomMat2DRotate = hHomMat2D.HomMat2dRotate(phi, row, col);
            Qx = hHomMat2DRotate.AffineTransPoint2d(row, col + radius, out Qy);
            affinRow = Qx;
            affinCol = Qy;
        }
        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            if (this.currentDrawCircle.radius == 0)
                this.currentDrawCircle = new CircleSector(row1 + 100, column1 + 100, 50, 0, 3.14, this.nodeSizeRect, "positive");
            base.AttachDrawingObjectToWindow();
        }
        public override void DetachDrawingObjectFromWindow()
        {
            //this.currentDrawCircle = new CircleSector();
            base.DetachDrawingObjectFromWindow();
        }
        public override void ClearDrawingObject()
        {
            base.ClearDrawingObject();
        }
        private void GetArrowPoint(CircleSector pixCircle, int Num, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            Row1 = new double[Num];
            Col1 = new double[Num];
            Row2 = new double[Num];
            Col2 = new double[Num];
            double step, start_phi, end_phi;
            start_phi = pixCircle.start_phi;
            end_phi = pixCircle.end_phi;
            switch (pixCircle.pointOrder)
            {
                default:
                case "positive":
                    if (start_phi < 0)
                        start_phi += Math.PI * 2; // 逆时针方向用正角度表示
                    if (end_phi < 0)
                        end_phi += Math.PI * 2; // 逆时针方向用正角度表示
                    break;
                case "negative": // 顺时针方向的用负角度表示，
                    if (start_phi > 0)
                        start_phi -= Math.PI * 2;
                    if (end_phi > 0)
                        end_phi -= Math.PI * 2;
                    break;
            }
            double Sx, Sy, Phi, Theta, Tx, Ty;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(pixCircle.row, pixCircle.col, start_phi, pixCircle.row, pixCircle.col, end_phi);
            hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            switch (pixCircle.pointOrder)
            {
                default:
                case "positive":
                    if (Phi < 0)
                        Phi += Math.PI * 2;
                    break;
                case "negative": // 顺时针方向的用负角度表示，
                    if (Phi > 0)
                        Phi -= Math.PI * 2;
                    break;
            }
            step = (Phi) / (Num - 1);
            //////////////
            pixCircle.normalPhi = new double[Num];
            for (int i = 0; i < Num; i++)
            {
                // 利用三个角度来绘制图形
                switch (i)
                {
                    case 0:
                        AffinePoint(pixCircle.row, pixCircle.col, pixCircle.start_phi, (pixCircle.radius - pixCircle.diffRadius), out Row1[i], out Col1[i]);
                        AffinePoint(pixCircle.row, pixCircle.col, pixCircle.start_phi, (pixCircle.radius + pixCircle.diffRadius), out Row2[i], out Col2[i]);
                        break;
                    case 1:
                        AffinePoint(pixCircle.row, pixCircle.col, pixCircle.mid_phi, (pixCircle.radius - pixCircle.diffRadius), out Row1[i], out Col1[i]);
                        AffinePoint(pixCircle.row, pixCircle.col, pixCircle.mid_phi, (pixCircle.radius + pixCircle.diffRadius), out Row2[i], out Col2[i]);
                        break;
                    case 2:
                        AffinePoint(pixCircle.row, pixCircle.col, pixCircle.end_phi, (pixCircle.radius - pixCircle.diffRadius), out Row1[i], out Col1[i]);
                        AffinePoint(pixCircle.row, pixCircle.col, pixCircle.end_phi, (pixCircle.radius + pixCircle.diffRadius), out Row2[i], out Col2[i]);
                        break;
                }
                ////////////////////////////////////////////////////////
                // old 方法
                //Row1[i] = pixCircle.row - (pixCircle.radius - pixCircle.diffRadius) * Math.Sin(step * i + start_phi);
                //Col1[i] = pixCircle.col + (pixCircle.radius - pixCircle.diffRadius) * Math.Cos(step * i + start_phi);
                ////////////
                //Row2[i] = pixCircle.row - (pixCircle.radius + pixCircle.diffRadius) * Math.Sin(step * i + start_phi);
                //Col2[i] = pixCircle.col + (pixCircle.radius + pixCircle.diffRadius) * Math.Cos(step * i + start_phi);
                /////
                pixCircle.normalPhi[i] = Math.Atan2((Row2[i] - Row1[i]) * -1, Col2[i] - Col1[i]);
            }

        }
        public HXLDCont GetArrowXLD()
        {
            double[] row1, col1, row2, col2;
            HXLDCont hXLD = new HXLDCont();
            switch (this.ShowMode)
            {
                default:
                case enShowMode.箭头:
                    GetArrowPoint(this.currentDrawCircle, 3, out row1, out col1, out row2, out col2);
                    hXLD = GenArrowContourXld(row1, col1, row2, col2, this.nodeSizeRect, this.nodeSizeRect);
                    break;
                case enShowMode.矩形:
                    GetArrowPoint(this.currentDrawCircle, 3, out row1, out col1, out row2, out col2);
                    for (int i = 0; i < row1.Length; i++)
                    {
                        HXLDCont rect2 = new HXLDCont();
                        rect2.GenRectangle2ContourXld((row1[i] + row2[i]) * 0.5, (col1[i] + col2[i]) * 0.5, this.currentDrawCircle.normalPhi[i], this.currentDrawCircle.diffRadius, 5);
                        hXLD = hXLD.ConcatObj(rect2);
                    }
                    break;
            }
            return hXLD;
        }
        private void UpdataMeasureRegion()
        {
            /// 设置计量模型
            //if (this.backImage == null && this.findCircleSector == null) return;
            /////////////////////////////
            //userPixCircleSector pixCircleSector = new userPixCircleSector(this.drawCircle.row, this.drawCircle.col, this.drawCircle.radius, this.drawCircle.start_phi, this.drawCircle.end_phi, this.backImage.CamParam, this.backImage.CamPose);
            //pixCircleSector.diffRadius = this.drawCircle.diffRadius;
            //pixCircleSector.normalPhi = this.drawCircle.normalPhi;
            //pixCircleSector.pointOrder = this.drawCircle.pointOrder;
            //this.findCircleSector.CircleSectorWcsPosition = pixCircleSector.GetWcsCircleSector(this.backImage.X_pos, this.backImage.Y_pos).Affine2DWcsCircleSector(this.findCircleSector.WcsCoordSystem.GetInvertVariationHomMat2D(), enAffineTransOrientation.反向变换);
            /////////////////
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
                        //this.wcsCircleSector.Execute(null);
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

        public override userPixCircleSector GetPixCircleSectorParam()
        {
            userPixCircleSector circlePixPosition = new userPixCircleSector(this.currentDrawCircle.row, this.currentDrawCircle.col, this.currentDrawCircle.radius, this.currentDrawCircle.start_phi, this.currentDrawCircle.end_phi, this.backImage?.CamParams);
            circlePixPosition.DiffRadius = this.currentDrawCircle.diffRadius;
            circlePixPosition.NormalPhi = this.currentDrawCircle.normalPhi;
            circlePixPosition.PointOrder = this.currentDrawCircle.pointOrder;
            circlePixPosition = circlePixPosition.AffineTransPixCircleSector(this.pixCoordSystem.GetInvertVariationHomMat2D());
            return circlePixPosition;
        }
        public override userWcsCircleSector GetWcsCircleSectorParam()
        {
            userWcsCircleSector circleWcsPosition = GetPixCircleSectorParam().GetWcsCircleSector(this.backImage.Grab_X, this.backImage.Grab_Y).Affine2DWcsCircleSector(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            return circleWcsPosition;
        }



    }





}
