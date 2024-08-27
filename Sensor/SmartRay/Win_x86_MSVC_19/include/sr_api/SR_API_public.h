/******************************************************************************
 *                               SmartRay API                                 *
 *----------------------------------------------------------------------------*
 * Copyright (c) SmartRay GmbH 2018.  All Rights Reserved.		              *
 *----------------------------------------------------------------------------*
 * Title    SR_API_public.h                                                   *
 * Version	SR_API_VERSION                                                    *
 ******************************************************************************/

/**
 * @file SR_API_public.h
 * @author SmartRay GmbH
 * @date 2018
 * @brief SmartRay API functions
 * @see http://www.smartray.com
 */

#ifndef SR_API_PUBLICh
#define SR_API_PUBLICh

#include <stdio.h>
#include <stdint.h>

#include "SR_API_Defines.h"
#include "SR_API_Types.h"

#ifdef __cplusplus
extern "C" {
#endif


/** @defgroup INIT-EXIT Initialization & Exit 
 *  Functions to initialize and exit SmartRay API
 *  @{
 */

/**
 *	@brief	Initialize SmartRay API
  * @param 	callback - for Status and Error messages
  *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_Initalize(Callback_StatusMessage callback);

/**
 *	@brief	De-initialize and free memory resources used by SmartRay API
 *  @return #SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_Exit(void);

/**
 *	@brief Returns API version as NULL terminated string
 *  @param version - The API version
 *	@return #SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetAPIVersion(char ** version);

/**
 * @brief Get error message from error code.
 *        Content of errMsg must not be changed!
 *        Due to backwards compatibility, interface of function is not changed,
 *        but treat it as >> char const **errMsg <<.
 * @param 	errorCode - error code for corresponding error message
 * @param 	errMsg - string to return the error message (pass a valid character pointer address)
 * @return	#ERR_SR_API_ARG_NULLPOINTER - Invalid character pointer address
 * @return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetErrorMsg(int32_t errorCode, char **errMsg);

/**
 *	@brief	Registers callback for Live Image
 * 	@param	callback - callback function pointer
 *	@return #ERR_SR_API_NR_USERCB_MAX
 *	@return #ERR_SR_API_NOT_VALID_CB_TYPE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_RegisterLiveImageCB(Callback_LiveImage callback);

/**
*	@brief	Registers callback for PIL image (PIL: Profile, Intensity, LaserLineThickness)
* 	@param	callback - User callback function pointer
*	@return #ERR_SR_API_NR_USERCB_MAX
*	@return #ERR_SR_API_NOT_VALID_CB_TYPE
*	@return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_RegisterPilImageCB(Callback_PilImage callback);

/**
*	@brief	Registers callback for Point Cloud
* 	@param	callback - User callback function pointer
*	@return #ERR_SR_API_NR_USERCB_MAX
*	@return #ERR_SR_API_NOT_VALID_CB_TYPE
*	@return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_RegisterPointCloudCB(Callback_PointCloud callback);

/**
*	@brief	Registers callback for ZIL image (ZIL: Z-map, Intensity, LaserLineThickness)
* 	@param	callback - User callback function pointer
*	@return #ERR_SR_API_NR_USERCB_MAX
*	@return #ERR_SR_API_NOT_VALID_CB_TYPE
*	@return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_RegisterZilImageCB(Callback_ZilImage callback);

/**
*	@brief	Registers callback for Point cloud (MSR)
* 	@param	callback - User callback function pointer
*	@return #ERR_SR_API_NR_USERCB_MAX
*	@return #ERR_SR_API_NOT_VALID_CB_TYPE
*	@return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_RegisterMSRPointCloudCB(Callback_MSRPointCloud callback);

/** @} */ // End of INIT-EXIT


/** @defgroup CONNECTION Connection
 *  Functions to manage connection with the sensor
 *  @{
 */

/**
 *	@brief	Connect sensor to host computer (API). \n
 *			The function returns if the connections was successful, or if the connection timeout elapsed
 *	@param	sensor              - Sensor object
 *	@param	connectionTimeoutS  - Number of seconds the API should try to connect to the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_SENSOR_NUMBER_OUT_OF_RANGE
 *	@return	#ERR_SR_API_MEMORY_ALLOCATION_FAILED
 *	@return	#ERR_SR_API_SENSOR_INDEX_GREATER_THAN_MAX_SENSOR_SUPPORTED
 *	@return	#ERR_SR_API_NEGATIVE_SENSOR_INDEX_NOT_ALLOWED
 *	@return #ERR_SR_API_CONNECTION_TIMEOUT
 *  @return	#ERR_SR_API_CONNECTION_TIMEOUT_INVALID
 *  @return	#ERR_SR_API_SENSOR_ALREADY_CONNECTED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_ConnectSensor(SRSensor* sensor, int32_t connectionTimeoutS);

/**
 *	@brief	Disconnect sensor from host computer (API)
 *	@param	sensor - Sensor object
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_SENSOR_NOT_DISCONNECTED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_DisconnectSensor(SRSensor* sensor);

/** @} */ // end of CONNECTION

/** @defgroup ROI Region of Interest (ROI)
 *  List of functions to configure sensor's Region of Interest (ROI) configuration
 *  @{
 */

/**
 *	@brief	Set Region of interest (ROI) of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	originX		-	Start position in X-direction, in pixels
 *	@param	width		-	ROI Width, in pixels
 *	@param	originY		-	Start position in Y-direction, in pixels
 *	@param	height		-	ROI Height, in pixels
 *  @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ROI_WIDTH_OUT_OF_RANGE
 *	@return	#ERR_SR_API_ROI_HEIGHT_OUT_OF_RANGE
 *	@return	#ERR_SR_API_ROI_ORIGINX_OUT_OF_RANGE
 *	@return	#ERR_SR_API_ROI_WIDTH_GRANURALITY_INVALID
 *	@return	#ERR_SR_API_ROI_HEIGHT_GRANURALITY_INVALID
 *	@return	#ERR_SR_API_ROI_ORIGINX_GRANURALITY_INVALID
 *	@return	#ERR_SR_API_ROI_ORIGINY_GRANURALITY_INVALID
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetROI(SRSensor *sensor, int32_t originX, int32_t width, int32_t originY, int32_t height);

/**
 *	@brief	Get Region of interest (ROI) of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*originX	-	Start position in X-direction, in pixels
 *	@param	*width		-	ROI Width, in pixels
 *	@param	*originY	-	Start position in Y-direction, in pixels
 *	@param	*height		-	ROI Height, in pixels
 *  @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetROI(SRSensor *sensor, int32_t *originX, int32_t *width, int32_t *originY, int32_t *height);

/**
* @brief  Get the granularity (a.k.a. step size) of the sensor in ROI-X and ROI-Y direction, i.e. the multiple in which ROI-X and ROI-Y can be configured
* @param  sensor - Sensor object
* @param  *roiGranularityX - Granularity in ROI-X direction
* @param  *roiGranularityY - Granularity in ROI-Y direction
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_GetSensorGranularity (SRSensor *sensor, int32_t *roiGranularityX, int32_t *roiGranularityY);

/**
 *	@brief	Get maximum image ROI-Width and ROI-Height (in pixels) of the sensor
 *	@param	sensor				-	Sensor object
 *	@param	*width				- 	Maximum ROI-Width of the sensor (in pixels)
 *	@param	*height				-	Maximum ROI-Height of the sensor (in pixels)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_SENSOR_NUMBER_OUT_OF_RANGE
 *	@return	#ERR_SR_API_WIDTH_IS_ZERO
 *	@return	#ERR_SR_API_HEIGHT_IS_ZERO
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorMaxDimensions (SRSensor *sensor, int32_t *width, int32_t *height);

/**
 *	@brief	Get sensor origin (X, Y) of the sensor
 *	@param	sensor				-	Sensor object
 *	@param	*originX			- 	originX of the sensor
 *	@param	*originY			- 	originY of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_SENSOR_NUMBER_OUT_OF_RANGE
 *	@return	#ERR_SR_API_ORIGIN_X_RANGE_ERROR
 *	@return	#ERR_SR_API_ORIGIN_Y_RANGE_ERROR
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorOrigin (SRSensor *sensor, int32_t *originX, int32_t *originY);

/** @} */ // end of ROI

/** @defgroup SMARTX SmartX
*  List of API functions for SmartX features, like SmartXact, SmartXpress and SmartXtract.
*  @{
*/

/** @defgroup SMARTXPRESS SmartXpress
*  List of API functions for SmartXpress feature.
*  @{
*/

/**
*	@brief	Enable or disable SmartXpress of the sensor \n
*          \b Note: Supported for ECCO 95 series only
*	@param	*sensor		-	Sensor object
*	@param	enable		-	Enable (true) or disable (false) SmartXpress
*	@return	#ERR_SR_API_SRSENSOR_NULLPOINTER
*	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
*	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
*	@return	#ERR_SR_API_SENSOR_NOT_CONNECTED
*	@return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_SetSmartXpress(SRSensor *sensor, bool8_t enable);

/**
*	@brief	Get SmartXpress status of the sensor \n
*          \b Note: Supported for ECCO 95 series only
*	@param	*sensor		-	Sensor object
*	@param	*enable		-	Enabled (true) or disabled (false) SmartXpress
*	@return	#ERR_SR_API_SRSENSOR_NULLPOINTER
*	@return	#ERR_SR_API_ARG_NULLPOINTER
*	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
*	@return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_GetSmartXpress(SRSensor *sensor, bool8_t *enable);

/**
*	@brief	Set SmartXpress configuration of the sensor with a path to sxp-file \n
*          \b Note: Supported for ECCO 95 series only
*	@param	*sensor		-	Sensor object
*	@param	*configurationFilePath		-	File path to the SmartXpress Configuration file (sxp)
*	@return	#ERR_SR_API_SRSENSOR_NULLPOINTER
*	@return	#ERR_SR_API_ARG_NULLPOINTER
*	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
*	@return	#ERR_SR_API_FILE_READ_FAILED
*	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
*	@return	#ERR_SR_API_SENSOR_NOT_CONNECTED
*	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
*	@return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_SetSmartXpressConfiguration(SRSensor *sensor, const char * configurationFilePath);

/**
* @brief  Get SmartXpress configuration of the sensor as sxp-filename \n
*         \b Note: Supported for ECCO 95 series only
* @param  *sensor - Sensor object
* @param  *configurationFilePath - File path to the SmartXpress configuration file (sxp)
* @param  bufferLength - Buffer length of the configurationFilePath 
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_SENSOR_NOT_CONNECTED
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_GetSmartXpressConfiguration(SRSensor *sensor, char * configurationFilePath, uint32_t bufferLength);

/** @} */ // end of SMARTXPRESS

/** @defgroup SMARTXTRACT SmartXtract
*  List of API functions for SmartXtract feature.
*  @{
*/

/**
* @brief  Dis-/Enable SmartXtract for given @sensor\n
*         \b Note: Supported for ECCO 95 series only
* @pre    Preset needs to be loaded first.
* @param  *sensor - Sensor object
* @param  *enabled - flag to dis-/enable SmartXtract
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_SENSOR_NOT_CONNECTED
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_EnableSmartXtract(SRSensor *sensor, bool8_t enable);

/**
* @brief  Checkif SmartXtract is enabled for given @sensor and store
*         result in @enabled.\n
*         \b Note: Supported for ECCO 95 series only
* @param  *sensor - Sensor object
* @param  *enabled - flag
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_IsEnabledSmartXtract(SRSensor *sensor, bool8_t *enabled);

/**
* @brief  In addition to the standard/default SmartXtract presets available as part of SDK 5 installation,
*         it is possible to create custom SmartXtract presets with support from SmartRay Applications Engineering,
*         which can be done case-by-case upon demand. The custom SmartXtract preset could further help improve data
*         quality, depending on the application. To get support with custom SmartXtract preset, a 'SmartXtract Archive'
*         file will be needed. This file (*.dat) contains information to help SmartRay Applications Engineer to create
*         a custom SmartXtract preset.\n
*         \b To know more on how you can avail a SmartXtract license or custom SmartXtract preset, please get in touch
*         with your SmartRay sales or technical contact person.\n
*         \b Note: SmartXtract is supported for ECCO 95 series only and 'SmartXtract Archive' only works in Snapshot Acquisition mode only
* @param  *sensor - Sensor object
* @param  *filename - filename of archive
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_SENSOR_NOT_CONNECTED
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_ArchiveSmartXtractData(SRSensor *sensor, char const *filename);

/**
* @brief  Disable archive SmartXtract data for given sensor\n
*         \b Note: Supported for ECCO 95 series only
* @param  *sensor - Sensor object
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_SENSOR_NOT_CONNECTED
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_DisableArchiveSmartXtractData(SRSensor *sensor);

/**
* @brief  Configure SmartXtract for given @sensor, with reflection filter 
*         load from@smartXtractPresetFilename.\n
*         \b Note: Supported for ECCO 95 series only
* @param  *sensor - Sensor object
* @param  *smartXtractConfigurationFilename - File path to the SmartXtract
*         configuration file (sxt)
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_SENSOR_NOT_CONNECTED
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_SetSmartXtractPreset(SRSensor *sensor, char const*smartXtractPresetFilename);

/**
* @brief  Set profile calculation mode for given sensor
* @param  *sensor - Sensor object
* @param  mode - DataGeneration3DModeType mode
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_MISSING_LICENSE
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_SetSmartXtract3DDataGenerationMode(SRSensor *sensor, DataGeneration3DModeType mode);

/**
* @brief  Get current profile calculation mode for given sensor
* @param  *sensor - Sensor object
* @param  *mode - DataGeneration3DModeType mode
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_MISSING_LICENSE
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_GetSmartXtract3DDataGenerationMode(SRSensor *sensor, DataGeneration3DModeType *mode);

/**
* @brief  Set exposure mode for current reflection filter for given sensor
* @param  *sensor - Sensor object
* @param  mode - reflection filter exposure mode
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_MISSING_LICENSE
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_SetSmartXtractAlgorithm(SRSensor *sensor, SmartXtractAlgorithmType mode);

/**
* @brief  Get current exposure mode for current reflection filter for given sensor
* @param  *sensor - Sensor object
* @param  *mode - reflection filter exposure mode
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_MISSING_LICENSE
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_GetSmartXtractAlgorithm(SRSensor *sensor, SmartXtractAlgorithmType *mode);

/** @} */ // end of SMARTXTRACT

/** @} */ // end of SMARTX

/** @defgroup EXPOSURE Exposure & Gain
 *  List of API functions related to exposure and gain configuration
 *  @{
 */

/**
 *	@brief	Set exposure time (by ID) of the sensor
 *	@param	sensor		    -	Sensor object
 *	@param	expIndex	    -	Exposure ID \n
 * 							    Range: \n
 *								    ECCO 35: 0 - 1 (Double-Exposure) \n
 *								    ECCO 55: 0 - 1 (Double-Exposure) \n
 *								    ECCO 75: 0 - 1 (Double-Exposure) \n
 *								    ECCO 95: 0 - 3 (Quad-Exposure) \n
 *								    SR5600-SR9600: 0 - 1 (Double-Exposure)
 *	@param	expTimeMicroS -	    Exposure time in micro seconds. \n
 *							    \b Note: Setting exposure time ID to 0 disables that exposure ID. \n
 *							    \b Example: Setting Exposure ID 2 = 0 would disable Double Exposure)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_EXPOSURE_INDEX_NOT_SUPPORTED
 *	@return	#ERR_SR_API_EXPOSURE_OUT_OF_RANGE
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetExposureTime(SRSensor* sensor, int32_t expIndex, int32_t expTimeMicroS);

/**
 *	@brief	Get exposure time (by ID) of the sensor
 *	@param	sensor		    -	Sensor object
 *	@param	expIndex	    -	Exposure ID
 *	@param	*expTimeMicroS	-	Exposure time (value in micro seconds)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_EXPOSURE2_NOT_ENABLED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetExposureTime(SRSensor* sensor, int32_t expIndex, int32_t *expTimeMicroS);

/**
 *	@brief	Set number of exposure times i.e. number of exposure time ID's
 *	@param	sensor					-	Sensor object
 *	@param	numberOfExposureTimes	-	Number of exposure time ID's, see range below \n
 * 										Range: \n
 *											- ECCO 35: 0 - 1 (Double-Exposure)
 *											- ECCO 55: 0 - 1 (Double-Exposure)
 *											- ECCO 75: 0 - 1 (Double-Exposure)
 *											- ECCO 95: 0 - 3 (Quad-Exposure)
 *											- SR5600-SR9600: 0 - 1 (Double-Exposure)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_VALUES_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetNumberOfExposureTimes(SRSensor* sensor, int32_t numberOfExposureTimes);

/**
 *	@brief	Get number of exposure times i.e. number of exposure time ID's which has been set
 *	@param	sensor					-	Sensor object
 *	@param	numberOfExposureTimes	-	current set number of exposure times
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetNumberOfExposureTimes(SRSensor* sensor, int32_t *numberOfExposureTimes);

/**
 *	@brief	Set multi-exposure mode \n
 *          \b Note: Supported for ECCO 95 series only
 *	@param	sensor					-	Sensor object
 *	@param	multiExposureMergeMode	-	Multi-exposure mode \n
 *										Range: \n
 * 											- rolling merge 				 = 0 (legacy)
 *											- no merge 						 = 1
 * 											- no merge + prune				 = 2 (no merge + prune i.e. remove inaccurate points)
 * 											- high-accuracy point selector	 = 3
 * 											- high-accuracy profile selector = 4
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_VALUES_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetMultiExposureMode (SRSensor* sensor, MultipleExposureMergeModeType multiExposureMergeMode);

/**
 *	@brief	Get multi-exposure mode \n
 *          \b Note: Supported for ECCO 95 series only
 *	@param	sensor					-	Sensor object
 *	@param	multiExposureMergeMode	-	Multi-exposure mode
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetMultiExposureMode (SRSensor* sensor, MultipleExposureMergeModeType *multiExposureMergeMode);

/**
 *	@brief	Set 'optimal value' for multi-exposure high accuracy modes i.e. high-accuracy point selector & high-accuracy profile selector
 *	@param	sensor					-	Sensor object
 *	@param	optimalValueType	    -	optimal value type
 *	@param	optimalValue	        -	optimal value
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetMultiExposureMergeModeOptimalValue (SRSensor* sensor, MultiExposureMergeModeOptimalValueType optimalValueType, uint32_t optimalValue);

/**
 *	@brief	Get 'optimal value' for multi-exposure high accuracy modes i.e. high-accuracy point selector & high-accuracy profile selector
 *	@param	sensor					         -	Sensor object
 *	@param	optimalValueType	             -	optimal value type
 *	@param	optimalValue                	 -	optimal value
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetMultiExposureMergeModeOptimalValue (SRSensor* sensor, MultiExposureMergeModeOptimalValueType *optimalValueType, uint32_t *optimalValue);

/**
 *	@brief	Set gain of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	enable		-	Enable (1) or disable (0) gain
 *	@param	gainValue	-	Gain value \n
 * 							Range: \n
 *								- ECCO 35: 0 - 16
 *								- ECCO 55: 0 - 7
 *								- ECCO 75: 0 - 100
 *								- ECCO 95: 0 - 7
 *								- SR5600-SR9600: 0 - 5
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_GAIN_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetGain(SRSensor *sensor, bool8_t enable, int32_t gainValue);

/**
 *	@brief	Get gain of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*enable		-	Enable (1) or disable (0) gain
 *	@param	*gainValue	-	Gain value
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetGain(SRSensor *sensor, bool8_t *enable, int32_t *gainValue);

/** @} */ // end of EXPOSURE

/** @defgroup PREFILTER PreFilter
*  List of API functions related to PreFilter
*  @{
*/

/**
* @brief  Set prefilter configuration of sensor to default values.
* @param  sensor Sensor object.
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER if no valid sensor object is given.
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE if function is not available for this sensor.
* @return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_SetPrefilterDefault(SRSensor *sensor);

/**
* @brief  Set minimum and maximum limits for laser line thickness.
* @pre    @min shall be less than @max.
* @param  sensor Sensor object
* @param  min Minimum value of laser line thickness.
* @param  max Maximum value of laser line thickness.
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER if no valid sensor object is given.
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE if function is not available for this sensor.
* @return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_SetPrefilterLaserLineThickness(SRSensor *sensor, uint16_t min, uint16_t max);

/**
* @brief  Get minimum and maximum limits for laser line thickness currently set.
* @param  sensor Sensor object
* @param  min [output] Minimum value of laser line thickness.
* @param  max [output] Maximum value of laser line thickness.
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER if no valid sensor object is given.
* @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE if function is not available for this sensor.
* @return #ERR_SR_API_ARG_NULLPOINTER if both output arguments are invalid.
* @return	#SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_GetPrefilterLaserLineThickness(SRSensor *sensor, uint16_t *min, uint16_t *max);

/** @} */ // end of PREFILTER

/** @defgroup STARTTRIGGER Start Trigger
 *  List of API functions related to start trigger
 *  @{
 */

 /**
 *	@brief	Set start trigger for 3D acquisition cycle
 *	@param	sensor			- Sensor object
 *	@param	source			- digital input (start trigger source) \n
 *							  Available digital input for start trigger: \n
 *                              - StartTriggerSource: Input0 (ECCO 35/55/75/95, SR5600, SR9600)
 *                              - StartTriggerSource: Input1 (ECCO 75/95 only)
 *                              - StartTriggerSource: Input2 (ECCO 75 only)
 *                              - StartTriggerSource: Input3 (ECCO 75 only)
 *	@param	enable			- enable or disable start trigger
 *	@param	condition		- start trigger condition (rising edge or falling edge)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_START_TRIGGER_ENABLE_OUT_OF_RANGE
 *	@return	#ERR_SR_API_START_TRIGGER_CONDITION_OUT_OF_RANGE
 *	@return	#ERR_SR_API_START_TRIGGER_SOURCE_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetStartTrigger(SRSensor *sensor, StartTriggerSource source, bool8_t enable, TriggerEdgeMode condition);

/**
 *	@brief	Get start trigger status of 3D acquisition cycle
 *	@param	sensor			- Sensor object
 *	@param	*source			- digital input (start trigger source) \n
 *							  Available digital input for start trigger: \n
 *                              - StartTriggerSource: Input0 (ECCO 35/55/75/95, SR5600, SR9600)
 *                              - StartTriggerSource: Input1 (ECCO 75/95 only)
 *                              - StartTriggerSource: Input2 (ECCO 75 only)
 *                              - StartTriggerSource: Input3 (ECCO 75 only)
 *	@param	*enable			- enable or disable start trigger
 *	@param	*condition		- start trigger condition (rising edge or falling edge)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_SENSOR_NUMBER_OUT_OF_RANGE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetStartTrigger(SRSensor *sensor, StartTriggerSource *source, bool8_t *enable, TriggerEdgeMode *condition);

/**
 * @brief	Set 'acquisition ready status' on digital output. \n
 *          \b Note: This feature is available only if 'start trigger' is enabled.
 * @param 	sensor 			- Sensor object
 * @param 	outputChannel 	- Digital output (\b Note: Only output 2 is supported)
 * @param 	enable 			- enable or disable the feature
 * @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
 * @return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 * @return	#SUCCESS - Successful
 */
SR_API_EXPORT int32_t SR_API_SetReadyForAcquisitionStatus(SRSensor* sensor, int32_t outputChannel, bool8_t enable);

/**
 * @brief	Get 'acquisition ready status' on digital output
 * @param 	sensor 			- Sensor object
 * @param 	outputChannel 	- Digital output (\b Note: Only output 2 is supported)
 * @param 	enable 			- enable or disable
 * @return	#SUCCESS - Successful
 * @return	#ERR_CD_NULLPOINTER - Sensor object is NULL or enable is NULL
 * @return	#ERR_FUNCTION_NOT_AVAILABLE - sensor not supported
 * @return	#ERR_FUNCTION_NOT_SUCCESSFUL - digital output channel not supported
 * @return	#ERR_PARAMETERFILE_NOT_FOUND - parameter set file not found
 */
SR_API_EXPORT int32_t SR_API_GetReadyForAcquisitionStatus(SRSensor* sensor, int32_t outputChannel, bool8_t *enable);

/** @} */ // end of STARTTRIGGER


/** @defgroup DATATRIGGER Data Trigger
 *	List of API functions related to data trigger functions
 *  @{
 */

/**
 *	@brief	Set data trigger mode
 *	@param	sensor		-	Sensor object
 *	@param	mode		- 	Data trigger mode \n
 *							Range: \n
 * 								- Free Run = 0 (as fast as possible depending on configured ROI and Exposure)
 *								- Internal = 1
 * 								- External = 2
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_DATA_TRIGGER_MODE_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetDataTriggerMode(SRSensor* sensor, DataTriggerMode mode);

/**
 *	@brief	Get data trigger mode
 *	@param	sensor		-	Sensor object
 *	@param	*mode		- 	Data trigger mode \n
 *							Range: \n
 * 								- Free Run = 0 (as fast as possible depending on configured ROI and Exposure)
 *								- Internal = 1
 * 								- External = 2
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetDataTriggerMode(SRSensor* sensor, DataTriggerMode *mode);

/**
 *	@brief	Set internal data trigger frequency
 *	@param	sensor					-	Sensor object
 *	@param	internalFrequencyHz		- 	Internal data trigger frequency (Hz) \n
 *										\b Note: Internal frequency should not be set higher than max. scan rate achievable by sensor (free run).
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_DATA_TRIGGER_FREQUENCY_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetDataTriggerInternalFrequency(SRSensor *sensor, int32_t internalFrequencyHz);

/**
 *	@brief	Get internal data trigger frequency
 *	@param	sensor					-	Sensor object
 *	@param	*internalFrequencyHz	- 	Internal data trigger frequency (Hz)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetDataTriggerInternalFrequency(SRSensor *sensor, int32_t *internalFrequencyHz);

/**
 *	@brief	Set external data trigger source
 *	@param	sensor					-	Sensor object
 *	@param	externalTriggerSource	- 	External data trigger source \n
 *										Range: \n
 *											- Input 1						= 0
 * 											- Input 2 						= 1
 *											- Input 1 or Input 2 (combined) = 2
 *											- Quadrature Encoder 			= 4
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_DATA_TRIGGER_SOURCE_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetDataTriggerExternalTriggerSource(SRSensor *sensor, DataTriggerSource externalTriggerSource);

/**
 *	@brief	Get external data trigger source
 *	@param	sensor					-	Sensor object
 *	@param	*externalTriggerSource	- 	External data trigger source
 *										Range: \n
 *											- Input 1						= 0
 * 											- Input 2 						= 1
 *											- Input 1 or Input 2 (combined) = 2
 *											- Quadrature Encoder 			= 4
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetDataTriggerExternalTriggerSource(SRSensor *sensor, DataTriggerSource *externalTriggerSource);

/**
 *	@brief	Set external data trigger settings
 *	@param	sensor				-	Sensor object
 *	@param	triggerDivider		- 	External data trigger divider (Range: 0 - 512) (\b Note: Should not be set to zero)
 *	@param	triggerDelay		- 	External data trigger delay (Range: 0 - 512)
 *	@param	triggerDirection	- 	External data trigger direction (forward or reverse / clockwise or anti-clockwise)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_DATA_TRIGGER_DELAY_OUT_OF_RANGE
 *	@return #ERR_SR_API_DATA_TRIGGER_DIRECTION_OUT_OF_RANGE
 *	@return #ERR_SR_API_DATA_TRIGGER_DIVIDER_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetDataTriggerExternalTriggerParameters(SRSensor *sensor, int32_t triggerDivider, int32_t triggerDelay, TriggerEdgeMode triggerDirection);

/**
 *	@brief	Get external data trigger settings
 *	@param	sensor				-	Sensor object
 *	@param	*triggerDivider		- 	External data trigger divider
 *	@param	*triggerDelay		- 	External data trigger delay
 *	@param	*triggerDirection	- 	External data trigger direction
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetDataTriggerExternalTriggerParameters(SRSensor *sensor, int32_t *triggerDivider, int32_t *triggerDelay, TriggerEdgeMode *triggerDirection);

 /**
 * @brief	Get external data trigger rate detected by the sensor either on encoder input or digital input, depending on configuration
 * @param 	sensor                  - Sensor object
 * @param 	externalTriggerRateHz   - Data trigger rate (Unit: Hz)
 * @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#SUCCESS - Successful
 */
SR_API_EXPORT int32_t SR_API_GetExternalTriggerRate(SRSensor *sensor, int32_t *externalTriggerRateHz);

/** @} */ // end of DATATRIGGER



/** @defgroup LASER Laser 
 *  List of API functions related to laser configuration
 *  @{
 */

/**
 *	@brief	Power-on or off the laser in parameter set
 *	@param	sensor		-	Sensor object
 *	@param	power		-	on/off
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_LASER_POWER_OUT_OF_RANGE
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetLaserPower (SRSensor* sensor, bool8_t power);

/**
 *	@brief	Get Power-on or off status from parameter set
 *	@param	sensor		-	Sensor object
 *	@param	*power		-	on/off
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetLaserPower (SRSensor* sensor, bool8_t *power);

/**
 *	@brief	Set laser mode
 *	@param	sensor		-	Sensor object
 *	@param	mode		-	Laser mode: \n
                            - 0 - pulsed wave laser mode i.e. laser is pulsed synchronous with exposure time
 *							- 1 - continuous wave laser mode i.e. laser is on continuously during the entire acquisition cycle
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_LASER_POWER_OUT_OF_RANGE
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetLaserMode (SRSensor* sensor, LaserMode mode);

/**
 *	@brief	Get laser mode
 *	@param	sensor		-	Sensor object
 *	@param	mode		-	Laser mode \n
                            Values: \n
                            - 0 - pulsed wave laser mode i.e. laser is pulsed synchronous with exposure time
 *							- 1 - continuous wave laser mode i.e. laser is on continuously during the entire acquisition cycle 
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetLaserMode (SRSensor* sensor, LaserMode *mode);

/**
 *	@brief	Set laser brightness in parameter set
 *	@param	sensor			        -	Sensor object
 *	@param	laserBrightnessPercent  - 	Laser brightness percentage (Range: 0 - 100%)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_LASER_BRIGHTNESS_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetLaserBrightness (SRSensor* sensor, int32_t laserBrightnessPercent);

/**
 *	@brief	Get laser brightness from parameter set
 *	@param	sensor				    -	Sensor object
 *	@param	*laserBrightnessPercent	- 	Laser brightness percentage
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetLaserBrightness (SRSensor* sensor, int32_t *laserBrightnessPercent);

/** @} */ // end of LASER



/** @defgroup PARAMETERSET Parameter Set
*          Sensor parameters such as ROI, exposure, number of 3D profiles to capture, etc, can be stored in a “parameter set”. \n
* 	       The parameter set can be saved on the computer as a file (*.par). \n
* 		   The parameter set file (*.par) can be loaded into the API. \n
*		   In this way, multiple parameter sets can be created and saved, and then loaded as per application requirement. \n
* 		   \a Example: \n
* 			1. Create parameter set to scan white metallic surface: WhiteMetallic.par \n
*			2. Create parameter set to scan black metallic surface: BlackMetallic.par \n
*		    3. Load the parameter sets as per application requirement. \n\n
*	        \b Note: Default parameter sets to capture Live Image data and 3D data are pre-installed by SDK here: \n
*                 \c \\\<installation-path>\\SmartRay\\SmartRay \c DevKit\\SR_API\\sr_parameter_sets\\
*          \n\n
*          \par
*       There are 3 pre-installed parameter set types: \n
*	        - Live Image
*			- PIL/ZIL Snapshot (Profile/Z-Map, Intensity, Laser Line Thickness)
*			- PIL/ZIL Repeat Snapshot (Profile/Z-Map, Intensity, Laser Line Thickness)
 *  @{
 */

/**
 *	@brief	Load parameter set from file (host-computer)
 *	@param	sensor			-	Sensor object
 *	@param	fileName		- 	Path to parameter set file (*.par)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FILE_READ_FAILED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_LoadParameterSetFromFile(SRSensor* sensor, const char* fileName);

/**
 *	@brief	Send/Load parameter to sensor
 *	@param	sensor			-	Sensor object
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SendParameterSetToSensor(SRSensor* sensor);

/**
 *	@brief	Save a parameter set to file (host-computer)
 *	@param	sensor			-	Sensor object
 *	@param	fileName		- 	Path to the parameter set file (*.par)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FILE_WRITE_FAILED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SaveParameterSet(SRSensor* sensor, const char* fileName);

/** @} */ // end of PARAMETERSET




/** @defgroup SensorDataAcquisition Sensor Data Acquisition
 *  List of API functions related to data acquisition from the sensor
 *  @{
 */

/**
 *	@brief	Set '3D data format' to be acquired by the sensor (Ex: PIL, ZIL or Point Cloud)
 *	@param	sensor				 -	Sensor object
 *	@param	imageAcquisitionType - 	3D data format type to be acquired by the sensor. \n
 *									Available 3D data formats: \n
 *										- #IMAGETYPE_PIL
 *										- #IMAGETYPE_PI (not recommended)
 *										- #IMAGETYPE_PROFILE
 *										- #IMAGETYPE_INTENSITY (not recommended)
 *										- #IMAGETYPE_ZIL
 *										- #IMAGETYPE_ZI (not recommended)
 *										- #IMAGETYPE_ZMAP
 *										- #IMAGETYPE_POINTCLOUD
 *										- #IMAGETYPE_LIVEIMAGE
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetImageAcquisitionType(SRSensor *sensor, ImageAquisitionType imageAcquisitionType);

/**
 *	@brief	Get the configured '3D data format' which will be acquired by the sensor (Ex: PIL, ZIL or Point Cloud)
 *	@param	sensor						-	Sensor object
 *	@param	*imageAcquisitionType		- 	3D data format type to be acquired by the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetImageAcquisitionType(SRSensor *sensor, ImageAquisitionType *imageAcquisitionType);

/**
 *	@brief	Set image acquisition mode for the sensor
 *	@param	sensor			-	Sensor object
 *	@param	acquisitionMode	- 	Acquisition mode of  the sensor. \n
 *								Available acquisition modes: \n
 *									- #SNAPSHOT_MODE 			(Capture configured number of profiles 'once' and automatically stop acquisition)
 *									- #REPEAT_SNAPSHOT_MODE	    (Capture configured number of profiles 'and repeat this cycle' until manually stopped by the application)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_ACQUISITION_MODE_INVALID
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetAcquisitionMode(SRSensor *sensor, AcquisitionMode acquisitionMode);

/**
 *	@brief	Get image acquisition mode of the sensor
 *	@param	sensor			 -	Sensor object
 *	@param	*acquisitionMode - 	Available acquisition modes
  *									- #SNAPSHOT_MODE 			(Capture configured number of profiles 'once' and automatically stop acquisition)
 *									- #REPEAT_SNAPSHOT_MODE	    (Capture configured number of profiles 'and repeat this cycle' until manually stopped by the application)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetAcquisitionMode(SRSensor *sensor, AcquisitionMode *acquisitionMode);

/**
 *	@brief	Set number of profiles to capture in an acquisition cycle
 *	@param	sensor			 - Sensor object
 *	@param	numberOfProfiles - Number of profiles to capture
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetNumberOfProfilesToCapture(SRSensor* sensor, uint32_t numberOfProfiles);

/**
 *	@brief	Get number of profiles to capture in an acquisition cycle
 *	@param	sensor			- Sensor object
 *	@param	numberOfProfiles- Number of profiles to capture
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetNumberOfProfilesToCapture(SRSensor* sensor, uint32_t *numberOfProfiles);


/**
 *	@brief	Set threshold (threshold applied on Live Image) to adjust sensitivity in detecting the laser line
 *          based on which the sensor generates 3D data (PIL)
 *	@param	sensor			-	Sensor object
 *	@param	exposureIndex	- 	Exposure ID on which the threshold (Live Image) is applied
 *	@param	threshold		-	Threshold \n
 *  							Default: 40 (recommended) \n
 *								Range: 0 - 255 | 0 => high sensitivity, 255 => low sensitivity
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_Set3DLaserLineBrightnessThreshold(SRSensor* sensor, int32_t exposureIndex, int32_t threshold);

/**
 *	@brief	Get threshold (threshold applied on Live Image) which has been set to adjust sensitivity in detecting the laser line
 *  		based on which the sensor generates 3D data (PIL)
 *	@param	sensor			-	Sensor object
 *	@param	exposureIndex	- 	Exposure ID on which the threshold (Live Image) is applied
 *	@param	*threshold		-	Threshold \n
 *  							Default: 40 (recommended) \n
 *								Range: 0 - 255 | 0 => high sensitivity, 255 => low sensitivity
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_Get3DLaserLineBrightnessThreshold(SRSensor* sensor, int32_t exposureIndex, int32_t *threshold);

/**
 *	@brief	Set reflection filter
 *	@param	sensor		-	Sensor object
 *	@param	enable		-	Boolean flag to enable/disable the reflection filter
 *	@param	algorithm	-	Configures the reflection filter algorithm to be used: \n
 *                              - 0 : Algorithm 0
 *                              - 1 : Algorithm 1
 *	@param	preset		-   Reflection filter preset to be used
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *  @return #ERR_SR_API_SENSOR_NOT_CONNECTED
 *  @return #ERR_SR_API_REFLECTIONFILTER_ALGORITHM_OUT_OF_RANGE
 *  @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
 *  @return #ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetReflectionFilter(SRSensor* sensor, bool8_t enable, int32_t algorithm, int32_t preset);

/**
 *	@brief	Get reflection filter
 *	@param	sensor		-	Sensor object
 *	@param	*enable		-	Feature status, enabled or disabled
 *	@param	*algorithm	-	Reflection filter algorithm
 *	@param	*preset		-	Reflection filter preset
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *  @return #ERR_SR_API_SENSOR_NOT_CONNECTED
 *  @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
 *  @return #ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetReflectionFilter(SRSensor* sensor, bool8_t *enable, int32_t *algorithm, int32_t *preset);

/**
 * @brief Get scan rate of the sensor based on ROI, exposure and data trigger configuration. Additionally, get access to a warning flag indicating "data trigger overflow". \n
 *			\b Note: Data trigger overflow can occur when 'data trigger rate' (Ex: data trigger from encoder) is configured higher than 'maximum scan rate' achievable by the sensor for the configured ROI and exposure. \n
 *			Scan rate of the sensor typically depends on 2 factors: ROI and Exposure \n
 *			Maximum scan rate of the sensor is the scan rate achieved in 'free run' data trigger mode
 * @param 	sensor 			- Sensor object
 * @param 	scanRateHz 		- Scan rate of the sensor (Unit: Hz)
 * @param 	triggerOverflow - flag (0/1) indicating trigger overflow
 * @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#SUCCESS - Successful
 */
SR_API_EXPORT int32_t SR_API_GetScanRate(SRSensor *sensor, int32_t *scanRateHz, int32_t *triggerOverflow);

/**
 * @brief	Get maximum scan rate of the sensor based on ROI and exposure (assuming  data trigger mode: free run)
 * @param 	sensor 		    - Sensor object
 * @param 	maxScanRateHz   - Maximum scan rate (Unit: Hz)
 * @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#SUCCESS - Successful
 */
SR_API_EXPORT int32_t SR_API_GetMaximumScanRate(SRSensor *sensor, int32_t *maxScanRateHz);

/**
 * @brief Get transmission rate between sensor and host-computer \n
 *			\b Note: Useful to troubleshoot in case there are data loss over the ethernet (network) interface\n
 *			\b Note: In a multi-sensor setup, the transmission rate provided is a combination of data transmitted from all sensors connected to the API (PC). 
 * @param 	sensor 				- Sensor object
 * @param 	transmissionRateHz 	- Transmission rate (Unit: Hz)
 * @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#SUCCESS - Successful
 */
SR_API_EXPORT int32_t SR_API_GetTransmissionRate(SRSensor *sensor, int32_t *transmissionRateHz);

/**
 *	@brief	Set packet size \n
 *          \b Note: Use this function with SR_API_SetNumberOfProfilesToCapture()
 *	@param	sensor		- Sensor object
 *	@param	packetSize	- packet size \n
 *						  \b Note:
 *                              It is recommended to set packet size to 0 which switches to 'auto packet mode' where an optimal packet size is computed to ensure best data transmission over ethernet interface. \n
 *						        If 'auto packet mode' is not used, packet size should be set to a multiple of 'number of profiles to capture'
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetPacketSize(SRSensor* sensor, uint32_t packetSize);

/**
 *	@brief	Get packet size \n
 *          \b Note: Use this function with SR_API_GetNumberOfProfilesToCapture()
 *	@param	sensor		- Sensor object
 *	@param	packetSize	- packet size
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetPacketSize(SRSensor* sensor, uint32_t *packetSize);

/**
 *	@brief	Set packet timeout
 *	@param	sensor			- Sensor object
 *	@param	packetTimeOut   - Packet timeout
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetPacketTimeOut(SRSensor* sensor, uint32_t packetTimeOut);

/**
 *	@brief	Get packet timeout
 *	@param	sensor		 	- Sensor object
 *	@param	packetTimeOut  	- Packet timeout
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetPacketTimeOut(SRSensor* sensor, uint32_t *packetTimeOut);

/**
 *	@brief	Start data acquisition from the sensor
 *	@param	sensor - Sensor object
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_SENSOR_ACTIVE
 *	@return	#ERR_SR_API_MEMORY_ALLOCATION_FAILED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_SENSOR_NOT_CONNECTED
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_StartAcquisition (SRSensor* sensor);

/**
 *	@brief	Stop data acquisition from the sensor
 *	@param	sensor - Sensor object
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_SENSOR_NOT_ACTIVE
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_SENSOR_NOT_DISCONNECTED
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_StopAcquisition (SRSensor* sensor);

/** @} */ // end of DATAACQUISITION


/** @defgroup METADATA Metadata
 *  Functions related to metadata
 *  @{
 */

/**
 * @brief	Set metadata i.e. additional data which are appended to every PIL, for example: timestamp, Data trigger counter, etc. \n
 *           \b Note: Applicable for ECCO 95 series only
 *	@param	sensor	 - Sensor object
 * @param	metadata - Boolean flag to enable or disable the feature.
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
 * @return	#SUCCESS
 */
SR_API_EXPORT int32_t SR_API_SetMetaDataExportEnabled(SRSensor *sensor, bool8_t metadata);

/**
* @brief	Get max time stamp in nanoseconds of meta data before it overflows. \n
*           \b Note: Applicable for ECCO 95 series only
*	@param	sensor	 - Sensor object
* @param	timeStampCycle - cycle time in ns for time stamps.
* @return	#SUCCESS if successful.
* @return	#ERR_SR_API_SRSENSOR_NULLPOINTER if sensor is not valid.
* @return	#ERR_SR_API_SENSOR_NOT_CONNECTED if sensor is not connected.
* @return	#ERR_SR_API_ARG_NULLPOINTER if argument to store result in is not valid.
* @return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE if function is not applicable for this sensor.
* @return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL otherwise.
*/
SR_API_EXPORT int32_t SR_API_GetMetaDataTimeStampPeriod(SRSensor *sensor, uint64_t *timeStamp);

/**
 * @brief	Get sensor temperature
 * @param	sensor 		- Sensor object
 * @param	temperature - temperature value (Unit: Degree Celsius)
 * @return	#SUCCESS
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 */
SR_API_EXPORT int32_t SR_API_GetSensorTemperature(SRSensor *sensor, float *temperature);

/** @} */ // end of METADATA



/** @defgroup CALIBRATION Calibration
 *  List of API functions related to calibration file and working with calibrated data (Point Cloud, ZIL)
 *  @{
 */

/**
 *	@brief	Load calibration data (*.lut) from file (host computer)
 *	@param	sensor			-	Sensor object
 *	@param	fileName		- 	Path to calibration file (*.lut)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_FILE_READ_FAILED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_LoadCalibrationDataFromFile(SRSensor *sensor, char *fileName);

/**
 *	@brief	Load calibration data (*.lut) from sensor (sensor's internal memory), if calibration file has been saved on the sensor \n
 *			\b Note: The calibration file is typically saved on the sensor by default in the factory
 *	@param	sensor			-	Sensor object
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_CALIBRATION_FILE_NOT_PRESENT_ON_SENSOR
 *	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_LoadCalibrationDataFromSensor(SRSensor *sensor);

/**
 *	@brief	Check if calibration file has been stored on the sensor
 *	@param	sensor			-	Sensor object
 *	@param	*available		- 	Calibration file stored on sensor status: Available or Not available
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_IsCalibrationDataAvailableOnSensor(SRSensor *sensor, bool8_t *available);

/**
 *	@brief	Transform Profile to Point Cloud using information from calibration file \n
 *			\b Note: Profile is 'P'IL image coordinate system => pixel world \n
 *				  Point Cloud is world coordinate system => milli-meter world
 *	@param	sensor - Sensor object
 *	@param	*profile - Pointer to single profile received from sensor (input array)
 *	@param	originX - ROI-OriginX (in pixels)
 *	@param	width - ROI-Width (in pixels)
 *	@param	*pointCloud - Pointer to the Point Cloud (output array)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_CreatePointCloudSingleProfile(
	SRSensor *sensor, uint16_t *profile,
	int32_t originX, int32_t width, SR_3DPOINT *pointCloud);
/**
 *	@brief	Transform a set of Profiles to Point Cloud using information from calibration file \n
 *			\b Note: Profile is 'P'IL image coordinate system => pixel world \n
 *				  Point Cloud is world coordinate system => milli-meter world
 *	@param	sensor - Sensor object
 *	@param	profile - Pointer to a set of profiles (Ex: 100 profiles) received from sensor (input array)
 *	@param	originX - ROI-OriginX (in pixels)
 *	@param	width - ROI-Width (in pixels)
 *	@param	numberOfProfiles - Number of Profiles to be transformed to Point Cloud
 *	@param	pointCloud - Pointer to the Point Cloud (output array)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return #ERR_SR_API_MEMORY_ALLOCATION_FAILED
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_CreatePointCloudMultipleProfile(
	SRSensor *sensor, uint16_t *profiles, int32_t originX,
	int32_t width, int32_t numberOfProfiles, SR_3DPOINT *pointCloud);

/**
* @brief Applies transport resolution for sequence of profiles.
* @param sensor Sensor object.
* @param *world Preallocated array of points to be changed of size @width * @height.
* @param width  Number of points per profile.
* @param height Number of profiles.
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_ApplyTransportResolution(
	SRSensor *sensor, SR_3DPOINT *world, int32_t width, int32_t height);

/**
* @brief Applies transport resolution to one profile
* @param xCoordinateOffset Offset added to x-coordinate of profile points.
* @param *world Preallocated array of points to be changed of size @widht * @height.
* @param size   Number of points of profile.
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_ApplyTransportResolutionOffset(
	float xCoordinateOffset, SR_3DPOINT *world, int32_t size);

/**
 *	@brief	Use this function mainly as 'helper function' before using SR_API_CreateZMap() function
 *	@param	sensor					-	Sensor object
 *	@param	originX					-	ROI-OriginX (in pixels)
 *	@param	width					-	ROI-Width (in pixels)
 *	@param	lateralResolution		-	Desired lateral resolution for ZIL (in milli-meters)
 *	@param	verticalResolution		-	Desired vertical resolution for ZIL (in milli-meters)
 *	@param	*zMapWidth				-	Computed 'ROI-Width (in pixels)' for ZIL
 *	@param	*fovRange				-	Computed maximum MR (measurement range)
 *	@param	*fovStartPos			-	Computed FOV start position (in milli-meters)
 *	@param	*fovEndPos				-	Computed FOV end position (in milli-meters)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return #ERR_SR_API_ORIGIN_Y_RANGE_ERROR
 *	@return	#ERR_SR_API_ORIGIN_X_RANGE_ERROR
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetZmapDimensions(SRSensor *sensor, int32_t originX, int32_t width,
	float lateralResolution, float verticalResolution,
	uint32_t *zMapWidth, float *fovRange, float *fovStartPos, float *fovEndPos);
/**
 *	@brief	Convert  Point Cloud to ZIL image by remapping/resampling based on the lateral & vertical resolution
 *	@param	sensor					-	Sensor object
 *	@param	originX					-	ROI-OriginX(in pixels)
 *	@param	width					-	ROI-Width (in pixels)
 *	@param	height					-	ROI-Height (in pixels)
 *	@param	lateralResolution		-	Lateral resolution for ZIL => This defines the constant spacing
 *   									between consecutive pixels of the ZIL images along the laser line
 *	@param	verticalResolution		-	Vertical resolution for ZIL => This defines minimum vertical (height or Z) resolution
 *										for the ZIL images
 *	@param *profile					-	Pointer to a set of profiles (Ex: 100 profiles) received from sensor (input array)
 *	@param *intensity				-	Pointer to a set of intensity (Ex: 100 lines of intensity) received from sensor (input array)
 *	@param *llt						-	Pointer to a set of laser line thickness (Ex: 100 lines of laser line thickness) received from sensor (input array)
 *	@param	*zmapWidth				-	ZIL image width (in pixels)
 *	@param	*zMap					-	Z-map image => Z
 *	@param	*intensityZMap			-	Intensity image (remapped) => I
 *	@param	*lltZmap				-	Laser Line Thickness image (remapped) => L
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ZMAP_WIDTH_INVALID
 *	@return #ERR_SR_API_ZMAP_HEIGHT_INVALID
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_MEMORY_ALLOCATION_FAILED
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_CreateZMap (
	SRSensor *sensor, uint32_t originX, uint32_t width, uint32_t height,
	float lateralResolution, float verticalResolution,
	uint16_t *profile,  uint16_t *intensity,  uint16_t *llt,
	uint32_t *zmapWidth,
	uint16_t *zMap, uint16_t *intensityZMap, uint16_t *lltZmap);
/**
 *	@brief	Set Z-map resolution
 *	@param	sensor					-	Sensor object
 *	@param	lateralResolution		- 	Lateral resolution in milli-meters (along laser line, Y)
 *	@param	verticalResolution		-	Vertical resolution in milli-meters (height, Z)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ZMAP_LATERAL_RESOLUTION_OUT_OF_RANGE
 *	@return	#ERR_SR_API_PARAMAETER_SET_NOT_INITIALIZED
 *	@return	#ERR_SR_API_ZMAP_VERTICAL_RESOLUTION_OUT_OF_RANGE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetZmapResolution(SRSensor* sensor, float lateralResolution, float verticalResolution);

/**
 *	@brief	Get Z-map resolution
 *	@param	sensor					-	Sensor object
 *	@param	*lateralResolution		- 	Lateral resolution in milli-meters (along laser line, Y)
 *	@param	*verticalResolution		-	Vertical resolution in milli-meters (height, Z)
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetZmapResolution(SRSensor* sensor, float *lateralResolution, float *verticalResolution);

/**
 * @brief	Get measurement range (MR) of the sensor model with reference to live image in image coordinates (pixel)
 * @param 	sensor 	- Sensor object
 * @param 	min 	- near field ID i.e. Y-coordinate (row number) in Live Image
 * @param 	max 	- far field ID i.e. Y-coordinate (row number) in Live Image
 * @return	#ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#SUCCESS - Successful
 */
SR_API_EXPORT int32_t SR_API_GetMeasurementRange(SRSensor *sensor, int32_t *min, int32_t *max);

/**
* @brief  Set pitch angle for tilt correction of point cloud
* @param  sensor - Sensor object
* @param  degree - angle in degree, has to be in range [-90, 90]
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL 
* @return #SUCCESS - Successful
*/
SR_API_EXPORT int32_t SR_API_SetTiltCorrectionPitch(SRSensor *sensor, float degree);

/**
* @brief  Get pitch angle for tilt correction of point cloud
* @param  sensor - Sensor object
* @param  degree - angle in degree, will be in range [-90, 90]
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL 
* @return #SUCCESS - Successful
*/
SR_API_EXPORT int32_t SR_API_GetTiltCorrectionPitch(SRSensor *sensor, float *degree);

/**
* @brief  Set yaw angle for tilt correction of point cloud
* @param  sensor - Sensor object
* @param  degree - angle in degree, has to be in range [-90, 90]
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL 
* @return #SUCCESS - Successful
*/
SR_API_EXPORT int32_t SR_API_SetTiltCorrectionYaw(SRSensor *sensor, float degree);

/**
* @brief  Get yaw angle for tilt correction of point cloud
* @param  sensor - Sensor object
* @param  degree - angle in degree, will be in range [-90, 90]
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL 
* @return #SUCCESS - Successful
*/
SR_API_EXPORT int32_t SR_API_GetTiltCorrectionYaw(SRSensor *sensor, float *degree);

/**
* @brief  Set transport resolution for point cloud
* @param  sensor - Sensor object
* @param  transportResolution - transport resolution in mm/profile
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL 
* @return #SUCCESS - Successful
*/
SR_API_EXPORT int32_t SR_API_SetTransportResolution(SRSensor *sensor, float transportResolution);

/**
* @brief  Get transport resolution for point cloud
* @param  sensor - Sensor object
* @param  transportResolution - transport resolution in mm/profile
* @return #ERR_SR_API_SRSENSOR_NULLPOINTER
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL 
* @return #SUCCESS - Successful
*/
SR_API_EXPORT int32_t SR_API_GetTransportResolution(SRSensor *sensor, float *transportResolution);

/** @} */ // end of CALIBRATION

/** @defgroup MSR Multi-Sensor Registration (MSR)
*  List of API functions to related to configure Multi-Sensor Registration (MSR)
*  @{
*/
/**
*	@brief 	The function is enable or disable MSR mode
*  @param 	enable - enable or disable MSR mode
*  @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
*  @return #SUCCESS
*/
SR_API_EXPORT int32_t SR_API_MSR_EnableRegistration(bool8_t enable);
/**
 * @brief	The function is used to load the registration file for MSR mode
 * @param	filePath - file path for the MSR registration file
 * @return	#ERR_SR_API_MSR_NOT_ENABLED
 * @return	#ERR_SR_API_MSR_INVALID_NUMBER_OF_SENSORS
 * @return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 * @return	#SUCCESS
 */
SR_API_EXPORT int32_t SR_API_MSR_LoadRegistrationFile(const char* filePath);
/**
 * @brief	The function is used to set the resolution for the MSR mode ZMAP creation
 * @param	transportResolution     - transport resolution along X direction (transport rate/ scan rate)
 * @param	lateralResolution       - lateral resolution along Y direction
 * @param	verticalResolution      - vertical resolution along Z direction
 * @return	#ERR_SR_API_MSR_NOT_ENABLED
 * @return	#ERR_SR_API_MSR_INVALID_RESOLUTION
 * @return	#SUCCESS
 */
SR_API_EXPORT int32_t SR_API_MSR_SetZmapResolution(double transportResolution, double lateralResolution, double verticalResolution);
/**
 * @brief	The function is used to get the resolution for the MSR mode ZMAP creation
 * @param	*transportResolution    - transport resolution along X direction (scan speed/ scan rate)
 * @param	*lateralResolution      - lateral resolution along Y direction
 * @param	*verticalResolution     - vertical resolution along Z direction
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#ERR_SR_API_MSR_NOT_ENABLED
 * @return	#SUCCESS
 */
SR_API_EXPORT int32_t SR_API_MSR_GetZmapResolution(double *transportResolution, double *lateralResolution, double *verticalResolution);

/**
 * @brief  The function is used to validate MSR settings
 * @return #ERR_SR_API_MSR_NOT_MATCHING_PROFILE_NUMBER
 * @return #SUCCESS
 */
SR_API_EXPORT int32_t SR_API_MSR_CheckSettings();

/**
* @brief  Set MSR merge mode of current configuration. Defines how conflicting z-values handled while generating
*         Zmap of MSR sensors.
* @return #ERR_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_MSR_NOT_ENABLED
* @return #SUCCESS
*/
SR_API_EXPORT int32_t SR_API_MSR_SetZmapMergeMode(MSRMergeModeType mode);
/**
* @brief  Get MSR merge mode of current configuration.
* @return #ERR_FUNCTION_NOT_SUCCESSFUL
* @return #ERR_SR_API_ARG_NULLPOINTER
* @return #ERR_SR_API_MSR_NOT_ENABLED
* @return #SUCCESS
*/
SR_API_EXPORT int32_t SR_API_MSR_GetZmapMergeMode(MSRMergeModeType *mode);
/** @} */ // end of MSR

/** @addtogroup SMARTX
 *  @{
 */

/** @defgroup SMARTXACT SmartXact
 *  List of API functions for SmartXact feature.
 *  @{
 */

 /**
 * @brief  This function can be used to improve accuracy and/or repeatability capability, depending on the application.
 *         In addition to the standard ‘default mode (mode = SR_API_SMARTXACT_MODE_DEFAULT)’ of generating 3D data,
 *         a NEW 3D data generation mode called 'SmartXact Metrology Mode (mode = SR_API_SMARTXACT_MODE_METROLOGY)' is
 *         available to help further improve accuracy and/or repeatability depending on the application.
 * @param  mode SR_API_SMARTXACT_MODE_DEFAULT or ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 * @return #SUCCESS
 * @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
 * @return #ERR_SR_API_MISSING_LICENSE
 * @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 */
SR_API_EXPORT int32_t SR_API_SetSmartXactMode(SRSensor *sensor, int32_t mode);
/**
 * @brief  The function is used to get the current SmartXact mode
 * @param mode SR_API_SMARTXACT_MODE_DEFAULT or ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 * @return #SUCCESS
 * @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 * @return #ERR_SR_API_FUNCTION_NOT_AVAILABLE
 * @return #ERR_SR_API_MISSING_LICENSE
 * @return #ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 */
SR_API_EXPORT int32_t SR_API_GetSmartXactMode(SRSensor *sensor, int32_t *mode);

/**
*  @brief  Get license informations for a connected sensor.
*  @param  sensor object
*  @param  outFeatures	Array of available features for specific sensor. This is organized as following:
*          features[numbFeatures][bufferSize]. Memory allocation has to happen outside of
*          this function.
*  @param  numberOfFeatures Number of licenses
*  @param  bufferSize Buffer size for char array for the feature name (recommendation: 64).
*  @return ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
*  @return ERR_SR_API_ARG_NULLPOINTER
*  @return SUCCESS
*/
SR_API_EXPORT int32_t SR_API_GetSensorLicenseFeatures(SRSensor* sensor, char** outFeatures, uint32_t numberOfFeatures, uint32_t bufferSize);

/**
*  @brief  Get number of licenses for a connected sensor.
*  @param  sensor object
*  @param  outNumberOfFeatures Number of licenses for a specific sensor.
*  @param  outMaxBufferSize Maximal bufferSize for the specific licenses
*  @return ERR_SR_API_ARG_NULLPOINTER
*  @return SUCCESS
*/
SR_API_EXPORT int32_t SR_API_GetSensorNumberOfLicenseFeatures(SRSensor* sensor, uint32_t* outNumberOfFeatures, uint32_t* outMaxBufferSize);

/** @} */ // end of SMARTXACT

/** @} */ // end of SMARTX

/** @defgroup POSTPROCESSING Post-Processing
 *	List of API functions for Post-processing
 *  @{
 */

/**
 * @brief	Apply mean filter on image data
 * @param 	kernelSizeX 	- mean filter kernel size X
 * @param 	kernelSizeY 	- mean filter kernel size Y
 * @param   excludeZeros 	- ignore zero's (pixel = 0) of input data
 * @param   imageWidth 		- image width
 * @param   imageHeight 	- image height
 * @param   inputData 		- pointer to input image array
 * @param   resultData 		- pointer to result image array
 * @return	#SUCCESS
 */
SR_API_EXPORT int32_t SR_API_MeanFilter(uint32_t kernelSizeX, uint32_t kernelSizeY, uint32_t excludeZeros, uint32_t imageWidth, uint32_t imageHeight, const uint16_t* inputData, uint16_t* resultData);

/**
 * @brief	Apply median filter on image data
 * @param 	kernelSizeX 	- median filter kernel size X
 * @param 	kernelSizeY 	- median filter kernel size Y
 * @param   excludeZeros 	- ignore zero's (pixel = 0) of input data
 * @param   imageWidth 		- image width
 * @param   imageHeight 	- image height
 * @param   inputData 		- pointer to input image array
 * @param   resultData 		- pointer to result image array
 * @return	#SUCCESS
 */
SR_API_EXPORT int32_t SR_API_MedianFilter(uint32_t kernelSizeX, uint32_t kernelSizeY, uint32_t excludeZeros, uint32_t imageWidth, uint32_t imageHeight, const uint16_t* inputData, uint16_t* resultData);

/**
 * @brief	Apply 2D outlier filter on image data \n
 *			\b Note: This filter replaces outliers with zeros (that means no data!)
 * @param 	kernelSizeX 	- outlier filter kernel size X
 * @param 	kernelSizeY 	- outlier filter kernel size Y
 * @param   outlierHeight   - outlier height
 * @param   neighbourRatio   - neighbour ratio threshold
 * @param   imageWidth 		- image width
 * @param   imageHeight 	- image height
 * @param   inputData 		- pointer to input image array
 * @param   resultData 		- pointer to result image array
 * @return	#SUCCESS
 */
SR_API_EXPORT int32_t SR_API_OutlierFilter_2DHeight(uint32_t kernelSizeX, uint32_t kernelSizeY, uint32_t outlierHeight, double neighbourRatio,
    uint32_t imageWidth, uint32_t imageHeight, const uint16_t* inputData, uint16_t* resultData);

/**
 * @brief	Smoothen profile image data \n
 *			\b Note: This filter is customized for 3D applications as it makes sure to smooth the data but still preserving the profile shape
 * @param	data 	- pointer to input profile image data
 * @param	width 	- profile image width, must be larger than kernel size
 * @param	height 	- profile image height, must be larger than kernel size
 * @param	preset 	- strength of smoothing
 *					  Available presets:
 *					  	#SMOOTH_MINIMAL
 *						#SMOOTH_LIGHT
 *						#SMOOTH_MEDIUM
 *						#SMOOTH_STRONG
 *						#SMOOTH_MAXIMUM
 * @return	#SUCCESS
 * @return	#ERR_SR_API_ARG_NULLPOINTER
 * @return	#ERR_SR_API_SMOOTHEN_PRESET_INVALID
 */
SR_API_EXPORT int32_t SR_API_SmoothImage(uint16_t *data, uint32_t width, uint32_t height, SmoothingPresets preset);

/** @} */ // end of POSTPROCESSING




/** @defgroup ADMIN Administration
 *	List of API functions related to sensor administration
 *  @{
 */

/**
 *	@brief	Get model name and the part number of the sensor
 *	@param	sensor				-	Sensor object
 *	@param	*modelName			- 	Name of the sensor model
 *	@param	*partNumber			-	Part number of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorModelName(SRSensor *sensor, char* modelName, char* partNumber);

/**
 *	@brief	Get the part number of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*partNumber	-	Part number of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return #SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorPartNumber(SRSensor *sensor, char* partNumber);

/**
*	@brief  Get the part number revision of the sensor
*	@param  sensor - Sensor object
*	@param  *partNumberRevision - Part number revision of the sensor
*	@return #ERR_SR_API_SRSENSOR_NULLPOINTER
*	@return	#ERR_SR_API_ARG_NULLPOINTER
*	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
*	@return #SUCCESS - successful
*/
SR_API_EXPORT int32_t SR_API_GetSensorPartNumberRevision(SRSensor *sensor, uint16_t* partNumberRevision);

/**
 *	@brief	Get serial number of the sensor
 *	@param	sensor			-	Sensor object
 *	@param	*serialNumber	- 	Serial number of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorSerialNumber(SRSensor *sensor, char* serialNumber);

/**
 *	@brief	Get firmware version of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*version	- 	Firmware version of the sensor \n
 *							\b Note: Required buffer size for firmware version are as follows: \n
 *								- ECCO 35, ECCO 55, SR5600, SR9600 series	: 128 bytes
 *							    - ECCO 75, ECCO 95 series					: 6464 bytes
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorFirmwareVersion(SRSensor *sensor, char* version);


/**
 *	@brief	Get MAC address of the sensor
 *	@param	sensor		-  Sensor object
 *	@param	*macAddress	-  MAC address of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorMacAddress(SRSensor *sensor, char* macAddress);

/**
 *	@brief	Set subnet mask of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*subnetMask	- 	Subnet mask of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetSensorSubnetMask(SRSensor *sensor, const char *subnetMask);

/**
 *	@brief	Get subnet mask of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*subnetMask	- 	Subnet mask of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorSubnetMask(SRSensor *sensor, char *subnetMask);

/**
 *	@brief	Set network gateway of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*gateway	- 	Network gateway of the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetSensorNetworkGateway(SRSensor *sensor, const char *gateway);

/**
 *	@brief	Get network gateway of the sensor
 *	@param	sensor		-	Sensor object
 *	@param	*gateway	- 	Network gateway of the sensor
 *  @return ##ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_NO_MPAR_AVAILABLE
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_GetSensorNetworkGateway(SRSensor *sensor, char *gateway);

/**
 *	@brief	Modify IP address and the port number of the sensor. \n
 *         \b Note-1: USE THIS FUNCTION WITH CAUTION AS WRONG USAGE MAY BREAK THE SENSOR \n
 *		   \b Note-2: It is STRONGLY RECOMMENDED TO UPDATE IP ADDRESS & PORT NUMBER USING STUDIO 4 SOFTWARE
 *	@param	sensor				-	Sensor object
 *	@param	ipAddress			- 	New IP address for the sensor
 *	@param	portNumber			-	New port number for the sensor
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#ERR_SR_API_NO_SYSPAR_AVAILABLE
 *	@return	#ERR_SR_API_IP_OR_PORT_IS_ZERO
 *	@return	#ERR_SR_API_IP_IS_INVALID
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_ChangeSensorIpAddress(SRSensor* sensor, unsigned char* ipAddress, uint16_t portNumber);

/**
 *	@brief	Update firmware of the sensor \n
 *          \b Note 1: USE THIS FUNCTION WITH CAUTION AS WRONG USAGE MAY BREAK THE SENSOR \n\n
 *			\b Note 2: It is STRONGLY RECOMMENDED TO UPDATE SENSOR FIRMWARE USING STUDIO 4 SOFTWARE
 *	@param	sensor				-	Sensor object
 *	@param	*fileName			- 	Path to sensor firmware update file \n
 *									\b Note: Every sensor series has a different firmware update file
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_ARG_NULLPOINTER
 *	@return	#ERR_SR_API_INVALID_FILENAME_OR_PATH
 *	@return	#ERR_SR_API_FIRMWARE_NOT_COMPATIBLE_WITH_SENSOR_SERIES
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_UpdateSensorFirmware(SRSensor *sensor, char* fileName);

/** @} */ // end of ADMIN


/** @defgroup DIGITALOUTPUT Digital Output
 *  List of API functions related to controlling sensor's digital outputs
 *  @{
 */

/**
 *	@brief	Set digital output of sensor
 *	@param	sensor			-	Sensor object
 *	@param	channel			- 	digital output number \n
 *								Available digital outputs: \n
 *									- Digital Output 1
 *									- Digital Output 2
 *	@param	enable			-	output enable
 *  @return #ERR_SR_API_SRSENSOR_NULLPOINTER
 *	@return	#ERR_SR_API_FUNCTION_NOT_AVAILABLE
 *	@return	#ERR_SR_API_FUNCTION_NOT_SUCCESSFUL
 *	@return	#SUCCESS - successful
 */
SR_API_EXPORT int32_t SR_API_SetDigitalOutput(SRSensor* sensor, DigitalOutput channel, bool8_t enable);

/** @} */ // end of DIGITALOUTPUT







/** @defgroup Logging API Logging
 *  List of API functions to troubleshoot API
 *  @{
 */

/**
 *	@brief 	Debug SmartRay API if necessary by enabling file logging. If enabled, a log file is created with the date and timestamp
 *  @param 	enable      - boolean flag to enable this feature (TRUE = enable, FALSE = disable)
 *  @param 	folderPath	- Folder (path) of the log file
 *	@return #ERR_SR_API_VALUES_OUT_OF_RANGE
 *  @return #SUCCESS
 */
SR_API_EXPORT int32_t SR_API_EnableFileLogging(bool8_t enable, const char* folderPath);

/**
*	@brief 	Enable console logging of the SmartRay API
*   @param 	enable  - boolean flag to enable this feature (TRUE=enable, FALSE=disable)
*	@return #ERR_OUT_OF_RANGE - LogLevel Value is out of range
*   @return #SUCCESS
*/
SR_API_EXPORT int32_t SR_API_EnableConsoleLogging(bool8_t enable);

/** @} */ // end of Logging

#ifndef SR_API_DO_NOT_USE_DEPRECATED
#include "SR_API_deprecated.h"
#endif

#ifdef __cplusplus
}

#endif
#endif
