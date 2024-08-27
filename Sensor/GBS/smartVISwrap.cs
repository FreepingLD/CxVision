using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Sensor
{
    public class smartVISwrap
    {
        /** @fn bool SV3D_init ( char *fname );
	    @brief global initialization
	    @param [in] fname fully	qualified filename for DLL main configuration file
	    @return false, if an error occured
	    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_init")]
        public static extern bool SV3D_init(string file);  // InitializeSmartVIS3DInterface

        [DllImport("smartVis3DMeas.dll", EntryPoint = "InitializeSmartVIS3DInterface")]
        public static extern bool InitializeSmartVIS3DInterface(string file);

        /** @fn bool SV3D_isInit ( void );
	    @brief gets the state of initialization
	    @return true if library is initialized, else false
	    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_isInit")]
        public static extern bool SV3D_isInit(); // IsSmartVIS3DInterfaceInitialized
        /** @fn bool IsSmartVIS3DInterfaceInitialized ( void );
         @brief gets the state of initialization
         @return true if library is initialized, else false
        */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "IsSmartVIS3DInterfaceInitialized")]
        public static extern bool IsSmartVIS3DInterfaceInitialized(); // 

        /** @fn bool SV3D_deInit( void );
	    @brief global deinitialization
	    @return false, if an error occured
	    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_deInit")]
        public static extern bool SV3D_deInit(); 

		/** @fn bool DeinitializeSmartVIS3DInterface( void );
         @brief global deinitialization
         @return false, if an error occured
        */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "DeinitializeSmartVIS3DInterface")]
		public static extern bool DeinitializeSmartVIS3DInterface(); 

		/** @fn bool SV3D_cleanUp( void );
    	@brief deletes all data, and reinitialize library
	    @return false, if an error occured
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_cleanUp")]
        public static extern bool SV3D_cleanUp();

        /** @fn bool SV3D_setZmin( double minval )
	    @brief sets lower Z position of driving range of piezo actor
	    If the current position is outside the range, the lower Z position is set automatically.   
	    @param [in] minval lower bound of the Z range [mm]
	    @return false, if value is not in the allowed range or greather than set upper value
	    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setZmin")]
        public static extern bool SV3D_setZmin(double minval);  

		/** @fn bool LockScanRangeMinimumPosition( double minval )
@brief sets lower Z position of driving range of piezo actor
If the current position is outside the range, the lower Z position is set automatically.   
@param [in] minval lower bound of the Z range [mm]
@return false, if value is not in the allowed range or greather than set upper value
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "LockScanRangeMinimumPosition")]
		public static extern bool LockScanRangeMinimumPosition(double minval);  //

		/** @fn bool SV3D_setZmax( double maxval )
	    @brief sets upper Z position of driving range of piezo actor
	    If the current position is outside the range, the upper Z position is set automatically.   
	    @param [in] maxval upper bound of the Z range [mm]
	    @return false, if value is not in the allowed range or smaller than set lower value
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setZmax")]
        public static extern bool SV3D_setZmax(double maxval);

		/** @fn bool SV3D_setZmax( double maxval )
@brief sets upper Z position of driving range of piezo actor
If the current position is outside the range, the upper Z position is set automatically.   
@param [in] maxval upper bound of the Z range [mm]
@return false, if value is not in the allowed range or smaller than set lower value
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "LockScanRangeMaximumPosition")]
		public static extern bool LockScanRangeMaximumPosition(double maxval);

		/** @fn bool SV3D_isZminLocked( bool* pIsLocked )
		@brief checks if zmin position of the actuator driving range is locked
		@param pIsLocked pointer to a boolean variable in which the check result is stored
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_isZminLocked")]
        public static extern bool SV3D_isZminLocked(out bool pIsLocked); //

		/** @fn bool IsScanRangeMinimumPositionLocked( bool* pIsLocked )
@brief checks if zmin position of the actuator driving range is locked
@param pIsLocked pointer to a boolean variable in which the check result is stored
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsScanRangeMinimumPositionLocked")]
		public static extern bool IsScanRangeMinimumPositionLocked(out bool pIsLocked); 

		/** @fn bool SV3D_isZmaxLocked( bool* pIsLocked )
		@brief checks if zmax position of the actuator driving range is locked
		@param pIsLocked pointer to a boolean variable in which the check result is stored
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_isZmaxLocked")]
        public static extern bool SV3D_isZmaxLocked(out bool pIsLocked);

		/** @fn bool IsScanRangeMaximumPositionLocked( bool* pIsLocked )
@brief checks if zmax position of the actuator driving range is locked
@param pIsLocked pointer to a boolean variable in which the check result is stored
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsScanRangeMaximumPositionLocked")]
		public static extern bool IsScanRangeMaximumPositionLocked(out bool pIsLocked);

		/** @fn void SV3D_resetZmin( )
		@brief releases minimum zpos lock, the absolute range minimum of the actuator is available
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_resetZmin")]
        public static extern void SV3D_resetZmin();

		/** @fn void ReleaseScanRangeMinimumPositionLock( )
@brief releases minimum zpos lock, the absolute range minimum of the actuator is available
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "ReleaseScanRangeMinimumPositionLock")]
		public static extern void ReleaseScanRangeMinimumPositionLock();

		/** @fn void SV3D_resetZmax( )
		@brief releases maximum zpos lock, the absolute range maximum of the actuator is available
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_resetZmax")]
        public static extern void SV3D_resetZmax();

		/** @fn void ReleaseScanRangeMaximumPositionLock( )
@brief releases maximum zpos lock, the absolute range maximum of the actuator is available
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "ReleaseScanRangeMaximumPositionLock")]
		public static extern void ReleaseScanRangeMaximumPositionLock();

		/** @fn void SV3D_resetZrange( void )
	    @brief deletes the set Z positioning range, the whole range of the positioner is available
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_resetZrange")]
        public static extern bool SV3D_resetZrange();

		/** @fn void ReleaseScanRangeLock( void )
@brief deletes the set Z positioning range, the whole range of the positioner is available
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "ReleaseScanRangeLock")]
		public static extern bool ReleaseScanRangeLock();

		/** @fn void SV3D_getZrange( double *Zmin, double *Zmax )
	    @brief gives the allowed Z range of the piezo actor
	    @param [out] Zmin lower bound of the Z range [mm]
	    @param [out] Zmax upper bound of the Z range [mm]
	    the given pointers must point to allowed memory locations
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getZrange")]
        public static extern bool SV3D_getZrange(out double Zmin, out double Zmax); // 获取扫描范围

		/** @fn void GetScanRange( double *Zmin, double *Zmax )
@brief gives the allowed Z range of the piezo actor
@param [out] Zmin lower bound of the Z range [mm]
@param [out] Zmax upper bound of the Z range [mm]
the given pointers must point to allowed memory locations
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanRange")]
		public static extern bool GetScanRange(out double Zmin, out double Zmax); // 获取扫描范围

		/** @fn void SV3D_getAbsoluteZrange( double* pZmin, double* pZmax )
		@brief gives the absolute Z range of the actuator
		@param [out] pZmin pointer to lower bound of the absolute Z range [mm]
		@param [out] pZmax pointer to upper bound of the absolute Z range [mm]
		the given pointers must point to allowed memory locations
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getAbsoluteZrange")]
        public static extern bool SV3D_getAbsoluteZrange(out double Zmin, out double Zmax);// 获取景深范围

		/** @fn void GetCompleteScanRange( double* pZmin, double* pZmax )
@brief gives the absolute Z range of the actuator
@param [out] pZmin pointer to lower bound of the absolute Z range [mm]
@param [out] pZmax pointer to upper bound of the absolute Z range [mm]
the given pointers must point to allowed memory locations
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetCompleteScanRange")]
		public static extern bool GetCompleteScanRange(out double Zmin, out double Zmax);// 获取景深范围

		/*
          同方法： GetSupportedScanStepSizeMultiplierList
        */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getSupportedZPosMultiplierList")]
        public static extern bool SV3D_getSupportedZPosMultiplierList(out uint[] zPosMultiplierList, out uint zPosMultiplierListSize);// 获取景深范围

        /*
         同方法：GetScanStepSizeMultiplierID
        */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getMultiplier")]
        public static extern bool SV3D_getMultiplier(out uint zPosMultiplierID);// 获取景深范围

        /*
         同方法 SetScanStepSizeMultiplierID
        */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setMultiplier")]
        public static extern bool SV3D_setMultiplier(uint zPosMultiplierID);// 获取景深范围



        /** @fn bool SV3D_getZpos( double* Zpos)
	    @brief gets the current position of the piezo actor
	    @param [out] Zpos current position of the piezo actor [mm]
	    the given pointer must point to an allowed memory location
	    @return false on error
	    */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getZpos")]
        public static extern bool SV3D_getZpos(out double Zpos); // GetScanPosition

		/** @fn bool GetScanPosition( double* Zpos)
@brief gets the current position of the piezo actor
@param [out] Zpos current position of the piezo actor [mm]
the given pointer must point to an allowed memory location
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetScanPosition")]
		public static extern bool GetScanPosition(out double Zpos); // GetScanPosition


		/** @fn bool SV3D_gotoZpos( double posval )
        @brief moves the piezo actor to the given position
        @param [in] posval destination of movement [mm]
        @return false if the position is outside the allowed range 
        The function return when the position is reached
        */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_gotoZpos")]
        public static extern bool SV3D_gotoZpos(double posval);  // MoveToScanPosition

		/** @fn bool MoveToScanPosition( double posval )
@brief moves the piezo actor to the given position
@param [in] posval destination of movement [mm]
@return false if the position is outside the allowed range 
The function return when the position is reached
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "MoveToScanPosition")]
		public static extern bool MoveToScanPosition(double posval);  // MoveToScanPosition

		/** @fn bool SV3D_isPosDevXposUsed( bool* pIsUsed )
		@brief checks whether the X axis of the positioning device is controlled internally
		@param pIsUsed pointer to a boolean variable in which the check result is stored
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_isPosDevXposUsed")]
        public static extern bool SV3D_isPosDevXposUsed(out bool pIsUsed);

		/** @fn bool IsXPositioningAxisSoftwareControllable( bool* pIsUsed )
@brief checks whether the X axis of the positioning device is controlled internally
@param pIsUsed pointer to a boolean variable in which the check result is stored
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsXPositioningAxisSoftwareControllable")]
		public static extern bool IsXPositioningAxisSoftwareControllable(out bool pIsUsed);

		/** @fn bool SV3D_isPosDevYposUsed( bool* pIsUsed )
		@brief checks whether the Y axis of the positioning device is controlled internally
		@param pIsUsed pointer to a boolean variable in which the check result is stored
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_isPosDevYposUsed")]
        public static extern bool SV3D_isPosDevYposUsed(out bool pIsUsed);

		/** @fn bool IsYPositioningAxisSoftwareControllable( bool* pIsUsed )
@brief checks whether the Y axis of the positioning device is controlled internally
@param pIsUsed pointer to a boolean variable in which the check result is stored
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsYPositioningAxisSoftwareControllable")]
		public static extern bool IsYPositioningAxisSoftwareControllable(out bool pIsUsed);

		/** @fn bool SV3D_isPosDevZposUsed( bool* pIsUsed )
		@brief checks whether the Z axis of the positioning device is controlled internally
		@param pIsUsed pointer to a boolean variable in which the check result is stored
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_isPosDevZposUsed")]
        public static extern bool SV3D_isPosDevZposUsed(out bool pIsUsed);

		/** @fn bool IsZPositioningAxisSoftwareControllable( bool* pIsUsed )
@brief checks whether the Z axis of the positioning device is controlled internally
@param pIsUsed pointer to a boolean variable in which the check result is stored
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsZPositioningAxisSoftwareControllable")]
		public static extern bool IsZPositioningAxisSoftwareControllable(out bool pIsUsed);

		/** @fn bool SV3D_getPosDevXpos( double* pPosval )
		@brief reads the current X coordinate of the positioning device
		@param [out] pPosval pointer to X coordinate of the positioning device [mm]
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getPosDevXpos")]
        public static extern bool SV3D_getPosDevXpos(out double pPosval);

		/** @fn bool GetXPositioningAxisCoordinate( double* pPosval )
@brief reads the current X coordinate of the positioning device
@param [out] pPosval pointer to X coordinate of the positioning device [mm]
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetXPositioningAxisCoordinate")]
		public static extern bool GetXPositioningAxisCoordinate(out double pPosval);

		/** @fn bool SV3D_getPosDevYpos( double* pPosval )
		@brief reads the current Y coordinate of the positioning device
		@param [out] pPosval pointer to Y coordinate of the positioning device [mm]
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getPosDevYpos")]
        public static extern bool SV3D_getPosDevYpos(out double pPosval);

		/** @fn bool GetYPositioningAxisCoordinate( double* pPosval )
@brief reads the current Y coordinate of the positioning device
@param [out] pPosval pointer to Y coordinate of the positioning device [mm]
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetYPositioningAxisCoordinate")]
		public static extern bool GetYPositioningAxisCoordinate(out double pPosval);

		/** @fn bool SV3D_getPosDevZpos( double* pPosval )
		@brief reads the current Z coordinate of the positioning device
		@param [out] pPosval pointer to Z coordinate of the positioning device [mm]
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getPosDevZpos")]
        public static extern bool SV3D_getPosDevZpos(out double pPosval);

		/** @fn bool GetZPositioningAxisCoordinate( double* pPosval )
@brief reads the current Z coordinate of the positioning device
@param [out] pPosval pointer to Z coordinate of the positioning device [mm]
@return false, if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetZPositioningAxisCoordinate")]
		public static extern bool GetZPositioningAxisCoordinate(out double pPosval);

		/** @fn bool SV3D_setExternPosDevXpos( double posval )
		@brief sets the X coordinate of an external positioning device for the current measurement
		@param [in] posval X coordinate of an external positioning device [mm]
		@return false if a configured X positioning device is activated for the measurement (X axis is controlled internally)
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setExternPosDevXpos")]
        public static extern bool SV3D_setExternPosDevXpos(double pPosval);

		/** @fn bool SetExternallyControlledXPositioningAxisCoordinate( double posval )
@brief sets the X coordinate of an external positioning device for the current measurement
@param [in] posval X coordinate of an external positioning device [mm]
@return false if a configured X positioning device is activated for the measurement (X axis is controlled internally)
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetExternallyControlledXPositioningAxisCoordinate")]
		public static extern bool SetExternallyControlledXPositioningAxisCoordinate(double pPosval);

		/** @fn bool SV3D_setExternPosDevYpos( double posval )
		@brief sets the Y coordinate of an external positioning device for the current measurement
		@param [in] posval Y coordinate of an external positioning device [mm]
		@return false if a configured Y positioning device is activated for the measurement (Y axis is controlled internally)
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setExternPosDevYpos")]
        public static extern bool SV3D_setExternPosDevYpos(double pPosval);

		/** @fn bool SetExternallyControlledYPositioningAxisCoordinate( double posval )
@brief sets the Y coordinate of an external positioning device for the current measurement
@param [in] posval Y coordinate of an external positioning device [mm]
@return false if a configured Y positioning device is activated for the measurement (Y axis is controlled internally)
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetExternallyControlledYPositioningAxisCoordinate")]
		public static extern bool SetExternallyControlledYPositioningAxisCoordinate(double pPosval);

		/** @fn bool SV3D_setExternPosDevZpos( double posval )
		@brief sets the Z coordinate of an external positioning device for the current measurement
		@param [in] posval Z coordinate of an external positioning device [mm]
		@return false if a configured Z positioning device is activated for the measurement (Z axis is controlled internally)
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setExternPosDevZpos")]
        public static extern bool SV3D_setExternPosDevZpos(double pPosval);

		/** @fn bool SetExternallyControlledZPositioningAxisCoordinate( double posval )
@brief sets the Z coordinate of an external positioning device for the current measurement
@param [in] posval Z coordinate of an external positioning device [mm]
@return false if a configured Z positioning device is activated for the measurement (Z axis is controlled internally)
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetExternallyControlledZPositioningAxisCoordinate")]
		public static extern bool SetExternallyControlledZPositioningAxisCoordinate(double pPosval);

		/** @fn bool SV3D_getMeasuringProcedureNumber ( unsigned int* count )
	    @brief gets number of measuring procedures (listed in the configuration file).
	    A measuring procedure contains the essential configuration data (camera dataset indices, 
	    positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
	    @param [out] count number of listed measuring procedure datasets in the configuration file
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getMeasuringProcedureNumber")]
        public static extern bool SV3D_getMeasuringProcedureNumber(out uint posval);

		/** @fn bool GetMeasurementProceduresNumber ( unsigned int* count )
@brief gets number of measuring procedures (listed in the configuration file).
A measuring procedure contains the essential configuration data (camera dataset indices, 
positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
@param [out] count number of listed measuring procedure datasets in the configuration file
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementProceduresNumber")]
		public static extern bool GetMeasurementProceduresNumber(out uint posval);

		/** @fn bool SV3D_getSelectedMeasuringProcedureIndex ( unsigned int* idx )
	    @brief gets index of the currently selected measuring procedure (listed in the configuration file).
	    A measuring procedure contains the essential configuration data (camera dataset indices, 
	    positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
	    @param [out] idx index of current measuring procedure dataset
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getSelectedMeasuringProcedureIndex")]
        public static extern bool SV3D_getSelectedMeasuringProcedureIndex(out uint idx);

		/** @fn bool GetMeasurementProcedureID ( unsigned int* idx )
@brief gets index of the currently selected measuring procedure (listed in the configuration file).
A measuring procedure contains the essential configuration data (camera dataset indices, 
positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
@param [out] idx index of current measuring procedure dataset
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementProcedureID")]
		public static extern bool GetMeasurementProcedureID(out uint idx);

		/** @fn bool SV3D_getMeasuringProcedureName ( unsigned int idx, char* name, unsigned int* nchar )
	    @brief gets short designation of the indexed measuring procedure (listed in the configuration file).
	    A measuring procedure contains the essential configuration data (camera dataset indices, 
	    positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
	    @param [in] idx index of measuring procedure dataset
	    @param [out] name[nchar] pointer to a zero terminated String of length (nchar) max, 
	    contains short designation of indexed measuring procedure
	    @param [in,out] nchar [in] pointer to the number of character (available space) in name[], 
	    [out] pointer to the number of character in name[] (length of the string that was filled)
	    @return false, if no measuring procedure is available for the given index
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getMeasuringProcedureName")]
        public static extern bool SV3D_getMeasuringProcedureName(uint idx, IntPtr name, ref uint nchar);

		/** @fn bool GetMeasurementProcedureName ( unsigned int idx, char* name, unsigned int* nchar )
@brief gets short designation of the indexed measuring procedure (listed in the configuration file).
A measuring procedure contains the essential configuration data (camera dataset indices, 
positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
@param [in] idx index of measuring procedure dataset
@param [out] name[nchar] pointer to a zero terminated String of length (nchar) max, 
contains short designation of indexed measuring procedure
@param [in,out] nchar [in] pointer to the number of character (available space) in name[], 
[out] pointer to the number of character in name[] (length of the string that was filled)
@return false, if no measuring procedure is available for the given index
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementProcedureName")]
		public static extern bool GetMeasurementProcedureName(uint idx, IntPtr name, ref uint nchar);

		/** @fn bool SV3D_selectMeasuringProcedure ( unsigned int idx )
	    @brief selects a measuring procedure from the listing in the configuration file.
	    A measuring procedure contains the essential configuration data (camera dataset indices, 
	    positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
	    @param [in] idx index of measuring procedure dataset
	    @return false, if no measuring procedure is available for the given index
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_selectMeasuringProcedure")]
        public static extern bool SV3D_selectMeasuringProcedure(uint idx);

		/** @fn bool SelectMeasurementProcedure ( unsigned int idx )
@brief selects a measuring procedure from the listing in the configuration file.
A measuring procedure contains the essential configuration data (camera dataset indices, 
positioning dataset index, lens index, lighting setting, measuring parameter, template names) 
@param [in] idx index of measuring procedure dataset
@return false, if no measuring procedure is available for the given index
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SelectMeasurementProcedure")]
		public static extern bool SelectMeasurementProcedure(uint idx);

		/** @fn bool SV3D_addMeasuringProcedure ( std::string MeasuringProcedureName_p, int& rMeasuringProcedureIdx_p )
		@brief creates a new measuring procedure with currently active settings for camera, light, piezo and stage units.
		@param [in] pMeasuringProcedureName_p name of the new measuring procedure dataset
		@param [out] rMeasuringProcedureIdx_p internally used list index of the new measuring procedure dataset
		@return false, if errors occurred and the measuring procedure couldn't be added to procedures list
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_addMeasuringProcedure")]
        public static extern bool SV3D_addMeasuringProcedure(IntPtr pMeasuringProcedureName_p, ref uint rMeasuringProcedureIdx_p);

		/** @fn bool AddMeasurementProcedure ( std::string MeasuringProcedureName_p, int& rMeasuringProcedureIdx_p )
@brief creates a new measuring procedure with currently active settings for camera, light, piezo and stage units.
@param [in] pMeasuringProcedureName_p name of the new measuring procedure dataset
@param [out] rMeasuringProcedureIdx_p internally used list index of the new measuring procedure dataset
@return false, if errors occurred and the measuring procedure couldn't be added to procedures list
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "AddMeasurementProcedure")]
		public static extern bool AddMeasurementProcedure(IntPtr pMeasuringProcedureName_p, ref uint rMeasuringProcedureIdx_p);


		/** @fn bool SV3D_getSupportedLensNumber ( unsigned int* lensNumber )
		@brief gets the number of objective lens datasets stored in configuration file
		@param pLensNumber pointer to number of objective lens datasets stored in configuration file
		@return false if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getSupportedLensNumber")]
        public static extern bool SV3D_getSupportedLensNumber(out uint pLensNumber);

		/** @fn bool GetSupportedObjectivesNumber ( unsigned int* lensNumber )
@brief gets the number of objective lens datasets stored in configuration file
@param pLensNumber pointer to number of objective lens datasets stored in configuration file
@return false if application is not initialized correctly
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetSupportedObjectivesNumber")]
		public static extern bool GetSupportedObjectivesNumber(out uint pLensNumber);

		/** @fn bool SV3D_selectLens ( unsigned int idx )
	    @brief selects a lens from the lens table in the configuration file.
	    This function sets the configuration data (scale) and 
	    changes also the lenses with an active objective changer.
	    @param [in] idx index of lens dataset
	    @return false, if no lens is available for the given index
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_selectLens")]
        public static extern bool SV3D_selectLens(uint idx);


		/** @fn bool SV3D_getSelectedLensID ( unsigned int* pIdx )
		@brief get the consecutive id of the lens dataset stored in the configuration file for the selected lens 
		@param pIdx pointer to index of the selected lens dataset
		@return false, if application is not initialized correctly
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getSelectedLensID")]
        public static extern bool SV3D_getSelectedLensID(out uint idx);


		/** @fn bool SV3D_getLensName ( unsigned int idx, char* objectiveName, unsigned int* nchar )
		@brief gets the objective lens name for the provided index idx
		@param idx index of the required lens dataset
		@param objectiveName[nchar] pointer to a zero terminated String of length (nchar) max, 
								contains the name for the objective lens for the provided index idx
		@param pnchar pointer to the number of character (available space) in objectiveName[]
		@return false, if application is not initialized correctly or wrong lens idx is provided
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getLensName")]
        public static extern bool SV3D_getLensName(uint idx, IntPtr objectiveName, ref uint pnchar);

		/** @fn bool GetObjectiveName ( unsigned int idx, char* objectiveName, unsigned int* nchar )
@brief gets the objective lens name for the provided index idx
@param idx index of the required lens dataset
@param objectiveName[nchar] pointer to a zero terminated String of length (nchar) max, 
						contains the name for the objective lens for the provided index idx
@param pnchar pointer to the number of character (available space) in objectiveName[]
@return false, if application is not initialized correctly or wrong lens idx is provided
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetObjectiveName")]
		public static extern bool GetObjectiveName(uint idx,  IntPtr objectiveName, ref uint pnchar);

		/** @fn void SV3D_getResolution ( double *Xres, double *Yres )
	    @brief supplies the metric resolution according to the selected lens and camera (and binning/subsampling setting).
	    @param [out] Xres lateral resolution in X direction [mm/Pixel]
	    @param [out] Yres lateral resolution in Y direction [mm/Pixel]
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getResolution")]
        public static extern bool SV3D_getResolution(out double Xres, out double Yres);

		/** @fn void GetMetricResolution ( double *Xres, double *Yres )
@brief supplies the metric resolution according to the selected lens and camera (and binning/subsampling setting).
@param [out] Xres lateral resolution in X direction [mm/Pixel]
@param [out] Yres lateral resolution in Y direction [mm/Pixel]
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetMetricResolution")]
		public static extern bool GetMetricResolution(out double Xres, out double Yres);

		/** @fn bool SV3D_getLensSpecificResolution(unsigned int idx, double* pXres, double* pYres)
		@brief supplies the metric resolution according to the lens with id idx and camera (and binning/subsampling setting).
		@param idx index of the required lens dataset
		@param [out] pXres lateral resolution in X direction [mm/Pixel]
		@param [out] pYres lateral resolution in Y direction [mm/Pixel]
		@return false on error
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getLensSpecificResolution")]
        public static extern bool SV3D_getLensSpecificResolution(uint idx, out double Xres, out double Yres);

		/** @fn bool GetObjectiveSpecificMetricResolution(unsigned int idx, double* pXres, double* pYres)
@brief supplies the metric resolution according to the lens with id idx and camera (and binning/subsampling setting).
@param idx index of the required lens dataset
@param [out] pXres lateral resolution in X direction [mm/Pixel]
@param [out] pYres lateral resolution in Y direction [mm/Pixel]
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetObjectiveSpecificMetricResolution")]
		public static extern bool GetObjectiveSpecificMetricResolution(uint idx, out double Xres, out double Yres);

		/** @fn bool SV3D_getLightingIntensity ( unsigned int *intensity )
    	@brief gets the current intensity value of illuminating unit
	    @param [out] intensity, current intensity value of illuminating unit [0...255]
	    the given pointer must point to an allowed memory location
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getLightingIntensity")]
        public static extern bool SV3D_getLightingIntensity(out uint intensity);

		/** @fn bool GetLuminousIntensity ( unsigned int *intensity )
@brief gets the current intensity value of illuminating unit
@param [out] intensity, current intensity value of illuminating unit [0...255]
the given pointer must point to an allowed memory location
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetLuminousIntensity")]
		public static extern bool GetLuminousIntensity(out uint intensity);

		/** @fn bool SV3D_setLightingIntensity ( unsigned int intensity )
	    @brief sets the intensity value of illuminating unit
	    @param [in] intensity, set value for illuminating unit [0...255]
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setLightingIntensity")]
        public static extern bool SV3D_setLightingIntensity(uint intensity);

		/** @fn bool SetLuminousIntensity ( unsigned int intensity )
@brief sets the intensity value of illuminating unit
@param [in] intensity, set value for illuminating unit [0...255]
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetLuminousIntensity")]
		public static extern bool SetLuminousIntensity(uint intensity);

		/** @fn bool SV3D_getLightingIntensityRange ( unsigned int intensity )
@brief sets the intensity value of illuminating unit
@param [in] intensity, set value for illuminating unit [0...255]
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getLightingIntensityRange")]
		public static extern bool SV3D_getLightingIntensityRange(uint intensity);

		/** @fn bool GetLuminousIntensityValuesRange ( unsigned int intensity )
@brief sets the intensity value of illuminating unit
@param [in] intensity, set value for illuminating unit [0...255]
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetLuminousIntensityValuesRange")]
		public static extern bool GetLuminousIntensityValuesRange(uint intensity);

		/** @fn SV3D_getCameraGainRange ( unsigned int *gainMin, unsigned int *gainMax );
	    @brief gets the range of camera gain control
	    @param [out] gainMin, minimum gain value of camera
	    @param [out] gainMax, maximum gain value of camera
	    the given pointer must point to an allowed memory location
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getCameraGainRange")]
        public static extern bool SV3D_getCameraGainRange(out uint gainMin, out uint gainMax);

		/** @fn GetCameraGainValuesRange ( unsigned int *gainMin, unsigned int *gainMax );
@brief gets the range of camera gain control
@param [out] gainMin, minimum gain value of camera
@param [out] gainMax, maximum gain value of camera
the given pointer must point to an allowed memory location
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraGainValuesRange")]
		public static extern bool GetCameraGainValuesRange(out uint gainMin, out uint gainMax);

		/** @fn bool SV3D_getCameraGain ( unsigned int *gainValue )
	    @brief gets the current gain value of camera (current selected camera dataset)
	    @param [out] gainValue, current gain value of camera
	    the given pointer must point to an allowed memory location
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getCameraGain")]
        public static extern bool SV3D_getCameraGain(out uint gainValue);

		/** @fn bool GetCameraGain ( unsigned int *gainValue )
@brief gets the current gain value of camera (current selected camera dataset)
@param [out] gainValue, current gain value of camera
the given pointer must point to an allowed memory location
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraGain")]
		public static extern bool GetCameraGain(out uint gainValue);

		/** @fn bool SV3D_setCameraGain ( unsigned int gainValue )
	    @brief sets the gain value of camera in all corresponding camera datasets (adressed in measuring procedure dataset)
	    @param [in] gainValue, set value for camera gain
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setCameraGain")]
        public static extern bool SV3D_setCameraGain(uint gainValue);

		/** @fn bool SetCameraGain ( unsigned int gainValue )
@brief sets the gain value of camera in all corresponding camera datasets (adressed in measuring procedure dataset)
@param [in] gainValue, set value for camera gain
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetCameraGain")]
		public static extern bool SetCameraGain(uint gainValue);

		/** @fn SV3D_getCameraExposureRange ( unsigned int *exposureMin, unsigned int *exposureMax );
	    @brief gets the range of camera exposure control
	    @param [out] exposureMin, minimum exposure value of camera
	    @param [out] exposureMax, maximum exposure value of camera
	    the given pointer must point to an allowed memory location
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getCameraExposureRange")]
        public static extern bool SV3D_getCameraExposureRange(out uint exposureMin, out uint exposureMax);

		/** @fn GetCameraExposureValuesRange ( unsigned int *exposureMin, unsigned int *exposureMax );
@brief gets the range of camera exposure control
@param [out] exposureMin, minimum exposure value of camera
@param [out] exposureMax, maximum exposure value of camera
the given pointer must point to an allowed memory location
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraExposureValuesRange")]
		public static extern bool GetCameraExposureValuesRange(out uint exposureMin, out uint exposureMax);

		/** @fn bool SV3D_getCameraExposure ( unsigned int *exposureValue )
	    @brief gets the current exposure value of camera (current selected camera dataset)
	    @param [out] exposureValue, current exposure value of camera [µs]
	    the given pointer must point to an allowed memory location
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getCameraExposure")]
        public static extern bool SV3D_getCameraExposure(out uint exposureValue);

		/** @fn bool GetCameraExposure ( unsigned int *exposureValue )
@brief gets the current exposure value of camera (current selected camera dataset)
@param [out] exposureValue, current exposure value of camera [µs]
the given pointer must point to an allowed memory location
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraExposure")]
		public static extern bool GetCameraExposure(out uint exposureValue);

		/** @fn bool SV3D_setCameraExposure ( unsigned int exposureValue )
	    @brief sets the exposure value of camera in all corresponding camera datasets (adressed in measuring procedure dataset)
	    @param [in] exposureValue, set value for camera exposure
	    @return false on error
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setCameraExposure")]
        public static extern bool SV3D_setCameraExposure(uint exposureValue);

		/** @fn bool SetCameraExposure ( unsigned int exposureValue )
@brief sets the exposure value of camera in all corresponding camera datasets (adressed in measuring procedure dataset)
@param [in] exposureValue, set value for camera exposure
@return false on error
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetCameraExposure")]
		public static extern bool SetCameraExposure(uint exposureValue);

		/** @fn void SV3D_setOutputFilename (char *fname )
	    @brief sets the filename of the output file with the measurement results, the extension must be ".sur"
	    @param fname fully qualified or relative file name to store the measurement results. If NULL is given, nothing is stored.
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setOutputFilename")]
        public static extern bool SV3D_setOutputFilename(string fname);

		/** @fn void SetMeasurementResultsFileName (char *fname )
@brief sets the filename of the output file with the measurement results, the extension must be ".sur"
@param fname fully qualified or relative file name to store the measurement results. If NULL is given, nothing is stored.
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetMeasurementResultsFileName")]
		public static extern bool SetMeasurementResultsFileName(string fname);

		/** @fn bool SV3D_measure()
	    @brief starts a scan through the given range and performs the white light interferometric calculation,
	    the result is saved in a file with the given file name. The function returns after start of the measurement.
	    Then the successful execution is signaled by the Event, given by SV3D_setResultReadyNotification.
	    @result Result of measurement execution
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_measure")]
        public static extern bool SV3D_measure();

		/** @fn bool ExecuteMeasurementProcedure()
@brief starts a scan through the given range and performs the white light interferometric calculation,
the result is saved in a file with the given file name. The function returns after start of the measurement.
Then the successful execution is signaled by the Event, given by SV3D_setResultReadyNotification.
@result Result of measurement execution
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "ExecuteMeasurementProcedure")]
		public static extern bool ExecuteMeasurementProcedure();

		/** @fn void SV3D_breakMeasure( void )
	    @brief cancels an active measurement
	    The active measurement process is canceled.
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_breakMeasure")]
        public static extern bool SV3D_breakMeasure();

		/** @fn void CancelMeasurementProcedure( void )
            @brief cancels an active measurement
            The active measurement process is canceled.
        */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "CancelMeasurementProcedure")]

		public static extern bool CancelMeasurementProcedure();

		/** @fn bool SV3D_configMeasurement( bool execUnit, bool smoothSurface, bool PSI )
	    @brief sets the options for the measurement mode
	    @param execBaseGPU if false computation is executed on CPU, if true on GPU (if license available)
	    @param smoothSurface if true the extended mode for smooth surfaces is activated
	    @param usePSI if true the PSI mode is used, otherwise the default VSI mode is used (see manual for instructions)
	    @param zPosMultiplier multiplier for the normal step height increment, allowed values are 1, 3, 5, 7, 9, 11, 13, 15, 17, default value is 1.
	    @return false, if the given parameter configuration is not allowed
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_configMeasurement")]
        public static extern bool SV3D_configMeasurement(bool execBaseGPU, bool smoothSurface, bool usePSI, int zPosMultiplier);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetMeasurementProcedureSettings")]
		public static extern bool SetMeasurementProcedureSettings(bool execBaseGPU, bool smoothSurface, bool usePSI, int zPosMultiplier);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_IsVSIModeEnabled")]
        public static extern bool SV3D_IsVSIModeEnabled(out bool pVSIModeEnabledStatus_p);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsVSIModeEnabled")]
		public static extern bool IsVSIModeEnabled(out bool pVSIModeEnabledStatus_p);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_IsEPSIModeEnabled")]
        public static extern bool SV3D_IsEPSIModeEnabled(out bool pEPSIModeEnabledStatus_p);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsEPSIModeEnabled")]
		public static extern bool IsEPSIModeEnabled(out bool pEPSIModeEnabledStatus_p);


		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_IsPSIModeEnabled")]
        public static extern bool SV3D_IsPSIModeEnabled(out bool pPSIModeEnabledStatus_p);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "IsPSIModeEnabled")]
		public static extern bool IsPSIModeEnabled(out bool pPSIModeEnabledStatus_p);

		/** @fn bool SV3D_setResultReadyNotification( HANDLE hResultReadyEvent, HANDLE hErrorEvent )
	    @brief sets event handle for event notification if the measurement result is available for supply and the event handle for a breaked measurement
	    The event handles are stored in the API and after finishing measurement, if a measurement result is available, the the
	    event is signaled by the API execution thread. If an error occurred or the measurement was cancelled, the hErrorEvent is set.
	    @param hResultReadyEvent Windows event handle for notification of the calling process (thread)
	    @param hErrorEvent Windows event handle for notification of an error during measurement (measurement is canceled), can be set to NULL if not used.
        */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setResultReadyNotification")]
        public static extern bool SV3D_setResultReadyNotification(IntPtr hResultReadyEvent, IntPtr hErrorEvent);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetResultReadyNotificationHandles")]
		public static extern bool SetResultReadyNotificationHandles(IntPtr hResultReadyEvent, IntPtr hErrorEvent);

		/** @fn bool SV3D_getHeightData(float *height, float *quality, unsigned int *columns, unsigned int *rows )
	    @brief supplies the measured data in height and the calculated quality in quality.
	    The memory must be supplied by the caller. The arrays are linear with size columns * rows (equals to image size) and row ordered.
	    The actual values are supplied in columns and rows. 
	    @param height pointer to array for height data. Height is in mm.
	    @param quality pointer to array for quality data. Quality is in the range of 0 .. 1. If quality is not needed a NULL pointer can be supplied.
	    @param columns horizontal size of the measurement field in pixels, equals the column count of the image
	    @param rows vertical size of the measurement field in pixels, equals the row count of the image
	    @param heightOffset common offset to height data for absolute height values
	    @return false on error
        */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getHeightData")]
        public static extern bool SV3D_getHeightData(IntPtr height, IntPtr quality, ref uint columns, ref uint rows, out float heightOffset);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetMeasurementResultData")]
		public static extern bool GetMeasurementResultData(IntPtr height, IntPtr quality, ref uint columns, ref uint rows, out float heightOffset);

		/** @fn bool SV3D_enableLiveImaging( bool enable )
	    @brief enables live imaging between measurements
	    Live imaging should be disabled for automatic measurement procedures because of camera internal switching time
	    @param enable if true enables live imaging otherwise no images are acquired in measurement pauses
        */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_enableLiveImaging")]
        public static extern bool SV3D_enableLiveImaging(bool enable);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "ChangeLiveImagingStatus")]
		public static extern bool ChangeLiveImagingStatus(bool enable);


		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setFrameReadyNotification")]
		public static extern bool SV3D_setFrameReadyNotification(out uint columns, out uint rows);


		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetFrameReadyNotificationHandle")]
		public static extern bool SetFrameReadyNotificationHandle(out uint columns, out uint rows);


		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_setFrameReadyCallback")]
		public static extern bool SV3D_setFrameReadyCallback(out uint columns, out uint rows);


		[DllImport("smartVis3DMeas.dll", EntryPoint = "SetFrameReadyCallbackFunctionData")]
		public static extern bool SetFrameReadyCallbackFunctionData(out uint columns, out uint rows);

		/** fn bool SV3D_getImageSize(unsigned int *xsize, unsigned int *ysize)
	    @brief determines the size of the camera image which is needed for memory allocation by client
	    @param columns contains the number of columns of the camera image 
	    @param rows contains the number of rows (lines) of the camera image
	    @return execution state, false if camera not initialized
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getImageSize")]
        public static extern bool SV3D_getImageSize(out uint columns, out uint rows);

		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraImageSize")]
		public static extern bool GetCameraImageSize(out uint columns, out uint rows);
		/** fn bool SV3D_getImageBPP (unsigned int *bpp, unsigned int *rshift)
	    @brief determines the resolution (byte per pixel) of the camera image which is needed for memory allocation by client
	    @param bpp contains the resolution (byte per pixel) of the camera image [1,2]
	    @param rshift contains the suitable value for a right shift of 16-bit image data before converting to 8-bit [0...8]
	    @return execution state, false if camera not initialized
	    */
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getImageBPP")]
        public static extern bool SV3D_getImageBPP(out uint bpp, out uint rshift);

        /** fn bool SV3D_getFrame(void *imageData, unsigned int sizeOfImageData)
    	@param imageData pointer at memory for a single image, to be supplied by caller
	    image is delivered in a single continuous memory array, line by line
	    @param sizeOfImageData the size of imageData in byte 
	    @return true, if image is available
        */
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getFrame")]
        public static extern bool SV3D_getFrame(IntPtr imageData,  uint sizeOfImageData);

		/** fn bool SV3D_getFrame(void *imageData, unsigned int sizeOfImageData)
@param imageData pointer at memory for a single image, to be supplied by caller
image is delivered in a single continuous memory array, line by line
@param sizeOfImageData the size of imageData in byte 
@return true, if image is available
*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "GetCameraImageData")]
		public static extern bool GetCameraImageData(out byte [] imageData, out uint sizeOfImageData);
		/** fn bool SV3D_clearFrameBuffer()
		clears frame buffer for access to the newest shot 
		@return true, if buffer cleared
		*/
		[DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_clearFrameBuffer")]
        public static extern bool SV3D_clearFrameBuffer();

        /** @fn bool SV3D_enableHighPrecisionMode( bool highPrecisionModeStepsNumber )
		@brief enables high precision mode which can be used to reduce noise influence on the measurement results
		@param highPrecisionModeStepsNumber number of used processing steps, range [0...10], higher values results in smoother results
		   at the cost of larger runtimes 
		@return false, if application is not initialized correctly
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_enableHighPrecisionMode")]
        public static extern bool SV3D_enableHighPrecisionMode(int highPrecisionModeStepsNumber);

        /** @fn bool SV3D_disableHighPrecisionMode( bool highPrecisionModeStepsNumber )
		@brief disables high precision mode which can be used to reduce noise influence on the measurement results
		@return false, if application is not initialized correctly
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_disableHighPrecisionMode")]
        public static extern bool SV3D_disableHighPrecisionMode();

        /** @fn bool SV3D_getHighPrecisionModeStatus( bool* pEnabledStatus, int* pHighPrecisionModeStepsNumber )
		@brief retrieves high precision mode status and if high precision mode is enabled the number of used processing steps
		@param pEnabledStatus pointer to a boolean variable in which the status will be stored
		@param pHighPrecisionModeStepsNumber pointer to a int variable in which the number of used processing steps in high precision mode will be stored
		@return false, if application is not initialized correctly
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getHighPrecisionModeStatus")]
        public static extern bool SV3D_getHighPrecisionModeStatus(out bool pEnabledStatus, out int pHighPrecisionModeStepsNumber);

        /** @fn bool SV3D_enablePhaseUnwrappingAlgorithmUsage( )
		@brief enables phase unwrapping algorithm usage which is used to optimize the measurement results if phase information is used in processing
		@return false, if application is not initialized correctly
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_enablePhaseUnwrappingAlgorithmUsage")]
        public static extern bool SV3D_enablePhaseUnwrappingAlgorithmUsage();

        /** @fn bool SV3D_disablePhaseUnwrappingAlgorithmUsage( )
		@brief disables phase unwrapping algorithm usage which is used to optimize the measurement results if phase information is used in processing
		@return false, if application is not initialized correctly
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_disablePhaseUnwrappingAlgorithmUsage")]
        public static extern bool SV3D_disablePhaseUnwrappingAlgorithmUsage();

        /** @fn bool SV3D_getPhaseUnwrappingAlgorithmStatus( bool* pEnabledStatus )
		@brief retrieves status of phase unwrapping algorithm usage
		@param pEnabledStatus pointer to a boolean variable in which the status will be stored
		@return false, if application is not initialized correctly
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_getPhaseUnwrappingAlgorithmStatus")]
        public static extern bool SV3D_getPhaseUnwrappingAlgorithmStatus(out bool pEnabledStatus);

        /** @fn bool SV3D_IsFastProcessingDeviceEnabled ( )
		@brief retrieves information if fast processing device (GPU) was detected in the PC 
		@return true, if a fast processing device (CUDA-enabled GPU) was detected in the PC, otherwise false.
		*/
        [DllImport("smartVis3DMeas.dll", EntryPoint = "SV3D_IsFastProcessingDeviceEnabled")]
        public static extern bool SV3D_IsFastProcessingDeviceEnabled();

        //Event Functions - Use this Functions to represent C++ compatible Events in C# -----------------------------------------------------------------------------------------------

        [DllImport("kernel32.dll", EntryPoint = "SetEvent")]
        public static extern bool SetEvent(IntPtr lpHandles);

        //OPEN EVENT
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr OpenEvent(UInt32 dwDesiredAccess, Boolean bInheritHandle, String lpName);

        //Create EVENT
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateEvent(IntPtr eventattr, Boolean manualReset, Boolean bInheritState, String lpName);

    }
}
