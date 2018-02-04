using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TradfriLib.Data
{
	/// <summary>
	/// A group of devices
	/// </summary>
	public class TradfriGroup
	{
		#region FIELDS

		private byte _brightness;

		#endregion

		#region EVENTS



		#endregion

		#region PROPERTIES

		/// <summary>
		/// Unique identifier
		/// </summary>
		public int Id { get; protected set; }

		/// <summary>
		/// Given name of the group (if using Tradfri mobile app)
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// Date and time when the group was created
		/// </summary>
		public DateTime CreatedAt { get; protected set; }

		/// <summary>
		/// Global state of the lamps in the group (ON / OFF)
		/// </summary>
		public LampState State { get; set; }

		/// <summary>
		/// Brightness of the lamp (0-255)
		/// </summary>
		public byte Brightness
		{
			get { return this._brightness; }
			set
			{
				this._brightness = value;

				if (value == 0)
				{
					this.State = LampState.Off;
				}
				else if (this.State == LampState.Off)
				{
					this.State = LampState.On;
				}
			}
		}

		/// <summary>
		/// Devices that this groups contains
		/// </summary>
		public IReadOnlyList<TradfriDevice> Devices { get; protected set; }

		#endregion

		#region CONSTRUCTOR

		internal TradfriGroup(Json.TradfriGroup jsonGroup)
		{
			this.Id = jsonGroup.Value9003;

			this.Name = jsonGroup.Value9001;

			this._brightness = jsonGroup.Value5851;
			this.State = jsonGroup.Value5850 == 0 ? LampState.Off : LampState.On;

			this.CreatedAt = Utility.UnixTimestampToDateTime(jsonGroup.Value9002);

			var devices = new List<TradfriDevice>();

			foreach (int deviceId in jsonGroup.Value9018.Value15002.Value9003)
			{
				try
				{
					devices.Add(Controller.GetDevice(deviceId));
				}
				catch
				{
					// TODO - proper error handling
					// ignore
				}
			}

			this.Devices = devices;
		}

		#endregion

		#region METHODS

		/// <summary>
		/// Parses the given JSON formatted string and returns a group object
		/// </summary>
		/// <param name="jsonValue">JSON formatted string (sent by the IKEA Tradfri LAN controller)</param>
		/// <returns>Parsed device object</returns>
		public static TradfriGroup Parse(string jsonValue)
		{
			string preparedJson = Regex.Replace(jsonValue, ",\"", ",\"Value");
			preparedJson = Regex.Replace(preparedJson, "{\"", "{\"Value");

			var jsonGroup = Newtonsoft.Json.JsonConvert.DeserializeObject<Json.TradfriGroup>(preparedJson);

			return new TradfriGroup(jsonGroup);
		}

		#endregion

		#region OVERRIDES for interfaces and base types

		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Group information string</returns>
		public override string ToString()
		{
			string devicesString = string.Join($"\t\t{Environment.NewLine}", this.Devices.Select(d => d.ToString()));
			return $"{this.Id}, Group:\"{this.Name}\", State={this.State}, Brightness={this.Brightness},{Environment.NewLine}\tDevices:{Environment.NewLine}{devicesString}";
		}

		#endregion
	}

}
