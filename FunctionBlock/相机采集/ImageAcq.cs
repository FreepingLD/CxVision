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
    [DefaultProperty(nameof(ImageData))]
    public class ImageAcq : BaseFunction, IFunction
    {
        private CoordSysAxisPosParam axisParam;
        [NonSerialized]
        private ImageDataClass _imageData = null; // 数据句柄
        [NonSerialized]
        private ImageDataClass _darkImageData = null; // 数据句柄
        public int FileIndex { get; set; }

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

        [DisplayName("输出图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass ImageData { get => _imageData; set => _imageData = value; }

        [DisplayName("输出图像2")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass DarkImageData { get => _darkImageData; set => _darkImageData = value; }

        [DisplayName("输出图像3")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass ImageData2 { get; set; }

        [DisplayName("输出图像4")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass ImageData3 { get; set; }

        [DisplayName("输出图像5")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass ImageData4 { get; set; }

        [DisplayName("拍照坐标")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint GrabPoint { get; set; }

        public CoordSysAxisPosParam AxisParam { get => axisParam; set => axisParam = value; }
        public ImageAcqParam AcqParam { get; set; }


        public ImageAcq()
        {
            this.axisParam = new CoordSysAxisPosParam();
            this.AcqParam = new ImageAcqParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }

        public ImageAcq(string _acqSourceName)
        {
            this.axisParam = new CoordSysAxisPosParam();
            this.AcqParam = new ImageAcqParam();
            this.AcqSourceName = _acqSourceName;
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            object value = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).Sensor.GetParam("曝光");
            double expose = 0;
            double.TryParse(value.ToString(), out expose); // 创建对象时
            this.AcqParam.Expose = expose;
            // 自动添加视图窗口名
            string cameName = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.Name;
            foreach (var item in HWindowManage.GetKeysList())
            {
                if (item.Contains(cameName))
                    this.AcqParam.ViewWindow = item;
            }
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }



        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            Dictionary<enDataItem, object> list;
            string state = "none";
            string ViewName = "";
            int GrabNo = 0;
            string ProductID = "";
            int DetectEdge = 1;
            TreeNode refNode = null;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ///// 检测传入的参数是否包含有图像 ///
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item == null) continue;
                        switch (item.GetType().Name)
                        {
                            case nameof(ImageDataClass):
                                this._imageData = item as ImageDataClass;
                                // 赋值拍照点坐标
                                if (this._imageData.IsInitialized())
                                {
                                    this.AcqParam.GrabPoint = new userWcsVector(this._imageData.Grab_X, this._imageData.Grab_Y, this._imageData.Grab_Z,
                                        this._imageData.Grab_U, this._imageData.Grab_V, this._imageData.Grab_Theta);
                                    state = "传入图像";
                                }
                                break;
                            case nameof(HImage):
                                this._imageData = new ImageDataClass((HImage)item);
                                if (this._imageData.IsInitialized())
                                    state = "传入图像";
                                break;
                            case nameof(String):
                                if (item.ToString().Contains("ViewName=")) // 传入视图名称
                                {
                                    ViewName = item.ToString().Replace("ViewName=", "");
                                }
                                if (item.ToString().Contains("ProductID=")) // 传入产品ID
                                {
                                    ProductID = item.ToString().Replace("ProductID=", "");
                                }
                                break;
                            case nameof(Int32):
                                if (item.ToString().Contains("GrabNo=")) // 传入视图名称
                                {
                                    int result = 0;
                                    int.TryParse(item.ToString().Replace("GrabNo=", ""), out result);
                                    GrabNo = result;
                                }
                                if (item.ToString().Contains("DetectEdge=")) // 传入视图名称
                                {
                                    int result = 0;
                                    int.TryParse(item.ToString().Replace("DetectEdge=", ""), out result);
                                    DetectEdge = result;
                                }
                                break;
                            case nameof(TreeNode):
                                refNode = item as TreeNode;
                                //Control control = refNode.TreeView.Parent;
                                break;
                        }
                    }
                }
                /////////////////////////
                switch (state)
                {
                    case "传入图像":// 适用于外部触发模式，异步采集
                        break;
                    default:
                        if (ImageAcqDevice.Instance.IsCamSource)
                        {
                            if (this.AcqSourceName == null)
                            {
                                LoggerHelper.Error(this.name + "->图像采集报错,采集源为空值", AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.Name);
                                return this.Result;
                            }
                            this._imageData = null;
                            //AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).Exposure = this.AcqParam.Expose; // 更新相机曝光
                            list = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.AcqImageData(this.LightParam, this.AcqParam.Expose);
                            // 更新数据
                            if (this.axisParam == null) this.axisParam = new CoordSysAxisPosParam();
                            this.axisParam?.UpdataAxisPosition(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName); // 实时使用当前位置     
                            if (list.ContainsKey(enDataItem.Image))
                                this._imageData = ((ImageDataClass)list[enDataItem.Image]);
                            if (list.ContainsKey(enDataItem.DarkImage))
                                this._darkImageData = ((ImageDataClass)list[enDataItem.DarkImage]);
                            if (this._imageData != null)
                            {
                                this._imageData.CamAxis_X = this.axisParam.GetAxisPosition(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enAxisName.相机轴X);
                                this._imageData.CamAxis_Y = this.axisParam.GetAxisPosition(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enAxisName.相机轴Y);
                                ////////////////////////////////////////////// CamAxisIncrement
                                this._imageData.CamParams.CaliParam.CamAxisIncrementX = this._imageData.CamAxis_X - this._imageData.CamParams.CaliParam.CamAxisCoord.X;
                                this._imageData.CamParams.CaliParam.CamAxisIncrementY = this._imageData.CamAxis_Y - this._imageData.CamParams.CaliParam.CamAxisCoord.Y;
                                this._imageData.Grab_X = axisParam.X; // + (this._imageData.CamParams.CaliParam.CamAxisCoord.X - this._imageData.CamAxis_X); // 用参考位置 - 当前位置
                                this._imageData.Grab_Y = axisParam.Y; // + (this._imageData.CamParams.CaliParam.CamAxisCoord.Y - this._imageData.CamAxis_Y); // 用参考位置 - 当前位置
                                this._imageData.Grab_Z = axisParam.Z;
                                this._imageData.Grab_Theta = axisParam.Theta;
                                this._imageData.Grab_U = axisParam.U;
                                this._imageData.Grab_V = axisParam.V;
                                this._imageData.ProductID = ProductID;
                                this._imageData.DetectEdge = DetectEdge;
                                if (GrabNo != 0)
                                    this._imageData.Tag = GrabNo; // 如果拍照位不等于0，那么将更新拍照位
                                else
                                {
                                    switch (this._imageData.CamParams.ImageIndex)
                                    {
                                        case "auto":
                                        case "Auto":
                                            break;
                                        case "GrabNo":
                                        case "grabno":
                                            this._imageData.Tag = CommunicationConfigParamManger.Instance.ReadValue(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enCommunicationCommand.GrabNo);
                                            break;
                                        case "SynToGrabNo": // 将拍照位与图像索引对应
                                        case "SynTograbno":
                                            CommunicationConfigParamManger.Instance.WriteValue(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enCommunicationCommand.GrabNo, this._imageData.Tag);
                                            break;
                                        default:
                                            this._imageData.Tag = this._imageData.CamParams.ImageIndex;
                                            break;
                                    }
                                }
                                if (ViewName != "") // 当视图名称不为空时才更新
                                    this._imageData.ViewWindow = ViewName; // 用拍照位作为视图窗口名称
                                //////////////// 如果视图窗口是自动，那么将自动将自动跟随拍照位 ////////////////
                                if (this.AcqParam.ViewWindow == "auto" || this.AcqParam.ViewWindow == "Auto")
                                    this._imageData.ViewWindow = $"图像视图:[{this._imageData.CamName}]-拍照位{this._imageData.Tag.ToString()}";//
                                else
                                    this._imageData.ViewWindow = this.AcqParam?.ViewWindow;
                            }
                            if (this._darkImageData != null)
                            {
                                this._darkImageData.CamAxis_X = this.axisParam.GetAxisPosition(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enAxisName.相机轴X);
                                this._darkImageData.CamAxis_Y = this.axisParam.GetAxisPosition(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enAxisName.相机轴Y);
                                //////////////////////////////////////////////
                                this._darkImageData.CamParams.CaliParam.CamAxisIncrementX = this._darkImageData.CamAxis_X - this._darkImageData.CamParams.CaliParam.CamAxisCoord.X;
                                this._darkImageData.CamParams.CaliParam.CamAxisIncrementY = this._darkImageData.CamAxis_Y - this._darkImageData.CamParams.CaliParam.CamAxisCoord.Y;
                                this._darkImageData.Grab_X = axisParam.X;// + (this._imageData.CamParams.CaliParam.CamAxisCoord.X - this._imageData.CamAxis_X);
                                this._darkImageData.Grab_Y = axisParam.Y;// + (this._imageData.CamParams.CaliParam.CamAxisCoord.Y - this._imageData.CamAxis_Y);
                                this._darkImageData.Grab_Z = axisParam.Z;
                                this._darkImageData.Grab_Theta = axisParam.Theta;
                                this._darkImageData.Grab_U = axisParam.U;
                                this._darkImageData.Grab_V = axisParam.V;
                                this._darkImageData.ProductID = ProductID;
                                this._darkImageData.DetectEdge = DetectEdge;
                                if (GrabNo != 0)
                                    this._darkImageData.Tag = GrabNo;  // 如果拍照位不等于0，那么将更新拍照位
                                else
                                {
                                    switch (this._imageData.CamParams.ImageIndex)
                                    {
                                        case "auto":
                                        case "Auto":
                                            break;
                                        case "GrabNo":
                                        case "grabno":
                                            this._imageData.Tag = CommunicationConfigParamManger.Instance.ReadValue(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enCommunicationCommand.GrabNo);
                                            break;
                                        case "SynToGrabNo": // 将拍照位与图像索引对应
                                        case "SynTograbno":
                                            CommunicationConfigParamManger.Instance.WriteValue(AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName, enCommunicationCommand.GrabNo, this._imageData.Tag);
                                            break;
                                        default:
                                            this._imageData.Tag = this._imageData.CamParams.ImageIndex;
                                            break;
                                    }
                                }
                                if (ViewName != "")  // 当视图名称不为空时才更新
                                    this._darkImageData.ViewWindow = ViewName;  // 用拍照位作为视图窗口名称
                                //////////////// 如果视图窗口是自动，那么将自动将自动跟随拍照位 ////////////////
                                if (this.AcqParam.ViewWindow == "auto" || this.AcqParam.ViewWindow == "Auto")
                                    this._darkImageData.ViewWindow = $"图像视图:[{this._darkImageData.CamName}]-拍照位{this._darkImageData.Tag.ToString()}";//
                                else
                                    this._darkImageData.ViewWindow = this.AcqParam?.ViewWindow;
                            }
                            // 赋值拍照点坐标
                            this.AcqParam.GrabPoint = new userWcsVector(axisParam.X, axisParam.Y, axisParam.Z, axisParam.U, axisParam.V, axisParam.Theta);
                        }
                        else
                        {
                            if (ImageAcqDevice.Instance.IsFileSource)
                            {
                                this._imageData = this.AcqParam.ReadImage(this.AcqParam.SingleFilePath, this.AcqSourceName);
                                //////////////// 如果视图窗口是自动，那么将自动跟随拍照位 ////////////////
                                if (this.AcqParam.ViewWindow == "auto" || this.AcqParam.ViewWindow == "Auto")
                                    this._imageData.ViewWindow = $"图像视图:[{this._imageData.CamName}]-拍照位{this._imageData.Tag.ToString()}";//
                                else
                                    this._imageData.ViewWindow = this.AcqParam?.ViewWindow;
                            }
                            else
                            {
                                if (this.AcqParam.FilePath != null)
                                {
                                    if (this.FileIndex == this.AcqParam.FilePath.Count)
                                    {
                                        this.FileIndex = 0;
                                        new UserMessageForm().ShowDialog("图像遍历完成!!!");
                                    }
                                    this._imageData = this.AcqParam.ReadImage(this.AcqParam.FilePath[this.FileIndex], this.AcqSourceName);
                                    this._imageData.Tag = this.FileIndex + 1;
                                    //this._imageData.ViewWindow = this.AcqParam.ViewWindow;
                                    //////////////// 如果视图窗口是自动(auto)，那么将自动跟随拍照位 ////////////////
                                    if (this.AcqParam.ViewWindow == "auto" || this.AcqParam.ViewWindow == "Auto")
                                        this._imageData.ViewWindow = $"图像视图:[{this._imageData.CamName}]-拍照位{this._imageData.Tag.ToString()}";//
                                    else
                                        this._imageData.ViewWindow = this.AcqParam?.ViewWindow;
                                    this.FileIndex++;
                                }
                            }
                            if (this._imageData != null)
                            {
                                this._imageData.Grab_X = this.AcqParam.GrabPoint.X;
                                this._imageData.Grab_Y = this.AcqParam.GrabPoint.Y;
                                this._imageData.Grab_Z = this.AcqParam.GrabPoint.Z;
                                this._imageData.Grab_Theta = this.AcqParam.GrabPoint.Grab_theta;
                                this._imageData.Grab_U = this.AcqParam.GrabPoint.Grab_u;
                                this._imageData.Grab_V = this.AcqParam.GrabPoint.Grab_v;
                                this._imageData.CamAxis_X = 0;
                                this._imageData.CamAxis_Y = 0;
                            }
                            if (this._darkImageData != null)
                            {
                                this._darkImageData.Grab_X = this.AcqParam.GrabPoint.X;
                                this._darkImageData.Grab_Y = this.AcqParam.GrabPoint.Y;
                                this._darkImageData.Grab_Z = this.AcqParam.GrabPoint.Z;
                                this._darkImageData.Grab_Theta = this.AcqParam.GrabPoint.Grab_theta;
                                this._darkImageData.Grab_U = this.AcqParam.GrabPoint.Grab_u;
                                this._darkImageData.Grab_V = this.AcqParam.GrabPoint.Grab_v;
                                this._darkImageData.CamAxis_X = 0;
                                this._darkImageData.CamAxis_Y = 0;
                            }
                        }
                        break;
                }
                stopwatch.Stop();
                ////////////////////////////////////////////////
                this.CreateResultInfo(12); // 共11条属性信息 
                if (this._imageData != null)
                {
                    this.GrabPoint = new userWcsPoint(this._imageData.Grab_X, this._imageData.Grab_Y, this._imageData.Grab_Z);
                    this.GrabPoint.U = this._imageData.Grab_U;
                    this.GrabPoint.V = this._imageData.Grab_V;
                    this.GrabPoint.Theta = this._imageData.Grab_Theta;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", this._imageData.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", this._imageData.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this._imageData.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].Std_Value = this._imageData.Grab_X; // - (this._imageData.CamParams.CaliParam.CamAxisCoord.X - this._imageData.CamAxis_X)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this._imageData.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].Std_Value = this._imageData.Grab_Y; //  - (this._imageData.CamParams.CaliParam.CamAxisCoord.Y - this._imageData.CamAxis_Y)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Z", this._imageData.Grab_Z);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].Std_Value = this._imageData.Grab_Z;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Grab_Theta", this._imageData.Grab_Theta);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].Std_Value = this._imageData.Grab_Theta;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Grab_U", this._imageData.Grab_U);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].Std_Value = this._imageData.Grab_U;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_V", this._imageData.Grab_V);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].Std_Value = this._imageData.Grab_V;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "CamAxis_X", this._imageData.CamAxis_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].Std_Value = this._imageData.CamParams.CaliParam.CamAxisCoord.X;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "CamAxis_Y", this._imageData.CamAxis_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].Std_Value = this._imageData.CamParams.CaliParam.CamAxisCoord.Y;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    this.GrabPoint = new userWcsPoint();
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Z", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Grab_Theta", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Grab_U", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_V", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "CamAxis_X", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "CamAxis_Y", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name, "Time(ms)", 0);
                }
                /////////////////////////////////////////////////
                if (refNode != null && this._imageData != null)
                    this._imageData.PathNode = $"{refNode.TreeView.Parent?.Text}.{refNode.Parent?.FullPath.Replace("\\", ".")}";
                //if (state != "传入图像") // 只有在没有传入图像时，才触发事件来传送图像
                this.OnExcuteCompleted(this._imageData?.CamName, this._imageData?.ViewWindow, this.name, this._imageData);
                if (this._imageData?.Image != null && this._imageData.Image.IsInitialized())
                {
                    this.Result.Succss = true;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[11].SetValue(this.name, "执行结果", 1);
                }
                else
                {
                    this.Result.Succss = false;
                    this.Result.ExcuteState = enExcuteState.图像采集失败中断;
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[11].SetValue(this.name, "执行结果", 0);
                }
                ///////////////////////////////////
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "->图像采集完成", AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.Name);
                else
                    LoggerHelper.Error(this.name + "->图像采集失败", AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.Name);
                // 更改UI字体　
                UpdataNodeElementStyle(param, this.Result.Succss);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->图像采集报错" + ex.ToString(), AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.Name);
                this.Result.Succss = false;
            }
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                    return this._imageData;
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
                case "move":
                case "Move":
                    enCoordSysName coordSysName = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName).CoordSysName;
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, this.axisParam.X);
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, this.axisParam.Y);
                    //CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Z, this.axisParam.Z);
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.FunctionNoToPlc, "Move");
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
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
