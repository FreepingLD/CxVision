using System;
using System.Windows.Forms;
using Sensor;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using FunctionBlock;

namespace FunctionBlock
{
    public partial class BoMingStructuredLightFrom : Form
    {

        public enum ImageType
        {
            CloudImage,
            DeepImage
        }

        #region Fileds
        bool mSyncScan = true;  // 是否同步方式
        IntPtr mDev = IntPtr.Zero;//设备句柄
        int mDevType = 0;   // 设备类型，1： 单投SS ; 2:双投 SD; 3:三投ST; 4: 四投SQ
        private readonly int mMaxImgHeight = 2048;  // 图像最大分辨率高
        private readonly int mMaxImgWidth = 2448;   // 图像最大分辨率宽
        private AlgoParas[] mPcdProcessParams = null;      // 点云处理参数
        FilterParas mHighQualityParams = new FilterParas(); // 高精度参数
        private event Bmsensor3Dsdk.BmsCallBackSortImageDataPtr  mImageCB = null;            // 图像回调函数
        private event Bmsensor3Dsdk.BmsCallBackConnectEventPtr mHotplugEventCB = null;       // 热插拔事件回调函数 

        bool mIsHardTrig = false;
        Semaphore mSem3DEnd = new Semaphore(0, 1); // 3d 处理完成信号
        bool mDevOpened = false;
        ImageType mImageType = ImageType.DeepImage;    // 3D图形类型
        private bool mHighSpeedEnabled = false;  // 高速率使能(下采样开启)
        private bool mHighQualityEnabled = false; // 高精度使能(高精度开启)
        bool mInited = false;   // 设备是否初始化过
        private bool mGenSinglePcl = false; // 是否生成独立点云，多投SD,ST,SQ设备可以选择生成各自投影融合前独立的点云
        Bms3DDataXYZ[] finalPcl = null; //最终生成的点云
        bool mSoftContinueTrig = false;

        ushort[] deep = null;
        object mDeepLock = new object();

        //当前图像宽高
        int mImgWidth = 0;
        int mImgHeight = 0;
        private bool mContinueEnd = false;
        private int mPclWidth;
        private int mPclHeight;
        #endregion

