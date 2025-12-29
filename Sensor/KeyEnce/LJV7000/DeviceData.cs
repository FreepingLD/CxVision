//----------------------------------------------------------------------------- 
// <copyright file="DeviceData.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

namespace LJV7_DllSampleAll.Datas
{
	#region Enum
	/// <summary>Device communication state</summary>
	public enum DeviceStatus
	{
		NoConnection = 0,
		Usb,
		UsbFast,
		Ethernet,
		EthernetFast,
	};
	#endregion

	/// <summary>
	/// Device data class
	/// </summary>
	public class DeviceData
	{
		#region Field
		/// <summary>Connection state</summary>
		private DeviceStatus _status = DeviceStatus.NoConnection;
		/// <summary>Ethernet settings</summary>
		private LJV7IF_ETHERNET_CONFIG _ethernetConfig;
		/// <summary>Profile data</summary>
		private List<ProfileData> _profileData;
		/// <summary>Measurement data</summary>
		private List<MeasureData> _measureData;
		#endregion

		#region Property
		/// <summary>
		/// Status property
		/// </summary>
		public DeviceStatus Status
		{ 
			get { return _status; }
			set
			{
				_profileData.Clear();
				_ethernetConfig = new LJV7IF_ETHERNET_CONFIG();
				_status = value; 
			} 
		}

		public LJV7IF_ETHERNET_CONFIG EthernetConfig{ get { return _ethernetConfig; } set { _ethernetConfig = value; } }
		public List<ProfileData> ProfileData { get { return _profileData; } set { _profileData = value; } }
		public List<MeasureData> MeasureData { get { return _measureData; } set { _measureData = value; } }
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public DeviceData()
		{
			_ethernetConfig = new LJV7IF_ETHERNET_CONFIG();
			_profileData = new List<ProfileData>();
			_measureData = new List<MeasureData>();
		}
		#endregion

		#region Method
		/// <summary>
		/// Connection status acquisition
		/// </summary>
		/// <returns>Connection status for display</returns>
		public string GetStatusString()
		{
			string status = _status.ToString();
			switch (_status)
			{			
			case DeviceStatus.Ethernet:
			case DeviceStatus.EthernetFast:
				status += string.Format("---{0}.{1}.{2}.{3}", _ethernetConfig.abyIpAddress[0], _ethernetConfig.abyIpAddress[1],
					_ethernetConfig.abyIpAddress[2], _ethernetConfig.abyIpAddress[3]);
				break;
			default:
				break;
			}
			return status;
		}
		#endregion
	}
}
