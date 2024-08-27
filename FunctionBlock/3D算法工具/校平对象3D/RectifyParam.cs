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
    public class RectifyParam
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
    public class RectifyMethod
    {

        /// <summary>
        /// 方法只用来释放内部的参数，不释放传入进去的参数
        /// </summary>
        /// <param name="ObjectModel"></param>
        /// <param name="rectifyObjectModel"></param>
        /// <param name="Pose3D"></param>
        /// <returns></returns>
        public bool RectifyObjectModel3D(HObjectModel3D ObjectModel, BindingList<RectifyParam> paramList, userCamParam CamParam, userCamPose CamPose, out HObjectModel3D rectifyObjectModel, out userWcsPose Pose3D)
        {
            bool result = false;
            rectifyObjectModel = null;
            HObjectModel3D ref_plane3D = new HObjectModel3D();
            HTuple dist = 0;
            HTuple ref_pose = null;
            HObjectModel3D reserveObjectModel = null, removeObjectModel = null;
            HObjectModel3D fitPlaneObjectModel = null;
            Pose3D = new userWcsPose();
            HTuple planePoseNormal, homMat3D, homMat3DRotate;
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
                HRegion reserveRegion = new HRegion();
                HRegion removeRegion = new HRegion();
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
                reserveObjectModel = removeObjectModel.ReduceObjectModel3dByView(reserveRegion.Union1(), CamParam.GetHCamPar(), CamPose.getHPose()); // 获取保留的区域对象
                reserveRegion?.Dispose();
                removeRegion?.Dispose();
                //////////////////////////////
                fitPlaneObjectModel = reserveObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                ref_pose = fitPlaneObjectModel.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = fitPlaneObjectModel.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2] < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                {
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                }
                ref_plane3D.GenPlaneObjectModel3d(new HPose(ref_pose), new HTuple(), new HTuple());
                ///////////
                ObjectModel.DistanceObjectModel3d(ref_plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                dist = ObjectModel.GetObjectModel3dParams("&distance");// 获取每一个点的距离值，并将该值设置为每个点的高度值，即完成了校正
                rectifyObjectModel = ObjectModel.SetObjectModel3dAttrib(new HTuple("point_coord_z"), "", dist);
                Pose3D = new userWcsPose(ref_pose);
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
                removeObjectModel?.ClearObjectModel3d();
                reserveObjectModel?.ClearObjectModel3d();
                fitPlaneObjectModel?.ClearObjectModel3d();
                ref_plane3D?.ClearObjectModel3d();
            }
            return result;
        }

        public bool RectifyObjectModel3D(HObjectModel3D[] ObjectModel, BindingList<RectifyParam> paramList, userCamParam CamParam, userCamPose CamPose, out HObjectModel3D rectifyObjectModel, out userWcsPose Pose3D)
        {
            bool result = false;
            rectifyObjectModel = null;
            HObjectModel3D ref_plane3D = new HObjectModel3D();
            HTuple dist = 0;
            HTuple ref_pose = null;
            HObjectModel3D fitPlaneObjectModel = null;
            HObjectModel3D unionObjectModel = null;
            HObjectModel3D reserveObjectModel = null, removeObjectModel = null;
            Pose3D = new userWcsPose();
            HTuple planePoseNormal, homMat3D, homMat3DRotate;
            try
            {
                //////////////////////////////////////////////////
                if (ObjectModel == null)
                {
                    throw new ArgumentNullException("ObjectModel");
                }
                //////////////////////////   
                HRegion hRegion = null;
                unionObjectModel = HObjectModel3D.UnionObjectModel3d(ObjectModel, "points_surface");
                HRegion reserveRegion = new HRegion();
                HRegion removeRegion = new HRegion();
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
                removeObjectModel = unionObjectModel.ReduceObjectModel3dByView(removeRegion.Union1().Complement(), CamParam.GetHCamPar(), CamPose.getHPose()); // 获取移除区域后的对象
                if (reserveRegion.CountObj() > 1)
                    reserveObjectModel = removeObjectModel.ReduceObjectModel3dByView(reserveRegion.Union1(), CamParam.GetHCamPar(), CamPose.getHPose()); // 获取保留的区域对象
                else
                    reserveObjectModel = removeObjectModel.Clone();
                reserveRegion?.Dispose();
                removeRegion?.Dispose();
                //////////////////////////////
                //reserveObjectModel.WriteObjectModel3d("om3", @"C:\Users\华硕\Desktop\123.om3", new HTuple(), new HTuple());
                fitPlaneObjectModel = reserveObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                ref_pose = fitPlaneObjectModel.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = fitPlaneObjectModel.GetObjectModel3dParams("primitive_parameter");
                if (planePoseNormal[2] < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                {
                    HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out ref_pose);
                }
                ref_plane3D.GenPlaneObjectModel3d(new HPose(ref_pose), new HTuple(), new HTuple());
                ///////////
                unionObjectModel.DistanceObjectModel3d(ref_plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                dist = unionObjectModel.GetObjectModel3dParams("&distance");// 获取每一个点的距离值，并将该值设置为每个点的高度值，即完成了校正
                rectifyObjectModel = unionObjectModel.SetObjectModel3dAttrib(new HTuple("point_coord_z"), "", dist);
                Pose3D = new userWcsPose(ref_pose);
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
                removeObjectModel?.ClearObjectModel3d();
                reserveObjectModel?.ClearObjectModel3d();
                fitPlaneObjectModel?.ClearObjectModel3d();
                ref_plane3D?.ClearObjectModel3d();
                unionObjectModel?.ClearObjectModel3d();
            }
            return result;
        }

    }
}
