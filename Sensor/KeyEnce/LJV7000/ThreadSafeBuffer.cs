//----------------------------------------------------------------------------- 
// <copyright file="ThreadSafeBuffer.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System.Collections.Generic;
using System;

namespace LJV7_DllSampleAll
{
	/// <summary>
	/// Thread-safe class for array storage
	/// </summary>
	public  class ThreadSafeBuffer
	{
		#region Field
		/// <summary>Data buffer</summary>
		private  List<int[]>[] _buffer = new List<int[]>[NativeMethods.DeviceCount];
		/// <summary>Buffer for the amount of data</summary>
		private  uint[] _count = new uint[NativeMethods.DeviceCount];
		/// <summary>Object for exclusive control</summary>
		private  object[] _syncObject = new object[NativeMethods.DeviceCount];
		/// <summary>Callback function notification parameter</summary>
		private  uint[] _notify = new uint[NativeMethods.DeviceCount];
		/// <summary>Batch number</summary>
		private  int[] _batchNo = new int[NativeMethods.DeviceCount];
		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public ThreadSafeBuffer()
		{
			for (int i = 0; i < NativeMethods.DeviceCount; i++)
			{
				_buffer[i] = new List<int[]>();
				_syncObject[i] = new object();
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// Element addition
		/// </summary>
		/// <param name="index">User information set when high-speed communication was initialized</param>
		/// <param name="value">Additional element</param>
		/// <param name="notify">Parameter for notification</param>
		public  void Add(int index, List<int[]> value, uint notify)
		{
			lock (_syncObject[index])
			{
				_buffer[index].AddRange(value);
				_count[index] += (uint)value.Count;
				_notify[index] |= notify;
				// Add the batch number if the batch has been finalized.
				if ((notify & (0x1 << 16)) != 0) _batchNo[index]++;
			}
		}

		/// <summary>
		/// Clear elements.
		/// </summary>
		/// <param name="index">Device ID</param>
		public  void Clear(int _currentDeviceId)
		{
			lock (_syncObject[_currentDeviceId])
			{
				_buffer[_currentDeviceId].Clear();
			}
		}

		/// <summary>
		/// Clear the number of elements.
		/// </summary>
		/// <param name="index">Device ID</param>
		public  void ClearCount(int index)
		{
			lock (_syncObject[index])
			{
				_count[index] = 0;
			}
		}

		/// <summary>
		/// Clear notifications.
		/// </summary>
		/// <param name="index">Device ID</param>
		public  void ClearNotify(int index)
		{
			lock (_syncObject[index])
			{
				_notify[index] = 0;
			}
		}

		/// <summary>
		/// Clear elements.
		/// </summary>
		public  void ClearAll()
		{
			for (int i = 0; i < NativeMethods.DeviceCount; i++)
			{
				ClearBuffer(i);
			}
		}

		/// <summary>
		/// Clear the buffer.
		/// </summary>
		/// <param name="_currentDeviceId">Device ID</param>
		public  void ClearBuffer(int _currentDeviceId)
		{
			Clear(_currentDeviceId);
			ClearCount(_currentDeviceId);
			_batchNo[_currentDeviceId] = 0;
			ClearNotify(_currentDeviceId);
		}

		/// <summary>
		/// Get element.
		/// </summary>
		/// <param name="index">Device ID</param>
		/// <param name="notify">Parameter for notification</param>
		/// <param name="batchNo">Batch number</param>
		/// <returns>Element</returns>
		public  List<int[]> GetData(int index, out uint notify, out int batchNo)
		{
			List<int[]> value = new List<int[]>();
			lock (_syncObject[index])
			{
				foreach (int[] data in _buffer[index])
				{
					value.Add(data);
				}
				_buffer[index].Clear();
				notify = _notify[index];
				_notify[index] = 0;
				batchNo = _batchNo[index];
			}
			return value;
		}

		/// <summary>
		/// Add the count
		/// </summary>
		/// <param name="index">Device ID</param>
		/// <param name="count">Count</param>
		/// <param name="notify">Parameter for notification</param>
		internal  void AddCount(int index, uint count, uint notify)
		{
			lock (_syncObject[index])
			{
				_count[index] += count;
				_notify[index] |= notify;
				// Add the batch number if the batch has been finalized.
				if ((notify & (0x1 << 16)) != 0) _batchNo[index]++;
			}
		}

		/// <summary>
		/// Get the count
		/// </summary>
		/// <param name="index">Device ID</param>
		/// <param name="notify">Parameter for notification</param>
		/// <param name="batchNo">Batch number</param>
		/// <returns></returns>
		internal  uint GetCount(int index, out uint notify, out int batchNo)
		{
			lock (_syncObject[index])
			{
				notify = _notify[index];
				_notify[index] = 0;
				batchNo = _batchNo[index];
				return _count[index];
			}
		}

		#endregion
	}
}
