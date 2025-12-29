using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;
using System.Diagnostics;
using AxMSTSCLib;

namespace FunctionBlock
{
    [Serializable]
    public class HeadFindDieMethod
    {

        public static bool FindDieMethod(userWcsPoint[] searchPoints, out userWcsPoint wcsPoint) //, out RfidDieIndex dieIndex
        {
            bool result = false;
            wcsPoint = new userWcsPoint();
            //dieIndex = new RfidDieIndex();
            if (searchPoints == null) throw new ArgumentNullException("points");
            ///////////////////////////////////////////////////////
            userWcsPoint refWcsPoint  = new userWcsPoint();
            for (int i = 0; i < searchPoints.Length; i++)
            {
                refWcsPoint= searchPoints[i];
                foreach (var item in searchPoints)
                {

                }
            }

          
            return result;

        }

        public static bool FindDieMethod2(userWcsPoint refWcsPoint, userWcsPoint[] searchPoints, WaferFindDieParam param, out userWcsPoint[] wcsPoint, out userWcsPoint targetWcsPoint)
        {
            bool result = false;
            targetWcsPoint = new userWcsPoint();
            wcsPoint = new userWcsPoint[0];
            if (searchPoints == null) throw new ArgumentNullException("points");
            if (param == null) throw new ArgumentNullException("param");
            if (searchPoints.Length == 0) return result;
            switch (param.WaferFindDie)
            {
                default:
                case enWaferFindDie.九宫格寻晶:
                    double legth_dis = Math.Sqrt(param.Dist_X * param.Dist_X + param.Dist_Y * param.Dist_Y); // 三角形的对角线长度
                    List<double> listDist = new List<double>();
                    double temPDist = double.MaxValue;
                    int index = 0;
                    // 寻找中心点,中心点不一定会有
                    for (int i = 0; i < searchPoints.Length; i++)
                    {
                        double dist = 0;//
                        if (refWcsPoint != null) // 以指定的参考点作为九宫格的中心点
                            dist = Math.Sqrt((searchPoints[i].X - refWcsPoint.X) * (searchPoints[i].X - refWcsPoint.X) + (searchPoints[i].Y - refWcsPoint.Y) * (searchPoints[i].Y - refWcsPoint.Y));
                        else // 以当前坐标位置作为九宫格的中心点，需满足相机坐标系原点在视野中心
                            dist = Math.Sqrt((searchPoints[i].X - searchPoints[i].Grab_x) * (searchPoints[i].X - searchPoints[i].Grab_x) + (searchPoints[i].Y - searchPoints[i].Grab_y) * (searchPoints[i].Y - searchPoints[i].Grab_y));
                        ///////////////////////////////////////
                        if (dist < temPDist)
                        {
                            temPDist = dist;
                            index = i;
                        }
                    }
                    ////////////////////////////////////////// 
                    userWcsPoint origionPoint = searchPoints[index];
                    double dist1 = double.MaxValue, dist2 = double.MaxValue, dist3 = double.MaxValue, dist4 = double.MaxValue, dist5 = double.MaxValue, dist6 = double.MaxValue, dist7 = double.MaxValue, dist8 = double.MaxValue, dist9 = double.MaxValue;
                    Dictionary<int, userWcsPoint> dic = new Dictionary<int, userWcsPoint>();
                    for (int i = 1; i <= 9; i++)
                    {
                        dic.Add(i, null);
                    }
                    dic[5] = origionPoint;
                    //dic.Add(5, origionPoint); // 中间点
                    foreach (var item in searchPoints)
                    {
                        double dist = Math.Sqrt((item.X - origionPoint.X) * (item.X - origionPoint.X) + (item.Y - origionPoint.Y) * (item.Y - origionPoint.Y));
                        double phi = Math.Atan2(item.Y - origionPoint.Y, item.X - origionPoint.X);
                        /// 记录第一个点
                        if (Math.Abs(135 * Math.PI / 180 - phi) <= 20 * Math.PI / 180) //125*Math.PI/180 <= phi &&  phi <= 165 * Math.PI / 180
                        {
                            if (dist <= dist1)
                            {
                                dist1 = dist;
                                if (dic.ContainsKey(1))
                                    dic[1] = item;
                                else
                                    dic.Add(1, item);
                            }
                            //if (legth_dis * 0.8 <= dist && dist <= legth_dis * 1.2)
                            //{
                            //    if (dic.ContainsKey(1))
                            //        dic[1] = item;
                            //    else
                            //        dic.Add(1, item);
                            //}    
                        }
                        /// 记录第二个点
                        if (Math.Abs(90 * Math.PI / 180 - phi) <= 20 * Math.PI / 180) //70 * Math.PI / 180 <= phi && phi <= 110 * Math.PI / 180
                        {
                            if (dist <= dist2)
                            {
                                dist2 = dist;
                                if (dic.ContainsKey(2))
                                    dic[2] = item;
                                else
                                    dic.Add(2, item);
                            }
                            //if (param.Dist_Y * 0.8 <= dist && dist <= param.Dist_Y * 1.2)
                            //{
                            //    if (dic.ContainsKey(2))
                            //        dic[2] = item;
                            //    else
                            //        dic.Add(2, item);
                            //}
                        }
                        /// 记录第三个点
                        if (Math.Abs(45 * Math.PI / 180 - phi) <= 20 * Math.PI / 180)
                        {
                            if (dist <= dist3)
                            {
                                dist3 = dist;
                                if (dic.ContainsKey(3))
                                    dic[3] = item;
                                else
                                    dic.Add(3, item);
                            }

                            //if (legth_dis * 0.8 <= dist && dist <= legth_dis * 1.2)
                            //{
                            //    if (dic.ContainsKey(3))
                            //        dic[3] = item;
                            //    else
                            //        dic.Add(3, item);
                            //}
                        }
                        /// 记录第四个点
                        if (Math.Abs(180 * Math.PI / 180 - phi) <= 20 * Math.PI / 180)
                        {
                            if (dist <= dist4)
                            {
                                dist4 = dist;
                                if (dic.ContainsKey(4))
                                    dic[4] = item;
                                else
                                    dic.Add(4, item);
                            }
                            //if (param.Dist_X * 0.8 <= dist && dist <= param.Dist_X * 1.2)
                            //{
                            //    if (dic.ContainsKey(4))
                            //        dic[4] = item;
                            //    else
                            //        dic.Add(4, item);
                            //}
                        }
                        /// 记录第六个点
                        if (Math.Abs(0 * Math.PI / 180 - phi) <= 20 * Math.PI / 180)
                        {
                            if (dist <= dist6)
                            {
                                dist6 = dist;
                                if (dic.ContainsKey(6))
                                    dic[6] = item;
                                else
                                    dic.Add(6, item);
                            }
                            //if (param.Dist_X * 0.8 <= dist && dist <= param.Dist_X * 1.2)
                            //{
                            //    if (dic.ContainsKey(6))
                            //        dic[6] = item;
                            //    else
                            //        dic.Add(6, item);
                            //}
                        }
                        /// 记录第七个点
                        if (Math.Abs(-135 * Math.PI / 180 - phi) <= 20 * Math.PI / 180)
                        {
                            if (dist <= dist7)
                            {
                                dist7 = dist;
                                if (dic.ContainsKey(7))
                                    dic[7] = item;
                                else
                                    dic.Add(7, item);
                            }
                            //if (legth_dis * 0.8 <= dist && dist <= legth_dis * 1.2)
                            //{
                            //    if (dic.ContainsKey(7))
                            //        dic[7] = item;
                            //    else
                            //        dic.Add(7, item);
                            //}
                        }
                        /// 记录第八个点
                        if (Math.Abs(-90 * Math.PI / 180 - phi) <= 20 * Math.PI / 180)
                        {
                            if (dist <= dist8)
                            {
                                dist8 = dist;
                                if (dic.ContainsKey(8))
                                    dic[8] = item;
                                else
                                    dic.Add(8, item);
                            }
                            //if (param.Dist_Y * 0.8 <= dist && dist <= param.Dist_Y * 1.2)
                            //{
                            //    if (dic.ContainsKey(8))
                            //        dic[8] = item;
                            //    else
                            //        dic.Add(8, item);
                            //}
                        }
                        /// 记录第九个点
                        if (Math.Abs(-45 * Math.PI / 180 - phi) <= 20 * Math.PI / 180)
                        {
                            if (dist <= dist9)
                            {
                                dist9 = dist;
                                if (dic.ContainsKey(9))
                                    dic[9] = item;
                                else
                                    dic.Add(9, item);
                            }
                            //if (legth_dis * 0.8 <= dist && dist <= legth_dis * 1.2)
                            //{
                            //    if (dic.ContainsKey(9))
                            //        dic[9] = item;
                            //    else
                            //        dic.Add(9, item);
                            //}
                        }
                    }
                    /////////// 输出匹配点 //////////
                    wcsPoint = new userWcsPoint[dic.Count];
                    dic.Values.CopyTo(wcsPoint, 0);
                    /////////// 输出目标点 //////////
                    switch (param.CoordQuadrant)
                    {
                        case enCoordQuadrant.第一象限:
                            if (dic[3] != null)
                                targetWcsPoint = dic[3];
                            else
                            {
                                if (dic[6] != null)
                                    targetWcsPoint = dic[6];
                                else
                                {
                                    if (dic[2] != null)
                                        targetWcsPoint = dic[2];
                                }
                            }
                            break;
                        case enCoordQuadrant.第二象限:
                            if (dic[1] != null)
                                targetWcsPoint = dic[1];
                            else
                            {
                                if (dic[4] != null)
                                    targetWcsPoint = dic[4];
                                else
                                {
                                    if (dic[2] != null)
                                        targetWcsPoint = dic[2];
                                }
                            }
                            break;
                        case enCoordQuadrant.第三象限:
                            if (dic[7] != null)
                                targetWcsPoint = dic[7];
                            else
                            {
                                if (dic[4] != null)
                                    targetWcsPoint = dic[4];
                                else
                                {
                                    if (dic[8] != null)
                                        targetWcsPoint = dic[8];
                                }
                            }
                            break;
                        case enCoordQuadrant.第四象限:
                            if (dic[9] != null)
                                targetWcsPoint = dic[9];
                            else
                            {
                                if (dic[6] != null)
                                    targetWcsPoint = dic[6];
                                else
                                {
                                    if (dic[8] != null)
                                        targetWcsPoint = dic[8];
                                }
                            }
                            break;
                    }
                    result = true;
                    break;
            }
            return result;

        }


    }

}
