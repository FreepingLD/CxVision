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
    public class WaferFindDieMethod
    {

        public static bool FindDieMethod(userWcsPoint refWcsPoint, userWcsPoint[] searchPoints, WaferFindDieParam param, out userWcsPoint[] wcsPoint, out userWcsPoint targetWcsPoint) //, out RfidDieIndex dieIndex
        {
            bool result = false;
            targetWcsPoint = new userWcsPoint();
            wcsPoint = new userWcsPoint[0];
            //dieIndex = new RfidDieIndex();
            if (searchPoints == null) throw new ArgumentNullException("points");
            if (param == null) throw new ArgumentNullException("param");
            if (searchPoints.Length == 0) return result;
            userWcsPoint _refWcsPoint = refWcsPoint?.Clone(); // 这里需要复制一个对象，不然修改时会将原对象也一起修改了
            ///////////////////////////////////////////////////////
            switch (param.WaferFindDie)
            {
                default:
                case enWaferFindDie.九宫格寻晶:
                    double length_dis = Math.Sqrt(param.Dist_X * param.Dist_X + param.Dist_Y * param.Dist_Y); // 三角形的对角线长度
                    List<double> listDist = new List<double>();
                    double temPDist = double.MaxValue;
                    int index = 0;
                    // 寻找中心点,中心点不一定会有
                    switch (param.RefObject)
                    {
                        case enRefObject.当前点:
                            SocketCommandRfid rfidCommand = CommunicationConfigParamManger.Instance.ReadValue(param.CoordSysName, enCommunicationCommand.SocketCommand) as SocketCommandRfid;
                            if (rfidCommand != null)
                            {
                                double X, Y, Z, Theta;
                                string _x = CommunicationConfigParamManger.Instance.ReadValue(param.CoordSysName, enCommunicationCommand.X).ToString();
                                string _y = CommunicationConfigParamManger.Instance.ReadValue(param.CoordSysName, enCommunicationCommand.Y).ToString();
                                string _z = CommunicationConfigParamManger.Instance.ReadValue(param.CoordSysName, enCommunicationCommand.Z).ToString();
                                string _theta = CommunicationConfigParamManger.Instance.ReadValue(param.CoordSysName, enCommunicationCommand.Theta).ToString();
                                double.TryParse(_x, out X);
                                double.TryParse(_y, out Y);
                                double.TryParse(_z, out Z);
                                double.TryParse(_theta, out Theta);
                                _refWcsPoint = new userWcsPoint(X, Y, Z);
                                param.DieIndex = rfidCommand.DieIndex;  // 在这里赋值
                                targetWcsPoint = _refWcsPoint;
                            }
                            else
                                return false;
                            break;
                        case enRefObject.示教点:
                            if (_refWcsPoint != null)
                                targetWcsPoint = _refWcsPoint;
                            else
                                return false;
                            break;
                        default:
                            _refWcsPoint = new userWcsPoint();
                            _refWcsPoint.Grab_x = searchPoints[0].Grab_x;
                            _refWcsPoint.Grab_y = searchPoints[0].Grab_y;
                            _refWcsPoint.CamName = searchPoints[0].CamName;
                            _refWcsPoint.CamParams = searchPoints[0].CamParams;
                            _refWcsPoint.ViewWindow = searchPoints[0].ViewWindow;
                            targetWcsPoint = _refWcsPoint;
                            break;
                    }
                    //for (int i = 0; i < searchPoints.Length; i++)
                    //{
                    //    double dist = 0;//
                    //    dist = Math.Sqrt((searchPoints[i].X - _refWcsPoint.X) * (searchPoints[i].X - _refWcsPoint.X) + (searchPoints[i].Y - _refWcsPoint.Y) * (searchPoints[i].Y - _refWcsPoint.Y));
                    //    //if (refWcsPoint != null)  // 以指定的参考点作为九宫格的中心点
                    //    //    dist = Math.Sqrt((searchPoints[i].X - refWcsPoint.X) * (searchPoints[i].X - refWcsPoint.X) + (searchPoints[i].Y - refWcsPoint.Y) * (searchPoints[i].Y - refWcsPoint.Y));
                    //    //else   // 以当前坐标位置作为九宫格的中心点，需满足相机坐标系原点在视野中心，适用于匹配的点必有中心点
                    //    //    dist = Math.Sqrt((searchPoints[i].X - searchPoints[i].Grab_x) * (searchPoints[i].X - searchPoints[i].Grab_x) + (searchPoints[i].Y - searchPoints[i].Grab_y) * (searchPoints[i].Y - searchPoints[i].Grab_y));
                    //    ///////////////////////////////////////
                    //    if (dist < temPDist)
                    //    {
                    //        temPDist = dist;
                    //        index = i;
                    //    }
                    //}
                    ////////////////////////////////////////// 
                    //userWcsPoint origionPoint = null;
                    double dist1 = double.MaxValue;//Math.Sqrt(param.Dist_X * param.Dist_X + param.Dist_Y * param.Dist_Y);// 
                    double dist2 = double.MaxValue;//param.Dist_Y;//double.MaxValue;
                    double dist3 = double.MaxValue;//Math.Sqrt(param.Dist_X * param.Dist_X + param.Dist_Y * param.Dist_Y);//double.MaxValue;
                    double dist4 = double.MaxValue;//param.Dist_X;//double.MaxValue;
                    double dist5 = double.MaxValue;// 0;//double.MaxValue;
                    double dist6 = double.MaxValue;//param.Dist_X;//double.MaxValue;
                    double dist7 = double.MaxValue;//Math.Sqrt(param.Dist_X * param.Dist_X + param.Dist_Y * param.Dist_Y);//double.MaxValue;
                    double dist8 = double.MaxValue;//param.Dist_Y;//double.MaxValue;
                    double dist9 = double.MaxValue;//Math.Sqrt(param.Dist_X * param.Dist_X + param.Dist_Y * param.Dist_Y);//double.MaxValue;
                    Dictionary<int, userWcsPoint> dic = new Dictionary<int, userWcsPoint>();
                    // 初始化九宫格
                    for (int i = 1; i <= 9; i++)
                    {
                        dic.Add(i, null);
                    }
                    dic[5] = _refWcsPoint;
                    //if (_refWcsPoint != null)
                    //{
                    //    origionPoint = _refWcsPoint;
                    //    dic[5] = origionPoint;
                    //}
                    //else
                    //{
                    //    origionPoint = searchPoints[index];
                    //    dic[5] = origionPoint;
                    //}
                    double minValue = Math.Min(param.Dist_X, param.Dist_Y);
                    double lineAngle = Math.Atan2(param.Dist_Y, param.Dist_X) * 180 / Math.PI;
                    foreach (var item in searchPoints)
                    {
                        double dist = Math.Sqrt((item.X - _refWcsPoint.X) * (item.X - _refWcsPoint.X) + (item.Y - _refWcsPoint.Y) * (item.Y - _refWcsPoint.Y));
                        double phi = Math.Atan2(item.Y - _refWcsPoint.Y, item.X - _refWcsPoint.X);
                        /// 记录第一个点
                        if (Math.Abs((180 - lineAngle) * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180) //125*Math.PI/180 <= phi &&  phi <= 165 * Math.PI / 180
                        {
                            if (minValue * 0.5 < dist && dist <= dist1)
                            {
                                dist1 = dist;
                                dic[1] = item;
                            }
                        }
                        /// 记录第二个点
                        if (Math.Abs(90 * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180) //70 * Math.PI / 180 <= phi && phi <= 110 * Math.PI / 180
                        {
                            if (minValue * 0.5 < dist && dist <= dist2)
                            {
                                dist2 = dist;
                                dic[2] = item;
                            }
                        }
                        /// 记录第三个点
                        if (Math.Abs(lineAngle * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180)
                        {
                            if (minValue * 0.5 < dist && dist <= dist3)
                            {
                                dist3 = dist;
                                dic[3] = item;
                            }
                        }
                        /// 记录第四个点
                        if (Math.Abs(180 * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180 || Math.Abs(-180 * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180)
                        {
                            if (minValue * 0.5 < dist && dist <= dist4)
                            {
                                dist4 = dist;
                                dic[4] = item;
                            }
                        }
                        /// 记录第六个点
                        if (Math.Abs(0 * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180)
                        {
                            if (minValue * 0.5 < dist && dist <= dist6)
                            {
                                dist6 = dist;
                                dic[6] = item;
                            }
                        }
                        /// 记录第七个点
                        if (Math.Abs((-90 - (90 - lineAngle)) * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180)
                        {
                            if (minValue * 0.5 < dist && dist <= dist7)
                            {
                                dist7 = dist;
                                dic[7] = item;
                            }
                        }
                        /// 记录第八个点
                        if (Math.Abs(-90 * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180)
                        {
                            if (minValue * 0.5 < dist && dist <= dist8)
                            {
                                dist8 = dist;
                                dic[8] = item;
                            }
                        }
                        /// 记录第九个点
                        if (Math.Abs(-lineAngle * Math.PI / 180 - phi) <= param.AngleTolerance * Math.PI / 180)
                        {
                            if (minValue * 0.5 < dist && dist <= dist9)
                            {
                                dist9 = dist;
                                dic[9] = item;
                            }
                        }
                    }
                    /////////// 输出匹配点 //////////
                    wcsPoint = new userWcsPoint[dic.Count];
                    dic.Values.CopyTo(wcsPoint, 0);
                    /////////// 输出目标点 //////////
                    if (param.CoordQuadrant == enCoordQuadrant.NONE)
                    {
                        double phi2 = Math.Atan2((param.DieIndex.TargetRowIndex - param.DieIndex.CurRowIndex) * -1, param.DieIndex.TargetColIndex - param.DieIndex.CurColIndex); // 左上角为(0,0)点
                        if (0 < phi2 && phi2 < Math.PI * 0.5)
                            param.CoordQuadrant = enCoordQuadrant.第一象限;
                        if (Math.PI * 0.5 < phi2 && phi2 < Math.PI)
                            param.CoordQuadrant = enCoordQuadrant.第二象限;
                        if (Math.PI * -1 < phi2 && phi2 < Math.PI * -0.5)
                            param.CoordQuadrant = enCoordQuadrant.第三象限;
                        if (Math.PI * -0.5 < phi2 && phi2 < 0)
                            param.CoordQuadrant = enCoordQuadrant.第四象限;
                    }
                    //////////////////////////////////////////////////
                    //double value = Math.Round(10.51);
                    switch (param.CoordQuadrant)
                    {
                        case enCoordQuadrant.第一象限:
                            if (dic[3] != null)
                            {
                                targetWcsPoint = dic[3];
                                ////////////////////  计算行间隔与列间隔  //////////////////////
                                double phi = Math.Atan2(dic[3].Y - _refWcsPoint.Y, dic[3].X - _refWcsPoint.X);
                                double dist = Math.Sqrt((dic[3].X - _refWcsPoint.X) * (dic[3].X - _refWcsPoint.X) + (dic[3].Y - _refWcsPoint.Y) * (dic[3].Y - _refWcsPoint.Y));
                                double dist_x = Math.Cos(phi) * dist;
                                double dist_y = Math.Sin(phi) * dist;
                                int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                ////////// 赋值行列坐标 ///////
                                for (int i = rowStrid; i >= 0; i--)
                                {
                                    if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                    {
                                        param.DieIndex.CurRowIndex -= rowStrid;
                                        //targetWcsPoint.Y += param.Dist_Y * rowStrid;
                                        break;
                                    }
                                }
                                for (int i = colStrid; i >= 0; i--)
                                {
                                    if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                    {
                                        param.DieIndex.CurColIndex += colStrid;
                                        //targetWcsPoint.X += param.Dist_X * colStrid;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (dic[6] != null)
                                {
                                    targetWcsPoint = dic[6];
                                    ////////////////////  计算行间隔与列间隔  //////////////////////
                                    double phi = Math.Atan2(dic[6].Y - _refWcsPoint.Y, dic[6].X - _refWcsPoint.X);
                                    double dist = Math.Sqrt((dic[6].X - _refWcsPoint.X) * (dic[6].X - _refWcsPoint.X) + (dic[6].Y - _refWcsPoint.Y) * (dic[6].Y - _refWcsPoint.Y));
                                    double dist_x = Math.Cos(phi) * dist;
                                    double dist_y = Math.Sin(phi) * dist;
                                    int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                    int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                    ////////// 赋值行列坐标 ///////
                                    for (int i = rowStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                        {
                                            param.DieIndex.CurRowIndex -= rowStrid;
                                            //targetWcsPoint.Y += param.Dist_Y * rowStrid;
                                            break;
                                        }
                                    }
                                    for (int i = colStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                        {
                                            param.DieIndex.CurColIndex += colStrid;
                                            //targetWcsPoint.X += param.Dist_X * colStrid;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dic[2] != null)
                                    {
                                        targetWcsPoint = dic[2];
                                        ////////////////////  计算行间隔与列间隔  //////////////////////
                                        double phi = Math.Atan2(dic[2].Y - _refWcsPoint.Y, dic[2].X - _refWcsPoint.X);
                                        double dist = Math.Sqrt((dic[2].X - _refWcsPoint.X) * (dic[2].X - _refWcsPoint.X) + (dic[2].Y - _refWcsPoint.Y) * (dic[2].Y - _refWcsPoint.Y));
                                        double dist_x = Math.Cos(phi) * dist;
                                        double dist_y = Math.Sin(phi) * dist;
                                        int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                        int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                        ////////// 赋值行列坐标 ///////
                                        for (int i = rowStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                            {
                                                param.DieIndex.CurRowIndex -= rowStrid;
                                                //targetWcsPoint.Y += param.Dist_Y * rowStrid;
                                                break;
                                            }
                                        }
                                        for (int i = colStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                            {
                                                param.DieIndex.CurColIndex += colStrid;
                                                //targetWcsPoint.X += param.Dist_X * colStrid;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case enCoordQuadrant.第二象限:
                            if (dic[1] != null)
                            {
                                targetWcsPoint = dic[1];
                                ////////////////////  计算行间隔与列间隔  //////////////////////
                                double phi = Math.Atan2(dic[1].Y - _refWcsPoint.Y, dic[1].X - _refWcsPoint.X);
                                double dist = Math.Sqrt((dic[1].X - _refWcsPoint.X) * (dic[1].X - _refWcsPoint.X) + (dic[1].Y - _refWcsPoint.Y) * (dic[1].Y - _refWcsPoint.Y));
                                double dist_x = Math.Cos(phi) * dist;
                                double dist_y = Math.Sin(phi) * dist;
                                int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                ////////// 赋值行列坐标 ///////
                                for (int i = rowStrid; i > 0; i--)
                                {
                                    if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                    {
                                        param.DieIndex.CurRowIndex -= rowStrid;
                                        //targetWcsPoint.Y += param.Dist_Y * rowStrid;
                                        break;
                                    }
                                }
                                for (int i = colStrid; i > 0; i--)
                                {
                                    if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                    {
                                        param.DieIndex.CurColIndex += colStrid;
                                        //targetWcsPoint.X -= param.Dist_X * colStrid;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (dic[4] != null)
                                {
                                    targetWcsPoint = dic[4];
                                    ////////////////////  计算行间隔与列间隔  //////////////////////
                                    double phi = Math.Atan2(dic[4].Y - _refWcsPoint.Y, dic[4].X - _refWcsPoint.X);
                                    double dist = Math.Sqrt((dic[4].X - _refWcsPoint.X) * (dic[4].X - _refWcsPoint.X) + (dic[4].Y - _refWcsPoint.Y) * (dic[4].Y - _refWcsPoint.Y));
                                    double dist_x = Math.Cos(phi) * dist;
                                    double dist_y = Math.Sin(phi) * dist;
                                    int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                    int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                    ////////// 赋值行列坐标 ///////
                                    for (int i = rowStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                        {
                                            param.DieIndex.CurRowIndex -= rowStrid;
                                            //targetWcsPoint.Y += param.Dist_Y * rowStrid;
                                            break;
                                        }
                                    }
                                    for (int i = colStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                        {
                                            param.DieIndex.CurColIndex += colStrid;
                                            //targetWcsPoint.X -= param.Dist_X * colStrid;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dic[2] != null)
                                    {
                                        targetWcsPoint = dic[2];
                                        ////////////////////  计算行间隔与列间隔  //////////////////////
                                        double phi = Math.Atan2(dic[2].Y - _refWcsPoint.Y, dic[2].X - _refWcsPoint.X);
                                        double dist = Math.Sqrt((dic[2].X - _refWcsPoint.X) * (dic[2].X - _refWcsPoint.X) + (dic[2].Y - _refWcsPoint.Y) * (dic[2].Y - _refWcsPoint.Y));
                                        double dist_x = Math.Cos(phi) * dist;
                                        double dist_y = Math.Sin(phi) * dist;
                                        int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                        int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                        ////////// 赋值行列坐标 ///////
                                        for (int i = rowStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                            {
                                                param.DieIndex.CurRowIndex -= rowStrid;
                                                //targetWcsPoint.Y += param.Dist_Y * rowStrid;
                                                break;
                                            }
                                        }
                                        for (int i = colStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                            {
                                                param.DieIndex.CurColIndex += colStrid;
                                                //targetWcsPoint.X -= param.Dist_X * colStrid;
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                            break;
                        case enCoordQuadrant.第三象限:
                            if (dic[7] != null)
                            {
                                targetWcsPoint = dic[7];
                                ////////////////////  计算行间隔与列间隔  //////////////////////
                                double phi = Math.Atan2(dic[7].Y - _refWcsPoint.Y, dic[7].X - _refWcsPoint.X);
                                double dist = Math.Sqrt((dic[7].X - _refWcsPoint.X) * (dic[7].X - _refWcsPoint.X) + (dic[7].Y - _refWcsPoint.Y) * (dic[7].Y - _refWcsPoint.Y));
                                double dist_x = Math.Cos(phi) * dist;
                                double dist_y = Math.Sin(phi) * dist;
                                int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                ////////// 赋值行列坐标 ///////
                                for (int i = rowStrid; i > 0; i--)
                                {
                                    if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                    {
                                        param.DieIndex.CurRowIndex -= rowStrid;
                                        //targetWcsPoint.Y -= param.Dist_Y * rowStrid;
                                        break;
                                    }
                                }
                                for (int i = colStrid; i > 0; i--)
                                {
                                    if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                    {
                                        param.DieIndex.CurColIndex += colStrid;
                                        //targetWcsPoint.X -= param.Dist_X * colStrid;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (dic[4] != null)
                                {
                                    targetWcsPoint = dic[4];
                                    ////////////////////  计算行间隔与列间隔  //////////////////////
                                    double phi = Math.Atan2(dic[4].Y - _refWcsPoint.Y, dic[4].X - _refWcsPoint.X);
                                    double dist = Math.Sqrt((dic[4].X - _refWcsPoint.X) * (dic[4].X - _refWcsPoint.X) + (dic[4].Y - _refWcsPoint.Y) * (dic[4].Y - _refWcsPoint.Y));
                                    double dist_x = Math.Cos(phi) * dist;
                                    double dist_y = Math.Sin(phi) * dist;
                                    int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                    int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                    ////////// 赋值行列坐标 ///////
                                    for (int i = rowStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                        {
                                            param.DieIndex.CurRowIndex -= rowStrid;
                                            //targetWcsPoint.Y -= param.Dist_Y * rowStrid;
                                            break;
                                        }
                                    }
                                    for (int i = colStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                        {
                                            param.DieIndex.CurColIndex += colStrid;
                                            //targetWcsPoint.X -= param.Dist_X * colStrid;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dic[8] != null)
                                    {
                                        targetWcsPoint = dic[8];
                                        ////////////////////  计算行间隔与列间隔  //////////////////////
                                        double phi = Math.Atan2(dic[8].Y - _refWcsPoint.Y, dic[8].X - _refWcsPoint.X);
                                        double dist = Math.Sqrt((dic[8].X - _refWcsPoint.X) * (dic[8].X - _refWcsPoint.X) + (dic[8].Y - _refWcsPoint.Y) * (dic[8].Y - _refWcsPoint.Y));
                                        double dist_x = Math.Cos(phi) * dist;
                                        double dist_y = Math.Sin(phi) * dist;
                                        int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                        int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                        ////////// 赋值行列坐标 ///////
                                        for (int i = rowStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                            {
                                                param.DieIndex.CurRowIndex -= rowStrid;
                                                //targetWcsPoint.Y -= param.Dist_Y * rowStrid;
                                                break;
                                            }
                                        }
                                        for (int i = colStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                            {
                                                param.DieIndex.CurColIndex += colStrid;
                                                //targetWcsPoint.X -= param.Dist_X * colStrid;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case enCoordQuadrant.第四象限:
                            if (dic[9] != null)
                            {
                                targetWcsPoint = dic[9];
                                ////////////////////  计算行间隔与列间隔  //////////////////////
                                double phi = Math.Atan2(dic[9].Y - _refWcsPoint.Y, dic[9].X - _refWcsPoint.X);
                                double dist = Math.Sqrt((dic[9].X - _refWcsPoint.X) * (dic[9].X - _refWcsPoint.X) + (dic[9].Y - _refWcsPoint.Y) * (dic[9].Y - _refWcsPoint.Y));
                                double dist_x = Math.Cos(phi) * dist;
                                double dist_y = Math.Sin(phi) * dist;
                                int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                ////////// 赋值行列坐标 ///////
                                for (int i = rowStrid; i > 0; i--)
                                {
                                    if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                    {
                                        param.DieIndex.CurRowIndex -= rowStrid;
                                        //targetWcsPoint.Y -= param.Dist_Y * rowStrid;
                                        break;
                                    }
                                }
                                for (int i = colStrid; i > 0; i--)
                                {
                                    if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                    {
                                        param.DieIndex.CurColIndex += colStrid;
                                        //targetWcsPoint.X += param.Dist_X * colStrid;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (dic[6] != null)
                                {
                                    targetWcsPoint = dic[6];
                                    ////////////////////  计算行间隔与列间隔  //////////////////////
                                    double phi = Math.Atan2(dic[6].Y - _refWcsPoint.Y, dic[6].X - _refWcsPoint.X);
                                    double dist = Math.Sqrt((dic[6].X - _refWcsPoint.X) * (dic[6].X - _refWcsPoint.X) + (dic[6].Y - _refWcsPoint.Y) * (dic[6].Y - _refWcsPoint.Y));
                                    double dist_x = Math.Cos(phi) * dist;
                                    double dist_y = Math.Sin(phi) * dist;
                                    int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                    int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                    ////////// 赋值行列坐标 ///////
                                    for (int i = rowStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                        {
                                            param.DieIndex.CurRowIndex -= rowStrid;
                                            //targetWcsPoint.Y -= param.Dist_Y * rowStrid;
                                            break;
                                        }
                                    }
                                    for (int i = colStrid; i > 0; i--)
                                    {
                                        if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                        {
                                            param.DieIndex.CurColIndex += colStrid;
                                            //targetWcsPoint.X += param.Dist_X * colStrid;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dic[8] != null)
                                    {
                                        targetWcsPoint = dic[8];
                                        ////////////////////  计算行间隔与列间隔  //////////////////////
                                        double phi = Math.Atan2(dic[8].Y - _refWcsPoint.Y, dic[8].X - _refWcsPoint.X);
                                        double dist = Math.Sqrt((dic[8].X - _refWcsPoint.X) * (dic[8].X - _refWcsPoint.X) + (dic[8].Y - _refWcsPoint.Y) * (dic[8].Y - _refWcsPoint.Y));
                                        double dist_x = Math.Cos(phi) * dist;
                                        double dist_y = Math.Sin(phi) * dist;
                                        int rowStrid = (int)Math.Round(dist_y / param.Dist_Y);
                                        int colStrid = (int)Math.Round(dist_x / param.Dist_X);
                                        ////////// 赋值行列坐标 ///////
                                        for (int i = rowStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurRowIndex - rowStrid >= param.DieIndex.TargetRowIndex)
                                            {
                                                param.DieIndex.CurRowIndex -= rowStrid;
                                                //targetWcsPoint.Y -= param.Dist_Y * rowStrid;
                                                break;
                                            }
                                        }
                                        for (int i = colStrid; i > 0; i--)
                                        {
                                            if (param.DieIndex.CurColIndex + colStrid <= param.DieIndex.TargetColIndex)
                                            {
                                                param.DieIndex.CurColIndex += colStrid;
                                                //targetWcsPoint.X += param.Dist_X * colStrid;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    ////// 赋值属性 //////////////////////////////////
                    result = true;
                    break;
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
