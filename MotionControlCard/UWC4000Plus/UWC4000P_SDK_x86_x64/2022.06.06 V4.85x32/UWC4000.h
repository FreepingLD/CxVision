//Uinon Win Control Tech
#ifndef _UWCTECH_
#define _UWCTECH_

#define __UWC4000_EXPORTS


//Axis ID define
#define   Axis_X			0	
#define   Axis_Y			1	
#define   Axis_Z			2
#define   Axis_U			3


//HC2的按键码
#define	KEYBOARD_KEY_MASK_XLOCK				0x0001
#define	KEYBOARD_KEY_MASK_YLOCK				0x0002
#define	KEYBOARD_KEY_MASK_ZLOCK				0x0004
#define	KEYBOARD_KEY_MASK_DELETE_POINT		0x0008
#define	KEYBOARD_KEY_MASK_HI_LOW			0x0010
#define	KEYBOARD_KEY_MASK_GOTO				0x0020
#define	KEYBOARD_KEY_MASK_FINISH			0x0040
#define	KEYBOARD_KEY_MASK_OperateObject		0x0080
#define	KEYBOARD_KEY_MASK_PROBE_OVERRIDE	0x0100
#define	KEYBOARD_KEY_MASK_SERVO_ON			0x0200
#define	KEYBOARD_KEY_MASK_Cordinate			0x0400
#define	KEYBOARD_KEY_MASK_F1				0x0800
#define	KEYBOARD_KEY_MASK_F2				0x1000
#define	KEYBOARD_KEY_MASK_F3				0x2000
#define KEYBOARD_KEY_MASK_LOCATION			0x4000
#define	KEYBOARD_KEY_MASK_F4				0x8000
//HC2 LED指示灯定义
#define	KEYBOARD_LED_MASK_XLOCK				0x00000001
#define	KEYBOARD_LED_MASK_YLOCK				0x00000002
#define	KEYBOARD_LED_MASK_ZLOCK				0x00000004
#define KEYBOARD_LED_MASK_DELETE_POINT		0x00000008
#define KEYBOARD_LED_MASK_CoordMachine		0x00000010
#define KEYBOARD_LED_MASK_CoordWorkpice		0x00000020
#define KEYBOARD_LED_MASK_GoTo				0x00000040
#define KEYBOARD_LED_MASK_Finish			0x00000080
#define KEYBOARD_LED_MASK_OperateMachine	0x00000100
#define KEYBOARD_LED_MASK_OperateProbeHead  0x00000200
#define KEYBOARD_LED_MASK_OperateLift		0x00000400
#define KEYBOARD_LED_MASK_OperateZoom		0x00000800
#define	KEYBOARD_LED_MASK_PROBE_OVERRIDE	0x00001000
#define	KEYBOARD_LED_MASK_PROBE_ENABLE		0x00002000
#define	KEYBOARD_LED_MASK_ProbeBeep			0x00004000
#define	KEYBOARD_LED_MASK_SERVE_ON			0x00008000
#define	KEYBOARD_LED_MASK_LOCATION_1		0x00010000
#define	KEYBOARD_LED_MASK_LOCATION_2		0x00020000
#define	KEYBOARD_LED_MASK_LOCATION_4		0x00040000
#define	KEYBOARD_LED_MASK_LOCATION_3		0x00080000
#define	KEYBOARD_LED_MASK_F5        		0x00100000
#define	KEYBOARD_LED_MASK_F1        		0x00200000
#define	KEYBOARD_LED_MASK_F6        		0x00400000
#define	KEYBOARD_LED_MASK_F2        		0x00800000
#define	KEYBOARD_LED_MASK_F7        		0x01000000
#define	KEYBOARD_LED_MASK_F3        		0x02000000
#define	KEYBOARD_LED_MASK_F8        		0x04000000
#define	KEYBOARD_LED_MASK_F4        		0x08000000
#define	KEYBOARD_LED_MASK_Shift      		0x10000000
#define	KEYBOARD_LED_MASK_HighSpeed  		0x20000000
#define	KEYBOARD_LED_MASK_LowSpeed   		0x40000000
#define	KEYBOARD_LED_MASK_KeyBeep   		0x80000000


