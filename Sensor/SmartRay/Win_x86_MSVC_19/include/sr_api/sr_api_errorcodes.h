/******************************************************************************
 *                   	   SmartRay API Error Codes                           *
 *----------------------------------------------------------------------------*
 * Copyright (c) SmartRay GmbH 2018.  All Rights Reserved.		              *
 *----------------------------------------------------------------------------*
 * Title    SR_API_errorcodes.h                                               *
 * Version	SR_API_VERSION                                                    *
 ******************************************************************************/

/**
 * @file sr_api_errorcodes.h
 * @author SmartRay GmbH
 * @date 2018
 * @brief List of SmartRay error codes
 * @see http://www.smartray.com
 */

#ifndef SR_API_ERRORCODES_H
#define SR_API_ERRORCODES_H

#define SUCCESS								                            0           /*!< Successful. */

#define ERR_SR_API_SRSENSOR_NULLPOINTER									-1			/*!< Sensor object is nullpointer */
#define ERR_SR_API_ARG_NULLPOINTER										-2			/*!< Input parameter is nullpointer */
#define ERR_SR_API_MEMORY_ALLOCATION_FAILED								-3			/*!< Memory allocation failed */
#define ERR_SR_API_FILE_READ_FAILED										-4			/*!< Cannot read file */
#define ERR_SR_API_FUNCTION_NOT_SUCCESSFUL								-5			/*!< Function not successful */
#define ERR_SR_API_FILE_WRITE_FAILED									-6			/*!< Cannot write file */
#define ERR_SR_API_INVALID_FILENAME_OR_PATH								-8			/*!< Invalid filename or path */
#define ERR_SR_API_INVALID_FILETYPE										-9			/*!< Invalid file type */
#define ERR_SR_API_FIRMWARE_NOT_COMPATIBLE_WITH_SENSOR_SERIES			-10			/*!< Loaded firmware file not compatible with sensor */
#define ERR_SR_API_FUNCTION_NOT_AVAILABLE								-11			/*!< Function not available */
#define ERR_SR_API_MISSING_LICENSE										-12			/*!< Missing License*/
#define ERR_SR_API_ALREADY_INITALIZED									-1000		/*!< API already initialized*/
#define ERR_SR_API_SENSOR_NUMBER_OUT_OF_RANGE							-1002		/*!< Maximum number of sensors reached*/
#define ERR_SR_API_SENSOR_ACTIVE										-1003		/*!< Sensor busy */
#define ERR_SR_API_SENSOR_NOT_ACTIVE									-1004		/*!< Sensor not active */
#define ERR_SR_API_SENSOR_NOT_DISCONNECTED								-1005		/*!< Sensor could not be disconnected*/
#define ERR_SR_API_SENSOR_NOT_CONNECTED									-1006		/*!< Sensor not connected */
#define	ERR_SR_API_NR_USERCB_MAX										-1009		/*!< Maximum callback function reached for this callback type*/
#define	ERR_SR_API_NOT_VALID_CB_TYPE									-1012		/*!< Invalid callback type */
#define	ERR_SR_API_NO_MPAR_AVAILABLE									-1013		/*!< Machine parameter not available */
#define	ERR_SR_API_NO_SYSPAR_AVAILABLE									-1014		/*!< System parameter not available */
#define	ERR_SR_API_IP_OR_PORT_IS_ZERO									-1015		/*!< IP address or port number is zero*/
#define ERR_SR_API_IP_IS_INVALID										-1016		/*!< New IP address invalid (Ex: 255.255.255.255)*/
#define	ERR_SR_API_WIDTH_IS_ZERO										-1017		/*!< ROI-width is zero */
#define	ERR_SR_API_HEIGHT_IS_ZERO										-1018		/*!< ROI-height is zero */
#define	ERR_SR_API_ORIGIN_X_RANGE_ERROR									-1019		/*!< ROI-OriginX is not between 0 and 1920 */
#define	ERR_SR_API_ORIGIN_Y_RANGE_ERROR									-1020		/*!< ROI-OriginY is not between 0 and 1200 */
#define ERR_SR_API_NEGATIVE_SENSOR_INDEX_NOT_ALLOWED					-1021		/*!< Sensor ID parameter is negative */
#define ERR_SR_API_SENSOR_INDEX_GREATER_THAN_MAX_SENSOR_SUPPORTED		-1023		/*!< Sensor ID parameter is greater than the maximum sensors supported by API*/
#define	ERR_SR_API_VALUES_OUT_OF_RANGE									-1026		/*!< Values not in range */
#define	ERR_SR_API_CALIBRATION_FILE_NOT_COMPATIBLE_WITH_SENSOR_SERIES	-1033		/*!< Calibration file  not compatible with sensor */
#define ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED						-1046		/*!< Parameter set not found */
#define ERR_SR_API_CALIBRATION_FILE_NOT_PRESENT_ON_SENSOR				-1053		/*!< Calibration file not stored on sensor */

