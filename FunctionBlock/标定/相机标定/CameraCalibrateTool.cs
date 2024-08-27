using Sensor;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.IO;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光扫描采集的数据类
    /// </summary>
    public class CameraCalibrateTool : INotifyPropertyChanged
    {
        private string method = "telecentric_planar";
        private double threshold = 128;
        private double circleDist = 2;
        private double circleRadius = 0.5;
        private double r_z = 0;
        private double axis_length = 1;
        private userPixRectangle2 rect2 = new userPixRectangle2();
        private int rowCount = 5;
        private int colCount = 5;
        private enCameraModel cameraModel = enCameraModel.area_scan_telecentric_division;
        private double x_offset = 0;
        private double y_offset = 0;
        private enCamCalibrateType camCalibrateType = enCamCalibrateType.单相机移动标定;
        private double pixWidth = 3.45;
        private double pixHeight = 3.45;
        private double focus = 0;
        private double magnification =0;
        private double kappa;
        private double tilt;
        private double rot;
        private HTuple camParam;
        private int imageWidth = 2048;
        private int imageHeight = 1536;
        /// /////////////
        private double K1;
        private double K2;
        private double K3;
        private double P1;
        private double P2;
        public event PropertyChangedEventHandler PropertyChanged;


        public enCameraModel CameraType
        {
            get
            {
                return cameraModel;
            }

            set
            {
                cameraModel = value;
                InitcamParam();
            }
        }
        public double PixWidth
        {
            get
            {
                return pixWidth;
            }

            set
            {
                pixWidth = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PixWidth"));
                InitcamParam();
            }
        }
        public double PixHeight
        {
            get
            {
                return pixHeight;
            }

            set
            {
                pixHeight = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PixHeight"));
                InitcamParam();
            }
        }
        public double Focus
        {
            get
            {
                return focus;
            }

            set
            {
                focus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Focus"));
                InitcamParam();
            }
        }
        public double Tilt
        {
            get
            {
                return tilt;
            }

            set
            {
                tilt = value;
                InitcamParam();
            }
        }
        public double Rot
        {
            get
            {
                return rot;
            }

            set
            {
                rot = value;
                InitcamParam();
            }
        }
        public int ImageWidth
        {
            get
            {
                return imageWidth;
            }

            set
            {
                imageWidth = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ImageWidth"));
                InitcamParam();
            }
        }
        public int ImageHeight
        {
            get
            {
                return imageHeight;
            }

            set
            {
                imageHeight = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ImageHeight"));
                InitcamParam();
            }
        }
        public int RowCount
        {
            get
            {
                return rowCount;
            }

            set
            {
                rowCount = value;
            }
        }
        public int ColCount
        {
            get
            {
                return colCount;
            }

            set
            {
                colCount = value;
            }
        }
        public userPixRectangle2 Rect2
        {
            get
            {
                return rect2;
            }

            set
            {
                rect2 = value;
            }
        }
        public double CircleDist
        {
            get
            {
                return circleDist;
            }

            set
            {
                circleDist = value;
            }
        }
        public double Threshold
        {
            get
            {
                return threshold;
            }

            set
            {
                threshold = value;
            }
        }
        public string Method
        {
            get
            {
                return method;
            }

            set
            {
                method = value;
            }
        }
        public double R_z
        {
            get
            {
                return r_z;
            }

            set
            {
                r_z = value;
            }
        }
        public double Axis_length
        {
            get
            {
                return axis_length;
            }

            set
            {
                axis_length = value;
            }
        }

        public double X_offset { get => x_offset; set => x_offset = value; }
        public double Y_offset { get => y_offset; set => y_offset = value; }
        public enCamCalibrateType CamCalibrateType { get => camCalibrateType; set => camCalibrateType = value; }
        public double CircleRadius { get => circleRadius; set => circleRadius = value; }
        public double Magnification { get => magnification; set => magnification = value; }

        public CameraCalibrateTool()
        {
            InitcamParam();
        }

        private void InitcamParam()
        {
            switch (cameraModel)
            {
                case enCameraModel.area_scan_division:
                    this.camParam = new HTuple("area_scan_division", focus, kappa, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_polynomial:
                    this.camParam = new HTuple("area_scan_polynomial", focus, K1, K2, K3, P1, P2, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_tilt_division:
                    this.camParam = new HTuple("area_scan_tilt_division", focus, kappa, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_tilt_polynomial:
                    this.camParam = new HTuple("area_scan_tilt_polynomial", focus, K1, K2, K3, P1, P2, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                    //////////////  远心镜头摄像机模型
                case enCameraModel.area_scan_telecentric_division:
                    this.focus = magnification;
                    this.camParam = new HTuple("area_scan_telecentric_division", magnification, kappa, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_telecentric_polynomial:
                    this.focus = magnification;
                    this.camParam = new HTuple("area_scan_telecentric_polynomial", magnification, K1, K2, K3, P1, P2, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_telecentric_tilt_division:
                    this.focus = magnification;
                    this.camParam = new HTuple("area_scan_telecentric_tilt_division", magnification, kappa, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
                case enCameraModel.area_scan_telecentric_tilt_polynomial:
                    this.focus = magnification;
                    this.camParam = new HTuple("area_scan_telecentric_tilt_polynomial", magnification, K1, K2, K3, P1, P2, tilt, rot, pixWidth, pixHeight, imageWidth / 2.0, imageHeight / 2.0, imageWidth, imageHeight);
                    break;
            }
            //HOperatorSet.ResetObjDb(imageWidth, imageHeight, 0);
        }
        public HXLDCont CalibrateCamera(HImage image, double camSlant, out HTuple camParam, out HTuple camPose, out HTuple Error, out AxisCalibration calibrateFile)
        {
            camParam = null;
            camPose = null;
            calibrateFile = null;
            Error = 10;
            HRegion region;
            HTuple width, height;
            HTuple Nx = new HTuple();
            HTuple Ny = new HTuple();
            HTuple Nz = new HTuple();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple StartCamParam, NStartPose, Quality, NFinalPose, homPose, homMat3d, homMat3dRotateX, homMat3dRotateY, homMat3dRotateZ, X, Y;
            HTuple circleRow, circleColumn, circleRadius, StartPhi, EndPhi, PointOrder, sortRow, sortCol, sortRow2, sortCol2;
            HObject line;
            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist, phi;
            HalconLibrary ha = new HalconLibrary();
            ///////////////////////////////////////////////////////
            try
            {
                HOperatorSet.GetImageSize(image, out width, out height);
                if (width.I != this.imageWidth || height.I != this.imageHeight)
                {
                    this.ImageWidth = width.I; // 改变图像后一定要重设一次参数
                    this.ImageHeight = height.I;
                }
                // 计算像素位置点
                region = new HRegion();
                region.GenRectangle2(this.rect2.Row, this.rect2.Col, this.rect2.Rad, this.rect2.Length1, this.rect2.Length2);
                HXLDCont circle = image.ReduceDomain(region).ThresholdSubPix(this.threshold).UnionCocircularContoursXld(0.5, 0.1, 0.2, 30, 10, 10, "true", 3); //              
                HTuple length = circle.LengthXld();
                circle = circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.66, length.TupleMax().D * 1.05);
                int count = circle.CountObj();
                circle.FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out circleRow, out circleColumn, out circleRadius, out StartPhi, out EndPhi, out PointOrder);
                circle.GenCircleContourXld(circleRow, circleColumn, circleRadius, StartPhi, EndPhi, PointOrder, 0.001);
                ha.sortPairs(circleRow, circleColumn, 1, out sortRow, out sortCol);// 行排序
                if (this.rowCount * this.colCount != sortRow.Length)
                    return circle;
                for (int i = 0; i < this.rowCount; i++)
                {
                    // 列排序
                    ha.sortPairs(sortRow.TupleSelectRange(i * this.colCount, (i + 1) * this.colCount - 1), sortCol.TupleSelectRange(i * this.colCount, (i + 1) * this.colCount - 1), 2, out sortRow2, out sortCol2);
                    Rows.Append(sortRow2);
                    Columns.Append(sortCol2);
                }
                for (int i = 0; i < this.rowCount; i++)
                {
                    // 以左上角第一个点作为参考点
                    for (int j = 0; j < this.colCount; j++)
                    {
                        Nx.Append(j * this.circleDist); //this.x_offset +
                        Ny.Append(i * this.circleDist * -1); // this.y_offset -   以世界坐标系作为参考坐标系
                        Nz.Append(0);
                    }
                }
                ///////////////////////
                if (Rows.Length != Nx.Length) MessageBox.Show("理论点与图像点不一致");
                // 修改相机参数
                StartCamParam = this.camParam;
                //double dist_x = HMisc.DistancePp(Rows[0].D, Columns[0].D, Rows[this.colCount-1].D, Columns[this.colCount - 1].D);
                //double dist_y = HMisc.DistancePp(Rows[0].D, Columns[0].D, Rows[(this.rowCount-1)*this.colCount].D, Columns[(this.rowCount - 1) * this.colCount].D);
                //StartCamParam[2] = this.circleRadius/ circleRadius.TupleMean().D;   //this.pixWidth * 0.001 ;
                //StartCamParam[3] = this.circleRadius / circleRadius.TupleMean().D;  //this.rowCount * this.circleDist/ dist_y;  //this.pixHeight * 0.001; // 相机的内参一定要用米制单位
                if (this.focus == 0) //StartCamParam[0].D
                {
                    StartCamParam[2] = this.circleRadius / circleRadius.TupleMean().D; // 用半径来计算像元尺寸 
                    StartCamParam[3] = this.circleRadius / circleRadius.TupleMean().D;
                    // 对于远心镜头，将其当做一个焦距很大的FA镜头来用
                    StartCamParam[0] = 100000000; // 如果相机为远心镜头，那么将焦距设置为很大的值，这样一来可以将远心镜头模拟为FA镜头，从而可以实现在远心摄像机模型下，使用Fa相机模型的算子
                    HOperatorSet.VectorToPose(Nx, Ny, Nz, Rows, Columns, StartCamParam, this.method, "error", out NStartPose, out Quality);
                    StartCamParam[0] = 0; // 计算完初始位姿后，将其复原
                    //NStartPose[2] = 200; // 计算完初始位姿后，将其复原
                }
                else  // 标定非远心模型
                {
                    // 像素大小的单位一定要与焦距的单位统一，统一用mm 作为单位;;经过这个算子计算后，会自动将计算位姿的XY轴与标定板的XY轴对齐
                    HOperatorSet.VectorToPose(Nx, Ny, Nz, Rows, Columns, StartCamParam, this.method, "error", out NStartPose, out Quality);
                }
                ////////////////// VectorToPose 算子不适用于远心相机模型 ，这一步执行后，将相机位姿标定到的标定板坐标系中，即相机位姿各轴平行于标定板坐标系
                HOperatorSet.CameraCalibration(Nx, Ny, Nz, Rows, Columns, StartCamParam, NStartPose, "all", out camParam, out NFinalPose, out Error);
                ///////////  因为校准是在指定平面上的测量，所以这里不能改变测量平面，即不能对三个轴做角度修正              
                HOperatorSet.PoseToHomMat3d(NFinalPose, out homMat3d);
                HMatrix hMatrix_x_std, hMatrix_y_std, hMatrix_x_cur, hMatrix_y_cur;
                HTuple X1, Y1, Nx1, Ny1, Nz1;
                switch (this.camCalibrateType)
                {
                    case enCamCalibrateType.单相机标定: // 单相机标定不需要考虑相机旋转角，因为在计算中已考虑了
                        HOperatorSet.HomMat3dRotateLocal(homMat3d, 0, "z", out homMat3dRotateZ); //  (Math.PI / 180) * NFinalPose[5] * -1  先旋转Z，再旋转Y，再旋转X  (Math.PI / 180) * NFinalPose[5] * -1
                        HOperatorSet.HomMat3dRotateLocal(homMat3dRotateZ, (Math.PI / 180) * NFinalPose[4].D * -1, "y", out homMat3dRotateY);
                        HOperatorSet.HomMat3dRotateLocal(homMat3dRotateY, (Math.PI / 180) * (180 - NFinalPose[3].D), "x", out homMat3dRotateX);
                        homMat3dRotateZ = homMat3dRotateX;
                        HOperatorSet.HomMat3dToPose(homMat3dRotateZ, out homPose);
                        HOperatorSet.SetOriginPose(homPose, this.x_offset, this.Y_offset, 0, out camPose); // 平移原点
                        ////////////////////////
                        HOperatorSet.ImagePointsToWorldPlane(camParam, camPose, Rows, Columns, 1, out X1, out Y1);
                        Nx1 = Nx + this.x_offset; // 单相机固定标定，由于测量坐标系与标定板坐标系重合，所以不需要再变换
                        Ny1 = Ny + this.Y_offset;
                        hMatrix_x_std = new HMatrix(this.rowCount, this.colCount, Nx1);    /// 
                        hMatrix_y_std = new HMatrix(this.rowCount, this.colCount, Ny1);
                        hMatrix_x_cur = new HMatrix(this.rowCount, this.colCount, X1);
                        hMatrix_y_cur = new HMatrix(this.rowCount, this.colCount, Y1);
                        //calibrateFile = new CalibrationXyPlane(hMatrix_x_std, hMatrix_x_cur, hMatrix_y_std, hMatrix_y_cur);
                        break;
                    case enCamCalibrateType.单相机移动标定: // 需将相机位姿与机台坐标系平齐，这样才能跟随移动
                        HOperatorSet.HomMat3dRotateLocal(homMat3d, (Math.PI / 180) * NFinalPose[5] * -1, "z", out homMat3dRotateZ); //    先旋转Z，再旋转Y，再旋转X  (Math.PI / 180) * NFinalPose[5] * -1
                        HOperatorSet.HomMat3dRotateLocal(homMat3dRotateZ, (Math.PI / 180) * NFinalPose[4].D * -1, "y", out homMat3dRotateY);
                        HOperatorSet.HomMat3dRotateLocal(homMat3dRotateY, (Math.PI / 180) * (180 - NFinalPose[3].D), "x", out homMat3dRotateX);
                        HOperatorSet.HomMat3dRotateLocal(homMat3dRotateX, (Math.PI / 180) * camSlant, "z", out homMat3dRotateZ); // (Math.PI / 180) * camSlant //位姿要平行于标定块的坐标系，所以要旋转一个相机角度
                        /////////////////////////////////////////////////////////////
                        HOperatorSet.HomMat3dToPose(homMat3dRotateZ, out homPose);
                        HOperatorSet.SetOriginPose(homPose, this.x_offset, this.y_offset, 0, out camPose); // 平移原点
                        HOperatorSet.ImagePointsToWorldPlane(camParam, camPose, Rows, Columns, 1, out X1, out Y1);
                        HTuple dist = HMisc.DistancePp(Y1[0].D,X1[0], Y1[2], X1[2]);
                        //// 将标准值变换到测量坐标系中，以便于补偿
                        HOperatorSet.HomMat3dIdentity(out homMat3d); // 创建的是右手坐标系，X向右，Y向上，Z向外
                        HOperatorSet.HomMat3dRotateLocal(homMat3d, (Math.PI / 180) * (NFinalPose[5].D - camSlant), "z", out homMat3dRotateZ);  //NFinalPose[5].D- 
                        HOperatorSet.HomMat3dToPose(homMat3dRotateZ, out NFinalPose);
                        HOperatorSet.AffineTransPoint3d(homMat3dRotateZ, Nx + this.x_offset, Ny + this.Y_offset,Nz,out Nx1,out Ny1,out Nz1); // 因为需要将标准值与计算值制作查找表，所以需将两者变换到同一坐标系中
                        dist = HMisc.DistancePp(Ny1[0].D, Nx1[0], Ny1[2], Nx1[2]); // 标准数值是在标定板坐标系中的，测量值是在测量坐标系中，所以，要么将测量值变换到标定板坐标系中，要么将标准值变换到测量坐标系中，这里是将标准值变换到测量坐标系中
                        hMatrix_x_std = new HMatrix(this.rowCount, this.colCount, Nx1);    /// 移动的话，需要测量坐标系与机台坐标系平行，建立一对一对应关系 
                        hMatrix_y_std = new HMatrix(this.rowCount, this.colCount, Ny1);
                        hMatrix_x_cur = new HMatrix(this.rowCount, this.colCount, X1);
                        hMatrix_y_cur = new HMatrix(this.rowCount, this.colCount, Y1);
                        //calibrateFile = new CalibrationXyPlane(hMatrix_x_std, hMatrix_x_cur, hMatrix_y_std, hMatrix_y_cur);
                        break;
                    case enCamCalibrateType.多相机标定:// 多相机在同一标定板中标定不需要考虑相机旋转角，因为在计算中已考虑了，让各自的坐标系平行于标定板坐标系即可，只适用于相机都在正面的情况 
                        HOperatorSet.HomMat3dRotateLocal(homMat3d, 0, "z", out homMat3dRotateZ); //  (Math.PI / 180) * NFinalPose[5] * -1  先旋转Z，再旋转Y，再旋转X  (Math.PI / 180) * NFinalPose[5] * -1
                        HOperatorSet.HomMat3dRotateLocal(homMat3dRotateZ, (Math.PI / 180) * NFinalPose[4].D * -1, "y", out homMat3dRotateY);
                        HOperatorSet.HomMat3dRotateLocal(homMat3dRotateY, (Math.PI / 180) * (180 - NFinalPose[3].D), "x", out homMat3dRotateX);
                        homMat3dRotateZ = homMat3dRotateX;
                        ////////////////////////
                        HOperatorSet.HomMat3dToPose(homMat3dRotateZ, out homPose);
                        HOperatorSet.SetOriginPose(homPose, this.x_offset, this.Y_offset, 0, out camPose); // 平移原点
                        ////////////////////////
                        HOperatorSet.ImagePointsToWorldPlane(camParam, camPose, Rows, Columns, 1, out X1, out Y1);
                        Nx1 = Nx + this.x_offset; // 多相机固定标定，由于测量坐标系与标定板坐标系重合，所以不需要再变换
                        Ny1 = Ny + this.Y_offset;
                        hMatrix_x_std = new HMatrix(this.rowCount, this.colCount, Nx1);    /// 
                        hMatrix_y_std = new HMatrix(this.rowCount, this.colCount, Ny1);
                        hMatrix_x_cur = new HMatrix(this.rowCount, this.colCount, X1);
                        hMatrix_y_cur = new HMatrix(this.rowCount, this.colCount, Y1);
                        //calibrateFile = new CalibrationXyPlane(hMatrix_x_std, hMatrix_x_cur, hMatrix_y_std, hMatrix_y_cur);
                        break;
                    default:
                        homMat3dRotateZ = homMat3d;
                        HOperatorSet.HomMat3dToPose(homMat3dRotateZ, out NFinalPose);
                        HOperatorSet.SetOriginPose(NFinalPose, this.x_offset, this.Y_offset, 0, out camPose); // 平移原点
                                                                                                              ////////////////////////
                        HOperatorSet.ImagePointsToWorldPlane(camParam, camPose, Rows, Columns, 1, out X1, out Y1);
                        Nx1 = Nx + this.x_offset;
                        Ny1 = Ny + this.Y_offset;
                        hMatrix_x_std = new HMatrix(this.rowCount, this.colCount, Nx1);    /// 
                        hMatrix_y_std = new HMatrix(this.rowCount, this.colCount, Ny1);
                        hMatrix_x_cur = new HMatrix(this.rowCount, this.colCount, X1);
                        hMatrix_y_cur = new HMatrix(this.rowCount, this.colCount, Y1);
                        // calibrateFile = new CalibrationXyPlane(hMatrix_x_std, hMatrix_x_cur, hMatrix_y_std, hMatrix_y_cur);
                        break;
                }
                return circle;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("标定算法出错" + ex.ToString());
                // return new HXLDCont();
                throw;
            }

        }

        public void ExtractCircleCenter(HImage image, double Threshold, out double row, out double col, out double radius)
        {
            row = 0;
            col = 0;
            radius = 0;
            if (image == null) return;
            HTuple Row, Column, Radius, StartPhi, EndPhi, PointOrder;
            if (image == null)
                throw new ArgumentNullException("image", "输入图像参数为NULL");
            if (Threshold < 0)
                throw new ArgumentNullException("Threshold", "输入阈值参数小于0");
            HXLDCont circle = image.ThresholdSubPix(Threshold).UnionCocircularContoursXld(0.5, 0.1, 0.2, 30, 10, 10, "true", 3).SelectShapeXld("circularity", "and", 0.7, 99999); // .SelectShapeXld("circularity", "and", 0.7, 99999)            
            if (circle.CountObj() == 0) return;
            HTuple length = circle.LengthXld();
            circle = circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.6, length.TupleMax().D * 1.1);
            ////////////////////////////
            circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.66, length.TupleMax().D * 1.5).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            if (Row != null && Row.Length > 0)
            {
                row = Row[Column.TupleSortIndex()[0].D].D;
                col = Column[Column.TupleSortIndex()[0].D].D;
                radius = Radius[Column.TupleSortIndex()[0].D].D;
            }
        }


        public HXLDCont CalibrateCamera(HImage image, out userCamParam camParam, out userCamPose camPose, out double Error)
        {
            camParam = new userCamParam();
            camPose = new userCamPose();
            Error = 10;
            HRegion region;
            HTuple width, height;
            HTuple Nx = new HTuple();
            HTuple Ny = new HTuple();
            HTuple Nz = new HTuple();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple circleRow, circleColumn, circleRadius, StartPhi, EndPhi, PointOrder, sortRow, sortCol, sortRow2, sortCol2;
            HalconLibrary ha = new HalconLibrary();
            ///////////////////////////////////////////////////////
            try
            {
                HOperatorSet.GetImageSize(image, out width, out height);
                if (width.I != this.imageWidth || height.I != this.imageHeight)
                {
                    this.ImageWidth = width.I; // 改变图像后一定要重设一次参数
                    this.ImageHeight = height.I;
                }
                // 计算像素位置点
                region = new HRegion();
                region.GenRectangle2(this.rect2.Row, this.rect2.Col, this.rect2.Rad, this.rect2.Length1, this.rect2.Length2);
                HXLDCont circle = image.ReduceDomain(region).ThresholdSubPix(this.threshold).UnionCocircularContoursXld(0.5, 0.1, 0.2, 30, 10, 10, "true", 3); //              
                HTuple length = circle.LengthXld();
                circle = circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.66, length.TupleMax().D * 1.05);
                int count = circle.CountObj();
                circle.FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out circleRow, out circleColumn, out circleRadius, out StartPhi, out EndPhi, out PointOrder);
                circle.GenCircleContourXld(circleRow, circleColumn, circleRadius, StartPhi, EndPhi, PointOrder, 0.001);
                ha.sortPairs(circleRow, circleColumn, 1, out sortRow, out sortCol);// 行排序
                if (this.rowCount * this.colCount != sortRow.Length)
                    return circle;
                for (int i = 0; i < this.rowCount; i++)
                {
                    // 列排序
                    ha.sortPairs(sortRow.TupleSelectRange(i * this.colCount, (i + 1) * this.colCount - 1), sortCol.TupleSelectRange(i * this.colCount, (i + 1) * this.colCount - 1), 2, out sortRow2, out sortCol2);
                    Rows.Append(sortRow2);
                    Columns.Append(sortCol2);
                }
                for (int i = 0; i < this.rowCount; i++)
                {
                    // 以左上角第一个点作为参考点
                    for (int j = 0; j < this.colCount; j++)
                    {
                        Nx.Append(j * this.circleDist); //this.x_offset +
                        Ny.Append(i * this.circleDist * -1); // this.y_offset -   以世界坐标系作为参考坐标系
                        Nz.Append(0);
                    }
                }
                ///////////////////////
                if (Rows.Length != Nx.Length) MessageBox.Show("理论点与图像点不一致");
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.VectorToHomMat2d(Columns, Rows, Nx, Ny);
                NinePointCalibParam.HHomMat2DToCamparaCamPos(hHomMat2D, width, height, this.magnification, out camParam, out camPose, out Error);

                return circle;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void CalibrateCamera(double []Rows,double [] Columns, double []X,double []Y, out userCamParam camParam, out userCamPose camPose, out double Error)
        {
            camParam = new userCamParam();
            camPose = new userCamPose();
            Error = 10;
            try
            {             
                ///////////////////////
                if (Rows.Length != X.Length) MessageBox.Show("理论点与图像点不一致");
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
                NinePointCalibParam.HHomMat2DToCamparaCamPos(hHomMat2D, this.imageWidth, this.imageHeight, this.magnification, out camParam, out camPose, out Error);
            }
            catch (Exception ex)
            {
                throw;
            }

        }




    }
}
