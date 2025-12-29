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
using System.Data;

namespace FunctionBlock
{
    [Serializable]
    public class CropParam
    {

        private enShapeType shapeType = enShapeType.矩形2;
        private enInsideOrOutside insideOrOutside = enInsideOrOutside.保留;

        [DisplayNameAttribute("形状类型")]
        public enShapeType ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        [DisplayNameAttribute("操作方法")]
        public enInsideOrOutside InsideOrOutside
        {
            get
            {
                return insideOrOutside;
            }

            set
            {
                insideOrOutside = value;
            }
        }

        [DisplayNameAttribute("形状参数")]
        public WcsROI RoiShape { get; set; }


    }

    [Serializable]
    public class CropMethod
    {
        /// <summary>
        /// 方法只用来释放内部的参数，不释放传入进去的参数
        /// </summary>
        /// <param name="ObjectModel"></param>
        /// <param name="cropObjectModel"></param>
        /// <param name="Pose3D"></param>
        /// <returns></returns>
        public bool CropObjectModel3D(HObjectModel3D ObjectModel, BindingList<CropParam> paramList, userCamParam CamParam, userCamPose CamPose, out HObjectModel3D cropObjectModel)
        {
            bool result = false;
            cropObjectModel = null;
            //HObjectModel3D ref_plane3D = new HObjectModel3D();
            //HTuple dist = 0;
            //HTuple ref_pose = null;
            HObjectModel3D  removeObjectModel = null;
            //HObjectModel3D fitPlaneObjectModel = null;
            // planePoseNormal, homMat3D, homMat3DRotate;
            try
            {
                //////////////////////////////////////////////////
                if (ObjectModel == null)
                {
                    throw new ArgumentNullException("ObjectModel");
                }
                if (!ObjectModel.IsInitialized())
                {
                    throw new ArgumentException("ObjectModel 未初始化!");
                }
                //////////////////////////   
                HRegion hRegion = null;
                HRegion reserveRegion = new HRegion(); // 保留区域
                HRegion removeRegion = new HRegion(); // 移除区域
                reserveRegion.GenEmptyRegion();
                removeRegion.GenEmptyRegion();
                foreach (var item in paramList)
                {
                    switch (item.InsideOrOutside)
                    {
                        default:
                        case enInsideOrOutside.保留:
                            switch (item.ShapeType)
                            {
                                case enShapeType.椭圆:
                                case enShapeType.圆:
                                case enShapeType.矩形1:
                                case enShapeType.矩形2:
                                case enShapeType.多边形:
                                case enShapeType.点:
                                case enShapeType.线:
                                    hRegion = (item.RoiShape).GetPixROI(new CameraParam(CamParam, CamPose)).GetRegion();
                                    reserveRegion = reserveRegion.ConcatObj(hRegion);
                                    break;
                                default:
                                    continue;
                            }
                            break;
                        case enInsideOrOutside.移除:
                            switch (item.ShapeType)
                            {
                                case enShapeType.椭圆:
                                case enShapeType.圆:
                                case enShapeType.矩形1:
                                case enShapeType.矩形2:
                                case enShapeType.多边形:
                                case enShapeType.点:
                                case enShapeType.线:
                                    hRegion = (item.RoiShape).GetPixROI(new CameraParam(CamParam, CamPose)).GetRegion();
                                    removeRegion = removeRegion.ConcatObj(hRegion);
                                    break;
                                default:
                                    continue;
                            }
                            break;
                    }
                }
                removeObjectModel = ObjectModel.ReduceObjectModel3dByView(removeRegion.Union1().Complement(), CamParam.GetHCamPar(), CamPose.getHPose()); // 获取移除区域后的对象
                if (reserveRegion.CountObj() > 1)
                    cropObjectModel = removeObjectModel.ReduceObjectModel3dByView(reserveRegion.Union1(), CamParam.GetHCamPar(), CamPose.getHPose()); // 获取保留的区域对象
                else
                    cropObjectModel = removeObjectModel.Clone();
                reserveRegion?.Dispose();
                removeRegion?.Dispose();
                //////////////////////////////
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                /////////////////////
                removeObjectModel?.ClearObjectModel3d();
                //reserveObjectModel?.ClearObjectModel3d();
                //fitPlaneObjectModel?.ClearObjectModel3d();
                //ref_plane3D?.ClearObjectModel3d();
            }
            return result;
        }

