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
    public class LineSegmentObjectModle3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D [] dataHandle3D = null;
        private string polarity = "positive";  //"positive" 或 "negative"
        private double plane_offset = 0.0;
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

        private HObjectModel3D [] extractRefSource1Data()
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
            return listObjectModel3D.ToArray(); //"points_surface"
        }
        public userWcsLine extractRefSource2Data()
        {
            userWcsLine wcsLine = new userWcsLine();
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
                        case "userWcsLine":
                            wcsLine = (userWcsLine)object3D;
                            break;
                        case "userWcsLine[]":
                            wcsLine = ((userWcsLine[])object3D)[0];
                            break;
                    }
                }
            }
            return wcsLine;
        }

        /// <summary>
        /// 而距离值的正负与平面的位姿相关,该功能待定
        /// </summary>
        /// <param name="targetObjectModel"></param>
        /// <param name="planeObjectModel"></param>
        /// <param name="planeOffset"></param>
        /// <param name="polarity">"positive" 或 "negative"</param>
        /// <param name="segmentObjectModel3D"></param>
        public bool SegmentObjectModel3DByLine(HObjectModel3D[] targetObjectModel, userWcsLine refLine, string polarity, double planeOffset, out HObjectModel3D[] segmentObjectModel3D)
        {
            bool result = false;
            //将平面部分拟合成一个平面来分割3D对象
            HTuple Phi, deg, meanX, meanY, planePose;
            HObjectModel3D Plane3D = new HObjectModel3D();
            ////////////////
            segmentObjectModel3D = new HObjectModel3D[0];
            //////////////////////////////////
            if (targetObjectModel == null)
            {
                throw new ArgumentNullException("targetObjectModel");
            }
            if (targetObjectModel.Length == 0)
            {
                throw new ArgumentNullException("targetObjectModel:对象中不包含元素");
            }
            HOperatorSet.LineOrientation(refLine.Y1, refLine.X1, refLine.Y2, refLine.X2, out Phi);
            HOperatorSet.TupleDeg(Phi, out deg);
            HOperatorSet.TupleMean(new HTuple(refLine.X1, refLine.X2), out meanX);
            HOperatorSet.TupleMean(new HTuple(refLine.Y1, refLine.Y2), out meanY);
            HOperatorSet.CreatePose(meanX, meanY, 0, 90, -deg, 0, "Rp+T", "gba", "point", out planePose);
            //////////////////
            Plane3D = new HObjectModel3D();
            Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
            segmentObjectModel3D = new HObjectModel3D[targetObjectModel.Length];
            /////////////////////////////////////////////////
            for (int i = 0; i < targetObjectModel.Length; i++)
            {
                /////////////(由于同一个平面可以由两个不同的位资决定，而距离值的正负与平面的位姿相关） 
                targetObjectModel[i].DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                // 大于平面的值为在平面上方，小于平面的值为在平面下方
                if (polarity == "positive")
                    segmentObjectModel3D[i] = targetObjectModel[i].SelectPointsObjectModel3d("&distance", 0, 10000000);
                if (polarity == "negative")
                    segmentObjectModel3D[i] = targetObjectModel[i].SelectPointsObjectModel3d("&distance", -1000000, 0);
                /////
                result = true;
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
                Result.Succss = SegmentObjectModel3DByLine(extractRefSource1Data(), extractRefSource2Data(), this.polarity, this.plane_offset, out this.dataHandle3D);
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
                if (this.dataHandle3D != null)
                {
                    foreach (var item in this.dataHandle3D)
                    {
                        item.Dispose();
                    }
                }                
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
