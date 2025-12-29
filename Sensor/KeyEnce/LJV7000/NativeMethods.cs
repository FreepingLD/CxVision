//----------------------------------------------------------------------------- 
// <copyright file="NativeMethods.cs.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System;
using System.Runtime.InteropServices;

namespace LJV7_DllSampleAll
{
	#region Enum

	/// <summary>
	/// Return value definition
	/// </summary>
	public enum Rc
	{
		/// <summary>Normal termination</summary>
		Ok = 0x0000,
		/// <summary>Failed to open the device</summary>
		ErrOpenDevice = 0x1000,
		/// <summary>Device not open</summary>
		ErrNoDevice,
		/// <summary>Command send error</summary>
		ErrSend,
		/// <summary>Response reception error</summary>
		ErrReceive,
		/// <summary>Timeout</summary>
		ErrTimeout,
		/// <summary>No free space</summary>
		ErrNomemory,
		/// <summary>Parameter error</summary>
		ErrParameter,
		/// <summary>Received header format error</summary>
		ErrRecvFmt,

		/// <summary>Not open error (for high-speed communication)</summary>
		ErrHispeedNoDevice = 0x1009,
		/// <summary>Already open error (for high-speed communication)</summary>
		ErrHispeedOpenYet,
		/// <summary>Already performing high-speed communication error (for high-speed communication)</summary>
		ErrHispeedRecvYet,
		/// <summary>Insufficient buffer size</summary>
		ErrBufferShort,
	}

	/// <summary>
	/// Definition that indicates the validity of a measurement value
	/// </summary>
	public enum LJV7IF_MEASURE_DATA_INFO
	{
		LJV7IF_MEASURE_DATA_INFO_VALID = 0x00,	// Valid
		LJV7IF_MEASURE_DATA_INFO_ALARM = 0x01,	// Alarm value
		LJV7IF_MEASURE_DATA_INFO_WAIT = 0x02	// Judgment wait value
	}

	/// <summary>
	/// Definition that indicates the tolerance judgment result of the measurement
	/// </summary>
	public enum LJV7IF_JUDGE_RESULT
	{
		LJV7IF_JUDGE_RESULT_HI = 0x01,	// HI
		LJV7IF_JUDGE_RESULT_GO = 0x02,	// GO
		LJV7IF_JUDGE_RESULT_LO = 0x04	// LO
	}

	/// Get batch profile position specification method designation
	public enum BatchPos : byte
	{
		/// <summary>From current</summary>
		Current = 0x00,
		/// <summary>Specify position</summary>
		Spec = 0x02,
		/// <summary>From current after commitment</summary>
		Commited = 0x03,
		/// <summary>Current only</summary>
		CurrentOnly = 0x04,
	};

	/// Setting value storage level designation
	public enum SettingDepth : byte
	{
		/// <summary>Settings write area</summary>
		Write = 0x00,
		/// <summary>Active measurement area</summary>
		Running = 0x01,
		/// <summary>Save area</summary>
		Save = 0x02,
	};

	/// Definition that indicates the "setting type" in LJV7IF_TARGET_SETTING structure.
	public enum SettingType : byte
	{
		/// <summary>Environment setting</summary>
		Environment = 0x01,
		/// <summary>Common measurement setting</summary>
		Common = 0x02,
		/// <summary>Measurement Program setting</summary>
		Program00 = 0x10,
		Program01,
		Program02,
		Program03,
		Program04,
		Program05,
		Program06,
		Program07,
		Program08,
		Program09,
		Program10,
		Program11,
		Program12,
		Program13,
		Program14,
		Program15,
	};

	/// Get profile target buffer designation
	public enum ProfileBank : byte
	{
		/// <summary>Active surface</summary>
		Active = 0x00,
		/// <summary>Inactive surface</summary>	
		Inactive = 0x01,
	};

	/// Get profile position specification method designation
	public enum ProfilePos : byte
	{
		/// <summary>From current</summary>
		Current = 0x00,
		/// <summary>From oldest</summary>
		Oldest = 0x01,
		/// <summary>Specify position</summary>
		Spec = 0x02,
	};

	#endregion

