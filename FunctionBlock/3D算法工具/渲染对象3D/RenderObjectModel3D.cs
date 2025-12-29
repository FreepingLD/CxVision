using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class RenderObjectModel3DToImage : BaseFunction, IFunction
    {
        private ImageDataClass image1 = null;
        private ImageDataClass image2 = null;
        private ImageDataClass image3 = null;
        private double resolution_x = 0.0035;
        private double resolution_y = 0.0035;
        private enImageRenderType imageRenderType = enImageRenderType.黑白渲染;
        public double Resolution_x { get => resolution_x; set => resolution_x = value; }
        public double Resolution_y { get => resolution_y; set => resolution_y = value; }
        public enImageRenderType ImageRenderType { get => imageRenderType; set => imageRenderType = value; }

        private HObjectModel3D[] extractRefSource1Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HTuple":
                            listObjectModel3D.Add(new HObjectModel3D(((HTuple)object3D).IP));
                            break;
                        case "HObjectModel3D":
                            listObjectModel3D.Add((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D);
                            break;
                        case "userWcsPose3D": // 如果是位姿，将其转换成3D对象
                            HObjectModel3D plane3D = new HObjectModel3D();
                            plane3D.GenPlaneObjectModel3d(((userWcsPose)object3D).GetHPose(), new HTuple(), new HTuple());
                            listObjectModel3D.Add(plane3D);
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }





        /// <summary>
        /// 渲染3D对象模型到红绿蓝3图像
        /// </summary>
        /// <param name="objectModel"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="camParam"></param>
        /// <param name="camPose"></param>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="image3"></param>
        public bool RenderObjectModel3DTo3Image(HObjectModel3D[] objectModel, double resolution_x, double resolution_y, out ImageDataClass image1, out ImageDataClass image2, out ImageDataClass image3)
        {
            HalconLibrary ha = new HalconLibrary();
            image1 = null;
            image2 = null;
            image3 = null;
            switch(this.imageRenderType)
            {
                default:
                case enImageRenderType.黑白渲染:
                    return ha.TransformObject3DToRealImageModify(HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface"), resolution_x, resolution_y, out image1);
                case enImageRenderType.彩色渲染:
                    return ha.RenderObjectModel3DTo3ImageModify(objectModel, resolution_x, resolution_y, out image1, out image2, out image3);
            }          
        }



        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Result.Succss = RenderObjectModel3DTo3Image(extractRefSource1Data(), this.resolution_x, this.resolution_y, out this.image1, out this.image2, out this.image3);
                OnImageAcqComplete(this.image2, this.name);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
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
                case "图像1":
                case "输出图像1":
                    return this.image1; //
                case "图像2":
                case "输出图像2":
                    return this.image2; //
                case "图像3":
                case "输出图像3":
                    return this.image3; //
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); //
                default:
                    if (this.name == propertyName)
                        return this.image1;
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
                if (this.image1 != null) this.image1.Dispose();
                if (this.image2 != null) this.image2.Dispose();
                if (this.image3 != null) this.image3.Dispose();
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
            finally
            {
                OnItemDeleteEvent(this, this.name);
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

        public enum enShowItems
        {
            输入3D对象,
            输出图像1,
            输出图像2,
            输出图像3,
        }
    }
}
