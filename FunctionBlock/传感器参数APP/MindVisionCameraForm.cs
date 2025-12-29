using Common;
using FunctionBlock;
using MotionControlCard;
using MVSDK;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{

    public partial class MindVisionCameraForm : Form
    {
        private ISensor _sensor;
        private string  savePath;
        private int m_hCamera;
        private AcqSource _acqSource;
        public MindVisionCameraForm(ISensor sensor)
        {
            InitializeComponent();
            this._sensor = sensor;
        }
        public MindVisionCameraForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = acqSource.Sensor;
        }
        private void MindVisionCameraForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void 参数设置button_Click(object sender, EventArgs e)
        {
            tSdkCameraDevInfo[] tCameraDevInfoList;
            this.m_hCamera = (int)this._sensor.GetParam(enSensorParamType.MindVision_相机句柄);
            if (m_hCamera > 0)
            {
                MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
                MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);
                MvApi.CameraShowSettingPage(m_hCamera, 1);//1 show ; 0 hide
            }
        }

        private void 软触发button_Click(object sender, EventArgs e)
        {
            if (m_hCamera > 0)
                MvApi.CameraSoftTriggerEx(m_hCamera, 1);
        }
        private void MindVisionCameraForm_Load(object sender, EventArgs e)
        {          
            //tSdkCameraDevInfo[] tCameraDevInfoList;
            this.m_hCamera = (int)this._sensor.GetParam(enSensorParamType.MindVision_相机句柄);
            /////
           //if (m_hCamera > 0)
           // {
           //     MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
           //     MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);
           //     MvApi.CameraShowSettingPage(m_hCamera, 1);//1 show ; 0 hide
           // }
        }
    }
}
