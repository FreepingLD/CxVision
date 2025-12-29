//----------------------------------------------------------------------------- 
// <copyright file="ProfileData.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace LJV7_DllSampleAll.Datas
{
	/// <summary>
	/// Profile data class
	/// </summary>
	public class ProfileData
	{
		#region Field

		/// <summary>
		/// Trigger count
		/// </summary>
		private int _triggerCnt;

		/// <summary>
		/// Encoder count
		/// </summary>
		private int _encoderCnt;

		/// <summary>
		/// Profile data
		/// </summary>
		private int[] _profData;

		/// <summary>
		/// Profile information
		/// </summary>
		private LJV7IF_PROFILE_INFO _profileInfo;

		#endregion

		#region Property
		/// <summary>
		/// Trigger count
		/// </summary>
		public int TriggerCnt
		{
			get { return _triggerCnt; }
		}

		/// <summary>
		/// Encoder count
		/// </summary>
		public int EncodeCnt
		{
			get { return _encoderCnt; }
		}

		/// <summary>
		/// Profile Data
		/// </summary>
		public int[] ProfDatas
		{
			get { return _profData; }
		}

		/// <summary>
		/// Profile Imformation
		/// </summary>
		public LJV7IF_PROFILE_INFO ProfInfo
		{
			get { return _profileInfo; }
		}
		 #endregion

		#region Method
		/// <summary>
		/// Constructor
		/// </summary>
		public ProfileData(int[] receiveBuffer, LJV7IF_PROFILE_INFO profileInfo)
		{
			SetData(receiveBuffer, profileInfo);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="receiveBuffer">Receive buffer</param>
		/// <param name="startIndex">Start position</param>
		/// <param name="profileInfo">Profile information</param>
		public ProfileData(byte[] receiveBuffer, int startIndex, LJV7IF_PROFILE_INFO profileInfo)
		{
			int bufIntSize = CalculateDataSize(profileInfo);
			int[] bufIntArray = new int[bufIntSize];
			_profileInfo = profileInfo;

			// Conversion from byte[] to int[]
			for (int i = 0; i < bufIntSize; i++)
			{
				bufIntArray[i] = BitConverter.ToInt32(receiveBuffer, (startIndex + i *  Marshal.SizeOf(typeof(int))));
			}
			SetData(bufIntArray, profileInfo);
		}

		/// <summary>
		/// Constructor Overload
		/// </summary>
		/// <param name="receiveBuffer">Receive buffer</param>
		/// <param name="startIndex">Start position</param>
		/// <param name="profileInfo">Profile information</param>
		public ProfileData(int[] receiveBuffer, int startIndex, LJV7IF_PROFILE_INFO profileInfo)
		{
			int bufIntSize = CalculateDataSize(profileInfo);
			int[] bufIntArray = new int[bufIntSize];
			_profileInfo = profileInfo;

			Array.Copy(receiveBuffer, startIndex, bufIntArray, 0, bufIntSize);
			SetData(bufIntArray, profileInfo);
		}

		/// <summary>
		/// Set the members to the arguments.
		/// </summary>
		/// <param name="receiveBuffer">Receive buffer</param>
		/// <param name="profileInfo">Profile information</param>
		private void SetData(int[] receiveBuffer, LJV7IF_PROFILE_INFO profileInfo)
		{
			_profileInfo = profileInfo;

			// Extract the header.
			int headerSize = Utility.GetByteSize(Utility.TypeOfStruct.PROFILE_HEADER) /  Marshal.SizeOf(typeof(int));
			int[] headerData = new int[headerSize];
			Array.Copy(receiveBuffer, 0, headerData, 0, headerSize);
			_triggerCnt = headerData[1];
			_encoderCnt = headerData[2];

			// Extract the footer.
			int footerSize = Utility.GetByteSize(Utility.TypeOfStruct.PROFILE_FOOTER) /  Marshal.SizeOf(typeof(int));
			int[] footerData = new int[footerSize];
			Array.Copy(receiveBuffer, receiveBuffer.Length - footerSize, footerData, 0, footerSize);

			// Extract the profile data.
			int profSize = receiveBuffer.Length - headerSize - footerSize;
			_profData = new int[profSize];
			Array.Copy(receiveBuffer, headerSize, _profData, 0, profSize);
		}

		/// <summary>
		/// Get the data string.
		/// </summary>
		/// <returns>Retained data</returns>
		public StringBuilder GetStringData()
		{
			StringBuilder retString = new StringBuilder();

			retString.AppendLine(string.Format(@"Trigger count	:{0}", _triggerCnt));
			retString.AppendLine(string.Format(@"Encoder count:{0}", _encoderCnt));

			return retString;
		}

		/// <summary>
		/// Data size calculation
		/// </summary>
		/// <param name="profileInfo">Profile information</param>
		/// <returns>Profile data size</returns>
		static public int CalculateDataSize(LJV7IF_PROFILE_INFO profileInfo)
		{
			LJV7IF_PROFILE_HEADER header = new LJV7IF_PROFILE_HEADER();
			LJV7IF_PROFILE_FOOTER footer = new LJV7IF_PROFILE_FOOTER();
			
			return profileInfo.wProfDataCnt * profileInfo.byProfileCnt * (profileInfo.byEnvelope + 1) +
				(Marshal.SizeOf(header) + Marshal.SizeOf(footer)) /  Marshal.SizeOf(typeof(int));
		}

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>String for display</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			// Data position calculation
			double posX = ProfInfo.lXStart;
			double deltaX = ProfInfo.lXPitch;

			int singleProfileCount = ProfInfo.wProfDataCnt;
			int dataCount = (int)ProfInfo.byProfileCnt * (ProfInfo.byEnvelope + 1);

			for (int i = 0; i < singleProfileCount; i++)
			{
				sb.AppendFormat("{0}\t", posX + deltaX * i);
				for (int j = 0; j < dataCount; j++)
				{
					sb.AppendFormat("{0}\t", _profData[i + ProfInfo.wProfDataCnt * j]);
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		/// <summary>
		/// Create the X-position string from the profile information.
		/// </summary>
		/// <param name="profileInfo">Profile information</param>
		/// <returns>X-position string</returns>
		public static string GetXPosString(LJV7IF_PROFILE_INFO profileInfo)
		{
			StringBuilder sb = new StringBuilder();
			// Data position calculation
			double posX = profileInfo.lXStart;
			double deltaX = profileInfo.lXPitch;

			int singleProfileCount = profileInfo.wProfDataCnt;
			int dataCount = (int)profileInfo.byProfileCnt * (profileInfo.byEnvelope + 1);

			for (int i = 0; i < dataCount; i++)
			{
				for (int j = 0; j < singleProfileCount; j++)
				{
					sb.AppendFormat("{0}\t", (posX + deltaX * j));
				}
			}
			return sb.ToString();
		}

		#endregion



	}
}