#define ERR_SR_API_ALG_GENERAL                                          -11000      /*!< ALG: General error */
#define ERR_SR_API_ALG_FILE_INPUT                                       -11001		/*!< ALG: Reading file failed */
#define ERR_SR_API_ALG_FILE_OUTPUT                                      -11002		/*!< ALG: Writing file failed */
#define ERR_SR_API_ALG_ASSERTION                                        -11003		/*!< ALG: An assertion failed */
#define ERR_SR_API_ALG_DATA_TYPE_NOT_SUPPORTED                          -11004      /*!< ALG: Data type not supported */
#define ERR_SR_API_ALG_INVALID_PARAMETER_VALUE                          -11005      /*!< ALG: Parameter value is invalid */
#define ERR_SR_API_ALG_DIVISION_BY_ZERO                                 -11006      /*!< ALG: Division by zero */

#define ERR_SR_API_MSR_GENERAL                                          -12000		/*!< MSR: General error */
#define ERR_SR_API_MSR_XML_INVALID_FORMAT                               -12001		/*!< MSR: Invalid format of XML file */
#define ERR_SR_API_MSR_INVALID_RESOLUTION                               -12002		/*!< MSR: Invalid resolution */
#define ERR_SR_API_MSR_INVALID_NUMBER_OF_SENSORS                        -12003		/*!< MSR: Invalid number of sensors */
#define ERR_SR_API_MSR_INVALID_SENSOR_ID                                -12004		/*!< MSR: Invalid sensor ID */
#define ERR_SR_API_MSR_INVALID_ARTIFACT                                 -12005      /*!< MSR: Artifact is invalid for this setup */
#define ERR_SR_API_MSR_POINT_CLOUD_DATA_INVALID                         -12006		/*!< MSR: Point cloud data not valid */
#define ERR_SR_API_MSR_NOT_ENOUGH_SPHERES_DETECTED                      -12007		/*!< MSR: Not enough spheres detected for computing registration */
#define ERR_SR_API_MSR_TOO_MANY_SPHERES_DETECTED                        -12008		/*!< MSR: Too many spheres detected for computing registration */
#define ERR_SR_API_MSR_INVALID_SPHERE_FIT                               -12009		/*!< MSR: Invalid fitting result */
#define ERR_SR_API_MSR_NO_PLANE_DETECTED                                -12010		/*!< MSR: No plane detected */
#define ERR_SR_API_MSR_REGISTRATION_DATA_INVALID                        -12011		/*!< MSR: Registration data not valid */
#define ERR_SR_API_MSR_INVALID_TRANSPORT_RESOLUTION                     -12012		/*!< MSR: Transport resolution is not valid */
#define ERR_SR_API_MSR_PART_NUMBER_INVALID								-12013		/*!< MSR: Sensor part number invalid */
#define ERR_SR_API_MSR_TRANSPORT_RESOLUTION_INVALID						-12014		/*!< MSR: Sensor transport resolution invalid */
#define ERR_SR_API_MSR_ROI_WIDTH_INVALID								-12015		/*!< MSR: Sensor ROI width invalid */
#define ERR_SR_API_MSR_NUMBER_OF_PROFILES_INVALID						-12016		/*!< MSR: Sensor number of profiles invalid */
#define ERR_SR_API_MSR_POINTCLOUD_SIZE_INVALID							-12017		/*!< MSR: Sensor point cloud size invalid */
#define ERR_SR_API_MSR_NOT_ENABLED										-12018		/*!< MSR: MSR mode is not enabled */
#define ERR_SR_API_MSR_SENSOR_FOV_TOO_SMALL                             -12019      /*!< MSR: FOV of sensor is too small for artifact */
#define ERR_SR_API_MSR_SENSOR_FOV_TOO_LARGE                             -12020      /*!< MSR: FOV of sensor is too large for artifact */
#define ERR_SR_API_MSR_INTENSITY_DATA_INVALID                           -12021      /*!< MSR: Intensity data not valid */
#define ERR_SR_API_MSR_LASERLINETHICKNESS_DATA_INVALID                  -12022      /*!< MSR: Laser line thickness data not valid */
#define ERR_SR_API_MSR_NOT_MATCHING_PROFILE_NUMBER                      -12023      /*!< MSR: Not matching number of profiles in between sensors */