        public bool CropObjectModel3D(HObjectModel3D[] ObjectModel, BindingList<CropParam> paramList, userCamParam CamParam, userCamPose CamPose, out HObjectModel3D[] cropObjectModel)
        {
            bool result = false;
            cropObjectModel = null;
            //HObjectModel3D ref_plane3D = new HObjectModel3D();
            HTuple dist = 0;
            //HObjectModel3D fitPlaneObjectModel = null;
            //HObjectModel3D unionObjectModel = null;
            HObjectModel3D[] removeObjectModel = null;
            //HTuple planePoseNormal, homMat3D, homMat3DRotate;
            try
            {
                //////////////////////////////////////////////////
                if (ObjectModel == null)
                {
                    throw new ArgumentNullException("ObjectModel");
                }
                //////////////////////////   
                HRegion hRegion = null;
                //unionObjectModel = HObjectModel3D.UnionObjectModel3d(ObjectModel, "points_surface");
                HRegion reserveRegion = new HRegion(); // 保留区域
                HRegion removeRegion = new HRegion(); // 移除区域
                reserveRegion.GenEmptyRegion();
                removeRegion.GenEmptyRegion();
                foreach (var item in paramList)
                {
                    switch (item.InsideOrOutside)
                    {
                        default:
                        case enInsideOrOutside.保留:
                            switch (item.ShapeType)
                            {
                                case enShapeType.椭圆:
                                case enShapeType.圆:
                                case enShapeType.矩形1:
                                case enShapeType.矩形2:
                                case enShapeType.多边形:
                                case enShapeType.点:
                                case enShapeType.线:
                                    hRegion = (item.RoiShape).GetPixROI(new CameraParam(CamParam, CamPose)).GetRegion();
                                    reserveRegion = reserveRegion.ConcatObj(hRegion);
                                    break;
                                default:
                                    continue;
                            }
                            break;
                        case enInsideOrOutside.移除:
                            switch (item.ShapeType)
                            {
                                case enShapeType.椭圆:
                                case enShapeType.圆:
                                case enShapeType.矩形1:
                                case enShapeType.矩形2:
                                case enShapeType.多边形:
                                case enShapeType.点:
                                case enShapeType.线:
                                    hRegion = (item.RoiShape).GetPixROI(new CameraParam(CamParam, CamPose)).GetRegion();
                                    removeRegion = removeRegion.ConcatObj(hRegion);
                                    break;
                                default:
                                    continue;
                            }
                            break;
                    }
                }
                HTuple count = reserveRegion.CountObj();
                removeObjectModel = HObjectModel3D.ReduceObjectModel3dByView(removeRegion.Union1().Complement(), ObjectModel, CamParam.GetHCamPar(), new HPose[] { CamPose.getHPose() });
                if (reserveRegion.CountObj() > 1)
                {
                    cropObjectModel = HObjectModel3D.ReduceObjectModel3dByView(reserveRegion.Union1(), removeObjectModel, CamParam.GetHCamPar(), new HPose[] { CamPose.getHPose() });
                }
                else
                {
                    cropObjectModel = new HObjectModel3D[removeObjectModel.Length];
                    for (int i = 0; i < cropObjectModel.Length; i++)
                    {
                        cropObjectModel[i] = removeObjectModel[i].Clone();
                    }
                }
                //removeObjectModel = unionObjectModel.ReduceObjectModel3dByView(removeRegion.Union1().Complement(), CamParam.GetHCamPar(), CamPose.getHPose()); // 获取移除区域后的对象
                //cropObjectModel = removeObjectModel.ReduceObjectModel3dByView(reserveRegion.Union1(), CamParam.GetHCamPar(), CamPose.getHPose()); // 获取保留的区域对象
                reserveRegion?.Dispose();
                removeRegion?.Dispose();
                ////
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                /////////////////////
                //removeObjectModel?.ClearObjectModel3d();
                //reserveObjectModel?.ClearObjectModel3d();
                HObjectModel3D.ClearObjectModel3d(removeObjectModel);
                //fitPlaneObjectModel?.ClearObjectModel3d();
                //ref_plane3D?.ClearObjectModel3d();
                //unionObjectModel?.ClearObjectModel3d();
            }
            return result;
        }



    }

}
