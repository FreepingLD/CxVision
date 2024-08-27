using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(DataCodeContent))]
    public class BarCodeDetection : BaseFunction, IFunction // 实现接口的类里，只包含输入、输出函数及接口方法
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private DataCodeResult _DataCodeResult;
        public string _DataCodeContent;

        [DescriptionAttribute("参数")]
        public DoBarCodeDetection DoBarCode { get; set; }

        [DisplayName("输入图像")]
        [DescriptionAttribute("输入属性1")]
        public ImageDataClass ImageData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(ImageDataClass):
                                        this._imageData = item as ImageDataClass;
                                        break;
                                    case nameof(HImage):
                                        this._imageData = new ImageDataClass((HImage)item);
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _imageData;
            }
            set
            {
                _imageData = value;
            }
        }




        [DisplayName("解码内容")]
        [DescriptionAttribute("输出属性")]
        public string DataCodeContent { get => _DataCodeContent; set => _DataCodeContent = value; }


        public BarCodeDetection()
        {
            this.DoBarCode = new DoBarCodeDetection();
            this.ResultInfo = new BindingList<DataCodeResultInfo>();
            ((BindingList<DataCodeResultInfo>)this.ResultInfo).Add(new DataCodeResultInfo());
            ((BindingList<DataCodeResultInfo>)this.ResultInfo).Add(new DataCodeResultInfo());
        }



        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                BarCodeResult DataCodeResult = new BarCodeResult();
                Result.Succss = this.DoBarCode.FindBarCode2D(this.ImageData.Image, out DataCodeResult);
                stopwatch.Stop();
                if (((BindingList<DataCodeResultInfo>)this.ResultInfo).Count > 0)
                    ((BindingList<DataCodeResultInfo>)this.ResultInfo)[0].SetValue(this.name, "二维码内容", DataCodeResult.Content);
                else
                    ((BindingList<DataCodeResultInfo>)this.ResultInfo).Add(new DataCodeResultInfo());
                if (((BindingList<DataCodeResultInfo>)this.ResultInfo).Count > 1)
                    ((BindingList<DataCodeResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                else
                    ((BindingList<DataCodeResultInfo>)this.ResultInfo).Add(new DataCodeResultInfo());
                OnExcuteCompleted(this._imageData.CamName, this._imageData?.ViewWindow, this.name, DataCodeResult.SymbolXLDs);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                case nameof(this.DataCodeContent):
                    return this.DataCodeContent; //
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
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
                this.DoBarCode.ClearDataCode2dModel();
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }

        public void Read(string path)
        {
            try
            {
                string modelPath = path.Replace(".txt", ".bcm");
                this.DoBarCode?.ReadDataCodeModel(modelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->读取条形码失败" + ex.ToString());
            }
        }
        public void Save(string path)
        {
            try
            {
                string modelPath = path.Replace(".txt", ".bcm");
                this.DoBarCode?.SaveDataCodeModel(modelPath); // 保存形状模型
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->保存条形码失败" + ex.ToString());
            }
            /////////////////////////////////////////////// 以xml的形式保存参数 //////////////////
        }

        #endregion



    }
}
