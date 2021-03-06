﻿using System;
using System.Text.RegularExpressions;

using TradfriLib.Data.Enums;
using TradfriLib.Json;

namespace TradfriLib.Data
{
	/// <summary>
	/// Main device
	/// </summary>
	public class TradfriDevice
	{
		/// <summary>
		/// Unique identifier
		/// </summary>
		public int Id { get; protected set; }

		/// <summary>
		/// Given name of the device (if using Tradfri mobile app)
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// Date and time when the device was created
		/// </summary>
		public DateTime CreatedAt { get; protected set; }

		/// <summary>
		/// Is the device reachable, meaning it is powered on/off (but could be turned on/off)
		/// </summary>
		public bool Reachable { get; protected set; }

		/// <summary>
		/// Date and time when the device was last seen powered on
		/// </summary>
		public DateTime LastSeen { get; protected set; }

		/// <summary>
		/// Type of the device
		/// </summary>
		public TradfriDeviceType DeviceType { get; protected set; }

		/// <summary>
		/// General device information
		/// </summary>
		public DeviceInformation DeviceInfo { get; protected set; }

		/// <summary>
		/// The raw JSON data sent by the LAN controller
		/// </summary>
		public string RawJson { get; protected set; }


		private TradfriDevice()
		{
			this.DeviceType = TradfriDeviceType.Unknown;
		}

		internal TradfriDevice(Json.TradfriDevice jsonDevice) : this()
		{
			this.Id = jsonDevice.Value9003;

			this.Name = jsonDevice.Value9001;

			this.DeviceInfo = new DeviceInformation(jsonDevice.Value3);

			this.CreatedAt = Utility.UnixTimestampToDateTime(jsonDevice.Value9002);

			this.Reachable = jsonDevice.Value9019 != 0;

			this.LastSeen = Utility.UnixTimestampToDateTime(jsonDevice.Value9020);
		}

		/// <summary>
		/// Parses the given JSON formatted string and returns a device object
		/// </summary>
		/// <param name="jsonValue">JSON formatted string (sent by the IKEA Tradfri LAN controller)</param>
		/// <returns>Parsed device object</returns>
		public static TradfriDevice Parse(string jsonValue)
		{
			string preparedJson = Regex.Replace(jsonValue, ",\"", ",\"Value");
			preparedJson = Regex.Replace(preparedJson, "{\"", "{\"Value");

			if (preparedJson.Contains("Value3311"))
			{
				// Lamp

				var lamp = Newtonsoft.Json.JsonConvert.DeserializeObject<TradfriLamp>(preparedJson);

				return new Lamp(lamp) { RawJson = jsonValue };
			}
			else if (preparedJson.Contains("Value15009"))
			{
				// Controller
				var controller = Newtonsoft.Json.JsonConvert.DeserializeObject<TradfriRemoteController>(preparedJson);

				return new RemoteController(controller) { RawJson = jsonValue };
			}
			else
			{
				// Unknown device
				var device = Newtonsoft.Json.JsonConvert.DeserializeObject<Json.TradfriDevice>(preparedJson);

				return new TradfriDevice(device) { RawJson = jsonValue };
			}
		}


		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Main device information string</returns>
		public override string ToString()
		{
			return $"{this.Id},{this.DeviceType}, \"{this.Name}\", Available={this.Reachable}, {this.DeviceInfo}";
		}
	}

}
