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
    public class AffinePose3D : BaseFunction, IFunction
    {
        private userWcsPose coord_System;
        private double trans_x = 0;
        private double trans__y = 0;
        private double trans__z = 0;
        private double rot_x = 0;
        private double rot_y = 0;
        private double rot_z = 0;
        private string orderOfTransform = "Rp+T";
        private string orderOfRotation = "gba";
        private string viewOfTransform = "point";
        private bool invertPose = false;

        public double TransX
        {
            get
            {
                return trans_x;
            }

            set
            {
                trans_x = value;
            }
        }
        public double TransY
        {
            get
            {
                return trans__y;
            }

            set
            {
                trans__y = value;
            }
        }
        public double TransZ
        {
            get
            {
                return trans__z;
            }

            set
            {
                trans__z = value;
            }
        }
        public double RotX
        {
            get
            {
                return rot_x;
            }

            set
            {
                rot_x = value;
            }
        }
        public double RotY
        {
            get
            {
                return rot_y;
            }

            set
            {
                rot_y = value;
            }
        }
        public double RotZ
        {
            get
            {
                return rot_z;
            }

            set
            {
                rot_z = value;
            }
        }
        public string OrderOfTransform
        {
            get
            {
                return orderOfTransform;
            }

            set
            {
                orderOfTransform = value;
            }
        }
        public string OrderOfRotation
        {
            get
            {
                return orderOfRotation;
            }

            set
            {
                orderOfRotation = value;
            }
        }
        public string ViewOfTransform
        {
            get
            {
                return viewOfTransform;
            }

            set
            {
                viewOfTransform = value;
            }
        }
        public bool InvertPose
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

        public AffinePose3D()
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


        private userWcsPose extractRefSource1Data()
        {
            userWcsPose wcsPose = new userWcsPose();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                //////////////////////////////
                if (object3D == null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userWcsPose":
                            wcsPose = (userWcsPose)object3D;
                            break;
                        case "userWcsCoordSystem":
                            wcsPose = ((userWcsCoordSystem)object3D).GetUserWcsPose3D();
                            break;
                        case "userWcsCoordSystem[]":
                            wcsPose = ((userWcsCoordSystem[])object3D)[0].GetUserWcsPose3D();
                            break;
                    }
                }
            }
            return wcsPose;
        }
        public bool Affine3DPose(userWcsPose ref_Pose, double T_x, double T_y, double T_z, double R_x, double R_y, double R_z, string orderOfTransform, string orderOfRotation, string viewOfTransform, bool isInvert, out HTuple pose)
        {
            HTuple tempPose = null, HomMat3d, homMat3DTranslate, homMat3DRotate_x, homMat3DRotate_y, homMat3DRotate_z;
            HTuple invertPose = null;
            pose = null;
            bool result = false;
            ///////////////////////////
            HOperatorSet.PoseToHomMat3d(new HTuple(ref_Pose.Tx, ref_Pose.Ty, ref_Pose.Tz, ref_Pose.Rx, ref_Pose.Ry, ref_Pose.Rz, ref_Pose.Type), out HomMat3d);
            HOperatorSet.HomMat3dTranslateLocal(HomMat3d, T_x, T_y, T_z, out homMat3DTranslate);
            HOperatorSet.HomMat3dRotateLocal(homMat3DTranslate, Math.PI * R_z / 180, "z", out homMat3DRotate_z);
            HOperatorSet.HomMat3dRotateLocal(homMat3DRotate_z, Math.PI * R_y / 180, "y", out homMat3DRotate_y);
            HOperatorSet.HomMat3dRotateLocal(homMat3DRotate_y, Math.PI * R_x / 180, "x", out homMat3DRotate_x);
            HOperatorSet.HomMat3dToPose(homMat3DRotate_x, out tempPose);
            HOperatorSet.PoseInvert(tempPose, out invertPose);
            if (isInvert)
                pose = invertPose.Clone();
            else
                pose = tempPose.Clone();
            result = true;
            return result;
        }

        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                HTuple Pose = null;
                Result.Succss = Affine3DPose(extractRefSource1Data(), this.trans_x, this.trans__y, this.trans__z, this.rot_x, this.rot_y, this.rot_z, this.orderOfTransform, this.orderOfRotation, this.viewOfTransform, this.invertPose, out Pose);
                this.coord_System = new userWcsPose(Pose);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Tx", Pose[0].D);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Ty", Pose[1].D);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Tz", Pose[2].D);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Rx", Pose[3].D);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Ry", Pose[4].D);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Rz", Pose[5].D);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Type", Pose[6].D);
                ///////////////////
                OnExcuteCompleted(this.name, this.coord_System);
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
                        return this.coord_System;
                    default:
                        if (this.name == propertyName)
                            return this.coord_System;
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
            try
            {
                switch (propertyName)
                {
                    case "X平移":
                        this.trans_x = Convert.ToDouble(value[0]);
                        return true;
                    case "名称":
                        this.name = value[0].ToString();
                        return true;
                    default:
                        return true;
                }
            }
            catch
            {
                throw new Exception();
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
           // throw new NotImplementedException();
        }


        #endregion
        public enum enShowItems
        {
            输入对象,
        }
    }

}
