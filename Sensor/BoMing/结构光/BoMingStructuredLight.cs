using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensor;
using System.Windows.Forms;
using HalconDotNet;
using Common;
using System.Threading;
using System.Diagnostics;

namespace Sensor
{
    [Serializable]
    public class BoMingStructuredLight : SensorBase, ISensor
    {
        private IntPtr _mDev = IntPtr.Zero;//设备句柄
        private int bmDevType = 0;   // 设备类型，1： 单投SS ; 2:双投 SD; 3:三投ST; 4: 四投SQ
        private event Bmsensor3Dsdk.BmsCallBackSortImageDataPtr mImageCB = null;            // 图像回调函数
        private bool mDevOpened = false;
        private readonly int mMaxImgHeight = 2048;  // 图像最大分辨率高
        private readonly int mMaxImgWidth = 2448;   // 图像最大分辨率宽
        private Bms3DDataXYZ[] finalPcl = null;
        private ushort[] deepData = null;
        private BoMingStructuredLightParams structuredLightParam;
        private int mPclWidth;
        private int mPclHeight;
        /// /////////


        public IntPtr MDev
        {
            get
            {
                return _mDev;
            }

            set
            {
                _mDev = value;
            }
        }
        public BoMingStructuredLightParams StructuredLightParam
        {
            get
            {
                return structuredLightParam;
            }

            set
            {
                structuredLightParam = value;
            }
        }
        public int BmDevType
        {
            get
            {
                return bmDevType;
            }

            set
            {
                bmDevType = value;
            }
        }


        public BoMingStructuredLight()
        {

        }