	#region Structure
	/// <summary>
	/// Ethernet settings structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_ETHERNET_CONFIG
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] abyIpAddress;
		public ushort wPortNo;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;

	};

	/// <summary>
	/// Date and time structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_TIME
	{
		public byte byYear;
		public byte byMonth;
		public byte byDay;
		public byte byHour;
		public byte byMinute;
		public byte bySecond;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
	};

	/// <summary>
	/// Setting item designation structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_TARGET_SETTING
	{
		public byte byType;
		public byte byCategory;
		public byte byItem;
		public byte reserve;
		public byte byTarget1;
		public byte byTarget2;
		public byte byTarget3;
		public byte byTarget4;
	};

	/// <summary>
	/// Measurement results structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_MEASURE_DATA
	{
		public byte byDataInfo;
		public byte byJudge;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public float fValue;
	};

	/// <summary>
	/// Profile information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_PROFILE_INFO
	{
		public byte byProfileCnt;
		public byte byEnvelope;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public short wProfDataCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
		public int lXStart;
		public int lXPitch;
	};

	/// <summary>
	/// Profile header information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_PROFILE_HEADER
	{
		public uint reserve;
		public uint dwTriggerCnt;
		public uint dwEncoderCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] reserve2;
	};

	/// <summary>
	/// Profile footer information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_PROFILE_FOOTER
	{
		public uint reserve;
	};

	/// <summary>
	/// High-speed mode get profile request structure (batch measurement: off)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_PROFILE_REQ
	{
		public byte byTargetBank;
		public byte byPosMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public uint dwGetProfNo;
		public byte byGetProfCnt;
		public byte byErase;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
	};

	/// <summary>
	/// High-speed mode get profile request structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_REQ
	{
		public byte byTargetBank;
		public byte byPosMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public uint dwGetBatchNo;
		public uint dwGetProfNo;
		public byte byGetProfCnt;
		public byte byErase;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
	};

	/// <summary>
	/// Advanced mode get profile request structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_ADVANCE_REQ
	{
		public byte byPosMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
		public uint dwGetBatchNo;
		public uint dwGetProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve2;
	};

	/// <summary>
	/// High-speed mode get profile response structure (batch measurement: off)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_PROFILE_RSP
	{
		public uint dwCurrentProfNo;
		public uint dwOldestProfNo;
		public uint dwGetTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
	};

	/// <summary>
	/// High-speed mode get profile response structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_RSP
	{
		public uint dwCurrentBatchNo;
		public uint dwCurrentBatchProfCnt;
		public uint dwOldestBatchNo;
		public uint dwOldestBatchProfCnt;
		public uint dwGetBatchNo;
		public uint dwGetBatchProfCnt;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		public byte byCurrentBatchCommited;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
	};

	/// <summary>
	/// Advanced mode get profile response structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP
	{
		public uint dwGetBatchNo;
		public uint dwGetBatchProfCnt;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
	};

	/// <summary>
	/// Storage status request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STRAGE_STATUS_REQ
	{
		public uint dwRdArea;		// Target surface to read
	};

	/// <summary>
	/// Storage status response structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STRAGE_STATUS_RSP
	{
		public uint dwSurfaceCnt;		// Storage surface number
		public uint dwActiveSurface;	// Active storage surface
	};

	/// <summary>
	/// Storage information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_STORAGE_INFO
	{
		public byte byStatus;
		public byte byProgramNo;
		public byte byTarget;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] reserve;
		public uint dwStorageCnt;
	};

	/// <summary>
	/// Get storage data request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STORAGE_REQ
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] reserve;
		public uint dwSurface;
		public uint dwStartNo;
		public uint dwDataCnt;
	};

	/// <summary>
	/// Get batch profile storage request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_STORAGE_REQ
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] reserve;
		public uint dwSurface;
		public uint dwGetBatchNo;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserved;
	};

	/// <summary>
	/// Get storage data response structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STORAGE_RSP
	{
		public uint dwStartNo;
		public uint dwDataCnt;
		public LJV7IF_TIME stBaseTime;
	};
	/// <summary>
	/// Get batch profile storage response structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP
	{
		public uint dwGetBatchNo;
		public uint dwGetBatchProfCnt;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
		public LJV7IF_TIME stBaseTime;
	};

	/// <summary>
	/// High-speed communication start preparation request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_HIGH_SPEED_PRE_START_REQ
	{
		public byte bySendPos;		// Send start position
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;		// Reservation 
	};

	#endregion

	#region Method
	/// <summary>
	/// Callback function for high-speed communication
	/// </summary>
	/// <param name="buffer">Received profile data pointer</param>
	/// <param name="size">Size in units of bytes of one profile</param>
	/// <param name="count">Number of profiles</param>
	/// <param name="notify">Finalization condition</param>
	/// <param name="user">Thread ID</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void HighSpeedDataCallBack(IntPtr buffer, uint size, uint count, uint notify, uint user);

	/// <summary>
	/// Function definitions
	/// </summary>
	internal class NativeMethods
	{
		/// <summary>
		/// Get measurement results (the data of all 16 OUTs, including those that are not being measured, is stored).
		/// </summary>
		internal static int MeasurementDataCount
		{
			get { return 16; }
		}

		/// <summary>
		/// Number of connectable devices
		/// </summary>
		internal static int DeviceCount
		{
			get { return 6; }
		}
		
		/// <summary>
		/// Fixed value for the bytes of environment settings data 
		/// </summary>
		internal static UInt32 EnvironmentSettingSize
		{
			get { return 60; }
		}

		/// <summary>
		/// Fixed value for the bytes of common measurement settings data 
		/// </summary>
		internal static UInt32 CommonSettingSize
		{
			get { return 12; }
		}

		/// <summary>
		/// Fixed value for the bytes of program settings data 
		/// </summary>
		internal static UInt32 ProgramSettingSize
		{
			get { return 10932; }
		}

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_Initialize();

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_Finalize();

		[DllImport("LJV7_IF.dll")]
		internal static extern uint LJV7IF_GetVersion();

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_UsbOpen(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_EthernetOpen(int lDeviceId, ref LJV7IF_ETHERNET_CONFIG ethernetConfig);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_CommClose(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_RebootController(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_RetrunToFactorySetting(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetError(int lDeviceId, byte byRcvMax, ref byte pbyErrCnt, IntPtr pwErrCode);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_ClearError(int lDeviceId, short wErrCode);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_Trigger(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_StartMeasure(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_StopMeasure(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_AutoZero(int lDeviceId, byte byOnOff, uint dwOut);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_Timing(int lDeviceId, byte byOnOff, uint dwOut);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_Reset(int lDeviceId, uint dwOut);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_ClearMemory(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_SetSetting(int lDeviceId, byte byDepth, LJV7IF_TARGET_SETTING TargetSetting, IntPtr pData, uint dwDataSize, ref uint pdwError);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetSetting(int lDeviceId, byte byDepth, LJV7IF_TARGET_SETTING TargetSetting, IntPtr pData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_InitializeSetting(int lDeviceId, byte byDepth, byte byTarget);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_ReflectSetting(int lDeviceId, byte byDepth, ref uint pdwError);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_RewriteTemporarySetting(int lDeviceId, byte byDepth);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_CheckMemoryAccess(int lDeviceId, ref byte pbyBusy);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_SetTime(int lDeviceId, ref LJV7IF_TIME time);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetTime(int lDeviceId, ref LJV7IF_TIME time);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_ChangeActiveProgram(int lDeviceId, byte byProgNo);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetActiveProgram(int lDeviceId, ref byte pbyProgNo);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetMeasurementValue(int lDeviceId, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetProfile(int lDeviceId, ref LJV7IF_GET_PROFILE_REQ pReq,
		ref LJV7IF_GET_PROFILE_RSP pRsp, ref LJV7IF_PROFILE_INFO pProfileInfo, IntPtr pdwProfileData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetBatchProfile(int lDeviceId, ref LJV7IF_GET_BATCH_PROFILE_REQ pReq,
		ref LJV7IF_GET_BATCH_PROFILE_RSP pRsp, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwBatchData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetProfileAdvance(int lDeviceId, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwProfileData, uint dwDataSize, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetBatchProfileAdvance(int lDeviceId, ref LJV7IF_GET_BATCH_PROFILE_ADVANCE_REQ pReq,
		ref LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP pRsp, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwBatchData, uint dwDataSize, [Out]LJV7IF_MEASURE_DATA[] pBatchMeasureData, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_StartStorage(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_StopStorage(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetStorageStatus(int lDeviceId, ref LJV7IF_GET_STRAGE_STATUS_REQ pReq,
		ref LJV7IF_GET_STRAGE_STATUS_RSP pRsp, ref LJV7IF_STORAGE_INFO pStorageInfo);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetStorageData(int lDeviceId, ref LJV7IF_GET_STORAGE_REQ pReq,
		ref LJV7IF_STORAGE_INFO pStorageInfo, ref LJV7IF_GET_STORAGE_RSP pRsp, IntPtr pdwData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetStorageProfile(int lDeviceId, ref LJV7IF_GET_STORAGE_REQ pReq,
		ref LJV7IF_STORAGE_INFO pStorageInfo, ref LJV7IF_GET_STORAGE_RSP pRes,
		ref LJV7IF_PROFILE_INFO pProfileInfo, IntPtr pdwData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_GetStorageBatchProfile(int lDeviceId,
		ref LJV7IF_GET_BATCH_PROFILE_STORAGE_REQ pReq, ref LJV7IF_STORAGE_INFO pStorageInfo,
		ref LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP pRes, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwData, uint dwDataSize, ref uint pTimeOffset, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_HighSpeedDataUsbCommunicationInitalize(int lDeviceId, HighSpeedDataCallBack pCallBack,
		uint dwProfileCnt, uint dwThreadId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_HighSpeedDataEthernetCommunicationInitalize(
		int lDeviceId, ref LJV7IF_ETHERNET_CONFIG pEthernetConfig, ushort wHighSpeedPortNo,
		HighSpeedDataCallBack pCallBack, uint dwProfileCnt, uint dwThreadId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_PreStartHighSpeedDataCommunication(
		int lDeviceId, ref LJV7IF_HIGH_SPEED_PRE_START_REQ pReq,
		ref LJV7IF_PROFILE_INFO pProfileInfo);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_StartHighSpeedDataCommunication(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_StopHighSpeedDataCommunication(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		internal static extern int LJV7IF_HighSpeedDataCommunicationFinalize(int lDeviceId);
	}
	#endregion

}
