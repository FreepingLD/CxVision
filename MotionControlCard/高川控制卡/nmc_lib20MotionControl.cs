
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotionControlCard
{
    [Serializable]
    public class nmc_lib20MotionControl : MotionBase, IMotionControl
    {
        //short devNo = 0;
        //short axisIndex = 0;
        private ushort BoardHandle; //设备句柄
        private ushort axisXHandle; //轴句柄
        private ushort axisYHandle; //轴句柄
        private ushort axisZHandle; //轴句柄
        private ushort axisUHandle; //轴句柄
        /// ////////////////////////////////////
        private CNMCLib20.TPtpPara ptpPrm = new CNMCLib20.TPtpPara(); // 设置轴运动点到点参数
        private CNMCLib20.TSafePara safePrm = new CNMCLib20.TSafePara(); // 设置轴运动安全参数
        private CNMCLib20.THomeSetting homeSetup = new CNMCLib20.THomeSetting(); //轴回零配置参数
        private CNMCLib20.TAxisStsPack stsPack = new CNMCLib20.TAxisStsPack();
        private int pulseFactor_x = 1366;
        private int pulseFactor_y = 1000;
        private int pulseFactor_z = 1000;
        private CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// 初始化控制器
        /// </summary>
        /// <returns></returns>
        private bool InitBoard()
        {
            short devNo = 0;
            short rtn = -1;
            // 创建板卡句柄
            try
            {
                if (BoardHandle > 0) return true;
                rtn = CNMCLib20.NMC_DevOpen(devNo, ref BoardHandle);
            }
            catch
            {

            }
            if (rtn != 0)
            {
                // MessageBox.Show("打开板卡失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 打开轴
        /// </summary>
        /// <returns></returns>
        private bool OpenAxis(short AxisNo)
        {
            //所有指令如无特殊说明，返回0零值表示成功
            short rtn = -1;
            switch (AxisNo)
            {
                case 0:
                    rtn = CNMCLib20.NMC_MtOpen(BoardHandle, AxisNo, ref axisXHandle);
                    break;
                case 1:
                    rtn = CNMCLib20.NMC_MtOpen(BoardHandle, AxisNo, ref axisYHandle);
                    break;
                case 2:
                    rtn = CNMCLib20.NMC_MtOpen(BoardHandle, AxisNo, ref axisZHandle);
                    break;
                case 3:
                    rtn = CNMCLib20.NMC_MtOpen(BoardHandle, AxisNo, ref axisUHandle);
                    break;
                default:
                    break;
            }
            if (rtn == 0) return true;
            return false;
        }

        /// <summary>
        /// 关闭轴
        /// </summary>
        /// <returns></returns>
        private bool CloseAxis(short AxisNo)
        {
            short rtn = -1;
            switch (AxisNo)
            {
                case 0:
                    rtn = CNMCLib20.NMC_MtClose(ref axisXHandle);
                    break;
                case 1:
                    rtn = CNMCLib20.NMC_MtClose(ref axisYHandle);
                    break;
                case 2:
                    rtn = CNMCLib20.NMC_MtClose(ref axisZHandle);
                    break;
                case 3:
                    rtn = CNMCLib20.NMC_MtClose(ref axisUHandle);
                    break;
                default:
                    break;
            }
            if (rtn == 0) return true;
            return false;
        }

        /// <summary>
        /// 停止轴运动
        /// </summary>
        /// <returns></returns>
        private bool StopAxis(short AxisNo)
        {
            short rtn = -1;
            switch (AxisNo)
            {
                case 0:
                    rtn = CNMCLib20.NMC_MtStop(axisXHandle);
                    break;
                case 1:
                    rtn = CNMCLib20.NMC_MtStop(axisYHandle);
                    break;
                case 2:
                    rtn = CNMCLib20.NMC_MtStop(axisZHandle);
                    break;
                case 3:
                    rtn = CNMCLib20.NMC_MtStop(axisUHandle);
                    break;
                default:
                    break;
            }
            // rtn = CNMCLib20.NMC_MtStop(axisXHandle);
            if (rtn == 0) return true;
            return false;
        }

        /// <summary>
        /// 配置轴参数
        /// </summary>
        /// <returns></returns>
        private bool AxisXConfig()
        {
            short rtn;
            // 设置允许的位置误差
            rtn = CNMCLib20.NMC_MtSetPosErrLmt(axisXHandle, 30000);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 限位开关触发电平设置为高电平
            rtn = CNMCLib20.NMC_MtLmtSns(axisXHandle, 0, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 启动限位开关检查
            rtn = CNMCLib20.NMC_MtLmtOnOff(axisXHandle, 1, 1);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置伺服报警开关电平为高电平
            rtn = CNMCLib20.NMC_MtAlarmSns(axisXHandle, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 伺服报警检查开启
            rtn = CNMCLib20.NMC_MtAlarmOnOff(axisXHandle, 1);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置轴安全参数
            safePrm.maxAcc = 50;
            safePrm.maxVel = 100;
            safePrm.estpDec = 50;
            rtn = CNMCLib20.NMC_MtSetSafePara(axisXHandle, ref safePrm);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置脉冲输出模式为脉冲+方向
            rtn = CNMCLib20.NMC_MtSetStepMode(axisXHandle, 0, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            return true;
        }
        private bool AxisZConfig()
        {
            short rtn;
            // 设置允许的位置误差
            rtn = CNMCLib20.NMC_MtSetPosErrLmt(axisZHandle, 30000);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 限位开关触发电平设置为高电平
            rtn = CNMCLib20.NMC_MtLmtSns(axisZHandle, 0, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 启动限位开关检查
            rtn = CNMCLib20.NMC_MtLmtOnOff(axisZHandle, 1, 1);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置伺服报警开关电平为高电平
            rtn = CNMCLib20.NMC_MtAlarmSns(axisZHandle, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 伺服报警检查开启
            rtn = CNMCLib20.NMC_MtAlarmOnOff(axisZHandle, 1);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置轴安全参数
            safePrm.maxAcc = 50;
            safePrm.maxVel = 100;
            safePrm.estpDec = 50;
            rtn = CNMCLib20.NMC_MtSetSafePara(axisZHandle, ref safePrm);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置脉冲输出模式为脉冲+方向
            rtn = CNMCLib20.NMC_MtSetStepMode(axisZHandle, 0, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            return true;
        }
        private bool AxisYConfig()
        {
            short rtn;
            // 设置允许的位置误差
            rtn = CNMCLib20.NMC_MtSetPosErrLmt(axisYHandle, 30000);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 限位开关触发电平设置为高电平
            rtn = CNMCLib20.NMC_MtLmtSns(axisYHandle, 0, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 启动限位开关检查
            rtn = CNMCLib20.NMC_MtLmtOnOff(axisYHandle, 1, 1);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置伺服报警开关电平为高电平
            rtn = CNMCLib20.NMC_MtAlarmSns(axisYHandle, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 伺服报警检查开启
            rtn = CNMCLib20.NMC_MtAlarmOnOff(axisYHandle, 1);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置轴安全参数
            safePrm.maxAcc = 50;
            safePrm.maxVel = 100;
            safePrm.estpDec = 50;
            rtn = CNMCLib20.NMC_MtSetSafePara(axisYHandle, ref safePrm);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            // 设置脉冲输出模式为脉冲+方向
            rtn = CNMCLib20.NMC_MtSetStepMode(axisYHandle, 0, 0);
            if (rtn != 0)
            {
                return false;     // 设置出错
            }

            return true;
        }
        private bool AxisConfig(short AxisNo)
        {
            bool rtn = false;
            switch (AxisNo)
            {
                case 0:
                    rtn = AxisXConfig();
                    break;
                case 1:
                    rtn = AxisYConfig();
                    break;
                case 2:
                    rtn = AxisZConfig();
                    break;
                case 3:
                    rtn = AxisZConfig();
                    break;
                default:
                    break;
            }
            return rtn;
        }

        /// <summary>
        /// 轴回零
        /// </summary>
        /// <returns></returns>
        private bool AxisXHome()
        {
            short rtn;
            // 回零参数设置（请根据自身机构特点配置）
            homeSetup.mode = (short)CNMCLib20.THomeMode.HM_MODE1;      // 回零模式
            homeSetup.dir = 0;              // 回零初始方向:负向
            homeSetup.offset = 0;           // 原点偏移
            homeSetup.scan1stVel = 5;       // 基本搜寻速度
            homeSetup.scan2ndVel = 1;       // 低速
            homeSetup.acc = 0.5;            // 加速度
            homeSetup.reScanEn = 1;         // 两次搜寻零点
            homeSetup.homeEdge = 0;         // 原点，触发沿,下降沿
            homeSetup.lmtEdge = 0;          //
            homeSetup.zEdge = 0;            //
            homeSetup.iniRetPos = 0;        // 起始反向运动距离
            homeSetup.retSwOffset = 0;      // 反向运动时离开开关距离
            homeSetup.safeLen = 0;          // 安全距离
            homeSetup.usePreSetPtpPara = 0; //当usePreSetPtpPara=0时，回零运动的//减加速度默认等于acc,起跳速度、终点速度、平滑系数默认为0

            // 设置回零参数
            rtn = CNMCLib20.NMC_MtSetHomePara(axisXHandle, ref homeSetup);
            if (rtn != 0)
            {
                return false;     // 参数配置出错
            }
            // 启动回零
            rtn = CNMCLib20.NMC_MtHome(axisXHandle);
            if (rtn != 0)
            {
                return false;     // 启动出错
            }
            // 回零过程查询
            short homeSts = 0;
            while (true)
            {
                rtn = CNMCLib20.NMC_MtGetHomeSts(axisXHandle, ref homeSts);
                if (rtn != 0)
                {
                    return false;
                }
                if ((homeSts & CNMCLib20.BIT_AXHOME_OK) != 0)
                {
                    break;          // 回零完成
                }
                if (((homeSts & CNMCLib20.BIT_AXHOME_FAIL) != 0)
                    || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_MV) != 0)
                    || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_SWT) != 0))
                {
                    return false;       // 回零过程出现错误
                }
            }
            ////////////////////////////
            return true;
        }
        private bool AxisYHome()
        {
            short rtn;
            // 回零参数设置（请根据自身机构特点配置）
            homeSetup.mode = (short)CNMCLib20.THomeMode.HM_MODE1;      // 回零模式
            homeSetup.dir = 0;              // 回零初始方向:负向
            homeSetup.offset = 0;           // 原点偏移
            homeSetup.scan1stVel = 5;       // 基本搜寻速度
            homeSetup.scan2ndVel = 1;       // 低速
            homeSetup.acc = 0.5;            // 加速度
            homeSetup.reScanEn = 1;         // 两次搜寻零点
            homeSetup.homeEdge = 0;         // 原点，触发沿,下降沿
            homeSetup.lmtEdge = 0;          //
            homeSetup.zEdge = 0;            //
            homeSetup.iniRetPos = 0;        // 起始反向运动距离
            homeSetup.retSwOffset = 0;      // 反向运动时离开开关距离
            homeSetup.safeLen = 0;          // 安全距离
            homeSetup.usePreSetPtpPara = 0; //当usePreSetPtpPara=0时，回零运动的//减加速度默认等于acc,起跳速度、终点速度、平滑系数默认为0

            // 设置回零参数
            rtn = CNMCLib20.NMC_MtSetHomePara(axisYHandle, ref homeSetup);
            if (rtn != 0)
            {
                return false;     // 参数配置出错
            }
            // 启动回零
            rtn = CNMCLib20.NMC_MtHome(axisYHandle);
            if (rtn != 0)
            {
                return false;     // 启动出错
            }
            // 回零过程查询
            short homeSts = 0;
            while (true)
            {
                rtn = CNMCLib20.NMC_MtGetHomeSts(axisYHandle, ref homeSts);
                if (rtn != 0)
                {
                    return false;
                }
                if ((homeSts & CNMCLib20.BIT_AXHOME_OK) != 0)
                {
                    break;          // 回零完成
                }
                if (((homeSts & CNMCLib20.BIT_AXHOME_FAIL) != 0)
                    || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_MV) != 0)
                    || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_SWT) != 0))
                {
                    return false;       // 回零过程出现错误
                }
            }
            ////////////////////////////
            return true;
        }
        private bool AxisZHome()
        {
            short rtn;
            // 回零参数设置（请根据自身机构特点配置）
            homeSetup.mode = (short)CNMCLib20.THomeMode.HM_MODE1;      // 回零模式
            homeSetup.dir = 0;              // 回零初始方向:负向
            homeSetup.offset = 0;           // 原点偏移
            homeSetup.scan1stVel = 5;       // 基本搜寻速度
            homeSetup.scan2ndVel = 1;       // 低速
            homeSetup.acc = 0.5;            // 加速度
            homeSetup.reScanEn = 1;         // 两次搜寻零点
            homeSetup.homeEdge = 0;         // 原点，触发沿,下降沿
            homeSetup.lmtEdge = 0;          //
            homeSetup.zEdge = 0;            //
            homeSetup.iniRetPos = 0;        // 起始反向运动距离
            homeSetup.retSwOffset = 0;      // 反向运动时离开开关距离
            homeSetup.safeLen = 0;          // 安全距离
            homeSetup.usePreSetPtpPara = 0; //当usePreSetPtpPara=0时，回零运动的//减加速度默认等于acc,起跳速度、终点速度、平滑系数默认为0

            // 设置回零参数
            rtn = CNMCLib20.NMC_MtSetHomePara(axisZHandle, ref homeSetup);
            if (rtn != 0)
            {
                return false;     // 参数配置出错
            }
            // 启动回零
            rtn = CNMCLib20.NMC_MtHome(axisZHandle);
            if (rtn != 0)
            {
                return false;     // 启动出错
            }
            // 回零过程查询
            short homeSts = 0;
            while (true)
            {
                rtn = CNMCLib20.NMC_MtGetHomeSts(axisZHandle, ref homeSts);
                if (rtn != 0)
                {
                    return false;
                }
                if ((homeSts & CNMCLib20.BIT_AXHOME_OK) != 0)
                {
                    break;          // 回零完成
                }
                if (((homeSts & CNMCLib20.BIT_AXHOME_FAIL) != 0)
                    || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_MV) != 0)
                    || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_SWT) != 0))
                {
                    return false;       // 回零过程出现错误
                }
            }
            ////////////////////////////
            return true;
        }
        private bool AxisHome(short AxisNo)
        {
            // short rtn=-1;
            switch (AxisNo)
            {
                case 0:
                    AxisXHome();
                    break;
                case 1:
                    AxisYHome();
                    break;
                case 2:
                    AxisZHome();
                    break;
                case 3:
                    AxisXHome();
                    break;
                default:
                    break;
            }
            ////////////////////////////
            return true;
        }

        /// <summary>
        /// 寸动移动   ；怎样实现连续移动？
        /// </summary>
        /// <param name="Speed"></param>
        /// <param name="bDir"></param>
        private bool MoveXJog(double Speed, bool bDir)
        {
            double acc = 0.5;
            double dec = 0.5;
            // 配置运动模式
            short rtn = CNMCLib20.NMC_MtSetPrfMode(axisXHandle, CNMCLib20.MT_JOG_PRF_MODE);
            if (rtn != 0)
            {
                return false;
            }
            // 设置参数
            CNMCLib20.TJogPara jogPrm;
            jogPrm.acc = acc;
            jogPrm.dec = dec;
            jogPrm.smoothCoef = 0;
            rtn = CNMCLib20.NMC_MtSetJogPara(axisXHandle, ref jogPrm);
            if (rtn != 0)
            {
                return false;
            }

            // 设置速度
            if (!bDir) // 这里不起作用
            {
                Speed = -1 * Speed;
            }
            rtn = CNMCLib20.NMC_MtSetVel(axisXHandle, Speed);
            if (rtn != 0)
            {
                return false;
            }

            // 启动运动，这里是更新参数还是启动轴运动？
            rtn = CNMCLib20.NMC_MtUpdate(axisXHandle);
            if (rtn != 0)
            {
                return false;
            }
            return true;
        }
        private bool MoveYJog(double Speed, bool bDir)
        {
            double acc = 0.5;
            double dec = 0.5;
            // 配置运动模式
            short rtn = CNMCLib20.NMC_MtSetPrfMode(axisYHandle, CNMCLib20.MT_JOG_PRF_MODE);
            if (rtn != 0)
            {
                return false;
            }
            // 设置参数
            CNMCLib20.TJogPara jogPrm;
            jogPrm.acc = acc;
            jogPrm.dec = dec;
            jogPrm.smoothCoef = 0;
            rtn = CNMCLib20.NMC_MtSetJogPara(axisYHandle, ref jogPrm);
            if (rtn != 0)
            {
                return false;
            }

            // 设置速度
            if (!bDir)
            {
                Speed = -1 * Speed;
            }
            rtn = CNMCLib20.NMC_MtSetVel(axisYHandle, Speed);
            if (rtn != 0)
            {
                return false;
            }

            // 启动运动，这里是更新参数还是启动轴运动？
            rtn = CNMCLib20.NMC_MtUpdate(axisYHandle);
            if (rtn != 0)
            {
                return false;
            }
            return true;
        }
        private bool MoveZJog(double Speed, bool bDir)
        {
            double acc = 0.5;
            double dec = 0.5;
            // 配置运动模式
            short rtn = CNMCLib20.NMC_MtSetPrfMode(axisZHandle, CNMCLib20.MT_JOG_PRF_MODE);
            if (rtn != 0)
            {
                return false;
            }
            // 设置参数
            CNMCLib20.TJogPara jogPrm;
            jogPrm.acc = acc;
            jogPrm.dec = dec;
            jogPrm.smoothCoef = 0;
            rtn = CNMCLib20.NMC_MtSetJogPara(axisZHandle, ref jogPrm);
            if (rtn != 0)
            {
                return false;
            }

            // 设置速度
            if (!bDir)
            {
                Speed = -1 * Speed;
            }
            rtn = CNMCLib20.NMC_MtSetVel(axisZHandle, Speed);
            if (rtn != 0)
            {
                return false;
            }

            // 启动运动，这里是更新参数还是启动轴运动？
            rtn = CNMCLib20.NMC_MtUpdate(axisZHandle);
            if (rtn != 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取轴的当前位置
        /// </summary>
        /// <param name="axisPos"></param>
        private void GetCurrentAxisXPose(out double axisPos)
        {
            // 读取电机当前的理论位置
            int Pos = 0;
            short rtn = CNMCLib20.NMC_MtGetAxisPos(axisXHandle, ref Pos);
            axisPos = Pos * 1.0 / (pulseFactor_x);
            if (rtn != 0)
            {
                return;     // 出错
            }
        }

        /// <summary>
        /// 获取轴的当前位置
        /// </summary>
        /// <param name="axisPos"></param>
        private void GetCurrentAxisYPose(out double axisPos)
        {
            // 读取电机当前的理论位置
            int Pos = 0;
            short rtn = CNMCLib20.NMC_MtGetAxisPos(axisYHandle, ref Pos);
            axisPos = Pos / pulseFactor_x;
            if (rtn != 0)
            {
                return;     // 出错
            }
        }

        /// <summary>
        /// 获取轴的当前位置
        /// </summary>
        /// <param name="axisPos"></param>
        private void GetCurrentAxisZPose(out double axisPos)
        {
            // 读取电机当前的理论位置
            int Pos = 0;
            short rtn = CNMCLib20.NMC_MtGetAxisPos(axisZHandle, ref Pos);
            axisPos = Pos / pulseFactor_x;
            if (rtn != 0)
            {
                return;     // 出错
            }
        }

        /// <summary>
        /// 轴移动
        /// </summary>
        /// <param name="maxVel"></param>
        /// <param name="targetPos"></param>
        private void MoveX(double maxVel, double targetPos)
        {
            short rtn;
            // 模式配置：将指定轴配置为点到点运动模式
            rtn = CNMCLib20.NMC_MtSetPrfMode(axisXHandle, CNMCLib20.MT_PTP_PRF_MODE);
            if (rtn != 0)
            {
                return;     // 模式配置出错
            }
            // 运动参数配置      
            ptpPrm.acc = 0.5;       //加速度
            ptpPrm.dec = 0.5;       //减加速度
            ptpPrm.endVel = 0;      //结束速度
            ptpPrm.startVel = 0;    //起跳速度
            ptpPrm.smoothCoef = 0;  //平滑系数
            rtn = CNMCLib20.NMC_MtSetPtpPara(axisXHandle, ref ptpPrm);
            if (rtn != 0)
            {
                return;     // 运动参数配置出错
            }

            // 设置规划的最高速度
            rtn = CNMCLib20.NMC_MtSetVel(axisXHandle, maxVel);
            if (rtn != 0)
            {
                return;     // 设置规划的最高速度出错
            }

            // 设置目标位置
            rtn = CNMCLib20.NMC_MtSetPtpTgtPos(axisXHandle, (int)(targetPos * pulseFactor_x));
            if (rtn != 0)
            {
                return;     // 设置目标位置出错
            }

            // 启动运动
            rtn = CNMCLib20.NMC_MtUpdate(axisXHandle);
            if (rtn != 0)
            {
                return;     // 启动运动出错
            }
            //IsAxisXStop();
        }

        /// <summary>
        /// 轴移动
        /// </summary>
        /// <param name="maxVel"></param>
        /// <param name="targetPos"></param>
        private void MoveY(double maxVel, double targetPos)
        {

            short rtn;
            // 模式配置：将指定轴配置为点到点运动模式
            rtn = CNMCLib20.NMC_MtSetPrfMode(axisYHandle, CNMCLib20.MT_PTP_PRF_MODE);
            if (rtn != 0)
            {
                return;     // 模式配置出错
            }
            // 运动参数配置      
            ptpPrm.acc = 0.5;       //加速度
            ptpPrm.dec = 0.5;       //减加速度
            ptpPrm.endVel = 0;      //结束速度
            ptpPrm.startVel = 0;    //起跳速度
            ptpPrm.smoothCoef = 0;  //平滑系数
            rtn = CNMCLib20.NMC_MtSetPtpPara(axisYHandle, ref ptpPrm);
            if (rtn != 0)
            {
                return;     // 运动参数配置出错
            }

            // 设置规划的最高速度
            rtn = CNMCLib20.NMC_MtSetVel(axisYHandle, maxVel);
            if (rtn != 0)
            {
                return;     // 设置规划的最高速度出错
            }

            // 设置目标位置
            rtn = CNMCLib20.NMC_MtSetPtpTgtPos(axisYHandle, (int)(targetPos * pulseFactor_x));
            if (rtn != 0)
            {
                return;     // 设置目标位置出错
            }

            // 启动运动
            rtn = CNMCLib20.NMC_MtUpdate(axisYHandle);
            if (rtn != 0)
            {
                return;     // 启动运动出错
            }
            //IsAxisYStop();
        }
        /// <summary>
        /// 轴移动
        /// </summary>
        /// <param name="maxVel"></param>
        /// <param name="targetPos"></param>
        private void MoveZ(double maxVel, double targetPos)
        {

            short rtn;
            // 模式配置：将指定轴配置为点到点运动模式
            rtn = CNMCLib20.NMC_MtSetPrfMode(axisZHandle, CNMCLib20.MT_PTP_PRF_MODE);
            if (rtn != 0)
            {
                return;     // 模式配置出错
            }
            // 运动参数配置      
            ptpPrm.acc = 0.5;       //加速度
            ptpPrm.dec = 0.5;       //减加速度
            ptpPrm.endVel = 0;      //结束速度
            ptpPrm.startVel = 0;    //起跳速度
            ptpPrm.smoothCoef = 0;  //平滑系数
            rtn = CNMCLib20.NMC_MtSetPtpPara(axisZHandle, ref ptpPrm);
            if (rtn != 0)
            {
                return;     // 运动参数配置出错
            }

            // 设置规划的最高速度
            rtn = CNMCLib20.NMC_MtSetVel(axisZHandle, maxVel);
            if (rtn != 0)
            {
                return;     // 设置规划的最高速度出错
            }

            // 设置目标位置
            rtn = CNMCLib20.NMC_MtSetPtpTgtPos(axisZHandle, (int)(targetPos * pulseFactor_x)); // 单位为脉冲
            if (rtn != 0)
            {
                return;     // 设置目标位置出错
            }

            // 启动运动
            rtn = CNMCLib20.NMC_MtUpdate(axisZHandle);
            if (rtn != 0)
            {
                return;     // 启动运动出错
            }
            //IsAxisZStop();
        }
        /// <summary>
        /// 判断轴是否停止
        /// </summary>
        /// <returns></returns>
        private bool IsAxisXStop()
        {
            // 运动过程控制,判断轴是否完成
            short axisSts = 0;
            while (true)
            {
                short rtn = CNMCLib20.NMC_MtGetSts(axisXHandle, ref axisSts);
                if (rtn != 0)
                {
                    return false;
                }
                if ((axisSts & CNMCLib20.BIT_AXIS_BUSY) == 0)
                {
                    break;  // 运动完成
                }
                Thread.Sleep(50);
            }
            return true;
        }
        private bool IsAxisYStop()
        {
            // 运动过程控制,判断轴是否完成
            short axisSts = 0;
            while (true)
            {
                short rtn = CNMCLib20.NMC_MtGetSts(axisYHandle, ref axisSts);
                if (rtn != 0)
                {
                    return false;
                }
                if ((axisSts & CNMCLib20.BIT_AXIS_BUSY) == 0)
                {
                    break;  // 运动完成
                }
                Thread.Sleep(50);
            }
            return true;
        }
        private bool IsAxisZStop()
        {
            // 运动过程控制,判断轴是否完成
            short axisSts = 0;
            while (true)
            {
                short rtn = CNMCLib20.NMC_MtGetSts(axisZHandle, ref axisSts);
                if (rtn != 0)
                {
                    return false;
                }
                if ((axisSts & CNMCLib20.BIT_AXIS_BUSY) == 0)
                {
                    break;  // 运动完成
                }
                Thread.Sleep(50);
            }
            return true;
        }

        /// <summary>
        /// 获取轴的当前状态信息
        /// </summary>
        /// <param name="axisPos"></param>
        /// <param name="axisVel"></param>
        /// <returns></returns>
        private short GetAxisState(out int axisPos, out double axisVel)
        {
            short rtn;
            short axisSts = -1;
            axisPos = 0;
            axisVel = 0;

            while (true)
            {
                //　读取单轴的状态标志，包括是否正在运动、是否限位报警触发等，请参考头文件对应位定义
                rtn = CNMCLib20.NMC_MtGetSts(axisXHandle, ref axisSts);
                if (rtn != 0)
                {
                    return rtn;
                }
                if ((axisSts & CNMCLib20.BIT_AXIS_BUSY) != 0)
                {
                    return 1;
                    // 轴正在运动
                }
                if ((axisSts & CNMCLib20.BIT_AXIS_ALM) != 0)
                {
                    return 400;
                    // 伺服报警触发
                }
                if ((axisSts & CNMCLib20.BIT_AXIS_LMTP) != 0)
                {
                    return 40;
                    // 正向限位触发
                }

                // 读取电机当前的理论位置
                rtn = CNMCLib20.NMC_MtGetAxisPos(axisXHandle, ref axisPos);
                // 读取电气档期的速度
                rtn = CNMCLib20.NMC_MtGetPrfVel(axisXHandle, ref axisVel);
            }
        }

        /// <summary>
        /// 清除轴错误状态
        /// </summary>
        private bool ClearAxisError(short AxisNo)
        {
            short rtn = -1;
            switch (AxisNo)
            {
                case 0:
                    rtn = CNMCLib20.NMC_MtClrError(axisXHandle);
                    // CNMCLib20.NMC_CrdClrError(BoardHandle);
                    break;
                case 1:
                    rtn = CNMCLib20.NMC_MtClrError(axisYHandle);
                    break;
                case 2:
                    rtn = CNMCLib20.NMC_MtClrError(axisZHandle);
                    break;
                case 3:
                    rtn = CNMCLib20.NMC_MtClrError(axisUHandle);
                    break;
                default:
                    break;
            }
            if (rtn != 0) return false;
            return true;
        }

        /// <summary>
        /// 伺服电机On
        /// </summary>
        /// <returns></returns>
        private bool ServeoOn(short AxisNo)
        {
            short rtn = -1;
            switch (AxisNo)
            {
                case 0:
                    rtn = CNMCLib20.NMC_MtSetSvOn(axisXHandle);
                    break;
                case 1:
                    rtn = CNMCLib20.NMC_MtSetSvOn(axisYHandle);
                    break;
                case 2:
                    rtn = CNMCLib20.NMC_MtSetSvOn(axisZHandle);
                    break;
                case 3:
                    rtn = CNMCLib20.NMC_MtSetSvOn(axisUHandle);
                    break;
                default:
                    break;
            }
            //short rtn = CNMCLib20.NMC_MtSetSvOn(axisXHandle);
            if (rtn != 0) return false;
            return true;
        }

        /// <summary>
        /// 伺服电机Off
        /// </summary>
        /// <returns></returns>
        private bool ServeoOff(short AxisNo)
        {
            short rtn = -1;
            switch (AxisNo)
            {
                case 0:
                    rtn = CNMCLib20.NMC_MtSetSvOff(axisXHandle);
                    break;
                case 1:
                    rtn = CNMCLib20.NMC_MtSetSvOff(axisYHandle);
                    break;
                case 2:
                    rtn = CNMCLib20.NMC_MtSetSvOff(axisZHandle);
                    break;
                case 3:
                    rtn = CNMCLib20.NMC_MtSetSvOff(axisUHandle);
                    break;
                default:
                    break;
            }
            //  short rtn = CNMCLib20.NMC_MtSetSvOff(axisXHandle);
            if (rtn != 0) return false;
            return true;
        }

        /// <summary>
        /// 初始化控制器
        /// </summary>
        /// <returns></returns>
        private bool init(short AxisNo)
        {
            bool a = false;
            bool b = false;
            bool c = false;
            bool d = false;
            try
            {
                a = InitBoard();
                b = OpenAxis(AxisNo);
                c = AxisConfig(AxisNo);
                ClearAxisError(AxisNo); // 只能在打开轴后清除轴错误
                d = ServeoOn(AxisNo);
            }
            catch
            {
            }
            if (a & b & c & d) return true;
            return false;
        }
        private void LinearComparator(enCoordSysName CoordSysName,CoordSysAxisParam axisPosition, double cmpDist, bool cmpStatus)
        {

            if (cmpStatus)
            {
                double pos_x, pos_y;
                //int DoValue;
                double[] cmpData_x = new double[0];
                double[] cmpData_y = new double[0];
                //this.cts = new CancellationTokenSource();
                // 获取起始点的坐标
                GetAxisPosition(CoordSysName,enAxisName.X轴, out pos_x);
                GetAxisPosition(CoordSysName,enAxisName.Y轴, out pos_y);
                // 创建比较点:axisPosition:表示终点的位置，
                CalculateCmpPoint(pos_x, pos_y, axisPosition.X, axisPosition.Y, cmpDist, out cmpData_x, out cmpData_y);
                ////////////////////////////
                Task.Run(() =>
                {
                    int index = 0;
                    if (this.cts != null)
                    {
                       while (!this.cts.IsCancellationRequested)
                        {
                            GetAxisPosition(CoordSysName,enAxisName.X轴,  out pos_x);
                            if (cmpData_x == null || cmpData_x.Length == 0) continue;
                            if (index == cmpData_x.Length) break;
                            if (Math.Abs(pos_x - cmpData_x[index]) < 0.05)
                            {
                                CNMCLib20.NMC_SetDOBit(this.BoardHandle, (short)0, 0);
                                Thread.Sleep(200);
                                CNMCLib20.NMC_SetDOBit(this.BoardHandle, (short)0, 1);
                                LoggerHelper.Info("线性触发位置:" + pos_x.ToString());
                                index++; // 每比较一次，索引增加1
                            }
                        }
                    }
                });

            }
        }

        #region 实现接口

        public override bool Init(DeviceConnectConfigParam name)
        {
            bool result = true;
            //this.cardType = enDeviceType.NMC_lib200;
            this.Name = name.DeviceName;
            //////////////////
            try
            {
                //this.InitParam(name);
                if (init(0))
                {
                    name.ConnectState = true;
                    result = true;
                }
                if (init(1))
                {
                    name.ConnectState = true;
                    result = true;
                }
                if (init(2))
                {
                    name.ConnectState = true;
                    result = true;
                }
                if (init(3))
                {
                    name.ConnectState = true;
                    result = true;
                }
            }
            catch
            {
                name.ConnectState = false;
                result = false;
            }
            name.ConnectState = result;
            return result ;
        }
        public override void MoveSingleAxis(enCoordSysName CoordSysName,enAxisName axisName, double speed, double axisPosition)
        {
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    MoveX(speed, axisPosition);
                    IsAxisXStop();
                    break;
                case enAxisName.Y轴:
                    MoveY(speed, axisPosition);
                    IsAxisYStop();
                    break;
                case enAxisName.Z轴:
                    MoveZ(speed, axisPosition);
                    IsAxisZStop();
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
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed,  CoordSysAxisParam axisPosition)
        {
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.XY轴直线插补:
                case enAxisName.XY轴:
                    LinearComparator(CoordSysName,axisPosition, 0.01, true);
                    MoveX(speed, axisPosition.X);
                    MoveY(speed, axisPosition.Y);
                    IsAxisXStop();
                    IsAxisYStop();
                    break;
                case enAxisName.XYZ轴:
                    LinearComparator(CoordSysName,axisPosition, 0.01, true);
                    MoveX(speed, axisPosition.X);
                    MoveY(speed, axisPosition.Y);
                    MoveZ(speed, axisPosition.Z);
                    IsAxisXStop();
                    IsAxisYStop();
                    IsAxisZStop();
                    break;
            }
        }
        public override void MoveMultyAxis(MoveCommandParam motionCommandParam)
        {
            //if (!this.connectState) return;
            switch (motionCommandParam.MoveAxis)
            {
                case enAxisName.XY轴直线插补:
                case enAxisName.XY轴:
                    LinearComparator(motionCommandParam.CoordSysName,motionCommandParam.AxisParam, 0.01, true);
                    MoveX(motionCommandParam.MoveSpeed, motionCommandParam.AxisParam.X);
                    MoveY(motionCommandParam.MoveSpeed, motionCommandParam.AxisParam.Y);
                    IsAxisXStop();
                    IsAxisYStop();
                    break;
                case enAxisName.XYZ轴:
                    LinearComparator(motionCommandParam.CoordSysName, motionCommandParam.AxisParam, 0.01, true);
                    MoveX(motionCommandParam.MoveSpeed, motionCommandParam.AxisParam.X);
                    MoveY(motionCommandParam.MoveSpeed, motionCommandParam.AxisParam.Y);
                    MoveZ(motionCommandParam.MoveSpeed, motionCommandParam.AxisParam.Z);
                    IsAxisXStop();
                    IsAxisYStop();
                    IsAxisZStop();
                    break;
            }
        }
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    AxisXHome();
                    break;
                case enAxisName.Y轴:
                    AxisYHome();
                    break;
                case enAxisName.Z轴:
                    AxisZHome();
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
        public override void MultyAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    AxisHome(0); // 因为只有一个轴，所以全部定义为Y轴
                    break;
                case enAxisName.Y轴:
                    AxisHome(1);
                    break;
                case enAxisName.Z轴:
                    AxisHome(2);
                    break;
                case enAxisName.U轴:
                    AxisHome(3);
                    break;
                case enAxisName.V轴:

                    break;
                case enAxisName.W轴:

                    break;
                default:
                    break;

            }
        }
        public override void GetAxisPosition(enCoordSysName CoordSysName, enAxisName axisName, out double position)
        {
            position = -0;
            //if (!this.connectState) return;
            switch (axisName)
            {
                case enAxisName.X轴:
                    GetCurrentAxisXPose(out position); // 因为只有一个轴，所以全部定义为Y轴
                    MirrorAxisCoord(CoordSysName, axisName, position, out position);
                    break;
                case enAxisName.Y轴:
                    GetCurrentAxisYPose(out position);
                    MirrorAxisCoord(CoordSysName, axisName, position, out position);
                    break;
                case enAxisName.Z轴:
                    GetCurrentAxisZPose(out position);
                    MirrorAxisCoord(CoordSysName, axisName, position, out position);
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
        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            double[] Max_Speed = new double[4];
            double[] Acc = new double[4];
            MirrorAxisJog(CoordSysName, axisName, speed, out speed);
            switch (axisName)
            {
                case enAxisName.X轴:                   
                    if (speed > 0)
                        MoveXJog(speed, true);
                    else
                        MoveXJog(speed, true);
                    break;
                case enAxisName.Y轴:
                    if (speed > 0)
                        MoveYJog(speed, true);
                    else
                        MoveYJog(speed, true);
                    break;
                case enAxisName.Z轴:
                    if (speed > 0)
                        MoveZJog(speed, true);
                    else
                        MoveZJog(speed, true);
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
        public override void JogAxisStop()
        {
            for (short i = 0; i < 3; i++)
            {
                StopAxis(i);
            }
        }
        public override void SlowDownStopAxis()
        {
            //throw new NotImplementedException();
        }
        public override void EmgStopAxis()
        {
            // if (!this.connectState) return;
            try
            {
                for (short i = 0; i < 3; i++)
                {
                    ServeoOff(i);
                }
            }
            catch
            {

            }
        }
        public override void UnInit()
        {
            // throw new NotImplementedException();
        }
        public override void SetIoOutputBit(object IoType, int IoPort, bool state)
        {
            //if (!this.connectState) return;
            int DoValue;
            enIoOutputMode ioType = enIoOutputMode.NONE;
            if (Enum.TryParse<enIoOutputMode>(IoType.ToString(), out ioType))
            { }
            else
                ioType = enIoOutputMode.NONE; // 
            switch (ioType)
            {
                case enIoOutputMode.双脉冲输出:
                    CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 1); // 关闭端口
                    Thread.Sleep(200);
                    CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 0); // 0:打开端口，为低电平;1:关闭端口：IO口为常闭端口
                    Thread.Sleep(200);
                    CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 1); // 关闭端口
                    break;
                case enIoOutputMode.脉冲输出:
                    if (state) // true:表示启用通道输出
                    {
                        CNMCLib20.NMC_SetDOBitRevs(this.BoardHandle, Convert.ToInt16(IoPort), 1);
                        //CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 1); // 关闭端口
                        //Thread.Sleep(200);
                        //CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 0); // 0:打开端口，为低电平;1:关闭端口：IO口为常闭端口
                        //Thread.Sleep(200);
                        //CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 1); // 关闭端口
                    }
                    else
                        CNMCLib20.NMC_SetDOBitRevs(this.BoardHandle, Convert.ToInt16(IoPort), 0);
                    //CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 1); // 关闭端口
                    break;
                case enIoOutputMode.高电平输出:
                    if (state) // true:表示启用通道输出
                    {
                        CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 1); // 关闭端口
                    }
                    else // false:表示禁用通道输出
                    {
                        CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 0);
                    }
                    break;
                case enIoOutputMode.低电平输出:
                    if (state) // true:表示启用通道输出
                    {
                        CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 0); // 打开端口
                        ///////////////////////////////
                    }
                    else // false:表示禁用通道输出
                    {
                        CNMCLib20.NMC_SetDOBit(this.BoardHandle, Convert.ToInt16(IoPort), 1);
                    }
                    break;
                case enIoOutputMode.线性比较IO输出:
                    if (state)
                    {
                       // this.cmpStatus = true;
                       // this.port = IoPort;
                    }
                    else
                    {
                        //this.cmpStatus = false;
                        //if (this.cts != null)
                           // this.cts.Cancel();
                    }
                    break;
                case enIoOutputMode.NONE:
                default:
                    break;
            }
        }
        public override void GetIoOutputBit(object IoType, int IoPort, out bool state)
        {
            int DoValue;
            state = false;
            // if (!this.connectState) return;
            switch (IoType.ToString())
            {
                case "通用IO输出":
                    CNMCLib20.NMC_GetDOGroup(this.BoardHandle, out DoValue, Convert.ToInt16(IoPort));
                    if (DoValue == 1)
                        state = true;
                    else
                        state = false;
                    break;
                case "高速比较IO输出":
                    CNMCLib20.NMC_GetDOGroup(this.BoardHandle, out DoValue, Convert.ToInt16(IoPort));
                    if (DoValue == 1)
                        state = true;
                    else
                        state = false;
                    break;
                case "通用比较IO输出":

                    break;
            }
        }
        public override void SetIoIntputBit(object IoType, int IoPort, bool state)
        {
            //throw new NotImplementedException();
        }
        public override void GetIoIntputBit(object IoType, int IoPort, out bool state)
        {
            state = false;
            //throw new NotImplementedException();
        }
        public override void SetParam(enParamType paramType, params object[] paramValue)
        {
            try
            {
                switch (paramType)
                {
                    case enParamType.触发间隔:
                        //this.LineCmpDist = (double)paramValue[0];
                        break;
                }
            }
            catch
            {

            }
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

        public override object ReadValue(enDataTypes dataType, string address, ushort length)
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
