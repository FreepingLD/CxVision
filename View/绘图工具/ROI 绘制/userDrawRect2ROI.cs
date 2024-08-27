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
    public class userDrawRect2ROI : VisualizeView
    {
        private Rect2 rect2;
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
            Rect2Inside,
            Rect2Over,
            Arrow,
            None,
            All,
        }

        public userDrawRect2ROI(HWindowControl hWindowControl, bool appendContextMenuStrip) : base(hWindowControl, appendContextMenuStrip)
        {
            this.CurrentButton = MouseButtons.None;
        }

        public userDrawRect2ROI(HWindowControl hWindowControl, userPixRectangle2 pixRect2, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
        {
            this.CurrentButton = MouseButtons.None;
            this.pixCoordSystem = new userPixCoordSystem();
            if (pixRect2 != null && pixRect2.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.pixCoordSystem = pixCoordSystem;
                userPixRectangle2 pixRect = pixRect2.AffineTransPixRect2(pixCoordSystem?.GetVariationHomMat2D());
                this.rect2 = new Rect2(pixRect.Row, pixRect.Col, pixRect.Rad, pixRect.Length1, pixRect.Length2, pixRect.DiffRadius);
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                this.rect2.diffRadius = this.rect2.length2 * 0.5;
            }
        }

        public userDrawRect2ROI(HWindowControl hWindowControl, userPixRectangle2 pixRect2) : base(hWindowControl)
        {
            this.CurrentButton = MouseButtons.None;
            if (pixRect2 != null && pixRect2.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
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
                        if (pixRect2.Length1 != 0 && pixRect2.Length2 != 0)
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
                        this.isDrawMode = true;
                        break;
                    case nameof(userWcsRectangle2):
                        if (!(param is userWcsRectangle2)) throw new ArgumentException("给定的数据类型异常，需要传入 userWcsRectangle2 类型");
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)param;
                        if (wcsRect2.CamParams != null)
                        {
                            userPixRectangle2 pixRect = wcsRect2.GetPixRectangle2();//.GetPixRectangle2();
                            this.rect2 = new Rect2(pixRect.Row, pixRect.Col, pixRect.Rad, pixRect.Length1, pixRect.Length2, pixRect.DiffRadius);
                        }
                        else
                        {
                            int row1, col1, row2, col2;
                            this.GetImagePart(out row1, out col1, out row2, out col2);
                            this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                            this.rect2.diffRadius = this.rect2.length2 * 0.5;
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
                this.rect2 = new Rect2(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, 0, (row2 - row1) * 0.08, (row2 - row1) * 0.04, this.nodeSizeRect);
                this.rect2.diffRadius = this.rect2.length2 * 0.5;
                this.pixCoordSystem = new userPixCoordSystem();
                this.isDrawMode = true;
            }
        }
        public override void DrawPixRect2OnWindow(enColor color, out userPixRectangle2 pixRec2)
        {            ////// 等待结束 //////////
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.isDrawMode = true;
            this.DrawingGraphicObject();
            while (true)
            {
                Application.DoEvents();
                if (MouseButtons.Right == this.CurrentButton || MouseButtons.None == this.CurrentButton) break;
                Thread.Sleep(20);
            }
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            pixRec2 = this.GetPixRectangle2Param();
            this.DetachDrawingObjectFromWindow();
        }
        public override void DrawPixRect2OnWindow(enColor color, out drawPixRect2 pixRec2)
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
            pixRec2 = this.GetDrawPixRect2Param();
            this.DetachDrawingObjectFromWindow();
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
                if (stopwatch.ElapsedMilliseconds > 60 * 2 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixRect2Param();
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
                if (stopwatch.ElapsedMilliseconds > 60 * 2 * 1000) break; //超过一分钟
            }
            stopwatch.Stop();
            this.isDrawMode = false;
            this.isTranslate = true;
            this.isDrawMode = false;
            this.isDisplayHImageData = true;
            this.CurrentButton = MouseButtons.None;
            roi = this.GetDrawPixRect2Param();
            this.DetachDrawingObjectFromWindow();
        }
        public override void ModifyPixRect2OnWindow(userPixRectangle2 pixRec2, out userPixRectangle2 outPixRec2)  //
        {
            this.isTranslate = false;
            this.isDisplayHImageData = true;
            this.isDrawMode = true;
            this.CurrentButton = MouseButtons.Left;
            this.mIsClick = false;
            this.isDrawMode = true;
            this.rect2 = new Rect2(pixRec2.Row, pixRec2.Col, pixRec2.Rad, pixRec2.Length1, pixRec2.Length2, pixRec2.DiffRadius);
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
            outPixRec2 = this.GetPixRectangle2Param();
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
                    if (selectedNode == pos)
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
            if (this.rect2.length1 != 0 && this.rect2.length2 != 0)
                xld.GenRectangle2ContourXld(this.rect2.row, this.rect2.col, this.rect2.phi, this.rect2.length1, this.rect2.length2);
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
            //HXLDCont rect = CreateRect2Object();
            //if (rect.Key.ToInt64() > 0)
            //    rect.DistancePc(y, x, out minDist, out maxDist);
            //else
            //    minDist = 1000;
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
            if (this.rect2.GetHXLD(this.nodeSizeRect).TestXldPoint(y, x) > 0)
                selectNode = PosSizableRect.Rect2Inside;
            if (minDist < 10 && dist5 < 10) // 表示移动
                selectNode = PosSizableRect.UpMidlleCorner;
            if (minDist < 10 && dist6 < 10) // 表示移动
                selectNode = PosSizableRect.RightMidlleCorner;
            /////////////////////////////
            if (minDist < 10 && dist7 < 10) // 表示移动
                selectNode = PosSizableRect.DownMidlleCorner;
            ///////////////////////////////
            if (minDist < 10 && dist8 < 10) // 表示移动
                selectNode = PosSizableRect.LeftMidlleCorner;
            //////////////////////////////////////////////////第二点
            //if (dist < Math.Abs((Math.Min(this.rect2.length1, this.rect2.length2))) * 0.7) //  - Math.Abs(this.rect2.diffRadius
            //    selectNode = PosSizableRect.Rect2Inside;
            //////////////////////////////////////////////////第二点
            if (dist1 < 10)
                selectNode = PosSizableRect.LeftUpCorner;
            //////////////////////////////////////////////////第二点
            if (dist2 < 10)
                selectNode = PosSizableRect.RightUpCorner;
            //////////////////////////////////////////////////第二点
            if (dist3 < 10)
                selectNode = PosSizableRect.RightDownCorner;
            //////////////////////////////////////////////////第二点
            if (dist4 < 10)
                selectNode = PosSizableRect.LeftDownCorner;
            //////////////////////////////////////////////////第三点
            //if (Math.Abs(this.rect2.diffRadius) * 0.9 < minDist && minDist < Math.Abs(this.rect2.diffRadius) * 1.1)
            //    selectNode = PosSizableRect.Arrow;
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

        /// <summary>
        /// 获取直线上指定长度处的点坐标
        /// </summary>
        /// <param name="RowBegin"></param>
        /// <param name="ColumnBegin"></param>
        /// <param name="RowEnd"></param>
        /// <param name="ColumnEnd"></param>
        /// <param name="length"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        private void GetPointLine(double RowBegin, double ColumnBegin, double RowEnd, double ColumnEnd, double length, out double Row, out double Col)
        {

            HTuple row, col;
            //////////////////
            if (HMisc.DistancePp(RowBegin, ColumnBegin, RowEnd, ColumnEnd) == 0)
            {
                Row = RowBegin;
                Col = ColumnBegin;
            }
            else
            {
                HOperatorSet.IntersectionSegmentCircle(RowBegin, ColumnBegin, RowEnd, ColumnEnd, RowBegin, ColumnBegin, length, 0, Math.PI * 2, "positive", out row, out col);
                if (row.Length > 0)
                {
                    Row = row.D;
                    Col = col.D;
                }
                else
                {
                    Row = RowEnd;
                    Col = ColumnEnd;
                }
            }

        }

        private void GetPointRect2(double Row, double Column, double phi, double length1, double length2, int num, out double[] rows, out double[] cols, out double[] phis)
        {
            rows = new double[(num) * 4];
            cols = new double[(num) * 4];
            phis = new double[(num) * 4];
            double leftUpRow, leftUpCol, leftDownRow, leftDownCol, rightUpRow, rightUpCol, rightDownRow, rightDownCol;
            ///////////////
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D homMat2dRotate = hHomMat2D.HomMat2dRotate(phi, Row, Column);
            leftUpRow = homMat2dRotate.AffineTransPoint2d(Row - length2, Column - length1, out leftUpCol);
            /////////////////////////
            leftDownRow = homMat2dRotate.AffineTransPoint2d(Row + length2, Column - length1, out leftDownCol); ;
            /////////////////////
            rightUpRow = homMat2dRotate.AffineTransPoint2d(Row - length2, Column + length1, out rightUpCol);
            ////////////////////////////
            rightDownRow = homMat2dRotate.AffineTransPoint2d(Row + length2, Column + length1, out rightDownCol);
            ////////////////////
            double row, col;
            double dist1Mea = HMisc.DistancePp(leftUpRow, leftUpCol, rightUpRow, rightUpCol) / (num - 1);
            double dist2Mea = HMisc.DistancePp(rightUpRow, rightUpCol, rightDownRow, rightDownCol) / (num - 1);
            double dist3Mea = HMisc.DistancePp(rightDownRow, rightDownCol, leftDownRow, leftDownCol) / (num - 1);
            double dist4Mea = HMisc.DistancePp(leftDownRow, leftDownCol, leftUpRow, leftUpCol) / (num - 1);
            //////////////////////////////////////////////////////
            for (int i = 0; i < num; i++)
            {
                GetPointLine(leftUpRow, leftUpCol, rightUpRow, rightUpCol, dist1Mea * i, out row, out col);
                rows[i] = row;
                cols[i] = col;
                phis[i] = Math.Atan2((leftUpRow - rightUpRow) * -1, leftUpCol - rightUpCol) - Math.PI * 0.5;
            }
            ////////////////////////
            for (int i = 0; i < num; i++)
            {
                GetPointLine(rightUpRow, rightUpCol, rightDownRow, rightDownCol, dist2Mea * i, out row, out col);
                rows[i + num] = row;
                cols[i + num] = col;
                phis[i + num] = Math.Atan2((rightUpRow - rightDownRow) * -1, rightUpCol - rightDownCol) - Math.PI * 0.5;
            }
            ////////////////////////
            for (int i = 0; i < num; i++)
            {
                GetPointLine(rightDownRow, rightDownCol, leftDownRow, leftDownCol, dist3Mea * i, out row, out col);
                rows[i + num * 2] = row;
                cols[i + num * 2] = col;
                phis[i + num * 2] = Math.Atan2((rightDownRow - leftDownRow) * -1, rightDownCol - leftDownCol) - Math.PI * 0.5;
            }
            ////////////////////////
            for (int i = 0; i < num; i++)
            {
                GetPointLine(leftDownRow, leftDownCol, leftUpRow, leftUpCol, dist4Mea * i, out row, out col);
                rows[i + num * 3] = row;
                cols[i + num * 3] = col;
                phis[i + num * 3] = Math.Atan2((leftDownRow - leftUpRow) * -1, leftDownCol - leftUpCol) - Math.PI * 0.5;
            }
        }

        /// <summary>
        /// 获取点的法向线坐标点
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="normalPhi"></param>
        /// <param name="normalLength"></param>
        /// <param name="Row1"></param>
        /// <param name="Col1"></param>
        /// <param name="Row2"></param>
        /// <param name="Col2"></param>
        private void GetNormalCoordPoint(double[] rows, double[] cols, double[] normalPhi, double normalLength, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            Row1 = new double[rows.Length];
            Col1 = new double[rows.Length];
            Row2 = new double[rows.Length];
            Col2 = new double[rows.Length];
            // 已知直线上的点坐标及该点的法线坐标，求法线直线的两点
            this.rect2.normalPhi = new double[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                Row1[i] = rows[i] + normalLength * Math.Sin(normalPhi[i]); // 因为像素坐标系与世界坐标系Y轴相反，所以这里Y也要用相反的操作符
                Col1[i] = cols[i] - normalLength * Math.Cos(normalPhi[i]);
                Row2[i] = rows[i] - normalLength * Math.Sin(normalPhi[i]);
                Col2[i] = cols[i] + normalLength * Math.Cos(normalPhi[i]);
                /////
                this.rect2.normalPhi[i] = Math.Atan2((Row2[i] - Row1[i]) * -1, Col2[i] - Col1[i]);
            }
        }
        private void GetArrowPoint(Rect2 pixRect2, int Num, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            ////////////////////////////////////////////
            double[] rows, cols, normalPhi;
            GetPointRect2(pixRect2.row, pixRect2.col, pixRect2.phi, pixRect2.length1, pixRect2.length2, Num, out rows, out cols, out normalPhi);
            //
            GetNormalCoordPoint(rows, cols, normalPhi, pixRect2.diffRadius, out Row1, out Col1, out Row2, out Col2);
        }
        public HXLDCont GetArrowXLD()
        {
            double[] row1, col1, row2, col2;
            HXLDCont hXLD = new HXLDCont();
            switch (this.ShowMode)
            {
                default:
                    break;
                case enShowMode.箭头:
                    GetArrowPoint(this.rect2, 5, out row1, out col1, out row2, out col2);
                    hXLD = GenArrowContourXld(row1, col1, row2, col2, this.nodeSizeRect, this.nodeSizeRect);
                    break;
                case enShowMode.矩形:
                    GetArrowPoint(this.rect2, 5, out row1, out col1, out row2, out col2);
                    for (int i = 0; i < row1.Length; i++)
                    {
                        HXLDCont rect2 = new HXLDCont();
                        rect2.GenRectangle2ContourXld((row1[i] + row2[i]) * 0.5, (col1[i] + col2[i]) * 0.5, this.rect2.normalPhi[i], this.rect2.diffRadius, 5);
                        hXLD = hXLD.ConcatObj(rect2);
                    }
                    break;
            }
            return hXLD;
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
        public drawPixRect2 GetDrawPixRect2Param()
        {
            drawPixRect2 PixRectangle2 = new drawPixRect2(this.rect2.row, this.rect2.col, this.rect2.phi, this.rect2.length1, this.rect2.length2);
            PixRectangle2 = PixRectangle2.AffinePixRect2(this.pixCoordSystem?.GetInvertVariationHomMat2D());
            return PixRectangle2;
        }
        public override userPixRectangle2 GetPixRectangle2Param()
        {
            userPixRectangle2 PixRectangle2 = new userPixRectangle2(this.rect2.row, this.rect2.col, this.rect2.phi, this.rect2.length1, this.rect2.length2, this.CameraParam);
            PixRectangle2.DiffRadius = this.rect2.diffRadius;
            PixRectangle2.NormalPhi = this.rect2.normalPhi;
            PixRectangle2.CamParams = this.CameraParam;
            PixRectangle2 = PixRectangle2.AffineTransPixRect2(this.pixCoordSystem?.GetInvertVariationHomMat2D());
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
