//Uinon Win Control Tech
#ifndef _UWCTECH_
#define _UWCTECH_

#define __UWC4000_EXPORTS


//Axis ID define
#define   Axis_X			0	
#define   Axis_Y			1	
#define   Axis_Z			2
#define   Axis_U			3


//HC2�İ�����
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
//HC2 LEDָʾ�ƶ���
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

//��ʼ������///////////////////////////////////////////////////////////////////////////
//��uwc4000��������,����ʼ������������
UWC4000_API int __stdcall uwc4000_initial(void);

//�Ͽ������������������
UWC4000_API int __stdcall uwc4000_close(void);

//��ȡ����ģ��״̬�� �÷������ο�:unsigned int status[4];uwc4000_get_count_status(status);
UWC4000_API int __stdcall uwc4000_get_count_status(unsigned int *status);

//��ȡ�˶�ģ��״̬�� �÷������ο�:unsigned int status;uwc4000_get_motion_status(&status);
UWC4000_API int __stdcall uwc4000_get_motion_status(unsigned int *status);

//������״̬ �÷������ο�:unsigned int status[4];uwc4000_get_axis_status(status);
UWC4000_API int __stdcall uwc4000_get_axis_status(unsigned int *status);

//��ȡ��������趨�ĸ�������ٶȺͼ��ٶ�
UWC4000_API int __stdcall uwc4000_get_profile(double *acc_time,double *max_speed);

//��ȡ��դ��λ�� �÷������ο�:double x_scale,double y_scale,double z_scale;uwc4000_get_scale(&x_scale,&y_scale,&z_scale)
UWC4000_API int __stdcall uwc4000_get_scale(double *x_scale,double *y_scale,double *z_scale);

//��ȡ4·��դ��λ��
UWC4000_API int __stdcall uwc4000_get_all_scale(double *scale_pos);

//��ȡ�������������
UWC4000_API int __stdcall uwc4000_get_pulse_count(long *count);

//���ù�Դ���� �÷������ο�:unsigned int light[44];uwc4000_set_light(light);
UWC4000_API int __stdcall uwc4000_set_light(unsigned int *light);

//��ȡ���ݸ���Ϣ �÷������ο�:int Message[5];uwc4000_get_joystick_Msg(Message);
UWC4000_API int __stdcall uwc4000_get_joystick_Msg(int *message);

//��ԭ���˶� �÷������ο�:uwc4000_go_home(0,10);//x�������ԭ��
UWC4000_API int __stdcall uwc4000_go_home(int axis,double search_speed);

//�����λ�˶� �÷������ο�:uwc4000_single_move_to(0,10000,10);//x��������ٶȶ�λ������ԭ��10mm��
UWC4000_API int __stdcall uwc4000_single_move_to(int axis,double speed,double target_pos);

//����˶���ʽ�����λ�˶�
UWC4000_API int __stdcall uwc4000_single_move_to_rel(int axis,double speed,double pos_rel);

//XYֱ�߲岹��ָ��λ�� �÷������ο�:double target[2]={10,20};uwc4000_XY_move_to(10000,target);//xy��������ٶȲ���ֱ�߲岹��ʽ��λ������ԭ��X=10mm,Y=20mm��
UWC4000_API int __stdcall uwc4000_XY_move_to(double speed,double *target_pos);

//XYֱ�߲岹��ָ���������λ��
UWC4000_API int __stdcall uwc4000_XY_move_to_rel(double speed,double *pos_rel);

//XYZֱ�߲岹��ָ��λ�� �÷������ο�:double target_pos[4]={10,10,10,0};uwc4000_XYZ_move_to(10000,target_pos);X/Y/Z������ٶ�ֱ�߲岹��λ��10,10,10
UWC4000_API int __stdcall uwc4000_XYZ_move_to(double speed,double *target_pos);

//XYZֱ�߲岹��ָ���������λ��
UWC4000_API int __stdcall uwc4000_XYZ_move_to_rel(double speed,double *pos_rel);

//ָ����ֱ�߲岹�˶�
UWC4000_API int __stdcall uwc4000_linear_move_to(long *axis_array,double speed,double *target_pos);

//��XOYƽ����Բ���岹�˶�,��line_speedΪ���ٶ�, x_center,y_centerΪԲ����������������λ��,angleΪ��ת�Ƕ�:-360--+360
UWC4000_API int __stdcall uwc4000_arc_move_xy(double x_center, double y_center, double line_speed, double angle);

