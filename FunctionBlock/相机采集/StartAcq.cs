using AlgorithmsLibrary;
using Command;
using Common;
using HalconDotNet;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(AcqSourceName))]
    public class StartAcq : BaseFunction, IFunction
    {

        /// <summary>
        /// 这里用采集源的名字而不应该用采集源对象，因为如果使用采集源对象，当采集源更改后，这里不会同步更新，必需选择一次，因为两个地方的对象不是同一个
        /// </summary>
        [DescriptionAttribute("采集源")]
        [DisplayName("采集源")]
        public string AcqSourceName
        {
            get;
            set;
        }

        public ImageAcqParam AcqParam { get; set; }


        public StartAcq()
        {
            this.AcqParam =  new ImageAcqParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name));
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (this.AcqSourceName == null)
                {
                    LoggerHelper.Error(this.name + "->图像采集开始报错,传感器名称为空值");
                    return this.Result;
                }
                this.Result.Succss = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).Sensor.StartTrigger();
                stopwatch.Stop();
                /////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "传感器名称", this.AcqSourceName);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "启动时间", stopwatch.ElapsedMilliseconds.ToString());
                //更新数据
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->图像采集开始报错" + ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->图像采集开始完成;");
            else
                LoggerHelper.Error(this.name + "->图像采集开始失败;");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    return this.name;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = value[0].ToString();
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
                    return true;
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                OnItemDeleteEvent(this, this.name);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "删除节点出错" + ex.ToString());
            }
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }


        #endregion



    }
}