//Axis status flag define and mask value( example: if(axis_status[Axis_X] & IsStoped){stoped}else{not}

//Axis status flag mask
#define		Bridge_ON				(0x00000001)
#define		CurntClose				(0x00000002)
#define		SpeedClose				(0x00000004)
#define		PositionClose			(0x00000008)
#define		ReadHeadDisconnection	(0x00000010)
#define		PosLimit				(0x00000020)
#define		NegLimit				(0x00000040)
#define		DiagnosedFlag			(0x00000080)
#define		DriverAlarm				(0x00000100)
#define		MotorDisconnection		(0x00000200)
#define		IsStoped				(0x00000400)
#define		IsHomed					(0x00000800)
#define		SoftPoslimit			(0x00001000)
#define		SoftNeglimit			(0x00002000)
#define		ReadHeadAlarm			(0x00004000)
#define		FolowErr				(0x00008000)

//Global status flag mask
  #define   sysAllStoped            (0x00000001)  //0
  #define   sysNewErr               (0x00000002)  //1
//  #define   sysJoyCombackPosActive  (0x00000004)  //2
  #define   sysHandControlActive	    (0x00000008)  //3
  #define   sysXMoveStauts          (0x00000010)  //4
  #define   sysYMoveStauts          (0x00000020)  //5
  #define   sysZMoveStauts          (0x00000040)  //6
  #define   sysUMoveStauts          (0x00000080)  //7
  
  #define   sysProbeEnable		      (0x00000200)  //9
  #define   sysHCEStopTrigred		    (0x00000400)  //10
  #define   sysTouchMoveFailed      (0x00000800)  //11
  #define   sysMachineEStopTrigred         (0x00001000)  //12
  #define   sysProbeInPosition      (0x00002000)  //13
  #define   sysProbeFault           (0x00004000)  //14
  #define   sysAutoTouchMoving      (0x00008000)  //15
  
  #define   sysProbeAbnormalTrig    (0x00040000)  //18

#ifdef __UWC4000_EXPORTS
#define UWC4000_API __declspec(dllexport)
#else
#define UWC4000_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif	

//初始化操作///////////////////////////////////////////////////////////////////////////
//与uwc4000建立连接,并初始化控制器参数
UWC4000_API int __stdcall uwc4000_initial(void);

//断开与控制器的数据连接
UWC4000_API int __stdcall uwc4000_close(void);

//读取计数模块状态字 用法简明参考:unsigned int status[4];uwc4000_get_count_status(status);
UWC4000_API int __stdcall uwc4000_get_count_status(unsigned int *status);

//读取运动模块状态字 用法简明参考:unsigned int status;uwc4000_get_motion_status(&status);
UWC4000_API int __stdcall uwc4000_get_motion_status(unsigned int *status);

//读各轴状态 用法简明参考:unsigned int status[4];uwc4000_get_axis_status(status);
UWC4000_API int __stdcall uwc4000_get_axis_status(unsigned int *status);

//读取调机软件设定的各轴最大速度和加速度
UWC4000_API int __stdcall uwc4000_get_profile(double *acc_time,double *max_speed);

//读取光栅尺位置 用法简明参考:double x_scale,double y_scale,double z_scale;uwc4000_get_scale(&x_scale,&y_scale,&z_scale)
UWC4000_API int __stdcall uwc4000_get_scale(double *x_scale,double *y_scale,double *z_scale);

//读取4路光栅尺位置
UWC4000_API int __stdcall uwc4000_get_all_scale(double *scale_pos);

//读取各轴脉冲计数器
UWC4000_API int __stdcall uwc4000_get_pulse_count(long *count);

//设置光源亮度 用法简明参考:unsigned int light[44];uwc4000_set_light(light);
UWC4000_API int __stdcall uwc4000_set_light(unsigned int *light);

//读取操纵杆信息 用法简明参考:int Message[5];uwc4000_get_joystick_Msg(Message);
UWC4000_API int __stdcall uwc4000_get_joystick_Msg(int *message);

