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
    public class TransformLaserPointCloudDataHandleBeifen
    {
        private List<double> x = new List<double>();
        private List<double> y = new List<double>();
        private List<double> z = new List<double>();
        private List<double> dist1Value = new List<double>();
        private List<double> dist2Value = new List<double>();
        private List<double> thickValue = new List<double>();
        /// ///////////
        [NonSerialized]
        private HTuple dist1DataHandle = null; // 数据句柄
        [NonSerialized]
        private HTuple dist2DataHandle = null; // 数据句柄
        [NonSerialized]
        private HTuple thickDataHandle = null; // 数据句柄
        public HTuple Dist1DataHandle
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
        public HTuple Dist2DataHandle
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
        public HTuple ThickDataHandle
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

        /// <summary>
        /// 转换定点采集数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NumLinePer"></param>
        /// <param name="m_coord"></param>
        public void TransformData(List<object> list, int NumLinePer, double[] m_coord)
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
            laserDist1 = (double[])list[0];
            laserDist2 = (double[])list[1];
            laserThick = (double[])list[2];
            encoder_X = (double[])list[4];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[5];
            encoder_Z = (double[])list[6];
            /// 获取XY坐标　
            if (NumLinePer == 1)  // 表示传感器为点激光
            {

                this.x.Add(m_coord[0]); // 如果没有接入编码器，那么则加入读取的机台坐标值，以机台坐标为准，编码器坐标作为增量坐标来用
                this.y.Add(m_coord[1]);
                ////////////////////////////
                this.z.Add(m_coord[2]);
                ////////////////获取激光测量值
                if (laserDist1 != null && laserDist1.Length > 0)
                {
                    ha.GaussSmooth2DPoint(laserDist1, out value);
                    this.dist1Value.Add(value + m_coord[2]);
                }
                else
                {
                    MessageBox.Show("激光点采集失败");
                }
                /// 距离2
                if (laserDist2 != null && laserDist2.Length > 0)
                {
                    ha.GaussSmooth2DPoint(laserDist2, out value);
                    this.dist2Value.Add(value + m_coord[2]);
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
            }
            //////////////////////线激光 
            if (NumLinePer > 1)
            {
                this.x.AddRange(ArrayAdd(new HTuple(encoder_X).TupleSelectRange(0, NumLinePer - 1).ToDArr(), m_coord[0])); // 激光坐标加上机台坐标是为了拼接
                                                                                                                           ///////////////////////////////////////////////
                this.y.AddRange(ArrayAdd(new HTuple(encoder_Y).TupleSelectRange(0, NumLinePer - 1).ToDArr(), m_coord[1]));
                //////////获取测量的距离值,z值用平均值              
                if (laserDist1 != null && laserDist1.Length > 0)
                {
                    this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, NumLinePer), m_coord[2]));
                }
                else
                {
                    MessageBox.Show("激光点采集失败");
                }
                //////////距离2////////
                if (laserDist2 != null && laserDist2.Length > 0)
                {
                    this.dist2Value.AddRange(ArrayAdd(GetAverangeValue(laserDist2, NumLinePer), m_coord[2]));
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
            }
            laserDist1 = null;
            laserDist2 = null;
            laserThick = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            UpdataObjectModel(m_coord[0], m_coord[1], m_coord[2]);
        }

        public void TransformDataAndCalibrateData(List<object> list, int NumLinePer, double[] m_coord, double[] calibrateData)
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
            laserDist1 = (double[])list[0];
            laserDist2 = (double[])list[1];
            laserThick = (double[])list[2];
            encoder_X = (double[])list[4];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[5];
            encoder_Z = (double[])list[6];
            /// 获取XY坐标　
            if (NumLinePer == 1)  // 表示传感器为点激光
            {
                this.x.Add(m_coord[0]); // 如果没有接入编码器，那么则加入读取的机台坐标值，以机台坐标为准，编码器坐标作为增量坐标来用
                this.y.Add(m_coord[1]);
                ////////////////////////////
                this.z.Add(m_coord[2]);
                ////////////////获取激光测量值
                if (laserDist1 != null && laserDist1.Length > 0)
                {
                    ha.GaussSmooth2DPoint(laserDist1, out value);
                    this.dist1Value.Add(value * Math.Cos(calibrateData[0]) + m_coord[2]);
                }
                else
                {
                    MessageBox.Show("激光点采集失败");
                }
                /// 距离2
                if (laserDist2 != null && laserDist2.Length > 0)
                {
                    ha.GaussSmooth2DPoint(laserDist2, out value);
                    this.dist2Value.Add(value * Math.Cos(calibrateData[0]) + m_coord[2]);
                }
                else
                {
                    // this.dist2Value.Add(-1024);
                }
                //////////////////
                if (laserThick != null && laserThick.Length > 0)
                {
                    ha.GaussSmooth2DPoint(laserThick, out value);
                    this.thickValue.Add(value * Math.Cos(calibrateData[0]) + 0); // 加上Z轴坐标是为了计算量程不够时计算方便, 但厚度测量不需要加Z值 m_coord[2] 
                }
                else
                {
                    //this.thickValue.Add(-1024);
                }
            }
            //////////////////////线激光 
            if (NumLinePer > 1)
            {
                this.x.AddRange(ArrayAdd(new HTuple(encoder_X).TupleSelectRange(0, NumLinePer - 1).ToDArr(), m_coord[0])); // 激光坐标加上机台坐标是为了拼接
                                                                                                                           ///////////////////////////////////////////////
                this.y.AddRange(ArrayAdd(new HTuple(encoder_Y).TupleSelectRange(0, NumLinePer - 1).ToDArr(), m_coord[1]));
                //////////获取测量的距离值,z值用平均值              
                if (laserDist1 != null && laserDist1.Length > 0)
                {
                    this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, NumLinePer), m_coord[2]));
                }
                else
                {
                    MessageBox.Show("激光点采集失败");
                }
                //////////距离2////////
                if (laserDist2 != null && laserDist2.Length > 0)
                {
                    this.dist2Value.AddRange(ArrayAdd(GetAverangeValue(laserDist2, NumLinePer), m_coord[2]));
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
            }
            laserDist1 = null;
            laserDist2 = null;
            laserThick = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            UpdataObjectModel(m_coord[0], m_coord[1], m_coord[2]);
        }

        /// <summary>
        /// 转换扫描数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NumLinePer"></param>
        /// <param name="step_y"></param>
        /// <param name="m_coord1"></param>
        /// <param name="m_coord2"></param>
        public void TransformData(List<object> list, int NumLinePer, double step_y, double[] m_coord1, double[] m_coord2)
        {
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            /////////////////////////////////////
            laserDist1 = (double[])list[0];
            laserDist2 = (double[])list[1];
            laserThick = (double[])list[2];
            encoder_X = (double[])list[4];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[5];
            encoder_Z = (double[])list[6];
            /////////////////////////////////////////////
            if (NumLinePer == 1) // 表示传感器为点激光
            {
                if (encoder_X != null && encoder_X.Length > 1 && Math.Abs(encoder_X.Max() - encoder_X.Min()) > 0) // 如果没有接入编码器，值应该为了
                {
                    this.x.AddRange(ArrayAdd(encoder_X, m_coord1[0])); // 编码器的坐标为相对坐标
                }
                else
                {
                    this.x.AddRange(GenSequence(m_coord1[0], m_coord1[1], m_coord2[0], m_coord2[1], laserDist1.Length, NumLinePer));
                    // 点激光不需要加当前机台坐标，因为XY都是当前机台坐标                
                }
                /////////////////////
                if (encoder_Y != null && encoder_Y.Length > 1 && Math.Abs(encoder_Y.Max() - encoder_Y.Min()) > 0)
                {
                    this.y.AddRange(ArrayAdd(encoder_Y, m_coord1[1]));
                }
                else
                {
                    this.y.AddRange(GenConstSequence(m_coord1[1], laserDist1.Length));
                }
                /////////////////////
                if (encoder_Z != null && encoder_Z.Length > 0)
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer));// 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0));
                }
                else
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, m_coord1[2]));
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, m_coord1[2]));// 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // 如果需要实时读取Z轴坐标，则需要使用他
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (NumLinePer > 1) // 表示传感器为线激光
            {
                //double value = encoder_Y[50000];
                this.x.AddRange(ArrayAdd(encoder_X, m_coord1[0])); // 不会有人去把激光线摆成斜的
                if (encoder_Y != null && encoder_Y.Length > 1 && Math.Abs(encoder_Y.Max() - encoder_Y.Min()) > 0)
                {
                    this.y.AddRange(ArrayAdd(encoder_Y, m_coord1[1])); //Y坐标要加一个值,encoder_Y:采用相对坐标

                    double maxY = encoder_Y.Max();
                    double minY = encoder_Y.Min();
                }
                else
                {
                    if (step_y != 0)
                    {
                        if (m_coord1[1] > m_coord2[1])
                            this.y.AddRange(GenYSequence(m_coord1[1], -step_y, laserDist1.Length, NumLinePer)); // 这里要不要给个启始点？
                        else
                            this.y.AddRange(GenYSequence(m_coord1[1], step_y, laserDist1.Length, NumLinePer)); // 这里要不要给个启始点？
                    }
                    else
                        this.y.AddRange(GenSequence(m_coord1[0], m_coord1[1], m_coord2[0], m_coord2[1], laserDist1.Length, NumLinePer));
                }
                //////////////////
                if (encoder_Z != null && encoder_Z.Length > 0)
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)); // 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer)); // 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, encoder_Z, m_coord1[2], NumLinePer));
                }
                else
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(ArrayAdd(laserDist1, m_coord1[2]));
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(ArrayAdd(laserDist2, m_coord1[2])); // 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, m_coord1[2])); // 如果需要实时读取Z轴坐标，则需要使用他
                }
            }
            // 最后清空数据
            laserDist1 = null;
            laserDist2 = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            laserThick = null;
            UpdataObjectModel(m_coord1[0], m_coord1[1], m_coord1[2]);
        }

        /// <summary>
        /// 转换扫描数据,并校准激光安装带来的误差，主要校准绕Y轴的倾斜
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NumLinePer"></param>
        /// <param name="step_y"></param>
        /// <param name="m_coord1"></param>
        /// <param name="m_coord2"></param>
        public void TransformDataAndCalibrateData(List<object> list, int NumLinePer, double step_y, double[] m_coord1, double[] m_coord2, double[] calibrateData)
        {
            double[] laserDist1;
            double[] laserDist2;
            double[] encoder_X;
            double[] encoder_Y;
            double[] encoder_Z;
            double[] laserThick;
            /////////////////////////////////////
            laserDist1 = (double[])list[0];
            laserDist2 = (double[])list[1];
            laserThick = (double[])list[2];
            encoder_X = (double[])list[4];  // 如果没接入编码器，则值为空
            encoder_Y = (double[])list[5];
            encoder_Z = (double[])list[6];
            /////////////////////////////////////////////
            if (NumLinePer == 1) // 表示传感器为点激光, 点激光的校准也放到这里面来做
            {
                if (encoder_X != null && encoder_X.Length > 1 && Math.Abs(encoder_X.Max() - encoder_X.Min()) > 0.001) // 如果没有接入编码器，值应该为了
                {
                    this.x.AddRange(calibration_xValue(encoder_X, laserDist1, m_coord1[0], calibrateData)); // 编码器的坐标为相对坐标 ArrayAdd(encoder_X, m_coord1[0])
                }
                else
                {
                    this.x.AddRange(GenSequence(m_coord1[0], m_coord1[1], m_coord2[0], m_coord2[1], laserDist1.Length, NumLinePer));
                    // 点激光不需要加当前机台坐标，因为XY都是当前机台坐标                
                }
                /////////////////////
                if (encoder_Y != null && encoder_Y.Length > 1 && Math.Abs(encoder_Y.Max() - encoder_Y.Min()) > 0.001)
                {
                    this.y.AddRange(ArrayAdd(encoder_Y, m_coord1[1]));
                }
                else
                {
                    this.y.AddRange(GenConstSequence(m_coord1[1], laserDist1.Length));
                }
                /////////////////////
                if (encoder_Z != null && encoder_Z.Length > 0)
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, m_coord1[2], calibrateData)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, m_coord1[2], calibrateData));// 如果需要实时读取Z轴坐标，则需要使用他ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer)
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // ArrayAdd(laserThick, encoder_Z, m_coord1[2], NumLinePer)
                }
                else
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, m_coord1[2], calibrateData)); // 因为Z值采用的也是相对坐标，所以这里需要加上一个机台高度ArrayAdd(laserDist1, encoder_Z, m_coord1[2], NumLinePer)
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, m_coord1[2], calibrateData));// 如果需要实时读取Z轴坐标，则需要使用他ArrayAdd(laserDist2, encoder_Z, m_coord1[2], NumLinePer)
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // ArrayAdd(laserThick, encoder_Z, m_coord1[2], NumLinePer)
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (NumLinePer > 1) // 表示传感器为线激光
            {
                this.x.AddRange(calibration_xValue(encoder_X,  m_coord1[0], NumLinePer, calibrateData)); // 不会有人去把激光线摆成斜的
                if (encoder_Y != null && encoder_Y.Length > 1 && Math.Abs(encoder_Y.Max() - encoder_Y.Min()) > 0)
                {
                    this.y.AddRange(ArrayAdd(encoder_Y, m_coord1[1])); //Y坐标要加一个值,encoder_Y:采用相对坐标

                    double maxY = encoder_Y.Max();
                    double minY = encoder_Y.Min();
                }
                else
                {
                    if (step_y != 0)
                    {
                        if (m_coord1[1] > m_coord2[1])
                            this.y.AddRange(GenYSequence(m_coord1[1], -step_y, laserDist1.Length, NumLinePer)); // 这里要不要给个启始点？
                        else
                            this.y.AddRange(GenYSequence(m_coord1[1], step_y, laserDist1.Length, NumLinePer)); // 这里要不要给个启始点？
                    }
                    else
                        this.y.AddRange(GenSequence(m_coord1[0], m_coord1[1], m_coord2[0], m_coord2[1], laserDist1.Length, NumLinePer));
                }
                //////////////////
                if (encoder_Z != null && encoder_Z.Length > 0)
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, m_coord1[2], NumLinePer, calibrateData)); // 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, m_coord1[2], NumLinePer, calibrateData)); // 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0));// 厚度值不需要加
                }
                else
                {
                    if (laserDist1 != null && laserDist1.Length > 0) this.dist1Value.AddRange(calibration_zValue(laserDist1, m_coord1[2], NumLinePer, calibrateData));
                    if (laserDist2 != null && laserDist2.Length > 0) this.dist2Value.AddRange(calibration_zValue(laserDist2, m_coord1[2], NumLinePer, calibrateData)); // 如果需要实时读取Z轴坐标，则需要使用他
                    if (laserThick != null && laserThick.Length > 0) this.thickValue.AddRange(ArrayAdd(laserThick, 0)); // 如果需要实时读取Z轴坐标，则需要使用他
                }
            }
            // 最后清空数据
            laserDist1 = null;
            laserDist2 = null;
            encoder_X = null;
            encoder_Y = null;
            encoder_Z = null;
            laserThick = null;
            UpdataObjectModel(m_coord1[0], m_coord1[1], m_coord1[2]);
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
        private double[] ArrayAdd(double[] array, double[] value, int countPerLine)
        {
            int result;
            int k = 0;
            //////////////////////////////////////////////////////////////////////////
            if (array != null && array.Length > 0)
            {
                double[] temArray = new double[array.Length];
                if (array.Length > value.Length)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        Math.DivRem(i, countPerLine, out result);
                        if (i != 0 && result == 0)
                            k++;
                        temArray[i] = array[i] + value[k];
                    }
                }
                else
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        temArray[i] = value[i] + array[i];
                    }
                }
                return temArray;
            }
            else
                return null;
        }
        private double[] ArrayAdd(double[] array, double[] value, double offsetvalue, int countPerLine)
        {
            int result;
            int k = 0;
            //////////////////////////////////////////////////////////////////////////
            if (array != null && array.Length > 0)
            {
                double[] temArray = new double[array.Length];
                if (array.Length > value.Length)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        Math.DivRem(i, countPerLine, out result);
                        if (i != 0 && result == 0)
                            k++;
                        temArray[i] = array[i] + value[k] + offsetvalue;
                    }
                }
                else
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        temArray[i] = value[i] + array[i] + offsetvalue;
                    }
                }
                return temArray;
            }
            else
                return null;
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
            HalconLibrary ha = new HalconLibrary();
            ha.ClearObjectModel3D(this.dist1DataHandle);
            ha.ClearObjectModel3D(this.dist2DataHandle);
            ha.ClearObjectModel3D(this.thickDataHandle);
            // 使用类，这里就不需要再清空了
        }
        private void UpdataObjectModel(double x, double y, double z)
        {
            HalconLibrary ha = new HalconLibrary();
            //更新数据
            if (this.dist1Value.Count > 0)
            {
                ha.ClearObjectModel3D(this.dist1DataHandle);
                ha.UpdataObjectModel(new HTuple(this.x.ToArray()), new HTuple(this.y.ToArray()), new HTuple(this.dist1Value.ToArray()), out this.dist1DataHandle);
                HOperatorSet.SetObjectModel3dAttribMod(this.dist1DataHandle, "&refPoint", new HTuple(), new HTuple(x, y, z));
            }
            if (this.dist2Value.Count > 0)
            {
                ha.ClearObjectModel3D(this.dist2DataHandle);
                ha.UpdataObjectModel(new HTuple(this.x.ToArray()), new HTuple(this.y.ToArray()), new HTuple(this.dist2Value.ToArray()), out this.dist2DataHandle);
                HOperatorSet.SetObjectModel3dAttribMod(this.dist2DataHandle, "&refPoint", new HTuple(), new HTuple(x, y, z));
            }
            if (this.thickValue.Count > 0)
            {
                ha.ClearObjectModel3D(this.thickDataHandle);
                ha.UpdataObjectModel(new HTuple(this.x.ToArray()), new HTuple(this.y.ToArray()), new HTuple(this.thickValue.ToArray()), out this.thickDataHandle);
                HOperatorSet.SetObjectModel3dAttribMod(this.thickDataHandle, "&refPoint", new HTuple(), new HTuple(x, y, z));
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

        #region 用于线激光
        private double[] calibration_zValue(double[] dist, double offsetZValue, int countPerLine, double[] calibrateData)
        {
            int result;
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                Math.DivRem(i, countPerLine, out result);
                if (dist[i] > 0)
                    dd[i] = (dist[i] - calibrateData[result + 3]) * Math.Cos(calibrateData[1] * Math.PI / 180) + offsetZValue;
                else
                    dd[i] = dist[i] + offsetZValue;
            }
            return dd;
        }
        private double[] calibration_xValue(double[] X, double offsetXValue, int countPerLine, double[] calibrateData)
        {
            int result;
            double[] dd = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                Math.DivRem(i, countPerLine, out result);
                if (result == 0)
                    dd[i] = X[i] + offsetXValue;
                else
                    dd[i] = X[i-1] + (X[i] - X[i - 1]) / Math.Cos(calibrateData[1]*Math.PI/180) + offsetXValue;  // 对于线激光，计算X时不需要考虑高度方向的值，只是将X坐标拉伸了
            }
            return dd;
        }
        //private double[] calibration_xValue(double[] X, double[] dist, double offsetXValue, int countPerLine, double[] calibrateData)
        //{
        //    int result;
        //    double[] dd = new double[X.Length];
        //    for (int i = 0; i < X.Length; i++)
        //    {
        //        Math.DivRem(i, countPerLine, out result);
        //        if ((dist[i] - calibrateData[result + 3]) >= 0)
        //            dd[i] = X[i] - (dist[i] - calibrateData[result + 3]) * Math.Sin(calibrateData[1]) + offsetXValue;
        //        else
        //            dd[i] = X[i] + (dist[i] - calibrateData[result + 3]) * Math.Sin(calibrateData[1]) + offsetXValue;
        //    }
        //    return dd;
        //}
        #endregion

        #region 用于点激光
        private double[] calibration_zValue(double[] dist, double offsetZValue, double[] calibrateData)
        { 
            // calibrateData:点激光的校正数据为三个：角度、X偏移、Z偏移，calibrateData[1]：X偏移；calibrateData[2]：Z偏移
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                dd[i] = (dist[i]) * Math.Cos(calibrateData[0]*Math.PI/180) + offsetZValue + calibrateData[2];
            }
            return dd;
        }
        private double[] calibration_xValue(double[] X, double[] dist, double offsetXValue, double[] calibrateData)
        {
            double[] dd = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                // calibrateData:点激光的校正数据为三个：角度、X偏移、Z偏移，calibrateData[1]：X偏移；calibrateData[2]：Z偏移
                if ((dist[i]) >= 0)
                    dd[i] = X[i] - (dist[i]) * Math.Sin(calibrateData[0] * Math.PI / 180) + offsetXValue + calibrateData[1];
                else
                    dd[i] = X[i] + (dist[i]) * Math.Sin(calibrateData[0] * Math.PI / 180) + offsetXValue + calibrateData[1];
            }
            return dd;
        }
        #endregion

    }
}
