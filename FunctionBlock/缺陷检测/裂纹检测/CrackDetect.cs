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
    [DefaultProperty(nameof(CrackRegion))]
    public class CrackDetect : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private RegionDataClass _crackRegion;
        [NonSerialized]
        private userPixCoordSystem _pixCoordSystem;
        [NonSerialized]
        private XldDataClass _xldData;

        [DisplayName("NG区域")]
        [DescriptionAttribute("输出属性")]
        public RegionDataClass CrackRegion { get => _crackRegion; set => _crackRegion = value; }

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
                            this._imageData = oo[0] as ImageDataClass;
                        }
                    }
                    //else
                    //{
                    //    this._imageData = null;
                    //}
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

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性2")]
        public userPixCoordSystem PixCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(userWcsCoordSystem):
                                        this._pixCoordSystem = ((userWcsCoordSystem)item)?.GetPixCoordSystem();
                                        if (this._pixCoordSystem == null)
                                            this._pixCoordSystem = new userPixCoordSystem();
                                        break;
                                    case nameof(userPixCoordSystem):
                                        this._pixCoordSystem = ((userPixCoordSystem)item);
                                        break;
                                }
                            }
                        }
                    }
                    if (this._pixCoordSystem == null)
                        this._pixCoordSystem = new userPixCoordSystem();
                    return _pixCoordSystem;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._pixCoordSystem = value;
            }
        }
        public CrackDetectParam Param
        {
            get;
            set;
        }


        public CrackDetect()
        {
            this.Param = new CrackDetectParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "NG区域数量", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "NG区域面积", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "时间", 0));
        }




        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HRegion hRegion = null;
                HXLDCont hXLDCont = null;
                this.Result.Succss = CrackDetectMethod.CrackDetect2(this.ImageData.Image, this.PixCoordSystem, this.Param, out hRegion, out hXLDCont);
                this._crackRegion?.Dispose();
                this._crackRegion = new RegionDataClass(hRegion);
                this._crackRegion.CamName = this._imageData?.CamName;
                this._crackRegion.ViewWindow = this._imageData?.ViewWindow;
                this._crackRegion.Tag = this._imageData?.Tag;
                this._crackRegion.Color = enColor.red;
                this._xldData?.Dispose();
                this._xldData = new XldDataClass(hXLDCont);
                this._xldData.CamName = this._imageData?.CamName;
                this._xldData.ViewWindow = this._imageData?.ViewWindow;
                this._xldData.Tag = this._imageData?.Tag;
                this._xldData.Color = enColor.green;
                int num = hRegion.CountObj();
                double area = hRegion.Union1().Area;
                hRegion?.Dispose();
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "NG区域数量", num);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "NG区域面积", area);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "执行时间", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._crackRegion.CamName, this._crackRegion.ViewWindow, this.name, this._crackRegion);
                OnExcuteCompleted(this._crackRegion.CamName, this._crackRegion.ViewWindow, this.name, this._xldData);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case "Name":
                    return this.name;
                case "输出区域":
                case "输出对象":
                    return this.CrackRegion;
                case nameof(this.CrackRegion):
                    return this.CrackRegion; //
                default:
                    return this.CrackRegion;
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
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除节点对象出错！");
            }
        }
        public void Read(string path)
        {
            try
            {
                //string modelPath = path.Replace(".txt", ".hobj");
                //if (!File.Exists(modelPath))
                //    LoggerHelper.Error(this.name + "->指定的区域路径不存在！");
                //else
                //    this.Param?.ReadTeachRegion(modelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->读取区域失败" + ex.ToString());
            }
        }
        public void Save(string path)
        {
            try
            {
                //string modelPath = path.Replace(".txt", ".hobj");
                //this.Param?.SaveTeachRegion(modelPath); // 保存形状模型
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->保存区域失败" + ex.ToString());
            }
            /////////////////////////////////////////////// 以xml的形式保存参数 //////////////////
        }




        #endregion


    }
}
