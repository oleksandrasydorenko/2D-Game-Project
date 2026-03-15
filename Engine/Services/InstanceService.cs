using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Numerics;

namespace RocketEngine
{
    /// <summary>
    /// Instance service is used to create and destroy game objects
    /// </summary>
    public static class InstanceService
	{
		public static Action<Instantiable> onInstantiableObjectCreated;
		public static Action<Instantiable> onInstantiableObjectDestroyed;

		// Copilot hat mir den code gegeben kp ob das passt, ich will wissen ob seit der base eine methode jemand überschriebne wurde in der ganzen vererbungs history
		static bool IsMethodOverriden(Type type, string methodName, Type baseType)
		{
			var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			return method != null && method.DeclaringType != baseType;
		}

		private static async Task Create<T>(T obj) where T : Instantiable
		{
			// if the update method is overriden we tell the programm to include this gameobject in the update loop but only after start is done 
			if (IsMethodOverriden(typeof(T), "Update", typeof(Instantiable)))
			{
				UpdateService.onUpdatableCreated?.Invoke(obj);
			}

			obj.Start();

		}

		public static T Instantiate<T>(T obj) where T : Instantiable
		{	
			obj.Construct();
			Create(obj); // we call start async because we dont want to block the main thread, the object calling the method doesnt care if the start method is done only if the obj is created
			onInstantiableObjectCreated?.Invoke(obj);
			return obj;
		}
		public static T InstantiateWithPosition<T>(T obj, Vector2 position) where T : ComponentBase
		{
			obj.SetPosition(position);
			obj.Construct();
			Create(obj); // we call start async because we dont want to block the main thread, the object calling the method doesnt care if the start method is done only if the obj is created
			onInstantiableObjectCreated?.Invoke(obj);
			return obj;
		}

		public static void Destroy<T>(T obj) where T : Instantiable
		{
			obj.Destroy();
			onInstantiableObjectDestroyed?.Invoke(obj);

			// Its faster to send the event and let the list check if the object is there then to check if update is overriden again
			UpdateService.onUpdatableDestroyed?.Invoke(obj);
		}



	}
}
