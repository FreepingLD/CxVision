/******************************************************************************
 *                               SmartRay API Types                           *
 *----------------------------------------------------------------------------*
 * Copyright (c) SmartRay GmbH 2018.  All Rights Reserved.                    *
 *----------------------------------------------------------------------------*
 * Title    SR_API_Types.h                                                    *
 * Version	SR_API_VERSION                                                    *
 ******************************************************************************/

/**
 * @file SR_API_Types.h
 * @author SmartRay GmbH
 * @date 2018
 * @brief SmartRay enums and types
 * @see http://www.smartray.com
 */

#ifndef SR_API_TYPESh
#define SR_API_TYPESh

#include <stdio.h>
#include <stdint.h>

#include "SR_API_Defines.h"

/** \name Sensor connection status: */
/**@{*/ 
#define STATE_CAMERA_CLOSED 			0    /**< Camera closed.*/
#define STATE_CONNECT_TO_CAMERA 		10   /**< Connect to camera.*/ 
#define STATE_CONNECT_TO_CAMERA_DELAYED 20   /**< Connect to camera delayed.*/  
#define STATE_CAMERA_CONNECTED			30   /**< Camera connected.*/
/**@}*/ 

/**
 *	\enum SensorConnectionStatus
    \brief Sensor connection status.
 */
typedef enum SensorConnectionStatus
{
	SensorConnectionStatus_SensorClosed 			= STATE_CAMERA_CLOSED,
	SensorConnectionStatus_ConnectToSensor 			= STATE_CONNECT_TO_CAMERA,
	SensorConnectionStatus_ConnectToSensorDelayed 	= STATE_CONNECT_TO_CAMERA_DELAYED,
	SensorConnectionStatus_SensorConnected 			= STATE_CAMERA_CONNECTED,
}SensorConnectionStatus;

/** \name Message types: */
/**@{*/ 
#define MSGTYPE_CONNECTION		1   /**< Connection message.*/
#define MSGTYPE_INFO			2   /**< Info message.*/
#define MSGTYPE_ERROR			3   /**< Error message.*/
#define MSGTYPE_DATA			4   /**< Data message.*/
/**@}*/ 

/**
 *	\enum MessageType
    \brief Message type.
 */
typedef enum MessageType
{
	MessageType_Connection 	= MSGTYPE_CONNECTION,
	MessageType_Info 		= MSGTYPE_INFO,
	MessageType_Error 		= MSGTYPE_ERROR,
	MessageType_Data 		= MSGTYPE_DATA,
}MessageType;

/** \name Message groups: */
/**@{*/ 
#define MSGGROUP_STATUS			1   /**< Status message group.*/
#define MSGGROUP_SW_APPLICERROR 2   /**< Software application error message group.*/
#define MSGGROUP_HW_APPLICERROR 3   /**< Harware application error message group.*/
#define MSGGROUP_API			4   /**< API message group.*/
#define MSGGROUP_MSR			5   /**< MSR message group.*/
/**@}*/ 

/** \name Data messages: */
/**@{*/ 
#define DATATYPE_IO				1
#define DATATYPE_CAMTEMP  		3
#define DATATYPE_SYSPAR	 		10
#define DATATYPE_MPAR    		11
/**@}*/ 

