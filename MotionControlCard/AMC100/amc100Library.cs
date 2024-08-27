using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MotionControlCard
{
    internal class amc100Library
    {
        public enum AxisState
        {
            DeviceNotFound,
            NotConnected,
            OutputDisabled,
            Ready,
            Moving,
            InTargetRange,
            EndOfTravel,
            WTF
        }
        public enum AMC_actorType
        {
            AMC_actorLinear = 0,                       /**< Actor is of linear type            */
            AMC_actorGonio = 1,                       /**< Actor is of goniometer type        */
            AMC_actorRot = 2                        /**< Actor is of rotator type           */
        }

        public enum AMC_RTOUT_MODE
        {
            AMC_RTOUT_AQUADB_LVTTL = 0,
            AMC_RTOUT_AQUADB_LVDS = 1
        }


        /** @brief  RealTime Input Mode                                                               */
        public enum AMC_RTIN_MODE
        {
            AMC_RTIN_AQUADB_LVTTL = 0,
            AMC_RTIN_AQUADB_LVDS = 1,
            AMC_RTIN_STEPPER_LVTTL = 8,
            AMC_RTIN_STEPPER_LVDS = 9,
            AMC_RTIN_TRIGGER_LVTTL = 10,
            AMC_RTIN_TRIGGER_LVDS = 11,
            AMC_RTIN_OFF = 15,
        }


        /** @brief  RealTime Input Feedback Loop Mode                                                               */
        public enum AMC_RTIN_LOOP_MODE
        {
            AMC_RTIN_LOOP_MODE_OPEN_LOOP = 0,
            AMC_RTIN_LOOP_MODE_CLOSED_LOOP = 1,
        }

        public enum result
        {
            NCB_Ok = 0,           /**< No error                              */
            NCB_Error = -1,           /**< Unspecified error                     */
            NCB_NotConnected = -2,           /**< No active connection to device        */
            NCB_DriverError = -3,           /**< Error in comunication with driver     */
            NCB_NetworkError = -4,           /**< Network error when connecting to AMC  */
            BAD_IP_ADDRESS = -5,
            CONNECTION_TIMEOUT = -6,
            NO_DEVICE_FOUND_ERR = -7,
            NCB_InvalidParam = -9,           /**< Parameter out of range                */
            NCB_FeatureNotAvailable = 10,          /**< Feature only available in pro version */
        }


        // Int32 NCB_API AMC_Connect( const char *deviceAddress, Int32* deviceHandle );
        [DllImport("amc.dll", CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_Connect(string deviceAddress, ref int deviceHandle);


        //Int32 NCB_API AMC_Close( Int32 deviceHandle );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_Close(int deviceHandle);


        //Int32 NCB_API AMC_getLockStatus(Int32 deviceHandle, Bln32* locked, Bln32* authorized);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getLockStatus(Int32 deviceHandle, ref int locked, ref int authorized);


        //Int32 NCB_API AMC_lock(Int32 deviceHandle, char* password);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_lock(Int32 deviceHandle, string password);

        //Int32 NCB_API AMC_grantAccess(Int32 deviceHandle, char* password);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_grantAccess(Int32 deviceHandle, string password);

        //Int32 NCB_API AMC_errorNumberToString(Int32 deviceHandle, int lang, int errcode, char* error);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_errorNumberToString(int deviceHandle, int lang, int errcode, string error);

        //Int32 NCB_API AMC_unlock(Int32 deviceHandle);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_unlock(int deviceHandle);

       // Int32 NCB_API AMC_controlOutput(Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlOutput(int deviceHandle, int axis, ref int enable, int set);

        //Int32 NCB_API AMC_controlAmplitude( Int32 deviceHandle, Int32 axis,Int32* amplitude,Bln32 set );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlAmplitude(int deviceHandle, int axis, ref int amplitude, int set);

        //Int32 NCB_API AMC_controlFrequency( Int32 deviceHandle,Int32 axis,Int32* frequency,Bln32 set );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlFrequency(int deviceHandle, int axis, ref int frequency, int set);

       // Int32 NCB_API AMC_controlActorSelection(Int32 deviceHandle,Int32 axis,Int32* actor,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlActorSelection(int deviceHandle, int axis, ref int actor, int set);

        //Int32 NCB_API AMC_getActorName( Int32 deviceHandle, Int32 axis,char* name,Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int intAMC_getActorName(int deviceHandle, int axis, out string name, int size); // 传一个地址进去，在函数内部分配一个指定的存储空间

        //Int32 NCB_API AMC_getActorType( Int32 deviceHandle, Int32 axis,AMC_actorType* type );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getActorType(int deviceHandle, int axis, out AMC_actorType type);

        //Int32 NCB_API AMC_setReset( Int32 deviceHandle,Int32 axis );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_setReset(int deviceHandle, int axis);

        /// <summary>
        /// 移动到目标位置
        /// </summary>
        /// <param name="deviceHandle"></param>
        /// <param name="axis"></param>
        /// <param name="enable"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        //Int32 NCB_API AMC_controlMove( Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlMove(int deviceHandle, int axis, ref int enable, int set);

        //Int32 NCB_API AMC_setNSteps( Int32 deviceHandle,Int32 axis,Bln32 backward,Int32 n);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_setNSteps(int deviceHandle, int axis, int backward, int nStep);

        //Int32 NCB_API AMC_getNSteps( Int32 deviceHandle,Int32 axis,Int32* n);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getNSteps(int deviceHandle, int axis, out int nStep);

        //Int32 NCB_API AMC_controlContinousFwd( Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlContinousFwd(int deviceHandle, int axis, ref int enable, int set);

        //Int32 NCB_API AMC_controlContinousBkwd( Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlContinousBkwd(int deviceHandle, int axis, ref int enable, int set);

        /// <summary>
        /// 设置绝对目标位置
        /// </summary>
        /// <param name="deviceHandle"></param>
        /// <param name="axis"></param>
        /// <param name="target"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        //Int32 NCB_API AMC_controlTargetPosition( Int32 deviceHandle,Int32 axis,Int32* target,Bln32 set );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlTargetPosition(int deviceHandle, int axis, ref int target, int set);

        //Int32 NCB_API AMC_getStatusReference( Int32 deviceHandle,Int32 axis,Bln32* valid );Moving status, 0: Idle; 1: Moving; 2: Pending
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getStatusReference(int deviceHandle, int axis, out int valid);

        //Int32 NCB_API AMC_getStatusMoving( Int32 deviceHandle,Int32 axis,Int32* moving );Moving status, 0: Idle; 1: Moving; 2: Pending
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getStatusMoving(int deviceHandle, int axis, out int moving);

        //Int32 NCB_API AMC_getStatusConnected( Int32 deviceHandle,Int32 axis,Bln32* connected );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getStatusConnected(int deviceHandle, int axis, out int connected);

        //Int32 NCB_API AMC_getReferencePosition( Int32 deviceHandle,Int32 axis,Int32* reference );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getReferencePosition(int deviceHandle, int axis, out int reference);

        //Int32 NCB_API AMC_getPosition( Int32 deviceHandle,Int32 axis,Int32* position );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getPosition(int deviceHandle, int axis, out int position);

        //Int32 NCB_API AMC_controlReferenceAutoUpdate( Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlReferenceAutoUpdate(int deviceHandle, int axis, ref int enable, int set);

        //Int32 NCB_API AMC_controlAutoReset( Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlAutoReset(int deviceHandle, int axis, ref int enable, int set);

        /// <summary>
        /// 目标范围指与目标位置的误差值
        /// </summary>
        /// <param name="deviceHandle"></param>
        /// <param name="axis"></param>
        /// <param name="range"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        //Int32 NCB_API AMC_controlTargetRange( Int32 deviceHandle,Int32 axis,Int32* range,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlTargetRange(int deviceHandle, int axis, ref int range, int set);

        //Int32 NCB_API AMC_getStatusTargetRange( Int32 deviceHandle,Int32 axis,Bln32* target );
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getStatusTargetRange(int deviceHandle, int axis, out int target);

        //Int32 NCB_API AMC_getFirmwareVersion( Int32 deviceHandle,char* version,Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getFirmwareVersion(Int32 deviceHandle, out string version, int size);

        //Int32 NCB_API AMC_getFpgaVersion(Int32 deviceHandle,char* version,Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getFpgaVersion(Int32 deviceHandle, out string version, Int32 size);

        //Int32 NCB_API AMC_rebootSystem(Int32 deviceHandle);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_rebootSystem(int deviceHandle);

        //Int32 NCB_API AMC_factoryReset(Int32 deviceHandle);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_factoryReset(int deviceHandle);

        //Int32 NCB_API AMC_getMacAddress(Int32 deviceHandle, char* mac, Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getMacAddress(int deviceHandle, out string mac, int size);

        //Int32 NCB_API AMC_getIpAddress(Int32 deviceHandle, char* ip, Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getIpAddress(int deviceHandle, out string ip, int size);

        //Int32 NCB_API AMC_getDeviceType(Int32 deviceHandle, char* type, Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getDeviceType(int deviceHandle, out string type, int size);

        //Int32 NCB_API AMC_getSerialNumber(Int32 deviceHandle, char* sn, Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getSerialNumber(int deviceHandle, out string sn, int size);

        //Int32 NCB_API AMC_getDeviceName(Int32 deviceHandle, char* name, Int32 size);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getDeviceName(int deviceHandle, out string name, int size);

        //Int32 NCB_API AMC_setDeviceName(Int32 deviceHandle, const char* name);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_setDeviceName(int deviceHandle, string name);

        //Int32 NCB_API AMC_controlDeviceId( Int32 deviceHandle,Int32* id,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlDeviceId(int deviceHandle, ref int id, int set);

        //Int32 NCB_API AMC_getStatusEotFwd( Int32 deviceHandle,Int32 axis,Bln32* EotDetected);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getStatusEotFwd(int deviceHandle, int axis, out int EotDetected);

        //Int32 NCB_API AMC_getStatusEotBkwd( Int32 deviceHandle,Int32 axis,Bln32* EotDetected);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getStatusEotBkwd(int deviceHandle, int axis, out int EotDetected);

        //Int32 NCB_API AMC_controlEotOutputDeactive( Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlEotOutputDeactive(int deviceHandle, int axis, ref int enable, int set);


        //Int32 NCB_API AMC_controlFixOutputVoltage( Int32 deviceHandle,Int32 axis,Int32* voltage,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlFixOutputVoltage(int deviceHandle, int axis, ref int voltage, int set);


        //Int32 NCB_API AMC_controlAQuadBInResolution(Int32 deviceHandle,Int32 axis,Int32* resolution,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlAQuadBInResolution(int deviceHandle, int axis, ref int resolution, int set);

        //Int32 NCB_API AMC_controlAQuadBOut( Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlAQuadBOut(int deviceHandle, int axis, ref int enable, int set);

        //Int32 NCB_API AMC_controlAQuadBOutResolution( Int32 deviceHandle,Int32 axis,Int32* resolution,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlAQuadBOutResolution(int deviceHandle, int axis, ref int resolution, int set);

        //Int32 NCB_API AMC_controlAQuadBOutClock( Int32 deviceHandle,Int32 axis,Int32* clock,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlAQuadBOutClock(int deviceHandle, int axis, ref int clock, int set);

        //Int32 NCB_API AMC_setActorParametersByName(Int32 deviceHandle,Int32 axis,const char* actorname);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_setActorParametersByName(int deviceHandle, int axis, string actorname);

        //Int32 NCB_API AMC_setActorParameters(Int32 deviceHandle,Int32 axis,const char* actorName,Int32 actorType,Int32 fmax,Int32 amax,Bln32 sensor_dir,Bln32 actor_dir,Int32 pitchOfGrading,Int32 sensivity,Int32 stepsiz);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_setActorParameters(int deviceHandle,
                                     int axis,
                                     string actorName,
                                     int actorType,
                                     int fmax,
                                     int amax,
                                     int sensor_dir,
                                     int actor_dir,
                                     int pitchOfGrading,
                                     int sensivity,
                                     int stepsize
);

        //Int32 NCB_API AMC_setActorParametersByParamNameBoolean(Int32 deviceHandle,Int32 axis,char* paramName,Bln32 paramValue);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_setActorParametersByParamNameBoolean(int deviceHandle, int axis, ref string paramName, int paramValue);

        //Int32 NCB_API AMC_setActorParametersByParamName(Int32 deviceHandle,Int32 axis,char* paramName,Int32 paramValue);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_setActorParametersByParamName(int deviceHandle, int axis, ref string paramName, int paramValue);

        //Int32 NCB_API AMC_getActorParametersByParamName(Int32 deviceHandle,Int32 axis,char* paramName,char* paramValue,Int32 paramValueSize);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getActorParametersByParamName(Int32 deviceHandle,int axis,ref string paramName,ref string paramValue,int paramValueSize);


        //Int32 NCB_API AMC_getPositionersList(Int32 deviceHandle,char* list,Int32 listSize);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getPositionersList(Int32 deviceHandle,ref string list,int listSize);


        //Int32 NCB_API AMC_getActorParameters(Int32 deviceHandle,Int32 axis,char* actorname,size_t actorname_size,Int32* actorType,Int32* fmax,Int32* amax,Bln32* sensor_dir,Bln32* actor_dir,Int32* pitchOfGrading,Int32* sensivity,Int32* stepsiz);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_getActorParameters(int deviceHandle,
                                             int axis,
                                             out string actorname,
                                             int actorname_size,
                                             out int actorType,
                                             out int fmax,
                                             out int amax,
                                             out int sensor_dir,
                                             out int actor_dir,
                                             out int pitchOfGrading,
                                             out int sensivity,
                                             out int stepsize
        );


        //Int32 NCB_API AMC_controlRtOutSignalMode(Int32 deviceHandle,Int32* mode,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlRtOutSignalMode(int deviceHandle, ref int mode, int set);

        //Int32 NCB_API AMC_controlRealtimeInputMode(Int32 deviceHandle,Int32 axis,Int32* mode,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlRealtimeInputMode(int deviceHandle, int axis, ref int mode, int set);

        //Int32 NCB_API AMC_controlRealtimeInputLoopMode(Int32 deviceHandle,Int32 axis,Int32* mode,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlRealtimeInputLoopMode(int deviceHandle, int axis, ref int mode, int set);

        //Int32 NCB_API AMC_controlRealtimeInputChangePerPulse(Int32 deviceHandle,Int32 axis,Int32* change,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlRealtimeInputChangePerPulse(int deviceHandle, int axis, ref int change, int set);

        //Int32 NCB_API AMC_controlRealtimeInputStepsPerPulse(Int32 deviceHandle,Int32 axis,Int32* steps,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlRealtimeInputStepsPerPulse(int deviceHandle, int axis, ref int steps, int set);

        //Int32 NCB_API AMC_controlRealtimeInputMove(Int32 deviceHandle,Int32 axis,Bln32* enable,Bln32 set);
        [DllImport("amc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int AMC_controlRealtimeInputMove(int deviceHandle,int axis,ref int enable, int set);


    }
}
