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
    public class CircleCrop : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D[] dataHandle3D = null;
        private double radius = 0.1;
        private string insideOrOutside = "Inside";
        private List<userWcsCircle> listCircle = new List<userWcsCircle>();
        private DataTable coord1Table = new DataTable();
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
 
        public List<userWcsCircle> ListCircle
        {
            get
            {
                return listCircle;
            }

            set
            {
                listCircle = value;
            }
        }

        public DataTable Coord1Table { get => coord1Table; set => coord1Table = value; }

        public CircleCrop()
        {
            Coord1Table.Columns.AddRange(new DataColumn[4] { new DataColumn("X坐标"), new DataColumn("Y坐标"), new DataColumn("Z坐标"), new DataColumn("半径R") });
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
                ////////////////////////////////////////////////////
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HTuple":
                            listObjectModel3D .Add( new HObjectModel3D(((HTuple)object3D).IP));
                            break;
                        case "HObjectModel3D":
                            listObjectModel3D .Add( (HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
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

        private userWcsCircle[] TransformPointToCircle(userWcsCoordSystem ref_Pose)
        {
            HalconLibrary ha = new HalconLibrary();
            DataRow[] dataRow = this.Coord1Table.Select();
            int legnth = dataRow.Length;
            userWcsCircle[]  circle = new userWcsCircle[legnth];
            for (int i = 0; i < legnth; i++)
            {
                ha.AffinePoint3D(new userWcsCircle(Convert.ToDouble(dataRow[i]["X坐标"]), Convert.ToDouble(dataRow[i]["Y坐标"]), Convert.ToDouble(dataRow[i]["Z坐标"]), Convert.ToDouble(dataRow[i]["半径R"])),
                ref_Pose.GetVariationPose3D(), out circle[i]);
            }
            return circle;
        }
        public bool ReduceObjectModel3dByCircle(HObjectModel3D [] objectModel, userWcsCircle[] wcsCircle, string InsideOrOutside, out HObjectModel3D[] reducedObjectModel3D)
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
        public bool ReduceObjectModel3dByCircle(HObjectModel3D objectModel, userWcsCircle wcsCircle, string InsideOrOutside, out HObjectModel3D reducedObjectModel3D)
        {
            bool result = false;
            reducedObjectModel3D = null;
            if (objectModel == null || objectModel.Handle.ToInt64() < 0) return result;
            HObjectModel3D cylinderObjectModel3d = null;
            HPose pose = null;
            //////////////////////////////////
            double ave_z = objectModel.GetObjectModel3dParams("point_coord_z").TupleMean();
            cylinderObjectModel3d = new HObjectModel3D();
            pose = new HPose();
            pose.CreatePose(wcsCircle.X, wcsCircle.Y, ave_z, 0.0, 0.0, 0.0, "Rp+T", "gba", "point");
            cylinderObjectModel3d.GenCylinderObjectModel3d(pose, wcsCircle.Radius, -100000, 100000);
            objectModel.DistanceObjectModel3d(cylinderObjectModel3d, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
            if (InsideOrOutside == "Inside")
                reducedObjectModel3D = objectModel.SelectPointsObjectModel3d("&distance", -1000000.0, 0.0);
            else
                reducedObjectModel3D = objectModel.SelectPointsObjectModel3d("&distance", 0.0, 1000000.0);
            //////////
            if (cylinderObjectModel3d != null) cylinderObjectModel3d.Dispose();
            if (pose != null) cylinderObjectModel3d.Dispose();
            /////////////////////////////////////////
            result = true;
            return result;
        }


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ////////////////////////
                ClearHandle(this.dataHandle3D);
                Result.Succss = ReduceObjectModel3dByCircle(extractRefSource1Data(), TransformPointToCircle(extractRefSource2Data()), this.insideOrOutside, out this.dataHandle3D);
                ////////
                //this.ResultDataTable.Rows.Clear();
                if (this.dataHandle3D.Length > 0)
                {
                    for (int i = 0; i < this.dataHandle3D.Length; i++)
                    {
                        //if (this.dataHandle3D[i].Handle.ToInt64() > 0)
                            //this.ResultDataTable.Rows.Add(this.name, "点云数量", this.dataHandle3D[i].GetObjectModel3dParams("num_points").I,0,0,0,"OK");
                    }
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
                case "截取3D对象":
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
    }
}
