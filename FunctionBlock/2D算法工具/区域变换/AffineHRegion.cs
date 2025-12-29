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
    public class AffineHRegion : BaseFunction, IFunction
    {
        private RegionDataClass regionData = null; // 转换的对象可能是XLD　region，image等 
        private string invertPose = "false";

        public string InvertPose
        {
            get
            {
                return invertPose;
            }

            set
            {
                invertPose = value;
            }
        }
        public bool AffineObjectModel2D(RegionDataClass hObjcet, userWcsCoordSystem wcsCoord, string isInvertPose, out RegionDataClass affineRegion)
        {
            bool result = false;
            if (hObjcet == null)
            {
                affineRegion = null;
                return result;
            }
            affineRegion = hObjcet.Clone();
            HHomMat2D homMat2D;
            HHomMat2D homMat2DInvert;
            ///////////////////////////////////////////
            homMat2D = new HHomMat2D(wcsCoord.GetVariationHomMat2D());
            if (isInvertPose == "true")
                homMat2DInvert = homMat2D.HomMat2dInvert();
            else
                homMat2DInvert = homMat2D;
            ////////////////////////////////
            affineRegion.Region = homMat2DInvert.AffineTransRegion(hObjcet.Region, "bilinear");
            result = true;
            return result;
        }

        private RegionDataClass extractRefSource1Data()
        {
            RegionDataClass xld = null;
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HXLDCont":
                            xld = new RegionDataClass((HRegion)object3D);
                            break;
                        case "RegionDataClass":
                            xld = (RegionDataClass)object3D;
                            break;
                    }
                }
            }
            return xld;
        }
        public userWcsCoordSystem[] extractRefSource2Data()
        {
            List<userWcsCoordSystem> coordSystem = new List<userWcsCoordSystem>();
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
                        case "userWcsCoordSystem":
                            coordSystem.Add(((userWcsCoordSystem)object3D));
                            break;
                        case "userWcsCoordSystem[]":
                            coordSystem.AddRange(((userWcsCoordSystem[])object3D));
                            break;
                        case "userWcsPose3D":
                            coordSystem.Add(new userWcsCoordSystem(((userWcsPose)object3D)));
                            break;
                    }
                }
            }
            if (coordSystem.Count == 0)
                coordSystem.Add(new userWcsCoordSystem());
            return coordSystem.ToArray();
        }



        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Result.Succss = AffineObjectModel2D(extractRefSource1Data(), extractRefSource2Data()[0], this.invertPose, out this.regionData);
                OnExcuteCompleted(this.name, this.regionData);
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
                case "变换对象":
                case "输出对象":
                    return this.regionData; //
                case "输入对象":
                    return extractRefSource1Data(); //
                default:
                    if (this.name == propertyName)
                        return this.regionData;
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
                /////////////////////             
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

        public enum enShowItems
        {
            输入对象,
            输出对象,
        }
    }
}