//回原点运动 用法简明参考:uwc4000_go_home(0,10);//x轴正向回原点
UWC4000_API int __stdcall uwc4000_go_home(int axis,double search_speed);

//单轴点位运动 用法简明参考:uwc4000_single_move_to(0,10000,10);//x轴以最大速度定位到距离原点10mm处
UWC4000_API int __stdcall uwc4000_single_move_to(int axis,double speed,double target_pos);

//相对运动方式单轴点位运动
UWC4000_API int __stdcall uwc4000_single_move_to_rel(int axis,double speed,double pos_rel);

//XY直线插补到指定位置 用法简明参考:double target[2]={10,20};uwc4000_XY_move_to(10000,target);//xy轴以最大速度采用直线插补方式定位到距离原点X=10mm,Y=20mm处
UWC4000_API int __stdcall uwc4000_XY_move_to(double speed,double *target_pos);

//XY直线插补到指定相对坐标位置
UWC4000_API int __stdcall uwc4000_XY_move_to_rel(double speed,double *pos_rel);

//XYZ直线插补到指定位置 用法简明参考:double target_pos[4]={10,10,10,0};uwc4000_XYZ_move_to(10000,target_pos);X/Y/Z以最大速度直线插补定位到10,10,10
UWC4000_API int __stdcall uwc4000_XYZ_move_to(double speed,double *target_pos);

//XYZ直线插补到指定相对坐标位置
UWC4000_API int __stdcall uwc4000_XYZ_move_to_rel(double speed,double *pos_rel);

//指定轴直线插补运动
UWC4000_API int __stdcall uwc4000_linear_move_to(long *axis_array,double speed,double *target_pos);

//在XOY平面作圆弧插补运动,以line_speed为线速度, x_center,y_center为圆心相对起点的相对坐标位置,angle为旋转角度:-360--+360
UWC4000_API int __stdcall uwc4000_arc_move_xy(double x_center, double y_center, double line_speed, double angle);

//在YOZ平面作圆弧插补运动,以line_speed为线速度, y_center,z_center为圆心相对起点的相对坐标位置,angle为旋转角度:-360--+360
UWC4000_API int __stdcall uwc4000_arc_move_yz(double y_center, double z_center, double line_speed, double angle);

//在ZOX平面作圆弧插补运动,以line_speed为线速度, z_center,x_center为圆心相对起点的相对坐标位置,angle为旋转角度:-360--+360
UWC4000_API int __stdcall uwc4000_arc_move_zx(double z_center, double x_center, double line_speed, double angle);

//示教运动启动 用法简明参考:uwc4000_jog_start(0,30);//x轴启动JOG运动,此后用uwc4000_change_speed变速范围为-30~30mm/s,变速细腻度800级
UWC4000_API int __stdcall uwc4000_jog_start(int axis,double jog_speed_max);

//示教运动变速 用法简明参考:uwc4000_change_speed(0,-3);//假如X轴此前已jog_start,令x轴jog速度改变,使X轴负向运动,速度为-3mm/s
UWC4000_API int __stdcall uwc4000_change_speed(int axis,double curnt_speed);

//停止示教运动 用法简明参考:uwc4000_jog_stop();//停下所有轴的jog运动,假如没有轴处于jog运动状态调用jog_stop无副作用
UWC4000_API int __stdcall uwc4000_jog_stop(void);

//禁止操纵杆控制 用法简明参考:uwc4000_disable_joystick();
UWC4000_API int __stdcall uwc4000_disable_joystick(void);

//允许操纵杆控制,同时指定各轴高/低档速设置
//用法简明参考:double low_speed[4]={5,5,5,5},high_speed[4]={50,50,50,50};//摇杆高速状态时,扳动摇杆到最大角度速度为50mm/s,低速状态时,扳动到最大角度速度为5mm/s
UWC4000_API int __stdcall uwc4000_enable_joystick(double *low_speed,double *high_speed);