/** \name Error messages: */
/**@{*/ 
#define MSGGROUP_API_CORRUPTNUMOFCAMS					1		/**< API, corrupted number of cameras.*/
#define MSGGROUP_API_INVALIDFILENAME					2		/**< API, invalid filename.*/
#define MSGGROUP_API_CORRUPTPARAMETER					3		/**< API, corrupted parameter.*/
#define MSGGROUP_API_NOREFLEXPARAMETERFILE				4		/**< API, no reflex parameter file.*/
#define MSGGROUP_API_INVALIDPARTNUMBER                  5		/**< API, invalid part number.*/
#define MSGGROUP_API_INVALIDCONFIGURATION               6		/**< API, invalid configuration.*/
#define MSGGROUP_STATUS_TCPTIMEOUT						1		/**< Status, TCP timeout.*/
#define MSGGROUP_STATUS_TCPSENDERROR					2		/**< Status, TCP send error.*/
#define MSGGROUP_STATUS_TCPRECVERROR					4		/**< Status, TCP receive error.*/
#define MSGGROUP_STATUS_FLASHWRITEERROR					5		/**< Status, flash erase error.*/
#define MSGGROUP_STATUS_FLASHERASEERROR					6		/**< Status, flash erase error.*/
#define MSGGROUP_STATUS_NOPARAMSLOADED					7		/**< Status, no parameters loaded.*/
#define MSGGROUP_STATUS_COMPATIBILITY					8		/**< Status, compatibility issue.*/
#define MSGGROUP_STATUS_MISSING_PROFILES					16		/**< Status, missing profiles for acquisition.*/
#define MSGGROUP_SW_APPLICERROR_BUFFER_OVERFLOW			1		/**< Software application error, buffer overflow.*/
#define MSGGROUP_HW_APPLICERROR_FIFO_OVERFLOW			2		/**< Hardware application error, FIFO overflow.*/
#define MSGGROUP_HW_APPLICERROR_CORRELATOR_OVERFLOW		3		/**< Hardware application error, correlator overflow.*/	
#define MSGGROUP_HW_APPLICERROR_CORRELATOR_SPEED		4		/**< Hardware application error, correlator speed.*/
#define MSGGROUP_REFLECTIONFILTERERR					5       /**< Reflection filter error.*/
#define MSGGROUP_HW_APPLICERROR_UNKNOWN					1000    /**< Hardware uknown application error.*/
#define MSGGROUP_HW_NOT_INITIALIZED  					1001    /**< Hardware not initialized.*/
#define MSGGROUP_STATUS_UNKNOWNCOMMAND					8       /**< Status unknown command.*/
#define MSGGROUP_MSR_MERGEERROR_NOPACKETS				1       /**< MSR merge error, no packets.*/
#define MSGGROUP_MSR_MERGEERROR_TOOMANYPACKETS			2       /**< MSR merge error, too many packets.*/
/**@}*/ 

/** \name Info messages: */
/**@{*/ 
#define CAMERA_DISCONNECTED						0   /**< Camera disconnected.*/
#define CAMERA_CONNECTED						1   /**< Camera connected.*/
#define MSGGROUP_API_INITIALIZED				1   /**< API initialized.*/
#define MSGGROUP_STATUS_REPLY					1   /**< Status reply.*/
#define MSGGROUP_STATUS_PARAMS_RECEIVED			2   /**< Status parameters received.*/
#define MSGGROUP_STATUS_PARAMS_FLASHWRITTEN		3   /**< Status parameters written in flash.*/
#define MSGGROUP_REFLECTIONFILTERSUCCESS		5   /**< Reflection filter success.*/
#define MSGGROUP_FIRMWARE_UPDATE_STATUS         4   /**< Firmware update status.*/
/**@}*/ 

/** \name Calibration file (LUT) specific messages: */
/**@{*/ 
#define MSGGROUP_LUT_STORE_SUCCESS				6   /**< LUT store success.*/
#define MSGGROUP_LUT_WRITE_FAILED				7   /**< LUT write failed.*/
#define MSGGROUP_LUT_READ_OUTOFRANGE			8   /**< LUT read our of range.*/
#define MSGGROUP_LUT_NODATA_IN_FLASH			9   /**< LUT no data in flash.*/
#define MSGGROUP_LUT_SEND_DATA_OVERFLOW			10  /**< LUT send data overflow.*/
#define MSGGROUP_LUT_HEADER_RECEIVED			11  /**< LUT header received.*/
#define MSGGROUP_LUT_DATA_RECEIVED				12  /**< LUT data received.*/
#define MSGGROUP_LUT_HEADER_CORRUPT				13  /**< LUT header corrupt.*/
#define MSGGROUP_LUT_DATA_CORRUPT				14  /**< LUT data corrupt.*/
#define MSGGROUP_LUT_DATA_INVALID				15  /**< LUT data invalid.*/ 
/**@}*/ 

