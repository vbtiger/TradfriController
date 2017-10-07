using System.Collections.Generic;

namespace TradfriLib.Json
{
	internal class TradfriLamp : TradfriDevice
	{
		public struct ValueType3311
		{
			/// <summary>
			/// Color RGB
			/// </summary>
			public string Value5706 { get; set; }

			public int Value5707 { get; set; }

			public int Value5708 { get; set; }
			
			/// <summary>
			/// Color X
			/// </summary>
			public int Value5709 { get; set; }
			
			/// <summary>
			/// Color Y
			/// </summary>
			public int Value5710 { get; set; }

			public int Value5711 { get; set; }

			/// <summary>
			/// Light state
			/// </summary>
			public int Value5850 { get; set; }

			/// <summary>
			/// Dimmer
			/// </summary>
			public byte Value5851 { get; set; }

			public int Value9003 { get; set; }
		}

		/// <summary>
		/// Light properties (array with single item)
		/// </summary>
		public List<ValueType3311> Value3311 { get; set; }
	}
}
