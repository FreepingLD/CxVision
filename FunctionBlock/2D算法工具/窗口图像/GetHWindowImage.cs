using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.Data;
using Common;
using System.ComponentModel;
using AlgorithmsLibrary;
using System.Drawing;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(ImageData))]
    public class GetHWindowImage : BaseFunction, IFunction
    {
        private string  targetWindow;
        [NonSerialized]
        private ImageDataClass _imageData;
        public string TargetWindow { get => targetWindow; set => targetWindow = value; }
        public ImageDataClass ImageData { get => _imageData; set => _imageData = value; }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                if (this.targetWindow != null)
                {
                    if(HWindowManage.HWindowList.ContainsKey(this.targetWindow))
                    {
                        HImage image = HWindowManage.HWindowList[this.targetWindow].DumpWindowImage();
                        this.ImageData = new ImageDataClass(image);
                    }
                }
                this.OnExcuteCompleted(this._imageData.CamName,  this.name, this._imageData);
                Result.Succss = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(this.name):
                    return this.name;
                case nameof(this.ImageData):
                    return this.ImageData; //
                default:
                    if (this.name == propertyName)
                        return this.ImageData;
                    else return null;
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
            // throw new NotImplementedException();
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }

        #endregion





    }
}
