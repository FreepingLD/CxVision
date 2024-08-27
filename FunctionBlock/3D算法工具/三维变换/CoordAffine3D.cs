using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(AffinePoint))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class CoordAffine3D : BaseFunction, IFunction
    {
        private userWcsPoint[] _affinePoint;
        private userWcsPoint[] _sourcePoint;
        private UserHomMat3D _HomMat3D;

        [DisplayName("映射矩阵")]
        [DescriptionAttribute("输入属性2")]
        public UserHomMat3D HomMat3D
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        if (oo != null && oo.Length > 0 && oo[0] is UserHomMat3D)
                        {
                            this._HomMat3D = (UserHomMat3D)oo[0];
                        }
                    }
                    else
                        this._HomMat3D = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _HomMat3D;
            }
            set { _HomMat3D = value; }
        }

        [DisplayName("源点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] SourcePoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userWcsPoint> list = new List<userWcsPoint>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                list.Add(item as userWcsPoint);
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                list.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                list.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                list.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                list.Add(new userWcsPoint(wcsCoord.CurrentPoint.X, wcsCoord.CurrentPoint.Y, wcsCoord.CurrentPoint.Z, wcsCoord.CurrentPoint.CamParams));
                                break;
                            case nameof(userWcsVector):
                                userWcsVector wcsVector = item as userWcsVector;
                                list.Add(new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._sourcePoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._sourcePoint = new userWcsPoint[0];
                return this._sourcePoint;
            }
            set
            {
                this._sourcePoint = value;
            }
        }

        public CoordAffineParam3D MapParam { get; set; }

        [DisplayName("变换点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint[] AffinePoint { get => _affinePoint; set => _affinePoint = value; }

        public CoordAffine3D()
        {
            this.MapParam = new CoordAffineParam3D();
        }



        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = this.MapParam.MapCoord(this.SourcePoint, this.HomMat3D, out this._affinePoint);
                // 使用发命令的方式来更新视图  
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "坐标变换成功，变换矩阵为：" + this.HomMat3D.ToString());
                else
                    LoggerHelper.Error(this.name + "坐标变换失败,变换矩阵为：" + this.HomMat3D.ToString());
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "坐标变换" + "报错" + ex);
                this.Result.Succss = false;
            }
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
                default:
                    return this.name;
                case nameof(this.AffinePoint):
                    return this._affinePoint;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
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
