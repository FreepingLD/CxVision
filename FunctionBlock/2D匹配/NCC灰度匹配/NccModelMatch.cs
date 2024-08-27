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
using System.Runtime.Serialization.Formatters.Binary;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsCoordSystem))]
    public class NccModelMatch : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private ImageDataClass _RectifyImageData;
        [NonSerialized]
        private userWcsCoordSystem[] _wcsCoordSystem;
        [NonSerialized]
        private userPixCoordSystem[] _pixCoordSystem;
        [NonSerialized]
        private userWcsCoordSystem _wcsSingleCoordSystem;
        [NonSerialized]
        private userPixCoordSystem _pixSingleCoordSystem;

        private DoNccModelMatch _NccModelMatch;
        public DoNccModelMatch DoNccModel { get => _NccModelMatch; set => _NccModelMatch = value; }

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
        public userWcsCoordSystem[] WcsCoordSystem
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
        public userPixCoordSystem[] PixCoordSystem
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
        [DisplayName("坐标系")]
        [DescriptionAttribute("输出属性")]
        public userWcsCoordSystem WcsSingleCoordSystem
        {
            get
            {
                return _wcsSingleCoordSystem;
            }
            set
            {
                this._wcsSingleCoordSystem = value;
            }
        }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输出属性")]
        public userPixCoordSystem PixSingleCoordSystem
        {
            get
            {
                return _pixSingleCoordSystem;
            }
            set
            {
                this._pixSingleCoordSystem = value;
            }
        }

        public NccModelMatch()
        {
            this._NccModelMatch = new DoNccModelMatch();
            InitBindingTable();
        }

        private void InitBindingTable(int count = 0)
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Row", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Col", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Rad", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Deg", 0));
            //////////////
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Ref_X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Ref_Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Ref_Z", 0));
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
                                this._NccModelMatch.FindNccModelParam.PixCoordSystem = ((userWcsCoordSystem)item).GetPixCoordSystem();
                                break;
                            case nameof(userPixCoordSystem):
                                this._NccModelMatch.FindNccModelParam.PixCoordSystem = ((userPixCoordSystem)item);
                                break;
                        }
                    }
                }
                /////////////////////////////////
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                this.Result.Succss = this._NccModelMatch.FindNccModels(this.ImageData.Image);
                stopwatch.Stop();
                /////////////////////////////////////////////////////////////////////////
                //InitBindingTable(this._NccModelMatch.NccMatchResult.PixCoordSystem.Length);
                this._wcsCoordSystem = new userWcsCoordSystem[this._NccModelMatch.NccMatchResult.PixCoordSystem.Length];
                this.PixCoordSystem = this._NccModelMatch.NccMatchResult.PixCoordSystem;
                for (int i = 0; i < this._NccModelMatch.NccMatchResult.PixCoordSystem.Length; i++)
                {
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].ReferencePoint.CamParams = this._imageData.CamParams;
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].CurrentPoint.CamParams = this._imageData.CamParams;
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].ReferencePoint.Grab_x = this._imageData.Grab_X;
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].ReferencePoint.Grab_y = this._imageData.Grab_Y;
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].ReferencePoint.Grab_theta = this._imageData.Grab_Theta;
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].CurrentPoint.Grab_x = this._imageData.Grab_X;
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].CurrentPoint.Grab_y = this._imageData.Grab_Y;
                    this._NccModelMatch.NccMatchResult.PixCoordSystem[i].CurrentPoint.Grab_theta = this._imageData.Grab_Theta;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////
                    this._wcsCoordSystem[i] = this._NccModelMatch.NccMatchResult.PixCoordSystem[i].GetWcsCoordSystem();
                }
                this._pixSingleCoordSystem = this._NccModelMatch.NccMatchResult.PixCoordSystem[0];
                this._wcsSingleCoordSystem = this._wcsCoordSystem[0];
                ////////////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Row", this._pixSingleCoordSystem.CurrentPoint.Row);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].Std_Value = this._pixSingleCoordSystem.ReferencePoint.Row;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Col", this._pixSingleCoordSystem.CurrentPoint.Col);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].Std_Value = this._pixSingleCoordSystem.ReferencePoint.Col;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Rad", this._pixSingleCoordSystem.CurrentPoint.Rad);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].Std_Value = this._pixSingleCoordSystem.ReferencePoint.Rad;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X", this._wcsSingleCoordSystem.CurrentPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].Std_Value = this._wcsSingleCoordSystem.ReferencePoint.X;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y", this._wcsSingleCoordSystem.CurrentPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].Std_Value = this._wcsSingleCoordSystem.ReferencePoint.Y;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z", this._wcsSingleCoordSystem.CurrentPoint.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].Std_Value = this._wcsSingleCoordSystem.ReferencePoint.Z;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Deg", this._wcsSingleCoordSystem.CurrentPoint.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].Std_Value = this._wcsSingleCoordSystem.ReferencePoint.Angle;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Ref_X", this._imageData.Grab_X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Ref_Y", this._imageData.Grab_Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "Ref_Z", this._imageData.Grab_Theta);
                if (this._NccModelMatch.NccMatchResult.MatchScore != null && this._NccModelMatch.NccMatchResult.MatchScore.Length > 0)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name, "Score", this._NccModelMatch.NccMatchResult.MatchScore[0]);
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 11)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[11].SetValue(this.name, "Time", stopwatch.ElapsedMilliseconds);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time", 0));
                /////////// 生成矫正图像
                if (this._NccModelMatch.NccMatchResult.PixCoordSystem.Length > 0)
                {
                    HImage hImage = this._imageData.Image.AffineTransImage(this._pixSingleCoordSystem.GetRectifyImageHomMat2D(), "bilinear", "false");
                    this._RectifyImageData = new ImageDataClass(hImage, this._imageData.CamParams);
                }
                this._NccModelMatch.NccMatchResult.MatchCont.CamParams = this._imageData.CamParams;
                OnExcuteCompleted(this._imageData.CamName, this._imageData?.ViewWindow, this.name, this._NccModelMatch.NccMatchResult.MatchCont);
                OnExcuteCompleted(this._imageData.CamName, this._imageData?.ViewWindow, this.name + "CoordSys", this._wcsSingleCoordSystem); // 发出第一个坐标系
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
                    return this.ImageData.Image.AffineTransImage(this._NccModelMatch.NccMatchResult.PixCoordSystem[0].GetHomMat2D(), "bilinear", "false");

                case "参考点":
                    if (this._NccModelMatch.NccMatchResult.PixCoordSystem != null && this._NccModelMatch.NccMatchResult.PixCoordSystem.Length > 0)
                        return this._NccModelMatch.NccMatchResult.PixCoordSystem[0].ReferencePoint;
                    else
                        return null;

                case "匹配点":
                    if (this._NccModelMatch.NccMatchResult.PixCoordSystem != null)
                    {
                        int i = 0;
                        userPixVector[] userPixVector = new userPixVector[this._NccModelMatch.NccMatchResult.PixCoordSystem.Length];
                        foreach (var item in this._NccModelMatch.NccMatchResult.PixCoordSystem)
                        {
                            userPixVector[i] = item.CurrentPoint;
                            i++;
                        }
                        return userPixVector;
                    }
                    else
                        return null;

                case "userPixCoordSystem[]":
                    return this._pixCoordSystem;
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
                OnItemDeleteEvent(this, this.name);
                this._NccModelMatch?.ClearNccModel(); // 删除时释放句柄
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
                //if (!File.Exists(modelPath))
                //    LoggerHelper.Error(this.name + "->指定的模型路径不存在！");
                //else
                this.DoNccModel?.ReadHShapeModel(modelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->读取NCC模型失败" + ex.ToString());
            }
        }
        public void Save(string path)
        {
            try
            {
                string modelPath = path.Replace(".txt", ".ncm");
                this.DoNccModel?.SaveHShapeModel(modelPath); // 保存形状模型
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->保存NCC模型失败" + ex.ToString());
            }
        }
        #endregion




    }
}
