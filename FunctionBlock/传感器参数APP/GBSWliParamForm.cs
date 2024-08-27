//SmartSharp Demo GUI System for the smartVISwrap Wrapper by Kay Wenzel
//Copyrights by GBS mbH Ilmenau
//Version v0.9
//This Software is only for Demonstration of the basic funktions of the smartVIS DLL and the C# Wrapper
//Some of teh Comments are for inspiration for your own program

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using SmartRemoteProxy;
using System.Threading.Tasks;
using Sensor;
using FunctionBlock;

namespace FunctionBlock
{
    public partial class GBSWliParamForm : Form
    {
        private GbsFaceWliLocal GbsFaceWliLocal;
        private GbsFaceWliRemote GbsFaceWliRemote;
        private string userMode = "本地";
        private bool isDebug = false;

        public GBSWliParamForm(AcqSource acqSource, bool state)
        {
            InitializeComponent();
            this.isDebug = state;
            switch (acqSource.Sensor.GetType().Name)
            {
                default:
                case "GbsFaceWliLocal":
                    this.GbsFaceWliLocal = (GbsFaceWliLocal)acqSource.Sensor;
                    break;
                case "GbsFaceWliRemote":
                    this.GbsFaceWliRemote = (GbsFaceWliRemote)acqSource.Sensor;
                    break;
            }
        }

        private Dictionary<enDataItem, object> listData;
        private double zminScanRange = 0;
        private double zmaxScanRange = 0;
        private double zmin = 0;
        private double zmax = 0;
        private double zPos = 0;
        private uint image_wide = 0;
        private uint image_heigt = 0;
        private uint image_bpp = 0;
        private uint image_rshift = 0;
        private uint image_size = 0;
        private IntPtr image_pointer;
        private uint light_val = 0;
        private ColorPalette grayscal_palette;
        private uint gainMin = 0;
        private uint gainMax = 0;
        private uint gain = 0;
        private uint expMin = 0;
        private uint expMax = 0;
        private uint exposure = 0;
        private string notificate = "";
        //Event System - Use C++ compatible Event System so far
        IntPtr handle_ready = IntPtr.Zero;
        IntPtr handle_error = IntPtr.Zero;
        AutoResetEvent ready_Event = new AutoResetEvent(false);
        AutoResetEvent error_Event = new AutoResetEvent(false);
        WaitHandle[] waitHandles;

        private void Form1_Load(object sender, EventArgs e)
        {
            //MinRangeTextBox.Text = (Z_Value_vScrollBar.Value / 1000000).ToString();
            main_timer.Enabled = false;
            init_system();
            //  wait_th = new Thread(wait_measure_event); //Make a new Thread for waiting of measureing end without CPU using
        }

        private void InitSensorButton_Click(object sender, EventArgs e)
        {
            //this.GbsFaceWliRemote = new GbsFaceWliRemote();
            //init_system();
        }



        private void CheckInitButton_Click(object sender, EventArgs e)
        {
            if (this.userMode == "远程")
                CheckInitButton.Text = this.GbsFaceWliRemote.ServerConfig.IsInit().ToString();
            else
                CheckInitButton.Text = smartVISwrap.SV3D_isInit().ToString();// smartSharpInterface.IsSmartVIS3DInterfaceInitialized().ToString();
        }

        private void GetZRangeButton_Click(object sender, EventArgs e)
        {
            if (this.userMode == "远程")
                this.GbsFaceWliRemote.ScanDeviceConfig.GetCompleteScanRange(out zmin, out zmax);
            else
                smartVISwrap.SV3D_getAbsoluteZrange(out zmin, out zmax);//smartSharpInterface.GetCompleteScanRange(out zmin, out zmax);
            ///////////////////////////
            this.MaxZvalueTextBox.Text = zmax.ToString();
            this.MinZValueTextBox.Text = zmin.ToString();
            ////////////////////////////
            Z_Value_vScrollBar.Minimum = (int)(zmax * -1000000.0);
            Z_Value_vScrollBar.Maximum = (int)(zmin * -1000000.0);
        }


        private void ResetZRangeButton_Click(object sender, EventArgs e)
        {
            if (this.userMode == "远程")
            {
                this.GbsFaceWliRemote.ScanDeviceConfig.ReleaseScanRangeLock();
                this.GbsFaceWliRemote.ScanDeviceConfig.GetCompleteScanRange(out zmin, out zmax);
            }
            else
            {
                smartVISwrap.SV3D_resetZrange();
                smartVISwrap.SV3D_getAbsoluteZrange(out zmin, out zmax);

                //smartSharpInterface.ReleaseScanRangeLock();
                //smartSharpInterface.GetCompleteScanRange(out zmin, out zmax);
            }
            this.MaxZvalueTextBox.Text = zmax.ToString();
            this.MinZValueTextBox.Text = zmin.ToString();
            /////////////////////
            Z_Value_vScrollBar.Minimum = (int)(zmax * -1000000.0); // 
            Z_Value_vScrollBar.Maximum = (int)(zmin * -1000000.0);
        }

        private void Z_Value_vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            double new_pos = (Z_Value_vScrollBar.Value / -1000000.0);
            this.GoToValueTextBox.Text = new_pos.ToString();
            /////////////////////////////////////////////////
            if (userMode == "远程")
            {
                if (this.GbsFaceWliRemote.ServerConfig.IsInit())
                {
                    this.GbsFaceWliRemote.ScanDeviceConfig.GetScanRange(out zminScanRange, out zmaxScanRange);
                    if (zminScanRange <= new_pos && new_pos <= zmaxScanRange)
                        this.GbsFaceWliRemote.ScanDeviceConfig.MoveToScanPosition(new_pos); // 只有在锁定的扫描范围内才能移动
                }
            }
            else
            {
                if (smartVISwrap.SV3D_isInit())
                {
                    smartVISwrap.SV3D_getZrange(out zminScanRange, out zmaxScanRange);
                    if (zminScanRange <= new_pos && new_pos <= zmaxScanRange)
                        smartVISwrap.SV3D_gotoZpos(new_pos);
                }

            }
        }

