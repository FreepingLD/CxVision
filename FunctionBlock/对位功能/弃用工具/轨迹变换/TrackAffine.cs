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
    [DefaultProperty(nameof(AffinePoint))]
    public class TrackAffine : BaseFunction, IFunction
    {
        private userWcsVector [] _affinePoint;

        private userWcsVector [] _trackPoint;

        public UserHomMat2D _homMat2D;

       [DisplayName("输出点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector [] AffinePoint { get => _affinePoint; set => _affinePoint = value; }


        [DisplayName("输入点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsVector[] TrackPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userWcsVector> listPoint = new List<userWcsVector>();
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = item as userWcsPoint;
                                    listPoint.Add(new userWcsVector(wcsPoint.X, wcsPoint.Y, wcsPoint.Z,0, wcsPoint.CamParams));
                                    break;
                                case nameof(userWcsVector):
                                    listPoint.Add(item as userWcsVector);
                                    break;
                                case "userWcsPoint[]":
                                    userWcsPoint[] wcsVectorA = item as userWcsPoint[];
                                    foreach (var item2 in wcsVectorA)
                                    {
                                        listPoint.Add(new userWcsVector(item2.X, item2.Y, item2.Z,0, item2.CamParams));
                                    }
                                    break;
                                case "userWcsVector[]":
                                    listPoint.AddRange(item as userWcsVector[]);
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

        [DisplayName("变换矩阵")]
        [DescriptionAttribute("输入属性2")]
        public UserHomMat2D HomMat2D
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(UserHomMat2D):
                                    this._homMat2D = item as UserHomMat2D;
                                    break;
                                case nameof(HHomMat2D):
                                    this._homMat2D = new UserHomMat2D(item as HHomMat2D);
                                    break;
                            }
                        }
                    }
                }
                this._homMat2D = new UserHomMat2D();
                return this._homMat2D;
            }
            set
            {
                this._homMat2D = value;
            }
        }

        public enRobotJawEnum Jaw { get; set; }


        public TrackAffine()
        {
            this.Jaw = enRobotJawEnum.NONE;
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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
                if (this._trackPoint != null)
                {
                    RobotJawParam JawParam = RobotJawParaManager.Instance.GetJawParam(this.Jaw);
                    this._homMat2D = new UserHomMat2D(JawParam.GetHomMat2D());
                    this._affinePoint = new userWcsVector[this._trackPoint.Length];
                    for (int i = 0; i < this._trackPoint.Length; i++)
                    {
                        this._affinePoint[i] = this.TrackPoint[i].AffineWcsVector(this._homMat2D.GetHTuple());
                    }
                    ///////////////////////////////////////////
                    double[] X = new double[this._affinePoint.Length];
                    double[] Y = new double[this._affinePoint.Length];
                    double[] Z = new double[this._affinePoint.Length];
                    double[] A = new double[this._affinePoint.Length];
                    double[] B = new double[this._affinePoint.Length];
                    double[] C = new double[this._affinePoint.Length];
                    for (int i = 0; i < this._affinePoint.Length; i++)
                    {
                        X[i] = this._affinePoint[i].X;
                        Y[i] = this._affinePoint[i].Y;
                        Z[i] = this._affinePoint[i].Z;
                        A[i] = this._affinePoint[i].Angle_x;
                        B[i] = this._affinePoint[i].Angle_y;
                        C[i] = this._affinePoint[i].Angle;
                    }
                    stopwatch.Stop();
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Track_X", string.Join(",", X));
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Track_Y", string.Join(",", Y));
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Track_Z", string.Join(",", Z));
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Rotate_X", string.Join(",", A));
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Rotate_Y", string.Join(",", B));
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Rotate_Z", string.Join(",", C));
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[6].SetValue(this.name, "时间(ms)", stopwatch.ElapsedMilliseconds.ToString());
                    this.Result.Succss = true;
                }
                else
                {
                    this.Result.Succss = false;
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Track_X", "");
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Track_Y", "");
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Track_Z", "");
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Rotate_X", "");
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Rotate_Y", "");
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Rotate_Z", "");
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[6].SetValue(this.name, "时间(ms)", stopwatch.ElapsedMilliseconds.ToString());
                }
                ////////////////////////////////////////////
                OnExcuteCompleted(this.name + index, this._affinePoint);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" + this._affinePoint.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败：" + this._affinePoint.ToString());
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
                case nameof(this.AffinePoint):
                    return this.AffinePoint; //
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
