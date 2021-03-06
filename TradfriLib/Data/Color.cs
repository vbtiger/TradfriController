﻿using System;
using System.Collections.Generic;

namespace TradfriLib.Data
{
	/// <summary>
	/// A color for a lamp
	/// </summary>
	public class Color
	{
		private readonly int _value5709;
		private readonly int _value5710;
		private readonly string _valueRGB;
		private readonly string _name;
		private readonly int _temperature;

		private static readonly IReadOnlyList<Color> availableColors = new List<Color>
																	   {
																		   Blue,
																		   LightBlue,
																		   SaturatedPurple,
																		   Lime,
																		   LightPurple,
																		   Yellow,
																		   SaturatedPink,
																		   DarkPeach,
																		   SaturatedRed,
																		   ColdSky,
																		   Pink,
																		   Peach,
																		   WarmAmber,
																		   LightPink,
																		   CoolDaylight,
																		   Candlelight,
																		   Sunrise,
																		   CoolWhite,
																		   WarmWhite,
																		   WarmGlow
																	   }.AsReadOnly();

		#region Built-In colors

		/// <summary>
		/// Returns all supported built-in colors
		/// </summary>
		public static IReadOnlyList<Color> AvailableColors => availableColors;

		/// <summary>
		/// Blue
		/// </summary>
		public static Color Blue => new Color(11469, 3277, "4A418A", "Blue");

		/// <summary>
		/// Light Blue
		/// </summary>
		public static Color LightBlue => new Color(13107, 6554, "6C83BA", "Light Blue");

		/// <summary>
		/// Saturated Purple
		/// </summary>
		public static Color SaturatedPurple => new Color(20316, 8520, "8F2686", "Saturated Purple");

		/// <summary>
		/// Lime
		/// </summary>
		public static Color Lime => new Color(26870, 33423, "A9D62B", "Lime");

		/// <summary>
		/// Light Purple
		/// </summary>
		public static Color LightPurple => new Color(22282, 12452, "C984BB", "Light Purple");

		/// <summary>
		/// Yellow
		/// </summary>
		public static Color Yellow => new Color(29491, 30802, "D6E44B", "Yellow");

		/// <summary>
		/// Saturated Pink
		/// </summary>
		public static Color SaturatedPink => new Color(32768, 15729, "D9337C", "Saturated Pink");

		/// <summary>
		/// Dark Peach
		/// </summary>
		public static Color DarkPeach => new Color(40632, 22282, "DA5D41", "Dark Peach");

		/// <summary>
		/// Saturated Red
		/// </summary>
		public static Color SaturatedRed => new Color(42926, 21299, "DC4B31", "Saturated Red");

		/// <summary>
		/// Cold Sky
		/// </summary>
		public static Color ColdSky => new Color(21109, 21738, "DCF0F8", "Cold sky");

		/// <summary>
		/// Pink
		/// </summary>
		public static Color Pink => new Color(32768, 18350, "E491AF", "Pink");

		/// <summary>
		/// Peach
		/// </summary>
		public static Color Peach => new Color(38011, 22938, "E57345", "Peach");

		/// <summary>
		/// Warm Amber
		/// </summary>
		public static Color WarmAmber => new Color(38011, 24904, "E78834", "Warm Amber");

		/// <summary>
		/// Light Pink
		/// </summary>
		public static Color LightPink => new Color(29491, 18350, "E8BEDD", "Light Pink");

		/// <summary>
		/// Cool Daylight
		/// </summary>
		public static Color CoolDaylight => new Color(22616, 23042, "EAF6FB", "Cool daylight");

		/// <summary>
		/// Candlelight
		/// </summary>
		public static Color Candlelight => new Color(35848, 26214, "EBB63E", "Candlelight");

		/// <summary>
		/// Sunrise
		/// </summary>
		public static Color Sunrise => new Color(28633, 26483, "F2ECCF", "Sunrise");


		/// <summary>
		/// Cool White
		/// </summary>
		public static Color CoolWhite => new Color(24930, 24694, "F5FAF6", "Cool white");

		/// <summary>
		/// Warm White
		/// </summary>
		public static Color WarmWhite => new Color(30140, 26909, "F1E0B5", "Warm white");

		/// <summary>
		/// Warm Glow
		/// </summary>
		public static Color WarmGlow => new Color(33135, 27211, "EFD275", "Warm glow");

		#endregion


		/// <summary>
		/// X value according to the Planckian locus
		/// <para>See: https://en.wikipedia.org/wiki/Planckian_locus </para>
		/// </summary>
		public int Value5709 => this._value5709;

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
		public string ValueRgb => this._valueRGB;

		/// <summary>
		/// Common name of the color
		/// </summary>
		public string Name => this._name;

		/// <summary>
		/// Temperature value of the light (in Kelvin)
		/// </summary>
		public int Temperature => this._temperature;