#define ERR_SR_API_GEO_GENERAL                                          -13000		/*!< GEO: General error */
#define ERR_SR_API_GEO_INSUFFICIENT_DATA                                -13001		/*!< GEO: Insufficient geometry data */

#define ERR_SR_API_IPR_GENERAL                                          -14000      /*!< IPR: General error */
#define ERR_SR_API_IPR_INVALID_IMAGE_HEADER                             -14001      /*!< IPR: Image header is invalid */
#define ERR_SR_API_IPR_INVALID_IMAGE_SIZE                               -14002      /*!< IPR: Image size is invalid */

#define ERR_SR_API_ROI_WIDTH_OUT_OF_RANGE								-20001		/*!< ROI width is not valid. It must be a multiple of the granularity for this sensor and must not exceed the range */
#define ERR_SR_API_ROI_HEIGHT_OUT_OF_RANGE								-20002		/*!< ROI height is not valid. It must be a multiple of the granularity for this sensor and must not exceed the range */
#define ERR_SR_API_ROI_WIDTH_GRANURALITY_INVALID						-20003		/*!< ROI width is not valid. It must be a multiple of the granularity for this sensor and must not exceed the range */
#define ERR_SR_API_ROI_HEIGHT_GRANURALITY_INVALID						-20004		/*!< ROI height is not valid */
#define ERR_SR_API_ROI_ORIGINX_GRANURALITY_INVALID						-20005		/*!< ROI origin is not valid. It must be a multiple of the granularity for this sensor and must not exceed the range */
#define ERR_SR_API_ROI_ORIGINX_OUT_OF_RANGE								-20006		/*!< ROI origin is not valid. It must be a multiple of the granularity for this sensor and must not exceed the range */
#define ERR_SR_API_ROI_ORIGINY_GRANURALITY_INVALID						-20007		/*!< ROI origin is not valid. It must be a multiple of the granularity for this sensor and must not exceed the range */
#define ERR_SR_API_ROI_ORIGINY_OUT_OF_RANGE								-20008		/*!< ROI origin is not valid. It must be a multiple of the granularity for this sensor and must not exceed the range */

#define ERR_SR_API_GAIN_OUT_OF_RANGE									-20010		/*!< Gain out of range */

#define ERR_SR_API_EXPOSURE1_OUT_OF_RANGE								-20020		/*!< Exposure time with ID 0 out of range */
#define ERR_SR_API_EXPOSURE2_OUT_OF_RANGE								-20021		/*!< Exposure time with ID 1 out of range */
#define ERR_SR_API_EXPOSURE2_NOT_ENABLED								-20022		/*!< Exposure 2 not enabled */
#define ERR_SR_API_EXPOSURE_INDEX_NOT_SUPPORTED							-20023		/*!< Exposure ID not supported */
#define ERR_SR_API_EXPOSURE_OUT_OF_RANGE                                -20024      /*!< Exposure time out of range */

#define ERR_SR_API_START_TRIGGER_SOURCE_OUT_OF_RANGE					-20030		/*!< Start trigger source out of range */
#define ERR_SR_API_START_TRIGGER_CONDITION_OUT_OF_RANGE					-20031		/*!< Start trigger condition out of range */
#define ERR_SR_API_START_TRIGGER_ENABLE_OUT_OF_RANGE					-20032		/*!< Start trigger enable out of range. Value must be 1 or 0 */

#define ERR_SR_API_DATA_TRIGGER_MODE_OUT_OF_RANGE						-20040		/*!< Data trigger mode out of range */
#define ERR_SR_API_DATA_TRIGGER_SOURCE_OUT_OF_RANGE						-20041		/*!< Data trigger source out of range */
#define ERR_SR_API_DATA_TRIGGER_EDGE_OUT_OF_RANGE						-20042		/*!< Data trigger edge out of range */
#define ERR_SR_API_DATA_TRIGGER_FREQUENCY_OUT_OF_RANGE					-20043		/*!< Data trigger frequency out of range */
#define ERR_SR_API_DATA_TRIGGER_DIVIDER_OUT_OF_RANGE					-20044		/*!< Data trigger divider out of range */
#define ERR_SR_API_DATA_TRIGGER_DELAY_OUT_OF_RANGE						-20045		/*!< Data trigger delay out of range */
#define ERR_SR_API_DATA_TRIGGER_DIRECTION_OUT_OF_RANGE					-20046		/*!< Data trigger direction out of range */

