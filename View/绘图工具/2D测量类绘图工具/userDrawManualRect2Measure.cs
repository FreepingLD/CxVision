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
    public class userDrawManualRect2Measure : DrawingBaseMeasure
    {
        private Rect2 rect2;
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
        //private userWcsRectangle2 wcsRect2;
        private userWcsCoordSystem wcsCoordSystem;
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
            Rect2Inside,
            Rect2Over,
            Arrow,
            None,
            All,
            Center,
        }

        public userDrawManualRect2Measure(HWindowControl hWindowControl) : base(hWindowControl)
        {

        }
        public userDrawManualRect2Measure(HWindowControl hWindowControl, userWcsRectangle2 wcsRect2, userWcsCoordSystem wcsCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsRect2 != null && wcsRect2.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //this.wcsRect2 = wcsRect2;
                this.wcsCoordSystem = wcsCoordSystem;
                userPixRectangle2 pixRect2 = wcsRect2.AffineWcsRectangle2D(wcsCoordSystem.GetVariationHomMat2D()).GetPixRectangle2();
                this.rect2 = new Rect2(pixRect2.Row, pixRect2.Col, pixRect2.Rad, pixRect2.Length1, pixRect2.Length2, pixRect2.DiffRadius);
                //this.wcsRect2 = wcsRect2;
            }
            else
            {
                //this.rect2 = new Rect2();
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                this.rect2.diffRadius = this.rect2.length2 * 0.5;
            }
        }
        public userDrawManualRect2Measure(HWindowControl hWindowControl, userPixRectangle2 pixRect2, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixRect2 != null && pixRect2.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //this.wcsRect2 = pixRect2;
                this.pixCoordSystem = pixCoordSystem;
                userPixRectangle2 pixRect = pixRect2.AffineTransPixRect2(pixCoordSystem?.GetVariationHomMat2D());
                this.rect2 = new Rect2(pixRect.Row, pixRect.Col, pixRect.Rad, pixRect.Length1, pixRect.Length2, pixRect.DiffRadius);
                //this.wcsRect2 = pixRect2;
            }
            else
            {
                //this.rect2 = new Rect2();
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                this.rect2.diffRadius = this.rect2.length2 * 0.5;
            }
        }
        public userDrawManualRect2Measure(HWindowControl hWindowControl, userWcsRectangle2 wcsRect2) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (wcsRect2 != null && wcsRect2.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                userPixRectangle2 pixRect2 = wcsRect2.GetPixRectangle2();
                this.rect2 = new Rect2(pixRect2.Row, pixRect2.Col, pixRect2.Rad, pixRect2.Length1, pixRect2.Length2, pixRect2.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                this.rect2.diffRadius = this.rect2.length2 * 0.5;
            }
        }
        public userDrawManualRect2Measure(HWindowControl hWindowControl, userPixRectangle2 pixRect2) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            if (pixRect2 != null && pixRect2.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                //userPixRectangle2 pixRect2 = pixRect2.GetPixRectangle2();
                this.rect2 = new Rect2(pixRect2.Row, pixRect2.Col, pixRect2.Rad, pixRect2.Length1, pixRect2.Length2, pixRect2.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                this.rect2.diffRadius = this.rect2.length2 * 0.5;
            }
        }

        public override void SetParam(object param)
        {
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixRectangle2):
                        if (!(param is userPixRectangle2)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixRectangle2 类型");
                        userPixRectangle2 pixRect2 = (userPixRectangle2)param;
                        //userPixRectangle2 pixRect = pixRect2;//.GetPixRectangle2();
                        //this.rect2 = new Rect2(pixRect2.Row, pixRect2.Col, pixRect2.Rad, pixRect2.Length1, pixRect2.Length2, pixRect2.DiffRadius);
                        if (pixRect2.CamParams != null)
                        {
                            userPixRectangle2 pixRect = pixRect2;//.GetPixRectangle2();
                            this.rect2 = new Rect2(pixRect.Row, pixRect.Col, pixRect.Rad, pixRect.Length1, pixRect.Length2, pixRect.DiffRadius);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                            this.rect2.diffRadius = this.rect2.length2 * 0.5;
                        }
                        break;
                    case nameof(userWcsRectangle2):
                        if (!(param is userWcsRectangle2)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsRectangle2 类型");
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)param;
                        if (wcsRect2.CamParams != null)
                        {
                            userPixRectangle2 pixRect = wcsRect2.GetPixRectangle2();
                            this.rect2 = new Rect2(pixRect.Row, pixRect.Col, pixRect.Rad, pixRect.Length1, pixRect.Length2, pixRect.DiffRadius);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                            this.rect2.diffRadius = this.rect2.length2 * 0.5;
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
                this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                this.rect2.diffRadius = this.rect2.length2 * 0.5;
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
                //    base.hWindowControl_HMouseDown(sender, e);
                //    break;
                case MouseButtons.Middle:
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
                    //case MouseButtons.Middle:
                    //if (this.wcsRect2 != null)
                    //    this.wcsRect2.Execute(null);
                    //break;
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
            if (!this.isDrwingObject) return;
            ChangeCursor(e.X, e.Y); // 在移动过程中改变光标
            /////////////////////////
            if (mIsClick == false || IsTranslate) // 只有在鼠标按下的状态下才执行移动
            {
                return;
            }
            ////////////////////
            //this.rect2.diffRadius = 10;
            switch (selectedNode)
            {
                case PosSizableRect.LeftUpCorner:
                case PosSizableRect.LeftDownCorner:
                case PosSizableRect.RightUpCorner:
                case PosSizableRect.RightDownCorner:
                case PosSizableRect.All:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.rect2.length2 += HMisc.DistancePp(e.Y, e.X, this.rect2.row, this.rect2.col) - HMisc.DistancePp(this.oldY, this.oldX, this.rect2.row, this.rect2.col);
                        this.rect2.length1 += HMisc.DistancePp(e.Y, e.X, this.rect2.row, this.rect2.col) - HMisc.DistancePp(this.oldY, this.oldX, this.rect2.row, this.rect2.col);
                        if (this.rect2.length1 < 5)
                            this.rect2.length1 = 5;
                        if (this.rect2.length2 < 5)
                            this.rect2.length2 = 5;
                        /////////////////////////////////
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftUpCornerPointRow, out this.rect2.leftUpCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftDownCornerPointRow, out this.rect2.leftDownCornerPointCol);
                        ///////////
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightDownCornerPointRow, out this.rect2.rightDownCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightUpCornerPointRow, out this.rect2.rightUpCornerPointCol);
                    }
                    else
                    {
                        //HTuple ATan;
                        //HOperatorSet.LineOrientation(this.rect2.row, this.rect2.col, e.Y, e.X, out ATan);
                        //this.rect2.phi = ATan.D;
                        this.rect2.phi += Math.Atan2(oldY - this.rect2.row, oldX - this.rect2.col) - Math.Atan2(e.Y - this.rect2.row, e.X - this.rect2.col);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftUpCornerPointRow, out this.rect2.leftUpCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftDownCornerPointRow, out this.rect2.leftDownCornerPointCol);
                        ///////////
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightDownCornerPointRow, out this.rect2.rightDownCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightUpCornerPointRow, out this.rect2.rightUpCornerPointCol);
                    }
                    break;

                case PosSizableRect.UpMidlleCorner:
                case PosSizableRect.DownMidlleCorner:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.rect2.length2 += HMisc.DistancePp(e.Y, e.X, this.rect2.row, this.rect2.col) - HMisc.DistancePp(this.oldY, this.oldX, this.rect2.row, this.rect2.col);
                        if (this.rect2.length1 < 5)
                            this.rect2.length1 = 5;
                        if (this.rect2.length2 < 5)
                            this.rect2.length2 = 5;
                        /////////////////////////////////
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftUpCornerPointRow, out this.rect2.leftUpCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftDownCornerPointRow, out this.rect2.leftDownCornerPointCol);
                        ///////////
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightDownCornerPointRow, out this.rect2.rightDownCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightUpCornerPointRow, out this.rect2.rightUpCornerPointCol);
                    }
                    else
                    {
                        //HTuple ATan;
                        //HOperatorSet.LineOrientation(this.rect2.row, this.rect2.col, e.Y, e.X, out ATan);
                        //this.rect2.phi = ATan.D;
                        this.rect2.phi += Math.Atan2(oldY - this.rect2.row, oldX - this.rect2.col) - Math.Atan2(e.Y - this.rect2.row, e.X - this.rect2.col);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftUpCornerPointRow, out this.rect2.leftUpCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftDownCornerPointRow, out this.rect2.leftDownCornerPointCol);
                        ///////////
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightDownCornerPointRow, out this.rect2.rightDownCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightUpCornerPointRow, out this.rect2.rightUpCornerPointCol);
                    }
                    break;

                case PosSizableRect.LeftMidlleCorner:
                case PosSizableRect.RightMidlleCorner:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.rect2.length1 += HMisc.DistancePp(e.Y, e.X, this.rect2.row, this.rect2.col) - HMisc.DistancePp(this.oldY, this.oldX, this.rect2.row, this.rect2.col);
                        if (this.rect2.length1 < 5)
                            this.rect2.length1 = 5;
                        if (this.rect2.length2 < 5)
                            this.rect2.length2 = 5;
                        /////////////////////////////////
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftUpCornerPointRow, out this.rect2.leftUpCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftDownCornerPointRow, out this.rect2.leftDownCornerPointCol);
                        ///////////
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightDownCornerPointRow, out this.rect2.rightDownCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightUpCornerPointRow, out this.rect2.rightUpCornerPointCol);
                    }
                    else
                    {
                        //HTuple ATan;
                        //HOperatorSet.LineOrientation(this.rect2.row, this.rect2.col, e.Y, e.X, out ATan);
                        //this.rect2.phi = ATan.D;
                        this.rect2.phi += Math.Atan2(oldY - this.rect2.row, oldX - this.rect2.col) - Math.Atan2(e.Y - this.rect2.row, e.X - this.rect2.col);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftUpCornerPointRow, out this.rect2.leftUpCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftDownCornerPointRow, out this.rect2.leftDownCornerPointCol);
                        ///////////
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightDownCornerPointRow, out this.rect2.rightDownCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightUpCornerPointRow, out this.rect2.rightUpCornerPointCol);
                    }
                    break;

                case PosSizableRect.Rect2Inside:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.rect2.col += e.X - oldX;
                        this.rect2.row += e.Y - oldY;
                        /////////////////////////////////
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftUpCornerPointRow, out this.rect2.leftUpCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col - this.rect2.length1, this.rect2.phi, out this.rect2.leftDownCornerPointRow, out this.rect2.leftDownCornerPointCol);
                        ///////////
                        this.rect2.AffinePoint(this.rect2.row + this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightDownCornerPointRow, out this.rect2.rightDownCornerPointCol);
                        this.rect2.AffinePoint(this.rect2.row - this.rect2.length2, this.rect2.col + this.rect2.length1, this.rect2.phi, out this.rect2.rightUpCornerPointRow, out this.rect2.rightUpCornerPointCol);
                    }
                    break;

            }
            //上一点需要实时更新，这样一来图像才不会乱
            oldX = e.X;
            oldY = e.Y;
            //重绘图形
            if (mIsClick)
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
            ///////////////// 将图中所有节点位置的矩形绘制出来 //////////////////////
            foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (this.isDispalyAttachDrawingProperty)
                    this.AttachDrawingPropertyData.Add(GetSizableRect(pos));
            }
            base.DrawingGraphicObject();
        }

        private HXLDCont CreateRect2Object(double nodeSizeRect = 0)
        {
            HXLDCont xld = new HXLDCont();
            if (this.rect2.length1 != 0 && this.rect2.length2 != 0)
                xld.GenRectangle2ContourXld(this.rect2.row, this.rect2.col, this.rect2.phi, this.rect2.length1 - nodeSizeRect, this.rect2.length2 - nodeSizeRect);
            return xld;
        }
        private HXLDCont GetSizableRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.RightUpCorner:
                    return CreateRectSizableNode(this.rect2.rightUpCornerPointCol, this.rect2.rightUpCornerPointRow);

                case PosSizableRect.RightDownCorner:
                    return CreateRectSizableNode(this.rect2.rightDownCornerPointCol, this.rect2.rightDownCornerPointRow);

                case PosSizableRect.LeftDownCorner:
                    return CreateRectSizableNode(this.rect2.leftDownCornerPointCol, this.rect2.leftDownCornerPointRow);

                case PosSizableRect.LeftUpCorner:
                    return CreateRectSizableNode(this.rect2.leftUpCornerPointCol, this.rect2.leftUpCornerPointRow);

                case PosSizableRect.UpMidlleCorner:
                    return CreateRectSizableNode((this.rect2.leftUpCornerPointCol + this.rect2.rightUpCornerPointCol) * 0.5, (this.rect2.leftUpCornerPointRow + this.rect2.rightUpCornerPointRow) * 0.5);

                case PosSizableRect.DownMidlleCorner:
                    return CreateRectSizableNode((this.rect2.leftDownCornerPointCol + this.rect2.rightDownCornerPointCol) * 0.5, (this.rect2.leftDownCornerPointRow + this.rect2.rightDownCornerPointRow) * 0.5);

                case PosSizableRect.LeftMidlleCorner:
                    return CreateRectSizableNode((this.rect2.leftUpCornerPointCol + this.rect2.leftDownCornerPointCol) * 0.5, (this.rect2.leftUpCornerPointRow + this.rect2.leftDownCornerPointRow) * 0.5);

                case PosSizableRect.RightMidlleCorner:
                    return CreateRectSizableNode((this.rect2.rightUpCornerPointCol + this.rect2.rightDownCornerPointCol) * 0.5, (this.rect2.rightUpCornerPointRow + this.rect2.rightDownCornerPointRow) * 0.5);

                case PosSizableRect.Rect2Inside:
                    return CreateRect2Object();

                case PosSizableRect.Center:
                    double Row2 = this.rect2.row - this.nodeSizeRect * 3 * Math.Sin(this.rect2.phi); // 因为像素坐标系与世界坐标系Y轴相反，所以这里Y也要用相反的操作符
                    double Col2 = this.rect2.col + this.nodeSizeRect * 3 * Math.Cos(this.rect2.phi);
                    return this.GenArrowContourXld(this.rect2.row, this.rect2.col, Row2, Col2, this.nodeSizeRect, this.nodeSizeRect);
                    //break;
                //case PosSizableRect.Arrow:
                //    return GetArrowXLD();

                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect selectNode = PosSizableRect.None;
            double minDist = 0, maxDist = 0;
            HXLDCont rect = CreateRect2Object();
            if (rect.Key.ToInt64() > 0)
                rect.DistancePc(y, x, out minDist, out maxDist);
            else
                minDist = 1000;
            double dist = Math.Sqrt((y - this.rect2.row) * (y - this.rect2.row) + (x - this.rect2.col) * (x - this.rect2.col));
            double dist1 = Math.Sqrt((y - this.rect2.leftUpCornerPointRow) * (y - this.rect2.leftUpCornerPointRow) + (x - this.rect2.leftUpCornerPointCol) * (x - this.rect2.leftUpCornerPointCol));
            double dist2 = Math.Sqrt((y - this.rect2.rightUpCornerPointRow) * (y - this.rect2.rightUpCornerPointRow) + (x - this.rect2.rightUpCornerPointCol) * (x - this.rect2.rightUpCornerPointCol));
            double dist3 = Math.Sqrt((y - this.rect2.rightDownCornerPointRow) * (y - this.rect2.rightDownCornerPointRow) + (x - this.rect2.rightDownCornerPointCol) * (x - this.rect2.rightDownCornerPointCol));
            double dist4 = Math.Sqrt((y - this.rect2.leftDownCornerPointRow) * (y - this.rect2.leftDownCornerPointRow) + (x - this.rect2.leftDownCornerPointCol) * (x - this.rect2.leftDownCornerPointCol));
            //////////////////////////////////
            double dist5 = HMisc.DistancePl(y, x, this.rect2.leftUpCornerPointRow, this.rect2.leftUpCornerPointCol, this.rect2.rightUpCornerPointRow, this.rect2.rightUpCornerPointCol);
            double dist6 = HMisc.DistancePl(y, x, this.rect2.rightUpCornerPointRow, this.rect2.rightUpCornerPointCol, this.rect2.rightDownCornerPointRow, this.rect2.rightDownCornerPointCol);
            double dist7 = HMisc.DistancePl(y, x, this.rect2.rightDownCornerPointRow, this.rect2.rightDownCornerPointCol, this.rect2.leftDownCornerPointRow, this.rect2.leftDownCornerPointCol);
            double dist8 = HMisc.DistancePl(y, x, this.rect2.leftDownCornerPointRow, this.rect2.leftDownCornerPointCol, this.rect2.leftUpCornerPointRow, this.rect2.leftUpCornerPointCol);
            /////////////////////////////////////////////////////////
            //if (minDist < 10) // 表示移动
            //    selectNode = PosSizableRect.Rect2Over;
            if (CreateRect2Object(Math.Abs(this.rect2.diffRadius)).TestXldPoint(y, x) > 0)  // 表示移动
                selectNode = PosSizableRect.Rect2Inside;

            if (minDist < this.nodeSizeRect && dist5 < this.nodeSizeRect) // 表示移动
                selectNode = PosSizableRect.UpMidlleCorner;
            if (minDist < this.nodeSizeRect && dist6 < this.nodeSizeRect) // 表示移动
                selectNode = PosSizableRect.RightMidlleCorner;
            /////////////////////////////
            if (minDist < this.nodeSizeRect && dist7 < this.nodeSizeRect) // 表示移动
                selectNode = PosSizableRect.DownMidlleCorner;
            ///////////////////////////////
            if (minDist < this.nodeSizeRect && dist8 < this.nodeSizeRect) // 表示移动
                selectNode = PosSizableRect.LeftMidlleCorner;
            //////////////////////////////////////////////////第二点
            if (dist < Math.Abs((Math.Min(this.rect2.length1, this.rect2.length2) - Math.Abs(this.rect2.diffRadius)) * 0.7))
                selectNode = PosSizableRect.Rect2Inside;
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
            //////////////////////////////////////////////////第三点
            if (Math.Abs(this.rect2.diffRadius) * 0.9 < minDist && minDist < Math.Abs(this.rect2.diffRadius) * 1.1)
                selectNode = PosSizableRect.Arrow;
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
                    return Cursors.Default;

                case PosSizableRect.RightUpCorner:
                    return Cursors.Default;

                case PosSizableRect.LeftDownCorner: //SizeNESW
                    return Cursors.Default;

                case PosSizableRect.LeftUpCorner:
                    return Cursors.Default;

                case PosSizableRect.Rect2Inside:
                    return Cursors.Hand;

                default:
                    return Cursors.Default;
            }
        }

        private struct Rect2
        {
            public double leftUpCornerPointRow;
            public double leftUpCornerPointCol;
            public double leftDownCornerPointRow;
            public double leftDownCornerPointCol;
            public double rightUpCornerPointRow;
            public double rightUpCornerPointCol;
            public double rightDownCornerPointRow;
            public double rightDownCornerPointCol;
            public double row;
            public double col;
            public double phi;
            public double length1;
            public double length2;
            public double diffRadius;
            public double[] normalPhi;

            public Rect2(double row, double col, double phi, double length1, double length2, double diffRadius)
            {
                this.row = row;
                this.col = col;
                this.phi = phi;
                this.length1 = length1;
                this.length2 = length2;
                this.leftUpCornerPointRow = row - length2;
                this.leftUpCornerPointCol = col - length1;
                this.leftDownCornerPointRow = row + length2;
                this.leftDownCornerPointCol = col - length1;
                this.rightDownCornerPointRow = row + length2;
                this.rightDownCornerPointCol = col + length1;
                this.rightUpCornerPointRow = row - length2;
                this.rightUpCornerPointCol = col + length1;
                this.diffRadius = diffRadius;
                this.normalPhi = null;
                /////////////////////////////////////////////
                AffinePoint(this.leftUpCornerPointRow, this.leftUpCornerPointCol, phi, out this.leftUpCornerPointRow, out this.leftUpCornerPointCol);
                AffinePoint(this.leftDownCornerPointRow, this.leftDownCornerPointCol, phi, out this.leftDownCornerPointRow, out this.leftDownCornerPointCol);
                ///////////
                AffinePoint(this.rightUpCornerPointRow, this.rightUpCornerPointCol, phi, out this.rightUpCornerPointRow, out this.rightUpCornerPointCol);
                AffinePoint(this.rightDownCornerPointRow, this.rightDownCornerPointCol, phi, out this.rightDownCornerPointRow, out this.rightDownCornerPointCol);
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
            public HXLDCont GetHXLD(double nodeSize = 0)
            {
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenRectangle2ContourXld(this.row, this.col, this.phi, this.length1 - nodeSize, this.length2 - nodeSize);
                return hXLDCont;
            }
        }


        public override void AttachDrawingObjectToWindow()
        {
            int row1, column1, row2, column2;
            this.hWindowControl.HalconWindow.GetPart(out row1, out column1, out row2, out column2);
            if (this.rect2.length1 == 0 && this.rect2.length2 == 0)
                this.rect2 = new Rect2(row1 + 200, column1 + 200, 0, 100, 100, 20);
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


        private void UpdataMeasureRegion()
        {
            /// 设置计量模型
            //if (this.backImage == null && this.wcsRect2 == null) return;
            //userPixRectangle2 Rect2PixPosition = new userPixRectangle2(this.rect2.row, this.rect2.col, this.rect2.phi, this.rect2.length1, this.rect2.length2, this.backImage.CamParams);
            //Rect2PixPosition.diffRadius = this.rect2.diffRadius;
            //Rect2PixPosition.normalPhi = this.rect2.normalPhi;
            //this.wcsRect2 = Rect2PixPosition.GetWcsRectangle2(this.backImage.Grab_X, this.backImage.Grab_Y).Affine2DWcsRectangle2(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
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
                        //this.wcsRect2.Execute(null);
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

        public override userPixRectangle2 GetPixRectangle2Param()
        {
            userPixRectangle2 PixRectangle2 = new userPixRectangle2(this.rect2.row, this.rect2.col, this.rect2.phi, this.rect2.length1, this.rect2.length2, this.backImage?.CamParams);
            PixRectangle2.DiffRadius = this.rect2.diffRadius;
            PixRectangle2.NormalPhi = this.rect2.normalPhi;
            PixRectangle2.CamParams = this.backImage.CamParams;
            PixRectangle2 = PixRectangle2.AffineTransPixRect2(this.pixCoordSystem.GetInvertVariationHomMat2D());
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
