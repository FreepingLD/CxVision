using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    /// <summary>
    /// 操作员参数类
    /// </summary>
    [Serializable]
   
    public class OperateParam
    {
        [System.Xml.Serialization.XmlIgnore]
        public HImage Image { get; set; }
        public string ProductSize
        {
            set;
            get;
        }

        public string CurrentProductSize
        {
            set;
            get;
        }
        public string ProductID
        {
            set;
            get;
        }
        public string Operate
        {
            set;
            get;
        }
        public string SaveDirect
        {
            set;
            get;
        }
        public BindingList<TrackMoveParam> TrackParam
        {
            set;
            get;
        }


        public enCoordSysName CoordSysName { get; set; }
        public double StdValue { get; set; }
        public double UpTolerance { get; set; }
        public double DownTolerance { get; set; }
        public string AcqSourceName1 { get; set; }
        public string AcqSourceName2 { get; set; }
        public double Center_X { get; set; }
        public double Center_Y { get; set; }
        public double ThickScale { get; set; }


        public OperateParam()
        {
            this.Operate = "张三";
            this.ProductID = "123456789";
            this.ProductSize = "8寸";
            this.CurrentProductSize = "8寸";
            this.SaveDirect = "D:\\测量数据";
            this.TrackParam = new BindingList<TrackMoveParam>();
            this.AcqSourceName1 = "NONE";
            this.AcqSourceName2 = "NONE";
            this.Center_X = 0;
            this.Center_Y = 0;
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.ThickScale = 0;
        }



    }



}
