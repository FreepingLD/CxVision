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
using System.Data;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class SegmentObjectModle3DStander : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;
        private string polarity = "positive";  //"positive" 或 "negative"
        private double plane_offset = 0.0;
        private string insideOrOutside = "Inside";
        private DataTable coord1Table = new DataTable();
        public string Polarity
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
        public double Plane_offset
        {
            get
            {
                return plane_offset;
            }

            set
            {
                plane_offset = value;
            }
        }
        public string InsideOrOutside { get => insideOrOutside; set => insideOrOutside = value; }
        public DataTable Coord1Table { get => coord1Table; set => coord1Table = value; }

        private HObjectModel3D extractRefSource1Data()
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
                        case "userWcsPose3D": // 如果是位姿，将其转换成3D对象
                            HObjectModel3D plane3D = new HObjectModel3D();
                            plane3D.GenPlaneObjectModel3d(((userWcsPose)object3D).GetHPose(), new HTuple(), new HTuple());
                            listObjectModel3D.Add(plane3D);
                            break;
                    }
                }
            }
            return HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
        }
        public userWcsCoordSystem extractRefSource2Data()
        {
            userWcsCoordSystem wcsPose = new userWcsCoordSystem();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                //////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userWcsPose":
                            wcsPose = new userWcsCoordSystem((userWcsPose)object3D);
                            break;
                        case "userWcsCoordSystem":
                            wcsPose = ((userWcsCoordSystem)object3D);
                            break;
                        case "userWcsCoordSystem[]":
                            wcsPose = ((userWcsCoordSystem[])object3D)[0];
                            break;
                    }
                }
            }
            return wcsPose;
        }

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
                //new Rectangle2Crop().ReduceObjectModel3dByRectangle2(targetObjectModel, rect2, this.insideOrOutside, out reduceObjectModel);
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
        private userWcsRectangle2[] TransformPointParamsToRect2(userWcsCoordSystem ref_Pose)
        {
            HalconLibrary ha = new HalconLibrary();
            DataRow[] dataRow = this.coord1Table.Select();
            int legnth = dataRow.Length;
            userWcsRectangle2[] rect2 = new userWcsRectangle2[legnth];
            for (int i = 0; i < legnth; i++)
            {
                ha.AffinePoint3D(new userWcsRectangle2(Convert.ToDouble(dataRow[i]["X坐标"]), Convert.ToDouble(dataRow[i]["Y坐标"]), Convert.ToDouble(dataRow[i]["Z坐标"]),
                Convert.ToDouble(dataRow[i]["角度"]), Convert.ToDouble(dataRow[i]["半长lenght1"]), Convert.ToDouble(dataRow[i]["半宽length2"])), ref_Pose.GetCurrentHomMat2D(), out rect2[i]);
            }
            return rect2;
        }


        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = SegmentObjectModel3DByPlane(extractRefSource1Data(), TransformPointParamsToRect2(extractRefSource2Data()), this.polarity, this.plane_offset, out this.dataHandle3D);
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
                case "分割3D对象":
                    return this.dataHandle3D; //
                case "目标3D对象":
                    return extractRefSource1Data(); //
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
                //////////////////////            
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
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

        #endregion
        public enum enShowItems
        {
            输入3D对象,
            分割3D对象,
        }
        public enum enSegmentPolarity
        {
            positive,
            negative,
        }
        public enum enKeepRegion
        {
            Inside,
            Outside,
        }




    }
}
