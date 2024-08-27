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
    public class LineCrop : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D[] dataHandle3D = null;
        private double dist_offset = 0.02;
        private double offsetValue = 0;
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
        public double OffsetValue
        {
            get
            {
                return offsetValue;
            }

            set
            {
                offsetValue = value;
            }
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
            return listObjectModel3D.ToArray();//HObjectModel3D.UnionObjectModel3d(listObjectModel3D.ToArray(), "points_surface");
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


        public bool lineCrop(HObjectModel3D[] objectModel, userWcsLine refLine, HTuple dist_offset, out HObjectModel3D[] sectionModel3D)
        {
            bool result = false;
            sectionModel3D = null;
            HObjectModel3D Plane3D = null;
            HTuple Phi, deg, meanX, meanY;
            HTuple planePose;
            HTuple has_triangles;
            //////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.Length == 0)
            {
                throw new ArgumentNullException("objectModel：对象中不包含元素");
            }
            sectionModel3D = new HObjectModel3D[objectModel.Length];
            /////////////////////////////
            HOperatorSet.LineOrientation(refLine.Y1, refLine.X1, refLine.Y2, refLine.X2, out Phi);
            HOperatorSet.TupleDeg(Phi, out deg);
            HOperatorSet.TupleMean(new HTuple(refLine.X1, refLine.X2), out meanX);
            HOperatorSet.TupleMean(new HTuple(refLine.Y1, refLine.Y2), out meanY);
            HOperatorSet.CreatePose(meanX, meanY, 0, 90, -deg, 0, "Rp+T", "gba", "point", out planePose);
            //////////////////
            Plane3D = new HObjectModel3D();
            Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
            for (int i = 0; i < objectModel.Length; i++)
            {
                if (objectModel[i].GetObjectModel3dParams("num_points").I == 0)
                {
                    continue;
                }
                has_triangles = objectModel[i].GetObjectModel3dParams("has_triangles");
                if (has_triangles.S == "false")
                {
                    objectModel[i].DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                    // 根据到平面的距离来选择对象
                    sectionModel3D[i] = objectModel[i].SelectPointsObjectModel3d("&distance", 0, dist_offset);
                }
                else
                {
                    sectionModel3D[i] = objectModel[i].IntersectPlaneObjectModel3d(new HPose(planePose));
                }
                // 取反截取平面的位姿，使其变换到XY平面上         
                result = true;
            }
            if (Plane3D != null) Plane3D.Dispose();
            return result;
        }



        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = lineCrop(extractRefSource1Data(),extractRefSource2Data(), this.dist_offset, out this.dataHandle3D);
                ////////////////////////////
                for (int i = 0; i < this.dataHandle3D.Length; i++)
                {
                    if (this.dataHandle3D[i] != null && this.dataHandle3D[i].Handle.ToInt64() > 0)
                        this.resultDataTable.Rows.Add(this.name + "-" + i.ToString(), "3D对象", this.dataHandle3D[i].GetObjectModel3dParams("num_points").I, this.dataHandle3D[i].GetObjectModel3dParams("num_points").I, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name + "-" + i.ToString(), "3D对象", 0, 0, 0, 0, "OK");
                }
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
                        item.Dispose();
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
            轮廓3D对象,
        }
    }
}
