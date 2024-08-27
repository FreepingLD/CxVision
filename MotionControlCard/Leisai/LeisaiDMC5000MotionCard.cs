using csLTDMC;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common;
using System.Text;


namespace MotionControlCard
{
    public class LeisaiDMC5000MotionCard : MotionBase
    {
        private ushort _CardID = 0;
        private double pulseFactor_x = 10000;
        private double pulseFactor_y = 10000;
        private double pulseFactor_z = 10000; //2000
        private double pulseFactor_u = 555.555555555555555;
        private double pulseFactor_v = 277.777778; // 1000
        private double pulseFactor_w = 1000;
        private double Tacc = 0.1;
        private double Tdec = 0.1;
        private int factorError = 10;// 单位：pluse


        private void SetEquiv()
        {
            LTDMC.dmc_set_equiv(this._CardID, 0, this.pulseFactor_x);
            LTDMC.dmc_set_equiv(this._CardID, 1, this.pulseFactor_y);
            LTDMC.dmc_set_equiv(this._CardID, 2, this.pulseFactor_z);
            LTDMC.dmc_set_equiv(this._CardID, 3, this.pulseFactor_u);
            LTDMC.dmc_set_equiv(this._CardID, 4, this.pulseFactor_v);
            LTDMC.dmc_set_equiv(this._CardID, 5, this.pulseFactor_w);
        }