//��YOZƽ����Բ���岹�˶�,��line_speedΪ���ٶ�, y_center,z_centerΪԲ����������������λ��,angleΪ��ת�Ƕ�:-360--+360
UWC4000_API int __stdcall uwc4000_arc_move_yz(double y_center, double z_center, double line_speed, double angle);

//��ZOXƽ����Բ���岹�˶�,��line_speedΪ���ٶ�, z_center,x_centerΪԲ����������������λ��,angleΪ��ת�Ƕ�:-360--+360
UWC4000_API int __stdcall uwc4000_arc_move_zx(double z_center, double x_center, double line_speed, double angle);

//ʾ���˶����� �÷������ο�:uwc4000_jog_start(0,30);//x������JOG�˶�,�˺���uwc4000_change_speed���ٷ�ΧΪ-30~30mm/s,����ϸ���800��
UWC4000_API int __stdcall uwc4000_jog_start(int axis,double jog_speed_max);

//ʾ���˶����� �÷������ο�:uwc4000_change_speed(0,-3);//����X���ǰ��jog_start,��x��jog�ٶȸı�,ʹX�Ḻ���˶�,�ٶ�Ϊ-3mm/s
UWC4000_API int __stdcall uwc4000_change_speed(int axis,double curnt_speed);

//ֹͣʾ���˶� �÷������ο�:uwc4000_jog_stop();//ͣ���������jog�˶�,����û���ᴦ��jog�˶�״̬����jog_stop�޸�����
UWC4000_API int __stdcall uwc4000_jog_stop(void);

//��ֹ���ݸ˿��� �÷������ο�:uwc4000_disable_joystick();
UWC4000_API int __stdcall uwc4000_disable_joystick(void);

//������ݸ˿���,ͬʱָ�������/�͵�������
//�÷������ο�:double low_speed[4]={5,5,5,5},high_speed[4]={50,50,50,50};//ҡ�˸���״̬ʱ,�⶯ҡ�˵����Ƕ��ٶ�Ϊ50mm/s,����״̬ʱ,�⶯�����Ƕ��ٶ�Ϊ5mm/s
UWC4000_API int __stdcall uwc4000_enable_joystick(double *low_speed,double *high_speed);

//��ȡ���ݸ˷�ʽ̽��ɵ㷴�����˲���դ������λ��
UWC4000_API int __stdcall uwc4000_get_joy_comeback_position(double *scale_pos);

//ͣ��ָ�� �÷������ο�:uwc4000_stop(0,0);//����ֹͣX��
UWC4000_API int __stdcall uwc4000_stop(int axis,int mode);

//���ø��ᶨλ����,��λ�������ò�����2΢�� �÷������ο�:double precision[4]={0.005,0,005,0.002,0.005};uwc4000_set_inposition_precision(precision);//x:5΢��,y:5΢��,z:2΢��
UWC4000_API int __stdcall uwc4000_set_inposition_precision(double *precision);

//̽�빦������ �÷������ο�:uwc4000_set_motion_probe(0x0001 | 0x0002);//0x0001ʹ��̽��,0x0000����̽��,0x0002renishaw//TESA
UWC4000_API int __stdcall uwc4000_set_motion_probe(unsigned int probe_set);

//����ҡ�˲ɵ���˲��� �÷������ο�:uwc4000_set_joystick_comeback(20,3);//ҡ�˲ɵ�ֹͣ����20mm/s�ٶȻ��˵����봥����3mmλ��
UWC4000_API int  __stdcall uwc4000_set_joystick_comeback(double comeback_speed,double comeback_distance);

//̽��ɵ��˶�ָ�� �÷������ο�:double target_pos[4]={10,10,10,0};uwc4000_touch_move(5,20,10,target_pos);//��5mm/s�ϳ��ٶȳ�������10,10,10��������,�������������20mm/s�ٶȻ��˵��ú������õ�;�����������,������������10,10,10λ��5mm��ͣ����.
UWC4000_API int __stdcall uwc4000_touch_move(double touch_speed,double comeback_speed,double search_radius,double *target_pos);

//��ȡ̽������ֵ �÷������ο�:double x_scale,y_scale,z_scale;uwc4000_get_probe_capture(&x_scale,&y_scale,&z_scale);
UWC4000_API int __stdcall uwc4000_get_probe_capture(double *x_scale,double *y_scale,double *z_scale);