/**
 *	\enum SubMessageType
    \brief Sub Message type.
 */
typedef enum SubMessageType
{
	// Info messages:
	SubMessageType_Info_Status = MSGGROUP_STATUS,
	SubMessageType_Info_Api = MSGGROUP_API,
	SubMessageType_Info_ApiInitialized = MSGGROUP_API_INITIALIZED,
	SubMessageType_Info_StatusReply = MSGGROUP_STATUS_REPLY,
	SubMessageType_Info_ParameterReceived = MSGGROUP_STATUS_PARAMS_RECEIVED,
	SubMessageType_Info_ParameterFlashRewritten = MSGGROUP_STATUS_PARAMS_FLASHWRITTEN,
	SubMessageType_Info_ReflectionFilterSucccess = MSGGROUP_REFLECTIONFILTERSUCCESS,
	SubMessageType_Info_CalibrationFileHeaderReceived = MSGGROUP_LUT_HEADER_RECEIVED,
	SubMessageType_Info_CalibrationFileDataReceived = MSGGROUP_LUT_DATA_RECEIVED,
	// Error messages:
	SubMessageType_Error_SoftwareApplicationError = MSGGROUP_SW_APPLICERROR,
	SubMessageType_Error_HardwareApplicationError = MSGGROUP_HW_APPLICERROR,
	SubMessageType_Error_Api = MSGGROUP_API,
	SubMessageType_Error_CalibrationFileNoDataInFlash = MSGGROUP_LUT_NODATA_IN_FLASH,
	SubMessageType_Error_CalibrationFileSendDataOverflow = MSGGROUP_LUT_SEND_DATA_OVERFLOW,
	SubMessageType_Error_CalibrationFileHeaderCorrupt = MSGGROUP_LUT_HEADER_CORRUPT,
	SubMessageType_Error_CalibrationFileDataCorrupt = MSGGROUP_LUT_DATA_CORRUPT,
	SubMessageType_Error_CalibrationFileDataInvalid = MSGGROUP_LUT_DATA_INVALID,
	// Connection related messages:
	SubMessageType_Connection_SensorDisconnected = CAMERA_DISCONNECTED,
	SubMessageType_Connection_SensorConnected = CAMERA_CONNECTED,
	// Data/Misc messages:
	SubMessageType_Data_Io = DATATYPE_IO,
	SubMessageType_Data_SensorTemperature = DATATYPE_CAMTEMP,
	SubMessageType_Data_SystemParameter = DATATYPE_SYSPAR,
	SubMessageType_Data_MachineParameter = DATATYPE_MPAR,
}SubMessageType;

/** \name Image data types: */
/**@{*/ 
#define DATATYPE_LIVEIMAGE		0    /**< Image data type 'Live image'.*/
#define DATATYPE_PROFILE		1    /**< Image data type 'Profile only'.*/
#define DATATYPE_INTENSITY		2    /**< Image data type 'Intensity only'.*/
#define DATATYPE_PIL	    	3    /**< Image data type 'PIL' (Profile, Intensity, LaserLineThickness).*/
#define DATATYPE_PI				9    /**< Image data type 'PI' (Profile, Intensity).*/
#define DATATYPE_ZMAP			10   /**< Image data type 'Z-Map only'.*/
#define DATATYPE_ZI				11   /**< Image data type 'ZI' (Z-map, Intensity).*/
#define DATATYPE_ZIL			12   /**< Image data type 'ZIL' (Z-map, Intensity, LaserLineThickness).*/
#define DATATYPE_POINTCLOUD     13   /**< Image data type 'Point cloud'.*/
#define DATATYPE_TP             14   /**< Image data type 'TP'.*/
/**@}*/ 

