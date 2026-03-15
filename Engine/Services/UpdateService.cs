using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public static class UpdateService
	{
		public static Action<IUpdatable> onUpdatableCreated;
		public static Action<IUpdatable> onUpdatableDestroyed;

		public static void CreateUpdatable(IUpdatable updatable)
		{
			onUpdatableCreated?.Invoke(updatable);
		}

		public static void DestroyUpdatable(IUpdatable updatable)
		{
			onUpdatableDestroyed?.Invoke(updatable);
		}

	}
}
