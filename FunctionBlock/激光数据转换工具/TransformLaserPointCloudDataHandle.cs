using Sensor;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.IO;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;

namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光扫描采集的数据类
    /// </summary>
    public class TransformLaserPointCloudDataHandle
    {
        private List<double> x = new List<double>();
        private List<double> y = new List<double>();
        private List<double> z = new List<double>();
        private List<double> dist1Value = new List<double>();
        private List<double> dist2Value = new List<double>();
        private List<double> thickValue = new List<double>();
        private CoordSysAxisParam _AxisParam;
        private int dist1Count = 0;
        private int dist2Count = 0;
        private int thickCount = 0;
        /// ///////////
        [NonSerialized]
        private HObjectModel3D dist1DataHandle = new HObjectModel3D(); // 数据句柄
        [NonSerialized]
        private HObjectModel3D dist2DataHandle = new HObjectModel3D(); // 数据句柄
        [NonSerialized]
        private HObjectModel3D thickDataHandle = new HObjectModel3D(); // 数据句柄
        public HObjectModel3D Dist1DataHandle
        {
            get
            {
                return dist1DataHandle;
            }

            set
            {
                dist1DataHandle = value;
            }
        }
        public HObjectModel3D Dist2DataHandle
        {
            get
            {
                return dist2DataHandle;
            }

            set
            {
                dist2DataHandle = value;
            }
        }
        public HObjectModel3D ThickDataHandle
        {
            get
            {
                return thickDataHandle;
            }

            set
            {
                thickDataHandle = value;
            }
        }
        public List<double> X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }
        public List<double> Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
        public List<double> Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }
        public List<double> Dist1Value
        {
            get
            {
                return dist1Value;
            }

            set
            {
                dist1Value = value;
            }
        }
        public List<double> Dist2Value
        {
            get
            {
                return dist2Value;
            }

            set
            {
                dist2Value = value;
            }
        }
        public List<double> ThickValue
        {
            get
            {
                return thickValue;
            }

            set
            {
                thickValue = value;
            }
        }
        public int Dist1Count { get => dist1Count; set => dist1Count = value; }
        public int Dist2Count { get => dist2Count; set => dist2Count = value; }
        public int ThickCount { get => thickCount; set => thickCount = value; }



        /// <summary>
        /// 转换定点采集数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NumLinePer"></param>
        /// <param name="AxisParam"></param>
        public void TransformData(Dictionary<enDataItem, object> list, enUserSensorType sensorType, int NumLinePer, CoordSysAxisParam AxisParam)
        {
            double value;
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            HalconLibrary ha = new HalconLibrary();
            /////////////////////////////////////
            laserDist1 = (double[])list[enDataItem.Dist1];
            laserDist2 = (double[])list[enDataItem.Dist2];
            laserThick = (double[])list[enDataItem.Thick];
            encoder_X = (double[])list[enDataItem.X];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[enDataItem.Y];
            encoder_Z = (double[])list[enDataItem.Z];
            this._AxisParam = AxisParam;
            /// 获取XY坐标　
            switch (sensorType)
            {
                case enUserSensorType.点激光:
                    this.x.Add(AxisParam.X); // 如果没有接入编码器，那么则加入读取的机台坐标值，以机台坐标为准，编码器坐标作为增量坐标来用
                    this.y.Add(AxisParam.Y);
                    ////////////////////////////
                    this.z.Add(AxisParam.Z);
                    ////////////////获取激光测量值
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        ha.GaussSmooth2DPoint(laserDist1, out value);
                        if (encoder_Z != null && encoder_Z.Length > 0) //encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum() / encoder_Z.Length > -1
                        {
                            this.dist1Value.Add(value + encoder_Z.Sum() / encoder_Z.Length);
                        }
                        else
                            this.dist1Value.Add(value + AxisParam.Z);
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    /// 距离2
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        ha.GaussSmooth2DPoint(laserDist2, out value);
                        if (encoder_Z != null && encoder_Z.Length > 0)  //encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum() / encoder_Z.Length > -1
                            this.dist2Value.Add(value + encoder_Z.Sum() / encoder_Z.Length);
                        else
                            this.dist2Value.Add(value + AxisParam.Z);
                    }
                    else
                    {
                        // this.dist2Value.Add(-1024);
                    }
                    //////////////////
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        ha.GaussSmooth2DPoint(laserThick, out value);
                        this.thickValue.Add(value + 0); // 加上Z轴坐标是为了计算量程不够时计算方便, 但厚度测量不需要加Z值 m_coord[2] 
                    }
                    else
                    {
                        //this.thickValue.Add(-1024);
                    }

                    break;
                //////////////////////线激光 
                case enUserSensorType.线激光:
                    // 激光坐标加上机台坐标是为了拼接
                    this.x.AddRange(ArrayAdd(new HTuple(encoder_X).TupleSelectRange(0, NumLinePer - 1).ToDArr(), AxisParam.X));
                    ///////////////////////////////////////////////
                    this.y.AddRange(ArrayAdd(new HTuple(encoder_Y).TupleSelectRange(0, NumLinePer - 1).ToDArr(), AxisParam.Y));
                    //////////获取测量的距离值,z值用平均值              
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length > 0) //encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum() / encoder_Z.Length > -1
                            this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, NumLinePer), encoder_Z));
                        else
                            this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, NumLinePer), AxisParam.Z));
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    //////////距离2////////
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length > 0) //如果没有接编码器，那么就给一个很小的值 encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum()/ encoder_Z.Length > -1
                            this.dist2Value.AddRange(ArrayAdd(GetAverangeValue(laserDist2, NumLinePer), encoder_Z));
                        else
                            this.dist2Value.AddRange(ArrayAdd(GetAverangeValue(laserDist2, NumLinePer), AxisParam.Z));
                    }
                    else
                    {
                    }
                    ///////////获取测量的厚度值             
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        this.thickValue.AddRange(ArrayAdd(GetAverangeValue(laserThick, NumLinePer), 0)); // m_coord[2] 厚度测量不需要加Z值 
                    }
                    else
                    {
                    }

                    break;
                //////////////////////面激光 
                case enUserSensorType.面激光:
                    // 激光坐标加上机台坐标是为了拼接
                    this.x.AddRange(ArrayAdd(encoder_X, AxisParam.X));
                    ///////////////////////////////////////////////
                    this.y.AddRange(ArrayAdd(encoder_Y, AxisParam.Y));
                    //////////获取测量的距离值,z值用平均值              
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length > 0) //encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum() / encoder_Z.Length > -1
                            this.dist1Value.AddRange(ArrayAdd(laserDist1, encoder_Z));
                        else
                            this.dist1Value.AddRange(ArrayAdd(laserDist1, AxisParam.Z));
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    //////////距离2////////
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length > 0) //如果没有接编码器，那么就给一个很小的值 encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum()/ encoder_Z.Length > -1
                            this.dist2Value.AddRange(ArrayAdd(laserDist2, encoder_Z));
                        else
                            this.dist2Value.AddRange(ArrayAdd(laserDist2, AxisParam.Z));
                    }
                    else
                    {
                    }
                    ///////////获取测量的厚度值             
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // m_coord[2] 厚度测量不需要加Z值 
                    }
                    else
                    {
                    }
                    break;
            }
            laserDist1 = null;
            laserDist2 = null;
            laserThick = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            UpdataObjectModel(AxisParam.X, AxisParam.Y, AxisParam.Z);
        }

        public void TransformDataAndCalibrateData(Dictionary<enDataItem, object> list, enUserSensorType sensorType, int dataWidth, int dataHeight, CoordSysAxisParam axisParam, double[] calibrateData)
        {
            double value;
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            HalconLibrary ha = new HalconLibrary();
            /////////////////////////////////////
            laserDist1 = (double[])list[enDataItem.Dist1];
            laserDist2 = (double[])list[enDataItem.Dist2];
            laserThick = (double[])list[enDataItem.Thick];
            encoder_X = (double[])list[enDataItem.X];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[enDataItem.Y];
            encoder_Z = (double[])list[enDataItem.Z];
            this._AxisParam = axisParam;
            /// 获取XY坐标　
            switch (sensorType)
            {
                case enUserSensorType.点激光:
                    this.x.Add(axisParam.X); // 如果没有接入编码器，那么则加入读取的机台坐标值，以机台坐标为准，编码器坐标作为增量坐标来用
                    this.y.Add(axisParam.Y);
                    ////////////////////////////
                    this.z.Add(axisParam.Z);
                    ////////////////获取激光测量值
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        GaussSmooth2DPoint(laserDist1, out value);
                        if (calibrateData != null && calibrateData.Length > 0)
                            this.dist1Value.Add(value * Math.Cos(calibrateData[0]) + axisParam.Z);
                        else
                            this.dist1Value.Add(value + axisParam.Z);
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    /// 距离2
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        GaussSmooth2DPoint(laserDist2, out value);
                        if (calibrateData != null && calibrateData.Length > 0)
                            this.dist2Value.Add(value * Math.Cos(calibrateData[0]) + axisParam.Z);
                        else
                            this.dist2Value.Add(value + axisParam.Z);
                    }
                    else
                    {
                        // this.dist2Value.Add(-1024);
                    }
                    //////////////////
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        GaussSmooth2DPoint(laserThick, out value);
                        if (calibrateData != null && calibrateData.Length > 0)
                            this.thickValue.Add(value * Math.Cos(calibrateData[0]) + 0); // 加上Z轴坐标是为了计算量程不够时计算方便, 但厚度测量不需要加Z值 m_coord[2] 
                        else
                            this.thickValue.Add(value + 0);
                    }
                    else
                    {
                        //this.thickValue.Add(-1024);
                    }
                    break;
                //////////////////////线激光  这个不要，如果确实要做平均，在外面做，不放在内部做
                case enUserSensorType.线激光:
                    this.x.AddRange(ArrayAdd(new HTuple(encoder_X).TupleSelectRange(0, dataWidth - 1).ToDArr(), axisParam.X)); // 激光坐标加上机台坐标是为了拼接                                                                                                                              ///////////////////////////////////////////////
                    this.y.AddRange(ArrayAdd(new HTuple(encoder_Y).TupleSelectRange(0, dataWidth - 1).ToDArr(), axisParam.Y));
                    //////////获取测量的距离值,z值用平均值              
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length != 0) //encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum() / encoder_Z.Length > -1
                            this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, dataWidth), encoder_Z));
                        else
                            this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, dataWidth), axisParam.Z));
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    //////////距离2////////
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length != 0) //如果没有接编码器，那么就给一个很小的值 encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum()/ encoder_Z.Length > -1
                            this.dist2Value.AddRange(ArrayAdd(GetAverangeValue(laserDist2, dataWidth), encoder_Z));
                        else
                            this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, dataWidth), axisParam.Z));
                    }
                    else
                    {
                    }
                    ///////////获取测量的厚度值             
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        this.thickValue.AddRange(ArrayAdd(GetAverangeValue(laserThick, dataWidth), 0)); // m_coord[2] 厚度测量不需要加Z值 
                    }
                    else
                    {
                    }
                    break;
                //////////////////////面激光 
                case enUserSensorType.面激光:
                    this.x.AddRange(calibration_xValue(encoder_X, axisParam.X, dataWidth, calibrateData)); // 激光坐标加上机台坐标是为了拼接                                                                                                       ///////////////////////////////////////////////
                    this.y.AddRange(calibration_yValue(encoder_X, encoder_Y, axisParam.Y, calibrateData));
                    //////////获取测量的距离值,z值用平均值              
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length != 0) //encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum() / encoder_Z.Length > -1
                            this.dist1Value.AddRange(calibration_zValueForFaceSensor(laserDist1, encoder_Z, axisParam.Z, dataWidth, dataHeight, calibrateData));
                        else
                            this.dist1Value.AddRange(calibration_zValueForFaceSensor(laserDist1, axisParam.Z, dataWidth, dataHeight, calibrateData));
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    //////////距离2////////
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        if (encoder_Z != null && encoder_Z.Length != 0) //如果没有接编码器，那么就给一个很小的值 encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum()/ encoder_Z.Length > -1
                            this.dist2Value.AddRange(calibration_zValueForFaceSensor(laserDist2, encoder_Z, axisParam.Z, dataWidth, dataHeight, calibrateData));
                        else
                            this.dist2Value.AddRange(calibration_zValueForFaceSensor(laserDist2, axisParam.Z, dataWidth, dataHeight, calibrateData));
                    }
                    else
                    {
                    }
                    ///////////获取测量的厚度值             
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // m_coord[2] 厚度测量不需要加Z值 
                    }
                    else
                    {
                    }
                    break;
            }
            laserDist1 = null;
            laserDist2 = null;
            laserThick = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            UpdataObjectModel(axisParam.X, axisParam.X, axisParam.Z);
        }

        /// <summary>
        /// 处理定点采集
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sensorType"></param>
        /// <param name="dataWidth"></param>
        /// <param name="dataHeight"></param>
        /// <param name="AxisParam"></param>
        /// <param name="laserCalibrateParam"></param>
        public void TransformDataAndCalibrateData(Dictionary<enDataItem, object> list, enUserSensorType sensorType, int dataWidth, int dataHeight, enScanAxis scanAxis, CoordSysAxisParam AxisParam, userLaserCalibrateParam laserCalibrateParam)
        {
            double value;
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            HalconLibrary ha = new HalconLibrary();
            /////////////////////////////////////
            laserDist1 = (double[])list[enDataItem.Dist1];
            laserDist2 = (double[])list[enDataItem.Dist2];
            laserThick = (double[])list[enDataItem.Thick];
            encoder_X = (double[])list[enDataItem.X];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[enDataItem.Y];
            encoder_Z = (double[])list[enDataItem.Z];
            this._AxisParam = AxisParam;
            /// 获取XY坐标　
            switch (sensorType)
            {
                case enUserSensorType.点激光:
                    laserCalibrateParam.LaserType = "点激光";
                    this.x.Add(AxisParam.X); // 如果没有接入编码器，那么则加入读取的机台坐标值，以机台坐标为准，编码器坐标作为增量坐标来用
                    this.y.Add(AxisParam.Y);
                    ////////////////////////////
                    this.z.Add(AxisParam.Z);
                    ////////////////获取激光测量值
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        GaussSmooth2DPoint(laserDist1, out value);
                        this.dist1Value.Add(value + AxisParam.Z);
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    /// 距离2
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        GaussSmooth2DPoint(laserDist2, out value);
                        this.dist2Value.Add(value + AxisParam.Z);
                    }
                    else
                    {
                        // this.dist2Value.Add(-1024);
                    }
                    //////////////////
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        GaussSmooth2DPoint(laserThick, out value);
                        this.thickValue.Add(value + 0);
                    }
                    else
                    {
                        //this.thickValue.Add(-1024);
                    }
                    break;
                //////////////////////线激光  这个不要，如果确实要做平均，在外面做，不放在内部做
                case enUserSensorType.线激光:
                    laserCalibrateParam.LaserType = "线激光";
                    switch (scanAxis)
                    {
                        default:
                        case enScanAxis.Y轴:
                            this.x.AddRange(ArrayAdd(new HTuple(encoder_X).TupleSelectRange(0, dataWidth - 1).ToDArr(), 0)); // 激光坐标加上机台坐标是为了拼接     m_coord[0]                                                                                                                          ///////////////////////////////////////////////
                            this.y.AddRange(GenConstSequence(AxisParam.Y, dataWidth));
                            break;
                        case enScanAxis.X轴:
                            this.x.AddRange(GenConstSequence(AxisParam.X, dataWidth));
                            this.y.AddRange(ArrayAdd(new HTuple(encoder_X).TupleSelectRange(0, dataWidth - 1).ToDArr(), 0)); // 激光坐标加上机台坐标是为了拼接    m_coord[1]                                                                                                                           ///////////////////////////////////////////////
                            break;
                    }
                    //////////获取测量的距离值,z值用平均值              
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, dataWidth), 0)); // m_coord[2]
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    //////////距离2////////
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, dataWidth), 0)); // m_coord[2]
                    }
                    else
                    {
                    }
                    ///////////获取测量的厚度值             
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        this.thickValue.AddRange(ArrayAdd(GetAverangeValue(laserThick, dataWidth), 0)); // m_coord[2] 厚度测量不需要加Z值 
                    }
                    else
                    {
                    }
                    break;
                //////////////////////面激光 
                case enUserSensorType.面激光:
                    laserCalibrateParam.LaserType = "面激光";
                    this.x.AddRange(encoder_X); // 激光坐标加上机台坐标是为了拼接      m_coord[0]        ArrayAdd(encoder_X, 0, dataWidth)                                                                                           ///////////////////////////////////////////////
                    this.y.AddRange(encoder_Y); // m_coord[1] ArrayAdd(encoder_Y, 0, dataHeight)
                    //////////获取测量的距离值,z值用平均值              
                    if (laserDist1 != null && laserDist1.Length > 0)
                    {
                        this.dist1Value.AddRange(ArrayAdd(laserDist1, 0)); // m_coord[2]
                    }
                    else
                    {
                        MessageBox.Show("激光点采集失败");
                    }
                    //////////距离2////////
                    if (laserDist2 != null && laserDist2.Length > 0)
                    {
                        this.dist2Value.AddRange(laserDist2); // m_coord[2] ArrayAdd(laserDist2, 0)
                    }
                    else
                    {
                    }
                    ///////////获取测量的厚度值             
                    if (laserThick != null && laserThick.Length > 0)
                    {
                        this.thickValue.AddRange(laserThick); // m_coord[2] 厚度测量不需要加Z值  ArrayAdd(laserThick, 0)
                    }
                    else
                    {
                    }
                    break;
            }
            laserDist1 = null;
            laserDist2 = null;
            laserThick = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            UpdataObjectModel(AxisParam.X, AxisParam.Y, AxisParam.Z, scanAxis, 0, laserCalibrateParam);
        }

        /// <summary>
        /// 转换扫描数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NumLinePer"></param>
        /// <param name="step_y"></param>
        /// <param name="m_coord1"></param>
        /// <param name="m_coord2"></param>
        public void TransformData(Dictionary<enDataItem, object> list, enUserSensorType sensorType, int NumLinePer, enScanAxis scanAxis,  CoordSysAxisParam AxisParam1, CoordSysAxisParam AxisParam2)
        {
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            /////////////////////////////////////
            laserDist1 = (double[])list[enDataItem.Dist1];
            laserDist2 = (double[])list[enDataItem.Dist2];
            laserThick = (double[])list[enDataItem.Thick];
            encoder_X = (double[])list[enDataItem.X];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[enDataItem.Y];
            encoder_Z = (double[])list[enDataItem.Z];
            this._AxisParam = AxisParam1;
            /////////////////////////////////////////////
            this.ClearData();
            switch (sensorType)
            {
                case enUserSensorType.点激光:
                    if (encoder_X != null && encoder_X.Length != 0 && Math.Abs(encoder_X.Sum()) > 0) // 如果没有接入编码器，值应该为了encoder_X != null && encoder_X.Length > 0 &&  encoder_X.Sum() / encoder_X.Length > -1
                    {
                        this.x.AddRange(ArrayAdd(encoder_X, AxisParam1.X)); // 编码器的坐标为相对坐标
                    }
                    else
                    {
                        this.x.AddRange(GenSequence(AxisParam1.X, AxisParam2.X, laserDist1.Length));
                        //this.x.AddRange(GenSequence(m_coord1[0], m_coord1[1], m_coord2[0], m_coord2[1], laserDist1.Length, NumLinePer));
                        // 点激光不需要加当前机台坐标，因为XY都是当前机台坐标                
                    }
                    /////////////////////
                    if (encoder_Y != null && encoder_Y.Length != 0 && Math.Abs(encoder_X.Sum()) > 0) //encoder_Y != null && encoder_Y.Length > 0 && encoder_Y.Sum() / encoder_Y.Length > -1
                    {
                        this.y.AddRange(ArrayAdd(encoder_Y, AxisParam1.Y));
                    }
                    else
                    {
                        this.y.AddRange(GenSequence(AxisParam1.Y, AxisParam2.Y, laserDist1.Length));
                        //this.y.AddRange(GenConstSequence(m_coord1[1], laserDist1.Length));
                    }
                    /////////////////////
                    if (encoder_Z != null && encoder_Z.Length != 0) //encoder_Z != null && encoder_Z.Length > 0 && encoder_Z.Sum() / encoder_Z.Length > -1
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, encoder_Z, AxisParam1.Z, NumLinePer)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, encoder_Z, AxisParam1.Z, NumLinePer));// 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0));
                    }
                    else
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, AxisParam1.Z));
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, AxisParam1.Z));// 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // 如果需要实时读取Z轴坐标，则需要使用他
                    }
                    break;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // 表示传感器为线激光
                case enUserSensorType.线激光:
                    switch (scanAxis)
                    {
                        case enScanAxis.Y轴:
                            if (encoder_X != null && encoder_X.Length != 0)
                                this.x.AddRange(ArrayAdd(encoder_X, AxisParam1.X)); // 不会有人去把激光线摆成斜的
                            if (encoder_Y != null && encoder_Y.Length != 0)
                                this.y.AddRange(ArrayAdd(encoder_Y, AxisParam1.Y, NumLinePer)); //Y坐标要加一个值,encoder_Y:采用相对坐标ArrayAdd(encoder_Y, m_coord1[1])
                            else
                                this.y.AddRange(GenYSequence(AxisParam1.Y, (AxisParam2.Y - AxisParam1.Y) / (laserDist1.Length / NumLinePer), laserDist1.Length, NumLinePer));
                            break;
                        case enScanAxis.X轴:
                            if (encoder_Y != null && encoder_Y.Length != 0) // 扫描方向始终为Y轴
                                this.x.AddRange(ArrayAdd(encoder_Y, AxisParam1.X, NumLinePer));
                            else
                                this.x.AddRange(GenYSequence(AxisParam1.X, (AxisParam2.X - AxisParam1.X) / (laserDist1.Length / NumLinePer), laserDist1.Length, NumLinePer)); // 不会有人去把激光线摆成斜的
                            //////////////////
                            if (encoder_X != null && encoder_X.Length != 0)
                                this.y.AddRange(ArrayAdd(encoder_X, AxisParam1.Y)); //Y坐标要加一个值,encoder_Y:采用相对坐标ArrayAdd(encoder_Y, m_coord1[1])
                            break;
                    }
                    //////////////////
                    if (encoder_Z != null && encoder_Z.Length != 0)
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, encoder_Z, AxisParam1.Z, NumLinePer)); // 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, encoder_Z, AxisParam1.Z, NumLinePer)); // 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, encoder_Z, AxisParam1.Z, NumLinePer));
                    }
                    else
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, AxisParam1.Z));
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, AxisParam1.Z)); // 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, AxisParam1.Z)); // 如果需要实时读取Z轴坐标，则需要使用他
                    }
                    break;
            }
            // 最后清空数据
            laserDist1 = null;
            laserDist2 = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            laserThick = null;
            UpdataObjectModel(AxisParam1.X, AxisParam1.Y, AxisParam1.Z);
        }

        /// <summary>
        /// 转换扫描数据,并校准激光安装带来的误差，主要校准绕Y轴的倾斜
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NumLinePer"></param>
        /// <param name="step_y"></param>
        /// <param name="m_coord1"></param>
        /// <param name="m_coord2"></param>
        public void TransformDataAndCalibrateData(Dictionary<enDataItem, object> list, enUserSensorType sensorType, int NumLinePer, enScanAxis scanAxis, CoordSysAxisParam AxisParam1, CoordSysAxisParam AxisParam2, double[] calibrateData)
        {
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            /////////////////////////////////////
            laserDist1 = (double[])list[enDataItem.Dist1];
            laserDist2 = (double[])list[enDataItem.Dist2];
            laserThick = (double[])list[enDataItem.Thick];
            encoder_X = (double[])list[enDataItem.X];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[enDataItem.Y];
            encoder_Z = (double[])list[enDataItem.Z];
            this._AxisParam = AxisParam1;
            /////////////////////////////////////////////
            // 表示传感器为点激光, 点激光的校准也放到这里面来做
            switch (sensorType)
            {
                case enUserSensorType.点激光:
                    ////////////
                    if (encoder_X != null && encoder_X.Length > 0 && Math.Abs(encoder_X.Sum()) > 0) // 如果没有接入编码器，值应该为了&& Math.Abs(encoder_X.Max() - encoder_X.Min()) > 0.000
                    {
                        this.x.AddRange(calibration_xValue(encoder_X, laserDist1, AxisParam1.X, calibrateData)); // 编码器的坐标为相对坐标 ArrayAdd(encoder_X, m_coord1[0])
                    }
                    else
                    {
                        this.x.AddRange(GenSequence(AxisParam1.X, AxisParam1.Y, AxisParam2.X, AxisParam2.Y, laserDist1.Length, NumLinePer));
                        // 点激光不需要加当前机台坐标，因为XY都是当前机台坐标                
                    }
                    /////////////////////
                    if (encoder_Y != null && encoder_Y.Length > 0 && Math.Abs(encoder_Y.Sum()) > 0) //&& Math.Abs(encoder_Y.Max() - encoder_Y.Min()) > 0.001
                    {
                        this.y.AddRange(ArrayAdd(encoder_Y, AxisParam1.Y));
                    }
                    else
                    {
                        this.y.AddRange(GenConstSequence(AxisParam1.Y, laserDist1.Length));
                    }
                    /////////////////////
                    if (encoder_Z != null && encoder_Z.Length > 0 && Math.Abs(encoder_Z.Sum()) > 0)
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, encoder_Z, AxisParam1.Z, calibrateData)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, encoder_Z, AxisParam1.Z, calibrateData));// 如果需要实时读取Z轴坐标，则需要使用他ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // ArrayAdd(laserThick, encoder_Z, m_coord1[2], NumLinePer)
                    }
                    else
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, AxisParam1.Z, calibrateData)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, AxisParam1.Z, calibrateData));// 如果需要实时读取Z轴坐标，则需要使用他ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // ArrayAdd(laserThick, encoder_Z, m_coord1[2], NumLinePer)
                    }
                    break;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // 表示传感器为线激光
                case enUserSensorType.线激光:
                    switch (scanAxis)
                    {
                        case enScanAxis.Y轴:
                            if (encoder_X != null && encoder_X.Length != 0)
                                this.x.AddRange(calibration_xValue(encoder_X, AxisParam1.X, NumLinePer, calibrateData)); // 不会有人去把激光线摆成斜的
                            if (encoder_Y != null && encoder_Y.Length != 0)
                                this.y.AddRange(calibration_yValue(encoder_X, encoder_Y, AxisParam1.Y, calibrateData)); //Y坐标要加一个值,encoder_Y:采用相对坐标ArrayAdd(encoder_Y, m_coord1[1])
                            else
                                this.y.AddRange(GenYSequence(AxisParam1.Y, (AxisParam2.Y - AxisParam1.Y) / (laserDist1.Length / NumLinePer), laserDist1.Length, NumLinePer));
                            break;
                        case enScanAxis.X轴:
                            if (encoder_Y != null && encoder_Y.Length != 0)
                                this.x.AddRange(calibration_yValue(encoder_Y, encoder_X, AxisParam1.X, calibrateData)); // 不会有人去把激光线摆成斜的
                            else
                                this.x.AddRange(GenYSequence(AxisParam1.X, (AxisParam2.X - AxisParam1.X) / (laserDist1.Length / NumLinePer), laserDist1.Length, NumLinePer)); // 不会有人去把激光线摆成斜的
                            ////////////
                            if (encoder_X != null && encoder_X.Length != 0)
                                this.y.AddRange(calibration_xValue(encoder_X, AxisParam1.Y, NumLinePer, calibrateData)); //Y坐标要加一个值,encoder_Y:采用相对坐标ArrayAdd(encoder_Y, m_coord1[1])
                            break;
                    }
                    //////////////////
                    if (encoder_Z != null && encoder_Z.Length != 0)
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, encoder_Z, AxisParam1.Z, NumLinePer, calibrateData)); // 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, encoder_Z, AxisParam1.Z, NumLinePer, calibrateData)); // 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0));// 厚度值不需要加
                    }
                    else
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, AxisParam1.Z, NumLinePer, calibrateData));
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, AxisParam1.Z, NumLinePer, calibrateData)); // 如果需要实时读取Z轴坐标，则需要使用他
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // 如果需要实时读取Z轴坐标，则需要使用他
                    }
                    break;
            }
            // 最后清空数据
            laserDist1 = null;
            laserDist2 = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            laserThick = null;
            UpdataObjectModel(AxisParam1.X, AxisParam1.Y, AxisParam1.Z);
        }

        /// <summary>
        /// 处理扫描数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sensorType"></param>
        /// <param name="NumLinePer"></param>
        /// <param name="scanAxis"></param>
        /// <param name="m_coord1"></param>
        /// <param name="m_coord2"></param>
        /// <param name="laserCalibrateParam"></param>
        public void TransformDataAndCalibrateData(Dictionary<enDataItem, object> list, enUserSensorType sensorType, int NumLinePer, enScanAxis scanAxis, CoordSysAxisParam AxisParam1, CoordSysAxisParam AxisParam2, userLaserCalibrateParam laserCalibrateParam)
        {
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            /////////////////////////////////////
            laserDist1 = (double[])list[enDataItem.Dist1];
            laserDist2 = (double[])list[enDataItem.Dist2];
            laserThick = (double[])list[enDataItem.Thick];
            encoder_X = (double[])list[enDataItem.X];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[enDataItem.Y];
            encoder_Z = (double[])list[enDataItem.Z];
            this._AxisParam = AxisParam1;
            /////////////////////////////////////////////
            // 表示传感器为点激光, 点激光的校准也放到这里面来做
            switch (sensorType)
            {
                case enUserSensorType.点激光:
                    ////////////
                    laserCalibrateParam.LaserType = "点激光";
                    if (encoder_X != null && encoder_X.Length > 0 && Math.Abs(encoder_X.Sum()) > 0) // 如果没有接入编码器，值应该为了&& Math.Abs(encoder_X.Max() - encoder_X.Min()) > 0.000
                    {
                        this.x.AddRange(ArrayAdd(encoder_X, AxisParam1.X)); // 编码器的坐标为相对坐标 ArrayAdd(encoder_X, m_coord1[0])
                    }
                    else
                    {
                        this.x.AddRange(GenSequence(AxisParam1.X, AxisParam2.X, laserDist1.Length, NumLinePer)); // 只适用于垂直于X轴或Y轴的情况
                        // 点激光不需要加当前机台坐标，因为XY都是当前机台坐标                
                    }
                    /////////////////////
                    if (encoder_Y != null && encoder_Y.Length > 0 && Math.Abs(encoder_Y.Sum()) > 0) //&& Math.Abs(encoder_Y.Max() - encoder_Y.Min()) > 0.001
                    {
                        this.y.AddRange(ArrayAdd(encoder_Y, AxisParam1.Y));
                    }
                    else
                    {
                        this.y.AddRange(GenSequence(AxisParam1.Y, AxisParam2.Y, laserDist1.Length, NumLinePer)); // 只适用于垂直于X轴或Y轴的情况
                    }
                    /////////////////////
                    if (encoder_Z != null && encoder_Z.Length > 0 && Math.Abs(encoder_Z.Sum()) > 0)
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, encoder_Z, AxisParam1.Z, NumLinePer)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, encoder_Z, AxisParam1.Z, NumLinePer));// 如果需要实时读取Z轴坐标，则需要使用他ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // ArrayAdd(laserThick, encoder_Z, m_coord1[2], NumLinePer)
                    }
                    else
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, AxisParam1.Z)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, AxisParam1.Z));// 如果需要实时读取Z轴坐标，则需要使用他ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer)
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // ArrayAdd(laserThick, encoder_Z, m_coord1[2], NumLinePer)
                    }
                    break;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // 表示传感器为线激光
                case enUserSensorType.线激光:
                    laserCalibrateParam.LaserType = "线激光";
                    switch (scanAxis)
                    {
                        case enScanAxis.Y轴:
                            if (encoder_X != null && encoder_X.Length != 0)
                                this.x.AddRange(ArrayAdd(encoder_X, 0)); // 不会有人去把激光线摆成斜的 m_coord1[0]
                            if (encoder_Y != null && encoder_Y.Length != 0)
                                this.y.AddRange(ArrayAdd(encoder_Y, 0)); //Y坐标要加一个值,encoder_Y:采用相对坐标ArrayAdd(encoder_Y, m_coord1[1])  m_coord1[1]
                            else
                                this.y.AddRange(GenYSequence(AxisParam1.Y, (AxisParam2.Y - AxisParam1.Y) / (laserDist1.Length / NumLinePer), laserDist1.Length, NumLinePer));
                            break;
                        case enScanAxis.X轴:
                            if (encoder_Y != null && encoder_Y.Length != 0) // 扫描方向始终是Y方向
                                this.x.AddRange(ArrayAdd(encoder_Y, AxisParam1.X));
                            else
                                this.x.AddRange(GenYSequence(AxisParam1.X, (AxisParam2.X - AxisParam1.X) / (laserDist1.Length / NumLinePer), laserDist1.Length, NumLinePer));
                            //////////////////
                            if (encoder_X != null && encoder_X.Length != 0)
                                this.y.AddRange(ArrayAdd(encoder_X, AxisParam1.Y)); //Y坐标要加一个值,encoder_Y:采用相对坐标ArrayAdd(encoder_Y, m_coord1[1])
                            break;
                    }
                    //////////////////
                    if (encoder_Z != null && encoder_Z.Length != 0)
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, encoder_Z, 0, NumLinePer)); // 如果需要实时读取Z轴坐标，则需要使用他 m_coord1[2]
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, encoder_Z, 0, NumLinePer)); // 如果需要实时读取Z轴坐标，则需要使用他 m_coord1[2]
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0));// 厚度值不需要加
                    }
                    else
                    {
                        if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, 0)); //m_coord1[2]
                        if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, 0)); // 如果需要实时读取Z轴坐标，则需要使用他  m_coord1[2]
                        if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // 如果需要实时读取Z轴坐标，则需要使用他
                    }
                    break;
            }
            // 最后清空数据
            laserDist1 = null;
            laserDist2 = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            laserThick = null;
            UpdataObjectModel(AxisParam1.X, AxisParam1.Y, AxisParam1.Z, scanAxis, HMisc.LineOrientation(AxisParam1.Y * -1, AxisParam1.X, AxisParam2.Y * -1, AxisParam2.X), laserCalibrateParam);
        }

        private double[] GenConstSequence(double Value, int arrayLength)
        {
            double[] data = new double[arrayLength];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Value;
            }
            return data;
        }
        private double[] GenSequence(double data1, double data2, int length, int countPerLine)
        {
            double[] data = new double[length];
            int rowCount = length / countPerLine;
            int result;
            double value = data1;
            double step = (data1 - data2) / rowCount;
            for (int i = 0; i < length; i++)
            {
                Math.DivRem(i, countPerLine, out result);
                if (i != 0 && result == 0)
                    value += step;
                data[i] = value;
            }
            return data;
        }

        private double[] GenSequence(double X1, double Y1, double X2, double Y2, int length, int countPerLine)
        {
            double[] data = new double[length];
            int rowCount = length / countPerLine;
            int result;
            double step = 0;
            double value = Y1;
            double dist = (double)Math.Sqrt((X1 - X2) * (X1 - X2) + (Y1 - Y2) * (Y1 - Y2));
            if (Y1 > Y2)
                step = dist / rowCount * -1;
            else
                step = dist / rowCount;
            for (int i = 0; i < length; i++)
            {
                Math.DivRem(i, countPerLine, out result);
                if (i != 0 && result == 0)
                    value += step;
                data[i] = value;
            }
            return data;
        }
        private double[] GenSequence(double value1, double value2, int length)
        {
            double[] data = new double[length];
            double dist = value2 - value1;
            double step = dist / length;
            for (int i = 0; i < length; i++)
            {
                data[i] = value1 + i * step;
            }
            return data;
        }
        private double[] GenYSequence(double startValue, double yStep, int length, int countPerLine)
        {
            double[] data = new double[length];
            int rowCount = length / countPerLine;
            int result;
            double value = startValue;
            for (int i = 0; i < length; i++)
            {
                Math.DivRem(i, countPerLine, out result);
                if (i != 0 && result == 0)
                    value += yStep;
                data[i] = value;
            }
            return data;
        }
        private double[] ArrayAdd(double[] array, double value)
        {
            if (array != null && array.Length > 0)
            {
                double[] temArray = new double[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    temArray[i] = array[i] + value;
                }
                return temArray;
            }
            else
                return null;
        }
        private double[] ArrayAdd(double[] array, double[] value)
        {
            if (array != null && array.Length > 0)
            {
                double[] temArray = new double[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    temArray[i] = array[i] + value[i];
                }
                return temArray;
            }
            else
                return null;
        }
        private double SumArray(double[] array)
        {
            double sum = 0.0f;
            if (array != null && array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    sum += array[i];
                }
            }
            return sum;
        }
        private double[] ArrayAdd(double[] array, double[] encoder, int countPerLine)
        {
            int result;
            int k = 0;
            //////////////////////////////////////////////////////////////////////////
            if (array != null && array.Length > 0)
            {
                double[] temArray = new double[array.Length];
                if (array.Length > encoder.Length)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        Math.DivRem(i, countPerLine, out result);
                        if (i != 0 && result == 0)
                            k++;
                        temArray[i] = array[i] + encoder[k];
                    }
                }
                else
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        temArray[i] = encoder[i] + array[i];
                    }
                }
                return temArray;
            }
            else
                return null;
        }
        private double[] ArrayAdd(double[] encoder, double offsetvalue, int countPerLine)
        {
            if (encoder != null && encoder.Length > 0)
            {
                double[] temArray = new double[encoder.Length * countPerLine];
                for (int i = 0; i < encoder.Length; i++)
                {
                    for (int j = 0; j < countPerLine; j++)
                    {
                        temArray[i * countPerLine + j] = encoder[i] + offsetvalue;
                    }
                }
                return temArray;
            }
            else
                return null;
        }
        private double[] ArrayAdd(double[] array, double[] encoder, double offsetvalue, int countPerLine)
        {
            int result;
            int k = 0;
            //////////////////////////////////////////////////////////////////////////
            if (array != null && array.Length > 0)
            {
                double[] temArray = new double[array.Length];
                if (array.Length > encoder.Length)  // 编码器的坐标可以是每条激光线只存储一个值，也可以每条激光线存储与激光线点数相同的值
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        Math.DivRem(i, countPerLine, out result);
                        if (i != 0 && result == 0)
                            k++;
                        temArray[i] = array[i] + encoder[k] + offsetvalue;
                    }
                }
                else
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        temArray[i] = encoder[i] + array[i] + offsetvalue;
                    }
                }
                return temArray;
            }
            else
                return null;
        }
        public void FilterByDist(double[] x, double[] y, double[] z, double dist, int minCount, out double[] filter_x, out double[] filter_y, out double[] filter_z)
        {
            filter_x = new double[0];
            filter_y = new double[0];
            filter_z = new double[0];
            List<double> allValue_x = new List<double>();
            List<double> allValue_y = new List<double>();
            List<double> allValue_z = new List<double>();
            ///////////////////////////////////////////////
            List<double> singleValue_x = new List<double>();
            List<double> singleValue_y = new List<double>();
            List<double> singleValue_z = new List<double>();
            if (z == null || x == null || y == null) return;

            if (z.Length > 0 && x.Length > 0 && y.Length > 0)
            {
                singleValue_x.Add(x[0]);
                singleValue_y.Add(y[0]);
                singleValue_z.Add(z[0]);
                for (int i = 0; i < z.Length - 1; i++)
                {
                    if (Math.Abs(z[i + 1] - z[i]) <= dist)
                    {
                        singleValue_x.Add(x[i + 1]);
                        singleValue_y.Add(y[i + 1]);
                        singleValue_z.Add(z[i + 1]);
                    }
                    else
                    {
                        if (singleValue_z.Count >= minCount)
                        {
                            allValue_x.AddRange(singleValue_x);
                            allValue_y.AddRange(singleValue_y);
                            allValue_z.AddRange(singleValue_z);
                        }
                        singleValue_x.Clear();
                        singleValue_y.Clear();
                        singleValue_z.Clear();
                        singleValue_x.Add(x[i + 1]);
                        singleValue_y.Add(y[i + 1]);
                        singleValue_z.Add(z[i + 1]);
                    }
                }
                if (singleValue_z.Count > 0)
                {
                    allValue_x.AddRange(singleValue_x);
                    allValue_y.AddRange(singleValue_y);
                    allValue_z.AddRange(singleValue_z);
                }
                singleValue_x.Clear();
                singleValue_y.Clear();
                singleValue_z.Clear();
            }
            filter_x = allValue_x.ToArray();
            filter_y = allValue_y.ToArray();
            filter_z = allValue_z.ToArray();
        }
        public void FilterByDist(double[] z, double dist, int minCount, out double[] filter_z)
        {
            filter_z = new double[0];
            List<double> allValue_z = new List<double>();
            List<double> singleValue_z = new List<double>();
            if (z == null || x == null || y == null) return;
            //////////////////////////////////
            if (z.Length > 0)
            {
                singleValue_z.Add(z[0]);
                for (int i = 0; i < z.Length - 1; i++)
                {
                    if (Math.Abs(z[i + 1] - z[i]) <= dist)
                    {
                        singleValue_z.Add(z[i + 1]);
                    }
                    else
                    {
                        if (singleValue_z.Count >= minCount)
                        {
                            allValue_z.AddRange(singleValue_z);
                        }
                        singleValue_z.Clear();
                        singleValue_z.Add(z[i + 1]);
                    }
                }
                if (singleValue_z.Count > 1) // 第一个添加的点不能用来判断
                {
                    allValue_z.AddRange(singleValue_z);
                }
                singleValue_z.Clear();
            }
            if (allValue_z.Count == 0)
                filter_z = z;
            else
                filter_z = allValue_z.ToArray();
        }
        public void GaussSmooth2DPoint(double[] value, out double smoothValue)
        {
            smoothValue = 0;
            List<double> filterValue = new List<double>();
            HTuple function, smoothFunction, smooth_x, smooth_y;
            double[] filter_z;
            if (value == null) return;
            // 去悼0值点
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] >= 0)
                    filterValue.Add(value[i]);
            }
            if (filterValue.Count < 10)
            {
                if (filterValue.Count == 0)
                    smoothValue = 0;
                else
                    smoothValue = (filterValue.Sum() / filterValue.Count);
            }
            else
            {
                FilterByDist(filterValue.ToArray(), 0.005, (int)(0.2 * filterValue.Count), out filter_z);  //滤悼噪点再平滑
                HTuple sigma = Math.Abs((filter_z.Length - 2) / 8.0);
                if (sigma.D < 0.01)
                    sigma = 0.01;
                HOperatorSet.CreateFunct1dArray(filter_z, out function);
                HOperatorSet.SmoothFunct1dGauss(function, sigma, out smoothFunction);
                HOperatorSet.Funct1dToPairs(smoothFunction, out smooth_x, out smooth_y);
                smoothValue = (smooth_y.TupleSum() / smooth_y.Length).D;
            }
        }
        public void ClearData()
        {
            this.x.Clear();
            this.y.Clear();
            this.z.Clear();
            this.dist1Value.Clear();
            this.dist2Value.Clear();
            this.thickValue.Clear();
        }
        public void ClearHandle()
        {
            //if (this.dist1DataHandle != null && this.dist1DataHandle.Count > 0)
            //{
            //    foreach (var item in this.dist1DataHandle)
            //        item.Dispose();
            //}
            //if (this.dist1DataHandle != null && this.dist1DataHandle.Count > 0)
            //{
            //    foreach (var item in this.dist2DataHandle)
            //        item.Dispose();
            //}
            //if (this.dist1DataHandle != null && this.dist1DataHandle.Count > 0)
            //{
            //    foreach (var item in this.thickDataHandle)
            //        item.Dispose();
            //}
            //if (this.dist1DataHandle != null)
            //    this.dist1DataHandle.Clear();
            //if (this.dist2DataHandle != null)
            //    this.dist2DataHandle.Clear();
            //if (this.thickDataHandle != null)
            //    this.thickDataHandle.Clear();
            if (this.dist1DataHandle != null) this.dist1DataHandle.Dispose();
            if (this.dist2DataHandle != null) this.dist2DataHandle.Dispose();
            if (this.thickDataHandle != null) this.thickDataHandle.Dispose();
            // 使用类，这里就不需要再清空了
        }

        /// <summary>
        /// 是否需要将其作为一个公共方法来处理？
        /// </summary>
        /// <param name="machine_x"></param>
        /// <param name="machine_y"></param>
        /// <param name="machine_z"></param>
        private void UpdataObjectModel(double machine_x, double machine_y, double machine_z,int width = 2000,int height = 2000)
        {
            //int num = 0;
            HTuple value = 0;
            double maxValue = 0;
            HObjectModel3D temPObject = new HObjectModel3D();
            HalconLibrary ha = new HalconLibrary();  // 在这里面加一个滤波，将0值去悼
            //更新数据
            if (this.dist1Value.Count > 0)
            {
                temPObject = new HObjectModel3D(new HTuple(this.x.ToArray()), new HTuple(this.y.ToArray()), new HTuple(this.dist1Value.ToArray()));
                temPObject.SetObjectModel3dAttribMod(new HTuple("&ref_point"), "object", new HTuple(this._AxisParam));
                this.dist1Count = temPObject.GetObjectModel3dParams("num_points").I;
                if (this.dist1Count > 0)
                    value = temPObject.GetObjectModel3dParams("point_coord_z");
                maxValue = 999 < value.TupleMax().D ? 999 : value.TupleMax().D;
                if (this.dist1Count > 1) // 表示是扫描，否则为单点采集
                {
                    if (this.dist1DataHandle.IsInitialized())
                        this.dist1DataHandle.Dispose();
                    this.dist1DataHandle = (temPObject.SelectPointsObjectModel3d("point_coord_z", 0, maxValue + 0.01));
                }                  
                else
                {
                    if (this.dist1DataHandle.IsInitialized())
                        this.dist1DataHandle.Dispose();
                    this.dist1DataHandle = (temPObject.CopyObjectModel3d("all"));
                }          
                if (temPObject != null)
                    temPObject.Dispose();
            }
            /////////////////////////////////////////
            if (this.dist2Value.Count > 0)
            {
                temPObject = new HObjectModel3D(new HTuple(this.x.ToArray()), new HTuple(this.y.ToArray()), new HTuple(this.dist2Value.ToArray()));
                temPObject.SetObjectModel3dAttribMod(new HTuple("&ref_point"), "object", new HTuple(this._AxisParam));
                this.dist2Count = temPObject.GetObjectModel3dParams("num_points").I;
                if (this.dist2Count > 0)
                    value = temPObject.GetObjectModel3dParams("point_coord_z");
                maxValue = 999 < value.TupleMax().D ? 999 : value.TupleMax().D;
                if (this.dist2Count > 1) // 表示是扫描而不是单点采集
                {
                    if (this.dist2DataHandle.IsInitialized())
                        this.dist2DataHandle.Dispose();
                    this.dist2DataHandle = (temPObject.SelectPointsObjectModel3d("point_coord_z", 0, maxValue + 0.01));
                }
                else
                {
                    if (this.dist2DataHandle.IsInitialized())
                        this.dist2DataHandle.Dispose();
                    this.dist2DataHandle = (temPObject.CopyObjectModel3d("all"));
                }
                if (temPObject != null)
                    temPObject.Dispose();
            }
            /////////////////////////////////////////
            if (this.thickValue.Count > 0)
            {
                temPObject = new HObjectModel3D(new HTuple(this.x.ToArray()), new HTuple(this.y.ToArray()), new HTuple(this.thickValue.ToArray()));
                temPObject.SetObjectModel3dAttribMod(new HTuple("&ref_point"), "object", new HTuple(this._AxisParam));
                this.thickCount = temPObject.GetObjectModel3dParams("num_points").I;
                if (this.thickCount > 0)
                    value = temPObject.GetObjectModel3dParams("point_coord_z");
                maxValue = 999 < value.TupleMax().D ? 999 : value.TupleMax().D;
                if (this.thickCount > 1)
                {
                    if (this.thickDataHandle.IsInitialized())
                        this.thickDataHandle.Dispose();
                    this.thickDataHandle = (temPObject.SelectPointsObjectModel3d("point_coord_z", 0, maxValue + 0.01));
                }         
                else
                {
                    if (this.thickDataHandle.IsInitialized())
                        this.thickDataHandle.Dispose();
                    this.thickDataHandle = (temPObject.CopyObjectModel3d("all"));
                }         
                if (temPObject != null)
                    temPObject.Dispose();
            }
            /////////
            ClearData();
        }
        private void UpdataObjectModel(double machine_x, double machine_y, double machine_z, enScanAxis scanAxis, double scanDirection, userLaserCalibrateParam laserCalibrateParam)
        {
            this.dist1Count = this.dist1Value.Count;
            this.dist2Count = this.dist2Value.Count;
            this.thickCount = this.thickValue.Count;
            if (this.dist1Value.Count > 0)
            {
                if (this.dist1DataHandle.IsInitialized())
                    this.dist1DataHandle.Dispose();
                this.dist1DataHandle = (CalibrateLaserSensor(this.x.ToArray(), this.y.ToArray(), this.dist1Value.ToArray(), machine_x, machine_y, machine_z, scanAxis, scanDirection, laserCalibrateParam));
            }
            if (this.dist2Value.Count > 0)
            {
                if (this.dist2DataHandle.IsInitialized())
                    this.dist2DataHandle.Dispose();
                this.dist2DataHandle = (CalibrateLaserSensor(this.x.ToArray(), this.y.ToArray(), this.dist2Value.ToArray(), machine_x, machine_y, machine_z, scanAxis, scanDirection, laserCalibrateParam));
            }
            if (this.thickValue.Count > 0)
            {
                if (this.thickDataHandle.IsInitialized())
                    this.thickDataHandle.Dispose();
                this.thickDataHandle = (CalibrateLaserSensor(this.x.ToArray(), this.y.ToArray(), this.thickValue.ToArray(), machine_x, machine_y, machine_z, scanAxis, scanDirection, laserCalibrateParam));
            }
            ClearData();
        }
        /// <summary>
        /// 校准激光采集数据
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <param name="Px">参考点X</param>
        /// <param name="Py">参考点Y</param>
        /// <param name="Pz">参考点Z</param>
        /// <param name="LaserCalibrateParam">激光校准的位资参数</param>
        /// <returns></returns>
        private HObjectModel3D CalibrateLaserSensor(double[] x, double[] y, double[] z, double Px, double Py, double Pz, enScanAxis scanAxis, double scanDirectionPhi, userLaserCalibrateParam LaserCalibrateParam)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            ////////////////////////////////////////////
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            ////////////////////////////////////////////
            if (z == null)
            {
                throw new ArgumentNullException("z");
            }
            ///
            HTuple Qx, Qy, Dist;
            HHomMat2D hHomMat2D, homMat2dSlant, homMat2dRotate;
            HHomMat3D hHomMat3D, homMat3dTranslate;
            HObjectModel3D hObjectModel3D = null, planeObjectModel3D = null, selectObjectModel3D = null, translateObjectModel3D = null;
            double[] r_x, r_y, r_z, Qz;
            switch (LaserCalibrateParam.LaserType)
            {
                case "点激光":
                    double phi = LaserCalibrateParam.LaserPose.Ry * Math.PI / 180;
                    r_x = new double[z.Length];
                    r_y = new double[z.Length];
                    r_z = new double[z.Length];
                    for (int i = 0; i < z.Length; i++)
                    {
                        if (z[i] >= 0) // 用于点激光校准的数据为正不能为负
                        {
                            r_x[i] = x[i] - z[i] * Math.Sin(phi);
                            r_y[i] = y[i];
                            r_z[i] = z[i] * Math.Cos(phi);
                        }
                        else
                        {
                            //throw new ArgumentException("z值中不能包含有负数");
                            r_x[i] = x[i] + z[i] * Math.Sin(phi);
                            r_y[i] = y[i];
                            r_z[i] = z[i] * Math.Cos(phi);
                        }
                    }
                    hObjectModel3D = new HObjectModel3D(r_x, r_y, r_z);
                    hHomMat3D = new HHomMat3D();
                    homMat3dTranslate = hHomMat3D.HomMat3dTranslate(LaserCalibrateParam.LaserPose.Tx, LaserCalibrateParam.LaserPose.Ty, LaserCalibrateParam.LaserPose.Tz);
                    translateObjectModel3D = hObjectModel3D.RigidTransObjectModel3d(homMat3dTranslate.HomMat3dToPose());
                    translateObjectModel3D.SetObjectModel3dAttribMod(new HTuple("&RefPoint"), "object", new HTuple(Px, Py, Pz));
                    return translateObjectModel3D.SelectPointsObjectModel3d("point_coord_z", -99, 9999999);
                case "线激光":
                    // 计算绕Z轴的旋转参数，XY坐标发了倾斜
                    hHomMat2D = new HHomMat2D();
                    switch (scanAxis)
                    {
                        case enScanAxis.X轴:
                            homMat2dSlant = hHomMat2D.HomMat2dSlant(LaserCalibrateParam.LaserPose.Rz * Math.PI / 180, "y", x[0], y[0]);
                            Qx = homMat2dSlant.AffineTransPoint2d(x, y, out Qy); // 当激光线倾斜安装后，激光线之间的间隔不再等于读取的Y方向步长，而是等于该步长 * 旋转角，所以这里需要将XY坐标剪切处理
                            x = Qx;
                            y = Qy;
                            // 校正扫描线走插补的情况，即校正一个扫描方向角
                            homMat2dRotate = hHomMat2D.HomMat2dRotate(scanDirectionPhi, x[0], y[0]); // 对于面激光，只会发生旋转，而不会发生剪切，所以这里要用旋转
                            Qx = homMat2dRotate.AffineTransPoint2d(x, y, out Qy);
                            break;
                        default:
                        case enScanAxis.Y轴:
                            homMat2dSlant = hHomMat2D.HomMat2dSlant(LaserCalibrateParam.LaserPose.Rz * Math.PI / 180, "x", x[0], y[0]);
                            Qx = homMat2dSlant.AffineTransPoint2d(x, y, out Qy);
                            x = Qx;
                            y = Qy;
                            // 校正扫描线走插补的情况，即校正一个扫描方向角
                            if (scanDirectionPhi > 0)
                                scanDirectionPhi = scanDirectionPhi - Math.PI * 0.5;
                            else
                                scanDirectionPhi = Math.PI * 0.5 + scanDirectionPhi;
                            homMat2dRotate = hHomMat2D.HomMat2dRotate(scanDirectionPhi, x[0], y[0]); // 对于面激光，只会发生旋转，而不会发生剪切，所以这里要用旋转
                            Qx = homMat2dRotate.AffineTransPoint2d(x, y, out Qy);
                            break;
                    }

                    ////////// 计算绕X轴的倾斜，即平行于激光线的轴///////////
                    Qz = new double[z.Length];
                    for (int i = 0; i < z.Length; i++)
                    {
                        Qz[i] = z[i] * Math.Cos(LaserCalibrateParam.LaserPose.Rx * Math.PI / 180);
                    }
                    //************** 计算绕Y轴的旋转参数，即垂直于激光线的轴,用于校正激光的倾斜  ************
                    hObjectModel3D = new HObjectModel3D(Qx, Qy, Qz);
                    planeObjectModel3D = new HObjectModel3D();
                    planeObjectModel3D.GenPlaneObjectModel3d(LaserCalibrateParam.LaserPose.GetHPose(), new HTuple(), new HTuple());
                    hObjectModel3D.DistanceObjectModel3d(planeObjectModel3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true")); // 在计算这个值时只能使用激光数据，而不能平移激光              
                    ///////////////////////  选择范围内的对象 /////////////////
                    selectObjectModel3D = hObjectModel3D.SelectPointsObjectModel3d("&distance", -99, 9999999);
                    Dist = selectObjectModel3D.GetObjectModel3dParams("&distance");
                    selectObjectModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), "", Dist); // Z值使用距离值来重置
                    ///////////////////////  平移对象 /////////////////////
                    hHomMat3D = new HHomMat3D();
                    homMat3dTranslate = hHomMat3D.HomMat3dTranslate(LaserCalibrateParam.LaserPose.Tx + Px, LaserCalibrateParam.LaserPose.Ty + Py, LaserCalibrateParam.LaserPose.Tz + Pz);
                    translateObjectModel3D = selectObjectModel3D.RigidTransObjectModel3d(homMat3dTranslate.HomMat3dToPose());
                    translateObjectModel3D.SetObjectModel3dAttribMod(new HTuple("&RefPoint"), "object", new HTuple(Px, Py, Pz));
                    ///////////////
                    if (planeObjectModel3D != null)
                        planeObjectModel3D.Dispose();
                    if (hObjectModel3D != null)
                        hObjectModel3D.Dispose();
                    if (selectObjectModel3D != null)
                        selectObjectModel3D.Dispose();
                    return translateObjectModel3D;
                case "面激光":
                    // 计算绕Z轴的旋转参数，XY坐标发了倾斜
                    hHomMat2D = new HHomMat2D();
                    homMat2dRotate = hHomMat2D.HomMat2dRotate(LaserCalibrateParam.LaserPose.Rz * Math.PI / 180, x[0], y[0]); // 对于面激光，只会发生旋转，而不会发生剪切，所以这里要用旋转
                    Qx = homMat2dRotate.AffineTransPoint2d(x, y, out Qy);
                    Qz = z;
                    //************** 计算绕Y轴和X轴的旋转参数，即垂直于激光线的轴,用于校正激光的倾斜  ************
                    hObjectModel3D = new HObjectModel3D(Qx, Qy, Qz);
                    planeObjectModel3D = new HObjectModel3D();
                    planeObjectModel3D.GenPlaneObjectModel3d(LaserCalibrateParam.LaserPose.GetHPose(), new HTuple(), new HTuple());
                    hObjectModel3D.DistanceObjectModel3d(planeObjectModel3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true")); // 在计算这个值时只能使用激光数据，而不能平移激光
                    ///////////////////////  选择范围内的对象 /////////////////
                    selectObjectModel3D = hObjectModel3D.SelectPointsObjectModel3d("&distance", -99, 9999999);
                    Dist = selectObjectModel3D.GetObjectModel3dParams("&distance");
                    selectObjectModel3D.SetObjectModel3dAttribMod(new HTuple("point_coord_z"), "", Dist); // Z值使用距离值来重置
                    ///////////////////////  平移对象 /////////////////////
                    hHomMat3D = new HHomMat3D();
                    homMat3dTranslate = hHomMat3D.HomMat3dTranslate(LaserCalibrateParam.LaserPose.Tx + Px, LaserCalibrateParam.LaserPose.Ty + Py, LaserCalibrateParam.LaserPose.Tz + Pz);
                    translateObjectModel3D = selectObjectModel3D.RigidTransObjectModel3d(homMat3dTranslate.HomMat3dToPose());
                    translateObjectModel3D.SetObjectModel3dAttribMod(new HTuple("&RefPoint"), "object", new HTuple(Px, Py, Pz));
                    if (planeObjectModel3D != null)
                        planeObjectModel3D.Dispose();
                    if (hObjectModel3D != null)
                        hObjectModel3D.Dispose();
                    if (selectObjectModel3D != null)
                        selectObjectModel3D.Dispose();
                    return translateObjectModel3D;
                default:
                    return new HObjectModel3D();
            }


        }


        // 获取平均值
        private double[] GetAverangeValue(double[] data, int NumLinePer)
        {
            HalconLibrary ha = new HalconLibrary();
            int num = data.Length / NumLinePer;
            int[] index = new int[num];
            double[] value = new double[NumLinePer];
            double smoothValue;
            double[] points = new double[num];
            double sum = 0;
            for (int i = 0; i < num; i++)
            {
                index[i] = i * NumLinePer;
            }
            //////////
            for (int i = 0; i < NumLinePer; i++)
            {
                sum = 0;
                int aa;
                for (int k = 0; k < index.Length; k++)
                {
                    aa = i + index[k];
                    sum += data[i + index[k]];
                    points[k] = data[i + index[k]];
                }
                ha.GaussSmooth2DPoint(points, out smoothValue);
                value[i] = (smoothValue);
            }
            return value;
        }

        #region 用于标定面激光
        private double[] calibration_zValueForFaceSensor(double[] dist, double offsetZValue, int dataWidth, int dataHeight, double[] calibrateData)
        {
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dataHeight; i++)
            {
                for (int j = 0; j < dataWidth; j++)
                {
                    if (dist[i * dataWidth + j] > 0)
                    {
                        if (calibrateData != null && calibrateData.Length > dataWidth * dataHeight)
                            dd[i * dataWidth + j] = (dist[i * dataWidth + j] - calibrateData[3 + i * dataWidth + j]) * Math.Cos(calibrateData[1] * Math.PI / 180) + offsetZValue;
                        else
                            dd[i * dataWidth + j] = dist[i * dataWidth + j] + offsetZValue;
                    }
                    else
                        dd[i * dataWidth + j] = dist[i * dataWidth + j] + offsetZValue;
                }
            }
            return dd;
        }
        private double[] calibration_zValueForFaceSensor(double[] dist, double[] encoderCoord, double offsetZValue, int dataWidth, int dataHeight, double[] calibrateData)
        {
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dataHeight; i++)
            {
                for (int j = 0; j < dist.Length; j++)
                {
                    if (dist[i * dataWidth + j] > 0)
                    {
                        if (calibrateData != null && calibrateData.Length > dataWidth * dataHeight)
                            dd[i * dataWidth + j] = (dist[i * dataWidth + j] - calibrateData[3 + i * dataWidth + j]) * Math.Cos(calibrateData[1] * Math.PI / 180) + encoderCoord[i * dataWidth + j] + offsetZValue; // 减去参考点还要再乖以一个倾斜角
                        else
                            dd[i * dataWidth + j] = dist[i * dataWidth + j] + encoderCoord[i * dataWidth + j] + offsetZValue;
                    }
                    else
                        dd[i * dataWidth + j] = dist[i * dataWidth + j] + encoderCoord[i * dataWidth + j] + offsetZValue;
                }
            }
            return dd;
        }

        #endregion

        #region 用于标定线激光
        private double[] calibration_zValue(double[] dist, double offsetZValue, int dataWidth, double[] calibrateData)
        {
            int result;
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                Math.DivRem(i, dataWidth, out result);
                if (dist[i] > 0)
                {
                    if (calibrateData != null && calibrateData.Length > 3)
                        dd[i] = (dist[i] - calibrateData[result + 3]) * Math.Cos(calibrateData[1] * Math.PI / 180) + offsetZValue;
                    else
                        dd[i] = dist[i] + offsetZValue;
                }
                else
                    dd[i] = dist[i] + offsetZValue;
            }
            return dd;
        }
        private double[] calibration_zValue(double[] dist, double[] encoderCoord, double offsetZValue, int dataWidth, double[] calibrateData)
        {
            int result;
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                Math.DivRem(i, dataWidth, out result);
                if (dist[i] > 0)
                {
                    if (calibrateData != null && calibrateData.Length > 3)
                        dd[i] = (dist[i] - calibrateData[result + 3]) * Math.Cos(calibrateData[1] * Math.PI / 180) + encoderCoord[i] + offsetZValue; // 减去参考点还要再乖以一个倾斜角
                    else
                        dd[i] = dist[i] + encoderCoord[i] + offsetZValue;
                }
                else
                    dd[i] = dist[i] + encoderCoord[i] + offsetZValue;
            }
            return dd;
        }
        private double[] calibration_xValue(double[] X, double offsetXValue, int dataWidth, double[] calibrateData)
        {
            int result;
            double firstPoint = 0;
            double[] dd = new double[X.Length];

            // 拼接对X方向上的点间隔精度要求很高，不然会拼接不好,所以一定要校准X距离
            for (int i = 0; i < X.Length; i++)
            {
                Math.DivRem(i, dataWidth, out result);
                if (result == 0)
                {
                    firstPoint = X[i]; // 对于某些品牌的三角激光，在不同的高度上激光线上的点间隔距离是不相等的，所以这里不能用第一个点作为参考点，如果在不同的高度上，点间隔相等，则可以使用第一个点作为参考点，这一步就不需要了
                    dd[i] = X[i] + offsetXValue;
                }
                else
                {
                    // 激光线绕Y轴的旋转，不需要考虑X轴上的拉伸，因为即使绕Y轴（垂直于激光线的轴）旋转了，其在面阵传感器上的投影也不会发生变化,所以可以直接使用位姿来表示，即直接做一个刚体变换
                    // 这里是否需要这么做还是按下面注释的做法？绕Z旋转的角度也要考虑进去 ,X[0] 不一定都相等的，这里要注意？ 所以这里还真不能这么做，需要用到行列
                    if (calibrateData != null && calibrateData.Length > 3)
                        dd[i] = X[0] + ((X[i] - X[0]) * Math.Cos(calibrateData[2] * Math.PI / 180)) + offsetXValue;
                    //dd[i] = X[0] + ((X[i] - X[0]) * Math.Cos(calibrateData[2] * Math.PI / 180) / Math.Cos(calibrateData[1] * Math.PI / 180)) + offsetXValue;  // 对于线激光，计算X时不需要考虑高度方向的值，只是将X坐标拉伸了                                                                                                                                                          
                    else
                        dd[i] = X[i] + offsetXValue;// 对于线激光，计算X时不需要考虑高度方向的值，只是将X坐标拉伸了  
                }
            }
            return dd;
        }
        private double[] calibration_yValue(double[] X, double[] Y, double offsetYValue, double[] calibrateData)
        {
            if (Y != null && Y.Length > 0)
            {
                double[] temArray = new double[Y.Length];
                for (int i = 0; i < Y.Length; i++)
                {
                    if (calibrateData != null && calibrateData.Length > 3)
                        temArray[i] = Y[i] + X[i] * Math.Sin(calibrateData[2] * Math.PI / 180) + offsetYValue; // 
                    else
                        temArray[i] = Y[i] + offsetYValue;  // 所有的计算一定要满足和笛卡尔坐标系
                }
                return temArray;    // 需要将激光线上X坐标投影到机台的X轴上，由于每条激光线上的点对应同一个Y值，因此需要将Y坐标值分别对应到相应的激光线点上去，校正Y，即为实现该功能　
            }
            else
                return null;
        }
        #endregion

        #region 用于标定点激光
        private double[] calibration_zValue(double[] dist, double offsetZValue, double[] calibrateData)
        {
            // calibrateData:点激光的校正数据为三个：角度、X偏移、Z偏移，calibrateData[0]：角度；calibrateData[1]：X偏移；calibrateData[2]：Z偏移
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                if (calibrateData != null && calibrateData.Length > 3)
                    dd[i] = (dist[i]) * Math.Cos(calibrateData[0] * Math.PI / 180) + offsetZValue + calibrateData[2];
                else
                    dd[i] = dist[i] + offsetZValue;
            }
            return dd;
        }
        private double[] calibration_zValue(double[] dist, double[] encoderCoord, double offsetZValue, double[] calibrateData)
        {
            // calibrateData:点激光的校正数据为三个：角度、X偏移、Z偏移，calibrateData[0]：角度；calibrateData[1]：X偏移；calibrateData[2]：Z偏移
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                if (calibrateData != null && calibrateData.Length > 3)
                    dd[i] = (dist[i]) * Math.Cos(calibrateData[0] * Math.PI / 180) + encoderCoord[i] + calibrateData[2] + offsetZValue;
                else
                    dd[i] = (dist[i]) + encoderCoord[i] + offsetZValue;
            }
            return dd;
        }
        private double[] calibration_xValue(double[] X, double[] dist, double offsetXValue, double[] calibrateData)
        {
            double[] dd = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                // calibrateData:点激光的校正数据为三个：角度、X偏移、Z偏移，calibrateData[0]：角度；calibrateData[1]：X偏移；calibrateData[2]：Z偏移
                if ((dist[i]) >= 0)
                {
                    if (calibrateData != null && calibrateData.Length > 3)
                        dd[i] = X[i] - (dist[i]) * Math.Sin(calibrateData[0] * Math.PI / 180) + offsetXValue + calibrateData[1];
                    else
                        dd[i] = X[i] + offsetXValue;
                }
                else
                {
                    if (calibrateData != null && calibrateData.Length > 3)
                        dd[i] = X[i] + (dist[i]) * Math.Sin(calibrateData[0] * Math.PI / 180) + offsetXValue + calibrateData[1];
                    else
                        dd[i] = X[i] + offsetXValue;
                }

            }
            return dd;
        }
        #endregion




    }

}
