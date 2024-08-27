using AlgorithmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Common;
using HalconDotNet;
using System.Windows.Forms;
using System.IO;
using Sensor;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace FunctionBlock
{

    /// <summary>
    /// 变形匹配获取矫正后的图像
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(RectifyImageData))]
    public class LocalDeformableModelMatch : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private ImageDataClass _RectifyImageData;
        [NonSerialized]
        private userWcsCoordSystem _wcsCoordSystem;
        [NonSerialized]
        private userPixCoordSystem _pixCoordSystem;

        private DoLocalDeformableModelMatch2D _DeformableModelMatch;
        public DoLocalDeformableModelMatch2D DeformableModelMatch { get => _DeformableModelMatch; set => _DeformableModelMatch = value; }

        [DisplayName("输入图像")]
        [DescriptionAttribute("输入属性1")]
        [XmlIgnore]
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
                            this._imageData = oo[0] as ImageDataClass;
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


        [DisplayName("输出图像")]
        [DescriptionAttribute("输出属性")]
        [XmlIgnore]
        public ImageDataClass RectifyImageData
        {
            get
            {
                if (this._RectifyImageData != null)
                    OnExcuteCompleted(this._RectifyImageData?.CamName, this._RectifyImageData?.ViewWindow, this.name, this._RectifyImageData);
                return _RectifyImageData;
            }
            set
            {
                this._RectifyImageData = value;
            }
        }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输出属性")]
        [XmlIgnore]
        public userWcsCoordSystem WcsCoordSystem
        {
            get
            {
                return _wcsCoordSystem;
            }
            set
            {
                this._wcsCoordSystem = value;
            }
        }

        [DisplayName("像素坐标系")]
        [DescriptionAttribute("输出属性")]
        [XmlIgnore]
        public userPixCoordSystem PixCoordSystem
        {
            get
            {
                return _pixCoordSystem;
            }
            set
            {
                this._pixCoordSystem = value;
            }
        }




        public LocalDeformableModelMatch()
        {
            this._DeformableModelMatch = new DoLocalDeformableModelMatch2D();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Row", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Col", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Rad", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Score", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time", 0));
        }



        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsCoordSystem):
                                this._DeformableModelMatch.FindDeformableModelParam.PixCoordSystem = ((userWcsCoordSystem)item).GetPixCoordSystem();
                                break;
                            case nameof(userPixCoordSystem):
                                this._DeformableModelMatch.FindDeformableModelParam.PixCoordSystem = ((userPixCoordSystem)item);
                                break;
                        }
                    }
                }
                /////////////////////////////////
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                this.Result.Succss = this._DeformableModelMatch.FindLocalDeformableModel(this.ImageData.Image);
                stopwatch.Stop();
                /////////////////////////////////////////////////////////////////////////
                this.PixCoordSystem = this._DeformableModelMatch.LocalDeformableMatchResult.PixCoordSystem;
                this._DeformableModelMatch.LocalDeformableMatchResult.PixCoordSystem.ReferencePoint.CamParams = this._imageData.CamParams;
                this._DeformableModelMatch.LocalDeformableMatchResult.PixCoordSystem.CurrentPoint.CamParams = this._imageData.CamParams;
                this._wcsCoordSystem = this.PixCoordSystem.GetWcsCoordSystem();
                this._RectifyImageData = new ImageDataClass( this._DeformableModelMatch.LocalDeformableMatchResult.RectifiedImage);
                //this._DeformableModelMatch.LocalDeformableMatchResult.MatchCont.CamParams = this._imageData.CamParams;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                ((BindingList<Match2DResultInfo>)this.ResultInfo)[0].SetValue(this.name, "匹配行坐标", this.PixCoordSystem.CurrentPoint.Row);
                ((BindingList<Match2DResultInfo>)this.ResultInfo)[0].SetValue(this.name, "匹配列坐标", this.PixCoordSystem.CurrentPoint.Col);
                ((BindingList<Match2DResultInfo>)this.ResultInfo)[0].SetValue(this.name, "匹配角度", this.PixCoordSystem.CurrentPoint.Rad);
                ((BindingList<Match2DResultInfo>)this.ResultInfo)[0].SetValue(this.name, "匹配得分", this._DeformableModelMatch.LocalDeformableMatchResult.MatchScore);
                ((BindingList<Match2DResultInfo>)this.ResultInfo)[0].SetValue(this.name, "匹配时间", stopwatch.ElapsedMilliseconds);
                /////////// 生成矫正图像
                OnExcuteCompleted(this._imageData.CamName, this._imageData?.ViewWindow, this.name, this._RectifyImageData);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->形状匹配执行成功");
            else
                LoggerHelper.Error(this.name + "->形状匹配执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "2D坐标系":
                case "3D坐标系":
                case "坐标系":
                default:
                case nameof(this.WcsCoordSystem):
                    return this.WcsCoordSystem; //
                case "输入对象":
                    return this.ImageData;

                case "矫正图像":
                    return this.RectifyImageData;

                case "参考点":
                    if (this._DeformableModelMatch.LocalDeformableMatchResult.PixCoordSystem != null)
                        return this._DeformableModelMatch.LocalDeformableMatchResult.PixCoordSystem.ReferencePoint;
                    else
                        return null;
                case "匹配点":
                    if (this._DeformableModelMatch.LocalDeformableMatchResult.PixCoordSystem != null)
                        return this._DeformableModelMatch.LocalDeformableMatchResult.PixCoordSystem.CurrentPoint;
                    else
                        return null;
            }
            //return null;
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
                OnItemDeleteEvent(this, this.name);
                this._DeformableModelMatch?.ClearHDeformableModel(); // 删除时释放句柄
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
                string modelPath = path.Replace(".txt", ".ncm");
                if (!File.Exists(modelPath))
                    LoggerHelper.Error(this.name + "->指定的模型路径不存在！");
                else
                    this.DeformableModelMatch?.ReaHDeformableModel(modelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->读取NCC模型失败" + ex.ToString());
            }
            //return null;
        }
        public void Save(string path)
        {
            try
            {
                string modelPath = path.Replace(".txt", ".ncm");
                this.DeformableModelMatch?.SaveHDeformableModel(modelPath); // 保存形状模型
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->保存NCC模型失败" + ex.ToString());
            }
        }
        #endregion




    }
}
