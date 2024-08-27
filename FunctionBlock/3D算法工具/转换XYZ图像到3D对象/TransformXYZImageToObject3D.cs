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
    public class TransformXYZImageToObject3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;


        private HImage extractRefSource1Data()
        {
            HImage image = null;
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
                        case "HImage":
                            image = (HImage)object3D;
                            break;
                        case "ImageDataClass":
                            image = ((ImageDataClass)object3D).Image;
                            break;
                    }
                }
            }
            return image;
        }
        private HImage extractRefSource3Data()
        {
            HImage image = null;
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource3.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource3[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource3[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HImage":
                            image = (HImage)object3D;
                            break;
                        case "ImageDataClass":
                            image = ((ImageDataClass)object3D).Image;
                            break;
                    }
                }
            }
            return image;
        }
        private HImage extractRefSource2Data()
        {
            HImage image = null;
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HImage":
                            image = (HImage)object3D;
                            break;
                        case "ImageDataClass":
                            image = ((ImageDataClass)object3D).Image;
                            break;
                    }
                }
            }
            return image;
        }
        public bool TransformXyzRealImageToObject3D(HImage imageX, HImage imageY, HImage imageZ, out HObjectModel3D objectModel)
        {
            bool result = false;
            objectModel = new HObjectModel3D();
            if (imageX == null)
            {
                throw new ArgumentNullException("imageX");
            }
            if (imageY == null)
            {
                throw new ArgumentNullException("imageY");
            }
            if (imageZ == null)
            {
                throw new ArgumentNullException("imageZ");
            }
            objectModel.XyzToObjectModel3d(imageX, imageY, imageZ);
            result = true;
            return result;
        }



        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = TransformXyzRealImageToObject3D(extractRefSource1Data(), extractRefSource2Data(), extractRefSource3Data(), out this.dataHandle3D);
                OnExcuteCompleted(this.name, this.dataHandle3D);
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
                case "3D对象":
                case "输出3D对象":
                    return this.dataHandle3D; //
                case "输入图像X":
                    return extractRefSource1Data(); //
                case "输入图像Y":
                    return extractRefSource2Data(); //
                case "输入图像Z":
                    return extractRefSource3Data(); //
                default:
                    if (this.name == propertyName)
                        return this.dataHandle3D;
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
                if (this.dataHandle3D != null)
                    this.dataHandle3D.Dispose();
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
            输出3D对象,
            输入图像X,
            输入图像Y,
            输入图像Z,
        }

    }
}
