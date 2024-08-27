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
using Common;
using System.ComponentModel;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class IntersectPlaneObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;
        private double dist_offset = 0.02;
        private double planeZoffset = 0;
        public double Dist_offset
        {
            get
            {
                return dist_offset;
            }

            set
            {
                dist_offset = value;
            }
        }
        public double PlaneZoffset
        {
            get
            {
                return planeZoffset;
            }

            set
            {
                planeZoffset = value;
            }
        }

        public IntersectPlaneObjectModel3D ()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }
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
                    }
                }
            }
            return listObjectModel3D.ToArray(); // HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
        }
        private HObjectModel3D[] extractRefSource2Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                //////////////////////////////
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

        public void CalculatePlanePose(HObjectModel3D planeObjectModel, enFittingAlgorithm fittingAlgorithm, out HTuple planePose)
        {
            planePose = null;
            HObjectModel3D fitPlaneObjectModel = null;
            HTuple planePrimitive;
            HTuple ref_pose = null;
            HTuple planePoseNormal, homMat3D, homMat3DRotate;
            try
            {
                //////////////////
                if (planeObjectModel == null) return;
                planePrimitive = planeObjectModel.GetObjectModel3dParams("has_primitive_data");
                // 以第二个面作为基准面
                if (planePrimitive.S == "false")
                {
                    fitPlaneObjectModel = planeObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", fittingAlgorithm.ToString()));
                    ref_pose = fitPlaneObjectModel.GetObjectModel3dParams("primitive_pose");
                    planePoseNormal = fitPlaneObjectModel.GetObjectModel3dParams("primitive_parameter");
                    if (planePoseNormal[2] < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                    {
                        HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                        HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                        HOperatorSet.HomMat3dToPose(homMat3DRotate, out planePose);
                    }
                    else
                        planePose = ref_pose;
                }
                else
                {
                    ref_pose = planeObjectModel.GetObjectModel3dParams("primitive_pose");
                    planePoseNormal = planeObjectModel.GetObjectModel3dParams("primitive_parameter");
                    if (planePoseNormal[2] < 0)
                    {
                        HOperatorSet.PoseToHomMat3d(ref_pose, out homMat3D);
                        HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                        HOperatorSet.HomMat3dToPose(homMat3DRotate, out planePose);
                    }
                    else
                        planePose = ref_pose;
                }
            }
            catch
            {

            }

        }
        public bool intersectPlaneObjectModel3D(HObjectModel3D objectModel, userWcsPose planePose, HTuple dist_offset, double planeZoffset, out HObjectModel3D sectionModel3D)
        {
            bool result = false;
            sectionModel3D = null;
            HObjectModel3D Plane3D = new HObjectModel3D();
            HObjectModel3D reducedObjectModel3D = null;
            HTuple invertPlanePose;
            HTuple Z, X, hasTriangle;
            //////////////////////
            if (objectModel == null) return result;
            planePose.Tz = planePose.Tz + planeZoffset;
            hasTriangle = objectModel.GetObjectModel3dParams("has_triangles");
            if (hasTriangle.S == "false")
            {
                Plane3D.GenPlaneObjectModel3d(new HPose(new HTuple(planePose.Tx, planePose.Ty, planePose.Tz, planePose.Rx, planePose.Ry, planePose.Rz, planePose.Type)), new HTuple(), new HTuple());
                objectModel.DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                reducedObjectModel3D = objectModel.SelectPointsObjectModel3d("&distance", 0, dist_offset);
            }
            else
            {
                reducedObjectModel3D = objectModel.IntersectPlaneObjectModel3d(new HPose(new HTuple(planePose.Tx, planePose.Ty, planePose.Tz, planePose.Rx, planePose.Ry, planePose.Rz, planePose.Type)));
            }
            HOperatorSet.PoseInvert(new HTuple(planePose.Tx, planePose.Ty, planePose.Tz, planePose.Rx, planePose.Ry, planePose.Rz, planePose.Type), out invertPlanePose);
            if (reducedObjectModel3D != null)
                sectionModel3D = reducedObjectModel3D.RigidTransObjectModel3d(new HPose(invertPlanePose));
            else
                return result;
            ///////////////////////
            X = sectionModel3D.GetObjectModel3dParams("num_points");
            HOperatorSet.TupleGenConst(X.D, 0.0, out Z);
            sectionModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), new HTuple(), Z);
            /////
            result = true;
            return result;
        }
        public bool intersectPlaneObjectModel3D(HObjectModel3D objectModel, HObjectModel3D planeObjectModel, double dist_offset, double planeZoffset, out HObjectModel3D sectionModel3D)
        {
            bool result = false;
            sectionModel3D = null;
            HObjectModel3D Plane3D = new HObjectModel3D();
            HObjectModel3D reducedObjectModel3D = null;
            HTuple invertPlanePose, planePose;
            HTuple Z, X, hasTriangle;
            //////////////////////
            if (objectModel == null) return result;

            CalculatePlanePose(planeObjectModel, enFittingAlgorithm.least_squares_tukey, out planePose);
            planePose[2] = planePose[2].D + planeZoffset;
            hasTriangle = objectModel.GetObjectModel3dParams("has_triangles");
            if (hasTriangle.S == "false")
            {
                Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
                objectModel.DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                reducedObjectModel3D = objectModel.SelectPointsObjectModel3d("&distance", 0, dist_offset);
            }
            else
            {
                reducedObjectModel3D = objectModel.IntersectPlaneObjectModel3d(new HPose(planePose));
            }
            HOperatorSet.PoseInvert(planePose, out invertPlanePose);
            if (reducedObjectModel3D != null)
                sectionModel3D = reducedObjectModel3D.RigidTransObjectModel3d(new HPose(invertPlanePose)); // 变换对象到XY平面
            else
                return result;
            ///////////////////////
            X = sectionModel3D.GetObjectModel3dParams("num_points");
            HOperatorSet.TupleGenConst(X.I, 0.0, out Z);
            sectionModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), new HTuple(), Z);
            ///
            result = true;

            return result;
        }

        private bool intersect(HObjectModel3D[] objectModel, HObjectModel3D[] planeObjectModel, out HObjectModel3D sectionModel3D)
        {
            bool result = false;
            HTuple primitiveData, ref_pose;
            sectionModel3D = null;
            try
            {
                for (int i = 0; i < objectModel.Length; i++)
                {
                    primitiveData = planeObjectModel[i].GetObjectModel3dParams("has_primitive_data");
                    if(primitiveData.S=="true")
                    {
                        ref_pose = planeObjectModel[i].GetObjectModel3dParams("primitive_pose");
                        intersectPlaneObjectModel3D(objectModel[i],new userWcsPose(ref_pose),this.dist_offset,this.planeZoffset,out sectionModel3D);
                    }
                    else
                    {
                        intersectPlaneObjectModel3D(objectModel[i], planeObjectModel[i], this.dist_offset, this.planeZoffset, out sectionModel3D);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                intersect(extractRefSource1Data(), extractRefSource2Data(), out this.dataHandle3D);
                //this.resultDataTable.Rows.Clear();
                //this.ResultDataTable.Rows.Add(this.name, "点云数量", this.dataHandle3D.GetObjectModel3dParams("num_points").I, 0, 0, 0, "OK");
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "点云数量", this.dataHandle3D.GetObjectModel3dParams("num_points").I);
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
                case "轮廓3D对象":
                    return this.dataHandle3D; //
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource2Data(); //
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
                if (this.dataHandle3D != null) this.dataHandle3D.Dispose();
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
           // throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }
        #endregion

        public enum enShowItems
        {
            输入3D对象,
            轮廓3D对象,
        }
    }
}
