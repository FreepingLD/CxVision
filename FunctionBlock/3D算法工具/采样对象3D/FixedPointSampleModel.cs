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
using System.ComponentModel;
using AlgorithmsLibrary;
using Common;
using System.Data;


namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class FixedPointSampleModel : BaseFunction, IFunction
    {
        private DataTable circleDataTable = new DataTable();
        [NonSerialized]
        private HObjectModel3D[] dataHandle3D = null;
        private double radius = 0.1;
        private string insideOrOutside = "Inside";
        private string regionType = "circle";
        private double deg = 0.0;
        private double length1 = 0.1;
        private double length2 = 0.1;
        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }
        public string InsideOrOutside
        {
            get
            {
                return insideOrOutside;
            }

            set
            {
                insideOrOutside = value;
            }
        }
        public DataTable CircleDataTable
        {
            get
            {
                return circleDataTable;
            }

            set
            {
                circleDataTable = value;
            }
        }
        public string RegionType { get => regionType; set => regionType = value; }
        public double Deg { get => deg; set => deg = value; }
        public double Length1 { get => length1; set => length1 = value; }
        public double Length2 { get => length2; set => length2 = value; }

        public FixedPointSampleModel()
        {
            CircleDataTable.Columns.AddRange(new DataColumn[3] { new DataColumn("X坐标"), new DataColumn("Y坐标"), new DataColumn("Z坐标")});
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
        public userWcsPoint [] extractRefSource2Data()
        {
            List<userWcsPoint> wcsPoint = new List<userWcsPoint>();
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
                        case "userWcsPoint[]":
                            wcsPoint.AddRange((userWcsPoint[])object3D); 
                            break;
                        case "userWcsPoint":
                            wcsPoint.Add((userWcsPoint)object3D);
                            break;
                        case "userPixPoint[]":
                            userPixPoint[] pixPoint = (userPixPoint[])object3D;
                            for (int j = 0; j < pixPoint.Length; j++)
                            {
                                wcsPoint.Add(pixPoint[j].GetWcsPoint());
                            }                           
                            break;
                        case "userPixPoint":
                            wcsPoint.Add(((userPixPoint)object3D).GetWcsPoint());
                            break;
                    }
                }
            }
            return wcsPoint.ToArray();
        }


        private bool ReduceObjectModel3dByCircle(HObjectModel3D [] objectModel, userWcsCircle[] wcsCircle, string InsideOrOutside, out HObjectModel3D[] reducedObjectModel3D)
        {
            bool result = false;
            reducedObjectModel3D = null;
            if (objectModel == null || wcsCircle == null || objectModel.Length==0) return result;
            reducedObjectModel3D = null; // new HObjectModel3D[wcsCircle.Length];
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            HObjectModel3D selectObjectModel3d = null;
            HObjectModel3D cylinderObjectModel3d = null;
            HPose pose = null;
            HObjectModel3D unionObjectModel3d = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            double ave_z = unionObjectModel3d.GetObjectModel3dParams("point_coord_z").TupleMean();
            for (int i = 0; i < wcsCircle.Length; i++)
            {
                cylinderObjectModel3d = new HObjectModel3D();
                pose = new HPose();
                pose.CreatePose(wcsCircle[i].X, wcsCircle[i].Y, ave_z, 0.0, 0.0, 0.0, "Rp+T", "gba", "point");
                cylinderObjectModel3d.GenCylinderObjectModel3d(pose, wcsCircle[i].Radius, -100000, 100000);
                unionObjectModel3d.DistanceObjectModel3d(cylinderObjectModel3d, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                if (InsideOrOutside == "Inside")
                    selectObjectModel3d = unionObjectModel3d.SelectPointsObjectModel3d("&distance", -1000000.0, 0.0);
                else
                    selectObjectModel3d = unionObjectModel3d.SelectPointsObjectModel3d("&distance", 0.0, 1000000.0);
                //////////////////////////////////////////////
                if (selectObjectModel3d.GetObjectModel3dParams("num_points").I > 0)
                    listObjectModel3D.Add(selectObjectModel3d);
                //////////
                if (cylinderObjectModel3d != null) cylinderObjectModel3d.Dispose();
                if (pose != null) cylinderObjectModel3d.Dispose();
            }
            reducedObjectModel3D = listObjectModel3D.ToArray();
            if (listObjectModel3D.Count > 0)
                result = true;
            return result;
        }
        private bool ReduceObjectModel3dByRectangle2(HObjectModel3D[] objectModel, userWcsRectangle2[] wcsRect2, string InsideOrOutside, out HObjectModel3D[] reducedObjectModel3D)
        {
            bool result = false;
            reducedObjectModel3D = null;
            if (objectModel == null || wcsRect2 == null || objectModel.Length == 0) return result;
            reducedObjectModel3D = null;
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            HObjectModel3D selectObjectModel3d = null;
            HObjectModel3D BoxObjectModel3d = null;
            HPose pose = null;
            HObjectModel3D unionObjectModel3d = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            double ave_z = unionObjectModel3d.GetObjectModel3dParams("point_coord_z").TupleMean();
            for (int i = 0; i < wcsRect2.Length; i++)
            {
                BoxObjectModel3d = new HObjectModel3D();
                pose = new HPose();
                pose.CreatePose(wcsRect2[i].X, wcsRect2[i].Y, ave_z, 0.0, 0.0, wcsRect2[i].Deg, "Rp+T", "gba", "point");
                BoxObjectModel3d.GenBoxObjectModel3d(pose, wcsRect2[i].Length1 * 2, wcsRect2[i].Length2 * 2, 10000000);
                unionObjectModel3d.DistanceObjectModel3d(BoxObjectModel3d, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                if (InsideOrOutside == "Inside")
                    selectObjectModel3d = unionObjectModel3d.SelectPointsObjectModel3d("&distance", -1000000.0, 0.0);
                else
                    selectObjectModel3d = unionObjectModel3d.SelectPointsObjectModel3d("&distance", 0.0, 1000000.0);
                if (selectObjectModel3d.GetObjectModel3dParams("num_points").I > 0)
                    listObjectModel3D.Add(selectObjectModel3d);
                //////////
                if (BoxObjectModel3d != null) BoxObjectModel3d.Dispose();
                if (pose != null) BoxObjectModel3d.Dispose();
            }
            reducedObjectModel3D = listObjectModel3D.ToArray();
            if (listObjectModel3D.Count > 0)
                result = true;
            return result;
        }

        private bool sampleModel(HObjectModel3D[] objectModel, userWcsPoint[] wcsPoint, string InsideOrOutside, out HObjectModel3D[] reducedObjectModel3D)
        {
            userWcsCircle[] wcsCircle;
            userWcsRectangle2[] wcsRect2;
            reducedObjectModel3D = new HObjectModel3D[0];
            try
            {
                switch (this.regionType)
                {
                    case "circle":
                        wcsCircle = new userWcsCircle[wcsPoint.Length];
                        for (int i = 0; i < wcsPoint.Length; i++)
                        {
                            wcsCircle[i] = new userWcsCircle(wcsPoint[i].X, wcsPoint[i].Y, wcsPoint[i].Z, this.radius, wcsPoint[i].CamParams);
                        }
                        ReduceObjectModel3dByCircle(objectModel, wcsCircle, InsideOrOutside, out reducedObjectModel3D);
                        break;
                    case "rect2":
                        wcsRect2 = new userWcsRectangle2[wcsPoint.Length];
                        for (int i = 0; i < wcsPoint.Length; i++)
                        {
                            wcsRect2[i] = new userWcsRectangle2(wcsPoint[i].X, wcsPoint[i].Y, wcsPoint[i].Z, this.deg, this.length1, this.length2, wcsPoint[i].CamParams);
                        }
                        ReduceObjectModel3dByRectangle2(objectModel, wcsRect2, InsideOrOutside, out reducedObjectModel3D);
                        break;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = sampleModel(extractRefSource1Data(), extractRefSource2Data(), this.insideOrOutside, out this.dataHandle3D);
                ////////
                if (this.dataHandle3D != null)
                {
                    for (int i = 0; i < this.dataHandle3D.Length; i++)
                    {
                        //this.ResultDataTable.Rows.Add(this.name, "点云数量", this.dataHandle3D[i].GetObjectModel3dParams("num_points").I, 0, 0, 0, "OK");
                    }              
                    OnExcuteCompleted(this.name, HObjectModel3D.UnionObjectModel3d(this.dataHandle3D, "points_surface"));
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
                case "圆区域":
                    return this.CircleDataTable;
                case "3D对象":
                case "采样3D对象":
                case "输出3D对象":
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
                    for (int i = 0; i < this.dataHandle3D.Length; i++)
                    {
                        this.dataHandle3D[i].Dispose();
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
            输出3D对象,
        }
        public enum enRegionType
        {
            circle,
            rect2,
        }
    }
}
