
namespace TradfriLib.Json
{
	internal class TradfriDevice
	{
		/// <summary>
		/// Device info
		/// <para>See: http://www.openmobilealliance.org/tech/profiles/LWM2M_Device-v1_0.xml </para>
		/// </summary>
		public struct ValueType3
		{
			/// <summary>
			/// Manufacturer
			/// </summary>
			public string Value0 { get; set; }

			/// <summary>
			/// Model number
			/// </summary>
			public string Value1 { get; set; }

			/// <summary>
			/// Serial number
			/// </summary>
			public string Value2 { get; set; }

			/// <summary>
			/// Firmware version
			/// </summary>
			public string Value3 { get; set; }

			/// <summary>
			/// Power source type
			/// </summary>
			public int Value6 { get; set; }

			/// <summary>
			/// Battery level (if present)
			/// </summary>
			public int Value9 { get; set; }
		}

		/// <summary>
		/// Application Type
		/// </summary>
		public int Value5750 { get; set; }

		/// <summary>
		/// Name
		/// </summary>
		public string Value9001 { get; set; }

		/// <summary>
		/// Created at
		/// </summary>
		public int Value9002 { get; set; }

		/// <summary>
		/// ID
		/// </summary>
		public int Value9003 { get; set; }

		/// <summary>
		/// Reachable state
		/// </summary>
		public int Value9019 { get; set; }

		/// <summary>
		/// Last seen
		/// </summary>
		public int Value9020 { get; set; }

		public int Value9054 { get; set; }

		/// <summary>
		/// Device info
		/// </summary>
		public ValueType3 Value3 { get; set; }
	}

}
