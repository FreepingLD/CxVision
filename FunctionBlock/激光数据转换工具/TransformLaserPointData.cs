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
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class TransformLaserPointData
    {
        private List<double> x = new List<double>();
        private List<double> y = new List<double>();
        private List<double> z = new List<double>();
        private List<double> dist1Value = new List<double>();
        private List<double> dist2Value = new List<double>();
        private List<double> thickValue = new List<double>();
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
        public void TransformData(Dictionary<enDataItem, object> list, int NumLinePer, double[] m_coord)
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
            /// 获取XY坐标　
            if (NumLinePer == 1)  // 表示传感器为点激光
            {
                //if (encoder_X != null && encoder_X.Length > 0)
                //    this.x.Add(m_coord[0] + (SumArray(encoder_X) / encoder_X.Length));
                //else
                this.x.Add(m_coord[0]); // 如果没有接入编码器，那么则加入读取的机台坐标值，以机台坐标为准，编码器坐标作为增量坐标来用
                ///////////////////////////
                //if (encoder_Y != null && encoder_Y.Length > 0)
                //    this.y.Add(m_coord[1] + (SumArray(encoder_Y) / encoder_Y.Length));
                //else
                this.y.Add(m_coord[1]);
                ////////////////////////////
                //if (encoder_Z != null && encoder_Z.Length > 0)
                //    this.z.Add(m_coord[2] + (SumArray(encoder_Z) / encoder_Z.Length));
                //else
                this.z.Add(m_coord[2]);
                ////////////////获取激光测量值
                if (laserDist1 != null && laserDist1.Length > 0)
                {
                    ha.GaussSmooth2DPoint(laserDist1, out value);
                    //if (encoder_Z != null && encoder_Z.Length > 0)
                    //    this.dist1Value.Add(value + m_coord[2] + (SumArray(encoder_Z) / encoder_Z.Length)); // (SumArray(encoder_Z) / encoder_Z.Length) 这一个是不是可以不要？
                    //else
                    this.dist1Value.Add(value + m_coord[2]);
                }
                else
                {
                    //this.dist1Value.Add(-1024);
                    MessageBox.Show("激光点采集失败");
                }
                /// 距离2
                if (laserDist2 != null && laserDist2.Length > 0)
                {
                    ha.GaussSmooth2DPoint(laserDist2, out value);
                    //if (encoder_Z != null && encoder_Z.Length > 0)
                    //    this.dist2Value.Add(value + m_coord[2] + (SumArray(encoder_Z) / encoder_Z.Length));
                    //else
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
                    //if (encoder_Z != null && encoder_Z.Length > 0)
                    //    this.thickValue.Add(value + m_coord[2] + (SumArray(encoder_Z) / encoder_Z.Length));
                    //else
                    this.thickValue.Add(value + m_coord[2]); // 加上Z轴坐标是为了计算量程不够时计算方便
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
                //if (encoder_Y != null && encoder_Y.Length > 0)
                //    this.y.AddRange(ArrayAdd(new HTuple(encoder_Y).TupleSelectRange(0, NumLinePer - 1).ToDArr(), m_coord[1] + SumArray(encoder_Y)));
                //else
                this.y.AddRange(ArrayAdd(new HTuple(encoder_Y).TupleSelectRange(0, NumLinePer - 1).ToDArr(), m_coord[1]));
                //////////获取测量的距离值,z值用平均值              
                if (laserDist1 != null && laserDist1.Length > 0)
                {
                    //if (encoder_Z != null && encoder_Z.Length > 0)
                    //    this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, NumLinePer), m_coord[2] + SumArray(encoder_Z)));
                    //else
                    this.dist1Value.AddRange(ArrayAdd(GetAverangeValue(laserDist1, NumLinePer), m_coord[2]));
                }
                else
                {
                    MessageBox.Show("激光点采集失败");
                }
                //////////距离2////////
                if (laserDist2 != null && laserDist2.Length > 0)
                {
                    //if (encoder_Z != null && encoder_Z.Length > 0)
                    //    this.dist2Value.AddRange(ArrayAdd(GetAverangeValue(laserDist2, NumLinePer), m_coord[2])+SumArray(encoder_Z));
                    //else
                    this.dist2Value.AddRange(ArrayAdd(GetAverangeValue(laserDist2, NumLinePer), m_coord[2]));
                }
                else
                {
                }
                ///////////获取测量的厚度值             
                if (laserThick != null && laserThick.Length > 0)
                {
                    //if (encoder_Z != null && encoder_Z.Length > 0)
                    //    this.thickValue.AddRange(ArrayAdd(GetAverangeValue(laserThick, NumLinePer), m_coord[2]+SumArray(encoder_Z))); // 编码器坐标是否应该与机台坐标相等？
                    //else
                    this.thickValue.AddRange(ArrayAdd(GetAverangeValue(laserThick, NumLinePer), m_coord[2]));
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
        }
        private double[] ArrayAdd(double[] array, double value)
        {
            if (array != null && array.Length > 0)
            {
                double[] temArray = new double[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    temArray[i] = value + array[i];
                }
                return temArray;
            }
            else
                return null;
        }
        private double SumArray(double[] array)
        {
            double sum = 0.0;
            if (array != null && array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    sum += array[i];
                }
            }
            return sum;
        }
        private int[] GetIndex(double[] data, int NumLinePer)
        {
            int num = data.Length / NumLinePer;
            int[] index = new int[num];
            for (int i = 0; i < num; i++)
            {
                index[i] = i * NumLinePer;
            }
            return index;
        }
        private double[] GetAverangeValue(double[] data, int NumLinePer)
        {
            int num = data.Length / NumLinePer;
            int[] index = new int[num];
            double[] value = new double[NumLinePer];
            double sum = 0;
            for (int i = 0; i < num; i++)
            {
                index[i] = i * NumLinePer;
            }
            //////////
            for (int i = 0; i < NumLinePer; i++)
            {
                sum = 0;
                for (int k = 0; k < index.Length; k++)
                {
                    sum += data[i + k];
                }
                value[i] = Convert.ToSingle(sum / num);
            }
            return value;
        }
        public void Clear()
        {
            this.x.Clear();
            this.y.Clear();
            this.z.Clear();
            this.dist1Value.Clear();
            this.dist2Value.Clear();
            this.thickValue.Clear();
        }



    }

}
