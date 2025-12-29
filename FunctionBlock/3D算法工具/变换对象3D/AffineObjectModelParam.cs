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
using System.Data;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class AffineObjectModelParam
    {
        private string _Method = "rigid_trans_object_model_3d";
        private string invertPose = "false";
        public string Method
        {
            get
            {
                return _Method;
            }

            set
            {
                _Method = value;
            }
        }
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

        public AffineObjectModelParam()
        {

        }

        public bool AffineTransObjectModel3D(HObjectModel3D objectModel, userWcsPose WcsPose , out HObjectModel3D AffineObjectModel)
        {
            bool result = false;
            //////////////////////////////////////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel == null)
            {
                throw new ArgumentException("objectModel对象中不包含元素");
            }
            ////////////////////
            HTuple Px, Py, Pz;
            AffineObjectModel = new HObjectModel3D();
            HTuple hTuple = objectModel.GetObjectModel3dParams("&RefPoint");
            if (hTuple != null && hTuple.Length > 0)
            {
                Px = hTuple[0].D;
                Py = hTuple[1].D;
                Pz = hTuple[2].D;
            }
            else
            {
                Px = objectModel.GetObjectModel3dParams("point_coord_x").TupleMax();
                Py = objectModel.GetObjectModel3dParams("point_coord_y").TupleMax();
                Pz = objectModel.GetObjectModel3dParams("point_coord_z").TupleMax();
            }
            HHomMat3D hHomMat3D = new HHomMat3D();
            HHomMat3D homMat3dTranslate = hHomMat3D.HomMat3dTranslate(WcsPose.Tx, WcsPose.Ty, WcsPose.Tz);
            HHomMat3D HomMat3dRotate_z = homMat3dTranslate.HomMat3dRotate(WcsPose.Rz * Math.PI / 180, "z", Px[0].D, Py[0].D, Pz[0].D);
            HHomMat3D HomMat3dRotate_y = HomMat3dRotate_z.HomMat3dRotate(WcsPose.Ry * Math.PI / 180, "y", Px[0].D, Py[0].D, Pz[0].D);
            HHomMat3D HomMat3dRotate_x = HomMat3dRotate_y.HomMat3dRotate(WcsPose.Rx * Math.PI / 180, "x", Px[0].D, Py[0].D, Pz[0].D);
            ////////////////
            switch (this.Method)
            {
                case "rigid_trans_object_model_3d":
                    if (this.InvertPose == "true")
                        AffineObjectModel = objectModel.RigidTransObjectModel3d(HomMat3dRotate_x.HomMat3dInvert().HomMat3dToPose());
                    else
                        AffineObjectModel = objectModel.RigidTransObjectModel3d(HomMat3dRotate_x.HomMat3dToPose());
                    break;
                case "affine_trans_object_model_3d":
                    if (this.InvertPose == "true")
                        AffineObjectModel = objectModel.AffineTransObjectModel3d(HomMat3dRotate_x.HomMat3dInvert());
                    else
                        AffineObjectModel = objectModel.AffineTransObjectModel3d(HomMat3dRotate_x);
                    break;
                case "projective_trans_object_model_3d":
                    if (this.InvertPose == "true")
                        AffineObjectModel = objectModel.ProjectiveTransObjectModel3d(HomMat3dRotate_x.HomMat3dInvert());
                    else
                        AffineObjectModel = objectModel.ProjectiveTransObjectModel3d(HomMat3dRotate_x);
                    break;
            }
            result = true;
            return result;
        }


    }
    public enum enTransMethod
    {
        rigid_trans_object_model_3d,
        projective_trans_object_model_3d,
        affine_trans_object_model_3d,

    }


}
