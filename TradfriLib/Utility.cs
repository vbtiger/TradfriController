using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradfriLib
{
	internal static class Utility
	{
		#region FIELDS



		#endregion

		#region EVENTS



		#endregion

		#region PROPERTIES



		#endregion

		#region CONSTRUCTOR



		#endregion

		#region METHODS



		/// <summary>
		/// https://stackoverflow.com/questions/249760/how-to-convert-a-unix-timestamp-to-datetime-and-vice-versa
		/// </summary>
		/// <param name="unixTimeStamp"></param>
		/// <returns></returns>
		public static DateTime UnixTimestampToDateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		#endregion

		#region OVERRIDES for interfaces and base types



		#endregion
	}

}
