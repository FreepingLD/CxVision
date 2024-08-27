using AlgorithmsLibrary;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Common;
using System.ComponentModel;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(PixCoordSystem))]
    public class CoordSystemRect2 : BaseFunction, IFunction
    {
        private userWcsCoordSystem wcsCoordSystem ;

        private userPixCoordSystem pixCoordSystem;

        private userPixRectangle2 _pixRect2;



        [DisplayName("世界坐标系")]
        [Description("输出属性2")]
        public userWcsCoordSystem WcsCoordSystem
        {
            get
            {
                return wcsCoordSystem;
            }

            set
            {
                wcsCoordSystem = value;
            }
        }

        [DisplayName("像素坐标系")]
        [Description("输出属性")]
        public userPixCoordSystem PixCoordSystem { get => pixCoordSystem; set => pixCoordSystem = value; }

        [DisplayName("矩形对象")]
        [Description("输入属性1")]
        public userPixRectangle2 PixRect2
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if(oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch(item.GetType().Name)
                                {
                                    case nameof(userWcsRectangle2):
                                        this._pixRect2 = ((userWcsRectangle2)item).GetPixRectangle2();
                                        break;
                                    case nameof(userPixRectangle2):
                                        this._pixRect2 = item as userPixRectangle2;
                                        break;
                                }
                            }
                        }
                    }
                    else
                        this._pixRect2 = new userPixRectangle2();
                    return _pixRect2;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._pixRect2 = value;
            }
        }
        public CoordSysParam Param { get; set; }
    

        public CoordSystemRect2()
        {
            this.Name = "坐标系_矩形";
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            this.Param = new CoordSysParam();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Row", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Col", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Rad", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Deg", 0));
            //////////////
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Deg", 0));
        }






        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                Result.Succss = CoordSysMethod.GenRect2CoordPoint(this.PixRect2,this.Param,out this.pixCoordSystem);
                //////////////////////////////////
                this.pixCoordSystem.ReferencePoint.Row = this.Param.RefPoint_Row;
                this.pixCoordSystem.ReferencePoint.Col = this.Param.RefPoint_Col;
                this.pixCoordSystem.ReferencePoint.Rad = this.Param.RefPoint_Rad;
                switch (this.Param.AdjustType)
                {
                    case enAdjustType.X:
                        this.pixCoordSystem.CurrentPoint.Row = this.Param.RefPoint_Row;
                        this.pixCoordSystem.CurrentPoint.Rad = this.Param.RefPoint_Rad;
                        break;
                    case enAdjustType.Y:
                        this.pixCoordSystem.CurrentPoint.Col = this.Param.RefPoint_Col;
                        this.pixCoordSystem.CurrentPoint.Rad = this.Param.RefPoint_Rad;
                        break;
                    case enAdjustType.XY:
                        this.pixCoordSystem.CurrentPoint.Rad = this.Param.RefPoint_Rad;
                        break;
                    case enAdjustType.Theta:
                        this.pixCoordSystem.CurrentPoint.Row = this.Param.RefPoint_Row;
                        this.pixCoordSystem.CurrentPoint.Col = this.Param.RefPoint_Col;
                        break;
                }
                this.wcsCoordSystem = this.pixCoordSystem.GetWcsCoordSystem();
                stopwatch.Stop();          
                //////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Row", this.pixCoordSystem.CurrentPoint.Row);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].Std_Value = this.pixCoordSystem.ReferencePoint.Row;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Col", this.pixCoordSystem.CurrentPoint.Col);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].Std_Value = this.pixCoordSystem.ReferencePoint.Col;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Rad", this.pixCoordSystem.CurrentPoint.Rad);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].Std_Value = this.pixCoordSystem.ReferencePoint.Rad;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X", this.wcsCoordSystem.CurrentPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].Std_Value = this.wcsCoordSystem.ReferencePoint.X;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y", this.wcsCoordSystem.CurrentPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].Std_Value = this.wcsCoordSystem.ReferencePoint.Y;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z", this.wcsCoordSystem.CurrentPoint.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].Std_Value = this.wcsCoordSystem.ReferencePoint.Z;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Deg", this.wcsCoordSystem.CurrentPoint.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].Std_Value = this.wcsCoordSystem.ReferencePoint.Angle;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_x", this.pixCoordSystem.ReferencePoint.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Grab_y", this.pixCoordSystem.ReferencePoint.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "Grab_theta", this.pixCoordSystem.ReferencePoint.Grab_theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name, "Time", stopwatch.ElapsedMilliseconds);
                //////////////////////////////
                OnExcuteCompleted(this.name, this.wcsCoordSystem);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->算子执行报错" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->算子执行成功;" + string.Format("原点坐标：x={0},y={1},z={2},deg={3}", this.wcsCoordSystem.CurrentPoint.X, this.wcsCoordSystem.CurrentPoint.Y, this.wcsCoordSystem.CurrentPoint.Z, this.wcsCoordSystem.CurrentPoint.Angle));
            else
                LoggerHelper.Error(this.name + "->算子执行失败;");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    return this.name;
                case nameof(WcsCoordSystem):
                default:
                    return this.wcsCoordSystem;
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
                //if (FunctionManage.CoordSystemList.Contains(this))
                //    FunctionManage.CoordSystemList.Remove(this);
                LoggerHelper.Info("从坐标系集合中删除坐标系：" + this.name);
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }
        public void Read(string path)
        {
           // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }
    }
}