//��ȡ̽�봥��ʸ��
UWC4000_API int __stdcall uwc4000_get_probe_capture_ijk(double *x_scale, double *y_scale, double *z_scale, double *u_scale, double *i, double *j, double *k);

//��ͨ�������״̬
UWC4000_API int __stdcall uwc4000_get_input(unsigned int *input);

//дͨ�������״̬
UWC4000_API int __stdcall uwc4000_set_output(unsigned int output);

//�������״̬
UWC4000_API int __stdcall uwc4000_get_output(unsigned int *output);

//��ȡuwc4000�������÷�
UWC4000_API int __stdcall uwc4000_get_function_describe(unsigned int *function_describe);

//��ȡ�����ѧ�߷ֱ���
UWC4000_API int __stdcall uwc4000_get_scale_resolution(double *resolution);

//���������դ��λ��
UWC4000_API int __stdcall uwc4000_set_scale(double x_scale,double y_scale,double z_scale);

//�������ù�դ��λ��, ������XYZU����
UWC4000_API int __stdcall uwc4000_set_scale2(int axis, double scale_pos);

//��ȡ�����λ����(����ڻ�е��λ)
UWC4000_API int __stdcall uwc4000_get_soft_limit(int axis,double *pos_limit,double *neg_limit);

//��ȡ��������ٶ����� �ο�
UWC4000_API int __stdcall uwc4000_get_max_speed(double *max_speed);

//��ȡ��λ���ȣ���λ��ɴ��ڣ�����
UWC4000_API int __stdcall uwc4000_get_inposition_precision(double *precision);

//��ȡ�̼�����汾
UWC4000_API int __stdcall uwc4000_get_firmware(char *firmware);

//��ȡ���һ��������Ϣ����
UWC4000_API int __stdcall uwc4000_get_last_err(unsigned int *err_code);

//��������
UWC4000_API int __stdcall uwc4000_super_command(unsigned int ch,int Slen,int Rlen,unsigned char *ucIn,unsigned char *ucOut);

//��ȡ��������ϵͳʱ��
UWC4000_API int __stdcall uwc4000_get_time(int *year, int *month, int *day, int *hour, int *minute, int *second);

//���;�����������굽HC���ݸ���ʾ��
UWC4000_API int __stdcall uwc4000_send_coordinate_to_HC(double *coordinate);

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
���º���Ϊ�ϰ汾���ݻ����Ŀ�Ķ��������󲿷ֹ�������Ч������ֱ��ʹ�ã����ʹ����Ҫ������UWC����֧����ϵ ljs1384@163.com  TEL:13510564386
�Ƽ�ʹ��uwc4000_super_commandʵ������APIδ����ٵĲ������뽫���������͵������ʼ���UWC���ṩһ�λ���super_command��װ�Ĵ�����ʵ��
*/
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//��ȡDSP���ݹ۲�������
UWC4000_API int  __stdcall uwc4000_get_MDSP_param(int number,long *param);

//����3D�������ᶨλ����,��λ�������ò�����5΢��,Ĭ��50um, �ο�uwc4000_set_inposition_precision
UWC4000_API int __stdcall uwc4000_set_inposition_precision_3D(double *precision);

//�����ȡ������״̬��Ϣ,ֻ��Ҫһ��ͨѶ���ɶ�ȡ���󲿷ֳ�����Ϣ:����״̬,�˶�ģ��״̬,����ģ��״̬,��դ������,�쳣����,���ݺм���״̬,ͨ�������״̬
//���ú���ִ��ʱ����ֻ��ȡ������Ϣ����ִ��ʱ�����, �ο�uwc4000_get_count_status uwc4000_get_motion_status uwc4000_get_axis_status uwc4000_get_profile uwc4000_get_scale
UWC4000_API long __stdcall uwc4000_get_controller_msg(unsigned long *ulStatus,double *dScalePosition);

//���ܹ�����ȥ������������
//��������ַ���
UWC4000_API int __stdcall uwc4000_set_security_string(int security_number,char *string);
//���м����㷨
UWC4000_API int __stdcall uwc4000_run_security_algorithmic(int security_number,float *param);

//��ȡ������UWC4000.INI�ļ��еĻ����ͺ�
UWC4000_API int __stdcall uwc4000_get_machine_type(char *string);

//��ȡmof����ֵ
UWC4000_API int __stdcall uwc4000_get_MOF_capture(double *x_scale,double *y_scale,double *z_scale);

