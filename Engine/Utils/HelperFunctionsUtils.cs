using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Utils
{
	public static class HelperFunctionsUtils
	{
		// credit rosettacode.org
		/// <summary>
		/// Maps a value from a old range to a new range
		/// </summary>
		public static float ReMap(float value, float min, float max, float newMin = 0, float newMax = 1)
		{
			return newMin + (value - min) * (newMax-newMin) / (max-min);
		}

		public static bool ImplimentsInterface(object obj,string interfaceName)
		{
			return obj.GetType().GetInterface(interfaceName) != null;
		}

		/// <summary>
		/// returns the time difference in milliseonds in float
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static float DateTimeDifferenceInMilliseconds(DateTime start, DateTime end)
		{
			return (float)((end - start).TotalMilliseconds);
		}

		/// <summary>
		/// returns the time difference in seconds in float
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static float DateTimeDifferenceInSeconds(DateTime start, DateTime end)
		{
			return (float)((end - start).TotalSeconds);
		}

	}
}
