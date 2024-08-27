//----------------------------------------------------------------------------- 
// <copyright file="Utility.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace LJV7_DllSampleAll
{
	/// <summary>
	/// Utility class
	/// </summary>
	static class Utility
	{
		#region Enum
		/// <summary>
		/// Structure classification
		/// </summary>
		public enum TypeOfStruct
		{
			PROFILE_HEADER,
			PROFILE_FOOTER,
			MEASURE_DATA
		}
		#endregion
		
		#region Constant
		/// <summary>
		/// Storage structure (storage status)
		/// </summary>
		private const int STORAGE_INFO_STATUS_EMPTY = 0;
		private const int STORAGE_INFO_STATUS_STORING = 1;
		private const int STORAGE_INFO_STATUS_FINISHED = 2;

		/// <summary>
		/// Storage structure (storage target)
		/// </summary>
		private const int STORAGE_INFO_TARGET_DATA = 0;
		private const int STORAGE_INFO_TARGET_PROFILE = 2;
		private const int STORAGE_INFO_TARGET_BATCH = 3;
		#endregion

		#region Method

		#region Get the byte size

		/// <summary>
		/// Get the byte size of the structure.
		/// </summary>
		/// <param name="profileHeader">Structure whose byte size you want to get.</param>
		/// <returns>Byte size</returns>
		public static int GetByteSize(TypeOfStruct type)
		{
			switch (type)
			{
				case TypeOfStruct.PROFILE_HEADER:
					LJV7IF_PROFILE_HEADER profileHeader = new LJV7IF_PROFILE_HEADER();
					return Marshal.SizeOf(profileHeader);

				case TypeOfStruct.PROFILE_FOOTER:
					LJV7IF_PROFILE_FOOTER profileFooter = new LJV7IF_PROFILE_FOOTER();
					return Marshal.SizeOf(profileFooter);

				case TypeOfStruct.MEASURE_DATA:
					LJV7IF_MEASURE_DATA measureData = new LJV7IF_MEASURE_DATA();
					return Marshal.SizeOf(measureData);
			}

			return 0;
		}
		#endregion

		#region Acquisition of log 

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_STORAGE_INFO storageInfo)
		{
			StringBuilder sb = new StringBuilder();

			string status = string.Empty;
			switch (storageInfo.byStatus)
			{
				case STORAGE_INFO_STATUS_EMPTY:
					status = @"EMPTY";
					break;
				case STORAGE_INFO_STATUS_STORING:
					status = @"STORING";
					break;
				case STORAGE_INFO_STATUS_FINISHED:
					status = @"FINISHED";
					break;
				default:
					status = @"UNEXPECTED";
					break;
			}
			sb.AppendLine(string.Format(@"  Status		: {0}", status));
			sb.AppendLine(string.Format(@"  ProgNo		: {0}", storageInfo.byProgramNo));
			string target = string.Empty; ;
			switch (storageInfo.byTarget)
			{
				case STORAGE_INFO_TARGET_DATA:
					target = @"DATA";
					break;
				case STORAGE_INFO_TARGET_PROFILE:
					target = @"PROFILE";
					break;
				case STORAGE_INFO_TARGET_BATCH:
					target = @"BATCH PROFILE";
					break;
				default:
					target = @"UNEXPECTED";
					break;
			}
			sb.AppendLine(string.Format(@"  Target		: {0}", target));
			sb.Append(string.Format(@"  StorageCnt	: {0}", storageInfo.dwStorageCnt));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_GET_STORAGE_RSP storageRsp)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format(@"  StartNo	: {0}", storageRsp.dwStartNo));
			sb.AppendLine(string.Format(@"  DataCnt	: {0}", storageRsp.dwDataCnt));
			sb.Append(ConvertToLogString(storageRsp.stBaseTime).ToString());

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_MEASURE_DATA measureData)
		{
			StringBuilder sb = new StringBuilder();

			string dataInfo = string.Empty;
			switch (measureData.byDataInfo)
			{
				case (int)LJV7IF_MEASURE_DATA_INFO.LJV7IF_MEASURE_DATA_INFO_VALID:
					dataInfo = @"Valid		";
					break;
				case (int)LJV7IF_MEASURE_DATA_INFO.LJV7IF_MEASURE_DATA_INFO_ALARM:
					dataInfo = @"Alarm value  ";
					break;
				case (int)LJV7IF_MEASURE_DATA_INFO.LJV7IF_MEASURE_DATA_INFO_WAIT:
					dataInfo = @"Judgment wait value  ";
					break;
				default:
					dataInfo = @"Unexpected value	";
					break;
			}
			sb.Append(dataInfo);
			string judge = string.Empty;
			switch (measureData.byJudge)
			{
				case 0:
					judge = @"______  ";
					break;
				case (int)LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_HI:
					judge = @"HI____  ";
					break;
				case (int)LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_GO:
					judge = @"__GO__  ";
					break;
				case (int)LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_LO:
					judge = @"____LO  ";
					break;
				case (int)(LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_HI | LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_LO):
					judge = @"HI__LO  ";
					break;
				case (int)(LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_HI
					| LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_GO
					| LJV7IF_JUDGE_RESULT.LJV7IF_JUDGE_RESULT_LO):
					judge = @"ALL BIT  ";
					break;
				default:
					judge = @"UNEXPECTED ";
					break;
			}
			sb.Append(judge);
			sb.Append(measureData.fValue.ToString());

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_PROFILE_INFO profileInfo)
		{
			StringBuilder sb = new StringBuilder();

			// Profile information of the profile obtained
			sb.AppendLine(string.Format(@"  Profile Data Num			: {0}", profileInfo.byProfileCnt));
			string envelope = profileInfo.byEnvelope == 0
				? @"OFF"
				: @"ON";
			sb.AppendLine(string.Format(@"  Envelope			: {0}", envelope));
			sb.AppendLine(string.Format(@"  Profile Data Points			: {0}", profileInfo.wProfDataCnt));
			sb.AppendLine(string.Format(@"  X coordinate of the first point	: {0}", profileInfo.lXStart));
			    sb.Append(string.Format(@"  X-direction interval		: {0}", profileInfo.lXPitch));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP rsp)
		{
			StringBuilder sb = new StringBuilder();

			// Profile information of the profile obtained
			sb.AppendLine(string.Format(@"  BatchNo		 : {0}", rsp.dwGetBatchNo));
			sb.AppendLine(string.Format(@"  BatchProfCnt	 : {0}", rsp.dwGetBatchProfCnt));
			sb.AppendLine(string.Format(@"  BatchTopProfNo	 : {0}", rsp.dwGetBatchTopProfNo));
			sb.AppendLine(string.Format(@"  ProfCnt		 : {0}", rsp.byGetProfCnt));
			sb.Append(ConvertToLogString(rsp.stBaseTime).ToString());

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_GET_BATCH_PROFILE_RSP rsp)
		{
			StringBuilder sb = new StringBuilder();

			// Profile information of the profile obtained
			sb.AppendLine(string.Format(@"  CurrentBatchNo			: {0}", rsp.dwCurrentBatchNo));
			sb.AppendLine(string.Format(@"  CurrentBatchProfCnt		: {0}", rsp.dwCurrentBatchProfCnt));
			sb.AppendLine(string.Format(@"  OldestBatchNo			: {0}", rsp.dwOldestBatchNo));
			sb.AppendLine(string.Format(@"  OldestBatchProfCnt		: {0}", rsp.dwOldestBatchProfCnt));
			sb.AppendLine(string.Format(@"  GetBatchNo			: {0}", rsp.dwGetBatchNo));
			sb.AppendLine(string.Format(@"  GetBatchProfCnt			: {0}", rsp.dwGetBatchProfCnt));
			sb.AppendLine(string.Format(@"  GetBatchTopProfNo		: {0}", rsp.dwGetBatchTopProfNo));
			sb.AppendLine(string.Format(@"  GetProfCnt			: {0}", rsp.byGetProfCnt));
			sb.Append(string.Format    (@"  CurrentBatchCommited		: {0}", rsp.byCurrentBatchCommited));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP rsp)
		{
			StringBuilder sb = new StringBuilder();

			// Profile information of the profile obtained
			sb.AppendLine(string.Format(@"  GetBatchNo		: {0}", rsp.dwGetBatchNo));
			sb.AppendLine(string.Format(@"  GetBatchProfCnt		: {0}", rsp.dwGetBatchProfCnt));
			sb.AppendLine(string.Format(@"  GetBatchTopProfNo	: {0}", rsp.dwGetBatchTopProfNo));
			sb.Append(string.Format    (@"  GetProfCnt		: {0}", rsp.byGetProfCnt));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_TIME time)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(
					string.Format("yy/mm/dd hh:mm:ss \n {0,0:d2}/{1,0:d2}/{2,0:d2} {3,0:d2}:{4,0:d2}:{5,0:d2}",
					time.byYear, time.byMonth, time.byDay,
					time.byHour, time.byMinute, time.bySecond));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_GET_PROFILE_RSP rsp)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format(@"  CurrentProfNo	: {0}", rsp.dwCurrentProfNo));
			sb.AppendLine(string.Format(@"  OldestProfNo	: {0}", rsp.dwOldestProfNo));
			sb.AppendLine(string.Format(@"  GetTopProfNo	: {0}", rsp.dwGetTopProfNo));
			sb.Append(string.Format(@"  GetProfCnt	: {0}", rsp.byGetProfCnt));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="obj">Structure that you want to convert to a string</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertToLogString(LJV7IF_GET_STRAGE_STATUS_RSP rsp)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format(@"  SurfaceCnt	: {0}", rsp.dwSurfaceCnt));
			sb.AppendLine(string.Format(@"  ActiveSurface	: {0}", rsp.dwActiveSurface));

			return sb;
		}
		#endregion

		#endregion
	}
}
