using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class PointCloudAcqParam
    {
        public LaserParam LaserParam { get; set; }
        public List<string> FilePath { get; set; }
        public string SingleFilePath { get; set; }
        public string FolderPath { get; set; }
        public bool IsLaserSource { get; set; }
        public bool IsFileSource { get; set; }
        public userWcsVector GrabPoint { get; set; }
        public bool SrartImageAcq { get; set; }
        public bool StopImageAcq { get; set; }
        public string ViewWindow { get; set; }
        public enCoordSysName CoordSysName { get; set; }
        public string AcqSourceName
        {
            get;
            set;
        }

        public PointCloudAcqParam()
        {
            this.IsLaserSource = true;
            this.IsFileSource = false;
            this.IsLaserSource = true;
            this.FilePath = new List<string>();
            this.SingleFilePath = "";
            this.FolderPath = "";
            this.GrabPoint = new userWcsVector(100, 100, 0, 0);
            this.SrartImageAcq = false;
            this.StopImageAcq = false;
            this.ViewWindow = "NONE";
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }

        public PointCloudData ReadPointCloud(string path, string acqSourceName)
        {
            if (File.Exists(path))
            {
                HTuple hTuple;
                HObjectModel3D hObjectModel3D = null;
                switch (new FileInfo(path).Extension)
                {
                    case ".txt":
                        List<double> x = new List<double>();
                        List<double> y = new List<double>();
                        List<double> z = new List<double>();
                        using (StreamReader sr = new StreamReader(path))
                        {
                            int count = 0;
                            while (true)
                            {
                                string content = sr.ReadLine();
                                if (content == null || content.Length == 0)
                                {
                                    count++;
                                    if (count > 50)
                                        break;
                                    else
                                    {
                                        count = 0;
                                        continue;
                                    }                                     
                                }
                                string[] value = content.Split(',', '\t', '\n', ';', ':');
                                if (value.Length > 0)
                                    x.Add(Convert.ToDouble(value[0]));
                                else
                                    x.Add(0);
                                //////////////////////////////////////
                                if (value.Length > 1)
                                    y.Add(Convert.ToDouble(value[1]));
                                else
                                    y.Add(0);
                                //////////////////////////////////////
                                if (value.Length > 2)
                                    z.Add(Convert.ToDouble(value[2]));
                                else
                                    z.Add(0);
                            }
                        }
                        hObjectModel3D = new HObjectModel3D(x.ToArray(), y.ToArray(), z.ToArray());
                        x.Clear();
                        y.Clear();
                        z.Clear();
                        break;
                    case ".om3":
                        hObjectModel3D = new HObjectModel3D(path, "m", "file_type", "om3", out hTuple);
                        break;
                    case ".dxf":
                        hObjectModel3D = new HObjectModel3D(path, "m", new HTuple(), new HTuple(), out hTuple);
                        break;
                    case ".off":
                        hObjectModel3D = new HObjectModel3D(path, "m", "file_type", "off", out hTuple);
                        break;
                    case ".obj":
                        hObjectModel3D = new HObjectModel3D(path, "m", "file_type", "obj", out hTuple);
                        break;
                    case ".ply":
                        hObjectModel3D = new HObjectModel3D(path, "m", "file_type", "ply", out hTuple);
                        break;
                    case ".stl":
                        hObjectModel3D = new HObjectModel3D(path, "m", "file_type", "stl", out hTuple);
                        break;
                    default:
                        hObjectModel3D = new HObjectModel3D(path, "m", new HTuple(), new HTuple(), out hTuple);
                        break;
                }
                string[] name = SensorManage.GetSensorName();
                foreach (var item in name)
                {
                    if (acqSourceName != null && acqSourceName.Contains(item))
                    {
                        this.LaserParam = SensorManage.GetSensor(item).LaserParam;
                    }
                } 
                if (this.LaserParam == null || this.LaserParam.SensorName == "NONE")
                    InitLaserCamPose(hObjectModel3D);
                return new PointCloudData(hObjectModel3D);
            }
            return null;
        }

        /// <summary>
        /// 这里读取的只可能是一个对象，因为保存时不可能多个对象同时保存
        /// </summary>
        /// <param name="Model3D"></param>
        private void InitLaserCamPose(HObjectModel3D Model3D)
        {
            if (Model3D == null)
            {
                this.LaserParam = new LaserParam();
                throw new ArgumentNullException("Model3D");
            }
            else
            {
                HTuple hTuple = Model3D.GetObjectModel3dParams("bounding_box1");
                double min_x = hTuple[0].D;
                double min_y = hTuple[1].D;
                double min_z = hTuple[2].D;
                double max_x = hTuple[3].D;
                double max_y = hTuple[4].D;
                double max_z = hTuple[5].D;
                ///////////////////////////////////
                double x_range = (max_x - min_x) < 1 ? 1 : max_x - min_x;
                double y_range = (max_y - min_y) < 1 ? 1 : max_y - min_y;
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
                HTuple Quality2;
                HTuple Qx = new HTuple(min_x, min_x, max_x, max_x);
                HTuple Qy = new HTuple(max_y, min_y, min_y, max_y);
                HTuple Rows = new HTuple(0, imageHeight, imageHeight, 0);
                HTuple Columns = new HTuple(0, 0, imageWidth, imageWidth);
                HPose pose = HImage.VectorToPose(Qx, Qy, new HTuple(), Rows, Columns, camParam.GetHCamPar(), "telecentric_planar_robust", "error", out Quality2);
                HTuple Quality = Quality2[0].D;
                if (this.LaserParam == null)
                    this.LaserParam = new LaserParam();
                this.LaserParam.CamPose = new userCamPose(pose);
                this.LaserParam.CamPose.Tz = 200;
                this.LaserParam.CamParam = camParam;
            }
        }

        private void InitLaserCamPose(HObjectModel3D [] Model3D)
        {
            if (Model3D == null)
            {
                this.LaserParam = new LaserParam();
                throw new ArgumentNullException("Model3D");
            }
            else
            {
                List<double> x_value = new List<double>();
                List<double> y_value = new List<double>();
                if (!Model3D[0].IsInitialized()) return;
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
                if (this.LaserParam == null)
                    this.LaserParam = new LaserParam();
                this.LaserParam.CamPose = new userCamPose(pose);
                this.LaserParam.CamPose.Tz = 200;
                this.LaserParam.CamParam = camParam;
            }
        }



    }

}
