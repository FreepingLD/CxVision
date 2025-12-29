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
    [DefaultProperty(nameof(HomMat2D))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class CoordMap : BaseFunction, IFunction
    {
        private UserHomMat2D _HomMat2D;
        public CoordPoint _CoordPoint1;
        public CoordPoint _CoordPoint2;


        public CoordMapParam MapParam { get; set; }


        [DisplayName("坐标点1")]
        [DescriptionAttribute("输入属性1")]
        public CoordPoint CoordPoint1
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        List<double> list_x = new List<double>();
                        List<double> list_y = new List<double>();
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = (userWcsPoint)item;
                                    list_x.Add(wcsPoint.X);
                                    list_y.Add(wcsPoint.Y);
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = (userWcsCircle)item;
                                    list_x.Add(wcsCircle.X);
                                    list_y.Add(wcsCircle.Y);
                                    break;
                                case nameof(CoordPoint):
                                    this._CoordPoint1 = (CoordPoint)oo[0];
                                    return this._CoordPoint1;
                            }
                        }
                        this._CoordPoint1 = new CoordPoint(list_x.Count);
                        this._CoordPoint1.X = list_x.ToArray();
                        this._CoordPoint1.Y = list_y.ToArray();
                        list_x.Clear();
                        list_y.Clear();
                    }
                    else
                    {
                        this._CoordPoint1 = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _CoordPoint1;
            }
            set
            {
                _CoordPoint1 = value;
            }
        }

        [DisplayName("坐标点2")]
        [DescriptionAttribute("输入属性2")]
        public CoordPoint CoordPoint2
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        List<double> list_x = new List<double>();
                        List<double> list_y = new List<double>();
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = (userWcsPoint)item;
                                    list_x.Add(wcsPoint.X);
                                    list_y.Add(wcsPoint.Y);
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = (userWcsCircle)item;
                                    list_x.Add(wcsCircle.X);
                                    list_y.Add(wcsCircle.Y);
                                    break;
                                case nameof(CoordPoint):
                                    this._CoordPoint2 = (CoordPoint)oo[0];
                                    return this._CoordPoint2;
                            }
                        }
                        this._CoordPoint2 = new CoordPoint(list_x.Count);
                        this._CoordPoint2.X = list_x.ToArray();
                        this._CoordPoint2.Y = list_y.ToArray();
                        list_x.Clear();
                        list_y.Clear();
                    }
                    else
                    {
                        this._CoordPoint2 = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _CoordPoint2;
            }
            set
            {
                _CoordPoint2 = value;
            }
        }


        [DisplayName("映射矩阵")]
        [DescriptionAttribute("输出属性")]
        public UserHomMat2D HomMat2D
        {
            get
            {
                return _HomMat2D;
            }
            set { _HomMat2D = value; }
        }
        public CoordMap()
        {
            this.MapParam = new CoordMapParam();
            InitBindingTable();
        }

        private void InitBindingTable()
        {
            //if (this.StdPointList == null || this.StdPointList.Count == 0)
            //{
            //    this.StdPointList = new BindingList<CoordSysAxisParam>();
            //}
        }

        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.InitBindingTable();
                this.HomMat2D = this.MapParam.GenMapMatrix(this.CoordPoint1, this.CoordPoint2);
                this.Result.Succss = true;
                // 使用发命令的方式来更新视图  
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "坐标映射成功->" + this.HomMat2D.ToString());
                else
                    LoggerHelper.Error(this.name + "坐标映射失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "坐标映射：" + "报错" + ex);
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
