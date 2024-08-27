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
    public class FaceToFaceDist3D : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private double[] mean_dist_value;

        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string state;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }

        public FaceToFaceDist3D()
        {
            //this.ResultDataTable.Rows.Add(this.name, "距离", 0, this.stdValue, this.upTolerance, this.downTolerance, 0);
            //this.ResultDataTable.ColumnChanged += new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
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
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
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
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }
        public bool CalculateFaceToFaceDist3D(HObjectModel3D[] planeObjectModel1, HObjectModel3D[] planeObjectModel2, out double[] dist)
        {
            bool result = false;
            dist = new double[0];
            if (planeObjectModel1 == null || planeObjectModel1.Length == 0)
            {
                throw new ArgumentNullException("planeObjectModel1", "参数为空或长度为0");
            }
            if (planeObjectModel2 == null || planeObjectModel2.Length == 0)
            {
                throw new ArgumentNullException("planeObjectModel2", "参数为空或长度为0");
            }
            if (planeObjectModel2.Length > 1 && (planeObjectModel1.Length != planeObjectModel2.Length))
            {
                throw new ArgumentException("两参数的长度不一致");
            }
            HalconLibrary ha = new HalconLibrary();
            HPose refPose, targetPose;
            ////////////////////////////
            dist = new double[planeObjectModel1.Length];
            ///////////////////////////////////////////
            for (int i = 0; i < planeObjectModel1.Length; i++)
            {
                
                if (planeObjectModel1[i].GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("对象中不包含数据点", "planeObjectModel1");
                if (planeObjectModel2.Length > 1)
                {
                    if (planeObjectModel2[i].GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("对象中不包含数据点", "planeObjectModel2");
                }
                else
                {
                    if (planeObjectModel2[0].GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("对象中不包含数据点", "planeObjectModel2");
                }  
                ha.GetPlaneObjectModel3DPose(planeObjectModel1[i], out targetPose);
                if (planeObjectModel2.Length > 1)
                    ha.GetPlaneObjectModel3DPose(planeObjectModel2[i], out refPose);
                else
                    ha.GetPlaneObjectModel3DPose(planeObjectModel2[0], out refPose);
                HObjectModel3D ref_plane3D = new HObjectModel3D();
                ref_plane3D.GenPlaneObjectModel3d(refPose, new HTuple(), new HTuple()); // 
                HObjectModel3D tar_plane3D = new HObjectModel3D(targetPose[0].D, targetPose[1].D, targetPose[2].D);
                // 计算平面到平面距离
                tar_plane3D.DistanceObjectModel3d(ref_plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                HTuple value = tar_plane3D.GetObjectModel3dParams("&distance");
                ref_plane3D.Dispose();
                tar_plane3D.Dispose();
                dist[i] = value.D;
            }
            result = true;
            return result;
        }
        public bool CalculateFaceToFaceDistance(HObjectModel3D[] targetObjectModel, HObjectModel3D[] refObjectModel, out HTuple Value)
        {
            bool result = false;
            Value = new HTuple();
            HObjectModel3D unionTargetObjectModel, refPlaneObjectModel3D, targetPlaneObjectModel3D;
            HPose ref_pose = new HPose();
            HTuple targetPose, pose, planePoseNormal;
            HObjectModel3D ref_plane3D = new HObjectModel3D();
            HObjectModel3D tar_plane3D = new HObjectModel3D();
            try
            {
                if (targetObjectModel == null || refObjectModel == null) return result;
                for (int i = 0; i < targetObjectModel.Length; i++)
                {
                    if (targetObjectModel[i].GetObjectModel3dParams("has_primitive_data").S == "false")
                    { }
                    else
                        throw new ArgumentException();
                }
                for (int i = 0; i < refObjectModel.Length; i++)
                {
                    if (refObjectModel[i].GetObjectModel3dParams("has_primitive_data").S == "false")
                    { }
                    else
                        throw new ArgumentException();
                }
                // 以上表示，如果参数类型不一致，则报错
                if (targetObjectModel.Length != refObjectModel.Length) // 长度不相同的情况
                {
                    // 如果参考对象为基本体对象
                    if (refObjectModel[0].GetObjectModel3dParams("has_primitive_data").S == "false") //如果不包含基本对象，则合并
                    {
                        unionTargetObjectModel = HObjectModel3D.UnionObjectModel3d(refObjectModel, "points_surface");
                        refPlaneObjectModel3D = unionTargetObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                        pose = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                        planePoseNormal = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                        if (planePoseNormal[2].D < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                        {
                            HTuple homMat3D, homMat3DRotate;
                            HOperatorSet.PoseToHomMat3d(pose, out homMat3D);
                            HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                            HOperatorSet.HomMat3dToPose(homMat3DRotate, out pose);
                            ref_pose = new HPose(pose);
                        }
                        else
                            ref_pose = new HPose(pose);
                    }
                    else
                    {
                        pose = refObjectModel[0].GetObjectModel3dParams("primitive_pose");
                        planePoseNormal = refObjectModel[0].GetObjectModel3dParams("primitive_parameter");
                        if (planePoseNormal[2].D < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                        {
                            HTuple homMat3D, homMat3DRotate;
                            HOperatorSet.PoseToHomMat3d(pose, out homMat3D);
                            HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                            HOperatorSet.HomMat3dToPose(homMat3DRotate, out pose);
                            ref_pose = new HPose(pose);
                        }
                        else
                            ref_pose = new HPose(pose);
                    }
                    ref_plane3D.GenPlaneObjectModel3d(ref_pose, new HTuple(), new HTuple()); //生成参考平面
                    // 计算平面到平面距离
                    for (int i = 0; i < targetObjectModel.Length; i++)
                    {
                        targetPlaneObjectModel3D = targetObjectModel[i].FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                        targetPose = targetPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                        if (targetPose.Length > 0)
                            tar_plane3D.GenObjectModel3dFromPoints(targetPose[0].D, targetPose[1].D, targetPose[2].D);
                        else
                            tar_plane3D.GenObjectModel3dFromPoints(0.0, 0.0, 0.0);
                        tar_plane3D.DistanceObjectModel3d(ref_plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                        Value.Append(tar_plane3D.GetObjectModel3dParams("&distance").D);
                        ///////////////////////////////
                        if (targetPlaneObjectModel3D != null) targetPlaneObjectModel3D.Dispose();
                        if (tar_plane3D != null) tar_plane3D.Dispose();
                    }
                    result = true;
                }
                else // 长度相同的情况 
                {
                    for (int i = 0; i < targetObjectModel.Length; i++)
                    {
                        refPlaneObjectModel3D = refObjectModel[i].FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                        pose = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                        planePoseNormal = refPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                        if (planePoseNormal[2].D < 0)  // 这一步是为了保证平面的法向向Z轴正方向
                        {
                            HTuple homMat3D, homMat3DRotate;
                            HOperatorSet.PoseToHomMat3d(pose, out homMat3D);
                            HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                            HOperatorSet.HomMat3dToPose(homMat3DRotate, out pose);
                            ref_pose = new HPose(pose);
                        }
                        else
                            ref_pose = new HPose(pose);
                        //生成参考平面
                        ref_plane3D.GenPlaneObjectModel3d(ref_pose, new HTuple(), new HTuple());
                        ///创建目标平面
                        targetPlaneObjectModel3D = targetObjectModel[i].FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_huber"));
                        targetPose = targetPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                        if (targetPose.Length > 0)
                            tar_plane3D.GenObjectModel3dFromPoints(targetPose[0].D, targetPose[1].D, targetPose[2].D);
                        else
                            tar_plane3D.GenObjectModel3dFromPoints(0.0, 0.0, 0.0);
                        tar_plane3D.DistanceObjectModel3d(ref_plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                        Value.Append(tar_plane3D.GetObjectModel3dParams("&distance").D);
                        ///////////////////////////////
                        if (targetPlaneObjectModel3D != null) targetPlaneObjectModel3D.Dispose();
                        if (refPlaneObjectModel3D != null) refPlaneObjectModel3D.Dispose();
                        if (ref_plane3D != null) ref_plane3D.Dispose();
                        if (tar_plane3D != null) tar_plane3D.Dispose();
                    }
                    result = true;
                }
            }
            catch
            {
                result = false;
                //throw new HalconException();
            }
            return result;
        }

        private void resultDataTable_DataColumnChange(object sender, DataColumnChangeEventArgs e)
        {
            switch (e.Column.ColumnName)
            {
                case "标准值":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.stdValue))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "上偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.upTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "下偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.downTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
            }
        }


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Result.Succss = CalculateFaceToFaceDist3D(extractRefSource1Data(), extractRefSource2Data(), out this.mean_dist_value);
                //this.ResultDataTable.Clear();
                for (int i = 0; i < this.mean_dist_value.Length; i++)
                {
                    if (this.stdValue + this.downTolerance <= this.mean_dist_value[i] && this.mean_dist_value[i] <= this.stdValue + this.upTolerance)
                        this.state = "OK";
                    else
                        this.state = "NG";
                    //this.ResultDataTable.Rows.Add(this.name + i.ToString(), "距离",  this.mean_dist_value[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                }
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
                case "平均值":
                    return this.mean_dist_value; //
                case "平面3D对象1":
                    return extractRefSource1Data(); //
                case "平面3D对象2":
                    return extractRefSource2Data(); //
                default:
                    if (this.name == propertyName)
                        return this.mean_dist_value;
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
                //this.ResultDataTable.ColumnChanged -= new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
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
           // throw new NotImplementedException();
        }

        #endregion
        public enum enShowItems
        {
            平面3D对象1,
            平面3D对象2,
        }
    }
}
