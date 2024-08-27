using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using Common;
using System.Data;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class SegmentObjectModle3DAlgorith
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;
        private enSegmentPolarity polarity = enSegmentPolarity.positive;  //"positive" 或 "negative"
        private double planeOffset = 0.0;
        private enKeepRegion insideOrOutside = enKeepRegion.Inside;

        public enSegmentPolarity Polarity
        {
            get
            {
                return polarity;
            }

            set
            {
                polarity = value;
            }
        }
        public double PlaneOffset
        {
            get
            {
                return planeOffset;
            }

            set
            {
                planeOffset = value;
            }
        }
        public enKeepRegion InsideOrOutside { get => insideOrOutside; set => insideOrOutside = value; }




        /// <summary>
        /// 而距离值的正负与平面的位姿相关,该功能待定
        /// </summary>
        /// <param name="targetObjectModel"></param>
        /// <param name="planeObjectModel"></param>
        /// <param name="planeOffset"></param>
        /// <param name="polarity">"positive" 或 "negative"</param>
        /// <param name="segmentObjectModel3D"></param>
        public bool SegmentObjectModel3DByPlane(HObjectModel3D targetObjectModel, HObjectModel3D planeObjectModel, HTuple polarity, HTuple planeOffset, out HObjectModel3D segmentObjectModel3D)
        {
            bool result = false;
            //将平面部分拟合成一个平面来分割3D对象
            HObjectModel3D fitPlaneObjectModel3D = null;
            HTuple planePose = null;
            HTuple planePoseNormal = null;
            HObjectModel3D Plane3D = new HObjectModel3D();
            ////////////////
            segmentObjectModel3D = null;
            HTuple homMat3D, homMat3DRotate;
            HTuple primitive_data;
            //////////////////////////////////
            if (targetObjectModel == null || targetObjectModel.GetObjectModel3dParams("num_points").I == 0) return result;
            if (planeObjectModel == null || planeObjectModel.GetObjectModel3dParams("num_points").I == 0) return result;

            /////////////
            primitive_data = planeObjectModel.GetObjectModel3dParams("has_primitive_data");
            if (primitive_data.S == "true")
            {
                planePose = planeObjectModel.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = planeObjectModel.GetObjectModel3dParams("primitive_parameter");
            }
            else
            {
                fitPlaneObjectModel3D = planeObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_tukey"));
                planePose = fitPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = fitPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                if (fitPlaneObjectModel3D != null) fitPlaneObjectModel3D.Dispose();
            }
            if (planePoseNormal.Length == 0) return result;
            if (planePoseNormal[2].D < 0)
            {
                HOperatorSet.PoseToHomMat3d(planePose, out homMat3D);
                HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                HOperatorSet.HomMat3dToPose(homMat3DRotate, out planePose);
            }
            planePose[2] = planePose[2] + planeOffset;
            Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
            /////////////(由于同一个平面可以由两个不同的位资决定，而距离值的正负与平面的位姿相关） 
            targetObjectModel.DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
            // 大于平面的值为在平面上方，小于平面的值为在平面下方
            if (polarity.S == "positive")
                segmentObjectModel3D = targetObjectModel.SelectPointsObjectModel3d("&distance", 0, 10000000);
            if (polarity.S == "negative")
                segmentObjectModel3D = targetObjectModel.SelectPointsObjectModel3d("&distance", -1000000, 0);
            /////
            Plane3D.Dispose();
            result = true;
            return result;
        }
        public bool SegmentObjectModel3DByPlane(HObjectModel3D targetObjectModel, userWcsRectangle2[] rect2, string polarity, double planeOffset, out HObjectModel3D segmentObjectModel3D)
        {
            bool result = false;
            //将平面部分拟合成一个平面来分割3D对象
            HObjectModel3D fitPlaneObjectModel3D = null;
            HObjectModel3D planeObjectModel = null;
            HObjectModel3D[] reduceObjectModel = null;
            HTuple planePose = null;
            HTuple planePoseNormal = null;
            HObjectModel3D Plane3D = new HObjectModel3D();
            ////////////////
            segmentObjectModel3D = null;
            HTuple homMat3D, homMat3DRotate;
            try
            {
                //////////////////////////////////
                if (targetObjectModel == null || targetObjectModel.GetObjectModel3dParams("num_points").I == 0) return result;
                if (rect2 == null || rect2.Length == 0) return result;
                /////////////////////////////////////////////////////
                //new Rectangle2Crop().ReduceObjectModel3dByRectangle2(targetObjectModel, rect2, this.insideOrOutside.ToString(), out reduceObjectModel); //  获取分割平面 
                /////////////
                planeObjectModel = HObjectModel3D.UnionObjectModel3d(reduceObjectModel, "points_surface");
                fitPlaneObjectModel3D = planeObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_tukey"));
                planePose = fitPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = fitPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                /////////////
                if (planePoseNormal.Length == 0) return result;
                if (planePoseNormal[2].D < 0)
                {
                    HOperatorSet.PoseToHomMat3d(planePose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out planePose);
                }
                planePose[2] = planePose[2] + planeOffset;
                Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
                /////////////(由于同一个平面可以由两个不同的位资决定，而距离值的正负与平面的位姿相关） 
                targetObjectModel.DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                // 大于平面的值为在平面上方，小于平面的值为在平面下方
                if (polarity == "positive")
                    segmentObjectModel3D = targetObjectModel.SelectPointsObjectModel3d("&distance", 0, 10000000);
                if (polarity == "negative")
                    segmentObjectModel3D = targetObjectModel.SelectPointsObjectModel3d("&distance", -1000000, 0);
                /////
                result = true;
            }
            finally
            {
                if(reduceObjectModel!=null)
                {
                    for (int i = 0; i < reduceObjectModel.Length; i++)
                        reduceObjectModel[i].Dispose();
                }
                if (fitPlaneObjectModel3D != null) fitPlaneObjectModel3D.Dispose();
                if (planeObjectModel != null) planeObjectModel.Dispose();
                if (Plane3D != null) Plane3D.Dispose();
            }
            return result;
        }
        public bool SegmentObjectModel3DByPlane(HObjectModel3D targetObjectModel, userWcsRectangle2[] rect2,  out HObjectModel3D segmentObjectModel3D)
        {
            bool result = false;
            //将平面部分拟合成一个平面来分割3D对象
            HObjectModel3D fitPlaneObjectModel3D = null;
            HObjectModel3D planeObjectModel = null;
            HObjectModel3D[] reduceObjectModel = null;
            HTuple planePose = null;
            HTuple planePoseNormal = null;
            HObjectModel3D Plane3D = new HObjectModel3D();
            ////////////////
            segmentObjectModel3D = null;
            HTuple homMat3D, homMat3DRotate;
            try
            {
                //////////////////////////////////
                if (targetObjectModel == null || targetObjectModel.GetObjectModel3dParams("num_points").I == 0) return result;
                if (rect2 == null || rect2.Length == 0) return result;
                /////////////////////////////////////////////////////
                //new Rectangle2Crop().ReduceObjectModel3dByRectangle2(targetObjectModel, rect2, this.insideOrOutside.ToString(), out reduceObjectModel); //  获取分割平面 
                /////////////
                planeObjectModel = HObjectModel3D.UnionObjectModel3d(reduceObjectModel, "points_surface");
                fitPlaneObjectModel3D = planeObjectModel.FitPrimitivesObjectModel3d(new HTuple("primitive_type", "fitting_algorithm"), new HTuple("plane", "least_squares_tukey"));
                planePose = fitPlaneObjectModel3D.GetObjectModel3dParams("primitive_pose");
                planePoseNormal = fitPlaneObjectModel3D.GetObjectModel3dParams("primitive_parameter");
                /////////////
                if (planePoseNormal.Length == 0) return result;
                if (planePoseNormal[2].D < 0)
                {
                    HOperatorSet.PoseToHomMat3d(planePose, out homMat3D);
                    HOperatorSet.HomMat3dRotateLocal(homMat3D, Math.PI, "x", out homMat3DRotate);
                    HOperatorSet.HomMat3dToPose(homMat3DRotate, out planePose);
                }
                planePose[2] = planePose[2] + this.planeOffset;
                Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
                /////////////(由于同一个平面可以由两个不同的位资决定，而距离值的正负与平面的位姿相关） 
                targetObjectModel.DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                // 大于平面的值为在平面上方，小于平面的值为在平面下方
                if (this.polarity == enSegmentPolarity.positive)
                    segmentObjectModel3D = targetObjectModel.SelectPointsObjectModel3d("&distance", 0, 10000000);
                if (this.polarity == enSegmentPolarity.negative)
                    segmentObjectModel3D = targetObjectModel.SelectPointsObjectModel3d("&distance", -1000000, 0);
                /////
                result = true;
            }
            finally
            {
                if (reduceObjectModel != null)
                {
                    for (int i = 0; i < reduceObjectModel.Length; i++)
                        reduceObjectModel[i].Dispose();
                }
                if (fitPlaneObjectModel3D != null) fitPlaneObjectModel3D.Dispose();
                if (planeObjectModel != null) planeObjectModel.Dispose();
                if (Plane3D != null) Plane3D.Dispose();
            }
            return result;
        }




    }
}
