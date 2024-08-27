using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Threading;
using Common;
using System.Windows.Forms;
using Common;
using HalconDotNet;


namespace MotionControlCard
{
    [Serializable]
    public class Uwc4000MotionControl : MotionBase
    {
        /// <summary>
        ///判定X轴是否停止,返回1表示停止,返回0表示还在运动
        /// </summary>
        /// <returns></returns>
        private int GetMonitorXStatus()
        {
            int Status = 0, Bit = 0;
            UWC4000Library.uwc4000_get_motion_status(ref Status);
            string s = Convert.ToString(Status, 2);
            Bit = int.Parse(s.Substring(s.Length - 5, 1));  //这里可以直接用逻辑运算符来做位运算判断 
            return Bit;
        }

        /// <summary>
        ///判定Y轴是否停止，返回1表示停止,返回0表示还在运动
        /// </summary>
        /// <returns></returns>
        private int GetMonitorYStatus()
        {
            int Status = 0, Bit = 0;
            UWC4000Library.uwc4000_get_motion_status(ref Status);
            string s = Convert.ToString(Status, 2);
            Bit = int.Parse(s.Substring(s.Length - 6, 1));
            return Bit;
        }

        /// <summary>
        ///判定Z轴是否停止，返回1表示停止,返回0表示还在运动
        /// </summary>
        /// <returns></returns>
        private int GetMonitorZStatus()
        {
            int Status = 0;
            UWC4000Library.uwc4000_get_motion_status(ref Status);
            string s = Convert.ToString(Status, 2);
            int Bit = int.Parse(s.Substring(s.Length - 7, 1));
            return Bit;

        }

        /// <summary>
        ///判定所有轴是否停止，返回1表示停止,返回0表示还在运动
        /// </summary>
        /// <returns></returns>
        private int GetMonitorXYZStatus()
        {
            int Status = 0;
            UWC4000Library.uwc4000_get_motion_status(ref Status);
            string s = Convert.ToString(Status, 2);
            int Bit = int.Parse(s.Substring(s.Length - 1, 1));
            return Bit;
        }

