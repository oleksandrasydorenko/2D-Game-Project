using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public static class Time
	{
		private static float timeScale = 1.0f;

		public static float TimeScale // timescale = 2 => faster, timescale = 0.5 => slower game
		{
			get { return timeScale; }

			set 
			{
				if (value <= 0) value = 0; 
				timeScale = value; 
			}
		}

		public static float UnscaledDeltaTime // time since last frame
		{
			get { return Raylib.GetFrameTime(); }
			private set { }
		}

		public static float DeltaTime // time since last frame but dependent on the time scale
		{
			get { return UnscaledDeltaTime * TimeScale; }
			private set { }
		}
	}
}