        private BoMingStructuredLight structuredLight;
        #region Constructors
        public BoMingStructuredLightFrom(ISensor sensor)
        {
            InitializeComponent();
            this.structuredLight = (BoMingStructuredLight)sensor;
            this.mDev = structuredLight.MDev;
            this.mDevType = structuredLight.BmDevType;
            // ！对于非托管的 Native 回调函数，要实例化对象的方式来注册，否则 Native 回调在没有使用时会被 GC 回收
            //this.mImageCB += new Bmsensor3Dsdk.BmsCallBackSortImageDataPtr(ImageCallback);
            //this.mHotplugEventCB += new Bmsensor3Dsdk.BmsCallBackConnectEventPtr(HotplugEventCallback);
            // ui
            DefaultUIValue();
        }
        public BoMingStructuredLightFrom(AcqSource acqSource)
        {
            InitializeComponent();
            this.structuredLight = (BoMingStructuredLight)acqSource.Sensor; // sensor;
            this.mDev = structuredLight.MDev;
            this.mDevType = structuredLight.BmDevType;


            DefaultUIValue();
        }
        void DefaultUIValue()
        {
            cbTrigSource.SelectedIndex = 0; // 触发模式默认：软件触发
            cbTrigLevel.SelectedIndex = 0; // 硬件触发电平：上升沿
            ckHardTrigReady.Visible = lbTrigLevel.Visible = cbTrigLevel.Visible = cbTrigSource.SelectedIndex != 0; // 硬件触发模式才显示触发电平选择

            cbOutExe.SelectedIndex = 0;  // 
            cbOutChannel.SelectedIndex = 0;  // 输出 io
            cbOutFunction.SelectedIndex = 0; // 输出 电平

            cbSignalType.SelectedIndex = 0;   // 信号类型，电平或者脉冲
            pnPulseWidth.Visible = cbSignalType.SelectedIndex == 1; // 脉冲模式才显示脉冲宽度设置

            cbDefaultLevel.SelectedIndex = 0;  // 电平类型，脉冲形式或者电平形式

            //
            if (this.structuredLight.StructuredLightParam != null)
            {
                cbTrigSource.SelectedIndex = (int)this.structuredLight.StructuredLightParam.BmTriggerMode;
                cbTrigLevel.SelectedIndex = 0; // 硬件触发电平：上升沿
                /// 相机参数
                this.txtExp.Text = this.structuredLight.StructuredLightParam.Expose.ToString();
                this.txtLed.Text = this.structuredLight.StructuredLightParam.LedLight.ToString();
                this.txtGain.Text = this.structuredLight.StructuredLightParam.Gain.ToString();
                this.ckHighQuality.Checked = this.structuredLight.StructuredLightParam.BmHighQualityEnabled;
                this.ckHighSpeed.Checked = this.structuredLight.StructuredLightParam.BmHighSpeedEnabled;
                this.ckSyncScan.Checked = true;
                ////////////////IO输入输出
                this.cbOutFunction.SelectedIndex = this.structuredLight.StructuredLightParam.BmIoOutPara.IoType;        // gpio 或特殊功能
                this.cbSignalType.SelectedIndex = this.structuredLight.StructuredLightParam.BmIoOutPara.OutType;       // 电平模式，脉冲模式
                this.cbDefaultLevel.SelectedIndex = this.structuredLight.StructuredLightParam.BmIoOutPara.LelDtLevel;  // 电平模式：默认电平
                //this.cbDefaultLevel.SelectedIndex = this.structuredLight.StructuredLightParam.BmIoOutPara.PwDtLevel;   // 脉冲模式：默认电平
                this.txtPulseWidth.Text = this.structuredLight.StructuredLightParam.BmIoOutPara.PulseWidth.ToString(); // 脉冲模式的脉冲宽度
                ///////////////////// 算法参数
                this.ckMaskFlag.Checked = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].maskFlag;
                this.txtThreshold.Text = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].threshold.ToString();
                this.txtRemovalMin.Text = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].removalMin.ToString();
                this.txtRemovalMax.Text = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].removalMax.ToString();
                this.txtMeanK.Text = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].meanK.ToString();
                this.txtStdThreshold.Text = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].stdThresh.ToString();
                this.txtRepeats.Text = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].repeats.ToString();
                this.ckIsFiltered.Checked = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].isFiltered;
                this.ckIsFillHole.Checked = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].isFillHole;
                this.txtHoleMaker.Text = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].holeMaker.ToString();
                this.取反3D点云checkBox.Checked = this.structuredLight.StructuredLightParam.BmPcdProcessParams[0].isUpDown;
                ///////////////////////////ROI参数
                this.txtROIX.Text = this.structuredLight.StructuredLightParam.BmRoiParams.leftTop_x.ToString();
                this.txtROIY.Text = this.structuredLight.StructuredLightParam.BmRoiParams.leftTop_y.ToString();
                this.txtROIW.Text = this.structuredLight.StructuredLightParam.BmRoiParams.width.ToString();
                this.txtROIH.Text = this.structuredLight.StructuredLightParam.BmRoiParams.height.ToString();
            }

            //
        }


        #endregion

        #region Form load, initialization
        private void MainFrom_Load(object sender, EventArgs e)
        {
            //// extra: 热插拔使能
            EnableHotplug(this.mDev, true);
            //// 3. 初始化设备，设置一些必要的参数
            if (!InitDevice(mDev, mDevType))
                return;
            // ui
            mDevOpened = true;
            OpenDevice2UI(mDevOpened);
            InitDevice2UI(mDevOpened);

            mInited = true;
        }

        #endregion

        #region Help methods
        int DecideDeviceType(IntPtr in_dev)
        {
            char[] model = new char[10];
            if (!Bmsensor3Dsdk.GetProductModel(in_dev, model))
            {
                MessageBox.Show("设备型号获取失败，可能是固件中没有写入型号！");
                return 0;
            }

            string sDeviceName = new string(model);
            int devType = 0;

            if (sDeviceName.Contains("SS")) devType = 1;
            if (sDeviceName.Contains("SD")) devType = 2;
            if (sDeviceName.Contains("ST")) devType = 3;
            if (sDeviceName.Contains("SQ")) devType = 4;

            btnSetRingLamp.Enabled = devType >= 3; // 三投以上设备才支持环形光功能

            return devType;
        }

        void EnableHotplug(IntPtr in_dev, bool hotplug)
        {
            if (!hotplug)
                return;
            if (!Bmsensor3Dsdk.SetUsbConnectStatusCallBack(in_dev, mHotplugEventCB, IntPtr.Zero, false))
                MessageBox.Show("设置热插拔事件回调函数失败");
        }

        bool OpenDevice(IntPtr in_dev)
        {
            if (in_dev == IntPtr.Zero)
                return false;

            // 控制器: Mini USB2.0 连接
            if (!Bmsensor3Dsdk.OpenController(in_dev))
            {
                MessageBox.Show("控制器打开失败，请检查控制器USB2.0连接，电源连接，尝试重新上电");
                return false;
            }

            // 相机： USB3.0 连接
            byte usbProtocol = 0;
            if (!Bmsensor3Dsdk.OpenCamera(in_dev, out usbProtocol))
            {
                MessageBox.Show("相机打开失败，请检查相机USB3.0连接");
                return false;
            }
            if (usbProtocol == 2)
            {
                MessageBox.Show("相机被识别成一个USB2.0设备，请检查是否连接到USB2.0接口，尝试重新插拔相机线缆");
                return false;
            }

            // 获取当前设备类型， 单投？ 双投 or 三投，四投?
            mDevType = DecideDeviceType(mDev);
            if (mDevType == 0)
                return false;

            mDevOpened = true;

            return true;
        }

        bool CloseDevice(IntPtr in_dev)
        {
            if (!Bmsensor3Dsdk.CloseCamera(mDev))
            {
                MessageBox.Show("相机关闭失败");
                return false;
            }
            Bmsensor3Dsdk.CloseController(mDev);
            return true;
        }

        bool InitDevice(IntPtr in_dev, int in_devType)
        {
            if (in_dev == IntPtr.Zero)
                return false;

            // work mode ,2d or 3d?
            IntPtr handle = rdDeepImage.Checked ? IntPtr.Zero : this.pictureBox1.Handle;
            if (!Bmsensor3Dsdk.SetWorkMode(in_dev, enBmsWorkModeType.WorkMode3D, handle))
            {
                MessageBox.Show("设置3D工作模式失败");
                return false;
            }

            // trigger mode: soft
            if (!Bmsensor3Dsdk.SetTriggerMode(in_dev, (enBmsTriggerMode)(cbTrigSource.SelectedIndex + 1)))
            {
                MessageBox.Show("设置3D扫描为 软件触发模式失败");
                return false;
            }

            // hard trig level: 如果是硬件触发，再设置触发电平
            if (cbTrigLevel.SelectedIndex > 0)
            {
                if (!Bmsensor3Dsdk.SetHardwareTriggerMode(mDev, (byte)(cbTrigSource.SelectedIndex - 1), (byte)cbTrigLevel.SelectedIndex))
                    MessageBox.Show("设置硬件触发电平失败");
            }

            // output ：输出 io 初始化
            SetOutSignal();

            // exposure: 10ms - 1000 ms
            int exp = int.Parse(txtExp.Text);
            if (!Bmsensor3Dsdk.SetCameraExposureTime(in_dev, (short)exp))
            {
                MessageBox.Show("设置曝光失败");
                return false;
            }

            // gain: 0-17x 
            float gain = float.Parse(txtGain.Text);
            if (!Bmsensor3Dsdk.SetCameraGain(in_dev, gain))
            {
                MessageBox.Show("增益设置失败");
                return false;
            }

            // led value： 0-250; set all to the same(can be different)
            int led = int.Parse(txtLed.Text);
            for (var channel = 1; channel <= in_devType; channel++)
            {
                if (!Bmsensor3Dsdk.SetLEDBrightness(in_dev, (byte)led, (byte)channel))
                {
                    MessageBox.Show($"投影{channel} led亮度设置失败");
                    return false;
                }
            }

            //image callback
            if (!mSyncScan)
            {

                if (!Bmsensor3Dsdk.SetSortImageDataCallBack(in_dev, mImageCB, IntPtr.Zero))
                {
                    MessageBox.Show("设置图像回调函数失败");
                    return false;
                }
            }


            // load calibration files
            if (!LoadCalibrationFiles(in_dev, in_devType))
                return false;

            mPcdProcessParams = InitPcdProcessParameters(in_dev, in_devType);
            if (mPcdProcessParams == null)
                return false;

            // 默认关闭 roi
            EnabledRoi(false);

            mInited = true;

            return true;
        }

        bool LoadCalibrationFiles(IntPtr in_dev, int in_devType)
        {
            string sCalibFilesDir = $@"{Application.StartupPath}\CalibrationFiles";
            //string sCalibFilesDir = $@"D:\中文路径\CalibrationFiles";

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

            if (!Bmsensor3Dsdk.LoadPhaseCoefficientsN(in_dev, sPaths, mDevType, mMaxImgHeight, mMaxImgWidth))
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

            if (!Bmsensor3Dsdk.LoadBaseMatFileN(in_dev, sPaths, mDevType))
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

        AlgoParas[] InitPcdProcessParameters(IntPtr in_dev, int in_devType) // 初始化点云处理参数
        {
            AlgoParas[] pcdParams = new AlgoParas[in_devType];
            // to be the same fo this sample.   ( can be different)
            for (int i = 0; i < pcdParams.Length; i++)
            {
                pcdParams[i].maskFlag = ckMaskFlag.Checked;
                pcdParams[i].threshold = float.Parse(txtThreshold.Text);
                pcdParams[i].removalMin = float.Parse(txtRemovalMin.Text);
                pcdParams[i].removalMax = float.Parse(txtRemovalMax.Text);
                pcdParams[i].meanK = int.Parse(txtMeanK.Text);
                pcdParams[i].stdThresh = float.Parse(txtStdThreshold.Text);
                pcdParams[i].repeats = int.Parse(txtRepeats.Text);
                pcdParams[i].isFiltered = ckIsFiltered.Checked;
                pcdParams[i].isUseROI = false;
                pcdParams[i].isFillHole = ckIsFillHole.Checked;
                pcdParams[i].holeMaker = int.Parse(txtHoleMaker.Text);
                pcdParams[i].fillHoleRepeats = 1;
                pcdParams[i].isUpDown = this.取反3D点云checkBox.Checked; // 用于取反3D点云
                pcdParams[i].enableUserInvalidValue = false;
            }

            // 高精度参数，对点云进行高精度优化处理，会增加点云生成的时间
            mHighQualityParams.meanBlurKernel = int.Parse(txtMeanBlurKernel.Text);
            mHighQualityParams.medianBlurKernel = int.Parse(txtMedianBlurKernel.Text);
            mHighQualityParams.d = int.Parse(txtd.Text);
            mHighQualityParams.sigmaColor = float.Parse(txtSigmaColor.Text);
            mHighQualityParams.sigmaSpace = float.Parse(txtSigmaSpace.Text);

            return pcdParams;
        }

        bool Switch2D()
        {
            // 先切换 2d 模式，再启动预览
            if (!Bmsensor3Dsdk.SetWorkMode(mDev, enBmsWorkModeType.WorkMode2D, this.pictureBox1.Handle))
            {
                MessageBox.Show("设置2d模式失败");
                return false;
            }

            var status2d = new enSSSensor_CaptureStatus_s();
            status2d.value = enSSSensor_CaptureStatus_e.Running;
            if (!Bmsensor3Dsdk.Set(mDev, enCmdCode_SSSensor.CaptureStatus, status2d))
            {
                MessageBox.Show(" 启动2D 预览失败");
                return false;
            }

            return true;
        }

        bool Switch3D()
        {
            // 先停止2d预览，再切换3d模式
            var status2d = new enSSSensor_CaptureStatus_s();
            status2d.value = enSSSensor_CaptureStatus_e.Stopped;
            if (!Bmsensor3Dsdk.Set(mDev, enCmdCode_SSSensor.CaptureStatus, status2d))
            {
                MessageBox.Show(" 2D 预览停止失败");
                return false;
            }

            IntPtr handle = rdCloudImage.Checked ? IntPtr.Zero : this.pictureBox1.Handle;
            if (!Bmsensor3Dsdk.SetWorkMode(mDev, enBmsWorkModeType.WorkMode3D, this.pictureBox1.Handle))
            {
                MessageBox.Show("设置3d模式失败");
                return false;
            }

            return true;
        }

        // 归一化处理，先获取 像素值 max，min
        void GetMaxMinPixel(ushort[] pGray16, int size, ref ushort max, ref ushort min)
        {
            ushort max1 = 0;
            ushort min1 = 65535;

            for (int i = 0; i < size; i++)
            {
                if (max1 < pGray16[i])
                    max1 = pGray16[i];

                if (min1 > pGray16[i])
                    min1 = pGray16[i];
            }

            max = max1;
            min = min1;
        }
        // 16 bit to 8bit
        void Gray16To8(ushort[] pGray16, byte[] pGray8, int width, int height, bool rotate)
        {
            ushort max = 0;
            ushort min = 0;

            int size = width * height;
            GetMaxMinPixel(pGray16, size, ref max, ref min);
            for (var i = 0; i < size; i++)
            {
                double pix = ((double)(pGray16[i] - min)) / ((double)(max - min));
                if (rotate)
                    pGray8[size - 1 - i] = (byte)(pix * (double)255);
                else
                    pGray8[i] = (byte)(pix * (double)255);
            }
        }
        void DrawGray16(ushort[] gray16, int width, int height, Graphics g, Rectangle rc)
        {
            // 为了显示高度图，要将 16bit 高度图数据转换为 8bit 灰度图数据
            int size = height * width;
            byte[] gray8 = new byte[height * width];
            Gray16To8(gray16, gray8, width, height, true);

            IntPtr imgPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(gray8, 0, imgPtr, size);

            // 得到 8bit 灰度图 bitmap
            Bitmap bmp = new Bitmap(width, height, width, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, imgPtr);

            // 填充 8bit bitmap 的调色板，用于显示
            ColorPalette tempPalette;
            using (Bitmap tempBmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
                tempPalette = tempBmp.Palette;

            for (int i = 0; i < 256; i++)
                tempPalette.Entries[i] = Color.FromArgb(i, i, i);

            bmp.Palette = tempPalette;

            // Graphics 绘图方式，picturebox 不会一直占用 bitmap。然后可以让gc 立即回收内存！
            g.DrawImage(bmp, rc);

            // 让gc 立即回收内存
            bmp.Dispose();
            bmp = null;
            Marshal.FreeHGlobal(imgPtr);
            gray8 = null;
        }


        #endregion

        #region Help methods， Notify ui
        void OpenDevice2UI(bool in_opened)
        {
            btnOpenDevice.BackColor = in_opened ? Color.Green : Color.Red;
            btnOpenDevice.Text = in_opened ? "关闭设备" : "打开设备";
        }
        void InitDevice2UI(bool in_inited)
        {
            gbContext.Enabled = in_inited;
            btn3DScan.Enabled = in_inited;

            // ui value

        }
        void EnabledUIWhile3DScanning(bool in_isScanning) // 3D 扫描过程中禁止操作ui修改参数;完毕后允许
        {
            gbContext.Enabled = in_isScanning;   // 配置
            gbPclProcParams.Enabled = in_isScanning;  // 点云算法参数
            gbIO.Enabled = in_isScanning; // io
            btn3DScan.Enabled = in_isScanning;  // 软件触发按钮
        }
        void ShowScanEndTip(bool in_scanningEnd, bool in_cloud) // 扫描完成提示
        {
            string text = "3D扫描中...";
            if (in_scanningEnd)
            {
                if (in_cloud)
                    text = $@"3D扫描完成，点云文件:{Application.StartupPath}\pcd\test.pcd";
                else
                    text = $@"3D扫描完成";
            }

            if (lbSavePclTip.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { lbSavePclTip.Text = x; };
                lbSavePclTip.BeginInvoke(actionDelegate, text); // 异步执行代理方法
            }
            else
                lbSavePclTip.Text = text;
        }
        void InvalidatePicBox(PictureBox pb)
        {
            //if (pb.InvokeRequired)
            //{
            //    Action<string> actionDelegate = (x) => { pb.Invalidate(); };
            //    pb.BeginInvoke(actionDelegate); // 异步执行代理方法
            //}
            //else
            pb.Invalidate();

        }
        #endregion

        #region Control events
        private void btnOpenDevice_Click(object sender, EventArgs e) // 打开/关闭设备
        {
            bool opened = mDevOpened = !mDevOpened;
            if (opened)
            {
                if (!OpenDevice(mDev))
                    return;

                if (mInited)  // 如果初始化过，重新打开设备，为了加快打开速度，只更新工作模式即可！
                {
                    // 当前是2D ？
                    if (rdWork2D.Checked)
                        Switch2D();  // 2d模式
                    else
                        Switch3D(); // 3d 模式  
                }
                else   // 从来没有初始化过，则执行完整的初始化流程
                {
                    InitDevice(mDev, mDevType);
                }

            }
            else
                CloseDevice(mDev);


            OpenDevice2UI(opened);
            InitDevice2UI(opened);
        }
        bool StartScan()
        {
            /// 异步方式
            /// 
            if (!mSyncScan)
            {
                if (!Bmsensor3Dsdk.StartTrigger(mDev))
                {
                    MessageBox.Show("启动3D扫描失败");
                    return false;
                }
            }
            /// 同步方式
            else
            {
                IntPtr pSortData = Bmsensor3Dsdk.StartTriggerSync(mDev);
                if (pSortData == IntPtr.Zero)
                {
                    MessageBox.Show("启动3D扫描失败");
                    return false;
                }

                // 默认最大分辨率是 2884*2048， roi时 宽高各降低一半
                mImgWidth = ckROIEnabled.Checked ? 2448 / 2 : 2448;
                mImgHeight = ckROIEnabled.Checked ? 2048 / 2 : 2048;
                DataProcess(IntPtr.Zero, mImgWidth, mImgHeight); //
            }


            return true;
        }
        private void btn3DScan_Click(object sender, EventArgs e) // 3D 扫描
        {
            // 触发 3d 扫描后，回调 ImageCallback 会收到图像数据，生成点云。
            // 这里等待点云生成处理完毕再允许下一次
            ShowScanEndTip(false, rdCloudImage.Checked);
            EnabledUIWhile3DScanning(false);

            if (!StartScan())
                return;

            // 异步扫描，单次软件触发，等待点云处理完成 后释放控件；
            if (!mSyncScan)
            {
                if (!mSoftContinueTrig && !mSem3DEnd.WaitOne(15000))   // 等待 3d 扫描处理结束，最多15s
                    MessageBox.Show("超时返回，点云处理可能发生了异常！");
            }


            EnabledUIWhile3DScanning(true);
            ShowScanEndTip(true, rdCloudImage.Checked);
        }
        private void ckSoftContinueScan_CheckedChanged(object sender, EventArgs e) // 软件连续扫描
        {
            mSoftContinueTrig = (sender as CheckBox).Checked;

            if (mSoftContinueTrig)
            {
                StartScan();
                timer1.Enabled = true; // 定时器中连续触发
            }
            else
            {
                timer1.Enabled = false;
            }

            EnabledUIWhile3DScanning(!mSoftContinueTrig);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mContinueEnd)
            {
                mContinueEnd = false;
                StartScan();
            }
        }
        private void rdWork2D_CheckedChanged(object sender, EventArgs e) // 2d模式切换
        {
            if (mDev == IntPtr.Zero) return;
            var rd = sender as RadioButton;
            if (!rd.Checked)
                return;

            Switch2D();
        }
        private void rdWork3D_CheckedChanged(object sender, EventArgs e) // 3d 模式切换
        {
            if (mDev == IntPtr.Zero) return;
            var rd = sender as RadioButton;
            btn3DScan.Enabled = rd.Checked;
            if (!rd.Checked)
                return;

            Switch3D();
        }
        private void ckHighSpeed_CheckedChanged(object sender, EventArgs e)  // 高速率使能
        {
            if (ckROIEnabled.Checked && ckHighSpeed.Checked)
            {
                MessageBox.Show("已经开启了ROI功能，无需再启用高速率");
                ckHighSpeed.Checked = false;
                return;
            }

            mHighSpeedEnabled = ckHighSpeed.Checked;
            this.structuredLight.StructuredLightParam.BmHighSpeedEnabled = mHighSpeedEnabled;
        }
        private void ckHighQuality_CheckedChanged(object sender, EventArgs e)
        {
            mHighQualityEnabled = (sender as CheckBox).Checked;
            this.structuredLight.StructuredLightParam.BmHighQualityEnabled = mHighQualityEnabled;
        }// 高精度使能
        private void rdCloudImage_CheckedChanged(object sender, EventArgs e)
        {
            IntPtr handle = rdCloudImage.Checked ? IntPtr.Zero : this.pictureBox1.Handle;
            if (!Bmsensor3Dsdk.SetWorkMode(mDev, enBmsWorkModeType.WorkMode3D, handle))
            {
                MessageBox.Show("设置3d模式失败");
                return;
            }
            mImageType = ImageType.CloudImage;

        }// 点云图生成
        private void rdDeepImage_CheckedChanged(object sender, EventArgs e)
        {
            IntPtr handle = rdCloudImage.Checked ? IntPtr.Zero : this.pictureBox1.Handle;
            if (!Bmsensor3Dsdk.SetWorkMode(mDev, enBmsWorkModeType.WorkMode3D, handle))
            {
                MessageBox.Show("设置3d模式失败");
                return;
            }
            mImageType = ImageType.DeepImage;
        } // 深度图生成
        private void ckSinglePcl_CheckedChanged(object sender, EventArgs e)
        {
            mGenSinglePcl = (sender as CheckBox).Checked;
        }// 是否生成独立点云

        private void btnSetExp_Click(object sender, EventArgs e) // exposure
        {
            if (mDev == IntPtr.Zero) return;
            int exp = int.Parse(txtExp.Text);
            this.structuredLight.StructuredLightParam.Expose = (short)exp;
            if (!Bmsensor3Dsdk.SetCameraExposureTime(mDev, (short)exp))
                MessageBox.Show($"设置曝光失败");

        }

        private void btnSetLed_Click(object sender, EventArgs e) // led
        {
            if (mDev == IntPtr.Zero) return;
            int led = int.Parse(txtLed.Text);
            this.structuredLight.StructuredLightParam.LedLight = (byte)led;
            for (int i = 0; i < mDevType; i++)
            {
                if (!Bmsensor3Dsdk.SetLEDBrightness(mDev, (byte)led, (byte)i))  // 多投设备时，各个投影设置同样的led亮度，也可以不同
                    MessageBox.Show($"设置投影{i}的led亮度失败");
            }
        }

        private void btnSetGain_Click(object sender, EventArgs e) //gain
        {
            if (mDev == IntPtr.Zero) return;
            float gain = float.Parse(txtGain.Text);
            this.structuredLight.StructuredLightParam.Gain = gain;
            if (!Bmsensor3Dsdk.SetCameraGain(mDev, gain))
                MessageBox.Show("设置增益失败");
        }

        private void btnSetPclProcParams_Click(object sender, EventArgs e)
        {
            mPcdProcessParams = InitPcdProcessParameters(mDev, mDevType); //mHighQualityParams
            this.structuredLight.StructuredLightParam.BmPcdProcessParams = mPcdProcessParams;
            this.structuredLight.StructuredLightParam.BmHighQualityParams = mHighQualityParams;
        }// 设置点云处理算法参数

        private void cbTrigType_SelectedIndexChanged(object sender, EventArgs e) // 修改点云采集触发类型: 软件、硬件
        {
            if (mDev == IntPtr.Zero) return;
            this.structuredLight.StructuredLightParam.BmTriggerMode = (enBmsTriggerMode)(cbTrigSource.SelectedIndex);
            if (!Bmsensor3Dsdk.SetTriggerMode(mDev, (enBmsTriggerMode)(cbTrigSource.SelectedIndex + 1)))
            {
                MessageBox.Show("触发模式设置失败");
                return;
            }

            ckHardTrigReady.Visible = lbTrigLevel.Visible = cbTrigLevel.Visible = cbTrigSource.SelectedIndex != 0; // 硬件触发模式才显示硬件触发电平选择
            mIsHardTrig = cbTrigSource.SelectedIndex != 0; // 是否硬件触发？ 保存
            ckHardTrigReady.Checked = cbTrigSource.SelectedIndex != 0; // 硬件触发过程中，禁止操作ui
        }
        private void cbTrigLevel_SelectedIndexChanged(object sender, EventArgs e) // 修改点云采集硬件触发电平：上升沿，下降沿
        {
            if (mDev == IntPtr.Zero) return;
            if (!Bmsensor3Dsdk.SetHardwareTriggerMode(mDev, (byte)(cbTrigSource.SelectedIndex - 1), (byte)cbTrigLevel.SelectedIndex))
                MessageBox.Show("硬件触发电平失败");
        }

        private void cbOutChannel_SelectedIndexChanged(object sender, EventArgs e) { SetOutSignal(); } // 修改输出通道1，2的配置
        private void cbOutFunction_SelectedIndexChanged(object sender, EventArgs e) { SetOutSignal(); } // 修改指定输出通道信号功能：作为GPIO； 特殊作用：忙信号，曝光信号
        private void cbSignalType_SelectedIndexChanged(object sender, EventArgs e) { SetOutSignal(); } // 修改指定输出通道信号类型：点平 或脉冲信号
        private void cbDefaultLevel_SelectedIndexChanged(object sender, EventArgs e) { SetOutSignal(); } // 修改输出信号的默认电平：低电平或高电平
        private void btnSetPulseWidth_Click(object sender, EventArgs e) { SetOutSignal(); } // 修改输出信号脉冲宽度（脉冲模式时有效）: 10us - 65ms
        void SetOutSignal() // 设置输出信号
        {
            if (mDev == IntPtr.Zero) return;

            BmsIoOutPara IoOutPara;
            IoOutPara.IoType = (byte)cbOutFunction.SelectedIndex;        // gpio 或特殊功能
            IoOutPara.OutType = (byte)cbSignalType.SelectedIndex;       // 电平模式，脉冲模式
            IoOutPara.LelDtLevel = (byte)cbDefaultLevel.SelectedIndex;  // 电平模式：默认电平
            IoOutPara.PwDtLevel = (byte)cbDefaultLevel.SelectedIndex;   // 脉冲模式：默认电平
            IoOutPara.PulseWidth = (short)int.Parse(txtPulseWidth.Text); // 脉冲模式的脉冲宽度
            IoOutPara.Reserved = (byte)0;

            this.structuredLight.StructuredLightParam.BmIoOutPara = IoOutPara;
            if (!Bmsensor3Dsdk.SetIoOutPara(mDev, (byte)cbOutChannel.SelectedIndex, IoOutPara))
                MessageBox.Show("设置设备输出IO失败");

            // 
            gbOutExe.Visible = IoOutPara.IoType == 0; // 输出被配置成通用 io 才可以执行手动输出功能
            pnPulseWidth.Visible = cbSignalType.SelectedIndex == 1; // 脉冲模式才显示脉冲宽度设置

            if (cbSignalType.SelectedIndex == 1) // 脉冲模式，输出信号固定就是脉冲
            {
                cbOutExe.Items.Add("脉冲");
                cbOutExe.SelectedIndex = 2; // 脉冲
            }
            else
            {
                if (cbOutExe.Items.Count > 2) // 电平模式，移除脉冲选项
                    cbOutExe.Items.RemoveAt(2);
            }
            cbOutExe.Enabled = cbSignalType.SelectedIndex != 1;
        }

        private void btnOutExe_Click(object sender, EventArgs e) // 让 io 输出一个脉冲或电平
        {
            if (mDev == IntPtr.Zero) return;

            if (!Bmsensor3Dsdk.StartGernalIoOut(mDev, (byte)cbOutChannel.SelectedIndex, (enBmsGernalIoOutType)cbOutExe.SelectedIndex))
                MessageBox.Show("IO 输出失败");
        }

        void EnabledRoi(bool in_roiEnbaled)
        {
            if (mDev == IntPtr.Zero) return;
            ImageROI roi = new ImageROI();
            if (in_roiEnbaled)
            {
                // 对于多投设备，设置第一个的roi参数即可，共用第一个！
                roi.leftTop_x = int.Parse(txtROIX.Text);
                roi.leftTop_y = int.Parse(txtROIY.Text);
                roi.width = int.Parse(txtROIW.Text);
                roi.height = int.Parse(txtROIH.Text);
                if (!Bmsensor3Dsdk.SetCameraROI(mDev, roi))
                    MessageBox.Show("roi 参数设置失败");
                for (int i = 0; i < mPcdProcessParams.Length; i++)
                {
                    mPcdProcessParams[i].isUseROI = true; // 算法 roi 同步使能
                }
                this.structuredLight.StructuredLightParam.BmRoiParams = roi;
            }
            else
            {
                // 关闭roi
                if (!Bmsensor3Dsdk.StopCameraROI(mDev))
                    MessageBox.Show("停止roi失败");
                for (int i = 0; i < mPcdProcessParams.Length; i++)
                {
                    mPcdProcessParams[i].isUseROI = false; // 算法 roi 同步禁止
                }
            }
            for (int i = 0; i < mPcdProcessParams.Length; i++)
            {
                mPcdProcessParams[i].ROI = roi;
            }
        }

        private void ckROIEnabled_CheckedChanged(object sender, EventArgs e) // 开启/关闭 roi
        {
            if (ckHighSpeed.Checked && ckROIEnabled.Checked)
            {
                MessageBox.Show("高速率模式下无需启用ROI");
                ckROIEnabled.Checked = false;
                return;
            }
            pnROI.Enabled = ckROIEnabled.Checked;
            EnabledRoi(ckROIEnabled.Checked);
        }

        private void btnSetROI_Click(object sender, EventArgs e) // 设置 roi
        {
            EnabledRoi(true);
        }

        private void btnSetRingLamp_Click(object sender, EventArgs e) // 环形光亮度设置，三投设备有效
        {
            if (mDev == IntPtr.Zero) return;
            var lamp = new enSSSensor_RingLampBrightness_s();
            lamp.value = int.Parse(txtRingLamp.Text);
            if (!Bmsensor3Dsdk.Set(mDev, enCmdCode_SSSensor.RingLampBrightness, lamp))
                MessageBox.Show(" 环形光亮度设置失败");
        }

        private void btnGetCalibrationFactor_Click(object sender, EventArgs e) // 获取标定系数
        {
            // 当前 z 平面 标定系数
            float[] listZ = new float[finalPcl.Length];
            for (int i = 0; i < finalPcl.Length; i++)
            {
                listZ[i] = finalPcl[i].Z;  // 单投时也可以用 fusionPcl
            }
            float[] coeff = new float[finalPcl.Length];
            bool ok = Bmsensor3Dsdk.CalcWorldCoeff(mDev, listZ, finalPcl.Length, coeff, 0); // 对于单投设备，点云索引为0
            if (!ok)
                MessageBox.Show("获取Z平面标定系数失败");

            listZ = null;
            coeff = null;
        }

        bool CheckBoxChecked(CheckBox ck)
        {
            if (ck.InvokeRequired)
            {
                Func<bool> actionDelegate = () => { return ck.Checked; };
                ck.BeginInvoke(actionDelegate);
            }
            else
                return ck.Checked;

            return false;
        }

        private void ckHardTrigReady_CheckedChanged(object sender, EventArgs e)
        {
            EnabledUIWhile3DScanning(!(sender as CheckBox).Checked);
        } // ui 参数修改使能
        #endregion

        #region Main: data process

        private void GetSinglePcl(ref Bms3DDataXYZ[] singlePcl, int cloudWidth, int cloudHeight, int cloudLen)  // 保存融合前独立点云
        {
            for (int i = 0; i < mDevType; i++)
            {
                Bms3DDataXYZ[] tempPcl = new Bms3DDataXYZ[cloudLen];
                Array.Copy(singlePcl, i * cloudLen, tempPcl, 0, cloudLen);

                Bmsensor3Dsdk.SavePCDFile(mDev, $@"{Application.StartupPath}\\pcd\test{i + 1}.pcd", tempPcl, cloudWidth, cloudHeight);

                tempPcl = null;
            }
        }

        /// <summary>
        /// 点云，深度图/高度图
        /// </summary>
        /// <param name="pData">相机输出的2D灰度图，如果把设备当作一个普通2D相用，这里即是2D图像</param>
        /// <param name="pFrameInfo">2D图像帧信息</param>
        /// <param name="pSortData">排序后的2D 图像，用于生成点云，高度图</param>
        /// <param name="pUser">用户参数</param>
        private void ImageCallback(IntPtr pData, MV_FRAME_OUT_INFO pFrameInfo, IntPtr pSortData, IntPtr pUser)
        {
            enBmsWorkModeType mode;
            if (!Bmsensor3Dsdk.GetWorkMode(mDev, out mode))
            {
                MessageBox.Show("回调：未能确定设备工作模式！");
                return;
            }
            if (mode == enBmsWorkModeType.WorkMode2D || pSortData == IntPtr.Zero)
                return;   // 2D 不处理！3d 扫描未完成，没有数据，直接返回

            mImgWidth = pFrameInfo.nWidth;
            mImgHeight = pFrameInfo.nHeight;

            DataProcess(pSortData, mImgWidth, mImgHeight);
        }

        void DataProcess(IntPtr pSortData, int imageWidth, int imageHeight)
        {
            if (mIsHardTrig && pSortData == IntPtr.Zero) // 硬件触发扫描提示，软件在点击按钮中提示。
                ShowScanEndTip(false, mImageType == ImageType.CloudImage);

            mPclWidth = mHighSpeedEnabled ? imageWidth / 2 : imageWidth; // 开下采样后宽度变成原来的一半
            mPclHeight = mHighSpeedEnabled ? imageHeight / 2 : imageHeight; // 开下采样后宽度变成原来的一半

            if (mImageType == ImageType.CloudImage)
            {
                int cloudLen = mPclWidth * mPclHeight;
                finalPcl = new Bms3DDataXYZ[cloudLen]; // 融合后的最终点云，必须设置
                Bms3DDataXYZ[] singlePcl = null;
                if (mGenSinglePcl)
                    singlePcl = new Bms3DDataXYZ[cloudLen * mDevType]; // 融合前各自投影的独立点云，多投适用(单投时与融合点云一致)，可选。

                Bmsensor3Dsdk.ComputeWorldPointXYZN(mDev, pSortData, imageHeight, imageWidth, 81, mHighSpeedEnabled, mPcdProcessParams, mPcdProcessParams.Length, finalPcl, singlePcl, mHighQualityEnabled, ref mHighQualityParams);
                Bmsensor3Dsdk.SavePCDFile(mDev, $@"{Application.StartupPath}\\pcd\test.pcd", finalPcl, mPclHeight, mPclWidth);  // 保存最终融合后的点云为 .pcd文件，请用 sensor_3d 软件查看效果！

                if (mGenSinglePcl)
                    GetSinglePcl(ref singlePcl, mPclWidth, mPclHeight, cloudLen);

                singlePcl = null;
                finalPcl = null;
            }
            else
            {
                lock (mDeepLock)
                {
                    if (deep == null)
                        deep = new ushort[imageHeight * imageWidth];
                    Bmsensor3Dsdk.ComputeDeepMap(mDev, pSortData, imageHeight, imageWidth, 81, mHighSpeedEnabled, mPcdProcessParams, mPcdProcessParams.Length, deep, mHighQualityEnabled, ref mHighQualityParams);
                }

                // 显示 16 bit深度图
                InvalidatePicBox(this.pictureBox1);
            }

            if (mIsHardTrig)  // 硬件触发扫描提示，软件在点击按钮中提示。
                ShowScanEndTip(true, mImageType == ImageType.CloudImage);
            else // 软件触发：
            {
                if (mSoftContinueTrig)
                    mContinueEnd = true;
                else if (!mSyncScan) // 异步扫描，释放信号
                    mSem3DEnd.Release();  // 软件触发，释放 3d 完成信号，使用户可以再次点击按钮启动下一次软件触发
            }
        }


        #endregion

        #region Device hotplug
        /// <summary>
        /// 设备热插拔事件
        /// </summary>
        /// <param name="pPara">用户参数</param>
        /// <param name="ConnectEvent">事件 ID </param>
        private void HotplugEventCallback(IntPtr pPara, enBmsConnectEventType ConnectEvent)
        {
            switch (ConnectEvent)
            {
                case enBmsConnectEventType.EventCtlerConnect:
                case enBmsConnectEventType.EventCamConnect:
                    MessageBox.Show("设备已经接入");
                    break;

                case enBmsConnectEventType.EventCtlerNoConnect:
                case enBmsConnectEventType.EventCamNoConnect:
                    MessageBox.Show("设备已经被移除");
                    break;
            }
            btnOpenDevice_Click(null, null);
        }

        #endregion

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            lock (mDeepLock)
            {
                if (deep == null)
                    return;

                DrawGray16(deep, mImgWidth, mImgHeight, e.Graphics, pictureBox1.ClientRectangle);

                deep = null;  // 让 GC 立即回收内存
            }
        }

        private void ckSyncScan_CheckedChanged(object sender, EventArgs e)
        {
            mSyncScan = (sender as CheckBox).Checked;
            // 同步的时候注销回调函数
            Bmsensor3Dsdk.BmsCallBackSortImageDataPtr imageCB = mSyncScan ? null : mImageCB;
            if (!Bmsensor3Dsdk.SetSortImageDataCallBack(mDev, imageCB, IntPtr.Zero))
            {
                MessageBox.Show("设置图像回调函数失败");
            }
        }

        private void MainFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.structuredLight.StructuredLightParam.Save(Application.StartupPath + "\\" + "configParam" + "\\" + this.structuredLight.ConfigParam.SensorName + ".txt");
            }
            finally
            {
               // this.mImageCB -= new Bmsensor3Dsdk.BmsCallBackSortImageDataPtr(ImageCallback);
               // this.mHotplugEventCB -= new Bmsensor3Dsdk.BmsCallBackConnectEventPtr(HotplugEventCallback);
            }
        }


    }
}