        //Do Some Continues Events - Just for demonstration
        private void main_timer_Tick(object sender, EventArgs e)
        {
            if (userMode == "远程")
            {
                if (this.GbsFaceWliRemote.ServerConfig.IsInit())
                {
                    this.GbsFaceWliRemote.ScanDeviceConfig.GetScanPosition(out zPos);
                    this.CurrentValueTextBox.Text = zPos.ToString(); // 扫描结束后会自动回到开始扫描位置
                }
            }
            else
            {
                if (smartVISwrap.SV3D_isInit() == true)
                {
                    smartVISwrap.SV3D_getZpos(out zPos);
                    //smartSharpInterface.GetScanPosition(out zPos);
                    this.CurrentValueTextBox.Text = zPos.ToString(); // 扫描结束后会自动回到开始扫描位置
                    get_fast_frame(null, null);
                }
            }
        }


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            main_timer.Enabled = true;
            //smartVISwrap.SV3D_enableLiveImaging(true);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            main_timer.Interval = (int)TimeerNumericUpDown.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userMode == "远程")
                this.GbsFaceWliRemote.MeasProcedureConfig.SelectMeasurementProcedure((uint)MeasureProcedureSelectComboBox.SelectedIndex);
            else
                //smartSharpInterface.SelectMeasurementProcedure((uint)MeasureProcedureSelectComboBox.SelectedIndex);
                smartVISwrap.SV3D_selectMeasuringProcedure((uint)MeasureProcedureSelectComboBox.SelectedIndex);
        }

        private void MeasureButton_Click(object sender, EventArgs e)
        {
            bool smooth = false;
            bool psi = false;
            int speed = 1;
            smooth = SmoothRadioButton.Checked;
            psi = EnablePSI_CheckBox.Checked;
            speed = Convert.ToInt32(MultiplycomboBox.Text); //步长的倍数
            Task.Run(() =>
            {
                if (listData != null)
                    listData.Clear();
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.StartTrigger();
                    this.GbsFaceWliRemote.StopTrigger();
                    listData = this.GbsFaceWliRemote.ReadData();
                }
                else
                {
                    this.GbsFaceWliLocal.StartTrigger();
                    this.GbsFaceWliLocal.StopTrigger();
                    listData = this.GbsFaceWliLocal.ReadData();
                }
            });
        }

        private void Gain_Valuue_hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            gain = (uint)Gain_Valuue_hScrollBar.Value;
            if (userMode == "远程")
                this.GbsFaceWliRemote.CameraConfig.SetCameraGain(gain);
            else
                //smartSharpInterface.SetCameraGain(gain);
                smartVISwrap.SetCameraGain(gain);
            label2.Text = gain.ToString();
        }

        private void Expose_Value_hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            exposure = (uint)c.Value;
            if (userMode == "远程")
                this.GbsFaceWliRemote.CameraConfig.SetCameraExposure(exposure);
            else
                // smartSharpInterface.SetCameraExposure(exposure);
                smartVISwrap.SV3D_setCameraExposure(exposure);
            label3.Text = exposure.ToString();
        }

        private void Light_Value_hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            light_val = (uint)Light_Value_hScrollBar.Value;
            if (userMode == "远程")
                this.GbsFaceWliRemote.LightConfig.SetLuminousIntensity(light_val);
            else
                // smartSharpInterface.SetLuminousIntensity(light_val);
                smartVISwrap.SV3D_setLightingIntensity(light_val);
            label4.Text = light_val.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.listData == null) return;
            show_img si = new show_img((int)image_size, image_wide, image_heigt, (double[])this.listData[enDataItem.Image], (double[])this.listData[enDataItem.Quality]);
            si.Owner = this;
            si.Show();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            main_timer.Enabled = false;
            try
            {
                if (userMode == "远程")
                {
                    if (this.GbsFaceWliRemote != null && this.GbsFaceWliRemote.ServerConfig.IsInit())
                    {
                        // 有些函数的执行顺序要很注意，不然会有意想不到的结果
                        this.GbsFaceWliRemote.DataConfig.OnFrameArrived -= new DataProxy.FrameArrived(get_fast_frame);
                        this.GbsFaceWliRemote.DataConfig.DisableLiveImgStream();
                        this.GbsFaceWliRemote.CameraConfig.ChangeLiveImagingStatus(false);
                        this.GbsFaceWliRemote = null;
                        //if (!this.isDebug)
                        //    this.GbsFaceWliRemote.Disconnect();
                    }
                }
                else
                {
                    smartVISwrap.SV3D_enableLiveImaging(false);
                    this.GbsFaceWliLocal = null;
                    //smartSharpInterface.ChangeLiveImagingStatus(false);
                    //this.GbsFaceWliLocal.OnFrameArrived -= new DataProxy.FrameArrived(get_fast_frame);
                    //if (!this.isDebug)
                    //    this.GbsFaceWliLocal.Disconnect();
                }
            }
            finally
            {
                // Marshal.FreeHGlobal(image_pointer);
                if (wait_th != null)
                    wait_th.Abort();
            }

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        Thread wait_th;
        private void init_system()
        {
            bool is_init = false;
            try
            {
                if (userMode == "远程")
                {
                    if (this.GbsFaceWliRemote.ConfigParam.SensorName == null || this.GbsFaceWliRemote.ConfigParam.SensorName.Trim().Length == 0)
                        is_init = this.GbsFaceWliRemote.Connect(new SensorConnectConfigParam());
                    else
                        is_init = true;
                }
                else
                {
                    if (this.GbsFaceWliLocal.ConfigParam.SensorName == null || this.GbsFaceWliLocal.ConfigParam.SensorName.Trim().Length == 0)
                        is_init = this.GbsFaceWliLocal.Connect(new SensorConnectConfigParam());
                    else
                        is_init = true;
                }
            }
            catch
            {
                MessageBox.Show("smartVIS3D Exception occurred");
            }
            if (is_init != true)
            {
                MessageBox.Show("WLI System nicht bereit!");
                InitSensorButton.Text = "Error";
            }
            else
            {
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.CameraConfig.GetCameraImageSize(out image_wide, out image_heigt);
                    this.GbsFaceWliRemote.CameraConfig.GetCameraImageColorDepthSettings(out image_bpp, out image_rshift);
                    image_size = image_wide * image_heigt;
                }
                else
                {
                    smartVISwrap.SV3D_getImageSize(out image_wide, out image_heigt);
                    smartVISwrap.SV3D_getImageBPP(out image_bpp, out image_rshift);
                    //smartSharpInterface.GetCameraImageSize(out image_wide, out image_heigt);
                    //smartSharpInterface.GetCameraImageColorDepthSettings(out image_bpp, out image_rshift);
                    image_size = image_wide * image_heigt;
                    //image_pointer = Marshal.AllocHGlobal((int)image_size);
                }
                label1.Text = image_wide.ToString() + "x" + image_heigt.ToString() + "\r\n" + image_bpp.ToString() + " " + image_rshift.ToString();
                ////////////////////////////////
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.CameraConfig.ChangeLiveImagingStatus(true);
                    this.GbsFaceWliRemote.LightConfig.GetLuminousIntensity(out light_val);
                }
                else
                {
                    smartVISwrap.SV3D_enableLiveImaging(true);
                    smartVISwrap.SV3D_getLightingIntensity(out light_val);
                    //smartSharpInterface.ChangeLiveImagingStatus(true);
                    //smartSharpInterface.GetLuminousIntensity(out light_val);
                }
                Light_Value_hScrollBar.Value = (int)light_val;
                label4.Text = light_val.ToString();

                grayscal_palette = creat_grayscall_palette();
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.ScanDeviceConfig.GetScanPosition(out zPos); // 获取锁定的扫描范围
                    this.GbsFaceWliRemote.ScanDeviceConfig.GetCompleteScanRange(out zmin, out zmax);//获取最大、最小范围
                    this.GbsFaceWliRemote.ScanDeviceConfig.GetScanRange(out zminScanRange, out zmaxScanRange);
                }
                else
                {
                    smartVISwrap.SV3D_getZpos(out zPos);
                    smartVISwrap.SV3D_getAbsoluteZrange(out zmin, out zmax);
                    smartVISwrap.SV3D_getZrange(out zminScanRange, out zmaxScanRange);
                    //smartSharpInterface.GetScanPosition(out zPos); // 获取锁定的扫描范围
                    //smartSharpInterface.GetCompleteScanRange(out zmin, out zmax);//获取最大、最小范围
                    //smartSharpInterface.GetScanRange(out zminScanRange, out zmaxScanRange);
                }
                this.MaxZvalueTextBox.Text = zmaxScanRange.ToString();
                this.MinZValueTextBox.Text = zminScanRange.ToString();
                this.GoToValueTextBox.Text = zPos.ToString();
                Z_Value_vScrollBar.Minimum = (int)(zmax * -1000000.0);
                Z_Value_vScrollBar.Maximum = (int)(zmin * -1000000.0);
                int barPos = -(int)(zPos * 1000000);
                if (Z_Value_vScrollBar.Minimum <= barPos && barPos <= Z_Value_vScrollBar.Maximum)
                    Z_Value_vScrollBar.Value = barPos;
                ////////////////
                get_all_Procedures();
                bool res;
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.DataConfig.OnFrameArrived += new SmartRemoteProxy.DataProxy.FrameArrived(get_fast_frame);
                    this.GbsFaceWliRemote.DataConfig.EnableLiveImgStream();
                }
                else
                {
                    //Measuring done Event System
                    //handle_ready = smartVISwrap.CreateEvent(IntPtr.Zero, false, false, "ReadyHandler");
                    //handle_error = smartVISwrap.CreateEvent(IntPtr.Zero, false, false, "ErrorHandler");
                    //ready_Event.Handle = handle_ready;  // Set the Handle to the event from
                    //error_Event.Handle = handle_error;
                    //waitHandles = new WaitHandle[2];
                    ////Put it in our array for WaitAny()
                    //waitHandles[0] = ready_Event;
                    //waitHandles[1] = error_Event;
                    //res = smartVISwrap.SV3D_setResultReadyNotification(handle_ready, handle_error);
                    //wait_th.Start(); //Start the Wait Thread
                    //this.GbsFaceWliLocal.OnFrameArrived += new SmartRemoteProxy.DataProxy.FrameArrived(get_fast_frame);
                }
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.CameraConfig.GetCameraGainValuesRange(out gainMin, out gainMax);
                    this.GbsFaceWliRemote.CameraConfig.GetCameraGain(out gain);
                }
                else
                {
                    smartVISwrap.SV3D_getCameraGainRange(out gainMin, out gainMax);
                    smartVISwrap.SV3D_getCameraGain(out gain);
                    //smartSharpInterface.GetCameraGainValuesRange(out gainMin, out gainMax);
                    //smartSharpInterface.GetCameraGain(out gain);
                }
                Gain_Valuue_hScrollBar.Minimum = (int)gainMin;
                Gain_Valuue_hScrollBar.Maximum = (int)gainMax;
                Gain_Valuue_hScrollBar.Value = (int)gain;

                label2.Text = gain.ToString();
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.CameraConfig.GetCameraExposureValuesRange(out expMin, out expMax);
                    this.GbsFaceWliRemote.CameraConfig.GetCameraExposure(out exposure);
                }
                else
                {
                    smartVISwrap.SV3D_getCameraExposureRange(out expMin, out expMax);
                    smartVISwrap.SV3D_getCameraExposure(out exposure);
                    //smartSharpInterface.GetCameraExposureValuesRange(out expMin, out expMax);
                    //smartSharpInterface.GetCameraExposure(out exposure);
                }
                if (userMode == "远程")
                {
                    this.RoughRadioButton.Checked = !this.GbsFaceWliRemote.EnableEPSIMode_p;
                    this.SmoothRadioButton.Checked = this.GbsFaceWliRemote.EnableEPSIMode_p;
                    this.EnablePSI_CheckBox.Checked = this.GbsFaceWliRemote.EnablePSIMode_p;
                    this.MultiplycomboBox.Text = this.GbsFaceWliRemote.ScanStepSizeMultiplier.ToString();
                }
                else
                {
                    this.RoughRadioButton.Checked = !this.GbsFaceWliLocal.EnableEPSIMode_p;
                    this.SmoothRadioButton.Checked = this.GbsFaceWliLocal.EnableEPSIMode_p;
                    this.EnablePSI_CheckBox.Checked = this.GbsFaceWliLocal.EnablePSIMode_p;
                    this.MultiplycomboBox.Text = this.GbsFaceWliLocal.ScanStepSizeMultiplier.ToString();
                }
                c.Minimum = (int)expMin;
                c.Maximum = (int)expMax;
                c.Value = (int)exposure;
                label3.Text = exposure.ToString();
                this.InitSensorButton.Text = "Ready";
                this.InitSensorButton.BackColor = Color.Green;
                UpdateObjectivesComboBox();  //更新镜头对话框


                main_timer.Enabled = true;
            }
        }

        private byte[] get_byte_frame()
        {
            byte[] image_data = new byte[image_size];
            bool get_img = smartVISwrap.SV3D_getFrame(image_pointer, image_size);
            Marshal.Copy(image_pointer, image_data, 0, (int)image_size);
            return image_data;
        }
        private void get_fast_frame(object sender, ImgDataEventArgs e)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap((int)image_wide, (int)image_heigt, PixelFormat.Format8bppIndexed);
            bmp.Palette = grayscal_palette;

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);
            bool get_img;
            if (userMode == "远程")
            {
                //byte[] data;
                //this.GbsFaceWli.DataConfig.GetCameraImageData(out data);
                Marshal.Copy(e.data, 0, bmpData.Scan0, e.size); // 给这个地址中赋值即可
            }
            else
                get_img = smartVISwrap.SV3D_getFrame(bmpData.Scan0, image_size);

            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            //Return the bitmap 
            this.pictureBox1.Image = bmp;
        }
        private Bitmap get_fast_frame()
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap((int)image_wide, (int)image_heigt, PixelFormat.Format8bppIndexed);
            bmp.Palette = grayscal_palette;

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);
            bool get_img;
            if (userMode == "远程")
            {
                byte[] data;
                this.GbsFaceWliRemote.DataConfig.GetCameraImageData(out data);
                Marshal.Copy(data, 0, bmpData.Scan0, data.Length); // 给这个地址中赋值即可
            }
            else
            {
                //byte[] data;
                byte[] data = new byte[image_size];
                uint size;
                smartVISwrap.SV3D_getFrame(image_pointer, image_size);
                Marshal.Copy(image_pointer, data, 0, (int)image_size);
                //smartSharpInterface.GetCameraImageData(out data, out size);
                //Marshal.Copy(data, 0, bmpData.Scan0, data.Length); // 给这个地址中赋值即可
            }



            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            //Return the bitmap 
            return bmp;
        }

        private void wait_measure_event()
        {
            while (true)
            {
                int waitResult = WaitHandle.WaitAny(waitHandles, -1, false);

                if (waitResult == 0)
                {
                    notificate = "measure_ready";
                    MeasureButton.BeginInvoke((MethodInvoker)delegate () { MeasureButton.Text = "Measure Ready"; });

                    /*MethodInvoker inv = delegate 
                    {
                      button7.Text = "Measure Ready"; 
                    }
                    Invoke(inv);*/
                }

                if (waitResult == 1)
                {
                    notificate = "measure_error";
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        public ColorPalette creat_grayscall_palette()
        {
            Bitmap bmp = new Bitmap(10, 10, PixelFormat.Format8bppIndexed);
            ColorPalette cp = bmp.Palette;
            Color[] cm_entries = cp.Entries;
            for (int i = 0; i < 256; i++)
            {
                Color b = new Color();
                b = Color.FromArgb((byte)i, (byte)i, (byte)i);
                cm_entries[i] = b;
            }

            return cp;
        }

        public void get_all_Procedures()
        {
            uint mpn = 0;
            if (userMode == "远程")
                this.GbsFaceWliRemote.MeasProcedureConfig.GetMeasurementProceduresNumber(out mpn);
            else
                //smartSharpInterface.GetMeasurementProceduresNumber(out mpn); 
                smartVISwrap.SV3D_getMeasuringProcedureNumber(out mpn);
            MeasureProcedureSelectComboBox.Items.Clear();
            for (uint i = 0; i < mpn; i++)
            {
                if (userMode == "远程")
                {
                    string cur_name;
                    this.GbsFaceWliRemote.MeasProcedureConfig.GetMeasurementProcedureName(i, out cur_name);
                    MeasureProcedureSelectComboBox.Items.Add(cur_name);
                }
                else
                {
                    string proc_name = "abcdefghijklmnopqrstuvwxyz1234567890";
                    uint str_count = (uint)proc_name.Length;
                    IntPtr str_pointer = Marshal.StringToHGlobalAnsi(proc_name);
                    smartVISwrap.SV3D_getMeasuringProcedureName(i, str_pointer, ref str_count);
                    //smartSharpInterface.GetMeasurementProcedureName(i, out cur_name,out size);
                    string cur_name = Marshal.PtrToStringAnsi(str_pointer);
                    MeasureProcedureSelectComboBox.Items.Add(cur_name);
                    Marshal.FreeHGlobal(str_pointer);
                }
            }
            uint curr_index = 0;
            if (userMode == "远程")
                this.GbsFaceWliRemote.MeasProcedureConfig.GetMeasurementProcedureID(out curr_index);
            else
                //smartSharpInterface.GetMeasurementProcedureID(out curr_index);
                smartVISwrap.SV3D_getSelectedMeasuringProcedureIndex(out curr_index);
            //////////////////
            MeasureProcedureSelectComboBox.Text = MeasureProcedureSelectComboBox.Items[(int)curr_index].ToString();
        }


        private void button11_Click(object sender, EventArgs e)
        {
            //smartTester st = new smartTester();
            //st.Show();
        }


        private void ResetMaxValueButton_Click(object sender, EventArgs e)
        {
            if (userMode == "远程")
            {
                this.GbsFaceWliRemote.ScanDeviceConfig.ReleaseScanRangeMaximumPositionLock();
                this.GbsFaceWliRemote.ScanDeviceConfig.GetCompleteScanRange(out zmin, out zmax);
            }
            else
            {
                smartVISwrap.SV3D_resetZmax();
                smartVISwrap.SV3D_getAbsoluteZrange(out zmin, out zmax);
                //smartSharpInterface.ReleaseScanRangeMaximumPositionLock();
                //smartSharpInterface.GetCompleteScanRange(out zmin, out zmax);
            }
            this.MaxZvalueTextBox.Text = zmax.ToString();
            Z_Value_vScrollBar.Minimum = (int)(zmax * -1000000.0);
        }

        private void SetMaxValuebutton_Click(object sender, EventArgs e)
        {
            double setz = Z_Value_vScrollBar.Value / -1000000.0;
            if (this.userMode == "远程")
                this.GbsFaceWliRemote.ScanDeviceConfig.LockScanRangeMaximumPosition(setz);
            else
                //smartSharpInterface.LockScanRangeMaximumPosition(setz);
                smartVISwrap.SV3D_setZmax(setz);
            this.MaxZvalueTextBox.Text = setz.ToString();
            //////////////////
            Z_Value_vScrollBar.Minimum = Z_Value_vScrollBar.Value;
        }

        private void ResetMinValuebutton_Click(object sender, EventArgs e)
        {
            if (userMode == "远程")
            {
                this.GbsFaceWliRemote.ScanDeviceConfig.ReleaseScanRangeMinimumPositionLock();
                this.GbsFaceWliRemote.ScanDeviceConfig.GetCompleteScanRange(out zmin, out zmax);
            }
            else
            {
                smartVISwrap.SV3D_resetZmin();
                smartVISwrap.SV3D_getAbsoluteZrange(out zmin, out zmax);
                //smartSharpInterface.ReleaseScanRangeMinimumPositionLock();
                //smartSharpInterface.GetCompleteScanRange(out zmin, out zmax);
            }
            /////////////////
            this.MinZValueTextBox.Text = zmin.ToString();
            Z_Value_vScrollBar.Maximum = (int)(zmin * -1000000.0);
        }

        private void SetMinValuebutton_Click(object sender, EventArgs e)
        {
            double setz = Z_Value_vScrollBar.Value / -1000000.0;
            if (this.userMode == "远程")
                this.GbsFaceWliRemote.ScanDeviceConfig.LockScanRangeMinimumPosition(setz);
            else
                //smartSharpInterface.LockScanRangeMinimumPosition(setz);
                smartVISwrap.SV3D_setZmin(setz);
            ///////////
            this.MinZValueTextBox.Text = setz.ToString();
            Z_Value_vScrollBar.Maximum = Z_Value_vScrollBar.Value;
        }

        private void GoToCurrentValueButton_Click(object sender, EventArgs e)
        {
            double newPos = Convert.ToDouble(this.GoToValueTextBox.Text);
            if (this.userMode == "远程")
            {
                if (this.GbsFaceWliRemote.ServerConfig.IsInit())
                {
                    this.GbsFaceWliRemote.ScanDeviceConfig.GetScanRange(out zminScanRange, out zmaxScanRange);
                    if (zminScanRange <= newPos && newPos <= zmaxScanRange)
                        this.GbsFaceWliRemote.ScanDeviceConfig.MoveToScanPosition(newPos); // 在移动之前一定要释放扫描锁
                    // 移动条到当前位置
                    int barPos = -(int)(newPos * 1000000);
                    if (barPos > Z_Value_vScrollBar.Maximum || barPos < Z_Value_vScrollBar.Minimum)
                    {
                        MessageBox.Show("Out of Bounds!", "Error");
                    }
                    else
                    {
                        Z_Value_vScrollBar.Value = barPos;
                    }
                }
            }
            else
            {
                if (smartVISwrap.SV3D_isInit() == true)
                {
                    smartVISwrap.SV3D_getZrange(out zminScanRange, out zmaxScanRange);
                    //smartSharpInterface.GetScanRange(out zminScanRange, out zmaxScanRange);
                    if (newPos < zmin || newPos > zmax)
                        newPos = zmin;
                    //smartSharpInterface.MoveToScanPosition(newPos);
                    smartVISwrap.SV3D_gotoZpos(newPos);
                    int barPos = -(int)(newPos * 1000000);
                    if (barPos > Z_Value_vScrollBar.Maximum || barPos < Z_Value_vScrollBar.Minimum)
                    {
                        MessageBox.Show("Out of Bounds!", "Error");
                    }
                    else
                    {
                        Z_Value_vScrollBar.Value = barPos;
                    }
                }
            }
        }

        private void HighPrecisionMode_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (userMode == "远程")
            {
                if (this.HighPrecisionMode_CheckBox.Checked)
                {
                    int step = int.Parse(this.平滑值domainUpDown.Text);
                    this.GbsFaceWliRemote.MeasurementConfig.EnableHighPrecisionMode(step);
                }
                else
                {
                    this.GbsFaceWliRemote.MeasurementConfig.DisableHighPrecisionMode();
                }
            }
            else
            {
                if (this.HighPrecisionMode_CheckBox.Checked)
                {
                    int step = int.Parse(this.平滑值domainUpDown.Text);
                    //smartSharpInterface.EnableHighPrecisionMode(step);
                    smartVISwrap.SV3D_enableHighPrecisionMode(step);
                }
                else
                {
                    //smartSharpInterface.DisableHighPrecisionMode();
                    smartVISwrap.SV3D_disableHighPrecisionMode();
                }
            }
        }

        private void 平滑值domainUpDown_TextChanged(object sender, EventArgs e)
        {
            HighPrecisionMode_CheckBox_CheckedChanged(null, null);
        }

        private void EnablePSI_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (userMode == "远程")
            {
                if (this.EnablePSI_CheckBox.Checked)
                {
                    this.GbsFaceWliRemote.EnablePSIMode_p = false; // 这个参数不能设置为true?
                    this.GbsFaceWliRemote.MeasurementConfig.EnablePhaseUnwrappingAlgorithmUsage();
                    this.UnwrappingMode_CheckBox.Enabled = true;
                    this.UnwrappingMode_CheckBox.Checked = true;
                }
                else
                {
                    this.GbsFaceWliRemote.EnablePSIMode_p = false; // 这个参数不能设置为true?
                    this.GbsFaceWliRemote.MeasurementConfig.DisablePhaseUnwrappingAlgorithmUsage();
                    this.UnwrappingMode_CheckBox.Enabled = false;
                    this.UnwrappingMode_CheckBox.Checked = false;
                }
                this.GbsFaceWliRemote.MeasurementConfig.SetMeasurementProcedureSettings(true, this.GbsFaceWliRemote.EnableEPSIMode_p, this.GbsFaceWliRemote.EnablePSIMode_p, this.GbsFaceWliRemote.ScanStepSizeMultiplier);
            }
            else
            {
                if (this.EnablePSI_CheckBox.Checked)
                {
                    this.GbsFaceWliRemote.EnablePSIMode_p = false;
                    smartVISwrap.SV3D_enablePhaseUnwrappingAlgorithmUsage();
                    this.UnwrappingMode_CheckBox.Enabled = true;
                    this.UnwrappingMode_CheckBox.Checked = true;
                }
                else
                {
                    this.GbsFaceWliRemote.EnablePSIMode_p = false;
                    smartVISwrap.SV3D_disablePhaseUnwrappingAlgorithmUsage();
                    this.UnwrappingMode_CheckBox.Enabled = false;
                    this.UnwrappingMode_CheckBox.Checked = false;
                }
                //smartSharpInterface.SetMeasurementProcedureSettings(true, this.GbsFaceWliRemote.EnableEPSIMode_p, this.GbsFaceWliRemote.EnablePSIMode_p, this.GbsFaceWliRemote.ScanStepSizeMultiplier);
                smartVISwrap.SV3D_configMeasurement(true, this.GbsFaceWliRemote.EnableEPSIMode_p, this.GbsFaceWliRemote.EnablePSIMode_p, (int)this.GbsFaceWliRemote.ScanStepSizeMultiplier);
            }
        }


        private void RoughRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (RoughRadioButton.Checked)
            {

                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.EnableEPSIMode_p = false;
                    this.GbsFaceWliRemote.EnablePSIMode_p = false; // 这个参数不能设置为true?
                    this.GbsFaceWliRemote.MeasurementConfig.SetMeasurementProcedureSettings(true, this.GbsFaceWliRemote.EnableEPSIMode_p, this.GbsFaceWliRemote.EnablePSIMode_p, this.GbsFaceWliRemote.ScanStepSizeMultiplier);
                }

                else
                {
                    this.GbsFaceWliLocal.EnableEPSIMode_p = false;
                    this.GbsFaceWliLocal.EnablePSIMode_p = false; // 这个参数不能设置为true?
                    //smartSharpInterface.SetMeasurementProcedureSettings(true, this.GbsFaceWliLocal.EnableEPSIMode_p, this.GbsFaceWliLocal.EnablePSIMode_p, this.GbsFaceWliLocal.ScanStepSizeMultiplier);
                    smartVISwrap.SV3D_configMeasurement(true, this.GbsFaceWliLocal.EnableEPSIMode_p, this.GbsFaceWliLocal.EnablePSIMode_p, (int)this.GbsFaceWliLocal.ScanStepSizeMultiplier);
                }

            }
        }

        private void SmoothRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SmoothRadioButton.Checked)
            {
                this.GbsFaceWliRemote.EnableEPSIMode_p = true;
                this.GbsFaceWliRemote.EnablePSIMode_p = false; // 这个参数不能设置为true?
                if (userMode == "远程")
                    this.GbsFaceWliRemote.MeasurementConfig.SetMeasurementProcedureSettings(true, this.GbsFaceWliRemote.EnableEPSIMode_p, this.GbsFaceWliRemote.EnablePSIMode_p, this.GbsFaceWliRemote.ScanStepSizeMultiplier);
                else
                    //smartSharpInterface.SetMeasurementProcedureSettings(true, this.GbsFaceWliRemote.EnableEPSIMode_p, this.GbsFaceWliRemote.EnablePSIMode_p, this.GbsFaceWliRemote.ScanStepSizeMultiplier);
                    smartVISwrap.SV3D_configMeasurement(true, this.GbsFaceWliRemote.EnableEPSIMode_p, this.GbsFaceWliRemote.EnablePSIMode_p, (int)this.GbsFaceWliRemote.ScanStepSizeMultiplier);
            }
        }

        private void UnwrappingMode_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (userMode == "远程")
            //{
            //    if (this.EnablePSI_CheckBox.Checked)
            //    {
            //        this.GbsFaceWli.EnablePSIMode_p = true;
            //        this.GbsFaceWli.MeasurementConfig.EnablePhaseUnwrappingAlgorithmUsage(); // 只应用于相位展开模式
            //        this.UnwrappingMode_CheckBox.Enabled = true;
            //        this.UnwrappingMode_CheckBox.Checked = true;
            //    }
            //    else
            //    {
            //        this.GbsFaceWli.EnablePSIMode_p = false;
            //        this.GbsFaceWli.MeasurementConfig.DisablePhaseUnwrappingAlgorithmUsage();
            //        this.UnwrappingMode_CheckBox.Enabled = false;
            //        this.UnwrappingMode_CheckBox.Checked = false;
            //    }
            //    this.GbsFaceWli.MeasurementConfig.SetMeasurementProcedureSettings(true, this.GbsFaceWli.EnableEPSIMode_p, this.GbsFaceWli.EnablePSIMode_p, this.GbsFaceWli.ScanStepSizeMultiplier);
            //}
            //else
            //{
            //    if (this.EnablePSI_CheckBox.Checked)
            //    {
            //        this.GbsFaceWli.EnablePSIMode_p = true;
            //        smartVISwrap.SV3D_enablePhaseUnwrappingAlgorithmUsage();
            //        this.UnwrappingMode_CheckBox.Enabled = true;
            //        this.UnwrappingMode_CheckBox.Checked = true;
            //    }
            //    else
            //    {
            //        this.GbsFaceWli.EnablePSIMode_p = false;
            //        smartVISwrap.SV3D_disablePhaseUnwrappingAlgorithmUsage();
            //        this.UnwrappingMode_CheckBox.Enabled = false;
            //        this.UnwrappingMode_CheckBox.Checked = false;
            //    }
            //    smartSharpInterface.SetMeasurementProcedureSettings(true, this.GbsFaceWli.EnableEPSIMode_p, this.GbsFaceWli.EnablePSIMode_p, this.GbsFaceWli.ScanStepSizeMultiplier);
            //}
        }

        private void MultiplycomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MultiplycomboBox.SelectedItem == null) return;
            uint step = uint.Parse(MultiplycomboBox.SelectedItem.ToString());
            if (userMode == "远程")
            {
                this.GbsFaceWliRemote.MeasurementConfig.SetScanStepSizeMultiplierID((uint)MultiplycomboBox.SelectedIndex);
                this.GbsFaceWliRemote.ScanStepSizeMultiplier = step;
            }
            else
            {
                smartVISwrap.SV3D_setMultiplier((uint)MultiplycomboBox.SelectedIndex);
                //smartSharpInterface.SetScanStepSizeMultiplierID((uint)MultiplycomboBox.SelectedIndex);
                this.GbsFaceWliLocal.ScanStepSizeMultiplier = (int)step;
            }
        }


        List<string> GetObjectivesList()
        {
            List<string> objectivesList = new List<string>();
            uint objectivesNumber = 0;
            int functionExecutionErrorCode = 0;
            if (userMode == "远程")
            {
                this.GbsFaceWliRemote.ObjectiveConfig.GetSupportedObjectivesNumber(out objectivesNumber);
                if (objectivesNumber == 0)
                    MessageBox.Show("Couldn't get objectives number. ErrorCode: " + functionExecutionErrorCode.ToString(), "Dialog error", MessageBoxButtons.OK);
            }
            else
            {
                //if (0x00000000 != (functionExecutionErrorCode = smartSharpInterface.GetSupportedObjectivesNumber(out objectivesNumber)))
                if (!smartVISwrap.SV3D_getSupportedLensNumber(out objectivesNumber))
                    MessageBox.Show("Couldn't get objectives number. ErrorCode: " + functionExecutionErrorCode.ToString(), "Dialog error", MessageBoxButtons.OK); //
            }

            for (uint objectiveListEntryID = 0; objectiveListEntryID < objectivesNumber; ++objectiveListEntryID)
            {
                string objectiveName = "";
                uint objectiveNameSize;
                if (userMode == "远程")
                {
                    this.GbsFaceWliRemote.ObjectiveConfig.GetObjectiveName(objectiveListEntryID, out objectiveName);
                }
                else
                {
                    //if (0x00000000 != (functionExecutionErrorCode = smartSharpInterface.GetObjectiveName(objectiveListEntryID, out objectiveName, out objectiveNameSize)))
                    //{
                    //    MessageBox.Show("Couldn't get objective name. ErrorCode: " + functionExecutionErrorCode.ToString(), "Dialog error", MessageBoxButtons.OK);
                    //    return new List<string>();
                    //}
                    string cur_name = "abcdefghijklmnopqrstuvwxyz1234567890";
                    objectiveNameSize = (uint)cur_name.Length;
                    IntPtr str_pointer = Marshal.StringToHGlobalAnsi(cur_name);
                    smartVISwrap.SV3D_getLensName(objectiveListEntryID, str_pointer, ref objectiveNameSize);
                    objectiveName = Marshal.PtrToStringAnsi(str_pointer);
                    Marshal.FreeHGlobal(str_pointer);
                }
                objectivesList.Add(objectiveName);
            }
            return objectivesList;
        }
        void UpdateObjectivesComboBox()
        {
            objectivesControl.Items.Clear();

            List<string> objectivesList = GetObjectivesList();

            for (int objectiveEntryID = 0; objectiveEntryID < objectivesList.Count(); ++objectiveEntryID)
                objectivesControl.Items.Insert(objectiveEntryID, objectivesList[objectiveEntryID]);

            UpdateSelectedObjectiveInGUI();
        }
        void UpdateSelectedObjectiveInGUI()
        {
            int functionExecutionErrorCode = 0;

            uint currentObjectiveID = 0;
            if (userMode == "远程")
            {
                this.GbsFaceWliRemote.ObjectiveConfig.GetObjectiveID(out currentObjectiveID);
                if (currentObjectiveID < 0)
                    return;
            }
            else
            {
                if (!smartVISwrap.SV3D_getSelectedLensID(out currentObjectiveID))
                {
                    MessageBox.Show("Couldn't get currently used objective id. ErrorCode: " + functionExecutionErrorCode.ToString(), "Dialog error", MessageBoxButtons.OK);
                    return;
                }
            }
            //////////////////
            objectivesControl.SelectedIndex = (int)currentObjectiveID;
            string currentObjectiveName = "";
            uint currentObjectiveNameSize;
            string cur_name = "abcdefghijklmnopqrstuvwxyz1234567890";
            currentObjectiveNameSize = (uint)cur_name.Length;
            IntPtr str_pointer = Marshal.StringToHGlobalAnsi(cur_name);
            smartVISwrap.SV3D_getLensName(currentObjectiveID, str_pointer, ref currentObjectiveNameSize);
            currentObjectiveName = Marshal.PtrToStringAnsi(str_pointer);
            Marshal.FreeHGlobal(str_pointer);
            /////////////////
            if (-1 != currentObjectiveName.IndexOf("Nikon"))
                companyControl.Text = "Nikon";
            else if (-1 != currentObjectiveName.IndexOf("Olympus"))
                companyControl.Text = "Olympus";
            ////////////////////
            int xPositionInObjectiveName = currentObjectiveName.IndexOf("x"); // X在镜头名称中的位置
            string tmpObjectiveNameSubstring = currentObjectiveName.Substring(0, xPositionInObjectiveName + 1);
            string magnification = tmpObjectiveNameSubstring.Substring(tmpObjectiveNameSubstring.LastIndexOf(" ")); // 倍率
            magnificationControl.Text = magnification;
            ////////////////
            double metricResolutionX = 0.0, metricResolutionY = 0.0;
            if (!smartVISwrap.SV3D_getResolution(out metricResolutionX, out metricResolutionY))
            {
                MessageBox.Show("Couldn't get metric resolution of current objective. ErrorCode: " + functionExecutionErrorCode.ToString(), "Dialog error", MessageBoxButtons.OK);
                return;
            }
            pixelResolutionControl.Text = Math.Round(metricResolutionX, 12).ToString("0.000000000000");
            this.filedLabe.Text = (this.image_wide * metricResolutionX).ToString("0.000") + "X" + (this.image_heigt * metricResolutionY).ToString("0.000");
        }

        private void objectivesControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userMode == "远程")
            {
                if (!this.GbsFaceWliRemote.ServerConfig.IsInit()) return;
                int selectedObjectiveID = objectivesControl.SelectedIndex;
                this.GbsFaceWliRemote.ObjectiveConfig.SelectObjective((uint)selectedObjectiveID);
            }
            else
            {
                if (!smartVISwrap.SV3D_isInit()) return;
                //if (!smartSharpInterface.IsSmartVIS3DInterfaceInitialized()) return;
                if (objectivesControl.SelectedIndex == -1) return;
                int selectedObjectiveID = objectivesControl.SelectedIndex;
                int functionExecutionErrorCode = 0;
                //if (0x00000000 != (functionExecutionErrorCode = smartSharpInterface.SelectObjective((uint)selectedObjectiveID)))
                if (!smartVISwrap.SV3D_selectLens((uint)selectedObjectiveID))
                {
                    MessageBox.Show("Couldn't select objective. ErrorCode: " + functionExecutionErrorCode.ToString(), "Dialog error", MessageBoxButtons.OK);
                    return;
                }
            }
            UpdateSelectedObjectiveInGUI();
        }


    }
}
