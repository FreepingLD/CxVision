using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensor;
using MotionControlCard;
using HalconDotNet;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;
namespace FunctionBlock
{

    /// <summary>
    /// 计算平面度，并返回平面度的值及位姿
    /// </summary>
    [Serializable]
    public class ProfileTolerance : BaseFunction, IFunction
    {
        private double[] value; // 测量结果
        private HObjectModel3D[] dataHandle3D = null;
        private double smoothSigma = 15;
        private double sampleDist = 0.01;
        private enProfileSmoothMethod smoothType = enProfileSmoothMethod.均值平滑;


        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string state;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }
        public double SmoothSigma { get => smoothSigma; set => smoothSigma = value; }
        public double SampleDist { get => sampleDist; set => sampleDist = value; }
        public enProfileSmoothMethod SmoothType { get => smoothType; set => smoothType = value; }

        public ProfileTolerance()
        {
           // this.ResultDataTable.ColumnChanged += new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
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
                    object3D = this.RefSource1[item.Split('.')[0]].GetPropertyValues(item.Split('.')[1]);
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
            return listObjectModel3D.ToArray();
        }
        private HObjectModel3D extractRefSource2Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item.Split('.')[0]].GetPropertyValues(item.Split('.')[1]);
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
            return listObjectModel3D.Last();
        }
        private bool CalculateProfile(HObjectModel3D[] objectModel3D, HObjectModel3D ref_ObjectModel3D, out double[] Profile, out HObjectModel3D[] trans_ObjectModel3D)
        {
            bool result = false;
            if (objectModel3D == null) throw new ArgumentNullException("objectModel3D");
            if (ref_ObjectModel3D == null) throw new ArgumentNullException("ref_ObjectModel3D");
            if (ref_ObjectModel3D.GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("参考对象中不包含3D点", "ref_ObjectModel3D");
            HalconLibrary ha = new HalconLibrary();
            double[] ref_x, ref_y, m_x, m_y, sample_x, sample_y, smooth_x, smooth_y, std_sample_x, std_sample_y;
            ha.TransformModel3DTo2DPoint(ref_ObjectModel3D, 3, out ref_x, out ref_y);
            ha.SampleProfileXY(ref_x, ref_y, this.sampleDist, out std_sample_x, out std_sample_y);
            /////////////////////////////////////////////
            Profile = new double[objectModel3D.Length];
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Qx = null, Qy = null, match_x = null, match_y = null, normal_y2 = null, normal_x2 = null, RowProj = null, ColProj = null;
            double initValue = 10000;
            trans_ObjectModel3D = new HObjectModel3D[objectModel3D.Length];
            double maxDiffDist = 0;
            double[] dist = new double[0];
            for (int i = 0; i < objectModel3D.Length; i++)
            {
                if (objectModel3D[i].GetObjectModel3dParams("num_points").I == 0)
                {
                    Profile[i] = 0;
                    trans_ObjectModel3D[i] = new HObjectModel3D(0.0, 0.0, 0.0);
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod("&std_point_coord_x", "object", 0.0);
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod("&std_point_coord_y", "object", 0.0);
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod("&std_point_coord_z", "object", 0.0);
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod("&ProfileDiff", "object", 0.0);
                    continue;
                }
                ha.TransformModel3DTo2DPoint(objectModel3D[i], 3, out m_x, out m_y);
                ha.SmoothProfileXY(m_x, m_y, smoothType, this.smoothSigma, out smooth_x, out smooth_y);
                ha.SampleProfileXY(smooth_x, smooth_y, this.sampleDist, out sample_x, out sample_y);
                /////////////////////////////////////////////////////////////////////////////////
                // 寻找最小轮廓度位置
                if (std_sample_x.Length < sample_x.Length)  //throw new ArgumentException("标准轮廓点的数量必需大于或等于测量轮廓点的数量");
                {
                    sample_x = new HTuple(sample_x).TupleSelectRange(0, std_sample_x.Length - 1);
                    sample_y = new HTuple(sample_y).TupleSelectRange(0, std_sample_x.Length - 1);
                }
                ///////
                int index = 0;
                for (int ii = 0; ii < std_sample_x.Length - sample_x.Length + 1; ii++)
                {
                    index = ii;
                    match_x = new HTuple(std_sample_x).TupleSelectRange(ii, sample_x.Length - 1 + ii);
                    match_y = new HTuple(std_sample_y).TupleSelectRange(ii, sample_x.Length - 1 + ii);
                    hHomMat2D.VectorToRigid(sample_x, sample_y, match_x, match_y);  // match_x, match_y:为需要变换到该位置的点
                    Qx = hHomMat2D.AffineTransPoint2d(sample_x, sample_y, out Qy);
                    new HXLDCont(match_y, match_x).GenParallelContourXld("regression_normal", 0.01).GetContourXld(out normal_y2, out normal_x2);
                    HMisc.ProjectionPl(Qy, Qx, match_y, match_x, normal_y2, normal_x2, out RowProj, out ColProj);
                    maxDiffDist = HMisc.DistancePp(match_y, match_x, RowProj, ColProj).TupleMax().D;
                    if (maxDiffDist < initValue)
                    {
                        initValue = maxDiffDist;
                        dist = new double[sample_x.Length];
                        for (int k = 0; k < sample_x.Length; k++)
                        {
                            if (RowProj[k].D < match_y[k].D)
                                dist[k] = HMisc.DistancePp(match_y[k].D, match_x[k].D, RowProj[k].D, ColProj[k].D) * -1;
                            else
                                dist[k] = HMisc.DistancePp(match_y[k].D, match_x[k].D, RowProj[k].D, ColProj[k].D);
                        }
                    }
                    else
                    {
                        dist = new double[sample_x.Length];
                        for (int k = 0; k < sample_x.Length; k++)
                        {
                            if (RowProj[k].D < match_y[k].D)
                                dist[k] = HMisc.DistancePp(match_y[k].D, match_x[k].D, RowProj[k].D, ColProj[k].D) * -1;
                            else
                                dist[k] = HMisc.DistancePp(match_y[k].D, match_x[k].D, RowProj[k].D, ColProj[k].D);
                        }
                        break;
                    }
                }

                //////////////////////////////
                if (Qx != null && Qx.Length > 0)
                {
                    Profile[i] = maxDiffDist * 2;
                    trans_ObjectModel3D[i] = new HObjectModel3D(Qx, Qy, HTuple.TupleGenConst(Qx.Length, 0.0));
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod(new HTuple("&std_point_coord_x"), "object", std_sample_x);
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod(new HTuple("&std_point_coord_y"), "object", std_sample_y);
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod(new HTuple("&std_point_coord_z"), "object", HTuple.TupleGenConst(std_sample_x.Length, 0.0));
                    trans_ObjectModel3D[i].SetObjectModel3dAttribMod(new HTuple("&ProfileDiff"), "object", dist);
                }
                result = true;
            }
            return result;
        }
        private void resultDataTable_DataColumnChange(object sender, DataColumnChangeEventArgs e)
        {
            switch (e.Column.ColumnName)
            {
                case "标准值":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.stdValue))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "上偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.upTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "下偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.downTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
            }
        }

        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = CalculateProfile(extractRefSource1Data(), extractRefSource2Data(), out this.value, out this.dataHandle3D);
                ///this.ResultDataTable.Clear();
                for (int i = 0; i < this.value.Length; i++)
                {
                    if (this.stdValue + this.downTolerance <= this.value[i] && this.value[i] <= this.stdValue + this.upTolerance)
                        this.state = "OK";
                    else
                        this.state = "NG";
                   // this.ResultDataTable.Rows.Add(this.name, "轮廓度", this.value[i], this.stdValue, this.upTolerance, this.downTolerance, this.state); // string.Join(",", this.value)
                }
                //////////////////////////////////                           
                OnExcuteCompleted(this.name, dataHandle3D);
                ///////////////////
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
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); //
                case "计算结果":
                    return this.value; //
                default:
                    if (this.name == propertyName)
                        return this.value;
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
                //////////////////////////////////////////////                        
                default:
                    return true;
            }
        }

        public void ReleaseHandle()
        {
            try
            {
                ///this.ResultDataTable.ColumnChanged -= new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
                if (this.dataHandle3D != null)
                {
                    for (int i = 0; i < this.dataHandle3D.Length; i++)
                    {
                        this.dataHandle3D[i].Dispose();
                    }
                }
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
            //throw new NotImplementedException();
        }

        #endregion



        public enum enShowItems
        {
            输入3D对象,
        }
    }
}