//读取操纵杆方式探针采点反弹完毕瞬间光栅尺坐标位置
UWC4000_API int __stdcall uwc4000_get_joy_comeback_position(double *scale_pos);

//停车指令 用法简明参考:uwc4000_stop(0,0);//减速停止X轴
UWC4000_API int __stdcall uwc4000_stop(int axis,int mode);

//设置各轴定位精度,定位精度设置不低于2微米 用法简明参考:double precision[4]={0.005,0,005,0.002,0.005};uwc4000_set_inposition_precision(precision);//x:5微米,y:5微米,z:2微米
UWC4000_API int __stdcall uwc4000_set_inposition_precision(double *precision);

//探针功能设置 用法简明参考:uwc4000_set_motion_probe(0x0001 | 0x0002);//0x0001使能探针,0x0000忽略探针,0x0002renishaw//TESA
UWC4000_API int __stdcall uwc4000_set_motion_probe(unsigned int probe_set);

//设置摇杆采点回退参数 用法简明参考:uwc4000_set_joystick_comeback(20,3);//摇杆采点停止后以20mm/s速度回退到距离触发点3mm位置
UWC4000_API int  __stdcall uwc4000_set_joystick_comeback(double comeback_speed,double comeback_distance);

//探针采点运动指令 用法简明参考:double target_pos[4]={10,10,10,0};uwc4000_touch_move(5,20,10,target_pos);//以5mm/s合成速度朝向坐标10,10,10搜索工件,如果搜索到就以20mm/s速度回退到该函数调用点;如果搜索不到,搜索超出距离10,10,10位置5mm后停下来.
UWC4000_API int __stdcall uwc4000_touch_move(double touch_speed,double comeback_speed,double search_radius,double *target_pos);

//读取探针锁存值 用法简明参考:double x_scale,y_scale,z_scale;uwc4000_get_probe_capture(&x_scale,&y_scale,&z_scale);
UWC4000_API int __stdcall uwc4000_get_probe_capture(double *x_scale,double *y_scale,double *z_scale);

//读取探针触发矢量
UWC4000_API int __stdcall uwc4000_get_probe_capture_ijk(double *x_scale, double *y_scale, double *z_scale, double *u_scale, double *i, double *j, double *k);

//读通用输入口状态
UWC4000_API int __stdcall uwc4000_get_input(unsigned int *input);

//写通用输出口状态
UWC4000_API int __stdcall uwc4000_set_output(unsigned int output);

//读输出口状态
UWC4000_API int __stdcall uwc4000_get_output(unsigned int *output);

//读取uwc4000功能配置符
UWC4000_API int __stdcall uwc4000_get_function_describe(unsigned int *function_describe);

//读取各轴光学尺分辩率
UWC4000_API int __stdcall uwc4000_get_scale_resolution(double *resolution);

//重置三轴光栅尺位置
UWC4000_API int __stdcall uwc4000_set_scale(double x_scale,double y_scale,double z_scale);

//按轴重置光栅尺位置, 可重置XYZU四轴
UWC4000_API int __stdcall uwc4000_set_scale2(int axis, double scale_pos);

//读取软件限位坐标(相对于机械零位)
UWC4000_API int __stdcall uwc4000_get_soft_limit(int axis,double *pos_limit,double *neg_limit);

//读取各轴最大速度设置 参考
UWC4000_API int __stdcall uwc4000_get_max_speed(double *max_speed);

//读取定位精度（定位完成窗口）设置
UWC4000_API int __stdcall uwc4000_get_inposition_precision(double *precision);

//读取固件程序版本
UWC4000_API int __stdcall uwc4000_get_firmware(char *firmware);

//读取最后一个出错信息代码
UWC4000_API int __stdcall uwc4000_get_last_err(unsigned int *err_code);

//超级命令
UWC4000_API int __stdcall uwc4000_super_command(unsigned int ch,int Slen,int Rlen,unsigned char *ucIn,unsigned char *ucOut);

//获取控制器内系统时间
UWC4000_API int __stdcall uwc4000_get_time(int *year, int *month, int *day, int *hour, int *minute, int *second);

