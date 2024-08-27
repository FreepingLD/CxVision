using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;
using System.Diagnostics;
using System.IO.Ports;

namespace Light
{
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
    using OPTControllerHandleType = IntPtr;

    class OPTControllerAPI
    {
        const string OPTControler_DLL = "OPTController.dll";
        private OPTControllerHandleType ControllerHandle = IntPtr.Zero;

        public const int OPT_SUCCEED = 0;       //operation succeed					 
        public const int OPT_ERR_INVALIDHANDLE = 3001001; //invalid handle  
        public const int OPT_ERR_UNKNOWN = 3001002; //error unknown 
        public const int OPT_ERR_INITSERIAL_FAILED = 3001003; //failed to initialize a serial port
        public const int OPT_ERR_RELEASESERIALPORT_FAILED = 3001004; //failed to release a serial port
        public const int OPT_ERR_SERIALPORT_UNOPENED = 3001005; //attempt to access an unopened serial port
        public const int OPT_ERR_CREATEETHECON_FAILED = 3001006; //failed to create an Ethernet connection
        public const int OPT_ERR_DESTROYETHECON_FAILED = 3001007; //failed to destroy an Ethernet connection
        public const int OPT_ERR_SN_NOTFOUND = 3001008; //SN is not found
        public const int OPT_ERR_TURNONCH_FAILED = 3001009; //failed to turn on the specified channel(s)
        public const int OPT_ERR_TURNOFFCH_FAILED = 3001010; //failed to turn off the specified channel(s)
        public const int OPT_ERR_SET_INTENSITY_FAILED = 3001011; //failed to set the intensity for the specified channel(s)
        public const int OPT_ERR_READ_INTENSITY_FAILED = 3001012; //failed to read the intensity for the specified channel(s)
        public const int OPT_ERR_SET_TRIGGERWIDTH_FAILED = 3001013; //failed to set trigger pulse width
        public const int OPT_ERR_READ_TRIGGERWIDTH_FAILED = 3001014; //failed to read trigger pulse width	
        public const int OPT_ERR_READ_HBTRIGGERWIDTH_FAILED = 3001015;//failed to read high brightness trigger pulse width
        public const int OPT_ERR_SET_HBTRIGGERWIDTH_FAILED = 3001016;//failed to set high brightness trigger pulse width
        public const int OPT_ERR_READ_SN_FAILED = 3001017; //failed to read serial number
        public const int OPT_ERR_READ_IPCONFIG_FAILED = 3001018; //failed to read IP address
        public const int OPT_ERR_CHINDEX_OUTRANGE = 3001019; //index(es) out of the range
        public const int OPT_ERR_WRITE_FAILED = 3001020; //failed to write data
        public const int OPT_ERR_PARAM_OUTRANGE = 3001021; //parameter(s) out of the range 
        public const int OPT_ERR_READ_MAC_FAILED = 3001022; //failed to read MAC
        public const int OPT_ERR_SET_MAXCURRENT_FAILED = 3001023; //failed to set max current
        public const int OPT_ERR_READ_MAXCURRENT_FAILED = 3001024; //failed to read max current
        public const int OPT_ERR_SET_TRIGGERACTIVATION_FAILED = 3001025; //failed to set trigger activation
        public const int OPT_ERR_READ_TRIGGERACTIVATION_FAILED = 3001026; //failed to read trigger activation
        public const int OPT_ERR_SET_WORKMODE_FAILED = 3001027;	 //failed to set work mode
        public const int OPT_ERR_READ_WORKMODE_FAILED = 3001028; //failed to read work mode
        public const int OPT_ERR_SET_BAUDRATE_FAILED = 3001029;	 //failed to set baud rate
        public const int OPT_ERR_SET_CHANNELAMOUNT_FAILED = 3001030;     //failed to set channel amount
        public const int OPT_ERR_SET_DETECTEDMINLOAD_FAILED = 3001031;	 //failed to set detected min load
        public const int OPT_ERR_READ_OUTERTRIGGERFREQUENCYUPPERBOUND_FAILED = 3001032;	 //failed to read outer trigger frequency upper bound
        public const int OPT_ERR_SET_AUTOSTROBEFREQUENCY_FAILED = 3001033;	 //failed to set auto-strobe frequency
        public const int OPT_ERR_READ_AUTOSTROBEFREQUENCY_FAILED = 3001034;	 //failed to read auto-strobe frequency
        public const int OPT_ERR_SET_DHCP_FAILED = 3001035;	 //failed to set DHCP
        public const int OPT_ERR_SET_LOADMODE_FAILED = 3001036;	 //failed to set load mode
        public const int OPT_ERR_READ_PROPERTY_FAILED = 3001037;	 //failed to read property
        public const int OPT_ERR_CONNECTION_RESET_FAILED = 3001038;	 //failed to reset connection
        public const int OPT_ERR_SET_HEARTBEAT_FAILED = 3001039;	 //failed to set Ethernet connection heartbeat
        public const int OPT_ERR_GETCONTROLLERLIST_FAILED = 3001040;     //Failed to get controller(s) list           
        public const int OPT_ERR_SOFTWARETRIGGER_FAILED = 3001041;     //Failed to software trigger                
        public const int OPT_ERR_GET_CHANNELSTATE_FAILED = 3001042;     //Failed to get channelstate          
        public const int OPT_ERR_SET_KEEPALIVEPARAMETERS_FAILED = 3001043;     //Failed to set keepalvie parameters          
        public const int OPT_ERR_ENABLE_KEEPALIVE_FAILED = 3001044;     //Failed to enable/disable keepalive
        public const int OPT_ERR_READSTEPCOUNT_FAILED = 3001045;     //Failed to read step count           
        public const int OPT_ERR_SETTRIGGERMODE_FAILED = 3001046;     //Failed to set trigger mode    
        public const int OPT_ERR_READTRIGGERMODE_FAILED = 3001047;     //Failed to read trigger mode      
        public const int OPT_ERR_SETCURRENTSTEPINDEX_FAILED = 3001048;     //Failed to set current step index          
        public const int OPT_ERR_READCURRENTSTEPINDEX_FAILED = 3001049;     //Failed to read current step index          
        public const int OPT_ERR_RESETSEQ_FAILED = 3001050;     //Failed to reset SEQ
        public const int OPT_ERR_SETTRIGGERDELAY_FAILED = 3001051;      //Failed to set trigger delay
        public const int OPT_ERR_GET_TRIGGERDELAY_FAILED = 3001052;     //Failed to get trigger delay
        public const int OPT_ERR_SETMULTITRIGGERDELAY_FAILED = 3001053;     //Failed to set multiple channels trigger delay
        public const int OPT_ERR_SETSEQTABLEDATA_FAILED = 3001054;     //Failed to set SEQ table data
        public const int OPT_ERR_READSEQTABLEDATA_FAILED = 3001055;      //Failed to Read SEQ table data
        public const int OPT_ERR_READ_CHANNELS_FAILED = 3001056;      //Failed to read controller's channel
        public const int OPT_ERR_READ_KEEPALIVE_STATE_FAILED = 3001057;      //Failed to read the state of keepalive
        public const int OPT_ERR_READ_KEEPALIVE_CONTINUOUS_TIME_FAILED = 3001058;      //Failed to read the continuous time of keepalive
        public const int OPT_ERR_READ_DELIVERY_TIMES_FAILED = 3001059;      //Failed to read the delivery times of prop packet
        public const int OPT_ERR_READ_INTERVAL_TIME_FAILED = 3001060;      //Failed to read the interval time of prop packet
        public const int OPT_ERR_READ_OUTPUTBOARD_VISION_FAILED = 3001061;      //Failed to read the vision of output board
        public const int OPT_ERR_READ_DETECT_MODE_FAILED = 3001062;    //Failed to read detect mode of load
        public const int OPT_ERR_SET_BOOT_PROTECTION_MODE_FAILED = 3001063;      //Failed to set mode of boot protection
        public const int OPT_ERR_READ_MODEL_BOOT_MODE_FAILED = 3001064;    //Failed to read the specified channel boot state
        public const int OPT_ERR_SET_OUTERTRIGGERFREQUENCYUPPERBOUND_FAILED = 3001065;	 //Failed to set outer trigger frequency upper bound
        public const int OPT_ERR_SET_IPCONFIG_FAILED = 3001066;   //Failed to set IP configuration of the controller

        public const int OPT_ERR_SET_VOLTAGE_FAILED = 3001067;	 //Failed to set voltage of specified channel voltage
        public const int OPT_ERR_READ_VOLTAGE_FAILED = 3001068;   //Failed to read the specified channel's voltage
        public const int OPT_ERR_SET_TIMEUNIT_FAILED = 3001069;	 //Failed to set time unit
        public const int OPT_ERR_READ_TIMEUNIT_FAILED = 3001070;   //Failed to read time unit

