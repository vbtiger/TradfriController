using TradfriLib.Data.Enums;

namespace TradfriLib.Data
{
	/// <summary>
	/// Remote controller
	/// </summary>
	public class RemoteController : TradfriDevice
	{
		internal RemoteController(Json.TradfriRemoteController jsonController) : base(jsonController)
		{
			this.DeviceType = TradfriDeviceType.RemoteController;
		}

		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Remote control device information string</returns>
		public override string ToString()
		{
			return $"{this.DeviceType}, \"{this.Name}\", {this.DeviceInfo}";
		}
	}

}
