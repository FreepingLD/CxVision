//----------------------------------------------------------------------------- 
// <copyright file="MeasureData.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace LJV7_DllSampleAll.Datas
{
	/// <summary>
	/// Measurement data class
	/// </summary>
	public class MeasureData
	{
		#region Field
		/// <summary>
		/// Elapsed time(ms)
		/// </summary>
		private uint _offsetTime;

		/// <summary>
		/// Measurement results
		/// </summary>
		private LJV7IF_MEASURE_DATA[] _data;
		#endregion

		#region Property
		/// <summary>
		/// Elapsed time(ms)
		/// </summary>
		public uint OffsetTime { get { return _offsetTime; } set { _offsetTime = value; } }

		/// <summary>
		/// Measurement results
		/// </summary>
		public LJV7IF_MEASURE_DATA[] Data { get { return _data; } }
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">Measurement results</param>
		public MeasureData(LJV7IF_MEASURE_DATA[] data)
		{
			_data = data;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">Measurement results</param>
		public MeasureData(byte[] data)
		{
			_offsetTime = 0;
			int measureDataIndex = 0;
			_data = new LJV7IF_MEASURE_DATA[NativeMethods.MeasurementDataCount];
			int measureDataSize = Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA);

			for (int i = 0; i < NativeMethods.MeasurementDataCount; i++)
			{
				_data[i].byDataInfo = data[measureDataIndex + 0];
				_data[i].byJudge = data[measureDataIndex + 1];
				_data[i].fValue = BitConverter.ToSingle(data, (measureDataIndex + 4));
				measureDataIndex += measureDataSize;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="offsetTime">Elapsed time</param>
		/// <param name="data">Measurement results</param>
		public MeasureData(uint offsetTime, LJV7IF_MEASURE_DATA[] data)
		{
			_offsetTime = offsetTime;
			_data = data;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="offsetTime">Elapsed time</param>
		/// <param name="data">Start position</param>
		public MeasureData(byte[] data, int startIndex)
		{
			_offsetTime = BitConverter.ToUInt32(data, startIndex);
			int measureDataIndex = startIndex + Marshal.SizeOf(typeof(uint));
			_data = new LJV7IF_MEASURE_DATA[NativeMethods.MeasurementDataCount];
			int measureDataSize = Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA);

			for (int i = 0; i < NativeMethods.MeasurementDataCount; i++)
			{				
				_data[i].byDataInfo = data[measureDataIndex + 0];
				_data[i].byJudge = data[measureDataIndex + 1];
				_data[i].fValue = BitConverter.ToSingle(data, (measureDataIndex + 4));
				measureDataIndex += measureDataSize;
			}
		}

		#endregion

		#region Method
		/// <summary>
		/// Size acquisition
		/// </summary>
		/// <returns>Data size</returns>
		public static int GetByteSize()
		{
			return (Marshal.SizeOf(typeof(uint)) + Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA) * NativeMethods.MeasurementDataCount);
		}

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>String for display</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < NativeMethods.MeasurementDataCount; i++)
			{
				sb.Append(string.Format("{0,0:f4}\t", _data[i].fValue));
			}

			return sb.ToString();
		}
		#endregion
	}
}
