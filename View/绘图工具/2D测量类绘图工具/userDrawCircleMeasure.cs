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
    public class userDrawCircleMeasure : DrawingBaseMeasure
    {
        private userPixCircle pixCircle = new userPixCircle();
        private PosSizableRect selectedNode = PosSizableRect.None;  // 通过枚标识选择的节点
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
            None,
            InnerCircle,
            MiddleCircle,
            OutSideCircle,
            CircleCenter,
        }

        public userDrawCircleMeasure(HWindowControl hWindowControl) : base(hWindowControl)
        {

        }

        public userDrawCircleMeasure(HWindowControl hWindowControl, userWcsCircle wcsCircle, userWcsCoordSystem wcsCoordSystem) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            ////////////////////
            if (wcsCircle != null && wcsCircle.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                this.wcsCoordSystem = wcsCoordSystem;
                this.pixCoordSystem = this.wcsCoordSystem.GetPixCoordSystem();
                this.pixCircle = wcsCircle.AffineWcsCircle2D(wcsCoordSystem.GetVariationHomMat2D()).GetPixCircle();
                this.pixCircle.DiffRadius = wcsCircle.DiffRadius;
            }
            else
            {
                int row1, col1, row2, col2;
                this.GetImagePart(out row1, out col1, out row2, out col2);
                this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
            }
        }
        public userDrawCircleMeasure(HWindowControl hWindowControl, userPixCircle pixCircle, userPixCoordSystem pixCoordSystem) : base(hWindowControl)
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
        public userDrawCircleMeasure(HWindowControl hWindowControl, userWcsCircle wcsCircle) : base(hWindowControl)
        {
            this.wcsCoordSystem = new userWcsCoordSystem();
            ////////////////////
            if (wcsCircle != null && wcsCircle.CamParams != null)// 等于null表示是第一次使用，否则则不是第一次使用
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
        }
        public userDrawCircleMeasure(HWindowControl hWindowControl, userPixCircle pixCircle) : base(hWindowControl)
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
            if (param != null)// 等于null表示是第一次使用，否则则不是第一次使用
            {
                switch (param.GetType().Name)
                {
                    case nameof(userPixCircle):
                        if (!(param is userPixCircle)) throw new ArgumentException("给定的数据类型异常，需要传入 userPixCircle 类型");
                        userPixCircle pixCircle = (userPixCircle)param;
                        if (pixCircle.CamParams != null)
                        {
                            this.pixCircle = pixCircle;//.GetPixCircle();
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
                            this.pixCircle.DiffRadius = this.pixCircle.DiffRadius;
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
                this.pixCircle = new userPixCircle(row1 + (row2 - row1) * 0.2, col1 + (col2 - col1) * 0.2, (row2 - row1) * 0.06);
                this.pixCircle.DiffRadius = this.pixCircle.Radius * 0.5;
                this.wcsCoordSystem = new userWcsCoordSystem();
                this.pixCoordSystem = new userPixCoordSystem();
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
                        this.isTranslate = true;
                    else
                        this.isTranslate = false;
                    /////////////
                    base.hWindowControl_HMouseDown(sender, e);
                    break;
                case MouseButtons.Middle:
                    //if (this.circleMeasure != null)
                    //    this.circleMeasure.Execute(null);
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
                case PosSizableRect.CircleCenter:
                    this.pixCircle.Row = e.Y;
                    this.pixCircle.Col = e.X;
                    break;
                case PosSizableRect.OutSideCircle:
                    this.pixCircle.DiffRadius = HMisc.DistancePp(this.pixCircle.Row, this.pixCircle.Col, e.Y, e.X) - this.pixCircle.Radius;
                    break;
                case PosSizableRect.InnerCircle:
                    this.pixCircle.DiffRadius = this.pixCircle.Radius - HMisc.DistancePp(this.pixCircle.Row, this.pixCircle.Col, e.Y, e.X);
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
                //UpdataMeasureRegion();
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
                if (this.isDispalyAttachDrawingProperty)
                    this.AttachDrawingPropertyData.Add(GetDrawingObject(pos));
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
        private void GetArrowPoint(userPixCircle pixCircle, int Num, out double[] Row1, out double[] Col1, out double[] Row2, out double[] Col2)
        {
            Row1 = new double[Num];
            Col1 = new double[Num];
            Row2 = new double[Num];
            Col2 = new double[Num];
            double step = Math.PI * 2 / (Num - 1);
            pixCircle.NormalPhi = new double[Num];
            for (int i = 0; i < Num; i++)
            {
                Row1[i] = pixCircle.Row - (pixCircle.Radius - pixCircle.DiffRadius) * Math.Sin(step * i);
                Col1[i] = pixCircle.Col + (pixCircle.Radius - pixCircle.DiffRadius) * Math.Cos(step * i);
                //////////
                Row2[i] = pixCircle.Row - (pixCircle.Radius + pixCircle.DiffRadius) * Math.Sin(step * i);
                Col2[i] = pixCircle.Col + (pixCircle.Radius + pixCircle.DiffRadius) * Math.Cos(step * i);
                //
                pixCircle.NormalPhi[i] = Math.Atan2((Row2[i] - Row1[i]) * -1, Col2[i] - Col1[i]);
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
                    GetArrowPoint(this.pixCircle, 5, out row1, out col1, out row2, out col2);
                    hXLD = GenArrowContourXld(row1, col1, row2, col2, this.nodeSizeRect, this.nodeSizeRect);
                    break;
                case enShowMode.矩形:
                    GetArrowPoint(this.pixCircle, 5, out row1, out col1, out row2, out col2);
                    for (int i = 0; i < row1.Length; i++)
                    {
                        HXLDCont rect2 = new HXLDCont();
                        rect2.GenRectangle2ContourXld((row1[i] + row2[i]) * 0.5, (col1[i] + col2[i]) * 0.5, this.pixCircle.NormalPhi[i], this.pixCircle.DiffRadius, 5);
                        hXLD = hXLD.ConcatObj(rect2);
                    }
                    break;
            }
            return hXLD;
        }

        private HXLDCont GetDrawingObject(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.RightBottomNode:
                    return CreateRectSizableNode(this.pixCircle.Col + pixCircle.Radius, this.pixCircle.Row);
                case PosSizableRect.UpMiddleNode:
                    return CreateRectSizableNode(this.pixCircle.Col, this.pixCircle.Row + pixCircle.Radius);
                case PosSizableRect.LeftBottomNode:
                    return CreateRectSizableNode(this.pixCircle.Col - pixCircle.Radius, this.pixCircle.Row);
                case PosSizableRect.DownMiddleNode:
                    return CreateRectSizableNode(this.pixCircle.Col, this.pixCircle.Row - pixCircle.Radius);
                //////////////////////////////////
                case PosSizableRect.MiddleCircle:
                    return CreateCircleSectorObject(this.pixCircle);
                case PosSizableRect.None:
                    return GetArrowXLD();
                default:
                    return new HXLDCont(0, 0);
            }
        }
        private PosSizableRect GetNodeSelectable(double x, double y)
        {
            PosSizableRect posSizable = PosSizableRect.None;
            double dist = Math.Sqrt((y - this.pixCircle.Row) * (y - this.pixCircle.Row) + (x - this.pixCircle.Col) * (x - this.pixCircle.Col));
            //if (dist < (this.pixCircle.Radius - Math.Abs(this.pixCircle.DiffRadius)) * 0.6)
            //    return PosSizableRect.CircleCenter;
            if (this.pixCircle.GetXLD(this.pixCircle.DiffRadius * 1.1).TestXldPoint(y, x) > 0)
                posSizable = PosSizableRect.CircleCenter;
            //////////////////////////////////////////////////
            if ((this.pixCircle.Radius - this.pixCircle.DiffRadius) * 0.9 < dist && (this.pixCircle.Radius - this.pixCircle.DiffRadius) * 1.1 > dist)
                posSizable = PosSizableRect.InnerCircle;
            //////////////////////////////////////////////////
            if ((this.pixCircle.Radius) * 0.9 < dist && (this.pixCircle.Radius) * 1.1 > dist)
                posSizable = PosSizableRect.MiddleCircle;
            //////////////////////////////////////////////////
            if ((this.pixCircle.Radius + this.pixCircle.DiffRadius) * 0.9 < dist && (this.pixCircle.Radius + this.pixCircle.DiffRadius) * 1.1 > dist)
                posSizable = PosSizableRect.OutSideCircle;
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
                    cursors = Cursors.Cross;
                    break;

                case PosSizableRect.CircleCenter:
                    cursors = Cursors.Hand;
                    break;
                //return Cursors.Hand;

                case PosSizableRect.UpMiddleNode:
                case PosSizableRect.DownMiddleNode:
                    cursors = Cursors.Cross;
                    break;
                //return Cursors.SizeNS;

                case PosSizableRect.LeftBottomNode:
                case PosSizableRect.RightBottomNode:
                    cursors = Cursors.Cross;
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

        private void UpdataMeasureRegion()
        {
            /// 设置计量模型
            //if (this.backImage == null && this.findCircle == null) return; // 在这里将世界坐标还原了
            //this.pixCircle.camParam = this.backImage.CamParam;
            //this.pixCircle.camPose = this.backImage.CamPose;
            //this.findCircle.CircleWcsPosition = this.pixCircle.GetWcsCircle(this.backImage.X_pos, this.backImage.Y_pos).Affine2DWcsCircle(this.findCircle.WcsCoordSystem.GetInvertVariationHomMat2D());
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

        public override userPixCircle GetPixCircleParam()
        {
            userPixCircle circlePixPosition = new userPixCircle(this.pixCircle.Row, this.pixCircle.Col, this.pixCircle.Radius, this.backImage?.CamParams);
            circlePixPosition.DiffRadius = this.pixCircle.DiffRadius;
            circlePixPosition.NormalPhi = this.pixCircle.NormalPhi;
            circlePixPosition = circlePixPosition.AffineTransPixCircle(this.pixCoordSystem.GetInvertVariationHomMat2D()); // 变换为之前的
            return circlePixPosition;
        }
        public override userWcsCircle GetWcsCircleParam()
        {
            //userWcsCircle circleWcsPosition = GetPixCircleParam().GetWcsCircle(this.backImage.Grab_X, this.backImage.Grab_Y).AffineWcsCircle2D(this.wcsCoordSystem.GetInvertVariationHomMat2D()); // 根据当前位置来修改参考位置
            userWcsCircle circleWcsPosition = GetPixCircleParam().GetWcsCircle(0, 0,0);
            return circleWcsPosition;
        }

    }





}
