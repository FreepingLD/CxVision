using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Windows.Forms;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using Sensor;
using System.IO;
using System.Linq;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty( nameof(ImageData))]
    public class ReadImage : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData = null;
        private int index = 0;

        [DisplayName("输出图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass ImageData { get => _imageData; set => _imageData = value; }

        public ReadImageParam ReadParam { get; set; }


        public ReadImage()
        {
           // FunctionManage.ImageSourceList.Add(this);
            this.ReadParam = new ReadImageParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }



        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                if (param == null) this.index = 0; // param不能为空
                else
                {
                    if (param.Length == 1)
                        int.TryParse(param[0].ToString(), out this.index);
                    if (param.Length == 2)
                        int.TryParse(param[1].ToString(), out this.index);
                }
                if (this.ReadParam.FilePath.Count > this.index)
                    this._imageData = this.ReadParam.ReadImage(this.ReadParam.FilePath[this.index]);// 读取第I个路径的图像
                else
                    this._imageData = this.ReadParam.ReadImage(this.ReadParam.FilePath[0]);
                this.ImageData.CamName = "Cam1";
                this.ImageData.CamParams.SensorName = "Cam1";
                if (this._imageData != null)
                {
                    this._imageData.Grab_X = this.ReadParam.GrabPoint.X;
                    this._imageData.Grab_Y = this.ReadParam.GrabPoint.Y;
                    this._imageData.Grab_Theta = this.ReadParam.GrabPoint.Angle;
                    ////////////////////////////////////////////////////
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "图像宽度", this._imageData.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "图像高度", this._imageData.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this._imageData.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this._imageData.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", this._imageData.Grab_Theta);
                    this.Result.Succss = true;
                    OnExcuteCompleted(this._imageData.CamName, this._imageData?.ViewWindow, this.name, this._imageData);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功：");
            else
                LoggerHelper.Error(this.name + "执行失败：");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName.Trim()) // 这个类只作一个图像输出
            {
                case nameof(this.Name):
                    return this.name;
                case nameof(this.ImageData):
                default:
                    return this._imageData;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
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
                LoggerHelper.Info("从图像源集合中删除图像源：" + this.name);
                //if (FunctionManage.ImageSourceList.Contains(this))
                //    FunctionManage.ImageSourceList.Remove(this);
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
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