#define ERR_SR_API_OUTPUT_CHANNEL_OUT_OF_RANGE							-20050		/*!< Output ID out of range */

#define ERR_SR_API_LASER_MODE_OUT_OF_RANGE								-20060		/*!< Laser mode out of range */
#define ERR_SR_API_LASER_BRIGHTNESS_OUT_OF_RANGE						-20061		/*!< Laser brightness out of range */
#define ERR_SR_API_LASER_POWER_OUT_OF_RANGE								-20062		/*!< Laserline power out of range */
#define ERR_SR_API_3D_LASER_LINE_THRESHOLD_OUT_OF_RANGE					-20063		/*!< Laserline threshold out of range */

#define ERR_SR_API_ACQUISITION_DATA_TYPE_INVALID						-20070		/*!< Acquisition data type invalid */
#define ERR_SR_API_ACQUISITION_MODE_INVALID								-20071		/*!< Acquisition mode invalid  */

#define ERR_SR_API_NUMBER_OF_PROFILES_TO_CAPTURE_INVALID				-20080		/*!< Number of profiles to capture invalid */
#define ERR_SR_API_PROFILE_PACKET_SIZE_INVALID							-20081		/*!< Profile packet size invalid */

#define ERR_SR_API_ZMAP_VERTICAL_RESOLUTION_OUT_OF_RANGE				-20090		/*!< Z-map vertical resolution out of range*/
#define ERR_SR_API_ZMAP_LATERAL_RESOLUTION_OUT_OF_RANGE					-20091		/*!< Z-map lateral resolution out of range */

#define ERR_SR_API_IPADDRESS_INVALID									-20100		/*!< IP address invalid */
#define ERR_SR_API_PORTNUMBER_INVALID									-20101		/*!< PortNumber invalid. Default: 40 */

#define ERR_SR_API_REFLECTIONFILTER_ENABLE_OUT_OF_RANGE					-20110		/*!< Reflection filter enable out of range. Value must be 0 or 1 */
#define ERR_SR_API_REFLECTIONFILTER_ALGORITHM_OUT_OF_RANGE				-20111		/*!< Reflection filter region out of range. Value must be 0,1, 2 or 3 */
#define ERR_SR_API_REFLECTIONFILTER_SEARCHAREA_OUT_OF_RANGE				-20112		/*!< Reflection filter search area is out of range. Value must be within the maximum ROI dimension */
#define ERR_SR_API_REFLECTIONFILTER_SEARCHAREA_INVALID					-20113		/*!< Reflection filter search area invalid. */

#define ERR_SR_API_ZMAP_WIDTH_INVALID									-20130		/*!< Z-map width invalid (<0) */
#define ERR_SR_API_ZMAP_HEIGHT_INVALID									-20131		/*!< Z-map height invalid (<0) */

#define ERR_SR_API_CALIBRATION_FILE_NOT_LOADED							-20150		/*!< Calibration file not loaded. This function requires that the calibration file is stored on the sensor*/

#define ERR_SR_API_SMOOTHEN_PRESET_INVALID								-20160		/*!< Smoothing preset invalid*/

#define ERR_SR_API_CONNECTION_TIMEOUT									-20170		/*!< Cannot connect to sensor, connection timeout reached */
#define ERR_SR_API_CONNECTION_TIMEOUT_INVALID							-20171		/*!< Connection timeout invalid*/

#define ERR_SR_API_PART_NUMBER_INVALID                                  -20172      /*!< Sensor part number invalid */

#define ERR_SR_API_SENSOR_CONFIG_P_AND_HIGHACCURACCY                    -20173      /*!< Sensor Configuration P-acquisition-mode and multi-exposure in high-accuraccy */

#define ERR_SR_API_SENSOR_ALREADY_CONNECTED							    -20174		/*!< Sensor already connected*/

#define ERR_LOG_FAILED_TO_CREATE_CONSOLESINK 							-500100     /*!< Failed to create console sink */
#define ERR_LOG_FAILED_TO_CREATE_FILESINK    							-500101     /*!< Failed to create file sink */
#define ERR_LOG_FAILED_TO_ADD_SINK           							-500102     /*!< Failed to add sink */