        private bool OpenDevice()
        {
            // 1. 获取设备对象
            this._mDev = Bmsensor3Dsdk.BmsSDK_Create();
            if (this._mDev == IntPtr.Zero)
                return false;
            Thread.Sleep(500);
            // 控制器: Mini USB2.0 连接
            if (!Bmsensor3Dsdk.OpenController(this._mDev))
                return false;
            // 相机： USB3.0 连接
            byte usbProtocol = 0;
            if (!Bmsensor3Dsdk.OpenCamera(this._mDev, out usbProtocol))
                return false;
            if (usbProtocol == 2)
            {
                return false;
            }
            // 获取当前设备类型， 单投？ 双投 or 三投，四投?
            this.bmDevType = DecideDeviceType();
            if (this.bmDevType == 0)
                return false;
            mDevOpened = true;

            return true;
        }
        private int DecideDeviceType()
        {
            char[] model = new char[10];
            if (!Bmsensor3Dsdk.GetProductModel(this._mDev, model))
            {
                return 0;
            }
            string sDeviceName = new string(model);
            int devType = 0;
            if (sDeviceName.Contains("SS")) devType = 1;
            if (sDeviceName.Contains("SD")) devType = 2;
            if (sDeviceName.Contains("ST")) devType = 3;
            if (sDeviceName.Contains("SQ")) devType = 4;
            //btnSetRingLamp.Enabled = devType >= 3; // 三投以上设备才支持环形光功能
            return devType;
        }
        private bool LoadCalibrationFiles(IntPtr in_dev, int in_devType)
        {
            string sCalibFilesDir = $@"{Application.StartupPath}\CalibrationFiles";
            string[] sPaths = new string[in_devType];
            // system calibration files
            sPaths[0] = $@"{sCalibFilesDir}\SysCalib.mb";
            if (in_devType >= 2) // SD, ST, SQ device?
                sPaths[1] = $@"{sCalibFilesDir}\SysCalib_R.mb";
            if (in_devType >= 3) //ST, SQ device?
                sPaths[2] = $@"{sCalibFilesDir}\SysCalib_T.mb";
            if (in_devType >= 4) //SQ device?
                sPaths[3] = $@"{sCalibFilesDir}\SysCalib_Q.mb";
            for (int i = 0; i < in_devType; i++)
            {
                if (!System.IO.File.Exists(sPaths[i]))
                {
                    MessageBox.Show("路径:" + sPaths[i] + "，文件不存在");
                    return false;
                }
            }
            if (!Bmsensor3Dsdk.LoadPhaseCoefficientsN(in_dev, sPaths, bmDevType, mMaxImgHeight, mMaxImgWidth))
            {
                MessageBox.Show("系统标定文件加载失败, 可能文件不存在或者分辨率不对");
                return false;
            }

            // base files
            sPaths[0] = $@"{sCalibFilesDir}\Base.mb";
            if (in_devType >= 2) // SD, ST, SQ device?
                sPaths[1] = $@"{sCalibFilesDir}\Base_R.mb";

            if (in_devType >= 3) //ST, SQ device?
                sPaths[2] = $@"{sCalibFilesDir}\Base_T.mb";

            if (in_devType >= 4) //SQ device?
                sPaths[3] = $@"{sCalibFilesDir}\Base_Q.mb";

            for (int i = 0; i < in_devType; i++)
            {
                if (!System.IO.File.Exists(sPaths[i]))
                {
                    MessageBox.Show("路径:" + sPaths[i] + "，文件不存在");
                    return false;
                }
            }

            if (!Bmsensor3Dsdk.LoadBaseMatFileN(in_dev, sPaths, bmDevType))
            {
                MessageBox.Show("基底标定文件加载失败, 可能文件不存在或者分辨率不对");
                return false;
            }

            // camera files
            sPaths[0] = $@"{sCalibFilesDir}\camCalib.xml";
            if (!System.IO.File.Exists(sPaths[0]))
            {
                MessageBox.Show("路径:" + sPaths[0] + "，文件不存在");
                return false;
            }
            if (!Bmsensor3Dsdk.LoadCalibrationFile(in_dev, sPaths[0]))
            {
                MessageBox.Show("基底标定文件加载失败, 可能文件不存在或者分辨率不对");
                return false;
            }

            return true;
        }
        private void EnabledRoi(bool in_roiEnbaled)
        {
            ImageROI roi = new ImageROI();
            if (in_roiEnbaled)
            {
                // 对于多投设备，设置第一个的roi参数即可，共用第一个！
                roi.leftTop_x = this.structuredLightParam.BmRoiParams.leftTop_x;
                roi.leftTop_y = this.structuredLightParam.BmRoiParams.leftTop_y;
                roi.width = this.structuredLightParam.BmRoiParams.width;
                roi.height = this.structuredLightParam.BmRoiParams.height;
                if (!Bmsensor3Dsdk.SetCameraROI(this._mDev, roi))
                    MessageBox.Show("roi 参数设置失败");
                for (int i = 0; i < this.structuredLightParam.BmPcdProcessParams.Length; i++)
                {
                    this.structuredLightParam.BmPcdProcessParams[i].isUseROI = true; // 算法 roi 同步使能
                }

            }
            else
            {
                // 关闭roi
                if (!Bmsensor3Dsdk.StopCameraROI(this._mDev))
                    MessageBox.Show("停止roi失败");
                for (int i = 0; i < this.structuredLightParam.BmPcdProcessParams.Length; i++)
                {
                    this.structuredLightParam.BmPcdProcessParams[i].isUseROI = false; // 算法 roi 同步禁止
                }
            }
            for (int i = 0; i < this.structuredLightParam.BmPcdProcessParams.Length; i++)
            {
                this.structuredLightParam.BmPcdProcessParams[i].ROI = roi;
            }
        }
        private bool InitDevice()
        {
            // work mode ,2d or 3d?
            if (!Bmsensor3Dsdk.SetWorkMode(this._mDev, enBmsWorkModeType.WorkMode3D, IntPtr.Zero))
                return false;
            // trigger mode: soft
            switch (this.structuredLightParam.BmTriggerMode)
            {
                case enBmsTriggerMode.NoTrigger:
                    Bmsensor3Dsdk.SetTriggerMode(this._mDev, enBmsTriggerMode.SoftwareTrigger);
                    break;
                case enBmsTriggerMode.SoftwareTrigger:
                    Bmsensor3Dsdk.SetTriggerMode(this._mDev, enBmsTriggerMode.SoftwareTrigger);
                    break;
                case enBmsTriggerMode.HardwareTriggerOne:
                    Bmsensor3Dsdk.SetTriggerMode(this._mDev, enBmsTriggerMode.HardwareTriggerOne);
                    Bmsensor3Dsdk.SetHardwareTriggerMode(this._mDev, 1, 0);
                    break;

                case enBmsTriggerMode.HardwareTriggerTwo:
                    Bmsensor3Dsdk.SetTriggerMode(this._mDev, enBmsTriggerMode.HardwareTriggerTwo);
                    Bmsensor3Dsdk.SetHardwareTriggerMode(this._mDev, 2, 0);
                    break;
                case enBmsTriggerMode.HardWareTriggerOneAndTwo:
                    Bmsensor3Dsdk.SetTriggerMode(this._mDev, enBmsTriggerMode.HardWareTriggerOneAndTwo);
                    Bmsensor3Dsdk.SetHardwareTriggerMode(this._mDev, 2, 0);
                    break;
            }
            // exposure: 10ms - 1000 ms
            if (!Bmsensor3Dsdk.SetCameraExposureTime(this._mDev, this.structuredLightParam.Expose))
                return false;
            // gain: 0-17x 
            if (!Bmsensor3Dsdk.SetCameraGain(this._mDev, this.structuredLightParam.Gain))
            {
                return false;
            }
            // led value： 0-250; set all to the same(can be different)
            for (var channel = 1; channel <= this.bmDevType; channel++)
            {
                if (!Bmsensor3Dsdk.SetLEDBrightness(this._mDev, this.structuredLightParam.LedLight, (byte)channel))
                {
                    return false;
                }
            }
            // load calibration files
            if (!LoadCalibrationFiles(this._mDev, this.bmDevType))
                return false;

            //this.mPcdProcessParams = InitPcdProcessParameters(this._mDev, this.mDevType);
            //if (mPcdProcessParams == null)
            //return false;

            // 默认关闭 roi
            EnabledRoi(this.structuredLightParam.EnbaledROI);

            return true;
        }