        public const int OPT_ERR_FILEEXT = 3001071;     //File suffix name is wrong
        public const int OPT_ERR_FILEPATH_EMPTY = 3001072;     //File path is empty
        public const int OPT_ERR_FILE_MAGIC_NUM = 3001073;     //magic number is wrong
        public const int OPT_ERR_FILE_CHECKSUM = 3001074;    //Checksum is wrong
        public const int OPT_ERR_SEQDATA_EQUAL = 3001075;     //Current SEQ table data is different from load file data
        public const int OPT_ERR_SET_HB_TIMEUNIT_FAILED = 3001076;     //Failed to set highlight time unit
        public const int OPT_ERR_READ_HB_TIMEUNIT_FAILED = 3001077;     //Failed to read highlight time unit
        public const int OPT_ERR_SET_TRIGGERDELAY_TIMEUNIT_FAILED = 3001078;     //Failed to set trigger delay time unit
        public const int OPT_ERR_READ_TRIGGERDELAY_TIMEUNIT_FAILED = 3001079;     //Failed to read trigger delay time unit
        public const int OPT_ERR_SET_PERCENT_FAILED = 3001080;     //Failed to set percent of brightening current
        public const int OPT_ERR_READ_PERCENT_FAILED = 3001081;     //Failed to read percent of brightening current
        public const int OPT_ERR_SET_HB_LIMIT_STATE_FAILED = 3001082;     //Failed to set high light trigger output duty limit switch state
        public const int OPT_ERR_READ_HB_LIMIT_STATE_FAILED = 3001083;    //Failed to read high light trigger output duty limit switch state
        public const int OPT_ERR_SET_HB_TRIGGER_OUTPUT_DUTY_RATIO_FAILED = 3001084;    //Failed to set high light trigger output duty limit ratio
        public const int OPT_ERR_READ_HB_TRIGGER_OUTPUT_DUTY_RATIO_FAILED = 3001085;    //Failed to read high light trigger output duty limit ratio
        public const int OPT_ERR_SET_DIFF_PRESURE_LIMIT_STATE_FAILED = 3001086;     //Failed to set differential pressure limit function switch status
        public const int OPT_ERR_READ_DIFF_PRESURE_LIMIT_STATE_FAILED = 3001087;    //Failed to read differential pressure limit function switch status
        public const int OPT_ERR_SET_START_ADDRESS_FAILED = 3001088; //Failed to set the specified channel's start address index
        public const int OPT_ERR_SET_END_ADDRESS_FAILED = 3001089; //Failed to set the specified channel's end address index
        public const int OPT_ERR_SET_HARDWARE_RESET_SWITCH_STATE_FAILED = 3001090; //Failed to set the specified channel's hardware reset switch state
        public const int OPT_ERR_SET_ADDRESS_SWITCH_STATE_FAILED = 3001091; //Failed to set the specified channel's hardware reset switch state
        public const int OPT_ERR_READ_PROGRAMMABLE_PARAM_FAILED = 3001092; //Failed to read programmable parameters for the specified channel's
        public const int OPT_ERR_SET_DBOARD_HANDLE_INTENSITY_RESULT_FAILED = 3001093; //Fail to set D board whether handle intensity result
        public const int OPT_ERR_READ_DBOARD_HANDLE_INTENSITY_RESULT_FAILED = 3001094; //Fail to read D board whether handle intensity result
        public const int OPT_ERR_SET_DO_CONTROL_SOURCE_FAILED = 3001095; //Fail to set DO control source
        public const int OPT_ERR_READ_DO_CONTROL_SOURCE_FAILD = 3001096; //Fail to read DO control source
        public const int OPT_ERR_SET_DO_CONTROL_MODE_FAILED = 3001097; //Fail to set DO control mode
        public const int OPT_ERR_READ_DO_CONTROL_MODE_FAILD = 3001098; //Fail to read DO control mode
        public const int OPT_ERR_SET_DO_POLARITY_FAILED = 3001099; //Fail to set DO polarity
        public const int OPT_ERR_READ_DO_POLARITY_FAILD = 3001100; //Fail to read DO polarity
        public const int OPT_ERR_SET_DO_STANDBY_LEVEL_FAILED = 3001101; //Fail to set DO standby level
        public const int OPT_ERR_READ_DO_STANDBY_LEVEL_FAILED = 3001102; //Fail to read DO standby level
        public const int OPT_ERR_SET_DO_SOFTWARE_LEVEL_CONTROL_FAILED = 3001103; //Fail to set DO software level control
        public const int OPT_ERR_READ_DO_SOFTWARE_LEVEL_CONTROL_FAILED = 3001104; //Fail to read DO software level control
        public const int OPT_ERR_READ_DO_ACTUAL_OUTPUT_LEVEL_FAILED = 3001105; //Fail to read DO actual output level
        public const int OPT_ERR_SET_DO_DELAY_FAILED = 3001106; //Fail to set DO delay
        public const int OPT_ERR_READ_DO_DELAY_FAILED = 3001107; //Fail to read DO delay
        public const int OPT_ERR_SET_DO_DELAY_UNIT_FAILED = 3001108; //Fail to set DO delay unit
        public const int OPT_ERR_READ_DO_DELAY_UNIT_FAILED = 3001109; //Fail to read DO delay unit
        public const int OPT_ERR_SET_DO_TRIGGER_WIDTH_FAILED = 3001110; //Fail to set DO trigger width
        public const int OPT_ERR_READ_DO_TRIGGER_WIDTH_FAILED = 3001111; //Fail to read DO trigger width
        public const int OPT_ERR_SET_DO_TRIGGER_WIDTH_UNIT_FAILED = 3001112; //Fail to set DO trigger width unit
        public const int OPT_ERR_READ_DO_TRIGGER_WIDTH_UNIT_FAILED = 3001113; //Fail to read DO trigger width unit
        public const int OPT_ERR_SET_TRIGGERFREQDIVMODE = 3001116; //Fail to set trigger frequency division mode
        public const int OPT_ERR_READ_TRIGGERFREQDIVMODE_FAILED = 3001117; //Fail to read trigger frequency division mode
        public const int OPT_ERR_SET_FREQDIVTRIGSOURCE_FAILED = 3001118; //Fail to set frequency division trigger source
        public const int OPT_ERR_READ_FREQDIVTRIGSRC_FAILED = 3001119; //Fail to Read frequency division trigger source
        public const int OPT_ERR_SET_FREQDIVCOUNT_FAILED = 3001120; //Fail to set frequency division count
        public const int OPT_ERR_READ_FREQDIVCOUNT_FAILED = 3001121; //Fail to read frequency division count
        public const int OPT_ERR_SET_FREQDIVCURRENTCOUNT_FAILED = 3001122; //Fail to set current count value of frequency division
        public const int OPT_ERR_READ_FREQDIVCURRENTCOUNT_FAILED = 3001123; //Fail to read current count value of frequency division
        public const int OPT_ERR_RESET_FREQDIVCURRENTCOUNT_FAILED = 3001124; //Fail to reset current count value of frequency division
        public const int OPT_ERR_SET_FREQDIVHARDWARERESETSRC_FAILED = 3001125; //Fail to set frequency division hardware reset source
        public const int OPT_ERR_READ_FREQDIVHARDWARERESETSRC_FAILED = 3001126; //Fail to read frequency division hardware reset source
        public const int OPT_ERR_SET_DOFREQDIVANDMULPARA_FAILED = 3001127; //Fail to set DO frequency division and multiplication parameters
        public const int OPT_ERR_READ_DOFREQDIVANDMULPARA_FAILED = 3001128; //Fail to read DO frequency division and multiplication parameters
        public const int OPT_ERR_SET_DOFREQUENCY_DIVISION_FAILED = 3001129; //Fail to set DO frequency division
        public const int OPT_ERR_READ_DOFREQUENCY_DIVISION_FAILED = 3001130; //Fail to read DO frequency division
        public const int OPT_ERR_SET_FREQDIVMODETRIGRECVCNTSW_FAILED = 3001131; //Fail to set frequency division mode trigger reception count switch
        public const int OPT_ERR_READ_FREQDIVMODETRIGRECVCNTSW_FAILED = 3001132; //Fail to read frequency division mode trigger reception count switch
        public const int OPT_ERR_SET_FREQDIVMODETRIGCOUNTPARA_FAILED = 3001133; //Fail to set frequency division mode trigger count numerical parameters
        public const int OPT_ERR_READ_FREQDIVMODETRIGCOUNTPARA_FAILED = 3001134; //Fail to read frequency division mode trigger count numerical parameters
        public const int OPT_ERR_SET_FREQDIVMULENCODERDIRECTSW_FAILED = 3001135; //Fail to set direction switch of frequency division and multiplication encoder
        public const int OPT_ERR_READ_FREQDIVMULENCODERDIRECTSW_FAILED = 3001136; //Fail to read direction switch of frequency division and multiplication encoder
        public const int OPT_ERR_SET_ENCODERDIRECTION_FAILED = 3001137; //Fail to set encoder direction
        public const int OPT_ERR_READ_ENCODERDIRECTION_FAILED = 3001138; //Fail to read encoder direction
        public const int OPT_ERR_SET_LINEOUTPUTPULSEWIDTH_FAILED = 3001139; //Fail to set line output pulse width
        public const int OPT_ERR_READ_LINEOUTPUTPULSEWIDTH_FAILED = 3001140; //Fail to read line output pulse width
        public const int OPT_ERR_SET_LINEOUTPUTPULSEWIDTHTIMEUNIT_FAILED = 3001141; //Fail to set line output pulse width time unit
        public const int OPT_ERR_READ_LINEOUTPUTPULSEWIDTHTIMEUNIT_FAILED = 3001142; //Fail to read line output pulse width time unit
        public const int OPT_ERR_SET_LINEOUTPUTDELAY_FAILED = 3001143; //Fail to set line output delay
        public const int OPT_ERR_READ_LINEOUTPUTDELAY_FAILED = 3001144; //Fail to read line output delay
        public const int OPT_ERR_SET_LINEOUTPUTPWDELAYTIMEUNIT_FAILED = 3001145; //Fail to set line output pulse width delay time unit
        public const int OPT_ERR_READ_LINEOUTPUTPWDELAYTIMEUNIT_FAILED = 3001146; //Fail to read line output pulse width delay time unit
        public const int OPT_ERR_SET_FRAMEOUTPUTPULSEWIDHT_FAILED = 3001147; //Fail to set frame output pulse width
        public const int OPT_ERR_READ_FRAMEOUTPUTPULSEWIDHT_FAILED = 3001148; //Fail to read frame outputssspulse width
        public const int OPT_ERR_SET_FRAMEOUTPUTPULSEWIDHTTIMEUNIT_FAILED = 3001149; //Fail to set frame output pulse width time unit
        public const int OPT_ERR_READ_FRAMEOUTPUTPULSEWIDHTTIMEUNIT_FAILED = 3001150; //Fail to read frame output pulse width time unit
        public const int OPT_ERR_SET_FRAMEOUTPUTDELAY_FAILED = 3001151; //Fail to set frame output delay
        public const int OPT_ERR_READ_FRAMEOUTPUTDELAY_FAILED = 3001152; //Fail to read frame output delay
        public const int OPT_ERR_SET_FRAMEOUTPUTPWDELAYTIMEUNIT_FAILED = 3001153; //Fail to set frame output pulse width time unit
        public const int OPT_ERR_READ_FRAMEOUTPUTPWDELAYTIMEUNIT_FAILED = 3001154; //Fail to read frame output pulse width time unit
        public const int OPT_ERR_SET_FREQDOUBLEPARAOFIFC_FAILED = 3001155; //Fail to set frequency doubling parameters of interface C
        public const int OPT_ERR_READ_FREQDOUBLEPARAOFIFC_FAILED = 3001156; //Fail to read frequency doubling parameters of interface C
        public const int OPT_ERR_SET_FREQDIVISIONPARAOFIFC_FAILED = 3001157; //Fail to set frequency division parameters of interface C
        public const int OPT_ERR_READ_FREQDIVISIONPARAOFIFC_FAILED = 3001158; //Fail to read frequency division parameters of interface C
        public const int OPT_ERR_SET_FREQDOUBLEPULSEWIDTHOFIFC_FAILED = 3001159; //Fail to set frequency doubling pulse width of C interface
        public const int OPT_ERR_READ_FREQDOUBLEPULSEWIDTHOFIFC_FAILED = 3001160; //Fail to read frequency doubling pulse width of C interface
        public const int OPT_ERR_SET_LINEPOLARITY_FAILED = 3001161; //Fail to set line polarity
        public const int OPT_ERR_READ_LINEPOLARITY_FAILED = 3001162; //Fail to read line polarity
        public const int OPT_ERR_SET_FRAMEPOLARITY_FAILED = 3001163; //Fail to set frame polarity
        public const int OPT_ERR_READ_FRAMEPOLARITY_FAILED = 3001164; //Fail to read frame polarity
        public const int OPT_ERR_WRITEFREQDIVTABLE_FAILED = 3001165; //Fail to write frequency division table
        public const int OPT_ERR_READ_FREQDIVTABLE_FAILED = 3001166; //Fail to read frequency division table
        // HACK C# OPTControllerAPI 