//�������������
UWC4000_API int __stdcall uwc4000_set_follow_err(unsigned int *follow_err,int active1_save0);

//�������������
UWC4000_API int __stdcall uwc4000_get_follow_err(unsigned int *follow_err);

//��ȡ���ݸ�adcת�����, �ο�uwc4000_get_joystick_Msg
UWC4000_API int __stdcall uwc4000_get_joystick_ADC(int *offset);

//��λuwc4000���ù�����ȥ������������
UWC4000_API int __stdcall uwc4000_reset(void);

//���ù�դ�߷ֱ���
UWC4000_API int __stdcall uwc4000_set_scale_resolution(double *resolution);

//��ȡUWC4000 ID��
UWC4000_API int __stdcall uwc4000_get_uwc4000_ID(long *ID);

//��ȡjoystick ID��
UWC4000_API int __stdcall uwc4000_get_joystick_ID(long *ID);

//���ø����դ��RI����
UWC4000_API int __stdcall uwc4000_set_RI_mode(int *RI,int active1_save0);

//��ȡ�����դ��RI����
UWC4000_API int __stdcall uwc4000_get_RI_mode(int *RI);

//����UWC4000 ID��
UWC4000_API int __stdcall uwc4000_set_uwc4000_ID(long ID);

//����joystick ID��
UWC4000_API int __stdcall uwc4000_set_joystick_ID(long ID);

//����joystick ������Ϣ
UWC4000_API int __stdcall uwc4000_set_joystick_calibration(int *data_array,int active1_save0);

//��ȡjoystick ������Ϣ
UWC4000_API int __stdcall uwc4000_get_joystick_calibration(int *data_array);

//���ø�������/��դ��������
UWC4000_API int __stdcall uwc4000_set_pulse_scale_map(long *pulse,long *scale,int active1_save0);

//��ȡ��������/��դ��������
UWC4000_API int __stdcall uwc4000_get_pulse_scale_map(long *pulse,long *scale);

//���ø�������ٶȺͼ��ٶ�
UWC4000_API int __stdcall uwc4000_set_profile(double *acc_time,double *max_speed,int active1_save0);

//����uwc4000�������÷�
UWC4000_API int __stdcall uwc4000_set_function_describe(unsigned int *function_describe);

//�����դ�߼�����������
UWC4000_API int __stdcall uwc4000_set_count_dir(unsigned int *dir,int active1_save0);

//��ȡ��դ�߼�����������
UWC4000_API int __stdcall uwc4000_get_count_dir(unsigned int *dir);

//�������ר��i/o����
UWC4000_API int __stdcall uwc4000_set_IO_config(unsigned int *config,int active1_save0);

//��ȡ����ר��i/o����
UWC4000_API int __stdcall uwc4000_get_IO_config(unsigned int *config);

//��������ͺ�
UWC4000_API int __stdcall uwc4000_set_machine_type(char *string);

//���ø������巽���ź�
UWC4000_API int __stdcall uwc4000_set_pulse_dir(unsigned int *dir,int active1_save0);

//��ȡ���巽������
UWC4000_API int __stdcall uwc4000_get_pulse_dir(unsigned int *dir);

//���ø���PID,Kvff����
UWC4000_API int __stdcall uwc4000_set_PID(float *Kp,float *Ki,float *Kd,float *Kvff,int active1_save0);

//��ȡ����PID,Kvff����
UWC4000_API int __stdcall uwc4000_get_PID(float *Kp,float *Ki,float *Kd,float *Kvff);

//����̽�볣�����������ͣ�̽�����������ʱ��  �ο�uwc4000_set_motion_probe
UWC4000_API int __stdcall uwc4000_set_probe(int enable,int led_logic,int buzzer_time);

//����̽���ź�ȥ����ʱ��
UWC4000_API int __stdcall uwc4000_set_trig_debounce(int time);

//���ô����ź�ȥ����ʱ��
UWC4000_API int __stdcall uwc4000_get_trig_debounce(int *time);

//RI_function:0-������,1-����1��, gohome������װ����һ����
UWC4000_API int __stdcall uwc4000_set_RI_function(unsigned int axis,int function);

//��ͨѶģ���������Ϣ, DLL�ڲ�����, �ο� uwc4000_get_last_err
UWC4000_API int __stdcall uwc4000_get_err_msg(int *err_msg);

//����0-2�ᶨλ��ʽ, �°汾�������ɵ�������趨
UWC4000_API int __stdcall uwc4000_set_inposition_mode(int axis,int mode);

