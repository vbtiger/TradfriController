namespace TradfriLib.Data.Enums
{
	/// <summary>
	/// Available Power Sources
	/// <para> See: http://www.openmobilealliance.org/tech/profiles/LWM2M_Device-v1_0.xml </para>
	/// </summary>
	public enum PowerSourceType
	{
		/// <summary>
		/// DC power - 0
		/// </summary>
		DCPower = 0,

		/// <summary>
		/// Internal Battery - 1
		/// </summary>
		InternalBattery = 1,

		/// <summary>
		/// External Battery - 2
		/// </summary>
		ExternalBattery = 2,

		/// <summary>
		/// Battery, not in specification but used by remote controller
		/// </summary>
		Battery = 3,

		/// <summary>
		/// Power over Ethernet - 4
		/// </summary>
		PowerOverEthernet = 4,

		/// <summary>
		/// USB - 5
		/// </summary>
		USB = 5,

		/// <summary>
		/// AC (Mains) power - 6
		/// </summary>
		ACPower = 6,

		/// <summary>
		/// Solar - 7
		/// </summary>
		Solar = 7
	}
}
