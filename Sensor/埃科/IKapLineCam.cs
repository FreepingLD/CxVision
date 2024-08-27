using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basler.Pylon;
using Common;
using HalconDotNet;
using IKapC.NET;

namespace Sensor
{
    public class IKapLineCam : SensorBase, ISensor
    {
        //private HImage _grabImage;
        //private HImage _grabDarkImage;
        private LineCamera _camera;
        private int _CurrentImageIndex = 0;
        public bool Connect(SensorConnectConfigParam configParam) // 传入的名称
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.CameraParam = (CameraParam)new CameraParam().Read(configParam.SensorName);
                this.CameraParam.SensorName = configParam.SensorName;
                this._camera.CamParam = this.CameraParam;
                if (!configParam.IsActive) return result;
                ///////////////////////////////////
                switch (configParam.ConnectType)
                {
                    /// 海康面阵相机接口
                    case enUserConnectType.DeviceName:
                    case enUserConnectType.SerialNumber:
                        this._camera.ImageAcqMethod = configParam.ImageAcqMethod;
                        result = this._camera.Open(configParam.SensorName);
                        this._camera.LoadConfigFile(Application.StartupPath + "\\IKap\\" + this.CameraParam.SensorName + ".ccf");
                        this._camera.CreateStreamAndBuffer();
                        ////////////////////////////////     
                        switch (this.CameraParam.AcqMode)
                        {
                            case enAcqMode.同步采集:
                                this._camera.ConfigureStreamAsyn(); // 配置同步采集流，  线扫相机，不管是同步采集还是异步采集，都设置为 异步方式 
                                break;
                            case enAcqMode.异步采集:
                                this._camera.ConfigureStreamAsyn(); // 配置异步采集流
                                break;
                        }
                        switch (this.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.NONE:
                                // 设置触发模式
                                //this._camera.SetCamPara(false);
                                this._camera.StopGrab(); // 停止采集
                                break;
                            case enUserTriggerSource.编码器触发:
                            case enUserTriggerSource.外部IO触发:
                            case enUserTriggerSource.软触发:
                                break;
                            default:
                                break;
                        }
                        break;
                    case enUserConnectType.Map:
                        this._MapName = configParam.ConnectAddress;
                        if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                            SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                            result = true;
                        else
                            result = false;
                        break;
                    /// halcon接口适用于所有支持 Gige 接口的相机 
                    default:
                        MessageBox.Show(configParam.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.SerialNumber + "或" + enUserConnectType.DeviceName.ToString());
                        break;
                }
            }
            catch (HalconException ex)
            {
                LoggerHelper.Error(this.CameraParam.SensorName + "打开相机失败", ex);
                result = false;
            }
            configParam.ConnectState = result;  // 连接状态
            return result;
        }

        public bool Disconnect()
        {
            bool result = false;
            try
            {
                this.cts?.Cancel();
                if (this._camera != null)
                {
                    result = _camera.Close();
                }
                if (result)
                    LoggerHelper.Info(this.CameraParam.SensorName + "关闭相机成功");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.CameraParam.SensorName + "关闭相机失败", ex);
                result = false;
            }
            return result;
        }

        public bool Init()
        {
            bool result = true;
            try
            {
                this._camera = new LineCamera();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            switch (this.CameraParam.AcqMode)
            {
                case enAcqMode.异步取图:
                    this.GetImageAsyn(out this._grabImage, out _grabDarkImage);
                    break;
            }
            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage, this.CameraParam, this._CurrentImageIndex)); //this.imageData
            list.Add(enDataItem.DarkImage, new ImageDataClass(this._grabDarkImage, this.CameraParam, this._CurrentImageIndex)); //this.imageData _grabDarkImage
            return list;
        }

        public object GetParam(object paramType)
        {
            object value = "";
            switch (paramType.ToString())
            {
                case "曝光":
                    value = this._camera.GetExpos();
                    break;
                case "增益":
                    value = this._camera.GetGain();
                    break;
            }
            return value;
        }

        public bool SetParam(object paramType, object value)
        {
            bool result = false;
            try
            {
                switch (paramType.ToString())
                {
                    case "曝光":
                        int expose = 0;
                        int.TryParse(value.ToString(), out expose);
                        this._camera.SetExpos(expose);
                        break;
                    case "增益":
                        expose = 0;
                        int.TryParse(value.ToString(), out expose);
                        this._camera.SetGain(expose);
                        break;
                    case "清空数据":
                    case "重置图像采集":
                    case "启动采集":
                        this._CurrentImageIndex = 0;
                        result = this._camera.StopGrab();
                        this._camera.ClearData();
                        result = this._camera.StartGrab();
                        break;
                    case "停止采集":
                        result = this._camera.StopGrab();
                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Info(this.CameraParam.SensorName + "设置参数出错!!");
            }
            return result;
        }

        public bool StartTrigger()
        {
            bool result = false;
            this._CurrentImageIndex = 0;
            //lock (this._lockState)   //锁加在这里比较好
            //{
                switch (this.ConfigParam.ConnectType)
                {
                    case enUserConnectType.Map:
                        SensorManage.GetSensor(this._MapName).StartTrigger();
                        if (this._grabImage != null && this._grabImage.IsInitialized())
                            this._grabImage.Dispose();
                        this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; // 用于远程调用的映射处理
                        break;
                    default:
                        switch (this.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.NONE: // 实时采集
                            case enUserTriggerSource.外部IO触发:
                            case enUserTriggerSource.内部IO触发:
                            case enUserTriggerSource.编码器触发:
                                if (this._grabImage != null && this._grabImage.IsInitialized())
                                    this._grabImage.Dispose();
                                switch (this.CameraParam.AcqMode)
                                {
                                    case enAcqMode.同步采集: // 同步触发、同步采集、同步停止 
                                        this._camera.ClearData();
                                        this._camera.StartGrab();
                                        result = this.GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                        this._camera.StopGrab();
                                        break;
                                    case enAcqMode.异步取图: // 异步触发、同步取图、异步停止 
                                        this._CurrentImageIndex = 0;
                                        result = this._camera.StopGrab(); //停止 
                                        this._camera.ClearData(); // 清空
                                        result = this._camera.StartGrab(); // 开始抓取
                                        break;
                                    case enAcqMode.异步采集: // 异步触发、同步取图、异步停止 
                                        this._CurrentImageIndex = 0;
                                        result = this._camera.StopGrab();
                                        this._camera.ClearData();
                                        result = this._camera.StartGrab();
                                        this.GetImageAsyn();
                                        break;
                                }
                                break;
                            case enUserTriggerSource.软触发:
                                break;
                        }
                        if (result) //  && this._grabImage.IsInitialized()
                            LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
                        else
                            LoggerHelper.Info(this.CameraParam.SensorName + "图像采集失败");
                        break;
                }
            //}
            return result;
        }

        public bool StopTrigger()
        {
            bool result = false;
            try
            {
                switch (this.CameraParam.TriggerSource)
                {
                    case enUserTriggerSource.NONE: // 实时采集
                    case enUserTriggerSource.外部IO触发:
                    case enUserTriggerSource.内部IO触发:
                    case enUserTriggerSource.编码器触发:
                    case enUserTriggerSource.软触发:
                        switch (this.CameraParam.AcqMode)
                        {
                            case enAcqMode.同步采集:
                                result = this._camera.StopGrab();
                                break;
                            case enAcqMode.异步取图:
                                result = this._camera.StopGrab();
                                break;
                            case enAcqMode.异步采集:
                                result = this._camera.StopGrab();
                                this.cts?.Cancel(); // 取消异步采图线程
                                break;
                        }
                        result = true;
                        break;
                }
                LoggerHelper.Error(this.Name + ":停止相机采集成功！");
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.Name + ":停止触发相机失败" + ex);
            }
            return result;
        }

        /// <summary>
        /// 异步采集，这里不使用，全部改为同步采集， 最好是在起点终点各给一个信号，异步采集使用事件来传送图片
        /// </summary>
        protected override bool GetImageAsyn(out HImage hImage, out HImage darkImage)
        {
            bool result = false;
            enAcqState state;
            hImage = new HImage();
            darkImage = new HImage();
            HImage image = this._camera.GetHImage(this.CameraParam.DataWidth, this.CameraParam.DataHeight, "byte", this.CameraParam.Timeout, out darkImage, out state);
            this.AdjImg(image, out this._grabImage);
            this.AdjImg(darkImage, out this._grabDarkImage);
            image?.Dispose();
            darkImage?.Dispose();
            this._CurrentImageIndex++;
            if (this._CurrentImageIndex >= int.MaxValue)
                this._CurrentImageIndex = 0;
            LoggerHelper.Info("异步采集,当前图片 = " + this._CurrentImageIndex.ToString());
            result = true;
            return result;
        }

        /// <summary>
        /// 同步采集
        /// </summary>
        /// <param name="hImage"></param>
        /// <param name="darkImage"></param>
        /// <returns></returns>
        protected override bool GetImageSyn(out HImage hImage, out HImage darkImage)
        {
            bool result = true;
            enAcqState state;
            hImage = new HImage();
            darkImage = new HImage();
            List<HImage> imageList = new List<HImage>();
            for (int i = 0; i < this.CameraParam.AverangeCount; i++)
            {
                HImage image = this._camera.GetHImage(this.CameraParam.DataWidth, this.CameraParam.DataHeight, "byte", this.CameraParam.Timeout, out darkImage, out state);
                HImage adjImage = new HImage();
                this.AdjImg(image, out adjImage);  // 调整图像的方向及图像畸变
                //this.AdjImg(darkImage, out this._grabDarkImage);
                image?.Dispose();
                darkImage?.Dispose();
                imageList.Add(adjImage);
            }
            //////////////////
            if (imageList.Count > 0)
            {
                hImage = AveImage(imageList);
                result = true;
            }
            else
            {
                result = false;
            }
            foreach (var item in imageList)
            {
                item.Dispose();
            }
            imageList.Clear();
            return result;
        }

        protected override void GetImageAsyn()
        {
            this.cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                this.cts = new CancellationTokenSource();
                enAcqState state;
                while (true)
                {
                    if (this.cts.IsCancellationRequested) break;
                    HImage darkImage;
                    HImage image = this._camera.GetHImage(this.CameraParam.DataWidth, this.CameraParam.DataHeight, "byte", this.CameraParam.Timeout, out darkImage, out state);
                    this.AdjImg(image, out this._grabImage);
                    this.AdjImg(darkImage, out this._grabDarkImage);
                    image?.Dispose();
                    darkImage?.Dispose();
                    this._CurrentImageIndex++;
                    if (this._CurrentImageIndex >= int.MaxValue)
                        this._CurrentImageIndex = 0;
                    if (this._grabImage != null && this._grabImage.IsInitialized())
                        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabImage, this.CameraParam)); // 异步发送图像出去
                    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabDarkImage, this.CameraParam)); // 异步发送图像出去
                    LoggerHelper.Info("异步采集,当前图片 = " + this._CurrentImageIndex.ToString());
                    if (state == enAcqState.End)
                    {
                        this._camera.StopGrab(); // 采图结束了，停止相机流采集
                        break;
                    }
                    Thread.Sleep(50);
                }
            });
        }



    }
}
