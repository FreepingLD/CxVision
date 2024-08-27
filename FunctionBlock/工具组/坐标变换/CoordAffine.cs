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
    [DefaultProperty(nameof(OutCoordPointData))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class CoordAffine : BaseFunction, IFunction
    {
        private CoordPoint[] _OutCoordPointData;
        private CoordPoint[] _CoordPointData;
        private UserHomMat2D _HomMat2D;

        [DisplayName("映射矩阵")]
        [DescriptionAttribute("输入属性2")]
        public UserHomMat2D HomMat2D
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        if (oo != null && oo.Length > 0 && oo[0] is UserHomMat2D)
                        {
                            this._HomMat2D = (UserHomMat2D)oo[0];
                        }
                    }
                    else
                        this._HomMat2D = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _HomMat2D;
            }
            set { _HomMat2D = value; }
        }

        [DisplayName("映射坐标")]
        [DescriptionAttribute("输入属性1")]
        public CoordPoint[] CoordPointData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        List<CoordPoint> CoordPointList = new List<CoordPoint>();
                        foreach (var item in oo)
                        {
                            if (item != null && item is CoordPoint)
                            {
                                CoordPointList.Add((CoordPoint)item);
                            }
                        }
                        this._CoordPointData = CoordPointList.ToArray();
                        CoordPointList.Clear();
                    }
                    else
                        this._CoordPointData = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _CoordPointData;
            }
            set { this._CoordPointData = value; }
        }

        [DisplayName("映射坐标")]
        [DescriptionAttribute("输出属性")]
        public CoordPoint[] OutCoordPointData
        {
            get
            {
                return this._OutCoordPointData;
            }
            set { this._OutCoordPointData = value; }
        }

        public CoordAffineParam MapParam { get; set; }


        public CoordAffine()
        {
            this.MapParam = new CoordAffineParam();
        }



        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = this.MapParam.MapCoord(this.CoordPointData, this.HomMat2D, out this._OutCoordPointData);
                // 使用发命令的方式来更新视图  
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "坐标变换成功，变换矩阵为：" + this.HomMat2D.ToString());
                else
                    LoggerHelper.Error(this.name + "坐标变换失败,变换矩阵为：" + this.HomMat2D.ToString());
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "坐标变换：" + "报错" + ex);
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
                case nameof(this.OutCoordPointData):
                    return this._OutCoordPointData;
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
