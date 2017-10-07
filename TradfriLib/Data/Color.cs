using System;

namespace TradfriLib.Data
{
	/// <summary>
	/// A color for a lamp
	/// </summary>
	public class Color
	{
		#region FIELDS

		private readonly int _value5709;
		private readonly int _value5710;
		private readonly string _valueRGB;
		private readonly string _name;

		private int _temperature;

		#endregion

		#region PROPERTIES

		//public static Color ColdWhite4000K { get { return new Color(24930, 24694, "f5faf6", "ColdWhite_4000K"); } }
		//public static Color WarmYellow2700K { get { return new Color(30140, 26909, "f1e0b5", "WarmYellow_2700K"); } }
		//public static Color WarmYellow2200K { get { return new Color(33135, 27211, "efd275", "WarmYellor_2200K"); } }

		//public static Color Blue { get { return new Color(0, 0, "4a418a", "Blue"); } }
		//public static Color LightBlue { get { return new Color(0, 0, "6c83ba", "Light Blue"); } }
		//public static Color SaturatedPurple { get { return new Color(0, 0, "8f2686", "Saturated Purple"); } }
		//public static Color Lime { get { return new Color(0, 0, "a9d62b", "Lime"); } }
		//public static Color LightPurple { get { return new Color(0, 0, "c984bb", "Light Purple"); } }
		//public static Color Yellow { get { return new Color(0, 0, "d6e44b", "Yellow"); } }
		//public static Color SaturatedPink { get { return new Color(0, 0, "d9337c", "Saturated Pink"); } }
		//public static Color DarkPeach { get { return new Color(0, 0, "da5d41", "Dark Peach"); } }
		//public static Color SaturatedRed { get { return new Color(0, 0, "dc4b31", "Saturated Red"); } }
		//public static Color ColdSky { get { return new Color(0, 0, "dcf0f8", "Cold sky"); } }
		//public static Color Pink { get { return new Color(0, 0, "e491af", "Pink"); } }
		//public static Color Peach { get { return new Color(0, 0, "e57345", "Peach"); } }
		//public static Color WarmAmber { get { return new Color(0, 0, "e78834", "Warm Amber"); } }
		//public static Color LightPink { get { return new Color(0, 0, "e8bedd", "Light Pink"); } }
		//public static Color CoolDaylight { get { return new Color(0, 0, "eaf6fb", "Cool daylight"); } }
		//public static Color Candlelight { get { return new Color(0, 0, "ebb63e", "Candlelight"); } }
		//public static Color Sunrise { get { return new Color(0, 0, "f2eccf", "Sunrise"); } }

		//public static Color CoolWhite { get { return new Color(24930, 24694, "f5faf6", "Cool white"); } }
		//public static Color WarmWhite { get { return new Color(30140, 26909, "f1e0b5", "Warm white"); } }
		//public static Color WarmGlow { get { return new Color(33135, 27211, "efd275", "Warm glow"); } }


		/// <summary>
		/// X value according to the Planckian locus
		/// <para>See: https://en.wikipedia.org/wiki/Planckian_locus </para>
		/// </summary>
		public int Value5709
		{
			get { return this._value5709; }
		}

		/// <summary>
		/// Y value according to the Planckian locus
		/// <para>See: https://en.wikipedia.org/wiki/Planckian_locus </para>
		/// </summary>
		public int Value5710
		{
			get { return this._value5710; }
		}

		/// <summary>
		/// RGB value of the color
		/// </summary>
		public string ValueRGB
		{
			get { return this._valueRGB; }
		}

		/// <summary>
		/// Common name of the color
		/// </summary>
		public string Name
		{
			get { return this._name; }
		}

		/// <summary>
		/// Temperature value of the light (in Kelvin)
		/// </summary>
		public int Temperature
		{
			get { return _temperature; }
		}

		#endregion

		#region CONSTRUCTOR

		internal Color(int value5709, int value5710)
		{
			this._value5709 = value5709;
			this._value5710 = value5710;

			this._temperature = this.GetApproximateTemperature();
			this._name = this._temperature + "K";
		}

		private Color(int value5709, int value5710, string valueRgb, string name)
			: this(value5709, value5710)
		{
			this._valueRGB = valueRgb;
			this._name = name;
		}

		#endregion

		#region METHODS