/*============================================================================*/
// DEPRECATED ERROR CODES (backward compatibility) | PLEASE DO NOT USE
/*============================================================================*/
#define	FOUND_STRING						1			/*!< Deprecated */
#define	FOUND_INTEGER						2			/*!< Deprecated */
#define	Z_OVERFLOW							3			/*!< Deprecated */
#define	FOUND_FLOAT							4			/*!< Deprecated */
#define	DIFFERENT							5			/*!< Deprecated */
#define EQUAL								6			/*!< Deprecated */
#define	CONNECTION_ESTABLISHED				7			/*!< Deprecated */
#define	FUNCTION_SUCCESSFUL					8			/*!< Deprecated */

#define ERR_CD_NULLPOINTER					-1			/*!< Deprecated */
#define ERR_NULLPOINTER						-2			/*!< Deprecated */
#define	ERR_ALLOCATION_FAILED				-3			/*!< Deprecated */
#define	ERR_READ_FILE_FAILED				-4			/*!< Deprecated */
#define	ERR_FUNCTION_NOT_SUCCESSFUL			-5			/*!< Deprecated */
#define	ERR_WRITE_FILE_FAILED				-6			/*!< Deprecated */
#define	ERR_FILE_NEVER_FOUND				-7			/*!< Deprecated */
#define	ERR_INVALID_FILENAME				-8			/*!< Deprecated */
#define	ERR_INVALID_FILETYPE				-9			/*!< Deprecated */
#define	ERR_INVALID_UPDATEFILE				-10			/*!< Deprecated */
#define	ERR_FUNCTION_NOT_AVAILABLE			-11			/*!< Deprecated */

#define ERR_API_IS_INITIALIZED				-1000		/*!< Deprecated */
#define ERR_LESS_NR_OF_CAM_INSTANCES		-1001		/*!< Deprecated */
#define ERR_TOO_MANY_NR_OF_CAM_INSTANCES	-1002		/*!< Deprecated */
#define ERR_CAM_STILL_ACTIVE				-1003		/*!< Deprecated */
#define	ERR_CAM_NOT_ACTIVE					-1004		/*!< Deprecated */
#define	ERR_CAM_CONN_STILL_OPEN				-1005		/*!< Deprecated */
#define	ERR_CAM_NOT_CONNECTED				-1006		/*!< Deprecated */
#define	ERR_SIZE_MISSMATCH					-1007		/*!< Deprecated */
#define	ERR_NR_USERCB_RAWDATA_MAX			-1008		/*!< Deprecated */
#define	ERR_NR_USERCB_CAMDATA_MAX			-1009		/*!< Deprecated */
#define	ERR_NR_USERCB_CAMDATAEXT_MAX		-1010		/*!< Deprecated */
#define	ERR_NR_USERCB_PIXELMODE_MAX			-1011		/*!< Deprecated */
#define	ERR_NO_VALID_CB_TYPE				-1012		/*!< Deprecated */
#define	ERR_NO_MPAR_AVAILABLE				-1013		/*!< Deprecated */
#define	ERR_NO_SYSPAR_AVAILABLE				-1014		/*!< Deprecated */
#define	ERR_IP_OR_PORT_IS_ZERO				-1015		/*!< Deprecated */
#define ERR_IP_IS_BROADCAST					-1016		/*!< Deprecated */
#define	ERR_WIDTH_IS_ZERO					-1017		/*!< Deprecated */
#define	ERR_HEIGHT_IS_ZERO					-1018		/*!< Deprecated */
#define	ERR_CENTERX_RANGE_ERROR				-1019		/*!< Deprecated */
#define	ERR_CENTERY_RANGE_ERROR				-1020		/*!< Deprecated */
#define	ERR_INDEX_LESS_THAN_ZERO			-1021		/*!< Deprecated */
#define	ERR_NO_TRACE_FOUND					-1022		/*!< Deprecated */
#define	ERR_INDEX_GREATER_THAN_MAX			-1023		/*!< Deprecated */
#define	ERR_CAM_NOT_RUNNING					-1024		/*!< Deprecated */
#define	ERR_WRONG_DATTYPE					-1025		/*!< Deprecated */
#define	ERR_OUT_OF_RANGE					-1026		/*!< Deprecated */
#define	ERR_COORDX_IS_ZERO					-1027		/*!< Deprecated */
#define	ERR_ANGLE_OUT_OF_RANGE				-1028		/*!< Deprecated */
#define	ERR_NO_COORD_GREAT_ENOUGH			-1029		/*!< Deprecated */
#define	ERR_LUTWIDTH_RANGE_ERROR			-1030		/*!< Deprecated */
#define	ERR_LUTHEIGHT_RANGE_ERROR			-1031		/*!< Deprecated */
#define	ERR_SEARCH_ITEM_NOT_FOUND			-1032		/*!< Deprecated */
#define	ERR_LUTWIDTH_IS_ZERO				-1033		/*!< Deprecated */
#define	ERR_LUTHEIGHT_IS_ZERO				-1034		/*!< Deprecated */
#define	ERR_NO_IMAGE_POINT_FOUND			-1035		/*!< Deprecated */
#define	ERR_RANGE_CHECK						-1036		/*!< Deprecated */
#define	ERR_CR_LUT_NOT_VALID				-1037		/*!< Deprecated */
#define	ERR_CR_LUT_VERSION					-1038		/*!< Deprecated */
#define	ERR_STEP_LESS_OR_EQUAL_ZERO			-1039		/*!< Deprecated */
#define	ERR_STEP_TOO_BIG					-1040		/*!< Deprecated */
#define ERR_MAX_MODULE_REACHED_MODE0		-1041		/*!< Deprecated */
#define ERR_MAX_MODULE_REACHED_MODE1		-1042		/*!< Deprecated */
#define	ERR_GAIN_RANGE						-1043		/*!< Deprecated */
#define	ERR_VAL_INVALID						-1044		/*!< Deprecated */
#define	ERR_CONNECTION_NOT_ESTABLISHED		-1045		/*!< Deprecated */
#define	ERR_PARAMETERFILE_NOT_FOUND			-1046		/*!< Deprecated */
#define	ERR_NULL_ARG						-1047		/*!< Deprecated */
#define	ERR_PARSET_INVALID					-1048		/*!< Deprecated */
#define ERR_ROW_RANGE						-1049		/*!< Deprecated */
#define ERR_STRLEN_RANGE					-1050		/*!< Deprecated */
#define ERR_NOACK_FROM_SENSOR				-1051		/*!< Deprecated */
#define ERR_FEATUREID_NOTEDEFINED			-1052		/*!< Deprecated */
#define ERR_NO_LUT_PRESENT_ON_SENSOR		-1053		/*!< Deprecated */

