using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradfriLib.Data
{
	/// <summary>
	/// Remote controller
	/// </summary>
	public class RemoteController : TradfriDevice
	{
		#region FIELDS



		#endregion

		#region EVENTS



		#endregion

		#region PROPERTIES



		#endregion

		#region CONSTRUCTOR

		internal RemoteController(Json.TradfriRemoteController jsonController) : base(jsonController)
		{
			this.DeviceType = TradfriDeviceType.RemoteController;
		}

		#endregion

		#region METHODS



		#endregion

		#region OVERRIDES for interfaces and base types

		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Remote control device information string</returns>
		public override string ToString()
		{
			return $"{this.DeviceType}, \"{this.Name}\", {this.DeviceInfo}";
		}

		#endregion
	}

}