        /// <summary>
        /// 点云，深度图/高度图,异步操作回调函数
        /// </summary>
        /// <param name="pData">相机输出的2D灰度图，如果把设备当作一个普通2D相用，这里即是2D图像</param>
        /// <param name="pFrameInfo">2D图像帧信息</param>
        /// <param name="pSortData">排序后的2D 图像，用于生成点云，高度图</param>
        /// <param name="pUser">用户参数</param>
        private void ImageCallback(IntPtr pData, MV_FRAME_OUT_INFO pFrameInfo, IntPtr pSortData, IntPtr pUser)
        {
            enBmsWorkModeType mode;
            if (!Bmsensor3Dsdk.GetWorkMode(this._mDev, out mode))
            {
                //MessageBox.Show("回调：未能确定设备工作模式！");
                return;
            }
            if (mode == enBmsWorkModeType.WorkMode2D || pSortData == IntPtr.Zero)
                return;   // 2D 不处理！3d 扫描未完成，没有数据，直接返回
            /////////////////////////////
            DataProcess(pSortData, pFrameInfo.nWidth, pFrameInfo.nHeight);
        }
        private void DataProcess(IntPtr pSortData, int imageWidth, int imageHeight)
        {
            FilterParas mHighQualityParams = this.structuredLightParam.BmHighQualityParams;
            this.mPclWidth = this.structuredLightParam.BmHighSpeedEnabled ? imageWidth / 2 : imageWidth; // 开下采样后宽度变成原来的一半
            this.mPclHeight = this.structuredLightParam.BmHighSpeedEnabled ? imageHeight / 2 : imageHeight; // 开下采样后宽度变成原来的一半
            /////////////////////
            int cloudLen = mPclWidth * mPclHeight; // 描述了点云的长宽
            this.LaserParam.DataWidth = mPclWidth;
            this.LaserParam.DataHeight = mPclHeight;
            finalPcl = new Bms3DDataXYZ[cloudLen]; // 融合后的最终点云，必须设置
            Stopwatch time1 = new Stopwatch();
            time1.Start();
            Bmsensor3Dsdk.ComputeWorldPointXYZN(this._mDev, pSortData, imageHeight, imageWidth, 81, this.structuredLightParam.BmHighSpeedEnabled, this.structuredLightParam.BmPcdProcessParams, this.structuredLightParam.BmPcdProcessParams.Length, finalPcl, null, this.structuredLightParam.BmHighQualityEnabled, ref mHighQualityParams);
            time1.Stop();
            long a = time1.ElapsedMilliseconds;
            /////////////////////////////////
        }


        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = true;
            try
            {
                this.ConfigParam = configParam;
                this.Name = this.ConfigParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(configParam.SensorName);
                this.LaserParam.SensorName = this.ConfigParam.SensorName;
                switch (configParam.ConnectType)
                {
                    default:
                    case enUserConnectType.USB:
                    case enUserConnectType.SerialNumber:
                        // 2. 打开设备
                        if (OpenDevice())
                        {
                            // 加载配置参数
                            this.structuredLightParam = new BoMingStructuredLightParams().Read(Application.StartupPath + "\\" + "configParam" + "\\" + this.ConfigParam.SensorName + ".txt");
                            if (this.structuredLightParam == null)
                            {
                                this.structuredLightParam = new BoMingStructuredLightParams();
                                this.structuredLightParam.BmPcdProcessParams = new AlgoParas[this.bmDevType];
                            }
                            // 3. 初始化设备，设置一些必要的参数
                            if (!InitDevice())
                                result = false;
                            else
                                result = true;
                        }
                        else
                            result = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.ConfigParam.SensorName + "打开失败", ex);
                result = false;
            }
            configParam.ConnectState = result;
            return result;
        }
        public bool Disconnect()
        {
            //mImageCB -= new Bmsensor3Dsdk.BmsCallBackSortImageDataPtr(ImageCallback);
            Bmsensor3Dsdk.CloseController(this._mDev);
            if (Bmsensor3Dsdk.CloseCamera(this._mDev))
                return true;
            else
                return false;
        }
        public object GetParam(object paramType)
        {
            // 根据不同的参数选择不同的分支来设置 
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            //////////////////////
            switch (type)
            {
                /////////////////
                case enSensorParamType.Coom_每线点数: //this.measureRange
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Coom_激光位姿: //this.measureRange
                    return this.LaserParam.LaserCalibrationParam;

                case enSensorParamType.Coom_激光校准参数:
                    return this.LaserParam.LaserCalibrationParam;

                case enSensorParamType.Coom_点云宽度:
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Coom_点云高度:
                    return this.LaserParam.DataHeight;

                case enSensorParamType.Coom_传感器类型:
                    return this.ConfigParam.SensorType;

                default:
                    return null;
            }
        }
        public bool Init()
        {
          
            this.LaserParam.DataHeight = 1;
            this.LaserParam.DataWidth = 1;
            return true;
        }
        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> listdata = new Dictionary<enDataItem, object>();
            double[] x;
            double[] y;
            double[] dist1;
            double[] dist2;
            if (this.finalPcl == null) return listdata;
            x = new double[this.finalPcl.Length];
            y = new double[this.finalPcl.Length];
            dist1 = new double[this.finalPcl.Length];
            dist2 = new double[this.finalPcl.Length];
            for (int i = 0; i < this.finalPcl.Length; i++)
            {
                if (float.IsNaN(this.finalPcl[i].Z))
                {
                    x[i] = 0;
                    y[i] = 0;
                    dist1[i] = -99999;
                    dist2[i] = 0;
                }
                else
                {
                    x[i] = this.finalPcl[i].X;
                    y[i] = this.finalPcl[i].Y;
                    if (this.LaserParam.IsMirrorZ)
                    {
                        dist1[i] = this.finalPcl[i].Z * -1;
                        dist2[i] = this.finalPcl[i].Z *-1;
                    }                 
                    else
                    {
                        dist1[i] = this.finalPcl[i].Z;
                        dist2[i] = this.finalPcl[i].Z;
                    }
                }
            }
            this.finalPcl = null;
            this.deepData = null;
            /////////////
            listdata.Add(enDataItem.Dist1,dist1);
            listdata.Add(enDataItem.Dist2, dist2);
            listdata.Add(enDataItem.X, x);
            listdata.Add(enDataItem.Y, y);
            return listdata;
        }
        public bool SetParam(object paramType, object value)
        {
            // 根据不同的参数选择不同的分支来设置 
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            //////////////////////
            switch (type)
            {
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth = (int)value;
                    break;
                case enSensorParamType.Coom_激光位姿: //this.measureRange
                    //this.LaserParam.LaserPose = (HTuple)value;
                    break;

                default:
                    break;
            }
            return true;
        }
        public bool StartTrigger()
        {
            /// 异步方式,将调用回调函数处理数据
            if (!this.structuredLightParam.BmSyncScan)
            {
                if (!Bmsensor3Dsdk.StartTrigger(this._mDev))
                {
                    //MessageBox.Show("启动3D扫描失败");
                    return false;
                }
            }
            /// 同步方式，不调用回调函数
            else
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                IntPtr pSortData = Bmsensor3Dsdk.StartTriggerSync(this._mDev);
                time.Stop();
                long a = time.ElapsedMilliseconds;
                if (pSortData == IntPtr.Zero)
                {
                    //MessageBox.Show("启动3D扫描失败");
                    return false;
                }
                // 默认最大分辨率是 2884*2048， roi时 宽高各降低一半
                int mImgWidth = this.structuredLightParam.EnbaledROI ? 2448 / 2 : 2448;
                int mImgHeight = this.structuredLightParam.EnbaledROI ? 2048 / 2 : 2048;
                DataProcess(pSortData, mImgWidth, mImgHeight);
            }
            return true;
        }
        public bool StopTrigger()
        {
            return true;
            //throw new NotImplementedException();
        }
    }



}
