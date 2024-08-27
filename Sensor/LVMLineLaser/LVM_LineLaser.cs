using Common;
using HalconDotNet;
using NvtLvmSdk;
using Sensor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static NvtLvmSdk.CameraModel;

namespace NvtLvmSdkDemo
{
    public class LVM_LineLaser:SensorBase, ISensor
    {
        private string configPath;
        private string ipAdress;
        private lvm_image_t imageData;//图像数据
        private lvm_depth_map_t depthMapData;//深度数据
        private lvm_point_cloud_t pointCloudData;//点云数据
        private HImage image;
        private CameraApi camera_api = new CameraApi();
        private grab_mode_t grabMode = grab_mode_t.POINT_CLOUD;
        private int scanNum = 100000; // 这里一定要给你个固定值，而且要足够大，可以容纳需要的最大轮廓数量
        //private uint numLinePer = 1920;
        /// /////////////////
        object monitor = new object();
        public bool Init()
        {
            //this.SensorParam.SensorType = enUserSensorType.线激光;
            this.LaserParam.DataWidth = 1920;
            return true;
        }

        /// <summary>
        /// 设置传感器参数
        /// </summary>
        /// <returns></returns>
        public bool SetParam(object paramType, object value)
        {
            camera_api.GrabStop();
            switch ((enSensorParamType)paramType)
            {
                case enSensorParamType.LVM_曝光:
                    this.camera_api.Config_param_T.expsure_time = Convert.ToInt32(value);
                    this.camera_api.SetConfigParam(ref camera_api.Config_param_T);
                    break;
                case enSensorParamType.LVM_数字增益:
                    this.camera_api.Config_param_T.gain_k = Convert.ToInt32(value);
                    this.camera_api.SetConfigParam(ref camera_api.Config_param_T);
                    break;
                case enSensorParamType.LVM_增益偏置:
                    this.camera_api.Config_param_T.gain_offset = Convert.ToInt32(value);
                    this.camera_api.SetConfigParam(ref camera_api.Config_param_T);
                    break;
                ////////
                case enSensorParamType.LVM_触发类型:
                    this.camera_api.Capture_param_T.trigger_input_type = (trigger_type_t)(value);
                    this.camera_api.SetTriggerParam(ref camera_api.Capture_param_T);
                    break;
                case enSensorParamType.LVM_触发频率:
                    this.camera_api.Capture_param_T.time_trigger_freq = Convert.ToSingle(value);
                    this.camera_api.SetTriggerParam(ref camera_api.Capture_param_T);
                    break;
                case enSensorParamType.LVM_触发分频:
                    this.camera_api.Capture_param_T.div_ratio = Convert.ToUInt32(value);
                    this.camera_api.SetTriggerParam(ref camera_api.Capture_param_T);
                    break;
                //////////////
                case enSensorParamType.LVM_采集类型:
                    this.grabMode = (grab_mode_t)value;
                    break;

                case enSensorParamType.LVM_激光亮度:
                    this.camera_api.Laser_param_T.laser_power = Convert.ToInt32(value);
                    this.camera_api.SetLaserParam(ref camera_api.Laser_param_T);
                    break;

                default:
                    break;
            }
            return true;
        }

        public object GetParam(object paramType)
        {
            switch ((enSensorParamType)paramType)
            {
                case enSensorParamType.LVM_曝光:
                    return this.camera_api.Config_param_T.expsure_time;

                case enSensorParamType.LVM_数字增益:
                    return this.camera_api.Config_param_T.gain_k;

                case enSensorParamType.LVM_增益偏置:
                    return this.camera_api.Config_param_T.gain_offset;

                ////////
                case enSensorParamType.LVM_触发类型:
                    return this.camera_api.Capture_param_T.trigger_input_type;

                case enSensorParamType.LVM_触发频率:
                    return this.camera_api.Capture_param_T.time_trigger_freq;

                case enSensorParamType.LVM_触发分频:
                    return this.camera_api.Capture_param_T.div_ratio;

                //////////////
                case enSensorParamType.LVM_采集类型:
                    return this.grabMode;

                case enSensorParamType.LVM_扫描行数:
                    return this.scanNum;

                case enSensorParamType.LVM_激光亮度:
                    return this.camera_api.Laser_param_T.laser_power;

                case enSensorParamType.Coom_每线点数:
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Coom_传感器类型:
                    return SensorConnectConfigParamManger.Instance.GetSensorConfigParam(this.Name);

                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// 连接控制器
        /// </summary>
        /// <returns></returns>
        public bool Connect(SensorConnectConfigParam configParam)
        {
            return false;
            bool result = false;
            this.ConfigParam = configParam;
            this.Name = configParam.SensorName;
            this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
            this.LaserParam.SensorName = configParam.SensorName;
            if (camera_api.Cam_Connect(configParam.ConnectAddress))
            {
                SetParam(enSensorParamType.LVM_采集类型, grab_mode_t.POINT_CLOUD);
                SetParam(enSensorParamType.LVM_曝光, 100);
                // r = camera_api.GrabStart(grabMode, scanNum, 1, frame_callback); // 高速模式使能；1：启用高速模式；0：禁用高速模式
                result = true;
            }
            configParam.ConnectState = result;
            return result;
        }

        /// <summary>
        /// 断开传感器连接
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            bool r = camera_api.Cam_Close();
            if (r)
            {
                //保存相机参数到本地
                string path = AppDomain.CurrentDomain.BaseDirectory + @"Data";
                string filename = @"Gen_Camera_para" + ".xml";
                string fullUrl = Path.Combine(path, filename);
                camera_api.SaveDevParam(fullUrl);
            }
            return r;
        }

