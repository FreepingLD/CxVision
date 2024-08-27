using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Sensor
{
    public enum UserResponseType
    {
        NoneResponseType = 0x0,
        OKResponseType = 0x01,
        CancelResponseType = 0x02,
        IgnoreResponseType = 0x04,
        RetryResponseType = 0x08,
        YesResponseType = 0x10,
        NoResponseType = 0x20
    };
    public enum UserNotificationType
    {
        InfoNotificationType,
        WarningNotificationType,
        ErrorNotificationType
    };

    public delegate void PositioningStatusNotificationFunction(ref object pFunctionCallerInfo_p, bool PositionReached_p, int ErrorCode_p);
    public delegate void FrameCallbackFunction(object pArg);
    public delegate void  MultipleMeasurementsModeExecutedMeasurementsNumberCallbackFunction (object pFunctionCallerInfo_p, int MeasurementsNumber_p);
    public delegate UserResponseType GetUserResponseFunction (int ErrorCode_p, string pAdditionalErrorMessage_p, UserResponseType AvailableUserResponseTypes_p, UserNotificationType UserNotificationType_p);
    public delegate void  ProcessProgramMessageFunction (int ErrorCode_p, string pAdditionalErrorMessage_p, UserNotificationType UserNotificationType_p);


    public class smartSharpInterface
    {
        /** @fn int InitializeSmartVIS3DInterface(char* pSettingsFileName_p);
		@brief global initialization of the Interface and all connected hardware
		@param [in] pSettingsFileName_p fully	qualified filename for DLL main XML configuration file, e.g. SVSettings.xml
		@param [in] pGetUserResponseFunction_p callback function pointer to allow user to react on program error and info messages
		@param [in] pProcessProgramMessageFunction_p callback function pointer for program status messages
		@return int error code, 0x00000000 - success, all other values represents errors (see smartVIS3D software manual)
		*/
        //SMARTVIS3D_MEAS_API int InitializeSmartVIS3DInterface(char* pSettingsFileName_p, GetUserResponseFunction pGetUserResponseFunction_p, ProcessProgramMessageFunction pProcessProgramMessageFunction_p);

        [DllImport("smartVis3DMeas.dll", EntryPoint = "InitializeSmartVIS3DInterface")]
        public static extern int InitializeSmartVIS3DInterface(string pSettingsFileName_p, IntPtr pGetUserResponseFunction_p, IntPtr pProcessProgramMessageFunction_p);

        /** @fn bool IsSmartVIS3DInterfaceInitialized();
        @brief gets the state of initialization
         @return true if library is initialized, else false
       */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsSmartVIS3DInterfaceInitialized")]
        public static extern bool IsSmartVIS3DInterfaceInitialized();

        /** @fn int DeinitializeSmartVIS3DInterface();
    @brief global deinitialization
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DeinitializeSmartVIS3DInterface")]
        public static extern int DeinitializeSmartVIS3DInterface();

        /** @fn int LockScanRangeMinimumPosition(double ScanRangeMinimumPosition_p)
    @brief sets and locks the scan range minimum position. if current scan device position is outside of the new range
           than the scan device will move to the new scan range minimum position
    @param [in] ScanRangeMinimumPosition_p required scan range minimum position [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "LockScanRangeMinimumPosition")]
        public static extern int LockScanRangeMinimumPosition(double ScanRangeMinimumPosition_p);

        /** @fn int LockScanRangeMaximumPosition(double ScanRangeMaximumPosition_p)
    @brief sets and locks the scan range maximum position. if current scan device position is outside of the new range
           than the scan device will move to the new scan range maximum position
    @param [in] ScanRangeMaximumPosition_p required scan range maximum position [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "LockScanRangeMaximumPosition")]
        public static extern int LockScanRangeMaximumPosition(double ScanRangeMaximumPosition_p);

        /** @fn int IsScanRangeMinimumPositionLocked(bool* pScanRangeMinimumPositionLockedStatus_p)
    @brief checks if scan range minimum position is locked
    @param [out] pScanRangeMinimumPositionLockedStatus_p pointer to a boolean variable in which the locked status of the scan range
                                                         minimum position will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsScanRangeMinimumPositionLocked")]
        public static extern int IsScanRangeMinimumPositionLocked(out bool pScanRangeMinimumPositionLockedStatus_p);

        /** @fn int IsScanRangeMaximumPositionLocked(bool* pScanRangeMaximumPositionLockedStatus_p)
    @brief checks if scan range maximum position is locked
    @param [out] pScanRangeMaximumPositionLockedStatus_p pointer to a boolean variable in which the locked status of the scan range
                                                         maximum position will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsScanRangeMaximumPositionLocked")]
        public static extern int IsScanRangeMaximumPositionLocked( out bool pScanRangeMaximumPositionLockedStatus_p);

        /** @fn int ReleaseScanRangeMinimumPositionLock()
    @brief releases the lock of the scan range minimum position
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "ReleaseScanRangeMinimumPositionLock")]
        public static extern int ReleaseScanRangeMinimumPositionLock();

        /** @fn int ReleaseScanRangeMaximumPositionLock( )
            @brief releases the lock of the scan range maximum position
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "ReleaseScanRangeMaximumPositionLock")]
        public static extern int ReleaseScanRangeMaximumPositionLock();

        /** @fn int ReleaseScanRangeLock()
    @brief releases the lock of the scan range minimum and maximum positions
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "ReleaseScanRangeLock")]
        public static extern int ReleaseScanRangeLock();


        /** @fn int GetScanRange(double* pScanRangeMinimumPosition_p, double* pScanRangeMaximumPosition_p)
    @brief access to the locked scan range, if not locked scan device full scan range will be returned
    @param [out] pScanRangeMinimumPosition_p pointer to a double variable in which the scan range minimum position will be stored [mm]
    @param [out] pScanRangeMaximumPosition_p pointer to a double variable in which the scan range maximum position will be stored [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanRange")]
        public static extern int GetScanRange(out double pScanRangeMinimumPosition_p,out double pScanRangeMaximumPosition_p);

        /** @fn int GetCompleteScanRange(double* pScanRangeMinimumPosition_p, double* pScanRangeMaximumPosition_p)
    @brief access to scan device full scan range
    @param [out] pScanRangeMinimumPosition_p pointer to a double variable in which the scan range minimum position will be stored [mm]
    @param [out] pScanRangeMaximumPosition_p pointer to a double variable in which the scan range maximum position will be stored [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCompleteScanRange")]
        public static extern int GetCompleteScanRange(out double pScanRangeMinimumPosition_p, out double pScanRangeMaximumPosition_p);

        /** @fn int GetScanPosition(double* pScanPosition_p)
    @brief gets the current position of the scan device
    @param [out] pScanPosition_p pointer to a double variable in which the scan device current position will be stored [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanPosition")]
        public static extern int GetScanPosition(out double pScanPosition_p);

        /** @fn int GetAxisLimits(const char* pAxisLabel_p, double* pAxisMinimum_p, double* pAxisMaximum_p)
    @brief get axis soft limits
    @param [in] pAxisLabel_p axis label, supported labels "X", "Y", "Z"
    @param [out] pAxisMinimum_p pointer to a double variable in which the axis minimum will be stored
    @param [out] pAxisMaximum_p pointer to a double variable in which the axis maximum will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetAxisLimits")]
        public static extern int GetAxisLimits( string pAxisLabel_p,out double pAxisMinimum_p, out double pAxisMaximum_p);


        /** @fn int MoveToScanPosition(double ScanPosition_p)
    @brief moves the scan device to scan position
    @param [in] ScanPosition_p destination of movement [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveToScanPosition")]
        public static extern int MoveToScanPosition(double ScanPosition_p);


        /** @fn int MoveToScanPositionWithPositionReachedNotification(double ScanPosition_p, PositioningStatusNotificationFunction pPositioningStatusNotification_p, void* pFunctionCallerInfo_p)
    @brief moves the scan device to scan position
    @param [in] ScanPosition_p destination of movement [mm]
    @param [in] pPositioningStatusNotification_p callback function used to notify the user in case the position is reached or the positioning command was aborted
    @param [in] pFunctionCallerInfo_p pointer to an optional function caller object, can be NULL
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveToScanPositionWithPositionReachedNotification")]
        public static extern int MoveToScanPositionWithPositionReachedNotification(double ScanPosition_p, IntPtr pPositioningStatusNotification_p, ref object pFunctionCallerInfo_p);


        /** @fn int MoveSoftwareControllableXAxisToPosition(double TargetXPosition_p)
    @brief moves the x positioning stage to the given position
    @param [in] TargetXPosition_p destination of movement [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveSoftwareControllableXAxisToPosition")]
        public static extern int MoveSoftwareControllableXAxisToPosition(double TargetXPosition_p);


        /** @fn int MoveSoftwareControllableYAxisToPosition(double TargetYPosition_p)
    @brief moves the y positioning stage to the given position
    @param [in] TargetYPosition_p destination of movement [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveSoftwareControllableYAxisToPosition")]
        public static extern int MoveSoftwareControllableYAxisToPosition(double TargetYPosition_p);

        /** @fn int MoveSoftwareControllableZAxisToPosition(double TargetZPosition_p)
    @brief moves the z positioning stage to the given position
    @param [in] TargetZPosition_p destination of movement [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveSoftwareControllableZAxisToPosition")]
        public static extern int MoveSoftwareControllableZAxisToPosition(double TargetZPosition_p);

        /** @fn int MoveSoftwareControllableXYAxisToPosition(double TargetXPosition_p, double TargetYPosition_p)
    @brief moves the x and y positioning stages to the given position
    @param [in] TargetXPosition_p destination of movement for the x axis [mm]
    @param [in] TargetYPosition_p destination of movement for the y axis [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveSoftwareControllableXYAxisToPosition")]
        public static extern int MoveSoftwareControllableXYAxisToPosition(double TargetXPosition_p, double TargetYPosition_p);

        /** @fn int MoveSoftwareControllableXZAxisToPosition(double TargetXPosition_p, double TargetZPosition_p)
    @brief moves the x and z positioning stages to the given position
    @param [in] TargetXPosition_p destination of movement for the x axis [mm]
    @param [in] TargetZPosition_p destination of movement for the Z axis [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveSoftwareControllableXZAxisToPosition")]
        public static extern int MoveSoftwareControllableXZAxisToPosition(double TargetXPosition_p, double TargetZPosition_p);

        /** @fn int MoveSoftwareControllableYZAxisToPosition(double TargetYPosition_p, double TargetZPosition_p)
    @brief moves the y and z positioning stages to the given position
    @param [in] TargetYPosition_p destination of movement for the y axis [mm]
    @param [in] TargetZPosition_p destination of movement for the Z axis [mm]
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveSoftwareControllableYZAxisToPosition")]
        public static extern int MoveSoftwareControllableYZAxisToPosition(double TargetYPosition_p, double TargetZPosition_p);

        /** @fn int CanceCurrentMovementCommand()
    @brief cancels any positioning command 
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "MoveSoftwareControllableYZAxisToPosition")]
        public static extern int CancelCurrentMovementCommand();


        /** @fn int IsPositioningAxisSoftwareControllable(const char* pAxisLabel_p, bool* pIsSoftwareControllable_p)
    @brief checks whether the required axis of the positioning device is controlled internally
    @param [in] pAxisLabel_p axis label, supported labels "X", "Y", "Z"
    @param [out] pIsSoftwareControllable_p pointer to a boolean variable in which the check result is stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsPositioningAxisSoftwareControllable")]
        public static extern int IsPositioningAxisSoftwareControllable(string pAxisLabel_p, out bool pIsSoftwareControllable_p);

        /** @fn int IsXPositioningAxisSoftwareControllable(bool* pIsSoftwareControllable_p)
    @brief checks whether the X axis of the positioning device is controlled internally
    @param [out] pIsSoftwareControllable_p pointer to a boolean variable in which the check result is stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsXPositioningAxisSoftwareControllable")]
        public static extern int IsXPositioningAxisSoftwareControllable(out bool pIsSoftwareControllable_p);



        /** @fn int GetMeasurementProceduresNumber(unsigned int* pMeasurementProceduresNumber_p)
    @brief gets number of measuring procedures (listed in the configuration file).
    A measuring procedure contains the essential configuration data (camera dataset indices,
    positioning dataset index, lens index, lighting setting, measuring parameter, template names)
    @param [out] pMeasurementProceduresNumber_p pointer to an unsigned integer variable in which the number of listed
                                                measuring procedure datasets in the configuration file will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementProceduresNumber")]
        public static extern int GetMeasurementProceduresNumber(out uint pMeasurementProceduresNumber_p);

        /** @fn int GetMeasurementProcedureID(unsigned int* pMeasurementProcedureID_p)
            @brief gets index of the currently selected measuring procedure (listed in the configuration file).
            A measuring procedure contains the essential configuration data (camera dataset indices,
            positioning dataset index, lens index, lighting setting, measuring parameter, template names)
            @param [out] pMeasurementProcedureID_p index of current measuring procedure dataset
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementProcedureID")]
        public static extern int GetMeasurementProcedureID(out uint pMeasurementProcedureID_p);

        /** @fn int GetMeasurementProcedureName(unsigned int MeasurementProcedureID_p, char* pMeasurementProcedureShortName_p, unsigned int* pMeasurementProcedureShortNameSize_p)
            @brief gets short designation of the indexed measuring procedure (listed in the configuration file).
                   A measuring procedure contains the essential configuration data (camera dataset indices, positioning dataset index, lens index,
                   lighting setting, measuring parameter, template names)
            @param [in] MeasurementProcedureID_p index of measuring procedure dataset
            @param [out] pMeasurementProcedureShortName_p pointer to a zero terminated String of length *pMeasurementProcedureShortNameSize_p
                                                          contains short designation of indexed measuring procedure
            @param [in, out] pMeasurementProcedureShortNameSize_p pointer to an unsigned integer variable in which the number of character (available space) 
                                                                  in pMeasurementProcedureShortName_p[] will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementProcedureName")]
        public static extern int GetMeasurementProcedureName(uint MeasurementProcedureID_p, out string pMeasurementProcedureShortName_p, out uint pMeasurementProcedureShortNameSize_p);


        /** @fn int SelectMeasurementProcedure(unsigned int MeasurementProcedureID_p)
    @brief selects a measuring procedure from the listing in the configuration file.
    A measuring procedure contains the essential configuration data (camera dataset indices,
    positioning dataset index, lens index, lighting setting, measuring parameter, template names)
    @param [in] MeasurementProcedureID_p index of measuring procedure dataset
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SelectMeasurementProcedure")]
        public static extern int SelectMeasurementProcedure(uint MeasurementProcedureID_p);

        /** @fn int AddMeasurementProcedure(const char* const pMeasurementProcedureName_p, int& rMeasurementProcedureID_p)
            @brief creates a new measuring procedure with currently active settings for camera, light, piezo and stage units.
            @param [in] pMeasurementProcedureName_p name of the new measuring procedure dataset
            @param [out] rMeasurementProcedureID_p internally used list index of the new measuring procedure dataset
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "AddMeasurementProcedure")]
        public static extern int AddMeasurementProcedure(string pMeasurementProcedureName_p, ref int rMeasurementProcedureID_p);

	/** @fn int GetSupportedObjectivesNumber(unsigned int* pSupportedObjectivesNumber_p)
		@brief gets the number of objective datasets stored in configuration file
		@param [out] pSupportedObjectivesNumber_p pointer to an unsigned integer variable in which the number of objective datasets 
												  available in the configuration file will be stored
		@return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
		*/
       [DllImport("smartVis3DMeas.dll", EntryPoint = "GetSupportedObjectivesNumber")]
        public static extern int GetSupportedObjectivesNumber(out uint pSupportedObjectivesNumber_p);

        /** @fn int SelectObjective(unsigned int ObjectiveID_p)
            @brief selects an objective from the objective table in the configuration file.
                   This function sets the configuration data (scale) and initiates position change of an available
                   objetive revolver to the position of the selected objective
            @param [in] ObjectiveID_p index of objective dataset
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SelectObjective")]
        public static extern int SelectObjective(uint ObjectiveID_p);



        /** @fn int GetObjectiveID(unsigned int* pObjectiveID_p)
    @brief get the consecutive id of the objective dataset stored in the configuration file for the selected objectives
    @param [out] pObjectiveID_p pointer to an unsigned integer variable in which the index of the selected objective dataset will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetObjectiveID")]
        public static extern int GetObjectiveID(out uint pObjectiveID_p);

        /** @fn int GetObjectiveName(unsigned int ObjectiveID_p, char* pObjectiveName_p, unsigned int* pObjectiveNameSize_p)
            @brief gets the objective lens name for the provided index idx
            @param [in] ObjectiveID_p index of the required lens dataset
            @param [out] pObjectiveName_p[pObjectiveNameSize_p] pointer to a zero terminated String of length (pObjectiveNameSize_p) max,
                                                                contains the name for the objective lens for the provided index idx
            @param [in, out] pObjectiveNameSize_p pointer to an unsigned integer variable in which the number of character (available space) in pObjectiveName_p[] will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetObjectiveName")]
        public static extern int GetObjectiveName(uint ObjectiveID_p, out string pObjectiveName_p, out uint pObjectiveNameSize_p);

        /** @fn int GetMetricResolution(double* pXMetricResolution_p, double* pYMetricResolution_p)
            @brief supplies the metric resolution according to the selected objective and camera (and binning/subsampling setting).
            @param [out] pXMetricResolution_p pointer to a double variable in which the lateral resolution in X direction [mm/Pixel] will be stored
            @param [out] pYMetricResolution_p pointer to a double variable in which the lateral resolution in Y direction [mm/Pixel] will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMetricResolution")]
        public static extern int GetMetricResolution(out double pXMetricResolution_p,out double pYMetricResolution_p);

        /** @fn int GetObjectiveSpecificMetricResolution(unsigned int ObjectiveID_p, double* pXMetricResolution_p, double* pYMetricResolution_p)
            @brief supplies the metric resolution according to the objective with id ObjectiveID_p and camera (and binning/subsampling setting).
            @param [in] ObjectiveID_p index of the required objective dataset
            @param [out] pXMetricResolution_p pointer to a double variable in which the lateral resolution in X direction [mm/Pixel] will be stored
            @param [out] pYMetricResolution_p pointer to a double variable in which the lateral resolution in Y direction [mm/Pixel] will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetObjectiveSpecificMetricResolution")]
        public static extern int GetObjectiveSpecificMetricResolution(uint ObjectiveID_p, out double pXMetricResolution_p, out double pYMetricResolution_p);

        /** @fn int GetLuminousIntensityValuesRange(unsigned int* pMinimumLuminousIntensity_p, unsigned int* pMaximumLuminousIntensity_p)
    @brief gets the available values range of the illumination unit
    @param [out] pMinimumLuminousIntensity_p pointer to an unsigned integer variable in which the minimum intensity value will be stored
    @param [out] pMaximumLuminousIntensity_p pointer to an unsigned integer variable in which the maximum intensity value will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetLuminousIntensityValuesRange")]
        public static extern int GetLuminousIntensityValuesRange(out uint pMinimumLuminousIntensity_p, out uint pMaximumLuminousIntensity_p);

        /** @fn int GetLuminousIntensity(unsigned int* pLuminousIntensity_p)
            @brief gets the current intensity value of illuminating unit
            @param [out] pLuminousIntensity_p pointer to an unsigned integer variable in which the currently set intensity of illuminating unit will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetLuminousIntensity")]
        public static extern int GetLuminousIntensity(out uint pLuminousIntensity_p);

        /** @fn int SetLuminousIntensity(unsigned int LuminousIntensity_p)
            @brief sets the intensity value of illuminating unit
            @param [in] LuminousIntensity_p, set value for illuminating unit [0...255]
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetLuminousIntensity")]
        public static extern int SetLuminousIntensity(uint LuminousIntensity_p);

        /** @fn int GetCameraGainValuesRange(unsigned int* pMinimumGain_p, unsigned int* pMaximumGain_p);
            @brief gets the range of camera gain control
            @param [out] pMinimumGain_p pointer to an unsigned integer variable in which the minimum gain value of camera will be stored
            @param [out] pMaximumGain_p pointer to an unsigned integer variable in which the maximum gain value of camera will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraGainValuesRange")]
        public static extern int GetCameraGainValuesRange(out uint pMinimumGain_p, out uint pMaximumGain_p);


        /** @fn int GetCameraGain(unsigned int* pGain_p)
    @brief gets the current gain value of camera (current selected camera dataset)
    @param [out] pGain_p pointer to an unsigned integer variable in which the current gain value of camera will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraGain")]
        public static extern int GetCameraGain(out uint pGain_p);

        /** @fn int SetCameraGain(unsigned int Gain_p)
            @brief sets the gain value of camera in all corresponding camera datasets (adressed in measuring procedure dataset)
            @param [in] Gain_p required gain value
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetCameraGain")]
        public static extern int SetCameraGain(uint Gain_p);

        /** @fn int GetCameraExposureValuesRange(unsigned int* pMinimumExposure_p, unsigned int* pMaximumExposure_p);
            @brief gets the range of camera exposure control
            @param [out] pMinimumExposure_p pointer to an unsigned integer variable in which the minimum exposure value [祍] of camera will be stored
            @param [out] pMaximumExposure_p pointer to an unsigned integer variable in which the maximum exposure value [祍] of camera will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraExposureValuesRange")]
        public static extern int GetCameraExposureValuesRange(out uint pMinimumExposure_p, out uint pMaximumExposure_p);

        /** @fn int GetCameraExposure(unsigned int* pExposure_p)
            @brief gets the current exposure value of camera (current selected camera dataset)
            @param [out] pExposure_p pointer to an unsigned integer variable in which the current exposure value of camera [祍] will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraExposure")]
        public static extern int GetCameraExposure(out uint pExposure_p);

        /** @fn int SetCameraExposure(unsigned int Exposure_p)
            @brief sets the exposure value of camera in all corresponding camera datasets (adressed in measuring procedure dataset)
            @param [in] Exposure_p required camera exposure value [祍]
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetCameraExposure")]
        public static extern int SetCameraExposure(uint Exposure_p);



        /** @fn int GetCameraFrameRateValuesRange(double* pMinimumFrameRate_p, double* pMaximumFrameRate_p);
    @brief gets the range of camera frame rate for the current camera settings (aoi, imaging mode, ...)
    @param [out] pMinimumFrameRate_p pointer to a double variable in which the minimum frame rate value of the camera will be stored
    @param [out] pMaximumFrameRate_p pointer to a double variable in which the maximum frame rate value of the camera will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraFrameRateValuesRange")]
        public static extern int GetCameraFrameRateValuesRange(out double pMinimumFrameRate_p, out double pMaximumFrameRate_p);

        /** @fn int SetMeasurementResultsFileName(const char* pMeasurementResultsFileName_p)
            @brief sets the filename of the output file with the measurement results, the extension must be ".sur"
            @param [in] pMeasurementResultsFileName_p fully qualified or relative file name to store the measurement results. If NULL is given, nothing is stored.
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetMeasurementResultsFileName")]
        public static extern int SetMeasurementResultsFileName(string pMeasurementResultsFileName_p);

        /** @fn int ExecuteMeasurementProcedure()
            @brief starts a scan through the given range and performs the white light interferometric calculation,
                   the result is saved in a file with the given file name. The function returns after start of the measurement.
                   Then the successful execution is signaled by the Event, given by SetResultReadyNotificationHandles.
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "ExecuteMeasurementProcedure")]
        public static extern int ExecuteMeasurementProcedure();

        /** @fn int CancelMeasurementProcedure()
            @brief cancels an active measurement
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "CancelMeasurementProcedure")]
        public static extern int CancelMeasurementProcedure();



        /** @fn int SetMeasurementProcedureSettings(bool UseGPUForComputation_p, bool EnableEPSIMode_p, bool EnablePSIMode_p, unsigned int ScanStepSizeMultiplier_p)
    @brief sets the options for the measurement mode
    @param [in] UseGPUForComputation_p if false computation is executed on CPU, if true on GPU (if license available)
    @param [in] EnableEPSIMode_p if true the extended mode for smooth surfaces is activated
    @param [in] EnablePSIMode_p if true the PSI mode is used, otherwise the default VSI mode is used (see manual for instructions)
    @param [in] ScanStepSizeMultiplier_p multiplier for the normal step height increment, allowed values are 1, 3, 5, 7, 9, 11, 13, 15, 17, default value is 1.
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetMeasurementProcedureSettings")]
        public static extern int SetMeasurementProcedureSettings(bool UseGPUForComputation_p, bool EnableEPSIMode_p, bool EnablePSIMode_p, uint ScanStepSizeMultiplier_p);


        /* ----------------------------------- MULTI-LAYER PROTOTYPE FUNCTIONS BEGIN ------------------------------------*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsMultiLayerMeasurementModeSupported")]
        public static extern int IsMultiLayerMeasurementModeSupported(out bool pMultiLayerMeasurementModeSupported_p);

        /** @fn int EnableMultiLayerMeasurementMode(int ExpectedLayerNumber_p, double* pExpectedLayerThicknessInfos_p)
            @brief enables multi layer measurement mode in smartVIS3D
            @param [in] ExpectedLayerNumber_p number of layers to be expected
            @param [in] pExpectedLayerThicknessInfos_p pointer to a double value which provide info about the minimum thickness of expected layers in 祄
                        if NULL than coherence length of the measurement system will be used
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnableMultiLayerMeasurementMode")]
        public static extern int EnableMultiLayerMeasurementMode(int ExpectedLayerNumber_p, ref double pExpectedLayerThicknessInfos_p);

        /** @fn int DisableMultiLayerMeasurementMode()
            @brief disables multi layer measurement mode in smartVIS3D
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DisableMultiLayerMeasurementMode")]
        public static extern int DisableMultiLayerMeasurementMode();

        /** @fn int GetMultiLayerMeasurementResultData(float* pHeightData_p, float* pQualityData_p, unsigned int* pDataFieldsWidth_p, unsigned int* pDataFieldsHeight_p, float* pHeightDataOffset_p)
            @brief supplies in multi layer measurement mode measured surface height data in pHeightData_p and the calculated quality in pQualityData_p.
                   The memory must be supplied by the caller. The array size is imageWidth * imageHeight * layerNumber (imageWidth and imageHeight may be accessed through GetCameraImageSize function)
                   layerNumber is set through EnableMultiLayerMeasurementMode function
                   Result data is stored lineary in the array in per layer basis
            @param [out] pHeightData_p pointer to a float data array for height data. Height is in mm.
            @param [out] pQualityData_p pointer to a float data array for quality data. Quality is in the range of 0 .. 1. If quality is not needed a NULL pointer can be supplied.
            @param [in, out] pDataFieldsWidth_p pointer to an unsigned integer variable in which the horizontal size of the measurement field in pixels,
                             equals the column count of the image, will be stored
            @param [in, out] pDataFieldsHeight_p pointer to an unsigned integer variable in which the vertical size of the measurement field in pixels,
                             equals the row count of the image, will be stored
            @param [out] pHeightDataOffset_p pointer to an floating point variable in which the common offset to height data for absolute height values will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMultiLayerMeasurementResultData")]
        public static extern int GetMultiLayerMeasurementResultData(out float []pHeightData_p, out float []pQualityData_p, out uint []pDataFieldsWidth_p, out uint []pDataFieldsHeight_p, out float []pHeightDataOffset_p);

        /** @fn int GetMultiLayerExtendedMeasurementResultData( float* pHeightData_p, float* pQuality, unsigned char* pSurfaceIntensityData_p, unsigned int* pColumns, unsigned int* pRows, float* pHeightOffset )
            @brief supplies the measured data in pHeightData_p, the calculated quality in pQuality, and the surface reflectance map in pSurfaceIntensityData_p.
                   The memory must be supplied by the caller. The array size is imageWidth * imageHeight * LayerNumber_p (imageWidth and imageHeight may be accessed through GetCameraImageSize function)
                   layerNumber is set through EnableMultiLayerMeasurementMode function
                   Result data is stored lineary in the array in per layer basis
            @param [out] pHeightData_p pointer to a float data array for height data. Height is in mm.
            @param [out] pQualityData_p pointer to a float data array for quality data. Quality is in the range of 0 .. 1. If quality is not needed a NULL pointer can be supplied.
            @param [out] pSurfaceIntensityData_p pointer to an unsigned char data array for surface reflectance data. Grayvalues are in the range of 0 .. 255.
                                                 if gray values are not needed a NULL pointer can be supplied (or use function GetMultiLayerMeasurementResultData).
            @param [in, out] pDataFieldsWidth_p pointer to an unsigned integer variable in which the horizontal size of the measurement field in pixels,
                             equals the column count of the image, will be stored
            @param [in, out] pDataFieldsHeight_p pointer to an unsigned integer variable in which the vertical size of the measurement field in pixels,
                             equals the row count of the image, will be stored
            @param [out] pHeightDataOffset_p pointer to an floating point variable in which the common offset to height data for absolute height values will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMultiLayerExtendedMeasurementResultData")]
        public static extern int GetMultiLayerExtendedMeasurementResultData(out float []floatpHeightData_p, out float [] pQualityData_p, out char [] pSurfaceIntensityData_p, out uint []pDataFieldsWidth_p,
                                                                           out uint []pDataFieldsHeight_p, out float []pHeightDataOffset_p);






        /** @fn int IsVSIModeEnabled(bool* pVSIModeEnabledStatus_p)
            @brief checks if rough measurement mode is enabled
            @param [out] pVSIModeEnabledStatus_p pointer to a boolean variable in which the status of rough measurement mode is stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsVSIModeEnabled")]
        public static extern int IsVSIModeEnabled(out bool  pVSIModeEnabledStatus_p);

        /** @fn int IsEPSIModeEnabled(bool* pEPSIModeEnabledStatus_p)
            @brief checks if rough measurement mode is enabled
            @param [out] pEPSIModeEnabledStatus_p pointer to a boolean variable in which the status of epsi measurement mode is stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsEPSIModeEnabled")]
        public static extern int IsEPSIModeEnabled(out bool pEPSIModeEnabledStatus_p);

        /** @fn int IsPSIModeEnabled(bool* pPSIModeEnabledStatus_p)
            @brief checks if rough measurement mode is enabled
            @param [out] pPSIModeEnabledStatus_p pointer to a boolean variable in which the status of psi measurement mode is stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsPSIModeEnabled")]
        public static extern int IsPSIModeEnabled(out bool pPSIModeEnabledStatus_p);

        /** @fn int GetSupportedScanStepSizeMultiplierList(unsigned int* pScanStepSizeMultiplierList_p, unsigned int* pScanStepSizeMultiplierListSize_p)
            @brief this function provides the list of supported multipliers for the scan device
                   size list can be retrieved using this functon with ppScanStepSizeMultiplierList_p parameter equal NULL
            @param [out] pScanStepSizeMultiplierList_p pointer to an initialized unsigned integer array in which the multiplier list data will be stored
            @param [out] pScanStepSizeMultiplierListSize_p pointer to an unsigned integer value in which the number of multipliers stored in pScanStepSizeMultiplierList_p is provided
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetSupportedScanStepSizeMultiplierList")]
        public static extern int GetSupportedScanStepSizeMultiplierList(out uint [] pScanStepSizeMultiplierList_p, out uint pScanStepSizeMultiplierListSize_p);

        /** @fn int GetScanStepSizeMultiplierID(unsigned int* pScanStepSizeMultiplierID_p)
            @brief this function provides the id of the currently used multiplier for the base step size of the scan device
                   id is in range [0 ... pScanStepSizeMultiplierListSize_p - 1]
                   pScanStepSizeMultiplierListSize_p can be retrieved using GetSupportedScanStepSizeMultiplierList function
            @param [out] pScanStepSizeMultiplierID_p pointer to unsigned integer value in which the id will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanStepSizeMultiplierID")]
        public static extern int GetScanStepSizeMultiplierID(out uint pScanStepSizeMultiplierID_p);

        /** @fn int SetScanStepSizeMultiplierID(unsigned int ScanStepSizeMultiplierID_p)
            @brief this function can be used to set the multiplier for the base step size of the scan device
                   the id must be in range [0 ... pScanStepSizeMultiplierListSize_p - 1],
                   pScanStepSizeMultiplierListSize_p can be retrieved using GetSupportedScanStepSizeMultiplierList function
            @param [in] ScanStepSizeMultiplierID_p id of the zpos step size multiplier which should be used
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetScanStepSizeMultiplierID")]
        public static extern int SetScanStepSizeMultiplierID(uint ScanStepSizeMultiplierID_p);

        /** @fn int GetScanBaseStepSize(double* pScanBaseStepSize_p)
            @brief this function can be used to get the base step size of the scan device [nm]
            @param [out] pScanBaseStepSize_p pointer to a double value in which the step size is stored (provided step size is in nanometers)
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanBaseStepSize")]
        public static extern int GetScanBaseStepSize(out double pScanBaseStepSize_p);

        /** @fn int GetScanStepSize(double* pScanStepSize_p)
            @brief this function can be used to get the used step size in nm
            @param [out] pScanStepSize_p pointer to the variable in which the step size is stored (provided step size is in nanometers)
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanStepSize")]
        public static extern int GetScanStepSize(out double pScanStepSize_p);

        /** @fn int GetScanTriggerRate(double* pScanTriggerRate_p)
            @brief gets the current trigger rate value for scan process (in current selected measuring procedure dataset)
            @param [out] pScanTriggerRate_p pointer to a double variable in which the current trigger rate value will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanTriggerRate")]
        public static extern int GetScanTriggerRate(out double pScanTriggerRate_p);

        /** @fn int SetScanTriggerRate(double ScanTriggerRate_p)
            @brief sets the trigger rate value for scan process (in current selected measuring procedure dataset)
            @param [in] ScanTriggerRate_p required trigger rate value
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetScanTriggerRate")]
        public static extern int SetScanTriggerRate(double ScanTriggerRate_p);

        /** @fn int SetResultReadyNotificationHandles(HANDLE hResultReadyEvent_p, HANDLE hErrorEvent_p)
            @brief sets event handle for event notification if the measurement result is available for supply and the event handle for a aborted measurement
            The event handles are stored in the API and after finishing measurement, if a measurement result is available, the the
            event is signaled by the API execution thread. If an error occurred or the measurement was canceled, the hErrorEvent is set.
            @param [in] hResultReadyEvent_p Windows event handle for notification of the calling process (thread)
            @param [in] hErrorEvent_p Windows event handle for notification of an error during measurement (measurement is canceled), can be set to NULL if not used.
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetResultReadyNotificationHandles")]
        public static extern int SetResultReadyNotificationHandles(IntPtr hResultReadyEvent_p, IntPtr hErrorEvent_p);

        /** @fn int GetMeasurementResultData(float* pHeightData_p, float* pQualityData_p, unsigned int* pDataFieldsWidth_p, unsigned int* pDataFieldsHeight_p, float* pHeightDataOffset_p)
            @brief supplies the measured data in pHeightData_p and the calculated quality in pQualityData_p.
            The memory must be supplied by the caller. The arrays are linear with size columns * rows (equals to image size) and row ordered.
            The actual values are supplied in columns and rows.
            @param [out] pHeightData_p pointer to a float data array for height data. Height is in mm.
            @param [out] pQualityData_p pointer to a float data array for quality data. Quality is in the range of 0 .. 1. If quality is not needed a NULL pointer can be supplied.
            @param [in, out] pDataFieldsWidth_p pointer to an unsigned integer variable in which the horizontal size of the measurement field in pixels, 
                             equals the column count of the image, will be stored
            @param [in, out] pDataFieldsHeight_p pointer to an unsigned integer variable in which the vertical size of the measurement field in pixels, 
                             equals the row count of the image, will be stored
            @param [out] pHeightDataOffset_p pointer to an floating point variable in which the common offset to height data for absolute height values will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementResultData")]
        public static extern int GetMeasurementResultData(out float[] pHeightData_p, out float[] pQualityData_p, out uint[] pDataFieldsWidth_p, out uint[] pDataFieldsHeight_p, out float[] pHeightDataOffset_p);

        /** @fn int GetExtendedMeasurementResultData( float* pHeightData_p, float* pQuality, unsigned char* pSurfaceIntensityData_p, unsigned int* pColumns, unsigned int* pRows, float* pHeightOffset )
            @brief supplies the measured data in pHeightData_p, the calculated quality in pQualityData_p, and the surface reflectance map in pSurfaceIntensityData_p.
            The memory must be supplied by the caller. The arrays are linear with size columns * rows (equals to image size) and row ordered.
            The actual values are supplied in columns and rows.
            @param [out] pHeightData_p pointer to a float data array for height data. Height is in mm.
            @param [out] pQualityData_p pointer to a float data array for quality data. Quality is in the range of 0 .. 1. If quality is not needed a NULL pointer can be supplied.
            @param [out] pSurfaceIntensityData_p pointer to an unsigned char data array for surface reflectance data. Grayvalues are in the range of 0 .. 255. 
                                                 if gray values are not needed a NULL pointer can be supplied (or use function GetMeasurementResultData).
            @param [in, out] pDataFieldsWidth_p pointer to an unsigned integer variable in which the horizontal size of the measurement field in pixels, 
                             equals the column count of the image, will be stored
            @param [in, out] pDataFieldsHeight_p pointer to an unsigned integer variable in which the vertical size of the measurement field in pixels, 
                             equals the row count of the image, will be stored
            @param [out] pHeightDataOffset_p pointer to an floating point variable in which the common offset to height data for absolute height values will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetExtendedMeasurementResultData")]
        public static extern int GetExtendedMeasurementResultData(out float [] pHeightData_p, out float[] pQualityData_p, out byte [] pSurfaceIntensityData_p, out uint pDataFieldsWidth_p,
                                                                 out uint  pDataFieldsHeight_p, out float pHeightDataOffset_p);

        /** @fn int ChangeLiveImagingStatus(bool LiveImagingStatus_p)
            @brief enable / disable live imaging. affects also return to live visualization process after measurement
                   live imaging should be disabled for automatic measurement procedures because of camera internal switching time
            @param [in] LiveImagingStatus_p if true enables live imaging otherwise no images are acquired in measurement pauses
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "ChangeLiveImagingStatus")]
        public static extern int ChangeLiveImagingStatus(bool LiveImagingStatus_p);

        /** @fn int SetFrameReadyNotificationHandle(HANDLE hFrameReadyEvent_p)
            @brief sets event handle for event notification if a new image is available for visualisation
                   if a new live image is available the event is set to signaled state.
            @param [in] hFrameReadyEvent_p Windows event handle for notification of the calling process (thread)
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetFrameReadyNotificationHandle")]
        public static extern int SetFrameReadyNotificationHandle(IntPtr hFrameReadyEvent_p);

        /** @fn int SetFrameReadyCallbackFunctionData(callback_function pFunction_p, void* pFunctionArgument_p)
            @brief sets callback function if a new image is available for visualisation
                   if an image is available pFunction_p is called with the argument pFunctionArgument_p.
            @param [in] pFunction_p function pointer
            @param [in] pFunctionArgument_p pointer to the function argument
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetFrameReadyCallbackFunctionData")]
         public static extern int SetFrameReadyCallbackFunctionData(IntPtr pFunction_p, ref object pFunctionArgument_p);

        /** @fn int GetCameraAOISettingLimits(unsigned int* pAOIXOffsetIncrement_p, unsigned int* pAOIYOffsetIncrement_p, unsigned int* pAOIWidthIncrement_p, unsigned int* pAOIHeightIncrement_p,
                                              unsigned int* pAOIWidthMinimum_p, unsigned int* pAOIWidthMaximum_p, unsigned int* pAOIHeightMinimum_p, unsigned int* pAOIHeightMaximum_p);
            @brief allows access to the camera aoi setting limits in currently used imaging mode
            @param [out] pAOIXOffsetIncrement_p pointer to an unsigned integer variable in which the increment value of camera aoi x start offset should be stored
            @param [out] pAOIYOffsetIncrement_p pointer to an unsigned integer variable in which the increment value of camera aoi y start offset should be stored
            @param [out] pAOIWidthIncrement_p pointer to an unsigned integer variable in which the increment value of camera aoi width setting should be stored
            @param [out] pAOIHeightIncrement_p pointer to an unsigned integer variable in which the increment value of camera aoi height setting should be stored
            @param [out] pAOIWidthMinimum_p pointer to an unsigned integer variable in which the minimum number of columns of the camera aoi should be stored
            @param [out] pAOIWidthMaximum_p pointer to an unsigned integer variable in which the maximum number of columns of the camera aoi should be stored
            @param [out] pAOIHeightMinimum_p pointer to an unsigned integer variable in which the minimum number of rows (lines) of the camera aoi should be stored
            @param [out] pAOIHeightMaximum_p pointer to an unsigned integer variable in which the maximum number of rows (lines) of the camera aoi should be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraAOISettingLimits")]
        public static extern int GetCameraAOISettingLimits(out uint pAOIXOffsetIncrement_p, out uint pAOIYOffsetIncrement_p, out uint pAOIWidthIncrement_p, out uint pAOIHeightIncrement_p,
                                                          out uint pAOIWidthMinimum_p, out uint pAOIWidthMaximum_p, out uint pAOIHeightMinimum_p, out uint pAOIHeightMaximum_p);

        /** @fn int GetCameraAOI(unsigned int* pAOIXOffset_p, unsigned int* pAOIYOffset_p, unsigned int* pAOIWidth_p, unsigned int* pAOIHeight_p);
            @brief allows access to currently used camera aoi settings
            @param [out] pAOIXOffset_p pointer to an unsigned integer variable in which the x start offset of the aoi should be stored
            @param [out] pAOIYOffset_p pointer to an unsigned integer variable in which the y start offset of the aoi should be stored
            @param [out] pAOIWidth_p pointer to an unsigned integer variable in which the width of the aoi should be stored
            @param [out] pAOIHeight_p pointer to an unsigned integer variable in which the height of the aoi should be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraAOI")]
        public static extern int GetCameraAOI(out uint pAOIXOffset_p, out uint pAOIYOffset_p, out uint pAOIWidth_p, out uint pAOIHeight_p);


        /** @fn int SetCameraAOI(unsigned int AOIXOffset_p, unsigned int AOIYOffset_p, unsigned int AOIWidth_p, unsigned int AOIHeight_p)
            @brief sets the aoi of the camera in all corresponding camera datasets (adressed in measuring procedure dataset)
                   with automatic maximization of the scan trigger rate (used in the measurement scan, can result in a reduction of the exposure time)
            @param [in] AOIXOffset_p required x start offset of the aoi
            @param [in] AOIYOffset_p required y start offset of the aoi
            @param [in] AOIWidth_p required width of the aoi
            @param [in] AOIHeight_p required height of the aoi
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetCameraAOI")]
        public static extern int SetCameraAOI(uint AOIXOffset_p, uint AOIYOffset_p, uint AOIWidth_p, uint AOIHeight_p);

        /** fn int GetCameraImageFullSize(unsigned int* pMaxImageWidth_p, unsigned int* pMaxImageHeight_p)
            @brief determines the size of the camera image which is needed for memory allocation by client
            @param [out] pMaxImageWidth_p pointer to an unsigned integer variable in which the maximum number of columns of the camera sensor (full size image) will be stored
            @param [out] pMaxImageHeight_p pointer to an unsigned integer variable in which the maximum number of rows (lines) of the camera sensor (full size image) will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraImageFullSize")]
        public static extern int GetCameraImageFullSize(out uint pMaxImageWidth_p, out uint pMaxImageHeight_p);

        /** fn int GetCameraImageSize(unsigned int* pImageWidth_p, unsigned int* pImageHeight_p)
            @brief determines the size of the camera image which is needed for memory allocation by client
            @param [out] pImageWidth_p pointer to an unsigned integer variable in which the number of columns of the camera image will be stored
            @param [out] pImageHeight_p pointer to an unsigned integer variable in which the number of rows (lines) of the camera image will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraImageSize")]
        public static extern int GetCameraImageSize(out uint pImageWidth_p, out uint pImageHeight_p);

        /** fn int GetCameraImageColorDepthSettings(unsigned int* pImageColorDepth_p, unsigned int* p16BitTo8BitConversionRightShift_p)
            @brief determines the byte per pixel color depth info of the camera image which is needed for memory allocation by client
            @param [out] pImageColorDepth_p pointer to an unsigned integer variable in which the resolution (byte per pixel) of the camera image [1, 2] will be stored
            @param [out] p16BitTo8BitConversionRightShift_p pointer to an unsigned integer variable in which the suitable value for a right shift of 
                                                            16-bit image data before converting to 8-bit [0...8] will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraImageColorDepthSettings")]
        public static extern int GetCameraImageColorDepthSettings(out uint pImageColorDepth_p, out uint p16BitTo8BitConversionRightShift_p);

        /** fn int GetCameraImageData(void* pImageData_p, unsigned int ImageDataSize_p)
            @brief captured image data is copied to memory location pointed by pImageData_p
            @param [out] pImageData_p pointer at memory for a single image, to be supplied by caller
                                      image is delivered in a single continuous memory array, line by line
            @param [in] ImageDataSize_p the size of pImageData_p in byte
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraImageData")]
        public static extern int GetCameraImageData(out byte [] pImageData_p, out uint ImageDataSize_p);

        /** fn bool ClearFrameBuffer()
            @brief clears frame buffer for access to the newest shot
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "ClearFrameBuffer")]
        public static extern int ClearFrameBuffer();

        /** @fn int EnablePhaseUnwrappingAlgorithmUsage()
            @brief enables phase unwrapping algorithm usage which is used to optimize the measurement results if phase information is used in processing
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnablePhaseUnwrappingAlgorithmUsage")]
        public static extern int EnablePhaseUnwrappingAlgorithmUsage();

        /** @fn int DisablePhaseUnwrappingAlgorithmUsage()
            @brief disables phase unwrapping algorithm usage which is used to optimize the measurement results if phase information is used in processing
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DisablePhaseUnwrappingAlgorithmUsage")]
        public static extern int DisablePhaseUnwrappingAlgorithmUsage();

        /** @fn int GetPhaseUnwrappingAlgorithmStatus(bool* pPhaseUnwrappingAlgorithmStatus_p)
            @brief retrieves status of phase unwrapping algorithm usage
            @param [out] pPhaseUnwrappingAlgorithmStatus_p pointer to a boolean variable in which the status will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetPhaseUnwrappingAlgorithmStatus")]
        public static extern int GetPhaseUnwrappingAlgorithmStatus(out bool pPhaseUnwrappingAlgorithmStatus_p);

        /** @fn int IsFastProcessingDeviceEnabled(bool* pFastProcessingDeviceEnabled_p)
            @brief retrieves information if fast processing device (GPU) was detected in the PC
            @param [out] pFastProcessingDeviceEnabled_p pointer to a boolean variable in which the status will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsFastProcessingDeviceEnabled")]
        public static extern int IsFastProcessingDeviceEnabled(out bool pFastProcessingDeviceEnabled_p);


	/** @fn int EnableMultipleMeasurementsModeStatus(MultipleMeasurementsModeExecutedMeasurementsNumberCallbackFunction pMultipleMeasurementsModeExecutedMeasurementsNumberCallbackFunction_p,
													 void* pFunctionCallerInfo_p)
		@brief enable multiple measurement mode
		@param [in] pMultipleMeasurementsModeExecutedMeasurementsNumberCallbackFunction_p function pointer used to provide the calling application with the info 
																						  of the number of executed measurements
		@param [in] pFunctionCallerInfo_p pointer to an optional function caller object, can be NULL
		@return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnableMultipleMeasurementsModeStatus")]
        public static extern int EnableMultipleMeasurementsModeStatus(IntPtr pMultipleMeasurementsModeExecutedMeasurementsNumberCallbackFunction_p,ref object pFunctionCallerInfo_p);

        /** @fn int DisableMultipleMeasurementsModeStatus()
            @brief disable multiple measurement mode
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DisableMultipleMeasurementsModeStatus")]
        public static extern int DisableMultipleMeasurementsModeStatus();

        /** @fn int GetMultipleMeasurementsModeStatus(bool* pMultipleMeasurementsModeStatus_p)
            @brief get the status of multiple measurement mode, true - on, false - off
            @param [out] pMultipleMeasurementsModeStatus_p pointer to a boolean value in which the status of the multiple measurements mode will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMultipleMeasurementsModeStatus")]
        public static extern int GetMultipleMeasurementsModeStatus(out bool pMultipleMeasurementsModeStatus_p);

        /** @fn int SetMultipleMeasurementsModeAutocompletionMeasurementsNumber(int MeasurementsNumber_p)
            @brief set measurements number for automatic finish of multiple measurements mode, if MeasurementsNumber_p is 0 then multiple measurement 
                   mode must be finished manually using FinishMultipleMeasurementMode function
            @param [in] MeasurementsNumber_p measurements number after which the multiple measurement mode will be automatically finished
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetMultipleMeasurementsModeAutocompletionMeasurementsNumber")]
        public static extern int SetMultipleMeasurementsModeAutocompletionMeasurementsNumber(int MeasurementsNumber_p);

        /** @fn int GetMultipleMeasurementsModeAutocompletionMeasurementsNumber(int* pMeasurementsNumber_p)
            @brief get the number of measurements used for autocompletion of the multiple measurements mode, if *pMeasurementsNumber_p is 0 autocompletion is disabled 
                   and the multiple measurements mode must be finished manually using FinishMultipleMeasurementMode function
            @param [out] pMeasurementsNumber_p pointer to a integer value in which the number of measurements used for autocompletion of the multiple measurements mode will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetMultipleMeasurementsModeAutocompletionMeasurementsNumber")]
        public static extern int GetMultipleMeasurementsModeAutocompletionMeasurementsNumber(out int pMeasurementsNumber_p);

        /** @fn int FinishMultipleMeasurementMode()
            @brief notifies the measurement control to finish the multiple measurement mode and to export all measurement results
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "FinishMultipleMeasurementMode")]
        public static extern int FinishMultipleMeasurementMode();


        /** @fn int SetLateralCalibrationScaleValue(double LateralCalibrationScaleValue_p)
    @brief this function allows to manually set the objectiv specific metric resolution of a pixel for currently used objective
    @param [in] LateralCalibrationScaleValue_p metric resolution of a pixel in mm
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetLateralCalibrationScaleValue")]
        public static extern int SetLateralCalibrationScaleValue(double LateralCalibrationScaleValue_p);

        /** @fn int SetObjectiveSpecificLateralCalibrationScaleValue(unsigned int ObjectiveID_p, double LateralCalibrationScaleValue_p)
            @brief this function allows to manually set the objectiv specific metric resolution of a pixel for the objective with id ObjectiveID_p
            @param [in] ObjectiveID_p index of the required objectiv dataset
            @param [in] LateralCalibrationScaleValue_p metric resolution of a pixel in mm
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetObjectiveSpecificLateralCalibrationScaleValue")]
        public static extern int SetObjectiveSpecificLateralCalibrationScaleValue(uint ObjectiveID_p, double LateralCalibrationScaleValue_p);

        /** @fn int DeinitializeLateralResolutionCalibration()
    @brief deinitialization of internal status of the lateral resolution calibration procedure
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DeinitializeLateralResolutionCalibration")]
        public static extern int DeinitializeLateralResolutionCalibration();


        /** @fn int GetLateralResolutionCalibrationMarkerPositions(double* pMarkerXPositions_p, double* pMarkerYPositions_p, int* pMarkerNumber_p)
    @brief provides access to at each lateral position computed marker positions. set pMarkerXPositions_p and pMarkerYPositions_p to NULL to get the marker positions number
    @param [out] pMarkerXPositions_p pointer to an array in which the x coordinates of the marker positions will be stored
    @param [out] pMarkerYPositions_p pointer to an array in which the y coordinates of the marker positions will be stored
    @param [out] pMarkerNumber_p pointer to an integer value in which the number of computed marker positions will be stored
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetLateralResolutionCalibrationMarkerPositions")]
        public static extern int GetLateralResolutionCalibrationMarkerPositions(out double pMarkerXPositions_p, out double pMarkerYPositions_p, out int pMarkerNumber_p);

        /** @fn int IsFlatnessCalibrationProcessAllowed(bool* pFlatnessCalibrationProcessAllowed_p)
            @brief checks if lateral resolution scale value is set either by user using SetLateralCalibrationScaleValue function or by the lateral resolution calibration process
            @param [out] pFlatnessCalibrationProcessAllowed_p pointer to a boolean variable in which the status of the lateral resolution scale value is stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsFlatnessCalibrationProcessAllowed")]
        public static extern int IsFlatnessCalibrationProcessAllowed(out bool  pFlatnessCalibrationProcessAllowed_p);



        /** @fn int DeinitializFlatnessCalibration()
    @brief deinitialization of internal status of the flatness calibration procedure
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DeinitializFlatnessCalibration")]
        public static extern int DeinitializFlatnessCalibration();

        /** @fn int GetErrorCodeDescription(unsigned int ErrorCode_p, char* pErrorCodeDescription_p, unsigned int* pErrorCodeDescriptionSize_p)
    @brief gets the error message which corresponds to an error code received from a sdk function
           error code description text length (zero terminated) can be retrieved using this function with ppErrorCodeDescription_p = NULL
    @param [in] ErrorCode_p from a sdk function received error code
    @param [out] pErrorCodeDescription_p pointer to an intialized memory area in which the error description will be stored
    @param [in / out] pErrorCodeDescriptionSize_p pointer to an unsigned integer variable in which the allocated memory size for the error description in pErrorCodeDescription_p
                                                  will be stored
    @return int error code, 0x00000000 - success, all other values represents errors
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetErrorCodeDescription")]
        public static extern int GetErrorCodeDescription(uint ErrorCode_p, out string pErrorCodeDescription_p, out uint pErrorCodeDescriptionSize_p);


        /** @fn void EnableSDKParametersDebugOutput(bool EnableDebugOutput_p)
            @brief helper function used to enable debug output of used parameters in sdk functions
            @param [in] EnableDebugOutput_p enable / disable debug output
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnableSDKParametersDebugOutput")]
        public static extern void EnableSDKParametersDebugOutput(bool EnableDebugOutput_p);

        /** @fn bool IsSDKParametersDebugOutputEnabled()
            @brief helper function used to check if sdk function parameters debug output is enabled
            @return bool status of debug output enabled flag
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsSDKParametersDebugOutputEnabled")]
        public static extern bool IsSDKParametersDebugOutputEnabled();



        /** @fn int GetCalibrationFilesRootPath(char* pCalibrationFilesRootPath_p, unsigned int* pCalibrationFilesRootPathSize_p)
    @brief gets the root path where calibration process related files are stored
           length of the root path string can be retrieved from pCalibrationFilesRootPathSize_p variable when pCalibrationFilesRootPath_p is set to NULL
    @param [out] pCalibrationFilesRootPath_p NULL pointer to get calibration files root path size in pCalibrationFilesRootPathSize_p variable
                                             pointer to initialized memory area of size stored in pCalibrationFilesRootPathSize_p
    @param [in / out] pCalibrationFilesRootPathSize_p pointer to an unsigned integer variable in which the required memory size for the calibration files root path will be stored
    @return int error code, 0x00000000 - success, all other values represents errors
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetCalibrationFilesRootPath")]
        public static extern int GetCalibrationFilesRootPath(out string pCalibrationFilesRootPath_p, out uint pCalibrationFilesRootPathSize_p);

        /** @fn int EnableImageStackSaving(bool ImageStackSavingEnabled_p)
            @brief enables / disabled persistent saving of the camera images to hdd during measurement procedure
            @param [in] ImageStackSavingEnabled_p boolean variable used to change the save image stack flag status
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnableImageStackSaving")]
        public static extern int EnableImageStackSaving(bool ImageStackSavingEnabled_p);

        /** @fn int CheckImageStackSavingStatus(bool* pImageStackSavingEnabled_p)
            @brief checks the status of the save image stack flag
            @param [out] pImageStackSavingEnabled_p pointer to an initialized boolean variable in which the save image stack flag status is stored
            @return int error code, 0x00000000 - success, all other values represents errors
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "CheckImageStackSavingStatus")]
        public static extern int CheckImageStackSavingStatus(ref bool pImageStackSavingEnabled_p);

        /** @fn int SetImageStackTargetPath(const char* const pImageStackTargetPath_p)
            @brief changes target path for image stack saving
            @param [in] pImageStackTargetPath_p target path
            @return int error code, 0x00000000 - success, all other values represents errors
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SetImageStackTargetPath")]
        public static extern int SetImageStackTargetPath(string pImageStackTargetPath_p);



        /** @fn int EnableHighPrecisionMode(int pProcessingStepsNumber_p)
    @brief enables high precision mode which can be used to reduce noise influence on the measurement results
    @param pProcessingStepsNumber_p number of used processing steps, range [0...10], higher values results in smoother results at the cost of larger runtimes
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnableHighPrecisionMode")]
        public static extern int EnableHighPrecisionMode(int pProcessingStepsNumber_p);

        /** @fn int DisableHighPrecisionMode()
            @brief disables high precision mode which can be used to reduce noise influence on the measurement results
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DisableHighPrecisionMode")]
        public static extern int DisableHighPrecisionMode();

        /** @fn int GetHighPrecisionModeStatus(bool* pEnabledStatus_p, int* pProcessingStepsNumber_p)
            @brief retrieves high precision mode status and the number of used processing steps
            @param pEnabledStatus_p pointer to a boolean variable in which the status will be stored
            @param pProcessingStepsNumber_p pointer to an integer variable in which the number of used processing steps in high precision mode will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetHighPrecisionModeStatus")]
        public static extern int GetHighPrecisionModeStatus(out bool pEnabledStatus_p, out int pProcessingStepsNumber_p);

        /** @fn int GetHighPrecisionModeProcessingStepsRange(int* pProcessingStepMinimum_p, int* pProcessingStepMaximum_p);
            @brief retrieves high precision mode processing step range
            @param pProcessingStepMinimum_p Pointer to an integer variable in which the minimum possible number of processing steps in high-precision mode is stored
            @param pProcessingStepMaximum_p Pointer to an integer variable in which the maximum possible number of processing steps in high-precision mode is stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetHighPrecisionModeProcessingStepsRange")]
        public static extern int GetHighPrecisionModeProcessingStepsRange(out int pProcessingStepMinimum_p, out int pProcessingStepMaximum_p);




        /** @fn int EnableOversamplingMode(unsigned int ScanStepSizeComputationFactor_p)
    @brief enables oversampling measurement / processing mode, instead of Multiplier * BaseScaStepSize the provided scan step size computation factor is used to compute
           the used scan step size as follows: Wavelength * ObjectiveSpecificNAFactor / (8.0 * ScanStepSizeComputationFactor)
           provided scan step size will be automatically recomputed on objective change
    @param [in] ScanStepSizeComputationFactor_p scan step size compuation factor which is used to compute the scan step size used for measurement
                supported values are 2 - 16
    @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnableOversamplingMode")]
        public static extern int EnableOversamplingMode(ref uint ScanStepSizeComputationFactor_p);

        /** @fn int DisableOversamplingMode()
            @brief disables oversampling measurement / processing mode
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "DisableOversamplingMode")]
        public static extern int DisableOversamplingMode();

        /** @fn int GetOversamplingModeStatus(double& rScanStepSize_p, int& rScanStepSizeComputationFactor_p, bool& rIsOversamplingModeEnabled_p)
            @brief allows access to the enabled / disabled status of the oversampling measurement / processing mode, to the scan step size computation factor and to the used scan step size as well
            @param [out] rScanStepSize_p reference to a double variable in which the used scan step size of the oversampling mode will be stored
            @param [out] rScanStepSizeComputationFactor_p reference to an integer variable in which the used oversampling mode scan step size computation factor will be stored
            @param [out] rIsOversamplingModeEnabled_p reference to a boolean variable in which the enabld (true) / disabled (false) status of the oversampling mode will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetOversamplingModeStatus")]
        public static extern int GetOversamplingModeStatus(out double rScanStepSize_p, out int rScanStepSizeComputationFactor_p,out bool rIsOversamplingModeEnabled_p);

        /** @fn int GetOversamplingModeScanStepSizeComputationFactorValuesRange(int& rMinimumScanStepSizeComputationFactor_p, int& rMaximumScanStepSizeComputationFactor_p)
            @brief provides access to the valid values range of the scan step size computation factor used in oversampling measurement / processing mode
            @param [out] rMinimumScanStepSizeComputationFactor_p reference to an integer variable in which the minimum scan step size computation factor will be stored
            @param [out] rMaximumScanStepSizeComputationFactor_p reference to an integer variable in which the maximum scan step size computation factor will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetOversamplingModeScanStepSizeComputationFactorValuesRange")]
        public static extern int GetOversamplingModeScanStepSizeComputationFactorValuesRange(out int rMinimumScanStepSizeComputationFactor_p, out int rMaximumScanStepSizeComputationFactor_p);


        [DllImport("smartVis3DMeas.dll", EntryPoint = "EnableAutomaticInspectionMode")]
        public static extern int EnableAutomaticInspectionMode(bool EnabledStatus_p);

        /** @fn int GetAutomaticInspectionModeStatus(bool* pEnabledStatus_p)
            @brief retrieves status of automatic inspection mode
            @param pEnabledStatus_p pointer to a boolean variable in which the status will be stored
            @return int error code, 0x00000000 - success, all other values represents errors (use function GetErrorCodeDescription to get readable error code description)
            */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "GetAutomaticInspectionModeStatus")]
        public static extern int GetAutomaticInspectionModeStatus(out bool pEnabledStatus_p);










        //Create EVENT
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern SafeWaitHandle CreateEvent(IntPtr eventattr, Boolean manualReset, Boolean bInheritState, String lpName);

        // Use interop to call the CreateMutex function.
        // For more information about CreateMutex,
        // see the unmanaged MSDN reference library.
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern SafeWaitHandle CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);





    }
}