//����0-2�ᶨλģʽ���õ�ini�ļ�
UWC4000_API int __stdcall uwc4000_set_inposition_mode_save(int axis,int mode);

//��ȡ0-2�ᶨλģʽ����
UWC4000_API int __stdcall uwc4000_get_inposition_mode(int axis,int *mode);

//�����״̬�Ĵ���
UWC4000_API int __stdcall uwc4000_clear_status(int axis);

//���ø������������
UWC4000_API int __stdcall uwc4000_set_pulse_count(long *count);

//��ȡָ����pid������Ϣ
UWC4000_API int __stdcall uwc4000_get_PID_tunning(int axis,double *data);

//ʹ�����������·
UWC4000_API int __stdcall uwc4000_enable_pulse_output(unsigned int enable);

//��ȡ��������Դ��ѹ
UWC4000_API int __stdcall uwc4000_get_power_voltage(double *voltage);

//ʹ��zoom��
UWC4000_API int __stdcall uwc4000_zoom(int enable);

//�����λ����(��ԭ�����Ч), �����λ�ɵ�������Զ�����趨, Ӧ������ɶ�������, �ο�uwc4000_get_soft_limit
UWC4000_API int __stdcall uwc4000_set_soft_limit(int axis,double pos_limit,double neg_limit);

//��������λ��ini�ļ�
UWC4000_API int __stdcall uwc4000_set_soft_limit_save(int axis,double pos_limit,double neg_limit);


//���������, �ο�uwc4000_get_last_err
UWC4000_API int  __stdcall uwc4000_clear_motion_err(int ID);

//����Զ�̽������� �ο�uwc4000_get_last_err
UWC4000_API int __stdcall uwc4000_clear_auto_touch_err(void);

//��ȡ��̤��������ֵ ʹ��̽�빦��ʵ�ֽ�̤���زɵ㹦��, �ο�uwc4000_get_probe_capture
UWC4000_API int __stdcall uwc4000_get_footswitch_capture(double *x_scale,double *y_scale,double *z_scale);

//���DSP���ݹ۲�������
UWC4000_API int  __stdcall uwc4000_clear_MDSP_param(int number);

//����2D������λ���ʱ��
UWC4000_API int __stdcall uwc4000_set_INP_time(double time);

//��ȡ2D������λ���ʱ��
UWC4000_API int __stdcall uwc4000_get_INP_time(double *time);

//ʹ�ܡ���ֹ����������־ �ò���ͨ�������������������
UWC4000_API int __stdcall uwc4000_enable_Error_Log(bool enable1_disable0);

//��������λ����ɲ������
UWC4000_API int __stdcall uwc4000_set_limit_brake_distance(unsigned int *distance,int active1_save0);

//��ȡ������λɲ������
UWC4000_API int __stdcall uwc4000_get_limit_brake_distance(unsigned int *distance);

//����̽�봥��ɲ������
UWC4000_API int __stdcall uwc4000_set_probe_brake_distance(double distance,int active1_save0);

//�����״̬���еĴ�����Ϣ
UWC4000_API int __stdcall uwc4000_clear_status(int axis);

//��ȡ̽�봥��ɲ������
UWC4000_API int __stdcall uwc4000_get_probe_brake_distance(double *distance);

//��ȡҡ��̽����
UWC4000_API int __stdcall uwc4000_get_joystick_touch_flag(unsigned int *flag);

//��ȡ�Զ�̽����
UWC4000_API int __stdcall uwc4000_get_auto_touch_flag(unsigned int *flag);

//��ȡ��Դ��ͨ��������ֵ
UWC4000_API int __stdcall uwc4000_get_light_current(unsigned int *current);

//���ò���ģʽ,0-2D����,1-3D����
UWC4000_API int __stdcall uwc4000_set_measure_mode(unsigned int un3D1_2D0);


//����3D������λʱ��
UWC4000_API int __stdcall uwc4000_set_INP_time_3D(double time);

//��ȡ3D������λʱ��
UWC4000_API int __stdcall uwc4000_get_INP_time_3D(double *time);

//��ȡDLL�汾��
UWC4000_API int __stdcall uwc4000_get_dll_version(char *ver);

//����ָ����������ٵ�����
UWC4000_API int __stdcall uwc4000_set_SD_position(int axis,double positive_position,double negtive_position,int active1_save0);