/**
 *	\enum ImageDataType
    \brief Image data type, to identify data as received in callbacks.
 */
typedef enum ImageDataType
{
	ImageDataType_LiveImage = DATATYPE_LIVEIMAGE,
	ImageDataType_Profile = DATATYPE_PROFILE,
	ImageDataType_Intensity = DATATYPE_INTENSITY,
	ImageDataType_ProfileIntensityLaserLineThickness = DATATYPE_PIL,
	ImageDataType_ProfileIntensity = DATATYPE_PI,
	ImageDataType_ZMap = DATATYPE_ZMAP,
	ImageDataType_ZMapIntensity = DATATYPE_ZI,
	ImageDataType_ZMapIntensityLaserLineThickness = DATATYPE_ZIL,
	ImageDataType_PointCloud = DATATYPE_POINTCLOUD,
	ImageDataType_Invalid = -1,
}ImageDataType;

/** \name Image aquisition types: */
/**@{*/ 
#define IMAGETYPE_PROFILE				0   /**< Image aquisition type 'Profile only'.*/
#define IMAGETYPE_INTENSITY				1   /**< Image aquisition type 'Intensity only' .*/
#define IMAGETYPE_PIL					2   /**< Image aquisition type 'PIL' (Profile, Intensity, LaserLineThickness).*/
#define IMAGETYPE_PI					3   /**< Image aquisition type 'PI' (Profile, Intensity).*/
#define IMAGETYPE_ZMAP					4   /**< Image aquisition type 'Z-Map only'.*/
#define IMAGETYPE_ZI					5   /**< Image aquisition type 'ZI' (Z-map, Intensity).*/
#define IMAGETYPE_ZIL					6   /**< Image aquisition type 'ZIL' (Z-map, Intensity, LaserLineThickness).*/
#define IMAGETYPE_LIVEIMAGE				7   /**< Image aquisition type 'Live image'.*/
#define IMAGETYPE_POINTCLOUD            8   /**< Image aquisition type 'Point cloud'.*/
/**@}*/ 

/**
 *	\enum ImageAquisitionType
    \brief Image aquisition type.
 */
typedef enum ImageAquisitionType
{
	ImageAquisitionType_Profile = IMAGETYPE_PROFILE,
	ImageAquisitionType_Intensity = IMAGETYPE_INTENSITY,
	ImageAquisitionType_ProfileIntensityLaserLineThickness = IMAGETYPE_PIL,
	ImageAquisitionType_ProfileIntensity = IMAGETYPE_PI,
	ImageAquisitionType_ZMap = IMAGETYPE_ZMAP,
	ImageAquisitionType_ZMapIntensity = IMAGETYPE_ZI,
	ImageAquisitionType_ZMapIntensityLaserLineThickness = IMAGETYPE_ZIL,
	ImageAquisitionType_LiveImage = IMAGETYPE_LIVEIMAGE,
	ImageAquisitionType_PointCloud = IMAGETYPE_POINTCLOUD
}ImageAquisitionType;

/** \name Acquisition modes: */
/**@{*/ 
#define SNAPSHOT_MODE			0   /**< Acquisition mode 'snapshot mode'.*/
#define REPEAT_SNAPSHOT_MODE	1   /**< Acquisition mode 'repeat snapshot mode'.*/
/**@}*/ 

/**
 *	\enum AcquisitionMode
    \brief Acquisition mode.
 */
typedef enum AcquisitionMode
{
	AcquisitionMode_Snapshot = SNAPSHOT_MODE,
	AcquisitionMode_RepeatSnapshot = REPEAT_SNAPSHOT_MODE,
}AcquisitionMode;

