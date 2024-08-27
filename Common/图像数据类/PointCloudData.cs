
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Runtime.InteropServices;
using Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Common
{
    [Serializable]
    /// <summary>
    /// 封装点云数据
    /// </summary>
    public class PointCloudData
    {
        private double grab_x = 100; // 当前采集图像的机台坐标
        private double grab_y = 100;
        private double grab_theta;
        private double grab_z;
        private double grab_u;
        private double grab_v;
        private LaserParam laserParams;
        private HObjectModel3D[] _ObjectModel3D;
        private object _Tag = 1;

        public HObjectModel3D[] ObjectModel3D
        {
            get
            {
                return _ObjectModel3D;
            }
            set
            {
                _ObjectModel3D = value;
            }
        }
        public double Grab_X
        {
            get
            {
                return grab_x;
            }

            set
            {
                grab_x = value;
            }
        }
        public double Grab_Y
        {
            get
            {
                return grab_y;
            }

            set
            {
                grab_y = value;
            }
        }
        public double Grab_Z
        {
            get
            {
                return grab_z;
            }

            set
            {
                grab_z = value;
            }
        }
        public double Grab_Theta { get => grab_theta; set => grab_theta = value; }
        public double Grab_U { get => grab_u; set => grab_u = value; }
        public double Grab_V { get => grab_v; set => grab_v = value; }
        public LaserParam LaserParams { get => laserParams; set => laserParams = value; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string SensorName { get; set; }
        public string ViewWindow { get; set; }
        public object Tag { get => this._Tag; set => _Tag = value; }


        public PointCloudData()
        {
            this.LaserParams = new LaserParam();
        }
        public PointCloudData(HObjectModel3D pointCloud)
        {
            this._ObjectModel3D = new HObjectModel3D[] { pointCloud };
            this.InitLaserCamPose(this.ObjectModel3D);
        }
        public PointCloudData(object pointCloud)
        {
            if (pointCloud != null && pointCloud is HObjectModel3D)
                this._ObjectModel3D = new HObjectModel3D[] { (HObjectModel3D)pointCloud }; ;
            this.InitLaserCamPose(this.ObjectModel3D);
        }
        public PointCloudData(object[] pointCloud)
        {
            if (pointCloud != null)
            {
                List<HObjectModel3D> list = new List<HObjectModel3D>();
                foreach (var item in pointCloud)
                {
                    list.Add((HObjectModel3D)item);
                }
                this._ObjectModel3D = list.ToArray();
            }
            this.InitLaserCamPose(this.ObjectModel3D);
        }
        public PointCloudData(HObjectModel3D[] pointCloud)
        {
            this._ObjectModel3D = pointCloud;
            this.InitLaserCamPose(this.ObjectModel3D);
        }
        public PointCloudData(HObjectModel3D pointCloud, int dataWidth, int dataHeight)
        {
            this._ObjectModel3D = new HObjectModel3D[] { pointCloud };
            this.Width = dataWidth;
            this.Height = dataHeight;
            this.InitLaserCamPose(this.ObjectModel3D);
        }


        public HObjectModel3D UnionObjectModel3d()
        {
            HObjectModel3D hObjectModel3D = null;
            if (this.ObjectModel3D != null)
            {
                hObjectModel3D = HObjectModel3D.UnionObjectModel3d(this.ObjectModel3D, "points_surface");
            }
            return hObjectModel3D;
        }
        public override string ToString()
        {
            return string.Format("宽度：{0},高度：{1}", this.Width, this.Height);
        }

        public void Add(HObjectModel3D hObjectModel3D)
        {
            List<HObjectModel3D> list = new List<HObjectModel3D>();
            if (this.ObjectModel3D != null && this.ObjectModel3D.Length > 0)
                list.AddRange(this.ObjectModel3D);
            list.Add(hObjectModel3D);
            this.ObjectModel3D = list.ToArray();
            list.Clear();
            this.InitLaserCamPose(this.ObjectModel3D);
        }
        public void Add(HObjectModel3D [] hObjectModel3D)
        {
            List<HObjectModel3D> list = new List<HObjectModel3D>();
            if (this.ObjectModel3D != null && this.ObjectModel3D.Length > 0)
                list.AddRange(this.ObjectModel3D);
            list.AddRange(hObjectModel3D);
            this.ObjectModel3D = list.ToArray();
            list.Clear();
            this.InitLaserCamPose(this.ObjectModel3D);
        }
        public bool IsInitialized()
        {
            bool result = true;
            if (this._ObjectModel3D != null)
            {
                foreach (var item in this._ObjectModel3D)
                {
                    if (item.IsInitialized())
                    {
                        result = false;
                        return result;
                    }
                }
            }
            return result;
        }
        public PointCloudData Clone()
        {
            PointCloudData pointData = new PointCloudData();
            try
            {
                object oo = null;
                BinaryFormatter binFormat = new BinaryFormatter(); //
                using (MemoryStream fStream = new MemoryStream())
                {
                    binFormat.Serialize(fStream, this);
                    oo = binFormat.Deserialize(fStream);
                }
                pointData = oo as PointCloudData;
            }
            catch (Exception ex)
            {

            }
            //if (this == null) return null;
            //PointCloudData pointData = new PointCloudData();
            //pointData._ObjectModel3D = this._ObjectModel3D?.Clone();
            //pointData.grab_x = this.grab_x;
            //pointData.grab_y = this.grab_y;
            //pointData.grab_z = this.grab_z;
            //pointData.Width = this.Width;
            //pointData.Height = this.Height;
            //pointData.laserParams = this.laserParams;
            //pointData.ViewWindow = this.ViewWindow;
            return pointData;
        }

        public int Count()
        {
            int num = 0;
            if (this._ObjectModel3D != null)
            {
                foreach (var item in this._ObjectModel3D)
                {
                    if (item != null && item.IsInitialized())
                    {
                        HTuple hTuple = item.GetObjectModel3dParams("num_points");
                        num += hTuple.I;
                    }

                }
            }
            return num;
        }
        public void Dispose()
        {
            if (this._ObjectModel3D != null)
            {
                foreach (var item in this._ObjectModel3D)
                {
                    item?.ClearObjectModel3d();
                    item?.Dispose();
                }
            }
        }
        public void ClearObjectModel3d()
        {
            if (this._ObjectModel3D != null)
            {
                HObjectModel3D.ClearObjectModel3d(this._ObjectModel3D);
            }
        }

        public HTuple GetObjectModel3dParams(string paramName = "point_coord_x")
        {
            HTuple hTuple = new HTuple();
            if (this.ObjectModel3D != null)
            {
                hTuple = HObjectModel3D.GetObjectModel3dParams(this.ObjectModel3D, paramName);
            }
            return hTuple;
        }

        public double GetHeightValueOnMouse(double wcs_x, double wcs_y, double tolerance = 0.5)
        {
            double value = 0;
            if (this.ObjectModel3D != null)
            {
                HObjectModel3D[] model3Ds = HObjectModel3D.SelectPointsObjectModel3d(this.ObjectModel3D, new HTuple("point_coord_x", "point_coord_y"), new HTuple(wcs_x - tolerance, wcs_y - tolerance), new HTuple(wcs_x + tolerance, wcs_y + tolerance));
                HTuple count = HObjectModel3D.GetObjectModel3dParams(model3Ds, "num_points");
                if (count.D > 0)
                {
                    double initDist = double.MaxValue;
                    int minIndex = 0;
                    HTuple hTuple1_x = HObjectModel3D.GetObjectModel3dParams(model3Ds, "point_coord_x");
                    HTuple hTuple1_y = HObjectModel3D.GetObjectModel3dParams(model3Ds, "point_coord_y");
                    HTuple hTuple1_z = HObjectModel3D.GetObjectModel3dParams(model3Ds, "point_coord_z");
                    for (int i = 0; i < hTuple1_x.Length; i++)
                    {
                        double dist = HMisc.DistancePp(hTuple1_x[i].D, hTuple1_y[i].D, wcs_x, wcs_y);
                        if (dist <= initDist)
                        {
                            initDist = dist;
                            minIndex = i;
                        }
                    }
                    value = hTuple1_z[minIndex].D;
                    //value = hTuple1_z.TupleMean().D;
                }
                HObjectModel3D.ClearObjectModel3d(model3Ds);
            }
            return value;
        }


        public void InitLaserCamPose(HObjectModel3D[] Model3D)
        {
            if (Model3D == null)
            {
                this.LaserParams = new LaserParam();
                throw new ArgumentNullException("Model3D");
            }
            else
            {
                List<double> x_value = new List<double>();
                List<double> y_value = new List<double>();
                if (Model3D[0] == null || !Model3D[0].IsInitialized()) return;
                HTuple num = Model3D[0].GetObjectModel3dParams("num_points");
                HTuple primitive = Model3D[0].GetObjectModel3dParams("has_primitive_data");
                if (num.I == 0 && primitive.S == "false") return; //  表示没有点也不是基本体则返回
                HTuple ParamValue = HObjectModel3D.GetObjectModel3dParams(Model3D, "bounding_box1"); // 如果是多个对象，获取的是每个对象的值
                if (ParamValue == null || ParamValue.Length == 0 || ParamValue.Length != Model3D.Length * 6) return;
                for (int i = 0; i < Model3D.Length; i++)
                {
                    x_value.Add(ParamValue[i * 6].D);
                    x_value.Add(ParamValue[i * 6 + 3].D);
                    y_value.Add(ParamValue[i * 6 + 1].D);
                    y_value.Add(ParamValue[i * 6 + 4].D);
                }
                double x_range = (x_value.Max() - x_value.Min());
                double y_range = (y_value.Max() - y_value.Min());
                double diameter = Math.Sqrt(x_range * x_range + y_range * y_range);
                /// 生成相机位姿
                userCamParam camParam = new userCamParam();
                if (diameter < 20)
                    camParam.Focus = 1;
                else
                    camParam.Focus = 0.5;
                double imageWidth = x_range * camParam.Focus / 0.005; // 这里为啥需要乘以 0.5 ？ 因为镜头的倍率设置为 0.5 倍
                double imageHeight = y_range * camParam.Focus / 0.005;
                camParam.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                //camParam.Focus = 0.5; // 这个值应该为镜头的倍率
                camParam.Sx = 0.005;
                camParam.Sy = 0.005;
                camParam.Cx = imageWidth * 0.5;
                camParam.Cy = imageHeight * 0.5;
                camParam.Width = (int)imageWidth;
                camParam.Height = (int)imageHeight;
                // 利用图像4个角点的世界坐标来计算位姿
                double min_x = x_value.Min();
                double max_x = x_value.Max();
                double min_y = y_value.Min();
                double max_y = y_value.Max();
                HTuple Quality2;
                HTuple Qx = new HTuple(min_x, min_x, max_x, max_x);
                HTuple Qy = new HTuple(max_y, min_y, min_y, max_y);
                HTuple Rows = new HTuple(0, imageHeight, imageHeight, 0);
                HTuple Columns = new HTuple(0, 0, imageWidth, imageWidth);
                HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, camParam.GetHCamPar(), "telecentric_planar_robust", "error", out Quality2);
                HTuple Quality = Quality2[0].D;
                if (this.LaserParams == null)
                    this.LaserParams = new LaserParam();
                this.LaserParams.CamPose = new userCamPose(pose);
                this.LaserParams.CamPose.Tz = 200;
                this.LaserParams.CamParam = camParam;
            }
        }

        public void InitLaserCamPose()
        {
            if (this.ObjectModel3D == null)
            {
                this.LaserParams = new LaserParam();
                throw new ArgumentNullException("Model3D");
            }
            else
            {
                List<double> x_value = new List<double>();
                List<double> y_value = new List<double>();
                if (this.ObjectModel3D[0] == null || !this.ObjectModel3D[0].IsInitialized()) return;
                HTuple num = this.ObjectModel3D[0].GetObjectModel3dParams("num_points");
                HTuple primitive = this.ObjectModel3D[0].GetObjectModel3dParams("has_primitive_data");
                if (num.I == 0 && primitive.S == "false") return; //  表示没有点也不是基本体则返回
                HTuple ParamValue = HObjectModel3D.GetObjectModel3dParams(this.ObjectModel3D, "bounding_box1"); // 如果是多个对象，获取的是每个对象的值
                if (ParamValue == null || ParamValue.Length == 0 || ParamValue.Length != this.ObjectModel3D.Length * 6) return;
                for (int i = 0; i < this.ObjectModel3D.Length; i++)
                {
                    x_value.Add(ParamValue[i * 6].D);
                    x_value.Add(ParamValue[i * 6 + 3].D);
                    y_value.Add(ParamValue[i * 6 + 1].D);
                    y_value.Add(ParamValue[i * 6 + 4].D);
                }
                double x_range = (x_value.Max() - x_value.Min());
                double y_range = (y_value.Max() - y_value.Min());
                double diameter = Math.Sqrt(x_range * x_range + y_range * y_range);
                /// 生成相机位姿
                userCamParam camParam = new userCamParam();
                if (diameter < 20)
                    camParam.Focus = 1;
                else
                    camParam.Focus = 0.5;
                double imageWidth = x_range * camParam.Focus / 0.005; // 这里为啥需要乘以 0.5 ？ 因为镜头的倍率设置为 0.5 倍
                double imageHeight = y_range * camParam.Focus / 0.005;
                camParam.CameraModel = enCameraModel.area_scan_telecentric_division.ToString();
                //camParam.Focus = 0.5; // 这个值应该为镜头的倍率
                camParam.Sx = 0.005;
                camParam.Sy = 0.005;
                camParam.Cx = imageWidth * 0.5;
                camParam.Cy = imageHeight * 0.5;
                camParam.Width = (int)imageWidth;
                camParam.Height = (int)imageHeight;
                // 利用图像4个角点的世界坐标来计算位姿
                double min_x = x_value.Min();
                double max_x = x_value.Max();
                double min_y = y_value.Min();
                double max_y = y_value.Max();
                HTuple Quality2;
                HTuple Qx = new HTuple(min_x, min_x, max_x, max_x);
                HTuple Qy = new HTuple(max_y, min_y, min_y, max_y);
                HTuple Rows = new HTuple(0, imageHeight, imageHeight, 0);
                HTuple Columns = new HTuple(0, 0, imageWidth, imageWidth);
                HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, camParam.GetHCamPar(), "telecentric_planar_robust", "error", out Quality2);
                HTuple Quality = Quality2[0].D;
                if (this.LaserParams == null)
                    this.LaserParams = new LaserParam();
                this.LaserParams.CamPose = new userCamPose(pose);
                this.LaserParams.CamPose.Tz = 200;
                this.LaserParams.CamParam = camParam;
            }
        }


    }

}