//发送经误差补偿后的坐标到HC操纵杆显示屏
UWC4000_API int __stdcall uwc4000_send_coordinate_to_HC(double *coordinate);

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
以下函数为老版本兼容或测试目的而保留，大部分功能已无效，请勿直接使用，如果使用需要，请与UWC技术支持联系 ljs1384@163.com  TEL:13510564386
推荐使用uwc4000_super_command实现以上API未能穷举的操作，请将技术需求发送到以上邮件，UWC将提供一段基于super_command封装的代码来实现
*/
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//读取DSP数据观测器内容
UWC4000_API int  __stdcall uwc4000_get_MDSP_param(int number,long *param);

//设置3D测量各轴定位精度,定位精度设置不低于5微米,默认50um, 参考uwc4000_set_inposition_precision
UWC4000_API int __stdcall uwc4000_set_inposition_precision_3D(double *precision);

//打包读取控制器状态信息,只需要一次通讯即可读取绝大部分常用信息:各轴状态,运动模块状态,计数模块状态,光栅尺坐标,异常代码,操纵盒键盘状态,通用输入口状态
//而该函数执行时间与只读取单个信息函数执行时间相等, 参考uwc4000_get_count_status uwc4000_get_motion_status uwc4000_get_axis_status uwc4000_get_profile uwc4000_get_scale
UWC4000_API long __stdcall uwc4000_get_controller_msg(unsigned long *ulStatus,double *dScalePosition);

//加密功能已去掉，函数保留
//保存加密字符串
UWC4000_API int __stdcall uwc4000_set_security_string(int security_number,char *string);
//运行加密算法
UWC4000_API int __stdcall uwc4000_run_security_algorithmic(int security_number,float *param);

//读取保存在UWC4000.INI文件中的机器型号
UWC4000_API int __stdcall uwc4000_get_machine_type(char *string);

//读取mof锁存值
UWC4000_API int __stdcall uwc4000_get_MOF_capture(double *x_scale,double *y_scale,double *z_scale);

//设置最大跟随误差
UWC4000_API int __stdcall uwc4000_set_follow_err(unsigned int *follow_err,int active1_save0);

//设置最大跟随误差
UWC4000_API int __stdcall uwc4000_get_follow_err(unsigned int *follow_err);

//读取操纵杆adc转换结果, 参考uwc4000_get_joystick_Msg
UWC4000_API int __stdcall uwc4000_get_joystick_ADC(int *offset);

//复位uwc4000，该功能已去掉，函数保留
UWC4000_API int __stdcall uwc4000_reset(void);

//设置光栅尺分辩率
UWC4000_API int __stdcall uwc4000_set_scale_resolution(double *resolution);

//读取UWC4000 ID号
UWC4000_API int __stdcall uwc4000_get_uwc4000_ID(long *ID);

//读取joystick ID号
UWC4000_API int __stdcall uwc4000_get_joystick_ID(long *ID);

//设置各轴光栅尺RI特性
UWC4000_API int __stdcall uwc4000_set_RI_mode(int *RI,int active1_save0);

//读取各轴光栅尺RI特性
UWC4000_API int __stdcall uwc4000_get_RI_mode(int *RI);

//保存UWC4000 ID号
UWC4000_API int __stdcall uwc4000_set_uwc4000_ID(long ID);

//保存joystick ID号
UWC4000_API int __stdcall uwc4000_set_joystick_ID(long ID);

//保存joystick 矫正信息
UWC4000_API int __stdcall uwc4000_set_joystick_calibration(int *data_array,int active1_save0);

//读取joystick 矫正信息
UWC4000_API int __stdcall uwc4000_get_joystick_calibration(int *data_array);

//设置各轴脉冲/光栅计数比例
UWC4000_API int __stdcall uwc4000_set_pulse_scale_map(long *pulse,long *scale,int active1_save0);

//读取各轴脉冲/光栅计数比例
UWC4000_API int __stdcall uwc4000_get_pulse_scale_map(long *pulse,long *scale);