        public override bool Init(DeviceConnectConfigParam name)
        {
            bool result = false;
            try
            {
                //this.InitParam(name);
                //this.pitchDist_z = 5;
                //this.cardType = enDeviceType.LeiSai_DMC5000;
                this.Name = name.DeviceName;
                ///////////////////
                short num = LTDMC.dmc_board_init();//获取卡数量
                if (num <= 0 || num > 8)
                    return result;
                /////////////////////////////////
                ushort _num = 0;
                ushort[] cardids = new ushort[8];
                uint[] cardtypes = new uint[8];
                short res = LTDMC.dmc_get_CardInfList(ref _num, cardtypes, cardids);
                if (res != 0)
                    return result;
                _CardID = cardids[0];
                string path = Application.StartupPath + "\\" + "DMC5800Param.ini";
                short res2 = LTDMC.dmc_download_configfile(_CardID, Application.StartupPath + "\\" + "DMC5800Param.ini");
                if (res != 0 && res2 != 0)
                    return result;
                /// 使能各轴 1:表示使能
                if (LTDMC.dmc_get_sevon_enable(_CardID, 0) == 0)
                    LTDMC.dmc_set_sevon_enable(_CardID, 0, 1);
                //////////////////////////////////////////////
                if (LTDMC.dmc_get_sevon_enable(_CardID, 1) == 0)
                    LTDMC.dmc_set_sevon_enable(_CardID, 1, 1);
                ////////////////////////////////////////
                if (LTDMC.dmc_get_sevon_enable(_CardID, 2) == 0)
                    LTDMC.dmc_set_sevon_enable(_CardID, 2, 1);
                ///////////////////////////////////////////
                if (LTDMC.dmc_get_sevon_enable(_CardID, 3) == 0)
                    LTDMC.dmc_set_sevon_enable(_CardID, 3, 1);
                ///////////////////////////////////////////
                if (LTDMC.dmc_get_sevon_enable(_CardID, 4) == 0)
                    LTDMC.dmc_set_sevon_enable(_CardID, 4, 1);
                ///////////////
                LTDMC.dmc_write_outbit(_CardID, 0, 0);// 松开Z轴刹车
                                                      //////////////////////////
                SetEquiv();
                result = true;
                //this.connectState = true;
            }
            catch
            {
                //this.connectState = false;
                result = false;
            }
            name.ConnectState = result;
            return result;
        }
        public override void UnInit()
        {
            LTDMC.dmc_write_outbit(_CardID, 0, 1);// 打开Z轴刹车
            LTDMC.dmc_board_close();

        }
        public override void MoveSingleAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, double axisPosition)
        {
            double targetPose;
            switch (axisName)
            {
                case enAxisName.X轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord( CoordSysName, enAxisName.X轴, axisPosition,out targetPose);
                        // 在开始运动前为什么要先停止 ？
                        LTDMC.dmc_stop(_CardID, 0, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 0, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 0, targetPose, 1); // 这里执行后为什么一直处理执行状态？
                        // 0:表示运动
                        while (LTDMC.dmc_check_done(_CardID, 0) == 0)  //判断回零运动是否完成; 
                        {
                            Application.DoEvents();
                        }
                    }

                    break;
                case enAxisName.Y轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord( CoordSysName, enAxisName.Y轴, axisPosition, out targetPose);
                        LTDMC.dmc_stop(_CardID, 1, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 1, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 1, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 1, targetPose, 1);
                        while (LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                case enAxisName.Z轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord( CoordSysName, enAxisName.Z轴, axisPosition, out targetPose);
                        LTDMC.dmc_stop(_CardID, 2, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 2, 5, 1);
                        LTDMC.dmc_set_profile_unit(this._CardID, 2, 0, speed * 0.1, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 2, targetPose, 1);// 这里使用反馈脉冲还是电机脉冲？
                        while (LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;

                case enAxisName.U轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord( CoordSysName, enAxisName.U轴, axisPosition, out targetPose);
                        LTDMC.dmc_stop(_CardID, 3, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 3, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 3, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 3, targetPose, 1);
                        while (LTDMC.dmc_check_done(_CardID, 3) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                case enAxisName.V轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord( CoordSysName, enAxisName.V轴, axisPosition, out targetPose);
                        LTDMC.dmc_stop(_CardID, 4, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 4, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 4, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 4, targetPose, 1);
                        while (LTDMC.dmc_check_done(_CardID, 4) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                case enAxisName.Theta轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord( CoordSysName, enAxisName.Theta轴, axisPosition, out targetPose);
                        LTDMC.dmc_stop(_CardID, 5, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 5, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 5, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 5, targetPose, 1);
                        while (LTDMC.dmc_check_done(_CardID, 5) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
            }
        }
        public override void MoveMultyAxis(enCoordSysName CoordSysName, enAxisName axisName, double speed, CoordSysAxisParam axisPosition)
        {
            double center_x, center_y;
            double x = 0, y = 0, z = 0, u = 0, v = 0, w = 0;
            MirrorAxisCoord( CoordSysName, axisPosition, out x, out y, out z, out u, out v, out w);
            /////////////////////
            switch (axisName)
            {
                case enAxisName.X轴:
                    if (this._CardID >= 0)
                    {
                        // 在开始运动前为什么要先停止 ？
                        LTDMC.dmc_stop(_CardID, 0, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 0, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1); // 这里执行后为什么一直处理执行状态？
                        // 0:表示运动
                        while (LTDMC.dmc_check_done(_CardID, 0) == 0)  //判断回零运动是否完成; 
                        {
                            Application.DoEvents();
                        }
                    }

                    break;
                case enAxisName.Y轴:
                    if (this._CardID >= 0)
                    {
                        LTDMC.dmc_stop(_CardID, 1, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 1, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 1, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                        while (LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                case enAxisName.Z轴:
                    if (this._CardID >= 0)
                    {
                        LTDMC.dmc_stop(_CardID, 2, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 2, 5, 1);
                        LTDMC.dmc_set_profile_unit(this._CardID, 2, 0, speed *0.1, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);// 这里使用反馈脉冲还是电机脉冲？
                        while (LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;

                case enAxisName.U轴:
                    if (this._CardID >= 0)
                    {
                        LTDMC.dmc_stop(_CardID, 3, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 3, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 3, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 3, u, 1);
                        while (LTDMC.dmc_check_done(_CardID, 3) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                case enAxisName.V轴:
                    if (this._CardID >= 0)
                    {
                        LTDMC.dmc_stop(_CardID, 4, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 4, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 4, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 4, v, 1);
                        while (LTDMC.dmc_check_done(_CardID, 4) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                case enAxisName.W轴:
                    if (this._CardID >= 0)
                    {
                        LTDMC.dmc_stop(_CardID, 5, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 5, 1, 10);
                        LTDMC.dmc_set_profile_unit(this._CardID, 5, 0, speed, this.Tacc, this.Tdec, 0);
                        LTDMC.dmc_pmove_unit(this._CardID, 5, w, 1);
                        while (LTDMC.dmc_check_done(_CardID, 5) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                case enAxisName.XY轴:
                    //if (axisPosition.Length < 1) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, 10);
                    ///////////////////////
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 1, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    break;
                case enAxisName.XZ轴:
                   // if (axisPosition.Length < 2) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, 10);
                    ////////////////////////////
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 2, 0, speed * 0.1, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    break;
                case enAxisName.XYZ轴:
                   // if (axisPosition.Length < 3) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, 10);
                    ////////////////////////
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 1, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 2, 0, speed *0.1, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    break;
                case enAxisName.XYZU轴:
                   // if (axisPosition.Length < 4) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_stop(_CardID, 3, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 3, 1, 10);
                    //////////////////////////////
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 1, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 2, 0, speed *0.1, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 3, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 3, u, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0 || LTDMC.dmc_check_done(_CardID, 3) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    break;
                case enAxisName.XYZUV轴:
                    //if (axisPosition.Length < 5) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_stop(_CardID, 3, 0);
                    LTDMC.dmc_stop(_CardID, 4, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 3, 1, 10);
                    LTDMC.dmc_set_factor_error(this._CardID, 4, 1, 10);
                    ///////////////////////////
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 1, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 2, 0, speed *0.1, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 3, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_profile_unit(this._CardID, 4, 0, speed, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 3, u, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 4, v, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0 || LTDMC.dmc_check_done(_CardID, 3) == 0 || LTDMC.dmc_check_done(_CardID, 4) == 0)
                    {
                        Application.DoEvents();
                    }
                    break;


                /// 插补运动///////////////////////
                case enAxisName.XY轴直线插补:
                    ////轴移动
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, 1); //执行两轴直线插补运动
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                    }
                    break;
                case enAxisName.XY轴圆弧插补:
                    /////////////////////////////////////
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    GetAxisPosition( CoordSysName, enAxisName.X轴, out center_x);
                    GetAxisPosition( CoordSysName, enAxisName.Y轴,  out center_y);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, 1); //先移动到圆弧起点+ axisPosition[3]*Math.Cos(1) * Math.Sin(1)
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                    }
                    ///////////////////
                    LTDMC.dmc_arc_move_center_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, new double[] { center_x, center_y }, 1, 1, 1); //执行 X、Y 轴圆弧插补运动+ axisPosition[3]
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    //判断坐标系运动状态，等待运动完成
                    {
                        Application.DoEvents();
                    }
                    break;
                case enAxisName.XY同心圆插补:
                case enAxisName.XY螺旋线插补:
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    GetAxisPosition(CoordSysName,enAxisName.X轴,  out center_x); // 圆心由直线的起点决定
                    GetAxisPosition(CoordSysName, enAxisName.Y轴,  out center_y);
                    ////////////////////////////////////////// 插补的起点与终点决定了螺旋线的类型//////////
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { (center_x + 0), center_y }, 1); //先移动到圆弧起点+ axisPosition[3]*Math.Cos(1) * Math.Sin(1)
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0) //axisPosition[3]:表示偏置
                    {
                        Application.DoEvents();
                    }
                    //////////////////////////////
                    LTDMC.dmc_arc_move_center_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, new double[] { center_x, center_y }, 1, 1, 1); //执行 X、Y 轴圆弧插补运动,
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    //判断坐标系运动状态，等待运动完成
                    {
                        Application.DoEvents();
                    }
                    break;

                case enAxisName.XY轴椭圆插补:


                    break;
            }
        }
        public override void MoveMultyAxis(MoveCommandParam moveParam)
        {
            double center_x, center_y, center_z;
            double x = 0, y = 0, z = 0, u = 0, v = 0, w = 0;
            /////////////////////
            switch (moveParam.MoveAxis)
            {
                case enAxisName.X轴:
                    if (this._CardID >= 0)
                    {
                        // 在开始运动前为什么要先停止 ？
                        MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                        LTDMC.dmc_stop(_CardID, 0, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                        LTDMC.dmc_set_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                        LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1); // 这里执行后为什么一直处理执行状态？
                        // 0:表示运动
                        while (LTDMC.dmc_check_done(_CardID, 0) == 0)  //判断轴运动是否完成; 
                        {
                            Application.DoEvents();
                            LoggerHelper.Warn("轴正在运动");
                        }
                    }

                    break;
                case enAxisName.Y轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord(moveParam.CoordSysName,enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                        LTDMC.dmc_stop(_CardID, 1, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                        LTDMC.dmc_set_profile_unit(this._CardID, 1, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                        LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                        while (LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                            LoggerHelper.Warn("轴正在运动");
                        }
                    }
                    break;
                case enAxisName.Z轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord(moveParam.CoordSysName,enAxisName.Z轴, moveParam.AxisParam.Z, out z);
                        LTDMC.dmc_stop(_CardID, 2, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 2, 1, moveParam.FactorError);
                        LTDMC.dmc_set_profile_unit(this._CardID, 2, moveParam.StartVel, moveParam.MoveSpeed *0.1, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                        LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);// 这里使用反馈脉冲还是电机脉冲？
                        while (LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                            LoggerHelper.Warn("轴正在运动");
                        }
                    }
                    break;

                case enAxisName.U轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord(moveParam.CoordSysName, enAxisName.U轴, moveParam.AxisParam.U, out u);
                        LTDMC.dmc_stop(_CardID, 3, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 3, 1, moveParam.FactorError);
                        LTDMC.dmc_set_profile_unit(this._CardID, 3, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                        LTDMC.dmc_pmove_unit(this._CardID, 3, u, 1);
                        while (LTDMC.dmc_check_done(_CardID, 3) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                            LoggerHelper.Warn("轴正在运动");
                        }
                    }
                    break;
                case enAxisName.V轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord(moveParam.CoordSysName, enAxisName.V轴, moveParam.AxisParam.V, out v);
                        LTDMC.dmc_stop(_CardID, 4, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 4, 1, moveParam.FactorError);
                        LTDMC.dmc_set_profile_unit(this._CardID, 4, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                        LTDMC.dmc_pmove_unit(this._CardID, 4, v, 1);
                        while (LTDMC.dmc_check_done(_CardID, 4) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                            LoggerHelper.Warn("轴正在运动");
                        }
                    }
                    break;
                case enAxisName.Theta轴:
                    if (this._CardID >= 0)
                    {
                        MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Theta轴, moveParam.AxisParam.Theta, out w);
                        LTDMC.dmc_stop(_CardID, 5, 0);
                        LTDMC.dmc_set_factor_error(this._CardID, 5, 1, moveParam.FactorError);
                        LTDMC.dmc_set_profile_unit(this._CardID, 5, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                        LTDMC.dmc_pmove_unit(this._CardID, 5, w, 1);
                        while (LTDMC.dmc_check_done(_CardID, 5) == 0)  //判断回零运动是否完成
                        {
                            Application.DoEvents();
                            LoggerHelper.Warn("轴正在运动");
                        }
                    }
                    break;
                case enAxisName.XY轴:
                   // if (moveParam.AxisParam.Length < 1) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    LTDMC.dmc_set_profile_unit(this._CardID, 1, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
                case enAxisName.XZ轴:
                    //if (moveParam.AxisParam.Length < 2) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.AxisParam.Y, out z);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, moveParam.FactorError);
                    ////////////////////////////
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    LTDMC.dmc_set_profile_unit(this._CardID, 2, moveParam.StartVel, moveParam.MoveSpeed * 0.1, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
                case enAxisName.XYZ轴:
                   // if (moveParam.AxisParam.Length < 3) return;
                    if (this._CardID < 0) return;
                    /////////////////////////////////////
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.AxisParam.Z, out z);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, moveParam.FactorError);
                    ////////////////////////
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    LTDMC.dmc_set_profile_unit(this._CardID, 1, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    LTDMC.dmc_set_profile_unit(this._CardID, 2, moveParam.StartVel, moveParam.MoveSpeed *0.1, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    LTDMC.dmc_pmove_unit(this._CardID, 0, x, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 1, y, 1);
                    LTDMC.dmc_pmove_unit(this._CardID, 2, z, 1);
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;

                /// 插补运动///////////////////////
                case enAxisName.XY轴直线插补:
                    ////轴移动
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, 1); //执行两轴直线插补运动
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
                case enAxisName.XZ轴直线插补:
                    ////轴移动
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.AxisParam.Y, out z);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, moveParam.FactorError);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 2, new ushort[] { 0, 2 }, new double[] { x, z }, 1); //执行两轴直线插补运动
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
                case enAxisName.YZ轴直线插补:
                    ////轴移动
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.X, out y);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.AxisParam.Y, out z);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, moveParam.FactorError);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 2, new ushort[] { 1, 2 }, new double[] { y, z }, 1); //执行两轴直线插补运动
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
                case enAxisName.XYZ轴直线插补:
                    ////轴移动
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.AxisParam.Z, out z);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, moveParam.FactorError);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 3, new ushort[] { 0, 1, 2 }, new double[] { x, y, z }, 1); //执行两轴直线插补运动
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
                case enAxisName.XY螺旋线插补:
                case enAxisName.XY轴圆弧插补:
                    /////////////////////////////////////
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.CircleParam.centerPosition[0], out center_x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.CircleParam.centerPosition[1], out center_y);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel); //设置插补运动速度曲线
                    LTDMC.dmc_arc_move_center_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, new double[] { center_x, center_y }, moveParam.CircleParam.Dir, moveParam.CircleParam.Count, 1); //执行 X、Y 轴圆弧插补运动+ axisPosition[3]
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    //判断坐标系运动状态，等待运动完成
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
                case enAxisName.XYZ螺旋线插补:
                case enAxisName.XYZ轴圆弧插补:
                    /////////////////////////////////////
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.AxisParam.Z, out z);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.CircleParam.centerPosition[0], out center_x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.CircleParam.centerPosition[1], out center_y);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.CircleParam.centerPosition[2], out center_z);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel); //设置插补运动速度曲线
                    LTDMC.dmc_arc_move_center_unit(this._CardID, 0, 3, new ushort[] { 0, 1, 2 }, new double[] { x, y, z }, new double[] { center_x, center_y, center_z }, moveParam.CircleParam.Dir, moveParam.CircleParam.Count, 1); //执行 X、Y 轴圆弧插补运动+ axisPosition[3]
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    //判断坐标系运动状态，等待运动完成
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;

                case enAxisName.XY轴矩形插补:
                    /////////////////////////////////////
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.AxisParam.X, out x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.AxisParam.Y, out y);
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel);
                    //设置插补速度曲线参数，插补运动最大矢量速度 4000unit/s，加减速时间 0.1s
                    LTDMC.dmc_rectangle_move_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, new double[] { moveParam.Rect2Param.vector_x, moveParam.Rect2Param.vector_y }, moveParam.Rect2Param.lineCount, moveParam.Rect2Param.InterpolateMode, 1);
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                    }
                    break;

                case enAxisName.XYZ轴轨迹运动:
                    int[] track_x = new int[moveParam.TrackData.Count];
                    int[] track_y = new int[moveParam.TrackData.Count];
                    int[] track_z = new int[moveParam.TrackData.Count];
                    double[] time = new double[moveParam.TrackData.Count];
                    double first_x,first_y,first_z; // 三轴的第一个参考点
                    ////////////////////////////////////////////
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.TrackData[0][0], out first_x);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.TrackData[0][1], out first_y);
                    MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.TrackData[0][2], out first_z);
                    for (int i = 0; i < moveParam.TrackData.Count; i++)
                    {
                        MirrorAxisCoord(moveParam.CoordSysName, enAxisName.X轴, moveParam.TrackData[i][0], out x);
                        MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Y轴, moveParam.TrackData[i][1], out y);
                        MirrorAxisCoord(moveParam.CoordSysName, enAxisName.Z轴, moveParam.TrackData[i][2], out z);
                        track_x[i] = Convert.ToInt32((x - first_x) * this.pulseFactor_x);
                        track_y[i] = Convert.ToInt32((y - first_y) * this.pulseFactor_y);
                        track_z[i] = Convert.ToInt32((z - first_z) * this.pulseFactor_z);
                        time[i] = Convert.ToDouble(moveParam.TrackData[i][3]);
                    }
                    // 运动到起点位置
                    LTDMC.dmc_stop(_CardID, 0, 0);
                    LTDMC.dmc_stop(_CardID, 1, 0);
                    LTDMC.dmc_stop(_CardID, 2, 0);
                    LTDMC.dmc_set_factor_error(this._CardID, 0, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 1, 1, moveParam.FactorError);
                    LTDMC.dmc_set_factor_error(this._CardID, 2, 1, moveParam.FactorError);
                    LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, moveParam.StartVel, moveParam.MoveSpeed, moveParam.Tacc, moveParam.Tdec, moveParam.StopVel); //设置插补运动速度曲线
                    LTDMC.dmc_line_unit(this._CardID, 0, 3, new ushort[] { 0, 1, 2 }, new double[] { first_x, first_y, first_z }, 1); //执行两轴直线插补运动
                    while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    // 轨迹运动的坐标是相对于起点而言的
                    if (track_x.Length == 0) return;
                    short oo = LTDMC.dmc_PvtsTable(this._CardID, 0, (uint)moveParam.TrackData.Count, time, track_x, 0.0, 0.0); // 只能传入相对对标，即XYZ第一个点坐标为0
                    oo = LTDMC.dmc_PvtsTable(this._CardID, 1, (uint)moveParam.TrackData.Count, time, track_y, 0, 0);
                    oo = LTDMC.dmc_PvtsTable(this._CardID, 2, (uint)moveParam.TrackData.Count, time, track_z, 0, 0);
                    oo = LTDMC.dmc_PvtMove(this._CardID, 3, new ushort[] { 0, 1, 2 });
                    switch (oo)
                    {
                        case 0:
                            LoggerHelper.Info("执行成功");
                            break;
                        case 1:
                            LoggerHelper.Error("未知错误");
                            break;
                        case 2:
                            LoggerHelper.Error("参数错误");
                            break;
                        case 3:
                            LoggerHelper.Error("PCI 通讯超时");
                            break;
                        case 4:
                            LoggerHelper.Error("轴正在运动");
                            break;
                    }
                    if (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0)
                    {
                        Application.DoEvents();
                        LoggerHelper.Warn("轴正在运动");
                    }
                    break;
            }
        }
        public override void SingleAxisHome(enCoordSysName CoordSysName, enAxisName axisName, double homSpeed)
        {
            double eauit = 0;
            LTDMC.dmc_get_equiv(this._CardID, 0, ref eauit);
            LTDMC.dmc_get_equiv(this._CardID, 1, ref eauit);
            LTDMC.dmc_get_equiv(this._CardID, 2, ref eauit);
            LTDMC.dmc_get_equiv(this._CardID, 3, ref eauit);
            LTDMC.dmc_get_equiv(this._CardID, 4, ref eauit);
            switch (axisName)
            {
                case enAxisName.X轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 0, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 0);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 0, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 0, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.Y轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 1, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 1);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 1, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 1, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.Z轴:
                    LTDMC.dmc_write_outbit(_CardID, 0, 0);// Z轴刹车
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 2, homSpeed *0.1, homSpeed *0.1, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 2);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 2, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 2, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.U轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 3, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 3);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 3) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 3, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 3, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.V轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 4, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 4);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 4) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 4, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 4, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.W轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 5, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 5);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 5) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 5, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 5, 0);   //轴的反馈位置清零
                    break;
            }


        }
        public override void MultyAxisHome(enCoordSysName CoordSysName,enAxisName axisName, double homSpeed)
        {
            switch (axisName)
            {
                case enAxisName.X轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 0, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 0);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 0, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 0, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.Y轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 1, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 1);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 1, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 1, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.Z轴:
                    LTDMC.dmc_write_outbit(_CardID, 0, 0);// Z轴刹车
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 2, homSpeed * 0.1, homSpeed * 0.1, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 2);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 2, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 2, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.U轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 3, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 3);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 3) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 3, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 3, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.V轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 4, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 4);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 4) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 4, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 4, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.W轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 5, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 5);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 5) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 5, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 5, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.XY轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 0, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 1, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_home_move(_CardID, 0);//启动回零
                    LTDMC.dmc_home_move(_CardID, 1);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 0, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 0, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 1, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 1, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.XZ轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 0, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 2, homSpeed *0.1, homSpeed *0.1, this.Tacc, this.Tdec);
                    LTDMC.dmc_write_outbit(_CardID, 0, 0);// Z轴刹车
                    LTDMC.dmc_home_move(_CardID, 0);//启动回零
                    LTDMC.dmc_home_move(_CardID, 2);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 0, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 0, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 2, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 2, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.XYZ轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 0, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 1, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 2, homSpeed *0.1, homSpeed *0.1, this.Tacc, this.Tdec);
                    LTDMC.dmc_write_outbit(_CardID, 0, 0);// Z轴刹车松开
                    LTDMC.dmc_home_move(_CardID, 0);//启动回零
                    LTDMC.dmc_home_move(_CardID, 1);//启动回零
                    LTDMC.dmc_home_move(_CardID, 2);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 0, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 0, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 1, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 1, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 2, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 2, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.XYZU轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 0, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 1, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 2, homSpeed *0.1, homSpeed *0.1, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 3, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_write_outbit(_CardID, 0, 0);// Z轴刹车
                    LTDMC.dmc_home_move(_CardID, 0);//启动回零
                    LTDMC.dmc_home_move(_CardID, 1);//启动回零
                    LTDMC.dmc_home_move(_CardID, 2);//启动回零
                    LTDMC.dmc_home_move(_CardID, 3);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0 || LTDMC.dmc_check_done(_CardID, 3) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 0, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 0, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 1, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 1, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 2, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 2, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 3, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 3, 0);   //轴的反馈位置清零
                    break;
                case enAxisName.XYZUV轴:
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 0, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 1, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 2, homSpeed *0.1, homSpeed *0.1, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 3, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_set_home_profile_unit(this._CardID, 4, homSpeed, homSpeed, this.Tacc, this.Tdec);
                    LTDMC.dmc_write_outbit(_CardID, 0, 0);// Z轴刹车
                    LTDMC.dmc_home_move(_CardID, 0);//启动回零
                    LTDMC.dmc_home_move(_CardID, 1);//启动回零
                    LTDMC.dmc_home_move(_CardID, 2);//启动回零
                    LTDMC.dmc_home_move(_CardID, 3);//启动回零
                    LTDMC.dmc_home_move(_CardID, 4);//启动回零
                    while (LTDMC.dmc_check_done(_CardID, 0) == 0 || LTDMC.dmc_check_done(_CardID, 1) == 0 || LTDMC.dmc_check_done(_CardID, 2) == 0 || LTDMC.dmc_check_done(_CardID, 3) == 0 || LTDMC.dmc_check_done(_CardID, 4) == 0)  //判断回零运动是否完成
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(200);
                    LTDMC.dmc_set_position_unit(_CardID, 0, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 0, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 1, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder(_CardID, 1, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 2, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 2, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 3, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 3, 0);   //轴的反馈位置清零
                    LTDMC.dmc_set_position_unit(_CardID, 4, 0); //轴的指令位置清零
                    LTDMC.dmc_set_encoder_unit(_CardID, 4, 0);   //轴的反馈位置清零
                    break;
            }
        }
        public override void GetAxisPosition(enCoordSysName CoordSysName,enAxisName axisName,out double position)
        {
            double pos = 0;
            position = 0;
            switch (axisName)
            {
                case enAxisName.X轴:
                    if (this._CardID >= 0)
                        LTDMC.dmc_get_encoder_unit(this._CardID, 0, ref pos);
                    MirrorAxisCoord(CoordSysName,enAxisName.X轴, pos, out position);
                    ///////////////

                    break;
                case enAxisName.Y轴:
                    if (this._CardID >= 0)
                        LTDMC.dmc_get_encoder_unit(this._CardID, 1, ref pos);
                    MirrorAxisCoord(CoordSysName,enAxisName.Y轴, pos, out position);

                    break;
                case enAxisName.Z轴:
                    if (this._CardID >= 0)
                        LTDMC.dmc_get_encoder_unit(this._CardID, 2, ref pos);
                    //position = pos / -5; // 因为脉冲位置与指令位置相反
                    // 因为Z轴每转一圈走5mm(丝杆的导程为5mm),电机每转一圈所需要脉冲数为10000，所以走1mm对应的脉冲数是2000，而光栅尺每运动1mm,发出的脉冲是10000，所以他们间的传动比是1：5
                    MirrorAxisCoord(CoordSysName,enAxisName.Z轴, pos, out position);

                    break;
                case enAxisName.U轴:
                    if (this._CardID >= 0)
                        LTDMC.dmc_get_encoder_unit(this._CardID, 3, ref pos);
                    MirrorAxisCoord(CoordSysName, enAxisName.U轴, pos, out position);
                    break;
                case enAxisName.V轴:
                    if (this._CardID >= 0)
                        LTDMC.dmc_get_encoder_unit(this._CardID, 4, ref pos);
                    MirrorAxisCoord(CoordSysName, enAxisName.V轴, pos, out position);
                    break;
                case enAxisName.Theta轴:
                    if (this._CardID >= 0)
                        LTDMC.dmc_get_encoder_unit(this._CardID, 5, ref pos);
                    MirrorAxisCoord(CoordSysName, enAxisName.Theta轴, pos, out position);
                    break;
            }
        }
        public override void JogAxisStart(enCoordSysName CoordSysName, enAxisName axisName, double speed)
        {
            double moveSpeed;
            switch (axisName)
            {
                case enAxisName.X轴:
                    if (this._CardID < 0) return;
                    MirrorAxisJog(CoordSysName,enAxisName.X轴, speed, out moveSpeed);
                    LTDMC.dmc_set_profile_unit(this._CardID, 0, 0, Math.Abs(moveSpeed), this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_dec_stop_time(_CardID, 0, 0.01); //设置减速停止时间
                    if (moveSpeed > 0)
                        LTDMC.dmc_vmove(_CardID, 0, 1);//连续运动
                    else
                        LTDMC.dmc_vmove(_CardID, 0, 0);//连续运动
                    break;
                case enAxisName.Y轴:
                    if (this._CardID < 0) return;
                    MirrorAxisJog(CoordSysName, enAxisName.Y轴, speed, out moveSpeed);
                    LTDMC.dmc_set_profile_unit(this._CardID, 1, 0, Math.Abs(moveSpeed), this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_dec_stop_time(_CardID, 1, 0.01); //设置减速停止时间
                    if (moveSpeed > 0)
                        LTDMC.dmc_vmove(_CardID, 1, 1);//连续运动
                    else
                        LTDMC.dmc_vmove(_CardID, 1, 0);//连续运动
                    break;
                case enAxisName.Z轴:
                    if (this._CardID < 0) return;
                    MirrorAxisJog(CoordSysName, enAxisName.Z轴, speed, out moveSpeed);
                    LTDMC.dmc_write_outbit(_CardID, 0, 0);// Z轴刹车
                    LTDMC.dmc_set_profile_unit(this._CardID, 2, 0, Math.Abs(moveSpeed) *0.1, this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_dec_stop_time(_CardID, 2, 0.01); //设置减速停止时间
                    if (moveSpeed > 0)
                        LTDMC.dmc_vmove(_CardID, 2, 1);//连续运动
                    else
                        LTDMC.dmc_vmove(_CardID, 2, 0);//连续运动
                    break;
                case enAxisName.U轴:
                    if (this._CardID < 0) return;
                    MirrorAxisJog(CoordSysName, enAxisName.U轴, speed, out moveSpeed);
                    LTDMC.dmc_set_profile_unit(this._CardID, 3, 0, Math.Abs(moveSpeed), this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_dec_stop_time(_CardID, 3, 0.01); //设置减速停止时间
                    if (moveSpeed > 0)
                        LTDMC.dmc_vmove(_CardID, 3, 1);//连续运动
                    else
                        LTDMC.dmc_vmove(_CardID, 3, 0);//连续运动
                    break;
                case enAxisName.V轴:
                    if (this._CardID < 0) return;
                    MirrorAxisJog(CoordSysName, enAxisName.V轴, speed, out moveSpeed);
                    LTDMC.dmc_set_profile_unit(this._CardID, 4, 0, Math.Abs(moveSpeed), this.Tacc, this.Tdec, 0);
                    LTDMC.dmc_set_dec_stop_time(_CardID, 4, 0.01); //设置减速停止时间
                    if (moveSpeed > 0)
                        LTDMC.dmc_vmove(_CardID, 4, 1);//连续运动
                    else
                        LTDMC.dmc_vmove(_CardID, 4, 0);//连续运动
                    break;
            }
        }
        public override void JogAxisStop()
        {
            LTDMC.dmc_stop(_CardID, 0, 0);
            LTDMC.dmc_stop(_CardID, 1, 0);
            LTDMC.dmc_stop(_CardID, 2, 0);
            LTDMC.dmc_stop(_CardID, 3, 0);
            LTDMC.dmc_stop(_CardID, 4, 0);
        }
        public override void SlowDownStopAxis()
        {
            LTDMC.dmc_emg_stop(this._CardID);
            //throw new NotImplementedException();
        }
        public override void EmgStopAxis()
        {
            LTDMC.dmc_emg_stop(this._CardID);
        }
        public override void SetIoOutputBit(object IoType, int IoPort, bool state)
        {
            enIoOutputMode ioType = enIoOutputMode.NONE;
            if (Enum.TryParse<enIoOutputMode>(IoType.ToString(), out ioType))
            { }
            else
                ioType = enIoOutputMode.NONE;
            switch (ioType)
            {
                case enIoOutputMode.脉冲输出:

                    break;
                case enIoOutputMode.线性比较IO输出:
                    if (state)
                    {
                        //this.cmpStatus = true;
                        //this.port = IoPort;
                        LTDMC.dmc_hcmp_2d_set_enable(this._CardID, 0, 1);
                        LTDMC.dmc_hcmp_clear_points(this._CardID, 0);
                        LTDMC.dmc_hcmp_2d_set_config(this._CardID, 0, 0, 0, 1, 1, 1, 10, 1, 100, 0, 0, 0, (ushort)IoPort, 0); // 14:IO PORT
                    }
                    else
                    {
                        //this.cmpStatus = false;
                        //if (this.cts != null)
                        //    this.cts.Cancel();
                        //this.cts = null;
                        LTDMC.dmc_hcmp_2d_set_enable(this._CardID, 0, 0);
                    }
                    break;
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
            switch (paramType)
            {
                case enParamType.清除轴1错误:
                    LTDMC.dmc_write_erc_pin(this._CardID, 0, 1); // 清除指定轴的误差报错
                    Thread.Sleep(100);
                    LTDMC.dmc_write_erc_pin(this._CardID, 0, 0);
                    break;
                case enParamType.清除轴2错误:
                    LTDMC.dmc_write_erc_pin(this._CardID, 1, 1);
                    Thread.Sleep(100);
                    LTDMC.dmc_write_erc_pin(this._CardID, 1, 0);
                    break;
                case enParamType.清除轴3错误:
                    LTDMC.dmc_write_erc_pin(this._CardID, 2, 1);
                    Thread.Sleep(100);
                    LTDMC.dmc_write_erc_pin(this._CardID, 2, 0);
                    break;
                case enParamType.清除轴4错误:
                    LTDMC.dmc_write_erc_pin(this._CardID, 3, 1);
                    Thread.Sleep(100);
                    LTDMC.dmc_write_erc_pin(this._CardID, 3, 0);
                    break;
                case enParamType.清除轴5错误:
                    LTDMC.dmc_write_erc_pin(this._CardID, 4, 1);
                    Thread.Sleep(100);
                    LTDMC.dmc_write_erc_pin(this._CardID, 4, 0);
                    break;
                case enParamType.清除轴6错误:
                    LTDMC.dmc_write_erc_pin(this._CardID, 5, 1);
                    Thread.Sleep(100);
                    LTDMC.dmc_write_erc_pin(this._CardID, 5, 0);
                    break;
                case enParamType.触发间隔:
                    //this.LineCmpDist = (double)paramValue[0];
                    break;
            }
        }
        public override object GetParam(enParamType paramType, params object[] paramValue)
        {
            return null;
            // throw new NotImplementedException();
        }
        public override void WriteIoOutputBit(enIoPortType ioPortType, int IoPort, params object[] value)
        {
            switch (ioPortType)
            {
                default:
                case enIoPortType.通用Io端口:
                    LTDMC.dmc_write_outbit(this._CardID, (ushort)IoPort, Convert.ToUInt16(value[0]));
                    break;
                case enIoPortType.高速Io端口:

                    break;
                case enIoPortType.计数Io端口:
                    LTDMC.dmc_set_io_count_value(this._CardID, (ushort)IoPort, Convert.ToUInt32(value[0]));
                    break;
                case enIoPortType.扩展Io端口:
                    //LTDMC.dmc_get_can_state()
                    break;
                case enIoPortType.反转Io端口:
                    LTDMC.dmc_reverse_outbit(this._CardID, (ushort)IoPort, Convert.ToDouble(value[0]));
                    break;
            }
        }
        public override void ReadIoOutputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            short result = 0;
            uint count = 0;
            switch (ioPortType)
            {
                default:
                case enIoPortType.反转Io端口:
                case enIoPortType.通用Io端口:
                    result = LTDMC.dmc_read_outbit(this._CardID, (ushort)IoPort);
                    value = new object[] { (int)result };
                    break;
                case enIoPortType.高速Io端口:
                    value = new object[0];
                    break;
                case enIoPortType.计数Io端口:
                    LTDMC.dmc_get_io_count_value(this._CardID, (ushort)IoPort, ref count);
                    value = new object[] { (int)count };
                    break;
                case enIoPortType.扩展Io端口:
                    //LTDMC.dmc_get_can_state()
                    value = new object[0];
                    break;
            }
        }
        public override void ReadIoIntputBit(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            short result = 0;
            uint count = 0;
            switch (ioPortType)
            {
                default:
                case enIoPortType.反转Io端口:
                case enIoPortType.通用Io端口:
                    result = LTDMC.dmc_read_inbit(this._CardID, (ushort)IoPort);
                    value = new object[] { (int)result };
                    break;
                case enIoPortType.高速Io端口:
                    value = new object[0];
                    break;
                case enIoPortType.计数Io端口:
                    LTDMC.dmc_get_io_count_value(this._CardID, (ushort)IoPort, ref count);
                    value = new object[] { (int)count };
                    break;
                case enIoPortType.扩展Io端口:
                    value = new object[0];
                    break;
            }
        }
        public override void WriteIoOutputGroup(enIoPortType ioPortType, int IoGroup, params object[] value)
        {
            switch (ioPortType)
            {
                default:
                case enIoPortType.通用Io端口:
                    LTDMC.dmc_write_outport(this._CardID, (ushort)IoGroup, Convert.ToUInt32(value[0]));
                    break;
                case enIoPortType.高速Io端口:

                    break;
                case enIoPortType.计数Io端口:

                    break;
                case enIoPortType.扩展Io端口:
                    //LTDMC.dmc_get_can_state()
                    break;
                case enIoPortType.反转Io端口:

                    break;
            }
        }
        public override void ReadIoOutputGroup(enIoPortType ioPortType, int IoGroup, out object[] value)
        {
            uint result = 0;
            switch (ioPortType)
            {
                default:
                case enIoPortType.反转Io端口:
                case enIoPortType.通用Io端口:
                    result = LTDMC.dmc_read_outport(this._CardID, (ushort)IoGroup);
                    value = new object[] { (int)result };
                    break;
                case enIoPortType.高速Io端口:
                    value = new object[0];
                    break;
                case enIoPortType.计数Io端口:
                    value = new object[0];
                    break;
                case enIoPortType.扩展Io端口:
                    value = new object[0];
                    break;
            }
        }
        public override void ReadIoIntputGroup(enIoPortType ioPortType, int IoPort, out object[] value)
        {
            uint result = 0;
            switch (ioPortType)
            {
                default:
                case enIoPortType.反转Io端口:
                case enIoPortType.通用Io端口:
                    result = LTDMC.dmc_read_inport(this._CardID, (ushort)IoPort);
                    value = new object[] { result };
                    break;
                case enIoPortType.高速Io端口:
                    value = new object[0];
                    break;
                case enIoPortType.计数Io端口:
                    value = new object[0];
                    break;
                case enIoPortType.扩展Io端口:
                    value = new object[0];
                    break;
            }
        }

        public override bool WriteValue(enDataTypes dataType, string address, object value)
        {
            throw new NotImplementedException();
        }


        public override object ReadValue(enDataTypes dataType, string address, ushort length)
        {
            throw new NotImplementedException();
        }


        #region 方法
        private void LsLinearCompare1D(double[] cmp_point, double targetPos, double speed, CmpParam1DLowSpeed cmpParam)
        {
            double Pose = 0;
            double encoodrScale = 1;
            switch (cmpParam.cmp_Axis)
            {
                case 0:
                    encoodrScale = this.pulseFactor_x;
                    break;
                case 1:
                    encoodrScale = this.pulseFactor_y;
                    break;
                case 2:
                    encoodrScale = this.pulseFactor_z;
                    break;
            }
            // 在开始运动前为什么要先停止 ？
            LTDMC.dmc_stop(_CardID, cmpParam.cmp_Axis, 0);
            LTDMC.dmc_set_factor_error(this._CardID, cmpParam.cmp_Axis, 1, 10);
            LTDMC.dmc_set_profile_unit(this._CardID, cmpParam.cmp_Axis, 0, speed, this.Tacc, this.Tdec, 0);
            //////////////////////
            LTDMC.dmc_compare_set_config(_CardID, cmpParam.cmp_Axis, 1, cmpParam.cmp_Source);
            LTDMC.dmc_compare_clear_points(_CardID, cmpParam.cmp_Axis);
            int currentIndex = 0;
            int remaindPoint = 0;
            for (int i = 0; i < cmp_point.Length; i++)
            {
                currentIndex++;
                if (i >= 125) break;
                LTDMC.dmc_compare_add_point(_CardID, cmpParam.cmp_Axis, (int)(cmp_point[i] * encoodrScale), cmpParam.cmp_Dir, cmpParam.cmp_Action, cmpParam.cmp_IoPort);
            }
            LTDMC.dmc_pmove_unit(this._CardID, 0, Pose, 1); // 这里执行后为什么一直处理执行状态？
            // 0:表示运动
            while (LTDMC.dmc_check_done(_CardID, 0) == 0)  //判断回零运动是否完成; 
            {
                Application.DoEvents();
                /////
                LTDMC.dmc_compare_get_points_remained(this._CardID, cmpParam.cmp_Axis, ref remaindPoint);
                if (remaindPoint == 0) continue;
                if (currentIndex < cmp_point.Length)
                {
                    LTDMC.dmc_compare_add_point(_CardID, cmpParam.cmp_Axis, (int)(cmp_point[currentIndex] * encoodrScale), cmpParam.cmp_Dir, cmpParam.cmp_Action, cmpParam.cmp_IoPort);
                    currentIndex++;
                }
            }

        }
        private void HsLinearCompare1D(double[] cmp_point, double targetPos, double speed, CmpParam1DHighSpeed cmpParam)
        {
            double Pose = 0;
            double encoodrScale = 1;
            switch (cmpParam.cmp_Axis)
            {
                case 0:
                    encoodrScale = this.pulseFactor_x;
                    break;
                case 1:
                    encoodrScale = this.pulseFactor_y;
                    break;
                case 2:
                    encoodrScale = this.pulseFactor_z;
                    break;
            }
            // 在开始运动前为什么要先停止 ？
            LTDMC.dmc_stop(_CardID, cmpParam.cmp_Axis, 0);
            LTDMC.dmc_set_factor_error(this._CardID, cmpParam.cmp_Axis, 1, 10);
            LTDMC.dmc_set_profile(this._CardID, cmpParam.cmp_Axis, 0, speed, this.Tacc, this.Tdec, 0);
            //////////////////////
            LTDMC.dmc_hcmp_set_config(_CardID, cmpParam.cmp_IoPort, cmpParam.cmp_Axis, cmpParam.cmp_Source, cmpParam.cmp_logic, cmpParam.pulseWidth);
            LTDMC.dmc_hcmp_clear_points(_CardID, cmpParam.cmp_IoPort);
            int currentIndex = 0;
            int remaindPoint = 0, currentPoint = 0, runPoint = 0;
            for (int i = 0; i < cmp_point.Length; i++)
            {
                currentIndex++;
                if (i >= 127) break;
                LTDMC.dmc_hcmp_add_point(_CardID, cmpParam.cmp_IoPort, (int)(cmp_point[i] * encoodrScale));
            }
            LTDMC.dmc_pmove_unit(this._CardID, 0, Pose, 1); // 这里执行后为什么一直处理执行状态？
            // 0:表示运动
            while (LTDMC.dmc_check_done(_CardID, 0) == 0)  //判断回零运动是否完成; 
            {
                Application.DoEvents();
                //////////////////////////////
                LTDMC.dmc_hcmp_get_current_state(this._CardID, cmpParam.cmp_IoPort, ref remaindPoint, ref currentPoint, ref runPoint);
                if (remaindPoint == 0) continue;
                if (currentIndex < cmp_point.Length)
                {
                    LTDMC.dmc_hcmp_add_point(_CardID, cmpParam.cmp_IoPort, (int)(cmp_point[currentIndex] * encoodrScale));
                    currentIndex++;
                }
            }


        }
        private void HsLinearComparator2D(double[] cmpPoint_x, double[] cmpPoint_y, double[] targetPos, double speed, CmpParam2DHighSpeed cmpParam)
        {
            short iret = LTDMC.dmc_hcmp_2d_set_enable(this._CardID, cmpParam.hcmp, 1); //二维高速位置比较功能使能
            iret = LTDMC.dmc_hcmp_2d_clear_points(this._CardID, cmpParam.hcmp);//清除比较点
            iret = LTDMC.dmc_hcmp_2d_set_config(this._CardID, cmpParam.hcmp, cmpParam.cmp_mode, cmpParam.cmp_Axis1, cmpParam.cmp_Source1, cmpParam.cmp_Axis2,
            cmpParam.cmp_Source2, cmpParam.cmp_error, cmpParam.cmp_logic, cmpParam.pulseWidth, cmpParam.pwm_enable, cmpParam.pwm_duty, cmpParam.pwm_freq, (ushort)cmpParam.cmp_IoPort, cmpParam.pwm_number);
            //配置二维高速位置比较比较器
            int currentIndex = 0;
            int remaindPoint = 0, x_currentPoint = 0, y_currentPoint = 0, runPoint = 0;
            ushort currentState = 0;
            for (int i = 0; i < cmpPoint_x.Length; i++)
            {
                currentIndex++;
                LTDMC.dmc_hcmp_2d_get_current_state(this._CardID, 0, ref remaindPoint, ref x_currentPoint, ref y_currentPoint, ref runPoint, ref currentState);
                if (remaindPoint == 0) break;
                LTDMC.dmc_hcmp_2d_add_point(this._CardID, 0, (int)(this.pulseFactor_x * cmpPoint_x[i]), (int)(this.pulseFactor_y * cmpPoint_y[i]));
            }
            LTDMC.dmc_stop(_CardID, cmpParam.cmp_Axis1, 0);
            LTDMC.dmc_set_factor_error(this._CardID, cmpParam.cmp_Axis1, 1, this.factorError);
            LTDMC.dmc_stop(_CardID, cmpParam.cmp_Axis2, 0);
            LTDMC.dmc_set_factor_error(this._CardID, cmpParam.cmp_Axis2, 1, this.factorError);
            iret = LTDMC.dmc_set_vector_profile_unit(this._CardID, cmpParam.coordSys, 0, speed, this.Tacc, this.Tdec, 0);
            //设置插补运动速度曲线
            iret = LTDMC.dmc_line_unit(this._CardID, cmpParam.coordSys, 2, new ushort[] { cmpParam.cmp_Axis1, cmpParam.cmp_Axis2 }, new double[] { targetPos[0], targetPos[1] }, 1);
            while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
            {
                Application.DoEvents();
                /////////////////////////
                LTDMC.dmc_hcmp_2d_get_current_state(this._CardID, 0, ref remaindPoint, ref x_currentPoint, ref y_currentPoint, ref runPoint, ref currentState);
                if (remaindPoint == 0) continue;
                if (currentIndex < cmpPoint_x.Length)
                {
                    LTDMC.dmc_hcmp_2d_add_point(this._CardID, 0, (int)(this.pulseFactor_x * cmpPoint_x[currentIndex]), (int)(this.pulseFactor_y * cmpPoint_y[currentIndex]));
                    currentIndex++;
                }
            }
        }
        private void LsLinearComparator2D(double[] cmpPoint_x, double[] cmpPoint_y, double[] targetPos, double speed, CmpParam2DLowSpeed cmpParam)
        {
            short iret = LTDMC.dmc_compare_set_config_extern(this._CardID, 1, cmpParam.cmp_Source);//设置二维比较器
            iret = LTDMC.dmc_compare_clear_points_extern(this._CardID);//清除二维位置比较点
            //配置二维低速位置比较比较器
            int currentIndex = 0;
            int remaindPoint = 0;
            for (int i = 0; i < cmpPoint_x.Length; i++)
            {
                currentIndex++;
                LTDMC.dmc_compare_get_points_remained_extern(this._CardID, ref remaindPoint);
                if (remaindPoint == 0) break;
                LTDMC.dmc_compare_add_point_extern(this._CardID, new ushort[] { cmpParam.cmp_Axis1, cmpParam.cmp_Axis2 }, new int[2] { (int)(this.pulseFactor_x * cmpPoint_x[i]), (int)(this.pulseFactor_y * cmpPoint_y[i]) },
                    new ushort[2] { cmpParam.cmp_Dir1, cmpParam.cmp_Dir2 }, cmpParam.cmp_Action, cmpParam.cmp_IoPort);
            }
            LTDMC.dmc_stop(_CardID, cmpParam.cmp_Axis1, 0);
            LTDMC.dmc_set_factor_error(this._CardID, cmpParam.cmp_Axis1, 1, this.factorError);
            LTDMC.dmc_stop(_CardID, cmpParam.cmp_Axis2, 0);
            LTDMC.dmc_set_factor_error(this._CardID, cmpParam.cmp_Axis2, 1, this.factorError);
            iret = LTDMC.dmc_set_vector_profile_unit(this._CardID, cmpParam.coordSys, 0, speed, this.Tacc, this.Tdec, 0);
            //设置插补运动速度曲线
            iret = LTDMC.dmc_line_unit(this._CardID, cmpParam.coordSys, 2, new ushort[] { cmpParam.cmp_Axis1, cmpParam.cmp_Axis2 }, new double[] { targetPos[0], targetPos[1] }, 1);
            while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
            {
                Application.DoEvents();
                ///////////////////////////////
                LTDMC.dmc_compare_get_points_remained_extern(this._CardID, ref remaindPoint); // 获取可添加的点数
                if (remaindPoint == 0) continue;
                if (currentIndex < cmpPoint_x.Length)
                {
                    LTDMC.dmc_compare_add_point_extern(this._CardID, new ushort[] { cmpParam.cmp_Axis1, cmpParam.cmp_Axis2 }, new int[2] { (int)(this.pulseFactor_x * cmpPoint_x[currentIndex]), (int)(this.pulseFactor_y * cmpPoint_y[currentIndex]) },
                        new ushort[2] { cmpParam.cmp_Dir1, cmpParam.cmp_Dir2 }, cmpParam.cmp_Action, cmpParam.cmp_IoPort);
                    currentIndex++;
                }
            }
        }
        // 
        private void RectangleInterpolationXY(MoveCommandParam motionCommandParam)
        {
            LTDMC.dmc_stop(_CardID, 0, 0);
            LTDMC.dmc_stop(_CardID, 1, 0);
            LTDMC.dmc_set_factor_error(this._CardID, 0, 1, motionCommandParam.FactorError);
            LTDMC.dmc_set_factor_error(this._CardID, 1, 1, motionCommandParam.FactorError);
            LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, motionCommandParam.MoveSpeed, motionCommandParam.Tacc, motionCommandParam.Tdec, 0);
            //设置插补速度曲线参数，插补运动最大矢量速度 4000unit/s，加减速时间 0.1s
            LTDMC.dmc_rectangle_move_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { motionCommandParam.AxisParam.X, motionCommandParam.AxisParam.Y }, new double[] { motionCommandParam.Rect2Param.vector_x, motionCommandParam.Rect2Param.vector_y }, motionCommandParam.Rect2Param.lineCount, motionCommandParam.Rect2Param.InterpolateMode, 1);
            while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 矩形插补
        /// </summary>
        /// <param name="targetPosition">矩形对角位置</param>
        /// <param name="vector_x">矩形方向向量</param>
        /// <param name="vector_y">矩形方向向量</param>
        /// <param name="speed">插补速度</param>
        /// <param name="lineCount">矩形行数</param>
        /// <param name="InterMode">插补模式，0：逐行插补；1：渐开线插补</param>
        private void RectangleInterpolationXY(double x, double y, double vector_x, double vector_y, double speed, int lineCount, ushort InterMode)
        {
            LTDMC.dmc_stop(_CardID, 0, 0);
            LTDMC.dmc_stop(_CardID, 1, 0);
            LTDMC.dmc_set_factor_error(this._CardID, 0, 1, this.factorError);
            LTDMC.dmc_set_factor_error(this._CardID, 1, 1, this.factorError);
            LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
            //设置插补速度曲线参数，插补运动最大矢量速度 4000unit/s，加减速时间 0.1s
            LTDMC.dmc_rectangle_move_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { x, y }, new double[] { vector_x, vector_y }, lineCount, InterMode, 1);
            while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// XY平面螺旋线插补
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="center_x"></param>
        /// <param name="center_y"></param>
        /// <param name="targetPos_x"></param>
        /// <param name="targetPos_y"></param>
        /// <param name="speed"></param>
        /// <param name="circleCount"></param>
        private void SpiralLineInterpolationXY(enAxisName axisName, double center_x, double center_y, double targetPos_x, double targetPos_y, double speed, ushort circleDir, int circleCount)
        {
            LTDMC.dmc_stop(_CardID, 0, 0);
            LTDMC.dmc_stop(_CardID, 1, 0);
            //////////////////////////////
            LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0);
            LTDMC.dmc_arc_move_center_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { targetPos_x, targetPos_y }, new double[] { center_x, center_y }, circleDir, circleCount, 1); //执行 X、Y 轴圆弧插补运动,
            while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)      //判断坐标系运动状态，等待运动完成
            {
                Application.DoEvents();
            }
        }
       
        /// <summary>
        /// XYZ螺旋线插补
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="center_x"></param>
        /// <param name="center_y"></param>
        /// <param name="center_z"></param>
        /// <param name="targetPos_x"></param>
        /// <param name="targetPos_y"></param>
        /// <param name="targetPos_z"></param>
        /// <param name="speed"></param>
        /// <param name="circleCount"></param>
        private void SpiralLineInterpolationXYZ(enAxisName axisName, double center_x, double center_y, double center_z, double targetPos_x, double targetPos_y, double targetPos_z, double speed, int circleCount)
        {
            LTDMC.dmc_stop(_CardID, 0, 0);
            LTDMC.dmc_stop(_CardID, 1, 0);
            LTDMC.dmc_stop(_CardID, 2, 0);
            //////////////////////////////
            LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0); // 设置插补运动速度曲线
            LTDMC.dmc_arc_move_center_unit(this._CardID, 0, 3, new ushort[] { 0, 1, 2 }, new double[] { targetPos_x, targetPos_y, targetPos_z }, new double[] { center_x, center_y, center_z }, 1, circleCount, 1); //执行 X、Y 轴圆弧插补运动,
            while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)  //判断坐标系运动状态，等待运动完成
            {
                Application.DoEvents();
            }
        }
        /// <summary>
        /// XY平面圆弧插补
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="center_x"></param>
        /// <param name="center_y"></param>
        /// <param name="targetPos_x"></param>
        /// <param name="targetPos_y"></param>
        /// <param name="speed"></param>
        /// <param name="circleCount"></param>
        private void CircleInterpolationXY(enAxisName axisName, double center_x, double center_y, double targetPos_x, double targetPos_y, double speed, int circleCount)
        {
            LTDMC.dmc_stop(_CardID, 0, 0);
            LTDMC.dmc_stop(_CardID, 1, 0);
            LTDMC.dmc_set_vector_profile_unit(this._CardID, 0, 0, speed, this.Tacc, this.Tdec, 0); //设置插补运动速度曲线 }
            ///////////////////
            LTDMC.dmc_arc_move_center_unit(this._CardID, 0, 2, new ushort[] { 0, 1 }, new double[] { targetPos_x, targetPos_y }, new double[] { center_x, center_y }, 1, 1, 1); //执行 X、Y 轴圆弧插补运动+ axisPosition[3]
            while (LTDMC.dmc_check_done_multicoor(this._CardID, 0) == 0)             //判断坐标系运动状态，等待运动完成
            {
                Application.DoEvents();
            }
        }








        #endregion

        public struct CmpParam1DLowSpeed
        {
            public ushort cmp_Axis; // 比较轴
            public ushort cmp_Dir;  // 比较方向- 0：小于等于，1：大于等于
            public ushort cmp_Action; // 比较IO口的输出方式
            public uint cmp_IoPort; // 比较输出的端口
            public ushort cmp_Source; //0:指令位置 ;1:反馈位置
        }
        public struct CmpParam1DHighSpeed
        {
            public ushort cmp_Axis; // 比较轴
            public ushort cmp_mode;  // 比较模式：0：禁止（默认值）;1：等于;2：小于;3：大于;4：队列，提供 127 个点比较空间;5：线性，提供起始比较点，位置增量，比较次数
            public ushort cmp_IoPort; // 高速比较器，取值范围：0~3（对应硬件 CMP0~CMP3 端口）
            public ushort cmp_Source; //0:指令位置 ;1:反馈位置
            public ushort cmp_logic; // 有效电平：0：低电平，1：高电平
            public int pulseWidth; //输出脉冲宽度：200us
        }
        public struct CmpParam2DLowSpeed
        {

            public ushort coordSys; // 坐标系号
            public ushort cmp_Axis1; // 比较轴1
            public ushort cmp_Axis2;// 比较轴2
            public ushort cmp_Source; // 比较源：0:指令位置;1:反馈位置
            public ushort cmp_Dir1; // 比较方向，0：小于等于，1：大于等于
            public ushort cmp_Dir2; // 比较方向，0：小于等于，1：大于等于
            public ushort cmp_Action;  // 比较IO口的输出方式；3：触发时 io 取反
            public uint cmp_IoPort; // 比较输出的端口
        }

        public struct CmpParam2DHighSpeed
        {
            public ushort coordSys;  //坐标系号
            public ushort cmp_Axis1; // 比较轴号
            public ushort cmp_Axis2; // 比较轴号
            public uint cmp_IoPort;  // 比较输出的端口
            public ushort hcmp; // 保留参参数，默认为0
            public ushort cmp_mode;  // 比较模式：0：进入误差带后触发;1：进入误差带单轴等于后再触发
            public ushort cmp_Source1; //比较源：0:指令位置;1:反馈位置
            public ushort cmp_Source2; //比较源：0:指令位置;1:反馈位置
            public int cmp_error; //pulsex/y 轴误差带设置，单位：pulse
            public ushort cmp_logic; // 有效电平：0：低电平，1：高电平
            public int pulseWidth; //输出脉冲宽度：200us
            public ushort pwm_enable; //PWM 输出使能：0:不使能;1:使能
            public double pwm_duty; //PWM 输出占空比
            public int pwm_freq; //PWM 输出频率
            public ushort pwm_number; //输出 PWM 脉冲个数
        }






    }



}