//��ȡָ����������ٵ�����
UWC4000_API int __stdcall uwc4000_get_SD_position(int axis,double *positive_position,double *negtive_position);

//����ָ�����������ٵ���ٶ�
UWC4000_API int __stdcall uwc4000_set_SD_speed(int axis,double low_speed,int active1_save0);

//��ȡָ�����������ٵ���ٶ�
UWC4000_API int __stdcall uwc4000_get_SD_speed(int axis,double *low_speed);

//����ͨ������˿�״̬
//port_ch=1-3,status=0-65535
UWC4000_API int __stdcall uwc4000_set_GP_outport(int port_ch,unsigned short status);

UWC4000_API int __stdcall uwc4000_get_GP_outport(int port_ch,unsigned short *status);

//��ȡͨ������˿�״̬
//port_ch=1-3,status=0-65535
UWC4000_API int __stdcall uwc4000_get_GP_inport(int port_ch,unsigned short *status);

//���ÿ����������
//0-OTP6����(ѡ����������ò��ݸ����ݲɼ���)
//1-OTP7����(ѡ�����ò��ݺ�)
//2-���ƻ����ò��ݺ� 
UWC4000_API int __stdcall uwc4000_set_control_panel_type(int type,int active1_save0);

UWC4000_API int __stdcall uwc4000_get_control_panel_type(int *type);

//�����ٶ�������S���߶�ռ�ܼӼ���ʱ��ı���
//scale=2-50,����S���߶�ռ�ܼӼ���ʱ���1/2~1/50,scale��ֵԽ��Խ�ӽ������ٶ�����Ч��
UWC4000_API int __stdcall uwc4000_set_profile_ts(int axis,int scale,int active1_save0);

UWC4000_API int __stdcall uwc4000_get_profile_ts(int axis,int *scale);

//���ø����������ٶ�
UWC4000_API int __stdcall uwc4000_set_design_max_speed(double *design_max_speed,int active1_save0);

//��ȡ�����������ٶ�
UWC4000_API int __stdcall uwc4000_get_design_max_speed(double *design_max_speed);

//����ָ������Ư 
UWC4000_API int __stdcall uwc4000_set_offset(int axis,int offset,int active1_save0);

//��ȡָ������Ư 
UWC4000_API int __stdcall uwc4000_get_offset(int axis,int *offset);

//���ÿ������������ѹ
UWC4000_API int __stdcall uwc4000_set_output_voltage(unsigned int axis,double voltage);

//����PID���ƻ����������
UWC4000_API int __stdcall uwc4000_set_sum_max(unsigned int axis,long sum_max,int active1_save0);

//��ȡPID���ƻ����������
UWC4000_API int __stdcall uwc4000_get_sum_max(unsigned int axis,long *sum_max);

//����2D��λʱ�����õ�ini�ļ�
UWC4000_API int __stdcall uwc4000_set_INP_time_save(double time);

//����3D��λʱ�����õ�ini�ļ�
UWC4000_API int __stdcall uwc4000_set_INP_time_3D_save(double time);

//����2D������λ��Χ���õ�ini�ļ�
UWC4000_API int __stdcall uwc4000_set_inposition_precision_save(double *precision);

//����3D������λ��Χ���õ�ini�ļ�
UWC4000_API int __stdcall uwc4000_set_inposition_precision_3D_save(double *precision);

//��������դ�߷ֱ��ʵ�ini�ļ�,��λ����
UWC4000_API int __stdcall uwc4000_set_scale_resolution_save(double *resolution);

//�ŷ�������ʹ��/��ֹ,1-ʹ��,0-��ֹ
UWC4000_API int __stdcall uwc4000_servo_enable(unsigned int axis,unsigned int enable1_disable0);

//���ÿ���/�ջ����Ʒ�ʽ
UWC4000_API int __stdcall uwc4000_close_loop(unsigned int axis,unsigned int close1_open0);

UWC4000_API int __stdcall uwc4000_light_doubling(unsigned int LightType,int *doubling,int active1_save0);

//��ȡ��Դ����
UWC4000_API int __stdcall uwc4000_get_light_property(int *property);

//��ȡ����������¼
UWC4000_API int __stdcall uwc4000_get_max_En(unsigned int axis,double *Max_En);

//����CPLD ˫�ֽڲ���
UWC4000_API int __stdcall uwc4000_set_CPLD16(unsigned char ucAdd,unsigned short value);

//����CPLD ���ֽڲ���
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



