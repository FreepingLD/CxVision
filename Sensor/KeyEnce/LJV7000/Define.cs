//----------------------------------------------------------------------------- 
// <copyright file="Define.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
namespace LJV7_DllSampleAll
{
	/// <summary>
	/// Constant class
	/// </summary>
	public static class Define
	{
		#region Constant

		/// <summary>
		/// Maximum amount of data for 1 profile
		/// </summary>
		public const int MAX_PROFILE_COUNT = 3200;

		/// <summary>
		/// Device ID (fixed to 0)
		/// </summary>
		public const int DEVICE_ID = 0;
		
		/// <summary>
		/// Size of data for sending and getting settings
		/// </summary>
		public const int WRITE_DATA_SIZE = 20 * 1024;

		/// <summary>
		/// Upper limit for the size of data to get
		/// </summary>
		public const int READ_DATA_SIZE = 1024 * 1024;

		/// <summary>
		/// Maximum amount of profile data to retain
		/// </summary>
		public const int PROFILE_DATA_MAX = 10;

		/// <summary>
		/// Measurement range X direction
		/// </summary>
		public const int MEASURE_RANGE_FULL = 800;
		public const int MEASURE_RANGE_MIDDLE = 600;
		public const int MEASURE_RANGE_SMALL = 400;

		/// <summary>
		/// Light reception characteristic
		/// </summary>
		public const int RECEIVED_BINNING_OFF = 1;
		public const int RECEIVED_BINNING_ON = 2;

		public const int COMPRESS_X_OFF = 1;
		public const int COMPRESS_X_2 = 2;
		public const int COMPRESS_X_4 = 4;
		/// <summary>
		/// Default name to use when exporting profiles
		/// </summary>
		public const string DEFAULT_PROFILE_FILE_NAME = @"ReceiveData_CS.txt";

		/// <summary>
		/// Unit conversion factor (mm) for profile values
		/// </summary>
		public const double PROFILE_UNIT_MM = 1E-5;

		#endregion
	}	
}
