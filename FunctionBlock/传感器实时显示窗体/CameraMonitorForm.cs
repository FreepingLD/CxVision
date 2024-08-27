

using FunctionBlock;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using View;
using System.Threading.Tasks;
using System.Threading;
using Common;
using HalconDotNet;
using AlgorithmsLibrary;
using MotionControlCard;

namespace FunctionBlock
{

    public partial class CameraMonitorForm : Form
    {
        private CancellationTokenSource cts; // 提供了标准的协作式取消
        private FunctionBlock.AcqSource _acqSource;
        private VisualizeView drawObject;
        public CameraMonitorForm() //AcqSource acqSource
        {
            InitializeComponent();
        }

        public CameraMonitorForm(FunctionBlock.AcqSource acqSource) //AcqSource acqSource
        {
            this._acqSource = acqSource;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1,true);
        }
        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            try
            {
                // 当相机为空，或相机名称不相等时，返回
                if (this._acqSource.Sensor.ConfigParam.SensorName != e.CamName) return;
                double x, y, z;
                if (this._acqSource != null)
                {
                    this._acqSource.GetAxisPosition(enAxisName.X轴, out x);
                    this._acqSource.GetAxisPosition(enAxisName.Y轴, out y);
                    this._acqSource.GetAxisPosition(enAxisName.Z轴, out z);
                    e.ImageData.Grab_X = x;
                    e.ImageData.Grab_Y = y;
                    e.ImageData.Grab_Z = z;
                    this.drawObject.BackImage = e.ImageData;
                }
                else
                    this.drawObject.BackImage = e.ImageData;
            }
            catch
            {
                MessageBox.Show("图像刷新失败");
            }
        }
        private void 实时checkBox_CheckedChanged()
        {
            ImageDataClass imageData;
            cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (cts.IsCancellationRequested) break;
                        imageData = (ImageDataClass)this._acqSource.AcqPointData()[0];
                        this.drawObject.BackImage = imageData.Clone();
                    }
                    catch (Exception ee)
                    {
                    }
                    Thread.Sleep(100);
                }
            });
        }

        public void UpdataView(ImageAcqCompleteEventArgs e)
        {
            drawObject.BackImage = e.ImageData;
        }

        private void DaHengCameraMonitorForm_Load(object sender, EventArgs e)
        {
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
        }

        private void LineLaserMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cts != null)
                cts.Cancel();
            //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
        }


    }
}
