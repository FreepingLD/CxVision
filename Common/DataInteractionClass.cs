using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Common
{
    public class DataInteractionClass
    {

        private Dictionary<Int32, HWindow> windowQueue = new Dictionary<Int32, HWindow>();
        private static DataInteractionClass instance = new DataInteractionClass();
        public event ExcuteCompletedEventHandler MeasureDisplayCompleted;
        public event ExcuteCompletedEventHandler NodeClick;
        public event ExcuteCompletedEventHandler GraphicDisplayEvent;
        public event ExcuteCompletedEventHandler ExcuteCompleted;
        public event ExcuteCompletedEventHandler OutPutData;
        public event ExcuteCompletedEventHandler MetrologyResultEvent;
        public event ExcuteCompletedEventHandler ExcuteUpdataGraphic;
        //public event ImageAcqCompleteEventHandler ImageAcqComplete;
        public event PointCloudAcqCompleteEventHandler PointsCloudAcqComplete;
        public event MetrolegyCompletedEventHandler MetrolegyComplete;
        public event EventHandler MeasureChange;
        public event EventHandler ClearGraphic;
        public event DisplayMeasureResultEventHandler DisplayMetrolegyObject;
        public event EventHandler DisplayCalculateReasult;

        public event DataSendEventHandler DataSend;

        private Dictionary<string, HXLDCont> showXLDDataItems = new Dictionary<string, HXLDCont>();
        private Dictionary<string, object> showModelDataItems = new Dictionary<string, object>();
        private DataInteractionClass()
        {
        }
        public Dictionary<Int32, HWindow> WindowQueue
        {
            get
            {
                return windowQueue;
            }

            set
            {
                windowQueue = value;
            }
        }

        public Dictionary<string, HXLDCont> ShowXLDDataItems
        {
            get
            {
                return showXLDDataItems;
            }

            set
            {
                showXLDDataItems = value;
            }
        }
        public Dictionary<string, object> ShowModelDataItems
        {
            get
            {
                return showModelDataItems;
            }

            set
            {
                showModelDataItems = value;
            }
        }

        public static DataInteractionClass getInstance()
        {
            return instance;
        }

        public void OnDataSend(DataSendEventArgs e)  //colorInfo
        {
            if (this.DataSend != null )
            {
                this.DataSend(this,e);
            }
        }

        /// <summary>
        /// 引发图像采集
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="sensorName"></param>
        public void OnImageAcqComplete(ImageDataClass imageData,string sensorName)  //colorInfo
        {
            //if (this.ImageAcqComplete != null && imageData != null)
            //{
            //    this.ImageAcqComplete(new ImageAcqCompleteEventArgs(imageData, sensorName));
            //}
        }
        /// <summary>
        /// 引发图像采集
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="sensorName"></param>
        public void OnDisplayCalculateReasult()  //colorInfo
        {
            if (this.DisplayCalculateReasult != null)
            {
                this.DisplayCalculateReasult(this,new EventArgs());
            }
        }

        /// <summary>
        /// 引发3D数据采集
        /// </summary>
        /// <param name="pointData"></param>
        /// <param name="sensorName"></param>
        public void OnPointsCloudAcqComplete(HObjectModel3D pointData, string sensorName)  //colorInfo
        {
            if (this.PointsCloudAcqComplete != null && pointData != null)
            {
                this.PointsCloudAcqComplete(this, new PointCloudAcqCompleteEventArgs( sensorName, pointData));
            }
        }

        public void OnExcuteCompleted(object propertyValue)  //colorInfo
        {
            if (this.ExcuteCompleted != null && propertyValue != null)
            {
                this.ExcuteCompleted(this, new ExcuteCompletedEventArgs(propertyValue));
            }
        }
        public void OnNodeClick(object propertyValue)  //colorInfo
        {
            if (this.NodeClick != null && propertyValue != null)
            {
                this.NodeClick(this, new ExcuteCompletedEventArgs(propertyValue));
            }
        }
        public void OnNodeClick(object nodeValue, string nodeName)  //colorInfo
        {
            if (this.NodeClick != null && nodeValue != null)
            {
                this.NodeClick(this,new ExcuteCompletedEventArgs(nodeName, nodeValue));
            }
        }
        /// <summary>
        /// 引发计量完成事件
        /// </summary>
        /// <param name="propertyValue"></param>
        public void OnMetrolegyComplete(MetrolegyCompletedEventArgs propertyValue)  //colorInfo
        { 
            if (this.MetrolegyComplete != null && propertyValue != null)
            {
                this.MetrolegyComplete(propertyValue);
            }
        }
        public void OnMeasureChange()  //colorInfo
        {
            if (this.MeasureChange != null )
            {
                this.MeasureChange(null, new EventArgs());
            }
        }
        public void OnClearGraphic()  //colorInfo
        {
            if (this.ClearGraphic != null)
            {
                this.ClearGraphic(null, new EventArgs());
            }
        }
        public void OnDisplayMetrolegyObject(object propertyValue)  //colorInfo
        {
            if (this.DisplayMetrolegyObject != null)
            {
                this.DisplayMetrolegyObject(new DisplayMetrolegyObjectEventArgs(propertyValue));
            }
        }
        public void OnDisplayMetrolegyObject(object propertyValue,string nodeName)  //colorInfo
        {
            if (this.DisplayMetrolegyObject != null)
            {
                this.DisplayMetrolegyObject(new DisplayMetrolegyObjectEventArgs(propertyValue, nodeName));
            }
        }
        public void OnExcuteCompleted(string propertyName, object propertyValue)
        {
            if (this.ExcuteCompleted != null && propertyName != null)
            {
                this.ExcuteCompleted(this, new ExcuteCompletedEventArgs(propertyName, propertyValue));
            }
        }
        public void OnGraphicDisplayEvent(object propertyValue)
        {
            if (this.GraphicDisplayEvent != null && propertyValue != null)
            {
                this.GraphicDisplayEvent(this, new ExcuteCompletedEventArgs(propertyValue)); //
            }
        }
        public void OnMetrologyResultEvent(object propertyValue)
        {
            if (this.MetrologyResultEvent != null && propertyValue != null)
            {
                this.MetrologyResultEvent(this, new ExcuteCompletedEventArgs(propertyValue));
            }
        }





    }
}
