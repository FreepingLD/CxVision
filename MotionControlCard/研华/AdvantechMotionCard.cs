using Advantech.Motion;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class AdvantechMotionCard : MotionBase
    {
        private DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];
        private IntPtr[] m_Axishand = new IntPtr[32];
        private IntPtr m_DeviceHandle = IntPtr.Zero;
        private uint AxesNumPerDev = 0;
        public override bool Init(DeviceConnectConfigParam param)
        {
            // 初始化板卡
            uint DeviceNum = 0, deviceCount = 0;
            int Result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
            if (Result != (int)ErrorCode.SUCCESS)
            {
                return false;
            }
            if (deviceCount > 0)
            {
                DeviceNum = CurAvailableDevs[0].DeviceNum;
            }
            /////////////////////////////////////   打开控制板  ///////////////
            Result = (int)Motion.mAcm_DevOpen(DeviceNum, ref m_DeviceHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                StringBuilder ErrorMsg = new StringBuilder("", 100);
                Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                throw new ArgumentException(ErrorMsg.ToString());
            }
            // 获取板上的轴数
            Result = (int)Motion.mAcm_GetU32Property(m_DeviceHandle, (uint)PropertyID.FT_DevAxesCount, ref AxesNumPerDev);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                StringBuilder ErrorMsg = new StringBuilder("", 100);
                string strTemp = "Get Axis Number Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                throw new ArgumentException(strTemp + ErrorMsg.ToString());
            }
            // 打开轴
            for (int i = 0; i < AxesNumPerDev; i++)
            {
                Result = (int)Motion.mAcm_AxOpen(m_DeviceHandle, (UInt16)i, ref m_Axishand[i]);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    StringBuilder ErrorMsg = new StringBuilder("", 100);
                    string strTemp = "Open Axis Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                    Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                    throw new ArgumentException(strTemp + ErrorMsg.ToString());
                }
                double cmdPosition = new double();
                cmdPosition = 0;
                //Set command position for the specified axis
                Motion.mAcm_AxSetCmdPosition(m_Axishand[i], cmdPosition);
                //Set actual position for the specified axis
                Motion.mAcm_AxSetActualPosition(m_Axishand[i], cmdPosition);
            }
            return true;
        }

        public override void UnInit()
        {
            try
            {
                UInt16[] usAxisState = new UInt16[32];
                uint AxisNum;
                //Stop Every Axes
                for (AxisNum = 0; AxisNum < this.AxesNumPerDev; AxisNum++)
                {
                    //Get the axis's current state
                    Motion.mAcm_AxGetState(m_Axishand[AxisNum], ref usAxisState[AxisNum]);
                    if (usAxisState[AxisNum] == (uint)AxisState.STA_AX_ERROR_STOP)
                    {
                        // Reset the axis' state. If the axis is in ErrorStop state, the state will be changed to Ready after calling this function
                        Motion.mAcm_AxResetError(m_Axishand[AxisNum]);
                    }
                    //To command axis to decelerate to stop.
                    Motion.mAcm_AxStopDec(m_Axishand[AxisNum]);
                }
                //Close Axes
                for (AxisNum = 0; AxisNum < AxesNumPerDev; AxisNum++)
                {
                    Motion.mAcm_AxClose(ref m_Axishand[AxisNum]);
                }
                AxesNumPerDev = 0; // 每个板卡上的轴数量
                //Close Device
                Motion.mAcm_DevClose(ref m_DeviceHandle);
                m_DeviceHandle = IntPtr.Zero;
            }
            catch
            {

            }
        }

        public override void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            throw new NotImplementedException();
        }

        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            throw new NotImplementedException();
        }

        public override void MoveMultyAxis(MoveCommandParam motionCommandParam)
        {
            if (this.m_DeviceHandle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0"); // 
            double x, y, z;
            uint Result = 0;
            switch (motionCommandParam.MoveAxis)
            {
                case enAxisName.X轴:
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.X轴, motionCommandParam.AxisParam.X, out x);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[0], (uint)PropertyID.PAR_AxVelHigh, motionCommandParam.MoveSpeed * 1000);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[0], x);
                    /////////////////////////////////////
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "PTP Move Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.Y轴:
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Y轴, motionCommandParam.AxisParam.Y, out y);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[1], (uint)PropertyID.PAR_AxVelHigh, motionCommandParam.MoveSpeed * 1000);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[1], y);
                    /////////////////////////////////////
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "PTP Move Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.Z轴:
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Z轴, motionCommandParam.AxisParam.Z, out z);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[2], (uint)PropertyID.PAR_AxVelHigh, motionCommandParam.MoveSpeed * 1000);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[2], z);
                    /////////////////////////////////////
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "PTP Move Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.XY轴:
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.X轴, motionCommandParam.AxisParam.X, out x);
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Y轴, motionCommandParam.AxisParam.Y, out y);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[0], x);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[1], y);
                    /////////////////////////////////////
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "PTP Move Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.XYZ轴:
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.X轴, motionCommandParam.AxisParam.X, out x);
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Y轴, motionCommandParam.AxisParam.Y, out y);
                    MirrorAxisCoord(motionCommandParam.CoordSysName, enAxisName.Z轴, motionCommandParam.AxisParam.Z, out z);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[0], x);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[1], y);
                    Result = Motion.mAcm_AxMoveAbs(m_Axishand[2], z);
                    /////////////////////////////////////
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "PTP Move Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
            }
        }

        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            if (this.m_DeviceHandle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            uint Result = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    Result = Motion.mAcm_SetU32Property(m_Axishand[0], (uint)PropertyID.PAR_AxHomeExSwitchMode, (uint)enSwitchMode.EdgeOn);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Set Property-PAR_AxHomeExSwitchMode Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_SetF64Property(m_Axishand[0], (uint)PropertyID.PAR_AxHomeCrossDistance, 10000);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Set Property-AxHomeCrossDistance Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_SetF64Property(m_Axishand[0], (uint)PropertyID.PAR_AxVelHigh, homSpeed * 1000);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_AxHome(m_Axishand[0], (UInt32)enHomMode.MODE1_Abs, (UInt32)enHomDir.NegativeDirection);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "AxHome Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.Y轴:
                    Result = Motion.mAcm_SetU32Property(m_Axishand[1], (uint)PropertyID.PAR_AxHomeExSwitchMode, (uint)enSwitchMode.EdgeOn);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Set Property-PAR_AxHomeExSwitchMode Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_SetF64Property(m_Axishand[1], (uint)PropertyID.PAR_AxHomeCrossDistance, 10000);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Set Property-AxHomeCrossDistance Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_SetF64Property(m_Axishand[1], (uint)PropertyID.PAR_AxVelHigh, homSpeed * 1000);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_AxHome(m_Axishand[1], (UInt32)enHomMode.MODE1_Abs, (UInt32)enHomDir.NegativeDirection);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "AxHome Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.Z轴:
                    Result = Motion.mAcm_SetU32Property(m_Axishand[2], (uint)PropertyID.PAR_AxHomeExSwitchMode, (uint)enSwitchMode.EdgeOn);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Set Property-PAR_AxHomeExSwitchMode Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_SetF64Property(m_Axishand[2], (uint)PropertyID.PAR_AxHomeCrossDistance, 10000);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Set Property-AxHomeCrossDistance Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_SetF64Property(m_Axishand[2], (uint)PropertyID.PAR_AxVelHigh, homSpeed * 1000);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    Result = Motion.mAcm_AxHome(m_Axishand[2], (UInt32)enHomMode.MODE1_Abs, (UInt32)enHomDir.NegativeDirection);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "AxHome Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
            }
        }

        public override void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            throw new NotImplementedException();
        }

        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            if (this.m_DeviceHandle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            double pos = 0;
            position = 0;
            uint Result = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    Result = Motion.mAcm_AxGetActualPosition(m_Axishand[0], ref pos);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Get Axis Position Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    MirrorAxisCoord(CoordSysName, axisName, pos, out position);
                    break;
                case enAxisName.Y轴:
                    Result = Motion.mAcm_AxGetActualPosition(m_Axishand[1], ref pos);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Get Axis Position Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    MirrorAxisCoord(CoordSysName, axisName, pos, out position);
                    break;
                case enAxisName.Z轴:
                    Result = Motion.mAcm_AxGetActualPosition(m_Axishand[2], ref pos);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Get Axis Position Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    MirrorAxisCoord(CoordSysName, axisName, pos, out position);
                    break;
                case enAxisName.U轴:
                    Result = Motion.mAcm_AxGetActualPosition(m_Axishand[3], ref pos);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Get Axis Position Failed With Error Code: [0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    MirrorAxisCoord(CoordSysName, axisName, pos, out position);
                    break;
            }
        }

        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            if (this.m_DeviceHandle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            double moveSpeed;
            uint Result = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    MirrorAxisJog(CoordSysName, axisName, speed, out moveSpeed);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[0], (uint)PropertyID.PAR_AxJerk, 0);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[0], (uint)PropertyID.PAR_AxAcc, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[0], (uint)PropertyID.PAR_AxDec, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[0], (uint)PropertyID.PAR_AxVelHigh, moveSpeed * 1000);
                    if (moveSpeed > 0)
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[0], 0);
                    else
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[0], 1);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.Y轴:
                    MirrorAxisJog(CoordSysName, axisName, speed, out moveSpeed);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[1], (uint)PropertyID.PAR_AxJerk, 0);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[1], (uint)PropertyID.PAR_AxAcc, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[1], (uint)PropertyID.PAR_AxDec, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[1], (uint)PropertyID.PAR_AxVelHigh, moveSpeed * 1000);
                    if (moveSpeed > 0)
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[1], 0);
                    else
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[1], 1);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.Z轴:
                    MirrorAxisJog(CoordSysName, axisName, speed, out moveSpeed);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[2], (uint)PropertyID.PAR_AxJerk, 0);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[2], (uint)PropertyID.PAR_AxAcc, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[2], (uint)PropertyID.PAR_AxDec, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[2], (uint)PropertyID.PAR_AxVelHigh, moveSpeed * 1000);
                    if (moveSpeed > 0)
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[2], 0);
                    else
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[2], 1);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
                case enAxisName.U轴:
                    MirrorAxisJog(CoordSysName, axisName, speed, out moveSpeed);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[30], (uint)PropertyID.PAR_AxJerk, 0);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[3], (uint)PropertyID.PAR_AxAcc, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[3], (uint)PropertyID.PAR_AxDec, 10000);
                    Result = Motion.mAcm_SetF64Property(m_Axishand[3], (uint)PropertyID.PAR_AxVelHigh, moveSpeed * 1000);
                    if (moveSpeed > 0)
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[3], 0);
                    else
                        Result = Motion.mAcm_AxMoveVel(m_Axishand[3], 1);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        StringBuilder ErrorMsg = new StringBuilder("", 100);
                        string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                        throw new ArgumentException(strTemp + ErrorMsg.ToString());
                    }
                    break;
            }
        }

        public override void JogAxisStop()
        {
            if (this.m_DeviceHandle.ToInt64() == 0) throw new ArgumentException("控制器未打开，对应句柄值为0");
            uint Result = 0;
            for (int i = 0; i < this.AxesNumPerDev; i++)
            {
                Result = Motion.mAcm_AxStopDec(m_Axishand[i]);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    StringBuilder ErrorMsg = new StringBuilder("", 100);
                    string strTemp = "Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                    Boolean res = Motion.mAcm_GetErrorMessage((uint)Result, ErrorMsg, 100);
                    throw new ArgumentException(strTemp + ErrorMsg.ToString());
                }
            }


        }

        public override void SlowDownStopAxis()
        {
            throw new NotImplementedException();
        }

        public override void EmgStopAxis()
        {
            throw new NotImplementedException();
        }
    }

    public enum enSwitchMode
    {
        LevelOn,
        LevelOff,
        EdgeOn,
        EdgeOff,
    }
    public enum enHomDir
    {
        PositiveDirection,
        NegativeDirection,
    }
    public enum enHomMode
    {
        MODE1_Abs,
        MODE2_Lmt,
        MODE3_Ref,
        MODE4_Abs_Ref,
        MODE5_Abs_NegRef,
        MODE6_Lmt_Ref,
        MODE7_AbsSearch,
        MODE8_LmtSearch,
        MODE9_AbsSearch_Ref,
        MODE10_AbsSearch_NegRef,
        MODE11_LmtSearch_Ref,
        MODE12_AbsSearchReFind,
        MODE13_LmtSearchReFind,
        MODE14_AbsSearchReFind_Ref,
        MODE15_AbsSearchReFind_NegRef,
        MODE16_LmtSearchReFind_Ref,
    }
}
