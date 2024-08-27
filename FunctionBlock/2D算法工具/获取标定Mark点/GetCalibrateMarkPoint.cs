using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsLibrary;
using HalconDotNet;
using System.Data;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    public class GetCalibrateMarkPoint : BaseFunction, IFunction
    {
        private userPixPoint pixPoint;
        private userPixPoint extractRefSource1Data()
        {
            userPixPoint pixPoint = new userPixPoint();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userPixPoint":
                            pixPoint = ((userPixPoint)object3D);
                            break;
                    }
                }
            }
            return pixPoint;
        }


        public GetCalibrateMarkPoint()
        {
            //resultDataTable.Columns.AddRange(new DataColumn[4] { new DataColumn("元素名称"), new DataColumn("属性名称"), new DataColumn("行坐标"), new DataColumn("列坐标")});
        }

        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.pixPoint = extractRefSource1Data();
                //this.ResultDataTable.Clear();
                //this.ResultDataTable.Rows.Add(this.name, "Mark点",  this.pixPoint.row, this.pixPoint.col);
                if (this.pixPoint.Row == 0 && this.pixPoint.Col == 0)
                    Result.Succss = false;
                else
                    Result.Succss = true;
                ////////////////////
                OnExcuteCompleted(this.name, this.pixPoint);
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.name + "->执行报错", ee);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "执行成功");
            else
                LoggerHelper.Error(this.name + "执行失败");
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
                case "点对象":
                    return this.pixPoint;
                case "输入对象1":
                    return extractRefSource1Data();
                default:
                    if (this.name == propertyName)
                        return this.pixPoint;
                    else return null;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
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
           // throw new NotImplementedException();
        }

        #endregion  

        public enum enShowItems
        {
            输入对象,
            输出对象,
        }


    }
}