/** \name Trigger edge modes: */
/**@{*/ 
#define RISING_EDGE		1   /**< Trigger edge mode 'rising edge'.*/
#define FALLING_EDGE	0   /**< Trigger edge mode 'falling edge'.*/
/**@}*/ 

/**
 *	\enum TriggerEdgeMode
    \brief Trigger edge mode.
 */
typedef enum TriggerEdgeMode
{
	TriggerEdgeMode_RisingEdge = RISING_EDGE,
	TriggerEdgeMode_FallingEdge = FALLING_EDGE,
	TriggerEdgeMode_Both = 2
}TriggerEdgeMode;

/**
 *	\enum DataTriggerMode
    \brief Data trigger mode.
 */
typedef enum DataTriggerMode
{
	DataTriggerMode_FreeRunning = 0,
	DataTriggerMode_Internal = 1,
	DataTriggerMode_External = 2
}DataTriggerMode;

/**
 *	\enum DataTriggerSource
    \brief Data trigger source.
 */
typedef enum DataTriggerSource
{
	DataTriggerSource_Input1 = 0,
	DataTriggerSource_Input2 = 1,
	DataTriggerSource_Combined = 2,
	DataTriggerSource_QuadEncoder = 4
}DataTriggerSource;

/**
 *	\enum StartTriggerSource
    \brief Start trigger source.
 */
typedef enum StartTriggerSource
{
	StartTriggerSource_Input0 = 0,
	StartTriggerSource_Input1 = 1,
	StartTriggerSource_Input2 = 2,
	StartTriggerSource_Input3 = 3,
	StartTriggerSource_None = -1
}StartTriggerSource;

/**
 *	\enum DigitalOutput
    \brief Digital output.
 */
typedef enum DigitalOutput
{
	DigitalOutput_Channel1 = 0,
	DigitalOutput_Channel2 = 1,
}DigitalOutput;

/**
 *	\enum LaserMode
    \brief Laser mode.
 */
typedef enum LaserMode
{
	LaserMode_PulsedMode 	= 0,
	LaserMode_ContinousMode = 1
}LaserMode;

/**
 *	\enum MultipleExposureMergeModeType
    \brief Multi-exposure modes.
 */
typedef enum MultipleExposureMergeModeType
{
	MultipleExposureMergeModeType_Merge = 0, // rolling merge (legacy) 
	MultipleExposureMergeModeType_No_Merge = 1, // no merge
	MultipleExposureMergeModeType_No_Merge_With_Prune = 2, // no merge + prune i.e. remove inaccurate points
	MultipleExposureMergeModeType_HighAccuracy_Point_Selector = 3, // selects accurate points with intelligence from laser line
	MultipleExposureMergeModeType_HighAccuracy_Profile_Selector = 4  // selects accurate points with intelligence from laser line
}MultipleExposureMergeModeType;

/**
 *	\enum MultiExposureMergeModeOptimalValueType
    \brief Type of Multi-Exposure Merge-Mode optimal value.
 */
typedef enum MultiExposureMergeModeOptimalValueType
{
    MultiExposureMergeModeOptimalValueType_LaserLineThickness = 0,
    MultiExposureMergeModeOptimalValueType_Intensity = 1
}MultiExposureMergeModeOptimalValueType;

/**
 *	\enum MSRMode
    \brief Multi-Sensor Registration (MSR) modes.
 */
typedef enum MSRMode
{
	MSRMode_FOV = 1, 
	MSRMode_MR = 2
}MSRMode;

/**
* \enum MSRMergeModeType
* \brief Multi-Sensor Registration (MSR) merge modes.
*        MSRMergeModeType_Default: default configuration
*        MSRMergeModeType_MinimumZ: always take lowest z-value
*        MSRMergeModeType_MaximumZ: always take highest z-value
*/
typedef enum MSRMergeModeType
{
	MSRMergeModeType_Default = 0, //! Default merge mode (implementation specific)
	MSRMergeModeType_MinimumZ, //! Always take minimum z-value
	MSRMergeModeType_MaximumZ //! Always take maximum z-value
}MSRMergeModeType;

