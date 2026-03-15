using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Utils
{
	public class ColorUtils
	{
		public static Raylib_cs.Color Multiply(Raylib_cs.Color c1, Raylib_cs.Color c2)
		{
			return new Raylib_cs.Color(
				(byte)(c1.R * c2.R / 255),
				(byte)(c1.G * c2.G / 255),
				(byte)(c1.B * c2.B / 255),
				(byte)(c1.A * c2.A / 255)
			);

		}

		public static Raylib_cs.Color Divide(Raylib_cs.Color c1, Raylib_cs.Color c2)
		{
			return new Raylib_cs.Color(
				(byte)(c2.R == 0 ? c1.R : c1.R * 255/ c2.R),
				(byte)(c2.G == 0 ? c1.G : c1.G * 255 / c2.G),
				(byte)(c2.B == 0 ? c1.B : c1.B * 255 / c2.B),
				(byte)(c2.A == 0 ? c1.A : c1.A * 255 / c2.A)
			);

		}
	}
}