        /// <summary>
        ///判定X轴的正限位状态
        /// </summary>
        /// <returns>0:未触发；1：触发</returns>
        public int XLimitPositiveStatus()
        {
            int[] Status = new int[4];  //声明一个数组
            IntPtr intp = Marshal.AllocHGlobal(128);  //在非托管内存中开辟一个存储区，并返回一个指向该存储区的指针
            Marshal.Copy(Status, 0, intp, 4);   //复制一个数组到该非托管存储区中
            UWC4000Library.uwc4000_get_axis_status(intp);  //获取轴的数据，并存储在该区域中
            Marshal.Copy(intp, Status, 0, 4);   //将非托管存储区的内容复制到托管数组中
            Marshal.FreeHGlobal(intp);          //释放之前开辟的非托管内存区
            string s = Convert.ToString(Status[0], 2);
            //将字符串的位数补齐16位
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 6, 1));
            return Bit;
        }

        /// <summary>
        ///判定X轴的负限位状态
        /// </summary>
        /// <returns>0:未触发；1：触发</returns>
        public int XLimitNegativeStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[0], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 7, 1));
            return Bit;
        }

        /// <summary>
        ///判定Y轴的正限位状态
        /// </summary>
        /// <returns>0:未触发；1：触发</returns>
        public int YLimitPositiveStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[1], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 6, 1));
            return Bit;
        }

        /// <summary>
        ///判定Y轴的负限位状态
        /// </summary>
        /// <returns>0:未触发；1：触发</returns>
        public int YLimitNegativeStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[1], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 7, 1));
            return Bit;
        }

        /// <summary>
        ///判定Z轴的正限位状态
        /// </summary>
        /// <returns>0:未触发；1：触发</returns>
        public int ZLimitPositiveStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[2], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 6, 1));
            return Bit;
        }

        /// <summary>
        ///判定Z轴的负限位状态
        /// </summary>
        /// <returns>0:未触发；1：触发</returns>
        public int ZLimitNegativeStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[2], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 7, 1));
            return Bit;
        }

        /// <summary>
        ///判定X轴的运动状态
        /// </summary>
        /// <returns>0:运动中；1：运动停止</returns>
        private int XmotionStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[0], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 11, 1));
            return Bit;
        }

        /// <summary>
        ///判定Y轴的运动状态
        /// </summary>
        /// <returns>0:运动中；1：运动停止</returns>
        private int YmotionStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[1], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 11, 1));
            return Bit;
        }

        /// <summary>
        ///判定Z轴的运动状态
        /// </summary>
        /// <returns> 0:运动中；1：运动停止</returns>
        private int ZmotionStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[2], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 11, 1));
            return Bit;
        }

        /// <summary>
        ///判定X轴的回原点状态
        /// </summary>
        /// <returns>0:未回原点或失败；1：回原点成功</returns>
        private int XOriginalStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[0], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 12, 1));
            return Bit;
        }

        /// <summary>
        ///判定Y轴的回原点状态
        /// </summary>
        /// <returns>0:未回原点或失败；1：回原点成功</returns>
        private int YOriginalStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[1], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 12, 1));
            return Bit;
        }

        /// <summary>
        ///判定Z轴的回原点状态
        /// </summary>
        /// <returns>0:未回原点或失败；1：回原点成功</returns>
        private int ZOriginalStatus()
        {
            int[] Status = new int[4];
            IntPtr intp = Marshal.AllocHGlobal(128);
            Marshal.Copy(Status, 0, intp, 4);
            UWC4000Library.uwc4000_get_axis_status(intp);
            Marshal.Copy(intp, Status, 0, 4);
            Marshal.FreeHGlobal(intp);
            string s = Convert.ToString(Status[2], 2);
            int L = 16 - s.Length;
            if (s.Length < 16)
            {
                for (int i = 0; i < L; i++)
                {
                    s = s.Insert(0, "0");
                }
            }
            int Bit = int.Parse(s.Substring(s.Length - 12, 1));
            return Bit;
        }


        /// <summary>
        /// 移动X轴
        /// </summary>
        /// <param name="Xposition"></param>
        /// <param name="ScanSpeed"></param>
        private void MoveX(double Xposition, double ScanSpeed)
        {
            // double[] Target = new double[4];
            UWC4000Library.uwc4000_single_move_to(0, ScanSpeed, Xposition);
            //判定XY轴是否停止
            while (true)
            {
                Thread.Sleep(50);
                int BitX = GetMonitorXStatus();
                if (BitX == 1) break;
            }
        }

        /// <summary>
        /// 移动Y轴
        /// </summary>
        /// <param name="Yposition"></param>
        /// <param name="ScanSpeed"></param>
        private void MoveY(double Yposition, double ScanSpeed)
        {
            // double[] Target = new double[4];
            UWC4000Library.uwc4000_single_move_to(1, ScanSpeed, Yposition);
            //判定XY轴是否停止
            while (true)
            {
                Thread.Sleep(50);
                int BitY = GetMonitorYStatus();
                if (BitY == 1) break;
            }
        }

        /// <summary>
        /// 移动Z轴
        /// </summary>
        /// <param name="Zposition"></param>
        /// <param name="ScanSpeed"></param>
        private void MoveZ(double Zposition, double ScanSpeed)
        {
            UWC4000Library.uwc4000_single_move_to(2, ScanSpeed, Zposition); // 非阻塞执行
            //判定XY轴是否停止
            while (true)
            {
                Thread.Sleep(50);
                int BitY = GetMonitorZStatus();
                if (BitY == 1) break;
            }
        }

        /// <summary>
        /// 移动XY轴
        /// </summary>
        /// <param name="Xposition"></param>
        /// <param name="Yposition"></param>
        /// <param name="ScanSpeed"></param>
        private void MoveXY(double Xposition, double Yposition, double ScanSpeed)
        {
            double[] Target = new double[4];
            Target[0] = Xposition;
            Target[1] = Yposition;
            int aa = UWC4000Library.uwc4000_XY_move_to(ScanSpeed, Target);
            //UWC4000Library.uwc4000_single_move_to(0, ScanSpeed, Target[0]);
            //UWC4000Library.uwc4000_single_move_to(1, ScanSpeed, Target[1]);
            //判定XY轴是否停止
            while (true)
            {
                Thread.Sleep(50);
                int BitX = GetMonitorXStatus();
                int BitY = GetMonitorYStatus();
                if (BitX == 1 && BitY == 1) break;
            }
        }
        /// <summary>
        /// 移动XY轴
        /// </summary>
        /// <param name="Xposition"></param>
        /// <param name="Yposition"></param>
        /// <param name="ScanSpeed"></param>
        private void CircleMoveXY(double center_position_x, double center_position_y, double radius, double ScanSpeed)
        {
            int a = UWC4000Library.uwc4000_arc_move_xy(radius, center_position_y, ScanSpeed, 360);
            //判定XY轴是否停止
            while (true)
            {
                Thread.Sleep(50);
                int BitX = GetMonitorXStatus();
                int BitY = GetMonitorYStatus();
                if (BitX == 1 && BitY == 1) break;
            }
        }


        /// <summary>
        /// 移动XYZ轴
        /// </summary>
        /// <param name="Xposition"></param>
        /// <param name="Yposition"></param>
        /// <param name="ScanSpeed"></param>
        private void MoveXYZ(double Xposition, double Yposition, double Zposition, double ScanSpeed)
        {

            double[] Target = new double[4];
            Target[0] = Xposition;
            Target[1] = Yposition;
            Target[2] = Zposition;
            UWC4000Library.uwc4000_XYZ_move_to(ScanSpeed, Target);
            //判定XY轴是否停止
            while (true)
            {
                Thread.Sleep(50);
                int BitX = GetMonitorXStatus();
                int BitY = GetMonitorYStatus();
                int BitZ = GetMonitorZStatus();
                if (BitX == 1 && BitY == 1 && BitZ == 1) break;
            }

        }



        #region  实现接口的操作

        public override bool Init(DeviceConnectConfigParam name)
        {
            bool result = false;
            this.Name = name.DeviceName;
            try
            {
                //this.InitParam(name);
                if (UWC4000Library.uwc4000_initial() == 0)
                {
                    result = true;
                    //this.connectState = true;
                    //this.cardType = enDeviceType.UWC4000;
                }
            }
            catch
            {
                //this.connectState = false;
                return result;
            }
            name.ConnectState = result;
            return result;
        }

        /// <summary>
        /// 单轴移动
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="axisPosition"></param>
        /// <param name="speed"></param>
        public override void MoveSingleAxis(enCoordSysName CoordSysName,enAxisName axisName, double speed, double axisPosition)
        {
            try
            {
                switch (axisName)
                {
                    case enAxisName.X轴:
                        MoveX(axisPosition, speed);
                        //OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                        break;
                    case enAxisName.Y轴:
                        MoveY(axisPosition, speed);
                        //OnPoseChanged(new PoseInfoEventArgs(0, axisPosition, 0, 0, 0, 0));
                        break;
                    case enAxisName.Z轴:
                        MoveZ(axisPosition, speed);
                        //OnPoseChanged(new PoseInfoEventArgs(0, 0, axisPosition, 0, 0, 0));
                        break;
                    case enAxisName.U轴:

                        break;
                    case enAxisName.V轴:

                        break;
                    case enAxisName.W轴:

                        break;
                    default:
                        break;

                }
            }
            catch
            {

            }


        }

        /// <summary>
        /// 多轴移动，XY轴；XYZ轴
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="axisPosition"></param>
        /// <param name="speed"></param>
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            double currentPosition_z;
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    MoveX(axisPosition.X, speed);
                    //OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                    break;
                case enAxisName.Y轴:
                    MoveY(axisPosition.Y, speed);
                    //OnPoseChanged(new PoseInfoEventArgs(0, axisPosition, 0, 0, 0, 0));
                    break;
                case enAxisName.Z轴:
                    MoveZ(axisPosition.Z, speed);
                    //OnPoseChanged(new PoseInfoEventArgs(0, 0, axisPosition, 0, 0, 0));
                    break;
                case enAxisName.XY轴直线插补:
                case enAxisName.XY轴:
                    MoveXY(axisPosition.X, axisPosition.Y, speed);
                    break;
                case enAxisName.XYZ轴:
                case enAxisName.XYZU轴:
                case enAxisName.XYZUV轴:
                    GetAxisPosition(CoordSysName,enAxisName.Z轴, out currentPosition_z);
                    if (Math.Abs(axisPosition.Z - currentPosition_z) < 0.01) // 当目标Z位置与当前Z位置变化很小时，不移动Z轴
                        MoveXY(axisPosition.X, axisPosition.Y, speed);
                    else
                    {
                        //MoveZ(currentPosition_z + GlobalVariable.pConfig.LiftUp_Z, GlobalVariable.pConfig.MoveSpeed); // Z轴先抬升，平移XY再下降
                        if (axisPosition.Z > currentPosition_z)
                        {
                            MoveZ(axisPosition.Z, speed);
                            MoveXY(axisPosition.X, axisPosition.Y, speed);
                        }
                        else
                        {
                            MoveXY(axisPosition.X, axisPosition.Y, speed);
                            MoveZ(axisPosition.Z, speed);
                        }
                    }
                    break;
            }
        }

        public override void MoveMultyAxis(MoveCommandParam motionCommandParam)
        {
            double currentPosition_z;
            //if (!this.connectState) return;
            switch (motionCommandParam.MoveAxis)
            {
                case enAxisName.X轴:
                    MoveX(motionCommandParam.AxisParam.X, motionCommandParam.MoveSpeed);
                    //OnPoseChanged(new PoseInfoEventArgs(axisPosition, 0, 0, 0, 0, 0));
                    break;
                case enAxisName.Y轴:
                    MoveY(motionCommandParam.AxisParam.X, motionCommandParam.MoveSpeed);
                    //OnPoseChanged(new PoseInfoEventArgs(0, axisPosition, 0, 0, 0, 0));
                    break;
                case enAxisName.Z轴:
                    MoveZ(motionCommandParam.AxisParam.X, motionCommandParam.MoveSpeed);
                    //OnPoseChanged(new PoseInfoEventArgs(0, 0, axisPosition, 0, 0, 0));
                    break;
                case enAxisName.XY轴直线插补:
                case enAxisName.XY轴:
                    MoveXY(motionCommandParam.AxisParam.X, motionCommandParam.AxisParam.Y, motionCommandParam.MoveSpeed);
                    break;
                case enAxisName.XYZ轴:
                case enAxisName.XYZU轴:
                case enAxisName.XYZUV轴:
                    GetAxisPosition(motionCommandParam.CoordSysName,enAxisName.Z轴,  out currentPosition_z);
                    if (Math.Abs(motionCommandParam.AxisParam.Z - currentPosition_z) < 0.01) // 当目标Z位置与当前Z位置变化很小时，不移动Z轴
                        MoveXY(motionCommandParam.AxisParam.X, motionCommandParam.AxisParam.Y, motionCommandParam.MoveSpeed);
                    else
                    {
                        //MoveZ(currentPosition_z + GlobalVariable.pConfig.LiftUp_Z, GlobalVariable.pConfig.MoveSpeed); // Z轴先抬升，平移XY再下降
                        if (motionCommandParam.AxisParam.Z > currentPosition_z)
                        {
                            MoveZ(motionCommandParam.AxisParam.Z, motionCommandParam.MoveSpeed);
                            MoveXY(motionCommandParam.AxisParam.X, motionCommandParam.AxisParam.Y, motionCommandParam.MoveSpeed);
                        }
                        else
                        {
                            MoveXY(motionCommandParam.AxisParam.X, motionCommandParam.AxisParam.Y, motionCommandParam.MoveSpeed);
                            MoveZ(motionCommandParam.AxisParam.Z, motionCommandParam.MoveSpeed);
                        }
                    }
                    break;
                case enAxisName.XY轴圆弧插补:
                    CircleMoveXY(motionCommandParam.CircleParam.centerPosition[0], motionCommandParam.CircleParam.centerPosition[1], motionCommandParam.CircleParam.Radius, motionCommandParam.MoveSpeed);
                    break;
            }
        }

        /// <summary>
        /// 执行单轴回零
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="homSpeed"></param>
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    UWC4000Library.uwc4000_go_home(0, homSpeed);
                    break;
                case enAxisName.Y轴:
                    UWC4000Library.uwc4000_go_home(1, homSpeed);
                    break;
                case enAxisName.Z轴:
                    UWC4000Library.uwc4000_go_home(2, homSpeed);
                    break;
                case enAxisName.U轴:
                    UWC4000Library.uwc4000_go_home(3, homSpeed);
                    break;
                case enAxisName.V轴:

                    break;
                case enAxisName.W轴:

                    break;
                case enAxisName.XYZ轴:
                    UWC4000Library.uwc4000_go_home(0, homSpeed);
                    UWC4000Library.uwc4000_go_home(1, homSpeed);
                    UWC4000Library.uwc4000_go_home(2, homSpeed);
                    break;
                case enAxisName.XYZU轴:
                    UWC4000Library.uwc4000_go_home(0, homSpeed);
                    UWC4000Library.uwc4000_go_home(1, homSpeed);
                    UWC4000Library.uwc4000_go_home(2, homSpeed);
                    UWC4000Library.uwc4000_go_home(3, homSpeed);
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 执行三轴或四轴回零
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="homSpeed"></param>
        public override void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    UWC4000Library.uwc4000_go_home(0, homSpeed);
                    break;
                case enAxisName.Y轴:
                    UWC4000Library.uwc4000_go_home(1, homSpeed);
                    break;
                case enAxisName.Z轴:
                    UWC4000Library.uwc4000_go_home(2, homSpeed);
                    break;
                case enAxisName.U轴:
                    UWC4000Library.uwc4000_go_home(3, homSpeed);
                    break;
                case enAxisName.V轴:

                    break;
                case enAxisName.W轴:

                    break;
                case enAxisName.XY轴:
                    UWC4000Library.uwc4000_go_home(0, homSpeed);
                    UWC4000Library.uwc4000_go_home(1, homSpeed);
                   // UWC4000Library.uwc4000_go_home(2, homSpeed);
                    break;
                case enAxisName.XYZ轴:
                    UWC4000Library.uwc4000_go_home(0, homSpeed);
                    UWC4000Library.uwc4000_go_home(1, homSpeed);
                    UWC4000Library.uwc4000_go_home(2, homSpeed);
                    break;
                case enAxisName.XYZU轴:
                    UWC4000Library.uwc4000_go_home(0, homSpeed);
                    UWC4000Library.uwc4000_go_home(1, homSpeed);
                    UWC4000Library.uwc4000_go_home(2, homSpeed);
                    UWC4000Library.uwc4000_go_home(3, homSpeed);
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 获取单轴的当前位置
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="position"></param>
        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName,out double position)
        {
            position = 0;
            double[] X_Scale = new double[4];
            double[] Y_Scale = new double[4];
            double[] Z_Scale = new double[4];
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    UWC4000Library.uwc4000_get_scale(X_Scale, Y_Scale, Z_Scale);// 这里获取的每次都是三个轴的位置吗？
                    position = X_Scale[0];
                    MirrorAxisCoord(CoordSysName, axisName, X_Scale[0],out position);
                    break;
                case enAxisName.Y轴:
                    UWC4000Library.uwc4000_get_scale(X_Scale, Y_Scale, Z_Scale);
                    position = Y_Scale[0];
                    MirrorAxisCoord(CoordSysName, axisName, Y_Scale[0], out position);
                    break;
                case enAxisName.Z轴:
                    UWC4000Library.uwc4000_get_scale(X_Scale, Y_Scale, Z_Scale);
                    position = Z_Scale[0];
                    MirrorAxisCoord(CoordSysName, axisName, Z_Scale[0], out position);
                    break;
                case enAxisName.U轴:
                    position = 0;
                    break;
                case enAxisName.V轴:
                    position = 0;
                    break;
                case enAxisName.W轴:
                    position = 0;
                    break;
                default:
                    break;

            }

        }

        /// <summary>
        /// Jog移动轴
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed">速度为正，往正方向移动，速度为负，往负方向移动</param>
        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            double[] axisPosition = new double[6];
            double[] Max_Speed = new double[4];
            double[] Acc = new double[4];
           // if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    UWC4000Library.uwc4000_get_profile(Acc, Max_Speed);
                    UWC4000Library.uwc4000_jog_start(0, Max_Speed[2]);
                    UWC4000Library.uwc4000_change_speed(0, speed);
                    ////////////////////////////////////////
                    break;
                case enAxisName.Y轴:
                    UWC4000Library.uwc4000_get_profile(Acc, Max_Speed);
                    UWC4000Library.uwc4000_jog_start(1, Max_Speed[2]);
                    UWC4000Library.uwc4000_change_speed(1, speed);
                    /////////////////////////////////////////////////////
                    break;
                case enAxisName.Z轴:
                    UWC4000Library.uwc4000_get_profile(Acc, Max_Speed);
                    UWC4000Library.uwc4000_jog_start(2, Max_Speed[2]);
                    UWC4000Library.uwc4000_change_speed(2, speed);
                    //////////////////////////////////////////////
                    break;
                case enAxisName.U轴:

                    break;
                case enAxisName.V轴:

                    break;
                case enAxisName.W轴:

                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 停止Jog移动
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed"></param>
        public override void JogAxisStop()
        {
            //if (!this.connectState) return;
            UWC4000Library.uwc4000_jog_stop();
        }

        /// <summary>
        /// 减速停止
        /// </summary>
        public override void SlowDownStopAxis()
        {
            //if (!this.connectState) return;
            UWC4000Library.uwc4000_stop(0, 0);
            UWC4000Library.uwc4000_stop(1, 0);
            UWC4000Library.uwc4000_stop(2, 0);
        }

        /// <summary>
        /// 立即停止 
        /// </summary>
        public override void EmgStopAxis()
        {
            //if (!this.connectState) return;
            UWC4000Library.uwc4000_stop(0, 1);
            UWC4000Library.uwc4000_stop(1, 1);
            UWC4000Library.uwc4000_stop(2, 1);
        }
        public override void UnInit()
        {
            // throw new NotImplementedException();
        }
        public override void SetIoOutputBit(object IoType, int IoNum, bool state)
        {
            enIoOutputMode ioType = enIoOutputMode.NONE;
            if (Enum.TryParse<enIoOutputMode>(IoType.ToString(), out ioType))
            { }
            else
                ioType = enIoOutputMode.NONE; // 
            uint un_output = 0xff; // 使用者6进制赋值
            switch (ioType)
            {
                case enIoOutputMode.双脉冲输出:

                    break;
                case enIoOutputMode.脉冲输出:
                    if (state)  //根据输出口状态要求，修改全局变量中对应 bit 的值 && un_output==255
                        un_output &= ~(uint)(0x01 << (IoNum));
                    else
                        un_output |= (uint)0x01 << (IoNum);// 使用者6进制赋值，255关闭所有端口                      
                    UWC4000Library.uwc4000_set_output(un_output); //将设置好的输出口状态更新
                    break;
                case enIoOutputMode.高电平输出:
                    if (state) //根据输出口状态要求，修改全局变量中对应 bit 的值
                        un_output &= ~(uint)(0x01 << (IoNum));
                    else
                        un_output |= (uint)0x01 << (IoNum);
                    UWC4000Library.uwc4000_set_output(un_output); //将设置好的输出口状态更新
                    break;
                case enIoOutputMode.低电平输出:
                    if (state)  //根据输出口状态要求，修改全局变量中对应 bit 的值 && un_output == 255
                        un_output &= ~(uint)(0x01 << (IoNum));
                    else
                        un_output = 0xff;// 使用者6进制赋值，255关闭所有端口                      
                    UWC4000Library.uwc4000_set_output(un_output); //将设置好的输出口状态更新
                    break;
                case enIoOutputMode.线性比较IO输出:

                    break;
                case enIoOutputMode.NONE:
                default:
                    break;
            }
        }
        public override void GetIoOutputBit(object param, int IoNum, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
        }
        public override void SetIoIntputBit(object param, int IoNum, bool state)
        {
            //throw new NotImplementedException();
        }
        public override void GetIoIntputBit(object param, int IoNum, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
        }
        public override void SetParam(enParamType paramType, params object[] paramValue)
        {
            //throw new NotImplementedException();
        }
        public override object GetParam(enParamType paramType, params object[] paramValue)
        {
            return null;
            //throw new NotImplementedException();
        }
        public override void WriteIoOutputBit(enIoPortType ioPortType, int IoPort, params object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoOutputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoIntputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override void WriteIoOutputGroup(enIoPortType ioPortType, int IoPort, params object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoOutputGroup(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override void ReadIoIntputGroup(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            throw new NullReferenceException();
        }
        public override object ReadValue(enDataTypes dataType, string address, int length)
        {
            throw new NotImplementedException();
        }
        public override bool WriteValue(enDataTypes dataType, string address, object value)
        {
            throw new NotImplementedException();
        }



        #endregion


    }
}
