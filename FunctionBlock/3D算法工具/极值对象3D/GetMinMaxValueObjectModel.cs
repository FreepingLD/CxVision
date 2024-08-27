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
using System.Threading;
using System.ComponentModel;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class GetMinMaxValueObjectModel : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;
        private enMinMaxMode minMaxMode = enMinMaxMode.极大值;
        private double planeOffsetDist = 0.003;
        private double lineOffsetDist = 0.005;
        private int lineOffsetCount = 5;
        private enOffsetType lineOffsetMethod = enOffsetType.对称偏置;
        private DataTable sectionDataTable = new DataTable();
        private userWcsLine fitLine;
        public enMinMaxMode MinMaxMode
        {
            get
            {
                return minMaxMode;
            }

            set
            {
                minMaxMode = value;
            }
        }
        public DataTable SectionDataTable
        {
            get
            {
                return sectionDataTable;
            }

            set
            {
                sectionDataTable = value;
            }
        }
        public double PlaneOffsetDist
        {
            get
            {
                return planeOffsetDist;
            }

            set
            {
                planeOffsetDist = value;
            }
        }
        public double LineOffsetDist
        {
            get
            {
                return lineOffsetDist;
            }

            set
            {
                lineOffsetDist = value;
            }
        }
        public int LineOffsetCount
        {
            get
            {
                return lineOffsetCount;
            }

            set
            {
                lineOffsetCount = value;
            }
        }
        public enOffsetType LineOffsetMethod
        {
            get
            {
                return lineOffsetMethod;
            }

            set
            {
                lineOffsetMethod = value;
            }
        }


        public GetMinMaxValueObjectModel()
        {
            sectionDataTable.Columns.AddRange(new DataColumn[6] { new DataColumn("X1坐标"), new DataColumn("Y1坐标"), new DataColumn("Z1坐标")
                , new DataColumn("X2坐标"), new DataColumn("Y2坐标"), new DataColumn("Z2坐标") });
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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
        private userWcsLine extractRefSource2Data()
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
                if (object3D != null && object3D is userWcsLine) // 这样做是为了动态获取名称
                    wcsLine = (userWcsLine)object3D;
            }
            return wcsLine;
        }

        private void extractLineData(out userWcsLine[] line)
        {
            DataRow[] dataRow = this.sectionDataTable.Select();
            line = new userWcsLine[dataRow.Length];
            for (int i = 0; i < dataRow.Length; i++)
            {
                line[i].X1 = Convert.ToDouble(dataRow[i][0]);
                line[i].Y1 = Convert.ToDouble(dataRow[i][1]);
                line[i].Z1 = Convert.ToDouble(dataRow[i][2]);
                line[i].X2 = Convert.ToDouble(dataRow[i][3]);
                line[i].Y2 = Convert.ToDouble(dataRow[i][4]);
                line[i].Z2 = Convert.ToDouble(dataRow[i][5]);
            }
        }

        // 要求点间隔是匀均的
        private void segmentObjectModel3D(HTuple objectModel, HTuple pointPitch, out HTuple segmentModel3D)
        {
            HTuple X, Y, Z, MinX, MaxX, Sequence, count, ObjectModel3DThresholded;
            HTuple unionObjectModel = null;
            segmentModel3D = new HTuple();
            //////////////////////
            if (objectModel != null && objectModel.Length > 0)
                HOperatorSet.UnionObjectModel3d(objectModel, "points_surface", out unionObjectModel);
            else
                throw new FormatException();
            try
            {
                ///////////////
                if (new HalconLibrary().IsContainPoint(unionObjectModel))
                {
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_x", out X);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_y", out Y);
                    HOperatorSet.GetObjectModel3dParams(unionObjectModel, "point_coord_z", out Z);
                    /////////////////////
                    HTuple sortX = X.TupleSort();
                    Sequence = new HTuple(sortX[0].D);
                    for (int i = 0; i < sortX.Length - 1; i++)
                    {
                        if (sortX[i + 1] - sortX[i] > 0.003)
                            Sequence.Append(sortX[i + 1]);
                    }
                    MinX = X.TupleMin();
                    MaxX = X.TupleMax();
                    HOperatorSet.TupleGenSequence(MinX, MaxX, pointPitch, out Sequence);
                    ////////////////////////
                    for (int i = 0; i < Sequence.Length; i++)
                    {
                        HOperatorSet.SelectPointsObjectModel3d(unionObjectModel, "point_coord_x", Sequence[i] - 0.001, Sequence[i] + 0.001, out ObjectModel3DThresholded);
                        HOperatorSet.GetObjectModel3dParams(ObjectModel3DThresholded, "num_points", out count);
                        if (count > 5)
                        {
                            segmentModel3D.Append(ObjectModel3DThresholded);
                        }
                    }
                    int aa = 10;
                }
            }
            catch
            {
                throw new HalconException();
            }
        }
        private void ReduceObjectModel3dByLine(HTuple objectModel, HTuple dist_offset, userWcsLine[] wcsLine, out HTuple segmentObjectModel3D)
        {
            segmentObjectModel3D = new HTuple();
            HTuple Phi = null;
            HTuple deg = null;
            HTuple meanX = null;
            HTuple meanY = null;
            HTuple PosePlane = null;
            HTuple objectModel3DPlane = null, ObjectModel3DThresholded, count;
            try
            {
                for (int i = 0; i < wcsLine.Length; i++)
                {
                    HOperatorSet.LineOrientation(wcsLine[i].Y1, wcsLine[i].X1, wcsLine[i].Y2, wcsLine[i].X2, out Phi);
                    HOperatorSet.TupleDeg(Phi, out deg);
                    HOperatorSet.TupleMean(new HTuple(wcsLine[i].X1, wcsLine[i].X2), out meanX);
                    HOperatorSet.TupleMean(new HTuple(wcsLine[i].Y1, wcsLine[i].Y2), out meanY);
                    HOperatorSet.CreatePose(meanX, meanY, 0, 90, -deg, 0, "Rp+T", "gba", "point", out PosePlane);
                    HOperatorSet.GenPlaneObjectModel3d(PosePlane, new HTuple(), new HTuple(), out objectModel3DPlane);
                    HOperatorSet.DistanceObjectModel3d(objectModel, objectModel3DPlane, new HTuple(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                    HOperatorSet.SelectPointsObjectModel3d(objectModel, "&distance", 0, dist_offset, out ObjectModel3DThresholded);
                    HOperatorSet.ClearObjectModel3d(objectModel3DPlane);
                    if (ObjectModel3DThresholded != null && ObjectModel3DThresholded.Length > 0)
                    {
                        HOperatorSet.GetObjectModel3dParams(ObjectModel3DThresholded, "num_points", out count);
                        if (count > 3)
                            segmentObjectModel3D.Append(ObjectModel3DThresholded);
                    }
                    ObjectModel3DThresholded = null;
                }
            }
            catch (Exception e)
            {

            }
        }
        private bool SelectMaxValueProfile(HTuple objectModel, HTuple dist_offset, userWcsLine[] wcsLine, out HTuple selectModel3D)
        {
            bool result = false;
            selectModel3D = null;
            HalconLibrary ha = new HalconLibrary();
            HTuple unionObjectModel = new HTuple();
            HTuple segmentModel3D = null;
            HTuple X, Y, Z, planeObjectModel, ParamValue, ObjectModel3DThresholded;
            try
            {
                if (objectModel == null || objectModel.Length == 0) return result;
                ReduceObjectModel3dByLine(objectModel, dist_offset, wcsLine, out segmentModel3D);
                HOperatorSet.GenPlaneObjectModel3d(new HTuple(0, 0, -1000, 0, 0, 0, 0), new HTuple(), new HTuple(), out planeObjectModel);
                for (int i = 0; i < segmentModel3D.Length; i++)
                {
                    HOperatorSet.GetObjectModel3dParams(segmentModel3D[i], "point_coord_x", out X);
                    HOperatorSet.GetObjectModel3dParams(segmentModel3D[i], "point_coord_y", out Y);
                    HOperatorSet.GetObjectModel3dParams(segmentModel3D[i], "point_coord_z", out Z);
                    //new HXLDCont(Z, Y).SmoothContoursXld(smooth).GetContourXld(out Z, out Y); //保证输入的3D对象经过了滤波
                    //HOperatorSet.SetObjectModel3dAttribMod(segmentModel3D[i], new HTuple("point_coord_x", "point_coord_y", "point_coord_z"), new HTuple(), new HTuple(X, Y, Z));
                    HOperatorSet.DistanceObjectModel3d(segmentModel3D[i], planeObjectModel, new HTuple(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                    HOperatorSet.GetObjectModel3dParams(segmentModel3D[i], "&distance", out ParamValue);
                    //double max = ParamValue.TupleMax();
                    if (this.minMaxMode == enMinMaxMode.极大值)
                        HOperatorSet.SelectPointsObjectModel3d(segmentModel3D[i], "&distance", ParamValue.TupleMax(), ParamValue.TupleMax(), out ObjectModel3DThresholded); //选择最大值的点
                    else
                        HOperatorSet.SelectPointsObjectModel3d(segmentModel3D[i], "&distance", ParamValue.TupleMin(), ParamValue.TupleMin(), out ObjectModel3DThresholded); //选择最小值的点
                    if (ObjectModel3DThresholded != null && ObjectModel3DThresholded.Length > 0)
                        unionObjectModel.Append(ObjectModel3DThresholded);
                    ObjectModel3DThresholded = null;
                }
                HOperatorSet.UnionObjectModel3d(unionObjectModel, "all", out selectModel3D);
                result = true;
            }
            catch
            {

            }
            finally
            {
                ha.ClearObjectModel3D(unionObjectModel);
                ha.ClearObjectModel3D(segmentModel3D);
            }
            return result;
        }

        private void ReduceObjectModel3dByLine(HObjectModel3D[] objectModel, HTuple dist_offset, userWcsLine[] wcsLine, out HObjectModel3D[] segmentObjectModel3D)
        {
            segmentObjectModel3D = null;
            HTuple Phi = null;
            HTuple deg = null;
            HTuple meanX = null;
            HTuple meanY = null;
            HTuple PosePlane = null;
            HObjectModel3D objectModel3DPlane = new HObjectModel3D();
            HObjectModel3D unionObjectModel3D = null;
            HObjectModel3D selectObjectModel3D = null;
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            /////////////////////////////////////////
            if (objectModel == null || objectModel.Length == 0) return;
            unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(objectModel, "points_surface");
            for (int i = 0; i < wcsLine.Length; i++)
            {
                HOperatorSet.LineOrientation(wcsLine[i].Y1, wcsLine[i].X1, wcsLine[i].Y2, wcsLine[i].X2, out Phi);
                HOperatorSet.TupleDeg(Phi, out deg);
                HOperatorSet.TupleMean(new HTuple(wcsLine[i].X1, wcsLine[i].X2), out meanX);
                HOperatorSet.TupleMean(new HTuple(wcsLine[i].Y1, wcsLine[i].Y2), out meanY);
                HOperatorSet.CreatePose(meanX, meanY, 0, 90, -deg, 0, "Rp+T", "gba", "point", out PosePlane);
                objectModel3DPlane.GenPlaneObjectModel3d(new HPose(PosePlane), new HTuple(), new HTuple());
                unionObjectModel3D.DistanceObjectModel3d(objectModel3DPlane, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                selectObjectModel3D = unionObjectModel3D.SelectPointsObjectModel3d("&distance", 0, dist_offset);
                if (selectObjectModel3D != null && selectObjectModel3D.GetObjectModel3dParams("num_points").I > 0)
                    listObjectModel3D.Add(selectObjectModel3D);
            }
            segmentObjectModel3D = listObjectModel3D.ToArray();
        }
        private bool SelectMaxValueProfile(HObjectModel3D[] objectModel, HTuple dist_offset, userWcsLine[] wcsLine, out HObjectModel3D selectModel3D)
        {
            bool result = false;
            selectModel3D = null;
            HObjectModel3D[] segmentModel3D = null;
            HTuple ParamValue;
            List<HObjectModel3D> ObjectModel3DThresholded = new List<HObjectModel3D>();
            HObjectModel3D refPlaneObjectModel = new HObjectModel3D();
            HObjectModel3D selectObjectModel3D = null;
            try
            {
                if (objectModel == null || objectModel.Length == 0) return result;
                ReduceObjectModel3dByLine(objectModel, dist_offset, wcsLine, out segmentModel3D);
                refPlaneObjectModel.GenPlaneObjectModel3d(new HPose(new HTuple(0, 0, -100000, 0, 0, 0, 0)), new HTuple(), new HTuple());
                ////////////////////////////////////
                for (int i = 0; i < segmentModel3D.Length; i++)
                {
                    segmentModel3D[i].DistanceObjectModel3d(refPlaneObjectModel, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "false"));
                    ParamValue = segmentModel3D[i].GetObjectModel3dParams("&distance");
                    //double max = ParamValue.TupleMax();
                    if (this.minMaxMode == enMinMaxMode.极大值)
                    {
                        selectObjectModel3D = segmentModel3D[i].SelectPointsObjectModel3d("&distance", ParamValue.TupleMax(), ParamValue.TupleMax());
                        if (selectObjectModel3D != null && selectObjectModel3D.GetObjectModel3dParams("num_points").I > 0)
                            ObjectModel3DThresholded.Add(selectObjectModel3D);
                    }
                    else
                    {
                        selectObjectModel3D = segmentModel3D[i].SelectPointsObjectModel3d("&distance", ParamValue.TupleMin(), ParamValue.TupleMin());
                        if (selectObjectModel3D != null && selectObjectModel3D.GetObjectModel3dParams("num_points").I > 0)
                            ObjectModel3DThresholded.Add(selectObjectModel3D);
                    }
                }
                if (ObjectModel3DThresholded.Count > 0)
                    selectModel3D = HObjectModel3D.UnionObjectModel3d(ObjectModel3DThresholded.ToArray(), "points_surface");
                result = true;
            }
            catch
            {
            }

            return result;
        }
        private void FitLineObjectModelToLine(HTuple lineObjectModel, out userWcsLine line)
        {
            line = new userWcsLine();
            HalconLibrary ha = new HalconLibrary();
            HTuple X, Y, Z, sortX, sortY, RowBeginZ, ColBeginZ, RowEndZ, ColEndZ, RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
            HObject lineXYContour, lineZContour;
            try
            {
                HOperatorSet.GetObjectModel3dParams(lineObjectModel, "point_coord_x", out X);
                HOperatorSet.GetObjectModel3dParams(lineObjectModel, "point_coord_y", out Y);
                HOperatorSet.GetObjectModel3dParams(lineObjectModel, "point_coord_z", out Z);
                if ((X.TupleMax() - X.TupleMin()) <= (Y.TupleMax() - Y.TupleMin()))
                {
                    ha.sortPairs(X, Y, 2, out sortX, out sortY);
                    HOperatorSet.GenContourPolygonXld(out lineZContour, Z, Y);
                }
                else
                {
                    ha.sortPairs(X, Y, 1, out sortX, out sortY);
                    HOperatorSet.GenContourPolygonXld(out lineZContour, Z, X);
                }
                HOperatorSet.GenContourPolygonXld(out lineXYContour, sortY, sortX); // 一定要先排序后再创建轮廓拟合
                HOperatorSet.FitLineContourXld(lineXYContour, "tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                HOperatorSet.FitLineContourXld(lineZContour, "tukey", -1, 0, 5, 2, out RowBeginZ, out ColBeginZ, out RowEndZ, out ColEndZ, out Nr, out Nc, out Dist);
                //////////////////////////////
                line = new userWcsLine(ColBegin.D, RowBegin.D, RowBeginZ.D, ColEnd.D, RowEnd.D, RowEndZ.D);
            }
            catch
            {

            }
        }



        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            userWcsLine[] line;
            try
            {
                ClearHandle(this.dataHandle3D);
                extractLineData(out line);
                Result.Succss = SelectMaxValueProfile(extractRefSource1Data(), this.planeOffsetDist, line, out this.dataHandle3D);
                ////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "点云数量", this.dataHandle3D.GetObjectModel3dParams("num_points").I);
                //this.resultDataTable.Rows.Clear();
                //this.ResultDataTable.Rows.Add(this.name, "点云数量", this.dataHandle3D.GetObjectModel3dParams("num_points").I, 0, 0, 0, "OK");
                OnExcuteCompleted(this.name, this.dataHandle3D);
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ee);
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
                case "直线对象":
                    return this.fitLine; // 所有的结构体都在二维上计算
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); // 所有的结构体都在二维上计算
                case "3D对象":
                case "输出3D对象":
                    return this.dataHandle3D; //
                default:
                    if (this.name == propertyName)
                        return this.dataHandle3D;
                    else return null;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            userWcsLine[] lineout;
            userWcsLine lineIn;
            HalconLibrary ha = new HalconLibrary();
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                case "清空直线":
                    this.sectionDataTable.Clear();
                    return true;
                case "添加直线":
                    if (value != null)
                    {
                        lineIn = (userWcsLine)value[0];
                        this.sectionDataTable.Rows.Add(lineIn.X1, lineIn.Y1, lineIn.Z1, lineIn.X2, lineIn.Y2, lineIn.Z2);
                        OnExcuteCompleted(this.name, lineIn);
                    }
                    return true;
                case "阵列直线": // 每一个类中必需实现这个标签
                    if (value != null)
                    {
                        ha.GenParallelLine((userWcsLine)value[0], this.lineOffsetMethod, this.lineOffsetDist, this.lineOffsetCount, out lineout);
                        for (int i = 0; i < lineout.Length; i++)
                        {
                            this.sectionDataTable.Rows.Add(lineout[i].X1, lineout[i].Y1, lineout[i].Z1, lineout[i].X2, lineout[i].Y2, lineout[i].Z2);
                        }
                    }
                    else // 
                    {
                        lineIn = extractRefSource2Data();
                        ha.GenParallelLine(lineIn, this.lineOffsetMethod, this.lineOffsetDist, this.lineOffsetCount, out lineout);
                    }
                    OnExcuteCompleted(this.name, lineout);
                    return true;
                case "显示":
                    DataRow[] dataRow = sectionDataTable.Select();
                    userWcsLine[] line = new userWcsLine[dataRow.Length];
                    for (int i = 0; i < dataRow.Length; i++)
                    {
                        line[i] = new userWcsLine(Convert.ToDouble(dataRow[i]["X1坐标"]), Convert.ToDouble(dataRow[i]["Y1坐标"]), Convert.ToDouble(dataRow[i]["Z1坐标"]),
                            Convert.ToDouble(dataRow[i]["X2坐标"]), Convert.ToDouble(dataRow[i]["Y2坐标"]), Convert.ToDouble(dataRow[i]["Z2坐标"]), null, null);
                    }
                    OnExcuteCompleted(this.name, line);
                    return true;
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
            输出3D对象,
            直线对象,
        }
    }
}