/** \name Trigger edge modes: */
/**@{*/ 
#define LOG_DISABLE				0   /**< Logging disabled. */
#define LOG_ENABLE				1   /**< Logging enabled. */
#define LOG_LEVEL_ALL			1 	/**< Log 'Sensor - API - Application'. */
#define TIMING_LEVEL 			3	/**< Log 'Sensor - API - Application' data timings. */
#define TIMING_LEVEL_WITHALIVE	4	/**< Log 'Sensor - API - Application' data timings and additionally logs Alive timings. */
#define DUMP_LEVEL 				5 	/**< Log Everything. */
/**@}*/ 

/**
 *	\enum LogLevel
    \brief Logging level.
 */
typedef enum LogLevel
{		
	LogLevel_None = LOG_DISABLE,
	LogLevel_Default = LOG_ENABLE,
	LogLevel_All = LOG_LEVEL_ALL,
	LogLevel_AllAndTimings = TIMING_LEVEL, 
	LogLevel_AllAndTimingsWithAlive = TIMING_LEVEL_WITHALIVE,
	LogLevel_Verbose = DUMP_LEVEL,
}LogLevel;

/**
 *	\enum SmoothingPresets
    \brief Post-processing smoothing filter presets.
 */
typedef enum SmoothingPresets
{
	SMOOTH_MINIMAL, 
	SMOOTH_LIGHT, 
	SMOOTH_MEDIUM, 
	SMOOTH_STRONG, 
	SMOOTH_MAXIMUM 
}SmoothingPresets;

/**
 *	\enum ImagerOverheadTime
    \brief Imager overhead time.
 */
typedef enum ImagerOverheadTime
{
	ImagerOverheadTime_ZROT = 0,
	ImagerOverheadTime_NROT = 1
}ImagerOverheadTime;

/**
*	\enum ImagerOverheadTime
\brief Imager overhead time.
*/
typedef enum DataGeneration3DModeType
{
	DataGeneration3DMode_Unknown = -1,
	DataGeneration3DMode_Default = 0,
	DataGeneration3DMode_TopEdgeOfLaserLine,
	DataGeneration3DMode_BottomEdgeOfLaserLine
} DataGeneration3DModeType;

/**
*	\enum SmartXtractAlgorithmType
\brief Exposure mode for reflection filter.
*/
typedef enum SmartXtractAlgorithmType
{
	SmartXtractAlgorithm_Unknown = -1,
	SmartXtractAlgorithm_SingleProfileAccuracy = 0,
	SmartXtractAlgorithm_MultiProfileAccuracy
} SmartXtractAlgorithmType;

// 3D coordinate system 
typedef struct
{
	float x; 
	float y;
	float z;
} SR_3DPOINT;

/**
 *	\struct SRSensor
    \brief Sensor object.
 */
typedef struct SR_API_EXPORT SRSensor
{
	int cam_index;
	char name[260];
	char IPAdr[17];
	unsigned short portnum;
	int command;
	unsigned char header[HEADERSIZE_USER];
	void* pcamdata;
	int camdatasize;
	int dataavailable;
	int connectionstate;
	int tcp_handle;
	int active;
	int alive_time;
	int archiv_active;
	FILE *archiv_handle;
	int acknowledge;
	void *lut;
	void *usercbf;
	int digio_out[4];
	int digio_in[4];
	int laser_status;
	int laserlight;
	int fps;
	int tps;
	float Temp;
	int running;    
} SRSensor;

/**
 *	\typedef CAMDESC
    \brief CAMera DESCriptor.
 */
typedef SRSensor CAMDESC;

