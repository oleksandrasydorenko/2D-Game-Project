using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Utils
{
	public static class MathUtils
	{

		public static Vector2 RotateVector2InDeg(Vector2 vector,float deg)
		{
			float angleInRadiants = deg * (MathF.PI / 180);

			return RotateVector2InRad(vector, angleInRadiants);
		}

		public static Vector2 RotateVector2InRad(Vector2 vector,float rad)
		{
			return new Vector2(((MathF.Cos(rad) * vector.X) + ((-MathF.Sin(rad)) * vector.Y)), ((MathF.Sin(rad) * vector.X) + (MathF.Cos(rad) * vector.Y)));
		}

        public static float Lerp(float a, float b, float t)
        {
            if (t <= 0f) return a;
            if (t >= 1f) return b;
            return a + (b - a) * t;
        }

		public static Vector2 Lerp (Vector2 a, Vector2 b, float t)
		{
			return new Vector2(Lerp(a.X, b.X, t), Lerp(a.Y, b.Y, t));
		}

		public static float RandomFloatInRange(float min, float max)
		{
            Random rnd = new Random();

            return  min + (float)rnd.NextDouble() * (max - min);
        }

		public static float Noise(float x)
		{
			int i = (int)MathF.Floor(x);
			float f = x - i;

			// Hash
			int h = i * 374761393;
			h = (h << 13) ^ h;
			float rnd = (1f - ((h * (h * h * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824f);

			int h2 = (i + 1) * 374761393;
			h2 = (h2 << 13) ^ h2;
			float rnd2 = (1f - ((h2 * (h2 * h2 * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824f);

			// Smooth interpolation
			float t = f * f * (3f - 2f * f);

			return rnd * (1 - t) + rnd2 * t;
		}
	}
}
