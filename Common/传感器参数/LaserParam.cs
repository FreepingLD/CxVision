using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class LaserParam : SensorParam
    {
        public double Resolution_X { set; get; }
        public double Resolution_Y { set; get; }
        public double Resolution_Z { set; get; }

        [Browsable(false)]
        public userLaserCalibrateParam LaserCalibrationParam { set; get; }
        public double MeasureRange { set; get; }

        /// <summary>
        /// 扫描步长
        /// </summary>
        public double ScanStep { set; get; }

        public enDeviceMode DeviceMode { set; get; }

        /// <summary>
        /// 扫描轴
        /// </summary>
        public enScanAxis ScanAxis { set; get; }
        public bool Enable_x { set; get; }
        public bool Enable_y { set; get; }
        public bool Enable_z { set; get; }

        /// <summary> 相机标定矩阵 </summary>
        public UserHomMat3D HomMat3D { get; set; }

        /// <summary> 相对位姿标定矩阵 </summary>
        public UserHomMat3D MapHomMat3D { get; set; }


        [Browsable(false)]
        public NinePointCalibParam CaliParam { get; set; }
        public int AcqCount { set; get; }
        public LaserParam()
        {
            this.SensorName = "NONE";
            this.CaliParam = new NinePointCalibParam();
            this.DataWidth = 1;
            this.DataHeight = 1;
            this.Resolution_X = 1;
            this.Resolution_Y = 1;
            this.Resolution_Z = 1;
            this.LaserCalibrationParam = new userLaserCalibrateParam();
            this.MeasureRange = 0;
            this.ScanStep = 0.01;
            this.ScanAxis = enScanAxis.Y轴;
            this.Enable_x = false;
            this.Enable_y = false;
            this.Enable_z = false;
            this.AcqCount = 1;
            this.HomMat3D = new UserHomMat3D();
            this.MapHomMat3D = new UserHomMat3D();
            this.DeviceMode = enDeviceMode.主设备;
        }
        public LaserParam(string laserName)
        {
            this.SensorName = laserName;
            this.HomMat3D = new UserHomMat3D();
            this.MapHomMat3D = new UserHomMat3D();
            this.CaliParam = new NinePointCalibParam(laserName);
            this.DataWidth = 1;
            this.DataHeight = 1;
            this.Resolution_X = 1;
            this.Resolution_Y = 1;
            this.Resolution_Z = 1;
            this.LaserCalibrationParam = new userLaserCalibrateParam();
            this.MeasureRange = 0;
            this.ScanStep = 0.01;
            this.ScanAxis = enScanAxis.Y轴;
            this.Enable_x = false;
            this.Enable_y = false;
            this.Enable_z = false;
            this.AcqCount = 1;
            this.DeviceMode = enDeviceMode.主设备;
        }

        public void ImagePointsToWorldPlane(HTuple Rows, HTuple Coluns, double grabImage_x, double grabImage_y, double grabImage_z, out HTuple wcs_x, out HTuple wcs_y, out HTuple wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
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
            if (Rows.Length == 0 || Coluns.Length == 0)
            {
                throw new ArgumentException("参Rows或Coluns长度等于0");
            }
            /////////////////////////////////////////////////////////
            HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            HTuple temp_x = new HTuple(wcs_x);
            HTuple temp_y = new HTuple(wcs_y);
            wcs_x = this.MapHomMat3D.GetHHomMat3D().AffineTransPoint3d(Qx, Qy, Qz, out wcs_y, out wcs_z);
        }
        public void ImagePointsToWorldPlane(double Rows, double Coluns, double grabImage_x, double grabImage_y, double grabImage_z, out double wcs_x, out double wcs_y, out double wcs_z)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            wcs_x = 0;
            wcs_y = 0;
            wcs_z = 0;
            /////////////////////////////////////////////////////////  这个标定中心在这里没什么用了，应该使用旋转的拍照位姿 ////////////////
            HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), Rows, Coluns, 1, out Qx, out Qy);
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            wcs_x = this.MapHomMat3D.GetHHomMat3D().AffineTransPoint3d(Qx, Qy, Qz, out wcs_y, out wcs_z);
        }

        public void WorldPointsToImagePlane(HTuple wcs_x, HTuple wcs_y, HTuple wcs_z, out HTuple Rows, out HTuple Coluns)
        {
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
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
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            if (this.MapHomMat3D == null || this.MapHomMat3D.c00 == 0 || this.MapHomMat3D.c11 == 0)
                this.MapHomMat3D = new UserHomMat3D(true);
            Qx = this.MapHomMat3D.GetHHomMat3D().HomMat3dInvert().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out Qy, out Qz);
            ////////////////////////////////////////
            // 世界点到图像点
            HTuple ProjMat, Cam_x, Cam_y, Cam_z;
            HOperatorSet.TupleGenConst(Qx.Length, 0, out Qz);
            if (this.CamParam.Kappa == 0)
            {
                HOperatorSet.CamParPoseToHomMat3d(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), out ProjMat);
                HOperatorSet.ProjectPointHomMat3d(ProjMat, Qx, Qy, Qz, out Coluns, out Rows);
            }
            else
            {
                HOperatorSet.PoseToHomMat3d(this.CamPose.GetHtuple(), out ProjMat);
                HOperatorSet.AffineTransPoint3d(ProjMat, Qx, Qy, Qz, out Cam_x, out Cam_y, out Cam_z);
                HOperatorSet.Project3dPoint(Cam_x, Cam_y, Cam_z, this.CamParam.GetHtuple(), out Rows, out Coluns);
            }
        }
        public void WorldPointsToImagePlane(double wcs_x, double wcs_y, double wcs_z, out double Rows, out double Coluns)
        {
            double Qx, Qy, Qz;
            Rows = 0;
            Coluns = 0;
            // 将相机坐标从一个坐标系转换到另一个坐标系，在使用了相对位姿标定后才有效
            if (this.MapHomMat3D == null || this.MapHomMat3D.c00 == 0 || this.MapHomMat3D.c11 == 0)
                this.MapHomMat3D = new UserHomMat3D(true);
            Qx = this.MapHomMat3D.GetHHomMat3D().HomMat3dInvert().AffineTransPoint3d(wcs_x, wcs_y, wcs_z, out Qy, out Qz);
            // 世界点到图像点
            HTuple ProjMat, Cam_x, Cam_y, Cam_z, row = 0, col = 0;
            if (this.CamParam != null && this.CamPose != null)
            {
                if (this.CamParam?.Kappa == 0)
                {
                    HOperatorSet.CamParPoseToHomMat3d(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), out ProjMat);
                    HOperatorSet.ProjectPointHomMat3d(ProjMat, Qx, Qy, 0, out col, out row);
                }
                else
                {
                    HOperatorSet.PoseToHomMat3d(this.CamPose.GetHtuple(), out ProjMat);
                    HOperatorSet.AffineTransPoint3d(ProjMat, Qx, Qy, 0, out Cam_x, out Cam_y, out Cam_z);
                    HOperatorSet.Project3dPoint(Cam_x, Cam_y, Cam_z, this.CamParam.GetHtuple(), out row, out col);
                }
            }
            Rows = row.D;
            Coluns = col.D;
        }
        public double TransPixLengthToWcsLength(double pixLength)
        {
            HTuple Qx, Qy, Qz;
            HOperatorSet.ImagePointsToWorldPlane(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), new HTuple(0, 0), new HTuple(0, pixLength), 1, out Qx, out Qy);
            double length = HMisc.DistancePp(Qy[0].D, Qx[0].D, Qy[1].D, Qx[1].D);
            return length;
        }
        public double TransWcsLengthToPixLength(double wcsLength)
        {
            HTuple ProjMat = null;
            HTuple Rows = null;
            HTuple Columns = null;
            HTuple Qx, Qy, Qz, pix_Length;
            /////////////////////////////////////
            if (this.CamParam.Kappa == 0)
            {
                HOperatorSet.CamParPoseToHomMat3d(this.CamParam.GetHtuple(), this.CamPose.GetHtuple(), out ProjMat);
                HOperatorSet.ProjectPointHomMat3d(ProjMat, new HTuple(0.0, 0.0), new HTuple(0.0, wcsLength), new HTuple(0.0, 0.0), out Columns, out Rows);
            }
            else
            {
                HOperatorSet.PoseToHomMat3d(this.CamPose.GetHtuple(), out ProjMat);
                HOperatorSet.AffineTransPoint3d(ProjMat, new HTuple(0.0, 0.0), new HTuple(0.0, wcsLength), new HTuple(0.0, 0.0), out Qx, out Qy, out Qz);
                HOperatorSet.Project3dPoint(Qx, Qy, Qz, this.CamParam.GetHtuple(), out Rows, out Columns);
            }
            HOperatorSet.DistancePp(Rows[0], Columns[0], Rows[1], Columns[1], out pix_Length);
            ///////////////////////////////////////
            switch (pix_Length.Type)
            {
                case HTupleType.DOUBLE:
                    return pix_Length.D;
                case HTupleType.INTEGER:
                    return Convert.ToDouble(pix_Length.I);
                case HTupleType.LONG:
                    return Convert.ToDouble(pix_Length.L);
                case HTupleType.MIXED:
                    return Convert.ToDouble(pix_Length.O);
                case HTupleType.EMPTY:
                case HTupleType.STRING:
                default:
                    return 0;
            }
        }

        public virtual bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(SavePath)) DirectoryEx.Create(SavePath);
            IsOk = IsOk && XML<LaserParam>.Save(this, SavePath + @"\" + this.SensorName + ".xml");
            return IsOk;
        }
        public virtual object Read()
        {
            LaserParam laserParam = null;
            if (File.Exists(SavePath + @"\" + this.SensorName + ".xml"))
                laserParam = XML<LaserParam>.Read(SavePath + @"\" + this.SensorName + ".xml");
            else
                laserParam = new LaserParam();
            return laserParam;
        }
        public virtual object Read(string sensorName)
        {
            LaserParam laserParam = null;
            if (File.Exists(SavePath + @"\" + sensorName + ".xml"))
                laserParam = XML<LaserParam>.Read(SavePath + @"\" + sensorName + ".xml");
            else
                laserParam = new LaserParam(sensorName);
            if (laserParam == null)
                laserParam = new LaserParam(sensorName);
            return laserParam;
        }




    }
}
