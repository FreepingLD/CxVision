using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using System.ComponentModel;
using AlgorithmsLibrary;
using System.Data;

namespace FunctionBlock
{
    [Serializable]
    public class GetPrimitive3DPose : BaseFunction, IFunction
    {
        private userWcsPose wcsPose;
        private string primitiveType = "plane";
        private string fittingAlgorithm = "least_squares_huber";
        private string output_point_coord = "false";


        public string PrimitiveType
        {
            get
            {
                return primitiveType;
            }

            set
            {
                primitiveType = value;
            }
        }
        public string FittingAlgorithm
        {
            get
            {
                return fittingAlgorithm;
            }

            set
            {
                fittingAlgorithm = value;
            }
        }


        public GetPrimitive3DPose()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }
        public bool FitPrimitive(HTuple objectModel, out userWcsPose wcsPose)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            HTuple planeobjectModel = null;
            HTuple pose, isPrimitive, primitiveType, planePoseNormal,homMat3D,homMat3DRotate;
            wcsPose = new userWcsPose();
            try
            {
                if (objectModel == null || objectModel.Length == 0) return result;
                HOperatorSet.FitPrimitivesObjectModel3d(objectModel, new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(this.primitiveType, this.fittingAlgorithm, this.output_point_coord, 0.01, 100000), out planeobjectModel);
                HOperatorSet.GetObjectModel3dParams(planeobjectModel, "has_primitive_data", out isPrimitive);
                HOperatorSet.GetObjectModel3dParams(planeobjectModel, "primitive_type", out primitiveType);
                if (isPrimitive.S == "true")
                {
                    HOperatorSet.GetObjectModel3dParams(planeobjectModel, "primitive_parameter_pose", out pose);
                    if (primitiveType.S == "plane")
                    {
                        HOperatorSet.GetObjectModel3dParams(planeobjectModel, "primitive_parameter", out planePoseNormal);
                        if (planePoseNormal[2] < 0)  // 这一步是为了保证平面的法向向Z轴正方向, 一般来说角度的旋转不会大于180度
                        {
                            HOperatorSet.PoseToHomMat3d(pose, out homMat3D);
                            HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                            HOperatorSet.HomMat3dToPose(homMat3DRotate, out pose);
                        }
                    }
                }
                else
                    pose = null;           
                if (pose != null && pose.Length > 0)
                    wcsPose = new userWcsPose(Convert.ToDouble(pose[0].O), Convert.ToDouble(pose[1].O), Convert.ToDouble(pose[2].O)
                        , Convert.ToDouble(pose[3].O), Convert.ToDouble(pose[4].O), Convert.ToDouble(pose[5].O), Convert.ToInt32(pose[6].O));
                result = true;
            }
            catch
            {
            }
            finally
            {
                ha.ClearObjectModel3D(planeobjectModel);
            }
            return result;
        }


        public bool FitPrimitive(HObjectModel3D [] objectModel, out userWcsPose wcsPose)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            HObjectModel3D fitObjectModel = null;
            HTuple pose, isPrimitive, primitiveType, planePoseNormal, homMat3D, homMat3DRotate;
            wcsPose = new userWcsPose();
            HObjectModel3D unionObjectModel3D;
            try
            {
                if (objectModel == null || objectModel.Length == 0) return result;
                unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
                fitObjectModel =unionObjectModel3D.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm", "output_point_coord", "min_radius", "max_radius"), new HTuple(this.primitiveType, this.fittingAlgorithm, this.output_point_coord, 0.01, 100000));
                isPrimitive = fitObjectModel.GetObjectModel3dParams("has_primitive_data");
                primitiveType = fitObjectModel.GetObjectModel3dParams("primitive_type");
                if (isPrimitive.S == "true")
                {
                    pose = fitObjectModel.GetObjectModel3dParams("primitive_parameter_pose");
                    if (primitiveType.S == "plane")
                    {
                        planePoseNormal = fitObjectModel.GetObjectModel3dParams("primitive_parameter");
                        if (planePoseNormal[2] < 0)  // 这一步是为了保证平面的法向向Z轴正方向, 一般来说角度的旋转不会大于180度
                        {
                            HOperatorSet.PoseToHomMat3d(pose, out homMat3D);
                            HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                            HOperatorSet.HomMat3dToPose(homMat3DRotate, out pose);
                        }
                    }
                }
                else
                    pose = null;
                if (pose != null && pose.Length > 0)
                    wcsPose = new userWcsPose(Convert.ToDouble(pose[0].O), Convert.ToDouble(pose[1].O), Convert.ToDouble(pose[2].O)
                        , Convert.ToDouble(pose[3].O), Convert.ToDouble(pose[4].O), Convert.ToDouble(pose[5].O), Convert.ToInt32(pose[6].O));
                result = true;
            }
            catch
            {
            }

            return result;
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


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            { 
                Result.Succss = FitPrimitive(extractRefSource1Data(),  out this.wcsPose);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Tx", wcsPose.Tx);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Ty", wcsPose.Ty);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Tz", wcsPose.Tz);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Rx", wcsPose.Rx);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Ry", wcsPose.Ry);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Rz", wcsPose.Rz);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Type", wcsPose.Type);
                OnExcuteCompleted(this.name, this.wcsPose);
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
            try
            {
                switch (propertyName)
                {
                    case "名称":
                        return this.name;
                    case "3D坐标系":
                        return this.wcsPose;
                    case "源3D对象":
                    case "输入3D对象":
                        return extractRefSource1Data();
                    //case "基本体3D对象":
                    //    return this.wcsPose;
                    default:
                        if (this.name == propertyName)
                            return this.wcsPose;
                        else
                            return null;
                }
            }
            catch
            {
                throw new Exception();
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

        public enum enPrimitiveType
        {
            cylinder,
            sphere,
            plane,
            all,
        }
        public enum enFitAlgorithm
        {
            least_squares,
            least_squares_huber,
            least_squares_tukey,
        }

    }
}