//设置各轴最大速度和加速度
UWC4000_API int __stdcall uwc4000_set_profile(double *acc_time,double *max_speed,int active1_save0);

//保存uwc4000功能配置符
UWC4000_API int __stdcall uwc4000_set_function_describe(unsigned int *function_describe);

//保存光栅尺计数方向设置
UWC4000_API int __stdcall uwc4000_set_count_dir(unsigned int *dir,int active1_save0);

//读取光栅尺计数方向设置
UWC4000_API int __stdcall uwc4000_get_count_dir(unsigned int *dir);

//保存各轴专用i/o配置
UWC4000_API int __stdcall uwc4000_set_IO_config(unsigned int *config,int active1_save0);

//读取各轴专用i/o配置
UWC4000_API int __stdcall uwc4000_get_IO_config(unsigned int *config);

//保存机器型号
UWC4000_API int __stdcall uwc4000_set_machine_type(char *string);

//设置各轴脉冲方向信号
UWC4000_API int __stdcall uwc4000_set_pulse_dir(unsigned int *dir,int active1_save0);

//读取脉冲方向设置
UWC4000_API int __stdcall uwc4000_get_pulse_dir(unsigned int *dir);

//设置各轴PID,Kvff参数
UWC4000_API int __stdcall uwc4000_set_PID(float *Kp,float *Ki,float *Kd,float *Kvff,int active1_save0);

//读取各轴PID,Kvff参数
UWC4000_API int __stdcall uwc4000_get_PID(float *Kp,float *Ki,float *Kd,float *Kvff);

//设置探针常开、常闭类型，探针蜂鸣器持续时间  参考uwc4000_set_motion_probe
UWC4000_API int __stdcall uwc4000_set_probe(int enable,int led_logic,int buzzer_time);

//设置探针信号去抖动时间
UWC4000_API int __stdcall uwc4000_set_trig_debounce(int time);

//设置触发信号去抖动时间
UWC4000_API int __stdcall uwc4000_get_trig_debounce(int *time);

//RI_function:0-无作用,1-清零1次, gohome函数封装了这一操作
UWC4000_API int __stdcall uwc4000_set_RI_function(unsigned int axis,int function);

//读通讯模块出错处理信息, DLL内部操作, 参考 uwc4000_get_last_err
UWC4000_API int __stdcall uwc4000_get_err_msg(int *err_msg);

//设置0-2轴定位方式, 新版本控制器由调试软件设定
UWC4000_API int __stdcall uwc4000_set_inposition_mode(int axis,int mode);

//保存0-2轴定位模式设置到ini文件
UWC4000_API int __stdcall uwc4000_set_inposition_mode_save(int axis,int mode);

//读取0-2轴定位模式设置
UWC4000_API int __stdcall uwc4000_get_inposition_mode(int axis,int *mode);

//清除轴状态寄存器
UWC4000_API int __stdcall uwc4000_clear_status(int axis);

//设置各轴脉冲计数器
UWC4000_API int __stdcall uwc4000_set_pulse_count(long *count);

//读取指定轴pid整定信息
UWC4000_API int __stdcall uwc4000_get_PID_tunning(int axis,double *data);

//使能脉冲输出电路
UWC4000_API int __stdcall uwc4000_enable_pulse_output(unsigned int enable);

//读取控制器电源电压
UWC4000_API int __stdcall uwc4000_get_power_voltage(double *voltage);

//使能zoom轴
UWC4000_API int __stdcall uwc4000_zoom(int enable);

//软件限位功能(回原点后有效), 软件限位由调试软件自动诊断设定, 应用软件可读不可设, 参考uwc4000_get_soft_limit
UWC4000_API int __stdcall uwc4000_set_soft_limit(int axis,double pos_limit,double neg_limit);

//保存软限位到ini文件
UWC4000_API int __stdcall uwc4000_set_soft_limit_save(int axis,double pos_limit,double neg_limit);


//清除错误标记, 参考uwc4000_get_last_err
UWC4000_API int  __stdcall uwc4000_clear_motion_err(int ID);

