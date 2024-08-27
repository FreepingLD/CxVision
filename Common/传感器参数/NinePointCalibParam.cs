using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(NinePointCalibParam))]
    public class NinePointCalibParam
    {
        public string CamName { get; set; }
        public string MapCamName { get; set; }
        public enCoordSysName CoordSysName { get; set; }
        public enCoordValueType CoordValueType { get; set; } // 
        public enCoordOriginType CoordOriginType { get; set; } //
        public enCamCaliModel CamCaliModel { get; set; }
        /// <summary> 相机是否随着X轴移动</summary>
        public bool IsMoveX { get; set; }
        /// <summary> 相机是否随着Y轴移动 </summary>
        public bool IsMoveY { get; set; }
        /// <summary> 相机是否随着Z轴移动 </summary>
        public bool IsMoveZ { get; set; }
        public string Describe { get; set; }

        /// <summary>标定方法 </summary>
        public enCalibMethod CalibMethod { get; set; }
        /// <summary> 旋转标定的起始点坐标X</summary>
        public userWcsVector RotateCalibPoint { get; set; }

        /// <summary> 角度旋转范围 </summary>
        public double AngleRange { get; set; }
        /// <summary>  角度旋转次数 </summary>
        public double AngleStep { get; set; }
        public enRotateDirection RotateDirection { get; set; }
        /// <summary> 角度旋转范围 </summary>
        public double AngleRangeAxisV { get; set; }
        /// <summary>  角度旋转次数 </summary>
        public double AngleStepAxisV { get; set; }
        public enRotateDirection RotateDirectionAxisV { get; set; }

        /// <summary> 角度旋转范围 </summary>
        public double AngleRangeAxisU { get; set; }
        /// <summary>  角度旋转次数 </summary>
        public double AngleStepAxisU { get; set; }
        public enRotateDirection RotateDirectionAxisU { get; set; }

        /// <summary>标定起始点X   </summary> 
        public userWcsVector StartCaliPoint { get; set; }
        /// <summary>标定终止点 </summary>
        public userWcsVector EndCalibPoint { get; set; }

        /// <summary>
        /// 标定时的相机轴坐标
        /// </summary>
        public userWcsPoint CamAxisCoord { get; set; }
        /// <summary>
        /// 旋转标定的标定中心
        /// </summary>
        public userWcsPoint CalibCenterXy { get; set; }
        public userWcsPoint CalibCenterXz { get; set; }
        public userWcsPoint CalibCenterYz { get; set; }

        public enCalibAxis CalibAxis { get; set; }

        public enCalculateMethod CalculateMethod { get; set; }
        public enInvertAxis InvertAxis { get; set; }
        public bool InvertX { get; set; }
        public bool InvertY { get; set; }
        public bool InvertZ { get; set; }
       
   
        public enMoveStage MoveStage { get; set; }
        public enCalibPlane CalibPlane { get; set; }
        /// <summary> 标定矩阵C02补偿量 </summary>
        public double AdjHomMatC02X { get; set; } = 0;
        /// <summary> 标定矩阵C12补偿量 </summary>
        public double AdjHomMatC12Y { get; set; } = 0;
        public object NONE { get; set; }

        public NinePointCalibParam() // 类中必须包含有无参构造函数，否则不能序列化和反序列化
        {
            this.CamName = "";
            this.MapCamName = "NONE";
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.CoordValueType = enCoordValueType.相对坐标;
            this.CoordOriginType = enCoordOriginType.机械原点;
            this.CamCaliModel = enCamCaliModel.Cali9PtCali;
            this.RotateCalibPoint = new userWcsVector();
            this.CalibCenterXy = new userWcsPoint();
            this.CalibCenterXz = new userWcsPoint();
            this.CalibCenterYz = new userWcsPoint();
            this.AngleRange = 2;
            this.AngleStep = 3;
            this.AngleRangeAxisV = 2;
            this.AngleStepAxisV = 3;
            this.AngleRangeAxisU = 2;
            this.AngleStepAxisU = 3;
            this.StartCaliPoint = new userWcsVector();
            this.EndCalibPoint = new userWcsVector();
            this.IsMoveX = false; // 默认为 true 表示以相机作为参考对象来定义轴方向 
            this.IsMoveY = false;
            this.IsMoveZ = true;
            this.Describe = "NONE";
            this.CalibMethod = enCalibMethod.先平移后旋转;
            this.CalibAxis = enCalibAxis.XY轴;
            this.InvertAxis = enInvertAxis.NONE;
            this.CalculateMethod = enCalculateMethod.拟合圆;
            this.RotateDirection = enRotateDirection.双向;
            this.MoveStage = enMoveStage.PLC;
            this.RotateDirectionAxisU = enRotateDirection.双向;
            this.RotateDirectionAxisV = enRotateDirection.双向;
            this.CalibPlane = enCalibPlane.XY;
            this.InvertX = false;
            this.InvertY = false;
            this.InvertZ = false;
            this.CamAxisCoord = new userWcsPoint();
        }
        public NinePointCalibParam(string camName = "Cam1")
        {
            this.CamName = camName;
            this.MapCamName = "NONE";
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.CoordValueType = enCoordValueType.相对坐标;
            this.CoordOriginType = enCoordOriginType.机械原点;
            this.CamCaliModel = enCamCaliModel.Cali9PtCali;
            this.RotateCalibPoint = new userWcsVector();
            this.CalibCenterXy = new userWcsPoint();
            this.CalibCenterXz = new userWcsPoint();
            this.CalibCenterYz = new userWcsPoint();
            this.AngleRange = 2;
            this.AngleStep = 3;
            this.AngleRangeAxisV = 2;
            this.AngleStepAxisV = 3;
            this.AngleRangeAxisU = 2;
            this.AngleStepAxisU = 3;
            this.StartCaliPoint = new userWcsVector();
            this.EndCalibPoint = new userWcsVector();
            this.IsMoveX = false; // 默认为 true 表示以相机作为参考对象来定义轴方向 
            this.IsMoveY = false;
            this.IsMoveZ = true;
            //this.HomMat2D = new UserHomMat2D(true);
            //this.MapHomMat2D = new UserHomMat2D(true);
            this.Describe = "NONE";
            this.CalibMethod = enCalibMethod.先平移后旋转;
            this.CalibAxis = enCalibAxis.XY轴;
            this.InvertAxis = enInvertAxis.NONE;
            this.CalculateMethod = enCalculateMethod.拟合圆;
            this.RotateDirection = enRotateDirection.双向;
            this.MoveStage = enMoveStage.PLC;
            this.RotateDirectionAxisU = enRotateDirection.双向;
            this.RotateDirectionAxisV = enRotateDirection.双向;
            this.CalibPlane = enCalibPlane.XY;
            this.InvertX = false;
            this.InvertY = false;
            this.InvertZ = false;
            this.CamAxisCoord = new userWcsPoint();
        }


        /// <summary>
        ///  获取手眼标定的9点
        /// </summary>
        /// <param name="StartPt"></param>
        /// <param name="EndPt"></param>
        /// <param name="StepCount"></param>
        /// <returns></returns>
        public List<userWcsVector> GetHandEyeMovePoint(int StepCount = 3)
        {
            List<userWcsVector> MotionPosW = new List<userWcsVector>();
            userWcsVector startPt = new userWcsVector(Math.Min(this.StartCaliPoint.X, this.EndCalibPoint.X), Math.Min(this.StartCaliPoint.Y, this.EndCalibPoint.Y));
            userWcsVector endPt = new userWcsVector(Math.Max(this.StartCaliPoint.X, this.EndCalibPoint.X), Math.Max(this.StartCaliPoint.Y, this.EndCalibPoint.Y));
            double recWidth = endPt.X - startPt.X;
            double recHeight = endPt.Y - startPt.Y;
            double stepX = recWidth / (StepCount - 1);
            double stepY = recHeight / (StepCount - 1);
            for (int row = 0; row <= StepCount - 1; row += 1)
            {
                for (int col = 0; col <= StepCount - 1; col += 1)
                {
                    //九点标定时的点，坐下角为第一个点，右上角为第九个点
                    MotionPosW.Add(new userWcsVector(startPt.X + col * stepX, startPt.Y + row * stepY));
                }
            }
            return MotionPosW;
        }



        /// <summary>
        /// 像素坐标转换到机械坐标或者世界坐标系,
        /// 2)相机移动：机械坐标 = 计算出的坐标+当前拍照的坐标;相机固定：机械坐标 = 计算出的坐标+当前拍照的坐标*-1
        /// 物体相对于机械坐标原点的坐标：
        /// 1)相机移动：机械坐标 = 计算出的坐标+当前拍照的坐标;
        /// 2）相机固定：机械坐标 = 计算出的坐标+当前拍照的坐标*-1
        /// 物体相对于旋转中心的坐标：
        /// 1）相机移动：相机坐标 = 计算出的坐标 + 当前拍照的坐标 - 标定中心坐标;
        /// 1）相机固定：相机坐标 = 计算出的坐标 + 当前拍照的坐标*-1 + 标定中心坐标;
        /// 注：X轴向右移动为正，Y轴向前移动为正
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Coluns"></param>
        /// <param name="grabImage_x"></param>
        /// <param name="grabImage_y"></param>
        /// <param name="wcs_x"></param>
        /// <param name="wcs_y"></param>
        public void ImagePointsToWorldPlane(HTuple Rows, HTuple Coluns, double grabImage_x, double grabImage_y, out HTuple wcs_x, out HTuple wcs_y)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            if (Rows == null)
            {
                throw new ArgumentNullException("Rows");
            }
            if (Coluns == null)
            {
                throw new ArgumentNullException("Coluns");
            }
            if (Rows.Length != Coluns.Length)
            {
                throw new ArgumentException("参Rows与Coluns长度不相等");
            }
            /////////////////////////////////////////////////////////
            double caliCenter_x = 0.5 * (this.StartCaliPoint.X + this.EndCalibPoint.X);
            double caliCenter_y = 0.5 * (this.StartCaliPoint.Y + this.EndCalibPoint.Y);
            //Qx = this.HomMat2D.GetHHomMat().AffineTransPoint2d(Coluns, Rows, out Qy);
            ////////////////////////////////////////
            switch (this.CoordOriginType)
            {
                default:
                case enCoordOriginType.机械原点:
                    if (this.IsMoveX) // 拍照坐标 + 计算值
                        wcs_x = grabImage_x + Qx;
                    else              // 拍照坐标*-1 + 计算值
                        wcs_x = grabImage_x * -1 + Qx;
                    if (this.IsMoveY)
                        wcs_y = grabImage_y + Qy;
                    else
                        wcs_y = grabImage_y * -1 + Qy;
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心:
                    if (this.IsMoveX) // 拍照坐标 + 计算值 - 标定中心值
                        wcs_x = Qx + grabImage_x - caliCenter_x;
                    else              // 拍照坐标*-1 + 计算值 + 标定中心值
                        wcs_x = Qx - grabImage_x + caliCenter_x;
                    if (this.IsMoveY)
                        wcs_y = Qy + grabImage_y - caliCenter_y;
                    else
                        wcs_y = Qy - grabImage_y + caliCenter_y;
                    break;
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    wcs_x = Qx + caliCenter_x;
                    wcs_y = Qy + caliCenter_y;
                    break;
            }

        }
        public void WorldPointsToImagePlane(HTuple wcs_x, HTuple wcs_y, double grabImage_x, double grabImage_y, out HTuple Rows, out HTuple Coluns)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            Rows = 0;
            Coluns = 0;
            if (wcs_x == null)
            {
                throw new ArgumentNullException("wcs_x");
            }
            if (wcs_y == null)
            {
                throw new ArgumentNullException("wcs_y");
            }
            if (wcs_x.Length != wcs_y.Length)
            {
                throw new ArgumentException("参wcs_x与wcs_y长度不相等");
            }
            /////////////////////////////////////////////////////////
            double caliCenter_x = 0.5 * (this.StartCaliPoint.X + this.EndCalibPoint.X);
            double caliCenter_y = 0.5 * (this.StartCaliPoint.Y + this.EndCalibPoint.Y);
            ////////////////////////////////////////
            switch (this.CoordOriginType)
            {
                default:
                case enCoordOriginType.机械原点:
                    if (this.IsMoveX)
                        Qx = wcs_x - grabImage_x;
                    else
                        Qx = wcs_x + grabImage_x;
                    if (this.IsMoveY)
                        Qy = wcs_y - grabImage_y;
                    else
                        Qy = wcs_y + grabImage_y;  // 相当于上下料模式
                    break;
                //////////////////  相对于某参考点的坐标，一般为旋转中心
                case enCoordOriginType.旋转中心: // 相对坐标与相机的移动无关
                    if (this.IsMoveX)
                        Qx = wcs_x + caliCenter_x - grabImage_x; // Qx + grab_x:当前的机械坐标，caliCenter_x：9点标定的标定中心点
                    else
                        Qx = wcs_x - caliCenter_x + grabImage_x;
                    if (this.IsMoveY)
                        Qy = wcs_y + caliCenter_y - grabImage_y;
                    else
                        Qy = wcs_y - caliCenter_y + grabImage_y;
                    break;
                case enCoordOriginType.IsLoading: // 表示在一个大视野相机下，执行上下料模式,这种情况下相机必需是静止的，至少拍照位置不能变
                    Qx = wcs_x - caliCenter_x;
                    Qy = wcs_y - caliCenter_y;
                    break;
            }
            // 世界点到图像点
            // Coluns = this.HomMat2D.GetHHomMat().HomMat2dInvert().AffineTransPoint2d(Qx, Qy, out Rows);
        }

        /// <summary>
        ///  将9点标定矩阵转换为相机的内外参形式
        /// </summary>
        /// <param name="hHomMat2D">N点标定矩阵</param>
        /// <param name="imageWidth">图像宽</param>
        /// <param name="imageHeight">图像高</param>
        /// <param name="Magnification">镜头放大倍率</param>
        /// <param name="camParam">相机内参</param>
        /// <param name="camPose">相机外参</param>
        /// <param name="Quality">质量</param>
        public static void HHomMat2DToCamparaCamPos(HHomMat2D hHomMat2D, int imageWidth, int imageHeight, double Magnification, out userCamParam camParam, out userCamPose camPose, out double Quality)
        {
            camParam = new userCamParam();
            camPose = new userCamPose();
            Quality = -1;
            if (hHomMat2D == null) throw new ArgumentNullException("hHomMat2D");
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            //camParam.HomMat2D = new UserHomMat2D(hHomMat2D);
            camParam.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
            camParam.Focus = Magnification; // 这个值应该为镜头的倍率
            camParam.Sx = Sx;
            camParam.Sy = Sy;
            camParam.Cx = imageWidth * 0.5;
            camParam.Cy = imageHeight * 0.5;
            camParam.Width = imageWidth;
            camParam.Height = imageHeight;
            // 利用图像4个角点的世界坐标来计算位姿
            HTuple Qx, Qy, Quality2;
            HTuple Rows = new HTuple(0, imageHeight, imageHeight, 0);
            HTuple Columns = new HTuple(0, 0, imageWidth, imageWidth);
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, camParam.GetHCamPar(), "telecentric_planar_robust", "error", out Quality2);
            Quality = Quality2[0].D;
            camPose = new userCamPose(pose);
            camPose.Tz = 100;
        }

        /// <summary>
        /// 九点标定，用于XY轴标定, 列对应 X，行对应 Y
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="Rows"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="HomMat2D00"></param>
        public static void Calibra9Point(HTuple Rows, HTuple Columns, HTuple x, HTuple y, out HHomMat2D HomMat2D, out double MaxError)
        {
            HomMat2D = new HHomMat2D();
            HTuple Qx, Qy, DistTest;
            MaxError = -1;
            try
            {
                HomMat2D.VectorToHomMat2d(Columns, Rows, x, y);
                Qx = HomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
                DistTest = HMisc.DistancePp(x, y, Qx, Qy);
                MaxError = DistTest.TupleMax().D;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 相似九点标定,用于单独标定 
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="Rows"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="HomMat2D00"></param>
        public static void Calibra9PointtSimilar(HTuple Columns, HTuple Rows, HTuple x, HTuple y, out HHomMat2D HomMat2D, out double MaxError)
        {
            HomMat2D = new HHomMat2D();
            HTuple Qx, Qy, DistTest;
            HomMat2D.VectorToSimilarity(Columns, Rows, x, y);
            Qx = HomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            DistTest = HMisc.DistancePp(x, y, Qx, Qy);
            MaxError = DistTest.TupleMax().D;
            /////////////////////////////////////////
        }

        public static void FitCircle(HTuple Rows, HTuple Cols, out double CenterRow, out double CenterCol, out double radius)
        {
            CenterRow = 0;
            CenterCol = 0;
            radius = 0;
            HTuple Row, Column, StartPhi, Radius, EndPhi, PointOrder;
            if (Rows == null)
                throw new ArgumentNullException("Rows");
            if (Cols == null)
                throw new ArgumentNullException("Cols");
            if (Rows.Length < 3 || Cols.Length < 3)
                throw new ArgumentException("点数不能小于3个");
            if (Rows.Length != Cols.Length)
                throw new ArgumentException("行和列的点数不一致");
            /////////////////////////////////////////////////////////////////////////////////////////////
            new HXLDCont(Rows, Cols).FitCircleContourXld("geometric", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            if (Row.Length > 0)
            {
                CenterRow = Row[0].D;
                CenterCol = Column[0].D;
                radius = Radius[0].D;
            }
        }


        /// <summary>
        /// 有两个圆心，手动选择
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Cols"></param>
        /// <param name="stepAngle"></param>
        /// <param name="centerRow"></param>
        /// <param name="centerCol"></param>
        /// <param name="centerRow2"></param>
        /// <param name="centerCol2"></param>
        public static void CalculateCenter(double[] Rows, double[] Cols, double stepAngle, out double centerRow, out double centerCol)
        {
            if (Rows.Length < 2)
                throw new ArgumentException("元素个数不能小于2");
            if (Rows.Length != Cols.Length)
                throw new ArgumentException("输入元素个数不相等");
            List<userPixLine> listPixLine = new List<userPixLine>();
            double line1Row1, line1Row2, line1Col1, line1Col2;
            List<double> listRow = new List<double>();
            List<double> listCol = new List<double>();
            List<double> listRow2 = new List<double>();
            List<double> listCol2 = new List<double>();
            List<double> radius = new List<double>();
            HTuple row, col;
            for (int i = 0; i < Rows.Length - 1; i++)
            {
                NormalLine(Rows[i], Cols[i], Rows[i + 1], Cols[i + 1], out line1Row1, out line1Col1, out line1Row2, out line1Col2);
                listPixLine.Add(new userPixLine(line1Row1, line1Col1, line1Row2, line1Col2)); // 法向直线集合
                double dist = HMisc.DistancePp(Rows[i], Cols[i], Rows[i + 1], Cols[i + 1]);
                double R = dist * 0.5 / Math.Sin(stepAngle * 0.5 * Math.PI / 180.0);
                radius.Add(R);
                HOperatorSet.IntersectionLineCircle(line1Row1, line1Col1, line1Row2, line1Col2, (Rows[i] + Rows[i + 1]) * 0.5, (Cols[i] + Cols[i + 1]) * 0.5,
                R, 0, 6.28318, "positive", out row, out col);
                if (row.Length > 0)
                {
                    if (row.Length > 0)
                    {
                        listRow.Add(row[0]);
                        listCol.Add(col[0]);
                    }
                    if (row.Length > 1)
                    {
                        listRow2.Add(row[1]);
                        listCol2.Add(col[1]);
                    }
                }
            }
            double centerRow1 = listRow.Average();
            double centerCol1 = listCol.Average();
            double centerRow2 = listRow2.Average();
            double centerCol2 = listCol2.Average();
            /////////////////////////////////////////////////////////
            HTuple Row, Column, StartPhi, Radius, EndPhi, PointOrder;
            new HXLDCont(Rows, Cols).FitCircleContourXld("geometric", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);

            if (Math.Abs(Row.D - centerRow1) < Math.Abs(Row.D - centerRow2) && Math.Abs(Column.D - centerCol1) < Math.Abs(Column.D - centerCol2))
            {
                centerRow = centerRow1;
                centerCol = centerCol1;
            }
            else
            {
                centerRow = centerRow2;
                centerCol = centerCol2;
            }
            #region 圆心方向判断
            if (Row.D / centerRow1 > 0 && Column / centerCol1 > 0)
            {
                centerRow = centerRow1;
                centerCol = centerCol1;
            }
            else
            {
                centerRow = centerRow2;
                centerCol = centerCol2;
            }
            #endregion
        }
        private static void NormalLine(double Row1, double Column1, double Row2, double Column2, out double lineRow1, out double lineCol1, out double lineRow2, out double lineCol2)
        {
            double mid_row = (Row1 + Row2) * 0.5;
            double mid_col = (Column1 + Column2) * 0.5;
            double ATan = Math.Atan2(Row1 - Row2, Column1 - Column2);
            double k = 1 / Math.Tan(ATan) * -1;
            double b = mid_row - mid_col * k;
            ///////////////////
            double y = (mid_col + 1) * k + b;
            lineRow1 = mid_row;
            lineCol1 = mid_col;
            lineRow2 = y;
            lineCol2 = mid_col + 1;
        }


        public virtual bool Save(string SavePath)
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            IsOk = IsOk && XML<NinePointCalibParam>.Save(this, SavePath + ".xml");
            return IsOk;
        }
        public virtual object Read(string SavePath)
        {
            if (File.Exists(SavePath + ".xml"))
                return XML<NinePointCalibParam>.Read(SavePath + ".xml");
            else
                return new NinePointCalibParam();
        }


    }





}