/**
 \struct SR_MetaData
 \brief Metadata object.
        StartTriggerNb: Sequence number (within the current acquisition). 1-based.
        DataTriggerNb: Trigger number (within the current acquisition). 1-based.
        ProfileNb: Profile number (might be different from trigger_number in case of multi-exposure). 1-based.
        TimeStamp: Timestamp in nanoseconds since start of sensor. Overflow every ~40s.
        TimeStampSequence: Timestamp in nanoseconds since start trigger.
        Input_0_State: State for input signal 0. Value 0 is low/falling, 1 is high/rising.
        Input_1_State: State for input signal 1. Value 0 is low/falling, 1 is high/rising.
        QuadStepCountFiltered: Count of quad_step direction filtered since acquisition start.
        QuadStepCountRaw: Count of quad_step raw (int32_t) since acquisition start.
        TriggerOverflow: Trigger overflow.
        OutputStatus: Output status. Bit 1 = Output 0, Bit2 Output 1 . Value 0 is low, 1 is high.
        DataTriggerOverflowCnt: Count data trigger overflows after second data trigger in 31 MSB bits, first sequence second data trigger overflow indicator is LSB.
*/
struct SR_API_EXPORT SR_MetaData
{
    uint32_t StartTriggerNb;
    uint32_t DataTriggerNb;
    uint32_t ProfileNb;    
    uint64_t TimeStamp;
    uint64_t TimeStampSequence;
    uint32_t Input_0_State;
    uint32_t Input_1_State;
    int32_t QuadStepCountFiltered;
    int32_t QuadStepCountRaw;
    uint32_t TriggerOverflow;
    uint32_t OutputStatus;
    uint32_t DataTriggerOverflowCnt;
};

// Callback function types	
// Live Image callback

/**
 * \typedef Callback_LiveImage
   \brief Callback type for Live Image.
 */
typedef int(*Callback_LiveImage) (SRSensor* sensor, ImageDataType imageType, int originX, int height, int width, uint8_t* liveImage);

/**
 * \typedef Callback_PilImage
   \brief Callback type for PIL image (non-calibrated).
 */
typedef int(*Callback_PilImage) (SRSensor *sensor, ImageDataType imageType, int originX, int height, int width, uint16_t* profileImage, uint16_t* intensityImage, uint16_t* lltImage, int numExtData, void *extData);

/**, 
 * \typedef Callback_ZilImage
   \brief Callback type for ZIL image (calibrated).
 */
typedef int(*Callback_ZilImage) (SRSensor* sensor, ImageDataType imageType, int height, int width, float verticalRes, float horizontalRes, uint16_t* zMap, uint16_t* intensityZmap, uint16_t* lltZmap, float originYMillimeters, int numExtData, void* extdata);

/**
 * \typedef Callback_PointCloud
   \brief Callback type for Point Cloud (calibrated).
 */
typedef int(*Callback_PointCloud) (
	SRSensor* sensor, ImageDataType dattyp,
	uint32_t numPoints, uint32_t numProfile,SR_3DPOINT *point_cloud,
	uint16_t* intensity, uint16_t* laserlinethickness,
	uint32_t *profileIdx, uint32_t *columnIdx,
	uint32_t numExtData, void *extData);

/**
 * \typedef Callback_MSRPointCloud
   \brief Callback type for MSR Point Cloud (calibrated).
 */
typedef int(*Callback_MSRPointCloud) (SRSensor* md, uint32_t dattyp, uint32_t numPoints,
    SR_3DPOINT *point_cloud, uint16_t *intensity, uint16_t *laserlinethickness, 
    uint32_t* sensorIdx, uint32_t *profileIdx, uint32_t *pointIdx, 
    uint32_t numMSRExtData, void *extMSRData);

/**
 * \typedef Callback_StatusMessage
   \brief Callback type for Sensor status message.
 */
typedef int(*Callback_StatusMessage) (SRSensor* sensor, MessageType msgType, SubMessageType subMsgType, int msgData, char *msg);

#endif // SR_API_TYPESh
