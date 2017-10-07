
namespace TradfriLib.Data
{
	/// <summary>
	/// General information about the device
	/// </summary>
	public struct DeviceInformation
	{
		/// <summary>
		/// Manufacturer
		/// </summary>
		public string Manufacturer { get; private set; }

		/// <summary>
		/// Model number
		/// </summary>
		public string ModelNumber { get; private set; }

		/// <summary>
		/// Serial number
		/// </summary>
		public string SerialNumber { get; private set; }

		/// <summary>
		/// Firmware version
		/// </summary>
		public string FirmwareVersion { get; private set; }

		/// <summary>
		/// Power source type
		/// </summary>
		public PowerSourceType PowerSourceType { get; private set; }

		/// <summary>
		/// Battery level (if present)
		/// </summary>
		public int BatteryLevel { get; private set; }

		internal DeviceInformation(Json.TradfriDevice.ValueType3 jsonDeviceInfo)
		{
			this.Manufacturer = jsonDeviceInfo.Value0;
			this.ModelNumber = jsonDeviceInfo.Value1;
			this.SerialNumber = jsonDeviceInfo.Value2;
			this.FirmwareVersion = jsonDeviceInfo.Value3;
			this.PowerSourceType = (PowerSourceType)jsonDeviceInfo.Value6;
			this.BatteryLevel = jsonDeviceInfo.Value9;
		}

		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Device information string</returns>
		public override string ToString()
		{
			return "{DeviceInfo}";
		}
	}
}