//清除自动探测错误标记 参考uwc4000_get_last_err
UWC4000_API int __stdcall uwc4000_clear_auto_touch_err(void);

//读取脚踏开关锁存值 使用探针功能实现脚踏开关采点功能, 参考uwc4000_get_probe_capture
UWC4000_API int __stdcall uwc4000_get_footswitch_capture(double *x_scale,double *y_scale,double *z_scale);

//清除DSP数据观测器内容
UWC4000_API int  __stdcall uwc4000_clear_MDSP_param(int number);

//设置2D测量定位完成时间
UWC4000_API int __stdcall uwc4000_set_INP_time(double time);

//读取2D测量定位完成时间
UWC4000_API int __stdcall uwc4000_get_INP_time(double *time);

//使能、禁止操作错误日志 该操作通过控制器调试软件进行
UWC4000_API int __stdcall uwc4000_enable_Error_Log(bool enable1_disable0);

//设置碰限位开关刹车距离
UWC4000_API int __stdcall uwc4000_set_limit_brake_distance(unsigned int *distance,int active1_save0);

//读取各轴限位刹车距离
UWC4000_API int __stdcall uwc4000_get_limit_brake_distance(unsigned int *distance);

//设置探针触发刹车距离
UWC4000_API int __stdcall uwc4000_set_probe_brake_distance(double distance,int active1_save0);

//清除轴状态字中的错误信息
UWC4000_API int __stdcall uwc4000_clear_status(int axis);

//读取探针触发刹车距离
UWC4000_API int __stdcall uwc4000_get_probe_brake_distance(double *distance);

//读取摇杆探测标记
UWC4000_API int __stdcall uwc4000_get_joystick_touch_flag(unsigned int *flag);

//读取自动探测标记
UWC4000_API int __stdcall uwc4000_get_auto_touch_flag(unsigned int *flag);

//读取光源各通道最大电流值
UWC4000_API int __stdcall uwc4000_get_light_current(unsigned int *current);

//设置测量模式,0-2D测量,1-3D测量
UWC4000_API int __stdcall uwc4000_set_measure_mode(unsigned int un3D1_2D0);


//设置3D测量定位时间
UWC4000_API int __stdcall uwc4000_set_INP_time_3D(double time);

//读取3D测量定位时间
UWC4000_API int __stdcall uwc4000_get_INP_time_3D(double *time);

//读取DLL版本号
UWC4000_API int __stdcall uwc4000_get_dll_version(char *ver);

//设置指定轴软件减速点坐标
UWC4000_API int __stdcall uwc4000_set_SD_position(int axis,double positive_position,double negtive_position,int active1_save0);

//读取指定轴软件减速点坐标
UWC4000_API int __stdcall uwc4000_get_SD_position(int axis,double *positive_position,double *negtive_position);

//设置指定轴过软件减速点低速度
UWC4000_API int __stdcall uwc4000_set_SD_speed(int axis,double low_speed,int active1_save0);

//读取指定轴过软件减速点低速度
UWC4000_API int __stdcall uwc4000_get_SD_speed(int axis,double *low_speed);

//设置通用输出端口状态
//port_ch=1-3,status=0-65535
UWC4000_API int __stdcall uwc4000_set_GP_outport(int port_ch,unsigned short status);

UWC4000_API int __stdcall uwc4000_get_GP_outport(int port_ch,unsigned short *status);

//读取通用输入端口状态
//port_ch=1-3,status=0-65535
UWC4000_API int __stdcall uwc4000_get_GP_inport(int port_ch,unsigned short *status);

//设置控制面板类型
//0-OTP6兼容(选择控制器内置操纵杆数据采集卡)
//1-OTP7兼容(选择外置操纵盒)
//2-客制化外置操纵盒 
UWC4000_API int __stdcall uwc4000_set_control_panel_type(int type,int active1_save0);

UWC4000_API int __stdcall uwc4000_get_control_panel_type(int *type);

//设置速度曲线中S曲线段占总加减速时间的比例
//scale=2-50,代表S曲线段占总加减速时间的1/2~1/50,scale数值越大越接近梯形速度曲线效果
UWC4000_API int __stdcall uwc4000_set_profile_ts(int axis,int scale,int active1_save0);