        /// <summary>
        /// 触发传感器采集
        /// </summary>
        /// <returns></returns>
        public bool StartTrigger()
        {
            camera_api.GrabStart(grabMode, this.scanNum, 1, frame_callback);
            return camera_api.GrabSoftwareTigger(true);
        }

        /// <summary>
        /// 停止触发传感器采集
        /// </summary>
        /// <returns></returns>
        public bool StopTrigger()
        {
            camera_api.GrabSoftwareTigger(false);
            return camera_api.GrabStop();
        }

        /// <summary>
        /// 从控制器中读取采集数据     0:dist;/ 1:dist2; /2:thick;/ 3:intensity;/ 4:x;/ 5:y; /6:z;/ 7:baryCenter
        /// </summary>
        /// <returns></returns>
        public Dictionary<enDataItem, object> ReadData()
        {
            double[] x;
            double[] y;
            double[] dist;
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            //////////////////////////////////////
            int pointSize = Marshal.SizeOf(typeof(lvm_point_t));
            IntPtr src = this.pointCloudData.p;
            lvm_point_t p3d;
            this.LaserParam.DataWidth =(int)this.pointCloudData.head.width;
            x = new double[this.pointCloudData.head.height * this.pointCloudData.head.width]; // width::数据的宽度；height：表示数据的高度
            y = new double[this.pointCloudData.head.height * this.pointCloudData.head.width];
            dist = new double[this.pointCloudData.head.height * this.pointCloudData.head.width];
            /////////////////////////////////////
            // uint height = this.pointCloudData.head.height;
            for (int i = 0; i < this.pointCloudData.head.height * this.pointCloudData.head.width; i++)
            {
                p3d = Marshal.PtrToStructure<lvm_point_t>(src + i * pointSize);
                if (!(!float.IsNaN(p3d.x) && !float.IsNaN(p3d.y) && !float.IsNaN(p3d.z)))
                {
                    x[i] = 0; // 如果不在量程范围内，则为无效数据
                    y[i] = 0;
                    dist[i] = 0;
                }
                else
                {
                    x[i] = p3d.x;
                    y[i] = p3d.y;
                    if(this.LaserParam.IsMirrorZ)
                        dist[i] = p3d.z * -1;
                    else
                        dist[i] = p3d.z;
                }
            }
            list.Add(enDataItem.Dist1,dist);
            list.Add(enDataItem.X, x);
            list.Add(enDataItem.Y, y);

            return list;
            // 
        }

        //相机数据采集回调
        private int frame_callback(IntPtr dev, IntPtr frame, IntPtr data, int lines)
        {
            if (data != IntPtr.Zero)
            {
                switch (grabMode)
                {
                    case grab_mode_t.IMAGE:
                        imageData = Marshal.PtrToStructure<lvm_image_t>(frame);
                        this.image = new HImage("uint2", Convert.ToInt32(imageData.head.width), Convert.ToInt32(imageData.head.height), imageData.data);
                        break;
                    case grab_mode_t.POINT_CLOUD:
                        pointCloudData = Marshal.PtrToStructure<lvm_point_cloud_t>(frame);
                        break;
                    case grab_mode_t.DEPTH_MAP:
                        depthMapData = Marshal.PtrToStructure<lvm_depth_map_t>(frame);
                        this.image = new HImage("uint2", Convert.ToInt32(imageData.head.width), Convert.ToInt32(imageData.head.height), imageData.data);
                        break;
                    default:
                        return -1;
                }
                return 0;
                //////////////////////////////////////////////              
            }
            return -1;

        }


    }





}
