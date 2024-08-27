/******************************************************************************
 *                       SmartRay API Deprecated Functions                    *
 *																			  *
 * Deprecated SmartRay API functions are for backward compatibility only      *
 *
 * It is strongly recommended to use API functions listed in SR_API_public.h  *
 *----------------------------------------------------------------------------*
 * Copyright (c) SmartRay GmbH 2018. All Rights Reserved.		              *
 *----------------------------------------------------------------------------*
 * Title    SR_API_Deprecated.h                                               *
 * Version	SR_API_VERSION                                                    *
 ******************************************************************************/

#ifndef SR_API_DEPRECATEDh
#define SR_API_DEPRECATEDh

#include "SR_API_Types.h"

// Data type definitions
#define DATATYPE_RAWIMAGE				    	0
#define DATATYPE_PROFILE				    	1
#define DATATYPE_INTENSITY				    	2
#define DATATYPE_PROFILE_PLUS_INTENSITY	    	3
#define DATATYPE_ADVANCED				    	4
#define DATATYPE_ADVZC_PROFILE   				5
#define DATATYPE_ADVANCED_HDR					6
#define DATATYPE_ADVZC_PROFILE_PLUS_INTENSITY  	7
#define DATATYPE_ADVZC_INTENSITY 				8
#define DATATYPE_PROFILE_AND_INTENSITY_ONLY		9
#define DATATYPE_ZMAP							10
#define DATATYPE_ZI								11
#define DATATYPE_ZIL							12
#define DATATYPE_POINTCLOUD                     13
#define DATATYPE_IOSTATUS				    	255

// Maximum number of callback functions that can be registered
#define MAXNUM_SRCB_CAMDATA 			8
#define MAXNUM_SRCB_CAMDATAEXT  		8
#define MAXNUM_SRCB_RAWDATA 			32
#define MAXNUM_SRCB_PIXELMODE 			2
#define MAXNUM_SRCB_SENSORDEFAULTMODE 	8
#define MAXNUM_SRCB_SENSORREFLEXFILTERTMODE 8

//status message callback function prototype
#define STATUS_MSG_ARGLST SRSensor* cd, int msgType, int msgdata1, int msgdata2, char *msg

// Reserved sensor data communication channels
#define CAMCB_RESERVED 0x2000 - 0x2fff
#define DEFAULT_PROFILEIMAGE_CHANNEL	0x8020
#define MAXREMAPDATALEN 1296

typedef struct
{
	int x;
	int y;
	int z;
} SRPIXEL;

#define SRCB_RAWDATAMODE			0
typedef int(*ucbf_rawdatamode)(CAMDESC* cd);
#define SRCB_CAMDATAMODE			1
typedef int(*ucbf_camdatamode)(CAMDESC* cd, int dattyp, int originX, int height, int width, void* pdata);
#define SRCB_CAMDATAEXTMODE			2
typedef int(*ucbf_camdataextmode)(CAMDESC* cd, int dattyp, int originX, int numlines, int width, void* data, int len_extdata, void* extdata);
#define SRCB_SENSORREFLEXFILTERMODE	5
typedef int(*ucbf_sensorreflexfiltermode)(CAMDESC* cd, int dattyp, int originX, int numlines, int width, void* profile, void* intensity, void* llt, int numExtData, void* extdata);
#define SRCB_SENSORDEFAULTMODE		10
typedef int(*ucbf_sensordefaultmode)(CAMDESC* cd, int dattyp, int originX, int numlines, int width, void* profile, void* intensity, void* llt, int numExtData, void* extdata);

