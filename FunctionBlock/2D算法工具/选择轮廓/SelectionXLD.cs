using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(SelectContours))]
    public class SelectionXLD : BaseFunction, IFunction
    {
        [NonSerialized]
        private XldDataClass selectContourXLD;
        private string selectMethodXLD = "select_shape_xld";

        private SelectShapeXld selectShape;
        private SelectContoursXld selectContours;

        public string SelectMethodXLD
        {
            get
            {
                return selectMethodXLD;
            }

            set
            {
                selectMethodXLD = value;
            }
        }

        public SelectShapeXld SelectShape { get => selectShape; set => selectShape = value; }
        public SelectContoursXld SelectContours { get => selectContours; set => selectContours = value; }


        public SelectionXLD()
        {
            this.selectShape = new SelectShapeXld();
            this.selectContours = new SelectContoursXld();
        }

        private XldDataClass extractRefSource1Data()
        {
            XldDataClass xld = null;
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
                        case "HXLDCont":
                            xld = new XldDataClass((HXLDCont)object3D);
                            break;
                        case "XldDataClass":
                            xld = (XldDataClass)object3D;
                            break;
                    }
                }
            }
            return xld;
        }

        private bool selectContour(XldDataClass contour, out XldDataClass selectContour)
        {
            bool result = false;
            HXLDCont selectcontour = new HXLDCont();
            selectContour = null;
            if (contour == null || contour.HXldCont == null) return result;
            switch (this.SelectMethodXLD.Trim())
            {
                case "select_contours_xld":
                    selectcontour =   this.selectContours.select_contours_xld(contour.HXldCont); 
                    break;
                case "select_shape_xld":
                    selectcontour = this.selectShape.select_shape_xld(contour.HXldCont);
                    break;
            }
            selectContour = new XldDataClass(selectcontour, contour.CamParams);
            return true;
        }





        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            HalconLibrary ha = new HalconLibrary();
            try
            {
                Result.Succss = selectContour(extractRefSource1Data(), out this.selectContourXLD);
                OnExcuteCompleted(this.name, this.selectContourXLD);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
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
                case "XLD轮廓":
                case "输出对象":
                    return this.selectContourXLD; //
                case "输入对象":
                    return extractRefSource1Data(); //
                default:
                    if (this.name == propertyName)
                        return this.selectContourXLD;
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




        public enum enSelectMethodXLD
        {
            select_contours_xld,
            select_shape_xld,
        }


        public enum enShowItems
        {
            输入对象,
            输出对象,
        }


    }
}
