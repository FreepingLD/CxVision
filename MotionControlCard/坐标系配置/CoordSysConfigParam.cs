using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace MotionControlCard
{

    /// <summary>
    /// 配置坐标系的6个轴名称及相应的轴参数，设计思路为：一个坐标系由6个轴组成：X、Y、Z、Theta、U、V
    /// </summary>
    public class CoordSysConfigParam
    {
        public string CardName { get; set; }
        public enCoordSysName CoordSysName { get; set; }
        public enAxisReadWriteState AxisReadWriteState { get; set; }
        public enAxisName AxisName { get; set; }
        public int AxisAddress { get; set; }
        public string AxisLable { get; set; }    // 用相机名称来标识较好，表明哪几个轴属于某个相机
        public enDataTypes DataType { get; set; }
        public int DataLength { get; set; }
        public double PulseEquiv { get; set; }  
        public bool InvertAxisFeedBack { get; set; }
        public bool InvertAxisCommandPos { get; set; }
        public bool InvertJogAxis { get; set; }
        public double TransmissionRatio { get; set; }
        public string AdressPrefix { get; set; }
        public object NONE { get; set; }


        public CoordSysConfigParam()
        {
            this.CardName = "";
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.AxisReadWriteState = enAxisReadWriteState.ReadWrite;
            this.AxisName = enAxisName.NONE;
            this.AxisAddress = 0;
            this.AxisLable = "NONE";
            this.DataType = enDataTypes.Int;
            this.DataLength = 1;
            this.InvertAxisFeedBack = false;
            this.InvertAxisCommandPos = false;
            this.InvertJogAxis = false;
            this.TransmissionRatio = 1;
            this.AdressPrefix = "D";
            this.PulseEquiv = 0.001;
        }
        public CoordSysConfigParam(enAxisName axisName,int AxisAddress)
        {
            this.CardName = "";
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.AxisReadWriteState = enAxisReadWriteState.ReadWrite;
            this.AxisName = axisName;
            this.AxisAddress = AxisAddress;
            this.AxisLable = "NONE";
            this.DataType = enDataTypes.Int;
            this.DataLength = 1;
            this.InvertAxisFeedBack = false;
            this.InvertAxisCommandPos = false;
            this.InvertJogAxis = false;
            this.TransmissionRatio = 1;
            this.AdressPrefix = "D";
            this.PulseEquiv = 0.001;
        }
        private double ReadPlc()
        {
            double value = 0;
            switch (this.AxisReadWriteState)
            {
                case enAxisReadWriteState.Read_Y:
                case enAxisReadWriteState.Read_X:
                case enAxisReadWriteState.Read_Z:
                case enAxisReadWriteState.Read_Theta:
                case enAxisReadWriteState.ReadOnly:
                case enAxisReadWriteState.ReadWrite:
                    value = Convert.ToDouble(MotionCardManage.GetCard(this.CardName).ReadValue(this.DataType, this.AdressPrefix + this.AxisAddress.ToString(), this.DataLength)) * this.PulseEquiv;
                    break;
            }
            return value;
        }
        private void WritePlc(double value)
        {
            switch (this.AxisReadWriteState)
            {
                case enAxisReadWriteState.Write_X:
                case enAxisReadWriteState.Write_Y:
                case enAxisReadWriteState.Write_Z:
                case enAxisReadWriteState.Write_Theta:
                case enAxisReadWriteState.WriteOnly:
                case enAxisReadWriteState.ReadWrite:
                    MotionCardManage.GetCard(this.CardName).WriteValue(this.DataType, this.AdressPrefix + this.AxisAddress.ToString(), value / this.PulseEquiv);
                    break;            
            }
        }




    }
}
