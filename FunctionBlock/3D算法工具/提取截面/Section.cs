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
using System.ComponentModel;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    [DefaultProperty("OutObjectModel3D")]
    public class Section : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;
        [NonSerialized]
        private HObjectModel3D outObjectModel3D = null;
        private double dist_offset = 0.02;
        private double offsetValue = 1;
        private DataTable coord1Table = new DataTable();
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

        public DataTable Coord1Table { get => coord1Table; set => coord1Table = value; }

        public BindingList<drawWcsLine> ListLine { get ; set ; }

        [DisplayName("输入3D对象")]
        [DescriptionAttribute("输入属性1")]
        public HObjectModel3D DataHandle3D 
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                        this.dataHandle3D = this.GetPropertyValue(this.RefSource1).Last() as HObjectModel3D;
                    else
                    {
                            this.dataHandle3D = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dataHandle3D;
            }
            set
            {
                dataHandle3D = value;
            }
        }

        [DisplayName("输出3D对象")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D OutObjectModel3D { get => outObjectModel3D; set => outObjectModel3D = value; }

        public Section()
        {
            ////coord1Table.Columns.AddRange(new DataColumn[6] { new DataColumn("X1坐标"), new DataColumn("Y1坐标"), new DataColumn("Z1坐标")
            ////    , new DataColumn("X2坐标"), new DataColumn("Y2坐标"), new DataColumn("Z2坐标") });
            this.ListLine = new BindingList<drawWcsLine>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "点云数量", 0));
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


        private userWcsLine[] TransformPointParamsToLine(userWcsCoordSystem ref_Pose)
        {
            HalconLibrary ha = new HalconLibrary();
            DataRow[] dataRow = this.coord1Table.Select();
            int legnth = dataRow.Length;
            userWcsLine[] line = new userWcsLine[legnth];
            for (int i = 0; i < legnth; i++)
            {
                ha.AffinePoint3D(new userWcsLine(Convert.ToDouble(dataRow[i]["X1坐标"]), Convert.ToDouble(dataRow[i]["Y1坐标"]), Convert.ToDouble(dataRow[i]["Z1坐标"]),
                 Convert.ToDouble(dataRow[i]["X2坐标"]), Convert.ToDouble(dataRow[i]["Y2坐标"]), Convert.ToDouble(dataRow[i]["Z2坐标"])), ref_Pose.GetVariationPose3D(), out line[i]);
            }
            return line;

        }
        // 直线相交3D对象模型

        public bool IntersectLineObjectModel3D(HObjectModel3D objectModel, BindingList<drawWcsLine> line, HTuple dist_offset, out HObjectModel3D sectionModel3D)
        {
            bool result = false;
            sectionModel3D = null;
            HObjectModel3D BoxObjectModel3d = null;
            HObjectModel3D selectObjectModel3D = null;
            List< HObjectModel3D>  sectionArray = null;
            HTuple Z, X, Phi, deg, meanX, meanY;
            HPose rect2Pose;
            HTuple has_triangles;
            //////////////////////
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (objectModel.GetObjectModel3dParams("num_points").I == 0)
            {
                throw new ArgumentException("objectModel 中不包含点");
            }
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            ////////////////////////////////////////////
            sectionArray = new List<HObjectModel3D>();
            foreach (var item in line)
            {
                HOperatorSet.LineOrientation(item.Y1, item.X1, item.Y2, item.X2, out Phi);
                HOperatorSet.TupleDeg(Phi, out deg);
                HOperatorSet.TupleMean(new HTuple(item.X1, item.X2), out meanX);
                HOperatorSet.TupleMean(new HTuple(item.Y1, item.Y2), out meanY);
                double ave_z = objectModel.GetObjectModel3dParams("point_coord_z").TupleMean();
                rect2Pose = new HPose();
                rect2Pose.CreatePose(meanX, meanY, ave_z, 0.0, 0.0, Phi.TupleDeg().D*-1, "Rp+T", "gba", "point");
                double dist = HMisc.DistancePp(item.Y1, item.X1, item.Y2, item.X2);
                //////////////////
                has_triangles = objectModel.GetObjectModel3dParams("has_triangles");
                if (has_triangles.S == "false")
                {
                    BoxObjectModel3d = new HObjectModel3D();
                    //Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
                    BoxObjectModel3d.GenBoxObjectModel3d(rect2Pose, dist, dist_offset, 10000000);
                    objectModel.DistanceObjectModel3d(BoxObjectModel3d, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                    // 根据到平面的距离来选择对象
                    selectObjectModel3D = objectModel.SelectPointsObjectModel3d("&distance", -1000000.0, 0.0);
                    BoxObjectModel3d?.Dispose();
                }
                else
                {
                    selectObjectModel3D = objectModel.IntersectPlaneObjectModel3d(new HPose(rect2Pose));
                }
                // 让XY坐标落在一条直线上
                double[] x, y,z;
                HTuple coord_x = selectObjectModel3D.GetObjectModel3dParams("point_coord_x");
                HTuple coord_y = selectObjectModel3D.GetObjectModel3dParams("point_coord_y");
                HTuple coord_z = selectObjectModel3D.GetObjectModel3dParams("point_coord_z");
                x = new double[coord_x.Length];
                y = new double[coord_x.Length];
                z = new double[coord_x.Length];
                // 拟合XY直线
                double Nr, Nc, Dist, x1, y1, z1, x2, y2, z2, Pro_x, Pro_y;
                new HXLDCont(coord_x, coord_y).FitLineContourXld("tukey", -1, 0, 5, 2, out x1, out y1, out x2, out y2, out Nr, out Nc, out Dist); // 坐标系要符合笛卡尔坐标系
                // 将离散的XY点转换到拟合的直线上，通过点到直线的投影
                for (int i = 0; i < coord_x.Length; i++)
                {
                    HMisc.ProjectionPl(coord_x[i].D, coord_y[i].D, x1, y1, x2, y2, out Pro_x, out Pro_y);
                    x[i] = Pro_x;
                    y[i] = Pro_y;
                    z[i] = 0;
                }
                //selectObjectModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_x"), new HTuple(),new HTuple(x));
                //selectObjectModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_y"), new HTuple(), new HTuple(z));
                //selectObjectModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), new HTuple(), new HTuple(y));
                //sectionArray.Add(selectObjectModel3D);
                selectObjectModel3D?.Dispose();
                sectionArray.Add(new HObjectModel3D(x,y, coord_z));
                /////////////
                coord_x?.Dispose();
                coord_y?.Dispose();
                coord_z?.Dispose();
            }
            sectionModel3D = HObjectModel3D.UnionObjectModel3d(sectionArray.ToArray(), "points_surface");
            foreach (var item in sectionArray)
            {
                item?.Dispose();
            }
            //
            result = true;
            return result;
        }
        public bool IntersectLineObjectModel3D(HObjectModel3D objectModel, userWcsLine line, HTuple dist_offset, out HObjectModel3D sectionModel3D)
        {
            bool result = false;
            sectionModel3D = null;
            HObjectModel3D Plane3D = null;
            HObjectModel3D selectObjectModel3D = null;
            HTuple invertPlanePose;
            HTuple Z, X, Phi, deg, meanX, meanY;
            HTuple planePose;
            HTuple has_triangles;
            //////////////////////
            if (objectModel == null || objectModel.GetObjectModel3dParams("num_points").I == 0) return result;
            HOperatorSet.LineOrientation(line.Y1, line.X1, line.Y2, line.X2, out Phi);
            HOperatorSet.TupleDeg(Phi, out deg);
            HOperatorSet.TupleMean(new HTuple(line.X1, line.X2), out meanX);
            HOperatorSet.TupleMean(new HTuple(line.Y1, line.Y2), out meanY);
            HOperatorSet.CreatePose(meanX, meanY, 0, 90, -deg, 0, "Rp+T", "gba", "point", out planePose);
            //////////////////
            Plane3D = new HObjectModel3D();
            // Plane3D.GenBoxObjectModel3d()
            Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
            has_triangles = objectModel.GetObjectModel3dParams("has_triangles");
            if (has_triangles.S == "false")
            {
                objectModel.DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                // 根据到平面的距离来选择对象
                selectObjectModel3D = objectModel.SelectPointsObjectModel3d("&distance", 0, dist_offset);
            }
            else
            {
                selectObjectModel3D = objectModel.IntersectPlaneObjectModel3d(new HPose(planePose));
            }
            // 取反截取平面的位姿，使其变换到XY平面上
            HOperatorSet.PoseInvert(planePose, out invertPlanePose);
            sectionModel3D = selectObjectModel3D.RigidTransObjectModel3d(new HPose(invertPlanePose));
            if (Plane3D != null) Plane3D.Dispose();
            if (selectObjectModel3D != null) selectObjectModel3D.Dispose();
            ///////////////////////
            X = sectionModel3D.GetObjectModel3dParams("num_points");
            HOperatorSet.TupleGenConst(X.I, 0.0, out Z);
            sectionModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), "", Z); // 要严格按形参来赋值
            //
            result = true;
            return result;
        }
        public bool IntersectLineObjectModel3D(HObjectModel3D[] objectModel, userWcsLine[] line, HTuple dist_offset, out HObjectModel3D[] sectionModel3D)
        {
            bool result = false;
            sectionModel3D = null;
            HObjectModel3D Plane3D = null;
            HObjectModel3D selectObjectModel3D = null;
            HObjectModel3D unionObjectModel3D = null;
            HTuple Z, X, Phi, deg, meanX, meanY;
            HTuple planePose;
            HTuple has_triangles;
            //////////////////////
            HalconLibrary ha = new HalconLibrary();
            if (objectModel == null)
            {
                throw new ArgumentNullException("objectModel");
            }
            if (line == null) throw new ArgumentNullException("line");
            unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            if (unionObjectModel3D.GetObjectModel3dParams("num_points").I == 0)
            {
                throw new ArgumentException("3D对象中不包含点");
            }
            sectionModel3D = new HObjectModel3D[line.Length];
            /////////////////////////////
            for (int i = 0; i < line.Length; i++)
            {
                HOperatorSet.LineOrientation(line[i].Y1, line[i].X1, line[i].Y2, line[i].X2, out Phi);
                HOperatorSet.TupleDeg(Phi, out deg);
                HOperatorSet.TupleMean(new HTuple(line[i].X1, line[i].X2), out meanX);
                HOperatorSet.TupleMean(new HTuple(line[i].Y1, line[i].Y2), out meanY);
                HOperatorSet.CreatePose(meanX, meanY, 0, 90, -deg, 0, "Rp+T", "gba", "point", out planePose);
                //////////////////
                Plane3D = new HObjectModel3D();
                Plane3D.GenPlaneObjectModel3d(new HPose(planePose), new HTuple(), new HTuple());
                has_triangles = unionObjectModel3D.GetObjectModel3dParams("has_triangles");
                if (has_triangles.S == "false")
                {
                    unionObjectModel3D.DistanceObjectModel3d(Plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                    // 根据到平面的距离来选择对象
                    selectObjectModel3D = unionObjectModel3D.SelectPointsObjectModel3d("&distance", 0, dist_offset);
                    ha.FitProfile3D(selectObjectModel3D, out sectionModel3D[i]);
                }
                else
                {
                    sectionModel3D[i] = unionObjectModel3D.IntersectPlaneObjectModel3d(new HPose(planePose));
                }
                sectionModel3D[i].SetObjectModel3dAttribMod(new HTuple("&RefPoint"), "object", new HTuple(line[i].X1, line[i].Y1, line[i].Z1)); 
                // 取反截取平面的位姿，使其变换到XY平面上
                if (Plane3D != null) 
                    Plane3D.Dispose();
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
                ClearHandle(this.outObjectModel3D);
                this.Result.Succss = IntersectLineObjectModel3D(this.DataHandle3D, this.ListLine, this.dist_offset, out this.outObjectModel3D);
                ////////////////////////////
                if (this.outObjectModel3D != null)
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "点云数量", this.outObjectModel3D.GetObjectModel3dParams("num_points").I);
                    this.Result.Succss = true;
                }
                OnExcuteCompleted(this.name, this.dataHandle3D);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
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
                this.outObjectModel3D?.Dispose();
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
          //  throw new NotImplementedException();
        }

        #endregion


    }
}