		private static Color GetColorFromTemperature(int kelvin)
		{
			if (kelvin < 2200 || 4000 < kelvin)
				throw new ArgumentOutOfRangeException(nameof(kelvin));

			///////////////////////////////////////////////////////////////////
			// Source:                                                       //
			// https://en.wikipedia.org/wiki/Planckian_locus#Approximation   //
			// http://fcam.garage.maemo.org/apiDocs/_color_8cpp_source.htmls //
			///////////////////////////////////////////////////////////////////

			// chromaticity x coefficients for T <= 4000K
			double A_x00 = -0.2661239;
			double A_x01 = -0.2343580;
			double A_x02 = 0.8776956;
			double A_x03 = 0.179910;
			// chromaticity x coefficients for T > 4000K
			double A_x10 = -3.0258469;
			double A_x11 = 2.1070379;
			double A_x12 = 0.2226347;
			double A_x13 = 0.24039;
			// chromaticity y coefficients for T <= 2222K
			double A_y00 = -1.1063814;
			double A_y01 = -1.34811020;
			double A_y02 = 2.18555832;
			double A_y03 = -0.20219683;
			// chromaticity y coefficients for 2222K < T <= 4000K
			double A_y10 = -0.9549476;
			double A_y11 = -1.37418593;
			double A_y12 = 2.09137015;
			double A_y13 = -0.16748867;
			// chromaticity y coefficients for T > 4000K
			double A_y20 = 3.0817580;
			double A_y21 = -5.87338670;
			double A_y22 = 3.75112997;
			double A_y23 = -0.37001483;

			double invKiloK = 1000.0d / kelvin;
			double xc, yc;
			if (kelvin <= 4000)
			{
				xc = A_x00 * invKiloK * invKiloK * invKiloK +
					 A_x01 * invKiloK * invKiloK +
					 A_x02 * invKiloK +
					 A_x03;
			}
			else
			{
				xc = A_x10 * invKiloK * invKiloK * invKiloK +
					 A_x11 * invKiloK * invKiloK +
					 A_x12 * invKiloK +
					 A_x13;
			}

			if (kelvin <= 2222)
			{
				yc = A_y00 * xc * xc * xc +
					 A_y01 * xc * xc +
					 A_y02 * xc +
					 A_y03;
			}
			else if (kelvin <= 4000)
			{
				yc = A_y10 * xc * xc * xc +
					 A_y11 * xc * xc +
					 A_y12 * xc +
					 A_y13;
			}
			else
			{
				yc = A_y20 * xc * xc * xc +
					 A_y21 * xc * xc +
					 A_y22 * xc +
					 A_y23;
			}

			int x = (int)(xc * 65535 + 0.5);
			int y = (int)(yc * 65535 + 0.5);

			return new Color(x, y, "", kelvin + "K");
		}

		private int GetApproximateTemperature()
		{
			///////////////////////////////////////////////////////////////////
			// Source:                                                       //
			// https://en.wikipedia.org/wiki/Color_temperature#Approximation //
			///////////////////////////////////////////////////////////////////

			double x_e = 0.3366;
			double y_e = 0.1735;
			double A0 = -949.86315;
			double A1 = 6253.80338;
			double t1 = 0.92159;
			double A2 = 28.70599;
			double t2 = 0.20039;
			double A3 = 0.00004;
			double t3 = 0.07125;

			double n = (this.Value5709 / 65535d - x_e) / (this.Value5710 / 65535d - y_e);

			double kelvin = A0 + A1 * Math.Exp(-n / t1) + A2 * Math.Exp(-n / t2) + A3 * Math.Exp(-n / t3);
			//if (kelvin < 3000 || kelvin > 50000)
			//{
			//	throw new ArgumentOutOfRangeException($"Conversion only accurate within 3000 to 50000K, result was {kelvin} K.");
			//}
			return (int)Math.Floor(kelvin + 0.5f);
		}

		private void CalculateRgb(int brightness)
		{
			////////////////////////////////////////////////////////////////
			// Source:                                                    //
			// https://en.wikipedia.org/wiki/Color_temperature#Background //
			////////////////////////////////////////////////////////////////

			double[][] m =
			{
				new[] {3.1956, 2.4478, -0.1434},
				new[] {-2.5455, 7.0492, 0.9963},
				new[] {0.0, 0.0d, 1.0}
			};

			double x = this.Value5709;
			double y = this.Value5710;
			double z = brightness;

			double r = m[0][0] * x + m[0][1] * y + m[0][2] * z;
			double g = m[1][0] * x + m[1][1] * y + m[1][2] * z;
			double b = m[2][0] * x + m[2][1] * y + m[2][2] * z;

			// TODO - return or set the values
		}

		#endregion

		#region OVERRIDES for interfaces and base types

		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Color information string</returns>
		public override string ToString()
		{
			return this._name ?? $"{{{this.Value5709},{this._value5710}}}";
		}

		#endregion
	}

}