		internal Color(int value5709, int value5710, string valueRgb)
		{
			this._value5709 = value5709;
			this._value5710 = value5710;

			this._temperature = GetTemperatureWithReverseAlgo(value5709, value5710);
			this._valueRGB = valueRgb;
			this._name = this._temperature + "K";
		}

		private Color(int value5709, int value5710, string valueRgb, string name)
			: this(value5709, value5710, valueRgb)
		{
			this._name = name;
		}

		private static Tuple<int, int> ConvertTemperatureToXY(int temperature)
		{
			if (temperature < 2200 || 4000 < temperature)
			{
				throw new ArgumentOutOfRangeException(nameof(temperature));
			}

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

			double invKiloK = 1000.0d / temperature;
			double xc, yc;
			if (temperature <= 4000)
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

			if (temperature <= 2222)
			{
				yc = A_y00 * xc * xc * xc +
					 A_y01 * xc * xc +
					 A_y02 * xc +
					 A_y03;
			}
			else if (temperature <= 4000)
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

			return new Tuple<int, int>(x, y);
		}

		/// <summary>
		/// Creates a Color object from the given temperature value
		/// </summary>
		/// <param name="temperature">Temperature value in Kelvin</param>
		/// <returns>Color object</returns>
		public static Color GetColorFromTemperature(int temperature)
		{
			var xy = ConvertTemperatureToXY(temperature);

			string rgb = CalculateRgb(temperature).ToUpperInvariant();

			return new Color(xy.Item1, xy.Item2, rgb, temperature + "K");
		}

		// NOTE - this is inaccurate for the lower ranges
		//public static int GetApproximateTemperature(int x, int y)
		//{
		//	///////////////////////////////////////////////////////////////////
		//	// Source:                                                       //
		//	// https://en.wikipedia.org/wiki/Color_temperature#Approximation //
		//	///////////////////////////////////////////////////////////////////

		//	double x_e = 0.3366;
		//	double y_e = 0.1735;
		//	double A0 = -949.86315;
		//	double A1 = 6253.80338;
		//	double t1 = 0.92159;
		//	double A2 = 28.70599;
		//	double t2 = 0.20039;
		//	double A3 = 0.00004;
		//	double t3 = 0.07125;

		//	double n = (x / 65535d - x_e) / (y / 65535d - y_e);

		//	double kelvin = A0 + A1 * Math.Exp(-n / t1) + A2 * Math.Exp(-n / t2) + A3 * Math.Exp(-n / t3);

		//	if (kelvin < 3000 || kelvin > 50000)
		//	{
		//		throw new ArgumentOutOfRangeException($"Conversion only accurate within 3000 to 50000K, result was {kelvin} K.");
		//	}

		//	return (int)Math.Floor(kelvin + 0.5f);
		//}

		private static int GetTemperatureWithReverseAlgo(int x, int y)
		{
			double xError = double.MaxValue;
			double yError = double.MaxValue;

			int bestTemperature = 0;

			for (int temp = 2200; temp < 4000; temp++)
			{
				var xy = ConvertTemperatureToXY(temp);

				double xErrorCurrent = Math.Abs(x - xy.Item1);
				double yErrorCurrent = Math.Abs(y - xy.Item2);

				if (xErrorCurrent > xError || yErrorCurrent > yError)
				{
					continue;
				}

				if (xError <= 0 && yError <= 0)
				{
					return bestTemperature;
				}

				xError = xErrorCurrent;
				yError = yErrorCurrent;

				bestTemperature = temp;
			}

			return bestTemperature;
		}

		private static string CalculateRgb(int temperature)
		{
			///////////////////////////////////////////////
			// Source:                                   //
			// https://github.com/mattdesl/kelvin-to-rgb //
			///////////////////////////////////////////////

			temperature = temperature / 100;

			double r = temperature <= 66 ? 255 : 329.698727466 * Math.Pow(temperature - 60, -0.1332047592);

			double g = temperature <= 66 ? 99.4708025861 * Math.Log(temperature) - 161.1195681661 : 288.1221695283 * Math.Pow(temperature - 60, -0.0755148492);

			double b = temperature >= 66 ? 255 : (temperature <= 19 ? 0 : 138.5177312231 * Math.Log(temperature - 10) - 305.0447927307);

			r = r < 0 ? 0 : r > 255 ? 255 : r;
			g = g < 0 ? 0 : g > 255 ? 255 : g;
			b = b < 0 ? 0 : b > 255 ? 255 : b;

			return $"{(int)r:X2}{(int)g:X2}{(int)b:X2}";
		}

		/// <summary>
		/// Converts this object to a string representation containing relevant property contents
		/// </summary>
		/// <returns>Color information string</returns>
		public override string ToString()
		{
			return $"{this._name},\"{this.ValueRgb}\",{{{this.Value5709};{this._value5710}}}";
		}
	}
}
