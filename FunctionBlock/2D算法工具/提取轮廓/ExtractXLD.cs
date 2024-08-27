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

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(ExtractContourXLD))]
    public class ExtractXLD : BaseFunction, IFunction
    {
        [NonSerialized]
        private XldDataClass extractContourXLD;
        private string extractMethodXLD = "edges_sub_pix";
        private EdgesSubPix edges_sub_pix;
        private EdgesColorSubPix edges_color_sub_pix;
        private ThresholdSubPix threshold_sub_pix;
        private ZeroCrossingSubPix zero_crossing_sub_pix;
        private LinesGauss lines_gauss;
        public string ExtractMethodXLD { get => extractMethodXLD; set => extractMethodXLD = value; }
        public EdgesSubPix Edges_sub_pix { get => edges_sub_pix; set => edges_sub_pix = value; }
        public EdgesColorSubPix Edges_color_sub_pix { get => edges_color_sub_pix; set => edges_color_sub_pix = value; }
        public ThresholdSubPix Threshold_sub_pix { get => threshold_sub_pix; set => threshold_sub_pix = value; }
        public ZeroCrossingSubPix Zero_crossing_sub_pix { get => zero_crossing_sub_pix; set => zero_crossing_sub_pix = value; }
        public LinesGauss Lines_gauss { get => lines_gauss; set => lines_gauss = value; }
        public XldDataClass ExtractContourXLD { get => extractContourXLD; set => extractContourXLD = value; }

        public ExtractXLD()
        {
            edges_sub_pix = new EdgesSubPix();
            edges_color_sub_pix = new EdgesColorSubPix();
            threshold_sub_pix = new ThresholdSubPix();
            zero_crossing_sub_pix = new ZeroCrossingSubPix();
            lines_gauss = new LinesGauss();
        }


        private ImageDataClass extractRefSource1Data()
        {
            object object3D = null;
            ImageDataClass image = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)    
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null && object3D is ImageDataClass) // 这样做是为了动态获取名称
                    image = (ImageDataClass)object3D;
            }
            return image;
        }
        private bool extractContour(ImageDataClass image, out XldDataClass contour)
        {
            bool reault = false;
            contour = new XldDataClass();
            HXLDCont edges = null;
            if (image == null || image.Image == null) return reault;
            switch (this.ExtractMethodXLD.Trim())
            {
                case "edges_sub_pix":
                    edges = edges_sub_pix.edges_sub_pix(image.Image);
                    break;
                case "edges_color_sub_pix":
                    edges = edges_color_sub_pix.edges_color_sub_pix(image.Image);
                    break;
                case "threshold_sub_pix":
                    edges = threshold_sub_pix.threshold_sub_pix(image.Image);
                    break;
                case "zero_crossing_sub_pix":
                    edges = zero_crossing_sub_pix.zero_crossing_sub_pix(image.Image);
                    break;
                case "lines_gauss":
                    edges = lines_gauss.lines_gauss(image.Image);
                    break;
            }
            contour = new XldDataClass(edges, image.CamParams); //.ContourToWorldPlaneXld(image.CamParam,new HPose(image.CamPose),1)
            return true;
        }





        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Result.Succss = extractContour(extractRefSource1Data(), out this.extractContourXLD);
                OnExcuteCompleted(this.name, this.extractContourXLD); // 发送到图形窗口
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
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
                    return this.name;
                case "XLD轮廓":
                case "输出对象":
                    return this.extractContourXLD; //
                case "输入对象":
                    return extractRefSource1Data(); //
                default:
                    if (this.name == propertyName)
                        return this.extractContourXLD;
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
            try
            {
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

        public enum enExtractMethodXLD
        {
            edges_sub_pix,
            edges_color_sub_pix,
            threshold_sub_pix,
            zero_crossing_sub_pix,
            lines_gauss,
        }
        public enum enShowItems
        {
            输入对象,
            输出对象,
        }



    }
}