SR_API_EXPORT int SR_API_Init(int (*userCB_StatusMessage) (STATUS_MSG_ARGLST));
SR_API_EXPORT int SR_API_GetErrorString(int error_code, char** errstr);
SR_API_EXPORT int SR_API_EnableApiConsoleLog(int apiLogLevel);
SR_API_EXPORT int SR_API_GetApiLogSettings(int* apilogEnable, int* apiLogLevel);
SR_API_EXPORT int SR_API_LogUserString(char* LogString);
SR_API_EXPORT int SR_API_EnableApiFileLog(char* folderPath, int msgLevel);
SR_API_EXPORT int SR_API_SetIpLogSettings(int iplogEnable);
SR_API_EXPORT int SR_API_GetIpLogSettings(int* iplogEnable);
SR_API_EXPORT int SR_API_RegisterUserCB(int CBtype, int command, void *userCB);
SR_API_EXPORT int32_t SR_API_StartCameraConnectionManagement(SRSensor* camdescription);
SR_API_EXPORT int32_t SR_API_StopCameraConnectionManagement(SRSensor* camdescription);
SR_API_EXPORT int32_t SR_API_StartCam(SRSensor* cd);
SR_API_EXPORT int32_t SR_API_StopCam(SRSensor* cd);
SR_API_EXPORT int SR_API_SetLaser(CAMDESC* cd, int pulse_mode, int external_mode, int enable, int power);
SR_API_EXPORT int SR_API_SetLaserParam(CAMDESC* cd, int inst, int pulse_mode, int external_mode, int enable, int power);
SR_API_EXPORT int SR_API_GetLaserParam(CAMDESC* cd, int inst, int *pulse_mode, int *external_mode, int *enable, int *power);
SR_API_EXPORT int SR_API_SetLaserLineThresholdMode(CAMDESC* cd, int inst, int mode);
SR_API_EXPORT int SR_API_SetLaserLineThreshold(CAMDESC* cd, int inst, int threshold, int exposureIdx);
SR_API_EXPORT int SR_API_GetLaserLineThreshold(CAMDESC* cd, int inst, int *threshold, int exposureIdx);
SR_API_EXPORT int SR_API_SetOutput(CAMDESC* cd, int channel, int val);
SR_API_EXPORT int SR_API_GetInput(CAMDESC* cd, int channel, int* val);
SR_API_EXPORT int SR_API_LoadLutFromFile(CAMDESC *cd, char *DateiPfad, char *info);
SR_API_EXPORT int SR_API_SaveLutToFile(CAMDESC *cd, char *DateiPfad, char *info);
SR_API_EXPORT int SR_API_ReadLutInfoFile(CAMDESC *cd, char *DateiPfad, char *infoLut);
SR_API_EXPORT int SR_API_ReadCalibFileFromSensor(CAMDESC* cd, void *user_data);
SR_API_EXPORT int SR_API_IsCalibrationFilePresentOnSensor(const CAMDESC* cd, int* isPresent);
SR_API_EXPORT int SR_API_CreateWorldData(CAMDESC *cd, int startX, int width, unsigned short *profil, SR_3DPOINT *world);
SR_API_EXPORT int SR_API_CreateWorldData_Img(CAMDESC *cd, int startX, int width, int height, unsigned short *profil, SR_3DPOINT *world);
SR_API_EXPORT int SR_API_FreeCreatedWorldData(SR_3DPOINT *world);
SR_API_EXPORT int SR_API_RemapWorldToMatrix(SR_3DPOINT *srW, unsigned short *intens, int len, float range_start, float range_end, unsigned short *iS, unsigned short *zS, float offsetZ, float scaleZ);
SR_API_EXPORT int SR_API_Get_RemapWorldToMatrixDimension(CAMDESC *cd, int startx, int len, float lat_resolution, float z_resolution, unsigned int *target_width, float *field_of_view_limits, float *lateral_start_pos, float *lateral_end_pos);
SR_API_EXPORT int SR_API_RemapWorldToMatrix_ext(CAMDESC *cd, SR_3DPOINT *srW, unsigned short *intens, unsigned short *llt, int startx, unsigned int len, unsigned int number_of_lines, float lat_resolution, float z_resolution, unsigned short *iS, unsigned short *zS, unsigned short *lS, unsigned int *target_width);
SR_API_EXPORT int SR_API_FreeRemappedData(unsigned short *iS, unsigned short *zS, unsigned short *lS);
SR_API_EXPORT int SR_API_ReadCamParsFromFile(char* filename);
SR_API_EXPORT int SR_API_SendParsToCam(CAMDESC* cd);
SR_API_EXPORT int SR_API_SetImager(CAMDESC* cd, int inst, int startx, int starty, int width, int height, int gainEnable, int gain);
SR_API_EXPORT int SR_API_GetImager(CAMDESC* cd, int inst, int *startx, int *starty, int *width, int *height, int *gainEnable, int *gain);
SR_API_EXPORT int SR_API_SetExposure(CAMDESC* cd, int inst, int enableDoubleExpo, int expo_small, int expo_large);
SR_API_EXPORT int SR_API_GetExposure(CAMDESC* cd, int inst, int *enableDoubleExpo, int *expo_small, int *expo_large);
SR_API_EXPORT int SR_API_SetTrigger(CAMDESC* cd, int inst, int mode, int source, int edge, int outputselect, int trigfrq, int trigcnt, int trigoffset);
SR_API_EXPORT int SR_API_GetTrigger(CAMDESC* cd, int inst, int *mode, int *source, int *edge, int *outputselect, int *trigfrq, int *trigcnt, int *trigoffset);
SR_API_EXPORT int SR_API_SetReadInput(CAMDESC* cd, int inst, int *active, int *condition);
SR_API_EXPORT int SR_API_GetReadInput(CAMDESC* cd, int inst, int *active, int *condition);
SR_API_EXPORT int SR_API_SetModule(CAMDESC* cd, int rowNum, char *nextName);
SR_API_EXPORT int SR_API_GetModule(CAMDESC* cd, int rowNum, char *name, char *methodName, int  *methodCode, char *nextName, int *inst, int *numModule);
SR_API_EXPORT int SR_API_SetParamProfile(CAMDESC* cd, int inst, int packetSize, int numProfiles);
SR_API_EXPORT int SR_API_GetParamProfile(CAMDESC* cd, int inst, int *packetSize, int *numProfiles);
SR_API_EXPORT int SR_API_SetAcquireParam(CAMDESC* cd, int inst, int captureType, int mode, unsigned short num_of_profiles);
SR_API_EXPORT int SR_API_GetAcquireParam(CAMDESC* cd, int inst, int *captureType, int *mode, unsigned short *num_of_profiles);
SR_API_EXPORT int SR_API_SetImageTypes(CAMDESC* cd, int inst, int imageType);
SR_API_EXPORT int SR_API_Update_Cam(CAMDESC* cd, char* filename);
SR_API_EXPORT int SR_API_ChangeIp(CAMDESC* cd, unsigned char* newipadr, unsigned short newport);
SR_API_EXPORT int SR_API_GetMACAdr(CAMDESC* cd, unsigned char* mac);
SR_API_EXPORT int SR_API_GetCamVersions(CAMDESC* cd, char* version_sw, char* version_hw);
SR_API_EXPORT int SR_API_GetSensorType(CAMDESC* cd, char* partNumber);
SR_API_EXPORT int SR_API_GetSerial(CAMDESC* cd, unsigned char* serial);
SR_API_EXPORT int SR_API_GetSensorName(CAMDESC* cd, char* productName);
SR_API_EXPORT int SR_API_GetImagerSize(CAMDESC* cd, int* width, int* height);
SR_API_EXPORT int SR_API_GetSensorCenterPosition(CAMDESC* cd, int* centerX, int* centerY);
SR_API_EXPORT int SR_API_GetImagerGranularity(CAMDESC* cd, int* stepSizeX, int* stepSizeY);
SR_API_EXPORT int32_t SR_API_GetSensorSize(CAMDESC* cd, int32_t* width, int32_t* height);
SR_API_EXPORT int SR_API_SetHWStartTrigger(CAMDESC* cd, int inst, int mode, int source, int edge);
SR_API_EXPORT int SR_API_GetHWStartTrigger(CAMDESC* cd, int inst, int *mode, int *souce, int *edge);
SR_API_EXPORT int SR_API_SmoothenProfileImg(unsigned short* profiledata, unsigned int width, unsigned int height, SmoothingPresets preset);
SR_API_EXPORT int SR_API_WorldToImage(CAMDESC *cd, SR_3DPOINT const *world, int *imgX, int *imgY);
SR_API_EXPORT int32_t SR_API_SetSmartXccelerate(SRSensor *sensor, bool8_t smartXccelerate);
SR_API_EXPORT int32_t SR_API_GetSmartXccelerate(SRSensor *sensor, bool8_t *smartXccelerate);
SR_API_EXPORT int32_t SR_API_SetSmartXCellerate(SRSensor *sensor, bool8_t smartXccelerate);
SR_API_EXPORT int32_t SR_API_GetSmartXCellerate(SRSensor *sensor, bool8_t *smartXccelerate);
SR_API_EXPORT const char* SR_API_GetCurrentFileLoggingFilename();

#endif // SR_API_DEPRECATEDh
