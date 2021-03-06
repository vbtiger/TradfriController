﻿using System.Linq;

using TradfriLib.Data.Enums;

namespace TradfriLib.Data
{
	/// <summary>
	/// IKEA Tradfri lamp
	/// </summary>
	public class Lamp : TradfriDevice
	{
		private byte _brightness;

		/// <summary>
		/// State of the lamp (ON / OFF)
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
		/// Color of the lamp
		/// </summary>
		public Color Color { get; set; }


		internal Lamp(Json.TradfriLamp jsonLamp) : base(jsonLamp)
		{
			var lightingValues = jsonLamp.Value3311.First();

			this._brightness = lightingValues.Value5851;
			this.State = lightingValues.Value5850 == 0 ? LampState.Off : LampState.On;

			//if (Math.Abs(lightingValues.Value5709 - 33150) < 150 && Math.Abs(lightingValues.Value5710 - 27200) < 150)
			//	this.Color = Color.WarmYellor2200K;
			//else if (Math.Abs(lightingValues.Value5709 - 30150) < 150 && Math.Abs(lightingValues.Value5710 - 26900) < 150)
			//	this.Color = Color.WarmYellow2700K;
			//else if (Math.Abs(lightingValues.Value5709 - 24900) < 150 && Math.Abs(lightingValues.Value5710 - 24700) < 150)
			//	this.Color = Color.ColdWhite4000K;
			//else
			this.Color = new Color(lightingValues.Value5709, lightingValues.Value5710, lightingValues.Value5706);

			this.DeviceType = TradfriDeviceType.Lamp;
		}


		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Lamp device information string</returns>
		public override string ToString()
		{
			return $"{base.ToString()}, State={this.State}, Dim={this.Brightness}, {this.Color}";
		}
	}

}