UWC4000_API int __stdcall uwc4000_get_profile_ts(int axis,int *scale);

//设置各轴设计最大速度
UWC4000_API int __stdcall uwc4000_set_design_max_speed(double *design_max_speed,int active1_save0);

//读取各轴设计最大速度
UWC4000_API int __stdcall uwc4000_get_design_max_speed(double *design_max_speed);

//设置指定轴零漂 
UWC4000_API int __stdcall uwc4000_set_offset(int axis,int offset,int active1_save0);

//读取指定轴零漂 
UWC4000_API int __stdcall uwc4000_get_offset(int axis,int *offset);

//设置开环控制输出电压
UWC4000_API int __stdcall uwc4000_set_output_voltage(unsigned int axis,double voltage);

//设置PID控制积分项饱和限制
UWC4000_API int __stdcall uwc4000_set_sum_max(unsigned int axis,long sum_max,int active1_save0);

//读取PID控制积分项饱和限制
UWC4000_API int __stdcall uwc4000_get_sum_max(unsigned int axis,long *sum_max);

//保存2D定位时间设置到ini文件
UWC4000_API int __stdcall uwc4000_set_INP_time_save(double time);

//保存3D定位时间设置到ini文件
UWC4000_API int __stdcall uwc4000_set_INP_time_3D_save(double time);

//保存2D测量定位范围设置到ini文件
UWC4000_API int __stdcall uwc4000_set_inposition_precision_save(double *precision);

//保存3D测量定位范围设置到ini文件
UWC4000_API int __stdcall uwc4000_set_inposition_precision_3D_save(double *precision);

//保存各轴光栅尺分辩率到ini文件,单位毫米
UWC4000_API int __stdcall uwc4000_set_scale_resolution_save(double *resolution);

//伺服驱动器使能/禁止,1-使能,0-禁止
UWC4000_API int __stdcall uwc4000_servo_enable(unsigned int axis,unsigned int enable1_disable0);

//设置开环/闭环控制方式
UWC4000_API int __stdcall uwc4000_close_loop(unsigned int axis,unsigned int close1_open0);

UWC4000_API int __stdcall uwc4000_light_doubling(unsigned int LightType,int *doubling,int active1_save0);

//获取光源属性
UWC4000_API int __stdcall uwc4000_get_light_property(int *property);

//读取最大跟随误差记录
UWC4000_API int __stdcall uwc4000_get_max_En(unsigned int axis,double *Max_En);

//设置CPLD 双字节参数
UWC4000_API int __stdcall uwc4000_set_CPLD16(unsigned char ucAdd,unsigned short value);

//设置CPLD 四字节参数
UWC4000_API int __stdcall uwc4000_set_CPLD32(unsigned char ucAdd,unsigned long value);

UWC4000_API int __stdcall uwc4000_get_CPLD16(unsigned char ucAdd,unsigned short *value);

UWC4000_API int __stdcall uwc4000_get_CPLD32(unsigned char ucAdd,unsigned long *value);

UWC4000_API int __stdcall uwc4000_set_squash(int axis,double squash,int active1_save0);

UWC4000_API int __stdcall uwc4000_get_squash(int axis,double *squash);

UWC4000_API int __stdcall uwc4000_set_auto_enable(unsigned int axis,unsigned int initial_auto_enable1_api_enable0);

UWC4000_API int __stdcall uwc4000_get_auto_enable(unsigned int axis,unsigned int *initial_auto_enable1_api_enable0);

UWC4000_API int __stdcall uwc4000_set_lens_type(unsigned int type);

UWC4000_API int __stdcall uwc4000_set_channel_light(unsigned int channel,unsigned int light);

UWC4000_API int __stdcall uwc4000_ums_write(long v_type, long add, long l, float f);

UWC4000_API int __stdcall uwc4000_ums_read(long v_type, long add, long *l, float *f);

#ifdef __cplusplus
}
#endif

#endif 



