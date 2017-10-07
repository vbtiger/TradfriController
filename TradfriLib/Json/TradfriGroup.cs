using System.Collections.Generic;

namespace TradfriLib.Json
{
	/// <summary>
	/// JSON container for groups
	/// </summary>
	internal struct TradfriGroup
	{
		/// <summary>
		/// Id list container container type
		/// </summary>
		internal struct ValueType9018
		{
			/// <summary>
			/// Id list container
			/// </summary>
			public ValueType15002 Value15002 { get; set; }
		}

		/// <summary>
		/// Id list container type
		/// </summary>
		internal struct ValueType15002
		{
			/// <summary>
			/// Id list for devices in the group
			/// </summary>
			public List<int> Value9003 { get; set; }
		}

		/// <summary>
		/// Light state
		/// </summary>
		public int Value5850 { get; set; }

		/// <summary>
		/// Dimmer
		/// </summary>
		public byte Value5851 { get; set; }

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

		public int Value9039 { get; set; }


		/// <summary>
		/// Id list container container
		/// </summary>
		public ValueType9018 Value9018 { get; set; }
	}
}