#define ERR_LUT_INVALID_HEADER				-2000		/*!< Deprecated */
#define ERR_LUT_HDU_NOT_FOUND				-2001		/*!< Deprecated */
#define ERR_LUT_EMPTY						-2002		/*!< Deprecated */
#define ERR_LUT_INVALID						-2003		/*!< Deprecated */

#define ERR_PARAM_NOT_AVAILABLE				-3001		/*!< Deprecated */

#define ERR_MODULE_ORDER					-10000		/*!< Deprecated */
#define	ERR_MODULE_GET_NEXT_LINE			-10001		/*!< Deprecated */
#define	ERR_MODULE_TRACKING					-10002		/*!< Deprecated */
#define	ERR_MODULE_CORRELATOR				-10003		/*!< Deprecated */
#define	ERR_MODULE_PROFILE					-10004		/*!< Deprecated */
#define	ERR_MODULE_CALCULATOR				-10005		/*!< Deprecated */
#define	ERR_MODULE_COMPARE					-10006		/*!< Deprecated */
#define	ERR_MODULE_GET_VAL					-10007		/*!< Deprecated */
#define	ERR_MODULE_LOGIC					-10008		/*!< Deprecated */
#define	ERR_MODULE_READ_INPUT				-10009		/*!< Deprecated */
#define	ERR_MODULE_TCP						-10010		/*!< Deprecated */
#define	ERR_MODULE_CMOS_ROI					-10011		/*!< Deprecated */
#define	ERR_MODULE_CMOS_STARTX				-10012		/*!< Deprecated */
#define	ERR_MODULE_CMOS_SIZE				-10013		/*!< Deprecated */
#define	ERR_MODULE_CMOS_GAIN				-10014		/*!< Deprecated */

#define ERR_HEIGHT_IS_NOT_SUPPORTED			-10020		/*!< Deprecated */
#define ERR_STARTX_LESS_THAN_ZERO			-10021		/*!< Deprecated */
#define ERR_WIDTH_IS_NOT_SUPPORTED			-10022		/*!< Deprecated */
#define ERR_CMOS_HEIGHT						-10023		/*!< Deprecated */
#define ERR_STARTY_LESS_THAN_ZERO			-10024		/*!< Deprecated */

#endif
