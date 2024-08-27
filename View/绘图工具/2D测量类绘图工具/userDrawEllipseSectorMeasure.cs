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
    public class userDrawEllipseSectorMeasure : DrawingBaseMeasure
    {
        private int state = 0;
        private EllipseSector ellipseSector;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        //private userWcsEllipseSector wcsEllipseSector;
        private userWcsCoordSystem wcsCoordSystem;
        private userPixCoordSystem pixCoordSystem;

        /// <summary>
        /// 可调大小节点的位置
        /// </summary>
        private enum PosSizableRect
        {
            UpMiddle,
            DownMiddle,
            LeftBottom,
            RightBottom,
            secondNode,
            forNode,
            CircleCenter,
            InnerCircle,
            OutSideCircle,
            Arrow,
            None,
        }


        public userDrawEllipseSectorMeasure(HWindowControl hWindowControl) : base(hWindowControl)
        {

        }

        public userDrawEllipseSectorMeasure(HWindowControl hWindowControl, userWcsEllipseSector wcsEllipseSector, userWcsCoordSystem wcsCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsEllipseSector != null && wcsEllipseSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.wcsCoordSystem = wcsCoordSystem;
                userPixEllipseSector PixEllipseSector = wcsEllipseSector.Affine2DWcsEllipseSector(wcsCoordSystem.GetVariationHomMat2D()).GetPixEllipseSector();
                this.ellipseSector = new EllipseSector(PixEllipseSector.Row, PixEllipseSector.Col, PixEllipseSector.Rad, PixEllipseSector.Radius1, PixEllipseSector.Radius2,
                   PixEllipseSector.Start_phi, PixEllipseSector.End_phi, PixEllipseSector.DiffRadius);
                this.ellipseSector.diffRadius = wcsEllipseSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipseSector = new EllipseSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, 0, 3.14, this.nodeSizeRect);
                this.ellipseSector.diffRadius = this.ellipseSector.radius2 * 0.5;
            }
        }
        public userDrawEllipseSectorMeasure(HWindowControl hWindowControl, userPixEllipseSector pixEllipseSector, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixEllipseSector != null && pixEllipseSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCoordSystem = pixCoordSystem;
                userPixEllipseSector PixEllipseSector = pixEllipseSector.AffineTransPixEllipseSector(wcsCoordSystem.GetVariationHomMat2D());//.GetPixEllipseSector();
                this.ellipseSector = new EllipseSector(PixEllipseSector.Row, PixEllipseSector.Col, PixEllipseSector.Rad, PixEllipseSector.Radius1, PixEllipseSector.Radius2,
                   PixEllipseSector.Start_phi, PixEllipseSector.End_phi, PixEllipseSector.DiffRadius);
                this.ellipseSector.diffRadius = pixEllipseSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipseSector = new EllipseSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, 0, 3.14, this.nodeSizeRect);
                this.ellipseSector.diffRadius = this.ellipseSector.radius2 * 0.5;
            }
        }
        public userDrawEllipseSectorMeasure(HWindowControl hWindowControl, userWcsEllipseSector wcsEllipseSector) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsEllipseSector != null && wcsEllipseSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                userPixEllipseSector PixEllipseSector = wcsEllipseSector.GetPixEllipseSector();
                this.ellipseSector = new EllipseSector(PixEllipseSector.Row, PixEllipseSector.Col, PixEllipseSector.Rad, PixEllipseSector.Radius1, PixEllipseSector.Radius2,
                   PixEllipseSector.Start_phi, PixEllipseSector.End_phi, PixEllipseSector.DiffRadius);
                this.ellipseSector.diffRadius = wcsEllipseSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipseSector = new EllipseSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, 0, 3.14, this.nodeSizeRect);
                this.ellipseSector.diffRadius = this.ellipseSector.radius2 * 0.5;
            }
        }
        public userDrawEllipseSectorMeasure(HWindowControl hWindowControl, userPixEllipseSector pixEllipseSector) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (pixEllipseSector != null && pixEllipseSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //userPixEllipseSector PixEllipseSector = pixEllipseSector.GetPixEllipseSector();
                this.ellipseSector = new EllipseSector(pixEllipseSector.Row, pixEllipseSector.Col, pixEllipseSector.Rad, pixEllipseSector.Radius1, pixEllipseSector.Radius2,
                   pixEllipseSector.Start_phi, pixEllipseSector.End_phi, pixEllipseSector.DiffRadius);
                this.ellipseSector.diffRadius = pixEllipseSector.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.ellipseSector = new EllipseSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, 0, 3.14, this.nodeSizeRect);
                this.ellipseSector.diffRadius = this.ellipseSector.radius2 * 0.5;
            }
        }

        public override void SetParam(object param)
        {
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixEllipseSector):
                        if (!(param is userPixEllipseSector)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixEllipseSector 类型");
                        userPixEllipseSector PixEllipseSector = (userPixEllipseSector)param;
                        if (PixEllipseSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
                        {
                            //userPixEllipseSector PixEllipseSector = PixEllipseSector;//.GetPixEllipseSector();
                            this.ellipseSector = new EllipseSector(PixEllipseSector.Row, PixEllipseSector.Col, PixEllipseSector.Rad, PixEllipseSector.Radius1, PixEllipseSector.Radius2,
                                   PixEllipseSector.Start_phi, PixEllipseSector.End_phi, PixEllipseSector.DiffRadius);
                            this.ellipseSector.diffRadius = PixEllipseSector.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.ellipseSector = new EllipseSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, 0, 3.14, this.nodeSizeRect);
                            this.ellipseSector.diffRadius = this.ellipseSector.radius2 * 0.5;
                        }
                        break;
                    case nameof(userWcsEllipseSector):
                        if (!(param is userWcsEllipseSector)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsEllipseSector 类型");
                        userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)param;
                        if (wcsEllipseSector.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
                        {
                             PixEllipseSector = wcsEllipseSector.GetPixEllipseSector();
                            this.ellipseSector = new EllipseSector(PixEllipseSector.Row, PixEllipseSector.Col, PixEllipseSector.Rad, PixEllipseSector.Radius1, PixEllipseSector.Radius2,
                                   PixEllipseSector.Start_phi, PixEllipseSector.End_phi, PixEllipseSector.DiffRadius);
                            this.ellipseSector.diffRadius = PixEllipseSector.DiffRadius;
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.ellipseSector = new EllipseSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, 0, 3.14, this.nodeSizeRect);
                            this.ellipseSector.diffRadius = this.ellipseSector.radius2 * 0.5;
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
                this.ellipseSector = new EllipseSector(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.07, (row2 - row1) * 0.05, 0, 3.14, this.nodeSizeRect);
                this.ellipseSector.diffRadius = this.ellipseSector.radius2 * 0.5;
                this.wcsCoordSystem = new userWcsCoordSystem();
                this.pixCoordSystem = new userPixCoordSystem();
            }
        }
        protected override void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    state = 5;
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
                    //if (this.wcsEllipseSector != null)
                    //    this.wcsEllipseSector.Execute(null);
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
                case PosSizableRect.DownMiddle:
                case PosSizableRect.UpMiddle:
                    this.ellipseSector.radius2 += HMisc.DistancePp(e.Y, e.X, this.ellipseSector.row, this.ellipseSector.col) - HMisc.DistancePp(this.oldY, this.oldX, this.ellipseSector.row, this.ellipseSector.col);
                    if (this.ellipseSector.radius1 < 5)
                        this.ellipseSector.radius1 = 5;
                    if (this.ellipseSector.radius2 < 5)
                        this.ellipseSector.radius2 = 5;
                    //////////////////////
                    this.ellipseSector.firstPointCol = this.ellipseSector.col - this.ellipseSector.radius2 - this.ellipseSector.radius1;
                    this.ellipseSector.firstPointRow = this.ellipseSector.row;
                    this.ellipseSector.thirdPointCol = this.ellipseSector.col + this.ellipseSector.radius2 + this.ellipseSector.radius1;
                    this.ellipseSector.thirdPointRow = this.ellipseSector.row;
                    this.ellipseSector.secondPointCol = this.ellipseSector.col;
                    this.ellipseSector.secondPointRow = this.ellipseSector.row + this.ellipseSector.radius1 * -1;
                    /////////////////////////////////
                    this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi + Math.PI, out this.ellipseSector.firstPointRow, out this.ellipseSector.firstPointCol);
                    this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi, out this.ellipseSector.thirdPointRow, out this.ellipseSector.thirdPointCol);
                    this.ellipseSector.AffinePoint(this.ellipseSector.radius2, this.ellipseSector.phi + Math.PI / 2.0, out this.ellipseSector.secondPointRow, out this.ellipseSector.secondPointCol);
                    break;

                case PosSizableRect.LeftBottom:
                case PosSizableRect.RightBottom:
                    this.ellipseSector.radius1 += HMisc.DistancePp(e.Y, e.X, this.ellipseSector.row, this.ellipseSector.col) - HMisc.DistancePp(this.oldY, this.oldX, this.ellipseSector.row, this.ellipseSector.col);
                    if (this.ellipseSector.radius1 < 5)
                        this.ellipseSector.radius1 = 5;
                    if (this.ellipseSector.radius2 < 5)
                        this.ellipseSector.radius2 = 5;
                    //////////////////////
                    this.ellipseSector.firstPointCol = this.ellipseSector.col - this.ellipseSector.radius2 - this.ellipseSector.radius1;
                    this.ellipseSector.firstPointRow = this.ellipseSector.row;
                    this.ellipseSector.thirdPointCol = this.ellipseSector.col + this.ellipseSector.radius2 + this.ellipseSector.radius1;
                    this.ellipseSector.thirdPointRow = this.ellipseSector.row;
                    this.ellipseSector.secondPointCol = this.ellipseSector.col;
                    this.ellipseSector.secondPointRow = this.ellipseSector.row + this.ellipseSector.radius1 * -1;
                    /////////////////////////////////
                    this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi + Math.PI, out this.ellipseSector.firstPointRow, out this.ellipseSector.firstPointCol);
                    this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi, out this.ellipseSector.thirdPointRow, out this.ellipseSector.thirdPointCol);
                    this.ellipseSector.AffinePoint(this.ellipseSector.radius2, this.ellipseSector.phi + Math.PI / 2.0, out this.ellipseSector.secondPointRow, out this.ellipseSector.secondPointCol);
                    break;

                case PosSizableRect.CircleCenter:
                    if (e.Button == MouseButtons.Left)// 左键表示平移，右键表示放转
                    {
                        this.ellipseSector.col += e.X - oldX;
                        this.ellipseSector.row += e.Y - oldY;
                        this.ellipseSector.firstPointCol += e.X - oldX;
                        this.ellipseSector.firstPointRow += e.Y - oldY;
                        this.ellipseSector.thirdPointCol += e.X - oldX;
                        this.ellipseSector.thirdPointRow += e.Y - oldY;
                        this.ellipseSector.secondPointCol += e.X - oldX;
                        this.ellipseSector.secondPointRow += e.Y - oldY;
                        /////////////////////////////////
                        this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi, out this.ellipseSector.firstPointRow, out this.ellipseSector.firstPointCol);
                        this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi + Math.PI, out this.ellipseSector.thirdPointRow, out this.ellipseSector.thirdPointCol);
                        this.ellipseSector.AffinePoint(this.ellipseSector.radius2, this.ellipseSector.phi + Math.PI / 2.0, out this.ellipseSector.secondPointRow, out this.ellipseSector.secondPointCol);
                        //this.ellipseSector.AffinePoint(this.ellipseSector.radius2, this.ellipseSector.phi + Math.PI / 2.0 + Math.PI, out this.ellipseSector.downMiddlePointRow, out this.ellipseSector.downMiddlePointCol);
                    }
                    else
                    {
                        HTuple ATan;
                        HOperatorSet.LineOrientation(this.ellipseSector.row, this.ellipseSector.col, e.Y, e.X, out ATan);
                        this.ellipseSector.phi = ATan.D;
                        this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi, out this.ellipseSector.firstPointRow, out this.ellipseSector.firstPointCol);
                        this.ellipseSector.AffinePoint(this.ellipseSector.radius1, this.ellipseSector.phi + Math.PI, out this.ellipseSector.thirdPointRow, out this.ellipseSector.thirdPointCol);
                        this.ellipseSector.AffinePoint(this.ellipseSector.radius2, this.ellipseSector.phi + Math.PI / 2.0, out this.ellipseSector.secondPointRow, out this.ellipseSector.secondPointCol);
                        //this.ellipseSector.AffinePoint(this.ellipseSector.radius2, this.ellipseSector.phi + Math.PI / 2.0 + Math.PI, out this.ellipseSector.downMiddlePointRow, out this.ellipseSector.downMiddlePointCol);
                    }
                    break;
                case PosSizableRect.OutSideCircle:
                case PosSizableRect.InnerCircle:
                    this.ellipseSector.diffRadius += HMisc.DistancePp(e.Y, e.X, this.ellipseSector.row, this.ellipseSector.col) - HMisc.DistancePp(this.oldY, this.oldX, this.ellipseSector.row, this.ellipseSector.col);
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
            if (this.ellipseSector.radius1 != 0)
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

        private void showGraphic(HTuple row, HTuple col)
        {
            string PointOrder;
            double Row, Column, Phi, Radius1, Radius2, StartPhi, EndPhi;
            try
            {
                new HXLDCont(row, col).FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
                ////////////////
                this.AttachDrawingPropertyData.Clear();
                ///////////////// 将图中所有节点位置的矩形绘制出来 //////////////////////
                foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
                {
                    this.AttachDrawingPropertyData.Add(GetSizableRect(pos));
                }
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenEllipseContourXld(Row, Column, Phi, Radius1, Radius2, StartPhi, EndPhi, PointOrder, 0.05);
                this.AttachDrawingPropertyData.Add(hXLDCont); // 因为半径在绘图时等于0，所以箭头不会绘制出来
                base.DrawingGraphicObject();
            }
            catch
            {

            }
        }
        private void FitEllipseParam()
        {
            string PointOrder;
            double Row, Column, Phi, Radius1, Radius2, StartPhi, EndPhi;
            if (this.ellipseSector.fivePointRow == 0) return;
            HXLDCont xld = new HXLDCont(new HTuple(this.ellipseSector.firstPointRow, this.ellipseSector.secondPointRow, this.ellipseSector.thirdPointRow, this.ellipseSector.forPointRow, this.ellipseSector.fivePointRow),
                               new HTuple(this.ellipseSector.firstPointCol, this.ellipseSector.secondPointCol, this.ellipseSector.thirdPointCol, this.ellipseSector.forPointCol, this.ellipseSector.fivePointCol));
            xld.FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
            // 保证点的顺序是积极的
            if (PointOrder == "negative")
            {
                xld.Dispose();
                xld = new HXLDCont(new HTuple(this.ellipseSector.fivePointRow, this.ellipseSector.forPointRow, this.ellipseSector.thirdPointRow, this.ellipseSector.secondPointRow, this.ellipseSector.firstPointRow),
                                   new HTuple(this.ellipseSector.fivePointCol, this.ellipseSector.forPointCol, this.ellipseSector.thirdPointCol, this.ellipseSector.secondPointCol, this.ellipseSector.firstPointCol));
                xld.FitEllipseContourXld("fitzgibbon", -1, 0, 0, 200, 3, 2, out Row, out Column, out Phi, out Radius1, out Radius2, out StartPhi, out EndPhi, out PointOrder);
            }
            this.ellipseSector = new EllipseSector(Row, Column, Phi, Radius1, Radius2, StartPhi, EndPhi, this.ellipseSector.diffRadius);
            //this.ellipseSector.row = Row;
            //this.ellipseSector.col = Column;
            //this.ellipseSector.phi = Phi;
            //this.ellipseSector.radius1 = Radius1;
            //this.ellipseSector.radius2 = Radius2;
            //this.ellipseSector.start_phi = StartPhi;
            //this.ellipseSector.end_phi = EndPhi;
            //this.ellipseSector.pointOrder = PointOrder;
            /////////////
            DrawingGraphicObject();
            ////////////        
        }
        private HXLDCont CreateEllipseSectorObject(double diffRadius)
        {
            HXLDCont xld = new HXLDCont();
            if (this.ellipseSector.radius1 != 0 && this.ellipseSector.radius2 != 0)
                xld.GenEllipseContourXld(this.ellipseSector.row, this.ellipseSector.col, this.ellipseSector.phi, Math.Abs((this.ellipseSector.radius1 + diffRadius)), Math.Abs((this.ellipseSector.radius2 + diffRadius)), this.ellipseSector.start_phi, this.ellipseSector.end_phi, "positive", 0.1);
            return xld;
        }
        private HXLDCont GetSizableRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftBottom:
                    return CreateRectSizableNode(this.ellipseSector.firstPointCol, this.ellipseSector.firstPointRow);
                case PosSizableRect.UpMiddle:
                    return CreateRectSizableNode(this.ellipseSector.thirdPointCol, this.ellipseSector.thirdPointRow);
                case PosSizableRect.RightBottom:
                    return CreateRectSizableNode(this.ellipseSector.fivePointCol, this.ellipseSector.fivePointRow);
                case PosSizableRect.secondNode:
                    return CreateRectSizableNode(this.ellipseSector.secondPointCol, this.ellipseSector.secondPointRow);
                case PosSizableRect.forNode:
                    return CreateRectSizableNode(this.ellipseSector.forPointCol, this.ellipseSector.forPointRow);
                case PosSizableRect.CircleCenter:
                    return CreateEllipseSectorObject(0);
                case PosSizableRect.Arrow:
                    return GetArrowXLD();
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double dist = Math.Sqrt((y - this.ellipseSector.row) * (y - this.ellipseSector.row) + (x - this.ellipseSector.col) * (x - this.ellipseSector.col));
            double dist1 = Math.Sqrt((y - this.ellipseSector.firstPointRow) * (y - this.ellipseSector.firstPointRow) + (x - this.ellipseSector.firstPointCol) * (x - this.ellipseSector.firstPointCol));
            double dist2 = Math.Sqrt((y - this.ellipseSector.secondPointRow) * (y - this.ellipseSector.secondPointRow) + (x - this.ellipseSector.secondPointCol) * (x - this.ellipseSector.secondPointCol));
            double dist3 = Math.Sqrt((y - this.ellipseSector.thirdPointRow) * (y - this.ellipseSector.thirdPointRow) + (x - this.ellipseSector.thirdPointCol) * (x - this.ellipseSector.thirdPointCol));
            HXLDCont innerXLD = CreateEllipseSectorObject(this.ellipseSector.diffRadius * -1);
            HXLDCont outXLD = CreateEllipseSectorObject(this.ellipseSector.diffRadius);
            double min1Dist = 0, min2Dist = 0, max1Dist = 0, max2Dist = 0;
            if (innerXLD.Key.ToInt64() > 0)
                innerXLD.DistancePc(y, x, out min1Dist, out max1Dist);
            if (outXLD.Key.ToInt64() > 0)
                outXLD.DistancePc(y, x, out min2Dist, out max2Dist);
            //////////////////////////////////
            if (dist < (this.ellipseSector.radius1 - Math.Abs(this.ellipseSector.diffRadius)) * 0.8) // 表示移动
                selectNode = PosSizableRect.CircleCenter;
            //////////////////////////////////////////////////第二点
            if (min1Dist < Math.Abs(0.1 * this.ellipseSector.radius1))
                selectNode = PosSizableRect.InnerCircle;
            //////////////////////////////////////////////////第三点
            if (min2Dist < Math.Abs(0.1 * this.ellipseSector.radius1))
                selectNode = PosSizableRect.OutSideCircle;
            //////////////////////////////////////////////////
            if (dist1 <= this.nodeSizeRect)  //Math.Abs(0.1 * this.ellipseSector.radius2)
                selectNode = PosSizableRect.LeftBottom;
            //////////////////////////////////////////////////
            if (dist2 <= this.nodeSizeRect)  //Math.Abs(0.1 * this.ellipseSector.radius1)
                selectNode = PosSizableRect.UpMiddle;
            //////////////////////////////////////////////////
            if (dist3 <= this.nodeSizeRect)  //Math.Abs(0.1 * this.ellipseSector.radius2)
                selectNode = PosSizableRect.RightBottom;
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

                case PosSizableRect.CircleCenter:
                    return Cursors.Hand;

                case PosSizableRect.LeftBottom:
                case PosSizableRect.RightBottom:
                case PosSizableRect.Arrow:
                case PosSizableRect.UpMiddle:
                    return Cursors.Cross;
                default:
                    return Cursors.Default;
            }
        }
        private struct EllipseSector
        {
            public double firstPointRow;
            public double firstPointCol;
            public double secondPointRow;
            public double secondPointCol;
            public double thirdPointRow;
            public double thirdPointCol;
            public double forPointRow;
            public double forPointCol;
            public double fivePointRow;
            public double fivePointCol;
            public double row;
            public double col;
            public double phi;
            public double radius1;
            public double radius2;
            public double start_phi;
            public double end_phi;
            public double diffRadius;
            public double[] normalPhi;
            public EllipseSector(double row, double col, double phi, double radius1, double radius2, double start_phi, double end_phi, double diffRadius)
            {
                this.row = row;
                this.col = col;
                this.phi = phi;
                this.radius1 = radius1;
                this.radius2 = radius2;
                this.firstPointRow = 0;
                this.firstPointCol = 0;
                this.secondPointRow = 0;
                this.secondPointCol = 0;
                this.thirdPointRow = 0;
                this.thirdPointCol = 0;
                this.forPointRow = 0;
                this.forPointCol = 0;
                this.fivePointRow = 0;
                this.fivePointCol = 0;
                this.start_phi = start_phi;
                this.end_phi = end_phi;
                this.diffRadius = diffRadius;
                this.normalPhi = null;
                /////////////////////////////////////////////
                double step;
                if (start_phi <= end_phi)
                    step = Math.Abs(end_phi - start_phi) / 4;
                else
                    step = (Math.PI * 2 - Math.Abs(end_phi - start_phi)) / 4; // 要保证等分的是需要的弧段
                //////////////////////////////////////////////
                //GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.start_phi, out this.firstPointRow, out this.firstPointCol);
                //GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.start_phi + step, out this.secondPointRow, out this.secondPointCol);
                //GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.start_phi + step*2, out this.thirdPointRow, out this.thirdPointCol);
                //GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.start_phi + step*3, out this.forPointRow, out this.forPointCol);
                //GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.end_phi, out this.fivePointRow, out this.fivePointCol);

                //////////////////////////////////////////////
                GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.start_phi, out this.firstPointRow, out this.firstPointCol);
                GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.start_phi + step * 2, out this.secondPointRow, out this.secondPointCol);
                GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.end_phi, out this.thirdPointRow, out this.thirdPointCol);
                //GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.start_phi + step * 3, out this.forPointRow, out this.forPointCol);
                //GetPointEllipse(this.row, this.col, this.phi, this.radius1, this.radius2, this.end_phi, out this.fivePointRow, out this.fivePointCol);

            }
            public void GetPointEllipse(double Row_Center, double Column_Center, double Phi, double Radius1, double Radius2, double rad, out double Row, out double Col)
            {
                HTuple rowPoint, colPoint;
                HOperatorSet.GetPointsEllipse(rad, Row_Center, Column_Center, Phi, Radius1, Radius2, out rowPoint, out colPoint);
                if (rowPoint != null && rowPoint.Length > 0)
                {
                    Row = rowPoint.D;
                    Col = colPoint.D;
                }
                else
                {
                    Row = Row_Center + Radius1;
                    Col = Column_Center + Radius1;
                }
            }

            public void AffinePoint(double row, double col, double phi, out double outRow, out double outCol)
            {
                HTuple homMat2dIdentity, homMat2dRotate, Qx, Qy;
                HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
                HOperatorSet.HomMat2dRotate(homMat2dIdentity, phi, this.row, this.col, out homMat2dRotate);
                HOperatorSet.AffineTransPoint2d(homMat2dRotate, row, col, out Qx, out Qy);
                outRow = Qx.D;
                outCol = Qy.D;
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


        }



        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            if (this.ellipseSector.radius1 == 0)
                this.ellipseSector = new EllipseSector(row1 + 100, column1 + 100, 0, 50, 40, 0, 3.14, this.nodeSizeRect);
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

        private void GetArrowPoint(EllipseSector pixEllipse, int Num, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            Row1 = new double[Num];
            Col1 = new double[Num];
            Row2 = new double[Num];
            Col2 = new double[Num];
            double step;
            if (pixEllipse.start_phi <= pixEllipse.end_phi)
                step = Math.Abs(pixEllipse.end_phi - pixEllipse.start_phi) / (Num - 1);
            else
                step = (Math.PI * 2 - Math.Abs(pixEllipse.end_phi - pixEllipse.start_phi)) / (Num - 1); // 要保证等分的是需要的弧段
            for (int i = 0; i < Num; i++)
            {
                HTuple rowPoint1, colPoint1, rowPoint2, colPoint2;
                HOperatorSet.GetPointsEllipse(step * i, pixEllipse.row, pixEllipse.col, pixEllipse.phi, Math.Abs(pixEllipse.radius1 - pixEllipse.diffRadius), Math.Abs(pixEllipse.radius2 - pixEllipse.diffRadius), out rowPoint1, out colPoint1);
                HOperatorSet.GetPointsEllipse(step * i, pixEllipse.row, pixEllipse.col, pixEllipse.phi, Math.Abs(pixEllipse.radius1 + pixEllipse.diffRadius), Math.Abs(pixEllipse.radius2 + pixEllipse.diffRadius), out rowPoint2, out colPoint2);
                if (rowPoint1.Length > 0)
                {
                    Row1[i] = rowPoint1[0].D;
                    Col1[i] = colPoint1[0].D;
                }
                else
                {
                    Row1[i] = pixEllipse.row + pixEllipse.radius1;
                    Col1[i] = pixEllipse.col + pixEllipse.radius1;
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
            ///////////////
            pixEllipse.normalPhi = new double[Row2.Length];
            for (int i = 0; i < Row2.Length; i++)
            {
                pixEllipse.normalPhi[i] = Math.Atan2((Row2[i] - Row1[i]) * -1, Col2[i] - Col1[i]);
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
                    GetArrowPoint(this.ellipseSector, 5, out row1, out col1, out row2, out col2);
                    hXLD = GenArrowContourXld(row1, col1, row2, col2, this.nodeSizeRect, this.nodeSizeRect);
                    break;
                case enShowMode.矩形:
                    GetArrowPoint(this.ellipseSector, 5, out row1, out col1, out row2, out col2);
                    for (int i = 0; i < row1.Length; i++)
                    {
                        HXLDCont rect2 = new HXLDCont();
                        rect2.GenRectangle2ContourXld((row1[i] + row2[i]) * 0.5, (col1[i] + col2[i]) * 0.5, this.ellipseSector.normalPhi[i], this.ellipseSector.diffRadius, 5);
                        hXLD = hXLD.ConcatObj(rect2);
                    }
                    break;
            }
            return hXLD;

        }


        private void UpdataMeasureRegion()
        {
            /// 设置计量模型
            //if (this.backImage == null && this.findEllipseSector == null) return;
            //userPixEllipseSector EllipseSectorPixPosition = new userPixEllipseSector(this.ellipseSector.row, this.ellipseSector.col, this.ellipseSector.phi, this.ellipseSector.radius1,
            //this.ellipseSector.radius2, this.ellipseSector.start_phi, this.ellipseSector.end_phi, this.backImage.CamParam, this.backImage.CamPose);
            //EllipseSectorPixPosition.diffRadius = this.ellipseSector.diffRadius;
            //EllipseSectorPixPosition.normalPhi = this.ellipseSector.normalPhi;
            //this.findEllipseSector.EllipseSectorWcsPosition = EllipseSectorPixPosition.GetWcsEllipseSector(this.backImage.X_pos, this.backImage.Y_pos).Affine2DWcsEllipseSector(this.findEllipseSector.WcsCoordSystem.GetInvertVariationHomMat2D(), enAffineTransOrientation.反向变换); // 根据当前位置来修改参考位置
            ////                                                                                                                                                                                                                                                                         /////////////////

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
                        //this.wcsEllipseSector.Execute(null);
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

        public override userPixEllipseSector GetPixEllipseSectorParam()
        {
            userPixEllipseSector pixEllipseSector = new userPixEllipseSector(this.ellipseSector.row, this.ellipseSector.col, this.ellipseSector.phi, this.ellipseSector.radius1, this.ellipseSector.radius2, this.ellipseSector.start_phi, this.ellipseSector.end_phi, this.backImage?.CamParams);
            pixEllipseSector.DiffRadius = this.ellipseSector.diffRadius;
            pixEllipseSector.NormalPhi = this.ellipseSector.normalPhi;
            pixEllipseSector = pixEllipseSector.AffineTransPixEllipseSector(this.pixCoordSystem.GetInvertVariationHomMat2D());
            return pixEllipseSector;
        }
        public override userWcsEllipseSector GetWcsEllipseSectorParam()
        {
            userWcsEllipseSector wcsEllipse = GetPixEllipseSectorParam().GetWcsEllipseSector(this.backImage.Grab_X, this.backImage.Grab_Y).Affine2DWcsEllipseSector(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            return wcsEllipse;
        }

    }



}
