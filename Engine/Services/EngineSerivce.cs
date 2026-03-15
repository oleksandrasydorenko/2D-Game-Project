using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public static class EngineSerivce
	{
		public static Action onGameClosing;

		public static bool isEditor = false;

		public static void CloseGame()
		{
			onGameClosing?.Invoke();
		}
	}
}
