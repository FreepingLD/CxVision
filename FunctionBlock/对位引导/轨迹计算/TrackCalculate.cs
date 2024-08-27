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
using System.Diagnostics;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(WcsVector))]
    public class TrackCalculate : BaseFunction, IFunction
    {
        private userWcsVector[] _wcsVector;

        private userWcsPoint[] _trackPoint;

        public UserHomMat2D _homMat2D;

        [DisplayName("轨迹点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector[] WcsVector { get => _wcsVector; set => _wcsVector = value; }


        [DisplayName("输入点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] TrackPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userWcsPoint> listPoint = new List<userWcsPoint>();
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    listPoint.Add(item as userWcsPoint);
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    listPoint.Add(new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams));
                                    break;
                                case "userWcsVector[]":
                                    userWcsVector[] wcsVectorA = item as userWcsVector[];
                                    foreach (var item2 in wcsVectorA)
                                    {
                                        listPoint.Add(new userWcsPoint(item2.X, item2.Y, item2.Z, item2.CamParams));
                                    }
                                    break;
                                case "userWcsPoint[]":
                                    listPoint.AddRange(item as userWcsPoint[]);
                                    break;
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = item as userWcsLine;
                                    listPoint.AddRange(wcsLine.GetFitWcsPoint());
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    listPoint.AddRange(wcsCircle.GetFitWcsPoint());
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    listPoint.AddRange(wcsCircleSector.GetFitWcsPoint());
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRec2 = item as userWcsRectangle2;
                                    double[] Px = new double[] { -1, 1, 1, -1 };
                                    double[] Py = new double[] { 1, 1, -1, -1 };
                                    HTuple Qx = null, Qy = null;
                                    HHomMat2D hHomMat2D = new HHomMat2D();
                                    hHomMat2D.VectorAngleToRigid(0, 0, 0, wcsRec2.X, wcsRec2.Y, wcsRec2.Deg * Math.PI / 180);
                                    Qx = hHomMat2D.AffineTransPoint2d(Px, Py, out Qy);
                                    if (Qx != null)
                                    {
                                        for (int i = 0; i < Qx.Length; i++)
                                        {
                                            listPoint.Add(new userWcsPoint(Qx[i].D,Qy[i].D, wcsRec2.Z, wcsRec2.CamParams));
                                        }
                                    }
                                    break;

                            }
                        }
                    }
                    this._trackPoint = listPoint.ToArray();
                    listPoint.Clear();
                }
                return this._trackPoint;
            }
            set
            {
                this._trackPoint = value;
            }
        }

        public TrackCalculateParam Param { get; set; }



        public TrackCalculate()
        {
            this.Param = new TrackCalculateParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
        }


        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            try
            {
                string index = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ///// 检测传入的参数是否包含有图像 ///
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item == null) continue;
                        switch (item.GetType().Name)
                        {
                            case nameof(String):
                                if (item.ToString().Split('=').Length > 0)
                                    index = item.ToString().Split('=').Last();
                                break;
                            case nameof(Double):
                            case nameof(Int32):
                                index = item.ToString();
                                break;
                        }
                    }
                }
                HXLDCont hXLDCont;
                this.Result.Succss = TrackCalculateMethod.CalculateTrack(this.TrackPoint, this.Param, out this._wcsVector, out hXLDCont);
                ///////////////////////////////////////////
                double[] X = new double[this._wcsVector.Length];
                double[] Y = new double[this._wcsVector.Length];
                double[] Z = new double[this._wcsVector.Length];
                double[] A = new double[this._wcsVector.Length];
                double[] B = new double[this._wcsVector.Length];
                double[] C = new double[this._wcsVector.Length];
                for (int i = 0; i < this._wcsVector.Length; i++)
                {
                    X[i] = this._wcsVector[i].X;
                    Y[i] = this._wcsVector[i].Y;
                    Z[i] = this._wcsVector[i].Z;
                    A[i] = this._wcsVector[i].Angle_x;
                    B[i] = this._wcsVector[i].Angle_y;
                    C[i] = this._wcsVector[i].Angle;
                }
                stopwatch.Stop();
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Track_X", string.Join(",", X));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Track_Y", string.Join(",", Y));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Track_Z", string.Join(",", Z));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Rotate_X", string.Join(",", A));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Rotate_Y", string.Join(",", B));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Rotate_Z", string.Join(",", C));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[6].SetValue(this.name, "时间(ms)", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////
                OnExcuteCompleted(this.name + index, this._wcsVector);
                OnExcuteCompleted(this.name + index + "1", hXLDCont);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" + this._wcsVector.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败：" + this._wcsVector.ToString());
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "点对象":
                default:
                case nameof(this.WcsVector):
                    return this.WcsVector; //
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




    }
}
