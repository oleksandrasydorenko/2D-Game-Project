using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Scenemanagement
{
	public static class SceneService
	{
		public static Action<string> onActiveSceneUnloaded; 
		public static Action<string> onActiveSceneLoaded;

		public static Scene ActiveScene
		{
			get; private set;
		}

		public static bool LoadSceneByName(string name)
		{
			Scene newScene = null;

			foreach( Scene scene in RocketEngine.Settings.GameSettings.scenes)
			{
				if (scene.Name == name)
				{
					newScene = scene;
					break;
				}
			}

			if (newScene == null) return false;

			LoadScene(newScene);

			return true;
		
		}

		public static bool LoadSceneByIndex(uint index)
		{
			Scene newScene = null;

			newScene = RocketEngine.Settings.GameSettings.scenes[index];

			if (newScene == null) return false;

			LoadScene(newScene);

			return true;
		}

		public static void LoadScene(Scene sceneToLoad)
		{

			UnloadActiveScene();

			ActiveScene = sceneToLoad;

			sceneToLoad.CreateScene();

			onActiveSceneLoaded?.Invoke(ActiveScene.Name);
		}

		private static void UnloadActiveScene()
		{
			if(ActiveScene != null)
			{
				ActiveScene.UnloadScene();
				onActiveSceneUnloaded?.Invoke(ActiveScene.Name);
			}
			ActiveScene = null;
		}

		public static List<Instantiable> FindAllInstantiablesWithName(string name)
		{
			List<Instantiable> instantiables = new List<Instantiable>();

			foreach(Instantiable instantiable in ActiveScene.instantiables)
			{
				if(instantiable.Name == name) instantiables.Add(instantiable);
			}

			return instantiables;
		}

		public static Instantiable? FindFirstInstantiablesWithName(string name)
		{
			Instantiable instantiableFound = null;
			foreach (Instantiable instantiable in ActiveScene.instantiables)
			{
				if (instantiable.Name == name)
				{
					instantiableFound = instantiable;
					break;
				}
			}

			return instantiableFound;
		}

	}
}