        [StructLayout(LayoutKind.Sequential)]
        public struct IntensityItem
        {
            public int channel;
            public int intensity;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TriggerWidthItem
        {
            public int channel;
            public int triggerWidth;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct HBTriggerWidthItem
        {
            public int channel;
            public int HBTriggerWidth;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SoftwareTriggerItem
        {
            public int channel;
            public int SoftwareTriggerTime;
        };


        [StructLayout(LayoutKind.Sequential)]
        public struct TriggerDelayItem
        {
            public int channel;
            public int TriggerDelayTime;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct MaxCurrentItem
        {
            public int channel;
            public int TriggerDelayTime;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct DOSoftwareLevelItem
        {
            public int channel;
            public int level;
        };

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_InitSerialPort(String SerialPortName, OPTControllerHandleType* ControllerHandle);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReleaseSerialPort(OPTControllerHandleType ControllerHandle);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_CreateEthernetConnectionByIP(String IP, OPTControllerHandleType* ControllerHandle);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_CreateEthernetConnectionBySN(String SN, OPTControllerHandleType* ControllerHandle);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_DestroyEthernetConnection(OPTControllerHandleType ControllerHandle);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_TurnOnChannel(OPTControllerHandleType ControllerHandle, int Channel);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_TurnOnMultiChannel(OPTControllerHandleType ControllerHandle, int[] ChannelArray, int len);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_TurnOffChannel(OPTControllerHandleType ControllerHandle, int Channel);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_TurnOffMultiChannel(OPTControllerHandleType ControllerHandle, int[] ChannelArray, int len);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetIntensity(OPTControllerHandleType ControllerHandle, int Channel, int vlaue);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetMultiIntensity(OPTControllerHandleType ControllerHandle, IntensityItem[] intensityArray, int length);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadIntensity(OPTControllerHandleType ControllerHandle, int Channel, int* vlaue);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetTriggerWidth(OPTControllerHandleType ControllerHandle, int Channel, int Value);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetMultiTriggerWidth(OPTControllerHandleType ControllerHandle, TriggerWidthItem[] triggerWidthArray, int length);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadTriggerWidth(OPTControllerHandleType ControllerHandle, int Channel, int* vlaue);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetHBTriggerWidth(OPTControllerHandleType ControllerHandle, int Channel, int Value);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetMultiHBTriggerWidth(OPTControllerHandleType ControllerHandle, HBTriggerWidthItem[] HBTriggerWidthArray, int length);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadHBTriggerWidth(OPTControllerHandleType ControllerHandle, int Channel, int* vlaue);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_EnableResponse(OPTControllerHandleType ControllerHandle, int nisResponse);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_EnableCheckSum(OPTControllerHandleType ControllerHandle, int nisCheckSum);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_EnablePowerOffBackup(OPTControllerHandleType ControllerHandle, int nisSave);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadSN(OPTControllerHandleType ControllerHandle, StringBuilder SN);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadIPConfig(OPTControllerHandleType ControllerHandle, StringBuilder IP, StringBuilder subnetMask, StringBuilder defaultGateway);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadIPConfigBySN(StringBuilder SN, StringBuilder IP, StringBuilder subnetMask, StringBuilder defaultGateway);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadProperties(OPTControllerHandleType ControllerHandle, int properties, StringBuilder value);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetMaxCurrent(OPTControllerHandleType ControllerHandle, int channelIndex, int current);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadMaxCurrent(OPTControllerHandleType ControllerHandle, int channelIndex, int mode, int* value);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetMultiMaxCurrent(OPTControllerHandleType ControllerHandle, MaxCurrentItem[] maxCurrentArray, int arrayLength);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadMAC(OPTControllerHandleType ControllerHandle, StringBuilder MAC);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetTriggerActivation(OPTControllerHandleType ControllerHandle, int channelIndex, int triggerActivation);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadTriggerActivation(OPTControllerHandleType ControllerHandle, int channelIndex, int* triggerActivation);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetWorkMode(OPTControllerHandleType ControllerHandle, int workMode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadWorkMode(OPTControllerHandleType ControllerHandle, int* workMode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetOuterTriggerFrequencyUpperBound(OPTControllerHandleType ControllerHandle, int channelIndex, int maxFrequency);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadOuterTriggerFrequencyUpperBound(OPTControllerHandleType ControllerHandle, int channelIndex, int* maxFrequency);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_AutoDetectLoadOnce(OPTControllerHandleType ControllerHandle);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetAutoStrobeFrequency(OPTControllerHandleType ControllerHandle, int channelIndex, int frequency);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadAutoStrobeFrequency(OPTControllerHandleType ControllerHandle, int channelIndex, int* frequency);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_EnableDHCP(OPTControllerHandleType ControllerHandle, int nbDHCP);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetLoadMode(OPTControllerHandleType ControllerHandle, int channelIndex, int loadMode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_GetVersion(StringBuilder version);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ConnectionResetBySN(StringBuilder serialNumber);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_IsConnect(OPTControllerHandleType ControllerHandle);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ConnectionResetByIP(String IP);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetEthernetConnectionHeartBeat(OPTControllerHandleType ControllerHandle, UInt32 timeout);


        /*******/
        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_GetControllerListOnEthernet(StringBuilder snList);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_GetChannelState(OPTControllerHandleType controllerHandle, int channelIdx, int* state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetKeepaliveParameter(OPTControllerHandleType controllerHandle, int keepalive_time,
                                         int keepalive_intvl, int keepalive_probes);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_EnableKeepalive(OPTControllerHandleType controllerHandle, int nenable);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SoftwareTrigger(OPTControllerHandleType controllerHandle, int channelIndex, int time);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_MultiSoftwareTrigger(OPTControllerHandleType controllerHandle, SoftwareTriggerItem[] softwareTriggerArray, int length);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadStepCount(OPTControllerHandleType controllerHandle, int moduleIndex, int* count);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetTriggerMode(OPTControllerHandleType controllerHandle, int moduleIndex, int mode);
        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadTriggerMode(OPTControllerHandleType controllerHandle, int moduleIndex, int* mode);
        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetCurrentStepIndex(OPTControllerHandleType controllerHandle, int moduleIndex, int curStepIndex);
        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadCurrentStepIndex(OPTControllerHandleType controllerHandle, int moduleIndex, int* curStepIndex);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ResetSEQ(OPTControllerHandleType controllerHandle, int moduleIndex);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetSeqTable(OPTControllerHandleType controllerHandle, int moduleIndex, int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadSeqTable(OPTControllerHandleType controllerHandle, int curStepIndex, int* seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
       OPTController_SaveSeqFile(int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth, String filePath);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
       OPTController_LoadSeqFile(String filePath, int* seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
       OPTController_CompareSeqTable(OPTControllerHandleType controllerHandle, int moduleIndex, String filePath);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SaveSeqFileToCSV(int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth, String filePath);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_LoadSeqFileFromCSV(String filePath, int* seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
       OPTController_CompareSeqTableFromCSV(OPTControllerHandleType controllerHandle, int moduleIndex, String filePath);


        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetTriggerDelay(OPTControllerHandleType controllerHandle, int channelIndex, int triggerDelay);


        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_GetTriggerDelay(OPTControllerHandleType controllerHandle, int channelIndex, int* triggerDelay);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetMultiTriggerDelay(OPTControllerHandleType controllerHandle, TriggerDelayItem[] triggerDelayArray, int length);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_GetControllerChannels(OPTControllerHandleType controllerHandle, int* channels);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadKeepaliveSwitchState(OPTControllerHandleType controllerHandle, int* state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadContinuousKeepaliveTime(OPTControllerHandleType controllerHandle, int* time);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadPacketDeliveryTimes(OPTControllerHandleType controllerHandle, int* times);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadIntervalTimeOfPropPacket(OPTControllerHandleType controllerHandle, int* time);


        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
       int
       OPTController_ReadOutputBoardVision(OPTControllerHandleType controllerHandle, StringBuilder vision);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
       int
       OPTController_ReadLoadDetectMode(OPTControllerHandleType controllerHandle, int channelIndex, int* mode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
       int
       OPTController_SetBootProtection(OPTControllerHandleType controllerHandle, int channelIndex, int mode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
       int
       OPTController_ReadModelBootState(OPTControllerHandleType controllerHandle, int channelIndex, int* state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
       int
       OPTController_SetIPConfiguration(OPTControllerHandleType controllerHandle, StringBuilder IP, StringBuilder subnetMask, StringBuilder defaultGateway);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
         OPTController_SetOutputVoltage(OPTControllerHandleType controllerHandle, int channelIndex, int voltage);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadOutputVoltage(OPTControllerHandleType controllerHandle, int channelIndex, int* voltage);


        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
         OPTController_SetTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* timeUnit);


        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetHBTriggerUnit(OPTControllerHandleType controllerHandle, int channelIndex, int timeUnit);


        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadHBTriggerUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetTriggerDelayUnit(OPTControllerHandleType controllerHandle, int channelIndex, int timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadTriggerDelayUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetPercentOfBrighteningCurrent(OPTControllerHandleType controllerHandle, int channelIndex, int percentage);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadPercentOfBrighteningCurrent(OPTControllerHandleType controllerHandle, int channelIndex, int* percentage);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetHBTriggerOutputDutyLimitSwitchState(OPTControllerHandleType controllerHandle, int channelIndex, int state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadHBTriggerOutputDutyLimitSwitchState(OPTControllerHandleType controllerHandle, int channelIndex, int* state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetHBTriggerOutputDutyRatio(OPTControllerHandleType controllerHandle, int channelIndex, int ratio);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadHBTriggerOutputDutyRatio(OPTControllerHandleType controllerHandle, int channelIndex, int* ratio);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDiffPresureLimitSwitchState(OPTControllerHandleType controllerHandle, int channelIndex, int state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDiffPresureLimitSwitchState(OPTControllerHandleType controllerHandle, int channelIndex, int* state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDBoardHandleIntensityResultSwitchState(OPTControllerHandleType controllerHandle, int state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDBoardHandleIntensityResultSwitchState(OPTControllerHandleType controllerHandle, int* state);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOControlSource(OPTControllerHandleType controllerHandle, int channelIndex, int ctrlSrc);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOControlSource(OPTControllerHandleType controllerHandle, int channelIndex, int* ctrlSrc);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOControlMode(OPTControllerHandleType controllerHandle, int channelIndex, int mode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOControlMode(OPTControllerHandleType controllerHandle, int channelIndex, int* mode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOPolarity(OPTControllerHandleType controllerHandle, int channelIndex, int polarity);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOPolarity(OPTControllerHandleType controllerHandle, int channelIndex, int* polarity);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOStandbyLevel(OPTControllerHandleType controllerHandle, int channelIndex, int level);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOStandbyLevel(OPTControllerHandleType controllerHandle, int channelIndex, int* level);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOSoftwareLevelControl(OPTControllerHandleType controllerHandle, int channelIndex, int level);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOSoftwareLevelControl(OPTControllerHandleType controllerHandle, int channelIndex, int* level);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOMultiSoftwareLevelControl(OPTControllerHandleType controllerHandle, DOSoftwareLevelItem[] softwareLevelArray, int length);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOActualOutputLevel(OPTControllerHandleType controllerHandle, int channelIndex, int* level);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDODelay(OPTControllerHandleType controllerHandle, int channelIndex, int delay);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDODelay(OPTControllerHandleType controllerHandle, int channelIndex, int* delay);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDODelayUnit(OPTControllerHandleType controllerHandle, int channelIndex, int unit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDODelayUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* unit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOTriggerWidth(OPTControllerHandleType controllerHandle, int channelIndex, int width);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOTriggerWidth(OPTControllerHandleType controllerHandle, int channelIndex, int* width);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOTriggerWidthUnit(OPTControllerHandleType controllerHandle, int channelIndex, int unit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOTriggerWidthUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* unit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetIPConfiguration(OPTControllerHandleType controllerHandle, String IP, String subnetMask, String defaultGateway);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetTriggerFreqDivMode(OPTControllerHandleType controllerHandle, int channelIndex, int mode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadTriggerFreqDivMode(OPTControllerHandleType controllerHandle, int channelIndex, int* mode);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivTrigSrc(OPTControllerHandleType controllerHandle, int channelIndex, int source);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivTrigSrc(OPTControllerHandleType controllerHandle, int channelIndex, int* source);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivCount(OPTControllerHandleType controllerHandle, int channelIndex, int count);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivCount(OPTControllerHandleType controllerHandle, int channelIndex, int* count);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivCurrentCount(OPTControllerHandleType controllerHandle, int channelIndex, int count);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivCurrentCount(OPTControllerHandleType controllerHandle, int channelIndex, int* count);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ResetFreqDivCurrentCount(OPTControllerHandleType controllerHandle, int channelIndex);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivHardWareResetSrc(OPTControllerHandleType controllerHandle, int channelIndex, int source);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivHardWareResetSrc(OPTControllerHandleType controllerHandle, int channelIndex, int* source);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOFreqDivAndMulPara(OPTControllerHandleType controllerHandle, int channelIndex, int para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOFreqDivAndMulPara(OPTControllerHandleType controllerHandle, int channelIndex, int* para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetDOFreqencyDivision(OPTControllerHandleType controllerHandle, int channelIndex, int value);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadDOFreqencyDivision(OPTControllerHandleType controllerHandle, int channelIndex, int* value);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivModeTrigRecvCntSW(OPTControllerHandleType controllerHandle, int channelIndex, int enSwitch);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivModeTrigRecvCntSW(OPTControllerHandleType controllerHandle, int channelIndex, int* enSwitch);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivModeTrigCountPara(OPTControllerHandleType controllerHandle, int channelIndex, int para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivModeTrigCountPara(OPTControllerHandleType controllerHandle, int channelIndex, int* para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivMulEncoderDirectSW(OPTControllerHandleType controllerHandle, int channelIndex, int enSwitch);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivMulEncoderDirectSW(OPTControllerHandleType controllerHandle, int channelIndex, int* enSwitch);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetEncoderDirection(OPTControllerHandleType controllerHandle, int channelIndex, int direct);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadEncoderDirection(OPTControllerHandleType controllerHandle, int channelIndex, int* direct);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetLineOutputPulseWidth(OPTControllerHandleType controllerHandle, int channelIndex, int pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadLineOutputPulseWidth(OPTControllerHandleType controllerHandle, int channelIndex, int* pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetLineOutputPulseWidthTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadLineOutputPulseWidthTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetLineOutputDelay(OPTControllerHandleType controllerHandle, int channelIndex, int delay);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadLineOutputDelay(OPTControllerHandleType controllerHandle, int channelIndex, int* delay);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetLineOutPWDelayTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadLineOutPWDelayTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFrameOutputPulseWidth(OPTControllerHandleType controllerHandle, int channelIndex, int pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFrameOutputPulseWidth(OPTControllerHandleType controllerHandle, int channelIndex, int* pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFrameOutputPulseWidthTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFrameOutputPulseWidthTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFrameOutputDelay(OPTControllerHandleType controllerHandle, int channelIndex, int delay);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFrameOutputDelay(OPTControllerHandleType controllerHandle, int channelIndex, int* delay);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFrameOutPWDelayTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFrameOutPWDelayTimeUnit(OPTControllerHandleType controllerHandle, int channelIndex, int* timeUnit);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDoubleParaOfIFC(OPTControllerHandleType controllerHandle, int channelIndex, int para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDoubleParaOfIFC(OPTControllerHandleType controllerHandle, int channelIndex, int* para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDivisionParaOfIFC(OPTControllerHandleType controllerHandle, int channelIndex, int para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivisionParaOfIFC(OPTControllerHandleType controllerHandle, int channelIndex, int* para);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFreqDoublePulseWidthOfIFC(OPTControllerHandleType controllerHandle, int channelIndex, int pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDoublePulseWidthOfIFC(OPTControllerHandleType controllerHandle, int channelIndex, int* pulseWidth);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetLinePolarity(OPTControllerHandleType controllerHandle, int channelIndex, int polarity);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadLinePolarity(OPTControllerHandleType controllerHandle, int channelIndex, int* polarity);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetFramePolarity(OPTControllerHandleType controllerHandle, int channelIndex, int polarity);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetUserName(OPTControllerHandleType controllerHandle, String userName);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadUserName(OPTControllerHandleType controllerHandle, StringBuilder userName);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFramePolarity(OPTControllerHandleType controllerHandle, int channelIndex, int* polarity);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_WriteFreqDivTable(OPTControllerHandleType controllerHandle, int channelIndex, int[] table);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_ReadFreqDivTable(OPTControllerHandleType controllerHandle, int channelIndex, int[] table);

        [DllImport(OPTControler_DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        int
        OPTController_SetIntervalTime(OPTControllerHandleType controllerHandle, int time_ms);

        //HACK C# OPTController C API import


        /******************************************************************************************/
        public unsafe
        int
        ConnectionResetByIP(string IP)
        {
            int iRet = 0;
            iRet = OPTController_ConnectionResetByIP(IP);
            return iRet;
        }

        public unsafe
        int
        InitSerialPort(string SerialPortName)
        {
            fixed (OPTControllerHandleType* controllerHandle = &ControllerHandle)
            {
                return OPTController_InitSerialPort(SerialPortName, controllerHandle);
            }
        }

        public unsafe
        int
        ReleaseSerialPort()
        {
            int iRet = OPTController_ReleaseSerialPort(ControllerHandle);
            ControllerHandle = IntPtr.Zero;
            return iRet;
        }

        public unsafe
        int
        CreateEthernetConnectionByIP(string ComName)
        {
            fixed (OPTControllerHandleType* controllerHandle = &ControllerHandle)
            {
                return OPTController_CreateEthernetConnectionByIP(ComName, controllerHandle);
            }
        }

        public unsafe
        int
        CreateEthernetConnectionBySN(string ComName)
        {
            fixed (OPTControllerHandleType* controllerHandle = &ControllerHandle)
            {
                return OPTController_CreateEthernetConnectionBySN(ComName, controllerHandle);
            }
        }

        public unsafe
        int
        DestroyEthernetConnect()
        {
            int iRet = OPTController_DestroyEthernetConnection(ControllerHandle);
            ControllerHandle = IntPtr.Zero;
            return iRet;
        }

        public unsafe
        int
        TurnOnChannel(int Channel)
        {
            return OPTController_TurnOnChannel(ControllerHandle, Channel);
        }

        public unsafe
        int
        TurnOnMultiChannel(int[] ChannelArray, int len)
        {
            return OPTController_TurnOnMultiChannel(ControllerHandle, ChannelArray, len);
        }

        public unsafe
        int
        TurnOffChannel(int Channel)
        {
            return OPTController_TurnOffChannel(ControllerHandle, Channel);
        }

        public unsafe
        int
        TurnOffMultiChannel(int[] ChannelArray, int len)
        {
            return OPTController_TurnOffMultiChannel(ControllerHandle, ChannelArray, len);
        }

        public unsafe
        int
        SetIntensity(int Channel, int Value)
        {
            return OPTController_SetIntensity(ControllerHandle, Channel, Value);
        }

        public unsafe
        int
        SetMultiIntensity(IntensityItem[] IntensityArray, int len)
        {
            return OPTController_SetMultiIntensity(ControllerHandle, IntensityArray, len);
        }

        public unsafe
        int
        ReadIntensity(int Channel, ref int Value)
        {
            fixed (int* value = &Value)
            {
                return OPTController_ReadIntensity(ControllerHandle, Channel, value);
            }
        }

        public unsafe
        int
        SetTriggerWidth(int Channel, int Value)
        {
            return OPTController_SetTriggerWidth(ControllerHandle, Channel, Value);
        }

        public unsafe
        int
        SetMultiTriggerWidth(TriggerWidthItem[] TriggerWidthArray, int len)
        {
            return OPTController_SetMultiTriggerWidth(ControllerHandle, TriggerWidthArray, len);
        }

        public unsafe
        int
        ReadTriggerWidth(int Channel, ref int Value)
        {
            fixed (int* value = &Value)
            {
                return OPTController_ReadTriggerWidth(ControllerHandle, Channel, value);
            }
        }

        public unsafe
        int
        SetHBTriggerWidth(int Channel, int Value)
        {
            return OPTController_SetHBTriggerWidth(ControllerHandle, Channel, Value);
        }

        public unsafe
        int
        SetMultiHBTriggerWidth(HBTriggerWidthItem[] HBTriggerWidthArray, int len)
        {
            return OPTController_SetMultiHBTriggerWidth(ControllerHandle, HBTriggerWidthArray, len);
        }

        public unsafe
        int
        ReadHBTriggerWidth(int Channel, ref int Value)
        {
            fixed (int* value = &Value)
            {
                return OPTController_ReadHBTriggerWidth(ControllerHandle, Channel, value);
            }
        }

        public unsafe
        int
        EnableResponse(int nisResponse)
        {
            return OPTController_EnableResponse(ControllerHandle, nisResponse);
        }

        public unsafe
        int
        EnableCheckSum(int nisCheckSum)
        {
            return OPTController_EnableCheckSum(ControllerHandle, nisCheckSum);
        }

        public unsafe
        int
        EnablePowerOffBackup(int nisSave)
        {
            return OPTController_EnablePowerOffBackup(ControllerHandle, nisSave);
        }

        public unsafe
        int
        ReadSN(StringBuilder SN)
        {
            return OPTController_ReadSN(ControllerHandle, SN);
        }

        public unsafe
        int
        ReadIPConfig(StringBuilder IP, StringBuilder subnetMask, StringBuilder defaultGateway)
        {
            return OPTController_ReadIPConfig(ControllerHandle, IP, subnetMask, defaultGateway);
        }

        public unsafe
        int
        ReadIPConfigBySN(StringBuilder SN, StringBuilder IP, StringBuilder subnetMask, StringBuilder defaultGateway)
        {
            return OPTController_ReadIPConfigBySN(SN, IP, subnetMask, defaultGateway);
        }

        public unsafe
        int
        ReadProperties(int properties, StringBuilder value)
        {
            return OPTController_ReadProperties(ControllerHandle, properties, value);
        }


        public unsafe
        int
        SetEthernetConnectionHeartBeat(UInt32 timeout)
        {
            return OPTController_SetEthernetConnectionHeartBeat(ControllerHandle, timeout);
        }

        public unsafe
        int
        SetMaxCurrent(int channelIndex, int current)
        {
            return OPTController_SetMaxCurrent(ControllerHandle, channelIndex, current);
        }

        public unsafe
        int
        ReadMaxCurrent(int channelIndex, int mode, ref int value)
        {
            fixed (int* nValue = &value)
            {
                return OPTController_ReadMaxCurrent(ControllerHandle, channelIndex, mode, nValue);
            }
        }

        public unsafe
        int
        SetMultiMaxCurrent(MaxCurrentItem[] maxCurrentArray, int arrayLenght)
        {
            return OPTController_SetMultiMaxCurrent(ControllerHandle, maxCurrentArray, arrayLenght);
        }

        public unsafe
        int
        ReadMAC(int properties, StringBuilder MAC)
        {
            return OPTController_ReadMAC(ControllerHandle, MAC);
        }

        public unsafe
        int
        SetTriggerActivation(int channelIndex, int triggerActivation)
        {
            return OPTController_SetTriggerActivation(ControllerHandle, channelIndex, triggerActivation);
        }

        public unsafe
        int
        ReadTriggerActivation(int channelIndex, ref int triggerActivation)
        {

            fixed (int* polarity = &triggerActivation)
            {
                return OPTController_ReadTriggerActivation(ControllerHandle, channelIndex, polarity);
            }
        }


        public unsafe
        int
        SetWorkMode(int workMode)
        {
            return OPTController_SetWorkMode(ControllerHandle, workMode);
        }

        public unsafe
        int
        ReadWorkMode(ref int workMode)
        {
            fixed (int* mode = &workMode)
            {
                return OPTController_ReadWorkMode(ControllerHandle, mode);
            }
        }

        public unsafe
        int
        SetOuterTriggerFrequencyUpperBound(int channelIndex, int maxFrequency)
        {
            return OPTController_SetOuterTriggerFrequencyUpperBound(ControllerHandle, channelIndex, maxFrequency);
        }

        public unsafe
        int
        ReadOuterTriggerFrequencyUpperBound(int channelIndex, ref int maxFrequency)
        {

            fixed (int* frequency = &maxFrequency)
            {
                return OPTController_ReadOuterTriggerFrequencyUpperBound(ControllerHandle, channelIndex, frequency);
            }
        }

        public unsafe
        int
        AutoDetectLoadOnce()
        {
            return OPTController_AutoDetectLoadOnce(ControllerHandle);
        }

        public unsafe
        int
        SetAutoStrobeFrequency(int channelIndex, int frequency)
        {
            return OPTController_SetAutoStrobeFrequency(ControllerHandle, channelIndex, frequency);
        }

        public unsafe
        int
        ReadAutoStrobeFrequency(int channelIndex, ref int frequency)
        {
            fixed (int* nfrequency = &frequency)
            {
                return OPTController_ReadAutoStrobeFrequency(ControllerHandle, channelIndex, nfrequency);
            }

        }

        public unsafe
        int
        EnableDHCP(int nbDHCP)
        {
            return OPTController_EnableDHCP(ControllerHandle, nbDHCP);
        }

        public unsafe
        int
        SetLoadMode(int channelIndex, int loadMode)
        {
            return OPTController_SetLoadMode(ControllerHandle, channelIndex, loadMode);
        }


        public unsafe
        int
        GetVersion(StringBuilder version)
        {
            return OPTController_GetVersion(version);
        }

        public unsafe
        int
        ConnectionResetBySN(StringBuilder serialNumber)
        {
            return OPTController_ConnectionResetBySN(serialNumber);
        }

        public unsafe
        int
        IsConnect()
        {
            return OPTController_IsConnect(ControllerHandle);
        }




        /******************/
        public unsafe
        int
        GetControllerListOnEthernet(StringBuilder snList)
        {
            return OPTController_GetControllerListOnEthernet(snList);
        }

        public unsafe
        int
         GetChannelState(int channelIdx, ref int state)
        {
            fixed (int* nState = &state)
            {
                return OPTController_GetChannelState(ControllerHandle, channelIdx, nState);
            }
        }


        public unsafe
        int
        SetKeepaliveParameter(int keepalive_time, int keepalive_intvl, int keepalive_probes)
        {
            return OPTController_SetKeepaliveParameter(ControllerHandle, keepalive_time, keepalive_intvl, keepalive_probes);
        }

        public unsafe
        int
        EnableKeepalive(int nenable)
        {
            return OPTController_EnableKeepalive(ControllerHandle, nenable);
        }

        public unsafe
        int
        SoftwareTrigger(int channelIndex, int time)
        {
            return OPTController_SoftwareTrigger(ControllerHandle, channelIndex, time);
        }

        public unsafe
        int
        MultiSoftwareTrigger(SoftwareTriggerItem[] softwareTriggerArray, int length)
        {
            return OPTController_MultiSoftwareTrigger(ControllerHandle, softwareTriggerArray, length);
        }


        public unsafe
        int
        ReadStepCount(int channelIndex, ref int count)
        {
            fixed (int* nCount = &count)
            {
                return OPTController_ReadStepCount(ControllerHandle, channelIndex, nCount);
            }
        }

        public unsafe
        int
        SetTriggerMode(int moduleIndex, int mode)
        {
            return OPTController_SetTriggerMode(ControllerHandle, moduleIndex, mode);
        }

        public unsafe
        int
        ReadTriggerMode(int moduleIndex, ref int mode)
        {
            fixed (int* nMode = &mode)
            {
                return OPTController_ReadTriggerMode(ControllerHandle, moduleIndex, nMode);
            }
        }

        public unsafe
        int
        SetCurrentStepIndex(int moduleIndex, int curStepIndex)
        {
            return OPTController_SetCurrentStepIndex(ControllerHandle, moduleIndex, curStepIndex);
        }

        public unsafe
        int
        ReadCurrentStepIndex(int moduleIndex, ref int curStepIndex)
        {
            fixed (int* nCurStepIndex = &curStepIndex)
            {
                return OPTController_ReadCurrentStepIndex(ControllerHandle, moduleIndex, nCurStepIndex);
            }
        }


        public unsafe
        int
        ResetSEQ(int moduleIndex)
        {
            return OPTController_ResetSEQ(ControllerHandle, moduleIndex);
        }


        public unsafe
        int
        SetTriggerDelay(int channelIndex, int triggerDelay)
        {
            return OPTController_SetTriggerDelay(ControllerHandle, channelIndex, triggerDelay);
        }

        public unsafe
        int
        GetTriggerDelay(int channelIndex, ref int triggerDelay)
        {
            fixed (int* nDelay = &triggerDelay)
            {
                return OPTController_GetTriggerDelay(ControllerHandle, channelIndex, nDelay);
            }
        }

        public unsafe
        int
        SetMultiTriggerDelay(TriggerDelayItem[] triggerDelayArray, int length)
        {
            return OPTController_SetMultiTriggerDelay(ControllerHandle, triggerDelayArray, length);
        }

        public unsafe
        int
        SetSeqTable(int moduleIndex, int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth)
        {
            return OPTController_SetSeqTable(ControllerHandle, moduleIndex, seqCount, triggerSource, intensity, pulseWidth);
        }

        public unsafe
        int
        ReadSeqTable(int moduleIndex, ref int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth)
        {

            fixed (int* nseqCount = &seqCount)
            {
                return OPTController_ReadSeqTable(ControllerHandle, moduleIndex, nseqCount, triggerSource, intensity, pulseWidth);
            }
        }


        public unsafe
        int
        SaveSeqFile(int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth, String filePath)
        {
            return OPTController_SaveSeqFile(seqCount, triggerSource, intensity, pulseWidth, filePath);
        }

        public unsafe
        int
        LoadSeqFile(String filePath, ref int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth)
        {

            fixed (int* nseqCount = &seqCount)
            {
                return OPTController_LoadSeqFile(filePath, nseqCount, triggerSource, intensity, pulseWidth);
            }
        }

        public unsafe
        int
        CompareSeqTable(int moduleIndex, String filePath)
        {
            return OPTController_CompareSeqTable(ControllerHandle, moduleIndex, filePath);
        }

        public unsafe
        int
        SaveSeqFileToCSV(int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth, String filePath)
        {
            return OPTController_SaveSeqFile(seqCount, triggerSource, intensity, pulseWidth, filePath);
        }

        public unsafe
        int
        LoadSeqFileFromCSV(String filePath, ref int seqCount, int[] triggerSource, int[] intensity, int[] pulseWidth)
        {

            fixed (int* nseqCount = &seqCount)
            {
                return OPTController_LoadSeqFile(filePath, nseqCount, triggerSource, intensity, pulseWidth);
            }
        }

        public unsafe
        int
        CompareSeqTableFromCSV(int moduleIndex, String filePath)
        {
            return OPTController_CompareSeqTable(ControllerHandle, moduleIndex, filePath);
        }


        public unsafe
        int
        GetControllerChannels(ref int channels)
        {
            fixed (int* nChannel = &channels)
            {
                return OPTController_GetControllerChannels(ControllerHandle, nChannel);
            }
        }

        public unsafe
        int
        ReadKeepaliveSwitchState(ref int state)
        {
            fixed (int* nState = &state)
            {
                return OPTController_ReadKeepaliveSwitchState(ControllerHandle, nState);
            }
        }

        public unsafe
        int
        ReadContinuousKeepaliveTime(ref int time)
        {
            fixed (int* nTime = &time)
            {
                return OPTController_ReadContinuousKeepaliveTime(ControllerHandle, nTime);
            }
        }

        public unsafe
        int
        ReadPacketDeliveryTimes(ref int times)
        {
            fixed (int* nTimes = &times)
            {
                return OPTController_ReadPacketDeliveryTimes(ControllerHandle, nTimes);
            }
        }

        public unsafe
        int
        ReadIntervalTimeOfPropPacket(ref int time)
        {
            fixed (int* nTime = &time)
            {
                return OPTController_ReadIntervalTimeOfPropPacket(ControllerHandle, nTime);
            }
        }

        public unsafe
        int
        ReadOutputBoardVision(StringBuilder vision)
        {
            return OPTController_ReadOutputBoardVision(ControllerHandle, vision);
        }

        public unsafe
        int
        ReadLoadDetectMode(int channelIndex, ref int mode)
        {
            fixed (int* nMode = &mode)
            {
                return OPTController_ReadLoadDetectMode(ControllerHandle, channelIndex, nMode);
            }
        }

        public unsafe
        int
        SetBootProtection(int channelIndex, int mode)
        {
            return OPTController_SetBootProtection(ControllerHandle, channelIndex, mode);
        }

        public unsafe
        int
        ReadModelBootState(int channelIndex, ref int state)
        {
            fixed (int* nState = &state)
            {
                return OPTController_ReadModelBootState(ControllerHandle, channelIndex, nState);
            }
        }

        public unsafe
        int
        SetIPConfiguration(StringBuilder IP, StringBuilder subnetMask, StringBuilder defaultGateway)
        {
            return OPTController_SetIPConfiguration(ControllerHandle, IP, subnetMask, defaultGateway);
        }

        public unsafe
        int
        SetOutputVoltage(int Channel, int Value)
        {
            return OPTController_SetOutputVoltage(ControllerHandle, Channel, Value);
        }

        public unsafe
        int
        ReadOutputVoltage(int Channel, ref int Value)
        {
            fixed (int* nValue = &Value)
            {
                return OPTController_ReadOutputVoltage(ControllerHandle, Channel, nValue);
            }
        }


        public unsafe
        int
        SetTimeUnit(int Channel, int timeUnit)
        {
            return OPTController_SetTimeUnit(ControllerHandle, Channel, timeUnit);
        }

        public unsafe
        int
        ReadTimeUnit(int Channel, ref int timeUnit)
        {
            fixed (int* nValue = &timeUnit)
            {
                return OPTController_ReadTimeUnit(ControllerHandle, Channel, nValue);
            }
        }


        public unsafe
        int
        SetHBTriggerUnit(int channelIndex, int timeUnit)
        {
            return OPTController_SetHBTriggerUnit(ControllerHandle, channelIndex, timeUnit);
        }



        public unsafe
        int
        ReadHBTriggerUnit(int channelIndex, ref int timeUnit)
        {
            fixed (int* nValue = &timeUnit)
            {
                return OPTController_ReadHBTriggerUnit(ControllerHandle, channelIndex, nValue);
            }

        }



        public unsafe
        int
        SetTriggerDelayUnit(int channelIndex, int timeUnit)
        {
            return OPTController_SetTriggerDelayUnit(ControllerHandle, channelIndex, timeUnit);
        }



        public unsafe
        int
        ReadTriggerDelayUnit(int channelIndex, ref int timeUnit)
        {
            fixed (int* nValue = &timeUnit)
            {
                return OPTController_ReadTriggerDelayUnit(ControllerHandle, channelIndex, nValue);
            }

        }


        public unsafe
        int
        SetPercentOfBrighteningCurrent(int channelIndex, int percentage)
        {
            return OPTController_SetPercentOfBrighteningCurrent(ControllerHandle, channelIndex, percentage);
        }



        public unsafe
        int
        ReadPercentOfBrighteningCurrent(int channelIndex, ref int percentage)
        {
            fixed (int* nValue = &percentage)
            {
                return OPTController_ReadPercentOfBrighteningCurrent(ControllerHandle, channelIndex, nValue);
            }

        }



        public unsafe
        int
        SetHBTriggerOutputDutyLimitSwitchState(int channelIndex, int state)
        {
            return OPTController_SetHBTriggerOutputDutyLimitSwitchState(ControllerHandle, channelIndex, state);
        }



        public unsafe
        int
        ReadHBTriggerOutputDutyLimitSwitchState(int channelIndex, ref int state)
        {
            fixed (int* nValue = &state)
            {
                return OPTController_ReadHBTriggerOutputDutyLimitSwitchState(ControllerHandle, channelIndex, nValue);
            }

        }


        public unsafe
        int
        SetHBTriggerOutputDutyRatio(int channelIndex, int ratio)
        {
            return OPTController_SetHBTriggerOutputDutyRatio(ControllerHandle, channelIndex, ratio);
        }



        public unsafe
        int
        ReadHBTriggerOutputDutyRatio(int channelIndex, ref int ratio)
        {
            fixed (int* nValue = &ratio)
            {
                return OPTController_ReadHBTriggerOutputDutyRatio(ControllerHandle, channelIndex, nValue);
            }

        }

        public unsafe
        int
        SetDiffPresureLimitSwitchState(int channelIndex, int state)
        {
            return OPTController_SetDiffPresureLimitSwitchState(ControllerHandle, channelIndex, state);
        }



        public unsafe
        int
        ReadDiffPresureLimitSwitchState(int channelIndex, ref int state)
        {
            fixed (int* nValue = &state)
            {
                return OPTController_ReadDiffPresureLimitSwitchState(ControllerHandle, channelIndex, nValue);
            }

        }

        public unsafe
        int
        SetDBoardHandleIntensityResultSwitchState(int state)
        {
            return OPTController_SetDBoardHandleIntensityResultSwitchState(ControllerHandle, state);
        }

        public unsafe
        int
        ReadDBoardHandleIntensityResultSwitchState(ref int state)
        {
            fixed (int* nValue = &state)
            {
                return OPTController_ReadDBoardHandleIntensityResultSwitchState(ControllerHandle, nValue);
            }
        }

        public unsafe
        int
        SetDOControlSource(int channelIndex, int ctrlSrc)
        {
            return OPTController_SetDOControlSource(ControllerHandle, channelIndex, ctrlSrc);
        }

        public unsafe
        int
        ReadDOControlSource(int channelIndex, ref int ctrlSrc)
        {
            fixed (int* nValue = &ctrlSrc)
            {
                return OPTController_ReadDOControlSource(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOControlMode(int channelIndex, int mode)
        {
            return OPTController_SetDOControlMode(ControllerHandle, channelIndex, mode);
        }

        public unsafe
        int
        ReadDOControlMode(int channelIndex, ref int mode)
        {
            fixed (int* nValue = &mode)
            {
                return OPTController_ReadDOControlMode(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOPolarity(int channelIndex, int polarity)
        {
            return OPTController_SetDOPolarity(ControllerHandle, channelIndex, polarity);
        }

        public unsafe
        int
        ReadDOPolarity(int channelIndex, ref int polarity)
        {
            fixed (int* nValue = &polarity)
            {
                return OPTController_ReadDOPolarity(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOStandbyLevel(int channelIndex, int level)
        {
            return OPTController_SetDOStandbyLevel(ControllerHandle, channelIndex, level);
        }

        public unsafe
        int
        ReadDOStandbyLevel(int channelIndex, ref int level)
        {
            fixed (int* nValue = &level)
            {
                return OPTController_ReadDOStandbyLevel(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOSoftwareLevelControl(int channelIndex, int level)
        {
            return OPTController_SetDOSoftwareLevelControl(ControllerHandle, channelIndex, level);
        }

        public unsafe
        int
        ReadDOSoftwareLevelControl(int channelIndex, ref int level)
        {
            fixed (int* nValue = &level)
            {
                return OPTController_ReadDOSoftwareLevelControl(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOMultiSoftwareLevelControl(DOSoftwareLevelItem[] softwareLevelArray, int length)
        {
            return OPTController_SetDOMultiSoftwareLevelControl(ControllerHandle, softwareLevelArray, length);
        }

        public unsafe
        int
        ReadDOActualOutputLevel(int channelIndex, ref int level)
        {
            fixed (int* nValue = &level)
            {
                return OPTController_ReadDOActualOutputLevel(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDODelay(int channelIndex, int delay)
        {
            return OPTController_SetDODelay(ControllerHandle, channelIndex, delay);
        }

        public unsafe
        int
        ReadDODelay(int channelIndex, ref int delay)
        {
            fixed (int* nValue = &delay)
            {
                return OPTController_ReadDODelay(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDODelayUnit(int channelIndex, int unit)
        {
            return OPTController_SetDODelayUnit(ControllerHandle, channelIndex, unit);
        }

        public unsafe
        int
        ReadDODelayUnit(int channelIndex, ref int unit)
        {
            fixed (int* nValue = &unit)
            {
                return OPTController_ReadDODelayUnit(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOTriggerWidth(int channelIndex, int width)
        {
            return OPTController_SetDOTriggerWidth(ControllerHandle, channelIndex, width);
        }

        public unsafe
        int
        ReadDOTriggerWidth(int channelIndex, ref int width)
        {
            fixed (int* nValue = &width)
            {
                return OPTController_ReadDOTriggerWidth(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOTriggerWidthUnit(int channelIndex, int unit)
        {
            return OPTController_SetDOTriggerWidthUnit(ControllerHandle, channelIndex, unit);
        }

        public unsafe
        int
        ReadDOTriggerWidthUnit(int channelIndex, ref int width)
        {
            fixed (int* nValue = &width)
            {
                return OPTController_ReadDOTriggerWidthUnit(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetIPConfiguration(String IP, String subnetMask, String defaultGateway)
        {
            return OPTController_SetIPConfiguration(ControllerHandle, IP, subnetMask, defaultGateway);
        }

        public unsafe
        int
        SetTriggerFreqDivMode(int channelIndex, int mode)
        {
            return OPTController_SetTriggerFreqDivMode(ControllerHandle, channelIndex, mode);
        }

        public unsafe
        int
        ReadTriggerFreqDivMode(int channelIndex, ref int mode)
        {
            fixed (int* nValue = &mode)
            {
                return OPTController_ReadTriggerFreqDivMode(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDivTrigSrc(int channelIndex, int source)
        {
            return OPTController_SetFreqDivTrigSrc(ControllerHandle, channelIndex, source);
        }

        public unsafe
        int
        ReadFreqDivTrigSrc(int channelIndex, ref int source)
        {
            fixed (int* nValue = &source)
            {
                return OPTController_ReadFreqDivTrigSrc(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDivCount(int channelIndex, int count)
        {
            return OPTController_SetFreqDivCount(ControllerHandle, channelIndex, count);
        }

        public unsafe
        int
        ReadFreqDivCount(int channelIndex, ref int count)
        {
            fixed (int* nValue = &count)
            {
                return OPTController_ReadFreqDivCount(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDivCurrentCount(int channelIndex, int count)
        {
            return OPTController_SetFreqDivCurrentCount(ControllerHandle, channelIndex, count);
        }

        public unsafe
        int
        ReadFreqDivCurrentCount(int channelIndex, ref int count)
        {
            fixed (int* nValue = &count)
            {
                return OPTController_ReadFreqDivCurrentCount(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        ResetFreqDivCurrentCount(int channelIndex, int count)
        {
            return OPTController_ResetFreqDivCurrentCount(ControllerHandle, channelIndex);
        }

        public unsafe
        int
        SetFreqDivHardWareResetSrc(int channelIndex, int source)
        {
            return OPTController_SetFreqDivHardWareResetSrc(ControllerHandle, channelIndex, source);
        }

        public unsafe
        int
        ReadFreqDivHardWareResetSrc(int channelIndex, ref int source)
        {
            fixed (int* nValue = &source)
            {
                return OPTController_ReadFreqDivHardWareResetSrc(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOFreqDivAndMulPara(int channelIndex, int para)
        {
            return OPTController_SetDOFreqDivAndMulPara(ControllerHandle, channelIndex, para);
        }

        public unsafe
        int
        ReadDOFreqDivAndMulPara(int channelIndex, ref int para)
        {
            fixed (int* nValue = &para)
            {
                return OPTController_ReadDOFreqDivAndMulPara(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetDOFreqencyDivision(int channelIndex, int value)
        {
            return OPTController_SetDOFreqencyDivision(ControllerHandle, channelIndex, value);
        }

        public unsafe
        int
        ReadDOFreqencyDivision(int channelIndex, ref int value)
        {
            fixed (int* nValue = &value)
            {
                return OPTController_ReadDOFreqencyDivision(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDivModeTrigRecvCntSW(int channelIndex, int enSwitch)
        {
            return OPTController_SetFreqDivModeTrigRecvCntSW(ControllerHandle, channelIndex, enSwitch);
        }

        public unsafe
        int
        ReadFreqDivModeTrigRecvCntSW(int channelIndex, ref int enSwitch)
        {
            fixed (int* nValue = &enSwitch)
            {
                return OPTController_ReadFreqDivModeTrigRecvCntSW(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDivModeTrigCountPara(int channelIndex, int para)
        {
            return OPTController_SetFreqDivModeTrigCountPara(ControllerHandle, channelIndex, para);
        }

        public unsafe
        int
        ReadFreqDivModeTrigCountPara(int channelIndex, ref int para)
        {
            fixed (int* nValue = &para)
            {
                return OPTController_ReadFreqDivModeTrigCountPara(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDivMulEncoderDirectSW(int channelIndex, int enSwitch)
        {
            return OPTController_SetFreqDivMulEncoderDirectSW(ControllerHandle, channelIndex, enSwitch);
        }

        public unsafe
        int
        ReadFreqDivMulEncoderDirectSW(int channelIndex, ref int enSwitch)
        {
            fixed (int* nValue = &enSwitch)
            {
                return OPTController_ReadFreqDivMulEncoderDirectSW(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetEncoderDirection(int channelIndex, int direct)
        {
            return OPTController_SetEncoderDirection(ControllerHandle, channelIndex, direct);
        }

        public unsafe
        int
        ReadEncoderDirection(int channelIndex, ref int direct)
        {
            fixed (int* nValue = &direct)
            {
                return OPTController_ReadEncoderDirection(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetLineOutputPulseWidth(int channelIndex, int pulseWidth)
        {
            return OPTController_SetLineOutputPulseWidth(ControllerHandle, channelIndex, pulseWidth);
        }

        public unsafe
        int
        ReadLineOutputPulseWidth(int channelIndex, ref int pulseWidth)
        {
            fixed (int* nValue = &pulseWidth)
            {
                return OPTController_ReadLineOutputPulseWidth(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetLineOutputPulseWidthTimeUnit(int channelIndex, int timeUnit)
        {
            return OPTController_SetLineOutputPulseWidthTimeUnit(ControllerHandle, channelIndex, timeUnit);
        }

        public unsafe
        int
        ReadLineOutputPulseWidthTimeUnit(int channelIndex, ref int timeUnit)
        {
            fixed (int* nValue = &timeUnit)
            {
                return OPTController_ReadLineOutputPulseWidthTimeUnit(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetLineOutputDelay(int channelIndex, int delay)
        {
            return OPTController_SetLineOutputDelay(ControllerHandle, channelIndex, delay);
        }

        public unsafe
        int
        ReadLineOutputDelay(int channelIndex, ref int delay)
        {
            fixed (int* nValue = &delay)
            {
                return OPTController_ReadLineOutputDelay(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetLineOutPWDelayTimeUnit(int channelIndex, int timeUnit)
        {
            return OPTController_SetLineOutPWDelayTimeUnit(ControllerHandle, channelIndex, timeUnit);
        }

        public unsafe
        int
        ReadLineOutPWDelayTimeUnit(int channelIndex, ref int timeUnit)
        {
            fixed (int* nValue = &timeUnit)
            {
                return OPTController_ReadLineOutPWDelayTimeUnit(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFrameOutputPulseWidth(int channelIndex, int pulseWidth)
        {
            return OPTController_SetFrameOutputPulseWidth(ControllerHandle, channelIndex, pulseWidth);
        }

        public unsafe
        int
        ReadFrameOutputPulseWidth(int channelIndex, ref int pulseWidth)
        {
            fixed (int* nValue = &pulseWidth)
            {
                return OPTController_ReadFrameOutputPulseWidth(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFrameOutputPulseWidthTimeUnit(int channelIndex, int pulseWidth)
        {
            return OPTController_SetFrameOutputPulseWidthTimeUnit(ControllerHandle, channelIndex, pulseWidth);
        }

        public unsafe
        int
        ReadFrameOutputPulseWidthTimeUnit(int channelIndex, ref int pulseWidth)
        {
            fixed (int* nValue = &pulseWidth)
            {
                return OPTController_ReadFrameOutputPulseWidthTimeUnit(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFrameOutputDelay(int channelIndex, int delay)
        {
            return OPTController_SetFrameOutputDelay(ControllerHandle, channelIndex, delay);
        }

        public unsafe
        int
        ReadFrameOutputDelay(int channelIndex, ref int delay)
        {
            fixed (int* nValue = &delay)
            {
                return OPTController_ReadFrameOutputDelay(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFrameOutPWDelayTimeUnit(int channelIndex, int timeUnit)
        {
            return OPTController_SetFrameOutPWDelayTimeUnit(ControllerHandle, channelIndex, timeUnit);
        }

        public unsafe
        int
        ReadFrameOutPWDelayTimeUnit(int channelIndex, ref int timeUnit)
        {
            fixed (int* nValue = &timeUnit)
            {
                return OPTController_ReadFrameOutPWDelayTimeUnit(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDoubleParaOfIFC(int channelIndex, int para)
        {
            return OPTController_SetFreqDoubleParaOfIFC(ControllerHandle, channelIndex, para);
        }

        public unsafe
        int
        ReadFreqDoubleParaOfIFC(int channelIndex, ref int para)
        {
            fixed (int* nValue = &para)
            {
                return OPTController_ReadFreqDoubleParaOfIFC(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDivisionParaOfIFC(int channelIndex, int para)
        {
            return OPTController_SetFreqDivisionParaOfIFC(ControllerHandle, channelIndex, para);
        }

        public unsafe
        int
        ReadFreqDivisionParaOfIFC(int channelIndex, ref int para)
        {
            fixed (int* nValue = &para)
            {
                return OPTController_ReadFreqDivisionParaOfIFC(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFreqDoublePulseWidthOfIFC(int channelIndex, int pulseWidth)
        {
            return OPTController_SetFreqDoublePulseWidthOfIFC(ControllerHandle, channelIndex, pulseWidth);
        }

        public unsafe
        int
        ReadFreqDoublePulseWidthOfIFC(int channelIndex, ref int pulseWidth)
        {
            fixed (int* nValue = &pulseWidth)
            {
                return OPTController_ReadFreqDoublePulseWidthOfIFC(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetLinePolarity(int channelIndex, int polarity)
        {
            return OPTController_SetLinePolarity(ControllerHandle, channelIndex, polarity);
        }

        public unsafe
        int
        ReadLinePolarity(int channelIndex, ref int polarity)
        {
            fixed (int* nValue = &polarity)
            {
                return OPTController_ReadLinePolarity(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetFramePolarity(int channelIndex, int polarity)
        {
            return OPTController_SetFramePolarity(ControllerHandle, channelIndex, polarity);
        }

        public unsafe
        int
        ReadFramePolarity(int channelIndex, ref int polarity)
        {
            fixed (int* nValue = &polarity)
            {
                return OPTController_ReadFramePolarity(ControllerHandle, channelIndex, nValue);
            }
        }

        public unsafe
        int
        SetUserName(String userName)
        {
            return OPTController_SetUserName(ControllerHandle, userName);
        }

        public unsafe
        int
        ReadUserName(StringBuilder userName)
        {
            return OPTController_ReadUserName(ControllerHandle, userName);
        }

        public unsafe
        int
        SetFramePolarity(int channelIndex, int[] table)
        {
            return OPTController_WriteFreqDivTable(ControllerHandle, channelIndex, table);
        }

        public unsafe
        int
        ReadFramePolarity(int channelIndex, int[] table)
        {
                return OPTController_ReadFreqDivTable(ControllerHandle, channelIndex, table);
        }

        public unsafe
        int
        SetIntervalTime(int time_ms)
        {
            return OPTController_SetIntervalTime(ControllerHandle, time_ms);
        }
        //HACK C# OPTController C# API
    }
}
