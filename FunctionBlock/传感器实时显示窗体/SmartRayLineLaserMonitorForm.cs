

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

namespace FunctionBlock
{

    public partial class SmartRayLineLaserMonitorForm : Form
    {
        private CancellationTokenSource cts; // 提供了标准的协作式取消
        private FunctionBlock.AcqSource _acqSource;
        private VisualizeView showImage;
        public SmartRayLineLaserMonitorForm() //AcqSource acqSource
        {
            InitializeComponent();
        }

        public SmartRayLineLaserMonitorForm(FunctionBlock.AcqSource acqSource) //AcqSource acqSource
        {
            this._acqSource = acqSource;
            InitializeComponent();
            showImage = new VisualizeView(this.hWindowControl1);      
        }



        public void UpdataView(GraphicUpdateEventArgs e)
        {
            showImage.BackImage = new ImageDataClass((HImage)e.DataContent );
        }
        public void UpdataView(ImageAcqCompleteEventArgs e)
        {
            showImage.BackImage = e.ImageData;
        }
        private void LineLaserMonitorForm_Load(object sender, EventArgs e)
        {
            //DataInteractionClass.getInstance().ImageAcqComplete += new ImageAcqCompleteEventHandler(UpdataView); // 这里不能使用公共的采集事件，不然多个相机会相互冲突
            ((SmartRayLineLaser)this._acqSource.Sensor).SmartSensor.liveImage += new RunResultEventHandler(UpdataView);
        }

        private void LineLaserMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DataInteractionClass.getInstance().ImageAcqComplete -= new ImageAcqCompleteEventHandler(UpdataView);
            ((SmartRayLineLaser)this._acqSource.Sensor).SmartSensor.liveImage -= new RunResultEventHandler(UpdataView);
        }


    }
}
