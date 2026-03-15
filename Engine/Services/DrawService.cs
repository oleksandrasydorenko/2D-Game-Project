using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	/// <summary>
	/// DrawService is used to tell the main programm what to Draw
	/// </summary>
	public static class DrawService
	{
		public static Action<IDrawable> onDrawableCreated;
		public static Action<IDrawable> onDrawableDestroyed;
		public static Action<IUiDrawable> onUiDrawableCreated;
		public static Action<IUiDrawable> onUiDrawableDestroyed;

		public static void CreateDrawable(IDrawable drawable)
		{
			onDrawableCreated?.Invoke(drawable);
		}

		public static void DestroyDrawable(IDrawable drawable)
		{
			onDrawableDestroyed?.Invoke(drawable);
		}

		public static void CreateUiDrawable(IUiDrawable drawable)
		{
			onUiDrawableCreated?.Invoke(drawable);
		}

		public static void DestroyUiDrawable(IUiDrawable drawable)
		{
			onUiDrawableDestroyed?.Invoke(drawable);
		}
	}
}
